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

public partial class HT_Return_item : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    bool check = false;
    string firstdate = "";
    DateTime dt = new DateTime();
    bool chk = false;
    string isreturnval = "";


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
            for (int i = 0; i < cbl_menutype.Items.Count; i++)
            {
                cbl_menutype.Items[i].Selected = true;
            }
            cbl_menutype_SelectIndexChange(sender, e);
            bindhostelname();
            rdomenuitemcon.Checked = true;
            txtdate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            txtdate.Attributes.Add("readonly", "readonly");
            txtpop1itemcode.Attributes.Add("readonly", "readonly");
            txtpop1itemname.Attributes.Add("readonly", "readonly");
            bindsession();
            loadmenuname();
            ViewState["reconsum"] = null;
            //lbl_alerterror.Visible = false;
            //alertmessage.Visible = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            //spreaddiv1.Visible = false;
            FpSpread1.Visible = false;
            rptprint.Visible = false;
            //btngo_Click(sender, e); 
            lblvalidation1.Visible = false;
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

    protected void rdomenuitemcon_check(object sender, EventArgs e)
    {
        if (rdomenuitemcon.Checked == true)
        {
            txt_menutype.Enabled = true;
            //spreaddiv1.Visible = false;
            FpSpread1.Visible = false;
            rptprint.Visible = false;
            btnsave.Visible = false;
            loadmenuname();
        }
        else
        {
            txt_menutype.Enabled = false;
            //spreaddiv1.Visible = false;
            FpSpread1.Visible = false;
            rptprint.Visible = false;
            btnsave.Visible = false;
        }

    }
    protected void rdocleanitem_check(object sender, EventArgs e)
    {
        if (rdocleanitem.Checked == true)
        {
            txt_menutype.Enabled = false;
            //spreaddiv1.Visible = false;
            FpSpread1.Visible = false;
            rptprint.Visible = false;
            btnsave.Visible = false;
            loaditemname();
        }
        else
        {
            txt_menutype.Enabled = true;
            //spreaddiv1.Visible = true;
            FpSpread1.Visible = true;
            rptprint.Visible = true;
            btnsave.Visible = true;
        }

    }
    public void loaditemname()
    {
        try
        {
            hat.Clear();
            string item = "";
            txtmenuname.Text = "--Select--";
            //string itemheadercode = "";
            chk_lstmenuname.Items.Clear();
            string hostelcode = "";
            //for (int i = 0; i < chklstsession.Items.Count; i++)
            //{
            //    if (chklstsession.Items[i].Selected == true)
            //    {
            //        if (item == "")
            //        {
            //            item = "" + chklstsession.Items[i].Value.ToString() + "";
            //        }
            //        else
            //        {
            //            item = item + "'" + "," + "'" + chklstsession.Items[i].Value.ToString() + "";
            //        }
            //    }
            //}
            item = Convert.ToString(ddlsession.SelectedItem.Value);
            //for (int i = 0; i < chklsthostelname.Items.Count; i++)
            //{
            //    if (chklsthostelname.Items[i].Selected == true)
            //    {
            //        if (hostelcode == "")
            //        {
            //            hostelcode = "" + chklsthostelname.Items[i].Value.ToString() + "";
            //        }
            //        else
            //        {
            //            hostelcode = hostelcode + "'" + "," + "'" + chklsthostelname.Items[i].Value.ToString() + "";
            //        }
            //    }
            //}
            hostelcode = Convert.ToString(ddlhostelname.SelectedItem.Value);

            if (item.Trim() != "")
            {
                string firstdate = Convert.ToString(txtdate.Text);
                DateTime dt = new DateTime();
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                if (item.Trim() != "" && hostelcode.Trim() != "")
                {
                    string menuquery = "";//22.07.16
                    menuquery = "select MenuMasterFK,SessionMasterFK from HT_MenuSchedule where SessionMasterFK in ('" + item + "') and MessMasterFK in ('" + hostelcode + "') and MenuScheduleDate ='" + dt.ToString("MM/dd/yyyy") + "' and ScheduleType='1'";
                    menuquery = menuquery + " select MenuMasterFK,SessionMasterFK from HT_MenuSchedule where SessionMasterFK in ('" + item + "') and MessMasterFK in ('" + hostelcode + "') and MenuScheduleday ='" + dt.ToString("dddd") + "' and ScheduleType='2'";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(menuquery, "Text");

                    string scheduletype = "";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        scheduletype = " and Schedule_Date='" + dt.ToString("MM/dd/yyyy") + "'";
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        scheduletype = " and Schedule_Day='" + dt.ToString("dddd") + "'";
                    }


                    /*menuquery = "";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            string menucode = Convert.ToString(ds.Tables[0].Rows[row][0]);
                            string[] split_new = menucode.Split(',');
                            if (split_new.Length > 0)
                            {
                                for (int low = 0; low <= split_new.GetUpperBound(0); low++)
                                {
                                    if (menuquery.Trim() == "")
                                    {
                                        menuquery = Convert.ToString(split_new[low]);
                                    }
                                    else
                                    {
                                        menuquery = menuquery + "'" + "," + "'" + Convert.ToString(split_new[low]);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            for (int row = 0; row < ds.Tables[1].Rows.Count; row++)
                            {
                                string menucode = Convert.ToString(ds.Tables[1].Rows[row][0]);
                                string[] split_new = menucode.Split(',');
                                if (split_new.Length > 0)
                                {
                                    for (int low = 0; low <= split_new.GetUpperBound(0); low++)
                                    {
                                        if (menuquery.Trim() == "")
                                        {
                                            menuquery = Convert.ToString(split_new[low]);
                                        }
                                        else
                                        {
                                            menuquery = menuquery + "'" + "," + "'" + Convert.ToString(split_new[low]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    */

                    //string deptquery = "select distinct ItemPK,ItemName from IM_ItemMaster where  ItemPK in('" + menuquery + "')  order by ItemPK ";
                    chk_lstmenuname.Items.Clear();
                    if (scheduletype.Trim() != "")
                    {
                        string deptquery = " select distinct ItemPK,ItemName from Cleaning_ItemMaseter m,Cleaning_ItemDetailMaster d,IM_ItemMaster i where d.Clean_ItemMasterFK =m.Clean_ItemMasterPK and i.ItemPK=d.Itemfk and SessionFK in ('" + item + "') and m.MessMasterFK in('" + hostelcode + "') " + scheduletype + "";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(deptquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            chk_lstmenuname.DataSource = ds;
                            chk_lstmenuname.DataTextField = "ItemName";
                            chk_lstmenuname.DataValueField = "ItemPK";
                            chk_lstmenuname.DataBind();
                            if (chk_lstmenuname.Items.Count > 0)
                            {
                                for (int i = 0; i < chk_lstmenuname.Items.Count; i++)
                                {
                                    chk_lstmenuname.Items[i].Selected = true;
                                }
                                txtmenuname.Text = "Item Name(" + chk_lstmenuname.Items.Count + ")";
                                lblmenuname.Text = "Item Name";
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

    protected void txtdate_Change(object sender, EventArgs e)
    {
        try
        {
            bindsession();
            if (rdomenuitemcon.Checked == true)
            {
                loadmenuname();
            }
            if (rdocleanitem.Checked == true)
            {
                loaditemname();
            }

        }
        catch
        {

        }
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            string itemheadercode = "";
            string hostelcode = "";
            string menuvalue = "";
            Printcontrol.Visible = false;

            //for (int i = 0; i < chklstsession.Items.Count; i++)
            //{
            //    if (chklstsession.Items[i].Selected == true)
            //    {
            //        if (itemheadercode == "")
            //        {
            //            itemheadercode = "" + chklstsession.Items[i].Value.ToString() + "";
            //        }
            //        else
            //        {
            //            itemheadercode = itemheadercode + "'" + "," + "'" + chklstsession.Items[i].Value.ToString() + "";
            //        }
            //    }
            //}
            itemheadercode = Convert.ToString(ddlsession.SelectedItem.Value);

            hostelcode = Convert.ToString(ddlhostelname.SelectedItem.Value);

            for (int i = 0; i < chk_lstmenuname.Items.Count; i++)
            {
                if (chk_lstmenuname.Items[i].Selected == true)
                {
                    if (menuvalue == "")
                    {
                        menuvalue = "" + chk_lstmenuname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        menuvalue = menuvalue + "'" + "," + "'" + chk_lstmenuname.Items[i].Value.ToString() + "";
                    }
                }
            }

            string itemFk = "";
            for (int i = 0; i < chk_lstmenuname.Items.Count; i++)
            {
                if (chk_lstmenuname.Items[i].Selected == true)
                {
                    if (itemFk == "")
                    {
                        itemFk = "" + chk_lstmenuname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemFk = itemFk + "'" + "," + "'" + chk_lstmenuname.Items[i].Value.ToString() + "";
                    }
                }
            }

            string firstdate = Convert.ToString(txtdate.Text);
            DateTime dt = new DateTime();
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            DataView dv = new DataView();
            DataView dv1 = new DataView();
            ArrayList Addvalue = new ArrayList();
            Hashtable hashset = new Hashtable();


            if (itemheadercode.Trim() != "" && hostelcode.Trim() != "" && menuvalue.Trim() != "")
            {
                if (rdomenuitemcon.Checked == true)
                {
                    #region menu item

                    // string selectquery = " select distinct i.Item_Code,item_name,i.item_unit,hand_qty,rpu,NoOfPersons  from Menu_ItemMaster m ,Menu_ItemDetail mi,item_master i,stock_master s,MenuMaster mu where m.Menu_ItemMasterCode =mi.Menu_ItemMasterCode and mi.Hostel_Code =m.Hostel_Code and i.item_code =mi.Item_Code and i.item_code =s.item_code and mi.Item_Code =s.item_code and mu.MenuCode =m.SessionMenu_Code  and m.Hostel_Code in ('" + hostelcode + "') and SessionMenu_Code in ('" + itemheadercode + "') and i.Item_Code not in (select Item_Code from DailyConsumption_Detail dd ,DailyConsumption_Master dm where dd.DailyConsumptionMaster_Code =dm.DailyConsumptionMaster_Code and dd.Hostel_Code=dm.Hostel_Code and Consumption_Date ='" + dt.ToString("MM/dd/yyyy") + "' and Session_Code='" + ddlsession.SelectedItem.Value + "' and SessionMenu_Code ='" + ddlmenuname.SelectedItem.Value + "' and dm.Hostel_Code ='" + ddlhostelname.SelectedItem.Value + "')";
                    //string selectquery = " select d.item_code,item_name,SUM(AvlQty)as hand_qty,i.item_unit,NoOfPersons,s.RPU from Menu_ItemDetail d, Menu_ItemMaster m,MenuMaster u,item_master i,MessMaster  h,Stock_Detail s where d.Menu_ItemMasterCode = m.Menu_ItemMasterCode and m.SessionMenu_Code = u.MenuCode and d.Hostel_Code = h.MessID  and d.Item_Code = i.item_code and d.Item_Code = s.Item_Code and i.item_code = s.Item_Code and s.Dept_Code =h.MessID and h.MessID in ('" + hostelcode + "') and u.MenuCode in ('" + menuvalue + "') group by d.item_code,item_name,i.item_unit,NoOfPersons,s.RPU   ";
                    //selectquery = selectquery + "   select Schedule_Day,Menu_Code,change_strength,Hostel_Code,Session_Code  from MenuSchedule_DayWise where schedule_type ='0' ";
                    //selectquery = selectquery + "   select  i.Item_Code,item_name,i.item_unit,hand_qty,m.Hostel_Code ,NoOfPersons,Needed_Qty,mu.MenuCode  from Menu_ItemMaster m ,Menu_ItemDetail mi,item_master i,stock_master s,MenuMaster mu where m.Menu_ItemMasterCode =mi.Menu_ItemMasterCode and mi.Hostel_Code =m.Hostel_Code and i.item_code =mi.Item_Code and i.item_code =s.item_code and mi.Item_Code =s.item_code and mu.MenuCode =m.SessionMenu_Code  and m.Hostel_Code in ('" + hostelcode + "') and SessionMenu_Code in ('" + menuvalue + "')";

                    //string selectquery = " select dd.Item_Code,i.item_name,item_unit,Hand_Qty,Total_Present,Consumption_Qty,RPU,Session_Code,dd.Hostel_Code,dm.SessionMenu_Code,dm.DailyConsumptionMaster_Code from DailyConsumption_Master dm,DailyConsumption_Detail dd,item_master i where dm.DailyConsumptionMaster_Code =dd.DailyConsumptionMaster_Code and dd.Item_Code =i.item_code and  Consumption_Date ='" + dt.ToString("MM/dd/yyyy") + "'  and typeofconsume ='0' and dm.Hostel_Code in ('" + hostelcode + "') and dm.Session_Code in ('" + itemheadercode + "')";//and dm.SessionMenu_Code in ('" + menuvalue + "') 

                    string selectquery = "select  dd.ItemFK, i.ItemCode,i.ItemName as item_name,i.ItemUnit as item_unit,Total_Present,ConsumptionQty as Consumption_Qty,RPU,SessionFK as Session_Code, dm.MessMasterFK as Hostel_Code,dm.SessionFK as SessionMenu_Code,dm.DailyConsumptionMasterPK as DailyConsumptionMaster_Code from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where dm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK and dd.ItemFK =i.ItemPK and dm.DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' and dm.ForMess ='1' and dm.MessMasterFK in('" + hostelcode + "') and dm.SessionFK in ('" + itemheadercode + "') ";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 9;
                        FpSpread1.Sheets[0].AutoPostBack = false;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Width = 917;

                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        FarPoint.Web.Spread.DoubleCellType db = new FarPoint.Web.Spread.DoubleCellType();
                        db.ErrorMessage = "Enter only Numbers";

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
                        //FpSpread1.Columns[4].Visible = false;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Total Strength";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[5].Width = 100;
                        FpSpread1.Columns[5].Locked = true;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Consumption Quantity";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[6].Width = 100;
                        // FpSpread1.Columns[6].BackColor = Color.Gainsboro;
                        FpSpread1.Columns[6].Locked = true;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Return Quantity";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[7].Width = 100;
                        FpSpread1.Columns[7].BackColor = Color.Gainsboro;
                        FpSpread1.Columns[7].Locked = false;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Select";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[8].Width = 50;
                        //FpSpread1.Columns[7].Visible = false;
                        FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                        cb.AutoPostBack = true;

                        FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                        cb1.AutoPostBack = false;

                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].CellType = cb;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(ds.Tables[0].Rows[i]["ItemFK"]);

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Hostel_Code"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Item_Name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Session_Code"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["rpu"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;


                            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i]["hand_qty"]), 2));

                            string handquantity = d2.GetFunction("select IssuedQty-isnull(UsedQty,'0') as BalQty from IT_StockDeptDetail where ItemFK ='" + Convert.ToString(ds.Tables[0].Rows[i]["ItemFK"]) + "' and DeptFK ='" + hostelcode + "'");
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = handquantity;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["SessionMenu_Code"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].ForeColor = Color.Blue;

                            #region unknown


                            //int hostelcount = 0;
                            //int strength = 0;
                            //double Need = 0;
                            //if (ds.Tables[2].Rows.Count > 0)
                            //{
                            //    ds.Tables[2].DefaultView.RowFilter = "item_code='" + Convert.ToString(ds.Tables[0].Rows[i]["Item_Code"]) + "'";
                            //    dv = ds.Tables[2].DefaultView;
                            //    if (dv.Count > 0)
                            //    {
                            //        for (int ro = 0; ro < dv.Count; ro++)
                            //        {
                            //            string Needquantiy = Convert.ToString(dv[ro]["Needed_Qty"]);
                            //            string noofpersion = Convert.ToString(dv[ro]["NoOfPersons"]);

                            //            if (ds.Tables[1].Rows.Count > 0)
                            //            {
                            //                for (int ik = 0; ik < chklstsession.Items.Count; ik++)
                            //                {
                            //                    if (chklstsession.Items[ik].Selected == true)
                            //                    {
                            //                        ds.Tables[1].DefaultView.RowFilter = "Schedule_Day='" + dt.ToString("dddd") + "' and Hostel_Code ='" + Convert.ToString(dv[ro]["Hostel_Code"]) + "' and ( Menu_Code like '" + Convert.ToString(dv[ro]["MenuCode"]) + "%'  or Menu_Code like '%" + Convert.ToString(dv[ro]["MenuCode"]) + "' or Menu_Code like '%" + Convert.ToString(dv[ro]["MenuCode"]) + "%') and Session_Code='" + Convert.ToString(chklstsession.Items[ik].Value) + "'";
                            //                        dv1 = ds.Tables[1].DefaultView;
                            //                        if (dv1.Count > 0)
                            //                        {
                            //                            string total = Convert.ToString(dv1[0]["change_strength"]);
                            //                            if (total.Trim() != "")
                            //                            {
                            //                                strength = strength + Convert.ToInt32(total);
                            //                                hostelcount = hostelcount + 1;
                            //                                if (noofpersion.Trim() != "" && Needquantiy.Trim() != "" && noofpersion.Trim() != "0" && Needquantiy.Trim() != "0")
                            //                                {
                            //                                    double valueamt = Convert.ToDouble(total) / Convert.ToDouble(noofpersion) * Convert.ToDouble(Needquantiy);
                            //                                    if (valueamt != 0)
                            //                                    {
                            //                                        Need = Need + valueamt;
                            //                                    }
                            //                                    if (!hashset.Contains(Convert.ToString(dv[ro]["Hostel_Code"]) + "-" + Convert.ToString(dv[ro]["MenuCode"]) + "-" + Convert.ToString(chklstsession.Items[ik].Value)))
                            //                                    {
                            //                                        hashset.Add(Convert.ToString(dv[ro]["Hostel_Code"]) + "-" + Convert.ToString(dv[ro]["MenuCode"]) + "-" + Convert.ToString(chklstsession.Items[ik].Value), dt.ToString("dddd") + "-" + Convert.ToString(dv[ro]["Hostel_Code"]) + "-" + Convert.ToString(dv[ro]["MenuCode"]) + "-" + Convert.ToString(chklstsession.Items[ik].Value) + "-" + total + "-" + Needquantiy + "-" + noofpersion + "-" + Convert.ToString(ds.Tables[0].Rows[i]["Item_Code"]) + "-" + Convert.ToString(ds.Tables[0].Rows[i]["rpu"]));
                            //                                    }
                            //                                    else
                            //                                    {
                            //                                        string getvalue = Convert.ToString(hashset[Convert.ToString(dv[ro]["Hostel_Code"]) + "-" + Convert.ToString(dv[ro]["MenuCode"]) + "-" + Convert.ToString(chklstsession.Items[ik].Value)]);
                            //                                        if (getvalue.Trim() != "")
                            //                                        {
                            //                                            hashset.Remove(Convert.ToString(dv[ro]["Hostel_Code"]) + "-" + Convert.ToString(dv[ro]["MenuCode"]) + "-" + Convert.ToString(chklstsession.Items[ik].Value));
                            //                                            getvalue = getvalue + "/" + dt.ToString("dddd") + "-" + Convert.ToString(dv[ro]["Hostel_Code"]) + "-" + Convert.ToString(dv[ro]["MenuCode"]) + "-" + Convert.ToString(chklstsession.Items[ik].Value) + "-" + total + "-" + Needquantiy + "-" + noofpersion + "-" + Convert.ToString(ds.Tables[0].Rows[i]["Item_Code"]) + "-" + Convert.ToString(ds.Tables[0].Rows[i]["rpu"]);
                            //                                            hashset.Add(Convert.ToString(dv[ro]["Hostel_Code"]) + "-" + Convert.ToString(dv[ro]["MenuCode"]) + "-" + Convert.ToString(chklstsession.Items[ik].Value), getvalue);
                            //                                        }
                            //                                    }
                            //                                }
                            //                            }
                            //                        }
                            //                    }
                            //                }
                            //            }
                            //        }
                            //    }
                            //}
                            #endregion

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Total_Present"]);//strength
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[i]["DailyConsumptionMaster_Code"]);//strength
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString();//hostelcount
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["Consumption_Qty"]);
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString();//hostelcount
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = "";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].CellType = db;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].ForeColor = Color.Green;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].CellType = cb1;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;

                        }
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        //spreaddiv1.Visible = true;
                        FpSpread1.Visible = true;
                        rptprint.Visible = true;
                        lblerror.Visible = false;
                        btnsave.Visible = true;
                        Session["AddArrayValue"] = hashset;
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "No Record Found";
                        //spreaddiv1.Visible = false;
                        FpSpread1.Visible = false;
                        rptprint.Visible = false;
                        btnsave.Visible = false;
                    }
                    #endregion
                }
                if (rdocleanitem.Checked == true)
                {
                    #region Cleaning item

                    //string selecnew_query = " select distinct i.Item_Code,item_name,i.item_unit,hand_qty,Needed_Qty,rpu from Cleaning_ItemMaseter cm,Cleaning_ItemDetailMaster cd,item_master i ,stock_master s where cm.Clean_ItemMasterCode =cd.Clean_ItemMasterCode and cm.Hostel_Code =cd.Hostel_Code and cd.Item_Code =i.item_code and cd.Item_Code =s.item_code and s.item_code =i.item_code  and cm.Hostel_Code in ('" + hostelcode + "')  and Session_Code in ('" + itemheadercode + "') and i.Item_Code in ('" + menuvalue + "') ";
                    //selecnew_query = selecnew_query + "   select Schedule_Day,Menu_Code,change_strength,Hostel_Code,Session_Code  from MenuSchedule_DayWise where schedule_type ='1' ";
                    //selecnew_query = selecnew_query + " select i.Item_Code,item_name,i.item_unit,hand_qty,cm.Hostel_Code ,Needed_Qty from Cleaning_ItemMaseter cm,Cleaning_ItemDetailMaster cd,item_master i ,stock_master s where cm.Clean_ItemMasterCode =cd.Clean_ItemMasterCode and cm.Hostel_Code =cd.Hostel_Code and cd.Item_Code =i.item_code and cd.Item_Code =s.item_code and s.item_code =i.item_code  and cm.Hostel_Code in ('" + hostelcode + "')  and Session_Code in ('" + itemheadercode + "') and i.Item_Code in ('" + menuvalue + "') ";

                    //string selecnew_query = "select  dd.Item_Code,i.item_name,item_unit,Hand_Qty,Total_Present,Consumption_Qty,RPU,Session_Code, dd.Hostel_Code,dm.SessionMenu_Code,dm.DailyConsumptionMaster_Code from DailyConsumption_Master dm,DailyConsumption_Detail dd,item_master i where dm.DailyConsumptionMaster_Code =dd.DailyConsumptionMaster_Code and dd.Item_Code =i.item_code and  Consumption_Date ='" + dt.ToString("MM/dd/yyyy") + "' and typeofconsume ='1' and dm.Hostel_Code in ('" + hostelcode + "') ";// and isreturn='" + isreturnval + "'  and dm.Session_Code in ('" + itemheadercode + "') and dm.SessionMenu_Code in ('" + menuvalue + "') 

                    string selecnew_query = "select  dd.itemfk, i.ItemCode,i.ItemName as item_name,i.ItemUnit as item_unit,Total_Present,ConsumptionQty as Consumption_Qty,RPU,SessionFK as Session_Code, dm.MessMasterFK as Hostel_Code,dm.SessionFK as SessionMenu_Code,dm.DailyConsumptionMasterPK as DailyConsumptionMaster_Code from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where dm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK and dd.ItemFK =i.ItemPK and dm.DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' and dm.ForMess ='0' and dm.MessMasterFK in ('" + hostelcode + "') and dd.itemfk in ('" + itemFk + "')";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selecnew_query, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 9;
                        FpSpread1.Sheets[0].AutoPostBack = false;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Width = 816;

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
                        FpSpread1.Columns[0].Locked = true;

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

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Consumption Quantity";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[6].Width = 100;
                        // FpSpread1.Columns[6].BackColor = Color.Gainsboro;
                        FpSpread1.Columns[6].Locked = true;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Return Quantity";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[7].Width = 100;
                        FpSpread1.Columns[7].BackColor = Color.Gainsboro;
                        FpSpread1.Columns[7].Locked = false;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Select";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[8].Width = 50;
                        //FpSpread1.Columns[7].Visible = false;
                        FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                        cb.AutoPostBack = true;

                        FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                        cb1.AutoPostBack = false;

                        FarPoint.Web.Spread.DoubleCellType db = new FarPoint.Web.Spread.DoubleCellType();
                        db.ErrorMessage = "Enter only Numbers";

                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].CellType = cb;

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(ds.Tables[0].Rows[i]["ItemFK"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Hostel_Code"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Item_Name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Session_Code"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["item_unit"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["rpu"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                            string handquantity = d2.GetFunction("select IssuedQty-isnull(UsedQty,'0') as BalQty from IT_StockDeptDetail where ItemFK ='" + Convert.ToString(ds.Tables[0].Rows[i]["ItemFK"]) + "' and DeptFK ='" + hostelcode + "'");
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = handquantity;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["SessionMenu_Code"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].ForeColor = Color.Blue;


                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["Consumption_Qty"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(ds.Tables[0].Rows[i]["DailyConsumptionMaster_Code"]);//hostelcount
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;


                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = "";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].CellType = db;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].ForeColor = Color.Green;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].CellType = cb1;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].ForeColor = Color.Green;

                        }
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        //spreaddiv1.Visible = true;
                        FpSpread1.Visible = true;
                        rptprint.Visible = true;
                        lblerror.Visible = false;
                        btnsave.Visible = true;

                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "No Record Found";
                        //spreaddiv1.Visible = false;
                        FpSpread1.Visible = false;
                        rptprint.Visible = false;
                        btnsave.Visible = false;
                    }
                    #endregion
                }

            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select All Fields";
                //spreaddiv1.Visible = false;
                FpSpread1.Visible = false;
                rptprint.Visible = false;
                btnsave.Visible = false;
            }
        }
        catch
        {
        }
    }
    protected void FpSpread1_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "8")
            {
                if (FpSpread1.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 8].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, 8].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, 8].Value = 0;
                        }
                    }
                }
            }
        }
        catch
        {

        }
    }

    //protected void chksession_checkedchange(object sender, EventArgs e)
    //{
    //    if (chksessionname.Checked == true)
    //    {
    //        for (int i = 0; i < chklstsession.Items.Count; i++)
    //        {
    //            chklstsession.Items[i].Selected = true;
    //        }
    //        txtsessionname.Text = "Session Name(" + (chklstsession.Items.Count) + ")";
    //    }
    //    else
    //    {
    //        for (int i = 0; i < chklstsession.Items.Count; i++)
    //        {
    //            chklstsession.Items[i].Selected = false;
    //        }
    //        txtsessionname.Text = "--Select--";
    //    }
    //    if (rdomenuitemcon.Checked == true)
    //    {
    //        loadmenuname();
    //    }
    //    if (rdocleanitem.Checked == true)
    //    {
    //        loaditemname();
    //    }
    //}
    //protected void chklstsession_Change(object sender, EventArgs e)
    //{
    //    txtsessionname.Text = "--Select--";
    //    chksessionname.Checked = false;
    //    int commcount = 0;
    //    for (int i = 0; i < chklstsession.Items.Count; i++)
    //    {
    //        if (chklstsession.Items[i].Selected == true)
    //        {
    //            commcount = commcount + 1;
    //        }
    //    }
    //    if (commcount > 0)
    //    {
    //        txtsessionname.Text = "Session Name(" + commcount.ToString() + ")";
    //        if (commcount == chklstsession.Items.Count)
    //        {
    //            chksessionname.Checked = true;
    //        }
    //    }
    //    if (rdomenuitemcon.Checked == true)
    //    {
    //        loadmenuname();
    //    }
    //    if (rdocleanitem.Checked == true)
    //    {
    //        loaditemname();
    //    }
    //}

    protected void chkmenuname_Change(object sender, EventArgs e)
    {
        if (chkmenuname.Checked == true)
        {
            if (chk_lstmenuname.Items.Count > 0)
            {
                for (int i = 0; i < chk_lstmenuname.Items.Count; i++)
                {
                    chk_lstmenuname.Items[i].Selected = true;
                }
                if (rdomenuitemcon.Checked == true)
                {
                    txtmenuname.Text = "Menu Name(" + (chk_lstmenuname.Items.Count) + ")";
                }
                else
                {
                    txtmenuname.Text = "Item Name(" + (chk_lstmenuname.Items.Count) + ")";
                }
            }
        }
        else
        {
            for (int i = 0; i < chk_lstmenuname.Items.Count; i++)
            {
                chk_lstmenuname.Items[i].Selected = false;
            }
            txtmenuname.Text = "--Select--";
        }

    }
    protected void chk_lstmenuname_Change(object sender, EventArgs e)
    {
        txtmenuname.Text = "--Select--";
        chkmenuname.Checked = false;
        int commcount = 0;
        for (int i = 0; i < chk_lstmenuname.Items.Count; i++)
        {
            if (chk_lstmenuname.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            if (rdomenuitemcon.Checked == true)
            {
                txtmenuname.Text = "Menu Name(" + commcount.ToString() + ")";
            }
            else
            {
                txtmenuname.Text = "Item Name(" + commcount.ToString() + ")";
            }
            if (commcount == chk_lstmenuname.Items.Count)
            {
                chkmenuname.Checked = true;
            }
        }
    }

    protected void ddlsession_change(object sender, EventArgs e)
    {
        if (rdomenuitemcon.Checked == true)
        {
            loadmenuname();
        }
        else
        {
            loaditemname();
        }
    }


    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        consumptioncheck();
        consum.Visible = false;
        btngo_Click(sender, e);

    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        consum.Visible = false;
        alertmessage.Visible = false;
        //popwindow.Visible = true;
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            firstdate = Convert.ToString(txtdate.Text);
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            bool emptyreturn = false;
            bool returnqty1 = false;
            bool finaltest1 = false;
            bool testflage1 = false;
            bool checkboxselect = false;
            int chkcount = 0;
            for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 8].Value);
                if (checkval == 1)
                {
                    checkboxselect = true;
                    string itemcode1 = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Note);
                    string sessioncode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Tag);
                    string sessionmenucode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Tag);
                    string hostel1 = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag);
                    string q = "";
                    if (rdomenuitemcon.Checked == true)
                    {
                        //q = "select * from DailyConsumption_Master dm,DailyConsumption_Detail dd where dm.DailyConsumptionMaster_Code = dd.DailyConsumptionMaster_Code and Item_Code ='" + itemcode1 + "' and Consumption_Date ='" + dt.ToString("MM/dd/yyyy") + "' and Session_Code='" + sessioncode + "' and SessionMenu_Code='" + sessionmenucode + "' and dd.Hostel_Code='" + hostel1 + "' and typeofconsume='0' and ISNULL(isreturn,0)<>0";

                        q = "select * from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd where dm.DailyConsumptionMasterPK = dd.DailyConsumptionMasterFK and ItemFK ='" + itemcode1 + "' and dm.DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' and SessionFK='" + sessioncode + "'  and dm.MessMasterFK='" + hostel1 + "' and ForMess='1' and ISNULL(Isreturn,0)<>0";
                        bool finaltest = false;
                        bool testflage = false;
                        string totalvalue = "";

                        double hand1 = 0;
                        double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 6].Text), out hand1);

                        string returnq = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 7].Text);
                        if (returnq == "")
                        {
                            emptyreturn = true;
                        }
                        double returnq1 = 0;
                        if (returnq != "")
                        {
                            returnq1 = Convert.ToDouble(returnq);
                        }
                        else
                        {
                            returnq1 = 0;
                        }
                        if (hand1 < returnq1)//barath 19.1.18
                        {
                            finaltest1 = true;
                        }
                        else
                        {
                            testflage = true;
                        }
                    }
                    else
                    {
                        //q = "select * from DailyConsumption_Master dm,DailyConsumption_Detail dd where dm.DailyConsumptionMaster_Code = dd.DailyConsumptionMaster_Code and Item_Code ='" + itemcode1 + "' and Consumption_Date ='" + dt.ToString("MM/dd/yyyy") + "' and  dd.Hostel_Code='" + hostel1 + "' and typeofconsume='1' and ISNULL(isreturn,0)<>0";

                        q = "select * from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd where dm.DailyConsumptionMasterPK = dd.DailyConsumptionMasterFK and ItemFK ='" + itemcode1 + "' and DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' and  dm.MessMasterFK='" + hostel1 + "' and ForMess='1'";

                        string hand = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 6].Text);
                        double hand1 = 0;
                        if (hand != "")
                        {
                            hand1 = Convert.ToDouble(hand);
                        }
                        else
                        {
                            hand1 = 0;
                        }
                        string returnq = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 7].Text);
                        if (returnq == "")
                        {
                            emptyreturn = true;
                        }
                        double returnq1 = 0;
                        if (returnq != "")
                        {
                            returnq1 = Convert.ToDouble(returnq);
                        }
                        else
                        {
                            returnq1 = 0;
                        }
                        if (hand1 < returnq1)//barath 19.1.18
                        {
                            finaltest1 = true;
                        }
                        else
                        {
                            testflage1 = true;
                        }
                    }
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q, "Text");
                    if (emptyreturn == false)
                    {
                        if (finaltest1 == false)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                chk = true;
                            }
                        }
                    }
                }
                else
                {
                    chkcount++;
                }
            }
            if (checkboxselect == false)
            {
                lbl_alerterror.Visible = true;
                lbl_alerterror.Text = "Please Select Any One Item";
                alertmessage.Visible = true;
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (chkcount == ds.Tables[0].Rows.Count)
                {
                    lbl_alerterror.Visible = true;
                    lbl_alerterror.Text = "Please Select Any One Item";
                    alertmessage.Visible = true;
                }
            }
            if (emptyreturn == true)
            {
                lbl_alerterror.Visible = true;
                lbl_alerterror.Text = "Please enter the return quantity";
                alertmessage.Visible = true;
            }
            if (finaltest1 == true)
            {
                lbl_alerterror.Visible = true;
                lbl_alerterror.Text = "Return quantity is greater than Consumption quantity";
                alertmessage.Visible = true;
            }
            if (chk == true)
            {
                ViewState["reconsum"] = 1;
                consum.Visible = true;
                lbl_sure.Text = "Already returned. Do you want to return again?";
            }
            else
            {
                if (emptyreturn == false)
                {
                    if (finaltest1 == false)
                    {
                        consumptioncheck();
                        btngo_Click(sender, e);
                    }
                }
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
            bool finaltest = false;
            bool stockdetail = false;
            FpSpread1.SaveChanges();
            Hashtable hnew = new Hashtable();
            string firstdate = Convert.ToString(txtdate.Text);
            DateTime dt = new DateTime();
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            DateTime dtaccessdate = DateTime.Now;
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            Hashtable addreturnvlaue = new Hashtable();
            if (FpSpread1.Sheets[0].RowCount > 0)
            {
                isreturnval = "1";
                if (rdomenuitemcon.Checked == true)
                {
                    for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                    {
                        int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 8].Value);
                        if (checkval == 1)
                        {
                            string inserquery = "";
                            string itemcode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Note);
                            string rpu = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Tag);
                            string hostel1 = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag);
                            string consume1 = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 6].Text);
                            string rpu1 = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Tag);
                            string handquantity1 = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Text);
                            string reqqty = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 7].Text);
                            string mastercode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 5].Tag);
                            string sessioncode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Tag);
                            string sessionmenucode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Tag);

                            string BalQty = d2.GetFunction("select IssuedQty-isnull(UsedQty,'0') as BalQty from IT_StockDeptDetail where ItemFK ='" + itemcode + "' and DeptFK ='" + hostel1 + "'");

                            double usedvalue = 0;
                            if (mastercode.Trim() != "" && mastercode.Trim() != "0")
                            {
                                inserquery = "update HT_DailyConsumptionMaster set DailyConsDate='" + dt.ToString("MM/dd/yyyy") + "' where SessionFK ='" + sessioncode + "' and MessMasterFK='" + hostel1 + "' and DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' and DailyConsumptionMasterPK='" + mastercode + "'";
                                int update = d2.update_method_wo_parameter(inserquery, "Text");
                            }

                            if (rpu1.Trim() != "")
                            {
                                usedvalue = Convert.ToDouble(reqqty) * Convert.ToDouble(rpu1);
                            }
                            string newinsert = "update HT_DailyConsumptionDetail set ConsumptionQty =ConsumptionQty-ISNULL('" + reqqty + "',0),Return_Qty =isnull(Return_Qty,0)+'" + reqqty + "',Isreturn ='" + isreturnval + "',RPU ='" + rpu1 + "'  where DailyConsumptionMasterFK ='" + mastercode + "' and ItemFK ='" + itemcode + "'";

                            int inst = d2.update_method_wo_parameter(newinsert, "Text");
                            if (inst != 0)
                            {
                                finaltest = true;
                            }
                            string stockvalue = "if exists (select * from IT_StockDeptDetail where ItemFK ='" + itemcode + "' and DeptFK ='" + hostel1 + "') update IT_StockDeptDetail set usedQty=isnull(UsedQty,0)-'" + reqqty + "',BalQty = isnull(BalQty,0)+'" + reqqty + "' where ItemFK ='" + itemcode + "' and DeptFK ='" + hostel1 + "'";
                            int in_s = d2.update_method_wo_parameter(stockvalue, "Text");
                            if (in_s != 0)
                            {
                                stockdetail = true;
                            }
                        }
                    }
                    if (finaltest == true && stockdetail == true)
                    {
                        //btngo_Click(sender, e);
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Saved Successfully";
                        alertmessage.Visible = true;
                    }
                }
                if (rdocleanitem.Checked == true)
                {
                    bool savecheck = false;
                    FpSpread1.SaveChanges();
                    if (FpSpread1.Sheets[0].RowCount > 0)
                    {
                        int mastercode = 0;
                        string getcode = "";
                        string inserquery = "";
                        int firstinsert = 0;
                        getcode = d2.GetFunction("select DailyConsumptionMasterPK from  HT_DailyConsumptionMaster where DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' and ForMess='1'");
                        if (getcode.Trim() != "" && getcode.Trim() != "0")
                        {
                            mastercode = Convert.ToInt32(getcode);
                            inserquery = "update HT_DailyConsumptionMaster set DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' where DailyConsDate ='" + dt.ToString("MM/dd/yyyy") + "' and DailyConsumptionMasterPK='" + mastercode + "' and ForMess='1'";
                            firstinsert = d2.update_method_wo_parameter(inserquery, "Text");
                        }

                        if (firstinsert != 0)
                        {
                            if (FpSpread1.Sheets[0].RowCount > 0)
                            {
                                for (int row = 1; row < FpSpread1.Sheets[0].RowCount; row++)
                                {
                                    int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, 8].Value);
                                    if (checkval == 1)
                                    {
                                        double usedquantity = 0;
                                        double usedvalue = 0;
                                        string itemcode = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Note);
                                        string rpu = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 3].Tag);
                                        string hostel1 = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Tag);
                                        string consume1 = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 6].Text);
                                        string rpu1 = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 3].Tag);
                                        string handquantity1 = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 4].Text);
                                        string reqqty = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 7].Text);
                                        mastercode = Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, 6].Tag);

                                        string handquantity = d2.GetFunction("select IssuedQty-ISNULL(UsedQty,0) as handqty from IT_StockDeptDetail where ItemFK='" + itemcode + "' and DeptFK='" + hostel1 + "'");
                                        if (consume1.Trim() != "")
                                        {
                                            usedquantity = Convert.ToDouble(consume1) - Convert.ToDouble(reqqty);
                                            usedvalue = Convert.ToDouble(usedquantity) * Convert.ToDouble(rpu1);
                                        }
                                        //string newinsert = "update DailyConsumption_Detail set Consumption_Qty ='" + usedquantity + "' ,Consumption_Value ='" + usedvalue + "' , isreturn= '" + isreturnval + "',Hand_Qty =Hand_Qty+ ISNULL('" + reqqty + "',0),Req_Qty =Req_Qty-ISNULL('" + reqqty + "',0)  where DailyConsumptionMaster_Code ='" + mastercode + "' and Item_Code ='" + itemcode + "'";
                                        string newinsert = "update HT_DailyConsumptionDetail set ConsumptionQty ='" + usedquantity + "' ,Isreturn= '" + isreturnval + "',Return_Qty =Return_Qty+'" + reqqty + "'  where DailyConsumptionMasterFK ='" + mastercode + "' and ItemFk ='" + itemcode + "'";

                                        int inst = d2.update_method_wo_parameter(newinsert, "Text");
                                        if (inst != 0)
                                        {
                                            savecheck = true;
                                        }
                                        string getvalue = "";
                                        double fingetval = 0.00;
                                        string finalval = "";
                                        if (handquantity.Trim() != "" && handquantity.Trim() != "0")
                                        {
                                            double inval = Convert.ToDouble(handquantity);
                                            if (inval >= Convert.ToDouble(reqqty))
                                            {
                                                inval = inval + Convert.ToDouble(reqqty);
                                                getvalue = Convert.ToString(inval);
                                                //fingetval = Convert.ToDouble(getvalue) - Convert.ToDouble(reqqty);
                                                //finalval = Convert.ToString(fingetval);
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
                                        //string stockvalue = "if exists (select * from stock_master where item_code ='" + itemcode + "') update stock_master set hand_qty='" + getvalue + "',opningQuantity_date='" + System.DateTime.Now.ToString("MM/dd/yyyy") + "',access_date='" + dtaccessdate + "',access_time='" + dtaccesstime + "',invuser_code='" + usercode + "' where item_code ='" + itemcode + "'";
                                        string stockvalue = "if exists (select * from IT_StockDeptDetail where ItemFK ='" + itemcode + "' and DeptFK ='" + hostel1 + "') update IT_StockDeptDetail set UsedQty=isnull(UsedQty,0)-'" + reqqty + "',BalQty ='" + getvalue + "' where ItemFK ='" + itemcode + "' and DeptFK ='" + hostel1 + "'";

                                        int in_s = d2.update_method_wo_parameter(stockvalue, "Text");
                                        if (in_s != 0)
                                        {
                                            stockdetail = true;
                                        }
                                    }
                                }
                            }
                            if (savecheck == true && stockdetail == true)
                            {
                                //btngo_Click(sender, e);
                                lbl_alerterror.Visible = true;
                                lbl_alerterror.Text = "Saved Successfully";
                                alertmessage.Visible = true;
                            }
                            else
                            {
                                //   lbl_alerterror.Visible = true;
                                //    lbl_alerterror.Text = "Please Select Any one Items";
                                //    alertmessage.Visible = true;
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
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alertmessage.Visible = false;
    }
    protected void btnaddnew_Click(object sender, EventArgs e)
    {
        try
        {
            popwindow.Visible = true;
        }
        catch
        {
        }
    }
    protected void btnpopadd_Click(object sender, EventArgs e)
    {

    }
    protected void btnpopexit_Click(object sender, EventArgs e)
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
    protected void btnpop_Click(object sender, EventArgs e)
    {
        try
        {
            popwindow1.Visible = true;
        }
        catch
        {
        }
    }
    protected void btnpop1add_Click(object sender, EventArgs e)
    {

    }
    protected void btnpop1exit_Click(object sender, EventArgs e)
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

    public void loadmenuname()
    {
        try
        {
            hat.Clear();
            string item = "";
            txtmenuname.Text = "--Select--";
            chk_lstmenuname.Items.Clear();
            string hostelcode = "";
            //for (int i = 0; i < chklstsession.Items.Count; i++)
            //{
            //    if (chklstsession.Items[i].Selected == true)
            //    {
            //        if (item == "")
            //        {
            //            item = "" + chklstsession.Items[i].Value.ToString() + "";
            //        }
            //        else
            //        {
            //            item = item + "'" + "," + "'" + chklstsession.Items[i].Value.ToString() + "";
            //        }
            //    }
            //}
            item = Convert.ToString(ddlsession.SelectedItem.Value);
            hostelcode = Convert.ToString(ddlhostelname.SelectedItem.Value);

            if (item.Trim() != "")
            {
                string firstdate = Convert.ToString(txtdate.Text);
                DateTime dt = new DateTime();
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                if (item.Trim() != "" && hostelcode.Trim() != "")
                {
                    string menuquery = "";

                    menuquery = "select MenuMasterFK,SessionMasterFK from HT_MenuSchedule where SessionMasterFK in ('" + item + "') and MessMasterFK in ('" + hostelcode + "') and MenuScheduleDate ='" + dt.ToString("MM/dd/yyyy") + "' and ScheduleType='1'";

                    menuquery = menuquery + " select MenuMasterFK,SessionMasterFK from HT_MenuSchedule where SessionMasterFK in ('" + item + "') and MessMasterFK in ('" + hostelcode + "') and MenuScheduleday ='" + dt.ToString("dddd") + "' and ScheduleType='2'";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(menuquery, "Text");
                    menuquery = "";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            string menucode = Convert.ToString(ds.Tables[0].Rows[row][0]);
                            string[] split_new = menucode.Split(',');
                            if (split_new.Length > 0)
                            {
                                for (int low = 0; low <= split_new.GetUpperBound(0); low++)
                                {
                                    if (menuquery.Trim() == "")
                                    {
                                        menuquery = Convert.ToString(split_new[low]);
                                    }
                                    else
                                    {
                                        menuquery = menuquery + "'" + "," + "'" + Convert.ToString(split_new[low]);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            for (int row = 0; row < ds.Tables[1].Rows.Count; row++)
                            {
                                string menucode = Convert.ToString(ds.Tables[1].Rows[row][0]);
                                string[] split_new = menucode.Split(',');
                                if (split_new.Length > 0)
                                {
                                    for (int low = 0; low <= split_new.GetUpperBound(0); low++)
                                    {
                                        if (menuquery.Trim() == "")
                                        {
                                            menuquery = Convert.ToString(split_new[low]);
                                        }
                                        else
                                        {
                                            menuquery = menuquery + "'" + "," + "'" + Convert.ToString(split_new[low]);
                                        }
                                    }
                                }
                            }
                        }
                    }
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
                    string deptquery = "select distinct MenuMasterPK,MenuName from HM_MenuMaster where MenuMasterPK in('" + menuquery + "') and menutype in('" + menutype + "') order by MenuMasterPK ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(deptquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        chk_lstmenuname.DataSource = ds;
                        chk_lstmenuname.DataTextField = "MenuName";
                        chk_lstmenuname.DataValueField = "MenuMasterPK";
                        chk_lstmenuname.DataBind();
                        if (chk_lstmenuname.Items.Count > 0)
                        {
                            for (int i = 0; i < chk_lstmenuname.Items.Count; i++)
                            {
                                chk_lstmenuname.Items[i].Selected = true;
                            }
                            txtmenuname.Text = "Menu Name(" + chk_lstmenuname.Items.Count + ")";
                            lblmenuname.Text = "Menu Name";
                        }
                    }
                }
            }
        }
        catch
        {

        }
    }

    protected void clear()
    {
        try
        {
            txtpopitemcode.Text = "";
            txtpopitemname.Text = "";
            txtpoprpu.Text = "";
            txtpopstockqty.Text = "";
            txtpopconqty.Text = "";
            txtpop1itemcode.Text = "";
            txtpop1itemname.Text = "";
        }
        catch
        {
        }

    }

    protected void ddlhostelname_change(object sender, EventArgs e)
    {
        try
        {
            bindsession();
            loadmenuname();
        }
        catch
        {

        }
    }

    //protected void chkhostelname_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (chkhostelname.Checked == true)
    //    {
    //        for (int i = 0; i < chklsthostelname.Items.Count; i++)
    //        {
    //            chklsthostelname.Items[i].Selected = true;
    //        }
    //        txthostelname.Text = "Mess Name(" + (chklsthostelname.Items.Count) + ")";
    //    }
    //    else
    //    {
    //        for (int i = 0; i < chklsthostelname.Items.Count; i++)
    //        {
    //            chklsthostelname.Items[i].Selected = false;
    //        }
    //        txthostelname.Text = "--Select--";
    //    }
    //    bindsession();
    //}
    ////protected void chklsthostelname_IndexChanged(object sender, EventArgs e)
    //{
    //    txthostelname.Text = "--Select--";
    //    chkhostelname.Checked = false;
    //    int commcount = 0;
    //    for (int i = 0; i < chklsthostelname.Items.Count; i++)
    //    {
    //        if (chklsthostelname.Items[i].Selected == true)
    //        {
    //            commcount = commcount + 1;
    //        }
    //    }
    //    if (commcount > 0)
    //    {
    //        txthostelname.Text = "Mess Name(" + commcount.ToString() + ")";
    //        if (commcount == chklsthostelname.Items.Count)
    //        {
    //            chkhostelname.Checked = true;
    //        }
    //    }
    //    bindsession();
    //}
    //public void bindsession()
    //{
    //    try
    //    {
    //        ds.Clear();
    //        string itemheader = "";
    //        chklstsession.Items.Clear();
    //        for (int i = 0; i < chklsthostelname.Items.Count; i++)
    //        {
    //            if (chklsthostelname.Items[i].Selected == true)
    //            {
    //                if (itemheader == "")
    //                {
    //                    itemheader = "" + chklsthostelname.Items[i].Value.ToString() + "";
    //                }
    //                else
    //                {
    //                    itemheader = itemheader + "'" + "," + "" + "'" + chklsthostelname.Items[i].Value.ToString() + "";
    //                }
    //            }
    //        }
    //        if (itemheader.Trim() != "")
    //        {
    //            string selecthostel = "select distinct Session_Code,Session_Name  from Session_Master where Hostel_Code in ('" + chklsthostelname.SelectedItem.Value + "')";
    //            ds = d2.select_method_wo_parameter(selecthostel, "Text");
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                chklstsession.DataSource = ds;
    //                chklstsession.DataTextField = "Session_Name";
    //                chklstsession.DataValueField = "Session_Code";
    //                chklstsession.DataBind();
    //                if (chklstsession.Items.Count > 0)
    //                {
    //                    for (int i = 0; i < chklstsession.Items.Count; i++)
    //                    {
    //                        chklstsession.Items[i].Selected = true;
    //                    }
    //                    txtsessionname.Text = "Session Name(" + chklstsession.Items.Count + ")";
    //                }
    //            }
    //            else
    //            {
    //                txtsessionname.Text = "--Select--";
    //            }
    //        }
    //        else
    //        {
    //            txtsessionname.Text = "--Select--";
    //        }

    //    }
    //    catch
    //    {

    //    }
    //}

    public void bindsession()
    {
        try
        {
            ds.Clear();
            if (ddlhostelname.SelectedItem.Value.Trim() != "")
            {
                string selecthostel = "select distinct SessionMasterPK,SessionName  from HM_SessionMaster where MessMasterFK in ('" + ddlhostelname.SelectedItem.Value + "')";
                ds = d2.select_method_wo_parameter(selecthostel, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlsession.DataSource = ds;
                    ddlsession.DataTextField = "SessionName";
                    ddlsession.DataValueField = "SessionMasterPK";
                    ddlsession.DataBind();

                    //chklstsession.DataSource = ds;
                    //chklstsession.DataTextField = "SessionName";
                    //chklstsession.DataValueField = "SessionMasterPK";
                    //chklstsession.DataBind();
                    //if (chklstsession.Items.Count > 0)
                    //{
                    //    for (int i = 0; i < chklstsession.Items.Count; i++)
                    //    {
                    //        chklstsession.Items[i].Selected = true;
                    //    }
                    //    txtsessionname.Text = "Session Name(" + chklstsession.Items.Count + ")";
                    //}
                }
                else
                {
                    //txtsessionname.Text = "--Select--";
                }
            }
            else
            {
                //txtsessionname.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void bindhostelname()
    {
        try
        {
            ds.Clear();
            ddlhostelname.Items.Clear();

            //ds = d2.Bindmess_inv(collegecode1);
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlhostelname.DataSource = ds;
                ddlhostelname.DataTextField = "MessName";
                ddlhostelname.DataValueField = "MessMasterPK";
                ddlhostelname.DataBind();

                //if (chklsthostelname.Items.Count > 0)
                //{
                //    for (int i = 0; i < chklsthostelname.Items.Count; i++)
                //    {
                //        chklsthostelname.Items[i].Selected = true;
                //    }

                //    txthostelname.Text = "Mess Name(" + chklsthostelname.Items.Count + ")";
                //}
            }
            else
            {
                //txthostelname.Text = "--Select--";

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
            string degreedetails = "Daily Consumption-Item Return Entry";
            string pagename = "Return_item.aspx";
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

        }
        catch (Exception ex)
        {

        }
        loadmenuname();
    }
    protected void cbl_menutype_SelectIndexChange(object sender, EventArgs e)
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
        }
        catch (Exception ex)
        {
        }
        loadmenuname();
    }
}