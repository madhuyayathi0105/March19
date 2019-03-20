using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Configuration;
public partial class logindetails : System.Web.UI.Page
{
    string usercode = "";
    string singleuser = "";
    string group_user = "";
    string collegecode = "";
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        lblerr.Visible = false;
        if (!IsPostBack)
        {
            //string PageLogOut = "";
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
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode = Session["Collegecode"].ToString();
            tbstart_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            tbend_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            tbstart_date.Attributes.Add("Readonly", "Readonly");
            tbend_date.Attributes.Add("Readonly", "Readonly");
            lgn_usr.Items.Add("Admin");
            lgn_usr.Items.Add("Student");
            lgn_usr.Items.Add("Staff");
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
    protected void btn_go_click(object sender, EventArgs e)
    {
        try
        {
            DataSet countds = new DataSet();
            lblerr.Text = "";
            ds.Clear();
            string sql = "";
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            ArrayList arr_staf = new ArrayList();
            dt.Columns.Add("SNo", typeof(string));
            dt.Columns.Add("User Name", typeof(string));

            dt.Columns.Add("Login Date", typeof(string));
            dt.Columns.Add("Login Used Count", typeof(string));
            dt.Columns.Add("staff_code", typeof(string));
            dt1.Columns.Add("S.No", typeof(int));
            dt1.Columns.Add("Date", typeof(string));
            dt1.Columns.Add("Count", typeof(string));
            string fdtime = "";
            string tdtime = "";
            string dtime = DateTime.Now.ToString("HH:mm:ss");
            string firstdate = Convert.ToString(tbstart_date.Text);
            string seconddate = Convert.ToString(tbend_date.Text);
            string[] splitdate = firstdate.Split('/');
            string[] splitdate2 = seconddate.Split('/');
            fdtime = splitdate[2].ToString() + "-" + splitdate[1].ToString() + "-" + splitdate[0].ToString() + " " + "00:00:00";
            tdtime = splitdate2[2].ToString() + "-" + splitdate2[1].ToString() + "-" + splitdate2[0].ToString() + " " + dtime;

            if (ddluser.Items[0].Selected == true)
            {
                sql = "SELECT DISTINCT ld.staff_code, count(ld.staff_code) AS count , CONVERT(VARCHAR, cast(ld.dateandtime as date), 101) as  date,um.Full_Name,um.Description,ld.flag FROM logindetails ld, UserMaster um where ld.staff_code=um.User_id and ld.dateandtime between '" + fdtime + "' and '" + tdtime + "' and ld.flag='0' GROUP BY ld.staff_code,cast(dateandtime as date),um.Full_Name,um.Description,flag  ";
            }
            if (ddluser.Items[1].Selected == true)
            {
                if (sql == "")
                {
                    sql = "SELECT DISTINCT ld.staff_code, count(ld.staff_code) AS count , CONVERT(VARCHAR, cast(ld.dateandtime as date), 101) as  date,um.Full_Name,um.Description,ld.flag FROM logindetails ld, UserMaster um where ld.staff_code=um.User_id and ld.dateandtime between '" + fdtime + "' and '" + tdtime + "' and um.is_staff='1' and ld.flag='1'  GROUP BY ld.staff_code,cast(dateandtime as date),um.Full_Name,um.Description,flag";
                }
                else
                {
                    sql = sql + " union SELECT DISTINCT ld.staff_code, count(ld.staff_code) AS count , CONVERT(VARCHAR, cast(ld.dateandtime as date), 101) as  date,um.Full_Name,um.Description,ld.flag FROM logindetails ld, UserMaster um where ld.staff_code=um.User_id and ld.dateandtime between '" + fdtime + "' and '" + tdtime + "' and um.is_staff='1' and ld.flag='1'  GROUP BY ld.staff_code,cast(dateandtime as date),um.Full_Name,um.Description,flag ";
                }
            }
            if (ddluser.Items[2].Selected == true)
            {
                if (sql == "")
                {
                    sql = "SELECT DISTINCT ld.staff_code, count(ld.staff_code) AS count , CONVERT(VARCHAR, cast(ld.dateandtime as date), 101) as  date,um.Stud_Name as Full_Name,'' Description,ld.flag FROM logindetails ld, registration um where ld.staff_code=um.Roll_No and ld.dateandtime between '" + fdtime + "' and '" + tdtime + "' and ld.flag='2' GROUP BY ld.staff_code,cast(dateandtime as date),um.Stud_Name,flag ";
                }
                else
                {
                    sql = sql + " union SELECT DISTINCT ld.staff_code, count(ld.staff_code) AS count , CONVERT(VARCHAR, cast(ld.dateandtime as date), 101) as  date,um.Stud_Name as Full_Name,'' Description,ld.flag FROM logindetails ld, registration um where ld.staff_code=um.Roll_No and ld.dateandtime between '" + fdtime + "' and '" + tdtime + "' and ld.flag='2' GROUP BY ld.staff_code,cast(dateandtime as date),um.Stud_Name,flag ";
                }
            }
            sql = sql + " order by flag desc;";
            sql = sql + ";" + "select  DISTINCT staff_code, count(staff_code) AS count , CONVERT(VARCHAR, cast(dateandtime as date), 101) as  date from logindetails where  dateandtime  between '" + fdtime + "' and '" + tdtime + "' GROUP BY staff_code, CONVERT(VARCHAR, cast(dateandtime as date), 101) order by staff_code";

            DataView dv_demand_data = new DataView();
            ds = da.select_method_wo_parameter(sql, "Text");
            int count = 0;
            int totalval = 0;
            string gridaddnewrow = "";
            for (int d = 0; d < ds.Tables[0].Rows.Count; d++)
            {
                ds.Tables[1].DefaultView.RowFilter = "staff_code='" + ds.Tables[0].Rows[d]["staff_code"].ToString() + "'";
                dv_demand_data = ds.Tables[1].DefaultView;
                int count4 = 0;
                count4 = dv_demand_data.Count;
                if (!arr_staf.Contains(ds.Tables[0].Rows[d]["staff_code"].ToString()))
                {
                    arr_staf.Add(ds.Tables[0].Rows[d]["staff_code"].ToString());
                    if ("admin" == ds.Tables[0].Rows[d]["staff_code"].ToString().Trim().ToLower())
                    {
                        DataRow dtrow = dt.NewRow();
                        dtrow[0] = "Admin";
                        dtrow[1] = "Admin";
                        dtrow[2] = "Admin";
                        dtrow[3] = "Admin";
                        dtrow[4] = "Admin";
                        dt.Rows.Add(dtrow);
                    }
                    else if ("1" == ds.Tables[0].Rows[d]["flag"].ToString().Trim().ToLower() && "admin" != ds.Tables[0].Rows[d]["staff_code"].ToString().Trim().ToLower() && gridaddnewrow == "")
                    {
                        DataRow dtrow = dt.NewRow();
                        gridaddnewrow = "1";
                        dtrow[0] = "Staff";
                        dtrow[1] = "Staff";
                        dtrow[2] = "Staff";
                        dtrow[3] = "Staff";
                        dtrow[4] = "Staff";

                        dt.Rows.Add(dtrow);
                        //goto rowaddskip;
                    }
                    else if ("2" == ds.Tables[0].Rows[d]["flag"].ToString().Trim().ToLower() && gridaddnewrow != ds.Tables[0].Rows[d]["staff_code"].ToString() && gridaddnewrow == "1")
                    {
                        DataRow dtrow = dt.NewRow();
                        gridaddnewrow = "2";
                        dtrow[0] = "Student";
                        dtrow[1] = "Student";
                        dtrow[2] = "Student";
                        dtrow[3] = "Student";
                        dtrow[4] = "Student";
                        dt.Rows.Add(dtrow);
                        // goto rowaddskip;
                    }
                    for (int i = 0; i < count4; i++) // poo 21.12.17
                    {
                        DataRow dtrow1 = dt.NewRow();
                        count++;
                        //dtrow1[0] = d + 1; // poo 
                        dtrow1[0] = count; // poo 
                        dtrow1[1] = ds.Tables[0].Rows[d]["Full_Name"].ToString(); //poo
                        dtrow1[2] = dv_demand_data[i]["date"].ToString(); //poo                        
                        //dtrow1[2] = ds.Tables[0].Rows[d]["date"].ToString(); //poo
                        totalval = 0;
                        if (count4 > 0)
                        {
                            if (arr_staf.Contains(ds.Tables[0].Rows[d]["staff_code"].ToString()))
                            {
                                //for (int i = 0; i < count4; i++) // poo 21.12.17
                                //{
                                //totalval = totalval + Convert.ToInt32(dv_demand_data[i]["count"]);
                                totalval = Convert.ToInt32(dv_demand_data[i]["count"]);
                                //}
                            }
                        }
                        dtrow1[3] = totalval;
                        //dtrow1[4] = dv_demand_data[i]["staff_code"].ToString(); //poo
                        dtrow1[4] = ds.Tables[0].Rows[d]["staff_code"].ToString(); // poo

                        dt.Rows.Add(dtrow1);
                    }
                }
            }
            int a = dt.Rows.Count;
            if (a == 0)
            {
                GridView3.Visible = false;
                GridView4.Visible = false;
                lblerr.Visible = true;
                lblerr.Text = "No Records Found";
            }
            else
            {
                lblerr.Text = "";
                GridView3.DataSource = dt;
                GridView3.DataBind();
                GridView3.Visible = true;
                GridView4.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblerr.Text = ex.ToString();
            lblerr.Visible = true;
        }
    }

    protected void rowbound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(GridView3, "Type$" + e.Row.RowIndex);
            e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(GridView3, "Type1$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(GridView3, "Type2$" + e.Row.RowIndex);
            e.Row.Cells[0].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(GridView3, "Type3$" + e.Row.RowIndex);
        }
    }

    protected void change(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            DataSet flagds = new DataSet();
            int row = Convert.ToInt32(e.CommandArgument);
            // GridView3.Rows[row].Cells[columnIndex].Style.BackColor = Color.Red;
            string code = ((GridView3.Rows[row].FindControl("code") as Label).Text);
            ds.Clear();
            string sql = "";
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            ArrayList arr_staf = new ArrayList();
            dt1.Columns.Add("SNo", typeof(int));
            dt1.Columns.Add("Date", typeof(string));
            dt1.Columns.Add("Time", typeof(string));
            dt1.Columns.Add("Count", typeof(string));
            string fdtime = "";
            string tdtime = "";
            string dtime = DateTime.Now.ToString("HH:mm:ss");
            string firstdate = Convert.ToString(tbstart_date.Text);
            string seconddate = Convert.ToString(tbend_date.Text);
            string[] splitdate = firstdate.Split('/');
            string[] splitdate2 = seconddate.Split('/');
            fdtime = splitdate[2].ToString() + "-" + splitdate[1].ToString() + "-" + splitdate[0].ToString() + " " + "00:00:00";
            tdtime = splitdate2[2].ToString() + "-" + splitdate2[1].ToString() + "-" + splitdate2[0].ToString() + " " + dtime;

            string sqlflag = "select flag from logindetails where staff_code='" + code + "'";
            string valueflag = "";
            flagds.Clear();
            flagds.Dispose();
            flagds = da.select_method_wo_parameter(sqlflag, "Text");
            if (flagds.Tables[0].Rows.Count > 0)
            {
                valueflag = flagds.Tables[0].Rows[0][0].ToString();
            }
            if (valueflag == "2")
            {
                sql = "SELECT DISTINCT ld.staff_code, count(ld.staff_code) AS count , CONVERT(VARCHAR, cast(ld.dateandtime as date), 101) as  date, um.Stud_Name as Full_Name FROM logindetails ld, registration um where ld.staff_code=um.Roll_No  and  ld.dateandtime between '" + fdtime + "' and '" + tdtime + "' and ld.staff_code='" + code + "'   GROUP BY ld.staff_code,cast(dateandtime as date), um.Stud_Name;";
            }
            else if (valueflag == "1")
            {
                sql = "SELECT DISTINCT ld.staff_code, count(ld.staff_code) AS count , CONVERT(VARCHAR, cast(ld.dateandtime as date), 101) as  date,um.Full_Name,um.Description FROM logindetails ld, UserMaster um where ld.staff_code=um.User_id  and ld.dateandtime between '" + fdtime + "' and '" + tdtime + "' and ld.staff_code='" + code + "'  GROUP BY ld.staff_code,cast(dateandtime as date), um.Full_Name,um.Description;";
            }
            else if (valueflag == "0")
            {
                sql = "SELECT DISTINCT ld.staff_code, count(ld.staff_code) AS count , CONVERT(VARCHAR, cast(ld.dateandtime as date), 101) as  date,um.Full_Name,um.Description FROM logindetails ld, UserMaster um where ld.staff_code=um.User_id  and ld.dateandtime between '" + fdtime + "' and '" + tdtime + "' and ld.staff_code='" + code + "'  GROUP BY ld.staff_code,cast(dateandtime as date), um.Full_Name,um.Description;";
            }
            DataSet ds1 = new DataSet();

            ds = da.select_method_wo_parameter(sql, "Text");
            int count = 0;
            int count1 = 1;
            for (int d = 0; d < ds.Tables[0].Rows.Count; d++)
            {
                count++;

                count = count + 1;

                sql = "  select LTRIM(RIGHT(CONVERT(CHAR(20), dateandtime, 22), 11)) as Time  from logindetails where staff_code='" + code + "' and cast(dateandtime as date) = '" + ds.Tables[0].Rows[d]["date"].ToString() + "' order by Time asc";
                ds1 = da.select_method_wo_parameter(sql, "Text");
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    DataRow dtrow1 = dt1.NewRow();
                    dtrow1[0] = count1;
                    dtrow1[1] = ds.Tables[0].Rows[d]["date"].ToString();
                    dtrow1[3] = ds.Tables[0].Rows[d]["count"].ToString();
                    dtrow1[2] = ds1.Tables[0].Rows[i]["Time"].ToString();
                    dt1.Rows.Add(dtrow1);
                }
                count1++;
            }
            GridView4.DataSource = dt1;
            GridView4.DataBind();
            GridView4.Visible = true;
        }
        catch (Exception ex)
        {
            lblerr.Text = ex.ToString();
            lblerr.Visible = true;
        }
    }
    protected void bindboundgv3(object sender, EventArgs e)
    {

        try
        {
            for (int i = 0; i < GridView3.Rows.Count; i++)
            {
                GridViewRow row = GridView3.Rows[i];
                Label lnlhead1 = (Label)row.FindControl("lblsno");
                Label lnlhead2 = (Label)row.FindControl("user");
                Label lnlhead3 = (Label)row.FindControl("lbldate");
                Label lnlhead4 = (Label)row.FindControl("lblcount");
                if (lnlhead1.Text == lnlhead2.Text && lnlhead3.Text == lnlhead4.Text)
                {
                    row.Cells[0].ColumnSpan = 4;
                    row.Cells[1].Visible = false;
                    row.Cells[2].Visible = false;
                    row.Cells[3].Visible = false;
                    row.Cells[0].BackColor = System.Drawing.Color.LightBlue;
                }
            }
        }
        catch (Exception ex)
        {
            lblerr.Text = ex.ToString();
            lblerr.Visible = true;
        }
    }
    protected void bindbound(object sender, EventArgs e)
    {
        try
        {
            for (int i = GridView4.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = GridView4.Rows[i];
                GridViewRow previousRow = GridView4.Rows[i - 1];

                Label lnlname = (Label)row.FindControl("gv4sno");
                Label lnlname1 = (Label)previousRow.FindControl("gv4sno");
                if (lnlname.Text == lnlname1.Text)
                {
                    if (previousRow.Cells[0].RowSpan == 0)
                    {
                        if (row.Cells[0].RowSpan == 0)
                        {
                            previousRow.Cells[0].RowSpan += 2;
                        }
                        else
                        {
                            previousRow.Cells[0].RowSpan = row.Cells[0].RowSpan + 1;
                        }
                        row.Cells[0].Visible = false;
                    }
                }

                lnlname = (Label)row.FindControl("gv4dt");
                lnlname1 = (Label)previousRow.FindControl("gv4dt");

                if (lnlname.Text == lnlname1.Text)
                {
                    if (previousRow.Cells[1].RowSpan == 0)
                    {
                        if (row.Cells[1].RowSpan == 0)
                        {
                            previousRow.Cells[1].RowSpan += 2;
                        }
                        else
                        {
                            previousRow.Cells[1].RowSpan = row.Cells[1].RowSpan + 1;
                        }
                        row.Cells[1].Visible = false;
                    }
                }


                Label lnlname2 = (Label)row.FindControl("gv4dt");
                Label lnlname12 = (Label)previousRow.FindControl("gv4dt");

                lnlname = (Label)row.FindControl("gv4count");
                lnlname1 = (Label)previousRow.FindControl("gv4count");

                if (lnlname.Text == lnlname1.Text && lnlname2.Text == lnlname12.Text)
                {
                    if (previousRow.Cells[3].RowSpan == 0)
                    {
                        if (row.Cells[3].RowSpan == 0)
                        {
                            previousRow.Cells[3].RowSpan += 2;
                        }
                        else
                        {
                            previousRow.Cells[3].RowSpan = row.Cells[3].RowSpan + 1;
                        }
                        row.Cells[3].Visible = false;
                    }
                }


            }
        }
        catch (Exception ex)
        {
            lblerr.Text = ex.ToString();
            lblerr.Visible = true;
        }
    }
    protected void tbstart_date_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            //GridView3.Visible = false;
            //GridView4.Visible = false;
            DateTime dtnow = DateTime.Now;
            //lblerroe.Visible = false;
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
                    // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "alert('Please Enter Valid From date');", true);
                    lblerr.Text = "Please Enter Valid From date";
                    GridView3.Visible = false;
                    GridView4.Visible = false;
                    tbstart_date.Text = DateTime.Now.ToString("dd/MM/yyy");
                }
            }
        }
        catch (Exception ex)
        {
            lblerr.Text = ex.ToString();
            lblerr.Visible = true;
        }
    }
    protected void tbend_date_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            //GridView3.Visible = false;
            //GridView4.Visible = false;
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
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "alert('Please Enter Valid To date');", true);
                    lblerr.Text = "Please Enter Valid To date";
                    GridView3.Visible = false;
                    GridView4.Visible = false;
                    tbend_date.Text = DateTime.Now.ToString("dd/MM/yyy");
                }
            }
            if (tbstart_date.Text != "" && tbend_date.Text != "")
            {
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
                            // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "alert('From Date Cant Be Greater Than To Date');", true);
                            GridView3.Visible = false;
                            GridView4.Visible = false;
                            lblerr.Text = "From Date Cant Be Greater Than To Date";
                            tbend_date.Text = DateTime.Now.ToString("dd/MM/yyy");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblerr.Text = ex.ToString();
            lblerr.Visible = true;
        }
    }

    protected void ddluser_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ucount = 0;
        for (int i = 0; i < ddluser.Items.Count; i++)
        {
            if (ddluser.Items[i].Selected == true)
            {
                ucount++;

            }
        }
        SelectAll.Checked = false;
        if (ucount != 0)
        {
            TextBox1.Text = "Users(" + Convert.ToString(ucount) + ")";
        }
        else
        {
            TextBox1.Text = "--Select--";
        }

    }
    protected void SelectAll_CheckedChanged(object sender, EventArgs e)
    {
        int ucount = 0;
        if (SelectAll.Checked == true)
        {
            for (int i = 0; i < ddluser.Items.Count; i++)
            {
                ddluser.Items[i].Selected = true;
                ucount++;
            }
            TextBox1.Text = "Users(" + Convert.ToString(ucount) + ")";
        }
        else
        {
            for (int i = 0; i < ddluser.Items.Count; i++)
            {
                ddluser.Items[i].Selected = false;

            }
            TextBox1.Text = "--Select--";
        }
    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }

}