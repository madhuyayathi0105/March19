using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.Drawing;
using System.Text.RegularExpressions;
using System.IO;

public partial class Default6 : System.Web.UI.Page
{
    SqlConnection con7 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con5 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlCommand cmd = new SqlCommand();
    public void Connection()
    {
        con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
        con.Open();
    }
    DAccess2 da = new DAccess2();
    DataSet ds;
    DataSet dss;
    SqlDataAdapter danew;
    DAccess2 dset = new DAccess2();
    int chk_up = 0;
    DataSet d1 = new DataSet();
    DataSet d2 = new DataSet();
    DataSet d3 = new DataSet();
    Hashtable hastab = new Hashtable();
    Hashtable ht = new Hashtable();
    static Hashtable spr_hash = new Hashtable();
    static Hashtable priority_hash = new Hashtable();

    Boolean Cellclick = false;
    static int chk = 0;
    string sqlcmd = "";
    FarPoint.Web.Spread.ComboBoxCellType cf = new FarPoint.Web.Spread.ComboBoxCellType();
    ArrayList keyarray = new ArrayList();
    ArrayList valuearray = new ArrayList();
    Hashtable loadhas = new Hashtable();
    DataSet dsload = new DataSet();
    Boolean cellclick = false;
    //Session["chk_upval"] == "";
    static Hashtable Has_Stage = new Hashtable();
    string PageLogOut = "";

    protected void lb2_Click(object sender, EventArgs e) //Sankar For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (btnsave2.Text == "Update")
        {
            Btn_addabove.Visible = true;
            Btn_Addbelow.Visible = true;
            Btn_Delete_Row.Visible = true;
        }
        else
        {
            Btn_addabove.Visible = false;
            Btn_Addbelow.Visible = false;
            Btn_Delete_Row.Visible = false;
        }
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
            Session["chk_upval"] = "";
            FpTransport.Sheets[0].AutoPostBack = true;
            FpTransport.CommandBar.Visible = false;
            FpTransport.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            FpTransport.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Center;
            FpTransport.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            FpTransport.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpTransport.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpTransport.Sheets[0].DefaultStyle.Font.Bold = false;
            FarPoint.Web.Spread.TextCellType tx1 = new FarPoint.Web.Spread.TextCellType();

            FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
            style1.Font.Size = 12;
            style1.Font.Bold = true;
            style1.HorizontalAlign = HorizontalAlign.Center;
            style1.ForeColor = Color.Black;
            FpTransport.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            FpTransport.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            FpTransport.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            FpTransport.Sheets[0].AllowTableCorner = true;

            FpTransport.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            FpTransport.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
            FpTransport.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
            FpTransport.Pager.Align = HorizontalAlign.Right;
            FpTransport.Pager.Font.Bold = true;
            FpTransport.Pager.Font.Name = "Book Antiqua";
            FpTransport.Pager.ForeColor = Color.DarkGreen;
            FpTransport.Pager.BackColor = Color.Beige;
            FpTransport.Pager.BackColor = Color.AliceBlue;


            FpTransport.Sheets[0].ColumnCount = 5;
            FpTransport.SheetCorner.Cells[0, 0].Text = "S.No";
            FpTransport.SheetCorner.Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpTransport.Sheets[0].ColumnHeader.Cells[0, 0].Text = "RouteID";
            FpTransport.Sheets[0].Columns[0].CellType = tx1;
            FpTransport.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpTransport.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Session";
            FpTransport.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Route";
            FpTransport.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Via";
            FpTransport.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Stage";
            FpTransport.Sheets[0].Columns[2].Locked = false;

            FpTransport.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //FpTransport.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

            FpTransport.Sheets[0].Columns[3].Width = 850;

            FpTransport.Visible = false;

            tbdate.Attributes.Add("readonly", "readonly");
            bindplace();
            loadstage();
            loadstage2();
            bindVehicleID();
            bindroute();
            load_detail();
            bind_droplist();
            btnMainGo_Click(sender, e);
            Accordion1.SelectedIndex = 0;
        }

    }
    public void bind_droplist()
    {
        string bindfrom = "";
        //bindfrom = "select * from Stage_Master";
        //DataSet dt_from = new DataSet();
        //dt_from = dset.select_method_wo_parameter(bindfrom, "text");
        //ddlfromstage.Items.Clear();
        //if (dt_from.Tables[0].Rows.Count > 0)
        //{
        //    ddlfromstage.DataSource = dt_from.Tables[0];
        //    ddlfromstage.DataTextField = "Stage_Name";
        //    ddlfromstage.DataValueField = "Stage_id";
        //    ddlfromstage.DataBind();
        //}
        //ddlfromstage.Items.Insert(0, "");


        //string bindto = "";
        //bindto = "select * from Stage_Master";
        //DataSet dt_to = new DataSet();
        //dt_to = dset.select_method_wo_parameter(bindto, "text");
        //ddltostage.Items.Clear();
        //if (dt_to.Tables[0].Rows.Count > 0)
        //{
        //    ddltostage.DataSource = dt_to.Tables[0];
        //    ddltostage.DataTextField = "Stage_Name";
        //    ddltostage.DataValueField = "Stage_id";
        //    ddltostage.DataBind();
        //}
        //ddltostage.Items.Insert(0, "");


    }
    public void load_detail()
    {
        //load_mainRoute();
        tbrouteid.Attributes.Add("onfocus", "changerouteid()");
        tbdate.Attributes.Add("onfocus", "changedate()");
        tbstages.Attributes.Add("onfocus", "changestage()");
        //tbfromstage1.Attributes.Add("onfocus", "stagefrom()");
        //tbtostage.Attributes.Add("onfocus", "stageTo()");
    }
    public void loadstage()
    {
        FarPoint.Web.Spread.TextCellType tb = new FarPoint.Web.Spread.TextCellType();
        FarPoint.Web.Spread.DoubleCellType du = new FarPoint.Web.Spread.DoubleCellType();
        FpSpreadstage.ActiveSheetView.SheetCorner.Cells[0, 0].Text = "S.No";
        FpSpreadstage.ActiveSheetView.DefaultRowHeight = 25;
        FpSpreadstage.ActiveSheetView.Rows.Default.Font.Name = "MS Sans Serif";
        FpSpreadstage.ActiveSheetView.Rows.Default.Font.Size = FontUnit.Small;
        FpSpreadstage.ActiveSheetView.Rows.Default.Font.Bold = false;
        FpSpreadstage.ActiveSheetView.Columns.Default.Font.Bold = false;
        FpSpreadstage.ActiveSheetView.Columns.Default.Font.Name = "MS Sans Serif";
        FpSpreadstage.ActiveSheetView.Columns.Default.Font.Size = FontUnit.Small;
        FpSpreadstage.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
        FpSpreadstage.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "MS Sans Serif";
        FpSpreadstage.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Small;
        FpSpreadstage.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
        FpSpreadstage.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
        FarPoint.Web.Spread.CheckBoxCellType ck = new FarPoint.Web.Spread.CheckBoxCellType();
        ck.AutoPostBack = true;
        FarPoint.Web.Spread.ComboBoxCellType cf = new FarPoint.Web.Spread.ComboBoxCellType();
        cf.AutoPostBack = true;
        Has_Stage.Clear();
        DataSet sp_set = new DataSet();
        string bindfrom = "";
        bindfrom = "select * from Stage_Master";
        sp_set = dset.select_method_wo_parameter(bindfrom, "text");
        if (sp_set.Tables[0].Rows.Count > 0)
        {
            cf.ShowButton = true;
            cf.UseValue = true;
            cf.DataSource = sp_set.Tables[0];
            cf.DataTextField = "Stage_Name";
            cf.DataValueField = "Stage_id";
            FpSpreadstage.Sheets[0].Columns[1].CellType = cf;
            for (int str = 0; str < sp_set.Tables[0].Rows.Count; str++)
            {
                if (Has_Stage.Contains(Convert.ToString(sp_set.Tables[0].Rows[str]["Stage_id"])) == false)
                {
                    Has_Stage.Add(Convert.ToString(sp_set.Tables[0].Rows[str]["Stage_id"]), Convert.ToString(sp_set.Tables[0].Rows[str]["Stage_Name"]));
                }
            }
        }

        FpSpreadstage.Sheets[0].RowCount = 0;
        FpSpreadstage.Sheets[0].ColumnCount = 6;
        FpSpreadstage.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Stage";
        FpSpreadstage.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Stage Name";
        FpSpreadstage.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Arriving Time";
        FpSpreadstage.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Departure Time";
        FpSpreadstage.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Waiting Time";
        FpSpreadstage.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Address";
        //FpSpreadstage.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Longitude";
        FpSpreadstage.Sheets[0].Columns[4].CellType = tb;
        FpSpreadstage.Sheets[0].Columns[2].CellType = tb;
        FpSpreadstage.Sheets[0].Columns[3].CellType = tb;
        FpSpreadstage.Sheets[0].Columns[0].Locked = true;

        FpSpreadstage.Sheets[0].Columns[0].Width = 100;
        FpSpreadstage.Sheets[0].Columns[1].Width = 100;
        FpSpreadstage.Sheets[0].Columns[2].Width = 100;
        FpSpreadstage.Sheets[0].Columns[3].Width = 150;
        FpSpreadstage.Sheets[0].Columns[4].Width = 100;
        FpSpreadstage.Sheets[0].Columns[5].Width = 300;
        FpSpreadstage.Sheets[0].PageSize = 100;
        FpSpreadstage.Width = 887;
        FpSpreadstage.Height = 271;
        FpSpreadstage.CommandBar.Visible = false;
        FpSpreadstage.SaveChanges();

    }
    public void loadstage2()
    {
        FarPoint.Web.Spread.TextCellType tb = new FarPoint.Web.Spread.TextCellType();
        sprdMainstage.ActiveSheetView.SheetCorner.Cells[0, 0].Text = "S.No";
        sprdMainstage.ActiveSheetView.DefaultRowHeight = 25;
        sprdMainstage.ActiveSheetView.Rows.Default.Font.Name = "MS Sans Serif";
        sprdMainstage.ActiveSheetView.Rows.Default.Font.Size = FontUnit.Small;
        sprdMainstage.ActiveSheetView.Rows.Default.Font.Bold = false;
        sprdMainstage.ActiveSheetView.Columns.Default.Font.Bold = false;
        sprdMainstage.ActiveSheetView.Columns.Default.Font.Name = "MS Sans Serif";
        sprdMainstage.ActiveSheetView.Columns.Default.Font.Size = FontUnit.Small;
        sprdMainstage.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
        sprdMainstage.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "MS Sans Serif";
        sprdMainstage.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Small;
        sprdMainstage.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
        sprdMainstage.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
        sprdMainstage.Sheets[0].ColumnCount = 6;
        sprdMainstage.Sheets[0].RowCount = 0;
        sprdMainstage.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Stage";
        sprdMainstage.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Stage Name";
        sprdMainstage.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Arriving Time";
        sprdMainstage.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Departure Time";
        sprdMainstage.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Waiting Time";
        sprdMainstage.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Address";
        //sprdMainstage.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Longitude";
        sprdMainstage.Sheets[0].Columns[4].CellType = tb;
        sprdMainstage.Sheets[0].Columns[2].CellType = tb;
        sprdMainstage.Sheets[0].Columns[3].CellType = tb;
        sprdMainstage.Sheets[0].Columns[0].Locked = true;
        sprdMainstage.Sheets[0].Columns[1].Locked = true;
        //FarPoint.Web.Spread.RegExpCellType rgex5_new = new FarPoint.Web.Spread.RegExpCellType();
        //rgex5_new.ValidationExpression = "^[a-zA-Z0-9]+$";
        //rgex5_new.ErrorMessage = "Enter Valid Latitude";
        //sprdMainstage.Sheets[0].Columns[5].CellType = rgex5_new;

        //FarPoint.Web.Spread.RegExpCellType rgex6_New = new FarPoint.Web.Spread.RegExpCellType();
        //rgex6_New.ValidationExpression = "^[a-zA-Z0-9]+$";
        //rgex6_New.ErrorMessage = "Enter Valid Longitude";
        //sprdMainstage.Sheets[0].Columns[6].CellType = rgex6_New;

        sprdMainstage.Sheets[0].Columns[0].Width = 100;
        sprdMainstage.Sheets[0].Columns[1].Width = 100;
        sprdMainstage.Sheets[0].Columns[2].Width = 100;
        sprdMainstage.Sheets[0].Columns[3].Width = 150;
        sprdMainstage.Sheets[0].Columns[4].Width = 100;
        sprdMainstage.Sheets[0].Columns[5].Width = 300;
        sprdMainstage.Sheets[0].PageSize = 100;
        sprdMainstage.Width = 887;
        sprdMainstage.Height = 271;
        sprdMainstage.CommandBar.Visible = false;
    }
    protected void ddlserachby_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlrouteID.Enabled = false;
        string sqlquery = string.Empty;
        ddlrouteID.Items.Clear();
        ddlrouteID.Items.Insert(0, new ListItem("All", "-1"));
        if (ddlserachby.Text == "-1")
        {
            sqlquery = "select distinct Route_ID from routemaster";
        }
        else
        {
            sqlquery = "select distinct Route_ID from routemaster where Stage_Name = '" + ddlserachby.SelectedItem.Value.ToString() + "'";
        }
        ds = da.select_method_wo_parameter(sqlquery, "txt");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ddlrouteID.Items.Add(ds.Tables[0].Rows[i]["Route_ID"].ToString());
            }
            ddlrouteID.SelectedIndex = 0;
        }
        con.Close();
    }
    public void load_mainRoute()
    {
        int i2 = 0;
        DAccess2 da = new DAccess2();
        DataSet ds = new DataSet();
        FpTransport.Visible = true;

        //cmd = new SqlCommand("select * from routemaster", con);
        //con.Open();
        //SqlDataReader dr_getval = cmd.ExecuteReader();
        string selectquery = "select * from RouteMaster order by Route_ID";
        SqlDataAdapter daselectquery = new SqlDataAdapter(selectquery, con5);
        DataTable dsselectquery = new DataTable();
        daselectquery.Fill(dsselectquery);
        con5.Close();
        con5.Open();
        ArrayList l1 = new ArrayList();
        ArrayList l2 = new ArrayList();
        //while (dr_getval.Read())
        FpTransport.Sheets[0].RowCount = 0;
        for (int i1 = 0; i1 < dsselectquery.Rows.Count; i1++)
        {
            FpTransport.Visible = true;
            string Route = dsselectquery.Rows[i1]["Route_ID"].ToString();
            string session = dsselectquery.Rows[i1]["sess"].ToString();
            string Sess_Route = Route + "-" + session;
            if (l1.Contains(Sess_Route) == false)
            {


                string stagename = string.Empty;
                DataView dvview = new DataView();
                dsselectquery.DefaultView.RowFilter = "Route_ID='" + Route + "'and sess='" + session + "' ";
                dvview = dsselectquery.DefaultView;

                //for (int inew = 0; inew < dsselectquery.Rows.Count; inew++)
                //{
                foreach (DataRowView viewdata in dvview)
                {
                    if (stagename == "")
                    {
                        //stagename = dr_getval["Stage_Name"].ToString();
                        //stagename = dsselectquery.Rows[i1]["Stage_Name"].ToString();
                        stagename = viewdata["Stage_Name"].ToString();

                    }
                    else
                    {
                        stagename = stagename + "-" + viewdata["Stage_Name"].ToString();
                    }
                }
                FpTransport.Sheets[0].RowCount++;
                i2 = FpTransport.Sheets[0].RowCount - 1;
                FpTransport.Sheets[0].Cells[i2, 0].Text = dsselectquery.Rows[i1]["Route_ID"].ToString();
                FpTransport.Sheets[0].Cells[i2, 1].Text = dsselectquery.Rows[i1]["sess"].ToString();
                FpTransport.Sheets[0].Cells[i2, 2].Text = dsselectquery.Rows[i1]["Rou_From"].ToString() + "-" + dsselectquery.Rows[i1]["Rou_To"].ToString();
                FpTransport.Sheets[0].Cells[i2, 3].Text = stagename;
                FpTransport.Sheets[0].Cells[i2, 4].Text = dsselectquery.Rows[i1]["Stages"].ToString();
                // FpTransport.Sheets[0].RowCount = dsselectquery.Rows.Count;
                FpTransport.Sheets[0].PageSize = FpTransport.Rows.Count;

            }
            //FpTransport.Sheets[0].RowCount = dsselectquery.Rows.Count;
            l1.Add(Sess_Route);

        }


    }
    public void bindplace()
    {
        Connection();
        ddlserachby.Items.Clear();
        ddlserachby.Items.Insert(0, new ListItem("All", "-1"));
        string sql;
        sql = "select distinct s.Stage_Name,s.stage_id from routemaster r,stage_master s where cast(r.stage_name as varchar(100))=cast(s.stage_id as varchar(100)) order by s.Stage_Name";
        ds = da.select_method_wo_parameter(sql, "txt");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                // ddlserachby.Items.Add(ds.Tables[0].Rows[i]["Stage_Name"].ToString());    
                ddlserachby.Items.Add(new ListItem(Convert.ToString(ds.Tables[0].Rows[i]["Stage_Name"]), Convert.ToString(ds.Tables[0].Rows[i]["stage_id"])));

            }

            ddlserachby.SelectedIndex = 0;

        }
        con.Close();
    }
    public void bindroute()
    {
        Connection();
        ddlrouteID.Items.Clear();
        ddlrouteID.Items.Insert(0, new ListItem("All", "-1"));
        string sql;
        sql = "select distinct Route_ID from routemaster order by Route_ID";
        ds = da.select_method_wo_parameter(sql, "txt");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ddlrouteID.Items.Add(ds.Tables[0].Rows[i]["Route_ID"].ToString());
            }
            ddlrouteID.SelectedIndex = 0;

        }
        con.Close();
    }
    protected void FpTransport_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpTransport.Sheets[0].AutoPostBack = true;
        FpTransport.SaveChanges();
        FpSpreadstage.SaveChanges();
        sprdMainstage.SaveChanges();

        if (Cellclick == true)
        {
            try
            {
                //clear();
                FpSpreadstage.Sheets[0].RowCount = 0;
                sprdMainstage.Sheets[0].RowCount = 0;
                tbrouteid.Enabled = false;
                tbstages.Enabled = true;
                string activerow = "";
                string activecol = "";
                btnsave2.Text = "Update";
                lblrouteadd.Text = "Modify";

                Btn_addabove.Visible = true;
                Btn_Addbelow.Visible = true;
                Btn_Delete_Row.Visible = true;

                activerow = FpTransport.ActiveSheetView.ActiveRow.ToString();

                activecol = FpTransport.ActiveSheetView.ActiveColumn.ToString();
                string purpose = FpTransport.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                string retroll = FpTransport.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Text;

                string dselect1 = "select * from RouteMaster Order by Route_ID,Arr_Time";
                SqlDataAdapter daselectquery2 = new SqlDataAdapter(dselect1, con5);
                DataTable dsselectquery2 = new DataTable();
                daselectquery2.Fill(dsselectquery2);
                con5.Close();
                con5.Open();

                string selectquery1 = "select DISTINCT sess from RouteMaster";

                if (selectquery1 != "")
                {
                    SqlDataAdapter daselectquery1 = new SqlDataAdapter(selectquery1, con5);
                    DataTable dsselectquery1 = new DataTable();
                    daselectquery1.Fill(dsselectquery1);
                    con5.Close();
                    con5.Open();
                    if (dsselectquery2.Rows.Count > 0)
                    {
                        chk = 1;
                        string stagename = "Stage";
                        for (int isee = 0; isee < dsselectquery1.Rows.Count; isee++)
                        {

                            Buttonsave.Enabled = true;
                            Buttondelete.Enabled = true;
                            string sess1 = dsselectquery1.Rows[isee]["sess"].ToString();

                            DataView dvview1 = new DataView();
                            dsselectquery2.DefaultView.RowFilter = "Route_ID='" + retroll + "'and sess='" + sess1 + "' ";
                            dvview1 = dsselectquery2.DefaultView;
                            int tempcount = -1; int i = 0;
                            foreach (DataRowView viewdata1 in dvview1)
                            {
                                tempcount++;
                                tbrouteid.Text = viewdata1["Route_ID"].ToString();
                                tbstages.Text = viewdata1["Stages"].ToString();
                                if (viewdata1["Rou_From"].ToString() != "")
                                {
                                    Boolean e1_new = isNumeric(viewdata1["Rou_From"].ToString(), System.Globalization.NumberStyles.Integer);
                                    if (e1_new)
                                    {
                                        string Get_Stage = GetFunction("select distinct Stage_Name from stage_master where Stage_id = '" + viewdata1["Rou_From"].ToString() + "'");
                                        string Get_Stage_id = GetFunction("select distinct Stage_id from stage_master where Stage_id = '" + viewdata1["Rou_From"].ToString() + "'");
                                        tbfromstage1.Text = Get_Stage;
                                    }
                                    else
                                    {
                                        tbfromstage1.Text = viewdata1["Rou_From"].ToString();
                                    }

                                }
                                else
                                {

                                }
                                if (viewdata1["Rou_To"].ToString() != "")
                                {
                                    Boolean e1_new = isNumeric(viewdata1["Rou_To"].ToString(), System.Globalization.NumberStyles.Integer);
                                    if (e1_new)
                                    {
                                        string Get_Stage = GetFunction("select distinct Stage_Name from stage_master where Stage_id = '" + viewdata1["Rou_To"].ToString() + "'");
                                        string Get_Stage_id = GetFunction("select distinct Stage_id from stage_master where Stage_id = '" + viewdata1["Rou_To"].ToString() + "'");
                                        tbtostage.Text = Get_Stage;
                                    }
                                    else
                                    {
                                        tbtostage.Text = viewdata1["Rou_To"].ToString();
                                    }

                                }
                                else
                                {

                                }
                                string veh_id = viewdata1["Veh_ID"].ToString();
                                ddlvehiclid.SelectedItem.Text = veh_id;
                                string Mdate = viewdata1["Mdate"].ToString();
                                DateTime ddd_apply = Convert.ToDateTime(Mdate);
                                tbdate.Text = ddd_apply.ToString("dd-MM-yyyy");

                                if (sess1 == "A")
                                {

                                    //isee = sprdMainstage.Sheets[0].RowCount - 1;                             
                                    string waitingfp1sp = viewdata1["Wait_Mins"].ToString();
                                    string waitingtimetrs = string.Empty;
                                    if (waitingfp1sp.Length > 1)
                                    {
                                        waitingtimetrs = "0." + waitingfp1sp;
                                    }
                                    else
                                    {
                                        waitingtimetrs = "0.0" + waitingfp1sp;
                                    }

                                    string Address = viewdata1["Address"].ToString();
                                    //string Longitude = viewdata1["Longitude"].ToString();

                                    string Finalarrtimevaluesplit = string.Empty;
                                    string Finalarrtimevaluesplit1 = string.Empty;
                                    if (viewdata1["Arr_Time"].ToString() != "-" && viewdata1["Arr_Time"].ToString() != "Halt")
                                    {
                                        string arrtimesplit = viewdata1["Arr_Time"].ToString();
                                        string[] arrtimevaluesplit = arrtimesplit.Split(' ');
                                        Finalarrtimevaluesplit = arrtimevaluesplit[0].ToString();

                                    }
                                    string FinalDepttimevaluesplit = string.Empty;
                                    string FinalDepttimevaluesplit1 = string.Empty;
                                    if (viewdata1["Dep_Time"].ToString() != "-" && viewdata1["Dep_Time"].ToString() != "Halt")
                                    {
                                        string Depttimesplit = viewdata1["Dep_Time"].ToString();
                                        string[] Depttimevaluesplit = Depttimesplit.Split(' ');
                                        FinalDepttimevaluesplit = Depttimevaluesplit[0].ToString();

                                    }

                                    sprdMainstage.SaveChanges();
                                    sprdMainstage.Sheets[0].RowCount++;
                                    sprdMainstage.Sheets[0].Cells[sprdMainstage.Sheets[0].RowCount - 1, 0].Text = stagename + Convert.ToString(tempcount + 1);
                                    Boolean e1 = isNumeric(viewdata1["Stage_Name"].ToString(), System.Globalization.NumberStyles.Integer);
                                    if (e1)
                                    {
                                        if (viewdata1["Stage_Name"].ToString() != "")
                                        {
                                            string Get_Stage = GetFunction("select distinct Stage_Name from stage_master where Stage_id = '" + viewdata1["Stage_Name"].ToString() + "'");
                                            string Get_Stage_id = GetFunction("select distinct Stage_id from stage_master where Stage_id = '" + viewdata1["Stage_Name"].ToString() + "'");
                                            sprdMainstage.Sheets[0].Cells[sprdMainstage.Sheets[0].RowCount - 1, 1].Text = Get_Stage;
                                            sprdMainstage.Sheets[0].Cells[sprdMainstage.Sheets[0].RowCount - 1, 1].Tag = Get_Stage_id;
                                        }
                                        else
                                        {
                                            sprdMainstage.Sheets[0].Cells[sprdMainstage.Sheets[0].RowCount - 1, 1].Text = viewdata1["Stage_Name"].ToString();
                                        }
                                    }
                                    else
                                    {
                                        sprdMainstage.Sheets[0].Cells[sprdMainstage.Sheets[0].RowCount - 1, 1].Text = viewdata1["Stage_Name"].ToString();
                                    }


                                    if (Finalarrtimevaluesplit != "")
                                    {
                                        if (Finalarrtimevaluesplit.Contains(":") == true)
                                        {
                                            string[] splitarrtimeincell = Finalarrtimevaluesplit.Split(':');
                                            Finalarrtimevaluesplit1 = splitarrtimeincell[0] + "." + splitarrtimeincell[1];
                                            sprdMainstage.Sheets[0].Cells[sprdMainstage.Sheets[0].RowCount - 1, 2].Text = Finalarrtimevaluesplit1;
                                        }
                                        else
                                        {
                                            sprdMainstage.Sheets[0].Cells[sprdMainstage.Sheets[0].RowCount - 1, 2].Text = Finalarrtimevaluesplit;
                                        }

                                    }
                                    else
                                    {
                                        sprdMainstage.Sheets[0].Cells[sprdMainstage.Sheets[0].RowCount - 1, 2].Text = viewdata1["Arr_Time"].ToString();
                                    }
                                    if (FinalDepttimevaluesplit != "")
                                    {
                                        if (FinalDepttimevaluesplit.Contains(":") == true)
                                        {
                                            string[] splitDepttimeincell = FinalDepttimevaluesplit.Split(':');
                                            FinalDepttimevaluesplit1 = splitDepttimeincell[0] + "." + splitDepttimeincell[1];
                                            sprdMainstage.Sheets[0].Cells[sprdMainstage.Sheets[0].RowCount - 1, 3].Text = FinalDepttimevaluesplit1;
                                        }
                                        else
                                        {
                                            sprdMainstage.Sheets[0].Cells[sprdMainstage.Sheets[0].RowCount - 1, 3].Text = FinalDepttimevaluesplit;
                                        }
                                    }
                                    else
                                    {
                                        sprdMainstage.Sheets[0].Cells[sprdMainstage.Sheets[0].RowCount - 1, 3].Text = viewdata1["Dep_Time"].ToString();
                                    }
                                    sprdMainstage.Sheets[0].Cells[sprdMainstage.Sheets[0].RowCount - 1, 4].Text = waitingtimetrs;
                                    sprdMainstage.Sheets[0].Cells[sprdMainstage.Sheets[0].RowCount - 1, 5].Text = Address;
                                    //sprdMainstage.Sheets[0].Cells[sprdMainstage.Sheets[0].RowCount - 1, 6].Text = Longitude;

                                }
                                else
                                {
                                    FpSpreadstage.SaveChanges();
                                    string waitingfp2sp = viewdata1["Wait_Mins"].ToString();
                                    string waitingtimefp1 = string.Empty;
                                    if (waitingfp2sp.Length > 1)
                                    {
                                        waitingtimefp1 = "0." + waitingfp2sp;
                                    }
                                    else
                                    {
                                        waitingtimefp1 = "0.0" + waitingfp2sp;
                                    }

                                    string Address1 = viewdata1["Address"].ToString();
                                    //string Longitude1 = viewdata1["Longitude"].ToString();

                                    string Finalarrtimevaluesplitfp1 = string.Empty;
                                    string Finalarrtimevaluesplitfp2 = string.Empty;
                                    if (viewdata1["Arr_Time"].ToString() != "-" && viewdata1["Arr_Time"].ToString() != "Halt")
                                    {
                                        string arrtimesplitfp1 = viewdata1["Arr_Time"].ToString();
                                        string[] arrtimevaluesplitfp1 = arrtimesplitfp1.Split(' ');
                                        Finalarrtimevaluesplitfp1 = arrtimevaluesplitfp1[0].ToString();

                                    }
                                    string FinalDepttimevaluesplitfp1 = string.Empty;
                                    string FinalDepttimevaluesplif2 = string.Empty;
                                    if (viewdata1["Dep_Time"].ToString() != "-" && viewdata1["Dep_Time"].ToString() != "Halt")
                                    {
                                        string Depttimesplitfp1 = viewdata1["Dep_Time"].ToString();
                                        string[] Depttimevaluesplitfp1 = Depttimesplitfp1.Split(' ');
                                        FinalDepttimevaluesplitfp1 = Depttimevaluesplitfp1[0].ToString();

                                    }

                                    //FpSpreadstage.SaveChanges();
                                    FpSpreadstage.Sheets[0].RowCount++;
                                    //isee = FpSpreadstage.Sheets[0].RowCount - 1;
                                    FpSpreadstage.Sheets[0].Cells[FpSpreadstage.Sheets[0].RowCount - 1, 0].Text = stagename + Convert.ToString(tempcount + 1);
                                    FarPoint.Web.Spread.ComboBoxCellType cf = new FarPoint.Web.Spread.ComboBoxCellType();
                                    cf.AutoPostBack = true;
                                    if (viewdata1["Stage_Name"].ToString() != "")
                                    {
                                        DataSet sp_set = new DataSet();
                                        string bindfrom = "";
                                        bindfrom = "select * from Stage_Master";
                                        sp_set = dset.select_method_wo_parameter(bindfrom, "text");
                                        if (sp_set.Tables[0].Rows.Count > 0)
                                        {
                                            cf.ShowButton = true;
                                            cf.UseValue = true;
                                            cf.DataSource = sp_set.Tables[0];
                                            cf.DataTextField = "Stage_Name";
                                            cf.DataValueField = "Stage_id";
                                            FpSpreadstage.Sheets[0].Columns[1].CellType = cf;

                                        }
                                        Boolean e1 = isNumeric(viewdata1["Stage_Name"].ToString(), System.Globalization.NumberStyles.Integer);
                                        if (e1)
                                        {
                                            DataSet sp_set1 = new DataSet();
                                            string bind = "";
                                            bind = "select * from Stage_Master where Stage_id = '" + viewdata1["Stage_Name"].ToString() + "'";
                                            sp_set1 = dset.select_method_wo_parameter(bind, "text");
                                            if (sp_set1.Tables[0].Rows.Count > 0)
                                            {
                                                for (int i2 = 0; i2 < sp_set1.Tables[0].Rows.Count; i2++)
                                                {
                                                    FpSpreadstage.Sheets[0].Cells[FpSpreadstage.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(sp_set1.Tables[0].Rows[i2]["Stage_Name"]);
                                                    FpSpreadstage.Sheets[0].Cells[FpSpreadstage.Sheets[0].RowCount - 1, 1].Value = Convert.ToString(sp_set1.Tables[0].Rows[i2]["Stage_id"]);
                                                }

                                                FpSpreadstage.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            FpSpreadstage.Sheets[0].Cells[FpSpreadstage.Sheets[0].RowCount - 1, 1].Text = viewdata1["Stage_Name"].ToString();
                                        }

                                    }
                                    else
                                    {
                                        FpSpreadstage.Sheets[0].Cells[FpSpreadstage.Sheets[0].RowCount - 1, 1].Text = viewdata1["Stage_Name"].ToString();
                                    }

                                    if (Finalarrtimevaluesplitfp1 != "")
                                    {
                                        if (Finalarrtimevaluesplitfp1.Contains(":") == true)
                                        {
                                            string[] Finalarrtimevaluespli = Finalarrtimevaluesplitfp1.Split(':');
                                            Finalarrtimevaluesplitfp2 = Finalarrtimevaluespli[0] + "." + Finalarrtimevaluespli[1];
                                            FpSpreadstage.Sheets[0].Cells[FpSpreadstage.Sheets[0].RowCount - 1, 2].Text = Finalarrtimevaluesplitfp2;
                                        }
                                        else
                                        {
                                            FpSpreadstage.Sheets[0].Cells[FpSpreadstage.Sheets[0].RowCount - 1, 2].Text = Finalarrtimevaluesplitfp1;
                                        }

                                    }
                                    else
                                    {
                                        FpSpreadstage.Sheets[0].Cells[FpSpreadstage.Sheets[0].RowCount - 1, 2].Text = viewdata1["Arr_Time"].ToString();
                                    }
                                    if (FinalDepttimevaluesplitfp1 != "")
                                    {
                                        if (FinalDepttimevaluesplitfp1.Contains(":") == true)
                                        {
                                            string[] FinalDepttimevaluespl = FinalDepttimevaluesplitfp1.Split(':');
                                            FinalDepttimevaluesplif2 = FinalDepttimevaluespl[0] + "." + FinalDepttimevaluespl[1];
                                            FpSpreadstage.Sheets[0].Cells[FpSpreadstage.Sheets[0].RowCount - 1, 3].Text = FinalDepttimevaluesplif2;
                                        }
                                        else
                                        {
                                            FpSpreadstage.Sheets[0].Cells[FpSpreadstage.Sheets[0].RowCount - 1, 3].Text = FinalDepttimevaluesplitfp1;
                                        }

                                    }
                                    else
                                    {
                                        FpSpreadstage.Sheets[0].Cells[FpSpreadstage.Sheets[0].RowCount - 1, 3].Text = viewdata1["Dep_Time"].ToString();
                                    }
                                    FpSpreadstage.Sheets[0].Cells[FpSpreadstage.Sheets[0].RowCount - 1, 4].Text = waitingtimefp1;
                                    FpSpreadstage.Sheets[0].Cells[FpSpreadstage.Sheets[0].RowCount - 1, 5].Text = Address1;
                                    //FpSpreadstage.Sheets[0].Cells[FpSpreadstage.Sheets[0].RowCount - 1, 6].Text = Longitude1;

                                    //sprdMainstage.Sheets[0].PageSize = dsselectquery1.Rows.Count;
                                }


                            }


                        }
                    }
                }
            }
            catch
            {
            }
        }

    }
    protected void FpTransport_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string activerow = FpTransport.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpTransport.ActiveSheetView.ActiveColumn.ToString();
        Cellclick = true;
        Accordion1.SelectedIndex = 1;
    }
    protected void btnMainGo_Click(object sender, EventArgs e)
    {
        int i2 = 0;

        DAccess2 da = new DAccess2();
        DataSet ds = new DataSet();
        FpTransport.Visible = true;
        FpTransport.SaveChanges();

        string selectquery = "select Route_ID,Mdate,Stages,Stage_Name,Arr_Time,Dep_Time,Wait_Mins,Remarks,sess,Route_Name, Rou_From, Rou_To,Veh_ID,Address from RouteMaster";

        if (ddlserachby.Text != "-1")//filter by stage
        {
            selectquery = "select Route_ID,Mdate,Stages,Stage_Name,Arr_Time,Dep_Time,Wait_Mins,Remarks,sess,Route_Name,Rou_From,Rou_To,Veh_ID,Address from RouteMaster where stage_name in(select stage_id from stage_master where  Stage_Name = '" + ddlserachby.SelectedItem.Text.ToString() + "')";
        }
        if (ddlrouteID.Text != "-1")
        {
            selectquery = "select Route_ID,Mdate,Stages,Stage_Name,Arr_Time,Dep_Time,Wait_Mins,Remarks,sess,Route_Name,Rou_From,Rou_To,Veh_ID,Address from RouteMaster where Route_ID='" + ddlrouteID.SelectedItem.Text.ToString() + "'";

        }
        selectquery += " order by len(Route_ID),route_id";
        SqlDataAdapter daselectquery = new SqlDataAdapter(selectquery, con5);
        DataTable dsselectquery = new DataTable();
        daselectquery.Fill(dsselectquery);
        con5.Close();
        con5.Open();
        ArrayList l1 = new ArrayList();
        ArrayList l2 = new ArrayList();
        DataTable dt = new DataTable();
        DataView dv = new DataView();
        FpTransport.Sheets[0].RowCount = 0;
        for (int i1 = 0; i1 < dsselectquery.Rows.Count; i1++)
        {
            FpTransport.Visible = true;
            string Route = dsselectquery.Rows[i1]["Route_ID"].ToString();
            string session = dsselectquery.Rows[i1]["sess"].ToString();
            string Sess_Route = Route + "-" + session;
            string orderby = "";
            if (session.Trim() == "M")
                orderby = "  Dep_Time ";
            else
                orderby = " Arr_Time";
            if (l1.Contains(Sess_Route) == false)
            {

                string stagename = string.Empty;
                DataView dvview = new DataView();
                dsselectquery.DefaultView.RowFilter = "Route_ID='" + Route + "'and sess='" + session + "'";
                dvview = dsselectquery.DefaultView;
                dt = dvview.ToTable();
                dv = new DataView(dt);
                dv.Sort = orderby;
                foreach (DataRowView viewdata in dv)
                {
                    if (Has_Stage.Contains(Convert.ToString(viewdata["Stage_Name"])) == true)
                    {
                        string stage_name = Convert.ToString(GetCorrespondingKey(Convert.ToString(viewdata["Stage_Name"]), Has_Stage));
                        if (stagename == "")
                        {
                            stagename = stage_name.ToString();
                        }
                        else
                        {
                            stagename = stagename + "-" + stage_name.ToString();
                        }
                    }
                }
                FpTransport.Sheets[0].RowCount++;
                i2 = FpTransport.Sheets[0].RowCount - 1;
                string Rou_From_Value = string.Empty;
                string Rou_To_Value = string.Empty;
                string val = dsselectquery.Rows[i1]["Rou_From"].ToString();

                if (Has_Stage.Contains(Convert.ToString(dsselectquery.Rows[i1]["Rou_From"])) == true)
                {
                    Rou_From_Value = Convert.ToString(GetCorrespondingKey(Convert.ToString(dsselectquery.Rows[i1]["Rou_From"]), Has_Stage));
                }
                if (Has_Stage.Contains(Convert.ToString(dsselectquery.Rows[i1]["Rou_To"])) == true)
                {
                    Rou_To_Value = Convert.ToString(GetCorrespondingKey(Convert.ToString(dsselectquery.Rows[i1]["Rou_To"]), Has_Stage));
                }


                FpTransport.Sheets[0].Cells[i2, 0].Text = dsselectquery.Rows[i1]["Route_ID"].ToString();
                FpTransport.Sheets[0].Cells[i2, 1].Text = dsselectquery.Rows[i1]["sess"].ToString();
                FpTransport.Sheets[0].Cells[i2, 2].Text = Rou_From_Value + "-" + Rou_To_Value;
                FpTransport.Sheets[0].Cells[i2, 3].Text = stagename;
                FpTransport.Sheets[0].Cells[i2, 4].Text = dsselectquery.Rows[i1]["Stages"].ToString();
                FpTransport.Sheets[0].PageSize = FpTransport.Rows.Count;

            }

            l1.Add(Sess_Route);
            FpTransport.SaveChanges();
            dt.Clear();
            dv = null;
        }




        FpTransport.SaveChanges();
    }
    public void bindVehicleID()
    {
        Connection();
        ddlvehiclid.Items.Clear();
        ddlvehiclid.Items.Insert(0, new ListItem(" ", "-1"));
        string sql;
        sql = "select distinct Veh_ID from vehicle_master order by Veh_ID";
        ds = da.select_method_wo_parameter(sql, "txt");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ddlvehiclid.Items.Add(ds.Tables[0].Rows[i]["Veh_ID"].ToString());
            }
            ddlvehiclid.SelectedIndex = 0;

        }
        con.Close();
    }
    protected void tbvehicleid_TextChanged(object sender, EventArgs e)
    {

    }
    protected void ddlvehiclid_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnroute_Click(object sender, EventArgs e)
    {

    }
    protected void tbstages_TextChanged(object sender, EventArgs e)
    {


    }
    protected void tbfromstage1_TextChanged(object sender, EventArgs e)
    {

    }
    protected void ddlfromstage_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void tbtostage_TextChanged(object sender, EventArgs e)
    {

    }
    protected void ddltostage_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void FpSpreadstage_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

    }
    protected void FpSpreadstage_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = e.SheetView.ActiveRow.ToString();
            string actcol = e.SheetView.ActiveColumn.ToString();
            string Fp_getvalue = string.Empty;
            string fp_FinlValue = string.Empty;
            if (cellclick == false && Session["chk_upval"] == "")
            {


                for (int j = 0; j < Convert.ToInt16(FpSpreadstage.Sheets[0].RowCount); j++)
                {
                    actcol = e.SheetView.ActiveColumn.ToString();
                    string seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
                    string set = FpSpreadstage.GetEditValue(Convert.ToInt16(actrow), 1).ToString();
                    if (set != "System.Object")
                    {
                        fp_FinlValue = "select * from stage_master where Stage_id = '" + set + "'";
                        SqlDataAdapter dr_get = new SqlDataAdapter(fp_FinlValue, con);
                        DataSet dt_get = new DataSet();
                        dr_get.Fill(dt_get);
                        if (dt_get.Tables[0].Rows.Count > 0)
                        {
                            string address = string.Empty;
                            address = dt_get.Tables[0].Rows[0]["Address"].ToString();
                            FpSpreadstage.Sheets[0].Cells[Convert.ToInt16(actrow), 5].Text = address;
                        }
                    }
                }

                DataSet ds_new = new DataSet();
                ArrayList stage_new = new ArrayList();
                int stagecount;
                stagecount = Convert.ToInt16(tbstages.Text);
                string arr_value = tbfromstage1.Text;
                string arr_To_Value = tbtostage.Text;
                string actrow1 = e.SheetView.ActiveRow.ToString();
                string actcol1 = e.SheetView.ActiveColumn.ToString();
                string set1 = FpSpreadstage.GetEditValue(Convert.ToInt16(actrow1), 1).ToString();
                int act1 = Convert.ToInt32(actrow1) + 1;
                stage_new.Add(arr_value);
                stage_new.Add(arr_To_Value);
                stage_new.Add(set1);
                string Final_ArrayList = string.Empty;
                for (int arr = 0; arr < stage_new.Count; arr++)
                {
                    if (Final_ArrayList == "")
                    {
                        Final_ArrayList = stage_new[arr].ToString();
                    }
                    else
                    {
                        Final_ArrayList = Final_ArrayList + "," + stage_new[arr].ToString();
                    }
                }
                for (int j = 0; j < Convert.ToInt16(FpSpreadstage.Sheets[0].RowCount); j++)
                {
                    if (j == 0)
                    {

                    }
                    else if (j <= stagecount - 2)
                    {
                        if (j == act1)
                        {

                            con.Close();
                            con.Open();
                            string querylist = "";
                            querylist = "select * from Stage_Master where Stage_Name not in('" + Final_ArrayList + "')";
                            ds_new = dset.select_method_wo_parameter(querylist, "text");
                            if (ds_new.Tables[0].Rows.Count > 0)
                            {
                                int act = Convert.ToInt32(actrow1);
                                FarPoint.Web.Spread.ComboBoxCellType cm = new FarPoint.Web.Spread.ComboBoxCellType();
                                cm.AutoPostBack = true;
                                cm.ShowButton = true;
                                cm.UseValue = true;
                                cm.DataSource = ds_new.Tables[0];
                                cm.DataTextField = "Stage_Name";
                                cm.DataValueField = "Stage_id";
                                FpSpreadstage.Sheets[0].Cells[act1, 1].Text = ds_new.Tables[0].Rows[0]["Stage_Name"].ToString();
                                FpSpreadstage.Sheets[0].Cells[act1, 1].Value = ds_new.Tables[0].Rows[0]["Stage_id"].ToString();
                                FpSpreadstage.Sheets[0].Cells[act1, 5].Text = ds_new.Tables[0].Rows[0]["Address"].ToString();
                                FpSpreadstage.Sheets[0].Cells[act1, 1].CellType = cm;
                                act1++;


                            }
                        }

                    }
                    else
                    {

                    }
                }
            }
            else
            {
                actcol = e.SheetView.ActiveColumn.ToString();
                string seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
                string set = FpSpreadstage.GetEditValue(Convert.ToInt16(actrow), 1).ToString();
                if (set != "System.Object")
                {
                    fp_FinlValue = "select * from stage_master where Stage_id = '" + set + "'";
                    SqlDataAdapter dr_get = new SqlDataAdapter(fp_FinlValue, con);
                    DataSet dt_get = new DataSet();
                    dr_get.Fill(dt_get);
                    if (dt_get.Tables[0].Rows.Count > 0)
                    {
                        string address = string.Empty;
                        address = dt_get.Tables[0].Rows[0]["Address"].ToString();
                        FpSpreadstage.Sheets[0].Cells[Convert.ToInt16(actrow), 5].Text = address;
                    }
                }
            }

        }
        catch
        {

        }



    }
    protected void sprdMainstage_ButtonCommand(object sender, EventArgs e)
    {

    }
    protected void Buttonsave_Click(object sender, EventArgs e)
    {
        Labelvalidation.Visible = false;
        string Stage_new = string.Empty;
        string temp2 = string.Empty;
        string temp3 = string.Empty;
        string wating = string.Empty;
        string wating1 = string.Empty;
        string arrtime = string.Empty;
        string arrtime1 = string.Empty;
        string Address_Value = string.Empty;
        string Address_Value1 = string.Empty;
        string stagename = "Stage";
        int stagecount;
        int tempcount = -1;
        int temptemp;
        stagecount = FpSpreadstage.Sheets[0].RowCount;
        int k = stagecount;
        int stagecount1;
        stagecount1 = stagecount - 1;
        FpSpreadstage.SaveChanges();
        sprdMainstage.SaveChanges();
        sprdMainstage.Sheets[0].RowCount = FpSpreadstage.Sheets[0].RowCount;
        for (int i2 = 0; i2 < stagecount; i2++)
        {
            tempcount++;
            k = k - 1;

            //FpSpreadstage.Sheets[0].Cells[i2, 0] = temp2.ToString();
            temp2 = FpSpreadstage.Sheets[0].Cells[i2, 1].Text.ToString();
            temp3 = FpSpreadstage.Sheets[0].Cells[i2, 1].Value.ToString();
            wating = FpSpreadstage.Sheets[0].Cells[i2, 4].Text.ToString();
            //wating1 = FpSpreadstage.Sheets[0].Cells[i2, 4].Value.ToString();
            Address_Value = FpSpreadstage.Sheets[0].Cells[i2, 5].Text.ToString();
            //Address_Value1 = FpSpreadstage.Sheets[0].Cells[i2, 5].Value.ToString();
            //Longitude_Value = FpSpreadstage.Sheets[0].Cells[i2, 6].Text.ToString();
            ErrorLabel.Visible = false;
            temptemp = tempcount + 1;
            sprdMainstage.Sheets[0].Cells[i2, 0].Text = stagename + Convert.ToString(tempcount + 1);
            if (sprdMainstage.Sheets[0].Cells[i2, 0].Text == "Stage1")
            {
                sprdMainstage.Sheets[0].Cells[i2, 2].Text = "-";
                sprdMainstage.Sheets[0].Cells[i2, 2].Locked = true;
            }

            sprdMainstage.Sheets[0].Cells[k, 1].Text = temp2;
            sprdMainstage.Sheets[0].Cells[k, 1].Tag = temp3;
            sprdMainstage.Sheets[0].Cells[k, 4].Text = wating.ToString();
            //sprdMainstage.Sheets[0].Cells[k, 4].Value = wating1.ToString();
            sprdMainstage.Sheets[0].Cells[k, 5].Text = Address_Value;
            //sprdMainstage.Sheets[0].Cells[k, 5].Value = Address_Value1;
            ////sprdMainstage.Sheets[0].Cells[k, 6].Text = Longitude_Value;
            arrtime = sprdMainstage.Sheets[0].Cells[i2, 3].Text.ToString();

            if (stagecount == temptemp)
            {
                sprdMainstage.Sheets[0].Cells[i2, 3].Text = "Halt";
                sprdMainstage.Sheets[0].Cells[i2, 3].Locked = true;
            }
            sprdMainstage.SaveChanges();


        }


    }
    protected void Buttonnew_Click(object sender, EventArgs e)
    {

    }
    protected void ButtonsaveRoute_Click(object sender, EventArgs e)
    {

        int fd = 0;

        int fyy = 0;
        int fm = 0;
        string dt = "", dt1 = "";
        try
        {

            if (ddlvehiclid.SelectedItem.Text == " ")
            {
                Labelvalidation.Visible = true;
                Labelvalidation.Text = "Select Vehicle-ID";
                return;
            }
            else
            {

            }
            if (tbrouteid.Text == "")
            {
                Labelvalidation.Visible = true;
                Labelvalidation.Text = "Select Route-ID";
                return;
            }
            if (tbdate.Text != "")
            {
                Labelvalidation.Visible = false;
                fd = int.Parse((tbdate.Text.Substring(0, 2).ToString()));
                fyy = int.Parse((tbdate.Text.Substring(6, 4).ToString()));
                fm = int.Parse((tbdate.Text.Substring(3, 2).ToString()));
                dt1 = fyy + "-" + fm + "-" + fd;

            }
            else
            {
                Labelvalidation.Visible = true;
                Labelvalidation.Text = "Select Date";
                return;
            }

            string tbfrom_value = GetFunction("select stage_id from stage_master where stage_name='" + tbfromstage1.Text + "'");
            string tbto_value = GetFunction("select stage_id from stage_master where stage_name='" + tbtostage.Text + "'");

            string updatequery2 = "";
            updatequery2 = "select * from RouteMaster where Route_ID='" + tbrouteid.Text + "'";
            SqlDataAdapter daupdatequery2 = new SqlDataAdapter(updatequery2, con5);
            DataTable dsupdatequery2 = new DataTable();
            daupdatequery2.Fill(dsupdatequery2);
            con5.Close();
            con5.Open();
            string stagename = string.Empty;
            string Dept_Time = string.Empty;
            string Address_up = string.Empty;
            string valueArr = string.Empty;
            string valuedept = string.Empty;
            //string Longitude_up = string.Empty;
            string sess = string.Empty;

            DataView dvview = new DataView();
            dsupdatequery2.DefaultView.RowFilter = "sess='M'";
            dvview = dsupdatequery2.DefaultView;

            if (FpSpreadstage.Sheets[0].RowCount > 0)
            {

                lblrouteadd.Text = "Add";
                Labelvalidation.Visible = false;
                for (int i = 0; i < FpSpreadstage.Sheets[0].RowCount; i++)
                {
                    string[] valrwait2;
                    if (btnsave2.Text == "Update")
                    {
                        //stagename = dvview[i]["Stage_Name"].ToString();
                        //Dept_Time = dvview[i]["Dep_Time"].ToString();
                        //Address_up = dvview[i]["Address"].ToString();
                        ////Longitude_up = dvview[i]["Longitude"].ToString();
                        //sess = dvview[i]["sess"].ToString();

                    }
                    FpSpreadstage.SaveChanges();
                    string valuestage = FpSpreadstage.Sheets[0].Cells[i, 0].Text.ToString();
                    string valuestagename = FpSpreadstage.Sheets[0].Cells[i, 1].Text.ToString();
                    string valuestagename_val = FpSpreadstage.Sheets[0].Cells[i, 1].Value.ToString();

                    if (FpSpreadstage.Sheets[0].Cells[i, 2].Text.ToString() != "" || FpSpreadstage.Sheets[0].Cells[i, 2].Text.ToString() == "-" || FpSpreadstage.Sheets[0].Cells[i, 2].Text.ToString() == "Halt")
                    {
                        valueArr = FpSpreadstage.Sheets[0].Cells[i, 2].Text.ToString();
                    }
                    else
                    {
                        Labelvalidation.Visible = true;
                        Labelvalidation.Text = "Enter the Arrival Time in Evening Session";
                        return;
                    }
                    if (FpSpreadstage.Sheets[0].Cells[i, 3].Text.ToString() != "")
                    {
                        valuedept = FpSpreadstage.Sheets[0].Cells[i, 3].Text.ToString();
                    }
                    else
                    {
                        Labelvalidation.Visible = true;
                        Labelvalidation.Text = "Enter the Departure Time in Evening Session";
                        return;
                    }

                    string valuewait = FpSpreadstage.Sheets[0].Cells[i, 4].Text.ToString();
                    string Address = FpSpreadstage.Sheets[0].Cells[i, 5].Text.ToString();
                    //string Longitude = FpSpreadstage.Sheets[0].Cells[i, 6].Text.ToString();
                    if (valuewait == "NaN")
                    {
                        valuewait = "0.00";
                    }
                    if (valuewait == "0.NaN")
                    {
                        valuewait = "0.00";
                    }
                    if (valuedept == "Halt")
                    {
                        valuedept = "Halt";
                    }
                    string[] fp1waiting = valuewait.Split('.');
                    string waitingtimefp1 = fp1waiting[1].ToString();

                    if (valueArr == "-")
                    {
                        valueArr = "Halt";
                    }
                    string queryspread = "";

                    if (btnsave2.Text == "Update" && i == 0)
                    {
                        con.Close();
                        con.Open();

                        queryspread = "delete from routemaster where Route_ID='" + tbrouteid.Text + "' ";
                        SqlCommand cmdstag = new SqlCommand(queryspread, con);
                        cmdstag.ExecuteNonQuery();
                    }

                    con.Close();
                    con.Open();

                    queryspread = "insert into routemaster(Route_ID,Mdate,Stages,Stage_Name,Arr_Time,Dep_Time,Wait_Mins,Remarks,sess,Route_Name,Rou_From,Rou_To,Veh_ID,Address) values('" + tbrouteid.Text + "','" + dt1 + "','" + tbstages.Text + "','" + valuestagename_val + "','" + valueArr + "','" + valuedept + "','" + waitingtimefp1 + "','0','M','0','" + tbfrom_value + "','" + tbto_value + "','" + ddlvehiclid.SelectedItem.Text.ToString() + "','" + Address + "')";
                    SqlCommand cmdsta = new SqlCommand(queryspread, con);
                    cmdsta.ExecuteNonQuery();

                }

            }


            string stagename1 = string.Empty;
            string Dept_Time1 = string.Empty;
            string Address_up1 = string.Empty;
            string valueArr1 = string.Empty;
            //string Longitude_up1 = string.Empty;
            string sess1 = string.Empty;
            DataView dvview1 = new DataView();
            dsupdatequery2.DefaultView.RowFilter = "sess='A'";
            dvview1 = dsupdatequery2.DefaultView;

            if (sprdMainstage.Sheets[0].RowCount > 0)
            {
                for (int i = 0; i < sprdMainstage.Sheets[0].RowCount; i++)
                {
                    if (btnsave2.Text == "Update")
                    {
                        //stagename1 = dvview1[i]["Stage_Name"].ToString();
                        //Dept_Time1 = dvview1[i]["Dep_Time"].ToString();
                        //Address_up1 = dvview[i]["Address"].ToString();
                        ////Longitude_up1 = dvview[i]["Longitude"].ToString();
                        //sess1 = dvview1[i]["sess"].ToString();

                    }
                    sprdMainstage.SaveChanges();
                    string valuestage1 = sprdMainstage.Sheets[0].Cells[i, 0].Text.ToString();
                    string valuestagename1 = sprdMainstage.Sheets[0].Cells[i, 1].Text.ToString();
                    string valuestagename1_val = sprdMainstage.Sheets[0].Cells[i, 1].Tag.ToString();

                    if (sprdMainstage.Sheets[0].Cells[i, 2].Text.ToString() != "" || sprdMainstage.Sheets[0].Cells[i, 2].Text.ToString() == "-" || sprdMainstage.Sheets[0].Cells[i, 2].Text.ToString() == "Halt")
                    {
                        valueArr1 = sprdMainstage.Sheets[0].Cells[i, 2].Text.ToString();
                    }
                    else
                    {
                        Labelvalidation.Visible = true;
                        Labelvalidation.Text = "Enter the Time in Morning session";
                        return;
                    }
                    string valuedept1 = sprdMainstage.Sheets[0].Cells[i, 3].Text.ToString();
                    string valuewait1 = sprdMainstage.Sheets[0].Cells[i, 4].Text.ToString();
                    string Address = sprdMainstage.Sheets[0].Cells[i, 5].Text.ToString();
                    //string Longitude = sprdMainstage.Sheets[0].Cells[i, 6].Text.ToString();
                    if (valuewait1 == "NaN")
                    {
                        valuewait1 = "0.00";
                    }
                    if (valuewait1 == "0.NaN")
                    {
                        valuewait1 = "0.00";
                    }
                    if (valuedept1 == "Halt")
                    {
                        valuedept1 = "Halt";
                    }
                    string[] fp2waiting = valuewait1.Split('.');
                    string waitingtime = fp2waiting[1].ToString();
                    con.Close();
                    con.Open();
                    string queryspread1 = "";
                    //if (btnsave2.Text == "Update" && i == 0)
                    //{
                    //    con.Close();
                    //    con.Open();
                    //    SqlCommand cmd_delete = new SqlCommand("delete from routemaster where Route_ID='" + tbrouteid.Text + "' ", con);
                    //    cmd_delete.ExecuteNonQuery();
                    //}

                    //if (btnsave2.Text == "Save")
                    //{
                    queryspread1 = "insert into routemaster(Route_ID,Mdate,Stages,Stage_Name,Arr_Time,Dep_Time,Wait_Mins,Remarks,sess,Route_Name,Rou_From,Rou_To,Veh_ID,Address) values('" + tbrouteid.Text + "','" + dt1 + "','" + tbstages.Text + "','" + valuestagename1_val + "','" + valueArr1 + "','" + valuedept1 + "','" + waitingtime + "','0','A','0','" + tbfrom_value + "','" + tbto_value + "','" + ddlvehiclid.SelectedItem.Text.ToString() + "','" + Address + "')";
                    con.Close();
                    con.Open();
                    SqlCommand cmdstag1 = new SqlCommand(queryspread1, con);
                    cmdstag1.ExecuteNonQuery();



                }


            }
            //update Vehicle_Master=======================================
            //Start==================
            Labelvalidation.Visible = false;
            con.Close();
            con.Open();
            string queryspread3 = "";
            queryspread3 = "update Vehicle_Master set Route='" + tbrouteid.Text + "' where Veh_ID = '" + ddlvehiclid.SelectedItem.Text.ToString() + "'";
            SqlCommand cmdstag3 = new SqlCommand(queryspread3, con);
            cmdstag3.ExecuteNonQuery();
            con.Close();
            //End===============================

            clear();

            if (btnsave2.Text == "Update")
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Updated successfully')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
            }

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record Not Saved')", true);
        }


    }
    protected void tbrouteid_TextChanged(object sender, EventArgs e)
    {
        if (tbrouteid.Text == "")
        {
            Labelvalidation.Visible = true;
            Labelvalidation.Text = "Select RouteID";
            return;
        }
        else
        {

            string chk_valida = string.Empty;
            chk_valida = "select * from routemaster where Route_ID = '" + tbrouteid.Text + "'";
            DataSet get_data = new DataSet();
            get_data = dset.select_method_wo_parameter(chk_valida, "text");
            if (get_data.Tables[0].Rows.Count > 0)
            {
                Labelvalidation.Visible = true;
                Labelvalidation.Text = "Already RouteID Set the Same Vehicle ";
                return;

            }
            else
            {
                string chk_valida1 = string.Empty;
                chk_valida1 = "select * from routemaster where Veh_ID = '" + ddlvehiclid.SelectedItem.Text.ToString() + "'";
                DataSet get_data1 = new DataSet();
                get_data = dset.select_method_wo_parameter(chk_valida1, "text");
                if (get_data.Tables[0].Rows.Count > 0)
                {
                    Labelvalidation.Visible = true;
                    Labelvalidation.Text = "Already Set the route in same Vehicle ";
                    return;
                }
                else
                {
                    Labelvalidation.Visible = false;
                    con.Open();
                    string queryspread2 = "";
                    queryspread2 = "update Vehicle_Master set Route='" + tbrouteid.Text + "' where Veh_ID = '" + ddlvehiclid.SelectedItem.Text.ToString() + "'";
                    SqlCommand cmdstag1 = new SqlCommand(queryspread2, con);
                    cmdstag1.ExecuteNonQuery();
                    con.Close();
                }

            }

        }

    }
    protected void Buttondelete_Click(object sender, EventArgs e)
    {
        //f (tbvehiid.Text.Trim() != "")
        if (ddlvehiclid.SelectedItem.Text.ToString() != "")
            mpemsgboxdelete.Show();
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        mpemsgboxdelete.Hide();
        try
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
            sqlcmd = "delete from RouteMaster where Route_ID='" + tbrouteid.Text + "'";
            int n = dset.update_method_wo_parameter(sqlcmd, "n");
            clear();
        }
        catch
        {

        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mpemsgboxdelete.Hide();
    }
    protected void tbDeptTime_TextChanged(object sender, EventArgs e)
    {

    }
    protected void ddlrouteID_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void LoadMainEnquiry()
    {

    }
    public void clear()
    {
        tbrouteid.Text = "";
        tbstages.Text = "";
        //tbfromstage1.Text = "";
        //tbtostage.Text = "";
        //ddlvehiclid.Items.Clear();
        Buttonsave.Enabled = true;
        sprdMainstage.SaveChanges();
        FpSpreadstage.Rows.Count = 0;
        sprdMainstage.Rows.Count = 0;
    }
    protected void btnnew_Click(object sender, EventArgs e)
    {
        clear();
        btnsave2.Text = "Save";
        tbrouteid.Enabled = true;
        Buttondelete.Enabled = false;
        lblrouteadd.Text = "Add";
        Btn_addabove.Visible = false;
        Btn_Addbelow.Visible = false;
        Btn_Delete_Row.Visible = false;
        //Buttonsave.Text = "Save";
    }
    protected void btnmaingoStage_Click(object sender, EventArgs e)
    {

        int stagecount;
        stagecount = Convert.ToInt16(tbstages.Text);
        loadstage();
        pagerow(stagecount);
        FpSpreadstage.SaveChanges();
        for (int i1 = 0; i1 < stagecount; i1++)
        {


            //FpSpreadstage.Sheets[0].RowCount = FpSpreadstage.Sheets[0].RowCount + 1;
            if (i1 == 0)
            {
                //FpSpreadstage.Sheets[0].Cells[i1, 1].Text = tbfromstage1.Text;
                //sprdMainstage.Sheets[0].Cells[i1, 1].Locked = true;

            }
            else if (i1 <= stagecount - 2)
            {
                if (i1 == 1)
                {
                    identifiStage();
                }
            }
            else
            {
                //FpSpreadstage.Sheets[0].Cells[i1, 1].Text = tbtostage.Text;

            }

        }
        //for (int i1 = 0; i1 < stagecount; i1++)
        //{


        //    FpSpreadstage.Sheets[0].RowCount = FpSpreadstage.Sheets[0].RowCount + 1;
        //    //if (i1 == 0)
        //    //{
        //    //    //FpSpreadstage.Sheets[0].Cells[i1, 1].Text = tbfromstage1.Text;
        //    //    //sprdMainstage.Sheets[0].Cells[i1, 1].Locked = true;

        //    //}
        //    //else if (i1 <= stagecount - 2)
        //    //{

        //    //    FpSpreadstage.Sheets[0].Cells[i1, 1].Text = "";
        //    //    sprdMainstage.Sheets[0].Cells[i1, 1].Text = "";
        //    //}
        //    //else
        //    //{
        //    //    //FpSpreadstage.Sheets[0].Cells[i1, 1].Text = tbtostage.Text;

        //    //}

        //}
    }
    public void pagerow(int count)
    {
        FpSpreadstage.Sheets[0].RowCount = 0;
        sprdMainstage.Sheets[0].RowCount = 0;
        int stagecount;
        stagecount = count;

        for (int i = 0; i < stagecount; i++)
        {
            FpSpreadstage.Sheets[0].RowCount = FpSpreadstage.Sheets[0].RowCount + 1;
            sprdMainstage.Sheets[0].RowCount = sprdMainstage.Sheets[0].RowCount + 1;


        }


        for (int i1 = 0; i1 < stagecount; i1++)
        {
            if (i1 == 0)
            {
                FpSpreadstage.Sheets[0].Cells[i1, 0].Text = "Stage" + (i1 + 1);
                FpSpreadstage.Sheets[0].Cells[i1, 2].Text = "-";
                FpSpreadstage.Sheets[0].Cells[i1, 2].Locked = true;


                sprdMainstage.Sheets[0].Cells[i1, 0].Text = "Stage" + (i1 + 1);
                sprdMainstage.Sheets[0].Cells[i1, 2].Text = "-";
                sprdMainstage.Sheets[0].Cells[i1, 2].Locked = true;

            }
            else if (i1 <= stagecount - 2)
            {

                FpSpreadstage.Sheets[0].Cells[i1, 0].Text = "Stage" + (i1 + 1);
                sprdMainstage.Sheets[0].Cells[i1, 0].Text = "Stage" + (i1 + 1);
            }
            else
            {
                FpSpreadstage.Sheets[0].Cells[i1, 0].Text = "End";
                FpSpreadstage.Sheets[0].Cells[i1, 3].Text = "Halt";
                FpSpreadstage.Sheets[0].Cells[i1, 3].Locked = true;
                sprdMainstage.Sheets[0].Cells[i1, 0].Text = "End";
                sprdMainstage.Sheets[0].Cells[i1, 3].Text = "Halt";
                sprdMainstage.Sheets[0].Cells[i1, 3].Locked = true;

            }



        }



    }
    int act_row = 0;
    string click_val = string.Empty;
    protected void Btn_addabove_Click(object sender, EventArgs e)
    {
        //FpSpreadstage.Sheets[0].RowCount = FpSpreadstage.Sheets[0].RowCount + 1;
        click_val = "Above";

        string activerow = FpSpreadstage.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpreadstage.ActiveSheetView.ActiveColumn.ToString();
        cellclick = true;
        Session["chk_upval"] = chk_up++;
        if (activerow != "")
        {
            act_row = Convert.ToInt32(activerow);
            Add_New_Row();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please click on any row then proceed.')", true);
        }
    }
    protected void Btn_addbelow_Click(object sender, EventArgs e)
    {
        click_val = "Below";

        string activerow = FpSpreadstage.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpreadstage.ActiveSheetView.ActiveColumn.ToString();
        cellclick = true;
        Session["chk_upval"] = chk_up++;
        if (activerow != "")
        {
            act_row = Convert.ToInt32(activerow);
            Add_New_Row();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please click on any row then proceed.')", true);
        }
    }
    protected void Btn_Delete_Row_Click(object sender, EventArgs e)
    {
        string activerow = FpSpreadstage.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpreadstage.ActiveSheetView.ActiveColumn.ToString();
        cellclick = true;
        Session["chk_upval"] = chk_up++;
        if (activerow != "")
        {
            act_row = Convert.ToInt32(activerow);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please click on any row then proceed.')", true);
        }

        sprdMainstage.Sheets[0].RowCount = 0;
        sprdMainstage.SaveChanges();

        DataTable dt_new = new DataTable();
        DataColumn dc_new = new DataColumn();

        dc_new = new DataColumn();
        dc_new.ColumnName = "Stage";
        dt_new.Columns.Add(dc_new);

        dc_new = new DataColumn();
        dc_new.ColumnName = "Address";
        dt_new.Columns.Add(dc_new);

        dc_new = new DataColumn();
        dc_new.ColumnName = "Arrive";
        dt_new.Columns.Add(dc_new);

        dc_new = new DataColumn();
        dc_new.ColumnName = "Dep";
        dt_new.Columns.Add(dc_new);

        dc_new = new DataColumn();
        dc_new.ColumnName = "Wait";
        dt_new.Columns.Add(dc_new);

        DataRow dr_new;
        FpSpreadstage.SaveChanges();

        for (int i = 0; i < FpSpreadstage.Sheets[0].RowCount; i++)
        {
            string stage = FpSpreadstage.Sheets[0].Cells[i, 1].Text.ToString();
            string arriv = FpSpreadstage.Sheets[0].Cells[i, 2].Text.ToString();
            string dep = FpSpreadstage.Sheets[0].Cells[i, 3].Text.ToString();
            string wait = FpSpreadstage.Sheets[0].Cells[i, 4].Text.ToString();
            string address = FpSpreadstage.Sheets[0].Cells[i, 5].Text.ToString();

            if (i != act_row)
            {
                dr_new = dt_new.NewRow();
                dr_new["Stage"] = stage;
                dr_new["Arrive"] = arriv;
                dr_new["Dep"] = dep;
                dr_new["Wait"] = wait;
                dr_new["Address"] = address;
                dt_new.Rows.Add(dr_new);
            }
        }

        pagerow(dt_new.Rows.Count);

        for (int k = 0; k < dt_new.Rows.Count; k++)
        {
            if (dt_new.Rows[k]["Dep"].ToString() == "" && dt_new.Rows[k]["Arrive"].ToString() == "")
            {
                FpSpreadstage.Sheets[0].Cells[k, 1].Text = dt_new.Rows[k]["Stage"].ToString();
            }
            else
            {
                DataSet ds_arr = new DataSet();
                ArrayList Stage = new ArrayList();
                int stagecount;
                stagecount = Convert.ToInt16(tbstages.Text);
                string arr_value = tbfromstage1.Text;
                string arr_To_Value = tbtostage.Text;
                Stage.Add(arr_value);
                Stage.Add(arr_To_Value);
                string Final_ArrayList = string.Empty;
                con.Close();
                con.Open();
                string querylist = "";
                querylist = "select * from Stage_Master";
                ds_arr = dset.select_method_wo_parameter(querylist, "text");
                if (ds_arr.Tables[0].Rows.Count > 0)
                {
                    FarPoint.Web.Spread.ComboBoxCellType cm1 = new FarPoint.Web.Spread.ComboBoxCellType();
                    cm1.AutoPostBack = true;
                    cm1.ShowButton = true;
                    cm1.UseValue = true;
                    cm1.DataSource = ds_arr.Tables[0];
                    cm1.DataTextField = "Stage_Name";
                    cm1.DataValueField = "Stage_id";
                    for (int ch = 0; ch < ds_arr.Tables[0].Rows.Count; ch++)
                    {
                        if (dt_new.Rows[k]["Stage"].ToString() == ds_arr.Tables[0].Rows[ch]["Stage_Name"].ToString())
                        {
                            FpSpreadstage.Sheets[0].Cells[k, 1].Text = ds_arr.Tables[0].Rows[ch]["Stage_Name"].ToString();
                            FpSpreadstage.Sheets[0].Cells[k, 1].Value = ds_arr.Tables[0].Rows[ch]["Stage_id"].ToString();
                            FpSpreadstage.Sheets[0].Cells[k, 5].Text = ds_arr.Tables[0].Rows[ch]["Address"].ToString();
                            FpSpreadstage.Sheets[0].Cells[k, 1].CellType = cm1;
                        }
                    }

                }
            }
            FpSpreadstage.Sheets[0].Cells[k, 2].Text = dt_new.Rows[k]["Arrive"].ToString();

            if (k == dt_new.Rows.Count - 1)
            {
                FpSpreadstage.Sheets[0].Cells[k, 3].Text = "Halt";
            }
            else
            {
                if (dt_new.Rows[k]["Dep"].ToString() != "Halt")
                {
                    FpSpreadstage.Sheets[0].Cells[k, 3].Text = dt_new.Rows[k]["Dep"].ToString();
                }
            }
            FpSpreadstage.Sheets[0].Cells[k, 4].Text = dt_new.Rows[k]["Wait"].ToString();
            if (dt_new.Rows[k]["Dep"].ToString() == "" && dt_new.Rows[k]["Arrive"].ToString() == "")
            {
                FpSpreadstage.Sheets[0].Cells[k, 5].Text = dt_new.Rows[k]["Address"].ToString();
            }
        }
    }
    void Add_New_Row()
    {
        sprdMainstage.Sheets[0].RowCount = 0;
        sprdMainstage.SaveChanges();

        DataTable dt_new = new DataTable();
        DataColumn dc_new = new DataColumn();

        dc_new = new DataColumn();
        dc_new.ColumnName = "Stage";
        dt_new.Columns.Add(dc_new);

        dc_new = new DataColumn();
        dc_new.ColumnName = "Address";
        dt_new.Columns.Add(dc_new);

        dc_new = new DataColumn();
        dc_new.ColumnName = "Arrive";
        dt_new.Columns.Add(dc_new);

        dc_new = new DataColumn();
        dc_new.ColumnName = "Dep";
        dt_new.Columns.Add(dc_new);

        dc_new = new DataColumn();
        dc_new.ColumnName = "Wait";
        dt_new.Columns.Add(dc_new);

        DataRow dr_new;
        FpSpreadstage.SaveChanges();

        for (int i = 0; i < FpSpreadstage.Sheets[0].RowCount; i++)
        {
            string stage = FpSpreadstage.Sheets[0].Cells[i, 1].Text.ToString();
            string arriv = FpSpreadstage.Sheets[0].Cells[i, 2].Text.ToString();
            string dep = FpSpreadstage.Sheets[0].Cells[i, 3].Text.ToString();
            string wait = FpSpreadstage.Sheets[0].Cells[i, 4].Text.ToString();
            string address = FpSpreadstage.Sheets[0].Cells[i, 5].Text.ToString();

            if (i == act_row && click_val == "Above")
            {
                dr_new = dt_new.NewRow();
                dr_new["Stage"] = "";
                dr_new["Address"] = "";
                dr_new["Arrive"] = "";
                dr_new["Dep"] = "";
                dr_new["Wait"] = "";
                dt_new.Rows.Add(dr_new);
            }

            dr_new = dt_new.NewRow();
            dr_new["Stage"] = stage;
            dr_new["Address"] = address;
            dr_new["Arrive"] = arriv;
            dr_new["Dep"] = dep;
            dr_new["Wait"] = wait;

            if (act_row != 0)
            {
                if (i < act_row && click_val == "Above")
                {
                    dr_new["Stage"] = stage;
                    dr_new["Address"] = address;
                    dr_new["Arrive"] = arriv;
                    dr_new["Dep"] = dep;
                    dr_new["Wait"] = wait;
                }
                else if (i <= act_row && click_val == "Below")
                {
                    dr_new["Stage"] = stage;
                    dr_new["Address"] = address;
                    dr_new["Arrive"] = arriv;
                    dr_new["Dep"] = dep;
                    dr_new["Wait"] = wait;
                }
                else
                {
                    dr_new["Stage"] = stage;
                    dr_new["Address"] = address;
                    dr_new["Arrive"] = arriv;
                    dr_new["Dep"] = dep;
                    dr_new["Wait"] = wait;
                }
            }
            else
            {
                dr_new["Stage"] = stage;
                dr_new["Address"] = address;
                dr_new["Arrive"] = arriv;
                dr_new["Dep"] = dep;
                dr_new["Wait"] = wait;
            }

            dt_new.Rows.Add(dr_new);

            if (i == act_row && click_val == "Below")
            {
                dr_new = dt_new.NewRow();
                dr_new["Stage"] = "";
                dr_new["Address"] = "";
                dr_new["Arrive"] = "";
                dr_new["Dep"] = "";
                dr_new["Wait"] = "";
                dt_new.Rows.Add(dr_new);
            }
        }

        pagerow(dt_new.Rows.Count);

        for (int k = 0; k < dt_new.Rows.Count; k++)
        {

            if (dt_new.Rows[k]["Dep"].ToString() == "" && dt_new.Rows[k]["Arrive"].ToString() == "")
            {
                FpSpreadstage.Sheets[0].Cells[k, 1].Text = dt_new.Rows[k]["Stage"].ToString();
            }
            else
            {
                DataSet ds_arr = new DataSet();
                ArrayList Stage = new ArrayList();
                int stagecount;
                stagecount = Convert.ToInt16(tbstages.Text);
                string arr_value = tbfromstage1.Text;
                string arr_To_Value = tbtostage.Text;
                Stage.Add(arr_value);
                Stage.Add(arr_To_Value);
                string Final_ArrayList = string.Empty;
                con.Close();
                con.Open();
                string querylist = "";
                querylist = "select * from Stage_Master";
                ds_arr = dset.select_method_wo_parameter(querylist, "text");
                if (ds_arr.Tables[0].Rows.Count > 0)
                {
                    FarPoint.Web.Spread.ComboBoxCellType cm1 = new FarPoint.Web.Spread.ComboBoxCellType();
                    cm1.AutoPostBack = true;
                    cm1.ShowButton = true;
                    cm1.UseValue = true;
                    cm1.DataSource = ds_arr.Tables[0];
                    cm1.DataTextField = "Stage_Name";
                    cm1.DataValueField = "Stage_id";
                    for (int ch = 0; ch < ds_arr.Tables[0].Rows.Count; ch++)
                    {
                        if (dt_new.Rows[k]["Stage"].ToString() == ds_arr.Tables[0].Rows[ch]["Stage_Name"].ToString())
                        {
                            FpSpreadstage.Sheets[0].Cells[k, 1].Text = ds_arr.Tables[0].Rows[ch]["Stage_Name"].ToString();
                            FpSpreadstage.Sheets[0].Cells[k, 1].Value = ds_arr.Tables[0].Rows[ch]["Stage_id"].ToString();
                            FpSpreadstage.Sheets[0].Cells[k, 5].Text = ds_arr.Tables[0].Rows[ch]["Address"].ToString();
                            FpSpreadstage.Sheets[0].Cells[k, 1].CellType = cm1;
                        }
                    }

                }
            }
            FpSpreadstage.Sheets[0].Cells[k, 2].Text = dt_new.Rows[k]["Arrive"].ToString();

            if (k == dt_new.Rows.Count - 1)
            {
                FpSpreadstage.Sheets[0].Cells[k, 3].Text = "Halt";
            }
            else
            {
                if (dt_new.Rows[k]["Dep"].ToString() != "Halt")
                {
                    FpSpreadstage.Sheets[0].Cells[k, 3].Text = dt_new.Rows[k]["Dep"].ToString();
                }
            }
            FpSpreadstage.Sheets[0].Cells[k, 4].Text = dt_new.Rows[k]["Wait"].ToString();
            if (dt_new.Rows[k]["Dep"].ToString() == "" && dt_new.Rows[k]["Arrive"].ToString() == "")
            {
                FpSpreadstage.Sheets[0].Cells[k, 5].Text = dt_new.Rows[k]["Address"].ToString();
            }
            //
        }
    }



    public string GetFunction(string Att_strqueryst)
    {

        string sqlstr;
        sqlstr = Att_strqueryst;
        getsql.Close();
        getsql.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, getsql);
        SqlDataReader drnew;
        SqlCommand cmd = new SqlCommand(sqlstr);
        cmd.Connection = getsql;
        drnew = cmd.ExecuteReader();
        drnew.Read();

        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "";
        }
    }
    public bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
    {
        Double result;
        return Double.TryParse(val, NumberStyle,
            System.Globalization.CultureInfo.CurrentCulture, out result);
    }
    public void changeStageName(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        DataSet ds_new = new DataSet();
        ArrayList stage_new = new ArrayList();
        int stagecount;
        stagecount = Convert.ToInt16(tbstages.Text);
        //string arr_value = ddlfromstage.SelectedValue;
        //string arr_To_Value = ddltostage.SelectedValue;
        string actrow = e.SheetView.ActiveRow.ToString();
        string actcol = e.SheetView.ActiveColumn.ToString();
        string set = FpSpreadstage.GetEditValue(Convert.ToInt16(actrow), 1).ToString();
        //stage_new.Add(arr_value);
        //stage_new.Add(arr_To_Value);

    }

    [System.Web.Script.Services.ScriptMethod()]

    [System.Web.Services.WebMethod]
    public static List<string> GetListofCountries(string prefixText)
    {
        string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        using (SqlConnection sqlconn = new SqlConnection(cs))
        {

            sqlconn.Open();

            SqlCommand cmd = new SqlCommand("select Stage_id,Stage_Name,Address,District from stage_master where Stage_Name like '" + prefixText + "%' ", sqlconn);

            cmd.Parameters.AddWithValue("@Stage_Name", prefixText);

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            da.Fill(dt);

            List<string> CountryNames = new List<string>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                //CountryNames.Add(dt.Rows[i]["stud_name"].ToString() + "|" + dt.Rows[i]["roll_no"].ToString() + "|" + dt.Rows[i]["reg_no"].ToString() + "\n\n");
                CountryNames.Add(dt.Rows[i]["Stage_Name"].ToString());

            }

            return CountryNames;

        }

    }

    public void identifiStage()
    {
        DataSet ds_arr = new DataSet();
        ArrayList Stage = new ArrayList();
        int stagecount;
        stagecount = Convert.ToInt16(tbstages.Text);
        string arr_value = tbfromstage1.Text;
        string arr_To_Value = tbtostage.Text;
        Stage.Add(arr_value);
        Stage.Add(arr_To_Value);
        string Final_ArrayList = string.Empty;
        Session["chk_upval"] = "";
        chk_up = 0;
        //Session["chk_upval"] = chk_up;
        for (int arr = 0; arr < Stage.Count; arr++)
        {
            if (Final_ArrayList == "")
            {
                Final_ArrayList = Stage[arr].ToString();
            }
            else
            {
                Final_ArrayList = Final_ArrayList + "," + Stage[arr].ToString();
            }
        }

        for (int m = 0; m < stagecount; m++)
        {
            if (m == 0)
            {
                con.Close();
                con.Open();
                string querylist = "";
                querylist = "select * from Stage_Master where Stage_Name ='" + arr_value + "'";
                ds_arr = dset.select_method_wo_parameter(querylist, "text");
                if (ds_arr.Tables[0].Rows.Count > 0)
                {
                    FpSpreadstage.Sheets[0].Cells[m, 5].Text = ds_arr.Tables[0].Rows[0]["Address"].ToString();
                    FpSpreadstage.Sheets[0].Cells[m, 1].Text = ds_arr.Tables[0].Rows[0]["Stage_id"].ToString();
                }

                FpSpreadstage.Sheets[0].Cells[m, 1].Locked = true;

            }
            else if (m <= stagecount - 2)
            {
                con.Close();
                con.Open();
                string querylist = "";
                querylist = "select * from Stage_Master where Stage_Name not in('" + Final_ArrayList + "')";
                ds_arr = dset.select_method_wo_parameter(querylist, "text");
                if (ds_arr.Tables[0].Rows.Count > 0)
                {
                    FarPoint.Web.Spread.ComboBoxCellType cm = new FarPoint.Web.Spread.ComboBoxCellType();
                    cm.AutoPostBack = true;
                    cm.ShowButton = true;
                    cm.UseValue = true;
                    cm.DataSource = ds_arr.Tables[0];
                    cm.DataTextField = "Stage_Name";
                    cm.DataValueField = "Stage_id";
                    // FpSpreadstage.Sheets[0].Columns[1].CellType = cf;                  
                    FpSpreadstage.Sheets[0].Cells[m, 1].Text = ds_arr.Tables[0].Rows[0]["Stage_Name"].ToString();
                    FpSpreadstage.Sheets[0].Cells[m, 1].Value = ds_arr.Tables[0].Rows[0]["Stage_id"].ToString();
                    FpSpreadstage.Sheets[0].Cells[m, 5].Text = ds_arr.Tables[0].Rows[0]["Address"].ToString();
                    FpSpreadstage.Sheets[0].Cells[m, 1].CellType = cm;
                    //FpSpreadstage.SaveChanges();


                }

            }
            else
            {
                con.Close();
                con.Open();
                string querylist = "";
                querylist = "select * from Stage_Master where Stage_Name ='" + arr_To_Value + "'";
                ds_arr = dset.select_method_wo_parameter(querylist, "text");
                if (ds_arr.Tables[0].Rows.Count > 0)
                {
                    FpSpreadstage.Sheets[0].Cells[m, 5].Text = ds_arr.Tables[0].Rows[0]["Address"].ToString();


                }
                FpSpreadstage.Sheets[0].Cells[m, 1].Text = ds_arr.Tables[0].Rows[0]["Stage_id"].ToString();
                FpSpreadstage.Sheets[0].Cells[m, 1].Locked = true;

            }
        }
        //FpSpreadstage.SaveChanges();
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        if (FpTransport.Sheets[0].RowCount > 0)
        {
            Session["column_header_row_count"] = 1;
            string degreedetails = "Route Informations";
            string pagename = "RouteInformation.aspx";
            Printcontrol.loadspreaddetails(FpTransport, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
    }
    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value;
            }
        }

        return null;
    }
}