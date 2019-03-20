using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.Data;
using Gios.Pdf;
using System.Text;


public partial class staffattendreport : System.Web.UI.Page
{

    string sql;
    string sql2;
    string stffcode = "";
    int countstaff = 0;
    string tempdept_ = "";
    double counttotalpresent = 0;
    double eveningpresent = 0;
    double eveningabsent = 0;
    double totalmorningpresent = 0;
    double totaleveningpresent = 0;
    double totalevenngabsent = 0;
    double totalmorningabsent = 0;
    double counttotalabsent = 0;
    StringBuilder SbStringMrngP = new StringBuilder();
    StringBuilder SbStringEveP = new StringBuilder();

    StringBuilder SbStringMrngA = new StringBuilder();
    StringBuilder SbStringEveA = new StringBuilder();

    SqlConnection myconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection myconn1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    DataSet ds = new DataSet();
    SqlDataAdapter da = new SqlDataAdapter();
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    DAccess2 d2 = new DAccess2();
    bool check = false;
    //Added By Srinath 1/4/2013
    string collegecode = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";

    int rowstr = 0;
    int spreadrowcnt;

    string coll_name = string.Empty;
    string coll_address1 = string.Empty;
    string coll_address2 = string.Empty;
    string coll_address3 = string.Empty;
    string pin_code = string.Empty;

    //==========Variable declaration for PDF
    int read_spread = 0;
    int read_spread1;
    string department = string.Empty;
    string total_no_of_staff = string.Empty;
    string morning1 = string.Empty;
    string morning2 = string.Empty;
    string evening1 = string.Empty;
    string evening2 = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            //string PageLogOut = "";
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("HRM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["HR"].Trim();
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

            if (!Request.FilePath.Contains("HRMenuIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/HRMOD/HRMenuIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            lblnorec.Visible = false;

            if (!IsPostBack)
            {
                fpattendance.Visible = false;
                lblexcel.Visible = false;
                txtxl.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                fpattendance.Sheets[0].RowHeader.Visible = false;
                string today = System.DateTime.Now.ToString();
                string today1;
                string[] split13 = today.Split(new char[] { ' ' });
                string[] split14 = split13[0].Split(new Char[] { '/' });
                today1 = split14[1].ToString() + "/" + split14[0].ToString() + "/" + split14[2].ToString();
                Txtentryfrom.Text = today1;
                string today2 = System.DateTime.Now.ToString();
                string today3;
                string[] split15 = today.Split(new char[] { ' ' });
                string[] split16 = split13[0].Split(new Char[] { '/' });
                today3 = split16[1].ToString() + "/" + split16[0].ToString() + "/" + split16[2].ToString();
                Txtentryto.Text = today3;
                load_dept();
            }
        }
        catch (Exception ex) { }
    }

    void load_dept()
    {

        cbldepttype.Visible = true;
        cbldepttype.Items.Clear();
        ds.Clear();
        ListItem lsitem = new ListItem();
        //Modified By Srinath 1/4/2013
        //con.Open();
        //SqlCommand cmd = new SqlCommand("select distinct dept_code,dept_name from hrdept_master  ", con);
        //da.SelectCommand = cmd;
        //da.Fill(ds);
        Hashtable hat = new Hashtable();
        string deptquery = "";
        string singleuser = Session["single_user"].ToString();
        if (singleuser == "True")
        {
            deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["collegecode"] + "') order by dept_name";
        }

        else
        {

            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["collegecode"] + "') order by dept_name";
        }
        if (deptquery != "")
        {
            ds = d2.select_method(deptquery, hat, "Text");
            cbldepttype.DataSource = ds.Tables[0];
            cbldepttype.DataTextField = "dept_name";
            cbldepttype.DataValueField = "dept_code";
            cbldepttype.DataBind();
            lsitem.Text = "All";
            cbldepttype.Items.Insert(0, lsitem);
            // con.Close();
        }
    }


    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            load_click();
        }
        catch (Exception ex)
        {
            //throw ex;
            d2.sendErrorMail(ex, Convert.ToString(Session["collegecode"]), "Staffattendreport.aspx");
        }
    }

    protected void fpattendance_Click(object sender, EventArgs e)
    {
        try
        {
            check = true;
        }
        catch
        {

        }
    }

    protected void fpattendance_Render(object sender, EventArgs e)
    {
        try
        {
            if (check)
            {
                string activrow = "";
                activrow = fpattendance.Sheets[0].ActiveRow.ToString();
                string activecol = fpattendance.Sheets[0].ActiveColumn.ToString();
                int actcol = Convert.ToInt16(activecol);
                Fpspreadpay1.Visible = false;

                Fpspreadpay1.Sheets[0].RowCount = 0;
                Fpspreadpay1.Sheets[0].ColumnCount = 0;
                Fpspreadpay1.CommandBar.Visible = false;
                Fpspreadpay1.Sheets[0].RowHeader.Visible = false;

                if (activrow.Trim() != "-1" && actcol != -1 && actcol > 2)
                {
                    string TagName = Convert.ToString((fpattendance.Sheets[0].Cells[Convert.ToInt32(activrow), Convert.ToInt32(actcol)].Tag));
                    string DepName = Convert.ToString((fpattendance.Sheets[0].Cells[Convert.ToInt32(activrow), Convert.ToInt32(1)].Text));
                    string Session = Convert.ToString((fpattendance.Sheets[0].ColumnHeader.Cells[Convert.ToInt32(1), Convert.ToInt32(actcol)].Text));
                    string ATtnd = Convert.ToString((fpattendance.Sheets[0].ColumnHeader.Cells[Convert.ToInt32(1), Convert.ToInt32(actcol)].Tag));
                    if (TagName.Length > 0)
                    {
                        TagName.Remove(TagName.Length - 1, 1);
                        string QueryStaff = "select Staff_code,staff_name,sex from staffMaster s,staff_appl_master sa where s.appl_no=sa.appl_no and s.staff_code in ('" + TagName.ToString() + "') order by sex,Staff_code";
                        DataSet Dstaff = d2.select_method_wo_parameter(QueryStaff, "Text");
                        if (Dstaff.Tables.Count > 0 && Dstaff.Tables[0].Rows.Count > 0)
                        {

                            DateSpan.InnerHtml = Txtentryfrom.Text.ToString();
                            DepartmentSpan.InnerHtml = DepName.ToString();
                            AttendanceSpan.InnerHtml = ATtnd.ToString();
                            SessionSpan.InnerHtml = Session.ToString();

                            BindHear();
                            int Count = 0;
                            DataTable dtGender = Dstaff.Tables[0].DefaultView.ToTable(true, "sex");
                            if (dtGender.Rows.Count > 0)
                            {
                                for (int intDG = 0; intDG < dtGender.Rows.Count; intDG++)
                                {
                                    Dstaff.Tables[0].DefaultView.RowFilter = "sex='" + Convert.ToString(dtGender.Rows[intDG]["sex"]) + "'";
                                    DataView dsNew = Dstaff.Tables[0].DefaultView;
                                    if (dsNew.Count > 0)
                                    {

                                        Fpspreadpay1.Sheets[0].RowCount++;
                                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dtGender.Rows[intDG]["sex"]);
                                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 0].ForeColor = Color.Blue;
                                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                        Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                        Fpspreadpay1.Sheets[0].SpanModel.Add(Fpspreadpay1.Sheets[0].RowCount - 1, 0, 1, 3);

                                        for (int intDs = 0; intDs < dsNew.Count; intDs++)
                                        {
                                            Count++;
                                            Fpspreadpay1.Sheets[0].RowCount++;
                                            Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(Count);
                                            Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                            Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                            Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dsNew[intDs]["Staff_code"]);
                                            Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                            Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                            Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                            Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dsNew[intDs]["staff_name"]);

                                            Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                            Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                            Fpspreadpay1.Sheets[0].Cells[Fpspreadpay1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                        }
                                    }
                                }
                                popper1.Visible = true;
                                Fpspreadpay1.Visible = true;
                                Fpspreadpay1.Sheets[0].PageSize = Fpspreadpay1.Sheets[0].RowCount;
                                Fpspreadpay1.Width = 500;
                                Fpspreadpay1.Height = 450;
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

    public void BindHear()
    {
        try
        {
            Fpspreadpay1.Sheets[0].ColumnCount = 3;
            Fpspreadpay1.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspreadpay1.Sheets[0].AutoPostBack = false;

            Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpspreadpay1.Sheets[0].Columns[0].Width = 80;

            Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Name";
            Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            Fpspreadpay1.Sheets[0].Columns[1].Width = 150;


            Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Code";
            Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            Fpspreadpay1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            Fpspreadpay1.Sheets[0].Columns[2].Width = 250;


        }
        catch
        {

        }
    }

    protected void imagebtnpopclose2_Click(object sender, EventArgs e)
    {
        try
        {
            popper1.Visible = false;
        }
        catch
        {

        }
    }

    void load_click()
    {

        fpattendance.Visible = true;
        btnprintmaster.Visible = true;
        string date1;
        string datefrom;
        string date2;
        string dateto;
        string date6;
        string year;
        int day3;
        string monyear;
        DateTime monyear1;
        int day5;
        date1 = Txtentryfrom.Text.ToString();
        string[] split = date1.Split(new Char[] { '/' });
        datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
        // monyearfrom= split[1].ToString() +"/" + split[2].ToString();
        //  day5=
        year = split[2].ToString();
        date2 = Txtentryto.Text.ToString();
        string[] split1 = date2.Split(new Char[] { '/' });
        dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
        date6 = split[1].ToString();
        DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
        DateTime dt2 = Convert.ToDateTime(dateto.ToString());
        TimeSpan t = dt2.Subtract(dt1);
        long days = t.Days;
        day3 = Convert.ToInt32(days);

        fpattendance.Sheets[0].RowCount = 0;
        fpattendance.Sheets[0].ColumnCount = 0;
        //fpattendance.CommandBar.Visible = false;
        //fpattendance.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";

        //fpattendance.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        //fpattendance.Sheets[0].SheetCorner.Cells[0, 0].Border.BorderColorBottom = Color.White;
        //fpattendance.Sheets[0].SheetCorner.Cells[0, 0].Font.Bold = true;
        //fpattendance.Sheets[0].SheetCorner.Cells[0, 0].Font.Size = FontUnit.Large;

        fpattendance.Sheets[0].PageSize = 10;
        fpattendance.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
        fpattendance.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
        fpattendance.Pager.Align = HorizontalAlign.Right;
        fpattendance.Pager.Font.Bold = true;
        fpattendance.Pager.Font.Name = "Book Antiqua";
        fpattendance.Pager.ForeColor = Color.DarkGreen;
        fpattendance.Pager.BackColor = Color.AliceBlue;
        fpattendance.Pager.PageCount = 5;

        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.Font.Name = "Book Antiqua";
        darkstyle.HorizontalAlign = HorizontalAlign.Center;
        darkstyle.Font.Bold = true;
        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        fpattendance.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

        //=============Added by Manikandan 08/05/2013
        fpattendance.Sheets[0].ColumnHeader.RowCount = 2;


        string str = "select isnull(collname, ' ') as collname,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
        con.Close();
        con.Open();
        SqlCommand comm = new SqlCommand(str, con);
        SqlDataReader drr = comm.ExecuteReader();
        drr.Read();
        coll_name = Convert.ToString(drr["collname"]);
        coll_address1 = Convert.ToString(drr["address1"]);
        coll_address2 = Convert.ToString(drr["address2"]);
        coll_address3 = Convert.ToString(drr["address3"]);
        pin_code = Convert.ToString(drr["pincode"]);

        fpattendance.Sheets[0].ColumnCount = 7;

        fpattendance.Sheets[0].ColumnHeader.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
        fpattendance.Sheets[0].ColumnHeader.Rows[1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
        //=========================

        fpattendance.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        fpattendance.Sheets[0].SetColumnWidth(0, 50);
        fpattendance.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
        fpattendance.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        fpattendance.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Department";
        fpattendance.Sheets[0].SetColumnWidth(1, 200);
        fpattendance.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Total No Of Staff";
        fpattendance.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Present";
        fpattendance.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 1, 2);
        fpattendance.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Absent";
        fpattendance.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 1, 2);

        fpattendance.Sheets[0].ColumnHeader.Cells[0, 1, 1, 5].HorizontalAlign = HorizontalAlign.Center;

        fpattendance.Sheets[0].ColumnHeader.Cells[1, 1, 1, 5].HorizontalAlign = HorizontalAlign.Center;



        fpattendance.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
        fpattendance.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);

        fpattendance.Sheets[0].ColumnHeader.Cells[1, 0].Text = " ";
        fpattendance.Sheets[0].ColumnHeader.Cells[1, 1].Text = " ";
        fpattendance.Sheets[0].ColumnHeader.Cells[1, 3].Text = "Morning";
        fpattendance.Sheets[0].ColumnHeader.Cells[1, 3].Tag = "Present";
        fpattendance.Sheets[0].ColumnHeader.Cells[1, 4].Text = "Evening";
        fpattendance.Sheets[0].ColumnHeader.Cells[1, 4].Tag = "Present";

        fpattendance.Sheets[0].ColumnHeader.Cells[1, 5].Text = "Morning";
        fpattendance.Sheets[0].ColumnHeader.Cells[1, 5].Tag = "Absent";
        fpattendance.Sheets[0].ColumnHeader.Cells[1, 6].Text = "Evening";
        fpattendance.Sheets[0].ColumnHeader.Cells[1, 6].Tag = "Absent";


        // added staffcategorizer table in below query by jairam 08-11-2014 Modififed By delsi 10.01.2018 Added join Date
        sql = "SELECT distinct staffmaster.staff_code , staff_name,desig_master.desig_name,dept_name,stafftrans.dept_code,desig_master.priority, stafftrans.smdate, staffmaster.staff_code  FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code inner join hr_privilege on hr_privilege.dept_code=hrdept_master.dept_code";
        sql = sql + " AND staffmaster.college_code = hrdept_master.college_code INNER JOIN desig_master ON stafftrans.desig_code = desig_master.desig_code and hrdept_master.dept_code=stafftrans.dept_code inner join staffcategorizer on staffcategorizer.college_code =hrdept_master.college_code and staffcategorizer.college_code=staffmaster.college_code and staffcategorizer.category_code =stafftrans.category_code and hrdept_master.college_code=" + Session["collegecode"] + " and desig_master.collegecode='" + Session["collegecode"] + "' where stafftrans.latestrec<>0 and ISNULL(DispReports,0) =1 and  (staffmaster.resign=0 and staffmaster.settled=0) and join_date<='" + datefrom + "' and staffmaster.college_code=" + Session["collegecode"] + "";

        if (cbldepttype.SelectedItem.Value.ToString() != "All")
        {
            sql = sql + " and hrdept_master.dept_code = '" + cbldepttype.SelectedItem.Value.ToString() + "'";
        }
        sql = sql + " order by hrdept_master.dept_name";
        con1.Open();
        SqlCommand cmd = new SqlCommand(sql, con1);
        SqlDataReader dr14;
        dr14 = cmd.ExecuteReader();

        tempdept_ = "";
        int sno = 0;


        while (dr14.Read())
        {
            if (dr14.HasRows == true)
            {
                string staffcode = "";

                staffcode = dr14["staff_code"].ToString();
                if (tempdept_ == "")
                {
                    sno++;
                    tempdept_ = dr14["dept_name"].ToString();
                    rowstr = fpattendance.Sheets[0].RowCount++;
                }
                else if ((tempdept_ != "") && (tempdept_ != dr14["dept_name"].ToString()))
                {
                    //spreadrowcnt++;
                    sno++;
                    rowstr = fpattendance.Sheets[0].RowCount++;
                    fpattendance.Sheets[0].Cells[rowstr, 0].Text = sno.ToString();
                    fpattendance.Sheets[0].Cells[rowstr, 1].Text = tempdept_.ToString();
                    fpattendance.Sheets[0].Cells[rowstr, 2].Text = countstaff.ToString();
                    fpattendance.Sheets[0].Cells[rowstr, 3].Text = totalmorningpresent.ToString();//Modified By Manikandan 08/05/2013
                    fpattendance.Sheets[0].Cells[rowstr, 4].Text = totaleveningpresent.ToString();//Added by Manikandan 08/05/2013
                    fpattendance.Sheets[0].Cells[rowstr, 5].Text = totalmorningabsent.ToString();//Modified by Manikandan 08/05/2013
                    fpattendance.Sheets[0].Cells[rowstr, 6].Text = totalevenngabsent.ToString();//Added by Manikandan 08/05/2013

                    totaleveningpresent = 0;
                    totalmorningpresent = 0;
                    counttotalpresent = 0;
                    totalmorningabsent = 0;
                    totalevenngabsent = 0;
                    counttotalabsent = 0;
                    countstaff = 0;
                    tempdept_ = dr14["dept_name"].ToString();

                    SbStringMrngP = new StringBuilder();
                    SbStringEveP = new StringBuilder();
                    SbStringMrngA = new StringBuilder();
                    SbStringEveA = new StringBuilder();

                }


                string totalpresent = getfunction(staffcode, datefrom);

                fpattendance.Sheets[0].Cells[rowstr, 0].Text = sno.ToString();
                fpattendance.Sheets[0].Cells[rowstr, 0].HorizontalAlign = HorizontalAlign.Center;
                fpattendance.Sheets[0].Cells[rowstr, 1].Text = tempdept_.ToString();
                fpattendance.Sheets[0].Cells[rowstr, 1].HorizontalAlign = HorizontalAlign.Left;
                fpattendance.Sheets[0].Cells[rowstr, 2].Text = countstaff.ToString();
                fpattendance.Sheets[0].Cells[rowstr, 2].HorizontalAlign = HorizontalAlign.Center;
                fpattendance.Sheets[0].Cells[rowstr, 3].Text = totalmorningpresent.ToString();//Modified by Manikandan 08/05/2013
                fpattendance.Sheets[0].Cells[rowstr, 3].Tag = SbStringMrngP.ToString();

                fpattendance.Sheets[0].Cells[rowstr, 4].Text = totaleveningpresent.ToString();//Added by Manikandan 08/05/2013
                fpattendance.Sheets[0].Cells[rowstr, 4].Tag = SbStringEveP.ToString();

                fpattendance.Sheets[0].Cells[rowstr, 3].HorizontalAlign = HorizontalAlign.Center;
                fpattendance.Sheets[0].Cells[rowstr, 4].HorizontalAlign = HorizontalAlign.Center;
                fpattendance.Sheets[0].Cells[rowstr, 5].Text = totalmorningabsent.ToString();//Modified by Manikandan 08/05/2013
                fpattendance.Sheets[0].Cells[rowstr, 5].Tag = SbStringMrngA.ToString();

                fpattendance.Sheets[0].Cells[rowstr, 6].Text = totalevenngabsent.ToString();//Modified by Manikandan 08/05/2013
                fpattendance.Sheets[0].Cells[rowstr, 6].Tag = SbStringEveA.ToString();

                fpattendance.Sheets[0].Cells[rowstr, 5].HorizontalAlign = HorizontalAlign.Center;
                fpattendance.Sheets[0].Cells[rowstr, 6].HorizontalAlign = HorizontalAlign.Center;

                lblexcel.Visible = true;
                txtxl.Visible = true;
                btnxl.Visible = true;
                btnprintmaster.Visible = true;
                lblnorec.Visible = false;
            }

            //spreadrowcnt = fpattendance.Sheets[0].RowCount;
        }

        fpattendance.Sheets[0].RowCount++;
        fpattendance.Sheets[0].Cells[fpattendance.Sheets[0].RowCount - 1, 1].Text = "Total";
        fpattendance.Sheets[0].Cells[fpattendance.Sheets[0].RowCount - 1, 1].Font.Bold = true;
        fpattendance.Sheets[0].Cells[fpattendance.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
        fpattendance.Sheets[0].Cells[fpattendance.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Right;

        for (int tot = 2; tot < fpattendance.Sheets[0].ColumnCount; tot++)
        {
            int total = 0;
            for (int tot_row = 0; tot_row < fpattendance.Sheets[0].RowCount - 1; tot_row++)
            {
                total = total + Convert.ToInt32(fpattendance.Sheets[0].Cells[tot_row, tot].Text.ToString());
            }
            fpattendance.Sheets[0].Cells[fpattendance.Sheets[0].RowCount - 1, tot].Text = total.ToString();

            fpattendance.Sheets[0].Cells[fpattendance.Sheets[0].RowCount - 1, tot].HorizontalAlign = HorizontalAlign.Center;
            fpattendance.Sheets[0].Cells[fpattendance.Sheets[0].RowCount - 1, tot].Font.Bold = true;
            fpattendance.Sheets[0].Cells[fpattendance.Sheets[0].RowCount - 1, tot].Font.Name = "Book Antiqua";
            fpattendance.Sheets[0].Cells[fpattendance.Sheets[0].RowCount - 1, tot].Font.Size = FontUnit.Medium;
        }


        Double totalRows = 0;
        totalRows = Convert.ToInt32(fpattendance.Sheets[0].RowCount);

        if (totalRows >= 10)
        {
            fpattendance.Sheets[0].PageSize = Convert.ToInt32(totalRows);

            fpattendance.Height = 350;
            fpattendance.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            fpattendance.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

        }
        else if (totalRows == 0)
        {

            fpattendance.Height = 300;
        }
        else
        {
            fpattendance.Sheets[0].PageSize = Convert.ToInt32(totalRows);

            fpattendance.Height = 75 + (75 * Convert.ToInt32(totalRows));
        }


        Session["totalPages"] = (int)Math.Ceiling(totalRows / fpattendance.Sheets[0].PageSize);




        if (dr14.HasRows == false)
        {
            fpattendance.Visible = false;
            lblexcel.Visible = false;
            txtxl.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            lblnorec.Visible = true;
            lblnorec.Text = "No Records Found!";
        }

        //fpattendance.Sheets[0].RowHeader.Cells[fpattendance.Sheets[0].RowCount - 1, 0].Text = " ";

    }

    public string getfunction(string scode, string fromdate)
    {
        DateTime frmdate;
        DateTime frmda2;
        int day5;
        string scode2, fromdate2, todate2;
        int month;
        scode2 = scode;
        fromdate2 = fromdate;
        // todate2 = todate;
        string monyear;
        DateTime monyear1;
        if (scode != stffcode)
        {

            countstaff = countstaff + 1;
        }
        stffcode = scode;
        string[] split = fromdate2.Split(new Char[] { '/' });
        fromdate2 = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
        frmda2 = Convert.ToDateTime(fromdate.ToString());
        // string[] split1 = todate2.Split(new Char[] { '/' });
        //todate2 = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
        //  DateTime dt1 = Convert.ToDateTime(fromdate2.ToString());
        //   DateTime dt2 = Convert.ToDateTime(todate2.ToString());
        // TimeSpan t = dt2.Subtract(dt1);
        // long days = t.Days;
        //  frmda2 = Convert.ToDateTime(frmdate.ToString());
        //  frmdate = Convert.ToDateTime(fromdate2);


        string date;
        date = Convert.ToString(frmda2);

        string[] split25 = date.Split(new char[] { ' ' });
        string[] spllit28 = split25[0].Split(new char[] { '/' });

        string datesplit = spllit28[0].ToString() + "/" + spllit28[1].ToString() + "/" + spllit28[2].ToString();
        month = Convert.ToInt16(spllit28[0].ToString());

        monyear = month + "/" + spllit28[2].ToString();



        day5 = Convert.ToInt16(spllit28[1].ToString());
        myconn1.Close();
        myconn1.Open();
        sql2 = "select * from staff_attnd where staff_code='" + scode2 + "' and mon_year='" + monyear + "'";

        SqlCommand cmd22 = new SqlCommand(sql2, myconn1);
        SqlDataReader dr23;
        dr23 = cmd22.ExecuteReader();
        while (dr23.Read())
        {
            if (dr23.HasRows == true)
            {
                day5 = Convert.ToInt16(day5 + 3);
                string att = "";
                att = dr23[day5].ToString();
                if (att != "")
                {
                    string mrng5;
                    string eveng5;
                    string[] tmpdate = att.ToString().Split(new char[] { '-' });


                    mrng5 = tmpdate[0].ToString();
                    eveng5 = tmpdate[1].ToString();
                    if ((mrng5 == "P") || (mrng5 == "LA") || (mrng5 == "PER") || (mrng5 == "OD") || (mrng5 == "OOD"))
                    {
                        totalmorningpresent++;// = totalmorningpresent + 0.5;
                        SbStringMrngP.Append(scode2 + "','");
                    }
                    if ((eveng5 == "P") || (eveng5 == "LA") || (eveng5 == "PER") || (eveng5 == "OD") || (eveng5 == "OOD"))
                    {
                        totaleveningpresent++;// = totaleveningpresent + 0.5;
                        SbStringEveP.Append(scode2 + "','");
                    }
                    if ((mrng5 == "A") || (mrng5 == "CL") || (mrng5 == "LOP") || (mrng5 == "NA"))
                    {
                        totalmorningabsent++;// = totalmorningabsent + 0.5;
                        SbStringMrngA.Append(scode2 + "','");
                    }
                    if ((eveng5 == "A") || (eveng5 == "CL") || (eveng5 == "LOP") || (eveng5 == "NA"))
                    {
                        totalevenngabsent++;// = totalevenngabsent + 0.5;
                        SbStringEveA.Append(scode2 + "','");
                    }

                    counttotalpresent = totaleveningpresent + totalmorningpresent;


                    counttotalabsent = totalmorningabsent + totalevenngabsent;

                    // frmdate=  dt1.AddDays(1);


                }
            }
            else
            {
                return "";
            }
            //dr23.Close();

        }
        //frmda2 = frmda2.AddDays(1);
        //goto l2;







        return "";

    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtxl.Text;

            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(fpattendance, reportname);


            }
            else
            {
                lblnorec.Text = "Please Enter Your Report Name";
                lblnorec.Visible = true;
            }
            txtxl.Text = "";
            reportname = "";
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
            lblnorec.Visible = true;
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string degreedetails = string.Empty;
        fpattendance.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        Session["column_header_row_count"] = fpattendance.Sheets[0].ColumnHeader.RowCount;

        fpattendance.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Left;
        degreedetails = "Staff Attendance Strength Report @Date: " + Txtentryfrom.Text.ToString();
        string pagename = "StudentTestReport.aspx";

        Printcontrol.loadspreaddetails(fpattendance, pagename, degreedetails);
        Printcontrol.Visible = true;

    }
}