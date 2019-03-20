using System;
using System.Collections;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
////using System.IO;
////using iTextSharp.text;
////using iTextSharp.text.html.simpleparser;
////using iTextSharp.text.pdf;
////using System.Web.UI.HtmlControls;
//using Gios.Pdf;

public partial class ConsolidateReport : System.Web.UI.Page
{
    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = "";
    string strquery = "";
    string regularflag = "";
    string genderflag = "";
    string strdayflag = "";
    Hashtable hat = new Hashtable();
    DAccess2 d2 = new DAccess2();
    DataTable dTab = new DataTable();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataView dv1 = new DataView();
    DataView dv2 = new DataView();
    Hashtable hastble4 = new Hashtable();
    Hashtable columnhash = new Hashtable();
    ArrayList columarray = new ArrayList();
    Hashtable columnhash1 = new Hashtable();
    ArrayList columarray1 = new ArrayList();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Visible = false;

            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            if (!Request.FilePath.Contains("CAMHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/MarkMod/CAMHome.aspx");
                    return;
                }
            }
            if (!IsPostBack)
            {

                bindschool();
                bindyear();
                bindschooltype();
                bindstandard();
                bindterm();
                bindsec();
                bindsubname();

                string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                ds = d2.select_method_wo_parameter(Master1, "text");

                Session["strvar"] = "";
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                Session["Daywise"] = "0";
                Session["Hourwise"] = "0";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                    {
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Days Scholor" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            strdayflag = " and (Stud_Type='Day Scholar'";
                        }
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Hostel" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            if (strdayflag != "" && strdayflag != "\0")
                            {
                                strdayflag = strdayflag + " or Stud_Type='Hostler'";
                            }
                            else
                            {
                                strdayflag = " and (Stud_Type='Hostler'";
                            }
                        }
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Regular")
                        {
                            regularflag = "and ((registration.mode=1)";

                            // Session["strvar"] = Session["strvar"] + " and (mode=1)";
                        }
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Lateral")
                        {
                            if (regularflag != "")
                            {
                                regularflag = regularflag + " or (registration.mode=3)";
                            }
                            else
                            {
                                regularflag = regularflag + " and ((registration.mode=3)";
                            }
                            //Session["strvar"] = Session["strvar"] + " and (mode=3)";
                        }
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Transfer")
                        {
                            if (regularflag != "")
                            {
                                regularflag = regularflag + " or (registration.mode=2)";
                            }
                            else
                            {
                                regularflag = regularflag + " and ((registration.mode=2)";
                            }
                            //Session["strvar"] = Session["strvar"] + " and (mode=2)";
                        }

                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Male" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            genderflag = " and (sex='0'";
                        }
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Female" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            if (genderflag != "" && genderflag != "\0")
                            {
                                genderflag = genderflag + " or sex='1'";
                            }
                            else
                            {
                                genderflag = " and (sex='1'";
                            }

                        }
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Day Wise" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            Session["Daywise"] = "1";
                        }
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Hour Wise" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            Session["Hourwise"] = "1";
                        }
                    }
                }
                FpSpread1.Visible = false;
                g1btnexcel.Visible = false;
                g1btnprint.Visible = false;
                txtexcelname.Visible = false;
                lblexportxl.Visible = false;

                FpSpread1.Sheets[0].SheetName = " ";
                FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;

                FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
                style1.Font.Size = 12;
                style1.Font.Bold = true;
                style1.HorizontalAlign = HorizontalAlign.Center;
                style1.ForeColor = System.Drawing.Color.Black;
                FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].AllowTableCorner = true;

                FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                FpSpread1.Pager.Align = HorizontalAlign.Right;
                FpSpread1.Pager.Font.Bold = true;
                FpSpread1.Pager.Font.Name = "Book Antiqua";
                FpSpread1.Pager.ForeColor = System.Drawing.Color.DarkGreen;
                FpSpread1.Pager.BackColor = System.Drawing.Color.Beige;
                FpSpread1.Pager.BackColor = System.Drawing.Color.AliceBlue;
                FpSpread1.Pager.PageCount = 5;
                FpSpread1.CommandBar.Visible = false;
                //---------------------------
            }
        }
        catch (Exception ex) { }
    }


    public void bindschool()
    {
        try
        {
            string columnfield = "";
            usercode = Session["UserCode"].ToString();
            group_code = Session["group_code"].ToString();
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddschool.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddschool.DataSource = ds;
                ddschool.DataTextField = "collname";
                ddschool.DataValueField = "college_code";
                ddschool.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void bindsubname()
    {
        try
        {
            dropsubname.Items.Clear();
            string qury = "SELECT SUBJECT_NO,SUBJECT_NAME  FROM syllabus_master Y,Subject S WHERE Y.syll_code = S.syll_code AND Batch_Year = " + dropyear.SelectedItem.Text + " AND degree_code = " + ddstandard.SelectedValue + "  AND Semester = " + dropterm.SelectedItem.Text + " ";
            ds = d2.select_method_wo_parameter(qury, "text");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                dropsubname.DataSource = ds;
                dropsubname.DataTextField = "SUBJECT_NAME";
                dropsubname.DataValueField = "SUBJECT_NO";
                dropsubname.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }
    public void bindyear()
    {
        try
        {
            dropyear.Items.Clear();
            ds = d2.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                dropyear.DataSource = ds;
                dropyear.DataTextField = "batch_year";
                dropyear.DataValueField = "batch_year";
                dropyear.DataBind();
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                dropyear.SelectedValue = max_bat.ToString();
            }
            dropyear.Text = "batch (" + 1 + ")";
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void bindschooltype()
    {
        try
        {
            ddschooltype.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = ddschool.SelectedItem.Value;
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Clear();
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds = d2.select_method("bind_degree", hat, "sp");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddschooltype.DataSource = ds;
                ddschooltype.DataTextField = "course_name";
                ddschooltype.DataValueField = "course_id";
                ddschooltype.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void bindstandard()
    {
        try
        {
            hat.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddschooltype.SelectedValue);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds = d2.select_method("bind_branch", hat, "sp");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddstandard.DataSource = ds;
                ddstandard.DataTextField = "dept_name";
                ddstandard.DataValueField = "degree_code";
                ddstandard.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void bindterm()
    {
        try
        {
            dropterm.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            string strstandard = "";

            if (ddstandard.SelectedValue != "")
            {
                strstandard = ddstandard.SelectedValue;
            }

            if (strstandard.Trim() != "")
            {
                strstandard = " and degree_code in(" + strstandard + ")";
            }

            strquery = "select distinct ndurations,first_year_nonsemester from ndegree where college_code=" + ddschool.SelectedValue.ToString() + " and batch_year=" + dropyear.Text.ToString() + " and degree_code=" + ddstandard.Text.ToString() + " order by NDurations desc";
            ds.Reset();
            ds.Dispose();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        dropterm.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        dropterm.Items.Add(i.ToString());
                    }
                }
            }
            else
            {
                strquery = "select distinct duration,first_year_nonsemester  from degree where college_code=" + ddschool.SelectedValue.ToString() + " " + ddstandard.SelectedValue.ToString() + " order by duration desc";
                ds.Reset();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(strquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());

                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            dropterm.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            dropterm.Items.Add(i.ToString());
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
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

    protected void ddschool_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindyear();
            bindschooltype();
            bindstandard();
            bindterm();
            bindsubname();
            txtexcelname.Visible = false;
            lblexportxl.Visible = false;
            // bindtestname(val22);
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void dropyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //string bn = "";
            bindschooltype();
            bindstandard();
            bindterm();
            bindsubname();
            lblerrormsg.Visible = false;
            FpSpread1.Visible = false;
            g1btnexcel.Visible = false;
            g1btnprint.Visible = false;
            txtexcelname.Visible = false;
            lblexportxl.Visible = false;
            //bindtestname(bn);
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void dropschooltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //string st = "";
            bindsec();
            // bindtestname(st);
            bindstandard();
            bindterm();
            bindsubname();
            lblerrormsg.Visible = false;
            FpSpread1.Visible = false;
            g1btnexcel.Visible = false;
            g1btnprint.Visible = false;
            txtexcelname.Visible = false;
            lblexportxl.Visible = false;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void ddstandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //string s = "";
            bindsec();
            //bindtestname(s);
            bindterm();
            bindsubname();
            lblerrormsg.Visible = false;
            FpSpread1.Visible = false;
            g1btnexcel.Visible = false;
            g1btnprint.Visible = false;
            txtexcelname.Visible = false;
            lblexportxl.Visible = false;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void dropterm_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsec();
            //lblcalc.Visible = false;
            //txtcalc.Visible = false;
            bindsubname();
            lblerrormsg.Visible = false;
            FpSpread1.Visible = false;
            g1btnexcel.Visible = false;
            g1btnprint.Visible = false;
            txtexcelname.Visible = false;
            lblexportxl.Visible = false;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void g1btnprint_OnClick(object sender, EventArgs e)
    {
        try
        {

            string degreedetails = "Subjectwise Mark and Grade Report" + '@' + "        " + "Year: " + dropyear.SelectedItem.ToString() + "   " + "Standard: " + ddstandard.SelectedItem.ToString() + "   " + "Term: " + dropterm.SelectedItem.ToString() + "   " + "Subject Name: " + dropsubname.SelectedItem.Text.ToString();
            string pagename = "cumreport.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }

    public string loadmarkat(string mr)
    {
        string strgetval = "";
        if (mr == "-1")
        {
            strgetval = "AAA";
        }
        else if (mr == "-2")
        {
            strgetval = "EL";
        }
        else if (mr == "-3")
        {
            strgetval = "EOD";
        }
        else if (mr == "-4")
        {
            strgetval = "ML";
        }
        else if (mr == "-5")
        {
            strgetval = "SOD";
        }
        else if (mr == "-6")
        {
            strgetval = "NSS";
        }
        else if (mr == "-7")
        {
            strgetval = "NJ";
        }
        else if (mr == "-8")
        {
            strgetval = "S";
        }
        else if (mr == "-9")
        {
            strgetval = "L";
        }
        else if (mr == "-10")
        {
            strgetval = "NCC";
        }
        else if (mr == "-11")
        {
            strgetval = "HS";
        }
        else if (mr == "-12")
        {
            strgetval = "PP";
        }
        else if (mr == "-13")
        {
            strgetval = "SYOD";
        }
        else if (mr == "-14")
        {
            strgetval = "COD";
        }
        else if (mr == "-15")
        {
            strgetval = "OOD";
        }
        else if (mr == "-16")
        {
            strgetval = "OD";
        }
        else if (mr == "-17")
        {
            strgetval = "LA";
        }
        else if (mr == "-18")
        {
            strgetval = "RAA";
        }
        return strgetval;
    }

    protected void g1btnexcel_OnClick(object sender, EventArgs e)
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
                lblerrormsg.Text = "Please Enter Your Report Name";
                lblerrormsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void dropsubname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Visible = false;
            FpSpread1.Visible = false;
            g1btnexcel.Visible = false;
            g1btnprint.Visible = false;
            txtexcelname.Visible = false;
            lblexportxl.Visible = false;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void dropreportdisplay_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Visible = false;
            FpSpread1.Visible = false;
            g1btnexcel.Visible = false;
            g1btnprint.Visible = false;
            txtexcelname.Visible = false;
            lblexportxl.Visible = false;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            if (dropsubname.SelectedValue != "")
            {
                FpSpread1.Visible = true;
                g1btnprint.Visible = false;
                g1btnexcel.Visible = false;
                Printcontrol.Visible = false;
                txtexcelname.Visible = false;
                lblexportxl.Visible = false;
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 3;
                FpSpread1.Sheets[0].ColumnCount = 4;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";

                if (Session["Rollflag"].ToString() == "0")
                {
                    FpSpread1.Sheets[0].ColumnHeader.Columns[1].Visible = false;
                }
                else
                {
                    FpSpread1.Sheets[0].ColumnHeader.Columns[1].Visible = true;
                }

                if (Session["Regflag"].ToString() == "0")
                {
                    FpSpread1.Sheets[0].ColumnHeader.Columns[2].Visible = false;
                }
                else
                {
                    FpSpread1.Sheets[0].ColumnHeader.Columns[2].Visible = true;
                }

                FpSpread1.Sheets[0].Columns[0].Width = 20;
                FpSpread1.Sheets[0].Columns[1].Width = 150;
                FpSpread1.Sheets[0].Columns[2].Width = 100;
                FpSpread1.Sheets[0].Columns[3].Width = 150;
                FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;

                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);

                Boolean testflag = false;
                string andsection = "";
                if (dropsec.Enabled == true)
                {
                    andsection = dropsec.SelectedItem.Text.Trim();
                    if (andsection != "")
                    {
                        andsection = "and r.Sections= '" + dropsec.SelectedItem.Text.Trim() + "'";
                    }
                }

                string query1 = "SELECT Istype,CRITERIA_NO FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + dropyear.SelectedItem.Text + "' and degree_code = '" + ddstandard.SelectedValue + "' and semester = '" + dropterm.SelectedItem.Text + "' AND M.subject_no = '" + dropsubname.SelectedValue + "' and Criteria_no!='0' order by Istype,Criteria_no";
                query1 = query1 + " ;select r.Roll_No,r.Reg_No,r.Stud_Name,r.serialno from Registration r,subjectChooser sc where r.Roll_No=sc.roll_no and r.Batch_Year = '" + dropyear.SelectedItem.Text + "' and r.degree_code = '" + ddstandard.SelectedValue + "' and semester = '" + dropterm.SelectedItem.Text + "' " + andsection + " and sc.subject_no='" + dropsubname.SelectedValue + "'";
                ds1 = d2.select_method_wo_parameter(query1, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    string strcritequery = "select c.criteria,c.Criteria_no,e.max_mark from CriteriaForInternal c,Exam_type e,syllabus_master sy where sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and sy.Batch_Year = '" + dropyear.SelectedItem.Text + "' and sy.degree_code =  '" + ddstandard.SelectedValue + "' and sy.semester = '" + dropterm.SelectedItem.Text + "'  and e.subject_no= '" + dropsubname.SelectedValue + "'";
                    DataSet dscriteria = d2.select_method_wo_parameter(strcritequery, "text");
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 50;
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = ds1.Tables[0].Rows[i]["Istype"].ToString();
                        string strcitset = ds1.Tables[0].Rows[i]["CRITERIA_NO"].ToString();
                        int spvcol = FpSpread1.Sheets[0].ColumnCount - 1;
                        string[] spcri = strcitset.Split(',');
                        int noco = 0;
                        string setbestma = "";
                        for (int spc = 0; spc <= spcri.GetUpperBound(0); spc++)
                        {
                            string getcido = spcri[spc].ToString();
                            if (getcido.Trim() != "" && getcido != null)
                            {
                                dscriteria.Tables[0].DefaultView.RowFilter = "Criteria_no='" + getcido + "'";
                                DataView dvgetval = dscriteria.Tables[0].DefaultView;
                                if (dvgetval.Count > 0)
                                {
                                    if (noco > 0)
                                    {
                                        FpSpread1.Sheets[0].ColumnCount++;
                                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 50;
                                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = dvgetval[0]["criteria"].ToString();
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = getcido;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = dvgetval[0]["max_mark"].ToString();
                                    setbestma = dvgetval[0]["max_mark"].ToString();
                                    noco++;
                                }
                            }
                        }
                        if (noco > 0)
                        {
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 50;
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Best";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = strcitset;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = setbestma;
                            noco++;
                            if (dropreportdisplay.SelectedItem.ToString() == "Grade")
                            {
                                noco++;
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 50;
                                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Grade";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                            }

                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, spvcol, 1, noco);

                        }
                    }
                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 50;
                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Term Total";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                    if (dropreportdisplay.SelectedItem.ToString() == "Grade")
                    {
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 50;
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Grade";
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 3, 1);
                    }

                    if (ds1.Tables[1].Rows.Count > 0)
                    {
                        string query2 = "SELECT U.roll_no,REG_NO,STUD_NAME,marks_obtained,Convert(nvarchar(15),Criteria)+' ('+Convert(nvarchar(15),e.max_mark)+')' as Criteria ,c.criteria_no,c.max_mark  FROM Result u,Registration r,Exam_type e,CriteriaForInternal c WHERE U.roll_no = R.Roll_No AND U.exam_code = E.exam_code AND E.criteria_no = C.Criteria_no AND subject_no = '" + dropsubname.SelectedValue + "'  order by u.roll_no,STUD_NAME,e.exam_date,c.criteria_no";
                        ds2 = d2.select_method_wo_parameter(query2, "Text");

                        string query3 = "SELECT distinct roll_no,Convert(nvarchar(15),s.Istype)+' ('+Convert(nvarchar(15),conversion)+')' as Istype,s.Istype as istype1,Exammark, conversion,s.subject_no, Criteria_no FROM tbl_Cam_Calculation C, internal_cam_calculation_master_setting S,syllabus_master y  WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = '" + dropyear.SelectedItem.Text + "'  and degree_code = '" + ddstandard.SelectedValue + "' and semester ='" + dropterm.SelectedItem.Text + "' AND C.subject_no  ='" + dropsubname.SelectedValue + "' order by Criteria_no";
                        DataSet dsoverall = d2.select_method_wo_parameter(query3, "Text");
                        int srno = 0;
                        for (int s = 0; s < ds1.Tables[1].Rows.Count; s++)
                        {
                            string rollno = ds1.Tables[1].Rows[s]["Roll_No"].ToString();
                            srno++;
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = rollno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds1.Tables[1].Rows[s]["Reg_No"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds1.Tables[1].Rows[s]["Stud_Name"].ToString();

                            string heafcriteria = "";
                            for (int col = 4; col < FpSpread1.Sheets[0].ColumnCount; col++)
                            {
                                testflag = true;
                                if (FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Text.ToString() != "")
                                {
                                    heafcriteria = FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Text.ToString();
                                }
                                if (heafcriteria == "Term Total")
                                {
                                    if (FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Text.ToString() == "Term Total")
                                    {
                                        dsoverall.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "' and istype1='Calculate 1'";
                                        DataView dvmark = dsoverall.Tables[0].DefaultView;
                                        if (dvmark.Count > 0)
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = dvmark[0]["Exammark"].ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Tag = dvmark[0]["Exammark"].ToString();
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[2, col].Text = dvmark[0]["conversion"].ToString();

                                        }
                                    }
                                }
                                else if (heafcriteria == "Grade")
                                {
                                    // ---------------


                                    string percc = FpSpread1.Sheets[0].ColumnHeader.Cells[2, col].Text.ToString();
                                    string[] splitpercc = percc.Split('%');
                                    percc = splitpercc[0];
                                    string frs = FpSpread1.Sheets[0].ColumnHeader.Cells[0, col - 1].Text.ToString().Trim().ToLower();
                                    if (frs == "term total")
                                    {
                                        percc = FpSpread1.Sheets[0].ColumnHeader.Cells[2, col - 1].Text.ToString();
                                        splitpercc = percc.Split('%');
                                        percc = splitpercc[0];

                                    }
                                    if (percc != "")
                                    {
                                        string markgrade = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col - 1].Text.ToString();
                                        string orginalmark = Convert.ToString(FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col - 1].Tag);

                                        if (Convert.ToDouble(orginalmark) > 0)
                                        {
                                            double calculatedval = Convert.ToDouble(markgrade) / Convert.ToDouble(percc);
                                            calculatedval = calculatedval * 100;
                                            // ------------
                                            //string sdr="select Mark_Grade from Grade_Master where '" + calculatedval + "' between frange and trange  and Criteria='' and Degree_Code='" + ddstandard.SelectedValue + "' and batch_year='" + dropyear.SelectedItem.Text + "'";
                                            string setgrade = d2.GetFunction("select Mark_Grade from Grade_Master where '" + calculatedval + "' between frange and trange  and Criteria='' and Degree_Code='" + ddstandard.SelectedValue + "' and batch_year='" + dropyear.SelectedItem.Text + "' and semester='0'");
                                            if (setgrade.Trim() != "" && setgrade != null && setgrade.Trim() != "0")
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = setgrade;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                        }
                                        else
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = " ";
                                        }
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = " ";
                                    }
                                }
                                else
                                {
                                    string getheadval = FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text.ToString();
                                    if (getheadval == "Best")
                                    {
                                        if (FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Tag != null)
                                        {
                                            string getcrino = FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Tag.ToString();
                                            dsoverall.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "' and criteria_no='" + getcrino + "'";
                                            DataView dvmark1 = dsoverall.Tables[0].DefaultView;
                                            if (dvmark1.Count > 0)
                                            {
                                                string getmark = dvmark1[0]["Exammark"].ToString();
                                                if (getmark.Trim() != "" && getmark != null)
                                                {
                                                    if (Convert.ToDouble(getmark) < 0)
                                                    {

                                                        getmark = loadmarkat(getmark);

                                                    }
                                                }

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = dvmark1[0]["Exammark"].ToString();
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Tag = Convert.ToString(getmark);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                        }
                                    }
                                    if (getheadval == "Grade")
                                    {
                                        string percc = FpSpread1.Sheets[0].ColumnHeader.Cells[2, col - 1].Text.ToString();
                                        string[] splitpercc = percc.Split('%');
                                        percc = splitpercc[0];
                                        if (percc != "")
                                        {
                                            string markgrade = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col - 1].Text.ToString();
                                            string orginalmark = Convert.ToString(FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col - 1].Tag);

                                            if (Convert.ToDouble(orginalmark) > 0)
                                            {
                                                double calculatedval = Convert.ToDouble(markgrade) / Convert.ToDouble(percc);
                                                calculatedval = calculatedval * 100;


                                                string setgrade = d2.GetFunction("select Mark_Grade from Grade_Master where '" + calculatedval + "' between frange and trange  and Criteria='" + heafcriteria + "' and Degree_Code='" + ddstandard.SelectedValue + "' and batch_year='" + dropyear.SelectedItem.Text + "'");
                                                if (setgrade.Trim() != "" && setgrade != null && setgrade.Trim() != "0")
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = setgrade;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = " ";
                                            }
                                        }
                                        else
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = " ";
                                        }
                                    }
                                    else
                                    {
                                        string getheadvalsfa = FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text.ToString();
                                        if (FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Tag != null && getheadvalsfa != "Best")
                                        {
                                            string getcrino = FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Tag.ToString();
                                            ds2.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "' and criteria_no='" + getcrino + "'";
                                            DataView dvmark2 = ds2.Tables[0].DefaultView;
                                            if (dvmark2.Count > 0)
                                            {
                                                string getmark = dvmark2[0]["marks_obtained"].ToString();
                                                string markvalue = Convert.ToString(dvmark2[0]["marks_obtained"]);
                                                if (getmark.Trim() != "" && getmark != null)
                                                {
                                                    if (Convert.ToDouble(getmark) < 0)
                                                    {
                                                        getmark = loadmarkat(getmark);
                                                    }
                                                }
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = getmark;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Tag = Convert.ToString(markvalue);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        string strpentest = "SELECT distinct Criteria ,c.criteria_no,c.max_mark FROM Result u,Registration r,Exam_type e,CriteriaForInternal c WHERE U.roll_no = R.Roll_No AND U.exam_code = E.exam_code AND E.criteria_no = C.Criteria_no AND subject_no ='" + dropsubname.SelectedValue + "' and criteria like '%pen%' order by criteria";
                        strpentest = strpentest + " SELECT U.roll_no,REG_NO,STUD_NAME,marks_obtained,Criteria ,c.criteria_no,c.max_mark  FROM Result u,Registration r,Exam_type e,CriteriaForInternal c WHERE U.roll_no = R.Roll_No AND U.exam_code = E.exam_code AND E.criteria_no = C.Criteria_no AND subject_no ='" + dropsubname.SelectedValue + "' and criteria like '%pen%' order by u.roll_no,STUD_NAME,e.exam_date,c.criteria_no";
                        DataSet dspen = d2.select_method_wo_parameter(strpentest, "text");
                        if (dspen.Tables[0].Rows.Count > 0)
                        {
                            for (int p = 0; p < dspen.Tables[0].Rows.Count; p++)
                            {
                                FpSpread1.Sheets[0].ColumnCount++;

                                if (p == 0)
                                {
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Pen-Paper Test Marks";
                                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 1, dspen.Tables[0].Rows.Count);
                                }
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = dspen.Tables[0].Rows[p]["Criteria"].ToString();
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = dspen.Tables[0].Rows[p]["max_mark"].ToString();
                                for (int r = 0; r < FpSpread1.Sheets[0].RowCount; r++)
                                {
                                    string roll = FpSpread1.Sheets[0].Cells[r, 1].Text.ToString();
                                    dspen.Tables[1].DefaultView.RowFilter = "roll_no='" + roll + "' and criteria_no='" + dspen.Tables[0].Rows[p]["criteria_no"].ToString() + "'";
                                    DataView dvpen = dspen.Tables[1].DefaultView;
                                    if (dvpen.Count > 0)
                                    {
                                        string getmark = dvpen[0]["marks_obtained"].ToString();
                                        if (getmark.Trim() != "" && getmark != null)
                                        {
                                            if (Convert.ToDouble(getmark) < 0)
                                            {
                                                getmark = loadmarkat(getmark);
                                            }
                                        }
                                        FpSpread1.Sheets[0].Cells[r, FpSpread1.Sheets[0].ColumnCount - 1].Text = getmark;
                                        FpSpread1.Sheets[0].Cells[r, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                }
                            }
                        }
                        if (testflag == true)
                        {
                            g1btnprint.Visible = true;
                            g1btnexcel.Visible = true;
                            Printcontrol.Visible = false;
                            txtexcelname.Visible = true;
                            lblexportxl.Visible = true;
                        }
                        else
                        {
                            FpSpread1.Visible = false;
                            lblerrormsg.Text = "No Marks Found";
                            lblerrormsg.Visible = true;
                        }
                    }
                    else
                    {
                        FpSpread1.Visible = false;
                        lblerrormsg.Text = "No Student's Available";
                        lblerrormsg.Visible = true;
                    }
                }
                else
                {
                    FpSpread1.Visible = false;
                    lblerrormsg.Text = "No Records Found";
                    lblerrormsg.Visible = true;
                }
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread1.SaveChanges();
            }
            else
            {
                FpSpread1.Visible = false;
                lblerrormsg.Text = "No Records Found";
                lblerrormsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }
    //protected void btngo_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string sbst = "";
    //        if (dropsubname.SelectedValue != "")
    //        {
    //            if (dropreportdisplay.SelectedItem.Text != "Mark")
    //            {
    //                int count = 1;
    //                string cribst = "";
    //                string query35A = "";

    //                int test = 0;
    //                DataRow dr = null;
    //                DataSet ds3A = new DataSet();
    //                ArrayList checkarray = new ArrayList();
    //                ArrayList addterm = new ArrayList();
    //                DataTable dtble = new DataTable();
    //                Hashtable hastble1 = new Hashtable();
    //                Hashtable hastble3 = new Hashtable();
    //                Hashtable hastbleee = new Hashtable();
    //                DataSet ds3A1 = new DataSet();
    //                DataSet dsgrd = new DataSet();
    //                DataSet ds3A2 = new DataSet();
    //                dTab.Columns.Add("S.No", typeof(Int32));
    //                dTab.Columns.Add("Roll No", typeof(string));
    //                dTab.Columns.Add("Reg No", typeof(string));
    //                dTab.Columns.Add("Student Name", typeof(string));
    //                Hashtable hastb1 = new Hashtable();

    //                // ************************ Query-1 for FA 1 and SA 1 ************************
    //                string query1 = "";
    //                query1 = "SELECT Istype,CRITERIA_NO FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " AND M.subject_no = " + dropsubname.SelectedValue + " and Criteria_no!='0' order by Istype,Criteria_no";
    //                ds1 = d2.select_method_wo_parameter(query1, "Text");

    //                DataSet dsbest = new DataSet();
    //                ArrayList addcheck = new ArrayList();
    //                if (ds1.Tables[0].Rows.Count > 0)
    //                {
    //                    for (int ik = 0; ik < ds1.Tables[0].Rows.Count; ik++)
    //                    {
    //                        if (!hastble1.ContainsKey(ds1.Tables[0].Rows[ik]["Istype"].ToString()))
    //                        {
    //                            string istyp = ds1.Tables[0].Rows[ik]["Istype"].ToString();
    //                            string crteria = ds1.Tables[0].Rows[ik]["CRITERIA_NO"].ToString();
    //                            ds1.Tables[0].DefaultView.RowFilter = "CRITERIA_NO='" + crteria + "'";
    //                            dv1 = ds1.Tables[0].DefaultView;
    //                            if (dv1.Count > 0)
    //                            {
    //                                string query31 = "";
    //                                query31 = "SELECT distinct C.Istype, Criteria_no FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " AND C.subject_no  = " + dropsubname.SelectedValue + " and Criteria_no!='0' order by Criteria_no";
    //                                DataSet dset31 = new DataSet();
    //                                dset31 = d2.select_method_wo_parameter(query31, "Text");
    //                                if (dset31.Tables[0].Rows.Count > 0)
    //                                {
    //                                    // *********** add FA-1 and SA-1 name ***********
    //                                    if (checkarray.Count >= 0)
    //                                    {
    //                                        dtble = dv1.ToTable();
    //                                        counttestvalue(dtble);

    //                                        hastb1.Add(dtble, ToString());
    //                                        checkarray.Add(crteria);
    //                                        string valu = Convert.ToString(dtble);
    //                                    }
    //                                    // *********** add FA-1 and SA-1 name ***********
    //                                }

    //                                // ************************ Query-2 for Criteria ************************
    //                                String query2 = "";
    //                                query2 = "SELECT U.roll_no,REG_NO,STUD_NAME,marks_obtained,Convert(nvarchar(15),Criteria)+' ('+Convert(nvarchar(15),e.max_mark)+')' as Criteria ,c.criteria_no,c.max_mark  FROM Result u,Registration r,Exam_type e,CriteriaForInternal c WHERE U.roll_no = R.Roll_No AND U.exam_code = E.exam_code AND E.criteria_no = C.Criteria_no AND subject_no = " + dropsubname.SelectedValue + " order by u.roll_no,STUD_NAME,e.exam_date,c.criteria_no";
    //                                ds2 = d2.select_method_wo_parameter(query2, "Text");

    //                                for (int ik1 = 0; ik1 < ds2.Tables[0].Rows.Count; ik1++)
    //                                {
    //                                    string rollno1 = ds2.Tables[0].Rows[ik1]["roll_no"].ToString();
    //                                    ds2.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno1 + "'";
    //                                    dv2 = ds2.Tables[0].DefaultView;
    //                                    if (dv2.Count > 0)
    //                                    {
    //                                        if (!hastble3.ContainsKey(dv2[0]["roll_no"]))
    //                                        {
    //                                            dr = dTab.NewRow();
    //                                            dr[0] = count;
    //                                            dr[1] = dv2[0]["roll_no"].ToString();
    //                                            dr[2] = dv2[0]["REG_NO"].ToString();
    //                                            dr[3] = dv2[0]["Stud_Name"].ToString();

    //                                            string query35 = "";
    //                                            count++;
    //                                            Hashtable hastble2 = new Hashtable();
    //                                            {
    //                                                {
    //                                                    // ************************ Query-2 add Tool Name ************************
    //                                                    for (int p = 0; p < dv2.Count; p++)
    //                                                    {
    //                                                        if (!hastble2.ContainsKey(dv2[p]["roll_no"] + "-" + dv2[p]["Criteria"]))
    //                                                        {
    //                                                            if (dv2[p]["roll_no"] + "-" + dv2[p]["Criteria"] == dv2[p]["roll_no"] + "-" + dv2[p]["Criteria"])
    //                                                            {
    //                                                                if (p == 0)
    //                                                                {
    //                                                                    cribst = "";
    //                                                                }
    //                                                                if (cribst == "")
    //                                                                {
    //                                                                    cribst = dv2[p]["criteria_no"].ToString();
    //                                                                }
    //                                                                else
    //                                                                {
    //                                                                    cribst = cribst + "," + dv2[p]["criteria_no"].ToString();
    //                                                                }
    //                                                                if (!hastble4.ContainsKey(dv2[p]["Criteria"]))
    //                                                                {
    //                                                                    hastble2.Add(dv2[p]["roll_no"] + "-" + dv2[p]["Criteria"], dv2[p]["Criteria"]);
    //                                                                    column++;

    //                                                                    // *********** add Best ***********
    //                                                                    string mm = "SELECT Cam_Option FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " AND M.subject_no = " + dropsubname.SelectedValue + "";
    //                                                                    DataSet dsv = new DataSet();
    //                                                                    dsv = d2.select_method_wo_parameter(mm, "text");
    //                                                                    if (dsv.Tables[0].Rows.Count > 0)
    //                                                                    {
    //                                                                        string bestquery = "SELECT distinct C.Istype, Criteria_no FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + "  AND C.subject_no  = " + dropsubname.SelectedValue + "  and Criteria_no!='0' and Criteria_no='" + cribst + "' order by Criteria_no";
    //                                                                        dsbest = d2.select_method_wo_parameter(bestquery, "Text");
    //                                                                        if (dsbest.Tables[0].Rows.Count == 0)
    //                                                                        {
    //                                                                            if (!hastble4.ContainsKey(dv2[p]["Criteria"].ToString()))
    //                                                                            {
    //                                                                                string[] pen = dv2[p]["Criteria"].ToString().Split(' ');
    //                                                                                if (pen[0].ToString().ToLower().Contains("pen"))
    //                                                                                {
    //                                                                                    test++;
    //                                                                                    if (test == 1)
    //                                                                                    {
    //                                                                                        //   if (dv2.Count - 1 == p)
    //                                                                                        {
    //                                                                                            string rollnoA218 = ds2.Tables[0].Rows[ik1]["roll_no"].ToString();
    //                                                                                            query35A = "SELECT distinct roll_no,Convert(nvarchar(15),s.Istype)+' ('+Convert(nvarchar(15),conversion)+')' as Istype,s.Istype as isty,Exammark, conversion,s.subject_no, Criteria_no FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " and c.Istype='Calculate 1' AND C.subject_no  = " + dropsubname.SelectedValue + " and roll_no='" + rollnoA218 + "' order by Criteria_no";
    //                                                                                            ds3A = d2.select_method_wo_parameter(query35A, "Text");

    //                                                                                            if (!hastble4.ContainsKey("Term Total"))
    //                                                                                            {
    //                                                                                                column = column + 1;
    //                                                                                                dTab.Columns.Add("Term Total" + " " + "(" + ds3A.Tables[0].Rows[ik1]["conversion"].ToString() + ")");
    //                                                                                                //addcheck.Add(column);
    //                                                                                                addterm.Add(column);
    //                                                                                                hastble4.Add("Term Total", column);
    //                                                                                                column = column + 1;
    //                                                                                                b++;
    //                                                                                                dTab.Columns.Add("Grade " + b, typeof(string));
    //                                                                                                hastble4.Add("Grade " + p, column);

    //                                                                                                string mark = ds3A.Tables[0].Rows[0]["Exammark"].ToString();
    //                                                                                                string best = "select Mark_Grade from Grade_Master where '" + mark + "' between frange and trange  and Criteria='" + ds3A.Tables[0].Rows[ik1]["isty"].ToString() + "'";
    //                                                                                                dsgrd = d2.select_method_wo_parameter(best, "Text");
    //                                                                                                int cnt2222 = Convert.ToInt32(hastble4["Grade " + p].ToString());
    //                                                                                                if (dsgrd.Tables[0].Rows.Count > 0)
    //                                                                                                {
    //                                                                                                    dr[cnt2222] = Convert.ToString(dsgrd.Tables[0].Rows[ik1]["Mark_Grade"].ToString());
    //                                                                                                }
    //                                                                                                else
    //                                                                                                {
    //                                                                                                    //dr[cnt2222] = "1st";
    //                                                                                                    dr[cnt2222] = " ";
    //                                                                                                }
    //                                                                                            }
    //                                                                                            string rollnoA21 = ds2.Tables[0].Rows[ik1]["roll_no"].ToString();
    //                                                                                            query35A = "SELECT * FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " and c.Istype='Calculate 1' AND C.subject_no  = " + dropsubname.SelectedValue + " and roll_no='" + rollnoA21 + "' order by Criteria_no";
    //                                                                                            ds3A = d2.select_method_wo_parameter(query35A, "Text");
    //                                                                                            if (ds3A.Tables[0].Rows.Count > 0)
    //                                                                                            {
    //                                                                                                int cnt22 = Convert.ToInt32(hastble4["Term Total"].ToString());
    //                                                                                                dr[cnt22] = Convert.ToString(ds3A.Tables[0].Rows[0]["Exammark"].ToString());
    //                                                                                                cribst = "";
    //                                                                                            }
    //                                                                                        }
    //                                                                                    }
    //                                                                                    else
    //                                                                                    {
    //                                                                                        string rollnoA218 = ds2.Tables[0].Rows[ik1]["roll_no"].ToString();
    //                                                                                        query35A = "SELECT distinct roll_no,Convert(nvarchar(15),s.Istype)+' ('+Convert(nvarchar(15),conversion)+')' as Istype,s.Istype as isty,Exammark, conversion,s.subject_no, Criteria_no FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " and c.Istype='Calculate 1' AND C.subject_no  = " + dropsubname.SelectedValue + " and roll_no='" + rollnoA218 + "' order by Criteria_no";
    //                                                                                        ds3A = d2.select_method_wo_parameter(query35A, "Text");
    //                                                                                        if (!hastble4.ContainsKey("Term Total"))
    //                                                                                        {
    //                                                                                            column = column + 1;
    //                                                                                            dTab.Columns.Add("Term Total" + " " + "(" + ds3A.Tables[0].Rows[p]["conversion"].ToString() + ")");
    //                                                                                            //addcheck.Add(column);
    //                                                                                            addterm.Add(column);
    //                                                                                            hastble4.Add("Term Total", column);

    //                                                                                            string mark = ds3A.Tables[0].Rows[0]["Exammark"].ToString();
    //                                                                                            string best = "select Mark_Grade from Grade_Master where '" + mark + "' between frange and trange  and Criteria='" + ds3A.Tables[0].Rows[ik1]["isty"].ToString() + "'";
    //                                                                                            dsgrd = d2.select_method_wo_parameter(best, "Text");
    //                                                                                            int cnt2222 = Convert.ToInt32(hastble4["Grade " + p].ToString());
    //                                                                                            if (dsgrd.Tables[0].Rows.Count > 0)
    //                                                                                            {
    //                                                                                                dr[cnt2222] = Convert.ToString(dsgrd.Tables[0].Rows[ik1]["Mark_Grade"].ToString());
    //                                                                                            }
    //                                                                                            else
    //                                                                                            {
    //                                                                                                dr[cnt2222] = "2nd";
    //                                                                                            }
    //                                                                                        }
    //                                                                                        //string rollnoA21 = ds2.Tables[0].Rows[ik1]["roll_no"].ToString();
    //                                                                                        ////query35A = "SELECT * FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " and c.Istype='Calculate 1' AND C.subject_no  = " + dropsubname.SelectedValue + " and roll_no='" + rollnoA21 + "' order by Criteria_no";
    //                                                                                        //query35A = "SELECT distinct roll_no,Convert(nvarchar(15),s.Istype)+' ('+Convert(nvarchar(15),conversion)+')' as Istype,Exammark, conversion,s.subject_no, Criteria_no FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " and c.Istype='Calculate 1' AND C.subject_no  = " + dropsubname.SelectedValue + " and roll_no='" + rollnoA21 + "' order by Criteria_no";
    //                                                                                        //ds3A = d2.select_method_wo_parameter(query35A, "Text");
    //                                                                                        if (ds3A.Tables[0].Rows.Count > 0)
    //                                                                                        {
    //                                                                                            int cnt22 = Convert.ToInt32(hastble4["Term Total"].ToString());
    //                                                                                            //hastble4.Add("Term Total", column);
    //                                                                                            dr[cnt22] = Convert.ToString(ds3A.Tables[0].Rows[0]["Exammark"].ToString());
    //                                                                                            cribst = "";

    //                                                                                            // ----------------- term grade ***************************
    //                                                                                            string mark = ds3A.Tables[0].Rows[0]["Exammark"].ToString();
    //                                                                                            string best = "select Mark_Grade from Grade_Master where '" + mark + "' between frange and trange  and Criteria='" + ds3A.Tables[0].Rows[ik1]["isty"].ToString() + "'";
    //                                                                                            dsgrd = d2.select_method_wo_parameter(best, "Text");
    //                                                                                            int cnt2222 = Convert.ToInt32(hastble4["Grade "] + p.ToString());
    //                                                                                            if (dsgrd.Tables[0].Rows.Count > 0)
    //                                                                                            {
    //                                                                                                dr[cnt2222 + 1] = Convert.ToString(dsgrd.Tables[0].Rows[ik1]["Mark_Grade"].ToString());
    //                                                                                            }
    //                                                                                            //else
    //                                                                                            //{
    //                                                                                            //    dr[cnt2222 + 1] = "3rd";
    //                                                                                            //}
    //                                                                                            // ----------------- term grade ***************************
    //                                                                                        }
    //                                                                                    }
    //                                                                                }

    //                                                                                dTab.Columns.Add(dv2[p]["Criteria"].ToString());
    //                                                                                sbst = dv2[p]["max_mark"].ToString();
    //                                                                                hastble4.Add(dv2[p]["Criteria"], column);
    //                                                                                int cnt = column;
    //                                                                                dr[cnt + 1] = Convert.ToString(dv2[p]["marks_obtained"]);
    //                                                                            }
    //                                                                            else
    //                                                                            {
    //                                                                                //  hastble4.Add(dv2[p]["Criteria"], column);
    //                                                                                int cnt = column;
    //                                                                                dr[cnt + 1] = Convert.ToString(dv2[p]["marks_obtained"]);
    //                                                                            }
    //                                                                        }
    //                                                                        else
    //                                                                        {

    //                                                                            if (!hastble4.ContainsKey(dv2[p]["Criteria"].ToString()))
    //                                                                            {
    //                                                                                dTab.Columns.Add(dv2[p]["Criteria"].ToString());
    //                                                                                hastble4.Add(dv2[p]["Criteria"], column);
    //                                                                            }
    //                                                                            int cnt = column;
    //                                                                            dr[cnt + 1] = Convert.ToString(dv2[p]["marks_obtained"]);

    //                                                                            b++;
    //                                                                            if (!hastble4.ContainsKey("Best " + p))
    //                                                                            {
    //                                                                                if (!hastb1.ContainsKey(dv1[ik]["Istype"].ToString()))
    //                                                                                {
    //                                                                                    column = column + 1;

    //                                                                                    dTab.Columns.Add("Best " + b + " " + ds1.Tables[0].Rows[b - 1]["Istype"].ToString() + " " + "(" + sbst + ")", typeof(string));
    //                                                                                    addcheck.Add(column);
    //                                                                                    hastble4.Add("Best " + p, column);
    //                                                                                    column = column + 1;
    //                                                                                    dTab.Columns.Add("Grade " + b, typeof(string));
    //                                                                                    hastble4.Add("Grade " + p, column);
    //                                                                                    if ((dv2.Count - 1) == p)
    //                                                                                    {
    //                                                                                        column = column + 1;
    //                                                                                        string rollnoA210 = ds2.Tables[0].Rows[ik1]["roll_no"].ToString();
    //                                                                                        query35A = "SELECT distinct roll_no,Convert(nvarchar(15),s.Istype)+' ('+Convert(nvarchar(15),conversion)+')' as Istype,s.Istype as isty,Exammark, conversion,s.subject_no, Criteria_no FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " and c.Istype='Calculate 1' AND C.subject_no  = " + dropsubname.SelectedValue + " and roll_no='" + rollnoA210 + "' order by Criteria_no";
    //                                                                                        ds3A = d2.select_method_wo_parameter(query35A, "Text");

    //                                                                                        dTab.Columns.Add("Term Total" + " " + "(" + ds3A.Tables[0].Rows[0]["conversion"].ToString() + ")");
    //                                                                                        hastble4.Add("Term Total", column);

    //                                                                                        int cnt22 = Convert.ToInt32(hastble4["Term Total"].ToString());
    //                                                                                        dr[cnt22 + 1] = Convert.ToString(ds3A.Tables[0].Rows[0]["Exammark"].ToString());

    //                                                                                        column = column + 1;
    //                                                                                        b++;
    //                                                                                        dTab.Columns.Add("Grade " + b, typeof(string));
    //                                                                                        //hastble4.Add("Grade " + p, column);

    //                                                                                        string mark = ds3A.Tables[0].Rows[0]["Exammark"].ToString();
    //                                                                                        string best = "select Mark_Grade from Grade_Master where '" + mark + "' between frange and trange  and Criteria='" + ds3A.Tables[0].Rows[ik1]["isty"].ToString() + "'";
    //                                                                                        dsgrd = d2.select_method_wo_parameter(best, "Text");
    //                                                                                        int cnt2222 = Convert.ToInt32(hastble4["Grade " + p].ToString());
    //                                                                                        if (dsgrd.Tables[0].Rows.Count > 0)
    //                                                                                        {
    //                                                                                            dr[cnt2222 + 1] = Convert.ToString(dsgrd.Tables[0].Rows[ik1]["Mark_Grade"].ToString());
    //                                                                                            //dr[cnt2222] = Convert.ToString(dsgrd.Tables[0].Rows[ik1]["Mark_Grade"].ToString());
    //                                                                                        }
    //                                                                                        else
    //                                                                                        {
    //                                                                                            dr[cnt2222 + 1] = "4th";
    //                                                                                        }
    //                                                                                    }
    //                                                                                }
    //                                                                            }
    //                                                                            // ------------------- best 1st Row ***********************
    //                                                                            string rollno2 = dv2[p]["roll_no"].ToString();
    //                                                                            query35 = "SELECT distinct roll_no,Convert(nvarchar(15),s.Istype)+' ('+Convert(nvarchar(15),conversion)+')' as Istype,s.Istype as istype1,Exammark, conversion,s.subject_no, Criteria_no FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " AND Criteria_no = '" + cribst + "' AND C.subject_no  = " + dropsubname.SelectedValue + " and roll_no='" + rollno2 + "' order by Criteria_no";
    //                                                                            ds3 = d2.select_method_wo_parameter(query35, "Text");
    //                                                                            //string istyp1 = ds3.Tables[0].Rows[0]["Istype"].ToString();
    //                                                                            if (ds3.Tables[0].Rows.Count > 0)
    //                                                                            {
    //                                                                                string mark = ds3.Tables[0].Rows[0]["Exammark"].ToString();
    //                                                                                string best = "select Mark_Grade from Grade_Master where '" + mark + "' between frange and trange  and Criteria='" + ds3.Tables[0].Rows[0]["istype1"].ToString() + "'";
    //                                                                                dsgrd = d2.select_method_wo_parameter(best, "Text");

    //                                                                                int cnt22 = Convert.ToInt32(hastble4["Best " + p].ToString());
    //                                                                                //int cnt22 = column;
    //                                                                                dr[cnt22 + 1] = Convert.ToString(ds3.Tables[0].Rows[0]["Exammark"].ToString());

    //                                                                                int cnt222 = Convert.ToInt32(hastble4["Grade " + p].ToString());
    //                                                                                if (dsgrd.Tables[0].Rows.Count > 0)
    //                                                                                {
    //                                                                                    dr[cnt222 + 1] = Convert.ToString(dsgrd.Tables[0].Rows[0]["Mark_Grade"].ToString());
    //                                                                                }
    //                                                                                //else
    //                                                                                //{
    //                                                                                //    dr[cnt222 + 1] = "5th";
    //                                                                                //}

    //                                                                                cribst = "";
    //                                                                            }
    //                                                                            // ------------------- best 1st Row ***********************
    //                                                                        }
    //                                                                    }
    //                                                                }
    //                                                                else
    //                                                                {
    //                                                                    string rollnoA210 = ds2.Tables[0].Rows[ik1]["roll_no"].ToString();
    //                                                                    query35A = "SELECT distinct roll_no,Convert(nvarchar(15),s.Istype)+' ('+Convert(nvarchar(15),conversion)+')' as Istype,s.Istype as isty,Exammark, conversion,s.subject_no, Criteria_no FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " and c.Istype='Calculate 1' AND C.subject_no  = " + dropsubname.SelectedValue + " and roll_no='" + rollnoA210 + "' order by Criteria_no";
    //                                                                    ds3A = d2.select_method_wo_parameter(query35A, "Text");
    //                                                                    if (!hastble4.ContainsKey("Term Total"))
    //                                                                    {
    //                                                                        column = column + 1;
    //                                                                        //dTab.Columns.Add("Term Total" + " " + ds3A.Tables[0].Rows[p]["conversion"].ToString());
    //                                                                        //hastble4.Add("Term Total", column);
    //                                                                    }
    //                                                                    else
    //                                                                    {

    //                                                                        int cnt = column;
    //                                                                        dr[cnt + 1] = Convert.ToString(dv2[p]["marks_obtained"]);

    //                                                                        // ------------ best 1 continue further row ***************************
    //                                                                        string rollno2 = dv2[p]["roll_no"].ToString();
    //                                                                        query35 = "SELECT distinct roll_no,Convert(nvarchar(15),s.Istype)+' ('+Convert(nvarchar(15),conversion)+')' as Istype,s.Istype as istype1,Exammark, conversion,s.subject_no, Criteria_no FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " AND Criteria_no = '" + cribst + "' AND C.subject_no  = " + dropsubname.SelectedValue + " and roll_no='" + rollno2 + "' order by Criteria_no";
    //                                                                        ds3 = d2.select_method_wo_parameter(query35, "Text");
    //                                                                        //string istyp1 = ds3.Tables[0].Rows[0]["Istype"].ToString();
    //                                                                        if (ds3.Tables[0].Rows.Count > 0)
    //                                                                        {
    //                                                                            string mark = ds3.Tables[0].Rows[0]["Exammark"].ToString();
    //                                                                            string best = "select Mark_Grade from Grade_Master where '" + mark + "' between frange and trange  and Criteria='" + ds3.Tables[0].Rows[0]["istype1"].ToString() + "'";
    //                                                                            dsgrd = d2.select_method_wo_parameter(best, "Text");

    //                                                                            int cnt22 = Convert.ToInt32(hastble4["Best " + p].ToString());
    //                                                                            //int cnt22 = column;
    //                                                                            dr[cnt22 + 1] = Convert.ToString(ds3.Tables[0].Rows[0]["Exammark"].ToString());

    //                                                                            int cnt222 = Convert.ToInt32(hastble4["Grade " + p].ToString());
    //                                                                            if (dsgrd.Tables[0].Rows.Count > 0)
    //                                                                            {
    //                                                                                dr[cnt222 + 1] = Convert.ToString(dsgrd.Tables[0].Rows[0]["Mark_Grade"].ToString());
    //                                                                            }
    //                                                                            //else
    //                                                                            //{
    //                                                                            //    dr[cnt222 + 1] = "6th";
    //                                                                            //}

    //                                                                            cribst = "";
    //                                                                        }
    //                                                                        // ------------- best 1 continue further row ****************************

    //                                                                        //string mark = dv2[p]["marks_obtained"].ToString();
    //                                                                        //string best = "select Mark_Grade from Grade_Master where '" + mark + "' between frange and trange  and Criteria='" + istyp + "'";
    //                                                                        //dsgrd = d2.select_method_wo_parameter(best, "Text");
    //                                                                        //int cnt2222 = Convert.ToInt32(hastble4["Grade "] + p.ToString());
    //                                                                        //if (dsgrd.Tables[0].Rows.Count > 0)
    //                                                                        //{
    //                                                                        //    dr[cnt2222 + 1] = Convert.ToString(dsgrd.Tables[0].Rows[0]["Mark_Grade"].ToString());
    //                                                                        //}
    //                                                                        //else
    //                                                                        //{
    //                                                                        //    dr[cnt2222 + 1] = "2nd";
    //                                                                        //}
    //                                                                    }
    //                                                                    // ---------------------------------

    //                                                                    int nm = Convert.ToInt32(hastble4[dv2[p]["Criteria"]]);
    //                                                                    int mr = Convert.ToInt32(dv2[p]["marks_obtained"]);
    //                                                                    if (mr == -1)
    //                                                                    {
    //                                                                        strgetval = "AAA";
    //                                                                    }
    //                                                                    else if (mr == -2)
    //                                                                    {
    //                                                                        strgetval = "EL";
    //                                                                    }
    //                                                                    else if (mr == -3)
    //                                                                    {
    //                                                                        strgetval = "EOD";
    //                                                                    }
    //                                                                    else if (mr == -4)
    //                                                                    {
    //                                                                        strgetval = "ML";
    //                                                                    }
    //                                                                    else if (mr == -5)
    //                                                                    {
    //                                                                        strgetval = "SOD";
    //                                                                    }
    //                                                                    else if (mr == -6)
    //                                                                    {
    //                                                                        strgetval = "NSS";
    //                                                                    }
    //                                                                    else if (mr == -7)
    //                                                                    {
    //                                                                        strgetval = "NJ";
    //                                                                    }
    //                                                                    else if (mr == -8)
    //                                                                    {
    //                                                                        strgetval = "S";
    //                                                                    }
    //                                                                    else if (mr == -9)
    //                                                                    {
    //                                                                        strgetval = "L";
    //                                                                    }
    //                                                                    else if (mr == -10)
    //                                                                    {
    //                                                                        strgetval = "NCC";
    //                                                                    }
    //                                                                    else if (mr == -11)
    //                                                                    {
    //                                                                        strgetval = "HS";
    //                                                                    }
    //                                                                    else if (mr == -12)
    //                                                                    {
    //                                                                        strgetval = "PP";
    //                                                                    }
    //                                                                    else if (mr == -13)
    //                                                                    {
    //                                                                        strgetval = "SYOD";
    //                                                                    }
    //                                                                    else if (mr == -14)
    //                                                                    {
    //                                                                        strgetval = "COD";
    //                                                                    }
    //                                                                    else if (mr == -15)
    //                                                                    {
    //                                                                        strgetval = "OOD";
    //                                                                    }
    //                                                                    else if (mr == -16)
    //                                                                    {
    //                                                                        strgetval = "OD";
    //                                                                    }
    //                                                                    else if (mr == -17)
    //                                                                    {
    //                                                                        strgetval = "LA";
    //                                                                    }
    //                                                                    else if (mr == -18)
    //                                                                    {
    //                                                                        strgetval = "RAA";
    //                                                                    }
    //                                                                    else
    //                                                                    {
    //                                                                        strgetval = Convert.ToString(dv2[p]["marks_obtained"]);
    //                                                                    }

    //                                                                    // -------------- Term Total 18-4-15
    //                                                                    string rollnoA218 = ds2.Tables[0].Rows[ik1]["roll_no"].ToString();
    //                                                                    query35A = "SELECT distinct roll_no,Convert(nvarchar(15),s.Istype)+' ('+Convert(nvarchar(15),conversion)+')' as Istype,s.Istype as isty,Exammark, conversion,s.subject_no, Criteria_no FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " and c.Istype='Calculate 1' AND C.subject_no  = " + dropsubname.SelectedValue + " and roll_no='" + rollnoA218 + "' order by Criteria_no";
    //                                                                    ds3A = d2.select_method_wo_parameter(query35A, "Text");
    //                                                                    if (!hastble4.ContainsKey("Term Total"))
    //                                                                    {
    //                                                                        //column = column + 1;
    //                                                                        //dTab.Columns.Add("Term Total" + " " + ds3A.Tables[0].Rows[p]["conversion"].ToString());
    //                                                                        ////addcheck.Add(column);
    //                                                                        //addterm.Add(column);
    //                                                                        //hastble4.Add("Term Total", column);
    //                                                                        int cnt22 = Convert.ToInt32(hastble4["Term Total"].ToString());
    //                                                                        dr[cnt22] = Convert.ToString(ds3A.Tables[0].Rows[0]["Exammark"].ToString());
    //                                                                    }

    //                                                                    //if (ds3A.Tables[0].Rows.Count > 0) *******************************
    //                                                                    else
    //                                                                    {
    //                                                                        //column = column + 1;
    //                                                                        int cnt22 = Convert.ToInt32(hastble4["Term Total"].ToString());
    //                                                                        dr[cnt22] = Convert.ToString(ds3A.Tables[0].Rows[0]["Exammark"].ToString());
    //                                                                        //cribst = "";

    //                                                                        // ----------------- term grade ***************************
    //                                                                        string mark = ds3A.Tables[0].Rows[0]["Exammark"].ToString();
    //                                                                        string best = "select Mark_Grade from Grade_Master where '" + mark + "' between frange and trange  and Criteria='" + ds3A.Tables[0].Rows[0]["isty"].ToString() + "'";
    //                                                                        dsgrd = d2.select_method_wo_parameter(best, "Text");
    //                                                                        int cnt2222 = Convert.ToInt32(hastble4["Grade "] + ik1.ToString());
    //                                                                        if (dsgrd.Tables[0].Rows.Count > 0)
    //                                                                        {
    //                                                                            dr[cnt22 + 1] = Convert.ToString(dsgrd.Tables[0].Rows[ik1]["Mark_Grade"].ToString());
    //                                                                        }
    //                                                                        else
    //                                                                        {
    //                                                                            //dr[cnt22 + 1] = "7th";
    //                                                                            dr[cnt22 + 1] = " ";
    //                                                                        }
    //                                                                        // ----------------- term grade ***************************
    //                                                                    }
    //                                                                    // -------------- Term Total ****************** Rajesh 1 up

    //                                                                    //string rollnoA2 = ds2.Tables[0].Rows[ik1]["roll_no"].ToString();
    //                                                                    //query35A2 = "SELECT * FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " AND Criteria_no = '" + cribst + "' AND C.subject_no  = " + dropsubname.SelectedValue + " and roll_no='" + rollnoA2 + "' order by Criteria_no";
    //                                                                    //ds3A2 = d2.select_method_wo_parameter(query35A2, "Text");

    //                                                                    //if (ds3A.Tables[0].Rows.Count > 0)
    //                                                                    //{
    //                                                                    //    string mark = ds3A.Tables[0].Rows[0]["Exammark"].ToString();

    //                                                                    //    string bestquery = "select Mark_Grade from Grade_Master where '" + mark + "' between frange and trange and Criteria='" + ds3A.Tables[0].Rows[0]["isty"].ToString() + "'";
    //                                                                    //    dsgrd = d2.select_method_wo_parameter(bestquery, "Text");
    //                                                                    //    column = column + 1;
    //                                                                    //    int cnt22 = Convert.ToInt32(hastble4["Best "] + p.ToString());
    //                                                                    //    int cnt222 = Convert.ToInt32(hastble4["Grade " + p].ToString());
    //                                                                    //    dr[cnt22 + 1] = Convert.ToString(ds3A2.Tables[0].Rows[0]["Exammark"].ToString());
    //                                                                    //    //column = column + 1;
    //                                                                    //    if (dsgrd.Tables[0].Rows.Count > 0)
    //                                                                    //    {
    //                                                                    //        dr[cnt222 + 1] = Convert.ToString(dsgrd.Tables[0].Rows[0]["Mark_Grade"].ToString());
    //                                                                    //    }
    //                                                                    //    else
    //                                                                    //    {
    //                                                                    //        dr[cnt222 + 1] = " ";
    //                                                                    //    }
    //                                                                    //    cribst = "";
    //                                                                    //}

    //                                                                    //string rollnoA21 = ds2.Tables[0].Rows[ik1]["roll_no"].ToString();
    //                                                                    //query35A1 = "SELECT distinct roll_no, s.Istype, Exammark, conversion, Criteria_no FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " and c.Istype='Calculate 1' AND C.subject_no  = " + dropsubname.SelectedValue + " and roll_no='" + rollnoA21 + "' order by Criteria_no";
    //                                                                    //ds3A1 = d2.select_method_wo_parameter(query35A1, "Text");
    //                                                                    //if (ds3A1.Tables[0].Rows.Count > 0)
    //                                                                    //{
    //                                                                    //    if (hastble4.Contains("Term Total"))
    //                                                                    //    {
    //                                                                    //        int cnt220 = Convert.ToInt32(hastble4["Term Total"].ToString());
    //                                                                    //        dr[cnt220] = Convert.ToString(ds3A1.Tables[0].Rows[0]["Exammark"].ToString());
    //                                                                    //    }

    //                                                                    //}                                                                        
    //                                                                }
    //                                                            }
    //                                                        }
    //                                                    }
    //                                                    // ************************ Query-2 add Tool Name ************************
    //                                                }
    //                                            }

    //                                            dTab.Rows.Add(dr);
    //                                            if (!hastble3.ContainsKey(dv2[0]["roll_no"]))
    //                                            {
    //                                                hastble3.Add(dv2[0]["roll_no"], dv2[0]["roll_no"]);
    //                                            }
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }

    //                    if (count > 0)
    //                    {
    //                        reportgrid1.DataSource = dTab;
    //                        reportgrid1.DataBind();
    //                        //dsgrid = reportgrid1.Columns.Count;

    //                        vargrid = dTab.Columns.Count;

    //                        if (reportgrid1.Rows.Count > 0)
    //                        {
    //                            if (addcheck.Count > 0)
    //                            {
    //                                for (int add = 0; add < addcheck.Count; add++)
    //                                {
    //                                    string index = Convert.ToString(addcheck[add]);
    //                                    reportgrid1.HeaderRow.Cells[Convert.ToInt32(index) + 1].BackColor = System.Drawing.Color.Linen;
    //                                    reportgrid1.HeaderRow.Cells[Convert.ToInt32(index) + 1].Width = 50;
    //                                    reportgrid1.HeaderRow.Cells[Convert.ToInt32(index) + 1].ForeColor = System.Drawing.Color.Brown;
    //                                    reportgrid1.HeaderRow.Cells[Convert.ToInt32(index) + 2].BackColor = System.Drawing.Color.Linen;
    //                                    reportgrid1.HeaderRow.Cells[Convert.ToInt32(index) + 2].ForeColor = System.Drawing.Color.Brown;
    //                                    //reportgrid1.HeaderRow.Cells[Convert.ToInt32(index)].ForeColor = System.Drawing.Color.Brown;
    //                                    reportgrid1.HeaderRow.Cells[Convert.ToInt32(index) + 2].Width = 70;
    //                                    reportgrid1.HeaderRow.Cells[Convert.ToInt32(index) + 1].Width = 100;
    //                                    for (int row = 0; row < reportgrid1.Rows.Count; row++)
    //                                    {
    //                                        reportgrid1.Rows[row].Cells[Convert.ToInt32(index) + 1].BackColor = System.Drawing.Color.Linen;
    //                                        reportgrid1.Rows[row].Cells[Convert.ToInt32(index) + 1].ForeColor = System.Drawing.Color.Brown;
    //                                        reportgrid1.Rows[row].Cells[Convert.ToInt32(index) + 2].BackColor = System.Drawing.Color.Linen;
    //                                        reportgrid1.Rows[row].Cells[Convert.ToInt32(index) + 2].ForeColor = System.Drawing.Color.Brown;
    //                                    }
    //                                }
    //                            }
    //                            if (addterm.Count > 0)
    //                            {
    //                                for (int add1 = 0; add1 < addterm.Count; add1++)
    //                                {
    //                                    string index1 = Convert.ToString(addterm[add1]);
    //                                    reportgrid1.HeaderRow.Cells[Convert.ToInt32(index1)].ForeColor = System.Drawing.Color.Blue;
    //                                    reportgrid1.HeaderRow.Cells[Convert.ToInt32(index1)].BackColor = System.Drawing.Color.Pink;
    //                                    reportgrid1.HeaderRow.Cells[Convert.ToInt32(index1) + 1].ForeColor = System.Drawing.Color.Blue;
    //                                    reportgrid1.HeaderRow.Cells[Convert.ToInt32(index1) + 1].BackColor = System.Drawing.Color.Pink;
    //                                    reportgrid1.HeaderRow.Cells[Convert.ToInt32(index1)].Width = 100;
    //                                    reportgrid1.HeaderRow.Cells[Convert.ToInt32(index1) + 1].Width = 70;
    //                                    for (int row1 = 0; row1 < reportgrid1.Rows.Count; row1++)
    //                                    {
    //                                        reportgrid1.Rows[row1].Cells[Convert.ToInt32(index1)].ForeColor = System.Drawing.Color.Blue;
    //                                        reportgrid1.Rows[row1].Cells[Convert.ToInt32(index1)].BackColor = System.Drawing.Color.Pink;
    //                                        reportgrid1.Rows[row1].Cells[Convert.ToInt32(index1) + 1].ForeColor = System.Drawing.Color.Blue;
    //                                        reportgrid1.Rows[row1].Cells[Convert.ToInt32(index1) + 1].BackColor = System.Drawing.Color.Pink;
    //                                    }
    //                                }
    //                            }
    //                        }
    //                        reportgrid1.Visible = true;
    //                        lblerrormsg.Visible = false;
    //                        g1btnexcel.Visible = true;
    //                        g1btnprint.Visible = true;
    //                        //lblcalc.Visible = true;
    //                        //txtcalc.Visible = true;
    //                    }
    //                    else
    //                    {
    //                        lblerrormsg.Visible = true;
    //                        lblerrormsg.Text = "No Records Found";
    //                        reportgrid1.Visible = false;
    //                        g1btnprint.Visible = false;
    //                        g1btnexcel.Visible = false;
    //                        //lblcalc.Visible = false;
    //                        //txtcalc.Visible = false;
    //                    }
    //                }
    //                else
    //                {
    //                    lblerrormsg.Visible = true;
    //                    lblerrormsg.Text = "No Records Found";
    //                    reportgrid1.Visible = false;
    //                    g1btnprint.Visible = false;
    //                    g1btnexcel.Visible = false;
    //                    //lblcalc.Visible = false;
    //                    //txtcalc.Visible = false;
    //                }
    //            }
    //            else
    //            {
    //                if (dropreportdisplay.SelectedItem.Text == "Mark")
    //                {
    //                    int count = 1;
    //                    string cribst = "";
    //                    string query35A = "";
    //                    int test = 0;
    //                    string query35A2 = "";
    //                    DataRow dr = null;
    //                    DataSet ds3A = new DataSet();
    //                    ArrayList checkarray1 = new ArrayList();
    //                    DataTable dtble1 = new DataTable();
    //                    Hashtable hastble1 = new Hashtable();
    //                    Hashtable hastble3 = new Hashtable();
    //                    Hashtable hastbleee = new Hashtable();
    //                    DataSet ds3A1 = new DataSet();
    //                    DataSet dsgrd = new DataSet();
    //                    DataSet ds3A2 = new DataSet();
    //                    ArrayList addterm = new ArrayList();
    //                    dTab.Columns.Add("S.No", typeof(Int32));
    //                    dTab.Columns.Add("Roll No", typeof(string));
    //                    dTab.Columns.Add("Reg No", typeof(string));
    //                    dTab.Columns.Add("Student Name", typeof(string));
    //                    Hashtable hastb1 = new Hashtable();

    //                    // ************************ Query-1 for FA 1 and SA 1 ************************
    //                    string query1 = "";
    //                    query1 = "SELECT Istype,CRITERIA_NO FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " AND M.subject_no = " + dropsubname.SelectedValue + " and Criteria_no!='0' order by Istype,Criteria_no";
    //                    ds1 = d2.select_method_wo_parameter(query1, "Text");

    //                    DataSet dsbest = new DataSet();
    //                    ArrayList addcheck1 = new ArrayList();
    //                    if (ds1.Tables[0].Rows.Count > 0)
    //                    {
    //                        for (int ik = 0; ik < ds1.Tables[0].Rows.Count; ik++)
    //                        {
    //                            if (!hastble1.ContainsKey(ds1.Tables[0].Rows[ik]["Istype"].ToString()))
    //                            {
    //                                string istyp = ds1.Tables[0].Rows[ik]["Istype"].ToString();
    //                                string crteria = ds1.Tables[0].Rows[ik]["CRITERIA_NO"].ToString();
    //                                ds1.Tables[0].DefaultView.RowFilter = "CRITERIA_NO='" + crteria + "'";
    //                                dv1 = ds1.Tables[0].DefaultView;
    //                                if (dv1.Count > 0)
    //                                {
    //                                    string query31 = "";
    //                                    query31 = "SELECT distinct C.Istype, Criteria_no FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " AND C.subject_no  = " + dropsubname.SelectedValue + " and Criteria_no!='0' order by Criteria_no";
    //                                    DataSet dset31 = new DataSet();
    //                                    dset31 = d2.select_method_wo_parameter(query31, "Text");
    //                                    if (dset31.Tables[0].Rows.Count > 0)
    //                                    {
    //                                        // *********** add FA-1 and SA-1 name ***********
    //                                        if (checkarray1.Count >= 0)
    //                                        {
    //                                            dtble1 = dv1.ToTable();
    //                                            markcounttestvalue(dtble1);

    //                                            hastb1.Add(dtble1, ToString());
    //                                            checkarray1.Add(crteria);
    //                                            string valu = Convert.ToString(dtble1);
    //                                        }
    //                                        // *********** add FA-1 and SA-1 name ***********
    //                                    }

    //                                    // ************************ Query-2 for Criteria ************************
    //                                    String query2 = "";
    //                                    query2 = "SELECT U.roll_no,REG_NO,STUD_NAME,marks_obtained,Convert(nvarchar(15),Criteria)+' ('+Convert(nvarchar(15),e.max_mark)+')' as Criteria ,c.criteria_no,c.max_mark  FROM Result u,Registration r,Exam_type e,CriteriaForInternal c WHERE U.roll_no = R.Roll_No AND U.exam_code = E.exam_code AND E.criteria_no = C.Criteria_no AND subject_no = " + dropsubname.SelectedValue + " order by u.roll_no,STUD_NAME,e.exam_date,c.criteria_no";
    //                                    ds2 = d2.select_method_wo_parameter(query2, "Text");

    //                                    for (int ik1 = 0; ik1 < ds2.Tables[0].Rows.Count; ik1++)
    //                                    {
    //                                        string rollno1 = ds2.Tables[0].Rows[ik1]["roll_no"].ToString();
    //                                        ds2.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno1 + "'";
    //                                        dv2 = ds2.Tables[0].DefaultView;
    //                                        if (dv2.Count > 0)
    //                                        {
    //                                            if (!hastble3.ContainsKey(dv2[0]["roll_no"]))
    //                                            {
    //                                                dr = dTab.NewRow();
    //                                                dr[0] = count;
    //                                                dr[1] = dv2[0]["roll_no"].ToString();
    //                                                dr[2] = dv2[0]["REG_NO"].ToString();
    //                                                dr[3] = dv2[0]["Stud_Name"].ToString();

    //                                                string query35 = "";
    //                                                count++;
    //                                                Hashtable hastble2 = new Hashtable();
    //                                                {
    //                                                    {
    //                                                        // ************************ Query-2 add Tool Name ************************
    //                                                        for (int p = 0; p < dv2.Count; p++)
    //                                                        {
    //                                                            if (!hastble2.ContainsKey(dv2[p]["roll_no"] + "-" + dv2[p]["Criteria"]))
    //                                                            {
    //                                                                if (dv2[p]["roll_no"] + "-" + dv2[p]["Criteria"] == dv2[p]["roll_no"] + "-" + dv2[p]["Criteria"])
    //                                                                {
    //                                                                    if (p == 0)
    //                                                                    {
    //                                                                        cribst = "";
    //                                                                    }
    //                                                                    if (cribst == "")
    //                                                                    {
    //                                                                        cribst = dv2[p]["criteria_no"].ToString();
    //                                                                    }
    //                                                                    else
    //                                                                    {
    //                                                                        cribst = cribst + "," + dv2[p]["criteria_no"].ToString();
    //                                                                    }
    //                                                                    if (!hastble4.ContainsKey(dv2[p]["Criteria"]))
    //                                                                    {
    //                                                                        hastble2.Add(dv2[p]["roll_no"] + "-" + dv2[p]["Criteria"], dv2[p]["Criteria"]);
    //                                                                        column++;

    //                                                                        // *********** add Best ***********
    //                                                                        string mm = "SELECT Cam_Option FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " AND M.subject_no = " + dropsubname.SelectedValue + "";
    //                                                                        DataSet dsv = new DataSet();
    //                                                                        dsv = d2.select_method_wo_parameter(mm, "text");
    //                                                                        if (dsv.Tables[0].Rows.Count > 0)
    //                                                                        {
    //                                                                            string bestquery = "SELECT distinct C.Istype, Criteria_no FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + "  AND C.subject_no  = " + dropsubname.SelectedValue + "  and Criteria_no!='0' and Criteria_no='" + cribst + "' order by Criteria_no";
    //                                                                            dsbest = d2.select_method_wo_parameter(bestquery, "Text");
    //                                                                            if (dsbest.Tables[0].Rows.Count == 0)
    //                                                                            {
    //                                                                                if (!hastble4.ContainsKey(dv2[p]["Criteria"].ToString()))
    //                                                                                {
    //                                                                                    string[] pen = dv2[p]["Criteria"].ToString().Split(' ');
    //                                                                                    if (pen[0].ToString().ToLower().Contains("pen"))
    //                                                                                    {
    //                                                                                        test++;
    //                                                                                        if (test == 1)
    //                                                                                        {
    //                                                                                            {
    //                                                                                                string rollnoA218 = ds2.Tables[0].Rows[ik1]["roll_no"].ToString();
    //                                                                                                query35A = "SELECT distinct roll_no,Convert(nvarchar(15),s.Istype)+' ('+Convert(nvarchar(15),conversion)+')' as Istype,Exammark, conversion,s.subject_no, Criteria_no FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " and c.Istype='Calculate 1' AND C.subject_no  = " + dropsubname.SelectedValue + " and roll_no='" + rollnoA218 + "' order by Criteria_no";
    //                                                                                                ds3A = d2.select_method_wo_parameter(query35A, "Text");

    //                                                                                                if (!hastble4.ContainsKey("Term Total"))
    //                                                                                                {
    //                                                                                                    column = column + 1;
    //                                                                                                    dTab.Columns.Add("Term Total" + " " + ds3A.Tables[0].Rows[ik1]["conversion"].ToString());
    //                                                                                                    //addcheck.Add(column);
    //                                                                                                    addterm.Add(column);
    //                                                                                                    hastble4.Add("Term Total", column);
    //                                                                                                }
    //                                                                                                string rollnoA21 = ds2.Tables[0].Rows[ik1]["roll_no"].ToString();
    //                                                                                                query35A = "SELECT * FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " and c.Istype='Calculate 1' AND C.subject_no  = " + dropsubname.SelectedValue + " and roll_no='" + rollnoA21 + "' order by Criteria_no";
    //                                                                                                ds3A = d2.select_method_wo_parameter(query35A, "Text");
    //                                                                                                if (ds3A.Tables[0].Rows.Count > 0)
    //                                                                                                {
    //                                                                                                    int cnt22 = Convert.ToInt32(hastble4["Term Total"].ToString());
    //                                                                                                    dr[cnt22] = Convert.ToString(ds3A.Tables[0].Rows[0]["Exammark"].ToString());
    //                                                                                                    cribst = "";
    //                                                                                                }
    //                                                                                            }
    //                                                                                        }
    //                                                                                        else
    //                                                                                        {
    //                                                                                            string rollnoA218 = ds2.Tables[0].Rows[ik1]["roll_no"].ToString();
    //                                                                                            query35A = "SELECT distinct roll_no,Convert(nvarchar(15),s.Istype)+' ('+Convert(nvarchar(15),conversion)+')' as Istype,Exammark, conversion,s.subject_no, Criteria_no FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " and c.Istype='Calculate 1' AND C.subject_no  = " + dropsubname.SelectedValue + " and roll_no='" + rollnoA218 + "' order by Criteria_no";
    //                                                                                            ds3A = d2.select_method_wo_parameter(query35A, "Text");
    //                                                                                            if (!hastble4.ContainsKey("Term Total"))
    //                                                                                            {
    //                                                                                                column = column + 1;
    //                                                                                                dTab.Columns.Add("Term Total" + " " + ds3A.Tables[0].Rows[p]["conversion"].ToString());
    //                                                                                                //addcheck.Add(column);
    //                                                                                                addterm.Add(column);
    //                                                                                                hastble4.Add("Term Total", column);
    //                                                                                            }
    //                                                                                            string rollnoA21 = ds2.Tables[0].Rows[ik1]["roll_no"].ToString();
    //                                                                                            query35A = "SELECT * FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " and c.Istype='Calculate 1' AND C.subject_no  = " + dropsubname.SelectedValue + " and roll_no='" + rollnoA21 + "' order by Criteria_no";
    //                                                                                            ds3A = d2.select_method_wo_parameter(query35A, "Text");
    //                                                                                            if (ds3A.Tables[0].Rows.Count > 0)
    //                                                                                            {
    //                                                                                                int cnt22 = Convert.ToInt32(hastble4["Term Total"].ToString());
    //                                                                                                dr[cnt22] = Convert.ToString(ds3A.Tables[0].Rows[0]["Exammark"].ToString());
    //                                                                                                cribst = "";
    //                                                                                            }
    //                                                                                        }
    //                                                                                    }

    //                                                                                    dTab.Columns.Add(dv2[p]["Criteria"].ToString());
    //                                                                                    sbst = dv2[p]["max_mark"].ToString();
    //                                                                                    hastble4.Add(dv2[p]["Criteria"], column);
    //                                                                                    int cnt = column;
    //                                                                                    dr[cnt + 1] = Convert.ToString(dv2[p]["marks_obtained"]);
    //                                                                                }
    //                                                                                else
    //                                                                                {
    //                                                                                    int cnt = column;
    //                                                                                    dr[cnt + 1] = Convert.ToString(dv2[p]["marks_obtained"]);
    //                                                                                }
    //                                                                            }
    //                                                                            else
    //                                                                            {
    //                                                                                if (!hastble4.ContainsKey(dv2[p]["Criteria"].ToString()))
    //                                                                                {
    //                                                                                    dTab.Columns.Add(dv2[p]["Criteria"].ToString());
    //                                                                                    hastble4.Add(dv2[p]["Criteria"], column);
    //                                                                                }
    //                                                                                int cnt = column;
    //                                                                                dr[cnt + 1] = Convert.ToString(dv2[p]["marks_obtained"]);

    //                                                                                b++;
    //                                                                                if (!hastble4.ContainsKey("Best " + p))
    //                                                                                {
    //                                                                                    if (!hastb1.ContainsKey(dv1[ik]["Istype"].ToString()))
    //                                                                                    {
    //                                                                                        column = column + 1;

    //                                                                                        dTab.Columns.Add("Best " + b + " " + ds1.Tables[0].Rows[b - 1]["Istype"].ToString() + " " + "(" + sbst + ")", typeof(string));
    //                                                                                        addcheck1.Add(column);
    //                                                                                        hastble4.Add("Best " + p, column);

    //                                                                                        if ((dv2.Count - 1) == p)
    //                                                                                        {
    //                                                                                            column = column + 1;
    //                                                                                            string rollnoA210 = ds2.Tables[0].Rows[ik1]["roll_no"].ToString();
    //                                                                                            query35A = "SELECT distinct roll_no,Convert(nvarchar(15),s.Istype)+' ('+Convert(nvarchar(15),conversion)+')' as Istype,Exammark, conversion,s.subject_no, Criteria_no FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " and c.Istype='Calculate 1' AND C.subject_no  = " + dropsubname.SelectedValue + " and roll_no='" + rollnoA210 + "' order by Criteria_no";
    //                                                                                            ds3A = d2.select_method_wo_parameter(query35A, "Text");

    //                                                                                            dTab.Columns.Add("Term Total" + " " + ds3A.Tables[0].Rows[0]["conversion"].ToString());
    //                                                                                            addterm.Add(column);
    //                                                                                            hastble4.Add("Term Total", column);

    //                                                                                            int cnt22 = Convert.ToInt32(hastble4["Term Total"].ToString());
    //                                                                                            dr[cnt22 + 1] = Convert.ToString(ds3A.Tables[0].Rows[0]["Exammark"].ToString());
    //                                                                                        }

    //                                                                                    }
    //                                                                                }

    //                                                                                string rollno2 = dv2[p]["roll_no"].ToString();
    //                                                                                query35 = "SELECT distinct roll_no,Convert(nvarchar(15),s.Istype)+' ('+Convert(nvarchar(15),conversion)+')' as Istype,Exammark, conversion,s.subject_no, Criteria_no FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " AND Criteria_no = '" + cribst + "' AND C.subject_no  = " + dropsubname.SelectedValue + " and roll_no='" + rollno2 + "' order by Criteria_no";
    //                                                                                ds3 = d2.select_method_wo_parameter(query35, "Text");
    //                                                                                if (ds3.Tables[0].Rows.Count > 0)
    //                                                                                {
    //                                                                                    int cnt22 = Convert.ToInt32(hastble4["Best " + p].ToString());
    //                                                                                    //int cnt22 = column;
    //                                                                                    dr[cnt22 + 1] = Convert.ToString(ds3.Tables[0].Rows[0]["Exammark"].ToString());
    //                                                                                    cribst = "";
    //                                                                                }
    //                                                                            }
    //                                                                        }
    //                                                                    }
    //                                                                    else
    //                                                                    {
    //                                                                        string rollnoA210 = ds2.Tables[0].Rows[ik1]["roll_no"].ToString();
    //                                                                        query35A = "SELECT distinct roll_no,Convert(nvarchar(15),s.Istype)+' ('+Convert(nvarchar(15),conversion)+')' as Istype,Exammark, conversion,s.subject_no, Criteria_no FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " and c.Istype='Calculate 1' AND C.subject_no  = " + dropsubname.SelectedValue + " and roll_no='" + rollnoA210 + "' order by Criteria_no";
    //                                                                        ds3A = d2.select_method_wo_parameter(query35A, "Text");
    //                                                                        if (!hastble4.ContainsKey("Term Total"))
    //                                                                        {
    //                                                                            column = column + 1;
    //                                                                            //dTab.Columns.Add("Term Total" + " " + ds3A.Tables[0].Rows[p]["conversion"].ToString());
    //                                                                            //hastble4.Add("Term Total", column);
    //                                                                        }
    //                                                                        else
    //                                                                        {
    //                                                                            int cnt22 = Convert.ToInt32(hastble4["Term Total"].ToString());
    //                                                                            dr[cnt22 + 1] = Convert.ToString(ds3A.Tables[0].Rows[0]["Exammark"].ToString());
    //                                                                        }

    //                                                                        int nm = Convert.ToInt32(hastble4[dv2[p]["Criteria"]]);

    //                                                                        int mr = Convert.ToInt32(dv2[p]["marks_obtained"]);
    //                                                                        if (mr == -1)
    //                                                                        {
    //                                                                            strgetval = "AAA";
    //                                                                        }
    //                                                                        else if (mr == -2)
    //                                                                        {
    //                                                                            strgetval = "EL";
    //                                                                        }
    //                                                                        else if (mr == -3)
    //                                                                        {
    //                                                                            strgetval = "EOD";
    //                                                                        }
    //                                                                        else if (mr == -4)
    //                                                                        {
    //                                                                            strgetval = "ML";
    //                                                                        }
    //                                                                        else if (mr == -5)
    //                                                                        {
    //                                                                            strgetval = "SOD";
    //                                                                        }
    //                                                                        else if (mr == -6)
    //                                                                        {
    //                                                                            strgetval = "NSS";
    //                                                                        }
    //                                                                        else if (mr == -7)
    //                                                                        {
    //                                                                            strgetval = "NJ";
    //                                                                        }
    //                                                                        else if (mr == -8)
    //                                                                        {
    //                                                                            strgetval = "S";
    //                                                                        }
    //                                                                        else if (mr == -9)
    //                                                                        {
    //                                                                            strgetval = "L";
    //                                                                        }
    //                                                                        else if (mr == -10)
    //                                                                        {
    //                                                                            strgetval = "NCC";
    //                                                                        }
    //                                                                        else if (mr == -11)
    //                                                                        {
    //                                                                            strgetval = "HS";
    //                                                                        }
    //                                                                        else if (mr == -12)
    //                                                                        {
    //                                                                            strgetval = "PP";
    //                                                                        }
    //                                                                        else if (mr == -13)
    //                                                                        {
    //                                                                            strgetval = "SYOD";
    //                                                                        }
    //                                                                        else if (mr == -14)
    //                                                                        {
    //                                                                            strgetval = "COD";
    //                                                                        }
    //                                                                        else if (mr == -15)
    //                                                                        {
    //                                                                            strgetval = "OOD";
    //                                                                        }
    //                                                                        else if (mr == -16)
    //                                                                        {
    //                                                                            strgetval = "OD";
    //                                                                        }
    //                                                                        else if (mr == -17)
    //                                                                        {
    //                                                                            strgetval = "LA";
    //                                                                        }
    //                                                                        else if (mr == -18)
    //                                                                        {
    //                                                                            strgetval = "RAA";
    //                                                                        }
    //                                                                        else
    //                                                                        {
    //                                                                            strgetval = Convert.ToString(dv2[p]["marks_obtained"]);
    //                                                                        }

    //                                                                        // -------------- Term Total
    //                                                                        string rollnoA218 = ds2.Tables[0].Rows[ik1]["roll_no"].ToString();
    //                                                                        query35A = "SELECT distinct roll_no,Convert(nvarchar(15),s.Istype)+' ('+Convert(nvarchar(15),conversion)+')' as Istype,Exammark, conversion,s.subject_no, Criteria_no FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " and c.Istype='Calculate 1' AND C.subject_no  = " + dropsubname.SelectedValue + " and roll_no='" + rollnoA218 + "' order by Criteria_no";
    //                                                                        ds3A = d2.select_method_wo_parameter(query35A, "Text");
    //                                                                        if (!hastble4.ContainsKey("Term Total"))
    //                                                                        {
    //                                                                            //column = column + 1;
    //                                                                            //dTab.Columns.Add("Term Total" + " " + ds3A.Tables[0].Rows[p]["conversion"].ToString());
    //                                                                            ////addcheck.Add(column);
    //                                                                            //hastble4.Add("Term Total", column);
    //                                                                            int cnt22 = Convert.ToInt32(hastble4["Term Total"].ToString());
    //                                                                            dr[cnt22 + 1] = Convert.ToString(ds3A.Tables[0].Rows[0]["Exammark"].ToString());
    //                                                                        }
    //                                                                        else
    //                                                                        {
    //                                                                            int cnt22 = Convert.ToInt32(hastble4["Term Total"].ToString());
    //                                                                            dr[cnt22] = Convert.ToString(ds3A.Tables[0].Rows[0]["Exammark"].ToString());
    //                                                                            //cribst = "";
    //                                                                        }
    //                                                                        // -------------- Term Total

    //                                                                        string rollnoA2 = ds2.Tables[0].Rows[ik1]["roll_no"].ToString();
    //                                                                        query35A2 = "SELECT * FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " AND Criteria_no = '" + cribst + "' AND C.subject_no  = " + dropsubname.SelectedValue + " and roll_no='" + rollnoA2 + "' order by Criteria_no";
    //                                                                        ds3A2 = d2.select_method_wo_parameter(query35A2, "Text");

    //                                                                        if (ds3A2.Tables[0].Rows.Count > 0)
    //                                                                        {

    //                                                                            column = column + 1;

    //                                                                            int cnt22 = Convert.ToInt32(hastble4["Best " + p].ToString());

    //                                                                            dr[cnt22 + 1] = Convert.ToString(ds3A2.Tables[0].Rows[0]["Exammark"].ToString());

    //                                                                            cribst = "";
    //                                                                        }
    //                                                                    }
    //                                                                }
    //                                                            }
    //                                                        }
    //                                                        // ************************ Query-2 add Tool Name ************************
    //                                                    }
    //                                                }

    //                                                dTab.Rows.Add(dr);
    //                                                if (!hastble3.ContainsKey(dv2[0]["roll_no"]))
    //                                                {
    //                                                    hastble3.Add(dv2[0]["roll_no"], dv2[0]["roll_no"]);
    //                                                }
    //                                            }
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                        }

    //                        if (count > 0)
    //                        {
    //                            reportgrid1.DataSource = dTab;
    //                            reportgrid1.DataBind();

    //                            if (reportgrid1.Rows.Count > 0)
    //                            {
    //                                if (addcheck1.Count > 0)
    //                                {
    //                                    for (int add = 0; add < addcheck1.Count; add++)
    //                                    {
    //                                        string index = Convert.ToString(addcheck1[add]);
    //                                        reportgrid1.HeaderRow.Cells[Convert.ToInt32(index) + 1].BackColor = System.Drawing.Color.Linen;
    //                                        reportgrid1.HeaderRow.Cells[Convert.ToInt32(index) + 1].Width = 50;
    //                                        reportgrid1.HeaderRow.Cells[Convert.ToInt32(index) + 1].ForeColor = System.Drawing.Color.Brown;
    //                                        reportgrid1.HeaderRow.Cells[Convert.ToInt32(index) + 2].Width = 60;
    //                                        for (int row = 0; row < reportgrid1.Rows.Count; row++)
    //                                        {
    //                                            reportgrid1.Rows[row].Cells[Convert.ToInt32(index) + 1].BackColor = System.Drawing.Color.Linen;
    //                                            reportgrid1.Rows[row].Cells[Convert.ToInt32(index) + 1].ForeColor = System.Drawing.Color.Brown;
    //                                        }
    //                                    }
    //                                }
    //                                if (addterm.Count > 0)
    //                                {
    //                                    for (int add1 = 0; add1 < addterm.Count; add1++)
    //                                    {
    //                                        string index1 = Convert.ToString(addterm[add1]);
    //                                        reportgrid1.HeaderRow.Cells[Convert.ToInt32(index1)].ForeColor = System.Drawing.Color.Blue;
    //                                        reportgrid1.HeaderRow.Cells[Convert.ToInt32(index1)].BackColor = System.Drawing.Color.Pink;
    //                                        for (int row1 = 0; row1 < reportgrid1.Rows.Count; row1++)
    //                                        {
    //                                            reportgrid1.Rows[row1].Cells[Convert.ToInt32(index1)].ForeColor = System.Drawing.Color.Blue;
    //                                            reportgrid1.Rows[row1].Cells[Convert.ToInt32(index1)].BackColor = System.Drawing.Color.Pink;
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                            reportgrid1.Visible = true;
    //                            lblerrormsg.Visible = false;
    //                            g1btnexcel.Visible = true;
    //                            g1btnprint.Visible = true;
    //                            //lblcalc.Visible = true;
    //                            //txtcalc.Visible = true;
    //                        }
    //                        else
    //                        {
    //                            lblerrormsg.Visible = true;
    //                            lblerrormsg.Text = "No Records Found";
    //                            reportgrid1.Visible = false;
    //                            g1btnprint.Visible = false;
    //                            g1btnexcel.Visible = false;
    //                            //lblcalc.Visible = false;
    //                            //txtcalc.Visible = false;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        lblerrormsg.Visible = true;
    //                        lblerrormsg.Text = "No Records Found";
    //                        reportgrid1.Visible = false;
    //                        g1btnprint.Visible = false;
    //                        g1btnexcel.Visible = false;
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            lblerrormsg.Visible = true;
    //            lblerrormsg.Text = "No Records Found";
    //            reportgrid1.Visible = false;
    //            g1btnprint.Visible = false;
    //            g1btnexcel.Visible = false;
    //            //lblcalc.Visible = false;
    //            //txtcalc.Visible = false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblerrormsg.Visible = true;
    //        lblerrormsg.Text = ex.ToString();
    //    }
    //}

    public void bindsec()
    {
        try
        {
            //string dyear = "";
            //string dstand = "";
            dropsec.Enabled = false;
            dropsec.Items.Clear();
            hat.Clear();
            //if (dropyear.SelectedValue != "")
            //{
            //  dyear = dropyear.SelectedValue.ToString();
            //}

            //  if (ddstandard.SelectedValue != "")
            //    {
            //        dstand = dstand + ',' + ddstandard.Items[ij].Value;
            //    }
            //               

            //if (dropyear.SelectedValue == "true" && ddstandard.SelectedValue == "true")
            //{
            ds = d2.BindSectionDetail(dropyear.SelectedValue, ddstandard.SelectedValue);
            int count5 = ds.Tables[0].Rows.Count;
            if (count5 > 0)
            {
                dropsec.DataSource = ds;
                dropsec.DataTextField = "sections";
                dropsec.DataValueField = "sections";
                dropsec.DataBind();
                dropsec.Enabled = true;
                dropsec.Items.Insert(0, "All");
            }
            //}
            else
            {
                dropsec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    //public void bindtestname(string val22)
    //{
    //    try
    //    {
    //        int cout = 0;
    //        string query = "";
    //        int i = 0;
    //        chcklisttestname.Items.Clear();
    //        if (val22 != "")
    //        {
    //            query = "select distinct c.criteria from criteriaforinternal c,registration r,syllabus_master s where r.degree_code=s.degree_code and r.batch_year=s.batch_year and c.syll_code=s.syll_code and cc=0 and delflag=0 and r.exam_flag<>'debar' and r.college_code='" + ddschool.Text.ToString() + "' and r.batch_year='" + dropyear.Text.ToString() + "' and s.semester='" + dropterm.SelectedItem.ToString() + "' order by criteria asc";
    //        }
    //        else
    //        {
    //            query = "select distinct c.criteria from criteriaforinternal c,registration r,syllabus_master s where r.degree_code=s.degree_code and r.batch_year=s.batch_year and c.syll_code=s.syll_code and cc=0 and delflag=0 and r.exam_flag<>'debar' and r.college_code='" + ddschool.Text.ToString() + "' and r.batch_year='" + dropyear.Text.ToString() + "' and s.semester='" + dropterm.SelectedItem.ToString() + "' order by criteria asc";
    //        }
    //        ds = da.select_method_wo_parameter(query, "Text");
    //        int count = ds.Tables[0].Rows.Count;
    //        if (count > 0)
    //        {
    //            chcklisttestname.DataSource = ds;
    //            chcklisttestname.DataTextField = "criteria";
    //            chcklisttestname.DataValueField = "criteria";
    //            chcklisttestname.DataBind();
    //        }

    //        if (chcklisttestname.Items.Count > 0)
    //        {
    //            for (i = 0; i < chcklisttestname.Items.Count; i++)
    //            {
    //                cout++;
    //                chcklisttestname.Items[i].Selected = true;
    //                txttestname.Text = "Test " + "(" + cout + ")";
    //            }
    //        }
    //        else
    //        {
    //            txttestname.Text = "--Select--";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    //protected void checktestname_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string buildvalue1 = "";
    //        string build1 = "";

    //        if (chcktestname.Checked == true)
    //        {
    //            for (int i = 0; i < chcklisttestname.Items.Count; i++)
    //            {

    //                if (chcktestname.Checked == true)
    //                {
    //                    chcklisttestname.Items[i].Selected = true;
    //                    txttestname.Text = "Test (" + (chcklisttestname.Items.Count) + ")";
    //                    build1 = chcklisttestname.Items[i].Value.ToString();
    //                    if (buildvalue1 == "")
    //                    {
    //                        buildvalue1 = build1;
    //                    }
    //                    else
    //                    {
    //                        buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            for (int i = 0; i < chcklisttestname.Items.Count; i++)
    //            {
    //                chcklisttestname.Items[i].Selected = false;
    //                txttestname.Text = "--Select--";

    //                chcklisttestname.ClearSelection();
    //                chcktestname.Checked = false;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    //protected void cheklisttestname_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int seatcount = 0;

    //        chcktestname.Checked = false;

    //        string buildvalue = "";
    //        string build = "";
    //        for (int i = 0; i < chcklisttestname.Items.Count; i++)
    //        {
    //            if (chcklisttestname.Items[i].Selected == true)
    //            {
    //                seatcount = seatcount + 1;
    //                txttestname.Text = "Select All";
    //                build = chcklisttestname.Items[i].Value.ToString();
    //                if (buildvalue == "")
    //                {
    //                    buildvalue = build;
    //                }
    //                else
    //                {
    //                    buildvalue = buildvalue + "'" + "," + "'" + build;

    //                }
    //            }
    //        }

    //        if (seatcount == chcklisttestname.Items.Count)
    //        {
    //            txttestname.Text = "Test (" + seatcount.ToString() + ")";
    //            chcktestname.Checked = true;
    //        }
    //        else if (seatcount == 0)
    //        {
    //            txttestname.Text = "--Select--";
    //            chcktestname.Text = "Select All";
    //        }
    //        else
    //        {
    //            txttestname.Text = "Test (" + seatcount.ToString() + ")";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //protected void reportgrid1_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //            e.Row.Cells[1].Width = 80;
    //            e.Row.Cells[2].Width = 200;
    //            if (Session["Regflag"].ToString() == "0")
    //            {
    //                e.Row.Cells[2].Visible = false;
    //            }

    //            if (Session["Rollflag"].ToString() == "0")
    //            {
    //                e.Row.Cells[1].Visible = false;
    //            }
    //            for (int j = 4; j >= col; j++)
    //            {
    //                e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;

    //            }
    //        }
    //        if (e.Row.RowType == DataControlRowType.Header)
    //        {
    //            if (Session["Regflag"].ToString() == "0")
    //            {
    //                e.Row.Cells[2].Visible = false;
    //            }
    //            if (Session["Rollflag"].ToString() == "0")
    //            {
    //                e.Row.Cells[1].Visible = false;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblerrormsg.Text = ex.ToString();
    //        lblerrormsg.Visible = true;
    //    }
    //}

    // *********** add FA-1 and SA-1 name ***********
    //protected void reportgrid1_DataBound(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DataSet dpen = new DataSet();
    //        GridView HeaderGrid = (GridView)sender;
    //        GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
    //        TableCell HeaderCell = null;
    //        if (columarray.Count > 0)
    //        {
    //            int col = 0;
    //            HeaderCell = new TableCell();
    //            HeaderCell.Text = "";
    //            if (Session["Regflag"].ToString() == "0" && Session["Rollflag"].ToString() != "0")
    //            {
    //                HeaderCell.ColumnSpan = 3;
    //            }
    //            else if (Session["Rollflag"].ToString() == "0" && Session["Regflag"].ToString() != "0")
    //            {
    //                HeaderCell.ColumnSpan = 3;
    //            }
    //            else if (Session["Rollflag"].ToString() == "0" && Session["Regflag"].ToString() == "0")
    //            {
    //                HeaderCell.ColumnSpan = 2;
    //            }
    //            else if (Session["Rollflag"].ToString() == "0" && Session["Regflag"].ToString() == "0")
    //            {
    //                HeaderCell.ColumnSpan = 4;
    //            }
    //            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
    //            HeaderGridRow.Cells.Add(HeaderCell);
    //            reportgrid1.Controls[0].Controls.AddAt(0, HeaderGridRow);
    //            for (int j = 0; j < columarray.Count; j++)
    //            {
    //                string value = Convert.ToString(columnhash[columarray[j]]);
    //                if (value.Trim() != "")
    //                {
    //                    col = col + Convert.ToInt32(value);
    //                }

    //                string querybound = "SELECT Istype,CRITERIA_NO FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " AND M.subject_no = " + dropsubname.SelectedValue + " and Criteria_no!='0' and Istype='" + columarray[j].ToString() + "' order by Criteria_no";
    //                ds1 = d2.select_method_wo_parameter(querybound, "Text");
    //                if (ds1.Tables[0].Rows.Count > 0)
    //                {
    //                    string var = ds1.Tables[0].Rows[0]["CRITERIA_NO"].ToString();
    //                    string[] array = var.Split(',');
    //                    if (array.Length > 0)
    //                    {
    //                        HeaderCell = new TableCell();
    //                        HeaderCell.Text = columarray[j].ToString();
    //                        HeaderCell.ColumnSpan = array.Length + 2;
    //                        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
    //                        HeaderGridRow.Cells.Add(HeaderCell);
    //                        reportgrid1.Controls[0].Controls.AddAt(0, HeaderGridRow);
    //                    }
    //                }
    //            }
    //            string pentst = "SELECT distinct Criteria FROM Result u,Registration r,Exam_type e,CriteriaForInternal c WHERE U.roll_no = R.Roll_No AND U.exam_code = E.exam_code AND E.criteria_no = C.Criteria_no AND subject_no = " + dropsubname.SelectedValue + " and Criteria like '%pen%' order by Criteria ";
    //            dpen = d2.select_method_wo_parameter(pentst, "Test");
    //            if (dpen.Tables[0].Rows.Count > 0)
    //            {
    //                string var1 = dpen.Tables[0].Rows[0]["Criteria"].ToString();
    //                string[] penarry = var1.Split(',');
    //                if(penarry.Length > 0)
    //                {
    //                HeaderCell = new TableCell();
    //                HeaderCell.Text = "Pen-Paper Test Marks";
    //                HeaderCell.ColumnSpan = dpen.Tables[0].Rows.Count + 1;
    //                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
    //                HeaderGridRow.Cells.Add(HeaderCell);
    //                reportgrid1.Controls[0].Controls.AddAt(1, HeaderGridRow);
    //                }
    //            }
    //            HeaderCell = new TableCell();
    //            HeaderCell.Text = "";
    //            HeaderCell.ColumnSpan = 4;
    //            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
    //            HeaderGridRow.Cells.Add(HeaderCell);
    //            reportgrid1.Controls[0].Controls.AddAt(0, HeaderGridRow);
    //        }

    //        ------------- mark
    //        if (dropreportdisplay.SelectedItem.Text == "Mark")
    //        {
    //            DataSet dpen1 = new DataSet();
    //            GridView HeaderGrid = (GridView)sender;
    //            GridViewRow HeaderGridRow1 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
    //            TableCell HeaderCell1 = null;
    //            if (columarray1.Count > 0)
    //            {
    //                int col = 0;
    //                HeaderCell1 = new TableCell();
    //                HeaderCell1.Text = "";
    //                if (Session["Regflag"].ToString() == "0" && Session["Rollflag"].ToString() != "0")
    //                {
    //                    HeaderCell1.ColumnSpan = 3;
    //                }
    //                else if (Session["Rollflag"].ToString() == "0" && Session["Regflag"].ToString() != "0")
    //                {
    //                    HeaderCell1.ColumnSpan = 3;
    //                }
    //                else if (Session["Rollflag"].ToString() == "0" && Session["Regflag"].ToString() == "0")
    //                {
    //                    HeaderCell1.ColumnSpan = 2;
    //                }
    //                else if (Session["Rollflag"].ToString() == "0" && Session["Regflag"].ToString() == "0")
    //                {
    //                    HeaderCell1.ColumnSpan = 4;
    //                }
    //                HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
    //                HeaderGridRow1.Cells.Add(HeaderCell1);
    //                reportgrid1.Controls[0].Controls.AddAt(0, HeaderGridRow1);
    //                for (int j = 0; j < columarray1.Count; j++)
    //                {
    //                    string valuee = Convert.ToString(columnhash1[columarray1[j]]);
    //                    if (valuee.Trim() != "")
    //                    {
    //                        col = col + Convert.ToInt32(valuee);
    //                    }

    //                    string querybound1 = "SELECT Istype,CRITERIA_NO FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = " + dropyear.SelectedItem.Text + " and degree_code = " + ddstandard.SelectedValue + " and semester = " + dropterm.SelectedItem.Text + " AND M.subject_no = " + dropsubname.SelectedValue + " and Criteria_no!='0' and Istype='" + columarray1[j].ToString() + "' order by Criteria_no";
    //                    ds1 = d2.select_method_wo_parameter(querybound1, "Text");
    //                    if (ds1.Tables[0].Rows.Count > 0)
    //                    {
    //                        string var1 = ds1.Tables[0].Rows[0]["CRITERIA_NO"].ToString();
    //                        string[] array1 = var1.Split(',');
    //                        if (array1.Length > 0)
    //                        {
    //                            HeaderCell1 = new TableCell();
    //                            HeaderCell1.Text = columarray1[j].ToString();
    //                            HeaderCell1.ColumnSpan = array1.Length + 1;
    //                            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
    //                            HeaderGridRow1.Cells.Add(HeaderCell1);
    //                            reportgrid1.Controls[0].Controls.AddAt(0, HeaderGridRow1);
    //                        }
    //                    }
    //                }
    //                string pentst1 = "SELECT distinct Criteria FROM Result u,Registration r,Exam_type e,CriteriaForInternal c WHERE U.roll_no = R.Roll_No AND U.exam_code = E.exam_code AND E.criteria_no = C.Criteria_no AND subject_no = " + dropsubname.SelectedValue + " and Criteria like '%pen%' order by Criteria ";
    //                dpen = d2.select_method_wo_parameter(pentst1, "Test");
    //                if (dpen.Tables[0].Rows.Count > 0)
    //                {
    //                    string var1 = dpen.Tables[0].Rows[0]["Criteria"].ToString();
    //                    string[] penarry = var1.Split(',');
    //                    if(penarry.Length > 0)
    //                    {
    //                    HeaderCell1 = new TableCell();
    //                    HeaderCell1.Text = "Pen-Paper Test Marks";
    //                    HeaderCell1.ColumnSpan = dpen.Tables[0].Rows.Count;
    //                    HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
    //                    HeaderGridRow1.Cells.Add(HeaderCell1);
    //                    reportgrid1.Controls[0].Controls.AddAt(1, HeaderGridRow1);
    //                    }
    //                }
    //                HeaderCell1 = new TableCell();
    //                HeaderCell1.Text = "";
    //                HeaderCell1.ColumnSpan = 4;
    //                HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
    //                HeaderGridRow1.Cells.Add(HeaderCell1);
    //                reportgrid1.Controls[0].Controls.AddAt(0, HeaderGridRow1);
    //            }
    //        }
    //        -------------mark

    //    }
    //    catch (Exception ex)
    //    {
    //        lblerrormsg.Text = ex.ToString();
    //        lblerrormsg.Visible = true;
    //    }
    //}
    // *********** add FA-1 and SA-1 name ***********

    // *********** add FA-1 and SA-1 name ***********
    //public void counttestvalue(DataTable d)
    //{
    //    try
    //    {
    //        if (d.Rows.Count > 0)
    //        {
    //            DataView dvcheck = new DataView(d);

    //            if (dropsubname.SelectedValue != "")
    //            {
    //                // string value = Convert.ToString(dropsubname.SelectedItem.Text);
    //                for (int ik = 0; ik < ds1.Tables[0].Rows.Count; ik++)
    //                {
    //                    string value = ds1.Tables[0].Rows[ik]["Istype"].ToString();
    //                    string gettoolname = ds1.Tables[0].Rows[ik]["CRITERIA_NO"].ToString();
    //                    dvcheck.RowFilter = "Istype='" + value + "'";
    //                    if (dvcheck.Count > 0)
    //                    {
    //                        if (!columarray.Contains(value))
    //                        {
    //                            columarray.Add(value);
    //                            columnhash.Add(value, dvcheck.Count);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblerrormsg.Text = ex.ToString();
    //        lblerrormsg.Visible = true;
    //    }
    //}
    // *********** add FA-1 and SA-1 name ***********

    //public void markcounttestvalue(DataTable d1)
    //{
    //    try
    //    {
    //        if (d1.Rows.Count > 0)
    //        {
    //            DataView dvcheck1 = new DataView(d1);

    //            if (dropsubname.SelectedValue != "")
    //            {
    //                // string value = Convert.ToString(dropsubname.SelectedItem.Text);
    //                for (int ik = 0; ik < ds1.Tables[0].Rows.Count; ik++)
    //                {
    //                    string valuee = ds1.Tables[0].Rows[ik]["Istype"].ToString();
    //                    string gettoolname1 = ds1.Tables[0].Rows[ik]["CRITERIA_NO"].ToString();
    //                    dvcheck1.RowFilter = "Istype='" + valuee + "'";
    //                    if (dvcheck1.Count > 0)
    //                    {
    //                        if (!columarray1.Contains(valuee))
    //                        {
    //                            columarray1.Add(valuee);
    //                            columnhash1.Add(valuee, dvcheck1.Count);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblerrormsg.Text = ex.ToString();
    //        lblerrormsg.Visible = true;
    //    }
    //}
}