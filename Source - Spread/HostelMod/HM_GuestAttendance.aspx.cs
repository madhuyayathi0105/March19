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

public partial class HM_GuestAttendance : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    bool check = false;
    bool checkdate = false;
    string fromdate = "";
    string todate = "";
    int i = 0;
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
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        usercode = Session["usercode"].ToString();
        calfromdate.EndDate = DateTime.Now;
        caltodate.EndDate = DateTime.Now;
        if (!IsPostBack)
        {
            bindhostelhostel();
            ViewState["colcountnewvalue"] = null;
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_todate.Attributes.Add("readonly", "readonly");
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Visible = false;
        }
    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }
    protected void cb_hostelname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_hostelname.Checked == true)
            {
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = true;
                }
                txt_hostelname.Text = "Hostel Name(" + (cbl_hostelname.Items.Count) + ")";

                cbl_hostelname_SelectedIndexChanged(sender, e);
            }
            else
            {
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = false;
                }
                txt_hostelname.Text = "--Select--";
                cbl_buildingname.Items.Clear();
                txt_buildingname.Text = "--Select--";
                cb_buildingname.Checked = false;
                cbl_floorname.Items.Clear();
                txt_floorname.Text = "--Select--";
                cb_floorname.Checked = false;
                cbl_roomname.Items.Clear();
                txt_roomname.Text = "--Select--";
                cb_roomname.Checked = false;


            }

        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_hostelname.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_hostelname.Text = "--Select--";
                    cb_hostelname.Checked = false;
                    build = cbl_hostelname.Items[i].Value.ToString();
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
            clgbuild(buildvalue);
            //clgfloorpop(buildvalue);
            if (seatcount == cbl_hostelname.Items.Count)
            {
                txt_hostelname.Text = "Hostel Name(" + seatcount + ")";
                cb_hostelname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_hostelname.Text = "--Select--";
            }
            else
            {
                txt_hostelname.Text = "Hostel Name(" + seatcount + ")";
            }
        }
        catch (Exception ex)
        {
        }

    }
    public void bindhostelhostel()
    {
        try
        {
            //ds = d2.BindHostel_inv(collegecode1);
            //string itemname = "select HostelMasterPK,HostelName from HM_HostelMaster order by HostelName ";
            //ds.Clear();
            //ds = d2.select_method_wo_parameter(itemname, "Text");

            //string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            //ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            string MessmasterFK = string.Empty;
            if (usercode != "" && usercode != "0")
               MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Hostel Rights' and user_code='" + usercode + "'");
            if (group_user != "" && group_user != "0")
                MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Hostel Rights' and user_code='" + group_user + "'");
            string itemname = "select HostelMasterPK,HostelName from HM_HostelMaster where  HostelMasterPK in (" + MessmasterFK + ") order by hostelname ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "HostelName";
                cbl_hostelname.DataValueField = "HostelMasterPK";
                cbl_hostelname.DataBind();

                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = true;
                    txt_hostelname.Text = "Hostel(" + (cbl_hostelname.Items.Count) + ")";
                    cb_hostelname.Checked = true;
                }

                string lochosname = "";
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    if (cbl_hostelname.Items[i].Selected == true)
                    {
                        string hosname = cbl_hostelname.Items[i].Value.ToString();
                        if (lochosname == "")
                        {
                            lochosname = hosname;
                        }
                        else
                        {
                            lochosname = lochosname + "'" + "," + "'" + hosname;
                        }
                    }
                }

                clgbuild(lochosname);

            }
            else
            {
                cbl_hostelname.Items.Insert(0, "--Select--");
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void txt_fromdate_Textchanged(object sender, EventArgs e)
    {
        try
        {
            lbl_error1.Visible = false;
            fromdate = txt_fromdate.Text;
            todate = txt_todate.Text;
            if (fromdate != "" && fromdate != null && todate != "" && todate != null)
            {
                string[] spiltfrom = fromdate.Split('/');
                string[] spitto = todate.Split('/');
                DateTime from = Convert.ToDateTime(spiltfrom[1] + '/' + spiltfrom[0] + '/' + spiltfrom[2]);
                DateTime to = Convert.ToDateTime(spitto[1] + '/' + spitto[0] + '/' + spitto[2]);
                if (from > to)
                {
                    lbl_error1.Visible = true;
                    lbl_error1.Text = "Please Enter To Date Greater Than From Date";
                    FpSpread1.Visible = false;
                    lbl_holiday.Visible = false;
                    pheaderfilter.Visible = false;
                    pcolumnorder.Visible = false;
                    dat.Visible = false;
                    btn_save.Visible = false;
                    btn_update.Visible = false;
                    btn_reset.Visible = false;
                    dat.Visible = false;
                    rptprint.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lbl_error1.Visible = true;
            lbl_error1.Text = ex.ToString();
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
                lblvalidation1.Text = "Please the report name";
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
            string degreedetails = "Hostel Guest Report";
            string pagename = "HM_GuestAttendance.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }

    }
    protected void txt_todate_Textchanged(object sender, EventArgs e)
    {
        try
        {
            lbl_error1.Visible = false;
            fromdate = txt_fromdate.Text;
            todate = txt_todate.Text;
            if (fromdate != "" && fromdate != null && todate != "" && todate != null)
            {
                string[] spiltfrom = fromdate.Split('/');
                string[] spitto = todate.Split('/');
                DateTime from = Convert.ToDateTime(spiltfrom[1] + '/' + spiltfrom[0] + '/' + spiltfrom[2]);
                DateTime to = Convert.ToDateTime(spitto[1] + '/' + spitto[0] + '/' + spitto[2]);

                string todate1 = DateTime.Now.ToString("dd/MM/yyyy");
                string[] todate2 = todate1.Split('/');
                DateTime todate3 = Convert.ToDateTime(todate2[1] + '/' + todate2[0] + '/' + todate2[2]);


                if (from > to)// && to <= todate3
                {
                    lbl_error1.Visible = true;
                    lbl_error1.Text = "Please Enter To Date Grater Than From Date";
                    FpSpread1.Visible = false;
                    dat.Visible = false;
                    btn_save.Visible = false;
                    btn_update.Visible = false;
                    btn_reset.Visible = false;
                    dat.Visible = false;
                    rptprint.Visible = false;
                    lbl_holiday.Visible = false;
                    pheaderfilter.Visible = false;
                    pcolumnorder.Visible = false;
                }
                if (to > todate3)
                {
                    lbl_error1.Visible = true;
                    //lbl_errorsearch1.Visible = false;
                    lbl_error1.Text = "Don't Enter Future Date";
                    FpSpread1.Visible = false;
                    dat.Visible = false;
                    btn_save.Visible = false;
                    btn_update.Visible = false;
                    btn_reset.Visible = false;
                    dat.Visible = false;
                    rptprint.Visible = false;
                    lbl_holiday.Visible = false;
                    pheaderfilter.Visible = false;
                    pcolumnorder.Visible = false;

                }
            }
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
            lbl_error.Text = ex.ToString();
        }
    }
    public void clgbuild(string hostelname)
    {
        try
        {
            cbl_buildingname.Items.Clear();
            string bul = "";
            bul = d2.GetBuildingCode_inv(hostelname);
            ds = d2.BindBuilding(bul);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_buildingname.DataSource = ds;
                cbl_buildingname.DataTextField = "Building_Name";
                cbl_buildingname.DataValueField = "code";
                cbl_buildingname.DataBind();
            }

            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
            {
                cbl_buildingname.Items[i].Selected = true;
                txt_buildingname.Text = "Building(" + (cbl_buildingname.Items.Count) + ")";
                cb_buildingname.Checked = true;
            }

            string locbuild = "";
            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
            {
                if (cbl_buildingname.Items[i].Selected == true)
                {
                    string builname = cbl_buildingname.Items[i].Text;
                    if (locbuild == "")
                    {
                        locbuild = builname;
                    }
                    else
                    {
                        locbuild = locbuild + "'" + "," + "'" + builname;
                    }
                }
            }
            clgfloor(locbuild);
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbbuildname_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_buildingname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                string lochosname = "";
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    if (cbl_hostelname.Items[i].Selected == true)
                    {
                        string hosname = cbl_hostelname.Items[i].Value.ToString();
                        if (lochosname == "")
                        {
                            lochosname = hosname;
                        }
                        else
                        {
                            lochosname = lochosname + "'" + "," + "'" + hosname;
                        }
                    }
                }
                cbl_buildingname.Items.Clear();
                clgbuild(lochosname);

                for (int i = 0; i < cbl_buildingname.Items.Count; i++)
                {
                    if (cb_buildingname.Checked == true)
                    {
                        cbl_buildingname.Items[i].Selected = true;
                        txt_buildingname.Text = "Building(" + (cbl_buildingname.Items.Count) + ")";
                        //txt_floorname.Text = "--Select--";
                        build1 = cbl_buildingname.Items[i].Text.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;

                        }

                    }
                }
                clgfloor(buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_buildingname.Items.Count; i++)
                {
                    cbl_buildingname.Items[i].Selected = false;
                    txt_buildingname.Text = "--Select--";
                    cbl_floorname.Items.Clear();
                    cb_floorname.Checked = false;
                    txt_floorname.Text = "--Select--";
                    txt_roomname.Text = "--Select--";
                    cb_roomname.Checked = false;
                    cbl_roomname.Items.Clear();
                }
            }
            //  Button2.Focus();

        }
        catch (Exception ex)
        {
        }
    }
    protected void cblbuildname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_buildingname.Checked = false;

            string buildvalue = "";
            string build = "";
            string lochosname = "";
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    string hosname = cbl_hostelname.Items[i].Value.ToString();
                    if (lochosname == "")
                    {
                        lochosname = hosname;
                    }
                    else
                    {
                        lochosname = lochosname + "'" + "," + "'" + hosname;
                    }
                }
            }
            //cbl_buildingname.Items.Clear();
            //clgbuild(lochosname);

            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
            {
                if (cbl_buildingname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    cb_floorname.Checked = true;
                    build = cbl_buildingname.Items[i].Text.ToString();
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

            clgfloor(buildvalue);

            if (seatcount == cbl_buildingname.Items.Count)
            {
                txt_buildingname.Text = "Building(" + seatcount + ")";
                cb_buildingname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_buildingname.Text = "--Select--";
            }
            else
            {
                txt_buildingname.Text = "Building(" + seatcount + ")";
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void clgfloor(string buildname)
    {
        try
        {
            //chklstfloorpo3.Items.Clear();
            cbl_floorname.Items.Clear();
            ds = d2.BindFloor(buildname);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_floorname.DataSource = ds;
                cbl_floorname.DataTextField = "Floor_Name";
                cbl_floorname.DataValueField = "FloorPK";
                cbl_floorname.DataBind();

            }
            else
            {
                txt_floorname.Text = "--Select--";
            }
            //for selected floor
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                cbl_floorname.Items[i].Selected = true;
                cb_floorname.Checked = true;
            }

            string locfloor = "";
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    txt_floorname.Text = "Floor(" + (cbl_floorname.Items.Count) + ")";
                    string flrname = cbl_floorname.Items[i].Text;
                    if (locfloor == "")
                    {
                        locfloor = flrname;
                    }
                    else
                    {
                        locfloor = locfloor + "'" + "," + "'" + flrname;
                    }
                }

            }
            clgroom(locfloor, buildname);
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbfloorname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_floorname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                string build2 = "";
                string buildvalue2 = "";

                if (cb_buildingname.Checked == true)
                {
                    for (int i = 0; i < cbl_buildingname.Items.Count; i++)
                    {
                        build1 = cbl_buildingname.Items[i].Text.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                }
                if (cb_floorname.Checked == true)
                {
                    for (int j = 0; j < cbl_floorname.Items.Count; j++)
                    {
                        cbl_floorname.Items[j].Selected = true;
                        txt_floorname.Text = "Floor(" + (cbl_floorname.Items.Count) + ")";
                        build2 = cbl_floorname.Items[j].Text.ToString();
                        if (buildvalue2 == "")
                        {
                            buildvalue2 = build2;
                        }
                        else
                        {
                            buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                        }
                    }
                }
                clgroom(buildvalue2, buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_floorname.Items.Count; i++)
                {
                    cbl_floorname.Items[i].Selected = false;
                    txt_floorname.Text = "--Select--";
                }
                cb_roomname.Checked = false;
                cbl_roomname.Items.Clear();
                txt_roomname.Text = "--Select--";
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    protected void cblfloorname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_floorname.Checked = false;
            string buildvalue1 = "";
            string build1 = "";
            string build2 = "";
            string buildvalue2 = "";
            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
            {
                if (cbl_buildingname.Items[i].Selected == true)
                {
                    build1 = cbl_buildingname.Items[i].Text.ToString();
                    if (buildvalue1 == "")
                    {
                        buildvalue1 = build1;
                    }
                    else
                    {
                        buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                    }

                }
            }
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    build2 = cbl_floorname.Items[i].Text.ToString();
                    if (buildvalue2 == "")
                    {
                        buildvalue2 = build2;
                    }
                    else
                    {
                        buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                    }
                }
            }
            clgroom(buildvalue2, buildvalue1);

            if (seatcount == cbl_floorname.Items.Count)
            {
                txt_floorname.Text = "Floor(" + seatcount.ToString() + ")";
                cb_floorname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_floorname.Text = "--Select--";
            }
            else
            {
                txt_floorname.Text = "Floor(" + seatcount.ToString() + ")";
            }
            //   Button2.Focus();
            //  clgroom(buildvalue1, buildvalue2);
        }
        catch (Exception ex)
        {
        }
    }
    public void clgroom(string floorname, string buildname)
    {
        try
        {
            cbl_roomname.Items.Clear();
            ds = d2.BindRoom(floorname, buildname);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_roomname.DataSource = ds;
                cbl_roomname.DataTextField = "Room_Name";
                cbl_roomname.DataValueField = "Roompk";
                cbl_roomname.DataBind();
            }
            else
            {
                txt_roomname.Text = "--Select--";
            }

            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                cbl_roomname.Items[i].Selected = true;
                txt_roomname.Text = "Room(" + (cbl_roomname.Items.Count) + ")";
                cb_roomname.Checked = true;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbroomname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_roomname.Checked == true)
            {
                for (int i = 0; i < cbl_roomname.Items.Count; i++)
                {
                    cbl_roomname.Items[i].Selected = true;
                }
                txt_roomname.Text = "Room(" + (cbl_roomname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_roomname.Items.Count; i++)
                {
                    cbl_roomname.Items[i].Selected = false;
                }
                txt_roomname.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cblroomname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_roomname.Checked = false;
            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                if (cbl_roomname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }

            }
            if (seatcount == cbl_roomname.Items.Count)
            {
                txt_roomname.Text = "Room(" + seatcount.ToString() + ")";
                cb_roomname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_roomname.Text = "--Select--";
            }
            else
            {
                txt_roomname.Text = "Room(" + seatcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string date = "";
            string floorname = "";
            string date1 = "";
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    string floorname1 = cbl_floorname.Items[i].Value.ToString();
                    if (floorname == "")
                    {
                        floorname = floorname1;
                    }
                    else
                    {
                        floorname = floorname + "'" + "," + "'" + floorname1;
                    }
                }
            }
            string buildingname = "";
            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
            {
                if (cbl_buildingname.Items[i].Selected == true)
                {
                    string buildingname1 = cbl_buildingname.Items[i].Value.ToString();
                    if (buildingname == "")
                    {
                        buildingname = buildingname1;
                    }
                    else
                    {
                        buildingname = buildingname + "'" + "," + "'" + buildingname1;
                    }
                }
            }
            string roomname = "";
            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                if (cbl_roomname.Items[i].Selected == true)
                {
                    string roomname1 = cbl_roomname.Items[i].Value.ToString();
                    if (roomname == "")
                    {
                        roomname = roomname1;
                    }
                    else
                    {
                        roomname = roomname + "'" + "," + "'" + roomname1;
                    }
                }
            }
            string hoscode = "";
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    string hoscode1 = cbl_hostelname.Items[i].Value.ToString();
                    if (hoscode == "")
                    {
                        hoscode = hoscode1;
                    }
                    else
                    {
                        hoscode = hoscode + "'" + "," + "'" + hoscode1;
                    }
                }
            }
            string fromdate = Convert.ToString(txt_fromdate.Text);
            DateTime dt = new DateTime();
            string[] split = fromdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            string todate = Convert.ToString(txt_todate.Text);
            DateTime dt1 = new DateTime();
            string[] split1 = todate.Split('/');
            dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
            string current = DateTime.Now.ToString("dd/MM/yyyy");
            string[] split2 = current.Split('/');
            DateTime dt3 = Convert.ToDateTime(split2[1] + "/" + split2[0] + "/" + split2[2]);

            string hoidaydate = "select CONVERT(varchar(10),  HolidayDate,103) as HolidayDate from HT_Holidays  where HolidayType =1  and HolidayDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and HolidayForDayscholar='1' and HolidayForHostler ='1' and HolidayForStaff ='1'";
            ds1.Clear();
            ds1 = d2.select_method_wo_parameter(hoidaydate, "Text");
            ArrayList newarray = new ArrayList();

            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    newarray.Add(Convert.ToString(ds1.Tables[0].Rows[i]["HolidayDate"]));
                }

            }
            if (txt_hostelname.Text.Trim() != "--Select--" && txt_buildingname.Text.Trim() != "--Select--" && txt_floorname.Text.Trim() != "--Select--" && txt_roomname.Text.Trim() != "--Select--")
            {
                //string q = "select hd.Hostel_Name, Guest_Name,GuestCode,Guest_Address,MobileNo,From_Company,Floor_Name,Room_Name,gr.Hostel_Code,convert(varchar(10),Admission_Date ,103)as Admission_Date,bm.Building_Name,bm.Code,Guest_Street,Guest_City,Guest_PinCode,Purpose from Hostel_GuestReg gr,Hostel_Details hd,Building_Master bm,Hostel_Details hh where gr.Hostel_Code=hd.Hostel_code and gr.Hostel_Code=hh.Hostel_code and bm.Building_Name=gr.Building_Name  and bm.Code in('" + buildingname + "') and gr.college_code='" + collegecode1 + "' and Floor_Name in('" + floorname + "') and Room_Name in('" + roomname + "') and gr.Hostel_Code in('" + hoscode + "')";//  and Admission_Date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'";

                string q = "select HM.HostelName as Hostel_Name,H.id,Vi.VenContactName as Guest_Name,Vi.VendorContactPK as GuestCode,V.VendorAddress as Guest_Address,Vi.VendorMobileNo as MobileNo,V.VendorCompName as From_Company,f.Floor_Name as Floor_Name,r.Room_Name as Room_Name,HM.HostelMasterPK as Hostel_Code,B.Building_Name,B.Code,V.VendorStreet as Guest_Street,V.VendorCity as Guest_City,V.VendorPin as Guest_PinCode from HT_HostelRegistration H,CO_VendorMaster V,IM_VendorContactMaster Vi,Building_Master B,Floor_Master f,Room_Detail r,HM_HostelMaster HM where hm.HostelMasterPK =h.HostelMasterFK and v.VendorPK=vi.VendorFK and b.Code =h.BuildingFK and f.FloorPK=H.FloorFK and r.RoomPk=H.RoomFK and B.Code in('" + buildingname + "') and H.FloorFK in('" + floorname + "') and H.RoomFK in('" + roomname + "') and HM.HostelMasterPK in('" + hoscode + "') and H.GuestVendorFK=v.VendorPK and vi.VendorContactPK=h.APP_No";//and HM.CollegeCode='" + collegecode1 + "' 

                ds.Clear();
                ds = d2.select_method_wo_parameter(q, "Text");
                if (dt <= dt1 && dt1 <= dt3)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 1;
                        FpSpread1.Sheets[0].AutoPostBack = false;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;

                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                        FpSpread1.Columns[0].Width = 50;
                        FpSpread1.Columns[0].Locked = true;

                        Hashtable columnhash = new Hashtable();
                        columnhash.Add("Hostel_Name", "Hostel Name");
                        columnhash.Add("id", "Guest Id");
                        columnhash.Add("Guest_Name", "Guest Name");
                        columnhash.Add("Guest_Address", "Guest Address");
                        columnhash.Add("MobileNo", "Mobile No");
                        columnhash.Add("From_Company", "From Company");
                        columnhash.Add("Floor_Name", "Floor Name");
                        columnhash.Add("Room_Name", "Room Name");
                        //columnhash.Add("Admission_Date", "Admission Date");
                        columnhash.Add("Building_Name", "Building Name");
                        columnhash.Add("Guest_Street", "Guest Street");
                        columnhash.Add("Guest_City", "Guest City");
                        columnhash.Add("Guest_PinCode", "Guest Pincode");
                        //columnhash.Add("Purpose", "Purpose");

                        if (ItemList.Count != 0)
                        {
                            FpSpread1.Sheets[0].SpanModel.Add(0, 0, 1, ItemList.Count);
                        }

                        if (ItemList.Count == 0)
                        {
                            ItemList.Add("Hostel_Name");
                            ItemList.Add("id");
                            ItemList.Add("Guest_Name");
                            ItemList.Add("MobileNo");
                            ItemList.Add("From_Company");
                        }

                        for (int jk = 0; jk < ds.Tables[0].Columns.Count; jk++)
                        {
                            string colno = Convert.ToString(ds.Tables[0].Columns[jk]);
                            if (ItemList.Contains(Convert.ToString(colno)))
                            {
                                int index = ItemList.IndexOf(Convert.ToString(colno));
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(columnhash[colno]);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                            }
                        }

                        while (dt <= dt1)
                        {
                            if (!newarray.Contains(dt.ToString("dd/MM/yyyy")))
                            {
                                checkdate = true;
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dt.ToString("dd/MM/yyyy"));
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;

                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "P";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;

                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "A";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;

                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 2, 1, 2);

                            }
                            else
                            {
                                if (date == "")
                                {
                                    date = "" + Convert.ToString(dt.ToString("dd/MM/yyyy")) + "";
                                }
                                else
                                {
                                    date = date + "," + Convert.ToString(dt.ToString("dd/MM/yyyy")) + "";
                                }
                                // date1=date;
                                // date = Convert.ToString(dt.ToString("dd/MM/yyyy"));

                            }
                            dt = dt.AddDays(1);
                        }
                        if (checkdate == true)
                        {

                            FpSpread1.Sheets[0].RowCount++;
                            if (txt_fromdate.Text.Trim() != "" && txt_todate.Text.Trim() != "")//&& ddl_hostelname.Text.Trim() != "Select"
                            {
                                FarPoint.Web.Spread.TextCellType txtreg = new FarPoint.Web.Spread.TextCellType();
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                                    {
                                        if (ItemList.Contains(Convert.ToString(ds.Tables[0].Columns[j].ToString())))
                                        {
                                            int index = ItemList.IndexOf(Convert.ToString(ds.Tables[0].Columns[j].ToString()));
                                            FpSpread1.Sheets[0].Columns[index + 1].Width = 150;
                                            FpSpread1.Sheets[0].Columns[index + 1].Locked = true;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, index + 1].CellType = txtreg;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, index + 1].Text = ds.Tables[0].Rows[i][j].ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, index + 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, index + 1].Font.Size = FontUnit.Medium;

                                        }
                                        string Hostel_Code1 = Convert.ToString(ds.Tables[0].Columns[j]);
                                        string Hostel_Code = "Hostel_Code";
                                        string guestcode1 = Convert.ToString(ds.Tables[0].Columns[j]);


                                        if (Hostel_Code1 == Hostel_Code)
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Hostel_Code"]);

                                        }
                                        if (guestcode1 == Convert.ToString("GuestCode"))
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["GuestCode"]);
                                        }
                                    }
                                }
                                FarPoint.Web.Spread.CheckBoxCellType chkdate = new FarPoint.Web.Spread.CheckBoxCellType();
                                chkdate.AutoPostBack = true;
                                chkdate.Text = " ";
                                FarPoint.Web.Spread.CheckBoxCellType chkdate1 = new FarPoint.Web.Spread.CheckBoxCellType();
                                chkdate1.AutoPostBack = true;
                                chkdate1.Text = " ";

                                string[] spiltfrom;
                                string Attendance = "";
                                string rollno = "";
                                string hostel = "";
                                string insertquery = "";
                                string columngetvalue = "";
                                string AttndDayvalue = "";
                                string attnmonth = "";
                                string attnyear = "";
                                string attnday = "";
                                ViewState["colcountnewvalue"] = ItemList.Count + 2;

                                for (int col = ItemList.Count + 2; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                                {
                                    FpSpread1.Sheets[0].Cells[0, col - 1].CellType = chkdate1;
                                    FpSpread1.Sheets[0].Cells[0, col - 1].HorizontalAlign = HorizontalAlign.Center;

                                    FpSpread1.Sheets[0].Cells[0, col].CellType = chkdate1;
                                    FpSpread1.Sheets[0].Cells[0, col].HorizontalAlign = HorizontalAlign.Center;

                                    //FpSpread1.Sheets[0].Cells[0, (col - 1) + 1].CellType = chkdate1;
                                    //FpSpread1.Sheets[0].Cells[0, (col - 1) + 1].HorizontalAlign = HorizontalAlign.Center;

                                    string columngetvalue1 = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, col - 1].Text);
                                    spiltfrom = columngetvalue1.Split('/');
                                    AttndDayvalue = Convert.ToString(spiltfrom[0]);
                                    AttndDayvalue = AttndDayvalue.TrimStart('0');
                                    attnday = AttndDayvalue;
                                    AttndDayvalue = "[d" + AttndDayvalue + "]";
                                    attnmonth = spiltfrom[1];
                                    attnmonth = attnmonth.TrimStart('0');
                                    attnyear = spiltfrom[2];

                                    for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                                    {
                                        FpSpread1.Sheets[0].Cells[i, col - 1].CellType = chkdate;
                                        FpSpread1.Sheets[0].Cells[i, col - 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Columns[col - 1].Width = 45;

                                        FpSpread1.Sheets[0].Cells[i, col].CellType = chkdate;
                                        FpSpread1.Sheets[0].Cells[i, col].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Columns[col].Width = 45;


                                        hostel = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag);
                                        rollno = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Tag);
                                        string getvalue = d2.GetFunction("select " + AttndDayvalue + " from HT_Attendance where App_No ='" + rollno + "' and AttnMonth='" + attnmonth + "' and attnYear='" + attnyear + "'");
                                        if (getvalue != "" && getvalue != "0")
                                        {
                                            if (getvalue.Trim() == "1")
                                            {
                                                FpSpread1.Sheets[0].Cells[i, col - 1].Value = 1;
                                            }
                                            else if (getvalue.Trim() == "2")
                                            {
                                                FpSpread1.Sheets[0].Cells[i, col].Value = 1;
                                            }

                                        }
                                        else
                                        {
                                            FpSpread1.Sheets[0].Cells[i, col].Value = 0;
                                            FpSpread1.Sheets[0].Cells[i, col].Value = 0;

                                        }
                                    }
                                }
                            }
                            dat.Visible = true;
                            FpSpread1.Visible = true;
                            btn_save.Visible = true;
                            btn_update.Visible = false;
                            btn_reset.Visible = true;
                            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                            FpSpread1.SaveChanges();
                            //FpSpread1.Sheets[0].SpanModel.Add(0, 0, 1, 4);
                            rptprint.Visible = true;
                            pheaderfilter.Visible = true;
                            pcolumnorder.Visible = true;
                            lbl_error.Visible = false;
                            if (date != "")
                            {
                                lbl_holiday.Visible = true;
                                lbl_holiday.Text = date + "---Holiday";
                            }
                            else
                            {
                                lbl_holiday.Visible = false;

                            }


                        }
                        else
                        {
                            btn_save.Visible = false;
                            btn_update.Visible = false;
                            btn_reset.Visible = false;
                            dat.Visible = false;
                            rptprint.Visible = false;
                            lbl_holiday.Visible = true;
                            pheaderfilter.Visible = false;
                            pcolumnorder.Visible = false;

                            lbl_holiday.Text = "Selected Date Is Holiday";
                        }


                    }
                    else
                    {
                        btn_save.Visible = false;
                        btn_update.Visible = false;
                        btn_reset.Visible = false;
                        dat.Visible = false;
                        rptprint.Visible = false;
                        pheaderfilter.Visible = false;
                        pcolumnorder.Visible = false;
                        lbl_holiday.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "No Records Found";


                    }
                }
            }
            else
            {
                btn_save.Visible = false;
                btn_update.Visible = false;
                btn_reset.Visible = false;
                dat.Visible = false;
                rptprint.Visible = false;
                pheaderfilter.Visible = false;
                pcolumnorder.Visible = false;
                lbl_holiday.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Please select all fields";
            }
        }
        catch
        { }
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
                    //if (rdb_cumulative.Checked == true)
                    //{
                    //    ItemList.Remove("Date");                    
                    //    Itemindex.Remove(si == "Date");
                    //    ItemList.Remove("Description");
                    //    Itemindex.Remove(si == "Description");
                    //}
                    //else
                    //{
                    si = Convert.ToString(i);
                    cblcolumnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                    ItemList.Add(cblcolumnorder.Items[i].Value.ToString());
                    Itemindex.Add(si);
                    //}
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
                    //if (tborder.Text == "")
                    //{
                    //    ItemList.Add("Roll No");
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
            for (i = 0; i < cblcolumnorder.Items.Count; i++)
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
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            fromdate = txt_fromdate.Text;
            todate = txt_todate.Text;
            bool saveflage = false;
            if (txt_floorname.Text.Trim() != "--Select--")
            {
                string[] spiltfrom = fromdate.Split('/');
                string[] spitto = todate.Split('/');
                DateTime from = Convert.ToDateTime(spiltfrom[1] + '/' + spiltfrom[0] + '/' + spiltfrom[2]);
                DateTime to = Convert.ToDateTime(spitto[1] + '/' + spitto[0] + '/' + spitto[2]);
                string attnday = spiltfrom[0];
                attnday = attnday.TrimStart('0');
                string attnmonth = spiltfrom[1];
                attnmonth = attnmonth.TrimStart('0');
                string attnyear = spiltfrom[2];
                string Attendance = "";
                string rollno = "";
                string insertquery = "";
                string columngetvalue = "";
                string AttndDayvalue = "";

                if (from > to)
                {
                    lbl_error1.Visible = true;
                    lbl_error1.Text = "Please Enter To Date Grater Than From Date";
                }
                else
                {
                    if (FpSpread1.Sheets[0].RowCount > 0)
                    {
                        int colnewvlaue = Convert.ToInt32(ViewState["colcountnewvalue"]);
                        for (int col = colnewvlaue; col < FpSpread1.Sheets[0].ColumnCount; col = col + 2)
                        {
                            FpSpread1.SaveChanges();
                            columngetvalue = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, col - 1].Text);
                            spiltfrom = columngetvalue.Split('/');
                            AttndDayvalue = Convert.ToString(spiltfrom[0]);
                            AttndDayvalue = AttndDayvalue.TrimStart('0');
                            attnday = AttndDayvalue;
                            AttndDayvalue = "[D" + AttndDayvalue + "]";

                            attnmonth = spiltfrom[1];
                            attnmonth = attnmonth.TrimStart('0');
                            attnyear = spiltfrom[2];


                            string hostelcode = "";
                            for (int j = 1; j < FpSpread1.Sheets[0].RowCount; j++)
                            {
                                hostelcode = Convert.ToString(FpSpread1.Sheets[0].Cells[j, 1].Tag);
                                string guestcode = "";
                                guestcode = Convert.ToString(FpSpread1.Sheets[0].Cells[j, 2].Tag);
                                //string app_no = d2.GetFunction("select App_No from Registration where Roll_No='" + guestcode.Trim() + "' and college_code='" + collegecode1 + "'");

                                Attendance = "0";

                                int checkvalue = Convert.ToInt32(FpSpread1.Sheets[0].Cells[j, col - 1].Value);
                                if (checkvalue == 1)
                                {
                                    Attendance = "1";
                                }
                                int checkvalue1 = Convert.ToInt32(FpSpread1.Sheets[0].Cells[j, col].Value);
                                if (checkvalue1 == 1)
                                {
                                    Attendance = "2";
                                }

                                //insertquery = "if exists (select * from HAttendance where Roll_No ='" + guestcode.Trim() + "' and AttnMonth='" + attnmonth.Trim() + "' and AttnYear='" + attnyear.Trim() + "' and Hostel_Code='" + hostelcode.Trim() + "' and stud_type='3') update HAttendance set " + AttndDayvalue.Trim() + "=" + Attendance.Trim() + " where Roll_No ='" + guestcode.Trim() + "' and AttnMonth='" + attnmonth.Trim() + "' and AttnYear='" + attnyear.Trim() + "' and Hostel_Code='" + hostelcode.Trim() + "' and stud_type='3' else insert into HAttendance(Roll_No,Hostel_Code,AttnMonth,AttnYear," + AttndDayvalue.Trim() + ",stud_type) values ('" + guestcode.Trim() + "','" + hostelcode.Trim() + "','" + attnmonth.Trim() + "','" + attnyear.Trim() + "','" + Attendance.Trim() + "','3')";

                                insertquery = "if exists (select * from HT_Attendance where App_No ='" + guestcode.Trim() + "' and AttnMonth='" + attnmonth.Trim() + "' and AttnYear='" + attnyear.Trim() + "') update HT_Attendance set " + AttndDayvalue.Trim() + "=" + Attendance.Trim() + " where App_No ='" + guestcode.Trim() + "' and AttnMonth='" + attnmonth.Trim() + "' and AttnYear='" + attnyear.Trim() + "' else insert into HT_Attendance(App_No,AttnMonth,AttnYear," + AttndDayvalue.Trim() + ") values ('" + guestcode.Trim() + "','" + attnmonth.Trim() + "','" + attnyear.Trim() + "','" + Attendance.Trim() + "')";

                                int retu = d2.update_method_wo_parameter(insertquery, "Text");
                                if (retu != 0)
                                {
                                    saveflage = true;
                                }

                            }
                        }
                    }
                    if (saveflage == true)
                    {
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Saved Successfully";
                        alertmessage.Visible = true;
                    }
                    else
                    {
                        //lbl_alerterror.Visible = true;
                        //lbl_alerterror.Text = "Please Update Attendance";
                        //alertmessage.Visible = true;
                    }
                }
            }


        }
        catch { }
    }
    protected void btn_update_Click(object sender, EventArgs e)
    {
        try { }
        catch { }
    }
    protected void btn_reset_Click(object sender, EventArgs e)
    {
        try
        {
            imgbtnclear_abstclick(sender, e);
            imgbtnclear_presentclick(sender, e);
        }
        catch { }

    }

    protected void imgbtnclear_abstclick(object sender, EventArgs e)
    {
        FpSpread1.SaveChanges();
        if (FpSpread1.Sheets[0].RowCount > 0)
        {
            //for (int col = 6; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
            //{
            int colnewvlaue = Convert.ToInt32(ViewState["colcountnewvalue"]);
            for (int col = colnewvlaue; col < FpSpread1.Sheets[0].ColumnCount; col = col + 2)
            {
                string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text);
                if (s == "A")
                {
                    for (i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                    {

                        FpSpread1.Sheets[0].Cells[i, col].Value = 0;
                        FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;

                    }
                }
            }
        }
        //cb_abst.Checked=false;
        //cb_present.Checked = false;
        //od.Checked = false;
    }

    protected void imgbtnclear_presentclick(object sender, EventArgs e)
    {
        FpSpread1.SaveChanges();
        if (FpSpread1.Sheets[0].RowCount > 0)
        {
            int colnewvlaue = Convert.ToInt32(ViewState["colcountnewvalue"]);
            for (int col = colnewvlaue; col < FpSpread1.Sheets[0].ColumnCount; col = col + 2)
            //for (int col = 6; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
            {
                string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col - 1].Text);
                if (s == "P")
                {
                    for (i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                    {
                        FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                        FpSpread1.Sheets[0].Cells[i, col].Value = 0;

                    }
                }
            }

        }
        //cb_abst.Checked=false;
        // //cb_present.Checked=false;
        // od.Checked = false;

    }

    protected void imgbtn_presentclick(object sender, EventArgs e)
    {
        FpSpread1.SaveChanges();
        if (FpSpread1.Sheets[0].RowCount > 0)
        {
            int colnewvlaue = Convert.ToInt32(ViewState["colcountnewvalue"]);
            for (int col = colnewvlaue; col < FpSpread1.Sheets[0].ColumnCount; col = col + 2)
            //for (int col = 6; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
            {
                string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col - 1].Text);
                if (s == "P")
                {
                    for (i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                    {
                        FpSpread1.Sheets[0].Cells[i, col - 1].Value = 1;
                        FpSpread1.Sheets[0].Cells[i, col].Value = 0;

                    }
                }
            }

        }
        //cb_abst.Checked=false;
        // //cb_present.Checked=false;
        // od.Checked = false;

    }
    protected void imgbtn_abstclick(object sender, EventArgs e)
    {
        FpSpread1.SaveChanges();
        if (FpSpread1.Sheets[0].RowCount > 0)
        {
            //for (int col = 6; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
            //{
            int colnewvlaue = Convert.ToInt32(ViewState["colcountnewvalue"]);
            for (int col = colnewvlaue; col < FpSpread1.Sheets[0].ColumnCount; col = col + 2)
            {
                string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text);
                if (s == "A")
                {
                    for (i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                    {

                        FpSpread1.Sheets[0].Cells[i, col].Value = 1;
                        FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;

                    }
                }
            }
        }
        //cb_abst.Checked=false;
        //cb_present.Checked = false;
        //od.Checked = false;
    }
    protected void FpSpread1_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string r = FpSpread1.Sheets[0].ActiveRow.ToString();
            string j = FpSpread1.Sheets[0].ActiveColumn.ToString();
            int k = Convert.ToInt32(j);

            int a = Convert.ToInt32(r);
            int b = Convert.ToInt32(j);

            if (r.Trim() != "")
            {
                if (Convert.ToInt32(r) == 0)
                {
                    if (r.Trim() != "" && j.Trim() != "")
                    {
                        if (FpSpread1.Sheets[0].RowCount > 0)
                        {
                            int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[a, b].Value);
                            if (checkval == 0)
                            {
                                string headervalue = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, b].Text);


                                for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                                {
                                    int checkvalue = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, b].Value);
                                    int checkvalue1 = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, b].Value);
                                    if (headervalue.Trim() == "P")
                                    {
                                        FpSpread1.Sheets[0].Cells[i, b].Value = 1;
                                        FpSpread1.Sheets[0].Cells[i, b + 1].Value = 0;
                                    }
                                    if (headervalue.Trim() == "A")
                                    {
                                        FpSpread1.Sheets[0].Cells[i, b].Value = 1;
                                        FpSpread1.Sheets[0].Cells[i, b - 1].Value = 0;

                                    }
                                }
                            }
                            //if (checkval == 1)
                            //{
                            //    for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                            //    {
                            //        FpSpread1.Sheets[0].Cells[i, b].Value = 0;
                            //    }
                            //}
                        }
                    }
                }
                else
                {
                    string headervalue = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, Convert.ToInt32(j)].Text);

                    if (headervalue.Trim() == "P")
                    {
                        FpSpread1.Sheets[0].Cells[a, b + 1].Value = 0;
                        FpSpread1.Sheets[0].Cells[a, b].Value = 0;
                    }
                    if (headervalue.Trim() == "A")
                    {
                        FpSpread1.Sheets[0].Cells[a, b - 1].Value = 0;
                        FpSpread1.Sheets[0].Cells[a, b].Value = 0;

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
}