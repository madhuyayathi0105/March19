using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Drawing;
using Gios.Pdf;
using System.IO;
using System.Globalization;

public partial class ConsolidateMarksheet : System.Web.UI.Page
{
    SqlCommand cmd;
    connection connection = new connection();
    Boolean Cellclick;
    SqlDataAdapter adaload;
    DAccess2 d2 = new DAccess2();
    string totcredits = "";
    string calculate = "";
    double total1 = 0;
    double gpacal1 = 0;
    Hashtable hat = new Hashtable();
    DataSet ds1 = new DataSet();
    Boolean flag_true = false;
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection conexam = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection setcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection holidaycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_subcrd = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    DAccess2 da = new DAccess2();
    DataSet daload = new DataSet();
    string collegecode = "";
    string Master = "";
    string generate = "";
    int sn0 = 0;
    int year = 0;
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
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("CoeHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/CoeMod/CoeHome.aspx");
                    return;
                }
            }
            //****************************************************//
            errmsg.Visible = false;
            if (!Page.IsPostBack)
            {
                collegecode = Session["collegecode"].ToString();

                txtdop.Attributes.Add("ReadOnly", "ReadOnly");
                txtdoi.Attributes.Add("ReadOnly", "ReadOnly");
                string dt1 = DateTime.Today.ToShortDateString();
                string[] dsplit = dt1.Split(new Char[] { '/' });
                string dateconcat = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
                txtdop.Text = dateconcat.ToString();
                txtdoi.Text = dateconcat.ToString();
                Master = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                setcon.Close();
                setcon.Open();
                SqlDataReader mtrdr;
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                SqlCommand mtcmd = new SqlCommand(Master, setcon);
                mtrdr = mtcmd.ExecuteReader();
                {
                    if (mtrdr.HasRows)
                    {
                        while (mtrdr.Read())
                        {
                            if (mtrdr["settings"].ToString() == "Roll No" && mtrdr["value"].ToString() == "1")
                            {
                                Session["Rollflag"] = "1";
                            }
                            if (mtrdr["settings"].ToString() == "Register No" && mtrdr["value"].ToString() == "1")
                            {
                                Session["Regflag"] = "1";
                            }
                        }

                    }
                }
                ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                ddlMonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                ddlMonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                ddlMonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                ddlMonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                ddlMonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
                ddlMonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                ddlMonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                ddlMonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                ddlMonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                ddlMonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                ddlMonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                ddlMonth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));



                int year1;
                year1 = Convert.ToInt16(DateTime.Today.Year);
                ddlYear.Items.Clear();
                for (int l = 0; l <= 10; l++)
                {

                    ddlYear.Items.Add(Convert.ToString(year1 - l));

                }
                ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));

                //HAllSpread.Sheets[0].PageSize = 10;
                HAllSpread.Sheets[0].RowHeader.Visible = false;
                HAllSpread.Sheets[0].ColumnCount = 6;
                HAllSpread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                HAllSpread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                HAllSpread.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                HAllSpread.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                HAllSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                HAllSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Year";
                HAllSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
                HAllSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Course";
                HAllSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Sem";
                HAllSpread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Total Students";
                HAllSpread.Sheets[0].Columns[0].Width = 40;
                HAllSpread.Sheets[0].Columns[1].Width = 40;
                HAllSpread.Sheets[0].Columns[5].Font.Underline = true;
                HAllSpread.Sheets[0].Columns[5].ForeColor = Color.Blue;
                HAllSpread.Sheets[0].AutoPostBack = true;
                HAllSpread.CommandBar.Visible = false;
                HAllSpread.Sheets[0].Columns[0].Locked = true;
                HAllSpread.Sheets[0].Columns[1].Locked = true;
                HAllSpread.Sheets[0].Columns[2].Locked = true;
                HAllSpread.Sheets[0].Columns[3].Locked = true;
                HAllSpread.Sheets[0].Columns[4].Locked = true;
                HAllSpread.Sheets[0].Columns[5].Locked = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

        lblno.Visible = false;
        HAllSpread.Visible = false;
        int overalltot = 0;
        string exammonth = ddlMonth.SelectedValue.ToString();
        string examyear = ddlYear.SelectedValue.ToString();

        if (exammonth != "0" && examyear != "0")
        {
            if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
            {

                year++;
                //string spraedbind = "select  distinct e.batchto,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.semester as sem,d.degree_code as degree,dpt.dept_code as dptcode from exmtt e,exmtt_det ex,Department dpt,degree d,course c,registration r where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.semester between 1 and 2 and  c.college_code=" + Session["collegecode"].ToString() + "  and ex.exam_code=e.exam_code and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''  and cc=0 and delflag=0 and exam_flag<>'Debar' order by e.semester ";
                string spraedbind = "select  distinct e.batch_year,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.current_semester as sem,d.degree_code as degree,dpt.dept_code as dptcode,e.exam_code from Exam_Details e,Department dpt,degree d,course c,registration r where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.current_semester between 1 and 2 and  c.college_code=" + Session["collegecode"].ToString() + "  and  e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''  and cc=0 and delflag=0 and exam_flag<>'Debar' order by e.current_semester ";//Rajumar 28/5/2018

                SqlCommand Todptcmd = new SqlCommand(spraedbind, con);

                SqlDataReader Toadeptreader;
                string course = "";
                string department = "";
                string sem = "";
                string degree = "";
                string batchyear = "";
                string department_code = "";
                string department_name = "";
                con.Close();
                con.Open();
                HAllSpread.Sheets[0].RowCount = 0;
                Toadeptreader = Todptcmd.ExecuteReader();
                if (Toadeptreader.HasRows)
                {
                    sn0++;
                    while (Toadeptreader.Read())
                    {
                        HAllSpread.Visible = true;
                        HAllSpread.Sheets[0].RowCount = HAllSpread.Sheets[0].RowCount + 1;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 0].Text = sn0 + "";
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Text = year + "";
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        degree = Toadeptreader["degree"].ToString();
                        course = Toadeptreader["course"].ToString();
                        department = Toadeptreader["dept"].ToString();
                        sem = Toadeptreader["sem"].ToString();
                        department_code = Toadeptreader["dptcode"].ToString();
                        department_name = Toadeptreader["deptname"].ToString();
                        batchyear = Toadeptreader["batch_year"].ToString();
                        string exam_code = Toadeptreader["exam_code"].ToString();
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Note = batchyear;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Tag = degree;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Text = department;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Note = degree;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Tag = department_name;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 2].Text = course;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].Text = sem + "";
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].Note = exam_code;
                        //string totstud = "select  isnull(count(*),0) as total from registration where degree_code='" + degree + "' and current_semester='" + sem + "' and college_code=" + Session["collegecode"].ToString() + " and cc=0  and delflag=0 and exam_flag<>'Debar'";
                        string totstud = "select distinct count(distinct r.roll_no) as total from Registration r,Exam_Details e,exam_application a where r.Roll_No=a.roll_no and e.exam_code=a.exam_code and r.degree_code=e.degree_code and e.batch_year=r.Batch_Year and e.exam_code='" + exam_code + "'";////Rajumar 28/5/2018
                        int semval = Convert.ToInt16(sem) + Convert.ToInt16(1);

                        SqlCommand Totcmd = new SqlCommand(totstud, con1);
                        con1.Close();
                        con1.Open();
                        int totalstudents = 0;
                        SqlDataReader Totreader;
                        Totreader = Totcmd.ExecuteReader();
                        if (Totreader.HasRows)
                        {
                            while (Totreader.Read())
                            {
                                totalstudents = totalstudents + Convert.ToInt32(Totreader["total"]);
                                overalltot = overalltot + totalstudents;
                                HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(Totreader["total"]); //totalstudents + "";
                                HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }

                    }
                }
                HAllSpread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                HAllSpread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                HAllSpread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            }
            //=======second year
            if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
            {

                year++;
                //string spraedbind = "select  distinct e.batchto,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.semester as sem,d.degree_code as degree,dpt.dept_code as dptcode from exmtt e,exmtt_det ex,Department dpt,degree d,course c,registration r where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.semester between 3 and 4 and  c.college_code=" + Session["collegecode"].ToString() + " and ex.exam_code=e.exam_code and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''  and cc=0 and delflag=0 and exam_flag<>'Debar' order by e.semester ";
                string spraedbind = "select  distinct e.batch_year,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.current_semester as sem,d.degree_code as degree,dpt.dept_code as dptcode,e.exam_code from Exam_Details e,Department dpt,degree d,course c,registration r where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.current_semester between 3 and 4 and  c.college_code=" + Session["collegecode"].ToString() + "  and  e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''  and cc=0 and delflag=0 and exam_flag<>'Debar' order by e.current_semester ";//Rajumar 28/5/2018


                SqlCommand Todptcmd = new SqlCommand(spraedbind, con);

                SqlDataReader Toadeptreader;
                string course = "";
                string department = "";
                string sem = "";
                string degree = "";
                string batchyear = "";
                string department_code = "";
                string department_name = "";
                con.Close();
                con.Open();

                Toadeptreader = Todptcmd.ExecuteReader();
                if (Toadeptreader.HasRows)
                {
                    sn0++;
                    while (Toadeptreader.Read())
                    {
                        HAllSpread.Visible = true;
                        HAllSpread.Sheets[0].RowCount = HAllSpread.Sheets[0].RowCount + 1;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 0].Text = sn0 + "";

                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Text = year + "";
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        degree = Toadeptreader["degree"].ToString();
                        course = Toadeptreader["course"].ToString();
                        department = Toadeptreader["dept"].ToString();
                        sem = Toadeptreader["sem"].ToString();
                        department_code = Toadeptreader["dptcode"].ToString();
                        department_name = Toadeptreader["deptname"].ToString();
                        batchyear = Toadeptreader["batch_year"].ToString();
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Note = batchyear;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Tag = degree;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Tag = department_name;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].Text = sem + "";
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Note = degree;

                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Text = department;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 2].Text = course;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                        //string totstud = "select isnull(count(*),0) as total from registration where degree_code='" + degree + "' and current_semester='" + sem + "' and college_code=" + Session["collegecode"].ToString() + " and cc=0  and delflag=0 and exam_flag<>'Debar'";
                        string exam_code = Toadeptreader["exam_code"].ToString();
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].Note = exam_code;
                        string totstud = "select distinct count(distinct r.roll_no) as total from Registration r,Exam_Details e,exam_application a where r.Roll_No=a.roll_no and e.exam_code=a.exam_code and r.degree_code=e.degree_code and e.batch_year=r.Batch_Year and e.exam_code='" + exam_code + "'";//Rajumar 28/5/2018

                        int semval = Convert.ToInt16(sem) + Convert.ToInt16(1);

                        SqlCommand Totcmd = new SqlCommand(totstud, con1);
                        con1.Close();
                        con1.Open();
                        int totalstudents = 0;
                        SqlDataReader Totreader;
                        Totreader = Totcmd.ExecuteReader();
                        if (Totreader.HasRows)
                        {
                            while (Totreader.Read())
                            {
                                totalstudents = totalstudents + Convert.ToInt32(Totreader["total"]);
                                overalltot = overalltot + totalstudents;
                                HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(Totreader["total"]);// totalstudents + "";
                                HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                }
                HAllSpread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                HAllSpread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                HAllSpread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            }
            //====3rd year
            if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
            {

                year++;
                //string spraedbind = "select  distinct e.batchto,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.semester as sem,d.degree_code as degree,dpt.dept_code as dptcode from exmtt e,exmtt_det ex,Department dpt,degree d,course c,registration r where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.semester between 5 and 6 and  c.college_code=" + Session["collegecode"].ToString() + "  and ex.exam_code=e.exam_code and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''  and cc=0 and delflag=0 and exam_flag<>'Debar' order by e.semester ";
                string spraedbind = "select  distinct e.batch_year,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.current_semester as sem,d.degree_code as degree,dpt.dept_code as dptcode,e.exam_code from Exam_Details e,Department dpt,degree d,course c,registration r where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.current_semester between 5 and 6 and  c.college_code=" + Session["collegecode"].ToString() + "  and  e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''  and cc=0 and delflag=0 and exam_flag<>'Debar' order by e.current_semester ";//Rajumar 28/5/2018

                SqlCommand Todptcmd = new SqlCommand(spraedbind, con);

                SqlDataReader Toadeptreader;
                string course = "";
                string department = "";
                string sem = "";
                string degree = "";
                string batchyear = "";
                string department_code = "";
                string department_name = "";
                con.Close();
                con.Open();

                Toadeptreader = Todptcmd.ExecuteReader();
                if (Toadeptreader.HasRows)
                {
                    sn0++;
                    while (Toadeptreader.Read())
                    {
                        HAllSpread.Visible = true;
                        HAllSpread.Sheets[0].RowCount = HAllSpread.Sheets[0].RowCount + 1;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 0].Text = sn0 + "";
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Text = year + "";
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        degree = Toadeptreader["degree"].ToString();
                        course = Toadeptreader["course"].ToString();
                        department = Toadeptreader["dept"].ToString();
                        sem = Toadeptreader["sem"].ToString();
                        department_code = Toadeptreader["dptcode"].ToString();
                        department_name = Toadeptreader["deptname"].ToString();
                        batchyear = Toadeptreader["batch_year"].ToString();
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Note = batchyear;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Tag = degree;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Tag = department_name;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].Text = sem + "";
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Note = degree;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Text = department;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 2].Text = course;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        //string totstud = "select count(*)as total from registration where degree_code='" + degree + "' and current_semester='" + sem + "' and college_code=" + Session["collegecode"].ToString() + "  and cc=0 and delflag=0 and exam_flag<>'Debar'";
                        string exam_code = Toadeptreader["exam_code"].ToString();
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].Note = exam_code;
                        string totstud = "select distinct count(distinct r.roll_no) as total from Registration r,Exam_Details e,exam_application a where r.Roll_No=a.roll_no and e.exam_code=a.exam_code and r.degree_code=e.degree_code and e.batch_year=r.Batch_Year and e.exam_code='" + exam_code + "'";//Rajumar 28/5/2018
                        int semval = Convert.ToInt16(sem) + Convert.ToInt16(1);

                        SqlCommand Totcmd = new SqlCommand(totstud, con1);
                        con1.Close();
                        con1.Open();
                        int totalstudents = 0;
                        SqlDataReader Totreader;
                        Totreader = Totcmd.ExecuteReader();
                        if (Totreader.HasRows)
                        {
                            while (Totreader.Read())
                            {
                                totalstudents = totalstudents + Convert.ToInt32(Totreader["total"]);
                                overalltot = overalltot + totalstudents;
                                HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].Text = totalstudents + "";
                                HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                }
                HAllSpread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                HAllSpread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                HAllSpread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            }
            //====4th year
            if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
            {

                year++;
                //string spraedbind = "select  distinct e.batchto,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.semester as sem,d.degree_code as degree,dpt.dept_code as dptcode from exmtt e,exmtt_det ex,Department dpt,degree d,course c,registration r where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.semester between 7 and 8 and  c.college_code=" + Session["collegecode"].ToString() + "  and ex.exam_code=e.exam_code and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''  and cc=0 and delflag=0 and exam_flag<>'Debar' order by e.semester ";
                string spraedbind = "select  distinct e.batch_year,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.current_semester as sem,d.degree_code as degree,dpt.dept_code as dptcode,e.exam_code from Exam_Details e,Department dpt,degree d,course c,registration r where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.current_semester between 7 and 8 and  c.college_code=" + Session["collegecode"].ToString() + "  and  e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''  and cc=0 and delflag=0 and exam_flag<>'Debar' order by e.current_semester ";//Rajumar 28/5/2018

                SqlCommand Todptcmd = new SqlCommand(spraedbind, con);

                SqlDataReader Toadeptreader;
                string course = "";
                string department = "";
                string sem = "";
                string degree = "";
                string batchyear = "";
                string department_code = "";
                string department_name = "";
                con.Close();
                con.Open();

                Toadeptreader = Todptcmd.ExecuteReader();
                if (Toadeptreader.HasRows)
                {
                    sn0++;
                    while (Toadeptreader.Read())
                    {
                        HAllSpread.Visible = true;
                        HAllSpread.Sheets[0].RowCount = HAllSpread.Sheets[0].RowCount + 1;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Note = batchyear;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 0].Text = sn0 + "";
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Text = year + "";
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        degree = Toadeptreader["degree"].ToString();
                        course = Toadeptreader["course"].ToString();
                        department = Toadeptreader["dept"].ToString();
                        sem = Toadeptreader["sem"].ToString();
                        department_code = Toadeptreader["dptcode"].ToString();
                        department_name = Toadeptreader["deptname"].ToString();
                        batchyear = Toadeptreader["batch_year"].ToString();
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Tag = degree;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Tag = department_name;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].Text = sem + "";
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Note = degree;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Text = department;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 2].Text = course;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        //string totstud = "select count(*)as total from registration where degree_code='" + degree + "' and current_semester='" + sem + "' and college_code=" + Session["collegecode"].ToString() + "  and cc=0 and delflag=0 and exam_flag<>'Debar'";
                        string exam_code = Convert.ToString(Toadeptreader["exam_code"]);
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].Note = exam_code;
                        string totstud = "select distinct count(distinct r.roll_no) as total from Registration r,Exam_Details e,exam_application a where r.Roll_No=a.roll_no and e.exam_code=a.exam_code and r.degree_code=e.degree_code and e.batch_year=r.Batch_Year and e.exam_code='" + exam_code + "'";//Rajumar 28/5/2018
                        int semval = Convert.ToInt16(sem) + Convert.ToInt16(1);

                        SqlCommand Totcmd = new SqlCommand(totstud, con1);
                        con1.Close();
                        con1.Open();
                        int totalstudents = 0;
                        SqlDataReader Totreader;
                        Totreader = Totcmd.ExecuteReader();
                        if (Totreader.HasRows)
                        {
                            while (Totreader.Read())
                            {
                                totalstudents = totalstudents + Convert.ToInt32(Totreader["total"]);
                                overalltot = overalltot + totalstudents;
                                HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].Text = totalstudents + "";
                                HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                }
                HAllSpread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                HAllSpread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                HAllSpread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            }
            HAllSpread.Sheets[0].RowCount = HAllSpread.Sheets[0].RowCount + 1;
            int o = overalltot;
            HAllSpread.Sheets[0].SpanModel.Add(HAllSpread.Sheets[0].RowCount - 1, 0, 1, 4);
            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].Text = "Total";
            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].Font.Bold = true;

            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].Text = overalltot + "";
            //  HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].Locked = true;// added by sridhar 11 Sep 2014

            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].Font.Bold = true;
            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
            string totalrows = HAllSpread.Sheets[0].RowCount.ToString();
            HAllSpread.Sheets[0].PageSize = Convert.ToInt32(totalrows);

            if (HAllSpread.Visible == false)
            {
                lblno.Visible = true;
                lblno.Text = "No Reords Found";
            }
            else
            {
                lblno.Visible = false;
            }
        }
        else
        {

        }

    }
    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread2.Visible = false;
        printbtn.Visible = false;
        Label2.Visible = false;
        HAllSpread.Visible = false;
        hiddenfiels();
        //selectMonth.Visible = false;
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread2.Visible = false;
        printbtn.Visible = false;
        Label2.Visible = false;
        HAllSpread.Visible = false;
        hiddenfiels();
        //selectyear.Visible = false;

    }
    protected void ddlformate_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread2.Visible = false;
        printbtn.Visible = false;
        Label2.Visible = false;
        HAllSpread.Visible = false;
        hiddenfiels();
        //selectyear.Visible = false;

    }


    protected void HAllSpread_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

        Cellclick = true;

        //Backbtn.Visible = true;
    }
    protected void HAllSpread_SelectedIndexChanged(Object sender, EventArgs e)
    {

        if (Cellclick == true)
        {
            printbtn.Visible = false;
            FpSpread2.Visible = false;
            //Button2.Visible = false;

            //HAllSpread.Visible = false;
            FpSpread2.CommandBar.Visible = false;
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].ColumnCount = 7;
            FpSpread2.Sheets[0].Columns[0].Width = 40;
            FpSpread2.Sheets[0].Columns[1].Width = 80;
            FpSpread2.Sheets[0].Columns[2].Width = 130;
            FpSpread2.Sheets[0].Columns[3].Width = 180;
            FpSpread2.Sheets[0].Columns[4].Width = 90;
            FpSpread2.Sheets[0].Columns[5].Width = 90;
            FpSpread2.Sheets[0].Columns[6].Width = 50;
            //FpSpread2.Sheets[0].AutoPostBack = true;
            FpSpread2.Sheets[0].RowHeader.Visible = false;
            FpSpread2.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread2.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].SheetCorner.RowCount = 2;

            FpSpread2.Sheets[0].Columns[0].Locked = true;
            FpSpread2.Sheets[0].Columns[1].Locked = true;
            FpSpread2.Sheets[0].Columns[2].Locked = true;
            FpSpread2.Sheets[0].Columns[3].Locked = true;
            FpSpread2.Sheets[0].Columns[4].Locked = true;
            FpSpread2.Sheets[0].Columns[5].Locked = true;

            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 1, 2);
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Subjects";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            FpSpread2.Sheets[0].ColumnHeader.Cells[1, 4].Text = "Regular";
            FpSpread2.Sheets[0].ColumnHeader.Cells[1, 5].Text = "Arrear";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Select";
            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            FpSpread2.Sheets[0].Columns[6].CellType = chkcell;
            FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 1;
            FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 0, 1, 6);
            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].CellType = chkcell1;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].FrozenRowCount = 1;
            chkcell1.AutoPostBack = true;
            string activerow = "";
            string activecol = "";
            string depart_code = "";
            int sno = 0;
            activerow = HAllSpread.ActiveSheetView.ActiveRow.ToString();
            activecol = HAllSpread.ActiveSheetView.ActiveColumn.ToString();
            // added by sridhar 11 sep 2014 ====start
            int totalrowssri = Convert.ToInt32(HAllSpread.Sheets[0].RowCount.ToString()) - 1;
            if (Convert.ToString(totalrowssri) == activerow)
            {
                return;


            }
            // added by sridhar 11 sep 2014 ====end
            string year = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
            string degree = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
            string course = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
            string studenttot = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text;
            depart_code = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Note;
            string sem = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
            string examCode=HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Note;
            string batchyearatt = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Note;
            string label1 = year + " " + "year" + " " + degree + " " + course + " " + "-" + studenttot;
            Label2.Text = label1;
            Session["semforsub"] = sem;
            Session["selecteddegreecode"] = depart_code;
            Session["selecteddegree"] = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
            Session["selectedcourse"] = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag;
            //=========
            lblno.Visible = false;
            int sprdvisibleflag = 0;
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

            //string studinfo = "select len(r.reg_no),r.reg_no,r.stud_name,r.roll_no,r.batch_year from registration r where r.current_semester=" + sem + "and r.degree_code=" + depart_code + "  and cc=0 and delflag=0 and exam_flag<>'Debar' order by len(r.reg_no),r.reg_no,r.stud_name";
            string studinfo = "select len(r.reg_no),r.reg_no,r.stud_name,r.roll_no,r.batch_year from registration r,exam_application a,Exam_Details e where r.Roll_No=a.roll_no and e.exam_code=a.exam_code and r.degree_code=e.degree_code and e.batch_year=r.Batch_Year and e.exam_code='" + examCode + "'  and cc=0 and delflag=0 and exam_flag<>'Debar' order by len(r.reg_no),r.reg_no,r.stud_name";
            int semval = Convert.ToInt16(sem) + Convert.ToInt16(1);

            SqlDataAdapter dastudinfo = new SqlDataAdapter(studinfo, con1);
            DataSet dsstudinfo = new DataSet();
            con1.Close();
            con1.Open();
            dastudinfo.Fill(dsstudinfo);
            if (dsstudinfo.Tables[0].Rows.Count > 0)
            {
                for (int studcount = 0; studcount < dsstudinfo.Tables[0].Rows.Count; studcount++)
                {
                    string regno = "";
                    string studname = "";
                    string rollno = "";
                    string batchyear = "";
                    Label2.Visible = true;
                    printbtn.Visible = true;
                    FpSpread2.Visible = true;
                    sno++;
                    batchyear = dsstudinfo.Tables[0].Rows[studcount]["batch_year"].ToString();
                    regno = dsstudinfo.Tables[0].Rows[studcount]["reg_no"].ToString();
                    studname = dsstudinfo.Tables[0].Rows[studcount]["stud_name"].ToString();
                    rollno = dsstudinfo.Tables[0].Rows[studcount]["roll_no"].ToString();
                    FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 1;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = sno + "";
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Note = batchyear;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = txt;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = rollno;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Note = rollno;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].CellType = txt;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = regno;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = studname;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    if (rollno != "")
                    {
                        string regpaper = "select count(*) as regularpap from subjectchooser sc,subject s,sub_sem as feesub where feesub.subtype_no=s.subtype_no and  feesub.syll_code=s.syll_code and feesub.promote_count=1 and s.subject_no=sc.subject_no and s.subtype_no=sc.subtype_no and roll_no='" + rollno + "' and semester=" + sem + "";
                        SqlCommand regularcmd = new SqlCommand(regpaper, con);
                        con.Close();
                        con.Open();
                        SqlDataReader regularreader = regularcmd.ExecuteReader();
                        string regularpaper = "";
                        if (regularreader.HasRows)
                        {
                            while (regularreader.Read())
                            {
                                sprdvisibleflag = 1;
                                regularpaper = regularreader["regularpap"].ToString();
                            }
                        }
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = regularpaper;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        string arrpap = "Select count(*) as arrearpap from subject,syllabus_master as smas,sub_sem as feesub where feesub.subtype_no=subject.subtype_no and  feesub.syll_code=subject.syll_code and feesub.promote_count=1 and smas.syll_code = subject.syll_code and subject_no in (select distinct subject_no from  mark_entry where subject_no in (select distinct subject_no from mark_entry where passorfail=0 and (result='Fail' or result='AAA') and roll_no='" + rollno + "') and roll_no='" + rollno + "' and Semester >= 1 and Semester < " + sem + " )";
                        SqlCommand arrearcmd = new SqlCommand(arrpap, con);
                        con.Close();
                        con.Open();
                        SqlDataReader arrearreader = arrearcmd.ExecuteReader();
                        string arrearpaper = "";
                        if (arrearreader.HasRows)
                        {
                            while (arrearreader.Read())
                            {
                                arrearpaper = arrearreader["arrearpap"].ToString();
                            }
                        }

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = arrearpaper;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;

                    }
                }
            }

            //===============
            string totalrows = FpSpread2.Sheets[0].RowCount.ToString();
            FpSpread2.Sheets[0].PageSize = (Convert.ToInt32(totalrows) * 20) + 40;
            FpSpread2.Height = (Convert.ToInt32(totalrows) * 20) + 40;

            FpSpread2.Sheets[0].Columns[5].Visible = false;
            if (Session["Rollflag"] == "0")
            {
                FpSpread2.Width = 510;
                FpSpread2.Sheets[0].Columns[1].Visible = false;
            }
            if (Session["Regflag"] == "0")
            {
                FpSpread2.Width = 410;
                FpSpread2.Sheets[0].Columns[2].Visible = false;
            }


            FpSpread2.Sheets[0].Columns[5].Visible = true;


            if (Session["Rollflag"] == "0")
            {
                FpSpread2.Width = 600;
                FpSpread2.Sheets[0].Columns[1].Visible = false;
            }
            if (Session["Regflag"] == "0")
            {
                FpSpread2.Width = 500;
                FpSpread2.Sheets[0].Columns[2].Visible = false;
            }

            if (sprdvisibleflag == 0)
            {
                FpSpread2.Visible = false;
                printbtn.Visible = false;
                //Button2.Visible = false;
                lblno.Visible = true;
                lblno.Text = "No Records Found";
                Label2.Visible = false;
            }
            FpSpread2.SaveChanges();
            for (int c = 0; c < FpSpread2.Sheets[0].RowCount; c++)
            {
                FpSpread2.Sheets[0].Cells[c, 6].Value = "0";
            }

        }


    }
    protected void FpSpread2_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        IblError.Visible = false;
        IblError.Text = " ";
        string actrow = e.SheetView.ActiveRow.ToString();
        if (flag_true == false && actrow == "0")
        {
            for (int j = 1; j < Convert.ToInt16(FpSpread2.Sheets[0].RowCount); j++)
            {
                string actcol = e.SheetView.ActiveColumn.ToString();
                string seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
                if (seltext != "System.Object")
                    FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = seltext.ToString();
            }
            flag_true = true;
        }

    }
    protected void printbtn_Click(object sender, EventArgs e) //////////Added by Jeyagandhi Dated (10-6-2015)
    {
        try
        {
            if (ddlformate.SelectedIndex == 0)
            {
                printbtn_Click();
            }
            else if (ddlformate.SelectedIndex == 1)
            {
                IblError.Visible = false;
                DataSet printds = new DataSet();
                DataSet printds_new = new DataSet();
                DataSet printds_rows = new DataSet();
                DataSet gradeds = new DataSet();
                string degree = "";
                string monthandyear = "";
                string studname = "";
                string rollnosub = "";
                string regnumber = "";
                string batch_year = "";
                string degree_code = "";
                string exam_code = "";
                string sem = "";
                string grade = "";
                string latmode = "";
                int colval = 0;
                string branch = "";
                string markgrade = "";
                string creditponts = "";
                string classcalcu = "";
                int month = 0;
                string monthstr = "";
                string sql2 = "";
                string sql3 = "";
                string roman = "";
                string semroman = "";
                int rowcount = 0;
                string seme = "";
                string stdphtsql = "";
                int i = 0;
                int colvel = 0;
                collegecode = Session["collegecode"].ToString();
                Font Fontpala12 = new Font("Palatino Linotype", 8, FontStyle.Bold);
                Font Fontarial7 = new Font("Arial", 7, FontStyle.Regular);
                Font Fontarial9 = new Font("Arial", 8, FontStyle.Regular);
                Font fontname = new Font("Arial", 9, FontStyle.Regular);
                Font fontarial12 = new Font("Arial", 12, FontStyle.Regular);
                Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InInches(8.5, 14));
                Gios.Pdf.PdfPage mypdfpage;
                string sql = "SELECT  r.roll_no,Reg_No,r.batch_year,r.app_no,r.degree_code,R.current_semester,R.Stud_Name,Course_Name as degree,Dept_name,CONVERT(VARCHAR, dob, 103) as dob FROM Registration R,Applyn A,Degree G,Course C,Department D WHERE R.App_No = A.App_No AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND D.College_Code = G.College_Code";
                if ((ddlMonth.SelectedValue.ToString() != "0") && (ddlYear.SelectedValue.ToString() != "0"))
                {
                    FpSpread2.SaveChanges();
                    for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
                    {
                        int isval = 0;
                        string s = FpSpread2.Sheets[0].Cells[res, 6].Text;
                        isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);
                        if (isval == 1)
                        {
                            IblError.Visible = false;
                            IblError.Text = "  ";
                            printds.Clear();
                            printds.Dispose();
                            printds_new.Clear();
                            printds_new.Dispose();
                            rollnosub = FpSpread2.Sheets[0].Cells[res, 1].Note;
                            regnumber = FpSpread2.Sheets[0].Cells[res, 2].Text;
                            string rsql = sql + "  AND Roll_No = '" + rollnosub + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                            printds = da.select_method_wo_parameter(rsql, "Text");
                            if (printds.Tables[0].Rows.Count > 0)
                            {
                                mypdfpage = mydoc.NewPage();
                                degree = printds.Tables[0].Rows[0]["degree"].ToString();
                                month = ddlMonth.SelectedIndex;
                                monthstr = ddlMonth.SelectedIndex.ToString();
                                string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
                                monthandyear = strMonthName + "  " + ddlYear.SelectedItem.Text;
                                monthandyear = monthandyear.ToUpper();
                                studname = printds.Tables[0].Rows[0]["Stud_Name"].ToString();
                                branch = printds.Tables[0].Rows[0]["Dept_name"].ToString();
                                string stdappno = printds.Tables[0].Rows[0]["App_No"].ToString();
                                batch_year = printds.Tables[0].Rows[0]["batch_year"].ToString();
                                degree_code = printds.Tables[0].Rows[0]["degree_code"].ToString();
                                sem = printds.Tables[0].Rows[0]["current_semester"].ToString();
                                stdphtsql = "select reg_no,roll_no,current_semester,(select photo from stdphoto s where r.app_no=s.app_no) as photo from registration r where  r.degree_code='" + degree_code + "' and r.Batch_Year='" + batch_year + "' and Roll_No='" + rollnosub + "'";

                                DataSet dsstdpho = new DataSet();
                                dsstdpho.Clear();
                                dsstdpho.Dispose();
                                dsstdpho = da.select_method_wo_parameter(stdphtsql, "Text");
                                MemoryStream memoryStream = new MemoryStream();
                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg")))
                                {
                                    if (dsstdpho.Tables[0].Rows[0]["photo"].ToString() != null && dsstdpho.Tables[0].Rows[0]["photo"].ToString().Trim() != "")
                                    {
                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg")))
                                        {
                                            byte[] file = (byte[])dsstdpho.Tables[0].Rows[0]["photo"];
                                            memoryStream.Write(file, 0, file.Length);
                                            if (file.Length > 0)
                                            {
                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            }
                                            memoryStream.Dispose();
                                            memoryStream.Close();
                                        }
                                    }
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg")))
                                {
                                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg"));
                                    mypdfpage.Add(LogoImage, 515, 128, 600);
                                }
                                else
                                {
                                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg"));
                                    mypdfpage.Add(LogoImage, 515, 128, 600);
                                }

                                int coltop = 200;
                                sql2 = "select * from exam_details where  batch_year='" + batch_year + "' and   degree_code='" + degree_code + "'  and exam_month='" + month + "' and exam_year='" + ddlYear.SelectedItem.Text + "'";
                                printds_new = da.select_method_wo_parameter(sql2, "Text");
                                if (printds_new.Tables[0].Rows.Count > 0)
                                {
                                    exam_code = printds_new.Tables[0].Rows[0]["exam_code"].ToString();
                                    sql3 = "SELECT subject_name,subject_code,roll_no,subject.subject_no,result,total,ISNULL(grade,'') grade,syllabus_master.degree_code,cp,credit_points,mark_entry.subject_no,syllabus_master.batch_year,semester,exam_month,exam_year,Mark_Entry.Exam_Code,Attempts,External_Mark from Mark_Entry,Subject,sub_sem,syllabus_master,exam_details where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and mark_entry.exam_code = exam_details.exam_code and semester <='8' and upper(result) ='PASS' and roll_no='" + rollnosub + "' and syllabus_master.degree_code ='" + degree_code + "' and syllabus_master.batch_year ='" + batch_year + "' order by semester asc,subject_type desc,subject.subject_no asc";
                                    printds_rows.Clear();
                                    printds_rows.Dispose();
                                    printds_rows = da.select_method_wo_parameter(sql3, "Text");
                                    rowcount = printds_rows.Tables[0].Rows.Count;

                                    PdfTextArea pdfhead = new PdfTextArea(fontarial12, System.Drawing.Color.Black, new PdfArea(mydoc, 150, 110, 553, 50), System.Drawing.ContentAlignment.TopLeft, studname.ToString());
                                    mypdfpage.Add(pdfhead);

                                    string degr = da.GetFunction("select Duration from Degree where Degree_Code='" + degree_code + "'");
                                    int passing = Convert.ToInt32(degr) / 2;
                                    passing = Convert.ToInt32(batch_year) + Convert.ToInt32(passing);

                                    Gios.Pdf.PdfTable table1forpagehead = mydoc.NewTable(fontarial12, 2, 4, 2);
                                    table1forpagehead.VisibleHeaders = false;
                                    table1forpagehead.SetBorders(Color.Black, 1, BorderType.None);
                                    table1forpagehead.SetColumnsWidth(new int[] { 75, 75, 85, 200 });
                                    table1forpagehead.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1forpagehead.Cell(0, 0).SetContent("");
                                    table1forpagehead.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1forpagehead.Cell(0, 1).SetContent(regnumber);
                                    table1forpagehead.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1forpagehead.Cell(0, 2).SetContent("");
                                    table1forpagehead.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1forpagehead.Cell(0, 3).SetContent(passing.ToString());
                                    table1forpagehead.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1forpagehead.Cell(1, 0).SetContent("");
                                    table1forpagehead.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1forpagehead.Cell(1, 1).SetContent(degree);
                                    table1forpagehead.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1forpagehead.Cell(1, 2).SetContent("");
                                    table1forpagehead.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1forpagehead.Cell(1, 3).SetContent(branch);
                                    Gios.Pdf.PdfTablePage newpdftabpagehead = table1forpagehead.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 34, 130, 480, 50));
                                    mypdfpage.Add(newpdftabpagehead);

                                    if (printds_rows.Tables[0].Rows.Count > 0)
                                    {
                                        for (i = 0; i < printds_rows.Tables[0].Rows.Count; i++)
                                        {
                                            seme = printds_rows.Tables[0].Rows[i]["Semester"].ToString();

                                            if (seme == "1")
                                            {
                                                semroman = "I";
                                            }
                                            else if (seme == "2")
                                            {
                                                semroman = "II";
                                            }
                                            else if (seme == "3")
                                            {
                                                semroman = "III";
                                            }
                                            else if (seme == "4")
                                            {
                                                semroman = "IV";
                                            }
                                            else if (seme == "5")
                                            {
                                                semroman = "V";
                                            }
                                            else if (seme == "6")
                                            {
                                                semroman = "VI";
                                            }
                                            else if (seme == "7")
                                            {
                                                semroman = "VII";
                                            }
                                            else if (seme == "8")
                                            {
                                                semroman = "VIII";
                                            }
                                            string examcode = printds_rows.Tables[0].Rows[i]["Exam_Code"].ToString();
                                            string exammonyea = da.GetFunction("select ltrim(str(Exam_Month))+'  '+ltrim(str(Exam_year)) ExamYear from Exam_Details where exam_code = '" + examcode + "'");
                                            string[] exam = exammonyea.Split(' ');
                                            string exammmonth = exam[0];
                                            if (exammmonth == "1")
                                            {
                                                exammmonth = "Jan";
                                            }
                                            if (exammmonth == "2")
                                            {
                                                exammmonth = "Feb ";
                                            }
                                            if (exammmonth == "3")
                                            {
                                                exammmonth = "Mar ";
                                            }
                                            if (exammmonth == "4")
                                            {
                                                exammmonth = "Apr ";
                                            }
                                            if (exammmonth == "5")
                                            {
                                                exammmonth = " May";
                                            }
                                            if (exammmonth == "6")
                                            {
                                                exammmonth = "Jun ";
                                            }
                                            if (exammmonth == "7")
                                            {
                                                exammmonth = "Jul ";
                                            }
                                            if (exammmonth == "8")
                                            {
                                                exammmonth = "Aug ";
                                            }
                                            if (exammmonth == "9")
                                            {
                                                exammmonth = "Sep ";
                                            }
                                            if (exammmonth == "10")
                                            {
                                                exammmonth = "Oct ";
                                            }
                                            if (exammmonth == "11")
                                            {
                                                exammmonth = "Nov ";
                                            }
                                            if (exammmonth == "12")
                                            {
                                                exammmonth = "Dec ";
                                            }

                                            Gios.Pdf.PdfTable table1forpage1 = mydoc.NewTable(Fontarial9, rowcount, 8, 1);
                                            table1forpage1.VisibleHeaders = false;
                                            table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                                            table1forpage1.SetColumnsWidth(new int[] { 15, 30, 220, 34, 34, 34, 34, 50 });
                                            table1forpage1.Cell(i, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1forpage1.Cell(i, 0).SetContent(semroman);
                                            table1forpage1.Cell(i, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1forpage1.Cell(i, 1).SetContent(printds_rows.Tables[0].Rows[i]["subject_code"].ToString());
                                            table1forpage1.Cell(i, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1forpage1.Cell(i, 2).SetContent(printds_rows.Tables[0].Rows[i]["subject_name"].ToString());
                                            double totfinal = Convert.ToDouble(printds_rows.Tables[0].Rows[i]["total"].ToString());
                                            totfinal = Math.Round(totfinal, 0);
                                            table1forpage1.Cell(i, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1forpage1.Cell(i, 3).SetContent(totfinal);
                                            string flag = da.GetFunction("select grade_flag from grademaster");
                                            if (flag == "3")
                                            {
                                                string gradesql = " select Mark_Grade,Credit_Points from Grade_Master where College_Code='" + collegecode + "' and semester='0' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'  and '" + totfinal + "'  between Frange and Trange";
                                                DataSet gradedscal = da.select_method_wo_parameter(gradesql, "text");
                                                if (gradedscal.Tables[0].Rows.Count > 0)
                                                {
                                                    markgrade = gradedscal.Tables[0].Rows[0][0].ToString();
                                                    creditponts = gradedscal.Tables[0].Rows[0][1].ToString();
                                                    table1forpage1.Cell(i, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table1forpage1.Cell(i, 4).SetContent(printds_rows.Tables[0].Rows[i]["credit_points"].ToString());
                                                    table1forpage1.Cell(i, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table1forpage1.Cell(i, 5).SetContent(markgrade);
                                                    table1forpage1.Cell(i, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table1forpage1.Cell(i, 6).SetContent(creditponts);
                                                    table1forpage1.Cell(i, 7).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table1forpage1.Cell(i, 7).SetContent(exammmonth.Trim() + "-" + exam[2].Trim());
                                                    Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 34, coltop, 544, 1000));
                                                    mypdfpage.Add(newpdftabpage2);
                                                    coltop = coltop + 8;
                                                }
                                            }
                                        }
                                        colvel = coltop;
                                        colvel = colvel + 150;
                                        PdfTextArea pdfdegree123 = new PdfTextArea(Fontpala12, System.Drawing.Color.Black, new PdfArea(mydoc, 20, colvel, 544, 50), System.Drawing.ContentAlignment.TopCenter, "*** End of Statement ***");
                                        mypdfpage.Add(pdfdegree123);
                                    }
                                    generatefolio();
                                    generate = da.GetFunction("select value from Master_Settings where settings='Consolidate Sheet' ");
                                    if (!string.IsNullOrEmpty(generate) && generate!="0")
                                    {
                                        string[] split = generate.Split('-');
                                        generate = split[0].ToString() + split[1].ToString();
                                        PdfTextArea pdfdegree253 = new PdfTextArea(fontarial12, System.Drawing.Color.Black, new PdfArea(mydoc, 515, 95, 544, 50), System.Drawing.ContentAlignment.TopLeft, generate);

                                        mypdfpage.Add(pdfdegree253);
                                    }
                                        PdfTextArea pdfdegree233 = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 50, 930, 544, 50), System.Drawing.ContentAlignment.TopLeft, txtdop.Text);
                                        mypdfpage.Add(pdfdegree233);
                                        PdfTextArea pdfdegree23 = new PdfTextArea(fontname, System.Drawing.Color.Black, new PdfArea(mydoc, 385, 930, 544, 50), System.Drawing.ContentAlignment.TopLeft, "English");
                                        mypdfpage.Add(pdfdegree23);
                                   
                                    string cgpa = da.Calculete_CGPA(rollnosub, sem, degree_code, batch_year, latmode, collegecode);

                                    Gios.Pdf.PdfTable table1forpage11 = mydoc.NewTable(fontname, 2, 2, 4);
                                    table1forpage11.VisibleHeaders = false;
                                    table1forpage11.SetBorders(Color.Black, 1, BorderType.None);
                                    table1forpage11.SetColumnsWidth(new int[] { 94, 122 });
                                    table1forpage11.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                                    table1forpage11.Cell(1, 0).SetContent(cgpa);
                                    if (cgpa == "-")
                                    {
                                        cgpa = "0";
                                    }
                                    string gettype = da.GetFunction("Select Edu_Level from course c,degree d where c.course_id=d.course_id and d.degree_code='" + degree_code + "'");

                                    string classcal = "select classification from coe_classification where edu_level = '" + gettype + "' and collegecode = '" + Session["collegecode"].ToString() + "'  and '" + cgpa + "' between frompoint and topoint ";
                                    DataSet dsclass = da.select_method_wo_parameter(classcal, "text");
                                    if (dsclass.Tables[0].Rows.Count > 0)
                                    {
                                        classcalcu = dsclass.Tables[0].Rows[0]["classification"].ToString();
                                        table1forpage11.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1forpage11.Cell(1, 1).SetContent(classcalcu);
                                    }
                                    else
                                    {
                                        errmsg.Visible = true;
                                        errmsg.Text = "No Records Found";
                                        classcalcu = " ";
                                        table1forpage11.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1forpage11.Cell(1, 1).SetContent(classcalcu.ToString());
                                    }

                                    Gios.Pdf.PdfTablePage newpdftabpage12 = table1forpage11.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 250, 950, 300, 50));
                                    mypdfpage.Add(newpdftabpage12);
                                    mypdfpage.SaveToDocument();
                                    string appPath = HttpContext.Current.Server.MapPath("~");
                                    if (appPath != "")
                                    {
                                        string szPath = appPath + "/Report/";
                                        string szFile = "Consolidatemarksheet" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                                        mydoc.SaveToFile(szPath + szFile);
                                        Response.ClearHeaders();
                                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                        Response.ContentType = "application/pdf";
                                        Response.WriteFile(szPath + szFile);
                                    }
                                }
                            }

                        }
                        else
                        {
                            IblError.Visible = true;
                            IblError.Text = "Please Select Any One Record And Then Proceed";
                        }
                    }
                }
            }
            else if (ddlformate.SelectedIndex == 2)
            {
                printbtn_Clickfrmt3();
            }
            else if (ddlformate.SelectedIndex == 3)
            {
                bindformate4mcc();
            }
        }
        catch (Exception ex)
        {
            IblError.Visible = true;
            IblError.Text = ex.ToString();
        }
    }   //////Added By JeyaGandhi //////////

    public void bindformate4mcc()
    {
        DataSet printds = new DataSet();

        DataSet printds_new = new DataSet();
        DataSet printds_rows = new DataSet();


        string degree = "";
        string monthandyear = "";
        string studname = "";
        string dob = "";
        string rollnosub = "";
        string regnumber = "";
        string batch_year = "";
        string degree_code = "";
        string exam_code = "";
        string sem = "";
        //int max_internal_mark = 0;
        //int max_external_mark = 0;
        int colval = 0;
        string branch = "";
        int month = 0;
        string monthstr = "";
        string sql2 = "";
        string sql3 = "";
        string roman = "";
        string semroman = "";
        string grade = "";
        string gradepoints = "";
        string coe = "";
        string subjectcode_Part1 = "";
        string subjectcode_Part2 = "";
        string subjectcode_Part3 = "";
        string subjectcode_Part4 = "";
        string cal_gpa = "";
        string principal = "";

        string subtype = "";
        DataSet gradeds = new DataSet();

        Font Fontco12 = new Font("Comic Sans MS", 12, FontStyle.Bold);
        Font Fontpala12 = new Font("Palatino Linotype", 10, FontStyle.Bold);
        Font Fontco10 = new Font("Comic Sans MS", 10, FontStyle.Regular);
        Font Fontco12a = new Font("Comic Sans MS", 12, FontStyle.Regular);
        Font Fontarial7 = new Font("Arial", 7, FontStyle.Regular);
        Font Fontarial9 = new Font("Arial", 9, FontStyle.Regular);

        Font Fontarial10 = new Font("Arial", 10, FontStyle.Regular);
        Font Fontarial12 = new Font("Arial", 12, FontStyle.Regular);
        //Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);

        //Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
        Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(25.5, 35));
        Gios.Pdf.PdfPage mypdfpage;
        if ((ddlMonth.SelectedValue.ToString() != "0") && (ddlYear.SelectedValue.ToString() != "0"))
        {

            FpSpread2.SaveChanges();
            for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
            {
                int isval = 0;

                string s = FpSpread2.Sheets[0].Cells[res, 6].Text;
                isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);
                double minimumcreditsreqiur = 0;
                if (isval == 1)
                {
                    printds.Clear();
                    printds.Dispose();
                    printds_new.Clear();
                    printds_new.Dispose();
                    rollnosub = FpSpread2.Sheets[0].Cells[res, 1].Note;
                    regnumber = FpSpread2.Sheets[0].Cells[res, 2].Text;
                    string sql = "SELECT Reg_No,r.batch_year, month(Adm_Date) as monthm,r.app_no,r.degree_code,R.current_semester,R.Stud_Name,Course_Name as degree,Dept_name,CONVERT(VARCHAR, dob, 103) as dob FROM Registration R,Applyn A,Degree G,Course C,Department D WHERE R.App_No = A.App_No AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND D.College_Code = G.College_Code AND Roll_No = '" + rollnosub + "' and r.college_code='" + Session["collegecode"].ToString() + "' ;  select coe,principal from collinfo where college_code='" + Session["collegecode"].ToString() + "';";

                    printds = da.select_method_wo_parameter(sql, "Text");
                    if (printds.Tables[0].Rows.Count > 0)
                    {
                        mypdfpage = mydoc.NewPage();
                        degree = printds.Tables[0].Rows[0]["degree"].ToString();
                        coe = printds.Tables[1].Rows[0]["coe"].ToString();
                        principal = printds.Tables[1].Rows[0]["principal"].ToString();
                        string[] spiltcoe = coe.Split('.');
                        string newcoe = "";
                        for (int ce = 1; ce <= spiltcoe.GetUpperBound(0); ce++)
                        {
                            if (newcoe == "")
                            {
                                newcoe = spiltcoe[ce].ToString();
                            }
                            else
                            {
                                newcoe = newcoe + "." + spiltcoe[ce].ToString();
                            }
                        }
                        coe = newcoe;
                        month = ddlMonth.SelectedIndex;

                        monthstr = ddlMonth.SelectedIndex.ToString();
                        string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

                        // string strMonthName = ddlMonth.SelectedItem.Text.Trim();
                        monthandyear = strMonthName + "  " + ddlYear.SelectedItem.Text;
                        monthandyear = monthandyear.ToUpper();
                        studname = printds.Tables[0].Rows[0]["Stud_Name"].ToString();
                        branch = printds.Tables[0].Rows[0]["Dept_name"].ToString();
                        dob = printds.Tables[0].Rows[0]["dob"].ToString();

                        string stdappno = printds.Tables[0].Rows[0]["App_No"].ToString();
                        batch_year = printds.Tables[0].Rows[0]["batch_year"].ToString();
                        degree_code = printds.Tables[0].Rows[0]["degree_code"].ToString();

                        string joinmonth = printds.Tables[0].Rows[0]["monthm"].ToString();

                        joinmonth = batch_year + " - " + ddlMonth.Items[Convert.ToInt32(joinmonth)].Text.ToString().ToUpper();

                        //sql2 = "select * from exam_details where    degree_code='" + degree_code + "' and current_semester='" + sem + "' and exam_month='" + month + "' and exam_year='" + ddlYear.SelectedItem.Text + "'";

                        //sql2 = "select * from exam_details where    degree_code='" + degree_code + "'  and exam_month='" + month + "' and exam_year='" + ddlYear.SelectedItem.Text + "' and  batch_year=" + batch_year + "";

                        //printds_new = da.select_method_wo_parameter(sql2, "Text");
                        //if (printds_new.Tables[0].Rows.Count > 0)
                        //{
                        sem = printds.Tables[0].Rows[0]["current_semester"].ToString();

                        if (sem == "1")
                        {
                            semroman = "I";
                        }
                        else if (sem == "2")
                        {
                            semroman = "II";
                        }
                        else if (sem == "3")
                        {
                            semroman = "III";
                        }
                        else if (sem == "4")
                        {
                            semroman = "IV";
                        }
                        else if (sem == "5")
                        {
                            semroman = "V";
                        }
                        else if (sem == "6")
                        {
                            semroman = "VI";
                        }
                        else if (sem == "7")
                        {
                            semroman = "VII";
                        }
                        else if (sem == "8")
                        {
                            semroman = "VIII";
                        }

                        string stdphtsql = "select * from StdPhoto where app_no='" + stdappno + "'";
                        MemoryStream memoryStream = new MemoryStream();
                        DataSet dsstdpho = new DataSet();
                        dsstdpho.Clear();
                        dsstdpho.Dispose();
                        dsstdpho = da.select_method_wo_parameter(stdphtsql, "Text");
                        if (dsstdpho.Tables[0].Rows.Count > 0)
                        {
                            byte[] file = (byte[])dsstdpho.Tables[0].Rows[0][1];
                            memoryStream.Write(file, 0, file.Length);
                            if (file.Length > 0)
                            {
                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg")))
                                {
                                    //image3.ImageUrl = "~/college/" + stdappno + ".jpeg";
                                }
                                else
                                {
                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                    //image3.ImageUrl = "~/college/" + stdappno + ".jpeg";
                                }



                            }

                        }


                        PdfTextArea pdfdoi = new PdfTextArea(Fontco10, System.Drawing.Color.Black, new PdfArea(mydoc, 57, 968, 305, 50), System.Drawing.ContentAlignment.TopLeft, txtdoi.Text.ToString());
                        mypdfpage.Add(pdfdoi);
                        // exam_code = printds_new.Tables[0].Rows[0]["exam_code"].ToString();

                        //sql3 = "Select semester,Subject_Type,SubTypeAcr,TextVal as Parts,subject_name,subject_code,s.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,maxtotal,grade,cp,credit_points,m.subject_no,semester,maxtotal,m.exam_code, convert(varchar(20),exam_month)+'-'+convert(varchar(20),exam_year)  as monthyear from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sy,exam_details ed,TextValTable where sy.syll_code=s.syll_code and ss.syll_code=sy.syll_code and s.syll_code=ss.syll_code and s.subtype_no= ss.subtype_no and m.Subject_No = s.Subject_No and upper(result) ='PASS' and  m.exam_code = ed.exam_code  and roll_no='" + rollnosub + "' and SubTypePart=TextCode order by sy.semester,subject_code, LEN(subject_type),subject_type";

                        sql3 = "Select semester,Subject_Type,s.acronym,isnull(s.Part_Type,'1') as Part_Type,subject_name,subject_code,s.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,maxtotal,grade,cp,credit_points,m.subject_no,semester,maxtotal,m.exam_code, convert(varchar(20),exam_month)+'-'+convert(varchar(20),exam_year)  as monthyear from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sy,exam_details ed where sy.syll_code=s.syll_code and ss.syll_code=sy.syll_code and s.syll_code=ss.syll_code and s.subtype_no= ss.subtype_no and m.Subject_No = s.Subject_No and upper(result) ='PASS' and  m.exam_code = ed.exam_code  and roll_no='" + rollnosub + "'  order by sy.semester,subject_code, LEN(subject_type),subject_type";

                        printds_rows.Clear();
                        printds_rows.Dispose();
                        printds_rows = da.select_method_wo_parameter(sql3, "Text");



                        strMonthName = ddlMonth.SelectedItem.Text;
                        Gios.Pdf.PdfTable table1forpage2 = mydoc.NewTable(Fontpala12, 1, 5, 1);

                        table1forpage2.VisibleHeaders = false;
                        table1forpage2.SetBorders(Color.Black, 1, BorderType.None);
                        table1forpage2.SetColumnsWidth(new int[] { 258, 88, 111, 145, 91 });
                        table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 0).SetContent(studname);
                        table1forpage2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 1).SetContent(dob);
                        table1forpage2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 2).SetContent(regnumber);
                        table1forpage2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 3).SetContent(branch);
                        table1forpage2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 4).SetContent(joinmonth);
                        Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 9, 142, 709, 50));//sr
                        mypdfpage.Add(newpdftabpage2);
                        colval = 200;

                        if (printds_rows.Tables[0].Rows.Count > 0)
                        {
                            Gios.Pdf.PdfTable table1marks = mydoc.NewTable(Fontarial10, printds_rows.Tables[0].Rows.Count + 1, 10, 2);
                            table1marks.VisibleHeaders = false;
                            table1marks.SetBorders(Color.Black, 1, BorderType.None);
                            table1marks.SetColumnsWidth(new int[] { 71, 249, 45, 45, 45, 54, 54, 45, 45, 74 });
                            for (int i = 0; i < printds_rows.Tables[0].Rows.Count; i++)
                            {

                                //if (printds_new.Tables[0].Rows.Count > 0)
                                //{
                                table1marks.Cell(i, 0).SetContent(printds_rows.Tables[0].Rows[i]["subject_code"].ToString());
                                table1marks.Cell(i, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                table1marks.Cell(i, 1).SetContent(printds_rows.Tables[0].Rows[i]["subject_name"].ToString());
                                table1marks.Cell(i, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1marks.Cell(i, 2).SetContent(printds_rows.Tables[0].Rows[i]["acronym"].ToString());
                                table1marks.Cell(i, 2).SetContentAlignment(ContentAlignment.TopLeft);
                                table1marks.Cell(i, 3).SetContent(printds_rows.Tables[0].Rows[i]["credit_points"].ToString());
                                minimumcreditsreqiur = minimumcreditsreqiur + Convert.ToDouble(printds_rows.Tables[0].Rows[i]["credit_points"].ToString());
                                table1marks.Cell(i, 3).SetContentAlignment(ContentAlignment.TopCenter);
                                table1marks.Cell(i, 4).SetContent(printds_rows.Tables[0].Rows[i]["internal_mark"].ToString());
                                table1marks.Cell(i, 4).SetContentAlignment(ContentAlignment.TopCenter);
                                table1marks.Cell(i, 5).SetContent(printds_rows.Tables[0].Rows[i]["external_mark"].ToString());
                                table1marks.Cell(i, 5).SetContentAlignment(ContentAlignment.TopCenter);
                                double totfinal = Convert.ToDouble(printds_rows.Tables[0].Rows[i]["total"].ToString());
                                totfinal = Math.Round(totfinal, 0);
                                table1marks.Cell(i, 6).SetContent(totfinal);
                                table1marks.Cell(i, 6).SetContentAlignment(ContentAlignment.TopCenter);


                                double checkmarkmm = Convert.ToDouble(printds_rows.Tables[0].Rows[i]["total"].ToString());
                                string gradesql = "    select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='" + sem + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 16/aug 2014
                                gradeds.Clear();
                                gradeds = da.select_method_wo_parameter(gradesql, "Text");
                                if (gradeds.Tables[0].Rows.Count == 0)
                                {
                                    gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 
                                    gradeds.Clear();
                                    gradeds = da.select_method_wo_parameter(gradesql, "Text");
                                }
                                for (int grd = 0; grd < gradeds.Tables[0].Rows.Count; grd++)
                                {
                                    if (Convert.ToInt32(gradeds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(gradeds.Tables[0].Rows[grd][2]) >= checkmarkmm)
                                    {
                                        grade = gradeds.Tables[0].Rows[grd][0].ToString();
                                        gradepoints = gradeds.Tables[0].Rows[grd]["Credit_Points"].ToString();
                                    }

                                }
                                double gradeibtpoint = Convert.ToDouble(printds_rows.Tables[0].Rows[i]["total"].ToString());
                                gradeibtpoint = gradeibtpoint / 10;
                                gradeibtpoint = Math.Round(gradeibtpoint, 1);
                                gradepoints = Convert.ToString(gradeibtpoint);

                                string result = ddlYear.SelectedItem.Text + " - " + ddlMonth.SelectedItem.Text.ToUpper();
                                //if (result == "fail")
                                //{
                                //    result = "RA";
                                //    grade = "U";
                                //}
                                //else if (result == "pass")
                                //{
                                //    result = "P";
                                //}
                                //else
                                //{
                                //    result = "AAA";
                                //    grade = "U";
                                //}

                                table1marks.Cell(i, 7).SetContentAlignment(ContentAlignment.TopCenter);
                                table1marks.Cell(i, 7).SetContent(gradepoints);

                                table1marks.Cell(i, 8).SetContentAlignment(ContentAlignment.TopCenter);
                                table1marks.Cell(i, 8).SetContent(grade);
                                table1marks.Cell(i, 9).SetContentAlignment(ContentAlignment.TopCenter);
                                table1marks.Cell(i, 9).SetContent(result);

                                //}


                            }
                            table1marks.Cell(printds_rows.Tables[0].Rows.Count, 0).SetContent(" --End Of Statement-- ");
                            table1marks.Cell(printds_rows.Tables[0].Rows.Count, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1marks.Cell(printds_rows.Tables[0].Rows.Count, 0).SetCellPadding(10);
                            table1marks.Cell(printds_rows.Tables[0].Rows.Count, 0).SetFont(Fontpala12);

                            foreach (PdfCell pr in table1marks.CellRange(printds_rows.Tables[0].Rows.Count, 0, printds_rows.Tables[0].Rows.Count, 0).Cells)
                            {
                                pr.ColSpan = 10;
                            }

                            Gios.Pdf.PdfTablePage newpdftable1table1marks = table1marks.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 9, 198, 709, 609));
                            mypdfpage.Add(newpdftable1table1marks);


                        }

                        //}
                        double totalcreditsearned = 0;

                        Gios.Pdf.PdfTable table1forpagecoe = mydoc.NewTable(Fontarial9, 1, 2, 1);
                        table1forpagecoe.VisibleHeaders = false;
                        table1forpagecoe.SetBorders(Color.Black, 1, BorderType.None);
                        table1forpagecoe.SetColumnsWidth(new int[] { 198, 198 });
                        table1forpagecoe.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpagecoe.Cell(0, 0).SetContent(coe);
                        table1forpagecoe.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpagecoe.Cell(0, 1).SetContent(principal);
                        Gios.Pdf.PdfTablePage newpdftabpagecoe = table1forpagecoe.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 326, 959, 397, 50));
                        mypdfpage.Add(newpdftabpagecoe);

                        string year = ddlYear.SelectedItem.Text;
                        string collcode = Session["collegecode"].ToString();

                        Gios.Pdf.PdfTable table1forpagegpa = mydoc.NewTable(Fontarial9, 5, 6, 1);
                        table1forpagegpa.VisibleHeaders = false;
                        table1forpagegpa.SetBorders(Color.Black, 1, BorderType.None);
                        table1forpagegpa.SetColumnsWidth(new int[] { 31, 113, 60, 60, 60, 196 });
                        table1forpagegpa.SetContentAlignment(ContentAlignment.MiddleCenter);


                        double partsums = 0.00;
                        int partrowcount = 0;
                        Double Credit_Points = 0.0;
                        Double grade_points = 0.0;
                        Double wpm = 0.0;
                        double creditstotal = 0;
                        double finalgpa1 = 0;
                        double finalwpm = 0;

                        if (printds_rows.Tables[0].Rows.Count > 0)
                        {
                            string sumpart = "";

                            DataView dv_demand_data = new DataView();
                            DataView dv_demand_datadummy = new DataView();
                            printds_rows.Tables[0].DefaultView.RowFilter = "Part_Type='1'";
                            dv_demand_datadummy = printds_rows.Tables[0].DefaultView;
                            partrowcount = dv_demand_datadummy.Count;
                            printds_rows.Tables[0].DefaultView.RowFilter = "Part_Type='1' and result='pass'";
                            dv_demand_data = printds_rows.Tables[0].DefaultView;
                            if (dv_demand_data.Count > 0 && partrowcount == dv_demand_data.Count)
                            {


                                for (int sum = 0; sum < dv_demand_data.Count; sum++)
                                {
                                    double checkmarkmm = Convert.ToDouble(dv_demand_data[sum]["total"].ToString());
                                    string gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='" + sem + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 16/aug 2014
                                    gradeds.Clear();
                                    gradeds = da.select_method_wo_parameter(gradesql, "Text");
                                    if (gradeds.Tables[0].Rows.Count == 0)
                                    {
                                        gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 
                                        gradeds.Clear();
                                        gradeds = da.select_method_wo_parameter(gradesql, "Text");
                                    }
                                    for (int grd = 0; grd < gradeds.Tables[0].Rows.Count; grd++)
                                    {
                                        if (Convert.ToInt32(gradeds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(gradeds.Tables[0].Rows[grd][2]) >= checkmarkmm)
                                        {
                                            Credit_Points = Convert.ToDouble(dv_demand_data[sum]["Credit_Points"].ToString());
                                            totalcreditsearned = totalcreditsearned + Credit_Points;
                                            // grade_points = grade_points / 10;
                                            grade_points = Convert.ToDouble(gradeds.Tables[0].Rows[grd]["Credit_Points"].ToString());
                                            creditstotal = creditstotal + Credit_Points;
                                            partsums = partsums + (grade_points * Credit_Points);
                                            wpm = wpm + partsums + (Convert.ToDouble(dv_demand_data[sum]["total"].ToString()) * Credit_Points);
                                        }

                                    }
                                }
                                partsums = Math.Round(partsums, 2);
                                sumpart = String.Format("{0:0.00}", partsums);
                            }
                            else if (partrowcount > 0)
                            {
                                sumpart = "0.00";
                            }
                            else
                            {
                                sumpart = "-";
                            }
                            //cal_gpa = Calulat_GPA_forpart(rollnosub, degree_code, batch_year, monthstr, year, collcode, subjectcode_Part1);
                            if (creditstotal > 0)
                            {
                                finalgpa1 = Math.Round((partsums / creditstotal), 2, MidpointRounding.AwayFromZero);
                                finalwpm = Math.Round((wpm / creditstotal), 2, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                finalgpa1 = 0;
                                finalwpm = 0;
                            }
                            table1forpagegpa.Cell(0, 0).SetContent("I");
                            table1forpagegpa.Cell(0, 1).SetContent(creditstotal);
                            table1forpagegpa.Cell(0, 2).SetContent(finalwpm);
                            table1forpagegpa.Cell(0, 3).SetContent(finalgpa1);
                            string classcoe = da.GetFunction("select classification from coe_classification where '" + finalgpa1 + "' between frompoint and topoint");
                            string gradecoe = da.GetFunction("select grade from coe_classification where '" + finalgpa1 + "' between frompoint and topoint");
                            table1forpagegpa.Cell(0, 4).SetContent(gradecoe);
                            table1forpagegpa.Cell(0, 5).SetContent(classcoe);
                            table1forpagegpa.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleLeft);

                        }
                        else
                        {
                            //table1forpagegpa.Cell(0, 0).SetContent("-");
                        }
                        partsums = 0;
                        partrowcount = 0;
                        creditstotal = 0;
                        finalgpa1 = 0;
                        finalwpm = 0;
                        table1forpagegpa.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                        if (printds_rows.Tables[0].Rows.Count > 0)
                        {
                            string sumpart = "";
                            DataView dv_demand_data = new DataView();
                            DataView dv_demand_datadummy = new DataView();
                            printds_rows.Tables[0].DefaultView.RowFilter = "Part_Type='2'";
                            dv_demand_datadummy = printds_rows.Tables[0].DefaultView;
                            partrowcount = dv_demand_datadummy.Count;
                            printds_rows.Tables[0].DefaultView.RowFilter = "Part_Type='2' and result='pass'";
                            dv_demand_data = printds_rows.Tables[0].DefaultView;
                            if (dv_demand_data.Count > 0 && partrowcount == dv_demand_data.Count)
                            {


                                for (int sum = 0; sum < dv_demand_data.Count; sum++)
                                {
                                    double checkmarkmm = Convert.ToDouble(dv_demand_data[sum]["total"].ToString());
                                    string gradesql = "    select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='" + sem + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 16/aug 2014
                                    gradeds.Clear();
                                    gradeds = da.select_method_wo_parameter(gradesql, "Text");
                                    if (gradeds.Tables[0].Rows.Count == 0)
                                    {
                                        gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 
                                        gradeds.Clear();
                                        gradeds = da.select_method_wo_parameter(gradesql, "Text");
                                    }
                                    for (int grd = 0; grd < gradeds.Tables[0].Rows.Count; grd++)
                                    {
                                        if (Convert.ToInt32(gradeds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(gradeds.Tables[0].Rows[grd][2]) >= checkmarkmm)
                                        {
                                            Credit_Points = Convert.ToDouble(dv_demand_data[sum]["Credit_Points"].ToString());
                                            totalcreditsearned = totalcreditsearned + Credit_Points;
                                            // grade_points = grade_points / 10;
                                            grade_points = Convert.ToDouble(gradeds.Tables[0].Rows[grd]["Credit_Points"].ToString());
                                            creditstotal = creditstotal + Credit_Points;
                                            partsums = partsums + (grade_points * Credit_Points);
                                            wpm = wpm + partsums + (Convert.ToDouble(dv_demand_data[sum]["total"].ToString()) * Credit_Points);
                                        }

                                    }
                                }
                                partsums = Math.Round(partsums, 2);
                                sumpart = String.Format("{0:0.00}", partsums);
                            }
                            else if (partrowcount > 0)
                            {
                                sumpart = "0.00";
                            }
                            else
                            {
                                sumpart = "-";
                            }
                            //cal_gpa = Calulat_GPA_forpart(rollnosub, degree_code, batch_year, monthstr, year, collcode, subjectcode_Part1);

                            if (creditstotal > 0)
                            {
                                finalgpa1 = Math.Round((partsums / creditstotal), 2, MidpointRounding.AwayFromZero);
                                finalwpm = Math.Round((wpm / creditstotal), 2, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                finalgpa1 = 0;
                                finalwpm = 0;
                            }
                            table1forpagegpa.Cell(1, 0).SetContent("II");
                            table1forpagegpa.Cell(1, 1).SetContent(creditstotal);
                            table1forpagegpa.Cell(1, 2).SetContent(finalwpm);
                            table1forpagegpa.Cell(1, 3).SetContent(finalgpa1);
                            string classcoe = da.GetFunction("select classification from coe_classification where '" + finalgpa1 + "' between frompoint and topoint");
                            string gradecoe = da.GetFunction("select grade from coe_classification where '" + finalgpa1 + "' between frompoint and topoint");
                            table1forpagegpa.Cell(1, 4).SetContent(gradecoe);
                            table1forpagegpa.Cell(1, 5).SetContent(classcoe);
                            table1forpagegpa.Cell(1, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                        }
                        else
                        {
                            // table1forpagegpa.Cell(0, 1).SetContent("-");
                        }
                        partsums = 0;
                        partrowcount = 0;
                        creditstotal = 0;
                        finalgpa1 = 0;
                        finalwpm = 0;
                        if (printds_rows.Tables[0].Rows.Count > 0)
                        {
                            string sumpart = "";
                            DataView dv_demand_data = new DataView();
                            DataView dv_demand_datadummy = new DataView();
                            printds_rows.Tables[0].DefaultView.RowFilter = "Part_Type='3'";
                            dv_demand_datadummy = printds_rows.Tables[0].DefaultView;
                            partrowcount = dv_demand_datadummy.Count;
                            printds_rows.Tables[0].DefaultView.RowFilter = "Part_Type='3' and result='pass'";
                            dv_demand_data = printds_rows.Tables[0].DefaultView;
                            if (dv_demand_data.Count > 0 && partrowcount == dv_demand_data.Count)
                            {

                                for (int sum = 0; sum < dv_demand_data.Count; sum++)
                                {
                                    double checkmarkmm = Convert.ToDouble(dv_demand_data[sum]["total"].ToString());
                                    string gradesql = "    select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='" + sem + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 16/aug 2014
                                    gradeds.Clear();
                                    gradeds = da.select_method_wo_parameter(gradesql, "Text");
                                    if (gradeds.Tables[0].Rows.Count == 0)
                                    {
                                        gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 
                                        gradeds.Clear();
                                        gradeds = da.select_method_wo_parameter(gradesql, "Text");
                                    }
                                    for (int grd = 0; grd < gradeds.Tables[0].Rows.Count; grd++)
                                    {
                                        if (Convert.ToInt32(gradeds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(gradeds.Tables[0].Rows[grd][2]) >= checkmarkmm)
                                        {
                                            Credit_Points = Convert.ToDouble(dv_demand_data[sum]["Credit_Points"].ToString());
                                            totalcreditsearned = totalcreditsearned + Credit_Points;
                                            // grade_points = grade_points / 10;
                                            grade_points = Convert.ToDouble(gradeds.Tables[0].Rows[grd]["Credit_Points"].ToString());
                                            creditstotal = creditstotal + Credit_Points;
                                            partsums = partsums + (grade_points * Credit_Points);
                                            wpm = wpm + partsums + (Convert.ToDouble(dv_demand_data[sum]["total"].ToString()) * Credit_Points);
                                        }

                                    }
                                }
                                partsums = Math.Round(partsums, 2);
                                sumpart = String.Format("{0:0.00}", partsums);
                            }
                            else if (partrowcount > 0)
                            {
                                sumpart = "0.00";
                            }
                            else
                            {
                                sumpart = "-";
                            }
                            //cal_gpa = Calulat_GPA_forpart(rollnosub, degree_code, batch_year, monthstr, year, collcode, subjectcode_Part1);
                            if (creditstotal > 0)
                            {
                                finalgpa1 = Math.Round((partsums / creditstotal), 2, MidpointRounding.AwayFromZero);
                                finalwpm = Math.Round((wpm / creditstotal), 2, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                finalgpa1 = 0;
                                finalwpm = 0;
                            }

                            table1forpagegpa.Cell(2, 0).SetContent("III");
                            table1forpagegpa.Cell(2, 1).SetContent(creditstotal);
                            table1forpagegpa.Cell(2, 2).SetContent(finalwpm);
                            table1forpagegpa.Cell(2, 3).SetContent(finalgpa1);
                            string classcoe = da.GetFunction("select classification from coe_classification where '" + finalgpa1 + "' between frompoint and topoint");
                            string gradecoe = da.GetFunction("select grade from coe_classification where '" + finalgpa1 + "' between frompoint and topoint");
                            table1forpagegpa.Cell(2, 4).SetContent(gradecoe);
                            table1forpagegpa.Cell(2, 5).SetContent(classcoe);
                            table1forpagegpa.Cell(2, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                        }
                        else
                        {
                            //table1forpagegpa.Cell(0, 2).SetContent("-");
                        }
                        partsums = 0;
                        partrowcount = 0;
                        creditstotal = 0;
                        finalgpa1 = 0;
                        finalwpm = 0;
                        if (printds_rows.Tables[0].Rows.Count > 0)
                        {
                            string sumpart = "";
                            DataView dv_demand_data = new DataView();
                            DataView dv_demand_datadummy = new DataView();
                            printds_rows.Tables[0].DefaultView.RowFilter = "Part_Type='4'";
                            dv_demand_datadummy = printds_rows.Tables[0].DefaultView;
                            partrowcount = dv_demand_datadummy.Count;
                            printds_rows.Tables[0].DefaultView.RowFilter = "Part_Type='4' and result='pass'";
                            dv_demand_data = printds_rows.Tables[0].DefaultView;
                            if (dv_demand_data.Count > 0 && partrowcount == dv_demand_data.Count)
                            {
                                for (int sum = 0; sum < dv_demand_data.Count; sum++)
                                {
                                    double checkmarkmm = Convert.ToDouble(dv_demand_data[sum]["total"].ToString());
                                    string gradesql = "    select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='" + sem + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 16/aug 2014
                                    gradeds.Clear();
                                    gradeds = da.select_method_wo_parameter(gradesql, "Text");
                                    if (gradeds.Tables[0].Rows.Count == 0)
                                    {
                                        gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 
                                        gradeds.Clear();
                                        gradeds = da.select_method_wo_parameter(gradesql, "Text");
                                    }
                                    for (int grd = 0; grd < gradeds.Tables[0].Rows.Count; grd++)
                                    {
                                        if (Convert.ToInt32(gradeds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(gradeds.Tables[0].Rows[grd][2]) >= checkmarkmm)
                                        {
                                            Credit_Points = Convert.ToDouble(dv_demand_data[sum]["Credit_Points"].ToString());
                                            totalcreditsearned = totalcreditsearned + Credit_Points;
                                            // grade_points = grade_points / 10;
                                            grade_points = Convert.ToDouble(gradeds.Tables[0].Rows[grd]["Credit_Points"].ToString());
                                            creditstotal = creditstotal + Credit_Points;
                                            partsums = partsums + (grade_points * Credit_Points);
                                            wpm = wpm + partsums + (Convert.ToDouble(dv_demand_data[sum]["total"].ToString()) * Credit_Points);
                                        }

                                    }
                                }
                                partsums = Math.Round(partsums, 2);
                                sumpart = String.Format("{0:0.00}", partsums);
                            }
                            else if (partrowcount > 0)
                            {
                                sumpart = "0.00";
                            }
                            else
                            {
                                sumpart = "-";
                            }
                            //cal_gpa = Calulat_GPA_forpart(rollnosub, degree_code, batch_year, monthstr, year, collcode, subjectcode_Part1);

                            if (creditstotal > 0)
                            {
                                finalgpa1 = Math.Round((partsums / creditstotal), 2, MidpointRounding.AwayFromZero);
                                finalwpm = Math.Round((wpm / creditstotal), 2, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                finalgpa1 = 0;
                                finalwpm = 0;
                            }
                            table1forpagegpa.Cell(3, 0).SetContent("IV");
                            table1forpagegpa.Cell(3, 1).SetContent(creditstotal);
                            table1forpagegpa.Cell(3, 2).SetContent(finalwpm);
                            table1forpagegpa.Cell(3, 3).SetContent(finalgpa1);
                            string classcoe = da.GetFunction("select classification from coe_classification where '" + finalgpa1 + "' between frompoint and topoint");
                            string gradecoe = da.GetFunction("select grade from coe_classification where '" + finalgpa1 + "' between frompoint and topoint");
                            table1forpagegpa.Cell(3, 4).SetContent(gradecoe);
                            table1forpagegpa.Cell(3, 5).SetContent(classcoe);
                            table1forpagegpa.Cell(3, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                        }
                        else
                        {
                            // table1forpagegpa.Cell(0, 3).SetContent("-");
                        }

                        partsums = 0;
                        partrowcount = 0;
                        creditstotal = 0;
                        finalgpa1 = 0;
                        finalwpm = 0;
                        if (printds_rows.Tables[0].Rows.Count > 0)
                        {
                            string sumpart = "";
                            DataView dv_demand_data = new DataView();
                            DataView dv_demand_datadummy = new DataView();
                            printds_rows.Tables[0].DefaultView.RowFilter = "Part_Type='5'";
                            dv_demand_datadummy = printds_rows.Tables[0].DefaultView;
                            partrowcount = dv_demand_datadummy.Count;
                            printds_rows.Tables[0].DefaultView.RowFilter = "Part_Type='5' and result='pass'";
                            dv_demand_data = printds_rows.Tables[0].DefaultView;
                            if (dv_demand_data.Count > 0 && partrowcount == dv_demand_data.Count)
                            {
                                for (int sum = 0; sum < dv_demand_data.Count; sum++)
                                {
                                    double checkmarkmm = Convert.ToDouble(dv_demand_data[sum]["total"].ToString());
                                    string gradesql = "    select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='" + sem + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 16/aug 2014
                                    gradeds.Clear();
                                    gradeds = da.select_method_wo_parameter(gradesql, "Text");
                                    if (gradeds.Tables[0].Rows.Count == 0)
                                    {
                                        gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 
                                        gradeds.Clear();
                                        gradeds = da.select_method_wo_parameter(gradesql, "Text");
                                    }
                                    for (int grd = 0; grd < gradeds.Tables[0].Rows.Count; grd++)
                                    {
                                        if (Convert.ToInt32(gradeds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(gradeds.Tables[0].Rows[grd][2]) >= checkmarkmm)
                                        {
                                            Credit_Points = Convert.ToDouble(dv_demand_data[sum]["Credit_Points"].ToString());
                                            totalcreditsearned = totalcreditsearned + Credit_Points;
                                            // grade_points = grade_points / 10;
                                            grade_points = Convert.ToDouble(gradeds.Tables[0].Rows[grd]["Credit_Points"].ToString());
                                            creditstotal = creditstotal + Credit_Points;
                                            partsums = partsums + (grade_points * Credit_Points);
                                            wpm = wpm + partsums + (Convert.ToDouble(dv_demand_data[sum]["total"].ToString()) * Credit_Points);
                                        }

                                    }
                                }
                                partsums = Math.Round(partsums, 2);
                                sumpart = String.Format("{0:0.00}", partsums);
                                if (creditstotal > 0)
                                {
                                    finalgpa1 = Math.Round((partsums / creditstotal), 2, MidpointRounding.AwayFromZero);
                                    finalwpm = Math.Round((wpm / creditstotal), 2, MidpointRounding.AwayFromZero);
                                }
                                else
                                {
                                    finalgpa1 = 0;
                                    finalwpm = 0;
                                }
                                table1forpagegpa.Cell(4, 0).SetContent("V");
                                table1forpagegpa.Cell(4, 1).SetContent(creditstotal);
                                table1forpagegpa.Cell(4, 2).SetContent(finalwpm);
                                table1forpagegpa.Cell(4, 3).SetContent(finalgpa1);
                                string classcoe = da.GetFunction("select classification from coe_classification where '" + finalgpa1 + "' between frompoint and topoint");
                                string gradecoe = da.GetFunction("select grade from coe_classification where '" + finalgpa1 + "' between frompoint and topoint");
                                table1forpagegpa.Cell(4, 4).SetContent(gradecoe);
                                table1forpagegpa.Cell(4, 5).SetContent(classcoe);
                                table1forpagegpa.Cell(4, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            }
                            else if (partrowcount > 0)
                            {
                                sumpart = "0.00";
                            }
                            else
                            {
                                sumpart = "-";
                            }
                            //cal_gpa = Calulat_GPA_forpart(rollnosub, degree_code, batch_year, monthstr, year, collcode, subjectcode_Part1);


                        }
                        else
                        {

                        }

                        newpdftabpagecoe = table1forpagegpa.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 9, 822, 499, 200));

                        mypdfpage.Add(newpdftabpagecoe);

                        Gios.Pdf.PdfTable table1forcredits = mydoc.NewTable(Fontarial9, 3, 2, 8);
                        table1forcredits.VisibleHeaders = false;
                        table1forcredits.SetBorders(Color.Black, 1, BorderType.None);
                        table1forcredits.SetColumnsWidth(new int[] { 142, 71 });

                        table1forcredits.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forcredits.Cell(0, 1).SetContent(minimumcreditsreqiur);

                        table1forcredits.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forcredits.Cell(1, 1).SetContent(totalcreditsearned);
                        table1forcredits.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forcredits.Cell(2, 1).SetContent("Pass");
                        Gios.Pdf.PdfTablePage newpdftabpagetable1forcredits = table1forcredits.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 496, 808, 213, 200));
                        mypdfpage.Add(newpdftabpagetable1forcredits);

                        mypdfpage.SaveToDocument();
                    }

                }

            }

        }

        string appPath = HttpContext.Current.Server.MapPath("~");
        if (appPath != "")
        {
            string szPath = appPath + "/Report/";
            string szFile = "marksheet" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";


            mydoc.SaveToFile(szPath + szFile);

            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
            Response.ContentType = "application/pdf";
            Response.WriteFile(szPath + szFile);
        }
    }
    public void printbtn_Click()
    {
        try
        {
            DataSet printds = new DataSet();

            DataSet printds_new = new DataSet();
            DataSet printds_rows = new DataSet();


            string degree = "";
            string monthandyear = "";
            string studname = "";
            string dob = "";
            string rollnosub = "";
            string regnumber = "";
            string batch_year = "";
            string degree_code = "";
            string exam_code = "";
            string sem = "";
            //int max_internal_mark = 0;
            //int max_external_mark = 0;
            int colval = 0;
            string branch = "";
            int month = 0;
            string monthstr = "";
            string sql2 = "";
            string sql3 = "";
            string roman = "";
            string semroman = "";
            string grade = "";
            string gradepoints = "";
            string coe = "";
            string subjectcode_Part1 = "";
            string subjectcode_Part2 = "";
            string subjectcode_Part3 = "";
            string subjectcode_Part4 = "";
            string cal_gpa = "";
            string principal = "";

            string subtype = "";
            DataSet gradeds = new DataSet();

            Font Fontco12 = new Font("Comic Sans MS", 12, FontStyle.Bold);
            Font Fontpala12 = new Font("Palatino Linotype", 10, FontStyle.Bold);
            Font Fontco10 = new Font("Comic Sans MS", 10, FontStyle.Regular);
            Font Fontco12a = new Font("Comic Sans MS", 12, FontStyle.Regular);
            Font Fontarial7 = new Font("Arial", 7, FontStyle.Regular);
            Font Fontarial9 = new Font("Arial", 9, FontStyle.Regular);
            Font Fontarial10 = new Font("Arial", 10, FontStyle.Regular);
            Font Fontarial12 = new Font("Arial", 12, FontStyle.Regular);
            //Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);

            // Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InInches(8.5, 14));
            Gios.Pdf.PdfPage mypdfpage;
            if ((ddlMonth.SelectedValue.ToString() != "0") && (ddlYear.SelectedValue.ToString() != "0"))
            {

                FpSpread2.SaveChanges();
                for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
                {
                    int isval = 0;

                    string s = FpSpread2.Sheets[0].Cells[res, 6].Text;
                    isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);

                    if (isval == 1)
                    {
                        printds.Clear();
                        printds.Dispose();
                        printds_new.Clear();
                        printds_new.Dispose();

                        rollnosub = FpSpread2.Sheets[0].Cells[res, 1].Note;
                        regnumber = FpSpread2.Sheets[0].Cells[res, 2].Text;
                        //string sql = "SELECT Reg_No,r.batch_year,r.app_no,r.degree_code,R.current_semester,R.Stud_Name,Course_Name+'('+Dept_acronym+')' as degree,Dept_name,CONVERT(VARCHAR, dob, 103) as dob FROM Registration R,Applyn A,Degree G,Course C,Department D WHERE R.App_No = A.App_No AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND D.College_Code = G.College_Code AND Roll_No = '" + rollnosub + "' and r.college_code='" + Session["collegecode"].ToString() + "' ;  select coe from collinfo where college_code='" + Session["collegecode"].ToString() + "';";
                        string sql = "SELECT Reg_No,r.batch_year,r.app_no,r.degree_code,R.current_semester,R.Stud_Name,Course_Name as degree,Dept_name,CONVERT(VARCHAR, dob, 103) as dob FROM Registration R,Applyn A,Degree G,Course C,Department D WHERE R.App_No = A.App_No AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND D.College_Code = G.College_Code AND Roll_No = '" + rollnosub + "' and r.college_code='" + Session["collegecode"].ToString() + "' ;  select coe,principal from collinfo where college_code='" + Session["collegecode"].ToString() + "';";//Rajumar 28/5/2018

                        //dummy
                        // sql = "SELECT Reg_No,r.batch_year,r.degree_code,R.current_semester,R.Stud_Name,Course_Name+'('+Dept_acronym+')' as degree,Dept_name,CONVERT(VARCHAR, dob, 103) as dob FROM Registration R,Applyn A,Degree G,Course C,Department D WHERE R.App_No = A.App_No AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND D.College_Code = G.College_Code AND Roll_No = '14UHI6001' ;  select coe from collinfo";

                        printds = da.select_method_wo_parameter(sql, "Text");
                        if (printds.Tables[0].Rows.Count > 0)
                        {
                            mypdfpage = mydoc.NewPage();
                            degree = printds.Tables[0].Rows[0]["degree"].ToString();
                            coe = printds.Tables[1].Rows[0]["coe"].ToString();
                            principal = printds.Tables[1].Rows[0]["principal"].ToString();
                            string[] spiltcoe = coe.Split('.');
                            string newcoe = "";
                            for (int ce = 1; ce <= spiltcoe.GetUpperBound(0); ce++)
                            {
                                if (newcoe == "")
                                {
                                    newcoe = spiltcoe[ce].ToString();
                                }
                                else
                                {
                                    newcoe = newcoe + "." + spiltcoe[ce].ToString();
                                }
                            }
                            coe = newcoe;
                            month = ddlMonth.SelectedIndex;
                            monthstr = ddlMonth.SelectedIndex.ToString();
                            // string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
                            string strMonthName = ddlMonth.SelectedItem.Text.Trim();
                            monthandyear = strMonthName + "  " + ddlYear.SelectedItem.Text;
                            monthandyear = monthandyear.ToUpper();
                            studname = printds.Tables[0].Rows[0]["Stud_Name"].ToString();
                            branch = printds.Tables[0].Rows[0]["Dept_name"].ToString();
                            dob = printds.Tables[0].Rows[0]["dob"].ToString();

                            string stdappno = printds.Tables[0].Rows[0]["App_No"].ToString();
                            batch_year = printds.Tables[0].Rows[0]["batch_year"].ToString();
                            degree_code = printds.Tables[0].Rows[0]["degree_code"].ToString();
                            sem = printds.Tables[0].Rows[0]["current_semester"].ToString();

                            if (sem == "1")
                            {
                                semroman = "I";
                            }
                            else if (sem == "2")
                            {
                                semroman = "II";
                            }
                            else if (sem == "3")
                            {
                                semroman = "III";
                            }
                            else if (sem == "4")
                            {
                                semroman = "IV";
                            }
                            else if (sem == "5")
                            {
                                semroman = "V";
                            }
                            else if (sem == "6")
                            {
                                semroman = "VI";
                            }
                            else if (sem == "7")
                            {
                                semroman = "VII";
                            }
                            else if (sem == "8")
                            {
                                semroman = "VIII";
                            }

                            //sql2 = "select * from exam_details where  batch_year='" + batch_year + "' and   degree_code='" + degree_code + "' and current_semester='" + sem + "' and exam_month='" + month + "' and exam_year='" + ddlYear.SelectedItem.Text + "'";
                            //dummy
                            // sql2 = "select * from exam_details where  batch_year='2014' and   degree_code='45' and current_semester='1' and exam_month='11' and exam_year='2014'";

                            //printds_new = da.select_method_wo_parameter(sql2, "Text");
                            //if (printds_new.Tables[0].Rows.Count > 0)
                            //{


                            string stdphtsql = "select * from StdPhoto where app_no='" + stdappno + "'";
                            MemoryStream memoryStream = new MemoryStream();
                            DataSet dsstdpho = new DataSet();
                            dsstdpho.Clear();
                            dsstdpho.Dispose();
                            dsstdpho = da.select_method_wo_parameter(stdphtsql, "Text");
                            if (dsstdpho.Tables[0].Rows.Count > 0)
                            {
                                byte[] file = (byte[])dsstdpho.Tables[0].Rows[0][1];
                                memoryStream.Write(file, 0, file.Length);
                                if (file.Length > 0)
                                {
                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg")))
                                    {
                                        //image3.ImageUrl = "~/college/" + stdappno + ".jpeg";
                                    }
                                    else
                                    {
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        //image3.ImageUrl = "~/college/" + stdappno + ".jpeg";
                                    }



                                }

                            }

                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg")))
                            {
                                Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg"));
                                mypdfpage.Add(LogoImage2, 25, 895, 450);
                            }
                            else
                            {
                                Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg"));
                                mypdfpage.Add(LogoImage2, 25, 895, 270);
                            }
                            PdfTextArea pdfdoi = new PdfTextArea(Fontco10, System.Drawing.Color.Black, new PdfArea(mydoc, 30, 880, 305, 50), System.Drawing.ContentAlignment.TopLeft, txtdoi.Text.ToString());
                            mypdfpage.Add(pdfdoi);
                            //  exam_code = printds_new.Tables[0].Rows[0]["exam_code"].ToString();

                            // sql3 = "Select Subject_Type,subject_name,subject_code,subject.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,maxtotal,grade,cp,credit_points,mark_entry.subject_no,semester,maxtotal,exam_code from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = '" + exam_code + "' and roll_no='" + rollnosub + "'  order by LEN(subject_type),subject_type";
                            sql3 = "Select semester,Subject_Type,subject_name,subject_code,s.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,maxtotal,grade,cp,credit_points,m.subject_no,semester,maxtotal,m.exam_code, convert(varchar(20),exam_month)+'-'+convert(varchar(20),exam_year)  as monthyear from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sy,exam_details ed where sy.syll_code=s.syll_code and ss.syll_code=sy.syll_code and s.syll_code=ss.syll_code and s.subtype_no= ss.subtype_no and m.Subject_No = s.Subject_No and upper(result) ='PASS' and  m.exam_code = ed.exam_code  and roll_no='" + rollnosub + "' order by sy.semester,subject_code, LEN(subject_type),subject_type";
                            //dummy
                            // sql3 = "Select Subject_Type,subject_name,subject_code,subject.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,maxtotal,grade,cp,credit_points,mark_entry.subject_no,semester,maxtotal,exam_code from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = 10146 and roll_no='14UHI6001'  order by LEN(subject_type),subject_type";
                            printds_rows.Clear();
                            printds_rows.Dispose();
                            printds_rows = da.select_method_wo_parameter(sql3, "Text");


                            //Gios.Pdf.PdfTable table1forpage1 = mydoc.NewTable(Fontco12, 1, 3, 1);

                            //table1forpage1.VisibleHeaders = false;
                            //table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                            //table1forpage1.SetColumnsWidth(new int[] { 91, 147, 99 });
                            //table1forpage1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage1.Cell(0, 0).SetContent(degree);
                            //table1forpage1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage1.Cell(0, 1).SetContent("");
                            //table1forpage1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage1.Cell(0, 2).SetContent(monthandyear);
                            //Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 130, 52, 336, 50));

                            //mypdfpage.Add(newpdftabpage2);

                            Gios.Pdf.PdfTable table1forpage2 = mydoc.NewTable(Fontco10, 1, 3, 1);

                            table1forpage2.VisibleHeaders = false;
                            table1forpage2.SetBorders(Color.Black, 1, BorderType.None);
                            table1forpage2.SetColumnsWidth(new int[] { 354, 108, 108 });
                            table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 0).SetContent(studname);
                            table1forpage2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 1).SetContent(dob);
                            table1forpage2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 2).SetContent(regnumber);
                            Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 27, 106, 567, 50));//sr

                            mypdfpage.Add(newpdftabpage2);

                            Gios.Pdf.PdfTable table1forpage2a = mydoc.NewTable(Fontco10, 1, 3, 1);
                            table1forpage2a.Columns[0].SetWidth(354);
                            table1forpage2a.VisibleHeaders = false;
                            table1forpage2a.SetBorders(Color.Black, 1, BorderType.None);
                            table1forpage2a.SetColumnsWidth(new int[] { 164, 298, 108 });
                            table1forpage2a.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2a.Cell(0, 0).SetContent(degree);
                            table1forpage2a.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2a.Cell(0, 1).SetContent(branch);
                            table1forpage2a.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2a.Cell(0, 2).SetContent(batch_year + "-" + System.DateTime.Now.ToString("yyyy"));
                            //table1forpage2a.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage2a.Cell(0, 3).SetContent(txtdop.Text);
                            newpdftabpage2 = table1forpage2a.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 27, 143, 567, 50));

                            mypdfpage.Add(newpdftabpage2);



                            colval = 200;

                            if (printds_rows.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < printds_rows.Tables[0].Rows.Count; i++)
                                {
                                    subtype = printds_rows.Tables[0].Rows[i]["Subject_Type"].ToString();
                                    string[] spitsubtype = subtype.Split(' ');
                                    if (spitsubtype.GetUpperBound(0) > 0)
                                    {
                                        roman = spitsubtype[1].ToString();
                                        if (roman == "1")
                                        {
                                            roman = "I";
                                            if (subjectcode_Part1 == "")
                                            {
                                                subjectcode_Part1 = printds_rows.Tables[0].Rows[i]["subject_no"].ToString();
                                            }
                                            else
                                            {
                                                subjectcode_Part1 = subjectcode_Part1 + "'" + "," + "'" + printds_rows.Tables[0].Rows[i]["subject_no"].ToString();
                                            }
                                        }
                                        else if (roman == "2")
                                        {
                                            roman = "II";
                                            if (subjectcode_Part2 == "")
                                            {
                                                subjectcode_Part2 = printds_rows.Tables[0].Rows[i]["subject_no"].ToString();
                                            }
                                            else
                                            {
                                                subjectcode_Part2 = subjectcode_Part2 + "'" + "," + "'" + printds_rows.Tables[0].Rows[i]["subject_no"].ToString();
                                            }
                                        }
                                        else if (roman == "3")
                                        {
                                            roman = "III";
                                            if (subjectcode_Part3 == "")
                                            {
                                                subjectcode_Part3 = printds_rows.Tables[0].Rows[i]["subject_no"].ToString();
                                            }
                                            else
                                            {
                                                subjectcode_Part3 = subjectcode_Part3 + "'" + "," + "'" + printds_rows.Tables[0].Rows[i]["subject_no"].ToString();
                                            }
                                        }
                                        else if (roman == "4")
                                        {
                                            roman = "IV";
                                            if (subjectcode_Part4 == "")
                                            {
                                                subjectcode_Part4 = printds_rows.Tables[0].Rows[i]["subject_no"].ToString();
                                            }
                                            else
                                            {
                                                subjectcode_Part4 = subjectcode_Part4 + "'" + "," + "'" + printds_rows.Tables[0].Rows[i]["subject_no"].ToString();
                                            }
                                        }
                                    }

                                    PdfTextArea pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 38, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["semester"].ToString());//17
                                    mypdfpage.Add(pdfdegree);

                                    pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 54, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, roman);//17
                                    mypdfpage.Add(pdfdegree);

                                    pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 75, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["subject_code"].ToString());
                                    mypdfpage.Add(pdfdegree);

                                    pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 130, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["subject_name"].ToString());
                                    mypdfpage.Add(pdfdegree);
                                    pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 363, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["maxtotal"].ToString());
                                    mypdfpage.Add(pdfdegree);

                                    pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 392, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["internal_mark"].ToString());
                                    mypdfpage.Add(pdfdegree);

                                    pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 418, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["external_mark"].ToString());
                                    mypdfpage.Add(pdfdegree);
                                    pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 442, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["total"].ToString());
                                    mypdfpage.Add(pdfdegree);
                                    //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 471, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["max_int_marks"].ToString());
                                    //mypdfpage.Add(pdfdegree);
                                    //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 478, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["max_ext_marks"].ToString());
                                    //mypdfpage.Add(pdfdegree);
                                    double checkmarkmm = Convert.ToDouble(printds_rows.Tables[0].Rows[i]["total"].ToString());
                                    string gradesql = "    select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='" + sem + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 16/aug 2014
                                    gradeds.Clear();
                                    gradeds = da.select_method_wo_parameter(gradesql, "Text");
                                    if (gradeds.Tables[0].Rows.Count == 0)
                                    {
                                        gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 
                                        gradeds.Clear();
                                        gradeds = da.select_method_wo_parameter(gradesql, "Text");
                                    }
                                    for (int grd = 0; grd < gradeds.Tables[0].Rows.Count; grd++)
                                    {
                                        if (Convert.ToInt32(gradeds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(gradeds.Tables[0].Rows[grd][2]) >= checkmarkmm)
                                        {
                                            grade = gradeds.Tables[0].Rows[grd][0].ToString();
                                            gradepoints = gradeds.Tables[0].Rows[grd]["Credit_Points"].ToString();
                                        }

                                    }
                                    double gradeibtpoint = Convert.ToDouble(printds_rows.Tables[0].Rows[i]["total"].ToString());
                                    gradeibtpoint = gradeibtpoint / 10;
                                    gradeibtpoint = Math.Round(gradeibtpoint, 2);
                                    gradepoints = Convert.ToString(gradeibtpoint);
                                    pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 471, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["credit_points"].ToString());
                                    mypdfpage.Add(pdfdegree);
                                    pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 480, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, gradepoints);
                                    mypdfpage.Add(pdfdegree);

                                    string result = printds_rows.Tables[0].Rows[i]["result"].ToString().ToLower();
                                    if (result == "fail")
                                    {
                                        result = "RA";
                                        grade = "U";
                                    }
                                    else if (result == "pass")
                                    {
                                        result = "P";
                                    }
                                    else
                                    {
                                        result = "AAA";
                                        grade = "U";
                                    }
                                    pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 500, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, grade);
                                    mypdfpage.Add(pdfdegree);

                                    string examccodemy = printds_rows.Tables[0].Rows[i]["monthyear"].ToString();
                                    string[] splitexamccodemy = examccodemy.Split('-');
                                    examccodemy = ddlMonth.Items[Convert.ToInt32(splitexamccodemy[0].ToString())].Text.ToString();
                                    examccodemy = examccodemy + " " + splitexamccodemy[1].ToString();
                                    pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 530, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, examccodemy);
                                    mypdfpage.Add(pdfdegree);
                                    colval = colval + 10;



                                }




                            }

                            //}
                            PdfTextArea pdfdegree123 = new PdfTextArea(Fontpala12, System.Drawing.Color.Black, new PdfArea(mydoc, 223, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, "****** End of Statement ******");
                            mypdfpage.Add(pdfdegree123);

                            pdfdegree123 = new PdfTextArea(Fontarial9, System.Drawing.Color.Black, new PdfArea(mydoc, 263, 940, 305, 50), System.Drawing.ContentAlignment.TopLeft, principal);
                            mypdfpage.Add(pdfdegree123);

                            Gios.Pdf.PdfTable table1forpagecoe = mydoc.NewTable(Fontarial9, 1, 2, 1);
                            table1forpagecoe.VisibleHeaders = false;
                            table1forpagecoe.SetBorders(Color.Black, 1, BorderType.None);
                            table1forpagecoe.SetColumnsWidth(new int[] { 184 });
                            table1forpagecoe.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            //table1forpagecoe.Cell(0, 0).SetContent("D");
                            //table1forpagecoe.Cell(0, 0).SetFont(Fontarial12);
                            //table1forpagecoe.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            //table1forpagecoe.Cell(0, 0).SetContent("R.");
                            //table1forpagecoe.Cell(0, 0).SetFont(Fontarial10);
                            table1forpagecoe.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpagecoe.Cell(0, 0).SetContent(coe);
                            Gios.Pdf.PdfTablePage newpdftabpagecoe = table1forpagecoe.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 440, 940, 184, 50));
                            mypdfpage.Add(newpdftabpagecoe);


                            table1forpagecoe = mydoc.NewTable(Fontarial9, 1, 1, 1);
                            table1forpagecoe.VisibleHeaders = false;
                            table1forpagecoe.SetBorders(Color.Black, 1, BorderType.None);
                            table1forpagecoe.SetColumnsWidth(new int[] { 10 });
                            table1forpagecoe.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpagecoe.Cell(0, 0).SetContent("D");
                            table1forpagecoe.Cell(0, 0).SetFont(Fontarial12);

                            newpdftabpagecoe = table1forpagecoe.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 420, 938, 10, 50));
                            mypdfpage.Add(newpdftabpagecoe);

                            table1forpagecoe = mydoc.NewTable(Fontarial9, 1, 1, 1);
                            table1forpagecoe.VisibleHeaders = false;
                            table1forpagecoe.SetBorders(Color.Black, 1, BorderType.None);
                            table1forpagecoe.SetColumnsWidth(new int[] { 10 });
                            table1forpagecoe.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpagecoe.Cell(0, 0).SetContent("R.");


                            newpdftabpagecoe = table1forpagecoe.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 430, 940, 10, 50));
                            mypdfpage.Add(newpdftabpagecoe);

                            string year = ddlYear.SelectedItem.Text;
                            string collcode = Session["collegecode"].ToString();

                            Gios.Pdf.PdfTable table1forpagegpa = mydoc.NewTable(Fontco12a, 1, 4, 1);
                            table1forpagegpa.VisibleHeaders = false;
                            table1forpagegpa.SetBorders(Color.Black, 1, BorderType.None);
                            table1forpagegpa.SetColumnsWidth(new int[] { 94, 94, 94, 94 });
                            table1forpagegpa.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);

                            //if (subjectcode_Part1.Trim() != "" && subjectcode_Part1.Trim() != null)
                            //{
                            //    cal_gpa = Calulat_GPA_forpart(rollnosub, degree_code, batch_year, monthstr, year, collcode, subjectcode_Part1);
                            //    table1forpagegpa.Cell(0, 0).SetContent(cal_gpa);
                            //}
                            //else
                            //{
                            //    table1forpagegpa.Cell(0, 0).SetContent("0.00");
                            //}

                            //table1forpagegpa.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                            //if (subjectcode_Part2.Trim() != "" && subjectcode_Part2.Trim() != null)
                            //{
                            //    cal_gpa = Calulat_GPA_forpart(rollnosub, degree_code, batch_year, monthstr, year, collcode, subjectcode_Part2);
                            //    table1forpagegpa.Cell(0, 1).SetContent(cal_gpa);
                            //}
                            //else
                            //{
                            //    table1forpagegpa.Cell(0, 1).SetContent("0.00");
                            //}

                            //if (subjectcode_Part3.Trim() != "" && subjectcode_Part3.Trim() != null)
                            //{
                            //    table1forpagegpa.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);

                            //    cal_gpa = Calulat_GPA_forpart(rollnosub, degree_code, batch_year, monthstr, year, collcode, subjectcode_Part3);
                            //    table1forpagegpa.Cell(0, 2).SetContent(cal_gpa);
                            //}
                            //else
                            //{
                            //    table1forpagegpa.Cell(0, 2).SetContent("0.00");
                            //}

                            //if (subjectcode_Part4.Trim() != "" && subjectcode_Part4.Trim() != null)
                            //{
                            //    table1forpagegpa.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);

                            //    cal_gpa = Calulat_GPA_forpart(rollnosub, degree_code, batch_year, monthstr, year, collcode, subjectcode_Part4);
                            //    table1forpagegpa.Cell(0, 3).SetContent(cal_gpa);
                            //}
                            //else
                            //{
                            //    table1forpagegpa.Cell(0, 3).SetContent("0.00");
                            //}
                            double partsums = 0.00;
                            if (printds_rows.Tables[0].Rows.Count > 0)
                            {
                                string sumpart = "";
                                DataView dv_demand_data = new DataView();
                                printds_rows.Tables[0].DefaultView.RowFilter = "Subject_Type='Part I' and result='pass'";
                                dv_demand_data = printds_rows.Tables[0].DefaultView;
                                if (dv_demand_data.Count > 0)
                                {
                                    for (int sum = 0; sum < dv_demand_data.Count; sum++)
                                    {
                                        partsums = partsums + Convert.ToDouble(dv_demand_data[sum]["total"].ToString());
                                    }
                                    partsums = (partsums / dv_demand_data.Count) / 10;
                                    partsums = Math.Round(partsums, 2);
                                    sumpart = String.Format("{0:0.00}", partsums);
                                }
                                else
                                {
                                    sumpart = "-";
                                }
                                //cal_gpa = Calulat_GPA_forpart(rollnosub, degree_code, batch_year, monthstr, year, collcode, subjectcode_Part1);

                                table1forpagegpa.Cell(0, 0).SetContent(sumpart);
                            }
                            else
                            {
                                table1forpagegpa.Cell(0, 0).SetContent("-");
                            }
                            partsums = 0;
                            table1forpagegpa.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                            if (printds_rows.Tables[0].Rows.Count > 0)
                            {
                                string sumpart = "";
                                DataView dv_demand_data = new DataView();
                                printds_rows.Tables[0].DefaultView.RowFilter = "subject_type='Part II' and result='pass'";
                                dv_demand_data = printds_rows.Tables[0].DefaultView;
                                if (dv_demand_data.Count > 0)
                                {
                                    for (int sum = 0; sum < dv_demand_data.Count; sum++)
                                    {
                                        partsums = partsums + Convert.ToDouble(dv_demand_data[sum]["total"].ToString());
                                    }
                                    partsums = (partsums / dv_demand_data.Count) / 10;
                                    partsums = Math.Round(partsums, 2);
                                    sumpart = String.Format("{0:0.00}", partsums);
                                }
                                else
                                {
                                    sumpart = "-";
                                }
                                //cal_gpa = Calulat_GPA_forpart(rollnosub, degree_code, batch_year, monthstr, year, collcode, subjectcode_Part1);

                                table1forpagegpa.Cell(0, 1).SetContent(sumpart);
                            }
                            else
                            {
                                table1forpagegpa.Cell(0, 1).SetContent("-");
                            }
                            partsums = 0;
                            if (printds_rows.Tables[0].Rows.Count > 0)
                            {
                                string sumpart = "";
                                DataView dv_demand_data = new DataView();
                                printds_rows.Tables[0].DefaultView.RowFilter = "subject_type='Part III' and result='pass'";
                                dv_demand_data = printds_rows.Tables[0].DefaultView;
                                if (dv_demand_data.Count > 0)
                                {
                                    for (int sum = 0; sum < dv_demand_data.Count; sum++)
                                    {
                                        partsums = partsums + Convert.ToDouble(dv_demand_data[sum]["total"].ToString());
                                    }
                                    partsums = (partsums / dv_demand_data.Count) / 10;
                                    partsums = Math.Round(partsums, 2);
                                    sumpart = String.Format("{0:0.00}", partsums);
                                }
                                else
                                {
                                    sumpart = "-";
                                }
                                //cal_gpa = Calulat_GPA_forpart(rollnosub, degree_code, batch_year, monthstr, year, collcode, subjectcode_Part1);

                                table1forpagegpa.Cell(0, 2).SetContent(sumpart);
                            }
                            else
                            {
                                table1forpagegpa.Cell(0, 2).SetContent("-");
                            }
                            partsums = 0;
                            if (printds_rows.Tables[0].Rows.Count > 0)
                            {
                                string sumpart = "";
                                DataView dv_demand_data = new DataView();
                                printds_rows.Tables[0].DefaultView.RowFilter = "subject_type='Part IV' and result='pass'";
                                dv_demand_data = printds_rows.Tables[0].DefaultView;
                                if (dv_demand_data.Count > 0)
                                {
                                    for (int sum = 0; sum < dv_demand_data.Count; sum++)
                                    {
                                        partsums = partsums + Convert.ToDouble(dv_demand_data[sum]["total"].ToString());
                                    }
                                    partsums = (partsums / dv_demand_data.Count) / 10;
                                    partsums = Math.Round(partsums, 2);
                                    sumpart = String.Format("{0:0.00}", partsums);
                                }
                                else
                                {
                                    sumpart = "-";
                                }
                                //cal_gpa = Calulat_GPA_forpart(rollnosub, degree_code, batch_year, monthstr, year, collcode, subjectcode_Part1);

                                table1forpagegpa.Cell(0, 3).SetContent(sumpart);
                            }
                            else
                            {
                                table1forpagegpa.Cell(0, 3).SetContent("-");
                            }

                            newpdftabpagecoe = table1forpagegpa.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 162, 890, 376, 50));

                            mypdfpage.Add(newpdftabpagecoe);


                            mypdfpage.SaveToDocument();
                        }

                    }

                }

            }

            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "marksheet" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";


                mydoc.SaveToFile(szPath + szFile);

                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
        }
        catch
        {
        }
    }

    public void printbtn_Clickfrmt3()
    {
        try
        {
            DataSet printds = new DataSet();

            DataSet printds_new = new DataSet();
            DataSet printds_rows = new DataSet();
            string mode = "";

            string degree = "";
            string monthandyear = "";
            string studname = "";
            string dob = "";
            string rollnosub = "";
            string regnumber = "";
            string batch_year = "";
            string degree_code = "";
            string exam_code = "";
            string sem = "";
            //int max_internal_mark = 0;
            //int max_external_mark = 0;
            int colval = 0;
            string branch = "";
            int month = 0;
            string monthstr = "";
            string sql2 = "";
            string sql3 = "";
            string roman = "";
            string semroman = "";
            string grade = "";
            string gradepoints = "";
            string coe = "";
            string subjectcode_Part1 = "";
            string subjectcode_Part2 = "";
            string subjectcode_Part3 = "";
            string subjectcode_Part4 = "";
            string cal_gpa = "";
            string principal = "";

            string subtype = "";
            DataSet gradeds = new DataSet();

            Font fontcal11 = new Font("Calibri (Body)", 11, FontStyle.Bold);
            Font fontcal14 = new Font("Calibri (Body)", 14, FontStyle.Bold);
            Font fontcal8 = new Font("Calibri (Body)", 8, FontStyle.Regular);
            Font fontcal8b = new Font("Calibri (Body)", 10, FontStyle.Bold);
            Font fontcal7 = new Font("Calibri (Body)", 7, FontStyle.Italic);
            // Font Fontco12 = new Font("Calibri (Body)", 11, FontStyle.Bold);
            //Font Fontpala12 = new Font("Palatino Linotype", 10, FontStyle.Bold);
            //Font Fontco10 = new Font("Comic Sans MS", 10, FontStyle.Regular);
            //Font Fontco12a = new Font("Comic Sans MS", 12, FontStyle.Regular);
            //Font Fontarial7 = new Font("Arial", 7, FontStyle.Regular);
            //Font Fontarial9 = new Font("Arial", 9, FontStyle.Regular);

            //Font Fontarial10 = new Font("Arial", 10, FontStyle.Regular);
            //Font Fontarial12 = new Font("Arial", 12, FontStyle.Regular);
            //Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            //Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);

            // Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            //  Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InInches(8.5, 14));
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage;
            if ((ddlMonth.SelectedValue.ToString() != "0") && (ddlYear.SelectedValue.ToString() != "0"))
            {

                FpSpread2.SaveChanges();
                for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
                {
                    int isval = 0;

                    string s = FpSpread2.Sheets[0].Cells[res, 6].Text;
                    isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);

                    if (isval == 1)
                    {
                        printds.Clear();
                        printds.Dispose();
                        printds_new.Clear();
                        printds_new.Dispose();

                        rollnosub = FpSpread2.Sheets[0].Cells[res, 1].Note;
                        regnumber = FpSpread2.Sheets[0].Cells[res, 2].Text;
                        //string sql = "SELECT Reg_No,r.batch_year,r.app_no,r.degree_code,R.current_semester,R.Stud_Name,Course_Name+'('+Dept_acronym+')' as degree,Dept_name,CONVERT(VARCHAR, dob, 103) as dob FROM Registration R,Applyn A,Degree G,Course C,Department D WHERE R.App_No = A.App_No AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND D.College_Code = G.College_Code AND Roll_No = '" + rollnosub + "' and r.college_code='" + Session["collegecode"].ToString() + "' ;  select coe from collinfo where college_code='" + Session["collegecode"].ToString() + "';";
                        string sql = "SELECT Reg_No,r.batch_year,r.app_no,r.degree_code,R.current_semester,R.Stud_Name,Course_Name as degree,Dept_name,CONVERT(VARCHAR, dob, 103) as dob,r.mode FROM Registration R,Applyn A,Degree G,Course C,Department D WHERE R.App_No = A.App_No AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND D.College_Code = G.College_Code AND Roll_No = '" + rollnosub + "' and r.college_code='" + Session["collegecode"].ToString() + "' ;  select coe,principal from collinfo where college_code='" + Session["collegecode"].ToString() + "';";

                        //dummy
                        // sql = "SELECT Reg_No,r.batch_year,r.degree_code,R.current_semester,R.Stud_Name,Course_Name+'('+Dept_acronym+')' as degree,Dept_name,CONVERT(VARCHAR, dob, 103) as dob FROM Registration R,Applyn A,Degree G,Course C,Department D WHERE R.App_No = A.App_No AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND D.College_Code = G.College_Code AND Roll_No = '14UHI6001' ;  select coe from collinfo";

                        printds = da.select_method_wo_parameter(sql, "Text");
                        if (printds.Tables[0].Rows.Count > 0)
                        {
                            mypdfpage = mydoc.NewPage();
                            degree = printds.Tables[0].Rows[0]["degree"].ToString();
                            coe = printds.Tables[1].Rows[0]["coe"].ToString();
                            principal = printds.Tables[1].Rows[0]["principal"].ToString();
                            string[] spiltcoe = coe.Split('.');
                            string newcoe = "";
                            for (int ce = 1; ce <= spiltcoe.GetUpperBound(0); ce++)
                            {
                                if (newcoe == "")
                                {
                                    newcoe = spiltcoe[ce].ToString();
                                }
                                else
                                {
                                    newcoe = newcoe + "." + spiltcoe[ce].ToString();
                                }
                            }
                            coe = newcoe;
                            month = ddlMonth.SelectedIndex;
                            monthstr = ddlMonth.SelectedIndex.ToString();
                            // string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
                            string strMonthName = ddlMonth.SelectedItem.Text.Trim();
                            monthandyear = strMonthName + "  " + ddlYear.SelectedItem.Text;
                            monthandyear = monthandyear.ToUpper();
                            studname = printds.Tables[0].Rows[0]["Stud_Name"].ToString();
                            branch = printds.Tables[0].Rows[0]["Dept_name"].ToString();
                            dob = printds.Tables[0].Rows[0]["dob"].ToString();
                            mode = printds.Tables[0].Rows[0]["mode"].ToString();
                            string stdappno = printds.Tables[0].Rows[0]["App_No"].ToString();
                            batch_year = printds.Tables[0].Rows[0]["batch_year"].ToString();
                            degree_code = printds.Tables[0].Rows[0]["degree_code"].ToString();
                            sem = printds.Tables[0].Rows[0]["current_semester"].ToString();

                            if (sem == "1")
                            {
                                semroman = "I";
                            }
                            else if (sem == "2")
                            {
                                semroman = "II";
                            }
                            else if (sem == "3")
                            {
                                semroman = "III";
                            }
                            else if (sem == "4")
                            {
                                semroman = "IV";
                            }
                            else if (sem == "5")
                            {
                                semroman = "V";
                            }
                            else if (sem == "6")
                            {
                                semroman = "VI";
                            }
                            else if (sem == "7")
                            {
                                semroman = "VII";
                            }
                            else if (sem == "8")
                            {
                                semroman = "VIII";
                            }

                            //sql2 = "select * from exam_details where  batch_year='" + batch_year + "' and   degree_code='" + degree_code + "' and current_semester='" + sem + "' and exam_month='" + month + "' and exam_year='" + ddlYear.SelectedItem.Text + "'";
                            //dummy
                            // sql2 = "select * from exam_details where  batch_year='2014' and   degree_code='45' and current_semester='1' and exam_month='11' and exam_year='2014'";

                            //printds_new = da.select_method_wo_parameter(sql2, "Text");
                            //if (printds_new.Tables[0].Rows.Count > 0)
                            //{


                            string stdphtsql = "select * from StdPhoto where app_no='" + stdappno + "'";
                            MemoryStream memoryStream = new MemoryStream();
                            DataSet dsstdpho = new DataSet();
                            dsstdpho.Clear();
                            dsstdpho.Dispose();
                            dsstdpho = da.select_method_wo_parameter(stdphtsql, "Text");
                            if (dsstdpho.Tables[0].Rows.Count > 0)
                            {
                                byte[] file = (byte[])dsstdpho.Tables[0].Rows[0][1];
                                memoryStream.Write(file, 0, file.Length);
                                if (file.Length > 0)
                                {
                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg")))
                                    {
                                        //image3.ImageUrl = "~/college/" + stdappno + ".jpeg";
                                    }
                                    else
                                    {
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        //image3.ImageUrl = "~/college/" + stdappno + ".jpeg";
                                    }



                                }

                            }

                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg")))
                            {
                                //Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg"));
                                //mypdfpage.Add(LogoImage2, 25, 895, 450);
                            }
                            else
                            {
                                //Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg"));
                                //mypdfpage.Add(LogoImage2, 25, 895, 270);
                            }
                            //PdfTextArea pdfdoi = new PdfTextArea(fontcal11, System.Drawing.Color.Black, new PdfArea(mydoc, 30, 880, 305, 50), System.Drawing.ContentAlignment.TopLeft, txtdoi.Text.ToString());
                            //mypdfpage.Add(pdfdoi);
                            //  exam_code = printds_new.Tables[0].Rows[0]["exam_code"].ToString();

                            // sql3 = "Select Subject_Type,subject_name,subject_code,subject.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,maxtotal,grade,cp,credit_points,mark_entry.subject_no,semester,maxtotal,exam_code from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = '" + exam_code + "' and roll_no='" + rollnosub + "'  order by LEN(subject_type),subject_type";
                            sql3 = "Select semester,Subject_Type,subject_name,subject_code,s.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,maxtotal,grade,cp,credit_points,m.subject_no,semester,maxtotal,m.exam_code, convert(varchar(20),exam_month)+'-'+convert(varchar(20),exam_year)  as monthyear from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sy,exam_details ed where sy.syll_code=s.syll_code and ss.syll_code=sy.syll_code and s.syll_code=ss.syll_code and s.subtype_no= ss.subtype_no and m.Subject_No = s.Subject_No and upper(result) ='PASS' and  m.exam_code = ed.exam_code  and roll_no='" + rollnosub + "' order by sy.semester,subject_code, LEN(subject_type),subject_type";
                            //dummy
                            // sql3 = "Select Subject_Type,subject_name,subject_code,subject.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,maxtotal,grade,cp,credit_points,mark_entry.subject_no,semester,maxtotal,exam_code from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = 10146 and roll_no='14UHI6001'  order by LEN(subject_type),subject_type";
                            printds_rows.Clear();
                            printds_rows.Dispose();
                            printds_rows = da.select_method_wo_parameter(sql3, "Text");

                            string strexam = degree + " (" + branch + ") " + "Degree Examinations" + " " + monthandyear;
                            PdfTextArea pdfexam = new PdfTextArea(fontcal8b, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 30, 612, 50), System.Drawing.ContentAlignment.MiddleCenter, strexam);//17
                            mypdfpage.Add(pdfexam);
                            //Gios.Pdf.PdfTable table1forpage1 = mydoc.NewTable(Fontco12, 1, 3, 1);

                            //table1forpage1.VisibleHeaders = false;
                            //table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                            //table1forpage1.SetColumnsWidth(new int[] { 91, 147, 99 });
                            //table1forpage1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage1.Cell(0, 0).SetContent(degree);
                            //table1forpage1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage1.Cell(0, 1).SetContent("");
                            //table1forpage1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage1.Cell(0, 2).SetContent(monthandyear);
                            //Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 130, 52, 336, 50));

                            //mypdfpage.Add(newpdftabpage2);

                            //Gios.Pdf.PdfTable table1forpage2 = mydoc.NewTable(fontcal11, 1, 3, 1);

                            //table1forpage2.VisibleHeaders = false;
                            //table1forpage2.SetBorders(Color.Black, 1, BorderType.None);
                            //table1forpage2.SetColumnsWidth(new int[] { 354, 108, 108 });
                            //table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage2.Cell(0, 0).SetContent(studname);
                            //table1forpage2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage2.Cell(0, 1).SetContent(dob);
                            //table1forpage2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage2.Cell(0, 2).SetContent(regnumber);
                            //Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 27, 106, 567, 50));//sr

                            //mypdfpage.Add(newpdftabpage2);

                            //Gios.Pdf.PdfTable table1forpage2a = mydoc.NewTable(fontcal11, 1, 3, 1);
                            //table1forpage2a.Columns[0].SetWidth(354);
                            //table1forpage2a.VisibleHeaders = false;
                            //table1forpage2a.SetBorders(Color.Black, 1, BorderType.None);
                            //table1forpage2a.SetColumnsWidth(new int[] { 164, 298, 108 });
                            //table1forpage2a.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage2a.Cell(0, 0).SetContent(degree);
                            //table1forpage2a.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage2a.Cell(0, 1).SetContent(branch);
                            //table1forpage2a.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage2a.Cell(0, 2).SetContent(batch_year + "-" + System.DateTime.Now.ToString("yyyy"));
                            ////table1forpage2a.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            ////table1forpage2a.Cell(0, 3).SetContent(txtdop.Text);
                            //newpdftabpage2 = table1forpage2a.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 27, 143, 567, 50));

                            //mypdfpage.Add(newpdftabpage2);

                            Gios.Pdf.PdfTable table1forpage2 = mydoc.NewTable(fontcal11, 3, 1, 3);
                            table1forpage2.VisibleHeaders = false;
                            table1forpage2.SetBorders(Color.Black, 1, BorderType.None);
                            table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(0, 0).SetContent(studname);
                            table1forpage2.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(1, 0).SetContent(regnumber);
                            table1forpage2.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(2, 0).SetContent(dob);
                            //table1forpage2.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            //table1forpage2.Cell(3, 0).SetContent(branch);
                            //table1forpage2.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            //table1forpage2.Cell(4, 0).SetContent(generate);

                            Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 213, 88, 567, 150));//sr
                            mypdfpage.Add(newpdftabpage2);

                            colval = 200;
                            ArrayList arrsemester = new ArrayList();
                            arrsemester.Clear();
                            ArrayList arrtable1rowspan = new ArrayList();
                            arrtable1rowspan.Clear();
                            ArrayList arrtable2rowspan = new ArrayList();
                            arrtable2rowspan.Clear();
                            DataTable dttable1 = new DataTable();

                            dttable1.Columns.Add("1");
                            dttable1.Columns.Add("2");
                            dttable1.Columns.Add("3");
                            dttable1.Columns.Add("4");
                            dttable1.Columns.Add("5");

                            DataTable dttable2 = new DataTable();

                            dttable2.Columns.Add("1");
                            dttable2.Columns.Add("2");
                            dttable2.Columns.Add("3");
                            dttable2.Columns.Add("4");
                            dttable2.Columns.Add("5");

                            string chksemesters = "";
                            int tablerowsextra1 = 0;

                            DataView dv_data = new DataView();
                            DataRow dr = null;
                            int tb1inc = 0;
                            int tb2inc = 0;
                            for (int i = 0; i < printds_rows.Tables[0].Rows.Count; i++)
                            {
                                if (!arrsemester.Contains(printds_rows.Tables[0].Rows[i]["semester"].ToString()))
                                {
                                    if (arrsemester.Count < 4)
                                    {
                                        arrsemester.Add(printds_rows.Tables[0].Rows[i]["semester"].ToString());

                                        dr = dttable1.NewRow();
                                        dr[1] = "Semester-" + printds_rows.Tables[0].Rows[i]["semester"].ToString();
                                        dttable1.Rows.Add(dr);
                                        arrtable1rowspan.Add(tb1inc);
                                        tb1inc++;

                                        printds_rows.Tables[0].DefaultView.RowFilter = "semester in ('" + printds_rows.Tables[0].Rows[i]["semester"].ToString() + "')";
                                        dv_data = printds_rows.Tables[0].DefaultView;
                                        tablerowsextra1 = dv_data.Count;

                                        for (int sj = 0; sj < tablerowsextra1; sj++)
                                        {


                                            dr = dttable1.NewRow();

                                            dr[0] = printds_rows.Tables[0].Rows[i]["subject_code"].ToString();

                                            dr[1] = printds_rows.Tables[0].Rows[i]["subject_name"].ToString();

                                            dr[2] = printds_rows.Tables[0].Rows[i]["credit_points"].ToString();

                                            dr[3] = printds_rows.Tables[0].Rows[i]["grade"].ToString();

                                            string examccodemy = printds_rows.Tables[0].Rows[i]["monthyear"].ToString();
                                            string[] splitexamccodemy = examccodemy.Split('-');
                                            examccodemy = ddlMonth.Items[Convert.ToInt32(splitexamccodemy[0].ToString())].Text.ToString();
                                            examccodemy = examccodemy + " " + splitexamccodemy[1].ToString();
                                            dr[4] = examccodemy;

                                            dttable1.Rows.Add(dr);
                                            tb1inc++;
                                        }
                                        dr = dttable1.NewRow();
                                        dr[0] = " ";
                                        dttable1.Rows.Add(dr);
                                        tb1inc++;
                                        dr = dttable1.NewRow();
                                        dr[0] = " ";
                                        dttable1.Rows.Add(dr);
                                        tb1inc++;
                                        dr = dttable1.NewRow();
                                        dr[0] = " ";
                                        dttable1.Rows.Add(dr);
                                        tb1inc++;
                                        dr = dttable1.NewRow();
                                        dr[0] = " ";
                                        dttable1.Rows.Add(dr);
                                        tb1inc++;
                                    }
                                    else
                                    {
                                        arrsemester.Add(printds_rows.Tables[0].Rows[i]["semester"].ToString());

                                        dr = dttable2.NewRow();
                                        dr[1] = "Semester-" + printds_rows.Tables[0].Rows[i]["semester"].ToString();
                                        dttable2.Rows.Add(dr);

                                        arrtable2rowspan.Add(tb2inc);
                                        tb2inc++;
                                        printds_rows.Tables[0].DefaultView.RowFilter = "semester in ('" + printds_rows.Tables[0].Rows[i]["semester"].ToString() + "')";
                                        dv_data = printds_rows.Tables[0].DefaultView;
                                        tablerowsextra1 = dv_data.Count;

                                        for (int sj = 0; sj < tablerowsextra1; sj++)
                                        {

                                            dr = dttable2.NewRow();

                                            dr[0] = printds_rows.Tables[0].Rows[i]["subject_code"].ToString();

                                            dr[1] = printds_rows.Tables[0].Rows[i]["subject_name"].ToString();

                                            dr[2] = printds_rows.Tables[0].Rows[i]["credit_points"].ToString();

                                            dr[3] = printds_rows.Tables[0].Rows[i]["grade"].ToString();

                                            string examccodemy = printds_rows.Tables[0].Rows[i]["monthyear"].ToString();
                                            string[] splitexamccodemy = examccodemy.Split('-');
                                            examccodemy = ddlMonth.Items[Convert.ToInt32(splitexamccodemy[0].ToString())].Text.ToString();
                                            examccodemy = examccodemy + " " + splitexamccodemy[1].ToString();
                                            dr[4] = examccodemy;
                                            dttable2.Rows.Add(dr);
                                            tb2inc++;

                                        }
                                        dr = dttable2.NewRow();
                                        dr[0] = " ";
                                        dttable2.Rows.Add(dr);
                                        tb2inc++;

                                        dr = dttable2.NewRow();
                                        dr[0] = " ";
                                        dttable2.Rows.Add(dr);
                                        tb2inc++;
                                        dr = dttable2.NewRow();
                                        dr[0] = " ";
                                        dttable2.Rows.Add(dr);
                                        tb2inc++;
                                        dr = dttable2.NewRow();
                                        dr[0] = " ";
                                        dttable2.Rows.Add(dr);
                                        tb2inc++;
                                    }


                                }
                            }

                            dr = dttable2.NewRow();
                            dr[1] = "Value Added Course";
                            dttable2.Rows.Add(dr);
                            tb2inc++;
                            arrtable2rowspan.Add(tb2inc);

                            dr = dttable2.NewRow();
                            dr[1] = "(Not Considered for CGPA Calculation)";
                            dttable2.Rows.Add(dr);
                            tb2inc++;
                            arrtable2rowspan.Add(tb2inc);

                            dr = dttable2.NewRow();
                            dr[1] = "Placement and Training";
                            dttable2.Rows.Add(dr);
                            tb2inc++;
                            arrtable2rowspan.Add(tb2inc);

                            dr = dttable2.NewRow();
                            dr[1] = "DCS & PLS Overview";
                            dttable2.Rows.Add(dr);
                            tb2inc++;
                            arrtable2rowspan.Add(tb2inc);

                            if (dttable1.Rows.Count > 0)
                            {

                                Gios.Pdf.PdfTable pdftable1 = mydoc.NewTable(fontcal8, dttable1.Rows.Count, 5, 1);
                                pdftable1.VisibleHeaders = false;
                                pdftable1.SetBorders(Color.Black, 1, BorderType.None);
                                pdftable1.SetColumnsWidth(new int[] { 57, 142, 28, 28, 40 });
                                pdftable1.Columns[1].SetContentAlignment(ContentAlignment.MiddleLeft);
                                // pdftable1.ImportDataTable(dttable1);
                                for (int i = 0; i < dttable1.Rows.Count; i++)
                                {
                                    pdftable1.Cell(i, 0).SetContent(dttable1.Rows[i][0].ToString());
                                    pdftable1.Cell(i, 1).SetContent(dttable1.Rows[i][1].ToString());
                                    pdftable1.Cell(i, 2).SetContent(dttable1.Rows[i][2].ToString());
                                    pdftable1.Cell(i, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdftable1.Cell(i, 3).SetContent(dttable1.Rows[i][3].ToString());
                                    pdftable1.Cell(i, 4).SetContent(dttable1.Rows[i][4].ToString());
                                }


                                for (int tb1 = 0; tb1 < arrtable1rowspan.Count; tb1++)
                                {
                                    pdftable1.Cell(Convert.ToInt32(arrtable1rowspan[tb1]), 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdftable1.Cell(Convert.ToInt32(arrtable1rowspan[tb1]), 1).SetFont(fontcal11);

                                }

                                newpdftabpage2 = pdftable1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 23, 143, 280, 1000));
                                mypdfpage.Add(newpdftabpage2);

                            }

                            if (dttable2.Rows.Count > 0)
                            {

                                Gios.Pdf.PdfTable pdftable2 = mydoc.NewTable(fontcal8, dttable2.Rows.Count, 5, 1);
                                pdftable2.VisibleHeaders = false;
                                pdftable2.SetBorders(Color.Black, 1, BorderType.None);
                                // pdftable2.SetColumnsWidth(new int[] { 57, 142, 28, 28, 37 });
                                pdftable2.SetColumnsWidth(new int[] { 57, 142, 28, 28, 40 });
                                // pdftable2.Columns[2].SetContentAlignment(ContentAlignment.MiddleLeft);
                                //pdftable2.ImportDataTable(dttable2);

                                for (int i = 0; i < dttable2.Rows.Count; i++)
                                {
                                    pdftable2.Cell(i, 0).SetContent(dttable2.Rows[i][0].ToString());
                                    pdftable2.Cell(i, 1).SetContent(dttable2.Rows[i][1].ToString());
                                    pdftable2.Cell(i, 2).SetContent(dttable2.Rows[i][2].ToString());
                                    pdftable2.Cell(i, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdftable2.Cell(i, 3).SetContent(dttable2.Rows[i][3].ToString());
                                    pdftable2.Cell(i, 4).SetContent(dttable2.Rows[i][4].ToString());
                                }

                                for (int tb1 = 0; tb1 < arrtable2rowspan.Count - 4; tb1++)
                                {
                                    pdftable2.Cell(Convert.ToInt32(arrtable2rowspan[tb1]), 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdftable2.Cell(Convert.ToInt32(arrtable2rowspan[tb1]), 1).SetFont(fontcal11);

                                }
                                pdftable2.Cell(dttable2.Rows.Count - 4, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                pdftable2.Cell(dttable2.Rows.Count - 4, 1).SetFont(fontcal11);
                                pdftable2.Cell(dttable2.Rows.Count - 4, 1).SetCellPadding(10);
                                // pdftable2.Cell(dttable2.Rows.Count - 3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                pdftable2.Cell(dttable2.Rows.Count - 3, 1).SetFont(fontcal7);


                                //pdftable2.Cell(arrtable2rowspan.Count - 3, 1).SetFont(fontcal8);

                                newpdftabpage2 = pdftable2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 292, 143, 280, 1000));
                                mypdfpage.Add(newpdftabpage2);
                            }

                            string year = ddlYear.SelectedItem.Text;
                            string collcode = Session["collegecode"].ToString();

                            Gios.Pdf.PdfTable table1forpagegpa = mydoc.NewTable(fontcal11, 1, 3, 1);
                            table1forpagegpa.VisibleHeaders = false;
                            table1forpagegpa.SetColumnsWidth(new int[] { 34, 111, 111 });

                            table1forpagegpa.SetBorders(Color.Black, 1, BorderType.None);
                            // string gpa = da.Calulat_GPA_Semwise(rollnosub, degree_code, batch_year, Convert.ToString(month), ddlYear.SelectedItem.Text, collcode);
                            string cgpa = Calculete_CGPA(rollnosub, sem, degree_code, batch_year, mode, collcode);
                            table1forpagegpa.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpagegpa.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpagegpa.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpagegpa.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);

                            // table1forpagegpa.Cell(0, 0).SetContent(gpa);

                            table1forpagegpa.Cell(0, 0).SetContent(cgpa);
                            table1forpagegpa.Cell(0, 1).SetContent(totcredits);
                            table1forpagegpa.Cell(0, 2).SetContent(totcredits);

                            Gios.Pdf.PdfTablePage newpdfgpa = table1forpagegpa.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 170, 680, 235, 100));
                            mypdfpage.Add(newpdfgpa);


                            mypdfpage.SaveToDocument();
                        }

                    }

                }

            }

            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "Consolidate" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";


                mydoc.SaveToFile(szPath + szFile);

                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
        }
        catch
        {
        }
    }

    public string Calculete_CGPA(string RollNo, string semval, string degree_code, string batch_year, string latmode, string collegecode)
    {
        double total = 0;
        bool flag = true;
        try
        {
            int jvalue = 0;
            string strgrade = "";
            double creditval = 0;
            double finalgpa1 = 0;
            double creditsum1 = 0;

            string strsubcrd = "";
            int gtempejval = 0;
            string syll_code = "";
            string examcodevalg = "";
            DataSet dggradetot = new DataSet();
            DataSet dssem = new DataSet();
            double strtot = 0, inte = 0, exte = 0;
            double strgradetempfrm = 0;
            double strgradetempto = 0;
            string strgradetempgrade = "";
            string strtotgrac = "";
            string sqlcmdgraderstotal = "";
            int attemptswith = 0;
            string strattmaxmark = "";
            int attmpt = 0, maxmark = 0;


            strattmaxmark = d2.GetFunctionv("select convert(varchar,attempts)+'-'+convert(varchar,maxmark) as amm from coe_attmaxmark where collegecode='" + collegecode + "'");

            string[] semecount = strattmaxmark.Split(new Char[] { '-' });

            if (semecount.GetUpperBound(0) == 1)
            {
                attmpt = Convert.ToInt32(semecount[0].ToString());
                maxmark = Convert.ToInt32(semecount[1].ToString());
                flag = true;
            }
            else
            {
                flag = false;
            }
            sqlcmdgraderstotal = " select distinct frange,trange,credit_points,mark_grade  from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + collegecode + "";
            dggradetot = d2.select_method(sqlcmdgraderstotal, hat, "Text");

            strsubcrd = " Select Subject.credit_points,Mark_Entry.internal_mark,Mark_Entry.external_mark,Mark_Entry.total,Mark_Entry.grade,Mark_Entry.attempts from Mark_Entry,Subject,Syllabus_Master where Mark_Entry.Subject_No = Subject.Subject_No and Syllabus_Master.syll_Code = Subject.syll_Code and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts>=1";
            strsubcrd = strsubcrd + " AND Exam_Code IN (SELECT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degree_code + "' AND Batch_Year =" + batch_year + " AND current_semester<=" + semval + " ) ";
            //strsubcrd = strsubcrd + " AND Exam_Code NOT IN (SELECT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degree_code + "' and batch_year=" + batch_year + " AND Exam_Year =2013 AND Exam_Month >4)";
            strsubcrd = strsubcrd + " AND Roll_No='" + RollNo + "' AND Syllabus_Master.Semester <=" + semval + " AND UPPER(Result) ='PASS' ";

            if (strsubcrd != null && strsubcrd != "")
            {
                SqlCommand cmd_subcrd = new SqlCommand(strsubcrd, con_subcrd);
                con_subcrd.Close();
                con_subcrd.Open();
                SqlDataReader dr_subcrd;
                dr_subcrd = cmd_subcrd.ExecuteReader();
                while (dr_subcrd.Read())
                {
                    if (dr_subcrd.HasRows)
                    {
                        if ((dr_subcrd["total"].ToString() != string.Empty) && (dr_subcrd["total"].ToString() != "0"))
                        {
                            if (dggradetot != null && dggradetot.Tables[0] != null && dggradetot.Tables[0].Rows.Count > 0)
                            {
                                strtot = Convert.ToDouble(dr_subcrd["total"].ToString());
                                inte = Convert.ToDouble(dr_subcrd["internal_mark"].ToString());
                                exte = Convert.ToDouble(dr_subcrd["external_mark"].ToString());
                                attemptswith = Convert.ToInt32(dr_subcrd["attempts"].ToString());
                                total = Convert.ToDouble(strtot) + Convert.ToDouble(total);

                                if (flag == true)
                                {
                                    if (attmpt > attemptswith)//ATTEMPTS compared with attempts in coe settings if attempts lower than coe settings
                                    {
                                        foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                        {
                                            if (gratemp["frange"].ToString() != null && gratemp["frange"].ToString() != "" && gratemp["trange"].ToString() != null && gratemp["trange"].ToString() != "")
                                            {
                                                strgradetempfrm = Convert.ToDouble(gratemp["frange"].ToString());
                                                strgradetempto = Convert.ToDouble(gratemp["trange"].ToString());

                                                if (strgradetempfrm <= strtot && strgradetempto >= strtot)
                                                {
                                                    strgrade = gratemp["credit_points"].ToString();
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        inte = 0;
                                        strtot = exte;// total only consider extermarks only
                                        foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                        {
                                            if (gratemp["frange"].ToString() != null && gratemp["frange"].ToString() != "" && gratemp["trange"].ToString() != null && gratemp["trange"].ToString() != "")
                                            {
                                                strgradetempfrm = Convert.ToDouble(gratemp["frange"].ToString());
                                                strgradetempto = Convert.ToDouble(gratemp["trange"].ToString());

                                                if (strgradetempfrm <= strtot && strgradetempto >= strtot)
                                                {
                                                    strgrade = gratemp["credit_points"].ToString();
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    strtot = Convert.ToDouble(dr_subcrd["total"].ToString());

                                    foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                    {
                                        if (gratemp["frange"].ToString() != null && gratemp["frange"].ToString() != "" && gratemp["trange"].ToString() != null && gratemp["trange"].ToString() != "")
                                        {
                                            strgradetempfrm = Convert.ToDouble(gratemp["frange"].ToString());
                                            strgradetempto = Convert.ToDouble(gratemp["trange"].ToString());

                                            if (strgradetempfrm <= strtot && strgradetempto >= strtot)
                                            {
                                                strgrade = gratemp["credit_points"].ToString();
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if ((dr_subcrd["grade"].ToString() != string.Empty))
                        {

                            if (dggradetot != null && dggradetot.Tables[0] != null && dggradetot.Tables[0].Rows.Count > 0)
                            {
                                strtotgrac = Convert.ToString(dr_subcrd["grade"].ToString());
                                foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                {
                                    strgradetempgrade = Convert.ToString(gratemp["mark_grade"].ToString());
                                    if (strgradetempgrade.ToString().Trim() == strtotgrac.ToString().Trim())
                                    {
                                        strgrade = gratemp["credit_points"].ToString();
                                        break;
                                    }

                                }

                            }
                        }

                        creditval = Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                        if (creditsum1 == 0)
                        {
                            creditsum1 = Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                        }
                        else
                        {
                            creditsum1 = creditsum1 + Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                        }

                        if (gpacal1 == 0)
                        {
                            if (strgrade != "")
                            {
                                gpacal1 = Convert.ToDouble(strgrade) * creditval;
                            }
                        }
                        else
                        {
                            if (strgrade != "")
                            {
                                gpacal1 = gpacal1 + (Convert.ToDouble(strgrade) * creditval);
                            }
                        }
                    }
                }
            }

            creditval = 0;
            strgrade = "";

            finalgpa1 = Math.Round((gpacal1 / creditsum1), 2, MidpointRounding.AwayFromZero);
            calculate = Convert.ToString(finalgpa1);
            totcredits = Convert.ToString(gpacal1);
            creditsum1 = 0;
            gpacal1 = 0;
            finalgpa1 = 0;
            total1 = Convert.ToDouble(total);

        }
        catch (Exception vel)
        {
            string exce = vel.ToString();

        }
        if (calculate == "NaN")
        {
            return "-";
        }
        else
        {
            return calculate;
        }

    }
    public void generatefolio()
    {
        try
        {
            //string dafd = "1005";
            //int val = 1005;
            //int gte = val++;
            //int gets = dafd.Length;
            //string fin = gte.ToString();
            //int len = gets - fin.Length;
            //string getsugg = "GST";
            //for (int i = 0; i < len; i++)
            //{
            //    getsugg = getsugg + "0";
            //}
            //getsugg = getsugg + "-" + gte.ToString();
            string folionum = da.GetFunction(" select value from Master_Settings where settings ='Consolidate Sheet'");
            string[] split = folionum.Split('-');
            folionum = split[1].ToString();
            int value = Convert.ToInt32(folionum);
            value++;
            int foliolength = folionum.Length;
            string count = value.ToString();
            int findlen = foliolength - count.Length;
            string acro = split[0].ToString();
            for (int m = 0; m < findlen; m++)
            {
                acro = acro + "0";
            }
            acro = acro + "-" + value.ToString();

            string folioexist = " if exists (select * from Master_Settings where settings='Consolidate Sheet') update Master_Settings set value='" + acro + "' where settings='Consolidate Sheet' else insert into Master_Settings (settings,value) values ('Consolidate Sheet','" + acro + "')";
            int update = da.update_method_wo_parameter(folioexist, "text");
            string getval = da.GetFunction("select value from Master_Settings where settings='Consolidate Sheet' ");
        }
        catch (Exception ex)
        {
        }
    }

    protected void linksetting(object sender, EventArgs e)
    {

        try
        {
            ArrayList random = new ArrayList();
            int count = 0;
            for (int i = 0; i < count; i++)
            {


            }

            generate = "C" + "";
        }
        catch (Exception ex)
        {

        }
    }

    public void hiddenfiels()
    {
        IblError.Visible = false;
        lblno.Visible = false;
        HAllSpread.Visible = false;
        FpSpread2.Visible = false;
        printbtn.Visible = false;

        // headoffp2.Visible = false;
    }

}
