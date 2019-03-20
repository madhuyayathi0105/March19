using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.Drawing;
using System.IO;
using Gios.Pdf;
using System.Text.RegularExpressions;


public partial class inv_purchaseorder_request : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string staffcode = string.Empty;
    string app_id = string.Empty;
    bool check = false;
    DataSet ds = new DataSet();
    DataTable d2 = new DataTable();
    DataSet ds1 = new DataSet();
    DAccess2 da = new DAccess2();
    DataSet ds2 = new DataSet();
    int i;
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
            staffcode = Session["Staff_Code"].ToString();
            app_id = da.GetFunction("select sa.appl_id from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and s.staff_code='" + staffcode + "'");

            CalendarExtender2.EndDate = DateTime.Now;
            CalendarExtender1.EndDate = DateTime.Now;
            if (!IsPostBack)
            {
                vendor();
                ordercode();
                item();
                txt_fromdate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.Visible = false;
                Fpspread3.Sheets[0].RowCount = 0;
                Fpspread3.Sheets[0].ColumnCount = 0;
                Fpspread3.Visible = false;

                rdb_request.Checked = true;
                rdb_request_CheckedChanged(sender, e);
                btn_go_Click(sender, e);
            }
        }
        catch (Exception ex) { }
    }
    protected void lnk_logout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }
    public void vendor()
    {
        cbl_vendor.Items.Clear();
        ds.Clear();
        ds = da.BindVendorNamevendorpk_inv();
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_vendor.DataSource = ds;
            cbl_vendor.DataTextField = "vendorcompname";
            cbl_vendor.DataValueField = "VendorPK";
            cbl_vendor.DataBind();

            if (cbl_vendor.Items.Count > 0)
            {
                for (i = 0; i < cbl_vendor.Items.Count; i++)
                {

                    cbl_vendor.Items[i].Selected = true;
                }

                txt_vendorname.Text = "Vendor(" + cbl_vendor.Items.Count + ")";
            }
        }
        else
        {
            txt_vendorname.Text = "--Select--";
        }
    }
    protected void cb_vendor_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;
        txt_vendorname.Text = "---Select---";

        if (cb_vendor.Checked == true)
        {
            cout++;
            for (i = 0; i < cbl_vendor.Items.Count; i++)
            {
                cbl_vendor.Items[i].Selected = true;
            }
            txt_vendorname.Text = "vendor(" + (cbl_vendor.Items.Count) + ")";
        }
        else
        {
            for (i = 0; i < cbl_vendor.Items.Count; i++)
            {
                cbl_vendor.Items[i].Selected = false;
            }
        }
        ordercode();
        item();

    }

    protected void cb_oc_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;
        txtordercode.Text = "---Select---";

        if (cb_oc.Checked == true)
        {
            cout++;
            for (i = 0; i < cbl_oc.Items.Count; i++)
            {
                cbl_oc.Items[i].Selected = true;
            }
            txtordercode.Text = "Order Code(" + (cbl_oc.Items.Count) + ")";
        }
        else
        {
            for (i = 0; i < cbl_oc.Items.Count; i++)
            {
                cbl_oc.Items[i].Selected = false;
            }
        }
        item();
    }
    protected void cbl_vendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        i = 0;
        cb_vendor.Checked = false;

        int commcount = 0;
        txt_vendorname.Text = "--Select--";
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
            txt_vendorname.Text = "Vendor(" + commcount.ToString() + ")";
        }
        ordercode();
        item();
    }

    protected void cbl_oc_SelectedIndexChanged(object sender, EventArgs e)
    {
        i = 0;
        cb_oc.Checked = false;

        int commcount = 0;
        txtordercode.Text = "--Select--";
        for (i = 0; i < cbl_oc.Items.Count; i++)
        {
            if (cbl_oc.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_oc.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_oc.Items.Count)
            {
                cb_oc.Checked = true;
            }
            txtordercode.Text = "Order Code(" + commcount.ToString() + ")";
        }
        item();
    }

    public void item()
    {
        cbl_item.Items.Clear();
        string deptquery = "";
        string buildvalue = "";
        string oc = "";

        for (i = 0; i < cbl_vendor.Items.Count; i++)
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

        for (i = 0; i < cbl_oc.Items.Count; i++)
        {
            if (cbl_oc.Items[i].Selected == true)
            {
                string build = cbl_oc.Items[i].Text.ToString();
                if (oc == "")
                {
                    oc = build;
                }
                else
                {
                    oc = oc + "'" + "," + "'" + build;
                }
            }
        }

        deptquery = "select distinct i.ItemName,i.ItemCode,i.ItemPK from IM_ItemMaster i,IM_VendorItemDept vd,IT_PurchaseOrder p,IT_PurchaseOrderDetail pd where i.ItemPK=vd.ItemFK and vd.ItemFK =pd.ItemFK and p.PurchaseOrderPK=pd.PurchaseOrderFK and p.VendorFK=vd.VenItemFK and pd.ItemFK=vd.ItemFK and vd.VenItemFK in('" + buildvalue + "') and p.OrderCode in('" + oc + "')";
        ds = da.select_method_wo_parameter(deptquery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_item.DataSource = ds;
            cbl_item.DataTextField = "ItemName";
            cbl_item.DataValueField = "ItemPK";
            cbl_item.DataBind();

            if (cbl_item.Items.Count > 0)
            {
                for (int i = 0; i < cbl_item.Items.Count; i++)
                {

                    cbl_item.Items[i].Selected = true;
                }

                txt_item.Text = "Items(" + cbl_item.Items.Count + ")";
            }
        }
        else
        {
            txt_item.Text = "--Select--";
        }
    }

    public void ordercode()
    {
        cbl_oc.Items.Clear();
        string deptquery = "";
        string buildvalue = "";

        for (i = 0; i < cbl_vendor.Items.Count; i++)
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
        deptquery = "select OrderCode,PurchaseOrderPK from IT_PurchaseOrder where VendorFK in('" + buildvalue + "') order by OrderCode ";
        ds.Clear();
        ds = da.select_method_wo_parameter(deptquery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_oc.DataSource = ds;
            cbl_oc.DataTextField = "OrderCode";
            cbl_oc.DataValueField = "PurchaseOrderPK";
            cbl_oc.DataBind();

            if (cbl_oc.Items.Count > 0)
            {
                for (int i = 0; i < cbl_oc.Items.Count; i++)
                {

                    cbl_oc.Items[i].Selected = true;
                }

                txtordercode.Text = "Order Code(" + cbl_oc.Items.Count + ")";
            }
        }
        else
        {
            txtordercode.Text = "--Select--";
        }
    }
    protected void cb_item_CheckedChange(object sender, EventArgs e)
    {

        int cout = 0;
        txt_item.Text = "---Select---";
        if (cb_item.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_item.Items.Count; i++)
            {
                cbl_item.Items[i].Selected = true;
            }
            txt_item.Text = "Items(" + (cbl_item.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_item.Items.Count; i++)
            {
                cbl_item.Items[i].Selected = false;
            }
        }
    }
    protected void cbl_item_SelectedIndexChange(object sender, EventArgs e)
    {
        int i = 0;
        cb_item.Checked = false;
        int commcount = 0;
        txt_item.Text = "--Select--";
        for (i = 0; i < cbl_item.Items.Count; i++)
        {
            if (cbl_item.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_item.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_item.Items.Count)
            {
                cb_item.Checked = true;
            }
            txt_item.Text = "Item(" + commcount.ToString() + ")";
        }
    }
    protected void txt_fromdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_fromdate.Text != "" && txt_todate.Text != "")
            {

                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate.Text);
                string seconddate = Convert.ToString(txt_todate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;

                if (dt > dt1)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Enter Fromdate less than or equal to the Todate";
                    txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                else
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void txt_todate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_fromdate.Text != "" && txt_todate.Text != "")
            {
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate.Text);
                string seconddate = Convert.ToString(txt_todate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;
                if (dt > dt1)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Enter Todate greater than or equal to the Fromdate ";
                    txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                else
                {

                }

            }
        }
        catch (Exception ex)
        {
        }
    }
    //check box events
    protected void reject_CheckedChanged(object sender, EventArgs e)
    {
        lbl_error.Visible = false;
        //lbl_error.Text = "No Record Founds";
        FpSpread1.Visible = false;
    }
    protected void approval_CheckedChanged(object sender, EventArgs e)
    {
        lbl_error.Visible = false;
        //lbl_error.Text = "No Record Founds";
        FpSpread1.Visible = false;
    }
    protected void rdb_wait_CheckedChanged(object sender, EventArgs e)
    {
        lbl_error.Visible = false;
        //lbl_error.Text = "No Record Founds";
        FpSpread1.Visible = false;
    }
    protected void rdb_request_CheckedChanged(object sender, EventArgs e)
    {
        lbl_error.Visible = false;
        //lbl_error.Text = "No Record Founds";
        FpSpread1.Visible = false;
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    //btn events
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string firstdate = Convert.ToString(txt_fromdate.Text);
            string seconddate = Convert.ToString(txt_todate.Text);
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

            string q1 = "";
            string q2 = "";
            string library = "";
            FpSpread1.Width = 870;
            if (dt <= dt1)
            {
                if (Rad_item.Checked)
                    library = "and Isnull(Vm.LibraryFlag,'')=''";
                if (Rad_book.Checked)
                    library = "and Vm.LibraryFlag='1'";
                if (rdb_request.Checked == true)
                {
                    q1 = "select distinct count(*) as noofitems, CONVERT(varchar(10),OrderDate,103)as orderdate,p.OrderCode,vm.VendorCompName,case when ApproveStatus=1 then 'Approval' when ApproveStatus=2 then 'Reject' else 'Waiting' end as ApproveStatus,p.Reqstaff_appno from IT_PurchaseOrderDetail pd,IT_PurchaseOrder p,CO_VendorMaster vm where p.PurchaseOrderPK=pd.PurchaseOrderFK and vm.VendorPK=p.VendorFK and p.OrderDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ISNULL(ApproveStatus,'')=0 and p.Reqstaff_appno='" + app_id + "' " + library + " group by PurchaseOrderFK,orderdate,OrderCode, VendorCompName,ApproveStatus,Reqstaff_appno";

                    q2 = "  select  distinct CONVERT(varchar(10),OrderDate,103)as orderdate,p.OrderCode,pd.PurchaseOrderFK, pd.ItemFK ,vm.VendorCompName, case when ApproveStatus=1 then 'Approval' when ApproveStatus=2 then 'Reject' else 'Waiting' end as ApproveStatus from IT_PurchaseOrderDetail pd,IT_PurchaseOrder p,CO_VendorMaster vm where p.PurchaseOrderPK=pd.PurchaseOrderFK and vm.VendorPK=p.VendorFK and p.OrderDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ISNULL(ApproveStatus,'')=0 and p.Reqstaff_appno='" + app_id + "'";

                    btn_poprint.Visible = false;
                }
                else if (rdb_wait.Checked == true)
                {

                    q1 = "select distinct count(*) as noofitems, CONVERT(varchar(10),OrderDate,103)as orderdate,p.OrderCode,vm.VendorCompName,case when ApproveStatus=1 then 'Approval' when ApproveStatus=2 then 'Reject' else 'Waiting' end as ApproveStatus,rh.ReqAppStaffAppNo from IT_PurchaseOrderDetail pd,IT_PurchaseOrder p,CO_VendorMaster vm,RQ_RequestHierarchy rh where p.PurchaseOrderPK=pd.PurchaseOrderFK and vm.VendorPK=p.VendorFK and p.OrderDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ISNULL(ApproveStatus,'')=0 and rh.ReqStaffAppNo=p.Reqstaff_appno and rh.ReqAppStaffAppNo ='" + app_id + "' " + library + " group by PurchaseOrderFK,orderdate,OrderCode, VendorCompName,ApproveStatus,ReqAppStaffAppNo";


                    q2 = " select  distinct CONVERT(varchar(10),OrderDate,103)as orderdate,p.OrderCode,pd.PurchaseOrderFK, pd.ItemFK ,vm.VendorCompName, case when ApproveStatus=1  then 'Approval' when ApproveStatus=2 then 'Reject' else 'Waiting' end as ApproveStatus from IT_PurchaseOrderDetail pd,IT_PurchaseOrder p,CO_VendorMaster vm,RQ_RequestHierarchy rh where p.PurchaseOrderPK=pd.PurchaseOrderFK and vm.VendorPK=p.VendorFK and p.OrderDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ISNULL(ApproveStatus,'')=0 and rh.ReqStaffAppNo=p.Reqstaff_appno and rh.ReqAppStaffAppNo='" + app_id + "'";
                    btn_poprint.Visible = false;
                    FpSpread1.Width = 967;
                }
                else if (rdb_approval.Checked == true)
                {
                    q1 = "select distinct count(*) as noofitems,CONVERT(varchar(10),OrderDate,103)as orderdate,p.OrderCode,vm.VendorCompName,case when ApproveStatus=1 then 'Approval' when ApproveStatus=2 then 'Reject' end as ApproveStatus,p.Reqstaff_appno from IT_PurchaseOrderDetail pd,IT_PurchaseOrder p,CO_VendorMaster vm,RQ_RequestHierarchy rh where p.PurchaseOrderPK=pd.PurchaseOrderFK and vm.VendorPK=p.VendorFK and p.OrderDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ApproveStatus=1 and rh.ReqAppStaffAppNo='" + app_id + "' and rh.ReqStaffAppNo=p.Reqstaff_appno " + library + "  group by PurchaseOrderFK,orderdate, OrderCode,VendorCompName,ApproveStatus,Reqstaff_appno";

                    q2 = " select  distinct CONVERT(varchar(10),OrderDate,103)as orderdate,p.OrderCode,pd.PurchaseOrderFK, pd.ItemFK ,vm.VendorCompName,case when ApproveStatus=1 then 'Approval' when ApproveStatus=2 then 'Reject' else 'Waiting' end as ApproveStatus ,p.Reqstaff_appno  from IT_PurchaseOrderDetail pd,IT_PurchaseOrder p,CO_VendorMaster vm,RQ_RequestHierarchy rh where p.PurchaseOrderPK=pd.PurchaseOrderFK and vm.VendorPK=p.VendorFK and p.OrderDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ApproveStatus=1 and  rh.ReqStaffAppNo=p.Reqstaff_appno and rh.ReqAppStaffAppNo='" + app_id + "'";

                    btn_poprint.Visible = true;
                }
                else if (rdb_reject.Checked == true)
                {
                    q1 = " select distinct count(*) as noofitems,CONVERT(varchar(10),OrderDate,103)as orderdate,p.OrderCode,vm.VendorCompName,case when ApproveStatus=2 then 'Reject' end as ApproveStatus,p.Reqstaff_appno from IT_PurchaseOrderDetail pd,IT_PurchaseOrder p,CO_VendorMaster vm,RQ_RequestHierarchy rh where p.PurchaseOrderPK=pd.PurchaseOrderFK and vm.VendorPK=p.VendorFK and p.OrderDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ApproveStatus=2 and rh.ReqAppStaffAppNo='" + app_id + "' and  rh.ReqStaffAppNo=p.Reqstaff_appno " + library + "  group by PurchaseOrderFK,orderdate,OrderCode,VendorCompName,ApproveStatus,Reqstaff_appno";

                    q2 = " select  distinct CONVERT(varchar(10),OrderDate,103)as orderdate,p.OrderCode,pd.PurchaseOrderFK, pd.ItemFK ,vm.VendorCompName,case when ApproveStatus=2 then 'Reject' end as ApproveStatus from IT_PurchaseOrderDetail pd,IT_PurchaseOrder p,CO_VendorMaster vm,RQ_RequestHierarchy rh where p.PurchaseOrderPK=pd.PurchaseOrderFK and vm.VendorPK=p.VendorFK and p.OrderDate between  '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ApproveStatus=2 and  rh.ReqStaffAppNo=p.Reqstaff_appno  and rh.ReqAppStaffAppNo='" + app_id + "'";

                    btn_poprint.Visible = false;
                }
                ds.Clear();
                ds = da.select_method_wo_parameter(q1, "Text");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //check approve state count
                    #region
                    ds1.Clear();
                    ds1 = da.select_method_wo_parameter(q2, "text");
                    string waitingappstatus = "";
                    string approvestatus = da.GetFunction("select distinct(CONVERT(varchar(20),r.ReqApproveStage) +'/'+ (CONVERT(varchar(20), rh.ReqApproveStateCount)))as Status from RQ_RequestHierarchy rh,RQ_Requisition r where rh.ReqStaffAppNo=r.ReqAppNo and rh.RequestType=9 and rh.ReqAppStaffAppNo='" + app_id + "'");

                    if (rdb_wait.Checked == true)
                    {
                        string wait = da.GetFunction("select distinct ReqStaffAppNo from RQ_RequestHierarchy where ReqAppStaffAppNo='" + app_id + "' and RequestType=9"); ;

                        waitingappstatus = da.GetFunction(" select distinct(CONVERT(varchar(20),r.ReqApproveStage) +'/'+ (CONVERT(varchar(20), rh.ReqApproveStateCount)))as Status from RQ_RequestHierarchy rh,RQ_Requisition r where rh.ReqStaffAppNo=r.ReqAppNo and rh.RequestType=9 and rh.ReqStaffAppNo='" + wait + "'");
                        if (waitingappstatus.Trim() == "")
                        {
                            waitingappstatus = "";
                        }
                    }
                    #endregion
                    // end
                    FpSpread1.Sheets[0].RowCount = 1;
                    FpSpread1.Sheets[0].ColumnCount = 0;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread1.CommandBar.Visible = false;
                    FpSpread1.Sheets[0].ColumnCount = 9;
                    FpSpread1.Sheets[0].AutoPostBack = false;
                    FpSpread1.Sheets[0].RowHeader.Visible = false;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Columns[0].Width = 50;
                    FpSpread1.Columns[0].Locked = true;

                    FarPoint.Web.Spread.ButtonCellType btn = new FarPoint.Web.Spread.ButtonCellType();
                    btn.Text = "View";
                    btn.CssClass = "textbox btn1";
                    btn.ForeColor = Color.Blue;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[1].Width = 50;


                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Order Code";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[2].Width = 100;
                    FpSpread1.Columns[2].Locked = true;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Request Date";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].VerticalAlign = VerticalAlign.Middle;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[3].Width = 100;
                    FpSpread1.Columns[3].Locked = true;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Request Name";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[4].Width = 150;
                    FpSpread1.Columns[4].Locked = true;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Vendor Name";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[5].Width = 200;
                    FpSpread1.Columns[5].Locked = true;

                    if (Rad_item.Checked)
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "No of Items";
                    if (Rad_book.Checked)
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "No of Books";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[6].Width = 100;
                    FpSpread1.Columns[6].Locked = true;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Approval Status";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[7].Width = 100;
                    FpSpread1.Columns[7].Locked = true;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Status";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[8].Width = 100;
                    FpSpread1.Columns[8].Locked = true;
                    FpSpread1.Visible = true;
                    if (rdb_request.Checked == true || rdb_approval.Checked == true)
                    {
                        FpSpread1.Columns[7].Visible = false;
                    }
                    else
                    {
                        FpSpread1.Columns[7].Visible = true;
                    }

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string Odcode = Convert.ToString(ds.Tables[0].Rows[i]["OrderCode"]);
                        DataView dv = new DataView();
                        ds1.Tables[0].DefaultView.RowFilter = "OrderCode='" + Odcode + "'";
                        dv = ds1.Tables[0].DefaultView;

                        //string ReqStaffAppNo = Convert.ToString(ds.Tables[0].Rows[i]["ReqStaffAppNo"]);
                        //DataView dv1 = new DataView();
                        //if (ReqStaffAppNo.Trim() != "")
                        //{
                        //    ds2.Tables[0].DefaultView.RowFilter = " and rh.ReqStaffAppNo in('" + ReqStaffAppNo + "')";
                        //    dv1 = ds2.Tables[0].DefaultView;
                        //}

                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = btn;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["OrderCode"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";


                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["orderdate"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";


                        string scode = "";
                        if (rdb_wait.Checked == true)
                        {
                            scode = Convert.ToString(ds.Tables[0].Rows[i]["ReqAppStaffAppNo"]);
                        }
                        else
                        {
                            scode = Convert.ToString(ds.Tables[0].Rows[i]["Reqstaff_appno"]);
                        }

                        string staffname = "";
                        if (scode.Trim() != "")
                        {
                            //staffname = da.GetFunction("select staff_name from staffmaster where staff_code='" + staffcode + "' and college_code='" + collegecode1 + "'");

                            staffname = da.GetFunction("select s.staff_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and sa.appl_id='" + scode + "'");
                        }

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = staffname;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dv.Count);//Convert.ToString(ds.Tables[0].Rows[i]["noofitems"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                        if (approvestatus.Trim() == "")
                        {
                            approvestatus = "";
                        }
                        if (rdb_wait.Checked == true)
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = waitingappstatus;
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = approvestatus;//Convert.ToString(ds.Tables[0].Rows[i]["ApproveStatus"]);
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["ApproveStatus"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                    }

                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.Visible = true;
                    FpSpread1.SaveChanges();
                    lbl_error.Visible = false;
                }
                else
                {
                    FpSpread1.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "No Record Founds";

                }
            }
            else
            {
                lbl_error.Visible = true;
                lbl_error.Text = "From date greater then from todate";
                FpSpread1.Visible = false;
            }
        }
        catch
        { }
    }

    protected void Fp_btn_Click(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            string actrow = e.SheetView.ActiveRow.ToString();
            string actcol = e.SheetView.ActiveColumn.ToString();
            string ordercode = "";
            lbl_time.Text = Convert.ToString(System.DateTime.Now.ToLongDateString());
            if (actrow.Trim() != "" && actcol.Trim() != "")
            {
                ordercode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Text);
                string reqdate = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Text);

                DateTime dt3 = new DateTime();
                string[] Split = reqdate.Split('/');
                dt3 = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
                string q2 = "";
                if (Rad_item.Checked)
                    q2 = "select i.itemname,i.ItemCode,pd.Qty,pd.RPU, pd.DiscountAmt,pd.IsDiscountPercent,CONVERT(varchar(10),OrderDate,103)as orderdate, pd.TaxPercent,pd.ItemFK, pd.EduCessPer,HigherEduCessPer,pd.ExeciseTaxPer, pd.OtherChargeAmt,vm.VendorCompName, pd.OtherChargeDesc, vm.VendorCode,p.VendorFK,p.OrderCode,p.Reqstaff_appno,isnull(p.ReqCompCode,0) ReqCompCode from IM_ItemMaster i,CO_VendorMaster vm,IT_PurchaseOrderDetail pd,IT_PurchaseOrder p where p.PurchaseOrderPK=pd.PurchaseOrderFK and vm.VendorPK=p.VendorFK and p.OrderCode='" + ordercode + "' and p.OrderDate='" + dt3.ToString("MM/dd/yyyy") + "'  and  i.ItemPK=pd.ItemFK and Isnull(vm.LibraryFlag,'')='' ";
                if (Rad_book.Checked)
                    q2 = "select bd.Title itemname,bd.BookID ItemCode,pd.Qty,pd.RPU, pd.DiscountAmt,pd.IsDiscountPercent,CONVERT(varchar(10),OrderDate,103)as orderdate, pd.TaxPercent,pd.ItemFK, pd.EduCessPer,HigherEduCessPer,pd.ExeciseTaxPer, pd.OtherChargeAmt,vm.VendorCompName, pd.OtherChargeDesc, vm.VendorCode,p.VendorFK,p.OrderCode,p.Reqstaff_appno,isnull(p.ReqCompCode,0) ReqCompCode from bookdetails bd,CO_VendorMaster vm,IT_PurchaseOrderDetail pd,IT_PurchaseOrder p where p.PurchaseOrderPK=pd.PurchaseOrderFK and vm.VendorPK=p.VendorFK and p.OrderCode='" + ordercode + "' and p.OrderDate='" + dt3.ToString("MM/dd/yyyy") + "'  and  bd.BookID=pd.ItemFK and vm.LibraryFlag='1'  ";
                if (rdb_request.Checked == true)
                {
                    q2 = q2 + "and ISNULL(ApproveStatus,'')=0";
                    btn_approval.Visible = false;
                    btn_reject.Visible = false;

                }
                else if (rdb_wait.Checked == true)
                {
                    q2 = q2 + "";

                    string userrequest = da.GetFunction("select rh.ReqStaffAppNo from RQ_RequestHierarchy rh,RQ_Requisition r where rh.ReqStaffAppNo=r.ReqAppNo and rh.RequestType=9 and rh.ReqAppStaffAppNo='" + app_id + "'");
                    if (userrequest.Trim() != "0")
                    {
                        btn_approval.Visible = true;
                        btn_reject.Visible = true;
                    }
                    else
                    {
                        btn_approval.Visible = false;
                        btn_reject.Visible = false;
                    }
                }
                else if (rdb_approval.Checked == true)
                {
                    q2 = q2 + "and ApproveStatus=1";
                    btn_approval.Visible = false;
                    btn_reject.Visible = false;
                }
                else if (rdb_reject.Checked == true)
                {
                    q2 = q2 + "and ApproveStatus=2";
                    btn_approval.Visible = false;
                    btn_reject.Visible = false;
                }

                ds.Clear();
                ds = da.select_method_wo_parameter(q2, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Fpspread3.Sheets[0].RowCount = 0;
                    Fpspread3.Sheets[0].ColumnCount = 0;
                    Fpspread3.CommandBar.Visible = false;
                    Fpspread3.Sheets[0].AutoPostBack = true;
                    Fpspread3.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread3.Sheets[0].RowHeader.Visible = false;
                    Fpspread3.Sheets[0].ColumnCount = 18;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread3.Columns[0].Width = 50;
                    Fpspread3.Columns[0].Locked = true;

                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread3.Columns[1].Width = 50;
                    Fpspread3.Columns[1].Visible = false;

                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Vendor Name";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread3.Columns[2].Width = 200;
                    Fpspread3.Columns[2].Locked = true;
                    //Fpspread3.Columns[2].Visible = false;

                    if (Rad_item.Checked)
                    {
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Item Name";
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Item Code";
                    }
                    if (Rad_book.Checked)
                    {
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Book Name";
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Book ID";
                        Fpspread3.Sheets[0].Columns[4].Visible = false;
                    }

                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread3.Columns[3].Width = 200;
                    Fpspread3.Columns[3].Locked = true;
                    //Fpspread3.Columns[3].Visible = false;


                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Item Code";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpspread3.Columns[4].Width = 100;
                    Fpspread3.Columns[4].Locked = true;
                    //Fpspread3.Columns[4].Visible = false;

                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Quantity";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    Fpspread3.Columns[5].Width = 100;
                    Fpspread3.Columns[5].Locked = true;
                    //Fpspread3.Columns[5].Visible = false;

                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Rpu";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    Fpspread3.Columns[6].Width = 100;
                    Fpspread3.Columns[6].Locked = true;
                    //Fpspread3.Columns[6].Visible = false;

                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Discount";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                    Fpspread3.Columns[7].Width = 100;
                    Fpspread3.Columns[7].Locked = true;
                    //Fpspread3.Columns[7].Visible = false;

                    //Fpspread3.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Discount Per";
                    //Fpspread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                    //Fpspread3.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                    //Fpspread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                    //Fpspread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                    //Fpspread3.Columns[8].Width = 100;
                    //Fpspread3.Columns[8].Locked = true;
                    Fpspread3.Columns[8].Visible = false;

                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Tax";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Columns[9].Width = 100;
                    Fpspread3.Columns[9].Locked = true;
                    //Fpspread3.Columns[9].Visible = false;

                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Exercies tax";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Columns[10].Width = 100;
                    Fpspread3.Columns[10].Locked = true;
                    //Fpspread3.Columns[10].Visible = false;


                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Education Cess";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Columns[11].Width = 100;
                    Fpspread3.Columns[11].Locked = true;
                    //Fpspread3.Columns[11].Visible = false;

                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Higher Edu.Cess";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 12].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 12].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 12].Font.Size = FontUnit.Medium;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 12].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Columns[12].Width = 150;
                    Fpspread3.Columns[12].Locked = true;
                    //Fpspread3.Columns[12].Visible = false;

                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Other Charges";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 13].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 13].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 13].Font.Size = FontUnit.Medium;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 13].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Columns[13].Width = 150;
                    Fpspread3.Columns[13].Locked = true;
                    //Fpspread3.Columns[13].Visible = false;

                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 14].Text = "Description";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 14].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 14].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 14].Font.Size = FontUnit.Medium;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 14].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Columns[14].Width = 150;
                    Fpspread3.Columns[14].Locked = true;
                    //Fpspread3.Columns[14].Visible = false;

                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 15].Text = "CallExTax";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 15].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 15].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 15].Font.Size = FontUnit.Medium;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 15].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread3.Columns[15].Width = 150;
                    Fpspread3.Columns[15].Locked = true;
                    Fpspread3.Columns[15].Visible = false;

                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 16].Text = "Cost";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 16].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 16].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 16].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 16].Font.Size = FontUnit.Medium;
                    Fpspread3.Columns[16].Width = 100;
                    Fpspread3.Columns[16].Locked = true;
                    //Fpspread3.Columns[16].Visible = false;


                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 17].Text = "Date";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 17].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 17].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 17].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 17].Font.Size = FontUnit.Medium;
                    Fpspread3.Columns[17].Width = 250;
                    Fpspread3.Columns[17].Locked = true;


                    //Fpspread3.Sheets[0].ColumnHeader.Cells[0, 17].Text = "Vendor Code";
                    //Fpspread3.Sheets[0].ColumnHeader.Cells[0, 17].Font.Bold = true;
                    //Fpspread3.Sheets[0].ColumnHeader.Cells[0, 17].HorizontalAlign = HorizontalAlign.Center;
                    //Fpspread3.Sheets[0].ColumnHeader.Cells[0, 17].Font.Name = "Book Antiqua";
                    //Fpspread3.Sheets[0].ColumnHeader.Cells[0, 17].Font.Size = FontUnit.Medium;
                    //Fpspread3.Columns[17].Width = 100;
                    //Fpspread3.Columns[17].Locked = true;
                    FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                    chkall.AutoPostBack = false;
                    string rpu1 = "";
                    double cost1 = 0;
                    double totalcost = 0;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Fpspread3.Sheets[0].RowCount++;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].CellType = chkall;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["VendorFK"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["itemname"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["ItemFK"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                        double qty = Convert.ToDouble(ds.Tables[0].Rows[i]["Qty"]);
                        double rpu = Convert.ToDouble(ds.Tables[0].Rows[i]["RPU"]);
                        double tax = Convert.ToDouble(ds.Tables[0].Rows[i]["TaxPercent"]);
                        double extax = Convert.ToDouble(ds.Tables[0].Rows[i]["ExeciseTaxPer"]);
                        double otherharge = Convert.ToDouble(ds.Tables[0].Rows[i]["OtherChargeAmt"]);
                        double dispercal = 0;
                        double disamtcal = 0;
                        string disper = Convert.ToString(ds.Tables[0].Rows[i]["IsDiscountPercent"]);
                        string disamt = Convert.ToString(ds.Tables[0].Rows[i]["DiscountAmt"]);
                        string discount = "";
                        double dis = 0;
                        //if (disper.Trim() != "")
                        //{
                        //    if (disper == "False")
                        //    {
                        //        discount = "0";
                        //    }
                        //    else
                        //    {
                        //        discount = disper;
                        //        dispercal = Convert.ToDouble(discount);
                        //    }
                        //}
                        //if (disamt.Trim() != "")
                        //{
                        //    discount = disamt;
                        //    disamtcal = Convert.ToDouble(discount);
                        //}
                        //else
                        //{
                        //    //discount = "0";
                        //    disamtcal = 0;
                        //}
                        double cost = 0;
                        cost = qty * rpu;
                        //if (disamtcal != 0)
                        //{
                        //   // cost = cost - disamtcal;
                        //    cost = disamtcal;
                        //}
                        //if (dispercal != 0)
                        //{
                        //    dis = (cost / 100) * dispercal;
                        //    cost = cost - dis;
                        //}
                        if (disper.ToUpper() == "TRUE")
                        {
                            if (Convert.ToDouble(disamt) != 0)
                            {
                                cost = cost - Convert.ToDouble(disamt);
                            }
                        }
                        else
                        {
                            if (Convert.ToDouble(disamt) != 0)
                            {
                                double d = (cost / 100) * Convert.ToDouble(disamt);
                                cost = cost - d;
                                //cost = discount;
                            }
                        }
                        if (tax != 0)
                        {
                            double t = (cost / 100) * tax;
                            cost = cost + t;
                        }
                        if (extax != 0)
                        {
                            double ex = cost / 100 * extax;
                            cost = cost + ex;
                        }
                        cost = cost + otherharge;

                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["OrderCode"]);

                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["qty"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Reqstaff_appno"]);

                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["RPU"]);

                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(ds.Tables[0].Rows[i]["ReqCompCode"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";


                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 7].Text = disamt;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[i]["TaxPercent"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(ds.Tables[0].Rows[i]["ExeciseTaxPer"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(ds.Tables[0].Rows[i]["EduCessPer"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";

                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(ds.Tables[0].Rows[i]["HigherEduCessPer"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 12].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 12].Font.Name = "Book Antiqua";

                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 13].Text = Convert.ToString(ds.Tables[0].Rows[i]["OtherChargeAmt"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 13].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 13].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 13].Font.Name = "Book Antiqua";

                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 14].Text = Convert.ToString(ds.Tables[0].Rows[i]["OtherChargeDesc"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 14].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 14].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 14].Font.Name = "Book Antiqua";

                        //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 15].Text = Convert.ToString(ds.Tables[0].Rows[i]["CallExTax"]);
                        //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 15].HorizontalAlign = HorizontalAlign.Right;
                        //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 15].Font.Size = FontUnit.Medium;
                        //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 15].Font.Name = "Book Antiqua";

                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 16].Text = Convert.ToString(Math.Round(cost, 2));
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 16].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 16].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 16].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Text = Convert.ToString(ds.Tables[0].Rows[i]["orderdate"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Font.Size = FontUnit.Medium;

                        if (cost != 0)
                        {
                            cost1 = cost;
                        }
                        if (cost1 != 0)
                        {
                            totalcost = totalcost + cost1;
                        }
                        cost1 = 0;
                    }
                    Fpspread3.Sheets[0].RowCount++;
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 14].Text = "Total Cost";
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 14].BackColor = Color.Ivory;
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 16].Text = Convert.ToString(Math.Round(totalcost, 2));
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 16].BackColor = Color.Ivory;
                    Fpspread3.Sheets[0].SpanModel.Add(Fpspread3.Sheets[0].RowCount - 1, 0, 1, 14);
                    Fpspread3.Sheets[0].PageSize = Fpspread3.Sheets[0].RowCount;
                    Fpspread3.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpspread3.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpspread3.Sheets[0].FrozenRowCount = 0;
                    Fpspread3.Visible = true;
                    Fpspread3.SaveChanges();
                    pop_purchaseitems.Visible = true;
                }
            }
        }
        catch
        { }
    }
    protected void btn_approval_Click(object sender, EventArgs e)
    {
        try
        {
            int ApproveStage = 0;
            int ApproveStateCount = 0;
            int approvestagestaff = 0;
            bool check = false;
            string query = "";
            string RequestType = "";
            string q1 = "";
            string reqcode = "";
            //check the approval statecount
            #region

            if (Fpspread3.Rows.Count > 0)
            {
                for (i = 0; i < Fpspread3.Rows.Count; i++)
                {
                    string ordercode1 = Convert.ToString(Fpspread3.Sheets[0].Cells[i, 4].Tag);
                    string poreqstaffid = Convert.ToString(Fpspread3.Sheets[0].Cells[i, 5].Tag);

                    query = da.GetFunction(" select OrderType from IT_PurchaseOrder where OrderCode='" + ordercode1 + "' and Reqstaff_appno='" + poreqstaffid + "'");
                    if (query.Trim() == "2")
                    {
                        RequestType = "1";
                        reqcode = "";
                    }
                    else
                    {
                        q1 = "";
                        q1 = da.GetFunction("select MasterCriteria1 from CO_MasterValues where MasterValue='" + ordercode1 + "' and MasterCriteria='PO Requestcode and Ordercode'");

                        RequestType = "9";
                        reqcode = " and RequestCode='" + q1 + "'";

                    }

                    string checkstage = "select distinct r.ReqApproveStage, rh.ReqApproveStateCount,rh.ReqApproveStage as approvestaff from RQ_RequestHierarchy rh,RQ_Requisition r where rh.ReqStaffAppNo=r.ReqAppNo and rh.RequestType='" + RequestType + "' and rh.ReqAppStaffAppNo in('" + app_id + "') " + reqcode + "";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(checkstage, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ApproveStage = Convert.ToInt32(ds.Tables[0].Rows[0]["ReqApproveStage"]);
                        ApproveStateCount = Convert.ToInt32(ds.Tables[0].Rows[0]["ReqApproveStateCount"]);
                        approvestagestaff = Convert.ToInt32(ds.Tables[0].Rows[0]["approvestaff"]);
                    }

                    if (ApproveStage == ApproveStateCount && ApproveStage == approvestagestaff)
                    {
                        if (Fpspread3.Rows.Count > 0)
                        {
                            for (int row = 0; row < Fpspread3.Sheets[0].RowCount - 1; row++)
                            {

                                string ordercode = Convert.ToString(Fpspread3.Sheets[0].Cells[row, 4].Tag);
                                string recomcode = Convert.ToString(Fpspread3.Sheets[0].Cells[row, 6].Tag);
                                string updatequery = "update IT_PurchaseOrder set approvestatus='1' where OrderCode in('" + ordercode + "')";
                                int up = da.update_method_wo_parameter(updatequery, "Text");
                                if (recomcode != "" && recomcode != "0")
                                {
                                    string updatequery1 = "";
                                    if (Rad_item.Checked)
                                        updatequery1 = "update IT_VendorReq set PurchaseStatus='1' where ReqCompCode in('" + recomcode + "') and Isnull(itemorlibrary,'')=''";
                                    if (Rad_book.Checked)
                                        updatequery1 = "update IT_VendorReq set PurchaseStatus='1' where ReqCompCode in('" + recomcode + "') and itemorlibrary='1'";
                                    int up1 = da.update_method_wo_parameter(updatequery1, "Text");
                                }

                                if (up != 0)
                                {
                                    check = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        string rqappno = da.GetFunction("select distinct ReqStaffAppNo from RQ_RequestHierarchy where ReqAppStaffAppNo='" + app_id + "' and RequestType='" + RequestType + "'");
                        if (rqappno.Trim() != "")
                        {
                            if (approvestagestaff != ApproveStage)
                            {
                                // string rqupate = "update RQ_Requisition set ReqApproveStage=ReqApproveStage+1 where  ReqAppNo='" + rqappno + "'";
                                string rqupate = "update RQ_Requisition set ReqApproveStage='" + approvestagestaff + "' where  ReqAppNo='" + rqappno + "' and RequestType='" + RequestType + "' " + reqcode + "";
                                int rq = da.update_method_wo_parameter(rqupate, "Text");
                                if (rq != 0)
                                {
                                    check = true;
                                }
                            }
                            //check the approval statecount
                            #region
                            ds.Clear();
                            ds = da.select_method_wo_parameter(checkstage, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                ApproveStage = Convert.ToInt32(ds.Tables[0].Rows[0]["ReqApproveStage"]);
                                ApproveStateCount = Convert.ToInt32(ds.Tables[0].Rows[0]["ReqApproveStateCount"]);
                                approvestagestaff = Convert.ToInt32(ds.Tables[0].Rows[0]["approvestaff"]);
                            }

                            if (ApproveStage == ApproveStateCount && ApproveStage == approvestagestaff)
                            {
                                if (Fpspread3.Rows.Count > 0)
                                {
                                    for (int row = 0; row < Fpspread3.Sheets[0].RowCount - 1; row++)
                                    {
                                        string ordercode = Convert.ToString(Fpspread3.Sheets[0].Cells[row, 4].Tag);
                                        string recomcode = Convert.ToString(Fpspread3.Sheets[0].Cells[row, 6].Tag);
                                        string updatequery = "update IT_PurchaseOrder set approvestatus='1' where OrderCode in('" + ordercode + "')";
                                        int up = da.update_method_wo_parameter(updatequery, "Text");
                                        if (recomcode != "" && recomcode != "0")
                                        {
                                            string updatequery1 = "";
                                            if (Rad_item.Checked)
                                                updatequery1 = "update IT_VendorReq set PurchaseStatus='1' where ReqCompCode in('" + recomcode + "') and Isnull(itemorlibrary,'')=''";
                                            if (Rad_book.Checked)
                                                updatequery1 = "update IT_VendorReq set PurchaseStatus='1' where ReqCompCode in('" + recomcode + "') and itemorlibrary='1'";
                                            int up1 = da.update_method_wo_parameter(updatequery1, "Text");
                                        }
                                        if (up != 0)
                                        {
                                            check = true;
                                        }
                                    }
                                }

                                q1 = "";
                                //q1 = "update RQ_RequestHierarchy set ReqApproveStage='" + approvestagestaff + "' where RequestType='" + RequestType + "' and ReqStaffAppNo='" + rqappno + "'";
                                q1 = " update RQ_Requisition set ReqAppStatus='1' where  ReqAppNo='" + rqappno + "' and RequestType='" + RequestType + "' " + reqcode + "";
                                int insert = da.update_method_wo_parameter(q1, "Text");


                            }
                            #endregion
                        }
                    }
                    if (check == true)
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Visible = true;
                        lbl_alert.Text = "Approved Successfully";
                        pop_purchaseitems.Visible = false;
                        // btn_go_Click(sender, e);
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Visible = true;
                        lbl_alert.Text = "Already Approved";
                        //pop_purchaseitems.Visible = false;

                    }
                }
            }
            btn_go_Click(sender, e);
            #endregion
        }
        catch { }
    }
    protected void btn_reject_Click(object sender, EventArgs e)
    {
        try
        {
            bool check1 = false;
            if (Fpspread3.Rows.Count > 0)
            {
                for (int row = 0; row < FpSpread1.Sheets[0].RowCount - 1; row++)
                {
                    string ordercode = Convert.ToString(Fpspread3.Sheets[0].Cells[row, 4].Tag);
                    string updatequery = "update IT_PurchaseOrder set approvestatus='2' where OrderCode in('" + ordercode + "')";

                    int up = da.update_method_wo_parameter(updatequery, "Text");
                    if (up != 0)
                    {
                        check = true;
                    }
                }
            }
            if (check == true)
            {
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Rejected Successfully";
                btn_go_Click(sender, e);
            }
        }
        catch { }
    }
    protected void imagebtnpopclose3_Click(object sender, EventArgs e)
    {
        pop_purchaseitems.Visible = false;
    }
    protected void btnpop1winexit_Click(object sender, EventArgs e)
    {
        pop_purchaseitems.Visible = false;
    }
    //po print
    protected void btn_poprint_Click(object sender, EventArgs e)
    {
        FpSpread1.SaveChanges();
        double page2col = 0;
        //string vendorcode = "";
        string vendorpk = "";
        string ordercode = "";
        string reqdate = "";
        DateTime dt3 = new DateTime();
        if (Fpspread3.Sheets[0].RowCount > 0)
        {
            for (int row = 0; row < Fpspread3.Sheets[0].RowCount - 1; row++)
            {
                if (vendorpk == "")
                {
                    vendorpk = Fpspread3.Sheets[0].Cells[row, 2].Tag.ToString();
                }
                else
                {
                    vendorpk = vendorpk + "','" + Fpspread3.Sheets[0].Cells[row, 2].Tag.ToString();
                }

                if (ordercode == "")
                {
                    ordercode = Fpspread3.Sheets[0].Cells[row, 4].Tag.ToString();
                }
                else
                {
                    ordercode = ordercode + "','" + Fpspread3.Sheets[0].Cells[row, 4].Tag.ToString();
                }

                reqdate = Convert.ToString(Fpspread3.Sheets[0].Cells[row, 17].Text);

                string[] Split = reqdate.Split('/');
                dt3 = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);


            }
        }
        int coltop = 0;
        string Collvalue = "";
        Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
        System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
        System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
        System.Drawing.Font Fontbold16 = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Bold);
        System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
        System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
        System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
        System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
        System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
        System.Drawing.Font Fontsmall1bold = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
        System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);
        Gios.Pdf.PdfPage mypdfpage;
        PdfTextArea collinfo1;
        mypdfpage = mydoc.NewPage();

        #region
        string strquery = "Select * from Collinfo where college_code=" + Session["collegecode"].ToString() + "";
        ds.Clear();
        ds = da.select_method_wo_parameter(strquery, "Text");
        string collegedetails = da.GetFunction("select college_details from tbl_print_master_settings where page_Name='Investorsposetting.aspx'");
        string[] spiltcollegedetails = collegedetails.Split('#');
        for (int i = 0; i <= spiltcollegedetails.GetUpperBound(0); i++)
        {
            coltop = coltop + 15;
            string collinfo = spiltcollegedetails[i].ToString();
            if (collinfo == "College Name")
            {
                collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0]["collname"].ToString() + "");
                mypdfpage.Add(collinfo1);

            }
            else if (collinfo == "University")
            {
                collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0]["university"].ToString() + "");
                mypdfpage.Add(collinfo1);
            }
            else if (collinfo == "Affliated By")
            {
                collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0]["affliatedby"].ToString() + "");
                mypdfpage.Add(collinfo1);
            }
            else if (collinfo == "Address")
            {
                string address1 = ds.Tables[0].Rows[0]["Address1"].ToString();
                string address2 = ds.Tables[0].Rows[0]["Address2"].ToString();
                string address3 = ds.Tables[0].Rows[0]["Address3"].ToString();
                if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                {
                    Collvalue = address1;
                }
                if (address2.Trim() != "" && address2 != null && address2.Length > 1)
                {
                    if (Collvalue.Trim() != "" && Collvalue != null)
                    {
                        Collvalue = Collvalue + ',' + address2;
                    }
                    else
                    {
                        Collvalue = address2;
                    }
                }
                if (address3.Trim() != "" && address3 != null && address3.Length > 1)
                {
                    if (Collvalue.Trim() != "" && Collvalue != null)
                    {
                        Collvalue = Collvalue + ',' + address3;
                    }
                    else
                    {
                        Collvalue = address3;
                    }
                }
                collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                mypdfpage.Add(collinfo1);
            }
            else if (collinfo == "City")
            {
                string address1 = ds.Tables[0].Rows[0]["Address3"].ToString();
                if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                {
                    Collvalue = address1;
                }
                collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                mypdfpage.Add(collinfo1);
            }
            else if (collinfo == "District & State & Pincode")
            {
                string district = ds.Tables[0].Rows[0]["district"].ToString();
                string state = ds.Tables[0].Rows[0]["State"].ToString();
                string pincode = ds.Tables[0].Rows[0]["Pincode"].ToString();
                if (district.Trim() != "" && district != null && district.Length > 1)
                {
                    Collvalue = district;
                }
                if (state.Trim() != "" && state != null && state.Length > 1)
                {
                    if (Collvalue.Trim() != "" && Collvalue != null)
                    {
                        Collvalue = Collvalue + ',' + state;
                    }
                    else
                    {
                        Collvalue = state;
                    }
                }
                if (pincode.Trim() != "" && pincode != null && pincode.Length > 1)
                {
                    if (Collvalue.Trim() != "" && Collvalue != null)
                    {
                        Collvalue = Collvalue + '-' + pincode;
                    }
                    else
                    {
                        Collvalue = pincode;
                    }
                }
                collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                mypdfpage.Add(collinfo1);
            }
            else if (collinfo == "Phone No & Fax")
            {
                string phone = ds.Tables[0].Rows[0]["Phoneno"].ToString();
                string fax = ds.Tables[0].Rows[0]["Faxno"].ToString();
                if (phone.Trim() != "" && phone != null && phone.Length > 1)
                {
                    Collvalue = "Phone :" + phone;
                }
                if (fax.Trim() != "" && fax != null && fax.Length > 1)
                {
                    if (Collvalue.Trim() != "" && Collvalue != null)
                    {
                        Collvalue = Collvalue + " , Fax : " + fax;
                    }
                    else
                    {
                        Collvalue = "Fax :" + fax;
                    }
                }

                collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                mypdfpage.Add(collinfo1);
            }
            else if (collinfo == "Email & Web Site")
            {
                string email = ds.Tables[0].Rows[0]["Email"].ToString();
                string website = ds.Tables[0].Rows[0]["Website"].ToString();
                if (email.Trim() != "" && email != null && email.Length > 1)
                {
                    Collvalue = "Email :" + email;
                }
                if (website.Trim() != "" && website != null && website.Length > 1)
                {
                    if (Collvalue.Trim() != "" && Collvalue != null)
                    {
                        Collvalue = Collvalue + " , Web Site : " + website;
                    }
                    else
                    {
                        Collvalue = "Web Site :" + website;
                    }
                }
                collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                mypdfpage.Add(collinfo1);
            }
            else if (collinfo == "Left Logo")
            {
                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                {
                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));

                    mypdfpage.Add(LogoImage, 25, 25, 400);

                }
            }
            else if (collinfo == "Right Logo")
            {
                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                {
                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                    mypdfpage.Add(LogoImage, 480, 25, 400);

                }
            }

        }
        #endregion

        DataView dv = new DataView();
        // string sql = "select distinct vm.VendorCode,vm.VendorCompName,vm.VendorAddress, vm.VendorCity,vm.VendorPin, vm.VendorDist,vm.VendorState,p.OrderCode,CONVERT(varchar(10),OrderDate,103)as orderdate,CONVERT(varchar(10),OrderDueDate,103)as orderduedate,i.ItemName,convert(money,(AppQty-ISNULL(RejQty ,0))) as App_Qty,pd.rpu,(AppQty*rpu)Amount,p.OrderDescription,p.TotTaxAmt,p.TotOtherChgAmt,p.NetAmount from IM_ItemMaster i,IM_VendorItemDept vd,CO_VendorMaster vm,IT_PurchaseOrder p,IT_PurchaseOrderDetail pd where i.ItemPK=vd.ItemFK and vd.ItemFK =pd.ItemFK and vm.VendorPK=p.VendorFK and  vm.VendorPK=vd.VenItemFK and p.PurchaseOrderPK=pd.PurchaseOrderFK and p.VendorFK=vd.VenItemFK  and pd.ItemFK=vd.ItemFK and p.VendorFK in ('" + vendorpk + "') and p.OrderCode in('" + ordercode + "') order by VendorCode,p.OrderCode";

        string sql = "select vm.VendorAddress,vm.VendorCity,vm.VendorPin,vm.VendorDist,vm.VendorState,i.itemname,i.ItemCode, pd.Qty,pd.RPU, pd.DiscountAmt,pd.IsDiscountPercent,CONVERT(varchar(10),OrderDate,103)as orderdate, CONVERT(varchar(10),OrderDueDate,103)as orderduedate, pd.TaxPercent,pd.ItemFK,p.TotTaxAmt,p.TotOtherChgAmt,p.NetAmount, p.OrderDescription,pd.EduCessPer,HigherEduCessPer,pd.ExeciseTaxPer,pd.OtherChargeAmt,vm.VendorCompName, pd.OtherChargeDesc, vm.VendorCode,vm.VendorPK,p.OrderCode,convert(decimal(12,2),(AppQty*rpu))Amount,convert(float,(AppQty-ISNULL(RejQty ,0)))as App_Qty from IM_ItemMaster i,CO_VendorMaster vm,IT_PurchaseOrderDetail pd,IT_PurchaseOrder p where p.PurchaseOrderPK=pd.PurchaseOrderFK and vm.VendorPK=p.VendorFK and  i.ItemPK=pd.ItemFK and  p.OrderCode in('" + ordercode + "') and p.OrderDate='" + dt3.ToString("MM/dd/yyyy") + "' and vm.VendorPK in('" + vendorpk + "')";

        ds.Clear();
        ds = da.select_method_wo_parameter(sql, "Text");
        string vendorcconew = "";
        Gios.Pdf.PdfTable table1forpage1;
        Gios.Pdf.PdfTable table1forpage1datas;
        Boolean pdfstart = true;
        double binddatatb = 0;

        double totalamt = 0;
        double totaltax = 0;
        double totalcharges = 0;
        double totaldiscnt = 0;
        double totalnet = 0;
        DataSet dsterms = new DataSet();
        DataSet ds1 = new DataSet();
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
            {
                if (vendorcconew != ds.Tables[0].Rows[ii]["OrderCode"].ToString())
                {
                    vendorcconew = ds.Tables[0].Rows[ii]["OrderCode"].ToString();
                    ds.Tables[0].DefaultView.RowFilter = "OrderCode='" + ds.Tables[0].Rows[ii]["OrderCode"].ToString() + "'";
                    dv = ds.Tables[0].DefaultView;
                    if (dv.Count > 0)
                    {
                        table1forpage1datas = mydoc.NewTable(Fontsmall1, dv.Count, 5, 3);
                        table1forpage1datas.VisibleHeaders = false;
                        table1forpage1datas.SetBorders(Color.Black, 1, BorderType.None);
                        table1forpage1datas.Columns[0].SetWidth(30);
                        table1forpage1datas.Columns[1].SetWidth(190);
                        table1forpage1datas.Columns[2].SetWidth(60);
                        table1forpage1datas.Columns[3].SetWidth(60);
                        table1forpage1datas.Columns[4].SetWidth(60);
                        for (int j = 0; j < dv.Count; j++)
                        {
                            #region
                            if (j == 0 && pdfstart == false)
                            {
                                mypdfpage.SaveToDocument();
                                mypdfpage = mydoc.NewPage();
                                coltop = 0;
                                strquery = "Select * from Collinfo where college_code=" + Session["collegecode"].ToString() + "";
                                ds1.Clear();
                                ds1 = da.select_method_wo_parameter(strquery, "Text");
                                collegedetails = da.GetFunction("select college_details from tbl_print_master_settings where page_Name='Investorsposetting.aspx'");
                                spiltcollegedetails = collegedetails.Split('#');
                                for (int i = 0; i <= spiltcollegedetails.GetUpperBound(0); i++)
                                {
                                    coltop = coltop + 15;
                                    string collinfo = spiltcollegedetails[i].ToString();
                                    if (collinfo == "College Name")
                                    {
                                        collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds1.Tables[0].Rows[0]["collname"].ToString() + "");
                                        mypdfpage.Add(collinfo1);

                                    }
                                    else if (collinfo == "University")
                                    {
                                        collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds1.Tables[0].Rows[0]["university"].ToString() + "");
                                        mypdfpage.Add(collinfo1);
                                    }
                                    else if (collinfo == "Affliated By")
                                    {
                                        collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds1.Tables[0].Rows[0]["affliatedby"].ToString() + "");
                                        mypdfpage.Add(collinfo1);
                                    }
                                    else if (collinfo == "Address")
                                    {
                                        string address1 = ds1.Tables[0].Rows[0]["Address1"].ToString();
                                        string address2 = ds1.Tables[0].Rows[0]["Address2"].ToString();
                                        string address3 = ds1.Tables[0].Rows[0]["Address3"].ToString();
                                        if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                        {
                                            Collvalue = address1;
                                        }
                                        if (address2.Trim() != "" && address2 != null && address2.Length > 1)
                                        {
                                            if (Collvalue.Trim() != "" && Collvalue != null)
                                            {
                                                Collvalue = Collvalue + ',' + address2;
                                            }
                                            else
                                            {
                                                Collvalue = address2;
                                            }
                                        }
                                        if (address3.Trim() != "" && address3 != null && address3.Length > 1)
                                        {
                                            if (Collvalue.Trim() != "" && Collvalue != null)
                                            {
                                                Collvalue = Collvalue + ',' + address3;
                                            }
                                            else
                                            {
                                                Collvalue = address3;
                                            }
                                        }
                                        collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                        mypdfpage.Add(collinfo1);
                                    }
                                    else if (collinfo == "City")
                                    {
                                        string address1 = ds1.Tables[0].Rows[0]["Address3"].ToString();
                                        if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                        {
                                            Collvalue = address1;
                                        }
                                        collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                        mypdfpage.Add(collinfo1);
                                    }
                                    else if (collinfo == "District & State & Pincode")
                                    {
                                        string district = ds1.Tables[0].Rows[0]["district"].ToString();
                                        string state = ds1.Tables[0].Rows[0]["State"].ToString();
                                        string pincode = ds1.Tables[0].Rows[0]["Pincode"].ToString();
                                        if (district.Trim() != "" && district != null && district.Length > 1)
                                        {
                                            Collvalue = district;
                                        }
                                        if (state.Trim() != "" && state != null && state.Length > 1)
                                        {
                                            if (Collvalue.Trim() != "" && Collvalue != null)
                                            {
                                                Collvalue = Collvalue + ',' + state;
                                            }
                                            else
                                            {
                                                Collvalue = state;
                                            }
                                        }
                                        if (pincode.Trim() != "" && pincode != null && pincode.Length > 1)
                                        {
                                            if (Collvalue.Trim() != "" && Collvalue != null)
                                            {
                                                Collvalue = Collvalue + '-' + pincode;
                                            }
                                            else
                                            {
                                                Collvalue = pincode;
                                            }
                                        }
                                        collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                        mypdfpage.Add(collinfo1);
                                    }
                                    else if (collinfo == "Phone No & Fax")
                                    {
                                        string phone = ds1.Tables[0].Rows[0]["Phoneno"].ToString();
                                        string fax = ds1.Tables[0].Rows[0]["Faxno"].ToString();
                                        if (phone.Trim() != "" && phone != null && phone.Length > 1)
                                        {
                                            Collvalue = "Phone :" + phone;
                                        }
                                        if (fax.Trim() != "" && fax != null && fax.Length > 1)
                                        {
                                            if (Collvalue.Trim() != "" && Collvalue != null)
                                            {
                                                Collvalue = Collvalue + " , Fax : " + fax;
                                            }
                                            else
                                            {
                                                Collvalue = "Fax :" + fax;
                                            }
                                        }

                                        collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                        mypdfpage.Add(collinfo1);
                                    }
                                    else if (collinfo == "Email & Web Site")
                                    {
                                        string email = ds1.Tables[0].Rows[0]["Email"].ToString();
                                        string website = ds1.Tables[0].Rows[0]["Website"].ToString();
                                        if (email.Trim() != "" && email != null && email.Length > 1)
                                        {
                                            Collvalue = "Email :" + email;
                                        }
                                        if (website.Trim() != "" && website != null && website.Length > 1)
                                        {
                                            if (Collvalue.Trim() != "" && Collvalue != null)
                                            {
                                                Collvalue = Collvalue + " , Web Site : " + website;
                                            }
                                            else
                                            {
                                                Collvalue = "Web Site :" + website;
                                            }
                                        }
                                        collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                        mypdfpage.Add(collinfo1);
                                    }
                                    else if (collinfo == "Left Logo")
                                    {
                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                        {
                                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));

                                            mypdfpage.Add(LogoImage, 25, 25, 400);

                                        }
                                    }
                                    else if (collinfo == "Right Logo")
                                    {
                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                        {
                                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                            mypdfpage.Add(LogoImage, 480, 25, 400);

                                        }
                                    }

                                }

                            }
                            if (j == 0)
                            {
                                pdfstart = false;
                                //collinfo1 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 140, 595, 50), System.Drawing.ContentAlignment.TopCenter, "APPROVE PURCHASE ORDER ITEMS");
                                collinfo1 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 140, 595, 50), System.Drawing.ContentAlignment.TopCenter, "PURCHASE ORDER");
                                mypdfpage.Add(collinfo1);//modified by rajasekar 27/07/2018

                                table1forpage1 = mydoc.NewTable(Fontsmall1, 5, 5, 2);
                                table1forpage1.VisibleHeaders = false;
                                table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                                table1forpage1.Columns[0].SetWidth(20);
                                table1forpage1.Columns[1].SetWidth(230);
                                table1forpage1.Columns[2].SetWidth(60);
                                table1forpage1.Columns[3].SetWidth(10);
                                table1forpage1.Columns[4].SetWidth(80);
                                // table1forpage1.Columns[5].SetWidth(65);
                                table1forpage1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(0, 0).SetContent("To   :");
                                table1forpage1.Cell(0, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(0, 1).SetContent(dv[j]["VendorCompName"].ToString());
                                table1forpage1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(0, 2).SetContent("Order No ");
                                table1forpage1.Cell(0, 2).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage1.Cell(0, 3).SetContent(":");
                                table1forpage1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(0, 4).SetContent(dv[j]["OrderCode"].ToString());

                                table1forpage1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage1.Cell(1, 0).SetContent("");
                                table1forpage1.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(1, 1).SetContent(dv[j]["VendorAddress"].ToString());
                                table1forpage1.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(1, 2).SetContent("Date");
                                table1forpage1.Cell(1, 2).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage1.Cell(1, 3).SetContent(":");
                                table1forpage1.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(1, 4).SetContent(dv[j]["orderdate"].ToString());

                                table1forpage1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage1.Cell(2, 0).SetContent("");
                                table1forpage1.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(2, 1).SetContent("" + dv[j]["VendorCity"].ToString() + " - " + dv[j]["VendorPin"].ToString() + "");
                                table1forpage1.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(2, 2).SetContent("Due Date");
                                table1forpage1.Cell(2, 2).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage1.Cell(2, 3).SetContent(":");
                                table1forpage1.Cell(2, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                if (dv[j]["orderduedate"].ToString() != "01/01/1900")//added by rajasekar 27/07/2018
                                    table1forpage1.Cell(2, 4).SetContent(dv[j]["orderduedate"].ToString());
                                else
                                    table1forpage1.Cell(2, 4).SetContent("");

                                table1forpage1.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage1.Cell(3, 0).SetContent("");
                                table1forpage1.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                                string vendistrict = Convert.ToString(dv[j]["VendorDist"].ToString());

                                vendistrict = da.GetFunction("select MasterValue from CO_MasterValues where MasterCode='" + vendistrict + "' and MasterCriteria='District'");
                                table1forpage1.Cell(3, 1).SetContent(vendistrict);


                                // table1forpage1.Cell(3, 1).SetContent(dv[j]["VendorDist"].ToString());
                                //table1forpage1.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                //table1forpage1.Cell(3, 2).SetContent("Delivery At");
                                //table1forpage1.Cell(3, 2).SetFont(Fontsmall1bold);
                                //table1forpage1.Cell(3, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                //table1forpage1.Cell(3, 3).SetContent(":");
                                //table1forpage1.Cell(3, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                //table1forpage1.Cell(3, 4).SetContent("");

                                table1forpage1.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                //table1forpage1.Cell(4, 1).SetContent(dv[j]["VendorState"].ToString());
                                string state = Convert.ToString(dv[j]["VendorState"].ToString());
                                state = da.GetFunction("select MasterValue from CO_MasterValues where MasterCode='" + state + "' and MasterCriteria='State'");
                                table1forpage1.Cell(4, 1).SetContent(state);
                                Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 50, 160, 567, 400));
                                mypdfpage.Add(newpdftabpage2);

                                Double getheigh = newpdftabpage2.Area.Height;
                                getheigh = Math.Round(getheigh, 2);
                                page2col = getheigh + 150;
                                collinfo1 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 80, page2col, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "With reference to your Quotation and subsequent discussion te undersigned had with you,We are");
                                mypdfpage.Add(collinfo1);
                                page2col = page2col + 10;
                                collinfo1 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 80, page2col, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "pleased to place the order on you as detailed below");
                                mypdfpage.Add(collinfo1);

                                page2col = page2col + 10;
                                collinfo1 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 50, page2col, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "-------------------------------------------------------------------------------------------------------------------------------------------------");
                                mypdfpage.Add(collinfo1);
                                page2col = page2col + 27;
                                table1forpage1 = mydoc.NewTable(Fontsmall1, 1, 5, 2);
                                table1forpage1.VisibleHeaders = false;
                                table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                                table1forpage1.Columns[0].SetWidth(30);
                                table1forpage1.Columns[1].SetWidth(190);
                                table1forpage1.Columns[2].SetWidth(50);
                                table1forpage1.Columns[3].SetWidth(50);
                                table1forpage1.Columns[4].SetWidth(80);
                                // table1forpage1.Columns[5].SetWidth(65);
                                table1forpage1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(0, 0).SetContent("S.No.");
                                // table1forpage1.Cell(0, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(0, 1).SetContent("Item Name");
                                table1forpage1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage1.Cell(0, 2).SetContent("Quantity");
                                // table1forpage1.Cell(0, 2).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage1.Cell(0, 3).SetContent("Price");
                                table1forpage1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage1.Cell(0, 4).SetContent("Amount");
                                binddatatb = page2col + 17;
                                Gios.Pdf.PdfTablePage newpdftabpage3 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 50, page2col, 500, 400));
                                mypdfpage.Add(newpdftabpage3);
                                page2col = page2col - 10;
                                collinfo1 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 50, page2col, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "-------------------------------------------------------------------------------------------------------------------------------------------------");
                                mypdfpage.Add(collinfo1);
                            }
                            #endregion

                            #region
                            // table1forpage1datas.Columns[5].SetWidth(65);
                            table1forpage1datas.Cell(j, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage1datas.Cell(j, 0).SetContent(j + 1);
                            // table1forpage1datas.Cell(0, 0).SetFont(Fontsmall1bold);
                            table1forpage1datas.Cell(j, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage1datas.Cell(j, 1).SetContent(dv[j]["ItemName"].ToString());
                            table1forpage1datas.Cell(j, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage1datas.Cell(j, 2).SetContent(dv[j]["App_Qty"].ToString());
                            // table1forpage1datas.Cell(0, 2).SetFont(Fontsmall1bold);
                            table1forpage1datas.Cell(j, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage1datas.Cell(j, 3).SetContent(dv[j]["RPU"].ToString());
                            table1forpage1datas.Cell(j, 4).SetContentAlignment(ContentAlignment.MiddleRight);
                            table1forpage1datas.Cell(j, 4).SetContent(dv[j]["Amount"].ToString());
                            double cc = 0;
                            if (double.TryParse(dv[j]["Amount"].ToString(), out cc))
                            {
                                totalamt = totalamt + Convert.ToDouble(dv[j]["Amount"].ToString());
                            }
                            if (double.TryParse(dv[j]["TotTaxAmt"].ToString(), out cc))
                            {
                                totaltax = Convert.ToDouble(dv[j]["TotTaxAmt"].ToString());
                            }
                            if (double.TryParse(dv[j]["TotOtherChgAmt"].ToString(), out cc))
                            {
                                totalcharges = Convert.ToDouble(dv[j]["TotOtherChgAmt"].ToString());
                            }
                            if (double.TryParse(dv[j]["OrderDescription"].ToString(), out cc))
                            {
                                totaldiscnt = Convert.ToDouble(dv[j]["OrderDescription"].ToString());
                            }

                            page2col = page2col + 16;
                            collinfo1 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Gray, new PdfArea(mydoc, 50, page2col, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "-------------------------------------------------------------------------------------------------------------------------------------------------");
                            mypdfpage.Add(collinfo1);

                        }

                        Gios.Pdf.PdfTablePage newpdftabpage4 = table1forpage1datas.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 50, binddatatb, 480, 400));
                        mypdfpage.Add(newpdftabpage4);
                            #endregion

                        #region
                        binddatatb = binddatatb + newpdftabpage4.Area.Height;
                        table1forpage1 = mydoc.NewTable(Fontsmall1, 5, 3, 2);
                        table1forpage1.VisibleHeaders = false;
                        table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                        table1forpage1.Columns[0].SetWidth(180);
                        table1forpage1.Columns[1].SetWidth(20);
                        table1forpage1.Columns[2].SetWidth(50);

                        // table1forpage1.Columns[5].SetWidth(65);
                        table1forpage1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                        table1forpage1.Cell(0, 0).SetContent("Sub Total");
                        table1forpage1.Cell(0, 0).SetFont(Fontsmall1bold);
                        table1forpage1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage1.Cell(0, 1).SetContent(":");
                        table1forpage1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                        table1forpage1.Cell(0, 2).SetContent(totalamt);
                        table1forpage1.Cell(0, 2).SetFont(Fontsmall1bold);

                        totaltax = (totalamt * totaltax) / 100;
                        totalamt = totalamt + totaltax;

                        table1forpage1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                        table1forpage1.Cell(1, 0).SetContent("Discount / Deductions (-)");
                        table1forpage1.Cell(1, 0).SetFont(Fontsmall1bold);
                        table1forpage1.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage1.Cell(1, 1).SetContent(":");
                        table1forpage1.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                        table1forpage1.Cell(1, 2).SetContent(totaldiscnt);
                        table1forpage1.Cell(1, 2).SetFont(Fontsmall1bold);
                        totalnet = (totalamt + totalcharges) - totaldiscnt;


                        table1forpage1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                        table1forpage1.Cell(2, 0).SetContent("Tax (+)");
                        table1forpage1.Cell(2, 0).SetFont(Fontsmall1bold);
                        table1forpage1.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage1.Cell(2, 1).SetContent(":");
                        table1forpage1.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                        table1forpage1.Cell(2, 2).SetContent(totaltax);
                        table1forpage1.Cell(2, 2).SetFont(Fontsmall1bold);

                        table1forpage1.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                        table1forpage1.Cell(3, 0).SetContent("Charges (+)");
                        table1forpage1.Cell(3, 0).SetFont(Fontsmall1bold);
                        table1forpage1.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage1.Cell(3, 1).SetContent(":");
                        table1forpage1.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                        table1forpage1.Cell(3, 2).SetContent(totalcharges);
                        table1forpage1.Cell(3, 2).SetFont(Fontsmall1bold);

                        table1forpage1.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                        table1forpage1.Cell(4, 0).SetContent("Net Value");
                        table1forpage1.Cell(4, 0).SetFont(Fontsmall1bold);
                        table1forpage1.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage1.Cell(4, 1).SetContent(":");
                        table1forpage1.Cell(4, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                        table1forpage1.Cell(4, 2).SetContent(totalnet);
                        table1forpage1.Cell(4, 2).SetFont(Fontsmall1bold);
                        Gios.Pdf.PdfTablePage newpdftabpage5 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 320, binddatatb, 210, 400));
                        mypdfpage.Add(newpdftabpage5);

                        binddatatb = binddatatb + newpdftabpage5.Area.Height;
                        collinfo1 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 50, binddatatb, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Amount in Words  :");
                        mypdfpage.Add(collinfo1);
                        binddatatb = binddatatb + 8;
                        collinfo1 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 50, binddatatb, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "---------------------------");
                        mypdfpage.Add(collinfo1);

                        binddatatb = binddatatb + 15;
                        string amtword = ConvertNumbertoWords(Convert.ToInt32(totalnet));
                        collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 50, binddatatb, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "RUPEES " + amtword + " ONLY");
                        mypdfpage.Add(collinfo1);
                        #endregion
                        if (dv.Count > 7)
                        {
                            #region
                            mypdfpage.SaveToDocument();
                            mypdfpage = mydoc.NewPage();
                            coltop = 0;
                            strquery = "Select * from Collinfo where college_code=" + Session["collegecode"].ToString() + "";
                            ds1.Clear();
                            ds1 = da.select_method_wo_parameter(strquery, "Text");
                            collegedetails = da.GetFunction("select college_details from tbl_print_master_settings where page_Name='Investorsposetting.aspx'");
                            spiltcollegedetails = collegedetails.Split('#');
                            for (int i = 0; i <= spiltcollegedetails.GetUpperBound(0); i++)
                            {
                                coltop = coltop + 15;
                                string collinfo = spiltcollegedetails[i].ToString();
                                if (collinfo == "College Name")
                                {
                                    collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds1.Tables[0].Rows[0]["collname"].ToString() + "");
                                    mypdfpage.Add(collinfo1);

                                }
                                else if (collinfo == "University")
                                {
                                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds1.Tables[0].Rows[0]["university"].ToString() + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (collinfo == "Affliated By")
                                {
                                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds1.Tables[0].Rows[0]["affliatedby"].ToString() + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (collinfo == "Address")
                                {
                                    string address1 = ds1.Tables[0].Rows[0]["Address1"].ToString();
                                    string address2 = ds1.Tables[0].Rows[0]["Address2"].ToString();
                                    string address3 = ds1.Tables[0].Rows[0]["Address3"].ToString();
                                    if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                    {
                                        Collvalue = address1;
                                    }
                                    if (address2.Trim() != "" && address2 != null && address2.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + ',' + address2;
                                        }
                                        else
                                        {
                                            Collvalue = address2;
                                        }
                                    }
                                    if (address3.Trim() != "" && address3 != null && address3.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + ',' + address3;
                                        }
                                        else
                                        {
                                            Collvalue = address3;
                                        }
                                    }
                                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (collinfo == "City")
                                {
                                    string address1 = ds1.Tables[0].Rows[0]["Address3"].ToString();
                                    if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                    {
                                        Collvalue = address1;
                                    }
                                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (collinfo == "District & State & Pincode")
                                {
                                    string district = ds1.Tables[0].Rows[0]["district"].ToString();
                                    string state = ds1.Tables[0].Rows[0]["State"].ToString();
                                    string pincode = ds1.Tables[0].Rows[0]["Pincode"].ToString();
                                    if (district.Trim() != "" && district != null && district.Length > 1)
                                    {
                                        Collvalue = district;
                                    }
                                    if (state.Trim() != "" && state != null && state.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + ',' + state;
                                        }
                                        else
                                        {
                                            Collvalue = state;
                                        }
                                    }
                                    if (pincode.Trim() != "" && pincode != null && pincode.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + '-' + pincode;
                                        }
                                        else
                                        {
                                            Collvalue = pincode;
                                        }
                                    }
                                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (collinfo == "Phone No & Fax")
                                {
                                    string phone = ds1.Tables[0].Rows[0]["Phoneno"].ToString();
                                    string fax = ds1.Tables[0].Rows[0]["Faxno"].ToString();
                                    if (phone.Trim() != "" && phone != null && phone.Length > 1)
                                    {
                                        Collvalue = "Phone :" + phone;
                                    }
                                    if (fax.Trim() != "" && fax != null && fax.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + " , Fax : " + fax;
                                        }
                                        else
                                        {
                                            Collvalue = "Fax :" + fax;
                                        }
                                    }

                                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (collinfo == "Email & Web Site")
                                {
                                    string email = ds1.Tables[0].Rows[0]["Email"].ToString();
                                    string website = ds1.Tables[0].Rows[0]["Website"].ToString();
                                    if (email.Trim() != "" && email != null && email.Length > 1)
                                    {
                                        Collvalue = "Email :" + email;
                                    }
                                    if (website.Trim() != "" && website != null && website.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + " , Web Site : " + website;
                                        }
                                        else
                                        {
                                            Collvalue = "Web Site :" + website;
                                        }
                                    }
                                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (collinfo == "Left Logo")
                                {
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                    {
                                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));

                                        mypdfpage.Add(LogoImage, 25, 25, 400);

                                    }
                                }
                                else if (collinfo == "Right Logo")
                                {
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                    {
                                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                        mypdfpage.Add(LogoImage, 480, 25, 400);

                                    }
                                }

                            }
                            #endregion
                            binddatatb = 150;
                        }
                        binddatatb = binddatatb + 20;
                        collinfo1 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 50, binddatatb, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Terms and Conditions  :");
                        mypdfpage.Add(collinfo1);
                        binddatatb = binddatatb + 8;
                        collinfo1 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 50, binddatatb, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "---------------------------------");
                        mypdfpage.Add(collinfo1);
                        Gios.Pdf.PdfTablePage newpdftabpage6 = null;
                        #region
                        string istrm = "";
                        string trmdesc = "";
                        string[] spilttrmdesc;
                        string againtrmdesc = "";

                        sql = "select * from IM_POSettings where collegecode='" + Session["collegecode"].ToString() + "'";

                        dsterms.Clear();
                        dsterms = da.select_method_wo_parameter(sql, "Text");
                        if (dsterms.Tables[0].Rows.Count > 0)
                        {
                            DataSet staffmaster = new DataSet();
                            staffmaster.Clear();
                            staffmaster = da.select_method_wo_parameter("select staff_code,staff_name from staffmaster", "Text");

                            for (int i = 0; i < dsterms.Tables[0].Rows.Count; i++)
                            {

                                istrm = dsterms.Tables[0].Rows[0]["IsTerms"].ToString();
                                if (istrm.Trim() == "True")
                                {
                                    trmdesc = dsterms.Tables[0].Rows[0]["TermsDesc"].ToString();
                                    if (trmdesc != "")
                                    {
                                        spilttrmdesc = trmdesc.Split(';');
                                        if (spilttrmdesc.Length > 0)
                                        {
                                            table1forpage1datas = mydoc.NewTable(Fontsmall1, spilttrmdesc.Length, 2, 3);
                                            table1forpage1datas.VisibleHeaders = false;
                                            table1forpage1datas.SetBorders(Color.Black, 1, BorderType.None);
                                            table1forpage1datas.Columns[0].SetWidth(2);
                                            //table1forpage1datas.Columns[1].SetWidth(300);
                                            for (int j = 0; j < spilttrmdesc.Length; j++)
                                            {
                                                againtrmdesc = spilttrmdesc[j].ToString();

                                                table1forpage1datas.Cell(j, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1forpage1datas.Cell(j, 0).SetContent(j + 1 + ".");
                                                // table1forpage1datas.Cell(0, 0).SetFont(Fontsmall1bold);
                                                table1forpage1datas.Cell(j, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1forpage1datas.Cell(j, 1).SetContent(againtrmdesc);
                                                //dt.Rows.Add(againtrmdesc);



                                            }

                                            binddatatb = binddatatb + 30;
                                            newpdftabpage6 = table1forpage1datas.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 50, binddatatb, 500, 400));
                                            mypdfpage.Add(newpdftabpage6);
                                            binddatatb = newpdftabpage6.Area.Height + binddatatb + 10;
                                        }

                                    }
                                }
                            }
                            //binddatatb = binddatatb + 30;
                            //Gios.Pdf.PdfTablePage newpdftabpage6 = table1forpage1datas.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 50, binddatatb, 500, 400));
                            //mypdfpage.Add(newpdftabpage6);
                        #endregion

                            if (binddatatb > 621)
                            {
                                #region
                                mypdfpage.SaveToDocument();
                                mypdfpage = mydoc.NewPage();
                                coltop = 0;
                                strquery = "Select * from Collinfo where college_code=" + Session["collegecode"].ToString() + "";
                                ds1.Clear();
                                ds1 = da.select_method_wo_parameter(strquery, "Text");
                                collegedetails = da.GetFunction("select college_details from tbl_print_master_settings where page_Name='Investorsposetting.aspx'");
                                spiltcollegedetails = collegedetails.Split('#');
                                for (int i = 0; i <= spiltcollegedetails.GetUpperBound(0); i++)
                                {
                                    coltop = coltop + 15;
                                    string collinfo = spiltcollegedetails[i].ToString();
                                    if (collinfo == "College Name")
                                    {
                                        collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds1.Tables[0].Rows[0]["collname"].ToString() + "");
                                        mypdfpage.Add(collinfo1);

                                    }
                                    else if (collinfo == "University")
                                    {
                                        collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds1.Tables[0].Rows[0]["university"].ToString() + "");
                                        mypdfpage.Add(collinfo1);
                                    }
                                    else if (collinfo == "Affliated By")
                                    {
                                        collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds1.Tables[0].Rows[0]["affliatedby"].ToString() + "");
                                        mypdfpage.Add(collinfo1);
                                    }
                                    else if (collinfo == "Address")
                                    {
                                        string address1 = ds1.Tables[0].Rows[0]["Address1"].ToString();
                                        string address2 = ds1.Tables[0].Rows[0]["Address2"].ToString();
                                        string address3 = ds1.Tables[0].Rows[0]["Address3"].ToString();
                                        if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                        {
                                            Collvalue = address1;
                                        }
                                        if (address2.Trim() != "" && address2 != null && address2.Length > 1)
                                        {
                                            if (Collvalue.Trim() != "" && Collvalue != null)
                                            {
                                                Collvalue = Collvalue + ',' + address2;
                                            }
                                            else
                                            {
                                                Collvalue = address2;
                                            }
                                        }
                                        if (address3.Trim() != "" && address3 != null && address3.Length > 1)
                                        {
                                            if (Collvalue.Trim() != "" && Collvalue != null)
                                            {
                                                Collvalue = Collvalue + ',' + address3;
                                            }
                                            else
                                            {
                                                Collvalue = address3;
                                            }
                                        }
                                        collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                        mypdfpage.Add(collinfo1);
                                    }
                                    else if (collinfo == "City")
                                    {
                                        string address1 = ds1.Tables[0].Rows[0]["Address3"].ToString();
                                        if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                        {
                                            Collvalue = address1;
                                        }
                                        collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                        mypdfpage.Add(collinfo1);
                                    }
                                    else if (collinfo == "District & State & Pincode")
                                    {
                                        string district = ds1.Tables[0].Rows[0]["district"].ToString();
                                        string state = ds1.Tables[0].Rows[0]["State"].ToString();
                                        string pincode = ds1.Tables[0].Rows[0]["Pincode"].ToString();
                                        if (district.Trim() != "" && district != null && district.Length > 1)
                                        {
                                            Collvalue = district;
                                        }
                                        if (state.Trim() != "" && state != null && state.Length > 1)
                                        {
                                            if (Collvalue.Trim() != "" && Collvalue != null)
                                            {
                                                Collvalue = Collvalue + ',' + state;
                                            }
                                            else
                                            {
                                                Collvalue = state;
                                            }
                                        }
                                        if (pincode.Trim() != "" && pincode != null && pincode.Length > 1)
                                        {
                                            if (Collvalue.Trim() != "" && Collvalue != null)
                                            {
                                                Collvalue = Collvalue + '-' + pincode;
                                            }
                                            else
                                            {
                                                Collvalue = pincode;
                                            }
                                        }
                                        collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                        mypdfpage.Add(collinfo1);
                                    }
                                    else if (collinfo == "Phone No & Fax")
                                    {
                                        string phone = ds1.Tables[0].Rows[0]["Phoneno"].ToString();
                                        string fax = ds1.Tables[0].Rows[0]["Faxno"].ToString();
                                        if (phone.Trim() != "" && phone != null && phone.Length > 1)
                                        {
                                            Collvalue = "Phone :" + phone;
                                        }
                                        if (fax.Trim() != "" && fax != null && fax.Length > 1)
                                        {
                                            if (Collvalue.Trim() != "" && Collvalue != null)
                                            {
                                                Collvalue = Collvalue + " , Fax : " + fax;
                                            }
                                            else
                                            {
                                                Collvalue = "Fax :" + fax;
                                            }
                                        }

                                        collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                        mypdfpage.Add(collinfo1);
                                    }
                                    else if (collinfo == "Email & Web Site")
                                    {
                                        string email = ds1.Tables[0].Rows[0]["Email"].ToString();
                                        string website = ds1.Tables[0].Rows[0]["Website"].ToString();
                                        if (email.Trim() != "" && email != null && email.Length > 1)
                                        {
                                            Collvalue = "Email :" + email;
                                        }
                                        if (website.Trim() != "" && website != null && website.Length > 1)
                                        {
                                            if (Collvalue.Trim() != "" && Collvalue != null)
                                            {
                                                Collvalue = Collvalue + " , Web Site : " + website;
                                            }
                                            else
                                            {
                                                Collvalue = "Web Site :" + website;
                                            }
                                        }
                                        collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                        mypdfpage.Add(collinfo1);
                                    }
                                    else if (collinfo == "Left Logo")
                                    {
                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                        {
                                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));

                                            mypdfpage.Add(LogoImage, 25, 25, 400);

                                        }
                                    }
                                    else if (collinfo == "Right Logo")
                                    {
                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                        {
                                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                            mypdfpage.Add(LogoImage, 480, 25, 400);

                                        }
                                    }

                                }
                                #endregion
                                binddatatb = 580;
                            }


                            collinfo1 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, binddatatb, 160, 50), System.Drawing.ContentAlignment.TopCenter, "Delivery At :");
                            mypdfpage.Add(collinfo1);//added by rajasekar 27/07/2018

                            collinfo1 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, binddatatb, 595, 50), System.Drawing.ContentAlignment.TopCenter, "Thanking You");
                            mypdfpage.Add(collinfo1);
                            //strquery = "";
                            // strquery = da.GetFunction("Select collname from Collinfo where college_code=" + Session["collegecode"].ToString() + "");

                            binddatatb = binddatatb + 3;

                            //collinfo1 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 50, binddatatb, 595, 50), System.Drawing.ContentAlignment.MiddleLeft,"For  " +strquery);
                            //mypdfpage.Add(collinfo1);

                            istrm = "";
                            istrm = dsterms.Tables[0].Rows[0]["AddressDesc"].ToString();
                            if (istrm.Trim() != "")
                            {


                                //collinfo1 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 50, 770, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "-------------------------------------------------------------------------------------------------------------------------------------------------");
                                //mypdfpage.Add(collinfo1);

                                collinfo1 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 50, binddatatb, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "" + istrm + " ");
                                mypdfpage.Add(collinfo1);
                            }

                            binddatatb = binddatatb + 12;
                            collinfo1 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Gray, new PdfArea(mydoc, 50, binddatatb, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "-------------------------------------------------------------------------------------------------------------------------------------------------");
                            mypdfpage.Add(collinfo1);

                            binddatatb = binddatatb + 42;
                            collinfo1 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, binddatatb, 150, 50), System.Drawing.ContentAlignment.TopCenter, "Purpose :");
                            mypdfpage.Add(collinfo1);//added by rajasekar 27/07/2018


                            for (int i = 0; i < dsterms.Tables[0].Rows.Count; i++)
                            {

                                istrm = dsterms.Tables[0].Rows[0]["IsFooterDesc"].ToString();
                                if (istrm.Trim() == "True")
                                {
                                    trmdesc = dsterms.Tables[0].Rows[0]["FooterDescStaff"].ToString();
                                    if (trmdesc != "")
                                    {
                                        spilttrmdesc = trmdesc.Split(';');
                                        table1forpage1datas = mydoc.NewTable(Fontsmall1, 3, spilttrmdesc.Length, 3);
                                        table1forpage1datas.VisibleHeaders = false;
                                        table1forpage1datas.SetBorders(Color.Black, 1, BorderType.None);
                                        table1forpage1datas.Cell(1, 0).SetCellPadding(15);
                                        for (int j = 0; j < spilttrmdesc.Length; j++)
                                        {
                                            againtrmdesc = spilttrmdesc[j].ToString();
                                            string[] spiltagaintrmdesc = againtrmdesc.Split('-');


                                            table1forpage1datas.Cell(0, j).SetContent(spiltagaintrmdesc[0]);
                                            table1forpage1datas.Cell(0, j).SetContentAlignment(ContentAlignment.TopLeft);
                                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].Text = Convert.ToString(spiltagaintrmdesc[0]);
                                            if (staffmaster.Tables[0].Rows.Count > 0)
                                            {
                                                staffmaster.Tables[0].DefaultView.RowFilter = "staff_code='" + spiltagaintrmdesc[1] + "'";
                                                dv = staffmaster.Tables[0].DefaultView;
                                                if (dv.Count > 0)
                                                {

                                                    table1forpage1datas.Cell(2, j).SetContent(Convert.ToString(dv[0][1].ToString().Trim()));
                                                    table1forpage1datas.Cell(2, j).SetContentAlignment(ContentAlignment.TopLeft);
                                                    // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Text = Convert.ToString(dv[0][1].ToString());
                                                }
                                            }


                                        }
                                        table1forpage1datas.Cell(2, 0).SetContentAlignment(ContentAlignment.BottomLeft);
                                    }

                                    binddatatb = binddatatb + 35;
                                    newpdftabpage6 = table1forpage1datas.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 50, binddatatb, 500, 400));
                                    mypdfpage.Add(newpdftabpage6);

                                    binddatatb = newpdftabpage6.Area.Height + binddatatb - 15;
                                    collinfo1 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 50, binddatatb, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Vendor : ");
                                    mypdfpage.Add(collinfo1);
                                    binddatatb = binddatatb + 12;
                                    collinfo1 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Gray, new PdfArea(mydoc, 50, binddatatb, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "-------------------------------------------------------------------------------------------------------------------------------------------------");
                                    mypdfpage.Add(collinfo1);

                                    binddatatb = binddatatb + 12;
                                    collinfo1 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 50, binddatatb, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Order acceptance : ");
                                    mypdfpage.Add(collinfo1);

                                    istrm = dsterms.Tables[0].Rows[0]["IsSignwithSeal"].ToString();
                                    if (istrm.Trim() == "True")
                                    {
                                        binddatatb = binddatatb + 30;

                                        collinfo1 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 50, binddatatb, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature with Seal ");
                                        mypdfpage.Add(collinfo1);
                                    }

                                }
                            }
                        }
                    }
                }
            }

            mypdfpage.SaveToDocument();
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "PurchaseOrderItems" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                Response.Buffer = true;
                Response.Clear();
                mydoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);

            }
        }
    }
    public static string ConvertNumbertoWords(int number)
    {
        if (number == 0)
            return "ZERO";
        if (number < 0)
            return "minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";
        if ((number / 1000000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000000) + " MILLION ";
            number %= 1000000;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
            number %= 100;
        }
        if (number > 0)
        {
            if (words != "")
                words += "AND ";
            var unitsMap = new[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };
            var tensMap = new[] { "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }
        return words;
    }

    protected void Rad_item_Click(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        lbl_error.Visible = false;
    }

    protected void Rad_book_Click(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        lbl_error.Visible = false;
    }
}