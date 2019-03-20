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

public partial class Vehicle_Details : System.Web.UI.Page
{
    public SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    string PageLogOut = "";
    string vech_values = string.Empty;
    InsproDirectAccess dirAcc = new InsproDirectAccess();
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
        txt_vech.Attributes.Add("readonly", "readonly");
        lbl_errmsg.Visible = false;

        Fp_Vehicle.Sheets[0].AutoPostBack = true;
        Fp_Vehicle.CommandBar.Visible = true;
        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        style.Font.Size = 10;
        style.Font.Bold = true;
        Fp_Vehicle.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        Fp_Vehicle.Sheets[0].AllowTableCorner = true;
        Fp_Vehicle.Sheets[0].RowHeader.Visible = false;

        Fp_Vehicle.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;

        Fp_Vehicle.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        Fp_Vehicle.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

        Fp_Vehicle.Sheets[0].DefaultColumnWidth = 50;
        Fp_Vehicle.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        Fp_Vehicle.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        Fp_Vehicle.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

        Fp_Vehicle.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        Fp_Vehicle.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        Fp_Vehicle.Sheets[0].DefaultStyle.Font.Bold = false;
        Fp_Vehicle.SheetCorner.Cells[0, 0].Font.Bold = true;

        if (!IsPostBack)
        {
            con.Close();
            con.Open();

            //SqlCommand cmd_vehicle_id = new SqlCommand("select distinct Veh_ID from vehicle_master order by Veh_ID", con);
            //SqlDataReader rdr_vehicle_id = cmd_vehicle_id.ExecuteReader();

            //int incre_veh = 0;
            //while (rdr_vehicle_id.Read())
            //{
            //    if (rdr_vehicle_id.HasRows == true)
            //    {
            //        incre_veh++;
            //        System.Web.UI.WebControls.ListItem list_vehicle_id = new System.Web.UI.WebControls.ListItem();

            //        list_vehicle_id.Text = (rdr_vehicle_id["Veh_ID"].ToString());

            //        vehiclechecklist.Items.Add(list_vehicle_id);
            //        vehiclechecklist.Items[incre_veh - 1].Selected = true;

            //    }
            //}
            string sqlvehidqry = "select * from vehicle_master order by len(Veh_ID), Veh_ID";
            DataTable dt = dirAcc.selectDataTable(sqlvehidqry);
            if (dt.Rows.Count > 0)
            {
                vehiclechecklist.DataSource = dt;
                vehiclechecklist.DataTextField = "Veh_ID";
                vehiclechecklist.DataValueField = "Veh_ID";
                vehiclechecklist.DataBind();
            }
            for (int i = 0; i < vehiclechecklist.Items.Count; i++)
            {
                vehiclechecklist.Items[i].Selected = true;
            }
            vehiclechecklist_SelectedIndexChanged(sender, e);
            btnMainGo_Click(sender, e);
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

    }

    protected void btnMainGo_Click(object sender, EventArgs e)
    {
        string vech_all = string.Empty;

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

        if (vech_all == "")
        {
            lbl_errmsg.Visible = true;
            lbl_errmsg.Text = "Please select any vehicle then proceed.";
            Fp_Vehicle.Visible = false;
            return;
        }

        con.Close();
        con.Open();

        //modified by prabha on jan 04 2018
        //SqlCommand cmd_get_veh = new SqlCommand("select veh_id,veh_type,permit_date,insurance_date,nextins_date,fc_date,nextfcdate,Permit_Type from vehicle_insurance where veh_id in('" + vech_all + "') order by veh_id,veh_type,permit_date,nextins_date,nextfcdate", con);
        SqlCommand cmd_get_veh = new SqlCommand("select vi.*,v.Reg_No,Permit_Type from vehicle_insurance vi,Vehicle_Master v where v.Veh_ID=vi.Veh_ID and vi.veh_id in('" + vech_all + "') order by len(vi.veh_id),vi.Veh_ID ", con);  //,vi.veh_type,vi.permit_date,vi.nextins_date,vi.nextfcdate 
        SqlDataAdapter ad_get_veh = new SqlDataAdapter(cmd_get_veh);
        DataTable dt_get_veh = new DataTable();
        ad_get_veh.Fill(dt_get_veh);

        if (dt_get_veh.Rows.Count > 0)
        {
            Header_Set();

            string temp_veh = string.Empty;
            Fp_Vehicle.Sheets[0].RowCount = 0;

            for (int i = 0; i < dt_get_veh.Rows.Count; i++)
            {
                string veh_id = dt_get_veh.Rows[i]["veh_id"].ToString();
                string veh_type = dt_get_veh.Rows[i]["veh_type"].ToString();
                string regno = dt_get_veh.Rows[i]["Reg_No"].ToString();
                if (veh_id != temp_veh)
                {
                    Fp_Vehicle.Sheets[0].RowCount++;

                    Fp_Vehicle.Sheets[0].Cells[Fp_Vehicle.Sheets[0].RowCount - 1, 0].Text = Fp_Vehicle.Sheets[0].RowCount.ToString();
                    Fp_Vehicle.Sheets[0].Cells[Fp_Vehicle.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fp_Vehicle.Sheets[0].Cells[Fp_Vehicle.Sheets[0].RowCount - 1, 1].Text = veh_type;
                    Fp_Vehicle.Sheets[0].Cells[Fp_Vehicle.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fp_Vehicle.Sheets[0].Cells[Fp_Vehicle.Sheets[0].RowCount - 1, 2].Text = veh_id;
                    Fp_Vehicle.Sheets[0].Cells[Fp_Vehicle.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fp_Vehicle.Sheets[0].Cells[Fp_Vehicle.Sheets[0].RowCount - 1, 3].Text = regno;
                    Fp_Vehicle.Sheets[0].Cells[Fp_Vehicle.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    //Fp_Vehicle.Sheets[0].Cells[Fp_Vehicle.Sheets[0].RowCount - 1, 8].Text = Permit_Type_Details;

                    for (int j = 2; j < dt_get_veh.Columns.Count; j++)
                    {
                        string col_text = string.Empty;
                        string col_text1 = string.Empty;
                        string col_text_Dis = string.Empty;

                        if (j == 2)
                        {
                            col_text = "Permit_Type";
                        }
                        else if (j == 3)
                        {
                            col_text = "Permit_Type";
                        }
                        else if (j == 4)
                        {
                            col_text = "insurance_date";
                            col_text1 = "nextins_date";
                        }
                        else if (j == 6)
                        {
                            col_text = "fc_date";
                            col_text1 = "nextfcdate";
                        }
                        else if (j == 8)
                        {
                            col_text_Dis = "Permit_Type";
                        }

                        DataView dv_get_veh = new DataView();
                        if (col_text != "")
                        {
                            if (col_text_Dis == "" && col_text != "")
                            {
                                if (col_text == "Permit_Type")
                                {
                                    dt_get_veh.DefaultView.RowFilter = "veh_id='" + veh_id + "' and " + col_text + " is not null and " + col_text + " <> ''";
                                }
                                else
                                {
                                    dt_get_veh.DefaultView.RowFilter = "veh_id='" + veh_id + "' and " + col_text + " is not null";
                                }

                            }
                            else
                            {
                                dt_get_veh.DefaultView.RowFilter = "veh_id='" + veh_id + "' and " + col_text_Dis + " is not null and " + col_text_Dis + " <> ''";
                            }

                            dv_get_veh = dt_get_veh.DefaultView;

                            if (dv_get_veh.Count > 0)
                            {
                                string re_date = "";
                                if (col_text_Dis == "")
                                {
                                    re_date = dv_get_veh[dv_get_veh.Count - 1][col_text].ToString();
                                }
                                else
                                {
                                    re_date = dv_get_veh[dv_get_veh.Count - 1][col_text_Dis].ToString();
                                }


                                if (j == 2)
                                {
                                    //Find Max Date=======================
                                    string permit_max_date = string.Empty;
                                    permit_max_date = "select max(Permit_Date) as PermitDate from Vehicle_Insurance where Veh_id='" + veh_id + "' and Veh_Type = '" + veh_type + "' and Permit_Type is not null and Permit_Type <>'' and Permit_Type='Standard'";
                                    SqlDataAdapter dr_date = new SqlDataAdapter(permit_max_date, con);
                                    DataTable dt_date = new DataTable();
                                    dr_date.Fill(dt_date);
                                    if (dt_date.Rows.Count > 0)
                                    {
                                        if (dt_date.Rows[0]["PermitDate"].ToString() != "")
                                        {
                                            DateTime date = Convert.ToDateTime(dt_date.Rows[0]["PermitDate"].ToString());
                                            re_date = date.ToString("dd-MM-yyyy");
                                        }
                                        else
                                        {
                                            re_date = "";
                                        }
                                    }


                                }
                                if (j == 3)//Chnged to 3 to 4 by srinath 12/6/2014
                                {
                                    //Find Max Date=======================
                                    string permit_max_date = string.Empty;
                                    permit_max_date = "select max(Permit_Date) as PermitDate from Vehicle_Insurance where Veh_id='" + veh_id + "' and Veh_Type = '" + veh_type + "' and Permit_Type is not null and Permit_Type <>'' and Permit_Type='District'";
                                    SqlDataAdapter dr_date = new SqlDataAdapter(permit_max_date, con);
                                    DataTable dt_date = new DataTable();
                                    dr_date.Fill(dt_date);
                                    if (dt_date.Rows.Count > 0)
                                    {
                                        if (dt_date.Rows[0]["PermitDate"].ToString() != "")
                                        {
                                            DateTime date = Convert.ToDateTime(dt_date.Rows[0]["PermitDate"].ToString());
                                            re_date = date.ToString("dd-MM-yyyy");
                                        }
                                        else
                                        {
                                            re_date = "";
                                        }
                                    }
                                }
                                int col = j + 1;//Added by srinath 12/6/2014
                                Fp_Vehicle.Sheets[0].Cells[Fp_Vehicle.Sheets[0].RowCount - 1, col + 1].Text = re_date;
                                Fp_Vehicle.Sheets[0].Cells[Fp_Vehicle.Sheets[0].RowCount - 1, col + 1].HorizontalAlign = HorizontalAlign.Left;

                                if (j != 2 && j != 3)
                                {
                                    if (col_text1 != "")
                                    {
                                        if (col_text == "Permit_Type")
                                        {
                                            //Find Max Date=======================
                                            string permit_max_date = string.Empty;
                                            permit_max_date = "select max(Permit_Date) as PermitDate from Vehicle_Insurance where Veh_id='" + veh_id + "' and Veh_Type = '" + veh_type + "' and Permit_Type is not null and Permit_Type <>'' and Permit_Type='" + re_date + "'";
                                            SqlDataAdapter dr_date = new SqlDataAdapter(permit_max_date, con);
                                            DataTable dt_date = new DataTable();
                                            dr_date.Fill(dt_date);
                                            if (dt_date.Rows.Count > 0)
                                            {
                                                if (dt_date.Rows[0]["PermitDate"].ToString() != "")
                                                {
                                                    DateTime date = Convert.ToDateTime(dt_date.Rows[0]["PermitDate"].ToString());
                                                    re_date = date.ToString("dd-MM-yyyy");
                                                }
                                                else
                                                {
                                                    re_date = "";
                                                }

                                            }
                                        }
                                        else
                                        {
                                            //modified on 30 jan 2018 by prabha 
                                            if (col_text1 == "nextins_date")
                                            {
                                                dt_get_veh.DefaultView.RowFilter = "veh_id='" + veh_id + "' and Insurance_Date='" + re_date + "'";
                                                DataView dvinsu = dt_get_veh.DefaultView;
                                                dvinsu.Sort = "nextins_date asc";
                                                DateTime date = Convert.ToDateTime(dv_get_veh[dvinsu.Count - 1][col_text1].ToString());
                                                re_date = date.ToString("dd-MM-yyyy");
                                            }
                                            else
                                            {
                                                DateTime date = Convert.ToDateTime(dv_get_veh[dv_get_veh.Count - 1][col_text1].ToString());
                                                re_date = date.ToString("dd-MM-yyyy");
                                            }
                                        }

                                    }
                                    col = j + 1;//Added by srinath 12/6/2014
                                    Fp_Vehicle.Sheets[0].Cells[Fp_Vehicle.Sheets[0].RowCount - 1, col + 2].Text = re_date;
                                    Fp_Vehicle.Sheets[0].Cells[Fp_Vehicle.Sheets[0].RowCount - 1, col + 2].HorizontalAlign = HorizontalAlign.Left;
                                }

                            }
                        }

                        if (j != 2 && j != 3)
                        {
                            j++;
                        }
                    }
                }

                temp_veh = veh_id;

                Fp_Vehicle.Sheets[0].PageSize = Fp_Vehicle.Sheets[0].RowCount;
                Fp_Vehicle.Visible = true;
                lbl_errmsg.Visible = false;
            }
        }
        else
        {
            lbl_errmsg.Visible = true;
            lbl_errmsg.Text = "No records found.";
            Fp_Vehicle.Visible = false;
            return;
        }

        if (Fp_Vehicle.Sheets[0].RowCount > 0)
        {
            Button1.Visible = true;
        }
        else
        {
            Button1.Visible = false;
        }

    }

    void Header_Set()
    {
        Fp_Vehicle.Sheets[0].ColumnHeader.RowCount = 2;
        Fp_Vehicle.Sheets[0].ColumnCount = 11;

        Fp_Vehicle.Sheets[0].ColumnHeader.Cells[Fp_Vehicle.Sheets[0].ColumnHeader.RowCount - 2, 0].Text = "Sl.No";
        Fp_Vehicle.Sheets[0].ColumnHeader.Cells[Fp_Vehicle.Sheets[0].ColumnHeader.RowCount - 2, 1].Text = "Vehicle Type";
        Fp_Vehicle.Sheets[0].ColumnHeader.Cells[Fp_Vehicle.Sheets[0].ColumnHeader.RowCount - 2, 2].Text = "Vehicle Id";
        Fp_Vehicle.Sheets[0].ColumnHeader.Cells[Fp_Vehicle.Sheets[0].ColumnHeader.RowCount - 2, 4].Text = "Permit Date";
        Fp_Vehicle.Sheets[0].ColumnHeader.Cells[Fp_Vehicle.Sheets[0].ColumnHeader.RowCount - 2, 3].Text = "Registration No";
        //Fp_Vehicle.Sheets[0].ColumnHeader.Cells[Fp_Vehicle.Sheets[0].ColumnHeader.RowCount - 2, 4].Text = "District Date";
        Fp_Vehicle.Sheets[0].ColumnHeader.Cells[Fp_Vehicle.Sheets[0].ColumnHeader.RowCount - 2, 6].Text = "Insurance Date";
        Fp_Vehicle.Sheets[0].ColumnHeader.Cells[Fp_Vehicle.Sheets[0].ColumnHeader.RowCount - 2, 8].Text = "Fc Date";
        Fp_Vehicle.Sheets[0].ColumnHeader.Cells[Fp_Vehicle.Sheets[0].ColumnHeader.RowCount - 2, 10].Text = "Permit Type";
        Fp_Vehicle.Sheets[0].Columns[10].Visible = false;

        Fp_Vehicle.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 1, 2);
        Fp_Vehicle.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
        Fp_Vehicle.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 1, 2);
        Fp_Vehicle.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 1, 2);

        Fp_Vehicle.Sheets[0].ColumnHeader.Cells[Fp_Vehicle.Sheets[0].ColumnHeader.RowCount - 1, 4].Text = "Standard Date";
        Fp_Vehicle.Sheets[0].ColumnHeader.Cells[Fp_Vehicle.Sheets[0].ColumnHeader.RowCount - 1, 5].Text = "District Date";
        Fp_Vehicle.Sheets[0].ColumnHeader.Cells[Fp_Vehicle.Sheets[0].ColumnHeader.RowCount - 1, 6].Text = "Last ";
        Fp_Vehicle.Sheets[0].ColumnHeader.Cells[Fp_Vehicle.Sheets[0].ColumnHeader.RowCount - 1, 7].Text = "Next";
        Fp_Vehicle.Sheets[0].ColumnHeader.Cells[Fp_Vehicle.Sheets[0].ColumnHeader.RowCount - 1, 8].Text = "Last ";
        Fp_Vehicle.Sheets[0].ColumnHeader.Cells[Fp_Vehicle.Sheets[0].ColumnHeader.RowCount - 1, 9].Text = "Next";

        Fp_Vehicle.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
        Fp_Vehicle.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
        Fp_Vehicle.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
        Fp_Vehicle.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 2, 1);
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string degreedetails = string.Empty;
        string pagename = "Detailed Vehicles Report";
        Session["column_header_row_count"] = Fp_Vehicle.ColumnHeader.RowCount;

        Printcontrol.loadspreaddetails(Fp_Vehicle, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

}