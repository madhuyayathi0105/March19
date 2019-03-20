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

public partial class Supplier_master : System.Web.UI.Page
{
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DAccess2 da = new DAccess2();
    DAccess2 d2 = new DAccess2();
    bool check = false;
    int i = 0;
    private object sender;
    private EventArgs e;

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
            lblvalidation1.Visible = false;
            if (!IsPostBack)
            {

                txt_state.Visible = false;
                txt_search.Visible = true;
                bindvendorname();
                //rdbpaymentcash.Checked = true;
                rdbpaymentCredit.Checked = true;
                Session["contactdata"] = null;
                Session["itemdata"] = null;
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.Visible = false;
                bindstatsu();
                btn_go_Click(sender, e);
                bindstate();
                ddl_district.Items.Insert(0, "Select");
                //binddistrict();
            }
            lblvalidation1.Visible = false;
        }
        catch (Exception ex) { }
    }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }

    protected void imagebtnpopclose2_Click(object sender, EventArgs e)
    {
        popcon.Visible = false;
    }

    protected void imagebtnpopclose3_Click(object sender, EventArgs e)
    {
        popitm.Visible = false;
    }

    public void vendortype()
    {
        try
        {
            for (int i = 0; i < cbl_vendortype.Items.Count; i++)
            {
                cbl_vendortype.Items[i].Selected = true;
            }
            txt_vendortype.Text = "Status(" + cbl_vendortype.Items.Count + ")";
        }
        catch
        {
        }
    }

    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        Printcontrol.Visible = false;
        bindstate();
        bindcode();
        bindstatsu();
        btn_save.Visible = true;
        btn_update.Visible = false;
        btn_delete.Visible = false;
        poperrjs.Visible = true;
        Clear();
        txt_startyear.Text = Convert.ToString(System.DateTime.Now.ToString("yyyy"));
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
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

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname1(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select distinct VendorCompName from CO_VendorMaster WHERE VendorCompName like '" + prefixText + "%' ";
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

    public void bindstatsu()
    {
        ddlstatus.Items.Clear();
        ddlstatus.Items.Add("Select");
        ddlstatus.Items.Add("Approved");
        ddlstatus.Items.Add("Blocked");
    }

    public void bindcode()
    {
        try
        {
            string newitemcode = "";
            string selectquery = "select VenAcr,VenStNo,VenSize from IM_CodeSettings order by StartDate desc";
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string itemacronym = Convert.ToString(ds.Tables[0].Rows[0]["VenAcr"]);
                string itemstarno = Convert.ToString(ds.Tables[0].Rows[0]["VenStNo"]);
                string itemsize = Convert.ToString(ds.Tables[0].Rows[0]["VenSize"]);
                selectquery = "select distinct top(1) VendorCode,VendorPK from CO_VendorMaster where VendorCode like '" + Convert.ToString(itemacronym) + "%' order by VendorPK desc";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string itemcode = Convert.ToString(ds.Tables[0].Rows[0]["VendorCode"]);
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
                txt_code.Text = Convert.ToString(newitemcode);
            }
        }
        catch
        {

        }
    }

    protected void cb_vendorname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_vendorname.Checked == true)
            {
                for (int i = 0; i < cbl_vendorname.Items.Count; i++)
                {
                    cbl_vendorname.Items[i].Selected = true;
                }
                txt_vendorname.Text = "Vendor Name(" + (cbl_vendorname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_vendorname.Items.Count; i++)
                {
                    cbl_vendorname.Items[i].Selected = false;
                }
                txt_vendorname.Text = "--Select--";
            }
        }
        catch
        {
        }
    }

    protected void cbl_vendorname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_vendorname.Text = "--Select--";
            cb_vendorname.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_vendorname.Items.Count; i++)
            {
                if (cbl_vendorname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_vendorname.Text = "Supplier Name(" + commcount.ToString() + ")";
                if (commcount == cbl_vendorname.Items.Count)
                {
                    cb_vendorname.Checked = true;
                }
            }
        }
        catch
        {
        }
    }

    protected void cb_vendortype_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_vendortype.Checked == true)
            {
                for (int i = 0; i < cbl_vendortype.Items.Count; i++)
                {
                    cbl_vendortype.Items[i].Selected = true;
                }
                txt_vendortype.Text = "Status(" + (cbl_vendortype.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_vendortype.Items.Count; i++)
                {
                    cbl_vendortype.Items[i].Selected = false;
                }
                txt_vendortype.Text = "--Select--";
            }
        }
        catch
        {
        }
    }

    protected void cbl_vendortype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_vendortype.Text = "--Select--";
            cb_vendortype.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_vendortype.Items.Count; i++)
            {
                if (cbl_vendortype.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_vendortype.Text = "Status(" + commcount.ToString() + ")";
                if (commcount == cbl_vendortype.Items.Count)
                {
                    cb_vendortype.Checked = true;
                }
            }
        }
        catch
        {
        }
    }

    protected void btn_exit_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }

    protected void btnitm_click(object sender, EventArgs e)
    {
        bindheader();
        bind_subheader();
        cblitem.Visible = false;
        //cbl_subheader.Visible = false;
        cb_departemt.Checked = false;
        cb_conitmselect.Checked = false;
        cb_alreadysup.Checked = false;
        //binditem();
        binditemRender();//14.12.17
        binddepartment();
        //Clear();
        popitm.Visible = true;
        for (int row = 0; row < SelectdptGrid.Rows.Count; row++)
        {
            string itemcode = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_itemcode") as Label).Text);
            string deptcode = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_deptcode") as Label).Text);
            string HeaderFK = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_itemheadercode") as Label).Text);
            string Duration = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_duration") as Label).Text);
            string Supplied = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_supplied") as Label).Text);
            string reference = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_reference") as Label).Text); ;
            cb_alreadysup.Checked = false;
            txt_itmrefence.Text = "";
            txt_consup.Text = "";
            if (Supplied.ToLower() == "yes")
                cb_alreadysup.Checked = true;
            if (Duration.ToLower() != "0")
                txt_consup.Text = Duration;
            if (reference.Trim() != "")
                txt_itmrefence.Text = reference;
            if (!string.IsNullOrEmpty(HeaderFK))
                cbl_header.Items.FindByValue(HeaderFK).Selected = true;
            if (!string.IsNullOrEmpty(itemcode))
                cblitem.Items.FindByValue(itemcode).Selected = true;
            if (!string.IsNullOrEmpty(deptcode))
                cbldepartment.Items.FindByValue(deptcode).Selected = true;
        }
    }

    protected void txtyear_Onchange(object sender, EventArgs e)
    {
        int year2 = Convert.ToInt32(System.DateTime.Now.ToString("yyyy"));
        string txtyear1 = Convert.ToString(txt_startyear.Text);
        if (txtyear1.Trim() != "")
        {
            int txtyear = Convert.ToInt32(txtyear1);

            int oldyear = Convert.ToInt32(oldyeartxt.Text);
            if (oldyear <= txtyear && year2 >= txtyear)
            {

            }
            else
            {
                txt_startyear.Text = "";
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Enter Valid Year";
            }
        }
    }

    public void bindheader()
    {
        try
        {
            cbl_header.Items.Clear();
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



            // string headerquery = "";
            //if (maninvalue.Trim() != "")
            //{
            //headerquery = "select distinct ItemHeaderCode ,itemheadername  from IM_ItemMaster where ItemHeaderCode in ('" + maninvalue + "')";
            ds.Clear();
            ds = d2.BindItemHeaderWithOutRights_inv();
            //}
            //else
            //{
            //    //headerquery = "select distinct ItemHeaderCode ,itemheadername  from IM_ItemMaster";
            //    ds.Clear();
            //    ds = d2.BindItemHeaderWithOutRights();
            //}

            cbl_header.Items.Clear();
            //string statequery = "select distinct ItemHeaderCode ,itemheadername  from IM_ItemMaster where Is_Hostel ='0'";
            //ds = da.select_method_wo_parameter(headerquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_header.DataSource = ds;
                cbl_header.DataTextField = "ItemHeaderName";
                cbl_header.DataValueField = "ItemHeaderCode";
                cbl_header.DataBind();
            }





        }
        catch
        {
        }
    }

    public void bind_subheader()
    {
        try
        {
            cbl_subheader.Items.Clear();
            ds.Clear();
            string statequery = "select distinct t.MasterCode,t.MasterValue  from CO_MasterValues t,IM_ItemMaster i where t.MasterCode=i.subheader_code";
            ds = da.select_method_wo_parameter(statequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_subheader.DataSource = ds;
                cbl_subheader.DataTextField = "MasterValue";
                cbl_subheader.DataValueField = "MasterCode";
                cbl_subheader.DataBind();
            }





        }
        catch
        {
        }
    }

    public void bindvendorname()
    {
        try
        {
            ds.Clear();
            cbl_vendorname.Items.Clear();
            ds = d2.BindVendorName_inv();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_vendorname.DataSource = ds;
                cbl_vendorname.DataTextField = "VendorCompName";
                cbl_vendorname.DataValueField = "VendorCode";
                cbl_vendorname.DataBind();
                if (cbl_vendorname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_vendorname.Items.Count; i++)
                    {
                        cbl_vendorname.Items[i].Selected = true;
                    }
                    txt_vendorname.Text = "Supplier Name(" + cbl_vendorname.Items.Count + ")";
                }
                vendortype();
            }
        }
        catch
        {
        }
    }

    public void binditem()
    {
        try
        {
            string itemheadercode = "";
            for (int i = 0; i < cbl_header.Items.Count; i++)
            {
                if (cbl_header.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_header.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cbl_header.Items[i].Value.ToString() + "";
                    }
                }
            }
            ds.Clear();
            cbl_subheader.Items.Clear();

            string subheaderquery = "select distinct t.MasterCode,t.MasterValue  from CO_MasterValues t,IM_ItemMaster i where t.MasterCode=i.subheader_code and ItemHeaderCode in ('" + itemheadercode + "') and CollegeCode in ('" + collegecode1 + "') order by MasterValue";
            ds = da.select_method_wo_parameter(subheaderquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_subheader.DataSource = ds;
                cbl_subheader.DataTextField = "MasterValue";
                cbl_subheader.DataValueField = "MasterCode";
                cbl_subheader.DataBind();
                cbl_subheader.Visible = true;
            }




            //string statequery = "select ItemCode,(ItemName +'-'+ ItemModel )as ItemName  from IM_ItemMaster where ItemHeaderCode in ('" + itemheadercode + "') and ItemHeaderCode<>'' order by ItemName +'-'+ ItemModel";
            //ds = da.select_method_wo_parameter(statequery, "Text");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    cblitem.DataSource = ds;
            //    cblitem.DataTextField = "ItemName";
            //    cblitem.DataValueField = "ItemCode";
            //    cblitem.DataBind();
            //    cblitem.Visible = true;
            //}
        }
        catch
        {
        }
    }

    public void bindsubitem()//delsi
    {
        try
        {
            string itemheadercode = "";
            for (int i = 0; i < cbl_header.Items.Count; i++)
            {
                if (cbl_header.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_header.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cbl_header.Items[i].Value.ToString() + "";
                    }
                }
            }

            string sub_headercode = "";
            for (int j = 0; j < cbl_subheader.Items.Count; j++)
            {
                if (cbl_subheader.Items[j].Selected == true)
                {
                    if (sub_headercode == "")
                    {
                        sub_headercode = "" + cbl_subheader.Items[j].Value.ToString() + "";
                    }
                    else
                    {
                        sub_headercode = sub_headercode + "'" + "," + "'" + cbl_subheader.Items[j].Value.ToString() + "";
                    }
                }
            }
            //cblitem.Items.Clear();

            if (itemheadercode.Trim() != "" && sub_headercode.Trim() != "")
            {
                ds.Clear();
                ds = d2.BindItemCodewithsubheaderMaster_inv(itemheadercode, sub_headercode);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    cblitem.DataSource = ds;
                    cblitem.DataTextField = "ItemName";
                    cblitem.DataValueField = "ItemCode";
                    cblitem.DataBind();
                    cblitem.Visible = true;

                }

            }



        }
        catch (Exception e)
        {

        }

    }

    public void binddepartment()
    {
        try
        {
            string deptquery = "select Dept_Code as DeptCode ,Dept_Name as DeptName from Department where college_code ='" + collegecode1 + "' order by Dept_Code ";
            ds.Clear();
            ds = da.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbldepartment.DataSource = ds;
                cbldepartment.DataTextField = "DeptName";
                cbldepartment.DataValueField = "DeptCode";
                cbldepartment.DataBind();
            }
        }
        catch
        {
        }
    }

    protected void cbl_header_Change(object sender, EventArgs e)
    {
        try
        {
            binditem();
        }
        catch
        {
        }
    }

    protected void cbl_subheader_Change(object sender, EventArgs e)
    {
        try
        {
            bindsubitem();
        }
        catch
        {
        }
    }

    protected void cbsubheader_Change(object sender, EventArgs e)
    {
        try
        {

            if (cbl_subheader.Items.Count > 0)
            {
                if (cb_subheader.Checked == true)
                {
                    for (int i = 0; i < cbl_subheader.Items.Count; i++)
                    {
                        cbl_subheader.Items[i].Selected = true;
                    }
                }
                else
                {
                    for (int i = 0; i < cbl_subheader.Items.Count; i++)
                    {
                        cbl_subheader.Items[i].Selected = false;
                    }
                }
            }

        }
        catch
        {
        }
    }

    protected void cb_conitmselect_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (cblitem.Items.Count > 0)
            {
                if (cb_conitmselect.Checked == true)
                {
                    for (int i = 0; i < cblitem.Items.Count; i++)
                    {
                        cblitem.Items[i].Selected = true;
                    }
                }
                else
                {
                    for (int i = 0; i < cblitem.Items.Count; i++)
                    {
                        cblitem.Items[i].Selected = false;
                    }
                }
            }



        }
        catch
        {
        }
    }

    protected void cbdepartment_Change(object sender, EventArgs e)
    {
        try
        {
            if (cbldepartment.Items.Count > 0)
            {
                if (cb_departemt.Checked == true)
                {
                    for (int i = 0; i < cbldepartment.Items.Count; i++)
                    {
                        cbldepartment.Items[i].Selected = true;
                    }
                }
                else
                {
                    for (int i = 0; i < cbldepartment.Items.Count; i++)
                    {
                        cbldepartment.Items[i].Selected = false;
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void btncontact_click(object sender, EventArgs e)
    {
        popcon.Visible = true;
        txt_connam.Text = "";
        txt_conpn.Text = "";
        txt_designation.Text = "";
        txt_conmob.Text = "";
        txt_confax.Text = "";
        txt_conmail.Text = "";

        //txtconbank.Text = "";
        //txtconifsc.Text = "";
        //txtconswift.Text = "";
        //txtconbankname.Text = "";
        //txtconbankbranch.Text = "";
    }

    protected void btn_conexit_Click(object sender, EventArgs e)
    {
        popcon.Visible = false;
    }

    protected void btn_exit1_Click(object sender, EventArgs e)
    {
        popitm.Visible = false;
    }

    public void bindstate()
    {
        try
        {
            ds.Clear();
            ddl_State.Items.Clear();

            string state = " select mastercode,mastervalue from CO_MasterValues where mastercriteria='State' and CollegeCode='" + collegecode1 + "' order by MasterValue";
            ds = d2.select_method_wo_parameter(state, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_State.DataSource = ds;
                ddl_State.DataTextField = "mastervalue";
                ddl_State.DataValueField = "mastercode";
                ddl_State.DataBind();
                ddl_State.Items.Insert(0, "Select");
                //ddl_State.Items.Insert(ddl_State.Items.Count, "Others");
            }
            else
            {
                ddl_State.Items.Insert(0, "Select");
                //ddl_State.Items.Insert(ddl_State.Items.Count, "Others");
            }
        }
        catch
        {
        }
    }

    public void binddistrict()
    {
        try
        {
            ds1.Clear();
            ddl_district.Items.Clear();
            if (ddl_State.SelectedItem.Text.Trim() != "Select")
            {
                string dist = " select  mastercode,mastervalue from CO_MasterValues where mastercriteria='District' and  MasterCriteriaValue2='" + ddl_State.SelectedItem.Text + "' and CollegeCode ='" + collegecode1 + "' order by MasterValue";

                ds1 = d2.select_method_wo_parameter(dist, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    ddl_district.DataSource = ds1;
                    ddl_district.DataTextField = "mastervalue";
                    ddl_district.DataValueField = "mastercode";
                    ddl_district.DataBind();
                    ddl_district.Items.Insert(0, "Select");
                }
                else
                {
                    ddl_district.Items.Insert(0, "Select");
                }
            }
            else
            {
                ddl_district.Items.Insert(0, "Select");
            }
        }
        catch
        {
        }
    }

    protected void ddl_State_Selectindexchange(object sender, EventArgs e)
    {
        binddistrict();
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            string dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            string vendarcode = Convert.ToString(txt_code.Text);
            string type = "";
            if (rdb_vendor.Checked == true)
            {
                type = "0";
            }
            string vendorname = Convert.ToString(txt_vendorname1.Text);
            vendorname = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(vendorname);
            string street = Convert.ToString(txt_street.Text);
            string city = Convert.ToString(txt_city.Text);
            string VendorPincode = Convert.ToString(txt_pin.Text);
            string state = "";
            if (ddl_State.SelectedItem.Text != "Select")
            {
                state = Convert.ToString(ddl_State.SelectedItem.Value);
            }
            else
            {

            }
            string district = "";
            if (ddl_State.SelectedItem.Text != "Select")
            {
                if (ddl_district.SelectedItem.Text != "Select")
                {
                    district = Convert.ToString(ddl_district.SelectedItem.Value);
                }
                else
                {

                }
            }
            if (district.Trim() == "")
            {
                district = "0";
            }

            string year = Convert.ToString(txt_startyear.Text);
            string status = Convert.ToString(ddlstatus.SelectedItem.Value);

            if (status.Trim() == "Approved")
            {
                status = "1";
            }
            if (status.Trim() == "Blocked")
            {
                status = "2";
            }

            string payment = "";
            if (rdbpaymentcash.Checked == true)
            {
                payment = "0";
            }
            if (rdbpaymentCredit.Checked == true)
            {
                payment = "1";
            }
            if (rdbpaymentCheque.Checked == true)
            {
                payment = "2";
            }
            string phoneno = Convert.ToString(txt_phn.Text);
            string faxno = Convert.ToString(txtfax.Text);
            string VendorEmailIDid = Convert.ToString(txt_email.Text);
            string website = Convert.ToString(txt_web.Text);
            string cstno = Convert.ToString(txt_cst.Text);
            string tinno = Convert.ToString(txt_tin.Text);
            string panno = Convert.ToString(txt_pan.Text);
            string moblieno = Convert.ToString(txt_mainmobileno.Text);

            string bankacno = Convert.ToString(txtconbank.Text);
            string ifsccode = Convert.ToString(txtconifsc.Text);
            string swiftcode = Convert.ToString(txtconswift.Text);
            string bankername = Convert.ToString(txtconbankname.Text);
            string bankderbranch = Convert.ToString(txtconbankbranch.Text);

            int cont = 0;
            int va = 0;
            int inst = 0;
            if (ChkLibrary.Checked == false)
            {
                if (SelectdptGrid.Rows.Count > 0 && ContactGrid.Rows.Count > 0)
                {
                    if (SelectdptGrid.Rows.Count > 0)
                    {
                        string insertquery = "insert into CO_VendorMaster (VendorCode,VendorCompName,VendorAddress,VendorPin,VendorPhoneNo,VendorFaxNo,VendorEmailID,VendorType,VendorTINNo,VendorCSTNo,VendorWebsite,VendorCity,VendorDist,VendorState,VendorStartYear,VendorPANNo,VendorPayType,VendorStatus,VendorMobileNo)values('" + vendarcode + "','" + vendorname + "','" + street + "','" + VendorPincode + "','" + phoneno + "','" + faxno + "','" + VendorEmailIDid + "','1','" + tinno + "','" + cstno + "','" + website + "','" + city + "','" + district + "','" + state + "','" + year + "','" + panno + "','" + payment + "','" + status + "','" + moblieno + "')";
                        inst = da.update_method_wo_parameter(insertquery, "Text");

                        string venfk = d2.getvenpk(vendarcode);
                        if (txtconbankname.Text.Trim() != "" && txtconbank.Text.Trim() != "")
                        {
                            string insertbank = "insert into IM_VendorBankMaster (VendorAccNo,VendorBankIFSCCode,VendorBankSWIFTCode,VenBankName,VenBankBranch,vendorfk) values('" + bankacno + "','" + ifsccode + "','" + swiftcode + "','" + bankername + "','" + bankderbranch + "','" + venfk + "')";
                            int venbank = d2.update_method_wo_parameter(insertbank, "Text");
                        }
                        for (int row = 0; row < SelectdptGrid.Rows.Count; row++)
                        {
                            string itemcode = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_itemcode") as Label).Text);
                            string deptcode = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_deptcode") as Label).Text);
                            string Duration = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_duration") as Label).Text);
                            if (Duration.Trim() == "")
                            {
                                Duration = "0";
                            }
                            string Refence = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_reference") as Label).Text);
                            string supplied = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_supplied") as Label).Text);
                            if (supplied.Trim() == "Yes")
                            {
                                supplied = "1";
                            }
                            else
                            {
                                supplied = "0";
                            }
                            if (deptcode.Trim() == "")
                            {
                                deptcode = "0";
                            }
                            string vendoritemfk = d2.getvenpk(vendarcode);
                            string itemPkvalue = d2.getitempk(itemcode);
                            string intquery = "insert into IM_VendorItemDept (VenItemFK,VenItemDeptFK,VenItemSupplyDur,VenItemIsSupplied,VenItemReference,ItemFK) values ('" + vendoritemfk + "','" + deptcode + "','" + Duration + "','" + supplied + "','" + Refence + "','" + itemPkvalue + "')";
                            va = da.update_method_wo_parameter(intquery, "Text");
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Please select item details";
                    }
                    if (ContactGrid.Rows.Count > 0)
                    {
                        for (int row = 0; row < ContactGrid.Rows.Count; row++)
                        {
                            string contactname = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_name") as Label).Text);
                            string designame = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_designation") as Label).Text);
                            string contactphone = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_phoneno") as Label).Text);
                            string contactmobile = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_mobileno") as Label).Text);
                            string contactfaxno = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_faxno") as Label).Text);
                            string contactVendorEmailID = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_email") as Label).Text);
                            string vendoritemfk = d2.getvenpk(vendarcode);
                            string vquery = "insert into  IM_VendorContactMaster (VenContactName,VenContactDesig,VendorPhoneNo,VendorMobileNo,VendorExtNo,VendorEmail,VendorFK)";
                            vquery = vquery + "  values ('" + contactname + "','" + designame + "','" + contactphone + "','" + contactmobile + "','" + contactfaxno + "','" + contactVendorEmailID + "','" + vendoritemfk + "')";
                            cont = da.update_method_wo_parameter(vquery, "Text");
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Please select contact details";
                    }
                }
                else if (SelectdptGrid.Rows.Count == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Please select item details";
                }
                else if (ContactGrid.Rows.Count == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Please select contact details";
                }
                if (va != 0 && inst != 0 && cont != 0)
                {
                    bindvendorname();
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Saved Successfully";
                    btn_addnew_Click(sender, e);
                    btn_go_Click(sender, e);
                    poperrjs.Visible = true;
                    Session["contactdata"] = null;

                    SelectdptGrid.DataSource = null;
                    SelectdptGrid.DataBind();
                    ContactGrid.DataSource = null;
                    ContactGrid.DataBind();
                }
            }
            if (ChkLibrary.Checked == true)
            {
                if (ContactGrid.Rows.Count > 0)
                {
                    string SupplierType = Convert.ToString(ddlSupplierType.SelectedItem.Text);
                    string EmailID1 = Convert.ToString(Txtemailid1.Text);
                    string EmailID2 = Convert.ToString(Txtemailid2.Text);
                    string Address2 = Convert.ToString(TxtAddress2.Text);
                    string insertqueryLib = "insert into CO_VendorMaster (VendorCode,VendorCompName,VendorAddress,VendorPin,VendorPhoneNo,VendorFaxNo,VendorEmailID,VendorType,VendorTINNo,VendorCSTNo,VendorWebsite,VendorCity,VendorDist,VendorState,VendorStartYear,VendorPANNo,VendorPayType,VendorStatus,VendorMobileNo,SupplierType,EmailID1,EmailID2,Address2,LibraryFlag)values('" + vendarcode + "','" + vendorname + "','" + street + "','" + VendorPincode + "','" + phoneno + "','" + faxno + "','" + VendorEmailIDid + "','1','" + tinno + "','" + cstno + "','" + website + "','" + city + "','" + district + "','" + state + "','" + year + "','" + panno + "','" + payment + "','','" + moblieno + "','" + SupplierType + "','" + EmailID1 + "','" + EmailID2 + "','" + Address2 + "','1')";//" + status + "
                    inst = da.update_method_wo_parameter(insertqueryLib, "Text");

                    string venfk = d2.getvenpk(vendarcode);

                    if (txtconbankname.Text.Trim() != "" && txtconbank.Text.Trim() != "")
                    {
                        string insertbank = "insert into IM_VendorBankMaster (VendorAccNo,VendorBankIFSCCode,VendorBankSWIFTCode,VenBankName,VenBankBranch,vendorfk) values('" + bankacno + "','" + ifsccode + "','" + swiftcode + "','" + bankername + "','" + bankderbranch + "','" + venfk + "')";
                        int venbank = d2.update_method_wo_parameter(insertbank, "Text");
                    }
                    if (ContactGrid.Rows.Count > 0)
                    {
                        for (int row = 0; row < ContactGrid.Rows.Count; row++)
                        {
                            string contactname = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_name") as Label).Text);
                            string designame = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_designation") as Label).Text);
                            string contactphone = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_phoneno") as Label).Text);
                            string contactmobile = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_mobileno") as Label).Text);
                            string contactfaxno = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_faxno") as Label).Text);
                            string contactVendorEmailID = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_email") as Label).Text);
                            string vendoritemfk = d2.getvenpk(vendarcode);
                            string vquery = "insert into  IM_VendorContactMaster (VenContactName,VenContactDesig,VendorPhoneNo,VendorMobileNo,VendorExtNo,VendorEmail,VendorFK)";
                            vquery = vquery + "  values ('" + contactname + "','" + designame + "','" + contactphone + "','" + contactmobile + "','" + contactfaxno + "','" + contactVendorEmailID + "','" + vendoritemfk + "')";
                            cont = da.update_method_wo_parameter(vquery, "Text");
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Please select contact details";
                    }
                }
                else if (ContactGrid.Rows.Count == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Please select contact details";
                }
                if (inst != 0 && cont != 0)
                {
                    bindvendorname();
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Saved Successfully";
                    btn_addnew_Click(sender, e);
                    btn_go_Click(sender, e);
                    poperrjs.Visible = true;
                    Session["contactdata"] = null;

                    SelectdptGrid.DataSource = null;
                    SelectdptGrid.DataBind();
                    ContactGrid.DataSource = null;
                    ContactGrid.DataBind();
                }
            }
        }
        catch
        {
        }
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            int index;
            string colno = "";
            int j = 0;
            Printcontrol.Visible = false;
            Hashtable columnhash = new Hashtable();
            columnhash.Add("VendorCode", "Supplier Code");
            columnhash.Add("VendorCompName", "Company Name");
            columnhash.Add("VendorAddress", "Street");
            columnhash.Add("VendorCity", "City");
            columnhash.Add("VendorPin", "Pincode");
            columnhash.Add("VendorPhoneNo", "Phone No");
            columnhash.Add("VendorFaxNo", "Fax No");
            columnhash.Add("VendorEmailID", "Mail Id");
            columnhash.Add("VendorStatus", "Status");
            columnhash.Add("VendorWebsite", "Website");
            columnhash.Add("VendorDist", "District");
            columnhash.Add("VendorState", "State");
            columnhash.Add("VendorMobileNo", "Mobile No");
            columnhash.Add("VendorCSTNo", "CST No");
            columnhash.Add("VendorPANNo", "PAN");
            columnhash.Add("VendorTINNo", "TIN");
            columnhash.Add("VendorStartYear", "Business Start Year");
            columnhash.Add("VendorPayType", "Payment Type");
            //columnhash.Add("Status", "Status");
            columnhash.Add("VenBankName", "Bank Name");
            columnhash.Add("VenBankBranch", "Bank Branch");
            columnhash.Add("VendorBankIFSCCode", "IFSC Code");
            columnhash.Add("VendorAccNo", "Bank A/C No");
            columnhash.Add("VendorBankSWIFTCode", "SWIFT Code");

            if (ItemList.Count == 0)
            {
                ItemList.Add("VendorCode");
                ItemList.Add("VendorCompName");
                ItemList.Add("VendorAddress");
            }

            string itemheadercode = "";
            for (int i = 0; i < cbl_vendorname.Items.Count; i++)
            {
                if (cbl_vendorname.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_vendorname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cbl_vendorname.Items[i].Value.ToString() + "";
                    }
                }
            }
            string itemheadercode1 = "";
            for (int i = 0; i < cbl_vendortype.Items.Count; i++)
            {
                if (cbl_vendortype.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode1 = "" + cbl_vendortype.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode1 = itemheadercode1 + "'" + "," + "'" + cbl_vendortype.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (txt_vendorname.Text.Trim() != "--Select--" && txt_vendortype.Text.Trim() != "--Select--")
            {

                if (itemheadercode.Trim() != "" && itemheadercode1.Trim() != "")
                {
                    string selectqurey = "";
                    if (txt_search.Text.Trim() != "")
                    {
                        selectqurey = "select distinct vm.VendorCode,im.ItemCode,im.ItemName,VendorStartYear,VendorCompName,VendorAddress,VendorPin,VendorMobileNo,VendorPhoneNo,VendorFaxNo,VendorEmailID,VendorType,VendorTINNo,VendorCSTNo,VendorCity,VendorDist,VendorState,VendorWebsite,bm.VendorAccNo,bm.VendorBankIFSCCode,bm.VendorBankSWIFTCode,bm.VenBankName,bm.VenBankBranch,case when VendorPayType=1 then 'Credit' when VendorPayType=0 then 'Cash' when VendorPayType=2 then 'Cheque' end VendorPayType, case when VendorStatus=1 then 'Approved' when VendorStatus=2 then 'Blocked'  end VendorStatus from CO_VendorMaster vm,IM_VendorItemDept vi,IM_ItemMaster im,IM_VendorBankMaster bm where vm.vendorpk=vi.venitemfk and  vi.itemfk =im.itempk and im.ItemName  ='" + Convert.ToString(txt_search.Text) + "' ";//and bm.VendorFK=vm.VendorPK
                    }
                    else if (txt_vendorname2.Text.Trim() != "")
                    {
                        selectqurey = "select distinct vm.vendorpk, VendorCode,VendorCompName,VendorAddress,VendorPin,VendorPhoneNo,VendorFaxNo,VendorEmailID,VendorMobileNo, VendorType,VendorTINNo,VendorCSTNo,VendorWebsite,VendorCity,VendorDist,VendorState,VendorStartYear,VendorPANNo,case when VendorPayType=1 then 'Credit' when VendorPayType=0 then 'Cash' when VendorPayType=2 then 'Cheque' end VendorPayType,case when VendorStatus=1 then 'Approved' when VendorStatus=2 then 'Blocked' end VendorStatus,bm.VendorAccNo,bm.VendorBankIFSCCode,bm.VendorBankSWIFTCode,bm.VenBankName,bm.VenBankBranch from CO_VendorMaster vm,IM_VendorBankMaster bm where VendorCompName='" + Convert.ToString(txt_vendorname2.Text) + "' and bm.VendorFK=vm.VendorPK and ISNULL(LibraryFlag,0)<>'1'";
                        selectqurey = selectqurey + " union all select distinct vm.vendorpk, VendorCode,VendorCompName,VendorAddress,VendorPin,VendorPhoneNo,VendorFaxNo,VendorEmailID,VendorMobileNo, VendorType,VendorTINNo,VendorCSTNo,VendorWebsite,VendorCity,VendorDist,VendorState,VendorStartYear,VendorPANNo,case when VendorPayType=1 then 'Credit' when VendorPayType=0 then 'Cash' when VendorPayType=2 then 'Cheque' end VendorPayType,case when VendorStatus=1 then 'Approved' when VendorStatus=2 then 'Blocked' end VendorStatus,'','','','','' from CO_VendorMaster vm where VendorCompName='" + Convert.ToString(txt_vendorname2.Text) + "' and LibraryFlag='1'  ";
                    }
                    else
                    {
                        selectqurey = "select distinct vm.vendorpk ,VendorCode,VendorCompName,VendorAddress,VendorPin,VendorPhoneNo,VendorFaxNo,VendorMobileNo,VendorEmailID,VendorType,VendorTINNo,VendorCSTNo,VendorWebsite,VendorCity,VendorDist,VendorState,VendorStartYear,VendorPANNo,case when VendorPayType=1 then 'Credit' when VendorPayType=0 then 'Cash' when VendorPayType=2 then 'Cheque' end VendorPayType, case when VendorStatus=1 then 'Approved' when VendorStatus=2 then 'Blocked' end VendorStatus, bm.VendorAccNo,bm.VendorBankIFSCCode,bm.VendorBankSWIFTCode,bm.VenBankName,bm.VenBankBranch from  CO_VendorMaster vm,IM_VendorBankMaster bm where VendorCode in('" + itemheadercode + "') and VendorType in ('" + itemheadercode1 + "') and bm.VendorFK =vm.VendorPK and ISNULL(LibraryFlag,0)<>'1' ";//and bm.VendorFK =vm.VendorPK
                        //Modified by saranya
                        selectqurey = selectqurey + "union all select distinct vm.vendorpk ,VendorCode,VendorCompName,VendorAddress,VendorPin,VendorPhoneNo,VendorFaxNo,VendorMobileNo,VendorEmailID,VendorType,VendorTINNo,VendorCSTNo,VendorWebsite,VendorCity,VendorDist,VendorState,VendorStartYear,VendorPANNo,case when VendorPayType=1 then 'Credit' when VendorPayType=0 then 'Cash' when VendorPayType=2 then 'Cheque' end VendorPayType, case when VendorStatus=1 then 'Approved' when VendorStatus=2 then 'Blocked' end VendorStatus, '','','','','' from  CO_VendorMaster vm,IM_VendorBankMaster bm where VendorCode in('" + itemheadercode + "') and VendorType in ('" + itemheadercode1 + "') and LibraryFlag='1'  order by VendorCode";
                    }
                    ds.Clear();
                    ds = da.select_method_wo_parameter(selectqurey, "Text");
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
                        for (j = 0; j < ds.Tables[0].Columns.Count; j++)
                        {
                            colno = Convert.ToString(ds.Tables[0].Columns[j]);
                            if (ItemList.Contains(Convert.ToString(colno)))
                            {
                                index = ItemList.IndexOf(Convert.ToString(colno));
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Text = Convert.ToString(columnhash[colno]);
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Bold = true;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                        for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Fpspread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                            Fpspread1.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;
                            for (j = 0; j < ds.Tables[0].Columns.Count; j++)
                            {
                                if (ItemList.Contains(Convert.ToString(ds.Tables[0].Columns[j].ToString())))
                                {
                                    index = ItemList.IndexOf(Convert.ToString(ds.Tables[0].Columns[j].ToString()));
                                    Fpspread1.Sheets[0].Columns[index + 1].Width = 150;
                                    Fpspread1.Sheets[0].Columns[index + 1].Locked = true;
                                    Fpspread1.Sheets[0].Cells[i, index + 1].CellType = txt;
                                    Fpspread1.Sheets[0].Cells[i, index + 1].Text = ds.Tables[0].Rows[i][j].ToString();
                                    Fpspread1.Sheets[0].Cells[i, index + 1].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].Cells[i, index + 1].Font.Size = FontUnit.Medium;
                                    string colunms = Convert.ToString(ds.Tables[0].Columns[j].ToString());
                                    if (colunms == "VendorDist")
                                    {
                                        string district = d2.GetFunction("select mastervalue from co_mastervalues where mastercriteria='district' and mastercode='" + Convert.ToString(ds.Tables[0].Rows[i][j].ToString()) + "'");
                                        if (district.Trim() == "0")
                                        {
                                            district = "";
                                        }
                                        Fpspread1.Sheets[0].Columns[index + 1].Width = 150;
                                        Fpspread1.Sheets[0].Columns[index + 1].Locked = true;
                                        Fpspread1.Sheets[0].Cells[i, index + 1].CellType = txt;
                                        Fpspread1.Sheets[0].Cells[i, index + 1].Text = district;
                                        Fpspread1.Sheets[0].Cells[i, index + 1].Font.Name = "Book Antiqua";
                                        Fpspread1.Sheets[0].Cells[i, index + 1].Font.Size = FontUnit.Medium;
                                    }
                                    if (colunms == "VendorState")
                                    {
                                        string state = d2.GetFunction("select mastervalue from co_mastervalues where mastercriteria='State' and mastercode='" + Convert.ToString(ds.Tables[0].Rows[i][j].ToString()) + "'");
                                        if (state.Trim() == "0")
                                        {
                                            state = "";
                                        }
                                        Fpspread1.Sheets[0].Columns[index + 1].Width = 150;
                                        Fpspread1.Sheets[0].Columns[index + 1].Locked = true;
                                        Fpspread1.Sheets[0].Cells[i, index + 1].CellType = txt;
                                        Fpspread1.Sheets[0].Cells[i, index + 1].Text = state;
                                        Fpspread1.Sheets[0].Cells[i, index + 1].Font.Name = "Book Antiqua";
                                        Fpspread1.Sheets[0].Cells[i, index + 1].Font.Size = FontUnit.Medium;
                                    }
                                }
                            }
                        }
                        Fpspread1.Visible = true;
                        rptprint.Visible = true;
                        div1.Visible = true;
                        lbl_error.Visible = false;
                        pcolumnorder.Visible = true;
                        pheaderfilter.Visible = true;
                        Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    }
                    else
                    {
                        Fpspread1.Visible = false;
                        rptprint.Visible = false;
                        div1.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "No Records Found";
                        pcolumnorder.Visible = false;
                        pheaderfilter.Visible = false;
                    }
                }
                else
                {
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                    div1.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "No Records Founds";
                    pcolumnorder.Visible = false;
                    pheaderfilter.Visible = false;
                }
            }
            else
            {
                Fpspread1.Visible = false;
                rptprint.Visible = false;
                div1.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Please select all fields";
                pcolumnorder.Visible = false;
                pheaderfilter.Visible = false;
            }
            txt_search.Text = "";
            txt_vendorname2.Text = "";
        }
        catch
        {
        }
    }

    public void cb_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string si = "";
            int j = 0;
            if (cb_column.Checked == true)
            {
                ItemList.Clear();
                for (i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    si = Convert.ToString(i);
                    cblcolumnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                    ItemList.Add(cblcolumnorder.Items[i].Value.ToString());
                    Itemindex.Add(si);
                }
                lnk_columnorder.Visible = true;
                tborder.Visible = true;
                tborder.Text = "";
                for (i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    tborder.Text = tborder.Text + ItemList[i].ToString();
                    tborder.Text = tborder.Text + "(" + (j).ToString() + ")  ";
                }
            }
            else
            {
                for (i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                    lnk_columnorder.Visible = false;
                    ItemList.Clear();
                    Itemindex.Clear();
                    cblcolumnorder.Items[0].Enabled = false;
                }
                tborder.Text = "";
                tborder.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void lb_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder.ClearSelection();
            cb_column.Checked = false;
            lnk_columnorder.Visible = false;
            ItemList.Clear();
            Itemindex.Clear();
            tborder.Text = "";
            tborder.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    public void cbl_columnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index;
            string value = "";
            string result = "";
            string sindex = "";
            cb_column.Checked = false;
            cblcolumnorder.Items[0].Selected = true;
            cblcolumnorder.Items[0].Enabled = false;
            value = string.Empty;
            result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            sindex = Convert.ToString(index);
            if (cblcolumnorder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    ItemList.Add(cblcolumnorder.Items[index].Value.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cblcolumnorder.Items[index].Value.ToString());
                Itemindex.Remove(sindex);
            }
            for (i = 0; i < cblcolumnorder.Items.Count; i++)
            {
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
            for (i = 0; i < ItemList.Count; i++)
            {
                tborder.Text = tborder.Text + ItemList[i].ToString();

                tborder.Text = tborder.Text + "(" + (i + 1).ToString() + ")  ";

            }
            if (ItemList.Count == 22)
            {
                cb_column.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                tborder.Visible = false;
                lnk_columnorder.Visible = false;
            }
        }
        catch (Exception ex)
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
                bindstate();
                bindstatsu();
                poperrjs.Visible = true;
                btn_save.Visible = false;
                btn_update.Visible = true;
                btn_delete.Visible = true;
                string activerow = "";
                string activecol = "";
                activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                collegecode = Session["collegecode"].ToString();
                for (int i = 0; i < Fpspread1.Sheets[0].RowCount; i++)
                {
                    if (i == Convert.ToInt32(activerow))
                    {
                        Fpspread1.Sheets[0].Rows[i].BackColor = Color.LightBlue;
                        Fpspread1.Sheets[0].SelectionBackColor = Color.LightBlue;
                    }
                    else
                    {
                        Fpspread1.Sheets[0].Rows[i].BackColor = Color.White;
                    }
                }
                if (activerow.Trim() != "")
                {
                    string vendorcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    string vendorpk = d2.GetFunction("select VendorPK FROM CO_VendorMaster where VendorCode='" + vendorcode + "'");
                    string selectquery = "select VendorPK,VendorCode,VendorCompName,VendorType,VendorAddress,VendorStreet,VendorCity,VendorDist,VendorState,VendorPin,VendorPhoneNo,VendorFaxNo,VendorEmailID,VendorWebsite,VendorStartYear,case when VendorPayType=1 then 'Credit' when VendorPayType=0 then 'Cash' when VendorPayType=2 then 'Cheque' end VendorPayType,VendorStatus,VendorBlockFrom,VendorBlockTo,VendorCSTNo,VendorTINNo,VendorPANNo,IdentityType,IdentityNo,VendorMobileNo,TypeofMagazine,VendorName,VendorCountry  from CO_VendorMaster where  VendorCode='" + vendorcode + "'";
                    selectquery = selectquery + " select itemheadername,ItemHeaderCode,i.ItemCode,ItemName,v.VenItemDeptFK,VenItemSupplyDur,VenItemIsSupplied,VenItemReference from IM_VendorItemDept v ,IM_ItemMaster i,CO_VendorMaster vm where v.itemfk=i.itempk and v.venitemfk=vm.vendorpk and vm.vendorcode ='" + vendorcode + "'";
                    selectquery = selectquery + " select vc.VenContactName,vc.VenContactDesig,vc.VendorPhoneNo,vc.VendorMobileNo,vc.VendorExtNo,vc.VendorEmail from IM_VendorContactMaster vc where VendorFK='" + vendorpk + "'";

                    selectquery = selectquery + " select bm.VenBankName,bm.VenBankBranch,bm.VenBankHolderName,bm.VendorAccName,bm.VendorAccNo,bm.VendorBankIFSCCode,bm.VendorBankSWIFTCode,bm.VendorFK from IM_VendorBankMaster bm where VendorFK='" + vendorpk + "'";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txt_code.Text = Convert.ToString(vendorcode);
                        txt_vendorname1.Text = Convert.ToString(ds.Tables[0].Rows[0]["VendorCompName"]);
                        txt_street.Text = Convert.ToString(ds.Tables[0].Rows[0]["VendorAddress"]);
                        txt_city.Text = Convert.ToString(ds.Tables[0].Rows[0]["VendorCity"]);
                        txt_pin.Text = Convert.ToString(ds.Tables[0].Rows[0]["VendorPin"]);

                        string statecode = Convert.ToString(ds.Tables[0].Rows[0]["vendorstate"]);
                        if (statecode.Trim() != "")
                        {
                            string State = d2.GetFunction("select mastervalue from CO_MasterValues where mastercode='" + statecode + "' and MasterCriteria='State'");

                            ddl_State.SelectedIndex = ddl_State.Items.IndexOf(ddl_State.Items.FindByText(State));
                            binddistrict();
                        }
                        string district = Convert.ToString(ds.Tables[0].Rows[0]["VendorDist"]);
                        if (district.Trim() != "")
                        {
                            district = d2.GetFunction("select mastervalue from CO_MasterValues where mastercode='" + district + "' and MasterCriteria='District'");

                            ddl_district.SelectedIndex = ddl_district.Items.IndexOf(ddl_district.Items.FindByText(district));
                        }
                        txt_startyear.Text = Convert.ToString(ds.Tables[0].Rows[0]["VendorStartYear"]);
                        string vtype = Convert.ToString(ds.Tables[0].Rows[0]["VendorStatus"]);

                        if (vtype.Trim() == "1")
                        {
                            ddlstatus.SelectedIndex = 1;
                        }
                        else if (vtype.Trim() == "2")
                        {
                            ddlstatus.SelectedIndex = 2;
                        }
                        else
                        {
                            ddlstatus.SelectedIndex = 0;
                        }

                        string payment = Convert.ToString(ds.Tables[0].Rows[0]["VendorPayType"]);
                        if (payment == "Cash")
                        {
                            rdbpaymentcash.Checked = true;
                            rdbpaymentCredit.Checked = false;
                            rdbpaymentCheque.Checked = false;
                        }
                        else if (payment == "Credit")
                        {
                            rdbpaymentCredit.Checked = true;
                            rdbpaymentcash.Checked = false;
                            rdbpaymentCheque.Checked = false;
                        }
                        else
                        {
                            rdbpaymentCheque.Checked = true;
                            rdbpaymentCredit.Checked = false;
                            rdbpaymentcash.Checked = false;
                        }
                        rdb_vendor.Checked = true;
                        txt_phn.Text = Convert.ToString(ds.Tables[0].Rows[0]["VendorPhoneNo"]);
                        txtfax.Text = Convert.ToString(ds.Tables[0].Rows[0]["VendorFaxNo"]);
                        txt_email.Text = Convert.ToString(ds.Tables[0].Rows[0]["VendorEmailID"]);
                        txt_web.Text = Convert.ToString(ds.Tables[0].Rows[0]["VendorWebsite"]);
                        txt_cst.Text = Convert.ToString(ds.Tables[0].Rows[0]["VendorCSTNo"]);
                        txt_tin.Text = Convert.ToString(ds.Tables[0].Rows[0]["VendorTINNo"]);
                        txt_pan.Text = Convert.ToString(ds.Tables[0].Rows[0]["VendorPANNo"]);
                        txt_mainmobileno.Text = Convert.ToString(ds.Tables[0].Rows[0]["VendorMobileNo"]);
                        txtconbank.Text = Convert.ToString(ds.Tables[3].Rows[0]["VendorAccNo"]);
                        txtconifsc.Text = Convert.ToString(ds.Tables[3].Rows[0]["VendorBankIFSCCode"]);
                        txtconswift.Text = Convert.ToString(ds.Tables[3].Rows[0]["VendorBankSWIFTCode"]);
                        txtconbankname.Text = Convert.ToString(ds.Tables[3].Rows[0]["VenBankName"]);
                        txtconbankbranch.Text = Convert.ToString(ds.Tables[3].Rows[0]["VenBankBranch"]);
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        DataView dv = new DataView();
                        DataRow dr;
                        dt.Columns.Add("Item Header");
                        dt.Columns.Add("Item Headercode");
                        dt.Columns.Add("Item Code");
                        dt.Columns.Add("Item Name");
                        dt.Columns.Add("Dept Name");
                        dt.Columns.Add("Dept Code");
                        dt.Columns.Add("Duration");
                        dt.Columns.Add("Supplied");
                        dt.Columns.Add("Reference");
                        DataTable d1 = new DataTable();
                        string supply = "";
                        bool niceflage = false;
                        d1 = ds.Tables[1];
                        if (d1.Rows.Count > 0)
                        {
                            for (int r = 0; r < d1.Rows.Count; r++)
                            {
                                dr = dt.NewRow();
                                niceflage = false;
                                for (int c = 0; c < d1.Columns.Count; c++)
                                {
                                    string columname = Convert.ToString(d1.Columns[c].ColumnName);
                                    ///02.10.15
                                    if (columname == "VenItemIsSupplied")
                                    {
                                        supply = Convert.ToString(d1.Rows[r][c]);
                                        if (supply.Trim() == "false")
                                        {
                                            dr[c] = Convert.ToString("No");
                                        }
                                        else
                                        {
                                            dr[c + 1] = Convert.ToString("yes");
                                        }

                                    }
                                    else if (columname.Trim() != "VenItemDeptFK")
                                    {
                                        if (niceflage != true)
                                        {
                                            dr[c] = Convert.ToString(d1.Rows[r][c]);
                                        }
                                        else
                                        {
                                            dr[c + 1] = Convert.ToString(d1.Rows[r][c]);
                                        }
                                    }
                                    else
                                    {
                                        niceflage = true;
                                        string vlaue = Convert.ToString(d1.Rows[r][c]);
                                        if (vlaue.Trim() != "0")
                                        {
                                            string getdeptname = da.GetFunction("select Dept_Name  from Department where Dept_Code ='" + vlaue + "'");
                                            if (getdeptname.Trim() != "" && getdeptname.Trim() != "0")
                                            {
                                                dr[c] = Convert.ToString(getdeptname);
                                                dr["Dept Code"] = vlaue;
                                            }
                                        }
                                        else
                                        {
                                            dr[c] = "";
                                        }
                                    }
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                        if (dt.Rows.Count > 0)
                        {
                            SelectdptGrid.DataSource = dt;
                            SelectdptGrid.DataBind();
                            SelectdptGrid.Visible = true;
                        }
                    }
                    else
                    {
                        SelectdptGrid.Visible = true;
                    }
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        DataRow dr;
                        dt.Columns.Add("Name");
                        dt.Columns.Add("Designation");
                        dt.Columns.Add("Phone");
                        dt.Columns.Add("Mobile No");
                        dt.Columns.Add("Fax No");
                        dt.Columns.Add("Email");
                        DataTable d1 = new DataTable();
                        d1 = ds.Tables[2];
                        if (d1.Rows.Count > 0)
                        {
                            for (int r = 0; r < d1.Rows.Count; r++)
                            {
                                dr = dt.NewRow();
                                for (int c = 0; c < d1.Columns.Count; c++)
                                {
                                    dr[c] = Convert.ToString(d1.Rows[r][c]);
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                        if (dt.Rows.Count > 0)
                        {
                            ContactGrid.DataSource = dt;
                            ContactGrid.DataBind();
                            ContactGrid.Visible = true;
                        }
                    }
                    else
                    {
                        ContactGrid.Visible = true;
                    }
                }
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
            string degreedetails = "Supplier Master Report";
            string pagename = "vendor.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
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
                da.printexcelreport(Fpspread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please enter the report name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
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

    protected void ddl_contyp1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_contyp.SelectedItem.ToString() == "Others")
        {
            txt_contyp.Visible = true;
        }
        else
        {
            txt_contyp.Visible = false;
        }
    }

    protected void btn_congo_Click(object sender, EventArgs e)
    {
        try
        {
            if (btn_congo.Text == "Save")
            {
                DataTable dt = new DataTable();
                DataView dv = new DataView();
                DataRow dr;
                dt.Columns.Add("Name");
                dt.Columns.Add("Designation");
                dt.Columns.Add("Phone");
                dt.Columns.Add("Mobile No");
                dt.Columns.Add("Fax No");
                dt.Columns.Add("Email");
                bool chk = false;
                if (Session["contactdata"] != null)
                {
                    DataTable d1 = new DataTable();
                    d1 = (DataTable)Session["contactdata"];
                    if (d1.Rows.Count > 0)
                    {
                        for (int r = 0; r < d1.Rows.Count; r++)
                        {
                            string name = d1.Rows[0]["Name"].ToString();
                            dr = dt.NewRow();
                            for (int c = 0; c < d1.Columns.Count; c++)
                            {
                                //if (name == Convert.ToString(d1.Rows[r][0]))
                                //{

                                //}
                                //else
                                //{
                                dr[c] = Convert.ToString(d1.Rows[r][c]);
                                //}
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }
                dr = dt.NewRow();
                dr[0] = Convert.ToString(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(txt_connam.Text));
                dr[1] = Convert.ToString(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(txt_designation.Text));
                dr[2] = Convert.ToString(txt_conpn.Text);
                dr[3] = Convert.ToString(txt_conmob.Text);
                dr[4] = Convert.ToString(txt_confax.Text);
                dr[5] = Convert.ToString(txt_conmail.Text);

                dt.Rows.Add(dr);
                if (dt.Rows.Count > 0)
                {
                    ContactGrid.DataSource = dt;
                    ContactGrid.DataBind();
                    ContactGrid.Visible = true;
                    Session["contactdata"] = dt;
                    popcon.Visible = false;
                }
                else
                {
                    popcon.Visible = true;
                }
            }
            else
            {
                string name = Convert.ToString(txt_connam.Text);
                string desing = Convert.ToString(txt_designation.Text);
                string ph = Convert.ToString(txt_conpn.Text);
                string mob = Convert.ToString(txt_conmob.Text);
                string fax = Convert.ToString(txt_confax.Text);
                string VendorEmailID = Convert.ToString(txt_conmail.Text);
                int row = Convert.ToInt32(Session["row_new_Value"]);
                (ContactGrid.Rows[row].FindControl("lbl_name") as Label).Text = Convert.ToString(name);
                (ContactGrid.Rows[row].FindControl("lbl_designation") as Label).Text = Convert.ToString(desing);
                (ContactGrid.Rows[row].FindControl("lbl_phoneno") as Label).Text = Convert.ToString(ph);
                (ContactGrid.Rows[row].FindControl("lbl_mobileno") as Label).Text = Convert.ToString(mob);
                (ContactGrid.Rows[row].FindControl("lbl_faxno") as Label).Text = Convert.ToString(fax);
                (ContactGrid.Rows[row].FindControl("lbl_email") as Label).Text = Convert.ToString(VendorEmailID);
                popcon.Visible = false;
                btn_congo.Text = "Save";
            }
        }
        catch
        {
        }
    }

    protected void btn_save1_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            DataView dv = new DataView();
            DataRow dr;
            dt.Columns.Add("Item Header");
            dt.Columns.Add("Item Headercode");
            dt.Columns.Add("Item Code");
            dt.Columns.Add("Item Name");
            dt.Columns.Add("Dept Name");
            dt.Columns.Add("Dept Code");
            dt.Columns.Add("Duration");
            dt.Columns.Add("Supplied");
            dt.Columns.Add("Reference");
            if (Session["itemdata"] != null)
            {
                DataTable d1 = new DataTable();
                d1 = (DataTable)Session["itemdata"];
                if (d1.Rows.Count > 0)
                {
                    for (int r = 0; r < d1.Rows.Count; r++)
                    {
                        dr = dt.NewRow();
                        for (int c = 0; c < d1.Columns.Count; c++)
                        {
                            dr[c] = Convert.ToString(d1.Rows[r][c]);
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            string val = "";
            if (cb_alreadysup.Checked == true)
            {
                val = "Yes";
            }
            else
            {
                val = "No";
            }
            bool checkflage = false;
            if (cbl_header.Items.Count > 0)
            {
                for (int i = 0; i < cbl_header.Items.Count; i++)
                {
                    if (cbl_header.Items[i].Selected == true)
                    {
                        string selectquery = "select ItemCode, ItemName from IM_ItemMaster where ItemHeaderCode in ('" + cbl_header.Items[i].Value + "')";
                        ds.Clear();
                        ds = da.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (cblitem.Items.Count > 0)
                            {
                                for (int item = 0; item < cblitem.Items.Count; item++)
                                {
                                    if (cblitem.Items[item].Selected == true)
                                    {
                                        ds.Tables[0].DefaultView.RowFilter = "ItemCode='" + cblitem.Items[item].Value + "'";
                                        dv = ds.Tables[0].DefaultView;
                                        if (dv.Count > 0)
                                        {
                                            if (cbldepartment.Items.Count > 0)
                                            {
                                                for (int dept = 0; dept < cbldepartment.Items.Count; dept++)
                                                {
                                                    if (cbldepartment.Items[dept].Selected == true)
                                                    {
                                                        checkflage = true;
                                                        dr = dt.NewRow();
                                                        dr[0] = Convert.ToString(cbl_header.Items[i].Text);
                                                        dr[1] = Convert.ToString(cbl_header.Items[i].Value);
                                                        dr[2] = Convert.ToString(cblitem.Items[item].Value);
                                                        dr[3] = Convert.ToString(cblitem.Items[item].Text);
                                                        dr[4] = Convert.ToString(cbldepartment.Items[dept].Text);
                                                        dr[5] = Convert.ToString(cbldepartment.Items[dept].Value);
                                                        dr[6] = Convert.ToString(txt_consup.Text);
                                                        dr[7] = Convert.ToString(val);
                                                        dr[8] = Convert.ToString(txt_itmrefence.Text);
                                                        dt.Rows.Add(dr);
                                                    }
                                                }
                                                if (checkflage == false)
                                                {
                                                    dr = dt.NewRow();
                                                    dr[0] = Convert.ToString(cbl_header.Items[i].Text);
                                                    dr[1] = Convert.ToString(cbl_header.Items[i].Value);
                                                    dr[2] = Convert.ToString(cblitem.Items[item].Value);
                                                    dr[3] = Convert.ToString(cblitem.Items[item].Text);
                                                    dr[4] = Convert.ToString("");
                                                    dr[5] = Convert.ToString("");
                                                    dr[6] = Convert.ToString(txt_consup.Text);
                                                    dr[7] = Convert.ToString(val);
                                                    dr[8] = Convert.ToString(txt_itmrefence.Text);
                                                    dt.Rows.Add(dr);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
                SelectdptGrid.DataSource = dt;
                SelectdptGrid.DataBind();
                SelectdptGrid.Visible = true;
                popitm.Visible = false;
                //Session["itemdata"] = dt;
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Fill All Required Fields";
            }
        }
        catch
        {
        }
    }

    protected void ContactGrid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int row = Convert.ToInt32(e.CommandArgument);
            Session["row_new_Value"] = Convert.ToString(row);
            if (e.CommandName == "instruction")
            {
                string Name = ((ContactGrid.Rows[row].FindControl("lbl_name") as Label).Text);
                string Designation = ((ContactGrid.Rows[row].FindControl("lbl_designation") as Label).Text);
                string phonenumber = ((ContactGrid.Rows[row].FindControl("lbl_phoneno") as Label).Text);
                string Mobileno = ((ContactGrid.Rows[row].FindControl("lbl_mobileno") as Label).Text);
                string fax = ((ContactGrid.Rows[row].FindControl("lbl_faxno") as Label).Text);
                string VendorEmailID = ((ContactGrid.Rows[row].FindControl("lbl_email") as Label).Text);

                txt_connam.Text = Convert.ToString(Name);
                txt_designation.Text = Convert.ToString(Designation);
                txt_conpn.Text = Convert.ToString(phonenumber);
                txt_conmob.Text = Convert.ToString(Mobileno);
                txt_confax.Text = Convert.ToString(fax);
                txt_conmail.Text = Convert.ToString(VendorEmailID);
                btn_congo.Text = "Update";
                popcon.Visible = true;
            }
        }
        catch
        {
        }
    }

    protected void typegrid_OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int ro = 0; ro < e.Row.Cells.Count; ro++)
                {
                    e.Row.Cells[ro].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.ContactGrid, "instruction$" + e.Row.RowIndex);
                }
            }
        }
        catch
        {
        }
    }

    protected void btn_update_Click(object sender, EventArgs e)
    {
        try
        {
            string dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            string vendarcode = Convert.ToString(txt_code.Text);
            string type = "";
            if (rdb_vendor.Checked == true)
            {
                type = "0";
            }
            string vendorname = Convert.ToString(txt_vendorname1.Text);
            string street = Convert.ToString(txt_street.Text);
            string city = Convert.ToString(txt_city.Text);
            string VendorPincode = Convert.ToString(txt_pin.Text);
            string dist = Convert.ToString(ddl_district.SelectedItem.Text);

            string district = Convert.ToString(ddl_district.SelectedItem.Value);
            string state = Convert.ToString(ddl_State.SelectedItem.Value);
            string year = Convert.ToString(txt_startyear.Text);
            string status = Convert.ToString(ddlstatus.SelectedItem.Text);
            if (status.Trim() == "Approved")
            {
                status = "1";
            }
            if (status.Trim() == "Blocked")
            {
                status = "2";
            }
            if (district.Trim() == "Select")
            {
                district = "0";
            }
            if (state.Trim() == "Select")
            {
                state = "0";
            }

            string payment = "";
            if (rdbpaymentcash.Checked == true)
            {
                payment = "0";
            }
            if (rdbpaymentCredit.Checked == true)
            {
                payment = "1";
            }
            if (rdbpaymentCheque.Checked == true)
            {
                payment = "2";
            }

            string phoneno = Convert.ToString(txt_phn.Text);
            string faxno = Convert.ToString(txtfax.Text);
            string VendorEmailIDid = Convert.ToString(txt_email.Text);
            string website = Convert.ToString(txt_web.Text);
            string cstno = Convert.ToString(txt_cst.Text);
            string tinno = Convert.ToString(txt_tin.Text);
            string panno = Convert.ToString(txt_pan.Text);
            string moblieno = Convert.ToString(txt_mainmobileno.Text);
            string bankacno = Convert.ToString(txtconbank.Text);
            string ifsccode = Convert.ToString(txtconifsc.Text);
            string swiftcode = Convert.ToString(txtconswift.Text);
            string bankername = Convert.ToString(txtconbankname.Text);
            string bankderbranch = Convert.ToString(txtconbankbranch.Text);
            string updatevenmaster = "";
            string vencodepk = d2.getvenpk(vendarcode);
            string SupplierType = Convert.ToString(ddlSupplierType.SelectedItem.Text);
            string EmailID1 = Convert.ToString(Txtemailid1.Text);
            string EmailID2 = Convert.ToString(Txtemailid2.Text);
            string Address2 = Convert.ToString(TxtAddress2.Text);
            if (ChkLibrary.Checked == false)
            {
                updatevenmaster = "update CO_VendorMaster set VendorCode='" + vendarcode + "',VendorCompName='" + vendorname + "',VendorAddress='" + street + "',VendorPin='" + VendorPincode + "',VendorPhoneNo='" + phoneno + "',VendorFaxNo='" + faxno + "',VendorEmailID='" + VendorEmailIDid + "',VendorType='1',VendorTINNo='" + tinno + "',VendorCSTNo='" + cstno + "',VendorWebsite='" + website + "',VendorCity='" + city + "',VendorDist='" + district + "',VendorState='" + state + "',VendorStartYear='" + year + "',VendorPANNo='" + panno + "',VendorPayType='" + payment + "',VendorMobileNo='" + moblieno + "',vendorstatus='" + status + "'  where vendorpk='" + vencodepk + "'";
            }
            if (ChkLibrary.Checked == true)
            {
                updatevenmaster = "update CO_VendorMaster set VendorCode='" + vendarcode + "',VendorCompName='" + vendorname + "',VendorAddress='" + street + "',VendorPin='" + VendorPincode + "',VendorPhoneNo='" + phoneno + "',VendorFaxNo='" + faxno + "',VendorEmailID='" + VendorEmailIDid + "',VendorType='1',VendorTINNo='" + tinno + "',VendorCSTNo='" + cstno + "',VendorWebsite='" + website + "',VendorCity='" + city + "',VendorDist='" + district + "',VendorState='" + state + "',VendorStartYear='" + year + "',VendorPANNo='" + panno + "',VendorPayType='" + payment + "',VendorMobileNo='" + moblieno + "',vendorstatus='" + status + "',SupplierType='" + SupplierType + "',EmailID1='" + EmailID1 + "',EmailID2='" + EmailID2 + "',Address2='" + Address2 + "',LibraryFlag='1'  where vendorpk='" + vencodepk + "'";
            }

            updatevenmaster = updatevenmaster + " update IM_VendorBankMaster set VendorAccNo='" + bankacno + "',VendorBankIFSCCode='" + ifsccode + "',VendorBankSWIFTCode='" + swiftcode + "',VenBankName='" + bankername + "',VenBankBranch='" + bankderbranch + "' where vendorfk='" + vencodepk + "'";

            int inst = da.update_method_wo_parameter(updatevenmaster, "Text");

            if (inst != 0)
            {
                if (SelectdptGrid.Rows.Count > 0)
                {
                    string vendelete = "delete IM_VendorItemDept where VenItemFK='" + vencodepk + "'";
                    int vendel = d2.update_method_wo_parameter(vendelete, "Text");
                    //if (vendel != 0)
                    //{
                    for (int row = 0; row < SelectdptGrid.Rows.Count; row++)
                    {
                        string itemcode = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_itemcode") as Label).Text);
                        string itempk = d2.GetFunction("select ItemPK from IM_ItemMaster where ItemCode='" + itemcode + "'");
                        string deptcode = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_deptcode") as Label).Text);
                        if (deptcode.Trim() == "")
                        {
                            deptcode = "0";
                        }
                        string Duration = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_duration") as Label).Text);
                        if (Duration.Trim() == "")
                        {
                            Duration = "0";
                        }
                        string Refence = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_reference") as Label).Text);
                        string supplied = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_supplied") as Label).Text);
                        if (supplied.Trim() == "True")
                        {
                            supplied = "1";
                        }
                        else
                        {
                            supplied = "0";
                        }

                        string intquery = "insert into IM_VendorItemDept (VenItemFK,VenItemDeptFK,VenItemSupplyDur,VenItemIsSupplied, VenItemReference,ItemFK) values ('" + vencodepk + "','" + deptcode + "','" + Duration + "','" + supplied + "','" + Refence + "','" + itempk + "')";
                        //string intquery = "update IM_VendorItemDept set VenItemDeptFK='" + deptcode + "',VenItemSupplyDur='" + Duration + "',VenItemIsSupplied='" + supplied + "',VenItemReference='" + Refence + "' where VenItemFK='" + vencodepk + "' and ItemFK='" + itempk + "'";
                        int va = da.update_method_wo_parameter(intquery, "Text");
                    }
                    //}
                }
                if (ContactGrid.Rows.Count > 0)
                {
                    for (int row = 0; row < ContactGrid.Rows.Count; row++)
                    {
                        string contactname = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_name") as Label).Text);
                        string designame = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_designation") as Label).Text);
                        string contactphone = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_phoneno") as Label).Text);
                        string contactmobile = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_mobileno") as Label).Text);
                        string contactfaxno = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_faxno") as Label).Text);
                        string contactVendorEmailID = Convert.ToString((ContactGrid.Rows[row].FindControl("lbl_email") as Label).Text);
                        string vencondetail = "update IM_VendorContactMaster set VenContactName='" + contactname + "',VenContactDesig='" + designame + "',VendorPhoneNo='" + contactphone + "',VendorExtNo='" + contactfaxno + "',VendorMobileNo='" + contactmobile + "',VendorEmail='" + contactVendorEmailID + "' where VendorFK='" + vencodepk + "' and VenContactName='" + contactname + "'";

                        int cont = da.update_method_wo_parameter(vencondetail, "Text");
                    }
                }
                bindvendorname();
                imgdiv2.Visible = true;
                lbl_alert.Text = "Updated Successfully";
                btn_go_Click(sender, e);
                poperrjs.Visible = false;

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
                lbl_sure.Text = "Do you want delete this record?";
            }
        }
        catch
        {
        }
    }

    protected void delete()
    {
        try
        {
            surediv.Visible = false;
            string vendarcode = Convert.ToString(txt_code.Text);

            string venpk = d2.getvenpk(vendarcode);
            string nicedeletequery = " delete from CO_VendorMaster where vendorcode ='" + vendarcode + "'";
            nicedeletequery = nicedeletequery + " delete from IM_VendorItemDept where VenItemFK ='" + venpk + "'";
            nicedeletequery = nicedeletequery + " delete from IM_VendorContactMaster where VendorFK ='" + venpk + "'";
            nicedeletequery = nicedeletequery + " delete from IM_VendorBankMaster where VendorFK ='" + venpk + "'";
            int del = da.update_method_wo_parameter(nicedeletequery, "Text");
            if (del != 0)
            {
                bindvendorname();
                imgdiv2.Visible = true;
                lbl_alert.Text = "Deleted Successfully";
                btn_go_Click(sender, e);
                poperrjs.Visible = false;
            }
        }
        catch
        {
        }
    }

    public void Clear()
    {
        try
        {
            txt_consup.Text = "";
            txt_itmrefence.Text = "";
            cb_alreadysup.Checked = false;
            cblitem.Items.Clear();

            rdb_vendor.Checked = true;
            rdb_customer.Visible = false;
            txt_vendorname1.Text = "";
            txt_street.Text = "";
            txt_city.Text = "";
            txt_pin.Text = "";
            txt_district.Text = "";
            ddl_State.SelectedItem.Text = "Select";
            ddl_district.SelectedIndex = 0;
            //ddlbis.SelectedItem.Text = Convert.ToString(System.DateTime.Now.ToString("yyyy"));
            txt_startyear.Text = "";
            ddlstatus.SelectedItem.Text = "Select";
            rdbpaymentcash.Checked = false;
            rdbpaymentCredit.Checked = true;
            rdbpaymentCheque.Checked = false;
            txt_phn.Text = "";
            txtfax.Text = "";
            txt_email.Text = "";
            txt_web.Text = "";
            txt_cst.Text = "";
            txt_tin.Text = "";
            txt_mainmobileno.Text = "";
            txt_pan.Text = "";
            txtconbankname.Text = "";
            txtconbankbranch.Text = "";
            txtconbank.Text = "";
            txtconswift.Text = "";
            txtconifsc.Text = "";
            SelectdptGrid.DataSource = null;
            SelectdptGrid.DataBind();
            ContactGrid.DataSource = null;
            ContactGrid.DataBind();
            SelectdptGrid.Visible = false;
            ContactGrid.Visible = false;
        }
        catch
        {
        }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
    }

    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        imgdiv2.Visible = false;
        poperrjs.Visible = true;
    }

    protected void OnDataBound(object sender, EventArgs e)
    {
        try
        {
            for (int i = SelectdptGrid.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = SelectdptGrid.Rows[i];
                GridViewRow previousRow = SelectdptGrid.Rows[i - 1];
                for (int j = 1; j <= 1; j++)
                {
                    //lbl_itemheader
                    Label lnlname = (Label)row.FindControl("lbl_itemheader");
                    Label lnlname1 = (Label)previousRow.FindControl("lbl_itemheader");

                    if (lnlname.Text == lnlname1.Text)
                    {
                        if (previousRow.Cells[j].RowSpan == 0)
                        {
                            if (row.Cells[j].RowSpan == 0)
                            {
                                previousRow.Cells[j].RowSpan += 2;
                            }
                            else
                            {
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                            }
                            row.Cells[j].Visible = false;
                        }
                    }
                    //lbl_deptname  
                    lnlname = (Label)row.FindControl("lbl_deptname");
                    lnlname1 = (Label)previousRow.FindControl("lbl_deptname");

                    if (lnlname.Text == lnlname1.Text)
                    {
                        if (previousRow.Cells[4].RowSpan == 0)
                        {
                            if (row.Cells[4].RowSpan == 0)
                            {
                                previousRow.Cells[4].RowSpan += 2;
                            }
                            else
                            {
                                previousRow.Cells[4].RowSpan = row.Cells[4].RowSpan + 1;
                            }
                            row.Cells[4].Visible = false;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void ddl_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_type.SelectedValue == "0")
        {
            txt_search.Visible = true;
            txt_vendorname2.Visible = false;

            txt_vendorname2.Text = "";

            Fpspread1.Visible = false;
            div1.Visible = false;
            rptprint.Visible = false;
        }
        else if (ddl_type.SelectedValue == "1")
        {
            txt_search.Visible = false;
            txt_vendorname2.Visible = true;
            txt_vendorname2.Text = "";
            Fpspread1.Visible = false;
            div1.Visible = false;
            rptprint.Visible = false;
        }
    }
    // 
    public void binditemRender()
    {
        try
        {
            ds.Clear();
            cblitem.Items.Clear();
            string statequery = "select ItemCode,(ItemName +'-'+ ItemModel )as ItemName  from IM_ItemMaster where ItemHeaderCode<>'' order by ItemName +'-'+ ItemModel";
            ds = da.select_method_wo_parameter(statequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblitem.DataSource = ds;
                cblitem.DataTextField = "ItemName";
                cblitem.DataValueField = "ItemCode";
                cblitem.DataBind();
                cblitem.Visible = true;
            }
        }
        catch
        {
        }
    }

    protected void ChkLibrary_OnCheckedChanged(object sender, EventArgs e)
    {
        if (ChkLibrary.Checked == true)
        {
            fildset.Visible = false;
            FldsetLibrary.Visible = true;
        }
        else
        {
            fildset.Visible = true;
            FldsetLibrary.Visible = false;
        }
    }

}