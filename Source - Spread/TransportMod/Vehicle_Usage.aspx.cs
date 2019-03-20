
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
using InsproDataAccess;

public partial class maintenancepage : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    public SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    Boolean waytonewpage = false;
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
    string caption = string.Empty;
    string sql = string.Empty;
    string vechileid = string.Empty;
    string typeofexpense = string.Empty;
    string expensekm = string.Empty;
    bool val = false;
    DataSet ds = new DataSet();
    DataSet ds_expense = new DataSet();
    DAccess2 obj = new DAccess2();
    Hashtable hastab = new Hashtable();
    Hashtable hat = new Hashtable();
    DateTime date;
    DateTime frmdate;
    DateTime todate;
    string fuel = "0";
    string seltext = "";
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    static int gerrow = -1;
    string[] itemarray;
    int totalff = 0;
    string PageLogOut = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        // Txttotalamount.Text = "100";
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
        lblerr1.Visible = false;
        //btn_delete.Enabled = false;
        // lbl_add.Text = "Add";
        lblerrmsg.Visible = false;
        lblmessagefule.Visible = false;
        txt_vech.Attributes.Add("readonly", "readonly");
        txtfrm_date.Attributes.Add("readonly", "readonly");
        txtend_date.Attributes.Add("readonly", "readonly");
        txt_date.Attributes.Add("readonly", "readonly");
        txtarivaldate.Attributes.Add("readonly", "readonly");
        Txtbilldate.Attributes.Add("readonly", "readonly");
        Txtdate.Attributes.Add("readonly", "readonly");
        lbl_Validation.Visible = false;
        errmsg.Visible = false;
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        Fpmaintenance.Sheets[0].AutoPostBack = true;
        Fpmaintenance.CommandBar.Visible = true;
        Fpmaintance.Sheets[0].AutoPostBack = true;
        Fpmaintance.CommandBar.Visible = true;
        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        style.Font.Size = 10;
        style.Font.Bold = true;
        Fpmaintance.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        Fpmaintance.Sheets[0].AllowTableCorner = true;
        Fpmaintance.Sheets[0].RowHeader.Visible = false;

        Fpmaintance.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        //FpSpread1.Sheets[0]

        Fpmaintance.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        Fpmaintance.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

        Fpmaintance.Sheets[0].DefaultColumnWidth = 50;
        Fpmaintance.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        Fpmaintance.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        Fpmaintance.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

        Fpmaintance.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        Fpmaintance.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        Fpmaintance.Sheets[0].DefaultStyle.Font.Bold = false;
        Fpmaintance.SheetCorner.Cells[0, 0].Font.Bold = true;


        Fp_Fuel.Sheets[0].AutoPostBack = true;
        Fp_Fuel.CommandBar.Visible = false;
        //FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
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

        Fp_Intimation_Driver.Sheets[0].AutoPostBack = true;
        Fp_Intimation_Driver.CommandBar.Visible = true;
        style.Font.Size = 10;
        style.Font.Bold = true;
        Fp_Intimation_Driver.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        Fp_Intimation_Driver.Sheets[0].AllowTableCorner = true;
        Fp_Intimation_Driver.Sheets[0].RowHeader.Visible = false;

        Fp_Intimation_Driver.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        Fp_Intimation_Driver.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        Fp_Intimation_Driver.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

        Fp_Intimation_Driver.Sheets[0].DefaultColumnWidth = 50;
        Fp_Intimation_Driver.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        Fp_Intimation_Driver.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        Fp_Intimation_Driver.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

        Fp_Intimation_Driver.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        Fp_Intimation_Driver.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        Fp_Intimation_Driver.Sheets[0].DefaultStyle.Font.Bold = false;
        Fp_Intimation_Driver.SheetCorner.Cells[0, 0].Font.Bold = true;

        Fp_Intimation_Vehicle.Sheets[0].AutoPostBack = true;
        Fp_Intimation_Vehicle.CommandBar.Visible = true;
        style.Font.Size = 10;
        style.Font.Bold = true;
        Fp_Intimation_Vehicle.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        Fp_Intimation_Vehicle.Sheets[0].AllowTableCorner = true;
        Fp_Intimation_Vehicle.Sheets[0].RowHeader.Visible = false;

        Fp_Intimation_Vehicle.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        Fp_Intimation_Vehicle.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        Fp_Intimation_Vehicle.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

        Fp_Intimation_Vehicle.Sheets[0].DefaultColumnWidth = 50;
        Fp_Intimation_Vehicle.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        Fp_Intimation_Vehicle.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        Fp_Intimation_Vehicle.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

        Fp_Intimation_Vehicle.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        Fp_Intimation_Vehicle.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        Fp_Intimation_Vehicle.Sheets[0].DefaultStyle.Font.Bold = false;
        Fp_Intimation_Vehicle.SheetCorner.Cells[0, 0].Font.Bold = true;


        FpfuelReport.Sheets[0].AutoPostBack = true;
        FpfuelReport.CommandBar.Visible = false;
        style.Font.Size = 10;
        style.Font.Bold = true;
        FpfuelReport.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        FpfuelReport.Sheets[0].AllowTableCorner = true;
        FpfuelReport.Sheets[0].RowHeader.Visible = false;
        FpfuelReport.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        FpfuelReport.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        FpfuelReport.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        FpfuelReport.Sheets[0].DefaultColumnWidth = 50;
        FpfuelReport.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        FpfuelReport.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        FpfuelReport.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        FpfuelReport.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        FpfuelReport.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        FpfuelReport.Sheets[0].DefaultStyle.Font.Bold = false;
        FpfuelReport.SheetCorner.Cells[0, 0].Font.Bold = true;

        Fpfueldetails.Sheets[0].AutoPostBack = true;
        Fpfueldetails.CommandBar.Visible = false;
        Fpfueldetails.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        Fpfueldetails.Sheets[0].AllowTableCorner = true;
        Fpfueldetails.Sheets[0].RowHeader.Visible = false;
        Fpfueldetails.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        Fpfueldetails.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        Fpfueldetails.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        Fpfueldetails.Sheets[0].DefaultColumnWidth = 50;
        Fpfueldetails.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        Fpfueldetails.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        Fpfueldetails.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        Fpfueldetails.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        Fpfueldetails.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        Fpfueldetails.Sheets[0].DefaultStyle.Font.Bold = false;
        Fpfueldetails.SheetCorner.Cells[0, 0].Font.Bold = true;
        if (Fpfueldetails.Visible == true)
        {
            btnback.Visible = true;
        }
        else
        {
            btnback.Visible = false;
        }
        add();
        if (!IsPostBack)
        {
            ddl_itemtype.Attributes.Add("onfocus", "frelig()");
            setLabelText();
            Txtfuelamount.Attributes.Add("Readonly", "Readonly");
            college();
            Btnupdate.Visible = false;
            Session["textval"] = "";
            company();
            btnsub.Visible = false;
            Intimation();
            maintainpanel.Visible = false;
            //rdbfuel.Checked = true;
            ddl_report.SelectedIndex = 0;
            typeremove.Visible = true;
            add();
            ddlpurpose.Attributes.Add("onfocus", "subu()");
            page_details();
            rdbfuel.Checked = true;
            fuelpanel.Visible = true;
            con.Close();
            con.Open();
            SqlCommand cmd_veh_type;
            ddlvechicletype.Items.Insert(0, "All");
            cmd_veh_type = new SqlCommand("Select distinct Veh_Type from vehicle_master", con);
            SqlDataReader rdr_veh_type = cmd_veh_type.ExecuteReader();


            ddl_vehtype.Items.Insert(0, "All");
            Ddlvehicleid.Items.Insert(0, "All");
            ddlvechicletype.Items.Insert(0, "All");
            int incre_type = 0;
            while (rdr_veh_type.Read())
            {
                if (rdr_veh_type.HasRows == true)
                {
                    incre_type++;
                    System.Web.UI.WebControls.ListItem list_veh_type = new System.Web.UI.WebControls.ListItem();
                    list_veh_type.Text = (rdr_veh_type["Veh_Type"].ToString());

                    ddl_vehtype.Items.Add(list_veh_type);
                    //chkls_type.Items.Add(list_veh_type);
                    //chkls_type.Items[incre_type - 1].Selected = true;
                }
            }

            con.Close();
            con.Open();
            SqlCommand cmd_vehicle_id;
            if (ddlselectcollege.Text == "All")
            {
                cmd_vehicle_id = new SqlCommand("select * from vehicle_master order by len(veh_id), Veh_ID", con);
            }
            else
            {
                cmd_vehicle_id = new SqlCommand("select * from vehicle_master  where college_code='" + ddlselectcollege.SelectedItem.Value + "' order by len(veh_id), Veh_ID", con);
            }
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
            bindvechicle1();
            bindvechicle();
            bindvechicletype();
            bindtypeofexpense();
            bindtypeofexpense1();
            bindMethod();

            txtfrm_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtend_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtarivaldate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            Txtdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            Txtbilldate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            btnMainGo_Click(sender, e);
            //Timer1_Tick(sender, e);
            mpemsgboxsave.Hide();
            Accordion1.SelectedIndex = 0;
            Fpmaintenance.Visible = false;
            Bind_Routes();
            Bind_driver();
            Bind_startplace();
            Get_Opening_KM();
            Get_Opening_KM1();

        }
        if (Session["textval"] != null)
        {
            if (Session["textval"].ToString() != "")
            {
                Txttotalamount.Text = Session["textval"].ToString();
            }
        }

        Get_Opening_KM();
        Get_Opening_KM1();
    }

    void Get_Opening_KM()
    {
        if (ddlvecid.Items.Count > 0)
        {
            con.Close();
            con.Open();

            SqlCommand cmd_initial_km = new SqlCommand("select isnull(intial_km,0) as initial_km from vehicle_master where veh_id='" + ddlvecid.SelectedItem.ToString() + "'", con);
            SqlDataAdapter ad_initial_km = new SqlDataAdapter(cmd_initial_km);
            DataTable dt_initial_km = new DataTable();
            ad_initial_km.Fill(dt_initial_km);

            int initial_km = 0;

            if (dt_initial_km.Rows.Count > 0)
            {
                if (dt_initial_km.Rows[0][0].ToString() != "")
                {
                    initial_km = Convert.ToInt32(dt_initial_km.Rows[0][0].ToString());
                }
                else
                {
                    initial_km = 0;
                }
            }

            con.Close();
            con.Open();

            SqlCommand cmd_get_closekm = new SqlCommand("select isnull(sum(travel_km),0) as close_km from vehicle_usage where vehicle_id='" + ddlvecid.SelectedItem.ToString() + "'  and billno is null and billdate is null and companyname is null and description is null", con);
            SqlDataAdapter ad_get_closekm = new SqlDataAdapter(cmd_get_closekm);
            DataTable dt_get_closekm = new DataTable();
            ad_get_closekm.Fill(dt_get_closekm);

            if (dt_get_closekm.Rows.Count > 0)
            {
                txt_openkm.Text = Convert.ToString(initial_km + Convert.ToInt32(dt_get_closekm.Rows[0][0].ToString()));
            }
            else
            {
                txt_openkm.Text = "0";
            }
        }
    }

    void Get_Opening_KM1()
    {
        if (Ddlvehicleid.Items.Count > 0)
        {
            con.Close();
            con.Open();

            SqlCommand cmdinitialkm = new SqlCommand("select isnull(intial_km,0) as initial_km from vehicle_master where veh_id='" + Ddlvehicleid.SelectedItem.ToString() + "' ", con);
            SqlDataAdapter ad_initial_km = new SqlDataAdapter(cmdinitialkm);
            DataTable dtinitial_km = new DataTable();
            ad_initial_km.Fill(dtinitial_km);

            int initial_km = 0;

            if (dtinitial_km.Rows.Count > 0)
            {
                if (dtinitial_km.Rows[0][0].ToString() != "")
                {
                    initial_km = Convert.ToInt32(dtinitial_km.Rows[0][0].ToString());
                }
                else
                {
                    initial_km = 0;
                }
            }

            con.Close();
            con.Open();

            SqlCommand cmd_get_closekm = new SqlCommand("select isnull(sum(travel_km),0) as close_km from vehicle_usage where vehicle_id='" + Ddlvehicleid.SelectedItem.ToString() + "' and billno is not null and billdate is not null and companyname is not null and description is not null", con);
            SqlDataAdapter ad_get_closekm = new SqlDataAdapter(cmd_get_closekm);
            DataTable dt_get_closekm = new DataTable();
            ad_get_closekm.Fill(dt_get_closekm);

            if (dt_get_closekm.Rows.Count > 0)
            {
                Txtopeingkm.Text = Convert.ToString(initial_km + Convert.ToInt32(dt_get_closekm.Rows[0][0].ToString()));
            }
            else
            {
                Txtopeingkm.Text = "0";
            }
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

    public void bindvechicle()
    {
        con.Close();
        con.Open();
        DataTable dt_vehid = new DataTable();

        if (ddl_vehtype.Text == "All")
        {
            SqlCommand cmd_vehid = new SqlCommand("select * from vehicle_master order by len(veh_id), Veh_ID", con);
            SqlDataAdapter ad_vehid = new SqlDataAdapter(cmd_vehid);

            ad_vehid.Fill(dt_vehid);
        }
        else
        {

            SqlCommand cmd_vehid = new SqlCommand("select * from vehicle_master where veh_type='" + ddl_vehtype.Text + " order by len(veh_id), Veh_ID' ", con);
            SqlDataAdapter ad_vehid = new SqlDataAdapter(cmd_vehid);

            ad_vehid.Fill(dt_vehid);
        }


        ddlvecid.Items.Clear();
        if (dt_vehid.Rows.Count > 0)
        {
            ddlvecid.DataSource = dt_vehid;
            ddlvecid.DataTextField = "Veh_ID";
            ddlvecid.DataBind();
            Bind_Routes();

        }
    }

    public void bindvechicletype()
    {
        con.Close();
        con.Open();
        DataTable dt_vehid = new DataTable();

        if (ddlvechicletype.Text == "All")
        {
            SqlCommand cmd_vehid = new SqlCommand("select distinct Veh_Type from vehicle_master ", con);
            SqlDataAdapter ad_vehid = new SqlDataAdapter(cmd_vehid);

            ad_vehid.Fill(dt_vehid);
        }
        else
        {

            SqlCommand cmd_vehid = new SqlCommand("select distinct Veh_Type from vehicle_master where veh_type='" + ddlvechicletype.Text + "' ", con);
            SqlDataAdapter ad_vehid = new SqlDataAdapter(cmd_vehid);

            ad_vehid.Fill(dt_vehid);
        }



        if (dt_vehid.Rows.Count > 0)
        {
            ddlvechicletype.DataSource = dt_vehid;
            ddlvechicletype.DataTextField = "Veh_Type";
            ddlvechicletype.DataBind();
            ddlvechicletype.Items.Insert(0, "All");

        }
    }

    public void bindvechicle1()
    {
        con.Close();
        con.Open();
        DataTable dtvehid = new DataTable();

        if (Ddlvehicleid.Text == "All")
        {
            SqlCommand cmdvehid = new SqlCommand("select * from vehicle_master order by len(veh_id), Veh_ID", con);
            SqlDataAdapter ad_vehid = new SqlDataAdapter(cmdvehid);

            ad_vehid.Fill(dtvehid);
        }
        else
        {

            SqlCommand cmd_vehid = new SqlCommand("select * from vehicle_master  where veh_type='" + ddlvechicletype.SelectedItem.Value + "' order by len(veh_id), Veh_ID ", con);
            SqlDataAdapter ad_vehid = new SqlDataAdapter(cmd_vehid);

            ad_vehid.Fill(dtvehid);
        }


        Ddlvehicleid.Items.Clear();
        if (dtvehid.Rows.Count > 0)
        {
            Ddlvehicleid.DataSource = dtvehid;
            Ddlvehicleid.DataTextField = "Veh_ID";
            Ddlvehicleid.DataValueField = "Veh_ID";
            Ddlvehicleid.DataBind();
        }
    }

    protected void ddl_report_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlselectcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        vehiclechecklist.Items.Clear();
        txt_vech.Text = "--Select--";
        SqlCommand cmd_vehicle_id;
        con.Close();
        con.Open();
        if (ddlselectcollege.Text == "All")
        {
            cmd_vehicle_id = new SqlCommand("select * from vehicle_master order by len(veh_id), Veh_ID", con);
        }
        else
        {
            cmd_vehicle_id = new SqlCommand("select * from vehicle_master  where college_code='" + ddlselectcollege.SelectedItem.Value + "' order by len(veh_id), Veh_ID", con);
        }
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
        con.Close();
    }

    protected void ddl_Vehid_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindvechicle();
        txt_closekm.Text = "";
        txt_expensekm.Text = "";
        Get_Opening_KM();
    }

    protected void ddlvechicletype_SelectedIndexChanged(object sender, EventArgs e)
    {
        string vehicle_id = "";
        Ddlvehicleid.Items.Clear();
        if (ddlvechicletype.SelectedItem.Value != "All")
        {
            vehicle_id = "select * from vehicle_master where Veh_Type='" + ddlvechicletype.SelectedItem.Value + "' order by len(veh_id), Veh_ID ";
        }
        else
        {
            vehicle_id = "select * from vehicle_master  where Veh_Type != '' order by len(veh_id), Veh_ID";
        }
        ds.Clear();
        if (vehicle_id != "")
        {
            ds = obj.select_method(vehicle_id, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Ddlvehicleid.DataSource = ds;
                Ddlvehicleid.DataTextField = "Veh_ID";
                Ddlvehicleid.DataValueField = "Veh_ID";
                Ddlvehicleid.DataBind();
                Ddlvehicleid.Items.Insert(0, "All");
            }
        }



        //bindvechicletype();
        txtclosingkm.Text = "";
        txttravellingkm.Text = "";
        Get_Opening_KM1();
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

    public void bindtypeofexpense()
    {
        con.Close();
        con.Open();
        string sqlcmd = "select distinct textcode,textval from textvaltable_new where textcriteria='etype'";
        ds = obj.select_method(sqlcmd, hat, "text");

        // ddlexpensestype.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlexpensestype.DataSource = ds.Tables[0];
            ddlexpensestype.DataTextField = "textval";
            ddlexpensestype.DataValueField = "textcode";
            ddlexpensestype.DataBind();
        }
        ddlexpensestype.Items.Insert(0, "");
    }

    public void bindtypeofexpense1()
    {
        con.Close();
        con.Open();
        string sqlcmd = "select distinct textcode,textval from textvaltable_new where textcriteria='etype'";
        ds = obj.select_method(sqlcmd, hat, "text");

        // ddlexpensestype.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlpurpose.DataSource = ds.Tables[0];
            ddlpurpose.DataTextField = "textval";
            ddlpurpose.DataValueField = "textcode";
            ddlpurpose.DataBind();
        }
        ddlpurpose.Items.Insert(0, "");
    }

    protected void ddlexpensestype_SelectedIndexChanged(object sender, EventArgs e)
    {


        if (txt_addexpense.Text == "repair" || ddlexpensestype.SelectedItem.Text == "diesel")
        {
            txt_fuel.Enabled = true;
            //txtFuelperLt.Enabled = true;
            //Lblrepairexpenses.Visible = false;
            //TxtRepairExpenses.Visible = false;
            //Lblfuelexpanse.Visible = true;
            //txtFuelDieselExpenses.Visible = true;
            //txtFuelDieselExpenses.Text = "";
        }
        if (txt_addexpense.Text == "repair" || ddlexpensestype.SelectedItem.Text == "repair")
        {
            txt_fuel.Enabled = false;
            //txtFuelperLt.Enabled = false;
            //txt_fuel.Text = "";
            //txtFuelperLt.Text = "";
            //Lblrepairexpenses.Visible = true;
            //TxtRepairExpenses.Visible = true;
            //Lblfuelexpanse.Visible = false;
            //txtFuelDieselExpenses.Visible = false;
            //txtFuelDieselExpenses.Text = "";
        }
        else
        {
            ddlexpensestype.Text = ddlexpensestype.SelectedValue.ToString();
        }
    }

    protected void ddlpurpose_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlpurpose.Text = ddlpurpose.SelectedValue.ToString();
        Btnadd.Visible = true;
        btnsub.Visible = true;
        ddlpurpose.Attributes.Add("onfocus", "subu()");
    }

    protected void ddlvecid_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlvecid.Text = ddlvecid.SelectedValue.ToString();
        Bind_Routes();
        // txt_closekm.Text = "";
        // txt_expensekm.Text = "";
    }

    protected void ddl_routeid_SelectedIndexChanged(object sender, EventArgs e)
    {
        Bind_driver();
    }

    protected void ddldriver_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlstartplace_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlhour_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddldescplace_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void txtarivaldate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime dtnow1 = DateTime.Now;
            string date2ad;
            string datetoad;
            string yr5, m5, d5;
            date2ad = txtarivaldate.Text.ToString();
            string[] split5 = date2ad.Split(new Char[] { '/' });



            if (split5.Length == 3)
            {
                datetoad = split5[0].ToString() + "/" + split5[1].ToString() + "/" + split5[2].ToString();
                yr5 = split5[2].ToString();
                m5 = split5[1].ToString();
                d5 = split5[0].ToString();
                datetoad = m5 + "/" + d5 + "/" + yr5;
                DateTime dt2 = Convert.ToDateTime(datetoad);

                if (dt2 > dtnow1)
                {
                    lblmessagefule.Visible = false;
                    lblmessagefule.Text = "Please Enter Valid To Date";
                    lblmessagefule.Visible = true;
                    txtarivaldate.Text = DateTime.Now.ToString("dd/MM/yyy");


                    goto label1;

                }
                else
                {
                    lblmessagefule.Visible = false;

                }
            }






            if (txt_date.Text != "" && txtarivaldate.Text != "")
            {
                lblmessagefule.Visible = false;
                string datefad, dtfromad;
                string datefromad;
                string yr4, m4, d4;
                datefad = txt_date.Text.ToString();
                string[] split4 = datefad.Split(new Char[] { '/' });
                if (split4.Length == 3)
                {
                    datefromad = split4[0].ToString() + "/" + split4[1].ToString() + "/" + split4[2].ToString();
                    yr4 = split4[2].ToString();
                    m4 = split4[1].ToString();
                    d4 = split4[0].ToString();
                    dtfromad = m4 + "/" + d4 + "/" + yr4;


                    string adatetoad;
                    string ayr5, am5, ad5;
                    date2ad = txtarivaldate.Text.ToString();
                    string[] asplit5 = date2ad.Split(new Char[] { '/' });
                    if (split5.Length == 3)
                    {
                        adatetoad = asplit5[0].ToString() + "/" + asplit5[1].ToString() + "/" + asplit5[2].ToString();
                        ayr5 = asplit5[2].ToString();
                        am5 = asplit5[1].ToString();
                        ad5 = asplit5[0].ToString();
                        adatetoad = am5 + "/" + ad5 + "/" + ayr5;
                        DateTime dt1 = Convert.ToDateTime(dtfromad);
                        DateTime dt2 = Convert.ToDateTime(adatetoad);

                        TimeSpan ts = dt2 - dt1;

                        int days = ts.Days;
                        if (days < 0)
                        {
                            txtarivaldate.Text = DateTime.Now.ToString("dd/MM/yyy");
                            txt_date.Text = DateTime.Now.ToString("dd/MM/yyy");
                            lblmessagefule.Text = "From Date Can't Be Greater Than To Date";

                            lblmessagefule.Visible = true;


                            btnprintmaster.Visible = false;
                        }
                    }
                }

            }

        label1: ;
        }
        catch
        {

        }
    }

    void Bind_Routes()
    {
        DataSet dsrr = new DataSet();
        dsrr.Clear();
        con.Close();
        con.Open();
        int count_items = 0;
        string cmd_bind_route = "select distinct r.Route_ID from routemaster r,vehicle_master v where r.Route_id=v.Route and v.Veh_Id in('" + ddlvecid.SelectedValue + "') ";

        dsrr = obj.select_method_wo_parameter(cmd_bind_route, "Text");
        ddl_routeid.Items.Clear();
        if (dsrr.Tables[0].Rows.Count > 0)
        {
            ddl_routeid.DataSource = dsrr;
            ddl_routeid.DataTextField = "Route_ID";
            ddl_routeid.DataBind();
            Bind_driver();

        }
        else
        {
            ddldriver.Items.Clear();
            ddlstartplace.Items.Clear();
            ddldescplace.Items.Clear();
        }
    }

    void Bind_driver()
    {
        DataSet dsrr = new DataSet();
        dsrr.Clear();
        con.Close();
        con.Open();
        int count_items = 0;
        if (ddl_routeid.Items.Count > 0)
        {
            //existing
            //string cmd_bind_route = "select distinct Staff_Name,staff_code from driverallotment  where Vehicle_Id in ('" + ddlvecid.SelectedItem.Text + "') and Route_Id in ('" + ddl_routeid.SelectedItem.Text + "') ";
            //new modified by prabha on jan 31 2018
            string cmd_bind_route = "select da.Staff_Code,sm.staff_name from DriverAllotment da,staffmaster sm where sm.staff_code=da.Staff_Code and da.Vehicle_Id in('" + ddlvecid.SelectedItem.Text + "') and da.Route_Id in ('" + ddl_routeid.SelectedItem.Text + "')  ";

            dsrr = obj.select_method_wo_parameter(cmd_bind_route, "Text");
            ddldriver.Items.Clear();
            if (dsrr.Tables[0].Rows.Count > 0)
            {
                ddldriver.DataSource = dsrr;
                ddldriver.DataTextField = "Staff_Name";
                ddldriver.DataValueField = "staff_code";
                ddldriver.DataBind();
                Bind_startplace();
            }
        }

    }

    void Bind_startplace()
    {
        //  string routeid = "";
        //if(ddl_routeid.Items.Count>0)
        //{
        //    routeid = ddl_routeid.SelectedItem.Text.ToString();
        //}
        DataSet dsrr = new DataSet();
        dsrr.Clear();
        con.Close();
        con.Open();
        int count_items = 0;
        ddlstartplace.Items.Clear();
        ddldescplace.Items.Clear();
        if (ddl_routeid.Items.Count > 0)
        {
            //string cmd_bind_route = "select distinct sm.Stage_Name,sm.Stage_id from stage_master sm, RouteMaster rm where sm.Stage_id=rm.Stage_Name and Route_ID in ('" + ddl_routeid.SelectedItem.Text + "') ";

            string cmd_bind_route = "select distinct sm.Stage_Name,sm.Stage_id,rm.Rou_From,rm.Rou_To from stage_master sm, RouteMaster rm where cast(sm.Stage_id as varchar(100))=cast(rm.Stage_Name as varchar(100)) and Route_ID in ('" + ddl_routeid.SelectedItem.Text + "') "; //modified by rajasekar 08/09/2018

            dsrr = obj.select_method_wo_parameter(cmd_bind_route, "Text");
            ddlstartplace.Items.Clear();
            ddldescplace.Items.Clear();
            if (dsrr.Tables[0].Rows.Count > 0)
            {
                ddlstartplace.DataSource = dsrr;
                ddlstartplace.DataTextField = "Stage_Name";
                ddlstartplace.DataValueField = "Stage_id";
                ddlstartplace.DataBind();

                ddldescplace.DataSource = dsrr;
                ddldescplace.DataTextField = "Stage_Name";
                ddldescplace.DataValueField = "Stage_id";
                ddldescplace.DataBind();

                //added by rajasekar 08/09/2018
                string starplace=dsrr.Tables[0].Rows[0]["Rou_From"].ToString();

                string endplace = dsrr.Tables[0].Rows[0]["Rou_To"].ToString();

                for (int i = 0; i < ddlstartplace.Items.Count; i++)
                {
                    if (ddlstartplace.Items[i].Value.ToString().ToLower().Trim() == starplace.ToLower().Trim())
                    {
                        ddlstartplace.SelectedIndex = i;
                    }
                }

                for (int i = 0; i < ddldescplace.Items.Count; i++)
                {
                    if (ddldescplace.Items[i].Value.ToString().ToLower().Trim() == endplace.ToLower().Trim())
                    {
                        ddldescplace.SelectedIndex = i;
                    }
                }
                //===============================//
            }
        }

    }

    protected void Ddlvehicleid_SelectedIndexChanged(object sender, EventArgs e)
    {
        Ddlvehicleid.Text = Ddlvehicleid.SelectedValue.ToString();
        txtclosingkm.Text = "";
        txttravellingkm.Text = "";
        if (Ddlvehicleid.SelectedItem.Text != "")
        {
            if (Ddlvehicleid.SelectedItem.Text == "All")
            {
                string strquery = "select reg_no from Vehicle_Master";
                ds = obj.select_method_wo_parameter(strquery, "Text");
                ddlregno.DataSource = ds;
                ddlregno.DataValueField = "reg_no";
                ddlregno.DataTextField = "reg_no";
                ddlregno.DataBind();
                ddlpurpose.Attributes.Add("onfocus", "subu()");

            }
            else
            {
                string strquery = "select reg_no from Vehicle_Master where Veh_ID='" + Ddlvehicleid.SelectedItem.Text + "'";
                ds = obj.select_method_wo_parameter(strquery, "Text");
                ddlregno.DataSource = ds;
                ddlregno.DataValueField = "reg_no";
                ddlregno.DataTextField = "reg_no";
                ddlregno.DataBind();
                ddlpurpose.Attributes.Add("onfocus", "subu()");
            }
        }
    }

    protected void txt_closekm_TextChanged(object sender, EventArgs e)
    {
        if (txt_closekm.Text != "")
        {
            // string ascj = txt_openkm.Text.ToString();

            double open_km = Convert.ToDouble(txt_openkm.Text);
            double close_km = Convert.ToDouble(txt_closekm.Text);
            //  txt_openkm.Text = Convert.ToString(open_km);
            if (close_km < open_km)
            {
               // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Closing Kilometer should not be less than Opening kilometer.')", true);

                alertpopwindow.Visible = true;
                lblalerterr.Text = "Closing Kilometer should not be less than Opening kilometer.";
                return;
            }

            txt_expensekm.Text = Convert.ToString(close_km - open_km);

        }
        else
        {
            txt_expensekm.Text = "";
        }
    }

    protected void txtclosingkm_TextChanged(object sender, EventArgs e)
    {
        if (txtclosingkm.Text != "")
        {
            int open_km = Convert.ToInt32(Txtopeingkm.Text);
            int close_km = Convert.ToInt32(txtclosingkm.Text);

            if (close_km < open_km)
            {
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Closing Kilometer should not be less than Opening kilometer.')", true);
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Closing Kilometer should not be less than Opening kilometer.";
                return;
            }

            txttravellingkm.Text = Convert.ToString(close_km - open_km);

        }
        else
        {
            txttravellingkm.Text = "";
        }
        ddlpurpose.Attributes.Add("onfocus", "subu()");
    }

    public void page_details()
    {
        ddlexpensestype.Attributes.Add("onfocus", "ftalukp()");
        //txt_remarks.Attributes.Add("onfocus", "fremarks()");
    }

    public void add()
    {
        ddlpurpose.Attributes.Add("onfocus", "add()");
    }

    protected void Fpmaintenance_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string active = e.CommandArgument.ToString();
            string[] splitdata = active.Split(',');
            string[] splitdata1 = splitdata[0].Split('=');

            string actrow = splitdata1[1].ToString();
            if (actrow != "-1" && actrow.Trim() != "" && actrow != null)
            {
                //mpemsgboxsave.Show();
                gerrow = int.Parse(actrow);
                if (gerrow >= 0)
                {
                    string vaid = Fpmaintenance.Sheets[0].Cells[gerrow, 2].Text.ToString();
                    string type = Fpmaintenance.Sheets[0].Cells[gerrow, 1].Text.ToString();
                    string date = Fpmaintenance.Sheets[0].Cells[gerrow, 3].Text.ToString();
                    string[] sd = date.Split('/');
                    DateTime dtdate = Convert.ToDateTime(sd[1] + '/' + sd[0] + '/' + sd[2]);
                    string national = Fpmaintenance.Sheets[0].Cells[gerrow, 4].Text.ToString();
                    string companyname = Fpmaintenance.Sheets[0].Cells[gerrow, 5].Text.ToString();
                    string billno = Fpmaintenance.Sheets[0].Cells[gerrow, 6].Text.ToString();
                    string billdate = Fpmaintenance.Sheets[0].Cells[gerrow, 7].Text.ToString();
                    string[] sd1 = date.Split('/');
                    DateTime dtdate1 = Convert.ToDateTime(sd1[1] + '/' + sd1[0] + '/' + sd1[2]);
                    string clokm = Fpmaintenance.Sheets[0].Cells[gerrow, 8].Text.ToString();
                    string description = Fpmaintenance.Sheets[0].Cells[gerrow, 9].Text.ToString();

                    string deleteqery = "delete from  Vehicle_Usage where Vehicle_Type='" + type + "' and Vehicle_Id='" + vaid + "' and date='" + dtdate + "' and Purpose='" + national + "' and companyname='" + companyname + "' and billno='" + billno + "' and billdate='" + dtdate1 + "' and Closing_Km='" + clokm + "' and description='" + description + "'";
                    int delete = obj.update_method_wo_parameter(deleteqery, "Text");
                }
                btnMainGo_Click(sender, e);

            }
        }
        catch
        {
        }

    }

    protected void btnMainGo_Click(object sender, EventArgs e)
    {
        FpfuelReport.Visible = false;
        Fpfueldetails.Visible = false;
        string vech_all = string.Empty;
        string sess_all = string.Empty;
        string route_all = string.Empty;
        string stage_all = string.Empty;
        string display_all = string.Empty;
        string stage_header = string.Empty;
        string route_header = string.Empty;
        string[] split_fDate = txtfrm_date.Text.Split(new char[] { '/' });
        string set_date_from = split_fDate[1] + '/' + split_fDate[0] + '/' + split_fDate[2];

        string[] split_tDate = txtend_date.Text.Split(new char[] { '/' });
        string set_date_to = split_tDate[1] + '/' + split_tDate[0] + '/' + split_tDate[2];

        frmdate = Convert.ToDateTime(set_date_from);
        todate = Convert.ToDateTime(set_date_to);

        if (todate < frmdate)
        {
            Fpmaintance.Visible = false;
            errmsg.Text = "Todate must be greater than or equal to Fromdate.";
            errmsg.Visible = true;
            return;
        }

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

        if (ddl_report.Text == "General")
        {
            con.Close();
            con.Open();

            SqlCommand cmd_km_count = new SqlCommand("select vehicle_type,vehicle_id,sum(travel_km) as km_total,sum(fuel) as fu_total from vehicle_usage  where  vehicle_id in('" + vech_all + "')  and vehicle_type<>'' and date between '" + frmdate + "' and '" + todate + "'  group by vehicle_type,vehicle_id order by Vehicle_type,vehicle_id", con);
            SqlDataAdapter ad_km_count = new SqlDataAdapter(cmd_km_count);
            DataTable dt_km_count = new DataTable();
            ad_km_count.Fill(dt_km_count);

            con.Close();
            con.Open();
            SqlCommand cmd_veh_data;
            if (ddlselectcollege.Text == "All")
            {
                cmd_veh_data = new SqlCommand("select distinct * from Vehicle_Usage u,Vehicle_Master m where u.Vehicle_Id=m.Veh_ID and vehicle_id in('" + vech_all + "') and date between '" + frmdate + "' and '" + todate + "' and billno is null and billdate is null and companyname is null and description is null order by Vehicle_type,vehicle_id,date", con);
            }
            else
            {
                cmd_veh_data = new SqlCommand("select distinct * from Vehicle_Usage u,Vehicle_Master m where u.Vehicle_Id=m.Veh_ID and vehicle_id in('" + vech_all + "') and date between '" + frmdate + "' and '" + todate + "' and billno is null and billdate is null and companyname is null and description is null and college_code='" + ddlselectcollege.SelectedItem.Value + "' order by Vehicle_type,vehicle_id,date", con);
            }
            SqlDataAdapter ad_veh_data = new SqlDataAdapter(cmd_veh_data);
            DataTable dt_veh_data = new DataTable();
            ad_veh_data.Fill(dt_veh_data);

            if (dt_veh_data.Rows.Count > 0)
            {
                Fpmaintenance.Visible = false;
                Fpmaintance.Visible = false;
                Fpmaintance.Sheets[0].RowCount = 0;
                Fpmaintance.Visible = true;
                Fp_Fuel.Visible = false;
                int sno = 0;

                Fpmaintance.Sheets[0].ColumnCount = 17;
                Fpmaintance.Sheets[0].ColumnHeader.Cells[Fpmaintance.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "S.No";
                Fpmaintance.Sheets[0].ColumnHeader.Cells[Fpmaintance.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "Vehicle Type";
                Fpmaintance.Sheets[0].ColumnHeader.Cells[Fpmaintance.Sheets[0].ColumnHeader.RowCount - 1, 2].Text = "Vehicle Id";

                Fpmaintance.Sheets[0].ColumnHeader.Cells[Fpmaintance.Sheets[0].ColumnHeader.RowCount - 1, 3].Text = "Start Date";
                Fpmaintance.Sheets[0].ColumnHeader.Cells[Fpmaintance.Sheets[0].ColumnHeader.RowCount - 1, 4].Text = "Arrival Date";
                Fpmaintance.Sheets[0].ColumnHeader.Cells[Fpmaintance.Sheets[0].ColumnHeader.RowCount - 1, 5].Text = "Purpose";
                Fpmaintance.Sheets[0].ColumnHeader.Cells[Fpmaintance.Sheets[0].ColumnHeader.RowCount - 1, 6].Text = "Opening Km";
                Fpmaintance.Sheets[0].ColumnHeader.Cells[Fpmaintance.Sheets[0].ColumnHeader.RowCount - 1, 7].Text = "Closing Km";
                Fpmaintance.Sheets[0].ColumnHeader.Cells[Fpmaintance.Sheets[0].ColumnHeader.RowCount - 1, 8].Text = "Travelling KM";
                Fpmaintance.Sheets[0].ColumnHeader.Cells[Fpmaintance.Sheets[0].ColumnHeader.RowCount - 1, 9].Text = "Total KM";
                
                Fpmaintance.Sheets[0].ColumnHeader.Cells[Fpmaintance.Sheets[0].ColumnHeader.RowCount - 1, 10].Text = "Filled Fuel-(Lt)";
                Fpmaintance.Sheets[0].ColumnHeader.Cells[Fpmaintance.Sheets[0].ColumnHeader.RowCount - 1, 11].Text = "Total Fuel";
                Fpmaintance.Sheets[0].ColumnHeader.Cells[Fpmaintance.Sheets[0].ColumnHeader.RowCount - 1, 12].Text = "Mileage";
                Fpmaintance.Sheets[0].ColumnHeader.Cells[Fpmaintance.Sheets[0].ColumnHeader.RowCount - 1, 13].Text = "Fuel/Lt (Rs)";
                Fpmaintance.Sheets[0].ColumnHeader.Cells[Fpmaintance.Sheets[0].ColumnHeader.RowCount - 1, 14].Text = "Driver Name";
                Fpmaintance.Sheets[0].ColumnHeader.Cells[Fpmaintance.Sheets[0].ColumnHeader.RowCount - 1, 15].Text = "Remark";
                Fpmaintance.Sheets[0].ColumnHeader.Cells[Fpmaintance.Sheets[0].ColumnHeader.RowCount - 1, 16].Text = "Delete";

                FarPoint.Web.Spread.ButtonCellType btnremove = new FarPoint.Web.Spread.ButtonCellType();
                btnremove.Text = "Remove";
                btnremove.CommandName = "Fpmaintance_ButtonCommand";
                Fpmaintance.Sheets[0].Columns[16].CellType = btnremove;

                Fpmaintance.Sheets[0].ColumnHeader.Cells[Fpmaintance.Sheets[0].ColumnHeader.RowCount - 1, 0].Column.HorizontalAlign = HorizontalAlign.Center;

                Fpmaintance.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpmaintance.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);

                Fpmaintance.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpmaintance.Sheets[0].SetColumnMerge(9, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpmaintance.Sheets[0].SetColumnMerge(11, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpmaintance.Sheets[0].SetColumnMerge(13, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpmaintance.Sheets[0].SetColumnMerge(14, FarPoint.Web.Spread.Model.MergePolicy.Always);

                for (int i = 0; i < dt_veh_data.Rows.Count; i++)
                {
                    string[] split_Date = dt_veh_data.Rows[i]["Date"].ToString().Split(new char[] { ' ' });
                    string date1 = split_Date[0].ToString();
                    string mileage = "";
                    string[] spl_date1 = date1.Split(new char[] { '/' });
                    string set_date = spl_date1[1] + '/' + spl_date1[0] + '/' + spl_date1[2];
                    string[] split_Date2 = dt_veh_data.Rows[i]["arrivalpdate"].ToString().Split(new char[] { ' ' });
                    string date2 = split_Date2[0].ToString();
                    string[] spl_date2 = date2.Split(new char[] { '/' });
                    string arrivalpdate = spl_date2[1] + '/' + spl_date2[0] + '/' + spl_date2[2];
                    sno++;
                    string veh_type = dt_veh_data.Rows[i]["Vehicle_Type"].ToString();
                    string vehid = dt_veh_data.Rows[i]["Vehicle_id"].ToString();
                    string date = set_date;
                    string travel = dt_veh_data.Rows[i]["Travel_Km"].ToString();
                    string purpose = dt_veh_data.Rows[i]["Purpose"].ToString();
                    string fuel = dt_veh_data.Rows[i]["Fuel"].ToString();
                    string routeid = dt_veh_data.Rows[i]["Route_ID"].ToString();
                    string item = dt_veh_data.Rows[i]["item"].ToString();
                    string fuel_rs = Math.Round(Convert.ToDouble(dt_veh_data.Rows[i]["fuelamount"].ToString()), 2).ToString(); 
                    
                    string drivername = d2.GetFunction("select sm.staff_name from DriverAllotment da,staffmaster sm where sm.staff_code=da.Staff_Code and da.Vehicle_Id in('"+vehid+"') and da.Route_Id in ('" + routeid + "')");
                    //string staffcode = dt_veh_data.Rows[i]["staffcode"].ToString();
                    //string starplace = dt_veh_data.Rows[i]["startplace"].ToString();
                    //string endplace = dt_veh_data.Rows[i]["arrivalplace"].ToString();
                    //string vmmillage = d2.GetFunction("  select Mileage from vehicle_master where Veh_Id ='" + vehid + "'");
                    if (travel.ToString() == "0")
                    {
                        fuel = "1";
                        mileage = Math.Round((Convert.ToDouble(travel) / Convert.ToDouble(fuel)), 2).ToString();
                        fuel = "0";
                    }
                    else
                    {
                        mileage = Math.Round((Convert.ToDouble(travel) / Convert.ToDouble(fuel)), 2).ToString();
                    }
                    string openingKm = dt_veh_data.Rows[i]["Opening_Km"].ToString();
                    string closingkm = dt_veh_data.Rows[i]["Closing_Km"].ToString();

                    string tot_km = string.Empty;
                    string tot_fuel = string.Empty;

                    DataView dv_count = new DataView();
                    dt_km_count.DefaultView.RowFilter = "vehicle_id='" + vehid + "'";
                    dv_count = dt_km_count.DefaultView;

                    if (dv_count.Count > 0)
                    {
                        tot_km = dv_count[0]["km_total"].ToString();
                        tot_fuel = dv_count[0]["fu_total"].ToString();
                    }

                    Fpmaintance.Sheets[0].RowCount = Convert.ToInt32(Fpmaintance.Sheets[0].RowCount) + 1;

                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 1].Text = veh_type;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                    // Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 2].Text = vehid;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 2].Tag = routeid;
                    // Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 2].Tag = staffcode;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;


                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 3].Text = date;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 4].Text = arrivalpdate;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 5].Text = purpose;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 6].Text = openingKm;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 7].Text = closingkm;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 8].Text = travel;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 9].Text = tot_km;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 9].VerticalAlign = VerticalAlign.Middle;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 10].Text = fuel;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 10].VerticalAlign = VerticalAlign.Middle;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 11].Text = tot_fuel;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Center;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 11].VerticalAlign = VerticalAlign.Middle;

                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 12].Text = mileage;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Center;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 12].VerticalAlign = VerticalAlign.Middle;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 13].Text = fuel_rs;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 13].HorizontalAlign = HorizontalAlign.Center;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 13].VerticalAlign = VerticalAlign.Middle;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 14].Text = drivername;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 14].HorizontalAlign = HorizontalAlign.Left;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 14].VerticalAlign = VerticalAlign.Middle;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 15].Text = item;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 15].HorizontalAlign = HorizontalAlign.Center;
                    Fpmaintance.Sheets[0].Cells[Fpmaintance.Sheets[0].RowCount - 1, 15].VerticalAlign = VerticalAlign.Middle;
                }

                Fpmaintance.Sheets[0].PageSize = Fpmaintance.Sheets[0].RowCount;
            }
            else
            {
                errmsg.Text = "No data found";
                errmsg.Visible = true;
                Fpmaintance.Sheets[0].RowCount = 0;
                Fp_Fuel.Sheets[0].RowCount = 0;
                errmsg.Font.Bold = true;
                Fpmaintance.Visible = false;
                Fp_Fuel.Visible = false;
            }
        }
        else if (ddl_report.Text == "Fuel Consumption")
        {
            con.Close();
            con.Open();

            SqlCommand cmd_km_count = new SqlCommand("select vehicle_type,vehicle_id,date,sum(travel_km) as km_total,sum(fuel) as fu_total from vehicle_usage  where  vehicle_id in('" + vech_all + "') and date between '" + frmdate + "' and '" + todate + "' group by vehicle_type,vehicle_id,date order by Vehicle_type,vehicle_id,date", con);
            SqlDataAdapter ad_km_count = new SqlDataAdapter(cmd_km_count);
            DataTable dt_km_count = new DataTable();
            ad_km_count.Fill(dt_km_count);

            if (dt_km_count.Rows.Count > 0)
            {
                Fpmaintenance.Visible = false;
                Fpmaintance.Visible = false;
                Fp_Fuel.Visible = true;
                Fpmaintance.Visible = false;
                Fp_Fuel.Sheets[0].RowCount = 0;
                Fp_Fuel.Sheets[0].ColumnCount = 9;
                Fp_Fuel.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "S.No";
                Fp_Fuel.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "Vehicle Type";
                Fp_Fuel.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 2].Text = "Vehicle Id";
                Fp_Fuel.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 3].Text = "Date";
                Fp_Fuel.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 4].Text = "Mileage";
                Fp_Fuel.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 5].Text = "Travel KM";
                Fp_Fuel.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 6].Text = "Total Fuel-(Lt)";
                Fp_Fuel.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 7].Text = "Remaining KM";
                Fp_Fuel.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 8].Text = "Total Remaining KM";

                Fp_Fuel.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 0].Column.HorizontalAlign = HorizontalAlign.Center;

                Fp_Fuel.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fp_Fuel.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                //Fp_Fuel.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpmaintance.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fp_Fuel.Sheets[0].SetColumnMerge(8, FarPoint.Web.Spread.Model.MergePolicy.Always);

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

                        string[] split_Date = dt_km_count.Rows[i]["Date"].ToString().Split(new char[] { ' ' });
                        string date1 = split_Date[0].ToString();

                        string[] spl_date1 = date1.Split(new char[] { '/' });
                        string set_date = spl_date1[1] + '/' + spl_date1[0] + '/' + spl_date1[2];

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
                        double actual_km = Convert.ToDouble(travel_km);

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

                        Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 3].Text = set_date;

                        Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 4].Text = mileage;
                        Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                        Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 5].Text = travel_km;
                        Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;

                        Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 6].Text = fuel_tot;
                        Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;

                        Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 7].Text = remain_km_x.ToString();
                        Fp_Fuel.Sheets[0].Cells[Fp_Fuel.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;

                    }
                    Fp_Fuel.Sheets[0].PageSize = Fp_Fuel.Sheets[0].RowCount;
                    Fp_Fuel.Visible = true;
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
                            Fp_Fuel.Sheets[0].Cells[k, 8].CellType = lbl_cell;

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

                            if (Convert.ToDouble(Fp_Fuel.Sheets[0].Cells[k, 8].Text.ToString()) <= 10)
                            {
                                Fp_Fuel.Sheets[0].Cells[k, 2].BackColor = Color.Salmon;
                                Fp_Fuel.Sheets[0].Cells[k, 3].BackColor = Color.Salmon;
                                Fp_Fuel.Sheets[0].Cells[k, 4].BackColor = Color.Salmon;
                                Fp_Fuel.Sheets[0].Cells[k, 5].BackColor = Color.Salmon;
                                Fp_Fuel.Sheets[0].Cells[k, 6].BackColor = Color.Salmon;
                                Fp_Fuel.Sheets[0].Cells[k, 7].BackColor = Color.Salmon;
                                Fp_Fuel.Sheets[0].Cells[k, 8].BackColor = Color.Salmon;

                            }
                        }
                    }

                }
            }
            else
            {
                errmsg.Text = "No data found";
                errmsg.Visible = true;
                Fpmaintance.Sheets[0].RowCount = 0;
                Fp_Fuel.Sheets[0].RowCount = 0;
                errmsg.Font.Bold = true;
                Fpmaintance.Visible = false;
                Fp_Fuel.Visible = false;
            }
        }
        else if (ddl_report.Text == "Fuel Report")
        {
            try
            {
                Fpmaintenance.Visible = false;
                Fpmaintance.Visible = false;
                Fpmaintance.Visible = false;
                Fp_Fuel.Visible = false;
                FpfuelReport.Visible = true;
                FpfuelReport.Sheets[0].RowCount = 0;
                FpfuelReport.Sheets[0].ColumnCount = 12;
                FpfuelReport.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "S.No";
                FpfuelReport.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "Vehicle Type";
                FpfuelReport.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 2].Text = "Vehicle Id";
                FpfuelReport.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 3].Text = "Route Id";
                FpfuelReport.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 4].Text = "Starting Stage";
                FpfuelReport.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 5].Text = "Ending Stage";
                FpfuelReport.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 6].Text = "Total Fuel(Lt)";
                FpfuelReport.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 7].Text = "Total Km";
                FpfuelReport.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 8].Text = "Millage";
                FpfuelReport.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 9].Text = "Item";
                FpfuelReport.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 10].Text = "View";
                FpfuelReport.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 11].Text = "Visit Place";
                // FpfuelReport.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 12].Text = "Running Status";

                Fpfueldetails.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

                FarPoint.Web.Spread.ButtonCellType btn = new FarPoint.Web.Spread.ButtonCellType();
                btn.Text = "View";
                btn.CommandName = "FpfuelReport_ButtonClickHandler";
                FarPoint.Web.Spread.ButtonCellType btn1 = new FarPoint.Web.Spread.ButtonCellType();
                btn1.Text = "Visit";
                btn1.CommandName = "FpfuelReport_ButtonClickHandler";

                FarPoint.Web.Spread.ButtonCellType btn2 = new FarPoint.Web.Spread.ButtonCellType();
                btn2.Text = "Running Status";
                btn2.CommandName = "FpfuelReport_ButtonClickHandler";
                string vstrvechicleid = "";
                vech_all = "";

                for (int vech_count = 0; vech_count < vehiclechecklist.Items.Count; vech_count++)
                {
                    if (vehiclechecklist.Items[vech_count].Selected == true)
                    {
                        if (vech_all == "")
                        {
                            vech_all = "'" + vehiclechecklist.Items[vech_count].Text + "'";
                        }
                        else
                        {
                            vech_all = vech_all + ",'" + vehiclechecklist.Items[vech_count].Text + "'";
                        }
                    }
                }

                if (vech_all != "")
                {
                    vstrvechicleid = " where VehicleID in(" + vech_all + ")";
                }
                string strquery = "select distinct VehicleID  from VTSGPRSData " + vstrvechicleid + "";

                DataSet dsvehicle = obj.select_method_wo_parameter(strquery, "Text");

                if (dsvehicle.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsvehicle.Tables[0].Rows.Count; i++)
                    {
                        string vechid = dsvehicle.Tables[0].Rows[i]["VehicleID"].ToString();
                        string iddetails = "select distinct s.Stage_Name,r.Stage_Name,v.Mileage,v.Veh_Type,r.Route_ID  from RouteMaster r,Stage_Master s,vehicle_master v where v.Veh_ID=r.Veh_ID and  s.Stage_id=r.Stage_Name and  r.Veh_ID='" + vechid + "' and (Dep_Time='Halt' or Arr_Time='halt')";
                        DataSet dsdet = obj.select_method_wo_parameter(iddetails, "Text");
                        Double milage = 0;
                        string type = "", Startpalec = "", Endplaec = "", routeid = "", totfuel = "";
                        if (dsdet.Tables[0].Rows.Count > 0)
                        {

                            type = dsdet.Tables[0].Rows[0]["Veh_Type"].ToString();
                            Endplaec = dsdet.Tables[0].Rows[0]["Stage_Name"].ToString();
                            routeid = dsdet.Tables[0].Rows[0]["Route_ID"].ToString();
                            string mil = dsdet.Tables[0].Rows[0]["Mileage"].ToString();
                            if (mil != null && mil.Trim() != "")
                            {
                                milage = Convert.ToDouble(dsdet.Tables[0].Rows[0]["Mileage"].ToString());
                            }
                            if (dsdet.Tables[0].Rows.Count > 1)
                            {

                                Startpalec = dsdet.Tables[0].Rows[1]["Stage_Name"].ToString();
                            }
                        }

                        double travelkm = 0, totalfuel = 0;


                        string fdate = txtfrm_date.Text.ToString();
                        string tdate = txtend_date.Text.ToString();
                        string[] spfd = fdate.Split('/');
                        string[] sptd = tdate.Split('/');
                        DateTime dtfrom = Convert.ToDateTime(spfd[1] + '/' + spfd[0] + '/' + spfd[2]);
                        DateTime dtto = Convert.ToDateTime(sptd[1] + '/' + sptd[0] + '/' + sptd[2]);

                        totfuel = obj.GetFunction("select SUM(fuel) from Vehicle_Usage where Vehicle_Id='" + vechid + "' and DATE between '" + dtfrom.ToString() + "' and '" + dtto.ToString() + "'");
                        if (totfuel.Trim() != "" && totfuel != null)
                        {
                            totalfuel = Convert.ToDouble(totfuel);
                        }

                        for (DateTime dt = dtfrom; dt <= dtto; dt = dt.AddDays(1))
                        {
                            double openkm = 0, closekm = 0, gettravel = 0;
                            string[] spdat = dt.ToString().Split(' ');
                            string[] getdate = spdat[0].ToString().Split('/');
                            string year = getdate[2].ToString();
                            string setyear = "" + year[2] + "" + year[3] + "";
                            string date = getdate[1].ToString();
                            if (date.Length == 1)
                            {
                                date = "0" + date;
                            }
                            string mon = getdate[0].ToString();
                            if (mon.Length == 1)
                            {
                                mon = "0" + mon;
                            }
                            string gprsdate = date + mon + setyear.ToString();
                            string getopeing = obj.GetFunction("select MAX(odometer) from VTSGPRSData where vehicleid='" + vechid + "' and date < '" + gprsdate + "'");
                            string getclose = obj.GetFunction("select MAX(odometer) from VTSGPRSData where vehicleid='" + vechid + "' and date = '" + gprsdate + "'");
                            if (getopeing.Trim() != "" && getopeing != null && getopeing.Trim() != "0")
                            {
                                openkm = Convert.ToDouble(getopeing);
                            }
                            else
                            {
                                getopeing = obj.GetFunction("select isnull(Intial_Km,0) as opkm from Vehicle_Master where Veh_ID='" + vechid + "'");
                                if (getopeing.Trim() != "" && getopeing != null)
                                {
                                    openkm = Convert.ToDouble(getopeing);
                                }
                            }
                            if (getclose.Trim() != "" && getclose != null)
                            {
                                closekm = Convert.ToDouble(getclose);
                            }
                            if (openkm < closekm)
                            {
                                gettravel = closekm - openkm;
                            }
                            if (gettravel > 0)
                            {
                                travelkm = travelkm + gettravel;
                            }
                        }
                        if (totalfuel > 0 && travelkm > 0)
                        {
                            milage = travelkm / totalfuel;
                        }
                        milage = Math.Round(milage, 0);
                        FpfuelReport.Sheets[0].RowCount++;
                        FpfuelReport.Sheets[0].Cells[FpfuelReport.Sheets[0].RowCount - 1, 0].Text = FpfuelReport.Sheets[0].RowCount.ToString();
                        FpfuelReport.Sheets[0].Cells[FpfuelReport.Sheets[0].RowCount - 1, 1].Text = type.ToString();
                        FpfuelReport.Sheets[0].Cells[FpfuelReport.Sheets[0].RowCount - 1, 2].Text = vechid;
                        FpfuelReport.Sheets[0].Cells[FpfuelReport.Sheets[0].RowCount - 1, 3].Text = routeid;
                        FpfuelReport.Sheets[0].Cells[FpfuelReport.Sheets[0].RowCount - 1, 4].Text = Startpalec.ToString();
                        FpfuelReport.Sheets[0].Cells[FpfuelReport.Sheets[0].RowCount - 1, 5].Text = Endplaec.ToString();
                        FpfuelReport.Sheets[0].Cells[FpfuelReport.Sheets[0].RowCount - 1, 6].Text = totalfuel.ToString();
                        FpfuelReport.Sheets[0].Cells[FpfuelReport.Sheets[0].RowCount - 1, 7].Text = travelkm.ToString();
                        FpfuelReport.Sheets[0].Cells[FpfuelReport.Sheets[0].RowCount - 1, 8].Text = milage.ToString();
                        // FpfuelReport.Sheets[0].Cells[FpfuelReport.Sheets[0].RowCount - 1, 9].Text = item.ToString();
                        //FpfuelReport.Sheets[0].Cells[FpfuelReport.Sheets[0].RowCount - 1,10 ].CellType = btn;
                        // FpfuelReport.Sheets[0].Cells[FpfuelReport.Sheets[0].RowCount - 1, 11].CellType = btn1;
                        FpfuelReport.Sheets[0].Cells[FpfuelReport.Sheets[0].RowCount - 1, 13].CellType = btn;
                        // FpfuelReport.Sheets[0].Cells[FpfuelReport.Sheets[0].RowCount - 1, 12].Text = item.ToString();
                        FpfuelReport.Sheets[0].Cells[FpfuelReport.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpfuelReport.Sheets[0].Cells[FpfuelReport.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpfuelReport.Sheets[0].Cells[FpfuelReport.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpfuelReport.Sheets[0].Cells[FpfuelReport.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpfuelReport.Sheets[0].Cells[FpfuelReport.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
                else
                {
                    errmsg.Text = "No Records found";
                    errmsg.Visible = true;
                    FpfuelReport.Sheets[0].RowCount = 0;
                    Fp_Fuel.Sheets[0].RowCount = 0;
                    errmsg.Font.Bold = true;
                    FpfuelReport.Visible = false;
                    FpfuelReport.Visible = false;
                }
                FpfuelReport.Sheets[0].PageSize = FpfuelReport.Sheets[0].RowCount;
            }
            catch
            {
            }
        }
        else if (ddl_report.Text == "Maintenance")
        {
            string sql = "";
            Fpmaintenance.Visible = true;
            Fp_Fuel.Visible = false;
            FpfuelReport.Visible = false;
            Fpmaintance.Visible = false;
            Fpmaintenance.Sheets[0].RowCount = 0;
            Fpmaintenance.Sheets[0].RowHeader.Visible = false;
            //Fpmaintenance.CommandBar.Visible = false;
            Fpmaintenance.Sheets[0].AutoPostBack = true;
            Fpmaintenance.Height = 200;
            Fpmaintenance.Width = 820;
            Fpmaintenance.Sheets[0].ColumnCount = 12;
            Fpmaintenance.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            Fpmaintenance.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            Fpmaintenance.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            Fpmaintenance.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Vehicle Type";
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Vehicle Id";
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 3].Text = " Date ";
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Nature of work/Purpose";
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Company Name";
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 6].Text = "  Bill No";
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 7].Text = " Bill Date";
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Closing Km";
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 9].Text = " Description/Item";
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 10].Text = " Amount";
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 11].Text = " Delete";
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
            Fpmaintenance.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
            Fpmaintenance.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            FarPoint.Web.Spread.ButtonCellType btnremove = new FarPoint.Web.Spread.ButtonCellType();
            btnremove.Text = "Remove";
            btnremove.CommandName = "Fpmaintance_ButtonCommand";
            Fpmaintenance.Sheets[0].Columns[11].CellType = btnremove;
            if (ddlselectcollege.Text == "All")
            {
                if (vech_all == "")
                {
                    sql = "select Vehicle_Type, Vehicle_Id,convert(varchar(10),Date,103) as Date ,Purpose,companyname,billno,convert(varchar(10),billdate,103) as billdate ,Closing_Km,description,(case when round(finalcost,2) is null then 0 else round(finalcost,2) end + case when round(totalfinalcost,2) is null then 0 else round(totalfinalcost,2) end )  as finalcost from Vehicle_Usage u,Vehicle_Master m where u.Vehicle_Id=m.Veh_ID and registerno!='' and Date  between '" + frmdate + "' and '" + todate + "'";
                }
                else
                {
                    sql = "select Vehicle_Type,Vehicle_Id, convert(varchar(10),Date,103) as Date ,Purpose,companyname,billno,convert(varchar(10),billdate,103) as billdate ,Closing_Km,description,(case when round(finalcost,2) is null then 0 else round(finalcost,2) end + case when round(totalfinalcost,2) is null then 0 else round(totalfinalcost,2) end ) as finalcost  from Vehicle_Usage u,Vehicle_Master m where u.Vehicle_Id=m.Veh_ID and registerno!='' and Vehicle_Id in('" + vech_all + "') and Date  between '" + frmdate + "' and '" + todate + "' ";
                }
            }
            else
            {
                if (vech_all == "")
                {
                    sql = "select Vehicle_Type, Vehicle_Id,convert(varchar(10),Date,103) as Date ,Purpose,companyname,billno,convert(varchar(10),billdate,103) as billdate ,Closing_Km,description,(case when round(finalcost,2) is null then 0 else round(finalcost,2) end + case when round(totalfinalcost,2) is null then 0 else round(totalfinalcost,2) end )  as finalcost from Vehicle_Usage u,Vehicle_Master m where u.Vehicle_Id=m.Veh_ID and registerno!='' and Date  between '" + frmdate + "' and '" + todate + "' and college_code='" + ddlselectcollege.SelectedItem.Value + "'";
                }
                else
                {
                    sql = "select Vehicle_Type,Vehicle_Id, convert(varchar(10),Date,103) as Date ,Purpose,companyname,billno,convert(varchar(10),billdate,103) as billdate ,Closing_Km,description,(case when round(finalcost,2) is null then 0 else round(finalcost,2) end + case when round(totalfinalcost,2) is null then 0 else round(totalfinalcost,2) end ) as finalcost  from Vehicle_Usage u,Vehicle_Master m where u.Vehicle_Id=m.Veh_ID and registerno!='' and Vehicle_Id in('" + vech_all + "') and Date  between '" + frmdate + "' and '" + todate + "' and college_code='" + ddlselectcollege.SelectedItem.Value + "'";
                }
            }
            DataSet maintain = new DataSet();
            maintain = obj.select_method_wo_parameter(sql, "text");
            if (maintain.Tables[0].Rows.Count > 0)
            {
                int c = 0;
                for (int i = 0; i < maintain.Tables[0].Rows.Count; i++)
                {
                    c++;
                    Fpmaintenance.Sheets[0].RowCount++;
                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 0].Text = c.ToString();
                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 1].Text = maintain.Tables[0].Rows[i]["Vehicle_Type"].ToString();
                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;

                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 2].Text = maintain.Tables[0].Rows[i]["Vehicle_Id"].ToString();
                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 3].Text = maintain.Tables[0].Rows[i]["Date"].ToString();
                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;

                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 4].Text = maintain.Tables[0].Rows[i]["Purpose"].ToString();
                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;

                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 5].Text = maintain.Tables[0].Rows[i]["companyname"].ToString();
                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;

                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 6].Text = maintain.Tables[0].Rows[i]["billno"].ToString();
                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;

                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 7].Text = maintain.Tables[0].Rows[i]["billdate"].ToString();
                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;

                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 8].Text = maintain.Tables[0].Rows[i]["Closing_Km"].ToString();
                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;

                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 9].Text = maintain.Tables[0].Rows[i]["description"].ToString();
                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;

                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 10].Text = maintain.Tables[0].Rows[i]["finalcost"].ToString();
                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Right;

                    Fpmaintenance.Sheets[0].Cells[Fpmaintenance.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                    Fpmaintenance.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpmaintenance.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpmaintenance.Sheets[0].SetColumnMerge(8, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpmaintenance.Sheets[0].SetColumnMerge(9, FarPoint.Web.Spread.Model.MergePolicy.Always);

                    Fpmaintenance.Sheets[0].PageSize = Fpmaintenance.Sheets[0].RowCount;


                }

                Fpmaintenance.Sheets[0].Columns[0].Locked = true;
                Fpmaintenance.Sheets[0].Columns[1].Locked = true;
                Fpmaintenance.Sheets[0].Columns[2].Locked = true;
                Fpmaintenance.Sheets[0].Columns[3].Locked = true;
                Fpmaintenance.Sheets[0].Columns[4].Locked = true;
                Fpmaintenance.Sheets[0].Columns[5].Locked = true;
                Fpmaintenance.Sheets[0].Columns[6].Locked = true;
                Fpmaintenance.Sheets[0].Columns[7].Locked = true;
                Fpmaintenance.Sheets[0].Columns[8].Locked = true;
            }
            else
            {
                errmsg.Text = "No Records Found";
                errmsg.Visible = true;
                Fpmaintenance.Sheets[0].RowCount = 0;

                errmsg.Font.Bold = true;
                Fpmaintenance.Visible = false;

            }





        }
    }

    protected void Fpmaintance_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string ar = e.CommandArgument.ToString();

            string[] spitval = ar.Split(',');
            string[] spitrow = spitval[0].Split('=');


            string row = spitrow[1].ToString();

            string activerow = Fpmaintance.ActiveSheetView.ActiveRow.ToString();
            if (row != "-1" && row.Trim() != "" && row != null)
            {
                mpemsgboxsave.Show();
                gerrow = int.Parse(row);
                remove(gerrow);
                btnMainGo_Click(sender, e);

            }

        }
        catch (Exception ex)
        {
        }
    }

    bool Cellclick;        //added by raghul on 29/12/207
    protected void Fpmaintance_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string activerow = Fpmaintance.ActiveSheetView.ActiveRow.ToString();
        string activecol = Fpmaintance.ActiveSheetView.ActiveColumn.ToString();
        Cellclick = true;
        Accordion1.SelectedIndex = 1;
        rdbfuel.Checked = true;
    }

    protected void Fpmaintenance_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

        string activerow = Fpmaintenance.ActiveSheetView.ActiveRow.ToString();
        string activecol = Fpmaintenance.ActiveSheetView.ActiveRow.ToString();
        Cellclick = true;
        Accordion1.SelectedIndex = 1;
        Rdbmailtaince.Checked = true;

    }

    protected void Fpmaintance_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddl_report.Text == "General")
            {

                string activerow = Fpmaintance.ActiveSheetView.ActiveRow.ToString();
                string activecol = Fpmaintance.ActiveSheetView.ActiveColumn.ToString();

                Accordion1.SelectedIndex = 2;
                string openingkm = Fpmaintance.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text;
                txt_openkm.Text = openingkm;
                if (Cellclick == true)
                {


                    //txt_openkm.Text = "";
                    new_page();
                    txt_closekm.Enabled = false;
                    ddl_vehtype.Enabled = false;
                    ddlvecid.Enabled = false;
                    ddl_routeid.Enabled = false;
                    lbl_add.Text = "Modify";
                    // txt_openkm.Text = "";
                    activerow = Fpmaintance.ActiveSheetView.ActiveRow.ToString();
                    activecol = Fpmaintance.ActiveSheetView.ActiveColumn.ToString();
                    Fpmaintance.ActiveSheetView.Rows[int.Parse(activerow)].BackColor = Color.LightCyan;
                    string veh_type = Fpmaintance.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                    //ddl_vehtype.SelectedItem.Text = vehid;

                    for (int i = 0; i < ddl_vehtype.Items.Count; i++)
                    {
                        if (ddl_vehtype.Items[i].Text.ToString().ToLower().Trim() == veh_type.ToLower().Trim())
                        {
                            ddl_vehtype.SelectedIndex = i;
                        }
                    }
                    string vehid = Fpmaintance.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                    // ddlvecid.SelectedItem.Text = veh_type;

                    for (int i = 0; i < ddlvecid.Items.Count; i++)
                    {
                        if (ddlvecid.Items[i].Text.ToString().ToLower().Trim() == vehid.ToLower().Trim())
                        {
                            ddlvecid.SelectedIndex = i;
                        }
                    }

                    string date = Fpmaintance.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                    txt_date.Text = date;
                    string arivaldate = Fpmaintance.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
                    txtarivaldate.Text = arivaldate;
                    string purpose = Fpmaintance.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text;
                    ddlexpensestype.SelectedItem.Text = purpose;

                    string closingkm = Fpmaintance.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Text;
                    txt_closekm.Text = closingkm;
                    string travellingkm = Fpmaintance.Sheets[0].Cells[Convert.ToInt32(activerow), 8].Text;
                    txt_expensekm.Text = travellingkm;
                    string fuel = Fpmaintance.Sheets[0].Cells[Convert.ToInt32(activerow), 10].Text;
                    //added by prabha jan 03 2018
                    string Remarks = Fpmaintance.Sheets[0].Cells[Convert.ToInt32(activerow), Fpmaintance.Sheets[0].ColumnCount-2].Text.Trim();


                    for (int i = 0; i < ddl_itemtype.Items.Count; i++)
                    {
                        if (ddl_itemtype.Items[i].Text.ToString().ToLower().Trim() == Remarks.ToLower().Trim())
                        {
                            ddl_itemtype.SelectedIndex = i;
                        }
                    }
                    

                    txt_fuel.Text = fuel;
                    btn_save.Visible = false;
                    Btnupdate.Visible = true;
                    btn_delete1.Visible = false;



                    //string Veh_ID = Fpmaintance.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                    //string date = Fpmaintance.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;

                    //string[] spl_date = date.Split('/');

                    //date = spl_date[1] + "/" + spl_date[0] + "/" + spl_date[2];

                    //con.Close();
                    //con.Open();
                    //SqlCommand cmd_edit = new SqlCommand("Select * from Vehicle_Usage where Vehicle_id='" + Veh_ID + "' and date='" + date + "'", con);
                    //SqlDataAdapter ad_edit = new SqlDataAdapter(cmd_edit);
                    //DataTable dt_edit = new DataTable();
                    //ad_edit.Fill(dt_edit);

                    //if (dt_edit.Rows.Count > 0)
                    //{
                    //    string[] split_Date = dt_edit.Rows[0]["Date"].ToString().Split(new char[] { ' ' });
                    //    string date1 = split_Date[0].ToString();

                    //    string[] spl_date1 = date1.Split(new char[] { '/' });
                    //    string set_date = spl_date1[1] + '/' + spl_date1[0] + '/' + spl_date1[2];

                    //    //ddl_vehtype.Text = dt_edit.Rows[0]["Vehicle_Type"].ToString();
                    //    ddlvecid.Text = dt_edit.Rows[0]["Vehicle_Id"].ToString();
                    //    txt_date.Text = set_date;
                    //    txt_expensekm.Text = dt_edit.Rows[0]["Travel_Km"].ToString();
                    //    ddlexpensestype.SelectedItem.Text = dt_edit.Rows[0]["Purpose"].ToString();
                    //    txt_fuel.Text = dt_edit.Rows[0]["Fuel"].ToString();

                    //    lbl_add.Text = "Modify";

                    //    btn_save.Text = "Update";
                    //    Panel5.Visible = false;
                    //    Panel1.Visible = true;
                    //    Accordion1.SelectedIndex = 1;

                    //    //btn_delete.Enabled = true;
                    //}
                    //Get_Opening_KM();

                    string routeid = Fpmaintance.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag.ToString();
                    string query = " select fuelamount,totalamount,remarks,staffcode,startplace,arrivalplace,convert(nvarchar(20),startpdate,103) as sdate,convert(nvarchar(20),arrivalpdate,103) as adate ,startptime,arrivalptime from Vehicle_Usage  where Vehicle_Id='" + vehid + "' and Route_ID='" + routeid + "' and closing_km="+closingkm;//rajasekar 2/5
                    DataSet dt_veh_data = new DataSet();
                    dt_veh_data.Clear();
                    dt_veh_data = obj.select_method_wo_parameter(query, "Text");
                    btn_delete1.Visible = true;
                    btn_delete1.Enabled = true;
                    //string staffcode = Fpmaintance.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag.ToString();
                    //string starplace = Fpmaintance.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag.ToString();
                    //string endplace = Fpmaintance.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag.ToString();
                    if (dt_veh_data.Tables[0].Rows.Count > 0)
                    {
                        string staffcode = dt_veh_data.Tables[0].Rows[0]["staffcode"].ToString();
                        string starplace = dt_veh_data.Tables[0].Rows[0]["startplace"].ToString();
                        string endplace = dt_veh_data.Tables[0].Rows[0]["arrivalplace"].ToString();
                        string startdate = dt_veh_data.Tables[0].Rows[0]["sdate"].ToString();
                        string enddate = dt_veh_data.Tables[0].Rows[0]["adate"].ToString();
                        string remarks = dt_veh_data.Tables[0].Rows[0]["remarks"].ToString();

                        string fuelamt = dt_veh_data.Tables[0].Rows[0]["fuelamount"].ToString();
                        txtfuelltrs.Text = fuelamt;
                        string fueltotalamt = dt_veh_data.Tables[0].Rows[0]["totalamount"].ToString();
                        Txtfuelamount.Text = fueltotalamt;

                        txtrm.Text = remarks;
                        string startdatetime = Convert.ToString(Convert.ToDateTime(Convert.ToString(dt_veh_data.Tables[0].Rows[0]["startptime"])).ToString("hh:mm tt"));
                        string enddatetime = Convert.ToString(Convert.ToDateTime(Convert.ToString(dt_veh_data.Tables[0].Rows[0]["arrivalptime"])).ToString("hh:mm tt"));

                        string[] splitstartdatetime = startdatetime.Split(':');

                        string hr = splitstartdatetime[0].ToString();
                        string minutes = splitstartdatetime[1].ToString();
                        splitstartdatetime = minutes.Split(' ');
                        minutes = splitstartdatetime[0].ToString();
                        string ampm = splitstartdatetime[1].ToString();

                        for (int i = 0; i < ddlhour.Items.Count; i++)
                        {
                            if (ddlhour.Items[i].Text.ToString().ToLower().Trim() == hr.ToLower().Trim())
                            {
                                ddlhour.SelectedIndex = i;
                            }
                        }

                        for (int i = 0; i < ddlmin.Items.Count; i++)
                        {
                            if (ddlmin.Items[i].Text.ToString().ToLower().Trim() == minutes.ToLower().Trim())
                            {
                                ddlmin.SelectedIndex = i;
                            }
                        }

                        for (int i = 0; i < ddlsession.Items.Count; i++)
                        {
                            if (ddlsession.Items[i].Text.ToString().ToLower().Trim() == ampm.ToLower().Trim())
                            {
                                ddlsession.SelectedIndex = i;
                            }
                        }
                        splitstartdatetime = enddatetime.Split(':');

                        hr = splitstartdatetime[0].ToString();
                        minutes = splitstartdatetime[1].ToString();
                        splitstartdatetime = minutes.Split(' ');
                        minutes = splitstartdatetime[0].ToString();
                        ampm = splitstartdatetime[1].ToString();

                        for (int i = 0; i < ddlendhour.Items.Count; i++)
                        {
                            if (ddlendhour.Items[i].Text.ToString().ToLower().Trim() == hr.ToLower().Trim())
                            {
                                ddlendhour.SelectedIndex = i;
                            }
                        }

                        for (int i = 0; i < ddlendmin.Items.Count; i++)
                        {
                            if (ddlendmin.Items[i].Text.ToString().ToLower().Trim() == minutes.ToLower().Trim())
                            {
                                ddlendmin.SelectedIndex = i;
                            }
                        }

                        for (int i = 0; i < ddlenssession.Items.Count; i++)
                        {
                            if (ddlenssession.Items[i].Text.ToString().ToLower().Trim() == ampm.ToLower().Trim())
                            {
                                ddlenssession.SelectedIndex = i;
                            }
                        }

                        Bind_Routes();
                        Bind_driver();
                        Bind_startplace();

                        for (int i = 0; i < ddl_routeid.Items.Count; i++)
                        {
                            if (ddl_routeid.Items[i].Text.ToString().ToLower().Trim() == routeid.ToLower().Trim())
                            {
                                ddl_routeid.SelectedIndex = i;
                            }
                        }
                        for (int i = 0; i < ddldriver.Items.Count; i++)
                        {
                            if (ddldriver.Items[i].Value.ToString().ToLower().Trim() == staffcode.ToLower().Trim())
                            {
                                ddldriver.SelectedIndex = i;
                            }
                        }

                        for (int i = 0; i < ddlstartplace.Items.Count; i++)
                        {
                            if (ddlstartplace.Items[i].Value.ToString().ToLower().Trim() == starplace.ToLower().Trim())
                            {
                                ddlstartplace.SelectedIndex = i;
                            }
                        }

                        for (int i = 0; i < ddldescplace.Items.Count; i++)
                        {
                            if (ddldescplace.Items[i].Value.ToString().ToLower().Trim() == endplace.ToLower().Trim())
                            {
                                ddldescplace.SelectedIndex = i;
                            }
                        }
                    }
                    //string 
                }
                //else
                //{
                //    ddl_vehtype.Enabled = true;
                //    ddlvecid.Enabled = true;
                //    ddl_routeid.Enabled = true;
                //}
            }
        }
        catch
        {
        }

    }

    //private void new_page()
    //{
    //    throw new NotImplementedException();
    //}
    protected void Btnupdate_click(object sender, EventArgs e)
    {
        try
        {
            string fuelltr = txt_fuel.Text.ToString();
            if (txt_fuel.Text.ToString() != "")
            {
                fuelltr = txt_fuel.Text.ToString();
            }
            else
            {
                fuelltr = "0";
            }

            string fuelltr_rs = txtfuelltrs.Text.ToString();
            if (txtfuelltrs.Text.ToString() != "")
            {
                fuelltr_rs = txtfuelltrs.Text.ToString();
            }
            else
            {
                fuelltr_rs = "0";
            }

            string fuelltr_tot_rs = Txtfuelamount.Text.ToString();
            if (Txtfuelamount.Text.ToString() != "")
            {
                fuelltr_tot_rs = Txtfuelamount.Text.ToString();
            }
            else
            {
                fuelltr_tot_rs = "0";
            }

            string route_idd = ddl_routeid.SelectedItem.Text;
            string staff_code = ddldriver.SelectedItem.Value;
            string startplace = ddlstartplace.SelectedItem.Value;
            string endplace = ddldescplace.SelectedItem.Value;
            string remarks = txtrm.Text;
            string datest = txt_date.Text.ToString();
            string[] split2 = datest.Split(new Char[] { '/' });

            string date1 = txtarivaldate.Text.ToString();
            string[] split = date1.Split(new Char[] { '/' });

            string newadate = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
            string newsdate = split2[1].ToString() + "-" + split2[0].ToString() + "-" + split2[2].ToString();
            string endTime = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString() + " " + ddlendhour.SelectedValue + ":" + ddlendmin.SelectedValue + ": 00 " + ddlenssession.SelectedValue;
            string startTime = split2[1].ToString() + "-" + split2[0].ToString() + "-" + split2[2].ToString() + " " + ddlhour.SelectedValue + ":" + ddlmin.SelectedValue + ": 00 " + ddlsession.SelectedValue;
            DateTime startime1 = Convert.ToDateTime(startTime);
            DateTime endtime1 = Convert.ToDateTime(endTime);
            TimeSpan span = endtime1.Subtract(startime1);

            if (startime1 >= endtime1)
            {
                //lblerror1.Visible = true;
                //lblerror1.Text = "Expected Time Entry Time Should Not Lesser Expected Time Exit";
                //return;
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Enter Valid Date')", true);

                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Enter Valid Date";
                return;
            }


            vechileid = ddlvecid.SelectedValue.ToString();

            string[] split_Date = txt_date.Text.Split(new char[] { '/' });
            string set_date = split_Date[1] + '/' + split_Date[0] + '/' + split_Date[2];

            date = Convert.ToDateTime(set_date);
            string strquery = "update Vehicle_Usage set Vehicle_Id='" + ddlvecid.SelectedItem.Text + "',Date='" + date + "',Fuel='" + txt_fuel.Text + "',fuelamount='" + fuelltr_rs + "',totalamount='" + fuelltr_tot_rs + "',Opening_Km='" + txt_openkm.Text + "',Closing_Km='" + txt_closekm.Text + "',Travel_Km='" + txt_expensekm.Text + "',Purpose='" + ddlexpensestype.SelectedItem.Text + "',item='" + ddl_itemtype.SelectedItem.Text + "',startplace='" + startplace + "' ,arrivalplace='" + endplace + "' ,startptime='" + startime1 + "' ,arrivalptime='" + endtime1 + "' ,remarks='" + remarks + "',staffcode='" + staff_code + "' ,startpdate='" + newsdate + "' ,arrivalpdate='" + newadate + "'   where Vehicle_Type='" + ddl_vehtype.SelectedItem.Text + "'  and Route_ID='" + route_idd + "' and startpdate='" + newsdate + "'  and arrivalpdate='" + newadate + "'";
            int ds = obj.update_method_wo_parameter(strquery, "Text");
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Updated Successfully')", true);

            alertpopwindow.Visible = true;
            lblalerterr.Text = "Updated Successfully";
            // new_page();
        }
        catch
        {
        }
    }

    void split_date(string date)
    {

        string[] split_fDate = date.Split(new char[] { ' ' });
        string date1 = split_fDate[0].ToString();

        string[] spl_date1 = date1.Split(new char[] { '/' });
        string set_date = spl_date1[1] + '/' + spl_date1[0] + '/' + spl_date1[2];

    }

    protected void typeadd_Click(object sender, EventArgs e)
    {
        Paneladd.Visible = true;

        // Paneladd.Attributes.Add("style", "width:230px; height:70px; top:183px; left:px; position: absolute;");
        newcaption.InnerHtml = "Type Of Expense";
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        Panel3.Visible = false;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        TextBox1.Text = ddlpurpose.SelectedItem.Text;
    }

    protected void typeremove_Click(object sender, EventArgs e)
    {
        con.Close();
        con.Open();

        SqlCommand cmd_delete = new SqlCommand("delete from textvaltable_new where Textval='" + ddlexpensestype.SelectedItem.ToString() + "'", con);
        int a = cmd_delete.ExecuteNonQuery();

        bindtypeofexpense();
        bindtypeofexpense1();

    }

    protected void remarksadd_Click(object sender, EventArgs e)
    {

    }

    protected void remarksremove_Click(object sender, EventArgs e)
    {

    }

    protected void btn_save_click(object sender, EventArgs e)
    {
        savedetails();
        if (waytonewpage == true)
        {
            waytonewpage = false;
            new_page();
            bindtypeofexpense();
            bindtypeofexpense1();
        }
        Accordion1.SelectedIndex = 0;//added by rajasekar 10/09/2018
    }

    public void savedetails()
    {
        try
        {


            if (ddlvecid.Text != "--Select--")
            {
                if (ddl_routeid.Items.Count == 0)
                {
                    lblmessagefule.Visible = true;
                    lblmessagefule.Text = "Please Allot Route For This Vehicle";
                    return;
                }

                if (ddldriver.Items.Count == 0)
                {
                    lblmessagefule.Visible = true;
                    lblmessagefule.Text = "Please Allot Driver For This Vehicle";
                    return;
                }
                if (ddlstartplace.Items.Count == 0)
                {
                    lblmessagefule.Visible = true;
                    lblmessagefule.Text = "Please Allot Start Place For This Vehicle";
                    return;
                }
                if (ddldescplace.Items.Count == 0)
                {
                    lblmessagefule.Visible = true;
                    lblmessagefule.Text = "Please Allot Designation Place For This Vehicle";
                    return;
                }

                string route_idd = ddl_routeid.SelectedItem.Text;
                string staff_code = ddldriver.SelectedItem.Value;
                string startplace = ddlstartplace.SelectedItem.Value;
                string endplace = ddldescplace.SelectedItem.Value;
                if (startplace == endplace)
                {
                    lblmessagefule.Visible = true;
                    lblmessagefule.Text = "Please Select Correct Designation Place For This Vehicle";
                    return;
                }
                lblmessagefule.Visible = false;

                string remarks = txtrm.Text;
                string datest = txt_date.Text.ToString();
                string[] split2 = datest.Split(new Char[] { '/' });

                string date1 = txtarivaldate.Text.ToString();
                string[] split = date1.Split(new Char[] { '/' });

                string newadate = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
                string newsdate = split2[1].ToString() + "-" + split2[0].ToString() + "-" + split2[2].ToString();
                string endTime = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString() + " " + ddlendhour.SelectedValue + ":" + ddlendmin.SelectedValue + ": 00 " + ddlenssession.SelectedValue;
                string startTime = split2[1].ToString() + "-" + split2[0].ToString() + "-" + split2[2].ToString() + " " + ddlhour.SelectedValue + ":" + ddlmin.SelectedValue + ": 00 " + ddlsession.SelectedValue;
                DateTime startime1 = Convert.ToDateTime(startTime);
                DateTime endtime1 = Convert.ToDateTime(endTime);
                TimeSpan span = endtime1.Subtract(startime1);

                if (startime1 >= endtime1)
                {
                    //lblerror1.Visible = true;
                    //lblerror1.Text = "Expected Time Entry Time Should Not Lesser Expected Time Exit";
                    //return;
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Enter Valid Date')", true);

                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter Valid Date";
                    return;
                }

                if (txt_closekm.Text == "")
                {
                    lblmessagefule.Text = "Please Enter Closing Km";
                    lblmessagefule.Visible = true;
                    return;
                }
                waytonewpage = true;
                vechileid = ddlvecid.SelectedValue.ToString();

                //fuel ltr and amount
                if (txt_fuel.Text == "")
                {
                    lblmessagefule.Text = "Please Enter Fuel(Lt)";
                    lblmessagefule.Visible = true;
                    return;
                }
                if (txtfuelltrs.Text == "")
                {
                    lblmessagefule.Text = "Please Enter Fuel/Lt (Rs)";
                    lblmessagefule.Visible = true;
                    return;
                }

                lbl_Validation.Visible = false;
                //ddlvecid.BorderColor = Color.Black;

                if (txt_date.Text != "")
                {
                    string[] split_Date = txt_date.Text.Split(new char[] { '/' });
                    string set_date = split_Date[1] + '/' + split_Date[0] + '/' + split_Date[2];

                    date = Convert.ToDateTime(set_date);
                    if (date > DateTime.Today)
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert( 'Please Choose Correct Date')", true);

                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please Choose Correct Date";
                    }
                    if (txt_closekm.Text == "")
                    {
                        lbl_Validation.Text = "Please Enter Closing Km";
                        lbl_Validation.Visible = true;
                    }
                    int close = Convert.ToInt32(txt_closekm.Text);
                    int start = Convert.ToInt32(txt_openkm.Text);
                    if (close < start)
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Closing Kilometer Should Not Be Less Than Opening Kilometer.')", true);

                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Closing Kilometer Should Not Be Less Than Opening Kilometer.";
                        return;
                    }
                    else
                    {
                        lbl_Validation.Visible = false;

                        if (txt_closekm.Text != "")
                        {
                            lbl_Validation.Visible = false;

                            typeofexpense = ddlexpensestype.SelectedValue.ToString();

                            expensekm = txt_expensekm.Text;

                            if (txt_fuel.Text != "")
                            {
                                fuel = txt_fuel.Text;
                            }

                            if (btn_save.Text == "Update")
                            {
                                con.Close();
                                con.Open();

                                SqlCommand cmd_delete = new SqlCommand("delete from vehicle_usage where vehicle_id='" + ddlvecid.Text + "' and date = '" + date + "'", con);
                                cmd_delete.ExecuteNonQuery();

                            }

                            lbl_Validation.Visible = false;
                            string vehtype = string.Empty;
                            con.Close();
                            con.Open();
                            SqlCommand cmd_veh_type = new SqlCommand("Select distinct Veh_Type from vehicle_master where veh_id='" + ddlvecid.Text + "' ", con);
                            SqlDataReader rdr_veh_type = cmd_veh_type.ExecuteReader();

                            while (rdr_veh_type.Read())
                            {
                                if (rdr_veh_type.HasRows == true)
                                {

                                    vehtype = (rdr_veh_type["Veh_Type"].ToString());

                                }
                            }

                            con.Close();
                            con.Open();
                            if (rdbfuel.Checked == true)
                            {
                                //SqlCommand insertdata = new SqlCommand("insert into Vehicle_Usage (Vehicle_Type,Vehicle_Id,Date,Travel_Km,Purpose,fuel,fuelamount,totalamount,Opening_km,Closing_km) values ('" + vehtype + "','" + vechileid + "','" + date + "','" + txt_expensekm.Text + "','" + ddlexpensestype.SelectedItem.ToString() + "','" + fuel + "','" + txtfuelltrs.Text.Trim() + "','" + Txtfuelamount.Text.Trim() + "','" + txt_openkm.Text + "','" + txt_closekm.Text + "')", con);
                                SqlCommand insertdata = new SqlCommand("insert into Vehicle_Usage (Vehicle_Type,Vehicle_Id,Date,Travel_Km,Purpose,fuel,fuelamount,totalamount,Opening_km,Closing_km,Route_ID,startplace,arrivalplace,startptime,arrivalptime,remarks,staffcode,startpdate,arrivalpdate,item) values ('" + vehtype + "','" + vechileid + "','" + date + "','" + txt_expensekm.Text + "','" + ddlexpensestype.SelectedItem.ToString() + "','" + fuel + "','" + txtfuelltrs.Text.Trim() + "','" + Txtfuelamount.Text.Trim() + "','" + txt_openkm.Text + "','" + txt_closekm.Text + "','" + route_idd + "','" + startplace + "','" + endplace + "','" + startime1 + "','" + endtime1 + "','" + remarks + "','" + staff_code + "','" + newsdate + "','" + newadate + "','" + ddl_itemtype.SelectedItem.Text.Trim() + "')", con);
                                //insertdata.ExecuteNonQuery();
                                insertdata.ExecuteNonQuery();
                                clear();
                            }

                            if (btn_save.Text == "Update")
                            {
                                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Updated Successfully')", true);

                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Updated Successfully";
                            }
                            else
                            {
                               // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);

                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Saved Successfully";
                            }

                        }
                        else
                        {
                            lbl_Validation.Visible = true;
                            lbl_Validation.Text = "Please enter Closing kilometer.";
                            //txt_expensekm.BorderColor = Color.Red;
                        }
                    }
                }
                else
                {
                    lbl_Validation.Visible = true;
                    lbl_Validation.Text = "Please select the date.";
                    //txt_date.BorderColor = Color.Red;
                }
            }

        }
        catch
        {

        }
    }

    protected void Btnsave_click(object sender, EventArgs e)
    {
        string vehtype = string.Empty;
        try
        {
            if (Ddlvehicleid.Text != "--Select--")
            {
                vechileid = Ddlvehicleid.SelectedValue.ToString();

                Lblwarning.Visible = false;
                //ddlvecid.BorderColor = Color.Black;

                if (Txtdate.Text != "")
                {
                    string[] split_Date = Txtdate.Text.Split(new char[] { '/' });
                    string set_date = split_Date[1] + '/' + split_Date[0] + '/' + split_Date[2];

                    date = Convert.ToDateTime(set_date);
                    if (date > DateTime.Today)
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert( 'Please Choose Correct Date')", true);


                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please Choose Correct Date";
                    }
                    if (txtclosingkm.Text == "")
                    {
                        lblerr1.Text = "Please Enter Closing Km";
                        lblerr1.Visible = true;
                    }
                    else
                    {

                        int close = Convert.ToInt32(txtclosingkm.Text);
                        int start = Convert.ToInt32(Txtopeingkm.Text);
                        lblerr1.Visible = false;
                        if (close < start)
                        {
                            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Closing Kilometer should not be less than Opening kilometer.')", true);

                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Closing Kilometer should not be less than Opening kilometer.";
                            return;
                        }
                        else
                        {
                            Lblwarning.Visible = false;

                            if (txtclosingkm.Text != "")
                            {
                                Lblwarning.Visible = false;
                                typeofexpense = ddlpurpose.SelectedValue.ToString();
                                expensekm = txt_expensekm.Text;
                                if (btn_save.Text == "Update")
                                {
                                    con.Close();
                                    con.Open();

                                    SqlCommand cmd_delete = new SqlCommand("delete from vehicle_usage where vehicle_id='" + ddlvecid.Text + "' and date = '" + date + "'", con);
                                    cmd_delete.ExecuteNonQuery();
                                }
                                Lblwarning.Visible = false;
                                con.Close();
                                con.Open();
                                SqlCommand cmd_veh_type = new SqlCommand("Select distinct Veh_Type from vehicle_master where Veh_Type='" + ddlvechicletype.SelectedValue + "' ", con);
                                SqlDataReader rdr_veh_type = cmd_veh_type.ExecuteReader();
                                while (rdr_veh_type.Read())
                                {
                                    if (rdr_veh_type.HasRows == true)
                                    {
                                        vehtype = (rdr_veh_type["Veh_Type"].ToString());
                                    }
                                }
                                con.Close();
                                con.Open();
                                if (Rdbmailtaince.Checked == true)
                                {
                                    Fpvehicle.SaveChanges();
                                    if (Rdbservice.Checked == true)
                                    {
                                        txt_fuel.Text = "";
                                        //txtFuelperLt.Text = "";
                                        if (txtdescription.Text == "")
                                        {
                                            lblerr1.Text = "Please Enter The Description";
                                            lblerr1.Visible = true;
                                        }
                                        else if (Txtfinalvat.Text == "")
                                        {
                                            lblerr1.Text = "Please Enter The Amount";
                                            lblerr1.Visible = true;
                                        }
                                        else
                                        {
                                            if (Txtvat.Text == "")
                                            {
                                                Txtvat.Text = "0";
                                            }
                                            string[] split_Date1 = Txtbilldate.Text.Split(new char[] { '/' });
                                            string set_date1 = split_Date1[1] + '/' + split_Date1[0] + '/' + split_Date1[2];
                                            DateTime bill = Convert.ToDateTime(set_date1);
                                            SqlCommand insertdata = new SqlCommand("insert into Vehicle_Usage (Vehicle_Type,Vehicle_Id,Date,Travel_Km,Purpose,fuel,Opening_km,Closing_km,repairamount,registerno,companyname,billno,billdate,tax,finalcost,description) values ('" + vehtype + "','" + vechileid + "','" + date + "','" + txttravellingkm.Text.Trim() + "','" + ddlpurpose.SelectedItem.ToString() + "','" + fuel + "','" + txt_openkm.Text + "','" + txtclosingkm.Text.Trim() + "','" + txtamount.Text.Trim() + "','" + ddlregno.SelectedItem.Text + "','" + txtcompanyname.Text + "','" + Txtbillno.Text + "','" + bill + "','" + Txtvat.Text + "','" + Txtfinalvat.Text + "','" + txtdescription.Text + "')", con);
                                            //insertdata.ExecuteNonQuery();
                                            insertdata.ExecuteNonQuery();
                                            clear();
                                            //TxtRepairExpenses.Text = "";
                                            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);

                                            alertpopwindow.Visible = true;
                                            lblalerterr.Text = "Saved Successfully";
                                            lblerr1.Visible = false;
                                            new_page();
                                            bindtypeofexpense1();
                                        }
                                    }
                                    if (Rdbitem.Checked == true)
                                    {
                                        Fpvehicle.SaveChanges();
                                        if (txtdescription.Text == "")
                                        {
                                            lblerr1.Text = "Please Enter The Description";
                                            lblerr1.Visible = true;
                                        }
                                        for (int i = 0; i < Fpvehicle.Sheets[0].Rows.Count - 1; i++)
                                        {
                                            string text = Fpvehicle.Sheets[0].Cells[i, 1].Text.ToString();
                                            if (text.ToString() == "")
                                            {
                                                lblerr1.Text = "Please Choose Item Name ";
                                                lblerr1.Visible = true;
                                            }
                                            else if (Txttotalamount.Text == "")
                                            {
                                                Txttotalamount.Text = "0";
                                            }
                                            else if (Txttax.Text == "")
                                            {
                                                Txttax.Text = "0";
                                            }
                                            else if (Txtfinalcost.Text == "")
                                            {
                                                lblerr1.Text = "Please Enter The Amount";
                                                lblerr1.Visible = true;
                                            }
                                            else
                                            {
                                                txt_fuel.Text = "";
                                                string[] split_Date1 = Txtbilldate.Text.Split(new char[] { '/' });
                                                string set_date1 = split_Date1[1] + '/' + split_Date1[0] + '/' + split_Date1[2];
                                                DateTime bill = Convert.ToDateTime(set_date1);
                                                string sqlqry = string.Empty;
                                                if (ddlregno.Items.Count > 0)  //modified by raghul dec 26 2017
                                                {
                                                    sqlqry = "insert into Vehicle_Usage (Vehicle_Type,Vehicle_Id,Date,Travel_Km,Purpose,fuel,Opening_km,Closing_km,totalamount,registerno,companyname,billno,billdate,totaltax,totalfinalcost,description,iemdescription) values ('" + vehtype + "','" + vechileid + "','" + date + "','" + txttravellingkm.Text.Trim() + "','" + ddlpurpose.SelectedItem.ToString() + "','" + fuel + "','" + txt_openkm.Text + "','" + txtclosingkm.Text.Trim() + "','" + Txttotalamount.Text.Trim() + "','" + ddlregno.SelectedItem.Text + "','" + txtcompanyname.Text + "','" + Txtbillno.Text + "','" + bill + "','" + Txttax.Text + "','" + Txtfinalcost.Text + "','" + text + "','" + txtdescription.Text + "')";
                                                }
                                                else
                                                {
                                                    sqlqry = "insert into Vehicle_Usage (Vehicle_Type,Vehicle_Id,Date,Travel_Km,Purpose,fuel,Opening_km,Closing_km,totalamount,registerno,companyname,billno,billdate,totaltax,totalfinalcost,description,iemdescription) values ('" + vehtype + "','" + vechileid + "','" + date + "','" + txttravellingkm.Text.Trim() + "','" + ddlpurpose.SelectedItem.ToString() + "','" + fuel + "','" + txt_openkm.Text + "','" + txtclosingkm.Text.Trim() + "','" + Txttotalamount.Text.Trim() + "','','" + txtcompanyname.Text + "','" + Txtbillno.Text + "','" + bill + "','" + Txttax.Text + "','" + Txtfinalcost.Text + "','" + text + "','" + txtdescription.Text + "')";
                                                }
                                                SqlCommand insertdata = new SqlCommand(sqlqry, con);
                                                insertdata.ExecuteNonQuery();
                                                clear();
                                                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);

                                                alertpopwindow.Visible = true;
                                                lblalerterr.Text = "Saved Successfully";
                                                lblerr1.Visible = false;
                                                new_page();
                                                bindtypeofexpense1();
                                            }
                                        }
                                    }
                                }
                                if (btn_save.Text == "Update")
                                {

                                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Updated Successfully')", true);

                                    alertpopwindow.Visible = true;
                                    lblalerterr.Text = "Updated Successfully";
                                }
                                else
                                {
                                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                                    //lblerr1.Visible = false;
                                }

                            }
                            else
                            {
                                Lblwarning.Visible = true;
                                Lblwarning.Text = "Please enter Closing kilometer.";
                                //txt_expensekm.BorderColor = Color.Red;
                            }
                        }
                    }
                }
            }
            else
            {
                Lblwarning.Visible = true;
                Lblwarning.Text = "Please select Vehicle Id.";
                //ddlvecid.BorderColor = Color.Red;
            }
        }
        catch
        {
        }
    }

    protected void btn_delete_click(object sender, EventArgs e)
    {

        if (txt_expensekm.Text != "")
        {
            lbl_Validation.Visible = false;

            //if (ddlexpensestype.Text != "") 
            //{
            string routeid = ddl_routeid.SelectedItem.Text.ToString();

            lbl_Validation.Visible = false;
            string[] split_Date = txt_date.Text.Split(new char[] { '/' });
            string set_date = split_Date[1] + '/' + split_Date[0] + '/' + split_Date[2];

            con.Close();
            con.Open();

            SqlCommand cmd_available = new SqlCommand("select * from vehicle_usage where vehicle_id='" + ddlvecid.SelectedItem.ToString() + "' and Route_ID='" + routeid + "' and date='" + set_date + "' and travel_km='" + txt_expensekm.Text + "'", con);
            SqlDataAdapter ad_available = new SqlDataAdapter(cmd_available);
            DataTable dt_available = new DataTable();
            ad_available.Fill(dt_available);

            if (dt_available.Rows.Count > 0)
            {

                con.Close();
                con.Open();

                SqlCommand cmd_delete_approval = new SqlCommand("delete from vehicle_usage where vehicle_id='" + ddlvecid.SelectedItem.ToString() + "' and Route_ID='" + routeid + "' and date='" + set_date + "' and travel_km='" + txt_expensekm.Text + "'", con);
                cmd_delete_approval.ExecuteNonQuery();

                //con.Close();
                //con.Open();
                //SqlCommand cmd_update = new SqlCommand("Update Vendor_Detailquotation set flag_status='false' where vendor_code='" + ddlvecid.SelectedItem.ToString() + "' and Actual_cost='" + txt_amt.Text + "' and Demand_date='" + set_date + "' and qty='" + txt_km.Text + "' and fee_code='" + ddl_ledger.SelectedValue + "'", con);
                //cmd_update.ExecuteNonQuery();

                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);

                alertpopwindow.Visible = true;
                lblalerterr.Text = "Deleted Successfully";

                btnMainGo_Click(sender, e);
                new_page();
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('This type of demand is not available.')", true);
                alertpopwindow.Visible = true;
                lblalerterr.Text = "This type of demand is not available.";
                new_page();
            }
            //    }


            //else
            //{
            //    lbl_Validation.Text = "Purpose should not be empty";
            //    lbl_Validation.Visible = true;
            //}
        }
        else
        {
            lbl_Validation.Text = "Kilometer should not be empty";
            lbl_Validation.Visible = true;
        }
    }

    protected void addnew_Click(object sender, EventArgs e)
    {
        caption = newcaption.InnerHtml;
        Paneladd.Visible = false;

        if (rdbfuel.Checked == true)
        {
            if (txt_addexpense.Text != "" && ddlexpensestype.Text == "")
            {
                Paneladd.Visible = false;
                hastab.Clear();

                hastab.Add("tcrit", "etype");
                hastab.Add("tval", txt_addexpense.Text.Trim());

                ds_expense = obj.select_method("enquiry_add_textcodenew", hastab, "sp");

                if (ds_expense.Tables.Count > 0)
                {
                    bindtypeofexpense();
                    lblerr.Visible = false;

                }
                else
                {
                    lblerr.Text = "Already Exists";
                    lblerr.Visible = true;
                }


                ddlexpensestype.SelectedIndex = ddlexpensestype.Items.IndexOf(ddlexpensestype.Items.FindByText(txt_addexpense.Text.Trim()));
                txt_addexpense.Text = "";
            }
        }

        if (Rdbmailtaince.Checked == true)
        {
            if (txt_addexpense.Text != "" && ddlpurpose.Text == "")
            {
                string str = txt_addexpense.Text;
                ddlpurpose.Items.Add(str);
                ddlpurpose.SelectedIndex = ddlpurpose.Items.IndexOf(ddlpurpose.Items.FindByText(txt_addexpense.Text.Trim()));
                //Paneladd.Visible = false;
                //hastab.Clear();

                //hastab.Add("tcrit", "etype");
                //hastab.Add("tval", txt_addexpense.Text.Trim());

                //ds_expense = obj.select_method("enquiry_add_textcodenew", hastab, "sp");

                //if (ds_expense.Tables.Count > 0)
                //{
                //    bindtypeofexpense();
                //    lblerr.Visible = false;

                //}
                //else
                //{
                //    lblerr.Text = "Already Exists";
                //    lblerr.Visible = true;
                //}


                //ddlpurpose.SelectedIndex = ddlpurpose.Items.IndexOf(ddlpurpose.Items.FindByText(txt_addexpense.Text.Trim()));
                //txt_addexpense.Text = "";
            }
        }
    }

    protected void Buttonadd1_Click(object sender, EventArgs e)
    {
        caption = newcaption.InnerHtml;
        Panel3.Visible = false;

        if (rdbfuel.Checked == true)
        {
            if (TextBox1.Text != "" && ddlpurpose.Text == "")
            {
                Panel3.Visible = false;
                hastab.Clear();

                hastab.Add("tcrit", "etype");
                hastab.Add("tval", txt_addexpense.Text.Trim());

                ds_expense = obj.select_method("enquiry_add_textcodenew", hastab, "sp");

                if (ds_expense.Tables.Count > 0)
                {
                    bindtypeofexpense1();
                    lblerr.Visible = false;

                }
                else
                {
                    lblerr.Text = "Already Exists";
                    lblerr.Visible = true;
                }


                ddlpurpose.SelectedIndex = ddlpurpose.Items.IndexOf(ddlpurpose.Items.FindByText(TextBox1.Text.Trim()));
                TextBox1.Text = "";
            }
        }

        if (Rdbmailtaince.Checked == true)
        {
            if (txt_addexpense.Text != "" && ddlpurpose.Text == "")
            {
                string str = txt_addexpense.Text;
                ddlpurpose.Items.Add(str);
                ddlpurpose.SelectedIndex = ddlpurpose.Items.IndexOf(ddlpurpose.Items.FindByText(txt_addexpense.Text.Trim()));
            }
        }
    }

    void clear()
    {
        txt_expensekm.Text = "";
        txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_fuel.Text = "";

    }

    protected void exitnew_Click(object sender, EventArgs e)
    {
        Paneladd.Visible = false;

    }

    protected void btn_reset_click(object sender, EventArgs e)
    {

    }

    protected void btn_cancel_click(object sender, EventArgs e)
    {
        new_page();
        bindtypeofexpense();
        bindtypeofexpense1();
    }

    void new_page()
    {
        txt_closekm.Enabled = true;
        ddl_vehtype.Enabled = true;
        ddlvecid.Enabled = true;
        ddl_routeid.Enabled = true;
        lbl_add.Text = "Add";
        btn_save.Visible = true;
        Btnupdate.Visible = false;
        btn_delete1.Visible = false;
        txtrm.Text = "";
        txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtarivaldate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        ddlhour.SelectedIndex = 0;
        ddlmin.SelectedIndex = 0;
        ddlsession.SelectedIndex = 0;

        ddlendhour.SelectedIndex = 0;
        ddlendmin.SelectedIndex = 0;
        ddlenssession.SelectedIndex = 0;


        txtfuelltrs.Text = "";
        // txt_openkm.Text = "";
        //Bind_Routes();
        //Bind_driver();
        //Bind_startplace();
        con.Close();
        con.Open();
        SqlCommand cmd_veh_type = new SqlCommand("Select distinct Veh_Type from vehicle_master", con);
        SqlDataReader rdr_veh_type = cmd_veh_type.ExecuteReader();

        ddl_vehtype.Items.Clear();
        ddl_vehtype.Items.Insert(0, "All");
        ddlvecid.Items.Insert(0, "All");
        //ddltype.Items.Insert(0, "All");
        Ddlvehicleid.Items.Clear();
        ddlvechicletype.Items.Clear();
        Ddlvehicleid.Items.Insert(0, "All");
        ddlvechicletype.Items.Insert(0, "All");
        Txtfuelamount.Text = "";
        int incre_type = 0;
        while (rdr_veh_type.Read())
        {
            if (rdr_veh_type.HasRows == true)
            {
                incre_type++;
                System.Web.UI.WebControls.ListItem list_veh_type = new System.Web.UI.WebControls.ListItem();
                list_veh_type.Text = (rdr_veh_type["Veh_Type"].ToString());

                ddl_vehtype.Items.Add(list_veh_type);
                //ddltype.Items.Add(list_veh_type);

            }
        }
        if (Rdbmailtaince.Checked == true)
        {
            txtamount.Text = "";
            txtclosingkm.Text = "";
            txttravellingkm.Text = "";
            ddlpurpose.Items.Clear();
            ddlregno.Items.Clear();
            txtcompanyname.Text = "";
            Txtbillno.Text = "";
            txtremarks.Text = "";
            Txtvat.Text = "";
            Txtfinalvat.Text = "";
            txtremarks.Text = "";
            Txttotalamount.Text = "";
            Txttax.Text = "";
            Txtfinalcost.Text = "";
            Fpvehicle.Sheets[0].RowCount = 0;
            txtdescription.Text = "";
        }

        ddl_vehtype.Enabled = true; ;
        ddlvecid.Enabled = true;
        txt_date.Enabled = true;
        btn_save.Enabled = true;
        btn_save.Text = "Save";

        bindvechicle();
        bindvechicle1();
        bindvechicletype();
        txt_expensekm.Text = "";
        //txtfuelltrs.Text = "";
        txt_closekm.Text = "";
        txtclosingkm.Text = "";
        txttravellingkm.Text = "";
        //txt_expensekm.Text = "";
        txt_fuel.Text = "";
        txt_closekm.Text = "";
        //txtFuelperLt.Text = "";
        //txtFuelDieselExpenses.Text = "";
        txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");

        Get_Opening_KM();
        Get_Opening_KM1();

    }

    protected void Btn_Close_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        Hashtable hat = new Hashtable();
        hat.Add(001, 002);
        hat.Remove(001);
        dt.Columns.Add("Name");
        dt.Columns.Add("Value");
        string a = dt.Rows[0][0].ToString();
        double adoub = Convert.ToDouble(a);
        double output = 0;
        if (double.TryParse(a, out output))
        {

        }
        ArrayList valuefield = new ArrayList();
        string valuealreadyexist = "";
        int count = 0;
        for (int i = 0; i < ddl_routeid.Items.Count; i++)
        {
            if (!valuefield.Contains(ddl_routeid.Items[i].Value.ToString()))
            {
                valuefield.Add(ddl_routeid.Items[i].Value.ToString());
            }
            else
            {
                dt.Rows.Add(ddl_routeid.Items[i].Text.ToString(), ddl_routeid.Items[i].Value.ToString());
                if (valuealreadyexist == "")
                {
                    valuealreadyexist = ddl_routeid.Items[i].Value.ToString();
                }
                else
                {
                    valuealreadyexist = valuealreadyexist + ";" + ddl_routeid.Items[i].Value.ToString();
                }

            }
            //valuefield.Remove(ddl_routeid.Items[i].Value.ToString());
        }

        //DataTable dt = new DataTable();
        //DataSet ds = new DataSet();
        //DataView dv = new DataView();


        //int dscount = ds.Tables[0].Rows.Count;
        //int dtcount = dt.Rows.Count;
        //int dvcount = dv.Count;

        //string dsvalue = ds.Tables[0].Rows[0]["Name"].ToString();
        //string dtvalue = dt.Rows[0]["Name"].ToString();
        //string dvvalue = dv[0]["Name"].ToString();

        Popup_Intimation.Hide();
    }

    void Intimation()
    {
        string cur_date = DateTime.Now.ToString("MM/dd/yyyy");

        string to_date = Convert.ToDateTime(cur_date).AddDays(7).ToString();

        string[] spl_cur_date = cur_date.Split(' ');

        string[] spl_to_date = to_date.Split(' ');

        con.Close();
        con.Open();

        SqlCommand cmd_intimation_licence = new SqlCommand("select * from driverallotment where renew_date between '" + cur_date + "' and '" + spl_to_date[0].ToString() + "'", con);
        SqlDataAdapter ad_intimation_licence = new SqlDataAdapter(cmd_intimation_licence);
        DataTable dt_intimation_licence = new DataTable();
        ad_intimation_licence.Fill(dt_intimation_licence);

        if (dt_intimation_licence.Rows.Count > 0)
        {
            Popup_Intimation.Show();
            Fp_Intimation_Driver.Visible = true;

            Fp_Intimation_Driver.Sheets[0].ColumnCount = 5;
            Fp_Intimation_Driver.Sheets[0].ColumnHeader.Cells[Fp_Intimation_Driver.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "S.No";
            Fp_Intimation_Driver.Sheets[0].ColumnHeader.Cells[Fp_Intimation_Driver.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "Staff Code";
            Fp_Intimation_Driver.Sheets[0].ColumnHeader.Cells[Fp_Intimation_Driver.Sheets[0].ColumnHeader.RowCount - 1, 2].Text = "Driver Name";
            Fp_Intimation_Driver.Sheets[0].ColumnHeader.Cells[Fp_Intimation_Driver.Sheets[0].ColumnHeader.RowCount - 1, 3].Text = "Description";
            Fp_Intimation_Driver.Sheets[0].ColumnHeader.Cells[Fp_Intimation_Driver.Sheets[0].ColumnHeader.RowCount - 1, 4].Text = "Renew Date";

            Fp_Intimation_Driver.Sheets[0].RowCount = 0;

            for (int i = 0; i < dt_intimation_licence.Rows.Count; i++)
            {
                Fp_Intimation_Driver.Sheets[0].RowCount++;
                Fp_Intimation_Driver.Sheets[0].Cells[i, 0].Text = Fp_Intimation_Driver.Sheets[0].RowCount.ToString();
                Fp_Intimation_Driver.Sheets[0].Cells[i, 1].Text = dt_intimation_licence.Rows[i]["Staff_Code"].ToString();
                Fp_Intimation_Driver.Sheets[0].Cells[i, 2].Text = dt_intimation_licence.Rows[i]["Staff_Name"].ToString();
                Fp_Intimation_Driver.Sheets[0].Cells[i, 3].Text = "Please Renew the Licence";

                string[] spl_renew = dt_intimation_licence.Rows[i]["Renew_Date"].ToString().Split(' ');

                string[] spl_date = spl_renew[0].Split('/');

                Fp_Intimation_Driver.Sheets[0].Cells[i, 4].Text = spl_date[1] + "/" + spl_date[0] + "/" + spl_date[2];
            }

        }
        else
        {
            Fp_Intimation_Driver.Visible = false;
        }

        con.Close();
        con.Open();

        SqlCommand cmd_intimation_vehicle = new SqlCommand("select veh_type,veh_id,nextins_date as ins,nextfcdate as fc,permit_date as permit from Vehicle_Insurance where CONVERT(Datetime, nextins_date, 120) between '" + cur_date + "' and '" + spl_to_date[0].ToString() + "' or CONVERT(Datetime, nextfcdate, 120) between '" + cur_date + "' and '" + spl_to_date[0].ToString() + "' or CONVERT(Datetime, permit_date, 120) between '" + cur_date + "' and '" + spl_to_date[0].ToString() + "' order by veh_id", con);
        SqlDataAdapter ad_intimation_vehicle = new SqlDataAdapter(cmd_intimation_vehicle);
        DataTable dt_intimation_vehicle = new DataTable();
        ad_intimation_vehicle.Fill(dt_intimation_vehicle);

        if (dt_intimation_vehicle.Rows.Count > 0)
        {
            Popup_Intimation.Show();

            Fp_Intimation_Vehicle.Visible = true;

            Fp_Intimation_Vehicle.Sheets[0].ColumnCount = 5;
            Fp_Intimation_Vehicle.Sheets[0].ColumnHeader.Cells[Fp_Intimation_Vehicle.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "S.No";
            Fp_Intimation_Vehicle.Sheets[0].ColumnHeader.Cells[Fp_Intimation_Vehicle.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "Vehicle Type";
            Fp_Intimation_Vehicle.Sheets[0].ColumnHeader.Cells[Fp_Intimation_Vehicle.Sheets[0].ColumnHeader.RowCount - 1, 2].Text = "Vehicle Id";
            Fp_Intimation_Vehicle.Sheets[0].ColumnHeader.Cells[Fp_Intimation_Vehicle.Sheets[0].ColumnHeader.RowCount - 1, 3].Text = "Description";
            Fp_Intimation_Vehicle.Sheets[0].ColumnHeader.Cells[Fp_Intimation_Vehicle.Sheets[0].ColumnHeader.RowCount - 1, 4].Text = "Renew Date";

            Fp_Intimation_Vehicle.Sheets[0].RowCount = 0;

            for (int i = 0; i < dt_intimation_vehicle.Rows.Count; i++)
            {
                Fp_Intimation_Vehicle.Sheets[0].RowCount++;
                Fp_Intimation_Vehicle.Sheets[0].Cells[i, 0].Text = Fp_Intimation_Vehicle.Sheets[0].RowCount.ToString();
                Fp_Intimation_Vehicle.Sheets[0].Cells[i, 1].Text = dt_intimation_vehicle.Rows[i]["veh_type"].ToString();
                Fp_Intimation_Vehicle.Sheets[0].Cells[i, 2].Text = dt_intimation_vehicle.Rows[i]["veh_id"].ToString();

                string description = string.Empty;
                string date = string.Empty;

                if (dt_intimation_vehicle.Rows[i]["ins"].ToString() != "")
                {
                    description = "Please Renew the Vehicle Insurance";
                    date = dt_intimation_vehicle.Rows[i]["ins"].ToString();
                }

                if (dt_intimation_vehicle.Rows[i]["fc"].ToString() != "")
                {
                    description = "Please Renew the Vehicle FC";
                    date = dt_intimation_vehicle.Rows[i]["fc"].ToString();
                }

                if (dt_intimation_vehicle.Rows[i]["permit"].ToString() != "")
                {
                    description = "Please Renew the Vehicle Permit";
                    date = dt_intimation_vehicle.Rows[i]["permit"].ToString();
                }

                Fp_Intimation_Vehicle.Sheets[0].Cells[i, 3].Text = description;

                Fp_Intimation_Vehicle.Sheets[0].Cells[i, 4].Text = date;
            }
        }

    }

    public void remove(int row)
    {
        try
        {
            if (row >= 0)
            {
                string vaid = Fpmaintance.Sheets[0].Cells[row, 2].Text.ToString();
                string type = Fpmaintance.Sheets[0].Cells[row, 1].Text.ToString();
                string date = Fpmaintance.Sheets[0].Cells[row, 3].Text.ToString();
                string[] sd = date.Split('/');
                DateTime dtdate = Convert.ToDateTime(sd[1] + '/' + sd[0] + '/' + sd[2]);
                string opnkm = Fpmaintance.Sheets[0].Cells[row, 6].Text.ToString();
                string clskm = Fpmaintance.Sheets[0].Cells[row, 7].Text.ToString();
                string trkm = Fpmaintance.Sheets[0].Cells[row, 8].Text.ToString();

                string deleteqery = "delete from Vehicle_Usage where Vehicle_Id='" + vaid + "' and Vehicle_Type='" + type + "' and DATE='" + dtdate + "' and Opening_Km='" + opnkm + "' and Closing_Km='" + clskm + "' and Travel_Km='" + trkm + "'";
                int delete = obj.update_method_wo_parameter(deleteqery, "Text");
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {

        if (Fpmaintance.Visible == true && Fpmaintance.Sheets[0].RowCount > 0)
        {
            string degreedetails = "General";
            string pagename = "Vehicle_Usage.aspx";
            Printcontrol.loadspreaddetails(Fpmaintance, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        else
            if (Fp_Fuel.Visible == true && Fp_Fuel.Sheets[0].RowCount > 0)
            {
                string degreedetails = "Fuel Consumption";
                string pagename = "Vehicle_Usage.aspx";
                Printcontrol.loadspreaddetails(Fp_Fuel, pagename, degreedetails);
                Printcontrol.Visible = true;
            }
            else if (Fpmaintance.Sheets[0].RowCount > 0 && Fpmaintance.Visible == true)
            {
                Session["column_header_row_count"] = 1;
                //string strdetails = "Batch Year:" + ddlbatch.Text.ToString() + "-" + "Degree:" + ddlbranch.Text.ToString() + "-" + "Semester:" + ddlsem.Text.ToString();

                //string strdetails1 = string.Empty;
                //strdetails1 = "Semester Information" + '@' + "Degree :" + ddlbatch.SelectedItem.ToString() + '-' + ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "-" + "Semester" + '-' + ddlsem.SelectedItem.ToString();
                //Fp_Fuel
                if (ddlselectcollege.SelectedItem.Text == "All")
                {
                    string query = obj.GetFunction("select com_name from collinfo");
                    string degreedetails = "College : " + " " + query;
                    string degreedetails1 = "Vehicle Fuel Details" + '@' + degreedetails;
                    string pagename = "Vehicle_Usage.aspx";
                    Printcontrol.loadspreaddetails(Fpmaintenance, pagename, degreedetails1);
                }
                else
                {
                    string degreedetails = "college:" + ddlselectcollege.SelectedItem.ToString() + "";
                    string degreedetails1 = "Vehicle Fuel Details" + '@' + "College :" + ddlselectcollege.SelectedItem.ToString() + "";
                    string pagename = "Vehicle_Usage.aspx";
                    Printcontrol.loadspreaddetails(Fpmaintance, pagename, degreedetails1);
                }
                Printcontrol.Visible = true;
            }
            else if (Fpfueldetails.Visible == true && Fpfueldetails.Sheets[0].RowCount > 0)
            {
                string degreedetails = "Vehicle Usage";
                string pagename = "Vehicle_Usage.aspx";
                Printcontrol.loadspreaddetails(Fpfueldetails, pagename, degreedetails);
                Printcontrol.Visible = true;
            }
            else if (FpfuelReport.Visible == true && FpfuelReport.Sheets[0].RowCount > 0)
            {
                string degreedetails = "Vehicle Usage";
                string pagename = "Vehicle_Usage.aspx";
                Printcontrol.loadspreaddetails(FpfuelReport, pagename, degreedetails);
                Printcontrol.Visible = true;
            }
            else if (Fpmaintenance.Visible == true && Fpmaintenance.Sheets[0].RowCount > 0)
            {
                if (ddlselectcollege.SelectedItem.Text == "All")
                {
                    string query = obj.GetFunction("select com_name from collinfo");
                    string degreedetails = "College : " + " " + query;
                    string degreedetails1 = "Vehicle Maintenance Details" + '@' + degreedetails;
                    string pagename = "Vehicle_Usage.aspx";
                    Printcontrol.loadspreaddetails(Fpmaintenance, pagename, degreedetails1);
                }
                else
                {
                    string degreedetails = "college:" + ddlselectcollege.SelectedItem.ToString() + "";

                    string degreedetails1 = "Vehicle Maintenance Details" + '@' + "College :" + ddlselectcollege.SelectedItem.ToString() + "";
                    string pagename = "Vehicle_Usage.aspx";
                    Printcontrol.loadspreaddetails(Fpmaintenance, pagename, degreedetails1);
                }
                Printcontrol.Visible = true;
            }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        mpemsgboxsave.Hide();
        remove(gerrow);
        btnMainGo_Click(sender, e);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mpemsgboxsave.Hide();
    }

    protected void FpfuelReport_ButtonClickHandler(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            errmsg.Visible = false;
            string ar = e.CommandArgument.ToString();

            string[] spitval = ar.Split(',');
            string[] spitrow = spitval[0].Split('=');
            string actrow = spitrow[1].ToString();

            string[] spiticol = spitval[1].Split('=');
            string[] spitvn = spiticol[1].Split('}');
            string actcol = spitvn[0].ToString();
            if (actrow != "-1" || actrow != "")
            {
                if (actcol == "9")
                {
                    Fpfueldetails.Visible = true;
                    FpfuelReport.Visible = false;
                    btnback.Visible = true;
                    Fpfueldetails.Sheets[0].RowCount = 0;
                    Fpfueldetails.Sheets[0].ColumnCount = 0;
                    Fpfueldetails.Sheets[0].ColumnCount = 6;
                    Fpfueldetails.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "S.No";
                    Fpfueldetails.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "Date";
                    Fpfueldetails.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 2].Text = "Opening Km";
                    Fpfueldetails.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 3].Text = "Closing Km";
                    Fpfueldetails.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 4].Text = "Travel Km";
                    Fpfueldetails.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 5].Text = "Added Fuel(Lt)";
                    Fpfueldetails.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 6].Text = "Item";


                    Fpfueldetails.Sheets[0].Columns[0].Width = 50;
                    Fpfueldetails.Sheets[0].Columns[1].Width = 100;
                    Fpfueldetails.Sheets[0].Columns[2].Width = 100;
                    Fpfueldetails.Sheets[0].Columns[3].Width = 100;
                    Fpfueldetails.Sheets[0].Columns[4].Width = 100;
                    Fpfueldetails.Sheets[0].Columns[5].Width = 100;
                    Fpfueldetails.Sheets[0].Columns[6].Width = 100;

                    string vechid = FpfuelReport.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Text.ToString();

                    string totfuel = "";
                    string fdate = txtfrm_date.Text.ToString();
                    string tdate = txtend_date.Text.ToString();
                    string[] spfd = fdate.Split('/');
                    string[] sptd = tdate.Split('/');
                    DateTime dtfrom = Convert.ToDateTime(spfd[1] + '/' + spfd[0] + '/' + spfd[2]);
                    DateTime dtto = Convert.ToDateTime(sptd[1] + '/' + sptd[0] + '/' + sptd[2]);

                    Double fopkm = 0, fclkm = 0, ftrkm = 0, ffuad = 0;

                    for (DateTime dt = dtfrom; dt <= dtto; dt = dt.AddDays(1))
                    {
                        Double travelkm = 0, totalfuel = 0, openkm = 0, closekm = 0, gettravel = 0;
                        string[] spdat = dt.ToString().Split(' ');
                        string[] getdate = spdat[0].ToString().Split('/');
                        string setdate = getdate[1] + '/' + getdate[0] + '/' + getdate[2];
                        string year = getdate[2].ToString();
                        string setyear = "" + year[2] + "" + year[3] + "";
                        string date = getdate[1].ToString();
                        if (date.Length == 1)
                        {
                            date = "0" + date;
                        }
                        string mon = getdate[0].ToString();
                        if (mon.Length == 1)
                        {
                            mon = "0" + mon;
                        }
                        string gprsdate = date + mon + setyear.ToString();
                        string getopeing = obj.GetFunction("select MAX(odometer) from VTSGPRSData where vehicleid='" + vechid + "' and date < '" + gprsdate + "'");
                        string getclose = obj.GetFunction("select MAX(odometer) from VTSGPRSData where vehicleid='" + vechid + "' and date = '" + gprsdate + "'");
                        if (getopeing.Trim() != "" && getopeing != null && getopeing.Trim() != "0")
                        {
                            openkm = Convert.ToDouble(getopeing);
                        }
                        else
                        {
                            getopeing = obj.GetFunction("select isnull(Intial_Km,0) as opkm from Vehicle_Master where Veh_ID='" + vechid + "'");
                            if (getopeing.Trim() != "" && getopeing != null)
                            {
                                openkm = Convert.ToDouble(getopeing);
                            }
                        }
                        if (getclose.Trim() != "" && getclose != null)
                        {
                            closekm = Convert.ToDouble(getclose);
                        }
                        if (openkm < closekm)
                        {
                            gettravel = closekm - openkm;
                        }
                        if (gettravel > 0)
                        {
                            travelkm = travelkm + gettravel;
                        }

                        if (travelkm > 0)
                        {

                            totalfuel = 0;
                            totfuel = obj.GetFunction("select SUM(fuel) from Vehicle_Usage where Vehicle_Id='" + vechid + "' and DATE='" + spdat[0].ToString() + "'");
                            if (totfuel.Trim() != "" && totfuel != null)
                            {
                                totalfuel = Convert.ToDouble(totfuel);
                            }
                            fopkm = fopkm + openkm;
                            fclkm = fclkm + closekm;
                            ftrkm = ftrkm + travelkm;
                            ffuad = ffuad + totalfuel;
                            Fpfueldetails.Sheets[0].RowCount++;
                            Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 0].Text = Fpfueldetails.Sheets[0].RowCount.ToString();
                            Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 1].Text = setdate.ToString();
                            Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 2].Text = openkm.ToString();
                            Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 3].Text = closekm.ToString();
                            Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 4].Text = travelkm.ToString();
                            Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 5].Text = travelkm.ToString();
                            // Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 6].Text = item.ToString();
                            Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    if (Fpfueldetails.Sheets[0].RowCount == 0)
                    {
                        errmsg.Text = "No Records found";
                        errmsg.Visible = true;
                        Fpfueldetails.Sheets[0].RowCount = 0;
                        errmsg.Font.Bold = true;
                        Fpfueldetails.Visible = false;
                    }
                    else
                    {
                        Fpfueldetails.Sheets[0].RowCount++;
                        Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 0].Text = "Total";
                        Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        Fpfueldetails.Sheets[0].SpanModel.Add(Fpfueldetails.Sheets[0].RowCount - 1, 0, 1, 2);
                        Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 2].Text = fopkm.ToString();
                        Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 3].Text = fclkm.ToString();
                        Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 4].Text = ftrkm.ToString();
                        Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 5].Text = ffuad.ToString();
                        Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    }


                }
                else if (actcol == "10")
                {
                    Fpfueldetails.Visible = true;
                    FpfuelReport.Visible = false;
                    btnback.Visible = true;
                    Fpfueldetails.Sheets[0].RowCount = 0;
                    Fpfueldetails.Sheets[0].ColumnCount = 0;
                    Fpfueldetails.Sheets[0].ColumnCount = 5;
                    Fpfueldetails.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "S.No";
                    Fpfueldetails.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "Vechile Id";
                    Fpfueldetails.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 2].Text = "Date";
                    Fpfueldetails.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 3].Text = "Time";
                    Fpfueldetails.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 4].Text = "Google Location";

                    Fpfueldetails.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpfueldetails.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);

                    Fpfueldetails.Sheets[0].Columns[0].Width = 40;
                    Fpfueldetails.Sheets[0].Columns[1].Width = 50;
                    Fpfueldetails.Sheets[0].Columns[2].Width = 50;
                    Fpfueldetails.Sheets[0].Columns[3].Width = 80;
                    Fpfueldetails.Sheets[0].Columns[4].Width = 500;

                    string vechid = FpfuelReport.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Text.ToString();

                    string fdate = txtfrm_date.Text.ToString();
                    string tdate = txtend_date.Text.ToString();
                    string[] spfd = fdate.Split('/');
                    string[] sptd = tdate.Split('/');
                    DateTime dtfrom = Convert.ToDateTime(spfd[1] + '/' + spfd[0] + '/' + spfd[2]);
                    DateTime dtto = Convert.ToDateTime(sptd[1] + '/' + sptd[0] + '/' + sptd[2]);

                    int colorchage = 0;
                    for (DateTime dt = dtfrom; dt <= dtto; dt = dt.AddDays(1))
                    {

                        string[] spdat = dt.ToString().Split(' ');
                        string[] getdate = spdat[0].ToString().Split('/');
                        string setdate = getdate[1] + '/' + getdate[0] + '/' + getdate[2];
                        string year = getdate[2].ToString();
                        string setyear = "" + year[2] + "" + year[3] + "";
                        string date = getdate[1].ToString();
                        if (date.Length == 1)
                        {
                            date = "0" + date;
                        }
                        string mon = getdate[0].ToString();
                        if (mon.Length == 1)
                        {
                            mon = "0" + mon;
                        }
                        string gprsdate = date + mon + setyear.ToString();
                        string stevdate = date + '/' + mon + "/20" + setyear;
                        string strvisiplacequery = "select distinct v.Veh_ID, vg.googleLocation,vg.Time from VTSGPRSData vg,vehicle_master v,RouteMaster r,Stage_Master s where v.Veh_ID=vg.VehicleID and r.Veh_ID=v.Veh_ID and vg.googleLocation=s.Address and cast(s.Stage_id as nvarchar(100))=cast(r.Stage_Name as nvarchar(100)) and v.Veh_ID='" + vechid + "' and DATE='" + gprsdate + "'  order by vg.Time";
                        DataSet dsvisit = obj.select_method_wo_parameter(strvisiplacequery, "Text");
                        if (dsvisit.Tables[0].Rows.Count > 0)
                        {
                            colorchage++;
                        }
                        for (int i = 0; i < dsvisit.Tables[0].Rows.Count; i++)
                        {
                            string final_time = "";
                            string gettime = dsvisit.Tables[0].Rows[i]["Time"].ToString();
                            if (gettime.Trim() != null && gettime.Trim() != "")
                            {
                                string Sp_hour = "", Sp_Min = "", Sp_Sec = "";
                                string[] sub_string = Regex.Split(gettime, "");
                                for (int ctr = 0; ctr < sub_string.Length; ctr++)
                                {
                                    if (ctr < 3 && sub_string[ctr] != "")
                                    {
                                        Sp_hour = Sp_hour + sub_string[ctr];
                                    }
                                    else if (ctr < 5 && sub_string[ctr] != "")
                                    {
                                        Sp_Min = Sp_Min + sub_string[ctr];
                                    }
                                    else if (sub_string[ctr] != "")
                                    {
                                        Sp_Sec = Sp_Sec + sub_string[ctr];
                                    }
                                }

                                string time_sess = " AM";
                                if (Convert.ToInt32(Sp_hour) > 12)
                                {
                                    Sp_hour = Convert.ToString(Convert.ToInt32(Sp_hour) - 12);
                                    time_sess = " PM";
                                }
                                final_time = Sp_hour + ":" + Sp_Min + ":" + Sp_Sec + time_sess;
                            }
                            Fpfueldetails.Sheets[0].RowCount++;
                            if (colorchage % 2 == 0)
                            {
                                Fpfueldetails.Sheets[0].Cells[(Fpfueldetails.Sheets[0].RowCount - 1), 2].BackColor = Color.White;
                                Fpfueldetails.Sheets[0].Cells[(Fpfueldetails.Sheets[0].RowCount - 1), 3].BackColor = Color.White;
                                Fpfueldetails.Sheets[0].Cells[(Fpfueldetails.Sheets[0].RowCount - 1), 4].BackColor = Color.White;
                            }
                            else
                            {
                                Fpfueldetails.Sheets[0].Cells[(Fpfueldetails.Sheets[0].RowCount - 1), 2].BackColor = Color.AliceBlue;
                                Fpfueldetails.Sheets[0].Cells[(Fpfueldetails.Sheets[0].RowCount - 1), 3].BackColor = Color.AliceBlue;
                                Fpfueldetails.Sheets[0].Cells[(Fpfueldetails.Sheets[0].RowCount - 1), 4].BackColor = Color.AliceBlue;
                            }
                            Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 0].Text = Fpfueldetails.Sheets[0].RowCount.ToString();
                            Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 1].Text = dsvisit.Tables[0].Rows[i]["Veh_ID"].ToString();
                            Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 2].Text = stevdate;
                            Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 3].Text = final_time.ToString();
                            Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 4].Text = dsvisit.Tables[0].Rows[i]["googleLocation"].ToString();
                            Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    if (Fpfueldetails.Sheets[0].RowCount == 0)
                    {
                        errmsg.Text = "No Records found";
                        errmsg.Visible = true;
                        Fpfueldetails.Sheets[0].RowCount = 0;
                        errmsg.Font.Bold = true;
                        Fpfueldetails.Visible = false;
                    }
                }
                else if (actcol == "11")
                {
                    Fpfueldetails.Visible = true;
                    FpfuelReport.Visible = false;
                    btnback.Visible = true;
                    Fpfueldetails.Sheets[0].RowCount = 0;
                    Fpfueldetails.Sheets[0].ColumnCount = 0;
                    Fpfueldetails.Sheets[0].ColumnCount = 6;
                    Fpfueldetails.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "S.No";
                    Fpfueldetails.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "Date";
                    Fpfueldetails.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 2].Text = "Stage";
                    Fpfueldetails.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 3].Text = "Start Time";
                    Fpfueldetails.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 4].Text = "End Time";
                    Fpfueldetails.Sheets[0].ColumnHeader.Cells[Fp_Fuel.Sheets[0].ColumnHeader.RowCount - 1, 5].Text = "Delay";

                    Fpfueldetails.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

                    Fpfueldetails.Sheets[0].Columns[0].Width = 80;
                    Fpfueldetails.Sheets[0].Columns[1].Width = 150;
                    Fpfueldetails.Sheets[0].Columns[2].Width = 300;
                    Fpfueldetails.Sheets[0].Columns[3].Width = 150;
                    Fpfueldetails.Sheets[0].Columns[4].Width = 150;
                    Fpfueldetails.Sheets[0].Columns[5].Width = 150;

                    string vechid = FpfuelReport.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Text.ToString();

                    string fdate = txtfrm_date.Text.ToString();
                    string tdate = txtend_date.Text.ToString();
                    string[] spfd = fdate.Split('/');
                    string[] sptd = tdate.Split('/');
                    DateTime dtfrom = Convert.ToDateTime(spfd[1] + '/' + spfd[0] + '/' + spfd[2]);
                    DateTime dtto = Convert.ToDateTime(sptd[1] + '/' + sptd[0] + '/' + sptd[2]);

                    int colorchage = 0;
                    for (DateTime dt = dtfrom; dt <= dtto; dt = dt.AddDays(1))
                    {

                        string[] spdat = dt.ToString().Split(' ');
                        string[] getdate = spdat[0].ToString().Split('/');
                        string setdate = getdate[1] + '/' + getdate[0] + '/' + getdate[2];
                        string year = getdate[2].ToString();
                        string setyear = "" + year[2] + "" + year[3] + "";
                        string date = getdate[1].ToString();
                        if (date.Length == 1)
                        {
                            date = "0" + date;
                        }
                        string mon = getdate[0].ToString();
                        if (mon.Length == 1)
                        {
                            mon = "0" + mon;
                        }
                        string gprsdate = date + mon + setyear.ToString();
                        string stevdate = date + '/' + mon + "/20" + setyear;

                        DateTime dtfinalstaget = Convert.ToDateTime("01/01/1900");
                        string runningtime = "";
                        string curstage = "";
                        string getquerfi = "select top 1(vg.Time) as timer,s.Stage_Name from VTSGPRSData vg,vehicle_master v,RouteMaster r,Stage_Master s where v.Veh_ID=vg.VehicleID and r.Veh_ID=v.Veh_ID and vg.googleLocation=s.Address and cast(s.Stage_id as nvarchar(100))=cast(r.Stage_Name as nvarchar(100)) and v.Veh_ID='" + vechid + "'  and DATE='" + gprsdate + "'  order by vg.Time desc";
                        DataSet dsfinadeta = obj.select_method_wo_parameter(getquerfi, "Text");

                        if (dsfinadeta.Tables[0].Rows.Count > 0)
                        {
                            runningtime = dsfinadeta.Tables[0].Rows[0]["timer"].ToString();
                            curstage = dsfinadeta.Tables[0].Rows[0]["Stage_Name"].ToString();
                        }

                        if (runningtime.Trim() != null && runningtime.Trim() != "" && runningtime.Trim() != "0")
                        {
                            string Sp_hour = "", Sp_Min = "", Sp_Sec = "";
                            string[] sub_string = Regex.Split(runningtime, "");
                            for (int ctr = 0; ctr < sub_string.Length; ctr++)
                            {
                                if (ctr < 3 && sub_string[ctr] != "")
                                {
                                    Sp_hour = Sp_hour + sub_string[ctr];
                                }
                                else if (ctr < 5 && sub_string[ctr] != "")
                                {
                                    Sp_Min = Sp_Min + sub_string[ctr];
                                }
                                else if (sub_string[ctr] != "")
                                {
                                    Sp_Sec = Sp_Sec + sub_string[ctr];
                                }
                            }
                            string time_sess = " AM";
                            if (Convert.ToInt32(Sp_hour) > 12)
                            {
                                Sp_hour = Convert.ToString(Convert.ToInt32(Sp_hour) - 12);
                                time_sess = " PM";
                            }
                            string final_stage_time = Sp_hour + ":" + Sp_Min + ":" + Sp_Sec + time_sess;
                            dtfinalstaget = Convert.ToDateTime(final_stage_time);
                        }
                        for (int se = 0; se < 2; se++)
                        {
                            string query = "";
                            if (se == 0)
                            {
                                //query = "select Arr_Time,Dep_Time,s.Stage_id,s.Stage_Name,s.Address from RouteMaster r,Stage_Master s where str(r.Stage_Name)=str(s.Stage_id) and sess='m' and Veh_ID='" + vechid + "' and (Arr_Time='halt' or Dep_Time='halt' or Arr_Time='-' or Dep_Time='-'  ) order by Dep_Time";
                                query = "select Arr_Time,Dep_Time,s.Stage_id,s.Stage_Name,s.Address from RouteMaster r,Stage_Master s where str(r.Stage_Name)=str(s.Stage_id) and sess='m' and Veh_ID='" + vechid + "'  order by Dep_Time";
                            }
                            else
                            {
                                //query = "select Arr_Time,Dep_Time,s.Stage_id,s.Stage_Name,s.Address from RouteMaster r,Stage_Master s where str(r.Stage_Name)=str(s.Stage_id) and sess='a' and Veh_ID='" + vechid + "' and (Arr_Time='halt' or Dep_Time='halt' or Arr_Time='-' or Dep_Time='-'  ) order by Dep_Time";
                                query = "select Arr_Time,Dep_Time,s.Stage_id,s.Stage_Name,s.Address from RouteMaster r,Stage_Master s where str(r.Stage_Name)=str(s.Stage_id) and sess='a' and Veh_ID='" + vechid + "' order by Dep_Time";
                            }

                            //  string query = "select s.Stage_Name,Arr_Time,Dep_Time,s.Address,sess from RouteMaster r,Stage_Master s where str(r.Stage_Name)=str(s.Stage_id)  and veh_id='" + vechid + "' Order by sess desc";
                            DataSet dsstatus = obj.select_method_wo_parameter(query, "Text");
                            if (dsstatus.Tables[0].Rows.Count > 0)
                            {
                                colorchage++;
                                for (int i = 0; i < dsstatus.Tables[0].Rows.Count; i++)
                                {
                                    string arrtime = dsstatus.Tables[0].Rows[i]["Arr_Time"].ToString();
                                    string depttime = dsstatus.Tables[0].Rows[i]["Dep_Time"].ToString();
                                    string delay = "";
                                    string address = dsstatus.Tables[0].Rows[i]["Address"].ToString();
                                    string final_time = "";
                                    string gettime = "";
                                    if (se == 0)
                                    {
                                        //if (i == 0)
                                        //{
                                        gettime = obj.GetFunction("select isnull(min(time),0) as time from VTSGPRSData  where Date='" + gprsdate + "' and GoogleLocation='" + address + "' and VehicleID='" + vechid + "'");
                                        //}
                                        //else
                                        //{
                                        //    gettime = obj.GetFunction("select isnull(min(time),0) as time from VTSGPRSData  where Date='" + gprsdate + "' and GoogleLocation='" + address + "' and VehicleID='" + vechid + "'");
                                        //}
                                    }
                                    else
                                    {
                                        //if (i == 0)
                                        //{
                                        gettime = obj.GetFunction("select isnull(max(time),0) as time from VTSGPRSData  where Date='" + gprsdate + "' and GoogleLocation='" + address + "' and VehicleID='" + vechid + "'");
                                        //}
                                        //else
                                        //{
                                        //    gettime = obj.GetFunction("select isnull(max(time),0) as time from VTSGPRSData  where Date='" + gprsdate + "' and GoogleLocation='" + address + "' and VehicleID='" + vechid + "'");
                                        //}
                                    }

                                    //  string gettime = obj.GetFunction("select min(time) as time from VTSGPRSData  where Date='" + gprsdate + "' and GoogleLocation='" + address + "'");
                                    string currdate = "";
                                    if (gettime.Trim() != null && gettime.Trim() != "" && gettime.Trim() != "0")
                                    {
                                        string Sp_hour = "", Sp_Min = "", Sp_Sec = "";
                                        string[] sub_string = Regex.Split(gettime, "");
                                        for (int ctr = 0; ctr < sub_string.Length; ctr++)
                                        {
                                            if (ctr < 3 && sub_string[ctr] != "")
                                            {
                                                Sp_hour = Sp_hour + sub_string[ctr];
                                            }
                                            else if (ctr < 5 && sub_string[ctr] != "")
                                            {
                                                Sp_Min = Sp_Min + sub_string[ctr];
                                            }
                                            else if (sub_string[ctr] != "")
                                            {
                                                Sp_Sec = Sp_Sec + sub_string[ctr];
                                            }
                                        }
                                        string time_sess = " AM";
                                        if (Convert.ToInt32(Sp_hour) > 12)
                                        {
                                            Sp_hour = Convert.ToString(Convert.ToInt32(Sp_hour) - 12);
                                            time_sess = " PM";
                                        }
                                        final_time = Sp_hour + ":" + Sp_Min + ":" + Sp_Sec + time_sess;
                                        DateTime dttime = Convert.ToDateTime(final_time);
                                        string checktime = "";
                                        if (se == 0)
                                        {
                                            if (i == 0)
                                            {
                                                checktime = depttime;
                                            }
                                            else
                                            {
                                                checktime = arrtime;
                                            }

                                        }
                                        else
                                        {
                                            if (i == 0)
                                            {
                                                checktime = depttime;
                                            }
                                            else
                                            {
                                                checktime = arrtime;
                                            }

                                        }
                                        if (checktime.Trim().ToLower() != "halt" && checktime.Trim() != null && checktime.Trim() != "")
                                        {
                                            string[] spactt = checktime.Split('.');
                                            string settime = "";
                                            for (int spt = 0; spt <= spactt.GetUpperBound(0); spt++)
                                            {
                                                if (settime == "")
                                                {
                                                    settime = spactt[spt].ToString();
                                                }
                                                else
                                                {
                                                    settime = settime + ":" + spactt[spt].ToString();
                                                }
                                            }

                                            string acttime = settime;
                                            if (se == 0)
                                            {
                                                acttime = acttime + ":00 AM";
                                            }
                                            else
                                            {
                                                acttime = acttime + ":00 PM";
                                            }
                                            DateTime dtact = Convert.ToDateTime(acttime);
                                            currdate = acttime;
                                            delay = Convert.ToString(dttime - dtact);
                                        }

                                    }



                                    Fpfueldetails.Sheets[0].RowCount++;
                                    if (colorchage % 2 == 0)
                                    {
                                        Fpfueldetails.Sheets[0].Rows[Fpfueldetails.Sheets[0].RowCount - 1].BackColor = Color.White;
                                    }
                                    else
                                    {
                                        Fpfueldetails.Sheets[0].Rows[Fpfueldetails.Sheets[0].RowCount - 1].BackColor = Color.White;
                                    }
                                    if (curstage == dsstatus.Tables[0].Rows[i]["Stage_Name"].ToString())
                                    {
                                        DateTime curste = Convert.ToDateTime(currdate);
                                        if (dtfinalstaget.ToString("dd/MM/yyyy") != "01/01/1900")
                                        {
                                            if (dtfinalstaget <= Convert.ToDateTime(currdate))
                                            {
                                                Fpfueldetails.Sheets[0].Rows[Fpfueldetails.Sheets[0].RowCount - 1].BackColor = Color.Gold;
                                            }
                                        }
                                    }
                                    Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 0].Text = Fpfueldetails.Sheets[0].RowCount.ToString();
                                    Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 1].Text = stevdate;
                                    Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 2].Text = dsstatus.Tables[0].Rows[i]["Stage_Name"].ToString();
                                    Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 3].Text = arrtime;
                                    Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 4].Text = depttime;
                                    Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 5].Text = delay;
                                    Fpfueldetails.Sheets[0].Cells[Fpfueldetails.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                        }
                    }
                    if (Fpfueldetails.Sheets[0].RowCount == 0)
                    {
                        errmsg.Text = "No Records found";
                        errmsg.Visible = true;
                        Fpfueldetails.Sheets[0].RowCount = 0;
                        errmsg.Font.Bold = true;
                        Fpfueldetails.Visible = false;
                    }
                }
                Fpfueldetails.Sheets[0].PageSize = Fpfueldetails.Sheets[0].RowCount;
            }
        }
        catch
        {
        }
    }

    protected void btnback_Click(object sender, EventArgs e)
    {
        try
        {
            //btnMainGo_Click(sender, e);
            Fpfueldetails.Visible = false;
            FpfuelReport.Visible = true;
            btnback.Visible = false;
        }
        catch
        {
        }
    }

    //protected void txt_fuel_TextChanged(object sender, EventArgs e)
    //{

    //}

    protected void txtfuelltrs_TextChanged(object sender, EventArgs e)
    {
        try
        {

            if (txtfuelltrs.Text.Trim() != "")
            {
                //Lblrepairexpenses.Visible = false;
                //TxtRepairExpenses.Visible = false;
                //Lblfuelexpanse.Visible = true;
                //txtFuelDieselExpenses.Visible = true;
                double total = Convert.ToDouble((Convert.ToDouble(txt_fuel.Text.Trim())) * (Convert.ToDouble(txtfuelltrs.Text.Trim())));
                Txtfuelamount.Text = total.ToString();
            }
            else
            {
                //Lblrepairexpenses.Visible = true;
                //TxtRepairExpenses.Visible = true;
                //Lblfuelexpanse.Visible = false;
                //txtFuelDieselExpenses.Visible = false;
                //txtFuelDieselExpenses.Text = "";
                Txtfuelamount.Text = "";
            }
        }
        catch
        {
        }

    }

    protected void txt_fuel_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_fuel.Text.Trim() != "" && txtfuelltrs.Text.Trim() != "")
            {
                Txtfuelamount.Text = Convert.ToString((Convert.ToDouble(txt_fuel.Text.Trim())) * (Convert.ToDouble(txtfuelltrs.Text.Trim())));
            }
        }
        catch
        {
        }
    }

    protected void rdbfuel_CheckedChanged(object sender, EventArgs e)
    {
        //Btnupdate.Visible = false;
        bindMethod();
        fuelpanel.Visible = true;
        maintainpanel.Visible = false;
        btnsub.Visible = false;
    }

    protected void Rdbmailtaince_CheckedChanged(object sender, EventArgs e)
    {
        bindMethod();
        fuelpanel.Visible = false;
        maintainpanel.Visible = true;
        btnsub.Visible = true;
        Rdbservice.Checked = true;
        lblamount.Visible = true;
        txtamount.Visible = true;

        lblvat.Visible = true;
        Txtvat.Visible = true;
        Lblfinalvat.Visible = true;
        Txtfinalvat.Visible = true;
        Fpvehicle.Visible = false;
        lbltotalamount.Visible = false;
        Txttotalamount.Visible = false;
        lbltax.Visible = false;
        Txttax.Visible = false;
        lblfinalcost.Visible = false;
        Txtfinalcost.Visible = false;

    }

    protected void Fp_Intimation_Driver_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

    }

    protected void Txtdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime dtnow = DateTime.Now;
            lblerrmsg.Visible = false;
            string datefad, dtfromad;
            string datefromad;
            string yr4, m4, d4;
            datefad = Txtdate.Text.ToString();
            string[] split4 = datefad.Split(new Char[] { '/' });
            if (split4.Length == 3)
            {
                datefromad = split4[0].ToString() + "/" + split4[1].ToString() + "/" + split4[2].ToString();
                yr4 = split4[2].ToString();
                m4 = split4[1].ToString();
                d4 = split4[0].ToString();
                dtfromad = m4 + "/" + d4 + "/" + yr4;
                DateTime dt1 = Convert.ToDateTime(dtfromad);
                if (dt1 > dtnow)
                {

                    lblerrmsg.Text = "Please Enter Valid Date";
                    lblerrmsg.Visible = true;
                    Txtdate.Text = DateTime.Now.ToString("dd/MM/yyy");

                }
                else
                {
                    lblerrmsg.Visible = false;

                }
            }

        }
        catch (Exception ex)
        {
        }
    }

    protected void Txtbilldate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime dtnow = DateTime.Now;
            lblerrmsg.Visible = false;
            string datefad, dtfromad;
            string datefromad;
            string yr4, m4, d4;
            datefad = Txtbilldate.Text.ToString();
            string[] split4 = datefad.Split(new Char[] { '/' });
            if (split4.Length == 3)
            {
                datefromad = split4[0].ToString() + "/" + split4[1].ToString() + "/" + split4[2].ToString();
                yr4 = split4[2].ToString();
                m4 = split4[1].ToString();
                d4 = split4[0].ToString();
                dtfromad = m4 + "/" + d4 + "/" + yr4;
                DateTime dt1 = Convert.ToDateTime(dtfromad);
                if (dt1 > dtnow)
                {

                    lblerrmsg.Text = "Please Enter Valid Date";
                    lblerrmsg.Visible = true;
                    Txtdate.Text = DateTime.Now.ToString("dd/MM/yyy");

                }
                else
                {
                    lblerrmsg.Visible = false;

                }
            }

        }
        catch (Exception ex)
        {
        }
    }

    protected void txt_date_TextChanged(object sender, EventArgs e)
    {
        try
        {

            DateTime dtnow = DateTime.Now;
            lblmessagefule.Visible = false;
            string datefad, dtfromad;
            string datefromad;
            string yr4, m4, d4;
            datefad = txt_date.Text.ToString();
            string[] split4 = datefad.Split(new Char[] { '/' });
            if (split4.Length == 3)
            {
                datefromad = split4[0].ToString() + "/" + split4[1].ToString() + "/" + split4[2].ToString();
                yr4 = split4[2].ToString();
                m4 = split4[1].ToString();
                d4 = split4[0].ToString();
                dtfromad = m4 + "/" + d4 + "/" + yr4;
                DateTime dt1 = Convert.ToDateTime(dtfromad);
                if (dt1 > dtnow)
                {

                    lblmessagefule.Text = "Please Enter Valid Date";
                    lblmessagefule.Visible = true;
                    txt_date.Text = DateTime.Now.ToString("dd/MM/yyy");

                }
                else
                {
                    lblmessagefule.Visible = false;

                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnaddpurpose_Click(object sender, EventArgs e)
    {
        caption = newcaption.InnerHtml;
        paneladdremove.Visible = false;

        if (Rdbmailtaince.Checked == true)
        {
            if (txtpurpose.Text != "")
            {
                paneladdremove.Visible = false;
                hastab.Clear();

                hastab.Add("tcrit", "etype");
                hastab.Add("tval", txtpurpose.Text.Trim());

                ds_expense = obj.select_method("enquiry_add_textcodenew", hastab, "sp");

                if (ds_expense.Tables.Count > 0)
                {
                    bindtypeofexpense1();
                    lblerr.Visible = false;

                }
                else
                {
                    lblerr.Text = "Already Exists";
                    lblerr.Visible = true;
                }


                ddlpurpose.SelectedIndex = ddlpurpose.Items.IndexOf(ddlpurpose.Items.FindByText(txtpurpose.Text.Trim()));
                txtpurpose.Text = "";
            }
        }
        ddlpurpose.Attributes.Add("onfocus", "subu()");
    }

    protected void btnminuspurpose_Click(object sender, EventArgs e)
    {
        paneladdremove.Visible = false;
        ddlpurpose.Attributes.Add("onfocus", "subu()");
    }

    protected void Btnadd_Click(object sender, EventArgs e)
    {
        paneladdremove.Visible = true;
        ddlpurpose.Attributes.Add("onfocus", "subu()");
    }

    protected void btnsub_Click(object sender, EventArgs e)
    {

        string cmd_delete = "delete from textvaltable_new where Textval='" + ddlpurpose.SelectedItem.ToString() + "'";
        int a = obj.update_method_wo_parameter(cmd_delete, "Text");

        bindtypeofexpense1();
        ddlpurpose.Attributes.Add("onfocus", "subu()");
    }

    public void company()
    {
        array();
        Fpvehicle.Visible = true;
        Fpvehicle.Sheets[0].RowCount = 0;
        Fpvehicle.Sheets[0].RowHeader.Visible = false;
        Fpvehicle.Sheets[0].AutoPostBack = false;
        Fpvehicle.Height = 150;
        Fpvehicle.Width = 730;
        Fpvehicle.Sheets[0].Columns[1].Width = 150;
        Fpvehicle.Sheets[0].ColumnCount = 5;
        Fpvehicle.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
        Fpvehicle.Sheets[0].ColumnHeader.Columns[0].Font.Size = FontUnit.Medium;
        Fpvehicle.Sheets[0].ColumnHeader.Columns[0].Font.Bold = true;
        Fpvehicle.Sheets[0].ColumnHeader.Columns[0].Font.Name = "Book Antiqua";
        Fpvehicle.Sheets[0].ColumnHeader.Columns[1].Label = "Item name";
        Fpvehicle.Sheets[0].ColumnHeader.Columns[1].Width = 350;
        Fpvehicle.Sheets[0].ColumnHeader.Columns[1].Font.Size = FontUnit.Medium;
        Fpvehicle.Sheets[0].ColumnHeader.Columns[1].Font.Bold = true;
        Fpvehicle.Sheets[0].ColumnHeader.Columns[1].Font.Name = "Book Antiqua";
        Fpvehicle.Sheets[0].ColumnHeader.Columns[2].Label = "Quantity";
        Fpvehicle.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
        Fpvehicle.Sheets[0].ColumnHeader.Columns[2].Font.Size = FontUnit.Medium;
        Fpvehicle.Sheets[0].ColumnHeader.Columns[2].Font.Bold = true;
        Fpvehicle.Sheets[0].ColumnHeader.Columns[2].Font.Name = "Book Antiqua";
        Fpvehicle.Sheets[0].ColumnHeader.Columns[3].Label = "Qty/Cost";
        Fpvehicle.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Right;
        Fpvehicle.Sheets[0].ColumnHeader.Columns[3].Font.Size = FontUnit.Medium;
        Fpvehicle.Sheets[0].ColumnHeader.Columns[3].Font.Bold = true;
        Fpvehicle.Sheets[0].ColumnHeader.Columns[3].Font.Name = "Book Antiqua";
        Fpvehicle.Sheets[0].ColumnHeader.Columns[4].Label = "Total Amount";
        Fpvehicle.Sheets[0].ColumnHeader.Columns[4].Font.Size = FontUnit.Medium;
        Fpvehicle.Sheets[0].ColumnHeader.Columns[4].Font.Bold = true;
        Fpvehicle.Sheets[0].ColumnHeader.Columns[4].Font.Name = "Book Antiqua";
        Fpvehicle.Columns[4].Locked = true;
        FarPoint.Web.Spread.DoubleCellType intgrcell = new FarPoint.Web.Spread.DoubleCellType();
        intgrcell.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
        // intgrcell.MaximumValue = Convert.ToInt32(100);
        intgrcell.MinimumValue = 0;
        intgrcell.ErrorMessage = "Enter valid Number";
        //Fpvehicle.Sheets[0].ColumnHeader.Columns[5].Label = "Totakjkk";
        //Fpvehicle.Sheets[0].ColumnHeader.Columns[5].Font.Size = FontUnit.Medium;
        //Fpvehicle.Sheets[0].ColumnHeader.Columns[5].Font.Bold = true;
        //Fpvehicle.Sheets[0].ColumnHeader.Columns[5].Font.Name = "Book Antiqua"; 
        FarPoint.Web.Spread.ComboBoxCellType textcel_type = new FarPoint.Web.Spread.ComboBoxCellType(itemarray);
        Fpvehicle.Sheets[0].Columns[1].CellType = textcel_type;
        //Fpvehicle.Sheets[0].Columns[0].Label = "1";
        FarPoint.Web.Spread.IntegerCellType currtype = new FarPoint.Web.Spread.IntegerCellType();
        Fpvehicle.Sheets[0].Columns[3].CellType = currtype;
        Fpvehicle.Sheets[0].Columns[2].CellType = currtype;
        //FarPoint.Web.Spread.DoubleCellType integer = new FarPoint.Web.Spread.DoubleCellType();
        //integer.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
        //integer.MinimumValue = 0;
        //integer.ErrorMessage = "Enter Valid Number";
    }

    public void array()
    {

        //string strquery = "select distinct itemcode,convert(varchar(max),itemname +'-'+(select mastervalue from co_mastervalues where convert(nvarchar(100),mastercode)=subheader_code))as item from im_itemmaster"; //rajasekar 22march2018
        string strquery = "select distinct itemcode,convert(varchar(max),itemname +'-'+(select mastervalue from co_mastervalues where convert(nvarchar(100),mastercode)=subheader_code))as item from im_itemmaster where isnull(ForHostelItem,0)!=0";//rajasekar 22march2018
        ds = obj.select_method_wo_parameter(strquery, "Text");
        int c = ds.Tables[0].Rows.Count;
        itemarray = new string[c];
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                itemarray[i] = ds.Tables[0].Rows[i]["item"].ToString();
            }

        }
        //itemarray[0] = "Others";
    }

    protected void Fpvehiclecmd(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string actrow = e.SheetView.ActiveRow.ToString();

        string actcol = e.SheetView.ActiveColumn.ToString();
        seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
        //if (seltext == "Others")
        //{
        //    Fpvehicle.Sheets[0].Columns[2].Visible = false;
        //}


        int a = (Fpvehicle.Sheets[0].RowCount - 1);
        string activerow = "";
        string activecol = "";



        if (Fpvehicle.Sheets[0].RowCount != 0)
        {
            activerow = Fpvehicle.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpvehicle.ActiveSheetView.ActiveColumn.ToString();
            int rowcount_1 = Fpvehicle.Sheets[0].RowCount;
            rowcount_1--;
            int row = Convert.ToInt32(activerow);
            row++;

            // e.Handled = true;

            Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 4].Formula = "SUM(C" + row + "*D" + row + ")";
            Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 4].Font.Bold = true;
            Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 4].HorizontalAlign = HorizontalAlign.Right;
            int a1 = Fpvehicle.Sheets[0].RowCount - 1;
            Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 4].Formula = "SUM(E1:E" + a1 + ")";
            Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 4].Font.Bold = true;
            Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;

            // Txttotalamount.Text = Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 4].Text.ToString();
            val = true;
        }






    }

    protected void fpcell(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        Fpvehicle.SaveChanges();

        if (Fpvehicle.Sheets[0].RowCount > 0)
        {
            if (Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 4].Text.Trim() != "")
            {
                string strva = Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 4].Text.ToString();
                Txttotalamount.Text = strva;
            }
        }
    }

    protected void fpvehirender(object sender, EventArgs e)
    {
        Fpvehicle.SaveChanges();
        if (val == true)
        {

            if (Fpvehicle.Sheets[0].RowCount > 0)
            {
                if (Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 4].Text.Trim() != "")
                {
                    Fpvehicle.SaveChanges();
                    string strva = Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 4].Text.ToString();
                    Session["textval"] = strva;
                }
            }
        }
    }

    //public void total()
    //{
    //    string test = Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 4].Text;
    //    Txttotalamount.Text = Convert.ToString(test);

    //}
    protected void Rdbservice_CheckedChanged(object sender, EventArgs e)
    {
        btnaddrow.Visible = false;
        btnremoverow.Visible = false;
        lblamount.Visible = true;
        txtamount.Visible = true;
        lblvat.Visible = true;
        Txtvat.Visible = true;
        Lblfinalvat.Visible = true;
        Txtfinalvat.Visible = true;
        Fpvehicle.Visible = false;
        lbltotalamount.Visible = false;
        Txttotalamount.Visible = false;
        lbltax.Visible = false;
        Txttax.Visible = false;
        lblfinalcost.Visible = false;
        Txtfinalcost.Visible = false;
        lblamountvalid.Visible = true;
        ddlpurpose.Attributes.Add("onfocus", "subu()");
    }

    protected void Rdbitem_CheckedChanged(object sender, EventArgs e)
    {
        btnaddrow.Visible = true;
        btnremoverow.Visible = true;
        lblamount.Visible = false;
        txtamount.Visible = false;
        lblvat.Visible = false;
        Txtvat.Visible = false;
        Lblfinalvat.Visible = false;
        Txtfinalvat.Visible = false;
        Fpvehicle.Visible = true;
        lbltotalamount.Visible = true;
        Txttotalamount.Visible = true;
        lbltax.Visible = true;
        Txttax.Visible = true;
        lblfinalcost.Visible = true;
        Txtfinalcost.Visible = true;
        lblamountvalid.Visible = false;
        ddlpurpose.Attributes.Add("onfocus", "subu()");
    }

    protected void btncmpy_Click(object sender, EventArgs e)
    {
        panel6.Visible = true;
        fsitem.Visible = true;
        fsitem.Sheets[0].Columns[2].Locked = true;
        fsitem.Sheets[0].Rows.Count = 0;
        fsitem.Sheets[0].SheetCorner.ColumnCount = 0;
        fsitem.CommandBar.Visible = false;
        fsitem.Sheets[0].Columns.Count = 3;
        fsitem.Height = 300;
        fsitem.Width = 500;
        fsitem.Columns[0].Width = 30;
        fsitem.Columns[1].Width = 100;
        fsitem.Columns[2].Width = 350;


        fsitem.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        fsitem.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Vendor Code";
        fsitem.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Vendor Name";
        string myquere = "select vendor_code,vendor_name from vendor_details";
        DataSet dnew = new DataSet();
        dnew = obj.select_method_wo_parameter(myquere, "Text");
        if (dnew.Tables[0].Rows.Count > 0)
        {

            for (int i = 0; i < dnew.Tables[0].Rows.Count; i++)
            {
                fsitem.Sheets[0].Rows.Count++;
                fsitem.Sheets[0].Cells[fsitem.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(fsitem.Sheets[0].Rows.Count);
                fsitem.Sheets[0].Cells[fsitem.Sheets[0].Rows.Count - 1, 1].Text = dnew.Tables[0].Rows[i]["vendor_code"].ToString();
                fsitem.Sheets[0].Cells[fsitem.Sheets[0].Rows.Count - 1, 2].Text = dnew.Tables[0].Rows[i]["vendor_name"].ToString();
                fsitem.Sheets[0].Cells[fsitem.Sheets[0].Rows.Count - 1, 1].Font.Bold = true;
                fsitem.Sheets[0].Cells[fsitem.Sheets[0].Rows.Count - 1, 2].Font.Bold = true;
                fsitem.Sheets[0].Cells[fsitem.Sheets[0].Rows.Count - 1, 1].Font.Bold = true;
                fsitem.Sheets[0].Cells[fsitem.Sheets[0].Rows.Count - 1, 2].Font.Name = "Book Antiqua";
                fsitem.Sheets[0].Cells[fsitem.Sheets[0].Rows.Count - 1, 1].Font.Name = "Book Antiqua";
                fsitem.Sheets[0].Cells[fsitem.Sheets[0].Rows.Count - 1, 2].Font.Size = FontUnit.Medium;
                fsitem.Sheets[0].Cells[fsitem.Sheets[0].Rows.Count - 1, 1].Font.Name = "Book Antiqua";
                fsitem.Sheets[0].PageSize = dnew.Tables[0].Rows.Count;

                mmmg.Visible = false;

            }

            msg2.Text = "No of items :" + dnew.Tables[0].Rows.Count;
            msg2.Visible = true;

        }
        else
        {
            mmmg.Text = "No Records Found";
            mmmg.Visible = true;
            msg2.Visible = false;
        }






    }

    protected void btnstaffadd_Click(object sender, EventArgs e)
    {
        try
        {
            string activerow = "";
            string activecol = "";
            if (fsitem.Sheets[0].RowCount != 0)
            {
                activerow = fsitem.ActiveSheetView.ActiveRow.ToString();
                activecol = fsitem.ActiveSheetView.ActiveColumn.ToString();
                if (activerow != Convert.ToString(-1))
                {

                    string name = fsitem.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                    txtcompanyname.Text = name;
                }
                panel6.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void btnexitpop_Click(object sender, EventArgs e)
    {
        panel6.Visible = false;
    }

    protected void btnaddrow_Click(object sender, EventArgs e)
    {
        try
        {
            int tc = 0;
            if (Fpvehicle.Sheets[0].RowCount == 0)
            {
                Fpvehicle.Sheets[0].RowCount++;
                Fpvehicle.Sheets[0].RowCount++;
                tc = Fpvehicle.Sheets[0].RowCount - 1;


                Fpvehicle.Sheets[0].SpanModel.Add(Fpvehicle.Sheets[0].RowCount - 1, 0, 1, 4);

                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 0].Text = "Total";
                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;

                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 0].Text = Convert.ToString(tc);
                //Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 0].Font.Bold = true;
                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 0].Font.Size = FontUnit.Medium;
                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;




            }




            else
            {
                Fpvehicle.Sheets[0].RowCount--;

                Fpvehicle.Sheets[0].RowCount++;
                Fpvehicle.Sheets[0].RowCount++;
                tc = Fpvehicle.Sheets[0].RowCount - 1;
                Fpvehicle.Sheets[0].SpanModel.Add(Fpvehicle.Sheets[0].RowCount - 1, 0, 1, 4);
                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 0].Text = "Total";
                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 0].Text = Convert.ToString(tc);
                // Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 0].Font.Bold = true;
                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 0].Font.Size = FontUnit.Medium;
                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;

            }
        }
        catch
        {
        }
    }

    protected void btnremoverow_Click(object sender, EventArgs e)
    {
        Fpvehicle.Sheets[0].RowCount--;
    }

    protected void txtamount_TextChanged(object sender, EventArgs e)
    {
        double total = Convert.ToDouble(txtamount.Text);
        // double tax = Convert.ToDouble(Txtvat.Text);
        if (Txtvat.Text == "")
        {
            double sums = (total * 1);
            Txtfinalvat.Text = sums.ToString();
        }
    }

    protected void Txtvat_TextChanged(object sender, EventArgs e)
    {
        try
        {
            double total = Convert.ToDouble(txtamount.Text);
            double tax = Convert.ToDouble(Txtvat.Text);

            if (Txtvat.Text == "0")
            {
                double sums = (total * 1);
                Txtfinalvat.Text = sums.ToString();
            }
            else
            {
                double sum = (total + (total / tax));
                Txtfinalvat.Text = sum.ToString();
                double taxs = Convert.ToDouble(Txtfinalvat.Text);

            }
            //double total = Convert.ToDouble ((Convert.ToDouble(Txttotalamount.Text.Trim)+((Txttotalamount.Text.Trim()) * (Convert.ToDouble(Txttax.Text.Trim())));

            // double total = Convert.ToDouble((Convert.ToDouble(txtamount.Text.Trim())) * (Convert.ToDouble(Txtvat.Text.Trim())));
            //Txtfuelamount.Text = total.ToString();
            //  Txtfinalvat.Text = total.ToString();
        }
        catch
        {
        }
    }

    protected void Txttotalamount_TextChanged(object sender, EventArgs e)
    {
        double total = Convert.ToDouble(Txttotalamount.Text);
        if (Txttax.Text == "")
        {
            double sums = (total * 1);
            Txtfinalcost.Text = sums.ToString();
        }

    }

    protected void Txttax_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Fpvehicle.SaveChanges();
            string strva = Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 4].Text.ToString();
            Txttotalamount.Text = strva;
            double total = Convert.ToDouble(Txttotalamount.Text);
            double tax = Convert.ToDouble(Txttax.Text);
            if (Txttax.Text == "0")
            {
                double sums = (total * 1);
                Txtfinalcost.Text = sums.ToString();
            }

            else
            {
                double sum = (total + (total / tax));
                //double total = Convert.ToDouble ((Convert.ToDouble(Txttotalamount.Text.Trim)+((Txttotalamount.Text.Trim()) * (Convert.ToDouble(Txttax.Text.Trim())));
                Txtfinalcost.Text = sum.ToString();
                double cost = Convert.ToDouble(Txtfinalcost.Text);
            }
        }
        catch
        {
        }
    }

    protected void btntolclick(object sender, EventArgs e)
    {
        // btnfocus.Focus();
    }

    public void college()
    {
        try
        {

            string college = "select college_code,collname from collinfo ";
            if (college != "")
            {
                ds = obj.select_method(college, hat, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlselectcollege.DataSource = ds;
                    ddlselectcollege.DataTextField = "collname";
                    ddlselectcollege.DataValueField = "college_code";
                    ddlselectcollege.DataBind();

                }
            }
            ddlselectcollege.Items.Insert(0, "All");
        }
        catch
        { }
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
        lbl.Add(Label3);
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

    protected void bindRemark()
    {
        //try
        //{
        //    ddl_itemtype.Items.Clear();
        //    ds.Clear();
        //    string sql = "select distinct Vehicle_Type,Vehicle_id  from vehicle_usage where item ='' and CollegeCode ='" + ddlselectcollege.SelectedValue + "'";
        //    ds = d2.select_method_wo_parameter(sql, "TEXT");
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        ddl_itemtype.DataSource = ds;
        //        ddl_itemtype.DataTextField = "Vehicle_Type";
        //        ddl_itemtype.DataValueField = "Vehicle_id";
        //        ddl_itemtype.DataBind();
        //    }
        //    //ddl_tccertificateissuedate.Items.Insert(0, new ListItem("Select", "0"));
        //}
        //catch { }
    }

    protected void btnreovecritreia_Click(object sender, EventArgs e)  //added by raghul dec 26 2017
    {
        try
        {
            if (ddl_itemtype.Items.Count > 0)
            {
                string deleteqry = "delete TextValTable where TextCriteria='TRREM' and college_code='" + Convert.ToString(Session["collegecode"]) + "' and TextVal='" + ddl_itemtype.SelectedItem.Text.Trim() + "'";
                int res = dirAcc.deleteData(deleteqry);
                if (res > 0)
                {
                    bindMethod();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btn_addgroup_Click(object sender, EventArgs e)  //added by raghul dec 26 2017
    {
        try
        {
            string textvalue = txt_addgroup.Text;
            if (!string.IsNullOrEmpty(textvalue))
            {
                ListItem li = new ListItem();
                li.Text = textvalue.Trim();
                if (ddl_itemtype.Items.Contains(li))
                {
                    plusdiv.Visible = true;
                    lblerror.Visible = true;
                    lblerror.Text = "Name has been already entered";
                }
                else
                {
                    string inseertqry = "insert into TextValTable (TextVal,TextCriteria,college_code) values('" + textvalue.Trim() + "','TRREM','" + Convert.ToString(Session["collegecode"]) + "')";
                    int i = dirAcc.insertData(inseertqry);
                    if (i > 0)
                    {
                        bindMethod();
                    }
                    plusdiv.Visible = false;
                    txt_addgroup.Text = string.Empty;
                }
            }
            else
            {
                lblerror.Text = "please enter you Remark";
                lblerror.Visible = true;
            }
        }
        catch
        {
        }
    }

    protected void bindMethod()  //added by raghul dec 26 2017  
    {
        string getqry = "select TextVal,TextCode from TextValTable where TextCriteria='TRREM' and college_code='" + Convert.ToString(Session["collegecode"]) + "'";
        DataTable dtvalues = dirAcc.selectDataTable(getqry);
        //ddl_itemtype
        if (dtvalues.Rows.Count > 0)
        {
            ddl_itemtype.DataSource = dtvalues;
            ddl_itemtype.DataTextField = "TextVal";
            ddl_itemtype.DataValueField = "TextCode";
            ddl_itemtype.DataBind();
        }
        else
        {
            ddl_itemtype.Items.Clear();
        }
        ddl_itemtype.Items.Insert(0, new ListItem(string.Empty, ""));
    }

    protected void btn_exitaddgroup_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = false;
        panel_addgroup.Visible = false;
        txt_addgroup.Text = "";
    }

    protected void btnnewcriteria_Click(object sender, EventArgs e)
    {
        try
        {
            plusdiv.Visible = true;
            panel_addgroup.Visible = true;
            lblcriteria.Text = "Remark";
            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    //protected void ddlcriteria_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    clear();
    //}

    protected void btnnewItemSave_Click(object sender, EventArgs e)
    {
        try
        {
            string newitem = txtADDnewItem.Text.Trim();
            ddl_itemtype.Items.Add(newitem);
            txtADDnewItem.Text = string.Empty;
            popupnewitem.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnnewItemExit_Click(object sender, EventArgs e)
    {
        try
        {
            txtADDnewItem.Text = string.Empty;
            popupnewitem.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    public void binditem()
    {

    }
}

