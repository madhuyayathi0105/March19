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
using System.Web.Services;
using System.Drawing;
public partial class room_rent_master : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    bool check = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("hostelindex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/HostelMod/hostelindex.aspx");
                    return;
                }
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                loadroomtype();
                loadcollege();
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.Visible = false;
                btn_go_Click(sender, e);
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_duedate.Attributes.Add("readonly", "readonly");
                txt_duedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                rdb_yearly.Checked = true;

            }
            lblvalidation1.Visible = false;
        }
        catch (Exception ex) { }
    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch
        {

        }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            try
            {
                Printcontrol.Visible = false;
                string clgcode = "";
                clgcode = Convert.ToString(ddl_collegename.SelectedItem.Value);
                string itemheadercode = "";
                for (int i = 0; i < cbl_roomtype.Items.Count; i++)
                {
                    if (cbl_roomtype.Items[i].Selected == true)
                    {
                        if (itemheadercode == "")
                        {
                            itemheadercode = "" + cbl_roomtype.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            itemheadercode = itemheadercode + "'" + "," + "'" + cbl_roomtype.Items[i].Value.ToString() + "";
                        }
                    }
                }

                if (itemheadercode.Trim() != "")
                {
                    string selectquery = "";
                    selectquery = "select Room_Type,CONVERT(varchar(10), From_Date,103) as From_Date,CONVERT(varchar(10), To_Date,103) as To_Date, case when Rent_Type=1 then 'Monthly' when Rent_Type =2 then 'Yearly' when Rent_Type =3 then 'Semester' end as renttype,Room_Cost,CONVERT(varchar(10), Due_Date,103) as Due_Date,RoomCost_Code  from RoomCost_Master where  Room_Type in('" + itemheadercode + "')and college_code in('" + clgcode + "')";



                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Fpspread1.Sheets[0].RowCount = 0;
                        Fpspread1.Sheets[0].ColumnCount = 0;
                        Fpspread1.CommandBar.Visible = false;
                        Fpspread1.Sheets[0].AutoPostBack = true;
                        Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpspread1.Sheets[0].RowHeader.Visible = false;
                        Fpspread1.Sheets[0].ColumnCount = 7;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Columns[0].Width = 50;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Room Type";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Columns[1].Width = 100;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "From Date";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Columns[2].Width = 100;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "To Date";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Columns[3].Width = 100;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Rent Type";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Columns[4].Width = 100;


                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Room Cost";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Due Date";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;



                        //   FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();

                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            Fpspread1.Sheets[0].RowCount++;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["Room_Type"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["RoomCost_Code"]);

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["From_Date"]);

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["To_Date"]);
                            //  Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].CellType = txtcell;

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["renttype"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["Room_Cost"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["Due_Date"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";



                        }
                        Fpspread1.Visible = true;
                        rptprint.Visible = true;
                        div1.Visible = true;
                        lbl_error.Visible = false;
                        Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    }
                    else
                    {
                        div1.Visible = false;
                        Fpspread1.Visible = false;
                        rptprint.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "No Records Found";
                    }
                }
                else
                {
                    div1.Visible = false;
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "Please Select Any one Item Name";
                }
            }
            catch
            {

            }
        }
        catch
        {
        }
    }

    protected void cb_roomtype_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_roomtype.Checked == true)
            {
                for (int i = 0; i < cbl_roomtype.Items.Count; i++)
                {
                    cbl_roomtype.Items[i].Selected = true;
                }
                txt_roomtype.Text = "Room Type(" + (cbl_roomtype.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_roomtype.Items.Count; i++)
                {
                    cbl_roomtype.Items[i].Selected = false;
                }
                txt_roomtype.Text = "--Select--";
            }

        }
        catch (Exception ex)
        {

        }
    }
    protected void cbl_roomtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_roomtype.Text = "--Select--";
            cb_roomtype.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_roomtype.Items.Count; i++)
            {
                if (cbl_roomtype.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_roomtype.Text = "Room Type(" + commcount.ToString() + ")";
                if (commcount == cbl_roomtype.Items.Count)
                {
                    cb_roomtype.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void loadroomtype()
    {

        try
        {
            cbl_roomtype.Items.Clear();

            string sql = "select distinct Room_type from Room_Detail where  college_code ='" + collegecode1 + "' and Room_type<>''";
            ds.Clear();
            ds = d2.select_method_wo_parameter(sql, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_roomtype.DataSource = ds;
                cbl_roomtype.DataTextField = "Room_type";
                cbl_roomtype.DataValueField = "Room_type";
                cbl_roomtype.DataBind();
                if (cbl_roomtype.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_roomtype.Items.Count; i++)
                    {
                        cbl_roomtype.Items[i].Selected = true;
                    }
                    txt_roomtype.Text = "Room Type(" + cbl_roomtype.Items.Count + ")";
                    cb_roomtype.Checked = true;
                }

            }
            else
            {
                txt_roomtype.Text = "--Select--";

            }

        }

        catch
        {

        }

    }
    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    public void loadcollege()
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }
            //binddept(ddl_collegename.SelectedItem.Value.ToString());
        }
        catch
        {
        }
    }

    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        btn_delete.Visible = false;
        btn_update.Visible = false;
        btn_save.Visible = true;
        poperrjs.Visible = true;

        txt_fromdate.Attributes.Add("readonly", "readonly");
        txt_todate.Attributes.Add("readonly", "readonly");
        txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_duedate.Attributes.Add("readonly", "readonly");
        txt_duedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        //rdb_monthly.Checked = true;
        rdb_yearly.Checked = true;
        rdb_sem.Checked = false;
        txt_cost.Text = "";
        loadclgadd();
        loadroomtypeadd();


    }
    protected void Cell_Click(object sender, EventArgs e)
    {
        try
        {
            check = true;
        }
        catch
        {

        }
    }

    protected void Fpspread1_render(object sender, EventArgs e)
    {
        try
        {
            if (check == true)
            {
                poperrjs.Visible = true;
                btn_delete.Visible = true;
                btn_update.Visible = true;
                btn_save.Visible = false;
                string activerow = "";
                string activecol = "";
                activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();


                if (activerow.Trim() != "")
                {

                    string roomtype = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    string roomcostcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                    string fromdate = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                    string todate = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                    string renttype = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
                    string roomcost = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
                    string duedate = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text);
                    //  ddl_roomtype.SelectedItem.Text = Convert.ToString(roomtype);
                    txt_fromdate.Text = Convert.ToString(fromdate);
                    txt_todate.Text = Convert.ToString(todate);
                    txt_duedate.Text = Convert.ToString(duedate);
                    txt_cost.Text = Convert.ToString(roomcost);
                    string clgcode = d2.GetFunction("select college_code from RoomCost_Master where RoomCost_Code='" + roomcostcode + "' ");
                    //if (renttype == "Monthly")
                    //{
                    //    rdb_monthly.Checked = true;
                    //    rdb_yearly.Checked = false;
                    //    rdb_sem.Checked = false;
                    //}
                    if (renttype == "Yearly")
                    {
                        rdb_yearly.Checked = true;
                        // rdb_monthly.Checked = false;
                        rdb_sem.Checked = false;
                    }
                    else if (renttype == "Semester")
                    {
                        rdb_sem.Checked = true;
                        rdb_yearly.Checked = false;
                        // rdb_monthly.Checked = false;
                    }
                    loadroomtypeadd();
                    ddl_roomtype.SelectedIndex = ddl_roomtype.Items.IndexOf(ddl_roomtype.Items.FindByText(roomtype));
                    loadclgadd();
                    ddl_clgadd.SelectedIndex = ddl_clgadd.Items.IndexOf(ddl_clgadd.Items.FindByText(clgcode));

                }
            }
            else
            {

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
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {

        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Room Rent Master Report";
            string pagename = "room_rent_master.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }

    }

    //protected void txt_fromdate_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (txt_fromdate.Text != "" && txt_todate.Text != "")
    //        {
    //            //txt_leavedays.Text = "";
    //            DateTime dt = new DateTime();
    //            DateTime dt1 = new DateTime();
    //            string firstdate = Convert.ToString(txt_fromdate.Text);
    //            string seconddate = Convert.ToString(txt_todate.Text);
    //            string[] split = firstdate.Split('/');
    //            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
    //            split = seconddate.Split('/');
    //            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
    //            TimeSpan ts = dt1 - dt;
    //            int days = ts.Days;

    //            if (dt > dt1)
    //            {
    //                imgdiv2.Visible = true;
    //                lbl_alerterr.Text = "Enter From Date Less than or Equal to the To Date";
    //                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
    //                //txt_leavedays.Text = "";
    //                //txt_rebatedays.Text = "";
    //            }
    //            else
    //            {
    //                //txt_leavedays.Text = Convert.ToString(days);
    //                //txt_rebatedays.Text = Convert.ToString(days);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //    // PopupMessage("Enter FromDate less than or equal to the ToDate", cv_fromtodt1);
    //}
    protected void txt_todate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_fromdate.Text != "" && txt_todate.Text != "")
            {
                //txt_leavedays.Text = "";
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate.Text);
                string seconddate = Convert.ToString(txt_todate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;
                if (dt > dt1)
                {
                    imgdiv2.Visible = true;
                    lbl_alerterr.Text = "Enter ToDate greater than or equal to the FromDate ";
                    txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //txt_leavedays.Text = "";
                    //txt_rebatedays.Text = "";
                }
                else
                {
                    //txt_leavedays.Text = Convert.ToString(days);
                    //txt_rebatedays.Text = Convert.ToString(days);
                }

            }
        }
        catch (Exception ex)
        {
        }

        // PopupMessage("Enter ToDate greater than or equal to the FromDate", cv_fromtodt2);
    }

    protected void loadroomtypeadd()
    {
        ddl_roomtype.Items.Clear();
        ds.Clear();
        string sql = "select distinct Room_type from Room_Detail where  college_code ='" + collegecode1 + "' and Room_type<>''";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_roomtype.DataSource = ds;
            ddl_roomtype.DataTextField = "Room_type";
            ddl_roomtype.DataValueField = "Room_type";
            ddl_roomtype.DataBind();
            ddl_roomtype.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            ddl_roomtype.Items.Insert(0, new ListItem("Select", "0"));
        }


    }

    protected void ddl_clgadd_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadroomtypeadd();
    }
    public void loadclgadd()
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_clgadd.DataSource = ds;
                ddl_clgadd.DataTextField = "collname";
                ddl_clgadd.DataValueField = "college_code";
                ddl_clgadd.DataBind();
            }
            //binddept(ddl_collegename.SelectedItem.Value.ToString());
        }
        catch
        {
        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            string pay = "";
            string clgcode = "";
            clgcode = Convert.ToString(ddl_clgadd.SelectedItem.Value);
            string roomtype = "";
            roomtype = Convert.ToString(ddl_roomtype.SelectedItem.Text);
            //if (rdb_monthly.Checked == true)
            //{
            //    pay = "1";
            //}
            if (rdb_yearly.Checked == true)
            {
                pay = "2";
            }
            else if (rdb_sem.Checked == true)
            {
                pay = "3";
            }
            string date = "";
            date = Convert.ToString(txt_fromdate.Text);
            string[] splitdate = date.Split('-');
            splitdate = splitdate[0].Split('/');
            DateTime dt = new DateTime();
            if (splitdate.Length > 0)
            {
                dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            }
            string getday = "";
            getday = dt.ToString("MM/dd/yyyy");

            string todate = "";
            todate = Convert.ToString(txt_todate.Text);
            string[] splittodate = todate.Split('-');
            splittodate = splittodate[0].Split('/');
            DateTime dttodate = new DateTime();
            if (splittodate.Length > 0)
            {
                dttodate = Convert.ToDateTime(splittodate[1] + "/" + splittodate[0] + "/" + splittodate[2]);
            }
            string getday1 = "";
            getday1 = dttodate.ToString("MM/dd/yyyy");

            string duedate = "";
            duedate = Convert.ToString(txt_duedate.Text);
            string[] splitduedate = duedate.Split('-');
            splitduedate = splitduedate[0].Split('/');
            DateTime dtduedate = new DateTime();
            if (splitduedate.Length > 0)
            {
                dtduedate = Convert.ToDateTime(splitduedate[1] + "/" + splitduedate[0] + "/" + splitduedate[2]);
            }
            string getday2 = "";
            getday2 = dtduedate.ToString("MM/dd/yyyy");

            string dtaccessdate = "";
            dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = "";
            dtaccesstime = DateTime.Now.ToLongTimeString();
            string cost = "";
            cost = Convert.ToString(txt_cost.Text);
            string datecheck = d2.GetFunction("select To_Date from RoomCost_Master where from_date>='" + getday + "' or To_Date >='" + getday + "' and from_date>='" + dt + "' or To_Date >='" + dt + "'");
            if (datecheck == "" || datecheck == "0")
            {

                string query = "if exists (select * from RoomCost_Master where Room_Type ='" + roomtype + "' and Rent_Type ='" + pay + "' )update RoomCost_Master set Access_Date='" + dtaccessdate + "',Access_Time='" + dtaccesstime + "',Room_Type='" + roomtype + "',From_Date='" + getday + "',To_Date='" + getday1 + "', Rent_Type='" + pay + "',Room_Cost='" + cost + "', Due_Date='" + getday2 + "',college_code='" + clgcode + "' where Room_Type='" + roomtype + "' and Rent_Type ='" + pay + "' else insert into RoomCost_Master (Access_Date,Access_Time,Room_Type,From_Date,To_Date,Rent_Type,Room_Cost,Due_Date,college_code) values ('" + dtaccessdate + "','" + dtaccesstime + "','" + roomtype + "','" + getday + "','" + getday1 + "','" + pay + "','" + cost + "','" + getday2 + "','" + clgcode + "')";

                int iv = d2.update_method_wo_parameter(query, "Text");
                if (iv != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alerterr.Text = "Saved Successfully";
                    clear();
                    poperrjs.Visible = true;
                    btn_go_Click(sender, e);
                    rdb_yearly.Checked = true;

                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Please Change The FromDate";
            }
        }
        catch (Exception ex)
        {
        }






    }
    public void clear()
    {
        loadroomtype();
        loadcollege();
        loadclgadd();
        loadroomtypeadd();
        txt_fromdate.Attributes.Add("readonly", "readonly");
        txt_todate.Attributes.Add("readonly", "readonly");
        txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_duedate.Attributes.Add("readonly", "readonly");
        txt_duedate.Text = DateTime.Now.ToString("dd/MM/yyyy");

        txt_cost.Text = "";

        //rdb_monthly.Checked = false;
        //rdb_yearly.Checked = false;
        //rdb_sem.Checked = false;
    }
    protected void savedetails()
    {

    }

    protected void btn_update_Click(object sender, EventArgs e)
    {

        try
        {
            string pay = "";
            string clgcode = "";
            clgcode = Convert.ToString(ddl_clgadd.SelectedItem.Value);
            string roomtype = "";
            roomtype = Convert.ToString(ddl_roomtype.SelectedItem.Text);

            //if (rdb_monthly.Checked == true)
            //{
            //    pay = "1";
            //}
            if (rdb_yearly.Checked == true)
            {
                pay = "2";
            }
            else if (rdb_sem.Checked == true)
            {
                pay = "3";
            }
            string date = "";
            date = Convert.ToString(txt_fromdate.Text);
            string[] splitdate = date.Split('-');
            splitdate = splitdate[0].Split('/');
            DateTime dt = new DateTime();
            if (splitdate.Length > 0)
            {
                dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            }
            string getday = "";
            getday = dt.ToString("MM/dd/yyyy");

            string todate = "";
            todate = Convert.ToString(txt_todate.Text);
            string[] splittodate = todate.Split('-');
            splittodate = splittodate[0].Split('/');
            DateTime dttodate = new DateTime();
            if (splittodate.Length > 0)
            {
                dttodate = Convert.ToDateTime(splittodate[1] + "/" + splittodate[0] + "/" + splittodate[2]);
            }
            string getday1 = "";
            getday1 = dttodate.ToString("MM/dd/yyyy");

            string duedate = "";
            duedate = Convert.ToString(txt_duedate.Text);
            string[] splitduedate = duedate.Split('-');
            splitduedate = splitduedate[0].Split('/');
            DateTime dtduedate = new DateTime();
            if (splitduedate.Length > 0)
            {
                dtduedate = Convert.ToDateTime(splitduedate[1] + "/" + splitduedate[0] + "/" + splitduedate[2]);
            }
            string getday2 = "";
            getday2 = dtduedate.ToString("MM/dd/yyyy");

            string dtaccessdate = "";
            dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = "";
            dtaccesstime = DateTime.Now.ToLongTimeString();
            string cost = "";
            cost = Convert.ToString(txt_cost.Text);

            string activerow = "";
            string activecol = "";
            activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
            string roomcostcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);

            string query = "if exists(select * from RoomCost_Master where RoomCost_Code='" + roomcostcode + "')update RoomCost_Master set Access_Date='" + dtaccessdate + "',Access_Time='" + dtaccesstime + "',Room_Type='" + roomtype + "',From_Date='" + getday + "',To_Date='" + getday1 + "', Rent_Type='" + pay + "',Room_Cost='" + cost + "', Due_Date='" + getday2 + "',college_code='" + clgcode + "' where RoomCost_Code='" + roomcostcode + "' else insert into RoomCost_Master (Access_Date,Access_Time,Room_Type,From_Date,To_Date,Rent_Type,Room_Cost,Due_Date,college_code) values ('" + dtaccessdate + "','" + dtaccesstime + "','" + roomtype + "','" + getday + "','" + getday1 + "','" + pay + "','" + cost + "','" + getday2 + "','" + clgcode + "')";

            int iv = d2.update_method_wo_parameter(query, "Text");
            if (iv != 0)
            {
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Updated Successfully";
                loadroomtype();
                loadcollege();
                poperrjs.Visible = false;
                btn_go_Click(sender, e);

            }


        }
        catch
        {

        }
    }
    protected void btn_delete_Click(object sender, EventArgs e)
    {
        try
        {
            if (btn_delete.Text == "Delete")
            {
                surediv.Visible = true;
                lbl_sure.Text = "Do you want to Delete this Record?";

            }
        }
        catch
        {

        }
    }
    protected void btn_exit_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;

    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    protected void btn_errclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void txt_cost_Textchanged(object sender, EventArgs e)
    {
        try
        {

            if (Convert.ToDouble(txt_cost.Text) > 0)
            {
                //btn_save_Click(sender, e);
            }
            else
            {
                txt_cost.Text = "";
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Please Enter Valid Amount";
            }

        }
        catch
        {
        }
    }

    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
        btn_go_Click(sender, e);

        poperrjs.Visible = false;
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        imgdiv2.Visible = false;
        poperrjs.Visible = true;
    }

    public void delete()
    {
        try
        {
            surediv.Visible = false;
            string activerow = "";
            string activecol = "";
            activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
            string roomcostcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
            string query = "delete from RoomCost_Master where RoomCost_Code ='" + roomcostcode + "' ";
            int iv = d2.update_method_wo_parameter(query, "Text");
            if (iv != 0)
            {
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Deleted Successfully";
                loadroomtype();
                loadcollege();
                poperrjs.Visible = false;
            }
        }
        catch
        {
        }
    }
}