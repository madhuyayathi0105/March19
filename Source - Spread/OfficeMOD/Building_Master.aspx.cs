using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Drawing;
using System.Xml.Linq;
using DalConnection;
using BalAccess;

public partial class Building_Master : System.Web.UI.Page
{
    string PageLogOut = "";
        DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    string collegecode1 = string.Empty;
    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //string sess = Convert.ToString(Session["IsLogin"]);
        //PageLogOut = Convert.ToString(Session["PageLogout"]);
        //if (sess == "")
        //{
        //}
        //else
        //{
        //    if (!Request.FilePath.Contains("OffM"))
        //    {
        //        string strPreviousPage = "";
        //        if (Request.UrlReferrer != null)
        //        {
        //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
        //        }
        //        if (strPreviousPage == "")
        //        {
        //            string redrURI = ConfigurationManager.AppSettings["Office"].Trim();
        //            Response.Redirect(redrURI, false);
        //            return;
        //        }
        //    }
        //}
        //if (Session["collegecode"] == null)
        //{
        //    string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
        //    Response.Redirect(redrURI, false);
        //    return;
        //}
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("OfficeHome"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/OfficeMOD/OfficeHome.aspx");
                return;
            }
        }
        collegecode1 = Session["collegecode"].ToString();
        //btn_go_click(sender, e);
        if (!IsPostBack)
        {

            building();
            floor();
            room();
            bindbuild();
            loaddesc1();

            //ddlbuild.Visible = true;
            //UpdatePanel1.Visible = true;
            UpdatePanel1.Visible = true;
            //UpdatePanel3.Visible = true;

            //lbl_flr.Visible = true;
            //lbl_rm.Visible = true;

            Building.Visible = true;
            txt_build.Visible = true;
            rb_building.Visible = true;
            rb_floor.Visible = true;
            rb_room.Visible = true;
            btn_new.Visible = true;
        }
    }
    protected void building()
    {
        ds.Clear();
        string item = "select distinct Building_Name,code from Building_Master order by code";
        ds = d2.select_method_wo_parameter(item, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlbuild.DataSource = ds;
            ddlbuild.DataTextField = "Building_Name";
            ddlbuild.DataValueField = "code";
            ddlbuild.DataBind();
        }
        floor();
    }



    protected void btn_new_Click(object sender, EventArgs e)
    {

        if (rb_building.Checked == true)
        {
            popper1.Visible = true;
            pop1.Visible = true;
            lbl_nofbuild.Visible = true;
            txt_nofbuild.Visible = true;
            txt_nofbuild.Text = "";
            lbl_buildacr.Visible = true;
            txt_buildacr.Visible = true;
            txt_buildacr.Text = "";
            lbl_serial.Visible = true;
            txt_serial.Visible = true;
            txt_serial.Text = "";
            btn_popgo.Visible = true;
            btn_save.Visible = false;
            btn_delete.Visible = false;
            FpSpread1.Visible = false;

            lbl_alert.Visible = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;


        }
        else
        {
            lbl_alert.Visible = true;
            lbl_alert.Text = "Please Select the Building and then Proceed ";
        }
        if (rb_floor.Checked == true)
        {
            popper1.Visible = false;
            pop1.Visible = false;
            lbl_nofbuild.Visible = false;
            txt_nofbuild.Visible = false;
            lbl_buildacr.Visible = false;
            txt_buildacr.Visible = false;
            lbl_serial.Visible = false;
            txt_serial.Visible = false;
            btn_popgo.Visible = false;

            div_floor.Visible = true;
            Div2.Visible = true;
            //ImageButton2.Visible = true;
            lbl_bname.Visible = true;
            txt_bname.Visible = true;
            //txt_bname.Text = "";
            //txt_bname.Enabled = true;
            // txt_bname.ReadOnly = false;
            txt_bname.Visible = false;
            ddlbname.Visible = true;
            lbl_totf.Visible = true;
            txt_totf.Visible = true;
            txt_totf.Text = "";
            lbl_facr.Visible = true;
            txt_facr.Visible = true;
            txt_facr.Text = "";
            lbl_ssw.Visible = true;
            txt_ssw.Visible = true;
            txt_ssw.Text = "";
            btn_fgo.Visible = true;
            btn_flrsave.Visible = false;
            btn_flrdelete.Visible = false;
            lbl_alert.Visible = false;
            FpSpread2.Visible = false;
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].ColumnCount = 0;
            bindBuilding();
            getAcronym();
            btn_fgo.Visible = true;
            txt_totf.Enabled = true;
            txt_facr.Enabled = true;
            txt_ssw.Enabled = true;
            ddlbname.Enabled = true;
        }
        else
        {
            lbl_alert.Visible = true;
            lbl_alert.Text = "Please Select the Building and then Proceed ";
            //div_floor.Visible = false;
            //ImageButton2.Visible = false;
        }
        if (rb_room.Checked == true)
        {
            div_floor.Visible = false;
            //ImageButton2.Visible = false;
            lbl_bname.Visible = false;
            txt_bname.Visible = false;
            lbl_totf.Visible = false;
            txt_totf.Visible = false;
            lbl_facr.Visible = false;
            txt_facr.Visible = false;
            lbl_ssw.Visible = false;
            txt_ssw.Visible = false;
            btn_fgo.Visible = false;

            div_room.Visible = true;
            Div3.Visible = true;
            lbl_rbn.Visible = true;
            //txt_rbn.Visible = true;
            //txt_rbn.Enabled = true;
            //txt_rbn.ReadOnly = false;
            //txt_rbn.Text = "";
            txt_rbn.Visible = false;
            ddlrbname.Visible = true;


            lbl_rflrn.Visible = true;
            //txt_rflrn.Visible = true;
            //txt_rflrn.Enabled = true;
            //txt_rflrn.ReadOnly = false;
            //txt_rflrn.Text = "";
            txt_rflrn.Visible = false;
            ddlrflrm.Visible = true;

            lbl_rtot.Visible = true;
            txt_rtot.Visible = true;
            txt_rtot.Text = "";
            lbl_racr.Visible = true;
            txt_racr.Visible = true;
            txt_racr.Text = "";
            lbl_ss.Visible = true;
            txt_ss.Visible = true;
            txt_ss.Text = "";
            btn_rsave.Visible = false;
            btn_roomgo.Visible = true;

            lbl_alert.Visible = false;
            FpSpread3.Sheets[0].RowCount = 0;
            FpSpread3.Sheets[0].ColumnCount = 0;
            bindBuilding();
            getAcronym();
            ddlrbname.Enabled = true;
            ddlrflrm.Enabled = true;
            txt_rtot.Enabled = true;
            txt_ss.Enabled = true;
            txt_racr.Enabled = true;
            FpSpread3.Visible = false;
            btn_rdelete.Visible = false;
        }
        else
        {
            lbl_alert.Visible = true;
            lbl_alert.Text = "Please Select the Building and then Proceed ";
        }
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            popper1.Visible = false;
            pop1.Visible = false;
            lbl_alert.Visible = false;
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            div_floor.Visible = false;
            Div2.Visible = false;
            lbl_alert.Visible = false;
        }
        catch (Exception ex)
        {

            Label5.Visible = true;
            Label5.Text = ex.ToString();
        }
    }
    protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            div_room.Visible = false;
            Div3.Visible = false;
            lbl_alert.Visible = false;
        }
        catch (Exception ex)
        {
            lbl_alertr.Visible = true;
            lbl_alertr.Text = ex.ToString();
        }
    }
    //entry go button
    protected void btn_go_click(object sender, EventArgs e)
    {

        try
        {
            FpSpread.Visible = false;
            btn_update.Visible = false;
            btn_Delete1.Visible = false;
            ArrayList criteriacol = new ArrayList();
            string selectquery = "";

            if (cbl_build.Items.Count != 0)
            {
                string buildcode = ""; string buildname = "";
                for (int i = 0; i < cbl_build.Items.Count; i++)
                {
                    if (cbl_build.Items[i].Selected == true)
                    {

                        if (buildcode == "")
                        {
                            buildcode = "" + cbl_build.Items[i].Value.ToString() + "";
                            buildname = "" + cbl_build.Items[i].Text.ToString() + "";
                        }
                        else
                        {
                            buildcode = buildcode + "'" + "," + "'" + cbl_build.Items[i].Value.ToString() + "";
                            buildname = buildname + "'" + "," + "'" + cbl_build.Items[i].Text.ToString() + "";
                        }
                    }
                }
                if (buildcode.Trim() == "")
                {
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Please Select the Building and then Proceed ";
                    return;
                }
                if (rdb_report.Checked == true)
                {
                    string flr = "";
                    for (int i = 0; i < cbl_flr.Items.Count; i++)
                    {
                        if (cbl_flr.Items[i].Selected == true)
                        {
                            if (flr == "")
                            {
                                flr = "" + cbl_flr.Items[i].Value.ToString() + "";
                            }
                            else
                            {
                                flr = flr + "'" + "," + "'" + cbl_flr.Items[i].Value.ToString() + "";
                            }
                        }
                    }
                    string rm = "";
                    for (int i = 0; i < cbl_rm.Items.Count; i++)
                    {
                        if (cbl_rm.Items[i].Selected == true)
                        {
                            if (rm == "")
                            {
                                rm = "" + cbl_rm.Items[i].Value.ToString() + "";
                            }
                            else
                            {
                                rm = rm + "'" + "," + "'" + cbl_rm.Items[i].Value.ToString() + "";
                            }
                        }
                    }
                    string build = Convert.ToString(ddlbuild.SelectedItem);
                    selectquery = "select Building_Name,Floor_Name,Room_Name,Room_type,room_size,students_allowed,StudPerSeat from Room_Detail where Building_Name ='" + build + "' and Floor_Name in('" + flr + "') and Room_Name in('" + rm + "') and College_Code='" + collegecode1 + "'";
                }
                if (rdb_detail.Checked == true)
                {

                    //if (rb_floor.Checked == true || rb_room.Checked == true || rb_floor.Checked == true || rb_room.Checked == true || rb_building.Checked == true || rb_room.Checked == true)
                    //{
                    //    FpSpread.Visible = false;
                    //    lbl_alert.Visible = true;
                    //    btn_update.Visible = false;
                    //    btn_Delete1.Visible = false;
                    //    lbl_alert.Text = "Please Select Building , Floor ,Room this format";
                    //}
                    if (rb_building.Checked == true)
                    {
                        selectquery = "select Code,Building_Acronym,StartingSerial,Building_Name,Builing_Description,Building_Area,Building_Colour,Building_Type,College_Code,building_description,BuildType from Building_Master where Code in('" + buildcode + "') and College_Code='" + collegecode1 + "'";
                    }
                    if (rb_floor.Checked == true)
                    {
                        selectquery = "select Building_Name,Floor_Acronym,Floor_Name,Floor_Description  from  Floor_Master where Building_Name in('" + buildname + "')";
                    }

                    if (rb_room.Checked == true)
                    {
                        selectquery = "select Building_Name,Floor_Name,Room_Name,Room_type,room_size,students_allowed,StudPerSeat from Room_Detail where Building_Name in('" + buildname + "')";
                    }
                    if (rb_building.Checked != true && rb_floor.Checked != true && rb_room.Checked != true)
                    {
                        lbl_alert.Visible = true;
                        lbl_alert.Text = "Please Select the Building and then Proceed ";
                    }
                }

                if (selectquery.Trim() != "")
                {
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {


                        FpSpread.Sheets[0].RowCount = 0;
                        FpSpread.Sheets[0].ColumnCount = 0;
                        FpSpread.CommandBar.Visible = false;
                        FpSpread.Sheets[0].AutoPostBack = false;
                        FpSpread.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread.Sheets[0].RowHeader.Visible = false;
                        FpSpread.Sheets[0].ColumnCount = 20;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread.Columns[0].Locked = true;
                        FpSpread.Columns[0].Width = 50;


                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread.Columns[1].Width = 80;

                        FarPoint.Web.Spread.CheckBoxCellType chkbox = new FarPoint.Web.Spread.CheckBoxCellType();
                        chkbox.AutoPostBack = true;

                        if (rdb_detail.Checked == true)
                        {

                            if (rb_building.Checked == true)
                            {
                                FpSpread.Columns[9].Visible = false;
                                FpSpread.Columns[10].Visible = false;
                                FpSpread.Columns[11].Visible = false;
                                FpSpread.Columns[12].Visible = false;
                                FpSpread.Columns[13].Visible = false;
                                FpSpread.Columns[14].Visible = false;
                                FpSpread.Columns[15].Visible = false;
                                FpSpread.Columns[16].Visible = false;
                                FpSpread.Columns[17].Visible = false;
                                FpSpread.Columns[18].Visible = false;
                                FpSpread.Columns[19].Visible = false;
                            }

                            if (rb_floor.Checked == true)
                            {

                                FpSpread.Columns[4].Visible = false;


                                FpSpread.Columns[9].Visible = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Building Name";
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                                FpSpread.Columns[9].Width = 200;


                                FpSpread.Columns[10].Visible = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Floor Acronym";
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                                FpSpread.Columns[10].Width = 100;


                                FpSpread.Columns[11].Visible = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Floor Name";
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
                                FpSpread.Columns[11].Width = 100;

                                FpSpread.Columns[12].Visible = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Floor Description";
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 12].Font.Bold = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 12].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 12].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 12].Font.Size = FontUnit.Medium;
                                FpSpread.Columns[12].Width = 100;





                                FpSpread.Columns[2].Visible = false;
                                FpSpread.Columns[3].Visible = false;
                                FpSpread.Columns[5].Visible = false;
                                FpSpread.Columns[6].Visible = false;
                                FpSpread.Columns[7].Visible = false;
                                FpSpread.Columns[8].Visible = false;
                                FpSpread.Columns[13].Visible = false;
                                FpSpread.Columns[14].Visible = false;
                                FpSpread.Columns[15].Visible = false;
                                FpSpread.Columns[16].Visible = false;
                                FpSpread.Columns[17].Visible = false;
                                FpSpread.Columns[18].Visible = false;
                                FpSpread.Columns[19].Visible = false;
                                // FpSpread.Columns[11].Visible = false;
                                // FpSpread.Columns[12].Visible = false;
                            }
                            if (rb_building.Checked == true && rb_room.Checked == true)
                            {
                                FpSpread.Visible = false;
                                lbl_alert.Visible = true;
                                btn_update.Visible = false;
                                btn_Delete1.Visible = false;
                                lbl_alert.Text = "Please Select Floor also";
                            }
                            if (rb_room.Checked == true)
                            {
                                FpSpread.Columns[13].Visible = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Building Name";
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 13].Font.Bold = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 13].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 13].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 13].Font.Size = FontUnit.Medium;
                                FpSpread.Columns[13].Width = 100;

                                FpSpread.Columns[14].Visible = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 14].Text = "Floor Name";
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 14].Font.Bold = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 14].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 14].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 14].Font.Size = FontUnit.Medium;
                                FpSpread.Columns[14].Width = 100;



                                FpSpread.Columns[15].Visible = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 15].Text = "Room Name";
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 15].Font.Bold = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 15].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 15].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 15].Font.Size = FontUnit.Medium;
                                FpSpread.Columns[15].Width = 100;

                                FpSpread.Columns[16].Visible = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 16].Text = "Room Type";
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 16].Font.Bold = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 16].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 16].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 16].Font.Size = FontUnit.Medium;
                                FpSpread.Columns[16].Width = 100;

                                FpSpread.Columns[17].Visible = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 17].Text = "Room Size";
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 17].Font.Bold = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 17].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 17].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 17].Font.Size = FontUnit.Medium;
                                FpSpread.Columns[17].Width = 100;

                                FpSpread.Columns[18].Visible = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 18].Text = "Student Allowed";
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 18].Font.Bold = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 18].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 18].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 18].Font.Size = FontUnit.Medium;
                                FpSpread.Columns[18].Width = 100;

                                FpSpread.Columns[19].Visible = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 19].Text = "Student Per Seat";
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 19].Font.Bold = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 19].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 19].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].ColumnHeader.Cells[0, 19].Font.Size = FontUnit.Medium;
                                FpSpread.Columns[19].Width = 100;


                                FpSpread.Columns[2].Visible = false;
                                FpSpread.Columns[3].Visible = false;
                                FpSpread.Columns[4].Visible = false;
                                FpSpread.Columns[5].Visible = false;
                                FpSpread.Columns[6].Visible = false;
                                FpSpread.Columns[7].Visible = false;
                                FpSpread.Columns[8].Visible = false;
                                FpSpread.Columns[9].Visible = false;
                                FpSpread.Columns[10].Visible = false;
                                FpSpread.Columns[11].Visible = false;
                                FpSpread.Columns[12].Visible = false;



                            }
                        }
                        if (rb_building.Checked == true)
                        {

                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Code";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                            FpSpread.Columns[2].Width = 100;

                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Building Acronym";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                            FpSpread.Columns[3].Width = 200;

                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Build Type";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                            FpSpread.Columns[4].Width = 100;

                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Building Name";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                            FpSpread.Columns[5].Width = 200;


                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Building Area";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                            FpSpread.Columns[6].Width = 100;

                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Building Colour";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                            FpSpread.Columns[7].Width = 100;

                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Building Type";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                            FpSpread.Columns[8].Width = 100;
                        }

                        if (rdb_report.Checked == true)
                        {
                            FpSpread.Columns[1].Visible = false;
                            FpSpread.Columns[2].Visible = false;
                            FpSpread.Columns[3].Visible = false;
                            FpSpread.Columns[4].Visible = false;
                            FpSpread.Columns[5].Visible = false;

                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Building Name";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                            FpSpread.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            FpSpread.Columns[6].Width = 200;


                            FpSpread.Columns[7].Visible = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Floor Name";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                            FpSpread.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            FpSpread.Columns[7].Width = 100;

                            FpSpread.Columns[8].Visible = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Room Name";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                            FpSpread.Columns[8].Width = 120;


                            FpSpread.Columns[9].Visible = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Room type";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                            FpSpread.Columns[9].Width = 100;

                            FpSpread.Columns[10].Visible = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Room size";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                            FpSpread.Columns[10].Width = 120;

                            FpSpread.Columns[11].Visible = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Students allowed ";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
                            FpSpread.Columns[11].Width = 100;

                            FpSpread.Columns[12].Visible = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 12].Text = "StudPerSeat";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 12].Font.Bold = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 12].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 12].Font.Name = "Book Antiqua";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, 12].Font.Size = FontUnit.Medium;
                            FpSpread.Columns[12].Width = 100;
                        }

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {

                            FpSpread.Sheets[0].RowCount++;
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].CellType = chkbox;
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                            if (rdb_report.Checked == true)
                            {
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["Building_Name"]);
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].VerticalAlign = FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["Floor_Name"]);
                                //FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 10].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Floor_Acronym"]);
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].VerticalAlign = FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["Room_Name"]);
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[i]["Room_type"]);
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(ds.Tables[0].Rows[i]["room_size"]);
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";

                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(ds.Tables[0].Rows[i]["students_allowed"]);
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";

                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(ds.Tables[0].Rows[i]["StudPerSeat"]);
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 12].Font.Size = FontUnit.Medium;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 12].Font.Name = "Book Antiqua";

                            }
                            rptprint.Visible = true;
                            btn_Delete1.Visible = false;
                            btn_update.Visible = false;
                            if (rdb_detail.Checked == true)
                            {
                                //if (rb_building.Checked == true && rb_floor.Checked == true)
                                //{
                                //    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[i]["Building_Name"]);
                                //    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                                //    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                                //    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                                //    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(ds.Tables[0].Rows[i]["Floor_Name"]);
                                //    //FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 10].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Floor_Acronym"]);
                                //    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Left;
                                //    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                                //    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";

                                //    FpSpread.Width = 700;

                                //}
                                //else
                                if (rb_building.Checked == true)
                                {

                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Code"]);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Building_Acronym"]);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["StartingSerial"]);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["BuildType"]);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Building_Name"]);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["Building_Area"]);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["Building_Colour"]);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["Building_Type"]);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                                }

                                if (rb_floor.Checked == true)//delsi
                                {
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[i]["Building_Name"]);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(ds.Tables[0].Rows[i]["Floor_Acronym"]);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";

                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(ds.Tables[0].Rows[i]["Floor_Name"]);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";

                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(ds.Tables[0].Rows[i]["Floor_Description"]);
                                    //FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 12].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Floor_Acronym"]);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 12].Font.Size = FontUnit.Medium;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 12].Font.Name = "Book Antiqua";
                                    //FpSpread.Width = 750;
                                }
                                if (rb_room.Checked == true)
                                {
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 13].Text = Convert.ToString(ds.Tables[0].Rows[i]["Building_Name"]);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 13].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 13].Font.Size = FontUnit.Medium;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 13].Font.Name = "Book Antiqua";

                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 14].Text = Convert.ToString(ds.Tables[0].Rows[i]["Floor_Name"]);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 14].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 14].Font.Size = FontUnit.Medium;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 14].Font.Name = "Book Antiqua";

                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 15].Text = Convert.ToString(ds.Tables[0].Rows[i]["Room_Name"]);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 15].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 15].Font.Size = FontUnit.Medium;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 15].Font.Name = "Book Antiqua";

                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 16].Text = Convert.ToString(ds.Tables[0].Rows[i]["Room_type"]);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 16].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 16].Font.Size = FontUnit.Medium;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 16].Font.Name = "Book Antiqua";

                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 17].Text = Convert.ToString(ds.Tables[0].Rows[i]["room_size"]);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 17].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 17].Font.Size = FontUnit.Medium;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 17].Font.Name = "Book Antiqua";

                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 18].Text = Convert.ToString(ds.Tables[0].Rows[i]["students_allowed"]);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 18].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 18].Font.Size = FontUnit.Medium;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 18].Font.Name = "Book Antiqua";


                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 19].Text = Convert.ToString(ds.Tables[0].Rows[i]["StudPerSeat"]);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 19].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 19].Font.Size = FontUnit.Medium;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 19].Font.Name = "Book Antiqua";




                                }
                                rptprint.Visible = false;
                                btn_update.Visible = true;
                                btn_Delete1.Visible = true;
                            }

                        }

                    }
                    FpSpread.Sheets[0].PageSize = FpSpread.Sheets[0].RowCount;
                    FpSpread.SaveChanges();
                    FpSpread.Visible = true;
                    //btn_update.Visible = true;
                    //btn_Delete1.Visible = true;
                    lbl_alert.Visible = false;

                }

            }
            else
            {
                lbl_alert.Visible = true;
                lbl_alert.Text = " No records Found ";
            }
        }
        catch (Exception ex)
        {
            lbl_alert.Visible = true;
            lbl_alert.Text = ex.ToString();
            d2.sendErrorMail(ex, collegecode1, "Building_Master.aspx");
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            d2.printexcelreport(FpSpread, reportname);


        }
        catch
        {

        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Building Master";
            string pagename = "BiodeviceInformation.aspx";
            Printcontrol.loadspreaddetails(FpSpread, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }
    }
    //button cmd floor
    protected void btn_spread_click(object sender, EventArgs e)
    {
        try
        {
            bool check = false;
            string activerow = "";
            string activecol = "";
            int actrow = 0;
            int actcol = 0;
            int cNT = 0;
            string acrName = string.Empty;
            string startDgt = string.Empty;
            FpSpread1.SaveChanges();
            if (FpSpread1.Rows.Count > 0)
            {
                activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
                activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
                int.TryParse(Convert.ToString(activerow), out actrow);
                int.TryParse(Convert.ToString(activecol), out actcol);
                int colVal = 0;
                int.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[actrow, actcol].Tag), out colVal);
                if (colVal == -1)
                {
                    string bname = Convert.ToString(FpSpread1.Sheets[0].Cells[actrow, 5].Text);
                    string bacr = Convert.ToString(FpSpread1.Sheets[0].Cells[actrow, 3].Text);
                    bindBuilding();
                    ddlbname.SelectedIndex = ddlbname.Items.IndexOf(ddlbname.Items.FindByText(bname));
                    string query = "select Building_Name,Floor_Acronym,Floor_Name,StartingSerial from Floor_Master where building_name='" + bname + "' ";
                    ds = d2.select_method_wo_parameter(query, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        #region
                        FpSpread2.Sheets[0].RowCount = 0;
                        FpSpread2.Sheets[0].ColumnCount = 0;
                        FpSpread2.CommandBar.Visible = false;
                        FpSpread2.Sheets[0].AutoPostBack = false;
                        FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread2.Sheets[0].RowHeader.Visible = false;
                        FpSpread2.Sheets[0].ColumnCount = 4;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread2.Columns[0].Locked = true;
                        FpSpread2.Columns[0].Width = 50;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Columns[1].Width = 80;

                        FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                        chkall.AutoPostBack = false;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Floor Acronym";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread2.Columns[2].Width = 100;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Floor Name";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread2.Columns[3].Width = 175;

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            cNT++;
                            FpSpread2.Sheets[0].RowCount++;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = chkall;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            acrName = Convert.ToString(ds.Tables[0].Rows[i]["Floor_Acronym"]);
                            startDgt = Convert.ToString(ds.Tables[0].Rows[i]["StartingSerial"]);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = acrName;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Floor_Name"]);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                        }
                        FpSpread2.Visible = true;
                        btn_flrsave.Visible = true;
                        btn_flrdelete.Visible = true;
                        //
                        div_floor.Visible = true;
                        ImageButton2.Visible = true;
                        Div2.Visible = true;
                        lbl_bname.Visible = true;
                        //txt_bname.Visible = true;
                        ddlbname.Visible = true;
                        lbl_totf.Visible = true;
                        txt_totf.Visible = true;
                        lbl_facr.Visible = true;
                        txt_facr.Visible = true;
                        lbl_ssw.Visible = true;
                        txt_ssw.Visible = true;
                        btn_fgo.Visible = true;
                        Label5.Visible = false;

                        txt_totf.Enabled = false;
                        txt_facr.Enabled = false;
                        txt_ssw.Enabled = false;
                        ddlbname.Enabled = false;
                        txt_totf.Text = Convert.ToString(cNT);
                        txt_facr.Text = Convert.ToString(acrName);
                        txt_ssw.Text = Convert.ToString(startDgt);
                        btn_fgo.Visible = false;
                        check = true;
                        #endregion
                    }
                }
                else if (colVal == -2)
                {
                    // btn_roomspread_click(sender, e);
                    check = loadroomvalues();
                }
                if (!check)
                {
                    imgdiv4.Visible = true;
                    lbl_alertr.Visible = true;
                    btn_roomok.Visible = true;
                    lbl_alertr.Text = "No Record Found";
                }
            }
        }
        catch (Exception ex)
        {
            Label5.Visible = true;
            Label5.Text = ex.ToString();
            d2.sendErrorMail(ex, collegecode1, "Building_Master.aspx");
        }
    }
    //button cmd rooms  

    protected void btn_roomspread_click(object sender, EventArgs e)
    {


    }
    protected bool loadroomvalues()
    {
        bool check = false;
        try
        {
            int colVal = 0;
            int actrow = 0;
            int actcol = 0;
            string activerow2 = "";
            string activecol2 = "";
            activerow2 = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            activecol2 = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
            int.TryParse(Convert.ToString(activerow2), out actrow);
            int.TryParse(Convert.ToString(activecol2), out actcol);
            string bname = Convert.ToString(FpSpread1.Sheets[0].Cells[actrow, 5].Text);
            string bacr = Convert.ToString(FpSpread1.Sheets[0].Cells[actrow, 3].Text);
            bindBuilding();
            ddlrbname.SelectedIndex = ddlrbname.Items.IndexOf(ddlrbname.Items.FindByText(bname));
            bindFloor();
            string query = "select Room_Acronym,Room_Name,Room_type,no_of_rows,no_of_columns,room_size,students_allowed,StudPerSeat,StartingSerial from Room_Detail where building_name='" + bname + "' ";
            //and floor_name='" + fname + "'
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                #region design
                FpSpread3.Visible = true;
                btn_rsave.Visible = true;
                btn_rdelete.Visible = true;

                FpSpread3.Sheets[0].RowCount = 0;
                FpSpread3.Sheets[0].ColumnCount = 0;
                FpSpread3.CommandBar.Visible = false;
                FpSpread3.Sheets[0].AutoPostBack = false;
                FpSpread3.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread3.Sheets[0].RowHeader.Visible = false;
                FpSpread3.Sheets[0].ColumnCount = 10;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread3.Columns[0].Locked = true;
                FpSpread3.Columns[0].Width = 50;

                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread3.Columns[1].Width = 80;

                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = true;

                //string[] items = { "a", "b" };
                //FarPoint.Web.Spread.ComboBoxCellType combo = new FarPoint.Web.Spread.ComboBoxCellType(items);
                //combo.AutoPostBack = true;
                //FarPoint.Web.Spread.ButtonCellType button = new FarPoint.Web.Spread.ButtonCellType();
                //button.Text = "Edit";

                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Room Acronym";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpread3.Columns[2].Width = 100;

                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Room Name";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpSpread3.Columns[3].Width = 175;

                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Room Type";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                FpSpread3.Columns[4].Width = 100;

                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Text = "No Of Rows";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                FpSpread3.Columns[5].Width = 150;


                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Text = "No Of Columns";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                FpSpread3.Columns[6].Width = 100;

                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Room Size";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                FpSpread3.Columns[7].Width = 75;

                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Max Allowed Students";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                FpSpread3.Columns[8].Width = 100;

                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Students Per Seat";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                FpSpread3.Columns[9].Width = 100;
                #endregion
                #region value
                string acrname = string.Empty;
                int totCnt = 0;
                string searial = string.Empty;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    totCnt++;
                    FpSpread3.Sheets[0].RowCount++;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].CellType = chkall;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                    //string acr1 = txt_racr.Text;

                    //string roomacr = "";
                    //roomacr = acr1 + val1;
                    //val1++;
                    acrname = Convert.ToString(ds.Tables[0].Rows[i]["Room_Acronym"]);
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Room_Acronym"]);
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Room_Name"]);
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Room_type"]);
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["no_of_rows"]);
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["no_of_columns"]);
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["room_size"]);
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["students_allowed"]);
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[i]["StudPerSeat"]);
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                    searial = Convert.ToString(ds.Tables[0].Rows[i]["StartingSerial"]);
                }
                txt_racr.Text = Convert.ToString(acrname);
                txt_rtot.Text = Convert.ToString(totCnt);
                txt_ss.Text = Convert.ToString(searial);
                FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
                FpSpread3.SaveChanges();
                FpSpread3.Visible = true;
                div_room.Visible = true;
                ImageButton3.Visible = true;
                Div3.Visible = true;
                lbl_rbn.Visible = true;
                //  txt_rbn.Visible = true;
                ddlrbname.Visible = true;
                ddlrbname.Enabled = true;
                lbl_rflrn.Visible = true;
                // txt_rflrn.Visible = true;
                ddlrflrm.Visible = true;
                ddlrflrm.Enabled = true;
                lbl_rtot.Visible = true;
                txt_rtot.Visible = true;
                lbl_racr.Visible = true;
                txt_racr.Visible = true;
                lbl_ss.Visible = true;
                txt_ss.Visible = true;

                //
                ddlrbname.Enabled = false;
                ddlrflrm.Enabled = false;
                txt_rtot.Enabled = false;
                txt_ss.Enabled = false;
                txt_racr.Enabled = false;

                btn_roomgo.Visible = false;
                check = true;
                #endregion
            }
        }
        catch (Exception ex)
        {
            lbl_ralert.Visible = true;
            lbl_ralert.Text = ex.ToString();
            d2.sendErrorMail(ex, collegecode1, "Building_Master.aspx");
        }
        return check;
    }

    protected void btn_save_click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            if (FpSpread1.Rows.Count > 0)
            {
                bool check = false;
                for (int row = 0; row < FpSpread1.Sheets[0].RowCount; row++)
                {
                    string startvalue1 = Convert.ToString(txt_serial.Text);
                    string bacr = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 3].Text);
                    string building_type = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 4].Text);
                    string buildname = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 5].Text);
                    string buildarea = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 6].Text);
                    string buildcolor = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 7].Text);

                    // string sserial = txt_serial.Text;
                    string buildtype = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 8].Text);
                  //  string type = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 7].Text);
                    if (!string.IsNullOrEmpty(bacr) && !string.IsNullOrEmpty(buildname))
                    {
                        string savequery = "if exists(select * from Building_Master where Building_Acronym='" + bacr + "' and Building_Name='" + buildname + "' and Building_Area='" + buildarea + "' and Building_Colour='" + buildcolor + "' and StartingSerial='" + startvalue1 + "' and College_Code='" + collegecode1 + "' )update Building_Master set Building_Acronym='" + bacr + "',Building_Name='" + buildname + "',Builing_Description='', Building_Area='" + buildarea + "',Building_Colour='" + buildcolor + "',Building_Type='" + building_type + "',College_Code='" + collegecode1 + "',building_description='',BuildType='" + buildtype + "',StartingSerial='" + startvalue1 + "' where Building_Acronym='" + bacr + "' and Building_Name='" + buildname + "' and Building_Area='" + buildarea + "' and Building_Colour='" + buildcolor + "' else insert into Building_Master(Building_Acronym,Building_Name,Builing_Description,Building_Area,Building_Colour,Building_Type,College_Code,building_description,BuildType,StartingSerial) values('" + bacr + "','" + buildname + "','','" + buildarea + "','" + buildcolor + "','" + building_type + "','" + collegecode1 + "','','" + buildtype + "','" + startvalue1 + "')";
                        int ins = d2.update_method_wo_parameter(savequery, "Text");
                        check = true;
                    }
                    //btn_go_click(sender, e);
                }
                if (check)
                {
                    imgdiv2.Visible = true;
                    lbl_alerterr.Visible = true;
                    btn_errorclose.Visible = true;
                    lbl_alerterr.Text = "Saved Successfully";
                    btn_go_click(sender, e);
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Building_Master.aspx");
        }
    }

    //floor save button
    protected void btn_flrsave_click(object sender, EventArgs e)
    {
        try
        {
            FpSpread2.SaveChanges();
            if (FpSpread2.Rows.Count > 0)
            {
                bool check = false;
                string alert = string.Empty;
                string buildname = Convert.ToString(ddlbname.SelectedItem.Text);
                string sserial = Convert.ToString(txt_ssw.Text);
                for (int row = 0; row < FpSpread2.Sheets[0].RowCount; row++)
                {
                    string facr = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 2].Text);
                    string fname = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 3].Text);
                    if (!string.IsNullOrEmpty(buildname) && !string.IsNullOrEmpty(facr) && !string.IsNullOrEmpty(fname) && !string.IsNullOrEmpty(sserial))
                    {
                        // string savequery = "if exists(select * from floor_master where Floor_Acronym='" + facr + "' and Building_Name='" + buildname + "' and College_Code='" + collegecode1 + "' and Floor_Name='" + fname + "' )update floor_master set Building_Name='" + buildname + "',Floor_Acronym='" + facr + "',StartingSerial='" + sserial + "',Floor_Name='" + fname + "',College_Code='" + collegecode1 + "' else insert into floor_master(Building_Name,Floor_Acronym,StartingSerial,Floor_Name,Floor_Description,College_Code) values('" + buildname + "','" + facr + "','" + sserial + "','" + fname + "','','" + collegecode1 + "')";
                        string savequery = "if not exists(select * from floor_master where  Building_Name='" + buildname + "' and College_Code='" + collegecode1 + "' and Floor_Name='" + fname + "' ) insert into floor_master(Building_Name,Floor_Acronym,StartingSerial,Floor_Name,Floor_Description,College_Code) values('" + buildname + "','" + facr + "','" + sserial + "','" + fname + "','','" + collegecode1 + "')";
                        int ins = d2.update_method_wo_parameter(savequery, "Text");
                        if (ins != 1)
                        {
                            if (alert == string.Empty)
                                alert = fname;
                            else
                                alert += "," + fname;
                        }
                        check = true;
                    }
                }
                if (check)
                {
                    imgdiv3.Visible = true;
                    lbl_alertf.Visible = true;
                    btn_flrok.Visible = true;
                    if (alert != "")
                        lbl_alertf.Text = "Saved Successfully<br>" + "-" + "Floor Name Exists " + alert;
                    else
                        lbl_alertf.Text = "Saved Successfully";
                }
            }
        }
        catch (Exception ex)
        {
            Label5.Visible = true;
            Label5.Text = ex.ToString();
            d2.sendErrorMail(ex, collegecode1, "Building_Master.aspx");
        }
    }
    //floor delete button
    protected void btn_flrdelete_Click(object sender, EventArgs e)
    {
        floorDeleteMethod();
        //btn_fgo_click(sender, e);
        btn_roomspread_click(sender, e);
    }
    protected void floorDeleteMethod()
    {
        try
        {
            FpSpread2.SaveChanges();
            if (FpSpread2.Rows.Count > 0)
            {
                int colVal = 0;
                bool check = false;
                string buildName = Convert.ToString(ddlbname.SelectedItem.Text);
                for (int row = 0; row < FpSpread2.Sheets[0].Rows.Count; row++)
                {
                    int.TryParse(Convert.ToString(FpSpread2.Sheets[0].Cells[row, 1].Value), out colVal);
                    if (colVal == 1)
                    {
                        string flrAcrnym = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 2].Text);
                        string floorName = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 3].Text);
                        if (!string.IsNullOrEmpty(buildName) && !string.IsNullOrEmpty(flrAcrnym) && !string.IsNullOrEmpty(floorName))
                        {
                            string delQ = "delete from Floor_Master where building_name='" + buildName + "' and floor_acronym='" + flrAcrnym + "' and floor_name='" + floorName + "' and college_code='" + collegecode1 + "'";
                            d2.update_method_wo_parameter(delQ, "Text");
                            check = true;
                        }

                    }
                }
                if (check)
                {
                    Div2.Visible = false;
                    alertdiv.Visible = true;
                    lblalert.Text = "Deleted Successfully";

                }
            }
        }
        catch
        {

        }
    }



    protected void rdb_detail_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rdb_detail.Checked == true)
        {
            rb_building.Visible = true;
            rb_floor.Visible = true;
            rb_room.Visible = true;
            btn_new.Visible = true;
            UpdatePanel1.Visible = true;
            UpdatePanel2.Visible = false;
            UpdatePanel3.Visible = false;
            ddlbuild.Visible = false;

            lbl_flr.Visible = false;
            FpSpread.Visible = false;
            rptprint.Visible = false;
            btn_Delete1.Visible = false;
            btn_update.Visible = false;
            lbl_rm.Visible = false;

        }
    }

    protected void rdb_report_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rdb_report.Checked == true)
        {
            rb_building.Visible = false;
            rb_floor.Visible = false;
            rb_room.Visible = false;
            btn_new.Visible = false;
            UpdatePanel1.Visible = false;
            UpdatePanel2.Visible = true;
            UpdatePanel3.Visible = true;
            ddlbuild.Visible = true;
            FpSpread.Visible = false;
            rptprint.Visible = false;
            btn_Delete1.Visible = false;
            btn_update.Visible = false;

            lbl_flr.Visible = true;
            lbl_rm.Visible = true;

        }
    }

    //room save button
    protected void btn_rsave_click(object sender, EventArgs e)
    {
        try
        {
            FpSpread3.SaveChanges();
            if (FpSpread3.Rows.Count > 0)
            {
                string alert = string.Empty;
                bool check = false;
                string buildname = Convert.ToString(ddlrbname.SelectedItem.Text);
                string fname = Convert.ToString(ddlrflrm.SelectedItem.Text);
                string sserial = Convert.ToString(txt_ss.Text);
                for (int row = 0; row < FpSpread3.Sheets[0].RowCount; row++)
                {
                    string racr = Convert.ToString(FpSpread3.Sheets[0].Cells[row, 2].Text);
                    string rname = Convert.ToString(FpSpread3.Sheets[0].Cells[row, 3].Text);
                    string rtype = Convert.ToString(FpSpread3.Sheets[0].Cells[row, 4].Text);
                    string noofrows = Convert.ToString(FpSpread3.Sheets[0].Cells[row, 5].Text);
                    string noofcol = Convert.ToString(FpSpread3.Sheets[0].Cells[row, 6].Text);
                    string rsize = Convert.ToString(FpSpread3.Sheets[0].Cells[row, 7].Text);
                    string maxallstds = Convert.ToString(FpSpread3.Sheets[0].Cells[row, 8].Text);
                    string stdperseat = Convert.ToString(FpSpread3.Sheets[0].Cells[row, 10].Text);
                    string maxStudStgh = Convert.ToString(FpSpread3.Sheets[0].Cells[row, 11].Text);
                    if (noofrows == string.Empty)
                        noofrows = "0";
                    if (noofcol == string.Empty)
                        noofcol = "0";
                    if (maxallstds == string.Empty)
                        maxallstds = "0";
                    if (stdperseat == string.Empty)
                        stdperseat = "0";
                    if (maxStudStgh == string.Empty)
                        maxStudStgh = "0";
                    if (!string.IsNullOrEmpty(buildname) && !string.IsNullOrEmpty(fname) && !string.IsNullOrEmpty(sserial) && !string.IsNullOrEmpty(rname))
                    {
                        //string savequery = "if exists(select * from Room_Detail where Building_Name='" + buildname + "' and Floor_Name='" + fname + "' and  College_Code='" + collegecode1 + "' and Room_Acronym='" + racr + "' )update Room_Detail set Building_Name='" + buildname + "',Floor_Name='" + fname + "',Room_Acronym='" + racr + "',StartingSerial='" + sserial + "',Room_Name='" + rname + "',College_Code='" + collegecode1 + "',Room_type='" + rtype + "',no_of_rows='" + noofrows + "',no_of_columns='" + noofcol + "',room_size='" + rsize + "',students_allowed='" + maxallstds + "',StudPerSeat='" + stdperseat + "', MaxStudClassStrength='" + maxStudStgh + "' where Building_Name='" + buildname + "' and Floor_Name='" + fname + "' and  College_Code='" + collegecode1 + "' and Room_Acronym='" + racr + "' else insert into Room_Detail(Building_Name,Floor_Name,Room_Acronym,StartingSerial,Room_Name,Room_Description,College_Code,Room_type,no_of_rows,no_of_columns,room_size,students_allowed,StudPerSeat,MaxStudClassStrength) values('" + buildname + "','" + fname + "','" + racr + "','" + sserial + "','" + rname + "','','" + collegecode1 + "','" + rtype + "','" + noofrows + "','" + noofcol + "','" + rsize + "','" + maxallstds + "','" + stdperseat + "','" + maxStudStgh + "')";
                        string savequery = "if not exists(select * from Room_Detail where Building_Name='" + buildname + "' and Floor_Name='" + fname + "' and  College_Code='" + collegecode1 + "' and Room_Name='" + rname + "' ) insert into Room_Detail(Building_Name,Floor_Name,Room_Acronym,StartingSerial,Room_Name,Room_Description,College_Code,Room_type,no_of_rows,no_of_columns,room_size,students_allowed,StudPerSeat,MaxStudClassStrength) values('" + buildname + "','" + fname + "','" + racr + "','" + sserial + "','" + rname + "','','" + collegecode1 + "','" + rtype + "','" + noofrows + "','" + noofcol + "','" + rsize + "','" + maxallstds + "','" + stdperseat + "','" + maxStudStgh + "')";
                        int ins = d2.update_method_wo_parameter(savequery, "Text");
                        if (ins != 1)
                        {
                            if (alert == string.Empty)
                                alert = fname;
                            else
                                alert += "," + fname;
                        }
                        check = true;
                    }
                }
                if (check)
                {
                    imgdiv4.Visible = true;
                    lbl_alertr.Visible = true;
                    btn_roomok.Visible = true;
                    if (alert != "")
                        lbl_alertr.Text = "Saved Successfully<br>" + "-" + "Floor Name Exists " + alert;
                    else
                        lbl_alertr.Text = "Saved Successfully";
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Building_Master.aspx");
        }
    }

    protected void btn_rdelete_Click(object sender, EventArgs e)
    {
        roomDeleteMethod();
        // btn_fgo_click(sender, e);
    }
    protected void roomDeleteMethod()
    {
        try
        {
            FpSpread3.SaveChanges();
            if (FpSpread3.Rows.Count > 0)
            {
                int colVal = 0;
                bool check = false;
                string buildName = Convert.ToString(ddlrbname.SelectedItem.Text);
                string floorName = Convert.ToString(ddlrflrm.SelectedItem.Text);
                for (int row = 0; row < FpSpread3.Sheets[0].Rows.Count; row++)
                {
                    int.TryParse(Convert.ToString(FpSpread3.Sheets[0].Cells[row, 1].Value), out colVal);
                    if (colVal == 1)
                    {
                        // string flrAcrnym = Convert.ToString(FpSpread3.Sheets[0].Cells[row, 2].Text);
                        // string floorName = Convert.ToString(FpSpread3.Sheets[0].Cells[row, 3].Text);
                        string roomacr = Convert.ToString(FpSpread3.Sheets[0].Cells[row, 2].Text);
                        string roomname = Convert.ToString(FpSpread3.Sheets[0].Cells[row, 3].Text);
                        if (!string.IsNullOrEmpty(buildName) && !string.IsNullOrEmpty(floorName) && !string.IsNullOrEmpty(roomacr) && !string.IsNullOrEmpty(roomname))
                        {
                            //string delQ = "delete from Floor_Master where building_name='" + buildName + "' and floor_acronym='" + flrAcrnym + "' and floor_name='" + floorName + "'";
                            string delQ = " delete from room_detail where building_name='" + buildName + "' and floor_name='" + floorName + "'and room_acronym='" + roomacr + "' and room_name='" + roomname + "' and college_code='" + collegecode1 + "'";
                            d2.update_method_wo_parameter(delQ, "Text");
                            check = true;
                        }

                    }
                }
                if (check)
                {
                    div_room.Visible = false;
                    alertdiv.Visible = true;
                    lblalert.Text = "Deleted Successfully";
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Building_Master.aspx");
        }
    }

    //update click
    protected void btn_update_click(object sender, EventArgs e)
    {
        try
        {

            txt_nofbuild.Visible = false;
            lbl_nofbuild.Visible = false;
            lbl_buildacr.Visible = false;
            txt_buildacr.Visible = false;
            lbl_serial.Visible = false;
            txt_serial.Visible = false;
            btn_popgo.Visible = false;
            if (rdb_detail.Checked == true)
            {
                if (rb_floor.Checked == true || rb_room.Checked == true || rb_floor.Checked == true && rb_room.Checked == true)
                {
                    FpSpread.Visible = false;
                    lbl_alert.Visible = true;
                    btn_update.Visible = false;
                    btn_Delete1.Visible = false;
                    lbl_alert.Text = "Please Select Building First";
                }

                if (rb_building.Checked == true)
                {
                    {
                        popper1.Visible = true;
                        pop1.Visible = true;
                        //lbl_nofbuild.Visible = true;
                        //txt_nofbuild.Visible = true;
                        //lbl_buildacr.Visible = true;
                        //txt_buildacr.Visible = true;
                        //lbl_serial.Visible = true;
                        //txt_serial.Visible = true;
                        //btn_popgo.Visible = true;
                        FpSpread1.Visible = true;
                        btn_save.Visible = true;
                        //btn_delete.Visible = true;

                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = false;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 11;
                        FarPoint.Web.Spread.StyleInfo darkstyle1 = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle1.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle1.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle1;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[0].Locked = true;
                        FpSpread1.Columns[0].Width = 50;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[1].Width = 80;

                        FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                        chkall.AutoPostBack = false;

                        string[] items = { "RCC", "ACC" };
                        FarPoint.Web.Spread.ComboBoxCellType combo = new FarPoint.Web.Spread.ComboBoxCellType(items);
                        combo.AutoPostBack = true;
                        FarPoint.Web.Spread.ButtonCellType button = new FarPoint.Web.Spread.ButtonCellType();
                        button.Text = "Add/Edit";
                        FarPoint.Web.Spread.ButtonCellType btnroom = new FarPoint.Web.Spread.ButtonCellType();
                        btnroom.Text = "Add/Edit";

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[2].Width = 100;
                        FpSpread1.Columns[2].Visible = false;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Building Acronym";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[3].Width = 175;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Building Type";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[4].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Building Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[5].Width = 150;


                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Building Area";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[6].Width = 100;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Building Colour";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[7].Width = 75;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Building Type";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[8].Width = 75;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Floor";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[9].Width = 75;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Room";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[10].Width = 75;

                        for (int i = 0; i < FpSpread.Sheets[0].RowCount; i++)
                        {

                            int checkval = Convert.ToInt32(FpSpread.Sheets[0].Cells[i, 1].Value);
                            if (checkval == 1)
                            {
                                string code = Convert.ToString(FpSpread.Sheets[0].Cells[Convert.ToInt32(i), 2].Text);
                                string Buildacr = Convert.ToString(FpSpread.Sheets[0].Cells[Convert.ToInt32(i), 3].Text);



                                string query = "select Code,Building_Acronym,StartingSerial,Building_Name,Building_Area,Building_Colour,BuildType,Building_Type From Building_Master where Building_Acronym ='" + Buildacr + "' and Code ='" + code + "'";
                                if (query.Trim() != "")
                                {
                                    ds.Clear();
                                    ds = d2.select_method_wo_parameter(query, "Text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {

                                        FpSpread1.Sheets[0].RowCount++;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(0 + 1);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chkall;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[0]["Code"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[0]["Building_Acronym"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[0]["StartingSerial"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = combo;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[0]["Building_Name"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[0]["Building_Area"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[0]["Building_Colour"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[0]["Building_Type"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].CellType = button;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Tag = "-1";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].CellType = btnroom;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Tag = "-2";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";

                                    }
                                }
                            }

                        }
                    }
                    if (rb_building.Checked == true && rb_floor.Checked == true)
                    {
                        div_floor.Visible = true;
                        Div2.Visible = true;
                        lbl_bname.Visible = true;
                        txt_bname.Visible = true;
                        txt_bname.Enabled = true;
                        txt_bname.ReadOnly = false;
                        lbl_totf.Visible = true;
                        txt_totf.Visible = true;
                        lbl_facr.Visible = true;
                        txt_facr.Visible = true;
                        lbl_ssw.Visible = true;
                        txt_ssw.Visible = true;
                        btn_fgo.Visible = true;
                        FpSpread2.Visible = true;
                        popper1.Visible = false;

                        FpSpread1.Visible = false;
                        FpSpread3.Visible = false;
                        btn_flrsave.Visible = true;

                        FpSpread2.Sheets[0].RowCount = 0;
                        FpSpread2.Sheets[0].ColumnCount = 0;
                        FpSpread2.CommandBar.Visible = false;
                        FpSpread2.Sheets[0].AutoPostBack = false;
                        FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread2.Sheets[0].RowHeader.Visible = false;
                        FpSpread2.Sheets[0].ColumnCount = 11;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread2.Columns[0].Locked = true;
                        FpSpread2.Columns[0].Width = 50;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Columns[1].Width = 80;

                        FarPoint.Web.Spread.CheckBoxCellType chkbox = new FarPoint.Web.Spread.CheckBoxCellType();
                        chkbox.AutoPostBack = true;
                        FarPoint.Web.Spread.ButtonCellType button = new FarPoint.Web.Spread.ButtonCellType();
                        button.Text = "Add/Edit";

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Building Name";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread2.Columns[2].Width = 200;


                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Floor Name";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread2.Columns[3].Width = 100;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Room";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread2.Columns[4].Width = 100;

                        FpSpread2.Columns[5].Visible = false;
                        FpSpread2.Columns[6].Visible = false;
                        FpSpread2.Columns[7].Visible = false;
                        FpSpread2.Columns[8].Visible = false;
                        FpSpread2.Columns[9].Visible = false;
                        FpSpread2.Columns[10].Visible = false;

                        for (int i = 0; i < FpSpread.Sheets[0].RowCount; i++)
                        {

                            int checkval = Convert.ToInt32(FpSpread.Sheets[0].Cells[i, 1].Value);
                            if (checkval == 1)
                            {
                                string code = Convert.ToString(FpSpread.Sheets[0].Cells[Convert.ToInt32(i), 9].Text);
                                string Buildacr = Convert.ToString(FpSpread.Sheets[0].Cells[Convert.ToInt32(i), 10].Text);

                                string query = "select Building_Name,Floor_Acronym,StartingSerial,Floor_Name,Floor_Description  from  Floor_Master where Building_Name ='" + code + "' and Floor_Name ='" + Buildacr + "'";
                                if (query.Trim() != "")
                                {
                                    ds.Clear();
                                    ds = d2.select_method_wo_parameter(query, "Text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        FpSpread2.Sheets[0].RowCount++;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(0 + 1);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = chkbox;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[0]["Building_Name"]);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[0]["Floor_Name"]);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[0]["Floor_Acronym"]);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].CellType = button;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                    }
                                }
                            }
                        }
                    }

                    if (rb_building.Checked == true && rb_floor.Checked == true && rb_room.Checked == true)
                    {
                        div_room.Visible = true;
                        Div3.Visible = true;
                        lbl_rbn.Visible = true;
                        txt_rbn.Visible = true;
                        txt_rbn.ReadOnly = false;
                        txt_rbn.Enabled = true;
                        lbl_rflrn.Visible = true;
                        txt_rflrn.Visible = true;
                        txt_rflrn.ReadOnly = false;
                        txt_rflrn.Enabled = true;
                        lbl_rtot.Visible = true;
                        txt_rtot.Visible = true;
                        lbl_racr.Visible = true;
                        txt_racr.Visible = true;
                        lbl_ss.Visible = true;
                        txt_ss.Visible = true;
                        btn_roomgo.Visible = true;
                        popper1.Visible = true;
                        div_floor.Visible = true;
                        FpSpread3.Visible = true;
                        FpSpread1.Visible = false;
                        FpSpread2.Visible = false;
                        btn_rsave.Visible = true;

                        FpSpread3.Sheets[0].RowCount = 0;
                        FpSpread3.Sheets[0].ColumnCount = 0;
                        FpSpread3.CommandBar.Visible = false;
                        FpSpread3.Sheets[0].AutoPostBack = false;
                        FpSpread3.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread3.Sheets[0].RowHeader.Visible = false;
                        FpSpread3.Sheets[0].ColumnCount = 12;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread3.Columns[0].Locked = true;
                        FpSpread3.Columns[0].Width = 50;

                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread3.Columns[1].Width = 80;

                        FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                        chkall.AutoPostBack = true;

                        string[] items = { "a", "b" };
                        FarPoint.Web.Spread.ComboBoxCellType combo = new FarPoint.Web.Spread.ComboBoxCellType(items);
                        combo.AutoPostBack = true;
                        FarPoint.Web.Spread.ButtonCellType button = new FarPoint.Web.Spread.ButtonCellType();
                        button.Text = "Edit";

                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Room Acronym";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread3.Columns[2].Width = 100;

                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Building Name";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread3.Columns[3].Width = 80;

                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Floor Name";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread3.Columns[4].Width = 200;


                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Room Name";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpread3.Columns[5].Width = 100;

                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Room type";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        FpSpread3.Columns[6].Width = 100;

                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Text = "No Of Rows";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                        FpSpread3.Columns[7].Width = 100;

                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Text = "No Of Columns";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                        FpSpread3.Columns[8].Width = 100;

                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Room Size";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                        FpSpread3.Columns[9].Width = 100;

                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Max Allowed studentss";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                        FpSpread3.Columns[8].Width = 100;

                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Students Per Seat";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
                        FpSpread3.Columns[9].Width = 100;

                        for (int i = 0; i < FpSpread.Sheets[0].RowCount; i++)
                        {

                            int checkval = Convert.ToInt32(FpSpread.Sheets[0].Cells[i, 1].Value);
                            if (checkval == 1)
                            {
                                string bname = Convert.ToString(FpSpread.Sheets[0].Cells[Convert.ToInt32(i), 9].Text);
                                string rname = Convert.ToString(FpSpread.Sheets[0].Cells[Convert.ToInt32(i), 11].Text);

                                string query = "select Room_Acronym,Building_Name,Floor_Name,Room_Name,Room_type,no_of_rows,no_of_columns,room_size,students_allowed,StudPerSeat from Room_Detail where Building_Name ='" + bname + "' and Room_Name ='" + rname + "'";
                                if (query.Trim() != "")
                                {
                                    ds.Clear();
                                    ds = d2.select_method_wo_parameter(query, "Text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        FpSpread3.Sheets[0].RowCount++;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(0 + 1);
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].CellType = chkall;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[0]["Room_Acronym"]);
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[0]["Building_Name"]);
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[0]["Floor_Name"]);
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[0]["Room_Name"]);
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[0]["Room_type"]);
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[0]["no_of_rows"]);
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[0]["no_of_columns"]);
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[0]["room_size"]);
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(ds.Tables[0].Rows[0]["students_allowed"]);
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";

                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(ds.Tables[0].Rows[0]["StudPerSeat"]);
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";


                                    }
                                }
                            }
                        }
                    }
                    lbl_alert.Visible = false;
                }


            }

        }
        catch (Exception ex)
        {
            lbl_alert.Visible = true;
            lbl_alert.Text = ex.ToString();
            d2.sendErrorMail(ex, collegecode1, "Building_Master.aspx");
        }
    }

    protected void btn_Delete1_Click(object sender, EventArgs e)
    {
        try
        {
            imgdiv2.Visible = true;
            lbl_alerterr.Visible = true;
            btn_errorclose.Visible = false;
            btn_yes.Visible = true;
            btn_no.Visible = true;
            lbl_alerterr.Text = "Are you Sure to delete this record";
        }
        catch (Exception ex)
        {
            lbl_alert.Visible = true;
            lbl_alert.Text = ex.ToString();
            d2.sendErrorMail(ex, collegecode1, "Building_Master.aspx");
        }

    }

    protected void btn_yes_Click(object sender, EventArgs e)
    {
        try
        {
            string activerow = "";
            string activecol = "";
            activerow = FpSpread.ActiveSheetView.ActiveRow.ToString();
            activecol = FpSpread.ActiveSheetView.ActiveColumn.ToString();
            if (activerow.Trim() != "")
            {
                string code = Convert.ToString(FpSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                string Buildacr = Convert.ToString(FpSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                string bname = Convert.ToString(FpSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Text);
                string fname = Convert.ToString(FpSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 10].Text);
                string rname = Convert.ToString(FpSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 11].Text);
                string buildname = Convert.ToString(FpSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 13].Text);
                string floorname = Convert.ToString(FpSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 14].Text);
                string roomname = Convert.ToString(FpSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 15].Text);
                if (rdb_report.Checked == true)
                {
                    string query = "delete from Building_Master where Building_Acronym in('" + Buildacr + "') and Code in('" + code + "')";
                    int ins = d2.update_method_wo_parameter(query, "Text");
                }
                if (rdb_detail.Checked == true)
                {
                    if (rb_building.Checked == true)
                    {
                        string query = "delete from Building_Master where Building_Acronym in('" + Buildacr + "') and Code in('" + code + "')";
                        int ins = d2.update_method_wo_parameter(query, "Text");
                    }
                    if (rb_floor.Checked == true)
                    {
                        string query = "delete from  Floor_Master where Building_Name ='" + bname + "' and Floor_Name ='" + rname + "'";
                        int ins = d2.update_method_wo_parameter(query, "Text");
                    }
                    if (rb_room.Checked == true)
                    {

                        string query = "delete from Room_Detail where Building_Name ='" + buildname + "' and Floor_Name='" + floorname + "' and Room_Name ='" + roomname + "'";
                        int ins = d2.update_method_wo_parameter(query, "Text");
                    }
                }
                imgdiv2.Visible = true;
                lbl_alerterr.Visible = true;
                btn_no.Visible = false;
                btn_yes.Visible = false;
                btn_errorclose.Visible = true;
                lbl_alerterr.Text = "Deleted Successfully";
                btn_go_click(sender, e);
            }
        }
        catch (Exception ex)
        {
            lbl_alert.Visible = true;
            lbl_alert.Text = ex.ToString();
            d2.sendErrorMail(ex, collegecode1, "Building_Master.aspx");
        }

    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        try
        {
            imgdiv2.Visible = false;
            popper1.Visible = false;
            pop1.Visible = false;
            ImageButton1.Visible = false;
            btn_go_click(sender, e);
        }
        catch (Exception ex)
        {
            lbl_alerterr.Visible = true;
            lbl_alerterr.Text = ex.ToString();
            d2.sendErrorMail(ex, collegecode1, "Building_Master.aspx");
        }
    }

    protected void btn_flrok_Click(object sender, EventArgs e)
    {
        try
        {
            imgdiv3.Visible = false;
            div_floor.Visible = false;
            Div2.Visible = false;
            ImageButton2.Visible = false;
            btn_go_click(sender, e);
        }
        catch (Exception ex)
        {
            lbl_alertf.Visible = true;
            lbl_alertf.Text = ex.ToString();
            d2.sendErrorMail(ex, collegecode1, "Building_Master.aspx");
        }
    }

    protected void btn_roomok_Click(object sender, EventArgs e)
    {
        try
        {
            imgdiv4.Visible = false;
            div_room.Visible = false;
            Div3.Visible = false;
            ImageButton3.Visible = false;
            btn_go_click(sender, e);
        }
        catch (Exception ex)
        {
            lbl_alertr.Visible = true;
            lbl_alertr.Text = ex.ToString();
            d2.sendErrorMail(ex, collegecode1, "Building_Master.aspx");
        }
    }

    protected void bindbuild()
    {
        try
        {
            ds.Clear();
            cbl_build.Items.Clear();
            cb_build.Checked = false;
            txt_build.Text = "--Select--";
            string item = "select distinct Building_Name,code from Building_Master order by code";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_build.DataSource = ds;
                cbl_build.DataTextField = "Building_Name";
                cbl_build.DataValueField = "code";
                cbl_build.DataBind();
                if (cbl_build.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_build.Items.Count; i++)
                    {
                        cbl_build.Items[i].Selected = true;
                    }
                    txt_build.Text = "Buildings (" + cbl_build.Items.Count + ")";
                    cb_build.Checked = true;
                }
            }
        }
        catch
        {


        }
    }

    protected void ddlbuild_SelectedIndexChanged(object sender, EventArgs e)
    {
        floor();
        room();
    }

    protected void floor()
    {
        try
        {
            ds.Clear();
            cbl_flr.Items.Clear();
            string bname = Convert.ToString(ddlbuild.SelectedItem);
            string item = "select distinct Floor_Name from Floor_Master where Building_Name='" + bname + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_flr.DataSource = ds;
                cbl_flr.DataTextField = "Floor_Name";
                //cbl_build.DataValueField = "code";
                cbl_flr.DataBind();
                if (cbl_flr.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_flr.Items.Count; i++)
                    {
                        cbl_flr.Items[i].Selected = true;
                    }
                    txt_flr.Text = "Floor (" + cbl_flr.Items.Count + ")";
                }
            }
            else
            {
                txt_flr.Text = "--Select--";
            }
            room();
        }
        catch
        {

        }
    }

    protected void room()
    {
        string flr = "";
        for (int i = 0; i < cbl_flr.Items.Count; i++)
        {
            if (cbl_flr.Items[i].Selected == true)
            {
                if (flr == "")
                {
                    flr = "" + cbl_flr.Items[i].Value.ToString() + "";
                }
                else
                {
                    flr = flr + "'" + "," + "'" + cbl_flr.Items[i].Value.ToString() + "";
                }
            }
        }
        ds.Clear();
        cbl_rm.Items.Clear();
        string statequery = "select distinct Room_Name from Room_Detail where Floor_Name in ('" + flr + "')";
        ds = da.select_method_wo_parameter(statequery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_rm.DataSource = ds;
            cbl_rm.DataTextField = "Room_Name";
            //cbl_rm.DataValueField = "item_code";
            cbl_rm.DataBind();
            cbl_rm.Visible = true;
            if (cbl_rm.Items.Count > 0)
            {
                for (int i = 0; i < cbl_rm.Items.Count; i++)
                {
                    cbl_rm.Items[i].Selected = true;
                }
                txt_rm.Text = "Room (" + cbl_rm.Items.Count + ")";
            }
        }
        else
        {
            txt_rm.Text = "--Select--";
        }
    }
    protected void cb_build_CheckedChange(object sender, EventArgs e)
    {

        if (cb_build.Checked == true)
        {
            for (int i = 0; i < cbl_build.Items.Count; i++)
            {
                cbl_build.Items[i].Selected = true;
            }
            txt_build.Text = "Buildings(" + cbl_build.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < cbl_build.Items.Count; i++)
            {
                cbl_build.Items[i].Selected = false;
            }
            txt_build.Text = "--Select--";
        }

    }

    protected void cbl_build_SelectedIndexChange(object sender, EventArgs e)
    {
        txt_build.Text = "--Select--";
        cb_build.Checked = false;
        int count = 0;
        for (int i = 0; i < cbl_build.Items.Count; i++)
        {
            if (cbl_build.Items[i].Selected == true)
            {
                count = count + 1;
            }
        }
        if (count > 0)
        {
            txt_build.Text = "Buildings(" + count + ")";
            if (count == cbl_build.Items.Count)
            {
                cb_build.Checked = true;
            }
        }
    }
    protected void cb_flr_CheckedChange(object sender, EventArgs e)
    {

        if (cb_flr.Checked == true)
        {
            for (int i = 0; i < cbl_flr.Items.Count; i++)
            {
                cbl_flr.Items[i].Selected = true;
            }
            txt_flr.Text = "Floor(" + cbl_flr.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < cbl_flr.Items.Count; i++)
            {
                cbl_flr.Items[i].Selected = false;
            }
            txt_flr.Text = "--Select--";
        }
        room();
    }
    protected void cbl_flr_SelectedIndexChange(object sender, EventArgs e)
    {
        txt_flr.Text = "--Select--";
        cb_flr.Checked = false;
        int count = 0;
        for (int i = 0; i < cbl_flr.Items.Count; i++)
        {
            if (cbl_flr.Items[i].Selected == true)
            {
                count = count + 1;
            }
        }
        if (count > 0)
        {
            txt_flr.Text = "Floor(" + count + ")";
            if (count == cbl_flr.Items.Count)
            {
                cb_flr.Checked = true;
            }
        }
        room();
    }

    protected void cb_rm_CheckedChange(object sender, EventArgs e)
    {

        if (cb_rm.Checked == true)
        {
            for (int i = 0; i < cbl_rm.Items.Count; i++)
            {
                cbl_rm.Items[i].Selected = true;
            }
            txt_rm.Text = "Room(" + cbl_rm.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < cbl_rm.Items.Count; i++)
            {
                cbl_rm.Items[i].Selected = false;
            }
            txt_rm.Text = "--Select--";
        }

    }
    protected void cbl_rm_SelectedIndexChange(object sender, EventArgs e)
    {
        txt_rm.Text = "--Select--";
        cb_rm.Checked = false;
        int count = 0;
        for (int i = 0; i < cbl_rm.Items.Count; i++)
        {
            if (cbl_rm.Items[i].Selected == true)
            {
                count = count + 1;
            }
        }
        if (count > 0)
        {
            txt_rm.Text = "Room(" + count + ")";
            if (count == cbl_rm.Items.Count)
            {
                cb_rm.Checked = true;
            }
        }
    }

    //button floor go button
    protected void btn_fgo_click(object sender, EventArgs e)
    {
        try
        {
            string buildName = string.Empty;
            if (ddlbname.Items.Count > 0)
                buildName = Convert.ToString(ddlbname.SelectedItem.Text);
            if (buildName == string.Empty)
                buildName = Convert.ToString(txt_bname.Text);
            string acrname = Convert.ToString(txt_facr.Text);
            string acrsno = Convert.ToString(txt_ssw.Text);
            string totalroolms = Convert.ToString(txt_totf.Text);
            if (!string.IsNullOrEmpty(buildName) && !string.IsNullOrEmpty(acrname) && !string.IsNullOrEmpty(acrsno) && !string.IsNullOrEmpty(totalroolms))
            {
                // string query = "select Building_Name,Floor_Acronym,Floor_Name from Floor_Master";
                // string query = "select Building_Name,Floor_Acronym,Floor_Name from Floor_Master where building_name='" + buildName + "' and floor_acronym='" + acrname + acrsno + "'";
                // ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables.Count == -1 && ds.Tables[0].Rows.Count > -1)
                {
                    #region
                    floorspread();
                    FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                    chkall.AutoPostBack = true;
                    string[] items = { "a", "b" };
                    FarPoint.Web.Spread.ComboBoxCellType combo = new FarPoint.Web.Spread.ComboBoxCellType(items);
                    combo.AutoPostBack = true;
                    FarPoint.Web.Spread.ButtonCellType button = new FarPoint.Web.Spread.ButtonCellType();
                    button.Text = "Add/Edit";
                    int noffloors = Convert.ToInt32(txt_totf.Text);
                    int val1 = Convert.ToInt32(txt_ssw.Text);
                    for (int i = 0; i < noffloors; i++)
                    {
                        FpSpread2.Sheets[0].RowCount++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = chkall;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                        string facr = txt_facr.Text;

                        string flooracr = "";
                        flooracr = facr + val1;
                        val1++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = flooracr;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = "";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].CellType = button;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        FpSpread2.Visible = true;
                        FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                        FpSpread2.SaveChanges();
                        btn_flrsave.Visible = true;
                        btn_flrdelete.Visible = true;
                    }
                    #endregion
                }
                else
                {
                    #region
                    FpSpread2.Sheets[0].RowHeader.Visible = false;
                    FpSpread2.CommandBar.Visible = false;
                    FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                    chkall.AutoPostBack = false;
                    FarPoint.Web.Spread.ButtonCellType button = new FarPoint.Web.Spread.ButtonCellType();
                    button.Text = "Add/Edit";
                    floorspread();
                    int noffloors = Convert.ToInt32(txt_totf.Text);
                    int val1 = 0;
                    string startvalue = Convert.ToString(txt_ssw.Text);
                    int s_len = startvalue.Length;
                    val1 = Convert.ToInt32(startvalue);

                    FpSpread2.Sheets[0].RowCount = 0;
                    for (int j = 0; j < noffloors; j++)
                    {

                        FpSpread2.Sheets[0].RowCount++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread2.Sheets[0].RowCount - 1 + 1);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = chkall;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        string s = "";
                        string ii = "";
                        string facr = txt_facr.Text;
                        int val1_len = Convert.ToString(val1).Length;
                        if (val1_len != s_len)
                        {
                            int v = s_len - val1_len;
                            s = ii.ToString().PadLeft(v, '0');
                        }
                        string flooracr = "";
                        flooracr = facr + s + val1;
                        //   val1++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = facr;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = "";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                        //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].CellType = button;
                        //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";


                    }
                    FpSpread2.Visible = true;
                    FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                    FpSpread2.SaveChanges();
                    btn_flrsave.Visible = true;
                    // btn_flrdelete.Visible = true;
                    #endregion
                }
            }
            else
            {
                alertdiv.Visible = true;
                lblalert.Visible = true;
                lblalert.Text = "Please Fill All The Fields";
            }
        }
        catch (Exception ex)
        {
            Label5.Visible = true;
            Label5.Text = ex.ToString();
        }
    }

    public void floorspread()
    {
        FpSpread2.Sheets[0].RowCount = 0;
        FpSpread2.Sheets[0].ColumnCount = 0;
        FpSpread2.CommandBar.Visible = false;
        FpSpread2.Sheets[0].AutoPostBack = false;
        FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
        FpSpread2.Sheets[0].RowHeader.Visible = false;
        FpSpread2.Sheets[0].ColumnCount = 5;
        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        darkstyle.ForeColor = Color.White;
        FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
        FpSpread2.Columns[0].Locked = true;
        FpSpread2.Columns[0].Width = 50;

        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
        FpSpread2.Columns[1].Width = 80;



        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Floor Acronym";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
        FpSpread2.Columns[2].Width = 100;

        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Floor Name";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
        FpSpread2.Columns[3].Width = 175;

        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Room";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
        FpSpread2.Columns[4].Width = 175;
        FpSpread2.Columns[4].Visible = false;
    }
    protected void btn_popgo_click(object sender, EventArgs e)
    {
        try
        {
            //string query = "select Code,Building_Acronym,StartingSerial,Building_Name,Building_Area,Building_Colour,BuildType,Building_Type From Building_Master";
            //ds = d2.select_method_wo_parameter(query, "Text");
            //if (FpSpread1.Sheets[0].RowCount == 0)
            //{
            FpSpread1.Visible = true;
            btn_save.Visible = true;
            btn_delete.Visible = true;

            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 9;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Columns[0].Locked = true;
            FpSpread1.Columns[0].Width = 50;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Columns[1].Width = 80;

            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
            chkall.AutoPostBack = true;

            string[] items = { "RCC", "ACC" };
            FarPoint.Web.Spread.ComboBoxCellType combo = new FarPoint.Web.Spread.ComboBoxCellType(items);
            combo.AutoPostBack = true;
            //string[] items1 = { "a", "b" };//delsi

            FarPoint.Web.Spread.ComboBoxCellType combo1 = new FarPoint.Web.Spread.ComboBoxCellType();

            string query = "select distinct TextCode,TextVal from textvaltable where TextCriteria ='Btype' and college_code='" + collegecode1 + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                combo1.DataSource = ds;
                combo1.DataTextField = "TextVal";
                combo1.DataValueField = "TextCode";



            }


            combo1.AutoPostBack = true;
            FarPoint.Web.Spread.ButtonCellType button = new FarPoint.Web.Spread.ButtonCellType();
            button.Text = "Add/Edit";

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Building Acronym";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Columns[2].Width = 100;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Building Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Columns[3].Width = 175;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Building Area";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Columns[4].Width = 100;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Building Colour";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpread1.Columns[5].Width = 150;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Building Type";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            FpSpread1.Columns[6].Width = 100;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Type";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            FpSpread1.Columns[7].Width = 75;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Floor";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            FpSpread1.Columns[8].Width = 75;
            FpSpread1.Columns[8].Visible = false;


            //if (ds.Tables[0].Rows.Count > 0)
            //{

            //    int nofbuild = Convert.ToInt32(txt_nofbuild.Text);
            //    int val1 = Convert.ToInt32(txt_serial.Text);
            //    for (int i = 0; i < nofbuild; i++)
            //    {
            //        FpSpread1.Sheets[0].RowCount++;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chkall;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

            //        string acr1 = txt_buildacr.Text;

            //        string buildacr = "";
            //        buildacr = acr1 + val1;
            //        val1++;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = buildacr;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = "";
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "";
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = "";
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].CellType = combo;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].CellType = combo1;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].CellType = button;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
            //    }
            //}
            //else
            //{
            //FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
            //chkall.AutoPostBack = true;

            //string[] items = { "RCC", "ACC" };
            //FarPoint.Web.Spread.ComboBoxCellType combo = new FarPoint.Web.Spread.ComboBoxCellType(items);
            //combo.AutoPostBack = true;
            //string[] items1 = { "a", "b" };
            //FarPoint.Web.Spread.ComboBoxCellType combo1 = new FarPoint.Web.Spread.ComboBoxCellType(items);
            //combo.AutoPostBack = true;
            //FarPoint.Web.Spread.ButtonCellType button = new FarPoint.Web.Spread.ButtonCellType();
            //button.Text = "Add/Edit";
            int nofbuild = 0;
            if (txt_nofbuild.Text != "")
            {
                nofbuild = Convert.ToInt32(txt_nofbuild.Text);
            }
            int val1 = 0;
            //= Convert.ToInt32(txt_serial.Text);
            string startvalue = Convert.ToString(txt_serial.Text);
            int s_len = startvalue.Length;
            val1 = Convert.ToInt32(startvalue);

            for (int i = 0; i < nofbuild; i++)
            {
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chkall;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                string s = "";
                string ii = "";
                string acr1 = txt_buildacr.Text;
                int val1_len = Convert.ToString(val1).Length;
                if (val1_len < s_len)
                {
                    int v = s_len - val1_len;
                    s = ii.ToString().PadLeft(v, '0');
                }
                string buildacr = "";
                buildacr = acr1 + s + val1;

                val1++;

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = buildacr;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = buildacr;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = "";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].CellType = combo;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].CellType = combo1;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].CellType = button;
                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
            }
            //}
            //}
            //else
            //{
            //    FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
            //    chkall.AutoPostBack = true;

            //    string[] items = { "RCC", "ACC" };
            //    FarPoint.Web.Spread.ComboBoxCellType combo = new FarPoint.Web.Spread.ComboBoxCellType(items);
            //    combo.AutoPostBack = true;
            //    string[] items1 = { "a", "b" };
            //    FarPoint.Web.Spread.ComboBoxCellType combo1 = new FarPoint.Web.Spread.ComboBoxCellType(items);
            //    combo.AutoPostBack = true;
            //    FarPoint.Web.Spread.ButtonCellType button = new FarPoint.Web.Spread.ButtonCellType();
            //    button.Text = "Add/Edit";
            //    int nofbuild = Convert.ToInt32(txt_nofbuild.Text);
            //    int val1 = Convert.ToInt32(txt_serial.Text);
            //    for (int i = 0; i < nofbuild; i++)
            //    {
            //        FpSpread1.Sheets[0].RowCount++;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].RowCount - 1 + 1);
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chkall;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

            //        string acr1 = txt_buildacr.Text;

            //        string buildacr = "";
            //        buildacr = acr1 + val1;
            //        val1++;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = buildacr;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = buildacr;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "";
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = "";
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].CellType = combo;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].CellType = combo1;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

            //        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].CellType = button;
            //        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
            //        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
            //        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
            //    }
            //}
            FpSpread1.Visible = true;
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();

        }

        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }
    //room button go 
    protected void btn_roomgo_click(object sender, EventArgs e)
    {
        try
        {
            string buildName = string.Empty;
            string floorName = string.Empty;
            if (ddlrbname.Items.Count > 0)
                buildName = Convert.ToString(ddlrbname.SelectedItem.Text);
            if (buildName == string.Empty)
                buildName = Convert.ToString(txt_rbn.Text);
            if (ddlrflrm.Items.Count > 0)
                floorName = Convert.ToString(ddlrflrm.SelectedItem.Text);
            if (floorName == string.Empty)
                floorName = Convert.ToString(txt_rflrn.Text);
            string acrname = Convert.ToString(txt_racr.Text);
            string acrsno = Convert.ToString(txt_ss.Text);
            string totalroolms = Convert.ToString(txt_rtot.Text);
            if (!string.IsNullOrEmpty(buildName) && !string.IsNullOrEmpty(floorName) && !string.IsNullOrEmpty(acrname) && !string.IsNullOrEmpty(acrsno) && !string.IsNullOrEmpty(totalroolms))
            {
                FpSpread3.Visible = false;
                //   string query = "select Room_Acronym,Room_Name,Room_type,no_of_rows,no_of_columns,room_size,students_allowed,StudPerSeat from Room_Detail ";
                //  ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables.Count == -1 && ds.Tables[0].Rows.Count > -1)
                {
                    #region addnew click
                    roomspread();
                    FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                    chkall.AutoPostBack = true;

                    string[] items = { "AC", "NON-AC" };
                    FarPoint.Web.Spread.ComboBoxCellType combo = new FarPoint.Web.Spread.ComboBoxCellType(items);
                    combo.AutoPostBack = true;
                    FarPoint.Web.Spread.ButtonCellType button = new FarPoint.Web.Spread.ButtonCellType();
                    button.Text = "Edit";
                    int totrooms = Convert.ToInt32(txt_rtot.Text);
                    int val1 = Convert.ToInt32(txt_ss.Text);
                    for (int i = 0; i < totrooms; i++)
                    {
                        FpSpread3.Sheets[0].RowCount++;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].CellType = chkall;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                        string acr1 = txt_racr.Text;

                        string roomacr = "";
                        roomacr = acr1 + val1;
                        val1++;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Text = roomacr;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Text = "";
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].CellType = combo;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Text = "";
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].Text = "";
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Text = "";
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].Text = "";
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                        //FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].CellType = button;
                        //FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                        //FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                        //FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].Text = "";
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].Text = "";
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 11].Text = "";
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";

                        FpSpread3.Visible = true;
                        FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
                        FpSpread3.SaveChanges();
                    }
                    #endregion
                }
                else
                {
                    #region button click
                    FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                    chkall.AutoPostBack = false;
                    FpSpread3.Sheets[0].ColumnCount = 11;
                    string[] items = { "AC", "NON-AC" };
                    FarPoint.Web.Spread.ComboBoxCellType combo = new FarPoint.Web.Spread.ComboBoxCellType(items);
                    combo.AutoPostBack = true;
                    FarPoint.Web.Spread.ButtonCellType button = new FarPoint.Web.Spread.ButtonCellType();
                    button.Text = "Edit";
                    roomspread();
                    FpSpread3.Sheets[0].RowCount = 0;
                    int totrooms = Convert.ToInt32(txt_rtot.Text);
                    int val1 = 0;
                    string startvalue = Convert.ToString(txt_ss.Text);
                    int s_len = startvalue.Length;
                    val1 = Convert.ToInt32(startvalue);


                    int c = 0;
                    for (int j = 0; j < totrooms; j++)
                    {
                        FpSpread3.Sheets[0].RowCount++;
                        c++;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(c);
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].CellType = chkall;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                        string acr1 = txt_racr.Text;
                        string s = "";
                        string ii = "";
                        int val1_len = Convert.ToString(val1).Length;
                        if (val1_len != s_len)
                        {
                            int v = val1_len - s_len;
                            s = ii.ToString().PadLeft(v, '0');
                        }
                        string roomacr = "";
                        roomacr = acr1 + s + val1;

                        val1++;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Text = acr1;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Text = "";
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].CellType = combo;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Text = "";
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].Text = "";
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Text = "";
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].Text = "";
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].Text = "";
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 11].Text = "";
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";
                    }
                    FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
                    FpSpread3.SaveChanges();
                    FpSpread3.Visible = true;
                    #endregion
                }
            }
            else
            {
                alertdiv.Visible = true;
                lblalert.Visible = true;
                lblalert.Text = "Please Fill All The Fields";
            }
        }
        catch (Exception ex)
        {
            lbl_ralert.Visible = true;
            lbl_ralert.Text = ex.ToString();
        }

    }

    public void roomspread()
    {
        FpSpread3.Visible = true;
        btn_rsave.Visible = true;
        //  btn_rdelete.Visible = true;

        FpSpread3.Sheets[0].RowCount = 0;
        FpSpread3.Sheets[0].ColumnCount = 0;
        FpSpread3.CommandBar.Visible = false;
        FpSpread3.Sheets[0].AutoPostBack = false;
        FpSpread3.Sheets[0].ColumnHeader.RowCount = 1;
        FpSpread3.Sheets[0].RowHeader.Visible = false;
        FpSpread3.Sheets[0].ColumnCount = 12;
        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        darkstyle.ForeColor = Color.White;
        FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
        FpSpread3.Columns[0].Locked = true;
        FpSpread3.Columns[0].Width = 50;

        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
        FpSpread3.Columns[1].Width = 80;



        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Room Acronym";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
        FpSpread3.Columns[2].Width = 100;

        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Room Name";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
        FpSpread3.Columns[3].Width = 175;

        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Room Type";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
        FpSpread3.Columns[4].Width = 100;

        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Text = "No Of Rows";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
        FpSpread3.Columns[5].Width = 150;


        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Text = "No Of Columns";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
        FpSpread3.Columns[6].Width = 100;

        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Room Size";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
        FpSpread3.Columns[7].Width = 75;

        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Max Allowed Students";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
        FpSpread3.Columns[8].Width = 100;

        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Seat Arrangement Details";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
        FpSpread3.Columns[9].Width = 100;

        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Students Per Seat";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
        FpSpread3.Columns[10].Width = 100;

        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Max Allowed Class Strength";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
        FpSpread3.Columns[11].Width = 100;

    }

    //added by sudhagar 02.01.2017
    //room
    protected void ddlbname_SelectedIndexChanged(object sender, EventArgs e)
    {
        getAcronym();
    }
    //floor
    protected void ddlrbname_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindFloor();
        getAcronym();
    }
    protected void ddlrflrm_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void bindBuilding()
    {
        ds.Clear();
        ddlbname.Items.Clear();
        ddlrbname.Items.Clear();
        string item = "select distinct Building_Name,code from Building_Master order by code";
        ds = d2.select_method_wo_parameter(item, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            //if (rb_floor.Checked)
            //{
            ddlbname.DataSource = ds;
            ddlbname.DataTextField = "Building_Name";
            ddlbname.DataValueField = "code";
            ddlbname.DataBind();
            //}
            //else if(rb_room.Checked)
            //{
            ddlrbname.DataSource = ds;
            ddlrbname.DataTextField = "Building_Name";
            ddlrbname.DataValueField = "code";
            ddlrbname.DataBind();
            bindFloor();
            //}
        }
    }

    protected void bindFloor()
    {
        try
        {
            ds.Clear();
            ddlrflrm.Items.Clear();
            string buildName = string.Empty;
            if (ddlrbname.Items.Count > 0)
                buildName = Convert.ToString(ddlrbname.SelectedItem.Text);
            if (!string.IsNullOrEmpty(buildName))
            {
                string item = "select distinct Floor_Name from Floor_Master where Building_Name='" + buildName + "'";
                ds = d2.select_method_wo_parameter(item, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlrflrm.DataSource = ds;
                    ddlrflrm.DataTextField = "Floor_Name";
                    ddlrflrm.DataValueField = "Floor_Name";
                    ddlrflrm.DataBind();
                }
            }
        }
        catch
        {
        }
    }

    protected void getAcronym()
    {
        try
        {
            string buildname = string.Empty;
            string fllorname = string.Empty;
            string strbuildname = string.Empty;
            string strfllorname = string.Empty;
            if (rb_floor.Checked)
            {
                if (ddlbname.Items.Count > 0)
                    buildname = Convert.ToString(ddlbname.SelectedItem.Text);
                if (!string.IsNullOrEmpty(buildname))
                    strbuildname = "Building_Name='" + buildname + "'";
            }
            else
            {
                if (ddlrbname.Items.Count > 0)
                    buildname = Convert.ToString(ddlrbname.SelectedItem.Text);
                if (ddlrflrm.Items.Count > 0)
                    fllorname = Convert.ToString(ddlrflrm.SelectedItem.Text);
                if (!string.IsNullOrEmpty(buildname))
                    strbuildname = "Building_Name='" + buildname + "'";
                if (!string.IsNullOrEmpty(fllorname))
                    strfllorname = " and floor_name='" + fllorname + "'";
            }

            string SelQ = "select distinct floor_acronym,startingserial from Floor_Master where " + strbuildname + " " + strfllorname + "";
            SelQ += "select distinct Room_Acronym,Room_Name,startingserial from Room_Detail where " + strbuildname + " " + strfllorname + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(SelQ, "Text");
            if (ds.Tables.Count > 0)
            {
                if (rb_floor.Checked && ds.Tables[0].Rows.Count > 0)
                {
                    txt_facr.Text = Convert.ToString(ds.Tables[0].Rows[0]["floor_acronym"]);
                    txt_ssw.Text = Convert.ToString(ds.Tables[0].Rows[0]["startingserial"]);
                }
                else
                {
                    txt_facr.Text = string.Empty;
                    txt_ssw.Text = string.Empty;
                }
                if (rb_room.Checked && ds.Tables[1].Rows.Count > 0)
                {
                    txt_racr.Text = Convert.ToString(ds.Tables[1].Rows[0]["Room_Acronym"]);
                    txt_ss.Text = Convert.ToString(ds.Tables[1].Rows[0]["startingserial"]);
                }
                else
                {
                    txt_racr.Text = string.Empty;
                    txt_ss.Text = string.Empty;
                }
            }
        }
        catch { }
    }

    protected void rb_building_Changed(object sender, EventArgs e)
    {
    }
    protected void rb_floor_Changed(object sender, EventArgs e)
    {
        bindBuilding();
    }
    protected void rb_room_Changed(object sender, EventArgs e)
    {
        bindBuilding();
    }
    protected void btn_close_Click(object sender, EventArgs e)
    {
        alertdiv.Visible = false;
    }

    public void btnerrclose22_Click(object sender, EventArgs e)
    {
        imgdiv6.Visible = false;
        panel_erroralert2.Visible = false;

    }



    public void btn_ad1_Click(object sender, EventArgs e)
    {
        imgdiv5.Visible = true;
        panel_description2.Visible = true;
    }

    public void btn_min1_Click(object sender, EventArgs e)
    {
        if (ddl_viewdetails2.SelectedIndex == -1)
        {
            imgdiv6.Visible = true;
            panel_erroralert2.Visible = true;
            lbl_erroralert2.Text = "No records found";
        }
        else if (ddl_viewdetails2.SelectedIndex == 0)
        {
            imgdiv6.Visible = true;
            panel_erroralert2.Visible = true;
            lbl_erroralert2.Text = "Select any record";
        }
        else if (ddl_viewdetails2.SelectedIndex != 0)
        {
            imgdivcnfm2.Visible = true;
            //imgdiv6.Visible = false;
        }
        else
        {
            imgdiv4.Visible = true;
            lbl_erroralert2.Text = "No records found";
        }
    }




    public void btn_adddesc2_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_description12.Text != "")
            {
                string sql = "if exists ( select * from textvaltable where TextVal ='" + txt_description12.Text + "' and TextCriteria ='Btype' and college_code ='" + collegecode1 + "') update textvaltable set TextVal ='" + txt_description12.Text + "' where TextVal ='" + txt_description12.Text + "' and TextCriteria='Btype' and college_code ='" + collegecode1 + "' else insert into textvaltable (TextVal,TextCriteria,college_code) values ('" + txt_description12.Text + "','Btype','" + collegecode1 + "')";
                int insert = d2.update_method_wo_parameter(sql, "TEXT");
                if (insert != 0)
                {
                    imgdiv6.Visible = true;
                    lbl_erroralert2.Text = "Saved sucessfully";
                    panel_erroralert2.Visible = true;
                    txt_description12.Text = "";
                    imgdiv5.Visible = false;
                    panel_description2.Visible = false;
                }
            }
            else
            {
                imgdiv6.Visible = true;
                lbl_erroralert2.Text = "Enter the description";
            }

            loaddesc1();
        }
        catch (Exception ex)
        {
        }
    }

    public void btn_exitdesc2_Click(object sender, EventArgs e)
    {
        imgdiv5.Visible = false;
        panel_description2.Visible = false;
    }


    public void btn_errorclose_cnfm2_Click(object sender, EventArgs e)
    {
        imgdivcnfm2.Visible = false;
        string sql = "delete from textvaltable where TextCode='" + ddl_viewdetails2.SelectedItem.Value.ToString() + "' and TextCriteria ='Btype' and college_code='" + collegecode1 + "' ";
        int delete = d2.update_method_wo_parameter(sql, "TEXT");
        if (delete != 0)
        {
            imgdiv6.Visible = true;
            lbl_erroralert2.Text = "Deleted Sucessfully";
            panel_erroralert2.Visible = true;
        }
        else
        {
            imgdiv6.Visible = true;
            lbl_erroralert2.Text = "No records found";
        }
        loaddesc1();
    }


    public void btn_errorclose_cncl2_Click(object sender, EventArgs e)
    {
        imgdivcnfm2.Visible = false;
    }



    public void loaddesc1()
    {
        ddl_viewdetails2.Items.Clear();
        ds.Tables.Clear();


        string sql = "select TextCode,TextVal from textvaltable where TextCriteria ='Btype' and college_code ='" + collegecode1 + "'";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_viewdetails2.DataSource = ds;
            ddl_viewdetails2.DataTextField = "TextVal";
            ddl_viewdetails2.DataValueField = "TextCode";
            ddl_viewdetails2.DataBind();
            ddl_viewdetails2.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            ddl_viewdetails2.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

}