using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Drawing;

public partial class Vechiclevacancyreport : System.Web.UI.Page
{
    string PageLogOut = "";
    int count = 0;
    string usercode = string.Empty;
    DAccess2 d2 = new DAccess2();
    DataSet ds2 = new DataSet();
    string vech_all = string.Empty;
    string sess_all = string.Empty;
    string route_all = string.Empty;
    int vech_count = 0;
    Hashtable hat = new Hashtable();
    int sess_count = 1;
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    string vech_values = string.Empty;
    string route_values = string.Empty;
    string ddl_sess = string.Empty;

    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }

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
            //btnprintmaster.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;

            lblerror1.Visible = false;
            con.Close();
            con.Open();
            lblerror1.Visible = false;
            //lblrptname.Visible = false;
            //txtexcelname.Visible = false;
            //btn_excel.Visible = false;

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

                for (int i = 0; i < vehiclechecklist.Items.Count; i++)
                {
                    vehiclechecklist.Items[i].Selected = true;

                    Vehicleid.Text = "Vehicle(" + (vehiclechecklist.Items.Count) + ")";

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
            con.Close();
            con.Open();
            SqlCommand cmd_route = new SqlCommand("select distinct Route_ID from routemaster ", con);
            SqlDataReader rdr_route = cmd_route.ExecuteReader();
            int route_count = 0;

            while (rdr_route.Read())
            {
                if (rdr_route.HasRows == true)
                {
                    route_count++;
                    System.Web.UI.WebControls.ListItem list_route = new System.Web.UI.WebControls.ListItem();


                    list_route.Text = (rdr_route["Route_ID"].ToString());
                    checkrolist.Items.Add(list_route);

                    checkrolist.Items[route_count - 1].Selected = true;
                }

                for (int i = 0; i < checkrolist.Items.Count; i++)
                {
                    if (checkrolist.Items[i].Selected == true)
                    {
                        // route_count = route_count + 1;
                        txt_route.Text = "Route(" + route_count.ToString() + ")";
                        //txt_route.Text = checkrolist.SelectedItem.ToString();
                        if (route_values == "")
                        {
                            route_values = checkrolist.Items[i].Text.ToString();
                        }
                        else
                        {
                            route_values = route_values + "," + checkrolist.Items[i].Text;
                        }
                    }
                }
            }


        }

    }

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {

        Control cntUpdateBtn = FpSpread1.FindControl("Update");
        Control cntCancelBtn = FpSpread1.FindControl("Cancel");
        Control cntCopyBtn = FpSpread1.FindControl("Copy");
        Control cntCutBtn = FpSpread1.FindControl("Clear");
        Control cntPasteBtn = FpSpread1.FindControl("Paste");
        // Control cntPageNextBtn = FpSpread1.FindControl("Next");
        // Control cntPagePreviousBtn = FpSpread1.FindControl("Prev");
        //Control cntPagePrintBtn = FpSpread1.FindControl("Print");

        if ((cntUpdateBtn != null))
        {

            TableCell tc = (TableCell)cntUpdateBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;

            tr.Cells.Remove(tc);

            tc = (TableCell)cntCancelBtn.Parent;
            tr.Cells.Remove(tc);


            tc = (TableCell)cntCopyBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntCutBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPasteBtn.Parent;
            tr.Cells.Remove(tc);

            //  tc = (TableCell)cntPageNextBtn.Parent;
            //   tr.Cells.Remove(tc);

            //   tc = (TableCell)cntPagePreviousBtn.Parent;
            //   tr.Cells.Remove(tc);

        }

        base.Render(writer);
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
                    Vehicleid.Text = "Vehicle(" + (vehiclechecklist.Items.Count) + ")";
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

                }
                Vehicleid.Text = " ";
            }
        }

        catch (Exception ex)
        {
            throw ex;
        }
        Bind_Routes();
        checkro_CheckedChanged(sender, e);



    }

    void Bind_Routes()
    {
        //if (ddl_session.Text == "Morning")
        //{
        //    ddl_sess = "M";
        //}
        //else if (ddl_session.Text == "Afternoon")
        //{
        //    ddl_sess = "A";
        //}
        //else
        //{
        //    ddl_sess = "M','A";
        //}
        con.Close();
        con.Open();
        int count_items = 0;
        SqlCommand cmd_bind_route = new SqlCommand("select distinct r.Route_ID from routemaster r,vehicle_master v where r.Route_id=v.Route and v.Veh_Id in('" + vech_values + "') ", con);
        SqlDataAdapter ad_bind_route = new SqlDataAdapter(cmd_bind_route);
        DataTable dt_bind_route = new DataTable();
        ad_bind_route.Fill(dt_bind_route);
        checkrolist.Items.Clear();
        if (dt_bind_route.Rows.Count > 0)
        {
            checkrolist.DataSource = dt_bind_route;
            checkrolist.DataTextField = "Route_ID";
            checkrolist.DataBind();

            for (int i = 0; i < checkrolist.Items.Count; i++)
            {
                checkrolist.Items[i].Selected = true;
                if (checkrolist.Items[i].Selected == true)
                {
                    count_items += 1;
                }
                if (checkrolist.Items.Count == count_items)
                {

                    checkro.Checked = true;

                }
            }
        }
    }

    protected void vehiclechecklist_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            vech_values = "";

            for (int i = 0; i < vehiclechecklist.Items.Count; i++)
            {
                if (vehiclechecklist.Items[i].Selected == true)
                {
                    vech_count = vech_count + 1;
                    Vehicleid.Text = "Vehicle(" + vech_count.ToString() + ")";
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
                // Vehicleid.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        Bind_Routes();
        checkro_CheckedChanged(sender, e);


    }

    protected void checkro_CheckedChanged(object sender, EventArgs e)
    {

        try
        {
            route_values = "";
            if (checkro.Checked == true)
            {
                for (int i = 0; i < checkrolist.Items.Count; i++)
                {
                    checkrolist.Items[i].Selected = true;
                    txt_route.Text = "Route(" + checkrolist.Items.Count + ")";

                    if (route_values == "")
                    {
                        route_values = checkrolist.Items[i].Text.ToString();
                    }
                    else
                    {
                        route_values = route_values + "," + checkrolist.Items[i].Text;
                    }
                }
            }
            else
            {
                for (int i = 0; i < checkrolist.Items.Count; i++)
                {
                    checkrolist.Items[i].Selected = false;

                }
                txt_route.Text = " ";
            }

            if (checkrolist.Items.Count == 0)
                txt_route.Text = " ";
        }

        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void checkrolist_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            route_values = "";
            int route_count = 0;
            for (int i = 0; i < checkrolist.Items.Count; i++)
            {
                if (checkrolist.Items[i].Selected == true)
                {
                    route_count = route_count + 1;
                    txt_route.Text = "Route(" + route_count.ToString() + ")";
                    if (route_values == "")
                    {
                        route_values = checkrolist.Items[i].Text.ToString();
                    }
                    else
                    {
                        route_values = route_values + "," + checkrolist.Items[i].Text;
                    }
                }
            }

            if (route_count == 0)
            {
                // txt_route.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void ddl_session_SelectedIndexChanged(object sender, EventArgs e)
    {


        Bind_Routes();
    }

    protected void btnMainGo_Click(object sender, EventArgs e)
    {
        try
        {

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

            int co = 0;
            for (int route_count = 0; route_count < checkrolist.Items.Count; route_count++)
            {
                if (checkrolist.Items[route_count].Selected == true)
                {
                    co++;
                    if (route_all == "")
                    {
                        route_all = checkrolist.Items[route_count].Text;

                    }
                    else
                    {
                        route_all = route_all + "','" + checkrolist.Items[route_count].Text;
                    }
                }
            }

            con.Close();
            con.Open();

            string routes = route_all;
            string[] route = routes.Split(',');

            string vehicles = vech_all;
            string[] vehicle = vehicles.Split(',');

            string cmd_grid_data1 = "select vehid,bus_routeid,boarding,count(stud_name)as studentcount from registration where vehid in('" + vech_all + "')  and bus_routeid in('" + route_all + "') group by boarding,vehid,bus_routeid  select  vehid,bus_routeid,boarding,count(staff_name)  as staffcount from  staffmaster s,stafftrans st,hrdept_master hm where s.staff_code=st.staff_code and st.dept_code=hm.dept_code  and Bus_RouteID is not null and Bus_RouteID<>'' and VehID is not null and  VehID<>''  and Boarding is not null and Boarding<>'' and s.college_code=hm.college_code and s.college_code=" + Session["collegecode"].ToString() + " and s.settled <>1  and s.resign <>1 and  st.latestrec<>0  and Bus_RouteID in('" + route_all + "') and VehID in('" + vech_all + "')   group by boarding,vehid,bus_routeid  select Veh_ID,route,isnull(TotalNo_Seat,0)TotalNo_Seat,Reg_No from vehicle_master where Veh_ID in('" + vech_all + "') and Route in('" + route_all + "') group by Veh_ID,route,TotalNo_Seat,Reg_No select distinct vehid,bus_routeid  from registration where vehid in('" + vech_all + "')  and bus_routeid in('" + route_all + "') ";
            DataSet ds = new DataSet();
            ds = d2.select_method(cmd_grid_data1, hat, "Text");


            DataView dv = new DataView();
            DataTable dat = new DataTable();

            DataTable dat1 = new DataTable();

            DataTable dat2 = new DataTable();

            DataTable dat3 = new DataTable();

            dat = ds.Tables[0];
            dat1 = ds.Tables[1];
            dat2 = ds.Tables[2];
            dat3 = ds.Tables[3];

            if (ds.Tables[2].Rows.Count > 0)
            {
                lblerror.Visible = false;
                lblerror1.Visible = false;
                //lblrptname.Visible = true;
                //txtexcelname.Visible = true;
                //btn_excel.Visible = true;

                FpSpread1.Visible = true;
                //btnprintmaster.Visible = true;
                ddlpagecount.Visible = true;
                lblpages.Visible = true;
                lblerror.Visible = false;

                FpSpread1.Sheets[0].AutoPostBack = true;
                FpSpread1.CommandBar.Visible = true;
                FarPoint.Web.Spread.StyleInfo styles = new FarPoint.Web.Spread.StyleInfo();
                styles.Font.Size = 10;
                styles.Font.Bold = true;
                FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(styles);
                FpSpread1.Sheets[0].AllowTableCorner = true;
                // FpSpread1.Sheets[0].RowHeader.Visible = false;

                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                //FpSpread1.Sheets[0]

                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

                FpSpread1.Sheets[0].DefaultColumnWidth = 50;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
                FpSpread1.SheetCorner.Cells[0, 0].Font.Bold = true;






                FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(styles);
                FpSpread1.Sheets[0].AllowTableCorner = true;

                FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].BackColor = Color.AliceBlue;




                // //span sheetcorner column header rows 
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                //FpSpread1.ActiveSheetView.SheetCornerSpanModel.Add(0, 0, 2, 1);


                FpSpread1.Sheets[0].ColumnCount = 8;
                FpSpread1.Sheets[0].RowCount = 0;


                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread1.ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[6].Locked = true;

                FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                //FpSpread1.Sheets[0].RowHeader.ColumnCount = 1;
                FpSpread1.Sheets[0].ColumnHeader.Visible = true;

                FpSpread1.Sheets[0].Rows.Default.Font.Size = FontUnit.Medium;
                //Color c = FpSpread1.ColumnHeader.DefaultStyle.BackColor;
                //FpSpread1.ActiveSheetView.SheetCorner.DefaultStyle.BackColor = Color.LightCyan;

                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;

                //  FpSpread1.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                //  FpSpread1.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Vehicle ID";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Registration No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Route ID";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 4, 1);

                //FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].Text = "Session";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total Alloted";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 5, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 5].Text = "Students";
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Text = "Staffs";

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Vacancies";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 6, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Travellers";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 1, 2);

                int slno = 0;
                for (int vacancycount = 0; vacancycount < ds.Tables[2].Rows.Count; vacancycount++)
                {
                    lblerror.Visible = false;
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].SheetName = " ";
                    string temp_vech_name = string.Empty;
                    temp_vech_name = dat2.Rows[vacancycount]["Veh_ID"].ToString();

                    string temproute_name = string.Empty;

                    temproute_name = dat2.Rows[vacancycount]["route"].ToString();

                    string regno = dat2.Rows[vacancycount]["reg_no"].ToString();
                    if (temproute_name != "")
                    {
                        dat.DefaultView.RowFilter = " vehid ='" + temp_vech_name + "'"; //and bus_routeid='" + temproute_name + "' ";

                        DataView dv1 = new DataView();
                        dv1 = dat.DefaultView;

                        dat1.DefaultView.RowFilter = " vehid ='" + temp_vech_name + "'";// and bus_routeid='" + temproute_name + "' ";


                        dat2.DefaultView.RowFilter = " veh_id ='" + temp_vech_name + "'";

                        DataView dv2 = new DataView();
                        dv2 = dat1.DefaultView;

                        DataView dv3 = new DataView();
                        dv3 = dat2.DefaultView;

                        string vech_id = "";
                        string route_id = "";
                        int vacanto = 0;
                        int totalsum = 0;
                        if (dv1.Count > 0)
                        {
                            foreach (DataRowView datarowviewUsers in dv1)
                            {
                                vech_id = datarowviewUsers["vehid"].ToString();
                                route_id = datarowviewUsers["bus_routeid"].ToString();
                                totalsum = totalsum + Convert.ToInt32(datarowviewUsers["studentcount"]);
                            }
                        }

                        int staffcount = 0;

                        if (dv2.Count > 0)
                        {
                            foreach (DataRowView datarowviewUsers2 in dv2)
                            {
                                staffcount = staffcount + Convert.ToInt32(datarowviewUsers2["staffcount"]);
                            }
                        }

                        int tonu = 0;

                        if (dv3.Count > 0)
                        {

                            foreach (DataRowView datarowviewUsers3 in dv3)
                            {
                                string b;
                                string a = Convert.ToString(datarowviewUsers3["TotalNo_Seat"].ToString());
                                if (a == "")
                                {
                                    b = "0";
                                }
                                else
                                {
                                    b = a;
                                }
                                tonu = Convert.ToInt32(b);
                            }
                        }

                        vacanto = tonu - (totalsum + staffcount);


                        slno++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = slno.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = temp_vech_name.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = regno;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = temproute_name.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = tonu.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = totalsum.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = staffcount.ToString();
                        if (vacanto < 0)
                        {
                            vacanto = vacanto * -1;
                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.Red;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = "(" + vacanto.ToString() + ")";
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = vacanto.ToString();
                        }
                    }
                }
                //int rowcount = FpSpread1.Sheets[0].RowCount;
                //FpSpread1.Height = 700;

                //FpSpread1.Sheets[0].PageSize = 25 + (rowcount * 20);
                //FpSpread1.SaveChanges();

                //  FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                int toalrow = FpSpread1.Sheets[0].RowCount;
                int divrow = toalrow / 30;
                int remrow = toalrow % 30;
                int n = 0;

                ddlpagecount.Items.Clear();
                if (remrow > 0)
                {
                    divrow++;
                }
                int pagevalue = divrow;
                for (int j = 0; j < divrow; j++)
                {

                    if (j == 0)
                    {
                        ddlpagecount.Items.Insert(n, new System.Web.UI.WebControls.ListItem("Page-" + pagevalue.ToString()));

                    }
                    else
                    {
                        pagevalue--;
                        ddlpagecount.Items.Insert(n, new System.Web.UI.WebControls.ListItem("Page-" + pagevalue.ToString()));
                    }
                }
                ddlpagecount.Items.Add(" ");
                ddlpagecount.Text = " ";
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread1.Sheets[0].RowHeader.Visible = false;


            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "There is no record found";
                FpSpread1.Visible = false;
                //btnprintmaster.Visible = false;
                ddlpagecount.Items.Clear();
                lblpages.Visible = false;
                ddlpagecount.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlpagecount_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = true;
        //btnprintmaster.Visible = true;
        FpSpread1.SaveChanges();
        ddlpage();
    }
    public void ddlpage()
    {
        try
        {
            string setpage = "";
            string poppage_value = ddlpagecount.SelectedValue.ToString();

            if (poppage_value == " ")
            {
                for (int vcount = 0; vcount < FpSpread1.Sheets[0].RowCount; vcount++)
                {
                    FpSpread1.Sheets[0].Rows[vcount].Visible = true;
                }
            }
            else
            {
                string[] split_page = poppage_value.Split(new char[] { '-' });
                setpage = split_page[1];
                if (Convert.ToInt32(setpage) > 1)
                {
                    FpSpread1.Sheets[0].Rows[0, (Convert.ToInt32(setpage) - 1) * 15].Visible = false;
                    int row = (Convert.ToInt32(setpage) - 1) * 30;
                    int v = FpSpread1.Sheets[0].RowCount - row;

                    if (v < 10)
                    {
                        FpSpread1.Sheets[0].Rows[row, FpSpread1.Sheets[0].RowCount - 1].Visible = true;
                    }
                    else
                    {
                        for (int iuj = 0; iuj < 30; iuj++)
                        {
                            FpSpread1.Sheets[0].Rows[row + iuj].Visible = true;
                        }

                    }


                    if (v > 10)
                    {
                        int hiderow = row + 30;
                        FpSpread1.Sheets[0].Rows[hiderow, FpSpread1.Sheets[0].RowCount - 1].Visible = false;
                    }

                }
                else
                {
                    FpSpread1.Sheets[0].PageSize = Convert.ToInt32(setpage) * 30;

                    int row = (Convert.ToInt32(setpage)) * 30;
                    int v = FpSpread1.Sheets[0].RowCount - row;

                    if (v < 10)
                    {
                        FpSpread1.Sheets[0].Rows[0, FpSpread1.Sheets[0].RowCount - 1].Visible = true;
                    }
                    else
                    {
                        for (int ij = 0; ij < 30; ij++)
                        {
                            //  FpSpread1.Sheets[0].Cells[row + iuj, 0, row + iuj, 3].Row.Visible = false;
                            FpSpread1.Sheets[0].Rows[ij].Visible = true;
                        }

                    }


                }

            }


        }

        catch (Exception ex)
        {
            throw ex;

        }

    }
    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;

            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
            }
            else
            {
                lblerror1.Text = "Please Enter Your Report Name";
                lblerror1.Visible = true;
            }

        }
        catch (Exception ex)
        {
            lblerror1.Text = ex.ToString();
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {

        Session["column_header_row_count"] = 2;

        string degreedetails = string.Empty;
        string section = string.Empty;
        string selected_vehicle = string.Empty;
        string selected_routeid = string.Empty;

        degreedetails = "Vehicle Vacancy Report";
        string pagename = "Vechiclevacancyreport.aspx";
        Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
        Printcontrol.Visible = true;
    }
}

