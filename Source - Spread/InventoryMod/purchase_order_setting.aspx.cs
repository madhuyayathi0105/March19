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

public partial class purchase_order_setting : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();

    bool check = false;
    Boolean Cellclick = false;
    string dtaccessdate = DateTime.Now.ToString();
    string dtaccesstime = DateTime.Now.ToLongTimeString();

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
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
                rdo_mess.Checked = true;
                rdbpopmess.Checked = true;
                rdbpopmess.Checked = true;
                bindcollege();

                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.Visible = false;
            }
        }
        catch (Exception ex) { }
    }
    protected void lnk_btnlogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }

    protected void btn_save_Click(object sender, EventArgs e)
    {


        string ms1 = "";
        if (rdbpopmess.Checked == true)
        {
            ms1 = "1";
        }
        if (rdbpopinv.Checked == true)
        {
            ms1 = "0";
        }
        if (rdbpoplib.Checked == true)
        {
            ms1 = "2";
        }
        string chk = "";
        if (rd_user_approval.Checked == true)
        {
            chk = "1";

        }
        if (rd_requestapproved.Checked == true)
        {
            chk = "2";

        }
        if (rd_direct_po.Checked == true)
        {

            chk = "3";
        }
        if (rd_request_po.Checked == true)
        {

            chk = "4";
        }
        if (rd_quatation_po.Checked == true)
        {

            chk = "5";
        }
        if (ms1.Trim() != "" && chk.Trim() != "")
        {
            string p1 = "";

            if (chk_purchase.Checked == true)
            {
                p1 = "1";
            }
            if (chk_service.Checked == true)
            {
                p1 = "2";
            }
            if (chk_purchase.Checked && chk_service.Checked == true)
            {
                p1 = "3";
            }

            string dt = txt_fromdate.Text;
            string yearend = txt_todate.Text;
            string[] Split = dt.Split('/');
            DateTime todate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);

            Split = yearend.Split('/');
            DateTime newdt = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
            string query = "";
            query = "if exists (select * from IM_POSettings where SettingForMess ='" + ms1 + "' and SettingType='" + 1 + "' and CollegeCode ='" + collegecode1 + "') update IM_POSettings set PurchaseOrderType='" + chk + "',SettingFromDate='" + todate.ToString("MM/dd/yyyy") + "', SettingToDate='" + newdt.ToString("MM/dd/yyyy") + "', SettingFor='" + p1 + "' where SettingForMess ='" + ms1 + "' and SettingType='1' and CollegeCode ='" + collegecode1 + "' else INSERT INTO IM_POSettings(SettingType,PurchaseOrderType,SettingForMess,SettingFromDate,SettingToDate,SettingFor,CollegeCode)values('1','" + chk + "','" + ms1 + "','" + todate.ToString("MM/dd/yyyy") + "','" + newdt.ToString("MM/dd/yyyy") + "','" + p1 + "','" + collegecode1 + "')";

            ds.Clear();
            int count = d2.update_method_wo_parameter(query, "Text");
            alertpopwindow.Visible = true;
            lblalerterr.Text = "Saved Successfully ";
            btn_go_Click(sender, e);
        }
        else
        {
            alertpopwindow.Visible = true;
            lblalerterr.Text = "Please Select Purchase Order Setting Type ";
        }
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        //rdo_inventory.Checked = false;
        //rdo_mess.Checked = false;
        rd_direct_po.Checked = false;
        chk_purchase.Checked = false;
        rd_quatation_po.Checked = false;
        rd_request_po.Checked = false;
        rd_requestapproved.Checked = false;
        chk_service.Checked = false;
        rd_user_approval.Checked = false;
        alertpopwindow.Visible = false;
        divPopper.Visible = false;

    }
    protected void btn_new_Click(object sender, EventArgs e)
    {
        rdbpopmess.Enabled = true;
        rdbpopinv.Enabled = true;

        txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_fromdate.Attributes.Add("readonly", "readonly");
        txt_todate.Attributes.Add("readonly", "readonly");

        btn_update.Visible = false;
        btn_delete.Visible = false;
        btn_save.Visible = true;
        btn_exit.Visible = true;
        divPopper.Visible = true;
        bindcollege1();

    }

    protected void imagebtnpopclose1_Click(object sender, ImageClickEventArgs e)
    {

        if (rdbpopmess.Checked == true)
        {

            rdo_mess.Checked = true;
        }
        if (rdbpopinv.Checked == true)
        {
            rdo_inventory.Checked = true;
        }


        divPopper.Visible = false;
        // rdo_mess.Checked = true;
        rd_direct_po.Checked = false;
        rd_quatation_po.Checked = false;
        rd_request_po.Checked = false;
        rd_requestapproved.Checked = false;
        rd_user_approval.Checked = false;
        rdbpopinv.Checked = false;
        rdbpopmess.Checked = false;
        chk_purchase.Checked = false;
        chk_service.Checked = false;

    }
    protected void btn_exit_Click(object sender, EventArgs e)
    {
        rdo_mess.Checked = true;
        divPopper.Visible = false;
    }



    protected void btn_go_Click(object sender, EventArgs e)
    {

        string ms1 = "";
        if (rdo_mess.Checked == true)
        {
            ms1 = "1";
        }
        if (rdo_inventory.Checked == true)
        {
            ms1 = "0";
        }
        if (rdo_Library.Checked == true)
        {
            ms1 = "2";
        }
        string selectquery1 = "select SettingType, case when PurchaseOrderType=1 then 'User approved vendor for indiviual Item' when PurchaseOrderType=2 then 'Select vendor for request and Approved' when PurchaseOrderType=3 then 'User direct purchase order' when PurchaseOrderType=4 then 'Use request and purchase order' when PurchaseOrderType=5 then 'Use quotation and purchase order' end as PurchaseOrderType1,PurchaseOrderType,case when SettingForMess=0 then 'Inventory' when SettingForMess=1 then 'Mess' when SettingForMess=2 then 'Library'  end as SettingForMess1,SettingForMess,convert(varchar(10), SettingFromDate,103) as SettingFromDate,convert(varchar(10), SettingToDate,103) as SettingToDate,case when SettingFor=1 then 'Purchase' when SettingFor=2 then 'Service' when SettingFor=3 then 'Both' end as SettingFor1,SettingFor,CollegeCode from IM_POSettings where CollegeCode=('" + collegecode1 + "') and SettingForMess=('" + ms1 + "') ";
        ds.Clear();
        ds = d2.select_method_wo_parameter(selectquery1, "Text");
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
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Columns[0].Width = 50;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Mess/Inventory/Library";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "PurchaseOrderType";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Purchase/Service";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "From Date";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "To Date";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;


            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[row]["CollegeCode"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["SettingForMess1"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["SettingForMess"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["PurchaseOrderType1"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["PurchaseOrderType"]);

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["SettingFor1"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[row]["SettingFor"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";


                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["SettingFromDate"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";


                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["SettingToDate"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
            }
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            spreaddiv.Visible = true;
            FpSpread1.Visible = true;
            errorlable.Visible = false;
            bindcollege1();
        }
        else
        {
            spreaddiv.Visible = false;
            FpSpread1.Visible = false;
            errorlable.Visible = true;
        }
    }

    protected void FpSpread1_render(object sender, EventArgs e)
    {


        if (check == true)
        {
            rdbpopmess.Enabled = false;
            rdbpopinv.Enabled = false;
            string activerow = "";
            btn_save.Visible = false;
            string activecol = "";
            activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
            // collegecode = Session["collegecode"].ToString();
            bindcollege1();
            if (activerow.Trim() != "")
            {
                string clname = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                string settingformess = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                string purchaseot = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                string settingfor = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag);
                string sfromdate = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
                string stodate = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);

                ddl_clg.Text = Convert.ToString(clname);
                if (purchaseot == "1")
                {
                    rd_user_approval.Checked = true;

                }
                if (purchaseot == "2")
                {
                    rd_requestapproved.Checked = true;

                }
                if (purchaseot == "3")
                {
                    rd_direct_po.Checked = true;

                }
                if (purchaseot == "4")
                {
                    rd_request_po.Checked = true;

                }
                if (purchaseot == "5")
                {
                    rd_quatation_po.Checked = true;

                }


                txt_fromdate.Text = Convert.ToString(sfromdate);
                txt_todate.Text = Convert.ToString(stodate);

                if (rdo_mess.Checked == true)
                {
                    rdbpopmess.Checked = true;
                }
                if (rdo_inventory.Checked == true)
                {
                    rdbpopinv.Checked = true;
                }
                if (rdo_Library.Checked == true)
                {
                    rdbpoplib.Checked = true;
                }



                if (settingfor == "1")
                {
                    chk_purchase.Checked = true;
                }
                if (settingfor == "2")
                {
                    chk_service.Checked = true;
                }
                if (settingfor == "3")
                {
                    chk_service.Checked = true;
                    chk_purchase.Checked = true;
                }
            }

            btn_update.Visible = true;
            btn_exit.Visible = true;
            btn_delete.Visible = true;
            DataView dv1 = new DataView();

        }

    }


    protected void btn_update_Click(object sender, EventArgs e)
    {
        string chk = "";
        if (rd_user_approval.Checked == true)
        {
            chk = "1";

        }
        if (rd_requestapproved.Checked == true)
        {
            chk = "2";

        }
        if (rd_direct_po.Checked == true)
        {

            chk = "3";

        }
        if (rd_request_po.Checked == true)
        {

            chk = "4";

        }
        if (rd_quatation_po.Checked == true)
        {

            chk = "5";

        }
        string s1 = "";

        if (chk_purchase.Checked == true)
        {
            s1 = "1";
        }
        if (chk_service.Checked == true)
        {
            s1 = "2";
        }
        if (chk_purchase.Checked && chk_service.Checked == true)
        {
            s1 = "3";
        }
        string del = "";
        string ms1 = "";
        if (rdo_mess.Checked == true)
        {
            ms1 = "1";
        }
        if (rdo_inventory.Checked == true)
        {
            ms1 = "0";
        }
        if (rdo_Library.Checked == true)
        {
            ms1 = "2";
        }
        string dt = txt_fromdate.Text;
        string yearend = txt_todate.Text;
        string[] Split = dt.Split('/');
        DateTime todate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);


        Split = yearend.Split('/');
        DateTime newdt = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
        string inserquery = "if exists (select * from IM_POSettings where SettingForMess ='" + ms1 + "' and SettingType='" + 1 + "' and CollegeCode ='" + collegecode1 + "') update IM_POSettings set PurchaseOrderType='" + chk + "',SettingFromDate='" + todate.ToString("MM/dd/yyyy") + "', SettingToDate='" + newdt.ToString("MM/dd/yyyy") + "', SettingFor='" + s1 + "' where SettingForMess ='" + ms1 + "' and SettingType='1' and CollegeCode ='" + collegecode1 + "' else INSERT INTO IM_POSettings(SettingType,PurchaseOrderType,SettingForMess,SettingFromDate,SettingToDate,SettingFor,CollegeCode)values('1','" + chk + "','" + ms1 + "','" + todate.ToString("MM/dd/yyyy") + "','" + newdt.ToString("MM/dd/yyyy") + "','" + s1 + "','" + collegecode1 + "')";
        int ins = d2.update_method_wo_parameter(inserquery, "Text");
        if (ins != 0)
        {
            alertpopwindow.Visible = true;
            lblalerterr.Visible = true;
            lblalerterr.Text = "Updated Successfully";
            btn_go_Click(sender, e);
            // divPopper.Visible = false;
        }

    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
        btn_go_Click(sender, e);
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        lblalerterr.Visible = false;
        Div3.Visible = true;
    }
    protected void delete()
    {
        string chk = "";
        if (rd_user_approval.Checked == true)
        {
            chk = "1";

        }
        if (rd_requestapproved.Checked == true)
        {
            chk = "2";

        }
        if (rd_direct_po.Checked == true)
        {

            chk = "3";

        }
        if (rd_request_po.Checked == true)
        {

            chk = "4";

        }
        if (rd_quatation_po.Checked == true)
        {

            chk = "5";

        }
        string s1 = "";

        if (chk_purchase.Checked == true)
        {
            s1 = "1";
        }
        if (chk_service.Checked == true)
        {
            s1 = "2";
        }
        if (chk_purchase.Checked && chk_service.Checked == true)
        {
            s1 = "3";
        }
        string del = "";
        string ms1 = "";
        if (rdo_mess.Checked == true)
        {
            ms1 = "1";
        }
        if (rdo_inventory.Checked == true)
        {
            ms1 = "0";
        }
        if (rdo_Library.Checked == true)
        {
            ms1 = "2";
        }
        string dt = txt_fromdate.Text;
        string yearend = txt_todate.Text;
        string[] Split = dt.Split('/');
        DateTime todate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);

        Split = yearend.Split('/');
        DateTime newdt = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
        del = "delete from IM_POSettings where CollegeCode='" + collegecode1 + "' and SettingFor='" + s1 + "' and SettingFromDate='" + todate.ToString("MM/dd/yyyy") + "' and SettingToDate='" + newdt.ToString("MM/dd/yyyy") + "' and PurchaseOrderType='" + chk + "'";
        int y = d2.update_method_wo_parameter(del, "Text");
        if (y != 0)
        {
            surediv.Visible = false;
            alertpopwindow.Visible = true;
            lblalerterr.Text = "Deleted Successfully ";
        }

    }
    protected void btn_delete_Click(object sender, EventArgs e)
    {
        if (btn_delete.Text == "Delete")
        {
            surediv.Visible = true;
            lbl_sure.Text = "Do you want to Delete this Record?";
        }
        btn_go_Click(sender, e);
    }
    protected void Cell_Click(object sender, EventArgs e)
    {
        try
        {
            check = true;
            divPopper.Visible = true;
        }
        catch
        {

        }
    }
    public void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
    }


    protected void bindcollege()
    {
        try
        {
            ds.Clear();
            ddl_college.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
            }
        }
        catch
        {
        }
    }
    public void ddl_clg_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void bindcollege1()
    {
        try
        {
            ds.Clear();
            ddl_clg.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_clg.DataSource = ds;
                ddl_clg.DataTextField = "collname";
                ddl_clg.DataValueField = "college_code";
                ddl_clg.DataBind();
            }
        }
        catch
        {
        }
    }
    public string stodate { get; set; }

    public string sfromdate { get; set; }
}