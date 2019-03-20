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
using System.Text;


public partial class vendor_quotation_compare : System.Web.UI.Page
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
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    Hashtable ht = new Hashtable();
    static bool rowCheck = false;
    DataTable datasuppl = new DataTable();
    DataRow drow;


    protected void Page_Load(object sender, EventArgs e)
    {
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
        lblvalidation1.Text = "";
        Session["vendor"] = null;
        Session["CompCode"] = null;
        Session["itembook"] = null;
        if (!IsPostBack)
        {
            ddl_reqcompcode_bind();
            ItemList.Clear();
            rowCheck = false;
            Rad_item.Checked = true;
            try
            {
                string get_vendorcode_fromcompare = (Convert.ToString(Request.QueryString["@@ssss$$"]));


                if (get_vendorcode_fromcompare.Trim() != "" && get_vendorcode_fromcompare.Trim() != null)
                {
                    string[] split = get_vendorcode_fromcompare.Split('s');

                    string get_vendorcode_fromcompare1 = split[0];

                    Session["vendor"] = split[0];
                    Session["CompCode"] = split[1];
                    Session["itembook"] = split[2];
                    btn_go1_Click(sender, e);

                }
            }
            catch
            {

            }

        }

    }
    protected void lnk_btnlogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }
    protected void cb_venname_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;
        txt_venname.Text = "--Select--";

        if (cb_venname.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_venname.Items.Count; i++)
            {
                cbl_venname.Items[i].Selected = true;
            }
            txt_venname.Text = "Supplier Name(" + (cbl_venname.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_venname.Items.Count; i++)
            {
                cbl_venname.Items[i].Selected = false;
            }
        }
    }
    protected void cbl_venname_SelectedIndexChange(object sender, EventArgs e)
    {
        int i = 0;
        cb_venname.Checked = false;
        int commcount = 0;
        txt_venname.Text = "--Select--";
        for (i = 0; i < cbl_venname.Items.Count; i++)
        {
            if (cbl_venname.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_venname.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_venname.Items.Count)
            {
                cb_venname.Checked = true;
            }
            txt_venname.Text = "Supplier Name(" + commcount.ToString() + ")";
        }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            Session["vendor"] = "";
            Session["CompCode"] = "";
            int c = FpSpread1.Sheets[0].RowCount;
            Printcontrol.Visible = false;
            lbl_baseerror.Visible = false;
            int j = 0;
            string venquocode = "";
            for (j = 0; j < cbl_venname.Items.Count; j++)
            {
                if (cbl_venname.Items[j].Selected == true)
                {
                    string build = cbl_venname.Items[j].Value.ToString();
                    if (venquocode == "")
                    {
                        venquocode = build;
                    }
                    else
                    {
                        venquocode = venquocode + "'" + "," + "'" + build;
                    }
                }
            }
            string q2 = "";

            if (txt_venname.Text.Trim() != "--Select--" && ddl_reqcompcode.SelectedItem.Text.Trim() != "Select")
            {
                if (Rad_item.Checked)
                    q2 = "select distinct rq.reqcompcode,vm.VendorPK,vm.VendorCompName,vm.VendorCode, vm.VendorMobileNo,rq.VenReqCode,vq.VenQuotNo,vq.VenQuotCode from IT_VendorReq rq,IT_VendorQuot vq,IT_VednorQuotDet vd,CO_VendorMaster vm where vq.VendorQuotPK=vd.VendorQuotFK and rq.VendorFK=vq.VendorFK and vm.VendorPK=vq.VendorFK and ISNULL(rq.itemorlibrary,'')='' and isnull(LibraryFlag,'')='' and vq.VendorFK in('" + venquocode + "') and rq.ReqCompCode in ('" + ddl_reqcompcode.SelectedItem.Text + "') and rq.VenReqPK=vq.VenReqFK ";
                //and rq.VenReqPK=vd.VednorQuotDetPK
                if (Rad_book.Checked)
                    q2 = "select distinct rq.reqcompcode,vm.VendorPK,vm.VendorCompName,vm.VendorCode, vm.VendorMobileNo,rq.VenReqCode,vq.VenQuotNo,vq.VenQuotCode from IT_VendorReq rq,IT_VendorQuot vq,IT_VednorQuotDet vd,CO_VendorMaster vm where vq.VendorQuotPK=vd.VendorQuotFK and rq.VendorFK=vq.VendorFK and vm.VendorPK=vq.VendorFK and rq.itemorlibrary='1' and LibraryFlag='1' and vq.VendorFK in('" + venquocode + "') and rq.ReqCompCode in ('" + ddl_reqcompcode.SelectedItem.Text + "') and rq.VenReqPK=vq.VenReqFK ";
                //and rq.VenReqPK=vd.VednorQuotDetPK
                ds.Clear();
                ds = d2.select_method_wo_parameter(q2, "Text");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    int startspanpoint = 0;
                    int rowcount1 = 1;
                    FpSpread1.Sheets[0].RowCount = 0;
                    FpSpread1.Sheets[0].ColumnCount = 0;
                    FpSpread1.CommandBar.Visible = false;
                    FpSpread1.Sheets[0].AutoPostBack = false;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread1.Sheets[0].RowHeader.Visible = false;
                    FpSpread1.Sheets[0].ColumnCount = 8;

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

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[1].Width = 50;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Supplier Name";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[2].Width = 200;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Supplier Code";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[3].Width = 100;
                    FpSpread1.Columns[3].Locked = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Supplier Mobile No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[4].Width = 150;
                    FpSpread1.Columns[4].Locked = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Supplier Request Code";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[5].Width = 100;
                    FpSpread1.Columns[5].Locked = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Supplier Quotation Code";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[6].Width = 100;
                    FpSpread1.Columns[6].Locked = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Supplier Quotation No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[7].Width = 100;
                    FpSpread1.Columns[7].Locked = true;
                    FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                    chkall.AutoPostBack = false;
                    string vennam = "";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        if (vennam != Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]))
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chkall;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                            FpSpread1.Sheets[0].SpanModel.Add(startspanpoint, 1, rowcount1, 1);
                            startspanpoint = FpSpread1.Sheets[0].RowCount - 1;
                        }
                        else
                        {
                            rowcount1++;
                        }
                        vennam = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["VendorPK"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";


                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCode"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["reqcompcode"]);

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorMobileNo"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["VenReqCode"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["VenQuotCode"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["VenQuotNo"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                    }

                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    // FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread1.Sheets[0].FrozenRowCount = 0;
                    FpSpread1.Visible = true;
                    spreaddiv.Visible = true;
                    btn_selectitem.Visible = true;
                    btn_purchasereq.Visible = true;

                    pheaderfilter.Visible = true;
                    pcolumnorder.Visible = true;
                }
                else
                {
                    FpSpread1.Visible = false;
                    spreaddiv.Visible = false;
                    lbl_baseerror.Visible = true;
                    pheaderfilter.Visible = false;
                    pcolumnorder.Visible = false;
                    btn_selectitem.Visible = false;
                    //tborder.Visible = false;
                    lbl_baseerror.Text = "No Record Founds";


                    if (FpSpread5.Visible == true)
                    {
                        btn_purchasereq.Visible = true;
                        rptprint.Visible = true;

                    }
                    else
                    {
                        btn_purchasereq.Visible = false;
                        rptprint.Visible = false;
                    }
                }
            }
            else
            {
                FpSpread1.Visible = false;
                spreaddiv.Visible = false;
                rptprint.Visible = false;
                btn_selectitem.Visible = false;
                lbl_baseerror.Visible = true;
                pheaderfilter.Visible = false;
                pcolumnorder.Visible = false;
                spreaddiv1.Visible = false;
                lbl_baseerror.Text = "Please Select All fields";
            }
        }
        catch
        {

        }

    }
    protected void btn_go1_Click(object sender, EventArgs e)
    {
        try
        {
            bool chk = false;
            bool check = false;
            string uncheck = "";
            string vendorpk = "";
            string reqcomparecode = "";
            string itembook1 = "";
            StringBuilder sbvendorpk = new StringBuilder();
            StringBuilder sbreqcomparecode = new StringBuilder();

            string venpk = "";
            string comcode = "";
            string vendorpk1 = string.Empty;
            string reqcomparecode1 = string.Empty;
            sbvendorpk.Append(venpk).Append("','");
            FpSpread5.SaveChanges();
            int rowcount = 0;
            int Columncount = 0;
            FpSpread5.Visible = true;
            Showgrid.Visible = true;
            Div_supplierDetails.Visible = true;
            Divgridprint.Visible = true;
            int c = FpSpread1.Sheets[0].RowCount;

            if (rowCheck)
            {
                rowcount = FpSpread5.Sheets[0].RowCount;
                int cocnt = 4;
                Columncount = FpSpread5.Sheets[0].ColumnCount;
                //for (int i = 0; i < rowcount; i++)
                //{
                for (cocnt = cocnt; cocnt < Columncount; cocnt = cocnt + 12)
                {
                    vendorpk = Convert.ToString(FpSpread5.Sheets[0].ColumnHeader.Cells[0, cocnt].Tag);
                    reqcomparecode = Convert.ToString(FpSpread5.Sheets[0].ColumnHeader.Cells[1, cocnt + 11].Tag);


                    if (vendorpk != "" && reqcomparecode != "")
                    {
                        sbvendorpk.Append(vendorpk).Append("','");
                        sbreqcomparecode.Append(reqcomparecode).Append("','");
                    }
                }
                //}
            }
            vendorpk1 = Convert.ToString(sbvendorpk);
            vendorpk1 = vendorpk1.TrimEnd(',');
            reqcomparecode1 = Convert.ToString(sbreqcomparecode);
            reqcomparecode1 = reqcomparecode1.TrimEnd(',');


            if (Session["vendor"] != null && Session["vendor"] != "" && Session["CompCode"] != null && Session["CompCode"] != "" && Session["itembook"] != null && Session["itembook"] != "")
            {
                vendorpk = Session["vendor"].ToString();
                reqcomparecode = Session["CompCode"].ToString();
                itembook1 = Session["itembook"].ToString();
                Session["vendor"] = "";
                Session["CompCode"] = "";
            }
            else
            {
                if (FpSpread1.Sheets[0].RowCount > 0)
                {
                    FpSpread1.SaveChanges();
                    for (int row = 0; row < FpSpread1.Sheets[0].RowCount; row++)
                    {
                        int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, 1].Value);
                        if (checkval == 1)
                        {
                            vendorpk = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 2].Tag);
                            reqcomparecode = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 3].Tag);
                            vendorpk = vendorpk + "','" + vendorpk1;
                            reqcomparecode = reqcomparecode + "','" + reqcomparecode1;
                            Session["vendorpk"] = vendorpk;
                            Session["reqcomparecode"] = reqcomparecode;


                        }

                    }
                }

            }
            if (itembook1 != "")
            {
                if (itembook1 == "1")
                    Rad_item.Checked = true;
                if (itembook1 == "2")
                    Rad_book.Checked = true;

            }
            string q1 = "";
            if (vendorpk != "" && reqcomparecode != "")
            {
                if (Rad_item.Checked)
                {
                    q1 = "select distinct vq.VendorfK,i.itemname,i.ItemCode,vd.RPU, vd.DiscountAmt,vd.TaxPercent,vd.ItemFK, vd.EduCessPer,HigherEduCessPer,vd.ExeciseTaxPer, vd.OtherChargeAmt,vm.VendorCompName,vm.VendorCode,rq.ReqCompCode,vd.IsDiscountPercent from IT_VendorReq rq,IT_VendorQuot vq,IT_VednorQuotDet vd,IM_ItemMaster i,CO_VendorMaster vm where vq.VendorQuotPK=vd.VendorQuotFK and rq.VendorFK=vq.VendorFK and i.ItemPK=vd.ItemFK and vm.VendorPK=vq.VendorFK and vq.VendorFK in('" + vendorpk + "') and rq.ReqCompCode in ('" + reqcomparecode + "') and ISNULL(rq.itemorlibrary,'')='' and isnull(LibraryFlag,'')='' and rq.VenReqPK=vq.VenReqFK and rq.PurchaseStatus<>'1'   order by itemname;";
                    q1 = q1 + "select distinct vq.VendorfK,vm.VendorCompName,rq.ReqCompCode from IT_VendorReq rq,IT_VendorQuot vq,IT_VednorQuotDet vd,IM_ItemMaster i,CO_VendorMaster vm where vq.VendorQuotPK=vd.VendorQuotFK and rq.VendorFK=vq.VendorFK and i.ItemPK=vd.ItemFK and vm.VendorPK=vq.VendorFK and vq.VendorFK in('" + vendorpk + "') and rq.ReqCompCode in ('" + reqcomparecode + "') and ISNULL(rq.itemorlibrary,'')='' and isnull(LibraryFlag,'')='' and rq.VenReqPK=vq.VenReqFK and rq.PurchaseStatus<>'1' ;";
                    q1 = q1 + "select Sum(vd.Qty)Qty,i.ItemName,vq.VendorfK  from IT_VendorReq rq,IT_VendorQuot vq,IT_VednorQuotDet vd,IM_ItemMaster i,CO_VendorMaster vm where vq.VendorQuotPK=vd.VendorQuotFK and rq.VendorFK=vq.VendorFK and i.ItemPK=vd.ItemFK and vm.VendorPK=vq.VendorFK and vq.VendorFK in('" + vendorpk + "') and rq.ReqCompCode in ('" + reqcomparecode + "') and ISNULL(rq.itemorlibrary,'')='' and isnull(LibraryFlag,'')='' and rq.VenReqPK=vq.VenReqFK and rq.PurchaseStatus<>'1'   Group by itemname,Vq.VendorfK;";
                }
                if (Rad_book.Checked)
                {
                    q1 = "select distinct vq.VendorfK,bd.Title itemname,bd.BookID ItemCode,vd.RPU, vd.DiscountAmt,vd.TaxPercent,vd.ItemFK, vd.EduCessPer,HigherEduCessPer,vd.ExeciseTaxPer, vd.OtherChargeAmt,vm.VendorCompName,vm.VendorCode,rq.ReqCompCode,vd.IsDiscountPercent from IT_VendorReq rq,IT_VendorQuot vq,IT_VednorQuotDet vd,bookdetails bd,CO_VendorMaster vm where vq.VendorQuotPK=vd.VendorQuotFK and rq.VendorFK=vq.VendorFK and bd.BookID=vd.ItemFK and vm.VendorPK=vq.VendorFK and vq.VendorFK in('" + vendorpk + "') and rq.ReqCompCode in ('" + reqcomparecode + "') and rq.itemorlibrary='1' and LibraryFlag='1' and rq.VenReqPK=vq.VenReqFK and rq.PurchaseStatus<>'1' ;";
                    q1 = q1 + "select distinct vq.VendorfK,vm.VendorCompName,rq.ReqCompCode from IT_VendorReq rq,IT_VendorQuot vq,IT_VednorQuotDet vd,bookdetails bd,CO_VendorMaster vm where vq.VendorQuotPK=vd.VendorQuotFK and rq.VendorFK=vq.VendorFK and bd.BookID=vd.ItemFK and vm.VendorPK=vq.VendorFK and vq.VendorFK in('" + vendorpk + "') and rq.ReqCompCode in ('" + reqcomparecode + "') and rq.itemorlibrary='1' and LibraryFlag='1' and rq.VenReqPK=vq.VenReqFK and rq.PurchaseStatus<>'1' ;";
                    q1 = q1 + "select Sum(vd.Qty)Qty,bd.Title itemname,vq.VendorfK from IT_VendorReq rq,IT_VendorQuot vq,IT_VednorQuotDet vd,bookdetails bd,CO_VendorMaster vm where vq.VendorQuotPK=vd.VendorQuotFK and rq.VendorFK=vq.VendorFK and bd.BookID=vd.ItemFK and vm.VendorPK=vq.VendorFK and vq.VendorFK in('" + vendorpk + "') and rq.ReqCompCode in ('" + reqcomparecode + "') and rq.itemorlibrary='1' and LibraryFlag='1' and rq.VenReqPK=vq.VenReqFK and rq.PurchaseStatus<>'1' ;";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    rowCheck = true;
                    int startspanpoint = 0;
                    int rowcount1 = 1;

                    #region ADDITEM
                    FpSpread5.Sheets[0].RowCount = 0;
                    FpSpread5.Sheets[0].ColumnCount = 0;
                    FpSpread5.CommandBar.Visible = false;
                    FpSpread5.Sheets[0].AutoPostBack = false;
                    FpSpread5.Sheets[0].ColumnHeader.RowCount = 2;
                    FpSpread5.Sheets[0].RowHeader.Visible = false;
                    FpSpread5.Sheets[0].ColumnCount = 4;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    FpSpread5.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    FpSpread5.Columns[0].Width = 50;
                    FpSpread5.Columns[0].Locked = true;

                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread5.Columns[1].Width = 50;



                    if (Rad_item.Checked)
                    {
                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Name";
                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Item Code";
                    }
                    if (Rad_book.Checked)
                    {
                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Book Name";
                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Book ID";
                    }

                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread5.Columns[2].Width = 200;
                    FpSpread5.Columns[2].Locked = true;
                    FpSpread5.Columns[2].Visible = false;



                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    FpSpread5.Columns[3].Width = 100;
                    FpSpread5.Columns[3].Locked = true;
                    FpSpread5.Columns[3].Visible = false;


                    FpSpread5.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                    FpSpread5.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                    FpSpread5.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                    FpSpread5.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                    #endregion



                    //FpSpread5.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Supplier Name";
                    //FpSpread5.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    //FpSpread5.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread5.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    //FpSpread5.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    //FpSpread5.Columns[4].Width = 200;
                    //FpSpread5.Columns[4].Locked = true;
                    //FpSpread5.Columns[4].Visible = false;



                    //Header Column Bind
                    string venname = "";


                    FarPoint.Web.Spread.CheckBoxCellType chkall1 = new FarPoint.Web.Spread.CheckBoxCellType();
                    chkall1.AutoPostBack = false;

                    FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                    chkall.AutoPostBack = false;

                    FpSpread5.Sheets[0].RowCount++;
                    int storecnt = 0;

                    #region Supplier_Grid
                    ArrayList arrColHdrNames1 = new ArrayList();
                    datasuppl.Columns.Clear();
                    datasuppl.Columns.Add("S.No", typeof(string));
                    datasuppl.Columns.Add("Vendor Name", typeof(string));
                    datasuppl.Columns.Add("No.Of Items", typeof(string));
                    datasuppl.Columns.Add("Rate", typeof(string));

                    arrColHdrNames1.Add("S.No");
                    arrColHdrNames1.Add("Vendor Name");
                    arrColHdrNames1.Add("No.Of Items");
                    arrColHdrNames1.Add("Rate");
                    DataRow drHdr1 = datasuppl.NewRow();

                    for (int grCol = 0; grCol < datasuppl.Columns.Count; grCol++)
                        drHdr1[grCol] = arrColHdrNames1[grCol];
                    datasuppl.Rows.Add(drHdr1);
                    #endregion


                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {

                        if (venname != Convert.ToString(ds.Tables[1].Rows[i]["VendorCompName"]))
                        {

                            FpSpread5.Sheets[0].ColumnCount = FpSpread5.Sheets[0].ColumnCount + 12;
                            storecnt++;
                            //FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 2].CellType = chkall1;
                            //FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                            // FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 2].BackColor = Color.Green;


                            FpSpread5.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread5.Sheets[0].ColumnCount - 12, 1, 12);
                            FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 12].Text = Convert.ToString(ds.Tables[1].Rows[i]["VendorCompName"]);
                            FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 12].Tag = Convert.ToString(ds.Tables[1].Rows[i]["VendorfK"]);
                            FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 12].Font.Bold = true;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 12].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 12].Font.Name = "Book Antiqua";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 12].Font.Size = FontUnit.Medium;

                            FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, FpSpread5.Sheets[0].ColumnCount - 12].CellType = chkall;
                            FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, FpSpread5.Sheets[0].ColumnCount - 12].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, FpSpread5.Sheets[0].ColumnCount - 12].BackColor = Color.Green;

                            FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, FpSpread5.Sheets[0].ColumnCount - 12].Locked = false;
                            FpSpread5.Sheets[0].SpanModel.Add(0, FpSpread5.Sheets[0].ColumnCount - 12, 1, 12);
                            //FpSpread5.Columns[colcnt].Width = 200;
                            //FpSpread5.Columns[colcnt].Locked = true;
                            //FpSpread5.Columns[colcnt].Visible = false;
                            //FpSpread5.Sheets[0].Cells[0, FpSpread5.Sheets[0].ColumnCount - 12].CellType = chkall1;
                            //startspanpoint = FpSpread5.Sheets[0].RowCount - 1;

                            venname = Convert.ToString(ds.Tables[1].Rows[i]["VendorCompName"]);


                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 12].Text = "Quantity";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 12].Font.Bold = true;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 12].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 12].Font.Name = "Book Antiqua";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 12].Font.Size = FontUnit.Medium;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 12].Width = 100;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 12].Locked = true;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 12].Visible = false;

                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 11].Text = "Rpu";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 11].Font.Bold = true;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 11].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 11].Font.Name = "Book Antiqua";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 11].Font.Size = FontUnit.Medium;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 11].Width = 100;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 11].Locked = true;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 11].Visible = false;



                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 10].Text = "Discount";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 10].Font.Bold = true;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 10].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 10].Font.Name = "Book Antiqua";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 10].Font.Size = FontUnit.Medium;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 10].Width = 100;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 10].Locked = true;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 10].Visible = false;


                            //FpSpread5.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Discount";
                            //FpSpread5.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                            //FpSpread5.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                            //FpSpread5.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                            //FpSpread5.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                            //FpSpread5.Columns[8].Width = 100;
                            //FpSpread5.Columns[8].Locked = true;
                            FpSpread5.Columns[7].Visible = false;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 9].Text = "Tax";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 9].Font.Bold = true;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 9].Font.Name = "Book Antiqua";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 9].Font.Size = FontUnit.Medium;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 9].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 9].Width = 100;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 9].Locked = true;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 9].Visible = false;


                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 8].Text = "Exercies tax";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 8].Font.Bold = true;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 8].Font.Name = "Book Antiqua";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 8].Font.Size = FontUnit.Medium;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 8].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 8].Width = 100;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 8].Locked = true;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 8].Visible = false;



                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 7].Text = "Education Cess";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 7].Font.Bold = true;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 7].Font.Name = "Book Antiqua";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 7].Font.Size = FontUnit.Medium;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 7].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 7].Width = 100;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 7].Locked = true;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 7].Visible = false;



                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 6].Text = "Higher Edu.Cess";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 6].Font.Bold = true;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 6].Font.Name = "Book Antiqua";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 6].Font.Size = FontUnit.Medium;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 6].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 6].Width = 150;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 6].Locked = true;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 6].Visible = false;



                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 5].Text = "Other Charges";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 5].Font.Bold = true;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 5].Font.Name = "Book Antiqua";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 5].Font.Size = FontUnit.Medium;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 5].Width = 150;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 5].Locked = true;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 5].Visible = false;



                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 4].Text = "CallEduCess";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 4].Font.Bold = true;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 4].Font.Name = "Book Antiqua";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 4].Font.Size = FontUnit.Medium;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 4].Width = 150;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 4].Locked = true;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 4].Visible = false;




                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 3].Text = "CallExTax";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 3].Font.Bold = true;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 3].Font.Name = "Book Antiqua";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 3].Font.Size = FontUnit.Medium;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 3].Width = 150;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 3].Locked = true;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 3].Visible = false;



                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 2].Text = "Cost";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 2].Font.Bold = true;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 2].Width = 100;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 2].Locked = true;
                            FpSpread5.Columns[FpSpread5.Sheets[0].ColumnCount - 2].Visible = false;

                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 1].Text = "Compare Code";
                            FpSpread5.Sheets[0].Columns[FpSpread5.Sheets[0].ColumnCount - 1].Visible = false;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread5.Sheets[0].ColumnHeader.Cells[1, FpSpread5.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[1].Rows[i]["ReqCompCode"]);


                        }
                        //else
                        //{
                        //    rowcount1++;
                        //}




                    }

                    // Row Bind


                    int colcnt = 4;
                    string vennam = "";
                    string itemname = "";
                    DataTable dtstdet = ds.Tables[0].DefaultView.ToTable(true, "itemname");
                    if (dtstdet.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtstdet.Rows.Count; i++)
                        {
                            string itemname1 = Convert.ToString(dtstdet.Rows[i]["itemname"]);
                            ds.Tables[0].DefaultView.RowFilter = " itemname='" + itemname1 + "'";
                            DataView dv = ds.Tables[0].DefaultView;

                            if (dv.Count > 0)
                            {
                                FpSpread5.Sheets[0].RowCount++;
                                FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                                FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";


                                FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 1].CellType = chkall;
                                FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                                FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv[0]["itemname"]);
                                FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[0]["ItemCode"]);
                                FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(dv[0]["ItemFK"]);
                                FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                int countcolumn = FpSpread5.Sheets[0].ColumnCount;
                                int colcount = 4;
                                int countcolumn1 = 16;
                                int prerpu = 0;
                                string prerate = "";

                                Dictionary<int, string> dicrpu = new Dictionary<int, string>();
                                ArrayList arrpu = new ArrayList();
                                //arrpu.Sort();
                                for (int j = 0; j < storecnt; j++)
                                {

                                    string store = Convert.ToString(FpSpread5.Sheets[0].ColumnHeader.Cells[0, colcount].Tag);
                                    ds.Tables[0].DefaultView.RowFilter = " itemname='" + itemname1 + "' and VendorfK='" + store + "'";
                                    DataView dv2 = ds.Tables[0].DefaultView;

                                    for (int k = 0; k < dv2.Count; k++)
                                    {
                                        

                                        ds.Tables[2].DefaultView.RowFilter = " itemname='" + itemname1 + "' and VendorfK='" + store + "'";
                                        DataView dv3 = ds.Tables[2].DefaultView;

                                        double qty = Convert.ToDouble(dv3[0]["Qty"]);
                                        double rpu = Convert.ToDouble(dv2[k]["RPU"]);
                                        double discount = Convert.ToDouble(dv2[k]["DiscountAmt"]);
                                        string disper = Convert.ToString(dv2[k]["IsDiscountPercent"]);
                                        double tax = Convert.ToDouble(dv2[k]["TaxPercent"]);
                                        double extax = Convert.ToDouble(dv2[k]["ExeciseTaxPer"]);
                                        double otherharge = Convert.ToDouble(dv2[k]["OtherChargeAmt"]);
                                        double cost = 0;
                                        cost = qty * rpu;


                                        //Add RPU ArraryList
                                        arrpu.Add(rpu);

                                        if (disper.ToUpper() == "TRUE")
                                        {
                                            if (discount != 0)
                                            {
                                                cost = cost - discount;
                                            }
                                        }
                                        else
                                        {
                                            if (discount != 0)
                                            {
                                                double d = (cost / 100) * discount;
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


                                        //for (colcount = colcount; colcount < countcolumn1; colcount = colcount + 12)
                                        //{
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString(dv3[0]["qty"]);
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount].Font.Size = FontUnit.Medium;
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount].Font.Name = "Book Antiqua";


                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 1].Text = Convert.ToString(dv2[k]["RPU"]);
                                        //Add RPU Dictionary
                                        dicrpu.Add(colcount + 1, Convert.ToString(dv2[k]["RPU"]));


                                        //for (int rp = 0; rp < dv.Count; rp++)
                                        //{
                                        //    double rpu1 = Convert.ToDouble(dv[rp]["RPU"]);
                                        //    if (rpu <= rpu1)
                                        //    {
                                        //        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 1].BackColor = Color.Yellow;
                                        //    }
                                        //}
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 1].Font.Size = FontUnit.Medium;
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 1].Font.Name = "Book Antiqua";


                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 2].Text = Convert.ToString(dv2[k]["DiscountAmt"]);
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 2].Font.Size = FontUnit.Medium;
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 2].Font.Name = "Book Antiqua";

                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 3].Text = Convert.ToString(dv2[k]["TaxPercent"]);
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 3].Font.Size = FontUnit.Medium;
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 3].Font.Name = "Book Antiqua";

                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 4].Text = Convert.ToString(dv2[k]["ExeciseTaxPer"]);
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 4].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 4].Font.Size = FontUnit.Medium;
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 4].Font.Name = "Book Antiqua";


                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 5].Text = Convert.ToString(dv2[k]["EduCessPer"]);
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 5].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 5].Font.Size = FontUnit.Medium;
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 5].Font.Name = "Book Antiqua";

                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 6].Text = Convert.ToString(dv2[k]["HigherEduCessPer"]);
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 6].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 6].Font.Size = FontUnit.Medium;
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 6].Font.Name = "Book Antiqua";

                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 7].Text = Convert.ToString(dv2[k]["OtherChargeAmt"]);
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 7].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 7].Font.Size = FontUnit.Medium;
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 7].Font.Name = "Book Antiqua";

                                        //FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 14].Text = Convert.ToString(ds.Tables[0].Rows[i]["CallEduCess"]);
                                        //FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 14].HorizontalAlign = HorizontalAlign.Right;
                                        //FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 14].Font.Size = FontUnit.Medium;
                                        //FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 14].Font.Name = "Book Antiqua";

                                        //FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 15].Text = Convert.ToString(ds.Tables[0].Rows[i]["CallExTax"]);
                                        //FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 15].HorizontalAlign = HorizontalAlign.Right;
                                        //FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 15].Font.Size = FontUnit.Medium;
                                        //FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 15].Font.Name = "Book Antiqua";


                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 10].Text = Convert.ToString(Math.Round(cost, 2));
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 10].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 10].Font.Size = FontUnit.Medium;
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 10].Font.Name = "Book Antiqua";
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, colcount + 11].Tag = Convert.ToString(dv[k]["ReqCompCode"]);
                                        // }
                                        chk = true;
                                        if (chk == true)
                                        {
                                            uncheck = "1";
                                        }

                                    }
                                    colcount = colcount + 12;
                                }
                                arrpu.Sort();
                                string minvalue = arrpu[0].ToString();
                                double minvalue1 = Convert.ToDouble(minvalue);
                                foreach (KeyValuePair<int, string> keyval in dicrpu)
                                {
                                    string value = keyval.Value;
                                    double value1 = Convert.ToDouble(value);
                                    if (minvalue1 == value1)
                                    {
                                        int key1 = keyval.Key;
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, key1].BackColor = Color.Violet;


                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, key1 + 9].BackColor = Color.Violet;
                                        //int co=key1+5;
                                        //int con = FpSpread5.Sheets[0].Columns.Count;

                                        //FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, co].BackColor = Color.Violet;
                                    }
                                }


                            }

                        }
                        if (FpSpread5.Sheets[0].RowCount > 0)
                        {

                            FpSpread5.Sheets[0].RowCount++;
                            FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 0].Text = "Total Cost";
                            FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 0].BackColor = Color.LightGreen;
                            FpSpread5.Sheets[0].AddSpanCell(FpSpread5.Sheets[0].RowCount - 1, 0, 1, 4);
                            int cocnt1 = 4;
                            int cnt = 0;
                            int Columncount1 = FpSpread5.Sheets[0].ColumnCount;
                            Dictionary<int, string> dicsupplitems = new Dictionary<int, string>();
                            for (cocnt1 = cocnt1; cocnt1 < Columncount1; cocnt1 = cocnt1 + 12)
                            {
                                double precost = 0;
                                double cost = 0;
                                cnt = cocnt1 + 10;
                                string totcost = "";
                                int noitems = 0;
                                for (int t = 1; t < FpSpread5.Sheets[0].RowCount - 1; t++)
                                {

                                    Color backcolr = FpSpread5.Sheets[0].Cells[t, cnt].BackColor;
                                    string cc = backcolr.Name;
                                    if (cc.ToUpper() == "VIOLET")
                                    {
                                        noitems++;
                                        double.TryParse(Convert.ToString(FpSpread5.Sheets[0].Cells[t, cnt].Value), out precost);
                                        cost = cost + precost;

                                    }
                                    totcost = Convert.ToString(Math.Round(cost, 2));
                                }
                                string dicvalue = Convert.ToString(noitems) + "#" + totcost;
                                dicsupplitems.Add(cocnt1, dicvalue);
                                FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, cnt].Text = totcost;
                                FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, cnt].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, cnt].Font.Size = FontUnit.Medium;
                                FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, cnt].Font.Name = "Book Antiqua";
                                FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, cnt].BackColor = Color.LightSkyBlue;
                            }
                            int no = 0;
                            for (int col = 4; col < FpSpread5.Sheets[0].Columns.Count; col = col + 12)
                            {
                                string vendorname = Convert.ToString(FpSpread5.Sheets[0].ColumnHeader.Cells[0, col].Text);
                                string itemsvalue = dicsupplitems[col];
                                string[] spilt = itemsvalue.Split('#');
                                no++;
                                drow = datasuppl.NewRow();
                                drow["S.No"] = Convert.ToString(no);
                                drow["Vendor Name"] = Convert.ToString(vendorname);
                                drow["No.Of Items"] = Convert.ToString(spilt[0]);
                                drow["Rate"] = Convert.ToString(spilt[1]);
                                datasuppl.Rows.Add(drow);
                            }
                            if (datasuppl.Columns.Count > 0 && datasuppl.Rows.Count > 0)
                            {
                                drow = datasuppl.NewRow();
                                datasuppl.Rows.Add(drow);

                                double prerate = 0;
                                double rate = 0;
                                for (int r = 1; r < datasuppl.Rows.Count - 1; r++)
                                {

                                    double.TryParse(Convert.ToString(datasuppl.Rows[r][3]), out prerate);
                                    rate = rate + prerate;
                                }
                                datasuppl.Rows[datasuppl.Rows.Count - 1][0] = "Total Cost";
                                datasuppl.Rows[datasuppl.Rows.Count - 1][3] = Convert.ToString(Math.Round(rate, 2));

                                Showgrid.DataSource = datasuppl;
                                Showgrid.DataBind();
                                Showgrid.Visible = true;
                                Div_supplierDetails.Visible = true;
                                Divgridprint.Visible = true;
                                Showgrid.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                Showgrid.Rows[0].Font.Bold = true;
                                Showgrid.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                                Showgrid.Rows[0].Enabled = false;

                                for (int r = 1; r < Showgrid.Rows.Count - 1; r++)
                                {
                                    Showgrid.Rows[r].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                    Showgrid.Rows[r].Cells[2].HorizontalAlign = HorizontalAlign.Center;
                                    Showgrid.Rows[r].Cells[3].HorizontalAlign = HorizontalAlign.Center;
                                }

                                Showgrid.Rows[Showgrid.Rows.Count - 1].Cells[0].ColumnSpan = 3;
                                Showgrid.Rows[Showgrid.Rows.Count - 1].Cells[1].Visible = false;
                                Showgrid.Rows[Showgrid.Rows.Count - 1].Cells[2].Visible = false;
                                Showgrid.Rows[Showgrid.Rows.Count - 1].Cells[0].BackColor = Color.LightGreen;
                                Showgrid.Rows[Showgrid.Rows.Count - 1].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                Showgrid.Rows[Showgrid.Rows.Count - 1].Cells[3].BackColor = Color.LightSkyBlue;
                                Showgrid.Rows[Showgrid.Rows.Count - 1].Cells[3].HorizontalAlign = HorizontalAlign.Center;

                            }



                        }


                    }
                    if (cblcolumnorder.Items.Count > 0)
                    {
                        for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                        {
                            if (cblcolumnorder.Items[i].Selected == true)
                            {
                                string headername = Convert.ToString(cblcolumnorder.Items[i].ToString());
                                if (headername == "Item Name")
                                {
                                    FpSpread5.Columns[2].Visible = true;
                                }
                                else if (headername == "Item Code")
                                {
                                    FpSpread5.Columns[3].Visible = true;
                                }
                                int countcolumn = FpSpread5.Sheets[0].ColumnCount;
                                int colcount = 4;
                                for (colcount = colcount; colcount < countcolumn; colcount = colcount + 12)
                                {
                                    if (headername == "Quantity")
                                    {
                                        FpSpread5.Columns[colcount].Visible = true;
                                    }
                                    else if (headername == "Rpu")
                                    {
                                        FpSpread5.Columns[colcount + 1].Visible = true;
                                    }
                                    else if (headername == "Discount")
                                    {
                                        FpSpread5.Columns[colcount + 2].Visible = true;
                                    }
                                    else if (headername == "Tax")
                                    {
                                        FpSpread5.Columns[colcount + 3].Visible = true;
                                    }
                                    else if (headername == "Exercies Tax")
                                    {
                                        FpSpread5.Columns[colcount + 4].Visible = true;
                                    }
                                    else if (headername == "Education Cess")
                                    {
                                        FpSpread5.Columns[colcount + 5].Visible = true;
                                    }
                                    else if (headername == " Higher Education Cess")
                                    {
                                        FpSpread5.Columns[colcount + 6].Visible = true;
                                    }
                                    else if (headername == "Other Charges")
                                    {
                                        FpSpread5.Columns[colcount + 7].Visible = true;
                                    }
                                    else if (headername == "Cost")
                                    {
                                        FpSpread5.Columns[colcount + 10].Visible = true;
                                    }
                                    check = true;
                                }
                            }
                        }
                    }
                    if (check == false)
                    {
                        CheckBox_column.Checked = true;
                        LinkButtonsremove_Click(sender, e);
                        for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                        {
                            if (cblcolumnorder.Items[i].Selected == true)
                            {
                                string headername = Convert.ToString(cblcolumnorder.Items[i].ToString());

                                int countcolumn = FpSpread5.Sheets[0].ColumnCount;
                                int colcount = 4;
                                if (headername == "Item Name")
                                {
                                    FpSpread5.Columns[2].Visible = true;
                                }
                                else if (headername == "Item Code")
                                {
                                    FpSpread5.Columns[3].Visible = true;
                                }
                                for (colcount = colcount; colcount < countcolumn; colcount = colcount + 14)
                                {

                                    if (headername == "Quantity")
                                    {
                                        FpSpread5.Columns[colcount].Visible = true;
                                    }
                                    else if (headername == "Rpu")
                                    {
                                        FpSpread5.Columns[colcount + 1].Visible = true;
                                    }
                                    else if (headername == "Discount")
                                    {
                                        FpSpread5.Columns[colcount + 2].Visible = true;
                                    }
                                    else if (headername == "Tax")
                                    {
                                        FpSpread5.Columns[colcount + 3].Visible = true;
                                    }
                                    else if (headername == "Exercies Tax")
                                    {
                                        FpSpread5.Columns[colcount + 4].Visible = true;
                                    }
                                    else if (headername == "Education Cess")
                                    {
                                        FpSpread5.Columns[colcount + 5].Visible = true;
                                    }
                                    else if (headername == " Higher Education Cess")
                                    {
                                        FpSpread5.Columns[colcount + 6].Visible = true;
                                    }
                                    else if (headername == "Other Charges")
                                    {
                                        FpSpread5.Columns[colcount + 7].Visible = true;
                                    }
                                    else if (headername == "Cost")
                                    {
                                        FpSpread5.Columns[colcount + 10].Visible = true;
                                    }
                                }
                            }
                        }
                    }
                    FpSpread5.Sheets[0].PageSize = FpSpread5.Sheets[0].RowCount;
                    //FpSpread5.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    //FpSpread5.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread5.Sheets[0].FrozenRowCount = 0;
                    FpSpread5.Visible = true;
                    spreaddiv1.Visible = true;
                    btn_purchasereq.Visible = true;
                    rptprint.Visible = true;
                    lbl_baseerror.Visible = false;
                    pheaderfilter.Visible = true;
                    //tborder.Visible = true;
                    rowcount = FpSpread5.Rows.Count;
                    Columncount = FpSpread5.Sheets[0].ColumnCount;

                }
                else
                {
                    FpSpread5.Visible = false;
                    spreaddiv1.Visible = false;
                    lbl_baseerror.Visible = true;
                    btn_purchasereq.Visible = false;
                    rptprint.Visible = false;
                    Showgrid.Visible = false;
                    Div_supplierDetails.Visible = false;
                    Divgridprint.Visible = false;
                    lbl_baseerror.Text = "No Record Founds";
                }
            }
            if (uncheck.Trim() != "1")
            {
                FpSpread5.Visible = false;
                spreaddiv1.Visible = false;
                lblalerterr.Visible = true;
                alertpopwindow.Visible = true;
                Showgrid.Visible = false;
                Div_supplierDetails.Visible = false;
                Divgridprint.Visible = false;
                lblalerterr.Text = "Please Select Any One Item";
            }


        }
        catch { }

    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    protected void ddl_reqcompcode_bind()
    {
        try
        {
            string q1 = "select distinct reqcompcode from it_vendorreq where PurchaseStatus<>'1' and  ISNULL(itemorlibrary,'')='' order by ReqCompCode Asc";

            ds.Clear();
            ds = d2.select_method_wo_parameter(q1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_reqcompcode.DataSource = ds;
                ddl_reqcompcode.DataTextField = "ReqCompCode";
                ddl_reqcompcode.DataBind();
                ddl_reqcompcode.Items.Insert(0, "Select");
            }
        }
        catch { }
    }
    protected void ddl_reqcompcode_selectedIndexchange(object sender, EventArgs e)
    {
        try
        {
            string venquery = "";
            if (Rad_item.Checked)
                venquery = "select distinct vq.Vendorfk,vm.VendorCompName from CO_VendorMaster vm,IT_VendorReq vq where vm.VendorPK=vq.VendorFK and ISNULL(vq.itemorlibrary,'')='' and isnull(LibraryFlag,'')='' and vq.ReqCompCode in('" + ddl_reqcompcode.SelectedItem.Text + "') ";
            if (Rad_book.Checked)
                venquery = "select distinct vq.Vendorfk,vm.VendorCompName from CO_VendorMaster vm,IT_VendorReq vq where vm.VendorPK=vq.VendorFK and vq.itemorlibrary='1' and LibraryFlag='1'  and vq.ReqCompCode in('" + ddl_reqcompcode.SelectedItem.Text + "') ";
            ds = d2.select_method_wo_parameter(venquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_venname.DataSource = ds;
                cbl_venname.DataTextField = "VendorCompName";
                cbl_venname.DataValueField = "Vendorfk";
                cbl_venname.DataBind();
                if (cbl_venname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_venname.Items.Count; i++)
                    {
                        cbl_venname.Items[i].Selected = true;
                    }
                    txt_venname.Text = "Supplier Name(" + cbl_venname.Items.Count + ")";
                }
            }
        }
        catch
        { }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread5, reportname);
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
            string degreedetails = "Supplier Quotation Compare";
            string pagename = "vendor_quotation_compare.aspx";
            Printcontrol.loadspreaddetails(FpSpread5, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }

    }
    protected void btn_purchase_request_Click(object sender, EventArgs e)
    {
        try
        {
            bool insert = false;
            int spreaditemchk = 0;
            int colchk = 0;
            int cocnt = 0;
            FpSpread5.SaveChanges();
            if (FpSpread5.Sheets[0].ColumnHeader.Rows.Count > 0)
            {
                if (FpSpread5.Sheets[0].RowCount > 0)
                {

                    for (int col = 4; col < FpSpread5.Sheets[0].Columns.Count; col = col + 12)
                    {
                        //int checkval1 = Convert.ToInt32(FpSpread5.Sheets[0].ColumnHeader.Cells[1, col].Value);
                        string d = Convert.ToString(FpSpread5.Sheets[0].ColumnHeader.Cells[1, col].Text);
                        string checkval2 = Convert.ToString(FpSpread5.Sheets[0].ColumnHeader.Cells[0, col].Text);
                        int checkval1 = Convert.ToInt32(FpSpread5.Sheets[0].Cells[0, col].Value);
                        if (checkval1 == 1)
                        {
                            cocnt = col;
                            colchk++;

                            for (int row = 1; row < FpSpread5.Sheets[0].RowCount; row++)
                            {
                                int checkval = Convert.ToInt32(FpSpread5.Sheets[0].Cells[row, 1].Value);
                                if (checkval == 1)
                                {
                                    spreaditemchk = spreaditemchk + 1;
                                }
                            }
                        }
                    }
                    if (colchk == 1)
                    {
                        if (spreaditemchk > 0)
                        {
                            if (FpSpread5.Sheets[0].RowCount > 0)
                            {
                                FpSpread5.SaveChanges();
                                string purchasevendorfk = "";
                                string recomcode = "";
                                string ItemorBook = "";
                                string ItemorBookid = "";
                                string ItemorBookno = "";
                                purchasevendorfk = Convert.ToString(FpSpread5.Sheets[0].ColumnHeader.Cells[0, cocnt].Tag);
                                recomcode = Convert.ToString(FpSpread5.Sheets[0].ColumnHeader.Cells[1, cocnt + 11].Tag);
                                if (Rad_item.Checked)
                                    ItemorBook = "1";
                                if (Rad_book.Checked)
                                    ItemorBook = "2";
                                for (int row = 1; row < FpSpread5.Sheets[0].RowCount; row++)
                                {
                                    int checkval = Convert.ToInt32(FpSpread5.Sheets[0].Cells[row, 1].Value);
                                    if (checkval == 1)
                                    {
                                        ItemorBookid = Convert.ToString(FpSpread5.Sheets[0].Cells[row, 3].Tag);
                                        if (ItemorBookno == "")
                                            ItemorBookno = ItemorBookid;
                                        else
                                            ItemorBookno = ItemorBookno + "','" + ItemorBookid;
                                        //purchasevendorfk = Convert.ToString(FpSpread5.Sheets[0].Cells[row, 2].Tag);
                                        //recomcode = Convert.ToString(FpSpread5.Sheets[0].Cells[row, 17].Tag);
                                        //string g = Session["vendorpk"].ToString();
                                        //string g1 = Session["reqcomparecode"].ToString();


                                    }
                                }
                                Response.Redirect("inv_purchase.aspx?@@barath$$=" + (purchasevendorfk) + "s" + (recomcode) + "s" + "1" + "s" + Session["vendorpk"] + "s" + Session["reqcomparecode"] + "s" + ItemorBook + "s" + ItemorBookno);

                            }
                            //string get_value = (Request.QueryString["app"].ToString());
                            //Response.Redirect("IndReport.aspx?app=" + Encrypt(app_no_stud));
                        }
                        else
                        {
                            lblalerterr.Visible = true;
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Please select Atleaset one Item";
                        }

                    }
                    else if (colchk > 1)
                    {
                        lblalerterr.Visible = true;
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "You can't select more then one vendor";
                    }
                    else if (colchk < 1)
                    {
                        lblalerterr.Visible = true;
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please select any one vendor";
                    }

                }
            }
            if (insert == true)
            {
                lblalerterr.Visible = true;
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Saved Successfully";
            }
        }
        catch
        { }
    }

    protected void LinkButtonsremove_Click(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column.Checked == true)
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = true;
                }
            }
            else
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                }
            }
        }
        catch { }
    }
    protected void CheckBox_column_CheckedChanged(object sender, EventArgs e)
    { }
    protected void cblcolumnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void FpSpread5_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actcol = FpSpread5.Sheets[0].ActiveColumn.ToString();
            if (actcol != "")
            {
                if (FpSpread5.Sheets[0].RowCount > 0)
                {
                    int actcl = Convert.ToInt32(actcol);
                    if (actcl != 1)
                    {
                        for (int col = 4; col < FpSpread5.Sheets[0].Columns.Count; col = col + 12)
                        {
                            if (actcl != col)
                            {
                                int checkval = Convert.ToInt32(FpSpread5.Sheets[0].Cells[0, col].Value);
                                if (checkval == 1)
                                {
                                    FpSpread5.Sheets[0].Cells[0, col].Value = false;

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

    //column order old method
    //protected void cblcolumnorder_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //    try
    //    {
    //        CheckBox_column.Checked = false;
    //        string value = "";
    //        int index;
    //        cblcolumnorder.Items[0].Selected = true;
    //        // cblcolumnorder.Items[0].Enabled = false;
    //        value = string.Empty;
    //        string result = Request.Form["__EVENTTARGET"];
    //        string[] checkedBox = result.Split('$');
    //        index = int.Parse(checkedBox[checkedBox.Length - 1]);
    //        string sindex = Convert.ToString(index);
    //        if (cblcolumnorder.Items[index].Selected)
    //        {
    //            if (!Itemindex.Contains(sindex))
    //            {
    //                //if (tborder.Text == "")
    //                //{
    //                //    ItemList.Add("Company Code");
    //                //}
    //                //ItemList.Add("Admission No");
    //                //ItemList.Add("Name");
    //                ItemList.Add(cblcolumnorder.Items[index].Text.ToString());
    //                Itemindex.Add(sindex);
    //            }
    //        }
    //        else
    //        {
    //            ItemList.Remove(cblcolumnorder.Items[index].Text.ToString());
    //            Itemindex.Remove(sindex);
    //        }
    //        for (int i = 0; i < cblcolumnorder.Items.Count; i++)
    //        {
    //            if (cblcolumnorder.Items[i].Selected == false)
    //            {
    //                sindex = Convert.ToString(i);
    //                ItemList.Remove(cblcolumnorder.Items[i].Text.ToString());
    //                Itemindex.Remove(sindex);
    //            }
    //        }

    //        lnk_columnorder.Visible = true;
    //        tborder.Visible = false;
    //        tborder.Text = "";
    //        string colname12 = "";
    //        for (int i = 0; i < ItemList.Count; i++)
    //        {
    //            if (colname12 == "")
    //            {
    //                colname12 = ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
    //            }
    //            else
    //            {
    //                colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
    //            }
    //            tborder.Text = tborder.Text + ItemList[i].ToString();

    //            tborder.Text = tborder.Text + "(" + (i + 1).ToString() + ")";

    //        }
    //        tborder.Text = colname12;
    //        if (ItemList.Count == 14)
    //        {
    //            CheckBox_column.Checked = true;
    //        }
    //        if (ItemList.Count == 0)
    //        {
    //            tborder.Visible = false;
    //            lnk_columnorder.Visible = false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}
    //protected void LinkButtonsremove_Click(object sender, EventArgs e)
    //{
    //    cblcolumnorder.ClearSelection();
    //    CheckBox_column.Checked = false;
    //    lnk_columnorder.Visible = false;
    //    //cblcolumnorder.Items[0].Selected = true;
    //    ItemList.Clear();
    //    Itemindex.Clear();
    //    tborder.Text = "";
    //    tborder.Visible = false;
    //}
    //protected void CheckBox_column_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (CheckBox_column.Checked == true)
    //        {
    //            tborder.Text = "";
    //            ItemList.Clear();
    //            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
    //            {
    //                string si = Convert.ToString(i);
    //                cblcolumnorder.Items[i].Selected = true;
    //                lnk_columnorder.Visible = true;
    //                ItemList.Add(cblcolumnorder.Items[i].Text.ToString());
    //                Itemindex.Add(si);
    //            }
    //            lnk_columnorder.Visible = true;
    //            tborder.Visible = true;
    //            tborder.Text = "";
    //            int j = 0;
    //            string colname12 = "";
    //            for (int i = 0; i < ItemList.Count; i++)
    //            {
    //                j = j + 1;
    //                if (colname12 == "")
    //                {
    //                    colname12 = ItemList[i].ToString() + "(" + (j).ToString() + ")";

    //                }
    //                else
    //                {
    //                    colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (j).ToString() + ")";
    //                }
    //            }
    //            tborder.Text = colname12;
    //        }
    //        else
    //        {
    //            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
    //            {
    //                cblcolumnorder.Items[i].Selected = false;
    //                lnk_columnorder.Visible = false;
    //                ItemList.Clear();
    //                Itemindex.Clear();
    //                //cblcolumnorder.Items[0].Selected = true;
    //            }

    //            tborder.Text = "";
    //            tborder.Visible = false;

    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}


    //Added by saranyadevi19.1.2018



    protected void ddl_reqcompcode_bind1()
    {
        try
        {
            string q1 = "select distinct reqcompcode from it_vendorreq where PurchaseStatus<>'1' and  itemorlibrary='1' order by ReqCompCode Asc";

            ds.Clear();
            ds = d2.select_method_wo_parameter(q1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_reqcompcode.DataSource = ds;
                ddl_reqcompcode.DataTextField = "ReqCompCode";
                ddl_reqcompcode.DataBind();
                ddl_reqcompcode.Items.Insert(0, "Select");
            }
        }
        catch { }
    }

    protected void Rad_item_Click(object sender, EventArgs e)
    {

        ddl_reqcompcode_bind();
        FpSpread1.Visible = false;
        spreaddiv.Visible = false;
        lbl_baseerror.Visible = false;
        btn_purchasereq.Visible = false;
        btn_selectitem.Visible = false;
        btn_purchasereq.Visible = false;
        rptprint.Visible = false;
        pheaderfilter.Visible = false;
        pcolumnorder.Visible = false;
        FpSpread5.Visible = false;
        Showgrid.Visible = false;
        Div_supplierDetails.Visible = false;
        Divgridprint.Visible = false;
        spreaddiv1.Visible = false;

    }
    protected void Rad_book_Click(object sender, EventArgs e)
    {
        ddl_reqcompcode_bind1();
        FpSpread1.Visible = false;
        spreaddiv.Visible = false;
        lbl_baseerror.Visible = false;
        btn_purchasereq.Visible = false;
        btn_selectitem.Visible = false;
        btn_purchasereq.Visible = false;
        rptprint.Visible = false;
        pheaderfilter.Visible = false;
        pcolumnorder.Visible = false;
        FpSpread5.Visible = false;
        spreaddiv1.Visible = false;
        Showgrid.Visible = false;
        Div_supplierDetails.Visible = false;
        Divgridprint.Visible = false;
    }






    #region Print PDF

    protected void btnprintmaster_Click1(object sender, EventArgs e)
    {
        try
        {

            string ss = null;
            string degreedetails = "Supplier Quotation Comparative";
            string pagename = "vendor_quotation_compare.aspx";
            Printcontrol1.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
            Printcontrol1.Visible = true;

        }
        catch (Exception ex)
        {

        }
    }

    #endregion Print PDF

    #region Print Excel

    protected void btnExcel_Click1(object sender, EventArgs e)
    {
        try
        {

            Printcontrol1.Visible = false;
            string reportname = txtexcelname1.Text;
            if (Convert.ToString(reportname).Trim() != "")
            {
                d2.printexcelreportgrid(Showgrid, reportname);

                lbl_norec1.Visible = false;
            }
            else
            {
                lbl_norec1.Text = "Please Enter Your Report Name";
                lbl_norec1.Visible = true;
                txtexcelname1.Focus();
            }

        }
        catch (Exception ex)
        {

        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }


    #endregion Print PDF

}