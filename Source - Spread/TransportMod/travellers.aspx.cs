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
using System.ComponentModel;
using System.Diagnostics;
using FarPoint.Web.Spread;
using FarPoint.Web.Spread.Model;

public partial class travellers : System.Web.UI.Page
{

    [Serializable()]
    public class MyImg : ImageCellType
    {
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            //''------------clg left logo
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(80);
            // img.Height = Unit.Percentage(80);
            return img;

            //'-------------clg right logo
            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image();
            img2.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img2.Width = Unit.Percentage(80);
            // img2.Height = Unit.Percentage(80);
            return img2;

        }
    }
    [Serializable()]
    public class MyImg3 : ImageCellType
    {
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            //'-------------studentphoto
            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image();
            img2.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img2.Width = Unit.Percentage(50);
            img2.Height = Unit.Percentage(50);

            return img2;

        }
    }
    [Serializable()]
    public class MyImg4 : ImageCellType
    {
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            //'-------------staff photo
            System.Web.UI.WebControls.Image imgstaff = new System.Web.UI.WebControls.Image();
            imgstaff.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            //imgstaff.Width = Unit.Percentage(70);
            //imgstaff.Height = Unit.Percentage(70);

            imgstaff.Attributes.Add("style", "position:relative; left:50px; top:2px;");

            imgstaff.Height = Unit.Pixel(100);
            imgstaff.Width = Unit.Pixel(80);

            //imgstaff.ImageAlign = ImageAlign.AbsMiddle;
            //imgstaff.ImageAlign = ImageAlign.Middle;
            //    imgstaff.CssClass = "position: relative; left: 10px";

            // height: 100px; width: 70px; position: relative; left: 60px;
            return imgstaff;
        }
    }

    string usercode = string.Empty;
    DAccess2 d2 = new DAccess2();
    DataSet ds2 = new DataSet();
    string vech_all = string.Empty;
    string sess_all = string.Empty;
    //string route_all = string.Empty;//hided by manikandan

    int vech_count = 0;
    int initialrow = 0;
    Hashtable hat = new Hashtable();

    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    string vech_values = string.Empty;
    string route_values = string.Empty;
    string ddl_sess = string.Empty;

    Hashtable hasrow = new Hashtable();

    string startname = "";
    string endname = "";
    string veh_id = "";
    string noofstage = "";
    string strtime = "";
    string endtim = "";
    string dep = "";
    string st_name = "";
    string roll_no = "";
    string stafname = "";
    string depname = "";
    string staffcode = "";
    string app_no = "";
    string PageLogOut = "";

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
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;

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


                //for (int i = 0; i < vehiclechecklist.Items.Count; i++)
                //{
                //    vehiclechecklist.Items[i].Selected = false;

                //}

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

                    //Vehicleid.Text = vehiclechecklist.SelectedItem.ToString();

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
                    //  Vehicleid.Text = vehiclechecklist.SelectedItem.ToString();
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
                //  Vehicleid.Text = "--Select--";
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

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {

        Control cntUpdateBtn = FpSpread1.FindControl("Update");
        Control cntCancelBtn = FpSpread1.FindControl("Cancel");
        Control cntCopyBtn = FpSpread1.FindControl("Copy");
        Control cntCutBtn = FpSpread1.FindControl("Clear");
        Control cntPasteBtn = FpSpread1.FindControl("Paste");
        Control cntPageNextBtn = FpSpread1.FindControl("Next");
        Control cntPagePreviousBtn = FpSpread1.FindControl("Prev");

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

            tc = (TableCell)cntPageNextBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPagePreviousBtn.Parent;
            tr.Cells.Remove(tc);
        }

        base.Render(writer);
    }

    protected void btnMainGo_Click(object sender, EventArgs e)
    {
        try
        {
            DataView dv = new DataView();
            DataTable dat = new DataTable();
            DataTable dat1 = new DataTable();
            DataTable dat2 = new DataTable();
            DataTable dat3 = new DataTable();
            DataTable dat4 = new DataTable();
            DataTable dat5 = new DataTable();
            DataTable dat6 = new DataTable();
            DataTable dat7 = new DataTable();

            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 4;

            con.Close();
            con.Open();


            string rouvalue = txt_route.Text;


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
            int checkrowcount = 0;
            for (int route_count = 0; route_count < checkrolist.Items.Count; route_count++)
            {
                if (checkrolist.Items[route_count].Selected == true)
                {
                    co++;
                    //if (route_all == "")
                    //{
                    string route_all = checkrolist.Items[route_count].Text;

                    //}
                    //else
                    //{
                    //    route_all = route_all + "','" + checkrolist.Items[route_count].Text;
                    //}
                    //    }
                    //}

                    con.Close();
                    con.Open();

                    string routes = route_all;
                    string[] route = routes.Split(',');

                    DataSet ds = new DataSet();
//                    string stquery = "select route_id, v.veh_id, stage_name,dep_time from routemaster,Vehicle_master v where route_id in('" + route_all + "') and arr_time='Halt' and sess='M' and route_id=v.Route  select  route_id,stage_name,arr_time from routemaster where route_id in('" + route_all + "') and dep_time='Halt' and sess='M' select distinct route_id ,stages from routemaster where route_id in('" + route_all + "') select distinct d.dept_acronym as dept_acronym,registration.app_no, registration.Roll_No as roll, registration.batch_year,c.course_name as course_name ,registration.sections,registration.current_semester as current_semester ,registration.Reg_No as regno,registration.stud_name as studname,registration.bus_routeid from registration, applyn a,course c,department d,degree de where a.app_no=registration.app_no and registration.degree_code=de.degree_code and c.course_id=de.course_id and d.dept_code=de.dept_code and  registration.bus_routeid in('" + route_all + "') order by  roll select sm.bus_routeid,sm.staff_name,s.dept_name,sm.appl_no,sm.staff_code from staffmaster sm,staff_appl_master s where bus_routeid in('" + route_all + "') and sm.appl_no=s.appl_no select photo,r.app_no,r.roll_no from registration r,stdphoto s,applyn a where  a.app_no=r.app_no and r.bus_routeid in('" + route_all + "') and a.app_no=s.app_no select route_id, staff_code from DriverAllotment where route_id in ('" + route_all + "') and design='Driver'  select route_id ,staff_code from DriverAllotment where route_id in ('" + route_all + "') and design='helper'";
                   //19march2018 string stquery = "select route_id, v.veh_id, (select s.stage_name from stage_master s where cast(s.stage_id as varchar(100))=cast(routemaster.stage_name as varchar(100))) as stage_name,dep_time from routemaster,Vehicle_master v where route_id in('" + route_all + "') and arr_time='Halt' and sess='M' and route_id=v.Route  select  route_id,(select s.stage_name from stage_master s where cast(s.stage_id as varchar(100))=cast(routemaster.stage_name as varchar(100)))  as stage_name,arr_time from routemaster where route_id in('" + route_all + "') and dep_time='Halt' and sess='M' select distinct route_id ,stages from routemaster where route_id in('" + route_all + "') select distinct d.dept_acronym as dept_acronym,registration.app_no, registration.Roll_No as roll, registration.batch_year,c.course_name as course_name ,registration.sections,registration.current_semester as current_semester ,registration.Reg_No as regno,registration.stud_name as studname,registration.bus_routeid from registration, applyn a,course c,department d,degree de where a.app_no=registration.app_no and registration.degree_code=de.degree_code and c.course_id=de.course_id and d.dept_code=de.dept_code and  registration.bus_routeid in('" + route_all + "') order by  roll select sm.bus_routeid,sm.staff_name,s.dept_name,sm.appl_no,sm.staff_code from staffmaster sm,staff_appl_master s where bus_routeid in('" + route_all + "') and sm.appl_no=s.appl_no select photo,r.app_no,r.roll_no from registration r,stdphoto s,applyn a where  a.app_no=r.app_no and r.bus_routeid in('" + route_all + "') and a.app_no=s.app_no select route_id, staff_code from DriverAllotment where route_id in ('" + route_all + "') and design='Driver'  select route_id ,staff_code from DriverAllotment where route_id in ('" + route_all + "') and design='helper'";

                    string stquery = "select route_id, v.veh_id, (select s.stage_name from stage_master s where cast(s.stage_id as varchar(100))=cast(routemaster.stage_name as varchar(100))) as stage_name,dep_time from routemaster,Vehicle_master v where route_id in('" + route_all + "')  and route_id=v.Route  select  route_id,(select s.stage_name from stage_master s where cast(s.stage_id as varchar(100))=cast(routemaster.stage_name as varchar(100)))  as stage_name,arr_time from routemaster where route_id in('" + route_all + "')  select distinct route_id ,stages from routemaster where route_id in('" + route_all + "') select distinct d.dept_acronym as dept_acronym,registration.app_no, registration.Roll_No as roll, registration.batch_year,c.course_name as course_name ,registration.sections,registration.current_semester as current_semester ,registration.Reg_No as regno,registration.stud_name as studname,registration.bus_routeid from registration, applyn a,course c,department d,degree de where a.app_no=registration.app_no and registration.degree_code=de.degree_code and c.course_id=de.course_id and d.dept_code=de.dept_code and  registration.bus_routeid in('" + route_all + "') order by  roll select sm.bus_routeid,sm.staff_name,s.dept_name,sm.appl_no,sm.staff_code from staffmaster sm,staff_appl_master s where bus_routeid in('" + route_all + "') and sm.appl_no=s.appl_no select photo,r.app_no,r.roll_no from registration r,stdphoto s,applyn a where  a.app_no=r.app_no and r.bus_routeid in('" + route_all + "') and a.app_no=s.app_no select route_id, staff_code from DriverAllotment where route_id in ('" + route_all + "') and design='Driver'  select route_id ,staff_code from DriverAllotment where route_id in ('" + route_all + "') and design='helper'";
                    SqlDataAdapter da = new SqlDataAdapter(stquery, con);
                    da.Fill(ds);
                    dat = ds.Tables[0];
                    dat1 = ds.Tables[1];
                    dat2 = ds.Tables[2];
                    dat3 = ds.Tables[3];
                    dat4 = ds.Tables[4];
                    dat5 = ds.Tables[6];
                    dat6 = ds.Tables[7];
                    dat7 = ds.Tables[5];

                    int sub = 0;
                    int re = 0;
                    int totr = 0;

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        checkrowcount++;
                        FpSpread1.Visible = true;
                        FpSpread1.Sheets[0].SheetName = " ";

                        ddlpagecount.Visible = true;
                        lblpages.Visible = true;

                        lblerror.Visible = false;
                        lblerror1.Visible = false;
                        //lblrptname.Visible = true;
                        //txtexcelname.Visible = true;
                        //btn_excel.Visible = true;

                        
                        
                        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;


                        FpSpread1.Sheets[0].Columns[0].Locked = true;
                        FpSpread1.Sheets[0].Columns[1].Locked = true;
                        FpSpread1.Sheets[0].Columns[2].Locked = true;
                        FpSpread1.Sheets[0].Columns[3].Locked = true;

                        FpSpread1.Sheets[0].Columns[0].Width = 200;
                        FpSpread1.Sheets[0].Columns[1].Width = 200;
                        FpSpread1.Sheets[0].Columns[2].Width = 200;
                        FpSpread1.Sheets[0].Columns[3].Width = 200;


                        for (int veccount = 0; veccount < ds.Tables[0].Rows.Count; veccount++)
                        {

                            FpSpread1.Sheets[0].RowCount++;

                            string semester = "";
                            string year = "";


                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "Vechicle ID" + "( " + veh_id + " )";

                            FpSpread1.Sheets[0].ColumnHeader.Visible = false;
                            FpSpread1.Sheets[0].RowHeader.Visible = false;

                            // FpSpread1.CommandBar.Visible = false;

                            FpSpread1.Sheets[0].AutoPostBack = true;


                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColorLeft = Color.Black;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColorRight = Color.White;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColorBottom = Color.White;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColorLeft = Color.Black;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColorBottom = Color.Black;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Border.BorderColorBottom = Color.Black;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Border.BorderColorBottom = Color.Black;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Border.BorderColorBottom = Color.Black;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Border.BorderColorRight = Color.White;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Border.BorderColorRight = Color.White;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Border.BorderColorRight = Color.White;

                            string temproute_name = string.Empty;

                            temproute_name = dat.Rows[veccount]["route_id"].ToString();

                            dat.DefaultView.RowFilter = " route_id='" + temproute_name + "' ";

                            dat1.DefaultView.RowFilter = "route_id='" + temproute_name + "' ";

                            dat2.DefaultView.RowFilter = "route_id='" + temproute_name + "'";

                            dat3.DefaultView.RowFilter = "Bus_routeid='" + temproute_name + "'";

                            dat4.DefaultView.RowFilter = "bus_routeid='" + temproute_name + "'";

                            dat5.DefaultView.RowFilter = "route_id='" + temproute_name + "'";

                            dat6.DefaultView.RowFilter = "route_id='" + temproute_name + "'";

                            DataView dv1 = new DataView();
                            dv1 = dat.DefaultView;

                            DataView dv2 = new DataView();
                            dv2 = dat1.DefaultView;

                            DataView dv3 = new DataView();
                            dv3 = dat2.DefaultView;

                            DataView dv4 = new DataView();
                            dv4 = dat3.DefaultView;

                            DataView dv5 = new DataView();
                            dv5 = dat4.DefaultView;

                            DataView dv6 = new DataView();
                            dv6 = dat5.DefaultView;

                            DataView dv7 = new DataView();
                            dv7 = dat6.DefaultView;

                            if (ds.Tables[0].Rows.Count > veccount)
                            {
                                startname = ds.Tables[0].Rows[veccount]["stage_name"].ToString();
                                strtime = ds.Tables[0].Rows[veccount]["dep_time"].ToString();
                                veh_id = ds.Tables[0].Rows[veccount]["veh_id"].ToString();
                            }
                            if (ds.Tables[1].Rows.Count > veccount)
                            {
                                endname = ds.Tables[1].Rows[veccount]["stage_name"].ToString();
                                endtim = ds.Tables[1].Rows[veccount]["arr_time"].ToString();
                            }

                            if (ds.Tables[2].Rows.Count > veccount ) 
                                noofstage = ds.Tables[2].Rows[veccount]["stages"].ToString();

                            if (dv6.Count > 0)
                            {
                                string staffdivercode = dv6[0]["staff_code"].ToString();
                                MyImg4 driverphoto = new MyImg4();

                                driverphoto.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + staffdivercode;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = driverphoto;
                                // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Note = temproute_name;
                            }
                            if (dv7.Count > 0)
                            {
                                string staffhelpercode = dv7[0]["staff_code"].ToString();
                                MyImg4 helperphoto = new MyImg4();

                                helperphoto.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + staffhelpercode;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = helperphoto;
                                // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Note = temproute_name;
                            }

                            MyImg4 mileft = new MyImg4();
                            mileft.ImageUrl = "Handler/Veh_Back_Photo.ashx?Veh_ID=" + veh_id;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].CellType = mileft;
                            //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = temproute_name;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Starting Place: " + startname.ToString() + "\n" + " Time:" + strtime + "\n" + " Ending Place: " + endname.ToString() + "\n" + "Time:" + endtim.ToString() + "\n" + "No of Stage:    " + noofstage.ToString(); ;

                            FpSpread1.Sheets[0].RowCount++;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColorBottom = Color.White;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Border.BorderColorBottom = Color.White;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Border.BorderColorBottom = Color.White;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Border.BorderColorBottom = Color.White;
                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Visible = false;

                            FpSpread1.Sheets[0].RowCount++;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColorBottom = Color.White;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Border.BorderColorBottom = Color.White;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Border.BorderColorBottom = Color.White;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Border.BorderColorBottom = Color.White;
                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Visible = false;

                            if (dv4.Count > 0)
                            {
                                int temp = 0;
                                int maxrow = FpSpread1.Sheets[0].RowCount - 1;
                                for (int i = 0; i < dv4.Count; i++)
                                {
                                    temp++;
                                    roll_no = dv4[i]["roll"].ToString();
                                    st_name = dv4[i]["studname"].ToString();
                                    dep = dv4[i]["dept_acronym"].ToString();

                                    semester = Convert.ToInt32(dv4[i]["current_semester"]).ToString();

                                    if ((semester == "3") || (semester == "4"))
                                    {
                                        year = "II  Year";
                                    }
                                    if ((semester == "1") || (semester == "2"))
                                    {
                                        year = "I  Year";
                                    }
                                    if ((semester == "5") || (semester == "6"))
                                    {
                                        year = "III  Year";
                                    }
                                    if ((semester == "7") || (semester == "8"))
                                    {
                                        year = "IV  Year";
                                    }

                                    year = year.ToString();

                                    if (temp == 4 || i == 0)
                                    {
                                        FpSpread1.Sheets[0].RowCount++;
                                        temp = 0;
                                        initialrow = FpSpread1.Sheets[0].RowCount - 1;
                                    }



                                    
                                    MyImg4 mi5 = new MyImg4();

                                    //mi5.ImageUrl = "/Handler/Handler4.ashx?rollno=" + roll_no;

                                    mi5.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + roll_no;//added by rajasekar 04/09/2018

                                    mi5.CssClass = "";

                                    FpSpread1.Sheets[0].Cells[initialrow, temp].CellType = mi5;

                                    FpSpread1.Sheets[0].Cells[initialrow, temp].Note = temproute_name;

                                    mi5.ImageAlign = ImageAlign.Middle;



                                   FpSpread1.Sheets[0].Rows[initialrow].Border.BorderColorBottom = Color.White;

                                    if (temp == 0 || i == 0)
                                    {
                                        FpSpread1.Sheets[0].RowCount++;

                                        temp = 0;
                                    }

                                    FpSpread1.Sheets[0].Cells[initialrow + 1, temp].Text = st_name;
                                    FpSpread1.Sheets[0].Cells[initialrow + 1, temp].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[initialrow + 1, temp].Note = temproute_name;

                                    FpSpread1.Sheets[0].Rows[initialrow + 1].Border.BorderColorBottom = Color.White;

                                    if (temp == 0 || i == 0)
                                    {
                                        FpSpread1.Sheets[0].RowCount++;

                                        temp = 0;
                                    }
                                    FpSpread1.Sheets[0].Cells[initialrow + 2, temp].Text = year + "-" + dep;
                                    FpSpread1.Sheets[0].Cells[initialrow + 2, temp].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[initialrow + 2, temp].Note = temproute_name;
                                }
                            }

                            if (dv5.Count > 0)
                            {
                                int tempstaff = 0;
                                initialrow = initialrow + 3;
                                for (int j = 0; j < dv5.Count; j++)
                                {

                                    tempstaff++;
                                    stafname = dv5[j]["staff_name"].ToString();
                                    depname = dv5[j]["dept_name"].ToString();
                                    staffcode = dv5[j]["staff_code"].ToString();
                                    app_no = dv5[j]["appl_no"].ToString();
                                    if (tempstaff == 4 || j == 0)
                                    {
                                        FpSpread1.Sheets[0].RowCount++;

                                        tempstaff = 0;
                                    }
                                    MyImg4 myimage = new MyImg4();
                                    myimage.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + staffcode;
                                    FpSpread1.Sheets[0].Cells[initialrow, tempstaff].CellType = myimage;

                                    FarPoint.Web.Spread.SmartPrintRulesCollection printrules = new FarPoint.Web.Spread.SmartPrintRulesCollection();

                                    printrules.Add(new FarPoint.Web.Spread.BestFitColumnRule(FarPoint.Web.Spread.ResetOption.All));

                                    FarPoint.Web.Spread.PrintInfo printset = new FarPoint.Web.Spread.PrintInfo();

                                    printset.SmartPrintRules = printrules;

                                    printset.UseSmartPrint = true;

                                    FpSpread1.Sheets[0].Cells[initialrow, tempstaff].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[initialrow, tempstaff].Note = temproute_name;
                                    FpSpread1.Sheets[0].Rows[initialrow].Border.BorderColorBottom = Color.White;

                                    // FpSpread1.Sheets[0].Cells[initialrow, tempstaff].Border.BorderColorBottom = Color.White;
                                    if (tempstaff == 0 || j == 0)
                                    {
                                        FpSpread1.Sheets[0].RowCount++;

                                        tempstaff = 0;
                                    }
                                    FpSpread1.Sheets[0].Cells[initialrow + 1, tempstaff].Text = stafname;
                                    FpSpread1.Sheets[0].Cells[initialrow + 1, tempstaff].Note = temproute_name;
                                    FpSpread1.Sheets[0].Rows[initialrow + 1].Border.BorderColorBottom = Color.White;
                                    //FpSpread1.Sheets[0].Cells[initialrow + 1, tempstaff].Border.BorderColorBottom = Color.White;
                                    if (tempstaff == 0 || j == 0)
                                    {
                                        FpSpread1.Sheets[0].RowCount++;

                                        tempstaff = 0;
                                    }
                                    FpSpread1.Sheets[0].Cells[initialrow + 2, tempstaff].Text = depname;
                                    FpSpread1.Sheets[0].Cells[initialrow + 2, tempstaff].Note = temproute_name;
                                }
                            }


                            totr = initialrow;
                            sub = totr / 18;
                            re = totr % 18;

                            if (re >= 0)
                            {
                                // FpSpread1.Sheets[0].Rows[0, FpSpread1.Sheets[0].RowCount - 1].Visible = true;

                                int trt = 18 - re;

                                FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + trt;

                                int iu = FpSpread1.Sheets[0].RowCount;

                                int hg = iu;


                                for (int i = initialrow + 3; i < hg; i++)
                                {
                                    FpSpread1.Sheets[0].Rows[i].Visible = false;
                                }
                                sub = sub + 1;
                            }

                            FpSpread1.Pager.Font.Bold = true;
                            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antique";
                            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
                            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                        }

                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;

                        int totalrows = initialrow;
                        int page = totalrows / 18;

                        int remainrows = totalrows % 18;
                        int n = 0;

                        //int i5 = 0;
                        ddlpagecount.Items.Clear();
                        if (remainrows > 0)
                        {
                            page++;
                        }
                        int pagevalue = page;
                        for (int j = 0; j < page; j++)
                        {

                            if (j == 0)
                            {
                                ddlpagecount.Items.Insert(n, new System.Web.UI.WebControls.ListItem("Page-" + pagevalue.ToString()));
                                //   mm = mm + 1;
                            }
                            else
                            {
                                pagevalue--;
                                ddlpagecount.Items.Insert(n, new System.Web.UI.WebControls.ListItem("Page-" + pagevalue.ToString()));
                            }

                        }

                        ddlpagecount.Items.Add(" ");
                        ddlpagecount.Text = " ";
                    }


                    
                    //start===========modified by manikandan
                }
            }
            if(checkrowcount == 0)
            {
                lblerror.Visible = true;
                lblerror.Text = "There is no record found";
                FpSpread1.Visible = false;
                ddlpagecount.Items.Clear();
                lblpages.Visible = false;
                ddlpagecount.Visible = false;
            }
               // }
            //}
            //end==============
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlpagecount_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = true;
        FpSpread1.SaveChanges();
        ddlpage();
    }
    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {

        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value.ToString();
            }
        }

        return null;
    }

    public void ddlpage()
    {
        try
        {
            string setpage = "";
            string poppage_value = ddlpagecount.SelectedValue.ToString();



            //  FpSpread1.Sheets[0].Rows[0, FpSpread1.Sheets[0].RowCount - 1].Visible = true;

            if (poppage_value == " ")
            {
                for (int ij = 0; ij < FpSpread1.Sheets[0].RowCount; ij++)
                {
                    string gfg1 = FpSpread1.Sheets[0].Cells[ij, 0].Note;
                    if (gfg1 == "")
                    {
                        FpSpread1.Sheets[0].Cells[ij, 0, ij, 3].Row.Visible = false;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Rows[ij].Visible = true;
                    }
                }

                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            }
            else
            {
                string[] split_page = poppage_value.Split(new char[] { '-' });
                setpage = split_page[1];
                if (Convert.ToInt32(setpage) > 1)
                {


                    FpSpread1.Sheets[0].Rows[0, (Convert.ToInt32(setpage) - 1) * 18].Visible = false;
                    int row = (Convert.ToInt32(setpage) - 1) * 18;

                    int v = FpSpread1.Sheets[0].RowCount - row;
                    // FpSpread1.Sheets[0].PageSize = 18;
                    // int hh=row + 18;
                    if (v < 18)
                    {
                        FpSpread1.Sheets[0].Rows[row, FpSpread1.Sheets[0].RowCount - 1].Visible = true;
                    }
                    else
                    {
                        for (int iuj = 0; iuj < 18; iuj++)
                        {

                            string gfg = FpSpread1.Sheets[0].Cells[row + iuj, 0].Note;

                            if (gfg == "")
                            {
                                FpSpread1.Sheets[0].Cells[row + iuj, 0, row + iuj, 3].Row.Visible = false;
                                // FpSpread1.Sheets[0].Rows[row, row + iuj].Visible = false;
                            }
                            else
                            {
                                // FpSpread1.Sheets[0].Cells[row + iuj, 0, row + iuj, 3].Row.Visible = true;

                                FpSpread1.Sheets[0].Rows[row + iuj].Visible = true;

                                // FpSpread1.Sheets[0].Rows[row, row + iuj].Visible = true;
                            }

                        }
                    }
                    if (v > 18)
                    {
                        int hiderow = row + 18;
                        FpSpread1.Sheets[0].Rows[hiderow, FpSpread1.Sheets[0].RowCount - 1].Visible = false;
                    }
                }
                else
                {
                    FpSpread1.Sheets[0].PageSize = Convert.ToInt32(setpage) * 18;


                    int row = (Convert.ToInt32(setpage)) * 18;
                    int v = FpSpread1.Sheets[0].RowCount - row;

                    if (v < 18)
                    {

                        FpSpread1.Sheets[0].Rows[row, FpSpread1.Sheets[0].RowCount - 1].Visible = true;

                    }
                    else
                    {
                        FpSpread1.Sheets[0].Rows[0, row + 18].Visible = true;
                        FpSpread1.Sheets[0].Rows[1].Visible = false;
                        FpSpread1.Sheets[0].Rows[2].Visible = false;

                    }
                    if (v > 18)
                    {
                        int hiderow = row;
                        FpSpread1.Sheets[0].Rows[hiderow, FpSpread1.Sheets[0].RowCount - 1].Visible = false;
                    }
                }
            }
            FpSpread1.SaveChanges();
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
}