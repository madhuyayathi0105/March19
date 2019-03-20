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
public partial class Inv_Transfer : System.Web.UI.Page
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
    string dtaccessdate = DateTime.Now.ToString();
    string dtaccesstime = DateTime.Now.ToLongTimeString();
    static string hostel = ""; int k = 0;
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
            lblvalidation1.Visible = false;
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();

            if (!IsPostBack)
            {
                ddl_popstore.Visible = false;
                rdb_showall.Checked = true;
                txtpopfrom.Enabled = false;
                txt_to1.Enabled = false;
                bindhostel();
                bindhostel1();
                bindbasestore();
                storetovisiablefalse();
                fromtodatevisibletrue();
                rdb_hostohosctrlfalse();

                rdb_hostohos.Visible = false;
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
                txt_hosname.Attributes.Add("readOnly", "readOnly");
                CalendarExtender3.EndDate = DateTime.Now;
                CalendarExtender1.EndDate = DateTime.Now;
                caltodate.EndDate = DateTime.Now;
                CalendarExtender2.EndDate = DateTime.Now;
                txt_itemname.Attributes.Add("readOnly", "readOnly");
                txt_itemmeasure.Attributes.Add("readOnly", "readOnly");
                txt_transferdate.Attributes.Add("readOnly", "readOnly");

                txtpopfrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_to1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtpopfrom.Attributes.Add("readonly", "readonly");
                txt_to1.Attributes.Add("readonly", "readonly");
                txt_degree.Attributes.Add("readonly", "readonly");

                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread2.Sheets[0].RowCount = 0;
                FpSpread2.Sheets[0].ColumnCount = 0;
                rptprint.Visible = false;
                ddl_option.SelectedIndex = 0;
                lbl_hostelname.Visible = true;
                upp1.Visible = true;
                rdo_acodomicdept.Checked = true;
                binddept();
                bind_dept1();
                requeststaff();
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
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        popwindow3.Visible = false;
    }

    public void bindhostel()
    {
        try
        {
            ds.Clear();
            cbl_hos.Items.Clear();
            //ds = d2.BindHostel(collegecode1);//Idhris 10/10/2015
            // ds = d2.Bindmess_inv(collegecode1);
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hos.DataSource = ds;
                cbl_hos.DataTextField = "MessName";
                cbl_hos.DataValueField = "MessMasterPK";
                //cbl_hos.DataTextField = "Hostel_Name";
                //cbl_hos.DataValueField = "Hostel_code";
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
        }
        catch
        {
        }
    }
    public void bindbasestore()
    {
        try
        {
            ds.Clear();
            cbl_mainstore.Items.Clear();
            //ds = d2.BindHostel(collegecode1);//Idhris 10/10/2015
            string storepk = d2.GetFunction("select value from Master_Settings where settings='Store Rights' and usercode='" + usercode + "'");
            ds = d2.BindStorebaseonrights_inv(storepk);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_mainstore.DataSource = ds;
                cbl_mainstore.DataTextField = "StoreName";
                cbl_mainstore.DataValueField = "StorePK";
                //cbl_hos.DataTextField = "Hostel_Name";
                //cbl_hos.DataValueField = "Hostel_code";
                cbl_mainstore.DataBind();

                if (cbl_mainstore.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_mainstore.Items.Count; i++)
                    {
                        cbl_mainstore.Items[i].Selected = true;
                    }

                    txt_basestore.Text = "Store(" + cbl_mainstore.Items.Count + ")";
                }
            }
            else
            {
                txt_basestore.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void ddl_option_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddl_option.SelectedItem.Text == "Mess Name" && ddl_option.SelectedIndex == 0)
            {
                lbl_hostelname.Visible = true;
                upp1.Visible = true;
                lblstorename.Visible = false;
                UpdatePanel1.Visible = false;
                lbl_degree.Visible = false;
                Upp4.Visible = false;
            }
            else if (ddl_option.SelectedItem.Text == "Store Name" && ddl_option.SelectedIndex == 1)
            {
                lblstorename.Visible = true;
                UpdatePanel1.Visible = true;
                lbl_hostelname.Visible = false;
                upp1.Visible = false;
                lbl_degree.Visible = false;
                Upp4.Visible = false;
            }
            else if (ddl_option.SelectedItem.Text == "Department" && ddl_option.SelectedIndex == 2)
            {
                lblstorename.Visible = false;
                UpdatePanel1.Visible = false;
                lbl_hostelname.Visible = false;
                upp1.Visible = false;
                lbl_degree.Visible = true;
                Upp4.Visible = true;
            }
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
            txt_hosname.Text = "Mess(" + (cbl_hos.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_hos.Items.Count; i++)
            {
                cbl_hos.Items[i].Selected = false;
            }
        }
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
            txt_hosname.Text = "Mess(" + commcount.ToString() + ")";
        }
    }

    protected void cb_mainstore_CheckedChange(object sender, EventArgs e)
    {
        int cout = 0;
        txt_basestore.Text = "---Select---";
        if (cb_mainstore.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_mainstore.Items.Count; i++)
            {
                cbl_mainstore.Items[i].Selected = true;
            }
            txt_basestore.Text = "Store(" + (cbl_mainstore.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_mainstore.Items.Count; i++)
            {
                cbl_mainstore.Items[i].Selected = false;
            }
        }
    }
    protected void cbl_mainstore_SelectedIndexChange(object sender, EventArgs e)
    {
        int i = 0;
        cb_mainstore.Checked = false;
        int commcount = 0;
        txt_basestore.Text = "--Select--";
        for (i = 0; i < cbl_mainstore.Items.Count; i++)
        {
            if (cbl_mainstore.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_mainstore.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_mainstore.Items.Count)
            {
                cb_mainstore.Checked = true;
            }
            txt_basestore.Text = "Store(" + commcount.ToString() + ")";
        }
    }

    public void bindhostel1()
    {
        try
        {
            ds.Clear();
            ddl_hostel1.Items.Clear();
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_hostel1.DataSource = ds;
                ddl_hostel1.DataTextField = "MessName";
                ddl_hostel1.DataValueField = "MessMasterPk";
                ddl_hostel1.DataBind();

                ddl_hostelname3.DataSource = ds;
                ddl_hostelname3.DataTextField = "MessName";
                ddl_hostelname3.DataValueField = "MessMasterPk";
                ddl_hostelname3.DataBind();
            }
        }
        catch
        {
        }
    }
    public void bindhostel2()
    {
        try
        {
            ds.Clear();
            ddl_hostel1.Items.Clear();
            // string selectquery = "select MessMasterPK,MessName  from HM_MessMaster  where CollegeCode in ('" + collegecode1 + "') and MessMasterPK <>'" + ddl_hostelname3.SelectedItem.Value + "' ";

            string value = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            string q1 = "select MessMasterPK,MessName from HM_MessMaster where MessMasterPK not in('" + Convert.ToString(ddl_hostelname3.SelectedItem.Value) + "')  and MessMasterPK in(" + value + ") order by MessName";
            //and CollegeCode='" + collegecode1 + "'
            ds = d2.select_method_wo_parameter(q1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_hostel1.DataSource = ds;
                ddl_hostel1.DataTextField = "MessName";
                ddl_hostel1.DataValueField = "MessMasterPK";
                ddl_hostel1.DataBind();
            }
        }
        catch
        {
        }
    }

    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        rdb_showall.Checked = true;
        rdb_datewise.Checked = false;
        btn_transfergo_Click(sender, e);
        popwindow.Visible = true;


    }
    protected void txtfrom_TextChanged(object sender, EventArgs e)
    {
        try
        {

            lbl_error.Visible = false;
            string fromdate = txtpopfrom.Text;
            string todate = txt_to1.Text;
            if (fromdate != "" && fromdate != null && todate != "" && todate != null)
            {
                string[] spiltfrom = fromdate.Split('/');
                string[] spitto = todate.Split('/');
                DateTime from = Convert.ToDateTime(spiltfrom[1] + '/' + spiltfrom[0] + '/' + spiltfrom[2]);
                DateTime to = Convert.ToDateTime(spitto[1] + '/' + spitto[0] + '/' + spitto[2]);
                if (from > to)
                {
                    lbl_error.Visible = true;
                    lbl_error.Text = "Please Enter To Date Greater Than From Date";
                }
            }
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread2, reportname);
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
            string degreedetails = "Stock Transfer Report";
            string pagename = "Transfor.aspx";
            Printcontrol.loadspreaddetails(FpSpread2, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }

    }
    protected void txtto_TextChanged(object sender, EventArgs e)
    {
        try
        {

            lbl_error.Visible = false;
            string fromdate = txtpopfrom.Text;
            string todate = txt_to1.Text;
            if (fromdate != "" && fromdate != null && todate != "" && todate != null)
            {
                string[] spiltfrom = fromdate.Split('/');
                string[] spitto = todate.Split('/');
                DateTime from = Convert.ToDateTime(spiltfrom[1] + '/' + spiltfrom[0] + '/' + spiltfrom[2]);
                DateTime to = Convert.ToDateTime(spitto[1] + '/' + spitto[0] + '/' + spitto[2]);
                if (from > to)
                {
                    lbl_error.Visible = true;
                    lbl_error.Text = "Please Enter To Date Grater Than From Date";
                }
            }
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
            //lbl_error.Text = ex.ToString();
        }
    }
    public string Getordercode(string orderFK)
    {
        string OrderPK = "";
        try
        {
            OrderPK = d2.GetFunction("select OrderCode from IT_PurchaseOrder where PurchaseOrderPK='" + orderFK + "'");
        }
        catch
        {

        }
        return OrderPK;
    }
    public string GetGoodsinwardcode(string goodsFK)
    {
        string goodsPK = "";
        try
        {
            goodsPK = d2.GetFunction("select GoodsInwardCode from IT_GoodsInward where GoodsInwardPK='" + goodsFK + "'");
        }
        catch
        {

        }
        return goodsPK;
    }


    protected void btn_transfergo_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string firstdate = Convert.ToString(txtpopfrom.Text);
            string seconddate = Convert.ToString(txt_to1.Text);
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            FpSpread1.SaveChanges();
            string storepk = d2.GetFunction("select value from Master_Settings where settings='Store Rights' and usercode='" + usercode + "'");

            if (ddl_transtype.SelectedItem.Value == "0")
            {
                #region Store to mess
                string query = "";
                if (rdb_datewise.Checked == false)
                {
                    if (txt_searchitem.Text != "")
                    {
                        query = "select distinct OrderFK,InwardFK,case when InwardType='1' then 'Purchase Inward' when InwardType='2' then 'Direct Inward' when  InwardType='3' then 'Transfer' end  InwardType,InwardType as Inward_code , i.ItemCode as item_code,i.ItemName as item_name,i.ItemUnit as item_unit,SUM(CONVERT(float, ISNULL(InwardQty,0))- CONVERT(float ,ISNULL(TransferQty,0))) as hand_qty,InwardRPU as rpu,sd.StoreFK as Store_Code,s.StoreName as Store_Name,sd.ItemFK from IM_ItemMaster i,IT_StockDetail sd,IM_StoreMaster s where i.ItemPK =sd.ItemFK and sd.StoreFK =s.StorePK and i.ItemName like '" + txt_searchitem.Text + "%' group by  i.ItemCode,i.ItemName,i.ItemUnit,InwardRPU,sd.StoreFK,s.StoreName,sd.ItemFK,OrderFK,InwardFK,InwardType order by sd.ItemFK";
                        query = query + " select distinct StorePK as Store_Code,StoreName as Store_Name from IM_StoreMaster s,IT_StockDetail sd where s.StorePK =sd.StoreFK and s.StorePK in(" + storepk + ") order by StorePK";//  InwardQty replace BalQty  //17.03.16
                    }
                    else
                    {
                        query = "select distinct OrderFK,InwardFK,case when InwardType='1' then 'Purchase Inward' when InwardType='2' then 'Direct Inward' when  InwardType='3' then 'Transfer' end  InwardType,InwardType as Inward_code,i.ItemCode as item_code,i.ItemName as item_name,i.ItemUnit as item_unit,SUM(CONVERT(float, ISNULL(InwardQty,0))- CONVERT(float ,ISNULL(TransferQty,0))) as hand_qty,InwardRPU as rpu,sd.StoreFK as Store_Code,s.StoreName as Store_Name,sd.ItemFK from IM_ItemMaster i,IT_StockDetail sd,IM_StoreMaster s where i.ItemPK =sd.ItemFK and sd.StoreFK =s.StorePK group by  i.ItemCode,i.ItemName,i.ItemUnit,InwardRPU,sd.StoreFK,s.StoreName,sd.ItemFK,OrderFK,InwardFK,InwardType order by sd.ItemFK";
                        query = query + " select distinct StorePK as Store_Code,StoreName as Store_Name from IM_StoreMaster s,IT_StockDetail sd where s.StorePK =sd.StoreFK and s.StorePK in(" + storepk + ") order by StorePK";
                    }
                }
                else
                {
                    if (txt_searchitem.Text.Trim() != "")
                    {
                        query = "select distinct OrderFK,InwardFK,case when InwardType='1' then 'Purchase Inward' when InwardType='2' then 'Direct Inward' when  InwardType='3' then 'Transfer' end  InwardType,InwardType as Inward_code,i.ItemCode as item_code,i.ItemName as item_name,i.ItemUnit as item_unit,SUM(CONVERT(float, ISNULL(sd.InwardQty,0))- CONVERT(float ,ISNULL(TransferQty,0))) as hand_qty,InwardRPU as rpu,sd.StoreFK as Store_Code,s.StoreName as Store_Name,sd.ItemFK from IM_ItemMaster i,IT_StockDetail sd,IM_StoreMaster s,IT_GoodsInward g where i.ItemPK =sd.ItemFK and sd.StoreFK =s.StorePK and g.GoodsInwardPK =sd.InwardFK and g.GoodsInwardDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'  and i.ItemName like '" + txt_searchitem.Text + "%' group by  i.ItemCode,i.ItemName,i.ItemUnit,InwardRPU,sd.StoreFK,s.StoreName,sd.ItemFK,OrderFK,InwardFK,InwardType order by sd.ItemFK";
                        query = query + " select distinct StorePK as Store_Code,StoreName as Store_Name from IM_StoreMaster s,IT_StockDetail sd where s.StorePK =sd.StoreFK and s.StorePK in(" + storepk + ") order by StorePK";
                    }
                    else
                    {
                        query = "select distinct OrderFK,InwardFK,case when InwardType='1' then 'Purchase Inward' when InwardType='2' then 'Direct Inward' when  InwardType='3' then 'Transfer' end  InwardType,InwardType as Inward_code,i.ItemCode as item_code,i.ItemName as item_name,i.ItemUnit as item_unit,SUM(CONVERT(float, ISNULL(sd.InwardQty,0))- CONVERT(float ,ISNULL(TransferQty,0))) as hand_qty,InwardRPU as rpu,sd.StoreFK as Store_Code,s.StoreName as Store_Name,sd.ItemFK from IM_ItemMaster i,IT_StockDetail sd,IM_StoreMaster s,IT_GoodsInward g where i.ItemPK =sd.ItemFK and sd.StoreFK =s.StorePK and g.GoodsInwardPK =sd.InwardFK and InwardType='1' and g.GoodsInwardDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' group by  i.ItemCode,i.ItemName,i.ItemUnit,InwardRPU,sd.StoreFK,s.StoreName,sd.ItemFK,OrderFK,InwardFK,InwardType order by sd.ItemFK";
                        query = query + " select distinct StorePK as Store_Code,StoreName as Store_Name from IM_StoreMaster s,IT_StockDetail sd where s.StorePK =sd.StoreFK and s.StorePK in(" + storepk + ") order by StorePK";
                    }
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                DataView dv = new DataView();
                if (ds.Tables[1].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
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
                        FpSpread1.Columns[0].Width = 50;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Order Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[1].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Inward Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[2].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Inward Type";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[3].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Item Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[4].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Item Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[5].Width = 200;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Total Stocks";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[6].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Rpu";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[7].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Transfer Quantity";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[8].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Mess Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[9].Width = 150;
                        int sno = 0;
                        for (int ik = 0; ik < ds.Tables[1].Rows.Count; ik++)
                        {
                            ds.Tables[0].DefaultView.RowFilter = " Store_Code='" + Convert.ToString(ds.Tables[1].Rows[ik]["Store_Code"]) + "'";
                            dv = ds.Tables[0].DefaultView;

                            if (dv.Count > 0)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(Convert.ToString(ds.Tables[1].Rows[ik]["Store_Name"]));
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[1].Rows[ik]["Store_Code"]);
                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 10);
                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].ForeColor = Color.Green;
                                for (int i = 0; i < dv.Count; i++)
                                {
                                    sno++;
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    //OrderFK,InwardFK
                                    string ordercode = Getordercode(Convert.ToString(dv[i]["OrderFK"]));
                                    if (ordercode == "0" || ordercode == "")
                                    {
                                        ordercode = "-";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ordercode;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dv[i]["OrderFK"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";


                                    string inwardcode = GetGoodsinwardcode(Convert.ToString(dv[i]["InwardFK"]));
                                    if (inwardcode == "0" || inwardcode == "")
                                    {
                                        inwardcode = "-";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = inwardcode;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dv[i]["InwardFK"]);

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                    string inwardtype = Convert.ToString(dv[i]["InwardType"]);
                                    if (inwardtype == "0" || inwardtype == "")
                                    {
                                        inwardtype = "-";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    }

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = inwardtype;// Convert.ToString(dv[i]["InwardType"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(dv[i]["Inward_code"]);
                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[i]["item_code"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(dv[i]["item_unit"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";


                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dv[i]["item_name"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(dv[i]["rpu"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dv[i]["hand_qty"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(dv[i]["Store_Code"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dv[i]["rpu"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Tag = Convert.ToString(dv[i]["ItemFK"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString("");
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString("");
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                                }
                            }
                        }
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        //FpSpread1.Sheets[0].FrozenRow.
                        //FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread1.Visible = true;
                        FpSpread1.SaveChanges();
                        spreaddiv.Visible = true;

                        lbl_error1.Visible = false;
                        //rptprint.Visible = true;
                        popwindow.Visible = true;
                        btn_exit1.Visible = true;
                        btn_Transfer.Visible = true;
                    }
                    else
                    {
                        lbl_error1.Visible = true;
                        FpSpread1.Visible = false;
                        spreaddiv.Visible = false;
                        lbl_error1.Text = "No Record Found";
                        btn_exit1.Visible = false;
                        //rptprint.Visible = false;
                        //  popwindow.Visible = false;
                        btn_Transfer.Visible = false;
                    }
                }
                else
                {
                    lbl_error1.Visible = true;
                    FpSpread1.Visible = false;
                    spreaddiv.Visible = false;
                    lbl_error1.Text = "No Record Found";
                    btn_exit1.Visible = false;
                    //rptprint.Visible = false;
                    //  popwindow.Visible = false;
                    btn_Transfer.Visible = false;
                }
                #endregion
            }
            if (ddl_transtype.SelectedItem.Value == "1")
            {
                #region Mess to Mess
                string query2 = "";
                if (txt_searchitem.Text.Trim() != "")
                {
                    query2 = "select distinct i.ItemCode as item_code,i.ItemName as item_name,i.ItemUnit as item_unit,sd.BalQty as AvlQty,sd.IssuedRPU as RPU,sd.ItemFK,sd.DeptFK from IM_ItemMaster i,IT_StockDeptDetail sd where i.ItemPK =sd.ItemFK and sd.DeptFK ='" + ddl_hostelname3.SelectedItem.Value + "' and i.ItemName like '" + txt_searchitem.Text + "%'";//OrderFK,InwardFK,case when Inward_Type='1' then 'Purchase Inward' when Inward_Type='2' then 'Direct Inward' when  Inward_Type='3' then 'Transfer' when Inward_Type='' then 'Openning Stock' end  Inward_Type,Inward_Type as Inward_code,
                }
                else
                {
                    query2 = "select distinct i.ItemCode as item_code,i.ItemName as item_name,i.ItemUnit as item_unit,sd.BalQty as AvlQty,sd.IssuedRPU as RPU,sd.ItemFK,sd.DeptFK from IM_ItemMaster i,IT_StockDeptDetail sd where i.ItemPK =sd.ItemFK and sd.DeptFK ='" + ddl_hostelname3.SelectedItem.Value + "'";// OrderFK,InwardFK,case when Inward_Type='1' then 'Purchase Inward' when Inward_Type='2' then 'Direct Inward' when  Inward_Type='3' then 'Transfer' end  Inward_Type,Inward_Type as Inward_code,
                }
                ds1.Clear();
                ds1 = d2.select_method_wo_parameter(query2, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
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
                    FpSpread1.Columns[0].Width = 50;
                    //
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Order Code";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[1].Width = 100;
                    FpSpread1.Columns[1].Visible = false;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Inward Code";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[2].Width = 100;
                    FpSpread1.Columns[2].Visible = false;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Inward Type";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[3].Width = 100;
                    FpSpread1.Columns[3].Visible = false;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Item Code";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[4].Width = 100;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Item Name";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[5].Width = 200;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Available Stocks";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[6].Width = 100;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Rpu";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[7].Width = 100;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Transfer Quantity";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[8].Width = 100;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Mess Name";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[9].Width = 150;

                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        #region 04.04.16


                        //string ordercode = Getordercode(Convert.ToString(ds1.Tables[0].Rows[i]["OrderFK"]));
                        //if (ordercode == "0" || ordercode == "")
                        //{
                        //    ordercode = "-";
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        //}
                        //else
                        //{
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        //}
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ordercode;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString((ds1.Tables[0].Rows[i]["OrderFK"]));
                        ////FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        //string inwardcode = GetGoodsinwardcode(Convert.ToString((ds1.Tables[0].Rows[i]["InwardFK"])));
                        //if (inwardcode == "0" || inwardcode == "")
                        //{
                        //    inwardcode = "-";
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        //}
                        //else
                        //{
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        //}
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = inwardcode;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString((ds1.Tables[0].Rows[i]["InwardFK"]));
                        //// FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        //string inwardtype = Convert.ToString((ds1.Tables[0].Rows[i]["Inward_Type"]));
                        //if (inwardtype == "0" || inwardtype == "")
                        //{
                        //    inwardtype = "-";
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        //}
                        //else
                        //{
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        //}
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = inwardtype;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString((ds1.Tables[0].Rows[i]["Inward_code"]));
                        ////FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                        #endregion

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds1.Tables[0].Rows[i]["item_code"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds1.Tables[0].Rows[i]["item_unit"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds1.Tables[0].Rows[i]["item_name"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds1.Tables[0].Rows[i]["rpu"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds1.Tables[0].Rows[i]["AvlQty"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(ds1.Tables[0].Rows[i]["ItemFK"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds1.Tables[0].Rows[i]["rpu"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Tag = Convert.ToString(ds1.Tables[0].Rows[i]["DeptFK"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString("");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString("");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                    }
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;

                    FpSpread1.Visible = true;
                    FpSpread1.SaveChanges();
                    spreaddiv.Visible = true;
                    lbl_error1.Visible = false;
                    //rptprint.Visible = true;
                    popwindow.Visible = true;
                    btn_exit1.Visible = true;
                    btn_Transfer.Visible = true;
                }
                else
                {
                    lbl_error1.Visible = true;
                    FpSpread1.Visible = false;
                    spreaddiv.Visible = false;
                    lbl_error1.Text = "No Record Found";
                    // popwindow.Visible = false;
                    btn_Transfer.Visible = false;
                    btn_exit1.Visible = false;
                }
                txt_searchitem.Text = "";
                #endregion
            }
            else if (ddl_transtype.SelectedItem.Value == "2")
            {
                //Alter InwardQty as BalQty
                #region Store to store

                string selectquery = "";
                if (txt_storetostore.Text.Trim() != "")
                {
                    selectquery = "select OrderFK,InwardFK,case when InwardType='1' then 'Purchase Inward' when InwardType='2' then 'Direct Inward' when  InwardType='3' then 'Transfer' end  InwardType,InwardType as Inward_code, i.ItemCode as item_code,i.ItemName as item_name,i.ItemUnit as item_unit,SUM(InwardQty-ISNULL(TransferQty,0 ))as hand_qty,InwardRPU as rpu,sd.ItemFK,sd.StoreFK from IM_ItemMaster i,IT_StockDetail sd,IM_StoreMaster sm where i.ItemPK =sd.ItemFK and sm.StorePK=sd.StoreFK and sm.StorePK='" + ddl_storename.SelectedItem.Value + "' and sm.StoreName like '" + txt_storetostore.Text + "%' group by  i.ItemCode,i.ItemName,i.ItemUnit,InwardRPU,sd.StoreFK,sm.StoreName,sd.ItemFK,OrderFK,InwardFK,InwardType order by sd.ItemFK";
                }
                else
                {
                    selectquery = "select  OrderFK,InwardFK,case when InwardType='1' then 'Purchase Inward' when InwardType='2' then 'Direct Inward' when  InwardType='3' then 'Transfer' end  InwardType,InwardType as Inward_code,i.ItemCode as item_code,i.ItemName as item_name,i.ItemUnit as item_unit,SUM(InwardQty-ISNULL(TransferQty,0 ))as hand_qty,InwardRPU as rpu,sd.ItemFK,sd.StoreFK from IM_ItemMaster i,IT_StockDetail sd,IM_StoreMaster sm where i.ItemPK =sd.ItemFK and sm.StorePK=sd.StoreFK and sm.StorePK='" + ddl_storename.SelectedItem.Value + "' group by  i.ItemCode,i.ItemName,i.ItemUnit,InwardRPU,sd.StoreFK,sm.StoreName,sd.ItemFK,OrderFK,InwardFK,InwardType order by sd.ItemFK";
                }

                if (selectquery.Trim() != "")
                {
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
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
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Columns[0].Width = 50;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Order Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[1].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Inward Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[2].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Inward Type";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[3].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Item Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[4].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Item Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[5].Width = 200;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Available Stock";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[6].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Rpu";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[7].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Transfer Quantity";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[8].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Transfer Store";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[9].Width = 150;
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            string ordercode = Getordercode(Convert.ToString(ds.Tables[0].Rows[row]["OrderFK"]));
                            if (ordercode == "0" || ordercode == "")
                            {
                                ordercode = "-";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            }


                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ordercode;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["OrderFK"]);
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                            string inwardcode = GetGoodsinwardcode(Convert.ToString(ds.Tables[0].Rows[row]["InwardFK"]));
                            if (inwardcode == "0" || inwardcode == "")
                            {
                                inwardcode = "-";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            }

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = inwardcode;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["InwardFK"]);
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            string inwardtype = Convert.ToString((ds.Tables[0].Rows[row]["InwardType"]));
                            if (inwardtype == "0" || inwardtype == "")
                            {
                                inwardtype = "-";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            }

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = inwardtype;// Convert.ToString(ds.Tables[0].Rows[row]["InwardType"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Inward_code"]);
                            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["item_code"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[row]["item_unit"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";


                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["item_name"]);
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[row]["rpu"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["hand_qty"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(ds.Tables[0].Rows[row]["ItemFK"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[row]["rpu"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Tag = Convert.ToString(ds.Tables[0].Rows[row]["StoreFk"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = "";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                        }
                        FpSpread1.Visible = true;
                        // rptprint.Visible = true;
                        spreaddiv.Visible = true;
                        btn_exit1.Visible = true;
                        btn_Transfer.Visible = true;
                        lbl_error.Visible = false;
                        lbl_error1.Visible = false;
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    }
                    else
                    {
                        btn_exit1.Visible = false;
                        btn_Transfer.Visible = false;
                        spreaddiv.Visible = false;
                        FpSpread1.Visible = false;
                        rptprint.Visible = false;
                        lbl_error1.Visible = true;
                        lbl_error1.Text = "No Records Found";
                    }
                }
                else
                {
                    spreaddiv.Visible = false;
                    FpSpread1.Visible = false;
                    rptprint.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "Please Select Any one Item Name";
                }
                #endregion
            }
            else if (ddl_transtype.SelectedItem.Value == "3")
            {
                #region Store to department
                string query = "";
                query = " select distinct OrderFK,InwardFK,case when InwardType='1' then 'Purchase Inward' when InwardType='2' then 'Direct Inward' when  InwardType='3' then 'Transfer' when isnull(InwardType,0)=0 then 'Openning Stock' end  InwardType,InwardType as Inward_code,i.ItemCode as item_code,i.ItemName as item_name,i.ItemUnit as item_unit,SUM(CONVERT(float, ISNULL(sd.InwardQty,0))- CONVERT(float ,ISNULL(TransferQty,0))) as hand_qty,InwardRPU as rpu,sd.StoreFK as Store_Code,s.StoreName as Store_Name,sd.ItemFK from IM_ItemMaster i,IT_StockDetail sd,IM_StoreMaster s,IM_ItemDeptMaster dp where i.ItemPK =sd.ItemFK and sd.StoreFK =s.StorePK  and dp.ItemFK =sd.ItemFK and dp.ItemDeptFK in('" + ddl_acadamic.SelectedItem.Value + "') group by i.ItemCode,i.ItemName,i.ItemUnit,InwardRPU,sd.StoreFK,s.StoreName,sd.ItemFK,OrderFK,InwardFK,InwardType,InwardType order by sd.ItemFK ";//and InwardType='3' 

                //query = "select  OrderFK,InwardFK,case when InwardType='1' then 'Purchase Inward' when InwardType='2' then 'Direct Inward' when  InwardType='3' then 'Transfer' end  InwardType,InwardType as Inward_code,i.ItemCode as item_code,i.ItemName as item_name,i.ItemUnit as item_unit,SUM(InwardQty-ISNULL(TransferQty,0 ))as hand_qty,InwardRPU as rpu,sd.ItemFK,sd.StoreFK from IM_ItemMaster i,IT_StockDetail sd,IM_StoreMaster sm where i.ItemPK =sd.ItemFK and sm.StorePK=sd.StoreFK group by i.ItemCode,i.ItemName,i.ItemUnit,InwardRPU,sd.StoreFK,sm.StoreName,sd.ItemFK,OrderFK,InwardFK,InwardType order by sd.ItemFK";

                query = query + " select distinct StorePK as Store_Code,StoreName as Store_Name from IM_StoreMaster s,IT_StockDetail sd where s.StorePK =sd.StoreFK  and s.StorePK in(" + storepk + ")  order by StorePK";

                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                DataView dv = new DataView();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
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
                        FpSpread1.Columns[0].Width = 50;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Order Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[1].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Inward Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[2].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Inward Type";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[3].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Item Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[4].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Item Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[5].Width = 200;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Total Stocks";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[6].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Rpu";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[7].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Transfer Quantity";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[8].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Mess Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[9].Width = 150;
                        int sno = 0;
                        for (int ik = 0; ik < ds.Tables[1].Rows.Count; ik++)
                        {
                            ds.Tables[0].DefaultView.RowFilter = " Store_Code='" + Convert.ToString(ds.Tables[1].Rows[ik]["Store_Code"]) + "'";
                            dv = ds.Tables[0].DefaultView;
                            if (dv.Count > 0)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(Convert.ToString(ds.Tables[1].Rows[ik]["Store_Name"]));
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[1].Rows[ik]["Store_Code"]);
                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 10);
                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].ForeColor = Color.Green;
                                for (int i = 0; i < dv.Count; i++)
                                {
                                    sno++;
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                    string ordercode = Getordercode(Convert.ToString(dv[i]["OrderFK"]));
                                    if (ordercode == "0" || ordercode == "")
                                    {
                                        ordercode = "-";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    }

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ordercode;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dv[i]["OrderFK"]);
                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                    string inwardcode = GetGoodsinwardcode(Convert.ToString(dv[i]["InwardFK"]));
                                    if (inwardcode == "0" || inwardcode == "")
                                    {
                                        inwardcode = "-";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    }

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = inwardcode;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dv[i]["InwardFK"]);
                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                    string inwardtype = Convert.ToString((ds.Tables[0].Rows[i]["InwardType"]));
                                    if (inwardtype == "0" || inwardtype == "")
                                    {
                                        inwardtype = "-";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    }

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = inwardtype;// Convert.ToString(dv[i]["InwardType"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(dv[i]["Inward_code"]);
                                    //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[i]["item_code"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(dv[i]["item_unit"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";


                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dv[i]["item_name"]);
                                    // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(dv[i]["rpu"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dv[i]["hand_qty"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(dv[i]["Store_Code"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dv[i]["rpu"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Tag = Convert.ToString(dv[i]["ItemFK"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString("");
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                                }
                            }
                        }
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        FpSpread1.Visible = true;
                        FpSpread1.SaveChanges();
                        spreaddiv.Visible = true;
                        lbl_error1.Visible = false;
                        popwindow.Visible = true;
                        btn_exit1.Visible = true;
                        btn_Transfer.Visible = true;
                    }
                    else
                    {
                        lbl_error1.Visible = true;
                        FpSpread1.Visible = false;
                        spreaddiv.Visible = false;
                        lbl_error1.Text = "No Record Found";
                        btn_exit1.Visible = false;
                        btn_Transfer.Visible = false;
                    }
                }
                else
                {
                    lbl_error1.Visible = true;
                    FpSpread1.Visible = false;
                    spreaddiv.Visible = false;
                    lbl_error1.Text = "No Record Found";
                    btn_exit1.Visible = false;
                    btn_Transfer.Visible = false;
                }
                #endregion
            }
            else if (ddl_transtype.SelectedItem.Value == "4" || ddl_transtype.SelectedItem.Value == "5")
            {
                #region department to department
                string query2 = "";

                query2 = "select distinct i.ItemCode as item_code,i.ItemName as item_name,i.ItemUnit as item_unit,sd.BalQty as AvlQty,sd.IssuedRPU as RPU,sd.ItemFK,sd.DeptFK from IM_ItemMaster i,IT_StockDeptDetail sd,Department d where i.ItemPK =sd.ItemFK and d.Dept_Code=sd.DeptFK and  sd.DeptFK ='" + Convert.ToString(ddl_acadamic.SelectedItem.Value) + "'";

                ds1.Clear();
                ds1 = d2.select_method_wo_parameter(query2, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    string name = "";
                    if (ddl_transtype.SelectedItem.Value == "4")
                    {
                        name = "Department Name";
                    }
                    else if (ddl_transtype.SelectedItem.Value == "5")
                    {
                        name = "Store Name";
                    }
                    string q1 = "S.No/Item Code/Item Name/Available Stocks/Rpu/Transfer Quantity/" + name + "";
                    Fpreadheaderbindmethod(q1, FpSpread1, "TRUE");
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds1.Tables[0].Rows[i]["item_code"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds1.Tables[0].Rows[i]["item_unit"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds1.Tables[0].Rows[i]["item_name"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds1.Tables[0].Rows[i]["rpu"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds1.Tables[0].Rows[i]["AvlQty"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds1.Tables[0].Rows[i]["ItemFK"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds1.Tables[0].Rows[i]["rpu"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds1.Tables[0].Rows[i]["DeptFK"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString("");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString("");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                    }
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.Visible = true;
                    FpSpread1.SaveChanges();
                    spreaddiv.Visible = true;
                    lbl_error1.Visible = false;
                    popwindow.Visible = true;
                    btn_exit1.Visible = true;
                    btn_Transfer.Visible = true;
                }
                else
                {
                    lbl_error1.Visible = true;
                    FpSpread1.Visible = false;
                    spreaddiv.Visible = false;
                    lbl_error1.Text = "No Record Found";
                    btn_exit1.Visible = false;
                    btn_Transfer.Visible = false;
                }
                #endregion
            }
        }
        catch
        {
        }
    }

    public void cb_degree_checkedchange(object sender, EventArgs e)
    {
        try
        {
            string buildvalue1 = "";
            string build1 = "";

            if (cb_degree.Checked == true)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {

                    if (cb_degree.Checked == true)
                    {
                        cbl_degree.Items[i].Selected = true;
                        txt_degree.Text = "Degree(" + (cbl_degree.Items.Count) + ")";
                        build1 = cbl_degree.Items[i].Value.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;

                        }

                    }
                }
            }
            else
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                    txt_degree.Text = "--Select--";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;

            cb_degree.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    build = cbl_degree.Items[i].Value.ToString();
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
            if (seatcount == cbl_degree.Items.Count)
            {
                txt_degree.Text = "Degree(" + seatcount.ToString() + ")";
                cb_degree.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_degree.Text = "--Select--";
            }
            else
            {
                txt_degree.Text = "Degree(" + seatcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void bind_dept1()
    {
        try
        {
            string deptquery = "select Dept_Code ,Dept_Name  from Department where college_code ='" + collegecode1 + "' order by Dept_Code";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "Dept_Name";
                cbl_degree.DataValueField = "Dept_Code";
                cbl_degree.DataBind();

                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        cbl_degree.Items[i].Selected = true;
                    }

                    txt_degree.Text = "Department(" + cbl_degree.Items.Count + ")";
                }
            }
        }
        catch
        { }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            if (txt_fromdate.Text.Trim() != "" && txt_todate.Text.Trim() != "" && txt_hosname.Text.Trim() != "---Select---")
            {
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate.Text);
                string seconddate = Convert.ToString(txt_todate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                string[] split1 = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);

                string hostelcode = "";
                string storecode = "";
                string deptcode = "";
                if (ddl_option.SelectedItem.Text == "Mess Name" && ddl_option.SelectedIndex == 0)
                {
                    for (int i = 0; i < cbl_hos.Items.Count; i++)
                    {
                        if (cbl_hos.Items[i].Selected == true)
                        {
                            if (hostelcode == "")
                            {
                                hostelcode = "" + cbl_hos.Items[i].Value.ToString() + "";
                            }
                            else
                            {
                                hostelcode = hostelcode + "'" + "," + "'" + cbl_hos.Items[i].Value.ToString() + "";
                            }
                        }
                    }
                }
                else if (ddl_option.SelectedIndex == 1)
                {
                    for (int i = 0; i < cbl_mainstore.Items.Count; i++)
                    {
                        if (cbl_mainstore.Items[i].Selected == true)
                        {
                            if (storecode == "")
                            {
                                storecode = "" + cbl_mainstore.Items[i].Value.ToString() + "";
                            }
                            else
                            {
                                storecode = storecode + "'" + "," + "'" + cbl_mainstore.Items[i].Value.ToString() + "";
                            }
                        }
                    }
                }
                else if (ddl_option.SelectedIndex == 2)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        if (cbl_degree.Items[i].Selected == true)
                        {
                            if (deptcode == "")
                            {
                                deptcode = "" + cbl_degree.Items[i].Value.ToString() + "";
                            }
                            else
                            {
                                deptcode = deptcode + "'" + "," + "'" + cbl_degree.Items[i].Value.ToString() + "";
                            }
                        }
                    }
                }

                //string q = "select i.item_code,item_name,hand_qty  from stock_master s,item_master i where s.item_code =i.item_code";
                //string q = " select CONVERT(varchar(10),transfer_date,103) as transfer_date ,MessName,h.MessId ,i.item_code,i.item_name,transfer_qty  from transfer t,item_master i,MessMaster h where t.item_code =i.item_code and h.MessId =t.hostel_code and h.MessId in ('" + hostelcode + "') and transfer_date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' order by i.item_code";

                string q = "";
                if (hostelcode.Trim() != "")
                {
                    // q = "select ItemPK,ItemCode as item_code,ItemName as item_name,TransferType,CONVERT(varchar(10),TrasnferDate,103) as transfer_date,TransferFrom,TrasferTo as MessId,TransferQty as transfer_qty from IM_ItemMaster i,IT_TransferItem t where  t.TrasnferDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and TrasferTo in ('" + hostelcode + "') and TransferType='2'";


                    q = "select ItemPK,ItemCode as item_code,ItemName as item_name,TransferType,CONVERT(varchar(10),TrasnferDate,103) as transfer_date,TransferFrom,TrasferTo as MessId,TransferQty as transfer_qty  from IT_TransferItem t,IM_ItemMaster i,HM_MessMaster m where t.itemfk=i.ItemPK and m.MessMasterPK=t.TrasferTo and t.TransferType='1' and t.TrasnferDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and t.TrasferTo in ('" + hostelcode + "') ";
                }
                if (storecode.Trim() != "")
                {
                    q = "select ItemPK,ItemCode as item_code,ItemName as item_name,TransferType,CONVERT(varchar(10),TrasnferDate,103) as transfer_date,TransferFrom,TrasferTo as MessId,TransferQty as transfer_qty from IM_ItemMaster i,IT_TransferItem t where  TransferType in('1','5') and t.itemfk=i.ItemPK and t.TrasnferDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and TrasferTo in ('" + storecode + "')";
                }
                if (deptcode.Trim() != "")
                {
                    // q = " select ItemPK,ItemCode as item_code,ItemName as item_name,TransferType,CONVERT(varchar(10),TrasnferDate,103) as transfer_date,TransferFrom,TrasferTo as MessId,TransferQty as transfer_qty from IT_TransferItem t,IM_ItemMaster i where TransferType='3' and t.itemfk=i.ItemPK and TrasferTo in('" + deptcode + "') and t.TrasnferDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'";
                    q = "  select  ItemPK,ItemCode as item_code,ItemName as item_name,TransferType,CONVERT(varchar(10),TrasnferDate,103) as transfer_date,TrasferTo as MessId,SUM(TransferQty) as transfer_qty from IT_TransferItem t,IM_ItemMaster i where TransferType in('3','4') and t.ItemFK=i.ItemPK and TrasferTo in('" + deptcode + "') and t.TrasnferDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' group by TransferType,TrasferTo,ItemFK,ItemCode,TrasnferDate,TrasferTo,ItemPK,ItemName ";
                }
                ds = d2.select_method_wo_parameter(q, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    FpSpread2.Sheets[0].RowCount = 0;
                    FpSpread2.Sheets[0].ColumnCount = 0;
                    FpSpread2.CommandBar.Visible = false;
                    FpSpread2.Sheets[0].AutoPostBack = true;
                    FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread2.Sheets[0].RowHeader.Visible = false;
                    FpSpread2.Sheets[0].ColumnCount = 6;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    FpSpread2.Columns[0].Width = 50;

                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Item Code";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread2.Columns[1].Width = 100;

                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Name";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread2.Columns[2].Width = 200;

                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Transfered Date";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    FpSpread2.Columns[3].Width = 100;

                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Transfered Quantity";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    FpSpread2.Columns[4].Width = 100;
                    string name = "";
                    if (hostelcode.Trim() != "")
                    {
                        name = "Mess Name";
                    }
                    if (storecode.Trim() != "")
                    {
                        name = "Store Name";
                    }
                    if (deptcode.Trim() != "")
                    {
                        name = "Department";
                    }

                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = name;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    FpSpread2.Columns[5].Width = 200;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FpSpread2.Sheets[0].RowCount++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["item_code"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["item_name"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["transfer_date"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["transfer_qty"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        string messorstorecode = Convert.ToString(ds.Tables[0].Rows[i]["MessId"]);
                        string messorstorename = "";
                        if (hostelcode.Trim() != "")
                        {
                            messorstorename = d2.GetFunction("select MessName from HM_MessMaster where MessMasterPK='" + messorstorecode + "'");
                        }
                        if (storecode.Trim() != "")
                        {
                            messorstorename = d2.GetFunction("select StoreName from IM_StoreMaster where StorePK='" + messorstorecode + "'");
                        }
                        if (deptcode.Trim() != "")
                        {
                            messorstorename = d2.GetFunction("select Dept_Name from Department where Dept_Code='" + messorstorecode + "'");
                        }
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = messorstorename;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Tag = messorstorecode;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    }
                    FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                    FpSpread2.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread2.Sheets[0].FrozenRowCount = 0;
                    FpSpread2.Visible = true;
                    spreaddiv1.Visible = true;
                    lbl_error.Visible = false;
                    rptprint.Visible = true;

                }
                else
                {
                    lbl_error.Visible = true;
                    FpSpread2.Visible = false;
                    spreaddiv1.Visible = false;
                    lbl_error.Text = "No Record Found";
                    rptprint.Visible = false;
                }
            }
            else
            {
                lbl_error.Visible = true;
                FpSpread2.Visible = false;
                spreaddiv1.Visible = false;
                rptprint.Visible = false;
                lbl_error.Text = "Please Select All Fields";

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
    protected void FpSpread1_Render(object sender, EventArgs e)
    {
        try
        {
            if (check == true)
            {
                if (Radio_ManTrans.Checked)
                {
                    if (ddl_transtype.SelectedItem.Value == "0")
                    {
                        #region Store to Mess

                        lbl_hostel1.Text = "Mess";
                        string activerow = "";
                        string activecol = "";
                        ddl_hostel1.Visible = true;

                        ddl_transdept.Visible = false;
                        activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
                        activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
                        collegecode = Session["collegecode"].ToString();
                        Session["activerow"] = Convert.ToString(activerow);
                        Session["activecoloumn"] = Convert.ToString(activecol);
                        if (activecol.Trim() != "0" && activecol.Trim() != "1")
                        {

                            string itemunit = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag);
                            if (itemunit.Trim() != "")
                            {
                                txt_itemmeasure.Text = itemunit;

                                string itemname = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
                                txt_itemname.Text = Convert.ToString(itemname);
                                string totalvalue = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text);
                                txt_totalQunatity.Text = Convert.ToString(totalvalue);
                                txt_transferdate.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                                string transfer = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 8].Text);
                                string hostel = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Text);
                                string hostelcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Tag);
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
                                    ddl_hostel1.SelectedItem.Text = Convert.ToString(hostel);
                                    ddl_hostel1.SelectedItem.Value = Convert.ToString(hostelcode);
                                    btn_newadd.Text = "Update";
                                }
                                bindhostel();
                                popwindow3.Visible = true;
                                btn_newadd.Visible = true;
                                div3.Visible = true;
                            }
                        }
                        #endregion
                    }
                    if (ddl_transtype.SelectedItem.Value == "1")
                    {
                        #region Mess to mess
                        lbl_hostel1.Text = "Transfer Mess";
                        lbl_fromhostel.Visible = true;
                        lbl_fromhostel.Text = "From Mess";
                        bhosname.Visible = true;
                        ddl_transdept.Visible = false;
                        ddl_hostel1.Visible = true;
                        ddl_popstore.Visible = false;

                        string activerow = "";
                        string activecol = "";
                        activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
                        activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
                        collegecode = Session["collegecode"].ToString();
                        Session["activerow"] = Convert.ToString(activerow);
                        Session["activecoloumn"] = Convert.ToString(activecol);
                        if (activecol.Trim() != "0" && activecol.Trim() != "1")
                        {
                            bindhostel1();
                            string itemunit = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag);
                            txt_itemmeasure.Text = itemunit;

                            string itemcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
                            string itemname = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
                            txt_itemname.Text = Convert.ToString(itemname);
                            string totalvalue = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text);
                            txt_totalQunatity.Text = Convert.ToString(totalvalue);
                            txt_transferdate.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                            string transfer = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 8].Text);
                            string hostel = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Text);
                            string hostelcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Tag);

                            bhosname.Text = Convert.ToString(ddl_hostelname3.SelectedItem.Text);
                            string bhostelcode = Convert.ToString(ddl_hostelname3.SelectedItem.Value);

                            ddl_hostel1.Items.Remove(ddl_hostelname3.SelectedItem.Value);

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
                                ddl_hostel1.SelectedItem.Text = Convert.ToString(hostel);
                                ddl_hostel1.SelectedItem.Value = Convert.ToString(hostelcode);
                                btn_newadd.Text = "Update";
                            }
                            popwindow3.Visible = true;
                            btn_newadd.Visible = true;
                            div3.Visible = true;
                        }
                        #endregion
                    }
                    else if (ddl_transtype.SelectedItem.Value == "2")
                    {
                        #region Store to store
                        ddl_transdept.Visible = false;
                        ddl_popstore.Visible = true;
                        ddl_hostel1.Visible = false;
                        lbl_hostel1.Text = "To Store";
                        lbl_fromhostel.Text = "From store";
                        lbl_fromhostel.Visible = true;
                        bhosname.Text = Convert.ToString(ddl_storename.SelectedItem.Text);
                        bhosname.Visible = true;
                        string activerow = "";
                        string activecol = "";
                        activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
                        activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
                        collegecode = Session["collegecode"].ToString();
                        Session["activerow"] = Convert.ToString(activerow);
                        Session["activecoloumn"] = Convert.ToString(activecol);
                        if (activecol.Trim() != "0" && activecol.Trim() != "1")
                        {
                            string itemunit = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag);
                            txt_itemmeasure.Text = itemunit;
                            string itemcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);

                            string itemname = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
                            txt_itemname.Text = itemname;

                            string handonqty = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text);
                            txt_totalQunatity.Text = handonqty;
                            string store = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Text);
                            string storecode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Tag);

                            string transfer = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 8].Text);
                            if (transfer.Trim() != "")
                            {
                                txt_transferqty.Text = Convert.ToString(transfer);
                            }
                            else
                            {
                                txt_transferqty.Text = "";
                            }

                            if (store.Trim() != "")
                            {
                                ddl_popstore.SelectedItem.Text = Convert.ToString(store);
                                ddl_popstore.SelectedItem.Value = Convert.ToString(storecode);
                                btn_newadd.Text = "Update";
                            }
                            txt_transferdate.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                        }
                        bind_popstore();
                        popwindow3.Visible = true;
                        btn_newadd.Visible = true;
                        div3.Visible = true;
                        #endregion
                    }
                    else if (ddl_transtype.SelectedItem.Value == "3")
                    {
                        #region Store to department
                        ddl_popstore.Visible = false;
                        ddl_transdept.Visible = true;
                        bindtransdept();
                        ddl_hostel1.Visible = false;
                        lbl_hostel1.Text = "Department";

                        lbl_fromhostel.Text = "";
                        bhosname.Text = "";

                        string activerow = "";
                        string activecol = "";
                        activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
                        activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
                        collegecode = Session["collegecode"].ToString();
                        Session["activerow"] = Convert.ToString(activerow);
                        Session["activecoloumn"] = Convert.ToString(activecol);
                        if (activecol.Trim() != "0" && activecol.Trim() != "1")
                        {
                            string itemunit = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag);
                            if (itemunit.Trim() != "")
                            {
                                txt_itemmeasure.Text = itemunit;
                                string itemname = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
                                txt_itemname.Text = Convert.ToString(itemname);
                                string totalvalue = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text);
                                txt_totalQunatity.Text = Convert.ToString(totalvalue);
                                txt_transferdate.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                                string transfer = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 8].Text);
                                string hostel = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Text);
                                string hostelcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Tag);
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
                                    ddl_hostel1.SelectedItem.Text = Convert.ToString(hostel);
                                    ddl_hostel1.SelectedItem.Value = Convert.ToString(hostelcode);
                                    btn_newadd.Text = "Update";
                                }
                                popwindow3.Visible = true;
                                btn_newadd.Visible = true;
                                div3.Visible = true;
                            }

                        }
                        #endregion
                    }
                    else if (ddl_transtype.SelectedItem.Value == "4")
                    {
                        #region department to department
                        bindtransdept();
                        lbl_hostel1.Text = "Transfer Department";
                        lbl_fromhostel.Visible = true;
                        lbl_fromhostel.Text = "From Department";
                        //bhosname
                        string activerow = "";
                        string activecol = "";
                        activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
                        activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
                        collegecode = Session["collegecode"].ToString();
                        Session["activerow"] = Convert.ToString(activerow);
                        Session["activecoloumn"] = Convert.ToString(activecol);
                        if (activecol.Trim() != "0" && activecol.Trim() != "1")
                        {
                            bhosname.Visible = true;
                            bhosname.Text = Convert.ToString(ddl_acadamic.SelectedItem.Text);
                            txt_itemmeasure.Text = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                            string itemname = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                            txt_itemname.Text = Convert.ToString(itemname);
                            string totalvalue = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                            txt_totalQunatity.Text = Convert.ToString(totalvalue);
                            txt_transferdate.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                            string transfer = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
                            string dept = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Tag);
                            if (transfer.Trim() != "")
                            {
                                txt_transferqty.Text = Convert.ToString(transfer);
                            }
                            else
                            {
                                txt_transferqty.Text = "";
                            }
                            if (dept.Trim() != "")
                            {
                                //ddl_hostel1.SelectedItem.Text = Convert.ToString(hostel);
                                //ddl_hostel1.SelectedItem.Value = Convert.ToString(hostelcode);
                                btn_newadd.Text = "Update";
                            }
                            popwindow3.Visible = true;
                            btn_newadd.Visible = true;
                            div3.Visible = true;
                        }
                        ddl_popstore.Visible = false;
                        ddl_hostel1.Visible = false;
                        ddl_transdept.Visible = true;
                        ddl_hostel1.Visible = false;

                        #endregion
                    }
                    else if (ddl_transtype.SelectedItem.Value == "5")
                    {
                        #region department to store
                        lbl_hostel1.Text = "Transfer Store";
                        lbl_fromhostel.Visible = true;
                        lbl_fromhostel.Text = "From Department";
                        ddl_transdept.Visible = false;
                        ddl_hostel1.Visible = false;
                        ddl_popstore.Visible = true;
                        string activerow = "";
                        string activecol = "";
                        activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
                        activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
                        collegecode = Session["collegecode"].ToString();
                        Session["activerow"] = Convert.ToString(activerow);
                        Session["activecoloumn"] = Convert.ToString(activecol);
                        if (activecol.Trim() != "0" && activecol.Trim() != "1")
                        {
                            bhosname.Text = Convert.ToString(ddl_acadamic.SelectedItem.Text);
                            bhosname.Visible = true;
                            txt_itemmeasure.Text = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                            string itemname = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                            txt_itemname.Text = Convert.ToString(itemname);
                            string totalvalue = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                            txt_totalQunatity.Text = Convert.ToString(totalvalue);
                            txt_transferdate.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                            string transfer = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
                            string dept = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Tag);
                            if (transfer.Trim() != "")
                            {
                                txt_transferqty.Text = Convert.ToString(transfer);
                            }
                            else
                            {
                                txt_transferqty.Text = "";
                            }
                            if (dept.Trim() != "")
                            {
                                //ddl_hostel1.SelectedItem.Text = Convert.ToString(hostel);
                                //ddl_hostel1.SelectedItem.Value = Convert.ToString(hostelcode);
                                btn_newadd.Text = "Update";
                            }
                            popwindow3.Visible = true;
                            btn_newadd.Visible = true;
                            div3.Visible = true;
                        }

                        bind_popstore();
                        #endregion
                    }
                }
                if (Radio_ReqTrans.Checked)
                {

                    if (ddl_Staff.Items.Count > 0)
                    {
                        string appno = Convert.ToString(ddl_Staff.SelectedItem.Value);




                        ddl_hostel1.Visible = false;
                        lbl_hostel1.Text = "Department";

                        lbl_fromhostel.Text = "";
                        bhosname.Text = "";

                        string activerow = "";
                        string activecol = "";
                        activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
                        activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
                        collegecode = Session["collegecode"].ToString();
                        Session["activerow"] = Convert.ToString(activerow);
                        Session["activecoloumn"] = Convert.ToString(activecol);
                        DataSet dsapp = new DataSet();
                        if (activecol.Trim() != "0" && activecol.Trim() != "1")
                        {

                            string itempk = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Tag);
                            string itemunit = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag);
                            string dept = " select distinct DeptFK  from RQ_Requisition r,RQ_RequisitionDet d,IM_ItemMaster i where RequestType=1 and RequisitionFK=RequisitionPK and d.ReqAppStatus='1' and r.ReqAppNo='" + appno + "' and ItemFK='" + itempk + "'";
                            dsapp.Clear();
                            dsapp = d2.select_method_wo_parameter(dept, "Text");
                            if (dsapp.Tables.Count > 0 && dsapp.Tables[0].Rows.Count > 0)
                            {

                                string dep1 = "";
                                for (int y = 0; y < dsapp.Tables[0].Rows.Count; y++)
                                {
                                    if (dep1 == "")
                                        dep1 = Convert.ToString(dsapp.Tables[0].Rows[y]["DeptFK"]);
                                    else
                                        dep1 = dep1 + "','" + Convert.ToString(dsapp.Tables[0].Rows[y]["DeptFK"]);
                                }
                                if (dep1 != "")
                                {
                                    string deptquery = "select distinct dept_code,dept_name from hrdept_master where college_code='" + collegecode1 + "' and  dept_code in('" + dep1 + "') order by dept_code";
                                    dsapp.Clear();
                                    dsapp = d2.select_method_wo_parameter(deptquery, "Text");
                                    if (dsapp.Tables.Count > 0 && dsapp.Tables[0].Rows.Count > 0)
                                    {
                                        ddl_transdept.DataSource = dsapp;
                                        ddl_transdept.DataTextField = "dept_name";
                                        ddl_transdept.DataValueField = "dept_code";
                                        ddl_transdept.DataBind();

                                    }
                                }
                                if (itemunit.Trim() != "")
                                {
                                    txt_itemmeasure.Text = itemunit;
                                    string itemname = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
                                    txt_itemname.Text = Convert.ToString(itemname);
                                    string totalvalue = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text);
                                    txt_totalQunatity.Text = Convert.ToString(totalvalue);
                                    txt_transferdate.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                                    string transfer = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 8].Text);
                                    string hostel = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Text);
                                    string hostelcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Tag);
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
                                        ddl_transdept.SelectedItem.Text = Convert.ToString(hostel);
                                        ddl_transdept.SelectedItem.Value = Convert.ToString(hostelcode);
                                        btn_newadd.Text = "Update";
                                    }
                                    ddl_transdept.Visible = true;
                                    popwindow3.Visible = true;
                                    btn_newadd.Visible = true;
                                    div3.Visible = true;
                                }
                            }

                        }
                    }

                }
            }
        }
        catch
        {
        }
    }

    protected void btn_newadd_Click(object sender, EventArgs e)
    {
        try
        {

            if (txt_transferqty.Text.Trim() != "")
            {
                if (Radio_ManTrans.Checked)
                {

                    if (ddl_transtype.SelectedItem.Value == "0")
                    {
                        #region Store to Mess
                        if (txt_transferqty.Text.Trim() != "")
                        {
                            string quantity = Convert.ToString(txt_transferqty.Text);
                            string itemname = Convert.ToString(txt_itemname.Text);
                            if (FpSpread1.Sheets[0].RowCount > 0)
                            {
                                FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 5].Text = Convert.ToString(itemname);
                                FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 8].Text = Convert.ToString(quantity);
                                FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 9].Text = Convert.ToString(ddl_hostel1.SelectedItem.Text);
                                FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 9].Tag = Convert.ToString(ddl_hostel1.SelectedItem.Value);
                                popwindow3.Visible = false;
                            }
                        }
                        #endregion
                    }

                    if (ddl_transtype.SelectedItem.Value == "1")
                    {
                        #region Mess to mess
                        if (txt_transferqty.Text.Trim() != "")
                        {
                            string quantity = Convert.ToString(txt_transferqty.Text);
                            string itemname = Convert.ToString(txt_itemname.Text);
                            if (FpSpread1.Sheets[0].RowCount > 0)
                            {
                                FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 5].Text = Convert.ToString(itemname);
                                FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 8].Text = Convert.ToString(quantity);
                                FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 9].Text = Convert.ToString(ddl_hostel1.SelectedItem.Text);
                                FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 9].Tag = Convert.ToString(ddl_hostel1.SelectedItem.Value);
                                popwindow3.Visible = false;
                            }
                        }
                        #endregion
                    }
                    if (ddl_transtype.SelectedItem.Value == "2")
                    {
                        #region Store to store
                        if (txt_transferqty.Text.Trim() != "")
                        {
                            string quantity = Convert.ToString(txt_transferqty.Text);
                            string itemname = Convert.ToString(txt_itemname.Text);
                            if (FpSpread1.Sheets[0].RowCount > 0)
                            {
                                FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 5].Text = Convert.ToString(itemname);
                                FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 8].Text = Convert.ToString(quantity);
                                FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 9].Text = Convert.ToString(ddl_popstore.SelectedItem.Text);
                                FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 9].Tag = Convert.ToString(ddl_popstore.SelectedItem.Value);
                                popwindow3.Visible = false;
                            }
                        }
                        #endregion

                    }
                    if (ddl_transtype.SelectedItem.Value == "3")
                    {
                        #region Mess to department
                        if (txt_transferqty.Text.Trim() != "")
                        {
                            string quantity = Convert.ToString(txt_transferqty.Text);
                            string itemname = Convert.ToString(txt_itemname.Text);
                            if (FpSpread1.Sheets[0].RowCount > 0)
                            {
                                FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 5].Text = Convert.ToString(itemname);
                                FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 8].Text = Convert.ToString(quantity);
                                FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 9].Text = Convert.ToString(ddl_transdept.SelectedItem.Text);
                                FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 9].Tag = Convert.ToString(ddl_transdept.SelectedItem.Value);
                                popwindow3.Visible = false;
                            }
                        }
                        #endregion
                    }
                    if (ddl_transtype.SelectedItem.Value == "4")
                    {
                        #region department to department
                        if (txt_transferqty.Text.Trim() != "")
                        {
                            string quantity = Convert.ToString(txt_transferqty.Text);
                            string itemname = Convert.ToString(txt_itemname.Text);
                            if (FpSpread1.Sheets[0].RowCount > 0)
                            {
                                FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 5].Text = Convert.ToString(quantity);
                                FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 6].Text = Convert.ToString(ddl_transdept.SelectedItem.Text);
                                FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 6].Tag = Convert.ToString(ddl_transdept.SelectedItem.Value);
                                popwindow3.Visible = false;
                            }
                        }

                        #endregion
                    }
                    if (ddl_transtype.SelectedItem.Value == "5")
                    {
                        #region department to store
                        if (txt_transferqty.Text.Trim() != "")
                        {
                            string quantity = Convert.ToString(txt_transferqty.Text);

                            if (FpSpread1.Sheets[0].RowCount > 0)
                            {
                                FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 5].Text = Convert.ToString(quantity);
                                FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 6].Text = Convert.ToString(ddl_popstore.SelectedItem.Text);
                                FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 6].Tag = Convert.ToString(ddl_popstore.SelectedItem.Value);
                                popwindow3.Visible = false;
                            }
                        }

                        #endregion
                    }

                }
                if (Radio_ReqTrans.Checked)
                {

                    if (txt_transferqty.Text.Trim() != "")
                    {
                        string quantity = Convert.ToString(txt_transferqty.Text);
                        string itemname = Convert.ToString(txt_itemname.Text);
                        if (FpSpread1.Sheets[0].RowCount > 0)
                        {
                            FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 5].Text = Convert.ToString(itemname);
                            FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 8].Text = Convert.ToString(quantity);
                            FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 9].Text = Convert.ToString(ddl_transdept.SelectedItem.Text);
                            FpSpread1.Sheets[0].Cells[Convert.ToInt32(Session["activerow"]), 9].Tag = Convert.ToString(ddl_transdept.SelectedItem.Value);
                            popwindow3.Visible = false;
                        }
                    }

                }
            }

            else
            {
                lbl_alerterror.Visible = true;
                lbl_alerterror.Text = "Please enter the transfor Quantity";
                alertmessage.Visible = true;
            }
        }
        catch
        {

        }
    }

    protected void btn_Transfer_Click(object sender, EventArgs e)
    {
        try
        {
            bool saveflag = false;
            string dtaccessdate = DateTime.Now.ToString("MM/dd/yyyy");
            //DateTime transdate = new DateTime();
            //string[] split2 = dtaccessdate.Split('/');
            //transdate = Convert.ToDateTime(split2[1] + "/" + split2[0] + "/" + split2[2]);

            string dtaccesstime = DateTime.Now.ToLongTimeString();
            string itemcode = "";
            string transqty = "";
            string activerow = "";
            string activecol = "";
            string transamt = "";
            string transfrom = "";
            string transto = "";
            double rpu = 0;
            string rpu1 = "";
            string transferdate = txt_transferdate.Text;
            string[] split = transferdate.Split('/');
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
            string inwardtype = "";

            if (Radio_ManTrans.Checked)
            {
                if (ddl_transtype.SelectedItem.Value == "0")
                {
                    #region Store to Mess

                    if (FpSpread1.Sheets[0].RowCount > 0)
                    {
                        for (int row = 0; row < FpSpread1.Sheets[0].RowCount; row++)
                        {
                            string storecode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 0].Tag);
                            if (storecode.Trim() == "")
                            {
                                itemcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 7].Tag);
                                transamt = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 8].Text);
                                rpu1 = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 7].Text);
                                string store_code = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 6].Tag);
                                string handqty = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 6].Text);
                                transto = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 9].Tag);
                                inwardtype = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 3].Tag);
                                string inwardcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 2].Tag);
                                string purchaseorder = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 1].Tag);
                                rpu = Convert.ToDouble(rpu1);
                                if (transamt.Trim() != "" && itemcode.Trim() != "")
                                {
                                    ds.Clear();
                                    string q = "";
                                    if (purchaseorder.Trim() != "" && inwardcode.Trim() != "" && inwardtype.Trim() != "")
                                    {
                                        q = "select TransferQty from IT_StockDetail where ItemFK ='" + itemcode + "' and StoreFK ='" + store_code + "' and InwardType='" + inwardtype + "' and InwardFK='" + inwardcode + "' and  OrderFK='" + purchaseorder + "'";
                                    }
                                    else if (inwardcode.Trim() != "" && inwardtype.Trim() != "")
                                    {
                                        q = "select TransferQty from IT_StockDetail where ItemFK ='" + itemcode + "' and StoreFK ='" + store_code + "' and InwardType='" + inwardtype + "' and InwardFK='" + inwardcode + "' ";
                                    }
                                    else if (inwardtype.Trim() != "")
                                    {
                                        q = "select TransferQty from IT_StockDetail where ItemFK ='" + itemcode + "' and StoreFK ='" + store_code + "' and InwardType='" + inwardtype + "' ";
                                    }
                                    else
                                    {
                                        q = "select TransferQty from IT_StockDetail where ItemFK ='" + itemcode + "' and StoreFK ='" + store_code + "' ";
                                    }
                                    ds = d2.select_method_wo_parameter(q, "Text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        transqty = Convert.ToString(ds.Tables[0].Rows[0][0]);
                                        if (transqty.Trim() != "")
                                        {
                                            double inval = Convert.ToDouble(transqty);
                                            inval = inval + Convert.ToDouble(transamt);
                                            transqty = Convert.ToString(inval);
                                        }
                                        else
                                        {
                                            transqty = transamt;
                                        }
                                    }
                                    else
                                    {
                                        transqty = transamt;
                                    }

                                    double deductqty = 0.00;
                                    if (handqty.Trim() != "" || handqty.Trim() != "0.00")
                                    {
                                        deductqty = Convert.ToDouble(handqty) - Convert.ToDouble(transamt);
                                    }
                                    else
                                    {
                                        lbl_alerterror.Visible = true;
                                        lbl_alerterror.Text = "You can't transfer!";
                                        return;
                                    }
                                    if (transqty.Trim() == "")
                                    {
                                        transqty = "0";
                                    }
                                    string q1 = "";
                                    //string q1 = "if exists (select * from IT_StockDetail where ItemFK ='" + itemcode + "' and StoreFK ='" + store_code + "' and InwardType='3') update IT_StockDetail set TransferQty ='" + transqty + "',BalQty='" + deductqty + "' where ItemFK ='" + itemcode + "' and StoreFK ='" + store_code + "' and InwardType='3' else Insert into IT_StockDetail (ItemFK,InwardQty,TransferQty,BalQty,InwardRPU,StoreFK,InwardType) values ('" + itemcode + "','" + deductqty + "','" + transqty + "','" + deductqty + "','" + rpu + "','" + store_code + "','3')";ISNULL(TransferQty,'0')+,inwardqty='" + deductqty + "'
                                    if (purchaseorder.Trim() != "" && inwardcode.Trim() != "" && inwardtype.Trim() != "")
                                    {
                                        q1 = "if exists (select * from IT_StockDetail where ItemFK ='" + itemcode + "' and StoreFK ='" + store_code + "' and InwardType='" + inwardtype + "' and OrderFK='" + purchaseorder + "' and InwardFK='" + inwardcode + "') update IT_StockDetail set TransferQty ='" + transqty + "',BalQty=InwardQty -isnull('" + transqty + "',0)  where ItemFK ='" + itemcode + "' and StoreFK ='" + store_code + "' and InwardType='" + inwardtype + "' and OrderFK='" + purchaseorder + "' and InwardFK='" + inwardcode + "'";
                                    }
                                    else if (inwardcode.Trim() != "" && inwardtype.Trim() != "")
                                    {
                                        q1 = "if exists (select * from IT_StockDetail where ItemFK ='" + itemcode + "' and StoreFK ='" + store_code + "' and InwardType='" + inwardtype + "' and InwardFK='" + inwardcode + "') update IT_StockDetail set TransferQty ='" + transqty + "',BalQty=InwardQty -isnull('" + transqty + "',0)  where ItemFK ='" + itemcode + "' and StoreFK ='" + store_code + "' and InwardType='" + inwardtype + "' and InwardFK='" + inwardcode + "'";//,inwardqty='" + deductqty + "'
                                    }
                                    else if (inwardtype.Trim() != "")
                                    {
                                        q1 = "if exists (select * from IT_StockDetail where ItemFK ='" + itemcode + "' and StoreFK ='" + store_code + "' and InwardType='" + inwardtype + "') update IT_StockDetail set TransferQty ='" + transqty + "',BalQty=InwardQty -isnull('" + transqty + "',0)  where ItemFK ='" + itemcode + "' and StoreFK ='" + store_code + "' and InwardType='" + inwardtype + "'";//,inwardqty='" + deductqty + "'
                                    }
                                    else
                                    {
                                        q1 = "if exists (select * from IT_StockDetail where ItemFK ='" + itemcode + "' and StoreFK ='" + store_code + "' and ISNULL(InwardType,0)=0) update IT_StockDetail set TransferQty ='" + transqty + "',BalQty=InwardQty -isnull('" + transqty + "',0)  where ItemFK ='" + itemcode + "' and StoreFK ='" + store_code + "' and ISNULL(InwardType,0)=0";//,inwardqty='" + deductqty + "'
                                    }
                                    ds.Clear();
                                    int i = d2.update_method_wo_parameter(q1, "Text");
                                    if (i != 0)
                                    {
                                        string q2 = "insert into IT_TransferItem (TrasnferDate,TransferQty,TransferType,TransferFrom,TrasferTo,ItemFK,TransferRpu) values ('" + dtaccessdate + "','" + transamt + "','1','" + store_code + "','" + transto + "','" + itemcode + "','" + rpu + "')";
                                        ds.Clear();
                                        int j = d2.update_method_wo_parameter(q2, "Text");
                                        if (j != 0)
                                        {
                                            string q4 = "select BalQty from IT_StockDeptDetail where ItemFK ='" + itemcode + "' and DeptFK ='" + ddl_hostel1.SelectedItem.Value + "'";
                                            ds.Clear();
                                            ds = d2.select_method_wo_parameter(q4, "Text");
                                            string avl1 = "";
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                string avl = Convert.ToString(ds.Tables[0].Rows[0][0]);
                                                if (avl.Trim() != "")
                                                {
                                                    double inval1 = Convert.ToDouble(avl);
                                                    inval1 = inval1 + Convert.ToDouble(transamt);
                                                    avl1 = Convert.ToString(inval1);
                                                }
                                                else
                                                {
                                                    avl1 = transamt;
                                                }
                                            }
                                            else
                                            {
                                                avl1 = transamt;
                                            }

                                            string avgrupstore = d2.GetFunction(" select AVG(InwardRPU) from IT_StockDetail where ItemFK in('" + itemcode + "') and StoreFK in('" + store_code + "')");
                                            string q3 = "";

                                            q3 = "if exists (select * from IT_StockDeptDetail where  ItemFK ='" + itemcode + "' and DeptFK ='" + ddl_hostel1.SelectedItem.Value + "' ) update IT_StockDeptDetail set BalQty ='" + avl1 + "',IssuedQty='" + avl1 + "',IssuedRPU='" + avgrupstore + "' where  ItemFK ='" + itemcode + "' and DeptFK ='" + ddl_hostel1.SelectedItem.Value + "' else insert into IT_StockDeptDetail (ItemFK,DeptFK,BalQty,IssuedRPU,IssuedQty) values ('" + itemcode + "','" + ddl_hostel1.SelectedItem.Value + "','" + avl1 + "','" + avgrupstore + "','" + avl1 + "')";
                                            ds.Clear();
                                            int k = d2.update_method_wo_parameter(q3, "Text");
                                            if (k != 0)
                                            {
                                                saveflag = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (saveflag == true)
                    {
                        btn_transfergo_Click(sender, e);
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Saved Successfully";
                        alertmessage.Visible = true;
                        FpSpread1.SaveChanges();
                    }
                    else
                    {
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Please enter the transfor quantity";
                        alertmessage.Visible = true;
                    }
                    #endregion
                }
                if (ddl_transtype.SelectedItem.Value == "1")
                {
                    #region Mess to Mess

                    if (FpSpread1.Sheets[0].RowCount > 0)
                    {
                        for (int row = 0; row < FpSpread1.Sheets[0].RowCount; row++)
                        {
                            //string inwardcode = "";
                            //string purchaseorder = "";
                            string hostelqunty = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 6].Text);
                            itemcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 6].Tag);
                            transamt = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 8].Text);
                            string deptfk = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 7].Tag);
                            //inwardtype = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 3].Tag);
                            string hostelcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 9].Tag);
                            //inwardcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 2].Tag);
                            //purchaseorder = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 1].Tag);


                            rpu1 = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 7].Text);
                            rpu = Convert.ToDouble(rpu1);

                            if (transamt.Trim() != "")
                            {
                                double deductqunty = 0.00;
                                if (hostelqunty.Trim() != "" || hostelqunty.Trim() != "0.00")
                                {
                                    deductqunty = Convert.ToDouble(hostelqunty) - Convert.ToDouble(transamt);
                                }
                                else
                                {
                                    lbl_alerterror.Visible = true;
                                    lbl_alerterror.Text = "You can't Transfer!";
                                    return;
                                }

                                #region 04.04.16
                                // string q1 = "";
                                //if (purchaseorder.Trim() != "" && inwardcode.Trim() != "")
                                //{
                                //    q1 = "if exists (select * from IT_StockDeptDetail where  ItemFK ='" + itemcode + "' and DeptFK ='" + deptfk + "' and Inward_Type='" + inwardtype + "' and InwardFK='" + inwardcode + "' and OrderFK='" + purchaseorder + "') update IT_StockDeptDetail set BalQty ='" + deductqunty + "',IssuedRPU='" + rpu + "' where  ItemFK ='" + itemcode + "' and DeptFK ='" + deptfk + "' and Inward_Type='" + inwardtype + "'and InwardFK='" + inwardcode + "' and OrderFK='" + purchaseorder + "'";
                                //}
                                //else if (inwardcode.Trim() != "")
                                //{
                                //    q1 = "if exists (select * from IT_StockDeptDetail where  ItemFK ='" + itemcode + "' and DeptFK ='" + deptfk + "' and Inward_Type='" + inwardtype + "' and InwardFK='" + inwardcode + "' ) update IT_StockDeptDetail set BalQty ='" + deductqunty + "',IssuedRPU='" + rpu + "' where  ItemFK ='" + itemcode + "' and DeptFK ='" + deptfk + "' and Inward_Type='" + inwardtype + "'and InwardFK='" + inwardcode + "' ";
                                //}
                                //else if (inwardtype.Trim() != "")
                                //{
                                //    q1 = "if exists (select * from IT_StockDeptDetail where  ItemFK ='" + itemcode + "' and DeptFK ='" + deptfk + "' and Inward_Type='" + inwardtype + "') update IT_StockDeptDetail set BalQty ='" + deductqunty + "',IssuedRPU='" + rpu + "' where  ItemFK ='" + itemcode + "' and DeptFK ='" + deptfk + "' and Inward_Type='" + inwardtype + "'";
                                //}
                                //else
                                //{
                                //    q1 = "if exists (select * from IT_StockDeptDetail where  ItemFK ='" + itemcode + "' and DeptFK ='" + deptfk + "' and ISNULL(Inward_Type,0)=0 ) update IT_StockDeptDetail set BalQty ='" + deductqunty + "',IssuedRPU='" + rpu + "' where  ItemFK ='" + itemcode + "' and DeptFK ='" + deptfk + "' and ISNULL(Inward_Type,0)=0";
                                //}
                                #endregion

                                string q1 = "if exists (select * from IT_StockDeptDetail where  ItemFK ='" + itemcode + "' and DeptFK ='" + deptfk + "' ) update IT_StockDeptDetail set BalQty ='" + deductqunty + "',IssuedRPU='" + rpu + "' where  ItemFK ='" + itemcode + "' and DeptFK ='" + deptfk + "' ";
                                ds.Clear();
                                int i = d2.update_method_wo_parameter(q1, "Text");
                                if (i != 0)
                                {
                                    string q2 = "insert into IT_TransferItem (TrasnferDate,TransferQty,TransferType,TransferFrom,TrasferTo,ItemFK,TransferRpu) values ('" + dtaccessdate + "','" + transamt + "','2','" + ddl_hostelname3.SelectedItem.Value + "','" + hostelcode + "','" + itemcode + "','" + rpu + "')";
                                    ds.Clear();
                                    int j = d2.update_method_wo_parameter(q2, "Text");
                                    if (j != 0)
                                    {
                                        string q4 = "select BalQty from IT_StockDeptDetail where ItemFK ='" + itemcode + "' and DeptFK ='" + ddl_hostel1.SelectedItem.Value + "'";// and Inward_Type='3'
                                        ds.Clear();
                                        ds = d2.select_method_wo_parameter(q4, "Text");
                                        string avl1 = "";

                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            string avl = Convert.ToString(ds.Tables[0].Rows[0][0]);
                                            if (avl.Trim() != "")
                                            {
                                                double inval1 = Convert.ToDouble(avl);
                                                inval1 = inval1 + Convert.ToDouble(transamt);
                                                avl1 = Convert.ToString(inval1);
                                            }
                                            else
                                            {
                                                avl1 = transamt;
                                            }
                                        }
                                        else
                                        {
                                            avl1 = transamt;
                                        }
                                        string q3 = "if exists (select * from IT_StockDeptDetail where  ItemFK ='" + itemcode + "' and DeptFK ='" + ddl_hostel1.SelectedItem.Value + "')update IT_StockDeptDetail set BalQty ='" + avl1 + "',IssuedRPU='" + rpu + "',IssuedQty=IssuedQty+'" + avl1 + "' where  ItemFK ='" + itemcode + "' and DeptFK ='" + ddl_hostel1.SelectedItem.Value + "' else insert into IT_StockDeptDetail (ItemFK,DeptFK,BalQty,IssuedRPU,IssuedQty) values ('" + itemcode + "','" + ddl_hostel1.SelectedItem.Value + "','" + avl1 + "','" + rpu + "','" + avl1 + "')";//" + inwardtype + "  and Inward_Type='3'
                                        ds.Clear();
                                        int k = d2.update_method_wo_parameter(q3, "Text");
                                        if (k != 0)
                                        {
                                            saveflag = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (saveflag == true)
                    {
                        btn_transfergo_Click(sender, e);
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Saved Successfully";
                        alertmessage.Visible = true;
                        FpSpread1.SaveChanges();
                    }
                    else
                    {
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Please enter the transfor quantity";
                        alertmessage.Visible = true;
                    }
                    #endregion
                }
                if (ddl_transtype.SelectedItem.Value == "2")
                {
                    #region Store to store

                    if (FpSpread1.Sheets[0].RowCount > 0)
                    {
                        for (int row = 0; row < FpSpread1.Sheets[0].RowCount; row++)
                        {
                            //itemcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 3].Tag);
                            //transamt = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 4].Text);
                            //string handonqty = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 3].Text);
                            //string storefrom = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 4].Tag);
                            //string storeto = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 5].Tag);
                            //string itemmesure = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 1].Note);
                            //inwardtype = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 3].Tag);
                            //rpu1 = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 2].Tag);

                            itemcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 6].Tag);
                            transamt = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 8].Text);
                            string handonqty = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 6].Text);
                            string storefrom = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 7].Tag);
                            string storeto = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 9].Tag);
                            string itemmesure = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 4].Tag);
                            inwardtype = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 3].Tag);
                            rpu1 = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 7].Text);
                            string inwardcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 2].Tag);
                            string purchaseorder = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 1].Tag);
                            rpu = Convert.ToDouble(rpu1);
                            double stockvalue = 0;

                            if (transamt.Trim() != "")
                            {
                                ds.Clear();

                                //string q = "select TransferQty from IT_StockDetail where ItemFK ='" + itemcode + "' and StoreFK ='" + storefrom + "' and InwardType='" + inwardtype + "'";
                                string q = "";
                                if (purchaseorder.Trim() != "" && inwardcode.Trim() != "" && inwardtype.Trim() != "")
                                {
                                    q = "select TransferQty from IT_StockDetail where ItemFK ='" + itemcode + "' and StoreFK ='" + storefrom + "' and InwardType='" + inwardtype + "' and InwardFK='" + inwardcode + "' and  OrderFK='" + purchaseorder + "'";
                                }
                                else if (inwardcode.Trim() != "" && inwardtype.Trim() != "")
                                {
                                    q = "select TransferQty from IT_StockDetail where ItemFK ='" + itemcode + "' and StoreFK ='" + storefrom + "' and InwardType='" + inwardtype + "' and InwardFK='" + inwardcode + "' ";
                                }
                                else if (inwardtype.Trim() != "")
                                {
                                    q = "select TransferQty from IT_StockDetail where ItemFK ='" + itemcode + "' and StoreFK ='" + storefrom + "' and InwardType='" + inwardtype + "' ";
                                }
                                else
                                {
                                    q = "select TransferQty from IT_StockDetail where ItemFK ='" + itemcode + "' and StoreFK ='" + storefrom + "' ";
                                }
                                ds = d2.select_method_wo_parameter(q, "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    transqty = Convert.ToString(ds.Tables[0].Rows[0][0]);
                                    if (transqty.Trim() != "")
                                    {
                                        double inval = Convert.ToDouble(transqty);
                                        inval = inval + Convert.ToDouble(transamt);
                                        transqty = Convert.ToString(inval);
                                    }
                                    else
                                    {
                                        transqty = transamt;
                                    }
                                }
                                else
                                {
                                    transqty = transamt;
                                }

                                double deductqty = 0.00;
                                if (handonqty.Trim() != "")
                                {
                                    deductqty = Convert.ToDouble(handonqty) - Convert.ToDouble(transamt);
                                }
                                else
                                {
                                    lbl_alerterror.Visible = true;
                                    lbl_alerterror.Text = "You can't Transfer!";
                                    return;
                                }
                                //double deductqunty = Convert.ToDouble(handonqty) - Convert.ToDouble(transamt);
                                //string q1 = "if exists (select * from IT_StockDetail where  ItemFK ='" + itemcode + "' and StoreFK ='" + storefrom + "' and InwardType='3') update IT_StockDetail set BalQty ='" + deductqty + "',TransferQty='" + transqty + "' where  ItemFK ='" + itemcode + "' and StoreFK ='" + storefrom + "' else insert into IT_StockDetail (ItemFK,InwardQty,TransferQty,BalQty,InwardRPU,StoreFK,InwardType) values ('" + itemcode + "','" + deductqty + "','" + transqty + "','" + deductqty + "','" + rpu + "','" + storefrom + "','3')";

                                string q1 = "";

                                if (purchaseorder.Trim() != "" && inwardcode.Trim() != "" && inwardtype.Trim() != "")
                                {
                                    q1 = "if exists (select * from IT_StockDetail where ItemFK ='" + itemcode + "' and StoreFK ='" + storefrom + "' and InwardType='" + inwardtype + "' and OrderFK='" + purchaseorder + "' and InwardFK='" + inwardcode + "') update IT_StockDetail set TransferQty ='" + transqty + "',BalQty='" + deductqty + "'  where ItemFK ='" + itemcode + "' and StoreFK ='" + storefrom + "' and InwardType='" + inwardtype + "' and OrderFK='" + purchaseorder + "' and InwardFK='" + inwardcode + "'";
                                }
                                else if (inwardcode.Trim() != "" && inwardtype.Trim() != "")
                                {
                                    q1 = "if exists (select * from IT_StockDetail where ItemFK ='" + itemcode + "' and StoreFK ='" + storefrom + "' and InwardType='" + inwardtype + "' and InwardFK='" + inwardcode + "') update IT_StockDetail set TransferQty ='" + transqty + "',BalQty='" + deductqty + "'  where ItemFK ='" + itemcode + "' and StoreFK ='" + storefrom + "' and InwardType='" + inwardtype + "' and InwardFK='" + inwardcode + "'";//,inwardqty='" + deductqty + "'
                                }
                                else if (inwardtype.Trim() != "")
                                {
                                    q1 = "if exists (select * from IT_StockDetail where ItemFK ='" + itemcode + "' and StoreFK ='" + storefrom + "' and InwardType='" + inwardtype + "') update IT_StockDetail set TransferQty ='" + transqty + "',BalQty='" + deductqty + "'  where ItemFK ='" + itemcode + "' and StoreFK ='" + storefrom + "' and InwardType='" + inwardtype + "'";//,inwardqty='" + deductqty + "'
                                }
                                else
                                {
                                    q1 = "if exists (select * from IT_StockDetail where ItemFK ='" + itemcode + "' and StoreFK ='" + storefrom + "' and ISNULL(InwardType,0)=0) update IT_StockDetail set TransferQty ='" + transqty + "',BalQty='" + deductqty + "'  where ItemFK ='" + itemcode + "' and StoreFK ='" + storefrom + "' and ISNULL(InwardType,0)=0";//,inwardqty='" + deductqty + "'
                                }


                                //  string q1 = "if exists (select * from IT_StockDetail where  ItemFK ='" + itemcode + "' and StoreFK ='" + storefrom + "' and InwardType='" + inwardtype + "') update IT_StockDetail set BalQty ='" + deductqty + "',TransferQty='" + transqty + "' where  ItemFK ='" + itemcode + "' and StoreFK ='" + storefrom + "' and InwardType='" + inwardtype + "'";

                                ds.Clear();
                                int i = d2.update_method_wo_parameter(q1, "Text");
                                if (i != 0)
                                {
                                    string q2 = "insert into IT_TransferItem (TrasnferDate,TransferQty,TransferType,TransferFrom,TrasferTo,ItemFK,TransferRpu) values ('" + dtaccessdate + "','" + transamt + "','3','" + storefrom + "','" + storeto + "','" + itemcode + "','" + rpu + "')";

                                    ds.Clear();
                                    int j = d2.update_method_wo_parameter(q2, "Text");
                                    if (j != 0)
                                    {
                                        string q4 = "select BalQty from IT_StockDetail where ItemFK ='" + itemcode + "' and StoreFK ='" + storeto + "' and InwardType='3'";
                                        ds.Clear();
                                        ds = d2.select_method_wo_parameter(q4, "Text");
                                        string avl1 = "";

                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            string avl = Convert.ToString(ds.Tables[0].Rows[0][0]);
                                            if (avl.Trim() != "")
                                            {
                                                double inval1 = Convert.ToDouble(avl);
                                                inval1 = inval1 + Convert.ToDouble(transamt);
                                                avl1 = Convert.ToString(inval1);
                                            }
                                            else
                                            {
                                                avl1 = transamt;
                                            }
                                        }
                                        else
                                        {
                                            avl1 = transamt;
                                        }
                                        string q3 = "if exists (select * from IT_StockDetail where  ItemFK ='" + itemcode + "' and StoreFK ='" + storeto + "' and InwardType='3')update IT_StockDetail set BalQty ='" + avl1 + "',InwardRPU='" + rpu + "',InwardQty='" + avl1 + "' where  ItemFK ='" + itemcode + "' and StoreFK ='" + storeto + "' and InwardType='3' else insert into IT_StockDetail (ItemFK,StoreFK,BalQty,InwardRPU,InwardQty,InwardType) values ('" + itemcode + "','" + storeto + "','" + avl1 + "','" + rpu + "','" + avl1 + "','3')";//" + inwardtype + "
                                        ds.Clear();
                                        int k = d2.update_method_wo_parameter(q3, "Text");
                                        if (k != 0)
                                        {
                                            saveflag = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (saveflag == true)
                    {
                        btn_transfergo_Click(sender, e);
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Saved Successfully";
                        alertmessage.Visible = true;
                        FpSpread1.SaveChanges();
                    }
                    else
                    {
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Please enter the transfer quantity";
                        alertmessage.Visible = true;
                    }
                    #endregion
                }
                if (ddl_transtype.SelectedItem.Value == "3")
                {
                    #region store to Department

                    if (FpSpread1.Sheets[0].RowCount > 0)
                    {
                        for (int row = 0; row < FpSpread1.Sheets[0].RowCount; row++)
                        {
                            string storecode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 0].Tag);
                            if (storecode.Trim() == "")
                            {
                                //string store = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 3].Tag);
                                //string handonqty = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 3].Text);
                                //transamt = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 4].Text);
                                //string itemfk = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 4].Tag);
                                //string transdept = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 5].Tag);
                                //string itemmesure = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 1].Tag);
                                //rpu1 = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 2].Tag);

                                string store = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 6].Tag);
                                string handonqty = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 6].Text);
                                transamt = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 8].Text);
                                string itemfk = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 7].Tag);
                                string transdept = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 9].Tag);
                                string itemmesure = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 4].Tag);
                                rpu1 = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 7].Text);
                                inwardtype = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 3].Tag);

                                string inwardcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 2].Tag);
                                string purchaseorder = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 1].Tag);

                                rpu = Convert.ToDouble(rpu1);
                                double stockvalue = 0;

                                if (transamt.Trim() != "")
                                {
                                    double deductqunty = 0.00;
                                    if (handonqty.Trim() != "" || handonqty.Trim() != "0.00")
                                    {
                                        deductqunty = Convert.ToDouble(handonqty) - Convert.ToDouble(transamt);
                                    }
                                    else
                                    {
                                        lbl_alerterror.Visible = true;
                                        lbl_alerterror.Text = "You can't Transfer!";
                                        return;
                                    }
                                    // string q1 = "if exists (select * from IT_StockDetail where  ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and inwardtype='" + inwardtype + "') update IT_StockDetail set BalQty ='" + deductqunty + "',TransferQty=ISNULL(TransferQty,0)+('" + transamt + "'),InwardRPU ='" + rpu + "' where  ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and inwardtype='" + inwardtype + "'";
                                    string q1 = "";
                                    string wherequery = "";
                                    if (purchaseorder.Trim() != "" && inwardcode.Trim() != "" && inwardtype.Trim() != "")
                                    {
                                        q1 = "if exists (select * from IT_StockDetail where ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and InwardType='" + inwardtype + "' and OrderFK='" + purchaseorder + "' and InwardFK='" + inwardcode + "') update IT_StockDetail set TransferQty=ISNULL(TransferQty,0)+('" + transamt + "') where ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and InwardType='" + inwardtype + "' and OrderFK='" + purchaseorder + "' and InwardFK='" + inwardcode + "'";//,BalQty=InwardQty -isnull('" + transamt + "',0) 
                                        wherequery = " ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and InwardType='" + inwardtype + "' and OrderFK='" + purchaseorder + "' and InwardFK='" + inwardcode + "'";
                                    }
                                    else if (inwardcode.Trim() != "" && inwardtype.Trim() != "")
                                    {
                                        q1 = "if exists (select * from IT_StockDetail where ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and InwardType='" + inwardtype + "' and InwardFK='" + inwardcode + "') update IT_StockDetail set TransferQty=ISNULL(TransferQty,0)+('" + transamt + "') where ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and InwardType='" + inwardtype + "' and InwardFK='" + inwardcode + "'";//,BalQty=InwardQty -isnull('" + transamt + "',0) 
                                        wherequery = " ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and InwardType='" + inwardtype + "' and InwardFK='" + inwardcode + "'";
                                    }
                                    else if (inwardtype.Trim() != "")
                                    {
                                        q1 = "if exists (select * from IT_StockDetail where ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and InwardType='" + inwardtype + "') update IT_StockDetail set TransferQty=ISNULL(TransferQty,0)+('" + transamt + "')  where ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and InwardType='" + inwardtype + "'";//,inwardqty='" + deductqty + "' ,BalQty=InwardQty -isnull('" + transamt + "',0)
                                        wherequery = "ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and InwardType='" + inwardtype + "'";
                                    }
                                    else
                                    {
                                        q1 = "if exists (select * from IT_StockDetail where ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and ISNULL(InwardType,0)=0) update IT_StockDetail set TransferQty=ISNULL(TransferQty,0)+('" + transamt + "') where ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and ISNULL(InwardType,0)=0";//,BalQty=InwardQty -isnull('" + transamt + "',0) 
                                        wherequery = "ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and ISNULL(InwardType,0)=0";
                                    }
                                    ds.Clear();
                                    int i = d2.update_method_wo_parameter(q1, "Text");
                                    string balup = "update IT_StockDetail set BalQty=InwardQty -isnull(TransferQty,0) where " + wherequery + "";
                                    int st = d2.update_method_wo_parameter(balup, "Text");

                                    //string deptmasterins = "if exists(select *from IM_ItemDeptMaster where ItemDeptFK='" + transdept + "' and ItemFK='" + itemfk + "') update IM_ItemDeptMaster set ItemDeptFK='" + transdept + "',ItemFK='" + itemfk + "' where ItemDeptFK='" + transdept + "' and ItemFK='" + itemfk + "' else insert into IM_ItemDeptMaster values('" + transdept + "','" + itemfk + "')";
                                    //int o = d2.update_method_wo_parameter(deptmasterins, "Text");

                                    if (i != 0)
                                    {
                                        string q2 = "insert into IT_TransferItem (TrasnferDate,TransferQty,TransferType,TransferFrom,TrasferTo,ItemFK,TransferRpu) values ('" + dtaccessdate + "','" + transamt + "','4','" + store + "','" + transdept + "','" + itemfk + "','" + rpu + "')";
                                        ds.Clear();
                                        int j = d2.update_method_wo_parameter(q2, "Text");
                                        if (j != 0)
                                        {
                                            string q4 = "select BalQty from IT_StockDeptDetail where ItemFK ='" + itemfk + "' and DeptFK ='" + transdept + "' ";//and Inward_Type='3'
                                            ds.Clear();
                                            ds = d2.select_method_wo_parameter(q4, "Text");
                                            string avl1 = "";

                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                string avl = Convert.ToString(ds.Tables[0].Rows[0][0]);
                                                if (avl.Trim() != "")
                                                {
                                                    double inval1 = Convert.ToDouble(avl);
                                                    inval1 = inval1 + Convert.ToDouble(transamt);
                                                    avl1 = Convert.ToString(inval1);
                                                }
                                                else
                                                {
                                                    avl1 = transamt;
                                                }
                                            }
                                            else
                                            {
                                                avl1 = transamt;
                                            }
                                            //string q3 = "if exists (select * from IT_StockDeptDetail where  ItemFK ='" + itemfk + "' and DeptFK ='" + transdept + "' and Inward_Type='3')update IT_StockDeptDetail set BalQty ='" + avl1 + "',IssuedRPU='" + rpu + "',IssuedQty=IssuedQty+'" + avl1 + "' where  ItemFK ='" + itemfk + "' and DeptFK ='" + transdept + "' and Inward_Type='3' else insert into IT_StockDeptDetail (ItemFK,DeptFK,BalQty,IssuedRPU,IssuedQty,Inward_Type) values ('" + itemfk + "','" + transdept + "','" + avl1 + "','" + rpu + "','" + avl1 + "','3')";//" + inwardtype + "

                                            string q3 = "if exists (select * from IT_StockDeptDetail where  ItemFK ='" + itemfk + "' and DeptFK ='" + transdept + "' )update IT_StockDeptDetail set BalQty ='" + avl1 + "',IssuedRPU='" + rpu + "',IssuedQty=IssuedQty+'" + avl1 + "' where  ItemFK ='" + itemfk + "' and DeptFK ='" + transdept + "' else insert into IT_StockDeptDetail (ItemFK,DeptFK,BalQty,IssuedRPU,IssuedQty) values ('" + itemfk + "','" + transdept + "','" + avl1 + "','" + rpu + "','" + avl1 + "')";
                                            ds.Clear();
                                            int k = d2.update_method_wo_parameter(q3, "Text");
                                            if (k != 0)
                                            {
                                                saveflag = true;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    lbl_alerterror.Visible = true;
                                    lbl_alerterror.Text = "Please enter the transfer quantity";
                                    alertmessage.Visible = true;
                                }
                                if (saveflag == true)
                                {
                                    btn_transfergo_Click(sender, e);
                                    lbl_alerterror.Visible = true;
                                    lbl_alerterror.Text = "Saved Successfully";
                                    alertmessage.Visible = true;
                                    FpSpread1.SaveChanges();
                                }
                            }
                        }
                    }
                    #endregion
                }
                if (ddl_transtype.SelectedItem.Value == "4")
                {
                    #region Departent to department
                    if (FpSpread1.Sheets[0].RowCount > 0)
                    {
                        for (int row = 0; row < FpSpread1.Sheets[0].RowCount; row++)
                        {
                            string deptfk = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 6].Tag);
                            if (deptfk.Trim() != "")
                            {
                                string itempk = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 3].Tag);
                                string rpu2 = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 2].Tag);
                                string handonqty = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 3].Text);
                                string deptfk1 = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 6].Tag);
                                transamt = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 5].Text);
                                if (transamt.Trim() != "")
                                {
                                    double deductqunty = 0.00;
                                    if (handonqty.Trim() != "" || handonqty.Trim() != "0.00")
                                    {
                                        deductqunty = Convert.ToDouble(handonqty) - Convert.ToDouble(transamt);
                                    }
                                    else
                                    {
                                        lbl_alerterror.Visible = true;
                                        lbl_alerterror.Text = "You can't Transfer!";
                                        return;
                                    }
                                    string q1 = "if exists (select * from IT_StockDeptDetail where  ItemFK ='" + itempk + "' and DeptFK ='" + Convert.ToString(ddl_acadamic.SelectedItem.Value) + "') update IT_StockDeptDetail set BalQty ='" + deductqunty + "',IssuedRPU='" + rpu2 + "' where  ItemFK ='" + itempk + "' and DeptFK ='" + Convert.ToString(ddl_acadamic.SelectedItem.Value) + "' ";
                                    ds.Clear();
                                    int i = d2.update_method_wo_parameter(q1, "Text");
                                    if (i != 0)
                                    {
                                        string q2 = "insert into IT_TransferItem (TrasnferDate,TransferQty,TransferType,TransferFrom,TrasferTo,ItemFK,TransferRpu) values ('" + dtaccessdate + "','" + transamt + "','5','" + ddl_acadamic.SelectedItem.Value + "','" + deptfk1 + "','" + itempk + "','" + rpu2 + "')";
                                        ds.Clear();
                                        int j = d2.update_method_wo_parameter(q2, "Text");
                                        if (j != 0)
                                        {
                                            string q4 = "select BalQty from IT_StockDeptDetail where ItemFK ='" + itempk + "' and DeptFK ='" + deptfk1 + "'";
                                            ds.Clear();
                                            ds = d2.select_method_wo_parameter(q4, "Text");
                                            string avl1 = "";

                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                string avl = Convert.ToString(ds.Tables[0].Rows[0][0]);
                                                if (avl.Trim() != "")
                                                {
                                                    double inval1 = Convert.ToDouble(avl);
                                                    inval1 = inval1 + Convert.ToDouble(transamt);
                                                    avl1 = Convert.ToString(inval1);
                                                }
                                                else
                                                {
                                                    avl1 = transamt;
                                                }
                                            }
                                            else
                                            {
                                                avl1 = transamt;
                                            }
                                            string q3 = "if exists (select * from IT_StockDeptDetail where  ItemFK ='" + itempk + "' and DeptFK ='" + deptfk1 + "')update IT_StockDeptDetail set BalQty ='" + avl1 + "',IssuedRPU='" + rpu2 + "',IssuedQty=IssuedQty+'" + avl1 + "' where  ItemFK ='" + itempk + "' and DeptFK ='" + deptfk1 + "' else insert into IT_StockDeptDetail (ItemFK,DeptFK,BalQty,IssuedRPU,IssuedQty) values ('" + itempk + "','" + deptfk1 + "','" + avl1 + "','" + rpu2 + "','" + avl1 + "')";
                                            ds.Clear();
                                            int k = d2.update_method_wo_parameter(q3, "Text");
                                            if (k != 0)
                                            {
                                                saveflag = true;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    lbl_alerterror.Visible = true;
                                    lbl_alerterror.Text = "Please enter the transfer quantity";
                                    alertmessage.Visible = true;
                                }
                                if (saveflag == true)
                                {
                                    btn_transfergo_Click(sender, e);
                                    lbl_alerterror.Visible = true;
                                    lbl_alerterror.Text = "Saved Successfully";
                                    alertmessage.Visible = true;
                                    FpSpread1.SaveChanges();
                                }
                            }
                        }
                    }
                    #endregion
                }
                if (ddl_transtype.SelectedItem.Value == "5")
                {
                    #region Department to Store
                    if (FpSpread1.Sheets[0].RowCount > 0)
                    {
                        for (int row = 0; row < FpSpread1.Sheets[0].RowCount; row++)
                        {
                            string storefk = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 6].Tag);
                            if (storefk.Trim() != "")
                            {
                                string itempk = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 3].Tag);
                                string rpu2 = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 2].Tag);
                                string handonqty = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 3].Text);
                                // string storefk = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 6].Tag);
                                transamt = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 5].Text);
                                if (transamt.Trim() != "")
                                {
                                    double deductqunty = 0.00;
                                    if (handonqty.Trim() != "" || handonqty.Trim() != "0.00")
                                    {
                                        deductqunty = Convert.ToDouble(handonqty) - Convert.ToDouble(transamt);
                                    }
                                    else
                                    {
                                        lbl_alerterror.Visible = true;
                                        lbl_alerterror.Text = "You can't Transfer!";
                                        return;
                                    }
                                    string q1 = "if exists (select * from IT_StockDeptDetail where  ItemFK ='" + itempk + "' and DeptFK ='" + Convert.ToString(ddl_acadamic.SelectedItem.Value) + "') update IT_StockDeptDetail set BalQty ='" + deductqunty + "',IssuedRPU='" + rpu2 + "' where  ItemFK ='" + itempk + "' and DeptFK ='" + Convert.ToString(ddl_acadamic.SelectedItem.Value) + "' ";
                                    ds.Clear();
                                    int i = d2.update_method_wo_parameter(q1, "Text");
                                    if (i != 0)
                                    {
                                        string q2 = "insert into IT_TransferItem (TrasnferDate,TransferQty,TransferType,TransferFrom,TrasferTo,ItemFK,TransferRpu) values ('" + dtaccessdate + "','" + transamt + "','6','" + ddl_acadamic.SelectedItem.Value + "','" + storefk + "','" + itempk + "','" + rpu2 + "')";
                                        ds.Clear();
                                        int j = d2.update_method_wo_parameter(q2, "Text");
                                        if (j != 0)
                                        {
                                            string q4 = "select BalQty from IT_StockDetail where ItemFK ='" + itempk + "' and StoreFK ='" + storefk + "'";
                                            ds.Clear();
                                            ds = d2.select_method_wo_parameter(q4, "Text");
                                            string avl1 = "";

                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                string avl = Convert.ToString(ds.Tables[0].Rows[0][0]);
                                                if (avl.Trim() != "")
                                                {
                                                    double inval1 = Convert.ToDouble(avl);
                                                    inval1 = inval1 + Convert.ToDouble(transamt);
                                                    avl1 = Convert.ToString(inval1);
                                                }
                                                else
                                                {
                                                    avl1 = transamt;
                                                }
                                            }
                                            else
                                            {
                                                avl1 = transamt;
                                            }
                                            string q3 = "if exists (select * from IT_StockDetail where  ItemFK ='" + itempk + "' and StoreFK ='" + storefk + "')update IT_StockDetail set BalQty ='" + avl1 + "',InwardRPU='" + rpu2 + "',InwardQty='" + avl1 + "' where  ItemFK ='" + itempk + "' and StoreFK ='" + storefk + "' else insert into IT_StockDetail (ItemFK,StoreFK,BalQty,InwardRPU,InwardQty) values ('" + itempk + "','" + storefk + "','" + avl1 + "','" + rpu2 + "','" + avl1 + "')";
                                            ds.Clear();
                                            int k = d2.update_method_wo_parameter(q3, "Text");
                                            if (k != 0)
                                            {
                                                saveflag = true;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    lbl_alerterror.Visible = true;
                                    lbl_alerterror.Text = "Please enter the transfer quantity";
                                    alertmessage.Visible = true;
                                }
                                if (saveflag == true)
                                {
                                    btn_transfergo_Click(sender, e);
                                    lbl_alerterror.Visible = true;
                                    lbl_alerterror.Text = "Saved Successfully";
                                    alertmessage.Visible = true;
                                    FpSpread1.SaveChanges();
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            if (Radio_ReqTrans.Checked)
            {
                if (ddl_transtype1.SelectedItem.Value == "0")
                {
                    //Store To Department
                    #region Store To Department
                    if (FpSpread1.Sheets[0].RowCount > 0)
                    {
                        for (int row = 0; row < FpSpread1.Sheets[0].RowCount; row++)
                        {
                            string storecode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 0].Tag);
                            if (storecode.Trim() == "")
                            {
                                string store = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 6].Tag);
                                string handonqty = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 6].Text);
                                transamt = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 8].Text);
                                string itemfk = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 7].Tag);
                                string transdept = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 9].Tag);
                                string itemmesure = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 4].Tag);
                                rpu1 = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 7].Text);
                                inwardtype = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 3].Tag);

                                string inwardcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 2].Tag);
                                string purchaseorder = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 1].Tag);

                                rpu = Convert.ToDouble(rpu1);
                                double stockvalue = 0;

                                if (transamt.Trim() != "")
                                {
                                    double deductqunty = 0.00;
                                    if (handonqty.Trim() != "" || handonqty.Trim() != "0.00")
                                    {
                                        deductqunty = Convert.ToDouble(handonqty) - Convert.ToDouble(transamt);
                                    }
                                    else
                                    {
                                        lbl_alerterror.Visible = true;
                                        lbl_alerterror.Text = "You can't Transfer!";
                                        return;
                                    }

                                    string q1 = "";
                                    string wherequery = "";
                                    if (purchaseorder.Trim() != "" && inwardcode.Trim() != "" && inwardtype.Trim() != "")
                                    {
                                        q1 = "if exists (select * from IT_StockDetail where ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and InwardType='" + inwardtype + "' and OrderFK='" + purchaseorder + "' and InwardFK='" + inwardcode + "') update IT_StockDetail set TransferQty=ISNULL(TransferQty,0)+('" + transamt + "') where ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and InwardType='" + inwardtype + "' and OrderFK='" + purchaseorder + "' and InwardFK='" + inwardcode + "'";//,BalQty=InwardQty -isnull('" + transamt + "',0) 
                                        wherequery = " ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and InwardType='" + inwardtype + "' and OrderFK='" + purchaseorder + "' and InwardFK='" + inwardcode + "'";
                                    }
                                    else if (inwardcode.Trim() != "" && inwardtype.Trim() != "")
                                    {
                                        q1 = "if exists (select * from IT_StockDetail where ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and InwardType='" + inwardtype + "' and InwardFK='" + inwardcode + "') update IT_StockDetail set TransferQty=ISNULL(TransferQty,0)+('" + transamt + "') where ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and InwardType='" + inwardtype + "' and InwardFK='" + inwardcode + "'";//,BalQty=InwardQty -isnull('" + transamt + "',0) 
                                        wherequery = " ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and InwardType='" + inwardtype + "' and InwardFK='" + inwardcode + "'";
                                    }
                                    else if (inwardtype.Trim() != "")
                                    {
                                        q1 = "if exists (select * from IT_StockDetail where ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and InwardType='" + inwardtype + "') update IT_StockDetail set TransferQty=ISNULL(TransferQty,0)+('" + transamt + "')  where ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and InwardType='" + inwardtype + "'";//,inwardqty='" + deductqty + "' ,BalQty=InwardQty -isnull('" + transamt + "',0)
                                        wherequery = "ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and InwardType='" + inwardtype + "'";
                                    }
                                    else
                                    {
                                        q1 = "if exists (select * from IT_StockDetail where ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and ISNULL(InwardType,0)=0) update IT_StockDetail set TransferQty=ISNULL(TransferQty,0)+('" + transamt + "') where ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and ISNULL(InwardType,0)=0";//,BalQty=InwardQty -isnull('" + transamt + "',0) 
                                        wherequery = "ItemFK ='" + itemfk + "' and StoreFK ='" + store + "' and ISNULL(InwardType,0)=0";
                                    }
                                    ds.Clear();
                                    int i = d2.update_method_wo_parameter(q1, "Text");
                                    string balup = "update IT_StockDetail set BalQty=InwardQty -isnull(TransferQty,0) where " + wherequery + "";
                                    int st = d2.update_method_wo_parameter(balup, "Text");



                                    if (i != 0)
                                    {
                                        string q2 = "insert into IT_TransferItem (TrasnferDate,TransferQty,TransferType,TransferFrom,TrasferTo,ItemFK,TransferRpu) values ('" + dtaccessdate + "','" + transamt + "','4','" + store + "','" + transdept + "','" + itemfk + "','" + rpu + "')";
                                        ds.Clear();
                                        int j = d2.update_method_wo_parameter(q2, "Text");
                                        if (j != 0)
                                        {
                                            string q4 = "select BalQty from IT_StockDeptDetail where ItemFK ='" + itemfk + "' and DeptFK ='" + transdept + "' ";
                                            ds.Clear();
                                            ds = d2.select_method_wo_parameter(q4, "Text");
                                            string avl1 = "";

                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                string avl = Convert.ToString(ds.Tables[0].Rows[0][0]);
                                                if (avl.Trim() != "")
                                                {
                                                    double inval1 = Convert.ToDouble(avl);
                                                    inval1 = inval1 + Convert.ToDouble(transamt);
                                                    avl1 = Convert.ToString(inval1);
                                                }
                                                else
                                                {
                                                    avl1 = transamt;
                                                }
                                            }
                                            else
                                            {
                                                avl1 = transamt;
                                            }


                                            string q3 = "if exists (select * from IT_StockDeptDetail where  ItemFK ='" + itemfk + "' and DeptFK ='" + transdept + "' )update IT_StockDeptDetail set BalQty ='" + avl1 + "',IssuedRPU='" + rpu + "',IssuedQty=IssuedQty+'" + avl1 + "' where  ItemFK ='" + itemfk + "' and DeptFK ='" + transdept + "' else insert into IT_StockDeptDetail (ItemFK,DeptFK,BalQty,IssuedRPU,IssuedQty) values ('" + itemfk + "','" + transdept + "','" + avl1 + "','" + rpu + "','" + avl1 + "')";
                                            ds.Clear();
                                            int k = d2.update_method_wo_parameter(q3, "Text");
                                            if (k != 0)
                                            {
                                                saveflag = true;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    lbl_alerterror.Visible = true;
                                    lbl_alerterror.Text = "Please enter the transfer quantity";
                                    alertmessage.Visible = true;
                                }

                            }
                        }

                    }
                    if (saveflag == true)
                    {

                        btn_transgo_Click(sender, e);
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Saved Successfully";
                        alertmessage.Visible = true;
                        FpSpread1.SaveChanges();
                    }
                    #endregion

                }
                if (ddl_transtype1.SelectedIndex == 1)
                {
                    //Mess To Department
                    #region Mess To Department
                    if (FpSpread1.Sheets[0].RowCount > 0)
                    {
                        for (int row = 0; row < FpSpread1.Sheets[0].RowCount; row++)
                        {

                            string hostelqunty = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 6].Text);
                            itemcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 6].Tag);
                            transamt = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 8].Text);
                            string deptfk = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 7].Tag);
                            string transdept = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 9].Tag);


                            rpu1 = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 7].Text);
                            rpu = Convert.ToDouble(rpu1);

                            if (transamt.Trim() != "")
                            {
                                double deductqunty = 0.00;
                                if (hostelqunty.Trim() != "" || hostelqunty.Trim() != "0.00")
                                {
                                    deductqunty = Convert.ToDouble(hostelqunty) - Convert.ToDouble(transamt);
                                }
                                else
                                {
                                    lbl_alerterror.Visible = true;
                                    lbl_alerterror.Text = "You can't Transfer!";
                                    return;
                                }



                                string q1 = "if exists (select * from IT_StockDeptDetail where  ItemFK ='" + itemcode + "' and DeptFK ='" + deptfk + "' ) update IT_StockDeptDetail set BalQty ='" + deductqunty + "',IssuedRPU='" + rpu + "' where  ItemFK ='" + itemcode + "' and DeptFK ='" + deptfk + "' ";
                                ds.Clear();
                                int i = d2.update_method_wo_parameter(q1, "Text");
                                if (i != 0)
                                {
                                    string q2 = "insert into IT_TransferItem (TrasnferDate,TransferQty,TransferType,TransferFrom,TrasferTo,ItemFK,TransferRpu) values ('" + dtaccessdate + "','" + transamt + "','2','" + deptfk + "','" + transdept + "','" + itemcode + "','" + rpu + "')";
                                    ds.Clear();
                                    int j = d2.update_method_wo_parameter(q2, "Text");
                                    if (j != 0)
                                    {
                                        string q4 = "select BalQty from IT_StockDeptDetail where ItemFK ='" + itemcode + "' and DeptFK ='" + deptfk + "'";
                                        ds.Clear();
                                        ds = d2.select_method_wo_parameter(q4, "Text");
                                        string avl1 = "";

                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            string avl = Convert.ToString(ds.Tables[0].Rows[0][0]);
                                            if (avl.Trim() != "")
                                            {
                                                double inval1 = Convert.ToDouble(avl);
                                                inval1 = inval1 + Convert.ToDouble(transamt);
                                                avl1 = Convert.ToString(inval1);
                                            }
                                            else
                                            {
                                                avl1 = transamt;
                                            }
                                        }
                                        else
                                        {
                                            avl1 = transamt;
                                        }
                                        string q3 = "if exists (select * from IT_StockDeptDetail where  ItemFK ='" + itemcode + "' and DeptFK ='" + deptfk + "')update IT_StockDeptDetail set BalQty ='" + avl1 + "',IssuedRPU='" + rpu + "',IssuedQty=IssuedQty+'" + avl1 + "' where  ItemFK ='" + itemcode + "' and DeptFK ='" + deptfk + "' else insert into IT_StockDeptDetail (ItemFK,DeptFK,BalQty,IssuedRPU,IssuedQty) values ('" + itemcode + "','" + deptfk + "','" + avl1 + "','" + rpu + "','" + avl1 + "')";
                                        ds.Clear();
                                        int k = d2.update_method_wo_parameter(q3, "Text");
                                        if (k != 0)
                                        {
                                            saveflag = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (saveflag == true)
                    {
                        btn_transgo_Click(sender, e);
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Saved Successfully";
                        alertmessage.Visible = true;
                        FpSpread1.SaveChanges();
                    }
                    else
                    {
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Please enter the transfor quantity";
                        alertmessage.Visible = true;
                    }
                    #endregion

                }

                if (ddl_transtype1.SelectedIndex == 2)
                {
                    //Department To Department

                    #region Departent to department
                    if (FpSpread1.Sheets[0].RowCount > 0)
                    {
                        for (int row = 0; row < FpSpread1.Sheets[0].RowCount; row++)
                        {
                            string deptfk = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 6].Tag);
                            if (deptfk.Trim() != "")
                            {
                                string itempk = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 3].Tag);
                                string rpu2 = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 2].Tag);
                                string handonqty = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 3].Text);
                                string deptfk1 = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 6].Tag);
                                transamt = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 5].Text);
                                if (transamt.Trim() != "")
                                {
                                    double deductqunty = 0.00;
                                    if (handonqty.Trim() != "" || handonqty.Trim() != "0.00")
                                    {
                                        deductqunty = Convert.ToDouble(handonqty) - Convert.ToDouble(transamt);
                                    }
                                    else
                                    {
                                        lbl_alerterror.Visible = true;
                                        lbl_alerterror.Text = "You can't Transfer!";
                                        return;
                                    }
                                    string q1 = "if exists (select * from IT_StockDeptDetail where  ItemFK ='" + itempk + "' and DeptFK ='" + deptfk + "') update IT_StockDeptDetail set BalQty ='" + deductqunty + "',IssuedRPU='" + rpu2 + "' where  ItemFK ='" + itempk + "' and DeptFK ='" + deptfk + "' ";
                                    ds.Clear();
                                    int i = d2.update_method_wo_parameter(q1, "Text");
                                    if (i != 0)
                                    {
                                        string q2 = "insert into IT_TransferItem (TrasnferDate,TransferQty,TransferType,TransferFrom,TrasferTo,ItemFK,TransferRpu) values ('" + dtaccessdate + "','" + transamt + "','5','" + deptfk + "','" + deptfk1 + "','" + itempk + "','" + rpu2 + "')";
                                        ds.Clear();
                                        int j = d2.update_method_wo_parameter(q2, "Text");
                                        if (j != 0)
                                        {
                                            string q4 = "select BalQty from IT_StockDeptDetail where ItemFK ='" + itempk + "' and DeptFK ='" + deptfk1 + "'";
                                            ds.Clear();
                                            ds = d2.select_method_wo_parameter(q4, "Text");
                                            string avl1 = "";

                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                string avl = Convert.ToString(ds.Tables[0].Rows[0][0]);
                                                if (avl.Trim() != "")
                                                {
                                                    double inval1 = Convert.ToDouble(avl);
                                                    inval1 = inval1 + Convert.ToDouble(transamt);
                                                    avl1 = Convert.ToString(inval1);
                                                }
                                                else
                                                {
                                                    avl1 = transamt;
                                                }
                                            }
                                            else
                                            {
                                                avl1 = transamt;
                                            }
                                            string q3 = "if exists (select * from IT_StockDeptDetail where  ItemFK ='" + itempk + "' and DeptFK ='" + deptfk1 + "')update IT_StockDeptDetail set BalQty ='" + avl1 + "',IssuedRPU='" + rpu2 + "',IssuedQty=IssuedQty+'" + avl1 + "' where  ItemFK ='" + itempk + "' and DeptFK ='" + deptfk1 + "' else insert into IT_StockDeptDetail (ItemFK,DeptFK,BalQty,IssuedRPU,IssuedQty) values ('" + itempk + "','" + deptfk1 + "','" + avl1 + "','" + rpu2 + "','" + avl1 + "')";
                                            ds.Clear();
                                            int k = d2.update_method_wo_parameter(q3, "Text");
                                            if (k != 0)
                                            {
                                                saveflag = true;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    lbl_alerterror.Visible = true;
                                    lbl_alerterror.Text = "Please enter the transfer quantity";
                                    alertmessage.Visible = true;
                                }

                            }
                        }
                    }
                    if (saveflag == true)
                    {
                        btn_transgo_Click(sender, e);
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Saved Successfully";
                        alertmessage.Visible = true;
                        FpSpread1.SaveChanges();
                    }
                    #endregion

                }
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
    protected void btn_ex_Click(object sender, EventArgs e)
    {
        try
        {
            popwindow3.Visible = false;
            //txt_transferqty.Text = "";
        }
        catch
        {

        }
    }
    protected void btn_exit1_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
        rptprint.Visible = false;
    }
    protected void rdb_datewise_Click(object sender, EventArgs e)
    {
        if (rdb_datewise.Checked == true)
        {
            txtpopfrom.Enabled = true;
            txt_to1.Enabled = true;
            //fromtodatevisibletrue();
            //rdb_hostohosctrlfalse();
        }
        else
        {
            txtpopfrom.Enabled = false;
            txt_to1.Enabled = false;
        }
    }
    protected void rdb_showall_Click(object sender, EventArgs e)
    {
        if (rdb_showall.Checked == true)
        {
            txtpopfrom.Enabled = false;
            txt_to1.Enabled = false;
            //fromtodatevisibletrue();
            //rdb_hostohosctrlfalse();
        }
    }

    //25.09.15

    protected void fromtodatevisibletrue()
    {
        lblpopfromdate.Visible = true;
        txtpopfrom.Visible = true;
        lbl_to.Visible = true;
        txt_to1.Visible = true;
    }
    protected void rdb_hostohosctrlfalse()
    {
        lbl_hostelname3.Visible = false;
        ddl_hostelname3.Visible = false;
        lbl_itemsearch.Visible = true;
        txt_searchitem.Visible = true;
    }
    protected void rdb_hostoctrltrue()
    {
        lbl_hostelname3.Visible = true;
        ddl_hostelname3.Visible = true;
        lbl_itemsearch.Visible = true;
        txt_searchitem.Visible = true;
    }
    protected void rdb_hostohos_Click(object sender, EventArgs e)
    {
        //FpSpread1.Visible = false;
        //spreaddiv.Visible = false;
        //lblpopfromdate.Visible = false;
        //txtpopfrom.Visible = false;
        //lbl_to.Visible = false;
        //txt_to1.Visible = false;
        //rdb_hostoctrltrue();
        //hostel = ddl_hostelname3.SelectedItem.Value;
    }
    protected void ddl_hostel_Selected_indexChange(object sender, EventArgs e)
    {
        hostel = ddl_hostelname3.SelectedItem.Value;
        txt_searchitem.Text = "";
    }
    protected void ddl_transtype_Selected_indexchange(object sender, EventArgs e)
    {
        if (ddl_transtype.SelectedItem.Value == "0")
        {
            fromtodatevisibletrue();
            rdb_showall.Visible = true;
            rdb_datewise.Visible = true;

            rdb_hostohosctrlfalse();
            rdb_hostohos.Visible = false;
            lbl_fromhostel.Visible = false;
            bhosname.Visible = false;
            FpSpread1.Visible = false;
            spreaddiv.Visible = false;
            btn_Transfer.Visible = false;
            btn_exit1.Visible = false;
            storetovisiablefalse();
            rdo_nonacodept.Visible = false;
            rdo_acodomicdept.Visible = false;
            ddl_acadamic.Visible = false;
            lbl_depttxt.Visible = false;

            rdo_acodomicdept.Checked = false;
        }
        else if (ddl_transtype.SelectedItem.Value == "1")
        {
            rdb_hostohos.Visible = false;
            lblpopfromdate.Visible = false;
            txtpopfrom.Visible = false;
            lbl_to.Visible = false;
            txt_to1.Visible = false;
            rdb_showall.Visible = false;
            rdb_datewise.Visible = false;
            rdb_hostoctrltrue();
            hostel = ddl_hostelname3.SelectedItem.Value;

            FpSpread1.Visible = false;
            spreaddiv.Visible = false;
            btn_Transfer.Visible = false;
            btn_exit1.Visible = false;
            storetovisiablefalse();

            rdo_nonacodept.Visible = false;
            rdo_acodomicdept.Visible = false;
            ddl_acadamic.Visible = false;
            lbl_depttxt.Visible = false;

            rdo_acodomicdept.Checked = false;
        }
        else if (ddl_transtype.SelectedItem.Value == "2")
        {
            lbl_hostelname3.Visible = false;
            ddl_hostelname3.Visible = false;
            lbl_itemsearch.Visible = false;
            txt_searchitem.Visible = false;
            rdb_hostohos.Visible = false;
            lblpopfromdate.Visible = false;
            txtpopfrom.Visible = false;
            lbl_to.Visible = false;
            txt_to1.Visible = false;
            rdb_showall.Visible = false;
            rdb_datewise.Visible = false;

            lbl_storename.Visible = true;
            ddl_storename.Visible = true;
            lbl_storesearch.Visible = true;
            txt_storetostore.Visible = true;
            bindstore();

            FpSpread1.Visible = false;
            spreaddiv.Visible = false;
            btn_Transfer.Visible = false;
            btn_exit1.Visible = false;

            rdo_nonacodept.Visible = false;
            rdo_acodomicdept.Visible = false;
            ddl_acadamic.Visible = false;
            lbl_depttxt.Visible = false;
            rdo_acodomicdept.Checked = false;
        }
        else if (ddl_transtype.SelectedItem.Value == "3")
        {
            lbl_hostelname3.Visible = false;
            ddl_hostelname3.Visible = false;
            lbl_itemsearch.Visible = false;
            txt_searchitem.Visible = false;
            rdb_hostohos.Visible = false;
            lblpopfromdate.Visible = false;
            txtpopfrom.Visible = false;
            lbl_to.Visible = false;
            txt_to1.Visible = false;
            rdb_showall.Visible = false;
            rdb_datewise.Visible = false;
            FpSpread1.Visible = false;
            spreaddiv.Visible = false;
            btn_Transfer.Visible = false;
            btn_exit1.Visible = false;
            lbl_storename.Visible = false;
            ddl_storename.Visible = false;
            lbl_storesearch.Visible = false;
            txt_storetostore.Visible = false;

            rdo_nonacodept.Visible = true;
            rdo_acodomicdept.Visible = true;
            ddl_acadamic.Visible = true;
            lbl_depttxt.Visible = true;
            rdo_acodomicdept.Checked = true;
        }
        else if (ddl_transtype.SelectedItem.Value == "4" || ddl_transtype.SelectedItem.Value == "5")
        {
            lbl_hostelname3.Visible = false;
            ddl_hostelname3.Visible = false;
            lbl_itemsearch.Visible = false;
            txt_searchitem.Visible = false;
            rdb_hostohos.Visible = false;
            lblpopfromdate.Visible = false;
            txtpopfrom.Visible = false;
            lbl_to.Visible = false;
            txt_to1.Visible = false;
            rdb_showall.Visible = false;
            rdb_datewise.Visible = false;
            FpSpread1.Visible = false;
            spreaddiv.Visible = false;
            btn_Transfer.Visible = false;
            btn_exit1.Visible = false;
            lbl_storename.Visible = false;
            ddl_storename.Visible = false;
            lbl_storesearch.Visible = false;
            txt_storetostore.Visible = false;

            rdo_nonacodept.Visible = false;
            rdo_acodomicdept.Visible = false;
            ddl_acadamic.Visible = false;
            lbl_depttxt.Visible = false;
            rdo_acodomicdept.Checked = false;

            rdb_hostohosctrlfalse();
            storetovisiablefalse();

            txt_searchitem.Visible = false;
            lbl_itemsearch.Visible = false;

            lbl_depttxt.Text = "Department Name";
            lbl_depttxt.Visible = true;
            ddl_acadamic.Visible = true;
        }

    }

    protected void storetovisiablefalse()
    {
        lbl_storename.Visible = false;
        ddl_storename.Visible = false;
        lbl_storesearch.Visible = false;
        txt_storetostore.Visible = false;
    }

    protected void transchange(object sender, EventArgs e)
    {
        double trans = 0.00;
        double total = 0.00;
        if (txt_transferqty.Text.Trim() != "")
        {
            trans = Convert.ToDouble(txt_transferqty.Text);
        }
        if (txt_totalQunatity.Text.Trim() != "")
        {
            total = Convert.ToDouble(txt_totalQunatity.Text);
        }
        if (total >= trans)
        {

        }
        else
        {
            txt_transferqty.Text = "";
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = "Transfer amount should be lesser than or equal to the Total Quantity";
            alertmessage.Visible = true;
        }

    }


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        //string query = "select distinct item_name from item_master WHERE item_name like '" + prefixText + "%'  ";
        string query = "";
        if (hostel.Trim() != "")
        {
            query = "select distinct ItemName from IM_ItemMaster im,IT_StockDeptDetail sm where im.ItemPK=sm.ItemFK and sm.DeptFK ='" + hostel + "' and ItemName like '" + prefixText + "%'";
        }
        else
        {
            query = "select distinct ItemName from IM_ItemMaster im,IT_StockDeptDetail sm where im.ItemPK=sm.ItemFK and ItemName like '" + prefixText + "%'";
        }
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables.Count > 0)
        {
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

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Itemname(string prefixText)
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

    protected void ddl_storename_Selected_indexChange(object sender, EventArgs e)
    {

    }
    protected void bindstore()
    {
        ds.Clear();
        //ds = d2.BindStore_inv(collegecode1);

        string storepk = d2.GetFunction("select value from Master_Settings where settings='Store Rights' and usercode='" + usercode + "'");
        ds = d2.BindStorebaseonrights_inv(storepk);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_storename.DataSource = ds;
            ddl_storename.DataTextField = "StoreName";
            ddl_storename.DataValueField = "StorePK";
            ddl_storename.DataBind();
        }
    }
    protected void bind_popstore()
    {
        string q1 = "";
        string storepk = d2.GetFunction("select value from Master_Settings where settings='Store Rights' and usercode='" + usercode + "'");
        if (ddl_transtype.SelectedItem.Value == "2")
        {
            q1 = "select StorePK,StoreName  from IM_StoreMaster where CollegeCode ='" + collegecode1 + "' and StorePK in(" + storepk + ") and StorePK<> ('" + Convert.ToString(ddl_storename.SelectedItem.Value) + "') order by StoreName";
        }
        else if (ddl_transtype.SelectedItem.Value == "5")
        {
            q1 = "select StorePK,StoreName  from IM_StoreMaster where CollegeCode ='" + collegecode1 + "'  and StorePK in(" + storepk + ")  order by StoreName";
        }
        ds.Clear();
        ds = d2.select_method_wo_parameter(q1, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_popstore.DataSource = ds;
            ddl_popstore.DataTextField = "StoreName";
            ddl_popstore.DataValueField = "StorePK";
            ddl_popstore.DataBind();

        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> storename(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select StoreName  from IM_StoreMaster WHERE StoreName like '" + prefixText + "%' order by StoreName ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["StoreName"].ToString());
            }
        }
        return name;
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
                    alertmessage.Visible = true;
                    lbl_alerterror.Visible = true;
                    lbl_alerterror.Text = "Enter FromDate less than or equal to the ToDate";
                    txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    spreaddiv1.Visible = false;
                    FpSpread2.Visible = false;
                    rptprint.Visible = false;
                    lbl_error.Visible = false;
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
            if (txt_todate.Text != "" && txt_fromdate.Text != "")
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
                    alertmessage.Visible = true;
                    lbl_alerterror.Visible = true;
                    lbl_alerterror.Text = "Enter ToDate greater than or equal to the FromDate";
                    txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    spreaddiv1.Visible = false;
                    FpSpread2.Visible = false;
                    rptprint.Visible = false;
                    lbl_error.Visible = false;
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
    protected void rdo_acodomicdept_oncheckedchange(object sender, EventArgs e)
    {
        if (rdo_acodomicdept.Checked == true)
        {
            lbl_depttxt.Text = "Acadamic";
        }
        else
        {
            lbl_depttxt.Text = "Non Acadamic";
        }
        binddept();
    }
    protected void rdo_nonacodept_oncheckedchange(object sender, EventArgs e)
    {
        if (rdo_acodomicdept.Checked == true)
        {
            lbl_depttxt.Text = "Acadamic";
        }
        else
        {
            lbl_depttxt.Text = "Non Acadamic";
        }
        binddept();
    }
    protected void ddl_acadamic_selected_indexchange(object sender, EventArgs e)
    {

    }
    protected void binddept()
    {
        string deptquery = "";
        if (ddl_transtype.SelectedItem.Value == "3")
        {
            if (rdo_acodomicdept.Checked == true)
            {
                deptquery = "select Dept_Code ,Dept_Name  from Department where college_code ='" + collegecode1 + "' and isacademic ='1' order by Dept_Code";
            }
            else if (rdo_nonacodept.Checked == true)
            {
                deptquery = "select Dept_Code ,Dept_Name  from Department where college_code ='" + collegecode1 + "' and isacademic ='0' order by Dept_Code";
            }
        }
        else //if (ddl_transtype.SelectedItem.Value == "4")
        {
            deptquery = "select Dept_Code ,Dept_Name  from Department where college_code ='" + collegecode1 + "' order by Dept_Name";
        }
        ds.Clear();
        ds = d2.select_method_wo_parameter(deptquery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_acadamic.DataSource = ds;
            ddl_acadamic.DataTextField = "Dept_Name";
            ddl_acadamic.DataValueField = "Dept_Code";
            ddl_acadamic.DataBind();
        }
        else
        {
            ddl_acadamic.Items.Clear();
        }
    }

    protected void bindtransdept()
    {
        string deptquery = "";
        if (ddl_transtype.SelectedItem.Value == "3")
        {
            if (rdo_acodomicdept.Checked == true)
            {
                deptquery = "select Dept_Code ,Dept_Name  from Department where college_code ='" + collegecode1 + "' and isacademic ='1' and Dept_Code not in ('" + ddl_acadamic.SelectedItem.Value + "') order by Dept_Code";
            }
            else if (rdo_nonacodept.Checked == true)
            {
                deptquery = "select Dept_Code ,Dept_Name  from Department where college_code ='" + collegecode1 + "' and isacademic ='0' and Dept_Code not in ('" + ddl_acadamic.SelectedItem.Value + "') order by Dept_Code";
            }
        }
        else if (ddl_transtype.SelectedItem.Value == "4")
        {
            deptquery = "select Dept_Code ,Dept_Name  from Department where college_code ='" + collegecode1 + "' and Dept_Code not in ('" + ddl_acadamic.SelectedItem.Value + "') order by Dept_Code";
        }
        ds.Clear();
        ds = d2.select_method_wo_parameter(deptquery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_transdept.DataSource = ds;
            ddl_transdept.DataTextField = "Dept_Name";
            ddl_transdept.DataValueField = "Dept_Code";
            ddl_transdept.DataBind();
        }
        else
        {
            ddl_acadamic.Items.Clear();
        }
    }
    //12.04.16
    public void Fpreadheaderbindmethod(string headername, FarPoint.Web.Spread.FpSpread spreadname, string AutoPostBack)
    {
        try
        {
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
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Font.Size = FontUnit.Smaller;
            lbl_alerterror.Text = ex.ToString();
        }
    }


    protected void Radio_ManTrans_Click(object sender, EventArgs e)
    {
        Trans.Visible = true;
        Reqtrans.Visible = false;
        spreaddiv.Visible = false;
    }

    protected void Radio_ReqTrans_Click(object sender, EventArgs e)
    {

        Trans.Visible = false;
        Reqtrans.Visible = true;
        spreaddiv.Visible = false;
    }


    protected void ddl_transtype1_Selected_indexchange(object sender, EventArgs e)
    {

        spreaddiv.Visible = false;
    }

    protected void ddl_Staff_Selected_indexchange(object sender, EventArgs e)
    {


    }

    protected void btn_transgo_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string firstdate = Convert.ToString(txtpopfrom.Text);
            string seconddate = Convert.ToString(txt_to1.Text);
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

            string storepk = d2.GetFunction("select value from Master_Settings where settings='Store Rights' and usercode='" + usercode + "'");
            string hostelcode = "";
            string deptcode = "";

            if (ddl_transtype1.SelectedIndex == 1)
            {
                for (int i = 0; i < cbl_hos.Items.Count; i++)
                {

                    if (hostelcode == "")
                    {
                        hostelcode = "" + cbl_hos.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        hostelcode = hostelcode + "'" + "," + "'" + cbl_hos.Items[i].Value.ToString() + "";
                    }

                }
            }

            if (ddl_transtype1.SelectedIndex == 2)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {

                    if (deptcode == "")
                    {
                        deptcode = "" + cbl_degree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        deptcode = deptcode + "'" + "," + "'" + cbl_degree.Items[i].Value.ToString() + "";
                    }

                }
            }

            FpSpread1.SaveChanges();

            if (ddl_transtype.SelectedItem.Value == "0")
            {
                #region Store
                string query = "";

                query = "select distinct OrderFK,InwardFK,case when InwardType='1' then 'Purchase Inward' when InwardType='2' then 'Direct Inward' when  InwardType='3' then 'Transfer' end  InwardType,InwardType as Inward_code,i.ItemCode as item_code,i.ItemName as item_name,i.ItemUnit as item_unit,SUM(CONVERT(float, ISNULL(InwardQty,0))- CONVERT(float ,ISNULL(TransferQty,0))) as hand_qty,InwardRPU as rpu,sd.StoreFK as Store_Code,s.StoreName as Store_Name,sd.ItemFK from IM_ItemMaster i,IT_StockDetail sd,IM_StoreMaster s where i.ItemPK =sd.ItemFK and sd.StoreFK =s.StorePK and InwardType='1' and sd.StoreFK in(" + storepk + ") group by  i.ItemCode,i.ItemName,i.ItemUnit,InwardRPU,sd.StoreFK,s.StoreName,sd.ItemFK,OrderFK,InwardFK,InwardType order by sd.ItemFK";
                query = query + " select distinct StorePK as Store_Code,StoreName as Store_Name from IM_StoreMaster s,IT_StockDetail sd where s.StorePK =sd.StoreFK and s.StorePK in(" + storepk + ") order by StorePK";



                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                DataView dv = new DataView();
                if (ds.Tables[1].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
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
                        FpSpread1.Columns[0].Width = 50;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Order Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[1].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Inward Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[2].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Inward Type";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[3].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Item Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[4].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Item Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[5].Width = 200;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Total Stocks";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[6].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Rpu";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[7].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Transfer Quantity";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[8].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Department Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[9].Width = 150;
                        int sno = 0;
                        for (int ik = 0; ik < ds.Tables[1].Rows.Count; ik++)
                        {
                            ds.Tables[0].DefaultView.RowFilter = " Store_Code='" + Convert.ToString(ds.Tables[1].Rows[ik]["Store_Code"]) + "'";
                            dv = ds.Tables[0].DefaultView;

                            if (dv.Count > 0)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(Convert.ToString(ds.Tables[1].Rows[ik]["Store_Name"]));
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[1].Rows[ik]["Store_Code"]);
                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 10);
                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].ForeColor = Color.Green;
                                for (int i = 0; i < dv.Count; i++)
                                {
                                    sno++;
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    //OrderFK,InwardFK
                                    string ordercode = Getordercode(Convert.ToString(dv[i]["OrderFK"]));
                                    if (ordercode == "0" || ordercode == "")
                                    {
                                        ordercode = "-";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ordercode;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dv[i]["OrderFK"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";


                                    string inwardcode = GetGoodsinwardcode(Convert.ToString(dv[i]["InwardFK"]));
                                    if (inwardcode == "0" || inwardcode == "")
                                    {
                                        inwardcode = "-";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = inwardcode;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dv[i]["InwardFK"]);

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                    string inwardtype = Convert.ToString(dv[i]["InwardType"]);
                                    if (inwardtype == "0" || inwardtype == "")
                                    {
                                        inwardtype = "-";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    }

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = inwardtype;// Convert.ToString(dv[i]["InwardType"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(dv[i]["Inward_code"]);
                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[i]["item_code"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(dv[i]["item_unit"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";


                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dv[i]["item_name"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(dv[i]["rpu"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dv[i]["hand_qty"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(dv[i]["Store_Code"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dv[i]["rpu"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Tag = Convert.ToString(dv[i]["ItemFK"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString("");
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString("");
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                                }
                            }
                        }
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        //FpSpread1.Sheets[0].FrozenRow.
                        //FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread1.Visible = true;
                        FpSpread1.SaveChanges();
                        spreaddiv.Visible = true;

                        lbl_error1.Visible = false;
                        //rptprint.Visible = true;
                        popwindow.Visible = true;
                        btn_exit1.Visible = true;
                        btn_Transfer.Visible = true;
                    }
                    else
                    {
                        lbl_error1.Visible = true;
                        FpSpread1.Visible = false;
                        spreaddiv.Visible = false;
                        lbl_error1.Text = "No Record Found";
                        btn_exit1.Visible = false;
                        //rptprint.Visible = false;
                        //  popwindow.Visible = false;
                        btn_Transfer.Visible = false;
                    }
                }
                else
                {
                    lbl_error1.Visible = true;
                    FpSpread1.Visible = false;
                    spreaddiv.Visible = false;
                    lbl_error1.Text = "No Record Found";
                    btn_exit1.Visible = false;
                    //rptprint.Visible = false;
                    //  popwindow.Visible = false;
                    btn_Transfer.Visible = false;
                }
                #endregion
            }
            if (ddl_transtype.SelectedItem.Value == "1")
            {
                #region Mess
                string query2 = "";
                query2 = "select distinct i.ItemCode as item_code,i.ItemName as item_name,i.ItemUnit as item_unit,sd.BalQty as AvlQty,sd.IssuedRPU as RPU,sd.ItemFK,sd.DeptFK,case when InwardType='1' then 'Purchase Inward' when InwardType='2' then 'Direct Inward' when  InwardType='3' then 'Transfer' end  InwardType from IM_ItemMaster i,IT_StockDeptDetail sd where i.ItemPK =sd.ItemFK and sd.DeptFK in ('" + hostelcode + "') and InwardType='1'";

                ds1.Clear();
                ds1 = d2.select_method_wo_parameter(query2, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
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
                    FpSpread1.Columns[0].Width = 50;
                    //
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Order Code";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[1].Width = 100;
                    FpSpread1.Columns[1].Visible = false;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Inward Code";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[2].Width = 100;
                    FpSpread1.Columns[2].Visible = false;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Inward Type";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[3].Width = 100;
                    FpSpread1.Columns[3].Visible = false;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Item Code";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[4].Width = 100;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Item Name";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[5].Width = 200;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Available Stocks";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[6].Width = 100;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Rpu";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[7].Width = 100;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Transfer Quantity";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[8].Width = 100;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Mess Name";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[9].Width = 150;

                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";



                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds1.Tables[0].Rows[i]["item_code"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds1.Tables[0].Rows[i]["item_unit"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds1.Tables[0].Rows[i]["item_name"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds1.Tables[0].Rows[i]["rpu"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds1.Tables[0].Rows[i]["AvlQty"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(ds1.Tables[0].Rows[i]["ItemFK"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds1.Tables[0].Rows[i]["rpu"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Tag = Convert.ToString(ds1.Tables[0].Rows[i]["DeptFK"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString("");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString("");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                    }
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;

                    FpSpread1.Visible = true;
                    FpSpread1.SaveChanges();
                    spreaddiv.Visible = true;
                    lbl_error1.Visible = false;
                    //rptprint.Visible = true;
                    popwindow.Visible = true;
                    btn_exit1.Visible = true;
                    btn_Transfer.Visible = true;
                }
                else
                {
                    lbl_error1.Visible = true;
                    FpSpread1.Visible = false;
                    spreaddiv.Visible = false;
                    lbl_error1.Text = "No Record Found";
                    // popwindow.Visible = false;
                    btn_Transfer.Visible = false;
                    btn_exit1.Visible = false;
                }
                txt_searchitem.Text = "";
                #endregion
            }
            else if (ddl_transtype.SelectedItem.Value == "3")
            {
                #region department
                string query2 = "";

                query2 = "select distinct i.ItemCode as item_code,i.ItemName as item_name,i.ItemUnit as item_unit,sd.BalQty as AvlQty,sd.IssuedRPU as RPU,sd.ItemFK,sd.DeptFK from IM_ItemMaster i,IT_StockDeptDetail sd,Department d where i.ItemPK =sd.ItemFK and d.Dept_Code=sd.DeptFK and  sd.DeptFK in('" + deptcode + "') and InwardType='1' ";

                ds1.Clear();
                ds1 = d2.select_method_wo_parameter(query2, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    string name = "";
                    name = "Department Name";
                    string q1 = "S.No/Item Code/Item Name/Available Stocks/Rpu/Transfer Quantity/" + name + "";
                    Fpreadheaderbindmethod(q1, FpSpread1, "TRUE");
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds1.Tables[0].Rows[i]["item_code"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds1.Tables[0].Rows[i]["item_unit"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds1.Tables[0].Rows[i]["item_name"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds1.Tables[0].Rows[i]["rpu"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds1.Tables[0].Rows[i]["AvlQty"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds1.Tables[0].Rows[i]["ItemFK"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds1.Tables[0].Rows[i]["rpu"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds1.Tables[0].Rows[i]["DeptFK"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString("");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString("");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                    }
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.Visible = true;
                    FpSpread1.SaveChanges();
                    spreaddiv.Visible = true;
                    lbl_error1.Visible = false;
                    popwindow.Visible = true;
                    btn_exit1.Visible = true;
                    btn_Transfer.Visible = true;
                }
                else
                {
                    lbl_error1.Visible = true;
                    FpSpread1.Visible = false;
                    spreaddiv.Visible = false;
                    lbl_error1.Text = "No Record Found";
                    btn_exit1.Visible = false;
                    btn_Transfer.Visible = false;
                }
                #endregion
            }
        }
        catch
        {
        }

    }



    public void requeststaff()
    {
        try
        {
            DataSet dstaff = new DataSet();
            string staff = "select distinct r.ReqAppNo  from RQ_Requisition r,RQ_RequisitionDet d,IM_ItemMaster i where RequestType=1 and RequisitionFK=RequisitionPK and i.ItemPK=d.ItemFK and d.ReqAppStatus='1'";
            dstaff.Clear();
            dstaff = d2.select_method_wo_parameter(staff, "Text");
            string staff1 = "";
            string staff2 = "";
            if (dstaff.Tables.Count > 0 && dstaff.Tables[0].Rows.Count > 0)
            {
                for (int t = 0; t < dstaff.Tables[0].Rows.Count; t++)
                {
                    if (staff2 == "")
                        staff2 = Convert.ToString(dstaff.Tables[0].Rows[t]["ReqAppNo"]);
                    else
                        staff2 = staff2 + ',' + Convert.ToString(dstaff.Tables[0].Rows[t]["ReqAppNo"]);
                }

            }
            if (staff2 != "")
            {
                string s = "  select distinct appl_id,staff_name  from staff_appl_master a, staffmaster s where a.appl_no=s.appl_no and appl_id in ('" + staff2 + "')";
                dstaff.Clear();
                dstaff = d2.select_method_wo_parameter(s, "Text");
                if (dstaff.Tables.Count > 0 && dstaff.Tables[0].Rows.Count > 0)
                {
                    ddl_Staff.DataSource = dstaff;
                    ddl_Staff.DataTextField = "staff_name";
                    ddl_Staff.DataValueField = "appl_id";
                    ddl_Staff.DataBind();
                }
            }

        }
        catch
        {

        }
    }
}
