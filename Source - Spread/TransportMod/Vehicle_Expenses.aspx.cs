using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Configuration;

public partial class Vehicle_Expenses : System.Web.UI.Page
{
    string user_code;
    string singleuser = "";
    string group_user = "";
    string college_code = "";
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    Hashtable ht = new Hashtable();
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

        college_code = Session["collegecode"].ToString();
        lblerr.Visible = false;
        if (!IsPostBack)
        {
            setLabelText();
            //txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //txtfromdate.Attributes.Add("ReadOnly", "ReadOnly");
            //txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //txttodate.Attributes.Add("ReadOnly", "ReadOnly");
            college();

            fromyear();
            //lblerr.Visible = false;
            Toyear();
            lblError.Visible = false;
            //lblrptname.Visible = false;
            //btnxl.Visible = false;
            //txtexcelname.Visible = false;
            //btnprintmaster.Visible = false;
            txtcollege.Text = "---Select---";
        }
    }
    public void college()
    {
        try
        {

            string college = "select college_code,Coll_acronymn from collinfo ";
            if (college != "")
            {
                ds = da.select_method(college, ht, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cheklist_clg.DataSource = ds;
                    cheklist_clg.DataTextField = "Coll_acronymn";
                    cheklist_clg.DataValueField = "college_code";
                    cheklist_clg.DataBind();
                }
            }
        }
        catch (Exception ex)
        { }
    }

    public void fromyear()
    {
        string strfromyear = "select distinct DATEPART (YEAR,date) as year from Vehicle_Usage where date is  not null ";
        ds = da.select_method(strfromyear, ht, "Text");
        ddlfromyear.DataSource = ds;
        ddlfromyear.DataValueField = "year";
        ddlfromyear.DataTextField = "year";
        ddlfromyear.DataBind();
    }
    public void Toyear()
    {
        string strfromyear = "select distinct DATEPART (YEAR,date) as year from Vehicle_Usage where date is  not null ";
        ds = da.select_method(strfromyear, ht, "Text");
        ddltoyear.DataSource = ds;
        ddltoyear.DataValueField = "year";
        ddltoyear.DataTextField = "year";
        ddltoyear.DataBind();
    }

    public void vehicle()
    {
        bool flag = false;
        try
        {
            if (txtcollege.Text != "---Select---")
            {


                Fpvehicle.Visible = true;
                Fpvehicle.Sheets[0].RowCount = 0;
                Fpvehicle.Sheets[0].RowHeader.Visible = false;
                Fpvehicle.Sheets[0].AutoPostBack = true;
                // Fpvehicle.Height = 500;
                //Fpvehicle.Width = 950;
                Fpvehicle.Sheets[0].Columns[1].Width = 150;
                Fpvehicle.Sheets[0].ColumnCount = 3;
                Fpvehicle.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
                Fpvehicle.Sheets[0].ColumnHeader.Columns[0].Font.Size = FontUnit.Medium;
                Fpvehicle.Sheets[0].ColumnHeader.Columns[0].Font.Bold = true;
                Fpvehicle.Sheets[0].ColumnHeader.Columns[0].Font.Name = "Book Antiqua";
                Fpvehicle.Sheets[0].ColumnHeader.Columns[1].Label = " Institution";
                Fpvehicle.Sheets[0].ColumnHeader.Columns[1].Width = 350;
                Fpvehicle.Sheets[0].ColumnHeader.Columns[1].Font.Size = FontUnit.Medium;
                Fpvehicle.Sheets[0].ColumnHeader.Columns[1].Font.Bold = true;
                Fpvehicle.Sheets[0].ColumnHeader.Columns[1].Font.Name = "Book Antiqua";
                Fpvehicle.Sheets[0].ColumnHeader.Columns[2].Label = "  ";
                Fpvehicle.Sheets[0].RowCount = 1;
                Fpvehicle.Sheets[0].Columns[2].Width = 150;
                string firstdate = ddlfrommonth.SelectedItem.Value.ToString() + "/" + ddlfromyear.SelectedItem.Text;
                //string[] split = firstdate.Split(new Char[] { '/' });
                //string date = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
                string todate = ddltomonth.SelectedItem.Value.ToString() + "/" + ddltoyear.SelectedItem.Text;
                //string[] split1 = todate.Split(new Char[] { '/' });
                //string date1 = split1[1].ToString() + "-" + split1[0].ToString() + "-" + split1[2].ToString();
                ArrayList al = new ArrayList();
                ArrayList al2 = new ArrayList();
                ArrayList al3 = new ArrayList();
                DateTime dt1 = Convert.ToDateTime(firstdate.ToString());
                DateTime dt2 = Convert.ToDateTime(todate.ToString());
                int months = (dt2.Year - dt1.Year) * 12 + dt2.Month - dt1.Month;
                for (int i = 0; i < months + 1; i++)
                {
                    string month = dt1.ToString("MMM");
                    al.Add(month.ToUpper());
                    al2.Add(dt1.ToString("MM"));
                    al3.Add(dt1.ToString("yyyy"));
                    dt1 = dt1.AddMonths(1);
                }
                for (int j = 0; j < al.Count; j++)
                {

                    Fpvehicle.Sheets[0].ColumnHeader.Columns.Count++;
                    Fpvehicle.Sheets[0].ColumnHeader.Cells[0, Fpvehicle.Sheets[0].ColumnHeader.Columns.Count - 1].Text = al[j].ToString();
                    Fpvehicle.Sheets[0].ColumnHeader.Cells[0, Fpvehicle.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Bold = true;
                    Fpvehicle.Sheets[0].ColumnHeader.Cells[0, Fpvehicle.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Size = FontUnit.Medium;
                    Fpvehicle.Sheets[0].ColumnHeader.Cells[0, Fpvehicle.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Name = "Book Antiqua";

                }

                Fpvehicle.Sheets[0].ColumnHeader.Columns.Count += 1;
                Fpvehicle.Sheets[0].ColumnHeader.Cells[0, Fpvehicle.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "TOTAL";
                Fpvehicle.Sheets[0].ColumnHeader.Cells[0, Fpvehicle.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Bold = true;
                Fpvehicle.Sheets[0].ColumnHeader.Cells[0, Fpvehicle.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Size = FontUnit.Medium;
                Fpvehicle.Sheets[0].ColumnHeader.Cells[0, Fpvehicle.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Name = "Book Antiqua";

                int sno = 0;
                for (int a = 0; a < cheklist_clg.Items.Count; a++)
                {
                    string data = "";
                    string check = "";
                    if (cheklist_clg.Items[a].Selected == true)
                    {


                        string collname = "";
                        collname = cheklist_clg.Items[a].Text;
                        int b = 2;
                        Boolean che = false;
                     
                        for (int k = 0; k < al2.Count; k++)
                        {

                           
                            b++;
                            string v = al2[k].ToString();
                            v = v.TrimStart('0');
                            string fromdate = ddlfromyear.Text + "/" + ddlfrommonth.Text + "/" + "01";
                            string toodate = ddltoyear.Text + "/" + ddltomonth.Text + "/" + "01";
                            string strquery = "select sum(Fuel) as Fuel,(sum(case when totalamount is null then 0 else totalamount end)) as totalamount,month(Date)as mon,year(Date)as years from Vehicle_Usage u,Vehicle_Master m where u.Vehicle_Id=m.Veh_ID and m.college_code='" + cheklist_clg.Items[a].Value + "' and month(date)='" + al2[k].ToString() + "'and year(date)='" + al3[k].ToString() + "' group by month(Date),year(Date)";
                            ds.Clear();
                            ds = da.select_method_wo_parameter(strquery, "Text");

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                //int b = 0;


                                if (che == false)
                                {
                                    Fpvehicle.Sheets[0].RowCount++;
                                }

                              
                                int ac = 0;
                                if (che == false)
                                {
                                    sno++;
                                }
                                che = true;
                                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 0].Text = sno.ToString();
                                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 0].Font.Bold = true;
                                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 0].Font.Name = "Book Antiqua";


                                flag = true;
                                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 1].Text = collname;
                                //Fpvehicle.Sheets[0].RowCount++;
                                //Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 2].Text = "Value";

                                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 1].HorizontalAlign = HorizontalAlign.Center;
                                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 1].Font.Bold = true;
                                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 1].Font.Name = "Book Antiqua";
                                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 1].Font.Size = FontUnit.Medium;
                                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 2].Text = "Value";
                                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 2].HorizontalAlign = HorizontalAlign.Left;
                                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 2].Font.Bold = true;
                                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 2].Font.Size = FontUnit.Medium;
                                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 2].Font.Name = "Book Antiqua";
                                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 2].Text = "Qty";
                                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                data = ds.Tables[0].Rows[ac]["totalamount"].ToString();
                                check = ds.Tables[0].Rows[ac]["Fuel"].ToString();
                                if (data == "")
                                {
                                    Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, b].Text = "-";
                                    Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, b].Font.Bold = true;
                                    Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, b].Font.Name = "Book Antiqua";
                                    Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, b].HorizontalAlign = HorizontalAlign.Center;
                                }
                                else
                                {
                                    Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, b].Text = data;
                                    Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, b].Font.Bold = true;
                                    Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, b].Font.Name = "Book Antiqua";
                                    Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, b].HorizontalAlign = HorizontalAlign.Right;
                                    Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, b].Font.Size = FontUnit.Medium;
                                }

                                //Fpvehicle.Sheets[0].RowCount++;
                                if (check == "")
                                {
                                    Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, b].Text = "-";
                                    Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, b].Font.Bold = true;
                                    Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, b].Font.Name = "Book Antiqua";
                                    Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, b].HorizontalAlign = HorizontalAlign.Center;
                                    Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, b].Font.Size = FontUnit.Small;
                                }
                                else
                                {
                                    Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, b].Text = check;
                                    Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, b].Font.Bold = true;
                                    Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, b].Font.Name = "Book Antiqua";
                                    Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, b].HorizontalAlign = HorizontalAlign.Right;
                                    Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, b].Font.Size = 10;

                                }
                                ac++;
                                //Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, a].Font.Bold = true;
                                //Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, a].Font.Size = FontUnit.Medium;
                                //Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, a].Font.Name = "Book Antiqua";
                                //Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, a].HorizontalAlign = HorizontalAlign.Right;


                            }

                        }

                    }







                    //  
                    //}
                    if (data != "" && check != "")
                    {
                        double totalamt = 0;
                        for (int h = 3; h < Fpvehicle.Sheets[0].Columns.Count; h++)
                        {



                            string firstvalue = Convert.ToString(Fpvehicle.Sheets[0].GetText(Fpvehicle.Sheets[0].RowCount - 2, h));
                            if (firstvalue != "0" && firstvalue != "-")
                            {
                                if (totalamt == 0)
                                {
                                    totalamt = Convert.ToDouble(firstvalue);

                                }
                                else
                                {
                                    totalamt = totalamt + Convert.ToDouble(firstvalue);
                                }
                            }


                        }


                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, Fpvehicle.Sheets[0].Columns.Count - 1].Text = totalamt.ToString();
                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, Fpvehicle.Sheets[0].Columns.Count - 1].Font.Bold = true;
                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, Fpvehicle.Sheets[0].Columns.Count - 1].Font.Name = "Book Antiqua";
                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, Fpvehicle.Sheets[0].Columns.Count - 1].Font.Size = FontUnit.Medium;
                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, Fpvehicle.Sheets[0].Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;

                        int totalamt1 = 0;


                        for (int h = 3; h < Fpvehicle.Sheets[0].Columns.Count; h++)
                        {

                            string firstvalue = Convert.ToString(Fpvehicle.Sheets[0].GetText(Fpvehicle.Sheets[0].RowCount - 1, h));
                            if (firstvalue != "0" && firstvalue != "-")
                            {
                                if (totalamt1 == 0)
                                {
                                    totalamt1 = Convert.ToInt32(firstvalue);

                                }
                                else
                                {
                                    totalamt1 = totalamt1 + Convert.ToInt32(firstvalue);
                                }
                            }



                        }


                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, Fpvehicle.Sheets[0].Columns.Count - 1].Text = totalamt1.ToString();
                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, Fpvehicle.Sheets[0].Columns.Count - 1].Font.Bold = true;
                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, Fpvehicle.Sheets[0].Columns.Count - 1].Font.Size = 10;
                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, Fpvehicle.Sheets[0].Columns.Count - 1].Font.Name = "Book Antiqua";
                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, Fpvehicle.Sheets[0].Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;




                        Fpvehicle.Sheets[0].RowCount++;

                    }

                    if (a == cheklist_clg.Items.Count - 1)
                    {

                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 1].Text = "TOTAL";
                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 2].Text = "Value";
                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpvehicle.Sheets[0].RowCount++;
                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 2].Text = "Qty";

                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        for (int h = 3; h < Fpvehicle.Sheets[0].Columns.Count; h++)
                        {

                            double totalamt1 = 0;
                            for (int j = 0; j < Fpvehicle.Sheets[0].RowCount - 1; j += 2)
                            {


                                string firstvalue = Convert.ToString(Fpvehicle.Sheets[0].GetText(j, h));
                                if (firstvalue != "0" && firstvalue != "-" && firstvalue != null)
                                {
                                    if (totalamt1 == 0)
                                    {
                                        totalamt1 = Convert.ToDouble(firstvalue);

                                    }
                                    else
                                    {
                                        totalamt1 = totalamt1 + Convert.ToDouble(firstvalue);
                                    }
                                }


                            }

                            Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, h].Text = totalamt1.ToString();
                            Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, h].Font.Bold = true;
                            Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, h].Font.Name = "Book Antiqua";
                            Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, h].HorizontalAlign = HorizontalAlign.Right;
                            Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, h].Font.Size = FontUnit.Medium;
                        }

                        for (int h = 3; h < Fpvehicle.Sheets[0].Columns.Count; h++)
                        {

                            double totalamt2 = 0;
                            for (int j = 1; j < Fpvehicle.Sheets[0].RowCount - 1; j += 2)
                            {



                                string firstvalue = Convert.ToString(Fpvehicle.Sheets[0].GetText(j, h));
                                if (firstvalue != "0" && firstvalue != "-")
                                {
                                    if (totalamt2 == 0)
                                    {
                                        totalamt2 = Convert.ToDouble(firstvalue);

                                    }
                                    else
                                    {
                                        totalamt2 = totalamt2 + Convert.ToDouble(firstvalue);
                                    }
                                }


                            }

                            Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, h].Text = totalamt2.ToString();
                            Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, h].Font.Bold = true;
                            Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, h].Font.Name = "Book Antiqua";
                            Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, h].Font.Size = 10;
                            Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 1, h].HorizontalAlign = HorizontalAlign.Right;
                        }
                    }
                }


                //}

                //Fpvehicle.Sheets[0].SpanModel.Add(0, 1, Fpvehicle.Sheets[0].RowCount - 2, 1);
                //Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 1].Text = "Total";
                //Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 1].Font.Bold = true;
                //Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 1].Font.Name = "Book Antiqua";
                //Fpvehicle.Sheets[0].Cells[Fpvehicle.Sheets[0].RowCount - 2, 1].HorizontalAlign = HorizontalAlign.Left;



                if (flag == true)
                {
                    Fpvehicle.Sheets[0].Visible = true;
                    Fpvehicle.Visible = true;
                    Fpvehicle.Sheets[0].PageSize = Fpvehicle.Sheets[0].RowCount;
                    lblError.Visible = false;
                    //if (flag == false)
                    //{
                    //    Fpvehicle.Visible = false;
                    //}

                    //lblrptname.Visible = true;
                    //btnxl.Visible = true;
                    //txtexcelname.Visible = true;
                    //btnprintmaster.Visible = true;
                }
                if (flag == false)
                {
                    Fpvehicle.Sheets[0].Visible = false;
                    Fpvehicle.Visible = false;
                    //  Fpvehicle.Sheets[0].PageSize = Fpvehicle.Sheets[0].RowCount;
                    lblError.Visible = true;
                    lblError.Text = "No Records Found";
                    lblerr.Visible = false;
                    //if (flag == false)
                    //{
                    //    Fpvehicle.Visible = false;
                    //}

                    //lblrptname.Visible = false;
                    //btnxl.Visible = false;
                    //txtexcelname.Visible = false;
                    //btnprintmaster.Visible = false;
                }



            }
            else
            {
                Fpvehicle.Sheets[0].Visible = false;
                Fpvehicle.Visible = false;
                //  Fpvehicle.Sheets[0].PageSize = Fpvehicle.Sheets[0].RowCount;
                lblError.Visible = true;
                lblError.Text = "Please Select Any One College";
                //if (flag == false)
                //{
                //    Fpvehicle.Visible = false;
                //}

                //lblrptname.Visible = false;
                //btnxl.Visible = false;
               // txtexcelname.Visible = false;
                //btnprintmaster.Visible = false;
            }
        }
        catch
        {
        }

    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        vehicle();
    }
    protected void logout_btn_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    //protected void txttodate_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DateTime dt1 = Convert.ToDateTime(txt);
    //        DateTime dt2 = Convert.ToDateTime(txttodate.Text);

    //        TimeSpan ts = dt2 - dt1;

    //        int days = ts.Days;
    //        if (days < 0)
    //        {
    //            lblError.Text = "From Date Should Be Less Than To Date";

    //            lblError.Visible = true;
    //            Fpvehicle.Visible = false;
    //            lblrptname.Visible = false;
    //            btnxl.Visible = false;
    //            txtexcelname.Visible = false;
    //            btnprintmaster.Visible = false;
    //        }
    //        if (dt1 > DateTime.Today)
    //        {
    //            lblError.Text = "You can not select From date  greater than today";

    //            lblError.Visible = true;
    //            Fpvehicle.Visible = false;
    //            lblrptname.Visible = false;
    //            btnxl.Visible = false;
    //            txtexcelname.Visible = false;
    //            btnprintmaster.Visible = false;
    //        }
    //        if (dt2 > DateTime.Today)
    //        {
    //            lblError.Text = "You can not select From date  greater than today";

    //            lblError.Visible = true;
    //            Fpvehicle.Visible = false;
    //            lblrptname.Visible = false;
    //            btnxl.Visible = false;
    //            txtexcelname.Visible = false;
    //            btnprintmaster.Visible = false;
    //        }
    //    }
    //    catch
    //    {
    //    }

    //}
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string departmentlist = "Vehicle Diesel Expenses Cumulative Report";
        Printcontrol.loadspreaddetails(Fpvehicle, "vehicle_expenses.aspx", departmentlist);
        Printcontrol.Visible = true;
    }
    protected void chekclg_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chekclg.Checked == true)
            {
                for (int i = 0; i < cheklist_clg.Items.Count; i++)
                {

                    cheklist_clg.Items[i].Selected = true;
                    txtcollege.Text = lblselectcollege.Text+"(" + (cheklist_clg.Items.Count) + ")";
                }

            }
            else
            {
                for (int i = 0; i < cheklist_clg.Items.Count; i++)
                {
                    cheklist_clg.Items[i].Selected = false;
                    txtcollege.Text = "---Select---";
                }
            }
        }
        catch
        {
        }
    }
    protected void cheklist_clg_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int seatcount = 0;

            chekclg.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cheklist_clg.Items.Count; i++)
            {
                if (cheklist_clg.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    build = cheklist_clg.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;

                    }

                }


            }

            if (seatcount == cheklist_clg.Items.Count)
            {
                txtcollege.Text = lblselectcollege.Text+"(" + seatcount.ToString() + ")";
                chekclg.Checked = true;
            }
            else if (seatcount == 0)
            {
                txtcollege.Text = "---Select---";
            }
            else
            {
                txtcollege.Text = lblselectcollege.Text + "(" + seatcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txtexcelname.Text;
            if (report.ToString().Trim() != "")
            {
                da.printexcelreport(Fpvehicle, report);
                lblerr.Visible = false;
            }
            else
            {
                lblerr.Text = "Please Enter Your Report Name";
                lblerr.Visible = true;
            }

        }
        catch (Exception ex)
        {
            throw ex;
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
        lbl.Add(lblselectcollege);
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
}