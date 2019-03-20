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
public partial class Transport_strength_Report : System.Web.UI.Page
{
    DAccess2 DataAccess = new DAccess2();
    Hashtable ht = new Hashtable();
    DataSet newds = new DataSet();
    bool chk = false;
    string usercode = "", collegecode = "", singleuser = "", group_user = "";
    int ar;
    int ac;
    static string selected_college = "", selected_courseid = "", selected_depid = "", selected_batch = "";
    static string college_code = "", stage_id = "",stage_id1 = "", veh_id = "",veh_id1="", route_id = "",route_id1="", college_code_1 = "";
    static string stage_id_1 = "", veh_id_1 = "", route_id_1 = "", dist_code = "", dist_code_1 = "", discode = "";
    int count = 0;
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
        usercode = Session["usercode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        try
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
            lblerrmsg.Visible = false;
            if (!IsPostBack)
            {
                setLabelText();
                spread_style();
                load_college();
                chk_college.Checked = true;
                chk_college_ChekedChanged(sender, e);
                load_district();
                chk_district.Checked = true;
                chk_district_ChekedChanged(sender, e);
                //load_stage();
                chk_stage.Checked = true;
                chk_stage_ChekedChanged(sender, e);
                load_vehicle();
                chk_vehicle.Checked = true;
                chk_vehicle_ChekedChanged(sender, e);
                load_route();
                chk_route.Checked = true;
                chk_route_ChekedChanged(sender, e);

                load_batch();
                chk_batch.Checked = true;
                chk_batch_CheckedChanged(sender, e);
                load_degree();
                chk_degree.Checked = true;
                chk_degree_CheckedChanged(sender, e);

                fp_stud.Sheets[0].AutoPostBack = true;
                ddl_sex_SelectedIndexChanged(sender, e);
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void load_district()
    {
        DataSet dnew = new DataSet();
        string sqlcmd = string.Empty;
        
        //modified by prabha on jan 31 2018
        //sqlcmd = "select distinct textcode,textval from textvaltable where textcriteria in ('Sdis','dis')";  //existing
        sqlcmd = "select distinct textcode,textval from stage_master s,textvaltable tv where tv.textcode=s.district and tv.textcriteria in('Sdis','dis') "; //new
        dnew = DataAccess.select_method_wo_parameter(sqlcmd, "text");
        if (dnew.Tables[0].Rows.Count > 0)
        {
            chklst_district.Items.Clear();

            chklst_district.DataSource = dnew.Tables[0];
            chklst_district.DataTextField = "textval";
            chklst_district.DataValueField = "textcode";
            chklst_district.DataBind();
        }

    }
    public void load_stage()
    {
        chklst_stage.Items.Clear();

        if (discode != "")
        {
            DataSet dnew1 = new DataSet();
            chklst_stage.Items.Clear();
            string sql;
            sql = "select distinct Stage_Name,Stage_id from stage_master s where " + discode + " order by Stage_Name";
            dnew1 = DataAccess.select_method_wo_parameter(sql, "txt");
            if (dnew1.Tables[0].Rows.Count > 0)
            {
                chklst_stage.DataSource = dnew1.Tables[0];
                chklst_stage.DataTextField = "Stage_Name";
                chklst_stage.DataValueField = "Stage_id";
                chklst_stage.DataBind();

            }
            
        }
        //count = 0;
        //for (int i = 0; i < chklst_stage.Items.Count; i++)
        //{
        //    if (chklst_stage.Items[i].Selected == true)
        //    {
        //        count++;
        //        chklst_stage.Items[i].Selected = true;
        //        txt_stage.Text = "Dist(" + count.ToString() + ")";
        //        if (dist_code == string.Empty)
        //        {
        //            dist_code = chklst_district.Items[i].Value;
        //            dist_code_1 = chklst_district.Items[i].Value;
        //        }
        //        else
        //        {
        //            dist_code = dist_code + "," + chklst_district.Items[i].Value;
        //            dist_code_1 = dist_code_1 + "," + chklst_district.Items[i].Value;
        //        }

        //    }
        //}
        //if (dist_code.ToString().Trim() != "")
        //{
        //    discode = "  s.district in(" + dist_code + ") ";
        //    dist_code = " and s.district in(" + dist_code + ") ";
        //    dist_code_1 = " and s.district in(" + dist_code + ") ";

        //}
    }
    public void load_vehicle()//rajasekar
    {
        chklst_vehicle.Items.Clear();
        if (route_id1 != "")
        {
            chklst_vehicle.Items.Clear();
            DataSet ds = new DataSet();
            string sql;

            //select * from vehicle_master order by len(veh_id), Veh_ID
            sql = "select distinct  convert (int,Veh_ID) Veh_ID from routemaster r where " + route_id1 + "  order by Veh_ID";
            ds = DataAccess.select_method_wo_parameter(sql, "txt");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklst_vehicle.DataSource = ds.Tables[0];
                chklst_vehicle.DataTextField = "Veh_ID";
                chklst_vehicle.DataValueField = "Veh_ID";
                chklst_vehicle.DataBind();

            }
        }

    }
    public void load_route()//rajasekar
    {
        chklst_route.Items.Clear();
        if (stage_id1 != "")
        {

            DataSet ds = new DataSet();
            string sqlquery = string.Empty;
            chklst_route.Items.Clear();
            //select distinct Route_ID from routemaster
            sqlquery = "select distinct Route_Id from routemaster r where   " + stage_id1 + "  order by Route_Id";

            ds = DataAccess.select_method_wo_parameter(sqlquery, "txt");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklst_route.DataSource = ds.Tables[0];
                chklst_route.DataTextField = "Route_ID";
                chklst_route.DataValueField = "Route_ID";
                chklst_route.DataBind();

            }
        }
    }
    public void load_college()
    {
        DataSet dss = new DataSet();
        Hashtable hat = new Hashtable();
        string grouporusercode = "";
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
        }
        else
        {
            grouporusercode = " and user_code=" + Session["usercode"].ToString().Trim() + "";
        }
        hat.Add("column_field", grouporusercode);
        dss = DataAccess.select_method("bind_college", hat, "sp");
        if (dss.Tables[0].Rows.Count > 0)
        {

            chklst_college.DataSource = dss;
            chklst_college.DataTextField = "collname";
            chklst_college.DataValueField = "college_code";
            chklst_college.DataBind();
        }
    }
    public void spread_style()
    {

        Fp_strength.Sheets[0].AutoPostBack = true;
        Fp_strength.CommandBar.Visible = false;
        FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
        style2.Font.Size = 10;
        style2.Font.Bold = true;
        Fp_strength.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style2);
        Fp_strength.Sheets[0].AllowTableCorner = true;
        Fp_strength.Sheets[0].RowHeader.Visible = false;
        Fp_strength.Sheets[0].SheetName = " ";
        Fp_strength.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        Fp_strength.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        Fp_strength.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        Fp_strength.Sheets[0].DefaultColumnWidth = 50;
        Fp_strength.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        Fp_strength.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        Fp_strength.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        Fp_strength.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        Fp_strength.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        Fp_strength.Sheets[0].DefaultStyle.Font.Bold = false;
        Fp_strength.SheetCorner.Cells[0, 0].Font.Bold = true;
        Fp_strength.Sheets[0].ColumnCount = 0;
        Fp_strength.Visible = false;

        fp_stud.Sheets[0].AutoPostBack = true;
        fp_stud.CommandBar.Visible = false;
        style2.Font.Size = 10;
        style2.Font.Bold = true;
        fp_stud.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style2);
        fp_stud.Sheets[0].AllowTableCorner = true;
        fp_stud.Sheets[0].RowHeader.Visible = false;
        fp_stud.Sheets[0].SheetName = " ";
        fp_stud.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        fp_stud.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        fp_stud.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        fp_stud.Sheets[0].DefaultColumnWidth = 50;
        fp_stud.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        fp_stud.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        fp_stud.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        fp_stud.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        fp_stud.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        fp_stud.Sheets[0].DefaultStyle.Font.Bold = false;
        fp_stud.SheetCorner.Cells[0, 0].Font.Bold = true;
        Fp_strength.Sheets[0].ColumnCount = 0;
        fp_stud.Visible = false;

        Fp_Individual_Strength.Sheets[0].AutoPostBack = true;
        Fp_Individual_Strength.CommandBar.Visible = false;
        style2.Font.Size = 10;
        style2.Font.Bold = true;
        Fp_Individual_Strength.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style2);
        Fp_Individual_Strength.Sheets[0].AllowTableCorner = true;
        Fp_Individual_Strength.Sheets[0].RowHeader.Visible = false;
        Fp_Individual_Strength.Sheets[0].SheetName = " ";
        Fp_Individual_Strength.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        Fp_Individual_Strength.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        Fp_Individual_Strength.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        Fp_Individual_Strength.Sheets[0].DefaultColumnWidth = 50;
        Fp_Individual_Strength.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        Fp_Individual_Strength.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        Fp_Individual_Strength.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        Fp_Individual_Strength.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        Fp_Individual_Strength.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        Fp_Individual_Strength.Sheets[0].DefaultStyle.Font.Bold = false;
        Fp_Individual_Strength.SheetCorner.Cells[0, 0].Font.Bold = true;
        Fp_Individual_Strength.Sheets[0].ColumnCount = 0;

        Fp_Individual_Strength.Visible = false;

    }
    protected void chk_district_ChekedChanged(object sender, EventArgs e)
    {
        int count = 0;
        dist_code = "";
        dist_code_1 = "";
        discode = "";
        chk_stage.Checked = false;
        chk_route.Checked = false;
        chk_vehicle.Checked = false;
        if (chk_district.Checked == true)
        {

            for (int i = 0; i < chklst_district.Items.Count; i++)
            {
                count++;
                chklst_district.Items[i].Selected = true;
                txt_district.Text = "Dist(" + count.ToString() + ")";
                if (dist_code == string.Empty)
                {
                    dist_code = chklst_district.Items[i].Value;
                    dist_code_1 = chklst_district.Items[i].Value;
                }
                else
                {
                    dist_code = dist_code + "," + chklst_district.Items[i].Value;
                    dist_code_1 = dist_code_1 + "," + chklst_district.Items[i].Value;
                }
            }
            
            
            

        }
        else
        {
            for (int i = 0; i < chklst_district.Items.Count; i++)
            {

                chklst_district.Items[i].Selected = false;
                txt_district.Text = "--Select--";
            }
        }
        if (dist_code.ToString().Trim() != "")
        {
            discode = "  s.district in(" + dist_code + ") ";
            dist_code = " and s.district in(" + dist_code + ") ";
            dist_code_1 = " and s.district in(" + dist_code + ") ";

        }
        load_stage();
        chk_stage.Checked = true;
        chk_stage_ChekedChanged(sender, e);//rajasekar
        
    }
    protected void chklst_district_SelectedIndexChanged(object sender, EventArgs e)
    {
        int count = 0;
        dist_code = "";
        dist_code_1 = "";
        discode = "";
        chk_district.Checked = false;//rajasekar
        chk_stage.Checked = false;
        txt_district.Text="--Select-- ";
        for (int i = 0; i < chklst_district.Items.Count; i++)
        {
            if (chklst_district.Items[i].Selected == true)
            {
                count++;
                chklst_district.Items[i].Selected = true;
                txt_district.Text = "Dist(" + count.ToString() + ")";
                if (dist_code == string.Empty)
                {
                    dist_code = chklst_district.Items[i].Value;
                    dist_code_1 = chklst_district.Items[i].Value;
                }
                else
                {
                    dist_code = dist_code + "," + chklst_district.Items[i].Value;
                    dist_code_1 = dist_code_1 + "," + chklst_district.Items[i].Value;
                }

            }
        }
        if (dist_code.ToString().Trim() != "")
        {
            discode = "  s.district in(" + dist_code + ") ";
            dist_code = " and s.district in(" + dist_code + ") ";
            dist_code_1 = " and s.district in(" + dist_code + ") ";

            
        }
        load_stage();
        chk_stage.Checked = true;
        chk_stage_ChekedChanged(sender, e);//rajasekar
        
    }
    protected void chk_stage_ChekedChanged(object sender, EventArgs e)
    {
        int count = 0;
        stage_id1 = "";
        stage_id = "";
        stage_id_1 = "";
        chk_route.Checked = false;
        if (chk_stage.Checked == true)
        {

            for (int i = 0; i < chklst_stage.Items.Count; i++)
            {
                count++;
                chklst_stage.Items[i].Selected = true;
                txt_stage.Text = "Stage(" + count.ToString() + ")";
                if (stage_id == string.Empty)
                {
                    stage_id = "'" + chklst_stage.Items[i].Value + "'";

                }
                else
                {
                    stage_id = stage_id + "," + "'" + chklst_stage.Items[i].Value + "'";
                }
            }
            if (stage_id == "")//rajasekar
            {
                chk_stage.Checked = false;
                txt_stage.Text = "--Select District--";
            }
        }
        else
        {
            for (int i = 0; i < chklst_stage.Items.Count; i++)
            {

                chklst_stage.Items[i].Selected = false;
                txt_stage.Text = "--Select--";
            }
        }
        if (stage_id.ToString().Trim() != "")
        {
            stage_id1 = " r.stage_name in(" + stage_id + ")";
            stage_id = "and r.stage_name in(" + stage_id + ")";
            stage_id_1 = " and r.stage_name in(" + stage_id + ")";
            
        }
        load_route();
        chk_route.Checked = true;
        chk_route_ChekedChanged(sender, e);
    }
    protected void chklst_stage_SelectedIndexChanged(object sender, EventArgs e)
    {
        int count = 0;
        stage_id1 = "";
        stage_id = "";
        chk_stage.Checked = false;//rajasekar
        txt_stage.Text = "--Select Stage-- ";
        for (int i = 0; i < chklst_stage.Items.Count; i++)
        {
            if (chklst_stage.Items[i].Selected == true)
            {
                count++;

                txt_stage.Text = "Stage(" + count.ToString() + ")";
                if (stage_id == string.Empty)
                {
                    stage_id = "'" + chklst_stage.Items[i].Value + "'";
                }
                else
                {
                    stage_id = stage_id + "," + "'" + chklst_stage.Items[i].Value + "'";
                }
            }
           
        }
        
        if (stage_id.ToString().Trim() != "")
        {
            stage_id1 = " r.stage_name in(" + stage_id + ")";
            stage_id = " and r.stage_name in(" + stage_id + ")";
            stage_id_1 = " and r.stage_name in(" + stage_id + ")";
            
        }
        load_route();
            chk_route.Checked = true;
            chk_route_ChekedChanged(sender, e);//rajasekar
    }
    protected void chk_vehicle_ChekedChanged(object sender, EventArgs e)
    {
        int count = 0;
        veh_id = "";
        veh_id1 = "";
        if (chk_vehicle.Checked == true)
        {

            for (int i = 0; i < chklst_vehicle.Items.Count; i++)
            {
                count++;
                chklst_vehicle.Items[i].Selected = true;
                txt_vehicle.Text = "Vehicle(" + count.ToString() + ")";
                if (veh_id == string.Empty)
                {
                    veh_id = "'" + chklst_vehicle.Items[i].Value + "'";
                }
                else
                {
                    veh_id = veh_id + "," + "'" + chklst_vehicle.Items[i].Value + "'";
                }
                
            }
            if (route_id == "")//rajasekar
            {
                chk_vehicle.Checked = false;
                txt_vehicle.Text = "--Select Route--";
            }
        }
        else
        {
            for (int i = 0; i < chklst_vehicle.Items.Count; i++)
            {

                chklst_vehicle.Items[i].Selected = false;
                txt_vehicle.Text = "--Select--";
            }
        }
        if (veh_id.ToString().Trim() != "")
        {
            veh_id1 = " v.veh_id in(" + veh_id + ") ";
            veh_id = " and v.veh_id in(" + veh_id + ") ";
            veh_id_1 = " and v.veh_id in(" + veh_id + ") ";
        }
    }
    protected void chklst_vehicle_SelectedIndexChanged(object sender, EventArgs e)
    {
        int count = 0;
        veh_id = "";
        veh_id1 = "";
        chk_vehicle.Checked = false;//rajasekar
        txt_stage.Text = "--Select Vehicle-- ";
        for (int i = 0; i < chklst_vehicle.Items.Count; i++)
        {
            if (chklst_vehicle.Items[i].Selected == true)
            {
                count++;
                chklst_vehicle.Items[i].Selected = true;
                txt_vehicle.Text = "Vehicle(" + count.ToString() + ")";
                if (veh_id == string.Empty)
                {
                    veh_id = "'" + chklst_vehicle.Items[i].Value + "'";
                }
                else
                {
                    veh_id = veh_id + "," + "'" + chklst_vehicle.Items[i].Value + "'";
                }
            }
        }
        if (veh_id.ToString().Trim() != "")
        {
            veh_id1 = " v.veh_id in(" + veh_id + ") ";
            veh_id = " and v.veh_id in(" + veh_id + ") ";
            veh_id_1 = " and v.veh_id in(" + veh_id + ") ";
        }
    }
    protected void chk_route_ChekedChanged(object sender, EventArgs e)
    {
        int count = 0;
        route_id = "";
        route_id1 = "";
        if (chk_route.Checked == true)
        {

            for (int i = 0; i < chklst_route.Items.Count; i++)
            {
                count++;
                chklst_route.Items[i].Selected = true;
                txt_route.Text = "Route(" + count.ToString() + ")";
                if (route_id == string.Empty)
                {
                    route_id = "'" + chklst_route.Items[i].Value + "'";
                }
                else
                {
                    route_id = route_id + "," + "'" + chklst_route.Items[i].Value + "'";
                }
                
            }
            if (route_id == "")//rajasekar
            {
                chk_route.Checked = false;
                txt_route.Text = "--Select stage--";
            }
        }
        else
        {
            for (int i = 0; i < chklst_route.Items.Count; i++)
            {

                chklst_route.Items[i].Selected = false;
                txt_route.Text = "--Select--";
            }
        }
        if (route_id.ToString().Trim() != "")
        {
            route_id1 = "  r.route_id in(" + route_id + ") ";
            route_id = " and r.route_id in(" + route_id + ") ";//rajasekar
            route_id_1 = " and r.route_id in(" + route_id + ") ";
        }
        load_vehicle();
        chk_vehicle.Checked = true;
        chk_vehicle_ChekedChanged(sender, e);
    }
    protected void chklst_route_SelectedIndexChanged(object sender, EventArgs e)
    {
        int count = 0;
        route_id = "";
        route_id1 = "";
        chk_route.Checked = false;//rajasekar
        txt_route.Text = "--Select Route-- ";
        for (int i = 0; i < chklst_route.Items.Count; i++)
        {
            if (chklst_route.Items[i].Selected == true)
            {
                count++;
                chklst_route.Items[i].Selected = true;
                txt_route.Text = "Route(" + count.ToString() + ")";
                if (route_id == string.Empty)
                {
                    route_id = "'" + chklst_route.Items[i].Value + "'";
                }
                else
                {
                    route_id = route_id + "," + "'" + chklst_route.Items[i].Value + "'";
                }
            }
            
        }
        if (route_id.ToString().Trim() != "")
        {
            route_id1 = " r.route_id in(" + route_id + ") ";
            route_id = "and r.route_id in(" + route_id + ") ";
            route_id_1 = " and r.route_id in(" + route_id + ") ";
           
        }
        load_vehicle();
        chk_vehicle.Checked = true;
        chk_vehicle_ChekedChanged(sender, e);//rajasekar
    }
    protected void chk_college_ChekedChanged(object sender, EventArgs e)
    {

        count = 0;
        selected_college = "";
        college_code = "";
        college_code_1 = "";
        if (chk_college.Checked == true)
        {
            for (int i = 0; i < chklst_college.Items.Count; i++)
            {
                count++;
                chklst_college.Items[i].Selected = true;
                txt_college.Text = lblcollege.Text + "(" + count.ToString() + ")";
                if (selected_college == "")
                {
                    selected_college = chklst_college.Items[i].Value.ToString();
                    college_code = chklst_college.Items[i].Value;
                    college_code_1 = chklst_college.Items[i].Value;
                }
                else
                {
                    selected_college = selected_college + "," + chklst_college.Items[i].Value.ToString();
                    college_code = college_code + "," + chklst_college.Items[i].Value;
                    college_code_1 = college_code_1 + "," + chklst_college.Items[i].Value;
                }

            }


        }
        else if (chk_college.Checked == false)
        {
            txt_college.Text = "";
            for (int i = 0; i < chklst_college.Items.Count; i++)
            {
                txt_college.Text = "--Select--";
                chklst_college.Items[i].Selected = false;
            }
        }

        if (college_code != "")
        {
            college_code = " and rg.college_code in(" + college_code + ") ";
        }

        load_batch();
        chk_batch.Checked = true;
        chk_batch_CheckedChanged(sender, e);
        load_degree();
        chk_degree.Checked = true;
        chk_degree_CheckedChanged(sender, e);
    }
    protected void chklst_college_SelectedIndexChanged(object sender, EventArgs e)
    {

        selected_college = "";
        count = 0;
        college_code = "";
        college_code_1 = "";
        for (int i = 0; i < chklst_college.Items.Count; i++)
        {
            if (chklst_college.Items[i].Selected == true)
            {
                count++;
                chklst_college.Items[i].Selected = true;
                txt_college.Text = lblcollege.Text + "(" + count.ToString() + ")";
                if (selected_college == "")
                {
                    selected_college = chklst_college.Items[i].Value.ToString();
                    college_code = chklst_college.Items[i].Value;
                    college_code_1 = chklst_college.Items[i].Value;
                }
                else
                {
                    selected_college = selected_college + "," + chklst_college.Items[i].Value.ToString();
                    college_code = college_code + "," + chklst_college.Items[i].Value;
                    college_code_1 = college_code_1 + "," + chklst_college.Items[i].Value;
                }
            }
        }
        if (college_code != "")
        {
            college_code = " and rg.college_code in(" + college_code + ") ";
        }
        load_batch();
        chk_batch.Checked = true;
        chk_batch_CheckedChanged(sender, e);
        load_degree();
        chk_degree.Checked = true;
        chk_degree_CheckedChanged(sender, e);
    }
    protected void GO_Click(object sender, EventArgs e)
    {
        try
        {
            Hashtable hat_1 = new Hashtable();
            DataSet ds = new DataSet();
            int snoo = 1;
            int total = 0;

            Fp_Individual_Strength.Visible = false;
            Fp_Individual_Strength.Sheets[0].Visible = false;
            fp_stud.Visible = false;
            fp_stud.Sheets[0].Visible = false;
            Fp_strength.Visible = false;
            Fp_strength.Sheets[0].Visible = false;
            lblerrmsg.Visible = false;
            btnprint.Visible = false;
            btnprintmaster.Visible = false;

            FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();


            if (txt_student.Text.ToString() != "")
            {
                fp_stud.Sheets[0].RowCount = 0;
                if (drp_studstaff.SelectedItem.Text == "Student")//Student Click
                {
                    hat_1.Clear();
                    hat_1.Add("STUD_FILTER", " and rg.roll_no='" + txt_student.Text.ToString() + "'");
                    hat_1.Add("STAFF_FILTER", " and rg.staff_code='" + txt_student.Text.ToString() + "'");
                    ds.Clear();
                    ds.Reset();
                    ds.Dispose();
                    ds = DataAccess.select_method("TRANSPORT_INDIVIDUAL_STUDSTAFF", hat_1, "sp");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        bind_studheader();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            fp_stud.Sheets[0].RowCount++;
                            int rowcnt = fp_stud.Sheets[0].RowCount - 1;
                            fp_stud.Sheets[0].Cells[rowcnt, 0].Text = Convert.ToString(snoo);
                            fp_stud.Sheets[0].Cells[rowcnt, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["coll_acronymn"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["batch_year"]) + "-" + Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + "[" + Convert.ToString(ds.Tables[0].Rows[i]["acronym"]) + "]";
                            fp_stud.Sheets[0].Cells[rowcnt, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["vehid"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Bus_RouteID"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["stage_name"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["roll_no"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["reg_no"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 7].CellType = textcel_type;
                            fp_stud.Sheets[0].Cells[rowcnt, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["stud_name"]);
                            snoo++;
                        }
                        fp_stud.Height = 40 + (fp_stud.Sheets[0].RowCount * 30);
                        fp_stud.Sheets[0].PageSize = fp_stud.Sheets[0].RowCount;

                        Fp_Individual_Strength.Visible = false;
                        Fp_Individual_Strength.Sheets[0].Visible = false;
                        fp_stud.Visible = true;
                        fp_stud.Sheets[0].Visible = true;
                        Fp_strength.Visible = false;
                        Fp_strength.Sheets[0].Visible = false;
                    }
                    chk = false;
                    if (fp_stud.Sheets[0].RowCount == 0)
                    {
                        Fp_Individual_Strength.Visible = false;
                        Fp_Individual_Strength.Sheets[0].Visible = false;
                        fp_stud.Visible = false;
                        fp_stud.Sheets[0].Visible = false;
                        Fp_strength.Visible = false;
                        Fp_strength.Sheets[0].Visible = false;

                        lblerrmsg.Text = "No Records Found";
                        lblerrmsg.Visible = true;
                        btnprint.Visible = false;
                    }
                    else
                    {
                        btnprint.Visible = true;
                    }
                    return;
                }
                if (drp_studstaff.SelectedItem.Text == "Staff")//Staff Click
                {

                    hat_1.Clear();
                    hat_1.Add("STAFF_FILTER", " and rg.staff_code='" + txt_student.Text.ToString() + "'");
                    hat_1.Add("STUD_FILTER", " and rg.roll_no='" + txt_student.Text.ToString() + "'");
                    ds.Clear();
                    ds.Reset();
                    ds.Dispose();
                    ds = DataAccess.select_method("TRANSPORT_INDIVIDUAL_STUDSTAFF", hat_1, "sp");
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        bind_staffheader();
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            fp_stud.Sheets[0].RowCount++;
                            int rowcnt = fp_stud.Sheets[0].RowCount - 1;
                            fp_stud.Sheets[0].Cells[rowcnt, 0].Text = Convert.ToString(snoo);
                            fp_stud.Sheets[0].Cells[rowcnt, 1].Text = Convert.ToString(ds.Tables[1].Rows[i]["coll_acronymn"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 2].Text = Convert.ToString(ds.Tables[1].Rows[i]["dept_name"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 3].Text = Convert.ToString(ds.Tables[1].Rows[i]["desig_name"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 4].Text = Convert.ToString(ds.Tables[1].Rows[i]["vehid"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 5].Text = Convert.ToString(ds.Tables[1].Rows[i]["Bus_RouteID"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 6].Text = Convert.ToString(ds.Tables[1].Rows[i]["stage_name"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 7].Text = Convert.ToString(ds.Tables[1].Rows[i]["Staff_code"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 8].Text = Convert.ToString(ds.Tables[1].Rows[i]["Staff_name"]);
                            snoo++;
                        }
                        fp_stud.Height = 40 + (fp_stud.Sheets[0].RowCount * 30);
                        fp_stud.Sheets[0].PageSize = fp_stud.Sheets[0].RowCount;
                        Fp_Individual_Strength.Visible = false;
                        Fp_Individual_Strength.Sheets[0].Visible = false;
                        fp_stud.Visible = true;
                        fp_stud.Sheets[0].Visible = true;
                        Fp_strength.Visible = false;
                        Fp_strength.Sheets[0].Visible = false;
                    }
                    chk = false;
                    if (fp_stud.Sheets[0].RowCount == 0)
                    {
                        Fp_Individual_Strength.Visible = false;
                        Fp_Individual_Strength.Sheets[0].Visible = false;
                        fp_stud.Visible = false;
                        fp_stud.Sheets[0].Visible = false;
                        Fp_strength.Visible = false;
                        Fp_strength.Sheets[0].Visible = false;
                        lblerrmsg.Text = "No Records Found";
                        lblerrmsg.Visible = true;
                        btnprint.Visible = false;
                    }
                    else
                    {
                        btnprint.Visible = true;
                    }
                    return;
                }
            }

            string dist_value = string.Empty;
            string veh_tag = string.Empty;
            string route_tag = string.Empty;
            string stage_tag = string.Empty;
            string class_var = "", deptcode = "";
            string studmale_tag = "", studfemale_tag = "", staffmale_tag = "", stafffemale_tag = "", studmale_tagbatch = "", studfemale_tagbatch = "";
            int sno = 0;

            Hashtable hat = new Hashtable();


            Fp_strength.Sheets[0].ColumnHeader.RowCount = 0;
            Fp_strength.Sheets[0].RowCount = 0;
            Fp_strength.Sheets[0].ColumnCount = 0;

            Fp_strength.Sheets[0].ColumnHeader.RowCount = 2;
            Fp_strength.Sheets[0].ColumnCount = 14;



            Fp_strength.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fp_strength.Sheets[0].ColumnHeader.Cells[0, 1].Text = "District";
            Fp_strength.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Vehicle";
            Fp_strength.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Route";
            Fp_strength.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Stages";
            Fp_strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            Fp_strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            Fp_strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            Fp_strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            Fp_strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

            Fp_strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 1, 3);
            Fp_strength.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Student";
            Fp_strength.Sheets[0].ColumnHeader.Cells[1, 5].Text = "M";
            Fp_strength.Sheets[0].ColumnHeader.Cells[1, 6].Text = "F";
            Fp_strength.Sheets[0].ColumnHeader.Cells[1, 7].Text = "TOT";

            Fp_strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 1, 3);
            Fp_strength.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Staff";
            Fp_strength.Sheets[0].ColumnHeader.Cells[1, 8].Text = "M";
            Fp_strength.Sheets[0].ColumnHeader.Cells[1, 9].Text = "F";
            Fp_strength.Sheets[0].ColumnHeader.Cells[1, 10].Text = "TOT";

            Fp_strength.Sheets[0].Columns[5].Visible = true;
            Fp_strength.Sheets[0].Columns[6].Visible = true;
            Fp_strength.Sheets[0].Columns[7].Visible = true;
            Fp_strength.Sheets[0].Columns[8].Visible = true;
            Fp_strength.Sheets[0].Columns[9].Visible = true;
            Fp_strength.Sheets[0].Columns[10].Visible = true;

            if (ddl_sex.Items[0].Selected == true && ddl_sex.Items[1].Selected == false)
            {
                Fp_strength.Sheets[0].Columns[5].Visible = true;
                Fp_strength.Sheets[0].Columns[6].Visible = false;
                Fp_strength.Sheets[0].Columns[7].Visible = false;

                Fp_strength.Sheets[0].Columns[8].Visible = true;
                Fp_strength.Sheets[0].Columns[9].Visible = false;
                Fp_strength.Sheets[0].Columns[10].Visible = false;
            }
            else if (ddl_sex.Items[0].Selected == false && ddl_sex.Items[1].Selected == true)
            {
                Fp_strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 1, 2);
                Fp_strength.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Student";
                Fp_strength.Sheets[0].Columns[5].Visible = false;
                Fp_strength.Sheets[0].Columns[6].Visible = true;
                Fp_strength.Sheets[0].Columns[7].Visible = false;

                Fp_strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 1, 2);
                Fp_strength.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Staff";
                Fp_strength.Sheets[0].Columns[8].Visible = false;
                Fp_strength.Sheets[0].Columns[9].Visible = true;
                Fp_strength.Sheets[0].Columns[10].Visible = false;

            }



            Fp_strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 11, 2, 1);
            Fp_strength.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Allotted";

            Fp_strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 12, 2, 1);
            Fp_strength.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Admitted";

            Fp_strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 13, 2, 1);
            Fp_strength.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Vacancy";


            Fp_strength.Sheets[0].Columns[0].Width = 50;
            Fp_strength.Sheets[0].Columns[1].Width = 200;
            Fp_strength.Sheets[0].Columns[2].Width = 70;
            Fp_strength.Sheets[0].Columns[3].Width = 70;
            Fp_strength.Sheets[0].Columns[4].Width = 70;
            Fp_strength.Sheets[0].Columns[5].Width = 70;
            Fp_strength.Sheets[0].Columns[6].Width = 70;
            Fp_strength.Sheets[0].Columns[7].Width = 70;
            Fp_strength.Sheets[0].Columns[8].Width = 70;
            Fp_strength.Sheets[0].Columns[9].Width = 70;
            Fp_strength.Sheets[0].Columns[10].Width = 70;
            Fp_strength.Sheets[0].Columns[11].Width = 70;

            Fp_strength.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            Fp_strength.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            Fp_strength.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            Fp_strength.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            Fp_strength.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            Fp_strength.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            Fp_strength.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            Fp_strength.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
            Fp_strength.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
            Fp_strength.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
            Fp_strength.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Center;
            Fp_strength.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Center;
            Fp_strength.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Center;
            Fp_strength.Sheets[0].Columns[13].HorizontalAlign = HorizontalAlign.Center;

            if (college_code == string.Empty)
            {
                Fp_strength.Visible = false; lblerrmsg.Text = "No Records Found"; lblerrmsg.Visible = true; return;
            }


            if (selected_batch.ToString() != "" && selected_courseid.ToString() != "" && selected_depid.ToString() != "")
            {
                class_var = " and d.course_id in(" + selected_courseid.ToString() + ") and d.dept_code in(" + selected_depid.ToString() + ") and rg.batch_year in(" + selected_batch.ToString() + ")";
                deptcode = " and ap.dept_code in(" + selected_depid.ToString() + ")";
            }


            for (int i = 0; i < chklst_district.Items.Count; i++)
            {
                if (chklst_district.Items[i].Selected == true)
                {
                    hat.Clear();
                    dist_value = " and s.district in(" + chklst_district.Items[i].Value + ")";
                    hat.Add("COLLEGECODE", college_code);
                    hat.Add("DISTRICT", dist_value);
                    hat.Add("STAGE", stage_id);
                    hat.Add("VEHICLE", veh_id);
                    hat.Add("ROUTE", route_id);
                    hat.Add("Stud_filter", class_var);
                    hat.Add("Staff_filter", deptcode);

                    ds = DataAccess.select_method("TRANSPORT_STRENGTH_REPORT", hat, "sp");

                    if (ds.Tables[3].Rows.Count > 0 || ds.Tables[4].Rows.Count > 0 || ds.Tables[5].Rows.Count > 0 || ds.Tables[6].Rows.Count > 0)
                    {
                        long noftravrs = 0;
                        studmale_tag = "";
                        studfemale_tag = "";
                        staffmale_tag = "";
                        stafffemale_tag = "";
                        studmale_tagbatch = "";
                        studfemale_tagbatch = "";
                        veh_tag = "";
                        route_tag = "";
                        stage_tag = "";

                        for (int veh = 0; veh < ds.Tables[0].Rows.Count; veh++)
                        {
                            if (veh_tag == string.Empty) { veh_tag = ds.Tables[0].Rows[veh][0].ToString(); }
                            else { veh_tag = veh_tag + "','" + ds.Tables[0].Rows[veh][0].ToString(); }
                            noftravrs = noftravrs + Convert.ToInt16(ds.Tables[0].Rows[count]["noftravrs"].ToString());
                        }
                        for (int route = 0; route < ds.Tables[1].Rows.Count; route++)
                        {
                            if (route_tag == string.Empty) { route_tag = ds.Tables[1].Rows[route][0].ToString(); }
                            else { route_tag = route_tag + "','" + ds.Tables[1].Rows[route][0].ToString(); }
                        }
                        for (int stage = 0; stage < ds.Tables[2].Rows.Count; stage++)
                        {
                            if (stage_tag == string.Empty) { stage_tag = ds.Tables[2].Rows[stage][0].ToString(); }
                            else { stage_tag = stage_tag + "','" + ds.Tables[2].Rows[stage][0].ToString(); }
                        }

                        for (int studmale = 0; studmale < ds.Tables[3].Rows.Count; studmale++)
                        {
                            if (studmale_tag == string.Empty) { studmale_tag = ds.Tables[3].Rows[studmale]["degree_code"].ToString(); }
                            else { studmale_tag = studmale_tag + "," + ds.Tables[3].Rows[studmale]["degree_code"].ToString(); }
                            if (studmale_tagbatch == string.Empty) { studmale_tagbatch = ds.Tables[3].Rows[studmale]["batch_year"].ToString(); }
                            else { studmale_tagbatch = studmale_tagbatch + "," + ds.Tables[3].Rows[studmale]["batch_year"].ToString(); }
                        }

                        for (int studfemale = 0; studfemale < ds.Tables[4].Rows.Count; studfemale++)
                        {
                            if (studfemale_tag == string.Empty) { studfemale_tag = ds.Tables[4].Rows[studfemale]["degree_code"].ToString(); }
                            else { studfemale_tag = studfemale_tag + "," + ds.Tables[4].Rows[studfemale]["degree_code"].ToString(); }
                            if (studfemale_tagbatch == string.Empty) { studfemale_tagbatch = ds.Tables[4].Rows[studfemale]["batch_year"].ToString(); }
                            else { studfemale_tagbatch = studfemale_tagbatch + "," + ds.Tables[4].Rows[studfemale]["batch_year"].ToString(); }
                        }


                        for (int staffmale = 0; staffmale < ds.Tables[5].Rows.Count; staffmale++)
                        {
                            if (staffmale_tag == string.Empty) { staffmale_tag = ds.Tables[5].Rows[staffmale]["dept_code"].ToString(); }
                            else { staffmale_tag = staffmale_tag + "," + ds.Tables[5].Rows[staffmale]["dept_code"].ToString(); }
                        }

                        for (int stafffemale = 0; stafffemale < ds.Tables[6].Rows.Count; stafffemale++)
                        {
                            if (stafffemale_tag == string.Empty) { stafffemale_tag = ds.Tables[6].Rows[stafffemale]["dept_code"].ToString(); }
                            else { stafffemale_tag = stafffemale_tag + "," + ds.Tables[6].Rows[stafffemale]["dept_code"].ToString(); }
                        }
                        long nnoftravrs_all = 0;
                        nnoftravrs_all = noftravrs;
                        noftravrs = noftravrs - Convert.ToInt16(ds.Tables[3].Rows.Count) - Convert.ToInt16(ds.Tables[4].Rows.Count) - Convert.ToInt16(ds.Tables[5].Rows.Count) - Convert.ToInt16(ds.Tables[6].Rows.Count);
                        total = Convert.ToInt16(ds.Tables[3].Rows.Count) + Convert.ToInt16(ds.Tables[4].Rows.Count) + Convert.ToInt16(ds.Tables[5].Rows.Count) + Convert.ToInt16(ds.Tables[6].Rows.Count);
                        if (noftravrs < 0)
                        {
                            noftravrs = 0;
                        }
                        sno++;
                        Fp_strength.Sheets[0].RowCount++;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 1].Text = chklst_district.Items[i].ToString();
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 1].Tag = chklst_district.Items[i].Value;

                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows.Count);
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 2].Tag = veh_tag;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 2].ForeColor = Color.Blue;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 2].Font.Underline = true;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[1].Rows.Count);
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 3].Tag = route_tag;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 3].ForeColor = Color.Blue;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 3].Font.Underline = true;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[2].Rows.Count);
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 4].Tag = stage_tag;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 4].ForeColor = Color.Blue;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 4].Font.Underline = true;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[3].Rows.Count);
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(studmale_tag);
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 5].Note = Convert.ToString(studmale_tagbatch);
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 5].ForeColor = Color.Blue;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 5].Font.Underline = true;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[4].Rows.Count);
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(studfemale_tag);
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 6].Note = Convert.ToString(studfemale_tagbatch);
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 6].ForeColor = Color.Blue;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 6].Font.Underline = true;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(Convert.ToInt16(ds.Tables[3].Rows.Count) + Convert.ToInt16(ds.Tables[4].Rows.Count));
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 7].ForeColor = Color.Blue;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 7].Font.Underline = true;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[5].Rows.Count);
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 8].Tag = Convert.ToString(staffmale_tag);
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 8].ForeColor = Color.Blue;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 8].Font.Underline = true;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[6].Rows.Count);
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 9].Tag = Convert.ToString(staffmale_tag);
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 9].ForeColor = Color.Blue;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 9].Font.Underline = true;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(Convert.ToInt16(ds.Tables[5].Rows.Count) + Convert.ToInt16(ds.Tables[6].Rows.Count));
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 10].ForeColor = Color.Blue;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 10].Font.Underline = true;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(nnoftravrs_all);
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(total);
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 13].Text = Convert.ToString(noftravrs);

                    }
                }
            }

            if (Fp_strength.Sheets[0].RowCount > 0)
            {

                Fp_strength.Sheets[0].RowCount++;
                Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 1].Text = "TOTAL";
                Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Right;
                Fp_strength.Sheets[0].Rows[Fp_strength.Sheets[0].RowCount - 1].Font.Bold = true;

                for (int col = 2; col < Fp_strength.Sheets[0].ColumnCount; col++)
                {
                    int tot_count = 0;
                    for (int row = 0; row < Fp_strength.Sheets[0].RowCount - 1; row++)
                    {
                        if (Convert.ToString(Fp_strength.Sheets[0].Cells[row, col].Text.ToString()) != "")
                        {
                            tot_count = tot_count + Convert.ToInt32(Fp_strength.Sheets[0].Cells[row, col].Text.ToString());
                        }
                    }

                    Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, col].Text = tot_count.ToString();
                    Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, col].Font.Bold = true;
                    if (col != Fp_strength.Sheets[0].ColumnCount - 1 && col != Fp_strength.Sheets[0].ColumnCount - 2 && col != Fp_strength.Sheets[0].ColumnCount - 3)
                    {
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, col].ForeColor = Color.Blue;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, col].Font.Underline = true;
                        Fp_strength.Sheets[0].Cells[Fp_strength.Sheets[0].RowCount - 1, col].Font.Bold = true;

                    }
                }

                Fp_strength.Height = 40 + (Fp_strength.Sheets[0].RowCount * 30);
                Fp_strength.Sheets[0].PageSize = Fp_strength.Sheets[0].RowCount;
                Fp_strength.Width = 620;
                lblerrmsg.Visible = false;
                Fp_Individual_Strength.Visible = false;
                Fp_Individual_Strength.Sheets[0].Visible = false;
                fp_stud.Visible = false;
                fp_stud.Sheets[0].Visible = false;
                Fp_strength.Visible = true;
                Fp_strength.Sheets[0].Visible = true;
                btnprintmaster.Visible = true;
            }
            else
            {
                Fp_Individual_Strength.Visible = false;
                Fp_Individual_Strength.Sheets[0].Visible = false;
                fp_stud.Visible = false;
                fp_stud.Sheets[0].Visible = false;
                Fp_strength.Visible = false;
                Fp_strength.Sheets[0].Visible = false;
                lblerrmsg.Text = "No Records Found";
                lblerrmsg.Visible = true;
                btnprintmaster.Visible = false;
            }

        }
        catch
        {
        }
    }

    protected void Fp_strength_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        chk = true;
    }
    protected void Fp_strength_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {

            FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
            if (chk == true)
            {
                //System.Threading.Thread.Sleep(2000);
                btnprint.Visible = false;
                lblerrmsg.Visible = false;
                //string college_code_1 = string.Empty;
                string dist_code_1 = string.Empty;
                string veh_id_1 = string.Empty;
                string route_id_1 = string.Empty;
                string stage_id_1 = string.Empty;
                string class_var = "", deptcode = "";
                string studmale_tag = "", studfemale_tag = "", staffmale_tag = "", stafffemale_tag = "", studmale_tagbatch = "", studfemale_tagbatch = "";
                string studbothbatch = "", studbothdegree = "", staffboth = "";
                int sno = 1;
                int temp = 0;

                DataSet ds = new DataSet();
                DataSet ds_1 = new DataSet();

                Hashtable hat = new Hashtable();
                Hashtable hat_1 = new Hashtable();

                if (selected_batch.ToString() != "" && selected_courseid.ToString() != "" && selected_depid.ToString() != "")
                {
                    class_var = " and d.course_id in(" + selected_courseid.ToString() + ") and d.dept_code in(" + selected_depid.ToString() + ") and rg.batch_year in(" + selected_batch.ToString() + ")";
                    deptcode = " and ap.dept_code in(" + selected_depid.ToString() + ")";
                }

                ar = Fp_strength.Sheets[0].ActiveRow;
                ac = Fp_strength.Sheets[0].ActiveColumn;


                if (ar != Fp_strength.Sheets[0].RowCount - 1)//Click Row from 1 to rowcount-1
                {

                    dist_code_1 = "" + Fp_strength.Sheets[0].Cells[ar, 1].Tag.ToString() + "";
                    veh_id_1 = "'" + Fp_strength.Sheets[0].Cells[ar, 2].Tag.ToString() + "'";
                    route_id_1 = "'" + Fp_strength.Sheets[0].Cells[ar, 3].Tag.ToString() + "'";
                    stage_id_1 = "'" + Fp_strength.Sheets[0].Cells[ar, 4].Tag.ToString() + "'";

                    if (Convert.ToString(Fp_strength.Sheets[0].Cells[ar, 5].Tag) != "")
                    {
                        studmale_tag = " and rg.degree_code in(" + Fp_strength.Sheets[0].Cells[ar, 5].Tag.ToString() + ") ";
                        studbothdegree = Fp_strength.Sheets[0].Cells[ar, 5].Tag.ToString();
                    }
                    if (Convert.ToString(Fp_strength.Sheets[0].Cells[ar, 6].Tag) != "")
                    {
                        studfemale_tag = " and rg.degree_code in(" + Fp_strength.Sheets[0].Cells[ar, 6].Tag.ToString() + ") ";
                        if (studbothdegree == "")
                        {
                            studbothdegree = Fp_strength.Sheets[0].Cells[ar, 6].Tag.ToString();
                        }
                        else
                        {
                            studbothdegree = studbothdegree + "," + Fp_strength.Sheets[0].Cells[ar, 6].Tag.ToString();
                        }
                    }
                    if (Convert.ToString(Fp_strength.Sheets[0].Cells[ar, 5].Note) != "")
                    {
                        studmale_tagbatch = " and rg.batch_year in(" + Fp_strength.Sheets[0].Cells[ar, 5].Note.ToString() + ") ";
                        studbothbatch = Fp_strength.Sheets[0].Cells[ar, 5].Note.ToString();
                    }
                    if (Convert.ToString(Fp_strength.Sheets[0].Cells[ar, 6].Note) != "")
                    {
                        studfemale_tagbatch = " and rg.batch_year in(" + Fp_strength.Sheets[0].Cells[ar, 6].Note.ToString() + ") ";
                        if (studbothbatch == "")
                        {
                            studbothbatch = Fp_strength.Sheets[0].Cells[ar, 6].Note.ToString();
                        }
                        else
                        {
                            studbothbatch = studbothbatch + "," + Fp_strength.Sheets[0].Cells[ar, 6].Note.ToString();
                        }
                    }

                    if (Convert.ToString(Fp_strength.Sheets[0].Cells[ar, 8].Tag) != "")
                    {
                        staffmale_tag = " and st.dept_code in(" + Fp_strength.Sheets[0].Cells[ar, 8].Tag.ToString() + ") ";
                        staffboth = Convert.ToString(Fp_strength.Sheets[0].Cells[ar, 8].Tag);
                    }
                    if (Convert.ToString(Fp_strength.Sheets[0].Cells[ar, 9].Tag) != "")
                    {
                        stafffemale_tag = " and st.dept_code in(" + Fp_strength.Sheets[0].Cells[ar, 9].Tag.ToString() + ") ";
                        if (staffboth == "")
                        {
                            staffboth = Convert.ToString(Fp_strength.Sheets[0].Cells[ar, 9].Tag);
                        }
                        else
                        {
                            staffboth = staffboth + "," + Convert.ToString(Fp_strength.Sheets[0].Cells[ar, 9].Tag);
                        }
                    }
                    string college_code_11 = "";//rajasekar 23/4
                    if (ac == 7)//rajasekar 23/4
                    {
                        
                        chklst_college_SelectedIndexChanged(sender, e);
                        college_code_11 = " and rg.college_code in(" + college_code_1 + ") ";//rajasekar 23/4
                    }
                    dist_code_1 = " and s.district in(" + dist_code_1 + ") ";
                    veh_id_1 = " and v.veh_id in(" + veh_id_1 + ") ";
                    route_id_1 = " and r.route_id in(" + route_id_1 + ") ";
                    stage_id_1 = " and r.stage_name in(" + stage_id_1 + ") ";

                    hat.Clear();
                    if(ac==7)
                        hat.Add("COLLEGECODE", college_code_11);
                    else
                        hat.Add("COLLEGECODE", college_code_1);
                    hat.Add("DISTRICT", dist_code_1);
                    hat.Add("STAGE", stage_id_1);
                    hat.Add("VEHICLE", veh_id_1);
                    hat.Add("ROUTE", route_id_1);

                    if (ac == 4)
                    {
                        hat.Add("TYPE", "Stage");
                    }
                    else if (ac == 3)
                    {
                        hat.Add("TYPE", "Route");
                    }
                    else if (ac == 2)
                    {
                        hat.Add("TYPE", "Vehicle");
                    }

                    if (ac >= 5) //Student or Staff Click 
                    {
                        
                        hat_1.Clear();
                        hat_1.Add("COLLEGECODE", college_code_11);
                        hat_1.Add("DISTRICT", dist_code_1);
                        hat_1.Add("STAGE", stage_id_1);
                        hat_1.Add("VEHICLE", veh_id_1);
                        hat_1.Add("ROUTE", route_id_1);

                        if (ac == 5)
                        {
                            string malestr = " and a.sex=0 " + studmale_tag + studmale_tagbatch;
                            hat_1.Add("STUD_FILTER", malestr);
                            hat_1.Add("STAFF_FILTER", "");
                        }
                        else if (ac == 6)
                        {
                            string Femalestr = " and a.sex=1 " + studfemale_tag + studfemale_tagbatch;
                            hat_1.Add("STUD_FILTER", Femalestr);
                            hat_1.Add("STAFF_FILTER", "");
                        }
                        else if (ac == 7)
                        {
                            if (studbothbatch != "")
                            {
                                studbothbatch = " and rg.batch_year in(" + studbothbatch + ")";
                            }
                            if (studbothdegree != "")
                            {
                                studbothdegree = " and rg.degree_code in(" + studbothdegree + ")";
                            }
                            string both = " and (a.sex=0 or a.sex=1) " + studbothbatch + studbothdegree;
                            hat_1.Add("STUD_FILTER", both);
                            hat_1.Add("STAFF_FILTER", "");
                        }
                        else if (ac == 8)
                        {
                            string malestr = " and ap.sex='Male' " + staffmale_tag;
                            hat_1.Add("STAFF_FILTER", malestr);
                            hat_1.Add("STUD_FILTER", "");
                        }
                        else if (ac == 9)
                        {
                            string malestr = " and ap.sex='Female' " + stafffemale_tag;
                            hat_1.Add("STAFF_FILTER", malestr);
                            hat_1.Add("STUD_FILTER", "");
                        }
                        else if (ac == 10)
                        {
                            if (staffboth != "")
                            {
                                staffboth = " and st.dept_code in(" + staffboth + ")";
                            }

                            string malestr = " and (ap.sex='Female' or ap.sex='Male') " + staffboth;
                            hat_1.Add("STAFF_FILTER", malestr);
                            hat_1.Add("STUD_FILTER", "");
                        }



                    }
                }

                else if (ar == Fp_strength.Sheets[0].RowCount - 1) //Click Total Row
                {
                    hat.Clear();
                    hat.Add("COLLEGECODE", college_code);
                    hat.Add("DISTRICT", dist_code);
                    hat.Add("STAGE", stage_id);
                    hat.Add("VEHICLE", veh_id);
                    hat.Add("ROUTE", route_id);

                    if (ac == 4)
                    {
                        hat.Add("TYPE", "Stage");
                    }
                    else if (ac == 3)
                    {
                        hat.Add("TYPE", "Route");
                    }
                    else if (ac == 2)
                    {
                        hat.Add("TYPE", "Vehicle");
                    }

                    if (ac >= 5) //Student or Staff Total Row Click
                    {
                        hat_1.Clear();
                        hat_1.Add("COLLEGECODE", college_code);
                        hat_1.Add("DISTRICT", dist_code);
                        hat_1.Add("STAGE", stage_id);
                        hat_1.Add("VEHICLE", veh_id);
                        hat_1.Add("ROUTE", route_id);

                        if (ac == 5)
                        {
                            string malestr = " and a.sex=0 " + class_var;
                            hat_1.Add("STUD_FILTER", malestr);
                            hat_1.Add("STAFF_FILTER", "");
                        }
                        else if (ac == 6)
                        {
                            string malestr = " and a.sex=1 " + class_var;
                            hat_1.Add("STUD_FILTER", malestr);
                            hat_1.Add("STAFF_FILTER", "");
                        }
                        else if (ac == 7)
                        {
                            string malestr = " and (a.sex=0 or a.sex=1) " + class_var;
                            hat_1.Add("STUD_FILTER", malestr);
                            hat_1.Add("STAFF_FILTER", "");
                        }
                        else if (ac == 8)
                        {
                            string malestr = " and ap.sex='Male' " + deptcode;
                            hat_1.Add("STAFF_FILTER", malestr);
                            hat_1.Add("STUD_FILTER", "");
                        }
                        else if (ac == 9)
                        {
                            string malestr = " and ap.sex='Female' " + deptcode;
                            hat_1.Add("STAFF_FILTER", malestr);
                            hat_1.Add("STUD_FILTER", "");
                        }
                        else if (ac == 10)
                        {
                            string malestr = " and (ap.sex='Male' or ap.sex='Female') " + deptcode;
                            hat_1.Add("STAFF_FILTER", malestr);
                            hat_1.Add("STUD_FILTER", "");
                        }
                    }
                }

                if ((ac == 5) || (ac == 6) || (ac == 7))//Student Click
                {
                    ds = DataAccess.select_method("TRANSPORT_STRENGTH_REPORT_STUDENT", hat_1, "sp");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        bind_studheader();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            fp_stud.Sheets[0].RowCount++;
                            int rowcnt = fp_stud.Sheets[0].RowCount - 1;
                            fp_stud.Sheets[0].Cells[rowcnt, 0].Text = Convert.ToString(sno);
                            fp_stud.Sheets[0].Cells[rowcnt, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["coll_acronymn"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["batch_year"]) + "-" + Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + "[" + Convert.ToString(ds.Tables[0].Rows[i]["acronym"]) + "]";
                            fp_stud.Sheets[0].Cells[rowcnt, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["vehid"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Bus_RouteID"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["stage_name"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["roll_no"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["reg_no"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 7].CellType = textcel_type;
                            fp_stud.Sheets[0].Cells[rowcnt, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["stud_name"]);
                            sno++;
                        }
                        //fp_stud.Height = 40 + (fp_stud.Sheets[0].RowCount * 10); 
                        fp_stud.Sheets[0].PageSize = fp_stud.Sheets[0].RowCount;
                        fp_stud.Visible = true;
                        fp_stud.Sheets[0].Visible = true;
                        Fp_Individual_Strength.Visible = false;
                        Fp_Individual_Strength.Sheets[0].Visible = false;
                        btnprint.Visible = true;
                    }
                    chk = false;
                    return;
                }
                if ((ac == 8) || (ac == 9) || (ac == 10))//Staff Click
                {
                    ds = DataAccess.select_method("TRANSPORT_STRENGTH_REPORT_STUDENT", hat_1, "sp");
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        bind_staffheader();
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            fp_stud.Sheets[0].RowCount++;
                            int rowcnt = fp_stud.Sheets[0].RowCount - 1;
                            fp_stud.Sheets[0].Cells[rowcnt, 0].Text = Convert.ToString(sno);
                            fp_stud.Sheets[0].Cells[rowcnt, 1].Text = Convert.ToString(ds.Tables[1].Rows[i]["coll_acronymn"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 2].Text = Convert.ToString(ds.Tables[1].Rows[i]["dept_name"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 3].Text = Convert.ToString(ds.Tables[1].Rows[i]["desig_name"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 4].Text = Convert.ToString(ds.Tables[1].Rows[i]["vehid"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 5].Text = Convert.ToString(ds.Tables[1].Rows[i]["Bus_RouteID"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 6].Text = Convert.ToString(ds.Tables[1].Rows[i]["stage_name"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 7].Text = Convert.ToString(ds.Tables[1].Rows[i]["Staff_code"]);
                            fp_stud.Sheets[0].Cells[rowcnt, 8].Text = Convert.ToString(ds.Tables[1].Rows[i]["Staff_name"]);
                            sno++;
                        }

                        //fp_stud.Height = 40 + (fp_stud.Sheets[0].RowCount * 30);
                        fp_stud.Sheets[0].PageSize = fp_stud.Sheets[0].RowCount;
                        fp_stud.Visible = true;
                        fp_stud.Sheets[0].Visible = true;
                        Fp_Individual_Strength.Visible = false;
                        Fp_Individual_Strength.Sheets[0].Visible = false;
                        btnprint.Visible = true;
                    }
                    chk = false;
                    return;
                }

                ds = DataAccess.select_method("TRANSPORT_STRENGTH_REPORT_STAGE", hat, "sp");

                long noftravrs = 0;
                long noftravrs_all = 0;
                string tmpvehid = "";
                int initialrow = 0;
                int endrow = 0;
                long occupy = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    veh_id_1 = " and rg.vehid in('" + ds.Tables[0].Rows[i]["veh_id"].ToString() + "') ";
                    route_id_1 = " and rg.Bus_RouteID in('" + ds.Tables[0].Rows[i]["route_id"].ToString() + "') ";
                    stage_id_1 = " and rg.boarding in('" + ds.Tables[0].Rows[i]["stage_id"].ToString() + "') ";

                    hat_1.Clear();
                    hat_1.Add("COLLEGECODE", college_code);//rajasekar 23/4
                    hat_1.Add("DISTRICT", dist_code_1);
                    hat_1.Add("STAGE", stage_id_1);
                    hat_1.Add("VEHICLE", veh_id_1);
                    hat_1.Add("ROUTE", route_id_1);

                    if (ar != Fp_strength.Sheets[0].RowCount - 1)
                    {
                        hat_1.Add("Stud_maledeg", studmale_tag);
                        hat_1.Add("Stud_femaledeg", studfemale_tag);
                        hat_1.Add("Stud_malebatch", studmale_tagbatch);
                        hat_1.Add("Stud_femalebatch", studfemale_tagbatch);
                        hat_1.Add("Staff_deptmale", staffmale_tag);
                        hat_1.Add("Staff_deptfemale", stafffemale_tag);
                    }
                    else
                    {
                        hat_1.Add("Stud_maledeg", class_var);
                        hat_1.Add("Stud_femaledeg", class_var);
                        hat_1.Add("Stud_malebatch", "");
                        hat_1.Add("Stud_femalebatch", "");
                        hat_1.Add("Staff_deptmale", deptcode);
                        hat_1.Add("Staff_deptfemale", deptcode);
                    }


                    ds_1.Clear();
                    ds_1 = DataAccess.select_method("TRANSPORT_STRENGTH_REPORT_STAGE_2", hat_1, "sp");

                    if (i == 0)
                    {
                        bind_header();
                    }


                    Fp_Individual_Strength.Sheets[0].RowCount++;
                    if (tmpvehid.ToString() == "")
                    {
                        tmpvehid = Convert.ToString(ds.Tables[0].Rows[i]["veh_id"]);
                        noftravrs = Convert.ToInt16(ds.Tables[0].Rows[i]["noftravrs"]);
                        initialrow = Fp_Individual_Strength.Sheets[0].RowCount - 1;
                        noftravrs_all = noftravrs;
                    }
                    Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    if (ac == 4)
                    {
                        Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["stage_name"].ToString();
                        Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["route_id"].ToString();
                        Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["veh_id"].ToString();

                    }
                    else if (ac == 3)
                    {
                        Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["route_id"].ToString();
                        Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["veh_id"].ToString();
                        Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["stage_name"].ToString();

                    }
                    else if (ac == 2)
                    {
                        Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["veh_id"].ToString();
                        Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["route_id"].ToString();
                        Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["stage_name"].ToString();

                    }

                    Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["sess"].ToString();
                    Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[i]["arr_time"].ToString();
                    Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 1, 6].Text = ds.Tables[0].Rows[i]["dep_time"].ToString();
                    Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 1, 7].Text = ds.Tables[0].Rows[i]["wait_mins"].ToString();
                    if (ds.Tables[0].Rows[i]["sess"].ToString() == "A")
                    {
                        Fp_Individual_Strength.Sheets[0].SpanModel.Add(Fp_Individual_Strength.Sheets[0].RowCount - 2, 8, 2, 1);
                        Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 2, 8].Text = Convert.ToString(ds_1.Tables[0].Rows.Count);
                        Fp_Individual_Strength.Sheets[0].SpanModel.Add(Fp_Individual_Strength.Sheets[0].RowCount - 2, 9, 2, 1);
                        Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 2, 9].Text = Convert.ToString(ds_1.Tables[1].Rows.Count);
                        Fp_Individual_Strength.Sheets[0].SpanModel.Add(Fp_Individual_Strength.Sheets[0].RowCount - 2, 10, 2, 1);
                        Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 2, 10].Text = Convert.ToString(Convert.ToInt16(ds_1.Tables[0].Rows.Count) + Convert.ToInt16(ds_1.Tables[1].Rows.Count));
                        Fp_Individual_Strength.Sheets[0].SpanModel.Add(Fp_Individual_Strength.Sheets[0].RowCount - 2, 11, 2, 1);
                        Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 2, 11].Text = Convert.ToString(ds_1.Tables[2].Rows.Count);
                        Fp_Individual_Strength.Sheets[0].SpanModel.Add(Fp_Individual_Strength.Sheets[0].RowCount - 2, 12, 2, 1);
                        Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 2, 12].Text = Convert.ToString(ds_1.Tables[3].Rows.Count);
                        Fp_Individual_Strength.Sheets[0].SpanModel.Add(Fp_Individual_Strength.Sheets[0].RowCount - 2, 13, 2, 1);
                        Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 2, 13].Text = Convert.ToString(Convert.ToInt16(ds_1.Tables[2].Rows.Count) + Convert.ToInt16(ds_1.Tables[3].Rows.Count));
                        occupy = occupy + Convert.ToInt16(ds_1.Tables[0].Rows.Count) + Convert.ToInt16(ds_1.Tables[1].Rows.Count) + Convert.ToInt16(ds_1.Tables[2].Rows.Count) + Convert.ToInt16(ds_1.Tables[3].Rows.Count);
                        if (ac == 4)
                        {
                            sno++;

                        }
                    }
                    if (Convert.ToString(tmpvehid) != Convert.ToString(ds.Tables[0].Rows[i]["veh_id"]))
                    {


                        if (noftravrs >= occupy)
                        {
                            noftravrs = noftravrs - occupy;
                        }
                        endrow = Fp_Individual_Strength.Sheets[0].RowCount - 1;

                        Fp_Individual_Strength.Sheets[0].SpanModel.Add(initialrow, 14, endrow - initialrow, 1);
                        Fp_Individual_Strength.Sheets[0].Cells[initialrow, 14].Text = Convert.ToString(noftravrs_all);
                        Fp_Individual_Strength.Sheets[0].SpanModel.Add(initialrow, 15, endrow - initialrow, 1);
                        Fp_Individual_Strength.Sheets[0].Cells[initialrow, 15].Text = Convert.ToString(occupy);
                        Fp_Individual_Strength.Sheets[0].SpanModel.Add(initialrow, 16, endrow - initialrow, 1);
                        Fp_Individual_Strength.Sheets[0].Cells[initialrow, 16].Text = Convert.ToString(noftravrs);

                        tmpvehid = Convert.ToString(ds.Tables[0].Rows[i]["veh_id"]);
                        noftravrs = Convert.ToInt16(ds.Tables[0].Rows[i]["noftravrs"]);
                        initialrow = Fp_Individual_Strength.Sheets[0].RowCount - 1;
                        occupy = 0;
                        noftravrs_all = noftravrs;
                        if ((ac == 3) || (ac == 2))
                        {
                            sno++;
                            Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                        }
                    }

                }
                if (noftravrs >= occupy)
                {
                    noftravrs = noftravrs - occupy;
                }
                endrow = Fp_Individual_Strength.Sheets[0].RowCount;
                //Fp_Individual_Strength.Sheets[0].SpanModel.Add(initialrow, 14, endrow - initialrow, 1);
                //Fp_Individual_Strength.Sheets[0].Cells[initialrow, 14].Text = Convert.ToString(noftravrs);

                Fp_Individual_Strength.Sheets[0].SpanModel.Add(initialrow, 14, endrow - initialrow, 1);
                Fp_Individual_Strength.Sheets[0].Cells[initialrow, 14].Text = Convert.ToString(noftravrs_all);

                Fp_Individual_Strength.Sheets[0].SpanModel.Add(initialrow, 15, endrow - initialrow, 1);
                Fp_Individual_Strength.Sheets[0].Cells[initialrow, 15].Text = Convert.ToString(occupy);
                Fp_Individual_Strength.Sheets[0].SpanModel.Add(initialrow, 16, endrow - initialrow, 1);
                Fp_Individual_Strength.Sheets[0].Cells[initialrow, 16].Text = Convert.ToString(noftravrs);

                chk = false;

                if (Fp_Individual_Strength.Sheets[0].RowCount > 0)
                {

                    Fp_Individual_Strength.Sheets[0].RowCount++;
                    Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 1, 7].Text = "TOTAL";
                    Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
                    Fp_Individual_Strength.Sheets[0].Rows[Fp_Individual_Strength.Sheets[0].RowCount - 1].Font.Bold = true;

                    for (int col = 8; col < Fp_Individual_Strength.Sheets[0].ColumnCount; col++)
                    {
                        int tot_count = 0;
                        for (int row = 0; row < Fp_Individual_Strength.Sheets[0].RowCount - 1; row++)
                        {
                            if (Convert.ToString(Fp_Individual_Strength.Sheets[0].Cells[row, col].Text.ToString()) != "")
                            {
                                tot_count = tot_count + Convert.ToInt32(Fp_Individual_Strength.Sheets[0].Cells[row, col].Text.ToString());
                            }
                        }

                        Fp_Individual_Strength.Sheets[0].Cells[Fp_Individual_Strength.Sheets[0].RowCount - 1, col].Text = tot_count.ToString();
                    }

                    Fp_Individual_Strength.Height = 40 + (Fp_Individual_Strength.Sheets[0].RowCount * 30);
                    Fp_Individual_Strength.Sheets[0].PageSize = Fp_Individual_Strength.Sheets[0].RowCount;


                    Fp_Individual_Strength.SaveChanges();
                    Fp_Individual_Strength.Visible = true;
                    Fp_Individual_Strength.Sheets[0].Visible = true;
                    fp_stud.Visible = false;
                    fp_stud.Sheets[0].Visible = true;
                    btnprint.Visible = true;
                }
                else
                {
                    lblerrmsg.Text = "No Records Found";
                    lblerrmsg.Visible = true;
                    btnprint.Visible = false;
                }
            }


        }
        catch (Exception ex)
        {
            
        }
    }
    public void bind_header()
    {

        Fp_Individual_Strength.Sheets[0].ColumnHeader.RowCount = 0;
        Fp_Individual_Strength.Sheets[0].RowCount = 0;
        Fp_Individual_Strength.Sheets[0].ColumnCount = 0;

        Fp_Individual_Strength.Sheets[0].ColumnHeader.RowCount = 2;
        Fp_Individual_Strength.Sheets[0].ColumnCount = 17;

        Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        if (ac == 4)
        {
            Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Stage";
            Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Route";
            Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Vehicle";
        }
        if (ac == 3)
        {
            Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Route";
            Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Vehicle";
            Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Stage";
        }
        if (ac == 2)
        {
            Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Vehicle";
            Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Route";
            Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Stage";
        }
        Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Seesion";
        Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Arrival";
        Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Departure";
        Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Waiting";



        Fp_Individual_Strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
        Fp_Individual_Strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
        Fp_Individual_Strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
        Fp_Individual_Strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
        Fp_Individual_Strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
        Fp_Individual_Strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
        Fp_Individual_Strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
        Fp_Individual_Strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);


        Fp_Individual_Strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 1, 3);
        Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Student";
        Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[1, 8].Text = "M";
        Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[1, 9].Text = "F";
        Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[1, 10].Text = "TOT";

        Fp_Individual_Strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 11, 1, 3);
        Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Staff";
        Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[1, 11].Text = "M";
        Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[1, 12].Text = "F";
        Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[1, 13].Text = "TOT";

        Fp_Individual_Strength.Sheets[0].Columns[8].Visible = true;
        Fp_Individual_Strength.Sheets[0].Columns[9].Visible = true;
        Fp_Individual_Strength.Sheets[0].Columns[10].Visible = true;
        Fp_Individual_Strength.Sheets[0].Columns[11].Visible = true;
        Fp_Individual_Strength.Sheets[0].Columns[12].Visible = true;
        Fp_Individual_Strength.Sheets[0].Columns[13].Visible = true;

        if (ddl_sex.Items[0].Selected == true && ddl_sex.Items[1].Selected == false)
        {
            Fp_Individual_Strength.Sheets[0].Columns[8].Visible = true;
            Fp_Individual_Strength.Sheets[0].Columns[9].Visible = false;
            Fp_Individual_Strength.Sheets[0].Columns[10].Visible = false;

            Fp_Individual_Strength.Sheets[0].Columns[11].Visible = true;
            Fp_Individual_Strength.Sheets[0].Columns[12].Visible = false;
            Fp_Individual_Strength.Sheets[0].Columns[13].Visible = false;
        }
        else if (ddl_sex.Items[0].Selected == false && ddl_sex.Items[1].Selected == true)
        {
            Fp_Individual_Strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 1, 2);
            Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Student";
            Fp_Individual_Strength.Sheets[0].Columns[8].Visible = false;
            Fp_Individual_Strength.Sheets[0].Columns[9].Visible = true;
            Fp_Individual_Strength.Sheets[0].Columns[10].Visible = false;

            Fp_Individual_Strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 12, 1, 2);
            Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Staff";
            Fp_Individual_Strength.Sheets[0].Columns[11].Visible = false;
            Fp_Individual_Strength.Sheets[0].Columns[12].Visible = true;
            Fp_Individual_Strength.Sheets[0].Columns[13].Visible = false;

        }


        Fp_Individual_Strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 14, 2, 1);
        Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[0, 14].Text = "Allotted";

        Fp_Individual_Strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 15, 2, 1);
        Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[0, 15].Text = "Admitted";

        Fp_Individual_Strength.Sheets[0].ColumnHeaderSpanModel.Add(0, 16, 2, 1);
        Fp_Individual_Strength.Sheets[0].ColumnHeader.Cells[0, 16].Text = "Vacancy";



        Fp_Individual_Strength.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        Fp_Individual_Strength.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
        Fp_Individual_Strength.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
        Fp_Individual_Strength.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Center;
        Fp_Individual_Strength.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Center;
        Fp_Individual_Strength.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Center;
        Fp_Individual_Strength.Sheets[0].Columns[13].HorizontalAlign = HorizontalAlign.Center;
        Fp_Individual_Strength.Sheets[0].Columns[14].HorizontalAlign = HorizontalAlign.Center;
        Fp_Individual_Strength.Sheets[0].Columns[15].HorizontalAlign = HorizontalAlign.Center;
        Fp_Individual_Strength.Sheets[0].Columns[16].HorizontalAlign = HorizontalAlign.Center;

        Fp_Individual_Strength.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
        Fp_Individual_Strength.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
        Fp_Individual_Strength.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
        Fp_Individual_Strength.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);






    }
    public void bind_studheader()
    {
        fp_stud.Sheets[0].ColumnHeader.RowCount = 0;
        fp_stud.Sheets[0].RowCount = 0;
        fp_stud.Sheets[0].ColumnCount = 0;

        fp_stud.Sheets[0].ColumnHeader.RowCount = 1;
        fp_stud.Sheets[0].ColumnCount = 9;

        fp_stud.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        fp_stud.Sheets[0].ColumnHeader.Cells[0, 1].Text = "College";
        fp_stud.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
        fp_stud.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Vehicle";
        fp_stud.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Route";
        fp_stud.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Stage";
        fp_stud.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Roll No";
        fp_stud.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Reg No";
        fp_stud.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Student Name";

        fp_stud.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
        fp_stud.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
        fp_stud.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
        fp_stud.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
        fp_stud.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
        fp_stud.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
        fp_stud.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
    }

    public void bind_staffheader()
    {
        fp_stud.Sheets[0].ColumnHeader.RowCount = 0;
        fp_stud.Sheets[0].RowCount = 0;
        fp_stud.Sheets[0].ColumnCount = 0;

        fp_stud.Sheets[0].ColumnHeader.RowCount = 1;
        fp_stud.Sheets[0].ColumnCount = 9;

        fp_stud.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        fp_stud.Sheets[0].ColumnHeader.Cells[0, 1].Text = "College";
        fp_stud.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Department";
        fp_stud.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Designation";
        fp_stud.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Vehicle";
        fp_stud.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Route";
        fp_stud.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Stage";
        fp_stud.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Staff Code";
        fp_stud.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Staff Name";

        fp_stud.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
        fp_stud.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
        fp_stud.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
        fp_stud.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
        fp_stud.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
        fp_stud.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
        fp_stud.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
        fp_stud.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
    }
    protected void ddl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        count = 0;
        selected_batch = "";
        for (int i = 0; i < ddl_batch.Items.Count; i++)
        {
            if (ddl_batch.Items[i].Selected == true)
            {
                count++;
                ddl_batch.Items[i].Selected = true;
                txt_batch.Text = "Batch(" + count.ToString() + ")";
                if (selected_batch == "")
                {
                    selected_batch = ddl_batch.Items[i].Value.ToString();
                }
                else
                {
                    selected_batch = selected_batch + "," + ddl_batch.Items[i].Value.ToString();
                }
            }
        }

    }
    protected void chk_batch_CheckedChanged(object sender, EventArgs e)
    {
        count = 0;
        selected_batch = "";
        txt_batch.Text = "";
        if (chk_batch.Checked == true)
        {
            for (int i = 0; i < ddl_batch.Items.Count; i++)
            {
                count++;
                ddl_batch.Items[i].Selected = true;
                txt_batch.Text = "Batch(" + count.ToString() + ")";
                if (selected_batch == "")
                {
                    selected_batch = ddl_batch.Items[i].Value.ToString();
                }
                else
                {
                    selected_batch = selected_batch + "," + ddl_batch.Items[i].Value.ToString();
                }
            }
        }
        else if (chk_batch.Checked == false)
        {

            for (int i = 0; i < ddl_batch.Items.Count; i++)
            {
                txt_batch.Text = "--Select--";
                ddl_batch.Items[i].Selected = false;
            }
        }
        load_degree();
    }
    protected void ddl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        count = 0;
        selected_courseid = "";
        for (int i = 0; i < ddl_degree.Items.Count; i++)
        {
            if (ddl_degree.Items[i].Selected == true)
            {
                count++;
                ddl_degree.Items[i].Selected = true;
                txt_degree.Text = Label2.Text + "(" + count.ToString() + ")";
                if (selected_courseid == "")
                {
                    selected_courseid = ddl_degree.Items[i].Value.ToString();
                }
                else
                {
                    selected_courseid = selected_courseid + "," + ddl_degree.Items[i].Value.ToString();
                }
            }
        }
        load_branch();
        chk_branch.Checked = true;
        chk_branch_CheckedChanged(sender, e);
    }
    protected void chk_degree_CheckedChanged(object sender, EventArgs e)
    {
        count = 0;
        selected_courseid = "";
        txt_degree.Text = "";
        if (chk_degree.Checked == true)
        {
            for (int i = 0; i < ddl_degree.Items.Count; i++)
            {
                count++;
                ddl_degree.Items[i].Selected = true;
                txt_degree.Text = Label2.Text + "(" + count.ToString() + ")";
                if (selected_courseid == "")
                {
                    selected_courseid = ddl_degree.Items[i].Value.ToString();
                }
                else
                {
                    selected_courseid = selected_courseid + "," + ddl_degree.Items[i].Value.ToString();
                }
            }
        }
        else if (chk_degree.Checked == false)
        {

            for (int i = 0; i < ddl_degree.Items.Count; i++)
            {
                txt_degree.Text = "--Select--";
                ddl_degree.Items[i].Selected = false;
            }
        }
        load_branch();
        chk_branch.Checked = true;
        chk_branch_CheckedChanged(sender, e);
    }
    protected void ddl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        count = 0;
        selected_depid = "";
        for (int i = 0; i < ddl_branch.Items.Count; i++)
        {
            if (ddl_branch.Items[i].Selected == true)
            {
                count++;
                ddl_branch.Items[i].Selected = true;
                txt_branch.Text = Label3.Text+"(" + count.ToString() + ")";
                if (selected_depid == "")
                {
                    selected_depid = ddl_branch.Items[i].Value.ToString();
                }
                else
                {
                    selected_depid = selected_depid + "," + ddl_branch.Items[i].Value.ToString();
                }
            }
        }

    }
    protected void chk_branch_CheckedChanged(object sender, EventArgs e)
    {
        txt_branch.Text = "";
        count = 0;
        selected_depid = "";
        if (chk_branch.Checked == true)
        {
            for (int i = 0; i < ddl_branch.Items.Count; i++)
            {
                count++;
                ddl_branch.Items[i].Selected = true;
                txt_branch.Text = Label3.Text + "(" + count.ToString() + ")";
                if (selected_depid == "")
                {
                    selected_depid = ddl_branch.Items[i].Value.ToString();
                }
                else
                {
                    selected_depid = selected_depid + "," + ddl_branch.Items[i].Value.ToString();
                }
            }
        }
        else if (chk_branch.Checked == false)
        {
            for (int i = 0; i < ddl_branch.Items.Count; i++)
            {
                txt_branch.Text = "--Select--";
                ddl_branch.Items[i].Selected = false;
            }
        }
    }
    private void load_batch()
    {
        ddl_batch.Items.Clear();
        newds.Dispose();
        newds.Clear();
        newds.Reset();
        if (selected_college.ToString().Trim() != "")
        {
            newds = DataAccess.select_method("select distinct batch_year from registration where college_code in(" + selected_college + ")", ht, "Text");
            if (newds.Tables[0].Rows.Count > 0)
            {
                ddl_batch.DataSource = newds;
                ddl_batch.DataTextField = "batch_year";
                ddl_batch.DataValueField = "batch_year";
                ddl_batch.DataBind();
            }

        }
    }

    private void load_degree()
    {
        ddl_degree.Items.Clear();
        string sqlstr = "";
        newds.Dispose();
        newds.Clear();
        newds.Reset();
        if (selected_college.ToString().Trim() != "")
        {
            if (singleuser == "True")
            {
                sqlstr = " select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code";
                sqlstr = sqlstr + " and degree.college_code in(" + selected_college + ") and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "' ";
            }
            else
            {
                sqlstr = " select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code ";
                sqlstr = sqlstr + " and degree.college_code  in(" + selected_college + ")  and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_user + "' ";
            }
            newds = DataAccess.select_method(sqlstr, ht, "Text");
            if (newds.Tables[0].Rows.Count > 0)
            {
                ddl_degree.DataSource = newds;
                ddl_degree.DataTextField = "course_name";
                ddl_degree.DataValueField = "course_id";
                ddl_degree.DataBind();
            }
        }
    }
    private void load_branch()
    {
        ddl_branch.Items.Clear();
        string sqlstr = "";
        newds.Dispose();
        newds.Clear();
        newds.Reset();
        if (selected_courseid.ToString().Trim() != "")
        {
            if (singleuser == "True")
            {
                sqlstr = " select distinct degree.degree_code,department.dept_name,degree.Acronym,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code";
                sqlstr = sqlstr + " and course.college_code =degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + selected_courseid + ") and degree.college_code in(" + selected_college + ")   and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "' ";
            }
            else
            {
                sqlstr = " select distinct degree.degree_code,department.dept_name,degree.Acronym,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code ";
                sqlstr = sqlstr + " and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + selected_courseid + ") and degree.college_code in(" + selected_college + ")   and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_user + "' ";
            }
            newds = DataAccess.select_method(sqlstr, ht, "Text");
            if (newds.Tables[0].Rows.Count > 0)
            {
                ddl_branch.DataSource = newds;
                ddl_branch.DataTextField = "dept_name";
                ddl_branch.DataValueField = "dept_code";
                ddl_branch.DataBind();
            }
        }
    }
    protected void ddl_sex_SelectedIndexChanged(object sender, EventArgs e)
    {
        count = 0;

        for (int i = 0; i < ddl_sex.Items.Count; i++)
        {
            if (ddl_sex.Items[i].Selected == true)
            {
                count++;
                ddl_sex.Items[i].Selected = true;
                txt_sex.Text = "Gender(" + count.ToString() + ")";

            }
        }

    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        if (Fp_strength.Sheets[0].Visible == false || Fp_strength.Visible == false)
        {
            lblerrmsg.Text = "There is no record to print";
            lblerrmsg.Visible = true;
            return;
        }
        Session["column_header_row_count"] = 2;
        string degreedetails = "Transport Detailed Report";
        string pagename = "Transport_strength_Report.aspx";
        Printcontrol.loadspreaddetails(Fp_strength, pagename, degreedetails);
        Printcontrol.Visible = true;
    }
    protected void btnprint_Click(object sender, EventArgs e)
    {

        Session["column_header_row_count"] = 2;
        string degreedetails = "Transport Detailed Report";
        string pagename = "Transport_strength_Report.aspx";



        if (Fp_Individual_Strength.Visible == true && Fp_Individual_Strength.Sheets[0].Visible == true)
        {
            if (Fp_Individual_Strength.Sheets[0].RowCount == 0)
            {
                lblerrmsg.Text = "There is no record to print";
                lblerrmsg.Visible = true;
                return;
            }
            Printcontrol.loadspreaddetails(Fp_Individual_Strength, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        else if (fp_stud.Visible == true && fp_stud.Sheets[0].Visible == true)
        {
            if (fp_stud.Sheets[0].RowCount == 0)
            {
                lblerrmsg.Text = "There is no record to print";
                lblerrmsg.Visible = true;
                return;
            }
            Printcontrol.loadspreaddetails(fp_stud, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        else
        {
            lblerrmsg.Text = "There is no record to print";
            lblerrmsg.Visible = true;
            return;
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
        lbl.Add(lblcollege);
        //lbl.Add(lbl_stream);
        lbl.Add(Label2);
        lbl.Add(Label3);
        //lbl.Add(lbl_sem);
        fields.Add(0);
        // fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        //fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
    // last modified 22-10-2016 sudhagar

}