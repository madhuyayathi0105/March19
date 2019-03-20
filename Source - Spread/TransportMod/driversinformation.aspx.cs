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



public partial class drivers_information : System.Web.UI.Page
{

    [Serializable()]
    public class MyImg3 : ImageCellType
    {
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            //'-------------studentphoto
            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image();
            img2.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img2.Width = Unit.Percentage(100);
            img2.Height = Unit.Percentage(100);
            return img2;

        }
    }
     [Serializable()]
    public class MyImg4 : ImageCellType
    {

        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            //'-------------licence front 
            System.Web.UI.WebControls.Image imagelicence = new System.Web.UI.WebControls.Image();
            imagelicence.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            imagelicence.Width = Unit.Percentage(80);
            imagelicence.Height = Unit.Percentage(70);
            return imagelicence;
        }
    }


    string usercode = string.Empty;
    DAccess2 d2 = new DAccess2();
    DataSet ds2 = new DataSet();
    string vech_all = string.Empty;
    string sess_all = string.Empty;
    string route_all = string.Empty;
    string ddl_sess = string.Empty;
    string drivername = "";
    string mobino = "";
    string rotid = "";
    string vecid = "";
    string frmdate = "";
    string todate = "";
    int vech_count = 0;
    Hashtable hat = new Hashtable();
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    string vech_values = string.Empty;
    string route_values = string.Empty;
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
            //btnprintmaster.Visible = false;
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
                   
                  
                //for (int i = 0; i < vehiclechecklist.Items.Count; i++)
                //{
                //    vehiclechecklist.Items[i].Selected = false;
                    
                //}
                    
            }
            con.Close();
            con.Open();
            SqlCommand cmd_route = new SqlCommand("select distinct Route_ID from routemaster ", con);
            SqlDataReader rdr_route = cmd_route.ExecuteReader();
            int route_count=0;

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
                Vehicleid.Text = "";
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
                txt_route.Text = "";
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
      //  Control cntCopyBtn = FpSpread1.FindControl("Copy");
       // Control cntCutBtn = FpSpread1.FindControl("Clear");
       // Control cntPasteBtn = FpSpread1.FindControl("Paste");
        Control cntPageNextBtn = FpSpread1.FindControl("Next");
        Control cntPagePreviousBtn = FpSpread1.FindControl("Prev");
        Control cntPageEditBtn = FpSpread1.FindControl("Edit");
        //Control cntPagePrintBtn = FpSpread1.FindControl("Print");

        if ((cntUpdateBtn != null))
        {

            TableCell tc = (TableCell)cntUpdateBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;

            tr.Cells.Remove(tc);

            tc = (TableCell)cntCancelBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPageEditBtn.Parent;
            tr.Cells.Remove(tc);

            //tc = (TableCell)cntCopyBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntCutBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPasteBtn.Parent;
            //tr.Cells.Remove(tc);

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
            con.Close();
            con.Open();
            FpSpread1.Sheets[0].RowCount = 0;

           
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

            string sqlquery = "select Vehicle_Id,route_id,Staff_Name,Mobile_No,Staff_Code from DriverAllotment where route_id in('" + route_all + "') and Vehicle_Id in('" + vech_all + "') and Design='Driver' order by vehicle_id,route_id asc";
            DataSet ds = new DataSet();

            SqlDataAdapter da = new SqlDataAdapter(sqlquery, con);
            da.Fill(ds);
            DataTable dat = new DataTable();
            dat = ds.Tables[0];

           // ds = d2.select_method(sqlquery, hat, "Text");


            if (ds.Tables[0].Rows.Count > 0)
            {
                FpSpread1.Visible = true;
                //btnprintmaster.Visible = true;
                lblerror.Visible = false;
                ddlpagecount.Visible = true;
                lblpages.Visible = true;

                lblerror1.Visible = false;
                //lblrptname.Visible = true;
                //txtexcelname.Visible = true;
                //btn_excel.Visible = true;
                
                FpSpread1.Sheets[0].SheetName = " ";
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                FpSpread1.Sheets[0].ColumnCount = 7;
                FpSpread1.Sheets[0].RowHeader.Visible = true;

                FpSpread1.Sheets[0].Rows.Default.Font.Size = FontUnit.Medium;
                Color c = FpSpread1.ColumnHeader.DefaultStyle.BackColor;
                FpSpread1.ActiveSheetView.SheetCorner.DefaultStyle.BackColor = Color.LightCyan;

                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                FpSpread1.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

                FpSpread1.Sheets[0].Columns[0].Width = 50;
                FpSpread1.Sheets[0].Columns[1].Width = 200;
                FpSpread1.Sheets[0].Columns[2].Width = 100;
                FpSpread1.Sheets[0].Columns[3].Width = 100;
                //FpSpread1.Sheets[0].Columns[4].Width = 200;
                FpSpread1.Sheets[0].Columns[5].Width = 200;
                FpSpread1.Sheets[0].Columns[6].Width = 200;

                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
               // FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[6].Locked = true;

                //  FpSpread1.Sheets[0].Columns[6].Width = 150;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
               
                //span sheetcorner column header rows 

                FpSpread1.ActiveSheetView.SheetCornerSpanModel.Add(0, 0, 2, 1);

                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 1, 2);
                //FpSpread1.Sheets[0].SpanModel.Add(0, 5, 0, 2);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Licence";

                // FpSpread1.Sheets[0].ColumnHeader.Cells[1, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Name";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Mobile No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 4, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Vehicle ID";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 5, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Route ID";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 6, 1);

                //FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Text = "Since";
                FpSpread1.Sheets[0].Columns[3].Visible = true;
                //FpSpread1.Sheets[0].Columns[4].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 5].Text = "Frontside";
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Text = "Backside";
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                string tevec = "";
                    string terou = "";
                    int slno = 0;
                for (int rowcount = 0; rowcount < ds.Tables[0].Rows.Count; rowcount++)
                {

                    string tempvec_name = string.Empty;

                    tempvec_name = dat.Rows[rowcount]["Vehicle_Id"].ToString();

                    string temproute_name = string.Empty;

                    temproute_name = dat.Rows[rowcount]["route_id"].ToString();

                   

                        if (temproute_name != "")
                        {
                            if (tevec != tempvec_name)
                            {
                        
                            dat.DefaultView.RowFilter = " route_id='" + temproute_name + "' and Vehicle_Id='" + tempvec_name + "'  ";

                            DataView dv1 = new DataView();
                            dv1 = dat.DefaultView;
                            
                            foreach (DataRowView datarowvalues in dv1)
                            {
                                slno++;
                                FpSpread1.Sheets[0].RowCount++;
                                drivername = datarowvalues["Staff_Name"].ToString();
                                mobino = datarowvalues["mobile_no"].ToString();
                                rotid = datarowvalues["route_id"].ToString();
                                vecid = datarowvalues["Vehicle_Id"].ToString();
                                //frmdate=datarowvalues["Duration_from"].ToString();
                                //todate = datarowvalues["Duration_To"].ToString();


                                string[] ffd = frmdate.Split(new char[] { ' ' });
                                string[] ttd = todate.Split(new char[] { ' ' });
                                string fr = ffd[0];
                                string tr = ttd[0];
                                string staffcode = datarowvalues["staff_code"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = slno.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = drivername.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = mobino.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = tempvec_name.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = temproute_name.ToString();


                                //   FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = fr + "-" + tr;

                                MyImg3 frontlicence = new MyImg3();
                                frontlicence.ImageUrl = "~/Handler/licencefront.ashx?Staff_code=" + staffcode;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].CellType = frontlicence;

                                MyImg3 backlicence = new MyImg3();
                                backlicence.ImageUrl = "~/Handler/LicenceBack.ashx?Staff_code=" + staffcode;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].CellType = backlicence;
                            }
                            tevec = tempvec_name;
                            terou = temproute_name;
                        }
                    }

                }

                int toalrow = FpSpread1.Sheets[0].RowCount;
                int divrow = toalrow / 7;
                int remrow = toalrow % 7;
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
               FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
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
                    FpSpread1.Sheets[0].Rows[0, (Convert.ToInt32(setpage) - 1) * 10].Visible = false;
                    int row = (Convert.ToInt32(setpage) - 1) * 7;
                    int v = FpSpread1.Sheets[0].RowCount - row;

                    if (v < 7)
                    {
                        FpSpread1.Sheets[0].Rows[row, FpSpread1.Sheets[0].RowCount - 1].Visible = true;
                    }
                    else
                    {
                        for (int iuj = 0; iuj < 7; iuj++)
                        {
                            //  FpSpread1.Sheets[0].Cells[row + iuj, 0, row + iuj, 3].Row.Visible = false;
                            FpSpread1.Sheets[0].Rows[row + iuj].Visible = true;
                        }

                    }

                    if (v > 7)
                    {
                        int hiderow = row + 7;
                        FpSpread1.Sheets[0].Rows[hiderow, FpSpread1.Sheets[0].RowCount - 1].Visible = false;
                    }
                }
                else
                {
                    FpSpread1.Sheets[0].PageSize = Convert.ToInt32(setpage) * 7;

                    int row = (Convert.ToInt32(setpage)) * 7;
                    int v = FpSpread1.Sheets[0].RowCount - row;

                    if (v < 7)
                    {
                        FpSpread1.Sheets[0].Rows[0, FpSpread1.Sheets[0].RowCount - 1].Visible = true;
                    }
                    else
                    {
                        for (int ij = 0; ij < 7; ij++)
                        {
                            //  FpSpread1.Sheets[0].Cells[row + iuj, 0, row + iuj, 3].Row.Visible = false;
                            FpSpread1.Sheets[0].Rows[row + ij].Visible = true;
                        }

                    }

                    //if(v>10)
                    //{
                    //    int hiderow = row;
                    //    FpSpread1.Sheets[0].Rows[hiderow, FpSpread1.Sheets[0].RowCount - 1].Visible = false;
                    //}
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
        string degreedetails = string.Empty;
        string section = string.Empty;
        string selected_vehicle = string.Empty;
        string selected_routeid = string.Empty;

        Session["column_header_row_count"] = 2;

        //for (int selectedrouteid = 0; selectedrouteid < checkrolist.Items.Count; selectedrouteid++)
        //{
        //    if (checkrolist.Items[selectedrouteid].Selected == true)
        //    {
        //        if (selected_routeid == "")
        //        {
        //            selected_routeid = checkrolist.Items[selectedrouteid].Text.ToString();
        //        }
        //        else
        //        {
        //            selected_routeid = selected_routeid + "," + checkrolist.Items[selectedrouteid].Text.ToString();
        //        }
        //    }
        //}

        //for (int vehi_id = 0; vehi_id < vehiclechecklist.Items.Count; vehi_id++)
        //{
        //    if (vehiclechecklist.Items[vehi_id].Selected == true)
        //    {
        //        if (selected_vehicle == "")
        //        {
        //            selected_vehicle = vehiclechecklist.Items[vehi_id].Text.ToString();
        //        }
        //        else
        //        {
        //            selected_vehicle = selected_vehicle + "," + vehiclechecklist.Items[vehi_id].Text.ToString();
        //        }
        //    }
        //}
        degreedetails = " Drivers Information Report";//@Vehicle ID:" + selected_vehicle + "@Route ID:" + selected_routeid;
        string pagename = "driversinformation.aspx";
        Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
        Printcontrol.Visible = true;
    }
}


         