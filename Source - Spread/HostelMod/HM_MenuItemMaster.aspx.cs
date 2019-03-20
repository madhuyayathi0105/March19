using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Gios.Pdf;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Net;
using System.Net.Mail;
using System.Configuration;

public partial class HM_MenuItemMaster : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    ArrayList itemcode = new ArrayList();
    bool check = false;
    static string hostel_name_code = "";
    int i;
    DataTable dt = new DataTable();
    DataTable dt2 = new DataTable();
    DataRow dr;
    static string checknew = "";

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
        lblvalidation1.Visible = false;
        if (!IsPostBack)
        {
            BindStudentType();
            bindmess();
            txt_item.Visible = true;
            bindhostelname();
            sessioname();
            menuname();
            itemheader();
            loadsubheadername();
            itemmaster();
            bindmenu1();
            hostel_name_code = "";
            //txtpopitem.Attributes.Add("readonly", "readonly");
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Fpspread1.Visible = false;
            btn_go_Click(sender, e);
            Session["Arraylist"] = null;
            Session["dt"] = null;
            checknew = "";
            btn_yes.Visible = false;
            btn_yes1.Visible = false;
        }
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
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
    public static List<string> Getitemcode(string prefixText)
    {

        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select distinct ItemCode from IM_ItemMaster WHERE ItemCode like '" + prefixText + "%' ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["ItemCode"].ToString());
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
        string query = "select distinct ItemHeaderName from IM_ItemMaster WHERE ItemHeaderName like '" + prefixText + "%' ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["ItemHeaderName"].ToString());
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
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname1(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        //query = "select MenuName from MenuMaster where  MenuName like '" + prefixText + "%'";
        if (hostel_name_code.Trim() != "")
        {
            query = "select distinct MenuName,MenuCode from MenuMaster m,Menu_ItemMaster mi where m.MenuCode =mi.SessionMenu_Code and mi.Hostel_Code in ('" + hostel_name_code + "') and m.MenuName like '" + prefixText + "%' ";
        }
        else
        {
            query = "select distinct MenuName,MenuCode from MenuMaster where  MenuName like '" + prefixText + "%' ";
        }
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getmenu(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select distinct MenuName  from HM_MenuMaster where  MenuName like '%" + prefixText + "%' order by MenuName";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["MenuName"].ToString());
            }
        }
        return name;
    }
    protected void ddl_search_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_search.SelectedValue == "0")
        {
            txt_item.Visible = true;
            txt_menu.Visible = false;
            txt_menu.Text = "";
        }
        else if (ddl_search.SelectedValue == "1")
        {
            txt_item.Visible = false;
            txt_menu.Visible = true;
            txt_item.Text = "";
        }
    }
    public void bindmenu1()
    {
        try
        {
            string deptquery = " select distinct MenuCode,MenuName from HM_MenuMaster mm,HM_MenuItemMaster mi where mm.MenuMasterPK=mi.MenuMasterFK and mm.CollegeCode=mi.CollegeCode and mi.MessMasterFK in ('" + Convert.ToString(ddl_basemessname.SelectedItem.Value) + "') order by MenuName";
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            cbl_menuname.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_menuname.DataSource = ds;
                cbl_menuname.DataTextField = "MenuName";
                cbl_menuname.DataValueField = "MenuCode";
                cbl_menuname.DataBind();
                if (cbl_menuname.Items.Count > 0)
                {
                    for (i = 0; i < cbl_menuname.Items.Count; i++)
                    {
                        cbl_menuname.Items[i].Selected = true;
                    }
                    txt_menuname.Text = "Menu Name(" + cbl_menuname.Items.Count + ")";
                }
            }
            else
            {
                txt_menuname.Text = "--Select--";
            }
        }

        catch (Exception ex)
        {
        }
    }
    protected void cb_menuname_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            if (cb_menuname.Checked == true)
            {
                for (i = 0; i < cbl_menuname.Items.Count; i++)
                {
                    cbl_menuname.Items[i].Selected = true;
                }
                txt_menuname.Text = "Menu Name(" + cbl_menuname.Items.Count + ")";
            }
            else
            {
                for (i = 0; i < cbl_menuname.Items.Count; i++)
                {
                    cbl_menuname.Items[i].Selected = false;
                }
                txt_menuname.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_menuname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            int i;
            txt_menuname.Text = "--Select--";
            cb_menuname.Checked = false;
            for (i = 0; i < cbl_menuname.Items.Count; i++)
            {
                if (cbl_menuname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_menuname.Text = "Menu Name(" + commcount.ToString() + ")";
                if (commcount == cbl_menuname.Items.Count)
                {
                    cb_menuname.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void sessioname()
    {
        try
        {
            cbl_session1.Items.Clear();
            string buildvalue = "";
            for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
            {
                if (cbl_hostelname1.Items[i].Selected == true)
                {
                    string build = cbl_hostelname1.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;
                    }
                }
                else
                {
                    txt_session1.Text = "--Select--";
                }
            }


            ds.Clear();
            ds = d2.BindSession(buildvalue);

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_session1.DataSource = ds;
                cbl_session1.DataTextField = "Session_Name";
                cbl_session1.DataValueField = "Session_Code";
                cbl_session1.DataBind();
                if (cbl_session1.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_session1.Items.Count; i++)
                    {
                        cbl_session1.Items[i].Selected = true;
                    }
                    txt_session1.Text = "Session Name(" + cbl_session1.Items.Count + ")";
                }
                else
                {
                    txt_session1.Text = "--Select--";
                }
            }

        }
        catch
        {

        }
    }
    public void menuname()
    {
        try
        {
            ddl_menuname1.Items.Clear();
            ds.Clear();
            string MenuType = string.Empty;
            if (ddl_menutype.SelectedItem.Text == "All")//15.02.18 barath 
                MenuType = " ";
            else
                MenuType = " and menutype in ('" + Convert.ToString(ddl_menutype.SelectedItem.Value) + "') ";
            if (MenuType != "")
            {
                string deptquery = "select distinct MenuCode,MenuName from HM_MenuMaster where CollegeCode ='" + collegecode1 + "' " + MenuType + " order by MenuName";
                //string deptquery = " select distinct MenuCode,MenuName from HM_MenuMaster mm,HM_MenuItemMaster mi where mm.MenuMasterPK=mi.MenuMasterFK and mm.CollegeCode=mi.CollegeCode and mm.MenuType in('" + mess1 + "') and mi.MessMasterFK in ('" + Convert.ToString(ddl_basemessname.SelectedItem.Value) + "') order by MenuName";

                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_menuname1.DataSource = ds;
                    ddl_menuname1.DataTextField = "MenuName";
                    ddl_menuname1.DataValueField = "MenuCode";
                    ddl_menuname1.DataBind();
                    ddl_menuname1.Items.Insert(0, "Select");
                    //if (cbl_itemheader3.Items.Count > 0)
                    //{
                    //    for (int i = 0; i < cbl_itemheader3.Items.Count; i++)
                    //    {

                    //        cbl_itemheader3.Items[i].Selected = true;
                    //    }

                    //    txt_itemheader3.Text = "Item Header(" + cbl_itemheader3.Items.Count + ")";
                    //}
                }
                else
                {
                    ddl_menuname1.Items.Insert(0, "Select");
                }
            }
            else
            {
                ddl_menuname1.Items.Insert(0, "Select");
            }
        }
        catch { }
    }
    public void itemheader()
    {
        try
        {
            cbl_itemheader3.Items.Clear();

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
            string selectnewquery = d2.GetFunction("select value from Master_Settings where settings='ItemHeaderRights' " + columnfield + "");
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
            // ds = d2.BindItemHeaderWithRights();
            string itemname = "select distinct ItemHeaderCode ,ItemHeaderName  from IM_ItemMaster";// where  ForHostelItem ='0'
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_itemheader3.DataSource = ds;
                cbl_itemheader3.DataTextField = "ItemHeaderName";
                cbl_itemheader3.DataValueField = "ItemHeaderCode";
                cbl_itemheader3.DataBind();


                if (cbl_itemheader3.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_itemheader3.Items.Count; i++)
                    {

                        cbl_itemheader3.Items[i].Selected = true;
                    }

                    txt_itemheader3.Text = "Item Header(" + cbl_itemheader3.Items.Count + ")";
                }
            }
            else
            {
                txt_itemheader3.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void itemmaster()
    {
        chklst_pop2itemtyp.Items.Clear();
        string itemheadercode = "";
        string subheader = "";
        for (int i = 0; i < cbl_itemheader3.Items.Count; i++)
        {
            if (cbl_itemheader3.Items[i].Selected == true)
            {
                if (itemheadercode == "")
                {
                    itemheadercode = "" + cbl_itemheader3.Items[i].Value.ToString() + "";
                }
                else
                {
                    itemheadercode = itemheadercode + "'" + "," + "'" + cbl_itemheader3.Items[i].Value.ToString() + "";
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
        if (itemheadercode.Trim() != "" && subheader.Trim() != "")
        {
            ds.Clear();
            //  ds = d2.BindItemCodewithsubheader(itemheadercode, subheader);
            string itemname = "select distinct ItemCode  ,ItemName   from IM_ItemMaster  where ItemHeaderCode in ('" + itemheadercode + "') and subheader_code in ('" + subheader + "')   order by ItemName";//and ForHostelItem ='0'
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
            chklst_pop2itemtyp.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklst_pop2itemtyp.DataSource = ds;
                chklst_pop2itemtyp.DataTextField = "ItemName";
                chklst_pop2itemtyp.DataValueField = "ItemCode";
                chklst_pop2itemtyp.DataBind();

                if (chklst_pop2itemtyp.Items.Count > 0)
                {
                    for (int i = 0; i < chklst_pop2itemtyp.Items.Count; i++)
                    {

                        chklst_pop2itemtyp.Items[i].Selected = true;
                    }
                    txt_itemname3.Text = "Item Name(" + chklst_pop2itemtyp.Items.Count + ")";
                }
            }
            else
            {
                txt_itemname3.Text = "--Select--";
            }
        }
        else
        {
            txt_itemname3.Text = "--Select--";
        }
    }

    protected void cbl_session_SelectedIndexChanged(object sender, EventArgs e)
    {
        int cout = 0;
        txt_session1.Text = "--Select--";
        if (cb_session1.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_session1.Items.Count; i++)
            {
                cbl_session1.Items[i].Selected = true;
            }
            txt_session1.Text = "Session Name(" + (cbl_session1.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_session1.Items.Count; i++)
            {
                cbl_session1.Items[i].Selected = false;
            }
        }
    }
    protected void cbl_session1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = 0;
        cb_session1.Checked = false;
        int commcount = 0;
        txt_session1.Text = "--Select--";
        for (i = 0; i < cbl_session1.Items.Count; i++)
        {
            if (cbl_session1.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_session1.Checked = false;
            }

        }
        if (commcount > 0)
        {
            if (commcount == cbl_session1.Items.Count)
            {
                cb_session1.Checked = true;
            }
            txt_session1.Text = "Session Name(" + (commcount) + ")";
        }

    }

    public void bindhostelname()
    {
        try
        {
            cbl_hostelname1.Items.Clear();
            ds.Clear();
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname1.DataSource = ds;
                cbl_hostelname1.DataTextField = "MessName";
                cbl_hostelname1.DataValueField = "MessMasterPK";
                cbl_hostelname1.DataBind();

                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "MessName";
                cbl_hostelname.DataValueField = "MessMasterPK";
                cbl_hostelname.DataBind();

                if (cbl_hostelname1.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
                    {
                        cbl_hostelname.Items[i].Selected = true;
                        cbl_hostelname1.Items[i].Selected = true;
                        if (hostel_name_code == "")
                        {
                            hostel_name_code = Convert.ToString(cbl_hostelname1.Items[i].Value);
                        }
                        else
                        {
                            hostel_name_code = hostel_name_code + "'" + "," + "'" + Convert.ToString(cbl_hostelname1.Items[i].Value);
                        }
                    }
                    txt_hostelname.Text = "Mess Name(" + cbl_hostelname.Items.Count + ")";
                    txt_hostelname1.Text = "Mess Name(" + cbl_hostelname1.Items.Count + ")";
                }
                menuname();
            }
        }
        catch
        {
        }
    }

    protected void bind_hostel2()
    {
        cbl_hostelname1.Items.Clear();
        ds.Clear();
        ds = d2.BindMess(collegecode1);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_hostelname1.DataSource = ds;
            cbl_hostelname1.DataTextField = "MessName";
            cbl_hostelname1.DataValueField = "MessID";
            cbl_hostelname1.DataBind();
        }
        if (cbl_hostelname1.Items.Count > 0)
        {
            for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
            {
                cbl_hostelname1.Items[i].Selected = true;
                //if (hostel_name_code == "")
                //{
                //    hostel_name_code = Convert.ToString(cbl_hostelname1.Items[i].Value);
                //}
                //else
                //{
                //    hostel_name_code = hostel_name_code + "'" + "," + "'" + Convert.ToString(cbl_hostelname1.Items[i].Value);
                //}
            }
            txt_hostelname1.Text = "Mess Name(" + cbl_hostelname1.Items.Count + ")";
        }

    }


    protected void cb_hostelname_CheckedChange(object sender, EventArgs e)
    {
        //int cout = 0;
        //txt_hostelname.Text = "--Select--";
        //if (cb_hostelname.Checked == true)
        //{
        //    cout++;
        //    for (int i = 0; i < cbl_hostelname.Items.Count; i++)
        //    {
        //        cbl_hostelname.Items[i].Selected = true;
        //    }
        //    txt_hostelname.Text = "Hostel Name(" + (cbl_hostelname.Items.Count) + ")";
        //}
        //else
        //{
        //    for (int i = 0; i < cbl_hostelname.Items.Count; i++)
        //    {
        //        cbl_hostelname.Items[i].Selected = false;
        //    }
        //}
        try
        {
            int i;
            if (cb_hostelname.Checked == true)
            {
                for (i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    if (cb_hostelname.Checked == true)
                    {
                        cbl_hostelname.Items[i].Selected = true;
                        txt_hostelname.Text = "Mess Name(" + (cbl_hostelname.Items.Count) + ")";
                        if (cbl_hostelname.Items[i].Selected == true)
                        {
                            if (hostel_name_code == "")
                            {
                                hostel_name_code = Convert.ToString(cbl_hostelname.Items[i].Value);
                            }
                            else
                            {
                                hostel_name_code = hostel_name_code + "'" + "," + "'" + Convert.ToString(cbl_hostelname.Items[i].Value);
                            }
                        }
                    }
                }
            }
            else
            {
                for (i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = false;
                    txt_hostelname.Text = "--Select--";
                    txt_menuname.Text = "--Select--";
                    cbl_menuname.ClearSelection();
                    cb_menuname.Checked = false;
                }
            }
            bindmenu1();
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        //cb_hostelname.Checked = false;
        //int commcount = 0;
        //txt_hostelname.Text = "--Select--";
        //for (i = 0; i < cbl_hostelname.Items.Count; i++)
        //{
        //    if (cbl_hostelname.Items[i].Selected == true)
        //    {
        //        commcount = commcount + 1;
        //    }
        //}
        //if (commcount > 0)
        //{
        //    txt_hostelname.Text = "Hostel Name(" + commcount.ToString() + ")";
        //    if (commcount == cbl_hostelname.Items.Count)
        //    {
        //        cb_hostelname.Checked = true;
        //    }
        //}
        try
        {
            int i = 0;
            int seatcount = 0;
            cb_hostelname.Checked = false;
            for (i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    if (hostel_name_code == "")
                    {
                        hostel_name_code = Convert.ToString(cbl_hostelname.Items[i].Value);
                    }
                    else
                    {
                        hostel_name_code = hostel_name_code + "'" + "," + "'" + Convert.ToString(cbl_hostelname.Items[i].Value);
                    }
                    seatcount = seatcount + 1;
                }
            }
            bindmenu1();
            if (seatcount == cbl_hostelname.Items.Count)
            {
                txt_hostelname.Text = "Mess Name(" + seatcount.ToString() + ")";
                cb_hostelname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_hostelname.Text = "--Select--";
            }
            else
            {
                txt_hostelname.Text = "Mess Name(" + seatcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void cb_hostelname1_CheckedChange(object sender, EventArgs e)
    {
        //int cout = 0;
        txt_hostelname1.Text = "--Select--";
        if (cb_hostelname1.Checked == true)
        {
            //cout++;
            for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
            {
                cbl_hostelname1.Items[i].Selected = true;
            }
            txt_hostelname1.Text = "Mess Name(" + (cbl_hostelname1.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
            {
                cbl_hostelname1.Items[i].Selected = false;
            }
            txt_hostelname1.Text = "--Select--";
        }
        sessioname();
        menuname();
    }
    protected void cbl_hostelname1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_hostelname1.Text = "--Select--";
            cb_hostelname1.Checked = false;
            for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
            {
                if (cbl_hostelname1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_hostelname1.Text = "Mess Name(" + commcount.ToString() + ")";
                if (commcount == cbl_hostelname1.Items.Count)
                {
                    cb_hostelname1.Checked = true;
                }
            }
            menuname();
        }
        catch (Exception ex)
        {
        }
        sessioname();
    }

    protected void cb_itemheader3_CheckedChange(object sender, EventArgs e)
    {
        int cout = 0;
        txt_itemheader3.Text = "--Select--";
        if (cb_itemheader3.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_itemheader3.Items.Count; i++)
            {
                cbl_itemheader3.Items[i].Selected = true;
            }
            txt_itemheader3.Text = "Item Header(" + (cbl_itemheader3.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_itemheader3.Items.Count; i++)
            {
                cbl_itemheader3.Items[i].Selected = false;
            }
        }
        loadsubheadername();
        itemmaster();
    }

    protected void cbl_itemheader_SelectedIndexChange(object sender, EventArgs e)
    {
        int i = 0;
        cb_hostelname1.Checked = false;
        int commcount = 0;
        txt_itemheader3.Text = "--Select--";
        for (i = 0; i < cbl_itemheader3.Items.Count; i++)
        {
            if (cbl_itemheader3.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_itemheader3.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_itemheader3.Items.Count)
            {
                cb_itemheader3.Checked = true;
            }
            txt_itemheader3.Text = "Item Header(" + commcount.ToString() + ")";
        }
        loadsubheadername();
        itemmaster();
    }

    protected void chklstitemtyp(object sender, EventArgs e)
    {
        int i = 0;
        chk_pop2itemtyp.Checked = false;
        int commcount = 0;
        txt_itemname3.Text = "--Select--";
        for (i = 0; i < chklst_pop2itemtyp.Items.Count; i++)
        {
            if (chklst_pop2itemtyp.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_hostelname1.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == chklst_pop2itemtyp.Items.Count)
            {
                chk_pop2itemtyp.Checked = true;
            }
            txt_itemname3.Text = "Item Name(" + commcount.ToString() + ")";
        }
    }

    protected void chkitemtyp(object sender, EventArgs e)
    {
        int cout = 0;
        txt_itemname3.Text = "--Select--";
        chk_pop2itemtyp.Checked = false;
        if (chk_pop2itemtyp.Checked == true)
        {
            cout++;
            for (int i = 0; i < chklst_pop2itemtyp.Items.Count; i++)
            {
                chklst_pop2itemtyp.Items[i].Selected = true;
            }
            txt_itemname3.Text = "Item Name(" + (chklst_pop2itemtyp.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < chklst_pop2itemtyp.Items.Count; i++)
            {
                chklst_pop2itemtyp.Items[i].Selected = false;
            }
        }
    }

    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        ddl_menuname1.Enabled = true;
        bindhostelname();
        sessioname();
        menuname();
        clear();
        txt_noofperson.Text = "";
        Session["Arraylist"] = null;
        Session["dt"] = null;
        btn_additem2.Visible = false;
        btn_save1.Visible = true;
        btn_delete.Visible = false;
        btn_update.Visible = false;
        popwindow.Visible = true;
        lblvalidation1.Visible = false;
    }
    protected void btn_addnew1_Click(object sender, EventArgs e)
    {
        if (ddl_menuname1.SelectedItem.Text.Trim() != "Select")
        {
            ViewState["selecteditems"] = null;
            selectitemgrid.DataSource = null;
            selectitemgrid.DataBind();

            menulbl.Text = Convert.ToString(ddl_menuname1.SelectedItem.Text);
            popwindow.Visible = false;
            popwindow1.Visible = true;
            txt_searchby.Visible = true;
            btn_go3_Click(sender, e);

            if (btn_update.Visible == true)
            {
                Session["dt"] = null;
                SelectdptGrid.DataSource = null;
                SelectdptGrid.DataBind();
            }
            else
            {
                if (Session["dt"] != null)
                {
                    DataTable addeditem = (DataTable)Session["dt"];
                    for (int k = 0; k < addeditem.Rows.Count; k++)
                    {
                        foreach (DataListItem gvrow in gvdatass.Items)
                        {
                            Label lblcode = (Label)gvrow.FindControl("lbl_itemcode");
                            string itemcode = lblcode.Text;
                            if (itemcode.Trim() == Convert.ToString(addeditem.Rows[k][0]))
                            {
                                CheckBox chkSelect = (gvrow.FindControl("CheckBox2") as CheckBox);
                                chkSelect.Checked = true;
                            }
                        }
                    }
                    selectedmenuchk(sender, e);
                }
            }
        }
        else
        {
            lbl_alert.Visible = true;
            lbl_alert.Text = "Please Select Menu Name";
            imgdiv2.Visible = true;
        }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {

            Printcontrol.Visible = false;
            string itemheadercode = "";

            if (txt_hostelname.Text.Trim() != "--Select--" && txt_menuname.Text.Trim() != "--Select--")
            {
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    if (cbl_hostelname.Items[i].Selected == true)
                    {
                        if (itemheadercode == "")
                        {
                            itemheadercode = "" + cbl_hostelname.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            itemheadercode = itemheadercode + "'" + "," + "'" + cbl_hostelname.Items[i].Value.ToString() + "";
                        }
                    }
                }

                string itemheadercode1 = "";
                for (int i = 0; i < cbl_menuname.Items.Count; i++)
                {
                    if (cbl_menuname.Items[i].Selected == true)
                    {
                        if (itemheadercode1 == "")
                        {
                            itemheadercode1 = "" + cbl_menuname.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            itemheadercode1 = itemheadercode1 + "'" + "," + "'" + cbl_menuname.Items[i].Value.ToString() + "";
                        }
                    }
                }
                DataView dv = new DataView();
                if (itemheadercode.Trim() != "")
                {
                    string selectquery = "";
                    collegecode = Session["collegecode"].ToString();

                    if (txt_item.Text.Trim() != "")
                    {
                        //selectquery = "select MenuName ,NoOfPersons ,NoOfItems ,h.MessName ,h.MessID ,m.Menu_ItemMasterCode from Menu_ItemMaster m,MessMaster h,MenuMaster ms,Menu_ItemDetail md,item_master i  where m.SessionMenu_Code =ms.MenuCode and h.MessID =m.Hostel_Code   and h.college_code =ms.College_Code and m.Menu_ItemMasterCode=md.Menu_ItemMasterCode   and i.item_code =md.Item_Code and m.Hostel_Code in ('" + itemheadercode + "')    and h.college_code ='" + collegecode + "' and i.item_name ='" + txt_item.Text + "'";
                        selectquery = "select ms.MenuName ,m.NoOfPerson,COUNT( md.ItemFK )as noofitem ,m.MenuMasterFK,m.MenuItemMasterPK  from HM_MenuItemMaster m,HM_MenuMaster ms,HM_MenuItemDetail md,IM_ItemMaster i   where m.MenuMasterFK =ms.MenuMasterPK and m.MenuItemMasterPK =md.MenuItemMasterFK   and m.CollegeCode =ms.CollegeCode and i.ItemPK =md.ItemFK  and m.CollegeCode ='" + collegecode + "' and i.ItemName ='" + txt_item.Text + "' and m.MessMasterFK in('" + Convert.ToString(ddl_basemessname.SelectedItem.Value) + "') group by ms.MenuName ,m.NoOfPerson,m.MenuMasterFK,m.MenuItemMasterPK";

                    }
                    else if (txt_menu.Text.Trim() != "")
                    {

                        selectquery = "select ms.MenuName ,m.NoOfPerson,COUNT( md.ItemFK )as noofitem ,m.MenuMasterFK,m.MenuItemMasterPK  from HM_MenuItemMaster m,HM_MenuMaster ms,HM_MenuItemDetail md  where m.MenuMasterFK =ms.MenuMasterPK and m.MenuItemMasterPK =md.MenuItemMasterFK   and m.CollegeCode =ms.CollegeCode  and m.CollegeCode ='" + collegecode + "' and ms.MenuCode in ('" + itemheadercode1 + "') and ms.MenuName='" + txt_menu.Text + "' and m.MessMasterFK in('" + Convert.ToString(ddl_basemessname.SelectedItem.Value) + "') group by ms.MenuName ,m.NoOfPerson,m.MenuMasterFK,m.MenuItemMasterPK";

                    }
                    else
                    {
                        //selectquery = "select MenuName ,NoOfPersons ,NoOfItems ,MessName ,h.MessID ,Menu_ItemMasterCode  from Menu_ItemMaster m,MessMaster h,MenuMaster ms  where m.SessionMenu_Code =ms.MenuCode  and h.MessID  =m.Hostel_Code   and h.college_code =ms.College_Code and m.Hostel_Code in ('" + itemheadercode + "') and h.college_code ='" + collegecode + "' and ms.MenuCode in ('" + itemheadercode1 + "')";

                        //selectquery = selectquery + "  select md.item_code,item_name,Needed_Qty,Hostel_Code  from Menu_ItemDetail md,item_master i where md.Item_Code =i.item_code and Hostel_Code in ('" + itemheadercode + "')";
                        selectquery = "select ms.MenuName ,m.NoOfPerson,COUNT( md.ItemFK )as noofitem ,m.MenuMasterFK,m.MenuItemMasterPK  from HM_MenuItemMaster m,HM_MenuMaster ms,HM_MenuItemDetail md  where m.MenuMasterFK =ms.MenuMasterPK and m.MenuItemMasterPK =md.MenuItemMasterFK   and m.CollegeCode =ms.CollegeCode  and m.CollegeCode ='" + collegecode + "' and ms.MenuCode in ('" + itemheadercode1 + "') and m.MessMasterFK in('" + Convert.ToString(ddl_basemessname.SelectedItem.Value) + "') group by ms.MenuName ,m.NoOfPerson,m.MenuMasterFK,m.MenuItemMasterPK";
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
                        Fpspread1.Sheets[0].ColumnCount = 7;

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

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Menu Name";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "No.of Person";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                        if (chk_option.Checked == true)
                        {
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Item Name";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Quantity";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[4].Visible = true;
                        }
                        else
                        {
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "No.of Item";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Quantity";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[4].Visible = false;
                        }

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Session Name";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Columns[5].Visible = false;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Mess Name";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Columns[6].Visible = false;
                        if (chk_option.Checked == true)
                        {
                            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                            {
                                if (ds.Tables[1].Rows.Count > 0)
                                {
                                    ds.Tables[1].DefaultView.RowFilter = "Hostel_Code='" + Convert.ToString(ds.Tables[0].Rows[row]["Hostel_Code"]) + "'";
                                    dv = ds.Tables[1].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        for (int i = 0; i < dv.Count; i++)
                                        {
                                            Fpspread1.Sheets[0].RowCount++;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["MenuName"]);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Menu_ItemMasterCode"]);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["NoOfPersons"]);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[i]["item_name"]);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(dv[i]["item_code"]);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[i]["Needed_Qty"]);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString("");
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["MessName"]);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                            {
                                Fpspread1.Sheets[0].RowCount++;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["MenuName"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["MenuItemMasterPK"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["NoOfPerson"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["noofitem"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString("");
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString("");
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                            }
                        }
                        Fpspread1.Visible = true;
                        rptprint.Visible = true;
                        div1.Visible = true;
                        lbl_error.Visible = false;
                        Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                        if (chk_option.Checked == true)
                        {
                            Fpspread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            Fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            Fpspread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            Fpspread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            Fpspread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            Fpspread1.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            Fpspread1.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        }
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
                    lbl_error.Text = "No Records Found";
                }
            }
            else
            {
                lbl_error.Visible = true;
                lbl_error.Text = "Please select all fields";
                div1.Visible = false;
                rptprint.Visible = false;
            }
        }
        catch
        {

        }
    }
    protected void btn_exit2_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }
    protected void btn_conexit4_Click(object sender, EventArgs e)
    {
        popwindow.Visible = true;
        popwindow1.Visible = false;
    }
    protected void Cell_Click(object sender, EventArgs e)
    {
        try
        {
            if (chk_option.Checked != true)
            {
                check = true;
            }
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
                ddl_menuname1.Enabled = false;
                DataView dv1 = new DataView();
                bindhostelname();
                string activerow = "";
                string activecol = "";
                btn_save1.Visible = false;
                btn_delete.Visible = true;
                btn_update.Visible = true;
                string noofpersons = "";
                activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                collegecode = Session["collegecode"].ToString();
                Session["activerow"] = Convert.ToString(activerow);
                Session["activecoloumn"] = Convert.ToString(activecol);
                if (activerow.Trim() != "")
                {
                    popwindow.Visible = true;
                    string menuitemmastercode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                    Session["menuitemcode"] = Convert.ToString(menuitemmastercode);
                    if (menuitemmastercode.Trim() != "")
                    {
                        string selecquery = "";
                        selecquery = "select distinct *  from HM_MenuItemMaster m,HM_MenuMaster ms  where m.MenuMasterFK =ms.MenuMasterPK and m.CollegeCode =ms.CollegeCode and  MenuItemMasterPK ='" + menuitemmastercode + "' and m.CollegeCode ='" + collegecode + "' ";
                        selecquery = selecquery + "select distinct md.ItemFK,ItemName,NeededQty,ItemUnit,ItemCode from HM_MenuItemDetail md,IM_ItemMaster i where md.ItemFK =i.ItemPK and MenuItemMasterFK  ='" + menuitemmastercode + "'";

                        selecquery = selecquery + "select * from HM_MenuMaster";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selecquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string sessioncode = "";
                            string menucode = Convert.ToString(ds.Tables[0].Rows[0]["MenuCode"]);
                            noofpersons = Convert.ToString(ds.Tables[0].Rows[0]["NoOfPerson"]);
                            string messfk = Convert.ToString(ds.Tables[0].Rows[0]["MessMasterFK"]);
                            if (messfk.Trim() != "")
                            {
                                ddl_messname.SelectedIndex = ddl_messname.Items.IndexOf(ddl_messname.Items.FindByValue(messfk));
                            }
                            int ch = 0;
                            //cbl_hostelname1.ClearSelection();
                            //if (cbl_hostelname1.Items.Count > 0)
                            //{
                            //    for (int row = 0; row < cbl_hostelname1.Items.Count; row++)
                            //    {
                            //        if (cbl_hostelname1.Items[row].Value == hostelcode)
                            //        {
                            //            ch++;
                            //            cbl_hostelname1.Items[row].Selected = true;
                            //        }
                            //    }
                            //    if (ch != 0)
                            //    {
                            //        txt_hostelname1.Text = "Mess Name (" + ch + ")";
                            //    }
                            //    else
                            //    {
                            //        txt_hostelname1.Text = "--Select--";
                            //    }
                            //}
                            txt_noofperson.Text = Convert.ToString(noofpersons);
                            //string menu = "";
                            //string build1 = "";
                            //string buildvalue1 = "";
                            //menu = ds.Tables[0].Rows[0]["MenuMasterFK"].ToString();
                            //ViewState["MenuMasterPK"] = Convert.ToString(menu);
                            //if (menu != "")
                            //{
                            //    ds.Tables[2].DefaultView.RowFilter = "MenuMasterPK in (" + menu + ")";
                            //    dv1 = ds.Tables[2].DefaultView;
                            //    if (dv1.Count > 0)
                            //    {
                            //        for (int row = 0; row < dv1.Count; row++)
                            //        {
                            //            build1 = Convert.ToString(dv1[row]["MenuName"]);
                            //            if (buildvalue1 == "")
                            //            {
                            //                buildvalue1 = build1;
                            //            }
                            //            else
                            //            {
                            //                buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                            //            }
                            //        }
                            //    }
                            //    ddl_menuname1.SelectedValue = Convert.ToString(buildvalue1);
                            //}
                            ddl_menuname1.SelectedValue = menucode;

                            //  ddlses.SelectedValue = sessioncode;                         
                            //txt_noofperson.Text = Convert.ToString(noofpersons);
                        }
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            DataRow dr;
                            dt.Columns.Add("ItemCode");
                            dt.Columns.Add("ItemName");
                            dt.Columns.Add("Measure");
                            dt.Columns.Add("Quantity");
                            for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                            {
                                dr = dt.NewRow();
                                dr[0] = Convert.ToString(ds.Tables[1].Rows[j]["ItemCode"]);
                                dr[1] = Convert.ToString(ds.Tables[1].Rows[j]["ItemName"]);
                                dr[2] = Convert.ToString(ds.Tables[1].Rows[j]["ItemUnit"]);
                                dr[3] = Convert.ToString(ds.Tables[1].Rows[j]["NeededQty"]);
                                dt.Rows.Add(dr);
                            }
                            if (dt.Rows.Count > 0)
                            {
                                SelectdptGrid.DataSource = dt;
                                SelectdptGrid.DataBind();
                                SelectdptGrid.Visible = true;
                                Session["dt"] = dt;
                            }
                            else
                            {
                                SelectdptGrid.Visible = false;

                            }
                            //txtpopitem.Text = "";
                            //txtpopqty.Text = "";
                        }
                        else
                        {
                            SelectdptGrid.Visible = false;

                        }
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
            string degreedetails = "Menu Item Master Report";
            string pagename = "canteen_menu_item_master.aspx";
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
                d2.printexcelreport(Fpspread1, reportname);
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

    protected void btn_go3_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["selecteditems"] != null)
            {
                DataTable dnew = (DataTable)ViewState["selecteditems"];
                ViewState["sb"] = dnew;
                checknew = "s";
            }

            string itemheadercode = "";
            for (int i = 0; i < cbl_itemheader3.Items.Count; i++)
            {
                if (cbl_itemheader3.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_itemheader3.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cbl_itemheader3.Items[i].Value.ToString() + "";
                    }
                }
            }
            string itemheadercode1 = "";
            for (int i = 0; i < chklst_pop2itemtyp.Items.Count; i++)
            {
                if (chklst_pop2itemtyp.Items[i].Selected == true)
                {
                    if (itemheadercode1 == "")
                    {
                        itemheadercode1 = "" + chklst_pop2itemtyp.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode1 = itemheadercode1 + "'" + "," + "'" + chklst_pop2itemtyp.Items[i].Value.ToString() + "";
                    }
                }
            }
            string selectquery = "";
            if (txt_searchby.Text.Trim() != "")
            {
                selectquery = "select ItemHeaderName,ItemHeaderCode,ItemCode,ItemName ,ItemModel,ItemSize ,ItemUnit,ItemSpecification from IM_ItemMaster where ItemName='" + txt_searchby.Text + "' order by ItemHeaderCode";
            }
            else if (txt_searchitemcode.Text.Trim() != "")
            {
                selectquery = "select ItemHeaderName,ItemHeaderCode,ItemCode,ItemName ,ItemModel,ItemSize ,ItemUnit,ItemSpecification from IM_ItemMaster where ItemCode='" + txt_searchitemcode.Text + "' order by ItemHeaderCode";
            }
            else if (txt_searchheadername.Text.Trim() != "")
            {
                selectquery = "select ItemHeaderName,ItemHeaderCode,ItemCode,ItemName ,ItemModel,ItemSize ,ItemUnit,ItemSpecification from IM_ItemMaster where ItemHeaderName='" + txt_searchheadername.Text + "' order by ItemHeaderCode";
            }
            else if (itemheadercode.Trim() != "" && itemheadercode1.Trim() != "")
            {
                //selectquery = "select distinct  item_code ,item_name , itemheader_code,itemheader_name,item_unit from item_master where itemheader_code in ('" + itemheadercode + "') and item_code in ('" + itemheadercode1 + "') order by item_code ";
                selectquery = "select distinct  ItemCode ,ItemName , ItemHeaderCode,ItemHeaderName,ItemUnit from IM_ItemMaster where ItemHeaderCode in ('" + itemheadercode + "') and ItemCode in ('" + itemheadercode1 + "') order by ItemCode ";
            }

            if (txt_itemheader3.Text.Trim() != "--Select--" && txt_itemname3.Text.Trim() != "--Select--")
            {
                if (selectquery.Trim() != "")
                {
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gvdatass.DataSource = ds.Tables[0];
                        gvdatass.DataBind();
                        gvdatass.Visible = true;
                        div2.Visible = true;
                        btn_itemsave4.Visible = true;
                        btn_conexist4.Visible = true;
                        lbl_error3.Visible = false;
                    }
                }
            }
            else
            {
                lbl_error3.Visible = true;
                lbl_error3.Text = "Please select all fields";
                div2.Visible = false;
                btn_itemsave4.Visible = false;
                btn_conexist4.Visible = false;

            }
            txt_searchby.Text = "";
            txt_searchitemcode.Text = "";
            txt_searchheadername.Text = "";
        }
        catch
        {

        }

    }

    protected void btn_itemsave4_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("ItemCode");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("Measure");
            dt.Columns.Add("Quantity");
            //20.10.15
            if (Session["dt"] != null)
            {
                DataTable d1 = new DataTable();
                d1 = (DataTable)Session["dt"];
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
            int count = 0;
            string itemname = "";
            //foreach (DataListItem gvrow in gvdatass.Items)
            ////for (int i = 0; i < selectitemgrid.Rows.Count;i++ )
            //{
            //    CheckBox chkSelect = (gvrow.FindControl("CheckBox2") as CheckBox);
            //    if (chkSelect.Checked)
            //    {
            //        count++;
            //        Label lbl_itemname = (Label)gvrow.FindControl("lbl_itemname");
            //        itemname = lbl_itemname.Text;
            //        Label lbl_itemcode = (Label)gvrow.FindControl("lbl_itemcode");
            //        string itemcode = lbl_itemcode.Text;
            //        Label lbl_itemmeasure = (Label)gvrow.FindControl("lbl_measureitem");
            //        string Measure = lbl_itemmeasure.Text;
            //        Session["itemnewcode"] = Convert.ToString(itemcode);

            //        //Label lbl_itemheadername = (Label)gvrow.FindControl("lbl_itemheadername");
            //        //string headername = lbl_itemheadername.Text;
            //        //Label lbl_itemheadercode = (Label)gvrow.FindControl("lbl_itemheadercode");
            //        //string headercode = lbl_itemheadercode.Text;

            //        string noofperson = Convert.ToString(txt_noofperson.Text);

            //        dr = dt.NewRow();

            //        dr[0] = Convert.ToString(Session["itemnewcode"]);
            //        dr[1] = Convert.ToString(itemname);
            //        dr[2] = Convert.ToString(Measure);
            //        dr[3] = Convert.ToString("");
            //        dt.Rows.Add(dr);
            //        if (dt.Rows.Count > 0)
            //        {
            //            SelectdptGrid.DataSource = dt;
            //            SelectdptGrid.DataBind();
            //            SelectdptGrid.Visible = true;
            //            Session["dt"] = dt;
            //            popwindow1.Visible = false;
            //            popwindow.Visible = true;
            //            btn_additem2.Visible = true;
            //        }
            //    }
            //}
            if (selectitemgrid.Rows.Count > 0)
            {
                for (int i = 0; i < selectitemgrid.Rows.Count; i++)
                {
                    dr = dt.NewRow();
                    dr[0] = Convert.ToString((selectitemgrid.Rows[i].FindControl("itemcodegv") as Label).Text);
                    dr[1] = Convert.ToString((selectitemgrid.Rows[i].FindControl("itemnamegv") as Label).Text);
                    dr[2] = Convert.ToString((selectitemgrid.Rows[i].FindControl("lbl_measureitem") as Label).Text);
                    string noofperson = Convert.ToString(txt_noofperson.Text);
                    dr[3] = Convert.ToString("");
                    dt.Rows.Add(dr);
                }
            }

            if (Session["dt"] != null)
            {
                DataTable d1 = new DataTable();
                d1 = (DataTable)Session["dt"];
                if (d1.Rows.Count > 0)
                {
                    for (int r = 0; r < d1.Rows.Count; r++)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (Convert.ToString(dt.Rows[i]["itemcode"]) == Convert.ToString(d1.Rows[r]["ItemCode"]))
                            {
                                if (Convert.ToString(dt.Rows[i]["Quantity"]).Trim() == "")
                                {
                                    dt.Rows.RemoveAt(i);
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
                Session["dt"] = dt;
                popwindow1.Visible = false;
                popwindow.Visible = true;
                btn_additem2.Visible = true;
            }
            if (count == 0)
            {
                //imgdiv2.Visible = true;
                //lbl_alert.Text = "Please Select Any one Item";
                //ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Any one Item\");", true); Response.Write("<script>alert('Data inserted successfully')</script>");


            }

        }
        catch
        {

        }
    }

    protected void btn_additem2_Clcik(object sender, EventArgs e)
    {
        try
        {
            if (btn_additem2.Text == "Remove")
            {
                surediv.Visible = true;
                btn_yes.Visible = false;
                btn_yes1.Visible = true;
                //  additems2();
                lbl_sure.Text = "Do you want to Remove this Item?";
            }

        }
        catch
        {
        }
    }
    protected void additems2()
    {
        try
        {
            surediv.Visible = false;
            DataTable dt = new DataTable();
            DataRow dr;
            bool newcheck = false;
            dt.Columns.Add("ItemCode");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("Measure");
            dt.Columns.Add("Quantity");
            if (SelectdptGrid.Rows.Count > 0)
            {
                for (int row = 0; row < SelectdptGrid.Rows.Count; row++)
                {
                    if ((SelectdptGrid.Rows[row].FindControl("cb_select") as CheckBox).Checked == false)
                    {
                        dr = dt.NewRow();
                        dr[0] = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_itemcode") as Label).Text);
                        dr[1] = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_itemname") as Label).Text);
                        dr[2] = Convert.ToString((SelectdptGrid.Rows[row].FindControl("lbl_itemmeasure") as Label).Text);
                        dr[3] = Convert.ToString((SelectdptGrid.Rows[row].FindControl("txt_quantity") as TextBox).Text);
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        newcheck = true;
                        //string menucode = Convert.ToString(ddl_menuname1.SelectedItem.Value);
                        //string itemcode = Convert.ToString((SelectdptGrid.Rows[i].FindControl("lbl_itemcode") as Label).Text);
                        //string quantityvalue = Convert.ToString((SelectdptGrid.Rows[i].FindControl("txt_quantity") as TextBox).Text);
                        //string itempk = d2.GetFunction("select ItemPK from  IM_ItemMaster where ItemCode='" + itemcode + "'");
                        //string menuitemmasterpkk = d2.GetFunction("select mi.MenuItemMasterPK  from  HM_MenuItemMaster mi,HM_MenuMaster hm,IM_ItemMaster im  where mi.MenuMasterFK=hm.MenuMasterPK and hm.MenuCode='" + menucode + "' and im.ItemCode='" + itemcode + "'");

                        //string menudel = "delete HM_MenuItemDetail  where MenuItemMasterFK='" + menuitemmasterpkk + "' and ItemFK='" + itempk + "' ";
                        //int del = d2.update_method_wo_parameter(menudel, "Text");
                        //if (del != 0)
                        //{

                        //}
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    SelectdptGrid.DataSource = dt;
                    SelectdptGrid.DataBind();
                    SelectdptGrid.Visible = true;
                    Session["dt"] = dt;
                }
                else
                {
                    SelectdptGrid.Visible = false;
                    Session["dt"] = null;
                }
                if (newcheck == true)
                {
                    lbl_sure.Text = "Do You Want Remove This Item?";
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Removed Successfully";
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Please Select Any one Item";
                }
            }
        }
        catch
        {
        }
    }
    protected void btn_save1_Clcik(object sender, EventArgs e)
    {
        try
        {
            string dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            string sessioncode = "";

            string menucode = Convert.ToString(ddl_menuname1.SelectedItem.Value);
            string persion = Convert.ToString(txt_noofperson.Text);
            bool valuecheck = false;
            int itemmenucount = 0;

            string insetquery = "";
            string selectquery = d2.GetFunction("select mi.MenuMasterFK from  HM_MenuItemMaster mi,HM_MenuMaster hm where hm.MenuCode='" + menucode + "' and mi.MenuMasterFK=hm.MenuMasterPK and MessMasterFK='" + Convert.ToString(ddl_messname.SelectedItem.Value) + "'");
            if (selectquery.Trim() != "" && selectquery.Trim() != "0")
            {
                string getvaluee = d2.GetFunction("select MenuMasterPK from  HM_MenuMaster where MenuCode='" + menucode + "'");
                string clgcode = d2.GetFunction("select CollegeCode from  HM_MenuMaster where MenuCode='" + menucode + "'");
                insetquery = "update HM_MenuItemMaster set NoOfPerson ='" + persion + "',CollegeCode='" + clgcode + "' where  MenuMasterFK ='" + getvaluee + "' and MessMasterFK='" + Convert.ToString(ddl_messname.SelectedItem.Value) + "' ";
                itemmenucount = Convert.ToInt32(selectquery);
            }
            else
            {
                string getvaluee = d2.GetFunction("select MenuMasterPK from  HM_MenuMaster where MenuCode='" + menucode + "'");
                string clgcode = d2.GetFunction("select CollegeCode from  HM_MenuMaster where MenuCode='" + menucode + "'");
                //MenuItemMasterPK,  '" + itemmenucount + "',
                insetquery = "INSERT INTO HM_MenuItemMaster(MenuMasterFK,NoOfPerson,CollegeCode,MessMasterFK) values ('" + getvaluee + "','" + persion + "','" + clgcode + "','" + Convert.ToString(ddl_messname.SelectedItem.Value) + "')";
            }
            int ins = d2.update_method_wo_parameter(insetquery, "Text");
            if (ins != 0)
            {
                if (SelectdptGrid.Rows.Count > 0)
                {
                    // string menuitemmasterpk = d2.GetFunction("select MenuItemMasterPK  from  HM_MenuItemMaster mi,HM_MenuMaster hm where mi.MenuMasterFK=hm.MenuMasterPK and hm.MenuCode='" + menucode + "'");
                    //  string delquery = "delete from HM_MenuItemDetail where MenuItemMasterFK ='" + menuitemmasterpk + "'";
                    //    int del = d2.update_method_wo_parameter(delquery, "Text");
                    for (int i = 0; i < SelectdptGrid.Rows.Count; i++)
                    {
                        string itemcode = Convert.ToString((SelectdptGrid.Rows[i].FindControl("lbl_itemcode") as Label).Text);
                        string quantityvalue = Convert.ToString((SelectdptGrid.Rows[i].FindControl("txt_quantity") as TextBox).Text);
                        if (quantityvalue.Trim() != "")
                        {
                            string itempk = d2.GetFunction("select ItemPK from  IM_ItemMaster where ItemCode='" + itemcode + "'");
                            string menuitemmasterpkk = d2.GetFunction("select mi.MenuItemMasterPK  from  HM_MenuItemMaster mi,HM_MenuMaster hm,IM_ItemMaster im  where mi.MenuMasterFK=hm.MenuMasterPK and hm.MenuCode='" + menucode + "' and im.ItemCode='" + itemcode + "' and MessMasterFK='" + Convert.ToString(ddl_messname.SelectedItem.Value) + "'");
                            //string updatequery = "INSERT INTO HM_MenuItemDetail(MenuItemMasterFK,ItemFK,NeededQty) values ('" + menuitemmasterpkk + "','" + itempk + "','" + quantityvalue + "')";
                            string updatequery = "if exists (select * from HM_MenuItemDetail where MenuItemMasterFK='" + menuitemmasterpkk + "' and ItemFK='" + itempk + "')update HM_MenuItemDetail set NeededQty ='" + quantityvalue + "' where MenuItemMasterFK='" + menuitemmasterpkk + "' and ItemFK='" + itempk + "' else INSERT INTO HM_MenuItemDetail(MenuItemMasterFK,ItemFK,NeededQty) values ('" + menuitemmasterpkk + "','" + itempk + "','" + quantityvalue + "')";
                            int upd = d2.update_method_wo_parameter(updatequery, "Text");
                            if (upd != 0)
                            {
                                valuecheck = true;
                            }
                        }
                    }
                }
            }
            if (valuecheck == true)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Saved Successfully";
                btn_addnew_Click(sender, e);
                bindhostelname();
                bindmenu1();
                btn_go_Click(sender, e);
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Update Quantity Values";
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
            string sessioncode = "";
            string menucode = Convert.ToString(ddl_menuname1.SelectedItem.Value);
            string persion = "";
            bool valuecheck = false;
            int itemmenucount = 0;
            itemmenucount = Convert.ToInt32(Session["menuitemcode"]);

            string insetquery = "";
            persion = Convert.ToString(txt_noofperson.Text);

            string getvaluee = d2.GetFunction("select MenuMasterPK from  HM_MenuMaster where MenuCode='" + menucode + "'");
            string clgcode = d2.GetFunction("select CollegeCode from  HM_MenuMaster where MenuCode='" + menucode + "'");
            insetquery = "if exists (select * from HM_MenuItemMaster where MenuMasterFK='" + getvaluee + "' and MessMasterFK='" + Convert.ToString(ddl_messname.SelectedItem.Value) + "') update HM_MenuItemMaster set NoOfPerson ='" + persion + "',CollegeCode='" + clgcode + "' where MenuMasterFK='" + getvaluee + "' and MessMasterFK='" + Convert.ToString(ddl_messname.SelectedItem.Value) + "' else INSERT INTO HM_MenuItemMaster(MenuMasterFK,NoOfPerson,CollegeCode,MessMasterFK) values ('" + getvaluee + "','" + persion + "','" + clgcode + "','" + Convert.ToString(ddl_messname.SelectedItem.Value) + "')";
            int ins = d2.update_method_wo_parameter(insetquery, "Text");
            if (ins != 0)
            {
                if (SelectdptGrid.Rows.Count > 0)
                {
                    string menumasterfk = d2.GetFunction("select MenuItemMasterPK  from HM_MenuItemMaster where CollegeCode='" + clgcode + "' and MenuMasterFK='" + getvaluee + "' and MessMasterFK='" + Convert.ToString(ddl_messname.SelectedItem.Value) + "'");//01.04.16
                    string del = " delete from HM_MenuItemDetail where MenuItemMasterFK ='" + menumasterfk + "'";

                    int de = d2.update_method_wo_parameter(del, "text");

                    for (int i = 0; i < SelectdptGrid.Rows.Count; i++)
                    {
                        string itemcode = Convert.ToString((SelectdptGrid.Rows[i].FindControl("lbl_itemcode") as Label).Text);
                        string quantityvalue = Convert.ToString((SelectdptGrid.Rows[i].FindControl("txt_quantity") as TextBox).Text);
                        if (quantityvalue.Trim() != "")
                        {
                            string itempk = d2.GetFunction("select ItemPK from  IM_ItemMaster where ItemCode='" + itemcode + "'");
                            string menuitemmasterpkk = d2.GetFunction("select mi.MenuItemMasterPK  from  HM_MenuItemMaster mi,HM_MenuMaster hm,IM_ItemMaster im  where mi.MenuMasterFK=hm.MenuMasterPK and hm.MenuCode='" + menucode + "' and im.ItemCode='" + itemcode + "' and MessMasterFK='" + Convert.ToString(ddl_messname.SelectedItem.Value) + "'");
                            string updatequery = "INSERT INTO HM_MenuItemDetail(MenuItemMasterFK,ItemFK,NeededQty) values ('" + menuitemmasterpkk + "','" + itempk + "','" + quantityvalue + "')";
                            int upd = d2.update_method_wo_parameter(updatequery, "Text");
                            if (upd != 0)
                            {
                                valuecheck = true;
                            }
                        }
                    }
                }
            }
            if (valuecheck == true)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Updated Successfully";
                btn_go_Click(sender, e);
                popwindow.Visible = false;
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Update Quantity Values";
            }
            // }
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
                btn_yes1.Visible = false;
                btn_yes.Visible = true;
                lbl_sure.Text = "Do you want to Delete this Record?";
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
            bool checkme = false;
            int itemmenucount = 0;
            itemmenucount = Convert.ToInt32(Session["menuitemcode"]);
            string sessioncode = "";
            string menucode = Convert.ToString(ddl_menuname1.SelectedItem.Value);

            string getvaluee = d2.GetFunction("select MenuMasterPK from  HM_MenuMaster where MenuCode='" + menucode + "'");
            string clgcode = d2.GetFunction("select CollegeCode from  HM_MenuMaster where MenuCode='" + menucode + "'");

            string deletequery = "";
            for (int i = 0; i < SelectdptGrid.Rows.Count; i++)
            {
                if ((SelectdptGrid.Rows[i].FindControl("cb_select") as CheckBox).Checked == true)
                {
                    string itemcode = Convert.ToString((SelectdptGrid.Rows[i].FindControl("lbl_itemcode") as Label).Text);
                    string itempk = d2.GetFunction("select ItemPK from  IM_ItemMaster where ItemCode='" + itemcode + "'");
                    //string menuitemmasterpkk = d2.GetFunction("select mi.MenuItemMasterPK  from  HM_MenuItemMaster mi,HM_MenuMaster hm,IM_ItemMaster im  where mi.MenuMasterFK=hm.MenuMasterPK and hm.MenuCode='" + menucode + "' and im.ItemCode='" + itemcode + "'");
                    string menuitemmasterpkk = d2.GetFunction(" select mi.MenuItemMasterPK  from  HM_MenuItemMaster mi,HM_MenuMaster hm  where mi.MenuMasterFK=hm.MenuMasterPK and mi.MessMasterFK ='" + Convert.ToString(ddl_messname.SelectedItem.Value) + "' and hm.MenuCode='" + menucode + "'");

                    deletequery = "delete from HM_MenuItemDetail where ItemFK ='" + itempk + "' and MenuItemMasterFK ='" + menuitemmasterpkk + "'";
                    int del = d2.update_method_wo_parameter(deletequery, "Text");
                    if (del != 0)
                    {
                        checkme = true;
                    }
                }
            }
            try
            {
                deletequery = " delete HM_MenuItemMaster where MenuMasterFK ='" + getvaluee + "' and MessMasterFK='" + Convert.ToString(ddl_messname.SelectedItem.Value) + "'";
                int del1 = d2.update_method_wo_parameter(deletequery, "Text");
            }
            catch { }
            if (checkme == true)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Deleted Successfully";
                btn_go_Click(sender, e);
                popwindow.Visible = false;
            }
        }
        catch
        {
        }
    }
    protected void SelectdptGrid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int row = Convert.ToInt32(e.CommandArgument);
            Session["rowvalue"] = Convert.ToString(row);
            if (e.CommandName == "instruction")
            {
                string itemcode = ((SelectdptGrid.Rows[row].FindControl("lbl_itemcode") as Label).Text);
                string itemname = ((SelectdptGrid.Rows[row].FindControl("lbl_itemname") as Label).Text);
                string qunatity = ((SelectdptGrid.Rows[row].FindControl("lblquantity") as Label).Text);
                //txtpopitem.Text = Convert.ToString(itemname);
                //txtpopqty.Text = Convert.ToString(qunatity);
                btn_additem2.Text = "Update";
                Session["itemnewcode"] = Convert.ToString(itemcode);
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
                e.Row.Cells[0].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.SelectdptGrid, "instruction$" + e.Row.RowIndex);
                e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.SelectdptGrid, "instruction$" + e.Row.RowIndex);
                e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.SelectdptGrid, "instruction$" + e.Row.RowIndex);
                e.Row.Cells[3].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.SelectdptGrid, "instruction$" + e.Row.RowIndex);
            }
        }
        catch
        {

        }

    }

    public void clear()
    {
        //ddlses.SelectedIndex = 0;
        //  ddl_menuname1.SelectedIndex = 0;
        //txtpopitem.Text = "";
        // txtpopqty.Text = "";
        // txt_noofperson.Text = "";
        SelectdptGrid.Visible = false;
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
    }
    protected void btn_sureyes1_Click(object sender, EventArgs e)
    {
        additems2();
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        imgdiv2.Visible = false;
        popwindow.Visible = true;
    }
    public object sender { get; set; }
    public EventArgs e { get; set; }

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


                Label lbl_headername = (Label)gvrow.FindControl("lblitemheadername");
                itemheadername = lbl_headername.Text;
                dr[3] = itemheadername;

                Label lbl_itemheadercode = (Label)gvrow.FindControl("lbl_itemheadercode");
                string itemheadercode = lbl_itemheadercode.Text;
                dr[4] = itemheadercode;

                Label lbl_measureitem = (Label)gvrow.FindControl("lbl_measureitem");
                string measureitem = lbl_measureitem.Text;
                //if(measureitem.Trim()!="")
                //{
                dr[5] = measureitem;
                //}
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
        dt.Columns.Add("Header Name");
        dt.Columns.Add("Header code");
        dt.Columns.Add("Item unit");
        dt.TableName = "selecteditems";
    }

    //theivamani 24.11.15

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
            // loadsubheadername();
            itemmaster();

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
            itemmaster();
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
            for (int i = 0; i < cbl_itemheader3.Items.Count; i++)
            {
                if (cbl_itemheader3.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_itemheader3.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_itemheader3.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemheader.Trim() != "")
            {
                string query = "";
                //query = "select distinct t.TextCode,t.TextVal  from TextValTable t,item_master i where t.TextCode=i.subheader_code and itemheader_code in ('" + itemheader + "') and college_code in ('" + collegecode1 + "')";
                query = "select distinct t.MasterCode,t.MasterValue  from CO_MasterValues t,IM_ItemMaster i where t.MasterCode=i.subheader_code and ItemHeaderCode in ('" + itemheader + "') and CollegeCode in ('" + collegecode1 + "')";
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
                        Panel5.Width = 300;
                        Panel5.Height = 300;
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
    protected void ddl_menutype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        menuname();
    }
    protected void ddl_messname_Selectedindexchange(object sender, EventArgs e)
    {

    }
    protected void bindmess()
    {
        try
        {
            ds.Clear();
            ddl_messname.Items.Clear();
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_messname.DataSource = ds;
                ddl_messname.DataTextField = "MessName";
                ddl_messname.DataValueField = "MessMasterPK";
                ddl_messname.DataBind();

                ddl_basemessname.DataSource = ds;
                ddl_basemessname.DataTextField = "MessName";
                ddl_basemessname.DataValueField = "MessMasterPK";
                ddl_basemessname.DataBind();
            }
        }
        catch
        {
        }
    }
    protected void ddl_basemessname_Selectedindexchange(object sender, EventArgs e)
    {
        //menuname();
        bindmenu1();
    }
    protected void BindStudentType()
    {
        try
        {
            ddl_menutype.Items.Clear();
            ds.Clear();
            string sql = "select StudentType-1 as StudentType,StudentTypeName from HostelStudentType where CollegeCode='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_menutype.DataSource = ds;
                ddl_menutype.DataTextField = "StudentTypeName";
                ddl_menutype.DataValueField = "StudentType";
                ddl_menutype.DataBind();
                ddl_menutype.Items.Insert(0, "All");
            }
            else { ddl_menutype.Items.Insert(0, "All"); }
        }
        catch
        {
        }
    }
}
/* 19.10.16 jpr change
 02.11.16 
 */