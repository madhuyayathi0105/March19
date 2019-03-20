using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections;
using System.Configuration;

public partial class Subject_Room_Allocation : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    DataSet ds = new DataSet();
    static Hashtable ht_sch = new Hashtable();
    static Hashtable ht_sdate = new Hashtable();
    static Hashtable ht_bell = new Hashtable();
    static Hashtable ht_period = new Hashtable();

    static Boolean hr_lock = false;
    string noofdays = "";
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    string course_id = string.Empty;
    string degree_var = "";

    string strday = "";
    string tmp_camprevar = "";
    string cur_camprevar = "";
    string strsction = "";
    Boolean dailyentryflag = false;
    Boolean attendanceentryflag = false;
    string start_datesem = "";
    string start_dayorder = "";

    string tmp_datevalue = "";
    String Day_Order = "";
    string Day_Var = "";
    string Att_strqueryst = "";
    string subj_count_in_onehr = "";
    static string selectedpath = "";
    static string storepath = "";
    Boolean check_record = false;
    string staff_code_value = "";


    protected void Page_Load(object sender, EventArgs e)
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
        staff_code_value = (string)Session["Staff_Code"];
        if (!IsPostBack)
        {
            txt_fromdate.Text = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            bindstaff();
            lbl_staffname1.Visible = false;
            lbl_stafnme2.Visible = false;
            FpSpread1.Visible = false;
            div_report.Visible = false;
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_todate.Attributes.Add("readonly", "readonly");



            Session["StaffSelector"] = "0";
            string minimumabsentsms = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Session["collegecode"].ToString().Trim() + "'");
            if (minimumabsentsms.Trim() == "1")
            {
                Session["StaffSelector"] = "1";
            }

        }
        lbl_stafnme2.Visible = false;
        Session["curr_year"] = DateTime.Now.ToString("yyyy");

        FpSpread1.Sheets[0].Columns.Default.Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].Columns.Default.Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Rows.Default.Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].Rows.Default.Font.Size = FontUnit.Medium;


        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.ForeColor = ColorTranslator.FromHtml("White");//nn
        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].RowHeader.DefaultStyle.Font.Bold = true;
        FpSpread1.Sheets[0].RowHeader.DefaultStyle.ForeColor = ColorTranslator.FromHtml("White");//nn
        FpSpread1.Sheets[0].RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;



        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        style.Font.Size = 15;
        style.Font.Bold = true;
        style.Font.Name = "Book Antiqua";

        //FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        //// style.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        //FpSpread1.Sheets[0].AllowTableCorner = true;
        //FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

        //FpSpread1.SheetCorner.Rows.Default.Font.Size = FontUnit.Medium;
        //FpSpread1.SheetCorner.Rows.Default.Font.Name = "Book Antiqua";
        //FpSpread1.SheetCorner.Rows.Default.Font.Bold = true;

        //FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].Text = "Date";
        //FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].BackColor = ColorTranslator.FromHtml("#0CA6CA");//nn
        //FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("White");

    }

    public void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    public void ddl_staffcode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string query = d2.GetFunction("select staff_name from staffmaster where staffmaster.staff_code='" + ddl_staffcode.SelectedValue.ToString() + "'");
            if (query.Trim() != "" && query != null && query.Trim() != "0")
            {
                lbl_stafnme2.Text = query;
                lbl_stafnme2.ForeColor = Color.White;
                ddl_staffname.SelectedValue = ddl_staffcode.SelectedValue.ToString();
                lbl_stafnme2.Visible = false;
            }


            else
            {
                lbl_staffname1.Visible = false;
                lbl_stafnme2.Text = "No Staff Available in this Code";
                lbl_stafnme2.ForeColor = Color.Red;

            }
            if (Session["Staff_Code_val"] != "")
            {
                lbl_stafnme2.Visible = false;
                lbl_staffname1.Visible = false;
            }
            string name_code = "";
            name_code = ddl_staffcode.SelectedValue.ToString();


            Session["Staff_Code_val"] = name_code.ToString();
        }
        catch (Exception ex)
        {
        }
    }
    public void ddl_staffname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string query = da.GetFunction("select staff_name from staffmaster where staffmaster.staff_code='" + ddl_staffname.SelectedValue.ToString() + "'");
            if (query.Trim() != "" && query != null && query.Trim() != "0")
            {
                lbl_stafnme2.Text = query;
                lbl_stafnme2.ForeColor = Color.White;
                ddl_staffcode.SelectedValue = ddl_staffname.SelectedValue.ToString();
            }
            else
            {
                lbl_stafnme2.Text = "No Staff Available in this Code";
                lbl_stafnme2.ForeColor = Color.Red;
            }
            if (Session["Staff_Code_val"] != "")
            {
                lbl_stafnme2.Visible = false;
                lbl_staffname1.Visible = false;
            }
            string name_code = "";
            name_code = ddl_staffcode.SelectedValue.ToString();


            Session["Staff_Code_val"] = name_code.ToString();
        }
        catch (Exception ex)
        {
        }
    }

    protected void txt_fromdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lbl_staf.Visible = false;


            string[] spiltfrom = txt_fromdate.Text.ToString().Split('/');
            string[] spitto = txt_todate.Text.ToString().Split('/');
            DateTime dtfrom = Convert.ToDateTime(spiltfrom[1] + '/' + spiltfrom[0] + '/' + spiltfrom[2]);
            DateTime dtto = Convert.ToDateTime(spitto[1] + '/' + spitto[0] + '/' + spitto[2]);
            DateTime dt1 = DateTime.Now.AddDays(-6);
            //string[] spiltfrom = txt_fromdate.Text.ToString().Split(new Char[] { '-' });
            //string[] spilto = txt_todate.Text.ToString().Split('-');
            //DateTime dtto = Convert.ToDateTime(spilto[1].ToString() + '-' + spilto[0].ToString() + '-' + spilto[2].ToString());
            //DateTime dtfrom = Convert.ToDateTime(spiltfrom[1].ToString() + '-' + spiltfrom[0].ToString() + '-' + spiltfrom[2].ToString());
            if (dtfrom > DateTime.Today)
            {
                lbl_staf.Visible = true;
                lbl_staf.Text = "Please Enter Valid From Date";
                FpSpread1.Visible = false;
            }
            if (dtfrom > dtto)
            {
                lbl_staf.Visible = true;
                lbl_staf.Text = "To Date Must be Greater than From Date";
                txt_todate.Text = txt_todate.Text;
                FpSpread1.Visible = false;
            }



        }
        catch
        {
            //lbl_staf.Visible = true;
            //lbl_staf.Text = "Please Enter Valid From Date";
            //FpSpread1.Visible = false;
        }
    }

    protected void txt_todate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lbl_staf.Visible = false;
            string[] spiltfrom = txt_fromdate.Text.ToString().Split(new Char[] { '/' });
            string[] spilto = txt_todate.Text.ToString().Split('/');
            DateTime dtto = Convert.ToDateTime(spilto[1].ToString() + '/' + spilto[0].ToString() + '/' + spilto[2].ToString());
            DateTime dtfrom = Convert.ToDateTime(spiltfrom[1].ToString() + '/' + spiltfrom[0].ToString() + '/' + spiltfrom[2].ToString());
            if (dtto > DateTime.Today)
            {
                lbl_staf.Visible = true;
                lbl_staf.Text = "Please Enter Valid From Date";
                FpSpread1.Visible = false;
            }
            if (dtfrom > dtto)
            {
                lbl_staf.Visible = true;
                lbl_staf.Text = "To Date Must be Greater than From Date";
                txt_todate.Text = txt_todate.Text;
                FpSpread1.Visible = false;
            }
        }
        catch
        {

        }
    }

    public void btn_go_Click(object sender, EventArgs e)
    {
        loadstafspread();
    }

    public void loadstafspread()
    {
        try
        {
            string sql_s = "";
            string Strsql = "";
            string SqlBatchYear = "";
            string SqlPrefinal1 = "";
            string SqlPrefinal2 = "";
            string SqlPrefinal3 = "";
            string SqlPrefinal4 = "";
            DataSet dsgetvalue = new DataSet();
            string SqlFinal = "";
            string sql1 = "";
            string tmp_varstr = "";
            string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            DataSet dsalterperiod = new DataSet();
            Hashtable hatsublab = new Hashtable();
            DataSet dsstuatt = new DataSet();
            Hashtable hatvalue = new Hashtable();

            string date1;
            string date2;
            int noofhrs = 0;
            string vari = "";
            DataSet ds_attndmaster = new DataSet();
            Hashtable ht_sch = new Hashtable();
            hat.Clear();
            hat.Add("college_code", Session["collegecode"].ToString());
            string sql_stringvar = "sp_select_details_staff";
            ds_attndmaster.Dispose();
            ds_attndmaster.Reset();
            ds_attndmaster = da.select_method(sql_stringvar, hat, "sp");
            if (ds_attndmaster.Tables[0].Rows.Count > 0)
            {
                for (int pcont = 0; pcont < ds_attndmaster.Tables[0].Rows.Count; pcont++)
                {
                    degree_var = Convert.ToString(ds_attndmaster.Tables[0].Rows[pcont]["degree_code"]) + "-" + Convert.ToString(ds_attndmaster.Tables[0].Rows[pcont]["semester"]);

                    if (!ht_sch.Contains(Convert.ToString(degree_var)))
                    {
                        vari = ds_attndmaster.Tables[0].Rows[pcont]["SchOrder"] + "," + ds_attndmaster.Tables[0].Rows[pcont]["nodays"];
                        ht_sch.Add(degree_var, Convert.ToString(vari));
                    }

                }
            }

            ht_sdate.Clear();

            if (ds_attndmaster.Tables[1].Rows.Count > 0)
            {
                for (int pcont = 0; pcont < ds_attndmaster.Tables[1].Rows.Count; pcont++)
                {
                    degree_var = Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["batch_year"]) + "-" + Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["degree_code"]) + "-" + Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["semester"]);

                    if (!ht_sdate.Contains(Convert.ToString(degree_var)))
                    {
                        vari = ds_attndmaster.Tables[1].Rows[pcont]["sdate"] + "," + ds_attndmaster.Tables[1].Rows[pcont]["starting_dayorder"];
                        ht_sdate.Add(degree_var, Convert.ToString(vari));
                    }

                }
            }

            ht_bell.Clear();

            if (ds_attndmaster.Tables[2].Rows.Count > 0)
            {
                for (int pcont = 0; pcont < ds_attndmaster.Tables[2].Rows.Count; pcont++)
                {
                    degree_var = Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["batch_year"]) + "-" + Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["degree_code"]) + "-" + Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["semester"]) + "-" + Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["period1"]);

                    if (!ht_bell.Contains(Convert.ToString(degree_var)))
                    {
                        vari = ds_attndmaster.Tables[2].Rows[pcont]["start_time"] + "," + ds_attndmaster.Tables[2].Rows[pcont]["end_time"];
                        ht_bell.Add(degree_var, Convert.ToString(vari));
                    }

                }
            }

            ht_period.Clear();

            if (ds_attndmaster.Tables[3].Rows.Count > 0)
            {
                for (int pcont = 0; pcont < ds_attndmaster.Tables[3].Rows.Count; pcont++)
                {
                    degree_var = Convert.ToString(ds_attndmaster.Tables[3].Rows[pcont]["lock_hr"]);

                    if (!ht_period.Contains(Convert.ToString(degree_var)))
                    {
                        vari = ds_attndmaster.Tables[3].Rows[pcont]["markatt_from"] + "," + ds_attndmaster.Tables[3].Rows[pcont]["markatt_to"];
                        ht_period.Add(degree_var, Convert.ToString(vari));
                    }
                }
            }

            hr_lock = false;


            if (ds_attndmaster.Tables[4].Rows.Count > 0)
            {
                string locktrue = ds_attndmaster.Tables[4].Rows[0]["hrlock"].ToString();
                if (locktrue == "1")
                {
                    hr_lock = true;
                }
            }


            string degreename = "";
            Hashtable hatdegreename = new Hashtable();

            for (int i = 0; i < ds_attndmaster.Tables[5].Rows.Count; i++)
            {
                if (!hatdegreename.Contains(ds_attndmaster.Tables[5].Rows[i]["Degree_Code"].ToString()))
                {
                    hatdegreename.Add(ds_attndmaster.Tables[5].Rows[i]["Degree_Code"].ToString(), ds_attndmaster.Tables[5].Rows[i]["course"].ToString() + '-' + ds_attndmaster.Tables[5].Rows[i]["dept_acronym"].ToString());
                }
            }

            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread1.Columns.Default.Font.Name = "Book Antiqua";
            FpSpread1.Columns.Default.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.Font.Bold = true;
            darkstyle.ForeColor = ColorTranslator.FromHtml("White");//n
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.ActiveSheetView.RowHeader.DefaultStyle = darkstyle;
            FpSpread1.ActiveSheetView.SheetCorner.DefaultStyle = darkstyle;
            FpSpread1.Sheets[0].ColumnCount = 2;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Date";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Details";

            date1 = txt_fromdate.Text.ToString();
            date2 = txt_todate.Text.ToString();
            string[] split = date1.Split('/');
            string[] split1 = date2.Split('/');
            DateTime ddf1 = Convert.ToDateTime(split[1] + '/' + split[0] + '/' + split[2]);
            DateTime ddt1 = Convert.ToDateTime(split1[1] + '/' + split1[0] + '/' + split1[2]);

            string ddf = Convert.ToString(ddf1);
            string ddt = Convert.ToString(ddt1);

            if (Convert.ToInt16(split[0].ToString()) <= 31 && Convert.ToInt16(split[1].ToString()) <= 12 && Convert.ToInt16(split[2].ToString()) <= Convert.ToInt16(Session["curr_year"]))
            {
                if (Convert.ToInt16(split1[0].ToString()) <= 31 && Convert.ToInt16(split1[1].ToString()) <= 12 && Convert.ToInt16(split1[2].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                {
                    long days = -1;
                    DateTime dt1 = DateTime.Now.AddDays(-6);
                    DateTime dt2 = DateTime.Now;
                    try
                    {
                        dt1 = Convert.ToDateTime(ddf);
                        dt2 = Convert.ToDateTime(ddt);
                        TimeSpan t = dt2.Subtract(dt1);
                        days = t.Days;
                    }
                    catch
                    {
                        try
                        {
                            dt1 = Convert.ToDateTime(date1);
                            dt2 = Convert.ToDateTime(date2);
                            TimeSpan t = dt2.Subtract(dt1);
                            days = t.Days;
                        }
                        catch
                        {
                            lbl_staf.Text = ddf + ddt;
                        }
                    }
                    if (days < 0)
                    {
                        lbl_staf.Visible = true;
                        FpSpread1.Visible = false;

                        return;
                    }
                    if (days >= 0)
                    {
                        lbl_staf.Visible = false;
                        FpSpread1.Visible = true;

                        string[] differdays = new string[days];
                        noofhrs = 0;
                        if (ds_attndmaster.Tables[6].Rows.Count > 0)
                        {
                            if (ds_attndmaster.Tables[6].Rows[0]["noofhours"].ToString().Trim() != "" && ds_attndmaster.Tables[6].Rows[0]["noofhours"].ToString().Trim() != null && ds_attndmaster.Tables[6].Rows[0]["noofhours"].ToString().Trim() != "0")
                            {
                                noofhrs = Convert.ToInt32(ds_attndmaster.Tables[6].Rows[0]["noofhours"].ToString());
                            }
                        }

                        if (noofhrs != 0)
                        {
                            for (int i = 1; i <= noofhrs; i++)
                            {
                                FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Period " + Convert.ToString(i);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = i.ToString();
                                if ((Session["StafforAdmin"] == "Staff") || (Session["StafforAdmin"] == "Admin"))
                                {
                                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Locked = true;
                                }
                            }
                            for (int row_inc = 0; row_inc <= days; row_inc++)
                            {
                                FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 2;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Text = dt2.AddDays(-row_inc).ToString("d-MM-yyyy");
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 1].Text = "Class / Batch";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Room";
                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 2, 0, 2, 1);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 1].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                            }
                            sql1 = "";
                            Strsql = "";
                            SqlFinal = "";


                            string stafcode = string.Empty;
                            if (staff_code_value.Trim() == "")
                            {
                                if (txt_staffcodesearch.Text.Trim() != "")
                                {
                                    stafcode = Convert.ToString(txt_staffcodesearch.Text);
                                }
                                else if (txt_staffnamesearch.Text.Trim() != "")
                                {
                                    stafcode = Convert.ToString(d2.GetFunction("select staff_code  from staffmaster where staff_name ='" + Convert.ToString(txt_staffnamesearch.Text) + "'"));
                                }
                                else
                                {
                                    string stafnamecode = ddl_staffcode.SelectedItem.ToString();
                                    stafcode = ddl_staffcode.SelectedValue.ToString();
                                }
                            }
                            else
                            {
                                stafcode = Session["Staff_Code"].ToString();
                            }
                            Session["staffnewcodevalue"] = Convert.ToString(stafcode);

                            for (int day_lp = 0; day_lp < 7; day_lp++)
                            {
                                strday = Days[day_lp].ToString();
                                sql1 = sql1 + "(";
                                tmp_varstr = "";
                                for (int i_loop = 1; i_loop <= noofhrs; i_loop++)
                                {
                                    Strsql = Strsql + strday + Convert.ToString(i_loop) + ",";
                                    if (tmp_varstr == "")
                                    {
                                        tmp_varstr = tmp_varstr + strday + Convert.ToString(i_loop) + " like '%" + stafcode + "%'";//Modified by Manikandan 14/08/2013 from above comment line

                                    }
                                    else
                                    {
                                        tmp_varstr = tmp_varstr + " or " + strday + Convert.ToString(i_loop) + " like '%" + stafcode + "%'";//Modified by Manikandan 14/08/2013 from above comment line
                                    }
                                }
                                if (day_lp != 6)
                                    tmp_varstr = tmp_varstr + ") or ";
                                else
                                    tmp_varstr = tmp_varstr + ")";

                                sql1 = sql1 + tmp_varstr.ToString();
                            }


                            SqlPrefinal1 = "";
                            SqlPrefinal2 = "";
                            SqlPrefinal3 = "";
                            SqlPrefinal4 = "";

                            sql_s = "select semester_schedule.degree_code,(select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=semester_schedule.degree_code and semester=semester_schedule.semester), ";
                            sql_s = sql_s + Strsql + "";
                            SqlBatchYear = "(select distinct(registration.batch_year) from registration,semester_schedule where registration.degree_code=semester_schedule.degree_code and registration.cc=0 and delflag=0 and registration.exam_flag<>'DEBAR' AND registration.current_Semester = semester_schedule.semester)";
                            SqlPrefinal1 = sql_s + " semester,sections,batch_year,FromDate from semester_schedule where lastrec=1 and batch_year in " + SqlBatchYear + " and " + sql1 + " and semester=1 and degree_code in (select degree_code from degree where Exam_System= 'Semester' and First_Year_Nonsemester<>0)";
                            SqlPrefinal2 = sql_s + " semester,sections,batch_year,FromDate from semester_schedule where lastrec=1 and batch_year in " + SqlBatchYear + " and " + sql1 + " and degree_code in (select degree_code from degree where Exam_System='Semester' and First_Year_Nonsemester=0)";
                            SqlPrefinal3 = sql_s + " semester,sections,batch_year,FromDate from semester_schedule where lastrec=1 and  batch_year in " + SqlBatchYear + " and " + sql1 + " and degree_code in (select degree_code from degree where Exam_System='Non Semester') ";
                            SqlPrefinal4 = sql_s + " semester,sections,batch_year,FromDate from semester_schedule where lastrec=1 and  batch_year in " + SqlBatchYear + " and " + sql1 + " and semester<>1 and semester<>-1 and degree_code in (select degree_code from degree where Exam_System= 'Semester' and First_Year_Nonsemester<>0)";
                            SqlFinal = "(" + SqlPrefinal1 + ") union all (" + SqlPrefinal4 + ") union all (" + SqlPrefinal2 + ") union all (" + SqlPrefinal3 + ")";
                            SqlFinal = SqlFinal + " order by batch_year,degree_code,semester,sections,FromDate";



                            SqlFinal = "";
                            SqlFinal = " select distinct  r.degree_code,r.batch_year,s.semester,r.sections ,";
                            SqlFinal = SqlFinal + " (select distinct  (c.course_name+'-'+ dt.dept_acronym) from degree d,course c,department dt where d.course_id=c.course_id and d.dept_code=dt.dept_code and degree_code=s.degree_code) as degree";
                            SqlFinal = SqlFinal + ", (select distinct si.end_date from seminfo si where si.degree_code=s.degree_code and si.batch_year=s.batch_year and si.semester=s.semester) as end_date";
                            SqlFinal = SqlFinal + " from semester_schedule s,registration r where s.semester=r.current_semester and s.batch_year=r.batch_year and s.degree_code=r.degree_code and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' and s.sections=r.sections and ";
                            SqlFinal = SqlFinal + "(" + sql1 + ")";
                            SqlFinal = SqlFinal + " order by r.degree_code,r.batch_year,s.semester,r.sections";


                            SqlFinal = " select distinct r.Batch_Year,r.degree_code,sy.semester,r.Sections,si.end_date from staff_selector ss,Registration r,";
                            SqlFinal = SqlFinal + " subject s,sub_sem sm,syllabus_master sy,seminfo si where sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code";
                            SqlFinal = SqlFinal + " and sy.semester=r.Current_Semester and sy.syll_code=sm.syll_code and sm.subType_no=s.subType_no ";
                            SqlFinal = SqlFinal + " and s.subject_no=ss.subject_no and r.sections=ss.sections and ss.batch_year=r.Batch_Year";
                            SqlFinal = SqlFinal + " and si.Batch_Year=r.Batch_Year and si.degree_code=r.degree_code and si.semester=r.Current_Semester and ";
                            SqlFinal = SqlFinal + " si.Batch_Year=sy.Batch_Year and sy.degree_code=r.degree_code and si.semester=sy.Semester and r.CC=0 and r.Exam_Flag<>'debar'";
                            SqlFinal = SqlFinal + " and r.DelFlag=0 and ss.staff_code='" + stafcode + "'";


                            DataView dvalternaet = new DataView();
                            DataView dvsemster = new DataView();
                            DataView dvholiday = new DataView();
                            DataView dvdaily = new DataView();
                            DataView dvsubject = new DataView();
                            DataView dvsublab = new DataView();

                            string getalldetails = "select * from Alternate_Schedule where FromDate between '" + ddf + "' and '" + ddt + "' ; ";
                            getalldetails = getalldetails + "select * from Semester_Schedule order by FromDate desc; ";
                            getalldetails = getalldetails + "Select * from holidaystudents where holiday_date between '" + ddf + "' and '" + ddt + "' ; ";
                            getalldetails = getalldetails + "select * from dailyentdet de,dailystaffentry ds where de.lp_code=ds.lp_code and ds.sch_date between '" + ddf + "' and '" + ddt + "'  ; ";
                            getalldetails = getalldetails + " select distinct s.subject_no,s.subject_name,s.subject_code,sy.Batch_Year,sy.degree_code,sy.semester,sm.Lab from syllabus_master sy,sub_sem sm,subject s,staff_selector ss where sy.syll_code=sm.syll_code and sy.syll_code=s.syll_code and sm.syll_code=s.syll_code and sm.subType_no=s.subType_no and ss.subject_no=s.subject_no and ss.batch_year=sy.Batch_Year and ss.staff_code='" + stafcode + "' order by sy.Batch_Year,sy.degree_code,sy.semester ;";
                            getalldetails = getalldetails + " select distinct Current_Semester,Batch_Year,degree_code from Registration where cc=0 and delflag=0 and exam_flag<>'debar'; ";
                            getalldetails = getalldetails + " select no_of_hrs_I_half_day as mor,no_of_hrs_I_half_day as eve,degree_code,semester from periodattndschedule";
                            getalldetails = getalldetails + " select * from tbl_consider_day_order";
                            getalldetails = getalldetails + " select distinct r.Batch_Year,r.degree_code,r.Current_Semester,r.Sections,s.TTName,l.Day_Value,l.Hour_Value,l.Timetablename,l.auto_switch,COUNT(distinct l.Stu_Batch) as no_of_batch  from LabAlloc l,Registration r,Semester_Schedule s where r.Batch_Year=s.batch_year and r.degree_code=s.degree_code and r.Current_Semester=s.semester and r.Sections=s.Sections and r.Batch_Year=l.Batch_Year and r.degree_code=l.degree_code and r.Current_Semester=l.Semester and r.Sections=s.Sections and s.Batch_Year=l.Batch_Year and s.degree_code=l.degree_code and s.Semester=l.Semester and s.Sections=s.Sections and l.Timetablename=s.TTName and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and s.FromDate<='" + ddt + "' and l.auto_switch<>'' group by r.Batch_Year,r.degree_code,r.Current_Semester,r.Sections,s.TTName,l.Day_Value,l.Hour_Value,l.Timetablename,l.auto_switch order by r.Batch_Year,r.degree_code,r.Current_Semester,r.Sections";


                            DataSet dsall = da.select_method_wo_parameter(getalldetails, "Text");


                            string strstaffselector = "";
                            if (Session["StaffSelector"].ToString() == "1")
                            {
                                strstaffselector = " and s.staffcode='" + Session["Staff_Code"].ToString() + "'";
                            }

                            Hashtable hatholiday = new Hashtable();
                            DataSet dsperiod = da.select_method(SqlFinal, hat, "Text");
                            if (dsperiod.Tables[0].Rows.Count > 0)
                            {
                                for (int pre = 0; pre < dsperiod.Tables[0].Rows.Count; pre++)
                                {
                                    cur_camprevar = Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]);
                                    string getdate = "";
                                    if (Convert.ToString(tmp_camprevar.Trim()) != Convert.ToString(cur_camprevar.Trim()))
                                    {
                                        strsction = "";
                                        if ((Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "") && (Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "-1"))
                                        {
                                            strsction = " and sections='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                        }

                                        DataSet dsgetsub = da.select_method_wo_parameter("select distinct s.subject_no,s.subject_name,s.subject_code,sy.Batch_Year,sy.degree_code,sy.semester,sm.Lab from syllabus_master sy,sub_sem sm,subject s where sy.syll_code=sm.syll_code and sy.syll_code=s.syll_code and sm.syll_code=s.syll_code and sm.subType_no=s.subType_no and sy.degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and sy.Batch_Year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and sy.semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' order by sy.Batch_Year,sy.degree_code,sy.semester ", "Text");
                                        dsgetsub.Tables[0].DefaultView.RowFilter = " degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"] + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                        DataView dtcurlab = dsgetsub.Tables[0].DefaultView;
                                        Hashtable hatcurlab = new Hashtable();
                                        for (int cula = 0; cula < dtcurlab.Count; cula++)
                                        {
                                            string lasubno = dtcurlab[cula]["subject_no"].ToString();
                                            string labhour = dtcurlab[cula]["lab"].ToString();
                                            if (labhour.Trim() == "1" || labhour.Trim().ToLower() == "true")
                                            {
                                                if (!hatcurlab.Contains(lasubno))
                                                {
                                                    hatcurlab.Add(lasubno, lasubno);
                                                }
                                            }
                                        }

                                        string strsubstucount = " select count(distinct r.Roll_No) as stucount,r.Batch_Year,r.degree_code,r.Current_Semester,r.Sections,s.subject_no,r.adm_date from registration r,subjectchooser s where  r.roll_no=s.roll_no and  r.current_semester=s.semester";
                                        strsubstucount = strsubstucount + " and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"] + "' and  degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "'  and current_semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'  and cc=0 and delflag=0 and exam_flag<>'debar' " + strsction + " " + strstaffselector + "  group by r.Batch_Year,r.degree_code,r.Current_Semester,r.Sections,s.subject_no,r.adm_date";
                                        DataSet dssubstucount = da.select_method_wo_parameter(strsubstucount, "Text");
                                        DataView dvsubstucount = new DataView();

                                        hatholiday.Clear();
                                        dsall.Tables[2].DefaultView.RowFilter = " degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " ";
                                        DataView duholiday = dsall.Tables[2].DefaultView;
                                        for (int i = 0; i < duholiday.Count; i++)
                                        {
                                            if (!hatholiday.Contains(duholiday[i]["holiday_date"].ToString()))
                                            {
                                                hatholiday.Add(duholiday[i]["holiday_date"].ToString(), duholiday[i]["holiday_desc"].ToString());
                                            }
                                        }

                                        int frshlf = 0, schlf = 0;
                                        dsall.Tables[6].DefaultView.RowFilter = " degree_code ='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and  semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                        DataView dvperiod = dsall.Tables[6].DefaultView;
                                        if (dvperiod.Count > 0)
                                        {
                                            string morhr = dvperiod[0]["mor"].ToString();
                                            string evehr = dvperiod[0]["mor"].ToString();
                                            if (morhr != null && morhr.Trim() != "")
                                            {
                                                frshlf = Convert.ToInt32(morhr);
                                            }
                                            if (evehr != null && evehr.Trim() != "")
                                            {
                                                schlf = Convert.ToInt32(evehr);
                                            }
                                        }
                                        string getcurrent_sem = "";
                                        dsall.Tables[5].DefaultView.RowFilter = "degree_code ='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "'  and batch_year = '" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "'";
                                        DataView dvcurrsem = dsall.Tables[5].DefaultView;
                                        if (dvcurrsem.Count > 0)
                                        {
                                            getcurrent_sem = dvcurrsem[0]["current_semester"].ToString();
                                        }
                                        if (Convert.ToString(getcurrent_sem) == Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]))
                                        {
                                            string semenddate = dsperiod.Tables[0].Rows[pre]["end_date"].ToString();
                                            string altersetion = "";
                                            if (dsperiod.Tables[0].Rows[pre]["sections"].ToString() != "-1" && dsperiod.Tables[0].Rows[pre]["sections"].ToString() != null && dsperiod.Tables[0].Rows[pre]["sections"].ToString().Trim() != "")
                                            {
                                                altersetion = "and Sections='" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "'";
                                            }
                                            Hashtable hatdc = new Hashtable();
                                            dsall.Tables[7].DefaultView.RowFilter = "degree_code ='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "'  and batch_year = '" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "  ";
                                            DataView dvdayorderchanged = dsall.Tables[7].DefaultView;
                                            for (int dc = 0; dc < dvdayorderchanged.Count; dc++)
                                            {
                                                DateTime dtdcf = Convert.ToDateTime(dvdayorderchanged[dc]["from_date"].ToString());
                                                DateTime dtdct = Convert.ToDateTime(dvdayorderchanged[dc]["to_date"].ToString());
                                                for (DateTime dtc = dtdcf; dtc <= dtdct; dtc = dtc.AddDays(1))
                                                {
                                                    if (!hatdc.Contains(dtc))
                                                    {
                                                        hatdc.Add(dtc, dtc);
                                                    }
                                                }
                                            }
                                            int row_inc = 0;
                                            for (int srt = 0; srt <= days; srt++) //Date Loop
                                            {
                                                if (hatdegreename.Contains(dsperiod.Tables[0].Rows[pre]["degree_code"].ToString()))
                                                {
                                                    degreename = GetCorrespondingKey(dsperiod.Tables[0].Rows[pre]["degree_code"].ToString(), hatdegreename).ToString();
                                                }
                                                DateTime cur_day = new DateTime();
                                                cur_day = dt2.AddDays(-srt);
                                                if (!hatdc.Contains(cur_day))
                                                {
                                                    tmp_datevalue = Convert.ToString(cur_day);
                                                    degree_var = Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]);
                                                    string SchOrder = "";
                                                    string day_from = cur_day.ToString("yyyy-MM-dd");

                                                    DateTime schfromdate = cur_day;
                                                    dsall.Tables[1].DefaultView.RowFilter = "batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and FromDate<='" + cur_day.ToString() + "'";
                                                    dvsemster = dsall.Tables[1].DefaultView;
                                                    if (dvsemster.Count > 0)
                                                    {
                                                        getdate = dvsemster[0]["FromDate"].ToString();
                                                    }
                                                    else
                                                    {
                                                        getdate = "";
                                                    }
                                                    if (Convert.ToString(getdate) != "" && Convert.ToString(getdate).Trim() != "0" && Convert.ToString(getdate).Trim() != null)
                                                    {
                                                        DateTime getsche = Convert.ToDateTime(getdate);

                                                        if (Convert.ToDateTime(schfromdate) == Convert.ToDateTime(getsche) || Convert.ToDateTime(schfromdate) != Convert.ToDateTime(getsche))
                                                        {
                                                            if (ht_sch.Contains(Convert.ToString(degree_var)))
                                                            {
                                                                string contvar = Convert.ToString(GetCorrespondingKey(Convert.ToString(degree_var), ht_sch));
                                                                string[] sp_rd_semi = contvar.Split(',');
                                                                if (sp_rd_semi.GetUpperBound(0) >= 1)
                                                                {
                                                                    SchOrder = sp_rd_semi[0].ToString();
                                                                    noofdays = sp_rd_semi[1].ToString();
                                                                }
                                                            }

                                                            Dictionary<string, string> dicautoswitch = new Dictionary<string, string>();
                                                            dsall.Tables[8].DefaultView.RowFilter = " batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and Current_Semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and TTName='" + dvsemster[0]["ttname"].ToString() + "'";
                                                            DataView dvautoswitch = dsall.Tables[8].DefaultView;
                                                            for (int au = 0; au < dvautoswitch.Count; au++)
                                                            {
                                                                string autoswi = dvautoswitch[au]["Day_Value"].ToString() + dvautoswitch[au]["Hour_Value"].ToString();
                                                                if (!dicautoswitch.ContainsKey(autoswi))
                                                                {
                                                                    dicautoswitch.Add(autoswi, dvautoswitch[au]["auto_switch"].ToString() + '-' + dvautoswitch[au]["no_of_batch"].ToString());
                                                                }
                                                            }

                                                            degree_var = Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]);

                                                            if (ht_sdate.Contains(Convert.ToString(degree_var)))
                                                            {
                                                                string contvar = Convert.ToString(GetCorrespondingKey(Convert.ToString(degree_var), ht_sdate));
                                                                string[] sp_rd_semi = contvar.Split(',');
                                                                if (sp_rd_semi.GetUpperBound(0) >= 1)
                                                                {
                                                                    start_datesem = sp_rd_semi[0].ToString();
                                                                    start_dayorder = sp_rd_semi[1].ToString();
                                                                }

                                                            }
                                                            if (noofdays.ToString().Trim() == "")
                                                            {
                                                                goto lb1;
                                                            }
                                                            Day_Order = "";
                                                            if (SchOrder == "1")
                                                            {
                                                                strday = cur_day.ToString("ddd"); //Week Dayorder
                                                                Day_Order = "0-" + Convert.ToString(strday);
                                                                //FpSpread1.Sheets[0].RowHeader.Cells[row_inc, 0].Text = cur_day.ToString("d-MM-yyyy") + " (" + cur_day.DayOfWeek.ToString() + ")";
                                                                FpSpread1.Sheets[0].Cells[row_inc, 0].Text = cur_day.ToString("d-MM-yyyy") + " (" + cur_day.DayOfWeek.ToString() + ")";
                                                            }
                                                            else
                                                            {
                                                                string[] sps = dt2.ToString().Split('/');
                                                                string curdate = sps[0] + '/' + sps[1] + '/' + sps[2];
                                                                strday = da.findday(cur_day.ToString(), dsperiod.Tables[0].Rows[pre]["degree_code"].ToString(), dsperiod.Tables[0].Rows[pre]["semester"].ToString(), dsperiod.Tables[0].Rows[pre]["batch_year"].ToString(), start_datesem.ToString(), noofdays.ToString(), start_dayorder);
                                                                Day_Order = "0-" + Convert.ToString(strday);
                                                                string Dateorder = "";
                                                                // if (cur_day.DayOfWeek.ToString().ToString().Trim().ToLower() != "sunday")
                                                                {
                                                                    if (strday == "mon")
                                                                    {
                                                                        Dateorder = "I - Day";
                                                                    }
                                                                    else if (strday == "tue")
                                                                    {
                                                                        Dateorder = "II - Day";
                                                                    }
                                                                    else if (strday == "wed")
                                                                    {
                                                                        Dateorder = "III - Day";
                                                                    }
                                                                    else if (strday == "thu")
                                                                    {
                                                                        Dateorder = "IV - Day";
                                                                    }
                                                                    else if (strday == "fri")
                                                                    {
                                                                        Dateorder = "V - Day";
                                                                    }
                                                                    else if (strday == "sat")
                                                                    {
                                                                        Dateorder = "VI - Day";
                                                                    }
                                                                }
                                                                FpSpread1.Sheets[0].Cells[row_inc, 0].Text = Dateorder;
                                                            }
                                                            if (strday.ToString().Trim() == "")
                                                            {
                                                                goto lb1;
                                                            }
                                                            //==check holiday
                                                            string reasonsun = "";
                                                            if (hatholiday.Contains(cur_day.ToString()))
                                                            {
                                                                reasonsun = GetCorrespondingKey(cur_day.ToString(), hatholiday).ToString();
                                                                if (reasonsun.Trim().ToLower() == "sunday")
                                                                {
                                                                    FpSpread1.Sheets[0].SpanModel.Add((row_inc), 0, 1, (FpSpread1.Sheets[0].ColumnCount));
                                                                    FpSpread1.Sheets[0].Cells[(row_inc), 0].Text = "Sunday Holiday";
                                                                    FpSpread1.Sheets[0].Cells[(row_inc), 0].Tag = "Selected day is Holiday- Reason-" + reasonsun;
                                                                    FpSpread1.Sheets[0].Cells[(row_inc), 0].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[(row_inc), 0].ForeColor = Color.Red;
                                                                    FpSpread1.Sheets[0].Rows[(row_inc)].Locked = true;
                                                                }
                                                            }
                                                            if (!hatholiday.Contains(cur_day.ToString()) || reasonsun.Trim().ToLower() != "sunday")
                                                            {
                                                                string str_day = strday;
                                                                string Atmonth = cur_day.Month.ToString();
                                                                string Atyear = cur_day.Year.ToString();
                                                                long strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);

                                                                sql1 = "";
                                                                Strsql = "";

                                                                for (int i_loop = 1; i_loop <= noofhrs; i_loop++)
                                                                {
                                                                    Strsql = Strsql + strday + Convert.ToString(i_loop) + ",";
                                                                    if (sql1 == "")
                                                                    {
                                                                        sql1 = sql1 + strday + Convert.ToString(i_loop) + " like '%" + stafcode + "%'";//Modified by Manikandan 14/08/2013 from above comment line

                                                                    }
                                                                    else
                                                                    {
                                                                        sql1 = sql1 + " or " + strday + Convert.ToString(i_loop) + " like '%" + stafcode + "%'";//Modified by Manikandan 14/08/2013 from above comment line
                                                                    }
                                                                }

                                                                string day_aten = cur_day.Day.ToString();
                                                                //Boolean check_hour = false;


                                                                string strsectionvar = "";
                                                                string labsection = "";
                                                                if (Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "" && Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "-1")
                                                                {
                                                                    strsectionvar = " and sections='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                                                    labsection = " and sections='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                                                }
                                                                sql1 = " and (" + sql1 + ")";
                                                                dsall.Tables[0].DefaultView.RowFilter = "degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and fromdate='" + day_from + "'";
                                                                dvalternaet = dsall.Tables[0].DefaultView;
                                                                string text_temp = "";
                                                                int temp = 0;
                                                                text_temp = "";
                                                                string getcolumnfield = "";

                                                                Boolean moringleav = false;
                                                                Boolean evenleave = false;
                                                                dsall.Tables[2].DefaultView.RowFilter = "holiday_date='" + cur_day.ToString("MM/dd/yyyy") + "' and degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                                                dvholiday = dsall.Tables[2].DefaultView;

                                                                if (dvholiday.Count > 0)
                                                                {
                                                                    if (!hatholiday.Contains(cur_day.ToString()))
                                                                    {
                                                                        hatholiday.Add(cur_day.ToString(), dvholiday[0]["holiday_desc"].ToString());
                                                                    }
                                                                    if (dvholiday[0]["morning"].ToString() == "1" || dvholiday[0]["morning"].ToString().Trim().ToLower() == "true")
                                                                    {
                                                                        moringleav = true;
                                                                    }
                                                                    if (dvholiday[0]["evening"].ToString() == "1" || dvholiday[0]["evening"].ToString().Trim().ToLower() == "true")
                                                                    {
                                                                        evenleave = true;
                                                                    }
                                                                    if (dvholiday[0]["halforfull"].ToString() == "0" || dvholiday[0]["halforfull"].ToString().Trim().ToLower() == "false")
                                                                    {
                                                                        evenleave = true;
                                                                        moringleav = true;
                                                                    }
                                                                }
                                                                for (temp = 1; temp <= noofhrs; temp++)
                                                                {
                                                                    string sp_rd = "";
                                                                    Boolean altfalg = false;
                                                                    if (dvalternaet.Count > 0)
                                                                    {
                                                                        sp_rd = dvalternaet[0]["" + strday.Trim() + temp + ""].ToString();

                                                                        if (hatdegreename.Contains(dvalternaet[0]["degree_code"].ToString()))
                                                                        {
                                                                            degreename = GetCorrespondingKey(dvalternaet[0]["degree_code"].ToString(), hatdegreename).ToString();
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        sp_rd = "";
                                                                    }
                                                                    if (sp_rd.Trim() != "" && sp_rd.Trim() != "0" && sp_rd != null)
                                                                    {
                                                                        altfalg = true;
                                                                        string[] sp_rd_split = sp_rd.Split(';');
                                                                        for (int index = 0; index <= sp_rd_split.GetUpperBound(0); index++)
                                                                        {
                                                                            string[] sp2 = sp_rd_split[index].Split(new Char[] { '-' });
                                                                            if (sp2.GetUpperBound(0) >= 1)
                                                                            {
                                                                                int upperbound = sp2.GetUpperBound(0);
                                                                                for (int multi_staff = 1; multi_staff < sp2.GetUpperBound(0); multi_staff++)
                                                                                {
                                                                                    if (sp2[multi_staff] == stafcode)
                                                                                    {
                                                                                        //==============================theroy batch=======================================
                                                                                        Boolean checklabhr = false;
                                                                                        for (int sr = 0; sr <= sp_rd_split.GetUpperBound(0); sr++)
                                                                                        {
                                                                                            string[] getlasub = sp_rd_split[sr].ToString().Split('-');
                                                                                            if (getlasub.GetUpperBound(0) > 1)
                                                                                            {
                                                                                                string srllab = getlasub[0].ToString();
                                                                                                if (hatcurlab.Contains(srllab))
                                                                                                {
                                                                                                    checklabhr = true;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        //======================================================================

                                                                                        string sect = dsperiod.Tables[0].Rows[pre]["sections"].ToString();

                                                                                        if (sect != "-1" && sect != null && sect.Trim() != "")
                                                                                        {
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            sect = "";
                                                                                        }

                                                                                        if (semenddate.Trim() != "" && semenddate.Trim() != null && semenddate.Trim() != "0")
                                                                                        {
                                                                                            if (cur_day <= (Convert.ToDateTime(semenddate)))
                                                                                            {
                                                                                                double Num;
                                                                                                bool isNum = double.TryParse(sp2[0].ToString(), out Num);
                                                                                                if (isNum)
                                                                                                {
                                                                                                    if (checklabhr == false)
                                                                                                    {
                                                                                                        dsall.Tables[4].DefaultView.RowFilter = " subject_no=" + sp2[0] + "";
                                                                                                        dvsubject = dsall.Tables[4].DefaultView;
                                                                                                        if (dvsubject.Count > 0)
                                                                                                        {
                                                                                                            text_temp = dvsubject[0]["subject_name"].ToString() + "-S";
                                                                                                        }
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        dsall.Tables[4].DefaultView.RowFilter = " subject_no=" + sp2[0] + "";
                                                                                                        dvsubject = dsall.Tables[4].DefaultView;
                                                                                                        if (dvsubject.Count > 0)
                                                                                                        {
                                                                                                            text_temp = dvsubject[0]["subject_name"].ToString() + "-L";
                                                                                                        }
                                                                                                    }
                                                                                                    string Schedule_string = "";

                                                                                                    if (dsperiod.Tables[0].Rows[pre]["sections"].ToString() == "-1" || dsperiod.Tables[0].Rows[pre]["sections"].ToString() == "" || dsperiod.Tables[0].Rows[pre]["sections"].ToString() == null)
                                                                                                    {
                                                                                                        if (checklabhr == false)
                                                                                                        {
                                                                                                            Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-S-0"; //+ sp_rd_semi.GetUpperBound(0);
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-L-0"; //+ sp_rd_semi.GetUpperBound(0);
                                                                                                        }
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        if (checklabhr == false)
                                                                                                        {
                                                                                                            Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-S-0";// +sp_rd_semi.GetUpperBound(0);
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-L-0";// +sp_rd_semi.GetUpperBound(0);
                                                                                                        }
                                                                                                    }
                                                                                                    Boolean allowleave = false;
                                                                                                    if (hatholiday.Contains(cur_day.ToString()))
                                                                                                    {
                                                                                                        if (moringleav == true)
                                                                                                        {
                                                                                                            if (frshlf >= temp)
                                                                                                            {
                                                                                                                allowleave = true;
                                                                                                            }
                                                                                                        }
                                                                                                        if (evenleave == true)
                                                                                                        {
                                                                                                            if (temp > frshlf)
                                                                                                            {
                                                                                                                allowleave = true;
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                    if (allowleave == true)
                                                                                                    {
                                                                                                        if (hatholiday.Contains(cur_day.ToString()))
                                                                                                        {
                                                                                                            string holidayreason = GetCorrespondingKey(cur_day.ToString(), hatholiday).ToString();
                                                                                                            if (FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Text.Trim() == "")
                                                                                                            {
                                                                                                                FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Text = "Selected day is Holiday- Reason-" + holidayreason + "-" + text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Tag = "Selected day is Holiday- Reason-" + holidayreason + "-" + Schedule_string.ToString() + "-alter";
                                                                                                                if (Convert.ToString(Day_Order).Trim() != "")
                                                                                                                {
                                                                                                                    FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Note = Convert.ToString(Day_Order);
                                                                                                                }
                                                                                                                FpSpread1.Sheets[0].Cells[(row_inc), temp + 1].ForeColor = Color.Blue;
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Text = FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Text + " * " + "Selected day is Holiday- Reason-" + holidayreason + "-" + text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Tag = FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Tag + " * " + "Selected day is Holiday- Reason-" + holidayreason + "-" + Schedule_string.ToString() + "-alter";
                                                                                                                if (Convert.ToString(Day_Order).Trim() != "")
                                                                                                                {
                                                                                                                    FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Note = Convert.ToString(Day_Order);
                                                                                                                }
                                                                                                                FpSpread1.Sheets[0].Cells[(row_inc), temp + 1].ForeColor = Color.Blue;
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        if (FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Text.Trim() == "")
                                                                                                        {
                                                                                                            string getroomname = "";
                                                                                                            if (Schedule_string.Trim() != "")
                                                                                                            {
                                                                                                                string[] split_schedule = Schedule_string.Split('-');
                                                                                                                string getroomname1 = d2.GetFunction("select Room_Name  from SubwiseRoomAllot where Batch_Year ='" + Convert.ToString(split_schedule[3]) + "' and Degree_Code ='" + Convert.ToString(split_schedule[0]) + "' and Semester ='" + Convert.ToString(split_schedule[1]) + "' and Subject_No ='" + Convert.ToString(split_schedule[2]) + "'");
                                                                                                                if (getroomname1.Trim() != "" && getroomname1.Trim() != "0")
                                                                                                                {
                                                                                                                    getroomname = getroomname1;
                                                                                                                }
                                                                                                            }
                                                                                                            FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Text = text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect;
                                                                                                            FpSpread1.Sheets[0].Cells[row_inc + 1, temp + 1].Text = getroomname;
                                                                                                            FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Tag = Schedule_string.ToString() + "-alter";
                                                                                                            if (Convert.ToString(Day_Order).Trim() != "")
                                                                                                            {
                                                                                                                FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Note = Convert.ToString(Day_Order);
                                                                                                            }
                                                                                                            FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Font.Bold = true;
                                                                                                            FpSpread1.Sheets[0].Cells[row_inc + 1, temp + 1].Font.Bold = true;
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            string getroomname = "";
                                                                                                            if (Schedule_string.Trim() != "")
                                                                                                            {
                                                                                                                string[] split_schedule = Schedule_string.Split('-');
                                                                                                                string getroomname1 = d2.GetFunction("select Room_Name  from SubwiseRoomAllot where Batch_Year ='" + Convert.ToString(split_schedule[3]) + "' and Degree_Code ='" + Convert.ToString(split_schedule[0]) + "' and Semester ='" + Convert.ToString(split_schedule[1]) + "' and Subject_No ='" + Convert.ToString(split_schedule[2]) + "'");
                                                                                                                if (getroomname1.Trim() != "" && getroomname1.Trim() != "0")
                                                                                                                {
                                                                                                                    getroomname = getroomname1;
                                                                                                                }
                                                                                                            }
                                                                                                            string tmpvar = "";
                                                                                                            string istemp = "";
                                                                                                            istemp = FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Text.ToString();
                                                                                                            tmpvar = text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                            if (Convert.ToString(istemp) != Convert.ToString(tmpvar))
                                                                                                            {
                                                                                                                FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Text = FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Text + " * " + text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + da.GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "") + "-" + da.GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "") + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect;
                                                                                                                FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Tag = FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Tag + " * " + Schedule_string.ToString() + "-alter";
                                                                                                                if (Convert.ToString(Day_Order).Trim() != "")
                                                                                                                {
                                                                                                                    FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Note = Convert.ToString(Day_Order);
                                                                                                                }
                                                                                                            }
                                                                                                            FpSpread1.Sheets[0].Cells[row_inc + 1, temp + 1].Text = getroomname;
                                                                                                        }
                                                                                                        dailyentryflag = false;
                                                                                                        attendanceentryflag = false;
                                                                                                        FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Font.Bold = true;
                                                                                                        FpSpread1.Sheets[0].Cells[row_inc + 1, temp + 1].Font.Bold = true;

                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    if (altfalg == false)
                                                                    {
                                                                        getcolumnfield = Convert.ToString(strday + temp);
                                                                        attendanceentryflag = false;
                                                                        dailyentryflag = false;
                                                                        if (dvsemster.Count > 0)
                                                                        {
                                                                            if (dvsemster[0][getcolumnfield].ToString() != "" && dvsemster[0][getcolumnfield].ToString() != null && dvsemster[0][getcolumnfield].ToString() != "\0")
                                                                            {
                                                                                string timetable = "";
                                                                                string name = dvsemster[0]["ttname"].ToString();
                                                                                if (name != null && name.Trim() != "")
                                                                                {
                                                                                    timetable = name;
                                                                                }
                                                                                sp_rd = dvsemster[0][getcolumnfield].ToString();
                                                                                string[] sp_rd_semi = sp_rd.Split(';');

                                                                                for (int semi = 0; semi <= sp_rd_semi.GetUpperBound(0); semi++)
                                                                                {
                                                                                    string[] sp2 = sp_rd_semi[semi].Split(new Char[] { '-' });
                                                                                    if (sp2.GetUpperBound(0) >= 1)
                                                                                    {
                                                                                        int upperbound = sp2.GetUpperBound(0);
                                                                                        for (int multi_staff = 1; multi_staff < sp2.GetUpperBound(0); multi_staff++)
                                                                                        {
                                                                                            if (sp2[multi_staff] == stafcode)
                                                                                            {
                                                                                                Boolean checklabhr = false;
                                                                                                for (int sr = 0; sr <= sp_rd_semi.GetUpperBound(0); sr++)
                                                                                                {
                                                                                                    string[] getlasub = sp_rd_semi[sr].ToString().Split('-');
                                                                                                    if (getlasub.GetUpperBound(0) > 1)
                                                                                                    {
                                                                                                        string srllab = getlasub[0].ToString();
                                                                                                        if (hatcurlab.Contains(srllab))
                                                                                                        {
                                                                                                            checklabhr = true;
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                                string sect = dsperiod.Tables[0].Rows[pre]["sections"].ToString();
                                                                                                if (sect == "-1" || sect == null || sect.Trim() == "")
                                                                                                {
                                                                                                    sect = "";
                                                                                                }
                                                                                                if (semenddate.Trim() != "" && semenddate.Trim() != null && semenddate.Trim() != "0")
                                                                                                {
                                                                                                    if (cur_day <= (Convert.ToDateTime(semenddate)))
                                                                                                    {
                                                                                                        double Num;
                                                                                                        bool isNum = double.TryParse(sp2[0].ToString(), out Num);
                                                                                                        if (isNum)
                                                                                                        {
                                                                                                            if (checklabhr == false)
                                                                                                            {
                                                                                                                dsall.Tables[4].DefaultView.RowFilter = " subject_no=" + sp2[0] + "";
                                                                                                                dvsubject = dsall.Tables[4].DefaultView;
                                                                                                                if (dvsubject.Count > 0)
                                                                                                                {
                                                                                                                    text_temp = dvsubject[0]["subject_name"].ToString() + "-S";
                                                                                                                }
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                dsall.Tables[4].DefaultView.RowFilter = " subject_no=" + sp2[0] + "";
                                                                                                                dvsubject = dsall.Tables[4].DefaultView;
                                                                                                                if (dvsubject.Count > 0)
                                                                                                                {
                                                                                                                    text_temp = dvsubject[0]["subject_name"].ToString() + "-S";
                                                                                                                }
                                                                                                            }
                                                                                                            string Schedule_string = "";
                                                                                                            if (dsperiod.Tables[0].Rows[pre]["sections"].ToString() == "-1" || dsperiod.Tables[0].Rows[pre]["sections"].ToString() == "" || dsperiod.Tables[0].Rows[pre]["sections"].ToString() == null)
                                                                                                            {
                                                                                                                if (checklabhr == false)
                                                                                                                {
                                                                                                                    Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-S-" + sp_rd_semi.GetUpperBound(0);
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-L-" + sp_rd_semi.GetUpperBound(0);
                                                                                                                }
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                if (checklabhr == false)
                                                                                                                {
                                                                                                                    Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-S-" + sp_rd_semi.GetUpperBound(0);
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-L-" + sp_rd_semi.GetUpperBound(0);
                                                                                                                }
                                                                                                            }

                                                                                                            Boolean allowleave = false;
                                                                                                            if (hatholiday.Contains(cur_day.ToString()))
                                                                                                            {
                                                                                                                if (moringleav == true)
                                                                                                                {
                                                                                                                    if (frshlf >= temp)
                                                                                                                    {
                                                                                                                        allowleave = true;
                                                                                                                    }
                                                                                                                }
                                                                                                                if (evenleave == true)
                                                                                                                {
                                                                                                                    if (temp > frshlf)
                                                                                                                    {
                                                                                                                        allowleave = true;
                                                                                                                    }
                                                                                                                }
                                                                                                            }

                                                                                                            if (allowleave == true)
                                                                                                            {
                                                                                                                if (hatholiday.Contains(cur_day.ToString()))
                                                                                                                {
                                                                                                                    string holidayreason = GetCorrespondingKey(cur_day.ToString(), hatholiday).ToString();
                                                                                                                    if (FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Text.Trim() == "")
                                                                                                                    {
                                                                                                                        FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Text = "Selected day is Holiday- Reason-" + holidayreason + "-" + text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                        FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Tag = "Selected day is Holiday- Reason-" + holidayreason + "-" + Schedule_string.ToString() + "-sem";
                                                                                                                        if (Convert.ToString(Day_Order).Trim() != "")
                                                                                                                        {
                                                                                                                            FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Note = Convert.ToString(Day_Order);
                                                                                                                        }
                                                                                                                        FpSpread1.Sheets[0].Cells[(row_inc), temp + 1].ForeColor = Color.Blue;
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Text = FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Text + '*' + "Selected day is Holiday- Reason-" + holidayreason + "-" + text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                        FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Tag = FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Tag.ToString() + '*' + "Selected day is Holiday- Reason-" + holidayreason + "-" + Schedule_string.ToString() + "-sem";
                                                                                                                        if (Convert.ToString(Day_Order).Trim() != "")
                                                                                                                        {
                                                                                                                            FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Note = Convert.ToString(Day_Order);
                                                                                                                        }
                                                                                                                        FpSpread1.Sheets[0].Cells[(row_inc), temp + 1].ForeColor = Color.Blue;
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                if (FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Text.Trim() == "") // nisha
                                                                                                                {
                                                                                                                    string getroomname = "";
                                                                                                                    if (Schedule_string.Trim() != "")
                                                                                                                    {
                                                                                                                        string[] split_schedule = Schedule_string.Split('-');
                                                                                                                        string getroomname1 = d2.GetFunction("select Room_Name  from SubwiseRoomAllot where Batch_Year ='" + Convert.ToString(split_schedule[3]) + "' and Degree_Code ='" + Convert.ToString(split_schedule[0]) + "' and Semester ='" + Convert.ToString(split_schedule[1]) + "' and Subject_No ='" + Convert.ToString(split_schedule[2]) + "'");
                                                                                                                        if (getroomname1.Trim() != "" && getroomname1.Trim() != "0")
                                                                                                                        {
                                                                                                                            getroomname = getroomname1;
                                                                                                                        }
                                                                                                                    }
                                                                                                                    FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Text = text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect;
                                                                                                                    FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Tag = Schedule_string.ToString() + "-sem";
                                                                                                                    FpSpread1.Sheets[0].Cells[row_inc + 1, temp + 1].Text = getroomname;
                                                                                                                    if (Convert.ToString(Day_Order).Trim() != "")
                                                                                                                    {
                                                                                                                        FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Note = Convert.ToString(Day_Order);
                                                                                                                    }
                                                                                                                    FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Font.Bold = true;
                                                                                                                    FpSpread1.Sheets[0].Cells[row_inc + 1, temp + 1].Font.Bold = true;
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    string getroomname = "";
                                                                                                                    if (Schedule_string.Trim() != "")
                                                                                                                    {
                                                                                                                        string[] split_schedule = Schedule_string.Split('-');
                                                                                                                        string getroomname1 = d2.GetFunction("select Room_Name  from SubwiseRoomAllot where Batch_Year ='" + Convert.ToString(split_schedule[3]) + "' and Degree_Code ='" + Convert.ToString(split_schedule[0]) + "' and Semester ='" + Convert.ToString(split_schedule[1]) + "' and Subject_No ='" + Convert.ToString(split_schedule[2]) + "'");
                                                                                                                        if (getroomname1.Trim() != "" && getroomname1.Trim() != "0")
                                                                                                                        {
                                                                                                                            getroomname = getroomname1;
                                                                                                                        }
                                                                                                                    }
                                                                                                                    if (FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Text.Trim() != "")
                                                                                                                    {
                                                                                                                        string tmpvar = "";
                                                                                                                        string istemp = "";


                                                                                                                        istemp = FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Text.ToString();
                                                                                                                        tmpvar = text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                        if (Convert.ToString(istemp) != Convert.ToString(tmpvar))
                                                                                                                        {
                                                                                                                            FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Text = FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Text + " * " + text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect;
                                                                                                                            FpSpread1.Sheets[0].Cells[row_inc + 1, temp + 1].Text = getroomname;
                                                                                                                            FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Tag = FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Tag + " * " + Schedule_string.ToString() + "-sem";
                                                                                                                            if (Convert.ToString(Day_Order).Trim() != "")
                                                                                                                            {
                                                                                                                                FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Note = Convert.ToString(Day_Order);
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Text = text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect;
                                                                                                                        FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Tag = Schedule_string.ToString() + "-sem";
                                                                                                                        FpSpread1.Sheets[0].Cells[row_inc + 1, temp + 1].Text = getroomname;
                                                                                                                        if (Convert.ToString(Day_Order).Trim() != "")
                                                                                                                        {
                                                                                                                            FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Note = Convert.ToString(Day_Order);
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                                FpSpread1.Sheets[0].Cells[row_inc, temp + 1].Font.Bold = true;
                                                                                                                FpSpread1.Sheets[0].Cells[row_inc + 1, temp + 1].Font.Bold = true;
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                row_inc = row_inc + 2;
                                            }
                                        }

                                    }
                                lb1: tmp_camprevar = Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]);

                                }
                            }
                        }
                    }

                    FpSpread1.Sheets[0].AutoPostBack = true;

                    FpSpread1.SaveChanges();
                    FpSpread1.Visible = true;
                    div1.Visible = true;
                    div_report.Visible = true;
                    lbl_staf.Visible = false;
                    FpSpread1.Sheets[0].Columns.Default.Width = 300;
                }
            }
            if (FpSpread1.Sheets[0].RowCount > 0)
                FpSpread1.Sheets[0].PageSize = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);
            else
            {
                FpSpread1.Visible = false;
                div_report.Visible = false;
            }
            FpSpread1.SaveChanges();
        }
        catch (Exception ex)
        {
            lbl_staf.Visible = true;
            lbl_staf.Text = ex.ToString();
        }
    }

    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {

        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value.ToString();
            }
        }

        return null;
    }
    public void bindstaff()
    {
        try
        {
            string strquerytext = "select distinct staff_name,m.staff_code from staffmaster m,stafftrans t,hrdept_master h,desig_master d,staff_selector st where m.resign<>1 and m.settled<>1 and m.staff_code = t.staff_code and t.dept_code = h.dept_code and t.desig_code = d.desig_code and latestrec = 1 and st.staff_code=m.staff_code and m.college_code = " + Session["collegecode"] + " and staff_name<>'' order by staff_name";
            ds = d2.select_method(strquerytext, hat, "Text");

            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddl_staffcode.DataSource = ds;
                ddl_staffcode.DataTextField = "staff_code";
                ddl_staffcode.DataValueField = "staff_code";
                ddl_staffcode.DataBind();

                ddl_staffname.DataSource = ds;
                ddl_staffname.DataTextField = "staff_name";
                ddl_staffname.DataValueField = "staff_code";
                ddl_staffname.DataBind();
            }
        }
        catch
        {
        }

    }
    public void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {

            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {
                //  FpSpread1.Sheets[0].Columns[1].Visible = false;
                d2.printexcelreport(FpSpread1, report);
                lbl_norec.Visible = false;
            }
            else
            {
                lbl_norec.Text = "Please Enter Your Report Name";
                lbl_norec.Visible = true;
            }
            btn_Excel.Focus();
        }

        catch (Exception ex)
        {
            lbl_norec.Text = ex.ToString();
        }
    }
    public void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string selectquery = "select Staff_name,h.dept_name from staffmaster m,hrdept_master h ,staff_appl_master a where a.appl_no =m.appl_no and a.dept_code =h.dept_code and m.settled =0 and m.resign =0 and m.staff_code ='" + Session["staffnewcodevalue"] + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            string staffname = "";
            string deptname = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                staffname = Convert.ToString(ds.Tables[0].Rows[0]["Staff_name"]);
                deptname = Convert.ToString(ds.Tables[0].Rows[0]["dept_name"]);
            }
            string degreedetails = "Individual Staff Timetable " + "@ Staff Name: " + staffname + "@ Department Name: " + deptname;
            string pagename = "Subject_Room_Allocation.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }
    protected void txtexcelname_TextChanged(object sender, EventArgs e)
    {
        try
        {

            txt_excelname.Visible = true;
            btn_Excel.Visible = true;
            btn_printmaster.Visible = true;
            lbl_reportname.Visible = true;
            btn_Excel.Focus();
        }
        catch (Exception ex)
        {

        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();

        string query = "select staff_name  from staffmaster where resign =0 and settled =0 and staff_name like  '" + prefixText + "%'";

        name = ws.Getname(query);
        return name;
    }


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getcode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();


        string query = "select staff_code  from staffmaster where resign =0 and settled =0 and staff_code like  '" + prefixText + "%'";

        name = ws.Getname(query);
        return name;
    }

}