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

public partial class Inv_student_token_details : System.Web.UI.Page
{

    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();

    static ArrayList ItemList1 = new ArrayList();
    static ArrayList Itemindex1 = new ArrayList();

    static ArrayList ItemList2 = new ArrayList();
    static ArrayList Itemindex2 = new ArrayList();

    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    DAccess2 dt = new DAccess2();
    private object sender;
    private EventArgs e;

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
        lbl_validation.Text = "";
        if (!IsPostBack)
        {
            bindhostelname();
            bindbuild();
            bindfloor();
            bindroom();

            bindhostelname1();
            bindbuild1();
            bindfloor1();
            bindroom1();
            loadmenuname();

            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Fpspread1.Visible = false;

            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnCount = 0;
            Fpspread2.Visible = false;
            btn_save.Visible = false;
            btn_exit.Visible = false;

            //loadyear();
            //loadmonth();

            txt_tokendate.Attributes.Add("readonly", "readonly");
            txt_tokendate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Attributes.Add("readonly", "readonly");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            rdb_student.Checked = true;
            rdb_student1.Checked = true;
            rdb_checkedchanged(sender, e);
            btn_go_Click(sender, e);
        }

        lbl_error.Visible = false;
    }
    protected void lnk_btn_logout_Click(object sender, EventArgs e)
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

    public void datevalidate(TextBox txt1, TextBox txt2)
    {
        try
        {
            if (txt1.Text != "" && txt2.Text != "")
            {
                //txt_leavedays.Text = "";
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt1.Text);
                string seconddate = Convert.ToString(txt2.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;
                if (dt > dt1)
                {
                    imgdiv2.Visible = true;
                    lbl_alerterror.Text = "Select ToDate greater than or equal to the FromDate ";
                    txt2.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txt1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //txt_leavedays.Text = "";
                    //txt_rebatedays.Text = "";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindhostelname()
    {
        try
        {
            cbl_hostelname.Items.Clear();
            //ds.Clear();
            // ds = d2.BindHostel_inv(collegecode1);

            //string itemname = "select HostelMasterPK,HostelName from HM_HostelMaster order by HostelName ";
            ds.Clear();
            //ds = d2.select_method_wo_parameter(itemname, "Text");

            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "HostelName";
                cbl_hostelname.DataValueField = "HostelMasterPK";
                cbl_hostelname.DataBind();
                if (cbl_hostelname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                    {
                        cbl_hostelname.Items[i].Selected = true;
                    }
                    txt_hostelname.Text = "Hostel Name(" + cbl_hostelname.Items.Count + ")";
                    cb_hostelname.Checked = true;
                }
            }
            else
            {
                txt_hostelname.Text = "--Select--";
                cb_hostelname.Checked = false;
            }
        }
        catch
        {
        }
    }
    protected void cb_hostelname_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;

        txt_hostelname.Text = "--Select--";
        if (cb_hostelname.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                cbl_hostelname.Items[i].Selected = true;
            }
            cb_hostelname.Checked = true;
            txt_hostelname.Text = "Hostel Name(" + (cbl_hostelname.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                cbl_hostelname.Items[i].Selected = false;
            }
            txt_hostelname.Text = "--Select--";
            cb_hostelname.Checked = false;
        }

        bindbuild();
        bindfloor();
        bindroom();
    }
    protected void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            cb_hostelname.Checked = false;
            int commcount = 0;
            //string buildvalue = "";
            //string build = "";
            txt_hostelname.Text = "--Select--";
            for (i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                //    if (cbl_hostelname.Items[i].Selected == true)
                //    {
                //        commcount = commcount + 1;
                //        cb_hostelname.Checked = false;

                //        build = cbl_hostelname.Items[i].Value.ToString();
                //        if (buildvalue == "")
                //        {
                //            buildvalue = build;
                //        }
                //        else
                //        {
                //            buildvalue = buildvalue + "'" + "," + "'" + build;
                //        }

                //    }
                //}
                //if (commcount > 0)
                //{
                //    if (commcount == cbl_hostelname.Items.Count)
                //    {
                //        cb_hostelname.Checked = true;
                //    }
                //    txt_hostelname.Text = "Hostel Name(" + commcount.ToString() + ")";
                //}

                if (cbl_hostelname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_hostelname.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_hostelname.Items.Count)
                {
                    cb_hostelname.Checked = true;
                }
                txt_hostelname.Text = "Hostel Name(" + commcount.ToString() + ")";
            }

            bindbuild();
            bindfloor();
            bindroom();
        }
        catch (Exception ex)
        {

        }
    }

    public void bindbuild()
    {
        try
        {
            cbl_building.Items.Clear();
            //txt_building.Text = "---Select---";
            //cb_building.Checked = false;
            string build = "";
            if (cbl_hostelname.Items.Count > 0)
            {
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    if (cbl_hostelname.Items[i].Selected == true)
                    {
                        if (build == "")
                        {
                            build = Convert.ToString(cbl_hostelname.Items[i].Value);
                        }
                        else
                        {
                            build = build + "'" + "," + "'" + Convert.ToString(cbl_hostelname.Items[i].Value);
                        }
                    }
                }
            }
            string bul = "";
            if (build != "")
            {
                bul = d2.GetBuildingCode_inv(build);
                ds = d2.BindBuilding(bul);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_building.DataSource = ds;
                    cbl_building.DataTextField = "Building_Name";
                    cbl_building.DataValueField = "code";
                    cbl_building.DataBind();
                    if (cbl_building.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_building.Items.Count; i++)
                        {
                            cbl_building.Items[i].Selected = true;
                        }
                        cb_building.Checked = true;
                        txt_building.Text = "Building Name(" + cbl_building.Items.Count + ")";
                    }
                }
            }
            else
            {
                cb_building.Checked = false;
                txt_building.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_building_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_building.Text = "--Select--";
            if (cb_building.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_building.Items.Count; i++)
                {
                    cbl_building.Items[i].Selected = true;
                }
                txt_building.Text = "Building Name(" + (cbl_building.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_building.Items.Count; i++)
                {
                    cbl_building.Items[i].Selected = false;
                }
                txt_building.Text = "--Select--";
            }
            bindfloor();
            bindroom();
        }
        catch
        {
        }
    }
    protected void cblbuilding_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            string buildvalue = "";
            string build = "";
            cb_building.Checked = false;
            int commcount = 0;
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
            //    if (cbl_building.Items[i].Selected == true)
            //    {
            //        commcount = commcount + 1;
            //    }
            //}
            //if (commcount > 0)
            //{
            //    if (commcount == cbl_building.Items.Count)
            //    {
            //        cb_building.Checked = true;
            //    }
            //    txt_building.Text = "Building Name(" + commcount.ToString() + ")";
            //}
            bindfloor();
            bindroom();
        }
        catch (Exception ex)
        {

        }
    }

    public void bindfloor()
    {
        try
        {
            string floorname = "";
            cbl_floor.Items.Clear();
            txt_floor.Text = "---Select---";
            cb_floor.Checked = false;
            if (cbl_building.Items.Count > 0)
            {
                for (int i = 0; i < cbl_building.Items.Count; i++)
                {
                    if (cbl_building.Items[i].Selected == true)
                    {
                        if (floorname == "")
                        {
                            floorname = Convert.ToString(cbl_building.Items[i].Text);
                        }
                        else
                        {
                            floorname = floorname + "'" + "," + "'" + Convert.ToString(cbl_building.Items[i].Text);
                        }
                    }
                }
            }
            if (floorname != "")
            {
                ds = d2.BindFloor_new(floorname);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_floor.DataSource = ds;
                    cbl_floor.DataTextField = "Floor_Name";
                    cbl_floor.DataValueField = "FloorPK";
                    cbl_floor.DataBind();
                    if (cbl_floor.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_floor.Items.Count; i++)
                        {
                            cbl_floor.Items[i].Selected = true;
                        }
                        txt_floor.Text = "Floor (" + cbl_floor.Items.Count + ")";
                        cb_floor.Checked = true;
                    }
                }
            }
            else
            {
                cb_floor.Checked = false;
                txt_floor.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbfloor_CheckedChanged(object sender, EventArgs e)
    {

        try
        {
            int cout = 0;
            txt_floor.Text = "--Select--";
            if (cb_floor.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_floor.Items.Count; i++)
                {
                    cbl_floor.Items[i].Selected = true;
                }
                txt_floor.Text = "Floor (" + (cbl_floor.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_floor.Items.Count; i++)
                {
                    cbl_floor.Items[i].Selected = false;
                }
                txt_floor.Text = "--Select--";
            }

            bindroom();
        }
        catch
        {
        }
    }
    protected void cblfloor_SelectedIndexChanged(object sender, EventArgs e)
    {
        cbl_room.Items.Clear();
        txt_room.Text = "--Select--";

        cb_room.Checked = false;
        int i = 0;
        cb_floor.Checked = false;
        int commcount = 0;

        txt_floor.Text = "--Select--";



        for (i = 0; i < cbl_floor.Items.Count; i++)
        {
            if (cbl_floor.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                //cb_floor.Checked = false;

            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_floor.Items.Count)
            {
                cb_floor.Checked = true;
            }
            txt_floor.Text = "Floor (" + commcount.ToString() + ")";
        }
        bindroom();
    }

    public void bindroom()
    {
        try
        {
            cbl_room.Items.Clear();
            txt_room.Text = "---Select---";
            cb_room.Checked = false;
            string flooor = "";
            string room = "";
            if (cbl_building.Items.Count > 0)
            {
                for (int i = 0; i < cbl_building.Items.Count; i++)
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
                for (int i = 0; i < cbl_floor.Items.Count; i++)
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
                ds = d2.BindRoom(room, flooor);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_room.DataSource = ds;
                    cbl_room.DataTextField = "Room_Name";
                    cbl_room.DataValueField = "Roompk";
                    cbl_room.DataBind();

                    if (cbl_room.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_room.Items.Count; row++)
                        {
                            cbl_room.Items[row].Selected = true;
                        }
                        txt_room.Text = "Room (" + cbl_room.Items.Count + ")";
                        cb_room.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_room_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            int commcount = 0;
            txt_room.Text = "--Select--";
            if (cb_room.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_room.Items.Count; i++)
                {
                    cbl_room.Items[i].Selected = true;
                }
                txt_room.Text = "Room (" + (cbl_room.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_room.Items.Count; i++)
                {
                    cbl_room.Items[i].Selected = false;
                }
                txt_room.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void cbl_room_SelectedIndexChanged(object sender, EventArgs e)
    {
        cb_room.Checked = false;
        int commcount = 0;

        txt_room.Text = "--Select--";

        for (int i = 0; i < cbl_room.Items.Count; i++)
        {
            if (cbl_room.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                //cb_room.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_room.Items.Count)
            {
                cb_room.Checked = true;
            }
            txt_room.Text = "Room (" + commcount.ToString() + ")";
        }
    }

    protected void txt_fromdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            datevalidate(txt_fromdate, txt_todate);
            Fpspread1.Visible = false;
            rptprint.Visible = false;
            btn_delete.Visible = false;
            //div1.Visible = false;

            pheaderfilter.Visible = false;
            pcolumnorder.Visible = false;
        }
        catch (Exception ex)
        {
        }
        // PopupMessage("Enter FromDate less than or equal to the ToDate", cv_fromtodt1);
    }
    protected void txt_todate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            datevalidate(txt_fromdate, txt_todate);
            Fpspread1.Visible = false;
            rptprint.Visible = false;
            btn_delete.Visible = false;
            //div1.Visible = false;


            pheaderfilter.Visible = false;
            pcolumnorder.Visible = false;
        }
        catch (Exception ex)
        {
        }
        // PopupMessage("Enter ToDate greater than or equal to the FromDate", cv_fromtodt2);
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            string hostelcode = "";
            string building = "";
            string floor = "";
            string room = "";
            DateTime fromdate = new DateTime();
            fromdate = TextToDate(txt_fromdate);
            DateTime todate = new DateTime();
            todate = TextToDate(txt_todate);

            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    if (hostelcode == "")
                    {
                        hostelcode = "" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        hostelcode = hostelcode + "'" + "," + "'" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                }
            }

            for (int i = 0; i < cbl_building.Items.Count; i++)
            {
                if (cbl_building.Items[i].Selected == true)
                {
                    if (building == "")
                    {
                        building = "" + cbl_building.Items[i].Value + "";
                    }
                    else
                    {
                        building = building + "'" + "," + "'" + cbl_building.Items[i].Value + "";
                    }
                }
            }

            for (int i = 0; i < cbl_floor.Items.Count; i++)
            {
                if (cbl_floor.Items[i].Selected == true)
                {
                    if (building == "")
                    {
                        floor = "" + cbl_floor.Items[i].Value + "";
                    }
                    else
                    {
                        floor = floor + "'" + "," + "'" + cbl_floor.Items[i].Value + "";
                    }
                }
            }

            for (int i = 0; i < cbl_room.Items.Count; i++)
            {
                if (cbl_room.Items[i].Selected == true)
                {
                    if (building == "")
                    {
                        room = "" + cbl_room.Items[i].Value + "";
                    }
                    else
                    {
                        room = room + "'" + "," + "'" + cbl_room.Items[i].Value + "";
                    }
                }
            }

            if (rdb_student1.Checked == true)
            {
                if (ItemList.Count == 0)
                {
                    ItemList.Add("Roll_No");
                    ItemList.Add("Stud_Name");
                    ItemList.Add("MenuName");
                    ItemList.Add("TokenQty");
                    // Fpspread1.Width = 470;
                }
                Hashtable columnhash = new Hashtable();
                columnhash.Clear();
                int colinc = 0;


                columnhash.Add("Roll_No", "Roll No");
                columnhash.Add("Stud_Name", "Name");
                columnhash.Add("SessionName", "Session Name");
                columnhash.Add("MenuName", "Menu Name");
                columnhash.Add("TokenQty", "Quantity");
                //columnhash.Add("Mess_Month", "Month");
                //columnhash.Add("Mess_Year", "Year");
                columnhash.Add("TokenDate", "Token Date");
                columnhash.Add("HostelName", "Hostel Name");
                columnhash.Add("Building_Name", "Building");
                columnhash.Add("Floor_Name", "Floor");
                columnhash.Add("Room_Name", "Room");

                if (hostelcode.Trim() != "" && building.Trim() != "" && floor.Trim() != "" && room.Trim() != "")
                {
                    //string selectqurey = "select distinct st.Roll_No,r.Stud_Name,sm.Session_Name,mm.MenuName, st.Qty,st.Mess_Month,st.Mess_Year,convert(varchar,st.TokenDate,103) as 'TokenDate',hd.Hostel_Name,hs.Building_Name,hs.Floor_Name, hs.Room_Name from StudentToken_Details st,Registration r,Hostel_StudentDetails hs,MenuMaster mm,Session_Master sm,Hostel_Details hd where st.Roll_No=r.Roll_No and r.Roll_No=hs.Roll_No and st.MenuCode=mm.MenuCode and st.Session_Code=sm.Session_Code and hs.Hostel_Code=hd.Hostel_code and st.Hostel_Code in('" + hostelcode + "') and hs.Building_Name in('" + building + "') and hs.Floor_Name in('" + floor + "') and hs.Room_Name in('" + room + "') and st.TokenDate between '" + fromdate + "' and '" + todate + "' order by st.TokenDate";


                    string selectqurey = "select r.Roll_No,r.app_no,r.Stud_Name,sm.SessionName,mm.MenuName, st.TokenQty,convert(varchar,st.TokenDate,103) as 'TokenDate',hm.HostelName,bm.Building_Name,fm.Floor_Name, rd.Room_Name from HT_StudTokenDetails st,Registration r,HT_HostelRegistration hs,HM_MenuMaster mm,HM_SessionMaster sm,HM_HostelMaster hm,Building_Master bm,Room_Detail rd,Floor_Master fm where st.App_No=r.App_No and st.MenuFK =mm.MenuMasterPK  and st.SessionFK =sm.SessionMasterPK  and hs.HostelMasterFK=hm.HostelMasterPK and rd.Roompk=hs.RoomFK and hs.BuildingFK=bm.Code and fm.Floorpk =hs.FloorFK and sm.SessionMasterPK=st.SessionFK and hs.HostelMasterFK=hm.HostelMasterPK and r.App_No=hs.APP_No and hs.HostelMasterFK in('" + hostelcode + "') and hs.BuildingFK in('" + building + "') and hs.FloorFK in('" + floor + "') and hs.RoomFK in('" + room + "') and st.TokenDate between '" + fromdate + "' and '" + todate + "' order by st.TokenDate";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectqurey, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Fpspread1.Sheets[0].RowCount = 0;
                        Fpspread1.Sheets[0].ColumnCount = 0;
                        Fpspread1.CommandBar.Visible = false;
                        Fpspread1.Sheets[0].AutoPostBack = false;
                        Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpspread1.Sheets[0].RowHeader.Visible = false;
                        Fpspread1.Sheets[0].ColumnCount = ItemList.Count + 2;
                        Fpspread1.Sheets[0].RowCount = ds.Tables[0].Rows.Count;
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

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Columns[1].Width = 50;

                        //////true for select all//////// 
                        FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                        check.AutoPostBack = true;

                        FarPoint.Web.Spread.CheckBoxCellType check1 = new FarPoint.Web.Spread.CheckBoxCellType();
                        check1.AutoPostBack = false;

                        Fpspread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

                        for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                        {
                            string colno = Convert.ToString(ds.Tables[0].Columns[j]);

                            if (ItemList.Contains(Convert.ToString(colno)))
                            {
                                int insdex = ItemList.IndexOf(Convert.ToString(colno));
                                //FpSpread1.Columns[insdex].Locked = true;
                                Fpspread1.Columns[insdex + 2].Width = 150;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Text = Convert.ToString(columnhash[colno]);
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Font.Bold = true;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].HorizontalAlign = HorizontalAlign.Center;

                                if (colno == "Stud_Name")
                                {
                                    Fpspread1.Columns[insdex + 2].Width = 200;
                                }
                            }
                        }

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Fpspread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                            Fpspread1.Sheets[0].Cells[i, 1].CellType = check1;
                            Fpspread1.Sheets[0].Cells[i, 1].HorizontalAlign = HorizontalAlign.Center;

                            for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                            {
                                if (ItemList.Contains(Convert.ToString(ds.Tables[0].Columns[j].ToString())))
                                {

                                    int insdex = ItemList.IndexOf(Convert.ToString(ds.Tables[0].Columns[j].ToString()));
                                    Fpspread1.Sheets[0].Cells[i, insdex + 2].CellType = txt;
                                    Fpspread1.Sheets[0].Cells[i, insdex + 2].Text = ds.Tables[0].Rows[i][j].ToString();
                                    Fpspread1.Sheets[0].Cells[i, insdex + 2].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].Cells[i, insdex + 2].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[i, insdex + 2].Locked = true;
                                    //Fpspread1.Columns[insdex].Width = 150;
                                }
                            }
                            Fpspread1.Sheets[0].Cells[i, 2].CellType = txt;
                            Fpspread1.Sheets[0].Cells[i, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["app_no"]);
                            Fpspread1.Sheets[0].Cells[i, 2].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[i, 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[i, 2].Locked = true;
                        }
                        Fpspread1.Visible = true;
                        //pheaderfilter.Visible = true;
                        //pcolumnorder.Visible = true;
                        rptprint.Visible = true;
                        btn_delete.Visible = true;
                        //div1.Visible = true;
                        lbl_error.Visible = false;
                        Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    }
                    else
                    {
                        Fpspread1.Visible = false;
                        rptprint.Visible = false;
                        btn_delete.Visible = false;
                        //div1.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "No Records Found";
                        //pheaderfilter.Visible = false;
                        //pcolumnorder.Visible = false;
                        //pheaderfilter.Visible = false;
                    }
                }
                else
                {
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                    btn_delete.Visible = false;
                    //div1.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "No Records Found";
                    //pheaderfilter.Visible = false;
                }
            }
            if (rdb_staff1.Checked == true)
            {
                if (ItemList1.Count == 0)
                {
                    ItemList1.Add("Staff_code");
                    ItemList1.Add("Staff_name");
                    ItemList1.Add("MenuName");
                    ItemList1.Add("TokenQty");
                    // Fpspread1.Width = 470;
                }
                Hashtable columnhash1 = new Hashtable();
                columnhash1.Clear();
                int colinc1 = 0;
                if (rdb_other1.Checked == true)
                {
                    columnhash1.Add("APP_No", "Roll No");
                    columnhash1.Add("VendorCompName", "Name");
                }
                columnhash1.Add("Staff_code", "Staff Code");
                columnhash1.Add("Staff_name", "Staff Name");
                columnhash1.Add("SessionName", "Session Name");
                columnhash1.Add("MenuName", "Menu Name");
                columnhash1.Add("TokenQty", "Quantity");
                columnhash1.Add("TokenDate", "Token Date");
                columnhash1.Add("HostelName", "Hostel Name");
                columnhash1.Add("Building_Name", "Building");
                columnhash1.Add("Floor_Name", "Floor");
                columnhash1.Add("Room_Name", "Room");

                if (hostelcode.Trim() != "" && building.Trim() != "" && floor.Trim() != "" && room.Trim() != "")
                {
                    string selectqurey = "   select s.Staff_name,s.Staff_code,a.appl_id ,hs.HostelMasterFK ,sm.SessionName,mm.MenuName, st.TokenQty,convert(varchar,st.TokenDate,103) as 'TokenDate', bm.Building_Name,fm.Floor_Name,Room_Name,h.HostelName from HT_StudTokenDetails st,HM_HostelMaster h, HT_HostelRegistration hs, staffmaster s,staff_appl_master a,Room_Detail rd,Building_Master bm,Floor_Master fm,HM_MenuMaster mm,HM_SessionMaster sm where st.App_No=hs.APP_No and s.appl_no=a.appl_no and  hs.APP_No =a.appl_id and st.MenuFK =mm.MenuMasterPK  and st.SessionFK =sm.SessionMasterPK  and ISNULL(IsSuspend,0)=0 and ISNULL(IsDiscontinued,0)=0 and ISNULL(IsVacated,0)=0 and h.HostelMasterPK=hs.HostelMasterFK and rd.Roompk=hs.RoomFK and fm.Floorpk=hs.FloorFK and bm.Code=hs.BuildingFK and hs.MemType='2'  and hs.HostelMasterFK in('" + hostelcode + "') and hs.BuildingFK in('" + building + "') and hs.FloorFK in('" + floor + "') and hs.RoomFK in('" + room + "') and st.TokenDate between '" + fromdate + "' and '" + todate + "' order by st.TokenDate";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectqurey, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Fpspread1.Sheets[0].RowCount = 0;
                        Fpspread1.Sheets[0].ColumnCount = 0;
                        Fpspread1.CommandBar.Visible = false;
                        Fpspread1.Sheets[0].AutoPostBack = false;
                        Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpspread1.Sheets[0].RowHeader.Visible = false;
                        Fpspread1.Sheets[0].ColumnCount = ItemList1.Count + 2;
                        Fpspread1.Sheets[0].RowCount = ds.Tables[0].Rows.Count;
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

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Columns[1].Width = 50;

                        //////true for select all//////// 
                        FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                        check.AutoPostBack = true;

                        FarPoint.Web.Spread.CheckBoxCellType check1 = new FarPoint.Web.Spread.CheckBoxCellType();
                        check1.AutoPostBack = false;

                        Fpspread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

                        for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                        {
                            string colno = Convert.ToString(ds.Tables[0].Columns[j]);

                            if (ItemList1.Contains(Convert.ToString(colno)))
                            {
                                int insdex = ItemList1.IndexOf(Convert.ToString(colno));
                                //FpSpread1.Columns[insdex].Locked = true;
                                Fpspread1.Columns[insdex + 2].Width = 150;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Text = Convert.ToString(columnhash1[colno]);
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Font.Bold = true;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].HorizontalAlign = HorizontalAlign.Center;

                                if (colno == "staff_name")
                                {
                                    Fpspread1.Columns[insdex + 2].Width = 200;
                                }
                            }
                        }
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Fpspread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                            Fpspread1.Sheets[0].Cells[i, 1].CellType = check1;
                            Fpspread1.Sheets[0].Cells[i, 1].HorizontalAlign = HorizontalAlign.Center;

                            for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                            {
                                if (ItemList1.Contains(Convert.ToString(ds.Tables[0].Columns[j].ToString())))
                                {

                                    int insdex = ItemList1.IndexOf(Convert.ToString(ds.Tables[0].Columns[j].ToString()));
                                    Fpspread1.Sheets[0].Cells[i, insdex + 2].CellType = txt;
                                    Fpspread1.Sheets[0].Cells[i, insdex + 2].Text = ds.Tables[0].Rows[i][j].ToString();
                                    Fpspread1.Sheets[0].Cells[i, insdex + 2].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].Cells[i, insdex + 2].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[i, insdex + 2].Locked = true;
                                    //Fpspread1.Columns[insdex].Width = 150;
                                }
                            }
                            Fpspread1.Sheets[0].Cells[i, 2].CellType = txt;
                            Fpspread1.Sheets[0].Cells[i, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["appl_id"]);
                            Fpspread1.Sheets[0].Cells[i, 2].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[i, 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[i, 2].Locked = true;
                        }
                        Fpspread1.Visible = true;
                        //pheaderfilter1.Visible = true;
                        //pcolumnorder1.Visible = true;
                        rptprint.Visible = true;
                        btn_delete.Visible = true;
                        //div1.Visible = true;
                        lbl_error.Visible = false;
                        Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    }
                    else
                    {
                        Fpspread1.Visible = false;
                        rptprint.Visible = false;
                        btn_delete.Visible = false;
                        //div1.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "No Records Found";
                        //pheaderfilter.Visible = false;
                        //pcolumnorder.Visible = false;
                        //pheaderfilter.Visible = false;
                    }
                }
                else
                {
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                    btn_delete.Visible = false;
                    //div1.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "No Records Found";
                    //pheaderfilter.Visible = false;
                }

            }
            if (rdb_other1.Checked == true)
            {
                if (ItemList2.Count == 0)
                {
                    ItemList2.Add("APP_No");
                    ItemList2.Add("VendorCompName");
                    ItemList2.Add("MenuName");
                    ItemList2.Add("TokenQty");
                    // Fpspread1.Width = 470;
                }
                Hashtable columnhash2 = new Hashtable();
                columnhash2.Clear();
                int colinc1 = 0;

                columnhash2.Add("APP_No", "Guest Code");
                columnhash2.Add("VendorCompName", "Guest Name");
                columnhash2.Add("SessionName", "Session Name");
                columnhash2.Add("MenuName", "Menu Name");
                columnhash2.Add("TokenQty", "Quantity");
                columnhash2.Add("TokenDate", "Token Date");
                columnhash2.Add("HostelName", "Hostel Name");
                columnhash2.Add("Building_Name", "Building");
                columnhash2.Add("Floor_Name", "Floor");
                columnhash2.Add("Room_Name", "Room");

                if (hostelcode.Trim() != "" && building.Trim() != "" && floor.Trim() != "" && room.Trim() != "")
                {
                    //string selectqurey = "   select s.Staff_name,s.Staff_code,a.appl_id ,hs.HostelMasterFK ,sm.SessionName,mm.MenuName, st.TokenQty,convert(varchar,st.TokenDate,103) as 'TokenDate', bm.Building_Name,fm.Floor_Name,Room_Name,h.HostelName from HT_StudTokenDetails st,HM_HostelMaster h, HT_HostelRegistration hs, staffmaster s,staff_appl_master a,Room_Detail rd,Building_Master bm,Floor_Master fm,HM_MenuMaster mm,HM_SessionMaster sm where st.App_No=hs.APP_No and s.appl_no=a.appl_no and  hs.APP_No =a.appl_id and st.MenuFK =mm.MenuMasterPK  and st.SessionFK =sm.SessionMasterPK  and ISNULL(IsSuspend,0)=0 and ISNULL(IsDiscontinued,0)=0 and ISNULL(IsVacated,0)=0 and h.HostelMasterPK=hs.HostelMasterFK and rd.Roompk=hs.RoomFK and fm.Floorpk=hs.FloorFK and bm.Code=hs.BuildingFK and hs.MemType='2'  and hs.HostelMasterFK in('" + hostelcode + "') and hs.BuildingFK in('" + building + "') and hs.FloorFK in('" + floor + "') and hs.RoomFK in('" + room + "') and st.TokenDate between '" + fromdate + "' and '" + todate + "' order by st.TokenDate";

                    string selectqurey = "   select VendorCompName,st.APP_No ,hs.HostelMasterFK,sm.SessionName,mm.MenuName, st.TokenQty,convert(varchar,st.TokenDate,103) as 'TokenDate', bm.Building_Name,fm.Floor_Name,Room_Name,h.HostelName from HM_HostelMaster h, HT_HostelRegistration hs,CO_VendorMaster vm ,Room_Detail rd,Building_Master bm,Floor_Master fm,HM_MenuMaster mm,HM_SessionMaster sm,HT_StudTokenDetails st where  hs.APP_No=st.App_No and st.MenuFK =mm.MenuMasterPK  and st.SessionFK =sm.SessionMasterPK and vm.VendorPK=hs.app_no and ISNULL(IsSuspend,0)=0 and ISNULL(IsDiscontinued,0)=0 and ISNULL(IsVacated,0)=0 and h.HostelMasterPK=hs.HostelMasterFK and rd.Roompk=hs.RoomFK and fm.Floorpk=hs.FloorFK and bm.Code=hs.BuildingFK and hs.MemType='3' and hs.HostelMasterFK in('" + hostelcode + "') and hs.BuildingFK in('" + building + "') and hs.FloorFK in('" + floor + "') and hs.RoomFK in('" + room + "') and st.TokenDate between '" + fromdate + "' and '" + todate + "' order by st.TokenDate";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectqurey, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Fpspread1.Sheets[0].RowCount = 0;
                        Fpspread1.Sheets[0].ColumnCount = 0;
                        Fpspread1.CommandBar.Visible = false;
                        Fpspread1.Sheets[0].AutoPostBack = false;
                        Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpspread1.Sheets[0].RowHeader.Visible = false;
                        Fpspread1.Sheets[0].ColumnCount = ItemList2.Count + 2;
                        Fpspread1.Sheets[0].RowCount = ds.Tables[0].Rows.Count;
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

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Columns[1].Width = 50;

                        //////true for select all//////// 
                        FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                        check.AutoPostBack = true;

                        FarPoint.Web.Spread.CheckBoxCellType check1 = new FarPoint.Web.Spread.CheckBoxCellType();
                        check1.AutoPostBack = false;

                        Fpspread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

                        for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                        {
                            string colno = Convert.ToString(ds.Tables[0].Columns[j]);

                            if (ItemList2.Contains(Convert.ToString(colno)))
                            {
                                int insdex = ItemList2.IndexOf(Convert.ToString(colno));
                                //FpSpread1.Columns[insdex].Locked = true;
                                Fpspread1.Columns[insdex + 2].Width = 150;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Text = Convert.ToString(columnhash2[colno]);
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Font.Bold = true;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].HorizontalAlign = HorizontalAlign.Center;

                                if (colno == "Staff_name")
                                {
                                    Fpspread1.Columns[insdex + 2].Width = 200;
                                }
                            }
                        }
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Fpspread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                            Fpspread1.Sheets[0].Cells[i, 1].CellType = check1;
                            Fpspread1.Sheets[0].Cells[i, 1].HorizontalAlign = HorizontalAlign.Center;

                            for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                            {
                                if (ItemList2.Contains(Convert.ToString(ds.Tables[0].Columns[j].ToString())))
                                {
                                    int insdex = ItemList2.IndexOf(Convert.ToString(ds.Tables[0].Columns[j].ToString()));
                                    Fpspread1.Sheets[0].Cells[i, insdex + 2].CellType = txt;
                                    Fpspread1.Sheets[0].Cells[i, insdex + 2].Text = ds.Tables[0].Rows[i][j].ToString();
                                    Fpspread1.Sheets[0].Cells[i, insdex + 2].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].Cells[i, insdex + 2].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[i, insdex + 2].Locked = true;
                                    //Fpspread1.Columns[insdex].Width = 150;
                                }
                            }
                            Fpspread1.Sheets[0].Cells[i, 2].CellType = txt;
                            Fpspread1.Sheets[0].Cells[i, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["APP_No"]);
                            Fpspread1.Sheets[0].Cells[i, 2].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[i, 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[i, 2].Locked = true;
                        }
                        Fpspread1.Visible = true;
                        //pheaderfilter2.Visible = true;
                        //pcolumnorder2.Visible = true;
                        rptprint.Visible = true;
                        btn_delete.Visible = true;
                        //div1.Visible = true;
                        lbl_error.Visible = false;
                        Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    }
                    else
                    {
                        Fpspread1.Visible = false;
                        rptprint.Visible = false;
                        btn_delete.Visible = false;
                        //div1.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "No Records Found";
                        //pheaderfilter.Visible = false;
                        //pcolumnorder.Visible = false;
                        //pheaderfilter.Visible = false;
                    }
                }
                else
                {
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                    btn_delete.Visible = false;
                    //div1.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "No Records Found";
                    //pheaderfilter.Visible = false;
                }
            }
        }
        catch
        {

        }

    }
    protected void rdb_checkedchanged(object sender, EventArgs e)
    {
        if (rdb_student1.Checked == true)
        {
            pheaderfilter.Visible = true;
            pheaderfilter1.Visible = false;
            pheaderfilter2.Visible = false;
            pcolumnorder.Visible = true;
            pcolumnorder1.Visible = false;
            pcolumnorder2.Visible = false;
            rdb_staff1.Checked = false;
            rdb_other1.Checked = false;
        }
        if (rdb_staff1.Checked == true)
        {
            pheaderfilter.Visible = false;
            pheaderfilter1.Visible = true;
            pheaderfilter2.Visible = false;
            pcolumnorder1.Visible = true;
            pcolumnorder.Visible = false;
            pcolumnorder2.Visible = false;
            rdb_student1.Checked = false;
            rdb_other1.Checked = false;
        }
        if (rdb_other1.Checked == true)
        {
            pheaderfilter.Visible = false;
            pheaderfilter1.Visible = false;
            pheaderfilter2.Visible = true;
            pcolumnorder2.Visible = true;
            pcolumnorder1.Visible = false;
            pcolumnorder.Visible = false;
            rdb_student1.Checked = false;
            rdb_staff1.Checked = false;
        }

    }
    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        Fpspread2.Visible = false;
        btn_save.Visible = false;
        btn_exit.Visible = false;
        btn_delete.Visible = false;
        poperrjs.Visible = true;
        bindmessname();
        bindhostelname1();
        bindbuild1();
        bindfloor1();
        bindroom1();
        lbl_errorsearch1.Visible = false;
        Bindsession();
        if (ddl_sessionaname.Items.Count > 0)
        {
            ddl_sessionaname.SelectedItem.Text = "Select";
        }
        //ddl_month.SelectedItem.Text = "Select";
        // ddl_year.SelectedItem.Text = "Select";
        txt_rollnum.Text = "";
        txt_tokendate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");

        loadmenuname();
        clearpopup();
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
    //staff
    public void cblcolumnorder_SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {
            CheckBox_column1.Checked = false;
            string value = "";
            int index;
            cblcolumnorder1.Items[0].Selected = true;
            // cblcolumnorder.Items[0].Enabled = false;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorder1.Items[index].Selected)
            {
                if (!Itemindex1.Contains(sindex))
                {
                    ItemList1.Add(cblcolumnorder1.Items[index].Value.ToString());
                    Itemindex1.Add(sindex);
                }
            }
            else
            {
                ItemList1.Remove(cblcolumnorder1.Items[index].Value.ToString());
                Itemindex1.Remove(sindex);
            }
            for (int i = 0; i < cblcolumnorder1.Items.Count; i++)
            {
                if (cblcolumnorder1.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList1.Remove(cblcolumnorder1.Items[i].Value.ToString());
                    Itemindex1.Remove(sindex);
                }
            }

            lnk_columnorder1.Visible = true;
            tborder1.Visible = true;
            tborder1.Text = "";
            string colname12 = "";
            for (int i = 0; i < ItemList1.Count; i++)
            {
                if (colname12 == "")
                {
                    colname12 = ItemList1[i].ToString() + "(" + (i + 1).ToString() + ")";

                }
                else
                {
                    colname12 = colname12 + "," + ItemList1[i].ToString() + "(" + (i + 1).ToString() + ")";
                }
            }
            tborder1.Text = colname12;
            if (ItemList1.Count == 14)
            {
                CheckBox_column1.Checked = true;
            }
            if (ItemList1.Count == 0)
            {
                tborder1.Visible = false;
                lnk_columnorder1.Visible = false;
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void CheckBox_column_CheckedChanged1(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column1.Checked == true)
            {
                tborder1.Text = "";
                ItemList1.Clear();
                for (int i = 0; i < cblcolumnorder1.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder1.Items[i].Selected = true;
                    lnk_columnorder1.Visible = true;
                    ItemList1.Add(cblcolumnorder1.Items[i].Value.ToString());
                    Itemindex1.Add(si);
                }
                lnk_columnorder1.Visible = true;
                tborder1.Visible = true;
                tborder1.Text = "";
                int j = 0;
                string colname12 = "";
                for (int i = 0; i < ItemList1.Count; i++)
                {
                    j = j + 1;
                    if (colname12 == "")
                    {
                        colname12 = ItemList1[i].ToString() + "(" + (j).ToString() + ")";

                    }
                    else
                    {
                        colname12 = colname12 + "," + ItemList1[i].ToString() + "(" + (j).ToString() + ")";
                    }
                    // tborder.Text = tborder.Text + ItemList[i].ToString();



                }
                tborder1.Text = colname12;

            }
            else
            {
                for (int i = 0; i < cblcolumnorder1.Items.Count; i++)
                {
                    cblcolumnorder1.Items[i].Selected = false;
                    lnk_columnorder1.Visible = false;
                    ItemList1.Clear();
                    Itemindex1.Clear();
                }
                tborder1.Text = "";
                tborder1.Visible = false;

            }
        }
        catch (Exception ex)
        {

        }
    }
    public void LinkButtonsremove_Click1(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder1.ClearSelection();
            CheckBox_column1.Checked = false;
            lnk_columnorder1.Visible = false;
            ItemList1.Clear();
            Itemindex1.Clear();
            tborder1.Text = "";
            tborder1.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }
    //guest
    public void cblcolumnorder_SelectedIndexChanged2(object sender, EventArgs e)
    {
        try
        {
            CheckBox_column2.Checked = false;
            string value = "";
            int index;
            cblcolumnorder2.Items[0].Selected = true;
            // cblcolumnorder.Items[0].Enabled = false;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorder2.Items[index].Selected)
            {
                if (!Itemindex2.Contains(sindex))
                {
                    ItemList2.Add(cblcolumnorder2.Items[index].Value.ToString());
                    Itemindex2.Add(sindex);
                }
            }
            else
            {
                ItemList2.Remove(cblcolumnorder2.Items[index].Value.ToString());
                Itemindex2.Remove(sindex);
            }
            for (int i = 0; i < cblcolumnorder2.Items.Count; i++)
            {
                if (cblcolumnorder2.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList2.Remove(cblcolumnorder2.Items[i].Value.ToString());
                    Itemindex2.Remove(sindex);
                }
            }

            lnk_columnorder2.Visible = true;
            tborder2.Visible = true;
            tborder2.Text = "";
            string colname12 = "";
            for (int i = 0; i < ItemList2.Count; i++)
            {
                if (colname12 == "")
                {
                    colname12 = ItemList2[i].ToString() + "(" + (i + 1).ToString() + ")";

                }
                else
                {
                    colname12 = colname12 + "," + ItemList2[i].ToString() + "(" + (i + 1).ToString() + ")";
                }
            }
            tborder2.Text = colname12;
            if (ItemList2.Count == 14)
            {
                CheckBox_column2.Checked = true;
            }
            if (ItemList2.Count == 0)
            {
                tborder2.Visible = false;
                lnk_columnorder2.Visible = false;
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void CheckBox_column_CheckedChanged2(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column2.Checked == true)
            {
                tborder2.Text = "";
                ItemList2.Clear();
                for (int i = 0; i < cblcolumnorder2.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder2.Items[i].Selected = true;
                    lnk_columnorder2.Visible = true;
                    ItemList2.Add(cblcolumnorder2.Items[i].Value.ToString());
                    Itemindex2.Add(si);
                }
                lnk_columnorder2.Visible = true;
                tborder2.Visible = true;
                tborder2.Text = "";
                int j = 0;
                string colname12 = "";
                for (int i = 0; i < ItemList2.Count; i++)
                {
                    j = j + 1;
                    if (colname12 == "")
                    {
                        colname12 = ItemList2[i].ToString() + "(" + (j).ToString() + ")";

                    }
                    else
                    {
                        colname12 = colname12 + "," + ItemList2[i].ToString() + "(" + (j).ToString() + ")";
                    }
                }
                tborder2.Text = colname12;
            }
            else
            {
                for (int i = 0; i < cblcolumnorder2.Items.Count; i++)
                {
                    cblcolumnorder2.Items[i].Selected = false;
                    lnk_columnorder2.Visible = false;
                    ItemList2.Clear();
                    Itemindex2.Clear();
                }
                tborder2.Text = "";
                tborder2.Visible = false;
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void LinkButtonsremove_Click2(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder2.ClearSelection();
            CheckBox_column2.Checked = false;
            lnk_columnorder2.Visible = false;
            ItemList2.Clear();
            Itemindex2.Clear();
            tborder2.Text = "";
            tborder2.Visible = false;
        }
        catch (Exception ex)
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
                d2.printexcelreport(Fpspread1, reportname);
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
    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Student Token Entry Report";
            string pagename = "student_token_details.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }
    }

    //Student Token Entry ** poperrjs **

    //public void loadyear()
    //{
    //    ddl_year.Items.Clear();
    //    int year = Convert.ToInt32(System.DateTime.Now.ToString("yyyy"));
    //    for (int l = 0; l < 15; l++)
    //    {
    //        ddl_year.Items.Add(Convert.ToString(year));
    //        year--;
    //    }
    //    ddl_year.Items.Insert(0, "Select");
    //}
    //public void loadmonth()
    //{
    //    ddl_month.Items.Clear();
    //    ddl_month.Items.Add(new ListItem("Select", "0"));
    //    ddl_month.Items.Add(new ListItem("January", "01"));
    //    ddl_month.Items.Add(new ListItem("February", "02"));
    //    ddl_month.Items.Add(new ListItem("March", "03"));
    //    ddl_month.Items.Add(new ListItem("April", "04"));
    //    ddl_month.Items.Add(new ListItem("May", "05"));
    //    ddl_month.Items.Add(new ListItem("June", "06"));
    //    ddl_month.Items.Add(new ListItem("July", "07"));
    //    ddl_month.Items.Add(new ListItem("August", "08"));
    //    ddl_month.Items.Add(new ListItem("September", "09"));
    //    ddl_month.Items.Add(new ListItem("October", "10"));
    //    ddl_month.Items.Add(new ListItem("November", "11"));
    //    ddl_month.Items.Add(new ListItem("December", "12"));
    //}
    public void clearpopup()
    {
        txt_tokendate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        Bindsession();
        loadmenuname();
        txt_rollnum.Text = "";
        //loadmonth();
        //loadyear();
        Fpspread2.Sheets[0].RowCount = 0;
        Fpspread2.Sheets[0].ColumnCount = 0;
    }

    public void bindhostelname1()
    {
        try
        {
            cbl_hostelname1.Items.Clear();
            ds.Clear();
            //string qu = "select HostelMasterPK,HostelName from HM_HostelMaster where MessMasterFK in('" + ddl_messname.SelectedItem.Value + "')";
            //ds = d2.select_method_wo_parameter(qu, "Text");

            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname1.DataSource = ds;
                cbl_hostelname1.DataTextField = "HostelName";
                cbl_hostelname1.DataValueField = "HostelMasterPK";
                cbl_hostelname1.DataBind();
                if (cbl_hostelname1.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
                    {
                        cbl_hostelname1.Items[i].Selected = true;
                    }
                    txt_hostelname1.Text = "Hostel Name(" + cbl_hostelname1.Items.Count + ")";
                    cb_hostelname1.Checked = true;
                }
            }
            else
            {
                txt_hostelname1.Text = "--Select--";
                cb_hostelname1.Checked = false;
            }
        }
        catch
        {
        }
    }
    protected void cb_hostelname1_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;

        txt_hostelname1.Text = "--Select--";
        if (cb_hostelname1.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
            {
                cbl_hostelname1.Items[i].Selected = true;
            }
            cb_hostelname1.Checked = true;
            txt_hostelname1.Text = "Hostel Name(" + (cbl_hostelname1.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
            {
                cbl_hostelname1.Items[i].Selected = false;
            }
            txt_hostelname1.Text = "--Select--";
            cb_hostelname1.Checked = false;
        }

        bindbuild1();
        bindfloor1();
        bindroom1();
    }
    protected void cbl_hostelname1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            cb_hostelname1.Checked = false;
            int commcount = 0;

            txt_hostelname1.Text = "--Select--";
            for (i = 0; i < cbl_hostelname1.Items.Count; i++)
            {

                if (cbl_hostelname1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_hostelname1.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_hostelname1.Items.Count)
                {
                    cb_hostelname1.Checked = true;
                }
                txt_hostelname1.Text = "Hostel Name(" + commcount.ToString() + ")";
            }

            bindbuild1();
            bindfloor1();
            bindroom1();
        }
        catch (Exception ex)
        {

        }
    }

    public void bindbuild1()
    {
        try
        {
            cbl_building1.Items.Clear();
            //txt_building.Text = "---Select---";
            //cb_building.Checked = false;
            string build = "";
            if (cbl_hostelname1.Items.Count > 0)
            {
                for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
                {
                    if (cbl_hostelname1.Items[i].Selected == true)
                    {
                        if (build == "")
                        {
                            build = Convert.ToString(cbl_hostelname1.Items[i].Value);
                        }
                        else
                        {
                            build = build + "'" + "," + "'" + Convert.ToString(cbl_hostelname1.Items[i].Value);
                        }
                    }
                }
            }
            string bul = "";
            if (build != "")
            {
                bul = d2.GetBuildingCode_inv(build);
                ds = d2.BindBuilding(bul);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_building1.DataSource = ds;
                    cbl_building1.DataTextField = "Building_Name";
                    cbl_building1.DataValueField = "code";
                    cbl_building1.DataBind();
                    if (cbl_building1.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_building1.Items.Count; i++)
                        {
                            cbl_building1.Items[i].Selected = true;
                        }
                        cb_building1.Checked = true;
                        txt_building1.Text = "Building Name(" + cbl_building1.Items.Count + ")";
                    }
                }
            }
            else
            {
                cb_building1.Checked = false;
                txt_building1.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_building1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_building1.Text = "--Select--";
            if (cb_building1.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_building1.Items.Count; i++)
                {
                    cbl_building1.Items[i].Selected = true;
                }
                txt_building1.Text = "Building Name(" + (cbl_building1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_building1.Items.Count; i++)
                {
                    cbl_building1.Items[i].Selected = false;
                }
                txt_building1.Text = "--Select--";
            }
            bindfloor1();
            bindroom1();
        }
        catch
        {
        }
    }
    protected void cblbuilding1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            string buildvalue = "";
            string build = "";
            cb_building1.Checked = false;
            int commcount = 0;
            txt_building1.Text = "--Select--";
            for (i = 0; i < cbl_building1.Items.Count; i++)
            {
                if (cbl_building1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_building1.Checked = false;
                    build = cbl_building1.Items[i].Text.ToString();
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
                if (commcount == cbl_building1.Items.Count)
                {
                    cb_building1.Checked = true;
                }
                txt_building1.Text = "Building Name(" + commcount.ToString() + ")";
            }
            bindfloor1();
            bindroom1();
        }
        catch (Exception ex)
        {

        }
    }

    public void bindfloor1()
    {
        try
        {
            string floorname = "";
            cbl_floor1.Items.Clear();
            txt_floor1.Text = "---Select---";
            cb_floor1.Checked = false;
            if (cbl_building1.Items.Count > 0)
            {
                for (int i = 0; i < cbl_building1.Items.Count; i++)
                {
                    if (cbl_building1.Items[i].Selected == true)
                    {
                        if (floorname == "")
                        {
                            floorname = Convert.ToString(cbl_building1.Items[i].Text);
                        }
                        else
                        {
                            floorname = floorname + "'" + "," + "'" + Convert.ToString(cbl_building1.Items[i].Text);
                        }
                    }
                }
            }
            if (floorname != "")
            {
                ds = d2.BindFloor_new(floorname);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_floor1.DataSource = ds;
                    cbl_floor1.DataTextField = "Floor_Name";
                    cbl_floor1.DataValueField = "FloorPK";
                    cbl_floor1.DataBind();
                    if (cbl_floor1.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_floor1.Items.Count; i++)
                        {
                            cbl_floor1.Items[i].Selected = true;
                        }

                        txt_floor1.Text = "Floor (" + cbl_floor1.Items.Count + ")";
                        cb_floor1.Checked = true;
                    }
                }
            }
            else
            {
                cb_floor1.Checked = false;
                txt_floor1.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbfloor1_CheckedChanged(object sender, EventArgs e)
    {

        try
        {
            int cout = 0;
            txt_floor1.Text = "--Select--";
            if (cb_floor1.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_floor1.Items.Count; i++)
                {
                    cbl_floor1.Items[i].Selected = true;
                }
                txt_floor1.Text = "Floor (" + (cbl_floor1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_floor1.Items.Count; i++)
                {
                    cbl_floor1.Items[i].Selected = false;
                }
                txt_floor1.Text = "--Select--";
            }

            bindroom1();
        }
        catch
        {
        }
    }
    protected void cblfloor1_SelectedIndexChanged(object sender, EventArgs e)
    {
        cbl_room1.Items.Clear();
        txt_room1.Text = "--Select--";

        cb_room1.Checked = false;
        int i = 0;
        //cbBuilding.Checked = false;
        int commcount = 0;

        txt_floor1.Text = "--Select--";
        cb_floor1.Checked = false;


        for (i = 0; i < cbl_floor1.Items.Count; i++)
        {
            if (cbl_floor1.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                //cb_floor.Checked = false;

            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_floor1.Items.Count)
            {
                cb_floor1.Checked = true;
            }
            txt_floor1.Text = "Floor (" + commcount.ToString() + ")";
        }
        bindroom1();
    }

    public void bindroom1()
    {
        try
        {
            cbl_room1.Items.Clear();
            txt_room1.Text = "---Select---";
            cb_room1.Checked = false;
            string flooor = "";
            string room = "";
            if (cbl_building1.Items.Count > 0)
            {
                for (int i = 0; i < cbl_building1.Items.Count; i++)
                {
                    if (cbl_building1.Items[i].Selected == true)
                    {
                        if (flooor == "")
                        {
                            flooor = Convert.ToString(cbl_building1.Items[i].Text);
                        }
                        else
                        {
                            flooor = flooor + "'" + "," + "'" + Convert.ToString(cbl_building1.Items[i].Text);
                        }
                    }
                }
            }
            if (cbl_floor1.Items.Count > 0)
            {
                for (int i = 0; i < cbl_floor1.Items.Count; i++)
                {
                    if (cbl_floor1.Items[i].Selected == true)
                    {
                        if (room == "")
                        {
                            room = Convert.ToString(cbl_floor1.Items[i].Text);
                        }
                        else
                        {
                            room = room + "'" + "," + "'" + Convert.ToString(cbl_floor1.Items[i].Text);
                        }
                    }
                }
            }

            if (flooor != "" && room != "")
            {
                ds = d2.BindRoom(room, flooor);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_room1.DataSource = ds;
                    cbl_room1.DataTextField = "Room_Name";
                    cbl_room1.DataValueField = "Roompk";
                    cbl_room1.DataBind();

                    if (cbl_room1.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_room1.Items.Count; row++)
                        {
                            cbl_room1.Items[row].Selected = true;
                        }
                        txt_room1.Text = "Room (" + cbl_room1.Items.Count + ")";
                        cb_room1.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_room1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            int commcount = 0;
            txt_room1.Text = "--Select--";
            if (cb_room1.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_room1.Items.Count; i++)
                {
                    cbl_room1.Items[i].Selected = true;
                }
                txt_room1.Text = "Room (" + (cbl_room1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_room1.Items.Count; i++)
                {
                    cbl_room1.Items[i].Selected = false;
                }
                txt_room1.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void cbl_room1_SelectedIndexChanged(object sender, EventArgs e)
    {
        cb_room1.Checked = false;
        int commcount = 0;

        txt_room1.Text = "--Select--";

        for (int i = 0; i < cbl_room1.Items.Count; i++)
        {
            if (cbl_room1.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                //cb_room.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_room1.Items.Count)
            {
                cb_room1.Checked = true;
            }
            txt_room1.Text = "Room (" + commcount.ToString() + ")";
        }
    }

    protected void txt_tokendate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Bindsession();
            loadmenuname();
        }
        catch
        {
        }
    }

    public void Bindsession()
    {
        try
        {
            ds.Clear();
            ddl_sessionaname.Items.Clear();
            // cbl_sessionname.Items.Clear();
            string itemheader = "";
            //if (cbl_hostelname1.Items.Count > 0)
            //{
            //    for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
            //    {
            //        if (cbl_hostelname1.Items[i].Selected == true)
            //        {
            //            if (itemheader == "")
            //            {
            //                itemheader = Convert.ToString(cbl_hostelname1.Items[i].Value);
            //            }
            //            else
            //            {
            //                itemheader = itemheader + "'" + "," + "'" + Convert.ToString(cbl_hostelname1.Items[i].Value);
            //            }
            //        }
            //    }
            //}

            itemheader = ddl_messname.SelectedItem.Value;
            if (itemheader.Trim() != "")
            {
                ds = d2.BindSession_inv(itemheader);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_sessionaname.DataSource = ds;
                    ddl_sessionaname.DataTextField = "SessionName";
                    ddl_sessionaname.DataValueField = "SessionMasterPK";
                    ddl_sessionaname.DataBind();
                    ddl_sessionaname.Items.Insert(0, "Select");
                    //if (cbl_sessionname.Items.Count > 0)
                    //{
                    //    for (int i = 0; i < cbl_sessionname.Items.Count; i++)
                    //    {
                    //        cbl_sessionname.Items[i].Selected = true;
                    //    }
                    //    cb_sessionname.Checked = true;
                    //    txt_sessionname.Text = "Session Name(" + cbl_sessionname.Items.Count + ")";
                    //}
                }
                else
                {
                    ddl_sessionaname.Items.Insert(0, "Select");
                    //txt_sessionname.Text = "--Select--";
                    //cb_sessionname.Checked = false;
                }
            }
            else
            {
                ddl_sessionaname.Items.Insert(0, "Select");
                //txt_sessionname.Text = "--Select--";
                //cb_sessionname.Checked = false;
            }
        }
        catch
        {
        }
    }
    protected void ddl_sessionname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadmenuname();
            lbl_errormessage.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddl_messname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindhostelname1();
            bindbuild1();
            bindfloor1();
            bindroom1();
            Bindsession();
            loadmenuname();
        }
        catch (Exception ex)
        {
        }
    }

    public void loadmenuname()
    {
        try
        {
            hat.Clear();
            string item = "";
            txt_menuname.Text = "--Select--";
            cb_menuname.Checked = false;
            string itemheadercode = "";
            cbl_menuname.Items.Clear();
            string hostelcode = "";
            //for (int i = 0; i < ddl_sessionaname.Items.Count; i++)
            //{
            //    if (ddl_sessionaname.SelectedIndex != 0 && ddl_sessionaname.SelectedIndex != -1)
            //    {
            //        if (item == "")
            //        {
            //            item = "" + ddl_sessionaname.SelectedItem.Value.ToString() + "";
            //        }
            //        else
            //        {
            //            item = item + "'" + "," + "'" + ddl_sessionaname.SelectedItem.Value.ToString() + "";
            //        }
            //    }

            //}
            //for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
            //{
            //    if (cbl_hostelname1.Items[i].Selected == true)
            //    {
            //        if (hostelcode == "")
            //        {
            //            hostelcode = "" + cbl_hostelname1.Items[i].Value.ToString() + "";
            //        }
            //        else
            //        {
            //            hostelcode = hostelcode + "'" + "," + "'" + cbl_hostelname1.Items[i].Value.ToString() + "";
            //        }
            //    }
            //}

            item = Convert.ToString(ddl_sessionaname.SelectedItem.Value);
            hostelcode = Convert.ToString(ddl_messname.SelectedItem.Value);

            if (item.Trim() != "")
            {
                string firstdate = Convert.ToString(txt_tokendate.Text);
                DateTime dt = new DateTime();
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                if (item.Trim() != "Select" && hostelcode.Trim() != "")
                {
                    string menuquery = "";
                    menuquery = "select MenuMasterFK,SessionMasterFK from HT_MenuSchedule where SessionMasterFK in('" + item + "') and MessMasterFK in('" + hostelcode + "')  and ScheudleItemType='1' and ScheduleType ='1' and MenuScheduleDate ='" + dt.ToString("MM/dd/yyyy") + "'";
                    menuquery = menuquery + "  select MenuMasterFK,SessionMasterFK from HT_MenuSchedule where SessionMasterFK in('" + item + "') and MessMasterFK in('" + hostelcode + "')  and ScheudleItemType='1' and ScheduleType ='2' and MenuScheduleday ='" + dt.ToString("dddd") + "'";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(menuquery, "Text");
                    menuquery = ""; string menucode = "";
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
                    else
                    {
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
                    }
                    string deptquery = "select distinct MenuMasterPK,MenuName,MenuCode  from HM_MenuMaster where CollegeCode ='" + collegecode1 + "' and MenuMasterPK in('" + menucode + "')  order by MenuName ";
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
                            cb_menuname.Checked = true;
                            txt_menuname.Text = "Menu Name(" + cbl_menuname.Items.Count + ")";
                            lbl_menuname.Text = "Menu Name";
                        }
                        else
                        {
                            txt_menuname.Text = "--Select--";
                            cb_menuname.Checked = false;
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
                txt_menuname.Text = "Menu Name(" + (cbl_menuname.Items.Count) + ")";
                //if (rdb_menuitemcon.Checked == true)
                //{
                //    txt_menuname.Text = "Menu Name(" + (cbl_menuname.Items.Count) + ")";
                //}
                //else
                //{
                //    txt_menuname.Text = "Item Name(" + (cbl_menuname.Items.Count) + ")";
                //}
            }
        }
        ////////}
        else
        {
            for (int i = 0; i < cbl_menuname.Items.Count; i++)
            {
                cbl_menuname.Items[i].Selected = false;
            }

            txt_menuname.Text = "--Select--";
            //cb_menuname.Checked = false;
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

            txt_menuname.Text = "Menu Name(" + commcount.ToString() + ")";
            //}
            //else
            //{
            //    txt_menuname.Text = "Item Name(" + commcount.ToString() + ")";
            //}
            if (commcount == cbl_menuname.Items.Count)
            {
                cb_menuname.Checked = true;
            }

        }
    }

    protected void btn_go1_OnClick(object sender, EventArgs e)
    {
        try
        {
            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnCount = 0;
            Fpspread2.Visible = false;
            btn_save.Visible = false;
            btn_exit.Visible = false;
            int sno = 0;
            string hostel = ddl_messname.SelectedItem.Value.ToString();
            //for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
            //{
            //    if (cbl_hostelname1.Items[i].Selected == true)
            //    {
            //        if (hostel == "")
            //        {
            //            hostel = "" + cbl_hostelname1.Items[i].Value.ToString() + "";
            //        }
            //        else
            //        {
            //            hostel = hostel + "'" + "," + "'" + cbl_hostelname1.Items[i].Value.ToString() + "";
            //        }
            //    }
            //}
            hostel = "";
            string building = "";
            for (int i = 0; i < cbl_building1.Items.Count; i++)
            {
                if (cbl_building1.Items[i].Selected == true)
                {
                    if (building == "")
                    {
                        building = "" + cbl_building1.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        building = building + "'" + "," + "'" + cbl_building1.Items[i].Value.ToString() + "";
                    }
                }
            }
            string floor = "";
            for (int i = 0; i < cbl_floor1.Items.Count; i++)
            {
                if (cbl_floor1.Items[i].Selected == true)
                {
                    if (floor == "")
                    {
                        floor = "" + cbl_floor1.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        floor = floor + "'" + "," + "'" + cbl_floor1.Items[i].Value.ToString() + "";
                    }
                }
            }
            string room = "";
            for (int i = 0; i < cbl_room1.Items.Count; i++)
            {
                if (cbl_room1.Items[i].Selected == true)
                {
                    if (room == "")
                    {
                        room = "" + cbl_room1.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        room = room + "'" + "," + "'" + cbl_room1.Items[i].Value.ToString() + "";
                    }
                }
            }
            //string sessionname = "";
            //for (int i = 0; i < cbl_sessionname.Items.Count; i++)
            //{
            //    if (cbl_sessionname.Items[i].Selected == true)
            //    {
            //        if (sessionname == "")
            //        {
            //            sessionname = "" + cbl_sessionname.Items[i].Text.ToString() + "";
            //        }
            //        else
            //        {
            //            sessionname = sessionname + "'" + "," + "'" + cbl_sessionname.Items[i].Text.ToString() + "";
            //        }
            //    }
            //}
            string menu = "";
            string mcode = "";
            for (int i = 0; i < cbl_menuname.Items.Count; i++)
            {
                if (cbl_menuname.Items[i].Selected == true)
                {
                    if (menu == "")
                    {
                        menu = "" + cbl_menuname.Items[i].Text.ToString() + "";
                        mcode = "" + cbl_menuname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        menu = menu + "'" + "," + "'" + cbl_menuname.Items[i].Text.ToString() + "";
                        mcode = mcode + "'" + "," + "'" + cbl_menuname.Items[i].Value.ToString() + "";
                    }
                }
            }
            string selectQuery = "";
            //selectQuery = "  select r.Roll_No,r.Reg_No,r.Stud_Name,r.Stud_Type,hs.Hostel_Code,C.Course_Name,Dt.Dept_Name,r.Current_Semester,r.Sections,hs.Building_Name,Floor_Name,Room_Name,h.Hostel_Name from Hostel_Details h,Hostel_StudentDetails hs,Registration r,Degree d,Department dt,Course c  where h.Hostel_code =hs.Hostel_Code and hs.Roll_No  =r.Roll_No and d.Degree_Code =r.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id  and Relived=0 and hs.Suspension =0 and hs.Vacated =0 ";
            string header = ""; string roll = ""; string count = "";
            if (rdb_student.Checked == true)
            {
                header = "Student Name"; roll = "Roll No"; count = "Student";
                selectQuery = " select r.Roll_No,r.Reg_No,r.Stud_Name,r.Stud_Type,hs.HostelMasterFK , C.Course_Name,Dt.Dept_Name, r.Current_Semester,r.Sections,bm.Building_Name,fm.Floor_Name,Room_Name,h.HostelName,r.App_No from HM_HostelMaster h, HT_HostelRegistration hs, Registration r,Degree d,Department dt,Course c ,Room_Detail rd,Building_Master bm,Floor_Master fm where hs.APP_No =r.App_No and d.Degree_Code =r.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and ISNULL(IsSuspend,0)=0 and ISNULL(IsDiscontinued,0)=0 and ISNULL(IsVacated,0)=0 and h.HostelMasterPK=hs.HostelMasterFK and rd.Roompk=hs.RoomFK and fm.Floorpk=hs.FloorFK and bm.Code=hs.BuildingFK";
                if (hostel != "")
                {
                    selectQuery += "AND h.HostelMasterPK in ('" + hostel + "')";
                }
                if (building != "")
                {
                    selectQuery += " AND hs.BuildingFK in ('" + building + "')";
                }
                if (floor != "")
                {
                    selectQuery += " AND hs.FloorFK in ('" + floor + "')";
                }
                if (room != "")
                {
                    selectQuery += " AND ltrim(hs.RoomFK) in ('" + room + "')";
                }
                if (txt_rollnum.Text.Trim() != "")
                {
                    selectQuery += " and R.Roll_No LIKE '" + Convert.ToString(txt_rollnum.Text) + "%'";
                }
                selectQuery += " order by h.HostelMasterPK,R.Roll_No";
            }
            else if (rdb_staff.Checked == true)
            {
                header = "Staff Name"; roll = "Staff Code"; count = "Staff";
                selectQuery = "  select s.staff_name as Stud_Name,s.staff_code as Roll_No,a.appl_id as App_No,hs.HostelMasterFK , bm.Building_Name,fm.Floor_Name,Room_Name,h.HostelName from HM_HostelMaster h, HT_HostelRegistration hs, staffmaster s,staff_appl_master a,Room_Detail rd,Building_Master bm,Floor_Master fm where  s.appl_no=a.appl_no and  hs.APP_No =a.appl_id and  ISNULL(IsSuspend,0)=0 and ISNULL(IsDiscontinued,0)=0 and ISNULL(IsVacated,0)=0 and h.HostelMasterPK=hs.HostelMasterFK and rd.Roompk=hs.RoomFK and fm.Floorpk=hs.FloorFK and bm.Code=hs.BuildingFK and hs.MemType='2'";

                if (hostel != "")
                {
                    selectQuery += "AND h.HostelMasterPK in ('" + hostel + "')";
                }
                if (building != "")
                {
                    selectQuery += " AND hs.BuildingFK in ('" + building + "')";
                }
                if (floor != "")
                {
                    selectQuery += " AND hs.FloorFK in ('" + floor + "')";
                }
                if (room != "")
                {
                    selectQuery += " AND ltrim(hs.RoomFK) in ('" + room + "')";
                }
                //if (txt_rollnum.Text.Trim() != "")
                //{
                //    selectQuery += " and R.Roll_No LIKE '" + Convert.ToString(txt_rollnum.Text) + "%'";
                //}

                selectQuery += " order by h.HostelMasterPK,S.staff_name ";

            }
            else if (rdb_other.Checked == true)
            {
                header = "Guest Name"; roll = "Guest Code"; count = "Guest";

                selectQuery = "  select VendorCompName as Stud_Name,APP_No as Roll_No,App_No,hs.HostelMasterFK , bm.Building_Name,fm.Floor_Name,Room_Name,h.HostelName from HM_HostelMaster h, HT_HostelRegistration hs,CO_VendorMaster vm ,Room_Detail rd,Building_Master bm,Floor_Master fm where vm.VendorPK=hs.app_no and ISNULL(IsSuspend,0)=0 and ISNULL(IsDiscontinued,0)=0 and ISNULL(IsVacated,0)=0 and h.HostelMasterPK=hs.HostelMasterFK and rd.Roompk=hs.RoomFK and fm.Floorpk=hs.FloorFK and bm.Code=hs.BuildingFK and hs.MemType='3'";
                if (hostel != "")
                {
                    selectQuery += "AND h.HostelMasterPK in ('" + hostel + "')";
                }
                if (building != "")
                {
                    selectQuery += " AND hs.BuildingFK in ('" + building + "')";
                }
                if (floor != "")
                {
                    selectQuery += " AND hs.FloorFK in ('" + floor + "')";
                }
                if (room != "")
                {
                    selectQuery += " AND ltrim(hs.RoomFK) in ('" + room + "')";
                }
                //if (txt_rollnum.Text.Trim() != "")
                //{
                //    selectQuery += " and R.Roll_No LIKE '" + Convert.ToString(txt_rollnum.Text) + "%'";
                //}
                selectQuery += " order by h.HostelMasterPK,vm.VendorCompName ";
            }

            ds.Clear();
            ds = d2.select_method_wo_parameter(selectQuery, "Text");

            string[] separators = { ",", "'" };
            string[] menuname = menu.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            string[] menucode = mcode.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            if (ddl_sessionaname.SelectedIndex == 0)
            {
                lbl_errormessage.Visible = true;
                lbl_errormessage.Text = "Select any session";
            }

            else if (menuname.Length <= 0)
            {
                //imgdiv2.Visible = true;
                lbl_errormessage.Visible = true;
                lbl_errormessage.Text = "No more menus are available for the selected session";
            }
            else if (ds.Tables[0].Rows.Count <= 0)
            {
                //imgdiv2.Visible = true;
                lbl_errormessage.Visible = true;
                lbl_errormessage.Text = "No records found";
            }
            else if (ds.Tables[0].Rows.Count > 0 && menuname.Length > 0 && ddl_sessionaname.SelectedIndex != 0)
            {

                Fpspread2.CommandBar.Visible = false;
                Fpspread2.Sheets[0].AutoPostBack = false;
                Fpspread2.Sheets[0].RowHeader.Visible = false;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                Fpspread2.CommandBar.Visible = false;
                Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread2.Sheets[0].RowHeader.Visible = false;

                Fpspread2.Sheets[0].RowCount = ds.Tables[0].Rows.Count;
                Fpspread2.Sheets[0].ColumnCount = 4 + menuname.Length;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Columns[0].Locked = true;
                Fpspread2.Columns[0].Width = 80;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = roll; //"Roll No";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Columns[2].Locked = true;
                Fpspread2.Columns[2].Width = 100;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = header;//"Name";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                Fpspread2.Columns[3].Locked = true;
                Fpspread2.Columns[3].Width = 220;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                Fpspread2.Columns[1].Width = 100;
                Fpspread2.Columns[1].Visible = false;

                Fpspread1.Width = 900;

                FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                check.AutoPostBack = true;
                FarPoint.Web.Spread.CheckBoxCellType check1 = new FarPoint.Web.Spread.CheckBoxCellType();
                check1.AutoPostBack = false;

                FarPoint.Web.Spread.DoubleCellType db = new FarPoint.Web.Spread.DoubleCellType();
                db.ErrorMessage = "Only Allow Numbers";

                for (int col = 0; col < menuname.Length; col++)
                {
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, col + 4].Text = Convert.ToString(menuname[col]);
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, col + 4].Tag = Convert.ToString(menucode[col]);
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, col + 4].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, col + 4].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, col + 4].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, col + 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Columns[4].BackColor = Color.Gainsboro;
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    Fpspread2.Sheets[0].Cells[i, 0].Text = (i + 1).ToString();
                    // Fpspread2.Sheets[0].Cells[i, 0].Font.Bold = true;
                    Fpspread2.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Cells[i, 0].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].Cells[i, 0].Font.Size = FontUnit.Medium;

                    Fpspread2.Sheets[0].Cells[i, 1].CellType = check1;
                    Fpspread2.Sheets[0].Cells[i, 1].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread2.Sheets[0].Cells[i, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                    Fpspread2.Sheets[0].Cells[i, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["HostelMasterFK"]);
                    Fpspread2.Sheets[0].Cells[i, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Cells[i, 2].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].Cells[i, 2].Font.Size = FontUnit.Medium;

                    Fpspread2.Sheets[0].Cells[i, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                    Fpspread2.Sheets[0].Cells[i, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["App_No"]);
                    Fpspread2.Sheets[0].Cells[i, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread2.Sheets[0].Cells[i, 3].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].Cells[i, 3].Font.Size = FontUnit.Medium;

                    for (int col = 0; col < menuname.Length; col++)
                    {
                        Fpspread2.Sheets[0].Cells[i, col + 4].CellType = db;
                        Fpspread2.Sheets[0].Cells[i, col + 4].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[i, col + 4].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[i, col + 4].BackColor = Color.Gainsboro;
                    }
                }
            }
            else
            {
                //imgdiv2.Visible = true;
                lbl_errormessage.Visible = true;
                lbl_errormessage.Text = "No records found";
                Fpspread2.Visible = false;
                btn_save.Visible = false;
                btn_exit.Visible = false;
            }
            if (Fpspread2.Sheets[0].RowCount > 0)
            {
                lbl_errormessage.Visible = false;
                Fpspread2.Visible = true;
                btn_save.Visible = true;
                btn_exit.Visible = true;
                //theivamani 18.11.15
                lbl_errorsearch1.Visible = true;

                lbl_errorsearch1.Text = "No of " + count + ":" + sno.ToString();
                Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
            }

        }
        catch { }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            string rollno = "";
            int query = 0;
            bool testflage = false;
            if (Fpspread2.Sheets[0].RowCount > 0)
            {
                for (int i = 0; i < Fpspread2.Sheets[0].RowCount; i++)
                {
                    Fpspread2.SaveChanges();
                    if (ddl_sessionaname.SelectedIndex == 0)
                    {
                        imgdiv2.Visible = true;
                        lbl_alerterror.Text = "Please select any session";
                    }
                    else if (ddl_sessionaname.SelectedIndex != 0 && cbl_menuname.Items.Count != 0)
                    {
                        string dtaccessdate = DateTime.Now.ToString("MM/dd/yyyy");
                        string dtaccesstime = DateTime.Now.ToLongTimeString();
                        string sessioncode = ddl_sessionaname.SelectedItem.Value.ToString();

                        DateTime tokendate = new DateTime();
                        string dt = Convert.ToString(txt_tokendate.Text);
                        string[] split = dt.Split('/');
                        tokendate = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                        if (rollno == "")
                        {
                            rollno = "" + Fpspread2.Sheets[0].Cells[i, 2].Text + "";
                        }
                        else
                        {
                            rollno = rollno + "'" + "," + "'" + Fpspread2.Sheets[0].Cells[i, 2].Text + "";
                        }

                        string menu = "";
                        string mcode = "";
                        string qty = "";
                        int col = -1;
                        for (int j = 0; j < cbl_menuname.Items.Count; j++)
                        {
                            if (cbl_menuname.Items[j].Selected == true)
                            {
                                col++;
                                if (ddl_sessionaname.SelectedIndex != 0 && ddl_sessionaname.SelectedIndex != -1)
                                {
                                    qty = "" + Fpspread2.Sheets[0].Cells[i, 4 + col].Text + "";

                                    if (qty.Trim() != "")
                                    {
                                        if (menu == "")
                                        {
                                            menu = "" + Fpspread2.Sheets[0].ColumnHeader.Cells[0, col + 4].Text + "";
                                            mcode = "" + Fpspread2.Sheets[0].ColumnHeader.Cells[0, col + 4].Tag + "";
                                        }
                                        else
                                        {
                                            menu = menu + "'" + "," + "'" + Fpspread2.Sheets[0].ColumnHeader.Cells[0, col + 4].Text + "";
                                            mcode = mcode + "'" + "," + "'" + Fpspread2.Sheets[0].ColumnHeader.Cells[0, col + 4].Tag + "";
                                        }
                                    }
                                }
                            }
                        }
                        qty = "";
                        col = -1;
                        for (int j = 0; j < cbl_menuname.Items.Count; j++)
                        {
                            if (cbl_menuname.Items[j].Selected == true)
                            {
                                col++;
                                if (ddl_sessionaname.SelectedIndex != 0 && ddl_sessionaname.SelectedIndex != -1)
                                {
                                    if (qty == "")
                                    {
                                        qty = "" + Fpspread2.Sheets[0].Cells[i, 4 + col].Text + "";
                                    }
                                    else
                                    {
                                        qty = qty + "'" + "," + "'" + Fpspread2.Sheets[0].Cells[i, 4 + col].Text + "";
                                    }
                                }
                            }
                        }
                        if (qty.Trim() != "" && menu.Trim() != "")
                        {

                            string[] separators = { ",", "'" };
                            string[] menuname = menu.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                            string[] menucode = mcode.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                            string[] quantity = qty.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                            string rno = Convert.ToString(Fpspread2.Sheets[0].Cells[i, 2].Text);

                            string hostelmasterfk = Convert.ToString(Fpspread2.Sheets[0].Cells[i, 2].Tag);
                            string appno = Convert.ToString(Fpspread2.Sheets[0].Cells[i, 3].Tag);
                            string memtype = "";
                            if (rdb_student.Checked == true)
                            { memtype = "1"; }
                            else if (rdb_staff.Checked == true) { memtype = "2"; } else if (rdb_other.Checked == true) { memtype = "3"; }

                            for (int menurow = 0; menurow < menucode.Length; menurow++)
                            {
                                string sql = "if exists (select * from HT_StudTokenDetails where App_No='" + appno + "' and  MessFK ='" + hostelmasterfk + "'  and SessionFK ='" + sessioncode + "' and MenuFK ='" + menucode[menurow] + "' and TokenDate='" + tokendate + "' and MemType='" + memtype + "') update HT_StudTokenDetails set TokenQty ='" + quantity[menurow] + "' where App_No='" + appno + "' and  MessFK='" + hostelmasterfk + "' and SessionFK='" + sessioncode + "' and MenuFK ='" + menucode[menurow] + "' and TokenDate='" + tokendate + "' and MemType='" + memtype + "' else INSERT INTO HT_StudTokenDetails(App_No,TokenDate,SessionFK,MenuFK,TokenQty,MessFK,MemType) VALUES('" + appno + "','" + tokendate + "','" + sessioncode + "','" + menucode[menurow] + "','" + quantity[menurow] + "','" + hostelmasterfk + "','" + memtype + "')";
                                query = d2.update_method_wo_parameter(sql, "TEXT");

                                string getcost = d2.GetFunction("select menuamount from HM_MenuCostMaster where MenuMasterFK in('" + menucode[menurow] + "')");
                                if (getcost.Trim() == "")
                                {
                                    getcost = "0";
                                }
                                if (getcost.Trim() != "0")
                                {
                                    double totacost = Convert.ToDouble(quantity[menurow]) * Convert.ToDouble(getcost);
                                    string insetquery = "if exists(select* from HT_StudAdditionalDet where App_No='" + appno + "' and TransDate='" + tokendate + "' and MemType='" + memtype + "')update HT_StudAdditionalDet set AdditionalAmt=AdditionalAmt+isnull('" + totacost + "',0) where App_No='" + appno + "' and TransDate='" + tokendate + "' and MemType='" + memtype + "' else insert into HT_StudAdditionalDet(MemType,App_No,TransDate,AdditionalAmt)values('" + memtype + "','" + appno + "','" + tokendate + "','" + totacost + "')";
                                    int Newinsert = d2.update_method_wo_parameter(insetquery, "Text");
                                }
                            }

                            if (query != 0)
                            {
                                testflage = true;
                            }
                        }
                    }
                }
                if (testflage == true)
                {
                    imgdiv2.Visible = true;
                    lbl_alerterror.Text = "Saved Sucessfully";
                    btn_addnew_Click(sender, e);
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alerterror.Text = "Please Update Quantity Values";
                    poperrjs.Visible = true;
                }
            }
            else
            {
                lbl_errormessage.Visible = true;
                lbl_errormessage.Text = "No records found";
            }

        }
        catch (Exception ex)
        {
        }
    }
    protected void btnexit_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
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
    protected void delete()
    {
        try
        {
            surediv.Visible = false;
            int insert = 0;
            if (Fpspread1.Sheets[0].RowCount > 0)
            {
                for (int i = 0; i < Fpspread1.Sheets[0].RowCount; i++)
                {
                    Fpspread1.SaveChanges();
                    int checkval = Convert.ToInt32(Fpspread1.Sheets[0].Cells[i, 1].Value);
                    if (checkval == 1)
                    {
                        string app_no = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 2].Tag);
                        string sql = "delete HT_StudTokenDetails where App_No='" + app_no + "'";
                        insert = d2.update_method_wo_parameter(sql, "TEXT");
                    }
                }
                if (insert != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alerterror.Text = "Deleted Sucessfully";
                    btn_go_Click(sender, e);
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alerterror.Text = "Please Select Any Record";
                    btn_go_Click(sender, e);
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alerterror.Text = "No Records Found";
                btn_go_Click(sender, e);
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
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
    }


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' and Stud_Type='Hostler' order by Roll_No";
        name = ws.Getname(query);
        return name;
    }

    public DateTime TextToDate(TextBox txt)
    {
        DateTime dt = new DateTime();
        string firstdate = Convert.ToString(txt.Text);

        string[] split = firstdate.Split('/');
        dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
        return dt;
    }
    public void bindmessname()
    {
        try
        {
            ds.Clear();
            //ds = d2.Bindmess_inv(collegecode1);
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_messname.DataSource = ds;
                ddl_messname.DataTextField = "MessName";
                ddl_messname.DataValueField = "MessMasterPK";
                ddl_messname.DataBind();
            }
        }
        catch
        {
        }
    }
}
