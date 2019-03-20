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
public partial class RoomAvailability : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    DAccess2 dt = new DAccess2();
    DataSet ds2 = new DataSet();

    string build = "";
    string floor = "";
    string flooor = "";
    string room = "";
    int i, j;
    int cout;
    int commcount;

    Hashtable hat = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
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
            lbl_validation.Text = "";
            if (!IsPostBack)
            {

                ddlbindhostel();
                clgbuild();
                clgfloor();
                clgroom();
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.Visible = false;
                tblStatus.Visible = false;
                rptprint.Visible = false;
                //divSpread.Visible = false;
                lbl_totalroom.Visible = false;
                lbl_totalvaccants.Visible = false;
                fill.Visible = false;
                partialfill.Visible = false;
                unfill.Visible = false;
                ddl_vaccant.SelectedIndex = 3;
                search();
            }
        }
        catch (Exception ex) { }
    }
    public void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }
    public void ddl_building_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clgfloor();
            clgroom();
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        search();
    }
    public void search()
    {
        try
        {
            lbl_totalroom.Visible = true;
            lbl_totalvaccants.Visible = true;
            fill.Visible = true;
            partialfill.Visible = true;
            unfill.Visible = true;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Fpspread1.Sheets[0].RowCount = 0;
            string hostelcode = Convert.ToString(ddl_hostel.SelectedValue);
            string building = "";
            if (ddl_building.Items.Count > 0)
            {
                building = Convert.ToString(ddl_building.SelectedItem.Text);
            }
            string vaccanttype = Convert.ToString(ddl_vaccant.SelectedItem.Text);

            floor = "";
            for (int i = 0; i < cbl_floor.Items.Count; i++)
            {
                if (cbl_floor.Items[i].Selected == true)
                {
                    if (floor == "")
                    {
                        floor = "" + cbl_floor.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        floor = floor + "'" + "," + "'" + cbl_floor.Items[i].Value.ToString() + "";
                    }
                }
            }
            string roomtype0 = "";
            for (int i = 0; i < cbl_room.Items.Count; i++)
            {
                if (cbl_room.Items[i].Selected == true)
                {
                    if (roomtype0 == "")
                    {
                        roomtype0 = "" + cbl_room.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        roomtype0 = roomtype0 + "'" + "," + "'" + cbl_room.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (floor.Trim() != "" && roomtype0.Trim() != "")
            {
                string bcode = d2.GetFunction(" select HostelBuildingFK  from HM_HostelMaster where HostelMasterPK ='" + hostelcode + "'");
                string selectquery = " select r.Room_type,r.Floor_Name, Room_Name,ISNULL(Students_Allowed,0) Students_Allowed,ISNULL(Avl_Student,0) Avl_Student,r.Building_Name,b.College_Code from Building_Master B,Room_Detail R where b.Building_Name =r.Building_Name and b.College_Code =r.College_Code and b.Code in (" + bcode + ")";

                if (ddl_vaccant.SelectedItem.Text.Trim().ToString() == "Filled")
                {
                    selectquery = selectquery + " AND R.Students_Allowed =  R.Avl_Student AND R.Avl_Student != 0";
                }
                else if (ddl_vaccant.SelectedItem.Text.Trim().ToString() == "Un Filled")
                {
                    selectquery = selectquery + " AND R.Avl_Student = 0";
                }
                else if (ddl_vaccant.SelectedItem.Text.Trim().ToString() == "Partially Filled")
                {
                    selectquery = selectquery + " AND R.Avl_Student != 0 And (R.Students_Allowed != R.Avl_Student)";
                }

                selectquery = selectquery + " Select Distinct F.Floor_Name+' - '+Room_Type RoomType,r.Room_type RT,f.Floor_Name FN  FROM Floor_Master F INNER JOIN Room_Detail R ON R.Floor_Name = F.Floor_Name INNER JOIN Building_Master B ON   B.Building_Name = F.Building_Name WHERE R.Building_Name in ('" + building + "') AND R.Floor_Name in ('" + floor + "') AND R.Room_Type in ('" + roomtype0 + "') ORDER BY F.Floor_Name+' - '+Room_Type";
                selectquery = selectquery + " select ISNULL(Room_Cost,0)as Room_Cost,Hostel_Code,Room_Type  from RoomCost_Master";

                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");

                int IntRoomLen = 0;
                int totalunfill = 0;
                int totalfill = 0;
                int totalpartialfill = 0;
                int totalvaccant = 0;
                string strRoomDetail = "";
                int colcnt = 0;
                Fpspread1.Sheets[0].ColumnCount = 0;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Fpspread1.CommandBar.Visible = false;
                    Fpspread1.Sheets[0].RowHeader.Visible = false;
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            Fpspread1.Sheets[0].RowHeader.Visible = false;

                            Fpspread1.Sheets[0].RowCount = Fpspread1.Sheets[0].RowCount + 1;
                            colcnt = 0;

                            if (Fpspread1.Sheets[0].ColumnCount - 1 < colcnt)
                            {
                                Fpspread1.Sheets[0].ColumnCount++;
                            }

                            string floorname = Convert.ToString(ds.Tables[1].Rows[i]["FN"]);
                            string roomtype = Convert.ToString(ds.Tables[1].Rows[i]["RT"]);
                            string alldetails = floorname + "-" + roomtype;

                            // string buildingname = Convert.ToString(ds.Tables[0].Rows[i]["Building_Name"]);

                            FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                            Fpspread1.Sheets[0].Columns[colcnt].CellType = textcel_type;
                            Fpspread1.Sheets[0].Cells[i, colcnt].Text = alldetails;
                            // 29.10.15
                            //  Fpspread1.Sheets[0].Cells[i, colcnt].Tag = ds.Tables[0].Rows[colcnt]["Building_Name"].ToString();

                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Floor/RoomType";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            Fpspread1.Sheets[0].Cells[i, 0].Font.Bold = true;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.LightSteelBlue;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].Font.Name = "Book Antiqua";
                            DataView dv = new DataView();
                            ds.Tables[0].DefaultView.RowFilter = "floor_name='" + floorname + "' and room_type='" + roomtype + "' ";
                            dv = ds.Tables[0].DefaultView;
                            if (dv.Count > 0)
                            {
                                //29.02.16
                                //Fpspread1.Sheets[0].Cells[i, colcnt].Tag = Convert.ToString(dv[0]["Building_Name"]);
                                int columncount = dv.Count;
                                for (int cnt = 0; cnt < dv.Count; cnt++)
                                {
                                    colcnt++;
                                    Fpspread1.Sheets[0].Cells[i, cnt].Tag = Convert.ToString(dv[cnt]["Building_Name"]);
                                    string s = Convert.ToString(dv[cnt]["room_name"]) + Convert.ToString(dv[cnt]["Students_Allowed"]) + Convert.ToString(dv[cnt]["Avl_Student"]);// +Convert.ToString(dv[cnt]["Room_Cost"]);
                                    //24.02.16
                                    DataView cost = new DataView(); string rmcost = "";
                                    if (ds.Tables[2].Rows.Count > 0)
                                    {
                                        for (int rmc = 0; rmc < ds.Tables[2].Rows.Count; rmc++)
                                        {
                                            ds.Tables[2].DefaultView.RowFilter = " Hostel_Code='" + hostelcode + "' and Room_Type='" + roomtype + "'";
                                            cost = ds.Tables[2].DefaultView;

                                            if (cost.Count > 0)
                                            {
                                                rmcost = Convert.ToString(cost[rmc]["Room_Cost"]);
                                            }
                                        }
                                    }
                                    if (rmcost.Trim() == "")
                                    {
                                        rmcost = "0";
                                    }
                                    s = s + rmcost;
                                    if (Fpspread1.Sheets[0].ColumnCount - 1 < colcnt)
                                    {
                                        Fpspread1.Sheets[0].ColumnCount = Fpspread1.Sheets[0].ColumnCount + 1;
                                        Fpspread1.Sheets[0].Columns[0].Locked = true;
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Room Details";
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, Fpspread1.Sheets[0].ColumnCount - 1);
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    }
                                    if (cb_includeall.Checked == true)
                                    {
                                        Fpspread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + rmcost;//(dv[cnt]["Room_Cost"]);
                                        Fpspread1.Sheets[0].Columns[colcnt].Locked = true;


                                        if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                        {
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                            totalunfill = totalunfill + 1;
                                        }
                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                        {
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                            totalpartialfill = totalpartialfill + 1;
                                        }

                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                        {
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Note = "filled";
                                            totalfill = totalfill + 1;
                                        }
                                        else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                        {
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                            totalpartialfill = totalpartialfill + 1;
                                        }
                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                        {
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                            totalunfill = totalunfill + 1;
                                        }

                                        //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                        IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + rmcost.Length;

                                        totalvaccant = totalvaccant + Convert.ToInt16(dv[cnt]["students_allowed"]) - Convert.ToInt16(dv[cnt]["avl_student"]);
                                        Fpspread1.Sheets[0].Cells[i, colcnt].Font.Size = FontUnit.Medium;
                                        Fpspread1.Sheets[0].Cells[i, colcnt].Font.Name = "Book Antiqua";
                                    }

                                    else
                                    {
                                        try
                                        {
                                            if (cb_includeall.Checked == false)
                                            {

                                                if (cbl_roomcheck.Items[0].Selected == false && cbl_roomcheck.Items[1].Selected == false && cbl_roomcheck.Items[2].Selected == false)
                                                {
                                                    Fpspread1.Sheets[0].Columns[colcnt].Locked = true;
                                                    if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                    {
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                        totalunfill = totalunfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                    {
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                        totalpartialfill = totalpartialfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                    {
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                        totalfill = totalfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                    {
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                        totalpartialfill = totalpartialfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0)
                                                    {
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                        totalunfill = totalunfill + 1;
                                                    }
                                                    strRoomDetail = strRoomDetail + (dv[cnt]["Room_Name"]);

                                                    if (IntRoomLen < strRoomDetail.Length)
                                                    {
                                                        IntRoomLen = strRoomDetail.Length;
                                                    }
                                                    Fpspread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "";
                                                    //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                    IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;

                                                    Fpspread1.Sheets[0].Cells[i, colcnt].Font.Size = FontUnit.Medium;
                                                    Fpspread1.Sheets[0].Cells[i, colcnt].Font.Name = "Book Antiqua";

                                                }
                                            }

                                            if (cbl_roomcheck.Items[0].Selected == true && cbl_roomcheck.Items[1].Selected == false && cbl_roomcheck.Items[2].Selected == false)
                                            {
                                                Fpspread1.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }

                                                Fpspread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]);

                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }

                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }

                                            else if (cbl_roomcheck.Items[0].Selected == true && cbl_roomcheck.Items[1].Selected == true && cbl_roomcheck.Items[2].Selected == false)
                                            {
                                                Fpspread1.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                Fpspread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]);
                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }

                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }

                                            else if (cbl_roomcheck.Items[1].Selected == true && cbl_roomcheck.Items[2].Selected == true && cbl_roomcheck.Items[0].Selected == false)
                                            {
                                                Fpspread1.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                Fpspread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + rmcost;//(dv[cnt]["Room_Cost"]);
                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }

                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }

                                            else if (cbl_roomcheck.Items[0].Selected == true && cbl_roomcheck.Items[2].Selected == true && cbl_roomcheck.Items[1].Selected == false)
                                            {
                                                Fpspread1.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                Fpspread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + rmcost;// (dv[cnt]["Room_Cost"]);
                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }

                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }

                                            else if (cbl_roomcheck.Items[1].Selected == true && cbl_roomcheck.Items[2].Selected == false && cbl_roomcheck.Items[0].Selected == false)
                                            {
                                                Fpspread1.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                Fpspread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Avl_Student"]);
                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }

                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }

                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }
                                            else if (cbl_roomcheck.Items[2].Selected == true && cbl_roomcheck.Items[1].Selected == false && cbl_roomcheck.Items[0].Selected == false)
                                            {
                                                Fpspread1.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                Fpspread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + rmcost;//(dv[cnt]["Room_Cost"]);
                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }

                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }
                                            else if (cbl_roomcheck.Items[0].Selected == true && cbl_roomcheck.Items[2].Selected == true && cbl_roomcheck.Items[1].Selected == true)
                                            {
                                                cb_includeall.Checked = true;
                                                Fpspread1.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                Fpspread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + rmcost;//(dv[cnt]["Room_Cost"]);

                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }

                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {

                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }
                                            totalvaccant = totalvaccant + Convert.ToInt16(dv[cnt]["students_allowed"]) - Convert.ToInt16(dv[cnt]["avl_student"]);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }

                                    for (int j = 1; j < Fpspread1.Sheets[0].ColumnCount; j++)
                                    {
                                        lbl_totalvaccants.Text = " ";
                                        lbl_totalroom.Text = " ";
                                        int totalroom = totalunfill + totalfill + totalpartialfill;
                                        lbl_totalroom.Text = "Total No.of Rooms :" + totalroom;
                                        lbl_totalvaccants.Text = "Total No.of Vacant :" + totalvaccant;
                                        fill.Text = ("Filled(" + totalfill + ")");
                                        unfill.Text = ("UnFilled(" + totalunfill + ")");
                                        partialfill.Text = ("Partially Filled(" + totalpartialfill + ")");
                                    }
                                }
                                int height = 360;
                                {
                                    //for (j = 1; j < Fpspread1.Sheets[0].RowCount; j++)
                                    //{
                                    //    height = height + Fpspread1.Sheets[0].Rows[j].Height;
                                    //}
                                    Fpspread1.Height = height;
                                    Fpspread1.SaveChanges();
                                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                                }

                                int width = 0;
                                if (Fpspread1.Sheets[0].ColumnCount == 7)
                                {
                                    Fpspread1.Sheets[0].Columns[0].Width = 400;
                                    for (j = 1; j < Fpspread1.Sheets[0].ColumnCount; j++)
                                    {
                                        width = width + Fpspread1.Sheets[0].Columns[j].Width;
                                    }
                                    width = width + 400;
                                }
                                else if (Fpspread1.Sheets[0].ColumnCount == 5)
                                {
                                    Fpspread1.Sheets[0].Columns[0].Width = 800;
                                    for (j = 1; j < Fpspread1.Sheets[0].ColumnCount; j++)
                                    {
                                        width = width + Fpspread1.Sheets[0].Columns[j].Width;
                                    }
                                    width = width + 800;
                                }
                                else
                                {
                                    width = 900;
                                }
                                Fpspread1.Width = width;
                                Fpspread1.SaveChanges();
                                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].ColumnCount;
                                Fpspread1.Sheets[0].FrozenColumnCount = 1;
                            }
                        }
                        Fpspread1.Visible = true;
                        tblStatus.Visible = true;
                        rptprint.Visible = true;
                        //divSpread.Visible = true;
                        lbl_error.Visible = false;
                        lbl_error.Text = "No Records Found";
                    }
                }
                else
                {
                    Fpspread1.Visible = false;
                    tblStatus.Visible = false;
                    rptprint.Visible = false;
                    //divSpread.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "No Records Found";
                }
            }
            else
            {
                tblStatus.Visible = false;
                Fpspread1.Visible = false;
                rptprint.Visible = false;
                //divSpread.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "No Records Found";
            }


            #region old query 06.07.16

            //if (floor.Trim() != "" && roomtype0.Trim() != "")
            //{
            //    string selectquery = "Select r.Room_type,r.Floor_Name, Room_Name,ISNULL(Students_Allowed,0) Students_Allowed,ISNULL(Avl_Student,0) Avl_Student, ISNULL(m.Room_Cost,0) Room_Cost,h.Hostel_Code  FROM Room_Detail R left join RoomCost_Master m on r.Room_type = m.Room_Type  left join Building_Master b on b.Building_Name =r.Building_Name left join Hostel_Details h on h.Building_Code =b.Code where R.Building_Name ='" + building + "' and r.Room_Type in ('" + roomtype0 + "') and Floor_Name in ('" + floor + "') and h.Hostel_Code ='" + hostelcode + "'";
            //    if (ddl_vaccant.SelectedItem.Text.Trim().ToString() == "Filled")
            //    {
            //        selectquery = selectquery + " AND R.Students_Allowed =  R.Avl_Student AND R.Avl_Student != 0";
            //    }
            //    else if (ddl_vaccant.SelectedItem.Text.Trim().ToString() == "Un Filled")
            //    {
            //        selectquery = selectquery + " AND R.Avl_Student = 0";
            //    }
            //    else if (ddl_vaccant.SelectedItem.Text.Trim().ToString() == "Partialy Filled")
            //    {
            //        selectquery = selectquery + " AND R.Avl_Student != 0 And (R.Students_Allowed != R.Avl_Student)";
            //    }

            //    selectquery = selectquery + " Select Distinct F.Floor_Name+' - '+Room_Type RoomType,r.Room_type RT,f.Floor_Name FN  FROM Floor_Master F INNER JOIN Room_Detail R ON R.Floor_Name = F.Floor_Name INNER JOIN Building_Master B ON   B.Building_Name = F.Building_Name WHERE R.Building_Name ='" + building + "' AND R.Floor_Name in ('" + floor + "') AND R.Room_Type in ('" + roomtype0 + "') ORDER BY F.Floor_Name+' - '+Room_Type";
            //    ds.Clear();
            //    ds = d2.select_method_wo_parameter(selectquery, "Text");

            //    int IntRoomLen = 0;
            //    int totalunfill = 0;
            //    int totalfill = 0;
            //    int totalpartialfill = 0;
            //    int totalvaccant = 0;
            //    string strRoomDetail = "";
            //    int colcnt = 0;
            //    Fpspread1.Sheets[0].ColumnCount = 0;

            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        Fpspread1.CommandBar.Visible = false;
            //        Fpspread1.Sheets[0].RowHeader.Visible = false;

            //        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            //        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            //        darkstyle.ForeColor = Color.White;
            //        Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            //        Fpspread1.Sheets[0].Visible = true;

            //        if (ds.Tables[1].Rows.Count > 0)
            //        {
            //            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
            //            {
            //                Fpspread1.Sheets[0].RowHeader.Visible = false;

            //                Fpspread1.Sheets[0].RowCount = Fpspread1.Sheets[0].RowCount + 1;
            //                colcnt = 0;

            //                if (Fpspread1.Sheets[0].ColumnCount - 1 < colcnt)
            //                {
            //                    Fpspread1.Sheets[0].ColumnCount++;
            //                }

            //                string floorname = Convert.ToString(ds.Tables[1].Rows[i]["FN"]);
            //                string roomtype = Convert.ToString(ds.Tables[1].Rows[i]["RT"]);
            //                string alldetails = floorname + "-" + roomtype;

            //                FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
            //                Fpspread1.Sheets[0].Columns[colcnt].CellType = textcel_type;
            //                Fpspread1.Sheets[0].Cells[i, colcnt].Text = alldetails;

            //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Floor/RoomType";
            //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            //                Fpspread1.Sheets[0].Cells[i, 0].Font.Bold = true;
            //                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.LightSteelBlue;
            //                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].Font.Size = FontUnit.Medium;
            //                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].Font.Name = "Book Antiqua";
            //                DataView dv = new DataView();
            //                ds.Tables[0].DefaultView.RowFilter = "floor_name='" + floorname + "' and room_type='" + roomtype + "' ";
            //                dv = ds.Tables[0].DefaultView;
            //                if (dv.Count > 0)
            //                {
            //                    int columncount = dv.Count;
            //                    for (int cnt = 0; cnt < dv.Count; cnt++)
            //                    {
            //                        colcnt++;
            //                        string s = Convert.ToString(dv[cnt]["room_name"]) + Convert.ToString(dv[cnt]["Students_Allowed"]) + Convert.ToString(dv[cnt]["Avl_Student"]) + Convert.ToString(dv[cnt]["Room_Cost"]);
            //                        if (Fpspread1.Sheets[0].ColumnCount - 1 < colcnt)
            //                        {
            //                            Fpspread1.Sheets[0].ColumnCount = Fpspread1.Sheets[0].ColumnCount + 1;
            //                            Fpspread1.Sheets[0].Columns[0].Locked = true;
            //                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Room Details";
            //                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            //                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            //                            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, Fpspread1.Sheets[0].ColumnCount - 1);
            //                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            //                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
            //                        }
            //                        if (cb_includeall.Checked == true)
            //                        {
            //                            Fpspread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + (dv[cnt]["Room_Cost"]);
            //                            Fpspread1.Sheets[0].Columns[colcnt].Locked = true;
            //                            if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
            //                            {

            //                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
            //                                totalunfill = totalunfill + 1;
            //                            }
            //                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
            //                            {
            //                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
            //                                totalpartialfill = totalpartialfill + 1;
            //                            }

            //                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
            //                            {
            //                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
            //                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Note = "filled";
            //                                totalfill = totalfill + 1;
            //                            }
            //                            else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
            //                            {
            //                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
            //                                totalpartialfill = totalpartialfill + 1;
            //                            }
            //                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
            //                            {
            //                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
            //                                totalunfill = totalunfill + 1;
            //                            }

            //                            IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
            //                            totalvaccant = totalvaccant + Convert.ToInt16(dv[cnt]["students_allowed"]) - Convert.ToInt16(dv[cnt]["avl_student"]);
            //                        }

            //                        else
            //                        {
            //                            try
            //                            {
            //                                if (cb_includeall.Checked == false)
            //                                {

            //                                    if (cbl_roomcheck.Items[0].Selected == false && cbl_roomcheck.Items[1].Selected == false && cbl_roomcheck.Items[2].Selected == false)
            //                                    {
            //                                        Fpspread1.Sheets[0].Columns[colcnt].Locked = true;
            //                                        if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
            //                                        {
            //                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
            //                                            totalunfill = totalunfill + 1;
            //                                        }
            //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
            //                                        {
            //                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
            //                                            totalpartialfill = totalpartialfill + 1;
            //                                        }

            //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
            //                                        {
            //                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
            //                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Note = "filled";
            //                                            totalfill = totalfill + 1;

            //                                        }
            //                                        else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
            //                                        {
            //                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
            //                                            totalpartialfill = totalpartialfill + 1;
            //                                        }
            //                                        else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0)
            //                                        {
            //                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
            //                                            totalunfill = totalunfill + 1;
            //                                        }
            //                                        strRoomDetail = strRoomDetail + (dv[cnt]["Room_Name"]);

            //                                        if (IntRoomLen < strRoomDetail.Length)
            //                                        {
            //                                            IntRoomLen = strRoomDetail.Length;

            //                                        }

            //                                        Fpspread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "";
            //                                        IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;


            //                                    }
            //                                }

            //                                if (cbl_roomcheck.Items[0].Selected == true && cbl_roomcheck.Items[1].Selected == false && cbl_roomcheck.Items[2].Selected == false)
            //                                {
            //                                    Fpspread1.Sheets[0].Columns[colcnt].Locked = true;
            //                                    if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
            //                                    {
            //                                        IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
            //                                    }

            //                                    Fpspread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]);

            //                                    if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
            //                                    {
            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
            //                                        totalunfill = totalunfill + 1;
            //                                    }
            //                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
            //                                    {
            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
            //                                        totalpartialfill = totalpartialfill + 1;
            //                                    }

            //                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
            //                                    {

            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Note = "filled";
            //                                        totalfill = totalfill + 1;
            //                                    }
            //                                    else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
            //                                    {
            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
            //                                        totalpartialfill = totalpartialfill + 1;
            //                                    }
            //                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
            //                                    {
            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
            //                                        totalunfill = totalunfill + 1;
            //                                    }
            //                                    IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
            //                                }


            //                                else if (cbl_roomcheck.Items[0].Selected == true && cbl_roomcheck.Items[1].Selected == true && cbl_roomcheck.Items[2].Selected == false)
            //                                {
            //                                    Fpspread1.Sheets[0].Columns[colcnt].Locked = true;
            //                                    if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
            //                                    {
            //                                        IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
            //                                    }
            //                                    Fpspread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]);
            //                                    if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
            //                                    {
            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
            //                                        totalunfill = totalunfill + 1;
            //                                    }
            //                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
            //                                    {
            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
            //                                        totalpartialfill = totalpartialfill + 1;
            //                                    }

            //                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
            //                                    {

            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Note = "filled";
            //                                        totalfill = totalfill + 1;
            //                                    }
            //                                    else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
            //                                    {
            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
            //                                        totalpartialfill = totalpartialfill + 1;
            //                                    }
            //                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
            //                                    {
            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
            //                                        totalunfill = totalunfill + 1;
            //                                    }

            //                                    IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
            //                                }

            //                                else if (cbl_roomcheck.Items[1].Selected == true && cbl_roomcheck.Items[2].Selected == true && cbl_roomcheck.Items[0].Selected == false)
            //                                {
            //                                    Fpspread1.Sheets[0].Columns[colcnt].Locked = true;
            //                                    if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
            //                                    {
            //                                        IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
            //                                    }
            //                                    Fpspread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + (dv[cnt]["Room_Cost"]);
            //                                    if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
            //                                    {
            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
            //                                        totalunfill = totalunfill + 1;
            //                                    }
            //                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
            //                                    {

            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
            //                                        totalpartialfill = totalpartialfill + 1;
            //                                    }

            //                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
            //                                    {
            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Note = "filled";
            //                                        totalfill = totalfill + 1;
            //                                    }
            //                                    else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
            //                                    {
            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
            //                                        totalpartialfill = totalpartialfill + 1;
            //                                    }
            //                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
            //                                    {
            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
            //                                        totalunfill = totalunfill + 1;
            //                                    }

            //                                    IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
            //                                }

            //                                else if (cbl_roomcheck.Items[0].Selected == true && cbl_roomcheck.Items[2].Selected == true && cbl_roomcheck.Items[1].Selected == false)
            //                                {
            //                                    Fpspread1.Sheets[0].Columns[colcnt].Locked = true;
            //                                    if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
            //                                    {
            //                                        IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
            //                                    }
            //                                    Fpspread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Room_Cost"]);
            //                                    if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
            //                                    {
            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
            //                                        totalunfill = totalunfill + 1;
            //                                    }
            //                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
            //                                    {

            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
            //                                        totalpartialfill = totalpartialfill + 1;
            //                                    }

            //                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
            //                                    {

            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Note = "filled";
            //                                        totalfill = totalfill + 1;
            //                                    }
            //                                    else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
            //                                    {

            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
            //                                        totalpartialfill = totalpartialfill + 1;
            //                                    }
            //                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
            //                                    {

            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
            //                                        totalunfill = totalunfill + 1;
            //                                    }

            //                                    IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
            //                                }

            //                                else if (cbl_roomcheck.Items[1].Selected == true && cbl_roomcheck.Items[2].Selected == false && cbl_roomcheck.Items[0].Selected == false)
            //                                {
            //                                    Fpspread1.Sheets[0].Columns[colcnt].Locked = true;
            //                                    if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
            //                                    {
            //                                        IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
            //                                    }
            //                                    Fpspread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Avl_Student"]);
            //                                    if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
            //                                    {

            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
            //                                        totalunfill = totalunfill + 1;
            //                                    }
            //                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
            //                                    {

            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
            //                                        totalpartialfill = totalpartialfill + 1;
            //                                    }

            //                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
            //                                    {

            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Note = "filled";
            //                                        totalfill = totalfill + 1;
            //                                    }
            //                                    else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
            //                                    {

            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
            //                                        totalpartialfill = totalpartialfill + 1;
            //                                    }
            //                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
            //                                    {
            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
            //                                        totalunfill = totalunfill + 1;
            //                                    }

            //                                    IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
            //                                }
            //                                else if (cbl_roomcheck.Items[2].Selected == true && cbl_roomcheck.Items[1].Selected == false && cbl_roomcheck.Items[0].Selected == false)
            //                                {
            //                                    Fpspread1.Sheets[0].Columns[colcnt].Locked = true;
            //                                    if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
            //                                    {
            //                                        IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
            //                                    }
            //                                    Fpspread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Room_Cost"]);
            //                                    if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
            //                                    {
            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
            //                                        totalunfill = totalunfill + 1;
            //                                    }
            //                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
            //                                    {

            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
            //                                        totalpartialfill = totalpartialfill + 1;
            //                                    }

            //                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
            //                                    {

            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Note = "filled";
            //                                        totalfill = totalfill + 1;
            //                                    }
            //                                    else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
            //                                    {

            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
            //                                        totalpartialfill = totalpartialfill + 1;
            //                                    }
            //                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
            //                                    {

            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
            //                                        totalunfill = totalunfill + 1;
            //                                    }

            //                                    IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
            //                                }


            //                                else if (cbl_roomcheck.Items[0].Selected == true && cbl_roomcheck.Items[2].Selected == true && cbl_roomcheck.Items[1].Selected == true)
            //                                {
            //                                    cb_includeall.Checked = true;
            //                                    Fpspread1.Sheets[0].Columns[colcnt].Locked = true;
            //                                    if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
            //                                    {
            //                                        IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
            //                                    }
            //                                    Fpspread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + (dv[cnt]["Room_Cost"]);

            //                                    if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
            //                                    {

            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
            //                                        totalunfill = totalunfill + 1;
            //                                    }
            //                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
            //                                    {

            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
            //                                        totalpartialfill = totalpartialfill + 1;
            //                                    }

            //                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
            //                                    {

            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Note = "filled";
            //                                        totalfill = totalfill + 1;
            //                                    }
            //                                    else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
            //                                    {

            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
            //                                        totalpartialfill = totalpartialfill + 1;
            //                                    }
            //                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
            //                                    {

            //                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
            //                                        totalunfill = totalunfill + 1;
            //                                    }

            //                                    IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;

            //                                }
            //                                totalvaccant = totalvaccant + Convert.ToInt16(dv[cnt]["students_allowed"]) - Convert.ToInt16(dv[cnt]["avl_student"]);
            //                            }

            //                            catch (Exception ex)
            //                            {
            //                            }
            //                        }

            //                        for (j = 1; j < Fpspread1.Sheets[0].ColumnCount; j++)
            //                        {
            //                            lbl_totalvaccants.Text = " ";
            //                            lbl_totalroom.Text = " ";
            //                            int totalroom = totalunfill + totalfill + totalpartialfill;
            //                            lbl_totalroom.Text = "Total No.of Rooms :" + totalroom;
            //                            lbl_totalvaccants.Text = "Total No.of Vacant :" + totalvaccant;
            //                            fill.Text = ("Filled(" + totalfill + ")");
            //                            unfill.Text = ("UnFilled(" + totalunfill + ")");
            //                            partialfill.Text = ("Partialy Filled(" + totalpartialfill + ")");
            //                        }

            //                    }

            //                    int height = 60;
            //                    {
            //                        for (j = 1; j < Fpspread1.Sheets[0].RowCount; j++)
            //                        {
            //                            height = height + Fpspread1.Sheets[0].Rows[j].Height;
            //                        }
            //                        Fpspread1.Height = height;
            //                        Fpspread1.SaveChanges();
            //                        Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
            //                    }

            //                    int width = 0;
            //                    if (Fpspread1.Sheets[0].ColumnCount == 7)
            //                    {
            //                        Fpspread1.Sheets[0].Columns[0].Width = 400;
            //                        for (j = 1; j < Fpspread1.Sheets[0].ColumnCount; j++)
            //                        {
            //                            width = width + Fpspread1.Sheets[0].Columns[j].Width;

            //                        }
            //                        width = width + 400;

            //                    }
            //                    else if (Fpspread1.Sheets[0].ColumnCount == 5)
            //                    {
            //                        Fpspread1.Sheets[0].Columns[0].Width = 800;
            //                        for (j = 1; j < Fpspread1.Sheets[0].ColumnCount; j++)
            //                        {
            //                            width = width + Fpspread1.Sheets[0].Columns[j].Width;

            //                        }
            //                        width = width + 800;
            //                    }
            //                    else
            //                    {
            //                        width = 900;
            //                    }

            //                    Fpspread1.Width = width;
            //                    Fpspread1.SaveChanges();
            //                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].ColumnCount;
            //                    Fpspread1.Sheets[0].FrozenColumnCount = 1;
            //                }

            //            }


            //            Fpspread1.Visible = true;
            //            tblStatus.Visible = true;
            //            rptprint.Visible = true;
            //            divSpread.Visible = true;
            //            lbl_error.Visible = false;
            //            lbl_error.Text = "No Records Found";
            //        }
            //    }
            //    else
            //    {

            //        Fpspread1.Visible = false;
            //        tblStatus.Visible = false;
            //        rptprint.Visible = false;
            //        divSpread.Visible = false;
            //        lbl_error.Visible = true;
            //        lbl_error.Text = "No Records Found";
            //    }
            //}
            //else
            //{
            //    tblStatus.Visible = false;
            //    Fpspread1.Visible = false;
            //    rptprint.Visible = false;
            //    divSpread.Visible = false;
            //    lbl_error.Visible = true;
            //    lbl_error.Text = "No Records Found";
            //}
            #endregion
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
            lbl_error.Text = ex.ToString();
        }
    }
    public void ddlbindhostel()
    {
        try
        {
            ddl_hostel.Items.Clear();
            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = dt.BindHostelbaseonmessrights_inv(MessmasterFK);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_hostel.DataSource = ds;
                ddl_hostel.DataTextField = "HostelName";
                ddl_hostel.DataValueField = "HostelMasterPK";
                ddl_hostel.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void clgbuild()
    {
        try
        {
            build = "";
            string bul = "";

            ddl_building.Items.Clear();
            build = Convert.ToString(ddl_hostel.SelectedItem.Value);
            if (build != "")
            {
                bul = dt.GetBuildingCode_inv(build);
                ds = dt.BindBuilding(bul);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_building.DataSource = ds;
                    ddl_building.DataTextField = "Building_Name";
                    ddl_building.DataValueField = "code";
                    ddl_building.DataBind();
                }
            }

        }
        catch (Exception ex)
        {
        }
    }
    public void clgfloor()
    {
        try
        {
            floor = "";
            cbl_floor.Items.Clear();
            floor = Convert.ToString(ddl_building.SelectedItem.Text);
            txt_floor.Text = "---Select---";
            cb_floor.Checked = false;
            if (floor != "")
            {
                ds = dt.BindFloor(floor);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_floor.DataSource = ds;
                    cbl_floor.DataTextField = "Floor_Name";
                    cbl_floor.DataValueField = "Floor_Name";
                    cbl_floor.DataBind();
                    if (cbl_floor.Items.Count > 0)
                    {
                        for (i = 0; i < cbl_floor.Items.Count; i++)
                        {
                            cbl_floor.Items[i].Selected = true; ;
                        }
                        txt_floor.Text = "Floor (" + cbl_floor.Items.Count + ")";
                        cb_floor.Checked = true;
                    }
                }
            }

        }
        catch (Exception ex)
        {
        }
    }
    public void clgroom()
    {
        try
        {
            build = "";
            floor = "";
            cbl_room.Items.Clear();
            cb_room.Checked = false;
            txt_room.Text = "---Select---";
            build = Convert.ToString(ddl_building.SelectedItem.Text);
            for (i = 0; i < cbl_floor.Items.Count; i++)
            {
                if (cbl_floor.Items[i].Selected)
                {

                    if (floor == "")
                    {
                        floor = Convert.ToString(cbl_floor.Items[i].Text);
                    }
                    else
                    {
                        floor = floor + "'" + "," + "'" + Convert.ToString(cbl_floor.Items[i].Text);
                    }
                }
            }

            if (build != "" && floor != "")
            {
                ds = dt.BindRoomtype(floor, build);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_room.DataSource = ds;
                    cbl_room.DataTextField = "room_type";
                    cbl_room.DataValueField = "room_type";
                    cbl_room.DataBind();

                    for (i = 0; i < cbl_room.Items.Count; i++)
                    {
                        cbl_room.Items[i].Selected = true;
                    }
                    txt_room.Text = "Room (" + cbl_room.Items.Count + ")";
                    cb_room.Checked = true;
                }
            }

        }
        catch (Exception ex)
        {
        }
    }
    protected void ddl_hostel_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clgbuild();
            clgfloor();
            clgroom();
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_floor_CheckedChanged(object sender, EventArgs e)
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
            clgroom();

        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_floor_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
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
            clgroom();
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_room_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_room.Checked == true)
            {
                for (i = 0; i < cbl_room.Items.Count; i++)
                {

                    cbl_room.Items[i].Selected = true;
                    txt_room.Text = "Room(" + (cbl_room.Items.Count) + ")";
                }
            }
            else
            {
                for (i = 0; i < cbl_room.Items.Count; i++)
                {
                    cbl_room.Items[i].Selected = false;
                    txt_room.Text = "--Select--";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_room_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_room.Checked = false;
            for (int i = 0; i < cbl_room.Items.Count; i++)
            {
                if (cbl_room.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }

            }
            if (seatcount == cbl_room.Items.Count)
            {
                txt_room.Text = "Room(" + seatcount.ToString() + ")";
                cb_room.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_room.Text = "--Select--";
            }
            else
            {
                txt_room.Text = "Room(" + seatcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_includeall_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_includeall.Checked == true)
            {
                for (i = 0; i < cbl_roomcheck.Items.Count; i++)
                {
                    cbl_roomcheck.Items[i].Selected = true;
                }
            }
            else
            {
                for (i = 0; i < cbl_roomcheck.Items.Count; i++)
                {
                    cbl_roomcheck.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_roomcheck_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbl_roomcheck.Items[0].Selected == true && cbl_roomcheck.Items[1].Selected == true && cbl_roomcheck.Items[2].Selected == true)
            {
                cb_includeall.Checked = true;
            }
            else
            {
                if (cbl_roomcheck.Items[0].Selected == false)
                {
                    cb_includeall.Checked = false;
                }
                if (cbl_roomcheck.Items[1].Selected == false)
                {
                    cb_includeall.Checked = false;
                }
                if (cbl_roomcheck.Items[2].Selected == false)
                {
                    cb_includeall.Checked = false;
                }
            }
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
            string degreedetails = "Room Availability Report";
            string pagename = "RoomAvailability.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }
    }
    // theivamani 18.11.15
    protected void ddl_vaccant_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fpspread1.Visible = false;
        tblStatus.Visible = false;
        rptprint.Visible = false;
        //divSpread.Visible = false;


        lbl_totalroom.Visible = false;
        lbl_totalvaccants.Visible = false;
        fill.Visible = false;
        partialfill.Visible = false;
        unfill.Visible = false;
    }
}