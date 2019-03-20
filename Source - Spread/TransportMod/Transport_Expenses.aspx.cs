using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class Transport_Expenses : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    Hashtable ht = new Hashtable();
    DAccess2 da = new DAccess2();
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
        lblerr.Visible = false;
        if (!IsPostBack)
        {
            setLabelText();
            lblerr.Visible = false;
            college();
            vehicleid();
            vehicletype();
            txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtfromdate.Attributes.Add("readonly", "readonly");
            txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txttodate.Attributes.Add("readonly", "readonly");
            lblrptname.Visible = false;
            btnxl.Visible = false;
            txtexcelname.Visible = false;
            btnprintmaster.Visible = false;
            txtcollege.Text = "--Select--";
        }
    }

    public void college()
    {
        try
        {

            string college = "select college_code,collname from collinfo ";
            if (college != "")
            {
                ds = da.select_method(college, ht, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cheklist_clg.DataSource = ds;
                    cheklist_clg.DataTextField = "collname";
                    cheklist_clg.DataValueField = "college_code";
                    cheklist_clg.DataBind();
                }
            }
        }
        catch
        { }
    }

    public void vehicleid()
    {
        string vehicle_id = "select distinct vehicle_id from Vehicle_Usage";

        if (vehicle_id != "")
        {
            ds = da.select_method(vehicle_id, ht, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int [] vehcarr=new int[ds.Tables[0].Rows.Count];
                string vehicleId = string.Empty;
                int i =0;
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    int vehid = 0;
                    vehicle_id = Convert.ToString(row["vehicle_id"]);
                    Int32.TryParse(vehicle_id,out vehid);
                    vehcarr[i] = vehid;
                    i++;
                }

                Array.Sort(vehcarr);

                foreach (int item in vehcarr)
                {
                    ListItem li = new ListItem(item.ToString(), item.ToString());
                    ddlvehicleid.Items.Add(li);
                }
                ddlvehicleid.Items.Insert(0, "All");
            }
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    ddlvehicleid.DataSource = ds;
            //    ddlvehicleid.DataTextField = "vehicle_id";
            //    ddlvehicleid.DataValueField = "vehicle_id";
            //    ddlvehicleid.DataBind();
            //    ddlvehicleid.Items.Insert(0, "All");
            //}
        }
    }

    public void vehicletype()
    {
        string vehicle_id = "select distinct Vehicle_Type from Vehicle_Usage where Vehicle_Type<>''";
        if (vehicle_id != "")
        {
            ds = da.select_method(vehicle_id, ht, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlvehicletype.DataSource = ds;
                ddlvehicletype.DataTextField = "Vehicle_Type";
                ddlvehicletype.DataValueField = "Vehicle_Type";
                ddlvehicletype.DataBind();
                ddlvehicletype.Items.Insert(0, "All");
            }
        }
    }

    protected void logout_btn_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    //btngo_Click  modified by prabha 22 dec 2017
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            if (cheklist_clg.Text == "")
            {
                lblinvalid.Text = "Please Select College";
                lblinvalid.Visible = true;
                lblerr.Visible = false;
                lblError.Visible = false;
                Fpexpenses.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                Fpexpenses.Visible = false;
            }
            else
            {
                string firstdate = txtfromdate.Text.ToString();
                string[] split = firstdate.Split(new Char[] { '/' });
                string date = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
                string todate = txttodate.Text.ToString();
                string[] split1 = todate.Split(new Char[] { '/' });
                string date1 = split1[1].ToString() + "-" + split1[0].ToString() + "-" + split1[2].ToString();
                Fpexpenses.Sheets[0].RowCount = 0;
                Fpexpenses.Sheets[0].RowHeader.Visible = false;
                Fpexpenses.Sheets[0].AutoPostBack = false;
                Fpexpenses.Height = 400;
                Fpexpenses.Width = 950;
                lblinvalid.Visible = false;

                int sno = 0;
                Fpexpenses.Sheets[0].ColumnCount = 13;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
                Fpexpenses.Sheets[0].ColumnHeader.Columns[0].Font.Size = FontUnit.Medium;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[0].Font.Bold = true;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[0].Font.Name = "Book Antiqua";

                Fpexpenses.Sheets[0].ColumnHeader.Columns[1].Label = "Vehicle ID";
                Fpexpenses.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[1].Font.Size = FontUnit.Medium;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[1].Font.Bold = true;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[1].Font.Name = "Book Antiqua";

                Fpexpenses.Sheets[0].ColumnHeader.Columns[2].Label = " Register Number";
                Fpexpenses.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[2].Font.Size = FontUnit.Medium;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[2].Font.Bold = true;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[2].Font.Name = "Book Antiqua";

                Fpexpenses.Sheets[0].ColumnHeader.Columns[3].Label = "Vehicle Type";
                Fpexpenses.Sheets[0].ColumnHeader.Columns[3].Font.Size = FontUnit.Medium;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[3].Font.Bold = true;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[3].Font.Name = "Book Antiqua";

                Fpexpenses.Sheets[0].ColumnHeader.Columns[4].Label = "Registration Year";
                Fpexpenses.Sheets[0].ColumnHeader.Columns[4].Font.Size = FontUnit.Medium;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[4].Font.Bold = true;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[4].Font.Name = "Book Antiqua";

                Fpexpenses.Sheets[0].ColumnHeader.Columns[5].Label = "Seating Capacity";
                Fpexpenses.Sheets[0].ColumnHeader.Columns[5].Font.Size = FontUnit.Medium;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[5].Font.Bold = true;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[5].Font.Name = "Book Antiqua";


                Fpexpenses.Sheets[0].ColumnHeader.Columns[6].Label = "KMS";
                Fpexpenses.Sheets[0].ColumnHeader.Columns[6].Font.Size = FontUnit.Medium;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[6].Font.Bold = true;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[6].Font.Name = "Book Antiqua";

                Fpexpenses.Sheets[0].ColumnHeader.Columns[7].Label = "Diesel Qty";
                Fpexpenses.Sheets[0].ColumnHeader.Columns[7].Font.Size = FontUnit.Medium;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[7].Font.Bold = true;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[7].Font.Name = "Book Antiqua";

                Fpexpenses.Sheets[0].ColumnHeader.Columns[8].Label = "Mileage";
                Fpexpenses.Sheets[0].ColumnHeader.Columns[8].Font.Size = FontUnit.Medium;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[8].Font.Bold = true;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[8].Font.Name = "Book Antiqua";
                
                Fpexpenses.Sheets[0].ColumnHeader.Columns[9].Label = "Diesel Exp";
                Fpexpenses.Sheets[0].ColumnHeader.Columns[9].Font.Size = FontUnit.Medium;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[9].Font.Bold = true;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[9].Font.Name = "Book Antiqua";

                Fpexpenses.Sheets[0].ColumnHeader.Columns[10].Label = "Repair Exp";
                Fpexpenses.Sheets[0].ColumnHeader.Columns[10].Font.Size = FontUnit.Medium;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[10].Font.Bold = true;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[10].Font.Name = "Book Antiqua";

                Fpexpenses.Sheets[0].ColumnHeader.Columns[11].Label = "Total Exp";
                Fpexpenses.Sheets[0].ColumnHeader.Columns[11].Font.Size = FontUnit.Medium;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[11].Font.Bold = true;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[11].Font.Name = "Book Antiqua";

                Fpexpenses.Sheets[0].ColumnHeader.Columns[12].Label = "Remarks";
                Fpexpenses.Sheets[0].ColumnHeader.Columns[12].Font.Size = FontUnit.Medium;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[12].Font.Bold = true;
                Fpexpenses.Sheets[0].ColumnHeader.Columns[12].Font.Name = "Book Antiqua";

                
                Fpexpenses.Sheets[0].Columns[1].Width = 80;
                Fpexpenses.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                Fpexpenses.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                Fpexpenses.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                Fpexpenses.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Right;
                Fpexpenses.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Right;
                Fpexpenses.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Right;
                Fpexpenses.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Right;
                Fpexpenses.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Right;
                Fpexpenses.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Right;
                Fpexpenses.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Right;
                Fpexpenses.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Right;
                Fpexpenses.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Right;
                Fpexpenses.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Right;
                Fpexpenses.Sheets[0].Columns[0].Locked = true;
                Fpexpenses.Sheets[0].Columns[1].Locked = true;
                Fpexpenses.Sheets[0].Columns[2].Locked = true;
                Fpexpenses.Sheets[0].Columns[3].Locked = true;
                Fpexpenses.Sheets[0].Columns[4].Locked = true;
                Fpexpenses.Sheets[0].Columns[5].Locked = true;
                Fpexpenses.Sheets[0].Columns[6].Locked = true;
                Fpexpenses.Sheets[0].Columns[7].Locked = true;
                Fpexpenses.Sheets[0].Columns[8].Locked = true;
                Fpexpenses.Sheets[0].Columns[9].Locked = true;
                Fpexpenses.Sheets[0].Columns[10].Locked = true;
                Fpexpenses.Sheets[0].Columns[11].Locked = true;
                Fpexpenses.Sheets[0].Columns[12].Locked = false;
                int height = 50;

                Fpexpenses.Visible = true;
                for (int s = 0; s < cheklist_clg.Items.Count; s++)
                {
                    if (cheklist_clg.Items[s].Selected == true)
                    {
                        
                        if (ddlvehicleid.SelectedItem.Text == "All" && ddlvehicletype.SelectedItem.Text == "All")
                        {
                            string strquery = "select distinct vm.Reg_No,(vm.Veh_Type),CONVERT(varchar,vm.Reg_Date,104)as RegisteredDate,vm.TotalNo_Seat as SeatingCapacity,sum(vu.Travel_Km) as Travel_Km,sum(vu.Fuel) as Fuel,round(((cast(sum(vu.Travel_Km)as numeric(10,4))/cast(nullif(sum(vu.Fuel),0)as numeric(10,4)))),2) as overamileage,sum(vu.totalamount) as totalamount,sum(vu.repairamount) as repairamount,vm.Veh_ID   from Vehicle_Usage vu,  Vehicle_Master vm where  vu.Vehicle_Id=vm.Veh_ID and college_code='" + cheklist_clg.Items[s].Value + "'  and vu.Date between '" + date + "' and '" + date1 + "'group by vm.Reg_No,vm.Veh_Type,vm.TotalNo_Seat,vm.Reg_Date,vm.Veh_ID";
                            ds = da.select_method_wo_parameter(strquery, "Text");


                        }
                        if (ddlvehicleid.SelectedItem.Text == "All" && ddlvehicletype.SelectedItem.Text != "All")
                        {
                            string strquery = "select distinct vm.Reg_No,(vm.Veh_Type),CONVERT(varchar,vm.Reg_Date,104)as RegisteredDate ,vm.TotalNo_Seat as SeatingCapacity,sum(vu.Travel_Km) as Travel_Km,sum(vu.Fuel) as fuel,round(((cast(sum(vu.Travel_Km)as numeric(10,4))/cast(nullif(sum(vu.Fuel),0)as numeric(10,4)))),2) as overamileage,sum(vu.totalamount)as totalamount,sum(vu.repairamount) as repairamount,vm.Veh_ID   from Vehicle_Usage vu,  Vehicle_Master vm where  vu.Vehicle_Id=vm.Veh_ID and college_code='" + cheklist_clg.Items[s].Value + "' and vm.Veh_Type='" + ddlvehicletype.SelectedItem.Text.Trim() + "'  and vu.Date between '" + date + "' and '" + date1 + "'group by vm.Reg_No,vm.Veh_Type,vm.TotalNo_Seat,vm.Reg_Date,vm.TotalNo_Seat,vm.Reg_Date,vm.Veh_ID";
                            ds = da.select_method_wo_parameter(strquery, "Text");
                        }
                        if (ddlvehicleid.SelectedItem.Text != "All" && ddlvehicletype.SelectedItem.Text == "All")
                        {
                            string strquery = "select distinct vm.Reg_No,(vm.Veh_Type),CONVERT(varchar,vm.Reg_Date,104)as RegisteredDate ,vm.TotalNo_Seat as SeatingCapacity,sum(vu.Travel_Km) as Travel_Km,sum(vu.Fuel) as Fuel,round(((cast(sum(vu.Travel_Km)as numeric(10,4))/cast(nullif(sum(vu.Fuel),0)as numeric(10,4)))),2) as overamileage,sum(vu.totalamount) as totalamount,sum(vu.repairamount) as repairamount,vm.Veh_ID  from Vehicle_Usage vu,  Vehicle_Master vm where  vu.Vehicle_Id=vm.Veh_ID and college_code='" + cheklist_clg.Items[s].Value + "' and  vu.Vehicle_Id='" + ddlvehicleid.SelectedItem.Text + "' and vu.Date between '" + date + "' and '" + date1 + "'group by vm.Reg_No,vm.Veh_Type,vm.TotalNo_Seat,vm.Reg_Date,vm.Veh_ID";
                            ds = da.select_method_wo_parameter(strquery, "Text");
                        }
                        if (ddlvehicleid.SelectedItem.Text != "All" && ddlvehicletype.SelectedItem.Text != "All")
                        {
                            string strquery = "select distinct vm.Reg_No,(vm.Veh_Type),CONVERT(varchar,vm.Reg_Date,104)as RegisteredDate,vm.TotalNo_Seat as SeatingCapacity,sum(vu.Travel_Km) as Travel_Km,sum (vu.Fuel) as Fuel,round(((cast(sum(vu.Travel_Km)as numeric(10,4))/cast(nullif(sum(vu.Fuel),0)as numeric(10,4)))),2) as overamileage,sum(vu.totalamount) as totalamount,sum(vu.repairamount) as repairamount,vm.Veh_ID   from Vehicle_Usage vu,  Vehicle_Master vm where  vu.Vehicle_Id=vm.Veh_ID and college_code='" + cheklist_clg.Items[s].Value + "' and vu.Vehicle_Id='" + ddlvehicleid.SelectedItem.Text + "' and vu.Vehicle_Type='" + ddlvehicletype.SelectedItem.Text + "' and vu.Date between '" + date + "' and '" + date1 + "'group by vm.Reg_No,vm.Veh_Type,vm.TotalNo_Seat,vm.Reg_Date,vm.Veh_ID";
                            ds = da.select_method_wo_parameter(strquery, "Text");
                        }

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            double totals = 0.0;
                            double total1 = 0.0;
                            double mileage = 0.0;
                            double mileage1 = 0.0;
                            double fuel = 0.0;
                            double fuel1 = 0.0;
                            double tamount = 0.0;
                            double tamount1 = 0.0;
                            double repairamount = 0.0;
                            double repairamount1 = 0.0;
                            string Registredyr = string.Empty;

                            for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                            {
                                if (ds.Tables[0].Rows[a]["Travel_Km"] != DBNull.Value)
                                {
                                    totals = Convert.ToDouble(ds.Tables[0].Rows[a]["Travel_Km"]);
                                    total1 = total1 + totals;
                                }
                                if (ds.Tables[0].Rows[a]["overamileage"] != DBNull.Value)
                                {
                                    mileage = Convert.ToDouble(ds.Tables[0].Rows[a]["overamileage"]);
                                    mileage1 = mileage1 + mileage;
                                }
                                if (ds.Tables[0].Rows[a]["Fuel"] != DBNull.Value)
                                {
                                    fuel = Convert.ToDouble(ds.Tables[0].Rows[a]["Fuel"]);
                                    fuel1 = fuel1 + fuel;
                                }
                                if (ds.Tables[0].Rows[a]["totalamount"] != DBNull.Value)
                                {
                                    tamount = Convert.ToDouble(ds.Tables[0].Rows[a]["totalamount"]);
                                    tamount1 = tamount1 + tamount;
                                }
                                if (ds.Tables[0].Rows[a]["repairamount"] != DBNull.Value)
                                {
                                    repairamount = Convert.ToDouble(ds.Tables[0].Rows[a]["repairamount"]);
                                    repairamount1 = repairamount1 + repairamount;
                                }
                               
                            }
                            int dataset = ds.Tables[0].Rows.Count;

                            ds.Tables[0].Rows.Add(1);
                            ds.Tables[0].Rows[dataset]["Reg_No"] = "";
                            ds.Tables[0].Rows[dataset]["Veh_Type"] = "TOTAL";
                            ds.Tables[0].Rows[dataset]["Travel_Km"] = total1;
                            ds.Tables[0].Rows[dataset]["overamileage"] = mileage1;
                            ds.Tables[0].Rows[dataset]["Fuel"] = fuel1;
                            ds.Tables[0].Rows[dataset]["totalamount"] = tamount1;
                            //Registredyr = Convert.ToString(ds.Tables[0].Rows[dataset]["RegisteredDate"]);
                            //Registredyr = Convert.ToString(Registredyr.Split('.').Last());
                            //ds.Tables[0].Rows[dataset]["RegisteredDate"] = Registredyr;
                            ds.Tables[0].Rows[dataset]["repairamount"] = repairamount1;


                            Fpexpenses.Sheets[0].RowCount++;
                            //string sql = "select collname from collinfo where collname='" + ddlselectcollege.SelectedItem.Text + "' ";
                            //ds = da.select_method_wo_parameter(sql, "text");
                            //string colname = "";
                            //if (ds.Tables[0].Rows.Count > 0)
                            //{
                            //    colname = ds.Tables[0].Rows[0]["collname"].ToString();


                            //}
                            string collname = "";
                            collname = cheklist_clg.Items[s].Text;

                            Fpexpenses.Sheets[0].SpanModel.Add(Fpexpenses.Sheets[0].RowCount - 1, 0, 1, 10);
                            Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, 0].Text = collname;
                            Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, 0].ForeColor = Color.Blue;
                            Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            //rajasekar
                            ArrayList vehicleArr = new ArrayList();
                            int rowvalue1, rowvalue2;
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    
                                    
                                    int row1 = 0;
                                    int row2 = row1 + 1;
                                    
                                    if (i != ds.Tables[0].Rows.Count - 1)
                                    {
                                        for (int v = 0; v < ds.Tables[0].Rows.Count - 2; v++)
                                        {
                                        
                                     
                                                rowvalue1 = Convert.ToInt32(ds.Tables[0].Rows[row1]["Veh_Id"]);
                                                rowvalue2 = Convert.ToInt32(ds.Tables[0].Rows[row2]["Veh_Id"]);
                                                if (!vehicleArr.Contains(rowvalue1))
                                                {
                                                    if (rowvalue1 > rowvalue2)
                                                    {
                                                        if (vehicleArr.Count > 0)
                                                        {
                                                            if (!vehicleArr.Contains(rowvalue2))
                                                            {

                                                               

                                                                row1 = row2;
                                                              
                                                            }
                                                        }
                                                        else
                                                            row1 = row2;

                                                    }
                                                }
                                                else
                                                    row1++;

                                            row2++;
                                     

                                        }
                                    rowvalue1 = Convert.ToInt32(ds.Tables[0].Rows[row1]["Veh_Id"]);
                                    vehicleArr.Add(rowvalue1);
                                     }
                                    else
                                        row1=i;
                        
                                    Fpexpenses.Sheets[0].RowCount++;

                                    Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                    if (i < ds.Tables[0].Rows.Count - 1)
                                    {
                                        sno++;
                                        Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                        Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, 1].Text = sno.ToString();
                                        Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    }


                                    for (int j = 0; j <= ds.Tables[0].Columns.Count - 3; j++)
                                    {
                                        Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, j + 1].Font.Name = "Book Antiqua";
                                        Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, j + 1].Font.Size = FontUnit.Medium;
                                        Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, j + 1].Font.Bold = true;
                                        string value = string.Empty;

                                        //if (j == 4)
                                        //{
                                        //    //value = ds.Tables[0].Rows[i][j].ToString();
                                        //    //value = Convert.ToString(value.Split('.').Last());
                                        //}
                                        if (j == 0)
                                            value = ds.Tables[0].Rows[row1]["Veh_Id"].ToString();
                                        else if (j == 1)
                                            value = ds.Tables[0].Rows[row1]["Reg_No"].ToString();
                                        else if (j == 2)
                                            value = ds.Tables[0].Rows[row1]["Veh_Type"].ToString();
                                        else if (j == 3)
                                        {
                                            value = ds.Tables[0].Rows[row1]["RegisteredDate"].ToString();
                                            value = Convert.ToString(value.Split('.').Last());
                                        }
                                        else if (j == 4)
                                            value = ds.Tables[0].Rows[row1]["SeatingCapacity"].ToString();
                                        else if (j == 5)
                                            value = ds.Tables[0].Rows[row1]["Travel_Km"].ToString();
                                        else if (j == 6)
                                            value = ds.Tables[0].Rows[row1]["Fuel"].ToString();
                                        else if (j == 7)
                                            value = ds.Tables[0].Rows[row1]["overamileage"].ToString();
                                        Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, j + 1].Text = value;

                                    }
                                    double de = 0;
                                    double re = 0;
                                    double totalamt = 0;

                                    for (int k = 8; k < 9; k++)
                                    {
                                        string total = Convert.ToString(ds.Tables[0].Rows[row1]["totalamount"]);
                                        string repair = Convert.ToString(ds.Tables[0].Rows[row1]["repairamount"]);
                                        if (total != "")
                                        {
                                            de = Convert.ToDouble(total);
                                        }
                                        else
                                        {
                                            de = 0;
                                        }
                                        if (repair != "")
                                        {
                                            re = Convert.ToDouble(repair);
                                        }
                                        else
                                        {
                                            re = 0;
                                        }

                                        Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, k + 1].Text = de.ToString();
                                        Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, k + 1].Font.Name = "Book Antiqua";
                                        Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, k + 1].Font.Size = FontUnit.Medium;
                                        Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, k + 1].Font.Bold = true;
                                        Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, k + 2].Text = re.ToString();
                                        Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, k + 2].Font.Name = "Book Antiqua";
                                        Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, k + 2].Font.Size = FontUnit.Medium;
                                        Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, k + 2].Font.Bold = true;
                                        Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, k + 3].Text = Convert.ToString(de + re);
                                        Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, k + 3].Font.Name = "Book Antiqua";
                                        Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, k + 3].Font.Size = FontUnit.Medium;
                                        Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, k + 3].Font.Bold = true;


                                        // Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, k + 2].Text = " Convert.ToString(de + re)";

                                    }

                                }
                            //Fpexpenses.Sheets[0].RowCount++;
                            //Fpexpenses.Sheets[0].SpanModel.Add(0, 0, 1, 11);
                            //Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, 2].Text = "Total";
                            //Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            //Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            //Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                            Fpexpenses.Sheets[0].PageSize = Fpexpenses.Sheets[0].RowCount;
                            lblError.Visible = false;
                            lblinvalid.Visible = false;
                            lblrptname.Visible = true;
                            btnxl.Visible = true;
                            txtexcelname.Visible = true;
                            btnprintmaster.Visible = true;
                            //for (int h = 3; h < Fpexpenses.Sheets[0].Columns.Count - 1; h++)
                            //{

                            //    Double totalamt2 = 0;
                            //    for (int j = 1; j < Fpexpenses.Sheets[0].RowCount - 1; j++)
                            //    {



                            //        string firstvalue = Convert.ToString(Fpexpenses.Sheets[0].GetText(j, h));
                            //        if (firstvalue != "0" && firstvalue != "-")
                            //        {
                            //            if (totalamt2 == 0)
                            //            {
                            //                totalamt2 = Convert.ToDouble(firstvalue);

                            //            }
                            //            else
                            //            {
                            //                totalamt2 = totalamt2 + Convert.ToDouble(firstvalue);
                            //            }
                            //        }


                            //    }
                            //    Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, h].Text = totalamt2.ToString();
                            //    Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, h].Font.Bold = true;
                            //    Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, h].Font.Size = FontUnit.Medium;
                            //    Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, h].Font.Name = "Book Antiqua";
                            //    Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, Fpexpenses.Sheets[0].Columns.Count - 1].Font.Name = "Book Antiqua";
                            //    Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, Fpexpenses.Sheets[0].Columns.Count - 1].Font.Size = FontUnit.Medium;
                            //    Fpexpenses.Sheets[0].Cells[Fpexpenses.Sheets[0].RowCount - 1, Fpexpenses.Sheets[0].Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;
                            //    Fpexpenses.Sheets[0].Columns[9].Locked = false;
                            //    // Fpexpenses.Sheets[0].RowCount++;
                            Fpexpenses.Visible = true;

                        }

                    }

                    //else
                    //{
                    //    lblinvalid.Text = "No Records Found";
                    //    lblinvalid.Visible = true;
                    //    lblerr.Visible = false;
                    //    lblError.Visible = false;
                    //    Fpexpenses.Visible = false;
                    //    lblrptname.Visible = false;
                    //    txtexcelname.Visible = false;
                    //    btnxl.Visible = false;
                    //    btnprintmaster.Visible = false;
                    //}
                    lblError.Visible = false;

                }

                if (Fpexpenses.Sheets[0].RowCount == 0)
                {
                    lblinvalid.Text = "No Records Found";
                    lblinvalid.Visible = true;
                    lblerr.Visible = false;
                    lblError.Visible = false;
                    Fpexpenses.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                }

                //}
            }
        }
        catch
        {
        }
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txtexcelname.Text;
            if (report.ToString().Trim() != "")
            {
                da.printexcelreport(Fpexpenses, report);
                lblerr.Visible = false;
            }
            else
            {
                lblerr.Text = "Please Enter Your Report Name";
                lblerr.Visible = true;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void chekclg_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chekclg.Checked == true)
            {
                for (int i = 0; i < cheklist_clg.Items.Count; i++)
                {

                    cheklist_clg.Items[i].Selected = true;
                    txtcollege.Text = lblselectcollege.Text+"(" + (cheklist_clg.Items.Count) + ")";
                }

            }
            else
            {
                for (int i = 0; i < cheklist_clg.Items.Count; i++)
                {
                    cheklist_clg.Items[i].Selected = false;
                    txtcollege.Text = "---Select---";
                }
            }
        }
        catch
        {
        }
    }

    protected void cheklist_clg_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;

            chekclg.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cheklist_clg.Items.Count; i++)
            {
                if (cheklist_clg.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    build = cheklist_clg.Items[i].Value.ToString();
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

            if (seatcount == cheklist_clg.Items.Count)
            {
                txtcollege.Text = lblselectcollege.Text + "(" + seatcount.ToString() + ")";
                chekclg.Checked = true;
            }
            else if (seatcount == 0)
            {
                txtcollege.Text = "--Select--";
            }
            else
            {
                txtcollege.Text = lblselectcollege.Text + "(" + seatcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        Fpexpenses.SaveChanges();
        string departmentlist = "VEHICLE EXPENSES ABSTRACT ";
        Printcontrol.loadspreaddetails(Fpexpenses, "transport_expenses.aspx", departmentlist);
        Printcontrol.Visible = true;
    }

    protected void txttodate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime dt1 = Convert.ToDateTime(txtfromdate.Text);
            DateTime dt2 = Convert.ToDateTime(txttodate.Text);

            TimeSpan ts = dt2 - dt1;

            int days = ts.Days;
            if (days < 0)
            {
                lblError.Text = "From Date Should Be Less Than To Date";

                lblError.Visible = true;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
            }
            if (dt1 > DateTime.Today)
            {
                lblError.Text = "You Can Not Select From Date Greater Than Today";

                lblError.Visible = true;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
            }
            if (dt2 > DateTime.Today)
            {
                lblError.Text = "You Can Not Select From Date Greater Than Today";

                lblError.Visible = true;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
            }
            lblinvalid.Visible = false;
        }
        catch
        {
        }
    }

    protected void txtfromdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime dt1 = Convert.ToDateTime(txtfromdate.Text);
            DateTime dt2 = Convert.ToDateTime(txttodate.Text);

            TimeSpan ts = dt2 - dt1;

            int days = ts.Days;
            if (days < 0)
            {
                lblError.Text = "From Date Should Be Less Than To Date";

                lblError.Visible = true;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
            }
            if (dt1 > DateTime.Today)
            {
                lblError.Text = "You Can Not Select From Date Greater Than Today";

                lblError.Visible = true;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
            }
            if (dt2 > DateTime.Today)
            {
                lblError.Text = "You Can Not Select From Date Greater Than Today";

                lblError.Visible = true;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
            }
            lblinvalid.Visible = false;
        }
        catch
        {
        }
    }

    protected void ddlvehicletype_SelectedIndexChanged(object sender, EventArgs e)
    {
        string vehicle_id = "";
        Fpexpenses.Visible = false;
        if (ddlvehicletype.SelectedItem.Value != "ALL")
        {
            vehicle_id = "select distinct vehicle_id from Vehicle_Usage where vehicle_Type='" + ddlvehicletype.SelectedItem.Value + "'";
        }
        else
        {
            vehicle_id = "select distinct vehicle_id from Vehicle_Usage";
        }
        if (vehicle_id != "")
        {
            ds = da.select_method(vehicle_id, ht, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlvehicleid.DataSource = ds;
                ddlvehicleid.DataTextField = "vehicle_id";
                ddlvehicleid.DataValueField = "vehicle_id";
                ddlvehicleid.DataBind();
                ddlvehicleid.Items.Insert(0, "All");
            }
        }
    }

    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();
        lbl.Add(lblselectcollege);
        //lbl.Add(lbl_stream);
        //lbl.Add(lbl_course);
        //lbl.Add(lbl_dept);
        //lbl.Add(lbl_sem);
        fields.Add(0);
        // fields.Add(1);
        //fields.Add(2);
        //fields.Add(3);
        //fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
    // last modified 22-10-2016 sudhagar
}