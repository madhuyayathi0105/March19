using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Text;
using System.Globalization;
using System.Web.UI.HtmlControls;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using System.Configuration;

public partial class Hostel_Attendance_Manual : System.Web.UI.Page
{
    string collegecode1 = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string staff_code = string.Empty;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DAccess2 d2 = new DAccess2();
    DataSet dsroom = new DataSet();
    DataSet dsatt = new DataSet();
    DataSet mesatt = new DataSet();
    DataSet messelatt = new DataSet();
    Hashtable hsroom = new Hashtable();
    Hashtable hsroomname = new Hashtable();
    Hashtable hsmessapp = new Hashtable();
    Hashtable hsmessroll = new Hashtable();
    DataTable dttempNew;
    static int chosedmode = 0;
    static int personmode = 0;
    static string query = "";
    DataRow drrowNew;
    int righ = 0;
    int mesrow = 0;
    int speardrow = 0;
    static string roomuser = string.Empty;
    static string roomgroupuser = string.Empty;
    static string hosname = string.Empty;
    static string floorname = string.Empty;
    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
    Boolean Cellclick = false;
    Boolean Cellclick1 = false;
    static string messco = string.Empty;
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
        Fpspread2.Attributes.Add("onMouseDown", "return showContextMenu(event)");
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        roomuser = usercode;
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        roomgroupuser = group_user;
        staff_code = (string)Session["Staff_Code"];
        txt_attandance.Attributes.Add("readonly", "readonly");
        txt_attandance.Text = DateTime.Now.ToString("dd/MM/yyyy");
        //if (rdbhostel.Checked == true)
        //{
        //    lblhostel.Visible = true;
        //    ddl_Hostel.Visible = true;
        //    lblAttendance.Visible = true;
        //    txt_attandance.Visible = true;
        //    Lblsession.Visible = true;
        //    ddlsession.Visible = true;
        //    //Lblstatus.Visible = true;
        //    //ddl_status.Visible = true;
        //    lbl_floorname.Visible = true;
        //    txt_floorname.Visible = true;
        //    Lblroom.Visible = true;
        //    txtroom.Visible = true;
        //    txt_room.Visible = true;
        //    txtroom.Enabled = false;
        //    btn_go.Visible = true;
        //    pflrnm.Visible = true;
        //    panel_room.Visible = true;
        //    Cbsearchroom.Visible = true;
        //    txrollno.Visible = false;
        //    Chroll.Visible = false;
        //}
        //else if (rdbmess.Checked == true)
        //{
        //    lblhostel.Visible = true;
        //    ddl_Hostel.Visible = true;
        //    lblAttendance.Visible = true;
        //    txt_attandance.Visible = true;
        //    Lblsession.Visible = true;
        //    ddlsession.Visible = true;
        //    //Lblstatus.Visible = true;
        //    //ddl_status.Visible = true;
        //    lbl_floorname.Visible = false;
        //    txt_floorname.Visible = false;
        //    Lblroom.Visible = false;
        //    //ddl_room.Visible = true;
        //    //txtroom.Visible = true;
        //    btn_go.Visible = true;
        //    pflrnm.Visible = false;
        //    panel_room.Visible = false;
        //    Cbsearchroom.Visible = false;
        //    txrollno.Visible = true;
        //    Chroll.Visible = true;
        //    Lblroom.Visible = false;
        //}
        //else
        //{
        //    lblhostel.Visible = false;
        //    ddl_Hostel.Visible = false;
        //    lblAttendance.Visible = false;
        //    txt_attandance.Visible = false;
        //    Lblsession.Visible = false;
        //    ddlsession.Visible = false;
        //    //Lblstatus.Visible = false;
        //    //ddl_status.Visible = false;
        //    Lblroom.Visible = false;
        //    //ddl_room.Visible = false;
        //    txtroom.Visible = false;
        //    txt_room.Visible = false;
        //    btn_go.Visible = false;
        //    lbl_floorname.Visible = false;
        //    txt_floorname.Visible = false;
        //    pflrnm.Visible = false;
        //    panel_room.Visible = false;
        //    txtroom.Enabled = false;
        //    Cbsearchroom.Visible = false;
        //    txrollno.Visible = false;
        //    Chroll.Visible = false;
        //}
        if (!IsPostBack)
        {
             string rights=string.Empty;
             if (usercode.Trim() != "")
             {
                 rights = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Hostel Attendance' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
             }
             if (group_user.Trim() != "" && group_user.Trim() != "0")
             {
                 rights = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Hostel Attendance' and Group_code ='" + group_user + "' and college_code ='" + collegecode1 + "'");
             }
            int.TryParse(rights, out righ);
            if (righ == 1)
                rdbhostel.Enabled = true;
            else
                rdbhostel.Enabled = false;
            if (righ == 2)
                rdbmess.Enabled = true;
            else
                rdbmess.Enabled = false;
            if (righ == 3)
            {
                rdbmess.Enabled = true;
                rdbhostel.Enabled = true;
            }
            else
            {
                if (righ == 1)
                    rdbhostel.Enabled = true;
                else
                    rdbhostel.Enabled = false;
                if (righ == 2)
                    rdbmess.Enabled = true;
                else
                    rdbmess.Enabled = false;

            }
            Fpspread.Visible = false;
            Fpspread1.Visible = false;
            loadsession();
            loadhostel();
            bindhostel();
            bindbuilding();
            bindfloor();
            bindroom();
            load_ddlrollno();
            Btnsavemess.Visible = false;
        }
    }
    public void loadsession()
    {
        try
        {
            ds.Clear();
            string deptquery = "select  SessionMasterPK,SessionName  from HM_SessionMaster where MessMasterFK in ('" + ddl_Hostel.SelectedValue + "') order by SessionMasterPK ";


            ds = d2.select_method_wo_parameter(deptquery, "Text");
            //ds = d2.BindSession(itemheader);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsession.DataSource = ds;
                ddlsession.DataTextField = "SessionName";
                ddlsession.DataValueField = "SessionMasterPK";
                ddlsession.DataBind();
            }
            else
            {
                ddlsession.DataSource = ds;
                ddlsession.DataTextField = "";
                ddlsession.DataValueField = "";
                ddlsession.DataBind();
            }
        }


        catch
        {
        }
    }
    public void loadhostel()
    {

        try
        {

            ddl_Hostel.Items.Clear();
            ds.Clear();
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_Hostel.DataSource = ds;
                ddl_Hostel.DataTextField = "MessName";
                ddl_Hostel.DataValueField = "MessMasterPK";
                ddl_Hostel.DataBind();
            }

        }
        catch
        {
        }
    }
    public void bindhostel()
    {
        try
        {
            ds.Clear();
            //string itemname = "select HostelMasterPK,HostelName  from HM_HostelMaster ";// where CollegeCode in ('" + ddl_college.SelectedItem.Value + "') order by HostelMasterPK ";
            //ds = d2.select_method_wo_parameter(itemname, "Text");
            ddl_Hostel.Items.Clear();
            string MessmasterFK = string.Empty;
            if (usercode != "" && usercode != "0")
                MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Hostel Rights' and user_code='" + usercode + "'");
            if (group_user != "" && group_user!="0")
                MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Hostel Rights' and user_code='" + group_user + "'");
            string itemname = "select HostelMasterPK,HostelName from HM_HostelMaster where  HostelMasterPK in (" + MessmasterFK + ") order by hostelname ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "text");
            // ds = d2.BindHostel(ddl_college.SelectedItem.Value);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_Hostel.DataSource = ds;
                ddl_Hostel.DataTextField = "HostelName";
                ddl_Hostel.DataValueField = "HostelMasterPK";
                ddl_Hostel.DataBind();
            }

        }
        catch
        {

        }
    }
    protected void rdbhostel_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            Btnmess.Visible = false;
            ddl_Hostel.Items.Clear();
            ddlsession.Items.Clear();
            lblhostel.Text = "Hostel Name";
            Lblroom.Text = "Room";
            txtroom.Visible = true;
            txt_room.Visible = true;
            lblhostel.Visible = true;
            ddl_Hostel.Visible = true;
            lblAttendance.Visible = true;
            txt_attandance.Visible = true;
            Lblsession.Visible = true;
            ddlsession.Visible = true;
            //Lblstatus.Visible = true;
            //ddl_status.Visible = true;
            Lblroom.Visible = true;
            //ddl_room.Visible = false;
            btn_go.Visible = true;
            //ddl_room.Visible = false;
            ddlsession.Items.Add(new System.Web.UI.WebControls.ListItem("Morning", "0"));
            ddlsession.Items.Add(new System.Web.UI.WebControls.ListItem("Evening", "1"));
            bindhostel();
            Btnsavemess.Visible = false;
            lbl_absent.Visible = false;
            lbl_total.Visible = false;
            Fpspread.Visible = false;
            Chroll.Visible = false;
            lbl_present.Visible = false;
            lblhostel.Visible = true;
            ddl_Hostel.Visible = true;
            lblAttendance.Visible = true;
            txt_attandance.Visible = true;
            Lblsession.Visible = true;
            ddlsession.Visible = true;
            //Lblstatus.Visible = true;
            //ddl_status.Visible = true;
            lbl_floorname.Visible = true;
            Lblbuild.Visible = true;
            drbbuilding.Visible = true;
            txt_floorname.Visible = true;
            Lblroom.Visible = true;
            txtroom.Visible = true;
            txt_room.Visible = true;
            txtroom.Enabled = false;
            btn_go.Visible = true;
            pflrnm.Visible = true;
            panel_room.Visible = true;
            Cbsearchroom.Visible = true;
          // txrollno.Visible = false;
            Chroll.Visible = false;
            updatepanel_room.Visible = true;
            lblnum.Visible = false;
            ddlrollno.Visible = false;
            txtno.Visible = false;
           


        }
        catch
        {
        }
    }
    protected void rdbmess_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            Btnmess.Visible = true;
            lblhostel.Text = "Mess Name";
            // Lblroom.Text = "Roll No";
            Lblroom.Visible = false;
            txtroom.Visible = false;
            //ddl_room.Visible = true;
            //ddl_room.Enabled = false;
            txt_room.Visible = false;
            lblhostel.Visible = true;
            ddl_Hostel.Visible = true;
            lblAttendance.Visible = true;
            txt_attandance.Visible = true;
            Lblsession.Visible = true;
            ddlsession.Visible = true;
            //Lblstatus.Visible = true;
            //ddl_status.Visible = true;
            Lblroom.Visible = false;
            btn_go.Visible = false;
            //ddl_room.Visible = true;
           // txrollno.Visible = true;
            txtroom.Visible = false;
            loadhostel();
            loadsession();
            Btnsavemess.Visible = false;
            lbl_absent.Visible = false;
            lbl_total.Visible = false;
            Fpspread.Visible = false;
            Chroll.Visible = true;
            lbl_present.Visible = false;

            lblhostel.Visible = true;
            ddl_Hostel.Visible = true;
            lblAttendance.Visible = true;
            txt_attandance.Visible = true;
            Lblsession.Visible = true;
            ddlsession.Visible = true;
            //Lblstatus.Visible = true;
            //ddl_status.Visible = true;
            lbl_floorname.Visible = false;
            txt_floorname.Visible = false;
            Lblbuild.Visible = false;
            drbbuilding.Visible = false;
            Lblroom.Visible = false;
            //ddl_room.Visible = true;
            //txtroom.Visible = true;
            pflrnm.Visible = false;
            panel_room.Visible = false;
            Cbsearchroom.Visible = false;
           // txrollno.Visible = true;
            Chroll.Visible = true;
            Lblroom.Visible = false; updatepanel_room.Visible = false;
            lblnum.Visible = true;
            ddlrollno.Visible = true;
            txtno.Visible = true;
           


        }
        catch
        {
        }
    }
    protected void rdbstudy_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            ddl_Hostel.Items.Clear();
            ddlsession.Items.Clear();
            lblhostel.Text = "Hostel Name";
            Lblroom.Text = "Room";
            ddlsession.Items.Add(new System.Web.UI.WebControls.ListItem("Morning", "0"));
            ddlsession.Items.Add(new System.Web.UI.WebControls.ListItem("Evening", "1"));
            bindhostel();

        }
        catch
        {
        }
    }
    public void bindfloor()
    {
        try
        {
            //string hostel = "";

            //for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            //{
            //    if (cbl_hostelname.Items[i].Selected == true)
            //    {
            //        if (hostel == "")
            //        {
            //            hostel = "" + cbl_hostelname.Items[i].Value.ToString() + "";
            //        }
            //        else
            //        {
            //            hostel = hostel + "'" + "," + "'" + cbl_hostelname.Items[i].Value.ToString() + "";
            //        }
            //    }
            //}

            //string build = d2.GetBuildingCode_inv(hostel);
            //char[] delimiterChars = { ',' };
            //string[] build1 = build.Split(delimiterChars);
            //string build2 = "";

            //foreach (string b in build1)
            //{
            //    if (build2 == "")
            //    {
            //        build2 = "" + b + "";
            //    }
            //    else
            //    {
            //        build2 = build2 + "'" + "," + "'" + b + "";
            //    }
            //}

            //ds1.Clear();
            //string floor = "select code,Building_Name from Building_Master where code in ('" + build2 + "')";
            //ds1 = d2.select_method_wo_parameter(floor, "Text");
            //string w = "";
            //if (ds1.Tables[0].Rows.Count > 0)
            //{
            //    string q1 = Convert.ToString(ds1.Tables[0].Rows[0][1]);
            //    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            //    {
            //        string q = Convert.ToString(ds1.Tables[0].Rows[i][1]);
            //        if (w == "")
            //        {
            //            w = "" + q + "";
            //        }
            //        else
            //        {
            //            w = w + "'" + "," + "'" + q + "";
            //        }
            //    }
            //}
            ds.Clear();
            // ds = d2.BindFloor_new(w);
            string itemname = "select * from Floor_Master f,Building_Master b where b.Building_Name=f.Building_Name and b.code  ='" + Convert.ToString(drbbuilding.SelectedValue) + "'";

            ds = d2.select_method_wo_parameter(itemname, "Text");
            cbl_floorname.Items.Clear();

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_floorname.DataSource = ds;
                cbl_floorname.DataTextField = "Floor_Name";
                cbl_floorname.DataValueField = "FloorPK";
                cbl_floorname.DataBind();




                if (cbl_floorname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_floorname.Items.Count; i++)
                    {

                        cbl_floorname.Items[i].Selected = true;
                    }

                    txt_floorname.Text = "Floor Name(" + cbl_floorname.Items.Count + ")";
                }
            }
            else
            {
                cbl_floorname.Items.Insert(0, "Select");
                txt_floorname.Text = "--Select--";
            }
            bindroom();
        }
        catch (Exception ex)
        {
        }
    }
    public void bindroom()
    {
        try
        {
            cbl_room.Items.Clear();
            txt_room.Text = "---Select---";
            cb_room.Checked = false;
            string query = "";
            string floors = "";

            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    if (floors == "")
                    {
                        floors = "" + cbl_floorname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        floors = floors + "'" + "," + "'" + cbl_floorname.Items[i].Value.ToString() + "";
                    }
                }
            }
            floorname = floors;
            query = "select distinct rd.Roompk,rd.Room_Name from Room_Detail rd,Floor_Master hd where rd.Floor_Name=hd.Floor_Name and hd.FloorPK in('" + floors + "') and hd.Building_Name=rd.Building_Name order by Roompk";


            string MessmasterFK = string.Empty;
            if (usercode != "" && usercode != "0")
                MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Room Rights' and user_code='" + usercode + "'");
            if (group_user != "" && group_user != "0")
                MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Room Rights' and user_code='" + group_user + "'");
            string itemname = "select distinct rd.Roompk,rd.Room_Name from Room_Detail rd,Floor_Master hd where rd.Floor_Name=hd.Floor_Name and hd.FloorPK in('" + floors + "') and hd.Building_Name=rd.Building_Name and Roompk in(" + MessmasterFK + ") order by Roompk";
            //select distinct Room_Name,Roompk from Room_Detail where Roompk in(" + MessmasterFK + ")";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
           // ds.Clear();
           // ds = d2.select_method_wo_parameter(query, "Text");
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
        catch (Exception ex)
        {
        }
    }
    protected void ddl_Hostel_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbuilding();
        bindfloor();
        hosname = Convert.ToString(ddl_Hostel.SelectedValue);
        txt_floorname.Text = "Floor Name (" + cbl_floorname.Items.Count + ")";
        cb_floorname.Checked = true;
        cb_floorname_CheckedChange(sender, e);
        if (ddl_Hostel.Text == "select")
        {
            txt_floorname.Text = "--Select--";
        }
        if (rdbmess.Checked == true)
            messco = Convert.ToString(ddl_Hostel.SelectedValue);
        //fpspreadvisiblefalse();
        //rptprint.Visible = false;

    }
    protected void cbl_floorname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            cb_floorname.Checked = false;
            txt_floorname.Text = "--Select--";

            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_floorname.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_floorname.Items.Count)
                {
                    cb_floorname.Checked = true;
                }
                txt_floorname.Text = "Floor Name(" + commcount.ToString() + ")";
                bindroom();
            }
        }
        catch { }
    }
    protected void cb_floorname_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            int c = 0;
            if (cb_floorname.Checked == true)
            {
                c++;
                for (int i = 0; i < cbl_floorname.Items.Count; i++)
                {
                    cbl_floorname.Items[i].Selected = true;
                }
                txt_floorname.Text = "Floor Name(" + (cbl_floorname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_floorname.Items.Count; i++)
                {
                    cbl_floorname.Items[i].Selected = false;
                }
                txt_floorname.Text = "--Select--";
            }
            bindroom();
        }
        catch { }
    }
    protected void cb_room_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
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
    protected void Cbsearchroom_CheckedChanged(object sender, EventArgs e)
    {
        if (Cbsearchroom.Checked == true)
        {
            txtroom.Enabled = true;
            txt_room.Enabled = false;
            txtroom.Text = "";
            txt_room.Text = "";
        }
        else
        {
            txtroom.Enabled = false;
            txt_room.Enabled = true;
            txtroom.Text = "Room";
            txt_room.Text = "";
        }
    }
    protected void Chroll_CheckedChanged(object sender, EventArgs e)
    {
        if (Chroll.Checked == true)
        {
            txtno.Enabled = true;
        }
        else
        {
            txtno.Enabled = false;
            txtno.Text = "";

        }
    }

    protected void Go_Click(object sender, EventArgs e)
    {
        try
        {
            if (rdbhostel.Checked == true || rdbstudy.Checked == true)
            {
               // gvatte1.Visible = false;
                Fpspread1.Visible = true;
                Fpspread.Visible = false;
                alertpopwindow.Visible = false;
                Fpspread1.Sheets[0].AutoPostBack = true;
                Fpspread1.Sheets[0].RowHeader.Visible = false;
                Fpspread1.Sheets[0].ColumnHeader.Visible = false;
                MyStyle.Font.Size = FontUnit.Medium;
                MyStyle.Font.Name = "Book Antiqua";
                MyStyle.Font.Bold = true;
                MyStyle.HorizontalAlign = HorizontalAlign.Center;
                MyStyle.ForeColor = Color.White;
                MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                Fpspread1.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].ColumnCount = 8;
                Fpspread1.Sheets[0].RowCount = 0;
                int fprows = 0;
                int fpcolm = 0;
                int hasrow = 0;
                string floors = "";
               string hos=string.Empty;
               string buildname = string.Empty;
                if(Convert.ToString(ddl_Hostel.SelectedValue)!="")
                {
                    hos = "" + ddl_Hostel.SelectedValue + "";
                    buildname = Convert.ToString(drbbuilding.SelectedValue);
          
                }
                if (cbl_floorname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_floorname.Items.Count; i++)
                    {
                        if (cbl_floorname.Items[i].Selected == true)
                        {
                            if (floors == "")
                            {
                                floors = "" + cbl_floorname.Items[i].Value.ToString() + "";
                            }
                            else
                            {
                                floors = floors + "'" + "," + "'" + cbl_floorname.Items[i].Value.ToString() + "";
                            }
                        }
                    }
                }
                string rooms = "";
                if (cbl_room.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_room.Items.Count; i++)
                    {
                        if (cbl_room.Items[i].Selected == true)
                        {
                            if (rooms == "")
                            {
                                rooms = "" + cbl_room.Items[i].Value.ToString() + "";
                            }
                            else
                            {
                                rooms = rooms + "'" + "," + "'" + cbl_room.Items[i].Value.ToString() + "";
                            }
                        }
                    }
                }
                string room = "";
                if (Cbsearchroom.Checked == false)
                {
                    if (txt_attandance.Text != "" && ddlsession.SelectedValue != "" && floors != "" && txt_room.Text != "")
                    {
                        Fpspread1.BorderWidth = 1;
                        room = "select distinct rd.Roompk,rd.Room_Name from Room_Detail rd,Floor_Master hd,HM_HostelMaster hm,Building_Master bm where  rd.Floor_Name=hd.Floor_Name  and bm.Building_Name=hd.Building_Name and hm.HostelMasterPK='" + Convert.ToString(ddl_Hostel.SelectedValue) + "'    and hd.Floorpk in('" + floors + "') and bm.code in('" + buildname + "') and Roompk in ('" + rooms + "') and rd.Building_Name=hd.Building_Name";//and hm.HostelBuildingFK= bm.Code
                        Fpspread1.Width = 900;
                        Fpspread1.Columns[2].Visible = true;
                        Fpspread1.Columns[3].Visible = true;
                        Fpspread1.Columns[4].Visible = true;
                        Fpspread1.Columns[5].Visible = true;
                        Fpspread1.Columns[6].Visible = true;
                        Fpspread1.Columns[7].Visible = true;
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        Fpspread1.Visible = false;
                        lblalerterr.Text = "Please set all feild";
                    }
                }

                else
                {
                    if (txt_attandance.Text != "" && ddlsession.SelectedValue != "" && floors != "" && txtroom.Text != "")
                {
                    Fpspread1.BorderWidth = 1;
                    //room = "select distinct rd.Roompk,rd.Room_Name from Room_Detail rd,Floor_Master hd,HM_HostelMaster hm,Building_Master bm where  rd.Floor_Name=hd.Floor_Name  and bm.Building_Name=hd.Building_Name and hm.HostelMasterPK='" + Convert.ToString(ddl_Hostel.SelectedValue) + "' and hd.College_Code='" + collegecode1 + "' and Avl_Student<>'0'  and rd.Room_Name in ('" + txtroom.Text + "')";//and hm.HostelBuildingFK= bm.Code  and hd.Floor_Name in('" + floors + "')
                    room = "select distinct rd.Roompk,rd.Room_Name from Room_Detail rd,Floor_Master hd,HM_HostelMaster hm,Building_Master bm where  rd.Floor_Name=hd.Floor_Name  and bm.Building_Name=hd.Building_Name and rd.Building_Name=hd.Building_Name and hm.HostelMasterPK='" + Convert.ToString(ddl_Hostel.SelectedValue) + "' and bm.code in('" + buildname + "') and rd.Room_Name in ('" + txtroom.Text + "') and hd.Floorpk in('" + floors + "')";
                    Fpspread1.Columns[2].Visible = false;
                    Fpspread1.Columns[3].Visible = false;
                    Fpspread1.Columns[4].Visible = false;
                    Fpspread1.Columns[5].Visible = false;
                    Fpspread1.Columns[6].Visible = false;
                    Fpspread1.Columns[7].Visible = false;
                    // Fpspread1.Columns[8].Visible = false;
                    Fpspread1.Columns[0].Width = 150;
                    Fpspread1.Columns[1].Width = 150;

                }
                    else
                    {
                        alertpopwindow.Visible = true;
                        Fpspread1.Visible = false;
                        lblalerterr.Text = "Please set all feild";
                    }
                }
               
                dsroom = d2.select_method_wo_parameter(room, "text");

                int fpcol = 0;
                int fprow = 0;
                Fpspread1.Sheets[0].RowCount = 1;
                if (dsroom.Tables[0].Rows.Count > 0 && dsroom.Tables.Count > 0)
                {
                    for (int i = 0; i < dsroom.Tables[0].Rows.Count; i++)
                    {

                        string[] spiltfrom;
                        string Attendance = "";
                        string rollno = "";
                        string insertquery = "";
                        string columngetvalue = "";
                        string AttndDayvalue = "";
                        string mrnevng_att = "";
                        string AttnEven = "";
                        string attnmonth = "";
                        string attnyear = "";
                        string attnday = ""; string mornA = ""; string evenA = ""; string mrn_evng = "";
                        string date = txt_attandance.Text;
                        spiltfrom = date.Split('/');
                        AttndDayvalue = Convert.ToString(spiltfrom[0]);
                        AttndDayvalue = AttndDayvalue.TrimStart('0');
                        attnday = AttndDayvalue;
                        if (ddlsession.SelectedItem.Text == "Morning")
                        {
                            AttndDayvalue = "[D" + AttndDayvalue + "]";
                            mornA = "D" + attnday;
                            mrnevng_att = AttndDayvalue;
                            mrn_evng = mornA;
                        }
                        else
                        {
                            AttnEven = "[D" + attnday + "E]";
                            evenA = "D" + attnday + "E";
                            mrnevng_att = AttnEven;
                            mrn_evng = evenA;
                        }
                        attnmonth = spiltfrom[1];
                        attnmonth = attnmonth.TrimStart('0');
                        attnyear = spiltfrom[2];
                        if (fpcol != 8)
                            {
                                string q = "select r.Roll_No,r.APP_No,r.Reg_No,r.Stud_Name,r.Stud_Type,hs.HostelRegistrationPK,hs.HostelMasterFK,Dt.Dept_Name,C.Course_Name ,r.Current_Semester,r.Sections,(select b.Building_Name from Building_Master b where Code=hs.BuildingFK) as Building_Name,(select f.Floor_Name from Floor_Master f where f.FloorPK=hs.FloorFK) as Floor_Name,(select r.Room_Name from Room_Detail r where r.Roompk=hs.RoomFK) as Room_Name,h.HostelName as Hostel_Name,hs.id from HM_HostelMaster h,HT_HostelRegistration hs,Registration r,Degree d,Department dt,Course c  where h.HostelMasterPK =hs.HostelMasterFK and hs.APP_No  =r.App_No and d.Degree_Code =r.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and ISNULL(IsSuspend,'0')=0  and ISNULL(IsVacated,'0') =0 and ISNULL(IsDiscontinued,'0')=0 and h.HostelMasterPK in('" + Convert.ToString(ddl_Hostel.SelectedValue) + "')  and RoomFK in('" + Convert.ToString(dsroom.Tables[0].Rows[i]["Roompk"]) + "') order by r.roll_no asc";//order by r.batch_year desc, r.degree_code asc,r.roll_no asc,hs.roomfk asc

                                ds = d2.select_method_wo_parameter(q, "Text");

                                if (ds.Tables[0].Rows.Count == 0)
                                {
                                    Fpspread1.Sheets[0].Cells[fprow, fpcol].BackColor = Color.PaleGreen;
                                    Fpspread1.Sheets[0].Cells[fprow, fpcol].Locked = true;
                                }
                                else
                                {
                                    string att = "select " + mrnevng_att + ",App_No from HT_Attendance where  AttnMonth='" + attnmonth + "' and AttnYear='" + attnyear + "' and App_No in(select r.APP_No from HM_HostelMaster h,HT_HostelRegistration hs,Registration r,Degree d,Department dt,Course c  where h.HostelMasterPK =hs.HostelMasterFK and hs.APP_No  =r.App_No and d.Degree_Code =r.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and ISNULL(IsSuspend,'0')=0  and ISNULL(IsVacated,'0') =0 and ISNULL(IsDiscontinued,'0')=0 and h.HostelMasterPK in('" + Convert.ToString(ddl_Hostel.SelectedValue) + "')  and RoomFK in('" + Convert.ToString(dsroom.Tables[0].Rows[i]["Roompk"]) + "'))";

                                    dsatt = d2.select_method_wo_parameter(att, "text");


                                    if (dsatt.Tables[0].Rows.Count == 0 && dsatt.Tables.Count > 0)
                                    {




                                        Fpspread1.Sheets[0].Cells[fprow, fpcol].BackColor = Color.Violet;

                                        //Fpspread1.Sheets[0].Cells[fprow, fpcol].Text = Convert.ToString(dsroom.Tables[0].Rows[i]["Room_Name"]);
                                        //Fpspread1.Sheets[0].Cells[fprow, fpcol].Tag = Convert.ToString(dsroom.Tables[0].Rows[i]["Roompk"]);
                                        //Fpspread1.Sheets[0].Cells[fprow, fpcol].HorizontalAlign = HorizontalAlign.Center;

                                        //Fpspread1.Sheets[0].Cells[fprow, fpcol].Font.Name = "Book Antiqua";
                                        //Fpspread1.Sheets[0].Cells[fprow, fpcol].Font.Size = FontUnit.Medium;
                                        //Fpspread1.Sheets[0].Cells[fprow, fpcol].Border.BorderSize = 6;
                                        //Fpspread1.Sheets[0].SelectionBackColor = Color.Pink;
                                        //Fpspread1.Sheets[0].SelectionForeColor = Color.Red;
                                        //Fpspread1.Sheets[0].Cells[fprow, fpcol].Border.BorderColor = Color.White;
                                        //fpcol++;


                                    }

                                    else
                                    {
                                        Fpspread1.Sheets[0].Cells[fprow, fpcol].BackColor = Color.Gray;
                                       
                                    }
                                }
                                Fpspread1.Sheets[0].Cells[fprow, fpcol].Text = Convert.ToString(dsroom.Tables[0].Rows[i]["Room_Name"]);
                                Fpspread1.Sheets[0].Cells[fprow, fpcol].Tag = Convert.ToString(dsroom.Tables[0].Rows[i]["Roompk"]);
                                Fpspread1.Sheets[0].Cells[fprow, fpcol].HorizontalAlign = HorizontalAlign.Center;

                                Fpspread1.Sheets[0].Cells[fprow, fpcol].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[fprow, fpcol].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[fprow, fpcol].Border.BorderSize = 6;
                                Fpspread1.Sheets[0].SelectionBackColor = Color.DarkSalmon;
                                Fpspread1.Sheets[0].SelectionForeColor = Color.Red;
                                Fpspread1.Sheets[0].Cells[fprow, fpcol].Border.BorderColor = Color.White;
                                fpcol++;

                        }
                             else
                            {
                                fpcol = 0;
                                fprow++;
                                Fpspread1.Sheets[0].Rows.Count++;
                                i--;
                            }
                       
//                        else
//                        {

//                            int room_no = Convert.ToInt32(dsroom.Tables[0].Rows[i]["Roompk"]);
//                            hsroom.Add(hasrow, room_no);
//                            hsroomname.Add(hasrow, Convert.ToString(dsroom.Tables[0].Rows[i]["Room_Name"]));
//                            hasrow++;
//                        }
//                    }
//                    if (hsroom.Count > 0)
//                    {
//                        fprows = fprow;
//                        fpcolm = 0;
//                        Fpspread1.Sheets[0].RowCount = fprows + 3;
//                        Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 2, 0, 1, 7);
//                        for (int mrow = 0; mrow < hsroom.Count; mrow++)
//                        {
//                            if (fpcolm != 8)
//                            {
//                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount -
//1, fpcolm].Text = Convert.ToString(hsroomname[mrow]);
//                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, fpcolm].Tag = Convert.ToString(hsroom[mrow]);
//                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, fpcolm].BackColor = Color.Gray;
//                                fpcolm++;

//                            }
//                            else
//                            {
//                                fpcolm = 0;
//                                fprow++;
//                                Fpspread1.Sheets[0].Rows.Count++;
//                                mrow--;
//                            }

                        }
                      Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    //Fpspread1.Width = 900;
                    Fpspread1.Height = 900;
                    Fpspread1.SaveChanges();
                    Fpspread1.Visible = true;
                    }

                  else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Found";
                    Fpspread1.Visible = false;
                }

                }
                


            
            else if (rdbmess.Checked == true)
            {
                messattendance();
            }

        }
        catch
        {

        }
    }
    protected void messattendance()
    {
        try
        {
            Fpspread.Visible = true;
            Fpspread1.Visible = false;
            alertpopwindow.Visible = false;
            Btnsavemess.Visible = true;
            Fpspread.Sheets[0].AutoPostBack = true;
            Fpspread.Sheets[0].RowHeader.Visible = false;
            Fpspread.Sheets[0].ColumnHeader.Visible = false;
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.Font.Bold = true;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.White;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            Fpspread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            Fpspread.CommandBar.Visible = false;
            Fpspread.Sheets[0].ColumnCount = 8;
            Fpspread.Sheets[0].RowCount = 0;
            Fpspread.BorderWidth = 1;
            int fncol = 0;
            int fnrow = 0;
            int absent_count = 0;
            int Present_count = 0;
            string mess = "";
            string securityrights = string.Empty;
            if (hsmessroll.Count == 0)
            {
                if (Chroll.Checked == true)
                {
                    if (txt_attandance.Text != "" && ddlsession.SelectedValue != "" && txtno.Text != "")
                    {
                        if (Convert.ToString(ddlrollno.SelectedItem) == "Roll No")
                        {
                            mess = "select r.Roll_No,r.Reg_No,r.App_No,r.Stud_Name,r.Stud_Type,hs.HostelRegistrationPK,hs.HostelMasterFK,Dt.Dept_Name,C.Course_Name ,r.Current_Semester,r.Sections,(select b.Building_Name from Building_Master b where Code=hs.BuildingFK) as Building_Name,(select f.Floor_Name from Floor_Master f where f.FloorPK=hs.FloorFK) as Floor_Name,(select r.Room_Name from Room_Detail r where r.Roompk=hs.RoomFK) as Room_Name,h.HostelName as Hostel_Name,hs.id from HM_HostelMaster h,HT_HostelRegistration hs,Registration r,Degree d,Department dt,HM_MessMaster mm,Course c  where h.HostelMasterPK =hs.HostelMasterFK and hs.APP_No  =r.App_No and d.Degree_Code =r.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and ISNULL(IsSuspend,'0')=0  and ISNULL(IsVacated,'0') =0 and ISNULL(IsDiscontinued,'0')=0 and  r.Roll_No='" + txtno.Text + "' and mm.MessMasterPK=hs.Messcode and hs.HostelMasterFK=h.HostelMasterPK";//and  mm.MessMasterPK ='" + Convert.ToString(ddl_Hostel.SelectedValue) + "'
                            dsroom = d2.select_method_wo_parameter(mess, "text");
                        }
                        if (Convert.ToString(ddlrollno.SelectedItem) == "Reg No")
                        {
                            mess = "select r.Roll_No,r.Reg_No,r.App_No,r.Stud_Name,r.Stud_Type,hs.HostelRegistrationPK,hs.HostelMasterFK,Dt.Dept_Name,C.Course_Name ,r.Current_Semester,r.Sections,(select b.Building_Name from Building_Master b where Code=hs.BuildingFK) as Building_Name,(select f.Floor_Name from Floor_Master f where f.FloorPK=hs.FloorFK) as Floor_Name,(select r.Room_Name from Room_Detail r where r.Roompk=hs.RoomFK) as Room_Name,h.HostelName as Hostel_Name,hs.id from HM_HostelMaster h,HT_HostelRegistration hs,Registration r,Degree d,Department dt,HM_MessMaster mm,Course c  where h.HostelMasterPK =hs.HostelMasterFK and hs.APP_No  =r.App_No and d.Degree_Code =r.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and ISNULL(IsSuspend,'0')=0  and ISNULL(IsVacated,'0') =0 and ISNULL(IsDiscontinued,'0')=0  and r.Reg_No='" + txtno.Text + "' and mm.MessMasterPK=hs.Messcode and hs.HostelMasterFK=h.HostelMasterPK";//and  mm.MessMasterPK ='" + Convert.ToString(ddl_Hostel.SelectedValue) + "'
                            dsroom = d2.select_method_wo_parameter(mess, "text");
                        }
                        if (Convert.ToString(ddlrollno.SelectedItem) == "Name")
                        {
                            mess = "select r.Roll_No,r.Reg_No,r.App_No,r.Stud_Name,r.Stud_Type,hs.HostelRegistrationPK,hs.HostelMasterFK,Dt.Dept_Name,C.Course_Name ,r.Current_Semester,r.Sections,(select b.Building_Name from Building_Master b where Code=hs.BuildingFK) as Building_Name,(select f.Floor_Name from Floor_Master f where f.FloorPK=hs.FloorFK) as Floor_Name,(select r.Room_Name from Room_Detail r where r.Roompk=hs.RoomFK) as Room_Name,h.HostelName as Hostel_Name,hs.id from HM_HostelMaster h,HT_HostelRegistration hs,Registration r,Degree d,Department dt,HM_MessMaster mm,Course c   where h.HostelMasterPK =hs.HostelMasterFK and hs.APP_No  =r.App_No and d.Degree_Code =r.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and ISNULL(IsSuspend,'0')=0  and ISNULL(IsVacated,'0') =0 and ISNULL(IsDiscontinued,'0')=0  and r.Stud_Name='" + txtno.Text + "' and mm.MessMasterPK=hs.Messcode and hs.HostelMasterFK=h.HostelMasterPK";//and  mm.MessMasterPK ='" + Convert.ToString(ddl_Hostel.SelectedValue) + "' and h.MessMasterFK=mm.MessMasterPK 
                            dsroom = d2.select_method_wo_parameter(mess, "text");
                        }
                        if (Convert.ToString(ddlrollno.SelectedItem) == "Hostel Id")
                        {
                            mess = "select r.Roll_No,r.Reg_No,r.App_No,r.Stud_Name,r.Stud_Type,hs.HostelRegistrationPK,hs.HostelMasterFK,Dt.Dept_Name,C.Course_Name ,r.Current_Semester,r.Sections,(select b.Building_Name from Building_Master b where Code=hs.BuildingFK) as Building_Name,(select f.Floor_Name from Floor_Master f where f.FloorPK=hs.FloorFK) as Floor_Name,(select r.Room_Name from Room_Detail r where r.Roompk=hs.RoomFK) as Room_Name,h.HostelName as Hostel_Name,hs.id from HM_HostelMaster h,HT_HostelRegistration hs,Registration r,Degree d,Department dt,HM_MessMaster mm,Course c  where h.HostelMasterPK =hs.HostelMasterFK and hs.APP_No  =r.App_No and d.Degree_Code =r.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and ISNULL(IsSuspend,'0')=0  and ISNULL(IsVacated,'0') =0 and ISNULL(IsDiscontinued,'0')=0  and hs.id='" + txtno.Text + "' and mm.MessMasterPK=hs.Messcode and hs.HostelMasterFK=h.HostelMasterPK";//and  mm.MessMasterPK ='" + Convert.ToString(ddl_Hostel.SelectedValue) + "' and h.MessMasterFK=mm.MessMasterPK 
                            dsroom = d2.select_method_wo_parameter(mess, "text");
                        }

                        Fpspread.Columns[2].Visible = false;
                        Fpspread.Columns[3].Visible = false;
                        Fpspread.Columns[4].Visible = false;
                        Fpspread.Columns[5].Visible = false;
                        Fpspread.Columns[6].Visible = false;
                        Fpspread.Columns[7].Visible = false;
                        // Fpspread1.Columns[8].Visible = false;
                        Fpspread.Columns[0].Width = 90;
                        Fpspread.Columns[1].Width = 90;
                    }
                }
                else if (txt_attandance.Text != "" && ddlsession.SelectedValue != "")
                {
                    mess = "select r.Roll_No,r.Reg_No,r.App_No,r.Stud_Name,r.Stud_Type,hs.HostelRegistrationPK,hs.HostelMasterFK,Dt.Dept_Name,C.Course_Name ,r.Current_Semester,r.Sections,(select b.Building_Name from Building_Master b where Code=hs.BuildingFK) as Building_Name,(select f.Floor_Name from Floor_Master f where f.FloorPK=hs.FloorFK) as Floor_Name,(select r.Room_Name from Room_Detail r where r.Roompk=hs.RoomFK) as Room_Name,h.HostelName as Hostel_Name,hs.id from HM_HostelMaster h,HT_HostelRegistration hs,Registration r,Degree d,HM_MessMaster mm,Department dt,Course c  where h.HostelMasterPK =hs.HostelMasterFK and hs.APP_No  =r.App_No and d.Degree_Code =r.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and ISNULL(IsSuspend,'0')=0  and ISNULL(IsVacated,'0') =0 and ISNULL(IsDiscontinued,'0')=0  and mm.MessMasterPK=hs.Messcode and hs.HostelMasterFK=h.HostelMasterPK and hs.Messcode='" + Convert.ToString(ddl_Hostel.SelectedValue) + "'    order by r.roll_no asc";//and h.MessMasterFK=mm.MessMasterPK
                    dsroom = d2.select_method_wo_parameter(mess, "text");
                    Fpspread.Width = 900;
                    Fpspread.Columns[2].Visible = true;
                    Fpspread.Columns[3].Visible = true;
                    Fpspread.Columns[4].Visible = true;
                    Fpspread.Columns[6].Visible = true;
                    Fpspread.Columns[7].Visible = true;
                }
                else
                {
                    alertpopwindow.Visible = true;
                    Fpspread.Visible = false;
                    Btnsavemess.Visible = false;
                    lblalerterr.Text = "Please set all feild";
                }
               
                Fpspread.Sheets[0].RowCount = 1;
                int fpcol = 0;
                int fprow = 0;
                if (Convert.ToString(ddlrollno.SelectedItem) == "Roll No")
                {
                    securityrights = "Roll_No";
                }
                if (Convert.ToString(ddlrollno.SelectedItem) == "Reg No")
                {
                    securityrights = "Reg_No";
                }
                if (Convert.ToString(ddlrollno.SelectedItem) == "Name")
                {
                    securityrights = "Stud_Name";
                }
                if (Convert.ToString(ddlrollno.SelectedItem) == "Hostel Id")
                {
                    securityrights = "id";
                }
                if (dsroom.Tables.Count > 0 && dsroom.Tables[0].Rows.Count > 0)
                {
                    lbl_total.Text = "No of Students :" + dsroom.Tables[0].Rows.Count;
                    lbl_total.Visible = true;
                    for (int i = 0; i < dsroom.Tables[0].Rows.Count; i++)
                    {
                        string todate = Convert.ToString(txt_attandance.Text);
                        DateTime dt1 = new DateTime();
                        string[] split1 = todate.Split('/');
                        dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);


                        if (txt_attandance.Text != "" && ddlsession.SelectedValue != "")
                        {
                            string mess_attendan = "select * from HostelMess_Attendance where Entry_Date ='" + dt1.ToString("MM/dd/yyyy") + "'and Session_Code='" + Convert.ToString(ddlsession.SelectedValue) + "' and Session_name='" + Convert.ToString(ddlsession.SelectedItem.Text) + "' and Hostel_code='" + Convert.ToString(ddl_Hostel.SelectedValue) + "' and Roll_No='" + Convert.ToString(dsroom.Tables[0].Rows[i]["Roll_No"]) + "'";
                            mesatt = d2.select_method_wo_parameter(mess_attendan, "text");
                            if (mesatt.Tables[0].Rows.Count == 0)
                            {
                                if (!hsmessroll.ContainsKey(i))
                                {
                                    if (fpcol != 8)
                                    {
                                        absent_count++;
                                        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                                        Fpspread.Sheets[0].Cells[fprow, fpcol].CellType = txt;

                                        Fpspread.Sheets[0].Cells[fprow, fpcol].Text = Convert.ToString(dsroom.Tables[0].Rows[i][securityrights]);
                                        Fpspread.Sheets[0].Cells[fprow, fpcol].Tag = Convert.ToString(dsroom.Tables[0].Rows[i]["App_No"]);
                                        Fpspread.Sheets[0].Cells[fprow, fpcol].HorizontalAlign = HorizontalAlign.Center;
                                        //Fpspread.Sheets[0].Cells[fprow, fpcol].Tag = Convert.ToString(dsroom.Tables[0].Rows[i]["Roompk"]);
                                        Fpspread.Sheets[0].Cells[fprow, fpcol].BackColor = Color.Red;
                                        Fpspread.Sheets[0].Cells[fprow, fpcol].Font.Name = "Book Antiqua";
                                        Fpspread.Sheets[0].Cells[fprow, fpcol].Font.Size = FontUnit.Medium;
                                        Fpspread.Sheets[0].Cells[fprow, fpcol].Border.BorderSize = 6;
                                        Fpspread.Sheets[0].SelectionBackColor = Color.LimeGreen;
                                        Fpspread.Sheets[0].SelectionForeColor = Color.Red;
                                        Fpspread.Sheets[0].Cells[fprow, fpcol].Border.BorderColor = Color.White;
                                        fpcol++;
                                    }


                                    else
                                    {
                                        fpcol = 0;
                                        fprow++;
                                        Fpspread.Sheets[0].Rows.Count++;
                                        i--;
                                    }

                                }
                            }
                            else
                            {
                                if (fpcol != 8)
                                {
                                    Present_count++;
                                    Fpspread.Sheets[0].Cells[fprow, fpcol].Text = Convert.ToString(dsroom.Tables[0].Rows[i][securityrights]);
                                    Fpspread.Sheets[0].Cells[fprow, fpcol].Tag = Convert.ToString(dsroom.Tables[0].Rows[i]["App_No"]);
                                    Fpspread.Sheets[0].Cells[fprow, fpcol].HorizontalAlign = HorizontalAlign.Center;
                                    //Fpspread.Sheets[0].Cells[fprow, fpcol].Tag = Convert.ToString(dsroom.Tables[0].Rows[i]["Roompk"]);
                                    Fpspread.Sheets[0].Cells[fprow, fpcol].BackColor = Color.Green;
                                    Fpspread.Sheets[0].Cells[fprow, fpcol].Font.Name = "Book Antiqua";
                                    Fpspread.Sheets[0].Cells[fprow, fpcol].Font.Size = FontUnit.Medium;
                                    Fpspread.Sheets[0].Cells[fprow, fpcol].Border.BorderSize = 6;
                                    fpcol++;
                                }
                                else
                                {
                                    fpcol = 0;
                                    fprow++;
                                    Fpspread.Sheets[0].Rows.Count++;
                                    i--;
                                }
                            }


                        }


                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Please Enter Attendance Date";
                        }

                    }
                    lbl_absent.Text = "Absent Students :" + absent_count;
                    lbl_absent.Visible = true;
                    lbl_present.Text = "Present Students :" + Present_count;
                    lbl_present.Visible = true;



                }
                else
                {
                    alertpopwindow.Visible = true;
                    Fpspread.Visible = false;
                    lblalerterr.Text = "No Record Found";
                }




                //string todate1 = Convert.ToString(txt_attandance.Text);
                //DateTime dt2 = new DateTime();
                //string[] split11 = todate1.Split('/');
                //dt2 = Convert.ToDateTime(split11[1] + "/" + split11[0] + "/" + split11[2]);
                //string mess_attendan1 = "select Roll_No,App_No from HostelMess_Attendance where Entry_Date ='" + dt2.ToString("MM/dd/yyyy") + "'and Session_Code='" + Convert.ToString(ddlsession.SelectedValue) + "' and Session_name='" + Convert.ToString(ddlsession.SelectedItem.Text) + "'  and Hostel_code='" + Convert.ToString(ddl_Hostel.SelectedValue) + "' order by Roll_No ";
                //mesatt = d2.select_method_wo_parameter(mess_attendan1, "text");
                //if (mesatt.Tables[0].Rows.Count > 0)
                //{
                //    Fpspread.Visible = true;
                //    alertpopwindow.Visible = false;
                //    Fpspread.Sheets[0].AutoPostBack = true;
                //    Fpspread.Sheets[0].RowHeader.Visible = false;
                //    Fpspread.Sheets[0].ColumnHeader.Visible = false;
                //    MyStyle.Font.Size = FontUnit.Medium;
                //    MyStyle.Font.Name = "Book Antiqua";
                //    MyStyle.Font.Bold = true;
                //    MyStyle.HorizontalAlign = HorizontalAlign.Center;
                //    MyStyle.ForeColor = Color.White;
                //    MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                //    Fpspread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                //    Fpspread.CommandBar.Visible = false;
                //    Fpspread.Sheets[0].ColumnCount = 8;

                //    Fpspread.Sheets[0].RowCount = fnrow + 3;
                //    Fpspread.BorderWidth = 1;
                //    Fpspread.Sheets[0].SpanModel.Add(Fpspread.Sheets[0].RowCount - 2, 0, 1, 7);
                //    Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 2, 0].Text = "Present Student :" + Convert.ToString(mesatt.Tables[0].Rows.Count);
                //    Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 2, 0].ForeColor = Color.Red;
                //    //Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 2, 0].CellType txt
                //    int fpcol1 = 0;
                //    int fprow1 = 0;
                //    fpcol1 = 0;
                //    fprow1 = fnrow + 2;
                //    for (int mess_attn = 0; mess_attn < mesatt.Tables[0].Rows.Count; mess_attn++)
                //    {
                //        if (fpcol1 != 8)
                //        {
                //            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                //            Fpspread.Sheets[0].Cells[fprow1, fpcol1].CellType = txt;
                //            Fpspread.Sheets[0].Cells[fprow1, fpcol1].Text = Convert.ToString(mesatt.Tables[0].Rows[mess_attn]["Roll_No"]);
                //            Fpspread.Sheets[0].Cells[fprow1, fpcol1].Tag = Convert.ToString(mesatt.Tables[0].Rows[mess_attn]["App_No"]);
                //            Fpspread.Sheets[0].Cells[fprow1, fpcol1].HorizontalAlign = HorizontalAlign.Center;
                //            //Fpspread.Sheets[0].Cells[fprow, fpcol].Tag = Convert.ToString(dsroom.Tables[0].Rows[i]["Roompk"]);
                //            Fpspread.Sheets[0].Cells[fprow1, fpcol1].BackColor = Color.Gray;
                //            Fpspread.Sheets[0].Cells[fprow1, fpcol1].Font.Name = "Book Antiqua";
                //            Fpspread.Sheets[0].Cells[fprow1, fpcol1].Font.Size = FontUnit.Medium;
                //            Fpspread.Sheets[0].Cells[fprow1, fpcol1].Border.BorderSize = 6;
                //            Fpspread.Sheets[0].SelectionBackColor = Color.Pink;
                //            Fpspread.Sheets[0].SelectionForeColor = Color.Red;
                //            Fpspread.Sheets[0].Cells[fprow1, fpcol1].Border.BorderColor = Color.White;
                //            fpcol1++;
                //        }
                //        else
                //        {
                //            fpcol1 = 0;
                //            fprow1++;
                //            Fpspread.Sheets[0].Rows.Count++;
                //            mess_attn--;
                //        }
                //    }

                //}
            }
            //if (hsmessroll.Count > 0)
            //{
            //    int columns = 0;
            //    int colrows = 0;
            //    speardrow = Fpspread.Sheets[0].Rows.Count;
            //    Fpspread.Sheets[0].Rows.Count = speardrow + 3;
            //    colrows = speardrow + 2;
            //    for (int hs = 0; hs < hsmessroll.Count; hs++)
            //    {
            //        if (columns != 8)
            //        {
            //            Fpspread.Sheets[0].Cells[colrows, columns].Text = Convert.ToString(hsmessroll[hs]);
            //            Fpspread.Sheets[0].Cells[colrows, columns].Tag = Convert.ToString(hsmessapp[hs]);
            //            Fpspread.Sheets[0].Cells[colrows, columns].HorizontalAlign = HorizontalAlign.Center;
            //            Fpspread.Sheets[0].Cells[colrows, columns].BackColor = Color.Gray;
            //            Fpspread.Sheets[0].Cells[colrows, columns].Font.Name = "Book Antiqua";
            //            Fpspread.Sheets[0].Cells[colrows, columns].Font.Size = FontUnit.Medium;
            //            Fpspread.Sheets[0].Cells[colrows, columns].Border.BorderSize = 6;
            //            Fpspread.Sheets[0].SelectionBackColor = Color.Pink;
            //            Fpspread.Sheets[0].SelectionForeColor = Color.Red;
            //            Fpspread.Sheets[0].Cells[colrows, columns].Border.BorderColor = Color.White;
            //            columns++;
            //        }
            //        else
            //        {
            //            columns = 0;
            //            colrows++;
            //            Fpspread.Sheets[0].Rows.Count++;
            //            hs--;
            //        }
            //    }
            //}
            Fpspread.Sheets[0].PageSize = Fpspread.Sheets[0].RowCount;
           // Fpspread.Width = 900;
            Fpspread.Height = 900;
            Fpspread.SaveChanges();

        }
        catch
        {
        }
    }
    //protected void messattendance()
    //{
    //    try
    //    {
    //        gvatte.Visible = false;
    //        popwindow1.Visible = false;
    //        alertpopwindow.Visible = false;
    //        Fpspread2.Visible = false;
    //        gvatte1.Visible = true;
    //        DataTable gdvheaders = new DataTable();
    //        gdvheaders.Columns.Add("S.No");

    //        gdvheaders.Columns.Add("Roll_no1");
    //        gdvheaders.Columns.Add("Roll_no2");
    //        gdvheaders.Columns.Add("Roll_no3");
    //        gdvheaders.Columns.Add("Roll_no4");
    //        gdvheaders.Columns.Add("Roll_no5");
    //        gdvheaders.Columns.Add("Roll_no6");
    //        gdvheaders.Columns.Add("Roll_no7");
    //        gdvheaders.Columns.Add("Roll_no8");

    //        DataRow dr = null;
    //        dr = gdvheaders.NewRow();
    //        dr[0] = "S.No";
    //        dr[1] = "Roll No";
    //        dr[2] = "Student Name";
    //        dr[3] = "Room No";
    //        dr[4] = "Current Status";
    //        dr[5] = "Mark as";
    //        dr[6] = "Mark as";

    //        gdvheaders.Rows.Add(dr);
    //        string mess = "select r.Roll_No,r.Reg_No,r.App_No,r.Stud_Name,mm.MessName,r.Stud_Type,hs.HostelRegistrationPK,hs.HostelMasterFK,Dt.Dept_Name,C.Course_Name ,r.Current_Semester,r.Sections,(select b.Building_Name from Building_Master b where Code=hs.BuildingFK) as Building_Name,(select f.Floor_Name from Floor_Master f where f.FloorPK=hs.FloorFK) as Floor_Name,(select r.Room_Name from Room_Detail r where r.Roompk=hs.RoomFK) as Room_Name,h.HostelName as Hostel_Name from HM_HostelMaster h,HT_HostelRegistration hs,Registration r,Degree d,Department dt,Course c,HM_MessMaster mm  where h.HostelMasterPK =hs.HostelMasterFK and hs.APP_No  =r.App_No and d.Degree_Code =r.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and ISNULL(IsSuspend,'0')=0  and ISNULL(IsVacated,'0') =0 and ISNULL(IsDiscontinued,'0')=0 and h.MessMasterFK=mm.MessMasterPK and  mm.MessMasterPK ='" + Convert.ToString(ddl_Hostel.SelectedValue) + "'  order by r.roll_no asc";
    //        dsroom = d2.select_method_wo_parameter(mess, "text");
    //        int fpcol = 0;
    //        int fprow = 0;
    //        dr = dsroom.Tables[0].NewRow();
    //        if (dsroom.Tables[0].Rows.Count > 0 && dsroom.Tables.Count > 0)
    //        {
    //            lbl_total.Text = "No of Students :" + dsroom.Tables[0].Rows.Count;
    //            lbl_total.Visible = true;
    //            for (int i = 0; i < dsroom.Tables[0].Rows.Count; i++)
    //            {
    //                string todate = Convert.ToString(txt_attandance.Text);
    //                DateTime dt1 = new DateTime();
    //                string[] split1 = todate.Split('/');
    //                dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);


    //                if (txt_attandance.Text != "" && ddlsession.SelectedValue != "")
    //                {
    //                    string mess_attendan = "select * from HostelMess_Attendance where Entry_Date ='" + dt1.ToString("MM/dd/yyyy") + "'and Session_Code='" + Convert.ToString(ddlsession.SelectedValue) + "' and Session_name='" + Convert.ToString(ddlsession.SelectedItem.Text) + "' and Hostel_code='" + Convert.ToString(ddl_Hostel.SelectedValue) + "' and Roll_No='" + Convert.ToString(dsroom.Tables[0].Rows[i]["Roll_No"]) + "'";
    //                    mesatt = d2.select_method_wo_parameter(mess_attendan, "text");
    //                    if (mesatt.Tables[0].Rows.Count == 0)
    //                    {
    //                        if (!hsmessroll.ContainsKey(i))
    //                        {
    //                            if (fpcol != 8)
    //                            {

    //                               // foreach (GridViewRow row1 in gvatte1.Rows)
    //                               // {



    //                                HtmlInputButton btne = (HtmlInputButton)gvatte1.Rows[i].Cells[1].FindControl("btn1");
    //                                    btne.Value = Convert.ToString(dsroom.Tables[0].Rows[i]["Roll_No"]);


    //                                    fpcol++;
    //                                    dr = dsroom.Tables[0].NewRow();
    //                                //}
    //                            }


    //                            else
    //                            {
    //                                fpcol = 0;
    //                                fprow++;
    //                                Fpspread.Sheets[0].Rows.Count++;
    //                                i--;
    //                            }

    //                        }
    //                    }
    //                    else
    //                    {
    //                        if (fpcol != 8)
    //                        {
    //                            // Present_count++;
    //                            //Fpspread.Sheets[0].Cells[fprow, fpcol].Text = Convert.ToString(dsroom.Tables[0].Rows[i]["Roll_No"]);
    //                            //Fpspread.Sheets[0].Cells[fprow, fpcol].Tag = Convert.ToString(dsroom.Tables[0].Rows[i]["App_No"]);
    //                            //Fpspread.Sheets[0].Cells[fprow, fpcol].HorizontalAlign = HorizontalAlign.Center;
    //                            ////Fpspread.Sheets[0].Cells[fprow, fpcol].Tag = Convert.ToString(dsroom.Tables[0].Rows[i]["Roompk"]);
    //                            //Fpspread.Sheets[0].Cells[fprow, fpcol].BackColor = Color.Green;
    //                            //Fpspread.Sheets[0].Cells[fprow, fpcol].Font.Name = "Book Antiqua";
    //                            //Fpspread.Sheets[0].Cells[fprow, fpcol].Font.Size = FontUnit.Medium;
    //                            //Fpspread.Sheets[0].Cells[fprow, fpcol].Border.BorderSize = 6;
    //                            gvatte1.Rows[fprow].Cells[fpcol].Text = Convert.ToString(dsroom.Tables[0].Rows[i]["Roll_No"]);
    //                            fpcol++;
    //                        }
    //                        else
    //                        {
    //                            fpcol = 0;
    //                            fprow++;
    //                            Fpspread.Sheets[0].Rows.Count++;
    //                            i--;
    //                        }
    //                    }


    //                }


    //                else
    //                {
    //                    alertpopwindow.Visible = true;
    //                    lblalerterr.Text = "Please Enter Attendance Date";
    //                }

    //            }
    //            //lbl_absent.Text = "Absent Students :" + absent_count;
    //            //lbl_absent.Visible = true;
    //            //lbl_present.Text = "Present Students :" + Present_count;
    //            //lbl_present.Visible = true;



    //        }
    //        else
    //        {
    //            alertpopwindow.Visible = true;
    //            Fpspread.Visible = false;
    //            lblalerterr.Text = "No Record Found";
    //        }

    //    }
    //    catch
    //    {
    //    }
    //}


    protected void Fpspread_CellClick(object sender, EventArgs e)
    {
        try
        {
            if (rdbhostel.Checked == true || rdbstudy.Checked == true)
                Cellclick = true;
            else if (rdbmess.Checked == true)
                Cellclick1 = true;

        }
        catch (Exception ex)
        {
        }
    }

    protected void Fpspread_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
             if (Cellclick == true)
            {
                DataSet dategate = new DataSet();
                popwindow1.Visible = true;
                alertpopwindow.Visible = false;
                Fpspread2.Visible = false;
                gvatte.Visible = true;
                DataTable gdvheaders = new DataTable();
                gdvheaders.Columns.Add("S.No");
                gdvheaders.Columns.Add("id");
                gdvheaders.Columns.Add("Roll_no");
                gdvheaders.Columns.Add("stud_name");
                gdvheaders.Columns.Add("Room_Name");
                DataRow dr = null;
                dr = gdvheaders.NewRow();
                dr[0] = "S.No";
                dr[1] = "id";
                dr[2] = "Roll No";
                dr[3] = "Student Name";
                dr[4] = "Room No";
                gdvheaders.Rows.Add(dr);
                string activerow = "";
                string activecol = "";
               
                string fromdate = Convert.ToString(txt_attandance.Text);
                DateTime dt = new DateTime();
                string[] split = fromdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                
                string hoidaydate = "select CONVERT(varchar(10),  HolidayDate,103) as HolidayDate from HT_Holidays  where HolidayType =1  and HolidayDate = '" + dt.ToString("MM/dd/yyyy") + "' and HolidayForDayscholar='1' and HolidayForHostler ='1' and HolidayForStaff ='1'";
                ds1.Clear();
                ds1 = d2.select_method_wo_parameter(hoidaydate, "Text");
                if (ds1.Tables[0].Rows.Count == 0)
                {
                    if (Convert.ToString(ddl_Hostel.SelectedValue) != "")
                    {
                        activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                        activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                        int col = 0;
                        int.TryParse(activecol, out col);
                        if (activerow.Trim() != "" && activecol.Trim() != "")
                        {

                            string roomdet = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), col].Tag);
                            if (roomdet != "")
                            {
                                string current = DateTime.Now.ToString("dd/MM/yyyy");

                                string[] split2 = current.Split('/');
                                DateTime dt3 = Convert.ToDateTime(split2[1] + "/" + split2[0] + "/" + split2[2]);

                                if (dt <= dt3)
                                {
                                    string q = "select r.Roll_No,r.APP_No,r.Reg_No,r.Stud_Name,r.Stud_Type,hs.HostelRegistrationPK,hs.HostelMasterFK,Dt.Dept_Name,C.Course_Name ,r.Current_Semester,r.Sections,(select b.Building_Name from Building_Master b where Code=hs.BuildingFK) as Building_Name,(select f.Floor_Name from Floor_Master f where f.FloorPK=hs.FloorFK) as Floor_Name,(select r.Room_Name from Room_Detail r where r.Roompk=hs.RoomFK) as Room_Name,h.HostelName as Hostel_Name,hs.id from HM_HostelMaster h,HT_HostelRegistration hs,Registration r,Degree d,Department dt,Course c  where h.HostelMasterPK =hs.HostelMasterFK and hs.APP_No  =r.App_No and d.Degree_Code =r.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and ISNULL(IsSuspend,'0')=0  and ISNULL(IsVacated,'0') =0 and ISNULL(IsDiscontinued,'0')=0 and h.HostelMasterPK in('" + Convert.ToString(ddl_Hostel.SelectedValue) + "')  and RoomFK in('" + roomdet + "') order by r.roll_no asc";//order by r.batch_year desc, r.degree_code asc,r.roll_no asc,hs.roomfk asc

                                    ds = d2.select_method_wo_parameter(q, "Text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                        {
                                            gvatte.DataSource = ds.Tables[0];
                                            gvatte.DataBind();


                                        }


                                        foreach (GridViewRow row in gvatte.Rows)
                                        {
                                            string[] spiltfrom;
                                            string Attendance = "";
                                            string rollno = "";
                                            string insertquery = "";
                                            string columngetvalue = "";
                                            string AttndDayvalue = "";
                                            string mrnevng_att = "";
                                            string AttnEven = "";
                                            string attnmonth = "";
                                            string attnyear = "";
                                            string attnday = ""; string mornA = ""; string evenA = ""; string mrn_evng = "";
                                            Label stud_rollno = (Label)row.FindControl("lblroll_no");
                                          


                                            string date = txt_attandance.Text;
                                            spiltfrom = date.Split('/');
                                            AttndDayvalue = Convert.ToString(spiltfrom[0]);
                                            AttndDayvalue = AttndDayvalue.TrimStart('0');
                                            attnday = AttndDayvalue;
                                            string sess = string.Empty;
                                            if (ddlsession.SelectedItem.Text == "Morning")
                                            {
                                                AttndDayvalue = "[D" + AttndDayvalue + "]";
                                                mornA = "D" + attnday;
                                                mrnevng_att = AttndDayvalue;
                                                mrn_evng = mornA;
                                                sess = "Break FAST";
                                            }
                                            else
                                            {
                                                AttnEven = "[D" + attnday + "E]";
                                                evenA = "D" + attnday + "E";
                                                mrnevng_att = AttnEven;
                                                mrn_evng = evenA;
                                                sess = "Dinner";
                                            }

                                             Label stud_rollno1 = (Label)row.FindControl("lblAdmitNo1");
                                             Label stud_rollno12 = (Label)row.FindControl("lblAdmitNo12");
                                             Label stud_rollno13 = (Label)row.FindControl("lblAdmitNo13");

                                            string sel_att1 ="select session_name from HostelMess_Attendance where roll_no='" + Convert.ToString(stud_rollno.Text) + "' and CONVERT(varchar(20),entry_date,103)='" + fromdate + "' ";//and session_name='" + Convert.ToString(sess) + "'

                                             DataSet messds = d2.select_method_wo_parameter(sel_att1, "Text");
                                             if (messds.Tables[0].Rows.Count > 0)
                                             {
                                                 for (int i = 0; i < messds.Tables[0].Rows.Count; i++)
                                                 {
                                                     string sesss = Convert.ToString(messds.Tables[0].Rows[i][0]);
                                                     if (sesss.ToUpper() == "BREAK FAST")
                                                     {
                                                          stud_rollno1.ForeColor = Color.Orchid;
                                                         stud_rollno1.Text = "Present";
                                                     }
                                                     if (sesss.ToUpper() == "LUNCH")
                                                     {
                                                         stud_rollno12.ForeColor = Color.Orchid;
                                                         stud_rollno12.Text = "Present";
                                                     }
                                                     if (sesss.ToUpper() == "DINNER")
                                                     {
                                                         stud_rollno13.ForeColor = Color.Orchid;
                                                         stud_rollno13.Text = "Present";
                                                     }
                                                 }
                                             }
                                             if (stud_rollno1.Text == "")
                                             {
                                                 stud_rollno1.ForeColor = Color.Aqua;
                                                 stud_rollno1.Text = "Absent";

                                             }
                                             if (stud_rollno13.Text == "")
                                             {

                                                 stud_rollno13.ForeColor = Color.Aqua;
                                                 stud_rollno13.Text = "Absent";
                                             }
                                             if (stud_rollno12.Text == "")
                                             {
                                                 stud_rollno12.ForeColor = Color.Aqua;
                                                 stud_rollno12.Text = "Absent";
                                             }
                                            
                                              
                                            attnmonth = spiltfrom[1];
                                            attnmonth = attnmonth.TrimStart('0');
                                            attnyear = spiltfrom[2];
                                            string gate1 = string.Empty;
                                            string gate = string.Empty;
                                            string exitdate = string.Empty;
                                            gate1 = "  select top 1* from GateEntryExit where App_No in(select App_No from Registration where Roll_No='" + stud_rollno.Text + "') order by GatepassExitdate desc";
                                            dategate = d2.select_method_wo_parameter(gate1, "text");
                                            if (dategate.Tables[0].Rows.Count > 0)
                                                exitdate = Convert.ToString(dategate.Tables[0].Rows[0]["GatepassEntrydate"]);
                                            else
                                                exitdate = "gatepass";
                                            if (exitdate != "")
                                            {

                                                string att = "select " + mrnevng_att + ",App_No from HT_Attendance where  AttnMonth='" + attnmonth + "' and AttnYear='" + attnyear + "' and App_No in(select App_No from Registration where Roll_No='" + stud_rollno.Text + "')";

                                                //  string att = "select " + AttndDayvalue + "," + AttnEven + " ,App_No from HT_Attendance where  AttnMonth='" + attnmonth + "' and AttnYear='" + attnyear + "' and App_No in(select App_No from Registration where Roll_No in (select r.Roll_No from HM_HostelMaster h,HT_HostelRegistration hs,Registration r,Degree d,Department dt,Course c  where h.HostelMasterPK =hs.HostelMasterFK and hs.APP_No  =r.App_No and d.Degree_Code =r.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and ISNULL(IsSuspend,'0')=0  and ISNULL(IsVacated,'0') =0 and ISNULL(IsDiscontinued,'0')=0 and h.HostelMasterPK in('" + Convert.ToString(ddl_Hostel.SelectedValue) + "')  and RoomFK in('" + roomdet + "')))";
                                                dsatt = d2.select_method_wo_parameter(att, "text");


                                                string evenvalue = ""; string dayvalue = "";
                                                if (dsatt.Tables[0].Rows.Count > 0)
                                                {
                                                    for (int m = 0; m < dsatt.Tables[0].Rows.Count; m++)
                                                    {
                                                        //string roll=d2.GetFunction("select Roll_No from Registration where App_No='"+Convert.ToString(dsatt.Tables[0].Rows[m]["App_No"])+"'");
                                                        dayvalue = Convert.ToString(dsatt.Tables[0].Rows[0][mrn_evng]);
                                                        if (dayvalue.Trim() == "1")
                                                        {
                                                            HtmlInputButton btn = (HtmlInputButton)row.Cells[5].FindControl("btn1");
                                                            //HtmlInputButton btn1 = (HtmlInputButton)row.Cells[6].FindControl("btn");
                                                            Label status = (Label)row.FindControl("lblAdmitNo");
                                                            btn.Value = "Absent";
                                                            status.Text = "Present";
                                                            //btn1.Value = "OD";
                                                            row.Cells[6].BackColor = Color.Red;
                                                            row.Cells[5].BackColor = Color.Green;
                                                            // row.Cells[6].BackColor = Color.Aqua;
                                                            btn.Style.Add(" background-color", "Red");
                                                            //btn1.Style.Add(" background-color", "Aqua");

                                                        }
                                                        if (dayvalue.Trim() == "2")
                                                        {
                                                            HtmlInputButton btn = (HtmlInputButton)row.Cells[5].FindControl("btn1");
                                                            //HtmlInputButton btn1 = (HtmlInputButton)row.Cells[6].FindControl("btn");
                                                            Label status = (Label)row.FindControl("lblAdmitNo");
                                                            btn.Value = "Present";
                                                            status.Text = "Absent";
                                                            //btn1.Value = "OD";
                                                            row.Cells[5].BackColor = Color.Red;
                                                            row.Cells[6].BackColor = Color.Green;
                                                            // row.Cells[6].BackColor = Color.Aqua;
                                                            btn.Style.Add(" background-color", "Green");
                                                            //btn1.Style.Add(" background-color", "Aqua");

                                                        }
                                                        if (dayvalue.Trim() == "3")
                                                        {
                                                            HtmlInputButton btn = (HtmlInputButton)row.Cells[5].FindControl("btn1");
                                                            //HtmlInputButton btn1 = (HtmlInputButton)row.Cells[6].FindControl("btn");
                                                            Label status = (Label)row.FindControl("lblAdmitNo");
                                                            btn.Value = "Present";
                                                            status.Text = "OD";
                                                            //btn1.Value = "Absent";
                                                            //row.Cells[6].BackColor = Color.Red;
                                                            row.Cells[6].BackColor = Color.Green;
                                                            row.Cells[5].BackColor = Color.Aqua;
                                                            btn.Style.Add(" background-color", "Green");
                                                            //btn1.Style.Add(" background-color", "Red");

                                                        }
                                                        if (dayvalue.Trim() == "")
                                                        {
                                                            string val = "";
                                                            string lbl = "";

                                                            HtmlInputButton btn = (HtmlInputButton)row.Cells[5].FindControl("btn1");
                                                            //HtmlInputButton btn1 = (HtmlInputButton)row.Cells[5].FindControl("btn");
                                                            Label status = (Label)row.FindControl("lblAdmitNo");

                                                            btn.Value = "Present";
                                                            status.Text = "Absent";
                                                            //btn1.Value = "OD";
                                                            row.Cells[6].BackColor = Color.Green;
                                                            row.Cells[5].BackColor = Color.Red;
                                                            //row.Cells[6].BackColor = Color.Aqua;
                                                            btn.Style.Add(" background-color", "Green");
                                                            //btn1.Style.Add(" background-color", "Aqua");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    string val = "";
                                                    string lbl = "";

                                                    HtmlInputButton btn = (HtmlInputButton)row.Cells[5].FindControl("btn1");
                                                    //HtmlInputButton btn1 = (HtmlInputButton)row.Cells[5].FindControl("btn");
                                                    Label status = (Label)row.FindControl("lblAdmitNo");

                                                    btn.Value = "Present";
                                                    status.Text = "Absent";
                                                    //btn1.Value = "OD";
                                                    row.Cells[6].BackColor = Color.Green;
                                                    row.Cells[5].BackColor = Color.Red;
                                                    //  row.Cells[6].BackColor = Color.Aqua;
                                                    btn.Style.Add(" background-color", "Green");
                                                    //btn1.Style.Add(" background-color", "Aqua");
                                                }
                                            }
                                            else
                                            {
                                                Label status = (Label)row.FindControl("lblAdmitNo");
                                                status.Text = "OD";
                                                row.Cells[5].BackColor = Color.Aqua;
                                                HtmlInputButton btn = (HtmlInputButton)row.Cells[5].FindControl
    ("btn1");
                                                btn.Value = "Present";
                                                btn.Style.Add(" background-color", "Green");
                                            }


                                        }
                                        dr = ds.Tables[0].NewRow();
                                    }
                                    else
                                    {
                                        alertpopwindow.Visible = true;
                                        lblalerterr.Text = "No Record Found";
                                        gvatte.Visible = false;
                                        popwindow1.Visible = false;

                                    }


                                }
                            }
                            else
                                popwindow1.Visible = false;


                        }
                    }
                }

            }
            else if (Cellclick1 == true)
            {

                mess_attendance(sender, e);
            }
        }
        catch
        {
        }
    }




    protected void mess_attendance(object sender, EventArgs e)
    {
        try
        {

            string activerow = "";
            string activecol = "";
            activerow = Fpspread.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpspread.ActiveSheetView.ActiveColumn.ToString();
            string rollno = Fpspread.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text;
            string app_no =Convert.ToString(Fpspread.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Tag);

            //rollno = d2.GetFunction("select Roll_no from Registration where App_No='" + app_no + "'");

            if (Fpspread.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].BackColor == Color.Green)
                Fpspread.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].BackColor = Color.Red;
            else
                Fpspread.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].BackColor = Color.Green;
               
            if (rollno != "" && rollno != "0")
            {
                if (Lblroll.Text == "")
                    Lblroll.Text = rollno;
                else
                    Lblroll.Text = Lblroll.Text + ',' + rollno;
            }
            if (app_no != "" && app_no != "0")
            {
                if (Lblapp.Text == "")
                    Lblapp.Text = app_no;
                else
                    Lblapp.Text = Lblapp.Text + ',' + app_no;
            }

        }
        catch
        {
        }
    }
   



    public void btnType_Click(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            //            Fpspread2.SaveChanges();
            //            string activerow = "";
            //            string activecol = "";
            //            string dayvalue = "";
            //            activerow = Fpspread2.ActiveSheetView.ActiveRow.ToString();
            //            activecol = Fpspread2.ActiveSheetView.ActiveColumn.ToString();
            //            FarPoint.Web.Spread.ButtonCellType btnType = new FarPoint.Web.Spread.ButtonCellType();
            //            FarPoint.Web.Spread.ButtonCellType btnType1 = new FarPoint.Web.Spread.ButtonCellType();
            //            FarPoint.Web.Spread.ButtonCellType btnType2 = new FarPoint.Web.Spread.ButtonCellType();

            //            dayvalue = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Note);
            //            if (dayvalue.Trim() == "1")
            //            {
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text = "Present";
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Note = "1";
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 4].ForeColor = Color.Green;
            //                btnType2.Text = "Absent";
            //                //Fpspread2.Rows[0].Height = 50;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].BackColor = Color.White;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].ForeColor = Color.White;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].CellType = btnType2;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Tag = btnType2.Text;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Note = "2";
            //                //Fpspread2.Sheets[0].SelectionBackColor = Color.Pink;
            //                //Fpspread2.Sheets[0].SelectionForeColor = Color.Red;
            //                btnType2.ForeColor = Color.White;
            //                btnType2.BackColor = Color.Red;

            //                btnType.Text = "OD";
            //                //Fpspread2.Rows[0].Height = 50;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 6].BackColor = Color.White;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 6].ForeColor = Color.White;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 6].CellType = btnType;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Tag = btnType.Text;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Note = "3";
            //                //Fpspread2.Sheets[0].SelectionBackColor = Color.Pink;
            //                //Fpspread2.Sheets[0].SelectionForeColor = Color.Red;
            //                btnType.ForeColor = Color.White;
            //                btnType.BackColor = Color.Blue;
            //            }
            //            else if (dayvalue.Trim() == "2")
            //            {
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text = "Absent";
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Note = "2";
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 4].ForeColor = Color.Red;
            //                btnType1.Text = "Present";
            //                //Fpspread2.Rows[0].Height = 50;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].BackColor = Color.White;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].ForeColor = Color.White;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].CellType = btnType1;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Tag =
            //btnType1.Text;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Note = "1";
            //                //Fpspread2.Sheets[0].SelectionBackColor = Color.Pink;
            //                //Fpspread2.Sheets[0].SelectionForeColor = Color.Green;
            //                btnType1.ForeColor = Color.White;
            //                btnType1.BackColor = Color.PaleGreen;
            //                btnType.Text = "OD";
            //                //Fpspread2.Rows[0].Height = 50;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 6].BackColor = Color.White;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 6].ForeColor = Color.White;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 6].CellType = btnType;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Tag = btnType.Text;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Note = "3";
            //                //Fpspread2.Sheets[0].SelectionBackColor = Color.Pink;
            //                //Fpspread2.Sheets[0].SelectionForeColor = Color.Red;
            //                btnType.ForeColor = Color.White;
            //                btnType.BackColor = Color.Blue;
            //            }
            //            else if (dayvalue.Trim() == "3")
            //            {
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text =

            //"OD";
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Note = "3";
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 4].ForeColor = Color.Blue;
            //                btnType1.Text = "Present";
            //                //Fpspread2.Rows[0].Height = 50;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].BackColor = Color.White;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].ForeColor = Color.White;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].CellType = btnType1;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Tag =
            //btnType1.Text;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Note = "1";
            //                //Fpspread2.Sheets[0].SelectionBackColor = Color.Pink;
            //                //Fpspread2.Sheets[0].SelectionForeColor = Color.Green;
            //                btnType1.ForeColor = Color.White;
            //                btnType1.BackColor = Color.PaleGreen;
            //                btnType2.Text = "Absent";
            //                //Fpspread2.Rows[0].Height = 50;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 6].BackColor = Color.White;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 6].ForeColor = Color.White;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 6].CellType = btnType2;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Tag =
            //btnType2.Text;
            //                Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Note = "2";
            //                //Fpspread2.Sheets[0].SelectionBackColor = Color.Pink;
            //                //Fpspread2.Sheets[0].SelectionForeColor = Color.Red;
            //                btnType2.ForeColor = Color.White;
            //                btnType2.BackColor = Color.Red;
            //            }

        }
        catch
        {

        }
    }
      protected void Btnsave_Click(object sender, EventArgs e)
    {
        try
        {

            string rollno = "";
            string app_no = "";
            string dayvalue = "";
            string Attendance = "";
            string attnmonth = "";
            string attnyear = "";
            string AttndDayvalue = "";
            string mrnevng_att = "";
            string AttnEven = "";
            string mornA = ""; string evenA = ""; string mrn_evng = "";
            string attnday = "";
            string[] spiltfrom;
            string getval= hid.Value;
            string[] ar = getval.Split(',');
            int ro = 0;
            int retu = 0;
            foreach (GridViewRow row1 in gvatte.Rows)
            {
                Label lbl3 = (Label)row1.Cells[4].FindControl("lblAdmitNo");
                HtmlInputButton btn = (HtmlInputButton)row1.Cells[5].FindControl("btn1");
                Label lbl = (Label)row1.Cells[1].FindControl("lblroll_no");
                app_no = d2.GetFunction("select app_no from registration where roll_no='" + lbl.Text + "'");
                string status = ar[ro].ToString();
                if (status == "Absent")
                {
                    lbl3.Text = "Absent";
                    btn.Value = "Present";
                    dayvalue = "2";
                    lbl3.BackColor = Color.Red;
                    lbl3.BorderColor = Color.Red;
                    btn.Style.Add(" background-color", "Green");
                }
                else if (status == "Present")
                {
                    lbl3.Text = "Present";
                    btn.Value = "Absent";
                    dayvalue = "1";
                    lbl3.BackColor = Color.Green;
                    lbl3.BorderColor = Color.Green;

                    btn.Style.Add(" background-color", "Red");
                }
                else if (status == "OD")
                {
                    lbl3.Text = "OD";
                    btn.Value = "Present";
                   
                    dayvalue = "3";
                    lbl3.BackColor = Color.Aqua;
                    lbl3.BorderColor = Color.Aqua;

                    btn.Style.Add(" background-color", "Green");
                }
                Attendance = dayvalue;
                string date = txt_attandance.Text;
                spiltfrom = date.Split('/');
                AttndDayvalue = Convert.ToString(spiltfrom[0]);
                AttndDayvalue = AttndDayvalue.TrimStart('0');
                attnday = AttndDayvalue;
                if (ddlsession.SelectedItem.Text == "Morning")
                {
                    AttndDayvalue = "[D" + AttndDayvalue + "]";
                    mornA = "D" + attnday;
                    mrnevng_att = AttndDayvalue;
                    mrn_evng = mornA;
                }
                else
                {
                    AttnEven = "[D" + attnday + "E]";
                    evenA = "D" + attnday + "E";
                    mrnevng_att = AttnEven;
                    mrn_evng = evenA;
                }
                attnmonth = spiltfrom[1];
                attnmonth = attnmonth.TrimStart('0');
                attnyear = spiltfrom[2];
                if (txt_attandance.Text != "")
                {
                    string insertquery = "if exists (select * from HT_Attendance where App_No ='" + app_no.Trim() + "' and AttnMonth='" + attnmonth.Trim() + "' and AttnYear='" + attnyear.Trim() + "') update HT_Attendance set " + mrnevng_att.Trim() + "=" + Attendance.Trim() + " where App_No ='" + app_no.Trim() + "' and AttnMonth='" + attnmonth.Trim() + "' and AttnYear='" + attnyear.Trim() + "' else insert into HT_Attendance(App_No,AttnMonth,AttnYear," + mrnevng_att.Trim() + ") values ('" + app_no.Trim() + "','" + attnmonth.Trim() + "','" + attnyear.Trim() + "','" + Attendance.Trim() + "')";

                    retu = d2.update_method_wo_parameter(insertquery, "Text");
                    Cellclick = true;
                    ro++;
                   
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please set all feild";
                }


            }
           Cellclick = false;
            if (retu == 1)
            {
                Div1.Visible = true;
                Lblerror.Text = "Saved Successfully";
            }
            
            // Go_Click(sender, e);
            //if (Lblroll.Text == "")
            //    Lblroll.Text = lbl.Text;
            //else
            //    Lblroll.Text = Lblroll.Text + ',' + lbl.Text;
            //if (Lblroll.Text == "")
            //    Lblroll.Text = lbl.Text;
            //else
            //    Lblroll.Text = Lblroll.Text + ',' + lbl.Text;



        }
        catch
        {
        }



    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    protected void btnerrclose_Click1(object sender, EventArgs e)
    {
        Div1.Visible = false;
    }
    protected void Btnclose_Click(object sender, EventArgs e)
    {
       // Go_Click(sender, e);
        popwindow1.Visible = false;
    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
    }
    protected void Btnsavemess_Click(object sender, EventArgs e)
    {
        try
        {
            // Fpspread1.Visible = true;
            alertpopwindow.Visible = false;
            Fpspread.Sheets[0].AutoPostBack = true;
            Fpspread.Sheets[0].RowHeader.Visible = false;
            Fpspread.Sheets[0].ColumnHeader.Visible = false;
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.Font.Bold = true;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.White;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            Fpspread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            Fpspread.CommandBar.Visible = false;
            Fpspread.Sheets[0].ColumnCount = 8;
            Fpspread.BorderWidth = 1;
            string isstaff = ""; string[] spiltfrom;
            string todate = Convert.ToString(txt_attandance.Text);
            DateTime dt1 = new DateTime();
            string[] split1 = todate.Split('/');
            int m = 0;
            string rollnoo = string.Empty;
            dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
            string hoidaydate = "select CONVERT(varchar(10),  HolidayDate,103) as HolidayDate from HT_Holidays  where HolidayType =1  and HolidayDate = '" + dt1.ToString("MM/dd/yyyy") + "' and HolidayForDayscholar='1' and HolidayForHostler ='1' and HolidayForStaff ='1'";
            ds1.Clear();
            ds1 = d2.select_method_wo_parameter(hoidaydate, "Text");
            if (ds1.Tables[0].Rows.Count == 0)
            {
                string current = DateTime.Now.ToString("dd/MM/yyyy");

                string[] split2 = current.Split('/');
                DateTime dt3 = Convert.ToDateTime(split2[1] + "/" + split2[0] + "/" + split2[2]);

                if (dt1 <= dt3)
                {
                    dt1 = Convert.ToDateTime(split1[2] + "-" + split1[1] + "-" + split1[0]);
                    string[] rollmess_attn = Lblroll.Text.Split(',');
                    string[] appmess_attn = Lblapp.Text.Split(',');
                    for (int ro = 0; ro < rollmess_attn.Length; ro++)
                    {
                        if (Convert.ToString(rollmess_attn[ro]) != "" && Convert.ToString(rollmess_attn[ro]) != "0")
                        {
                            rollnoo = d2.GetFunction("select Roll_no from Registration where App_No='" + appmess_attn[ro] + "'");
                            string sel_att = "select roll_no from HostelMess_Attendance where roll_no='" + Convert.ToString(rollnoo) + "' and entry_date='" + dt1.ToString("MM/dd/yyyy") + "' and session_name='" + Convert.ToString(ddlsession.SelectedItem) + "' and session_code='" + Convert.ToString(ddlsession.SelectedValue) + "' and is_staff='" + isstaff + "' and college_code='" + collegecode1 + "'and Hostel_code='" + Convert.ToString(ddl_Hostel.SelectedValue) + "' and app_no='" + Convert.ToString(appmess_attn[ro]) + "'";
                            messelatt = d2.select_method_wo_parameter(sel_att, "Text");
                            if (messelatt.Tables[0].Rows.Count == 0)
                            {

                                string q1 = "insert into HostelMess_Attendance (roll_no,entry_date,Entry_time,session_name,session_code, is_staff,Hostel_code,college_code,app_no)values('" + Convert.ToString(rollnoo) + "','" + dt1.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("h:mm:ss tt") + "','" + Convert.ToString(ddlsession.SelectedItem) + "','" + Convert.ToString(ddlsession.SelectedValue) + "','" + isstaff + "','" + Convert.ToString(ddl_Hostel.SelectedValue) + "','" + collegecode1 + "','" + Convert.ToString(appmess_attn[ro]) + "')";
                                int up = d2.update_method_wo_parameter(q1, "Text");
                                m = up;
                            }
                            else
                            {
                                try
                                {

                                    Fpspread2.SaveChanges();
                                    string delete_atten = "delete from HostelMess_Attendance where roll_no='" + Convert.ToString(rollnoo) + "' and entry_date='" + dt1.ToString("MM/dd/yyyy") + "' and session_name='" + Convert.ToString(ddlsession.SelectedItem) + "' and  session_code='" + Convert.ToString(ddlsession.SelectedValue) + "' and is_staff='" + isstaff + "' and Hostel_code='" + Convert.ToString(ddl_Hostel.SelectedValue) + "' and  college_code='" + collegecode1 + "' and app_no='" + Convert.ToString(appmess_attn[ro]) + "' ";
                                    int up = d2.update_method_wo_parameter(delete_atten, "Text");
                                    m = up;

                                }
                                catch
                                {
                                }
                            }
                        }
                    }

                   

                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter Attendance Date Grater Than  Or Equal To Current  Date ";
                }

            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Selected Date is Holiday";
            }
           // txrollno.Text = "";
            Go_Click(sender, e);
            Lblroll.Text = "";
            Lblapp.Text = "";
            if (m == 1)
            {
              // Page.MaintainScrollPositionOnPostBack = true;
                alertpopwindow.Visible = true;
                pnl2.Visible = true;
                lblalerterr.Text = "Saved Successfully";
            }
        }
        catch
        {

        }
    }
    # region roomno
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> roomno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {

            DAccess2 dd = new DAccess2();
            WebService ws = new WebService();
            string query = "";
            query = "select distinct rd.Room_Name,rd.Roompk  from Room_Detail rd,Floor_Master hd where rd.Floor_Name=hd.Floor_Name and hd.FloorPK in('" + floorname + "') and hd.Building_Name=rd.Building_Name and rd.Room_Name like '" + prefixText + "%' order by rd.Roompk";
           //query = "select rd.Room_Name from Room_Detail rd,Floor_Master hd where rd.Floor_Name=hd.Floor_Name and rd.Room_Name like '" + prefixText + "%' order by rd.Roompk";

            string MessmasterFK = string.Empty;
            if (roomuser != "" && roomuser != "0")
                MessmasterFK = dd.GetFunction("select LinkValue from New_InsSettings where LinkName='Room Rights' and user_code='" + roomuser + "'");
            if (roomgroupuser != "" && roomgroupuser != "0")
                MessmasterFK = dd.GetFunction("select LinkValue from New_InsSettings where LinkName='Room Rights' and user_code='" + roomgroupuser + "'");
            string itemname = "select distinct rd.Room_Name,rd.Roompk from Room_Detail rd,Floor_Master hd where rd.Floor_Name=hd.Floor_Name and hd.FloorPK in('" + floorname + "') and hd.Building_Name=rd.Building_Name and Roompk in(" + MessmasterFK + ") order by Roompk";
           // string itemname = "select distinct Room_Name,Roompk from Room_Detail where Roompk in(" + MessmasterFK + ")";
          
           // name = ws.Getname(query);
            name = ws.Getname(itemname);
            return name;
        }
        catch { return name; }
    }
    # endregion roomno

    # region rollno
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> rollno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {

            WebService ws = new WebService();
            string query = "";

            query = "select r.Roll_No from HT_HostelRegistration hs,Registration r where hs.APP_No  =r.App_No and ISNULL(IsSuspend,'0')=0  and ISNULL(IsVacated,'0') =0 and ISNULL(IsDiscontinued,'0')=0 and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and  r.Roll_No like '" + prefixText + "%' order by r.roll_no asc";

            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    # endregion

    protected void ddlrollno_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Error.Visible = false;
        //FpSpread1.Visible = false;
        //btnprintmaster.Visible = false;
        txtno.Text = "";
        lblnum.Text = ddlrollno.SelectedItem.ToString();
       
            switch (Convert.ToUInt32(ddlrollno.SelectedItem.Value))
            {
                case 0:
                    txtno.Attributes.Add("placeholder", "Roll No");
                    chosedmode = 0;
                    break;
                case 1:
                    txtno.Attributes.Add("placeholder", "Reg No");
                    chosedmode = 1;
                    break;
                //case 2:
                //    txtno.Attributes.Add("placeholder", "Admin No");
                //    chosedmode = 2;
                //    break;
                //case 3:
                //    txtno.Attributes.Add("placeholder", "App No");
                //    chosedmode = 3;
                //    break;
                case 2:
                    txtno.Attributes.Add("placeholder", "Name");
                    chosedmode = 2;
                    break;
                case 3:
                    txtno.Attributes.Add("placeholder", "Hostel Id");
                    chosedmode = 3;
                    break;
            }
       
      


    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {

            WebService ws = new WebService();
            if (personmode == 0)
            {
                //student query
                if (chosedmode == 0)
                {

                    //query = " select distinct r.Roll_No from HT_HostelRegistration h,Registration r where r.App_No=h.APP_No and r.Roll_No like '" + prefixText + "%' order by Roll_No ";
                    query = "select r.Roll_No,r.Reg_No,r.App_No,r.Stud_Name,mm.MessName,r.Stud_Type,hs.HostelRegistrationPK,hs.HostelMasterFK,Dt.Dept_Name,C.Course_Name ,r.Current_Semester,r.Sections,(select b.Building_Name from Building_Master b where Code=hs.BuildingFK) as Building_Name,(select f.Floor_Name from Floor_Master f where f.FloorPK=hs.FloorFK) as Floor_Name,(select r.Room_Name from Room_Detail r where r.Roompk=hs.RoomFK) as Room_Name,h.HostelName as Hostel_Name from HM_HostelMaster h,HT_HostelRegistration hs,Registration r,Degree d,Department dt,Course c,HM_MessMaster mm  where h.HostelMasterPK =hs.HostelMasterFK and hs.APP_No  =r.App_No and d.Degree_Code =r.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and ISNULL(IsSuspend,'0')=0  and ISNULL(IsVacated,'0') =0 and ISNULL(IsDiscontinued,'0')=0  and mm.MessMasterPK=hs.Messcode and hs.Messcode='" + messco + "' and r.Roll_No like '" + prefixText + "%' order by Roll_No ";


                }
                else if (chosedmode == 1)
                {

                    query = "select r.Reg_No,r.Roll_No,r.App_No,r.Stud_Name,mm.MessName,r.Stud_Type,hs.HostelRegistrationPK,hs.HostelMasterFK,Dt.Dept_Name,C.Course_Name ,r.Current_Semester,r.Sections,(select b.Building_Name from Building_Master b where Code=hs.BuildingFK) as Building_Name,(select f.Floor_Name from Floor_Master f where f.FloorPK=hs.FloorFK) as Floor_Name,(select r.Room_Name from Room_Detail r where r.Roompk=hs.RoomFK) as Room_Name,h.HostelName as Hostel_Name from HM_HostelMaster h,HT_HostelRegistration hs,Registration r,Degree d,Department dt,Course c,HM_MessMaster mm  where h.HostelMasterPK =hs.HostelMasterFK and hs.APP_No  =r.App_No and d.Degree_Code =r.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and ISNULL(IsSuspend,'0')=0  and ISNULL(IsVacated,'0') =0 and ISNULL(IsDiscontinued,'0')=0 and mm.MessMasterPK=hs.Messcode and hs.Messcode='" + messco + "' and r.Reg_No like '" + prefixText + "%' order by Reg_No";
                }

                //else if (chosedmode == 2)
                //{

                //    query = "select distinct r.Roll_Admit from HT_HostelRegistration h,Registration r where r.App_No=h.APP_No and r.Roll_Admit like '" + prefixText + "%' order by Roll_Admit";

                //}
                //else if (chosedmode == 3)
                //{
                //    query = "select distinct r.App_No from HT_HostelRegistration h,Registration r where r.App_No=h.APP_No and r.App_No like '" + prefixText + "%'";

                //}
                else if (chosedmode == 2)
                {

                    query = "select r.Stud_Name,r.Reg_No,r.App_No,r.Roll_No,mm.MessName,r.Stud_Type,hs.HostelRegistrationPK,hs.HostelMasterFK,Dt.Dept_Name,C.Course_Name ,r.Current_Semester,r.Sections,(select b.Building_Name from Building_Master b where Code=hs.BuildingFK) as Building_Name,(select f.Floor_Name from Floor_Master f where f.FloorPK=hs.FloorFK) as Floor_Name,(select r.Room_Name from Room_Detail r where r.Roompk=hs.RoomFK) as Room_Name,h.HostelName as Hostel_Name from HM_HostelMaster h,HT_HostelRegistration hs,Registration r,Degree d,Department dt,Course c,HM_MessMaster mm  where h.HostelMasterPK =hs.HostelMasterFK and hs.APP_No  =r.App_No and d.Degree_Code =r.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and ISNULL(IsSuspend,'0')=0  and ISNULL(IsVacated,'0') =0 and ISNULL(IsDiscontinued,'0')=0  and mm.MessMasterPK=hs.Messcode and hs.Messcode='" + messco + "' and r.Stud_Name like '" + prefixText + "%'";

                }

                else if (chosedmode == 3)
                {
                    query = "select hs.id from HT_HostelRegistration hs,Registration r where   hs.APP_No  =r.App_No   and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and ISNULL(IsSuspend,'0')=0  and ISNULL(IsVacated,'0') =0 and ISNULL(IsDiscontinued,'0')=0 and hs.Messcode='" + messco + "' and hs.id like '" + prefixText + "%'";
                }
            }

           name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    public void load_ddlrollno()
    {
        try
        {
            System.Web.UI.WebControls.ListItem lst1 = new System.Web.UI.WebControls.ListItem("Roll No", "0");
            System.Web.UI.WebControls.ListItem lst2 = new System.Web.UI.WebControls.ListItem("Reg No", "1");
            //System.Web.UI.WebControls.ListItem lst3 = new System.Web.UI.WebControls.ListItem("Admin No", "2");
            //System.Web.UI.WebControls.ListItem lst4 = new System.Web.UI.WebControls.ListItem("App No", "3");
            System.Web.UI.WebControls.ListItem lst5 = new System.Web.UI.WebControls.ListItem("Name", "2");
            System.Web.UI.WebControls.ListItem lst51 = new System.Web.UI.WebControls.ListItem("Hostel Id", "3");

            //Roll Number or Reg Number or Admission No or Application Number
            ddlrollno.Items.Clear();
            string insqry1 = "select value from Master_Settings where settings='Roll No' and usercode ='" + usercode + "' --and college_code ='" + collegecode1 + "'";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Roll No
                ddlrollno.Items.Add(lst1);
            }


            insqry1 = "select value from Master_Settings where settings='Register No' and usercode ='" + usercode + "' --and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                ddlrollno.Items.Add(lst2);
            }

            insqry1 = "select value from Master_Settings where settings='Hostel Id' and usercode ='" + usercode + "' --and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                ddlrollno.Items.Add(lst51);
            }

            //insqry1 = "select value from Master_Settings where settings='Admission No' and usercode ='" + usercode + "' --and college_code ='" + collegecode1 + "'";
            //save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            //if (save1 == 1)
            //{
            //    //Admission No - Roll Admit
            //    ddlrollno.Items.Add(lst3);
            //}

            //insqry1 = "select value from Master_Settings where settings='Application No' and usercode ='" + usercode + "' --and college_code ='" + collegecode1 + "'";
            //save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            //if (save1 == 1)
            //{
            //    //App Form Number - Application Number
            //    ddlrollno.Items.Add(lst4);

            //}
            if (ddlrollno.Items.Count == 0)
            {
                ddlrollno.Items.Add(lst1);
            }
            ddlrollno.Items.Add(lst5);
           
            switch (Convert.ToUInt32(ddlrollno.SelectedItem.Value))
            {
                case 0:
                    txtno.Attributes.Add("placeholder", "Roll No");
                    chosedmode = 0;
                    break;
                case 1:
                    txtno.Attributes.Add("placeholder", "Reg No");
                    chosedmode = 1;
                    break;
                case 2:
                    txtno.Attributes.Add("placeholder", "Hostel Id");
                    chosedmode =3;
                    break;
                //case 2:
                //    txtno.Attributes.Add("placeholder", "Admin No");
                //    chosedmode = 2;
                //    break;
                //case 3:
                //    txtno.Attributes.Add("placeholder", "App No");
                //    chosedmode = 3;
                //    break;
            }
           
        }
        catch { }
    }

    protected void drbbuilding_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindfloor();
        }
        catch
        {
        }
    }
    public void bindbuilding()
    {
        try
        {
            string hostel = string.Empty;
            if (ddl_Hostel.Items.Count > 0)
                hostel = "" + ddl_Hostel.SelectedValue + "";
            string MessmasterFK = string.Empty;
            if (usercode != "" && usercode != "0")
                MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Building Rights' and user_code='" + usercode + "'");
            if (group_user != "" && group_user != "0")
                MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Building Rights' and user_code='" + group_user + "'");
            string itemname =d2.GetFunction( "select HostelBuildingFK From  HM_HostelMaster where HostelMasterPK IN ('" + hostel + "') ");
            string itemnames = "select * from  Building_Master where code in(" + MessmasterFK + ") and code in(" + itemname + ")";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemnames, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                drbbuilding.DataSource = ds;
                drbbuilding.DataTextField = "Building_name";
                drbbuilding.DataValueField = "code";
                drbbuilding.DataBind();
            }

        }
        catch
        {
        }
    }
}
