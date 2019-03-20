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
using System.IO;
public partial class inv_opening_stock : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
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
            //****************************************************//
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
                txt_searchby.Visible = true;
                //txt_fromdate.Attributes.Add("readonly", "readonly");
                //txt_todate.Attributes.Add("readonly", "readonly");
                txt_opendate1.Attributes.Add("readonly", "readonly");
                txt_total1.Attributes.Add("readonly", "readonly");
                txt_store.Attributes.Add("readonly", "readonly");
                txt_mess.Attributes.Add("readonly", "readonly");
                //txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_opendate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //cb_datewise.Checked = false;
                //cb_datewise.Enabled = false;
                //cb_datewise_change(sender, e);
                //txt_fromdate.Enabled = true;
                //txt_todate.Enabled = true;
                loadheadername();
                loadsubheadername();
                loaditem();
                ddlitemnamenew();
                // ddlitemname();
                rdb_store.Checked = true;
                rdb_store1.Checked = true;
                storetrue();
                hostelfalse();
                deptnamefalse();
                bindstore();
                bindhostelname();
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.Visible = false;
                spreadimport.Sheets[0].RowCount = 0;
                spreadimport.Sheets[0].ColumnCount = 0;
                spreadimport.Sheets[0].ColumnHeader.Visible = false;
                spreadimport.CommandBar.Visible = false;
                //btn_go_Click(sender, e);
                FileUpload1.Visible = false;
                btn_import.Visible = false;
                bindstore_chk(); bindmess_chk();
                rb_mess_OnCheckedChanged(sender, e);
                bind_deptname();
                bind_popdept();
                binditemcode();
                ViewState["Itemcode"] = null;
            }
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
    //main page
    //public void loaditem()
    //{
    //    try
    //    {
    //        ds.Clear();
    //        ds = d2.BindItemCodeWithOutParameter();
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            chklst_itemname.DataSource = ds;
    //            chklst_itemname.DataTextField = "item_name";
    //            chklst_itemname.DataValueField = "item_code";
    //            chklst_itemname.DataBind();
    //            if (chklst_itemname.Items.Count > 0)
    //            {
    //                for (int i = 0; i < chklst_itemname.Items.Count; i++)
    //                {
    //                    chklst_itemname.Items[i].Selected = true;
    //                }
    //                txt_itemname.Text = "Item Name(" + chklst_itemname.Items.Count + ")";
    //            }
    //        }
    //        else
    //        {
    //            txt_itemname.Text = "--Select--";
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}
    //protected void chk_itemname_CheckedChange(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (chk_itemname.Checked == true)
    //        {
    //            for (int i = 0; i < chklst_itemname.Items.Count; i++)
    //            {
    //                chklst_itemname.Items[i].Selected = true;
    //            }
    //            txt_itemname.Text = "Item Name(" + (chklst_itemname.Items.Count) + ")";
    //        }
    //        else
    //        {
    //            for (int i = 0; i < chklst_itemname.Items.Count; i++)
    //            {
    //                chklst_itemname.Items[i].Selected = false;
    //            }
    //            txt_itemname.Text = "--Select--";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //protected void chk_itemname_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        txt_itemname.Text = "--Select--";
    //        chk_itemname.Checked = false;
    //        int commcount = 0;
    //        for (int i = 0; i < chklst_itemname.Items.Count; i++)
    //        {
    //            if (chklst_itemname.Items[i].Selected == true)
    //            {
    //                commcount = commcount + 1;
    //            }
    //        }
    //        if (commcount > 0)
    //        {
    //            txt_itemname.Text = "Item Name(" + commcount.ToString() + ")";
    //            if (commcount == chklst_itemname.Items.Count)
    //            {
    //                chk_itemname.Checked = true;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    protected void cb_headername_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_headername.Checked == true)
            {
                for (int i = 0; i < cbl_headername.Items.Count; i++)
                {
                    cbl_headername.Items[i].Selected = true;
                }
                txt_headername.Text = "Header Name(" + (cbl_headername.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_headername.Items.Count; i++)
                {
                    cbl_headername.Items[i].Selected = false;
                }
                txt_headername.Text = "--Select--";
            }
            loadsubheadername();
            loaditem();
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_headername_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_headername.Text = "--Select--";
            cb_headername.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_headername.Items.Count; i++)
            {
                if (cbl_headername.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_headername.Text = "Header Name(" + commcount.ToString() + ")";
                if (commcount == cbl_headername.Items.Count)
                {
                    cb_headername.Checked = true;
                }
            }
            loadsubheadername();
            loaditem();
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_itemname_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_itemname.Checked == true)
            {
                for (int i = 0; i < cbl_itemname.Items.Count; i++)
                {
                    cbl_itemname.Items[i].Selected = true;
                }
                txt_itemname.Text = "Item Name(" + (cbl_itemname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_itemname.Items.Count; i++)
                {
                    cbl_itemname.Items[i].Selected = false;
                }
                txt_itemname.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_itemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_itemname.Text = "--Select--";
            cb_itemname.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_itemname.Items.Count; i++)
            {
                if (cbl_itemname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_itemname.Text = "Item Name(" + commcount.ToString() + ")";
                if (commcount == cbl_itemname.Items.Count)
                {
                    cb_itemname.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void loadheadername()
    {
        try
        {
            cbl_headername.Items.Clear();
            ddl_itemheadername.Items.Clear();
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
            ds.Clear();
            ds = d2.BindItemHeaderWithRights_inv();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_headername.DataSource = ds;
                cbl_headername.DataTextField = "ItemHeaderName";
                cbl_headername.DataValueField = "ItemHeaderCode";
                cbl_headername.DataBind();
                ddl_itemheadername.DataSource = ds;
                ddl_itemheadername.DataTextField = "ItemHeaderName";
                ddl_itemheadername.DataValueField = "ItemHeaderCode";
                ddl_itemheadername.DataBind();
                ddl_itemheadername.Items.Insert(0, "Select");
                if (cbl_headername.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_headername.Items.Count; i++)
                    {
                        cbl_headername.Items[i].Selected = true;
                    }
                    txt_headername.Text = "Header Name(" + cbl_headername.Items.Count + ")";
                }
            }
            else
            {
                ddl_itemheadername.Items.Insert(0, "Select");
                txt_headername.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void loaditem()
    {
        try
        {
            cbl_itemname.Items.Clear();
            string itemheader = "";
            string subheader = "";
            for (int i = 0; i < cbl_headername.Items.Count; i++)
            {
                if (cbl_headername.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_headername.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_headername.Items[i].Value.ToString() + "";
                    }
                }
            }
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
            if (itemheader.Trim() != "" && subheader.Trim() != "")
            {
                ds.Clear();
                ds = d2.BindItempkwithsubheader_inv(itemheader, subheader);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_itemname.DataSource = ds;
                    cbl_itemname.DataTextField = "itemname";
                    cbl_itemname.DataValueField = "itempk";
                    cbl_itemname.DataBind();
                    //ddl_itemname1.DataSource = ds;
                    //ddl_itemname1.DataTextField = "item_name";
                    //ddl_itemname1.DataValueField = "item_code";
                    //ddl_itemname1.DataBind();
                    //ddl_itemname1.Items.Insert(0, "Select");
                    if (cbl_itemname.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_itemname.Items.Count; i++)
                        {
                            cbl_itemname.Items[i].Selected = true;
                        }
                        txt_itemname.Text = "Item Name(" + cbl_itemname.Items.Count + ")";
                    }
                    if (cbl_itemname.Items.Count > 5)
                    {
                        Panel1.Width = 300;
                        Panel1.Height = 300;
                    }
                }
                else
                {
                    txt_itemname.Text = "--Select--";
                }
            }
            else
            {
                txt_itemname.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    //protected void cb_datewise_change(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (cb_datewise.Checked == false)
    //        {
    //            txt_fromdate.Enabled = false;
    //            txt_todate.Enabled = false;
    //        }
    //        else
    //        {
    //            txt_fromdate.Enabled = true;
    //            txt_todate.Enabled = true;
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}
    protected void rb_store_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rb_store.Checked == true)
        {
            lbl_store.Text = "Store";
            txt_store.Visible = true;
            Panel3.Visible = true;
            txt_mess.Visible = false;
            Panel4.Visible = false;
            upp6.Visible = false;
        }
        else if (rb_mess.Checked == true)
        {
            lbl_store.Text = "Mess";
            txt_mess.Visible = true;
            Panel4.Visible = true;
            txt_store.Visible = false;
            Panel3.Visible = false;
            upp6.Visible = false;
        }
        else if (rb_dept.Checked == true)
        {
            lbl_store.Text = "";
            upp6.Visible = true;
            txt_store.Visible = false;
            Panel3.Visible = false;
            txt_mess.Visible = false;
            Panel4.Visible = false;
        }
    }
    protected void rb_dept_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rb_store.Checked == true)
        {
            lbl_store.Text = "Store";
            txt_store.Visible = true;
            Panel3.Visible = true;
            txt_mess.Visible = false;
            Panel4.Visible = false;
            upp6.Visible = false;
        }
        else if (rb_mess.Checked == true)
        {
            lbl_store.Text = "Mess";
            txt_mess.Visible = true;
            Panel4.Visible = true;
            txt_store.Visible = false;
            Panel3.Visible = false;
            upp6.Visible = false;
        }
        else if (rb_dept.Checked == true)
        {
            lbl_store.Text = "";
            upp6.Visible = true;
            txt_store.Visible = false;
            Panel3.Visible = false;
            txt_mess.Visible = false;
            Panel4.Visible = false;
        }
    }
    protected void rb_mess_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rb_mess.Checked == true)
        {
            lbl_store.Text = "Mess";
            txt_mess.Visible = true;
            Panel4.Visible = true;
            txt_store.Visible = false;
            Panel3.Visible = false;
            upp6.Visible = false;
        }
        else if (rb_store.Checked == true)
        {
            lbl_store.Text = "Store";
            txt_store.Visible = true;
            Panel3.Visible = true;
            txt_mess.Visible = false;
            Panel4.Visible = false;
            upp6.Visible = false;
        }
        else if (rb_dept.Checked == true)
        {
            lbl_store.Text = "";
            upp6.Visible = true;
            txt_store.Visible = false;
            Panel3.Visible = false;
            txt_mess.Visible = false;
            Panel4.Visible = false;
        }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            if (cb_direct.Checked == false)
            {
                string itemheadercode = "";
                for (int i = 0; i < cbl_itemname.Items.Count; i++)
                {
                    if (cbl_itemname.Items[i].Selected == true)
                    {
                        if (itemheadercode == "")
                        {
                            itemheadercode = "" + cbl_itemname.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            itemheadercode = itemheadercode + "'" + "," + "'" + cbl_itemname.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string storepk = "";
                for (int i = 0; i < cbl_storeb.Items.Count; i++)
                {
                    if (cbl_storeb.Items[i].Selected == true)
                    {
                        if (storepk == "")
                        {
                            storepk = "" + cbl_storeb.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            storepk = storepk + "'" + "," + "'" + cbl_storeb.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string messpk = "";
                for (int i = 0; i < cbl_messb.Items.Count; i++)
                {
                    if (cbl_messb.Items[i].Selected == true)
                    {
                        if (messpk == "")
                        {
                            messpk = "" + cbl_messb.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            messpk = messpk + "'" + "," + "'" + cbl_messb.Items[i].Value.ToString() + "";
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
                if (itemheadercode.Trim() != "")
                {
                    string selectquery = "";
                    if (rb_store.Checked == true)
                    {
                        #region date wise
                        //if (cb_datewise.Checked == true)
                        //{
                        //    string firstdate = Convert.ToString(txt_fromdate.Text);
                        //    string secondate = Convert.ToString(txt_todate.Text);
                        //    string[] splitdate = firstdate.Split('/');
                        //    DateTime dt = new DateTime();
                        //    dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
                        //    splitdate = secondate.Split('/');
                        //    DateTime dt1 = new DateTime();
                        //    dt1 = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
                        //    if (txt_searchby.Text.Trim() != "")
                        //    {
                        //        //'" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'
                        //        selectquery = "select distinct i.itempk,i.itemcode,itemname,i.itemunit,InwardRPU,BalQty,cast((BalQty*InwardRPU) as decimal(10,2))as stock_value,s.StoreFk,Sailing_prize from IT_StockDetail s,IM_ItemMaster i where s.ItemFK =i.ItemPK and  i.ItemName ='" + txt_searchby.Text + "' and s.StoreFK in('" + storepk + "') and ISNULL(InwardType,0)<>2 ";//and ISNULL(BalQty,0)<>0
                        //    }
                        //    else if (txt_searchitemcode.Text.Trim() != "")
                        //    {
                        //        selectquery = "select distinct i.itempk,i.itemcode,itemname,i.itemunit,InwardRPU,BalQty,cast((BalQty*InwardRPU) as decimal(10,2))as stock_value,s.StoreFk,Sailing_prize from IT_StockDetail s,IM_ItemMaster i where s.ItemFK =i.ItemPK and i.itemcode in('" + txt_searchitemcode.Text + "')  and s.StoreFK in('" + storepk + "') and ISNULL(InwardType,0)<>2";//and ISNULL(BalQty,0)<>0
                        //    }
                        //    else
                        //    {
                        //        selectquery = "select distinct i.itempk, i.itemcode,itemname,i.itemunit,InwardRPU,BalQty,cast((BalQty*InwardRPU) as decimal(10,2))as stock_value,s.StoreFk,Sailing_prize from IT_StockDetail s,IM_ItemMaster i where s.ItemFK =i.ItemPK and i.itempk in('" + itemheadercode + "')  and s.StoreFK in('" + storepk + "') and ISNULL(InwardType,0)<>2 ";//and ISNULL(BalQty,0)<>0
                        //    }
                        //}
                        //else
                        //{
                            if (txt_searchby.Text.Trim() != "")
                            {
                                selectquery = "select i.itempk, i.itemcode,itemname,i.itemunit,InwardRPU,BalQty,cast((BalQty*InwardRPU) as decimal(10,2))as stock_value,s.StoreFk,Sailing_prize from IT_StockDetail s,IM_ItemMaster i where s.ItemFK =i.ItemPK and i.ItemName ='" + txt_searchby.Text + "' and s.StoreFK in('" + storepk + "') and ISNULL(InwardType,0)<>2 ";//and ISNULL(BalQty,0)<>0
                            }
                            else if (txt_searchitemcode.Text.Trim() != "")
                            {
                                selectquery = "select i.itempk, i.itemcode,itemname,i.itemunit,InwardRPU,BalQty,cast((BalQty*InwardRPU) as decimal(10,2))as stock_value,s.StoreFk,Sailing_prize from IT_StockDetail s,IM_ItemMaster i where s.ItemFK =i.ItemPK and i.ItemCode='" + txt_searchitemcode.Text + "'  and s.StoreFK in('" + storepk + "') and ISNULL(InwardType,0)<>2";//and InwardType ='3' and ISNULL(BalQty,0)<>0
                            }
                            else
                            {
                                selectquery = "select i.itempk,i.itemcode,itemname,i.itemunit,InwardRPU,BalQty,cast((BalQty*InwardRPU) as decimal(10,2))as stock_value,s.StoreFk,Sailing_prize from IT_StockDetail s,IM_ItemMaster i where s.ItemFK =i.ItemPK and i.ItemPK in ('" + itemheadercode + "') and s.StoreFK in('" + storepk + "') and ISNULL(InwardType,0)<>2 ";// and InwardType ='3' and ISNULL(BalQty,0)<>0 
                            }
                        //}
                        #endregion
                    }
                    else if (rb_mess.Checked == true)
                    {
                        //if (cb_datewise.Checked == true)
                        //{
                        //    string firstdate = Convert.ToString(txt_fromdate.Text);
                        //    string secondate = Convert.ToString(txt_todate.Text);
                        //    string[] splitdate = firstdate.Split('/');
                        //    DateTime dt = new DateTime();
                        //    dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
                        //    splitdate = secondate.Split('/');
                        //    DateTime dt1 = new DateTime();
                        //    dt1 = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
                        //    if (txt_searchby.Text.Trim() != "")
                        //    {
                        //        //'" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'
                        //        selectquery = "select distinct i.itempk, i.itemcode,itemname,i.itemunit,IssuedRPU,IssuedQty-ISNULL(UsedQty ,'0') BalQty,cast(((IssuedQty-ISNULL(UsedQty ,'0')) * IssuedRPU)  as decimal(10,2)) as stock_value,s.DeptFK,Sailing_prize from IT_StockDeptDetail s,IM_ItemMaster i where s.ItemFK =i.ItemPK and  i.ItemName ='" + txt_searchby.Text + "' ";//and ISNULL(OrderFK,0)=0 and ISNULL(InwardFK,0)=0 and ISNULL(Inward_Type,0)=0  
                        //    }
                        //    else if (txt_searchitemcode.Text.Trim() != "")
                        //    {
                        //        selectquery = "select distinct i.itempk, i.itemcode,itemname,i.itemunit,IssuedRPU,IssuedQty-ISNULL(UsedQty ,'0') BalQty,cast(((IssuedQty-ISNULL(UsedQty ,'0')) * IssuedRPU) as decimal(10,2)) as stock_value,s.DeptFK,Sailing_prize from IT_StockDeptDetail s,IM_ItemMaster i where s.ItemFK =i.ItemPK and i.itemcode in('" + txt_searchitemcode.Text + "'  )";//and ISNULL(OrderFK,0)=0 and ISNULL(InwardFK,0)=0 and ISNULL(Inward_Type,0)=0 
                        //    }
                        //    else
                        //    {
                        //        selectquery = "select distinct i.itempk, i.itemcode,itemname,i.itemunit,IssuedRPU,IssuedQty-ISNULL(UsedQty ,'0') BalQty,cast(((IssuedQty-ISNULL(UsedQty ,'0')) * IssuedRPU)  as decimal(10,2))as stock_value,s.DeptFK,Sailing_prize from IT_StockDeptDetail s,IM_ItemMaster i where s.ItemFK =i.ItemPK and i.itempk in('" + itemheadercode + "')";// and ISNULL(OrderFK,0)=0 and ISNULL(InwardFK,0)=0 and ISNULL(Inward_Type,0)=0  
                        //    }
                        //}
                        //else
                        //{
                            if (txt_searchby.Text.Trim() != "")
                            {
                                selectquery = "select  i.itempk,i.itemcode,itemname,i.itemunit,IssuedRPU,IssuedQty-ISNULL(UsedQty ,'0') BalQty,cast(((IssuedQty-ISNULL(UsedQty ,'0')) * IssuedRPU) as decimal(10,2)) as stock_value,s.DeptFK,Sailing_prize from IT_StockDeptDetail s,IM_ItemMaster i where s.ItemFK =i.ItemPK and i.ItemName ='" + txt_searchby.Text + "'  and s.DeptFK in('" + messpk + "') ";//and ISNULL(OrderFK,0)=0 and ISNULL(InwardFK,0)=0 and ISNULL(Inward_Type,0)=0  
                            }
                            else if (txt_searchitemcode.Text.Trim() != "")
                            {
                                selectquery = "select i.itempk, i.itemcode,itemname,i.itemunit,IssuedRPU,IssuedQty-ISNULL(UsedQty ,'0') BalQty,cast(((IssuedQty-ISNULL(UsedQty ,'0')) * IssuedRPU) as decimal(10,2)) as stock_value,s.DeptFK,Sailing_prize from IT_StockDeptDetail s,IM_ItemMaster i where s.ItemFK =i.ItemPK and i.ItemCode='" + txt_searchitemcode.Text + "' and s.DeptFK in('" + messpk + "') ";//and ISNULL(OrderFK,0)=0 and ISNULL(InwardFK,0)=0 and ISNULL(Inward_Type,0)=0  
                                //selectquery = " select i.itempk,i.itemcode,itemname,i.itemunit,IssuedRPU,sd.BalQty,(sd.BalQty*IssuedRPU)as stock_value,sd.DeptFK from IT_StockDeptDetail sd,IM_ItemMaster i,IT_StockDetail s where s.StoreFK=sd.DeptFK and s.ItemFK=sd.ItemFK and s.ItemFK=i.ItemPK and sd.ItemFK=i.ItemPK and s.InwardType='3' and i.ItemCode='" + txt_searchitemcode.Text + "' and sd.DeptFK in('" + messpk + "')";
                            }
                            else
                            {
                                selectquery = "select i.itempk,i.itemcode,itemname,i.itemunit,IssuedRPU,IssuedQty-ISNULL(UsedQty ,'0') BalQty,cast(((IssuedQty-ISNULL(UsedQty ,'0')) * IssuedRPU) as decimal(10,2)) as stock_value,s.DeptFK,Sailing_prize from IT_StockDeptDetail s,IM_ItemMaster i where s.ItemFK =i.ItemPK and i.ItemPK in ('" + itemheadercode + "')  and s.DeptFK in('" + messpk + "')";// and ISNULL(OrderFK,0)=0 and ISNULL(InwardFK,0)=0 and ISNULL(Inward_Type,0)=0  
                            }
                        //}
                    }
                    else if (rb_dept.Checked == true)
                    {
                        if (txt_searchby.Text.Trim() != "")
                        {
                            selectquery = " select i.itempk,i.itemcode,itemname,i.itemunit,IssuedRPU,IssuedQty-ISNULL(UsedQty ,'0') BalQty,cast(((IssuedQty-ISNULL(UsedQty ,'0')) * IssuedRPU) as decimal(10,2)) as stock_value,s.DeptFK,Sailing_prize from IT_StockDeptDetail s,IM_ItemMaster i,Department d where s.ItemFK =i.ItemPK  and d.Dept_Code=s.DeptFK  and i.Itemname ='" + txt_searchby.Text + "'";// and ISNULL(OrderFK,0)=0 and ISNULL(InwardFK,0)=0 and ISNULL(Inward_Type,0)=0  
                        }
                        else if (txt_searchitemcode.Text.Trim() != "")
                        {
                            selectquery = " select i.itempk,i.itemcode,itemname,i.itemunit,IssuedRPU,IssuedQty-ISNULL(UsedQty ,'0') BalQty,cast(((IssuedQty-ISNULL(UsedQty ,'0')) * IssuedRPU) as decimal(10,2)) as stock_value,s.DeptFK,Sailing_prize from IT_StockDeptDetail s,IM_ItemMaster i,Department d where s.ItemFK =i.ItemPK  and d.Dept_Code=s.DeptFK  and i.ItemCode ='" + txt_searchitemcode.Text + "'";// and ISNULL(OrderFK,0)=0 and ISNULL(InwardFK,0)=0 and ISNULL(Inward_Type,0)=0  
                        }
                        else
                        {
                            selectquery = " select i.itempk,i.itemcode,itemname,i.itemunit,IssuedRPU,IssuedQty-ISNULL(UsedQty ,'0') BalQty,cast(((IssuedQty-ISNULL(UsedQty ,'0')) * IssuedRPU) as decimal(10,2)) as stock_value,s.DeptFK,Sailing_prize from IT_StockDeptDetail s,IM_ItemMaster i,Department d where s.ItemFK =i.ItemPK and i.ItemPK in ('" + itemheadercode + "') and d.Dept_Code=s.DeptFK  and s.DeptFK in('" + deptcode + "') ";//and ISNULL(OrderFK,0)=0 and ISNULL(InwardFK,0)=0 and ISNULL(Inward_Type,0)=0  
                        }
                    }
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Fpspread1.Sheets[0].RowCount = 0;
                        Fpspread1.Sheets[0].ColumnCount = 0;
                        Fpspread1.CommandBar.Visible = false;
                        Fpspread1.Sheets[0].AutoPostBack = true;
                        Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpspread1.Sheets[0].RowHeader.Visible = false;
                        Fpspread1.Sheets[0].ColumnCount = 8;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Columns[0].Width = 50;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Item Code";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Columns[1].Width = 100;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Name";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Columns[2].Width = 200;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Quantity Measure";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Opening Quantity";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Rate Per Unit";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Sailing Prize";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total Value";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            Fpspread1.Sheets[0].RowCount++;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["itemcode"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["itempk"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["itemname"]);
                            if (rb_store.Checked == true)
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["InwardRPU"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["InwardRPU"]);
                            }
                            if (rb_mess.Checked == true)
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["IssuedRPU"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["IssuedRPU"]);
                            }
                            if (rb_dept.Checked == true)
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["IssuedRPU"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["IssuedRPU"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[row]["DeptFK"]);
                            }
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["itemunit"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Stock_Value"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["BalQty"]);
                            if (rb_store.Checked == true)
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[row]["StoreFK"]);
                            }
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            // Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["InwardRPU"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                            if (rb_mess.Checked == true)
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[row]["DeptFK"]);
                            }
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["Sailing_prize"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[row]["Stock_Value"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                        }
                        Fpspread1.Visible = true;
                        rptprint.Visible = true;
                        div1.Visible = true;
                        lbl_error.Visible = false;
                        Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    }
                    else
                    {
                        div1.Visible = false;
                        Fpspread1.Visible = false;
                        rptprint.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "No Records Found";
                    }
                }
                else
                {
                    div1.Visible = false;
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "Please Select Any one Item Name";
                }
            }
        }
        catch
        {
        }
    }
    protected void btn_delete_Click(object sender, EventArgs e)
    {
        try
        {
            if (btn_delete.Text == "Delete")
            {
                surediv.Visible = true;
                lbl_sure.Text = "Do you want to delete this record?";
            }
        }
        catch
        {
        }
    }
    public void delete()
    {
        try
        {
            bool delete = false;
            string itemfk = Convert.ToString(ddl_itemname1.SelectedItem.Value);
            string delquery = "";
            if (rdb_store.Checked == true)
            {
                delquery = "delete from IT_StockDetail where itemfk ='" + itemfk + "' and StoreFK='" + Convert.ToString(ddl_storename.SelectedItem.Value) + "' and ISNULL(OrderFK,0)=0 and ISNULL(InwardFK,0)=0 and ISNULL(InwardType,0)=0 ";
                int ins = d2.update_method_wo_parameter(delquery, "Text");
                if (ins != 0)
                {
                    delete = true;
                }
            }
            else if (rdb_hostel.Checked == true)
            {
                delquery = "delete from IT_StockDeptDetail where itemfk ='" + itemfk + "' and DeptFK ='" + Convert.ToString(ddl_Hostelname.SelectedItem.Value) + "'";
                int ins = d2.update_method_wo_parameter(delquery, "Text");
                if (ins != 0)
                {
                    delete = true;
                }
            }
            else if (rdb_dept.Checked == true)
            {
                delquery = "delete from IT_StockDeptDetail where itemfk ='" + itemfk + "' and DeptFK ='" + Convert.ToString(ddl_deptname.SelectedItem.Value) + "'";
                int ins = d2.update_method_wo_parameter(delquery, "Text");
                if (ins != 0)
                {
                    delete = true;
                }
            }
            if (delete == true)
            {
                alertmessage.Visible = true;
                surediv.Visible = false;
                lbl_alerterror.Visible = true;
                lbl_alerterror.Text = "Deleted Successfully";
                popwindow.Visible = false;
            }
            else { }
        }
        catch
        {
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
        alertmessage.Visible = false;
        popwindow.Visible = true;
    }
    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        clear();
        Printcontrol.Visible = false;
        btn_delete.Visible = false;
        btn_update.Visible = false;
        btn_save1.Visible = true;
        popwindow.Visible = true;
        txt_openquantity1.Enabled = true;
        ddl_deptname.Enabled = true;
        ddl_Hostelname.Enabled = true;
        ddl_storename.Enabled = true;
        rdb_store.Enabled = true;
        rdb_hostel.Enabled = true;
        rdb_dept.Enabled = true;
        cb_show.Enabled = true;
        if (cb_show.Checked == true)
        {
            loadheadername();
            loadddlsubheader();
            ddlitemname();
        }
        else
        {
            loadheadername();
            loadddlsubheader();
            ddlitemnamenew();
        }
        bindhostelname();
        bindstore();
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
            string degreedetails = "Opening Stock Entry Report" + '@' + "                                                                                                                                                                                                                                                            Date :" + System.DateTime.Now.ToString("dd/MM/yyyy");
            string pagename = "Opening_stock_entry.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
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
    protected void Fpspread1_render(object sender, EventArgs e)
    {
        try
        {
            if (check == true)
            {
                cb_show.Enabled = false;
                ddl_itemheadername.Enabled = false;
                ddl_itemname1.Enabled = false;
                ddl_subheadername.Enabled = false;
                popwindow.Visible = true;
                btn_delete.Visible = true;
                btn_update.Visible = false;//Barath update Remove
                btn_save1.Visible = false;
                string activerow = "";
                string activecol = "";
                activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                collegecode = Session["collegecode"].ToString();
                if (activerow.Trim() != "")
                {
                    string itemcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    string itemname = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                    string itempk = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                    string Quanityvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
                    string storefk = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag);
                    string Deptfk = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Tag);
                    string Quantitymeasure = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                    string stock_value = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag);
                    string rpu = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                    string sailingprize = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text);
                    txt_openquantity1.Enabled = false;
                    string itemheader = d2.GetFunction("select ItemHeaderName from IM_ItemMaster where ItemCode='" + itemcode + "'");
                    loadheadername();
                    loadddlsubheader();
                    ddl_itemheadername.SelectedIndex = ddl_itemheadername.Items.IndexOf(ddl_itemheadername.Items.FindByText(itemheader));
                    // ddlitemname();
                    ddlitemnamenew();
                    ddl_itemname1.SelectedIndex = ddl_itemname1.Items.IndexOf(ddl_itemname1.Items.FindByText(itemname));
                    ddl_itemname1.SelectedItem.Text = Convert.ToString(itemname);
                    ddl_itemname1.SelectedItem.Value = Convert.ToString(itempk);
                    txt_quantitymeasure1.Text = Convert.ToString(Quantitymeasure);
                    //txt_opendate1.Text = Convert.ToString(Quanitydate);
                    txt_openquantity1.Text = Convert.ToString(Quanityvalue);
                    txt_rateper1.Text = Convert.ToString(rpu);
                    txt_total1.Text = Convert.ToString(stock_value);
                    txt_sailingprize.Text = Convert.ToString(sailingprize);
                    string subheader = d2.GetFunction("select distinct  MasterValue from CO_MasterValues m,IM_ItemMaster i where m.MasterCode=i.subheader_code and i.itemcode='" + itemcode + "'");
                    ddl_subheadername.SelectedIndex = ddl_subheadername.Items.IndexOf(ddl_subheadername.Items.FindByText(subheader));
                    ddl_subheadername.SelectedItem.Text = Convert.ToString(subheader);
                    if (storefk.Trim() != "")
                    {
                        string store = "select distinct s.StoreName from IM_ItemMaster i,IM_StoreMaster s,IT_StockDetail sd where s.Storepk=i.StoreFK  and sd.StoreFK=s.StorePK and sd.StoreFK='" + storefk + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(store, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string storename = "";
                            storename = Convert.ToString(ds.Tables[0].Rows[0]["StoreName"].ToString());
                            ddl_storename.SelectedIndex = ddl_storename.Items.IndexOf(ddl_storename.Items.FindByText(storename));
                            rdb_store.Checked = true;
                            ddl_storename.Visible = true;
                            ddl_storename.Enabled = false;
                            rdb_store.Enabled = true;
                            rdb_dept.Checked = false;
                            rdb_dept.Enabled = false;
                            rdb_hostel.Checked = false;
                            lbl_hostelname.Visible = false;
                            ddl_Hostelname.Visible = false;
                            rdb_hostel.Enabled = false;
                            lbl_dept.Visible = false;
                            ddl_deptname.Visible = false;
                            rdb_dept.Enabled = false;
                            rdb_dept.Checked = false;
                        }
                    }
                    if (Deptfk.Trim() != "")
                    {
                        string hostel = "select distinct m.MessName  from HM_MessMaster m,IM_ItemMaster i,IT_StockDeptDetail sd where m.MessMasterPK=sd.Deptfk and sd.ItemFK=i.ItemPK and sd.DeptFK='" + Deptfk + "'";
                        ds1.Clear();
                        ds1 = d2.select_method_wo_parameter(hostel, "Text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            string hostelname = "";
                            hostelname = Convert.ToString(ds1.Tables[0].Rows[0]["MessName"].ToString());
                            ddl_Hostelname.SelectedIndex = ddl_Hostelname.Items.IndexOf(ddl_Hostelname.Items.FindByText(hostelname));
                            rdb_hostel.Checked = true;
                            lbl_hostelname.Visible = true;
                            ddl_Hostelname.Visible = true;
                            rdb_hostel.Enabled = true;
                            ddl_Hostelname.Enabled = false;
                            rdb_store.Enabled = false;
                            rdb_store.Checked = false;
                            lbl_storename.Visible = false;
                            ddl_storename.Visible = false;
                            lbl_dept.Visible = false;
                            ddl_deptname.Visible = false;
                            rdb_dept.Enabled = false;
                            rdb_dept.Checked = false;
                        }
                        string dept = "select Dept_Name from Department where Dept_Code in('" + Deptfk + "')";
                        ds1.Clear();
                        ds1 = d2.select_method_wo_parameter(dept, "Text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            string hostelname = "";
                            hostelname = Convert.ToString(ds1.Tables[0].Rows[0]["Dept_Name"].ToString());
                            hostelname = Convert.ToString(hostelname);
                            ddl_deptname.SelectedIndex = ddl_deptname.Items.IndexOf(ddl_deptname.Items.FindByText(hostelname));
                            rdb_dept.Checked = true;
                            ddl_deptname.Visible = true;
                            lbl_dept.Visible = true;
                            rdb_dept.Enabled = true;
                            ddl_deptname.Enabled = false;
                            lbl_hostelname.Visible = false;
                            ddl_Hostelname.Visible = false;
                            rdb_hostel.Checked = false;
                            rdb_hostel.Enabled = false;
                            rdb_store.Checked = false;
                            lbl_storename.Visible = false;
                            ddl_storename.Visible = false;
                            rdb_store.Enabled = false;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void img_btnpopclose_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }
    protected void cb_directimport_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_direct.Checked == true)
        {
            FileUpload1.Visible = true;
            btn_import.Visible = true;
            div1.Visible = false;
            rptprint.Visible = false;
        }
        else if (cb_direct.Checked == false)
        {
            FileUpload1.Visible = false;
            btn_import.Visible = false;
            spreadimport.Visible = false;
            rdb_store1.Visible = false;
            rdb_hostel1.Visible = false;
            ddl_deptname1.Visible = false;
            rdb_dept1.Visible = false;
            lbl_storename1.Visible = false;
            ddl_storename1.Visible = false;
            btn_save2.Visible = false;
        }
    }
    protected void btn_import_Click(object sender, EventArgs e)
    {
        try
        {
            using (Stream stream = this.FileUpload1.FileContent as Stream)
            {
                string extension = Path.GetFileName(FileUpload1.PostedFile.FileName);
                if (extension.Trim() != "")
                {
                    div1.Visible = false;
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                    stream.Position = 0;
                    this.spreadimport.OpenExcel(stream);
                    spreadimport.OpenExcel(stream);
                    spreadimport.SaveChanges();
                    spreadimport.SheetCorner.ColumnCount = 0;
                    spreadimport.CommandBar.Visible = false;
                    spreadimport.Sheets[0].AutoPostBack = true;
                    lbl_error.Visible = false;
                    btn_save2.Visible = true;
                    rdb_store1.Visible = true;
                    rdb_hostel1.Visible = true;
                    rdb_dept1.Visible = true;
                    lbl_storename1.Visible = true;
                    ddl_storename1.Visible = true;
                }
                else
                {
                    div1.Visible = false;
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                    spreadimport.CommandBar.Visible = false;
                    btn_save2.Visible = false;
                    rdb_store1.Visible = false;
                    rdb_hostel1.Visible = false;
                    rdb_dept1.Visible = false;
                    lbl_storename1.Visible = false;
                    ddl_storename1.Visible = false;
                    spreadimport.Visible = false;
                    alertmessage.Visible = true;
                    lbl_alerterror.Visible = true;
                    lbl_alerterror.Text = "Please Browse Import File";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void binditemcode()
    {
        try
        {
            clear();
            string newitemcode = "";
            string selectquery = "select ItemAcr,ItemStNo,ItemSize  from IM_CodeSettings order by startdate desc";//where Latestrec =1"
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string itemacronym = Convert.ToString(ds.Tables[0].Rows[0]["ItemAcr"]);
                string itemstarno = Convert.ToString(ds.Tables[0].Rows[0]["ItemStNo"]);
                string itemsize = Convert.ToString(ds.Tables[0].Rows[0]["ItemSize"]);
                if (itemacronym.Trim() != "" && itemstarno.Trim() != "")
                {
                    selectquery = " select distinct top (1) ItemCode  from IM_ItemMaster where ItemCode like '" + Convert.ToString(itemacronym) + "%' order by ItemCode desc";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string itemcode = Convert.ToString(ds.Tables[0].Rows[0]["ItemCode"]);
                        string itemacr = Convert.ToString(itemacronym);
                        int len = itemacr.Length;
                        itemcode = itemcode.Remove(0, len);
                        int len1 = Convert.ToString(itemcode).Length;
                        string newnumber = Convert.ToString((Convert.ToInt32(itemcode) + 1));
                        len = Convert.ToString(newnumber).Length;
                        len1 = len1 - len;
                        if (len1 == 2)
                        {
                            newitemcode = "00" + newnumber;
                        }
                        else if (len1 == 1)
                        {
                            newitemcode = "0" + newnumber;
                        }
                        else if (len1 == 3)
                        {
                            newitemcode = "000" + newnumber;
                        }
                        else if (len1 == 4)
                        {
                            newitemcode = "0000" + newnumber;
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
                        else if (size == 3)
                        {
                            newitemcode = "000" + itemstarno;
                        }
                        else if (size == 4)
                        {
                            newitemcode = "0000" + itemstarno;
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
                    ViewState["Itemcode"] = Convert.ToString(newitemcode);
                }
                else
                {
                    lbl_alerterror.Visible = true;
                    lbl_alerterror.Text = "Please Update Code Master";
                    alertmessage.Visible = true;
                }
            }
        }
        catch
        {
        }
    }
    protected void btn_save2_Click(object sender, EventArgs e)
    {
        try
        {
            bool inserted = false;
            string stockvalue = "";
            string dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            string update = "";
            if (spreadimport.Rows.Count > 0)
            {
                for (int i = 1; i < spreadimport.Rows.Count; i++)
                {
                    string itemcode = Convert.ToString(spreadimport.Sheets[0].Cells[i, 0].Text);
                    string itemname = Convert.ToString(spreadimport.Sheets[0].Cells[i, 1].Text);
                    string Quantitymeasure = Convert.ToString(spreadimport.Sheets[0].Cells[i, 2].Text);
                    string Quanityvalue = Convert.ToString(spreadimport.Sheets[0].Cells[i, 3].Text);
                    string rpu = Convert.ToString(spreadimport.Sheets[0].Cells[i, 4].Text);
                    string itemfk = "";
                    itemfk = d2.getitempk(itemcode);
                    if (stockvalue.Trim() == "")
                    {
                        double temp = Convert.ToDouble(rpu);
                        temp = temp * Convert.ToDouble(Quanityvalue);
                        stockvalue = stockvalue + Convert.ToString(temp);
                    }
                    string date = System.DateTime.Now.ToString("MM/dd/yyyy");
                    if (itemfk.Trim() != "" && itemfk.Trim() != "0")
                    {
                        if (Quanityvalue.Trim() != "")
                        {
                            if (rdb_store1.Checked == true)
                            {
                                string inserquery = "if exists(select*from IT_StockDetail where itemfk='" + itemfk + "' and StoreFK='" + ddl_storename1.SelectedItem.Value + "')update IT_StockDetail set InwardQty=ISNULL(InwardQty,0)+ISNULL('" + Quanityvalue + "',0),InwardRPU='" + rpu + "',StoreFK='" + ddl_storename1.SelectedItem.Value + "' where ItemFK='" + itemfk + "' and StoreFK='" + ddl_storename1.SelectedItem.Value + "' else insert into IT_StockDetail(ItemFK,inwardqty,InwardRPU,StoreFK)values('" + itemfk + "','" + Quanityvalue + "','" + rpu + "','" + ddl_storename1.SelectedItem.Value + "')";
                                int ins = d2.update_method_wo_parameter(inserquery, "Text");
                                if (ins != 0)
                                {
                                    update = "";
                                    update = "update IT_StockDetail set BalQty=ISNULL(inwardqty,0)-ISNULL(TransferQty,0) where ItemFK='" + itemfk + "' and StoreFK='" + ddl_storename1.SelectedItem.Value + "'";
                                    ins = d2.update_method_wo_parameter(update, "text");
                                    if (ins != 0)
                                    {
                                        inserted = true;
                                    }
                                }
                            }
                            else if (rdb_hostel1.Checked == true)
                            {
                                string q2 = "if exists(select*from IT_StockDeptDetail where DeptFK ='" + ddl_Hostelname1.SelectedItem.Value + "' and ItemFK='" + itemfk + "')update IT_StockDeptDetail set IssuedQty=ISNULL(IssuedQty,0)+ISNULL('" + Quanityvalue + "',0) ,IssuedRPU='" + rpu + "' where DeptFK ='" + ddl_Hostelname1.SelectedItem.Value + "' and ItemFK='" + itemfk + "' else insert into IT_StockDeptDetail(IssuedQty,IssuedRPU,DeptFK,ItemFK)values('" + Quanityvalue + "','" + rpu + "','" + ddl_Hostelname1.SelectedItem.Value + "','" + itemfk + "')";
                                int val1 = d2.update_method_wo_parameter(q2, "Text");
                                if (val1 != 0)
                                {
                                    update = " update IT_StockDeptDetail set BalQty=ISNULL(IssuedQty,0)-ISNULL('" + Quanityvalue + "',0)  where ItemFK='" + itemfk + "' and DeptFK='" + ddl_Hostelname1.SelectedItem.Value + "'";
                                    val1 = d2.update_method_wo_parameter(update, "Text");
                                    if (val1 != 0)
                                    {
                                        inserted = true;
                                    }
                                }
                            }
                            else if (rdb_dept1.Checked == true)
                            {
                                string q3 = "if exists(select*from IT_StockDeptDetail where itemfk='" + itemfk + "' and DeptFK ='" + ddl_deptname1.SelectedItem.Value + "')update IT_StockDeptDetail set IssuedQty=ISNULL(IssuedQty,0)+ISNULL('" + Quanityvalue + "',0),IssuedRPU='" + rpu + "' where ItemFK='" + itemfk + "' and DeptFK ='" + ddl_deptname1.SelectedItem.Value + "'  else insert into IT_StockDeptDetail(ItemFK,IssuedQty,IssuedRPU,DeptFK)values('" + itemfk + "','" + Quanityvalue + "','" + rpu + "','" + ddl_deptname1.SelectedItem.Value + "')";
                                int val1 = d2.update_method_wo_parameter(q3, "Text");
                                if (val1 != 0)
                                {
                                    update = " update IT_StockDeptDetail set BalQty=ISNULL(IssuedQty,0)-ISNULL('" + Quanityvalue + "',0)  where ItemFK='" + itemfk + "' and DeptFK='" + ddl_deptname1.SelectedItem.Value + "'";
                                    val1 = d2.update_method_wo_parameter(q3, "text");
                                    if (val1 != 0)
                                    {
                                        inserted = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        alertmessage.Visible = true;
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Please Check Valid Item Code";
                    }
                }
            }
            if (inserted == true)
            {
                popwindow.Visible = false;
                alertmessage.Visible = true;
                lbl_alerterror.Visible = true;
                lbl_alerterror.Text = "Saved Successfully";
                clear();
                btn_go_Click(sender, e);
            }
            else
            {
                alertmessage.Visible = true;
                lbl_alerterror.Visible = true;
                lbl_alerterror.Text = "Save Not Successfully";
            }
        }
        catch
        {
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = "Some Issues Occur.";
        }
    }
    public void storetrue1()
    {
        lbl_storename1.Visible = true;
        ddl_storename1.Visible = true;
    }
    public void storefalse1()
    {
        lbl_storename1.Visible = false;
        ddl_storename1.Visible = false;
    }
    public void hosteltrue1()
    {
        lbl_hostelname1.Visible = true;
        ddl_Hostelname1.Visible = true;
    }
    public void hostelfalse1()
    {
        lbl_hostelname1.Visible = false;
        ddl_Hostelname1.Visible = false;
    }
    public void deptnametrue1()
    {
        lbl_dept1.Visible = true;
        ddl_deptname1.Visible = true;
    }
    public void deptnamefalse1()
    {
        lbl_dept1.Visible = false;
        ddl_deptname1.Visible = false;
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
                ddl_deptname1.DataSource = ds;
                ddl_deptname1.DataTextField = "Dept_name";
                ddl_deptname1.DataValueField = "dept_code";
                ddl_deptname1.DataBind();
            }
        }
        catch { }
    }
    protected void rdb_store1_Click(object sender, EventArgs e)
    {
        storetrue1();
        hostelfalse1();
        deptnamefalse1();
    }
    protected void rdb_Hostel1_Click(object sender, EventArgs e)
    {
        hosteltrue1();
        storefalse1();
        deptnamefalse1();
    }
    protected void rdb_dept1_Click(object sender, EventArgs e)
    {
        deptnametrue1();
        storefalse1();
        hostelfalse1();
    }
    //popup1=popwindow
    protected void cb_show_Change(object sender, EventArgs e)
    {
        try
        {
            if (cb_show.Checked == true)
            {
                loadheadername();
                loadddlsubheader();
                ddlitemnamenew();
            }
            else
            {
                loadheadername();
                loadddlsubheader();
                ddlitemname();
            }
        }
        catch
        {
        }
    }
    protected void ddlitemname()
    {
        try
        {
            ddl_itemname1.Items.Clear();
            string buildvalue1 = "";
            string build1 = "";
            build1 = ddl_itemheadername.SelectedValue;
            buildvalue1 = ddl_subheadername.SelectedValue;
            ds.Clear();
            if (build1.Trim() != "" && buildvalue1.Trim() != "" && build1.Trim() != "Select" && buildvalue1.Trim() != "Select")
            {
                ds = d2.BindItempkwithsubheader_inv(build1, buildvalue1);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_itemname1.DataSource = ds;
                    ddl_itemname1.DataTextField = "itemname";
                    ddl_itemname1.DataValueField = "itempk";
                    ddl_itemname1.DataBind();
                    ddl_itemname1.Items.Insert(0, "Select");
                }
                else
                {
                    ddl_itemname1.Items.Insert(0, "Select");
                }
            }
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
        string query = "select distinct itemname from IM_ItemMaster WHERE itemname like '" + prefixText + "%' ";
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
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getitemcode(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select distinct itemcode from im_itemmaster WHERE itemcode like '" + prefixText + "%' ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["itemcode"].ToString());
            }
        }
        return name;
    }
    protected void ddl_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_type.SelectedValue == "0")
        {
            txt_searchby.Visible = true;
            txt_searchitemcode.Visible = false;
            txt_searchitemcode.Text = "";
        }
        else if (ddl_type.SelectedValue == "1")
        {
            txt_searchby.Visible = false;
            txt_searchitemcode.Visible = true;
            txt_searchby.Text = "";
        }
    }
    protected void ddl_itemname1_Change(object sender, EventArgs e)
    {
        try
        {
            string itemcode = Convert.ToString(ddl_itemname1.SelectedItem.Value);
            if (itemcode.Trim() != "")
            {
                string unit = d2.GetFunction("select ItemUnit from IM_ItemMaster where ItemPK='" + itemcode + "'");
                if (unit.Trim() != "" && unit.Trim() != "0")
                {
                    txt_quantitymeasure1.Text = Convert.ToString(unit);
                }
            }
        }
        catch
        {
        }
    }
    protected void ddl_itemheadername1_Change(object sender, EventArgs e)
    {
        loadddlsubheader();
        if (cb_show.Checked == true)
        {
            // ddlitemname();
        }
        else
        {
            //  ddlitemnamenew();
        }
    }
    protected void ddlitemnamenew()
    {
        try
        {
            ddl_itemname1.Items.Clear();
            string buildvalue1 = ddl_subheadername.SelectedValue;
            //string itemname = "select item_name,item_code from item_master where itemheader_code='" + ddl_itemheadername.SelectedItem.Value.ToString() + "' and subheader_code='" + buildvalue1 + "' and Is_Hostel ='0' and item_code not in (select item_code from stock_master)";
            if (ddl_itemheadername.SelectedItem.Value.Trim() != "Select" && buildvalue1.Trim() != "" && buildvalue1.Trim() != "Select")
            {
                string itemname = "select itemname,itemcode,itempk from IM_ItemMaster  where itemheadercode='" + ddl_itemheadername.SelectedItem.Value.ToString() + "' and subheader_code='" + buildvalue1 + "'  and ItemPK not in (select ItemfK from IT_StockDetail) order by itemname";//and  ForHostelItem='0'
                ds.Clear();
                ds = d2.select_method_wo_parameter(itemname, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_itemname1.DataSource = ds;
                    ddl_itemname1.DataTextField = "itemname";
                    ddl_itemname1.DataValueField = "itempk";
                    ddl_itemname1.DataBind();
                    ddl_itemname1.Items.Insert(0, "Select");
                }
                else
                {
                    ddl_itemname1.Items.Insert(0, "Select");
                }
            }
            else
            {
                ddl_itemname1.Items.Insert(0, "Select");
            }
        }
        catch
        {
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
        lbl_hostelname.Visible = true;
        ddl_Hostelname.Visible = true;
    }
    public void hostelfalse()
    {
        lbl_hostelname.Visible = false;
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
    protected void rdb_store_Click(object sender, EventArgs e)
    {
        storetrue();
        hostelfalse();
        deptnamefalse();
    }
    protected void rdb_Hostel_name(object sender, EventArgs e)
    {
        hosteltrue();
        storefalse();
        deptnamefalse();
    }
    protected void rdb_dept_Click(object sender, EventArgs e)
    {
        deptnametrue();
        storefalse();
        hostelfalse();
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
    protected void btn_save1_Click(object sender, EventArgs e)
    {
        try
        {
            string itemfk = Convert.ToString(ddl_itemname1.SelectedItem.Value);
            string opningquantity = Convert.ToString(txt_openquantity1.Text);
            string stock_value = Convert.ToString(txt_total1.Text);
            string rpu = Convert.ToString(txt_rateper1.Text);
            string opningdate = Convert.ToString(txt_opendate1.Text);
            string sailingprize = Convert.ToString(txt_sailingprize.Text);
            if (sailingprize.Trim() == "")
            {
                sailingprize = "0";
            }
            string[] splitdate = opningdate.Split('/');
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            string quantityunit = Convert.ToString(txt_quantitymeasure1.Text);
            DateTime dtaccessdate = DateTime.Now;
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            bool inserted = false;
            if (opningquantity.Trim() != "")
            {
                //Transfer type
                //store to mess=1
                //mess to mess=2
                //store to store=3
                //store to Department=4
                //Departent to department=5
                //Department to Store=6
                if (rdb_store.Checked == true)
                {
                    string inserquery = "if exists(select * from IT_StockDetail where itemfk='" + itemfk + "' and StoreFK='" + ddl_storename.SelectedItem.Value + "' and ISNULL(InwardType,0)=0 ) update IT_StockDetail set BalQty=BalQty+'" + opningquantity + "',InwardQty=InwardQty+'" + opningquantity + "',InwardRPU='" + rpu + "',StoreFK='" + ddl_storename.SelectedItem.Value + "',Sailing_prize='" + sailingprize + "' where ItemFK='" + itemfk + "' and StoreFK='" + ddl_storename.SelectedItem.Value + "' and ISNULL(InwardType,0)=0 else insert into IT_StockDetail(ItemFK,BalQty,InwardQty,InwardRPU,StoreFK,Sailing_prize)values('" + itemfk + "','" + opningquantity + "','" + opningquantity + "','" + rpu + "','" + ddl_storename.SelectedItem.Value + "','" + sailingprize + "')";//and ISNULL(OrderFK,0)=0 and ISNULL(InwardFK,0)=0 and ISNULL(InwardType,0)=0  
                    int ins = d2.update_method_wo_parameter(inserquery, "Text");
                    if (ins != 0)
                    {
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Saved Successfully";
                        alertmessage.Visible = true;
                        clear();
                        btn_go_Click(sender, e);
                    }
                }
                else if (rdb_hostel.Checked == true)
                {
                    string q2 = "";
                    //q2 = "if exists(select * from IT_StockDetail where itemfk='" + itemfk + "' and StoreFK='" + ddl_Hostelname.SelectedItem.Value + "' and InwardType='3') update IT_StockDetail set BalQty=BalQty+'" + opningquantity + "',InwardQty=InwardQty+'" + opningquantity + "',InwardRPU='" + rpu + "',StoreFK='" + ddl_Hostelname.SelectedItem.Value + "' where ItemFK='" + itemfk + "' and StoreFK='" + ddl_Hostelname.SelectedItem.Value + "' and InwardType='3' else insert into IT_StockDetail(ItemFK,BalQty,InwardQty,InwardRPU,StoreFK,InwardType)values('" + itemfk + "','" + opningquantity + "','" + opningquantity + "','" + rpu + "','" + ddl_Hostelname.SelectedItem.Value + "','3')";
                    q2 = " insert into IT_TransferItem (TrasnferDate,TransferType,TransferFrom,TrasferTo,TransferQty,itemfk,TransferRpu) values ('" + dtaccessdate.ToString("MM/dd/yyyy") + "','1','" + Convert.ToString(ddl_Hostelname.SelectedItem.Value) + "','" + Convert.ToString(ddl_Hostelname.SelectedItem.Value) + "','" + opningquantity + "','" + Convert.ToString(ddl_itemname1.SelectedItem.Value) + "','" + rpu + "')";
                    //string invrpu = d2.GetFunction("select IssuedRPU from IT_StockDeptDetail where itemfk='" + Convert.ToString(ddl_itemname1.SelectedItem.Value) + "' and DeptFK ='" + Convert.ToString(ddl_Hostelname.SelectedItem.Value) + "' and IssuedQty<>isnull(UsedQty,0) ");//15.02.18 barath
                    //double avrrpu = 0;
                    //if (invrpu.Trim() != "0")
                    //{
                    //    if (rpu != invrpu)
                    //    {
                    //        invrpu = Convert.ToString(Convert.ToDouble(rpu) + Convert.ToDouble(invrpu));
                    //        double.TryParse(invrpu, out avrrpu);
                    //        rpu = Convert.ToString(avrrpu / 2);
                    //    }
                    //}
                    q2 = q2 + " if exists(select*from IT_StockDeptDetail where itemfk='" + Convert.ToString(ddl_itemname1.SelectedItem.Value) + "' and DeptFK ='" + Convert.ToString(ddl_Hostelname.SelectedItem.Value) + "' ) update IT_StockDeptDetail set BalQty=BalQty+'" + opningquantity + "',IssuedQty=IssuedQty+'" + opningquantity + "',IssuedRPU='" + rpu + "',Sailing_prize='" + sailingprize + "' where ItemFK='" + Convert.ToString(ddl_itemname1.SelectedItem.Value) + "' and DeptFK ='" + ddl_Hostelname.SelectedItem.Value + "' else insert into IT_StockDeptDetail(ItemFK,BalQty,IssuedQty,IssuedRPU,DeptFK,Sailing_prize)values('" + Convert.ToString(ddl_itemname1.SelectedItem.Value) + "','" + opningquantity + "','" + opningquantity + "','" + rpu + "','" + Convert.ToString(ddl_Hostelname.SelectedItem.Value) + "','" + sailingprize + "')";//and ISNULL(OrderFK,0)=0 and ISNULL(InwardFK,0)=0 and ISNULL(Inward_Type,0)=0 
                    int val1 = d2.update_method_wo_parameter(q2, "Text");
                    if (val1 != 0)
                    {
                        inserted = true;
                    }
                    if (inserted == true)
                    {
                        alertmessage.Visible = true;
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Saved Successfully";
                        clear();
                        btn_go_Click(sender, e);
                        alertmessage.Visible = true;
                    }
                }
                else if (rdb_dept.Checked == true)
                {
                    string q3 = " insert into IT_TransferItem (TrasnferDate,TransferType,TransferFrom,TrasferTo,TransferQty,itemfk,TransferRpu) values ('" + dtaccessdate.ToString("MM/dd/yyyy") + "','4','" + Convert.ToString(ddl_deptname.SelectedItem.Value) + "','" + Convert.ToString(ddl_deptname.SelectedItem.Value) + "','" + opningquantity + "','" + Convert.ToString(ddl_itemname1.SelectedItem.Value) + "','" + rpu + "')";
                    //string invrpu = d2.GetFunction("select IssuedRPU from IT_StockDeptDetail where itemfk='" + Convert.ToString(ddl_itemname1.SelectedItem.Value) + "' and DeptFK ='" + Convert.ToString(ddl_deptname.SelectedItem.Value) + "' and IssuedQty<>isnull(UsedQty,0) ");//15.02.18 barath
                    //double avrrpu = 0;
                    //if (invrpu.Trim() != "0")
                    //{
                    //    if (rpu != invrpu)
                    //    {
                    //        invrpu = Convert.ToString(Convert.ToDouble(rpu) + Convert.ToDouble(invrpu));
                    //        double.TryParse(invrpu, out avrrpu);
                    //        rpu = Convert.ToString(avrrpu / 2);
                    //    }
                    //}
                    q3 = q3 + " if exists(select*from IT_StockDeptDetail where itemfk='" + Convert.ToString(ddl_itemname1.SelectedItem.Value) + "' and DeptFK ='" + Convert.ToString(ddl_deptname.SelectedItem.Value) + "' )update IT_StockDeptDetail set BalQty=BalQty+'" + opningquantity + "',IssuedQty=IssuedQty+'" + opningquantity + "',IssuedRPU='" + rpu + "',Sailing_prize='" + sailingprize + "' where ItemFK='" + Convert.ToString(ddl_itemname1.SelectedItem.Value) + "' and DeptFK ='" + Convert.ToString(ddl_deptname.SelectedItem.Value) + "' else insert into IT_StockDeptDetail(ItemFK,BalQty,IssuedQty,IssuedRPU,DeptFK,Sailing_prize)values('" + Convert.ToString(ddl_itemname1.SelectedItem.Value) + "','" + opningquantity + "','" + opningquantity + "','" + rpu + "','" + Convert.ToString(ddl_deptname.SelectedItem.Value) + "','" + sailingprize + "')";// and ISNULL(OrderFK,0)=0 and ISNULL(InwardFK,0)=0 and ISNULL(Inward_Type,0)=0
                    int val1 = d2.update_method_wo_parameter(q3, "Text");
                    if (val1 != 0)
                    {
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Saved Successfully";
                        clear();
                        btn_go_Click(sender, e);
                        alertmessage.Visible = true;
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void btn_exit1_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }
    protected void btn_update_Click(object sender, EventArgs e)
    {
        try
        {
            string itemcode = Convert.ToString(ddl_itemname1.SelectedItem.Value);
            string opningquantity = Convert.ToString(txt_openquantity1.Text);
            string opningdate = Convert.ToString(txt_opendate1.Text);
            string stock_value = Convert.ToString(txt_total1.Text);
            string rpu = Convert.ToString(txt_rateper1.Text);
            string sailingprize = Convert.ToString(txt_sailingprize.Text);
            if (sailingprize.Trim() == "")
            {
                sailingprize = "0";
            }
            string[] splitdate = opningdate.Split('/');
            //DateTime dt = new DateTime();
            //dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            string quantityunit = Convert.ToString(txt_quantitymeasure1.Text);
            string dtaccessdate = DateTime.Now.ToString();
            bool inserted = false;
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            if (opningquantity.Trim() != "")
            {
                if (rdb_store.Checked == true)
                {
                    string inserquery = "if exists(select * from IT_StockDetail where itemfk='" + itemcode + "' and StoreFK='" + ddl_storename.SelectedItem.Value + "' ) update IT_StockDetail set InwardRPU='" + rpu + "',Sailing_prize='" + sailingprize + "' where ItemFK='" + itemcode + "' and StoreFK='" + ddl_storename.SelectedItem.Value + "'";//and ISNULL(OrderFK,0)=0 and ISNULL(InwardFK,0)=0 and ISNULL(InwardType,0)=0 BalQty='" + opningquantity + "',InwardQty='" + opningquantity + "', else insert into IT_StockDetail(ItemFK,BalQty,InwardQty,InwardRPU,StoreFK,Sailing_prize)values('" + itemcode + "','" + opningquantity + "','" + opningquantity + "','" + rpu + "','" + ddl_storename.SelectedItem.Value + "','" + sailingprize + "')
                    int ins = d2.update_method_wo_parameter(inserquery, "Text");
                    if (ins != 0)
                        inserted = true;
                }
                if (rdb_hostel.Checked == true)
                {
                    //string q2 = "if exists(select * from IT_StockDeptDetail where itemfk='" + ddl_itemname1.SelectedItem.Value + "' and DeptFK ='" + ddl_Hostelname.SelectedItem.Value + "' )update IT_StockDeptDetail set BalQty='" + opningquantity + "',IssuedQty='" + opningquantity + "',IssuedRPU='" + rpu + "',Sailing_prize='" + sailingprize + "' where ItemFK='" + ddl_itemname1.SelectedItem.Value + "' and DeptFK ='" + ddl_Hostelname.SelectedItem.Value + "' else insert into IT_StockDeptDetail(ItemFK,BalQty,IssuedQty,IssuedRPU,DeptFK,Sailing_prize)values('" + ddl_itemname1.SelectedItem.Value + "','" + opningquantity + "','" + opningquantity + "','" + rpu + "','" + ddl_Hostelname.SelectedItem.Value + "','" + sailingprize + "')";//and ISNULL(OrderFK,0)=0 and ISNULL(InwardFK,0)=0 and ISNULL(Inward_Type,0)=0  
                    string q2 = "if exists(select * from IT_StockDeptDetail where itemfk='" + ddl_itemname1.SelectedItem.Value + "' and DeptFK ='" + ddl_Hostelname.SelectedItem.Value + "' )update IT_StockDeptDetail set IssuedRPU='" + rpu + "',Sailing_prize='" + sailingprize + "' where ItemFK='" + ddl_itemname1.SelectedItem.Value + "' and DeptFK ='" + ddl_Hostelname.SelectedItem.Value + "' ";
                    int val1 = d2.update_method_wo_parameter(q2, "Text");
                    if (val1 != 0)
                        inserted = true;
                }
                if (rdb_dept.Checked == true)
                {
                    string q2 = "if exists(select * from IT_StockDeptDetail where itemfk='" + ddl_itemname1.SelectedItem.Value + "' and DeptFK ='" + ddl_deptname.SelectedItem.Value + "')update IT_StockDeptDetail set IssuedRPU='" + rpu + "',Sailing_prize='" + sailingprize + "' where ItemFK='" + ddl_itemname1.SelectedItem.Value + "' and DeptFK ='" + ddl_deptname.SelectedItem.Value + "' ";// and ISNULL(OrderFK,0)=0 and ISNULL(InwardFK,0)=0 and ISNULL(Inward_Type,0)=0  BalQty='" + opningquantity + "',IssuedQty='" + opningquantity + "',else insert into IT_StockDeptDetail(ItemFK,BalQty,IssuedQty,IssuedRPU,DeptFK,Sailing_prize)values('" + ddl_itemname1.SelectedItem.Value + "','" + opningquantity + "','" + opningquantity + "','" + rpu + "','" + ddl_deptname.SelectedItem.Value + "','" + sailingprize + "')
                    int val1 = d2.update_method_wo_parameter(q2, "Text");
                    if (val1 != 0)
                        inserted = true;
                }
                if (inserted == true)
                {
                    btn_go_Click(sender, e);
                    popwindow.Visible = false;
                    lbl_alerterror.Visible = true;
                    lbl_alerterror.Text = "Updated Successfully";
                    alertmessage.Visible = true;
                }
            }
        }
        catch
        {
        }
    }
    //both functions   
    protected void bindstore()
    {
        ds.Clear();
        // ds = d2.BindStore_inv(collegecode1);
        string storepk = d2.GetFunction("select value from Master_Settings where settings='Store Rights' and usercode='" + usercode + "'  and value<>''");
        if (storepk.Trim() != "0")
        {
            ds = d2.BindStorebaseonrights_inv(storepk);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_storename.DataSource = ds;
                ddl_storename.DataTextField = "StoreName";
                ddl_storename.DataValueField = "StorePK";
                ddl_storename.DataBind();
                ddl_storename1.DataSource = ds;
                ddl_storename1.DataTextField = "StoreName";
                ddl_storename1.DataValueField = "StorePK";
                ddl_storename1.DataBind();
            }
        }
    }
    protected void bindhostelname()
    {
        ds.Clear();
        //ds = d2.Bindmess_inv(collegecode1);
        ds = d2.Bindmess_basedonrights(usercode, collegecode1);
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_Hostelname.DataSource = ds;
                ddl_Hostelname.DataTextField = "MessName";
                ddl_Hostelname.DataValueField = "MessMasterPK";
                ddl_Hostelname.DataBind();
                ddl_Hostelname1.DataSource = ds;
                ddl_Hostelname1.DataTextField = "MessName";
                ddl_Hostelname1.DataValueField = "MessMasterPK";
                ddl_Hostelname1.DataBind();
            }
        }
    }
    protected void clear()
    {
        if (cb_show.Checked == true)
        {
            loadheadername();
            loadddlsubheader();
            ddlitemname();
        }
        else
        {
            loadheadername();
            loadddlsubheader();
            ddlitemnamenew();
        }
        loaditem();
        loadheadername();
        loadsubheadername();
        bindstore();
        bindhostelname();
        txt_quantitymeasure1.Text = "";
        txt_openquantity1.Text = "";
        txt_total1.Text = "";
        txt_rateper1.Text = ""; txt_sailingprize.Text = "";
        txt_opendate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
        rdb_hostel.Checked = false;
        rdb_dept.Checked = false;
        rdb_store.Checked = true;
        storetrue();
        hostelfalse();
        deptnamefalse();
        ddl_itemheadername.Enabled = true;
        ddl_itemname1.Enabled = true;
        ddl_subheadername.Enabled = true;
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alertmessage.Visible = false;
    }
    protected void ddl_subheadername_Change(object sender, EventArgs e)
    {
        if (cb_show.Checked == true)
        {
            ddlitemname();
        }
        else
        {
            ddlitemnamenew();
        }
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
            for (int i = 0; i < cbl_headername.Items.Count; i++)
            {
                if (cbl_headername.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_headername.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_headername.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemheader.Trim() != "")
            {
                string query = "";
                query = "select distinct MasterCode,MasterValue from CO_MasterValues m,IM_ItemMaster i where m.MasterCode=i.subheader_code and itemheadercode in ('" + itemheader + "') and collegecode in ('" + collegecode1 + "')";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
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
                    }
                    if (cbl_subheadername.Items.Count > 5)
                    {
                        Panel2.Width = 300;
                        Panel2.Height = 300;
                    }
                }
                else
                {
                    txt_subheadername.Text = "--Select--";
                }
            }
            else
            {
                txt_subheadername.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void loadddlsubheader()
    {
        try
        {
            if (cb_show.Checked == true)
            {
                ddlitemname();
            }
            else
            {
                ddlitemnamenew();
            }
            ddl_subheadername.Items.Clear();
            string query = "";
            query = "select distinct MasterCode,MasterValue from CO_MasterValues m,IM_ItemMaster i where m.MasterCode=i.subheader_code and itemheadercode ='" + ddl_itemheadername.SelectedItem.Value.ToString() + "' and collegecode in ('" + collegecode1 + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_subheadername.DataSource = ds;
                ddl_subheadername.DataTextField = "MasterValue";
                ddl_subheadername.DataValueField = "MasterCode";
                ddl_subheadername.DataBind();
                ddl_subheadername.Items.Insert(0, "Select");
            }
            else
            {
                ddl_subheadername.Items.Insert(0, "Select");
            }
        }
        catch
        {
        }
    }
    //protected void txt_fromdate_Textchanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (txt_fromdate.Text != "" && txt_todate.Text != "")
    //        {
    //            DateTime dt = new DateTime();
    //            DateTime dt1 = new DateTime();
    //            string firstdate = Convert.ToString(txt_fromdate.Text);
    //            string seconddate = Convert.ToString(txt_todate.Text);
    //            string[] split = firstdate.Split('/');
    //            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
    //            split = seconddate.Split('/');
    //            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
    //            TimeSpan ts = dt1 - dt;
    //            int days = ts.Days;
    //            if (dt > dt1)
    //            {
    //                alertmessage.Visible = true;
    //                lbl_alerterror.Visible = true;
    //                lbl_alerterror.Text = "Enter FromDate less than or equal to the ToDate";
    //                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
    //            }
    //            else
    //            {
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //protected void txt_todate_Textchanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (txt_todate.Text != "" && txt_fromdate.Text != "")
    //        {
    //            DateTime dt = new DateTime();
    //            DateTime dt1 = new DateTime();
    //            string firstdate = Convert.ToString(txt_fromdate.Text);
    //            string seconddate = Convert.ToString(txt_todate.Text);
    //            string[] split = firstdate.Split('/');
    //            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
    //            split = seconddate.Split('/');
    //            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
    //            TimeSpan ts = dt1 - dt;
    //            int days = ts.Days;
    //            if (dt > dt1)
    //            {
    //                alertmessage.Visible = true;
    //                lbl_alerterror.Visible = true;
    //                lbl_alerterror.Text = "Enter ToDate greater than or equal to the FromDate";
    //                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
    //            }
    //            else
    //            {
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
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
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_messb_oncheckedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_messb.Checked == true)
            {
                for (int i = 0; i < cbl_messb.Items.Count; i++)
                {
                    cbl_messb.Items[i].Selected = true;
                }
                txt_mess.Text = "Mess Name(" + (cbl_messb.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_messb.Items.Count; i++)
                {
                    cbl_messb.Items[i].Selected = false;
                }
                txt_mess.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_messb_selectedindexchange(object sender, EventArgs e)
    {
        try
        {
            txt_mess.Text = "--Select--";
            cb_messb.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_messb.Items.Count; i++)
            {
                if (cbl_messb.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_mess.Text = "Mess Name(" + commcount.ToString() + ")";
                if (commcount == cbl_messb.Items.Count)
                {
                    cb_messb.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void bindstore_chk()
    {
        try
        {
            ds.Clear();
            //cbl_storeb.Items.Clear();
            //ds = d2.BindStore_inv(collegecode1);
            string storepk = d2.GetFunction("select value from Master_Settings where settings='Store Rights' and usercode='" + usercode + "'  and value<>''");
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
    protected void bindmess_chk()
    {
        try
        {
            ds.Clear();
            //cbl_messb.Items.Clear();
            // ds = d2.Bindmess_inv(collegecode1);
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_messb.DataSource = ds;
                cbl_messb.DataTextField = "MessName";
                cbl_messb.DataValueField = "MessMasterPK";
                cbl_messb.DataBind();
                for (int i = 0; i < cbl_messb.Items.Count; i++)
                {
                    cbl_messb.Items[i].Selected = true;
                    txt_mess.Text = "Mess Name(" + (cbl_messb.Items.Count) + ")";
                    cb_messb.Checked = true;
                }
            }
        }
        catch { }
    }
}
/*
 19.10.16 changes of nec 
 
 
 */