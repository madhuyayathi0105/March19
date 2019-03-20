using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Configuration;
public partial class inv_daily_consumption : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    Hashtable itemcoderepect = new Hashtable();
    bool check = false;
    string grouporusercode = "";
    string firstdate = "";
    DateTime dt = new DateTime();
    bool chk = false;
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
            txt_menutype.Enabled = true;
            for (int i = 0; i < cbl_menutype.Items.Count; i++)
            {
                cbl_menutype.Items[i].Selected = true;
            }
            txt_menutype.Text = "Menu Type(" + cbl_menutype.Items.Count + ")";
            bindhostelname();
            rdb_menuitemcon.Checked = true;
            txt_date.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            txt_date.Attributes.Add("readonly", "readonly");
            txt_itemcode2.Attributes.Add("readonly", "readonly");
            txt_itemname2.Attributes.Add("readonly", "readonly");
            txtDirDt.Attributes.Add("readonly", "readonly");
            bindsession();
            if (rdb_menuitemcon.Checked == true)
            {
                loadmenuname();
            }
            if (rdb_cleanitem.Checked == true)
            {
                loaditemname();
            }
            loadaddtional();
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Visible = false;
            labltotallable.Visible = false;
            rptprint.Visible = false;
            btn_go_Click(sender, e);
            ViewState["Reconsumption"] = null;
            ViewState["Reconsumption"] = d2.GetFunction("select value from Master_Settings where settings='Daily Consumption Allow Reconsumption' and usercode='" + usercode + "'");
            bindPurposeCatagory();
            cb_Additionalitem_check(sender, e);
        }
        lbl_error1.Visible = false;
        lblDirNorec.Visible = false;
        lblDirErr1.Visible = false;
    }
    protected void loadaddtional()
    {
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
        }
        else
        {
            grouporusercode = " and usercode=" + Session["usercode"].ToString().Trim() + "";
        }
        string Master = d2.GetFunction("select value from Master_Settings where settings='Additional Item Rights' " + grouporusercode + "");
        if (Master.Trim() != "0")
        {
            cb_Additionalitem.Enabled = true;
        }
        else
        {
            cb_Additionalitem.Enabled = false;
        }
    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }
    protected void imgbtnclose1_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
    }
    protected void lnk_btnlogout_click(object sender, EventArgs e)
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
    protected void FpSpread1_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        if (cb_Additionalitem.Checked)
            Common_Command(FpSpread1, 7);
        else
            Common_Command(FpSpread1, 9);
    }
    protected void cb_session_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_sessionname, cbl_session, txt_sessionname, "Session Name");
        if (rdb_menuitemcon.Checked == true)
        {
            loadmenuname();
        }
        if (rdb_cleanitem.Checked == true)
        {
            loaditemname();
        }
    }
    protected void cbl_session_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_sessionname, cbl_session, txt_sessionname, "Session Name");
        if (rdb_menuitemcon.Checked == true)
        {
            loadmenuname();
        }
        if (rdb_cleanitem.Checked == true)
        {
            loaditemname();
        }
    }
    public void loadmenuname()
    {
        try
        {
            hat.Clear();
            string item = "";
            txt_menuname.Text = "--Select--";
            //string itemheadercode = "";
            cbl_menuname.Items.Clear();
            string hostelcode = "";
            for (int i = 0; i < cbl_session.Items.Count; i++)
            {
                if (cbl_session.Items[i].Selected == true)
                {
                    if (item == "")
                    {
                        item = "" + cbl_session.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        item = item + "'" + "," + "'" + cbl_session.Items[i].Value.ToString() + "";
                    }
                }
            }
            hostelcode = Convert.ToString(ddl_hostelname.SelectedItem.Value);
            if (item.Trim() != "")
            {
                string firstdate = Convert.ToString(txt_date.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                if (item.Trim() != "" && hostelcode.Trim() != "")
                {
                    string menuquery = "";
                    //menuquery = "select Menu_Code,Session_Code from MenuSchedule_DateWise where Session_Code in ('" + item + "') and Hostel_Code in ('" + hostelcode + "') and Schedule_Date ='" + dt.ToString("MM/dd/yyyy") + "' and schedule_type='0'";
                    //menuquery = menuquery + " select Menu_Code,Session_Code from MenuSchedule_DayWise where Session_Code in ('" + item + "') and Hostel_Code in ('" + hostelcode + "') and Schedule_Day ='" + dt.ToString("dddd") + "' and schedule_type='0'";
                    ds.Clear();
                    menuquery = "select MenuMasterFK,SessionMasterFK from HT_MenuSchedule where SessionMasterFK in('" + item + "') and MessMasterFK in('" + hostelcode + "')  and ScheudleItemType='1' and ScheduleType ='1' and MenuScheduleDate ='" + dt.ToString("MM/dd/yyyy") + "'";
                    menuquery = menuquery + "  select MenuMasterFK,SessionMasterFK from HT_MenuSchedule where SessionMasterFK in('" + item + "') and MessMasterFK in('" + hostelcode + "')  and ScheudleItemType='1' and ScheduleType ='2' and MenuScheduleday ='" + dt.ToString("dddd") + "'";
                    ds = d2.select_method_wo_parameter(menuquery, "Text");
                    string menucode = "";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                        {
                            string mcode = Convert.ToString(ds.Tables[0].Rows[k]["MenuMasterFK"]);
                            if (menucode.Contains(mcode) == false)
                            {
                                if (menucode == "")
                                {
                                    menucode = mcode;
                                }
                                else
                                {
                                    menucode = menucode + "'" + "," + "'" + mcode;
                                }
                            }
                        }
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int k = 0; k < ds.Tables[1].Rows.Count; k++)
                        {
                            string mcode = Convert.ToString(ds.Tables[1].Rows[k]["MenuMasterFK"]);
                            if (menucode.Contains(mcode) == false)
                            {
                                if (menucode == "")
                                {
                                    menucode = mcode;
                                }
                                else
                                {
                                    menucode = menucode + "'" + "," + "'" + mcode;
                                }
                            }
                        }
                    }
                    //}
                    string menutype = "";
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
                    }
                    if (menutype.Trim() == "")
                    {
                        menutype = "2";
                    }
                    string deptquery = "select distinct MenuMasterPK,MenuName,MenuCode  from HM_MenuMaster where CollegeCode ='" + collegecode1 + "' and MenuMasterPK in('" + menucode + "') and MenuType in('" + menutype + "')  order by MenuName ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(deptquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cbl_menuname.DataSource = ds;
                        cbl_menuname.DataTextField = "MenuName";
                        cbl_menuname.DataValueField = "MenuMasterPK";
                        cbl_menuname.DataBind();
                        if (cbl_menuname.Items.Count > 0)
                        {
                            for (int i = 0; i < cbl_menuname.Items.Count; i++)
                            {
                                cbl_menuname.Items[i].Selected = true;
                            }
                            txt_menuname.Text = "Menu Name(" + cbl_menuname.Items.Count + ")";
                            lbl_menuname.Text = "Menu Name";
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void cb_menuname_CheckedChange(object sender, EventArgs e)
    {
        if (cb_menuname.Checked == true)
        {
            if (cbl_menuname.Items.Count > 0)
            {
                for (int i = 0; i < cbl_menuname.Items.Count; i++)
                {
                    cbl_menuname.Items[i].Selected = true;
                }
                if (rdb_menuitemcon.Checked == true)
                {
                    txt_menuname.Text = "Menu Name(" + (cbl_menuname.Items.Count) + ")";
                }
                else
                {
                    txt_menuname.Text = "Item Name(" + (cbl_menuname.Items.Count) + ")";
                }
            }
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
    protected void cbl_menuname_SelectedIndexChange(object sender, EventArgs e)
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
            if (rdb_menuitemcon.Checked == true)
            {
                txt_menuname.Text = "Menu Name(" + commcount.ToString() + ")";
            }
            else
            {
                txt_menuname.Text = "Item Name(" + commcount.ToString() + ")";
            }
            if (commcount == cbl_menuname.Items.Count)
            {
                cb_menuname.Checked = true;
            }
        }
    }
    public void loaditemname()
    {
        try
        {
            hat.Clear();
            string item = "";
            txt_menuname.Text = "--Select--";
            cbl_menuname.Items.Clear();
            string hostelcode = "";
            for (int i = 0; i < cbl_session.Items.Count; i++)
            {
                if (cbl_session.Items[i].Selected == true)
                {
                    if (item == "")
                    {
                        item = "" + cbl_session.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        item = item + "'" + "," + "'" + cbl_session.Items[i].Value.ToString() + "";
                    }
                }
            }
            hostelcode = Convert.ToString(ddl_hostelname.SelectedItem.Value);
            lbl_menuname.Text = "Item Name";
            if (item.Trim() != "")
            {
                string firstdate = Convert.ToString(txt_date.Text);
                DateTime dt = new DateTime();
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                if (item.Trim() != "" && hostelcode.Trim() != "")
                {
                    string menuquery = "";
                    string menucode = "";
                    menuquery = "select MenuMasterFK,SessionMasterFK from HT_MenuSchedule where SessionMasterFK in('" + item + "') and MessMasterFK in('" + hostelcode + "')  and ScheudleItemType='2' and ScheduleType ='1' and MenuScheduleDate ='" + dt.ToString("MM/dd/yyyy") + "'";
                    menuquery = menuquery + "  select MenuMasterFK,SessionMasterFK from HT_MenuSchedule where SessionMasterFK in('" + item + "') and MessMasterFK in('" + hostelcode + "')  and ScheudleItemType='2' and ScheduleType ='2' and MenuScheduleday ='" + dt.ToString("dddd") + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(menuquery, "Text");
                    menuquery = "";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                        {
                            string mcode = Convert.ToString(ds.Tables[0].Rows[k]["MenuMasterFK"]);
                            if (menucode.Contains(mcode) == false)
                            {
                                if (menucode == "")
                                {
                                    menucode = mcode;
                                }
                                else
                                {
                                    menucode = menucode + "'" + "," + "'" + mcode;
                                }
                            }
                        }
                    }
                    //else
                    //{
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int k = 0; k < ds.Tables[1].Rows.Count; k++)
                        {
                            string mcode = Convert.ToString(ds.Tables[1].Rows[k]["MenuMasterFK"]);
                            if (menucode.Contains(mcode) == false)
                            {
                                if (menucode == "")
                                {
                                    menucode = mcode;
                                }
                                else
                                {
                                    menucode = menucode + "'" + "," + "'" + mcode;
                                }
                            }
                        }
                    }
                    //}
                    string deptquery = "select distinct ItemCode,ItemPK,itemname from IM_ItemMaster  where  itempk in('" + menucode + "')  order by ItemName";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(deptquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cbl_menuname.DataSource = ds;
                        cbl_menuname.DataTextField = "itemname";
                        cbl_menuname.DataValueField = "ItemPK";
                        cbl_menuname.DataBind();
                        if (cbl_menuname.Items.Count > 0)
                        {
                            for (int i = 0; i < cbl_menuname.Items.Count; i++)
                            {
                                cbl_menuname.Items[i].Selected = true;
                            }
                            txt_menuname.Text = "Item Name(" + cbl_menuname.Items.Count + ")";
                            lbl_menuname.Text = "Item Name";
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void txt_date_Change(object sender, EventArgs e)
    {
        try
        {
            bindsession();
            if (rdb_menuitemcon.Checked == true)
            {
                loadmenuname();
            }
            if (rdb_cleanitem.Checked == true)
            {
                loaditemname();
            }
        }
        catch
        {
        }
    }
    protected void rdb_menuitemcon_CheckedChange(object sender, EventArgs e)
    {
        if (rdb_menuitemcon.Checked == true)
        {
            FpSpread1.Visible = false;
            labltotallable.Visible = false;
            rptprint.Visible = false;
            btn_save.Visible = false;
            loadmenuname();
            txt_menutype.Enabled = true;
        }
        else
        {
            FpSpread1.Visible = false;
            labltotallable.Visible = false;
            rptprint.Visible = false;
            btn_save.Visible = false;
            txt_menutype.Enabled = false;
        }
    }
    protected void rdb_cleanitem_check(object sender, EventArgs e)
    {
        if (rdb_cleanitem.Checked == true)
        {
            FpSpread1.Visible = false;
            labltotallable.Visible = false;
            rptprint.Visible = false;
            btn_save.Visible = false;
            loaditemname();
            txt_menutype.Enabled = false;
        }
        else
        {
            txt_menutype.Enabled = true;
            FpSpread1.Visible = true;
            rptprint.Visible = true;
            btn_save.Visible = true;
        }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string itemheadercode = "";
            string hostelcode = "";
            string menuvalue = "";
            Printcontrol.Visible = false;
            itemheadercode = GetSelectedItemsValueAsString(cbl_session);
            hostelcode = Convert.ToString(ddl_hostelname.SelectedItem.Value);
            menuvalue = GetSelectedItemsValueAsString(cbl_menuname);
            string firstdate = Convert.ToString(txt_date.Text);
            DateTime dt = new DateTime();
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            DataView dv = new DataView();
            DataView dv1 = new DataView();
            ArrayList Addvalue = new ArrayList();
            Hashtable hashset = new Hashtable();
            FpSpread1.SaveChanges();
            if (itemheadercode.Trim() != "" && hostelcode.Trim() != "" && menuvalue.Trim() != "")
            {
                if (cb_Additionalitem.Checked == false)
                {
                    if (rdb_menuitemcon.Checked == true)
                    {
                        #region menuItem
                        string selectquery = "select i.ItemCode,d.ItemFK,itemname,IssuedQty-ISNULL(UsedQty,'0') as hand_qty,i.itemunit,s.IssuedRPU from HM_MenuItemDetail d, HM_MenuItemMaster m,HM_MenuMaster u,IM_ItemMaster i,HM_MessMaster h,IT_StockDeptDetail s WHERE D.MenuItemMasterFK =M.MenuItemMasterPK and i.ItemPK=d.ItemFK and m.MenuMasterFK=u.MenuMasterPK and s.ItemFK=i.ItemPK and s.ItemFK=d.ItemFK and s.DeptFK in('" + hostelcode + "') and u.MenuMasterPK in ('" + menuvalue + "') and s.DeptFK =h.MessMasterPK and h.MessMasterPK=m.MessMasterFK group by i.ItemCode,d.ItemFK,itemname,IssuedQty,UsedQty,i.itemunit,IssuedRPU";//d.ItemFK in('" + menuvalue + "')"; 20.07.16 ,m.NoOfPerson
                        // selectquery = selectquery + "   select MenuScheduleday,MenuMasterFK,change_strength, MessMasterFK ,SessionMasterFK from HT_MenuSchedule where ScheudleItemType='1' and ScheduleType='2'";
                        selectquery += "  select MenuScheduleday,MenuScheduleDate,ScheduleType,MenuMasterFK,change_strength, MessMasterFK ,SessionMasterFK,(HostelVegCount+DayscholorVegCount+GuestVegCount+StaffVegCount) as vegCount,(HostelNonvegCount+ DayscholorNonvegCount+GuestNonvegCount+StaffNonvegCount) as nonVegCount,m.MenuType,convert(varchar(10), Strengthdate,101)Strengthdate from HM_MenuMaster m,HT_MenuSchedule ms,HT_HostelStudMenuStrength msm where msm.MenuscheduleFK=ms.MenuSchedulePK and m.MenuMasterPK=ms.MenuMasterFK  and ScheudleItemType='1'  and m.MenuMasterPK in ('" + menuvalue + "')";//and ScheduleType='2'
                        selectquery += "   select  i.ItemCode,itemname,i.itemunit,BalQty as hand_qty, mu.MenuMasterPK,MessMasterPK  from HM_MenuItemMaster m ,HM_MenuItemDetail mi,IM_ItemMaster i,IT_StockDeptDetail s,HM_MenuMaster mu,HM_MessMaster h where m.MenuItemMasterPK  =Mi.MenuItemMasterFK and mi.ItemFK =i.ItemPK and mi.ItemFK=s.ItemFK and m.MenuMasterFK=mu.MenuMasterPK and m.MenuMasterFK=mu.MenuMasterPK and s.DeptFK =h.MessMasterPK and h.MessMasterPK in ('" + hostelcode + "') and mu.MenuMasterPK in ('" + menuvalue + "') and h.MessMasterPK=m.MessMasterFK ";// mi.itemfk in('" + menuvalue + "')"; 20.07.16 m.NoOfPerson,mi.NeededQty,
                        //selectquery += "  select distinct s.SessionMasterPK ,s.SessionName +'(Student :'+ ISNULL( hostler,'0') +',Day Scholar :'+ISNULL( DayScholor,'0')+',Staff :'+ISNULL( Staffcount,'0') +',Guest :'+ISNULL( Guestcount,'0')+',Total:'+(change_strength)+' )' as change_strength from HT_MenuSchedule d,HM_SessionMaster s where ScheudleItemType  ='1'  and d.SessionMasterFK in ('" + itemheadercode + "') and d.SessionMasterFK  =s.SessionMasterPK  and MenuScheduleday ='" + dt.ToString("dddd") + "'";
                        selectquery += " select distinct s.SessionMasterPK ,s.SessionName +'(Student :'+ ISNULL( hostler,'0') +',Day Scholar :'+ISNULL( DayScholor,'0')+',Staff :'+ISNULL( Staffcount,'0') +',Guest :'+ISNULL( Guestcount,'0')+',Total:'+(change_strength)+' )' as change_strength from HT_MenuSchedule d,HM_SessionMaster s  ,HT_HostelStudMenuStrength msm where msm.MenuscheduleFK=d.MenuSchedulePK and ScheudleItemType  ='1'  and d.SessionMasterFK in ('" + itemheadercode + "') and d.SessionMasterFK  =s.SessionMasterPK  and MenuScheduleday ='" + dt.ToString("dddd") + "'  and Strengthdate='" + dt.ToString("MM/dd/yyyy") + "'";
                        //selectquery = selectquery + "  select distinct s.SessionMasterPK ,s.SessionName +'(Student Veg:'+ convert(varchar(100),ISNULL( ss.HostelVegCount,'0'))+' Non Veg' +convert(varchar(100),ISNULL( ss.HostelNonvegCount,'0'))+',Day Scholar Veg:'+convert(varchar(100),ISNULL( DayscholorVegCount,'0'))+' Non Veg '+ convert(varchar(100),ISNULL( DayscholorNonvegCount,'0'))+',Staff Veg:'+convert(varchar(100),ISNULL( StaffVegCount,'0'))+' Non Veg '+ convert(varchar(100),ISNULL( StaffNonvegCount,'0'))+',Guest Veg:'+convert(varchar(100),ISNULL( GuestVegCount,'0'))+' Non Veg '+ convert(varchar(100),ISNULL( GuestNonvegCount,'0'))+',Total:'+(change_strength)+' )' as change_strength from HT_MenuSchedule d,HM_SessionMaster s,HT_HostelStudMenuStrength ss where d.MenuSchedulePK=ss.MenuscheduleFK and ScheudleItemType  ='1' and d.SessionMasterFK in ('" + itemheadercode + "') and d.SessionMasterFK  =s.SessionMasterPK  and MenuScheduleday ='" + dt.ToString("dddd") + "'";
                        selectquery += "   select NeededQty,NoOfPerson ,ItemFK,MenuMasterFK from HM_MenuItemDetail m,HM_MenuItemMaster h where h.MenuItemMasterPK =m.MenuItemMasterFK  and MenuMasterFK in ('" + menuvalue + "') and h.MessMasterFK in('" + hostelcode + "')";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            #region Header
                            FpSpread1.Sheets[0].RowCount = 0;
                            FpSpread1.Sheets[0].ColumnCount = 0;
                            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                            FpSpread1.CommandBar.Visible = false;
                            FpSpread1.Sheets[0].ColumnCount = 10;
                            FpSpread1.Sheets[0].AutoPostBack = false;
                            FpSpread1.Sheets[0].RowHeader.Visible = false;
                            FpSpread1.Width = 980;
                            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            darkstyle.ForeColor = Color.White;
                            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Columns[0].Width = 50;
                            FpSpread1.Columns[0].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Item Code";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Columns[1].Width = 100;
                            FpSpread1.Columns[1].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Name";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[2].Width = 200;
                            FpSpread1.Columns[2].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Item Measure";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[3].Width = 63;
                            FpSpread1.Columns[3].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Item in Hand";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[4].Width = 100;
                            FpSpread1.Columns[4].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Total Strength";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[5].Width = 125;
                            FpSpread1.Columns[5].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 5].Text = "Veg";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 5].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 5].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[5].Width = 100;
                            FpSpread1.Columns[5].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Text = "Non Veg";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[6].Width = 100;
                            FpSpread1.Columns[6].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Required Quantity";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[7].Width = 100;
                            FpSpread1.Columns[7].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].Text = "Veg";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[7].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 8].Text = "Non Veg";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 8].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 8].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 8].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[8].Locked = true;
                            FpSpread1.Columns[8].Width = 100;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Select";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[9].Width = 50;
                            FpSpread1.Columns[9].Visible = true;
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 1, 2);
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 1, 2);
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
                            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                            cb.AutoPostBack = true;
                            FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                            cb1.AutoPostBack = false;
                            #region Header label
                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                labltotallable.Visible = true;
                                string checkvalue = "";
                                for (int rs = 0; rs < ds.Tables[3].Rows.Count; rs++)
                                {
                                    if (checkvalue == "")
                                    {
                                        checkvalue = Convert.ToString(ds.Tables[3].Rows[rs]["change_strength"]);
                                    }
                                    else
                                    {
                                        checkvalue = checkvalue + "   " + Convert.ToString(ds.Tables[3].Rows[rs]["change_strength"]);
                                    }
                                }
                                labltotallable.Text = Convert.ToString(checkvalue);
                            }
                            #endregion
                            //FarPoint.Web.Spread.DoubleCellType db1 = new FarPoint.Web.Spread.DoubleCellType(); change_strength
                            //db1.ErrorMessage = "Enter Only Number";
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].CellType = cb;
                            #endregion
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["ItemFK"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemName"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["itemunit"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["IssuedRPU"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["hand_qty"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].ForeColor = Color.Blue;
                                string handquntiy = Convert.ToString(ds.Tables[0].Rows[i]["hand_qty"]);
                                double hand = 0;
                                if (handquntiy.Trim() != "")
                                {
                                    hand = Convert.ToDouble(handquntiy);
                                }
                                int strength = 0;
                                double VegNeed = 0;
                                double NonVegNeed = 0;
                                double vegStrengthtotal = 0;
                                double nonVegStrengthtotal = 0;
                                Hashtable itemnameVegNonveg = new Hashtable();
                                if (ds.Tables[2].Rows.Count > 0)
                                {
                                    ds.Tables[2].DefaultView.RowFilter = "itemcode='" + Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]) + "'";
                                    dv = ds.Tables[2].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        vegStrengthtotal = 0;
                                        nonVegStrengthtotal = 0;

                                        VegNeed = 0;//barath 19.1.18
                                        NonVegNeed = 0;
                                        for (int ro = 0; ro < dv.Count; ro++)
                                        {
                                            //VegNeed = 0;//barath 19.1.18
                                            //NonVegNeed = 0;
                                            if (ds.Tables[1].Rows.Count > 0)
                                            {
                                                for (int ik = 0; ik < cbl_session.Items.Count; ik++)
                                                {
                                                    if (cbl_session.Items[ik].Selected == true)
                                                    {

                                                        ds.Tables[1].DefaultView.RowFilter = "MenuScheduleDate='" + dt.ToString("MM/dd/yyyy") + "' and MessMasterFK ='" + Convert.ToString(dv[ro]["MessMasterPK"]) + "' and SessionMasterFK='" + Convert.ToString(cbl_session.Items[ik].Value) + "' and MenuMasterFK in('" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "') and Strengthdate='" + dt.ToString("MM/dd/yyyy") + "' and ScheduleType='1'";
                                                        dv1 = ds.Tables[1].DefaultView;
                                                        if (dv1.Count == 0)
                                                        {
                                                            ds.Tables[1].DefaultView.RowFilter = "MenuScheduleday='" + dt.ToString("dddd") + "' and MessMasterFK ='" + Convert.ToString(dv[ro]["MessMasterPK"]) + "' and SessionMasterFK='" + Convert.ToString(cbl_session.Items[ik].Value) + "' and MenuMasterFK in('" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "') and Strengthdate='" + dt.ToString("MM/dd/yyyy") + "' and ScheduleType='2'";
                                                            dv1 = ds.Tables[1].DefaultView;
                                                        }
                                                      //  ds.Tables[1].DefaultView.RowFilter = "MenuScheduleday='" + dt.ToString("dddd") + "' and MessMasterFK ='" + Convert.ToString(dv[ro]["MessMasterPK"]) + "' and SessionMasterFK='" + Convert.ToString(cbl_session.Items[ik].Value) + "' and MenuMasterFK in('" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "') and Strengthdate='" + dt.ToString("MM/dd/yyyy") + "'";
                                                       // dv1 = ds.Tables[1].DefaultView;
                                                        if (dv1.Count > 0)
                                                        {
                                                            if (Convert.ToString(dv1[0]["MenuType"]) == "1")
                                                                double.TryParse(Convert.ToString(dv1[0]["nonVegCount"]), out nonVegStrengthtotal);
                                                            else
                                                                double.TryParse(Convert.ToString(dv1[0]["vegCount"]), out vegStrengthtotal);
                                                            double fpspreadColumnValue = 0;
                                                            fpspreadColumnValue = Convert.ToString(dv1[0]["MenuType"]) == "1" ? nonVegStrengthtotal : vegStrengthtotal;
                                                            if (!itemnameVegNonveg.ContainsKey(Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]) + "-" + Convert.ToString(dv1[0]["MenuType"])))
                                                            {
                                                                itemnameVegNonveg.Add(Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]) + "-" + Convert.ToString(dv1[0]["MenuType"]), Convert.ToString(dv1[0]["MenuType"]) + "-" + fpspreadColumnValue);
                                                            }
                                                            string total = Convert.ToString(dv1[0]["change_strength"]);
                                                            strength = strength + Convert.ToInt32(total);
                                                            string Needquantiy = string.Empty;
                                                            string noofpersion = string.Empty;
                                                            if (ds.Tables[4].Rows.Count > 0)
                                                            {
                                                                ds.Tables[4].DefaultView.RowFilter = "ItemFK='" + Convert.ToString(ds.Tables[0].Rows[i]["ItemFK"]) + "' and MenuMasterFK='" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "'";
                                                                DataView dv3 = ds.Tables[4].DefaultView;
                                                                if (dv3.Count > 0)
                                                                {
                                                                    Needquantiy = Convert.ToString(dv3[0]["NeededQty"]);
                                                                    noofpersion = Convert.ToString(dv3[0]["NoOfPerson"]);
                                                                }
                                                            }
                                                            if (noofpersion.Trim() != "" && Needquantiy.Trim() != "" && noofpersion.Trim() != "0" && Needquantiy.Trim() != "0")
                                                            {
                                                                double VegAmt = (Convert.ToDouble(vegStrengthtotal) / Convert.ToDouble(noofpersion)) * Convert.ToDouble(Needquantiy);
                                                                if (VegAmt != 0)
                                                                    VegNeed += VegAmt;
                                                                double NonvegAmt = (Convert.ToDouble(nonVegStrengthtotal) / Convert.ToDouble(noofpersion)) * Convert.ToDouble(Needquantiy);
                                                                if (NonvegAmt != 0)
                                                                    NonVegNeed += NonvegAmt;
                                                                VegNeed = Math.Round(VegNeed, 2);
                                                                NonVegNeed = Math.Round(NonVegNeed, 2);
                                                                if (total.Trim() != "")
                                                                {
                                                                    double vegStrength = 0;
                                                                    double nonVegstrength = 0;
                                                                    if (itemnameVegNonveg.ContainsKey(Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]) + "-0"))
                                                                    {
                                                                        string[] value = Convert.ToString(itemnameVegNonveg[Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]) + "-0"]).Split('-');
                                                                        double.TryParse(Convert.ToString(value[1]), out vegStrength);
                                                                    }
                                                                    if (itemnameVegNonveg.ContainsKey(Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]) + "-1"))
                                                                    {
                                                                        string[] value = Convert.ToString(itemnameVegNonveg[Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]) + "-1"]).Split('-');
                                                                        double.TryParse(Convert.ToString(value[1]), out nonVegstrength);
                                                                    }
                                                                    if (!hashset.Contains(Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["menumasterpk"]) + "-" + Convert.ToString(cbl_session.Items[ik].Value)))
                                                                    {
                                                                        hashset.Add(Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["menumasterpk"]) + "-" + Convert.ToString(cbl_session.Items[ik].Value), dt.ToString("dddd") + "-" + Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["menumasterpk"]) + "-" + Convert.ToString(cbl_session.Items[ik].Value) + "-" + total + "-" + Needquantiy + "-" + noofpersion + "-" + Convert.ToString(ds.Tables[0].Rows[i]["ItemFK"]) + "-" + Convert.ToString(ds.Tables[0].Rows[i]["IssuedRPU"]) + "-" + Convert.ToString(ds.Tables[0].Rows[i]["hand_qty"]) + "-" + vegStrength + "-" + nonVegstrength + "-" + Convert.ToString(dv1[0]["MenuType"]));
                                                                    }
                                                                    else
                                                                    {
                                                                        string getvalue = Convert.ToString(hashset[Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["menumasterpk"]) + "-" + Convert.ToString(cbl_session.Items[ik].Value)]);
                                                                        if (getvalue.Trim() != "")
                                                                        {
                                                                            hashset.Remove(Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["menumasterpk"]) + "-" + Convert.ToString(cbl_session.Items[ik].Value));
                                                                            getvalue = getvalue + "/" + dt.ToString("dddd") + "-" + Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["menumasterpk"]) + "-" + Convert.ToString(cbl_session.Items[ik].Value) + "-" + total + "-" + Needquantiy + "-" + noofpersion + "-" + Convert.ToString(ds.Tables[0].Rows[i]["ItemFK"]) + "-" + Convert.ToString(ds.Tables[0].Rows[i]["IssuedRPU"] + "-" + Convert.ToString(ds.Tables[0].Rows[i]["hand_qty"]) + "-" + vegStrength + "-" + nonVegstrength + "-" + Convert.ToString(dv1[0]["MenuType"]));
                                                                            hashset.Add(Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["menumasterpk"]) + "-" + Convert.ToString(cbl_session.Items[ik].Value), getvalue);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            #region old 19.07.17
                                                            //string total = Convert.ToString(dv1[0]["change_strength"]);
                                                            //if (total.Trim() != "")
                                                            //{
                                                            //    strength = strength + Convert.ToInt32(total);
                                                            //    string Needquantiy = ""; string noofpersion = "";
                                                            //    if (ds.Tables[4].Rows.Count > 0)
                                                            //    {
                                                            //        ds.Tables[4].DefaultView.RowFilter = "ItemFK='" + Convert.ToString(ds.Tables[0].Rows[i]["ItemFK"]) + "' and MenuMasterFK='" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "'";
                                                            //        DataView dv3 = ds.Tables[4].DefaultView;
                                                            //        if (dv3.Count > 0)
                                                            //        {
                                                            //            Needquantiy = Convert.ToString(dv3[0]["NeededQty"]);
                                                            //            noofpersion = Convert.ToString(dv3[0]["NoOfPerson"]);
                                                            //        }
                                                            //    }
                                                            //    if (noofpersion.Trim() != "" && Needquantiy.Trim() != "" && noofpersion.Trim() != "0" && Needquantiy.Trim() != "0")
                                                            //    {
                                                            //        double valueamt = Convert.ToDouble(total) / Convert.ToDouble(noofpersion) * Convert.ToDouble(Needquantiy);
                                                            //        if (valueamt != 0)
                                                            //        {
                                                            //            Need = Need + valueamt;
                                                            //        }
                                                            //        if (!hashset.Contains(Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["menumasterpk"]) + "-" + Convert.ToString(cbl_session.Items[ik].Value)))
                                                            //        {
                                                            //            hashset.Add(Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["menumasterpk"]) + "-" + Convert.ToString(cbl_session.Items[ik].Value), dt.ToString("dddd") + "-" + Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["menumasterpk"]) + "-" + Convert.ToString(cbl_session.Items[ik].Value) + "-" + total + "-" + Needquantiy + "-" + noofpersion + "-" + Convert.ToString(ds.Tables[0].Rows[i]["ItemFK"]) + "-" + Convert.ToString(ds.Tables[0].Rows[i]["IssuedRPU"]) + "-" + Convert.ToString(ds.Tables[0].Rows[i]["hand_qty"]));
                                                            //        }
                                                            //        else
                                                            //        {
                                                            //            string getvalue = Convert.ToString(hashset[Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["menumasterpk"]) + "-" + Convert.ToString(cbl_session.Items[ik].Value)]);
                                                            //            if (getvalue.Trim() != "")
                                                            //            {
                                                            //                hashset.Remove(Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["menumasterpk"]) + "-" + Convert.ToString(cbl_session.Items[ik].Value));
                                                            //                getvalue = getvalue + "/" + dt.ToString("dddd") + "-" + Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["menumasterpk"]) + "-" + Convert.ToString(cbl_session.Items[ik].Value) + "-" + total + "-" + Needquantiy + "-" + noofpersion + "-" + Convert.ToString(ds.Tables[0].Rows[i]["ItemFK"]) + "-" + Convert.ToString(ds.Tables[0].Rows[i]["IssuedRPU"] + "-" + Convert.ToString(ds.Tables[0].Rows[i]["hand_qty"]));
                                                            //                hashset.Add(Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["menumasterpk"]) + "-" + Convert.ToString(cbl_session.Items[ik].Value), getvalue);
                                                            //            }
                                                            //        }
                                                            //    }
                                                            //}
                                                            #endregion
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                bool vegItemCount = false;
                                bool NonVegItemCount = false;
                                if (itemnameVegNonveg.ContainsKey(Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]) + "-0"))
                                {
                                    string[] value = Convert.ToString(itemnameVegNonveg[Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]) + "-0"]).Split('-');
                                    vegItemCount = (value[0] == "0") ? true : false;
                                    double.TryParse(Convert.ToString(value[1]), out  vegStrengthtotal);
                                }
                                if (itemnameVegNonveg.ContainsKey(Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]) + "-1"))
                                {
                                    string[] value = Convert.ToString(itemnameVegNonveg[Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]) + "-1"]).Split('-');
                                    NonVegItemCount = (value[0] == "1") ? true : false;
                                    double.TryParse(Convert.ToString(value[1]), out  nonVegStrengthtotal);
                                }
                                if (!vegItemCount)
                                    vegStrengthtotal = 0;
                                if (!NonVegItemCount)
                                    nonVegStrengthtotal = 0;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(vegStrengthtotal);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(nonVegStrengthtotal);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(Math.Round(VegNeed, 3));
                                if (hand >= VegNeed)
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].ForeColor = Color.Green;
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].ForeColor = Color.Red;
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(Math.Round(NonVegNeed, 3));
                                if (hand >= NonVegNeed)
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].ForeColor = Color.Green;
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].ForeColor = Color.Red;
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].CellType = cb1;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                            }
                            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                            FpSpread1.Sheets[0].FrozenRowCount = 0;
                            FpSpread1.Visible = true;
                            rptprint.Visible = true;
                            lbl_error1.Visible = false;
                            btn_save.Visible = true;
                            Session["AddArrayValue"] = hashset;
                            if (hashset.Count > 0)
                            {
                                int itemcount = 0;
                                foreach (DictionaryEntry pr1 in hashset)
                                {
                                    string value = Convert.ToString(pr1.Value);
                                    string[] splitnew = value.Split('/');
                                    if (splitnew.Length > 0)
                                    {
                                        for (int j = 0; j < splitnew.Length; j++)
                                        {
                                            string[] splitfirst1 = splitnew[j].Split('-');
                                            if (!itemcoderepect.ContainsKey(Convert.ToString(splitfirst1[7])))
                                            {
                                                itemcount = 1;
                                                itemcoderepect.Add(Convert.ToString(splitfirst1[7]), Convert.ToString(itemcount));
                                            }
                                            else
                                            {
                                                int NewVal = Convert.ToInt32(itemcoderepect[Convert.ToString(splitfirst1[7])]);
                                                itemcount = NewVal + 1;
                                                itemcoderepect.Remove(splitfirst1[7]);
                                                itemcoderepect.Add(Convert.ToString(splitfirst1[7]), Convert.ToString(itemcount));
                                            }
                                        }
                                    }
                                }
                                Session["itemcoderepect"] = itemcoderepect;
                            }
                        }
                        else
                        {
                            lbl_error1.Visible = true;
                            lbl_error1.Text = "No Record Found";
                            FpSpread1.Visible = false;
                            rptprint.Visible = false;
                            btn_save.Visible = false;
                            labltotallable.Visible = false;
                        }
                        #endregion
                    }
                    if (rdb_cleanitem.Checked == true)
                    {
                        #region Cleaning Item
                        string selecnew_query = "select i.ItemCode,i.itemname,i.itempk,i.itemunit, s.IssuedQty-isnull(UsedQty,'0')as BalQty, s.IssuedRPU from Cleaning_ItemMaseter cm,Cleaning_ItemDetailMaster cd,IM_ItemMaster i ,IT_StockDeptDetail s where cm.Clean_ItemMasterPK=cd.Clean_ItemMasterFK and cd.Itemfk=i.ItemPK and s.ItemFK=i.ItemPK and s.ItemFK=cd.Itemfk and cm.MessMasterFK in('" + hostelcode + "') and cm.SessionFK in('" + itemheadercode + "') and cd.Itemfk in('" + menuvalue + "') and s.DeptFK in('" + hostelcode + "') and s.DeptFK = cm.MessMasterFK group by i.ItemCode,i.itemname,i.itempk,i.itemunit, s.IssuedQty,UsedQty, s.IssuedRPU";
                        selecnew_query = selecnew_query + "  select MenuScheduleday,MenuMasterFK,change_strength,MessMasterFK, SessionMasterFK  from HT_MenuSchedule where ScheudleItemType='2' and ScheduleType='2'";//Needed_Qty,
                        selecnew_query = selecnew_query + "  select i.ItemCode,i.itempk,i.itemname,i.itemunit,s.IssuedQty-isnull(UsedQty,'0')as BalQty,cm.MessMasterFK, Needed_Qty,SessionFK,Schedule_Day from Cleaning_ItemMaseter cm,Cleaning_ItemDetailMaster cd,IM_ItemMaster i ,IT_StockDeptDetail s where cm.Clean_ItemMasterPK  =cd.Clean_ItemMasterFK  and cd.Itemfk =i.ItemPK  and s.ItemFK =i.ItemPK and s.DeptFK = cm.MessMasterFK  and cm.MessMasterFK in ('" + hostelcode + "')  and cm.SessionFK in ('" + itemheadercode + "') and i.ItemPK in ('" + menuvalue + "')";
                        //IT_StockDetail
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selecnew_query, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            FpSpread1.Sheets[0].RowCount = 0;
                            FpSpread1.Sheets[0].ColumnCount = 0;
                            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                            FpSpread1.CommandBar.Visible = false;
                            FpSpread1.Sheets[0].ColumnCount = 8;
                            FpSpread1.Sheets[0].AutoPostBack = false;
                            FpSpread1.Sheets[0].RowHeader.Visible = false;
                            FpSpread1.Width = 717;
                            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            darkstyle.ForeColor = Color.White;
                            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Columns[0].Width = 50;
                            FpSpread1.Columns[0].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Item Code";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Columns[1].Width = 100;
                            FpSpread1.Columns[1].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Name";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[2].Width = 200;
                            FpSpread1.Columns[2].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Item Measure";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[3].Width = 100;
                            FpSpread1.Columns[3].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Item in Hand";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[4].Width = 100;
                            FpSpread1.Columns[4].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Total Strength";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[5].Width = 100;
                            FpSpread1.Columns[5].Visible = false;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Required Quantity";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[6].Width = 100;
                            FpSpread1.Columns[6].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Select";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[7].Width = 50;
                            FpSpread1.Columns[7].Visible = true;
                            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                            cb.AutoPostBack = true;
                            FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                            cb1.AutoPostBack = false;
                            //FarPoint.Web.Spread.DoubleCellType db1 = new FarPoint.Web.Spread.DoubleCellType();
                            //db1.ErrorMessage = "Enter Only Number";
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].CellType = cb;
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["itempk"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemName"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["itemunit"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["IssuedRPU"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["BalQty"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].ForeColor = Color.Blue;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["Needed_Qty"]);
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].CellType = db1;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                //string needed = Convert.ToString(ds.Tables[0].Rows[i]["Needed_Qty"]);
                                string balqty = Convert.ToString(ds.Tables[0].Rows[i]["BalQty"]);
                                //if (needed.Trim() == "")
                                //{
                                //    needed = "0";
                                //}
                                if (balqty.Trim() == "")
                                {
                                    balqty = "0";
                                }
                                double Need = 0;
                                if (ds.Tables[2].Rows.Count > 0)
                                {
                                    ds.Tables[2].DefaultView.RowFilter = "itemcode='" + Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]) + "'";
                                    dv = ds.Tables[2].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        //for (int ro = 0; ro < dv.Count; ro++)
                                        //{
                                        if (ds.Tables[1].Rows.Count > 0)
                                        {
                                            for (int ik = 0; ik < cbl_session.Items.Count; ik++)
                                            {
                                                if (cbl_session.Items[ik].Selected == true)
                                                {
                                                    ds.Tables[1].DefaultView.RowFilter = "MenuScheduleday='" + dt.ToString("dddd") + "' and MessMasterFK ='" + Convert.ToString(ddl_hostelname.SelectedItem.Value) + "' and SessionMasterFK='" + Convert.ToString(cbl_session.Items[ik].Value) + "' ";
                                                    dv1 = ds.Tables[1].DefaultView;
                                                    if (dv1.Count > 0)
                                                    {
                                                        string Needquantiy = "";
                                                        if (ds.Tables[2].Rows.Count > 0)
                                                        {
                                                            ds.Tables[2].DefaultView.RowFilter = "ItemPK='" + Convert.ToString(ds.Tables[0].Rows[i]["ItemPK"]) + "' and MessMasterFK ='" + Convert.ToString(ddl_hostelname.SelectedItem.Value) + "' and SessionFK='" + Convert.ToString(dv1[0]["SessionMasterFK"]) + "' and Schedule_Day='" + dt.ToString("dddd") + "' ";
                                                            DataView dv3 = ds.Tables[2].DefaultView;
                                                            if (dv3.Count > 0)
                                                            {
                                                                Needquantiy = Convert.ToString(dv3[0]["Needed_Qty"]);
                                                                if (Needquantiy.Trim() == "")
                                                                    Needquantiy = "0";
                                                                Need = Need + Convert.ToDouble(Needquantiy);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        //}
                                    }
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(Need);
                                if (Convert.ToDouble(balqty) >= Convert.ToDouble(Need))
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.Green;
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.Red;
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].CellType = cb1;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].ForeColor = Color.Green;
                            }
                            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                            FpSpread1.Visible = true;
                            rptprint.Visible = true;
                            lbl_error1.Visible = false;
                            btn_save.Visible = true;
                        }
                        else
                        {
                            lbl_error1.Visible = true;
                            lbl_error1.Text = "No Record Found";
                            FpSpread1.Visible = false;
                            rptprint.Visible = false;
                            btn_save.Visible = false;
                            labltotallable.Visible = false;
                        }
                        #endregion
                    }
                }
                else
                {
                    if (rdb_menuitemcon.Checked == true)
                    {
                        #region Menuitem
                        string selectquery = "  select i.ItemCode,d.ItemFK,itemname,IssuedQty-ISNULL(UsedQty,'0')  as hand_qty,i.itemunit,s.IssuedRPU as Rpu,i.ItemPK,u.MenuType from HM_MenuItemDetail d, HM_MenuItemMaster m,HM_MenuMaster u,IM_ItemMaster i,HM_MessMaster h,IT_StockDeptDetail s WHERE D.MenuItemMasterFK =M.MenuItemMasterPK and i.ItemPK=d.ItemFK and m.MenuMasterFK=u.MenuMasterPK and s.ItemFK=i.ItemPK and s.ItemFK=d.ItemFK and s.DeptFK = h.MessMasterPK and s.DeptFK in('" + hostelcode + "') and u.MenuMasterPK in ('" + menuvalue + "') and h.MessMasterPK=m.MessMasterFK group by i.ItemCode,d.ItemFK,itemname,IssuedQty,UsedQty,i.itemunit,IssuedRPU,itempk,u.MenuType";
                        selectquery += "   select  MenuScheduleday,MenuMasterFK,change_strength, MessMasterFK ,SessionMasterFK,MenuSchedulePK,m.MenuType from HT_MenuSchedule ht,hm_menumaster m where ht.menumasterfk=m.menumasterpk and ScheudleItemType='1' and ScheduleType='2'";
                        //select MenuScheduleday,MenuMasterFK,change_strength, MessMasterFK ,SessionMasterFK,MenuSchedulePK from HT_MenuSchedule where ScheudleItemType='1' and ScheduleType='2'";
                        selectquery += "   select  i.ItemCode,i.itempk,itemname,i.itemunit,BalQty as hand_qty, m.NoOfPerson,mi.NeededQty,mu.MenuMasterPK,MessMasterPK,mu.MenuType  from HM_MenuItemMaster m ,HM_MenuItemDetail mi,IM_ItemMaster i,IT_StockDeptDetail s,HM_MenuMaster mu,HM_MessMaster h where m.MenuItemMasterPK  =Mi.MenuItemMasterFK and mi.ItemFK =i.ItemPK and mi.ItemFK=s.ItemFK and m.MenuMasterFK=mu.MenuMasterPK and m.MenuMasterFK=mu.MenuMasterPK and h.MessMasterPK in ('" + hostelcode + "') and mu.MenuMasterPK in ('" + menuvalue + "') and s.DeptFK = h.MessMasterPK and h.MessMasterPK=m.MessMasterFK";
                        selectquery += "   select NeededQty,NoOfPerson ,ItemFK,MenuMasterFK from HM_MenuItemDetail m,HM_MenuItemMaster h where h.MenuItemMasterPK =m.MenuItemMasterFK  and MenuMasterFK in ('" + menuvalue + "') and h.MessMasterFK in('" + hostelcode + "')";
                        selectquery += " select MenuscheduleFK,PurposeCode,VegCount,NonVegCount from HT_HostelMenupurposeStrength ";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            FpSpread1.Sheets[0].RowCount = 0;
                            FpSpread1.Sheets[0].ColumnCount = 0;
                            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                            FpSpread1.CommandBar.Visible = false;
                            FpSpread1.Sheets[0].ColumnCount = 8;
                            FpSpread1.Sheets[0].AutoPostBack = false;
                            FpSpread1.Sheets[0].RowHeader.Visible = false;
                            FpSpread1.Width = 866;
                            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            darkstyle.ForeColor = Color.White;
                            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Columns[0].Width = 50;
                            FpSpread1.Columns[0].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Item Code";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Columns[1].Width = 100;
                            FpSpread1.Columns[1].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Name";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[2].Width = 200;
                            FpSpread1.Columns[2].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Item Measure";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[3].Width = 100;
                            FpSpread1.Columns[3].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Item in Hand";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[4].Width = 100;
                            FpSpread1.Columns[4].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Total Strength";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[5].Width = 125;
                            FpSpread1.Columns[5].Locked = false;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Required Quantity";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[6].Width = 125;
                            //  FpSpread1.Columns[6].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Select";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[7].Width = 50;
                            FpSpread1.Columns[7].Visible = true;
                            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                            cb.AutoPostBack = true;
                            FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                            cb1.AutoPostBack = false;
                            FarPoint.Web.Spread.DoubleCellType db1 = new FarPoint.Web.Spread.DoubleCellType();
                            db1.ErrorMessage = "Enter Only Number";
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].CellType = cb;
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["itempk"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemName"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["itemunit"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["rpu"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["hand_qty"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].ForeColor = Color.Blue;
                                string handquntiy = Convert.ToString(ds.Tables[0].Rows[i]["hand_qty"]);
                                double hand = 0;
                                if (handquntiy.Trim() != "")
                                {
                                    hand = Convert.ToDouble(handquntiy);
                                }
                                int strength = 0;
                                double Need = 0;
                                if (ds.Tables[2].Rows.Count > 0)
                                {
                                    ds.Tables[2].DefaultView.RowFilter = "itemcode='" + Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]) + "'";
                                    dv = ds.Tables[2].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        for (int ro = 0; ro < dv.Count; ro++)
                                        {
                                            strength = 0;
                                            //string Needquantiy = Convert.ToString(dv[ro]["NeededQty"]);
                                            //string noofpersion = Convert.ToString(dv[ro]["NoOfPerson"]);
                                            if (ds.Tables[1].Rows.Count > 0)
                                            {
                                                for (int ik = 0; ik < cbl_session.Items.Count; ik++)
                                                {
                                                    if (cbl_session.Items[ik].Selected == true)
                                                    {
                                                        ds.Tables[1].DefaultView.RowFilter = "MenuScheduleday='" + dt.ToString("dddd") + "' and MessMasterFK ='" + Convert.ToString(dv[ro]["MessMasterPK"]) + "' and SessionMasterFK='" + Convert.ToString(cbl_session.Items[ik].Value) + "' and MenuMasterFK in('" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "')";
                                                        dv1 = ds.Tables[1].DefaultView;
                                                        if (dv1.Count > 0)
                                                        {
                                                            string total = string.Empty;
                                                            string MenuType = Convert.ToString(dv1[0]["MenuType"]);
                                                            if (ddl_purposeCategory.Items.Count > 0)
                                                            {
                                                                if (ddl_purposeCategory.SelectedIndex != 0)
                                                                {
                                                                    ds.Tables[4].DefaultView.RowFilter = " MenuscheduleFK='" + Convert.ToString(dv1[0]["MenuSchedulePK"]) + "' and PurposeCode='" + Convert.ToString(ddl_purposeCategory.SelectedItem.Value) + "'";
                                                                    DataView PurposeCategoryDV = ds.Tables[4].DefaultView;
                                                                    if (PurposeCategoryDV.Count > 0)
                                                                    {
                                                                        double Veg = 0; double Nonveg = 0;
                                                                        double.TryParse(Convert.ToString(PurposeCategoryDV[0]["VegCount"]), out Veg);
                                                                        double.TryParse(Convert.ToString(PurposeCategoryDV[0]["NonVegCount"]), out Nonveg);
                                                                        double PurposeTotal = Veg + Nonveg;
                                                                        total = Convert.ToString(PurposeTotal);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    total = Convert.ToString(dv1[0]["change_strength"]);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                total = Convert.ToString(dv1[0]["change_strength"]);
                                                            }
                                                            if (total.Trim() != "")
                                                            {
                                                                strength = strength + Convert.ToInt32(total);
                                                                string Needquantiy = ""; string noofpersion = "";
                                                                if (ds.Tables[3].Rows.Count > 0)
                                                                {
                                                                    ds.Tables[3].DefaultView.RowFilter = "ItemFK='" + Convert.ToString(ds.Tables[0].Rows[i]["ItemFK"]) + "' and MenuMasterFK='" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "'";
                                                                    DataView dv3 = ds.Tables[3].DefaultView;
                                                                    if (dv3.Count > 0)
                                                                    {
                                                                        Needquantiy = Convert.ToString(dv3[0]["NeededQty"]);
                                                                        noofpersion = Convert.ToString(dv3[0]["NoOfPerson"]);
                                                                    }
                                                                }
                                                                if (noofpersion.Trim() != "" && Needquantiy.Trim() != "" && noofpersion.Trim() != "0" && Needquantiy.Trim() != "0")
                                                                {
                                                                    double valueamt = Convert.ToDouble(total) / Convert.ToDouble(noofpersion) * Convert.ToDouble(Needquantiy);
                                                                    if (valueamt != 0)
                                                                    {
                                                                        Need = Need + valueamt;
                                                                    }
                                                                    if (!hashset.Contains(Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "-" + Convert.ToString(cbl_session.Items[ik].Value)))
                                                                    {
                                                                        hashset.Add(Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "-" + Convert.ToString(cbl_session.Items[ik].Value), dt.ToString("dddd") + "-" + Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "-" + Convert.ToString(cbl_session.Items[ik].Value) + "-" + total + "-" + Needquantiy + "-" + noofpersion + "-" + Convert.ToString(ds.Tables[0].Rows[i]["ItemFK"]) + "-" + Convert.ToString(ds.Tables[0].Rows[i]["rpu"]) + "-" + MenuType);
                                                                    }
                                                                    else
                                                                    {
                                                                        string getvalue = Convert.ToString(hashset[Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "-" + Convert.ToString(cbl_session.Items[ik].Value)]);
                                                                        if (getvalue.Trim() != "")
                                                                        {
                                                                            hashset.Remove(Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "-" + Convert.ToString(cbl_session.Items[ik].Value));
                                                                            getvalue = getvalue + "/" + dt.ToString("dddd") + "-" + Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "-" + Convert.ToString(cbl_session.Items[ik].Value) + "-" + total + "-" + Needquantiy + "-" + noofpersion + "-" + Convert.ToString(ds.Tables[0].Rows[i]["ItemFK"]) + "-" + Convert.ToString(ds.Tables[0].Rows[i]["rpu"]) + "-" + MenuType;
                                                                            hashset.Add(Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "-" + Convert.ToString(cbl_session.Items[ik].Value), getvalue);
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
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(strength);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].CellType = db1;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].BackColor = Color.LightYellow;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(Math.Round(Need, 2));
                                //if (hand >= Need)
                                //{
                                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.Green;
                                //}
                                //else
                                //{
                                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.Red;
                                //}
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].CellType = cb1;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                            }
                            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                            FpSpread1.Sheets[0].FrozenRowCount = 0;
                            FpSpread1.Visible = true;
                            rptprint.Visible = true;
                            lbl_error1.Visible = false;
                            btn_save.Visible = true;
                            Session["AddArrayValue"] = hashset;
                            if (hashset.Count > 0)
                            {
                                int itemcount = 0;
                                foreach (DictionaryEntry pr1 in hashset)
                                {
                                    string value = Convert.ToString(pr1.Value);
                                    string[] splitnew = value.Split('/');
                                    if (splitnew.Length > 0)
                                    {
                                        for (int j = 0; j < splitnew.Length; j++)
                                        {
                                            string[] splitfirst1 = splitnew[j].Split('-');
                                            if (!itemcoderepect.ContainsKey(Convert.ToString(splitfirst1[7])))
                                            {
                                                itemcount = 1;
                                                itemcoderepect.Add(Convert.ToString(splitfirst1[7]), Convert.ToString(itemcount));
                                            }
                                            else
                                            {
                                                int NewVal = Convert.ToInt32(itemcoderepect[Convert.ToString(splitfirst1[7])]);
                                                itemcount = NewVal + 1;
                                                itemcoderepect.Remove(splitfirst1[7]);
                                                itemcoderepect.Add(Convert.ToString(splitfirst1[7]), Convert.ToString(itemcount));
                                            }
                                        }
                                    }
                                }
                                Session["itemcoderepect"] = itemcoderepect;
                            }
                        }
                        else
                        {
                            lbl_error1.Visible = true;
                            lbl_error1.Text = "No Record Found";
                            FpSpread1.Visible = false;
                            labltotallable.Visible = false;
                            rptprint.Visible = false;
                            btn_save.Visible = false;
                        }
                        #endregion
                    }
                    if (rdb_cleanitem.Checked == true)
                    {
                        #region Cleaning Item
                        string selecnew_query = "select i.ItemCode,i.itemname,i.itempk,i.itemunit, s.IssuedQty-isnull(UsedQty,'0')as BalQty, s.IssuedRPU from Cleaning_ItemMaseter cm,Cleaning_ItemDetailMaster cd,IM_ItemMaster i ,IT_StockDeptDetail s where cm.Clean_ItemMasterPK=cd.Clean_ItemMasterFK and cd.Itemfk=i.ItemPK and s.ItemFK=i.ItemPK and s.ItemFK=cd.Itemfk and cm.MessMasterFK in('" + hostelcode + "') and cm.SessionFK in('" + itemheadercode + "') and cd.Itemfk in('" + menuvalue + "') and s.DeptFK in('" + hostelcode + "')  and s.DeptFK = cm.MessMasterFK group by i.ItemCode,i.itemname,i.itempk,i.itemunit, s.IssuedQty,UsedQty, s.IssuedRPU";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selecnew_query, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            FpSpread1.Sheets[0].RowCount = 0;
                            FpSpread1.Sheets[0].ColumnCount = 0;
                            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                            FpSpread1.CommandBar.Visible = false;
                            FpSpread1.Sheets[0].ColumnCount = 8;
                            FpSpread1.Sheets[0].AutoPostBack = false;
                            FpSpread1.Sheets[0].RowHeader.Visible = false;
                            FpSpread1.Width = 717;
                            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            darkstyle.ForeColor = Color.White;
                            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Columns[0].Width = 50;
                            FpSpread1.Columns[0].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Item Code";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Columns[1].Width = 100;
                            FpSpread1.Columns[1].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Name";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[2].Width = 200;
                            FpSpread1.Columns[2].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Item Measure";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[3].Width = 100;
                            FpSpread1.Columns[3].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Item in Hand";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[4].Width = 100;
                            FpSpread1.Columns[4].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Total Strength";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[5].Width = 100;
                            FpSpread1.Columns[5].Visible = false;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Required Quantity";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[6].Width = 100;
                            // FpSpread1.Columns[6].Locked = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Select";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                            FpSpread1.Columns[7].Width = 50;
                            FpSpread1.Columns[7].Visible = true;
                            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                            cb.AutoPostBack = true;
                            FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                            cb1.AutoPostBack = false;
                            FarPoint.Web.Spread.DoubleCellType db1 = new FarPoint.Web.Spread.DoubleCellType();
                            db1.ErrorMessage = "Enter Only Number";
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].CellType = cb;
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["itempk"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemName"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["itemunit"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["IssuedRPU"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["BalQty"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].ForeColor = Color.Blue;
                                string handqnty = Convert.ToString(ds.Tables[0].Rows[i]["BalQty"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].CellType = db1;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].BackColor = Color.LightYellow;
                                if (handqnty.Trim() != "" && handqnty.Trim() != "0")
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Locked = false;
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Locked = true;
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].CellType = cb1;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].ForeColor = Color.Green;
                            }
                            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                            FpSpread1.Visible = true;
                            rptprint.Visible = true;
                            lbl_error1.Visible = false;
                            btn_save.Visible = true;
                        }
                        else
                        {
                            lbl_error1.Visible = true;
                            lbl_error1.Text = "No Record Found";
                            FpSpread1.Visible = false;
                            labltotallable.Visible = false;
                            rptprint.Visible = false;
                            btn_save.Visible = false;
                        }
                        #endregion
                    }
                }
            }
            else
            {
                lbl_error1.Visible = true;
                lbl_error1.Text = "Please Select All Fields";
                FpSpread1.Visible = false;
                labltotallable.Visible = false;
                rptprint.Visible = false;
                btn_save.Visible = false;
            }
        }
        catch
        {
        }
    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        consumptioncheck();
        consum.Visible = false;
        btn_go_Click(sender, e);
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        consum.Visible = false;
        alertmessage.Visible = false;
        //popwindow.Visible = true;
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            firstdate = Convert.ToString(txt_date.Text);
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            bool newcheck = false;
            bool itemcheck = false;
            bool Negtive = false;
            for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                int column = 7;
                if (!cb_Additionalitem.Checked)
                    column = 9;
                int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, column].Value);
                if (checkval == 1)
                {
                    newcheck = true;
                    string itemcode1 = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag);
                    string PurposeQry = string.Empty;
                    if (ddl_purposeCategory.SelectedIndex != 0)
                        PurposeQry = " and PurposeCatagory='" + Convert.ToString(ddl_purposeCategory.SelectedItem.Value) + "'";
                    string q = "select * from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd where dd.DailyConsumptionMasterFK=dm.DailyConsumptionMasterPK and  dd.ItemFK ='" + itemcode1 + "' and MessMasterFK='" + Convert.ToString(ddl_hostelname.SelectedItem.Value) + "' and DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' " + PurposeQry + "";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        chk = true;
                    }
                    if (FpSpread1.Sheets[0].Cells[i, 6].ForeColor == Color.Red)
                    {
                        Negtive = true;
                    }
                }
            }
            if (chk == true && newcheck == true && itemcheck == false && Negtive == false)
            {
                if ((Convert.ToString(ViewState["Reconsumption"]) == "1"))
                {
                    consum.Visible = true;
                    lbl_sure.Text = "All ready Consumed. Do you want to reconsumed ?";
                }
                else
                {
                    lbl_alerterror.Visible = true;
                    lbl_alerterror.Text = "All ready Consumed.";
                    alertmessage.Visible = true;
                }
            }
            else if (chk == false && newcheck == true && itemcheck == false && Negtive == false)
            {
                consumptioncheck();
                btn_go_Click(sender, e);
            }
            else if (chk == false && newcheck == true && itemcheck == false && Negtive == true)
            {
                lblConsuption.Visible = true;
                lblConsuption.Text = "Few Items Hand Quantity is Very Low. Do you want to adjust Consume ?";
                Div1.Visible = true;
            }
            else if (chk == true && newcheck == true && itemcheck == false && Negtive == true)
            {
                lblConsuption.Visible = true;
                lblConsuption.Text = "All ready Consumed. Few Items Hand Quantity is Very Low. Do you want to adjust and reconsumed ?";
                Div1.Visible = true;
            }
            else if (newcheck == false)
            {
                lbl_alerterror.Visible = true;
                lbl_alerterror.Text = "Please Select Any One Item";
                alertmessage.Visible = true;
            }
            else if (itemcheck == true)
            {
                lbl_alerterror.Visible = true;
                lbl_alerterror.Text = "Please Select All Menu Items";
                alertmessage.Visible = true;
            }
        }
        catch
        {
        }
    }
    protected void consumptioncheck()
    {
        try
        {
            bool checkdailyconsum = false;
            FpSpread1.SaveChanges();
            Hashtable hnew = new Hashtable();
            string firstdate = Convert.ToString(txt_date.Text);
            DateTime dt = new DateTime();
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            string dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            ArrayList additemcode = new ArrayList();
            //25.06.16
            string setrpu = d2.GetFunction("select value from Master_Settings where settings='Consumption Rpu' and usercode='" + usercode + "'");
            bool emptyrpu = false;
            if (cb_Additionalitem.Checked == false)
            {
                #region without addtional items
                if (rdb_menuitemcon.Checked == true)
                {
                    #region Menu Item
                    if (FpSpread1.Sheets[0].RowCount > 0)
                    {
                        bool testflage = false;
                        bool finaltest = false;
                        bool fin = false;
                        bool Negtive = false;
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 9].Value);
                            if (checkval == 1)
                            {
                                if (!additemcode.Contains(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag)))
                                {
                                    additemcode.Add(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag));
                                }
                                fin = true;
                                if (FpSpread1.Sheets[0].Cells[i, 7].ForeColor == Color.Red || FpSpread1.Sheets[0].Cells[i, 8].ForeColor == Color.Red)
                                {
                                    Negtive = true;
                                }
                            }
                        }
                        if (fin == true && Negtive == false)
                        {
                            Hashtable sessArray = (Hashtable)Session["AddArrayValue"];
                            if (sessArray.Count > 0)
                            {
                                foreach (DictionaryEntry pr in sessArray)
                                {
                                    string key = Convert.ToString(pr.Key);
                                    string keyvalue = Convert.ToString(pr.Value);
                                    string[] splitfirst = key.Split('-');
                                    if (splitfirst.Length > 0)
                                    {
                                        string hostel = Convert.ToString(splitfirst[0]);
                                        string menucode = Convert.ToString(splitfirst[1]);
                                        string session = Convert.ToString(splitfirst[2]);
                                        string[] splitnew = keyvalue.Split('/');
                                        splitnew = splitnew[0].Split('-');
                                        //string total1 = Convert.ToString(splitnew[4]);
                                        double Vegcount = 0;
                                        double Nonvegcount = 0;
                                        double.TryParse(Convert.ToString(splitnew[10]), out Vegcount);
                                        double.TryParse(Convert.ToString(splitnew[11]), out Nonvegcount);
                                        string Menutype = Convert.ToString(splitnew[12]);
                                        double total1 = Vegcount + Nonvegcount;
                                        string inserquery = "if exists( select DailyConsumptionMasterPK from  HT_DailyConsumptionMaster where SessionFK ='" + session + "' and MessMasterFK in('" + hostel + "') and DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' and ForMess='1'  and MenumasterFK='" + menucode + "') update HT_DailyConsumptionMaster set Total_Present='" + total1 + "',VegStrength='" + Vegcount + "',NonvegStrength='" + Nonvegcount + "' where SessionFK ='" + session + "' and MessMasterFK ='" + hostel + "' and DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' and ForMess='1'  and MenumasterFK='" + menucode + "' else insert into HT_DailyConsumptionMaster  (DailyConsDate,ForMess,MessMasterFK,SessionFK,DeptFK,UserCode, Total_Present, MenumasterFK,VegStrength,NonvegStrength)values('" + dt.ToString("MM/dd/yyyy") + "','1','" + hostel + "','" + session + "','" + hostel + "','" + usercode + "','" + total1 + "','" + menucode + "','" + Vegcount + "','" + Nonvegcount + "')";
                                        int firstinsert = d2.update_method_wo_parameter(inserquery, "text");
                                        if (firstinsert != 0)
                                        {
                                            splitfirst = keyvalue.Split('/');
                                            if (splitfirst.Length > 0)
                                            {
                                                for (int rs = 0; rs <= splitfirst.GetUpperBound(0); rs++)
                                                {
                                                    string[] secondsplit = splitfirst[rs].Split('-');
                                                    if (secondsplit.Length > 0)
                                                    {
                                                        string day = Convert.ToString(secondsplit[0]);
                                                        string hostel1 = Convert.ToString(secondsplit[1]);
                                                        string menucode1 = Convert.ToString(secondsplit[2]);
                                                        string session1 = Convert.ToString(secondsplit[3]);
                                                        string total = Convert.ToString(secondsplit[4]);
                                                        string needquantity = Convert.ToString(secondsplit[5]);
                                                        string noofpersion = Convert.ToString(secondsplit[6]);
                                                        string itemcode = Convert.ToString(secondsplit[7]);
                                                        Vegcount = 0;
                                                        Nonvegcount = 0;
                                                        double.TryParse(Convert.ToString(secondsplit[10]), out Vegcount);
                                                        double.TryParse(Convert.ToString(secondsplit[11]), out Nonvegcount);
                                                        Menutype = Convert.ToString(secondsplit[12]);
                                                        //string rpu = Convert.ToString(secondsplit[8]);24.06.16
                                                        double Handonqty = 0;
                                                        double.TryParse(Convert.ToString(secondsplit[9]), out Handonqty);
                                                        //string rpu = string.Empty;
                                                        //if (setrpu.Trim() == "0")
                                                        //    rpu = d2.GetFunction("select AVG(IssuedRPU) Avg_rpu from IT_StockDeptDetail where ItemFK='" + itemcode + "' and DeptFK in('" + hostel + "')");
                                                        //if (setrpu.Trim() == "1")
                                                        //rpu = d2.GetFunction("select AVG(Sailing_prize) Avg_rpu from IT_StockDeptDetail where ItemFK='" + itemcode + "' and DeptFK in('" + hostel + "')");
                                                        if (additemcode.Contains(Convert.ToString(itemcode)) == true)
                                                        {
                                                            double requiredquantity = 0;
                                                            string DailyConsumptionMasterPK = string.Empty;
                                                            DataSet dailydetailsDs = new DataSet();
                                                            double handquantity = 0;
                                                            string q1 = "select DailyConsumptionMasterPK from HT_DailyConsumptionMaster where DailyConsDate='" + dt.ToString("MM/dd/yyyy") + "' and SessionFK='" + session + "' and MessMasterFK ='" + hostel + "' and ForMess='1'  and MenumasterFK='" + menucode + "'";
                                                            q1 += " select IssuedQty-isnull(UsedQty,'0')as BalQty from IT_StockDeptDetail where ItemFK='" + itemcode + "' and DeptFK='" + hostel1 + "'";
                                                            dailydetailsDs = d2.select_method_wo_parameter(q1, "text");
                                                            if (dailydetailsDs.Tables != null)
                                                            {
                                                                if (dailydetailsDs.Tables[0].Rows.Count > 0)
                                                                    DailyConsumptionMasterPK = Convert.ToString(dailydetailsDs.Tables[0].Rows[0]["DailyConsumptionMasterPK"]);
                                                                if (dailydetailsDs.Tables[1].Rows.Count > 0)
                                                                    double.TryParse(Convert.ToString(dailydetailsDs.Tables[1].Rows[0]["BalQty"]), out handquantity);
                                                            }
                                                            double Need = 0;
                                                            int inst = 0;
                                                            #region VegConsumption
                                                            if (Vegcount != 0)
                                                            {
                                                                double valueamt = Convert.ToDouble(Vegcount) / Convert.ToDouble(noofpersion) * Convert.ToDouble(needquantity);
                                                                if (valueamt != 0)
                                                                {
                                                                    Need = Need + valueamt;
                                                                }
                                                                requiredquantity = Need;
                                                                requiredquantity = Math.Round(requiredquantity, 2);//20.01.18
                                                                if (requiredquantity != 0)//rpu.Trim() != "" && 
                                                                {
                                                                    inst = 0;
                                                                    #region Rpu Calculation Barath 13.02.18
                                                                    DataSet RpuDS = new DataSet();
                                                                    RpuDS = d2.select_method_wo_parameter("select TransferQty,transferrpu,isnull(usedQuantity,0)as usedQuantity,TransferItemPK from IT_TransferItem where ItemFK='" + itemcode + "' and TrasferTo in('" + hostel + "') and TransferType in('0','1') and TransferQty<>ISNULL(usedQuantity,0) order by TrasnferDate", "text");
                                                                    if (RpuDS.Tables != null)
                                                                    {
                                                                        if (RpuDS.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            double TransferQty, TransferRpu, UsedQuantity, RemainingQty = 0;
                                                                            string newinsert = string.Empty;
                                                                            foreach (DataRow dr in RpuDS.Tables[0].Rows)
                                                                            {
                                                                                double.TryParse(Convert.ToString(dr["TransferQty"]), out TransferQty);
                                                                                double.TryParse(Convert.ToString(dr["transferrpu"]), out TransferRpu);
                                                                                double.TryParse(Convert.ToString(dr["usedQuantity"]), out UsedQuantity);
                                                                                string TransferItemPK = Convert.ToString(Convert.ToString(dr["TransferItemPK"]));
                                                                                if (TransferQty >= UsedQuantity + requiredquantity)//100>=120
                                                                                {
                                                                                    if (RemainingQty != 0)
                                                                                        requiredquantity = RemainingQty;
                                                                                    newinsert = " if exists(select*from HT_DailyConsumptionDetail  where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "'  and RPU='" + TransferRpu + "')update HT_DailyConsumptionDetail set ConsumptionQty=isnull(ConsumptionQty,0)+ ISNULL('" + requiredquantity + "',0),RPU='" + TransferRpu + "',Menutype='" + Menutype + "' where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "'  and RPU='" + TransferRpu + "' else insert into HT_DailyConsumptionDetail (ConsumptionQty,RPU,ItemFK,DailyConsumptionMasterFK,Menutype)values('" + requiredquantity + "','" + TransferRpu + "','" + itemcode + "','" + DailyConsumptionMasterPK + "','" + Menutype + "')";
                                                                                    newinsert += " update IT_TransferItem set UsedQuantity=isnull(UsedQuantity,0)+ISNULL('" + requiredquantity + "',0) where TransferItemPK='" + TransferItemPK + "'";
                                                                                    inst = d2.update_method_wo_parameter(newinsert, "Text");
                                                                                    RemainingQty = 0;
                                                                                }
                                                                                else if (TransferQty < UsedQuantity + requiredquantity)//100<120
                                                                                {
                                                                                    //RemainingQty = UsedQuantity + requiredquantity - TransferQty;//20
                                                                                    double TransBal = TransferQty - UsedQuantity;
                                                                                    RemainingQty = requiredquantity - TransBal;//20
                                                                                    requiredquantity = TransferQty - UsedQuantity;
                                                                                    newinsert = " if exists(select*from HT_DailyConsumptionDetail  where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "'  and RPU='" + TransferRpu + "')update HT_DailyConsumptionDetail set ConsumptionQty=isnull(ConsumptionQty,0)+ ISNULL('" + requiredquantity + "',0),RPU='" + TransferRpu + "',Menutype='" + Menutype + "' where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "'  and RPU='" + TransferRpu + "' else insert into HT_DailyConsumptionDetail (ConsumptionQty,RPU,ItemFK,DailyConsumptionMasterFK,Menutype)values('" + requiredquantity + "','" + TransferRpu + "','" + itemcode + "','" + DailyConsumptionMasterPK + "','" + Menutype + "')";
                                                                                    newinsert += " update IT_TransferItem set UsedQuantity=isnull(UsedQuantity,0)+ISNULL('" + requiredquantity + "',0) where TransferItemPK='" + TransferItemPK + "'";
                                                                                    inst = d2.update_method_wo_parameter(newinsert, "Text");
                                                                                }
                                                                                if (RemainingQty == 0)
                                                                                    break;
                                                                            }
                                                                        }
                                                                    }
                                                                    #endregion
                                                                    //requiredquantity = (Math.Ceiling(requiredquantity));
                                                                    //string newinsert = " if exists(select*from HT_DailyConsumptionDetail  where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "')update HT_DailyConsumptionDetail set ConsumptionQty=isnull(ConsumptionQty,0)+ ISNULL('" + requiredquantity + "',0),RPU='" + rpu + "',Menutype='" + Menutype + "' where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "' else insert into HT_DailyConsumptionDetail (ConsumptionQty,RPU,ItemFK,DailyConsumptionMasterFK,Menutype)values('" + requiredquantity + "','" + rpu + "','" + itemcode + "','" + DailyConsumptionMasterPK + "','" + Menutype + "')";
                                                                    //int inst = d2.update_method_wo_parameter(newinsert, "Text");//13.02.18
                                                                    if (inst != 0)
                                                                    {
                                                                        finaltest = true;
                                                                    }
                                                                    double balQty = 0;
                                                                    int in_s = 0;
                                                                    if (handquantity >= Need && handquantity != 0)//requiredquantity
                                                                    {
                                                                        balQty = handquantity - Need;//requiredquantity
                                                                        string stockvalue = "if exists(select*from IT_StockDeptDetail where ItemFK='" + itemcode + "' and DeptFK='" + ddl_hostelname.SelectedItem.Value + "')update IT_StockDeptDetail set ItemFK ='" + itemcode + "',BalQty='" + balQty + "',usedqty=ISNULL(usedqty,0)+ISNULL('" + Need + "',0)where ItemFK='" + itemcode + "' and DeptFK='" + ddl_hostelname.SelectedItem.Value + "'";//,IssuedQty='" + usedqty1 + "',IssuedRPU ='" + Convert.ToString(secondsplit[8]) + "' 
                                                                        in_s = d2.update_method_wo_parameter(stockvalue, "Text");
                                                                        if (in_s != 0)
                                                                            testflage = true;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    emptyrpu = true;
                                                                }
                                                            }
                                                            #endregion
                                                            Need = 0;
                                                            #region NonVegConsumption
                                                            if (Nonvegcount != 0)
                                                            {
                                                                double valueamt = Convert.ToDouble(Nonvegcount) / Convert.ToDouble(noofpersion) * Convert.ToDouble(needquantity);
                                                                if (valueamt != 0)
                                                                    Need = Need + valueamt;
                                                                requiredquantity = Need;
                                                                if (requiredquantity != 0)
                                                                {
                                                                    #region Rpu Calculation Barath 13.02.18
                                                                    DataSet RpuDS = new DataSet();
                                                                    RpuDS = d2.select_method_wo_parameter("select TransferQty,transferrpu,isnull(usedQuantity,0)as usedQuantity,TransferItemPK from IT_TransferItem where ItemFK='" + itemcode + "' and TrasferTo in('" + hostel + "') and TransferType in('0','1') and TransferQty<>ISNULL(usedQuantity,0) order by TrasnferDate", "text");
                                                                    if (RpuDS.Tables != null)
                                                                    {
                                                                        if (RpuDS.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            double TransferQty, TransferRpu, UsedQuantity, RemainingQty = 0;
                                                                            string newinsert = string.Empty;
                                                                            foreach (DataRow dr in RpuDS.Tables[0].Rows)
                                                                            {
                                                                                double.TryParse(Convert.ToString(dr["TransferQty"]), out TransferQty);
                                                                                double.TryParse(Convert.ToString(dr["transferrpu"]), out TransferRpu);
                                                                                double.TryParse(Convert.ToString(dr["usedQuantity"]), out UsedQuantity);
                                                                                string TransferItemPK = Convert.ToString(Convert.ToString(dr["TransferItemPK"]));
                                                                                if (TransferQty >= UsedQuantity + requiredquantity)//100>=120
                                                                                {
                                                                                    if (RemainingQty != 0)
                                                                                        requiredquantity = RemainingQty;
                                                                                    newinsert = " if exists(select*from HT_DailyConsumptionDetail  where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "'  and RPU='" + TransferRpu + "')update HT_DailyConsumptionDetail set ConsumptionQty=isnull(ConsumptionQty,0)+ ISNULL('" + requiredquantity + "',0),RPU='" + TransferRpu + "',Menutype='" + Menutype + "' where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "'  and RPU='" + TransferRpu + "' else insert into HT_DailyConsumptionDetail (ConsumptionQty,RPU,ItemFK,DailyConsumptionMasterFK,Menutype)values('" + requiredquantity + "','" + TransferRpu + "','" + itemcode + "','" + DailyConsumptionMasterPK + "','" + Menutype + "')";
                                                                                    newinsert += "update IT_TransferItem set UsedQuantity=isnull(UsedQuantity,0)+ISNULL('" + requiredquantity + "',0) where TransferItemPK='" + TransferItemPK + "'";
                                                                                    inst = d2.update_method_wo_parameter(newinsert, "Text");
                                                                                    RemainingQty = 0;
                                                                                }
                                                                                else if (TransferQty < UsedQuantity + requiredquantity)//100<120
                                                                                {
                                                                                    //RemainingQty = UsedQuantity + requiredquantity - TransferQty;//20
                                                                                    RemainingQty = requiredquantity - (TransferQty - UsedQuantity);//20
                                                                                    requiredquantity = TransferQty - UsedQuantity;
                                                                                    newinsert = " if exists(select*from HT_DailyConsumptionDetail  where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "'  and RPU='" + TransferRpu + "')update HT_DailyConsumptionDetail set ConsumptionQty=isnull(ConsumptionQty,0)+ ISNULL('" + requiredquantity + "',0),RPU='" + TransferRpu + "',Menutype='" + Menutype + "' where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "'  and RPU='" + TransferRpu + "' else insert into HT_DailyConsumptionDetail (ConsumptionQty,RPU,ItemFK,DailyConsumptionMasterFK,Menutype)values('" + requiredquantity + "','" + TransferRpu + "','" + itemcode + "','" + DailyConsumptionMasterPK + "','" + Menutype + "')";
                                                                                    newinsert += "update IT_TransferItem set UsedQuantity=isnull(UsedQuantity,0)+ISNULL('" + requiredquantity + "',0) where TransferItemPK='" + TransferItemPK + "'";
                                                                                    inst = d2.update_method_wo_parameter(newinsert, "Text");
                                                                                }
                                                                                if (RemainingQty == 0)
                                                                                    break;
                                                                            }
                                                                        }
                                                                    }
                                                                    #endregion
                                                                    //requiredquantity = (Math.Ceiling(requiredquantity));//13.02.18
                                                                    //string newinsert = " if exists(select*from HT_DailyConsumptionDetail  where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "')update HT_DailyConsumptionDetail set ConsumptionQty=isnull(ConsumptionQty,0)+ ISNULL('" + requiredquantity + "',0),RPU='" + rpu + "',Menutype='" + Menutype + "' where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "' else insert into HT_DailyConsumptionDetail (ConsumptionQty,RPU,ItemFK,DailyConsumptionMasterFK,Menutype)values('" + requiredquantity + "','" + rpu + "','" + itemcode + "','" + DailyConsumptionMasterPK + "','" + Menutype + "')";
                                                                    //int inst = d2.update_method_wo_parameter(newinsert, "Text");
                                                                    if (inst != 0)
                                                                    {
                                                                        finaltest = true;
                                                                    }
                                                                    double balQty = 0;
                                                                    int in_s = 0;
                                                                    double.TryParse(d2.GetFunction(" select IssuedQty-isnull(UsedQty,'0')as BalQty from IT_StockDeptDetail where ItemFK='" + itemcode + "' and DeptFK='" + hostel1 + "'"), out handquantity);
                                                                    if (handquantity >= Need && handquantity != 0)//requiredquantity
                                                                    {
                                                                        balQty = handquantity - Need;//requiredquantity
                                                                        string stockvalue = "if exists(select*from IT_StockDeptDetail where ItemFK='" + itemcode + "' and DeptFK='" + ddl_hostelname.SelectedItem.Value + "')update IT_StockDeptDetail set ItemFK ='" + itemcode + "',BalQty='" + balQty + "',usedqty=ISNULL(usedqty,0)+ISNULL('" + Need + "',0) where ItemFK='" + itemcode + "' and DeptFK='" + ddl_hostelname.SelectedItem.Value + "'";//,IssuedQty='" + usedqty1 + "',IssuedRPU ='" + Convert.ToString(secondsplit[8]) + "'
                                                                        in_s = d2.update_method_wo_parameter(stockvalue, "Text");
                                                                        if (in_s != 0)
                                                                            testflage = true;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    emptyrpu = true;
                                                                }
                                                            }
                                                            #endregion
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            lblConsuption.Visible = true;
                            lblConsuption.Text = "Few Items Hand Quantity is Very Low. Do you want to adjust Consume ?";
                            Div1.Visible = true;
                        }
                        if (fin == false)
                        {
                            lbl_alerterror.Visible = true;
                            lbl_alerterror.Text = "Please Select Any one Items";
                            alertmessage.Visible = true;
                        }
                        if (finaltest == true && testflage == true)
                        {
                            //btn_go_Click(sender, e);
                            lbl_alerterror.Visible = true;
                            lbl_alerterror.Text = "Saved Successfully";
                            alertmessage.Visible = true;
                        }
                        if (emptyrpu == true)
                        {
                            lbl_alerterror.Visible = true;
                            lbl_alerterror.Text = "Please Enter Sailing Prize";
                            alertmessage.Visible = true;
                        }
                    }
                    #endregion
                }
                if (rdb_cleanitem.Checked == true)
                {
                    #region Cleaning Item
                    bool checktemp = false;
                    bool savecheck = false;
                    bool insert = false;
                    bool Negtive = false;
                    if (FpSpread1.Sheets[0].RowCount > 0)
                    {
                        for (int row = 1; row < FpSpread1.Sheets[0].RowCount; row++)
                        {
                            int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, 7].Value);
                            if (checkval == 1)
                            {
                                checktemp = true;
                                if (FpSpread1.Sheets[0].Cells[row, 6].ForeColor == Color.Red)
                                {
                                    Negtive = true;
                                }
                            }
                        }
                        if (checktemp == true && Negtive == false)
                        {
                            int mastercode = 0;
                            string getcode = "";
                            string inserquery = "";
                            getcode = d2.GetFunction("select distinct top 1 DailyConsumptionMasterPK  from  HT_DailyConsumptionMaster where DailyConsDate='" + dt.ToString("MM/dd/yyyy") + "' and ForMess='0' order by DailyConsumptionMasterPK desc");
                            if (getcode.Trim() != "" && getcode.Trim() != "0")
                            {
                                mastercode = Convert.ToInt32(getcode);
                                inserquery = "update HT_DailyConsumptionMaster set DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' where DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' and ForMess='0'";
                            }
                            else
                            {
                                getcode = d2.GetFunction("select distinct top 1 DailyConsumptionMasterPK  from  HT_DailyConsumptionMaster  order by DailyConsumptionMasterPK desc");
                                if (getcode.Trim() != "" && getcode.Trim() != "0")
                                {
                                    mastercode = Convert.ToInt32(getcode) + 1;
                                }
                                else
                                {
                                    mastercode = 1;
                                }
                                inserquery = "insert into HT_DailyConsumptionMaster (DailyConsDate,ForMess,MessMasterFK,DeptFK)values('" + dt.ToString("MM/dd/yyyy") + "','0','" + ddl_hostelname.SelectedItem.Value + "','" + ddl_hostelname.SelectedItem.Value + "')";
                            }
                            int firstinsert = d2.update_method_wo_parameter(inserquery, "Text");
                            if (firstinsert != 0)
                            {
                                if (FpSpread1.Sheets[0].RowCount > 0)
                                {
                                    for (int row = 1; row < FpSpread1.Sheets[0].RowCount; row++)
                                    {
                                        int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, 7].Value);
                                        if (checkval == 1)
                                        {
                                            string itemcode = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Tag);
                                            string handquantity = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 4].Text);
                                            string requirquantity = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 6].Text);
                                            string rpuunit = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 3].Tag);//FpSpread1.Sheets[0].RowCount - 1
                                            double rpu = 0;
                                            double conusume = 0;
                                            if (rpuunit.Trim() != "")
                                            {
                                                rpu = Convert.ToDouble(rpuunit);
                                                conusume = Convert.ToDouble(requirquantity) * Convert.ToDouble(rpu);
                                            }
                                            string newinsert = " if exists(select * from HT_DailyConsumptionDetail  where DailyConsumptionMasterFK='" + mastercode + "' and ItemFK='" + itemcode + "')update HT_DailyConsumptionDetail set ConsumptionQty=ConsumptionQty+ ISNULL('" + requirquantity + "',0),RPU='" + rpu + "' where DailyConsumptionMasterFK='" + mastercode + "' and ItemFK='" + itemcode + "' else insert into HT_DailyConsumptionDetail (ConsumptionQty,RPU,ItemFK,DailyConsumptionMasterFK)values('" + requirquantity + "','" + rpu + "','" + itemcode + "','" + mastercode + "')";
                                            int inst = d2.update_method_wo_parameter(newinsert, "Text");
                                            if (inst != 0)
                                            {
                                                savecheck = true;
                                            }
                                            string getvalue = "";
                                            if (handquantity.Trim() != "" && handquantity.Trim() != "0")
                                            {
                                                double inval = Convert.ToDouble(handquantity);
                                                if (inval >= Convert.ToDouble(requirquantity))
                                                {
                                                    inval = inval - Convert.ToDouble(requirquantity);
                                                    getvalue = Convert.ToString(inval);
                                                }
                                                else
                                                {
                                                    getvalue = "0";
                                                }
                                            }
                                            else
                                            {
                                                getvalue = "0";
                                            }
                                            if (handquantity.Trim() == "")
                                            {
                                                handquantity = "0";
                                            }
                                            if (requirquantity.Trim() == "")
                                            {
                                                requirquantity = "0";
                                            }
                                            //double usedqty = 0;
                                            //if (getvalue.Trim() != "")
                                            //{
                                            //    usedqty = Convert.ToDouble(getvalue) + Convert.ToDouble(requirquantity);
                                            //}
                                            string stockvalue = "if exists(select*from IT_StockDeptDetail where ItemFK='" + itemcode + "' and DeptFK='" + ddl_hostelname.SelectedItem.Value + "')update IT_StockDeptDetail set ItemFK ='" + itemcode + "',BalQty='" + getvalue + "',usedqty=ISNULL(usedqty,0)+ISNULL('" + requirquantity + "',0),IssuedRPU ='" + rpu + "' where ItemFK='" + itemcode + "' and DeptFK='" + ddl_hostelname.SelectedItem.Value + "'";//IssuedQty='" + usedqty + "',
                                            int in_s = d2.update_method_wo_parameter(stockvalue, "Text");
                                            if (in_s != 0)
                                            {
                                                insert = true;
                                            }
                                        }
                                    }
                                }
                                if (savecheck == true && insert == true)
                                {
                                    //btn_go_Click(sender, e);
                                    lbl_alerterror.Visible = true;
                                    lbl_alerterror.Text = "Saved Successfully";
                                    alertmessage.Visible = true;
                                }
                                else
                                {
                                    lbl_alerterror.Visible = true;
                                    lbl_alerterror.Text = "Please Select Any one Items";
                                    alertmessage.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            if (Negtive == true)
                            {
                                lbl_alerterror.Visible = true;
                                lbl_alerterror.Text = "Item in Hand Very Low";
                                alertmessage.Visible = true;
                            }
                            else
                            {
                                lbl_alerterror.Visible = true;
                                lbl_alerterror.Text = "Please Select Any one Items";
                                alertmessage.Visible = true;
                            }
                        }
                    }
                    #endregion
                }
                #endregion
            }
            else
            {
                #region cleaning item
                if (rdb_cleanitem.Checked == true)
                {
                    bool checktemp = false;
                    bool savecheck = false;
                    bool insert = false;
                    bool Negtive = false;
                    if (FpSpread1.Sheets[0].RowCount > 0)
                    {
                        for (int row = 1; row < FpSpread1.Sheets[0].RowCount; row++)
                        {
                            int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, 7].Value);
                            if (checkval == 1)
                            {
                                checktemp = true;
                                double newhandvalue = 0;
                                double newgetvalue = 0;
                                string handvalue = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 4].Text);
                                string getvalue = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 6].Text);
                                if (handvalue.Trim() != "")
                                {
                                    newhandvalue = Convert.ToDouble(handvalue);
                                }
                                if (getvalue.Trim() != "")
                                {
                                    newgetvalue = Convert.ToDouble(getvalue);
                                }
                                if (newhandvalue < newgetvalue)
                                {
                                    Negtive = true;
                                }
                            }
                        }
                        if (checktemp == true && Negtive == false)
                        {
                            int mastercode = 0;
                            string getcode = "";
                            string inserquery = "";
                            getcode = d2.GetFunction("select distinct top 1 DailyConsumptionMasterPK  from  HT_DailyConsumptionMaster where DailyConsDate='" + dt.ToString("MM/dd/yyyy") + "' and ForMess='0' order by DailyConsumptionMasterPK desc");
                            if (getcode.Trim() != "" && getcode.Trim() != "0")
                            {
                                mastercode = Convert.ToInt32(getcode);
                                inserquery = "update HT_DailyConsumptionMaster set DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' where DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' and ForMess='0'";
                            }
                            else
                            {
                                getcode = d2.GetFunction("select distinct top 1 DailyConsumptionMasterPK  from  HT_DailyConsumptionMaster  order by DailyConsumptionMasterPK desc");
                                if (getcode.Trim() != "" && getcode.Trim() != "0")
                                {
                                    mastercode = Convert.ToInt32(getcode) + 1;
                                }
                                else
                                {
                                    mastercode = 1;
                                }
                                inserquery = "insert into HT_DailyConsumptionMaster (DailyConsDate,ForMess,MessMasterFK,DeptFK)values('" + dt.ToString("MM/dd/yyyy") + "','0','" + ddl_hostelname.SelectedItem.Value + "','" + ddl_hostelname.SelectedItem.Value + "')";
                            }
                            int firstinsert = d2.update_method_wo_parameter(inserquery, "Text");
                            if (mastercode != 0)
                            {
                                if (firstinsert != 0)
                                {
                                    if (FpSpread1.Sheets[0].RowCount > 0)
                                    {
                                        for (int row = 1; row < FpSpread1.Sheets[0].RowCount; row++)
                                        {
                                            int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, 7].Value);
                                            if (checkval == 1)
                                            {
                                                string itemcode = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Tag);
                                                string handquantity = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 4].Text);
                                                string requirquantity = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 6].Text);
                                                string rpuunit = Convert.ToString(FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag);
                                                double rpu = 0;
                                                double conusume = 0;
                                                if (rpuunit.Trim() != "")
                                                {
                                                    rpu = Convert.ToDouble(rpuunit);
                                                    conusume = Convert.ToDouble(requirquantity) * Convert.ToDouble(rpu);
                                                }
                                                string newinsert = " if exists(select * from HT_DailyConsumptionDetail  where DailyConsumptionMasterFK='" + mastercode + "' and ItemFK='" + itemcode + "')update HT_DailyConsumptionDetail set ConsumptionQty=ConsumptionQty+ ISNULL('" + requirquantity + "',0),RPU='" + rpu + "' where DailyConsumptionMasterFK='" + mastercode + "' and ItemFK='" + itemcode + "' else insert into HT_DailyConsumptionDetail (ConsumptionQty,RPU,ItemFK,DailyConsumptionMasterFK)values('" + requirquantity + "','" + rpu + "','" + itemcode + "','" + mastercode + "')";
                                                int inst = d2.update_method_wo_parameter(newinsert, "Text");
                                                if (inst != 0)
                                                {
                                                    savecheck = true;
                                                }
                                                string getvalue = "";
                                                if (handquantity.Trim() != "" && handquantity.Trim() != "0")
                                                {
                                                    double inval = Convert.ToDouble(handquantity);
                                                    if (inval >= Convert.ToDouble(requirquantity))
                                                    {
                                                        inval = inval - Convert.ToDouble(requirquantity);
                                                        getvalue = Convert.ToString(inval);
                                                    }
                                                    else
                                                    {
                                                        getvalue = "0";
                                                    }
                                                }
                                                else
                                                {
                                                    getvalue = "0";
                                                }
                                                if (requirquantity.Trim() == "")
                                                {
                                                    requirquantity = "0";
                                                }
                                                //double usedqty = 0;
                                                //if (getvalue.Trim() != "")
                                                //{
                                                //    usedqty = Convert.ToDouble(getvalue) + Convert.ToDouble(requirquantity);
                                                //}
                                                string stockvalue = "if exists(select*from IT_StockDeptDetail where ItemFK='" + itemcode + "' and DeptFK='" + ddl_hostelname.SelectedItem.Value + "')update IT_StockDeptDetail set ItemFK ='" + itemcode + "',BalQty='" + getvalue + "',usedqty=ISNULL(usedqty,0)+ISNULL('" + requirquantity + "',0),IssuedRPU ='" + rpu + "' where ItemFK='" + itemcode + "' and DeptFK='" + ddl_hostelname.SelectedItem.Value + "'";//,IssuedQty='" + usedqty + "'
                                                //string stockvalue = "if exists(select*from IT_StockDeptDetail where ItemFK='" + itemcode + "' and DeptFK='" + ddl_hostelname.SelectedItem.Value + "')update IT_StockDeptDetail set ItemFK ='" + itemcode + "',UsedQty='" + getvalue + "',IssuedRPU ='" + rpu + "' where ItemFK='" + itemcode + "' and DeptFK='" + ddl_hostelname.SelectedItem.Value + "'";
                                                int in_s = d2.update_method_wo_parameter(stockvalue, "Text");
                                                if (in_s != 0)
                                                {
                                                    insert = true;
                                                }
                                            }
                                        }
                                    }
                                    if (savecheck == true && insert == true)
                                    {
                                        //btn_go_Click(sender, e);
                                        lbl_alerterror.Visible = true;
                                        lbl_alerterror.Text = "Saved Successfully";
                                        alertmessage.Visible = true;
                                    }
                                    else
                                    {
                                        lbl_alerterror.Visible = true;
                                        lbl_alerterror.Text = "Please Select Any one Items";
                                        alertmessage.Visible = true;
                                    }
                                }
                            }
                            else
                            {
                                lbl_alerterror.Visible = true;
                                lbl_alerterror.Text = "Please Select Any one Items";
                                alertmessage.Visible = true;
                            }
                        }
                        else
                        {
                            if (Negtive == true)
                            {
                                lbl_alerterror.Visible = true;
                                lbl_alerterror.Text = "Item in Hand Very Low";
                                alertmessage.Visible = true;
                            }
                            else
                            {
                                lbl_alerterror.Visible = true;
                                lbl_alerterror.Text = "Please Select Any one Items";
                                alertmessage.Visible = true;
                            }
                        }
                    }
                }
                #endregion
                #region Menu Item
                if (rdb_menuitemcon.Checked == true)
                {

                    FpSpread1.SaveChanges();
                    if (FpSpread1.Sheets[0].RowCount > 0)
                    {
                        bool testflage = false;
                        bool finaltest = false;
                        bool fin = false;
                        string totalvalue = "";
                        bool Negtive = false;
                        Hashtable needarray = new Hashtable();
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 7].Value);
                            if (checkval == 1)
                            {
                                if (!additemcode.Contains(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag)))
                                {
                                    additemcode.Add(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag));
                                }
                                totalvalue = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 5].Text);
                                fin = true;
                                double newhandvalue = 0;
                                double newgetvalue = 0;
                                string handvalue = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Text);
                                string getvalue = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 6].Text);//entry requireqty 
                                if (getvalue.Trim() == "")
                                {
                                    getvalue = "0";
                                }
                                Hashtable itemcoderepect1 = (Hashtable)Session["itemcoderepect"];
                                if (itemcoderepect1.Count > 0)
                                {
                                    newgetvalue = Convert.ToDouble(getvalue);
                                    double row = 0;
                                    string count = Convert.ToString(itemcoderepect1[Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag)]);
                                    double.TryParse(count, out row);
                                    double avgitemrequired = Convert.ToDouble(getvalue) / row;
                                    getvalue = Convert.ToString(avgitemrequired);
                                }
                                if (!needarray.Contains(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag)))
                                {
                                    needarray.Add(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag), Convert.ToString(getvalue) + "-" + totalvalue);
                                }
                                if (handvalue.Trim() != "")
                                {
                                    newhandvalue = Convert.ToDouble(handvalue);
                                }
                                if (newhandvalue < newgetvalue)
                                {
                                    Negtive = true;
                                }
                            }
                        }
                        if (fin == true && Negtive == false)
                        {
                            Hashtable sessArray = (Hashtable)Session["AddArrayValue"];
                            if (sessArray.Count > 0)
                            {
                                foreach (DictionaryEntry pr in sessArray)
                                {
                                    string key = Convert.ToString(pr.Key);
                                    string keyvalue = Convert.ToString(pr.Value);
                                    string[] splitfirst = key.Split('-');
                                    if (splitfirst.Length > 0)
                                    {
                                        string hostel = Convert.ToString(splitfirst[0]);
                                        string menucode = Convert.ToString(splitfirst[1]);
                                        string session = Convert.ToString(splitfirst[2]);
                                        string[] splitnew = keyvalue.Split('/');
                                        splitnew = splitnew[0].Split('-');
                                        string total1 = Convert.ToString(splitnew[4]);
                                        if (needarray.ContainsKey(Convert.ToString(splitnew[7])))
                                        {
                                            string getSpreadstength = Convert.ToString(needarray[Convert.ToString(splitnew[7])]);
                                            string[] Spreadstrength = getSpreadstength.Split('-');
                                            if (Spreadstrength.Length == 2)
                                            {
                                                total1 = Convert.ToString(Spreadstrength[1]);
                                            }
                                        }
                                        string PurposeQry = string.Empty;
                                        string PurposeQryValue = string.Empty;
                                        string PurposeCatagoryCol = string.Empty;
                                        if (ddl_purposeCategory.Items.Count > 0)
                                        {
                                            if (ddl_purposeCategory.SelectedIndex != 0)
                                            {
                                                PurposeQry = " and PurposeCatagory='" + Convert.ToString(ddl_purposeCategory.SelectedItem.Value) + "'";
                                                PurposeCatagoryCol = ",PurposeCatagory";
                                                PurposeQryValue = " ,'" + Convert.ToString(ddl_purposeCategory.SelectedItem.Value) + "'";
                                            }
                                        }

                                        string inserquery = "if exists( select DailyConsumptionMasterPK from  HT_DailyConsumptionMaster where SessionFK ='" + session + "' and MessMasterFK in('" + hostel + "') and DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' and ForMess='1'  and MenumasterFK='" + menucode + "' " + PurposeQry + ") update HT_DailyConsumptionMaster set Total_Present='" + total1 + "' where SessionFK ='" + session + "' and MessMasterFK ='" + hostel + "' and DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' and ForMess='1'  and MenumasterFK='" + menucode + "' " + PurposeQry + " else insert into HT_DailyConsumptionMaster  (DailyConsDate,ForMess,MessMasterFK,SessionFK,DeptFK,UserCode,Total_Present, MenumasterFK " + PurposeCatagoryCol + ")values('" + dt.ToString("MM/dd/yyyy") + "','1','" + hostel + "','" + session + "','" + hostel + "','" + usercode + "','" + total1 + "','" + menucode + "' " + PurposeQryValue + ")";
                                        int insertQry = d2.update_method_wo_parameter(inserquery, "text");
                                        if (insertQry != 0)
                                        {
                                            splitfirst = keyvalue.Split('/');
                                            if (splitfirst.Length > 0)
                                            {
                                                for (int rs = 0; rs <= splitfirst.GetUpperBound(0); rs++)
                                                {
                                                    string[] secondsplit = splitfirst[rs].Split('-');
                                                    if (secondsplit.Length > 0)
                                                    {
                                                        string day = Convert.ToString(secondsplit[0]);
                                                        string hostel1 = Convert.ToString(secondsplit[1]);
                                                        string menucode1 = Convert.ToString(secondsplit[2]);
                                                        string session1 = Convert.ToString(secondsplit[3]);
                                                        //string total = Convert.ToString(secondsplit[4]);
                                                        string needquantity = Convert.ToString(secondsplit[5]);
                                                        string noofpersion = Convert.ToString(secondsplit[6]);
                                                        string itemcode = Convert.ToString(secondsplit[7]);
                                                        string rpu = Convert.ToString(secondsplit[8]);
                                                        string Menutype = Convert.ToString(secondsplit[9]);
                                                        if (additemcode.Contains(Convert.ToString(itemcode)) == true)
                                                        {
                                                            double requiredquantity = 0; double Need = 0;
                                                            //double.TryParse(Convert.ToString(needarray[Convert.ToString(itemcode)]), out requiredquantity);
                                                            if (needarray.ContainsKey(itemcode))
                                                            {
                                                                string getSpreadstength = Convert.ToString(needarray[itemcode]);
                                                                string[] Spreadstrength = getSpreadstength.Split('-');
                                                                if (getSpreadstength.Length > 1)
                                                                {
                                                                    total1 = Convert.ToString(Spreadstrength[1]);
                                                                    double.TryParse(Convert.ToString(Spreadstrength[0]), out requiredquantity);
                                                                }
                                                                else
                                                                    double.TryParse(Convert.ToString(Spreadstrength[0]), out requiredquantity);
                                                                Need = requiredquantity;
                                                            }
                                                            //requiredquantity = (Math.Ceiling(requiredquantity));
                                                            string DailyConsumptionMasterPK = string.Empty;
                                                            DataSet dailydetailsDs = new DataSet();
                                                            double handquantity = 0;
                                                            string q1 = "select DailyConsumptionMasterPK from HT_DailyConsumptionMaster where DailyConsDate='" + dt.ToString("MM/dd/yyyy") + "' and SessionFK='" + session1 + "' and MessMasterFK ='" + hostel1 + "' and ForMess='1'  and MenumasterFK='" + menucode1 + "' " + PurposeQry + "";
                                                            q1 += " select IssuedQty-isnull(UsedQty,'0')as BalQty from IT_StockDeptDetail where ItemFK='" + itemcode + "' and DeptFK='" + hostel1 + "'";
                                                            dailydetailsDs = d2.select_method_wo_parameter(q1, "text");
                                                            if (dailydetailsDs.Tables != null)
                                                            {
                                                                if (dailydetailsDs.Tables[0].Rows.Count > 0)
                                                                    DailyConsumptionMasterPK = Convert.ToString(dailydetailsDs.Tables[0].Rows[0]["DailyConsumptionMasterPK"]);
                                                                if (dailydetailsDs.Tables[1].Rows.Count > 0)
                                                                    double.TryParse(Convert.ToString(dailydetailsDs.Tables[1].Rows[0]["BalQty"]), out handquantity);
                                                            }
                                                            int inst = 0;
                                                            if (!string.IsNullOrEmpty(DailyConsumptionMasterPK))
                                                            {
                                                                #region Rpu Calculation Barath 13.02.18
                                                                DataSet RpuDS = new DataSet();
                                                                RpuDS = d2.select_method_wo_parameter("select TransferQty,transferrpu,isnull(usedQuantity,0)as usedQuantity,TransferItemPK from IT_TransferItem where ItemFK='" + itemcode + "' and TrasferTo in('" + hostel1 + "') and TransferType in('0','1') and TransferQty<>ISNULL(usedQuantity,0) order by TrasnferDate", "text");
                                                                if (RpuDS.Tables != null)
                                                                {
                                                                    if (RpuDS.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        double TransferQty, TransferRpu, UsedQuantity, RemainingQty = 0;
                                                                        string newinsert = string.Empty;
                                                                        foreach (DataRow dr in RpuDS.Tables[0].Rows)
                                                                        {
                                                                            double.TryParse(Convert.ToString(dr["TransferQty"]), out TransferQty);
                                                                            double.TryParse(Convert.ToString(dr["transferrpu"]), out TransferRpu);
                                                                            double.TryParse(Convert.ToString(dr["usedQuantity"]), out UsedQuantity);
                                                                            string TransferItemPK = Convert.ToString(Convert.ToString(dr["TransferItemPK"]));
                                                                            if (TransferQty >= UsedQuantity + requiredquantity)//100>=120
                                                                            {
                                                                                if (RemainingQty != 0)
                                                                                    requiredquantity = RemainingQty;
                                                                                newinsert = " if exists(select*from HT_DailyConsumptionDetail  where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "' and RPU='" + TransferRpu + "')update HT_DailyConsumptionDetail set ConsumptionQty=isnull(ConsumptionQty,0)+ ISNULL('" + requiredquantity + "',0),RPU='" + TransferRpu + "',Menutype='" + Menutype + "' where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "' and RPU='" + TransferRpu + "' else insert into HT_DailyConsumptionDetail (ConsumptionQty,RPU,ItemFK,DailyConsumptionMasterFK,Menutype)values('" + requiredquantity + "','" + TransferRpu + "','" + itemcode + "','" + DailyConsumptionMasterPK + "','" + Menutype + "')";
                                                                                newinsert += " update IT_TransferItem set UsedQuantity=isnull(UsedQuantity,0)+ISNULL('" + requiredquantity + "',0) where TransferItemPK='" + TransferItemPK + "'";
                                                                                inst = d2.update_method_wo_parameter(newinsert, "Text");
                                                                                RemainingQty = 0;
                                                                            }
                                                                            else if (TransferQty < UsedQuantity + requiredquantity)//100<120
                                                                            {
                                                                                double TransBal = TransferQty - UsedQuantity;
                                                                                RemainingQty = requiredquantity - TransBal;//20
                                                                                requiredquantity = TransferQty - UsedQuantity;
                                                                                newinsert = " if exists(select*from HT_DailyConsumptionDetail  where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "'  and RPU='" + TransferRpu + "')update HT_DailyConsumptionDetail set ConsumptionQty=isnull(ConsumptionQty,0)+ ISNULL('" + requiredquantity + "',0),RPU='" + TransferRpu + "',Menutype='" + Menutype + "' where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "'  and RPU='" + TransferRpu + "' else insert into HT_DailyConsumptionDetail (ConsumptionQty,RPU,ItemFK,DailyConsumptionMasterFK,Menutype)values('" + requiredquantity + "','" + TransferRpu + "','" + itemcode + "','" + DailyConsumptionMasterPK + "','" + Menutype + "')";
                                                                                newinsert += " update IT_TransferItem set UsedQuantity=isnull(UsedQuantity,0)+ISNULL('" + requiredquantity + "',0) where TransferItemPK='" + TransferItemPK + "'";
                                                                                inst = d2.update_method_wo_parameter(newinsert, "Text");
                                                                            }
                                                                            if (RemainingQty == 0)
                                                                                break;
                                                                        }
                                                                    }
                                                                }
                                                                #endregion
                                                                //string consumptionDet = " if exists(select*from HT_DailyConsumptionDetail  where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "')update HT_DailyConsumptionDetail set ConsumptionQty=ConsumptionQty+ ISNULL('" + requiredquantity + "',0),RPU='" + rpu + "' ,Menutype='" + Menutype + "' where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "' else insert into HT_DailyConsumptionDetail (DailyConsumptionMasterFK,ConsumptionQty,RPU,ItemFK,Menutype)values('" + DailyConsumptionMasterPK + "','" + requiredquantity + "','" + rpu + "','" + itemcode + "','" + Menutype + "')";
                                                                //int inst = d2.update_method_wo_parameter(consumptionDet, "Text");
                                                                if (inst != 0)
                                                                    finaltest = true;
                                                                if (handquantity >= Need && handquantity != 0)//requiredquantity
                                                                {
                                                                    double balQty = handquantity - Need;
                                                                    string stockDet = "if exists(select*from IT_StockDeptDetail where ItemFK='" + itemcode + "' and DeptFK='" + hostel1 + "')update IT_StockDeptDetail set ItemFK ='" + itemcode + "',BalQty='" + balQty + "',usedqty=ISNULL(usedqty,0)+ISNULL('" + Need + "',0) where ItemFK='" + itemcode + "' and DeptFK='" + hostel1 + "'";//,IssuedRPU ='" + rpu + "'
                                                                    inst = d2.update_method_wo_parameter(stockDet, "Text");
                                                                    if (inst != 0)
                                                                    {
                                                                        testflage = true;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #region old 18.07.17
                                        /*int mastercode = 0;
                                        string getcode = "";
                                        string inserquery = "";
                                        getcode = d2.GetFunction("select DailyConsumptionMasterPK from  HT_DailyConsumptionMaster where SessionFK ='" + session + "' and MessMasterFK in('" + hostel + "') and DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' and ForMess='1'  and MenumasterFK='" + menucode + "'");
                                        if (getcode.Trim() != "" && getcode.Trim() != "0")
                                        {
                                            mastercode = Convert.ToInt32(getcode);
                                            inserquery = "update HT_DailyConsumptionMaster set Total_Present='" + total1 + "' where SessionFK ='" + session + "' and MessMasterFK ='" + hostel + "' and DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' and ForMess='1'  and MenumasterFK='" + menucode + "'";
                                        }
                                        else
                                        {
                                            getcode = d2.GetFunction("select distinct top 1 DailyConsumptionMasterPK  from  HT_DailyConsumptionMaster  order by DailyConsumptionMasterPK desc");
                                            if (getcode.Trim() != "" && getcode.Trim() != "0")
                                            {
                                                mastercode = Convert.ToInt32(getcode) + 1;
                                            }
                                            else
                                            {
                                                mastercode = 1;
                                            }
                                            inserquery = "insert into HT_DailyConsumptionMaster  (DailyConsDate,ForMess,MessMasterFK,SessionFK,DeptFK,UserCode,Total_Present, MenumasterFK)values('" + dt.ToString("MM/dd/yyyy") + "','1','" + hostel + "','" + session + "','" + hostel + "','" + usercode + "','" + total1 + "','" + menucode + "')";
                                        }
                                        int firstinsert = d2.update_method_wo_parameter(inserquery, "Text");
                                        if (firstinsert != 0)
                                        {
                                            splitfirst = keyvalue.Split('/');
                                            if (splitfirst.Length > 0)
                                            {
                                                for (int rs = 0; rs <= splitfirst.GetUpperBound(0); rs++)
                                                {
                                                    string[] secondsplit = splitfirst[rs].Split('-');
                                                    if (secondsplit.Length > 0)
                                                    {
                                                        string day = Convert.ToString(secondsplit[0]);
                                                        string hostel1 = Convert.ToString(secondsplit[1]);
                                                        string menucode1 = Convert.ToString(secondsplit[2]);
                                                        string session1 = Convert.ToString(secondsplit[3]);
                                                        string total = Convert.ToString(secondsplit[4]);
                                                        string needquantity = Convert.ToString(secondsplit[5]);
                                                        string noofpersion = Convert.ToString(secondsplit[6]);
                                                        string itemcode = Convert.ToString(secondsplit[7]);
                                                        string rpu = Convert.ToString(secondsplit[8]);
                                                        if (additemcode.Contains(Convert.ToString(itemcode)) == true)
                                                        {
                                                            string handquantity = d2.GetFunction("select IssuedQty-isnull(UsedQty,'0')as BalQty from IT_StockDeptDetail where ItemFK='" + itemcode + "' and DeptFK='" + hostel1 + "'");
                                                            if (noofpersion.Trim() == "")
                                                            {
                                                                noofpersion = "0";
                                                            }
                                                            if (total.Trim() == "")
                                                            {
                                                                total = "0";
                                                            }
                                                            string requiredquantity = "";
                                                            double consumvalue = 0;
                                                            double Need = 0;
                                                            double valueamt = Convert.ToDouble(total) / Convert.ToDouble(noofpersion) * Convert.ToDouble(needquantity);
                                                            if (valueamt != 0)
                                                            {
                                                                Need = Need + valueamt;
                                                            }
                                                            requiredquantity = Convert.ToString(needarray[Convert.ToString(itemcode)]);
                                                            if (requiredquantity.Trim() != "" && requiredquantity.Trim() != "0")
                                                            {
                                                                if (rpu.Trim() != "" && requiredquantity.Trim() != "")
                                                                {
                                                                    consumvalue = Convert.ToDouble(rpu) * Convert.ToDouble(requiredquantity);
                                                                }
                                                                string DailyConsumptionMasterPK = d2.GetFunction("select DailyConsumptionMasterPK from HT_DailyConsumptionMaster where DailyConsDate='" + dt.ToString("MM/dd/yyyy") + "' and SessionFK='" + session + "' and MessMasterFK ='" + hostel + "' and ForMess='1'  and MenumasterFK='" + menucode + "'");
                                                                string newinsert = " if exists(select*from HT_DailyConsumptionDetail  where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "')update HT_DailyConsumptionDetail set ConsumptionQty=ConsumptionQty+ ISNULL('" + requiredquantity + "',0),RPU='" + rpu + "' where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "' else insert into HT_DailyConsumptionDetail (DailyConsumptionMasterFK,ConsumptionQty,RPU,ItemFK)values('" + DailyConsumptionMasterPK + "','" + requiredquantity + "','" + rpu + "','" + itemcode + "')";
                                                                int inst = d2.update_method_wo_parameter(newinsert, "Text");
                                                                if (inst != 0)
                                                                {
                                                                    finaltest = true;
                                                                }
                                                                string getvalue = "";
                                                                if (handquantity.Trim() != "" && handquantity.Trim() != "0")
                                                                {
                                                                    double inval = Convert.ToDouble(handquantity);
                                                                    if (inval >= Convert.ToDouble(requiredquantity))
                                                                    {
                                                                        inval = inval - Convert.ToDouble(requiredquantity);
                                                                        getvalue = Convert.ToString(inval);
                                                                    }
                                                                    else
                                                                    {
                                                                        getvalue = "0";
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    getvalue = "0";
                                                                }
                                                                if (requiredquantity.Trim() == "")
                                                                {
                                                                    requiredquantity = "0";
                                                                }
                                                                //double usedqty = 0;
                                                                //if (getvalue.Trim() != "")
                                                                //{
                                                                //    usedqty = Convert.ToDouble(getvalue) - Convert.ToDouble(requiredquantity);
                                                                //}
                                                                //string stockvalue = "if exists(select*from IT_StockDeptDetail where ItemFK='" + itemcode + "' and DeptFK='" + ddl_hostelname.SelectedItem.Value + "')update IT_StockDeptDetail set ItemFK ='" + itemcode + "',BalQty='" + getvalue + "',usedqty=ISNULL('" + requiredquantity + "',0),IssuedQty='" + usedqty + "',IssuedRPU ='" + rpu + "' where ItemFK='" + itemcode + "' and DeptFK='" + ddl_hostelname.SelectedItem.Value + "'";
                                                                string stockvalue = "if exists(select*from IT_StockDeptDetail where ItemFK='" + itemcode + "' and DeptFK='" + ddl_hostelname.SelectedItem.Value + "')update IT_StockDeptDetail set ItemFK ='" + itemcode + "',BalQty='" + getvalue + "',usedqty=ISNULL(usedqty,0)+ISNULL('" + requiredquantity + "',0),IssuedRPU ='" + rpu + "' where ItemFK='" + itemcode + "' and DeptFK='" + ddl_hostelname.SelectedItem.Value + "'";
                                                                int in_s = d2.update_method_wo_parameter(stockvalue, "Text");
                                                                if (in_s != 0)
                                                                {
                                                                    testflage = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }*/
                                        #endregion
                                    }
                                }
                            }
                        }
                        else
                        {
                            lbl_alerterror.Visible = true;
                            lbl_alerterror.Text = "Item in Hand Very Low";
                            alertmessage.Visible = true;
                        }
                        if (fin == false)
                        {
                            lbl_alerterror.Visible = true;
                            lbl_alerterror.Text = "Please Select Any one Items";
                            alertmessage.Visible = true;
                        }
                        if (finaltest == true && testflage == true)
                        {
                            // btn_go_Click(sender, e);
                            lbl_alerterror.Visible = true;
                            lbl_alerterror.Text = "Saved Successfully";
                            alertmessage.Visible = true;
                        }
                    }
                }
                #endregion
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
    protected void btn_add1_Click(object sender, EventArgs e)
    {
    }
    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        try
        {
            popwindow.Visible = true;
        }
        catch
        {
        }
    }
    protected void btn_exit1_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            popwindow.Visible = false;
        }
        catch
        {
        }
    }
    protected void btn_qmark_Click(object sender, EventArgs e)
    {
        try
        {
            popwindow1.Visible = true;
        }
        catch
        {
        }
    }
    protected void btn_save2_Click(object sender, EventArgs e)
    {
    }
    protected void btn_exit2_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            popwindow1.Visible = false;
        }
        catch
        {
        }
    }
    protected void clear()
    {
        try
        {
            txt_itemcode1.Text = "";
            txt_itemname1.Text = "";
            txt_rpu1.Text = "";
            txt_stockqty1.Text = "";
            txt_conqty1.Text = "";
            txt_itemcode2.Text = "";
            txt_itemname2.Text = "";
        }
        catch
        {
        }
    }
    protected void bindhostelname()
    {
        try
        {
            ddl_hostelname.Items.Clear();
            //string selectQuery = "select MessMasterPK,MessName,MessAcr from HM_MessMaster where CollegeCode=" + collegecode1 + " order by MessMasterPK asc";
            ds.Clear();
            // ds = d2.select_method_wo_parameter(selectQuery, "Text");
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_hostelname.DataSource = ds;
                ddl_hostelname.DataTextField = "MessName";
                ddl_hostelname.DataValueField = "MessMasterPK";
                ddl_hostelname.DataBind();
            }
            else
            {
            }
        }
        catch
        {
        }
    }
    protected void ddl_hostelname_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            bindsession();
            if (rdb_menuitemcon.Checked == true)
            {
                loadmenuname();
            }
            if (rdb_cleanitem.Checked == true)
            {
                loaditemname();
            }
        }
        catch
        {
        }
    }
    public void bindsession()
    {
        try
        {
            ds.Clear();
            if (ddl_hostelname.SelectedItem.Value.Trim() != "")
            {
                string selecthostel = "select SessionMasterPK,SessionName FROM HM_SessionMaster where MessMasterFK='" + ddl_hostelname.SelectedItem.Value + "' order by SessionName";
                ds = d2.select_method_wo_parameter(selecthostel, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_session.DataSource = ds;
                    ddl_session.DataTextField = "SessionName";
                    ddl_session.DataValueField = "SessionMasterPK";
                    ddl_session.DataBind();
                    cbl_session.DataSource = ds;
                    cbl_session.DataTextField = "SessionName";
                    cbl_session.DataValueField = "SessionMasterPK";
                    cbl_session.DataBind();
                    if (cbl_session.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_session.Items.Count; i++)
                        {
                            cbl_session.Items[i].Selected = true;
                        }
                        txt_sessionname.Text = "Session Name(" + cbl_session.Items.Count + ")";
                    }
                }
                else
                {
                    txt_sessionname.Text = "--Select--";
                }
            }
            else
            {
                txt_sessionname.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void ddl_session_SelectedIndexChange(object sender, EventArgs e)
    {
        if (rdb_menuitemcon.Checked == true)
        {
            loadmenuname();
        }
        if (rdb_cleanitem.Checked == true)
        {
            loaditemname();
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Daily Consumption Status Report";
            string pagename = "inv_Daily_consumption.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
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
    protected void cb_Additionalitem_check(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            labltotallable.Visible = false;
            rptprint.Visible = false;
            btn_save.Visible = false;
            if (cb_Additionalitem.Checked)
            {
                purposecatagoryTR.Visible = true;
                maintable.Attributes.Add("style", "border: 1px solid #0CA6CA; border-radius: 10px; margin-left: 34px;background-color: #0CA6CA; position: absolute; width: 940px; height: 110px; box-shadow: 0px 0px 8px #7bc1f7;");
                directDailyConsumption.Attributes.Add("style", "margin-right: 750px;margin-top: 45px; background-color: LightGreen;");
            }
            else
            {
                purposecatagoryTR.Visible = false;
                maintable.Attributes.Add("style", "border: 1px solid #0CA6CA; border-radius: 10px; margin-left: 34px;background-color: #0CA6CA; position: absolute; width: 940px; height: 80px; box-shadow: 0px 0px 8px #7bc1f7;");
                directDailyConsumption.Attributes.Add("style", "margin-right: 750px;margin-top: 5px; background-color: LightGreen;");
            }
        }
        catch
        {
        }
    }
    protected void btn_consumption_Click(object sender, EventArgs e)
    {
        DailyConsume();
        btn_go_Click(sender, e);
    }
    protected void btn_consumption_exit_Click(object sender, EventArgs e)
    {
        Div1.Visible = false;
    }
    public void DailyConsume()
    {
        FpSpread1.SaveChanges();
        Hashtable hnew = new Hashtable();
        string firstdate = Convert.ToString(txt_date.Text);
        DateTime dt = new DateTime();
        string[] split = firstdate.Split('/');
        dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
        string dtaccessdate = DateTime.Now.ToString();
        string dtaccesstime = DateTime.Now.ToLongTimeString();
        string setrpu = d2.GetFunction("select value from Master_Settings where settings='Consumption Rpu' and usercode='" + usercode + "'");
        if (rdb_menuitemcon.Checked == true)
        {
            ArrayList additemcode = new ArrayList();
            bool testflage = false;
            bool finaltest = false;
            bool rpuempty = false;
            bool fin = false;
            string totalvalue = "";
            bool Negtive = false;
            int column = 7;
            if (!cb_Additionalitem.Checked)
                column = 9;
            for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                // int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 7].Value);
                int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, column].Value);
                if (checkval == 1)
                {
                    if (!additemcode.Contains(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag)))
                    {
                        additemcode.Add(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag));
                    }
                }
            }
            Hashtable sessArray = (Hashtable)Session["AddArrayValue"];
            if (sessArray.Count > 0)
            {
                foreach (DictionaryEntry pr in sessArray)
                {
                    string key = Convert.ToString(pr.Key);
                    string keyvalue = Convert.ToString(pr.Value);
                    string[] splitfirst = key.Split('-');
                    if (splitfirst.Length > 0)
                    {
                        string hostel = Convert.ToString(splitfirst[0]);
                        string menucode = Convert.ToString(splitfirst[1]);
                        string session = Convert.ToString(splitfirst[2]);
                        string[] splitnew = keyvalue.Split('/');
                        splitnew = splitnew[0].Split('-');
                        //string total1 = Convert.ToString(splitnew[4]);
                        double Vegcount = 0;
                        double Nonvegcount = 0;
                        double.TryParse(Convert.ToString(splitnew[10]), out Vegcount);
                        double.TryParse(Convert.ToString(splitnew[11]), out Nonvegcount);
                        string Menutype = Convert.ToString(splitnew[12]);
                        double total1 = Vegcount + Nonvegcount;
                        string getcode = "";
                        string inserquery = "";
                        getcode = d2.GetFunction("select DailyConsumptionMasterPK from  HT_DailyConsumptionMaster where SessionFK ='" + session + "' and MessMasterFK in('" + hostel + "') and DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' and ForMess='1'  and MenumasterFK='" + menucode + "'");
                        if (getcode.Trim() != "" && getcode.Trim() != "0")
                        {
                            inserquery = "update HT_DailyConsumptionMaster set Total_Present='" + total1 + "',VegStrength='" + Vegcount + "',NonvegStrength='" + Nonvegcount + "' where SessionFK ='" + session + "' and MessMasterFK ='" + hostel + "' and DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' and ForMess='1'  and MenumasterFK='" + menucode + "'";
                        }
                        else
                        {
                            inserquery = "insert into HT_DailyConsumptionMaster  (DailyConsDate,ForMess,MessMasterFK,SessionFK,DeptFK,UserCode,Total_Present, MenumasterFK,VegStrength,NonvegStrength)values('" + dt.ToString("MM/dd/yyyy") + "','1','" + hostel + "','" + session + "','" + hostel + "','" + usercode + "','" + total1 + "','" + menucode + "','" + Vegcount + "','" + Nonvegcount + "')";
                        }
                        int firstinsert = d2.update_method_wo_parameter(inserquery, "Text");
                        if (firstinsert != 0)
                        {
                            splitfirst = keyvalue.Split('/');
                            if (splitfirst.Length > 0)
                            {
                                for (int rs = 0; rs <= splitfirst.GetUpperBound(0); rs++)
                                {
                                    string[] secondsplit = splitfirst[rs].Split('-');
                                    if (secondsplit.Length > 0)
                                    {
                                        string day = Convert.ToString(secondsplit[0]);
                                        string hostel1 = Convert.ToString(secondsplit[1]);
                                        string menucode1 = Convert.ToString(secondsplit[2]);
                                        string session1 = Convert.ToString(secondsplit[3]);
                                        string total = Convert.ToString(secondsplit[4]);
                                        string needquantity = Convert.ToString(secondsplit[5]);
                                        string noofpersion = Convert.ToString(secondsplit[6]);
                                        string itemcode = Convert.ToString(secondsplit[7]);
                                        string Handqunty = Convert.ToString(secondsplit[9]);
                                        Vegcount = 0;
                                        Nonvegcount = 0;
                                        double.TryParse(Convert.ToString(splitnew[10]), out Vegcount);
                                        double.TryParse(Convert.ToString(splitnew[11]), out Nonvegcount);
                                        Menutype = Convert.ToString(splitnew[12]);

                                        //24.06.16
                                        //string rpu = Convert.ToString(secondsplit[8]);
                                        //string rpu =d2.GetFunction("select AVG(Sailing_prize) Avg_rpu from IT_StockDeptDetail where ItemFK='" + itemcode + "'");
                                        //string rpu = "";//14.02.18 barath
                                        //if (setrpu.Trim() == "0")
                                        //{
                                        //    rpu = d2.GetFunction("select AVG(IssuedRPU) Avg_rpu from IT_StockDeptDetail where ItemFK='" + itemcode + "' and DeptFK in('" + hostel1 + "')");
                                        //}
                                        //if (setrpu.Trim() == "1")
                                        //{
                                        //    rpu = d2.GetFunction("select AVG(Sailing_prize) Avg_rpu from IT_StockDeptDetail where ItemFK='" + itemcode + "' and DeptFK in('" + hostel1 + "')");
                                        //}
                                        if (additemcode.Contains(Convert.ToString(itemcode)) == true)
                                        {
                                            string handquantity = d2.GetFunction("select IssuedQty-isnull(UsedQty,'0')as BalQty from IT_StockDeptDetail where ItemFK='" + itemcode + "' and DeptFK='" + hostel1 + "'");
                                            double requiredquantity = 0;
                                            double consumvalue = 0;
                                            double Need = 0;
                                            double AdjustQuantity = 0;
                                            double consumequantity = 0;
                                            string isadjust = "0";
                                            double hand = Convert.ToDouble(handquantity);
                                            int inst = 0;
                                            #region VegConsumption
                                            if (Vegcount != 0)
                                            {
                                                double valueamt = Convert.ToDouble(Vegcount) / Convert.ToDouble(noofpersion) * Convert.ToDouble(needquantity);
                                                if (valueamt != 0)
                                                    Need += valueamt;
                                                if (Need > hand)
                                                {
                                                    isadjust = "1";
                                                    AdjustQuantity = Need - hand;
                                                    consumequantity = hand;
                                                }
                                                else
                                                    consumequantity = Need;
                                                double.TryParse(Convert.ToString(Math.Round(Need, 2)), out requiredquantity);//20.01.18);                                               
                                                if (requiredquantity != 0)//rpu.Trim() != "" &&
                                                {
                                                    string DailyConsumptionMasterPK = d2.GetFunction("select DailyConsumptionMasterPK from HT_DailyConsumptionMaster where DailyConsDate='" + dt.ToString("MM/dd/yyyy") + "' and SessionFK='" + session + "' and MessMasterFK ='" + hostel + "' and ForMess='1'  and MenumasterFK='" + menucode + "'");
                                                    #region Rpu Calculation Barath 13.02.18
                                                    DataSet RpuDS = new DataSet();
                                                    RpuDS = d2.select_method_wo_parameter("select TransferQty,transferrpu,isnull(usedQuantity,0)as usedQuantity,TransferItemPK from IT_TransferItem where ItemFK='" + itemcode + "' and TrasferTo in('" + hostel + "') and TransferType in('0','1') and TransferQty<>ISNULL(usedQuantity,0) order by TrasnferDate", "text");
                                                    if (RpuDS.Tables != null)
                                                    {
                                                        if (RpuDS.Tables[0].Rows.Count > 0)
                                                        {
                                                            double TransferQty, TransferRpu, UsedQuantity, RemainingQty = 0;
                                                            string newinsert = string.Empty;
                                                            foreach (DataRow dr in RpuDS.Tables[0].Rows)
                                                            {
                                                                double.TryParse(Convert.ToString(dr["TransferQty"]), out TransferQty);
                                                                double.TryParse(Convert.ToString(dr["transferrpu"]), out TransferRpu);
                                                                double.TryParse(Convert.ToString(dr["usedQuantity"]), out UsedQuantity);
                                                                string TransferItemPK = Convert.ToString(Convert.ToString(dr["TransferItemPK"]));
                                                                if (TransferQty >= UsedQuantity + requiredquantity)//100>=120
                                                                {
                                                                    if (RemainingQty != 0)
                                                                        requiredquantity = RemainingQty;
                                                                    newinsert = " if exists(select*from HT_DailyConsumptionDetail  where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "'  and RPU='" + TransferRpu + "')update HT_DailyConsumptionDetail set ConsumptionQty=isnull(ConsumptionQty,0)+ ISNULL('" + requiredquantity + "',0),RPU='" + TransferRpu + "',Menutype='" + Menutype + "' where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "' and RPU='" + TransferRpu + "' else insert into HT_DailyConsumptionDetail (ConsumptionQty,RPU,ItemFK,DailyConsumptionMasterFK,Menutype)values('" + requiredquantity + "','" + TransferRpu + "','" + itemcode + "','" + DailyConsumptionMasterPK + "','" + Menutype + "')";
                                                                    newinsert += " update IT_TransferItem set UsedQuantity=isnull(UsedQuantity,0)+ISNULL('" + requiredquantity + "',0) where TransferItemPK='" + TransferItemPK + "'";
                                                                    inst = d2.update_method_wo_parameter(newinsert, "Text");
                                                                    RemainingQty = 0;
                                                                }
                                                                else if (TransferQty < UsedQuantity + requiredquantity)//100<120
                                                                {
                                                                    //RemainingQty = UsedQuantity + requiredquantity - TransferQty;//20
                                                                    double TransBal = TransferQty - UsedQuantity;
                                                                    RemainingQty = requiredquantity - TransBal;//20
                                                                    requiredquantity = TransferQty - UsedQuantity;
                                                                    newinsert = " if exists(select*from HT_DailyConsumptionDetail  where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "'  and RPU='" + TransferRpu + "')update HT_DailyConsumptionDetail set ConsumptionQty=isnull(ConsumptionQty,0)+ ISNULL('" + requiredquantity + "',0),RPU='" + TransferRpu + "',Menutype='" + Menutype + "' where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "'  and RPU='" + TransferRpu + "' else insert into HT_DailyConsumptionDetail (ConsumptionQty,RPU,ItemFK,DailyConsumptionMasterFK,Menutype)values('" + requiredquantity + "','" + TransferRpu + "','" + itemcode + "','" + DailyConsumptionMasterPK + "','" + Menutype + "')";
                                                                    newinsert += " update IT_TransferItem set UsedQuantity=isnull(UsedQuantity,0)+ISNULL('" + requiredquantity + "',0) where TransferItemPK='" + TransferItemPK + "'";
                                                                    inst = d2.update_method_wo_parameter(newinsert, "Text");
                                                                }
                                                                if (RemainingQty == 0)
                                                                    break;
                                                            }
                                                        }
                                                    }
                                                    #endregion
                                                    //barath 14.02.18
                                                    //string newinsert = " if exists(select*from HT_DailyConsumptionDetail  where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "')update HT_DailyConsumptionDetail set ConsumptionQty=ConsumptionQty+ ISNULL('" + requiredquantity + "',0),RPU='" + rpu + "',Adjust_Qty=isnull(Adjust_Qty,0)+'" + AdjustQuantity + "' where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "' else insert into HT_DailyConsumptionDetail (ConsumptionQty,RPU,ItemFK,Isadjust,Adjust_Qty )values('" + requiredquantity + "','" + rpu + "','" + itemcode + "','" + isadjust + "','" + AdjustQuantity + "')";
                                                    //int inst = d2.update_method_wo_parameter(newinsert, "Text");
                                                    if (inst != 0)
                                                        finaltest = true;
                                                    string getvalue = string.Empty;
                                                    if (handquantity.Trim() != "" && handquantity.Trim() != "0" && handquantity.Trim() != "0.00")
                                                    {
                                                        double inval = Convert.ToDouble(handquantity);
                                                        if (inval >= Convert.ToDouble(Need))//requiredquantity
                                                        {
                                                            inval = inval - Convert.ToDouble(Need); //requiredquantity
                                                            getvalue = Convert.ToString(inval);
                                                        }
                                                        else
                                                            getvalue = "0";
                                                    }
                                                    else
                                                        getvalue = "0";
                                                    string stockvalue = "if exists(select*from IT_StockDeptDetail where ItemFK='" + itemcode + "' and DeptFK='" + ddl_hostelname.SelectedItem.Value + "')update IT_StockDeptDetail set ItemFK ='" + itemcode + "',BalQty='" + getvalue + "',usedqty=usedqty+ISNULL('" + Need + "',0) where ItemFK='" + itemcode + "' and DeptFK='" + ddl_hostelname.SelectedItem.Value + "'";//25.06.16,IssuedQty='" + usedqty1 + "',IssuedRPU ='" + Convert.ToString(secondsplit[8]) + "'
                                                    int in_s = d2.update_method_wo_parameter(stockvalue, "Text");
                                                    if (in_s != 0)
                                                        testflage = true;
                                                }
                                                else
                                                    rpuempty = true;
                                            }
                                            #endregion
                                            #region NonVegConsumption
                                            if (Nonvegcount != 0)
                                            {
                                                double valueamt = Convert.ToDouble(Nonvegcount) / Convert.ToDouble(noofpersion) * Convert.ToDouble(needquantity);
                                                if (valueamt != 0)
                                                    Need += valueamt;
                                                if (Need > hand)
                                                {
                                                    isadjust = "1";
                                                    AdjustQuantity = Need - hand;
                                                    consumequantity = hand;
                                                }
                                                else
                                                    consumequantity = Need;
                                                double.TryParse(Convert.ToString(Math.Round(Need, 2)), out requiredquantity);//20.01.18
                                                if (requiredquantity != 0)//(rpu.Trim() != "" &&
                                                {
                                                    string DailyConsumptionMasterPK = d2.GetFunction("select DailyConsumptionMasterPK from HT_DailyConsumptionMaster where DailyConsDate='" + dt.ToString("MM/dd/yyyy") + "' and SessionFK='" + session + "' and MessMasterFK ='" + hostel + "' and ForMess='1'  and MenumasterFK='" + menucode + "'");
                                                    #region Rpu Calculation Barath 13.02.18
                                                    DataSet RpuDS = new DataSet();
                                                    RpuDS = d2.select_method_wo_parameter("select TransferQty,transferrpu,isnull(usedQuantity,0)as usedQuantity,TransferItemPK from IT_TransferItem where ItemFK='" + itemcode + "' and TrasferTo in('" + hostel + "') and TransferType in('0','1') and TransferQty<>ISNULL(usedQuantity,0) order by TrasnferDate", "text");
                                                    if (RpuDS.Tables != null)
                                                    {
                                                        if (RpuDS.Tables[0].Rows.Count > 0)
                                                        {
                                                            double TransferQty, TransferRpu, UsedQuantity, RemainingQty = 0;
                                                            string newinsert = string.Empty;
                                                            foreach (DataRow dr in RpuDS.Tables[0].Rows)
                                                            {
                                                                double.TryParse(Convert.ToString(dr["TransferQty"]), out TransferQty);
                                                                double.TryParse(Convert.ToString(dr["transferrpu"]), out TransferRpu);
                                                                double.TryParse(Convert.ToString(dr["usedQuantity"]), out UsedQuantity);
                                                                string TransferItemPK = Convert.ToString(Convert.ToString(dr["TransferItemPK"]));
                                                                if (TransferQty >= UsedQuantity + requiredquantity)//100>=120
                                                                {
                                                                    if (RemainingQty != 0)
                                                                        requiredquantity = RemainingQty;
                                                                    newinsert = " if exists(select*from HT_DailyConsumptionDetail  where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "'  and RPU='" + TransferRpu + "')update HT_DailyConsumptionDetail set ConsumptionQty=ConsumptionQty+ ISNULL('" + requiredquantity + "',0),RPU='" + TransferRpu + "',Adjust_Qty=isnull(Adjust_Qty,0)+'" + AdjustQuantity + "' where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "'  and RPU='" + TransferRpu + "' else insert into HT_DailyConsumptionDetail (ConsumptionQty,RPU,ItemFK,Isadjust,Adjust_Qty )values('" + requiredquantity + "','" + TransferRpu + "','" + itemcode + "','" + isadjust + "','" + AdjustQuantity + "')";
                                                                    newinsert += " update IT_TransferItem set UsedQuantity=isnull(UsedQuantity,0)+ISNULL('" + requiredquantity + "',0) where TransferItemPK='" + TransferItemPK + "'";
                                                                    inst = d2.update_method_wo_parameter(newinsert, "Text");
                                                                    RemainingQty = 0;
                                                                }
                                                                else if (TransferQty < UsedQuantity + requiredquantity)//100<120
                                                                {
                                                                    //RemainingQty = UsedQuantity + requiredquantity - TransferQty;//20
                                                                    double TransBal = TransferQty - UsedQuantity;
                                                                    RemainingQty = requiredquantity - TransBal;//20
                                                                    requiredquantity = TransferQty - UsedQuantity;
                                                                    newinsert = "if exists(select*from HT_DailyConsumptionDetail  where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "'  and RPU='" + TransferRpu + "')update HT_DailyConsumptionDetail set ConsumptionQty=ConsumptionQty+ ISNULL('" + requiredquantity + "',0),RPU='" + TransferRpu + "',Adjust_Qty=isnull(Adjust_Qty,0)+'" + AdjustQuantity + "' where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "'  and RPU='" + TransferRpu + "' else insert into HT_DailyConsumptionDetail (ConsumptionQty,RPU,ItemFK,Isadjust,Adjust_Qty )values('" + requiredquantity + "','" + TransferRpu + "','" + itemcode + "','" + isadjust + "','" + AdjustQuantity + "')";
                                                                    newinsert += " update IT_TransferItem set UsedQuantity=isnull(UsedQuantity,0)+ISNULL('" + requiredquantity + "',0) where TransferItemPK='" + TransferItemPK + "'";
                                                                    inst = d2.update_method_wo_parameter(newinsert, "Text");
                                                                }
                                                                if (RemainingQty == 0)
                                                                    break;
                                                            }
                                                        }
                                                    }
                                                    #endregion
                                                    //string newinsert = " if exists(select*from HT_DailyConsumptionDetail  where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "')update HT_DailyConsumptionDetail set ConsumptionQty=ConsumptionQty+ ISNULL('" + requiredquantity + "',0),RPU='" + rpu + "',Adjust_Qty=isnull(Adjust_Qty,0)+'" + AdjustQuantity + "' where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "' else insert into HT_DailyConsumptionDetail (ConsumptionQty,RPU,ItemFK,Isadjust,Adjust_Qty )values('" + requiredquantity + "','" + rpu + "','" + itemcode + "','" + isadjust + "','" + AdjustQuantity + "')";
                                                    //int inst = d2.update_method_wo_parameter(newinsert, "Text");
                                                    if (inst != 0)
                                                        finaltest = true;
                                                    string getvalue = "";
                                                    if (handquantity.Trim() != "" && handquantity.Trim() != "0" && handquantity.Trim() != "0.00")
                                                    {
                                                        double inval = Convert.ToDouble(handquantity);
                                                        if (inval >= Convert.ToDouble(requiredquantity))
                                                        {
                                                            inval = inval - Convert.ToDouble(requiredquantity);
                                                            getvalue = Convert.ToString(inval);
                                                        }
                                                        else
                                                            getvalue = "0";
                                                    }
                                                    else
                                                        getvalue = "0";
                                                    string stockvalue = "if exists(select*from IT_StockDeptDetail where ItemFK='" + itemcode + "' and DeptFK='" + ddl_hostelname.SelectedItem.Value + "')update IT_StockDeptDetail set ItemFK ='" + itemcode + "',BalQty='" + getvalue + "',usedqty=usedqty+ISNULL('" + requiredquantity + "',0),IssuedRPU ='" + Convert.ToString(secondsplit[8]) + "' where ItemFK='" + itemcode + "' and DeptFK='" + ddl_hostelname.SelectedItem.Value + "'";//25.06.16,IssuedQty='" + usedqty1 + "'
                                                    int in_s = d2.update_method_wo_parameter(stockvalue, "Text");
                                                    if (in_s != 0)
                                                        testflage = true;
                                                }
                                                else
                                                    rpuempty = true;
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (finaltest == true && testflage == true)
            {
                // btn_go_Click(sender, e);
                Div1.Visible = false;
                lbl_alerterror.Visible = true;
                lbl_alerterror.Text = "Saved Successfully";
                alertmessage.Visible = true;
            }
            if (rpuempty == true)
            {
                Div1.Visible = false;
                lbl_alerterror.Visible = true;
                lbl_alerterror.Text = "Please Enter Sailing Prize";
                alertmessage.Visible = true;
            }
        }
        else
        {
            bool checktemp = false;
            bool savecheck = false;
            bool insert = false;
            bool Negtive = false;
            if (FpSpread1.Sheets[0].RowCount > 0)
            {
                for (int row = 1; row < FpSpread1.Sheets[0].RowCount; row++)
                {
                    int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, 7].Value);
                    if (checkval == 1)
                    {
                        checktemp = true;
                        if (FpSpread1.Sheets[0].Cells[row, 6].ForeColor == Color.Red)
                        {
                            Negtive = true;
                        }
                    }
                }
                if (checktemp == true && Negtive == true)
                {
                    int mastercode = 0;
                    string getcode = "";
                    string inserquery = "";
                    getcode = d2.GetFunction("select DailyConsumptionMasterPK from  HT_DailyConsumptionMaster where DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' and ForMess='1' and MessMasterFK ='" + ddl_hostelname.SelectedItem.Value + "'");
                    if (getcode.Trim() != "" && getcode.Trim() != "0")
                    {
                        mastercode = Convert.ToInt32(getcode);
                        inserquery = "update HT_DailyConsumptionMaster set MessMasterFK ='" + ddl_hostelname.SelectedItem.Value + "' where MessMasterFK ='" + ddl_hostelname.SelectedItem.Value + "' and DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' and ForMess='1'";
                    }
                    else
                    {
                        inserquery = "insert into HT_DailyConsumptionMaster(DailyConsDate,ForMess,MessMasterFK,DeptFK,UserCode)values('" + dt.ToString("MM/dd/yyyy") + "','1','" + ddl_hostelname.SelectedItem.Value + "','" + ddl_hostelname.SelectedItem.Value + "','" + usercode + "')";
                    }
                    int firstinsert = d2.update_method_wo_parameter(inserquery, "Text");
                    if (firstinsert != 0)
                    {
                        if (FpSpread1.Sheets[0].RowCount > 0)
                        {
                            for (int row = 1; row < FpSpread1.Sheets[0].RowCount; row++)
                            {
                                int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, 7].Value);
                                if (checkval == 1)
                                {
                                    string itemcode = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Tag);
                                    string handquantity = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 4].Text);
                                    string requirquantity = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 6].Text);
                                    string rpuunit = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 3].Tag);
                                    double hand = 0;
                                    double reqty = 0;
                                    string adjust = "";
                                    double adjqty = 0;
                                    if (handquantity.Trim() != "")
                                    {
                                        hand = Convert.ToDouble(handquantity);
                                    }
                                    if (requirquantity.Trim() != "")
                                    {
                                        reqty = Convert.ToDouble(requirquantity);
                                    }
                                    if (hand < reqty)
                                    {
                                        adjust = "1";
                                        adjqty = reqty - hand;
                                        //requirquantity = Convert.ToString(hand);
                                        requirquantity = Convert.ToString(Math.Round(hand, 2));//20.01.18
                                    }

                                    double rpu = 0;
                                    double conusume = 0;
                                    if (rpuunit.Trim() != "")
                                    {
                                        rpu = Convert.ToDouble(rpuunit);
                                        conusume = Convert.ToDouble(requirquantity) * Convert.ToDouble(rpu);
                                    }
                                    if (mastercode == 0)
                                    {
                                        getcode = d2.GetFunction("select DailyConsumptionMasterPK from  HT_DailyConsumptionMaster where MessMasterFK ='" + ddl_hostelname.SelectedItem.Value + "' and DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' and ForMess='1'");
                                        mastercode = Convert.ToInt32(getcode);
                                    }
                                    string newinsert = "insert into HT_DailyConsumptionDetail(ItemFK,ConsumptionQty,DailyConsumptionMasterFK,isadjust,adjust_qty)values('" + itemcode + "','" + requirquantity + "','" + mastercode + "','" + adjust + "','" + adjqty + "')";
                                    int inst = d2.update_method_wo_parameter(newinsert, "Text");
                                    if (inst != 0)
                                    {
                                        savecheck = true;
                                    }
                                    string getvalue = "";
                                    if (handquantity.Trim() != "" && handquantity.Trim() != "0")
                                    {
                                        double inval = Convert.ToDouble(handquantity);
                                        if (inval >= Convert.ToDouble(requirquantity))
                                        {
                                            inval = inval - Convert.ToDouble(requirquantity);
                                            getvalue = Convert.ToString(inval);
                                        }
                                        else
                                        {
                                            getvalue = "0";
                                        }
                                    }
                                    else
                                    {
                                        getvalue = "0";
                                    }
                                    string stockvalue = "if exists (select * from IT_StockDeptDetail where ItemFK ='" + itemcode + "' and DeptFK='" + ddl_hostelname.SelectedItem.Value + "') update IT_StockDeptDetail set BalQty ='" + getvalue + "' where ItemFK='" + itemcode + "' and DeptFK='" + ddl_hostelname.SelectedItem.Value + "'";
                                    int in_s = d2.update_method_wo_parameter(stockvalue, "Text");
                                    if (in_s != 0)
                                    {
                                        insert = true;
                                    }
                                }
                            }
                        }
                        if (savecheck == true && insert == true)
                        {
                            //btn_go_Click(sender, e);
                            lbl_alerterror.Visible = true;
                            lbl_alerterror.Text = "Saved Successfully";
                            alertmessage.Visible = true;
                        }
                        else
                        {
                            lbl_alerterror.Visible = true;
                            lbl_alerterror.Text = "Please Select Any one Items";
                            alertmessage.Visible = true;
                        }
                    }
                }
            }
        }
    }
    //18.04.16 
    protected void cb_menutype_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cb_menutype, cbl_menutype, txt_menutype, "Menu Type");
        loadmenuname();
    }
    protected void cbl_menutype_SelectIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_menutype, cbl_menutype, txt_menutype, "Menu Type");
        loadmenuname();
    }
    private void loadsession()
    {
        try
        {
            ds.Clear();
            cblDirSes.Items.Clear();
            if (ddlDirMessName.SelectedItem.Value.Trim() != "")
            {
                string selecthostel = "select SessionMasterPK,SessionName FROM HM_SessionMaster where MessMasterFK='" + ddlDirMessName.SelectedItem.Value + "' order by SessionName";
                ds = d2.select_method_wo_parameter(selecthostel, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cblDirSes.DataSource = ds;
                    cblDirSes.DataTextField = "SessionName";
                    cblDirSes.DataValueField = "SessionMasterPK";
                    cblDirSes.DataBind();
                    if (cblDirSes.Items.Count > 0)
                    {
                        for (int i = 0; i < cblDirSes.Items.Count; i++)
                        {
                            cblDirSes.Items[i].Selected = true;
                        }
                        txtDirSes.Text = "Session Name(" + cblDirSes.Items.Count + ")";
                    }
                }
                else
                {
                    txtDirSes.Text = "--Select--";
                }
            }
            else
            {
                txtDirSes.Text = "--Select--";
            }
            directinvward();
        }
        catch { }
    }
    public void bindmenuname()
    {
        try
        {
            hat.Clear();
            string item = "";
            txtDirMenuName.Text = "--Select--";
            cblDirMenuName.Items.Clear();
            string hostelcode = "";
            for (int i = 0; i < cblDirSes.Items.Count; i++)
            {
                if (cblDirSes.Items[i].Selected == true)
                {
                    if (item == "")
                    {
                        item = "" + cblDirSes.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        item = item + "'" + "," + "'" + cblDirSes.Items[i].Value.ToString() + "";
                    }
                }
            }
            hostelcode = Convert.ToString(ddlDirMessName.SelectedItem.Value);
            if (item.Trim() != "")
            {
                string firstdate = Convert.ToString(txtDirDt.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                if (item.Trim() != "" && hostelcode.Trim() != "")
                {
                    string menuquery = "";
                    ds.Clear();
                    menuquery = "select MenuMasterFK,SessionMasterFK from HT_MenuSchedule where SessionMasterFK in('" + item + "') and MessMasterFK in('" + hostelcode + "')  and ScheudleItemType='1' and ScheduleType ='1' and MenuScheduleDate ='" + dt.ToString("MM/dd/yyyy") + "'";
                    menuquery = menuquery + "  select MenuMasterFK,SessionMasterFK from HT_MenuSchedule where SessionMasterFK in('" + item + "') and MessMasterFK in('" + hostelcode + "')  and ScheudleItemType='1' and ScheduleType ='2' and MenuScheduleday ='" + dt.ToString("dddd") + "'";
                    ds = d2.select_method_wo_parameter(menuquery, "Text");
                    string menucode = "";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                        {
                            string mcode = Convert.ToString(ds.Tables[0].Rows[k]["MenuMasterFK"]);
                            if (menucode.Contains(mcode) == false)
                            {
                                if (menucode == "")
                                {
                                    menucode = mcode;
                                }
                                else
                                {
                                    menucode = menucode + "'" + "," + "'" + mcode;
                                }
                            }
                        }
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int k = 0; k < ds.Tables[1].Rows.Count; k++)
                        {
                            string mcode = Convert.ToString(ds.Tables[1].Rows[k]["MenuMasterFK"]);
                            if (menucode.Contains(mcode) == false)
                            {
                                if (menucode == "")
                                {
                                    menucode = mcode;
                                }
                                else
                                {
                                    menucode = menucode + "'" + "," + "'" + mcode;
                                }
                            }
                        }
                    }
                    //}
                    string menutype = "";
                    for (int i = 0; i < cblDirMenu.Items.Count; i++)
                    {
                        if (cblDirMenu.Items[i].Selected == true)
                        {
                            if (menutype == "")
                            {
                                menutype = "" + cblDirMenu.Items[i].Value.ToString() + "";
                            }
                            else
                            {
                                menutype = menutype + "'" + "," + "'" + cblDirMenu.Items[i].Value.ToString() + "";
                            }
                        }
                    }
                    if (menutype.Trim() == "")
                    {
                        menutype = "2";
                    }
                    string deptquery = "select distinct MenuMasterPK,MenuName,MenuCode  from HM_MenuMaster where CollegeCode ='" + collegecode1 + "' and MenuMasterPK in('" + menucode + "') and MenuType in('" + menutype + "')  order by MenuName ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(deptquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cblDirMenuName.DataSource = ds;
                        cblDirMenuName.DataTextField = "MenuName";
                        cblDirMenuName.DataValueField = "MenuMasterPK";
                        cblDirMenuName.DataBind();
                        if (cblDirMenuName.Items.Count > 0)
                        {
                            for (int i = 0; i < cblDirMenuName.Items.Count; i++)
                            {
                                cblDirMenuName.Items[i].Selected = true;
                            }
                            txtDirMenuName.Text = "Menu Name(" + cblDirMenuName.Items.Count + ")";
                        }
                    }
                }
            }
        }
        catch { }
    }
    protected void loadhostelname()
    {
        try
        {
            ddlDirMessName.Items.Clear();
            ds.Clear();
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlDirMessName.DataSource = ds;
                ddlDirMessName.DataTextField = "MessName";
                ddlDirMessName.DataValueField = "MessMasterPK";
                ddlDirMessName.DataBind();
            }
            else
            {
            }
        }
        catch { }
    }
    protected void lnkdirconsume_onclick(object sender, EventArgs e)
    {
        rdb_dirmenuconssum.Checked = true;
        rdb_dircleanconssum.Checked = false;
        dirDiv.Visible = true;
        txtDirDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
        for (int i = 0; i < cblDirMenu.Items.Count; i++)
        {
            cblDirMenu.Items[i].Selected = true;
        }
        txtDirMenu.Text = "Menu Type(" + cblDirMenu.Items.Count + ")";
        cbDirMenu.Checked = true;
        loadhostelname();
        loadsession();
        bindmenuname();
        lblDirErr1.Visible = false;
        FpSpreadDirConsume.Visible = false;
        divDirrPrint.Visible = false;
    }
    protected void imagebtndirDivclose_Click(object sender, EventArgs e)
    {
        dirDiv.Visible = false;
    }
    protected void txtDirDt_Change(object sender, EventArgs e)
    {
        loadsession();
        bindmenuname();
    }
    protected void ddlDirMessName_SelectedIndexChange(object sender, EventArgs e)
    {
        loadsession();
        bindmenuname();
    }
    protected void cbDirSes_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cbDirSes, cblDirSes, txtDirSes, "Session Name");
        directinvward();
    }
    protected void cblDirSes_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cbDirSes, cblDirSes, txtDirSes, "Session Name");
        directinvward();
    }
    protected void cbDirMenu_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cbDirMenu, cblDirMenu, txtDirMenu, "Menu Type");
        bindmenuname();
    }
    protected void cblDirMenu_SelectIndexChange(object sender, EventArgs e)
    {
        chklstchange(cbDirMenu, cblDirMenu, txtDirMenu, "Menu Type");
        bindmenuname();
    }
    protected void cbDirMenuName_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cbDirMenuName, cblDirMenuName, txtDirMenuName, "Menu Name");
    }
    protected void cblDirMenuName_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cbDirMenuName, cblDirMenuName, txtDirMenuName, "Menu Name");
    }
    private void Common_Command(FarPoint.Web.Spread.FpSpread FpSpread, int column)
    {
        try
        {
            string actrow = FpSpread.Sheets[0].ActiveRow.ToString();
            string actcol = FpSpread.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == Convert.ToString(column))
            {
                if (FpSpread.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(FpSpread.Sheets[0].Cells[0, column].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < FpSpread.Sheets[0].RowCount; i++)
                        {
                            FpSpread.Sheets[0].Cells[i, column].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < FpSpread.Sheets[0].RowCount; i++)
                        {
                            FpSpread.Sheets[0].Cells[i, column].Value = 0;
                        }
                    }
                }
            }
        }
        catch { }
    }
    protected void FpSpreadDirConsume_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        Common_Command(FpSpreadDirConsume, 7);
    }
    protected void btnDirExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtDirExcel.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpreadDirConsume, reportname);
                lblDirNorec.Visible = false;
            }
            else
            {
                lblDirNorec.Text = "Please Enter Your Report Name";
                lblDirNorec.Visible = true;
                txtDirExcel.Focus();
            }
        }
        catch { }
    }
    protected void btnDirPrint_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Direct Daily Consumption Status Report";
            string pagename = "inv_Daily_consumption.aspx";
            Printmaster1.loadspreaddetails(FpSpreadDirConsume, pagename, degreedetails);
            Printmaster1.Visible = true;
        }
        catch { }
    }
    protected void btnDirSave_Click(object sender, EventArgs e)
    {
        try
        {
            bool checkdailyconsum = false;
            FpSpreadDirConsume.SaveChanges();
            Hashtable hnew = new Hashtable();
            string firstdate = Convert.ToString(txtDirDt.Text);
            DateTime dt = new DateTime();
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            string dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            ArrayList additemcode = new ArrayList();
            if (rdb_dirmenuconssum.Checked == true)
            {
                #region dir menu item
                if (FpSpreadDirConsume.Sheets[0].RowCount > 0)
                {
                    bool testflage = false;
                    bool finaltest = false;
                    bool fin = false;
                    string totalvalue = "";
                    bool Negtive = false;
                    Hashtable needarray = new Hashtable();
                    for (int i = 1; i < FpSpreadDirConsume.Sheets[0].RowCount; i++)
                    {
                        int checkval = Convert.ToInt32(FpSpreadDirConsume.Sheets[0].Cells[i, 7].Value);
                        if (checkval == 1)
                        {
                            if (!additemcode.Contains(Convert.ToString(FpSpreadDirConsume.Sheets[0].Cells[i, 1].Tag)))
                            {
                                additemcode.Add(Convert.ToString(FpSpreadDirConsume.Sheets[0].Cells[i, 1].Tag));
                            }
                            totalvalue = Convert.ToString(FpSpreadDirConsume.Sheets[0].Cells[i, 5].Text);
                            fin = true;
                            double newhandvalue = 0;
                            double newgetvalue = 0;
                            string handvalue = Convert.ToString(FpSpreadDirConsume.Sheets[0].Cells[i, 4].Text);
                            string getvalue = Convert.ToString(FpSpreadDirConsume.Sheets[0].Cells[i, 6].Text);
                            if (getvalue.Trim() == "")
                            {
                                getvalue = "0";
                            }
                            Hashtable itemcoderepect1 = (Hashtable)Session["itemDircoderepect"];
                            if (itemcoderepect1.Count > 0)
                            {
                                newgetvalue = Convert.ToDouble(getvalue);
                                double row = 0;
                                string count = Convert.ToString(itemcoderepect1[Convert.ToString(FpSpreadDirConsume.Sheets[0].Cells[i, 1].Tag)]);
                                double.TryParse(count, out row);
                                double avgitemrequired = Convert.ToDouble(getvalue) / row;
                                getvalue = Convert.ToString(avgitemrequired);
                            }
                            if (!needarray.Contains(Convert.ToString(FpSpreadDirConsume.Sheets[0].Cells[i, 1].Tag)))
                            {
                                needarray.Add(Convert.ToString(FpSpreadDirConsume.Sheets[0].Cells[i, 1].Tag), Convert.ToString(getvalue));
                            }
                            if (handvalue.Trim() != "")
                            {
                                newhandvalue = Convert.ToDouble(handvalue);
                            }
                            if (newhandvalue < newgetvalue)
                            {
                                Negtive = true;
                            }
                        }
                    }
                    if (fin == true && Negtive == false)
                    {
                        Hashtable sessArray = (Hashtable)Session["AddDirArrayValue"];
                        if (sessArray.Count > 0)
                        {
                            foreach (DictionaryEntry pr in sessArray)
                            {
                                string key = Convert.ToString(pr.Key);
                                string keyvalue = Convert.ToString(pr.Value);
                                string[] splitfirst = key.Split('-');
                                if (splitfirst.Length > 0)
                                {
                                    string hostel = Convert.ToString(splitfirst[0]);
                                    string menucode = Convert.ToString(splitfirst[1]);
                                    string session = Convert.ToString(splitfirst[2]);
                                    string[] splitnew = keyvalue.Split('/');
                                    splitnew = splitnew[0].Split('-');
                                    string total1 = Convert.ToString(splitnew[4]);
                                    int mastercode = 0;
                                    string getcode = "";
                                    string inserquery = "";
                                    getcode = d2.GetFunction("select DailyConsumptionMasterPK from  HT_DailyConsumptionMaster where MessMasterFK in('" + hostel + "') and DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' and ForMess='1'");   //SessionFK ='" + session + "' and  and MenumasterFK='" + menucode + "'
                                    if (getcode.Trim() != "" && getcode.Trim() != "0")
                                    {
                                        mastercode = Convert.ToInt32(getcode);
                                        inserquery = "update HT_DailyConsumptionMaster set Total_Present='" + total1 + "' where  MessMasterFK ='" + hostel + "' and DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' and ForMess='1'";  //SessionFK ='" + session + "' and   and MenumasterFK='" + menucode + "'
                                    }
                                    else
                                    {
                                        getcode = d2.GetFunction("select distinct top 1 DailyConsumptionMasterPK  from  HT_DailyConsumptionMaster  order by DailyConsumptionMasterPK desc");
                                        if (getcode.Trim() != "" && getcode.Trim() != "0")
                                        {
                                            mastercode = Convert.ToInt32(getcode) + 1;
                                        }
                                        else
                                        {
                                            mastercode = 1;
                                        }
                                        inserquery = "insert into HT_DailyConsumptionMaster  (DailyConsDate,ForMess,MessMasterFK,UserCode,Total_Present)values('" + dt.ToString("MM/dd/yyyy") + "','1','" + hostel + "','" + usercode + "','" + total1 + "')";   //DeptFK, '" + hostel + "', SessionFK,MenumasterFK   ,'" + session + "'   ,'" + menucode + "'
                                    }
                                    int firstinsert = d2.update_method_wo_parameter(inserquery, "Text");
                                    if (firstinsert != 0)
                                    {
                                        splitfirst = keyvalue.Split('/');
                                        if (splitfirst.Length > 0)
                                        {
                                            for (int rs = 0; rs <= splitfirst.GetUpperBound(0); rs++)
                                            {
                                                string[] secondsplit = splitfirst[rs].Split('-');
                                                if (secondsplit.Length > 0)
                                                {
                                                    string day = Convert.ToString(secondsplit[0]);
                                                    string hostel1 = Convert.ToString(secondsplit[1]);
                                                    string menucode1 = Convert.ToString(secondsplit[2]);
                                                    string session1 = Convert.ToString(secondsplit[3]);
                                                    string total = Convert.ToString(secondsplit[4]);
                                                    string needquantity = Convert.ToString(secondsplit[5]);
                                                    string noofpersion = Convert.ToString(secondsplit[6]);
                                                    string itemcode = Convert.ToString(secondsplit[7]);
                                                    string rpu = Convert.ToString(secondsplit[8]);
                                                    if (additemcode.Contains(Convert.ToString(itemcode)) == true)
                                                    {
                                                        string handquantity = d2.GetFunction("select IssuedQty-isnull(UsedQty,'0')as BalQty from IT_StockDeptDetail where ItemFK='" + itemcode + "' and DeptFK='" + hostel1 + "'");
                                                        if (noofpersion.Trim() == "")
                                                        {
                                                            noofpersion = "0";
                                                        }
                                                        if (total.Trim() == "")
                                                        {
                                                            total = "0";
                                                        }
                                                        string requiredquantity = "";
                                                        double consumvalue = 0;
                                                        double Need = 0;
                                                        double valueamt = Convert.ToDouble(total) / Convert.ToDouble(noofpersion) * Convert.ToDouble(needquantity);
                                                        if (valueamt != 0)
                                                        {
                                                            Need = Need + valueamt;
                                                        }
                                                        requiredquantity = Convert.ToString(needarray[Convert.ToString(itemcode)]);
                                                        if (requiredquantity.Trim() != "" && requiredquantity.Trim() != "0")
                                                        {
                                                            if (rpu.Trim() != "" && requiredquantity.Trim() != "")
                                                            {
                                                                consumvalue = Convert.ToDouble(rpu) * Convert.ToDouble(requiredquantity);
                                                            }
                                                            string DailyConsumptionMasterPK = d2.GetFunction("select DailyConsumptionMasterPK from HT_DailyConsumptionMaster where DailyConsDate='" + dt.ToString("MM/dd/yyyy") + "' and MessMasterFK ='" + hostel + "' and ForMess='1'");   //and SessionFK='" + session + "' and MenumasterFK='" + menucode + "'
                                                            string newinsert = " if exists(select * from HT_DailyConsumptionDetail  where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "') update HT_DailyConsumptionDetail set ConsumptionQty=ConsumptionQty+ ISNULL('" + requiredquantity + "',0),RPU='" + rpu + "' where DailyConsumptionMasterFK='" + DailyConsumptionMasterPK + "' and ItemFK='" + itemcode + "' else insert into HT_DailyConsumptionDetail (DailyConsumptionMasterFK,ConsumptionQty,RPU,ItemFK)values('" + DailyConsumptionMasterPK + "','" + requiredquantity + "','" + rpu + "','" + itemcode + "')";
                                                            int inst = d2.update_method_wo_parameter(newinsert, "Text");
                                                            if (inst != 0)
                                                            {
                                                                finaltest = true;
                                                            }
                                                            string getvalue = "";
                                                            if (handquantity.Trim() != "" && handquantity.Trim() != "0")
                                                            {
                                                                double inval = Convert.ToDouble(handquantity);
                                                                if (inval >= Convert.ToDouble(requiredquantity))
                                                                {
                                                                    inval = inval - Convert.ToDouble(requiredquantity);
                                                                    getvalue = Convert.ToString(inval);
                                                                }
                                                                else
                                                                {
                                                                    getvalue = "0";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                getvalue = "0";
                                                            }
                                                            if (requiredquantity.Trim() == "")
                                                            {
                                                                requiredquantity = "0";
                                                            }
                                                            string stockvalue = "if exists(select * from IT_StockDeptDetail where ItemFK='" + itemcode + "' and DeptFK='" + ddlDirMessName.SelectedItem.Value + "') update IT_StockDeptDetail set ItemFK ='" + itemcode + "',BalQty='" + getvalue + "',usedqty=ISNULL(usedqty,0)+ISNULL('" + requiredquantity + "',0),IssuedRPU ='" + rpu + "' where ItemFK='" + itemcode + "' and DeptFK='" + ddlDirMessName.SelectedItem.Value + "'";
                                                            int in_s = d2.update_method_wo_parameter(stockvalue, "Text");
                                                            if (in_s != 0)
                                                            {
                                                                testflage = true;
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
                    }
                    else
                    {
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Item in Hand Very Low";
                        alertmessage.Visible = true;
                    }
                    if (fin == false)
                    {
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Please Select Any one Items";
                        alertmessage.Visible = true;
                    }
                    if (finaltest == true && testflage == true)
                    {
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Saved Successfully";
                        alertmessage.Visible = true;
                    }
                }
                #endregion
            }
            else
            {
                #region cleaning item
                bool checktemp = false;
                bool savecheck = false;
                bool insert = false;
                bool Negtive = false;
                if (FpSpreadDirConsume.Sheets[0].RowCount > 0)
                {
                    for (int row = 1; row < FpSpreadDirConsume.Sheets[0].RowCount; row++)
                    {
                        int checkval = Convert.ToInt32(FpSpreadDirConsume.Sheets[0].Cells[row, 7].Value);
                        if (checkval == 1)
                        {
                            checktemp = true;
                            double newhandvalue = 0;
                            double newgetvalue = 0;
                            string handvalue = Convert.ToString(FpSpreadDirConsume.Sheets[0].Cells[row, 4].Text);
                            string getvalue = Convert.ToString(FpSpreadDirConsume.Sheets[0].Cells[row, 6].Text);
                            if (handvalue.Trim() != "")
                            {
                                newhandvalue = Convert.ToDouble(handvalue);
                            }
                            if (getvalue.Trim() != "")
                            {
                                newgetvalue = Convert.ToDouble(getvalue);
                            }
                            if (newhandvalue < newgetvalue)
                            {
                                Negtive = true;
                            }
                        }
                    }
                    if (checktemp == true && Negtive == false)
                    {
                        int mastercode = 0;
                        string getcode = "";
                        string inserquery = "";
                        getcode = d2.GetFunction("select distinct top 1 DailyConsumptionMasterPK  from  HT_DailyConsumptionMaster where DailyConsDate='" + dt.ToString("MM/dd/yyyy") + "' and ForMess='1' order by DailyConsumptionMasterPK desc");
                        if (getcode.Trim() != "" && getcode.Trim() != "0")
                        {
                            mastercode = Convert.ToInt32(getcode);
                            inserquery = "update HT_DailyConsumptionMaster set DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' where DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' and ForMess='1'";
                        }
                        else
                        {
                            getcode = d2.GetFunction("select distinct top 1 DailyConsumptionMasterPK  from  HT_DailyConsumptionMaster  order by DailyConsumptionMasterPK desc");
                            if (getcode.Trim() != "" && getcode.Trim() != "0")
                            {
                                mastercode = Convert.ToInt32(getcode) + 1;
                            }
                            else
                            {
                                mastercode = 1;
                            }
                            inserquery = "insert into HT_DailyConsumptionMaster (DailyConsDate,ForMess,MessMasterFK,DeptFK)values('" + dt.ToString("MM/dd/yyyy") + "','1','" + ddl_hostelname.SelectedItem.Value + "','" + ddl_hostelname.SelectedItem.Value + "')";
                        }
                        int firstinsert = d2.update_method_wo_parameter(inserquery, "Text");
                        if (mastercode != 0)
                        {
                            if (firstinsert != 0)
                            {
                                if (FpSpreadDirConsume.Sheets[0].RowCount > 0)
                                {
                                    for (int row = 1; row < FpSpreadDirConsume.Sheets[0].RowCount; row++)
                                    {
                                        int checkval = Convert.ToInt32(FpSpreadDirConsume.Sheets[0].Cells[row, 7].Value);
                                        if (checkval == 1)
                                        {
                                            string itemcode = Convert.ToString(FpSpreadDirConsume.Sheets[0].Cells[row, 1].Tag);
                                            string handquantity = Convert.ToString(FpSpreadDirConsume.Sheets[0].Cells[row, 4].Text);
                                            string requirquantity = Convert.ToString(FpSpreadDirConsume.Sheets[0].Cells[row, 6].Text);
                                            if (requirquantity.Trim() == "")
                                                requirquantity = "0";
                                            string rpuunit = Convert.ToString(FpSpreadDirConsume.Sheets[0].Cells[row, 3].Tag);
                                            double rpu = 0;
                                            double conusume = 0;
                                            if (rpuunit.Trim() != "")
                                            {
                                                rpu = Convert.ToDouble(rpuunit);
                                                conusume = Convert.ToDouble(requirquantity) * Convert.ToDouble(rpu);
                                            }
                                            string newinsert = " if exists(select * from HT_DailyConsumptionDetail  where DailyConsumptionMasterFK='" + mastercode + "' and ItemFK='" + itemcode + "')update HT_DailyConsumptionDetail set ConsumptionQty=ConsumptionQty+ ISNULL('" + requirquantity + "',0),RPU='" + rpu + "' where DailyConsumptionMasterFK='" + mastercode + "' and ItemFK='" + itemcode + "' else insert into HT_DailyConsumptionDetail (ConsumptionQty,RPU,ItemFK,DailyConsumptionMasterFK)values('" + requirquantity + "','" + rpu + "','" + itemcode + "','" + mastercode + "')";
                                            int inst = d2.update_method_wo_parameter(newinsert, "Text");
                                            if (inst != 0)
                                            {
                                                savecheck = true;
                                            }
                                            string getvalue = "";
                                            if (handquantity.Trim() != "" && handquantity.Trim() != "0")
                                            {
                                                double inval = Convert.ToDouble(handquantity);
                                                if (inval >= Convert.ToDouble(requirquantity))
                                                {
                                                    inval = inval - Convert.ToDouble(requirquantity);
                                                    getvalue = Convert.ToString(inval);
                                                }
                                                else
                                                {
                                                    getvalue = "0";
                                                }
                                            }
                                            else
                                            {
                                                getvalue = "0";
                                            }
                                            if (requirquantity.Trim() == "")
                                            {
                                                requirquantity = "0";
                                            }
                                            string stockvalue = "if exists(select*from IT_StockDeptDetail where ItemFK='" + itemcode + "' and DeptFK='" + ddl_hostelname.SelectedItem.Value + "')update IT_StockDeptDetail set ItemFK ='" + itemcode + "',BalQty='" + getvalue + "',usedqty=ISNULL(usedqty,0)+ISNULL('" + requirquantity + "',0),IssuedRPU ='" + rpu + "' where ItemFK='" + itemcode + "' and DeptFK='" + ddl_hostelname.SelectedItem.Value + "'";
                                            int in_s = d2.update_method_wo_parameter(stockvalue, "Text");
                                            if (in_s != 0)
                                            {
                                                insert = true;
                                            }
                                        }
                                    }
                                }
                                if (savecheck == true && insert == true)
                                {
                                    btnDirGo_Click(sender, e);
                                    lbl_alerterror.Visible = true;
                                    lbl_alerterror.Text = "Saved Successfully";
                                    alertmessage.Visible = true;
                                }
                                else
                                {
                                    lbl_alerterror.Visible = true;
                                    lbl_alerterror.Text = "Please Select Any one Items";
                                    alertmessage.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            lbl_alerterror.Visible = true;
                            lbl_alerterror.Text = "Please Select Any one Items";
                            alertmessage.Visible = true;
                        }
                    }
                    else
                    {
                        if (Negtive == true)
                        {
                            lbl_alerterror.Visible = true;
                            lbl_alerterror.Text = "Item in Hand Very Low";
                            alertmessage.Visible = true;
                        }
                        else
                        {
                            lbl_alerterror.Visible = true;
                            lbl_alerterror.Text = "Please Select Any one Items";
                            alertmessage.Visible = true;
                        }
                    }
                }
                #endregion
            }
        }
        catch { }
    }
    protected void btnDirGo_Click(object sender, EventArgs e)
    {
        try
        {
            Printmaster1.Visible = false;
            string itemheadercode = "";
            string hostelcode = "";
            string menuvalue = "";
            Printcontrol.Visible = false;
            itemheadercode = GetSelectedItemsValueAsString(cblDirSes);
            menuvalue = GetSelectedItemsValueAsString(cblDirMenuName);
            hostelcode = Convert.ToString(ddlDirMessName.SelectedItem.Value);
            string firstdate = Convert.ToString(txtDirDt.Text);
            DateTime dt = new DateTime();
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            DataView dv = new DataView();
            DataView dv1 = new DataView();
            ArrayList Addvalue = new ArrayList();
            Hashtable hashset = new Hashtable();
            FpSpreadDirConsume.SaveChanges();
            if (itemheadercode.Trim() != "" && hostelcode.Trim() != "" && menuvalue.Trim() != "")
            {
                string selectquery = "";
                FpSpreadDirConsume.Sheets[0].RowCount = 0;
                FpSpreadDirConsume.Sheets[0].ColumnCount = 0;
                FpSpreadDirConsume.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpreadDirConsume.CommandBar.Visible = false;
                FpSpreadDirConsume.Sheets[0].ColumnCount = 8;
                FpSpreadDirConsume.Sheets[0].AutoPostBack = false;
                FpSpreadDirConsume.Sheets[0].RowHeader.Visible = false;
                FpSpreadDirConsume.Width = 866;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                FpSpreadDirConsume.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpreadDirConsume.Columns[0].Width = 50;
                FpSpreadDirConsume.Columns[0].Locked = true;
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Item Code";
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpreadDirConsume.Columns[1].Width = 100;
                FpSpreadDirConsume.Columns[1].Locked = true;
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Name";
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpreadDirConsume.Columns[2].Width = 200;
                FpSpreadDirConsume.Columns[2].Locked = true;
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Item Measure";
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpSpreadDirConsume.Columns[3].Width = 100;
                FpSpreadDirConsume.Columns[3].Locked = true;
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Item in Hand";
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                FpSpreadDirConsume.Columns[4].Width = 100;
                FpSpreadDirConsume.Columns[4].Locked = true;
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Total Strength";
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                FpSpreadDirConsume.Columns[5].Width = 125;
                FpSpreadDirConsume.Columns[5].Locked = true;
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Required Quantity";
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                FpSpreadDirConsume.Columns[6].Width = 125;
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Select";
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                FpSpreadDirConsume.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                FpSpreadDirConsume.Columns[7].Width = 50;
                FpSpreadDirConsume.Columns[7].Visible = true;
                FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                cb.AutoPostBack = true;
                FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                cb1.AutoPostBack = false;
                FarPoint.Web.Spread.DoubleCellType db1 = new FarPoint.Web.Spread.DoubleCellType();
                db1.ErrorMessage = "Enter Only Number";
                FpSpreadDirConsume.Sheets[0].RowCount++;
                FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 7].CellType = cb;
                if (rdb_dirmenuconssum.Checked == true)
                {
                    #region menuconsump
                    selectquery = "  select i.ItemCode,d.ItemFK,itemname,IssuedQty-ISNULL(UsedQty,'0')  as hand_qty,i.itemunit,s.IssuedRPU as Rpu,i.ItemPK from HM_MenuItemDetail d, HM_MenuItemMaster m,HM_MenuMaster u,IM_ItemMaster i,HM_MessMaster h,IT_StockDeptDetail s WHERE D.MenuItemMasterFK =M.MenuItemMasterPK and i.ItemPK=d.ItemFK and m.MenuMasterFK=u.MenuMasterPK and s.ItemFK=i.ItemPK and s.ItemFK=d.ItemFK and s.DeptFK = h.MessMasterPK and s.DeptFK in('" + hostelcode + "') and u.MenuMasterPK in ('" + menuvalue + "') and h.MessMasterPK=m.MessMasterFK group by i.ItemCode,d.ItemFK,itemname,IssuedQty,UsedQty,i.itemunit,IssuedRPU,itempk";
                    selectquery = selectquery + "   select MenuScheduleday,MenuMasterFK,change_strength, MessMasterFK ,SessionMasterFK,MenuScheduleDate,ScheduleType from HT_MenuSchedule where ScheudleItemType='1' ";
                    selectquery = selectquery + "   select  i.ItemCode,i.itempk,itemname,i.itemunit,BalQty as hand_qty, m.NoOfPerson,mi.NeededQty,mu.MenuMasterPK,MessMasterPK  from HM_MenuItemMaster m ,HM_MenuItemDetail mi,IM_ItemMaster i,IT_StockDeptDetail s,HM_MenuMaster mu,HM_MessMaster h where m.MenuItemMasterPK  =Mi.MenuItemMasterFK and mi.ItemFK =i.ItemPK and mi.ItemFK=s.ItemFK and m.MenuMasterFK=mu.MenuMasterPK and m.MenuMasterFK=mu.MenuMasterPK and h.MessMasterPK in ('" + hostelcode + "') and mu.MenuMasterPK in ('" + menuvalue + "') and s.DeptFK = h.MessMasterPK and h.MessMasterPK=m.MessMasterFK";
                    selectquery = selectquery + "   select NeededQty,NoOfPerson ,ItemFK,MenuMasterFK from HM_MenuItemDetail m,HM_MenuItemMaster h where h.MenuItemMasterPK =m.MenuItemMasterFK  and MenuMasterFK in ('" + menuvalue + "') and h.MessMasterFK in('" + hostelcode + "')";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpreadDirConsume.Sheets[0].RowCount++;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["itempk"]);
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemName"]);
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["itemunit"]);
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["rpu"]);
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["hand_qty"]);
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 4].ForeColor = Color.Blue;
                            string handquntiy = Convert.ToString(ds.Tables[0].Rows[i]["hand_qty"]);
                            double hand = 0;
                            if (handquntiy.Trim() != "")
                            {
                                hand = Convert.ToDouble(handquntiy);
                            }
                            int strength = 0;
                            double Need = 0;
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                ds.Tables[2].DefaultView.RowFilter = "itemcode='" + Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]) + "'";
                                dv = ds.Tables[2].DefaultView;
                                if (dv.Count > 0)
                                {
                                    for (int ro = 0; ro < dv.Count; ro++)
                                    {
                                        strength = 0;
                                        if (ds.Tables[1].Rows.Count > 0)
                                        {
                                            for (int ik = 0; ik < cblDirSes.Items.Count; ik++)
                                            {
                                                if (cblDirSes.Items[ik].Selected == true)
                                                {


                                                    ds.Tables[1].DefaultView.RowFilter = "MenuScheduleDate='" + dt.ToString("MM/dd/yyyy") + "' and ScheduleType='1' and MessMasterFK ='" + Convert.ToString(dv[ro]["MessMasterPK"]) + "' and SessionMasterFK='" + Convert.ToString(cblDirSes.Items[ik].Value) + "' and MenuMasterFK in('" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "')";
                                                    dv1 = ds.Tables[1].DefaultView;
                                                    if (dv1.Count == 0)
                                                    {
                                                        ds.Tables[1].DefaultView.RowFilter = "MenuScheduleday='" + dt.ToString("dddd") + "' and ScheduleType='2' and MessMasterFK ='" + Convert.ToString(dv[ro]["MessMasterPK"]) + "' and SessionMasterFK='" + Convert.ToString(cblDirSes.Items[ik].Value) + "' and MenuMasterFK in('" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "')";
                                                       
                                                        dv1 = ds.Tables[1].DefaultView;
                                                    }
                                                    //ds.Tables[1].DefaultView.RowFilter = "MenuScheduleday='" + dt.ToString("dddd") + "' and MessMasterFK ='" + Convert.ToString(dv[ro]["MessMasterPK"]) + "' and SessionMasterFK='" + Convert.ToString(cblDirSes.Items[ik].Value) + "' and MenuMasterFK in('" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "')";
                                                    //dv1 = ds.Tables[1].DefaultView;
                                                    if (dv1.Count > 0)
                                                    {
                                                        string total = Convert.ToString(dv1[0]["change_strength"]);
                                                        if (total.Trim() != "")
                                                        {
                                                            strength = strength + Convert.ToInt32(total);
                                                            string Needquantiy = ""; string noofpersion = "";
                                                            if (ds.Tables[3].Rows.Count > 0)
                                                            {
                                                                ds.Tables[3].DefaultView.RowFilter = "ItemFK='" + Convert.ToString(ds.Tables[0].Rows[i]["ItemFK"]) + "' and MenuMasterFK='" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "'";
                                                                DataView dv3 = ds.Tables[3].DefaultView;
                                                                if (dv3.Count > 0)
                                                                {
                                                                    Needquantiy = Convert.ToString(dv3[0]["NeededQty"]);
                                                                    noofpersion = Convert.ToString(dv3[0]["NoOfPerson"]);
                                                                }
                                                            }
                                                            if (noofpersion.Trim() != "" && Needquantiy.Trim() != "" && noofpersion.Trim() != "0" && Needquantiy.Trim() != "0")
                                                            {
                                                                double valueamt = Convert.ToDouble(total) / Convert.ToDouble(noofpersion) * Convert.ToDouble(Needquantiy);
                                                                if (valueamt != 0)
                                                                {
                                                                    Need = Need + valueamt;
                                                                }
                                                                if (!hashset.Contains(Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "-" + Convert.ToString(cbl_session.Items[ik].Value)))
                                                                {
                                                                    hashset.Add(Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "-" + Convert.ToString(cblDirSes.Items[ik].Value), dt.ToString("dddd") + "-" + Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "-" + Convert.ToString(cblDirSes.Items[ik].Value) + "-" + total + "-" + Needquantiy + "-" + noofpersion + "-" + Convert.ToString(ds.Tables[0].Rows[i]["ItemFK"]) + "-" + Convert.ToString(ds.Tables[0].Rows[i]["rpu"]));
                                                                }
                                                                else
                                                                {
                                                                    string getvalue = Convert.ToString(hashset[Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "-" + Convert.ToString(cblDirSes.Items[ik].Value)]);
                                                                    if (getvalue.Trim() != "")
                                                                    {
                                                                        hashset.Remove(Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "-" + Convert.ToString(cblDirSes.Items[ik].Value));
                                                                        getvalue = getvalue + "/" + dt.ToString("dddd") + "-" + Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "-" + Convert.ToString(cblDirSes.Items[ik].Value) + "-" + total + "-" + Needquantiy + "-" + noofpersion + "-" + Convert.ToString(ds.Tables[0].Rows[i]["ItemFK"]) + "-" + Convert.ToString(ds.Tables[0].Rows[i]["rpu"]);
                                                                        hashset.Add(Convert.ToString(dv[ro]["MessMasterPK"]) + "-" + Convert.ToString(dv[ro]["MenuMasterPK"]) + "-" + Convert.ToString(cblDirSes.Items[ik].Value), getvalue);
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
                            }
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(strength);
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 6].Text = "";
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 6].CellType = db1;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 6].BackColor = Color.LightYellow;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 7].CellType = cb1;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        }
                        FpSpreadDirConsume.Sheets[0].PageSize = FpSpreadDirConsume.Sheets[0].RowCount;
                        FpSpreadDirConsume.Sheets[0].FrozenRowCount = 0;
                        FpSpreadDirConsume.Visible = true;
                        divDirrPrint.Visible = true;
                        lblDirErr1.Visible = false;
                        btnDirSave.Visible = true;
                        Session["AddDirArrayValue"] = hashset;
                        if (hashset.Count > 0)
                        {
                            int itemcount = 0;
                            foreach (DictionaryEntry pr1 in hashset)
                            {
                                string value = Convert.ToString(pr1.Value);
                                string[] splitnew = value.Split('/');
                                if (splitnew.Length > 0)
                                {
                                    for (int j = 0; j < splitnew.Length; j++)
                                    {
                                        string[] splitfirst1 = splitnew[j].Split('-');
                                        if (!itemcoderepect.ContainsKey(Convert.ToString(splitfirst1[7])))
                                        {
                                            itemcount = 1;
                                            itemcoderepect.Add(Convert.ToString(splitfirst1[7]), Convert.ToString(itemcount));
                                        }
                                        else
                                        {
                                            int NewVal = Convert.ToInt32(itemcoderepect[Convert.ToString(splitfirst1[7])]);
                                            itemcount = NewVal + 1;
                                            itemcoderepect.Remove(splitfirst1[7]);
                                            itemcoderepect.Add(Convert.ToString(splitfirst1[7]), Convert.ToString(itemcount));
                                        }
                                    }
                                }
                            }
                            Session["itemDircoderepect"] = itemcoderepect;
                        }
                    }
                    else
                    {
                        lblDirErr1.Visible = true;
                        lblDirErr1.Text = "No Record Found";
                        FpSpreadDirConsume.Visible = false;
                        divDirrPrint.Visible = false;
                        btnDirSave.Visible = false;
                    }
                    #endregion
                }
                if (rdb_dircleanconssum.Checked == true)
                {
                    #region cleaning item
                    selectquery = "select i.ItemCode,i.itemname,i.itempk,i.itemunit, s.IssuedQty-isnull(UsedQty,'0')as BalQty, s.IssuedRPU from Cleaning_ItemMaseter cm,Cleaning_ItemDetailMaster cd,IM_ItemMaster i ,IT_StockDeptDetail s where cm.Clean_ItemMasterPK=cd.Clean_ItemMasterFK and cd.Itemfk=i.ItemPK and s.ItemFK=i.ItemPK and s.ItemFK=cd.Itemfk and cm.MessMasterFK in('" + hostelcode + "') and cm.SessionFK in('" + itemheadercode + "') and cd.Itemfk in('" + menuvalue + "') and s.DeptFK in('" + hostelcode + "')  and s.DeptFK = cm.MessMasterFK group by i.ItemCode,i.itemname,i.itempk,i.itemunit, s.IssuedQty,UsedQty, s.IssuedRPU";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FpSpreadDirConsume.Columns[5].Visible = false;
                        FpSpreadDirConsume.Width = 742;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpreadDirConsume.Sheets[0].RowCount++;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["itempk"]);
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemName"]);
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["itemunit"]);
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["IssuedRPU"]);
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["BalQty"]);
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 4].ForeColor = Color.Blue;
                            string handquntiy = Convert.ToString(ds.Tables[0].Rows[i]["BalQty"]);
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 5].Text = Convert.ToString("");
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 6].Text = "";
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 6].CellType = db1;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 6].BackColor = Color.LightYellow;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 7].CellType = cb1;
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                            FpSpreadDirConsume.Sheets[0].Cells[FpSpreadDirConsume.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        }
                        FpSpreadDirConsume.Sheets[0].PageSize = FpSpreadDirConsume.Sheets[0].RowCount;
                        FpSpreadDirConsume.Sheets[0].FrozenRowCount = 0;
                        FpSpreadDirConsume.Visible = true;
                        divDirrPrint.Visible = true;
                        lblDirErr1.Visible = false;
                        btnDirSave.Visible = true;
                    }
                    #endregion
                }
            }
            else
            {
                lblDirErr1.Visible = true;
                lblDirErr1.Text = "Please Select All Fields";
                FpSpreadDirConsume.Visible = false;
                divDirrPrint.Visible = false;
                btnDirSave.Visible = false;
            }
        }
        catch
        {
            lblDirErr1.Visible = true;
            lblDirErr1.Text = "Please Select All Fields";
            FpSpreadDirConsume.Visible = false;
            divDirrPrint.Visible = false;
            btnDirSave.Visible = false;
        }
    }
    protected void chkchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            if (chkchange.Checked == true)
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = true;
                }
                txtchange.Text = label + "(" + Convert.ToString(chklstchange.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = false;
                }
                txtchange.Text = "--Select--";
            }
        }
        catch { }
    }
    protected void chklstchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            txtchange.Text = "--Select--";
            chkchange.Checked = false;
            int count = 0;
            for (int i = 0; i < chklstchange.Items.Count; i++)
            {
                if (chklstchange.Items[i].Selected == true)
                    count = count + 1;
            }
            if (count > 0)
            {
                txtchange.Text = label + "(" + count + ")";
                if (count == chklstchange.Items.Count)
                    chkchange.Checked = true;
            }
        }
        catch { }
    }
    private string GetSelectedItemsValueAsString(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int j = 0; j < cblSelected.Items.Count; j++)
            {
                if (cblSelected.Items[j].Selected == true)
                {
                    if (sbSelected.Length == 0)
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Value));
                    else
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[j].Value));
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    protected void rdb_dirmenuconssum_CheckedChange(object sender, EventArgs e)
    {
        txtDirMenu.Enabled = true;
        lblDirMenuName.Text = "Menu Name";
        bindmenuname();
    }
    protected void rdb_dircleanconssum_CheckedChange(object sender, EventArgs e)
    {
        txtDirMenu.Enabled = false;
        lblDirMenuName.Text = "Item Name";
        binditemdir();
    }
    public void binditemdir()
    {
        try
        {
            string sessionFk = GetSelectedItemsValueAsString(cblDirSes);
            cblDirMenuName.Items.Clear();
            txtDirMenuName.Text = "--Select--";
            string messcode = Convert.ToString(ddlDirMessName.SelectedItem.Value);
            lblDirMenuName.Text = "Item Name";
            if (sessionFk.Trim() != "")
            {
                string firstdate = Convert.ToString(txt_date.Text);
                DateTime dt = new DateTime();
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                if (sessionFk.Trim() != "" && messcode.Trim() != "")
                {
                    string menuquery = "";
                    string menucode = "";
                    menuquery = "select MenuMasterFK,SessionMasterFK from HT_MenuSchedule where SessionMasterFK in('" + sessionFk + "') and MessMasterFK in('" + messcode + "')  and ScheudleItemType='2' and ScheduleType ='1' and MenuScheduleDate ='" + dt.ToString("MM/dd/yyyy") + "'";
                    menuquery = menuquery + "  select MenuMasterFK,SessionMasterFK from HT_MenuSchedule where SessionMasterFK in('" + sessionFk + "') and MessMasterFK in('" + messcode + "')  and ScheudleItemType='2' and ScheduleType ='2' and MenuScheduleday ='" + dt.ToString("dddd") + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(menuquery, "Text");
                    menuquery = "";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                        {
                            string mcode = Convert.ToString(ds.Tables[0].Rows[k]["MenuMasterFK"]);
                            if (menucode.Contains(mcode) == false)
                            {
                                if (menucode == "")
                                {
                                    menucode = mcode;
                                }
                                else
                                {
                                    menucode = menucode + "'" + "," + "'" + mcode;
                                }
                            }
                        }
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int k = 0; k < ds.Tables[1].Rows.Count; k++)
                        {
                            string mcode = Convert.ToString(ds.Tables[1].Rows[k]["MenuMasterFK"]);
                            if (menucode.Contains(mcode) == false)
                            {
                                if (menucode == "")
                                {
                                    menucode = mcode;
                                }
                                else
                                {
                                    menucode = menucode + "'" + "," + "'" + mcode;
                                }
                            }
                        }
                    }
                    string deptquery = "select distinct ItemCode,ItemPK,itemname from IM_ItemMaster  where  itempk in('" + menucode + "')  order by ItemName";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(deptquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cblDirMenuName.DataSource = ds;
                        cblDirMenuName.DataTextField = "itemname";
                        cblDirMenuName.DataValueField = "ItemPK";
                        cblDirMenuName.DataBind();
                        if (cblDirMenuName.Items.Count > 0)
                        {
                            for (int i = 0; i < cblDirMenuName.Items.Count; i++)
                            {
                                cblDirMenuName.Items[i].Selected = true;
                            }
                            txtDirMenuName.Text = "Item Name(" + cblDirMenuName.Items.Count + ")";
                            lblDirMenuName.Text = "Item Name";
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void directinvward()
    {
        if (rdb_dirmenuconssum.Checked == true)
        {
            txtDirMenu.Enabled = true;
            lblDirMenuName.Text = "Menu Name";
            bindmenuname();
        }
        if (rdb_dircleanconssum.Checked == true)
        {
            txtDirMenu.Enabled = false;
            lblDirMenuName.Text = "Item Name";
            binditemdir();
        }
    }
    protected void bindPurposeCatagory()
    {
        try
        {
            ddl_purposeCategory.Items.Clear();
            ds.Clear();
            string sql = "select distinct MasterCode,MasterValue  from CO_MasterValues where MasterCriteria ='Menu Purpose Category' and CollegeCode ='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_purposeCategory.DataSource = ds;
                ddl_purposeCategory.DataTextField = "MasterValue";
                ddl_purposeCategory.DataValueField = "MasterCode";
                ddl_purposeCategory.DataBind();
                ddl_purposeCategory.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddl_purposeCategory.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch
        { }
    }
}