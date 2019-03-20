using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections;
using System.Drawing;

public partial class Fual_Consumption : System.Web.UI.Page
{
    public SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    string vech_values = string.Empty;
    string type_values = string.Empty;
    string route_values = string.Empty;
    string stage_values = string.Empty;
    string event_values = string.Empty;
    string vech_values1 = string.Empty;
    string route_values1 = string.Empty;
    string stage_values1 = string.Empty;
    string event_values1 = string.Empty;
    string collegecode = string.Empty;
    string usercode = string.Empty;
    string caption = "", fee_code = "", fee_amt = "", header_id = "", semval = "";
    string sql = string.Empty;
    DataSet ds = new DataSet();
    DataSet ds_expense = new DataSet();
    DAccess2 obj = new DAccess2();
    Hashtable hastab = new Hashtable();
    Hashtable hat = new Hashtable();
    string vechileid = "";
    DateTime date;
    string typeofexpense = "";
    string amount = "";
    string expensekm = "";
    string expextrskm = "";
    string duration = "";
    DateTime frmdate;
    DateTime todate;
    string remarks = "";
    string fuel = "0";
    DAccess2 d2 = new DAccess2();
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
        txt_vech.Attributes.Add("readonly", "readonly");
        //txtfrm_date.Attributes.Add("readonly", "readonly");
        //txtend_date.Attributes.Add("readonly", "readonly");

        Fp_Fuel.Sheets[0].AutoPostBack = true;
        Fp_Fuel.CommandBar.Visible = false;
        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        style.Font.Size = 10;
        style.Font.Bold = true;
        Fp_Fuel.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        Fp_Fuel.Sheets[0].AllowTableCorner = true;
        Fp_Fuel.Sheets[0].RowHeader.Visible = false;

        Fp_Fuel.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        //FpSpread1.Sheets[0]

        Fp_Fuel.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        Fp_Fuel.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

        Fp_Fuel.Sheets[0].DefaultColumnWidth = 50;
        Fp_Fuel.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        Fp_Fuel.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        Fp_Fuel.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

        Fp_Fuel.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        Fp_Fuel.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        Fp_Fuel.Sheets[0].DefaultStyle.Font.Bold = false;
        Fp_Fuel.SheetCorner.Cells[0, 0].Font.Bold = true;

        if (!IsPostBack)
        {
            con.Close();
            con.Open();

            SqlCommand cmd_vehicle_id = new SqlCommand("select * from vehicle_master order by len(veh_id), Veh_ID", con);
            SqlDataReader rdr_vehicle_id = cmd_vehicle_id.ExecuteReader();

            int incre_veh = 0;
            while (rdr_vehicle_id.Read())
            {
                if (rdr_vehicle_id.HasRows == true)
                {
                    incre_veh++;
                    System.Web.UI.WebControls.ListItem list_vehicle_id = new System.Web.UI.WebControls.ListItem();

                    list_vehicle_id.Text = (rdr_vehicle_id["Veh_ID"].ToString());

                    vehiclechecklist.Items.Add(list_vehicle_id);
                    vehiclechecklist.Items[incre_veh - 1].Selected = true;

                }
            }

            txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Jan", "1"));
            //ddlMonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Feb", "2"));
            //ddlMonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Mar", "3"));
            //ddlMonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Apr", "4"));
            //ddlMonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("May", "5"));
            //ddlMonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("Jun", "6"));
            //ddlMonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jul", "7"));
            //ddlMonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Aug", "8"));
            //ddlMonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Sep", "9"));
            //ddlMonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Oct", "10"));
            //ddlMonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Nov", "11"));
            //ddlMonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Dec", "12"));

            //DropDownList1.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Jan", "1"));
            //DropDownList1.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Feb", "2"));
            //DropDownList1.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Mar", "3"));
            //DropDownList1.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Apr", "4"));
            //DropDownList1.Items.Insert(4, new System.Web.UI.WebControls.ListItem("May", "5"));
            //DropDownList1.Items.Insert(5, new System.Web.UI.WebControls.ListItem("Jun", "6"));
            //DropDownList1.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jul", "7"));
            //DropDownList1.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Aug", "8"));
            //DropDownList1.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Sep", "9"));
            //DropDownList1.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Oct", "10"));
            //DropDownList1.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Nov", "11"));
            //DropDownList1.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Dec", "12"));

            //int year;
            //year = Convert.ToInt16(DateTime.Today.Year);
            //ddlYear.Items.Clear();
            //DropDownList2.Items.Clear();
            //for (int l = 0; l <= 20; l++)
            //{

            //    ddlYear.Items.Add(Convert.ToString(year - l));
            //    DropDownList2.Items.Add(Convert.ToString(year - l));

            //}

            vehiclechecklist_SelectedIndexChanged(sender, e);
            btnMainGo_Click(sender, e);
        }
    }

    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }

    protected void vehiclecheck_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            vech_values = "";
            if (vehiclecheck.Checked == true)
            {
                for (int i = 0; i < vehiclechecklist.Items.Count; i++)
                {
                    vehiclechecklist.Items[i].Selected = true;
                    txt_vech.Text = "Vehicle(" + (vehiclechecklist.Items.Count) + ")";
                    if (vech_values == "")
                    {
                        vech_values = vehiclechecklist.Items[i].Text.ToString();
                    }
                    else
                    {
                        vech_values = vech_values + "','" + vehiclechecklist.Items[i].Text;
                    }
                }
            }
            else
            {
                for (int i = 0; i < vehiclechecklist.Items.Count; i++)
                {
                    vehiclechecklist.Items[i].Selected = false;
                    txt_vech.Text = "--Select--";
                }
            }
        }

        catch (Exception ex)
        {
            throw ex;
        }
        //Bind_Routes1();
    }

    protected void vehiclechecklist_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            vech_values = "";
            int vech_count = 0;
            for (int i = 0; i < vehiclechecklist.Items.Count; i++)
            {
                if (vehiclechecklist.Items[i].Selected == true)
                {
                    vech_count = vech_count + 1;
                    txt_vech.Text = "Vehicle(" + vech_count.ToString() + ")";
                    if (vech_values == "")
                    {
                        vech_values = vehiclechecklist.Items[i].Text.ToString();
                    }
                    else
                    {
                        vech_values = vech_values + "','" + vehiclechecklist.Items[i].Text;
                    }
                }
            }

            if (vech_count == 0)
            {
                txt_vech.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        //Bind_Routes1();

    }

    protected void btnMainGo_Click(object sender, EventArgs e)
    {
        string vech_all = string.Empty;
        string sess_all = string.Empty;
        string route_all = string.Empty;
        string stage_all = string.Empty;
        string display_all = string.Empty;
        string stage_header = string.Empty;
        string route_header = string.Empty;

        errmsg.Visible = false;
        for (int vech_count = 0; vech_count < vehiclechecklist.Items.Count; vech_count++)
        {
            if (vehiclechecklist.Items[vech_count].Selected == true)
            {
                if (vech_all == "")
                {
                    vech_all = vehiclechecklist.Items[vech_count].Text;
                }
                else
                {
                    vech_all = vech_all + "','" + vehiclechecklist.Items[vech_count].Text;
                }
            }
        }

        con.Close();
        con.Open();

        string[] spitfrom = txtfromdate.Text.Split('/');
        DateTime dtfrom = Convert.ToDateTime(spitfrom[1] + '/' + spitfrom[0] + '/' + spitfrom[2]);

        string[] spilttodate = txttodate.Text.Split('/');
        DateTime dtto = Convert.ToDateTime(spilttodate[1] + '/' + spilttodate[0] + '/' + spilttodate[2]);

        //   SqlCommand cmd_km_count = new SqlCommand("select vehicle_type,vehicle_id,sum(travel_km) as km_total,sum(fuel) as fu_total from vehicle_usage  where  vehicle_id in('" + vech_all + "') and month(date) between '" + ddlMonth.SelectedValue.ToString() + "' and '" + DropDownList1.SelectedValue.ToString() + "' and year(date) between '" + ddlYear.SelectedItem.ToString() + "' and '" + DropDownList2.SelectedItem.ToString() + "' group by vehicle_type,vehicle_id order by Vehicle_type,vehicle_id", con);
        SqlCommand cmd_km_count = new SqlCommand();
        if (Chkdate.Checked == true)
        {
            //Modified by strinath 1262014
            // cmd_km_count = new SqlCommand("select vehicle_type,vehicle_id,sum(travel_km) as km_total,sum(fuel) as fu_total from vehicle_usage  where  vehicle_id in('" + vech_all + "') and date between '" + dtfrom.ToString() + "' and '" + dtto.ToString() + "' group by vehicle_type,vehicle_id order by Vehicle_type,vehicle_id", con);

            //modified by prabha 
            //existing
            //cmd_km_count = new SqlCommand("select vehicle_type,vehicle_id,v.Reg_No,sum(travel_km) as km_total,sum(fuel) as fu_total from vehicle_usage vu,Vehicle_Master v  where v.Veh_ID=vu.Vehicle_Id  and vehicle_id in('" + vech_all + "') and date between '" + dtfrom.ToString() + "' and '" + dtto.ToString() + "' group by vehicle_type,v.Reg_No,vehicle_id order by Vehicle_type,vehicle_id", con);
            //new   
            cmd_km_count = new SqlCommand("select vehicle_type,vehicle_id,v.Reg_No,sum(travel_km) as km_total,sum(fuel) as fu_total from vehicle_usage vu,Vehicle_Master v  where v.Veh_ID=vu.Vehicle_Id  and vehicle_id in('" + vech_all + "') and arrivalpdate between '" + dtfrom.ToString() + "' and '" + dtto.ToString() + "' group by vehicle_type,v.Reg_No,vehicle_id order by Vehicle_type,vehicle_id", con);
        }
        else
        {//Modified by strinath 1262014
            //cmd_km_count = new SqlCommand("select vehicle_type,vehicle_id,sum(travel_km) as km_total,sum(fuel) as fu_total from vehicle_usage  where  vehicle_id in('" + vech_all + "') group by vehicle_type,vehicle_id order by Vehicle_type,vehicle_id", con);
            cmd_km_count = new SqlCommand("select vehicle_type,vehicle_id,v.Reg_No,sum(travel_km) as km_total,sum(fuel) as fu_total from vehicle_usage vu,Vehicle_Master v  where v.Veh_ID=vu.Vehicle_Id  and  vehicle_id in('" + vech_all + "') group by vehicle_type,vehicle_id,v.Reg_No order by Vehicle_type,vehicle_id", con);
        }

        SqlDataAdapter ad_km_count = new SqlDataAdapter(cmd_km_count);
        DataTable dt_km_count = new DataTable();
        ad_km_count.Fill(dt_km_count);

        if (dt_km_count.Rows.Count > 0)
        {
            Fp_Fuel.Visible = true;
            btnprintmaster.Visible = true;//added by SRinath 8/10/2013
            Fp_Fuel.Sheets[0].RowCount = 0;
            Fp_Fuel.Sheets[0].ColumnCount = 9;//Modified by SRinath 12/6/2014
            Fp_Fuel.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "S.No";
            Fp_Fuel.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "Vehicle Type";
            Fp_Fuel.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 2].Text = "Vehicle Id";
            Fp_Fuel.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 3].Text = "Driver Name";
            Fp_Fuel.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 4].Text = "Registration No";
            //Fp_Fuel.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 3].Text = "Date";
            Fp_Fuel.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 5].Text = "Mileage";
            Fp_Fuel.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 6].Text = "Total Travel KM";
            Fp_Fuel.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 7].Text = "Total Fuel-(Lt)";
            Fp_Fuel.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 8].Text = "Total Remaining KM";

            Fp_Fuel.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 0].Column.HorizontalAlign = HorizontalAlign.Center;

            Fp_Fuel.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Fp_Fuel.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);

            con.Close();
            con.Open();

            SqlCommand cmd_get_mileage = new SqlCommand("select * from vehicle_master where veh_id in('" + vech_all + "')", con);
            SqlDataAdapter ad_get_mileage = new SqlDataAdapter(cmd_get_mileage);
            DataTable dt_get_mileage = new DataTable();
            ad_get_mileage.Fill(dt_get_mileage);

            if (dt_get_mileage.Rows.Count > 0)
            {
                int sno = 0;

                DataTable dt_remain = new DataTable();

                DataColumn dc = new DataColumn();
                dc.ColumnName = "vehid";
                dt_remain.Columns.Add(dc);

                DataColumn dc_min = new DataColumn();
                dc_min.ColumnName = "totkm";
                dt_remain.Columns.Add(dc_min);

                DataRow dr;

                double tot_remain_km = 0;
                string temp_vehid = string.Empty;

                for (int i = 0; i < dt_km_count.Rows.Count; i++)
                {
                    sno++;
                    string veh_id = dt_km_count.Rows[i]["vehicle_id"].ToString();
                    string veh_type = dt_km_count.Rows[i]["vehicle_type"].ToString();
                    string travel_km = dt_km_count.Rows[i]["km_total"].ToString();
                    string fuel_tot = dt_km_count.Rows[i]["fu_total"].ToString();
                    string regno = dt_km_count.Rows[i]["Reg_No"].ToString();
                    string set_date = "";
                    string drivername = d2.GetFunction("select sm.staff_name from DriverAllotment da,staffmaster sm where sm.staff_code=da.Staff_Code and da.Vehicle_Id in('" + veh_id + "') ");

                    DataView dv_mileage = new DataView();
                    dt_get_mileage.DefaultView.RowFilter = "veh_id='" + veh_id + "'";
                    dv_mileage = dt_get_mileage.DefaultView;

                    string mileage = "0";

                    if (dv_mileage.Count > 0)
                    {
                        mileage = dv_mileage[0]["mileage"].ToString();

                        if (mileage == "")
                        {
                            mileage = "0";
                        }
                    }

                    if (fuel_tot == "")
                    {
                        fuel_tot = "0";
                    }

                    if (temp_vehid == "" && dt_km_count.Rows.Count == 1)
                    {
                        tot_remain_km = tot_remain_km + ((Convert.ToDouble(mileage) * Convert.ToDouble(fuel_tot)) - Convert.ToDouble(travel_km));

                        dr = dt_remain.NewRow();
                        dr["vehid"] = veh_id;
                        dr["totkm"] = tot_remain_km.ToString();
                        dt_remain.Rows.Add(dr);
                    }
                    else if ((temp_vehid != "" && temp_vehid != veh_id))
                    {
                        dr = dt_remain.NewRow();
                        dr["vehid"] = temp_vehid;
                        dr["totkm"] = tot_remain_km.ToString();
                        dt_remain.Rows.Add(dr);

                        tot_remain_km = 0;
                    }

                    double plan_km = Convert.ToDouble(mileage) * Convert.ToDouble(fuel_tot);
                    double actual_km = 0;
                    double.TryParse(Convert.ToString(travel_km), out actual_km);


                    double remain_km = plan_km - actual_km;
                    tot_remain_km = tot_remain_km + remain_km;

                    if (temp_vehid != "" && dt_km_count.Rows.Count != 1 && i == dt_km_count.Rows.Count - 1)
                    {
                        temp_vehid = veh_id;

                        dr = dt_remain.NewRow();
                        dr["vehid"] = veh_id;
                        dr["totkm"] = tot_remain_km.ToString();
                        dt_remain.Rows.Add(dr);
                    }

                    double remain_km_x = Math.Round(remain_km, 2);

                    temp_vehid = veh_id;

                    Fp_Fuel.Sheets[0].RowCount = Convert.ToInt32(Fp_Fuel.Sheets[0].RowCount) + 1;

                    Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                    Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 1].Text = veh_type;
                    Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                    Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 2].Text = veh_id;
                    Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                    //Added by Rajasekar 22/10/2018
                    Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 3].Text = drivername;
                    Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                    //=================//

                    //Added by srinath 12/6/2014
                    Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 4].Text = regno;
                    Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                    Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 5].Text = mileage;
                    Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                    Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 6].Text = travel_km;
                    Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;

                    Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 7].Text = fuel_tot;
                    Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;

                    Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 8].Text = remain_km_x.ToString();
                    Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;

                }
                Fp_Fuel.Sheets[0].PageSize = Fp_Fuel.Sheets[0].RowCount;
                Fp_Fuel.Visible = true;
                btnprintmaster.Visible = true;//added by SRinath 8/10/2013
                Fp_Fuel.SaveChanges();

                FarPoint.Web.Spread.LabelCellType lbl_cell = new FarPoint.Web.Spread.LabelCellType();

                lbl_cell.CssClass = "blinkytext";

                for (int k = 0; k < Fp_Fuel.Sheets[0].RowCount; k++)
                {
                    string vehi_id = Fp_Fuel.Sheets[0].Cells[k, 2].Text;

                    DataView dv_remain = new DataView();
                    dt_remain.DefaultView.RowFilter = "vehid='" + vehi_id + "'";
                    dv_remain = dt_remain.DefaultView;

                    if (dv_remain.Count > 0)
                    {
                        Fp_Fuel.Sheets[0].Cells[k, 6].CellType = lbl_cell;
                        if (dv_remain.Count > 0)
                        {
                            double tot_km = Math.Round(Convert.ToDouble(dv_remain[0]["totkm"].ToString()), 2);

                            Fp_Fuel.Sheets[0].Cells[k, 8].Text = tot_km.ToString();
                        }
                        else
                        {
                            Fp_Fuel.Sheets[0].Cells[k, 8].Text = "0";
                        }

                        Fp_Fuel.Sheets[0].Cells[k, 8].HorizontalAlign = HorizontalAlign.Center;
                        Fp_Fuel.Sheets[0].Cells[k, 8].VerticalAlign = VerticalAlign.Middle;


                    }
                }

            }
        }
        else
        {
            errmsg.Text = "No data found";
            errmsg.Visible = true;
            Fp_Fuel.Sheets[0].RowCount = 0;
            errmsg.Font.Bold = true;
            Fp_Fuel.Visible = false;
            btnprintmaster.Visible = false;//added by SRinath 8/10/2013
        }
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        if (Fp_Fuel.Sheets[0].RowCount > 0)
        {
            for (int i = 0; i < Fp_Fuel.Rows.Count; i++)
            {
                if (Convert.ToDouble(Fp_Fuel.Sheets[0].Cells[i, 6].Text.ToString()) <= 10)
                {
                    if (Fp_Fuel.Sheets[0].Cells[i, 2].BackColor == Color.Salmon)
                    {
                        Fp_Fuel.Sheets[0].Cells[i, 2].BackColor = Color.Yellow;
                        Fp_Fuel.Sheets[0].Cells[i, 3].BackColor = Color.Yellow;
                        Fp_Fuel.Sheets[0].Cells[i, 4].BackColor = Color.Yellow;
                        Fp_Fuel.Sheets[0].Cells[i, 5].BackColor = Color.Yellow;
                        Fp_Fuel.Sheets[0].Cells[i, 6].BackColor = Color.Yellow;

                    }
                    else
                    {
                        Fp_Fuel.Sheets[0].Cells[i, 2].BackColor = Color.Salmon;
                        Fp_Fuel.Sheets[0].Cells[i, 3].BackColor = Color.Salmon;
                        Fp_Fuel.Sheets[0].Cells[i, 4].BackColor = Color.Salmon;
                        Fp_Fuel.Sheets[0].Cells[i, 5].BackColor = Color.Salmon;
                        Fp_Fuel.Sheets[0].Cells[i, 6].BackColor = Color.Salmon;
                    }
                }
                else
                {
                    Fp_Fuel.Sheets[0].Rows[i].BackColor = Color.White;
                }
            }
            Fp_Fuel.SaveChanges();
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        Session["column_header_row_count"] = 1;
        string degreedetails = "Fuel Consumption Report";
        string pagename = "Fuel_Consumption.aspx";
        Printcontrol.loadspreaddetails(Fp_Fuel, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

    protected void txtfromdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtfromdate.Text != "")
            {
                string[] spitfrom = txtfromdate.Text.Split('/');
                DateTime dtfrom = Convert.ToDateTime(spitfrom[1] + '/' + spitfrom[0] + '/' + spitfrom[2]);

                string[] spilttodate = txttodate.Text.Split('/');
                DateTime dtto = Convert.ToDateTime(spilttodate[1] + '/' + spilttodate[0] + '/' + spilttodate[2]);
                if (dtto < dtfrom)
                {
                    txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    errmsg.Visible = true;
                    errmsg.Text = "To Date Must Be Greater Than From Date";
                }

            }
            else
            {
                txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }

        }
        catch (Exception ex)
        {
            txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            errmsg.Visible = true;
            errmsg.Text = "Please Enter Valid From Date";
        }
    }

    protected void txttodate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txttodate.Text != "")
            {
                string[] spitfrom = txtfromdate.Text.Split('/');
                DateTime dtfrom = Convert.ToDateTime(spitfrom[1] + '/' + spitfrom[0] + '/' + spitfrom[2]);

                string[] spilttodate = txttodate.Text.Split('/');
                DateTime dtto = Convert.ToDateTime(spilttodate[1] + '/' + spilttodate[0] + '/' + spilttodate[2]);
                if (dtto < dtfrom)
                {
                    txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    errmsg.Visible = true;
                    errmsg.Text = "To Date Must Be Greater Than From Date";
                }

            }
            else
            {
                txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }

        }
        catch (Exception ex)
        {
            txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            errmsg.Visible = true;
            errmsg.Text = "Please Enter Valid From Date";
        }
    }


}