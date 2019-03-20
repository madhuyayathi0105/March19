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


public partial class HM_Hostelsupplier_report : System.Web.UI.Page
{
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string selectQuery = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

    string build = "";
    int commcount;
    int i;
    int cout;


    DAccess2 queryObject = new DAccess2();
    DAccess2 da = new DAccess2();


    protected void Page_Load(object sender, EventArgs e)
    {
        //****************************************************//
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("hostelindex"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/HostelMod/hostelindex.aspx");
                return;
            }
        }
        //****************************************************//
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        Cal_fromdate.EndDate = DateTime.Now;
        cal_todate.EndDate = DateTime.Now;
        if (!IsPostBack)
        {

            //bindclg();
            bindhostelname();
            bindsuppliername();
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Attributes.Add("readonly", "readonly");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");

        }
    }

    protected void lb3_Click(object sender, EventArgs e)
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
    protected void ddl_college_OnSelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void cb_hostel_CheckedChanged(object sender, EventArgs e)
    {
        cout = 0;
        txt_hostelname.Text = "--Select--";

        if (cb_hostel.Checked == true)
        {
            cout++;
            for (i = 0; i < cbl_hostel.Items.Count; i++)
            {
                cbl_hostel.Items[i].Selected = true;
            }
            txt_hostelname.Text = "Mess Name(" + (cbl_hostel.Items.Count) + ")";
        }
        else
        {
            for (i = 0; i < cbl_hostel.Items.Count; i++)
            {
                cbl_hostel.Items[i].Selected = false;
            }
        }


    }
    protected void cbl_hostel_SelectedIndexChanged(object sender, EventArgs e)
    {
        cb_hostel.Checked = false;
        commcount = 0;
        txt_hostelname.Text = "--Select--";
        for (i = 0; i < cbl_hostel.Items.Count; i++)
        {
            if (cbl_hostel.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_hostel.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_hostel.Items.Count)
            {
                cb_hostel.Checked = true;
            }
            txt_hostelname.Text = "Mess Name(" + commcount.ToString() + ")";
        }


    }
    protected void cb_supplier_CheckedChanged(object sender, EventArgs e)
    {
        cout = 0;
        txt_supplier.Text = "--Select--";

        if (cb_supplier.Checked == true)
        {
            cout++;
            for (i = 0; i < cbl_supplier.Items.Count; i++)
            {
                cbl_supplier.Items[i].Selected = true;
            }
            txt_supplier.Text = "Supplier Name(" + (cbl_supplier.Items.Count) + ")";
        }
        else
        {
            for (i = 0; i < cbl_supplier.Items.Count; i++)
            {
                cbl_supplier.Items[i].Selected = false;
            }
        }
    }
    protected void cbl_supplier_SelectedIndexChanged(object sender, EventArgs e)
    {

        cb_supplier.Checked = false;
        commcount = 0;
        txt_supplier.Text = "--Select--";
        for (i = 0; i < cbl_supplier.Items.Count; i++)
        {
            if (cbl_supplier.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_supplier.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_supplier.Items.Count)
            {
                cb_supplier.Checked = true;
            }
            txt_supplier.Text = "Supplier Name(" + commcount.ToString() + ")";
        }


    }

    //public void bindclg()
    //{
    //    try
    //    {
    //        ds.Clear();
    //        ddl_college.Items.Clear();
    //        selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
    //        ds = d2.select_method_wo_parameter(selectQuery, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            ddl_college.DataSource = ds;
    //            ddl_college.DataTextField = "collname";
    //            ddl_college.DataValueField = "college_code";
    //            ddl_college.DataBind();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    public void bindhostelname()
    {
        try
        {
            cbl_hostel.Items.Clear();
            //ds = queryObject.BindHostel(collegecode1);
            ds.Clear();
            //ds = d2.Bindmess_inv(collegecode1);
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostel.DataSource = ds;
                cbl_hostel.DataTextField = "MessName";
                cbl_hostel.DataValueField = "MessMasterPK";
                cbl_hostel.DataBind();

                if (cbl_hostel.Items.Count > 0)
                {
                    for (i = 0; i < cbl_hostel.Items.Count; i++)
                    {
                        cbl_hostel.Items[i].Selected = true;
                    }
                    txt_hostelname.Text = "Mess Name(" + cbl_hostel.Items.Count + ")";
                    cb_hostel.Checked = true;
                }
            }
            else
            {
                txt_hostelname.Text = "--Select--";
                cb_hostel.Checked = false;
            }
        }
        catch
        {
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
                    // imgdiv2.Visible = true;
                    lbl_alert.Text = "Enter FromDate less than or equal to the ToDate";
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
                    // imgdiv2.Visible = true;
                    lbl_alert.Text = "Enter FromDate less than or equal to the ToDate";
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


    public void bindsuppliername()
    {
        try
        {
            cbl_supplier.Items.Clear();

            ds = queryObject.BindVendorNamevendorpk_inv();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_supplier.DataSource = ds;
                cbl_supplier.DataTextField = "VendorCompName";
                cbl_supplier.DataValueField = "VendorPK";
                cbl_supplier.DataBind();

                if (cbl_supplier.Items.Count > 0)
                {
                    for (i = 0; i < cbl_supplier.Items.Count; i++)
                    {
                        cbl_supplier.Items[i].Selected = true;
                    }
                    txt_supplier.Text = "Supplier Name(" + cbl_supplier.Items.Count + ")";
                    cb_supplier.Checked = true;
                }
            }
            else
            {
                txt_supplier.Text = "--Select--";
                cb_supplier.Checked = false;
            }
        }
        catch
        {
        }
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    public void cblcolumnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox_column.Checked = false;
            string value = "";
            int index;
            cblcolumnorder.Items[0].Selected = true;
            // cblcolumnorder.Items[0].Enabled = false;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    //if (tborder.Text == "")
                    //{
                    //    ItemList.Add("Company Code");
                    //}
                    //ItemList.Add("Admission No");
                    //ItemList.Add("Name");
                    ItemList.Add(cblcolumnorder.Items[index].Value.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cblcolumnorder.Items[index].Value.ToString());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                //if (i == 0 || i == 1 || i == 2)
                //{
                //    cblcolumnorder.Items[0].Selected = true;
                //    cblcolumnorder.Items[1].Selected = true;
                //    cblcolumnorder.Items[2].Selected = true;
                //}
                if (cblcolumnorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cblcolumnorder.Items[i].Value.ToString());
                    Itemindex.Remove(sindex);

                }
            }

            lnk_columnorder.Visible = true;
            tborder.Visible = true;
            tborder.Text = "";
            string colname12 = "";
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (colname12 == "")
                {
                    colname12 = ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";

                }
                else
                {
                    colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
                }
                //tborder.Text = tborder.Text + ItemList[i].ToString();

                //tborder.Text = tborder.Text + "(" + (i + 1).ToString() + ")  ";

            }
            tborder.Text = colname12;
            if (ItemList.Count == 14)
            {
                CheckBox_column.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                tborder.Visible = false;
                lnk_columnorder.Visible = false;
            }

            //  Button2.Focus();
        }
        catch (Exception ex)
        {

        }
    }

    public void CheckBox_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column.Checked == true)
            {
                tborder.Text = "";
                ItemList.Clear();
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                    ItemList.Add(cblcolumnorder.Items[i].Value.ToString());
                    Itemindex.Add(si);
                }
                lnk_columnorder.Visible = true;
                tborder.Visible = true;
                tborder.Text = "";
                int j = 0;
                string colname12 = "";
                for (int i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    if (colname12 == "")
                    {
                        colname12 = ItemList[i].ToString() + "(" + (j).ToString() + ")";

                    }
                    else
                    {
                        colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (j).ToString() + ")";
                    }
                    // tborder.Text = tborder.Text + ItemList[i].ToString();



                }
                tborder.Text = colname12;

            }
            else
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                    lnk_columnorder.Visible = false;
                    ItemList.Clear();
                    Itemindex.Clear();
                    //cblcolumnorder.Items[0].Selected = true;
                }

                tborder.Text = "";
                tborder.Visible = false;

            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {

        }
    }
    public void LinkButtonsremove_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder.ClearSelection();
            CheckBox_column.Checked = false;
            lnk_columnorder.Visible = false;
            //cblcolumnorder.Items[0].Selected = true;
            ItemList.Clear();
            Itemindex.Clear();
            tborder.Text = "";
            tborder.Visible = false;
            //Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = "";
            string colno = "";
            string menuvalue1 = "";
            string item = "";
            //for (int i = 0; i < cbl_hostel.Items.Count; i++)
            //{
            //    if (cbl_hostel.Items[i].Selected == true)
            //    {
            //        if (menuvalue1 == "")
            //        {
            //            menuvalue1 = "" + cbl_hostel.Items[i].Value.ToString() + "";
            //        }
            //        else
            //        {
            //            menuvalue1 = menuvalue1 + "'" + "," + "'" + cbl_hostel.Items[i].Value.ToString() + "";
            //        }
            //    }
            //}
            for (int i = 0; i < cbl_supplier.Items.Count; i++)
            {
                if (cbl_supplier.Items[i].Selected == true)
                {
                    if (item == "")
                    {
                        item = "" + cbl_supplier.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        item = item + "'" + "," + "'" + cbl_supplier.Items[i].Value.ToString() + "";
                    }
                }
            }
            //if (ItemList.Count == 0)
            //{
            //    ItemList.Add("Hostel_Name");
            //    ItemList.Add("Session_Name");
            //    ItemList.Add("MenuName");
            //    ItemList.Add("Item_Code");
            //    ItemList.Add("item_name");
            //    ItemList.Add("RPU");
            //    ItemList.Add("Consumption_Date");
            //    ItemList.Add("qut");
            //    ItemList.Add("value");
            //    ItemList.Add("Total_Present");
            //}

            Hashtable columnhash = new Hashtable();
            columnhash.Clear();

            int colinc = 0;
            columnhash.Add("vendor_name", "Supplier Name");
            columnhash.Add("GI_Date", "Supplied Date");
            columnhash.Add("Order_Date", "Purchase Order Date");
            columnhash.Add("Order_Code", "Purchase Order Code");
            columnhash.Add("itemheader_name", "Item Header Name");
            columnhash.Add("Item_Code", "Item Code");
            columnhash.Add("Item_Name", "Item Name");
            columnhash.Add("Item_unit", "Measure");
            columnhash.Add("order_qty", "Ordered QTY");
            columnhash.Add("OrderedAmt", "Ordered QTY Amount");
            columnhash.Add("Request_qty", "Requested QTY");
            columnhash.Add("Request_Amt", "Requested QTY Amount");
            columnhash.Add("RejQty", "Rejected QTY");
            columnhash.Add("MasterValue", "Reject Reason");


            if (ItemList.Count == 0)
            {
                ItemList.Add("Order_Date");
                ItemList.Add("Order_Code");
                ItemList.Add("GI_Date");
                ItemList.Add("itemheader_name");
                ItemList.Add("Item_Code");
                ItemList.Add("Item_Name");
                //ItemList.Add("Consumption_Date");
                //ItemList.Add("qut");
                //ItemList.Add("value");
                //ItemList.Add("Total_Present");
            }

            string getday = "";
            string gettoday = "";
            string from = "";
            string to = "";
            from = Convert.ToString(txt_fromdate.Text);
            string[] splitdate = from.Split('-');
            splitdate = splitdate[0].Split('/');
            DateTime dt = new DateTime();
            if (splitdate.Length > 0)
            {
                dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            }
            getday = dt.ToString("MM/dd/yyyy");

            to = Convert.ToString(txt_todate.Text);
            string[] splitdate1 = to.Split('-');
            splitdate1 = splitdate1[0].Split('/');
            DateTime dt1 = new DateTime();
            if (splitdate1.Length > 0)
            {
                dt1 = Convert.ToDateTime(splitdate1[1] + "/" + splitdate1[0] + "/" + splitdate1[2]);
            }
            gettoday = dt1.ToString("MM/dd/yyyy");

            if (item != "")
            {

                string selectquery = "";
                selectquery = "SELECT distinct P.OrderCode as Order_Code,CONVERT(varchar(10), OrderDate,103) as Order_Date,G.GoodsInwardCode as GI_Code,CONVERT(varchar(10), GoodsInwardDate,103) as GI_Date,ItemHeaderName as itemheader_name,m.itemcode as Item_Code,M.ItemName as Item_Name,M.ItemUnit as Item_unit,(AppQty-ISNULL(RejQty,0)) as order_qty,((AppQty-ISNULL(RejQty,0))*RPU) as OrderedAmt,AppQty as Request_qty,(AppQty * rpu) as Request_Amt,InwardQty,(InwardQty*RPU) InwardAmt,RejQty,v.VendorCompName as vendor_name,(select MasterValue from CO_MasterValues c where c.MasterCode=i.Reject_reason)as MasterValue  FROM IT_PurchaseOrder P,IT_PurchaseOrderDetail I,IT_GoodsInward G,IM_ItemMaster M,CO_VendorMaster V WHERE P.PurchaseOrderPK = I.PurchaseOrderFK and I.ItemFK=M.ItemPK and I.ItemFK=G.ItemFK and p.VendorFK =v.VendorPK AND p.VendorFK in ('" + item + "') AND OrderDate between  '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and p.PurchaseOrderPK=g.PurchaseOrderFK ";

                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    pcolumnorder.Visible = true;
                    Fpspread1.Sheets[0].RowHeader.Visible = false;
                    //Fpspread1.Sheets[0].ColumnCount = 11;
                    Fpspread1.CommandBar.Visible = false;
                    Fpspread1.Sheets[0].RowCount = 0;
                    Fpspread1.SheetCorner.ColumnCount = 0;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread1.Sheets[0].RowHeader.Visible = false;
                    Fpspread1.Sheets[0].ColumnCount = ItemList.Count + 1;
                    Fpspread1.Sheets[0].RowCount = ds.Tables[0].Rows.Count;
                    Fpspread1.Sheets[0].AutoPostBack = true;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        colno = Convert.ToString(ds.Tables[0].Columns[j]);
                        if (ItemList.Contains(Convert.ToString(colno)))
                        {
                            int index = ItemList.IndexOf(Convert.ToString(colno));
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Text = Convert.ToString(columnhash[colno]);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Fpspread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                        Fpspread1.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;

                        for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (ItemList.Contains(Convert.ToString(ds.Tables[0].Columns[j].ToString())))
                            {
                                int index = ItemList.IndexOf(Convert.ToString(ds.Tables[0].Columns[j].ToString()));
                                Fpspread1.Sheets[0].Columns[index + 1].Width = 150;
                                Fpspread1.Sheets[0].Columns[index + 1].Locked = true;
                                Fpspread1.Sheets[0].Cells[i, index + 1].CellType = txt;
                                Fpspread1.Sheets[0].Cells[i, index + 1].Text = ds.Tables[0].Rows[i][j].ToString();
                                Fpspread1.Sheets[0].Cells[i, index + 1].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[i, index + 1].Font.Size = FontUnit.Medium;
                                if (ds.Tables[0].Columns[j].ToString() == "order_qty")
                                {
                                    Fpspread1.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Right;
                                }
                                if (ds.Tables[0].Columns[j].ToString() == "OrderedAmt")
                                {
                                    Fpspread1.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Right;
                                }
                                if (ds.Tables[0].Columns[j].ToString() == "Request_qty")
                                {
                                    Fpspread1.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Right;
                                }
                                if (ds.Tables[0].Columns[j].ToString() == "Request_Amt")
                                {
                                    Fpspread1.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Right;
                                }
                                if (ds.Tables[0].Columns[j].ToString() == "Rej_qty")
                                {
                                    Fpspread1.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Right;
                                }
                                Fpspread1.Sheets[0].SetColumnMerge(index + 1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                Fpspread1.Sheets[0].Cells[i, index + 1].VerticalAlign = VerticalAlign.Middle;
                            }
                        }
                    }

                    //Fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    ////Fpspread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    ////Fpspread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);

                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    rptprint.Visible = true;
                    Fpspread1.Visible = true;
                    div1.Visible = true;
                    lbl_error.Visible = false;
                    pheaderfilter.Visible = true;
                    pcolumnorder.Visible = true;

                }
                else
                {
                    rptprint.Visible = false;
                    //imgdiv2.Visible = true;
                    // lbl_alert.Text = "No records found";
                    lbl_error.Visible = true;
                    lbl_error.Text = "No records found";
                    pheaderfilter.Visible = false;
                    pcolumnorder.Visible = false;
                    div1.Visible = false;
                    Fpspread1.Visible = false;
                }
            }
            else
            {
                rptprint.Visible = false;
                // imgdiv2.Visible = true;
                //lbl_alert.Text = "No records found";
                lbl_error.Visible = true;
                lbl_error.Text = "Please Select Any One Supplier Name";
                pheaderfilter.Visible = false;
                pcolumnorder.Visible = false;
                div1.Visible = false;
                Fpspread1.Visible = false;
            }
        }
        catch (Exception ex)
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
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Supplier History Report";
            string pagename = "HostelReport.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }

    }
}