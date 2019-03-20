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


public partial class inv_StockDetails : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string q = "";
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    DAccess2 dt = new DAccess2();
    bool check_value = false;
    bool check = false;
    string room = "";
    string build = "";
    string bul = "";
    string floor = "";
    string buildvalue = "";
    string flooor = "";
    string hostel = "";
    string building = "";

    string dtaccessdate = "";
    string dtaccesstime = "";

    string itemCode = "";
    string itemQuantity = "";
    string selectQuery = "";

    DAccess2 queryObject = new DAccess2();
    DAccess2 da = new DAccess2();

    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    int commcount;
    int i;
    int row;
    int cout;
    int netConnection = 0;
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
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            lbl_validation.Visible = false;

            if (!IsPostBack)
            {
                bindhostelname();
                bindbuild();
                bindfloor();
                bindroom();

                FpSpread2.Sheets[0].ColumnCount = 0;
                FpSpread2.Sheets[0].RowCount = 0;
                FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread2.CommandBar.Visible = false;
                FpSpread2.SheetCorner.ColumnCount = 0;
                FpSpread2.Sheets[0].AutoPostBack = false;
                FpSpread2.Sheets[0].RowHeader.Visible = false;


                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.Visible = false;
                divColOrder.Visible = false;
                div1.Visible = false;

                FpSpread2.Sheets[0].RowCount = 0;
                FpSpread2.Sheets[0].ColumnCount = 0;

                FpSpread3.Sheets[0].RowCount = 0;
                FpSpread3.Sheets[0].ColumnCount = 0;
                FpSpread3.Visible = false;

                loadheadername();
                loaditem();
                txt_searchby.Visible = true;
                ddl_type3.SelectedIndex = 0;

                cb_column.Checked = true;
                cb_column_CheckedChanged(sender, e);
                btn_go_Click(sender, e);

                btn_save.Visible = false;
                btn_update.Visible = false;
                btn_delete.Visible = false;
                lbl_error.Visible = false;
                ViewState["itempk"] = null;
            }
        }
        catch (Exception ex) { }
    }


    // main page

    public void bindhostelname()
    {
        try
        {
            ds.Clear();
            cbl_hostel.Items.Clear();
            //ds = queryObject.BindHostel_inv(collegecode1);
            //string itemname = "select HostelMasterPK,HostelName from HM_HostelMaster  order by HostelName ";
            //ds.Clear();
            //ds = queryObject.select_method_wo_parameter(itemname, "Text");

            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostel.DataSource = ds;
                cbl_hostel.DataTextField = "HostelName";
                cbl_hostel.DataValueField = "HostelMasterPK";
                cbl_hostel.DataBind();

                if (cbl_hostel.Items.Count > 0)
                {
                    for (i = 0; i < cbl_hostel.Items.Count; i++)
                    {
                        cbl_hostel.Items[i].Selected = true;
                    }
                    txt_hostelname.Text = "Hostel Name(" + cbl_hostel.Items.Count + ")";
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
    protected void cb_hostel_OnCheckedChanged(object sender, EventArgs e)
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
            txt_hostelname.Text = "Hostel Name(" + (cbl_hostel.Items.Count) + ")";
        }
        else
        {
            for (i = 0; i < cbl_hostel.Items.Count; i++)
            {
                cbl_hostel.Items[i].Selected = false;
            }
        }
        bindbuild();
        bindfloor();
        bindroom();
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
            txt_hostelname.Text = "Hostel Name(" + commcount.ToString() + ")";
        }
        bindbuild();
        bindfloor();
        bindroom();
    }

    public void bindbuild()
    {
        try
        {
            ds.Clear();
            cbl_building.Items.Clear();
            txt_building.Text = "---Select---";
            cb_building.Checked = false;
            build = "";
            if (cbl_hostel.Items.Count > 0)
            {
                for (i = 0; i < cbl_hostel.Items.Count; i++)
                {
                    if (cbl_hostel.Items[i].Selected == true)
                    {
                        if (build == "")
                        {
                            build = Convert.ToString(cbl_hostel.Items[i].Value);
                        }
                        else
                        {
                            build = build + "'" + "," + "'" + Convert.ToString(cbl_hostel.Items[i].Value);
                        }
                    }
                }
            }
            bul = "";
            if (build != "")
            {
                bul = queryObject.GetBuildingCode_inv(build);
                ds = queryObject.BindBuilding(bul);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_building.DataSource = ds;
                    cbl_building.DataTextField = "Building_Name";
                    cbl_building.DataValueField = "code";
                    cbl_building.DataBind();
                    if (cbl_building.Items.Count > 0)
                    {
                        for (row = 0; row < cbl_building.Items.Count; row++)
                        {
                            cbl_building.Items[row].Selected = true;
                        }
                        txt_building.Text = "Building Name(" + cbl_building.Items.Count + ")";
                        cb_building.Checked = true;
                    }
                }
            }

        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_building_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            cout = 0;
            txt_building.Text = "--Select--";
            if (cb_building.Checked == true)
            {
                cout++;
                for (i = 0; i < cbl_building.Items.Count; i++)
                {
                    cbl_building.Items[i].Selected = true;
                }
                txt_building.Text = "Building Name(" + (cbl_building.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_building.Items.Count; i++)
                {
                    cbl_building.Items[i].Selected = false;
                }
            }

            bindfloor();
            bindroom();

        }
        catch
        {
        }
    }
    protected void cbl_building_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        i = 0;
        cb_building.Checked = false;
        commcount = 0;
        buildvalue = "";
        build = "";
        txt_building.Text = "--Select--";
        for (i = 0; i < cbl_building.Items.Count; i++)
        {
            if (cbl_building.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_building.Checked = false;
                build = cbl_building.Items[i].Text.ToString();
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
        if (commcount > 0)
        {
            if (commcount == cbl_building.Items.Count)
            {
                cb_building.Checked = true;
            }
            txt_building.Text = "Building Name(" + commcount.ToString() + ")";
        }
        bindfloor();
        bindroom();

    }

    public void bindfloor()
    {
        try
        {
            ds.Clear();
            cbl_floor.Items.Clear();
            txt_floor.Text = "---Select---";
            cb_floor.Checked = false;
            floor = "";
            if (cbl_building.Items.Count > 0)
            {
                for (i = 0; i < cbl_building.Items.Count; i++)
                {
                    if (cbl_building.Items[i].Selected == true)
                    {
                        if (floor == "")
                        {
                            floor = Convert.ToString(cbl_building.Items[i].Text);
                        }
                        else
                        {
                            floor = floor + "'" + "," + "'" + Convert.ToString(cbl_building.Items[i].Text);
                        }
                    }
                }
            }
            if (floor != "")
            {
                ds = queryObject.BindFloor_new(floor);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_floor.DataSource = ds;
                    cbl_floor.DataTextField = "Floor_Name";
                    cbl_floor.DataValueField = "FloorPK";
                    cbl_floor.DataBind();
                    if (cbl_floor.Items.Count > 0)
                    {
                        for (row = 0; row < cbl_floor.Items.Count; row++)
                        {
                            cbl_floor.Items[row].Selected = true;
                        }
                        txt_floor.Text = "Floor Name(" + cbl_floor.Items.Count + ")";
                        cb_floor.Checked = true;
                    }
                }
            }

        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_floor_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            cout = 0;
            txt_floor.Text = "--Select--";
            if (cb_floor.Checked == true)
            {
                cout++;
                for (i = 0; i < cbl_floor.Items.Count; i++)
                {
                    cbl_floor.Items[i].Selected = true;
                }
                txt_floor.Text = "Floor Name(" + (cbl_floor.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_floor.Items.Count; i++)
                {
                    cbl_floor.Items[i].Selected = false;
                }
            }

            bindroom();
        }
        catch
        {
        }
    }
    protected void cbl_floor_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        i = 0;
        cb_floor.Checked = false;
        commcount = 0;
        txt_floor.Text = "---Select---";
        for (i = 0; i < cbl_floor.Items.Count; i++)
        {
            if (cbl_floor.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_floor.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_floor.Items.Count)
            {
                cb_floor.Checked = true;
            }
            txt_floor.Text = "Floor Name(" + commcount.ToString() + ")";
        }
        bindroom();

    }

    public void bindroom()
    {
        try
        {
            ds.Clear();
            cbl_room.Items.Clear();
            txt_room.Text = "---Select---";
            cb_room.Checked = false;
            flooor = "";
            room = "";
            if (cbl_building.Items.Count > 0)
            {
                for (i = 0; i < cbl_building.Items.Count; i++)
                {
                    if (cbl_building.Items[i].Selected == true)
                    {
                        if (flooor == "")
                        {
                            flooor = Convert.ToString(cbl_building.Items[i].Text);
                        }
                        else
                        {
                            flooor = flooor + "'" + "," + "'" + Convert.ToString(cbl_building.Items[i].Text);
                        }
                    }
                }
            }
            if (cbl_floor.Items.Count > 0)
            {
                for (i = 0; i < cbl_floor.Items.Count; i++)
                {
                    if (cbl_floor.Items[i].Selected == true)
                    {
                        if (room == "")
                        {
                            room = Convert.ToString(cbl_floor.Items[i].Text);
                        }
                        else
                        {
                            room = room + "'" + "," + "'" + Convert.ToString(cbl_floor.Items[i].Text);
                        }
                    }
                }
            }

            if (flooor != "" && room != "")
            {
                ds = queryObject.BindRoom(room, flooor);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_room.DataSource = ds;
                    cbl_room.DataTextField = "Room_Name";
                    cbl_room.DataValueField = "Roompk";
                    cbl_room.DataBind();

                    if (cbl_room.Items.Count > 0)
                    {
                        for (row = 0; row < cbl_room.Items.Count; row++)
                        {
                            cbl_room.Items[row].Selected = true;
                        }
                        txt_room.Text = "Room Name(" + cbl_room.Items.Count + ")";
                        cb_room.Checked = true;
                    }
                }
            }

        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_room_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            cout = 0;
            txt_room.Text = "--Select--";
            if (cb_room.Checked == true)
            {
                cout++;
                for (i = 0; i < cbl_room.Items.Count; i++)
                {
                    cbl_room.Items[i].Selected = true;
                }
                txt_room.Text = "Room Type(" + (cbl_room.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_room.Items.Count; i++)
                {
                    cbl_room.Items[i].Selected = false;
                }
            }
        }
        catch
        {
        }
    }
    protected void cbl_room_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        cb_room.Checked = false;
        commcount = 0;
        txt_room.Text = "--Select--";
        for (i = 0; i < cbl_room.Items.Count; i++)
        {
            if (cbl_room.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_room.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_room.Items.Count)
            {
                cb_room.Checked = true;
            }
            txt_room.Text = "Room Type(" + commcount.ToString() + ")";
        }
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {

        hostel = "";
        building = "";
        floor = "";
        room = "";
        Printmaster1.Visible = false;
        for (i = 0; i < cbl_hostel.Items.Count; i++)
        {
            if (cbl_hostel.Items[i].Selected)
            {
                if (hostel == "")
                {
                    hostel = "" + cbl_hostel.Items[i].Value.ToString();
                }
                else
                {
                    hostel += "','" + cbl_hostel.Items[i].Value.ToString() + "";
                }
            }
        }

        for (i = 0; i < cbl_building.Items.Count; i++)
        {
            if (cbl_building.Items[i].Selected)
            {
                if (building == "")
                {
                    building = "" + cbl_building.Items[i].Value.ToString();
                }
                else
                {
                    building += "','" + cbl_building.Items[i].Value.ToString() + "";
                }
            }
        }

        for (i = 0; i < cbl_floor.Items.Count; i++)
        {
            if (cbl_floor.Items[i].Selected)
            {
                if (floor == "")
                {
                    floor = "" + cbl_floor.Items[i].Value.ToString();
                }
                else
                {
                    floor += "','" + cbl_floor.Items[i].Value.ToString() + "";
                }
            }
        }
        for (i = 0; i < cbl_room.Items.Count; i++)
        {
            if (cbl_room.Items[i].Selected)
            {
                if (room == "")
                {
                    room = "" + cbl_room.Items[i].Value.ToString();
                }
                else
                {
                    room += "','" + cbl_room.Items[i].Value.ToString() + "";
                }
            }
        }

        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].ColumnCount = 0;

        #region without column order
        //string selectQuery = "select h.Hostel_Code,h.Hostel_Name,Building_Name,Floor_Name,Room_Name,case when Net_Connection='0' then 'No' when Net_Connection='1' then 'Yes' end as Net_Connection,i.item_name,i.item_code ,rs.Qty   from RoomStock_Master rm,Hostel_Details h ,RoomStock_Detail rs,item_master i where rm.Hostel_Code =h.Hostel_code and rm.RoomStockMaster_Code =rs.RoomStockMaster_Code and rs.Item_Code =i.item_code and rs.Hostel_Code =rm.Hostel_Code and h.Hostel_code in('" + hostel + "') and Building_Name in ('" + building + "') and Floor_Name in ('" + floor + "') and Room_Name in ('" + room + "')";

        //ds.Clear();
        //ds = d2.select_method_wo_parameter(selectQuery, "Text");

        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    FpSpread1.Sheets[0].RowCount = 0;
        //    FpSpread1.Sheets[0].ColumnCount = 0;
        //    FpSpread1.CommandBar.Visible = false;
        //    FpSpread1.Sheets[0].AutoPostBack = true;
        //    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
        //    FpSpread1.Sheets[0].RowHeader.Visible = false;
        //    FpSpread1.Sheets[0].ColumnCount = 8;
        //    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        //    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        //    darkstyle.ForeColor = Color.White;
        //    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
        //    FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
        //    FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
        //    FpSpread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
        //    FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);

        //    FpSpread1.Sheets[0].SetColumnWidth(0, 50);
        //    //FpSpread1.Sheets[0].SetColumnWidth(1, 100);
        //    //FpSpread1.Sheets[0].SetColumnWidth(2, 100);
        //    //FpSpread1.Sheets[0].SetColumnWidth(3, 90);
        //    //FpSpread1.Sheets[0].SetColumnWidth(4, 100);
        //    //FpSpread1.Sheets[0].SetColumnWidth(5, 90);
        //    FpSpread1.Sheets[0].SetColumnWidth(6, 250);
        //    //FpSpread1.Sheets[0].SetColumnWidth(7, 100);

        //    FpSpread1.Columns[0].Locked = true;
        //    FpSpread1.Columns[1].Locked = true;
        //    FpSpread1.Columns[2].Locked = true;
        //    FpSpread1.Columns[3].Locked = true;
        //    FpSpread1.Columns[4].Locked = true;
        //    FpSpread1.Columns[5].Locked = true;


        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;

        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Hostel Name";
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Building Name";
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Floor Name";
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Room Name";
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;

        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Net Connection";
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;

        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Item Name";
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;

        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Quantity";
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;

        //    for (row = 0; row < ds.Tables[0].Rows.Count; row++)
        //    {
        //        FpSpread1.Sheets[0].RowCount++;

        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["Hostel_Name"]);
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Hostel_Code"]);
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Building_Name"]);
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Floor_Name"]);
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["Room_Name"]);
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["Net_Connection"]);
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["item_name"]);
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(ds.Tables[0].Rows[row]["item_code"]);
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[row]["Qty"]);
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
        //    }

        #endregion
        Hashtable columnhash = new Hashtable();
        columnhash.Add("HostelName", "Hostel Name");
        columnhash.Add("Building_Name", "Building Name");
        columnhash.Add("Floor_Name", "Floor Name");
        columnhash.Add("Room_Name", "Room Name");
        columnhash.Add("Net_Connection", "Net Connection");
        columnhash.Add("item_count", "Total no of Item");

        if (ItemList.Count == 0)
        {
            ItemList.Add("HostelName");
            ItemList.Add("Building_Name");
            ItemList.Add("Floor_Name");
            ItemList.Add("Room_Name");
        }

        if (hostel.Trim() != "" && building.Trim() != "" && floor.Trim() != "" && room.Trim() != "")
        {
            //string selectquery = "select h.Hostel_Name,Building_Name,Floor_Name,Room_Name,case when Net_Connection='0' then 'No' when Net_Connection='1' then 'Yes' end as Net_Connection ,count(i.item_code) as item_count,h.Hostel_code from RoomStock_Master rm,Hostel_Details h ,RoomStock_Detail rs,item_master i where rm.Hostel_Code =h.Hostel_code and rm.RoomStockMaster_Code= rs.RoomStockMaster_Code and rs.Item_Code =i.item_code and rs.Hostel_Code =rm.Hostel_Code and h.Hostel_code in  ('" + hostel + "') and Building_Name in ('" + building + "') and Floor_Name in ('" + floor + "') and Room_Name in ('" + room + "') group by h.Hostel_Name ,Building_Name,Floor_Name,Room_Name,Net_Connection,h.Hostel_code order by h.Hostel_code";

            // string selectquery = "select h.HostelName,h.HostelMasterPK,Building_Name,Floor_Name,Room_Name,case when Net_Connection='0' then 'No' when Net_Connection='1' then 'Yes' end as Net_Connection ,count(i.ItemPK) as item_count,h.HostelMasterPK  from RoomStock_Master rm,HM_HostelMaster h ,RoomStock_Detail rs,IM_ItemMaster i where rm.Hostel_Code =h.HostelMasterPK  and rm.RoomStockMaster_Code= rs.RoomStockMaster_Code and rs.Item_Code =i.ItemPK  and rs.Hostel_Code =rm.Hostel_Code and h.HostelMasterPK in  ('" + hostel + "') and Building_Name in ('" + building + "') and Floor_Name in ('" + floor + "') and Room_Name in ('" + room + "') group by h.HostelMasterPK ,Building_Name,Floor_Name,Room_Name,Net_Connection,HostelName  order by h.HostelMasterPK ";

            string selectquery = "select h.HostelName,h.HostelMasterPK,bm.Building_Name,fm.Floor_Name,rd.Room_Name,rm.Building_Name,rm.Floor_Name,rm.Room_Name,case when Net_Connection='0' then 'No' when Net_Connection='1' then 'Yes' end as Net_Connection ,count(i.ItemPK) as item_count,h.HostelMasterPK  from RoomStock_Master rm,HM_HostelMaster h ,RoomStock_Detail rs,IM_ItemMaster i,Building_Master bm,Floor_Master fm,Room_Detail rd where rm.Hostel_Code =h.HostelMasterPK  and rm.RoomStockMaster_Code= rs.RoomStockMaster_Code and rs.Item_Code =i.ItemPK and bm.Code=rm.Building_Name and rm.Room_Name =rd.RoomPk and fm.Floorpk =rm.Floor_Name and rs.Hostel_Code =rm.Hostel_Code and h.HostelMasterPK in  ('" + hostel + "') and rm.Building_Name in ('" + building + "') and rm.Floor_Name in ('" + floor + "') and rm.Room_Name in ('" + room + "') group by h.HostelMasterPK ,bm.Building_Name,fm.Floor_Name,rd.Room_Name, Net_Connection,h.HostelName,rm.Building_Name,rm.Floor_Name,rm.Room_Name order by h.HostelMasterPK ";

            ds.Clear();
            ds = da.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.CommandBar.Visible = false;
                FpSpread1.Sheets[0].AutoPostBack = true;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FpSpread1.Sheets[0].ColumnCount = ItemList.Count + 1;
                FpSpread1.Sheets[0].RowCount = ds.Tables[0].Rows.Count;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[0].Width = 50;

                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                //FpSpread1.Sheets[0].Columns[1].Width = 50;

                //FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                //cb.AutoPostBack = true;

                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                //FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                //for (int i = 0; i < Itemindex.Count;i++ )
                //{
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    string colno = Convert.ToString(ds.Tables[0].Columns[j]);
                    if (ItemList.Contains(Convert.ToString(colno)))
                    {
                        int insdex = ItemList.IndexOf(Convert.ToString(colno));

                        FpSpread1.Columns[insdex + 1].Width = 100;
                        insdex = ItemList.IndexOf(Convert.ToString(colno));
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, insdex + 1].Text = Convert.ToString(columnhash[colno]);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, insdex + 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, insdex + 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, insdex + 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, insdex + 1].HorizontalAlign = HorizontalAlign.Center;
                        if (colno == "Building_Name")
                        {
                            FpSpread1.Columns[insdex + 1].Width = 80;
                        }
                        if (colno == "Floor_Name")
                        {
                            FpSpread1.Columns[insdex + 1].Width = 50;
                        }
                        if (colno == "Room_Name")
                        {
                            FpSpread1.Columns[insdex + 1].Width = 50;
                        }

                        if (colno == "item_count")
                        {
                            FpSpread1.Columns[insdex + 1].Width = 30;
                        }
                    }
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    FpSpread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (ItemList.Contains(Convert.ToString(ds.Tables[0].Columns[j].ToString())))
                        {
                            int insdex = ItemList.IndexOf(Convert.ToString(ds.Tables[0].Columns[j].ToString()));
                            FpSpread1.Sheets[0].Cells[i, insdex + 1].Text = ds.Tables[0].Rows[i][j].ToString();
                            FpSpread1.Sheets[0].Cells[i, insdex + 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[i, insdex + 1].Font.Size = FontUnit.Medium;
                            //FpSpread1.Columns[insdex].Width = 150;

                            string colname = Convert.ToString(ds.Tables[0].Columns[j]);
                            if (colname == "HostelName")
                            {
                                FpSpread1.Sheets[0].Cells[i, insdex + 1].Tag = ds.Tables[0].Rows[i]["HostelMasterPK"].ToString();
                            }
                            if (ds.Tables[0].Columns[j].ToString() == "Building_Name")
                            {
                                FpSpread1.Sheets[0].Cells[i, insdex + 1].HorizontalAlign = HorizontalAlign.Center;

                                FpSpread1.Sheets[0].Cells[i, insdex + 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Building_Name1"]);
                            }

                            if (ds.Tables[0].Columns[j].ToString() == "Floor_Name")
                            {
                                FpSpread1.Sheets[0].Cells[i, insdex + 1].HorizontalAlign = HorizontalAlign.Center;

                                FpSpread1.Sheets[0].Cells[i, insdex + 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Floor_Name1"]);
                            }

                            if (ds.Tables[0].Columns[j].ToString() == "Room_Name")
                            {
                                FpSpread1.Sheets[0].Cells[i, insdex + 1].HorizontalAlign = HorizontalAlign.Center;

                                FpSpread1.Sheets[0].Cells[i, insdex + 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Room_Name1"]);
                            }
                            if (ds.Tables[0].Columns[j].ToString() == "Net_Connection")
                            {
                                FpSpread1.Sheets[0].Cells[i, insdex + 1].HorizontalAlign = HorizontalAlign.Center;

                            }
                            if (ds.Tables[0].Columns[j].ToString() == "item_count")
                            {
                                FpSpread1.Sheets[0].Cells[i, insdex + 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                }
                FpSpread1.Visible = true;
                divColOrder.Visible = true;
                div1.Visible = true;
                rptprint.Visible = true;
                lbl_error.Visible = false;
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            }
            else
            {
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.Visible = false;
                divColOrder.Visible = false;
                div1.Visible = false;
                rptprint.Visible = false;
                lbl_error.Text = "No Records Found";
                lbl_error.Visible = true;
            }
        }
        else
        {
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Visible = false;
            divColOrder.Visible = false;
            div1.Visible = false;
            rptprint.Visible = false;
            lbl_error.Text = "No Records Found";
            lbl_error.Visible = true;
        }

    }
    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        try
        {
            bindhostelnameDDl();
            bindhostelname2();
            clgbuild2();
            clgfloor2();
            clgroom2();

            loadSpread2();
            btn_additem1.Enabled = true;

            txt_item.Text = "";
            txt_itemCode.Text = "";
            txt_quant.Text = "";
            cb_netcon.Checked = false;

            popwindow1.Visible = true;
            btn_save.Visible = true;
            btn_update.Visible = false;
            btn_delete.Visible = false;
            rdbl_selection.Visible = true;


            ddl_hostelname2.Enabled = true;
            ddl_building2.Enabled = true;
            ddl_floor2.Enabled = true;
            ddl_room2.Enabled = true;

            txt_hostelname2.Enabled = true;
            txt_building2.Enabled = true;
            txt_floor2.Enabled = true;
            txt_room2.Enabled = true;
        }
        catch
        {
        }
    }

    //popupwindow1

    public void clgbuild2()
    {
        if (rdbl_selection.SelectedIndex == 1)
        {
            try
            {
                build = "";
                bul = "";
                ds.Clear();
                cbl_building2.Items.Clear();
                txt_building2.Text = "---Select---";
                build = Convert.ToString(ddl_hostelname2.SelectedValue.ToString());
                if (build != "")
                {
                    bul = queryObject.GetBuildingCode_inv(build);
                    ds = queryObject.BindBuilding(bul);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cbl_building2.DataSource = ds;
                        cbl_building2.DataTextField = "Building_Name";
                        cbl_building2.DataValueField = "code";
                        cbl_building2.DataBind();
                        if (cbl_building.Items.Count > 0)
                        {
                            for (row = 0; row < cbl_building2.Items.Count; row++)
                            {
                                cbl_building2.Items[row].Selected = true;
                            }
                            txt_building2.Text = "Building Name(" + cbl_building2.Items.Count + ")";
                            cb_building2.Checked = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }
        else
        {
            //New Code
            try
            {
                build = "";
                bul = "";
                ddl_building2.Items.Clear();
                build = Convert.ToString(ddl_hostelname2.SelectedValue.ToString());
                build = Convert.ToString(build);
                if (build != "")
                {
                    bul = queryObject.GetBuildingCode_inv(build);
                    ds = queryObject.BindBuilding(bul);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddl_building2.DataSource = ds;
                        ddl_building2.DataTextField = "Building_Name";
                        ddl_building2.DataValueField = "code";
                        ddl_building2.DataBind();
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }
    }
    protected void cb_building2_OnCheckedChanged(object sender, EventArgs e)
    {

        try
        {
            cout = 0;
            txt_building2.Text = "--Select--";

            if (cb_building2.Checked == true)
            {
                cout++;
                for (i = 0; i < cbl_building2.Items.Count; i++)
                {
                    cbl_building2.Items[i].Selected = true;
                }
                txt_building2.Text = "Building Name(" + (cbl_building2.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_building2.Items.Count; i++)
                {
                    cbl_building2.Items[i].Selected = false;
                }
            }
            clgfloor2();
            clgroom2();
        }
        catch
        {
        }
    }
    protected void cbl_building2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdbl_selection.SelectedIndex == 1)
        {
            i = 0;
            cb_building2.Checked = false;
            commcount = 0;

            txt_building2.Text = "--Select--";
            for (i = 0; i < cbl_building2.Items.Count; i++)
            {
                if (cbl_building2.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_building2.Checked = false;

                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_building2.Items.Count)
                {
                    cb_building2.Checked = true;
                }
                txt_building2.Text = "Building Name(" + commcount.ToString() + ")";
            }
            clgfloor2();
            clgroom2();
        }
        else
        {
            clgfloor2();
            clgroom2();

        }
    }
    protected void ddl_building2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        clgfloor2();
        clgroom2();

    }

    public void clgfloor2()
    {
        if (rdbl_selection.SelectedIndex == 1)
        {
            ds.Clear();
            cbl_floor2.Items.Clear();
            txt_floor2.Text = "---Select---";
            cb_floor2.Checked = false;
            build = "";
            try
            {
                if (cbl_building2.Items.Count > 0)
                {
                    for (i = 0; i < cbl_building2.Items.Count; i++)
                    {
                        if (cbl_building2.Items[i].Selected == true)
                        {
                            if (build == "")
                            {
                                build = Convert.ToString(cbl_building2.Items[i].Text);
                            }
                            else
                            {
                                build = build + "'" + "," + "'" + Convert.ToString(cbl_building2.Items[i].Text);
                            }
                        }
                    }
                }

                if (build != "")
                {
                    ds = queryObject.BindFloor_new(build);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cbl_floor2.DataSource = ds;
                        cbl_floor2.DataTextField = "Floor_Name";
                        cbl_floor2.DataValueField = "FloorPK";
                        cbl_floor2.DataBind();
                        if (cbl_floor2.Items.Count > 0)
                        {
                            for (row = 0; row < cbl_floor2.Items.Count; row++)
                            {
                                cbl_floor2.Items[row].Selected = true;
                            }
                            txt_floor2.Text = "Floor Name(" + cbl_floor2.Items.Count + ")";
                            cb_floor2.Checked = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }
        else
        {
            //New Code
            try
            {
                ddl_floor2.Items.Clear();
                build = "";
                build = Convert.ToString(ddl_building2.SelectedItem.ToString());
                if (build != "")
                {
                    ds = queryObject.BindFloor_new(build);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddl_floor2.DataSource = ds;
                        ddl_floor2.DataTextField = "Floor_Name";
                        ddl_floor2.DataValueField = "FloorPK";
                        ddl_floor2.DataBind();
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }
    }
    protected void cb_floor2_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            cout = 0;
            txt_floor2.Text = "--Select--";
            if (cb_floor2.Checked == true)
            {
                cout++;
                for (i = 0; i < cbl_floor2.Items.Count; i++)
                {
                    cbl_floor2.Items[i].Selected = true;
                }
                txt_floor2.Text = "Floor Name(" + (cbl_floor2.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_floor2.Items.Count; i++)
                {
                    cbl_floor2.Items[i].Selected = false;
                }
            }
            clgroom2();
        }
        catch
        {
        }
    }
    protected void cbl_floor2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdbl_selection.SelectedIndex == 1)
        {

            cb_floor2.Checked = false;
            txt_floor2.Text = "---Select---";

            i = 0;
            commcount = 0;
            txt_floor2.Text = "--Select--";

            for (i = 0; i < cbl_floor2.Items.Count; i++)
            {
                if (cbl_floor2.Items[i].Selected == true)
                {
                    commcount = commcount + 1;

                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_floor2.Items.Count)
                {
                    cb_floor2.Checked = true;
                }
                txt_floor2.Text = "Floor Name(" + commcount.ToString() + ")";
            }
            clgroom2();
        }
        else
        {
            clgroom2();
        }
    }
    protected void ddl_floor2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        clgroom2();
    }

    public void clgroom2()
    {
        build = "";
        floor = "";
        if (rdbl_selection.SelectedIndex == 1)
        {
            try
            {
                ds.Clear();
                cbl_room2.Items.Clear();
                txt_room2.Text = "---Select---";
                cb_room2.Checked = false;
                if (cbl_building2.Items.Count > 0)
                {
                    for (i = 0; i < cbl_building2.Items.Count; i++)
                    {
                        if (cbl_building2.Items[i].Selected == true)
                        {
                            if (build == "")
                            {
                                build = Convert.ToString(cbl_building2.Items[i].Text);
                            }
                            else
                            {
                                build = build + "'" + "," + "'" + Convert.ToString(cbl_building2.Items[i].Text);
                            }
                        }
                    }
                }
                if (cbl_floor2.Items.Count > 0)
                {
                    for (i = 0; i < cbl_floor2.Items.Count; i++)
                    {
                        if (cbl_floor2.Items[i].Selected == true)
                        {
                            if (floor == "")
                            {
                                floor = Convert.ToString(cbl_floor2.Items[i].Text);
                            }
                            else
                            {
                                floor = floor + "'" + "," + "'" + Convert.ToString(cbl_floor2.Items[i].Text);
                            }
                        }
                    }
                }
                if (build != "" && floor != "")
                {
                    ds = queryObject.BindRoom(floor, build);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cbl_room2.DataSource = ds;
                        cbl_room2.DataTextField = "room_name";
                        cbl_room2.DataValueField = "Roompk";
                        cbl_room2.DataBind();
                        if (cbl_room2.Items.Count > 0)
                        {
                            for (row = 0; row < cbl_room2.Items.Count; row++)
                            {
                                cbl_room2.Items[row].Selected = true;
                            }
                            txt_room2.Text = "Room Name(" + cbl_room2.Items.Count + ")";
                            cb_room2.Checked = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }
        else
        {

            //New Code
            try
            {
                ddl_room2.Items.Clear();
                build = Convert.ToString(ddl_building2.SelectedItem.ToString());
                floor = Convert.ToString(ddl_floor2.SelectedItem.ToString());
                if (build != "" && floor != "")
                {

                    ds = queryObject.BindRoom(floor, build);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddl_room2.DataSource = ds;
                        ddl_room2.DataTextField = "room_name";
                        ddl_room2.DataValueField = "Roompk";
                        ddl_room2.DataBind();
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }
    }
    protected void cb_room2_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            cout = 0;
            txt_room2.Text = "--Select--";
            if (cb_room2.Checked == true)
            {
                cout++;
                for (i = 0; i < cbl_room2.Items.Count; i++)
                {
                    cbl_room2.Items[i].Selected = true;
                }
                txt_room2.Text = "Room Type(" + (cbl_room2.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_room2.Items.Count; i++)
                {
                    cbl_room2.Items[i].Selected = false;
                }
            }
        }
        catch
        {
        }
    }
    protected void cbl_room2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        cb_room2.Checked = false;
        commcount = 0;

        txt_room2.Text = "--Select--";
        for (i = 0; i < cbl_room2.Items.Count; i++)
        {
            if (cbl_room2.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_room2.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_building2.Items.Count)
            {
                cb_room2.Checked = true;
            }
            txt_room2.Text = "Room Type(" + commcount.ToString() + ")";
        }
    }

    public void bindhostelname2()
    {
        try
        {
            ds.Clear();
            txt_hostelname2.Text = "--Select--";
            cb_hostel2.Checked = true;

            cbl_hostel2.Items.Clear();
            //string itemname = "select HostelMasterPK,HostelName from HM_HostelMaster  order by HostelName ";
            //ds.Clear();
            //ds =queryObject.select_method_wo_parameter(itemname, "Text");
            //ds = queryObject.BindHostel_inv(collegecode1);


            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = queryObject.BindHostelbaseonmessrights_inv(MessmasterFK);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostel2.DataSource = ds;
                cbl_hostel2.DataTextField = "HostelName";
                cbl_hostel2.DataValueField = "HostelMasterPK";
                cbl_hostel2.DataBind();

                if (cbl_hostel2.Items.Count > 0)
                {
                    for (i = 0; i < cbl_hostel2.Items.Count; i++)
                    {
                        cbl_hostel2.Items[i].Selected = true;
                    }
                    txt_hostelname2.Text = "Hostel Name(" + cbl_hostel2.Items.Count + ")";
                }
            }
        }
        catch
        {
        }
    }
    public void bindhostelnameDDl()
    {
        try
        {
            ds.Clear();
            ddl_hostelname2.Items.Clear();
            //ds = queryObject.BindHostel_inv(collegecode1);
            //string itemname = "select HostelMasterPK,HostelName from HM_HostelMaster  order by HostelName ";
            //ds.Clear();
            //ds = queryObject.select_method_wo_parameter(itemname, "Text");

            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_hostelname2.DataSource = ds;
                ddl_hostelname2.DataTextField = "HostelName";
                ddl_hostelname2.DataValueField = "HostelMasterPK";
                ddl_hostelname2.DataBind();
            }
        }
        catch
        {
        }
    }
    protected void cb_hostel2_OnCheckedChanged(object sender, EventArgs e)
    {
        txt_hostelname2.Text = "--Select--";
        if (cb_hostel2.Checked == true)
        {
            for (i = 0; i < cbl_hostel2.Items.Count; i++)
            {
                cbl_hostel2.Items[i].Selected = true;
            }
            txt_hostelname2.Text = "Hostel Name(" + (cbl_hostel2.Items.Count) + ")";
        }
        else
        {
            for (i = 0; i < cbl_hostel2.Items.Count; i++)
            {
                cbl_hostel2.Items[i].Selected = false;
            }
        }
        clgbuild2();
    }
    protected void cbl_hostel2_SelectedIndexChanged(object sender, EventArgs e)
    {
        clgbuild2();
    }
    protected void ddl_hostelname2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        clgbuild2();
        clgfloor2();
        clgroom2();
    }

    protected void btn_go1_Click(object sender, EventArgs e)
    {
        try
        {
            string itemheadercode = "";
            for (i = 0; i < cbl_itemheader3.Items.Count; i++)
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
            string itemcode = "";
            for (i = 0; i < cbl_itemname3.Items.Count; i++)
            {
                if (cbl_itemname3.Items[i].Selected == true)
                {
                    if (itemcode == "")
                    {
                        itemcode = "" + cbl_itemname3.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemcode = itemcode + "'" + "," + "'" + cbl_itemname3.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemcode.Trim() != "" && itemheadercode.Trim() != "")
            {
                string selectquery = "";

                if (txt_searchby.Text.Trim() != "")
                {
                    selectquery = "  select itemheadername,itemheadercode,itemcode,itempk,itemname ,ItemModel ,ItemSize ,itemunit,ItemSpecification from IM_ItemMaster  where itemname='" + txt_searchby.Text + "' order by itemcode";

                    //selectquery = "select itemheader_name,itemheader_code,item_code,item_name ,model_name,Size_name ,item_unit,description ,special_instru from Item_Master where item_name='" + txt_searchby.Text + "' order by item_code";
                }
                else if (txt_searchitemcode.Text.Trim() != "")
                {
                    selectquery = "select itemheadername,itemheadercode,itemcode,itemname,itempk ,ItemModel ,ItemSize ,itemunit,ItemSpecification from IM_ItemMaster where itemcode='" + txt_searchitemcode.Text + "' order by itemcode";
                }
                else if (txt_searchheadername.Text.Trim() != "")
                {
                    selectquery = "select itemheadername,itemheadercode,itemcode,itemname ,itempk,ItemModel ,ItemSize ,itemunit,ItemSpecification from IM_ItemMaster where itemheadername='" + txt_searchheadername.Text + "' order by itemcode";
                }
                else
                {
                    selectquery = "select itemheadername,itemheadercode,itemcode,itemname ,itempk,ItemModel ,ItemSize ,itemunit,ItemSpecification from IM_ItemMaster where itemheadercode in ('" + itemheadercode + "') and itemcode in('" + itemcode + "') order by itemcode";
                }

                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    FpSpread3.Sheets[0].RowCount = 0;
                    FpSpread3.Sheets[0].ColumnCount = 0;
                    FpSpread3.CommandBar.Visible = false;
                    FpSpread3.Sheets[0].AutoPostBack = false;

                    FpSpread3.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread3.Sheets[0].RowHeader.Visible = false;
                    FpSpread3.Sheets[0].ColumnCount = 9;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    FpSpread3.Sheets[0].Columns[0].Width = 50;
                    FpSpread3.Columns[0].Locked = true;

                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Item Header";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[1].Width = 150;
                    FpSpread3.Columns[1].Locked = true;

                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Code";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[2].Width = 80;
                    FpSpread3.Columns[2].Locked = true;

                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Item Name";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[3].Width = 100;
                    FpSpread3.Columns[3].Locked = true;

                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Model";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[4].Width = 92;
                    FpSpread3.Columns[4].Locked = true;

                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Size";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[5].Width = 50;
                    FpSpread3.Columns[5].Locked = true;

                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Unit";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[6].Width = 60;
                    FpSpread3.Columns[6].Locked = true;

                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Specification";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[7].Width = 70;
                    FpSpread3.Columns[7].Locked = true;
                    FpSpread3.Columns[7].Visible = false;

                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Special Instruction";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[8].Width = 100;
                    FpSpread3.Columns[8].Locked = true;

                    for (row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        FpSpread3.Sheets[0].RowCount++;

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["itemheadername"]);
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["itemheadercode"]);
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["itemcode"]);
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["itempk"]);
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["itemname"]);
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["ItemModel"]);
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["ItemSize"]);
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["itemunit"]);
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                        //FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[row]["description"]);
                        //FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                        //FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        //FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[row]["ItemSpecification"]);
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                    }
                    FpSpread3.Visible = true;
                    //rptprint.Visible = true;
                    //div1.Visible = true;
                    //lbl_error.Visible = false;
                    FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
                    btn_itemok.Visible = true;
                    btn_itemexit.Visible = true;
                    newdiv.Visible = true;

                }
                else
                {
                    //div1.Visible = false;
                    FpSpread3.Visible = false;
                    btn_itemok.Visible = false;
                    btn_itemexit.Visible = false;
                    newdiv.Visible = false;
                    //rptprint.Visible = false;
                    //lbl_error.Visible = true;
                    //lbl_error.Text = "No Records Found";
                }
            }
            else
            {
                //div1.Visible = false;
                FpSpread3.Visible = false;
                btn_itemok.Visible = false;
                btn_itemexit.Visible = false;
                newdiv.Visible = false;
                //rptprint.Visible = false;
                //lbl_error.Visible = true;
                //lbl_error.Text = "No Records Found";
            }
        }
        catch
        {

        }

    }

    public void loadSpread2()
    {
        //Spread Code
        FpSpread2.Visible = true;

        FpSpread2.Sheets[0].ColumnCount = 0;
        FpSpread2.Sheets[0].RowCount = 0;
        FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
        FpSpread2.CommandBar.Visible = false;
        FpSpread2.SheetCorner.ColumnCount = 0;
        FpSpread2.Sheets[0].ColumnCount = 4;
        FpSpread2.Sheets[0].AutoPostBack = false;
        FpSpread2.Sheets[0].RowHeader.Visible = false;

        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        darkstyle.ForeColor = Color.White;
        FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Columns[0].Width = 50;
        FpSpread2.Columns[0].Locked = true;


        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Item Name";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
        FpSpread2.Columns[1].Width = 150;
        FpSpread2.Columns[1].Locked = true;

        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Code";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
        FpSpread2.Columns[2].Width = 150;
        FpSpread2.Columns[2].Locked = true;

        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Quantity";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
        FpSpread2.Columns[3].Width = 100;
        FpSpread2.Columns[3].Locked = false;
    }
    protected void btn_additem2_Click(object sender, EventArgs e)
    {
        FpSpread2.Sheets[0].RowCount++;
        FarPoint.Web.Spread.DoubleCellType db = new FarPoint.Web.Spread.DoubleCellType();
        db.ErrorMessage = "Enter only Numbers";

        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread2.Sheets[0].RowCount);
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(txt_item.Text);
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(txt_itemCode.Text);
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ViewState["itempk"]);
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].CellType = db;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(txt_quant.Text);
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;

        FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
        FpSpread2.Visible = true;
        txt_item.Text = "";
        txt_itemCode.Text = "";
        txt_quant.Text = "";
    }

    protected void btn_exit_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
        FpSpread2.Sheets[0].ColumnCount = 0;
        FpSpread2.Sheets[0].RowCount = 0;
        bindhostelname2();

    }
    protected void btn_additem1_Click(object sender, EventArgs e)
    {
        popwindow2.Visible = true;
        FpSpread3.Sheets[0].RowCount = 0;
        FpSpread3.Sheets[0].ColumnCount = 0;
        FpSpread3.Visible = false;
        btn_go1_Click(sender, e);

    }
    protected void btn_itemexit_Click(object sender, EventArgs e)
    {
        popwindow2.Visible = false;
        //btn_itemok.Visible = false;
        //btn_itemexit.Visible = false;
    }
    protected void btn_itemok_Click(object sender, EventArgs e)
    {
        try
        {
            string activerow = "";
            string activecol = "";
            activerow = FpSpread3.ActiveSheetView.ActiveRow.ToString();
            activecol = FpSpread3.ActiveSheetView.ActiveColumn.ToString();
            collegecode = Session["collegecode"].ToString();
            if (activerow.Trim() != "")
            {
                string itemCode = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                string itemName = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                string itempk = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                ViewState["itempk"] = itempk;
                txt_item.Text = itemName;
                txt_itemCode.Text = itemCode;
            }
            popwindow2.Visible = false;
        }
        catch
        {
        }
    }
    // popupwindow2
    public void loadheadername()
    {
        try
        {
            cbl_itemheader3.Items.Clear();
            // ddlpopitemheadername.Items.Clear();

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
                    for (row = 0; row <= splitnew.GetUpperBound(0); row++)
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
            string headerquery = "";
            if (maninvalue.Trim() != "")
            {
                headerquery = "select distinct itemheadercode ,itemheadername  from im_itemmaster where itemheadercode in ('" + maninvalue + "')";
            }
            else
            {
                headerquery = "select distinct itemheadercode ,itemheadername  from im_itemmaster";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(headerquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_itemheader3.DataSource = ds;
                cbl_itemheader3.DataTextField = "itemheadername";
                cbl_itemheader3.DataValueField = "itemheadercode";
                cbl_itemheader3.DataBind();

                //ddlpopitemheadername.DataSource = ds;
                //ddlpopitemheadername.DataTextField = "itemheader_name";
                //ddlpopitemheadername.DataValueField = "itemheader_code";
                //ddlpopitemheadername.DataBind();

                //ddlpopitemheadername.Items.Insert(0, "Select");
                //ddlpopitemheadername.Items.Insert(ddlpopitemheadername.Items.Count, "Others");
                if (cbl_itemheader3.Items.Count > 0)
                {
                    for (i = 0; i < cbl_itemheader3.Items.Count; i++)
                    {
                        cbl_itemheader3.Items[i].Selected = true;
                    }
                    txt_itemheader3.Text = "Header Name(" + cbl_itemheader3.Items.Count + ")";
                }
                if (cbl_itemheader3.Items.Count > 5)
                {
                    panel_itemheader3.Width = 250;
                    panel_itemheader3.Height = 300;
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
    protected void cb_itemheader3_OnCheckedChanged(object sender, EventArgs e)
    {
        txt_itemheader3.Text = "--Select--";
        if (cb_itemheader3.Checked == true)
        {
            for (i = 0; i < cbl_itemheader3.Items.Count; i++)
            {
                cbl_itemheader3.Items[i].Selected = true;
            }
            txt_itemheader3.Text = "Header Name(" + (cbl_itemheader3.Items.Count) + ")";
        }
        else
        {
            for (i = 0; i < cbl_itemheader3.Items.Count; i++)
            {
                cbl_itemheader3.Items[i].Selected = false;
            }
            txt_itemheader3.Text = "--Select--";
        }
        loaditem();
    }
    protected void cbl_itemheader3_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txt_itemheader3.Text = "--Select--";
        cb_itemheader3.Checked = false;
        commcount = 0;
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
            txt_itemheader3.Text = "Header Name(" + commcount.ToString() + ")";
        }
        loaditem();
    }

    public void loaditem()
    {
        try
        {
            ds.Clear();
            cbl_itemname3.Items.Clear();
            string itemheader = "";
            for (i = 0; i < cbl_itemheader3.Items.Count; i++)
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
                ds = queryObject.BindItemCode_inv(itemheader);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_itemname3.DataSource = ds;
                    cbl_itemname3.DataTextField = "itemname";
                    cbl_itemname3.DataValueField = "itemcode";
                    cbl_itemname3.DataBind();
                    if (cbl_itemname3.Items.Count > 0)
                    {
                        for (i = 0; i < cbl_itemname3.Items.Count; i++)
                        {
                            cbl_itemname3.Items[i].Selected = true;
                        }
                        txt_itemname3.Text = "Item Name(" + cbl_itemname3.Items.Count + ")";
                    }
                    //if (cbl_itemname3.Items.Count > 5)
                    //{
                    //    panel_itemname3.Width = 300;
                    //    panel_itemname3.Height = 300;
                    //}
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
        catch
        {
        }
    }
    protected void cb_itemname3_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_itemname3.Text = "--Select--";
            if (cb_itemname3.Checked == true)
            {
                for (i = 0; i < cbl_itemname3.Items.Count; i++)
                {
                    cbl_itemname3.Items[i].Selected = true;
                }
                txt_itemname3.Text = "Item Name(" + (cbl_itemname3.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_itemname3.Items.Count; i++)
                {
                    cbl_itemname3.Items[i].Selected = false;
                }
                txt_itemname3.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_itemname3_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_itemname3.Text = "--Select--";
            cb_itemname3.Checked = false;
            int commcount = 0;
            for (i = 0; i < cbl_itemname3.Items.Count; i++)
            {
                if (cbl_itemname3.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_itemname3.Items.Count)
                {
                    cb_itemname3.Checked = true;
                }
                txt_itemname3.Text = "Item Name(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddl_type3_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txt_searchby.Visible = false;
        txt_searchitemcode.Visible = false;
        txt_searchheadername.Visible = false;
        txt_searchby.Text = "";
        txt_searchheadername.Text = "";
        txt_searchitemcode.Text = "";

        if (ddl_type3.SelectedValue == "0")
        {
            txt_searchby.Visible = true;
        }
        else if (ddl_type3.SelectedValue == "1")
        {
            txt_searchitemcode.Visible = true;
        }
        else if (ddl_type3.SelectedValue == "2")
        {
            txt_searchheadername.Visible = true;
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct itemname from im_itemmaster WHERE itemname like '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;

    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getitemcode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct itemcode from im_itemmaster WHERE itemcode like '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getitemheader(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct itemheadername from im_itemmaster WHERE itemheadername like '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        if (rdbl_selection.SelectedIndex == 0)
        {
            try
            {
                hostel = "";
                building = "";
                floor = "";
                int netConnection = 0;

                dtaccessdate = DateTime.Now.ToString();
                dtaccesstime = DateTime.Now.ToLongTimeString();
                bool insok = false;

                if (cb_netcon.Checked)
                {
                    netConnection = 1;
                }


                hostel = ddl_hostelname2.SelectedValue.ToString();

                if (ddl_building2.Items.Count > 0)
                {
                    building = ddl_building2.SelectedItem.Value.ToString();

                    if (ddl_floor2.Items.Count > 0)
                    {
                        floor = ddl_floor2.SelectedItem.Value.ToString();

                        if (ddl_room2.Items.Count > 0)
                        {
                            room = ddl_room2.SelectedItem.Value.ToString();

                            //Insert into Roomstock_Master
                            int itemroomcount = 0;
                            string getvalue = d2.GetFunction("select  distinct top 1 RoomStockMaster_Code  from RoomStock_Master order by RoomStockMaster_Code desc");
                            if (getvalue.Trim() != "" && getvalue.Trim() != "0")
                            {
                                itemroomcount = Convert.ToInt32(getvalue);
                                itemroomcount = itemroomcount + 1;
                            }
                            else
                            {
                                itemroomcount = 1;
                            }
                            string insertRoomQuery = "INSERT INTO RoomStock_Master (Access_Date,Access_Time,RoomStockMaster_Code,Building_Name,Floor_Name,Room_Name,Hostel_Code,Net_Connection) values('" + dtaccessdate + "','" + dtaccesstime + "'," + itemroomcount + ",'" + building + "','" + floor + "','" + room + "','" + hostel + "'," + netConnection + ")";
                            int instRoom = d2.update_method_wo_parameter(insertRoomQuery, "Text");
                            if (instRoom != 0)
                            {

                                //Insert into RoomStock_Detail
                                //For Every Added Item
                                if (FpSpread2.Sheets[0].Rows.Count > 0)
                                {
                                    for (int m = 0; m < FpSpread2.Sheets[0].Rows.Count; m++)
                                    {

                                        string itemCode = Convert.ToString(FpSpread2.Sheets[0].Cells[m, 2].Tag);
                                        string itemQuantity = Convert.ToString(FpSpread2.Sheets[0].Cells[m, 3].Text);

                                        string insertStockQuery = "INSERT INTO RoomStock_Detail (Access_Date,Access_Time,Item_Code,Qty,RoomStockMaster_Code,Hostel_Code) values('" + dtaccessdate + "','" + dtaccesstime + "','" + itemCode + "','" + itemQuantity + "'," + itemroomcount + ",'" + hostel + "')";

                                        int instStock = d2.update_method_wo_parameter(insertStockQuery, "Text");
                                        if (instStock != 0)
                                        {
                                            insok = true;

                                        }
                                    }
                                }
                            }

                        }


                    }


                }

                if (insok == true)
                {
                    loadSpread2();
                    imgdiv2.Visible = true;
                    popwindow1.Visible = false;
                    btn_go_Click(sender, e);
                    lbl_alerterr.Text = "Saved Successfully";
                    clear();


                }

            }
            catch { }
        }
        else
        {
            try
            {
                hostel = "";
                building = "";
                floor = "";
                int netConnection = 0;

                dtaccessdate = DateTime.Now.ToString();
                dtaccesstime = DateTime.Now.ToLongTimeString();
                bool insok = false;

                if (cb_netcon.Checked)
                {
                    netConnection = 1;
                }

                //For Hostel 
                //for (int i = 0; i < cbl_hostel2.Items.Count; i++)
                //{
                //For each selected Hostel 
                //if (cbl_hostel2.Items[i].Selected)
                //{
                hostel = ddl_hostelname2.SelectedValue.ToString();
                //For Building
                for (int k = 0; k < cbl_building2.Items.Count; k++)
                {
                    //For each selected building
                    if (cbl_building2.Items[k].Selected)
                    {
                        building = cbl_building2.Items[k].Value.ToString();
                        //For Floor
                        for (int j = 0; j < cbl_floor2.Items.Count; j++)
                        {
                            //For each selected Floor
                            if (cbl_floor2.Items[j].Selected)
                            {
                                floor = cbl_floor2.Items[j].Value.ToString();
                                //For Room Type
                                for (int l = 0; l < cbl_room2.Items.Count; l++)
                                {
                                    //For each selected Room Type
                                    if (cbl_room2.Items[l].Selected)
                                    {
                                        room = cbl_room2.Items[l].Value.ToString();

                                        //Insert into Roomstock_Master
                                        int itemroomcount = 0;
                                        string getvalue = d2.GetFunction("select  distinct top 1 RoomStockMaster_Code  from RoomStock_Master order by RoomStockMaster_Code desc");
                                        if (getvalue.Trim() != "" && getvalue.Trim() != "0")
                                        {
                                            itemroomcount = Convert.ToInt32(getvalue);
                                            itemroomcount = itemroomcount + 1;
                                        }
                                        else
                                        {
                                            itemroomcount = 1;
                                        }
                                        string insertRoomQuery = "INSERT INTO RoomStock_Master (Access_Date,Access_Time,RoomStockMaster_Code,Building_Name,Floor_Name,Room_Name,Hostel_Code,Net_Connection) values('" + dtaccessdate + "','" + dtaccesstime + "'," + itemroomcount + ",'" + building + "','" + floor + "','" + room + "','" + hostel + "'," + netConnection + ")";
                                        int instRoom = d2.update_method_wo_parameter(insertRoomQuery, "Text");
                                        if (instRoom != 0)
                                        {

                                            //Insert into RoomStock_Detail
                                            //For Every Added Item
                                            if (FpSpread2.Sheets[0].Rows.Count > 0)
                                            {
                                                for (int m = 0; m < FpSpread2.Sheets[0].Rows.Count; m++)
                                                {

                                                    string itemCode = Convert.ToString(FpSpread2.Sheets[0].Cells[m, 2].Tag);
                                                    string itemQuantity = Convert.ToString(FpSpread2.Sheets[0].Cells[m, 3].Text);

                                                    string insertStockQuery = "INSERT INTO RoomStock_Detail (Access_Date,Access_Time,Item_Code,Qty,RoomStockMaster_Code,Hostel_Code) values('" + dtaccessdate + "','" + dtaccesstime + "','" + itemCode + "','" + itemQuantity + "'," + itemroomcount + ",'" + hostel + "')";

                                                    int instStock = d2.update_method_wo_parameter(insertStockQuery, "Text");
                                                    if (instStock != 0)
                                                    {
                                                        insok = true;
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
                //}
                //}
                if (insok)
                {
                    loadSpread2();
                    popwindow1.Visible = false;
                    btn_go_Click(sender, e);
                    lbl_alerterr.Text = "Saved Successfully";
                    imgdiv2.Visible = true;
                    clear();
                }
            }
            catch { }
        }
    }

    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Room Stock Details Report";
            string pagename = "inv_StockDetails.aspx";
            Printmaster1.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printmaster1.Visible = true;
        }
        catch
        {
        }
    }
    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txt_excelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
                lbl_validation.Visible = false;
            }
            else
            {
                lbl_validation.Text = "Please Enter Your Report Name";
                lbl_validation.Visible = true;
                txt_excelname.Focus();
            }
        }
        catch
        {
        }
    }
    protected void FpSpread1_OnCellClick(object sender, EventArgs e)
    {
        check_value = true;
    }
    protected void FpSpread1_Selectedindexchange(object sender, EventArgs e)
    {
        if (check_value == true)
        {
            string activerow = "";
            string activecol = "";
            activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();

            if (Convert.ToInt32(activecol) > 5)
            {
                rdbl_selection.Visible = false;
                rdbl_selection.SelectedIndex = 0;

                ddl_hostelname2.Enabled = false;
                ddl_building2.Enabled = false;
                ddl_floor2.Enabled = false;
                ddl_room2.Enabled = false;

                txt_hostelname2.Enabled = false;
                txt_building2.Enabled = false;
                txt_floor2.Enabled = false;
                txt_room2.Enabled = false;

                rdoClear();
                string hostelName = "";
                string hostelCode = "";
                string buildingName = "";
                string floorName = "";
                string roomName = "";
                string netConn = "";
                int netcon = 0;
                //string itemCode = "";
                //string itemName = "";
                string collegeCode = "";
                //string quantity = "";

                collegeCode = Session["collegecode"].ToString();
                if (activerow.Trim() != "")
                {
                    hostelName = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    hostelCode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                    buildingName = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                    floorName = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                    roomName = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text); ;
                    netConn = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);

                    string buildfk = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                    string floorfk = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag);
                    string roomfk = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag);

                    bindhostelnameDDl();
                    clgbuild2();
                    clgfloor2();
                    clgroom2();
                    ddl_hostelname2.SelectedIndex = ddl_hostelname2.Items.IndexOf(ddl_hostelname2.Items.FindByValue(hostelCode));
                    ddl_building2.SelectedIndex = ddl_building2.Items.IndexOf(ddl_building2.Items.FindByText(buildingName));
                    ddl_floor2.SelectedIndex = ddl_floor2.Items.IndexOf(ddl_floor2.Items.FindByText(floorName));
                    ddl_room2.SelectedIndex = ddl_room2.Items.IndexOf(ddl_room2.Items.FindByText(roomName));

                    #region old ddl
                    //for (i = 0; i < ddl_hostelname2.Items.Count; i++)
                    //{
                    //    if (ddl_hostelname2.Items[i].Value == hostelCode)
                    //    {
                    //        ddl_hostelname2.SelectedIndex = i;
                    //        break;
                    //    }
                    //}


                    ////cbl_hostel2_SelectedIndexChanged(sender, e);

                    //for (i = 0; i < ddl_building2.Items.Count; i++)
                    //{
                    //    if (ddl_building2.Items[i].Text == buildingName)
                    //    {
                    //        ddl_building2.SelectedIndex = i;
                    //        break;
                    //    }
                    //}

                    ////cbl_building2_OnSelectedIndexChanged(sender, e);

                    //for (i = 0; i < ddl_floor2.Items.Count; i++)
                    //{
                    //    if (ddl_floor2.Items[i].Text == floorName)
                    //    {
                    //        ddl_floor2.SelectedIndex = i;
                    //        break;
                    //    }
                    //}

                    ////cbl_floor2_OnSelectedIndexChanged(sender, e);


                    //for (i = 0; i < ddl_room2.Items.Count; i++)
                    //{
                    //    if (ddl_room2.Items[i].Text == roomName)
                    //    {
                    //        ddl_room2.SelectedIndex = i;
                    //        break;
                    //    }
                    //}
                    #endregion

                    if (netConn == "Yes")
                    {
                        cb_netcon.Checked = true;
                        netcon = 1;
                    }
                    else
                    {
                        cb_netcon.Checked = false;
                    }

                    //txt_item.Text = itemName;
                    //txt_itemCode.Text = itemCode;
                    //txt_quant.Text = quantity;

                    loadSpread2();


                    string selectquery = "  select i.itemcode,i.itemname,i.itempk,rd.Qty from RoomStock_Detail rd,RoomStock_Master rm,IM_ItemMaster  i where rd.RoomStockMaster_Code =rm.RoomStockMaster_Code and i.ItemPK =rd.Item_Code  and rd.Hostel_Code =rm.Hostel_Code and rd.Hostel_Code ='" + hostelCode + "' and rm.Room_Name ='" + roomfk + "' and rm.Floor_Name ='" + floorfk + "'  and Building_Name ='" + buildfk + "' and Net_Connection='" + netcon + "' order by i.itemcode ";

                    //string selectquery = "  select i.item_code,i.item_name,rd.Qty from RoomStock_Detail rd,RoomStock_Master rm,item_master i        where rd.RoomStockMaster_Code =rm.RoomStockMaster_Code and i.item_code =rd.Item_Code  and rd.Hostel_Code =rm.Hostel_Code and rd.Hostel_Code ='" + hostelCode + "' and rm.Room_Name ='" + roomName + "' and rm.Floor_Name ='" + floorName + "'  and Building_Name ='" + buildingName + "' and Net_Connection='" + netcon + "' order by i.item_code ";

                    ds.Clear();
                    ds = da.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpread2.Sheets[0].RowCount++;
                            FarPoint.Web.Spread.DoubleCellType db = new FarPoint.Web.Spread.DoubleCellType();
                            db.ErrorMessage = "Enter only Numbers";

                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread2.Sheets[0].RowCount);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["itemname"].ToString());
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["itemcode"].ToString());
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["itempk"]);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].CellType = db;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Qty"].ToString());
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        }
                    }
                }
                FpSpread2.Sheets[0].Visible = true;
                FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                btn_save.Visible = false;
                btn_update.Visible = true;
                btn_delete.Visible = true;
                popwindow1.Visible = true;
            }
        }
    }
    protected void rdbl_selection_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        rdoClear();
        btn_addnew_Click(sender, e);
    }
    public void rdoClear()
    {
        ddl_hostelname2.Visible = true;
        ddl_building2.Visible = false;
        ddl_floor2.Visible = false;
        ddl_room2.Visible = false;

        txt_hostelname2.Visible = false;
        txt_building2.Visible = false;
        txt_floor2.Visible = false;
        txt_room2.Visible = false;

        panel_hostel2.Visible = false;
        panel_building2.Visible = false;
        panel_floor2.Visible = false;
        panel_room2.Visible = false;

        txt_item.Text = "";
        txt_itemCode.Text = "";
        txt_quant.Text = "";
        cb_netcon.Checked = false;

        if (rdbl_selection.SelectedIndex == 0)
        {
            ddl_building2.Visible = true;
            ddl_floor2.Visible = true;
            ddl_room2.Visible = true;
        }
        else
        {
            //txt_hostelname2.Visible = true;
            txt_building2.Visible = true;
            txt_floor2.Visible = true;
            txt_room2.Visible = true;

            //panel_hostel2.Visible = true;
            panel_building2.Visible = true;
            panel_floor2.Visible = true;
            panel_room2.Visible = true;
        }
    }
    protected void btn_errclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    #region For Column order
    public void cbl_columnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            cb_column.Checked = false;
            string value = "";
            int index;

            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cbl_columnorder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    ItemList.Add(cbl_columnorder.Items[index].Value.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cbl_columnorder.Items[index].Value.ToString());
                Itemindex.Remove(sindex);
            }
            for (i = 0; i < cbl_columnorder.Items.Count; i++)
            {
                if (cbl_columnorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cbl_columnorder.Items[i].Value.ToString());
                    Itemindex.Remove(sindex);

                }
            }

            lnk_columnorder.Visible = true;
            txt_order.Visible = true;
            txt_order.Text = "";
            string colname12 = "";
            for (i = 0; i < ItemList.Count; i++)
            {
                if (colname12 == "")
                {
                    colname12 = ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";

                }
                else
                {
                    colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
                }
            }
            txt_order.Text = colname12;
            if (ItemList.Count == 14)
            {
                cb_column.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                txt_order.Visible = false;
                lnk_columnorder.Visible = false;
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void cb_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (cb_column.Checked == true)
            {
                txt_order.Text = "";
                ItemList.Clear();
                for (i = 0; i < cbl_columnorder.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cbl_columnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                    ItemList.Add(cbl_columnorder.Items[i].Value.ToString());
                    Itemindex.Add(si);
                }
                lnk_columnorder.Visible = true;
                txt_order.Visible = true;
                txt_order.Text = "";
                int j = 0;
                string colname12 = "";
                for (i = 0; i < ItemList.Count; i++)
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
                }
                txt_order.Text = colname12;
            }
            else
            {
                for (i = 0; i < cbl_columnorder.Items.Count; i++)
                {
                    cbl_columnorder.Items[i].Selected = false;
                    lnk_columnorder.Visible = false;
                    ItemList.Clear();
                    Itemindex.Clear();
                }

                txt_order.Text = "";
                txt_order.Visible = false;

            }
        }
        catch (Exception ex)
        {
        }
    }
    public void lnk_columnorder_Click(object sender, EventArgs e)
    {
        try
        {
            cbl_columnorder.ClearSelection();
            cb_column.Checked = false;
            lnk_columnorder.Visible = false;
            ItemList.Clear();
            Itemindex.Clear();
            txt_order.Text = "";
            txt_order.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    public void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }
    protected void btn_update_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread2.SaveChanges();
            hostel = "";
            building = "";
            floor = "";
            room = "";

            hostel = Convert.ToString(ddl_hostelname2.SelectedValue.ToString());
            building = Convert.ToString(ddl_building2.SelectedItem.Value.ToString());
            floor = Convert.ToString(ddl_floor2.SelectedItem.Value.ToString());
            room = Convert.ToString(ddl_room2.SelectedItem.Value.ToString());

            dtaccessdate = DateTime.Now.ToString();
            dtaccesstime = DateTime.Now.ToLongTimeString();

            itemCode = "";
            itemQuantity = "";
            netConnection = 0;
            bool updateOk = false;

            if (cb_netcon.Checked)
            {
                netConnection = 1;
            }

            //Update RoomStock_Detail
            if (FpSpread2.Sheets[0].Rows.Count > 0)
            {
                for (i = 0; i < FpSpread2.Sheets[0].Rows.Count; i++)
                {
                    itemCode = Convert.ToString(FpSpread2.Sheets[0].Cells[i, 2].Text);
                    string itempk = Convert.ToString(FpSpread2.Sheets[0].Cells[i, 2].Tag);
                    itemQuantity = Convert.ToString(FpSpread2.Sheets[0].Cells[i, 3].Text);

                    selectQuery = "select RoomStockMaster_Code from RoomStock_Master where Building_Name='" + building + "' and Floor_Name='" + floor + "' and Room_Name ='" + room + "' and Hostel_Code ='" + hostel + "' and Net_Connection='" + netConnection + "'";
                    string roommasterCode = "";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(selectQuery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        roommasterCode = Convert.ToString(ds.Tables[0].Rows[0][0].ToString());
                        string updateStockQuery = "if exists (select * from  RoomStock_Detail where RoomStockMaster_Code ='" + roommasterCode + "' and Item_Code ='" + itempk + "' and Hostel_Code='" + hostel + "') update RoomStock_Detail set Access_Date ='" + dtaccessdate + "',Access_Time ='" + dtaccesstime + "', Qty ='" + itemQuantity + "' where RoomStockMaster_Code ='" + roommasterCode + "' and Item_Code ='" + itempk + "' and Hostel_Code='" + hostel + "' else insert into RoomStock_Detail  (Access_Date,Access_Time,Item_Code,Qty,RoomStockMaster_Code,Hostel_Code) values ('" + dtaccessdate + "','" + dtaccesstime + "','" + itempk + "','" + itemQuantity + "'," + roommasterCode + ",'" + hostel + "')";

                        int updStock = d2.update_method_wo_parameter(updateStockQuery, "Text");
                        if (updStock != 0)
                        {
                            updateOk = true;
                        }
                    }
                }
            }
            if (updateOk)
            {
                loadSpread2();
                imgdiv2.Visible = true;
                popwindow1.Visible = false;
                btn_go_Click(sender, e);
                lbl_alerterr.Text = "Updated Successfully";
            }
        }
        catch { }
    }
    public void delete()
    {
        try
        {
            surediv.Visible = false;
            // FpSpread2.SaveChanges();
            hostel = "";
            building = "";
            floor = "";
            room = "";

            hostel = Convert.ToString(ddl_hostelname2.SelectedValue.ToString());
            building = Convert.ToString(ddl_building2.SelectedItem.Value.ToString());
            floor = Convert.ToString(ddl_floor2.SelectedItem.Value.ToString());
            room = Convert.ToString(ddl_room2.SelectedItem.Value.ToString());

            dtaccessdate = DateTime.Now.ToString();
            dtaccesstime = DateTime.Now.ToLongTimeString();

            itemCode = "";
            itemQuantity = "";
            netConnection = 0;
            bool deleteOk = false;

            if (cb_netcon.Checked)
            {
                netConnection = 1;
            }
            //Update RoomStock_Detail
            if (FpSpread2.Sheets[0].Rows.Count > 0)
            {
                for (i = 0; i < FpSpread2.Sheets[0].Rows.Count; i++)
                {
                    itemCode = Convert.ToString(FpSpread2.Sheets[0].Cells[i, 2].Text);
                    string itempk = Convert.ToString(FpSpread2.Sheets[0].Cells[i, 2].Tag);
                    itemQuantity = Convert.ToString(FpSpread2.Sheets[0].Cells[i, 3].Text);

                    selectQuery = "select RoomStockMaster_Code from RoomStock_Master where Building_Name='" + building + "' and Floor_Name='" + floor + "' and Room_Name ='" + room + "' and Hostel_Code ='" + hostel + "' and Net_Connection='" + netConnection + "'";
                    string roommasterCode = "";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(selectQuery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        roommasterCode = Convert.ToString(ds.Tables[0].Rows[0][0].ToString());
                        string deleteStockQuery = "delete from RoomStock_Detail where RoomStockMaster_Code ='" + roommasterCode + "' ; delete from RoomStock_Master where  RoomStockMaster_Code ='" + roommasterCode + "'";

                        int delStock = d2.update_method_wo_parameter(deleteStockQuery, "Text");
                        if (delStock != 0)
                        {
                            deleteOk = true;
                        }
                    }
                }
            }
            if (deleteOk)
            {
                loadSpread2();
                imgdiv2.Visible = true;
                popwindow1.Visible = false;
                btn_go_Click(sender, e);
                lbl_alerterr.Text = "Deleted Successfully";
            }
        }
        catch { }
    }
    protected void btn_delete_Click(object sender, EventArgs e)
    {
        try
        {
            if (btn_delete.Text == "Delete")
            {
                surediv.Visible = true;
                lbl_sure.Text = "Do you want Delete this record?";
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
            bindhostelnameDDl();
            bindhostelname2();
            clgbuild2();
            clgfloor2();
            clgroom2();

            loadSpread2();
            btn_additem1.Enabled = true;

            txt_item.Text = "";
            txt_itemCode.Text = "";
            txt_quant.Text = "";
            cb_netcon.Checked = false;

            popwindow1.Visible = true;
            btn_save.Visible = true;
            btn_update.Visible = false;
            btn_delete.Visible = false;
            rdbl_selection.Visible = true;
        }
        catch { }
    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
        //surediv.Visible = false;
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        imgdiv2.Visible = false;
        popwindow1.Visible = true;
    }
}