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
using FarPoint.Web.Spread;
using System.IO;
using System.Security.Cryptography;
using System.Collections;
using System.Drawing;
using AjaxControlToolkit;

public partial class Driver_Allotment : System.Web.UI.Page
{
    public SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());

    string vech_values = string.Empty;
    string route_values = string.Empty;
    string stage_values = string.Empty;
    string event_values = string.Empty;
    string vech_values1 = string.Empty;
    string route_values1 = string.Empty;
    string stage_values1 = string.Empty;
    string event_values1 = string.Empty;
    string collegecode = string.Empty;
    string usercode = string.Empty;
    string sql = string.Empty;
    DateTime from_date;
    DateTime to_date;
    bool Cellclick;
    bool Cellclick1;
    Hashtable hastab = new Hashtable();
    string alert = string.Empty;
    DAccess2 da2 = new DAccess2();
    DAccess2 dacces2 = new DAccess2();
    string PageLogOut = "";
    
    public class MyImg3 : ImageCellType
    {
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            //'-------------studentphoto
            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image();
            img2.ImageUrl = base.ImageUrl; //base.ImageUrl;  
            //img2.ImageUrl = "image" + "\\" + Convert.ToString(val);
            img2.Width = Unit.Percentage(80);
            img2.Height = Unit.Percentage(70);
            return img2;

        }
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

        btn_delete.Enabled = false;
        lbl_add.Text = "Add";

        txt_vech.Attributes.Add("readonly", "readonly");
        txt_route.Attributes.Add("readonly", "readonly");

        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        txt_dvr_name.Enabled = false;
        txt_helper.Enabled = false;
        lblup_error.Visible = false;

        ddlstaff.Visible = false;
        txt_search.Visible = false;
        lblsearchby.Visible = false;

        FpSpread1.Sheets[0].AutoPostBack = true;
        FpSpread1.CommandBar.Visible = false;
        FarPoint.Web.Spread.StyleInfo styles = new FarPoint.Web.Spread.StyleInfo();
        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
        styles.Font.Size = 10;
        styles.Font.Bold = true;
        FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(styles);
        FpSpread1.Sheets[0].AllowTableCorner = true;
        FpSpread1.Sheets[0].RowHeader.Visible = false;

        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;

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

        //Fp_Helper.Sheets[0].AutoPostBack = true;
        Fp_Helper.CommandBar.Visible = true;
        FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
        styles.Font.Size = 10;
        styles.Font.Bold = true;
        Fp_Helper.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
        Fp_Helper.Sheets[0].AllowTableCorner = true;
        Fp_Helper.Sheets[0].RowHeader.Visible = false;

        Fp_Helper.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;

        Fp_Helper.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        Fp_Helper.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

        Fp_Helper.Sheets[0].DefaultColumnWidth = 50;
        Fp_Helper.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        Fp_Helper.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        Fp_Helper.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

        Fp_Helper.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        Fp_Helper.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        Fp_Helper.Sheets[0].DefaultStyle.Font.Bold = false;
        Fp_Helper.SheetCorner.Cells[0, 0].Font.Bold = true;


        Fp_Helper.Sheets[0].ColumnCount = 10;
        Fp_Helper.Sheets[0].ColumnHeader.Cells[Fp_Helper.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "Sl.No";
        Fp_Helper.Sheets[0].ColumnHeader.Cells[Fp_Helper.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "Staff Code";
        Fp_Helper.Sheets[0].ColumnHeader.Cells[Fp_Helper.Sheets[0].ColumnHeader.RowCount - 1, 2].Text = "Staff Name";
        Fp_Helper.Sheets[0].ColumnHeader.Cells[Fp_Helper.Sheets[0].ColumnHeader.RowCount - 1, 3].Text = "Address";
        Fp_Helper.Sheets[0].ColumnHeader.Cells[Fp_Helper.Sheets[0].ColumnHeader.RowCount - 1, 4].Text = "Mobile No";
        Fp_Helper.Sheets[0].ColumnHeader.Cells[Fp_Helper.Sheets[0].ColumnHeader.RowCount - 1, 5].Text = "Reference Name";
        Fp_Helper.Sheets[0].ColumnHeader.Cells[Fp_Helper.Sheets[0].ColumnHeader.RowCount - 1, 6].Text = "Reference Address";
        Fp_Helper.Sheets[0].ColumnHeader.Cells[Fp_Helper.Sheets[0].ColumnHeader.RowCount - 1, 7].Text = "Ref Mobile No";

        Fp_Helper.Sheets[0].ColumnHeader.Cells[Fp_Helper.Sheets[0].ColumnHeader.RowCount - 1, 8].Text = "Staff Photo";
        Fp_Helper.Sheets[0].ColumnHeader.Cells[Fp_Helper.Sheets[0].ColumnHeader.RowCount - 1, 9].Text = "Post";

        Fp_Helper.Sheets[0].ColumnHeader.Cells[Fp_Helper.Sheets[0].ColumnHeader.RowCount - 1, 0].Column.Width = 45;
        Fp_Helper.Sheets[0].ColumnHeader.Cells[Fp_Helper.Sheets[0].ColumnHeader.RowCount - 1, 1].Column.Width = 80;
        Fp_Helper.Sheets[0].ColumnHeader.Cells[Fp_Helper.Sheets[0].ColumnHeader.RowCount - 1, 2].Column.Width = 150;
        Fp_Helper.Sheets[0].ColumnHeader.Cells[Fp_Helper.Sheets[0].ColumnHeader.RowCount - 1, 3].Column.Width = 180;
        Fp_Helper.Sheets[0].ColumnHeader.Cells[Fp_Helper.Sheets[0].ColumnHeader.RowCount - 1, 4].Column.Width = 90;
        Fp_Helper.Sheets[0].ColumnHeader.Cells[Fp_Helper.Sheets[0].ColumnHeader.RowCount - 1, 5].Column.Width = 150;
        Fp_Helper.Sheets[0].ColumnHeader.Cells[Fp_Helper.Sheets[0].ColumnHeader.RowCount - 1, 6].Column.Width = 100;
        Fp_Helper.Sheets[0].ColumnHeader.Cells[Fp_Helper.Sheets[0].ColumnHeader.RowCount - 1, 7].Column.Width = 90;
        Fp_Helper.Sheets[0].ColumnHeader.Cells[Fp_Helper.Sheets[0].ColumnHeader.RowCount - 1, 8].Column.Width = 100;
        Fp_Helper.Sheets[0].ColumnHeader.Cells[Fp_Helper.Sheets[0].ColumnHeader.RowCount - 1, 9].Column.Width = 100;

        FarPoint.Web.Spread.RegExpCellType rgex5 = new FarPoint.Web.Spread.RegExpCellType();
        rgex5.ValidationExpression = "^[0-9]*$";
        rgex5.ErrorMessage = "Number only allowed to enter";
        Fp_Helper.Sheets[0].Columns[7].CellType = rgex5;

        Fp_Helper.Sheets[0].Columns[0].Locked = true;
        Fp_Helper.Sheets[0].Columns[1].Locked = true;
        Fp_Helper.Sheets[0].Columns[2].Locked = true;
        Fp_Helper.Sheets[0].Columns[3].Locked = true;
        Fp_Helper.Sheets[0].Columns[4].Locked = true;
        Fp_Helper.Sheets[0].Columns[8].Locked = true;


        FarPoint.Web.Spread.StyleInfo styles1 = new FarPoint.Web.Spread.StyleInfo();
        styles1.Font.Size = 10;
        styles1.Font.Bold = true;

        //Fp_Driver.Sheets[0].AutoPostBack = true;
        Fp_Driver.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(styles1);
        Fp_Driver.Sheets[0].AllowTableCorner = true;
        Fp_Driver.Sheets[0].RowHeader.Visible = false;

        Fp_Driver.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        //FpSpread1.Sheets[0]

        Fp_Driver.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        Fp_Driver.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

        Fp_Driver.Sheets[0].DefaultColumnWidth = 50;
        Fp_Driver.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        Fp_Driver.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        Fp_Driver.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        //Fp_Route.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;

        Fp_Driver.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        Fp_Driver.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        Fp_Driver.Sheets[0].DefaultStyle.Font.Bold = false;
        Fp_Driver.SheetCorner.Cells[0, 0].Font.Bold = true;
        //FarPoint.Web.Spread.ButtonCellType btnmodify1 = new FarPoint.Web.Spread.ButtonCellType();
        //FarPoint.Web.Spread.ButtonCellType btnmodify2 = new FarPoint.Web.Spread.ButtonCellType();
        //FarPoint.Web.Spread.ButtonCellType btnmodify3 = new FarPoint.Web.Spread.ButtonCellType();
        //FarPoint.Web.Spread.ButtonCellType btnmodify4 = new FarPoint.Web.Spread.ButtonCellType();
        //btnmodify1.Text = "Add";
        //btnmodify2.Text = "View";
        //btnmodify3.Text = "Add";
        //btnmodify4.Text = "View";

        //Fp_Driver.Sheets[0].ColumnCount = 14;
        //Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Sl.No";
        //Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
        //Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Driver Name";
        //Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Address";
        //Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Mobile No";
        //Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Reference Name";
        //Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Reference Address";
        //Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Ref Mobile No";
        //Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Driver Photo";
        //Fp_Driver.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Justify;
        //Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Add Licence Front";
        //Fp_Driver.Sheets[0].Columns[9].Visible = true;
        //Fp_Driver.Sheets[0].Columns[9].CellType = btnmodify1;
        //Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Licence Front Photo";
        //Fp_Driver.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Justify;
        //Fp_Driver.Sheets[0].Columns[10].CellType = btnmodify2;
        //Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Add Licence Back";
        //Fp_Driver.Sheets[0].Columns[11].Visible = true;
        //Fp_Driver.Sheets[0].Columns[11].CellType = btnmodify3;
        //Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Licence Back Photo";
        //Fp_Driver.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Justify;
        //Fp_Driver.Sheets[0].Columns[12].CellType = btnmodify4;

        //Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Licence Renew Date";

        //modify by rajasekar 14/09/2018
        Fp_Driver.Sheets[0].ColumnCount = 12;
        Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Sl.No";
        Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
        Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Driver Name";
        Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Address";
        Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Mobile No";
        Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Reference Name";
        Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Reference Address";
        Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Ref Mobile No";
        Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Driver Photo";
        Fp_Driver.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Justify;
        Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Licence Front Photo";
        Fp_Driver.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Justify;

        Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Licence Back Photo";
        Fp_Driver.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Justify;

        Fp_Driver.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Licence Renew Date";
        
        
        
        


        FarPoint.Web.Spread.RegExpCellType rgex4 = new FarPoint.Web.Spread.RegExpCellType();
        rgex4.ValidationExpression = "^[0-9]*$";
        rgex4.ErrorMessage = "Number only allowed to enter";
        Fp_Driver.Sheets[0].Columns[7].CellType = rgex4;

        Fp_Driver.Sheets[0].Columns[0].Locked = true;
        Fp_Driver.Sheets[0].Columns[1].Locked = true;
        Fp_Driver.Sheets[0].Columns[2].Locked = true;
        Fp_Driver.Sheets[0].Columns[3].Locked = true;
        Fp_Driver.Sheets[0].Columns[4].Locked = true;
        Fp_Driver.Sheets[0].Columns[8].Locked = true;
        Fp_Driver.Sheets[0].Columns[9].Locked = true;
        Fp_Driver.Sheets[0].Columns[10].Locked = true;
        Fp_Driver.Sheets[0].Columns[11].Locked = true;
        

        Fp_Driver.Sheets[0].ColumnHeader.Cells[Fp_Driver.Sheets[0].ColumnHeader.RowCount - 1, 0].Column.Width = 45;
        Fp_Driver.Sheets[0].ColumnHeader.Cells[Fp_Driver.Sheets[0].ColumnHeader.RowCount - 1, 1].Column.Width = 70;
        Fp_Driver.Sheets[0].ColumnHeader.Cells[Fp_Driver.Sheets[0].ColumnHeader.RowCount - 1, 2].Column.Width = 150;
        Fp_Driver.Sheets[0].ColumnHeader.Cells[Fp_Driver.Sheets[0].ColumnHeader.RowCount - 1, 3].Column.Width = 180;
        Fp_Driver.Sheets[0].ColumnHeader.Cells[Fp_Driver.Sheets[0].ColumnHeader.RowCount - 1, 4].Column.Width = 90;
        Fp_Driver.Sheets[0].ColumnHeader.Cells[Fp_Driver.Sheets[0].ColumnHeader.RowCount - 1, 5].Column.Width = 100;
        Fp_Driver.Sheets[0].ColumnHeader.Cells[Fp_Driver.Sheets[0].ColumnHeader.RowCount - 1, 6].Column.Width = 150;
        Fp_Driver.Sheets[0].ColumnHeader.Cells[Fp_Driver.Sheets[0].ColumnHeader.RowCount - 1, 7].Column.Width = 90;
        Fp_Driver.Sheets[0].ColumnHeader.Cells[Fp_Driver.Sheets[0].ColumnHeader.RowCount - 1, 8].Column.Width = 100;
        Fp_Driver.Sheets[0].ColumnHeader.Cells[Fp_Driver.Sheets[0].ColumnHeader.RowCount - 1, 9].Column.Width = 100;//rajasekar
        Fp_Driver.Sheets[0].ColumnHeader.Cells[Fp_Driver.Sheets[0].ColumnHeader.RowCount - 1, 10].Column.Width = 100;//rajasekar
        Fp_Driver.Sheets[0].ColumnHeader.Cells[Fp_Driver.Sheets[0].ColumnHeader.RowCount - 1, 11].Column.Width = 100;//rajasekar
        //Fp_Driver.Sheets[0].ColumnHeader.Cells[Fp_Driver.Sheets[0].ColumnHeader.RowCount - 1, 12].Column.Width = 100;//rajasekar
        //Fp_Driver.Sheets[0].ColumnHeader.Cells[Fp_Driver.Sheets[0].ColumnHeader.RowCount - 1, 13].Column.Width = 100;

        fsstaff.CommandBar.Visible = true;

        styles.Font.Size = 10;
        styles.Font.Bold = true;
        fsstaff.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
        fsstaff.Sheets[0].AllowTableCorner = true;
        fsstaff.Sheets[0].RowHeader.Visible = false;

        fsstaff.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;

        fsstaff.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        fsstaff.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

        fsstaff.Sheets[0].DefaultColumnWidth = 50;
        fsstaff.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        fsstaff.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        fsstaff.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

        fsstaff.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        fsstaff.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        fsstaff.Sheets[0].DefaultStyle.Font.Bold = false;
        fsstaff.SheetCorner.Cells[0, 0].Font.Bold = true;

        fsstaff.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
        fsstaff.Sheets[0].ColumnHeader.Columns[1].Label = "Staff Name";
        fsstaff.Sheets[0].ColumnHeader.Columns[2].Label = "Staff Code";
        fsstaff.Sheets[0].ColumnHeader.Columns[3].Label = "Select";

        fsstaff.Sheets[0].Columns[0].Locked = true;
        fsstaff.Sheets[0].Columns[1].Locked = true;
        fsstaff.Sheets[0].Columns[2].Locked = true;

        fsstaff.Sheets[0].Columns[0].Width = 50;
        fsstaff.Sheets[0].Columns[1].Width = 290;
        fsstaff.Sheets[0].Columns[2].Width = 65;
        fsstaff.Sheets[0].Columns[3].Width = 62;

        fsstaff.Sheets[0].ColumnCount = 4;

        if (!IsPostBack)
        {
            Fp_Driver.Sheets[0].RowCount = 0;
            Fp_Helper.Sheets[0].RowCount = 0;

            PanelUpload.Visible = false;
            fsstaff.Visible = false;
            Driver_Img.Visible = false;
            Panel3.Visible = false;

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
                    string ddl_items = list_vehicle_id.Text;
                    vehiclechecklist.Items[incre_veh - 1].Selected = true;

                    ddl_vehid.Items.Add(ddl_items);

                }
            }
            loadvehicle();

            con.Close();
            con.Open();
            SqlCommand cmd_route = new SqlCommand("select distinct Route_ID from routemaster", con);
            SqlDataReader rdr_route = cmd_route.ExecuteReader();

            int incre_route = 0;

            while (rdr_route.Read())
            {
                if (rdr_route.HasRows == true)
                {
                    incre_route++;
                    System.Web.UI.WebControls.ListItem list_route = new System.Web.UI.WebControls.ListItem();

                    list_route.Text = (rdr_route["Route_ID"].ToString());

                    checkrolist.Items.Add(list_route);
                    string ddl_item = list_route.Text;
                    checkrolist.Items[incre_route - 1].Selected = true;

                    ddl_routeid.Items.Add(ddl_item);

                }
            }
            loadroute();
            lblerror.Visible = false;
            FpSpread1.Visible = false;
            Accordion1.SelectedIndex = 0;
        }

        btnMainGo_Click(sender, e);
    }

    protected void loadvehicle()
    {
        for (int i = 0; i < vehiclechecklist.Items.Count; i++)
        {
            vehiclechecklist.Items[i].Selected = true;
        }
        vehiclecheck.Checked = true;
        txt_vech.Text = "Vehicle(" + vehiclechecklist.Items.Count + ")";
    }

    protected void loadroute()
    {
        for (int i = 0; i < checkrolist.Items.Count; i++)
        {
            checkrolist.Items[i].Selected = true;
        }
        checkro.Checked = true;
        txt_route.Text = "Route(" + checkrolist.Items.Count + ")";
    }

    void Fill_Grid()
    {
        //FarPoint.Web.Spread.DateTimeCellType Calendar = new FarPoint.Web.Spread.DateTimeCellType();
        //Calendar.Format("yyyy/MM/dd");
        //Calendar.ErrorMessage = "Please enter this format yyyy/MM/dd";
        //old
        //FarPoint.Web.Spread.Extender.DateCalendarCellType Calendar = new FarPoint.Web.Spread.Extender.DateCalendarCellType();
        //Calendar.DateFormat = "yyyy/MM/dd";
        //AjaxControlToolkit.MaskedEditExtender mee = new AjaxControlToolkit.MaskedEditExtender();
        //Calendar.ShowEditor = true;
        //Calendar.ShowPopupButton = true;
        //mee.Mask = "9999/99/99";
        //mee.MaskType = MaskedEditType.Date;

        //mee.ClearMaskOnLostFocus = true;
        //Calendar.DateFormat = "yyyy/MM/dd";
        //mee.CultureName = "zh-cn";
        //Calendar.Extenders.Add(mee);

        for (int i = 0; i < Fp_Driver.Sheets[0].RowCount; i++)
        {
            //Fp_Driver.Sheets[0].Cells[i, 13].CellType = Calendar;
            Fp_Driver.Sheets[0].Cells[i, 11].HorizontalAlign = HorizontalAlign.Center;
        }
    }

    void Fill_Grid1()
    {
        FarPoint.Web.Spread.ComboBoxCellType ddl_post = new FarPoint.Web.Spread.ComboBoxCellType();
        ddl_post.ShowButton = true;

        DataTable dt_post = new DataTable();

        DataColumn dc_post = new DataColumn();
        dc_post.ColumnName = "Post";
        dt_post.Columns.Add(dc_post);

        DataRow dr;

        dr = dt_post.NewRow();
        dr["Post"] = "Helper";
        dt_post.Rows.Add(dr);

        dr = dt_post.NewRow();
        dr["Post"] = "Checker";
        dt_post.Rows.Add(dr);

        ddl_post.DataSource = dt_post;
        ddl_post.DataTextField = "Post";
        ddl_post.DataValueField = "Post";

        for (int i = 0; i < Fp_Helper.Sheets[0].RowCount; i++)
        {
            Fp_Helper.Sheets[0].Cells[i, 9].CellType = ddl_post;
        }

    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {

        Control cntUpdateBtn = Fp_Driver.FindControl("Update");
        Control cntCancelBtn = Fp_Driver.FindControl("Cancel");
        Control cntCopyBtn = Fp_Driver.FindControl("Copy");
        Control cntCutBtn = Fp_Driver.FindControl("Clear");
        Control cntPasteBtn = Fp_Driver.FindControl("Paste");
        Control cntPageNextBtn = Fp_Driver.FindControl("Next");
        Control cntPagePreviousBtn = Fp_Driver.FindControl("Prev");
        Control cntPagePrintBtn = Fp_Driver.FindControl("Print");

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

            tc = (TableCell)cntPagePrintBtn.Parent;
            tr.Cells.Remove(tc);
        }

        Control cntUpdateBtn1 = Fp_Helper.FindControl("Update");
        Control cntCancelBtn1 = Fp_Helper.FindControl("Cancel");
        Control cntCopyBtn1 = Fp_Helper.FindControl("Copy");
        Control cntCutBtn1 = Fp_Helper.FindControl("Clear");
        Control cntPasteBtn1 = Fp_Helper.FindControl("Paste");
        Control cntPageNextBtn1 = Fp_Helper.FindControl("Next");
        Control cntPagePreviousBtn1 = Fp_Helper.FindControl("Prev");
        Control cntPagePrintBtn1 = Fp_Helper.FindControl("Print");

        if ((cntUpdateBtn1 != null))
        {

            TableCell tc = (TableCell)cntUpdateBtn1.Parent;
            TableRow tr = (TableRow)tc.Parent;

            tr.Cells.Remove(tc);

            tc = (TableCell)cntCancelBtn1.Parent;
            tr.Cells.Remove(tc);


            tc = (TableCell)cntCopyBtn1.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntCutBtn1.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPasteBtn1.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPageNextBtn1.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPagePreviousBtn1.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPagePrintBtn1.Parent;
            tr.Cells.Remove(tc);

        }

        Control cntUpdateBtn10 = fsstaff.FindControl("Update");
        Control cntCancelBtn10 = fsstaff.FindControl("Cancel");
        Control cntCopyBtn10 = fsstaff.FindControl("Copy");
        Control cntCutBtn10 = fsstaff.FindControl("Clear");
        Control cntPasteBtn10 = fsstaff.FindControl("Paste");
        Control cntPageNextBtn10 = fsstaff.FindControl("Next");
        Control cntPagePreviousBtn10 = fsstaff.FindControl("Prev");
        Control cntPagePrintBtn10 = fsstaff.FindControl("Print");

        if ((cntUpdateBtn10 != null))
        {

            TableCell tc = (TableCell)cntUpdateBtn10.Parent;
            TableRow tr = (TableRow)tc.Parent;

            tr.Cells.Remove(tc);

            tc = (TableCell)cntCancelBtn10.Parent;
            tr.Cells.Remove(tc);


            tc = (TableCell)cntCopyBtn10.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntCutBtn10.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPasteBtn10.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPageNextBtn10.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPagePreviousBtn10.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPagePrintBtn10.Parent;
            tr.Cells.Remove(tc);

        }

        base.Render(writer);
    }

    protected void ddl_vehid_SelectedIndexChanged(object sender, EventArgs e)
    {
        Bind_Routes();
    }

    protected void ddl_routeid_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void txt_helper_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txt_num_dvr_TextChanged(object sender, EventArgs e)
    {


    }

    public void load_details()
    {
        txt_route.Attributes.Add("onfocus", "changevehipur()");
        //txt_num_dvr.Attributes.Add("onfocus", "changedriver()");
    }

    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void chkbatch_CheckedChanged(object sender, EventArgs e)
    {

    }

    protected void ddltypeview_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlvehicletypeview_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void checklistroute_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void checkroute_CheckedChanged(object sender, EventArgs e)
    {

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
        Bind_Routes1();
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
        Bind_Routes1();

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
                        route_values = route_values + "','" + checkrolist.Items[i].Text;
                    }
                }
            }
            else
            {
                for (int i = 0; i < checkrolist.Items.Count; i++)
                {
                    checkrolist.Items[i].Selected = false;
                    txt_route.Text = "--Select--";
                }
            }
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
                        route_values = route_values + "','" + checkrolist.Items[i].Text;
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
    }

    void Bind_Routes()
    {

        con.Close();
        con.Open();
        int count_items = 0;
        SqlCommand cmd_bind_route = new SqlCommand("select distinct r.Route_ID from routemaster r,vehicle_master v where r.Route_id=v.Route and v.Veh_Id in('" + ddl_vehid.SelectedValue + "') ", con);
        SqlDataAdapter ad_bind_route = new SqlDataAdapter(cmd_bind_route);
        DataTable dt_bind_route = new DataTable();
        ad_bind_route.Fill(dt_bind_route);

        ddl_routeid.Items.Clear();
        if (dt_bind_route.Rows.Count > 0)
        {
            ddl_routeid.DataSource = dt_bind_route;
            ddl_routeid.DataTextField = "Route_ID";
            ddl_routeid.DataBind();

        }
    }

    void Bind_Routes1()
    {

        con.Close();
        con.Open();
        int count_items = 0;
        checkrolist.Items.Clear();
        SqlCommand cmd_bind_route = new SqlCommand("select distinct r.Route_ID from routemaster r,vehicle_master v where r.Route_id=v.Route and v.Veh_Id in('" + vech_values + "') ", con);
        SqlDataAdapter ad_bind_route = new SqlDataAdapter(cmd_bind_route);
        DataTable dt_bind_route = new DataTable();
        ad_bind_route.Fill(dt_bind_route);

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
            txt_route.Text = "Route(" + checkrolist.Items.Count + ")";

        }
        else
        {
            txt_route.Text = "--Select--";
            checkro.Checked = false;
        }
    }

    protected void btn_driv_name_Click(object sender, EventArgs e)
    {
        //if (txt_num_dvr.Text != "")
        //{
        //panel8.Visible = true;
        mpedirect.Show();

        //panelrollnopop.Visible = false;
        fsstaff.Visible = true;
        btnstaffadd.Text = "Ok";
        fsstaff.Sheets[0].RowCount = 0;
        BindCollege();
        loadstaffdep(collegecode);

        bind_stafType();
        bind_design();
        loadfsstaff();


    }

    protected void btn_helper_Click(object sender, EventArgs e)
    {

        //panel8.Visible = true;

        mpedirect.Show();
        btnstaffadd.Text = "Ok ";

        fsstaff.Visible = true;
        fsstaff.Sheets[0].RowCount = 0;
        BindCollege();
        loadstaffdep(collegecode);

        loadfsstaff();
        bind_stafType();
        bind_design();

    }

    void BindCollege()
    {
        con.Close();
        con.Open();
        SqlCommand cmd = new SqlCommand("select collname,college_code from collinfo", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlcollege.DataSource = ds;
        ddlcollege.DataTextField = "collname";
        ddlcollege.DataValueField = "college_code";
        ddlcollege.DataBind();

    }

    void loadstaffdep(string collegecode)
    {
        con.Close();
        con.Open();
        //SqlCommand cmd = new SqlCommand("select distinct dept_name,dept_code from hrdept_master where college_code=" + Session["collegecode"] + "", con); rajasekar 20march2018
        SqlCommand cmd = new SqlCommand("select distinct dept_name,dept_code from hrdept_master where college_code=" + ddlcollege.SelectedValue + "", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddldepratstaff.DataSource = ds;
        ddldepratstaff.DataTextField = "dept_name";
        ddldepratstaff.DataValueField = "dept_code";
        ddldepratstaff.DataBind();
        //ddldepratstaff.Items.Insert(0, "All");

    }

    void bind_stafType()
    {
        con.Close();
        con.Open();
        // SqlCommand cmd_get_stftype = new SqlCommand("SELECT DISTINCT StfType FROM StaffTrans T,HrDept_Master D WHERE T.Dept_Code = D.Dept_Code AND T.Latestrec = 1 and d.college_code=" + Session["collegecode"] + "", con); rajasekar 20march2018
        SqlCommand cmd_get_stftype = new SqlCommand("SELECT DISTINCT StfType FROM StaffTrans T,HrDept_Master D WHERE T.Dept_Code = D.Dept_Code AND T.Latestrec = 1 and d.college_code=" + ddlcollege.SelectedValue + "", con);
        SqlDataAdapter ad_get_stftype = new SqlDataAdapter(cmd_get_stftype);
        DataTable dt_get_stftype = new DataTable();
        ad_get_stftype.Fill(dt_get_stftype);

        if (dt_get_stftype.Rows.Count > 0)
        {
            ddl_stftype.DataSource = dt_get_stftype;
            ddl_stftype.DataTextField = "StfType";
            ddl_stftype.DataValueField = "StfType";
            ddl_stftype.DataBind();

        }
    }

    void bind_design()
    {
        con.Close();
        con.Open();
        // SqlCommand cmd_get_design = new SqlCommand("SELECT distinct Desig_Name FROM StaffTrans T,Desig_Master G WHERE T.Desig_Code = G.Desig_Code AND Latestrec = 1 and G.collegecode=" + Session["collegecode"] + " and stftype='" + ddl_stftype.Text + "'", con); rajasekar 20march2018
        SqlCommand cmd_get_design = new SqlCommand("SELECT distinct Desig_Name FROM StaffTrans T,Desig_Master G WHERE T.Desig_Code = G.Desig_Code AND Latestrec = 1 and G.collegecode=" + ddlcollege.SelectedValue + " and stftype='" + ddl_stftype.Text + "'", con);
        SqlDataAdapter ad_get_design = new SqlDataAdapter(cmd_get_design);
        DataTable dt_get_design = new DataTable();
        ad_get_design.Fill(dt_get_design);

        if (dt_get_design.Rows.Count > 0)
        {

            ddl_design.DataSource = dt_get_design;
            ddl_design.DataTextField = "Desig_Name";
            ddl_design.DataValueField = "Desig_Name";
            ddl_design.DataBind();

        }
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


        for (int route_count = 0; route_count < checkrolist.Items.Count; route_count++)
        {
            if (checkrolist.Items[route_count].Selected == true)
            {
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


        FpSpread1.Sheets[0].ColumnCount = 12;
        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
        FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "Sl.No";
        FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "Vehicle Id";
        FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 2].Text = "Route Id";
        FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 3].Text = "Staff Code";
        FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 4].Text = "Staff Name";
        FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 5].Text = "Address";
        FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 6].Text = "Mobile No";
        FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 7].Text = "Reference Name";
        FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 8].Text = "Reference Address";
        FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 9].Text = "Reference Mobile No";
        FpSpread1.Sheets[0].Columns[9].CellType = txt;
        FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 10].Text = "Designation";
        FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 11].Text = "Staff Photo";


        if (vech_all != "")
        {
            if (route_all != "")
            {
                con.Close();
                con.Open();

                SqlCommand cmd_driver_report = new SqlCommand("select * from driverallotment where vehicle_id in('" + vech_all + "') and route_id in('" + route_all + "')  order by len(vehicle_id), vehicle_id", con);
                SqlDataAdapter ad_driver_report = new SqlDataAdapter(cmd_driver_report);
                DataTable dt_driver_report = new DataTable();
                ad_driver_report.Fill(dt_driver_report);

                if (dt_driver_report.Rows.Count > 0)
                {
                    FpSpread1.Sheets[0].RowCount = 0;
                    int sno = 0;
                    for (int i = 0; i < dt_driver_report.Rows.Count; i++)
                    {
                        sno++;

                        string vehid = dt_driver_report.Rows[i]["Vehicle_id"].ToString();
                        string routeid = dt_driver_report.Rows[i]["Route_id"].ToString();
                        string staffcode = dt_driver_report.Rows[i]["Staff_code"].ToString();
                        string drivname = dt_driver_report.Rows[i]["Staff_Name"].ToString();
                        string adrs = dt_driver_report.Rows[i]["Address"].ToString();
                        string mobno = dt_driver_report.Rows[i]["Mobile_No"].ToString();
                        string ref1name = dt_driver_report.Rows[i]["Ref_name"].ToString();
                        string ref1add = dt_driver_report.Rows[i]["Ref_Address"].ToString();
                        string ref1mob = dt_driver_report.Rows[i]["Ref_Mobile"].ToString();
                        string desig = dt_driver_report.Rows[i]["Design"].ToString();


                        FpSpread1.Sheets[0].RowCount = Convert.ToInt32(FpSpread1.Sheets[0].RowCount) + 1;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txt;//added by rajasekar 20/06/2018
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = vehid;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = routeid;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = staffcode;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = drivname;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = adrs;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = mobno;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = ref1name;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = ref1add;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = ref1mob;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = desig;


                        MyImg3 driverphoto = new MyImg3();
                        driverphoto.ImageUrl = "~/images/10BIT001.jpeg";
                        driverphoto.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + staffcode;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].CellType = driverphoto;

                        FpSpread1.Visible = true;
                        lbl_err.Visible = false;
                    }
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                }

                else
                {
                    lbl_err.Text = "No Data Found";
                    lbl_err.Visible = true;
                    FpSpread1.Sheets[0].RowCount = 0;

                }
            }
            else
            {
                lbl_err.Text = "Please Select Route Id";
                lbl_err.Visible = true;
                FpSpread1.Sheets[0].RowCount = 0;

            }
        }
        else
        {
            lbl_err.Text = "Please Select Vechicle Id";
            lbl_err.Visible = true;
            FpSpread1.Sheets[0].RowCount = 0;

        }
    }

    protected void ddldepratstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        fsstaff.Sheets[0].RowCount = 0;
        bind_stafType();
        mpedirect.Show();
    }

    protected void ddl_stftype_SelectedIndexChanged(object sender, EventArgs e)
    {
        fsstaff.Sheets[0].RowCount = 0;
        

        bind_design();
        loadfsstaff();
        mpedirect.Show();
    }

    protected void ddl_design_SelectedIndexChanged(object sender, EventArgs e)
    {
        fsstaff.Sheets[0].RowCount = 0;
        loadfsstaff();
        mpedirect.Show();
        //bind_design();

    }

    protected void ddlstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        fsstaff.Sheets[0].RowCount = 0;
        //loadfsstaff();
    }

    protected void txt_search_TextChanged(object sender, EventArgs e)
    {
        fsstaff.Sheets[0].RowCount = 0;

        loadfsstaff();
    }

    protected void txt_dvr_name_TextChanged(object sender, EventArgs e)
    {
        Driver_Img.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + txt_dvr_name.Text;
        Driver_Img.Visible = true;
    }

    protected void loadfsstaff()
    {
        if (ddldepratstaff.SelectedIndex != 0)
        {
            if (txt_search.Text != "")
            {
                if (ddlstaff.SelectedIndex == 0)
                {
                    sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0)and  (staffmaster.settled = 0)  and (staff_name like '" + txt_search.Text + "%') and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";
                }
                else if (ddlstaff.SelectedIndex == 1)
                {
                    sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_code like '" + txt_search.Text + "%') and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";
                }
            }
            else
            {
                //sql = "SELECT staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (hrdept_master.dept_name = '" + ddldepratstaff.Text + "') AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "' and (staffmaster.college_code =hrdept_master.college_code)";
                sql = "SELECT staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";

            }
        }
        else if (txt_search.Text != "")
        {
            if (ddlstaff.SelectedIndex == 0)
            {
                sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_name like '" + txt_search.Text + "%') and (staffmaster.college_code =hrdept_master.college_code)";
            }
            else if (ddlstaff.SelectedIndex == 1)
            {
                sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_code like '" + txt_search.Text + "%') and (staffmaster.college_code =hrdept_master.college_code)";
            }
            else if (ddlcollege.SelectedIndex != -1)
            {
                sql = "select distinct staffmaster.staff_code, staff_name  from stafftrans,staffmaster where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and settled=0 and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";
            }

            else
            {
                sql = "select distinct staffmaster.staff_code, staff_name from stafftrans,staffmaster,hrdept_master.dept_name where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0";

            }
        }
        else
            if (ddldepratstaff.SelectedValue.ToString() == "All")
            {
                sql = "select distinct staffmaster.staff_code, staff_name  from stafftrans,staffmaster where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and settled=0 and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";

            }
        fsstaff.Sheets[0].RowCount = 0;
        fsstaff.SaveChanges();

        FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
        FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();

        fsstaff.Sheets[0].RowCount = fsstaff.Sheets[0].RowCount + 1;
        fsstaff.Sheets[0].SpanModel.Add(fsstaff.Sheets[0].RowCount - 1, 0, 1, 3);
        fsstaff.Sheets[0].AutoPostBack = false;
        string bindspread = sql;

        string design_name = string.Empty;

        if (ddl_design.Items.Count > 0)
        {
            design_name = ddl_design.SelectedItem.ToString();
        }
        con.Close();
        con.Open();

        SqlCommand cmd_get_stafflist = new SqlCommand("select distinct s.staff_code,s.staff_name from staffmaster s,hrdept_master h,desig_master d,stafftrans st where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and h.dept_code = '" + ddldepratstaff.SelectedValue.ToString() + "' and d.desig_name='" + design_name + "' and s.college_code='" + ddlcollege.SelectedValue.ToString() + "' and resign = 0 and settled = 0 and latestrec=1", con);
        SqlDataAdapter dabindspread = new SqlDataAdapter(cmd_get_stafflist);
        DataSet dsbindspread = new DataSet();
        dabindspread.Fill(dsbindspread);
        con.Close();
        con.Open();
        mpedirect.Show();
        if (dsbindspread.Tables[0].Rows.Count > 0)
        {
            int sno = 0;
            for (int rolcount = 0; rolcount < dsbindspread.Tables[0].Rows.Count; rolcount++)
            {
                sno++;
                string name = dsbindspread.Tables[0].Rows[rolcount]["staff_name"].ToString();
                string code = dsbindspread.Tables[0].Rows[rolcount]["staff_code"].ToString();


                fsstaff.Sheets[0].RowCount = fsstaff.Sheets[0].RowCount + 1;
                fsstaff.Sheets[0].Rows[fsstaff.Sheets[0].RowCount - 1].Font.Bold = false;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].Text = name;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].Text = code;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;

                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 3].CellType = chkcell1;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                fsstaff.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                //chkcell1.AutoPostBack = true;

            }
            int rowcount = fsstaff.Sheets[0].RowCount;
            fsstaff.Height = 300;
            fsstaff.Sheets[0].PageSize = 25 + (rowcount * 20);
            fsstaff.SaveChanges();


        }
    }

    protected void btnstaffadd_Click(object sender, EventArgs e)
    {
        try
        {
            fsstaff.SaveChanges();
            string staffcodes = string.Empty;
            string txtbox_text = string.Empty;
            DataTable dt = new DataTable();
            DataColumn dc;
            dc = new DataColumn();
            dc.ColumnName = "StaffCode";
            dt.Columns.Add(dc);
            DataRow dr;
            int staff_count = 0;
            int txt_count = 0;
            DataSet ds = new DataSet();

            for (int kk = 0; kk < fsstaff.Rows.Count; kk++)
            {
                int val = Convert.ToInt32(fsstaff.Sheets[0].Cells[kk, 3].Value);
                if (val == 1)
                {
                    staff_count++;
                    if (staffcodes == "")
                    {
                        txtbox_text = fsstaff.Sheets[0].Cells[kk, 2].Text;
                        staffcodes = fsstaff.Sheets[0].Cells[kk, 2].Text;
                    }
                    else
                    {
                        txtbox_text = txtbox_text + "," + fsstaff.Sheets[0].Cells[kk, 2].Text;
                        staffcodes = staffcodes + "','" + fsstaff.Sheets[0].Cells[kk, 2].Text;
                    }

                    dr = dt.NewRow();
                    dr["StaffCode"] = fsstaff.Sheets[0].Cells[kk, 2].Text;
                    dt.Rows.Add(dr);
                }
            }


            string Query = "select Vehicle_Id from DriverAllotment where Staff_Code in ('" + staffcodes + "')";
            ds = dacces2.select_method_wo_parameter(Query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "This driver already attoted vehicle ID '" + ds.Tables[0].Rows[0]["Vehicle_Id"].ToString() + "'";
                return;
            }



            if (staffcodes != "")
            {
                if (btnstaffadd.Text == "Ok")
                {
                    //txt_count = Convert.ToInt32( txt_num_dvr.Text.Trim());
                    Fp_Driver.Sheets[0].RowCount = 0;
                }
                else if (btnstaffadd.Text == "Ok ")
                {
                    //txt_count = Convert.ToInt32(txt_numhelper.Text.Trim());
                    Fp_Helper.Sheets[0].RowCount = 0;
                }

                if (staff_count > 0)
                {
                    con.Close();
                    con.Open();
                    SqlCommand cmd_dvrdata = new SqlCommand("select appl_name,comm_address,com_pincode,com_mobileno,per_phone,referee_info from staff_appl_master where appl_no in(select appl_no from staffmaster where staff_code in('" + staffcodes + "'))", con);
                    SqlDataAdapter ad_dvrdata = new SqlDataAdapter(cmd_dvrdata);
                    DataTable dt_dvrdata = new DataTable();
                    ad_dvrdata.Fill(dt_dvrdata);

                    int slno = 0;

                    if (dt_dvrdata.Rows.Count > 0)
                    {
                        for (int d = 0; d < dt_dvrdata.Rows.Count; d++)
                        {
                            slno++;
                            string staf_code = dt.Rows[d]["StaffCode"].ToString();
                            string staf_name = dt_dvrdata.Rows[d]["appl_name"].ToString();
                            string staf_address = dt_dvrdata.Rows[d]["comm_address"].ToString();
                            string staf_pin = dt_dvrdata.Rows[d]["com_pincode"].ToString();
                            string staf_mob = dt_dvrdata.Rows[d]["com_mobileno"].ToString();
                            string staf_phone = dt_dvrdata.Rows[d]["per_phone"].ToString();
                            string ref_info = dt_dvrdata.Rows[d]["referee_info"].ToString();

                            if (btnstaffadd.Text == "Ok")
                            {
                                Fp_Driver.Sheets[0].RowCount = Convert.ToInt32(Fp_Driver.Sheets[0].RowCount) + 1;

                                Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 0].Text = slno.ToString();
                                Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 1].Text = staf_code.ToString();
                                Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 2].Text = staf_name.ToString();
                                Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 3].Text = staf_address.ToString();

                                if (staf_mob != "")
                                {
                                    Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 4].Text = staf_mob.ToString();
                                }
                                else
                                {
                                    Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 4].Text = staf_phone.ToString();
                                }
                                if (ref_info != "")
                                {
                                    string[] spli_ref_info = ref_info.Split(';');
                                    if (spli_ref_info[0] != ";" && spli_ref_info[0] != "")
                                    {
                                        Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 5].Text = spli_ref_info[0].ToString();
                                    }
                                    else
                                    {
                                        Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 5].Text = "-";
                                    }
                                    if (spli_ref_info[0] != ";" && spli_ref_info[0] != "")
                                    {
                                        Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 6].Text = spli_ref_info[1].ToString();
                                    }
                                    else
                                    {
                                        Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 6].Text = "-";
                                    }
                                    if (spli_ref_info[8] != ";" && spli_ref_info[8] != "")
                                    {
                                        Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 7].Text = spli_ref_info[8].ToString();
                                    }
                                    else
                                    {
                                        Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 7].Text = "-";
                                    }

                                }

                                string renew_date = dacces2.GetFunction("select LicenseRenewDate from StaffPhoto where staff_code='" + staf_code +"'");


                                if (renew_date != "1/1/1900 12:00:00 AM" && renew_date != "" && renew_date != "0")
                                {
                                    string[] spl_renew = renew_date.Split(' ');

                                    string[] spl_date = spl_renew[0].Split('/');

                                    renew_date = spl_date[2] + "/" + spl_date[0] + "/" + spl_date[1];

                                    Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 11].Text = renew_date;
                                }


                                MyImg3 driver_photo = new MyImg3();
                                driver_photo.ImageUrl = "~/images/10BIT001.jpeg";
                                driver_photo.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + staf_code;

                                Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 8].CellType = driver_photo;
                                Fp_Driver.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Justify;


                                MyImg3 Lin_Front = new MyImg3();
                                Lin_Front.ImageUrl = "~/images/10BIT001.jpeg";
                                Lin_Front.ImageUrl = "~/Handler/licencefront.ashx?Staff_code=" + staf_code;

                                Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 9].CellType = Lin_Front;
                                Fp_Driver.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Justify;

                                MyImg3 Lin_Back = new MyImg3();
                                Lin_Back.ImageUrl = "~/images/10BIT001.jpeg";
                                Lin_Back.ImageUrl = "~/Handler/LicenceBack.ashx?Staff_code=" + staf_code;

                                Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 10].CellType = Lin_Back;
                                Fp_Driver.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Justify;


                                txt_dvr_name.Text = txtbox_text;
                                Fp_Driver.Sheets[0].PageSize = Fp_Driver.Sheets[0].RowCount;

                            }

                            else if (btnstaffadd.Text == "Ok ")
                            {

                                Fp_Helper.Sheets[0].RowCount = Convert.ToInt32(Fp_Helper.Sheets[0].RowCount) + 1;

                                Fp_Helper.Sheets[0].Cells[Fp_Helper.Sheets[0].RowCount - 1, 0].Text = slno.ToString();
                                Fp_Helper.Sheets[0].Cells[Fp_Helper.Sheets[0].RowCount - 1, 1].Text = staf_code.ToString();
                                Fp_Helper.Sheets[0].Cells[Fp_Helper.Sheets[0].RowCount - 1, 2].Text = staf_name.ToString();
                                Fp_Helper.Sheets[0].Cells[Fp_Helper.Sheets[0].RowCount - 1, 3].Text = staf_address.ToString();
                                if (staf_mob != "")
                                {
                                    Fp_Helper.Sheets[0].Cells[Fp_Helper.Sheets[0].RowCount - 1, 4].Text = staf_mob.ToString();
                                }
                                else
                                {
                                    Fp_Helper.Sheets[0].Cells[Fp_Helper.Sheets[0].RowCount - 1, 4].Text = staf_phone.ToString();
                                }
                                if (ref_info != "")
                                {
                                    string[] spli_ref_info = ref_info.Split(';');
                                    if (spli_ref_info[0] != ";" && spli_ref_info[0] != "")
                                    {
                                        Fp_Helper.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 5].Text = spli_ref_info[0].ToString();
                                    }
                                    else
                                    {
                                        Fp_Helper.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 5].Text = "-";
                                    }
                                    if (spli_ref_info[0] != ";" && spli_ref_info[0] != "")
                                    {
                                        Fp_Helper.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 6].Text = spli_ref_info[1].ToString();
                                    }
                                    else
                                    {
                                        Fp_Helper.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 6].Text = "-";
                                    }
                                    if (spli_ref_info[8] != ";" && spli_ref_info[8] != "")
                                    {
                                        Fp_Helper.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 7].Text = spli_ref_info[8].ToString();
                                    }
                                    else
                                    {
                                        Fp_Helper.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 7].Text = "-";
                                    }

                                }

                                MyImg3 driver_photo = new MyImg3();
                                driver_photo.ImageUrl = "~/images/10BIT001.jpeg";
                                driver_photo.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + staf_code;

                                Fp_Helper.Sheets[0].Cells[Fp_Helper.Sheets[0].RowCount - 1, 8].CellType = driver_photo;
                                Fp_Driver.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                                txt_helper.Text = txtbox_text;
                                Fp_Helper.Sheets[0].PageSize = Fp_Helper.Sheets[0].RowCount;

                                Fill_Grid1();
                            }

                        }
                    }

                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Please select a staff name";
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please select a staff name')", true);
                }
                //panel8.Visible = false;
            }

            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please select a staff name";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please select a staff name')", true);
            }

            Fill_Grid();
        }

        catch (Exception ex)
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Please select a staff name";
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please select a staff name')", true);
        }
    }

    protected void exitpop_Click(object sender, EventArgs e)
    {
        //panel8.Visible = false;

    }

    byte[] ReadFile(string sPath)
    {
        byte[] data = null;
        System.IO.FileInfo fInfo = new System.IO.FileInfo(sPath);
        long numBytes = fInfo.Length;
        System.IO.FileStream fStream = new System.IO.FileStream(sPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

        System.IO.BinaryReader br = new System.IO.BinaryReader(fStream);
        data = br.ReadBytes((int)numBytes);
        fStream.Dispose();
        br.Dispose();
        return data;

    }

    protected void Btn_save_Click(object sender, EventArgs e)
    {
        if (ddl_vehid.Items.Count > 0)
        {
            if (ddl_vehid.SelectedItem.ToString() == "")
            {

                imgAlert.Visible = true;
                lbl_alert.Text = "Please select the vehicle.";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please select the vehicle.')", true);
                return;
            }
        }
        else
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Please select the vehicle.";
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please select the vehicle.')", true);
            return;
        }

        if (ddl_routeid.Items.Count > 0)
        {
            if (ddl_routeid.SelectedItem.ToString() == "")
            {

                imgAlert.Visible = true;
                lbl_alert.Text = "Please select the route.";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please select the route.')", true);
                return;
            }
        }
        else
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Please select the route.";
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please select the route.')", true);
            return;
        }

        try
        {
            lbl_Validation.Visible = false;

            Fp_Driver.SaveChanges();
            Fp_Helper.SaveChanges();
            string dri_design = "Driver";
            string hlpr_design = string.Empty;

            string temp_alert = string.Empty;
            string temp_stf_code = string.Empty;

            //if (Btn_save.Text == "Update")//rajasekar 04/06/2018
            //{
            //    con.Close();
            //    con.Open();
            //    SqlCommand cmd_delete = new SqlCommand("Delete from DriverAllotment where Vehicle_Id='" + ddl_vehid.Text + "' and Route_Id='" + ddl_routeid.Text + "'", con);
            //    cmd_delete.ExecuteNonQuery();

            //}

            if (txt_dvr_name.Text != "")
            {
                con.Close();
                con.Open();

                //SqlCommand cmd_temp_data = new SqlCommand("Select * from Temp_Table", con);
                //SqlDataAdapter ad_temp_data = new SqlDataAdapter(cmd_temp_data);
                //DataTable dt_temp_data = new DataTable();
                //ad_temp_data.Fill(dt_temp_data);

                for (int cnt = 0; cnt < Fp_Driver.Rows.Count; cnt++)
                {
                    string stf_code = Fp_Driver.Sheets[0].Cells[cnt, 1].Text;

                    //DataView dv_temp_data = new DataView();
                    //dt_temp_data.DefaultView.RowFilter = "Staff_Code='" + stf_code + "' and Type='Front'";
                    //dv_temp_data = dt_temp_data.DefaultView;

                    //DataView dv_temp_data1 = new DataView();
                    //dt_temp_data.DefaultView.RowFilter = "Staff_Code='" + stf_code + "' and Type='Back'";
                    //dv_temp_data1 = dt_temp_data.DefaultView;

                    //if (dv_temp_data.Count > 0 && dv_temp_data1.Count > 0)
                    //{

                    //}

                    //else
                    //{
                    //    temp_alert = "No";

                    //    if (temp_stf_code == "")
                    //    {
                    //        temp_stf_code = stf_code;
                    //    }
                    //    else
                    //    {
                    //        temp_stf_code = temp_stf_code + "," + stf_code;
                    //    }
                    //}
                }

                //if (temp_alert == "No")
                //{
                //    lbl_Validation.Text = "Please Upload Licence for the following staff(s) "+temp_stf_code;
                //    lbl_Validation.Visible = true;
                //}
                //else
                //{
                //for (int cnt = 0; cnt < Fp_Driver.Rows.Count; cnt++)
                //{
                //    if (Fp_Driver.Sheets[0].Cells[cnt, 11].Text.ToString() == "")
                //    {
                //        lbl_Validation.Text = "Please select licence renew date for all drivers";
                //        lbl_Validation.Visible = true;
                //        return;
                //    }
                //    string renewdate = Fp_Driver.Sheets[0].Cells[cnt, 11].Text.ToString();
                //    string[] renewarr = renewdate.Split('/');
                //    if (!(renewarr.Length > 1))
                //    {
                //        lbl_Validation.Text = "Please select licence renew date in the format of dd/mm/yyyy";
                //        lbl_Validation.Visible = true;
                //        return;
                //    }
                    

                //}


                for (int cnt = 0; cnt < Fp_Driver.Rows.Count; cnt++)
                {
                    string vehid = ddl_vehid.Text;
                    string routeid = ddl_routeid.Text;
                    string stf_code = Fp_Driver.Sheets[0].Cells[cnt, 1].Text;
                    string stf_name = Fp_Driver.Sheets[0].Cells[cnt, 2].Text;
                    string stf_address = Fp_Driver.Sheets[0].Cells[cnt, 3].Text;
                    string stf_mob = Fp_Driver.Sheets[0].Cells[cnt, 4].Text;
                    string ref_name = Fp_Driver.Sheets[0].Cells[cnt, 5].Text;
                    string ref_address = Fp_Driver.Sheets[0].Cells[cnt, 6].Text;
                    string ref_mob = Fp_Driver.Sheets[0].Cells[cnt, 7].Text;

                    

                    //string[] spl_date = Fp_Driver.Sheets[0].Cells[cnt, 11].Text.ToString().Split('/');

                    //string renew_date = spl_date[1] + "/" + spl_date[2] + "/" + spl_date[0];

                    //DataView dv_temp_data = new DataView();
                    //dt_temp_data.DefaultView.RowFilter = "Staff_Code='" + stf_code + "'";
                    //dv_temp_data = dt_temp_data.DefaultView;

                    //byte[] licence_front;
                    //byte[] licence_back;

                    //rajasekar 04/06/2018
                    //if (dv_temp_data.Count == 1)
                    //{

                    //    if (dv_temp_data[0]["Type"].ToString() == "Back")
                    //    {

                    //        imgAlert.Visible = true;
                    //        lbl_alert.Text = "Please select Licence Front Photo";
                    //           // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please select Licence Front Photo')", true);
                    //            return;
                            
                    //    }

                    //    if (dv_temp_data[0]["Type"].ToString() == "Front")
                    //    {
                    //        imgAlert.Visible = true;
                    //        lbl_alert.Text = "Please select Licence Back Photo";
                    //            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please select Licence Back Photo')", true);
                    //            return;
                    //    }
                        
                    //}

                    

                    if (Btn_save.Text == "Update")
                    {
                        con.Close();
                        con.Open();
                        SqlCommand cmd_delete = new SqlCommand("Delete from DriverAllotment where Vehicle_Id='" + ddl_vehid.Text + "' and Route_Id='" + ddl_routeid.Text + "'", con);
                        cmd_delete.ExecuteNonQuery();

                    }


                    //if (dv_temp_data.Count == 2)//rajasekar 04/06/2018
                    //{




                    //    licence_front = (byte[])dv_temp_data[0]["File_Name"];
                    //    licence_back = (byte[])dv_temp_data[1]["File_Name"];

                    //    con.Close();
                    //    con.Open();
                    //    SqlCommand cmd_insert_driver = new SqlCommand("insert into driverallotment(Vehicle_Id,Route_Id,Staff_Code,Staff_Name,Address,Mobile_No,Licence_Front,Licence_Back,Ref_Name,Ref_Address,Ref_Mobile,Design,Renew_Date,Remainder,Last_Remin) values('" + vehid + "','" + routeid + "','" + stf_code + "','" + stf_name + "','" + stf_address + "','" + stf_mob + "',@frontlicence,@backlicence,'" + ref_name + "','" + ref_address + "','" + ref_mob + "','" + dri_design + "','" + renew_date + "','0','')", con);

                    //    cmd_insert_driver.Parameters.AddWithValue("@frontlicence", (object)licence_front);
                    //    cmd_insert_driver.Parameters.AddWithValue("@backlicence", (object)licence_back);

                    //    cmd_insert_driver.ExecuteNonQuery();
                    //}
                    //else
                    //{
                        con.Close();
                        con.Open();
                        SqlCommand cmd_insert_driver = new SqlCommand("insert into driverallotment(Vehicle_Id,Route_Id,Staff_Code,Staff_Name,Address,Mobile_No,Licence_Front,Licence_Back,Ref_Name,Ref_Address,Ref_Mobile,Design,Renew_Date,Remainder,Last_Remin) values('" + vehid + "','" + routeid + "','" + stf_code + "','" + stf_name + "','" + stf_address + "','" + stf_mob + "','','','" + ref_name + "','" + ref_address + "','" + ref_mob + "','" + dri_design + "','','0','')", con);

                        //cmd_insert_driver.Parameters.AddWithValue("@frontlicence", (object)licence_front);
                        //cmd_insert_driver.Parameters.AddWithValue("@backlicence", (object)licence_back);

                        cmd_insert_driver.ExecuteNonQuery();
                    //}



                    if (Btn_save.Text == "Update")
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Updated Successfully";
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Updated Successfully')", true);
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Saved Successfully";
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                    }

                }

                for (int cnt = 0; cnt < Fp_Helper.Rows.Count; cnt++)
                {
                    string vehid = ddl_vehid.Text;
                    string routeid = ddl_routeid.Text;
                    string stf_code = Fp_Helper.Sheets[0].Cells[cnt, 1].Text;
                    string stf_name = Fp_Helper.Sheets[0].Cells[cnt, 2].Text;
                    string stf_address = Fp_Helper.Sheets[0].Cells[cnt, 3].Text;
                    string stf_mob = Fp_Helper.Sheets[0].Cells[cnt, 4].Text;
                    string ref_name = Fp_Helper.Sheets[0].Cells[cnt, 5].Text;
                    string ref_address = Fp_Helper.Sheets[0].Cells[cnt, 6].Text;
                    string ref_mob = Fp_Helper.Sheets[0].Cells[cnt, 7].Text;
                    hlpr_design = Fp_Helper.Sheets[0].Cells[cnt, 9].Text;
                    string licence_front = string.Empty;
                    string licence_back = string.Empty;

                    con.Close();
                    con.Open();
                    SqlCommand cmd_insert_driver = new SqlCommand("insert into driverallotment(Vehicle_Id,Route_Id,Staff_Code,Staff_Name,Address,Mobile_No,Licence_Front,Licence_Back,Ref_Name,Ref_Address,Ref_Mobile,Design,Renew_Date,Remainder,Last_Remin) values('" + vehid + "','" + routeid + "','" + stf_code + "','" + stf_name + "','" + stf_address + "','" + stf_mob + "','" + licence_front + "','" + licence_back + "','" + ref_name + "','" + ref_address + "','" + ref_mob + "','" + hlpr_design + "','','','')", con);
                    cmd_insert_driver.ExecuteNonQuery();
                    if (Btn_save.Text == "Update")
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Updated Successfully";
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Updated Successfully')", true);
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Saved Successfully";
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                    }
                }
                new_page();
                //}
            }
            else
            {
                lbl_Validation.Text = "Please enter driver details";
                lbl_Validation.Visible = true;
            }

            btnMainGo_Click(sender, e);

        }
        catch (Exception ex)
        {
            da2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Special Hour Entry");
        }
    }

    protected void btn_delete_click(object sender, EventArgs e)
    {
        if (txt_dvr_name.Text != "")
        {
            con.Close();
            con.Open();

            SqlCommand cmd_delete = new SqlCommand("Delete from DriverAllotment where Vehicle_Id='" + ddl_vehid.Text + "' and Route_Id='" + ddl_routeid.Text + "'", con);
            cmd_delete.ExecuteNonQuery();


            imgAlert.Visible = true;
            lbl_alert.Text = "Deleted Successfully";
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
            new_page();
        }
        else
        {
            lbl_Validation.Text = "Driver name should not be empty";
            lbl_Validation.Visible = true;
        }
    }

    protected void FpSpread1_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

        string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
        Cellclick = true;
        Accordion1.SelectedIndex = 1;

    }

    protected void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cellclick == true)
        {
            new_page();

            string txt_dvr = string.Empty;
            string txt_hlpr = string.Empty;

            lbl_add.Text = "Modify";
            Btn_save.Text = "Update";
            btn_delete.Enabled = true;
            string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();

            string Veh_ID = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;

            con.Close();
            con.Open();
            SqlCommand cmd_edit = new SqlCommand("Select * from DriverAllotment where Vehicle_id='" + Veh_ID + "'", con);
            SqlDataAdapter ad_edit = new SqlDataAdapter(cmd_edit);
            DataTable dt_edit = new DataTable();
            ad_edit.Fill(dt_edit);

            if (dt_edit.Rows.Count > 0)
            {
                ddl_vehid.Text = dt_edit.Rows[0]["Vehicle_Id"].ToString();
                ddl_routeid.Text = dt_edit.Rows[0]["Route_Id"].ToString();

                DataView dv_driver = new DataView();
                dt_edit.DefaultView.RowFilter = "Design='Driver'";
                dv_driver = dt_edit.DefaultView;
                Fp_Driver.Sheets[0].RowCount = 0;
                if (dv_driver.Count > 0)
                {
                    int slno = 0;
                    for (int i = 0; i < dv_driver.Count; i++)
                    {
                        slno++;
                        Fp_Driver.Sheets[0].RowCount = Convert.ToInt32(Fp_Driver.Sheets[0].RowCount) + 1;

                        Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 0].Text = slno.ToString();
                        Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 1].Text = dv_driver[i]["staff_code"].ToString();
                        Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 2].Text = dv_driver[i]["staff_name"].ToString();
                        Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 3].Text = dv_driver[i]["address"].ToString();
                        Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 4].Text = dv_driver[i]["Mobile_No"].ToString();
                        Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 5].Text = dv_driver[i]["Ref_Name"].ToString();
                        Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 6].Text = dv_driver[i]["Ref_Address"].ToString();
                        Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 7].Text = dv_driver[i]["Ref_Mobile"].ToString();

                        //string renew_date = dv_driver[i]["renew_date"].ToString();

                        string renew_date = dacces2.GetFunction("select LicenseRenewDate from StaffPhoto where staff_code='"+ dv_driver[i]["staff_code"].ToString()+"'");


                        if (renew_date != "1/1/1900 12:00:00 AM" && renew_date != "" && renew_date != "0")
                        {
                            string[] spl_renew = renew_date.Split(' ');

                            string[] spl_date = spl_renew[0].Split('/');

                            renew_date = spl_date[2] + "/" + spl_date[0] + "/" + spl_date[1];

                            Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 11].Text = renew_date;
                        }

                        if (txt_dvr == "")
                        {
                            txt_dvr = dv_driver[i]["staff_code"].ToString(); ;
                        }
                        else
                        {
                            txt_dvr = txt_dvr + "," + dv_driver[i]["staff_code"].ToString(); ;
                        }
                        MyImg3 driver_photo = new MyImg3();
                        driver_photo.ImageUrl = "~/images/10BIT001.jpeg";
                        driver_photo.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + dv_driver[i]["staff_code"].ToString();

                        Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 8].CellType = driver_photo;


                        MyImg3 Lin_Front = new MyImg3();
                        Lin_Front.ImageUrl = "~/images/10BIT001.jpeg";
                        Lin_Front.ImageUrl = "~/Handler/licencefront.ashx?Staff_code=" + dv_driver[i]["staff_code"].ToString();

                        Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 9].CellType = Lin_Front;
                        Fp_Driver.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Justify;

                        MyImg3 Lin_Back = new MyImg3();
                        Lin_Back.ImageUrl = "~/images/10BIT001.jpeg";
                        Lin_Back.ImageUrl = "~/Handler/LicenceBack.ashx?Staff_code=" + dv_driver[i]["staff_code"].ToString();

                        Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 10].CellType = Lin_Back;
                        Fp_Driver.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Justify;


                        Fp_Driver.Sheets[0].PageSize = Fp_Driver.Sheets[0].RowCount;
                    }

                    txt_dvr_name.Text = txt_dvr;

                }

                DataView dv_helper = new DataView();
                dt_edit.DefaultView.RowFilter = "Design<>'Driver'";
                dv_helper = dt_edit.DefaultView;

                Fp_Helper.Sheets[0].RowCount = 0;
                if (dv_helper.Count > 0)
                {

                    //txt_numhelper.Text = dv_helper.Count.ToString();

                    int slno = 0;
                    for (int i = 0; i < dv_helper.Count; i++)
                    {
                        slno++;
                        Fp_Helper.Sheets[0].RowCount = Convert.ToInt32(Fp_Helper.Sheets[0].RowCount) + 1;

                        Fp_Helper.Sheets[0].Cells[Fp_Helper.Sheets[0].RowCount - 1, 0].Text = slno.ToString();
                        Fp_Helper.Sheets[0].Cells[Fp_Helper.Sheets[0].RowCount - 1, 1].Text = dv_helper[i]["staff_code"].ToString();
                        Fp_Helper.Sheets[0].Cells[Fp_Helper.Sheets[0].RowCount - 1, 2].Text = dv_helper[i]["staff_name"].ToString();
                        Fp_Helper.Sheets[0].Cells[Fp_Helper.Sheets[0].RowCount - 1, 3].Text = dv_helper[i]["address"].ToString();
                        Fp_Helper.Sheets[0].Cells[Fp_Helper.Sheets[0].RowCount - 1, 4].Text = dv_helper[i]["Mobile_No"].ToString();
                        Fp_Helper.Sheets[0].Cells[Fp_Helper.Sheets[0].RowCount - 1, 5].Text = dv_helper[i]["Ref_Name"].ToString();
                        Fp_Helper.Sheets[0].Cells[Fp_Helper.Sheets[0].RowCount - 1, 6].Text = dv_helper[i]["Ref_Address"].ToString();
                        Fp_Helper.Sheets[0].Cells[Fp_Helper.Sheets[0].RowCount - 1, 7].Text = dv_helper[i]["Ref_Mobile"].ToString();
                        Fp_Helper.Sheets[0].Cells[Fp_Helper.Sheets[0].RowCount - 1, 9].Text = dv_helper[i]["Design"].ToString();


                        if (txt_hlpr == "")
                        {
                            txt_hlpr = dv_helper[i]["staff_code"].ToString();
                        }
                        else
                        {
                            txt_hlpr = txt_hlpr + "," + dv_helper[i]["staff_code"].ToString();
                        }

                        MyImg3 driver_photo = new MyImg3();
                        driver_photo.ImageUrl = "~/images/10BIT001.jpeg";
                        driver_photo.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + dv_helper[i]["staff_code"].ToString();

                        Fp_Helper.Sheets[0].Cells[Fp_Helper.Sheets[0].RowCount - 1, 8].CellType = driver_photo;

                        Fp_Helper.Sheets[0].PageSize = Fp_Helper.Sheets[0].RowCount;
                    }

                    txt_helper.Text = txt_hlpr;
                }
            }
        }

        Fill_Grid();
        Fill_Grid1();
    }

    protected void Fp_Driver_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
        Cellclick1 = true;
    }

    protected void Fp_Driver_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cellclick1 == true)
        {
            PanelUpload.Visible = true;
        }
    }

    protected void Btn_reset_Click(object sender, EventArgs e)
    {

        clear_all();
    }

    protected void Btn_cancel_Click(object sender, EventArgs e)
    {
        new_page();
    }

    protected void fsstaff_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void fsstaff_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

    }

    protected void btn_upload_Click(object sender, EventArgs e)
    {
        try
        {
            lblup_error.Visible = false;
            string activerow = Fp_Driver.ActiveSheetView.ActiveRow.ToString();
            string activecol = Fp_Driver.ActiveSheetView.ActiveColumn.ToString();

            string staff_cod = Fp_Driver.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;

            if (staff_cod != "")
            {
                if (licence_upload.FileName != "")
                {
                    if (licence_upload.FileName.EndsWith(".jpg") || licence_upload.FileName.EndsWith(".gif") || licence_upload.FileName.EndsWith(".png") || licence_upload.FileName.EndsWith(".jpeg"))
                    {
                        string img_type = string.Empty;

                        if (Convert.ToInt32(activecol) == 9)
                        {
                            img_type = "Front";
                        }
                        else if (Convert.ToInt32(activecol) == 11)
                        {
                            img_type = "Back";
                        }

                        con.Close();
                        con.Open();

                        SqlCommand cmd_delete = new SqlCommand("Delete from Temp_Table where Staff_Code='" + staff_cod + "' and Type='" + img_type + "'", con);
                        cmd_delete.ExecuteNonQuery();

                        //licence_upload.SaveAs(Server.MapPath("image") + "\\" + licence_upload.FileName);
                        //licence_upload.SaveAs(Server.MapPath("image") + "\\" + licence_upload.FileName);

                        //byte[] licence_front = ReadFile(Server.MapPath("image") + "\\" + licence_upload.FileName);
                        //byte[] licence_back = ReadFile(Server.MapPath("image") + "\\" + licence_upload.FileName);


                        licence_upload.SaveAs(Server.MapPath("~/image/") + licence_upload.FileName);
                        //licence_upload.SaveAs(Server.MapPath("image") + "\\" + licence_upload.FileName);

                        byte[] licence_front = ReadFile(Server.MapPath("~/image/") + licence_upload.FileName);
                        //byte[] licence_back = ReadFile(Server.MapPath("image") + "\\" + licence_upload.FileName);
                        Session["FileName"]=licence_upload.FileName;
                        con.Close();
                        con.Open();

                        //rajasekar 23/3/2018
                        if (img_type == "Front")
                        {
                            MyImg3 Lin_Front = new MyImg3();
                            Lin_Front.ImageUrl = "~/image/" + licence_upload.FileName;
                            //Lin_Front.ImageUrl = "image" + "\\" + licence_upload.FileName;
                            //Lin_Front.ImageUrl = "image" + "\\" + licence_upload.FileName;
                            Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 10].CellType = Lin_Front;
                            Fp_Driver.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Justify;

                        }
                        else if (img_type == "Back")
                        {
                            MyImg3 Lin_Back = new MyImg3();
                            Lin_Back.ImageUrl = "~/image/" + licence_upload.FileName;
                            //Lin_Back.ImageUrl = "image" + "\\" + licence_upload.FileName;
                            //Lin_Back.ImageUrl = "image" + "\\" + licence_upload.FileName;

                            Fp_Driver.Sheets[0].Cells[Fp_Driver.Sheets[0].RowCount - 1, 12].CellType = Lin_Back;
                            Fp_Driver.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Justify;

                        }

                        //rajasekar



                        SqlCommand cmd_temp_insert = new SqlCommand("insert into Temp_Table values('" + staff_cod + "',@frontlicence,'" + img_type + "')", con);
                        cmd_temp_insert.Parameters.AddWithValue("@frontlicence", (object)licence_front);

                        cmd_temp_insert.ExecuteNonQuery();

                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Uploaded Successfully')", true);
                        PanelUpload.Visible = false;

                    }


                    else
                    {
                        lblup_error.Text = "File not correct format. Only allowed (.jpg,.png,.gif).";
                        lblup_error.Visible = true;

                    }
                }
                else
                {
                    lblup_error.Text = "Please select file.";
                    lblup_error.Visible = true;

                }
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please First select the Driver.";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please First select the Driver.')", true);
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btn_close_Click(object sender, EventArgs e)
    {
        PanelUpload.Visible = false;
    }

    protected void btnclose_view_Click(object sender, EventArgs e)
    {
        Panel_View.Visible = false;
    }

    protected void FpDriver_ButtonCommand(object sender, EventArgs e)
    {


        string activerow = Fp_Driver.ActiveSheetView.ActiveRow.ToString();
        string activecol = Fp_Driver.ActiveSheetView.ActiveColumn.ToString();

        string staff_cod = Fp_Driver.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;

        if (Convert.ToInt32(activecol) == 9)
        {
            if (staff_cod != "")
            {
                PanelUpload.Visible = true;
            }
            else
            {

                imgAlert.Visible = true;
                lbl_alert.Text = "Please First select the Driver.";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please First select the Driver.')", true);
            }

        }

        else if (Convert.ToInt32(activecol) == 10)
        {

            if (staff_cod != "")
            {
                con.Close();
                con.Open();
                SqlCommand cmd_view = new SqlCommand("Select * from Temp_Table where Staff_Code='" + staff_cod + "' and Type='Front'", con);
                SqlDataAdapter ad_view = new SqlDataAdapter(cmd_view);
                DataTable dt_viwe = new DataTable();
                ad_view.Fill(dt_viwe);
                



                //added by rajasekar 21/07/2018
                DataSet ds11 = new DataSet();
                ad_view.Fill(ds11);
                string photo = "";
                byte[] photoid = new byte[0];
                if (ds11.Tables[0].Rows.Count > 0)
                {
                    if (ds11.Tables[0].Rows[0]["File_Name"] != null && Convert.ToString(ds11.Tables[0].Rows[0]["File_Name"]) != "")
                    {
                        photoid = (byte[])(ds11.Tables[0].Rows[0]["File_Name"]);
                        if (photoid.Length > 0)
                        {

                             string base64String = Convert.ToBase64String(photoid) ;

                            photo = "'data:image/png;base64," + Convert.ToBase64String(photoid) + "'";
                            Panel_View.Visible = true;
                            Img_Licence.ImageUrl = String.Format("data:image/jpg;base64,{0}", base64String);
                            Img_Licence.Visible = true;
                        }
                    }
                }

                //if (dt_viwe.Rows.Count > 0)
                //{

                //    Panel_View.Visible = true;
                //    Img_Licence.ImageUrl = "Handler/ViewLicence.ashx?staff_code=" + staff_cod;
                //    Img_Licence.Visible = true;


                //}
                //=====================================//
                else
                {

                    imgAlert.Visible = true;
                    lbl_alert.Text = "No images uploaded yet.";
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No images uploaded yet.')", true);
                }
            }

            else
            {

                imgAlert.Visible = true;
                lbl_alert.Text = "Please First select the Driver.";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please First select the Driver.')", true);
            }
        }

        else if (Convert.ToInt32(activecol) == 11)
        {
            if (staff_cod != "")
            {
                PanelUpload.Visible = true;
            }
            else
            {

                imgAlert.Visible = true;
                lbl_alert.Text = "Please First select the Driver.";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please First select the Driver.')", true);
            }

        }

        else if (Convert.ToInt32(activecol) == 12)
        {

            if (staff_cod != "")
            {
                con.Close();
                con.Open();
                SqlCommand cmd_view = new SqlCommand("Select * from Temp_Table where Staff_Code='" + staff_cod + "' and Type='Back'", con);
                SqlDataAdapter ad_view = new SqlDataAdapter(cmd_view);
                DataTable dt_viwe = new DataTable();
                ad_view.Fill(dt_viwe);




                //added by rajasekar 21/07/2018
                DataSet ds11 = new DataSet();
                ad_view.Fill(ds11);
                string photo = "";
                byte[] photoid = new byte[0];
                if (ds11.Tables[0].Rows.Count > 0)
                {
                    if (ds11.Tables[0].Rows[0]["File_Name"] != null && Convert.ToString(ds11.Tables[0].Rows[0]["File_Name"]) != "")
                    {
                        photoid = (byte[])(ds11.Tables[0].Rows[0]["File_Name"]);
                        if (photoid.Length > 0)
                        {

                            string base64String = Convert.ToBase64String(photoid);

                            photo = "'data:image/png;base64," + Convert.ToBase64String(photoid) + "'";
                            Panel_View.Visible = true;
                            Img_Licence.ImageUrl = String.Format("data:image/jpg;base64,{0}", base64String);
                            Img_Licence.Visible = true;
                        }
                    }
                }
                //if (dt_viwe.Rows.Count > 0)
                //{

                //    Panel_View.Visible = true;
                //    Img_Licence.ImageUrl = "ViewHandler/LicenceBack.ashx?staff_code=" + staff_cod;
                //    Img_Licence.Visible = true;
                //}
                //=====================================//
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "No images uploaded yet.";

                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No images uploaded yet.')", true);
                }
            }

            else
            {

                imgAlert.Visible = true;
                lbl_alert.Text = "Please First select the Driver.";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please First select the Driver.')", true);
            }
        }
    }

    void new_page()
    {
        lbl_Validation.Visible = false;
        Btn_save.Text = "Save";

        Fp_Driver.Sheets[0].RowCount = 0;
        Fp_Helper.Sheets[0].RowCount = 0;
        txt_dvr_name.Text = "";
        txt_helper.Text = "";

        con.Close();
        con.Open();

        SqlCommand cmd_vehicle_id = new SqlCommand("select * from vehicle_master order by len(veh_id), Veh_ID", con);
        SqlDataReader rdr_vehicle_id = cmd_vehicle_id.ExecuteReader();
        int incre_veh = 0;

        ddl_vehid.Items.Clear();
        while (rdr_vehicle_id.Read())
        {
            if (rdr_vehicle_id.HasRows == true)
            {
                incre_veh++;
                System.Web.UI.WebControls.ListItem list_vehicle_id = new System.Web.UI.WebControls.ListItem();

                list_vehicle_id.Text = (rdr_vehicle_id["Veh_ID"].ToString());


                string ddl_items = list_vehicle_id.Text;


                ddl_vehid.Items.Add(ddl_items);

            }
        }


        con.Close();
        con.Open();
        SqlCommand cmd_route = new SqlCommand("select distinct Route_ID from routemaster", con);
        SqlDataReader rdr_route = cmd_route.ExecuteReader();

        int incre_route = 0;
        ddl_routeid.Items.Clear();
        while (rdr_route.Read())
        {
            if (rdr_route.HasRows == true)
            {
                incre_route++;
                System.Web.UI.WebControls.ListItem list_route = new System.Web.UI.WebControls.ListItem();


                list_route.Text = (rdr_route["Route_ID"].ToString());


                string ddl_item = list_route.Text;


                ddl_routeid.Items.Add(ddl_item);


            }
        }
    }
    protected void btn_alertclose_Click(object sender, EventArgs e)
    {

        imgAlert.Visible = false;

    }
    void clear_all()
    {

        txt_helper.Text = "";
        txt_dvr_name.Text = "";

        Driver_Img.Dispose();
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        if (FpSpread1.Sheets[0].RowCount > 0)
        {
            Session["column_header_row_count"] = 1;
            string degreedetails = "Driver Details";
            string pagename = "Driver_Allotment.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)//rajasekar
    {
        fsstaff.Sheets[0].RowCount = 0;
        loadstaffdep(collegecode);
        bind_stafType();
        mpedirect.Show();
        //loadfsstaff();
    }
}