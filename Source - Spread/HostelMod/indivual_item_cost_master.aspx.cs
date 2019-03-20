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

public partial class indivual_item_cost_master : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    bool check = false;

    string itemeasure = "";
    string costperunit = "";

    string getday = "";
    string date = "";

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
        if (!IsPostBack)
        {
            loadheadername();
            loadsubheadername();
            loaditem();
            // ddlitemname();
            txt_searchby.Visible = true;
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Fpspread1.Visible = false;
            txt_date.Attributes.Add("readonly", "readonly");
            txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            btn_go_Click(sender, e);
        }


    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select distinct item_name from item_master WHERE item_name like '" + prefixText + "%' ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["Item_name"].ToString());
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
        string query = "select distinct item_code from item_master WHERE item_code like '" + prefixText + "%' ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["item_code"].ToString());
            }
        }
        return name;

    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getitemheader(string prefixText)
    {

        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select distinct itemheader_name from item_master WHERE itemheader_name like '" + prefixText + "%' ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["itemheader_name"].ToString());
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
            txt_searchheadername.Visible = false;
            txt_searchheadername.Text = "";
            txt_searchitemcode.Text = "";
        }
        else if (ddl_type.SelectedValue == "1")
        {
            txt_searchby.Visible = false;
            txt_searchitemcode.Visible = true;
            txt_searchheadername.Visible = false;
            txt_searchby.Text = "";
            txt_searchheadername.Text = "";

        }
        else if (ddl_type.SelectedValue == "2")
        {
            txt_searchby.Visible = false;
            txt_searchitemcode.Visible = false;
            txt_searchheadername.Visible = true;
            txt_searchby.Text = "";

            txt_searchitemcode.Text = "";
        }
    }
    //protected void cb_headername_CheckedChange(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (cb_headername.Checked == true)
    //        {
    //            for (int i = 0; i < cbl_headername.Items.Count; i++)
    //            {
    //                cbl_headername.Items[i].Selected = true;
    //            }
    //            txt_headername.Text = "Header Name(" + (cbl_headername.Items.Count) + ")";
    //        }
    //        else
    //        {
    //            for (int i = 0; i < cbl_headername.Items.Count; i++)
    //            {
    //                cbl_headername.Items[i].Selected = false;
    //            }
    //            txt_headername.Text = "--Select--";
    //        }
    //        loaditem();
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}
    //protected void cbl_headername_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {

    //        txt_headername.Text = "--Select--";
    //        cb_headername.Checked = false;
    //        int commcount = 0;
    //        for (int i = 0; i < cbl_headername.Items.Count; i++)
    //        {
    //            if (cbl_headername.Items[i].Selected == true)
    //            {
    //                commcount = commcount + 1;
    //            }
    //        }
    //        if (commcount > 0)
    //        {
    //            txt_headername.Text = "Header Name(" + commcount.ToString() + ")";
    //            if (commcount == cbl_headername.Items.Count)
    //            {
    //                cb_headername.Checked = true;
    //            }
    //        }
    //        loaditem();
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

    //protected void cb_itemname_CheckedChange(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (cb_itemname.Checked == true)
    //        {
    //            for (int i = 0; i < cbl_itemname.Items.Count; i++)
    //            {
    //                cbl_itemname.Items[i].Selected = true;
    //            }
    //            txt_itemname.Text = "Item Name(" + (cbl_itemname.Items.Count) + ")";
    //        }
    //        else
    //        {
    //            for (int i = 0; i < cbl_itemname.Items.Count; i++)
    //            {
    //                cbl_itemname.Items[i].Selected = false;
    //            }
    //            txt_itemname.Text = "--Select--";
    //        }

    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}
    //protected void cbl_itemname_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        txt_itemname.Text = "--Select--";
    //        cb_itemname.Checked = false;
    //        int commcount = 0;
    //        for (int i = 0; i < cbl_itemname.Items.Count; i++)
    //        {
    //            if (cbl_itemname.Items[i].Selected == true)
    //            {
    //                commcount = commcount + 1;
    //            }
    //        }
    //        if (commcount > 0)
    //        {
    //            txt_itemname.Text = "Item Name(" + commcount.ToString() + ")";
    //            if (commcount == cbl_itemname.Items.Count)
    //            {
    //                cb_itemname.Checked = true;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

    //public void loadheadername()
    //{
    //    try
    //    {
    //        cbl_headername.Items.Clear();

    //        string group_code = Session["group_code"].ToString();
    //        string columnfield = "";
    //        if (group_code.Contains(';'))
    //        {
    //            string[] group_semi = group_code.Split(';');
    //            group_code = group_semi[0].ToString();
    //        }
    //        if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
    //        {
    //            columnfield = " and group_code='" + group_code + "'";
    //        }
    //        else
    //        {
    //            columnfield = " and usercode='" + Session["usercode"] + "'";
    //        }
    //        string maninvalue = "";
    //        string selectnewquery = d2.GetFunction("select value  from Master_Settings where settings='ItemHeaderRights' " + columnfield + "");
    //        if (selectnewquery.Trim() != "" && selectnewquery.Trim() != "0")
    //        {
    //            string[] splitnew = selectnewquery.Split(',');
    //            if (splitnew.Length > 0)
    //            {
    //                for (int row = 0; row <= splitnew.GetUpperBound(0); row++)
    //                {
    //                    if (maninvalue == "")
    //                    {
    //                        maninvalue = Convert.ToString(splitnew[row]);
    //                    }
    //                    else
    //                    {
    //                        maninvalue = maninvalue + "'" + "," + "'" + Convert.ToString(splitnew[row]);
    //                    }
    //                }
    //            }
    //        }

    //        if (maninvalue.Trim() != "")
    //        {
    //            ds.Clear();
    //            ds = d2.BindItemHeaderWithRights(maninvalue);
    //        }
    //        else
    //        {
    //            ds.Clear();
    //            ds = d2.BindItemHeaderWithOutRights();
    //        }

    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            cbl_headername.DataSource = ds;
    //            cbl_headername.DataTextField = "itemheader_name";
    //            cbl_headername.DataValueField = "itemheader_code";
    //            cbl_headername.DataBind();


    //            if (cbl_headername.Items.Count > 0)
    //            {
    //                for (int i = 0; i < cbl_headername.Items.Count; i++)
    //                {
    //                    cbl_headername.Items[i].Selected = true;
    //                }
    //                txt_headername.Text = "Header Name(" + cbl_headername.Items.Count + ")";
    //                cb_headername.Checked = true;
    //            }
    //        }
    //        else
    //        {

    //            txt_headername.Text = "--Select--";
    //        }
    //    }
    //    catch
    //    {

    //    }
    //}

    //public void loaditem()
    //{

    //    try
    //    {

    //        ds.Clear();
    //        ds = d2.BindItemCodeWithOutParameter();


    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            cbl_itemname.DataSource = ds;
    //            cbl_itemname.DataTextField = "item_name";
    //            cbl_itemname.DataValueField = "item_code";
    //            cbl_itemname.DataBind();
    //            if (cbl_itemname.Items.Count > 0)
    //            {
    //                for (int i = 0; i < cbl_itemname.Items.Count; i++)
    //                {
    //                    cbl_itemname.Items[i].Selected = true;
    //                }
    //                txt_itemname.Text = "Item Name(" + cbl_itemname.Items.Count + ")";
    //                cb_itemname.Checked = true;
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
    //    //try
    //    //{
    //    //    cbl_itemname.Items.Clear();
    //    //    string itemheader = "";
    //    //    for (int i = 0; i < cbl_headername.Items.Count; i++)
    //    //    {
    //    //        if (cbl_headername.Items[i].Selected == true)
    //    //        {
    //    //            if (itemheader == "")
    //    //            {
    //    //                itemheader = "" + cbl_headername.Items[i].Value.ToString() + "";
    //    //            }
    //    //            else
    //    //            {
    //    //                itemheader = itemheader + "'" + "," + "" + "'" + cbl_headername.Items[i].Value.ToString() + "";
    //    //            }
    //    //        }
    //    //    }
    //    //    if (itemheader.Trim() != "")
    //    //    {
    //    //        ds.Clear();
    //    //        ds = d2.BindItemCode(itemheader);

    //    //        if (ds.Tables[0].Rows.Count > 0)
    //    //        {
    //    //            cbl_itemname.DataSource = ds;
    //    //            cbl_itemname.DataTextField = "item_name";
    //    //            cbl_itemname.DataValueField = "item_code";
    //    //            cbl_itemname.DataBind();

    //    //            //ddl_itemname1.DataSource = ds;
    //    //            //ddl_itemname1.DataTextField = "item_name";
    //    //            //ddl_itemname1.DataValueField = "item_code";
    //    //            //ddl_itemname1.DataBind();

    //    //            //ddl_itemname1.Items.Insert(0, "Select");

    //    //            if (cbl_itemname.Items.Count > 0)
    //    //            {
    //    //                for (int i = 0; i < cbl_itemname.Items.Count; i++)
    //    //                {
    //    //                    cbl_itemname.Items[i].Selected = true;
    //    //                }
    //    //                txt_itemname.Text = "Item Name(" + cbl_itemname.Items.Count + ")";
    //    //            }
    //    //            if (cbl_itemname.Items.Count > 5)
    //    //            {
    //    //                Panel1.Width = 300;
    //    //                Panel1.Height = 300;
    //    //            }
    //    //        }
    //    //        else
    //    //        {
    //    //            //ddl_itemname1.Items.Insert(0, "Select");

    //    //            //txt_itemname.Text = "--Select--";
    //    //        }
    //    //    }
    //    //    else
    //    //    {
    //    //        txt_itemname.Text = "--Select--";
    //    //    }


    //    //}
    //    //catch
    //    //{

    //    //}
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

            ds = d2.BindItemHeaderWithRights();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_headername.DataSource = ds;
                cbl_headername.DataTextField = "itemheader_name";
                cbl_headername.DataValueField = "itemheader_code";
                cbl_headername.DataBind();


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
                ds = d2.BindItemCodewithsubheader(itemheader, subheader);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_itemname.DataSource = ds;
                    cbl_itemname.DataTextField = "item_name";
                    cbl_itemname.DataValueField = "item_code";
                    cbl_itemname.DataBind();

                    ddl_itemname1.DataSource = ds;
                    ddl_itemname1.DataTextField = "item_name";
                    ddl_itemname1.DataValueField = "item_code";
                    ddl_itemname1.DataBind();
                    ddl_itemname1.Items.Insert(0, "Select");

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
                ddl_itemname1.Items.Insert(0, "Select");
                txt_itemname.Text = "--Select--";
            }
        }
        catch
        {
        }
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
    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        btn_delete.Visible = false;
        btn_update.Visible = false;
        btn_save.Visible = true;
        txt_costperunit.Text = "";
        txt_itemmeasure.Text = "";
        txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
        poperrjs.Visible = true;
        //ddlitemname();
        loadheadername();
        loaditem();


    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    protected void btn_exit_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    protected void ddl_itemname1_Change(object sender, EventArgs e)
    {
        try
        {
            string itemcode = Convert.ToString(ddl_itemname1.SelectedItem.Value);
            if (itemcode.Trim() != "")
            {
                string unit = d2.GetFunction("select item_unit  from item_master where item_code ='" + itemcode + "'");
                if (unit.Trim() != "" && unit.Trim() != "0")
                {
                    txt_itemmeasure.Text = Convert.ToString(unit);
                }
                else if (unit.Trim() != "select")
                {
                    txt_itemmeasure.Text = "";
                }
                else
                {
                }

            }

        }
        catch
        {

        }
    }
    //protected void ddlitemname()
    //{
    //    try
    //    {
    //        ddl_itemname1.Items.Clear();

    //        ds.Clear();

    //        ds = d2.BindItemCodeWithOutParameter();

    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            ddl_itemname1.DataSource = ds;
    //            ddl_itemname1.DataTextField = "item_name";
    //            ddl_itemname1.DataValueField = "item_code";
    //            ddl_itemname1.DataBind();
    //            ddl_itemname1.Items.Insert(0, "Select");
    //            //ddl_itemname1.Items.Insert(ddl_itemname1.Items.Count, "Others");
    //        }
    //        else
    //        {
    //            ddl_itemname1.Items.Insert(0, "Select");
    //            //ddl_itemname1.Items.Insert(ddl_itemname1.Items.Count, "Others");
    //        }
    //    }
    //    catch
    //    {

    //    }
    //}
    protected void btn_save_Click(object sender, EventArgs e)
    {
        savedetails();
        btn_go_Click(sender, e);
        ddl_itemname1.SelectedItem.Text = "Select";
        txt_itemmeasure.Text = "";
        txt_costperunit.Text = "";
    }
    protected void savedetails()
    {
        try
        {
            string itemcode = Convert.ToString(ddl_itemname1.SelectedItem.Value);
            itemeasure = Convert.ToString(txt_itemmeasure.Text);
            costperunit = Convert.ToString(txt_costperunit.Text);
            date = Convert.ToString(txt_date.Text);
            string[] splitdate = date.Split('-');
            splitdate = splitdate[0].Split('/');
            DateTime dt = new DateTime();
            if (splitdate.Length > 0)
            {
                dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            }
            getday = dt.ToString("MM/dd/yyyy");
            string query = "if exists (select * from ItemRateMaster where Item_Code ='" + itemcode + "' and RPUDate ='" + dt.ToString("MM/dd/yyyy") + "' )update ItemRateMaster set RPU ='" + costperunit + "' where  Item_Code ='" + itemcode + "' and RPUDate ='" + dt.ToString("MM/dd/yyyy") + "' else insert into ItemRateMaster (Item_Code,RPU,RPUDate) values ('" + itemcode + "','" + costperunit + "','" + dt.ToString("MM/dd/yyyy") + "')";
            int iv = d2.update_method_wo_parameter(query, "Text");
            if (iv != 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Saved Successfully";
                clear();

            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
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

            if (itemheadercode.Trim() != "")
            {
                string selectquery = "";

                if (txt_searchby.Text.Trim() != "")
                {
                    selectquery = "select i.item_code,i.item_name,CONVERT(varchar(10), RPUdate,103) as RPUdate,RPU from ItemRateMaster im,item_master i where i.item_code =im.item_Code and i.item_name ='" + txt_searchby.Text + "'";
                }
                else if (txt_searchitemcode.Text.Trim() != "")
                {
                    selectquery = "select i.item_code,i.item_name,CONVERT(varchar(10), RPUdate,103) as RPUdate,RPU from ItemRateMaster im,item_master i where i.item_code =im.item_Code and i.item_code ='" + txt_searchitemcode.Text + "'";
                }
                else if (txt_searchheadername.Text.Trim() != "")
                {
                    selectquery = "select i.item_code,i.item_name,CONVERT(varchar(10), RPUdate,103) as RPUdate,RPU from ItemRateMaster im,item_master i where i.item_code =im.item_Code and i.itemheader_name ='" + txt_searchheadername.Text + "'";
                }
                else
                {
                    selectquery = "select i.item_code,i.item_name,CONVERT(varchar(10), RPUdate,103) as RPUdate,RPU from ItemRateMaster im,item_master i where i.item_code =im.item_Code and i.item_code in ('" + itemheadercode + "') ";
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
                    Fpspread1.Sheets[0].ColumnCount = 5;
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

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Date";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Cost Per Unit";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;

                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        Fpspread1.Sheets[0].RowCount++;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["item_code"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["item_name"]);
                        //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["rpu"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["RPUdate"]);
                        // Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Stock_Value"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["RPU"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

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
                imgdiv2.Visible = true;

                lbl_alert.Visible = true;
                lbl_alert.Text = "Please Select Any one Item Name";
                // lbl_error.Visible = true;
                //  lbl_error.Text = "Please Select Any one Item Name";
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

    protected void Fpspread1_render(object sender, EventArgs e)
    {
        try
        {
            if (check == true)
            {

                poperrjs.Visible = true;
                btn_delete.Visible = true;
                btn_update.Visible = true;
                btn_save.Visible = false;
                string activerow = "";
                string activecol = "";
                activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                collegecode = Session["collegecode"].ToString();
                if (activerow.Trim() != "")
                {

                    string itemname = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                    string itemcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    string date = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                    string costperunit = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
                    //string costperunit = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);

                    ddl_itemname1.SelectedItem.Value = Convert.ToString(itemcode);
                    ddl_itemname1.SelectedItem.Text = Convert.ToString(itemname);
                    string itemmeasure = d2.GetFunction("select item_unit from item_master where item_name='" + itemname + "' ");
                    txt_itemmeasure.Text = itemmeasure;
                    costperunit = d2.GetFunction("select RPU from ItemRateMaster where Item_Code='" + itemcode + "' ");
                    txt_costperunit.Text = Convert.ToString(costperunit);
                    txt_date.Text = Convert.ToString(date);

                }
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
            string degreedetails = "Individual Item Cost Master Report";
            string pagename = "indivual_item_cost_master.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }

    }
    protected void btn_update_Click(object sender, EventArgs e)
    {
        try
        {
            try
            {
                string itemcode = Convert.ToString(ddl_itemname1.SelectedItem.Value);
                itemeasure = Convert.ToString(txt_itemmeasure.Text);
                costperunit = Convert.ToString(txt_costperunit.Text);
                date = Convert.ToString(txt_date.Text);
                string[] splitdate = date.Split('-');
                splitdate = splitdate[0].Split('/');
                DateTime dt = new DateTime();
                if (splitdate.Length > 0)
                {
                    dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
                }
                getday = dt.ToString("MM/dd/yyyy");
                //string[] splitdate = date.Split('/');
                //DateTime dt = new DateTime();
                //dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
                string query1 = "if exists (select * from ItemRateMaster where Item_Code ='" + itemcode + "' and RPUDate ='" + dt.ToString("MM/dd/yyyy") + "' )update ItemRateMaster set RPU ='" + costperunit + "' where  Item_Code ='" + itemcode + "' and RPUDate ='" + dt.ToString("MM/dd/yyyy") + "' else insert into ItemRateMaster (Item_Code,RPU,RPUDate) values ('" + itemcode + "','" + costperunit + "','" + dt.ToString("MM/dd/yyyy") + "')";
                int iv = d2.update_method_wo_parameter(query1, "Text");
                if (iv != 0)
                {
                    imgdiv2.Visible = true;
                    btn_go_Click(sender, e);
                    loadheadername();
                    loadsubheadername();
                    loaditem();
                    //ddlitemname();
                    lbl_alert.Text = "Updated Successfully";
                    poperrjs.Visible = false;
                }

            }
            catch
            {

            }
        }
        catch
        {

        }
    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
        btn_go_Click(sender, e);
        clear1();
        poperrjs.Visible = false;
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        imgdiv2.Visible = false;
        poperrjs.Visible = true;
    }

    public void delete()
    {
        try
        {
            surediv.Visible = false;
            string itemcode = Convert.ToString(ddl_itemname1.SelectedItem.Value);
            date = Convert.ToString(txt_date.Text);
            string[] splitdate = date.Split('-');
            splitdate = splitdate[0].Split('/');
            DateTime dt = new DateTime();
            if (splitdate.Length > 0)
            {
                dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            }
            getday = dt.ToString("MM/dd/yyyy");
            string query2 = "delete from ItemRateMaster where Item_Code ='" + itemcode + "' and RPUDate='" + getday + "'";
            int iv = d2.update_method_wo_parameter(query2, "Text");
            if (iv != 0)
            {


                loadheadername();
                loadsubheadername();
                loaditem();
                // ddlitemname();
                imgdiv2.Visible = true;
                surediv.Visible = false;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Deleted Successfully";
                //poperrjs.Visible = false;
            }
        }
        catch
        {

        }
    }
    public void clear()
    {
        btn_delete.Visible = false;
        btn_update.Visible = false;
        btn_save.Visible = true;
        txt_costperunit.Text = "";
        txt_itemmeasure.Text = "";
        txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
        poperrjs.Visible = true;
        // ddlitemname();
        loadheadername();
        loadsubheadername();
        loaditem();

    }
    public void btn_delete_Click(object sender, EventArgs e)
    {
        try
        {
            if (btn_delete.Text == "Delete")
            {
                surediv.Visible = true;
                lbl_sure.Text = "Do you want to delete this Record?";

            }
        }
        catch
        {
        }
    }
    public void clear1()
    {
        txt_costperunit.Text = "";
        txt_itemmeasure.Text = "";
        txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
        poperrjs.Visible = true;
        // ddlitemname();
        loadheadername();
        loaditem();
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
                query = "select distinct t.TextCode,t.TextVal  from TextValTable t,item_master i where t.TextCode=i.subheader_code and itemheader_code in ('" + itemheader + "') and college_code in ('" + collegecode1 + "')";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                // ds.Clear();
                // ds = d2.BindItemCodeAll(itemheader);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_subheadername.DataSource = ds;
                    cbl_subheadername.DataTextField = "TextVal";
                    cbl_subheadername.DataValueField = "TextCode";
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

}