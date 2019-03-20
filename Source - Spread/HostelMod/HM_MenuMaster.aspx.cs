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

public partial class HM_MenuMaster : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    bool check = false;
    string menutype = "";
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
            txt_search.Visible = true;
            cb_menutype.Checked = true;
            //menutype = "0','1";
            BindStudentType();
            loadmenuname();

            //  loadgroupname();
            //rdb_veg1.Checked = true;
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Fpspread1.Visible = false;
            for (int i = 0; i < cbl_menutype.Items.Count; i++)
            {
                cbl_menutype.Items[i].Selected = true;
            }
            txt_menutype.Text = "Menu Type(" + cbl_menutype.Items.Count + ")";
            btn_go_Click(sender, e);
        }
        lblvalidation1.Visible = false;
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }

    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        clear();
        menucodeautogen();
        btn_save.Visible = true;
        btn_update.Visible = false;
        btn_delete.Visible = false;
        popwindow.Visible = true;
    }


    public void menucodeautogen()
    {
        try
        {
            clear();
            string newitemcode = "";
            string selectquery = "select  MenuAcr,MenuStNo,MenuSize from IM_CodeSettings order by  StartDate desc";
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string itemacronym = Convert.ToString(ds.Tables[0].Rows[0]["MenuAcr"]);
                string itemstarno = Convert.ToString(ds.Tables[0].Rows[0]["MenuStNo"]);
                string itemsize = Convert.ToString(ds.Tables[0].Rows[0]["MenuSize"]);
                selectquery = "select distinct top (1) MenuCode  from HM_MenuMaster where MenuCode like '" + Convert.ToString(itemacronym) + "%' order by MenuCode desc";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string itemcode = Convert.ToString(ds.Tables[0].Rows[0]["MenuCode"]);
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
                txt_menuid1.Enabled = false;
                txt_menuid1.Text = Convert.ToString(newitemcode);
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Set The Code Master";
            }
        }
        #region menucode old


        //    string newitemcode = "";
        //    string selectquery = "select  MenuAcr,MenuStNo,MenuSize from IM_CodeSettings ";
        //    selectquery = selectquery + " select distinct top (1) MenuCode  from HM_MenuMaster order by MenuCode desc";
        //    // selectquery = " select distinct top (1) MenuCode  from HM_MenuMaster where MenuCode like '" + Convert.ToString(menuacronym) + "%' order by MenuCode desc";
        //    ds.Clear();
        //    ds = d2.select_method_wo_parameter(selectquery, "Text");
        //    if (ds.Tables[1].Rows.Count > 0)
        //    {
        //        string itemcode = Convert.ToString(ds.Tables[1].Rows[0]["MenuCode"]);
        //        string itemacr = Convert.ToString(ds.Tables[0].Rows[0]["MenuAcr"]);
        //        string itemsize = Convert.ToString(ds.Tables[0].Rows[0]["MenuSize"]);
        //        int len = itemacr.Length;
        //        itemcode = itemcode.Remove(0, len);

        //        int len1 = Convert.ToString(itemcode).Length;

        //        string newnumber = Convert.ToString((Convert.ToInt32(itemcode) + 1));
        //        len = Convert.ToString(newnumber).Length;
        //        len1 = Convert.ToInt32(itemsize) - len;
        //        if (len1 == 2)
        //        {
        //            newitemcode = "00" + newnumber;
        //        }
        //        else if (len1 == 1)
        //        {
        //            newitemcode = "0" + newnumber;
        //        }
        //        else if (len1 == 4)
        //        {
        //            newitemcode = "0000" + newnumber;
        //        }
        //        else if (len1 == 3)
        //        {
        //            newitemcode = "000" + newnumber;
        //        }
        //        else if (len1 == 5)
        //        {
        //            newitemcode = "00000" + newnumber;
        //        }
        //        else if (len1 == 6)
        //        {
        //            newitemcode = "000000" + newnumber;
        //        }
        //        else
        //        {
        //            newitemcode = Convert.ToString(newnumber);
        //        }
        //        if (newitemcode.Trim() != "")
        //        {
        //            newitemcode = itemacr + "" + newitemcode;
        //        }
        //    }
        //    else if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        string itemstarno = Convert.ToString(ds.Tables[0].Rows[0]["MenuStNo"]);
        //        string itemacr = Convert.ToString(ds.Tables[0].Rows[0]["MenuAcr"]);
        //        string itemsize = Convert.ToString(ds.Tables[0].Rows[0]["MenuSize"]);
        //        string newnumber = Convert.ToString((Convert.ToInt32(itemstarno) + 1));
        //        int len = newnumber.Length;
        //        string items = Convert.ToString(itemsize);
        //        int len1 = Convert.ToInt32(items);

        //        int size = len1 - len;

        //        if (size == 2)
        //        {
        //            newitemcode = "00" + newnumber;
        //        }
        //        else if (size == 1)
        //        {
        //            newitemcode = "0" + newnumber;
        //        }
        //        else if (size == 4)
        //        {
        //            newitemcode = "0000" + newnumber;
        //        }
        //        else if (size == 3)
        //        {
        //            newitemcode = "000" + newnumber;
        //        }
        //        else if (size == 5)
        //        {
        //            newitemcode = "00000" + newnumber;
        //        }
        //        else if (size == 6)
        //        {
        //            newitemcode = "000000" + newnumber;
        //        }
        //        else
        //        {
        //            newitemcode = Convert.ToString(itemstarno);
        //        }
        //        newitemcode = Convert.ToString(itemacr) + "" + Convert.ToString(newitemcode);
        //    }
        //    else
        //    {
        //        imgdiv2.Visible = true;
        //        lbl_alert.Text = "Please Set The Code Master";
        //    }
        //    txt_menuid1.Enabled = false;
        //    txt_menuid1.Text = Convert.ToString(newitemcode);
        //}
        #endregion
        catch
        {

        }
    }
    protected void btn_exit_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            string dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            string menuid = Convert.ToString(txt_menuid1.Text).ToUpper();
            string groupname = "";
            if (groupname.Trim() == "Others")
            {
                groupname = Convert.ToString(txt_group.Text);
            }
            else if (groupname.Trim() == "Select")
            {
                groupname = "";
            }
            string menuname = Convert.ToString(txt_menuname1.Text.First().ToString().ToUpper() + txt_menuname1.Text.Substring(1));
            menuname = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(menuname);
            string menutype = Convert.ToString(ddlStudType.SelectedValue); //barath 15.02.18
            //if (rdb_nonveg1.Checked == true)
            //{
            //    menutype = "1";
            //}
            //else
            //{
            //    menutype = "0";
            //}
            string isprint = string.Empty;
            if (cb_printtoken1.Checked == true)
            {
                isprint = "1";
            }
            else
            {
                isprint = "0";
            }
            //string insertquery = "insert into MenuMaster (Access_Date,Access_Time,MenuID,MenuName,GroupName,MenuType,Is_PrintToken,College_Code)values ('" + dtaccessdate + "','" + dtaccesstime + "','" + menuid + "','" + menuname + "','" + groupname + "','" + menutype + "','" + isprint + "','" + collegecode1 + "')";
            string insertquery = "insert into HM_MenuMaster (MenuCode,MenuName,MenuType,CollegeCode)values ('" + menuid + "','" + menuname + "','" + menutype + "','" + collegecode1 + "')";
            int ints = d2.update_method_wo_parameter(insertquery, "Text");
            if (ints != 0)
            {
                loadmenuname();
                //loadgroupname();
                imgdiv2.Visible = true;
                lbl_alert.Text = "Saved Successfully";
                btn_addnew_Click(sender, e);
                btn_go_Click(sender, e);
                menucodeautogen();

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
        string query = "select distinct MenuName from HM_MenuMaster where MenuName like '" + prefixText + "%'";
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

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getmenu(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select distinct  MenuCode  from HM_MenuMaster where   MenuCode like '%" + prefixText + "%'";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["MenuCode"].ToString());
            }
        }
        return name;
    }

    [WebMethod]
    public static string CheckUserName(string MenuName)
    {
        string returnValue = "1";
        try
        {
            DAccess2 dd = new DAccess2();
            string user_name = MenuName;
            if (user_name.Trim() != "" && user_name != null)
            {
                string query = dd.GetFunction("select distinct MenuName,MenuMasterPK from HM_MenuMaster where MenuName ='" + user_name + "'");
                if (query.Trim() == "" || query == null || query == "0" || query == "-1")
                {
                    returnValue = "0";
                }
            }
            else
            {
                returnValue = "2";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            string itemheadercode = "";
            for (int i = 0; i < cbl_menuname.Items.Count; i++)
            {
                if (cbl_menuname.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_menuname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cbl_menuname.Items[i].Value.ToString() + "";
                    }
                }
            }

            string itemheadercode1 = "";
            for (int i = 0; i < cbl_menutype.Items.Count; i++)
            {
                if (cbl_menutype.Items[i].Selected == true)
                {
                    if (itemheadercode1 == "")
                    {
                        itemheadercode1 = "" + cbl_menutype.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode1 = itemheadercode1 + "'" + "," + "'" + cbl_menutype.Items[i].Value.ToString() + "";
                    }
                }
            }

            string selectquery = "";
            if (txt_menuid.Text.Trim() != "")
            {
                selectquery = " select (select StudentTypeName From HostelStudentType where StudentType-1=MenuType)  as MenuType,MenuType as MenuTypecode,MenuCode,MenuName,MenuMasterPK from HM_MenuMaster where CollegeCode='" + collegecode1 + "' and MenuCode like '%" + txt_menuid.Text + "%'  order by MenuCode";
            }
            else if (txt_search.Text.Trim() != "")
            {
                selectquery = "select (select StudentTypeName From HostelStudentType where StudentType-1=MenuType)  as MenuType,MenuType as MenuTypecode,MenuCode,MenuName,MenuMasterPK  from HM_MenuMaster where CollegeCode='" + collegecode1 + "' and MenuName like '%" + txt_search.Text + "%' order by MenuCode";
            }
            else
            {
                if (itemheadercode.Trim() != "" && itemheadercode1.Trim() != "")
                {
                    selectquery = "select (select StudentTypeName From HostelStudentType where StudentType-1=MenuType)  as MenuType,MenuType as MenuTypecode,MenuCode,MenuName ,MenuMasterPK from HM_MenuMaster  where CollegeCode='" + collegecode1 + "' and MenuCode in ('" + itemheadercode + "') and MenuType in ('" + itemheadercode1 + "') order by MenuCode";
                }//case when MenuType ='0' then 'Veg' else 'Non Veg' end
                else
                {
                    div1.Visible = false;
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "Please Select All Field";
                }
            }
            ds.Clear();
            if (selectquery.Trim() != "")
            {
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Fpspread1.Sheets[0].RowCount = 0;
                    Fpspread1.Sheets[0].ColumnCount = 0;
                    Fpspread1.CommandBar.Visible = false;
                    Fpspread1.Sheets[0].AutoPostBack = true;
                    Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread1.Sheets[0].RowHeader.Visible = false;
                    Fpspread1.Sheets[0].ColumnCount = 4;
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

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Menu Type";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;


                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Menu Code";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Menu Name";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;


                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        Fpspread1.Sheets[0].RowCount++;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["MenuType"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["MenuMasterPK"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["MenuCode"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["MenuTypecode"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["MenuName"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

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
    protected void Fpspread1_render(object sender, EventArgs e)
    {
        try
        {
            if (check == true)
            {
                popwindow.Visible = true;
                btn_save.Visible = false;
                btn_update.Visible = true;
                btn_delete.Visible = true;
                string activerow = "";
                string activecol = "";
                activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                collegecode = Session["collegecode"].ToString();
                if (activerow.Trim() != "")
                {
                    string menupk = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                    Session["menupk"] = Convert.ToString(menupk);

                    string MenuType = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    string menucode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                    string menuname = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);

                    int MenuTypeCode = 0;
                    int.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag), out MenuTypeCode);
                    // Session["menucode"] = Convert.ToString(menucode);

                    //string isprinttoken = d2.GetFunction("select Is_PrintToken  from MenuMaster where College_Code ='" + collegecode + "' and MenuCode ='" + menucode + "'");
                    //if (isprinttoken.Trim() != "" && isprinttoken.Trim() != "0")
                    //{
                    //    cb_printtoken1.Checked = true;
                    //}
                    //if (Groupname.Trim() != "")
                    //{
                    //    ddl_group1.SelectedItem.Text = Convert.ToString(Groupname);
                    //}
                    //else
                    //{
                    //    ddl_group1.SelectedIndex = 0;
                    //}
                    txt_menuid1.Text = Convert.ToString(menucode);
                    txt_menuname1.Text = Convert.ToString(menuname);
                    //Barath 15.02.18
                    ddlStudType.SelectedIndex = ddlStudType.Items.IndexOf(ddlStudType.Items.FindByValue(Convert.ToString(MenuTypeCode)));
                    //if (MenuType.Trim() == "Veg")
                    //{
                    //    rdb_veg1.Checked = true;
                    //    rdb_nonveg1.Checked = false;
                    //}
                    //else
                    //{
                    //    rdb_nonveg1.Checked = true;
                    //    rdb_veg1.Checked = false;
                    //}
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

            string degreedetails = "Menu Master Report";
            string pagename = "HM_MenuMaster.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            printdiv.Visible = true;
            Printcontrol.Visible = true;
            // 
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

    protected void btn_update_Click(object sender, EventArgs e)
    {
        try
        {
            string dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            string menuid = Convert.ToString(txt_menuid1.Text);
            string groupname = "";
            collegecode = Session["collegecode"].ToString();
            if (groupname.Trim() == "Others")
            {
                groupname = Convert.ToString(txt_group.Text);
            }
            else if (groupname.Trim() == "Select")
            {
                groupname = "";
            }
            string menuname = Convert.ToString(txt_menuname1.Text.First().ToString().ToUpper() + txt_menuname1.Text.Substring(1));
            string menutype = Convert.ToString(ddlStudType.SelectedValue);//barath 15.02.18
            //if (rdb_nonveg1.Checked == true)
            //    menutype = "1";
            //else
            //    menutype = "0";
            string isprint = "";
            if (cb_printtoken1.Checked == true)
            {
                isprint = "1";
            }
            else
            {
                isprint = "0";
            }
            //string insertquery = "update MenuMaster set Access_Date ='" + dtaccessdate + "',Access_Time='" + dtaccesstime + "',MenuID='" + menuid + "',MenuName ='" + menuname + "',GroupName='" + groupname + "' ,MenuType ='" + menutype + "' ,Is_PrintToken ='" + isprint + "' where College_Code ='" + collegecode + "' and MenuCode ='" + Convert.ToString(Session["menucode"]) + "'";


            string insertquery = "update HM_MenuMaster set MenuType ='" + menutype + "', MenuName ='" + menuname + "' where CollegeCode ='" + collegecode + "' and MenuMasterPK ='" + Convert.ToString(Session["menupk"]) + "'";
            int ints = d2.update_method_wo_parameter(insertquery, "Text");
            if (ints != 0)
            {
                // btn_go_Click(sender, e);
                loadmenuname();
                // loadgroupname();
                imgdiv2.Visible = true;
                lbl_alert.Text = "Updated Successfully";
                btn_go_Click(sender, e);
                popwindow.Visible = false;
                //ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Updated Sucessfully\");", true);
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
            string delqquery = "delete from HM_MenuMaster where MenuMasterPK ='" + Convert.ToString(Session["menupk"]) + "'";
            int ints = d2.update_method_wo_parameter(delqquery, "Text");
            if (ints != 0)
            {
                loadmenuname();
                // loadgroupname();
                imgdiv2.Visible = true;
                lbl_alert.Text = "Deleted Successfully";
                btn_go_Click(sender, e);
                popwindow.Visible = false;
            }
        }
        catch
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Can't Deleted. Because this Menu using another process";
        }
    }
    protected void btn_delete_Click(object sender, EventArgs e)
    {
        try
        {
            if (btn_delete.Text == "Delete")
            {
                surediv.Visible = true;
                lbl_sure.Text = "Do you want to Delete this Record?";
            }
        }
        catch
        {
        }
    }

    public void clear()
    {
        try
        {
            txt_menuid1.Text = "";
            txt_menuname1.Text = "";
            //rdb_veg1.Checked = true;
        }
        catch
        {
        }
    }

    //public void loadgroupname()
    //{
    //    try
    //    {
    //        string deptquery = "select distinct GroupName from MenuMaster where GroupName <>'' order by GroupName ";
    //        ds.Clear();
    //        ds = d2.select_method_wo_parameter(deptquery, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            ddl_group1.DataSource = ds;
    //            ddl_group1.DataTextField = "GroupName";
    //            ddl_group1.DataValueField = "GroupName";
    //            ddl_group1.DataBind();
    //            ddl_group1.Items.Insert(0, "Select");
    //            ddl_group1.Items.Insert(ddl_group1.Items.Count, "Others");
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}
    protected void cb_menutype_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_menutype.Checked == true)
            {
                for (int i = 0; i < cbl_menutype.Items.Count; i++)
                {
                    cbl_menutype.Items[i].Selected = true;
                }
                txt_menutype.Text = "Menu Type(" + (cbl_menutype.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_menutype.Items.Count; i++)
                {
                    cbl_menutype.Items[i].Selected = false;
                }
                txt_menutype.Text = "--Select--";
            }
            loadmenuname();
        }
        catch
        {
        }
    }
    protected void cbl_menutype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_menutype.Text = "--Select--";
            cb_menutype.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_menutype.Items.Count; i++)
            {
                if (cbl_menutype.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_menutype.Text = "Menu Type(" + commcount.ToString() + ")";
                if (commcount == cbl_menutype.Items.Count)
                {
                    cb_menutype.Checked = true;
                }
            }
            loadmenuname();
        }
        catch
        {
        }
    }

    public void loadmenuname()
    {
        try
        {
            //string deptquery = "select distinct MenuCode,MenuName  from MenuMaster where College_Code ='" + collegecode1 + "' order by MenuCode ";

            //menutype = "";          
            for (int i = 0; i < cbl_menutype.Items.Count; i++)
            {
                if (cbl_menutype.Items[i].Selected == true)
                {
                    if (menutype == "")
                    {
                        menutype = "" + cbl_menutype.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        menutype = menutype + "'" + "," + "'" + cbl_menutype.Items[i].Value.ToString() + "";
                    }
                }
                else if (cbl_menutype.Items[i].Selected == false)
                {
                    txt_menuname.Text = "--Select--";
                    cbl_menuname.Items.Clear();
                    cbl_menuname_SelectedIndexChanged(sender, e);
                    //menutype = "2";
                }
            }
            if (menutype == "")
            {
                menutype = "1000";
            }

            string deptquery = "select distinct MenuCode,MenuName from HM_MenuMaster where CollegeCode ='" + collegecode1 + "' and MenuType in ('" + menutype + "') order by MenuName";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            //ds = d2.BindMenuName(collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_menuname.DataSource = ds;
                cbl_menuname.DataTextField = "MenuName";
                cbl_menuname.DataValueField = "MenuCode";
                cbl_menuname.DataBind();
                if (cbl_menuname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_menuname.Items.Count; i++)
                    {
                        cbl_menuname.Items[i].Selected = true;
                    }
                    txt_menuname.Text = "Menu Name(" + cbl_menuname.Items.Count + ")";
                }
            }
        }
        catch
        {
        }
    }
    protected void cb_menuname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_menuname.Checked == true)
            {
                for (int i = 0; i < cbl_menuname.Items.Count; i++)
                {
                    cbl_menuname.Items[i].Selected = true;
                }
                txt_menuname.Text = "Menu Name(" + (cbl_menuname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_menuname.Items.Count; i++)
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
            txt_menuname.Text = "--Select--";
            cb_menuname.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_menuname.Items.Count; i++)
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
        popwindow.Visible = true;
    }

    public object sender { get; set; }
    public EventArgs e { get; set; }
    protected void ddl_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_type.SelectedValue == "0")
        {
            txt_search.Visible = true;
            txt_menuid.Visible = false;

            txt_menuid.Text = "";
        }
        else if (ddl_type.SelectedValue == "1")
        {
            txt_search.Visible = false;
            txt_menuid.Visible = true;
            txt_search.Text = "";
        }
    }
    protected void btnplus_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addgroup.Visible = true;
        lblerror.Visible = false;
    }
    protected void btnminus_Click(object sender, EventArgs e)
    {
        surediv_del.Visible = true;
        lbl_suredel.Text = "Do You Want to Delete This Record?";
    }
    protected void btn_addgroup_Click(object sender, EventArgs e)
    {
        try
        {
            string group = Convert.ToString(txtStudentType.Text);
            group = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(group);
            if (txtStudentType.Text != "")
            {
                string sql = "if not exists(select *from HostelStudentType where StudentTypeName='" + group + "' and CollegeCode='" + collegecode1 + "' )insert into HostelStudentType (StudentTypeName,collegecode) values('" + group + "','" + collegecode1 + "')";
                int insert = d2.update_method_wo_parameter(sql, "Text");
                if (insert != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Saved Successfully";
                    plusdiv.Visible = false;
                    panel_addgroup.Visible = false;
                    txtStudentType.Text = "";
                }
                BindStudentType();
            }
            else
            {
                plusdiv.Visible = true;
                lblerror.Visible = true;
                lblerror.Text = "Enter the StudentType";
                //lblalerterr.Text = "Enter the Group Name";
            }
        }
        catch
        { }
    }
    protected void BindStudentType()
    {
        try
        {
            ddlStudType.Items.Clear();
            ds.Clear();
            string sql = "select StudentType-1 as StudentType,StudentTypeName from HostelStudentType where CollegeCode='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlStudType.DataSource = ds;
                ddlStudType.DataTextField = "StudentTypeName";
                ddlStudType.DataValueField = "StudentType";
                ddlStudType.DataBind();
                cbl_menutype.DataSource = ds;
                cbl_menutype.DataTextField = "StudentTypeName";
                cbl_menutype.DataValueField = "StudentType";
                cbl_menutype.DataBind();
                for (int i = 0; i < cbl_menutype.Items.Count; i++)
                {
                    cbl_menutype.Items[i].Selected = true;
                    if (cbl_menutype.Items[i].Selected == true)
                    {
                        txt_menutype.Text = lbl_menutype.Text + " (" + Convert.ToString(cbl_menutype.Items.Count) + ")";
                        cb_menutype.Checked = true;
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void btn_suredel_Click(object sender, EventArgs e)
    {
        try
        {
            surediv_del.Visible = false;
            if (ddlStudType.SelectedIndex != -1)
            {
                string sql = " delete HostelStudentType where studenttype='" + Convert.ToString(ddlStudType.SelectedItem.Value) + "' and CollegeCode='" + collegecode1 + "'";
                int delete = d2.update_method_wo_parameter(sql, "Text");
                if (delete != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Deleted Successfully";
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "No Record Selected";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "No Record Selected";
            }
            BindStudentType();
        }
        catch
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No Record Selected";
        }
    }
    protected void btn_delno_Click(object sender, EventArgs e)
    {
        surediv_del.Visible = false;
    }
    protected void btn_exitaddgroup_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = false;
        panel_addgroup.Visible = false;
    }
}