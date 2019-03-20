using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Configuration;

public partial class Gps_tracking_status : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet main = new DataSet();
    DataSet route = new DataSet();
    DataSet datecalculate = new DataSet();
    ArrayList arrcount = new ArrayList();
    Boolean Cellclick;
    string vech_values = "";
    string route_values = "";
    string vech_all = "";
    string route_all = "";
    string datecalc = "";
    string PageLogOut = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("TransportIndex"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/TransportMod/TransportIndex.aspx");
                return;
            }
        }
        if (!IsPostBack)
        {
            FpSpread1.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 9;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
            FpSpread1.CommandBar.Visible = false;
            FpSpread2.Visible = false;

            FpSpread2.Sheets[0].RowHeader.Visible = false;
            FpSpread2.Sheets[0].AutoPostBack = true;
            FpSpread2.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread2.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].DefaultStyle.Font.Bold = false;
            FpSpread2.CommandBar.Visible = false;
            bindvehicle();
        }

        spreadexcel1.Visible = false;
        spreadpdf1.Visible = false;
    }

    public void bindvehicle()
    {
        try
        {
            string vehicle = "select * from vehicle_master order by len(veh_id), Veh_ID";
            ds = da.select_method_wo_parameter(vehicle, "text");
            int incre_veh = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int count = 0; count < ds.Tables[0].Rows.Count; count++)
                {
                    incre_veh++;
                    System.Web.UI.WebControls.ListItem list_vehicle_id = new System.Web.UI.WebControls.ListItem();
                    list_vehicle_id.Text = (ds.Tables[0].Rows[count]["Veh_ID"].ToString());
                    cbl_Vechicle.Items.Add(list_vehicle_id);
                    cbl_Vechicle.Items[incre_veh - 1].Selected = true;
                }

                for (int i = 0; i < cbl_Vechicle.Items.Count; i++)
                {
                    cbl_Vechicle.Items[i].Selected = true;
                    txt_Vechicle.Text = "Vehicle(" + (cbl_Vechicle.Items.Count) + ")";
                    if (vech_values == "")
                    {
                        vech_values = cbl_Vechicle.Items[i].Text.ToString();
                    }
                    else
                    {
                        vech_values = vech_values + "','" + cbl_Vechicle.Items[i].Text;
                    }
                }
            }
            else
            {

            }
        }
        catch (Exception ex)
        { }
    }

    public void bindroute()
    {
        try
        {
            int count_items = 0;
            string routecount = "select distinct r.Route_ID from routemaster r,vehicle_master v where r.Route_id=v.Route and v.Veh_Id in('" + vech_values + "') ";
            route = da.select_method_wo_parameter(routecount, "text");
            cb1_Route.Items.Clear();
            cb_Route.Checked = false;
            if (route.Tables[0].Rows.Count > 0)
            {
                cb1_Route.DataSource = route;
                cb1_Route.DataTextField = "Route_ID";
                cb1_Route.DataBind();

                for (int i = 0; i < cb1_Route.Items.Count; i++)
                {
                    txt_Route.Text = "Route(" + (cb1_Route.Items.Count) + ")";
                    cb1_Route.Items[i].Selected = true;
                    if (cb1_Route.Items[i].Selected == true)
                    {
                        count_items += 1;
                    }
                    if (cb1_Route.Items.Count == count_items)
                    {
                        cb_Route.Checked = true;
                    }
                }
            }

            else
            {

            }
        }
        catch (Exception ex)
        { }
    }

    protected void cbl_Vehicle_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int vech_count = 0;
            vech_values = "";
            txt_Vechicle.Text = "--Select--";
            for (int i = 0; i < cbl_Vechicle.Items.Count; i++)
            {
                if (cbl_Vechicle.Items[i].Selected == true)
                {
                    vech_count = vech_count + 1;
                    txt_Vechicle.Text = "Vehicle(" + vech_count.ToString() + ")";
                    if (vech_values == "")
                    {
                        vech_values = cbl_Vechicle.Items[i].Text.ToString();
                    }
                    else
                    {
                        vech_values = vech_values + "','" + cbl_Vechicle.Items[i].Text;

                    }

                    //txt_Route.Text = "Route(" + cb1_Route.Items.Count + ")";

                }
                if (cbl_Vechicle.Items[i].Selected == false)
                {
                    txt_Route.Text = "--Select--";
                }
            }
        }
        catch (Exception ex)
        { }
        bindroute();
    }

    protected void cb_Vehicle_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            vech_values = "";
            if (cb_Vechicle.Checked == true)
            {
                for (int i = 0; i < cbl_Vechicle.Items.Count; i++)
                {
                    cbl_Vechicle.Items[i].Selected = true;
                    txt_Vechicle.Text = "Vehicle(" + (cbl_Vechicle.Items.Count) + ")";
                    if (vech_values == "")
                    {
                        vech_values = cbl_Vechicle.Items[i].Text.ToString();
                        bindroute();
                    }
                    else
                    {
                        vech_values = vech_values + "','" + cbl_Vechicle.Items[i].Text;
                        bindroute();
                    }

                }
            }
            else
            {
                for (int i = 0; i < cbl_Vechicle.Items.Count; i++)
                {
                    cbl_Vechicle.Items[i].Selected = false;
                }

                txt_Vechicle.Text = "--Select--";
            }

            //txt_Route.Text = "Route(" + cb1_Route.Items.Count + ")";

            if (cb_Vechicle.Checked == false)
            {
                txt_Route.Text = "--Select--";
                vech_values = "";
            }
        }
        catch (Exception ex)
        { }
    }

    protected void cb_Route_Checkedchanged(object sender, EventArgs e)
    {
        try
        {
            route_values = "";
            //txt_Route.Text = "Route(" + cb1_Route.Items.Count + ")";
            if (cb1_Route.Items.Count == 0)
            {
                txt_Route.Text = "--Select--";
                cb_Route.Checked = false;
            }
            if (cb_Route.Checked == true)
            {
                for (int i = 0; i < cb1_Route.Items.Count; i++)
                {
                    cb1_Route.Items[i].Selected = true;
                    txt_Route.Text = "Route(" + cb1_Route.Items.Count + ")";
                    if (route_values == "")
                    {
                        route_values = cb1_Route.Items[i].Text.ToString();
                    }
                    else
                    {
                        route_values = route_values + "," + cb1_Route.Items[i].Text;
                    }
                }
            }
            else
            {
                for (int i = 0; i < cb1_Route.Items.Count; i++)
                {
                    cb1_Route.Items[i].Selected = false;
                }
                txt_Route.Text = " ";
            }

            if (cb_Route.Checked == false)
            {

                txt_Route.Text = "--Select--";
            }

        }
        catch (Exception ex)
        { }
    }

    protected void cbl_Route_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            route_values = "";
            int route_count = 0;
            for (int i = 0; i < cb1_Route.Items.Count; i++)
            {
                if (cb1_Route.Items[i].Selected == true)
                {
                    route_count = route_count + 1;
                    txt_Route.Text = "Route(" + route_count.ToString() + ")";
                    if (route_values == "")
                    {
                        route_values = cb1_Route.Items[i].Text.ToString();
                    }
                    else
                    {
                        route_values = route_values + "," + cb1_Route.Items[i].Text;
                    }
                }
            }
            if (route_count == 0)
            {
                txt_Route.Text = "--Select--";
            }
        }
        catch (Exception ex)
        { }
    }

    public void btncal()
    {
        try
        {
            for (int vech_count = 0; vech_count < cbl_Vechicle.Items.Count; vech_count++)
            {
                if (cbl_Vechicle.Items[vech_count].Selected == true)
                {
                    if (vech_all == "")
                    {
                        vech_all = cbl_Vechicle.Items[vech_count].Text;
                    }
                    else
                    {
                        vech_all = vech_all + "','" + cbl_Vechicle.Items[vech_count].Text;
                    }
                }
            }

            int co = 0;
            for (int route_count = 0; route_count < cb1_Route.Items.Count; route_count++)
            {
                if (cb1_Route.Items[route_count].Selected == true)
                {
                    co++;
                    if (route_all == "")
                    {
                        route_all = cb1_Route.Items[route_count].Text;
                    }
                    else
                    {
                        route_all = route_all + "','" + cb1_Route.Items[route_count].Text;
                    }
                }
            }
        }
        catch (Exception ex)
        { }

    }
    protected void btn_go(object sender, EventArgs e)
    {
        try
        {
            FarPoint.Web.Spread.TextCellType timecell = new FarPoint.Web.Spread.TextCellType();
            btncal();
            Printcontrol.Visible = false;
            FpSpread2.Visible = false;
            string vehicalid = "";
            string routeid = "";
            string currentloc = "";
            int serialno = 1;
            string startstage = "";
            string endstage = "";
            string starttime = "";
            string status = "";
            string endtime = "";
            string val = Convert.ToString(DateTime.Now);
            string[] split = val.Split();
            string time = split[2].ToString();

            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = Color.White;
            style2.BackColor = Color.Teal;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            FpSpread1.Sheets[0].AllowTableCorner = true;
            FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style2);

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColor = Color.Black;
            FpSpread1.Sheets[0].Columns[0].Width = 10;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Vehicle ID ";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColor = Color.Black;
            FpSpread1.Sheets[0].Columns[1].Width = 120;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Route ID";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColor = Color.Black;
            FpSpread1.Sheets[0].Columns[2].Width = 90;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Start Stage";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColor = Color.Black;
            FpSpread1.Sheets[0].Columns[3].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "End Stage";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Border.BorderColor = Color.Black;
            FpSpread1.Sheets[0].Columns[4].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Start Time";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Border.BorderColor = Color.Black;
            FpSpread1.Sheets[0].Columns[5].Width = 90;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "End Time";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Border.BorderColor = Color.Black;
            FpSpread1.Sheets[0].Columns[6].Width = 90;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Current Location";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Border.BorderColor = Color.Black;
            FpSpread1.Sheets[0].Columns[7].Width = 250;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Status";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Border.BorderColor = Color.Black;
            FpSpread1.Sheets[0].Columns[8].Width = 70;
            FpSpread1.Sheets[0].Columns[7].Font.Underline = true;
            FpSpread1.Sheets[0].Columns[7].ForeColor = Color.Blue;
            FpSpread1.Sheets[0].Columns[7].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Columns[0].Locked = true;
            FpSpread1.Sheets[0].Columns[1].Locked = true;
            FpSpread1.Sheets[0].Columns[2].Locked = true;
            FpSpread1.Sheets[0].Columns[3].Locked = true;
            FpSpread1.Sheets[0].Columns[4].Locked = true;
            FpSpread1.Sheets[0].Columns[5].Locked = true;
            FpSpread1.Sheets[0].Columns[6].Locked = true;
            FpSpread1.Sheets[0].Columns[7].Locked = false;
            FpSpread1.Sheets[0].Columns[8].Locked = true;

            string mainquery = "select distinct veh_id,route_id from RouteMaster r,Stage_Master s where r.stage_name = s.Stage_id and route_id in('" + route_all + "')";
            mainquery = mainquery + "select * from VTSGPRSData where VehicleID in( '" + vech_all + "')";
            main = da.select_method_wo_parameter(mainquery, "text");
            FpSpread1.Sheets[0].RowCount = 0;

            if (main.Tables[0].Rows.Count > 0)
            {
                for (int loop = 0; loop < main.Tables[0].Rows.Count; loop++)
                {
                    vehicalid = main.Tables[0].Rows[loop]["veh_id"].ToString();
                    routeid = main.Tables[0].Rows[loop]["route_id"].ToString();
                    DataView dlocation = new DataView();
                    main.Tables[1].DefaultView.RowFilter = "VehicleID in('" + vehicalid + "') ";
                    dlocation = main.Tables[1].DefaultView;

                    if (dlocation.Count > 0)
                    {
                        currentloc = Convert.ToString(dlocation[0]["GoogleLocation"]);
                    }
                    else
                    {
                        currentloc = "Location Not Found";
                    }
                    if (currentloc == "")
                    {
                        currentloc = "Location Not Found";
                    }
                    if (time == "PM")
                    {
                        datecalc = " select r.Route_ID,r.Mdate,r.Stage_Name as stage,Arr_Time,Dep_Time,sess,Veh_ID,s.Stage_id,s.Stage_Name,  s.Address from RouteMaster r,Stage_Master s where str(r.Stage_Name)=str(s.Stage_id) and sess='A' and   Route_ID in ('" + route_all + "')  order by convert(nvarchar(5),Dep_Time,103) asc  ";
                        datecalc = datecalc + "         select r.Route_ID,r.Mdate,r.Stage_Name as stage,Arr_Time,Dep_Time,sess,Veh_ID,s.Stage_id,s.Stage_Name,  s.Address from RouteMaster r,Stage_Master s where str(r.Stage_Name)=str(s.Stage_id) and sess='A' and   Route_ID in ('" + route_all + "') order by convert(nvarchar(5),Arr_Time,103) desc";
                    }
                    else
                    {
                        datecalc = " select r.Route_ID,r.Mdate,r.Stage_Name as stage,Arr_Time,Dep_Time,sess,Veh_ID,s.Stage_id,s.Stage_Name,  s.Address from RouteMaster r,Stage_Master s where str(r.Stage_Name)=str(s.Stage_id) and sess='M' and   Route_ID in ('" + route_all + "')  order by convert(nvarchar(5),Dep_Time,103) asc  ";
                        datecalc = datecalc + "         select r.Route_ID,r.Mdate,r.Stage_Name as stage,Arr_Time,Dep_Time,sess,Veh_ID,s.Stage_id,s.Stage_Name,  s.Address from RouteMaster r,Stage_Master s where str(r.Stage_Name)=str(s.Stage_id) and sess='M' and   Route_ID in ('" + route_all + "') order by convert(nvarchar(5),Dep_Time,103) desc";
                    }

                    datecalculate = da.select_method_wo_parameter(datecalc, "text");
                    if (datecalculate.Tables[0].Rows.Count > 0)
                    {
                        DataView dft = new DataView();
                        datecalculate.Tables[0].DefaultView.RowFilter = "Route_ID in('" + routeid + "') ";

                        dft = datecalculate.Tables[0].DefaultView;
                        startstage = dft[0]["Stage_Name"].ToString();
                        starttime = dft[0]["Dep_Time"].ToString();

                        DataView endview = new DataView();
                        datecalculate.Tables[1].DefaultView.RowFilter = "Route_ID in('" + routeid + "') ";
                        endview = datecalculate.Tables[1].DefaultView;
                        endstage = endview[0]["Stage_Name"].ToString();
                        endtime = endview[0]["Arr_Time"].ToString();
                        if (endtime == "Halt")
                        {
                            endtime = endview[1]["Arr_Time"].ToString();
                        }
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = serialno.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = vehicalid.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = routeid.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = startstage.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = endstage.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].CellType = timecell;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = starttime.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].CellType = timecell;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = endtime.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = currentloc.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        status = "Halt";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = status.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        string statusquery = "   select Digitalinput1status from VTSGPRSData where VehicleID='" + vehicalid + "'";
                        DataSet statusds = new DataSet();
                        statusds = da.select_method_wo_parameter(statusquery, "text");
                        if (statusds.Tables[0].Rows.Count > 0)
                        {
                            if (status.Trim() != "Running")
                            {
                                status = statusds.Tables[0].Rows[0]["Digitalinput1status"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = status.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else
                            {
                                status = "Halt";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = status.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }

                        serialno++;
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        FpSpread1.Visible = true;
                        spreadexcel1.Visible = true;
                        spreadpdf1.Visible = true;
                    }
                    else
                    {
                        IblError.Text = "No Records Found";
                        IblError.Visible = true;
                        FpSpread1.Visible = false;
                    }
                }
            }
            else
            {
                IblError.Text = "No Records Found";
                IblError.Visible = true;
                FpSpread1.Visible = false;
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void FSpread1_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            Cellclick = true;
        }
        catch (Exception ex)
        { }

    }
    protected void FSpread1_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (Cellclick == true)
            {


                modelpopsetting.Show();
                FpSpread1.Visible = true;

                FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.DoubleCellType speed1 = new FarPoint.Web.Spread.DoubleCellType();
                Panelshow.Visible = true;

                spreadexcel1.Visible = false;
                spreadpdf1.Visible = false;
                FpSpread2.Visible = true;
                string query = "";
                string activerow = "";
                string activecol = "";
                double speed = 0;
                string vehid = "";
                int sno = 1;
                DataSet dcal = new DataSet();
                FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                style2.Font.Size = 13;
                style2.Font.Name = "Book Antiqua";
                style2.Font.Bold = true;
                style2.HorizontalAlign = HorizontalAlign.Center;
                style2.ForeColor = Color.White;
                style2.BackColor = Color.Teal;
                FpSpread2.Sheets[0].ColumnCount = 10;
                FpSpread2.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style2);
                FpSpread2.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
                FpSpread2.Sheets[0].AllowTableCorner = true;
                FpSpread2.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style2);
                FpSpread2.Sheets[0].AutoPostBack = true;
                FpSpread2.Sheets[0].RowCount = 0;
                activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
                activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
                string vechicle = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                string route = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                string course = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                FpSpread2.Sheets[0].ColumnHeader.RowCount = 2;
                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColor = Color.Black;
                FpSpread2.Sheets[0].Columns[0].Width = 10;

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Vehicle ID ";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColor = Color.Black;
                FpSpread2.Sheets[0].Columns[1].Visible = false;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Route ID";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColor = Color.Black;
                FpSpread2.Sheets[0].Columns[2].Visible = false;
                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Start Time";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColor = Color.Black;
                FpSpread2.Sheets[0].Columns[3].Width = 90;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "End Time";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Border.BorderColor = Color.Black;
                FpSpread2.Sheets[0].Columns[4].Width = 80;
                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Stage";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Border.BorderColor = Color.Black;
                FpSpread2.Sheets[0].Columns[5].Width = 150;
                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(1, 0, 1, 8);
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Speed";

                FpSpread2.Sheets[0].Columns[6].Width = 90;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total No of Student";
                FpSpread2.Sheets[0].Columns[7].Width = 150;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Text = "";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 9].Text = "";
                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 1, 3);
                FpSpread2.Sheets[0].ColumnHeader.Cells[1, 7].Text = "Male";
                FpSpread2.Sheets[0].Columns[8].Width = 150;
                FpSpread2.Sheets[0].ColumnHeader.Cells[1, 8].Text = "Female";
                FpSpread2.Sheets[0].Columns[9].Width = 150;
                FpSpread2.Sheets[0].ColumnHeader.Cells[1, 9].Text = "Staff";
                FpSpread2.Sheets[0].Columns[0].Locked = true;
                FpSpread2.Sheets[0].Columns[1].Locked = true;
                FpSpread2.Sheets[0].Columns[2].Locked = true;
                FpSpread2.Sheets[0].Columns[3].Locked = true;
                FpSpread2.Sheets[0].Columns[4].Locked = true;


                string currenttime = Convert.ToString(DateTime.Now);
                string[] split = currenttime.Split();  // date  time am/pm
                string timenow = split[2].ToString();
                string curtime = split[1].ToString();

                if (timenow.Trim() == "AM")
                {
                    query = "select r.Route_ID,r.Mdate,r.Stage_Name as stage,Arr_Time,Dep_Time,sess,Veh_ID,s.Stage_id,s.Stage_Name,s.Address from RouteMaster r,Stage_Master s where str(r.Stage_Name)=str(s.Stage_id) and sess='M' and Route_ID in('" + route + "')  order by convert(nvarchar(5),Dep_Time,103) asc   ";
                }
                else
                {
                    query = "select r.Route_ID,r.Mdate,r.Stage_Name as stage,Arr_Time,Dep_Time,sess,Veh_ID,s.Stage_id,s.Stage_Name,s.Address from RouteMaster r,Stage_Master s where str(r.Stage_Name)=str(s.Stage_id) and sess='A' and Route_ID in('" + route + "')  order by convert(nvarchar(5),Dep_Time,103) asc  ";
                }


                dcal = da.select_method_wo_parameter(query, "text");
                for (int temp = 0; temp < dcal.Tables[0].Rows.Count; temp++)
                {
                    string routid = dcal.Tables[0].Rows[temp]["Route_ID"].ToString();
                    vehid = dcal.Tables[0].Rows[temp]["Veh_ID"].ToString();
                    string stastage = dcal.Tables[0].Rows[temp]["Stage_Name"].ToString();
                    string enddingtime = dcal.Tables[0].Rows[temp]["Dep_Time"].ToString();
                    string starttime = dcal.Tables[0].Rows[temp]["Arr_Time"].ToString();
                    string stageid = dcal.Tables[0].Rows[temp]["Stage_id"].ToString();


                    FpSpread2.Sheets[0].RowCount++;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Tag = vehid.ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Tag);
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = routid.ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].CellType = txtcell;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = starttime.ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].CellType = txtcell;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = enddingtime.ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = stastage.ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;

                    //string tottab = " select COUNT(*) as tot from Registration r,applyn a where Boarding = '" + stageid + "' and VehID ='" + vehid + "'";
                    //DataSet spreadtot = new DataSet();
                    //spreadtot = da.select_method_wo_parameter(tottab, "text");
                    //if (spreadtot.Tables[0].Rows.Count > 0)
                    //{
                    //    string totcount = spreadtot.Tables[0].Rows[0]["tot"].ToString();

                    //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Text = totcount.ToString();
                    //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                    //}
                    //else
                    //{
                    //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Text = "";
                    //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                    //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].v

                    //}

                    string maletab = " select COUNT(*) as male from Registration r,applyn a where r.App_No = a.app_no and Boarding = '" + stageid + "' and VehID ='" + vehid + "' and sex = 0";
                    DataSet spreadmale = new DataSet();
                    spreadmale = da.select_method_wo_parameter(maletab, "text");
                    if (spreadmale.Tables[0].Rows.Count > 0)
                    {
                        string malecount = spreadmale.Tables[0].Rows[0]["male"].ToString();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Text = malecount.ToString();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                    }
                    else
                    {
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Text = "";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                    }


                    string femaletab = " select COUNT(*) as female from Registration r,applyn a where r.App_No = a.app_no and Boarding = '" + stageid + "' and VehID ='" + vehid + "' and sex = 1";
                    DataSet spreadfemale = new DataSet();
                    spreadfemale = da.select_method_wo_parameter(femaletab, "text");

                    if (spreadfemale.Tables[0].Rows.Count > 0)
                    {

                        string femalecount = spreadfemale.Tables[0].Rows[0]["female"].ToString();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = femalecount.ToString();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                    }
                    else
                    {
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = "";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                    }


                    string stafftab = "select COUNT(*) as staffcount from staffmaster m where Boarding = '" + stageid + "' and VehID = '" + vehid + "'  ";
                    DataSet spreadstaff = new DataSet();
                    spreadstaff = da.select_method_wo_parameter(stafftab, "text");

                    if (spreadstaff.Tables[0].Rows.Count > 0)
                    {
                        string staffcount = spreadstaff.Tables[0].Rows[0]["staffcount"].ToString();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Text = staffcount.ToString();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                    }
                    else
                    {
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Text = "";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                    }

                    FpSpread2.Sheets[0].Columns[6].Visible = false;

                    sno++;
                    string stage = "select GPSFixstatus,Speed from VTSGPRSData  where VehicleID ='" + vehid + "'";
                    DataSet dstage = new DataSet();
                    string gpstage = "";
                    dstage = da.select_method_wo_parameter(stage, "Text");

                    if (dstage.Tables[0].Rows.Count > 0)
                    {
                        gpstage = dstage.Tables[0].Rows[0]["GPSFixstatus"].ToString();
                        speed = Convert.ToDouble(dstage.Tables[0].Rows[0]["Speed"]);
                    }
                    else
                    {
                        gpstage = "";
                        //speed = "Speed Not Found";
                    }

                    string status = "";
                    ArrayList add = new ArrayList();
                    if (dstage.Tables[0].Rows.Count > 0)
                    {
                        stage = stage + "            select distinct address from RouteMaster where Route_ID='" + routid + "' ";
                        dstage = da.select_method_wo_parameter(stage, "Text");
                        for (int b = 0; b < dstage.Tables[1].Rows.Count; b++)
                        {
                            status = dstage.Tables[1].Rows[b]["address"].ToString();
                            add.Add(status);
                        }
                    }
                    else
                    {

                    }

                    if (add.Contains(gpstage))
                    {
                        for (int search = 0; search < FpSpread2.Sheets[0].RowCount; search++)
                        {
                            if (stastage == gpstage)
                            {
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].BackColor = Color.Khaki;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].BackColor = Color.Khaki;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].BackColor = Color.Khaki;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].BackColor = Color.Khaki;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].BackColor = Color.Khaki;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].BackColor = Color.Khaki;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].BackColor = Color.Khaki;
                                Error.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        //Error.Text = "Stage Not Found";
                        //Error.Visible = true;                     

                    }

                    if (add.Contains(gpstage))
                    {
                        for (int search = 0; search < FpSpread2.Sheets[0].RowCount; search++)
                        {
                            if (stastage == gpstage)
                            {
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].CellType = speed1;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(speed);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].CellType = speed1;
                            }
                        }
                    }
                    else
                    {
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = "";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
                string count = "select COUNT(*) as count from Registration r,applyn a where r.App_No = a.app_no and VehID = '" + vehid + "' ";
                DataSet dstot = new DataSet();
                dstot = da.select_method_wo_parameter(count, "text");
                Label1.Text = dstot.Tables[0].Rows[0]["count"].ToString();

                string female = "select COUNT(*) as count from Registration r,applyn a where r.App_No = a.app_no and VehID = '" + vehid + "'and sex = 1";
                DataSet dsfemale = new DataSet();
                dstot = da.select_method_wo_parameter(female, "text");
                Label3.Text = dstot.Tables[0].Rows[0]["count"].ToString();
                lblmale.Text = "Male";
                Label5.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + ":";
                lblstaff.Text = "Staff";
                Label6.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + ":";
                lblfemale.Text = "Female";
                Label7.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + ":";
                Totalstudent.Text = "Total No of Student ";
                Label8.Text = ":";
                string male = "select COUNT(*) as count from Registration r,applyn a where r.App_No = a.app_no and VehID = '" + vehid + "'and sex = 0";
                DataSet dsmale = new DataSet();
                dstot = da.select_method_wo_parameter(male, "text");
                Label2.Text = dstot.Tables[0].Rows[0]["count"].ToString();


                string staff = "select COUNT(*) as count from staffmaster m where VehID = '" + vehid + "'";
                DataSet dsstaff = new DataSet();
                dstot = da.select_method_wo_parameter(staff, "text");
                Label4.Text = dstot.Tables[0].Rows[0]["count"].ToString();

                FpSpread2.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread2.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread2.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                FpSpread2.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                FpSpread2.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                sno++;
                modelpopsetting.Show();
                Panelshow.Visible = true;

                FpSpread2.Visible = true;
                FpSpread1.Visible = true;
                Label1.Visible = true;
                Label2.Visible = true;
                Label3.Visible = true;
                Label4.Visible = true;
                Totalstudent.Visible = true;
                lblmale.Visible = true;
                lblfemale.Visible = true;
                lblstaff.Visible = true;
                Label5.Visible = true;
                Label6.Visible = true;
                Label7.Visible = true;
                Label8.Visible = true;
                FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;

            }
            else
            {
                FpSpread2.Visible = false;
            }
        }
        catch (Exception ex)
        { }
    }
    protected void Logout_btn_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch (Exception ex)
        { }
    }

    protected void closepanel(object sender, EventArgs e)
    {
        FpSpread2.Visible = false;
        //spreadexcel2.Visible = false;
        //spreadpdf2.Visible = false;
        spreadexcel1.Visible = true;
        spreadpdf1.Visible = true;
        FpSpread1.Visible = true;
    }


    protected void spreadexcel1_click(object sender, EventArgs e)
    {
        try
        {
            da.printexcelreport(FpSpread1, "GpsTrackingSystem");
        }
        catch (Exception ex)
        { }
    }

    //protected void spreadexcel2_click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        da.printexcelreport(FpSpread2, "GpsTrackingSystem");
    //    }
    //    catch (Exception ex)
    //    { }
    //}

    //protected void pdf2_click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        FpSpread2.Visible = true;
    //        string date = "@" + "Date :" + System.DateTime.Now.ToString("dd/MM/yyy") + "@";
    //        string pagename = "Gps_tracking_status.aspx";
    //        string degreedetails = "Gps Tracking System" + date;
    //        Printcontrol.loadspreaddetails(FpSpread2, pagename, degreedetails);
    //        Printcontrol.Visible = true;
    //    }
    //    catch (Exception ex)
    //    { }
    //}

    protected void spreadpdf1_click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = true;
            string date = "@" + "Date :" + System.DateTime.Now.ToString("dd/MM/yyy") + "@";
            string pagename = "Gps_tracking_status.aspx";
            string degreedetails = "Gps Tracking System" + date;
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        { }
    }

}