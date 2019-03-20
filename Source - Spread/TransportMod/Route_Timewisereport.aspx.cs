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

public partial class Route_Timewisereport : System.Web.UI.Page
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
            FpSpread1.Sheets[0].ColumnCount = 5;
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
                    chklst_Vechicle.Items.Add(list_vehicle_id);
                    //chklst_Vechicle.Items[incre_veh - 1].Selected = true;
                }

                //for (int i = 0; i < chklst_Vechicle.Items.Count; i++)
                //{
                //    chklst_Vechicle.Items[i].Selected = true;
                //    txt_Vechicle.Text = "Vehicle(" + (chklst_Vechicle.Items.Count) + ")";
                //    if (vech_values == "")
                //    {
                //        vech_values = chklst_Vechicle.Items[i].Text.ToString();
                //    }
                //    else
                //    {
                //        vech_values = vech_values + "','" + chklst_Vechicle.Items[i].Text;
                //    }
                //}
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
            chklst_Route.Items.Clear();
            chk_Route.Checked = false;
            if (route.Tables[0].Rows.Count > 0)
            {
                chklst_Route.DataSource = route;
                chklst_Route.DataTextField = "Route_ID";
                chklst_Route.DataBind();
                for (int i = 0; i < chklst_Route.Items.Count; i++)
                {
                    txt_Route.Text = "Route(" + (chklst_Route.Items.Count) + ")";
                    chklst_Route.Items[i].Selected = true;
                    if (chklst_Route.Items[i].Selected == true)
                    {
                        count_items += 1;
                    }
                    if (chklst_Route.Items.Count == count_items)
                    {
                        chk_Route.Checked = true;
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

    protected void chklstvehicle_selected(object sender, EventArgs e)
    {
        try
        {
            Panelshow.Visible = false;
            modelpopsetting.Hide();
            Labelheader.Visible = false;
            lbl_routeid.Visible = false;
            lbl_routename.Visible = false;
            lbl_rpt.Visible = false;
            txt_rpt.Visible = false;
            spreadexcel2.Visible = false;
            spreadpdf2.Visible = false;
            FpSpread1.Visible = false;
            spreadexcel1.Visible = false;
            spreadpdf1.Visible = false;
            lbl_rptname.Visible = false;
            txt_name.Visible = false;
            int vech_count = 0;
            vech_values = "";
            txt_Vechicle.Text = "--Select--";
            for (int i = 0; i < chklst_Vechicle.Items.Count; i++)
            {
                if (chklst_Vechicle.Items[i].Selected == true)
                {
                    vech_count = vech_count + 1;
                    txt_Vechicle.Text = "Vehicle(" + vech_count.ToString() + ")";
                    if (vech_values == "")
                    {
                        vech_values = chklst_Vechicle.Items[i].Text.ToString();
                    }
                    else
                    {
                        vech_values = vech_values + "','" + chklst_Vechicle.Items[i].Text;
                    }
                }
                else
                {
                    chk_Vechicle.Checked = false;
                }
            }
        }
        catch (Exception ex)
        { }
        bindroute();
    }

    protected void chk_Vechicle_checkedchanged(object sender, EventArgs e)
    {
        try
        {
            vech_values = "";
            if (chk_Vechicle.Checked == true)
            {
                for (int i = 0; i < chklst_Vechicle.Items.Count; i++)
                {
                    chklst_Vechicle.Items[i].Selected = true;
                    txt_Vechicle.Text = "Vehicle(" + (chklst_Vechicle.Items.Count) + ")";
                    if (vech_values == "")
                    {
                        vech_values = chklst_Vechicle.Items[i].Text.ToString();
                        bindroute();
                    }
                    else
                    {
                        vech_values = vech_values + "','" + chklst_Vechicle.Items[i].Text;
                        bindroute();
                    }
                }
            }
            else
            {
                for (int i = 0; i < chklst_Vechicle.Items.Count; i++)
                {
                    chklst_Vechicle.Items[i].Selected = false;
                }
                chklst_Route.Items.Clear();
                txt_Vechicle.Text = "--Select--";
                txt_Route.Text = "--Select--";
                chk_Route.Checked = false;
            }

        }
        catch (Exception ex)
        { }
    }

    protected void chk_Route_Checkedchanged(object sender, EventArgs e)
    {
        try
        {
            route_values = "";
            if (chklst_Route.Items.Count == 0)
            {
                txt_Route.Text = "--Select--";
                chk_Route.Checked = false;
            }
            if (chk_Route.Checked == true)
            {
                for (int i = 0; i < chklst_Route.Items.Count; i++)
                {
                    chklst_Route.Items[i].Selected = true;
                    txt_Route.Text = "Route(" + chklst_Route.Items.Count + ")";
                    if (route_values == "")
                    {
                        route_values = chklst_Route.Items[i].Text.ToString();
                    }
                    else
                    {
                        route_values = route_values + "," + chklst_Route.Items[i].Text;
                    }
                }
            }
            else
            {
                for (int i = 0; i < chklst_Route.Items.Count; i++)
                {
                    chklst_Route.Items[i].Selected = false;
                }
                txt_Route.Text = " ";
            }
            if (chk_Route.Checked == false)
            {
                txt_Route.Text = "--Select--";
            }
        }
        catch (Exception ex)
        { }
    }

    protected void chklst_Route_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Panelshow.Visible = false;
            modelpopsetting.Hide();
            Labelheader.Visible = false;
            lbl_routeid.Visible = false;
            lbl_routename.Visible = false;
            lbl_rpt.Visible = false;
            txt_rpt.Visible = false;
            spreadexcel2.Visible = false;
            spreadpdf2.Visible = false;
            FpSpread1.Visible = false;
            spreadexcel1.Visible = false;
            spreadpdf1.Visible = false;
            lbl_rptname.Visible = false;
            txt_name.Visible = false;
            route_values = "";
            int route_count = 0;
            for (int i = 0; i < chklst_Route.Items.Count; i++)
            {
                if (chklst_Route.Items[i].Selected == true)
                {
                    route_count = route_count + 1;
                    txt_Route.Text = "Route(" + route_count.ToString() + ")";
                    if (route_values == "")
                    {
                        route_values = chklst_Route.Items[i].Text.ToString();
                    }
                    else
                    {
                        route_values = route_values + "," + chklst_Route.Items[i].Text;
                    }
                }
                else
                {
                    chk_Route.Checked = false;
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
            for (int vech_count = 0; vech_count < chklst_Vechicle.Items.Count; vech_count++)
            {
                if (chklst_Vechicle.Items[vech_count].Selected == true)
                {
                    if (vech_all == "")
                    {
                        vech_all = chklst_Vechicle.Items[vech_count].Text;
                    }
                    else
                    {
                        vech_all = vech_all + "','" + chklst_Vechicle.Items[vech_count].Text;
                    }
                }
            }
            int co = 0;
            for (int route_count = 0; route_count < chklst_Route.Items.Count; route_count++)
            {
                if (chklst_Route.Items[route_count].Selected == true)
                {
                    co++;
                    if (route_all == "")
                    {
                        route_all = chklst_Route.Items[route_count].Text;
                    }
                    else
                    {
                        route_all = route_all + "','" + chklst_Route.Items[route_count].Text;
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
            Printcontrol.Visible = false;
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
            FpSpread1.Sheets[0].Rows.Count = 0;
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
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Boarding Point";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColor = Color.Black;
            FpSpread1.Sheets[0].Columns[3].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Ending Point";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Border.BorderColor = Color.Black;
            FpSpread1.Sheets[0].Columns[4].Width = 200;
            FpSpread1.Sheets[0].Columns[0].Locked = true;
            FpSpread1.Sheets[0].Columns[1].Locked = true;
            FpSpread1.Sheets[0].Columns[2].Locked = true;
            FpSpread1.Sheets[0].Columns[3].Locked = true;
            FpSpread1.Sheets[0].Columns[4].Locked = true;
            string mainquery = "select distinct veh_id,route_id from RouteMaster r,Stage_Master s where r.stage_name = s.Stage_id and route_id in('" + route_all + "')";
            mainquery = mainquery + "select * from VTSGPRSData where VehicleID in( '" + vech_all + "')";
            main = da.select_method_wo_parameter(mainquery, "text");

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
                        datecalc = " select r.Route_ID,r.Mdate,r.Stage_Name as stage,Arr_Time,Dep_Time,sess,Veh_ID,s.Stage_id,s.Stage_Name,  s.Address from RouteMaster r,Stage_Master s where str(r.Stage_Name)=str(s.Stage_id) and sess='M' and   Route_ID in ('" + route_all + "')  order by convert(nvarchar(5),Dep_Time,103) asc  ";
                        datecalc = datecalc + "         select r.Route_ID,r.Mdate,r.Stage_Name as stage,Arr_Time,Dep_Time,sess,Veh_ID,s.Stage_id,s.Stage_Name,  s.Address from RouteMaster r,Stage_Master s where str(r.Stage_Name)=str(s.Stage_id) and sess='M' and   Route_ID in ('" + route_all + "') order by convert(nvarchar(5),Dep_Time,103) desc";

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

                        status = "Halt";
                        //string statusquery = "   select Digitalinput1status from VTSGPRSData where VehicleID='" + vehicalid + "'";
                        //DataSet statusds = new DataSet();
                        //statusds = da.select_method_wo_parameter(statusquery, "text");
                        ////if (statusds.Tables[0].Rows.Count > 0)
                        ////{
                        ////    if (status.Trim() != "Running")
                        ////    {
                        ////        status = statusds.Tables[0].Rows[0]["Digitalinput1status"].ToString();
                        ////        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = status.ToString();
                        ////        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        ////    }
                        ////    else
                        ////    {
                        ////        status = "Halt";
                        ////        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = status.ToString();
                        ////        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        ////    }
                        ////}

                        serialno++;
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        FpSpread1.Visible = true;
                        spreadexcel1.Visible = true;
                        spreadpdf1.Visible = true;
                        lbl_rptname.Visible = true;
                        txt_name.Visible = true;
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
            IblError.Visible = true;
            IblError.Text = ex.ToString();
        }
    }

    protected void closepanel(object sender, EventArgs e)
    {
        FpSpread2.Visible = false;
        spreadexcel1.Visible = true;
        spreadpdf1.Visible = true;
        FpSpread1.Visible = true;
        lbl_routename.Visible = true;

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
                lbl_errmsg.Visible = false;
                lblerr.Visible = false;
                Labelheader.Visible = true;
                lbl_rpt.Visible = true;
                txt_rpt.Visible = true;
                spreadpdf2.Visible = true;
                spreadexcel2.Visible = true;
                Panelshow.Height = 500;
                modelpopsetting.Show();
                FpSpread1.Visible = true;
                Panelshow.Visible = true;
                spreadexcel1.Visible = true;
                spreadpdf1.Visible = true;
                FpSpread2.Visible = true;
                lbl_routename.Visible = true;
                lbl_routeid.Visible = true;
                string query = "";
                string activerow = "";
                string activecol = "";
                DataSet dcal = new DataSet();
                FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.DoubleCellType speed1 = new FarPoint.Web.Spread.DoubleCellType();
                FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                style2.Font.Size = 13;
                style2.Font.Name = "Book Antiqua";
                style2.Font.Bold = true;
                style2.HorizontalAlign = HorizontalAlign.Center;
                style2.ForeColor = Color.Purple;
                style2.BackColor = ColorTranslator.FromHtml("#58D3F7");
                FpSpread2.Sheets[0].ColumnCount = 2;
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
                string stage = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
                string course = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "BOARDING POINT";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColor = Color.Black;
                FpSpread2.Sheets[0].Columns[0].Width = 200;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "TIME ";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColor = Color.Black;
                FpSpread2.Sheets[0].Columns[1].Width = 100;
                FpSpread2.Sheets[0].Columns[0].Locked = true;
                FpSpread2.Sheets[0].Columns[1].Locked = true;
                string currenttime = Convert.ToString(DateTime.Now);
                string[] split = currenttime.Split();  // date  time am/pm
                string timenow = split[2].ToString();
                string curtime = split[1].ToString();

                lbl_routeid.Text = "  " + route.ToString() + " : ";
                lbl_routename.Text = course.ToString();
                FpSpread2.Sheets[0].RowCount++;
                FpSpread2.Sheets[0].Cells[0, 0].Text = "MORNING TIME";
                FpSpread2.Sheets[0].SpanModel.Add(0, 0, 1, 2);
                FpSpread2.Sheets[0].Cells[0, 0].ForeColor = Color.Brown;
                FpSpread2.Sheets[0].Cells[0, 0].Font.Bold = true;
                FpSpread2.Sheets[0].Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Cells[0, 0].BackColor = ColorTranslator.FromHtml("#CEECF5");
                query = "select r.Route_ID,r.Mdate,r.Stage_Name as stage,Arr_Time,Dep_Time,sess,Veh_ID,s.Stage_id,s.Stage_Name,s.Address from RouteMaster r,Stage_Master s where str(r.Stage_Name)=str(s.Stage_id) and sess='M' and Route_ID in('" + route + "')  order by convert(nvarchar(5),Dep_Time,103) asc   ";

                dcal = da.select_method_wo_parameter(query, "text");
                for (int temp = 0; temp < dcal.Tables[0].Rows.Count; temp++)
                {
                    string stastage = dcal.Tables[0].Rows[temp]["Stage_Name"].ToString();
                    string starttime = dcal.Tables[0].Rows[temp]["Arr_Time"].ToString();
                    if (starttime == "Halt" || starttime == "-")
                    {
                        starttime = dcal.Tables[0].Rows[temp]["Dep_Time"].ToString();
                    }
                    FpSpread2.Sheets[0].RowCount++;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = stastage.ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#CEECF5");
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = txtcell;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = starttime.ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].BackColor = ColorTranslator.FromHtml("#CEECF5");
                }
                FpSpread2.Sheets[0].RowCount++;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = "EVENING TIME";
                FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 0, 1, 2);
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#CEECF5");
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.Brown;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                query = "select r.Route_ID,r.Mdate,r.Stage_Name as stage,Arr_Time,Dep_Time,sess,Veh_ID,s.Stage_id,s.Stage_Name,s.Address from RouteMaster r,Stage_Master s where str(r.Stage_Name)=str(s.Stage_id) and sess='A' and Route_ID in('" + route + "')  order by convert(nvarchar(5),Dep_Time,103) asc  ";

                dcal = da.select_method_wo_parameter(query, "text");
                for (int temp = 0; temp < dcal.Tables[0].Rows.Count; temp++)
                {
                    string stastage = dcal.Tables[0].Rows[temp]["Stage_Name"].ToString();
                    string starttime1 = dcal.Tables[0].Rows[temp]["Arr_Time"].ToString();
                    if (starttime1 == "-" || starttime1 == "Halt")
                    {
                        starttime1 = dcal.Tables[0].Rows[temp]["Dep_Time"].ToString();
                    }
                    FpSpread2.Sheets[0].RowCount++;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = stastage.ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#CEECF5");
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = txtcell;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = starttime1.ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].BackColor = ColorTranslator.FromHtml("#CEECF5");
                }

                FpSpread2.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread2.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                FpSpread2.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                modelpopsetting.Show();
                Panelshow.Visible = true;
                FpSpread2.Visible = true;
                FpSpread1.Visible = true;
                FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                FpSpread2.Width = 500;
                FpSpread2.Height = 400;
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
            IblError.Visible = true;
            IblError.Text = ex.ToString();
        }
    }
    protected void btn_spreadexcel1(object sender, EventArgs e)
    {
        try
        {
            string rptname = txt_name.Text;
            if (rptname != "")
            {

                da.printexcelreport(FpSpread1, rptname);
            }
            else
            {
                lbl_errmsg.Text = "Please Enter The Report Name";
                lbl_errmsg.Visible = true;
            }
        }
        catch (Exception ex)
        { }
    }
    protected void btn_spreadpdf1(object sender, EventArgs e)
    {
        try
        {
            lbl_errmsg.Visible = false;
            modelpopsetting.Hide();
            FpSpread2.Visible = false;
            string pagename = "Route_Timewisereport.aspx";
            string degreedetails = "Route & Timewise Report";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        { }
    }
    protected void btn_spreadexcel2(object sender, EventArgs e)
    {
        try
        {
            modelpopsetting.Show();
            FpSpread2.Visible = true;
            string name = txt_rpt.Text;
            if (name != "")
            {
                da.printexcelreport(FpSpread2, name);
            }
            else
            {
                lblerr.Text = "Please Enter The Report Name";
                lblerr.Visible = true;

            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_pdf2(object sender, EventArgs e)
    {
        try
        {
            modelpopsetting.Hide();
            FpSpread2.Visible = false;
            lblerr.Visible = false;
            int activerow = FpSpread1.Sheets[0].ActiveRow;
            int activecol = FpSpread1.Sheets[0].ActiveColumn;
            string veh = FpSpread1.Sheets[0].Cells[activerow, 1].Text;
            string routename = FpSpread1.Sheets[0].Cells[activerow, 2].Text;
            string boarding = FpSpread1.Sheets[0].Cells[activerow, 3].Text;

            string date = "@" + "VEHICLE ID" + " : " + veh + " @ " + "ROUTE" + " : " + routename + "@" + "BOARDING POINT" + " : " + boarding;
            string pagename = "Route_Timewisereport.aspx";
            string degreedetails = "Route & Timewise Report" + date;
            Printcontrol.loadspreaddetails(FpSpread2, pagename, degreedetails);
            Printcontrol.Visible = true;
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
}