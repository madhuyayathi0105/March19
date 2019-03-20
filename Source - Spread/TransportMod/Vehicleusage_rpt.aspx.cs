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
using System.IO;
using Gios.Pdf;
using System.Text.RegularExpressions;

public partial class Vehicleusage_rpt : System.Web.UI.Page
{
    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = "";
    string strquery = "";
    string regularflag = "";
    string genderflag = "";
    string strdayflag = "";
    string sql = "";
    string sqlcondition = "";
    string collcode = "";
    string batchyear = "";
    string degreecode = "";
    string term = "";
    string sec = "";
    string rollnos = "";
    string currentsem = "";
    DataSet ds = new DataSet();
    Hashtable hat = new Hashtable();
    DAccess2 d2 = new DAccess2();
    FarPoint.Web.Spread.ComboBoxCellType combocol = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.ComboBoxCellType combocolgrade = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkboxcol = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkboxsel_all = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.TextCellType txtceltype = new FarPoint.Web.Spread.TextCellType();
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
        if (!IsPostBack)
        {
            Bind_Routes();
            bindvechicle();
            tbstart_date.Attributes.Add("readonly", "readonly");
            tbend_date.Attributes.Add("readonly", "readonly");
            tbstart_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            tbend_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            final.Visible = false;
            FpSpread1.Visible = false;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 10;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            // FpSpread1.Sheets[0].ColumnHeader.Columns[0].Width = 40;
            //FpSpread1.Sheets[0].ColumnHeader.Columns[1].Width = 50;
            //FpSpread1.Sheets[0].ColumnHeader.Columns[2].Width = 120;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No.";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Vehicle No.";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Starting Place";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Starting Date Time";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Destination";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Arrival Date Time";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Starting Km";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Ending Km";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Total Km";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Driver";

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = System.Drawing.Color.Teal;
            //darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Border.BorderSize = 1;
            darkstyle.Border.BorderColor = System.Drawing.Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            for (int i = 0; i < FpSpread1.Sheets[0].ColumnCount; i++)
            {
                FpSpread1.Sheets[0].Columns[i].Locked = true;
            }

            FpSpread1.Sheets[0].Columns[0].Width = 40;
            FpSpread1.Sheets[0].Columns[2].Width = 200;
            FpSpread1.Sheets[0].Columns[3].Width = 200;
            FpSpread1.Sheets[0].Columns[4].Width = 200;
            FpSpread1.Sheets[0].Columns[5].Width = 200;
            //FpSpread1.Sheets[0].ColumnHeader.Columns[1].Width = 50;
            //FpSpread1.Sheets[0].ColumnHeader.Columns[2].Width = 120;
            for (int i = 0; i < FpSpread1.Sheets[0].ColumnCount; i++)
            {
                FpSpread1.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Left;
            }
            for (int g = 0; g < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; g++)
            {
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, g].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, g].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, g].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, g].ForeColor = Color.White;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, g].HorizontalAlign = HorizontalAlign.Center;

                FpSpread1.Sheets[0].Columns[g].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Columns[g].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[g].Font.Bold = true;
                FpSpread1.Sheets[0].Columns[g].ForeColor = Color.Black;
            }

            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;



            chkboxsel_all.AutoPostBack = true;


            FpSpread1.SaveChanges();

            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            //---------------------------
        }
    }

    protected void Fpspread1_Command(object sender, EventArgs e)
    {

        if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 1].Value) == 1)
        {
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                FpSpread1.Sheets[0].Cells[i, 1].Value = 1;
            }
        }
        else if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 1].Value) == 0)
        {
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                FpSpread1.Sheets[0].Cells[i, 1].Value = 0;
            }

        }


    }

    protected void go_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {

            hide();
            string vehid = "";
            string routeid = "";
            string startdate = tbstart_date.Text;
            string enddate = tbend_date.Text;
            int counterr = 0;
            for (int i = 0; i < cblveh.Items.Count; i++)
            {
                if (cblveh.Items[i].Selected == true)
                {
                    counterr++;
                    if (vehid == "")
                    {
                        vehid = "" + cblveh.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        vehid = vehid + "','" + "" + cblveh.Items[i].Value.ToString() + "";
                    }
                }
            }

            if (counterr == 0)
            {
                lblerroe.Text = "Please Select Atleast One Vehicle";
                lblerroe.Visible = true;
                hide();
                return;
            }
            counterr = 0;
            for (int i = 0; i < cblrt.Items.Count; i++)
            {
                if (cblrt.Items[i].Selected == true)
                {
                    counterr++;
                    if (routeid == "")
                    {
                        routeid = "" + cblrt.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        routeid = routeid + "','" + "" + cblrt.Items[i].Value.ToString() + "";
                    }
                }
            }

            if (counterr == 0)
            {
                lblerroe.Text = "Please Select Atleast One Route ID";
                lblerroe.Visible = true;
                hide();
                return;
            }
            string date2ad = tbend_date.Text.ToString();
            string date1ad = tbstart_date.Text.ToString();
            string[] split5 = date2ad.Split(new Char[] { '/' });
            if (split5.Length == 3)
            {
                date2ad = split5[1].ToString() + "/" + split5[0].ToString() + "/" + split5[2].ToString();

            }
            split5 = date1ad.Split(new Char[] { '/' });
            if (split5.Length == 3)
            {
                date1ad = split5[1].ToString() + "/" + split5[0].ToString() + "/" + split5[2].ToString();

            }



            string sql = "select distinct Vehicle_Id,startplace,CONVERT(varchar(20),startptime,105) as sdate,startptime, CONVERT(varchar(20),arrivalptime,105) as adate,arrivalplace,arrivalptime,Opening_Km,Closing_Km,(closing_km - Opening_Km) as totalkm,staffmaster.staff_name from Vehicle_Usage ,staffmaster  where staffmaster.staff_code=Vehicle_Usage.staffcode and Vehicle_Id in ('" + vehid + "') and Route_ID in ('" + routeid + "') and startpdate >='" + date1ad + "'  and startpdate <='" + date2ad + "' and arrivalpdate >='" + date1ad + "'  and arrivalpdate <='" + date2ad + "'   order by Vehicle_Id";

            ds.Clear();

            ds.Clear();
            double totalkmgrand = 0;
            double outputdbl=0;
            FpSpread1.Sheets[0].Rows.Count = 0;
            ds = d2.select_method_wo_parameter(sql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                FpSpread1.Sheets[0].Rows.Count = ds.Tables[0].Rows.Count;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string startdatetime = Convert.ToString(ds.Tables[0].Rows[i]["sdate"]) + " " + Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[i]["startptime"])).ToString("hh:mm tt");
                    string enddatetime = Convert.ToString(ds.Tables[0].Rows[i]["adate"]) + " " + Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[i]["arrivalptime"])).ToString("hh:mm tt");

                    FpSpread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                    FpSpread1.Sheets[0].Cells[i, 1].CellType = txtceltype;
                    FpSpread1.Sheets[0].Cells[i, 1].Text = ds.Tables[0].Rows[i]["Vehicle_Id"].ToString();

                    //string place = d2.GetFunction("  select sm.Stage_Name from stage_master sm, RouteMaster rm where sm.Stage_id=rm.Stage_Name and sm.Stage_id ='" + ds.Tables[0].Rows[i]["startplace"].ToString() + "' ");

                    string place = d2.GetFunction("  select sm.Stage_Name from stage_master sm, RouteMaster rm where cast(sm.Stage_id as varchar(100))=cast(rm.Stage_Name as varchar(100)) and sm.Stage_id ='" + ds.Tables[0].Rows[i]["startplace"].ToString() + "' ");//modified by rajasekar 08/09/2018

                    FpSpread1.Sheets[0].Cells[i, 2].Text = place;
                    FpSpread1.Sheets[0].Cells[i, 3].Text = startdatetime;

                    //place = d2.GetFunction("  select sm.Stage_Name from stage_master sm, RouteMaster rm where sm.Stage_id=rm.Stage_Name and sm.Stage_id ='" + ds.Tables[0].Rows[i]["arrivalplace"].ToString() + "' ");

                    place = d2.GetFunction("  select sm.Stage_Name from stage_master sm, RouteMaster rm where cast(sm.Stage_id as varchar(100))=cast(rm.Stage_Name as varchar(100)) and sm.Stage_id ='" + ds.Tables[0].Rows[i]["arrivalplace"].ToString() + "' ");//modified by rajasekar 08/09/2018
                    FpSpread1.Sheets[0].Cells[i, 4].Text = place;
                    FpSpread1.Sheets[0].Cells[i, 5].Text = enddatetime;

                    FpSpread1.Sheets[0].Cells[i, 6].Text = ds.Tables[0].Rows[i]["Opening_Km"].ToString();
                    FpSpread1.Sheets[0].Cells[i, 7].Text = ds.Tables[0].Rows[i]["Closing_Km"].ToString();
                    FpSpread1.Sheets[0].Cells[i, 8].Text = ds.Tables[0].Rows[i]["totalkm"].ToString();

                    if (double.TryParse(ds.Tables[0].Rows[i]["totalkm"].ToString(), out outputdbl))
                    {
                        totalkmgrand=totalkmgrand+Convert.ToDouble(ds.Tables[0].Rows[i]["totalkm"].ToString());
                    }
                    FpSpread1.Sheets[0].Cells[i, 9].Text = ds.Tables[0].Rows[i]["staff_name"].ToString();

                    //FpSpread1.Sheets[0].Cells[i, 2].HorizontalAlign = HorizontalAlign.Left;
                }

                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1,0,1,8);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(totalkmgrand);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total : ";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;

                FpSpread1.SaveChanges();
                FpSpread1.Visible = true;
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                final.Visible = true;
                lblerroe.Visible = false;
                for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                {
                    for (int j = 0; j < FpSpread1.Sheets[0].ColumnCount; j++)
                    {
                        FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
                    }

                }
            }
            else
            {
                lblerroe.Text = "No Records Found";
                lblerroe.Visible = true;
                hide();
            }



        }
        catch
        {
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string print = "";
            string appPath = HttpContext.Current.Server.MapPath("~");
            string strexcelname = "";
            if (appPath != "")
            {
                strexcelname = txtexcelname.Text;
                appPath = appPath.Replace("\\", "/");
                if (strexcelname != "")
                {
                    print = strexcelname;
                    //FpEntry.SaveExcel(appPath + "/Report/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + print + ".xls" + " saved in" + " " + appPath + "/Report" + " successfully')", true);
                    //Aruna on 26feb2013============================
                    string szPath = appPath + "/Report/";
                    string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")

                    FpSpread1.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.Flush();
                    Response.WriteFile(szPath + szFile);
                    //=============================================
                }
                else
                {
                    lblnorec.Text = "Please Enter Your Report Name";
                    lblnorec.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
        }

    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = true;
            lblnorec.Text = "";


            string date_filt = "From : " + tbstart_date.Text.ToString() + "   " + "To : " + tbend_date.Text.ToString();


            string degreedetails = string.Empty;

            degreedetails = "TRIP SHEET" + "@" + date_filt;
            string pagename = "Vehicleusage_rpt.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }

    }

    protected void cbveh_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblerroe.Visible = false;
            hide();
            if (cbveh.Checked == true)
            {
                int cout = 0;
                for (int i = 0; i < cblveh.Items.Count; i++)
                {
                    cout++;
                    cblveh.Items[i].Selected = true;
                    cbveh.Checked = true;
                    txtveh.Text = "Vehicle (" + cout + ")";
                }
            }
            else
            {
                int cout = 0;
                for (int i = 0; i < cblveh.Items.Count; i++)
                {
                    cout++;
                    cblveh.Items[i].Selected = false;
                    txtveh.Text = "-Select-";
                    cbveh.Checked = false;
                }
            }
            Bind_Routes();
        }
        catch (Exception ex)
        {
            //lblmsg.Visible = true;
            //lblmsg.Text = ex.ToString();
        }
    }

    protected void cblveh_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblerroe.Visible = false;
            hide();
            int cout = 0;
            cbveh.Checked = false;
            txtveh.Text = "--Select--";
            for (int i = 0; i < cblveh.Items.Count; i++)
            {
                if (cblveh.Items[i].Selected == true)
                {
                    cout++;
                }
            }
            if (cout > 0)
            {
                txtveh.Text = "Vehicle (" + cout + ")";
                if (cout == cblveh.Items.Count)
                {
                    cbveh.Checked = true;
                }
            }
            Bind_Routes();
        }
        catch (Exception ex)
        {
            //lblmsg.Visible = true;
            //lblmsg.Text = ex.ToString();
        }
    }
   
    protected void cbrt_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblerroe.Visible = false;
            hide();
            if (cbrt.Checked == true)
            {
                int cout = 0;
                for (int i = 0; i < cblrt.Items.Count; i++)
                {
                    cout++;
                    cblrt.Items[i].Selected = true;
                    cbrt.Checked = true;
                    txtroute.Text = "Route (" + cout + ")";
                }
            }
            else
            {
                int cout = 0;
                for (int i = 0; i < cblrt.Items.Count; i++)
                {
                    cout++;
                    cblrt.Items[i].Selected = false;
                    txtroute.Text = "-Select-";
                    cbrt.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            //lblmsg.Visible = true;
            //lblmsg.Text = ex.ToString();
        }
    }

    protected void cblrt_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblerroe.Visible = false;
            hide();
            int cout = 0;
            cbrt.Checked = false;
            txtroute.Text = "--Select--";
            for (int i = 0; i < cblrt.Items.Count; i++)
            {
                if (cblrt.Items[i].Selected == true)
                {
                    cout++;
                }
            }
            if (cout > 0)
            {
                txtroute.Text = "Route (" + cout + ")";
                if (cout == cblrt.Items.Count)
                {
                    cbrt.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            //lblmsg.Visible = true;
            //lblmsg.Text = ex.ToString();
        }
    }

    void Bind_Routes()
    {

        ds.Clear();
        string selveh = "";
        for (int i = 0; i < cblveh.Items.Count; i++)
        {
            if (cblveh.Items[i].Selected == true)
            {

                if (selveh == "")
                {
                    selveh = cblveh.Items[i].Text;
                }
                else
                {
                    selveh = selveh + "','" + cblveh.Items[i].Text;
                }
            }
        }
        int count_items = 0;
        string cmd_bind_route = "select distinct r.Route_ID from routemaster r,vehicle_master v where r.Route_id=v.Route and v.Veh_Id in('" + selveh + "') ";

        ds = d2.select_method_wo_parameter(cmd_bind_route, "Text");
        cblrt.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            cblrt.DataSource = ds;
            cblrt.DataTextField = "Route_ID";
            cblrt.DataBind();

        }
        else
        {
            cbrt.Checked = false;
            txtroute.Text = "--Select--";

        }
    }
   
    void bindvechicle()
    {

        ds.Clear();

        string cmd_bind_route = "select * from vehicle_master order by len(veh_id), Veh_ID";

        ds = d2.select_method_wo_parameter(cmd_bind_route, "Text");
        cblveh.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            cblveh.DataSource = ds;
            cblveh.DataTextField = "Veh_ID";
            cblveh.DataBind();

        }
        else
        {
            txtroute.Text = "--Select--";
            txtveh.Text = "--Select--";
        }

    }

    protected void tbstart_date_OnTextChanged(object sender, EventArgs e)
    {


        try
        {
          
            lblerroe.Text = "";
            lblerroe.Visible = true;
            hide();
          
            DateTime dtnow = DateTime.Now;
            lblerroe.Visible = false;
            string datefad, dtfromad;
            string datefromad;
            string yr4, m4, d4;
            datefad = tbstart_date.Text.ToString();
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
                    lblerroe.Visible = false;
                    lblerroe.Text = "Please Enter Valid From date";
                    lblerroe.Visible = true;
                    tbstart_date.Text = DateTime.Now.ToString("dd/MM/yyy");

                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnprintmaster.Visible = false;


                }

            }




            else if (tbend_date.Text == "")
            {
                lblerroe.Visible = false;
                lblerroe.Text = "Please Enter to date";
                lblerroe.Visible = true;

                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnprintmaster.Visible = false;
            }
           

        }
        catch
        {

        }

    }
   
    protected void tbend_date_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            
            lblerroe.Text = "";
            lblerroe.Visible = true;
            hide();
           
            DateTime dtnow1 = DateTime.Now;
            string date2ad;
            string datetoad;
            string yr5, m5, d5;
            date2ad = tbend_date.Text.ToString();
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
                    lblerroe.Visible = false;
                    lblerroe.Text = "Please Enter Valid To Date";
                    lblerroe.Visible = true;
                    tbend_date.Text = DateTime.Now.ToString("dd/MM/yyy");

                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnprintmaster.Visible = false;
                    goto label1;

                }
                else
                {
                    lblerroe.Visible = false;

                }
            }






            if (tbstart_date.Text != "" && tbend_date.Text != "")
            {
                lblerroe.Visible = false;
                string datefad, dtfromad;
                string datefromad;
                string yr4, m4, d4;
                datefad = tbstart_date.Text.ToString();
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
                    date2ad = tbend_date.Text.ToString();
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
                            tbend_date.Text = DateTime.Now.ToString("dd/MM/yyy");
                            tbstart_date.Text = DateTime.Now.ToString("dd/MM/yyy");
                            lblerroe.Text = "From Date Can't Be Greater Than To Date";

                            lblerroe.Visible = true;

                            lblrptname.Visible = false;
                            txtexcelname.Visible = false;
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

    public void hide()
    {
        lblnorec.Visible = false;
        Printcontrol.Visible = false;
        FpSpread1.Visible = false;
        final.Visible = false;

    }

}