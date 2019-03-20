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

public partial class Transfer_details : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DateTime dt = new DateTime();
    DateTime dt1 = new DateTime();
    string q1 = "";
    int insert = 0;
    int i = 0;
    int k = 0;
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
            lblvalidation1.Text = "";
            if (!IsPostBack)
            {
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
                lbl_hostelname.Visible = true;
                upp1.Visible = true;
                bind_mess();
                bind_store();
                bind_deptartment();

                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.Visible = false;
            }
            CalendarExtender2.EndDate = DateTime.Now;
            CalendarExtender1.EndDate = DateTime.Now;

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
    protected void cb_mess_CheckedChange(object sender, EventArgs e)
    {
        int cout = 0;
        txt_hosname.Text = "--Select--";
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
    }
    protected void cbl_mess_SelectedIndexChange(object sender, EventArgs e)
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
            txt_basestore.Text = "Store Name(" + (cbl_mainstore.Items.Count) + ")";
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
            txt_basestore.Text = "Store Name(" + commcount.ToString() + ")";
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
                        txt_degree.Text = "Degree Name(" + (cbl_degree.Items.Count) + ")";
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
                txt_degree.Text = "Degree Name(" + seatcount.ToString() + ")";
                cb_degree.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_degree.Text = "--Select--";
            }
            else
            {
                txt_degree.Text = "Degree Name(" + seatcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void bind_deptartment()
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

                    txt_degree.Text = "Department Name(" + cbl_degree.Items.Count + ")";
                }
            }
        }
        catch
        { }
    }
    public void bind_mess()
    {
        try
        {
            ds.Clear();
            cbl_hos.Items.Clear();
            //ds = d2.Bindmess_inv(collegecode1);
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
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
        }
        catch
        {
        }
    }
    public void bind_store()
    {
        try
        {
            ds.Clear();
            cbl_mainstore.Items.Clear();
            ds = d2.BindStore_inv(collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_mainstore.DataSource = ds;
                cbl_mainstore.DataTextField = "StoreName";
                cbl_mainstore.DataValueField = "StorePK";
                cbl_mainstore.DataBind();
                if (cbl_mainstore.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_mainstore.Items.Count; i++)
                    {
                        cbl_mainstore.Items[i].Selected = true;
                    }

                    txt_basestore.Text = "Store Name(" + cbl_mainstore.Items.Count + ")";
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
                    alertpopwindow.Visible = true;
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Enter FromDate less than or equal to the ToDate";
                    txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //spreaddiv1.Visible = false;
                    FpSpread1.Visible = false;
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
                    alertpopwindow.Visible = true;
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Enter ToDate greater than or equal to the FromDate";
                    txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //spreaddiv1.Visible = false;
                    FpSpread1.Visible = false;
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
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Stock Transfer Report";
            string pagename = "Transfor.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }

    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            string firstdate = Convert.ToString(txt_fromdate.Text);
            string seconddate = Convert.ToString(txt_todate.Text);
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            string[] split1 = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
            string Messcode = ""; string storecode = ""; string deptcode = "";
            if (ddl_option.SelectedIndex == 0)
            {
                for (int i = 0; i < cbl_hos.Items.Count; i++)
                {
                    if (cbl_hos.Items[i].Selected == true)
                    {
                        if (Messcode == "")
                        {
                            Messcode = "" + cbl_hos.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            Messcode = Messcode + "'" + "," + "'" + cbl_hos.Items[i].Value.ToString() + "";
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
            string header = "";
            if (ddl_option.SelectedIndex == 0)
            {
                q1 = "";
                q1 = "select convert(varchar(10), TrasnferDate,103)as TrasnferDate,itemcode,ItemName,TransferFrom, TrasferTo, m.MessName as toname,TransferType,TransferQty,TransferRpu,i.subheader_code,i.ItemPK from IT_TransferItem t,HM_MessMaster m,IM_ItemMaster i where i.ItemPK=t.ItemFK and  m.MessMasterPK=TrasferTo and TrasferTo in ('" + Messcode + "') and TrasnferDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'";
                //q1 = "select convert(varchar(10), TrasnferDate,103)as TrasnferDate,itemcode,ItemName,TransferFrom, TrasferTo, m.MessName as toname,TransferType,t.TransferQty,sd.IssuedRPU from IT_TransferItem t,HM_MessMaster m,IM_ItemMaster i,IT_StockDeptDetail sd where i.ItemPK=t.ItemFK and  m.MessMasterPK=TrasferTo and  i.ItemPK =sd.ItemFK and t.ItemFK=sd.ItemFK and m.MessMasterPK=sd.DeptFK and TrasferTo in ('" + Messcode + "') and TrasnferDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'";
            }
            else if (ddl_option.SelectedIndex == 1)
            {
                q1 = "";
                q1 = "select convert(varchar(10), TrasnferDate,103)as TrasnferDate,itemcode,ItemName,TransferFrom,TrasferTo, s.StoreName as toname, TransferType,TransferQty,TransferRpu,i.subheader_code,i.ItemPK  from IT_TransferItem t,IM_StoreMaster s,IM_ItemMaster i where i.ItemPK=t.ItemFK and  s.StorePK=TrasferTo and TrasferTo in ('" + storecode + "') and TrasnferDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'";
            }
            else if (ddl_option.SelectedIndex == 2)
            {
                q1 = "";
                q1 = "select convert(varchar(10), TrasnferDate,103)as TrasnferDate,itemcode,ItemName,TransferFrom,TrasferTo,Dept_Name as toname, TransferType,TransferQty,TransferRpu,i.subheader_code,i.ItemPK from IT_TransferItem t,Department d,IM_ItemMaster i where i.ItemPK=t.ItemFK and  d.Dept_Code=TrasferTo and TrasferTo in ('" + deptcode + "') and TrasnferDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'";
            }
            ds.Clear();
            q1 = q1 + " select distinct TransferType from IT_TransferItem order by TransferType";
            ds = d2.select_method_wo_parameter(q1, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                header = "S.No-50/Transfer Date-100/Item Code-150/Item Name-150/Brand Name-150/Supplier Name-200/Transfer From-200/Transfer To-200/Transfer Quantity-100/RPU-100/Cost-100";
                Fpreadheaderbindmethod(header, FpSpread1, "false");
                for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                {
                    ds.Tables[0].DefaultView.RowFilter = "TransferType='" + Convert.ToString(ds.Tables[1].Rows[j]["TransferType"]) + "'";
                    DataView dv1 = ds.Tables[0].DefaultView;
                    if (dv1.Count > 0)
                    {
                        FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(transtype(Convert.ToString(ds.Tables[1].Rows[j]["TransferType"])));
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].ForeColor = Color.Green;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 11);
                        for (int k = 0; k < dv1.Count; k++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(k + 1);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv1[k]["TrasnferDate"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv1[k]["itemcode"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv1[k]["itemname"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                            string Brandname = d2.GetFunction("select distinct t.MasterValue  from CO_MasterValues t,IM_ItemMaster i where t.MasterCode=i.subheader_code  and subheader_code='" + Convert.ToString(dv1[k]["subheader_code"]) + "'");
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Brandname;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                            string suppname = d2.GetFunction("select distinct vm.VendorCompName  from  IT_GoodsInward gi,IM_ItemMaster im,IT_PurchaseOrder p,IT_PurchaseOrderDetail pi,CO_VendorMaster vm where p.PurchaseOrderPK =pi.PurchaseOrderFK and p.PurchaseOrderPK =gi.PurchaseOrderFK and pi.ItemFK =gi.itemfk and gi.itemfk =im.ItemPK and   im.ItemPK in('" + Convert.ToString(dv1[k]["ItemPK"]) + "') and p.ApproveStatus ='1'  and pi.Inward_Status=1 and isnull(gi.PurchaseOrderFK,'0')<>0 and vm.VendorPK=gi.VendorFK");

                            if (suppname == "" || suppname == "0")
                                suppname = "-";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = suppname;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                            string transfertype = Convert.ToString(dv1[k]["TransferType"]);
                            string transferfrom = Convert.ToString(dv1[k]["TransferFrom"]);
                            string transferto = Convert.ToString(dv1[k]["TrasferTo"]);
                            string transfromname = "";
                            string transtoname = "";
                            if (transfertype.Trim() == "1" || transfertype.Trim() == "3" || transfertype.Trim() == "4")
                            {
                                transfromname = getstorename(Convert.ToString(dv1[k]["TransferFrom"]));
                            }
                            else if (transfertype.Trim() == "2")
                            {
                                transfromname = getmessname(Convert.ToString(dv1[k]["TransferFrom"]));
                            }
                            else if (transfertype.Trim() == "5" || transfertype.Trim() == "6")
                            {
                                transfromname = getdeptname(Convert.ToString(dv1[k]["TransferFrom"]));
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = transfromname;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dv1[k]["toname"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(dv1[k]["TransferQty"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dv1[k]["TransferRpu"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                            double trsqty = 0;
                            double trsrpu = 0;
                            double cost = 0;
                            double.TryParse(Convert.ToString(dv1[k]["TransferQty"]), out trsqty);
                            double.TryParse(Convert.ToString(dv1[k]["TransferRpu"]), out trsrpu);
                            cost = trsqty * trsrpu;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(Math.Round(cost, 2));
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                        }
                    }
                    else
                    {
                        FpSpread1.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "No Records Founds";
                        rptprint.Visible = false;
                    }
                }
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread1.Visible = true;
                lbl_error.Visible = false;
                rptprint.Visible = true;
            }
            else
            {
                FpSpread1.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "No Records Founds";
                rptprint.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
            lbl_error.Text = ex.ToString();
        }
    }
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
            alertpopwindow.Visible = true;
            lblalerterr.Visible = true;
            lblalerterr.Font.Size = FontUnit.Smaller;
            lblalerterr.Text = ex.ToString();
        }
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    protected string transtype(string transfertype)
    {
        string name = "";
        try
        {
            if (transfertype.Trim() == "1")
            {
                name = "Store to Mess";
            }
            else if (transfertype.Trim() == "2")
            {
                name = "Mess to Mess";
            }
            else if (transfertype.Trim() == "3")
            {
                name = "Store to Store";
            }
            else if (transfertype.Trim() == "4")
            {
                name = "Store to Department";
            }
            else if (transfertype.Trim() == "5")
            {
                name = "Department to Department";
            }
            else if (transfertype.Trim() == "6")
            {
                name = "Department to Store";
            }
        }
        catch { }
        return name;
    }
    protected string getstorename(string storefk)
    {
        string name = "";
        try
        {
            name = d2.GetFunction("select StoreName from IM_StoreMaster where StorePK='" + storefk + "'");
        }
        catch { }
        return name;
    }
    protected string getmessname(string messname)
    {
        string name = "";
        try
        {
            name = d2.GetFunction("select MessName from HM_MessMaster where MessMasterPK='" + messname + "'");
        }
        catch { }
        return name;
    }
    protected string getdeptname(string deptname)
    {
        string name = "";
        try
        {
            name = d2.GetFunction("select dept_name from Department where Dept_code='" + deptname + "'");
        }
        catch { }
        return name;
    }
}