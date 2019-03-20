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
using System.Drawing;
using System.Threading;
using System.Globalization;

public partial class inv_Purchase : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string staffcode = string.Empty;
    string app_id = string.Empty;
    string activerow = "";
    string activecol = "";
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    bool check = false;
    bool Cellclick = false;
    static string purchaseordertype = "";
    DataTable dt = new DataTable();
    DataTable dt2 = new DataTable();
    DataRow dr;
    static string checknew = "";
    static string itembook = "";



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
            app_id = d2.GetFunction("select sa.appl_id from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and s.staff_code='" + staffcode + "'");
            calorderdate.EndDate = DateTime.Now;

            if (!IsPostBack)
            {
                Session["purchaseordertype"] = null;
                Session["requestcode"] = null;
                Session["vendorpk"] = null;
                Session["ReqCompCode"] = null;
                Session["vendor"] = null;
                Session["CompCode"] = null;
                Session["Back"] = null;
                Session["ItemOrBook"] = null;
                Session["ItemOrBookid"] = null;
                Fpspread3.Sheets[0].RowCount = 0;
                Fpspread3.Sheets[0].ColumnCount = 0;
                Fpspread3.Visible = false;
                txt_deliverydate.Attributes.Add("readonly", "readonly");
                txt_deliverydate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtorderdate.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                txtorderdate.Attributes.Add("readonly", "readonly");
                txtduedate.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                txtduedate.Attributes.Add("readonly", "readonly");
                txt_date.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                txt_date.Attributes.Add("readonly", "readonly");

                txtpop1totalcost.Attributes.Add("readonly", "readonly");
                txt_invitem.Attributes.Add("readonly", "readonly");
                txt_requestcode.Attributes.Add("readonly", "readonly");

                txtduedate.Enabled = false;
                bindordercode();
                Session["dt"] = null;
                Session["purchaseordertype"] = null;
                bindreqcode();

                try
                {
                    string get_vendorcode_fromcompare = (Convert.ToString(Request.QueryString["@@barath$$"]));

                    if (get_vendorcode_fromcompare.Trim() != "" && get_vendorcode_fromcompare.Trim() != null)
                    {
                        string[] split = get_vendorcode_fromcompare.Split('s');

                        string get_vendorcode_fromcompare1 = split[0];
                        Session["vendorpk"] = split[0];
                        Session["ReqCompCode"] = split[1];
                        Session["Back"] = split[2];
                        Session["vendor"] = split[3];
                        Session["CompCode"] = split[4];
                        Session["ItemOrBook"] = split[5];
                        Session["ItemOrBookid"] = split[6];

                        txtvendorname.Text = d2.GetFunction("select VendorCompName from CO_VendorMaster where VendorPK='" + get_vendorcode_fromcompare1 + "'");
                        txtvendorname.Enabled = true;
                        btn_baseGo.Enabled = true;
                        btnadd.Enabled = true;

                        if (split[5].ToString() != "")
                        {
                            if (split[5].ToString() == "1")
                            {
                                purchaseordertype = d2.GetFunction("select purchaseordertype from IM_POSettings where collegecode='" + collegecode1 + "' and settingtype='1' and settingformess='0'");
                                Rad_item.Checked = true;
                            }
                            if (split[5].ToString() == "2")
                            {
                                purchaseordertype = d2.GetFunction("select purchaseordertype from IM_POSettings where collegecode='" + collegecode1 + "' and settingtype='1' and settingformess='2'");
                                Rad_book.Checked = true;
                            }
                        }
                        Session["purchaseordertype"] = purchaseordertype;
                        lblvendorname.Text = "Vendor Name";
                        txt_invitem.Visible = false;
                        txt_requestcode.Visible = false;
                        txt_appven.Visible = false;
                        btn_basego_click(sender, e);
                        if (purchaseordertype.Trim() != "5")
                        {
                            btn_baseGo.Enabled = false;
                            btnadd.Enabled = false;
                            txtvendorname.Enabled = false;
                            lbl_alerterror.Visible = true;
                            alertmessage.Visible = true;
                            btn_basego_click(sender, e);
                            lbl_alerterror.Text = "Please Change Purchase Order Setting.";
                        }
                        if (Session["Back"] != "")
                            Button_Backpage.Visible = true;
                        else
                            Button_Backpage.Visible = false;

                    }
                }
                catch
                {
                    if (Rad_item.Checked)
                        purchaseordertype = d2.GetFunction("select purchaseordertype from IM_POSettings where collegecode='" + collegecode1 + "' and settingtype='1' and settingformess='0'");
                    if (Rad_book.Checked)
                        purchaseordertype = d2.GetFunction("select purchaseordertype from IM_POSettings where collegecode='" + collegecode1 + "' and settingtype='1' and settingformess='2'");
                    Session["purchaseordertype"] = purchaseordertype;
                    if (purchaseordertype.Trim() == "3")
                    {
                        lblvendorname.Text = "Vendor Name";
                        txt_invitem.Visible = false;
                        txt_requestcode.Visible = false;
                        txt_appven.Visible = false;

                    }
                    else if (purchaseordertype.Trim() == "1")
                    {
                        if (Rad_item.Checked)
                        {
                            lbl_itemtype3.Text = "Item Name";
                            lblvendorname.Text = "Item Name";
                        }
                        if (Rad_book.Checked)
                        {
                            lbl_itemtype3.Text = "Book Name";
                            lblvendorname.Text = "Book Name";
                        }
                        ddl_requestcode.Visible = false;
                        txtvendorname.Visible = false;
                        txt_invitem.Visible = true;
                        txt_requestcode.Visible = false;
                        txt_ind_item.Visible = true;
                        p51.Visible = true;
                        txt_appven.Visible = false;
                        btn_baseGo.Visible = false;
                    }
                    else if (purchaseordertype.Trim() == "4")
                    {
                        ddl_requestcode.Visible = true;
                        lbl_itemtype3.Text = "Request Code";
                        txt_invitem.Visible = false;
                        txtvendorname.Visible = false;
                        lblvendorname.Text = "Request Code";
                        txt_requestcode.Visible = true;
                        txt_ind_item.Visible = true;
                        p51.Visible = true;
                        //txt_reqcode.Visible = true;
                        // Panel1.Visible = true;
                        bindrequestcode();
                        txt_appven.Visible = false;
                        lbl_2itemname.Visible = true;
                        btn_baseGo.Visible = false;
                        if (Rad_item.Checked)
                            lbl_2itemname.Text = "Item Name";
                        if (Rad_book.Checked)
                            lbl_2itemname.Text = "Item Name";
                    }
                    else if (purchaseordertype.Trim() == "2")
                    {
                        ddl_requestcode.Visible = true;
                        txt_invitem.Visible = false;
                        txtvendorname.Visible = false;
                        txt_requestcode.Visible = true;
                        //txt_reqcode.Visible = true;
                        //Panel1.Visible = true;
                        lbl_itemtype3.Text = "Request Code";
                        lblvendorname.Text = "Request Code";
                        txt_appven.Visible = false;
                        lbl_2itemname.Visible = true;
                        p51.Visible = true;
                        txt_ind_item.Visible = true;
                        btn_baseGo.Visible = false;
                        if (Rad_item.Checked)
                        {
                            lbl_2itemname.Text = "Item Name";
                            bindrequestcode();
                        }
                        if (Rad_book.Checked)
                        {
                            lbl_2itemname.Text = "Book Name";
                        }
                    }
                    else
                    {
                        if (Rad_item.Checked)
                            lblvendorname.Text = "Item Name";
                        if (Rad_book.Checked)
                            lblvendorname.Text = "Book Name";
                        txt_requestcode.Visible = false;
                        txt_invitem.Visible = false;
                        txtvendorname.Enabled = false;
                        btn_baseGo.Enabled = false;
                        btnadd.Enabled = false;
                        txt_appven.Visible = false;
                        lbl_alerterror.Visible = true;
                        alertmessage.Visible = true;
                        lbl_alerterror.Text = "Permission is required !!!";
                    }
                }
            }
            purchaseordertype = Convert.ToString(Session["purchaseordertype"]);
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
    protected void imagebtnpopclose2_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
    }
    protected void btnadd_Click(object sender, EventArgs e)
    {

        if (purchaseordertype.Trim() == "3")
        {
            pop_vendor.Visible = true;
        }
        else if (purchaseordertype.Trim() == "1")
        {
            if (Rad_item.Checked)
                bind_itemname();
            if (Rad_book.Checked)
                bind_Bookname();

            btn_goinvitem_click(sender, e);
            ViewState["selecteditems"] = null;
            selectitemgrid.DataSource = null;
            selectitemgrid.DataBind();
            pop_individualitem.Visible = true;
            txt_search_itemname.Visible = true;
            txt_reqitemsearch.Visible = false;

        }
        else if (purchaseordertype.Trim() == "4")
        {
            pop_individualitem.Visible = true;
            txt_search_itemname.Visible = false;
            txt_reqitemsearch.Visible = true;
        }
        else if (purchaseordertype.Trim() == "2")
        {
            if (Rad_item.Checked)
                bind_itemname();
            if (Rad_book.Checked)
                bind_Bookname();
            pop_individualitem.Visible = true;
            txt_appsearchpop.Visible = true;
            btn_goinvitem_click(sender, e);
        }
    }
    protected void btnselect_Click(object sender, EventArgs e)
    {
        popbtntypediv.Visible = true;
    }
    protected void chkboxduedate_Click(object sender, EventArgs e)
    {
        if (chkboxdue.Checked == true)
        {
            txtduedate.Enabled = true;
        }
        else
        {
            txtduedate.Enabled = false;
        }
    }

    protected void imagebtnpopclose3_Click(object sender, EventArgs e)
    {
        pop_vendor.Visible = false;
    }

    protected void FpSpread1_CellClick(object sender, EventArgs e)
    {
        try
        {
            Cellclick = true;
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddl_requestcode_selectedindexchange(object sender, EventArgs e)
    {
        try
        {
            if (Rad_item.Checked)
                bind_itemname();
            if (Rad_book.Checked)
                bind_Bookname();
        }
        catch { }
    }
    protected void FpSpread1_render(object sender, EventArgs e)
    {
        if (Cellclick == true)
        {
            try
            {
                string activerow = "";
                string activecol = "";
                activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
                activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
                if (activerow.Trim() != "" && activecol != "0")
                {
                    string vendorcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    string vendorname = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                    string vendorpk = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                    txtvendorname.Text = vendorname;
                    Session["vendorpk"] = vendorpk;
                    //lbl_error.Text = "";
                    pop_vendor.Visible = false;
                    if (purchaseordertype.Trim() == "4")
                    {
                        btn_basego_click(sender, e);
                    }
                }
            }
            catch { }
        }
    }
    protected void txt_popsearchvendor_txt_change(object sender, EventArgs e)
    {
        try
        {
            string q1 = d2.GetFunction("select distinct vendorcode,VendorCompName from CO_VendorMaster where VendorCompName='" + txt_popsearchvendor.Text + "' order by VendorCompName");
            if (q1.Trim() != "" && q1.Trim() != "0")
            {
                lbl_error2.Visible = false;
                Session["vendorpk"] = d2.GetFunction("select distinct VendorPK from CO_VendorMaster WHERE VendorCompName='" + txt_popsearchvendor.Text + "'");
                btn_popgo_Click(sender, e);
            }
            else
            {
                txt_popsearchvendor.Text = "";
                lbl_error2.Visible = true;
                vendorsearch_div.Visible = false;
                lbl_error2.Text = "Please enter the correct vendor name";

            }
        }
        catch { }
    }
    protected void txtvendorname_base_onchange(object sender, EventArgs e)
    {
        try
        {
            string q1 = d2.GetFunction("select distinct vendorcode,VendorCompName from CO_VendorMaster where VendorCompName='" + txtvendorname.Text + "' order by VendorCompName");
            if (q1.Trim() != "" && q1.Trim() != "0")
            {
                lbl_error2.Visible = false;
                Session["vendorpk"] = d2.GetFunction("select distinct VendorPK from CO_VendorMaster WHERE VendorCompName='" + txtvendorname.Text + "'");
                btn_popgo_Click(sender, e);
                lbl_baseerror.Visible = false;
            }
            else
            {
                txtvendorname.Text = "";
                lbl_baseerror.Visible = true;
                vendorsearch_div.Visible = false;
                lbl_baseerror.Text = "Please enter the correct vendor name";

            }

        }
        catch
        { }
    }

    protected void btn_exit2_Click(object sender, EventArgs e)
    {
        try
        {
            pop_individualitem.Visible = false;
        }
        catch
        {

        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname1(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "";
        if (itembook == "1")
            query = "select distinct VendorCompName from CO_VendorMaster WHERE VendorType=1 and VendorCompName like '" + prefixText + "%' order by VendorCompName ";
        if (itembook == "2")
            query = "select distinct VendorCompName from CO_VendorMaster WHERE VendorType=1 and LibraryFlag='1' and VendorCompName like '" + prefixText + "%' order by VendorCompName ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["VendorCompName"].ToString());
            }
        }
        return name;
    }
    protected void btn_popgo_Click(object sender, EventArgs e)
    {
        string vendorname = "";
        if (txt_popsearchvendor.Text.Trim() != "")
        {
            if (Rad_item.Checked)
                vendorname = " select distinct vendorcode,VendorCompName,vendorpk from CO_VendorMaster where VendorType=1 and VendorCompName='" + txt_popsearchvendor.Text + "' order by vendorcode";
            if (Rad_book.Checked)
                vendorname = " select distinct vendorcode,VendorCompName,vendorpk from CO_VendorMaster where VendorType=1 and LibraryFlag='1' and VendorCompName='" + txt_popsearchvendor.Text + "' order by vendorcode";
        }
        else
        {
            if (Rad_item.Checked)
                vendorname = "select distinct vendorcode,VendorCompName,vendorpk from CO_VendorMaster where VendorType=1 order by vendorcode";
            if (Rad_book.Checked)
                vendorname = "select distinct vendorcode,VendorCompName,vendorpk from CO_VendorMaster where VendorType=1 and LibraryFlag='1' order by vendorcode";
        }
        ds.Clear();
        ds = d2.select_method_wo_parameter(vendorname, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 3;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Columns[0].Width = 50;
            FpSpread1.Columns[0].Locked = true;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Vendor Code";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Columns[1].Width = 150;
            FpSpread1.Columns[1].Locked = true;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Vendor Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Columns[2].Width = 350;
            FpSpread1.Columns[2].Locked = true;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["vendorcode"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["vendorpk"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
            }

            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

            FpSpread1.Visible = true;
            vendorsearch_div.Visible = true;
            lbl_error2.Visible = false;

        }
        else
        {
            FpSpread1.Visible = false;
            vendorsearch_div.Visible = false;
            lbl_error2.Visible = true;
            lbl_error2.Text = "No Record Found";
        }
    }

    protected void btnpopbtntypeok_Click(object sender, EventArgs e)
    {
        try
        {
            popbtntypediv.Visible = false;
            activerow = "";
            activecol = "";
            activerow = Fpspread3.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpspread3.ActiveSheetView.ActiveColumn.ToString();
            if (activecol.Trim() != "" && activerow.Trim() != "")
            {
                if (Fpspread3.Sheets[0].RowCount > 0)
                {
                    Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text = Convert.ToString(txtpop1qnty.Text);
                    Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 5].HorizontalAlign = HorizontalAlign.Right;
                    Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text = Convert.ToString(txtpop1rateunit.Text);
                    Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 6].HorizontalAlign = HorizontalAlign.Right;
                    Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Text = Convert.ToString(txtpop1dia.Text);
                    Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 7].HorizontalAlign = HorizontalAlign.Right;
                    Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Text = Convert.ToString(txtpop1tax.Text);
                    Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 9].HorizontalAlign = HorizontalAlign.Right;
                    Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 16].Text = Convert.ToString(txtpop1totalcost.Text);
                    Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 16].HorizontalAlign = HorizontalAlign.Right;
                    Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 10].Text = Convert.ToString(txtpop1exetax.Text);
                    Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 10].HorizontalAlign = HorizontalAlign.Right;
                    Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 11].Text = Convert.ToString(txtpop1educess.Text);
                    Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 11].HorizontalAlign = HorizontalAlign.Right;
                    Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 12].Text = Convert.ToString(txtpop1higher.Text);
                    Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 12].HorizontalAlign = HorizontalAlign.Right;
                    Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 13].Text = Convert.ToString(txtpop1otherchar.Text);
                    Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 13].HorizontalAlign = HorizontalAlign.Right;

                    Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 14].Text = Convert.ToString(txtpop1des.Text);
                    Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 14].HorizontalAlign = HorizontalAlign.Right;
                    Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 17].Text = Convert.ToString(txt_date.Text);
                    Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 17].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            if (Fpspread3.Sheets[0].RowCount > 0)
            {
                double totalvalue = 0;
                for (int row = 0; row < Fpspread3.Sheets[0].RowCount; row++)
                {
                    string value = Convert.ToString(Fpspread3.Sheets[0].Cells[row, 16].Text);
                    if (value.Trim() != "")
                    {
                        totalvalue = totalvalue + Convert.ToDouble(value);
                    }
                }
                txttotalcost.Text = Convert.ToString(totalvalue);
                txtdummyno.Text = Convert.ToString(totalvalue);
                string Advpayment = Convert.ToString(txtadpay.Text);
                if (Advpayment.Trim() != "")
                {
                    totalvalue = totalvalue - Convert.ToDouble(Advpayment);
                }
                txtbalcost.Text = Convert.ToString(totalvalue);
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

    protected void Fpspread3_render(object sender, EventArgs e)
    {
        try
        {
            if (check == true)
            {
                activerow = Fpspread3.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread3.ActiveSheetView.ActiveColumn.ToString();
                collegecode = Session["collegecode"].ToString();
                if (activerow.Trim() != "" && activecol.Trim() != "")
                {
                    Clear();
                    popbtntypediv.Visible = true;

                    txtpop1qnty.Text = Convert.ToString(Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);

                    txtpop1rateunit.Text = Convert.ToString(Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text);

                    txtpop1dia.Text = Convert.ToString(Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Text);

                    txtpop1tax.Text = Convert.ToString(Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Text);

                    txtpop1totalcost.Text = Convert.ToString(Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 16].Text);

                    txtpop1exetax.Text = Convert.ToString(Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 10].Text);

                    txtpop1educess.Text = Convert.ToString(Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 11].Text);

                    txtpop1higher.Text = Convert.ToString(Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 12].Text);

                    txtpop1otherchar.Text = Convert.ToString(Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 13].Text);

                    txtpop1des.Text = Convert.ToString(Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 14].Text);

                    txt_date.Text = Convert.ToString(Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 17].Text);
                }
            }
        }
        catch
        {

        }
    }
    public void clearnew()
    {
        try
        {
            txtper.Text = "";
            txttax.Text = "";
            txtothercharges.Text = "";
            txtdes.Text = "";
            txttotalcost.Text = "";
            txtround.Text = "";
            txtadpay.Text = "";
            txtbalcost.Text = "";
            txtpageno.Text = "";
        }
        catch
        {

        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            bool insert = false;
            if (Fpspread3.Sheets[0].RowCount > 0)
            {
                string ordercode = Convert.ToString(txtordercode.Text);
                string orderdate = Convert.ToString(txtorderdate.Text);
                string deliverydate = Convert.ToString(txt_deliverydate.Text);
                string isDuedate = "";
                string q1 = "";
                string[] split = orderdate.Split('/');
                DateTime dt = new DateTime();
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = deliverydate.Split('/');
                DateTime dt2 = new DateTime();
                dt2 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                string dudate = "";
                DateTime dt1 = new DateTime();
                if (chkboxdue.Checked == true)
                {
                    dudate = Convert.ToString(txtduedate.Text);
                    split = dudate.Split('/');
                    dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                    dudate = Convert.ToString(dt1.ToString("MM/dd/yyyy"));
                    isDuedate = "1";
                }
                else
                {
                    isDuedate = "0";
                }
                string totalamount = Convert.ToString(txttotalcost.Text);
                string advancePay = Convert.ToString(txtadpay.Text);
                if (advancePay.Trim() == "")
                {
                    advancePay = "0";
                }
                string Tax = Convert.ToString(txttax.Text);
                if (Tax.Trim() == "")
                {
                    Tax = "0";
                }
                string discounttxt = Convert.ToString(txtper.Text);
                if (discounttxt.Trim() == "")
                {
                    discounttxt = "0";
                }

                string discount = "";
                string disper = "";
                string disamt = "";
                if (cbxamount.Checked == false)
                {
                    disper = discounttxt;
                }
                else
                {
                    disamt = discounttxt;
                }
                string otherchagrges = Convert.ToString(txtothercharges.Text);
                if (otherchagrges.Trim() == "")
                {
                    otherchagrges = "0";
                }
                string description = Convert.ToString(txtdes.Text);
                string round = Convert.ToString(txtround.Text);
                if (round.Trim() == "")
                {
                    round = "0";
                }
                string roundtype = "";
                if (chkround.Checked == true)
                {
                    roundtype = "1";
                }
                else
                {
                    roundtype = "0";
                }
                string pageno = Convert.ToString(txtpageno.Text);
                if (pageno.Trim() == "")
                {
                    pageno = "0";
                }

                string ordertype = Convert.ToString(purchaseordertype);
                //,ForHostel,ApproveStatus, InwardStatus,
                if (disamt.Trim() == "")
                {
                    disamt = "0";
                }
                if (purchaseordertype.Trim() != "2")
                {
                    string reqstagecount = d2.GetFunction("select distinct ReqApproveStateCount from RQ_RequestHierarchy where RequestType='9' and ReqStaffAppNo='" + app_id + "'");
                    string query = "insert into RQ_Requisition(RequestType,RequestCode,RequestDate,ReqAppNo,MemType,RequestBy,ReqApproveStage) values('9','" + Convert.ToString(Session["requestcode"]) + "','" + Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy")) + "','" + app_id + "','2','0','0')";//'" + reqstagecount + "', ReqApproveStage,
                    int q = d2.update_method_wo_parameter(query, "TEXT");
                    if (q != 0)
                    {
                        q1 = "if exists(select*from CO_MasterValues where MasterValue='" + ordercode + "' and MasterCriteria1='" + Convert.ToString(Session["requestcode"]) + "' and MasterCriteria='PO Requestcode and Ordercode')update CO_MasterValues set MasterValue='" + ordercode + "' where MasterValue='" + ordercode + "' and MasterCriteria1='" + Convert.ToString(Session["requestcode"]) + "' and MasterCriteria='PO Requestcode and Ordercode' else insert into CO_MasterValues(MasterValue,MasterCriteria,MasterCriteria1,CollegeCode)values('" + ordercode + "','PO Requestcode and Ordercode','" + Convert.ToString(Session["requestcode"]) + "','" + collegecode1 + "')";
                        int ins = d2.update_method_wo_parameter(q1, "Text");
                    }
                }
                q1 = "";
                q1 = "insert into IT_PurchaseOrder (OrderCode,OrderDate,OrderType,OrderMode,IsOrderDueDate, OrderDueDate,IsTotDisPercent,TotDisAmt,TotTaxAmt,TotOtherChgAmt,OrderDescription,PageNo,VendorFK,InwardStatus,Reqstaff_appno,ReqCompCode) values ('" + ordercode + "','" + dt.ToString("MM/dd/yyyy") + "','" + ordertype + "','','','" + dudate + "','" + disper + "','" + disamt + "','" + Tax + "','" + otherchagrges + "','" + description + "','" + pageno + "','" + Convert.ToString(Session["vendorpk"]) + "','0','" + app_id + "','" + Convert.ToString(Session["ReqCompCode"]) + "')";
                int insertpurchase = d2.update_method_wo_parameter(q1, "Text");
                if (insertpurchase != 0)
                {
                    for (int i = 0; i < Fpspread3.Sheets[0].RowCount; i++)
                    {
                        string itemfk = Convert.ToString(Fpspread3.Sheets[0].Cells[i, 3].Tag);
                        string vendorfk = Convert.ToString(Fpspread3.Sheets[0].Cells[i, 2].Tag);
                        string qty = Convert.ToString(Fpspread3.Sheets[0].Cells[i, 5].Text);
                        string rpu = Convert.ToString(Fpspread3.Sheets[0].Cells[i, 6].Text);
                        string discountamt = Convert.ToString(Fpspread3.Sheets[0].Cells[i, 7].Text);
                        string tax = Convert.ToString(Fpspread3.Sheets[0].Cells[i, 9].Text);
                        string totalcosr = Convert.ToString(Fpspread3.Sheets[0].Cells[i, 16].Text);
                        string extax = Convert.ToString(Fpspread3.Sheets[0].Cells[i, 10].Text);
                        string educess = Convert.ToString(Fpspread3.Sheets[0].Cells[i, 11].Text);
                        string eduhigher = Convert.ToString(Fpspread3.Sheets[0].Cells[i, 12].Text);
                        string otherchar = Convert.ToString(Fpspread3.Sheets[0].Cells[i, 13].Text);
                        string decription = Convert.ToString(Fpspread3.Sheets[0].Cells[i, 14].Text);
                        string fpdate = Convert.ToString(Fpspread3.Sheets[0].Cells[i, 17].Text);
                      
                        string discountper = "";
                        string discountper1 =Convert.ToString(Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 5].Tag);
                        if (qty.Trim() != "" && rpu.Trim() != "")
                        {
                            if (qty.Trim() == "")
                            {
                                qty = "0";
                            }
                            if (rpu.Trim() == "")
                            {
                                rpu = "0";
                            }
                            if (cbdis.Checked == true)
                            {
                                discountper = "1";
                                //discountamt = discount1;
                            }
                            else if (discountper1 != "")
                            {
                                discountper = discountper1;
                            }
                            else
                            {

                                discountper = "0";
                            }
                            if (discountamt.Trim() == "")
                            {
                                discountamt = "0";
                            }
                            //if (discountper != "")
                            //{
                            //    discountper = "0";
                            //}
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

                            string purchaseorderpk = d2.GetFunction("select purchaseorderpk from IT_PurchaseOrder where OrderCode='" + ordercode + "' and VendorFK='" + Convert.ToString(Session["vendorpk"]) + "'");
                            string q2 = "insert into IT_PurchaseOrderDetail (ItemFK,Qty,RPU,IsDiscountPercent, DiscountAmt,TaxPercent,ExeciseTaxPer,EduCessPer,HigherEduCessPer,OtherChargeAmt,OtherChargeDesc,AppQty,PurchaseOrderFK)values('" + itemfk + "','" + qty + "','" + rpu + "','" + discountper + "','" + discountamt + "','" + tax + "','" + extax + "','" + educess + "','" + eduhigher + "','" + otherchar + "','" + decription + "','" + qty + "','" + purchaseorderpk + "')";
                            int ins_purchaseorderdet = d2.update_method_wo_parameter(q2, "Text");
                            if (ins_purchaseorderdet != 0)
                            {
                                insert = true;
                            }
                        }
                        else
                        {
                            lbl_alerterror.Visible = true;
                            alertmessage.Visible = true;
                            lbl_alerterror.Text = "Please Select Enter the Quantity and Rate per Unit";
                        }
                    }
                }
                if (insert == true)
                {
                    lbl_alerterror.Visible = true;
                    alertmessage.Visible = true;
                    lbl_alerterror.Text = "Saved Successfully";
                    Clear();
                    clearnew();
                    bindordercode();
                    bindreqcode();
                }
            }
            else
            {

            }
        }
        catch { }
    }
    protected void bindreqcode()
    {
        try
        {
            string newitemcode = "";
            string selectquery = "select ReqAcr,ReqSize,ReqStNo  from IM_CodeSettings order by StartDate desc";
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string itemacronym = Convert.ToString(ds.Tables[0].Rows[0]["ReqAcr"]);
                string itemstarno = Convert.ToString(ds.Tables[0].Rows[0]["ReqStNo"]);
                string itemsize = Convert.ToString(ds.Tables[0].Rows[0]["ReqSize"]);
                selectquery = "select distinct top (1)  RequestCode  from RQ_Requisition where RequestCode like '" + Convert.ToString(itemacronym) + "%' order by RequestCode desc";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string itemcode = Convert.ToString(ds.Tables[0].Rows[0]["RequestCode"]);
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
                    else if (len1 == 5)
                    {
                        newitemcode = "00000" + itemstarno;
                    }
                    else if (len1 == 6)
                    {
                        newitemcode = "000000" + itemstarno;
                    }
                    else
                    {
                        newitemcode = Convert.ToString(itemstarno);
                    }
                    newitemcode = Convert.ToString(itemacronym) + "" + Convert.ToString(newitemcode);
                }
                Session["requestcode"] = Convert.ToString(newitemcode);
            }
        }
        catch
        { }
    }


    protected void btnexit_Click(object sender, EventArgs e)
    {
        try
        {
            popwindow1.Visible = true;
        }
        catch
        {

        }
    }
    protected void btnpop1Exit_Click(object sender, EventArgs e)
    {
        popbtntypediv.Visible = false;
        Clear();
    }
    protected void btngo_vendoritem_Click(object sender, EventArgs e)
    {
        try
        {
            //bindspread();
        }
        catch
        {

        }
    }
    protected void Fpspread2_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = Fpspread2.Sheets[0].ActiveRow.ToString();
            string actcol = Fpspread2.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "4")
            {
                if (Fpspread2.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(Fpspread2.Sheets[0].Cells[0, 4].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < Fpspread2.Sheets[0].RowCount; i++)
                        {
                            Fpspread2.Sheets[0].Cells[i, 4].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < Fpspread2.Sheets[0].RowCount; i++)
                        {
                            Fpspread2.Sheets[0].Cells[i, 4].Value = 0;
                        }
                    }
                }
            }
        }
        catch
        {

        }
    }

    protected void btnpurchase_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch
        {

        }
    }
    protected void btnpop1winexit_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
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
                    else if (len1 == 5)
                    {
                        newitemcode = "00000" + itemstarno;
                    }
                    else if (len1 == 6)
                    {
                        newitemcode = "000000" + itemstarno;
                    }
                    else
                    {
                        newitemcode = Convert.ToString(itemstarno);
                    }
                    newitemcode = Convert.ToString(itemacronym) + "" + Convert.ToString(newitemcode);
                }
                txtordercode.Text = Convert.ToString(newitemcode);
            }
        }
        catch
        {

        }
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alertmessage.Visible = false;
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
        //ddlpop1dep.SelectedIndex = 0;
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        popbtntypediv.Visible = false;
    }
    protected void Chksechosname(object sender, EventArgs e)
    {
        int cout = 0;
        txtvendor.Text = "---Select---";
        if (Chkven.Checked == true)
        {
            cout++;
            for (int i = 0; i < Cblven.Items.Count; i++)
            {
                Cblven.Items[i].Selected = true;
            }
            txtvendor.Text = "vendor(" + (Cblven.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < Cblven.Items.Count; i++)
            {
                Cblven.Items[i].Selected = false;
            }
        }
        item();
    }
    protected void Cblsechosname(object sender, EventArgs e)
    {
        int i = 0;
        Chkven.Checked = false;
        int commcount = 0;
        txtvendor.Text = "--Select--";
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
            txtvendor.Text = "Vendor(" + commcount.ToString() + ")";
        }
        item();
    }
    public void item()
    {


    }
    protected void cbitem_change(object sender, EventArgs e)
    {
        try
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
                txtitmname.Text = "Item Name(" + (Cblitm.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Cblitm.Items.Count; i++)
                {
                    Cblitm.Items[i].Selected = false;
                }
            }
        }
        catch
        {

        }
    }

    protected void Cblitmname(object sender, EventArgs e)
    {
        int i = 0;
        Chkven.Checked = false;
        int commcount = 0;
        txtitmname.Text = "--Select--";
        for (i = 0; i < Cblitm.Items.Count; i++)
        {
            if (Cblitm.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                Chkven.Checked = false;
            }
        }
        if (commcount > 0)
        {
            txtitmname.Text = "Item Name(" + commcount.ToString() + ")";
            if (commcount == Cblitm.Items.Count)
            {
                Chkitm.Checked = true;
            }
        }
    }
    protected void btn_basego_click(object sender, EventArgs e)
    {
        try
        {
            string q1 = "";

            if (purchaseordertype.Trim() == "5")
            {
                #region quatation and purchase
                if (txtvendorname.Text.Trim() != "")
                {
                    string itemorbookno = Session["ItemOrBookid"].ToString();
                    if (Rad_item.Checked)
                        q1 = "select distinct i.itemname,i.ItemCode,vd.RPU, vd.DiscountAmt,vd.IsDiscountPercent, vd.TaxPercent,vd.ItemFK, vd.EduCessPer,HigherEduCessPer,vd.ExeciseTaxPer, vd.OtherChargeAmt,vm.VendorCompName, vm.VendorCode,vq.VendorFK,vd.OtherChargeDesc,isnull(vq.VenDeliverydate,'')VenDeliverydate from IT_VendorReq rq,IT_VendorQuot vq,IT_VednorQuotDet vd,IM_ItemMaster i,CO_VendorMaster vm where vq.VendorQuotPK=vd.VendorQuotFK and rq.VendorFK=vq.VendorFK and i.ItemPK=vd.ItemFK and vm.VendorPK=vq.VendorFK and vm.vendorpk='" + Convert.ToString(Session["vendorpk"]) + "' and vd.ItemFK in('" + itemorbookno + "') and ISNULL(rq.itemorlibrary,'')='' and isnull(LibraryFlag,'')='' and rq.VenReqPK=vq.VenReqFK and rq.PurchaseStatus<>'1'";
                        //q1 = "select distinct i.itemname,i.ItemCode,vd.Qty,vd.RPU, vd.DiscountAmt,vd.IsDiscountPercent, vd.TaxPercent,vd.ItemFK, vd.EduCessPer,HigherEduCessPer,vd.ExeciseTaxPer, vd.OtherChargeAmt,vm.VendorCompName, vm.VendorCode,vq.VendorFK,vd.OtherChargeDesc,isnull(vq.VenDeliverydate,'')VenDeliverydate,vd.IsDiscountPercent from IT_VendorReq rq,IT_VendorQuot vq,IT_VednorQuotDet vd,IM_ItemMaster i,CO_VendorMaster vm where vq.VendorQuotPK=vd.VendorQuotFK and rq.VendorFK=vq.VendorFK and i.ItemPK=vd.ItemFK and vm.VendorPK=vq.VendorFK and vm.vendorpk='" + Convert.ToString(Session["vendorpk"]) + "' and vd.ItemFK in('" + itemorbookno + "')";
                    //and vq.VendorFK in('" + vendorpk + "') and rq.ReqCompCode in ('" + reqcomparecode + "')";
                    if (Rad_book.Checked)
                        //q1 = "select distinct i.Title itemname,i.BookID ItemCode,vd.Qty,vd.RPU, vd.DiscountAmt,vd.IsDiscountPercent, vd.TaxPercent,vd.ItemFK, vd.EduCessPer,HigherEduCessPer,vd.ExeciseTaxPer, vd.OtherChargeAmt,vm.VendorCompName, vm.VendorCode,vq.VendorFK,vd.OtherChargeDesc,isnull(vq.VenDeliverydate,'')VenDeliverydate,vd.IsDiscountPercent from IT_VendorReq rq,IT_VendorQuot vq,IT_VednorQuotDet vd,bookdetails i,CO_VendorMaster vm where vq.VendorQuotPK=vd.VendorQuotFK and rq.VendorFK=vq.VendorFK and i.BookID=vd.ItemFK and vm.VendorPK=vq.VendorFK and vm.vendorpk='" + Convert.ToString(Session["vendorpk"]) + "' and vd.ItemFK in('" + itemorbookno + "')";
                        q1 = "select distinct i.Title itemname,i.BookID ItemCode,vd.RPU, vd.DiscountAmt,vd.IsDiscountPercent, vd.TaxPercent,vd.ItemFK, vd.EduCessPer,HigherEduCessPer,vd.ExeciseTaxPer, vd.OtherChargeAmt,vm.VendorCompName, vm.VendorCode,vq.VendorFK,vd.OtherChargeDesc,isnull(vq.VenDeliverydate,'')VenDeliverydate from IT_VendorReq rq,IT_VendorQuot vq,IT_VednorQuotDet vd,bookdetails i,CO_VendorMaster vm where vq.VendorQuotPK=vd.VendorQuotFK and rq.VendorFK=vq.VendorFK and i.BookID=vd.ItemFK and vm.VendorPK=vq.VendorFK and vm.vendorpk='" + Convert.ToString(Session["vendorpk"]) + "' and vd.ItemFK in('" + itemorbookno + "') and ISNULL(rq.itemorlibrary,'')='' and isnull(LibraryFlag,'')='' and rq.VenReqPK=vq.VenReqFK and rq.PurchaseStatus<>'1'";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q1, "Text");
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
                        }

                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        Fpspread3.Columns[3].Width = 200;
                        Fpspread3.Columns[3].Locked = true;
                        //Fpspread3.Columns[3].Visible = false;



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
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string deldate = Convert.ToString(ds.Tables[0].Rows[0]["VenDeliverydate"]);
                            if (deldate != "" && deldate == "1900-01-01 12:00:00 AM")
                                txt_deliverydate.Text = deldate;

                        }
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

                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["itemname"]);
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["ItemFK"]);

                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                            string qty1 = d2.GetFunction("select Sum(vd.Qty)Qty from IT_VendorReq rq,IT_VendorQuot vq,IT_VednorQuotDet vd,IM_ItemMaster i,CO_VendorMaster vm where vq.VendorQuotPK=vd.VendorQuotFK and rq.VendorFK=vq.VendorFK and i.ItemPK=vd.ItemFK and vm.VendorPK=vq.VendorFK and vm.vendorpk='" + Convert.ToString(Session["vendorpk"]) + "' and vd.ItemFK in('" + Convert.ToString(ds.Tables[0].Rows[i]["ItemFK"]) + "') and ISNULL(rq.itemorlibrary,'')='' and isnull(LibraryFlag,'')='' and rq.VenReqPK=vq.VenReqFK and rq.PurchaseStatus<>'1';");


                            double qty = Convert.ToDouble(qty1);
                            double rpu = Convert.ToDouble(ds.Tables[0].Rows[i]["RPU"]);
                            //double discountamt = Convert.ToDouble(ds.Tables[0].Rows[i]["DiscountAmt"]);
                            //double discountper = Convert.ToDouble(ds.Tables[0].Rows[i]["IsDiscountPercent"]); 
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
                            //    discount = disper;
                            //    dispercal = Convert.ToDouble(discount);
                            //}
                            //else if (disamt.Trim() != "")
                            //{
                            //    discount = disamt;
                            //    disamtcal = Convert.ToDouble(discount);
                            //}
                            //else
                            //{
                            //    discount = "0";
                            //}
                            double cost = 0;
                            cost = qty * rpu;
                            if (disper.ToUpper() == "TRUE")
                            {
                                if (disamt != "")
                                {
                                    discount = disamt;
                                    disamtcal = Convert.ToDouble(disamt);
                                    cost = cost - disamtcal;
                                }
                            }
                            else
                            {
                                if (disamt.Trim() != "")
                                {
                                    discount =disamt;
                                    disamtcal = Convert.ToDouble(disamt);
                                    double d = (cost / 100) * disamtcal;
                                    cost = cost - d;
                                }
                            }
                           

                            //if (disamtcal != 0)
                            //{
                            //    //double d = (cost / 100) * disamtcal;
                            //    //cost = cost - d;
                            //    cost = cost - disamtcal;
                            //}
                            //if (dispercal != 0)
                            //{
                            //    dis = (cost / 100) * dispercal;
                            //    cost = cost - dis;
                            //}
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

                            //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(Math.Round(cost, 2));
                            //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                            //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(qty);
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(disper);
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";


                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["RPU"]);
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";


                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 7].Text = discount;
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

                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Text = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Font.Name = "Book Antiqua";
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Font.Size = FontUnit.Medium;

                            string itemfk = Convert.ToString(Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Tag);
                            if (itemfk.Trim() != "")
                            {
                                if (rpu != 0)
                                {
                                    //cost1 = Convert.ToDouble(rpu) * Convert.ToDouble(qty);
                                    string ct = Convert.ToString(Math.Round(cost, 2));
                                    cost1 = Convert.ToDouble(ct);
                                }
                                if (cost1 != 0)
                                {
                                    totalcost = totalcost + cost1;
                                }
                            }
                        }
                        txttotalcost.Text = Convert.ToString(totalcost);
                        txtbalcost.Text = Convert.ToString(totalcost);
                        Fpspread3.Sheets[0].PageSize = Fpspread3.Sheets[0].RowCount;
                        Fpspread3.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        Fpspread3.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        Fpspread3.Sheets[0].FrozenRowCount = 0;
                        Fpspread3.Visible = true;
                        Fpspread3.SaveChanges();
                        //spreaddiv1.Visible = true;
                        lbl_baseerror.Visible = false;
                    }
                }
                else
                {
                    lbl_baseerror.Visible = true;
                    lbl_baseerror.Text = "Please Select Vendor Name";
                    Fpspread3.Visible = false;
                }
                #endregion
            }
            else if (purchaseordertype.Trim() == "1")
            {
                //No use Of Go
                #region Indiviual itemwise
                if (txt_invitem.Text.Trim() != "")
                {
                    string q2 = "select distinct vm.VendorCompName,vm.VendorPK,vm.VendorCode,i.ItemCode,i.ItemName,i.ItemPK from CO_VendorMaster vm,IM_VendorItemDept vd,im_itemmaster i where vm.vendorpk=vd.venitemfk and vd.itemfk=i.itempk and i.ItemName='" + txt_invitem.Text + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q2, "Text");
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
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Item Name";
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

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Fpspread3.Sheets[0].RowCount++;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].CellType = chkall;
                            //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["VendorPK"]);
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemName"]);
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["ItemPK"]);
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Text = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Font.Name = "Book Antiqua";
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Font.Size = FontUnit.Medium;
                        }
                        Fpspread3.Sheets[0].PageSize = Fpspread3.Sheets[0].RowCount;
                        Fpspread3.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        Fpspread3.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        Fpspread3.Sheets[0].FrozenRowCount = 0;
                        Fpspread3.Visible = true;
                        Fpspread3.SaveChanges();
                        //btn_selecteditem.Visible = true;
                        lbl_baseerror.Visible = false;

                    }
                }
                else
                {
                    lbl_baseerror.Visible = true;
                    lbl_baseerror.Text = "Please Select Item Name";
                    Fpspread3.Visible = false;
                }
                #endregion
            }
            else if (purchaseordertype.Trim() == "4")
            {
                #region request and purchase
                if (txt_requestcode.Text.Trim() != "")
                {

                    string q3 = "";
                    if (Rad_item.Checked)
                        q3 = "select distinct  vm.VendorCompName,vm.VendorPK,vm.VendorCode,i.itemname,i.ItemCode,i.ItemPK from RQ_Requisition rq,RQ_RequisitionDet rd,IM_VendorItemDept vd,IM_ItemMaster i,CO_VendorMaster vm where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='0' and vd.ItemFK=rd.ItemFK and i.itempk=vd.ItemFK  and vm.VendorPK=vd.VenItemFK and rq.RequestCode='" + txt_requestcode.Text + "' and vm.VendorPK in('" + Convert.ToString(Session["vendorpk"]) + "')";//and vd.VenItemFK='1'
                    if (Rad_book.Checked)
                    {
                        q3 = "select distinct  vm.VendorCompName,vm.VendorPK,vm.VendorCode from CO_VendorMaster vm where vm.LibraryFlag='1'  and vm.vendorpk='" + Convert.ToString(Session["vendorpk"]) + "'";
                        q3 += "select distinct bd.Title,bd.BookID  from RQ_Requisition rq,RQ_RequisitionDet rd,bookdetails bd where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='0' and  bd.BookID=rd.Lib_Bookid  and rq.RequestCode='" + txt_requestcode.Text + "'";
                    }
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q3, "Text");


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
                            Fpspread3.Columns[4].Visible = true;
                        }
                        if (Rad_book.Checked)
                        {
                            Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Book Name";
                            Fpspread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Book ID";
                            Fpspread3.Columns[4].Visible = false;
                        }
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        Fpspread3.Columns[3].Width = 200;
                        Fpspread3.Columns[3].Locked = true;
                        //Fpspread3.Columns[3].Visible = false;


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

                        if (Rad_item.Checked)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Fpspread3.Sheets[0].RowCount++;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].CellType = chkall;
                                //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["VendorPK"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                if (Rad_book.Checked)
                                {
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemName"]);
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["ItemPK"]);
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Text = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Font.Name = "Book Antiqua";
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Font.Size = FontUnit.Medium;
                                }

                                if (Rad_book.Checked)
                                {
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                                        {
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[1].Rows[j]["Title"]);
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[1].Rows[j]["BookID"]);
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[1].Rows[j]["BookID"]);
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Text = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].HorizontalAlign = HorizontalAlign.Center;
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Font.Name = "Book Antiqua";
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Font.Size = FontUnit.Medium;
                                            if (j != (ds.Tables[1].Rows.Count - 1))
                                                Fpspread3.Sheets[0].RowCount++;
                                        }
                                    }
                                }
                            }
                        }

                        Fpspread3.Sheets[0].PageSize = Fpspread3.Sheets[0].RowCount;
                        Fpspread3.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        Fpspread3.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        Fpspread3.Sheets[0].FrozenRowCount = 0;
                        Fpspread3.Visible = true;
                        Fpspread3.SaveChanges();
                        //btn_selecteditem.Visible = true;
                        lbl_baseerror.Visible = false;
                    }
                }
                else
                {
                    lbl_baseerror.Visible = true;
                    lbl_baseerror.Text = "Please Select Request Code";
                    Fpspread3.Visible = false;
                }
                #endregion
            }
            else if (purchaseordertype.Trim() == "2")
            {
                //No use Of Go
                #region vendor approved and purchase
                if (txt_appven.Text.Trim() != "")
                {
                    lbl_baseerror.Visible = true;
                    lbl_baseerror.Text = "Please Select Vendor Item";
                }
                #endregion
            }
            else if (purchaseordertype.Trim() == "3")
            {
                #region Direct purchase

                if (txtvendorname.Text.Trim() != "")
                {
                    string q2 = "";
                    if (Rad_item.Checked)
                        q2 = "select distinct vm.VendorCompName,vm.VendorPK,vm.VendorCode,i.ItemCode,i.ItemName,i.ItemPK from CO_VendorMaster vm,IM_VendorItemDept vd,im_itemmaster i where vm.vendorpk=vd.venitemfk and vd.itemfk=i.itempk and vm.VendorCompName='" + txtvendorname.Text + "'";
                    if (Rad_book.Checked)
                    {
                        q2 = "select distinct  vm.VendorCompName,vm.VendorPK,vm.VendorCode from CO_VendorMaster vm where vm.LibraryFlag='1'  and vm.VendorCompName='" + txtvendorname.Text + "'";
                        q2 += "select distinct bd.Title,bd.BookID  from bookdetails bd where BookID='19125'";//saran
                    }
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q2, "Text");
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
                            Fpspread3.Columns[4].Visible = true;
                        }

                        if (Rad_book.Checked)
                        {
                            Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Book Name";
                            Fpspread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Book ID";
                            Fpspread3.Columns[4].Visible = false;
                        }
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        Fpspread3.Columns[3].Width = 200;
                        Fpspread3.Columns[3].Locked = true;
                        //Fpspread3.Columns[3].Visible = false;


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

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Fpspread3.Sheets[0].RowCount++;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].CellType = chkall;
                            //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["VendorPK"]);
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                            if (Rad_item.Checked)
                            {
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemName"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["ItemPK"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Text = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Font.Name = "Book Antiqua";
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Font.Size = FontUnit.Medium;
                            }
                            if (Rad_book.Checked)
                            {
                                if (ds.Tables[1].Rows.Count > 0)
                                {
                                    for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                                    {
                                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[1].Rows[j]["Title"]);
                                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[1].Rows[j]["BookID"]);
                                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[1].Rows[j]["BookID"]);
                                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Text = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));
                                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Font.Name = "Book Antiqua";
                                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Font.Size = FontUnit.Medium;
                                        if (j != (ds.Tables[1].Rows.Count - 1))
                                            Fpspread3.Sheets[0].RowCount++;
                                    }
                                }
                            }
                        }
                        Fpspread3.Sheets[0].PageSize = Fpspread3.Sheets[0].RowCount;
                        Fpspread3.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        Fpspread3.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        Fpspread3.Sheets[0].FrozenRowCount = 0;
                        Fpspread3.Visible = true;
                        Fpspread3.SaveChanges();
                        //btn_selecteditem.Visible = true;
                        lbl_baseerror.Visible = false;

                    }
                }
                else
                {
                    lbl_baseerror.Visible = true;
                    lbl_baseerror.Text = "Please Select Vendor Name";
                    Fpspread3.Visible = false;
                }

                #endregion
            }
        }
        catch { }
    }
    /////individual item
    protected void imagebtnpopclose4_Click(object sender, EventArgs e)
    {
        selectvendor_div.Visible = false;
        pop_individualitem.Visible = false;
    }
    protected void btn_goinvitem_click(object sender, EventArgs e)
    {
        try
        {
            if (purchaseordertype.Trim() == "1")
            {
                if (ViewState["selecteditems"] != null)
                {
                    DataTable dnew = (DataTable)ViewState["selecteditems"];
                    ViewState["sb"] = dnew;
                    checknew = "s";
                }
                string itempk = "";
                for (int i = 0; i < cb1_invitem.Items.Count; i++)
                {
                    if (cb1_invitem.Items[i].Selected == true)
                    {
                        if (itempk == "")
                        {
                            itempk = "" + cb1_invitem.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            itempk = itempk + "'" + "," + "'" + cb1_invitem.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string q1 = "";
                if (Rad_item.Checked)
                {
                    if (txt_search_itemname.Text.Trim() != "")
                    {
                        q1 = "select distinct  itemcode ,itemname,itempk from IM_ItemMaster where itemname ='" + txt_search_itemname.Text + "' order by itemname";
                    }
                    else
                    {
                        q1 = "select distinct  itemcode ,itemname,itempk from IM_ItemMaster where itempk in('" + itempk + "') order by itemname";
                    }
                }
                if (Rad_book.Checked)
                {
                    if (txt_search_itemname.Text.Trim() != "")
                    {
                        q1 = "select distinct  BookID itemcode,Title itemname,BookID itempk from bookdetails where Title ='" + txt_search_itemname.Text + "' order by Title";
                    }
                    else
                    {
                        q1 = "select distinct  BookID itemcode,Title itemname,BookID itempk from bookdetails where BookID in('" + itempk + "') order by Title";
                    }
                }
                if (txt_ind_item.Text.Trim() != "--Select--")
                {
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gvdatass.DataSource = ds.Tables[0];
                        gvdatass.DataBind();
                        gvdatass.Visible = true;
                        div2.Visible = true;
                        btn_selecteditem.Visible = true;
                        btn_exit2.Visible = true;
                        lbl_inverror.Visible = false;
                    }
                }
                else
                {
                    lbl_inverror.Visible = true;
                    lbl_inverror.Text = "Please Select Item Name";
                    gvdatass.Visible = false;
                    div2.Visible = false;
                    btn_selecteditem.Visible = false;
                    btn_exit2.Visible = false;
                }
            }
            else if (purchaseordertype.Trim() == "4")
            {
                if (ViewState["selecteditems"] != null)
                {
                    DataTable dnew = (DataTable)ViewState["selecteditems"];
                    ViewState["sb"] = dnew;
                    checknew = "s";
                }
                string itempk = "";

                for (int i = 0; i < cb1_invitem.Items.Count; i++)
                {
                    if (cb1_invitem.Items[i].Selected == true)
                    {
                        if (itempk == "")
                        {
                            itempk = "" + cb1_invitem.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            itempk = itempk + "'" + "," + "'" + cb1_invitem.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string q1 = "";
                if (txt_search_itemname.Text.Trim() != "")
                {
                    if (Rad_item.Checked)
                        q1 = "select distinct i.itemname,i.ItemCode,i.ItemPK from RQ_Requisition rq,RQ_RequisitionDet rd,IM_VendorItemDept vd,IM_ItemMaster i where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='0' and vd.ItemFK=rd.ItemFK and i.itempk=vd.ItemFK order by itemname";
                    if (Rad_book.Checked)
                        q1 = "select distinct bd.Title itemname,bd.BookID ItemCode,bd.BookID ItemPK from RQ_Requisition rq,RQ_RequisitionDet rd,bookdetails bd where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='0' and bd.BookID=rd.Lib_Bookid  order by Title";
                }
                else
                {
                    if (Rad_item.Checked)
                        q1 = "select distinct i.itemname,i.ItemCode,i.ItemPK from RQ_Requisition rq,RQ_RequisitionDet rd,IM_VendorItemDept vd,IM_ItemMaster i where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='0' and vd.ItemFK=rd.ItemFK and i.itempk=vd.ItemFK and  i.ItemPK in('" + itempk + "') and rq.RequestCode ='" + Convert.ToString(ddl_requestcode.SelectedItem.Value) + "' order by itemname";

                    if (Rad_book.Checked)
                        q1 = "select distinct bd.Title itemname,bd.BookID ItemCode,bd.BookID ItemPK  from RQ_Requisition rq,RQ_RequisitionDet rd,bookdetails bd where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='0' and bd.BookID=rd.Lib_Bookid  and  bd.BookID in('" + itempk + "') and rq.RequestCode ='" + Convert.ToString(ddl_requestcode.SelectedItem.Value) + "' order by Title";
                }
                if (txt_search_itemname.Text.Trim() != "--Select--")
                {
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gvdatass.DataSource = ds.Tables[0];
                        gvdatass.DataBind();
                        gvdatass.Visible = true;
                        div2.Visible = true;
                        btn_selecteditem.Visible = true;
                        btn_exit2.Visible = true;
                        lbl_inverror.Visible = false;
                    }
                }
                else
                {
                    lbl_inverror.Visible = true;
                    lbl_inverror.Text = "Please Select Item Name";
                    gvdatass.Visible = false;
                    div2.Visible = false;
                    btn_selecteditem.Visible = false;
                    btn_exit2.Visible = false;
                }
            }
            else if (purchaseordertype.Trim() == "2")
            {
                if (ViewState["selecteditems"] != null)
                {
                    DataTable dnew = (DataTable)ViewState["selecteditems"];
                    ViewState["sb"] = dnew;
                    checknew = "s";
                }
                string itempk = "";
                for (int i = 0; i < cb1_invitem.Items.Count; i++)
                {
                    if (cb1_invitem.Items[i].Selected == true)
                    {
                        if (itempk == "")
                        {
                            itempk = "" + cb1_invitem.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            itempk = itempk + "'" + "," + "'" + cb1_invitem.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string q1 = "";
                if (txt_search_itemname.Text.Trim() != "")
                {
                    if (Rad_item.Checked)
                        q1 = "select distinct i.itemname,i.ItemPK,i.ItemCode from RQ_Requisition rq,RQ_RequisitionDet rd,IM_VendorItemDept vd,IM_ItemMaster i where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='1'  and vd.ItemFK=rd.ItemFK and i.ItemPK=vd.ItemFK  order by itemname";//and rd.ReqAppStatus='1'
                    if (Rad_book.Checked)
                        q1 = "  select distinct bd.Title itemname,bd.BookID ItemCode,bd.BookID ItemPK from RQ_Requisition rq,RQ_RequisitionDet rd,bookdetails bd  where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='1'  and bd.BookID=rd.Lib_Bookid    order by Title";
                }
                else
                {
                    if (Rad_item.Checked)
                        q1 = "select distinct i.itemname,i.ItemPK,i.ItemCode from RQ_Requisition rq,RQ_RequisitionDet rd,IM_VendorItemDept vd,IM_ItemMaster i where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='1'  and vd.ItemFK=rd.ItemFK and i.ItemPK=vd.ItemFK  and  i.ItemPK in('" + itempk + "') and rq.RequestCode ='" + Convert.ToString(ddl_requestcode.SelectedItem.Value) + "' order by itemname";//and rd.ReqAppStatus='1'
                    if (Rad_book.Checked)
                        q1 = "select distinct bd.Title itemname,bd.BookID ItemCode,bd.BookID ItemPK  from RQ_Requisition rq,RQ_RequisitionDet rd,bookdetails bd where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='1'  and bd.BookID=rd.Lib_Bookid    and  bd.BookID in('" + itempk + "') and rq.RequestCode ='" + Convert.ToString(ddl_requestcode.SelectedItem.Value) + "' order by Title";//and rd.ReqAppStatus='1'
                }
                if (txt_search_itemname.Text.Trim() != "--Select--")
                {
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gvdatass.DataSource = ds.Tables[0];
                        gvdatass.DataBind();
                        gvdatass.Visible = true;
                        div2.Visible = true;
                        btn_selecteditem.Visible = true;
                        btn_exit2.Visible = true;
                        lbl_inverror.Visible = false;
                    }
                    else
                    {
                        lbl_inverror.Visible = true;
                        lbl_inverror.Text = "No Records Founds";
                        gvdatass.Visible = false;
                        div2.Visible = false;
                        btn_selecteditem.Visible = false;
                        btn_exit2.Visible = false;
                    }
                }
                else
                {
                    lbl_inverror.Visible = true;
                    lbl_inverror.Text = "Please Select Item Name";
                    gvdatass.Visible = false;
                    div2.Visible = false;
                    btn_selecteditem.Visible = false;
                    btn_exit2.Visible = false;
                }
            }
        }
        catch
        {

        }
    }
    protected void cbl_invitem_selectedindexchange(object sender, EventArgs e)
    {
        int i = 0;
        cb_invitem.Checked = false;
        int commcount = 0;
        txt_ind_item.Text = "--Select--";
        for (i = 0; i < cb1_invitem.Items.Count; i++)
        {
            if (cb1_invitem.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cb1_invitem.Items.Count)
            {
                cb_invitem.Checked = true;
            }
            txt_ind_item.Text = "Item Name(" + commcount.ToString() + ")";
        }
    }
    protected void cb_invitem_checkchange(object sender, EventArgs e)
    {
        int cout = 0;
        txt_ind_item.Text = "--Select--";
        if (cb_invitem.Checked == true)
        {
            cout++;
            for (int i = 0; i < cb1_invitem.Items.Count; i++)
            {
                cb1_invitem.Items[i].Selected = true;
            }
            txt_ind_item.Text = "Item Name(" + (cb1_invitem.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cb1_invitem.Items.Count; i++)
            {
                cb1_invitem.Items[i].Selected = false;
            }
        }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> inv_itemname(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "";
        if (itembook != "")
        {
            if (itembook == "1")
                query = "select distinct ItemName from IM_ItemMaster where ItemName like '" + prefixText + "%' ";
            if (itembook == "2")
                query = "select distinct Title from bookdetails where Title like '" + prefixText + "%' ";
            dw = dn.select_method_wo_parameter(query, "Text");
            if (dw.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
                {
                    name.Add(dw.Tables[0].Rows[i]["ItemName"].ToString());
                }
            }
        }
        return name;
    }
    protected void txt_invitem_base_onchange(object sender, EventArgs e)
    {
        try
        {
            string itemname = d2.GetFunction("select distinct vm.VendorPK from CO_VendorMaster vm,IM_VendorItemDept vd,im_itemmaster i where vm.vendorpk=vd.venitemfk and vd.itemfk=i.itempk and i.ItemName='" + txt_invitem.Text + "'");
            if (itemname.Trim() == "" || itemname.Trim() == "0")
            {
                txt_invitem.Text = "";
                lbl_baseerror.Visible = true;
                lbl_baseerror.Text = "Please Enter Valid Item Name";
                Fpspread3.Visible = false;
            }
            else
            {
                lbl_baseerror.Visible = false;
                Session["vendorpk"] = itemname;
            }
        }
        catch { }
    }
    protected void bind_itemname()
    {
        ds.Clear();
        cb1_invitem.Items.Clear();
        //ds = d2.BindItemCodeWithOutParameter_inv();
        string q1 = "";
        if (purchaseordertype.Trim() == "1")
        {
            q1 = "select distinct ItemName,ItemPK from IM_ItemMaster order by itemname";
        }
        else if (purchaseordertype.Trim() == "2")
        {
            q1 = "select distinct i.itemname,i.ItemPK from RQ_Requisition rq,RQ_RequisitionDet rd,IM_VendorItemDept vd,IM_ItemMaster i where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='1'  and vd.ItemFK=rd.ItemFK and i.ItemPK=vd.ItemFK and rq.RequestCode in('" + Convert.ToString(ddl_requestcode.SelectedItem.Value) + "')";//and rd.ReqAppStatus='1'
        }
        else if (purchaseordertype.Trim() == "4")
        {
            q1 = "select distinct i.itemname,i.ItemPK from RQ_Requisition rq,RQ_RequisitionDet rd,IM_VendorItemDept vd,IM_ItemMaster i where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='0' and rd.ReqAppStatus='0' and vd.ItemFK=rd.ItemFK and i.ItemPK=vd.ItemFK and rq.RequestCode in('" + Convert.ToString(ddl_requestcode.SelectedItem.Value) + "') ";
        }

        ds = d2.select_method_wo_parameter(q1, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cb1_invitem.DataSource = ds;
            cb1_invitem.DataTextField = "itemname";
            cb1_invitem.DataValueField = "ItemPK";
            cb1_invitem.DataBind();

            if (cb1_invitem.Items.Count > 0)
            {
                for (int i = 0; i < cb1_invitem.Items.Count; i++)
                {
                    cb1_invitem.Items[i].Selected = true;
                }

                txt_ind_item.Text = "Item Name(" + cb1_invitem.Items.Count + ")";
            }
        }
        else
        {
            txt_ind_item.Text = "--Select--";
        }
    }
    protected void selectedmenuchk(object sender, EventArgs e)
    {
        int count = 0;
        bindtable();
        if (checknew == "s")
        {
            if (ViewState["sb"] != null)
            {
                DataTable dts = (DataTable)ViewState["sb"];
                DataView dv = new DataView(dts);
                dt = dv.ToTable();
                dr = null;
            }
        }
        else
        {

        }
        foreach (DataListItem gvrow in gvdatass.Items)
        {
            CheckBox chkSelect = (gvrow.FindControl("CheckBox2") as CheckBox);
            if (chkSelect.Checked)
            {
                count++;
                dr = dt.NewRow();
                string itemcode = "";
                string itemnamegv = "";
                string itemheadername = "";

                dr[0] = Convert.ToString(count);

                Label lbl_itemname = (Label)gvrow.FindControl("lbl_itemname");
                itemnamegv = lbl_itemname.Text;
                dr[1] = itemnamegv;

                Label lbl_itemcode = (Label)gvrow.FindControl("lbl_itemcode");
                itemcode = lbl_itemcode.Text;
                dr[2] = itemcode;

                Label lbl_headername = (Label)gvrow.FindControl("lbl_itempk");
                itemheadername = lbl_headername.Text;
                dr[3] = itemheadername;

                if (dt.Rows.Count > 0)
                {
                    DataView d = new DataView(dt);
                    d.RowFilter = "ItemCode ='" + itemcode + "'";
                    if (d.Count == 0)
                    {
                        dt.Rows.Add(dr);
                    }
                }
                else
                {
                    dt.Rows.Add(dr);
                }
                selectitemgrid.DataSource = dt;
                selectitemgrid.DataBind();
            }
            else
            {

            }
        }
        selectitemgrid.DataSource = dt;
        selectitemgrid.DataBind();
        ViewState["selecteditems"] = dt;
    }
    public void bindtable()
    {
        dt.Columns.Add("S.No");
        dt.Columns.Add("Item Name");
        dt.Columns.Add("ItemCode");
        dt.Columns.Add("Item Pk");
        dt.TableName = "selecteditems";
    }
    protected void btn_selecteditem_Click(object sender, EventArgs e)
    {
        try
        {
            string itempk = "";
            if (selectitemgrid.Rows.Count > 0)
            {
                for (int i = 0; i < selectitemgrid.Rows.Count; i++)
                {
                    //Convert.ToString((selectitemgrid.Rows[i].FindControl("itemcodegv") as Label).Text);
                    //Convert.ToString((selectitemgrid.Rows[i].FindControl("itemnamegv") as Label).Text);
                    string itemp = Convert.ToString((selectitemgrid.Rows[i].FindControl("lbl_headername") as Label).Text);
                    if (itempk.Trim() == "")
                    {
                        itempk = itemp;
                    }
                    else
                    {
                        itempk = itempk + "'" + "," + "'" + itemp + "";
                    }
                }
            }
            else
            {
                lbl_alerterror.Visible = true;
                alertmessage.Visible = true;
                lbl_alerterror.Text = "Please select any one item";
            }
            if (itempk.Trim() != "")
            {
                string q2 = "";

                if (Rad_item.Checked)
                {
                    q2 = "select distinct vm.VendorCompName,vm.VendorPK,vm.VendorCode,i.ItemCode,i.ItemName,i.ItemPK from CO_VendorMaster vm,IM_VendorItemDept vd,im_itemmaster i where vm.vendorpk=vd.venitemfk and vd.itemfk=i.itempk and i.itempk in('" + itempk + "') order by vm.VendorCompName ";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q2, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Fpspread4.Sheets[0].RowCount = 0;
                        Fpspread4.Sheets[0].ColumnCount = 0;
                        Fpspread4.CommandBar.Visible = false;
                        Fpspread4.Sheets[0].AutoPostBack = false;
                        Fpspread4.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpspread4.Sheets[0].RowHeader.Visible = false;
                        Fpspread4.Sheets[0].ColumnCount = 5;

                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        Fpspread4.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        Fpspread4.Columns[0].Width = 50;
                        Fpspread4.Columns[0].Locked = true;

                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspread4.Columns[1].Width = 50;
                        Fpspread4.Columns[1].Visible = true;

                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Vendor Name";
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        Fpspread4.Columns[2].Width = 200;
                        Fpspread4.Columns[2].Locked = true;

                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Item Name";
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        Fpspread4.Columns[3].Width = 200;
                        Fpspread4.Columns[3].Locked = true;

                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Item Code";
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        Fpspread4.Columns[4].Width = 100;
                        Fpspread4.Columns[4].Locked = true;
                        FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                        cb1.AutoPostBack = false;
                        int startspanpoint = 0;
                        int rowcount1 = 1;
                        string vennam = "";
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Fpspread4.Sheets[0].RowCount++;
                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            if (vennam != Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]))
                            {
                                Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 1].CellType = cb1;
                                Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread4.Sheets[0].SpanModel.Add(startspanpoint, 1, rowcount1, 1);
                                startspanpoint = Fpspread4.Sheets[0].RowCount - 1;
                            }
                            else
                            {
                                rowcount1++;
                            }
                            vennam = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);

                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);
                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["VendorPK"]);
                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemName"]);
                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["ItemPK"]);
                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            //txt_invitem.Text = Convert.ToString();


                        }
                        Fpspread4.Sheets[0].PageSize = Fpspread4.Sheets[0].RowCount;
                        Fpspread4.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        Fpspread4.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        Fpspread4.SaveChanges();
                        Fpspread4.Visible = true;
                        btn_vendorselect.Visible = true;
                        btn_vendorExit.Visible = true;
                        selectvendor_div.Visible = true;
                    }
                }
                if (Rad_book.Checked)
                {
                    q2 = "select distinct vm.VendorCompName,vm.VendorPK,vm.VendorCode from CO_VendorMaster vm where vm.LibraryFlag='1' order by vm.VendorCompName;";
                    q2 += "select * from bookdetails bd where bd.BookID in('" + itempk + "')";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q2, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Fpspread4.Sheets[0].RowCount = 0;
                        Fpspread4.Sheets[0].ColumnCount = 0;
                        Fpspread4.CommandBar.Visible = false;
                        Fpspread4.Sheets[0].AutoPostBack = false;
                        Fpspread4.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpspread4.Sheets[0].RowHeader.Visible = false;
                        Fpspread4.Sheets[0].ColumnCount = 4;

                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        Fpspread4.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        Fpspread4.Columns[0].Width = 50;
                        Fpspread4.Columns[0].Locked = true;

                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspread4.Columns[1].Width = 50;
                        Fpspread4.Columns[1].Visible = true;

                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Vendor Name";
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        Fpspread4.Columns[2].Width = 200;
                        Fpspread4.Columns[2].Locked = true;

                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Book Name";
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        Fpspread4.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        Fpspread4.Columns[3].Width = 200;
                        Fpspread4.Columns[3].Locked = true;

                        FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                        cb1.AutoPostBack = false;
                        int startspanpoint = 0;
                        int rowcount1 = 1;
                        string vennam = "";
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Fpspread4.Sheets[0].RowCount++;
                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            if (vennam != Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]))
                            {
                                Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 1].CellType = cb1;
                                Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread4.Sheets[0].SpanModel.Add(startspanpoint, 1, rowcount1, 1);
                                startspanpoint = Fpspread4.Sheets[0].RowCount - 1;
                            }
                            else
                            {
                                rowcount1++;
                            }
                            vennam = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);

                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);
                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["VendorPK"]);
                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";


                            for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                            {

                                Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[1].Rows[j]["Title"]);
                                Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[1].Rows[j]["BookID"]);
                                Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                if (j != (ds.Tables[1].Rows.Count - 1))
                                    Fpspread4.Sheets[0].RowCount++;
                            }

                        }
                        Fpspread4.Sheets[0].PageSize = Fpspread4.Sheets[0].RowCount;
                        Fpspread4.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        Fpspread4.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        Fpspread4.SaveChanges();
                        Fpspread4.Visible = true;
                        btn_vendorselect.Visible = true;
                        btn_vendorExit.Visible = true;
                        selectvendor_div.Visible = true;
                    }
                }
            }
        }
        catch
        {

        }
    }
    protected void selected_vendor()
    {
        try
        {


        }
        catch { }
    }
    protected void btn_vendorselect_Click(object sender, EventArgs e)
    {
        try
        {
            string VendorPK = "";
            string itempk = "";
            string itempk1 = "";
            int spreaditemchk = 0;
            if (Fpspread4.Sheets[0].RowCount > 0)
            {
                Fpspread4.SaveChanges();
                for (int row = 0; row < Fpspread4.Sheets[0].RowCount; row++)
                {
                    int checkval = Convert.ToInt32(Fpspread4.Sheets[0].Cells[row, 1].Value);
                    if (checkval == 1)
                    {
                        spreaditemchk = spreaditemchk + 1;
                    }
                }
            }
            if (spreaditemchk == 1)
            {
                if (Fpspread4.Sheets[0].RowCount > 0)
                {
                    Fpspread4.SaveChanges();
                    string valuenewvendor = "";
                    for (int row = 0; row < Fpspread4.Sheets[0].RowCount; row++)
                    {
                        int checkval = Convert.ToInt32(Fpspread4.Sheets[0].Cells[row, 1].Value);
                        VendorPK = Convert.ToString(Fpspread4.Sheets[0].Cells[row, 2].Tag);
                        if (valuenewvendor != Convert.ToString(VendorPK))
                        {
                            if (checkval == 1)
                            {
                                VendorPK = Convert.ToString(Fpspread4.Sheets[0].Cells[row, 2].Tag);
                                Session["vendorpk"] = VendorPK;
                                valuenewvendor = VendorPK;
                                itempk = Convert.ToString(Fpspread4.Sheets[0].Cells[row, 3].Tag);
                                if (itempk1.Trim() == "")
                                {
                                    itempk1 = itempk;
                                }
                                else
                                {
                                    itempk1 = itempk1 + "'" + "," + "'" + itempk;
                                }
                            }
                        }
                        else
                        {
                            itempk = Convert.ToString(Fpspread4.Sheets[0].Cells[row, 3].Tag);
                            if (itempk1.Trim() == "")
                            {
                                itempk1 = itempk;
                            }
                            else
                            {
                                itempk1 = itempk1 + "'" + "," + "'" + itempk;
                            }
                        }
                    }
                }
                VendorPK = Convert.ToString(Session["vendorpk"]);
                if (VendorPK.Trim() != "")
                {
                    string q2 = "";
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
                        Fpspread3.Columns[4].Visible = true;
                    }
                    if (Rad_book.Checked)
                    {
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Book Name";
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Book Code";
                        Fpspread3.Columns[4].Visible = false;
                    }
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread3.Columns[3].Width = 200;
                    Fpspread3.Columns[3].Locked = true;
                    //Fpspread3.Columns[3].Visible = false;


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

                    if (Rad_item.Checked)
                    {
                        if (purchaseordertype.Trim() == "1")
                        {

                            q2 = "select distinct vm.VendorCompName,vm.VendorPK,vm.VendorCode,i.ItemCode,i.ItemName,i.ItemPK from CO_VendorMaster vm,IM_VendorItemDept vd,im_itemmaster i where vm.vendorpk=vd.venitemfk and vd.itemfk=i.itempk and vm.VendorPK in('" + VendorPK + "') and i.ItemPK in('" + itempk1 + "')";

                        }
                        else if (purchaseordertype.Trim() == "4")
                        {
                            q2 = "select distinct  vm.VendorCompName,vm.VendorPK,vm.VendorCode,i.itemname,i.ItemCode,i.ItemPK from RQ_Requisition rq,RQ_RequisitionDet rd,IM_VendorItemDept vd,IM_ItemMaster i,CO_VendorMaster vm where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='0' and vd.ItemFK=rd.ItemFK and i.itempk=vd.ItemFK and vd.VenItemFK='" + VendorPK + "' and vm.VendorPK=vd.VenItemFK and i.ItemPK in('" + itempk1 + "') order by ItemName";
                        }
                        else if (purchaseordertype.Trim() == "2")
                        {
                            q2 = "select distinct vm.VendorCompName,vm.VendorPK,vm.VendorCode,i.ItemCode,i.ItemName,i.ItemPK from CO_VendorMaster vm,IM_VendorItemDept vd,im_itemmaster i where vm.vendorpk=vd.venitemfk and vd.itemfk=i.itempk and i.itempk in('" + itempk1 + "') and vendorpk in('" + VendorPK + "') order by vm.VendorCompName";
                        }
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(q2, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string item = "";
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Fpspread3.Sheets[0].RowCount++;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].CellType = chkall;
                                //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["VendorPK"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemName"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["ItemPK"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                string rate = d2.GetFunction("select top(1) RPU from IT_PurchaseOrderDetail where ItemFK in('" + Convert.ToString(ds.Tables[0].Rows[i]["ItemPK"]) + "') and Inward_Status='1' order by IT_PurchaseOrderDetailPK desc");

                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 6].Text = rate;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Text = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Font.Name = "Book Antiqua";
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Font.Size = FontUnit.Medium;
                                txt_appven.Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);

                                if (purchaseordertype.Trim() == "1")
                                {
                                    if (item.Trim() != "")
                                    {
                                        item = Convert.ToString(ds.Tables[0].Rows[i]["ItemName"]);
                                    }
                                    else
                                    {
                                        item = item + "," + Convert.ToString(ds.Tables[0].Rows[i]["ItemName"]);
                                    }
                                }
                            }

                            if (purchaseordertype.Trim() == "1")
                            {
                                txt_invitem.Text = item;
                            }
                            if (purchaseordertype.Trim() == "2" || purchaseordertype.Trim() == "4")
                            {
                                txt_requestcode.Text = Convert.ToString(ddl_requestcode.SelectedItem.Text);
                            }

                            Fpspread3.Sheets[0].PageSize = Fpspread3.Sheets[0].RowCount;
                            Fpspread3.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            //Fpspread3.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            Fpspread3.Sheets[0].FrozenRowCount = 0;
                            Fpspread3.Visible = true;
                            Fpspread3.SaveChanges();
                            //btn_selecteditem.Visible = true;
                            lbl_baseerror.Visible = false;
                            pop_individualitem.Visible = false;
                            selectvendor_div.Visible = false;
                        }
                    }
                    if (Rad_book.Checked)
                    {
                        if (purchaseordertype.Trim() == "1" || purchaseordertype.Trim() == "2")
                        {
                            q2 = "select distinct vm.VendorCompName,vm.VendorPK,vm.VendorCode from CO_VendorMaster vm where vm.LibraryFlag='1' and vm.VendorPK in('" + VendorPK + "') order by vm.VendorCompName;";
                            q2 += "select * from bookdetails bd where bd.BookID in('" + itempk1 + "')";

                        }
                        else if (purchaseordertype.Trim() == "4")
                        {
                            q2 = "select distinct vm.VendorCompName,vm.VendorPK,vm.VendorCode from CO_VendorMaster vm where vm.LibraryFlag='1' and vm.VendorPK in('" + VendorPK + "') order by vm.VendorCompName;";
                            q2 += "select distinct  bd.Title,bd.BookID from RQ_Requisition rq,RQ_RequisitionDet rd,bookdetails bd where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='0' and bd.BookID=rd.ItemFK  and bd.BookID in('" + itempk1 + "') order by Title";
                        }

                        ds.Clear();
                        ds = d2.select_method_wo_parameter(q2, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            string item = "";
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Fpspread3.Sheets[0].RowCount++;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["VendorPK"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";


                                for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                                {
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[1].Rows[i]["Title"]);
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[1].Rows[i]["BookID"]);
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[1].Rows[i]["BookID"]);
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                    string rate = d2.GetFunction("select top(1) RPU from IT_PurchaseOrderDetail where ItemFK in('" + Convert.ToString(ds.Tables[1].Rows[i]["BookID"]) + "') and Inward_Status='1' order by IT_PurchaseOrderDetailPK desc");

                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 6].Text = rate;
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";


                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Text = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Font.Name = "Book Antiqua";
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 17].Font.Size = FontUnit.Medium;

                                    if (j != (ds.Tables[1].Rows.Count - 1))
                                        Fpspread3.Sheets[0].RowCount++;



                                    if (purchaseordertype.Trim() == "1")
                                    {
                                        if (item.Trim() != "")
                                        {
                                            item = Convert.ToString(ds.Tables[1].Rows[i]["Title"]);
                                        }
                                        else
                                        {
                                            item = item + "," + Convert.ToString(ds.Tables[1].Rows[i]["Title"]);
                                        }
                                    }
                                }
                                txt_appven.Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);
                            }

                            if (purchaseordertype.Trim() == "1")
                            {
                                txt_invitem.Text = item;
                            }
                            if (purchaseordertype.Trim() == "2" || purchaseordertype.Trim() == "4")
                            {
                                txt_requestcode.Text = Convert.ToString(ddl_requestcode.SelectedItem.Text);
                            }

                            Fpspread3.Sheets[0].PageSize = Fpspread3.Sheets[0].RowCount;
                            Fpspread3.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            //Fpspread3.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            Fpspread3.Sheets[0].FrozenRowCount = 0;
                            Fpspread3.Visible = true;
                            Fpspread3.SaveChanges();
                            //btn_selecteditem.Visible = true;
                            lbl_baseerror.Visible = false;
                            pop_individualitem.Visible = false;
                            selectvendor_div.Visible = false;
                        }

                    }


                }
            }
            else if (spreaditemchk > 1)
            {
                lbl_alerterror.Visible = true;
                alertmessage.Visible = true;
                lbl_alerterror.Text = "You can't select more then one vendor";
            }
            else if (spreaditemchk < 1)
            {
                lbl_alerterror.Visible = true;
                alertmessage.Visible = true;
                lbl_alerterror.Text = "Please select any one vendor";
            }
        }
        catch
        {

        }
    }
    protected void btn_vendorExit_Click(object sender, EventArgs e)
    {
        selectvendor_div.Visible = false;
    }
    //request and purchase order
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> requestpo(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select distinct RequestCode from RQ_Requisition rq,RQ_RequisitionDet rd where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='0' and RequestCode like '" + prefixText + "%' order by RequestCode";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["RequestCode"].ToString());
            }
        }
        return name;
    }
    protected void txt_requestcode_base_onchange(object sender, EventArgs e)
    {
        try
        {
            string q1 = d2.GetFunction("select distinct RequestCode from RQ_Requisition rq,RQ_RequisitionDet rd where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='0' and RequestCode='" + txt_requestcode.Text + "' order by RequestCode");
            if (q1.Trim() != "" && q1.Trim() != "0")
            {
                lbl_baseerror.Visible = false;
                Session["vendorpk"] = d2.GetFunction("select distinct vd.VenItemFK from RQ_Requisition rq,RQ_RequisitionDet rd,IM_VendorItemDept vd where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='0' and vd.ItemFK=rd.ItemFK and rq.RequestCode='" + q1 + "'");
                btn_popgo_Click(sender, e);
            }
            else
            {
                txt_popsearchvendor.Text = "";
                lbl_baseerror.Visible = true;
                vendorsearch_div.Visible = false;
                lbl_baseerror.Text = "Please enter the valid request code";
            }
        }
        catch { }
    }
    protected void imagebtnpopclose5_Click(object sender, EventArgs e)
    {
        reqpurchaseorder_div.Visible = false;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> requestsearchitem(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select distinct i.itemname from RQ_Requisition rq,RQ_RequisitionDet rd,IM_VendorItemDept vd,IM_ItemMaster i where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='0' and vd.ItemFK=rd.ItemFK and i.itempk=vd.ItemFK and i.ItemName like '" + prefixText + "%' order by ItemName";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["itemname"].ToString());
            }
        }
        return name;
    }
    protected void cbl_request_selectedindexchange(object sender, EventArgs e)
    {
        int i = 0;
        cb_request.Checked = false;
        int commcount = 0;
        txt_reqcode.Text = "--Select--";
        for (i = 0; i < cbl_request.Items.Count; i++)
        {
            if (cbl_request.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_request.Items.Count)
            {
                cb_request.Checked = true;
            }
            txt_reqcode.Text = "Request Code(" + commcount.ToString() + ")";
        }
    }
    protected void cb_request_checkchange(object sender, EventArgs e)
    {
        txt_reqcode.Text = "--Select--";
        if (cb_request.Checked == true)
        {
            for (int i = 0; i < cbl_request.Items.Count; i++)
            {
                cbl_request.Items[i].Selected = true;
            }
            txt_reqcode.Text = "Request Code(" + (cbl_request.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_request.Items.Count; i++)
            {
                cbl_request.Items[i].Selected = false;
            }
        }
    }
    protected void bindrequestcode()
    {
        try
        {
            string q1 = "";
            if (purchaseordertype.Trim() == "4")
            {
                q1 = "select distinct RequestCode from RQ_Requisition rq,RQ_RequisitionDet rd where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='0'  and RequestType='1' order by RequestCode desc"; //,itemfk
            }
            if (purchaseordertype.Trim() == "2")
            {
                q1 = "select distinct RequestCode from RQ_Requisition rq,RQ_RequisitionDet rd where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='1'  and RequestType='1' order by RequestCode desc";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(q1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                ddl_requestcode.DataSource = ds;
                ddl_requestcode.DataTextField = "RequestCode";
                ddl_requestcode.DataValueField = "RequestCode";
                ddl_requestcode.DataBind();

                cbl_request.DataSource = ds;
                cbl_request.DataTextField = "RequestCode";
                cbl_request.DataValueField = "RequestCode";
                cbl_request.DataBind();
                if (cbl_request.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_request.Items.Count; i++)
                    {
                        cbl_request.Items[i].Selected = true;
                    }
                    txt_reqcode.Text = "Request code(" + cbl_request.Items.Count + ")";
                }
            }
            else
            {
                txt_reqcode.Text = "--Select--";
            }
            bind_itemname();
        }
        catch
        {
        }
    }
    //end
    // vendor request and approval
    protected void txt_appven_base_onchange(object sender, EventArgs e)
    {
        try
        {
            //txt_appven
        }
        catch
        {

        }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> app_vendorsearch(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select distinct vm.VendorCompName,vm.VendorPK from RQ_Requisition rq,RQ_RequisitionDet rd,IM_VendorItemDept vd,CO_VendorMaster vm where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='1' and rd.ReqAppStatus='1' and vd.ItemFK=rd.ItemFK and vd.VenItemFK=vm.VendorPK and VendorCompName like '" + prefixText + "%' order by VendorCompName ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["VendorCompName"].ToString());
            }
        }
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> appvenitems(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "";

        if (itembook == "1")
            query = "select distinct i.itemname from RQ_Requisition rq,RQ_RequisitionDet rd,IM_VendorItemDept vd,IM_ItemMaster i where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='1' and rd.ReqAppStatus='1' and vd.ItemFK=rd.ItemFK and i.ItemPK=vd.ItemFK and i.ItemName like '" + prefixText + "%' order by ItemName";
        if (itembook == "2")
            query = "select distinct bd.Title itemname  from RQ_Requisition rq,RQ_RequisitionDet rd,bookdetails bd where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='1' and rd.ReqAppStatus='1'  and bd.BookID=rd.Lib_Bookid and bd.Title like '" + prefixText + "%' order by Title";
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
    //end vendor request and approval


    protected void Button_Backpage_Click(object sender, EventArgs e)
    {

        Response.Redirect("vendor_quotation_compare.aspx?@@ssss$$=" + Session["vendor"] + "s" + Session["CompCode"] + "s" + Session["ItemOrBook"]);

    }

    protected void bindrequestcode1()
    {
        try
        {
            string q1 = "";
            if (purchaseordertype.Trim() == "4")
            {
                q1 = "select distinct RequestCode from RQ_Requisition rq,RQ_RequisitionDet rd where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='0'  and RequestType='13' order by RequestCode desc"; //,itemfk
            }
            if (purchaseordertype.Trim() == "2")
            {
                q1 = "select distinct RequestCode from RQ_Requisition rq,RQ_RequisitionDet rd where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='1'  and RequestType='13' order by RequestCode desc";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(q1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                ddl_requestcode.DataSource = ds;
                ddl_requestcode.DataTextField = "RequestCode";
                ddl_requestcode.DataValueField = "RequestCode";
                ddl_requestcode.DataBind();

                cbl_request.DataSource = ds;
                cbl_request.DataTextField = "RequestCode";
                cbl_request.DataValueField = "RequestCode";
                cbl_request.DataBind();
                if (cbl_request.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_request.Items.Count; i++)
                    {
                        cbl_request.Items[i].Selected = true;
                    }
                    txt_reqcode.Text = "Request code(" + cbl_request.Items.Count + ")";
                }
            }
            else
            {
                txt_reqcode.Text = "--Select--";
            }
            bind_Bookname();
        }
        catch
        {
        }
    }

    protected void bind_Bookname()
    {
        ds.Clear();
        cb1_invitem.Items.Clear();
        //ds = d2.BindItemCodeWithOutParameter_inv();
        string q1 = "";
        if (purchaseordertype.Trim() == "1")
        {
            //q1 = "select distinct Title,BookID from bookdetails order by Title";
            q1 = "select distinct Title,BookID from bookdetails where BookID='19125' order by Title";
        }
        else if (purchaseordertype.Trim() == "2")
        {
            q1 = "select distinct bd.Title,bd.BookID  from RQ_Requisition rq,RQ_RequisitionDet rd,bookdetails bd where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='1'  and bd.BookID=rd.Lib_Bookid  and rq.RequestCode in('" + Convert.ToString(ddl_requestcode.SelectedItem.Value) + "')";//and rd.ReqAppStatus='1'
        }
        else if (purchaseordertype.Trim() == "4")
        {
            q1 = "select distinct bd.Title,bd.BookID from RQ_Requisition rq,RQ_RequisitionDet rd,bookdetails bd where rq.RequisitionPK=rd.RequisitionFK and rq.ReqAppStatus='0' and rd.ReqAppStatus='0' and bd.BookID=rd.Lib_Bookid  and rq.RequestCode in('" + Convert.ToString(ddl_requestcode.SelectedItem.Value) + "') ";
        }

        ds = d2.select_method_wo_parameter(q1, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cb1_invitem.DataSource = ds;
            cb1_invitem.DataTextField = "Title";
            cb1_invitem.DataValueField = "BookID";
            cb1_invitem.DataBind();

            if (cb1_invitem.Items.Count > 0)
            {
                for (int i = 0; i < cb1_invitem.Items.Count; i++)
                {
                    cb1_invitem.Items[i].Selected = false;
                }

                txt_ind_item.Text = "Book Name(" + cb1_invitem.Items.Count + ")";
            }
        }
        else
        {
            txt_ind_item.Text = "--Select--";
        }
    }

    protected void Rad_item_Click(object sender, EventArgs e)
    {
        itembook = "1";
        lbl_selectitem3.Text = "Select the Item";
        lbl_searchitem.Text = "Search Item Name";
        lbl_2itemname.Text = "Item Name";
        if (purchaseordertype.Trim() == "3")
        {
            lblvendorname.Text = "Vendor Name";
            txt_invitem.Visible = false;
            txt_requestcode.Visible = false;
            txt_appven.Visible = false;

        }
        else if (purchaseordertype.Trim() == "1")
        {
            ddl_requestcode.Visible = false;
            lbl_itemtype3.Text = "Item Name";
            txtvendorname.Visible = false;
            txt_invitem.Visible = true;
            txt_requestcode.Visible = false;
            lblvendorname.Text = "Item Name";
            txt_ind_item.Visible = true;
            p51.Visible = true;
            txt_appven.Visible = false;
            btn_baseGo.Visible = false;
        }
        else if (purchaseordertype.Trim() == "4")
        {
            ddl_requestcode.Visible = true;
            lbl_itemtype3.Text = "Request Code";
            txt_invitem.Visible = false;
            txtvendorname.Visible = false;
            lblvendorname.Text = "Request Code";
            txt_requestcode.Visible = true;
            txt_ind_item.Visible = true;
            p51.Visible = true;
            //txt_reqcode.Visible = true;
            // Panel1.Visible = true;
            bindrequestcode();
            txt_appven.Visible = false;
            lbl_2itemname.Visible = true;
            lbl_2itemname.Text = "Item Name";
            btn_baseGo.Visible = false;
        }
        else if (purchaseordertype.Trim() == "2")
        {
            ddl_requestcode.Visible = true;
            txt_invitem.Visible = false;
            txtvendorname.Visible = false;
            txt_requestcode.Visible = true;
            //txt_reqcode.Visible = true;
            //Panel1.Visible = true;
            lbl_itemtype3.Text = "Request Code";
            lblvendorname.Text = "Request Code";
            txt_appven.Visible = false;
            lbl_2itemname.Visible = true;
            lbl_2itemname.Text = "Item Name";
            p51.Visible = true;
            txt_ind_item.Visible = true;
            bindrequestcode();
            btn_baseGo.Visible = false;
        }
        else
        {
            lblvendorname.Text = "Item Name";
            txt_requestcode.Visible = false;
            txt_invitem.Visible = false;
            txtvendorname.Enabled = false;
            btn_baseGo.Enabled = false;
            btnadd.Enabled = false;
            txt_appven.Visible = false;
            lbl_alerterror.Visible = true;
            alertmessage.Visible = true;
            lbl_alerterror.Text = "Permission is required !!!";
        }

    }
    protected void Rad_book_Click(object sender, EventArgs e)
    {
        itembook = "2";
        lbl_selectitem3.Text = "Select the Book";
        lbl_searchitem.Text = "Search Book Name";
        btnadd.Enabled = true;
        purchaseordertype = d2.GetFunction("select purchaseordertype from IM_POSettings where collegecode='" + collegecode1 + "' and settingtype='1' and settingformess='2'");
        Session["purchaseordertype"] = purchaseordertype;
        if (purchaseordertype.Trim() == "3")
        {
            lblvendorname.Text = "Vendor Name";
            txt_invitem.Visible = false;
            txt_requestcode.Visible = false;
            txt_appven.Visible = false;

        }
        else if (purchaseordertype.Trim() == "1")
        {

            lbl_itemtype3.Text = "Book Name";
            txt_search_itemname.Visible = true;
            lblvendorname.Text = "Book Name";
            ddl_requestcode.Visible = false;
            txtvendorname.Visible = false;
            txt_invitem.Visible = true;
            txt_requestcode.Visible = false;
            txt_ind_item.Visible = true;
            p51.Visible = true;
            txt_appven.Visible = false;
            btn_baseGo.Visible = false;
        }
        else if (purchaseordertype.Trim() == "4")
        {
            ddl_requestcode.Visible = true;
            lbl_itemtype3.Text = "Request Code";
            txt_invitem.Visible = false;
            txtvendorname.Visible = false;
            lblvendorname.Text = "Request Code";
            txt_requestcode.Visible = true;
            txt_ind_item.Visible = true;
            p51.Visible = true;
            txt_appven.Visible = false;
            lbl_2itemname.Visible = true;
            btn_baseGo.Visible = false;
            lbl_2itemname.Text = "Book Name";
            bindrequestcode1();
        }
        else if (purchaseordertype.Trim() == "2")
        {
            ddl_requestcode.Visible = true;
            txt_invitem.Visible = false;
            txtvendorname.Visible = false;
            txt_requestcode.Visible = true;
            bindrequestcode1();
            lbl_itemtype3.Text = "Request Code";
            lblvendorname.Text = "Request Code";
            txt_appven.Visible = false;
            lbl_2itemname.Visible = true;
            p51.Visible = true;
            txt_ind_item.Visible = true;
            btn_baseGo.Visible = false;
            lbl_2itemname.Text = "Book Name";

        }
        else
        {

            lblvendorname.Text = "Book Name";
            txt_requestcode.Visible = false;
            txt_invitem.Visible = false;
            txtvendorname.Enabled = false;
            btn_baseGo.Enabled = false;
            btnadd.Enabled = false;
            txt_appven.Visible = false;
            lbl_alerterror.Visible = true;
            alertmessage.Visible = true;
            lbl_alerterror.Text = "Permission is required !!!";
        }

    }


}