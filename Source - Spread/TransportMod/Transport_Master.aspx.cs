using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Collections;
using FarPoint.Web.Spread;
using System.Text.RegularExpressions;
using System.Web.UI.DataVisualization.Charting;
using System.Net;
using System.IO;
//using Ref_Service;

public partial class Transport_Master : System.Web.UI.Page
{
    public SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    string user_code = string.Empty;
    string ddl_sess = string.Empty;
    string vech_values = string.Empty;
    string route_values = string.Empty;
    string stage_values = string.Empty;
    string event_values = string.Empty;
    string user_id = string.Empty;
    string collegecode = string.Empty;
    string SenderID = string.Empty;
    string Password = string.Empty;
    string mobile = string.Empty;
    string sms_msg = string.Empty;
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    string PageLogOut = "";

    protected void Page_Load(object sender, EventArgs e)
    {

        user_code = Session["UserCode"].ToString();
        collegecode = Session["collegecode"].ToString();
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
        send_sms();
        // Servicel ghh = new Servicel();
        //Console.WriteLine(ghh
        //Service ser = new Service();
        //ser.InsertGPRSData();        
        FpTransport.Sheets[0].AutoPostBack = true;
        FpTransport.CommandBar.Visible = true;
        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        style.Font.Size = 10;
        style.Font.Bold = true;
        FpTransport.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        FpTransport.Sheets[0].AllowTableCorner = true;
        FpTransport.Sheets[0].RowHeader.Visible = false;
        FpTransport.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        //FpTransport.Sheets[0].ColumnHeader.DefaultStyle.ForeColor = Color.White;
        //FpTransport.Sheets[0].ColumnHeader.DefaultStyle.BackColor = Color.Black;
        FpTransport.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        FpTransport.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        //FpTransport.Sheets[0].DefaultRowHeight = 20;
        FpTransport.Sheets[0].DefaultColumnWidth = 50;
        FpTransport.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        FpTransport.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        FpTransport.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        FpTransport.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        FpTransport.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        FpTransport.Sheets[0].DefaultStyle.Font.Bold = false;
        FpTransport.SheetCorner.Cells[0, 0].Font.Bold = true;
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
                    chkls_vech.Items.Add(list_vehicle_id);
                    chkls_vech.Items[incre_veh - 1].Selected = true;
                }
            }
            if (ddl_session.Text == "Morning")
            {
                ddl_sess = "M";
            }
            else if (ddl_session.Text == "Afternoon")
            {
                ddl_sess = "A";
            }
            else
            {
                ddl_sess = "M','A";
            }
            con.Close();
            con.Open();
            SqlCommand cmd_route = new SqlCommand("select distinct Route_ID from routemaster where sess in('" + ddl_sess + "')", con);
            SqlDataReader rdr_route = cmd_route.ExecuteReader();
            int incre_route = 0;

            while (rdr_route.Read())
            {
                if (rdr_route.HasRows == true)
                {
                    incre_route++;
                    System.Web.UI.WebControls.ListItem list_route = new System.Web.UI.WebControls.ListItem();

                    //list_route.Value = (rdr_route["Route_ID"].ToString());
                    list_route.Text = (rdr_route["Route_ID"].ToString());
                    Chkls_route.Items.Add(list_route);
                    Chkls_route.Items[incre_route - 1].Selected = true;
                }
            }

            con.Close();
            con.Open();
            SqlCommand cmd_stage = new SqlCommand("select distinct s.stage_id,s.stage_name from stage_master s,RouteMaster r where cast(r.stage_name as varchar(100))=cast(s.stage_id as varchar(100))  and sess in('" + ddl_sess + "')", con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd_stage);
            DataSet rdr_stage = new DataSet();
            sda.Fill(rdr_stage);
            if (rdr_stage.Tables[0].Rows.Count > 0)
            {
                Chkls_stage.DataSource = rdr_stage;
                Chkls_stage.DataTextField = "stage_name";
                Chkls_stage.DataValueField = "stage_id";
                Chkls_stage.DataBind();
            }
           
           

            chkls_display.Items.Add(new System.Web.UI.WebControls.ListItem("PlaningTimeIn", "0"));
            chkls_display.Items.Add(new System.Web.UI.WebControls.ListItem("PlaningTimeOut", "1"));
            chkls_display.Items.Add(new System.Web.UI.WebControls.ListItem("ActualTimeIn", "2"));
            chkls_display.Items.Add(new System.Web.UI.WebControls.ListItem("ActualTimeOut", "3"));
            //chkls_display.Items.Add(new System.Web.UI.WebControls.ListItem("Color", "4"));
            chkls_display.Items[2].Selected = true;
           
            ddl_session.Text = "Both";
            txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            Rbtnlstinout.Items[0].Selected = false;
            btngo_Click1(sender, e);

        }

        if (ddl_view.Text == "Monitor Screen")
        {

            ddl_view_type.Visible = false;
            lbl_view_type.Visible = false;

            txt_events.Enabled = true;
        }
        else if (ddl_view.Text == "Alter Report")
        {
            ddl_view_type.Visible = false;
            lbl_view_type.Visible = false;

            txt_events.Enabled = false;
        }
        else
        {
            ddl_view_type.Visible = true;
            lbl_view_type.Visible = true;
            txt_events.Enabled = false;
        }
        Error_Msg.Visible = false;
        //FpTransport.Visible = false;


    }

    protected void chk_vech_ChekedChange(object sender, EventArgs e)
    {

        try
        {
            vech_values = "";
            if (chk_vech.Checked == true)
            {
                for (int i = 0; i < chkls_vech.Items.Count; i++)
                {
                    chkls_vech.Items[i].Selected = true;
                    txt_vech.Text = "Vehicle(" + (chkls_vech.Items.Count) + ")";
                    if (vech_values == "")
                    {
                        vech_values = chkls_vech.Items[i].Text.ToString();
                    }
                    else
                    {
                        vech_values = vech_values + "','" + chkls_vech.Items[i].Text;
                    }
                }
            }
            else
            {
                for (int i = 0; i < chkls_vech.Items.Count; i++)
                {
                    chkls_vech.Items[i].Selected = false;
                    txt_vech.Text = "--Select--";
                }
            }
        }

        catch (Exception ex)
        {
            throw ex;
        }
        Bind_Routes();
    }

    protected void chkls_vech_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            vech_values = "";
            int vech_count = 0;
            for (int i = 0; i < chkls_vech.Items.Count; i++)
            {
                if (chkls_vech.Items[i].Selected == true)
                {
                    vech_count = vech_count + 1;
                    txt_vech.Text = "Vehicle(" + vech_count.ToString() + ")";
                    if (vech_values == "")
                    {
                        vech_values = chkls_vech.Items[i].Text.ToString();
                    }
                    else
                    {
                        vech_values = vech_values + "','" + chkls_vech.Items[i].Text;
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
        Bind_Routes();


    }

    protected void Chk_route_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            route_values = "";
            if (Chk_route.Checked == true)
            {
                for (int i = 0; i < Chkls_route.Items.Count; i++)
                {
                    Chkls_route.Items[i].Selected = true;
                    txt_route.Text = "Route(" + Chkls_route.Items.Count + ")";

                    if (route_values == "")
                    {
                        route_values = Chkls_route.Items[i].Text.ToString();
                    }
                    else
                    {
                        route_values = route_values + "','" + Chkls_route.Items[i].Text;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Chkls_route.Items.Count; i++)
                {
                    Chkls_route.Items[i].Selected = false;
                    txt_route.Text = "--Select--";
                }
            }
        }

        catch (Exception ex)
        {
            throw ex;
        }
        Bind_stage();
    }

    protected void Chkls_route_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            route_values = "";
            int route_count = 0;
            for (int i = 0; i < Chkls_route.Items.Count; i++)
            {
                if (Chkls_route.Items[i].Selected == true)
                {
                    route_count = route_count + 1;
                    txt_route.Text = "Route(" + route_count.ToString() + ")";
                    if (route_values == "")
                    {
                        route_values = Chkls_route.Items[i].Text.ToString();
                    }
                    else
                    {
                        route_values = route_values + "','" + Chkls_route.Items[i].Text;
                    }
                }
            }

            if (route_count == 0)
            {
                txt_route.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

        Bind_stage();

    }

    protected void Chk_stage_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            stage_values = "";
            if (Chk_stage.Checked == true)
            {
                for (int i = 0; i < Chkls_stage.Items.Count; i++)
                {
                    Chkls_stage.Items[i].Selected = true;
                    txt_stage.Text = "Stage(" + Chkls_stage.Items.Count + ")";
                    if (stage_values == "")
                    {
                        stage_values = Chkls_stage.Items[i].Value.ToString();  //Chkls_stage.Items[i].Text.ToString();
                    }
                    else
                    {
                        stage_values = stage_values + "','" + Chkls_stage.Items[i].Value.ToString ();  //Chkls_stage.Items[i].Text;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Chkls_stage.Items.Count; i++)
                {
                    Chkls_stage.Items[i].Selected = false;
                    txt_stage.Text = "--Select--";
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void Chkls_stage_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            stage_values = "";
            int stage_count = 0;
            for (int i = 0; i < Chkls_stage.Items.Count; i++)
            {
                if (Chkls_stage.Items[i].Selected == true)
                {
                    stage_count = stage_count + 1;
                    txt_stage.Text = "Stage(" + stage_count.ToString() + ")";
                    if (stage_values == "")
                    {
                        stage_values = Chkls_stage.Items[i].Value.ToString(); //Chkls_stage.Items[i].Text.ToString();
                    }
                    else
                    {
                        stage_values = stage_values + "','" + Chkls_stage.Items[i].Value.ToString(); 
                    }
                }
            }

            if (stage_count == 0)
            {
                txt_stage.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void txt_vech_TextChanged(object sender, EventArgs e)
    {

    }

    protected void ddl_view_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_view.Text == "Chart")
        {
            ddl_view_type.Visible = true;
            lbl_view_type.Visible = true;
        }
        else if (ddl_view.Text == "Monitor Screen")
        {
            ddl_view_type.Visible = false;
            lbl_view_type.Visible = false;
        }
    }
    protected void ddl_view_type_TextChanged(object sender, EventArgs e)
    {
        if (ddl_view.Text == "Chart")
        {
            ddl_view_type.Visible = true;
            lbl_view_type.Visible = true;
        }
        else if (ddl_view.Text == "Monitor Screen")
        {
            ddl_view_type.Visible = false;
            lbl_view_type.Visible = false;
        }
    }
    void Bind_Routes()
    {
        if (ddl_session.Text == "Morning")
        {
            ddl_sess = "M";
        }
        else if (ddl_session.Text == "Afternoon")
        {
            ddl_sess = "A";
        }
        else
        {
            ddl_sess = "M','A";
        }
        con.Close();
        con.Open();
        int count_items = 0;
        SqlCommand cmd_bind_route = new SqlCommand("select distinct r.Route_ID from routemaster r,vehicle_master v where r.Route_id=v.Route and v.Veh_Id in('" + vech_values + "') and r.sess in('" + ddl_sess + "')", con);
        SqlDataAdapter ad_bind_route = new SqlDataAdapter(cmd_bind_route);
        DataTable dt_bind_route = new DataTable();
        ad_bind_route.Fill(dt_bind_route);

        if (dt_bind_route.Rows.Count > 0)
        {
            Chkls_route.DataSource = dt_bind_route;
            Chkls_route.DataTextField = "Route_ID";
            Chkls_route.DataBind();

            for (int i = 0; i < Chkls_route.Items.Count; i++)
            {
                Chkls_route.Items[i].Selected = true;
                if (Chkls_route.Items[i].Selected == true)
                {
                    count_items += 1;
                }
                if (Chkls_route.Items.Count == count_items)
                {
                    Chk_route.Checked = true;
                }
            }
        }
    }

    void Bind_stage()
    {
        if (ddl_session.Text == "Morning")
        {
            ddl_sess = "M";
        }
        else if (ddl_session.Text == "Afternoon")
        {
            ddl_sess = "A";
        }
        else
        {
            ddl_sess = "M','A";
        }
        con.Close();
        con.Open();
        int count_items = 0;
        SqlCommand cmd_bind_stage = new SqlCommand("select distinct s.stage_id,s.stage_name from stage_master s,RouteMaster r where cast(r.stage_name as varchar(100))=cast(s.stage_id as varchar(100))  and Route_ID in('" + route_values + "')", con);
        SqlDataAdapter ad_bind_stage = new SqlDataAdapter(cmd_bind_stage);
        DataTable dt_bind_stage = new DataTable();
        ad_bind_stage.Fill(dt_bind_stage);

        if (dt_bind_stage.Rows.Count > 0)
        {
            Chkls_stage.DataSource = dt_bind_stage;
            Chkls_stage.DataTextField = "Stage_Name";
            Chkls_stage.DataValueField = "stage_id";
            Chkls_stage.DataBind();

            for (int i = 0; i < Chkls_stage.Items.Count; i++)
            {
                Chkls_stage.Items[i].Selected = true;
                if (Chkls_stage.Items[i].Selected == true)
                {
                    count_items += 1;
                }
                if (Chkls_stage.Items.Count == count_items)
                {
                    Chk_stage.Checked = true;
                }
            }
        }


    }


    protected void btngo_Click1(object sender, EventArgs e)
    {
        //try
        //{
        string vech_all = string.Empty;
        string vehid_all = string.Empty;
        string sess_all = string.Empty;
        string route_all = string.Empty;
        string stage_all = string.Empty;
        string display_all = string.Empty;
        string stage_header = string.Empty;
        string route_header = string.Empty;
        int sess_count = 1;

        string[] split_Date = txt_date.Text.Split(new char[] { '/' });
        string set_date = split_Date[1] + '/' + split_Date[0] + '/' + split_Date[2];
        int gps_year = Convert.ToInt32(split_Date[2]) - 2000;

        string date_day = split_Date[0].ToString();
        string date_month = split_Date[1].ToString();

        if (date_day.Length == 1)
        {
            date_day = "0" + date_day;
        }

        if (date_day.Length == 1)
        {
            date_month = "0" + date_month;
        }

        string GPRS_date = date_day + date_month + Convert.ToString(gps_year);

        DateTime from_date = Convert.ToDateTime(set_date);

        for (int vech_count = 0; vech_count < chkls_vech.Items.Count; vech_count++)
        {
            if (chkls_vech.Items[vech_count].Selected == true)
            {
                if (vech_all == "")
                {
                    vech_all = chkls_vech.Items[vech_count].Text;
                    vehid_all = chkls_vech.Items[vech_count].Text;
                }
                else
                {
                    vech_all = vech_all + "','" + chkls_vech.Items[vech_count].Text;
                    vehid_all = vehid_all + "@" + chkls_vech.Items[vech_count].Text;

                }
            }
        }

        if (ddl_session.Text == "Morning")
        {
            ddl_sess = "M";
        }
        else if (ddl_session.Text == "Afternoon")
        {
            ddl_sess = "A";
        }
        else
        {
            ddl_sess = "M','A";
            sess_count = 2;
        }

        for (int route_count = 0; route_count < Chkls_route.Items.Count; route_count++)
        {
            if (Chkls_route.Items[route_count].Selected == true)
            {
                if (route_all == "")
                {
                    route_all = Chkls_route.Items[route_count].Text;

                }
                else
                {
                    route_all = route_all + "','" + Chkls_route.Items[route_count].Text;
                }
            }
        }

        for (int stage_count = 0; stage_count < Chkls_stage.Items.Count; stage_count++)
        {
            if (Chkls_stage.Items[stage_count].Selected == true)
            {
                // max_count++;

                if (stage_all == "")
                {
                    stage_all = Chkls_stage.Items[stage_count].Value; // Chkls_stage.Items[stage_count].Text;
                    stage_header = Chkls_stage.Items[stage_count].Value;  //Chkls_stage.Items[stage_count].Text;
                }
                else
                {
                    stage_all = stage_all + "','" + Chkls_stage.Items[stage_count].Value;  //Chkls_stage.Items[stage_count].Text;
                    stage_header = stage_header + "," + Chkls_stage.Items[stage_count].Value;  //Chkls_stage.Items[stage_count].Text;

                }
            }
        }

        ArrayList Aryli_display = new ArrayList();

        string Default_item = "Stage_Name";
        string Default_item1 = "Total_Student";

        Aryli_display.Add(Default_item);
        int num_of_selected = 1;

        for (int display_count = 0; display_count < chkls_display.Items.Count; display_count++)
        {
            if (chkls_display.Items[display_count].Selected == true)
            {
                num_of_selected++;
                string temp_li_display = chkls_display.Items[display_count].ToString();
                Aryli_display.Add(temp_li_display);
            }
        }

        Aryli_display.Add(Default_item1);
        num_of_selected++;
        int temp_incre = 0;

        if (Aryli_display.Contains("Color") == true)
        {
            temp_incre = num_of_selected - 1;
        }
        else
        {
            temp_incre = num_of_selected;
        }

        con.Close();
        con.Open();
        SqlCommand cmd_stage_count = new SqlCommand("select distinct count(r.stage_name) as count from stage_master s,RouteMaster r where cast(r.stage_name as varchar(100))=cast(s.stage_id as varchar(100))  and Route_ID in('" + route_all + "') and sess in('" + ddl_sess + "')  group by route_id", con);
        SqlDataAdapter ad_stage_count = new SqlDataAdapter(cmd_stage_count);
        DataTable dt_stage_count = new DataTable();
        ad_stage_count.Fill(dt_stage_count);

        int max_count = 0;
        if (dt_stage_count.Rows.Count > 0)
        {
            for (int c = 0; c < dt_stage_count.Rows.Count; c++)
            {
                int temp_stage_count = 0;
                temp_stage_count = Convert.ToInt32(dt_stage_count.Rows[c]["Count"]);
                if (temp_stage_count > max_count)
                {
                    max_count = temp_stage_count;
                }
            }
        }
        if (ddl_session.Text == "Both")
        {
            max_count = max_count / 2;
        }
        FpTransport.Sheets[0].ColumnCount = 3;
        FpTransport.Sheets[0].ColumnHeader.Cells[FpTransport.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "Sl.No";
        FpTransport.Sheets[0].ColumnHeader.Cells[FpTransport.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "Vehicle No";
        FpTransport.Sheets[0].ColumnHeader.Cells[FpTransport.Sheets[0].ColumnHeader.RowCount - 1, 2].Text = "Session";

        FpTransport.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
        FpTransport.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
        FpTransport.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
        FpTransport.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);

        string stage_text = "Stage";
        for (int k = 1; k <= max_count; k++)
        {
            string text_stage = string.Empty;

            text_stage = stage_text + "-" + k.ToString();

            FpTransport.Sheets[0].ColumnCount++;
            FpTransport.Sheets[0].ColumnHeader.Cells[FpTransport.Sheets[0].ColumnHeader.RowCount - 1, FpTransport.Sheets[0].ColumnCount - 1].Text = text_stage.ToString();
            FpTransport.Sheets[0].ColumnHeader.Cells[FpTransport.Sheets[0].ColumnHeader.RowCount - 1, FpTransport.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;

        }

        con.Close();
        con.Open();
        SqlCommand cmd_grid_data = new SqlCommand("select v.Veh_id,v.route,s.Stage_Name,r.Arr_Time as PlaningTimeIn,r.Dep_Time as PlaningTimeOut,r.sess,s.address,s.Stage_id from vehicle_master v,routemaster r ,stage_master s where cast(r.stage_name as varchar(100))=cast(s.stage_id as varchar(100)) and v.veh_id in('" + vech_all + "') and r.route_id in('" + route_all + "') and r.Stage_Name in('" + stage_all + "') and v.route=r.route_id and r.sess in('" + ddl_sess + "')", con);
        SqlDataAdapter ad_grid_date = new SqlDataAdapter(cmd_grid_data);
        DataTable dt_grid_data = new DataTable();
        ad_grid_date.Fill(dt_grid_data);

        con.Close();
        con.Open();
        SqlCommand cmd_stud_plan_count = new SqlCommand("select distinct r.vehid,r.boarding,count(*) as Total_Student, isnull( s.address,'') as address from registration r,routemaster m  ,stage_master s where cast(m.stage_name as varchar(100))=cast(s.stage_id as varchar(100)) and r.vehid in('" + vech_all + "') and r.boarding in('" + stage_all + "') and cast(r.boarding as varchar(100)) =cast(m.stage_name as varchar(100)) and m.sess='m' group by r.vehid ,r.boarding,s.address order by vehid", con);
        SqlDataAdapter ad_stud_plan_count = new SqlDataAdapter(cmd_stud_plan_count);
        DataTable dt_stud_plan_count = new DataTable();
        ad_stud_plan_count.Fill(dt_stud_plan_count);

        con.Close();
        con.Open();
        SqlCommand cmd_get_GPRS_data = new SqlCommand("select * from VTSGPRSData where date='" + GPRS_date + "' and rfiddata<>'00000000'", con);
        SqlDataAdapter ad_get_GPRS_data = new SqlDataAdapter(cmd_get_GPRS_data);
        DataTable dt_get_GPRS_data = new DataTable();
        ad_get_GPRS_data.Fill(dt_get_GPRS_data);

        con.Close();
        con.Open();

        SqlCommand cmd_get_min = new SqlCommand("select min(time) as time,vehicleid,date,rfiddata,googlelocation,flag_status from vtsgprsdata where date='" + GPRS_date + "' and rfiddata<>'00000000' group by rfiddata,googlelocation,date,flag_status,vehicleid", con);
        SqlDataAdapter ad_get_min = new SqlDataAdapter(cmd_get_min);
        DataTable dt_get_min = new DataTable();
        ad_get_min.Fill(dt_get_min);

        con.Close();
        con.Open();

        SqlCommand cmd_get_max = new SqlCommand("select max(time) as time,vehicleid,date,rfiddata,googlelocation,flag_status from vtsgprsdata where date='" + GPRS_date + "' and rfiddata<>'00000000' group by rfiddata,googlelocation,date,flag_status,vehicleid", con);
        SqlDataAdapter ad_get_max = new SqlDataAdapter(cmd_get_max);
        DataTable dt_get_max = new DataTable();
        ad_get_max.Fill(dt_get_max);

        //DataView dv_test = new DataView();
        //dt_get_GPRS_data.DefaultView.RowFilter = " time=Max(time)";
        //dv_test = dt_get_GPRS_data.DefaultView;

        if (chktimeing.Checked == false && chkabs.Checked == false)
        {
            if (ddl_view.Text == "Monitor Screen")
            {
                if (dt_grid_data.Rows.Count > 0)
                {
                    FpTransport.Sheets[0].RowCount = 0;
                    int Row_cnt = 0;
                    int sl_no = 0;
                    string temp = string.Empty;
                    ArrayList al_vech = new ArrayList();
                    int cell_row_cnt = -temp_incre;

                    for (int cc = 0; cc < dt_grid_data.Rows.Count; cc++)
                    {
                        string temp_vech_name = string.Empty;
                        temp_vech_name = dt_grid_data.Rows[cc]["Veh_id"].ToString();

                        if (al_vech.Contains(temp_vech_name) == false)
                        {
                            sl_no++;
                            for (int ss = 0; ss < sess_count; ss++)
                            {

                                int first_time = 0;
                                int second_time = 1;
                                string temp_sess_input = string.Empty;
                                int increment = 1;

                                for (int dis_count = 0; dis_count < num_of_selected; dis_count++)
                                {
                                    

                                    increment++;
                                    FpTransport.Sheets[0].RowCount = Convert.ToInt32(FpTransport.Sheets[0].RowCount) + 1;
                                    Row_cnt = Convert.ToInt32(FpTransport.Sheets[0].RowCount) - 1;


                                    FpTransport.Sheets[0].Cells[Row_cnt, 0].Text = sl_no.ToString();
                                    FpTransport.Sheets[0].Cells[Row_cnt, 1].Text = temp_vech_name.ToString();

                                    if (increment % 2 == 0)
                                    {
                                        FpTransport.Sheets[0].Cells[Row_cnt, 1].BackColor = Color.AliceBlue;
                                    }
                                    else
                                    {
                                        FpTransport.Sheets[0].Cells[Row_cnt, 1].BackColor = Color.White;
                                    }
                                    if (ddl_session.Text == "Both")
                                    {
                                        if (ss == first_time)
                                        {
                                            temp_sess_input = "M";
                                            FpTransport.Sheets[0].Cells[Row_cnt, 2].Text = "Morning";

                                        }
                                        else if (ss == second_time)
                                        {
                                            temp_sess_input = "A";
                                            FpTransport.Sheets[0].Cells[Row_cnt, 2].Text = "Afternoon";
                                        }
                                    }
                                    else
                                    {
                                        temp_sess_input = ddl_sess;
                                        FpTransport.Sheets[0].Cells[Row_cnt, 2].Text = ddl_session.Text;
                                    }

                                    string column_index = Aryli_display[dis_count].ToString();

                                    if (column_index == "ActualTimeIn" || column_index == "ActualTimeOut")
                                    {
                                        int Col_cnt = 3;
                                        //cell_row_cnt = cell_row_cnt + temp_incre;
                                        int rowcount = cell_row_cnt + temp_incre;
                                        for (int k = 0; k < max_count; k++)
                                        {
                                            string temp_cell_value = string.Empty;

                                            FpTransport.SaveChanges();
                                            if (FpTransport.Sheets[0].Cells[rowcount, Col_cnt].Text.ToString() != null && FpTransport.Sheets[0].Cells[rowcount, Col_cnt].Text.ToString() != "")
                                            {
                                                temp_cell_value = FpTransport.Sheets[0].Cells[rowcount, Col_cnt].Tag.ToString();
                                            }
                                            
                                            if (temp_cell_value != "")
                                            {
                                                DataView dv_Actual_grid_data = new DataView();

                                                if (column_index == "ActualTimeIn")  //if (temp_sess_input == "M")
                                                {

                                                    dt_get_min.DefaultView.RowFilter = "VehicleId='" + temp_vech_name + "'and GoogleLocation='" + temp_cell_value + "' ";
                                                    dv_Actual_grid_data = dt_get_min.DefaultView;
                                                }
                                                else if (column_index == "ActualTimeOut")// else if (temp_sess_input == "A")
                                                {
                                                    dt_get_max.DefaultView.RowFilter = "VehicleId='" + temp_vech_name + "'and GoogleLocation='" + temp_cell_value + "' ";
                                                    dv_Actual_grid_data = dt_get_max.DefaultView;
                                                }
                                                

                                                if (dv_Actual_grid_data.Count > 0)
                                                {

                                                    ArrayList Al_temp_Location = new ArrayList();
                                                    foreach (DataRowView data_row_view in dv_Actual_grid_data)
                                                    {
                                                        string location = data_row_view["GoogleLocation"].ToString();
                                                        if (Al_temp_Location.Contains(location) == false)
                                                        {
                                                            string Sp_hour = string.Empty;
                                                            string Sp_Min = string.Empty;
                                                            string Sp_Sec = string.Empty;

                                                            string input = data_row_view["Time"].ToString();
                                                            string[] sub_string = Regex.Split(input, "");

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

                                                            string final_time = Sp_hour + ":" + Sp_Min + ":" + Sp_Sec + time_sess;
                                                            FpTransport.Sheets[0].Cells[Row_cnt, Col_cnt].Text = final_time;

                                                            Col_cnt++;
                                                            Al_temp_Location.Add(location);
                                                        }
                                                    }
                                                }

                                                else
                                                {
                                                    FpTransport.Sheets[0].Cells[Row_cnt, Col_cnt].Text = "Nil";
                                                    Col_cnt++;
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        DataView dv_grid_data = new DataView();
                                        dt_grid_data.DefaultView.RowFilter = "veh_id ='" + temp_vech_name + "' and sess='" + temp_sess_input + "'";
                                        dv_grid_data = dt_grid_data.DefaultView;

                                        int Col_cnt = 3;

                                        foreach (DataRowView datarowview in dv_grid_data)
                                        {
                                            DataView dv_stud_plan_count = new DataView();
                                            dt_stud_plan_count.DefaultView.RowFilter = "vehid='" + temp_vech_name + "' and boarding='" + datarowview["Stage_id"].ToString() + "'";
                                            dv_stud_plan_count = dt_stud_plan_count.DefaultView;

                                            if (column_index != "Total_Student")
                                            {

                                                FpTransport.Sheets[0].Cells[Row_cnt, Col_cnt].Text = datarowview[column_index].ToString();
                                                FpTransport.Sheets[0].Cells[Row_cnt, Col_cnt].Tag = datarowview["address"].ToString();

                                            }

                                            else
                                            {
                                                if (dv_stud_plan_count.Count > 0)
                                                {

                                                    foreach (DataRowView countview in dv_stud_plan_count)
                                                    {
                                                        DataView dv_get_GPRS_data = new DataView();
                                                        dt_get_GPRS_data.DefaultView.RowFilter = "Vehicleid='" + countview["vehid"].ToString() + "' and GoogleLocation ='" + countview["address"].ToString() + "'";
                                                        dv_get_GPRS_data = dt_get_GPRS_data.DefaultView;
                                                        if (dv_get_GPRS_data.Count > 0)
                                                        {
                                                            int plan_Stu_Total = Convert.ToInt32(countview[column_index].ToString());
                                                            int Actual_Stu_Total = dv_get_GPRS_data.Count;
                                                            // int Actual_Stu_Total = Convert.ToInt32(dv_location_count[0]["Location_Count"]);
                                                            int Count_Answer = plan_Stu_Total - Actual_Stu_Total;
                                                            string count_result = Convert.ToString(Actual_Stu_Total) + "/" + Convert.ToString(plan_Stu_Total);

                                                            FpTransport.Sheets[0].Cells[Row_cnt, Col_cnt].Text = count_result;
                                                        }
                                                        else
                                                        {
                                                            FpTransport.Sheets[0].Cells[Row_cnt, Col_cnt].Text = countview[column_index].ToString();
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    FpTransport.Sheets[0].Cells[Row_cnt, Col_cnt].Text = "0";
                                                }
                                            }
                                            if (ss == first_time)
                                            {
                                                FpTransport.Sheets[0].Cells[Row_cnt, Col_cnt].BackColor = Color.Lavender;
                                                //FpTransport.Sheets[0].Cells[Row_cnt, Col_cnt].ForeColor = Color.White;
                                            }
                                            else if (ss == second_time)
                                            {
                                                FpTransport.Sheets[0].Cells[Row_cnt, Col_cnt].BackColor = Color.LavenderBlush;
                                            }
                                            Col_cnt++;
                                        }
                                    }
                                   al_vech.Add(temp_vech_name);

                                }

                            }
                        }
                    }
                }

                FpTransport.Sheets[0].PageSize = FpTransport.Sheets[0].RowCount;
                FpTransport.Visible = true;
                Fp_Absenties.Visible = false;
                Fp_InOut.Visible = false;

            }

            else if (ddl_view.Text == "Chart")
            {
                FpTransport.Visible = false;
                Fp_Absenties.Visible = false;
                Fp_InOut.Visible = false;
                Graph_Chart.Visible = false;

                //Chart1.Visible = true;

                con.Close();
                con.Open();
                SqlCommand cmd_driver_data = new SqlCommand("Select * from driverallotment", con);
                SqlDataAdapter ad_driver_data = new SqlDataAdapter(cmd_driver_data);
                DataTable dt_driver_data = new DataTable();
                ad_driver_data.Fill(dt_driver_data);

                con.Close();
                con.Open();
                SqlCommand cmd_route_data = new SqlCommand("select v.veh_id,r.route_id,r.stage_name from vehicle_master v,routemaster r where v.route=r.route_id", con);
                SqlDataAdapter ad_route_data = new SqlDataAdapter(cmd_route_data);
                DataTable dt_route_data = new DataTable();
                ad_route_data.Fill(dt_route_data);


                FpTransport.Visible = false;
                Chart1.Visible = true;
                if (dt_grid_data.Rows.Count > 0)
                {
                    ArrayList al_vech = new ArrayList();
                    List<string> ChartAreaList = new List<string>();

                    DataTable dt = new DataTable();
                    DataColumn dc;

                    dc = new DataColumn();
                    dc.ColumnName = "Veh";
                    dt.Columns.Add(dc);

                    dc = new DataColumn();
                    dc.ColumnName = "StageName";
                    dt.Columns.Add(dc);

                    dc = new DataColumn();
                    dc.ColumnName = "Time";
                    dt.Columns.Add(dc);

                    dc = new DataColumn();
                    dc.ColumnName = "Session";
                    dt.Columns.Add(dc);

                    DataRow dr;

                    int incre_chart = 0;
                    for (int cc = 0; cc < dt_grid_data.Rows.Count; cc++)
                    {
                        string temp_vech_name = string.Empty;
                        string temp_route_name = string.Empty;

                        temp_vech_name = dt_grid_data.Rows[cc]["Veh_id"].ToString();
                        temp_route_name = dt_grid_data.Rows[cc]["Route"].ToString();


                        if (al_vech.Contains(temp_vech_name) == false)
                        {

                            ChartAreaList.Add(temp_vech_name);

                            incre_chart++;
                            for (int ss = 0; ss < sess_count; ss++)
                            {
                                int first_time = 0;
                                int second_time = 1;
                                string temp_sess_input = string.Empty;

                                if (ddl_session.Text == "Both")
                                {

                                    if (ss == first_time)
                                    {
                                        temp_sess_input = "M";

                                    }
                                    else if (ss == second_time)
                                    {
                                        temp_sess_input = "A";
                                    }
                                }
                                else
                                {
                                    temp_sess_input = ddl_sess;
                                }

                                DataView dv_grid_data = new DataView();
                                dt_grid_data.DefaultView.RowFilter = "veh_id ='" + temp_vech_name + "' and sess='" + temp_sess_input + "'";
                                dv_grid_data = dt_grid_data.DefaultView;

                                if (dv_grid_data.Count > 0)
                                {
                                    ArrayList ar_stage = new ArrayList();
                                    foreach (DataRowView datarv in dv_grid_data)
                                    {
                                        string temp_stage_name = datarv["address"].ToString();

                                        if (ar_stage.Contains(temp_stage_name) == false)
                                        {
                                            string temp_pln_time = datarv["PlaningTimeIn"].ToString();

                                            DataView dv_Actual_grid_data = new DataView();

                                            if (temp_sess_input == "M")
                                            {

                                                dt_get_min.DefaultView.RowFilter = "VehicleId='" + temp_vech_name + "'and GoogleLocation='" + temp_stage_name + "' ";
                                                dv_Actual_grid_data = dt_get_min.DefaultView;
                                            }
                                            else if (temp_sess_input == "A")
                                            {
                                                dt_get_max.DefaultView.RowFilter = "VehicleId='" + temp_vech_name + "'and GoogleLocation='" + temp_stage_name + "' ";
                                                dv_Actual_grid_data = dt_get_max.DefaultView;
                                            }

                                            if (dv_Actual_grid_data.Count > 0)
                                            {
                                                foreach (DataRowView datrwvw in dv_Actual_grid_data)
                                                {
                                                    string temp_actual_time = datrwvw["Time"].ToString();

                                                    if (temp_actual_time != "")
                                                    {
                                                        string[] spl_pln_time;

                                                        if (temp_pln_time != "Halt" && temp_pln_time != "" && temp_pln_time != "-")
                                                        {

                                                            spl_pln_time = temp_pln_time.Split(' ');
                                                            int pln_time_1 = 0;
                                                            int pln_time_2 = 0;

                                                            string str_plan_time = spl_pln_time[0];
                                                            string str_plan_sess = spl_pln_time[1];

                                                            string[] str_spl_pln_time;

                                                            if (str_plan_time.Contains(":") == true)
                                                            {
                                                                str_spl_pln_time = str_plan_time.Split(':');

                                                                pln_time_1 = Convert.ToInt32(str_spl_pln_time[0]);
                                                                pln_time_2 = Convert.ToInt32(str_spl_pln_time[1]);

                                                            }

                                                            else if (str_plan_time.Contains(".") == true)
                                                            {
                                                                str_spl_pln_time = str_plan_time.Split('.');

                                                                pln_time_1 = Convert.ToInt32(str_spl_pln_time[0]);
                                                                pln_time_2 = Convert.ToInt32(str_spl_pln_time[1]);

                                                            }

                                                            string Sp_hour = string.Empty;
                                                            string Sp_Min = string.Empty;
                                                            string Sp_Sec = string.Empty;

                                                            string[] sub_string = Regex.Split(temp_actual_time, "");

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

                                                            string time_sess = "AM";
                                                            if (Convert.ToInt32(Sp_hour) > 12)
                                                            {
                                                                Sp_hour = Convert.ToString(Convert.ToInt32(Sp_hour) - 12);
                                                                time_sess = "PM";
                                                            }

                                                            string final_pln_time = pln_time_1 + "." + pln_time_2;
                                                            string final_act_time = Sp_hour + "." + Sp_Min;

                                                            double cnvrt_pln_time = Convert.ToDouble(final_pln_time);
                                                            double cnvrt_act_time = Convert.ToDouble(final_act_time);


                                                            if ((cnvrt_pln_time == cnvrt_act_time || cnvrt_pln_time > cnvrt_act_time) && time_sess == str_plan_sess)
                                                            {
                                                                dr = dt.NewRow();
                                                                dr["Veh"] = temp_vech_name;
                                                                //dr["Route"] = temp_route_name;
                                                                dr["StageName"] = temp_stage_name;
                                                                dr["Time"] = cnvrt_act_time;
                                                                dr["Session"] = temp_sess_input;
                                                                dt.Rows.Add(dr);
                                                            }

                                                            else
                                                            {
                                                                dr = dt.NewRow();
                                                                dr["Veh"] = temp_vech_name;
                                                                //dr["Route"] = temp_route_name;
                                                                dr["StageName"] = temp_stage_name;
                                                                dr["Time"] = -cnvrt_act_time;
                                                                dr["Session"] = temp_sess_input;
                                                                dt.Rows.Add(dr);
                                                            }
                                                        }

                                                    }

                                                    else
                                                    {

                                                    }
                                                }
                                            }

                                            ar_stage.Add(temp_stage_name);
                                        }
                                    }
                                }
                            }
                            al_vech.Add(temp_vech_name);

                            //--------------------------------------------------------------------------------------------------------------------------------
                            //Chart1.Titles[0].Text = "Chart Of Bus-A";
                            //Chart1.Titles[0].Font = new Font("Arial", 16f);
                            //Chart1.Titles[0].ForeColor = Color.Red;                              

                            //Chart1.Series.Add("Series" + incre_chart.ToString());
                            //Chart1.ChartAreas.Add("ChartArea" + incre_chart.ToString());

                            //Chart1.ChartAreas["ChartArea"+incre_chart.ToString()].AxisY.Minimum = -12;
                            //Chart1.ChartAreas["ChartArea" + incre_chart.ToString()].AxisY.Maximum = 12;
                            //Chart1.ChartAreas["ChartArea" + incre_chart.ToString()].AxisY.Interval = 1;

                            //Chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                            //Chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Coral;

                            //Chart1.DataSource = dt;

                            //Chart1.ChartAreas["ChartArea" + incre_chart.ToString()].AxisX.Title = "Stage Name";

                            //Chart1.ChartAreas["ChartArea" + incre_chart.ToString()].AxisY.Title = "Time";

                            //Chart1.Series["Series" + incre_chart.ToString()].XValueMember = "Stage";

                            //Chart1.Series["Series" + incre_chart.ToString()].YValueMembers = "Value";

                            //Chart1.DataBind();
                            //-------------------------------------------------------------------------------------------------------------------------------------
                        }
                    }

                    //Chart1.Titles[0].Text = "Chart Details Of Vehicles";
                    //Chart1.Titles[0].Font = new Font("Arial", 16f);
                    //Chart1.Titles[0].ForeColor = Color.Red;               

                    int width = 200;
                    int height = 250;
                    int height1 = 1000;
                    int width1 = 1000;
                    int wid_hit_incre = 0;

                    for (int ss = 0; ss < sess_count; ss++)
                    {
                        int first_time = 0;
                        int second_time = 1;
                        string temp_sess_input = string.Empty;

                        if (ddl_session.Text == "Both")
                        {

                            if (ss == first_time)
                            {
                                temp_sess_input = "M";

                            }
                            else if (ss == second_time)
                            {
                                temp_sess_input = "A";
                            }
                        }
                        else
                        {
                            temp_sess_input = ddl_sess;
                        }

                        int area_count = ChartAreaList.Count;


                        int tl_incre = -1;
                        for (int i = 0; i < area_count; i++)
                        {
                            string veh_name = ChartAreaList[i].ToString();

                            if (ddl_session.Text == "Both")
                            {
                                if (temp_sess_input == "A")
                                {
                                    veh_name = ChartAreaList[i].ToString() + "1";
                                }
                            }

                            DataView dv_view = new DataView();
                            dt.DefaultView.RowFilter = "Veh='" + veh_name + "' and Session='" + temp_sess_input + "'";
                            dv_view = dt.DefaultView;

                            DataView dv_driver_data = new DataView();
                            dt_driver_data.DefaultView.RowFilter = "Vehicle_Id='" + veh_name + "'";
                            dv_driver_data = dt_driver_data.DefaultView;

                            DataView dv_route_data = new DataView();
                            dt_route_data.DefaultView.RowFilter = "veh_id='" + veh_name + "'";
                            dv_route_data = dt_route_data.DefaultView;

                            int cnt = dv_route_data.Count / 2;


                            if (dv_view.Count > 0)
                            {
                                tl_incre++;
                                wid_hit_incre++;

                                Chart1.ChartAreas.Add(ChartAreaList[i]);
                                Chart1.Series.Add(ChartAreaList[i]);


                                Chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;

                                if (dv_driver_data.Count > 0)
                                {
                                    Chart1.Titles.Add(new Title("Vehicle Id-" + ChartAreaList[i].ToString() + "(" + dv_route_data[0]["Stage_Name"] + "-" + dv_route_data[cnt - 1]["Stage_Name"] + ")" + "\n" + "Driver: " + dv_driver_data[0]["Driver_Name"].ToString() + "-" + dv_driver_data[0]["Mobile_No"].ToString(), Docking.Top, new Font("Verdana", 10f, FontStyle.Bold), Color.Black));
                                }

                                else
                                {
                                    Chart1.Titles.Add(new Title("Vehicle Id-" + ChartAreaList[i].ToString() + "(" + dv_route_data[0]["Stage_Name"] + "-" + dv_route_data[cnt - 1]["Stage_Name"] + ")", Docking.Top, new Font("Verdana", 10f, FontStyle.Bold), Color.Black));
                                }
                                Chart1.Titles[tl_incre].DockedToChartArea = ChartAreaList[i].ToString();
                                //Chart1.Titles[tl_incre].ForeColor = Color.Red;


                                Chart1.ChartAreas[ChartAreaList[i]].AlignWithChartArea = ChartAreaList[i];

                                Chart1.ChartAreas[ChartAreaList[i]].AxisY.Minimum = -12;
                                Chart1.ChartAreas[ChartAreaList[i]].AxisY.Maximum = 12;
                                Chart1.ChartAreas[ChartAreaList[i]].AxisY.Interval = 1;
                                Chart1.ChartAreas[ChartAreaList[i]].AlignmentStyle = AreaAlignmentStyles.PlotPosition;
                                Chart1.ChartAreas[ChartAreaList[i]].AxisX.MajorGrid.Enabled = false;
                                Chart1.ChartAreas[ChartAreaList[i]].AxisY.MajorGrid.Enabled = false;
                                Chart1.ChartAreas[ChartAreaList[i]].AxisX.Title = "Stage Name";
                                Chart1.ChartAreas[ChartAreaList[i]].AxisX.TitleForeColor = Color.Blue;

                                if (ddl_view_type.Text == "3D Effect")
                                {
                                    Chart1.ChartAreas[ChartAreaList[i]].Area3DStyle.Enable3D = true;
                                }

                                Chart1.ChartAreas[ChartAreaList[i]].AxisY.Title = "Time";
                                Chart1.ChartAreas[ChartAreaList[i]].AxisY.TitleForeColor = Color.Blue;

                                if (tl_incre % 2 == 0)
                                {
                                    Chart1.ChartAreas[ChartAreaList[i]].BackColor = Color.AntiqueWhite;
                                }

                                else
                                {
                                    Chart1.ChartAreas[ChartAreaList[i]].BackColor = Color.CornflowerBlue;
                                }



                                Chart1.Series[ChartAreaList[i]].ChartArea = ChartAreaList[i];
                                Chart1.Series[ChartAreaList[i]]["DrawingStyle"] = "Cylinder";
                                Chart1.Series[ChartAreaList[i]].Points.DataBindXY(dv_view, "StageName", dv_view, "Time");
                                // Chart1.Series[ChartAreaList[i]].Color = Color.Green;

                                Chart1.Series[ChartAreaList[i]].Label = "#VALY";

                                // foreach (ChartSeriesItem item in chart1.Series[0].Items)
                                //System.Web.UI.DataVisualization.Charting.DataPoint aaa = new DataPoint();

                                Random random = new Random();
                                foreach (var item in Chart1.Series[ChartAreaList[i]].Points)
                                {
                                    string a = item.YValues[0].ToString();

                                    if (Convert.ToDouble(a) < 0.00)
                                    {
                                        item.Color = Color.Tomato;
                                    }
                                    else
                                    {
                                        item.Color = Color.LimeGreen;
                                    }
                                    //Color c = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                                    //item.Color = c;
                                }
                            }
                        }
                    }

                    if (wid_hit_incre > 4)
                    {
                        Chart1.Width = width * wid_hit_incre;
                        Chart1.Height = height * wid_hit_incre;
                    }
                    else
                    {
                        Chart1.Width = width1;
                        Chart1.Height = height1;
                    }
                }
            }

            else if (ddl_view.Text == "Graph")
            {
                FpTransport.Visible = false;
                Fp_Absenties.Visible = false;
                Fp_InOut.Visible = false;
                Chart1.Visible = false;

                con.Close();
                con.Open();
                SqlCommand cmd_get_GPRS_data1 = new SqlCommand("select * from VTSGPRSData where date='" + GPRS_date + "'", con);
                SqlDataAdapter ad_get_GPRS_data1 = new SqlDataAdapter(cmd_get_GPRS_data1);
                DataTable dt_get_GPRS_data1 = new DataTable();
                ad_get_GPRS_data1.Fill(dt_get_GPRS_data1);

                if (dt_get_GPRS_data1.Rows.Count > 0)
                {
                    if (vehid_all != "")
                    {
                        Graph_Chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
                        Graph_Chart.BorderlineColor = System.Drawing.Color.FromArgb(26, 59, 105);
                        Graph_Chart.BorderlineWidth = 3;
                        Graph_Chart.BackColor = Color.RoyalBlue;

                        Graph_Chart.ChartAreas.Add("chtArea");
                        Graph_Chart.ChartAreas[0].AxisX.Title = "Time";
                        Graph_Chart.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Verdana", 11, System.Drawing.FontStyle.Bold);
                        Graph_Chart.ChartAreas[0].AxisY.Title = "Speed";
                        Graph_Chart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Verdana", 11, System.Drawing.FontStyle.Bold);
                        Graph_Chart.ChartAreas[0].BorderDashStyle = ChartDashStyle.Solid;
                        Graph_Chart.ChartAreas[0].BorderWidth = 2;

                        string[] spl_vehid = vehid_all.Split('@');

                        int series = -1;
                        int color_incre = 0;

                        for (int i = 0; spl_vehid.GetUpperBound(0) >= i; i++)
                        {
                            string vehicle_id = spl_vehid[i].ToString();

                            DataView dv_get_GPRSdata = new DataView();
                            dt_get_GPRS_data1.DefaultView.RowFilter = "vehicleid='" + vehicle_id + "'";
                            dv_get_GPRSdata = dt_get_GPRS_data1.DefaultView;

                            if (dv_get_GPRSdata.Count > 0)
                            {
                                series++;

                                DataTable dt_graph_data = new DataTable();
                                DataColumn dc;

                                dc = new DataColumn();
                                dc.ColumnName = "Time";
                                dt_graph_data.Columns.Add(dc);

                                dc = new DataColumn();
                                dc.ColumnName = "Speed";
                                dt_graph_data.Columns.Add(dc);

                                DataRow dr;

                                foreach (DataRowView data_row_view in dv_get_GPRSdata)
                                {
                                    string Sp_hour = string.Empty;
                                    string Sp_Min = string.Empty;
                                    string Sp_Sec = string.Empty;

                                    string speed = data_row_view["Speed"].ToString();
                                    string input = data_row_view["Time"].ToString();

                                    string[] sub_string = Regex.Split(input, "");

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

                                    string final_time = Sp_hour + "." + Sp_Min;

                                    dr = dt_graph_data.NewRow();

                                    dr["Time"] = final_time;
                                    dr["Speed"] = speed;
                                    dt_graph_data.Rows.Add(dr);

                                }

                                Graph_Chart.Legends.Add(vehicle_id);
                                Graph_Chart.Series.Add(vehicle_id);
                                Graph_Chart.Series[series].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                                Graph_Chart.Series[series].Points.DataBindXY(dt_graph_data.DefaultView, "Time", dt_graph_data.DefaultView, "Speed");

                                Graph_Chart.Series[series].IsVisibleInLegend = true;
                                Graph_Chart.Series[series].IsValueShownAsLabel = true;
                                Graph_Chart.Series[series].ToolTip = "Data Point Y Value: #VALY{G}";

                                color_incre = color_incre + 5;

                                string color_code = string.Empty;

                                if (Convert.ToString(color_incre).Length > 2)
                                {
                                    color_code = "000" + Convert.ToString(color_incre);
                                }
                                else if (Convert.ToString(color_incre).Length > 3)
                                {
                                    color_code = "00" + Convert.ToString(color_incre);
                                }
                                else if (Convert.ToString(color_incre).Length > 4)
                                {
                                    color_code = "0" + Convert.ToString(color_incre);
                                }

                                Graph_Chart.Series[series].BorderWidth = 3;
                                Graph_Chart.Series[series].Color = ColorTranslator.FromHtml("#FF" + color_code);

                                Graph_Chart.Legends[0].LegendStyle = LegendStyle.Table;
                                Graph_Chart.Legends[0].TableStyle = LegendTableStyle.Wide;
                                Graph_Chart.Legends[0].Docking = Docking.Bottom;

                                Fp_Absenties.Visible = false;
                                Fp_InOut.Visible = false;
                                FpTransport.Visible = false;
                                Chart1.Visible = false;

                                Graph_Chart.Visible = true;
                            }
                        }

                    }
                }
            }

            else if (ddl_view.SelectedItem.ToString() == "Alter Report")
            {
                FpTransport.Visible = false;
                Fp_Absenties.Visible = false;
                Fp_InOut.Visible = false;
                Chart1.Visible = false;

                FpTransport.Sheets[0].RowCount = 0;

                FpTransport.Sheets[0].ColumnCount = 6;
                FpTransport.Sheets[0].ColumnHeader.Cells[FpTransport.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "Sl.No";
                FpTransport.Sheets[0].ColumnHeader.Cells[FpTransport.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "Roll No";
                FpTransport.Sheets[0].ColumnHeader.Cells[FpTransport.Sheets[0].ColumnHeader.RowCount - 1, 2].Text = "Student Name";
                FpTransport.Sheets[0].ColumnHeader.Cells[FpTransport.Sheets[0].ColumnHeader.RowCount - 1, 3].Text = "Session";
                FpTransport.Sheets[0].ColumnHeader.Cells[FpTransport.Sheets[0].ColumnHeader.RowCount - 1, 4].Text = "Planning Stage";
                FpTransport.Sheets[0].ColumnHeader.Cells[FpTransport.Sheets[0].ColumnHeader.RowCount - 1, 5].Text = "Actual Stage";

                FpTransport.Sheets[0].ColumnHeader.Cells[FpTransport.Sheets[0].ColumnHeader.RowCount - 1, 0].Column.Width = 50;
                FpTransport.Sheets[0].ColumnHeader.Cells[FpTransport.Sheets[0].ColumnHeader.RowCount - 1, 1].Column.Width = 100;
                FpTransport.Sheets[0].ColumnHeader.Cells[FpTransport.Sheets[0].ColumnHeader.RowCount - 1, 2].Column.Width = 200;
                FpTransport.Sheets[0].ColumnHeader.Cells[FpTransport.Sheets[0].ColumnHeader.RowCount - 1, 3].Column.Width = 100;
                FpTransport.Sheets[0].ColumnHeader.Cells[FpTransport.Sheets[0].ColumnHeader.RowCount - 1, 4].Column.Width = 300;
                FpTransport.Sheets[0].ColumnHeader.Cells[FpTransport.Sheets[0].ColumnHeader.RowCount - 1, 5].Column.Width = 300;

                FpTransport.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpTransport.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpTransport.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpTransport.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);


                con.Close();
                con.Open();

                SqlCommand cmd_get_student = new SqlCommand("select r.roll_no,r.stud_name,r.bus_routeid,r.boarding,r.vehid,r.smart_serial_no,s.address,rm.route_id,rm.stage_name from registration r,routemaster rm,stage_master s where cast(rm.stage_name as varchar(100))=cast(s.stage_id as varchar(100)) and r.bus_routeid=rm.route_id and cast(r.boarding as varchar(100))=cast(rm.stage_name  as varchar(100)) and rm.sess='M'and s.address<>''", con);
                SqlDataAdapter ad_get_student = new SqlDataAdapter(cmd_get_student);
                DataTable dt_get_student = new DataTable();
                ad_get_student.Fill(dt_get_student);

                if (dt_get_student.Rows.Count > 0)
                {
                    int Row_cnt = 0;
                    int sl_no = 0;

                    for (int i = 0; i < dt_get_student.Rows.Count; i++)
                    {

                        string roll_no = dt_get_student.Rows[i]["roll_no"].ToString();
                        string stud_name = dt_get_student.Rows[i]["stud_name"].ToString();
                        string smart_serial_no = dt_get_student.Rows[i]["smart_serial_no"].ToString();
                        string planing_boarding = dt_get_student.Rows[i]["address"].ToString();

                        for (int ss = 0; ss < sess_count; ss++)
                        {
                            int first_time = 0;
                            int second_time = 1;
                            string temp_sess_input = string.Empty;
                            int increment = 1;

                            DataView dv_Actual_grid_data = new DataView();

                            if (ddl_session.Text == "Both")
                            {
                                if (ss == first_time)
                                {
                                    dt_get_min.DefaultView.RowFilter = "rfiddata='" + smart_serial_no + "'";
                                    dv_Actual_grid_data = dt_get_min.DefaultView;
                                }
                                else if (ss == second_time)
                                {
                                    dt_get_max.DefaultView.RowFilter = "rfiddata='" + smart_serial_no + "'";
                                    dv_Actual_grid_data = dt_get_max.DefaultView;
                                }
                            }
                            else
                            {

                                if (ddl_sess == "M")
                                {
                                    dt_get_min.DefaultView.RowFilter = "rfiddata='" + smart_serial_no + "'";
                                    dv_Actual_grid_data = dt_get_min.DefaultView;
                                }
                                else
                                {
                                    dt_get_max.DefaultView.RowFilter = "rfiddata='" + smart_serial_no + "'";
                                    dv_Actual_grid_data = dt_get_max.DefaultView;
                                }

                            }

                            if (dv_Actual_grid_data.Count > 0)
                            {
                                string actual_boarding = dv_Actual_grid_data[0]["GoogleLocation"].ToString();

                                if (actual_boarding != planing_boarding)
                                {
                                    if (ss == 0)
                                    {
                                        sl_no++;
                                    }

                                    increment++;
                                    FpTransport.Sheets[0].RowCount = Convert.ToInt32(FpTransport.Sheets[0].RowCount) + 1;
                                    Row_cnt = Convert.ToInt32(FpTransport.Sheets[0].RowCount) - 1;

                                    FpTransport.Sheets[0].Cells[Row_cnt, 0].Text = sl_no.ToString();
                                    FpTransport.Sheets[0].Cells[Row_cnt, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpTransport.Sheets[0].Cells[Row_cnt, 1].Text = roll_no;
                                    FpTransport.Sheets[0].Cells[Row_cnt, 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpTransport.Sheets[0].Cells[Row_cnt, 2].Text = stud_name;

                                    if (Row_cnt % 2 == 0)
                                    {
                                        FpTransport.Sheets[0].Cells[Row_cnt, 0].BackColor = Color.AliceBlue;
                                        FpTransport.Sheets[0].Cells[Row_cnt, 1].BackColor = Color.AliceBlue;
                                        FpTransport.Sheets[0].Cells[Row_cnt, 2].BackColor = Color.AliceBlue;
                                    }
                                    else
                                    {
                                        FpTransport.Sheets[0].Cells[Row_cnt, 1].BackColor = Color.White;
                                    }

                                    if (ddl_session.Text == "Both")
                                    {
                                        if (ss == first_time)
                                        {

                                            temp_sess_input = "M";
                                            FpTransport.Sheets[0].Cells[Row_cnt, 3].Text = "Morning";

                                        }
                                        else if (ss == second_time)
                                        {
                                            temp_sess_input = "A";
                                            FpTransport.Sheets[0].Cells[Row_cnt, 3].Text = "Afternoon";
                                        }
                                    }
                                    else
                                    {

                                        temp_sess_input = ddl_sess;
                                        FpTransport.Sheets[0].Cells[Row_cnt, 3].Text = ddl_session.Text;
                                    }

                                    FpTransport.Sheets[0].Cells[Row_cnt, 4].Text = planing_boarding;
                                    FpTransport.Sheets[0].Cells[Row_cnt, 5].Text = actual_boarding;

                                    if (ss == first_time)
                                    {
                                        FpTransport.Sheets[0].Cells[Row_cnt, 3].BackColor = Color.Lavender;
                                        FpTransport.Sheets[0].Cells[Row_cnt, 4].BackColor = Color.Lavender;
                                        FpTransport.Sheets[0].Cells[Row_cnt, 5].BackColor = Color.Lavender;
                                    }
                                    else if (ss == second_time)
                                    {
                                        FpTransport.Sheets[0].Cells[Row_cnt, 3].BackColor = Color.LavenderBlush;
                                        FpTransport.Sheets[0].Cells[Row_cnt, 4].BackColor = Color.LavenderBlush;
                                        FpTransport.Sheets[0].Cells[Row_cnt, 5].BackColor = Color.LavenderBlush;
                                    }

                                    FpTransport.Visible = true;
                                    FpTransport.Sheets[0].PageSize = FpTransport.Sheets[0].RowCount;
                                }
                            }

                        }
                    }

                    if (FpTransport.Visible == false)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No data found.')", true);
                    }
                }
            }
        }

        if (chkabs.Checked == true)
        {
            transport_absent();
        }

        if (chktimeing.Checked == true)
        {
            timing();
        }
        //}

        //catch (Exception ex)
        //{
        //    throw ex;
        //}
    }
    protected void ddl_session_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_session.Text == "Morning")
        {
            ddl_sess = "M";
        }
        else if (ddl_session.Text == "Afternoon")
        {
            ddl_sess = "A";
        }
        else
        {
            ddl_sess = "M','A";
        }

        Bind_Routes();
    }
    protected void Chk_events_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (chk_display.Checked == true)
            {
                for (int i = 0; i < chkls_display.Items.Count; i++)
                {
                    chkls_display.Items[i].Selected = true;
                    txt_events.Text = "Events(" + chkls_display.Items.Count + ")";
                }
            }
            else
            {
                for (int i = 0; i < chkls_display.Items.Count; i++)
                {
                    chkls_display.Items[i].Selected = false;
                    txt_events.Text = "--Select--";
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void Chkls_events_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int event_count = 0;
            for (int i = 0; i < chkls_display.Items.Count; i++)
            {
                if (chkls_display.Items[i].Selected == true)
                {
                    event_count = event_count + 1;
                    txt_events.Text = "Events(" + event_count.ToString() + ")";
                    if (event_values == "")
                    {
                        event_values = chkls_display.Items[i].Text.ToString();
                    }
                    else
                    {
                        event_values = event_values + "','" + chkls_display.Items[i].Text;
                    }
                }
            }

            if (event_count == 0)
            {
                txt_events.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }


    void send_sms()
    {
        DataSet ds1 = new DataSet();
        //string strsenderquery = "select SMS_User_ID,college_code from Track_Value where college_code = '" + collegecode + "'";
        ds1.Dispose();
        ds1.Reset();

        con.Close();
        con.Open();
        SqlCommand cmd_get_user = new SqlCommand("select SMS_User_ID,college_code from Track_Value where college_code = '" + collegecode + "'", con);
        SqlDataAdapter ad_get_user = new SqlDataAdapter(cmd_get_user);
        ad_get_user.Fill(ds1);

        if (ds1.Tables[0].Rows.Count > 0)
        {
            user_id = Convert.ToString(ds1.Tables[0].Rows[0]["SMS_User_ID"]);

        }

        //modified by srinath 1/8/2014
        //GetUserapi(user_id);
        string getval = d2.GetUserapi(user_id);
        string[] spret = getval.Split('-');
        if (spret.GetUpperBound(0) == 1)
        {

            SenderID = spret[0].ToString();
            Password = spret[0].ToString();
            Session["api"] = user_id;
            Session["senderid"] = SenderID;
        }

        con.Close();
        con.Open();

        SqlCommand cmd_get_mobno = new SqlCommand("select ROW_NUMBER() OVER (ORDER BY  Roll_no) As SrNo,roll_no,reg_no,registration.stud_name,registration.stud_type,applyn.sex,registration.smart_serial_no,applyn.Student_Mobile,applyn.parentF_Mobile,applyn.parentM_Mobile,applyn.stuper_id,sio.start_date,registration.Adm_Date,registration.mode as Mode from seminfo sio,registration inner join applyn on applyn.app_no = registration.app_no where registration.degree_code=sio.degree_code and registration.batch_year=sio.batch_year and registration.current_semester=sio.semester and  RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' and smart_serial_no<>'' order by Len(roll_no),roll_no", con);
        SqlDataAdapter ad_get_mobno = new SqlDataAdapter(cmd_get_mobno);
        DataTable dt_get_mobno = new DataTable();
        ad_get_mobno.Fill(dt_get_mobno);


        con.Close();
        con.Open();
        SqlCommand cmd_stud_plan_count = new SqlCommand("select distinct r.vehid,r.boarding,isnull(count(*),0) as Total_Student, isnull( s.address,'') as address from registration r,routemaster m,stage_master s  where  cast(m.stage_name as varchar(100))=cast(s.stage_id as varchar(100)) and cast(r.boarding as varchar(100))=cast(m.stage_name as varchar(100))and m.sess='m' group by r.vehid ,r.boarding,s.address order by vehid", con);
        SqlDataAdapter ad_stud_plan_count = new SqlDataAdapter(cmd_stud_plan_count);
        DataTable dt_stud_plan_count = new DataTable();
        ad_stud_plan_count.Fill(dt_stud_plan_count);

        string get_date = DateTime.Now.ToString("dd/MM/yyyy");

        string[] split_Date = get_date.Split(new char[] { '/' });
        string set_date = split_Date[1] + '/' + split_Date[0] + '/' + split_Date[2];
        int gps_year = Convert.ToInt32(split_Date[2]) - 2000;
        string GPRS_date = split_Date[0] + split_Date[1] + Convert.ToString(gps_year);

        con.Close();
        con.Open();

        SqlCommand cmd_get_GPRSdata = new SqlCommand("Select * from VTSGPRSData where Flag_Status='0' and rfiddata<>'00000000'", con);
        SqlDataAdapter ad_get_GPRSdata = new SqlDataAdapter(cmd_get_GPRSdata);
        DataTable dt_get_GPRSdata = new DataTable();
        ad_get_GPRSdata.Fill(dt_get_GPRSdata);

        con.Close();
        con.Close();

        SqlCommand cmd_get_not = new SqlCommand("select max(time) as time from vtsgprsdata where rfiddata='00000000' and speed<>'000.0' and date ='" + GPRS_date + "'", con);
        SqlDataAdapter ad_get_not = new SqlDataAdapter(cmd_get_not);
        DataTable dt_get_not = new DataTable();
        ad_get_not.Fill(dt_get_not);

        con.Close();
        con.Close();

        SqlCommand cmd_get_in = new SqlCommand("select max(time) as time from vtsgprsdata where rfiddata<>'00000000' and date ='" + GPRS_date + "'", con);
        SqlDataAdapter ad_get_in = new SqlDataAdapter(cmd_get_in);
        DataTable dt_get_in = new DataTable();
        ad_get_in.Fill(dt_get_in);

        if (dt_get_GPRSdata.Rows.Count > 0)
        {
            con.Close();
            con.Open();

            SqlCommand cmd_get_data = new SqlCommand("Select * from VTSGPRSData where date ='" + GPRS_date + "' and rfiddata<>'00000000'", con);
            SqlDataAdapter ad_get_data = new SqlDataAdapter(cmd_get_data);
            DataTable dt_get_data = new DataTable();
            ad_get_data.Fill(dt_get_data);

            for (int i = 0; i < dt_get_GPRSdata.Rows.Count; i++)
            {
                string smart_slno = dt_get_GPRSdata.Rows[i]["rfiddata"].ToString();
                string date = dt_get_GPRSdata.Rows[i]["date"].ToString();
                string time = dt_get_GPRSdata.Rows[i]["time"].ToString();
                string location = dt_get_GPRSdata.Rows[i]["GoogleLocation"].ToString();
                string veh_id = dt_get_GPRSdata.Rows[i]["VehicleId"].ToString();

                DataView dv_get_sess = new DataView();
                dt_get_data.DefaultView.RowFilter = "rfiddata='" + smart_slno + "'";
                dv_get_sess = dt_get_data.DefaultView;

                string sp_date = string.Empty;
                string sp_month = string.Empty;
                string sp_year = string.Empty;

                int incre_d = 0;
                if (date.Length == 6)
                {
                    incre_d = 3;
                }
                else
                {
                    incre_d = 2;
                }

                string[] spl_date = Regex.Split(date, "");

                for (int ctr = 0; ctr < spl_date.Length; ctr++)
                {
                    if (ctr < incre_d && spl_date[ctr] != "")
                    {
                        sp_date = sp_date + spl_date[ctr];
                    }
                    else if (ctr < incre_d + 2 && spl_date[ctr] != "")
                    {
                        sp_month = sp_month + spl_date[ctr];
                    }
                    else if (spl_date[ctr] != "")
                    {
                        sp_year = sp_year + spl_date[ctr];
                    }
                }

                string final_date = sp_date + "-" + sp_month + "-" + sp_year;

                string Sp_hour = "0"; // string.Empty;
                string Sp_Min = "0"; // string.Empty;
                string Sp_Sec = "0"; // string.Empty;

                int incre_t = 0;
                if (time.Length == 6)
                {
                    incre_t = 3;
                }
                else
                {
                    incre_t = 2;
                }
                string[] sub_string = Regex.Split(time, "");

                for (int ctr = 0; ctr < sub_string.Length; ctr++)
                {
                    if (ctr < incre_t && sub_string[ctr] != "")
                    {
                        Sp_hour = Sp_hour + sub_string[ctr];
                    }
                    else if (ctr < incre_t + 2 && sub_string[ctr] != "")
                    {
                        Sp_Min = Sp_Min + sub_string[ctr];
                    }
                    else if (sub_string[ctr] != "")
                    {
                        Sp_Sec = Sp_Sec + sub_string[ctr];
                    }
                }

                
                string sess = string.Empty;
                if (Convert.ToInt32(Sp_hour) > 12)
                {
                    Sp_hour = Convert.ToString(Convert.ToInt32(Sp_hour) - 12);
                    sess = ".pm.";
                }
                else
                {
                    sess = ".am.";
                }

                string final_time = Sp_hour + ":" + Sp_Min + ":" + Sp_Sec;

                DataView dv_get_student = new DataView();
                dt_get_mobno.DefaultView.RowFilter = "smart_serial_no='" + smart_slno + "'";
                dv_get_student = dt_get_mobno.DefaultView;

                if (dv_get_student.Count > 0)
                {
                    string mobile_no = dv_get_student[0]["parentF_Mobile"].ToString();
                    string stu_name = dv_get_student[0]["stud_name"].ToString();
                    string gender = dv_get_student[0]["sex"].ToString();

                    string stu_gender = string.Empty;
                    if (gender == "0")
                    {
                        stu_gender = " Son Mr.";
                    }
                    else
                    {
                        stu_gender = " Daughter Ms.";
                    }

                    string sms_content = string.Empty;
                    if (dv_get_sess.Count < 2)
                    {
                        sms_content = "Your" + stu_gender + stu_name + " has Boarded The College Bus from " + location + " on " + final_date + " at " + final_time + sess;
                    }
                    else
                    {
                        sms_content = "Your" + stu_gender + stu_name + " has been Dropped from The College Bus at " + location + " on " + final_date + " at " + final_time + sess;
                    }
                    //Modified by srinath8/2/2014
                   // string strpath1 = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobile_no + "&text=" + sms_content + "&priority=ndnd&stype=normal";
                  //  string strpath1 = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + mobile_no + "&message=" + sms_content + "&sender=" + SenderID;
                    // System.Diagnostics.Process.Start(strpath1);

                    con.Close();
                    con.Open();

                    SqlCommand cmd_update = new SqlCommand("update VTSGPRSData set Flag_Status='1' where vehicleid='" + veh_id + "' and date='" + date + "' and time='" + time + "' and rfiddata='" + smart_slno + "' ", con);
                    cmd_update.ExecuteNonQuery();

                    string isstf = "1";
                    mobile = mobile_no;
                    sms_msg = sms_content;
                    //smsreport(strpath1, isstf);
                    int nofosmssend = d2.send_sms(user_id, collegecode, Session["usercode"].ToString(), mobile_no, sms_content, "0");

                }
            }
        }

        if (dt_get_not.Rows.Count > 0 && dt_get_in.Rows.Count > 0)
        {
            string time_max_not = dt_get_not.Rows[0][0].ToString();
            string time_max_in = dt_get_in.Rows[0][0].ToString();

            con.Close();
            con.Open();

            SqlCommand cmd_get_GPRS = new SqlCommand("Select * from VTSGPRSData where date='" + GPRS_date + "' and rfiddata<>'00000000'", con);
            SqlDataAdapter ad_get_GPRS = new SqlDataAdapter(cmd_get_GPRS);
            DataTable dt_get_GPRS = new DataTable();
            ad_get_GPRS.Fill(dt_get_GPRS);

            if(dt_get_GPRS.Rows.Count>0)
            {
            DataTable dt_getcount = new DataTable();
            ad_get_GPRS.Fill(dt_getcount);

            DataView dv_get_GPRS = new DataView();
            dt_get_GPRS.DefaultView.RowFilter = "time='" + time_max_in + "'";
            dv_get_GPRS = dt_get_GPRS.DefaultView;

            string max_location = string.Empty;

            if (dv_get_GPRS.Count > 0)
            {
                max_location = dv_get_GPRS[0]["GoogleLocation"].ToString();
            }

            DataView dv_stu_count = new DataView();
            dt_stud_plan_count.DefaultView.RowFilter = "address='" + max_location + "'";
            dv_stu_count = dt_stud_plan_count.DefaultView;

            string pln_stu_count = "0";
            if (dv_stu_count.Count  > 0)
            {
                pln_stu_count = dv_stu_count[0]["Total_Student"].ToString();
            }
            DataView dv_act_count = new DataView();
            dt_getcount.DefaultView.RowFilter = "GoogleLocation='" + max_location + "' and date='" + GPRS_date + "'";
            dv_act_count = dt_getcount.DefaultView;

            string act_stu_count = dv_act_count.Count.ToString();

            if (Convert.ToInt32(pln_stu_count) > Convert.ToInt32(act_stu_count))
            {
                string all_rfid = string.Empty;

                for (int i = 0; i < dv_act_count.Count; i++)
                {
                    if (all_rfid == "")
                    {
                        all_rfid = dv_act_count[i]["rfiddata"].ToString();
                    }
                    else
                    {
                        all_rfid = all_rfid + "','" + dv_act_count[i]["rfiddata"].ToString();
                    }
                }


                con.Close();
                con.Open();

                SqlCommand cmd_get_absent_stu = new SqlCommand("select * from registration where smart_serial_no not in('" + all_rfid + "') and boarding=(select distinct stage_name from routemaster where address='" + max_location + "')", con);
                SqlDataAdapter ad_get_absent_stu = new SqlDataAdapter(cmd_get_absent_stu);
                DataTable dt_get_absent_stu = new DataTable();
                ad_get_absent_stu.Fill(dt_get_absent_stu);

                if (dt_get_absent_stu.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_get_absent_stu.Rows.Count; i++)
                    {

                        string smart_slno = dt_get_absent_stu.Rows[i]["smart_serial_no"].ToString();

                        DataView dv_get_student = new DataView();
                        dt_get_mobno.DefaultView.RowFilter = "smart_serial_no='" + smart_slno + "'";
                        dv_get_student = dt_get_mobno.DefaultView;

                        if (dv_get_student.Count > 0)
                        {
                            string mobile_no = dv_get_student[0]["parentF_Mobile"].ToString();
                            string stu_name = dv_get_student[0]["stud_name"].ToString();
                            string gender = dv_get_student[0]["sex"].ToString();

                            string stu_gender = string.Empty;
                            if (gender == "0")
                            {
                                stu_gender = " Son Mr.";
                            }
                            else
                            {
                                stu_gender = " Daughter Ms.";
                            }
                            string sms_content = "Your" + stu_gender + stu_name + " is not Boarded The School Bus today";
                            //modified By Srinath 8/2/2014
                            //string strpath1 = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobile_no + "&text=" + sms_content + "&priority=ndnd&stype=normal";
                          //  string strpath1 = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + mobile_no + "&message=" + sms_content + "&sender=" + SenderID;
                            // System.Diagnostics.Process.Start(strpath1);


                            string isstf = "1";
                            mobile = mobile_no;
                            sms_msg = sms_content;
                            //smsreport(strpath1, isstf);
                            int nofosmssend = d2.send_sms(user_id, collegecode, Session["usercode"].ToString(), mobile_no, sms_content, "0");

                        }


                    }
                }

            }
            }

        }
    }
    //Modified by srinath 8/2/2014
    //public void GetUserapi(string user_id)
    //{
    //    try
    //    {
    //        if (user_id == "AAACET")
    //        {
    //            SenderID = "AAACET";
    //            Password = "AAACET";
    //        }
    //        else if (user_id == "SVschl")
    //        {
    //            SenderID = "SVschl";
    //            Password = "SVschl";
    //        }
    //        else if (user_id == "AALIME")
    //        {
    //            SenderID = "AALIME";
    //            Password = "AALIME";
    //        }
    //        else if (user_id == "ACETVM")
    //        {
    //            SenderID = "ACETVM";
    //            Password = "ACETVM";
    //        }
    //        else if (user_id == "AGNICT")
    //        {
    //            SenderID = "AGNICT";
    //            Password = "AGNICT";
    //        }
    //        else if (user_id == "AMSPTC")
    //        {
    //            SenderID = "AMSPTC";
    //            Password = "AMSPTC";
    //        }
    //        else if (user_id == "ANGE")
    //        {
    //            SenderID = "ANGE";
    //            Password = "ANGE";
    //        }
    //        else if (user_id == "ARASUU")
    //        {
    //            SenderID = "ARASUU";
    //            Password = "ARASUU";
    //        }
    //        else if (user_id == "DAVINC")
    //        {
    //            SenderID = "DAVINC";
    //            Password = "DAVINC";
    //        }
    //        else if (user_id == "EASACG")
    //        {
    //            SenderID = "EASACG";
    //            Password = "EASACG";
    //        }
    //        else if (user_id == "ECESMS")
    //        {
    //            SenderID = "ECESMS";
    //            Password = "ECESMS";
    //        }
    //        else if (user_id == "ESECED")
    //        {
    //            SenderID = "ESECED";
    //            Password = "ESECED";
    //        }
    //        else if (user_id == "ESENGG")
    //        {
    //            SenderID = "ESENGG";
    //            Password = "ESENGG";
    //        }
    //        else if (user_id == "ESEPTC")
    //        {
    //            SenderID = "ESEPTC";
    //            Password = "ESEPTC";
    //        }
    //        else if (user_id == "ESMSCH")
    //        {
    //            SenderID = "ESMSCH";
    //            Password = "ESMSCH";
    //        }
    //        else if (user_id == "GKMCET")
    //        {
    //            SenderID = "GKMCET";
    //            Password = "GKMCET";
    //        }
    //        else if (user_id == "IJAYAM")
    //        {
    //            SenderID = "IJAYAM";
    //            Password = "IJAYAM";
    //        }
    //        else if (user_id == "JJAAMC")
    //        {
    //            SenderID = "JJAAMC";
    //            Password = "JJAAMC";
    //        }

    //        else if (user_id == "KINGSE")
    //        {
    //            SenderID = "KINGSE";
    //            Password = "KINGSE";
    //        }
    //        else if (user_id == "KNMHSS")
    //        {
    //            SenderID = "KNMHSS";
    //            Password = "KNMHSS";
    //        }
    //        else if (user_id == "KSRIET")
    //        {
    //            SenderID = "KSRIET";
    //            Password = "KSRIET";
    //        }
    //        else if (user_id == "KTVRKP")
    //        {
    //            SenderID = "KTVRKP";
    //            Password = "KTVRKP";
    //        }
    //        else if (user_id == "MPNMJS")
    //        {
    //            SenderID = "MPNMJS";
    //            Password = "MPNMJS";
    //        }
    //        else if (user_id == "NANDHA")
    //        {
    //            SenderID = "NANDHA";
    //            Password = "NANDHA";
    //        }
    //        else if (user_id == "NECARE")
    //        {
    //            SenderID = "NECARE";
    //            Password = "NECARE";
    //        }
    //        else if (user_id == "NSNCET")
    //        {
    //            SenderID = "NSNCET";
    //            Password = "NSNCET";
    //        }
    //        else if (user_id == "PETENG")
    //        {
    //            SenderID = "PETENG";
    //            Password = "PETENG";
    //        }
    //        else if (user_id == "PMCTEC")
    //        {
    //            SenderID = "PMCTEC";
    //            Password = "PMCTEC";
    //        }
    //        else if (user_id == "PPGITS")
    //        {
    //            SenderID = "PPGITS";
    //            Password = "PPGITS";
    //        }
    //        else if (user_id == "PROFCL")
    //        {
    //            SenderID = "PROFCL";
    //            Password = "PROFCL";
    //        }
    //        else if (user_id == "PSVCET")
    //        {
    //            SenderID = "PSVCET";
    //            Password = "PSVCET";
    //        }
    //        else if (user_id == "SASTH")
    //        {
    //            SenderID = "SASTH";
    //            Password = "SASTH";
    //        }
    //        else if (user_id == "SCTSBS")
    //        {
    //            SenderID = "SCTSBS";
    //            Password = "SCTSBS";
    //        }
    //        else if (user_id == "SCTSCE")
    //        {
    //            SenderID = "SCTSCE";
    //            Password = "SCTSCE";
    //        }
    //        else if (user_id == "SCTSEC")
    //        {
    //            SenderID = "SCTSEC";
    //            Password = "SCTSEC";
    //        }
    //        else if (user_id == "SKCETC")
    //        {
    //            SenderID = "SKCETC";
    //            Password = "SKCETC";
    //        }
    //        else if (user_id == "SRECCG")
    //        {
    //            SenderID = "SRECCG";
    //            Password = "SRECCG";
    //        }
    //        else if (user_id == "SLAECT")
    //        {
    //            SenderID = "SLAECT";
    //            Password = "SLAECT";
    //        }
    //        else if (user_id == "SSCENG")
    //        {
    //            SenderID = "SSCENG";
    //            Password = "SSCENG";
    //        }
    //        else if (user_id == "SSMCEE")
    //        {
    //            SenderID = "SSMCEE";
    //            Password = "SSMCEE";
    //        }
    //        else if (user_id == "SVICET")
    //        {
    //            SenderID = "SVICET";
    //            Password = "SVICET";
    //        }
    //        else if (user_id == "SVCTCG")
    //        {
    //            SenderID = "SVCTCG";
    //            Password = "SVCTCG";
    //        }
    //        else if (user_id == "SVSCBE")
    //        {
    //            SenderID = "SVSCBE";
    //            Password = "SVSCBE";
    //        }
    //        else if (user_id == "TECENG")
    //        {
    //            SenderID = "TECENG";
    //            Password = "TECENG";
    //        }
    //        else if (user_id == "TJENGG")
    //        {
    //            SenderID = "TJENGG";
    //            Password = "TJENGG";
    //        }
    //        else if (user_id == "TSMJCT")
    //        {
    //            SenderID = "TSMJCT";
    //            Password = "TSMJCT";
    //        }
    //        else if (user_id == "VCWSMS")
    //        {
    //            SenderID = "VCWSMS";
    //            Password = "VCWSMS";
    //        }
    //        else if (user_id == "VRSCET")
    //        {
    //            SenderID = "VRSCET";
    //            Password = "VRSCET";
    //        }
    //        Session["api"] = user_id;
    //        Session["senderid"] = SenderID;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //public void GetUserapi(string user_id)
    //{
    //    try
    //    {
    //        if (user_id == "DEANSEC")
    //        {
    //            SenderID = "DEANSE";
    //            Password = "DEANSEC";
    //        }
    //        else if (user_id == "PROFCL")
    //        {
    //            SenderID = "PROFCL";
    //            Password = "PROFCL";
    //        }
    //        else if (user_id == "SASTHA")
    //        {
    //            SenderID = "SASTHA";
    //            Password = "SASTHA";
    //        }
    //        else if (user_id == "SSMCE")
    //        {
    //            SenderID = "SSMCE";
    //            Password = "SSMCE";
    //        }
    //        else if (user_id == "NECARE")
    //        {
    //            SenderID = "NECARE";
    //            Password = "NECARE";
    //        }
    //        else if (user_id == "SVCTCG")
    //        {
    //            SenderID = "SVCTCG";
    //            Password = "SVCTCG";
    //        }
    //        else if (user_id == "AGNICT")
    //        {
    //            SenderID = "AGNICT";
    //            Password = "AGNICT";
    //        }
    //        else if (user_id == "NANDHA")
    //        {
    //            SenderID = "NANDHA";
    //            Password = "NANDHA";
    //        }
    //        else if (user_id == "DHIRA")
    //        {
    //            SenderID = "DHIRAJ";
    //            Password = "DHIRA";
    //        }
    //        else if (user_id == "ANGEL123")
    //        {
    //            SenderID = "ANGELS";
    //            Password = "ANGEL123";
    //        }
    //        else if (user_id == "BALAJI12")
    //        {
    //            SenderID = "BALAJI";
    //            Password = "BALAJI12";
    //        }
    //        else if (user_id == "AKSHYA123")
    //        {
    //            SenderID = "AKSHYA";
    //            Password = "AKSHYA";
    //        }
    //        else if (user_id == "PPGITS")
    //        {
    //            SenderID = "PPGITS";
    //            Password = "PPGITS";
    //        }
    //        else if (user_id == "PETENG")
    //        {
    //            SenderID = "PETENG";
    //            Password = "PETENG";
    //        }
    //        else if (user_id == "JJCET")
    //        {
    //            SenderID = "JJCET";
    //            Password = "JJCET";
    //        }
    //        else if (user_id == "PSVCET")
    //        {
    //            SenderID = "PSVCET";
    //            Password = "PSVCET";
    //        }
    //        else if (user_id == "AMSECE")
    //        {
    //            SenderID = "AMSECE";
    //            Password = "AMSECE";
    //        }

    //        else if (user_id == "GKMCET")
    //        {
    //            SenderID = "GKMCET";
    //            Password = "GKMCET";
    //        }
    //        else if (user_id == "SLAECT")
    //        {
    //            SenderID = "SLAECT";
    //            Password = "SLAECT";
    //        }
    //        else if (user_id == "DCTSCE")
    //        {
    //            SenderID = "DCTSCE";
    //            Password = "DCTSCE";
    //        }
    //        else if (user_id == "DCTSCE")
    //        {
    //            SenderID = "DCTSCE";
    //            Password = "DCTSCE";
    //        }
    //        else if (user_id == "DCTSEC")
    //        {
    //            SenderID = "DCTSEC";
    //            Password = "DCTSEC";
    //        }
    //        else if (user_id == "DCTSBS")
    //        {
    //            SenderID = "DCTSBS";
    //            Password = "DCTSBS";
    //        }
    //        else if (user_id == "SCTSCE")
    //        {
    //            SenderID = "SCTSCE";
    //            Password = "SCTSCE";
    //        }

    //        else if (user_id == "SCTSEC")
    //        {
    //            SenderID = "SCTSEC";
    //            Password = "SCTSEC";
    //        }
    //        else if (user_id == "SCTSBS")
    //        {
    //            SenderID = "SCTSBS";
    //            Password = "SCTSBS";
    //        }

    //        else if (user_id == "ESECED")
    //        {
    //            SenderID = "ESECED";
    //            Password = "ESECED";
    //        }

    //        else if (user_id == "IJAYAM")
    //        {
    //            SenderID = "IJAYAM";
    //            Password = "IJAYAM";
    //        }
    //        else if (user_id == "MPNMJS")
    //        {
    //            SenderID = "MPNMJS";
    //            Password = "MPNMJS";
    //        }

    //        else if (user_id == "EASACG")
    //        {
    //            SenderID = "EASACG";
    //            Password = "EASACG";
    //        }
    //        else if (user_id == "KTVRKP")
    //        {
    //            SenderID = "KTVRKP";
    //            Password = "KTVRKP";
    //        }
    //        else if (user_id == "SVSCBE")
    //        {
    //            SenderID = "SVSCBE";
    //            Password = "SVSCBE";
    //        }
    //        else if (user_id == "AIHTCH")
    //        {
    //            SenderID = "AIHTCH";
    //            Password = "AIHTCH";
    //        }
    //        else if (user_id == "NSNCET")
    //        {
    //            SenderID = "NSNCET";
    //            Password = "NSNCET";
    //        }
    //        else if (user_id == "SVICET")
    //        {
    //            SenderID = "SVICET";
    //            Password = "SVICET";
    //        }
    //        else if (user_id == "SSCENG")
    //        {
    //            SenderID = "SSCENG";
    //            Password = "SSCENG";
    //        }
    //        else if (user_id == "ECESMS")
    //        {
    //            SenderID = "ECESMS";
    //            Password = "ECESMS";
    //        }
    //        else if (user_id == "NGPTEC")
    //        {
    //            SenderID = "NGPTEC";
    //            Password = "NGPTEC";
    //        }
    //        else if (user_id == "NGPTEC")
    //        {
    //            SenderID = "NGPTEC";
    //            Password = "NGPTEC";
    //        }

    //        else if (user_id == "KSRIET")
    //        {
    //            SenderID = "KSRIET";
    //            Password = "KSRIET";
    //        }

    //        else if (user_id == "VCWSMS")
    //        {
    //            SenderID = "VCWSMS";
    //            Password = "VCWSMS";
    //        }

    //        else if (user_id == "PMCTEC")
    //        {
    //            SenderID = "PMCTEC";
    //            Password = "PMCTEC";
    //        }

    //        Session["api"] = user_id;
    //        Session["senderid"] = SenderID;
    //    }


    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    public void smsreport(string uril, string isstaff)
    {

        WebRequest request = WebRequest.Create(uril);
        WebResponse response = request.GetResponse();
        Stream data = response.GetResponseStream();
        StreamReader sr = new StreamReader(data);
        string strvel = sr.ReadToEnd();

        string groupmsgid = "";
        groupmsgid = strvel;
        string date = DateTime.Now.ToString("MM/dd/yyyy");
        int sms = 0;
        string smsreportinsert = "";

        smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date,sender_id)values( '" + mobile + "','" + groupmsgid + "','" + sms_msg + "','" + collegecode + "','" + isstaff + "','" + date + "' ,'" + Session["UserCode"].ToString() + "')"; // Added by jairam 21-11-2014
        sms = d2.insert_method(smsreportinsert, hat, "Text");

    }

    protected void chkabs_CheckedChanged(object sender, EventArgs e)
    {

        if (chkabs.Checked == true)
        {
            pabsenties.Visible = true;
            chktimeing.Checked = false;

            //txt_date.Enabled = false;
            //txt_events.Enabled = false;
            //ddl_view.Enabled = false;

            Image9.Visible = false;
            Image15.Visible = false;
            Image16.Visible = false;
            Image20.Visible = false;

            Ptime.Visible = false;
            Rbtnlstinout.Visible = false;


        }
            
        else if (chktimeing.Checked == true)
        {
            //txt_date.Enabled = false;
            //txt_events.Enabled = false;
            //ddl_view.Enabled = false;

            Image9.Visible = true;
            Image15.Visible = true;
            Image16.Visible = true;
            Image20.Visible = true;

            Ptime.Visible = true;
            Rbtnlstinout.Visible = true;
        }
        else
        {
            txt_date.Enabled = true;
            txt_events.Enabled = true;
            ddl_view.Enabled = true;
        }

    }
    protected void chktimeing_CheckedChanged(object sender, EventArgs e)
    {

        if (chktimeing.Checked == true)
        {
            pabsenties.Visible = true;
            chkabs.Checked = false;

            //txt_date.Enabled = false;
            //txt_events.Enabled = false;
            //ddl_view.Enabled = false;

            Image9.Visible = true;
            Image15.Visible = true;
            Image16.Visible = true;
            Image20.Visible = true;

            Ptime.Visible = true;
            Rbtnlstinout.Visible = true;

        }
        else if (chkabs.Checked == true)
        {
            //txt_date.Enabled = false;
            //txt_events.Enabled = false;
            //ddl_view.Enabled = false;

            Image9.Visible = false;
            Image15.Visible = false;
            Image16.Visible = false;
            Image20.Visible = false;

            Ptime.Visible = false;
            Rbtnlstinout.Visible = false;
        }
        else
        {
            txt_date.Enabled = true;
            txt_events.Enabled = true;
            ddl_view.Enabled = true;
        }

    }
    public void transport_absent()
    {
        try
        {
            FpTransport.Visible = false;
            Fp_Absenties.Visible = false;
            Fp_InOut.Visible = false;
            Chart1.Visible = false;

            Fp_Absenties.Sheets[0].ColumnCount = 6;
            Fp_Absenties.Sheets[0].ColumnHeader.Height = 30;
            Fp_Absenties.Width = 920;
            Fp_Absenties.Sheets[0].ColumnHeader.Cells[Fp_Absenties.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "Sl.No";
            Fp_Absenties.Sheets[0].ColumnHeader.Cells[Fp_Absenties.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "Vehicle No";
            Fp_Absenties.Sheets[0].ColumnHeader.Cells[Fp_Absenties.Sheets[0].ColumnHeader.RowCount - 1, 2].Text = "Date";
            Fp_Absenties.Sheets[0].ColumnHeader.Cells[Fp_Absenties.Sheets[0].ColumnHeader.RowCount - 1, 3].Text = "Session";
            Fp_Absenties.Sheets[0].ColumnHeader.Cells[Fp_Absenties.Sheets[0].ColumnHeader.RowCount - 1, 4].Text = "Student Name";
            Fp_Absenties.Sheets[0].ColumnHeader.Cells[Fp_Absenties.Sheets[0].ColumnHeader.RowCount - 1, 5].Text = "Stage";

            Fp_Absenties.Sheets[0].Columns[0].Width = 70;
            Fp_Absenties.Sheets[0].Columns[1].Width = 100;
            Fp_Absenties.Sheets[0].Columns[2].Width = 100;
            Fp_Absenties.Sheets[0].Columns[3].Width = 100;
            Fp_Absenties.Sheets[0].Columns[4].Width = 250;
            Fp_Absenties.Sheets[0].Columns[5].Width = 350;

            Fp_Absenties.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            Fp_Absenties.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            Fp_Absenties.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            Fp_Absenties.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            Fp_Absenties.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            Fp_Absenties.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            Fp_Absenties.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            Fp_Absenties.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            Fp_Absenties.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
            Fp_Absenties.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;


            //Fp_Absenties.Sheets[0].Columns[0].Width = 50;

            Fp_Absenties.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Fp_Absenties.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Fp_Absenties.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Fp_Absenties.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Fp_Absenties.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);


            string sel_veh = string.Empty;
            string sel_route = string.Empty;
            string sel_stage = string.Empty;
            string complete_date = string.Empty;

            string[] fromdate_split = txtfrom.Text.Split(new char[] { '/' });
            string fromdateincre = fromdate_split[1] + "/" + fromdate_split[0] + "/" + fromdate_split[2];
            DateTime fromdtime = Convert.ToDateTime(fromdateincre);
            //string splited_fromdate = fromdate_split[0] + fromdate_split[1] + fromdate_split[2];

            string[] todate_split = txtto.Text.Split(new char[] { '/' });
            string todateincre = todate_split[1] + "/" + todate_split[0] + "/" + todate_split[2];
            DateTime todtime = Convert.ToDateTime(todateincre);

            int morning_time = 120000;
            int Afternoon = 115959;
            SqlCommand cmd_gprsdata = new SqlCommand("select * from VTSGPRSdata order by vehicleid,Googlelocation,time", con);
            SqlDataAdapter da_gprsdata = new SqlDataAdapter(cmd_gprsdata);
            DataTable dt_gprsdata = new DataTable();
            da_gprsdata.Fill(dt_gprsdata);

            SqlCommand cmd_reg = new SqlCommand("select * from registration order by vehid,boarding", con);
            SqlDataAdapter da_reg = new SqlDataAdapter(cmd_reg);
            DataTable dt_reg = new DataTable();
            da_reg.Fill(dt_reg);

            Fp_Absenties.Sheets[0].RowCount = 0;
            int slno = 0;

            for (DateTime cnt_datetime = fromdtime; cnt_datetime <= todtime; cnt_datetime = cnt_datetime.AddDays(1))
            {
                string incre_date = cnt_datetime.ToString("MM/dd/yyyy");
                string[] split_date = incre_date.Split('/');
                string splited_date = split_date[1] + split_date[0] + split_date[2];
                string spread_date = split_date[1] + "/" + split_date[0] + "/" + split_date[2];
                complete_date = "'" + splited_date + "'";
                //Morning Session Absenties
                if (RadioButtonList1.Items[0].Selected == true)
                {
                    for (int cnt_vehid = 0; cnt_vehid < chkls_vech.Items.Count; cnt_vehid++)
                    {
                        if (chkls_vech.Items[cnt_vehid].Selected == true)
                        {
                            sel_veh = "'" + chkls_vech.Items[cnt_vehid].Text.ToString() + "'";

                            for (int cnt_stage = 0; cnt_stage < Chkls_stage.Items.Count; cnt_stage++)
                            {
                                if (Chkls_stage.Items[cnt_stage].Selected == true)
                                {
                                    sel_stage = "'" + Chkls_stage.Items[cnt_stage].Value.ToString() + "'";  //"'" + Chkls_stage.Items[cnt_stage].Text.ToString() + "'";
                                    DataView dv_reg = new DataView();
                                    DataView dv_gprs = new DataView();
                                    if (dt_gprsdata.Rows.Count > 0)
                                    {
                                        dt_gprsdata.DefaultView.RowFilter = "VehicleId in (" + sel_veh + ")  and time<'" + morning_time + "' and date in (" + complete_date + ") and Googlelocation in(" + sel_stage + ")";
                                        dv_gprs = dt_gprsdata.DefaultView;

                                        string pre_stud_rno = "";
                                        string pre_stud_vehid = "";
                                        string pre_stud_location = "";

                                        for (int cnt_dtgprs = 0; cnt_dtgprs < dv_gprs.Count; cnt_dtgprs++)
                                        {
                                            if (pre_stud_rno == "")
                                            {
                                                pre_stud_rno = "'" + dv_gprs[cnt_dtgprs]["RFIDData"].ToString() + "'";
                                                pre_stud_vehid = "'" + dv_gprs[cnt_dtgprs]["vehicleid"].ToString() + "'";
                                                pre_stud_location = "'" + dv_gprs[cnt_dtgprs]["Googlelocation"].ToString() + "'";

                                            }
                                            else
                                            {
                                                pre_stud_rno = pre_stud_rno + "," + "'" + dv_gprs[cnt_dtgprs]["RFIDData"].ToString() + "'";
                                                pre_stud_vehid = pre_stud_vehid + "," + "'" + dv_gprs[cnt_dtgprs]["vehicleid"].ToString() + "'";
                                                pre_stud_location = pre_stud_location + "," + "'" + dv_gprs[cnt_dtgprs]["Googlelocation"].ToString() + "'";

                                            }
                                        }

                                        if (dt_reg.Rows.Count > 0)
                                        {
                                            if (dv_gprs.Count > 0)
                                            {
                                                {
                                                    dt_reg.DefaultView.RowFilter = "roll_no not in (" + pre_stud_rno + ") and vehid in (" + pre_stud_vehid + ") and Boarding in(" + pre_stud_location + ")";
                                                    dv_reg = dt_reg.DefaultView;
                                                    //Fp_Absenties.Sheets[0].RowCount = 0;
                                                    //int slno = 0;
                                                    for (int cnt_reg = 0; cnt_reg < dv_reg.Count; cnt_reg++)
                                                    {
                                                        slno++;
                                                        string vehicle_id = dv_reg[cnt_reg]["vehid"].ToString();
                                                        string student_name = dv_reg[cnt_reg]["stud_name"].ToString();
                                                        string stage = dv_reg[cnt_reg]["boarding"].ToString();
                                                        Fp_Absenties.Sheets[0].RowCount++;
                                                        Fp_Absenties.Sheets[0].Cells[Fp_Absenties.Sheets[0].RowCount - 1, 0].Text = slno.ToString();
                                                        Fp_Absenties.Sheets[0].Cells[Fp_Absenties.Sheets[0].RowCount - 1, 1].Text = vehicle_id.ToString();
                                                        Fp_Absenties.Sheets[0].Cells[Fp_Absenties.Sheets[0].RowCount - 1, 2].Text = spread_date.ToString();
                                                        Fp_Absenties.Sheets[0].Cells[Fp_Absenties.Sheets[0].RowCount - 1, 3].Text = "Morning";
                                                        Fp_Absenties.Sheets[0].Cells[Fp_Absenties.Sheets[0].RowCount - 1, 4].Text = student_name.ToString();
                                                        Fp_Absenties.Sheets[0].Cells[Fp_Absenties.Sheets[0].RowCount - 1, 5].Text = stage.ToString();
                                                        Fp_Absenties.Visible = true;
                                                        FpTransport.Visible = false;
                                                    }
                                                    Fp_Absenties.Sheets[0].PageSize = Fp_Absenties.Sheets[0].RowCount;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //Afternoon Session Absenties

                else if (RadioButtonList1.Items[1].Selected == true)
                {
                    for (int cnt_vehid = 0; cnt_vehid < chkls_vech.Items.Count; cnt_vehid++)
                    {
                        if (chkls_vech.Items[cnt_vehid].Selected == true)
                        {

                            sel_veh = "'" + chkls_vech.Items[cnt_vehid].Text.ToString() + "'";

                            for (int cnt_stage = 0; cnt_stage < Chkls_stage.Items.Count; cnt_stage++)
                            {
                                if (Chkls_stage.Items[cnt_stage].Selected == true)
                                {
                                    sel_stage = "'" + Chkls_stage.Items[cnt_stage].Value.ToString() + "'";  //"'" + Chkls_stage.Items[cnt_stage].Text.ToString() + "'";
                                    DataView dv_reg = new DataView();
                                    DataView dv_gprs = new DataView();
                                    if (dt_gprsdata.Rows.Count > 0)
                                    {
                                        dt_gprsdata.DefaultView.RowFilter = "VehicleId in (" + sel_veh + ")  and out_time>'" + Afternoon + "' and date in (" + complete_date + ") and Googlelocation in(" + sel_stage + ")";
                                        dv_gprs = dt_gprsdata.DefaultView;

                                        string pre_stud_rno = "";
                                        string pre_stud_vehid = "";
                                        string pre_stud_stage = "";

                                        for (int cnt_dtgprs = 0; cnt_dtgprs < dv_gprs.Count; cnt_dtgprs++)
                                        {
                                            if (pre_stud_rno == "")
                                            {
                                                pre_stud_rno = "'" + dv_gprs[cnt_dtgprs]["RFIDData"].ToString() + "'";
                                                pre_stud_vehid = "'" + dv_gprs[cnt_dtgprs]["vehicleid"].ToString() + "'";
                                                pre_stud_stage = "'" + dv_gprs[cnt_dtgprs]["Googlelocation"].ToString() + "'";

                                            }
                                            else
                                            {
                                                pre_stud_rno = pre_stud_rno + "," + "'" + dv_gprs[cnt_dtgprs]["RFIDData"].ToString() + "'";
                                                pre_stud_vehid = pre_stud_vehid + "," + "'" + dv_gprs[cnt_dtgprs]["vehicleid"].ToString() + "'";
                                                pre_stud_stage = pre_stud_stage + "," + "'" + dv_gprs[cnt_dtgprs]["Googlelocation"].ToString() + "'";

                                            }
                                        }

                                        if (dt_reg.Rows.Count > 0)
                                        {
                                            if (dv_gprs.Count > 0)
                                            {
                                                dt_reg.DefaultView.RowFilter = "roll_no not in (" + pre_stud_rno + ") and vehid in (" + pre_stud_vehid + ") and Boarding in(" + pre_stud_stage + ")";
                                                dv_reg = dt_reg.DefaultView;
                                                //Fp_Absenties.Sheets[0].RowCount = 0;
                                                //int slno = 0;
                                                for (int cnt_reg = 0; cnt_reg < dv_reg.Count; cnt_reg++)
                                                {
                                                    slno++;
                                                    string vehicle_id = dv_reg[cnt_reg]["vehid"].ToString();
                                                    string student_name = dv_reg[cnt_reg]["stud_name"].ToString();
                                                    string stage = dv_reg[cnt_reg]["Boarding"].ToString();
                                                    Fp_Absenties.Sheets[0].RowCount++;
                                                    Fp_Absenties.Sheets[0].Cells[Fp_Absenties.Sheets[0].RowCount - 1, 0].Text = slno.ToString();
                                                    Fp_Absenties.Sheets[0].Cells[Fp_Absenties.Sheets[0].RowCount - 1, 1].Text = vehicle_id.ToString();
                                                    Fp_Absenties.Sheets[0].Cells[Fp_Absenties.Sheets[0].RowCount - 1, 2].Text = spread_date.ToString();
                                                    Fp_Absenties.Sheets[0].Cells[Fp_Absenties.Sheets[0].RowCount - 1, 3].Text = "AfterNoon";
                                                    Fp_Absenties.Sheets[0].Cells[Fp_Absenties.Sheets[0].RowCount - 1, 4].Text = student_name.ToString();
                                                    Fp_Absenties.Sheets[0].Cells[Fp_Absenties.Sheets[0].RowCount - 1, 5].Text = stage.ToString();
                                                    Fp_Absenties.Visible = true;
                                                    FpTransport.Visible = false;
                                                }
                                            }
                                            Fp_Absenties.Sheets[0].PageSize = Fp_Absenties.Sheets[0].RowCount;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //Morning and AfterNoon Session

                else if (RadioButtonList1.Items[2].Selected == true)
                {
                    for (int cnt_vehid = 0; cnt_vehid < chkls_vech.Items.Count; cnt_vehid++)
                    {
                        if (chkls_vech.Items[cnt_vehid].Selected == true)
                        {

                            sel_veh = "'" + chkls_vech.Items[cnt_vehid].Text.ToString() + "'";

                            for (int cnt_stage = 0; cnt_stage < Chkls_stage.Items.Count; cnt_stage++)
                            {
                                if (Chkls_stage.Items[cnt_stage].Selected == true)
                                {
                                    sel_stage = "'" + Chkls_stage.Items[cnt_stage].Value.ToString() + "'";  //"'" + Chkls_stage.Items[cnt_stage].Text.ToString() + "'";
                                    DataView dv_reg = new DataView();
                                    DataView dv_gprs = new DataView();
                                    if (dt_gprsdata.Rows.Count > 0)
                                    {
                                        dt_gprsdata.DefaultView.RowFilter = "VehicleId in (" + sel_veh + ") and date in (" + complete_date + ") and Googlelocation in(" + sel_stage + ")";
                                        dv_gprs = dt_gprsdata.DefaultView;

                                        if (dt_reg.Rows.Count > 0)
                                        {
                                            if (dv_gprs.Count > 0)
                                            {
                                                for (int i = 0; i < 2; i++)
                                                {
                                                    string pre_stud_rno = "";
                                                    string pre_stud_vehid = "";
                                                    string pre_stud_stage = "";
                                                    string sess = "";
                                                    if (i == 0)
                                                    {
                                                        for (int cnt_dtgprs = 0; cnt_dtgprs < dv_gprs.Count; cnt_dtgprs++)
                                                        {
                                                            int chk_session = Convert.ToInt32(dv_gprs[cnt_dtgprs]["time"].ToString());
                                                            if (chk_session < 120000)
                                                            {
                                                                if (pre_stud_rno == "")
                                                                {
                                                                    pre_stud_rno = "'" + dv_gprs[cnt_dtgprs]["RFIDData"].ToString() + "'";
                                                                    pre_stud_vehid = "'" + dv_gprs[cnt_dtgprs]["vehicleid"].ToString() + "'";
                                                                    pre_stud_stage = "'" + dv_gprs[cnt_dtgprs]["Googlelocation"].ToString() + "'";
                                                                    sess = "Morning";
                                                                }
                                                                else
                                                                {
                                                                    pre_stud_rno = pre_stud_rno + "," + "'" + dv_gprs[cnt_dtgprs]["RFIDData"].ToString() + "'";
                                                                    pre_stud_vehid = pre_stud_vehid + "," + "'" + dv_gprs[cnt_dtgprs]["vehicleid"].ToString() + "'";
                                                                    pre_stud_stage = pre_stud_stage + "," + "'" + dv_gprs[cnt_dtgprs]["Googlelocation"].ToString() + "'";

                                                                }
                                                            }
                                                        }
                                                    }
                                                    else if (i == 1)
                                                    {
                                                        for (int cnt_dtgprs = 0; cnt_dtgprs < dv_gprs.Count; cnt_dtgprs++)
                                                        {
                                                            int chk_session = Convert.ToInt32(dv_gprs[cnt_dtgprs]["out_time"].ToString());
                                                            if (chk_session > 115959)
                                                            {
                                                                if (pre_stud_rno == "")
                                                                {
                                                                    pre_stud_rno = "'" + dv_gprs[cnt_dtgprs]["RFIDData"].ToString() + "'";
                                                                    pre_stud_vehid = "'" + dv_gprs[cnt_dtgprs]["vehicleid"].ToString() + "'";
                                                                    pre_stud_stage = "'" + dv_gprs[cnt_dtgprs]["Googlelocation"].ToString() + "'";
                                                                    sess = "AfterNoon";
                                                                }
                                                                else
                                                                {
                                                                    pre_stud_rno = pre_stud_rno + "," + "'" + dv_gprs[cnt_dtgprs]["RFIDData"].ToString() + "'";
                                                                    pre_stud_vehid = pre_stud_vehid + "," + "'" + dv_gprs[cnt_dtgprs]["vehicleid"].ToString() + "'";
                                                                    pre_stud_stage = pre_stud_stage + "," + "'" + dv_gprs[cnt_dtgprs]["Googlelocation"].ToString() + "'";

                                                                }
                                                            }
                                                        }
                                                    }

                                                    if (pre_stud_rno != "" && pre_stud_vehid != "")
                                                    {
                                                        dt_reg.DefaultView.RowFilter = "roll_no not in (" + pre_stud_rno + ") and vehid in (" + pre_stud_vehid + ") and Boarding in (" + pre_stud_stage + ")";
                                                        dv_reg = dt_reg.DefaultView;

                                                        //int slno = 0;
                                                        for (int cnt_reg = 0; cnt_reg < dv_reg.Count; cnt_reg++)
                                                        {
                                                            slno++;
                                                            string vehicle_id = dv_reg[cnt_reg]["vehid"].ToString();
                                                            string student_name = dv_reg[cnt_reg]["stud_name"].ToString();
                                                            string stage = dv_reg[cnt_reg]["boarding"].ToString();

                                                            Fp_Absenties.Sheets[0].RowCount++;
                                                            Fp_Absenties.Sheets[0].Cells[Fp_Absenties.Sheets[0].RowCount - 1, 0].Text = slno.ToString();
                                                            Fp_Absenties.Sheets[0].Cells[Fp_Absenties.Sheets[0].RowCount - 1, 1].Text = vehicle_id.ToString();
                                                            Fp_Absenties.Sheets[0].Cells[Fp_Absenties.Sheets[0].RowCount - 1, 2].Text = spread_date.ToString();
                                                            Fp_Absenties.Sheets[0].Cells[Fp_Absenties.Sheets[0].RowCount - 1, 3].Text = sess.ToString();
                                                            Fp_Absenties.Sheets[0].Cells[Fp_Absenties.Sheets[0].RowCount - 1, 4].Text = student_name.ToString();
                                                            Fp_Absenties.Sheets[0].Cells[Fp_Absenties.Sheets[0].RowCount - 1, 5].Text = stage.ToString();
                                                            Fp_Absenties.Visible = true;
                                                            FpTransport.Visible = false;
                                                        }
                                                    }
                                                    Fp_Absenties.Sheets[0].PageSize = Fp_Absenties.Sheets[0].RowCount;
                                                    Fp_Absenties.Visible = true;
                                                }

                                            }
                                        }
                                    }
                                }
                            }
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

    public void timing()
    {
        try
        {
            Fp_InOut.Sheets[0].ColumnHeader.Height = 30;

            Fp_InOut.Sheets[0].ColumnCount = 7;
            Fp_InOut.Sheets[0].ColumnHeader.Cells[Fp_InOut.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "Sl.No";
            Fp_InOut.Sheets[0].ColumnHeader.Cells[Fp_InOut.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "Date";
            Fp_InOut.Sheets[0].ColumnHeader.Cells[Fp_InOut.Sheets[0].ColumnHeader.RowCount - 1, 2].Text = "Vehicle ID";
            Fp_InOut.Sheets[0].ColumnHeader.Cells[Fp_InOut.Sheets[0].ColumnHeader.RowCount - 1, 3].Text = "Department";
            Fp_InOut.Sheets[0].ColumnHeader.Cells[Fp_InOut.Sheets[0].ColumnHeader.RowCount - 1, 4].Text = "Roll No";
            Fp_InOut.Sheets[0].ColumnHeader.Cells[Fp_InOut.Sheets[0].ColumnHeader.RowCount - 1, 5].Text = "Student Name";
            Fp_InOut.Sheets[0].ColumnHeader.Cells[Fp_InOut.Sheets[0].ColumnHeader.RowCount - 1, 6].Text = "Stage";

            Fp_InOut.Sheets[0].Columns[0].Width = 70;
            Fp_InOut.Sheets[0].Columns[1].Width = 70;
            Fp_InOut.Sheets[0].Columns[2].Width = 100;
            Fp_InOut.Sheets[0].Columns[3].Width = 350;
            Fp_InOut.Sheets[0].Columns[4].Width = 100;
            Fp_InOut.Sheets[0].Columns[5].Width = 350;
            Fp_InOut.Sheets[0].Columns[6].Width = 400;

            Fp_InOut.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            Fp_InOut.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            Fp_InOut.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            Fp_InOut.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            Fp_InOut.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            Fp_InOut.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            Fp_InOut.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            Fp_InOut.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            Fp_InOut.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
            Fp_InOut.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            Fp_InOut.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;
            Fp_InOut.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;

            Fp_InOut.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Fp_InOut.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Fp_InOut.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Fp_InOut.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Fp_InOut.Height = 1000;

            string sel_veh = string.Empty;
            string sel_route = string.Empty;
            string sel_stage = string.Empty;
            string complete_date = string.Empty;

            string[] fromdate_split = txtfrom.Text.Split(new char[] { '/' });
            string fromdateincre = fromdate_split[1] + "/" + fromdate_split[0] + "/" + fromdate_split[2];
            DateTime fromdtime = Convert.ToDateTime(fromdateincre);

            string[] todate_split = txtto.Text.Split(new char[] { '/' });
            string todateincre = todate_split[1] + "/" + todate_split[0] + "/" + todate_split[2];
            DateTime todtime = Convert.ToDateTime(todateincre);

            con.Close();
            con.Open();

            SqlCommand cmd_gprsdata = new SqlCommand("select min(time) as time,vehicleid,date,rfiddata,googlelocation,flag_status from vtsgprsdata where rfiddata<>'00000000' group by rfiddata,googlelocation,date,flag_status,vehicleid", con);
            SqlDataAdapter da_gprsdata = new SqlDataAdapter(cmd_gprsdata);
            DataTable dt_gprsdata = new DataTable();
            da_gprsdata.Fill(dt_gprsdata);

            con.Close();
            con.Open();

            SqlCommand cmd_gprsdata1 = new SqlCommand("select max(time) as time,vehicleid,date,rfiddata,googlelocation,flag_status from vtsgprsdata where rfiddata<>'00000000' group by rfiddata,googlelocation,date,flag_status,vehicleid", con);
            SqlDataAdapter da_gprsdata1 = new SqlDataAdapter(cmd_gprsdata1);
            DataTable dt_gprsdata_1 = new DataTable();
            da_gprsdata1.Fill(dt_gprsdata_1);


            con.Close();
            con.Open();

            SqlCommand cmd_reg = new SqlCommand("select * from registration order by vehid,boarding", con);
            SqlDataAdapter da_reg = new SqlDataAdapter(cmd_reg); DataTable dt_reg = new DataTable();
            da_reg.Fill(dt_reg);

            con.Close();
            con.Open();

            SqlCommand cmd_get_address = new SqlCommand("select s.stage_id,s.stage_name,r.route_id,s.address from stage_master s,RouteMaster r where cast(r.stage_name as varchar(100))=cast(s.stage_id as varchar(100))", con);
            SqlDataAdapter ad_get_address = new SqlDataAdapter(cmd_get_address);
            DataTable dt_get_address = new DataTable();
            ad_get_address.Fill(dt_get_address);

            con.Close();
            con.Open();

            SqlCommand cmd_department = new SqlCommand("select * from department", con);
            SqlDataAdapter da_department = new SqlDataAdapter(cmd_department);
            DataTable dt_department = new DataTable();
            da_department.Fill(dt_department);


            //Fp_Absenties.Sheets[0].RowCount = 0;
            Fp_InOut.Sheets[0].RowCount = 0;
            int slno = 0;

            if (Rbtnlstinout.Items[0].Selected == true)
            {

                for (int i = 0; i < 2; i++)
                {
                    Fp_InOut.Sheets[0].ColumnCount++;
                }
                Fp_InOut.Sheets[0].ColumnHeader.Cells[Fp_InOut.Sheets[0].ColumnHeader.RowCount - 1, 7].Text = "In Time";
                Fp_InOut.Sheets[0].ColumnHeader.Cells[Fp_InOut.Sheets[0].ColumnHeader.RowCount - 1, 8].Text = "Out Time";
                Fp_InOut.Sheets[0].Columns[6].Width = 70;
                Fp_InOut.Sheets[0].Columns[7].Width = 70;

            }
            else if (Rbtnlstinout.Items[1].Selected == true)
            {
                for (int i = 0; i < 1; i++)
                {
                    Fp_InOut.Sheets[0].ColumnCount++;
                }


                Fp_InOut.Sheets[0].ColumnHeader.Cells[Fp_InOut.Sheets[0].ColumnHeader.RowCount - 1, 7].Text = "In Time";
                Fp_InOut.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fp_InOut.Sheets[0].Columns[6].Width = 70;
            }

            else if (Rbtnlstinout.Items[2].Selected == true)
            {
                for (int i = 0; i < 1; i++)
                {
                    Fp_InOut.Sheets[0].ColumnCount++;
                }


                Fp_InOut.Sheets[0].ColumnHeader.Cells[Fp_InOut.Sheets[0].ColumnHeader.RowCount - 1, 7].Text = "Out Time";
                Fp_InOut.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fp_InOut.Sheets[0].Columns[6].Width = 70;
            }

            for (DateTime cnt_datetime = fromdtime; cnt_datetime <= todtime; cnt_datetime = cnt_datetime.AddDays(1))
            {
                string incre_date = cnt_datetime.ToString("MM/dd/yyyy");
                string[] split_date = incre_date.Split('/');
                string splited_date = split_date[1] + split_date[0] + Convert.ToString(Convert.ToInt32(split_date[2]) - 2000);
                string spread_date = split_date[1] + "/" + split_date[0] + "/" + split_date[2];
                complete_date = splited_date;

                for (int cnt_vehi = 0; cnt_vehi < chkls_vech.Items.Count; cnt_vehi++)
                {
                    if (chkls_vech.Items[cnt_vehi].Selected == true)
                    {
                        string vehid = chkls_vech.Items[cnt_vehi].Text.ToString();
                        string route_id = Chkls_route.Items[cnt_vehi].Text.ToString();

                        for (int cnt_stage = 0; cnt_stage < Chkls_stage.Items.Count; cnt_stage++)
                        {
                            if (Chkls_stage.Items[cnt_stage].Selected == true)
                            {
                                sel_stage = Chkls_stage.Items[cnt_stage].Value.ToString(); // Chkls_stage.Items[cnt_stage].Text.ToString();

                                // get Time from Intime
                                int intimefrom_hr = 0;
                                string intimehr = string.Empty;
                                int intimefrom_min = 0;
                                string intimemin = string.Empty;

                                string inhr_min = string.Empty;

                                int intimeto_hr = 0;
                                string intimetohr = string.Empty;
                                int intimeto_min = 0;
                                string intimetomin = string.Empty;

                                string intimetohr_min = string.Empty;

                                // get Time from Out Time

                                int outtimefrom_hr = 0;
                                string outtime_hr = string.Empty;
                                int outtimefrom_min = 0;
                                string outtimemin = string.Empty;

                                string outhrmin = string.Empty;

                                int outtimeto_hr = 0;
                                string outtimetohr = string.Empty;
                                int outtimeto_min = 0;
                                string outtimetomin = string.Empty;

                                string outtimetohr_min = string.Empty;

                                if (Rbtnlstinout.Items[0].Selected == true)
                                {
                                    //-----------Hour and Minutes Calculation for In time
                                    if (ddl_fromhr.Text != "HH" && ddl_frommin.Text != "MM" && ddl_tohr.Text != "HH" && ddl_tomin.Text != "MM")
                                    {
                                        intimefrom_hr = Convert.ToInt32(ddl_fromhr.Text.ToString());
                                        intimehr = ddl_fromhr.Text.ToString();
                                        intimefrom_min = Convert.ToInt32(ddl_frommin.Text.ToString());
                                        intimemin = ddl_frommin.Text.ToString();

                                        inhr_min = intimehr + intimemin + "00";

                                        if (ddl_frommerdian.Text == "PM" && ddl_fromhr.Text != "12")
                                        {
                                            intimefrom_hr = 12 + intimefrom_hr;
                                            inhr_min = intimefrom_hr + intimemin + "00";
                                        }

                                        intimeto_hr = Convert.ToInt32(ddl_tohr.Text.ToString());
                                        intimetohr = ddl_tohr.Text.ToString();
                                        intimeto_min = Convert.ToInt32(ddl_tomin.Text.ToString());
                                        intimetomin = ddl_tomin.Text.ToString();

                                        intimetohr_min = intimetohr + intimetomin + "00";

                                        if (ddl_tomeridian.Text == "PM" && ddl_tohr.Text != "12")
                                        {
                                            intimeto_hr = 12 + intimeto_hr;
                                            intimetohr_min = intimeto_hr + intimetomin + "00";
                                        }
                                    }

                                    //------------Hour Only calculation for In time
                                    else if (ddl_fromhr.Text != "HH" && ddl_tohr.Text != "HH")
                                    {
                                        intimefrom_hr = Convert.ToInt32(ddl_fromhr.Text.ToString());
                                        intimehr = ddl_fromhr.Text.ToString();

                                        inhr_min = intimehr + "00" + "00";

                                        if (ddl_frommerdian.Text == "PM" && ddl_fromhr.Text != "12")
                                        {
                                            intimefrom_hr = 12 + intimefrom_hr;
                                            inhr_min = intimefrom_hr + "00" + "00";
                                        }

                                        intimeto_hr = Convert.ToInt32(ddl_tohr.Text.ToString());
                                        intimetohr = ddl_tohr.Text.ToString();

                                        intimetohr_min = intimetohr + "00" + "00";

                                        if (ddl_tomeridian.Text == "PM" && ddl_tohr.Text != "12")
                                        {
                                            intimeto_hr = 12 + intimeto_hr;
                                            intimetohr_min = intimeto_hr + "00" + "00";
                                        }
                                    }
                                    //------------- Hour and Minutes Calculation for Out Time

                                    if (ddl_fromouthr.Text != "HH" && ddl_fromoutmin.Text != "MM" && ddl_toouthr.Text != "HH" && ddl_tooutmin.Text != "MM")
                                    {
                                        outtimefrom_hr = Convert.ToInt32(ddl_fromouthr.Text.ToString());
                                        outtime_hr = ddl_fromouthr.Text.ToString();
                                        outtimefrom_min = Convert.ToInt32(ddl_fromoutmin.Text.ToString());
                                        outtimemin = ddl_fromoutmin.Text.ToString();

                                        outhrmin = outtime_hr + outtimemin + "00";

                                        if (ddl_fromoutmeri.Text == "PM" && ddl_fromouthr.Text != "12")
                                        {
                                            outtimefrom_hr = 12 + outtimefrom_hr;
                                            outhrmin = outtimefrom_hr + outtimemin + "00";
                                        }

                                        outtimeto_hr = Convert.ToInt32(ddl_toouthr.Text.ToString());
                                        outtimetohr = ddl_toouthr.Text.ToString();
                                        outtimeto_min = Convert.ToInt32(ddl_tooutmin.Text.ToString());
                                        outtimetomin = ddl_tooutmin.Text.ToString();

                                        outtimetohr_min = intimetohr + intimetomin + "00";

                                        if (ddl_tooutmeri.Text == "PM" && ddl_toouthr.Text != "12")
                                        {
                                            outtimeto_hr = 12 + outtimeto_hr;
                                            outtimetohr_min = outtimeto_hr + outtimetomin + "00";
                                        }
                                    }

                                    //------------Hour only Calculation for Out Time                              


                                    else if (ddl_fromouthr.Text != "HH" && ddl_fromouthr.Text != "HH")
                                    {
                                        outtimefrom_hr = Convert.ToInt32(ddl_fromouthr.Text.ToString());
                                        outtime_hr = ddl_fromouthr.Text.ToString();

                                        outhrmin = outtime_hr + "00" + "00";

                                        if (ddl_fromoutmeri.Text == "PM" && ddl_fromouthr.Text != "12")
                                        {
                                            outtimefrom_hr = 12 + outtimefrom_hr;
                                            outhrmin = outtimefrom_hr + "00" + "00";
                                        }

                                        outtimeto_hr = Convert.ToInt32(ddl_toouthr.Text.ToString());
                                        outtimetohr = ddl_toouthr.Text.ToString();

                                        outtimetohr_min = intimetohr + "00" + "00";

                                        if (ddl_tooutmeri.Text == "PM" && ddl_toouthr.Text != "12")
                                        {
                                            outtimeto_hr = 12 + outtimeto_hr;
                                            outtimetohr_min = outtimeto_hr + "00" + "00";
                                        }
                                    }
                                }
                                if (Rbtnlstinout.Items[1].Selected == true)
                                {
                                    if (ddl_fromhr.Text != "HH" && ddl_frommin.Text != "MM" && ddl_tohr.Text != "HH" && ddl_tomin.Text != "MM")
                                    {
                                        intimefrom_hr = Convert.ToInt32(ddl_fromhr.Text.ToString());
                                        intimehr = ddl_fromhr.Text.ToString();
                                        intimefrom_min = Convert.ToInt32(ddl_frommin.Text.ToString());
                                        intimemin = ddl_frommin.Text.ToString();

                                        inhr_min = intimehr + intimemin + "00";

                                        if (ddl_frommerdian.Text == "PM" && ddl_fromhr.Text != "12")
                                        {
                                            intimefrom_hr = 12 + intimefrom_hr;
                                            inhr_min = intimefrom_hr + intimemin + "00";
                                        }

                                        intimeto_hr = Convert.ToInt32(ddl_tohr.Text.ToString());
                                        intimetohr = ddl_tohr.Text.ToString();
                                        intimeto_min = Convert.ToInt32(ddl_tomin.Text.ToString());
                                        intimetomin = ddl_tomin.Text.ToString();

                                        intimetohr_min = intimetohr + intimetomin + "00";

                                        if (ddl_tomeridian.Text == "PM" && ddl_tohr.Text != "12")
                                        {
                                            intimeto_hr = 12 + intimeto_hr;
                                            intimetohr_min = intimeto_hr + intimetomin + "00";
                                        }
                                    }

                                    //------------Hour Only calculation for In time
                                    else if (ddl_fromhr.Text != "HH" && ddl_tohr.Text != "HH")
                                    {
                                        intimefrom_hr = Convert.ToInt32(ddl_fromhr.Text.ToString());
                                        intimehr = ddl_fromhr.Text.ToString();

                                        inhr_min = intimehr + "00" + "00";

                                        if (ddl_frommerdian.Text == "PM" && ddl_fromhr.Text != "12")
                                        {
                                            intimefrom_hr = 12 + intimefrom_hr;
                                            inhr_min = intimefrom_hr + "00" + "00";
                                        }

                                        intimeto_hr = Convert.ToInt32(ddl_tohr.Text.ToString());
                                        intimetohr = ddl_tohr.Text.ToString();

                                        intimetohr_min = intimetohr + "00" + "00";

                                        if (ddl_tomeridian.Text == "PM" && ddl_tohr.Text != "12")
                                        {
                                            intimeto_hr = 12 + intimeto_hr;
                                            intimetohr_min = intimeto_hr + "00" + "00";
                                        }
                                    }
                                }

                                if (Rbtnlstinout.Items[2].Selected == true)
                                {
                                    if (ddl_fromouthr.Text != "HH" && ddl_fromoutmin.Text != "MM" && ddl_toouthr.Text != "HH" && ddl_tooutmin.Text != "MM")
                                    {
                                        outtimefrom_hr = Convert.ToInt32(ddl_fromouthr.Text.ToString());
                                        outtime_hr = ddl_fromouthr.Text.ToString();
                                        outtimefrom_min = Convert.ToInt32(ddl_fromoutmin.Text.ToString());
                                        outtimemin = ddl_fromoutmin.Text.ToString();

                                        outhrmin = outtime_hr + outtimemin + "00";

                                        if (ddl_fromoutmeri.Text == "PM" && ddl_fromouthr.Text != "12")
                                        {
                                            outtimefrom_hr = 12 + outtimefrom_hr;
                                            outhrmin = outtimefrom_hr + outtimemin + "00";
                                        }

                                        outtimeto_hr = Convert.ToInt32(ddl_toouthr.Text.ToString());
                                        outtimetohr = ddl_toouthr.Text.ToString();
                                        outtimeto_min = Convert.ToInt32(ddl_tooutmin.Text.ToString());
                                        outtimetomin = ddl_tooutmin.Text.ToString();

                                        outtimetohr_min = intimetohr + intimetomin + "00";

                                        if (ddl_tooutmeri.Text == "PM" && ddl_toouthr.Text != "12")
                                        {
                                            outtimeto_hr = 12 + outtimeto_hr;
                                            outtimetohr_min = outtimeto_hr + outtimetomin + "00";
                                        }
                                    }

                                    else if (ddl_fromouthr.Text != "HH" && ddl_fromouthr.Text != "HH")
                                    {
                                        outtimefrom_hr = Convert.ToInt32(ddl_fromouthr.Text.ToString());
                                        outtime_hr = ddl_fromouthr.Text.ToString();

                                        outhrmin = outtime_hr + "00" + "00";

                                        if (ddl_fromoutmeri.Text == "PM" && ddl_fromouthr.Text != "12")
                                        {
                                            outtimefrom_hr = 12 + outtimefrom_hr;
                                            outhrmin = outtimefrom_hr + "00" + "00";
                                        }

                                        outtimeto_hr = Convert.ToInt32(ddl_toouthr.Text.ToString());
                                        outtimetohr = ddl_toouthr.Text.ToString();
                                        outtimetohr_min = intimetohr + "00" + "00";

                                        if (ddl_tooutmeri.Text == "PM" && ddl_toouthr.Text != "12")
                                        {
                                            outtimeto_hr = 12 + outtimeto_hr;
                                            outtimetohr_min = outtimeto_hr + "00" + "00";
                                        }
                                    }
                                }

                                DataView dv_gprsdata = new DataView();
                                DataView dv_studname = new DataView();
                                DataView dv_department = new DataView();
                                DataView dv_outtime = new DataView();
                                DataView dv_get_address = new DataView();

                                dt_get_address.DefaultView.RowFilter = "route_id='" + route_id + "' and stage_id='" + sel_stage + "'";
                                dv_get_address = dt_get_address.DefaultView;

                                string goo_stage = string.Empty;

                                if (dv_get_address.Count > 0)
                                {
                                    goo_stage = dv_get_address[0]["Address"].ToString();
                                }


                                if (Rbtnlstinout.Items[0].Selected == true)
                                {

                                    dt_gprsdata.DefaultView.RowFilter = "vehicleid ='" + vehid + "' and date ='" + complete_date + "' and Googlelocation='" + goo_stage + "'";
                                    dv_gprsdata = dt_gprsdata.DefaultView;

                                }

                                if (Rbtnlstinout.Items[1].Selected == true)
                                {

                                    dt_gprsdata.DefaultView.RowFilter = "vehicleid ='" + vehid + "' and date ='" + complete_date + "' and Googlelocation='" + goo_stage + "' and time>='" + inhr_min + "' and time<='" + intimetohr_min + "'";
                                    dv_gprsdata = dt_gprsdata.DefaultView;

                                }

                                if (Rbtnlstinout.Items[2].Selected == true)
                                {

                                    dt_gprsdata.DefaultView.RowFilter = "vehicleid ='" + vehid + "' and date ='" + complete_date + "' and Googlelocation='" + goo_stage + "' and time>='" + outhrmin + "' and time<='" + outtimetohr_min + "'";
                                    dv_outtime = dt_gprsdata.DefaultView;
                                }

                                if (Rbtnlstinout.Items[0].Selected == true || Rbtnlstinout.Items[1].Selected == true)
                                {
                                    for (int cnt_dvgprs = 0; cnt_dvgprs < dv_gprsdata.Count; cnt_dvgprs++)
                                    {
                                        string rol_no = dv_gprsdata[cnt_dvgprs]["RFIDData"].ToString();
                                        string veh_id = dv_gprsdata[cnt_dvgprs]["VehicleId"].ToString();
                                        string stud_stage = dv_gprsdata[cnt_dvgprs]["Googlelocation"].ToString();
                                        string intime = dv_gprsdata[cnt_dvgprs]["time"].ToString();
                                        string outtime = "";
                                        string time_1 = "";

                                        dt_gprsdata_1.DefaultView.RowFilter = "vehicleid ='" + vehid + "' and RFIDData='" + rol_no + "' and date ='" + complete_date + "' and Googlelocation ='" + goo_stage + "' and time>='" + outhrmin + "' and time<='" + outtimetohr_min + "'";
                                        dv_outtime = dt_gprsdata_1.DefaultView;

                                        if (dv_outtime.Count > 0)
                                        {
                                            outtime = dv_outtime[0]["time"].ToString();
                                            string[] outtime_1 = Regex.Split(outtime, "");
                                            time_1 = outtime_1[1] + outtime_1[2] + ":" + outtime_1[3] + outtime_1[4] + ":" + outtime_1[5] + outtime_1[6];
                                        }

                                        string[] sub_string = Regex.Split(intime, "");
                                        string time = sub_string[1] + sub_string[2] + ":" + sub_string[3] + sub_string[4] + ":" + sub_string[5] + sub_string[6];


                                        dt_reg.DefaultView.RowFilter = "smart_serial_no ='" + rol_no + "'";
                                        dv_studname = dt_reg.DefaultView;

                                        string branchcode = string.Empty;
                                        string stud_name = string.Empty;
                                        string rollno = string.Empty;

                                        if (dv_studname.Count > 0)
                                        {
                                            branchcode = dv_studname[0]["Branch_code"].ToString();
                                            stud_name = dv_studname[0]["Stud_name"].ToString();
                                            rollno = dv_studname[0]["Roll_no"].ToString();
                                        }

                                        dt_department.DefaultView.RowFilter = "dept_code='" + branchcode + "'";
                                        dv_department = dt_department.DefaultView;

                                        string dept_name = "";
                                        if (dv_department.Count > 0)
                                        {
                                            dept_name = dv_department[0]["Dept_Name"].ToString();
                                        }

                                        if (Rbtnlstinout.Items[0].Selected == true)
                                        {
                                            for (int cnt_studname = 0; cnt_studname < dv_studname.Count; cnt_studname++)
                                            {

                                                slno++;
                                                Fp_InOut.Sheets[0].RowCount++;
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 0].Text = slno.ToString();
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 1].Text = spread_date.ToString();
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 2].Text = veh_id.ToString();
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 3].Text = dept_name.ToString();
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 4].Text = rollno.ToString();
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 5].Text = stud_name.ToString();
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 6].Text = stud_stage.ToString();
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 7].Text = time.ToString();
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 8].Text = time_1.ToString();
                                            }

                                            Fp_InOut.Sheets[0].PageSize = Fp_InOut.Sheets[0].RowCount;
                                            Fp_InOut.Visible = true;
                                            Fp_Absenties.Visible = false;
                                            FpTransport.Visible = false;
                                        }

                                        else if (Rbtnlstinout.Items[1].Selected == true)
                                        {
                                            for (int cnt_studname = 0; cnt_studname < dv_studname.Count; cnt_studname++)
                                            {
                                                //string stud_name = dv_studname[0]["Stud_name"].ToString();
                                                slno++;
                                                Fp_InOut.Sheets[0].RowCount++;
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 0].Text = slno.ToString();
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 1].Text = spread_date.ToString();
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 2].Text = veh_id.ToString();
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 3].Text = dept_name.ToString();
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 4].Text = rol_no.ToString();
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 5].Text = stud_name.ToString();
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 6].Text = stud_stage.ToString();
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 7].Text = time.ToString();
                                            }

                                            Fp_InOut.Sheets[0].PageSize = Fp_InOut.Sheets[0].RowCount;
                                            Fp_InOut.Visible = true;
                                            Fp_Absenties.Visible = false;
                                            FpTransport.Visible = false;
                                        }

                                    }
                                }

                                if (Rbtnlstinout.Items[2].Selected == true)
                                {
                                    for (int i = 0; i < dv_outtime.Count; i++)
                                    {

                                        string rol_no = dv_outtime[i]["RFIDData"].ToString();
                                        string veh_id = dv_outtime[i]["VehicleId"].ToString();
                                        string stud_stage = dv_outtime[i]["Googlelocation"].ToString();

                                        string outtime = "";
                                        string time_1 = "";

                                        dt_gprsdata_1.DefaultView.RowFilter = "vehicleid ='" + vehid + "' and RFIDData='" + rol_no + "' and date ='" + complete_date + "' and Googlelocation ='" + goo_stage + "' and time>='" + outhrmin + "' and time<='" + outtimetohr_min + "'"; ;
                                        dv_outtime = dt_gprsdata_1.DefaultView;

                                        if (dv_outtime.Count > 0)
                                        {
                                            outtime = dv_outtime[i]["time"].ToString();

                                            string[] outtime_1 = Regex.Split(outtime, "");
                                            time_1 = outtime_1[1] + outtime_1[2] + ":" + outtime_1[3] + outtime_1[4] + ":" + outtime_1[5] + outtime_1[6];

                                        }

                                        dt_reg.DefaultView.RowFilter = "smart_serial_no ='" + rol_no + "'";
                                        dv_studname = dt_reg.DefaultView;

                                        string branchcode = string.Empty;
                                        if (dv_studname.Count > 0)
                                        {
                                            branchcode = dv_studname[0]["Branch_code"].ToString();
                                        }

                                        dt_department.DefaultView.RowFilter = "dept_code='" + branchcode + "'";
                                        dv_department = dt_department.DefaultView;

                                        string dept_name = "";
                                        if (dv_department.Count > 0)
                                        {
                                            dept_name = dv_department[0]["Dept_Name"].ToString();
                                        }

                                        if (Rbtnlstinout.Items[2].Selected == true)
                                        {
                                            for (int cnt_studname = 0; cnt_studname < dv_studname.Count; cnt_studname++)
                                            {
                                                string stud_name = dv_studname[0]["Stud_name"].ToString();
                                                slno++;
                                                Fp_InOut.Sheets[0].RowCount++;
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 0].Text = slno.ToString();
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 1].Text = spread_date.ToString();
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 2].Text = veh_id.ToString();
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 3].Text = dept_name.ToString();
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 4].Text = rol_no.ToString();
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 5].Text = stud_name.ToString();
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 6].Text = stud_stage.ToString();
                                                Fp_InOut.Sheets[0].Cells[Fp_InOut.Sheets[0].RowCount - 1, 7].Text = time_1.ToString();
                                            }

                                            Fp_InOut.Sheets[0].PageSize = Fp_InOut.Sheets[0].RowCount;
                                            Fp_InOut.Visible = true;
                                            Fp_Absenties.Visible = false;
                                            FpTransport.Visible = false;

                                        }
                                    }
                                }

                            }
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
    protected void Rbtnlstinout_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Rbtnlstinout.Items[0].Selected == true)
        {
            ddl_fromouthr.Enabled = true;
            ddl_fromoutmin.Enabled = true;
            ddl_fromoutmeri.Enabled = true;
            ddl_toouthr.Enabled = true;
            ddl_tooutmin.Enabled = true;
            ddl_tooutmeri.Enabled = true;

            ddl_fromhr.Enabled = true;
            ddl_frommin.Enabled = true;
            ddl_frommerdian.Enabled = true;
            ddl_tohr.Enabled = true;
            ddl_tomin.Enabled = true;
            ddl_tomeridian.Enabled = true;
        }
        else if (Rbtnlstinout.Items[1].Selected == true)
        {
            ddl_fromouthr.Enabled = false;
            ddl_fromoutmin.Enabled = false;
            ddl_fromoutmeri.Enabled = false;
            ddl_toouthr.Enabled = false;
            ddl_tooutmin.Enabled = false;
            ddl_tooutmeri.Enabled = false;



            ddl_fromhr.Enabled = true;
            ddl_frommin.Enabled = true;
            ddl_frommerdian.Enabled = true;
            ddl_tohr.Enabled = true;
            ddl_tomin.Enabled = true;
            ddl_tomeridian.Enabled = true;
        }
        else if (Rbtnlstinout.Items[2].Selected == true)
        {
            ddl_fromhr.Enabled = false;
            ddl_frommin.Enabled = false;
            ddl_frommerdian.Enabled = false;
            ddl_tohr.Enabled = false;
            ddl_tomin.Enabled = false;
            ddl_tomeridian.Enabled = false;



            ddl_fromouthr.Enabled = true;
            ddl_fromoutmin.Enabled = true;
            ddl_fromoutmeri.Enabled = true;
            ddl_toouthr.Enabled = true;
            ddl_tooutmin.Enabled = true;
            ddl_tooutmeri.Enabled = true;
        }

    }
    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        string intime = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
        if (Session["Entry_Code"].ToString() == null)
        {
            Session["Entry_Code"] = 0;
        }
        int a = d2.update_method_wo_parameter("update UserEELog  set Out_Time='" + intime + "',LogOff='1' where entry_code='" + Session["Entry_Code"] + "'", "Text");

        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }
}




