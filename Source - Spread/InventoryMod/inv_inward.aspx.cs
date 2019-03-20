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

public partial class inv_inward : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    bool check = false; bool check1 = false;
    int i;
    static string dept = "";
    static Hashtable pohashtable = new Hashtable();
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
            lblvalidation1.Visible = false;
            CalendarExtender1.EndDate = DateTime.Now;
            caltodate.EndDate = DateTime.Now;
            CalendarExtender2.EndDate = DateTime.Now;
            CalendarExtender3.EndDate = DateTime.Now;
            //CalendarExtender4.EndDate = DateTime.Now;
            //CalendarExtender5.EndDate = DateTime.Now;
            CalendarExtender6.EndDate = DateTime.Now;
            CalendarExtender7.EndDate = DateTime.Now;
            if (!IsPostBack)
            {
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txt_fd.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txt_td.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
                txt_BilldDate.Attributes.Add("readonly", "readonly");
                txt_fromdate1.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate1.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                //txt_BilldDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate1.Attributes.Add("readonly", "readonly");
                txt_todate1.Attributes.Add("readonly", "readonly");
                txt_date.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                txt_date.Attributes.Add("readonly", "readonly");
                ViewState["WardenCode"] = null;
                txtpop1totalcost.Attributes.Add("readonly", "readonly");

                vendorname();
                //item();
                vendor();
                ords();
                // itempop();
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.Visible = false;
                FpSpread2.Sheets[0].RowCount = 0;
                FpSpread2.Sheets[0].ColumnCount = 0;
                FpSpread2.Visible = false;
                FpSpread3.Sheets[0].RowCount = 0;
                FpSpread3.Sheets[0].ColumnCount = 0;
                FpSpread3.Visible = false;
                rdb_received.Checked = true;
                // btn_go_Click(sender, e);
                lbl_error.Visible = false;
                bindcollege();
                binddepartment();

                bindstore();
                bindhostelname();
                //rdb_store.Checked = true;
                storetrue();
                lbl_messname.Visible = false;
                ddl_Hostelname.Visible = false;

                lbl_fromdate.Visible = true;
                txt_fromdate.Visible = true;
                lbl_todate.Visible = true;
                txt_todate.Visible = true;
                rdb_yettorec.Visible = true;
                rdb_received.Visible = true;
                rdb_reject.Visible = true;
                lbl_ords.Visible = true;
                txt_ords.Visible = true;
                pords.Visible = true;
                //rdb_store_Click(sender, e);
                bind_popdept();
                bindgoodinwardcode();
                bind_poplib();
                MultiView1.ActiveViewIndex = 3;
            }
        }
        catch (Exception ex) { }
    }
    protected void bindhostelname()
    {

        //string Mess = " select MessName,MessMasterPK from HM_MessMaster ";
        ds.Clear();
        //ds = d2.select_method_wo_parameter(Mess, "Text");
        //ds.Clear();
        ds = d2.Bindmess_basedonrights(usercode, collegecode1);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_Hostelname.DataSource = ds;
            ddl_Hostelname.DataTextField = "MessName";
            ddl_Hostelname.DataValueField = "MessMasterPK";
            ddl_Hostelname.DataBind();

            cbl_dirmessbase.DataSource = ds;
            cbl_dirmessbase.DataTextField = "MessName";
            cbl_dirmessbase.DataValueField = "MessMasterPK";
            cbl_dirmessbase.DataBind();
            for (int i = 0; i < cbl_dirmessbase.Items.Count; i++)
            {
                cbl_dirmessbase.Items[i].Selected = true;
                txt_dirmessbase.Text = "Mess Name(" + (cbl_dirmessbase.Items.Count) + ")";
                cb_dirmessbase.Checked = true;
            }
        }
    }

    protected void bindstore()
    {
        string storepk = d2.GetFunction("select value from Master_Settings where settings='Store Rights' and usercode='" + usercode + "' and value<>''");
        //string store = "  select StoreName,StorePK from IM_StoreMaster ";
        ds.Clear();
        ds = d2.BindStorebaseonrights_inv(storepk);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_storename.DataSource = ds;
            ddl_storename.DataTextField = "StoreName";
            ddl_storename.DataValueField = "StorePK";
            ddl_storename.DataBind();


            cbl_dirstorebase.DataSource = ds;
            cbl_dirstorebase.DataTextField = "StoreName";
            cbl_dirstorebase.DataValueField = "StorePK";
            cbl_dirstorebase.DataBind();
            for (int i = 0; i < cbl_dirstorebase.Items.Count; i++)
            {
                cbl_dirstorebase.Items[i].Selected = true;
                txt_dirstorebase.Text = "Store Name(" + (cbl_dirstorebase.Items.Count) + ")";
                cb_dirstorebase.Checked = true;
            }
        }
    }
    protected void imgbtn_closepopclose_Click(object sender, EventArgs e)
    {
        Div1.Visible = false;
    }
    protected void imgbtn_closepopclose1_Click(object sender, EventArgs e)
    {
        popwindow3.Visible = false;
    }
    protected void imgbtn_closepopclose2_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;

    }

    protected void lnk_btnlogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }

    public void vendorname()
    {
        try
        {
            cbl_vendor.Items.Clear();
            string ven = "";

            if (!rdb_dirbook.Checked && !RadioButton_lib.Checked)
                ven = "select vendorcompname,VendorPK from CO_VendorMaster where VendorType=1 and isnull(libraryflag,'')='' order by VendorCompName";
            else
                ven = "select vendorcompname,VendorPK from CO_VendorMaster where VendorType=1 and libraryflag='1' order by VendorCompName";
            ds.Clear();
            ds = d2.select_method_wo_parameter(ven, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_vendor.DataSource = ds;
                cbl_vendor.DataTextField = "VendorCompName";
                cbl_vendor.DataValueField = "VendorPK";
                cbl_vendor.DataBind();

                if (cbl_vendor.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_vendor.Items.Count; i++)
                    {
                        cbl_vendor.Items[i].Selected = true;
                    }
                    txt_vendor.Text = "Vendor(" + cbl_vendor.Items.Count + ")";
                }
            }
            else
            {
                txt_vendor.Text = "--Select--";

            }
            ords();
        }
        catch
        { }
    }
    protected void cb_vendor_CheckedChange(object sender, EventArgs e)
    {
        int cout = 0;
        txt_vendor.Text = "---Select---";
        //vendorname();
        if (cb_vendor.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_vendor.Items.Count; i++)
            {
                cbl_vendor.Items[i].Selected = true;
            }
            txt_vendor.Text = "vendor(" + (cbl_vendor.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_vendor.Items.Count; i++)
            {
                cbl_vendor.Items[i].Selected = false;
            }
            txt_vendor.Text = "--Select--";
        }
        ords();
    }
    protected void Cbl_vendor_SelectedIndexChange(object sender, EventArgs e)
    {
        int i = 0;
        cb_vendor.Checked = false;

        int commcount = 0;
        txt_vendor.Text = "--Select--";
        for (i = 0; i < cbl_vendor.Items.Count; i++)
        {
            if (cbl_vendor.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_vendor.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_vendor.Items.Count)
            {
                cb_vendor.Checked = true;
            }
            txt_vendor.Text = "Vendor(" + commcount.ToString() + ")";
        }
        ords();
    }

    public void item()
    {

        string buildvalue = "";

        for (int i = 0; i < cbl_ords.Items.Count; i++)
        {
            if (cbl_ords.Items[i].Selected == true)
            {
                string build = cbl_ords.Items[i].Value.ToString();
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
        string vendorcode = "";
        for (int i = 0; i < cbl_vendor.Items.Count; i++)
        {
            if (cbl_vendor.Items[i].Selected == true)
            {
                string build = cbl_vendor.Items[i].Value.ToString();
                if (vendorcode == "")
                {
                    vendorcode = build;
                }
                else
                {
                    vendorcode = vendorcode + "'" + "," + "'" + build;
                }
            }
        }

        ds.Clear();
        string deptquery = "";
        if (cb_direct.Checked == false)
        {
            //deptquery = "select distinct im.ItemName,im.ItemPK from IT_PurchaseOrder po, CO_VendorMaster vm,IM_ItemDeptMaster i,IM_VendorItemDept vd,IM_ItemMaster im where i.ItemFK=vd.ItemFK and vm.VendorPK=vd.VenItemFK and im.ItemPK=vd.ItemFK  and  po.PurchaseOrderPK in('" + buildvalue + "')";

            if (!rdb_dirbook.Checked && !RadioButton_lib.Checked)
                deptquery = " select distinct i.ItemName,i.ItemPK from IT_PurchaseOrder p,IT_PurchaseOrderDetail pd,IM_VendorItemDept vd,IM_ItemMaster i where i.ItemPK=vd.ItemFK and p.PurchaseOrderPK=pd.PurchaseOrderFK and pd.ItemFK=i.ItemPK and pd.PurchaseOrderFK in ('" + buildvalue + "')  and pd.ItemFK=vd.ItemFK ";
            else
                deptquery = "select distinct bd.Title ItemName,bd.BookID ItemPK from IT_PurchaseOrder p,IT_PurchaseOrderDetail pd,bookdetails bd where   p.PurchaseOrderPK=pd.PurchaseOrderFK and pd.ItemFK=bd.bookID and pd.PurchaseOrderFK in('" + buildvalue + "') and pd.ItemFK=bd.BookID";
        }
        else if (cb_direct.Checked == true)
        {
            // deptquery = "select distinct im.ItemName,im.ItemPK from IT_PurchaseOrder po, CO_VendorMaster vm,IM_ItemDeptMaster i,IM_VendorItemDept vd,IM_ItemMaster im where i.ItemFK=vd.ItemFK and vm.VendorPK=vd.VenItemFK and im.ItemPK=vd.ItemFK and vm.vendorpk in ('" + vendorcode + "')";

            deptquery = "  select distinct im.ItemName,im.ItemPK from  CO_VendorMaster vm,IM_VendorItemDept vd,IM_ItemMaster im where vd.ItemFK=im.ItemPK and vd.VenItemFK=vm.VendorPK and vm.vendorpk in ('" + vendorcode + "')order by ItemName";
        }
        Cbl_item.Items.Clear();
        ds = d2.select_method_wo_parameter(deptquery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            Cbl_item.DataSource = ds;
            Cbl_item.DataTextField = "ItemName";
            Cbl_item.DataValueField = "ItemPK";
            Cbl_item.DataBind();

            if (Cbl_item.Items.Count > 0)
            {
                for (i = 0; i < Cbl_item.Items.Count; i++)
                {

                    Cbl_item.Items[i].Selected = true;
                }
                if (!rdb_dirbook.Checked)
                    txt_item.Text = "Items(" + Cbl_item.Items.Count + ")";
                else
                    txt_item.Text = "Books(" + Cbl_item.Items.Count + ")";
            }
        }
        else
        {
            txt_item.Text = "--Select--";
        }
    }

    protected void cb_item_CheckedChange(object sender, EventArgs e)
    {
        int cout = 0;
        txt_item.Text = "---Select---";
        if (cb_item.Checked == true)
        {
            cout++;
            for (int i = 0; i < Cbl_item.Items.Count; i++)
            {
                Cbl_item.Items[i].Selected = true;
            }
            txt_item.Text = "Items(" + (Cbl_item.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < Cbl_item.Items.Count; i++)
            {
                Cbl_item.Items[i].Selected = false;
            }
        }
    }
    protected void Cbl_item_SelectedIndexChange(object sender, EventArgs e)
    {
        int i = 0;
        cb_item.Checked = false;
        int commcount = 0;
        txt_item.Text = "--Select--";
        for (i = 0; i < Cbl_item.Items.Count; i++)
        {
            if (Cbl_item.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_item.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == Cbl_item.Items.Count)
            {
                cb_item.Checked = true;
            }
            txt_item.Text = "Items(" + commcount.ToString() + ")";
        }
    }

    protected void cb_direct_CheckedChange(object sender, EventArgs e)
    {
        if (cb_direct.Checked == true)
        {
            MultiView1.ActiveViewIndex = 0;
            lbl_ords.Visible = false;
            txt_ords.Visible = false;
            pords.Visible = false;
            dirinwardtypetrue();

            //txt_ords.Enabled = false;
            //lbl_fromdate.Enabled = false;
            //txt_fromdate.Enabled = false;
            // lbl_todate.Enabled = false;
            // txt_todate.Enabled = false;
            RadioButton_Inv.Checked = false;
            RadioButton_Inv.Visible = false;
            RadioButton_lib.Visible = false;
            RadioButton_lib.Checked = false;
            rdb_yettorec.Visible = false;
            rdb_received.Visible = false;
            rdb_reject.Visible = false;
            dirdiv.Visible = true;
            div4.Visible = false;
            lbl_itm.Text = "Items";
            mdiv.Attributes.Add("Style", "width:750px;");
            //cb_direct.Attributes.Add("Style", "margin-left:-360px;");
            //btn_go.Attributes.Add("Style", "margin-left:-210px;");
            //btn_add.Attributes.Add("Style", "margin-left:-170px;");
            //FpSpread1.Visible = false;
            //lblrptname.Visible = false;
            //txtexcelname.Visible = false;
            //btnExcel.Visible = false;
            //btnprintmaster.Visible = false;
            //lbl_orders.Visible = false;
            //txt_orders.Visible = false;
            rdb_dirstore.Checked = true;
            rdb_dirbook.Visible = false;
            rdb_dirbook.Checked = false;
            vendorname();
            vendor1();
            item();
        }
        else
        {
            MultiView1.ActiveViewIndex = 3;
            mdiv.Attributes.Add("Style", "width:935px;");
            dirinwardtypefalse();
            //cb_direct.Attributes.Add("Style", "margin-left:10px;");
            //btn_go.Attributes.Add("Style", "margin-left:10px;");
            //btn_add.Attributes.Add("Style", "margin-left:-170px;");
            lbl_ords.Visible = true;
            txt_ords.Visible = true;
            pords.Visible = true;
            vendorname();
            //txt_ords.Enabled = true;
            // lbl_fromdate.Enabled = true;
            // txt_fromdate.Enabled = true;
            // lbl_todate.Enabled = true;
            //txt_todate.Enabled = true;
            rdb_yettorec.Visible = true;
            rdb_received.Visible = true;
            rdb_reject.Visible = true;
            div4.Visible = true;
            dirdiv.Visible = false;
            //FpSpread1.Visible = false;
            //lblrptname.Visible = false;
            //txtexcelname.Visible = false;
            //btnExcel.Visible = false;
            //btnprintmaster.Visible = false;
            //lbl_orders.Visible = true;
            //txt_orders.Visible = true;
        }
    }

    protected void btn_add_Click(object sender, EventArgs e)
    {
        try
        {
            if (!rdb_dirbook.Checked)
                lbl_item1.Text = "Items";
            else
                lbl_item1.Text = "Books";


            if (!rdb_dirbook.Checked)
                vendor();
            else
                Libvendor();


            itempop();
            order();
            //vendorname();
            FpSpread2.Visible = false;
            divbtns.Visible = false;
            lbl_storename.Visible = false;
            ddl_storename.Visible = false;
            popwindow.Visible = true;
            txt_staff.Text = "";
            //btn_dir_Click(sender, e);
            Printcontrol.Visible = false;
            lbl_error1.Visible = false;
            //  rdb_hostel.Visible = false;
            // rdb_store.Visible = false;
            lbl_messname.Visible = false;
            ddl_Hostelname.Visible = false;

            lbl_billdno.Visible = false;
            txt_dbillno.Visible = false;
            lbl_billddate.Visible = false;
            txt_BilldDate.Visible = false;
            if (cb_direct.Checked == true)
            {
                dirdiv.Visible = true;
                div4.Visible = false;
                vendor1();
                itemdir();
                btn_reject1.Visible = false;
                FpSpread2.Visible = false;
                FpSpread4.Visible = false;

            }
            else
            {
                div4.Visible = true;
                dirdiv.Visible = false;
                btn_reject1.Visible = true;
                FpSpread2.Visible = false;
                FpSpread4.Visible = false;
            }
            deptnamefalse();
            //rdb_dept.Visible = false;
            //rdb_dept.Checked = false;
            lbl_orders.Visible = true;
            txt_orders.Visible = true;
            //rdb_store.Checked = true;
            //storetrue();
            //lbl_messname.Visible = false;
            //ddl_Hostelname.Visible = false;
            lbl_lib.Visible = false;
            ddl_Libname.Visible = false;
            lbl_upbillno.Visible = false;
            txt_upbillno.Visible = false;
            lbl_upbilldate.Visible = false;
            txt_upbilldate.Visible = false;
            lbl_upstaff.Visible = false;
            txt_upstaff.Visible = false;
            btnupQ1.Visible = false;
            Clear();
            ViewState["directinwardallow"] = null;
            ViewState["directinwardallowmess"] = null;
        }
        catch
        {
        }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["directinwardupgoodFk"] = null;
            ViewState["directinwarduppurchasepk"] = null;
            ViewState["directinwarditemfk"] = null;
            ViewState["directinwarddeptFK"] = null;

            Printcontrol.Visible = false;
            string items = "";
            for (int i = 0; i < Cbl_item.Items.Count; i++)
            {
                if (Cbl_item.Items[i].Selected == true)
                {
                    if (items == "")
                    {
                        items = "" + Cbl_item.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        items = items + "'" + "," + "'" + Cbl_item.Items[i].Value.ToString() + "";
                    }
                }
            }
            string chkvendor = "";
            for (int i = 0; i < cbl_vendor.Items.Count; i++)
            {
                if (cbl_vendor.Items[i].Selected == true)
                {
                    if (chkvendor == "")
                    {
                        chkvendor = "" + cbl_vendor.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        chkvendor = chkvendor + "'" + "," + "'" + cbl_vendor.Items[i].Value.ToString() + "";
                    }
                }
            }
            string firstdate = Convert.ToString(txt_fromdate.Text);
            string seconddate = Convert.ToString(txt_todate.Text);

            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            if (chkvendor.Trim() != "" && items.Trim() != "")
            {
                string storerights = ""; string messrights = "";
                messrights = returnwithsinglecodevalue(cbl_dirmessbase);
                storerights = returnwithsinglecodevalue(cbl_dirstorebase);

                string q = ""; string headername = "";
                if (cb_direct.Checked == false)
                {
                    #region  inward

                    if (rdb_received.Checked == true)
                    {
                        if (!rdb_dirbook.Checked && !RadioButton_lib.Checked)
                            q = "  select distinct convert(varchar(10), gi.InvoiceDate,103)as InvoiceDate , CONVERT (varchar(10), gi.GoodsInwardDate,103) as GoodsInwardDate,gi.GoodsInwardCode,gi.InwardQty,pi.Qty as OrderQty,gi.PurchaseOrderFK,im.ItemCode,im.ItemName, gi.InvoiceNo,pi.Sailing_prize,pi.rpu  from  IT_GoodsInward gi,IM_ItemMaster im,IT_PurchaseOrder p,IT_PurchaseOrderDetail pi where p.PurchaseOrderPK =pi.PurchaseOrderFK and p.PurchaseOrderPK =gi.PurchaseOrderFK and pi.ItemFK =gi.itemfk and gi.itemfk =im.ItemPK and   im.ItemPK in('" + items + "') and p.ApproveStatus ='1'  and pi.Inward_Status=1 and isnull(gi.PurchaseOrderFK,'0')<>0 and GoodsInwardDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ISNULL(p.OrderDescription,0)<>'Direct Inward' order by gi.GoodsInwardCode";//and InwardStatus='1' gi.OrderQty pi.Qty gi.GoodsInwardPK, 17.03.16
                        else
                            q = "select distinct convert(varchar(10), gi.InvoiceDate,103)as InvoiceDate , CONVERT (varchar(10), gi.GoodsInwardDate,103) as GoodsInwardDate,gi.GoodsInwardCode,gi.InwardQty,pi.Qty as OrderQty,gi.PurchaseOrderFK,im.Acc_No ItemCode,im.Title ItemName, gi.InvoiceNo,pi.Sailing_prize,pi.rpu  from  IT_GoodsInward gi,bookdetails im,IT_PurchaseOrder p,IT_PurchaseOrderDetail pi where p.PurchaseOrderPK =pi.PurchaseOrderFK and p.PurchaseOrderPK =gi.PurchaseOrderFK and pi.ItemFK =gi.itemfk and gi.itemfk =im.BookID and   im.BookID in('" + items + "') and p.ApproveStatus ='1'  and pi.Inward_Status=1 and isnull(gi.PurchaseOrderFK,'0')<>0 and GoodsInwardDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ISNULL(p.OrderDescription,0)<>'Direct Inward' order by gi.GoodsInwardCode";
                        FpSpread1.Width = 850;
                    }
                    if (rdb_reject.Checked == true)
                    {
                        if (!rdb_dirbook.Checked && !RadioButton_lib.Checked)
                            q = "  select distinct convert(varchar(10), g.InvoiceDate,103)as InvoiceDate , g.OrderQty, g.PurchaseOrderFK, g.GoodsInwardCode, i.ItemCode,i.ItemPK,i.ItemName,AppQty-ISNULL(RejQty,'0')as App_qty,g.InwardQty,CONVERT(varchar(10),GoodsInwardDate,103)as GoodsInwardDate,RejQty,vm.VendorCode,vm.VendorPK,p.OrderCode,g.InvoiceNo,pd.Sailing_prize,pd.rpu from IT_GoodsInward g,IM_ItemMaster i,IT_PurchaseOrderDetail pd,IT_PurchaseOrder p, CO_VendorMaster vm where i.ItemPK=pd.ItemFK and vm.VendorPK=p.VendorFK and p.PurchaseOrderPK=pd.PurchaseOrderFK and g.Itemfk =pd.ItemFK and pd.PurchaseOrderFK=g.PurchaseOrderFK and p.ApproveStatus='1' and pd.Inward_Status='2' and g.Itemfk=pd.ItemFK and ISNULL(pd.Inward_Status,'')<>1 and  i.ItemPK in ('" + items + "') and vm.VendorPK in ('" + chkvendor + "') and GoodsInwardDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and isnull(g.PurchaseOrderFK,'0')<>0  and ISNULL(p.OrderDescription,0)<>'Direct Inward'";
                        else
                            q = " select distinct convert(varchar(10), g.InvoiceDate,103)as InvoiceDate , g.OrderQty, g.PurchaseOrderFK, g.GoodsInwardCode, i.Acc_No ItemCode,i.BookId ItemPK,i.Title ItemName,AppQty-ISNULL(RejQty,'0')as App_qty,g.InwardQty,CONVERT(varchar(10),GoodsInwardDate,103)as GoodsInwardDate,RejQty,vm.VendorCode,vm.VendorPK,p.OrderCode,g.InvoiceNo,pd.Sailing_prize,pd.rpu from IT_GoodsInward g,bookdetails i,IT_PurchaseOrderDetail pd,IT_PurchaseOrder p, CO_VendorMaster vm where i.BookId=pd.ItemFK and vm.VendorPK=p.VendorFK and p.PurchaseOrderPK=pd.PurchaseOrderFK and g.Itemfk =pd.ItemFK and pd.PurchaseOrderFK=g.PurchaseOrderFK and p.ApproveStatus='1' and pd.Inward_Status='2' and g.Itemfk=pd.ItemFK and ISNULL(pd.Inward_Status,'')<>1 and  i.BookId in ('" + items + "') and vm.VendorPK in ('" + chkvendor + "') and GoodsInwardDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and isnull(g.PurchaseOrderFK,'0')<>0  and ISNULL(p.OrderDescription,0)<>'Direct Inward'";
                        FpSpread1.Width = 868;
                    }
                    if (rdb_yettorec.Checked == true)
                    {
                        if (!rdb_dirbook.Checked && !RadioButton_lib.Checked)
                            q = "select distinct convert(varchar(10), g.InvoiceDate,103)as InvoiceDate , i.ItemCode,i.ItemName,g.InwardQty,AppQty-ISNULL(RejQty,0)as App_qty,AppQty-ISNULL(InwardQty,0)as App_qty1,CONVERT(varchar(10),GoodsInwardDate,103)as GoodsInwardDate,g.GoodsInwardCode,p.OrderCode,vm.VendorCode, vm.VendorCompName, p.Reqstaff_appno ,g.PurchaseOrderFK,pd.Qty as OrderQty,g.InvoiceNo,pd.Sailing_prize,pd.rpu from IT_GoodsInward g,IM_ItemMaster i,IM_VendorItemDept vd,IT_PurchaseOrderDetail pd,IT_PurchaseOrder p, CO_VendorMaster vm where i.ItemPK=vd.ItemFK and vm.VendorPK=vd.VenItemFK and p.PurchaseOrderPK=pd.PurchaseOrderFK and g.Itemfk =pd.ItemFK and pd.PurchaseOrderFK=g.PurchaseOrderFK and p.VendorFK=vd.VenItemFK and p.ApproveStatus='1' and vm.VendorStatus='1' and AppQty-ISNULL(RejQty,0)<>0 and g.GoodsInwardDate  between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and vm.vendorpk in('" + chkvendor + "') and isnull(g.PurchaseOrderFK,'0')<>0 and pd.ItemFK=i.ItemPK and ISNULL(p.OrderDescription,0)<>'Direct Inward' order by i.ItemCode";//g.OrderQty 
                        else
                            q = " select distinct convert(varchar(10), g.InvoiceDate,103)as InvoiceDate , i.Acc_No ItemCode,i.Title ItemName,g.InwardQty,AppQty-ISNULL(RejQty,0)as App_qty,AppQty-ISNULL(InwardQty,0)as App_qty1,CONVERT(varchar(10),GoodsInwardDate,103)as GoodsInwardDate,g.GoodsInwardCode,p.OrderCode,vm.VendorCode, vm.VendorCompName, p.Reqstaff_appno ,g.PurchaseOrderFK,pd.Qty as OrderQty,g.InvoiceNo,pd.Sailing_prize,pd.rpu from IT_GoodsInward g,bookdetails i,IT_PurchaseOrderDetail pd,IT_PurchaseOrder p, CO_VendorMaster vm where   p.PurchaseOrderPK=pd.PurchaseOrderFK and g.Itemfk =pd.ItemFK and pd.PurchaseOrderFK=g.PurchaseOrderFK and p.VendorFK=vm.VendorPK and p.ApproveStatus='1' and vm.VendorStatus='1' and AppQty-ISNULL(RejQty,0)<>0 and g.GoodsInwardDate  between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and vm.vendorpk in('" + chkvendor + "') and isnull(g.PurchaseOrderFK,'0')<>0 and pd.ItemFK=i.BookID and ISNULL(p.OrderDescription,0)<>'Direct Inward' order by i.Acc_No";
                        FpSpread1.Width = 857;
                    }
                    #endregion
                }
                else
                {
                    #region Direct Inward

                    if (rdb_dirstore.Checked == true)
                    {
                        q = "  select distinct convert(varchar(10), gi.InvoiceDate,103)as InvoiceDate , gi.Itemfk ,gi.GoodsInwardPK,p.PurchaseOrderPK ,isnull(sd.TransferQty,0) as TransferQty, CONVERT (varchar(10), gi.GoodsInwardDate,103) as GoodsInwardDate,v.VendorCode,v.VendorCompName,i.ItemCode,i.ItemName,i.ItemUnit,sd.InwardRPU,gi.InwardQty,sd.StoreFK,gi.Received_staffcode,gi.InvoiceNo,pi.Sailing_prize from IT_GoodsInward gi, IT_StockDetail sd,IM_ItemMaster i,CO_VendorMaster v,IM_VendorItemDept vi,IT_PurchaseOrder p,IT_PurchaseOrderDetail pi where  sd.ItemFK =i.ItemPK and gi.itemfk =sd.ItemFK and v.VendorPK =vi.VenItemFK and vi.ItemFK =gi.itemfk and vi.ItemFK =sd.ItemFK and vi.ItemFK =i.ItemPK and  v.VendorPK in ('" + chkvendor + "') and i.ItemPK in ('" + items + "') and gi.VendorFK=vi.VenItemFK and gi.VendorFK=v.VendorPK and gi.GoodsInwardDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and gi.PurchaseOrderFK=p.PurchaseOrderPK  and p.VendorFK=gi.VendorFK and p.VendorFK=v.VendorPK and gi.VendorFK=p.VendorFK and pi.Inward_Status ='1'  and gi.Inward_Type='1' and pi.ItemFK=sd.ItemFK and  gi.PurchaseOrderFK=p.PurchaseOrderPK  and p.VendorFK=gi.VendorFK and p.VendorFK=v.VendorPK and gi.VendorFK=p.VendorFK and pi.Inward_Status ='1' and  gi.PurchaseOrderFK=pi.PurchaseOrderFK and sd.InwardFK=gi.GoodsInwardPK  and sd.StoreFK in('" + storerights + "') order by gi.InvoiceDate,v.VendorCode,i.ItemName";
                        headername = "Store Name";
                    }
                    else if (rdb_dirmess.Checked == true)
                    {
                        q = " select distinct convert(varchar(10), g.InvoiceDate,103)as InvoiceDate , g.Itemfk , g.GoodsInwardPK ,p.PurchaseOrderPK,''TransferQty,CONVERT (varchar(10), g.GoodsInwardDate,103) as GoodsInwardDate ,v.VendorCode,v.VendorCompName,i.ItemCode,i.ItemName,i.ItemUnit,pt.rpu as InwardRPU,g.InwardQty,DeptFK as name,g.Received_staffcode,g.DeptFK,g.InvoiceNo,pt.Sailing_prize from IT_GoodsInward g,IT_PurchaseOrder p ,IT_PurchaseOrderDetail pt,IM_ItemMaster i,CO_VendorMaster v,HM_MessMaster hm where g.PurchaseOrderFK =pt.PurchaseOrderFK and p.PurchaseOrderPK =g.PurchaseOrderFK and pt.PurchaseOrderFK =p.PurchaseOrderPK and pt.ItemFK =g.Itemfk and i.ItemPK =pt.ItemFK and i.ItemPK =g.Itemfk and g.Inward_Type ='1' and v.VendorPK=p.VendorFK and g.VendorFK=v.VendorPK and g.GoodsInwardDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and v.VendorPK in('" + chkvendor + "') and g.Itemfk in('" + items + "') and hm.MessMasterPK=g.deptfk  and g.DeptFK in('" + messrights + "')order by g.InvoiceDate,v.VendorCode,i.ItemName";
                        headername = "Mess Name";
                    }
                    else if (rdb_dirdept.Checked == true)
                    {
                        q = " select distinct convert(varchar(10), g.InvoiceDate,103)as InvoiceDate , g.Itemfk, g.GoodsInwardPK,p.PurchaseOrderPK ,''TransferQty,CONVERT (varchar(10), g.GoodsInwardDate,103) as GoodsInwardDate ,v.VendorCode,v.VendorCompName,i.ItemCode,i.ItemName,i.ItemUnit,pt.rpu as InwardRPU,g.InwardQty,DeptFK as name,g.Received_staffcode,g.DeptFK,g.InvoiceNo,pt.Sailing_prize from IT_GoodsInward g,IT_PurchaseOrder p ,IT_PurchaseOrderDetail pt,IM_ItemMaster i,CO_VendorMaster v,Department d  where g.PurchaseOrderFK =pt.PurchaseOrderFK and p.PurchaseOrderPK =g.PurchaseOrderFK and pt.PurchaseOrderFK =p.PurchaseOrderPK and pt.ItemFK =g.Itemfk and i.ItemPK =pt.ItemFK and i.ItemPK =g.Itemfk and g.Inward_Type ='1' and v.VendorPK=p.VendorFK and g.VendorFK=v.VendorPK and g.GoodsInwardDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and v.VendorPK in('" + chkvendor + "') and g.Itemfk in('" + items + "') and d.Dept_Code=g.deptfk order by  g.InvoiceDate,v.VendorCode,i.ItemName";
                        headername = "Department Name";
                    }
                    #endregion
                    FpSpread1.Width = 965;
                }
                if (q.Trim() != "")
                {
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string header = "";
                        if (cb_direct.Checked == false)//,pi.Sailing_prize,pi.RPU
                        {
                            if (!rdb_dirbook.Checked && !RadioButton_lib.Checked)
                                header = "S.No-50/GoodsInwardDate-200/GoodsInwardCode-200/Item Code-150/Item Name-150/Rpu-100/Sailing Prize-100/OrderQuantity-150/InwardQuantity-150/Bill No-100";
                            else
                                header = "S.No-50/GoodsInwardDate-200/GoodsInwardCode-200/Book AccNo-150/Book Name-150/Rpu-100/Sailing Prize-100/OrderCopy-150/InwardCopy-150/Bill No-100";
                            Fpreadheaderbindmethod(header, FpSpread1, "false");
                        }
                        else
                        {
                            //header = "S.No-50/GoodsInwardDate-100/Vendor Code-100/Vendor Name-150/Item Code-100/Item Name-150/Item Unit-200/Rpu-100/Sailing Prize-120/Inward Quantity-200/" + headername + "-150/Received Staff Name-200/Bill No-100";
                            header = "S.No/GoodsInwardDate/Vendor Code/Vendor Name/Item Code/Item Name/Item Unit/Rpu/Sailing Prize/Inward Quantity/" + headername + "/Received Staff Name/Bill No";
                            Fpreadheaderbindmethod(header, FpSpread1, "true");

                        }
                        if (cb_direct.Checked == false)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["GoodsInwardDate"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";


                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["GoodsInwardCode"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemName"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["PurchaseOrderFK"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Rpu"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["Sailing_prize"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";



                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["OrderQty"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["InwardQty"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                                string bill = Convert.ToString(ds.Tables[0].Rows[i]["InvoiceNo"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Right;
                                if (bill.Trim() == "")
                                {
                                    bill = "-";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = bill;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                            }
                        }
                        else
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["TransferQty"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(ds.Tables[0].Rows[i]["GoodsInwardPK"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["PurchaseOrderPK"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Note = Convert.ToString(ds.Tables[0].Rows[i]["Itemfk"]);

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["GoodsInwardDate"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";


                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCode"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemName"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemUnit"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["InwardRPU"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["Sailing_prize"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";



                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[i]["InwardQty"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                                string receiveddept = ""; string storemessfk = "";
                                if (rdb_dirmess.Checked == true)
                                {
                                    receiveddept = getmessname(Convert.ToString(ds.Tables[0].Rows[i]["DeptFK"]));
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Left;
                                    storemessfk = Convert.ToString(ds.Tables[0].Rows[i]["DeptFK"]);
                                }
                                else if (rdb_dirdept.Checked == true)
                                {
                                    receiveddept = getdeptname(Convert.ToString(ds.Tables[0].Rows[i]["DeptFK"]));
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Left;
                                    storemessfk = Convert.ToString(ds.Tables[0].Rows[i]["DeptFK"]);
                                }
                                else if (rdb_dirstore.Checked == true)
                                {
                                    receiveddept = getstorename(Convert.ToString(ds.Tables[0].Rows[i]["StoreFK"]));
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Left;
                                    storemessfk = Convert.ToString(ds.Tables[0].Rows[i]["StoreFK"]);
                                }
                                if (receiveddept.Trim() == "0" || receiveddept.Trim() == "")
                                {
                                    receiveddept = "-";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = receiveddept;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Tag = storemessfk;
                                //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 14].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                                //string receiveddept = "";

                                receiveddept = getstaffappid(Convert.ToString(ds.Tables[0].Rows[i]["Received_staffcode"]));
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Left;
                                if (receiveddept.Trim() == "0" || receiveddept.Trim() == "")
                                {
                                    receiveddept = "-";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Center;
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Text = receiveddept;

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Right;

                                string bill = Convert.ToString(ds.Tables[0].Rows[i]["InvoiceNo"]);
                                if (bill.Trim() == "")
                                {
                                    bill = "-";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Center;
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12].Text = bill;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12].Tag = Convert.ToString(ds.Tables[0].Rows[i]["InvoiceDate"]);

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12].Font.Name = "Book Antiqua";
                            }
                        }
                        for (int m = 0; m < FpSpread1.Sheets[0].ColumnCount; m++)
                        {
                            FpSpread1.Columns[m].Locked = true;
                        }

                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        FpSpread1.Visible = true;
                        lblrptname.Visible = true;
                        txtexcelname.Visible = true;
                        btnExcel.Visible = true;
                        btnprintmaster.Visible = true;
                        lbl_error.Visible = false;
                        rptprint.Visible = true;
                    }
                    else
                    {
                        lbl_error.Visible = true;
                        FpSpread1.Visible = false;
                        lbl_error.Text = "No Record Found";
                        //spreaddiv.Visible = false;
                        rptprint.Visible = false;
                    }
                }
                else
                {
                    lbl_error.Visible = true;
                    FpSpread1.Visible = false;
                    lbl_error.Text = "No Record Found";
                    //spreaddiv.Visible = false;
                    rptprint.Visible = false;
                }
            }
            else
            {
                lbl_error.Visible = true;
                FpSpread1.Visible = false;
                lbl_error.Text = "Please Select All Fields";
                //spreaddiv.Visible = false;
                rptprint.Visible = false;
            }
        }
        catch
        {
        }
    }
    protected string getstorename(string storecode)
    {
        string sname = "";
        try
        {
            sname = d2.GetFunction(" select StoreName from IM_StoreMaster where StorePK='" + storecode + "'");
        }
        catch { }
        return sname;
    }

    public string getstaffappid(string staffappid)
    {
        string app = "";
        try
        {
            app = d2.GetFunction("select distinct staff_name from staff_appl_master sam,staffmaster sm where  sam.appl_id='" + staffappid + "' and sam.appl_no = sm.appl_no");
        }
        catch { }
        return app;
    }
    protected string getdeptname(string staffcode)
    {
        string sname = "";
        try
        {
            sname = d2.GetFunction("select Dept_Name from Department where Dept_Code='" + staffcode + "'");
        }
        catch { }
        return sname;
    }
    protected string getmessname(string messcode)
    {
        string sname = "";
        try
        {
            sname = d2.GetFunction("select messname from HM_MessMaster where MessMasterPK='" + messcode + "'");
        }
        catch { }
        return sname;
    }
    protected void dirinwardtypefalse()
    {
        rdb_dirstore.Visible = true;
        rdb_dirmess.Visible = true;
        rdb_dirdept.Visible = true;
        rdb_dirbook.Visible = true;
    }
    protected void dirinwardtypetrue()
    {
        rdb_dirstore.Visible = true;
        rdb_dirmess.Visible = true;
        rdb_dirdept.Visible = true;
    }
    protected void btn_yettoreceived_Click(object sender, EventArgs e)
    {
        try
        {
            bool yetrecved = false;
            FpSpread1.SaveChanges();
            if (FpSpread1.Sheets[0].RowCount > 0)
            {
                for (int row = 1; row < FpSpread1.Sheets[0].RowCount; row++)
                {
                    int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, 1].Value);
                    if (checkval == 1)
                    {
                        string itemcode = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 2].Text);
                        string vendorcode = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 2].Tag);
                        string ordercode = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 6].Tag);
                        string goodscode = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 7].Tag);
                        string inwardamt = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 8].Text);

                        string q = "if exists(select*from goods_inward g,goodsinward_items gi,purchaseorder_items pi,purchase_order p,item_master i,Vendor_ItemDetails v where g.gi_code =gi.gi_code and g.gi_code='" + goodscode + "' and v.vendor_code='" + vendorcode + "' and i.item_code='" + itemcode + "' and p.order_code='" + ordercode + "') Update goodsinward_items set inward_qty=inward_qty+ISNULL('" + inwardamt + "',0) where item_code='" + itemcode + "' and gi_code='" + goodscode + "'";
                        int received = d2.update_method_wo_parameter(q, "Text");
                        if (received != 0)
                        {
                            yetrecved = true;
                        }
                        else
                        {

                        }
                    }
                }
            }
            if (yetrecved == true)
            {
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Received Sucessfully";
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Please Select Any One Item";
            }
        }
        catch
        {

        }
    }
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
    protected void FpSpread3_Render(object sender, EventArgs e)
    {
        try
        {
            if (check == true)
            {
                string activerow = "";
                string activecol = "";
                activerow = FpSpread3.ActiveSheetView.ActiveRow.ToString();
                activecol = FpSpread3.ActiveSheetView.ActiveColumn.ToString();
                collegecode = Session["collegecode"].ToString();
                Session["activerow"] = Convert.ToString(activerow);
                Session["activecoloumn"] = Convert.ToString(activecol);
                if (activecol.Trim() != "0" && activecol.Trim() != "1")
                {
                    bindhostel();
                    string totalvalue = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                    txt_totalQunatity.Text = Convert.ToString(totalvalue);
                    txt_transferdate.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                    string transfer = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
                    string hostel = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
                    string hostelcode = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
                    if (transfer.Trim() != "")
                    {
                        txt_transferqty.Text = Convert.ToString(transfer);
                    }
                    else
                    {
                        txt_transferqty.Text = "";
                    }
                    if (hostel.Trim() != "")
                    {
                        ddl_hostel3.SelectedItem.Text = Convert.ToString(hostel);
                        ddl_hostel3.SelectedItem.Value = Convert.ToString(hostelcode);
                        btn_newadd3.Text = "Update";
                    }
                    popwindow3.Visible = true;
                    btn_newadd3.Visible = true;
                    div3.Visible = true;
                }
            }
        }
        catch
        {

        }
    }
    protected void btn_exit3it_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }

    //popup1

    public void vendor()
    {
        ddl_vendor1.Items.Clear();
        ds.Clear();
        ds = d2.BindVendorNamevendorpk_inv();

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_vendor1.DataSource = ds;
            ddl_vendor1.DataTextField = "vendorcompname";
            ddl_vendor1.DataValueField = "VendorPK";
            ddl_vendor1.DataBind();

        }
    }

    public void vendor1()
    {

        ddl_vendor2.Items.Clear();
        ds.Clear();
        ds = d2.BindVendorNamevendorpk_inv();

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_vendor2.DataSource = ds;
            ddl_vendor2.DataTextField = "vendorcompname";
            ddl_vendor2.DataValueField = "VendorPK";
            ddl_vendor2.DataBind();
            //itemdir();
        }
    }



    protected void ddlvendorselect(object sender, EventArgs e)
    {
        order();
    }

    protected void ddl_vendor2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        itemdir();
    }

    public void order()
    {
        cbl_orders.Items.Clear();
        // string buildvalue = Convert.ToString(ddl_vendor1.SelectedItem.Value);
        string ordquery = "";
        string vendorfk = "";
        if (ddl_vendor1.Items.Count > 0)
            vendorfk = Convert.ToString(ddl_vendor1.SelectedItem.Value);

        ordquery = "select OrderCode,PurchaseOrderPK from IT_PurchaseOrder where VendorFK='" + vendorfk + "' and ApproveStatus ='1'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(ordquery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_orders.DataSource = ds;
            cbl_orders.DataTextField = "OrderCode";
            cbl_orders.DataValueField = "PurchaseOrderPK";
            cbl_orders.DataBind();
        }
        if (cbl_orders.Items.Count > 0)
        {
            for (int i = 0; i < cbl_orders.Items.Count; i++)
            {

                cbl_orders.Items[i].Selected = true;
            }

            txt_orders.Text = "Orders(" + cbl_orders.Items.Count + ")";
        }

        else
        {
            txt_orders.Text = "--Select--";
        }
        itempop();
    }

    public void ords()
    {
        cbl_ords.Items.Clear();
        string buildvalue = "";
        for (int i = 0; i < cbl_vendor.Items.Count; i++)
        {
            if (cbl_vendor.Items[i].Selected == true)
            {
                string build = cbl_vendor.Items[i].Value.ToString();
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

        string ordsquery = "";
        ordsquery = "select OrderCode,PurchaseOrderPK from IT_PurchaseOrder where VendorFK in('" + buildvalue + "') and ApproveStatus ='1'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(ordsquery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_ords.DataSource = ds;
            cbl_ords.DataTextField = "OrderCode";
            cbl_ords.DataValueField = "PurchaseOrderPK";
            cbl_ords.DataBind();
        }
        if (cbl_ords.Items.Count > 0)
        {
            for (int i = 0; i < cbl_ords.Items.Count; i++)
            {

                cbl_ords.Items[i].Selected = true;
            }

            txt_ords.Text = "Orders(" + cbl_ords.Items.Count + ")";
        }
        // }
        else
        {
            txt_ords.Text = "--Select--";
        }
        item();
    }

    public void itempop()
    {

        // string deptquery = "select distinct it.item_code,it.item_name,v.vendor_code from item_master it,vendor_details v where v.vendor_code='" + cbl_items1.SelectedItem.Value + "'";        
        string ord = "";
        for (int i = 0; i < cbl_orders.Items.Count; i++)
        {
            if (cbl_orders.Items[i].Selected == true)
            {
                if (ord == "")
                {
                    ord = "" + cbl_orders.Items[i].Value.ToString() + "";

                }
                else
                {
                    ord = ord + "'" + "," + "'" + cbl_orders.Items[i].Value.ToString() + "";
                }
            }
        }
        string buildvalue = "";
        if (ddl_vendor1.Items.Count > 0)
        {
            buildvalue = Convert.ToString(ddl_vendor1.SelectedItem.Value);
        }
        string query1 = "";

        //query1 = "select distinct im.ItemName,im.ItemPK from IT_PurchaseOrder po, CO_VendorMaster vm,IM_ItemDeptMaster i,IM_VendorItemDept vd,IM_ItemMaster im where i.ItemFK=vd.ItemFK and vm.VendorPK=vd.VenItemFK and im.ItemPK=vd.ItemFK  and  po.PurchaseOrderPK in('" + ord + "')";
        if (!rdb_dirbook.Checked)
            query1 = "  select distinct i.ItemName,i.ItemPK from IT_PurchaseOrder p,IT_PurchaseOrderDetail pd,IM_VendorItemDept vd,IM_ItemMaster i where i.ItemPK=vd.ItemFK and p.PurchaseOrderPK=pd.PurchaseOrderFK and pd.ItemFK=i.ItemPK and pd.PurchaseOrderFK in('" + ord + "') and pd.ItemFK=vd.ItemFK ";
        else
        {
            query1 = "select distinct bd.Title ItemName,bd.BookID ItemPK from IT_PurchaseOrder p,IT_PurchaseOrderDetail pd,bookdetails bd where   p.PurchaseOrderPK=pd.PurchaseOrderFK and pd.ItemFK=bd.bookID and pd.PurchaseOrderFK in('" + ord + "') and pd.ItemFK=bd.BookID";

        }
        cbl_items1.Items.Clear();
        ds.Clear();
        ds = d2.select_method_wo_parameter(query1, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_items1.DataSource = ds;
            cbl_items1.DataTextField = "ItemName";
            cbl_items1.DataValueField = "ItemPK";
            cbl_items1.DataBind();

            if (cbl_items1.Items.Count > 0)
            {
                for (int i = 0; i < cbl_items1.Items.Count; i++)
                {

                    cbl_items1.Items[i].Selected = true;
                }
                if (!rdb_dirbook.Checked)
                    txt_item1.Text = "Items(" + cbl_items1.Items.Count + ")";
                else
                    txt_item1.Text = "Books(" + cbl_items1.Items.Count + ")";
            }
        }
        else
        {
            txt_item1.Text = "--Select--";
        }
    }

    public void itemdir()
    {
        cbl_dir.Items.Clear();
        if (ddl_vendor2.Items.Count > 0)
        {
            string ven = Convert.ToString(ddl_vendor2.SelectedItem.Value);
            string query1 = "";

            query1 = "select distinct i.ItemName,i.ItemPK from CO_VendorMaster vm,IM_VendorItemDept vd,IM_ItemMaster i where i.ItemPK=vd.ItemFK  and i.ItemPK=vd.ItemFK  and  vd.VenItemFK in('" + ven + "') order by ItemName ";

            ds.Clear();
            ds = d2.select_method_wo_parameter(query1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_dir.DataSource = ds;
                cbl_dir.DataTextField = "ItemName";
                cbl_dir.DataValueField = "ItemPK";
                cbl_dir.DataBind();

                if (cbl_dir.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_dir.Items.Count; i++)
                    {

                        cbl_dir.Items[i].Selected = true;
                    }

                    txt_item2.Text = "Items(" + cbl_dir.Items.Count + ")";
                }
            }
            else
            {
                txt_item2.Text = "--Select--";
            }
        }
    }

    protected void cb_items1_CheckedChange(object sender, EventArgs e)
    {
        int cout = 0;
        txt_item1.Text = "---Select---";
        itempop();
        if (cb_items1.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_items1.Items.Count; i++)
            {
                cbl_items1.Items[i].Selected = true;
            }
            txt_item1.Text = "Items(" + (cbl_items1.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_items1.Items.Count; i++)
            {
                cbl_items1.Items[i].Selected = false;
            }
            txt_item1.Text = "--Select--";
        }
    }
    protected void cbl_items1_SelectedIndexChange(object sender, EventArgs e)
    {
        int commcount = 0;
        txt_item1.Text = "--Select--";
        cb_items1.Checked = false;
        for (i = 0; i < cbl_items1.Items.Count; i++)
        {
            if (cbl_items1.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txt_item1.Text = "Items(" + commcount.ToString() + ")";
            if (commcount == cbl_items1.Items.Count)
            {
                cb_items1.Checked = true;
            }
        }
    }

    protected void cb_dir_CheckedChange(object sender, EventArgs e)
    {
        int cout = 0;
        txt_item2.Text = "---Select---";
        itemdir();
        if (cb_dir.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_dir.Items.Count; i++)
            {
                cbl_dir.Items[i].Selected = true;
            }
            txt_item2.Text = "Items(" + (cbl_dir.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_dir.Items.Count; i++)
            {
                cbl_dir.Items[i].Selected = false;
            }
            txt_item2.Text = "--Select--";
        }
    }
    protected void cbl_dir_SelectedIndexChange(object sender, EventArgs e)
    {
        int commcount = 0;
        txt_item2.Text = "--Select--";
        cb_dir.Checked = false;
        for (i = 0; i < cbl_dir.Items.Count; i++)
        {
            if (cbl_dir.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txt_item2.Text = "Items(" + commcount.ToString() + ")";
            if (commcount == cbl_dir.Items.Count)
            {
                cb_dir.Checked = true;
            }
        }
    }

    protected void cbl_ords_SelectedIndexChange(object sender, EventArgs e)
    {
        int commcount = 0;
        txt_ords.Text = "--Select--";
        cb_ords.Checked = false;
        for (i = 0; i < cbl_ords.Items.Count; i++)
        {
            if (cbl_ords.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txt_ords.Text = "Orders(" + commcount.ToString() + ")";
            if (commcount == cbl_ords.Items.Count)
            {
                cb_ords.Checked = true;
            }
        }
        item();
    }

    protected void cb_ords_CheckedChange(object sender, EventArgs e)
    {
        int cout = 0;
        txt_ords.Text = "---Select---";
        //ords();
        if (cb_ords.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_ords.Items.Count; i++)
            {
                cbl_ords.Items[i].Selected = true;
            }
            txt_ords.Text = "Orders(" + (cbl_ords.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_ords.Items.Count; i++)
            {
                cbl_ords.Items[i].Selected = false;
            }
            txt_ords.Text = "--Select--";
        }

        item();
    }


    protected void cb_orders_CheckedChange(object sender, EventArgs e)
    {
        int cout = 0;
        txt_orders.Text = "---Select---";
        order();
        if (cb_orders.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_orders.Items.Count; i++)
            {
                cbl_orders.Items[i].Selected = true;
            }
            txt_orders.Text = "Orders(" + (cbl_orders.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_orders.Items.Count; i++)
            {
                cbl_orders.Items[i].Selected = false;
            }
            txt_orders.Text = "--Select--";
        }
        itempop();

    }

    protected void cbl_orders_SelectedIndexChange(object sender, EventArgs e)
    {
        int commcount = 0;
        txt_orders.Text = "--Select--";
        cb_orders.Checked = false;
        for (i = 0; i < cbl_orders.Items.Count; i++)
        {
            if (cbl_orders.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txt_orders.Text = "Orders(" + commcount.ToString() + ")";
            if (commcount == cbl_orders.Items.Count)
            {
                cb_orders.Checked = true;
            }
        }
        itempop();
    }
    protected void btn_dir_Click(object sender, EventArgs e)//delsis1
    {
        try
        {

            txt_date.Text = Convert.ToString(DateTime.Now.ToString("dd/MM/yyyy"));
            if (rdb_dirstore.Checked == true)
            {

                storetrue();
                hostelfalse();
                deptnamefalse();

            }
            if (rdb_dirmess.Checked == true)
            {
                hosteltrue();
                storefalse();
                deptnamefalse();

            }
            if (rdb_dirdept.Checked == true)
            {
                deptnametrue();
                hostelfalse();
                storefalse();
            }

            string popitems = "";
            for (int i = 0; i < cbl_dir.Items.Count; i++)
            {
                if (cbl_dir.Items[i].Selected == true)
                {
                    if (popitems == "")
                    {
                        popitems = "" + cbl_dir.Items[i].Value.ToString() + "";

                    }
                    else
                    {
                        popitems = popitems + "'" + "," + "'" + cbl_dir.Items[i].Value.ToString() + "";
                    }
                }
            }

            //string vendor = "";
            //for (int i = 0; i < cbl_vendor1.Items.Count; i++)
            //{
            //    if (cbl_vendor1.Items[i].Selected == true)
            //    {
            //        if (vendor == "")
            //        {
            //            vendor = "" + cbl_vendor1.Items[i].Value.ToString() + "";

            //        }
            //        else
            //        {
            //            vendor = vendor + "'" + "," + "'" + cbl_vendor1.Items[i].Value.ToString() + "";
            //        }
            //    }
            //}
            //string warden = Convert.ToString(ViewState["WardenCode"]);
            // string firstdate = Convert.ToString(txt_fd.Text);
            //string seconddate = Convert.ToString(txt_td.Text);
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            // string[] split = firstdate.Split('/');
            // dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            //split = seconddate.Split('/');
            //dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            //if (vendor.Trim() != "" && popitems.Trim() != "")

            //if (popitems.Trim() != "" && ven.Trim() != "")
            //{
            string selectquery = "";
            string ven = Convert.ToString(ddl_vendor2.SelectedItem.Value);
            selectquery = "select distinct vm.VendorCode,vm.VendorCompName,i.ItemCode,i.ItemName,i.ItemUnit,i.ItemPK from CO_VendorMaster vm,IM_VendorItemDept vd,IM_ItemMaster i where i.ItemPK=vd.ItemFK  and i.ItemPK=vd.ItemFK  and vm.VendorPK=vd.VenItemFK and  vd.VenItemFK in('" + ven + "') and i.ItemPK in('" + popitems + "') order by VendorCode,ItemName";

            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                divbtns.Visible = true;
                FpSpread4.Visible = true;
                FpSpread4.Sheets[0].RowCount = 0;
                FpSpread4.Sheets[0].ColumnCount = 0;
                FpSpread4.CommandBar.Visible = false;
                FpSpread4.Sheets[0].AutoPostBack = true;
                FpSpread4.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread4.Sheets[0].RowHeader.Visible = false;
                FpSpread4.Sheets[0].ColumnCount = 19;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                FpSpread4.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                //FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].Columns[0].Width = 75;
                FpSpread4.Sheets[0].Columns[0].Locked = true;

                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].Text = "OrderCode";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].Columns[1].Width = 90;
                FpSpread4.Sheets[0].Columns[1].Visible = false;

                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].Text = "VendorCode";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].Columns[2].Width = 100;
                FpSpread4.Sheets[0].Columns[2].Locked = true;

                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].Text = "VendorCompName";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].Columns[3].Width = 180;
                FpSpread4.Sheets[0].Columns[3].Locked = true;

                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Item Code";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].Columns[4].Width = 110;
                FpSpread4.Sheets[0].Columns[4].Locked = true;

                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Item Name";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].Columns[5].Width = 100;
                FpSpread4.Sheets[0].Columns[5].Locked = true;

                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Item Unit";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].Columns[6].Width = 100;
                FpSpread4.Sheets[0].Columns[6].Locked = true;

                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 7].Text = "RPU";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].Columns[7].Width = 100;
                FpSpread4.Sheets[0].Columns[7].Locked = true;

                //24.06.16
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Sailing Prize";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].Columns[8].Width = 100;
                FpSpread4.Sheets[0].Columns[8].Locked = true;

                //if (cb_direct.Checked != true)
                //{
                //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 8].Text = "AppQty";
                //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                //    FpSpread4.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                //    FpSpread4.Sheets[0].Columns[8].Width = 100;
                //    FpSpread4.Sheets[0].Columns[8].Locked = true;
                //}
                //else
                //{
                //    FpSpread4.Sheets[0].Columns[8].Visible = false;

                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Discount";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Right;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].Columns[10].Width = 100;
                FpSpread4.Sheets[0].Columns[10].Locked = true;

                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Tax(%)";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Right;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].Columns[11].Width = 100;
                FpSpread4.Sheets[0].Columns[11].Locked = true;

                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Exercise tax(%)";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 12].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 12].HorizontalAlign = HorizontalAlign.Right;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 12].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 12].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].Columns[12].Width = 100;
                FpSpread4.Sheets[0].Columns[12].Locked = true;

                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Education Cess";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 13].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 13].HorizontalAlign = HorizontalAlign.Right;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 13].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 13].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].Columns[13].Width = 100;
                FpSpread4.Sheets[0].Columns[13].Locked = true;

                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 14].Text = "Higher Education Cess";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 14].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 14].HorizontalAlign = HorizontalAlign.Right;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 14].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 14].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].Columns[14].Width = 100;
                FpSpread4.Sheets[0].Columns[14].Locked = true;

                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 15].Text = "Other Charges";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 15].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 15].HorizontalAlign = HorizontalAlign.Right;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 15].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 15].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].Columns[15].Width = 100;
                FpSpread4.Sheets[0].Columns[15].Locked = true;

                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 16].Text = "Description";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 16].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 16].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 16].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 16].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].Columns[16].Width = 100;
                FpSpread4.Sheets[0].Columns[16].Locked = true;

                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 17].Text = "Total";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 17].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 17].HorizontalAlign = HorizontalAlign.Right;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 17].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 17].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].Columns[17].Width = 100;
                FpSpread4.Sheets[0].Columns[17].Locked = true;

                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 18].Text = "Bill No";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 18].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 18].HorizontalAlign = HorizontalAlign.Right;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 18].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 18].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].Columns[18].Width = 100;
                FpSpread4.Sheets[0].Columns[18].Locked = true;
                FpSpread4.Sheets[0].Columns[18].Visible = false;

                //}
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 9].Text = "InwardQty";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].Columns[9].Width = 100;
                FpSpread4.Sheets[0].Columns[9].Locked = true;

                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Locked = true;                 
                FarPoint.Web.Spread.CheckBoxCellType chkbox = new FarPoint.Web.Spread.CheckBoxCellType();
                chkbox.AutoPostBack = false;
                FarPoint.Web.Spread.DoubleCellType db = new FarPoint.Web.Spread.DoubleCellType();
                db.ErrorMessage = "Only Allowed For Numbers";
                FpSpread4.Columns[8].CellType = db;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    FpSpread4.Sheets[0].RowCount++;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCode"]);
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemName"]);
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemUnit"]);
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(ds.Tables[0].Rows[i]["ItemPK"]);
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 7].Text = "";
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                    //if (cb_direct.Checked != true)
                    //{
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 8].Text = "";// Convert.ToString(ds.Tables[0].Rows[i]["app_qty"]);
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                    //}

                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 9].Text = "";
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                }
                dircectinward.Visible = true;
                FpSpread4.Sheets[0].PageSize = FpSpread4.Sheets[0].RowCount;
                FpSpread4.Visible = true;
                lbl_error1.Visible = false;
                //rdb_store.Visible = true;
                //rdb_hostel.Visible = true;
                //rdb_dept.Visible = true;

                lbl_billdno.Visible = true;
                txt_dbillno.Visible = true;
                lbl_billddate.Visible = true;
                txt_BilldDate.Visible = true;


                //hostelfalse();
                //storetrue();
                //deptnamefalse();
                //rdb_hostel.Checked = false;
                //rdb_dept.Checked = false;
                //rdb_store.Checked = true;
                //rdb_store_Click(sender, e);
            }
            else
            {
                lbl_error1.Visible = true;
                FpSpread4.Visible = false;
                hostelfalse();
                storefalse(); deptnamefalse();
                //  rdb_store.Visible = false;
                // rdb_hostel.Visible = false;
                //rdb_dept.Visible = false;
                lbl_billdno.Visible = false;
                txt_dbillno.Visible = false;
                lbl_billddate.Visible = false;
                txt_BilldDate.Visible = false;
                lbl_error1.Text = "No Record Found";
            }
            //}
            //else
            //{
            //    lbl_error1.Visible = true;
            //    FpSpread2.Visible = false;
            //    lbl_error1.Text = "Please select anyone staff name";
            //}
            // }
            //if (popitems.Trim() == "")
            ////{
            //    lbl_error1.Visible = true;
            //    FpSpread2.Visible = false;
            //    lbl_error1.Text = "Please update vendor items";
            //}

        }
        catch
        {
        }
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        directinwardpop.Visible = false;
    }
    protected void btn_go1_Click(object sender, EventArgs e)
    {
        try
        {
            if (rdb_dirstore.Checked == true)
            {

                storetrue();
                hostelfalse();
                deptnamefalse();
                Libraryfalse();
            }
            if (rdb_dirmess.Checked == true)
            {
                hosteltrue();
                storefalse();
                deptnamefalse();
                Libraryfalse();
            }
            if (rdb_dirdept.Checked == true)
            {
                deptnametrue();
                hostelfalse();
                storefalse();
                Libraryfalse();
            }
            if (rdb_dirdept.Checked == true)
            {
                deptnametrue();
                hostelfalse();
                storefalse();
                Libraryfalse();
            }
            if (rdb_dirbook.Checked == true)
            {
                deptnamefalse();
                hostelfalse();
                storefalse();
                Librarytrue();
            }
            string popitems = "";
            for (int i = 0; i < cbl_items1.Items.Count; i++)
            {
                if (cbl_items1.Items[i].Selected == true)
                {
                    if (popitems == "")
                    {
                        popitems = "" + cbl_items1.Items[i].Value.ToString() + "";

                    }
                    else
                    {
                        popitems = popitems + "'" + "," + "'" + cbl_items1.Items[i].Value.ToString() + "";
                    }
                }
            }
            string buildvalue = Convert.ToString(ddl_vendor1.SelectedItem.Value);
            string ordercode = "";
            for (int i = 0; i < cbl_orders.Items.Count; i++)
            {
                if (cbl_orders.Items[i].Selected == true)
                {
                    if (ordercode == "")
                    {
                        ordercode = "" + cbl_orders.Items[i].Value.ToString() + "";

                    }
                    else
                    {
                        ordercode = ordercode + "'" + "," + "'" + cbl_orders.Items[i].Value.ToString() + "";
                    }
                }
            }
            //string warden = Convert.ToString(ViewState["WardenCode"]);
            string firstdate = Convert.ToString(txt_fromdate1.Text);
            string seconddate = Convert.ToString(txt_todate1.Text);
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            //if (vendor.Trim() != "" && popitems.Trim() != "")
            if (popitems.Trim() != "" && buildvalue.Trim() != "")
            {
                string selectquery = "";
                //selectquery = "select distinct vm.VendorCode,vm.VendorCompName,vm.VendorPK,i.ItemName,i.ItemCode,i.itempk,i.ItemUnit,p.OrderCode,p.PurchaseOrderPK,pd.RPU,(AppQty-ISNULL(RejQty ,0)) as app_qty,pd.rejQty from IM_ItemMaster i,IM_VendorItemDept vd,CO_VendorMaster vm,IT_PurchaseOrder p,IT_PurchaseOrderDetail pd where i.ItemPK=vd.ItemFK and vd.ItemFK =pd.ItemFK and vm.VendorPK=p.VendorFK and vm.VendorPK=vd.VenItemFK and p.PurchaseOrderPK=pd.PurchaseOrderFK and p.VendorFK=vd.VenItemFK and pd.ItemFK=vd.ItemFK and vm.VendorPK in('" + buildvalue + "') and i.ItemPK in('" + popitems + "') and p.OrderDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ISNULL(InwardStatus,'')<>1  order by i.ItemName ";
                //and g.PurchaseOrderFK=p.PurchaseOrderPK and g.orderqty-ISNULL(g.inwardqty,'0')<>0 order by i.ItemName ";

                if (!rdb_dirbook.Checked)
                    selectquery = " select distinct  vm.VendorCode,vm.VendorCompName,vm.VendorPK,i.ItemName,i.ItemCode,i.itempk,i.ItemUnit,p.OrderCode,p.PurchaseOrderPK,pd.RPU,(AppQty-ISNULL(RejQty ,0)) as app_qty,pd.rejQty,pd.sailing_prize from IM_ItemMaster i,IM_VendorItemDept vd,CO_VendorMaster vm,IT_PurchaseOrder p,IT_PurchaseOrderDetail pd where i.ItemPK=vd.ItemFK and vd.ItemFK =pd.ItemFK and vm.VendorPK=p.VendorFK and p.PurchaseOrderPK=pd.PurchaseOrderFK  and pd.ItemFK=vd.ItemFK and vm.VendorPK in('" + buildvalue + "') and i.ItemPK in('" + popitems + "') and p.OrderDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and isnull(pd.Inward_Status,'0')<>1 and p.PurchaseOrderPK in('" + ordercode + "') and Isnull(vm.LibraryFlag,'')=''";// ,IT_GoodsInward g  and p.PurchaseOrderPK not in (g.PurchaseOrderFK) 
                else
                    selectquery = "select distinct  vm.VendorCode,vm.VendorCompName,vm.VendorPK, bd.Title ItemName,bd.BookID ItemCode,p.OrderCode,p.PurchaseOrderPK,pd.RPU,(AppQty-ISNULL(RejQty ,0)) as app_qty,pd.rejQty,pd.sailing_prize,bd.Lib_Code,isnull(bd.book_status,'') book_status,bd.Author from bookdetails bd,CO_VendorMaster vm,IT_PurchaseOrder p,IT_PurchaseOrderDetail pd where  bd.BookID =pd.ItemFK and vm.VendorPK=p.VendorFK and p.PurchaseOrderPK=pd.PurchaseOrderFK  and vm.LibraryFlag='1' and vm.VendorPK in('" + buildvalue + "') and  bd.BookID in('" + popitems + "') and p.OrderDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and isnull(pd.Inward_Status,'0')<>1 and p.PurchaseOrderPK in('" + ordercode + "')";

                // vm.VendorPK=vd.VenItemFK and and p.VendorFK=vd.VenItemFK



                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    divbtns.Visible = true;
                    FpSpread2.Visible = true;
                    FpSpread2.Sheets[0].RowCount = 0;
                    FpSpread2.Sheets[0].ColumnCount = 0;
                    FpSpread2.CommandBar.Visible = false;
                    FpSpread2.Sheets[0].AutoPostBack = false;
                    FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread2.Sheets[0].RowHeader.Visible = false;
                    FpSpread2.Sheets[0].ColumnCount = 12;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    //FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Columns[0].Width = 50;
                    FpSpread2.Sheets[0].Columns[0].Locked = true;

                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "OrderCode";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Columns[1].Width = 100;

                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "VendorCode";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Columns[2].Width = 100;
                    FpSpread2.Sheets[0].Columns[2].Locked = true;

                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "VendorCompName";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Columns[3].Width = 145;
                    FpSpread2.Sheets[0].Columns[3].Locked = true;


                    if (!rdb_dirbook.Checked)
                    {
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Item Name";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "ItemCode";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "ItemUnit";
                        FpSpread2.Sheets[0].Columns[5].Visible = true;
                        FpSpread2.Sheets[0].Columns[6].Visible = true;

                    }
                    else
                    {
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Book Name";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Bookid";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "ItemUnit";
                        FpSpread2.Sheets[0].Columns[5].Visible = false;
                        FpSpread2.Sheets[0].Columns[6].Visible = false;
                    }

                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Columns[4].Width = 90;
                    FpSpread2.Sheets[0].Columns[4].Locked = true;


                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Columns[5].Width = 80;
                    FpSpread2.Sheets[0].Columns[5].Locked = true;


                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Columns[6].Width = 80;
                    FpSpread2.Sheets[0].Columns[6].Locked = true;

                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Text = "RPU";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Columns[7].Width = 100;
                    FpSpread2.Sheets[0].Columns[7].Locked = true;

                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Sailing Prize";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Columns[8].Width = 100;
                    //FpSpread2.Sheets[0].Columns[8].Locked = true;

                    if (cb_direct.Checked != true)
                    {
                        if (!rdb_dirbook.Checked)
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 9].Text = "AppQty";
                        else
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 9].Text = "AppCopy";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Columns[9].Width = 80;
                        FpSpread2.Sheets[0].Columns[9].Locked = true;
                    }
                    else
                    {
                        FpSpread2.Sheets[0].Columns[9].Visible = false;
                    }
                    if (!rdb_dirbook.Checked)
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 10].Text = "InwardQty";
                    else
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 10].Text = "InwardCopy";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Columns[10].Width = 90;

                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Bill No";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Columns[11].Width = 100;
                    FpSpread2.Sheets[0].Columns[11].Visible = false;

                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Locked = true;                 
                    FarPoint.Web.Spread.CheckBoxCellType chkbox = new FarPoint.Web.Spread.CheckBoxCellType();
                    chkbox.AutoPostBack = false;
                    FarPoint.Web.Spread.DoubleCellType db = new FarPoint.Web.Spread.DoubleCellType();
                    db.ErrorMessage = "Only Allowed For Numbers";
                    FpSpread2.Columns[9].CellType = db;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FpSpread2.Sheets[0].RowCount++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["OrderCode"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["PurchaseOrderPK"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCode"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["VendorPK"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);
                        //  FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["rpu"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        //
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemName"]);//itemcodeb;
                        //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["rpu"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        if (!rdb_dirbook.Checked)
                        {
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Itempk"]);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemUnit"]);

                        }
                        else
                        {
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Author"]);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = "";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = "";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(ds.Tables[0].Rows[i]["book_status"]);
                        }
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";


                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["RPU"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].CellType = db;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = "";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].BackColor = Color.BlanchedAlmond;

                        if (cb_direct.Checked != true)
                        {
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[i]["app_qty"]);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                        }
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].CellType = db;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Text = "";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].BackColor = Color.BlanchedAlmond;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].Text = "";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].BackColor = Color.BlanchedAlmond;
                    }
                    FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                    FpSpread2.SaveChanges();
                    FpSpread2.Visible = true;
                    lbl_error1.Visible = false;
                    //rdb_store.Visible = true;
                    //rdb_hostel.Visible = true;
                    //rdb_dept.Visible = true;
                    //hostelfalse();
                    //deptnamefalse();
                    //storetrue();

                    lbl_billdno.Visible = true;
                    txt_dbillno.Visible = true;
                    lbl_billddate.Visible = true;
                    txt_BilldDate.Visible = true;
                }
                else
                {
                    divbtns.Visible = false;
                    lbl_error1.Visible = true;
                    FpSpread2.Visible = false;
                    hostelfalse();
                    storefalse();
                    deptnamefalse();
                    Libraryfalse();
                    //rdb_store.Visible = false;
                    //rdb_hostel.Visible = false;
                    //rdb_dept.Visible = false;

                    lbl_billdno.Visible = false;
                    txt_dbillno.Visible = false;
                    lbl_billddate.Visible = false;
                    txt_BilldDate.Visible = false;
                    lbl_error1.Text = "No Record Found";
                }
            }
            if (popitems.Trim() == "")
            {
                lbl_error1.Visible = true;
                FpSpread2.Visible = false;
                hostelfalse();
                storefalse();
                deptnamefalse();
                Libraryfalse();
                //rdb_store.Visible = false;
                //rdb_hostel.Visible = false;
                divbtns.Visible = false;
                lbl_billdno.Visible = false;
                txt_dbillno.Visible = false;
                lbl_billddate.Visible = false;
                txt_BilldDate.Visible = false;
                //rdb_dept.Visible = false;
                lbl_error1.Text = "Please update vendor items";
            }

        }
        catch
        {
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_name from staffmaster where resign =0 and settled =0 and staff_name like '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }

    public string getbrandname(string Criteria, string itemcode, string ordercode)
    {
        string brandname = "";
        try
        {
            string textcode = d2.GetFunction("select brand_code from PurchaseOrder_Items where item_code='" + itemcode + "' and order_code='" + ordercode + "'");
            if (textcode.Trim() == "")
            {
                textcode = "0";
            }
            string select_subno = "select TextVal from textvaltable where TextCriteria='" + Criteria + "' and college_code =" + collegecode1 + " and TextCode='" + textcode + "'";
            ds1.Clear();
            ds1 = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                brandname = Convert.ToString(ds1.Tables[0].Rows[0]["TextVal"]);
            }
            else
            {
                brandname = "";
            }
        }
        catch { }
        return brandname;
    }
    protected void FpSpread2_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = FpSpread2.Sheets[0].ActiveRow.ToString();
            string actcol = FpSpread2.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (FpSpread2.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < FpSpread2.Sheets[0].RowCount; i++)
                        {
                            FpSpread2.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < FpSpread2.Sheets[0].RowCount; i++)
                        {
                            FpSpread2.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void bindgoodinwardcode()
    {
        try
        {
            string selectquery = "select GIAcr,GIStNo,GISize from IM_CodeSettings order by StartDate";
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string newitemcode = "";

                string itemacronym = Convert.ToString(ds.Tables[0].Rows[0]["GIAcr"]);
                string itemstarno = Convert.ToString(ds.Tables[0].Rows[0]["GIStNo"]);
                string itemsize = Convert.ToString(ds.Tables[0].Rows[0]["GISize"]);
                selectquery = " select distinct top (1) GoodsInwardCode  from IT_GoodsInward where  GoodsInwardCode like '" + Convert.ToString(itemacronym) + "%' order by GoodsInwardCode desc";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string itemcode = Convert.ToString(ds.Tables[0].Rows[0]["GoodsInwardCode"]);
                    string itemacr = Convert.ToString(itemacronym);
                    int len = itemacr.Length;
                    itemcode = itemcode.Remove(0, len);
                    int len1 = Convert.ToString(itemcode).Length;
                    string newnumber = Convert.ToString((Convert.ToInt32(itemcode) + 1));
                    len = Convert.ToString(newnumber).Length;
                    len1 = Convert.ToInt32(itemsize) - len;
                    if (len1 == 2)
                    {
                        newitemcode = "00" + newnumber;
                    }
                    else if (len1 == 1)
                    {
                        newitemcode = "0" + newnumber;
                    }
                    else if (len1 == 4)
                    {
                        newitemcode = "0000" + newnumber;
                    }
                    else if (len1 == 3)
                    {
                        newitemcode = "000" + newnumber;
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
                    int len1 = Convert.ToInt32(items);//items.Length;

                    int size = len1 - len;//Convert.ToInt32(itemacr);

                    if (size == 2)
                    {
                        newitemcode = "00" + itemstarno;
                    }
                    else if (size == 1)
                    {
                        newitemcode = "0" + itemstarno;
                    }
                    else if (size == 4)
                    {
                        newitemcode = "0000" + itemstarno;
                    }
                    else if (size == 3)
                    {
                        newitemcode = "000" + itemstarno;
                    }
                    else
                    {
                        newitemcode = Convert.ToString(itemstarno);
                    }
                    newitemcode = Convert.ToString(itemacronym) + "" + Convert.ToString(newitemcode);
                }
                ViewState["goodinwardcode"] = newitemcode;
            }
        }
        catch { }
    }


    protected void btn_receive_Click(object sender, EventArgs e)
    {
        try
        {
            //bindgoodinwardcode();
            bindordercode();
            bool insertquery = false;
            bool inward_stauts = false;
            FpSpread2.SaveChanges();
            FpSpread4.SaveChanges();
            string firstdate = Convert.ToString(txt_fromdate1.Text);
            string seconddate = Convert.ToString(txt_todate1.Text);
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            if (btn_receive.Text == "Received")
            {
                if (txt_staff.Text != "")
                {
                    string dtaccessdate = DateTime.Now.ToString("MM/dd/yyyy");
                    string dtaccesstime = DateTime.Now.ToLongTimeString();
                    string newitemcode = Convert.ToString(ViewState["goodinwardcode"]);
                    string staff = Convert.ToString(ViewState["WardenCode"]);
                    bool greater = false;
                    string invoiceno = ""; string InvoiceDate = "";
                    invoiceno = Convert.ToString(txt_dbillno.Text.Trim());
                    InvoiceDate = Convert.ToString(txt_BilldDate.Text.Trim());

                    if (InvoiceDate.Trim() != "")
                    {
                        string[] split3 = InvoiceDate.Split('/');
                        DateTime invdate = Convert.ToDateTime(split3[1] + "/" + split3[0] + "/" + split3[2]);
                        InvoiceDate = invdate.ToString("MM/dd/yyyy");
                    }
                    if (cb_direct.Checked == false)
                    {
                        #region Inward

                        for (int row = 0; row < FpSpread2.Sheets[0].RowCount; row++)
                        {
                            if (FpSpread2.Sheets[0].Cells[row, 10].Text != "")
                            {
                                string sailingprize = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 8].Text);
                                if (sailingprize.Trim() == "")
                                {
                                    sailingprize = "0";
                                }
                                string bookid = "";
                                string Bookstataus = "";
                                string Author = "";
                                string Title = "";
                                string ordqty = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 9].Text);
                                string inwardquantity = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 10].Text);
                                //string popk = cbl_orders.SelectedValue;
                                string popk = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 1].Tag);
                                string ordercode = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 1].Text);
                                string rpu = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 7].Text);
                                string ifk = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 5].Tag);
                                //invoiceno = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 11].Text);
                                if (rdb_dirbook.Checked == true)
                                {
                                    bookid = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 5].Tag);
                                    Bookstataus = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 6].Tag);
                                    Author = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 4].Tag);
                                    Title = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 4].Text);
                                }

                                if (Convert.ToUInt32(ordqty) >= Convert.ToUInt32(inwardquantity))
                                {
                                    string goodsdeptfk = ""; string depq = ""; string deptq = "";
                                    if (rdb_dirmess.Checked == true)
                                    {
                                        goodsdeptfk = Convert.ToString(ddl_Hostelname.SelectedValue);
                                        depq = " and DeptFK='" + goodsdeptfk + "'";
                                        deptq = " ,DeptFK";
                                        goodsdeptfk = ",'" + Convert.ToString(ddl_Hostelname.SelectedValue) + "'";
                                    }
                                    else if (rdb_dirdept.Checked == true)
                                    {
                                        goodsdeptfk = Convert.ToString(ddl_deptname.SelectedValue);
                                        depq = " and DeptFK='" + goodsdeptfk + "'";
                                        deptq = " ,DeptFK";
                                        goodsdeptfk = ",'" + Convert.ToString(ddl_deptname.SelectedValue) + "'";
                                    }
                                    else if (rdb_dirbook.Checked == true)
                                    {
                                        goodsdeptfk = Convert.ToString(ddl_Libname.SelectedValue);
                                        depq = " and DeptFK='" + goodsdeptfk + "'";
                                        deptq = " ,DeptFK";
                                        goodsdeptfk = ",'" + Convert.ToString(ddl_Libname.SelectedValue) + "'";
                                    }
                                    else { goodsdeptfk = ""; depq = ""; deptq = ""; }

                                    //string goodsdeptfk = "";
                                    //if (rdb_hostel.Checked == true)
                                    //{
                                    //    goodsdeptfk = Convert.ToString(ddl_Hostelname.SelectedValue);
                                    //}
                                    //else if (rdb_dept.Checked == true)
                                    //{
                                    //    goodsdeptfk = Convert.ToString(ddl_deptname.SelectedValue);
                                    //}
                                    //else { goodsdeptfk = "0"; }

                                    string insetquery = "if exists (select * from IT_GoodsInward where PurchaseOrderFK='" + popk + "' and itemfk='" + ifk + "' and GoodsInwardCode='" + newitemcode + "' and ISNULL(Inward_Type,0)=0  " + depq + ") update IT_GoodsInward set InwardQty =InwardQty +'" + inwardquantity + "' where PurchaseOrderFK='" + popk + "' and itemfk='" + ifk + "' and VendorFK='" + Convert.ToString(ddl_vendor1.SelectedItem.Value) + "' and ISNULL(Inward_Type,0)=0 " + depq + "  else insert into IT_GoodsInward(GoodsInwardCode,GoodsInwardDate,OrderQty,InwardQty,PurchaseOrderFK, itemfk,Received_staffcode,VendorFK,InvoiceNo,InvoiceDate" + deptq + ") values('" + newitemcode + "','" + dtaccessdate + "','" + ordqty + "','" + inwardquantity + "','" + popk + "','" + ifk + "','" + staff + "','" + Convert.ToString(ddl_vendor1.SelectedItem.Value) + "','" + invoiceno + "','" + InvoiceDate + "'" + goodsdeptfk + ")";
                                    int ins = d2.update_method_wo_parameter(insetquery, "Text");
                                    if (ins != 0)
                                    {
                                        insertquery = true;
                                    }
                                    if (rdb_dirstore.Checked == true)
                                    {
                                        string inwrdfk = d2.GetFunction("select GoodsInwardPK from IT_GoodsInward Where GoodsInwardCode='" + newitemcode + "' and itemfk='" + ifk + "'");
                                        string sfk = ddl_storename.SelectedValue;
                                        inwardquantity = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 9].Text);

                                        string inserquery = " if exists (select * from IT_StockDetail where OrderFK ='" + popk + "' and InwardFK ='" + inwrdfk + "' and ItemFK ='" + ifk + "' and StoreFK ='" + sfk + "' and InwardType ='1') update IT_StockDetail set InwardQty=InwardQty+'" + inwardquantity + "',BalQty =BalQty +'" + ordqty + "' where OrderFK ='" + popk + "' and InwardFK ='" + inwrdfk + "' and ItemFK ='" + ifk + "' and StoreFK ='" + sfk + "' and InwardType ='1' else insert into IT_StockDetail(OrderFK,InwardFK,ItemFK,OrderQty,InwardQty,BalQty,InwardRPU,StoreFK,InwardType) values('" + popk + "','" + inwrdfk + "','" + ifk + "','" + ordqty + "','" + inwardquantity + "','" + ordqty + "','" + rpu + "','" + sfk + "','1')";
                                        int in_s = d2.update_method_wo_parameter(inserquery, "Text");
                                        if (in_s != 0)
                                        {
                                            insertquery = true;
                                        }
                                    }
                                    else if (rdb_dirmess.Checked == true)
                                    {
                                        string deptfk = ddl_Hostelname.SelectedValue;
                                        string inwrdfk = d2.GetFunction("select GoodsInwardPK from IT_GoodsInward Where GoodsInwardCode='" + newitemcode + "' and itemfk='" + ifk + "'");
                                        //string insert_mess = "if exists (select * from IT_StockDeptDetail where OrderFK ='" + popk + "' and InwardFK ='" + inwrdfk + "' and ItemFK ='" + ifk + "' and DeptFK ='" + deptfk + "' and Inward_Type='1') update IT_StockDeptDetail set IssuedQty=IssuedQty+'" + inwardquantity + "',BalQty =BalQty +'" + ordqty + "' where OrderFK ='" + popk + "' and InwardFK ='" + inwrdfk + "' and ItemFK ='" + ifk + "' and DeptFK ='" + deptfk + "' and Inward_Type='1' else insert into IT_StockDeptDetail(OrderFK,InwardFK,ItemFK,IssuedQty,BalQty,IssuedRPU,DeptFK,Inward_Type) values('" + popk + "','" + inwrdfk + "','" + ifk + "','" + inwardquantity + "','" + ordqty + "','" + rpu + "','" + deptfk + "','1')";
                                        //string invrpu = d2.GetFunction("select IssuedRPU from IT_StockDeptDetail where ItemFK ='" + ifk + "' and DeptFK ='" + deptfk + "' and IssuedQty<>isnull(UsedQty,0)");
                                        //double avrrpu = 0;//13.02.2018 barath
                                        //if (invrpu.Trim() != "0")
                                        //{
                                        //    if (rpu != invrpu)
                                        //    {
                                        //        rpu = Convert.ToString(Convert.ToDouble(rpu) + Convert.ToDouble(invrpu));
                                        //        double.TryParse(rpu, out avrrpu);
                                        //        rpu = Convert.ToString(avrrpu / 2);
                                        //    }
                                        //}
                                        string insert_mess = "if exists (select * from IT_StockDeptDetail where ItemFK ='" + ifk + "' and DeptFK ='" + deptfk + "') update IT_StockDeptDetail set IssuedQty=IssuedQty+'" + inwardquantity + "',BalQty =BalQty +'" + ordqty + "'   ,IssuedRPU='" + rpu + "' where  ItemFK ='" + ifk + "' and DeptFK ='" + deptfk + "' else insert into IT_StockDeptDetail(ItemFK,IssuedQty,BalQty,IssuedRPU,DeptFK) values('" + ifk + "','" + inwardquantity + "','" + ordqty + "','" + rpu + "','" + deptfk + "')";
                                        insert_mess = insert_mess + " insert into IT_TransferItem (TrasnferDate,TransferType,TransferFrom,TrasferTo,TransferQty,itemfk,TransferRpu) values ('" + dtaccessdate + "','1','" + deptfk + "','" + deptfk + "','" + inwardquantity + "','" + ifk + "','" + rpu + "')";
                                        int in_s1 = d2.update_method_wo_parameter(insert_mess, "Text");
                                        if (in_s1 != 0)
                                        {
                                            insertquery = true;
                                        }
                                    }
                                    else if (rdb_dirdept.Checked == true)
                                    {
                                        string deptfk = Convert.ToString(ddl_deptname.SelectedItem.Value);
                                        string inwrdfk = d2.GetFunction("select GoodsInwardPK from IT_GoodsInward Where GoodsInwardCode='" + newitemcode + "' and itemfk='" + ifk + "'");
                                        //string invrpu = d2.GetFunction("select IssuedRPU from IT_StockDeptDetail where ItemFK ='" + ifk + "' and DeptFK ='" + deptfk + "' and IssuedQty<>isnull(UsedQty,0)");// and IssuedQty>=isnull(UsedQty,0) ";
                                        //double avrrpu = 0;//13.02.18 barath
                                        //if (invrpu.Trim() != "0")
                                        //{
                                        //    if (rpu != invrpu)
                                        //    {
                                        //        rpu = Convert.ToString(Convert.ToDouble(rpu) + Convert.ToDouble(invrpu));
                                        //        double.TryParse(rpu, out avrrpu);
                                        //        rpu = Convert.ToString(avrrpu / 2);
                                        //    }
                                        //}
                                        string insert_mess = "if exists (select * from IT_StockDeptDetail where ItemFK ='" + ifk + "' and DeptFK ='" + deptfk + "' ) update IT_StockDeptDetail set IssuedQty=IssuedQty+'" + inwardquantity + "',BalQty =BalQty +'" + ordqty + "' ,IssuedRPU='" + rpu + "' where ItemFK ='" + ifk + "' and DeptFK ='" + deptfk + "' else insert into IT_StockDeptDetail(ItemFK,IssuedQty,BalQty,IssuedRPU,DeptFK) values('" + ifk + "','" + inwardquantity + "','" + ordqty + "','" + rpu + "','" + deptfk + "')";
                                        insert_mess = insert_mess + " insert into IT_TransferItem (TrasnferDate,TransferType,TransferFrom,TrasferTo,TransferQty,itemfk,TransferRpu) values ('" + dtaccessdate + "','4','" + deptfk + "','" + deptfk + "','" + inwardquantity + "','" + ifk + "','" + rpu + "')";
                                        int in_s1 = d2.update_method_wo_parameter(insert_mess, "Text");
                                        if (in_s1 != 0)
                                        {
                                            insertquery = true;
                                        }
                                    }
                                    //Library 26.1.2019

                                    else if (rdb_dirbook.Checked == true)
                                    {
                                        string accno = "";
                                        string libcode = Convert.ToString(ddl_Libname.SelectedItem.Value);
                                        if (libcode != "")
                                        {

                                            accno = Auto_AccessNo(libcode);
                                            if (accno != "")
                                            {

                                                string[] spilt = accno.Split('#');
                                                string aa = spilt[0].Split(new string[] { spilt[1].ToString() }, StringSplitOptions.None)[1];
                                                int inwardcopy = Convert.ToInt32(inwardquantity);
                                                string Accnumber = "";
                                                for (int t = 0; t < inwardcopy; t++)
                                                {
                                                    if (Accnumber == "")
                                                        Accnumber = accno;
                                                    else
                                                    {
                                                        int no = Convert.ToInt32(aa);
                                                        no = no + (t + 1);
                                                        string acc = spilt[1] + Convert.ToString(no);
                                                        Accnumber = Accnumber + "," + acc;
                                                    }
                                                }
                                                if (Accnumber != "")
                                                {
                                                    if (Accnumber.Contains('#'))
                                                        Accnumber = Accnumber.Replace('#', ' ');
                                                    Accnumber = Accnumber.Trim();
                                                    if (Bookstataus == "Request")
                                                    {
                                                        string[] spilt1 = Accnumber.Split(',');
                                                        if (spilt1.Length > 0)
                                                        {
                                                            if (spilt1.Length == 1)
                                                            {
                                                                string insqry = "if exists (select * from bookdetails where  book_status='Request' and BookID='" + bookid + "') update bookdetails set Lib_Code='" + libcode + "',CopyNo='" + 1 + "',Acc_No='" + spilt1[0].ToString() + "',book_status='Available' where  book_status='Request' and BookID='" + bookid + "'";
                                                                int inscnt = d2.update_method_wo_parameter(insqry, "Text");
                                                            }
                                                            else
                                                            {
                                                                int copy = 0;
                                                                for (int k1 = 0; k1 < spilt1.Length; k1++)
                                                                {
                                                                    copy++;
                                                                    string insqry = "if exists (select * from bookdetails where  book_status='Request' and BookID='" + bookid + "') update bookdetails set Lib_Code='" + libcode + "',CopyNo='" + copy.ToString() + "',Acc_No='" + spilt1[k1].ToString() + "',book_status='Available' where  book_status='Request' and BookID='" + bookid + "' else insert into bookdetails(acc_no,title,author,book_status,Lib_Code,MulticopyAccNos,CopyNo,Rack_Flag) values('" + spilt1[k1].ToString() + "','" + Title + "','" + Author + "','Available','" + libcode + "','" + Accnumber + "','" + copy.ToString() + "','')";
                                                                    int inscnt = d2.update_method_wo_parameter(insqry, "Text");

                                                                }
                                                            }
                                                        }

                                                    }
                                                    else
                                                    {
                                                        string copnumber = d2.GetFunction("select isnull(CopyNo,'')CopyNo from bookdetails where BookID='" + bookid + "' and Lib_Code='" + libcode + "'");
                                                        int copyy = 1;
                                                        if (copnumber != "" && copnumber != "0")
                                                        {
                                                            copyy = Convert.ToInt32(copnumber);
                                                        }
                                                        string[] spilt1 = Accnumber.Split(',');
                                                        if (spilt1.Length > 0)
                                                        {
                                                            if (spilt1.Length == 1)
                                                            {
                                                                string insqry = "insert into bookdetails(acc_no,title,author,book_status,Lib_Code,CopyNo,Rack_Flag) values('" + spilt1[0].ToString() + "','" + Title + "','" + Author + "','Available','" + libcode + "','" + copyy.ToString() + "','')";
                                                                int inscnt = d2.update_method_wo_parameter(insqry, "Text");

                                                            }
                                                            else
                                                            {
                                                                for (int k1 = 0; k1 < spilt1.Length; k1++)
                                                                {
                                                                    copyy = copyy + k1;
                                                                    string insqry = "insert into bookdetails(acc_no,title,author,book_status,Lib_Code,MulticopyAccNos,CopyNo,Rack_Flag) values('" + spilt1[k1].ToString() + "','" + Title + "','" + Author + "','Available','" + libcode + "','" + Accnumber + "','" + copyy.ToString() + "','')";
                                                                    int inscnt = d2.update_method_wo_parameter(insqry, "Text");

                                                                }
                                                            }
                                                        }

                                                    }
                                                }
                                            }

                                        }
                                        //string inwrdfk = d2.GetFunction("select GoodsInwardPK from IT_GoodsInward Where GoodsInwardCode='" + newitemcode + "' and itemfk='" + ifk + "'");

                                        //string insert_mess = "if exists (select * from IT_StockDeptDetail where ItemFK ='" + ifk + "' and DeptFK ='" + deptfk + "' ) update IT_StockDeptDetail set IssuedQty=IssuedQty+'" + inwardquantity + "',BalQty =BalQty +'" + ordqty + "' ,IssuedRPU='" + rpu + "' where ItemFK ='" + ifk + "' and DeptFK ='" + deptfk + "' else insert into IT_StockDeptDetail(ItemFK,IssuedQty,BalQty,IssuedRPU,DeptFK) values('" + ifk + "','" + inwardquantity + "','" + ordqty + "','" + rpu + "','" + deptfk + "')";
                                        //insert_mess = insert_mess + " insert into IT_TransferItem (TrasnferDate,TransferType,TransferFrom,TrasferTo,TransferQty,itemfk,TransferRpu) values ('" + dtaccessdate + "','4','" + deptfk + "','" + deptfk + "','" + inwardquantity + "','" + ifk + "','" + rpu + "')";
                                        //int in_s1 = d2.update_method_wo_parameter(insert_mess, "Text");
                                        //if (in_s1 != 0)
                                        //{
                                        //    insertquery = true;
                                        //}
                                    }


                                    if (insertquery != false)
                                    {
                                        string inwardstatus = "update IT_PurchaseOrder set InwardStatus='1' where OrderCode='" + ordercode + "' ";
                                        int instatus = d2.update_method_wo_parameter(inwardstatus, "text");
                                        //string purchasepk = d2.GetFunction("select PurchaseOrderPK from IT_PurchaseOrder where OrderCode='" + ordercode + "'");
                                        string podetails = "update IT_PurchaseOrderDetail set Inward_Status='1',Sailing_prize='" + sailingprize + "' where PurchaseOrderFK='" + popk + "' and ItemFK='" + ifk + "'";
                                        int podelip = d2.update_method_wo_parameter(podetails, "Text");
                                        if (instatus != 0 && podelip != 0)
                                        {
                                            inward_stauts = true;
                                        }
                                    }
                                }
                                else
                                {
                                    greater = true;
                                }
                            }
                            if (insertquery == false)
                            {
                                imgdiv2.Visible = true;
                                lbl_alert.Visible = true;
                                lbl_alert.Text = "Please Enter the Inward Quantity";
                            }
                        }
                        if (greater == true)
                        {
                            imgdiv2.Visible = true;
                            lbl_alert.Visible = true;
                            lbl_alert.Text = "Order Quantity Greater than Inward Quantity";
                        }
                        #endregion
                    }
                    else
                    {
                        string Goods_date = string.Empty;
                        #region Direct inward
                        for (int row = 0; row < FpSpread4.Sheets[0].RowCount; row++)
                        {
                            if (FpSpread4.Sheets[0].Cells[row, 7].Text != "" && FpSpread4.Sheets[0].Cells[row, 9].Text != "")
                            {
                                string itfk = Convert.ToString(FpSpread4.Sheets[0].Cells[row, 6].Tag);
                                string invqty = Convert.ToString(FpSpread4.Sheets[0].Cells[row, 9].Text);
                                string invrpu = Convert.ToString(FpSpread4.Sheets[0].Cells[row, 7].Text);
                                //invoiceno = Convert.ToString(FpSpread4.Sheets[0].Cells[row, 18].Text);
                                string purchaseorderpk = "";
                                #region 24.03.16
                                //  Qty rpu Dis tax tocost extax educess high othercharge desc date sailingprize //pohashtable
                                string q1 = "";

                                foreach (DictionaryEntry item in pohashtable)
                                {
                                    string podetails = Convert.ToString(item.Value);
                                    string poitemkey = Convert.ToString(item.Key);
                                    if (poitemkey == itfk)
                                    {
                                        string[] split3 = podetails.Split('-');
                                        if (split3.Length > 0)
                                        {
                                            string discount1 = Convert.ToString(split3[2]);
                                            string tax = Convert.ToString(split3[3]);
                                            string totalcosr = Convert.ToString(split3[4]);
                                            string extax = Convert.ToString(split3[5]);
                                            string educess = Convert.ToString(split3[6]);
                                            string eduhigher = Convert.ToString(split3[7]);
                                            string otherchar = Convert.ToString(split3[8]);
                                            string decription = Convert.ToString(split3[9]);
                                            string fpdate = Convert.ToString(split3[10]);
                                            string[] splitdate = fpdate.Split('/');
                                            DateTime goodsdate = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);

                                            Goods_date = goodsdate.ToString("MM/dd/yyyy");
                                            string sailingprice = Convert.ToString(split3[11]);
                                            if (sailingprice.Trim() == "")
                                            {
                                                sailingprice = "0";
                                            }
                                            string discountamt = "";
                                            string discountper = "";
                                            if (invqty.Trim() == "")
                                            {
                                                invqty = "0";
                                            }
                                            if (invrpu.Trim() == "")
                                            {
                                                invrpu = "0";
                                            }
                                            if (cbdis.Checked == true)
                                            {
                                                discountamt = discount1;
                                            }
                                            else
                                            {
                                                discountper = discount1;
                                            }
                                            if (discountamt.Trim() == "")
                                            {
                                                discountamt = "0";
                                            }
                                            if (discountper == "")
                                            {
                                                discountper = "0";
                                            }
                                            if (tax.Trim() == "")
                                            {
                                                tax = "0";
                                            }
                                            if (totalcosr.Trim() == "")
                                            {
                                                totalcosr = "0";
                                            }
                                            if (extax.Trim() == "")
                                            {
                                                extax = "0";
                                            }
                                            if (educess.Trim() == "")
                                            {
                                                educess = "0";
                                            }
                                            if (eduhigher.Trim() == "")
                                            {
                                                eduhigher = "0";
                                            }
                                            if (otherchar.Trim() == "")
                                            {
                                                otherchar = "0";
                                            }
                                            if (decription.Trim() == "")
                                            {
                                                decription = "";
                                            }
                                            string orderdate = Convert.ToString(split3[10]);
                                            string[] split4 = orderdate.Split('/');
                                            DateTime dt3 = new DateTime();
                                            dt3 = Convert.ToDateTime(split4[1] + "/" + split4[0] + "/" + split4[2]);
                                            q1 = "if not exists(select*from IT_PurchaseOrder where OrderCode='" + Convert.ToString(ViewState["ordercode"]) + "') insert into IT_PurchaseOrder (OrderCode,OrderDate,ApproveStatus,VendorFK,OrderDescription,InwardStatus) values ('" + Convert.ToString(ViewState["ordercode"]) + "','" + dt3.ToString("MM/dd/yyyy") + "','3','" + Convert.ToString(ddl_vendor2.SelectedItem.Value) + "','Direct Inward','1')";
                                            int insertpurchase = d2.update_method_wo_parameter(q1, "Text");
                                            if (insertpurchase != 0)
                                            {
                                                purchaseorderpk = d2.GetFunction("select purchaseorderpk from IT_PurchaseOrder where OrderCode='" + Convert.ToString(ViewState["ordercode"]) + "' and VendorFK='" + Convert.ToString(ddl_vendor2.SelectedItem.Value) + "'");
                                                string q2 = "insert into IT_PurchaseOrderDetail (ItemFK,Qty,RPU,IsDiscountPercent, DiscountAmt,TaxPercent,ExeciseTaxPer,EduCessPer,HigherEduCessPer,OtherChargeAmt,OtherChargeDesc,AppQty,PurchaseOrderFK,Inward_Status,Sailing_prize)values('" + itfk + "','" + invqty + "','" + invrpu + "','" + discountper + "','" + discountamt + "','" + tax + "','" + extax + "','" + educess + "','" + eduhigher + "','" + otherchar + "','" + decription + "','" + invrpu + "','" + purchaseorderpk + "','1','" + sailingprice + "')";
                                                int ins_purchaseorderdet = d2.update_method_wo_parameter(q2, "Text");
                                                if (ins_purchaseorderdet != 0)
                                                {
                                                    // insertquery = true;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion

                                string goodsdeptfk = ""; string depq = ""; string deptq = "";
                                if (rdb_dirmess.Checked == true)//rdb_hostel
                                {
                                    goodsdeptfk = Convert.ToString(ddl_Hostelname.SelectedValue);
                                    depq = " and DeptFK='" + goodsdeptfk + "'";
                                    deptq = " ,DeptFK";
                                    goodsdeptfk = ",'" + Convert.ToString(ddl_Hostelname.SelectedValue) + "'";
                                }
                                else if (rdb_dirdept.Checked == true)
                                {
                                    goodsdeptfk = Convert.ToString(ddl_deptname.SelectedValue);
                                    depq = " and DeptFK='" + goodsdeptfk + "'";
                                    deptq = " ,DeptFK";
                                    goodsdeptfk = ",'" + Convert.ToString(ddl_deptname.SelectedValue) + "'";
                                }
                                else { goodsdeptfk = ""; depq = ""; deptq = ""; }//delsi
                                string dirinsqry = "if exists (select * from IT_GoodsInward where itemfk='" + itfk + "' and GoodsInwardCode='" + newitemcode + "' and  PurchaseOrderFK='" + purchaseorderpk + "' and Inward_Type='1' " + depq + ") update IT_GoodsInward set InwardQty =InwardQty +'" + invqty + "'  where  itemfk='" + itfk + "' and VendorFK='" + Convert.ToString(ddl_vendor2.SelectedItem.Value) + "' and  PurchaseOrderFK='" + purchaseorderpk + "' and Inward_Type='1' " + depq + " else insert into IT_GoodsInward(GoodsInwardCode,GoodsInwardDate,InwardQty,itemfk,Received_staffcode,VendorFK,PurchaseOrderFK,Inward_Type,InvoiceNo,InvoiceDate " + deptq + ") values('" + newitemcode + "','" + Goods_date + "','" + invqty + "','" + itfk + "','" + staff + "','" + Convert.ToString(ddl_vendor2.SelectedItem.Value) + "','" + purchaseorderpk + "','1','" + invoiceno + "','" + InvoiceDate + "' " + goodsdeptfk + ")";
                                int ins = d2.update_method_wo_parameter(dirinsqry, "Text");
                                if (ins != 0)
                                {
                                    insertquery = true;
                                }
                                if (rdb_dirstore.Checked == true)
                                {
                                    string inwrdfk = d2.GetFunction("select GoodsInwardPK from IT_GoodsInward Where GoodsInwardCode='" + newitemcode + "' and itemfk='" + itfk + "' and Inward_Type='1'");
                                    string sfk = ddl_storename.SelectedValue;
                                    string inserquery = "if exists (select * from IT_StockDetail where  InwardFK ='" + inwrdfk + "' and ItemFK ='" + itfk + "' and StoreFK ='" + sfk + "' and InwardType ='2') update IT_StockDetail set InwardQty=InwardQty+'" + invqty + "' where InwardFK ='" + inwrdfk + "' and ItemFK ='" + itfk + "' and StoreFK ='" + sfk + "' and InwardType ='2' else insert into IT_StockDetail(InwardFK,ItemFK,InwardQty,InwardRPU,StoreFK,InwardType) values('" + inwrdfk + "','" + itfk + "','" + invqty + "','" + invrpu + "','" + sfk + "','2')";
                                    //string inserquery = "if exists (select * from IT_StockDetail where  ItemFK ='" + itfk + "' and StoreFK ='" + sfk + "' ) update IT_StockDetail set InwardQty=InwardQty+'" + invqty + "' where ItemFK ='" + itfk + "'  and StoreFK ='" + sfk + "' else insert into IT_StockDetail(ItemFK,InwardQty,InwardRPU,StoreFK) values('" + itfk + "','" + invqty + "','" + invrpu + "','" + sfk + "')";
                                    int in_s = d2.update_method_wo_parameter(inserquery, "Text");
                                    if (in_s != 0)
                                    {
                                        insertquery = true;
                                    }
                                }
                                else if (rdb_dirmess.Checked == true)
                                {
                                    string deptfk = ddl_Hostelname.SelectedValue;
                                    //string inwrdfk = d2.GetFunction("select GoodsInwardPK from IT_GoodsInward Where GoodsInwardCode='" + newitemcode + "' and itemfk='" + itfk + "' and Inward_Type='1'");
                                    // string insert_mess = "if exists (select * from IT_StockDeptDetail where InwardFK ='" + inwrdfk + "' and ItemFK ='" + itfk + "' and DeptFK ='" + deptfk + "' and Inward_Type='2') update IT_StockDeptDetail set IssuedQty=IssuedQty+'" + invqty + "',balQty=isnull(balqty,0)+'" + invqty + "' where InwardFK ='" + inwrdfk + "' and ItemFK ='" + itfk + "' and DeptFK ='" + deptfk + "' and Inward_Type='2' else insert into IT_StockDeptDetail(InwardFK,ItemFK,IssuedQty,IssuedRPU,DeptFK,Inward_Type,balQty) values('" + inwrdfk + "','" + itfk + "','" + invqty + "','" + invrpu + "','" + deptfk + "','2','" + invqty + "')";
                                    //string rpu = d2.GetFunction("select IssuedRPU from IT_StockDeptDetail where ItemFK ='" + itfk + "' and DeptFK ='" + deptfk + "' and IssuedQty<>isnull(UsedQty,0) ");
                                    //double avrrpu = 0;//13.02.18 barath
                                    //if (rpu.Trim() != "0")
                                    //{
                                    //    if (rpu != invrpu)
                                    //    {
                                    //        invrpu = Convert.ToString(Convert.ToDouble(rpu) + Convert.ToDouble(invrpu));
                                    //        double.TryParse(invrpu, out avrrpu);
                                    //        invrpu = Convert.ToString(avrrpu / 2);
                                    //    }
                                    //}
                                    string insert_mess = "if exists (select * from IT_StockDeptDetail where ItemFK ='" + itfk + "' and DeptFK ='" + deptfk + "' ) update IT_StockDeptDetail set IssuedQty=IssuedQty+'" + invqty + "',balQty=isnull(balqty,0)+'" + invqty + "' ,IssuedRPU='" + invrpu + "' where ItemFK ='" + itfk + "' and DeptFK ='" + deptfk + "' else insert into IT_StockDeptDetail(ItemFK,IssuedQty,IssuedRPU,DeptFK,balQty) values('" + itfk + "','" + invqty + "','" + invrpu + "','" + deptfk + "','" + invqty + "')";
                                    insert_mess = insert_mess + " insert into IT_TransferItem (TrasnferDate,TransferType,TransferFrom,TrasferTo,TransferQty,itemfk,TransferRpu) values ('" + Goods_date + "','1','" + deptfk + "','" + deptfk + "','" + invqty + "','" + itfk + "','" + invrpu + "')";
                                    int in_s1 = d2.update_method_wo_parameter(insert_mess, "Text");
                                    if (in_s1 != 0)
                                    {
                                        insertquery = true;
                                    }
                                }
                                else if (rdb_dirdept.Checked == true)
                                {
                                    string deptfk = Convert.ToString(ddl_deptname.SelectedItem.Value);
                                    //string inwrdfk = d2.GetFunction("select GoodsInwardPK from IT_GoodsInward Where GoodsInwardCode='" + newitemcode + "' and itemfk='" + itfk + "'");
                                    //string insert_mess = "if exists (select * from IT_StockDeptDetail where InwardFK ='" + inwrdfk + "' and ItemFK ='" + itfk + "' and DeptFK ='" + deptfk + "' and Inward_Type='2') update IT_StockDeptDetail set IssuedQty=IssuedQty+'" + invqty + "',balQty=isnull(balqty,0)+'" + invqty + "' where InwardFK ='" + inwrdfk + "' and ItemFK ='" + itfk + "' and DeptFK ='" + deptfk + "' and Inward_Type='2' else insert into IT_StockDeptDetail(InwardFK,ItemFK,IssuedQty,IssuedRPU,DeptFK,Inward_Type,balQty) values('" + inwrdfk + "','" + itfk + "','" + invqty + "','" + invrpu + "','" + deptfk + "','2','" + invqty + "')";
                                    //string rpu = d2.GetFunction("select IssuedRPU from IT_StockDeptDetail where ItemFK ='" + itfk + "' and DeptFK ='" + deptfk + "' and IssuedQty<>isnull(UsedQty,0)");// and IssuedQty>=isnull(UsedQty,0) ";
                                    //double avrrpu = 0;//13.02.18 barath 
                                    //if (rpu.Trim() != "0")
                                    //{
                                    //    if (rpu != invrpu)
                                    //    {
                                    //        invrpu = Convert.ToString(Convert.ToDouble(rpu) + Convert.ToDouble(invrpu));
                                    //        double.TryParse(invrpu, out avrrpu);
                                    //        invrpu = Convert.ToString(avrrpu / 2);
                                    //    }
                                    //}
                                    string insert_mess = "if exists (select * from IT_StockDeptDetail where ItemFK ='" + itfk + "' and DeptFK ='" + deptfk + "' ) update IT_StockDeptDetail set IssuedQty=IssuedQty+'" + invqty + "',balQty=isnull(balqty,0)+'" + invqty + "' ,IssuedRPU='" + invrpu + "' where ItemFK ='" + itfk + "' and DeptFK ='" + deptfk + "'  else insert into IT_StockDeptDetail(ItemFK,IssuedQty,IssuedRPU,DeptFK,balQty) values('" + itfk + "','" + invqty + "','" + invrpu + "','" + deptfk + "','" + invqty + "')";
                                    insert_mess = insert_mess + " insert into IT_TransferItem (TrasnferDate,TransferType,TransferFrom,TrasferTo,TransferQty,itemfk,TransferRpu) values ('" + dtaccessdate + "','4','" + deptfk + "','" + deptfk + "','" + invqty + "','" + itfk + "','" + invrpu + "')";
                                    int in_s1 = d2.update_method_wo_parameter(insert_mess, "Text");
                                    if (in_s1 != 0)
                                    {
                                        insertquery = true;
                                    }
                                }
                            }
                            bindgoodinwardcode();
                        }
                        if (insertquery == true)
                        {
                            imgdiv2.Visible = true;
                            lbl_alert.Visible = true;
                            lbl_alert.Text = "Saved Successfully";
                            btn_go_Click(sender, e);
                            popwindow.Visible = false;
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alert.Visible = true;
                            lbl_alert.Text = "Please Enter the Rpu & Inward Quantity";
                        }
                        #endregion
                    }
                }
                else
                {
                    popwindow.Visible = true;
                    imgdiv2.Visible = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Please Select Anyone Staff Name";
                }
            }
            if (insertquery == true && inward_stauts == true)
            {
                popwindow.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Received Successfully";
                btn_go_Click(sender, e);
                btn_dir_Click(sender, e);
                pohashtable.Clear();
            }
        }
        catch (Exception ex)
        {
            imgdiv2.Visible = true;
            lbl_alert.Visible = true;
            lbl_alert.Text = ex.ToString();
        }
    }

    public void storetrue()
    {
        lbl_storename.Visible = true;
        ddl_storename.Visible = true;
    }
    public void storefalse()
    {
        lbl_storename.Visible = false;
        ddl_storename.Visible = false;
    }
    public void hosteltrue()
    {
        lbl_messname.Visible = true;
        ddl_Hostelname.Visible = true;
    }
    public void hostelfalse()
    {
        lbl_messname.Visible = false;
        ddl_Hostelname.Visible = false;
    }
    public void deptnametrue()
    {
        lbl_dept.Visible = true;
        ddl_deptname.Visible = true;
    }
    public void deptnamefalse()
    {
        lbl_dept.Visible = false;
        ddl_deptname.Visible = false;
    }
    protected void bind_popdept()
    {
        try
        {
            ds.Clear();
            string q = "select Dept_Code ,Dept_Name  from Department where college_code ='" + collegecode1 + "' order by Dept_Name";
            ds = d2.select_method_wo_parameter(q, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_deptname.DataSource = ds;
                ddl_deptname.DataTextField = "Dept_name";
                ddl_deptname.DataValueField = "dept_code";
                ddl_deptname.DataBind();


                cbl_dirdepbase.DataSource = ds;
                cbl_dirdepbase.DataTextField = "Dept_name";
                cbl_dirdepbase.DataValueField = "dept_code";
                cbl_dirdepbase.DataBind();
                for (int i = 0; i < cbl_dirdepbase.Items.Count; i++)
                {
                    cbl_dirdepbase.Items[i].Selected = true;
                    txt_dirdepartbase.Text = "Department(" + (cbl_dirdepbase.Items.Count) + ")";
                    cb_dirdepbase.Checked = true;
                }
            }
        }
        catch { }
    }
    //protected void rdb_store_Click(object sender, EventArgs e)
    //{
    //    storetrue();
    //    hostelfalse();
    //    deptnamefalse();
    //}
    //protected void rdb_Hostel_name(object sender, EventArgs e)
    //{
    //    hosteltrue();
    //    storefalse();
    //    deptnamefalse();
    //}
    //protected void rdb_dept_Click(object sender, EventArgs e)
    //{
    //    deptnametrue();
    //    hostelfalse();
    //    storefalse();
    //}
    protected void btn_reject1_Click(object sender, EventArgs e)
    {
        try
        {
            bool saveflage = false;
            bool greater = false;
            FpSpread2.SaveChanges();
            bindgoodinwardcode();
            if (btn_reject1.Text == "Reject")
            {
                if (txt_staff.Text != "")
                {
                    string dtaccessdate = DateTime.Now.ToString("MM/dd/yyyy");
                    string dtaccesstime = DateTime.Now.ToLongTimeString();
                    string staff = Convert.ToString(ViewState["WardenCode"]);

                    string invoiceno = Convert.ToString(txt_dbillno.Text.Trim());
                    string InvoiceDate = Convert.ToString(txt_BilldDate.Text.Trim());

                    if (InvoiceDate.Trim() != "")
                    {
                        string[] split3 = InvoiceDate.Split('/');
                        DateTime invdate = Convert.ToDateTime(split3[1] + "/" + split3[0] + "/" + split3[2]);
                        InvoiceDate = invdate.ToString("MM/dd/yyyy");
                    }
                    for (int row = 0; row < FpSpread2.Sheets[0].RowCount; row++)
                    {
                        if (FpSpread2.Sheets[0].Cells[row, 9].Text != "")
                        {
                            string newitemcode = Convert.ToString(ViewState["goodinwardcode"]);
                            string itemcode_value = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 5].Text);
                            string ordqty = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 9].Text);
                            string inwardquantity = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 10].Text);
                            string ordercode = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 1].Text);
                            string rpu = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 7].Text);
                            //string ifk = cbl_items1.SelectedValue; //string popk = cbl_orders.SelectedValue;
                            string ifk = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 5].Tag);
                            string popk = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 1].Tag);
                            if (Convert.ToUInt32(ordqty) >= Convert.ToUInt32(inwardquantity))
                            {
                                if (inwardquantity.Trim() == "")
                                {
                                    inwardquantity = "0";
                                }
                                string insetquery = "if exists (select * from IT_GoodsInward where PurchaseOrderFK='" + popk + "' and itemfk='" + ifk + "') update IT_GoodsInward set InwardQty =InwardQty +'" + inwardquantity + "' where PurchaseOrderFK='" + popk + "' and itemfk='" + ifk + "' else insert into IT_GoodsInward(GoodsInwardCode,GoodsInwardDate,OrderQty,InwardQty,PurchaseOrderFK,itemfk,Received_staffcode,InvoiceDate,InvoiceNo,VendorFK) values('" + newitemcode + "','" + dtaccessdate + "','" + ordqty + "','" + inwardquantity + "','" + popk + "','" + ifk + "','" + staff + "','" + InvoiceDate + "','" + invoiceno + "','" + Convert.ToString(ddl_vendor1.SelectedItem.Value) + "')";
                                int ins = d2.update_method_wo_parameter(insetquery, "Text");
                                string inwrdfk = d2.GetFunction("select GoodsInwardPK from IT_GoodsInward Where GoodsInwardCode='" + newitemcode + "'");
                                if (ins != 0)
                                {
                                    string purchase = " update IT_PurchaseOrder set InwardStatus='1' where PurchaseOrderPK='" + popk + "'";
                                    int poupdate = d2.update_method_wo_parameter(purchase, "Text");//ApproveStatus

                                    // string purchasepk = d2.GetFunction("select PurchaseOrderPK from IT_PurchaseOrder where OrderCode='" + ordercode + "'");
                                    string podetails = "update IT_PurchaseOrderDetail set Inward_Status='2' where PurchaseOrderFK='" + popk + "' and ItemFK='" + ifk + "'";//,RejQty='" + inwardquantity + "'
                                    int podelip = d2.update_method_wo_parameter(podetails, "Text");

                                    if (poupdate != 0 && podelip != 0)
                                    {
                                        saveflage = true;
                                    }
                                }
                            }
                            else
                            {
                                greater = true;
                            }
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alert.Visible = true;
                            lbl_alert.Text = "Please Enter the Inward Quantity";
                        }
                        bindgoodinwardcode();
                    }
                    if (greater == true)
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Visible = true;
                        lbl_alert.Text = "Order Quantity Greater than Inward Quantity";
                    }
                    // }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Please Select Anyone Staff Name";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please select anyone item";
            }
            if (saveflage == true)
            {
                popwindow.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Text = "Rejected Successfully";
            }
        }
        catch (Exception ex)
        {
            imgdiv2.Visible = true;
            lbl_alert.Visible = true;
            lbl_alert.Text = ex.ToString();
        }
    }

    protected void btn_wait1_Click(object sender, EventArgs e)
    {
    }
    protected void btn_exit1_Click(object sender, EventArgs e)
    {
        divbtns.Visible = false;
        popwindow.Visible = false;
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
                lblvalidation1.Text = "Please enter the report name ";
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
            string degreedetails = "Inward Entry Report";
            string pagename = "inward.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }
    }
    protected void Issue_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            bool che = false;
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                int chkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 1].Value);
                if (chkval == 1)
                {
                    che = true;
                }
            }
            if (che == true)
            {
                Div1.Visible = true;
                loadhostel();
                btn_transfergo_Click(sender, e);
            }
            else
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Any one Item first\");", true);
            }
        }
        catch
        {

        }
    }

    public void loadhostel()
    {
        try
        {
            ds.Clear();
            cbl_hostel1.Items.Clear();
            ds = d2.BindHostel(collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostel1.DataSource = ds;
                cbl_hostel1.DataTextField = "Hostel_Name";
                cbl_hostel1.DataValueField = "Hostel_code";
                cbl_hostel1.DataBind();

                if (cbl_hostel1.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_hostel1.Items.Count; i++)
                    {
                        cbl_hostel1.Items[i].Selected = true;
                    }

                    txt_hostel1.Text = "Hostel Name(" + cbl_hostel1.Items.Count + ")";
                }
            }
            else
            {
                txt_hostel1.Text = "--Select--";

            }
        }
        catch
        {

        }
    }
    protected void cb_hostel1_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_hostel1.Text = "--Select--";
            if (cb_hostel1.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_hostel1.Items.Count; i++)
                {
                    cbl_hostel1.Items[i].Selected = true;
                }
                txt_hostel1.Text = "vendor(" + (cbl_hostel1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_hostel1.Items.Count; i++)
                {
                    cbl_hostel1.Items[i].Selected = false;
                }
            }
        }
        catch
        {

        }
    }
    protected void cbl_hostel1_SelectIndexChange(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            cb_hostel1.Checked = false;
            int commcount = 0;
            txt_hostel1.Text = "--Select--";
            for (i = 0; i < cbl_vendor.Items.Count; i++)
            {
                if (cbl_hostel1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_hostel1.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_hostel1.Items.Count)
                {
                    cb_hostel1.Checked = true;
                }
                txt_hostel1.Text = "Vendor(" + commcount.ToString() + ")";
            }
        }
        catch
        {

        }
    }

    protected void btn_transfergo_Click(object sender, EventArgs e)
    {
        try
        {
            string items = "";
            FpSpread1.SaveChanges();
            if (FpSpread1.Sheets[0].RowCount > 0)
            {
                for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                {
                    int chkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 1].Value);
                    if (chkval == 1)
                    {
                        if (items == "")
                        {
                            items = "" + Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text) + "";

                        }
                        else
                        {
                            items = items + "'" + "," + "'" + Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text) + "";
                        }
                    }
                }
                string chkvendor = "";
                for (int i = 0; i < cbl_vendor.Items.Count; i++)
                {
                    if (cbl_vendor.Items[i].Selected == true)
                    {
                        if (chkvendor == "")
                        {
                            chkvendor = "" + cbl_vendor.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            chkvendor = chkvendor + "'" + "," + "'" + cbl_vendor.Items[i].Value.ToString() + "";
                        }
                    }
                }
                if (chkvendor.Trim() != "" && items.Trim() != "")
                {
                    string q = "select i.item_code,item_name,hand_qty  from stock_master s,item_master i where s.item_code =i.item_code and i. item_code in('" + items + "')  order by i.item_code";
                    ds = d2.select_method_wo_parameter(q, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        // spreaddiv.Visible = true;
                        FpSpread3.Sheets[0].RowCount = 0;
                        FpSpread3.Sheets[0].ColumnCount = 0;
                        FpSpread3.CommandBar.Visible = false;
                        FpSpread3.Sheets[0].AutoPostBack = true;
                        FpSpread3.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread3.Sheets[0].RowHeader.Visible = false;
                        FpSpread3.Sheets[0].ColumnCount = 6;

                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread3.Columns[0].Width = 50;

                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Item Code";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread3.Columns[1].Width = 100;

                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Name";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread3.Columns[2].Width = 200;

                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Total Stocks";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread3.Columns[3].Width = 100;

                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Quantity Transfer to hostel";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread3.Columns[4].Width = 100;

                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Hostel Name";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpread3.Columns[5].Width = 150;

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpread3.Sheets[0].RowCount++;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["item_code"]);
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["item_name"]);
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["hand_qty"]);
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Text = Convert.ToString("");
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Text = Convert.ToString("");
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        }
                        FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
                        FpSpread3.Visible = true;
                        // spreaddiv.Visible = true;
                        lbl_error.Visible = false;
                        rptprint.Visible = true;
                        div2.Visible = true;
                    }
                    else
                    {
                        lbl_error.Visible = true;
                        FpSpread3.Visible = false;
                        lbl_error.Text = "No Record Found";
                        // spreaddiv.Visible = false;
                        rptprint.Visible = false;
                        div2.Visible = false;

                    }
                }
                else
                {
                    lbl_error.Visible = true;
                    FpSpread3.Visible = false;
                    lbl_error.Text = "Please Select All Fields";
                    // spreaddiv.Visible = false;
                    rptprint.Visible = false;
                    div2.Visible = false;
                }
            }
        }
        catch
        {
        }
    }
    protected void FpSpread1_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (FpSpread1.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
            //FpSpread1.SaveChanges();
        }
        catch
        {

        }
    }

    protected void btn_transfer_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch
        {

        }
    }
    protected void btn_newadd3_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_transferqty.Text.Trim() != "")
            {
                string quantity = Convert.ToString(txt_transferqty.Text);
                if (FpSpread3.Sheets[0].RowCount > 0)
                {
                    FpSpread3.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 4].Text = Convert.ToString(quantity);
                    FpSpread3.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 5].Text = Convert.ToString(ddl_hostel3.SelectedItem.Text);
                    FpSpread3.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 5].Tag = Convert.ToString(ddl_hostel3.SelectedItem.Value);
                    popwindow3.Visible = false;
                }
            }

        }
        catch
        {

        }
    }
    protected void btn_exit3_Click(object sender, EventArgs e)
    {
        try
        {
            popwindow3.Visible = false;
        }
        catch
        {

        }
    }
    protected void btn_transferexit_Click(object sender, EventArgs e)
    {
        try
        {
            Div1.Visible = false;
        }
        catch
        {

        }
    }
    protected void btnupdate_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch
        {

        }
    }
    public void bindhostel()
    {
        try
        {
            ddl_hostel3.Items.Clear();
            ds.Clear();
            ds = d2.BindHostel(collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_hostel3.DataSource = ds;
                ddl_hostel3.DataTextField = "Hostel_Name";
                ddl_hostel3.DataValueField = "Hostel_code";
                ddl_hostel3.DataBind();
                ddl_hostel3.Items.Insert(0, "Select");
            }
        }
        catch
        {

        }
    }

    protected void imagebtnpopclose4_Click(object sender, EventArgs e)
    {
        popupsscode1.Visible = false;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();

        string query = "select s.staff_name from staffmaster s, stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and h.dept_code in ('" + dept + "') and s.staff_name like '" + prefixText + "%'";
        //string query = "select staff_name  from staffmaster where resign =0 and settled =0 and staff_name like  '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    protected void checkstaffname(object sender, EventArgs e)
    {
        string check = d2.GetFunction("select s.staff_name from staffmaster s, stafftrans st,hrdept_master h ,desig_master d where  s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1  and resign =0 and settled =0 and s.college_code =h.college_code and s.staff_name='" + txt_staff.Text + "'");
        if (check.Trim() != "0" && check.Trim() != null && check.Trim() != "")
        {
            string wardencode = d2.GetFunction("select appl_id  from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and s.staff_name='" + txt_staff.Text + "'");
            //string wardencode = d2.GetFunction("select appl_id from staffmaster s,staff_appl_master sa, stafftrans st,hrdept_master h ,desig_master d where  s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1  and resign =0 and settled =0 and s.college_code =h.college_code and sa.appl_no=s.appl_no and s.staff_name='" + txt_staff.Text + "'");
            ViewState["WardenCode"] = Convert.ToString(wardencode);
        }
        else
        {
            txt_staff.Text = "";
        }
    }
    protected void checkstaffname1(object sender, EventArgs e)
    {
        string check = d2.GetFunction("select s.staff_name from staffmaster s, stafftrans st,hrdept_master h ,desig_master d where  s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1  and resign =0 and settled =0 and s.college_code =h.college_code and s.staff_name='" + txt_upstaff.Text + "'");
        if (check.Trim() != "0" && check.Trim() != null && check.Trim() != "")
        {
            string wardencode = d2.GetFunction("select appl_id  from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and s.staff_name='" + txt_upstaff.Text + "'");
            ViewState["WardenCode"] = Convert.ToString(wardencode);
        }
        else
        {
            txt_upstaff.Text = "";
        }
    }
    protected void btn_staff_Click(object sender, EventArgs e)
    {
        try
        {
            popupsscode1.Visible = true;
            btn_save1.Visible = false;
            btn_exit2.Visible = false;
            Fpstaff.Visible = false;
            bindcollege();
            binddepartment();
            lbl_errorsearch.Visible = false;
            txt_searchby.Text = "";
            btn_go2_Click(sender, e);
        }
        catch
        {
        }
    }

    protected void bindcollege()
    {
        try
        {
            string clgname = "";
            ds.Clear();
            ddl_college.Items.Clear();
            clgname = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(clgname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void binddepartment()
    {
        ds.Clear();
        //query = "";
        //query = "select distinct dept_name,dept_code from hrdept_master where college_code='" + ddl_college2.SelectedValue.ToString() + "'";
        ds = d2.loaddepartment(ddl_college.SelectedValue.ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_department.DataSource = ds;
            ddl_department.DataTextField = "Dept_Name";
            ddl_department.DataValueField = "Dept_Code";
            ddl_department.DataBind();
            ddl_department.Items.Insert(0, "All");
            dept = ddl_department.SelectedItem.Value;
        }


    }
    protected void department_selectedindex_change(object sender, EventArgs e)
    {
        dept = ddl_department.SelectedItem.Value;
    }

    protected void btn_save1_Click(object sender, EventArgs e)
    {
        try
        {
            string name = "";
            string wardencode = "";
            string activerow = "";
            string activecol = "";
            if (Fpstaff.Sheets[0].RowCount != 0)
            {
                activerow = Fpstaff.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpstaff.ActiveSheetView.ActiveColumn.ToString();
                if (activerow != Convert.ToString(-1))
                {
                    if (Convert.ToString(ViewState["directinwardallow"]).Trim() == "1" || Convert.ToString(ViewState["directinwardallowmess"]).Trim() == "2")
                    {
                        name = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                        txt_upstaff.Text = name;
                        wardencode = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                    }
                    else
                    {
                        name = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                        txt_staff.Text = name;
                        wardencode = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                    }
                    ViewState["WardenCode"] = Convert.ToString(wardencode);
                }
                popupsscode1.Visible = false;
            }
            else
            {
                lbl_errorsearch.Visible = true;
                lbl_errorsearch.Text = "Please Select Any One Staff";
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void btn_go2_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = "";
            int rowcount;
            int rolcount = 0;
            int sno = 0;
            if (txt_searchby.Text != "")
            {
                if (ddl_searchby.SelectedIndex == 0)
                {
                    sql = "select ap.appl_id , s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,d.desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d,staff_appl_master ap where ap.appl_no=s.appl_no and s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and s.Staff_name ='" + Convert.ToString(txt_searchby.Text) + "'  order by s.staff_code";
                }
            }
            else
            {
                if (ddl_department.SelectedItem.Text == "All")
                {
                    sql = "select ap.appl_id , s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,d.desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d,staff_appl_master ap where ap.appl_no=s.appl_no and s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code  order by s.staff_code";
                }
                else
                {
                    sql = "select ap.appl_id , s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,d.desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d,staff_appl_master ap where ap.appl_no=s.appl_no and s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and h.dept_code in ('" + ddl_department.SelectedItem.Value + "')  order by s.staff_code";
                }
            }
            Fpstaff.Sheets[0].RowCount = 0;
            Fpstaff.SaveChanges();
            Fpstaff.SheetCorner.ColumnCount = 0;
            Fpstaff.CommandBar.Visible = false;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fpstaff.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            Fpstaff.Sheets[0].RowCount = Fpstaff.Sheets[0].RowCount + 1;
            Fpstaff.Sheets[0].SpanModel.Add(Fpstaff.Sheets[0].RowCount - 1, 0, 1, 3);
            Fpstaff.Sheets[0].AutoPostBack = false;
            ds = d2.select_method_wo_parameter(sql, "Text");
            Fpstaff.Sheets[0].RowCount = 0;
            Fpstaff.Sheets[0].ColumnCount = 5;

            if (ds.Tables[0].Rows.Count > 0)
            {
                Fpstaff.Visible = true;
                btn_save1.Visible = true;
                btn_exit2.Visible = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[0].Locked = true;
                Fpstaff.Columns[0].Width = 80;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[1].Locked = true;
                Fpstaff.Columns[1].Width = 100;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[2].Locked = true;
                Fpstaff.Columns[2].Width = 200;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[3].Locked = true;
                Fpstaff.Columns[3].Width = 250;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Designation";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpstaff.Columns[4].Width = 200;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[4].Locked = true;
                Fpstaff.Width = 700;

                for (rolcount = 0; rolcount < ds.Tables[0].Rows.Count; rolcount++)
                {
                    sno++;
                    //Fpstaff.Sheets[0].RowCount++;
                    //name = ds.Tables[0].Rows[rolcount]["staff_name"].ToString();
                    //code = ds.Tables[0].Rows[rolcount]["staff_code"].ToString();

                    Fpstaff.Sheets[0].RowCount = Fpstaff.Sheets[0].RowCount + 1;
                    //Fpstaff.Sheets[0].Rows[Fpstaff.Sheets[0].RowCount - 1].Font.Bold = false;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["staff_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["appl_id"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["staff_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["dept_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["dept_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["desig_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["desig_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                }
                Fpstaff.Visible = true;
                btn_save1.Visible = true;
                btn_exit2.Visible = true;
                lbl_errorsearch.Visible = true;
                err.Visible = false;
                lbl_errorsearch.Text = "No Records Found";
                lbl_errorsearch.Text = "No of Staff :" + sno.ToString();
                rowcount = Fpstaff.Sheets[0].RowCount;
                Fpstaff.Height = 370;
                Fpstaff.Sheets[0].PageSize = 25 + (rowcount * 20);
                Fpstaff.SaveChanges();

            }
            else
            {
                btn_save1.Visible = false;
                btn_exit2.Visible = false;
                Fpstaff.Visible = false;
                lbl_errorsearch.Visible = false;
                err.Visible = true;
                err.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_exit2_Click(object sender, EventArgs e)
    {
        try
        {
            popupsscode1.Visible = false;
        }
        catch
        {
        }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
        // popwindow.Visible = false;
    }
    protected void receiced_check(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        rptprint.Visible = false;
        btn_yettoreceived.Visible = false;
    }
    protected void yettoreceived(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        rptprint.Visible = false;
    }
    protected void reject(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        rptprint.Visible = false;
        btn_yettoreceived.Visible = false;
    }
    protected void rdb_dirstore_Click(object sender, EventArgs e)
    {
        if (cb_direct.Checked == true)
        {
            MultiView1.ActiveViewIndex = 0;
        }
        FpSpread1.Visible = false;
        rptprint.Visible = false;
        lbl_itm.Text = "Items";
        vendorname();
        ords();
    }
    protected void rdb_dirmess_Check(object sender, EventArgs e)
    {
        if (cb_direct.Checked == true)
        {
            MultiView1.ActiveViewIndex = 1;
        }
        FpSpread1.Visible = false;
        rptprint.Visible = false;
        lbl_itm.Text = "Items";
        vendorname();
        ords();
    }
    protected void rdb_dirdept_Click(object sender, EventArgs e)
    {
        if (cb_direct.Checked == true)
        {
            MultiView1.ActiveViewIndex = 2;
        }
        FpSpread1.Visible = false;
        rptprint.Visible = false;
        lbl_itm.Text = "Items";
        vendorname();
        ords();
    }

    //Added by Saranyadevi25.1.2018
    protected void rdb_dirbook_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_itm.Text = "Books";
            vendorname();
            ords();
        }
        catch
        {

        }


    }


    public void Librarytrue()
    {
        lbl_lib.Visible = true;
        ddl_Libname.Visible = true;
    }
    public void Libraryfalse()
    {
        lbl_lib.Visible = false;
        ddl_Libname.Visible = false;
    }

    //24.03.16
    protected void FpSpread4Cell_Click(object sender, EventArgs e)
    {
        try
        {
            check = true;
        }
        catch
        { }
    }
    protected void Fpspread4_render(object sender, EventArgs e)
    {
        try
        {
            if (check == true)
            {
                directinwardpop.Visible = true; btn_directpo.Text = "Save";
                string activerow = FpSpread4.ActiveSheetView.ActiveRow.ToString();
                string activecol = FpSpread4.ActiveSheetView.ActiveColumn.ToString();
                // txt_date.Text = Convert.ToString(DateTime.Now.ToString("dd/MM/yyyy"));
                if (activerow.Trim() != "" && activecol.Trim() != "")
                {



                }
            }
        }
        catch { }
    }
    protected void btnpop1exit_click(object sender, EventArgs e)
    {
        directinwardpop.Visible = false;
    }
    protected void btn_directpo_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToString(ViewState["directinwardallow"]) == "")
            {
                string activerow = FpSpread4.ActiveSheetView.ActiveRow.ToString();
                string activecol = FpSpread4.ActiveSheetView.ActiveColumn.ToString();
                string itemFK = "";
                if (activerow.Trim() != "" && activecol.Trim() != "")
                {
                    if (FpSpread4.Rows.Count > 0)
                    {
                        itemFK = Convert.ToString(FpSpread4.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Tag);
                    }
                }
                FpSpread4.Sheets[0].Cells[Convert.ToInt32(activerow), 8].Text = Convert.ToString(txt_sailingprice.Text);
                FpSpread4.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Text = Convert.ToString(txtpop1rateunit.Text);
                FpSpread4.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Text = Convert.ToString(txtpop1qnty.Text);
                FpSpread4.Sheets[0].Cells[Convert.ToInt32(activerow), 10].Text = Convert.ToString(txtpop1dia.Text);
                FpSpread4.Sheets[0].Cells[Convert.ToInt32(activerow), 11].Text = Convert.ToString(txtpop1tax.Text);
                FpSpread4.Sheets[0].Cells[Convert.ToInt32(activerow), 12].Text = Convert.ToString(txtpop1exetax.Text);
                FpSpread4.Sheets[0].Cells[Convert.ToInt32(activerow), 13].Text = Convert.ToString(txtpop1educess.Text);
                FpSpread4.Sheets[0].Cells[Convert.ToInt32(activerow), 14].Text = Convert.ToString(txtpop1higher.Text);
                FpSpread4.Sheets[0].Cells[Convert.ToInt32(activerow), 15].Text = Convert.ToString(txtpop1otherchar.Text);
                FpSpread4.Sheets[0].Cells[Convert.ToInt32(activerow), 16].Text = Convert.ToString(txtpop1des.Text);
                FpSpread4.Sheets[0].Cells[Convert.ToInt32(activerow), 17].Text = Convert.ToString(txtpop1totalcost.Text);
                //FpSpread4.Sheets[0].Cells[Convert.ToInt32(activerow), 18].Text = Convert.ToString(txt_billno.Text);

                pohashtable.Remove(itemFK);

                pohashtable.Add(itemFK, Convert.ToString(txtpop1qnty.Text) + '-' + Convert.ToString(txtpop1rateunit.Text) + '-' + Convert.ToString(txtpop1dia.Text) + '-' + Convert.ToString(txtpop1tax.Text) + '-' + Convert.ToString(txtpop1totalcost.Text) + '-' + Convert.ToString(txtpop1exetax.Text) + '-' + Convert.ToString(txtpop1educess.Text) + '-' + Convert.ToString(txtpop1higher.Text) + '-' + Convert.ToString(txtpop1otherchar.Text) + '-' + Convert.ToString(txtpop1des.Text) + '-' + Convert.ToString(txt_date.Text) + '-' + Convert.ToString(txt_sailingprice.Text));
                Clear();
                directinwardpop.Visible = false;
            }
            else
            {
                if (Convert.ToString(ViewState["directinwardupgoodFk"]).Trim() != "" && Convert.ToString(ViewState["directinwarduppurchasepk"]).Trim() != "" && Convert.ToString(ViewState["directinwarditemfk"]).Trim() != "")
                {
                    string discountamt = ""; string discountper = "";
                    if (cbdis.Checked == true)
                    {
                        discountamt = Convert.ToString(txtpop1dia.Text);
                    }
                    else
                    {
                        discountper = Convert.ToString(txtpop1dia.Text);
                    }
                    string tax = Convert.ToString(txtpop1tax.Text);
                    string extax = Convert.ToString(txtpop1exetax.Text);
                    string educess = Convert.ToString(txtpop1educess.Text);
                    string eduhigher = Convert.ToString(txtpop1higher.Text);
                    string otherchar = Convert.ToString(txtpop1otherchar.Text);
                    string decription = Convert.ToString(txtpop1des.Text);
                    string sailingprice = Convert.ToString(txt_sailingprice.Text);

                    string invqty = Convert.ToString(txtpop1qnty.Text);
                    string invrpu = Convert.ToString(txtpop1rateunit.Text);

                    if (sailingprice.Trim() == "")
                    {
                        sailingprice = "0";
                    }

                    if (invqty.Trim() == "")
                    {
                        invqty = "0";
                    }
                    if (invrpu.Trim() == "")
                    {
                        invrpu = "0";
                    }

                    if (discountamt.Trim() == "")
                    {
                        discountamt = "0";
                    }
                    if (discountper == "")
                    {
                        discountper = "0";
                    }
                    if (tax.Trim() == "")
                    {
                        tax = "0";
                    }
                    if (extax.Trim() == "")
                    {
                        extax = "0";
                    }
                    if (educess.Trim() == "")
                    {
                        educess = "0";
                    }
                    if (eduhigher.Trim() == "")
                    {
                        eduhigher = "0";
                    }
                    if (otherchar.Trim() == "")
                    {
                        otherchar = "0";
                    }
                    if (decription.Trim() == "")
                    {
                        decription = "";
                    }

                    string[] splitdate = Convert.ToString(txt_upbilldate.Text).Split('/');
                    DateTime dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
                    string indatee = dt.ToString("MM/dd/yyyy");


                    if (txt_upstaff.Text.Trim() != "")
                    {
                        string q1 = " update IT_PurchaseOrderDetail set Qty='" + invqty + "',RPU='" + invrpu + "',IsDiscountPercent='" + discountper + "', DiscountAmt='" + discountamt + "',TaxPercent='" + tax + "',ExeciseTaxPer='" + extax + "',EduCessPer='" + educess + "',HigherEduCessPer='" + eduhigher + "',OtherChargeAmt='" + otherchar + "',OtherChargeDesc='" + decription + "',PurchaseOrderFK='" + Convert.ToString(ViewState["directinwarduppurchasepk"]).Trim() + "',Sailing_prize ='" + sailingprice + "' where ItemFK='" + Convert.ToString(ViewState["directinwarditemfk"]).Trim() + "' and PurchaseOrderFK='" + Convert.ToString(ViewState["directinwarduppurchasepk"]).Trim() + "' ";

                        q1 += " update IT_GoodsInward set InwardQty ='" + invqty + "',Received_staffcode='" + Convert.ToString(ViewState["WardenCode"]) + "',InvoiceNo='" + Convert.ToString(txt_upbillno.Text.Trim()) + "',InvoiceDate ='" + indatee + "'  where  itemfk='" + Convert.ToString(ViewState["directinwarditemfk"]).Trim() + "'  and GoodsInwardPK='" + Convert.ToString(ViewState["directinwardupgoodFk"]).Trim() + "'";

                        if (rdb_dirstore.Checked == true)
                        {
                            q1 += " update IT_StockDetail set InwardQty='" + invqty + "' , InwardRPU='" + invrpu + "' where InwardFK ='" + Convert.ToString(ViewState["directinwardupgoodFk"]).Trim() + "' and ItemFK ='" + Convert.ToString(ViewState["directinwarditemfk"]).Trim() + "'  and InwardType ='2'";//and StoreFK ='" + sfk + "'
                        }
                        if (rdb_dirmess.Checked == true || rdb_dirdept.Checked == true)
                        {
                            string deptfk = Convert.ToString(ViewState["directinwarddeptFK"]);
                            string itfk = Convert.ToString(ViewState["directinwarditemfk"]).Trim();
                            string rpu = d2.GetFunction("select IssuedRPU from IT_StockDeptDetail where ItemFK ='" + Convert.ToString(ViewState["directinwarditemfk"]).Trim() + "' and DeptFK ='" + Convert.ToString(ViewState["directinwarddeptFK"]) + "' and IssuedQty<>isnull(UsedQty,0) ");//" + deptfk + "
                            //double avrrpu = 0;//13.02.18 barath
                            //if (rpu.Trim() != "0")
                            //{
                            //    if (rpu != invrpu)
                            //    {
                            //        invrpu = Convert.ToString(Convert.ToDouble(rpu) + Convert.ToDouble(invrpu));
                            //        double.TryParse(invrpu, out avrrpu);
                            //        invrpu = Convert.ToString(avrrpu / 2);
                            //    }
                            //}

                            string[] split3 = Convert.ToString(txt_upbilldate.Text).Split('/');
                            DateTime invdate = Convert.ToDateTime(split3[1] + "/" + split3[0] + "/" + split3[2]);
                            invdate.ToString("MM/dd/yyyy");
                            // DateTime invdate = Convert.ToDateTime(txt_upbilldate.Text);
                            ds.Clear(); string transferpk = ""; double transoldqty = 0;
                            string q2 = "select TransferItemPK,TransferQty  from IT_TransferItem where TrasnferDate='" + invdate.ToString("MM/dd/yyyy") + "' and TransferType='1' and ItemFK='" + itfk + "' and TrasferTo='" + deptfk + "'";
                            ds = d2.select_method_wo_parameter(q2, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                transferpk = Convert.ToString(ds.Tables[0].Rows[0]["TransferItemPK"]);
                                double.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["TransferQty"]), out transoldqty);
                            }
                            double val = 0;
                            val = Convert.ToDouble(invqty) - transoldqty;
                            q1 += "if exists (select * from IT_StockDeptDetail where ItemFK ='" + itfk + "' and DeptFK ='" + deptfk + "' ) update IT_StockDeptDetail set IssuedQty=IssuedQty+'" + val + "',balQty=isnull(balqty,0)+'" + val + "' ,IssuedRPU='" + invrpu + "' where ItemFK ='" + itfk + "' and DeptFK ='" + deptfk + "'";
                            q1 += " update IT_TransferItem set TransferQty='" + invqty + "',TransferRpu='" + invrpu + "' where TransferItemPK='" + transferpk + "'";
                        }

                        Clear();
                        int up = d2.update_method_wo_parameter(q1, "text");
                        if (up != 0)
                        {
                            imgdiv2.Visible = true;
                            lbl_alert.Visible = true;
                            lbl_alert.Text = "Saved Successfully";
                            directinwardpop.Visible = false;
                            btn_go_Click(sender, e);
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Visible = true;
                        lbl_alert.Text = "Please Enter StaffName";
                    }
                }
            }
        }
        catch { }
    }
    public void bindordercode()
    {
        try
        {
            string newitemcode = "";
            string selectquery = "select POAcr,POSize,POStNo from IM_CodeSettings order by StartDate desc";
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string itemacronym = Convert.ToString(ds.Tables[0].Rows[0]["POAcr"]);
                string itemstarno = Convert.ToString(ds.Tables[0].Rows[0]["POStNo"]);
                string itemsize = Convert.ToString(ds.Tables[0].Rows[0]["POSize"]);
                selectquery = "select distinct top(1) ordercode from IT_PurchaseOrder where OrderCode like '" + Convert.ToString(itemacronym) + "%' order by OrderCode desc";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string itemcode = Convert.ToString(ds.Tables[0].Rows[0]["ordercode"]);
                    string itemacr = Convert.ToString(itemacronym);
                    int len = itemacr.Length;
                    itemcode = itemcode.Remove(0, len);
                    int len1 = Convert.ToString(itemcode).Length;
                    string newnumber = Convert.ToString((Convert.ToInt32(itemcode) + 1));
                    len = Convert.ToString(newnumber).Length;
                    len1 = Convert.ToInt32(itemsize) - len;
                    if (len1 == 2)
                    {
                        newitemcode = "00" + newnumber;
                    }
                    else if (len1 == 1)
                    {
                        newitemcode = "0" + newnumber;
                    }
                    else if (len1 == 4)
                    {
                        newitemcode = "0000" + newnumber;
                    }
                    else if (len1 == 3)
                    {
                        newitemcode = "000" + newnumber;
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
                    else if (size == 4)
                    {
                        newitemcode = "0000" + itemstarno;
                    }
                    else if (size == 3)
                    {
                        newitemcode = "000" + itemstarno;
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
                ViewState["ordercode"] = Convert.ToString(newitemcode);
            }
        }
        catch
        {

        }
    }
    protected void Clear()
    {
        txtpop1des.Text = "";
        txtpop1dia.Text = "";
        txtpop1educess.Text = "";
        txtpop1exetax.Text = "";
        txtpop1higher.Text = "";
        txtpop1otherchar.Text = "";
        txtpop1qnty.Text = "";
        txtpop1rateunit.Text = "";
        txtpop1tax.Text = "";
        txtpop1totalcost.Text = "";
        //txt_billno.Text = "";
    }
    public void Fpreadheaderbindmethod(string headername, FarPoint.Web.Spread.FpSpread spreadname, string AutoPostBack)
    {
        try
        {
            string[] header = headername.Split('/');
            int k = 0;
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
            imgdiv2.Visible = true;
            lbl_alert.Visible = true;
            lbl_alert.Font.Size = FontUnit.Smaller;
            lbl_alert.Text = ex.ToString();
        }
    }

    protected void FpSpread1Cell_Click(object sender, EventArgs e)
    {
        try
        {
            check1 = true;
        }
        catch { }
    }
    protected void Fpspread1_render(object sender, EventArgs e)//delsi
    {
        if (check1 == true)
        {
            string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
            if (activerow.Trim() != "" && activecol.Trim() != "")
            {
                string transferqty = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                if (transferqty.Trim() == "0.00")
                {
                    ViewState["directinwardallow"] = "1";
                    lbl_upbillno.Visible = true;
                    txt_upbillno.Visible = true;
                    lbl_upbilldate.Visible = true;
                    txt_upbilldate.Visible = true;
                    lbl_upstaff.Visible = true;
                    txt_upstaff.Visible = true;
                    btnupQ1.Visible = true;
                    txt_date.Text = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    txt_sailingprice.Text = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 8].Text);
                    txtpop1rateunit.Text = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Text);
                    txtpop1qnty.Text = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Text);
                    txtpop1totalcost.Text = Convert.ToString(Convert.ToDouble(txtpop1rateunit.Text) * Convert.ToDouble(txtpop1qnty.Text));
                    txt_upstaff.Text = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 11].Text);
                    if (txt_upstaff.Text.Trim() != "")
                    {
                        ViewState["WardenCode"] = d2.GetFunction("select appl_id  from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and s.staff_name='" + txt_upstaff.Text + "'");
                    }
                    txt_upbillno.Text = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 12].Text);
                    string[] split = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 12].Tag).Split('/');
                    DateTime dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                    txt_upbilldate.Text = dt.ToString("dd/MM/yyyy");
                    directinwardpop.Visible = true;
                    btn_directpo.Text = "Update";
                    ViewState["directinwardupgoodFk"] = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Note);
                    ViewState["directinwarduppurchasepk"] = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                    ViewState["directinwarditemfk"] = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Note);
                    return;
                }
                if (transferqty.Trim() == "")
                {
                    ViewState["directinwardallowmess"] = "2"; ViewState["directinwardallow"] = "1";
                    lbl_upbillno.Visible = true;
                    txt_upbillno.Visible = true;
                    lbl_upbilldate.Visible = true;
                    txt_upbilldate.Visible = true;
                    lbl_upstaff.Visible = true;
                    txt_upstaff.Visible = true;
                    btnupQ1.Visible = true;
                    txt_date.Text = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    txt_sailingprice.Text = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 8].Text);
                    txtpop1rateunit.Text = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Text);
                    txtpop1qnty.Text = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Text);
                    txtpop1totalcost.Text = Convert.ToString(Convert.ToDouble(txtpop1rateunit.Text) * Convert.ToDouble(txtpop1qnty.Text));
                    txt_upstaff.Text = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 11].Text);
                    if (txt_upstaff.Text.Trim() != "")
                    {
                        ViewState["WardenCode"] = d2.GetFunction("select appl_id  from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and s.staff_name='" + txt_upstaff.Text + "'");
                    }
                    txt_upbillno.Text = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 12].Text);

                    //string[] split = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 12].Tag).Split('/');
                    //DateTime dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                    //txt_upbilldate.Text = dt.ToString("dd/MM/yyyy");//delsi

                    txt_upbilldate.Text = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 12].Tag);


                    directinwardpop.Visible = true;
                    btn_directpo.Text = "Update";
                    ViewState["directinwardupgoodFk"] = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Note);
                    ViewState["directinwarduppurchasepk"] = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                    ViewState["directinwarditemfk"] = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Note);
                    ViewState["directinwarddeptFK"] = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 10].Tag);
                    return;
                }
                else
                {
                    imgdiv2.Visible = true; lbl_alert.Visible = true;
                    lbl_alert.Text = " Inward Item is transfered So can't update";
                    ViewState["directinwardallow"] = "";
                    ViewState["directinwardallowmess"] = "";
                }
            }
        }
    }

    protected void cb_dirstore_CheckedChange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_dirstorebase, cbl_dirstorebase, txt_dirstorebase, "Store Name", "--Select--");
    }
    protected void cbl_dirstore_SelectedIndexChange(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_dirstorebase, cbl_dirstorebase, txt_dirstorebase, "Store Name");
    }
    protected void cb_dirmessbase_CheckedChange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_dirmessbase, cbl_dirmessbase, txt_dirmessbase, "Mess Name", "--Select--");
    }
    protected void cbl_dirmessbase_SelectedIndexChange(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_dirmessbase, cbl_dirmessbase, txt_dirmessbase, "Mess Name");
    }
    protected void cb_dirdepbase_CheckedChange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_dirdepbase, cbl_dirdepbase, txt_dirdepartbase, "Department", "--Select--");
    }
    protected void cbl_dirdepbase_SelectedIndexChange(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_dirdepbase, cbl_dirdepbase, txt_dirdepartbase, "Department");
    }

    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst)
    {
        try
        {
            int sel = 0;
            int count = 0;
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = dipst + "(" + count + ")";
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
            else
            {
                txt.Text = "--Select--";
            }
        }
        catch { }
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
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
    protected string returnwithsinglecodevalue(CheckBoxList cb)
    {
        string empty = "";
        for (int i = 0; i < cb.Items.Count; i++)
        {
            if (cb.Items[i].Selected == true)
            {
                if (empty == "")
                {
                    empty = Convert.ToString(cb.Items[i].Value);
                }
                else
                {
                    empty = empty + "','" + Convert.ToString(cb.Items[i].Value);
                }
            }
        }
        return empty;
    }



    //Library

    public void Libvendor()
    {
        ddl_vendor1.Items.Clear();
        ds.Clear();
        string sqlqry = "select vendorcompname,VendorPK from CO_VendorMaster where vendortype=1 and libraryflag='1' order by VendorCompName";
        ds = d2.select_method_wo_parameter(sqlqry, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_vendor1.DataSource = ds;
            ddl_vendor1.DataTextField = "vendorcompname";
            ddl_vendor1.DataValueField = "VendorPK";
            ddl_vendor1.DataBind();

        }
    }


    protected void bind_poplib()
    {
        try
        {
            ds.Clear();
            string q = "select distinct lib_name,lib_code from library where college_code ='" + collegecode1 + "' order by lib_name";
            ds = d2.select_method_wo_parameter(q, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_Libname.DataSource = ds;
                ddl_Libname.DataTextField = "lib_name";
                ddl_Libname.DataValueField = "lib_code";
                ddl_Libname.DataBind();

            }
        }
        catch { }
    }

    public string Auto_AccessNo(string librarycode)
    {
        string RetAccno = "";
        try
        {
            string codeno = string.Empty;
            string codeno1 = string.Empty;
            string libcodeval = Convert.ToString(librarycode);



            DataSet dsAutoAccess = new DataSet();
            DataSet dsBack = new DataSet();

            string sql = "SELECT ISNULL(AutoAccessNo,0) AutoAccessNo,ISNULL(gen_acr,'') gen_acr,ISNULL(gen_stno,1) gen_stno FROM Library Where Lib_Code ='" + libcodeval + "'";
            dsAutoAccess = d2.select_method_wo_parameter(sql, "text");
            if (dsAutoAccess.Tables[0].Rows.Count > 0)
            {
                string book = Convert.ToString(dsAutoAccess.Tables[0].Rows[0]["AutoAccessNo"]);
                if (book == "1" || book == "0")
                {

                    sql = "SELECT * FROM bookdetails WHERE Lib_Code ='" + libcodeval + "' ORDER BY LEN(acc_no),acc_no";
                    dsBack.Clear();
                    dsBack = d2.select_method_wo_parameter(sql, "text");
                    if (dsBack.Tables[0].Rows.Count > 0)
                    {
                        codeno = Convert.ToString(dsBack.Tables[0].Rows[dsBack.Tables[0].Rows.Count - 1]["acc_no"]);
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
                        codeno1 = Convert.ToString(dsAutoAccess.Tables[0].Rows[0]["gen_acr"]) + jj;
                        RetAccno = codeno1 + "#" + Convert.ToString(dsAutoAccess.Tables[0].Rows[0]["gen_acr"]);
                    }
                    else
                    {
                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["gen_acr"]) + Convert.ToString(ds.Tables[0].Rows[0]["gen_stno"]);
                        RetAccno = codeno1 + "#" + Convert.ToString(ds.Tables[0].Rows[0]["gen_acr"]);
                    }
                }

            }

        }
        catch (Exception)
        {
        }
        return RetAccno;
    }

    protected void RadioButton_Inv_Checeked(object sender, EventArgs e)
    {

        lbl_itm.Text = "Items";
        vendorname();
        ords();
        rdb_dirbook.Checked = false;
        rdb_dirdept.Checked = false;
        rdb_dirmess.Checked = false;
        rdb_dirstore.Checked = true;

    }

    protected void RadioButton_lib_Checeked(object sender, EventArgs e)
    {

        lbl_itm.Text = "Books";
        vendorname();
        ords();
        rdb_dirbook.Checked = true;
        rdb_dirdept.Checked = false;
        rdb_dirmess.Checked = false;
        rdb_dirstore.Checked = false;
    }
}
/*
 19.10.16 changes of nec 
 01.11.16 changes of nec 17.06
 */