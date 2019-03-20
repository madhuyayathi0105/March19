using System;
using System.Collections;
using System.Configuration;
using System.Data;
//using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gios.Pdf;

public partial class ExamMarkSheetForTheoryAndPractical : System.Web.UI.Page
{
    SqlCommand cmd;
    string totcredits = string.Empty;
    string calculate = string.Empty;
    double total1 = 0;
    double gpacal1 = 0;
    DAccess2 d2 = new DAccess2();
    connection connection = new connection();
    Boolean Cellclick;
    SqlDataAdapter adaload;
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
    string collegecode = string.Empty;
    string Master = string.Empty;
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
                ddlformate.Visible = false;
                // generatefolio();
                collegecode = Session["collegecode"].ToString();
                txtdop.Attributes.Add("ReadOnly", "ReadOnly");
                txtdoi.Attributes.Add("ReadOnly", "ReadOnly");
                txtdop.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtdoi.Text = DateTime.Now.ToString("dd/MM/yyyy");
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
                // RadioButton1.Checked = true;
                int year1 = Convert.ToInt16(DateTime.Now.ToString("yyyy"));
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
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //RadioButton1.Visible = false;
        //RadioButton2.Visible = false;
        //RadioButton3.Visible = false;
        //RadioButton4.Visible = false;
        ddlformate.Visible = false;
        lblno.Visible = false;
        HAllSpread.Visible = false;
        int overalltot = 0;
        string exammonth = ddlMonth.SelectedValue.ToString();
        string examyear = ddlYear.SelectedValue.ToString();
        HAllSpread.Sheets[0].Columns[0].Locked = true;
        HAllSpread.Sheets[0].Columns[1].Locked = true;
        HAllSpread.Sheets[0].Columns[2].Locked = true;
        HAllSpread.Sheets[0].Columns[3].Locked = true;
        HAllSpread.Sheets[0].Columns[4].Locked = true;
        HAllSpread.Sheets[0].Columns[5].Locked = true;
        if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
        {
            year++;
            // string spraedbind = "select  distinct e.batchto,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.semester as sem,d.degree_code as degree,dpt.dept_code as dptcode from exmtt e,exmtt_det ex,Department dpt,degree d,course c,registration r where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.semester between 1 and 2 and  c.college_code=" + Session["collegecode"].ToString() + "  and ex.exam_code=e.exam_code and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''   order by e.semester ";
            // spraedbind = "select  distinct e.batch_year as batchto,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.current_semester as sem,d.degree_code as degree,dpt.dept_code as dptcode from Exam_Details e,Department dpt,degree d,course c,registration r where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.current_semester between 1 and 2 and  c.college_code=" + Session["collegecode"].ToString() + "  and e.exam_code=e.exam_code and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''   order by e.current_semester ";
            string spraedbind = "select distinct e.batch_year as batchto,p.Dept_Acronym as dept,p.Dept_Name as deptname,c.course_name as course,e.current_semester as sem, e.degree_code as degree,p.dept_code as dptcode,count(distinct a.roll_no) as studcount   from exam_details e,exam_application a,registration r,degree g,course c,department p  where e.exam_code = a.exam_code and a.roll_no = r.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id and g.dept_code = p.dept_code   and c.college_code=" + Session["collegecode"].ToString() + "  and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + "  and ltrim(r.roll_no) <>''        and e.current_semester between 1 and 2  group by e.batch_year,p.Dept_Acronym,p.Dept_Name,c.course_name,e.current_semester,e.degree_code,p.dept_code     order by e.current_semester ";
            SqlCommand Todptcmd = new SqlCommand(spraedbind, con);
            SqlDataReader Toadeptreader;
            string course = string.Empty;
            string department = string.Empty;
            string sem = string.Empty;
            string degree = string.Empty;
            string batchyear = string.Empty;
            string department_code = string.Empty;
            string department_name = string.Empty;
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
                    batchyear = Toadeptreader["batchto"].ToString();
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Note = batchyear;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Tag = degree;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Text = department;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Note = degree;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Tag = department_name;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 2].Text = course;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].Text = sem + "";
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    //  string totstud = "select  isnull(count(*),0) as total from registration where degree_code='" + degree + "' and current_semester='" + sem + "' and college_code=" + Session["collegecode"].ToString() + " and cc=0  and delflag=0 and exam_flag<>'Debar'";
                    string studinfo = da.GetFunction("select distinct count(r.reg_no) as count  from exam_details e,exam_application a,registration r,degree g,course c,department p        where e.exam_code = a.exam_code and a.roll_no = r.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id         and g.dept_code = p.dept_code          and c.college_code='" + Session["collegecode"].ToString() + "'  and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + "            and ltrim(r.roll_no) <>''                    and r.degree_code=" + degree + " and e.current_semester= " + sem + "");
                    string totstud = studinfo;
                    int totalstudents = 0;
                    if (Int32.TryParse(studinfo, out totalstudents))
                    {
                        totalstudents = totalstudents + Convert.ToInt32(totstud);
                        overalltot = overalltot + totalstudents;
                    }
                    else
                    {
                        totalstudents = 0;
                    }
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(totstud); //totalstudents + "";
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    int semval = Convert.ToInt16(sem) + Convert.ToInt16(1);
                    //SqlCommand Totcmd = new SqlCommand(totstud, con1);
                    //con1.Close();
                    //con1.Open();
                    //int totalstudents = 0;
                    //SqlDataReader Totreader;
                    //Totreader = Totcmd.ExecuteReader();
                    //if (Totreader.HasRows)
                    //{
                    //    while (Totreader.Read())
                    //    {
                    //        totalstudents = totalstudents + Convert.ToInt32(Totreader["total"]);
                    //        overalltot = overalltot + totalstudents;
                    //        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(Totreader["total"]); //totalstudents + "";
                    //        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    //    }
                    //}
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
            // string spraedbind = "select  distinct e.batchto,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.semester as sem,d.degree_code as degree,dpt.dept_code as dptcode from exmtt e,exmtt_det ex,Department dpt,degree d,course c,registration r where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.semester between 3 and 4 and  c.college_code=" + Session["collegecode"].ToString() + " and ex.exam_code=e.exam_code and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''   order by e.semester ";
            string spraedbind = "select distinct e.batch_year as batchto,p.Dept_Acronym as dept,p.Dept_Name as deptname,c.course_name as course,e.current_semester as sem, e.degree_code as degree,p.dept_code as dptcode,count(distinct a.roll_no) as studcount   from exam_details e,exam_application a,registration r,degree g,course c,department p  where e.exam_code = a.exam_code and a.roll_no = r.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id and g.dept_code = p.dept_code   and c.college_code=" + Session["collegecode"].ToString() + "  and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + "  and ltrim(r.roll_no) <>''        and e.current_semester between 3 and 4  group by e.batch_year,p.Dept_Acronym,p.Dept_Name,c.course_name,e.current_semester,e.degree_code,p.dept_code     order by e.current_semester ";
            SqlCommand Todptcmd = new SqlCommand(spraedbind, con);
            SqlDataReader Toadeptreader;
            string course = string.Empty;
            string department = string.Empty;
            string sem = string.Empty;
            string degree = string.Empty;
            string batchyear = string.Empty;
            string department_code = string.Empty;
            string department_name = string.Empty;
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
                    batchyear = Toadeptreader["batchto"].ToString();
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Note = batchyear;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Tag = degree;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Tag = department_name;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].Text = sem + "";
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Note = degree;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Text = department;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 2].Text = course;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    // string totstud = "select isnull(count(*),0) as total from registration where degree_code='" + degree + "' and current_semester='" + sem + "' and college_code=" + Session["collegecode"].ToString() + " and cc=0  and delflag=0 and exam_flag<>'Debar'";
                    //string totstud = Toadeptreader["degree"].ToString();
                    //int semval = Convert.ToInt16(sem) + Convert.ToInt16(1);
                    string studinfo = da.GetFunction("select distinct count(r.reg_no) as count  from exam_details e,exam_application a,registration r,degree g,course c,department p        where e.exam_code = a.exam_code and a.roll_no = r.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id         and g.dept_code = p.dept_code          and c.college_code='" + Session["collegecode"].ToString() + "'  and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + "            and ltrim(r.roll_no) <>''                    and r.degree_code=" + degree + " and e.current_semester= " + sem + "");
                    string totstud = studinfo;
                    int totalstudents = 0;
                    if (Int32.TryParse(studinfo, out totalstudents))
                    {
                        totalstudents = totalstudents + Convert.ToInt32(totstud);
                        overalltot = overalltot + totalstudents;
                    }
                    else
                    {
                        totalstudents = 0;
                    }
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(totstud); //totalstudents + "";
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    int semval = Convert.ToInt16(sem) + Convert.ToInt16(1);
                    //SqlCommand Totcmd = new SqlCommand(totstud, con1);
                    //con1.Close();
                    //con1.Open();
                    //int totalstudents = 0;
                    //SqlDataReader Totreader;
                    //Totreader = Totcmd.ExecuteReader();
                    //if (Totreader.HasRows)
                    //{
                    //    while (Totreader.Read())
                    //    {
                    //        totalstudents = totalstudents + Convert.ToInt32(Totreader["total"]);
                    //        overalltot = overalltot + totalstudents;
                    //        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(Totreader["total"]);// totalstudents + "";
                    //        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    //    }
                    //}
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
            // string spraedbind = "select  distinct e.batchto,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.semester as sem,d.degree_code as degree,dpt.dept_code as dptcode from exmtt e,exmtt_det ex,Department dpt,degree d,course c,registration r where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.semester between 5 and 6 and  c.college_code=" + Session["collegecode"].ToString() + "  and ex.exam_code=e.exam_code and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''   order by e.semester ";
            string spraedbind = "select distinct e.batch_year as batchto,p.Dept_Acronym as dept,p.Dept_Name as deptname,c.course_name as course,e.current_semester as sem, e.degree_code as degree,p.dept_code as dptcode,count(distinct a.roll_no) as studcount   from exam_details e,exam_application a,registration r,degree g,course c,department p  where e.exam_code = a.exam_code and a.roll_no = r.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id and g.dept_code = p.dept_code   and c.college_code=" + Session["collegecode"].ToString() + "  and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + "  and ltrim(r.roll_no) <>''        and e.current_semester between 5 and 6  group by e.batch_year,p.Dept_Acronym,p.Dept_Name,c.course_name,e.current_semester,e.degree_code,p.dept_code     order by e.current_semester ";
            SqlCommand Todptcmd = new SqlCommand(spraedbind, con);
            SqlDataReader Toadeptreader;
            string course = string.Empty;
            string department = string.Empty;
            string sem = string.Empty;
            string degree = string.Empty;
            string batchyear = string.Empty;
            string department_code = string.Empty;
            string department_name = string.Empty;
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
                    batchyear = Toadeptreader["batchto"].ToString();
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Note = batchyear;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Tag = degree;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Tag = department_name;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].Text = sem + "";
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Note = degree;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Text = department;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 2].Text = course;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    // string totstud = "select count(*)as total from registration where degree_code='" + degree + "' and current_semester='" + sem + "' and college_code=" + Session["collegecode"].ToString() + "  ";
                    string studinfo = da.GetFunction("select distinct count(r.reg_no) as count  from exam_details e,exam_application a,registration r,degree g,course c,department p        where e.exam_code = a.exam_code and a.roll_no = r.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id         and g.dept_code = p.dept_code          and c.college_code='" + Session["collegecode"].ToString() + "'  and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + "            and ltrim(r.roll_no) <>''                    and r.degree_code=" + degree + " and e.current_semester= " + sem + "");
                    string totstud = studinfo;
                    int totalstudents = 0;
                    if (Int32.TryParse(studinfo, out totalstudents))
                    {
                        totalstudents = totalstudents + Convert.ToInt32(totstud);
                        overalltot = overalltot + totalstudents;
                    }
                    else
                    {
                        totalstudents = 0;
                    }
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(totstud); //totalstudents + "";
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    int semval = Convert.ToInt16(sem) + Convert.ToInt16(1);
                    //SqlCommand Totcmd = new SqlCommand(totstud, con1);
                    //con1.Close();
                    //con1.Open();
                    //int totalstudents = 0;
                    //SqlDataReader Totreader;
                    //Totreader = Totcmd.ExecuteReader();
                    //if (Totreader.HasRows)
                    //{
                    //    while (Totreader.Read())
                    //    {
                    //        totalstudents = totalstudents + Convert.ToInt32(Totreader["total"]);
                    //        overalltot = overalltot + totalstudents;
                    //        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].Text = totalstudents + "";
                    //        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    //    }
                    //}
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
            //string spraedbind = "select  distinct e.batchto,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.semester as sem,d.degree_code as degree,dpt.dept_code as dptcode from exmtt e,exmtt_det ex,Department dpt,degree d,course c,registration r where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.semester between 7 and 8 and  c.college_code=" + Session["collegecode"].ToString() + "  and ex.exam_code=e.exam_code and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''   order by e.semester ";
            //string spraedbind = "select  distinct e.batch_year as batchto,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.current_semester   as sem,d.degree_code as degree,dpt.dept_code as dptcode from Exam_Details e,Department dpt,degree d,course c,registration r   where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.current_semester   between 7 and 8 and  c.college_code=" + Session["collegecode"].ToString() + "  and e.exam_code=e.exam_code and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''   order by e.current_semester    ";
            string spraedbind = "select distinct e.batch_year as batchto,p.Dept_Acronym as dept,p.Dept_Name as deptname,c.course_name as course,e.current_semester as sem, e.degree_code as degree,p.dept_code as dptcode,count(distinct a.roll_no) as studcount   from exam_details e,exam_application a,registration r,degree g,course c,department p  where e.exam_code = a.exam_code and a.roll_no = r.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id and g.dept_code = p.dept_code   and c.college_code=" + Session["collegecode"].ToString() + "  and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + "  and ltrim(r.roll_no) <>''        and e.current_semester between 7 and 8  group by e.batch_year,p.Dept_Acronym,p.Dept_Name,c.course_name,e.current_semester,e.degree_code,p.dept_code     order by e.current_semester ";
            SqlCommand Todptcmd = new SqlCommand(spraedbind, con);
            SqlDataReader Toadeptreader;
            string course = string.Empty;
            string department = string.Empty;
            string sem = string.Empty;
            string degree = string.Empty;
            string batchyear = string.Empty;
            string department_code = string.Empty;
            string department_name = string.Empty;
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
                    batchyear = Toadeptreader["batchto"].ToString();
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Tag = degree;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Tag = department_name;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].Text = sem + "";
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Note = degree;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Text = department;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 2].Text = course;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    // string totstud = "select count(*)as total from registration where degree_code='" + degree + "' and current_semester='" + sem + "' and college_code=" + Session["collegecode"].ToString() + "  ";
                    string studinfo = da.GetFunction("select distinct count(r.reg_no) as count  from exam_details e,exam_application a,registration r,degree g,course c,department p        where e.exam_code = a.exam_code and a.roll_no = r.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id         and g.dept_code = p.dept_code          and c.college_code='" + Session["collegecode"].ToString() + "'  and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + "            and ltrim(r.roll_no) <>''                    and r.degree_code=" + degree + " and e.current_semester= " + sem + "");
                    string totstud = studinfo;
                    int totalstudents = 0;
                    if (Int32.TryParse(studinfo, out totalstudents))
                    {
                        totalstudents = totalstudents + Convert.ToInt32(totstud);
                        overalltot = overalltot + totalstudents;
                    }
                    else
                    {
                        totalstudents = 0;
                    }
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(totstud); //totalstudents + "";
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    int semval = Convert.ToInt16(sem) + Convert.ToInt16(1);
                    //SqlCommand Totcmd = new SqlCommand(totstud, con1);
                    //con1.Close();
                    //con1.Open();
                    //int totalstudents = 0;
                    //SqlDataReader Totreader;
                    //Totreader = Totcmd.ExecuteReader();
                    //if (Totreader.HasRows)
                    //{
                    //    while (Totreader.Read())
                    //    {
                    //        totalstudents = totalstudents + Convert.ToInt32(Totreader["total"]);
                    //        overalltot = overalltot + totalstudents;
                    //        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].Text = totalstudents + "";
                    //        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    //    }
                    //}
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
            //RadioButton1.Visible = false;
            //RadioButton2.Visible = false;
            //RadioButton3.Visible = false;
            //RadioButton4.Visible = false;
            ddlformate.Visible = false;
        }
        else
        {
            lblno.Visible = false;
        }
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread2.Visible = false;
        printbtn.Visible = false;
        Label2.Visible = false;
        HAllSpread.Visible = false;
        //RadioButton1.Visible = false;
        //RadioButton2.Visible = false;
        //RadioButton3.Visible = false;
        //RadioButton4.Visible = false;
        ddlformate.Visible = false;
        hiddenfiels();
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread2.Visible = false;
        printbtn.Visible = false;
        Label2.Visible = false;
        HAllSpread.Visible = false;
        //RadioButton1.Visible = false;
        //RadioButton2.Visible = false;
        //RadioButton3.Visible = false;
        ddlformate.Visible = false;
        hiddenfiels();
    }

    protected void HAllSpread_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        Cellclick = true;
    }

    protected void HAllSpread_SelectedIndexChanged(Object sender, EventArgs e)
    {
        if (Cellclick == true)
        {
            string exammonth = ddlMonth.SelectedValue.ToString();
            string examyear = ddlYear.SelectedValue.ToString();
            //RadioButton1.Visible = true;
            //RadioButton2.Visible = true;
            //RadioButton3.Visible = true;
            //RadioButton4.Visible = true;
            ddlformate.Visible = true;
            printbtn.Visible = false;
            FpSpread2.Visible = false;
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
            FpSpread2.Sheets[0].Columns[0].Locked = true;
            FpSpread2.Sheets[0].Columns[1].Locked = true;
            FpSpread2.Sheets[0].Columns[2].Locked = true;
            FpSpread2.Sheets[0].Columns[3].Locked = true;
            FpSpread2.Sheets[0].Columns[4].Locked = true;
            FpSpread2.Sheets[0].Columns[5].Locked = true;
            FpSpread2.Sheets[0].RowHeader.Visible = false;
            FpSpread2.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread2.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].SheetCorner.RowCount = 2;
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
            string activerow = string.Empty;
            string activecol = string.Empty;
            string depart_code = string.Empty;
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
            // string studinfo = "select len(r.reg_no),r.reg_no,r.stud_name,r.roll_no,r.batch_year from registration r where r.current_semester=" + sem + "and r.degree_code=" + depart_code + "   order by len(r.reg_no),r.reg_no,r.stud_name";
            string studinfo = "select distinct r.reg_no,r.stud_name,r.roll_no,r.batch_year  from exam_details e,exam_application a,registration r,degree g,course c,department p        where e.exam_code = a.exam_code and a.roll_no = r.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id         and g.dept_code = p.dept_code          and c.college_code='" + Session["collegecode"].ToString() + "'  and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + "            and ltrim(r.roll_no) <>''                    and r.degree_code=" + depart_code + " and e.current_semester= " + sem + "          ";
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
                    string regno = string.Empty;
                    string studname = string.Empty;
                    string rollno = string.Empty;
                    string batchyear = string.Empty;
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
                        string regularpaper = string.Empty;
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
                        string arrearpaper = string.Empty;
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
        string actrow = e.SheetView.ActiveRow.ToString();
        if (flag_true == false && actrow == "0")
        {
            for (int j = 1; j < Convert.ToInt16(FpSpread2.Sheets[0].RowCount); j++)
            {
                IblError.Visible = false;
                string actcol = e.SheetView.ActiveColumn.ToString();
                string seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
                if (seltext != "System.Object")
                    FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = seltext.ToString();
            }
            flag_true = true;
        }
        if (actrow != "0")
        {
            IblError.Text = " ";
            IblError.Visible = false;
        }
    }

    protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
        }
    }

    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
        }
    }

    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
        }
    }

    protected void printbtn_Click(object sender, EventArgs e)
    {
        try
        {
            IblError.Visible = false;
            //RadioButton1.Visible = true;
            //RadioButton2.Visible = true;
            //RadioButton3.Visible = true;
            //RadioButton4.Visible = true;
            ddlformate.Visible = true;
            if (ddlformate.SelectedIndex == 0)
            {
                DataSet printds = new DataSet();
                DataSet printds_new = new DataSet();
                DataSet printds_rows = new DataSet();
                string degree = string.Empty;
                string monthandyear = string.Empty;
                string studname = string.Empty;
                string dob = string.Empty;
                string rollnosub = string.Empty;
                string regnumber = string.Empty;
                string batch_year = string.Empty;
                string degree_code = string.Empty;
                string exam_code = string.Empty;
                string sem = string.Empty;
                //int max_internal_mark = 0;
                //int max_external_mark = 0;
                int colval = 0;
                string branch = string.Empty;
                int month = 0;
                string monthstr = string.Empty;
                string sql2 = string.Empty;
                string sql3 = string.Empty;
                string roman = string.Empty;
                string semroman = string.Empty;
                string grade = string.Empty;
                string gradepoints = string.Empty;
                string coe = string.Empty;
                string subjectcode_Part1 = string.Empty;
                string subjectcode_Part2 = string.Empty;
                string subjectcode_Part3 = string.Empty;
                string subjectcode_Part4 = string.Empty;
                string cal_gpa = string.Empty;
                string current_semester = string.Empty;
                string subtype = string.Empty;
                DataSet gradeds = new DataSet();
                Font Fontco12 = new Font("Comic Sans MS", 12, FontStyle.Bold);
                Font Fontpala12 = new Font("Palatino Linotype", 10, FontStyle.Bold);
                Font Fontco10 = new Font("Comic Sans MS", 10, FontStyle.Regular);
                Font Fontco12a = new Font("Comic Sans MS", 12, FontStyle.Bold);
                Font Fontarial7 = new Font("Arial", 7, FontStyle.Regular);
                Font Fontarial7r = new Font("Arial", 6, FontStyle.Bold);
                Font Fontarial9 = new Font("Arial", 8, FontStyle.Bold);
                Font Fontarial10 = new Font("Arial", 10, FontStyle.Regular);
                Font Fontarial12 = new Font("Arial", 12, FontStyle.Regular);
                //Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
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
                            string sql = "SELECT Reg_No,r.batch_year,r.app_no,r.degree_code,R.current_semester,R.Stud_Name,Course_Name as degree,Dept_name,CONVERT(VARCHAR, dob, 103) as dob FROM Registration R,Applyn A,Degree G,Course C,Department D WHERE R.App_No = A.App_No AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND D.College_Code = G.College_Code AND Roll_No = '" + rollnosub + "' and r.college_code='" + Session["collegecode"].ToString() + "' ;  select coe from collinfo where college_code='" + Session["collegecode"].ToString() + "';";
                            //dummy
                            // sql = "SELECT Reg_No,r.batch_year,r.degree_code,R.current_semester,R.Stud_Name,Course_Name+'('+Dept_acronym+')' as degree,Dept_name,CONVERT(VARCHAR, dob, 103) as dob FROM Registration R,Applyn A,Degree G,Course C,Department D WHERE R.App_No = A.App_No AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND D.College_Code = G.College_Code AND Roll_No = '14UHI6001' ;  select coe from collinfo";
                            printds = da.select_method_wo_parameter(sql, "Text");
                            if (printds.Tables[0].Rows.Count > 0)
                            {
                                //current_semester = printds.Tables[0].Rows[0]["current_semester"].ToString();
                                current_semester = Session["semforsub"].ToString();
                                mypdfpage = mydoc.NewPage();
                                degree = printds.Tables[0].Rows[0]["degree"].ToString();
                                coe = printds.Tables[1].Rows[0]["coe"].ToString();
                                string[] spiltcoe = coe.Split('.');
                                string newcoe = string.Empty;
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
                                sql2 = "select * from exam_details where    degree_code='" + degree_code + "' and current_semester='" + sem + "' and exam_month='" + month + "' and exam_year='" + ddlYear.SelectedItem.Text + "'";
                                sql2 = "select * from exam_details where    degree_code='" + degree_code + "'  and exam_month='" + month + "' and exam_year='" + ddlYear.SelectedItem.Text + "' and  batch_year=" + batch_year + "";
                                //dummy
                                // sql2 = "select * from exam_details where  batch_year='2014' and   degree_code='45' and current_semester='1' and exam_month='11' and exam_year='2014'";
                                printds_new = da.select_method_wo_parameter(sql2, "Text");
                                if (printds_new.Tables[0].Rows.Count > 0)
                                {
                                    sem = printds_new.Tables[0].Rows[0]["current_semester"].ToString();
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
                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                            if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg")))
                                            {
                                                //image3.ImageUrl = "~/college/" + stdappno + ".jpeg";
                                            }
                                            else
                                            {
                                                thumb.Save(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                //image3.ImageUrl = "~/college/" + stdappno + ".jpeg";
                                            }
                                        }
                                    }
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg")))
                                    {
                                        Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg"));
                                        mypdfpage.Add(LogoImage2, 500, 13, 340);
                                    }
                                    else
                                    {
                                        Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg"));
                                        mypdfpage.Add(LogoImage2, 500, 13, 340);
                                    }
                                    PdfTextArea pdfdoi = new PdfTextArea(Fontco10, System.Drawing.Color.Black, new PdfArea(mydoc, 74, 681, 305, 50), System.Drawing.ContentAlignment.TopLeft, txtdoi.Text.ToString());
                                    mypdfpage.Add(pdfdoi);
                                    exam_code = printds_new.Tables[0].Rows[0]["exam_code"].ToString();
                                    sql3 = "Select syllabus_master.semester,Subject_Type,subject_name,subject_code,subject.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,maxtotal,grade,cp,credit_points,mark_entry.subject_no,semester,maxtotal,exam_code from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = '" + exam_code + "' and roll_no='" + rollnosub + "'  order by syllabus_master.semester,subject_type,sub_sem.lab,subject.subject_no";
                                    //dummy
                                    // sql3 = "Select Subject_Type,subject_name,subject_code,subject.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,maxtotal,grade,cp,credit_points,mark_entry.subject_no,semester,maxtotal,exam_code from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = 10146 and roll_no='14UHI6001'  order by LEN(subject_type),subject_type";
                                    printds_rows.Clear();
                                    printds_rows.Dispose();
                                    printds_rows = da.select_method_wo_parameter(sql3, "Text");
                                    Gios.Pdf.PdfTable table1forpage1 = mydoc.NewTable(Fontco12, 1, 3, 1);
                                    table1forpage1.VisibleHeaders = false;
                                    table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                                    table1forpage1.SetColumnsWidth(new int[] { 91, 157, 119 });
                                    table1forpage1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage1.Cell(0, 0).SetContent(degree);
                                    table1forpage1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage1.Cell(0, 1).SetContent("");
                                    table1forpage1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage1.Cell(0, 2).SetContent(monthandyear);
                                    Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 116, 69, 366, 50));
                                    mypdfpage.Add(newpdftabpage2);
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
                                    newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 12, 125, 567, 50));//sr
                                    mypdfpage.Add(newpdftabpage2);
                                    Gios.Pdf.PdfTable table1forpage2a = mydoc.NewTable(Fontco10, 1, 4, 1);
                                    table1forpage2a.Columns[0].SetWidth(354);
                                    table1forpage2a.VisibleHeaders = false;
                                    table1forpage2a.SetBorders(Color.Black, 1, BorderType.None);
                                    table1forpage2a.SetColumnsWidth(new int[] { 298, 56, 108, 108 });
                                    table1forpage2a.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage2a.Cell(0, 0).SetContent(branch);
                                    table1forpage2a.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage2a.Cell(0, 1).SetContent(semroman);
                                    table1forpage2a.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage2a.Cell(0, 2).SetContent(monthandyear);
                                    table1forpage2a.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage2a.Cell(0, 3).SetContent(txtdop.Text);
                                    newpdftabpage2 = table1forpage2a.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 12, 158, 567, 50));
                                    mypdfpage.Add(newpdftabpage2);
                                    colval = 215;
                                    //newpdftabpage2 = fullmark.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 12, 215, 570, 800));
                                    //mypdfpage.Add(newpdftabpage2);
                                    if (printds_rows.Tables[0].Rows.Count > 0)
                                    {
                                        Gios.Pdf.PdfTable fullmark = mydoc.NewTable(Fontarial7, printds_rows.Tables[0].Rows.Count + 1, 13, 2);
                                        fullmark.VisibleHeaders = false;
                                        fullmark.SetBorders(Color.Black, 1, BorderType.None);
                                        fullmark.Columns[0].SetWidth(23);
                                        fullmark.Columns[1].SetWidth(43);
                                        fullmark.Columns[2].SetWidth(232);
                                        fullmark.Columns[3].SetWidth(27);
                                        fullmark.Columns[4].SetWidth(27);
                                        fullmark.Columns[5].SetWidth(27);
                                        fullmark.Columns[6].SetWidth(27);
                                        fullmark.Columns[7].SetWidth(27);
                                        fullmark.Columns[8].SetWidth(27);
                                        fullmark.Columns[9].SetWidth(27);
                                        fullmark.Columns[10].SetWidth(34);
                                        fullmark.Columns[11].SetWidth(23);
                                        fullmark.Columns[12].SetWidth(23);
                                        for (int i = 0; i < printds_rows.Tables[0].Rows.Count; i++)
                                        {
                                            subtype = printds_rows.Tables[0].Rows[i]["Subject_Type"].ToString();
                                            string[] spitsubtype = subtype.Split(' ');
                                            if (spitsubtype.GetUpperBound(0) > 0)
                                            {
                                                roman = spitsubtype[1].ToString();
                                                if (roman == "1" || roman.Trim().ToUpper() == "I")
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
                                                else if (roman == "2" || roman.Trim().ToUpper() == "II")
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
                                                else if (roman == "3" || roman.Trim().ToUpper() == "III")
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
                                                else if (roman == "4" || roman.Trim().ToUpper() == "IV")
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
                                            if (printds_new.Tables[0].Rows.Count > 0)
                                            {
                                                //PdfTextArea pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 23, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, roman);//17
                                                //mypdfpage.Add(pdfdegree);
                                                fullmark.Cell(i, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                fullmark.Cell(i, 0).SetContent(roman);
                                                //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 40, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["subject_code"].ToString());
                                                //mypdfpage.Add(pdfdegree);
                                                fullmark.Cell(i, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                fullmark.Cell(i, 1).SetContent(printds_rows.Tables[0].Rows[i]["subject_code"].ToString());
                                                //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 87, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["subject_name"].ToString().ToUpper());
                                                //mypdfpage.Add(pdfdegree);
                                                fullmark.Cell(i, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                fullmark.Cell(i, 2).SetContent(printds_rows.Tables[0].Rows[i]["subject_name"].ToString().ToUpper());
                                                //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 321, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["max_int_marks"].ToString());
                                                //mypdfpage.Add(pdfdegree);
                                                fullmark.Cell(i, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                fullmark.Cell(i, 3).SetContent(printds_rows.Tables[0].Rows[i]["max_int_marks"].ToString());
                                                double internalmmark = 0;
                                                double externalmark1 = 0;
                                                double totalintext = 0;
                                                //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 348, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["internal_mark"].ToString());
                                                //mypdfpage.Add(pdfdegree);
                                                fullmark.Cell(i, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                fullmark.Cell(i, 4).SetContent(printds_rows.Tables[0].Rows[i]["internal_mark"].ToString());
                                                internalmmark = Convert.ToDouble(printds_rows.Tables[0].Rows[i]["internal_mark"].ToString());
                                                //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 375, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["max_ext_marks"].ToString());
                                                //mypdfpage.Add(pdfdegree);
                                                fullmark.Cell(i, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                fullmark.Cell(i, 5).SetContent(printds_rows.Tables[0].Rows[i]["max_ext_marks"].ToString());
                                                double extfinal = Convert.ToDouble(printds_rows.Tables[0].Rows[i]["external_mark"].ToString());
                                                extfinal = Math.Round(extfinal, 0);
                                                externalmark1 = extfinal;
                                                string checkedmark = string.Empty;
                                                if (extfinal < 0)
                                                {
                                                    checkedmark = loadmarkat(Convert.ToString(extfinal));
                                                }
                                                else
                                                {
                                                    checkedmark = Convert.ToString(extfinal);
                                                }
                                                if (internalmmark >= 0)
                                                {
                                                    if (extfinal > 0)
                                                    {
                                                        totalintext = internalmmark + externalmark1;
                                                    }
                                                    else
                                                    {
                                                        totalintext = internalmmark;
                                                    }
                                                }
                                                else
                                                {
                                                    if (extfinal > 0)
                                                    {
                                                        totalintext = externalmark1;
                                                    }
                                                    else
                                                    {
                                                        totalintext = 0;
                                                    }
                                                }
                                                //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 403, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, checkedmark);
                                                //mypdfpage.Add(pdfdegree);
                                                fullmark.Cell(i, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                fullmark.Cell(i, 6).SetContent(checkedmark);
                                                //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 428, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["maxtotal"].ToString());
                                                //mypdfpage.Add(pdfdegree);
                                                fullmark.Cell(i, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                fullmark.Cell(i, 7).SetContent(printds_rows.Tables[0].Rows[i]["maxtotal"].ToString());
                                                double totfinal = 0;
                                                totfinal = Math.Round(totalintext, 0);
                                                //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 456, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(totfinal));
                                                //mypdfpage.Add(pdfdegree);
                                                fullmark.Cell(i, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                fullmark.Cell(i, 8).SetContent(Convert.ToString(totfinal));
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
                                                //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 485, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["credit_points"].ToString());
                                                //mypdfpage.Add(pdfdegree);
                                                fullmark.Cell(i, 9).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                fullmark.Cell(i, 9).SetContent(printds_rows.Tables[0].Rows[i]["credit_points"].ToString());
                                                // gradepoints = String.Format("{0:0.00}", gradepoints);
                                                gradepoints = Convert.ToString(Math.Round(Convert.ToDouble(gradepoints), 2));
                                                gradepoints = String.Format("{0:0.00}", Convert.ToDouble(gradepoints));
                                                string result = printds_rows.Tables[0].Rows[i]["result"].ToString().ToLower();
                                                if (result == "fail")
                                                {
                                                    result = "RA";
                                                    grade = "U";
                                                    //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 511, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, "0.00");
                                                    //mypdfpage.Add(pdfdegree);
                                                    fullmark.Cell(i, 10).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    fullmark.Cell(i, 10).SetContent("0.00");
                                                }
                                                else if (result == "pass")
                                                {
                                                    result = "P";
                                                    //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 511, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, gradepoints);
                                                    //mypdfpage.Add(pdfdegree);
                                                    fullmark.Cell(i, 10).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    fullmark.Cell(i, 10).SetContent(gradepoints);
                                                }
                                                else
                                                {
                                                    result = "AB";
                                                    grade = "U";
                                                    //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 511, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, "0.00");
                                                    //mypdfpage.Add(pdfdegree);
                                                    fullmark.Cell(i, 10).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    fullmark.Cell(i, 10).SetContent("0.00");
                                                }
                                                //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 543, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, grade);
                                                //mypdfpage.Add(pdfdegree);
                                                fullmark.Cell(i, 11).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                string newggrade = "   " + grade;
                                                fullmark.Cell(i, 11).SetContent(newggrade);
                                                //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 567, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, result);
                                                //mypdfpage.Add(pdfdegree);
                                                fullmark.Cell(i, 12).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                fullmark.Cell(i, 12).SetContent(result);
                                                colval = colval + 12;
                                            }
                                        }
                                        fullmark.Cell(printds_rows.Tables[0].Rows.Count, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        fullmark.Cell(printds_rows.Tables[0].Rows.Count, 0).SetFont(Fontpala12);
                                        fullmark.Cell(printds_rows.Tables[0].Rows.Count, 0).SetContent("****** End of Statement ******");
                                        foreach (PdfCell pr in fullmark.CellRange(printds_rows.Tables[0].Rows.Count, 0, printds_rows.Tables[0].Rows.Count, 0).Cells)
                                        {
                                            pr.ColSpan = 13;
                                        }
                                        newpdftabpage2 = fullmark.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 14, 215, 567, 800));
                                        mypdfpage.Add(newpdftabpage2);
                                    }
                                }
                                //PdfTextArea pdfdegree123 = new PdfTextArea(Fontpala12, System.Drawing.Color.Black, new PdfArea(mydoc, 231, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, "****** End of Statement ******");
                                //mypdfpage.Add(pdfdegree123);
                                Gios.Pdf.PdfTable table1forpagecoe = mydoc.NewTable(Fontarial9, 1, 2, 1);
                                table1forpagecoe.VisibleHeaders = false;
                                table1forpagecoe.SetBorders(Color.Black, 1, BorderType.None);
                                table1forpagecoe.SetColumnsWidth(new int[] { 190 });
                                table1forpagecoe.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                //table1forpagecoe.Cell(0, 0).SetContent("D");
                                //table1forpagecoe.Cell(0, 0).SetFont(Fontarial12);
                                //table1forpagecoe.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                //table1forpagecoe.Cell(0, 0).SetContent("R.");
                                //table1forpagecoe.Cell(0, 0).SetFont(Fontarial10);
                                table1forpagecoe.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpagecoe.Cell(0, 0).SetContent("." + coe);
                                Gios.Pdf.PdfTablePage newpdftabpagecoe = table1forpagecoe.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 432, 782, 190, 50));
                                mypdfpage.Add(newpdftabpagecoe);
                                table1forpagecoe = mydoc.NewTable(Fontarial9, 1, 1, 1);
                                table1forpagecoe.VisibleHeaders = false;
                                table1forpagecoe.SetBorders(Color.Black, 1, BorderType.None);
                                table1forpagecoe.SetColumnsWidth(new int[] { 10 });
                                table1forpagecoe.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                                table1forpagecoe.Cell(0, 0).SetContent("D");
                                // table1forpagecoe.Cell(0, 0).SetFont(Fontarial12);
                                newpdftabpagecoe = table1forpagecoe.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 420, 782, 10, 25));
                                mypdfpage.Add(newpdftabpagecoe);
                                table1forpagecoe = mydoc.NewTable(Fontarial9, 1, 1, 1);
                                table1forpagecoe.VisibleHeaders = false;
                                table1forpagecoe.SetBorders(Color.Black, 1, BorderType.None);
                                table1forpagecoe.SetColumnsWidth(new int[] { 10 });
                                table1forpagecoe.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpagecoe.Cell(0, 0).SetContent("R");
                                table1forpagecoe.Cell(0, 0).SetFont(Fontarial7r);
                                newpdftabpagecoe = table1forpagecoe.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 428, 783.5, 10, 25));
                                mypdfpage.Add(newpdftabpagecoe);
                                string year = ddlYear.SelectedItem.Text;
                                string collcode = Session["collegecode"].ToString();
                                Gios.Pdf.PdfTable table1forpagegpa = mydoc.NewTable(Fontco12a, 1, 4, 1);
                                table1forpagegpa.VisibleHeaders = false;
                                table1forpagegpa.SetBorders(Color.Black, 1, BorderType.None);
                                table1forpagegpa.SetColumnsWidth(new int[] { 94, 94, 94, 94 });
                                table1forpagegpa.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                double partsums = 0.00;
                                int partrowcount = 0;
                                Double Credit_Points = 0.0;
                                Double grade_points = 0.0;
                                double creditstotal = 0;
                                if (printds_rows.Tables[0].Rows.Count > 0)
                                {
                                    string sumpart = string.Empty;
                                    DataView dv_demand_data = new DataView();
                                    DataView dv_demand_datadummy = new DataView();
                                    printds_rows.Tables[0].DefaultView.RowFilter = "subject_type='Part I'  and semester='" + current_semester + "'";
                                    dv_demand_datadummy = printds_rows.Tables[0].DefaultView;
                                    partrowcount = dv_demand_datadummy.Count;
                                    printds_rows.Tables[0].DefaultView.RowFilter = "Subject_Type='Part I' and result='pass' and semester='" + current_semester + "'";
                                    dv_demand_data = printds_rows.Tables[0].DefaultView;
                                    // dv_demand_data.Sort = "arrearcount asc";
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
                                                gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 
                                                gradeds.Clear();
                                                gradeds = da.select_method_wo_parameter(gradesql, "Text");
                                            }
                                            for (int grd = 0; grd < gradeds.Tables[0].Rows.Count; grd++)
                                            {
                                                if (Convert.ToInt32(gradeds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(gradeds.Tables[0].Rows[grd][2]) >= checkmarkmm)
                                                {
                                                    grade_points = Convert.ToDouble(dv_demand_data[sum]["total"].ToString());
                                                    grade_points = grade_points / 10;
                                                    Credit_Points = Convert.ToDouble(dv_demand_data[sum]["credit_points"].ToString());
                                                    creditstotal = creditstotal + Credit_Points;
                                                    partsums = partsums + (grade_points * Credit_Points);
                                                }
                                            }
                                        }
                                        //partsums = (partsums / creditstotal);
                                        //partsums = Math.Round(partsums, 2);
                                        //sumpart = String.Format("{0:0.00}", partsums);
                                        if (creditstotal == 0)
                                        {
                                            sumpart = "0.00";
                                        }
                                        else if (creditstotal > 0)
                                        {
                                            partsums = (partsums / creditstotal);
                                            partsums = Math.Round(partsums, 2);
                                            sumpart = String.Format("{0:0.00}", partsums);
                                        }
                                        else
                                        {
                                            sumpart = "0.00";
                                        }
                                    }
                                    else if (partrowcount > 0)
                                    {
                                        sumpart = "0.00";
                                    }
                                    else
                                    {
                                        sumpart = "--";
                                    }
                                    //cal_gpa = Calulat_GPA_forpart(rollnosub, degree_code, batch_year, monthstr, year, collcode, subjectcode_Part1);
                                    table1forpagegpa.Cell(0, 0).SetContent(sumpart);
                                }
                                else
                                {
                                    table1forpagegpa.Cell(0, 0).SetContent("--");
                                }
                                partsums = 0;
                                partrowcount = 0;
                                creditstotal = 0;
                                table1forpagegpa.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                if (printds_rows.Tables[0].Rows.Count > 0)
                                {
                                    string sumpart = string.Empty;
                                    DataView dv_demand_data = new DataView();
                                    DataView dv_demand_datadummy = new DataView();
                                    printds_rows.Tables[0].DefaultView.RowFilter = "subject_type='Part II' and semester='" + current_semester + "'";
                                    dv_demand_datadummy = printds_rows.Tables[0].DefaultView;
                                    partrowcount = dv_demand_datadummy.Count;
                                    printds_rows.Tables[0].DefaultView.RowFilter = "subject_type='Part II' and result='pass' and semester='" + current_semester + "'";
                                    dv_demand_data = printds_rows.Tables[0].DefaultView;
                                    if (dv_demand_data.Count > 0 && partrowcount == dv_demand_data.Count)
                                    {
                                        //for (int sum = 0; sum < dv_demand_data.Count; sum++)
                                        //{
                                        //    partsums = partsums + Convert.ToDouble(dv_demand_data[sum]["total"].ToString());
                                        //}
                                        //partsums = (partsums / dv_demand_data.Count) / 10;
                                        //partsums = Math.Round(partsums, 2);
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
                                                    grade_points = Convert.ToDouble(dv_demand_data[sum]["total"].ToString());
                                                    grade_points = grade_points / 10;
                                                    Credit_Points = Convert.ToDouble(dv_demand_data[sum]["credit_points"].ToString());
                                                    creditstotal = creditstotal + Credit_Points;
                                                    partsums = partsums + (grade_points * Credit_Points);
                                                }
                                            }
                                        }
                                        //partsums = (partsums / creditstotal);
                                        //partsums = Math.Round(partsums, 2);
                                        //sumpart = String.Format("{0:0.00}", partsums);
                                        if (creditstotal == 0)
                                        {
                                            sumpart = "0.00";
                                        }
                                        else if (creditstotal > 0)
                                        {
                                            partsums = (partsums / creditstotal);
                                            partsums = Math.Round(partsums, 2);
                                            sumpart = String.Format("{0:0.00}", partsums);
                                        }
                                        else
                                        {
                                            sumpart = "0.00";
                                        }
                                    }
                                    else if (partrowcount > 0)
                                    {
                                        sumpart = "0.00";
                                    }
                                    else
                                    {
                                        sumpart = "--";
                                    }
                                    //cal_gpa = Calulat_GPA_forpart(rollnosub, degree_code, batch_year, monthstr, year, collcode, subjectcode_Part1);
                                    table1forpagegpa.Cell(0, 1).SetContent(sumpart);
                                }
                                else
                                {
                                    table1forpagegpa.Cell(0, 1).SetContent("--");
                                }
                                partsums = 0;
                                partrowcount = 0;
                                creditstotal = 0;
                                if (printds_rows.Tables[0].Rows.Count > 0)
                                {
                                    string sumpart = string.Empty;
                                    DataView dv_demand_data = new DataView();
                                    DataView dv_demand_datadummy = new DataView();
                                    printds_rows.Tables[0].DefaultView.RowFilter = "subject_type='Part III' and semester='" + current_semester + "'";
                                    dv_demand_datadummy = printds_rows.Tables[0].DefaultView;
                                    partrowcount = dv_demand_datadummy.Count;
                                    printds_rows.Tables[0].DefaultView.RowFilter = "subject_type='Part III' and result='pass' and semester='" + current_semester + "'";
                                    dv_demand_data = printds_rows.Tables[0].DefaultView;
                                    if (dv_demand_data.Count > 0 && partrowcount == dv_demand_data.Count)
                                    {
                                        //for (int sum = 0; sum < dv_demand_data.Count; sum++)
                                        //{
                                        //    partsums = partsums + Convert.ToDouble(dv_demand_data[sum]["total"].ToString());
                                        //}
                                        //partsums = (partsums / dv_demand_data.Count) / 10;
                                        //partsums = Math.Round(partsums, 2);
                                        //sumpart = String.Format("{0:0.00}", partsums);
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
                                                    grade_points = Convert.ToDouble(dv_demand_data[sum]["total"].ToString());
                                                    grade_points = grade_points / 10;
                                                    Credit_Points = Convert.ToDouble(dv_demand_data[sum]["credit_points"].ToString());
                                                    creditstotal = creditstotal + Credit_Points;
                                                    partsums = partsums + (grade_points * Credit_Points);
                                                }
                                            }
                                        }
                                        if (creditstotal == 0)
                                        {
                                            sumpart = "0.00";
                                        }
                                        else if (creditstotal > 0)
                                        {
                                            partsums = (partsums / creditstotal);
                                            partsums = Math.Round(partsums, 2);
                                            sumpart = String.Format("{0:0.00}", partsums);
                                        }
                                        else
                                        {
                                            sumpart = "0.00";
                                        }
                                    }
                                    else if (partrowcount > 0)
                                    {
                                        sumpart = "0.00";
                                    }
                                    else
                                    {
                                        sumpart = "--";
                                    }
                                    //cal_gpa = Calulat_GPA_forpart(rollnosub, degree_code, batch_year, monthstr, year, collcode, subjectcode_Part1);
                                    table1forpagegpa.Cell(0, 2).SetContent(sumpart);
                                }
                                else
                                {
                                    table1forpagegpa.Cell(0, 2).SetContent("--");
                                }
                                partsums = 0;
                                partrowcount = 0;
                                creditstotal = 0;
                                if (printds_rows.Tables[0].Rows.Count > 0)
                                {
                                    string sumpart = string.Empty;
                                    DataView dv_demand_data = new DataView();
                                    DataView dv_demand_datadummy = new DataView();
                                    printds_rows.Tables[0].DefaultView.RowFilter = "subject_type='Part IV' and semester='" + current_semester + "'";
                                    dv_demand_datadummy = printds_rows.Tables[0].DefaultView;
                                    partrowcount = dv_demand_datadummy.Count;
                                    printds_rows.Tables[0].DefaultView.RowFilter = "subject_type='Part IV' and result='pass' and semester='" + current_semester + "'";
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
                                                    grade_points = Convert.ToDouble(dv_demand_data[sum]["total"].ToString());
                                                    grade_points = grade_points / 10;
                                                    Credit_Points = Convert.ToDouble(dv_demand_data[sum]["credit_points"].ToString());
                                                    creditstotal = creditstotal + Credit_Points;
                                                    partsums = partsums + (grade_points * Credit_Points);
                                                }
                                            }
                                        }
                                        //partsums = (partsums / creditstotal);
                                        //partsums = Math.Round(partsums, 2);
                                        //sumpart = String.Format("{0:0.00}", partsums);
                                        if (creditstotal == 0)
                                        {
                                            sumpart = "0.00";
                                        }
                                        else if (creditstotal > 0)
                                        {
                                            partsums = (partsums / creditstotal);
                                            partsums = Math.Round(partsums, 2);
                                            sumpart = String.Format("{0:0.00}", partsums);
                                        }
                                        else
                                        {
                                            sumpart = "0.00";
                                        }
                                    }
                                    else if (partrowcount > 0)
                                    {
                                        sumpart = "0.00";
                                    }
                                    else
                                    {
                                        sumpart = "--";
                                    }
                                    //cal_gpa = Calulat_GPA_forpart(rollnosub, degree_code, batch_year, monthstr, year, collcode, subjectcode_Part1);
                                    table1forpagegpa.Cell(0, 3).SetContent(sumpart);
                                }
                                else
                                {
                                    table1forpagegpa.Cell(0, 3).SetContent("--");
                                }
                                newpdftabpagecoe = table1forpagegpa.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 148, 725, 376, 50));
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
            else if (ddlformate.SelectedIndex == 1)
            {
                bindformattwo();
                //RadioButton1.Visible = true;
                //RadioButton2.Visible = true;
                //RadioButton3.Visible = true;
                //RadioButton4.Visible = true;
                ddlformate.Visible = true;
            }
            else if (ddlformate.SelectedIndex == 2)
            {
                bindformatthree();
            }
            else if (ddlformate.SelectedIndex == 3)
            {
                bindformate4();
            }
            else if (ddlformate.SelectedIndex == 4)
            {
                bindformate5mcc();
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindformate5mcc()
    {
        DataSet printds = new DataSet();
        DataSet printds_new = new DataSet();
        DataSet printds_rows = new DataSet();
        string degree = string.Empty;
        string monthandyear = string.Empty;
        string studname = string.Empty;
        string dob = string.Empty;
        string rollnosub = string.Empty;
        string regnumber = string.Empty;
        string batch_year = string.Empty;
        string degree_code = string.Empty;
        string exam_code = string.Empty;
        string sem = string.Empty;
        //int max_internal_mark = 0;
        //int max_external_mark = 0;
        int colval = 0;
        string branch = string.Empty;
        int month = 0;
        string monthstr = string.Empty;
        string sql2 = string.Empty;
        string sql3 = string.Empty;
        string roman = string.Empty;
        string semroman = string.Empty;
        string grade = string.Empty;
        string gradepoints = string.Empty;
        string coe = string.Empty;
        string subjectcode_Part1 = string.Empty;
        string subjectcode_Part2 = string.Empty;
        string subjectcode_Part3 = string.Empty;
        string subjectcode_Part4 = string.Empty;
        string cal_gpa = string.Empty;
        string subtype = string.Empty;
        DataSet gradeds = new DataSet();
        Font Fontco12 = new Font("Comic Sans MS", 12, FontStyle.Bold);
        Font Fontpala12 = new Font("Palatino Linotype", 10, FontStyle.Bold);
        Font Fontco10 = new Font("Comic Sans MS", 10, FontStyle.Regular);
        Font Fontco12a = new Font("Comic Sans MS", 12, FontStyle.Regular);
        Font Fontarial7 = new Font("Arial", 7, FontStyle.Regular);
        Font Fontarial9 = new Font("Arial", 9, FontStyle.Regular);
        Font Fontarial10 = new Font("Arial", 10, FontStyle.Regular);
        Font Fontarial10b = new Font("Arial", 10, FontStyle.Bold);
        Font Fontarial12 = new Font("Arial", 12, FontStyle.Regular);
        //Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
        Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
        // Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(20.99, 29.7));
        // Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InInches(595, 842));
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
                    string sql = "SELECT Reg_No,r.batch_year,r.app_no,r.degree_code,R.current_semester,R.Stud_Name,Course_Name as degree,Dept_name,CONVERT(VARCHAR, dob, 103) as dob FROM Registration R,Applyn A,Degree G,Course C,Department D WHERE R.App_No = A.App_No AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND D.College_Code = G.College_Code AND Roll_No = '" + rollnosub + "' and r.college_code='" + Session["collegecode"].ToString() + "' ;  select coe from collinfo where college_code='" + Session["collegecode"].ToString() + "';";
                    printds = da.select_method_wo_parameter(sql, "text");
                    DataTable dtProjectPaper = new DataTable();
                    string qry = " select distinct r.ID,AppNo,sm.semester,s.subject_code SubjectNo,ProjectPaperName from ProjectPaperDetails prj,subject s,syllabus_master sm,Registration r where r.App_No=prj.appNo and sm.syll_code=s.syll_code and s.subject_no=prj.SubjectNo  AND r.Roll_No = '" + rollnosub + "'";//and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "'
                    dtProjectPaper = da.select_method_wop_table(qry, "text");
                    if (printds.Tables.Count > 0 && printds.Tables[0].Rows.Count > 0)
                    {
                        mypdfpage = mydoc.NewPage();
                        degree = printds.Tables[0].Rows[0]["degree"].ToString();
                        coe = printds.Tables[1].Rows[0]["coe"].ToString();
                        //string[] spiltcoe = coe.Split('.');
                        //string newcoe = string.Empty;
                        //for (int ce = 1; ce <= spiltcoe.GetUpperBound(0); ce++)
                        //{
                        //    if (newcoe == "")
                        //    {
                        //        newcoe = spiltcoe[ce].ToString();
                        //    }
                        //    else
                        //    {
                        //        newcoe = newcoe + "." + spiltcoe[ce].ToString();
                        //    }
                        //}
                        //coe = newcoe;
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
                        sql2 = "select * from exam_details where    degree_code='" + degree_code + "' and current_semester='" + sem + "' and exam_month='" + month + "' and exam_year='" + ddlYear.SelectedItem.Text + "'";
                        sql2 = "select * from exam_details where    degree_code='" + degree_code + "'  and exam_month='" + month + "' and exam_year='" + ddlYear.SelectedItem.Text + "' and  batch_year=" + batch_year + "";
                        printds_new = da.select_method_wo_parameter(sql2, "Text");
                        if (printds_new.Tables[0].Rows.Count > 0)
                        {
                            sem = printds_new.Tables[0].Rows[0]["current_semester"].ToString();
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
                            PdfTextArea pdfdoi = new PdfTextArea(Fontco10, System.Drawing.Color.Black, new PdfArea(mydoc, 71, 810, 305, 30), System.Drawing.ContentAlignment.TopLeft, txtdoi.Text.ToString());
                            mypdfpage.Add(pdfdoi);
                            exam_code = printds_new.Tables[0].Rows[0]["exam_code"].ToString();
                            sql3 = "Select ss.CommSubcode,ss.Subject_Type,s.subject_name,s.subject_code,s.subject_no,m.result,s.max_int_marks,s.max_ext_marks,m.internal_mark,m.external_mark,m.total,s.maxtotal,m.grade,m.cp,s.credit_points,m.subject_no,sm.semester,s.maxtotal,m.exam_code,s.subjectpriority from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm where sm.syll_code=s.syll_code and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and  m.Exam_Code ='" + exam_code + "' and m.roll_no='" + rollnosub + "'   order by sm.semester,s.subjectpriority";
                            printds_rows.Clear();
                            printds_rows.Dispose();
                            printds_rows = da.select_method_wo_parameter(sql3, "Text");
                            strMonthName = ddlMonth.SelectedItem.Text;
                            Gios.Pdf.PdfTable table1forpage2 = mydoc.NewTable(Fontarial10b, 1, 6, 1);
                            table1forpage2.VisibleHeaders = false;
                            table1forpage2.SetBorders(Color.Black, 1, BorderType.None);
                            table1forpage2.SetColumnsWidth(new int[] { 185, 85, 88, 60, 57, 60 });
                            table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            studname = "  " + studname;
                            table1forpage2.Cell(0, 0).SetContent(studname);
                            table1forpage2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(0, 1).SetContent(branch);
                            table1forpage2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 2).SetContent(regnumber);
                            table1forpage2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 3).SetContent(strMonthName.ToUpper());
                            table1forpage2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 4).SetContent(ddlYear.SelectedItem.Text);
                            table1forpage2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 5).SetContent(semroman);
                            Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 18, 150, 565, 385));
                            mypdfpage.Add(newpdftabpage2);
                            colval = 200;
                            if (printds_rows.Tables.Count > 0 && printds_rows.Tables[0].Rows.Count > 0)
                            {
                                //
                                Gios.Pdf.PdfTable table1marks = mydoc.NewTable(Fontarial10, printds_rows.Tables[0].Rows.Count + 1, 9, 2);
                                table1marks.VisibleHeaders = false;
                                table1marks.SetBorders(Color.Black, 1, BorderType.None);
                                table1marks.SetColumnsWidth(new int[] { 71, 213, 40, 40, 40, 40, 28, 40, 45 });
                                for (int i = 0; i < printds_rows.Tables[0].Rows.Count; i++)
                                {
                                    if (printds_new.Tables[0].Rows.Count > 0)
                                    {
                                        if (printds_rows.Tables[0].Rows[i]["CommSubcode"].ToString().Trim() != "" && printds_rows.Tables[0].Rows[i]["CommSubcode"].ToString().Trim() != null)
                                        {
                                            table1marks.Cell(i, 0).SetContent(printds_rows.Tables[0].Rows[i]["CommSubcode"].ToString());
                                        }
                                        else
                                        {
                                            table1marks.Cell(i, 0).SetContent(printds_rows.Tables[0].Rows[i]["subject_code"].ToString());
                                        }

                                        string subjectName = string.Empty;
                                        DataTable dtSubjectName = new DataTable();
                                        subjectName = Convert.ToString(printds_rows.Tables[0].Rows[i]["subject_name"]).Trim();
                                        if (dtProjectPaper.Rows.Count > 0)
                                        {
                                            dtProjectPaper.DefaultView.RowFilter = "SubjectNo='" + Convert.ToString(printds_rows.Tables[0].Rows[i]["subject_code"]).Trim() + "'";
                                            dtSubjectName = dtProjectPaper.DefaultView.ToTable();
                                            if (dtSubjectName.Rows.Count > 0)
                                            {
                                                subjectName = Convert.ToString(dtSubjectName.Rows[0]["ProjectPaperName"]).Trim();
                                            }
                                        }
                                        table1marks.Cell(i, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                        table1marks.Cell(i, 1).SetContent(subjectName);
                                        table1marks.Cell(i, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1marks.Cell(i, 2).SetContent(printds_rows.Tables[0].Rows[i]["credit_points"].ToString());
                                        table1marks.Cell(i, 2).SetContentAlignment(ContentAlignment.TopCenter);
                                        table1marks.Cell(i, 3).SetContent(printds_rows.Tables[0].Rows[i]["internal_mark"].ToString());
                                        table1marks.Cell(i, 3).SetContentAlignment(ContentAlignment.TopCenter);
                                        table1marks.Cell(i, 4).SetContent(printds_rows.Tables[0].Rows[i]["external_mark"].ToString());
                                        table1marks.Cell(i, 4).SetContentAlignment(ContentAlignment.TopCenter);
                                        //table1marks.Cell(i, 5).SetContent(printds_rows.Tables[0].Rows[i]["total"].ToString());
                                        //table1marks.Cell(i, 5).SetContentAlignment(ContentAlignment.TopCenter);
                                        double totfinal = Convert.ToDouble(printds_rows.Tables[0].Rows[i]["total"].ToString());
                                        totfinal = Math.Round(totfinal, 0);
                                        table1marks.Cell(i, 5).SetContent(totfinal);
                                        table1marks.Cell(i, 5).SetContentAlignment(ContentAlignment.TopCenter);
                                        double checkmarkmm = Convert.ToDouble(printds_rows.Tables[0].Rows[i]["total"].ToString());
                                        string gradesql = "    select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='" + sem + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 16/aug 2014
                                        gradeds.Clear();
                                        gradeds = da.select_method_wo_parameter(gradesql, "Text");
                                        //Hashtable hat = new Hashtable();
                                        if (gradeds.Tables[0].Rows.Count == 0)
                                        {
                                            gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and Degree_Code='" + degree_code + "' ";//added by sridhar //and batch_year='" + batch_year + "'
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
                                        gradeibtpoint = Math.Round(gradeibtpoint, 1, MidpointRounding.AwayFromZero);
                                        //gradepoints = Convert.ToString(gradeibtpoint);
                                        gradepoints = String.Format("{0:0.0}", gradeibtpoint);
                                        string result = printds_rows.Tables[0].Rows[i]["result"].ToString().ToLower();
                                        if (result.Trim().ToLower() == "fail")
                                        {
                                            result = "RA";
                                            grade = "U";
                                        }
                                        else if (result.Trim().ToLower() == "pass")
                                        {
                                            result = "Pass";
                                        }
                                        else
                                        {
                                            result = "AAA";
                                            grade = "U";
                                        }
                                        table1marks.Cell(i, 6).SetContentAlignment(ContentAlignment.TopCenter);
                                        table1marks.Cell(i, 6).SetContent(gradepoints);
                                        table1marks.Cell(i, 7).SetContentAlignment(ContentAlignment.TopCenter);
                                        table1marks.Cell(i, 7).SetContent(grade);
                                        table1marks.Cell(i, 8).SetContentAlignment(ContentAlignment.TopCenter);
                                        table1marks.Cell(i, 8).SetContent(result);
                                    }
                                }
                                table1marks.Cell(printds_rows.Tables[0].Rows.Count, 1).SetContentAlignment(ContentAlignment.TopCenter);
                                table1marks.Cell(printds_rows.Tables[0].Rows.Count, 1).SetContent("--END OF STATEMENT--");
                                Gios.Pdf.PdfTablePage newpdftable1table1marks = table1marks.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 18, 218, 565, 385));
                                mypdfpage.Add(newpdftable1table1marks);
                            }
                        }
                        Gios.Pdf.PdfTable table1forpagecoe = mydoc.NewTable(Fontarial9, 1, 1, 1);
                        table1forpagecoe.VisibleHeaders = false;
                        table1forpagecoe.SetBorders(Color.Black, 1, BorderType.None);
                        table1forpagecoe.SetColumnsWidth(new int[] { 190 });
                        table1forpagecoe.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpagecoe.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        //table1forpagecoe.Cell(0, 0).SetContent(coe);
                        Gios.Pdf.PdfTablePage newpdftabpagecoe = table1forpagecoe.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 425, 800, 190, 50));
                        mypdfpage.Add(newpdftabpagecoe);
                        string year = ddlYear.SelectedItem.Text;
                        string collcode = Session["collegecode"].ToString();
                        Gios.Pdf.PdfTable table1forpagegpa = mydoc.NewTable(Fontarial10, 1, 3, 1);
                        table1forpagegpa.VisibleHeaders = false;
                        table1forpagegpa.SetBorders(Color.Black, 1, BorderType.None);
                        // table1forpagegpa.SetColumnsWidth(new int[] { 184, 213, 40 });
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
                            string sumpart = string.Empty;
                            DataView dv_demand_data = new DataView();
                            DataView dv_demand_datadummy = new DataView();
                            printds_rows.Tables[0].DefaultView.RowFilter = "subject_type<>''";
                            dv_demand_datadummy = printds_rows.Tables[0].DefaultView;
                            partrowcount = dv_demand_datadummy.Count;
                            printds_rows.Tables[0].DefaultView.RowFilter = "Subject_Type<>'' and result='pass'";
                            dv_demand_data = printds_rows.Tables[0].DefaultView;
                            Hashtable hat = new Hashtable();
                            if (dv_demand_data.Count > 0 && partrowcount == dv_demand_data.Count)
                            {
                                for (int sum = 0; sum < dv_demand_data.Count; sum++)
                                {
                                    double checkmarkmm = Convert.ToDouble(dv_demand_data[sum]["total"].ToString());
                                    string subNo = Convert.ToString(dv_demand_data[sum]["subject_no"].ToString());
                                    string gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='" + sem + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 16/aug 2014
                                    gradeds.Clear();
                                    gradeds = da.select_method_wo_parameter(gradesql, "Text");
                                    if (gradeds.Tables[0].Rows.Count == 0)
                                    {
                                        gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and Degree_Code='" + degree_code + "'";//added by sridhar / and batch_year='" + batch_year + "'
                                        gradeds.Clear();
                                        gradeds = da.select_method_wo_parameter(gradesql, "Text");
                                    }
                                    for (int grd = 0; grd < gradeds.Tables[0].Rows.Count; grd++)
                                    {
                                        if (Convert.ToInt32(gradeds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(gradeds.Tables[0].Rows[grd][2]) >= checkmarkmm)
                                        {
                                            if (!hat.ContainsKey(subNo))
                                            {
                                                Credit_Points = Convert.ToDouble(dv_demand_data[sum]["Credit_Points"].ToString());
                                                hat.Add(subNo, Credit_Points);
                                                // grade_points = grade_points / 10;
                                                grade_points = Convert.ToDouble(gradeds.Tables[0].Rows[grd]["Credit_Points"].ToString());
                                                creditstotal = creditstotal + Credit_Points;
                                                partsums = partsums + (grade_points * Credit_Points);
                                                wpm = wpm + (Convert.ToDouble(dv_demand_data[sum]["total"].ToString()) * Credit_Points);
                                            }
                                        }
                                    }
                                }
                                partsums = Math.Round(partsums, 2);
                                sumpart = String.Format("{0:0.00}", partsums);
                                finalgpa1 = Math.Round((partsums / creditstotal), 2, MidpointRounding.AwayFromZero);
                                finalwpm = Math.Round((wpm / creditstotal), 2, MidpointRounding.AwayFromZero);
                                if (creditstotal >= 0)
                                {
                                    table1forpagegpa.Cell(0, 0).SetContent(creditstotal);
                                }
                                else
                                {
                                    table1forpagegpa.Cell(0, 1).SetContent(0);
                                }
                                if (finalwpm >= 0)
                                {
                                    string wam = String.Format("{0:0.00}", finalwpm);
                                    table1forpagegpa.Cell(0, 1).SetContent(wam);
                                }
                                else
                                {
                                    table1forpagegpa.Cell(0, 1).SetContent("0.00");
                                }
                                if (finalgpa1 >= 0)
                                {
                                    //string finalgg=Convert.ToString(finalgpa1);
                                    sumpart = String.Format("{0:0.0}", finalgpa1);
                                    table1forpagegpa.Cell(0, 2).SetContent(sumpart);
                                }
                                else
                                {
                                    table1forpagegpa.Cell(0, 2).SetContent("0.0");
                                }
                            }
                            else
                            {
                                table1forpagegpa.Cell(0, 0).SetContent("-");
                                table1forpagegpa.Cell(0, 1).SetContent("-");
                                table1forpagegpa.Cell(0, 2).SetContent("-");
                                table1forpagegpa.Cell(0, 2).SetContent("-");
                            }
                            //cal_gpa = Calulat_GPA_forpart(rollnosub, degree_code, batch_year, monthstr, year, collcode, subjectcode_Part1);
                        }
                        else
                        {
                            //table1forpagegpa.Cell(0, 0).SetContent("-");
                        }
                        //partsums = 0;
                        //partrowcount = 0;
                        //creditstotal = 0;
                        //finalgpa1 = 0;
                        //finalwpm = 0;
                        //table1forpagegpa.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        //if (printds_rows.Tables[0].Rows.Count > 0)
                        //{
                        //    string sumpart = string.Empty;
                        //    DataView dv_demand_data = new DataView();
                        //    DataView dv_demand_datadummy = new DataView();
                        //    printds_rows.Tables[0].DefaultView.RowFilter = "subject_type='Part II'";
                        //    dv_demand_datadummy = printds_rows.Tables[0].DefaultView;
                        //    partrowcount = dv_demand_datadummy.Count;
                        //    printds_rows.Tables[0].DefaultView.RowFilter = "subject_type='Part II' and result='pass'";
                        //    dv_demand_data = printds_rows.Tables[0].DefaultView;
                        //    if (dv_demand_data.Count > 0 && partrowcount == dv_demand_data.Count)
                        //    {
                        //        for (int sum = 0; sum < dv_demand_data.Count; sum++)
                        //        {
                        //            double checkmarkmm = Convert.ToDouble(dv_demand_data[sum]["total"].ToString());
                        //            string gradesql = "    select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='" + sem + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 16/aug 2014
                        //            gradeds.Clear();
                        //            gradeds = da.select_method_wo_parameter(gradesql, "Text");
                        //            if (gradeds.Tables[0].Rows.Count == 0)
                        //            {
                        //                gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 
                        //                gradeds.Clear();
                        //                gradeds = da.select_method_wo_parameter(gradesql, "Text");
                        //            }
                        //            for (int grd = 0; grd < gradeds.Tables[0].Rows.Count; grd++)
                        //            {
                        //                if (Convert.ToInt32(gradeds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(gradeds.Tables[0].Rows[grd][2]) >= checkmarkmm)
                        //                {
                        //                    Credit_Points = Convert.ToDouble(dv_demand_data[sum]["Credit_Points"].ToString());
                        //                    // grade_points = grade_points / 10;
                        //                    grade_points = Convert.ToDouble(gradeds.Tables[0].Rows[grd]["Credit_Points"].ToString());
                        //                    creditstotal = creditstotal + Credit_Points;
                        //                    partsums = partsums + (grade_points * Credit_Points);
                        //                    wpm = wpm + partsums + (Convert.ToDouble(dv_demand_data[sum]["total"].ToString()) * Credit_Points);
                        //                }
                        //            }
                        //        }
                        //        partsums = Math.Round(partsums, 2);
                        //        sumpart = String.Format("{0:0.00}", partsums);
                        //    }
                        //    else if (partrowcount > 0)
                        //    {
                        //        sumpart = "0.00";
                        //    }
                        //    else
                        //    {
                        //        sumpart = "-";
                        //    }
                        //    //cal_gpa = Calulat_GPA_forpart(rollnosub, degree_code, batch_year, monthstr, year, collcode, subjectcode_Part1);
                        //    finalgpa1 = Math.Round((partsums / creditstotal), 2, MidpointRounding.AwayFromZero);
                        //    finalwpm = Math.Round((wpm / creditstotal), 2, MidpointRounding.AwayFromZero);
                        //    table1forpagegpa.Cell(1, 1).SetContent(creditstotal);
                        //    table1forpagegpa.Cell(1, 2).SetContent(finalwpm);
                        //    table1forpagegpa.Cell(1, 3).SetContent(finalgpa1);
                        //}
                        //else
                        //{
                        //    // table1forpagegpa.Cell(0, 1).SetContent("-");
                        //}
                        //partsums = 0;
                        //partrowcount = 0;
                        //creditstotal = 0;
                        //finalgpa1 = 0;
                        //finalwpm = 0;
                        //if (printds_rows.Tables[0].Rows.Count > 0)
                        //{
                        //    string sumpart = string.Empty;
                        //    DataView dv_demand_data = new DataView();
                        //    DataView dv_demand_datadummy = new DataView();
                        //    printds_rows.Tables[0].DefaultView.RowFilter = "subject_type='Part III'";
                        //    dv_demand_datadummy = printds_rows.Tables[0].DefaultView;
                        //    partrowcount = dv_demand_datadummy.Count;
                        //    printds_rows.Tables[0].DefaultView.RowFilter = "subject_type='Part III' and result='pass'";
                        //    dv_demand_data = printds_rows.Tables[0].DefaultView;
                        //    if (dv_demand_data.Count > 0 && partrowcount == dv_demand_data.Count)
                        //    {
                        //        for (int sum = 0; sum < dv_demand_data.Count; sum++)
                        //        {
                        //            double checkmarkmm = Convert.ToDouble(dv_demand_data[sum]["total"].ToString());
                        //            string gradesql = "    select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='" + sem + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 16/aug 2014
                        //            gradeds.Clear();
                        //            gradeds = da.select_method_wo_parameter(gradesql, "Text");
                        //            if (gradeds.Tables[0].Rows.Count == 0)
                        //            {
                        //                gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 
                        //                gradeds.Clear();
                        //                gradeds = da.select_method_wo_parameter(gradesql, "Text");
                        //            }
                        //            for (int grd = 0; grd < gradeds.Tables[0].Rows.Count; grd++)
                        //            {
                        //                if (Convert.ToInt32(gradeds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(gradeds.Tables[0].Rows[grd][2]) >= checkmarkmm)
                        //                {
                        //                    Credit_Points = Convert.ToDouble(dv_demand_data[sum]["Credit_Points"].ToString());
                        //                    // grade_points = grade_points / 10;
                        //                    grade_points = Convert.ToDouble(gradeds.Tables[0].Rows[grd]["Credit_Points"].ToString());
                        //                    creditstotal = creditstotal + Credit_Points;
                        //                    partsums = partsums + (grade_points * Credit_Points);
                        //                    wpm = wpm + partsums + (Convert.ToDouble(dv_demand_data[sum]["total"].ToString()) * Credit_Points);
                        //                }
                        //            }
                        //        }
                        //        partsums = Math.Round(partsums, 2);
                        //        sumpart = String.Format("{0:0.00}", partsums);
                        //    }
                        //    else if (partrowcount > 0)
                        //    {
                        //        sumpart = "0.00";
                        //    }
                        //    else
                        //    {
                        //        sumpart = "-";
                        //    }
                        //    //cal_gpa = Calulat_GPA_forpart(rollnosub, degree_code, batch_year, monthstr, year, collcode, subjectcode_Part1);
                        //    finalgpa1 = Math.Round((partsums / creditstotal), 2, MidpointRounding.AwayFromZero);
                        //    finalwpm = Math.Round((wpm / creditstotal), 2, MidpointRounding.AwayFromZero);
                        //    table1forpagegpa.Cell(0, 5).SetContent(creditstotal);
                        //    table1forpagegpa.Cell(0, 6).SetContent(finalwpm);
                        //    table1forpagegpa.Cell(0, 7).SetContent(finalgpa1);
                        //}
                        //else
                        //{
                        //    //table1forpagegpa.Cell(0, 2).SetContent("-");
                        //}
                        //partsums = 0;
                        //partrowcount = 0;
                        //creditstotal = 0;
                        //finalgpa1 = 0;
                        //finalwpm = 0;
                        //if (printds_rows.Tables[0].Rows.Count > 0)
                        //{
                        //    string sumpart = string.Empty;
                        //    DataView dv_demand_data = new DataView();
                        //    DataView dv_demand_datadummy = new DataView();
                        //    printds_rows.Tables[0].DefaultView.RowFilter = "subject_type='Part IV'";
                        //    dv_demand_datadummy = printds_rows.Tables[0].DefaultView;
                        //    partrowcount = dv_demand_datadummy.Count;
                        //    printds_rows.Tables[0].DefaultView.RowFilter = "subject_type='Part IV' and result='pass'";
                        //    dv_demand_data = printds_rows.Tables[0].DefaultView;
                        //    if (dv_demand_data.Count > 0 && partrowcount == dv_demand_data.Count)
                        //    {
                        //        for (int sum = 0; sum < dv_demand_data.Count; sum++)
                        //        {
                        //            double checkmarkmm = Convert.ToDouble(dv_demand_data[sum]["total"].ToString());
                        //            string gradesql = "    select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='" + sem + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 16/aug 2014
                        //            gradeds.Clear();
                        //            gradeds = da.select_method_wo_parameter(gradesql, "Text");
                        //            if (gradeds.Tables[0].Rows.Count == 0)
                        //            {
                        //                gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 
                        //                gradeds.Clear();
                        //                gradeds = da.select_method_wo_parameter(gradesql, "Text");
                        //            }
                        //            for (int grd = 0; grd < gradeds.Tables[0].Rows.Count; grd++)
                        //            {
                        //                if (Convert.ToInt32(gradeds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(gradeds.Tables[0].Rows[grd][2]) >= checkmarkmm)
                        //                {
                        //                    Credit_Points = Convert.ToDouble(dv_demand_data[sum]["Credit_Points"].ToString());
                        //                    // grade_points = grade_points / 10;
                        //                    grade_points = Convert.ToDouble(gradeds.Tables[0].Rows[grd]["Credit_Points"].ToString());
                        //                    creditstotal = creditstotal + Credit_Points;
                        //                    partsums = partsums + (grade_points * Credit_Points);
                        //                    wpm = wpm + partsums + (Convert.ToDouble(dv_demand_data[sum]["total"].ToString()) * Credit_Points);
                        //                }
                        //            }
                        //        }
                        //        partsums = Math.Round(partsums, 2);
                        //        sumpart = String.Format("{0:0.00}", partsums);
                        //    }
                        //    else if (partrowcount > 0)
                        //    {
                        //        sumpart = "0.00";
                        //    }
                        //    else
                        //    {
                        //        sumpart = "-";
                        //    }
                        //    //cal_gpa = Calulat_GPA_forpart(rollnosub, degree_code, batch_year, monthstr, year, collcode, subjectcode_Part1);
                        //    finalgpa1 = Math.Round((partsums / creditstotal), 2, MidpointRounding.AwayFromZero);
                        //    finalwpm = Math.Round((wpm / creditstotal), 2, MidpointRounding.AwayFromZero);
                        //    table1forpagegpa.Cell(1, 5).SetContent(creditstotal);
                        //    table1forpagegpa.Cell(1, 6).SetContent(finalwpm);
                        //    table1forpagegpa.Cell(1, 7).SetContent(finalgpa1);
                        //}
                        //else
                        //{ 
                        //    // table1forpagegpa.Cell(0, 3).SetContent("-");
                        //}
                        newpdftabpagecoe = table1forpagegpa.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 18, 625, 565, 100));
                        mypdfpage.Add(newpdftabpagecoe);
                        Gios.Pdf.PdfTable table1forlast = mydoc.NewTable(Fontarial10, 4, 2, 3);
                        table1forlast.VisibleHeaders = false;
                        table1forlast.SetBorders(Color.Black, 1, BorderType.None);
                        table1forlast.SetColumnsWidth(new int[] { 452, 101 });
                        table1forlast.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forlast.Cell(0, 1).SetContent("");
                        table1forlast.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forlast.Cell(1, 1).SetContent("");
                        table1forlast.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forlast.Cell(2, 1).SetContent("50%");
                        table1forlast.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forlast.Cell(3, 1).SetContent("");
                        Gios.Pdf.PdfTablePage newpdftabpagetable1forlast = table1forlast.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 18, 678, 565, 369));
                        mypdfpage.Add(newpdftabpagetable1forlast);
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

    public void bindformate4()
    {
        DataSet printds = new DataSet();
        DataSet printds_new = new DataSet();
        DataSet printds_rows = new DataSet();
        string mode = string.Empty;
        string degree = string.Empty;
        string monthandyear = string.Empty;
        string studname = string.Empty;
        string dob = string.Empty;
        string rollnosub = string.Empty;
        string regnumber = string.Empty;
        string batch_year = string.Empty;
        string degree_code = string.Empty;
        string exam_code = string.Empty;
        string sem = string.Empty;
        //int max_internal_mark = 0;
        //int max_external_mark = 0;
        int colval = 0;
        string branch = string.Empty;
        int month = 0;
        string monthstr = string.Empty;
        string sql2 = string.Empty;
        string sql3 = string.Empty;
        string roman = string.Empty;
        string semroman = string.Empty;
        string grade = string.Empty;
        string gradepoints = string.Empty;
        string coe = string.Empty;
        string subjectcode_Part1 = string.Empty;
        string subjectcode_Part2 = string.Empty;
        string subjectcode_Part3 = string.Empty;
        string subjectcode_Part4 = string.Empty;
        string cal_gpa = string.Empty;
        string subtype = string.Empty;
        DataSet gradeds = new DataSet();
        Font fontcal11 = new Font("Calibri (Body)", 11, FontStyle.Bold);
        Font fontcal14 = new Font("Calibri (Body)", 14, FontStyle.Bold);
        Font fontcal8 = new Font("Calibri (Body)", 8, FontStyle.Bold);
        // Font Fontco12 = new Font("Calibri (Body)", 11, FontStyle.Bold);
        //Font Fontpala12 = new Font("Palatino Linotype", 10, FontStyle.Bold);
        //Font Fontco10 = new Font("Comic Sans MS", 10, FontStyle.Regular);
        //Font Fontco12a = new Font("Comic Sans MS", 12, FontStyle.Regular);
        //Font Fontarial7 = new Font("Arial", 7, FontStyle.Regular);
        //Font Fontarial9 = new Font("Arial", 9, FontStyle.Regular);
        //Font Fontarial10 = new Font("Arial", 10, FontStyle.Regular);
        //Font Fontarial12 = new Font("Arial", 12, FontStyle.Regular);
        //Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
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
                    string sql = "SELECT Reg_No,r.batch_year,r.app_no,r.degree_code,R.current_semester,R.Stud_Name,Course_Name as degree,Dept_name,CONVERT(VARCHAR, dob, 103) as dob,r.mode FROM Registration R,Applyn A,Degree G,Course C,Department D WHERE R.App_No = A.App_No AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND D.College_Code = G.College_Code AND Roll_No = '" + rollnosub + "' and r.college_code='" + Session["collegecode"].ToString() + "' ;  select coe from collinfo where college_code='" + Session["collegecode"].ToString() + "';";
                    //dummy
                    // sql = "SELECT Reg_No,r.batch_year,r.degree_code,R.current_semester,R.Stud_Name,Course_Name+'('+Dept_acronym+')' as degree,Dept_name,CONVERT(VARCHAR, dob, 103) as dob FROM Registration R,Applyn A,Degree G,Course C,Department D WHERE R.App_No = A.App_No AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND D.College_Code = G.College_Code AND Roll_No = '14UHI6001' ;  select coe from collinfo";
                    printds = da.select_method_wo_parameter(sql, "Text");
                    if (printds.Tables[0].Rows.Count > 0)
                    {
                        mypdfpage = mydoc.NewPage();
                        degree = printds.Tables[0].Rows[0]["degree"].ToString();
                        coe = printds.Tables[1].Rows[0]["coe"].ToString();
                        string[] spiltcoe = coe.Split('.');
                        string newcoe = string.Empty;
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
                        mode = printds.Tables[0].Rows[0]["mode"].ToString();
                        string stdappno = printds.Tables[0].Rows[0]["App_No"].ToString();
                        batch_year = printds.Tables[0].Rows[0]["batch_year"].ToString();
                        degree_code = printds.Tables[0].Rows[0]["degree_code"].ToString();
                        sql2 = "select * from exam_details where    degree_code='" + degree_code + "' and current_semester='" + sem + "' and exam_month='" + month + "' and exam_year='" + ddlYear.SelectedItem.Text + "'";
                        sql2 = "select * from exam_details where    degree_code='" + degree_code + "'  and exam_month='" + month + "' and exam_year='" + ddlYear.SelectedItem.Text + "' and  batch_year=" + batch_year + "";
                        //dummy
                        // sql2 = "select * from exam_details where  batch_year='2014' and   degree_code='45' and current_semester='1' and exam_month='11' and exam_year='2014'";
                        printds_new = da.select_method_wo_parameter(sql2, "Text");
                        if (printds_new.Tables[0].Rows.Count > 0)
                        {
                            sem = printds_new.Tables[0].Rows[0]["current_semester"].ToString();
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
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg")))
                            {
                                Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg"));
                                mypdfpage.Add(LogoImage2, 454, 80, 500);
                            }
                            else
                            {
                                Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg"));
                                mypdfpage.Add(LogoImage2, 454, 80, 500);
                            }
                            //PdfTextArea pdfdoi = new PdfTextArea(Fontco10, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 664, 305, 50), System.Drawing.ContentAlignment.TopLeft, txtdoi.Text.ToString());
                            //mypdfpage.Add(pdfdoi);
                            exam_code = printds_new.Tables[0].Rows[0]["exam_code"].ToString();
                            sql3 = "Select distinct Subject_Type,subject_name,subject_code,subject.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,maxtotal,grade,cp,credit_points,mark_entry.subject_no,semester,maxtotal,exam_code from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = '" + exam_code + "' and roll_no='" + rollnosub + "'   order by subject_code,subject_type";
                            //dummy
                            // sql3 = "Select Subject_Type,subject_name,subject_code,subject.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,maxtotal,grade,cp,credit_points,mark_entry.subject_no,semester,maxtotal,exam_code from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = 10146 and roll_no='14UHI6001'  order by LEN(subject_type),subject_type";
                            printds_rows.Clear();
                            printds_rows.Dispose();
                            printds_rows = da.select_method_wo_parameter(sql3, "Text");
                            //Gios.Pdf.PdfTable table1forpage1 = mydoc.NewTable(Fontco12, 1, 3, 1);
                            //table1forpage1.VisibleHeaders = false;
                            //table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                            //table1forpage1.SetColumnsWidth(new int[] { 91, 157, 119 });
                            //table1forpage1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage1.Cell(0, 0).SetContent(degree);
                            //table1forpage1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage1.Cell(0, 1).SetContent("Degree Examinations");
                            //table1forpage1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage1.Cell(0, 2).SetContent(monthandyear);
                            //Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 136, 55, 366, 50));
                            //mypdfpage.Add(newpdftabpage2);
                            string strexam = degree + "  " + "Degree Examinations" + "  " + monthandyear;
                            PdfTextArea pdfexam = new PdfTextArea(fontcal14, System.Drawing.Color.Black, new PdfArea(mydoc, 38, 30, 500, 50), System.Drawing.ContentAlignment.MiddleCenter, strexam);//17
                            mypdfpage.Add(pdfexam);
                            //Gios.Pdf.PdfTable table1forpage2 = mydoc.NewTable(Fontco10, 1, 3, 1);
                            //table1forpage2.VisibleHeaders = false;
                            //table1forpage2.SetBorders(Color.Black, 1, BorderType.None);
                            //table1forpage2.SetColumnsWidth(new int[] { 354, 108, 108 });
                            //table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage2.Cell(0, 0).SetContent(studname);
                            //table1forpage2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage2.Cell(0, 1).SetContent(dob);
                            //table1forpage2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage2.Cell(0, 2).SetContent(regnumber);
                            //Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 27, 108, 567, 50));//sr
                            //mypdfpage.Add(newpdftabpage2);
                            //Gios.Pdf.PdfTable table1forpage2a = mydoc.NewTable(Fontco10, 1, 4, 1);
                            //table1forpage2a.Columns[0].SetWidth(354);
                            //table1forpage2a.VisibleHeaders = false;
                            //table1forpage2a.SetBorders(Color.Black, 1, BorderType.None);
                            //table1forpage2a.SetColumnsWidth(new int[] { 298, 56, 108, 108 });
                            //table1forpage2a.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage2a.Cell(0, 0).SetContent(branch);
                            //table1forpage2a.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage2a.Cell(0, 1).SetContent(semroman);
                            //table1forpage2a.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage2a.Cell(0, 2).SetContent(monthandyear);
                            //table1forpage2a.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage2a.Cell(0, 3).SetContent(txtdop.Text);
                            //newpdftabpage2 = table1forpage2a.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 27, 143, 567, 50));
                            //mypdfpage.Add(newpdftabpage2);
                            generatefolio();
                            string generate = da.GetFunction("select value from Master_Settings where settings='Consolidate Sheet' ");
                            string[] split = generate.Split('-');
                            if (split.GetUpperBound(0) > 0)
                            {
                                generate = split[0].ToString() + split[1].ToString();
                            }
                            else
                            {
                                generate = "-";
                            }
                            Gios.Pdf.PdfTable table1forpage2 = mydoc.NewTable(fontcal11, 5, 1, 3);
                            table1forpage2.VisibleHeaders = false;
                            table1forpage2.SetBorders(Color.Black, 1, BorderType.None);
                            table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(0, 0).SetContent(studname);
                            table1forpage2.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(1, 0).SetContent(dob);
                            table1forpage2.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(2, 0).SetContent(regnumber);
                            table1forpage2.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(3, 0).SetContent(branch);
                            table1forpage2.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(4, 0).SetContent(generate);
                            Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 142, 88, 567, 150));//sr
                            mypdfpage.Add(newpdftabpage2);
                            colval = 200;
                            if (printds_rows.Tables[0].Rows.Count > 0)
                            {
                                table1forpage2 = mydoc.NewTable(fontcal8, printds_rows.Tables[0].Rows.Count, 6, 3);
                                table1forpage2.VisibleHeaders = false;
                                table1forpage2.SetBorders(Color.Black, 1, BorderType.None);
                                table1forpage2.SetColumnsWidth(new int[] { 35, 57, 283, 35, 35, 35 });
                                for (int i = 0; i < printds_rows.Tables[0].Rows.Count; i++)
                                {
                                    if (printds_new.Tables[0].Rows.Count > 0)
                                    {
                                        table1forpage2.Cell(i, 0).SetContent(printds_rows.Tables[0].Rows[i]["semester"].ToString());
                                        table1forpage2.Cell(i, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1forpage2.Cell(i, 1).SetContent(printds_rows.Tables[0].Rows[i]["subject_code"].ToString());
                                        table1forpage2.Cell(i, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1forpage2.Cell(i, 2).SetContent(printds_rows.Tables[0].Rows[i]["subject_name"].ToString());
                                        table1forpage2.Cell(i, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1forpage2.Cell(i, 3).SetContent(printds_rows.Tables[0].Rows[i]["credit_points"].ToString());
                                        table1forpage2.Cell(i, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1forpage2.Cell(i, 4).SetContent(printds_rows.Tables[0].Rows[i]["grade"].ToString());
                                        table1forpage2.Cell(i, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1forpage2.Cell(i, 5).SetContent(printds_rows.Tables[0].Rows[i]["result"].ToString());
                                        table1forpage2.Cell(i, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //sem
                                        //printds_rows.Tables[0].Rows[i]["subject_code"].ToString()
                                        //printds_rows.Tables[0].Rows[i]["subject_name"].ToString()
                                        //printds_rows.Tables[0].Rows[i]["credit_points"].ToString()
                                        //printds_rows.Tables[0].Rows[i]["grade"].ToString()
                                        //printds_rows.Tables[0].Rows[i]["result"].ToString()
                                        //PdfTextArea pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 38, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, roman);//17
                                        //mypdfpage.Add(pdfdegree);
                                        //PdfTextArea pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 56, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["subject_code"].ToString());
                                        //mypdfpage.Add(pdfdegree);
                                        //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 102, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["subject_name"].ToString());
                                        //mypdfpage.Add(pdfdegree);
                                        //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 335, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["max_int_marks"].ToString());
                                        //mypdfpage.Add(pdfdegree);
                                        //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 363, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["internal_mark"].ToString());
                                        //mypdfpage.Add(pdfdegree);
                                        //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 392, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["max_ext_marks"].ToString());
                                        //mypdfpage.Add(pdfdegree);
                                        //double extfinal = Convert.ToDouble(printds_rows.Tables[0].Rows[i]["external_mark"].ToString());
                                        //extfinal = Math.Round(extfinal, 0);
                                        //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 418, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(extfinal));
                                        //mypdfpage.Add(pdfdegree);
                                        //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 442, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["maxtotal"].ToString());
                                        //mypdfpage.Add(pdfdegree);
                                        //double totfinal = Convert.ToDouble(printds_rows.Tables[0].Rows[i]["total"].ToString());
                                        //totfinal = Math.Round(totfinal, 0);
                                        //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 471, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(totfinal));
                                        //mypdfpage.Add(pdfdegree);
                                        //double checkmarkmm = Convert.ToDouble(printds_rows.Tables[0].Rows[i]["total"].ToString());
                                        //string gradesql = "    select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='" + sem + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 16/aug 2014
                                        //gradeds.Clear();
                                        //gradeds = da.select_method_wo_parameter(gradesql, "Text");
                                        //if (gradeds.Tables[0].Rows.Count == 0)
                                        //{
                                        //    gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";//added by sridhar 
                                        //    gradeds.Clear();
                                        //    gradeds = da.select_method_wo_parameter(gradesql, "Text");
                                        //}
                                        //for (int grd = 0; grd < gradeds.Tables[0].Rows.Count; grd++)
                                        //{
                                        //    if (Convert.ToInt32(gradeds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(gradeds.Tables[0].Rows[grd][2]) >= checkmarkmm)
                                        //    {
                                        //        grade = gradeds.Tables[0].Rows[grd][0].ToString();
                                        //        gradepoints = gradeds.Tables[0].Rows[grd]["Credit_Points"].ToString();
                                        //    }
                                        //}
                                        //double gradeibtpoint = Convert.ToDouble(printds_rows.Tables[0].Rows[i]["total"].ToString());
                                        //gradeibtpoint = gradeibtpoint / 10;
                                        //gradeibtpoint = Math.Round(gradeibtpoint, 1);
                                        //gradepoints = Convert.ToString(gradeibtpoint);
                                        //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 499, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, printds_rows.Tables[0].Rows[i]["credit_points"].ToString());
                                        //mypdfpage.Add(pdfdegree);
                                        //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 523, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, gradepoints);
                                        //mypdfpage.Add(pdfdegree);
                                        //string result = printds_rows.Tables[0].Rows[i]["result"].ToString().ToLower();
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
                                        //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 559, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, grade);
                                        //mypdfpage.Add(pdfdegree);
                                        //pdfdegree = new PdfTextArea(Fontarial7, System.Drawing.Color.Black, new PdfArea(mydoc, 579, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, result);
                                        //mypdfpage.Add(pdfdegree);
                                        //colval = colval + 10;
                                    }
                                }
                                newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 57, 200, 482, 500));//sr
                                mypdfpage.Add(newpdftabpage2);
                            }
                        }
                        //rollnosub	sem 	degree_code	batch_year	mode 	collcode
                        string year = ddlYear.SelectedItem.Text;
                        string collcode = Session["collegecode"].ToString();
                        Gios.Pdf.PdfTable table1forpagegpa = mydoc.NewTable(fontcal11, 1, 4, 1);
                        table1forpagegpa.VisibleHeaders = false;
                        table1forpagegpa.SetBorders(Color.Black, 1, BorderType.None);
                        string gpa = da.Calulat_GPA_Semwise(rollnosub, degree_code, batch_year, Convert.ToString(month), ddlYear.SelectedItem.Text, collcode);
                        string cgpa = Calculete_CGPA(rollnosub, sem, degree_code, batch_year, mode, collcode);
                        table1forpagegpa.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpagegpa.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpagegpa.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpagegpa.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpagegpa.Cell(0, 0).SetContent(gpa);
                        table1forpagegpa.Cell(0, 1).SetContent(cgpa);
                        table1forpagegpa.Cell(0, 2).SetContent(totcredits);
                        table1forpagegpa.Cell(0, 3).SetContent(totcredits);
                        Gios.Pdf.PdfTablePage newpdfgpa = table1forpagegpa.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 57, 539, 482, 500));
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
            string szFile = "marksheet" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
            mydoc.SaveToFile(szPath + szFile);
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
            Response.ContentType = "application/pdf";
            Response.WriteFile(szPath + szFile);
        }
    }

    public string Calculete_CGPA(string RollNo, string semval, string degree_code, string batch_year, string latmode, string collegecode)
    {
        double total = 0;
        bool flag = true;
        try
        {
            int jvalue = 0;
            string strgrade = string.Empty;
            double creditval = 0;
            double finalgpa1 = 0;
            double creditsum1 = 0;
            string strsubcrd = string.Empty;
            int gtempejval = 0;
            string syll_code = string.Empty;
            string examcodevalg = string.Empty;
            DataSet dggradetot = new DataSet();
            DataSet dssem = new DataSet();
            double strtot = 0, inte = 0, exte = 0;
            double strgradetempfrm = 0;
            double strgradetempto = 0;
            string strgradetempgrade = string.Empty;
            string strtotgrac = string.Empty;
            string sqlcmdgraderstotal = string.Empty;
            int attemptswith = 0;
            string strattmaxmark = string.Empty;
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
            strgrade = string.Empty;
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
            string folionum = da.GetFunction(" select value from Master_Settings where settings ='Consolidate Sheet'");
            string[] split = folionum.Split('-');
            folionum = split[1].ToString();
            int value = Convert.ToInt32(folionum);
            value++;
            int foliolength = folionum.Length;
            string count = value.ToString();
            int findlen = foliolength - count.Length;
            string acro = split[0].ToString();
            string zerolist = string.Empty;
            for (int m = 0; m < findlen; m++)
            {
                zerolist = zerolist + "0";
            }
            acro = acro + "-" + zerolist + "" + value.ToString();
            string folioexist = " if exists (select * from Master_Settings where settings='Consolidate Sheet') update Master_Settings set value='" + acro + "' where settings='Consolidate Sheet' else insert into Master_Settings (settings,value) values ('Consolidate Sheet','" + acro + "')";
            int update = da.update_method_wo_parameter(folioexist, "text");
            string getval = da.GetFunction("select value from Master_Settings where settings='Consolidate Sheet' ");
        }
        catch (Exception ex)
        {
        }
    }

    public void generateIntramural()
    {
        try
        {
            string folionum = da.GetFunction(" select value from Master_Settings where settings ='Intramural Sheet'");
            string[] split = folionum.Split('-');
            folionum = split[1].ToString();
            int value = Convert.ToInt32(folionum);
            value++;
            int foliolength = folionum.Length;
            string count = value.ToString();
            int findlen = foliolength - count.Length;
            string acro = split[0].ToString();
            string zerolist = string.Empty;
            for (int m = 0; m < findlen; m++)
            {
                zerolist = zerolist + "0";
            }
            acro = acro + "-" + zerolist + "" + value.ToString();
            string folioexist = " if exists (select * from Master_Settings where settings='Intramural Sheet') update Master_Settings set value='" + acro + "' where settings='Intramural Sheet' else insert into Master_Settings (settings,value) values ('Intramural Sheet','" + acro + "')";
            int update = da.update_method_wo_parameter(folioexist, "text");
            string getval = da.GetFunction("select value from Master_Settings where settings='Intramural Sheet' ");
        }
        catch (Exception ex)
        {
        }
    }

    public void generategrade()
    {
        try
        {
            string folionum = da.GetFunction(" select value from Master_Settings where settings ='Grade Sheet'");
            string[] split = folionum.Split('-');
            folionum = split[1].ToString();
            int value = Convert.ToInt32(folionum);
            value++;
            int foliolength = folionum.Length;
            string count = value.ToString();
            int findlen = foliolength - count.Length;
            string acro = split[0].ToString();
            string zerolist = string.Empty;
            for (int m = 0; m < findlen; m++)
            {
                zerolist = zerolist + "0";
            }
            acro = acro + "-" + zerolist + "" + value.ToString();
            string folioexist = " if exists (select * from Master_Settings where settings='Grade Sheet') update Master_Settings set value='" + acro + "' where settings='Grade Sheet' else insert into Master_Settings (settings,value) values ('Grade Sheet','" + acro + "')";
            int update = da.update_method_wo_parameter(folioexist, "text");
            string getval = da.GetFunction("select value from Master_Settings where settings='Grade Sheet' ");
        }
        catch (Exception ex)
        {
        }
    }

    public void bindformattwo() ////////Added by jeyagandhi////////////
    {
        try
        {
            //////////////////Grade Sheet/////////////////
            collegecode = Session["collegecode"].ToString();
            DataSet printds = new DataSet();
            DataSet printds_new = new DataSet();
            DataSet printds_rows = new DataSet();
            DataSet dssqlmain = new DataSet();
            DataSet dsexammonthyear = new DataSet();
            DataSet dssqlexamdetails = new DataSet();
            DataSet gradeds = new DataSet();
            //RadioButton1.Visible = true;
            //RadioButton2.Visible = true;
            //RadioButton3.Visible = true;
            //RadioButton4.Visible = true;
            ddlformate.Visible = true;
            string sgpa = string.Empty;
            string creditdreg = string.Empty;
            string creditregno = string.Empty;
            string dssyllcode = string.Empty;
            string cgpa = string.Empty;
            string latmode = string.Empty;
            string stdphtsql = string.Empty;
            string monthandyear = string.Empty;
            string studname = string.Empty;
            string creditrn = string.Empty;
            string rollnosub = string.Empty;
            string regnumber = string.Empty;
            string batch_year = string.Empty;
            string degree_code = string.Empty;
            string exam_code = string.Empty;
            string sem = string.Empty;
            int colval = 0;
            string branch = string.Empty;
            string month1 = string.Empty;
            int month = 0;
            string year = string.Empty;
            string monthstr = string.Empty;
            string sql3 = string.Empty;
            string syllcode = string.Empty;
            string semroman = string.Empty;
            string grade = string.Empty;
            string gradepoints = string.Empty;
            string regno = string.Empty;
            string degree1 = string.Empty;
            string examquery = string.Empty;
            DataSet dscreditsyll = new DataSet();
            Font Fontpala12 = new Font("Palatino Linotype", 10, FontStyle.Bold);
            Font Fontarial9 = new Font("Arial", 9, FontStyle.Regular);
            Font Fontarial12 = new Font("Arial", 12, FontStyle.Regular);
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage;
            string creditearn = " select sum(s.credit_points) as credits from syllabus_master as sy,subject as s,subjectchooser as sc where sy.syll_code=s.syll_code and sc.subject_no=s.subject_no ";
            string creditsyll = " select s.syll_code from syllabus_master as sy,subject as s,subjectchooser as sc where sy.syll_code=s.syll_code and sc.subject_no=s.subject_no ";
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
                        IblError.Text = string.Empty;
                        printds.Clear();
                        printds.Dispose();
                        printds_new.Clear();
                        printds_new.Dispose();
                        rollnosub = FpSpread2.Sheets[0].Cells[res, 1].Note;
                        regnumber = FpSpread2.Sheets[0].Cells[res, 2].Text;
                        string sql = "SELECT Reg_No,r.batch_year,r.app_no,r.degree_code,R.current_semester,R.Stud_Name,Course_Name as degree,Dept_name,CONVERT(VARCHAR, dob, 103) as dob FROM Registration R,Applyn A,Degree G,Course C,Department D WHERE R.App_No = A.App_No AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND D.College_Code = G.College_Code AND Roll_No = '" + rollnosub + "' and r.college_code='" + Session["collegecode"].ToString() + "' ";
                        printds = da.select_method_wo_parameter(sql, "Text");
                        batch_year = printds.Tables[0].Rows[0]["batch_year"].ToString();
                        degree_code = printds.Tables[0].Rows[0]["degree_code"].ToString();
                        examquery = "select * from exam_details where  batch_year='" + batch_year + "' and   degree_code='" + degree_code + "' ";
                        printds_new = da.select_method_wo_parameter(examquery, "text");
                        if (printds.Tables[0].Rows.Count > 0)
                        {
                            mypdfpage = mydoc.NewPage();
                            degree1 = printds.Tables[0].Rows[0]["degree"].ToString();
                            month1 = ddlMonth.SelectedValue.ToString();
                            month = ddlMonth.SelectedIndex;
                            year = ddlYear.SelectedItem.Text;
                            monthstr = ddlMonth.SelectedIndex.ToString();
                            string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
                            monthandyear = strMonthName + "  " + ddlYear.SelectedItem.Text;
                            monthandyear = monthandyear.ToUpper();
                            regno = printds.Tables[0].Rows[0]["Reg_No"].ToString();
                            studname = printds.Tables[0].Rows[0]["Stud_Name"].ToString();
                            branch = printds.Tables[0].Rows[0]["Dept_name"].ToString();
                            string stdappno = printds.Tables[0].Rows[0]["App_No"].ToString();
                            // sem = printds.Tables[0].Rows[0]["current_semester"].ToString();
                            sem = Session["semforsub"].ToString();
                            exam_code = printds_new.Tables[0].Rows[0]["exam_code"].ToString();
                            int sum = 0;
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
                                mypdfpage.Add(LogoImage, 505, 113, 600);
                            }
                            else
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg"));
                                mypdfpage.Add(LogoImage, 505, 113, 600);
                            }
                            PdfTextArea pdfhead = new PdfTextArea(Fontarial12, System.Drawing.Color.Black, new PdfArea(mydoc, 184, 128, 553, 50), System.Drawing.ContentAlignment.TopLeft, studname.ToString());
                            mypdfpage.Add(pdfhead);
                            PdfTextArea pdfhead1 = new PdfTextArea(Fontarial12, System.Drawing.Color.Black, new PdfArea(mydoc, 113, 148, 553, 50), System.Drawing.ContentAlignment.TopLeft, regnumber);
                            mypdfpage.Add(pdfhead1);
                            PdfTextArea pdfhead2 = new PdfTextArea(Fontarial12, System.Drawing.Color.Black, new PdfArea(mydoc, 230, 148, 553, 50), System.Drawing.ContentAlignment.TopLeft, monthandyear.ToString());
                            mypdfpage.Add(pdfhead2);
                            PdfTextArea pdfhead3 = new PdfTextArea(Fontarial12, System.Drawing.Color.Black, new PdfArea(mydoc, 113, 168, 553, 50), System.Drawing.ContentAlignment.TopLeft, degree1);
                            mypdfpage.Add(pdfhead3);
                            PdfTextArea pdfhead4 = new PdfTextArea(Fontarial12, System.Drawing.Color.Black, new PdfArea(mydoc, 230, 168, 553, 50), System.Drawing.ContentAlignment.TopLeft, branch);
                            mypdfpage.Add(pdfhead4);
                            string details = examquery + "  and exam_month='" + ddlMonth.SelectedIndex.ToString() + "' and exam_year='" + ddlYear.SelectedItem.Text + "' ";
                            DataSet dsdetails = da.select_method_wo_parameter(details, "text");
                            string exam_codes = dsdetails.Tables[0].Rows[0][0].ToString();
                            sql3 = "Select distinct Subject_Type,subject_name,subject.syll_code,subject_code,subject.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,maxtotal,grade,cp,credit_points,mark_entry.subject_no,semester,maxtotal,exam_code from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = '" + exam_codes + "' and roll_no='" + rollnosub + "'   order by subject_code,subject_type";
                            printds_rows = da.select_method_wo_parameter(sql3, "Text");
                            int rowcount = printds_rows.Tables[0].Rows.Count;
                            colval = 230;
                            if (printds_rows.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < printds_rows.Tables[0].Rows.Count; i++)
                                {
                                    Gios.Pdf.PdfTable table1forpage1 = mydoc.NewTable(Fontarial9, rowcount, 7, 1);
                                    table1forpage1.VisibleHeaders = false;
                                    table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                                    table1forpage1.SetColumnsWidth(new int[] { 40, 57, 289, 43, 45, 45, 45 });
                                    table1forpage1.Cell(i, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    sem = printds_rows.Tables[0].Rows[i]["semester"].ToString();
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
                                    table1forpage1.Cell(i, 0).SetContent(semroman);
                                    table1forpage1.Cell(i, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1forpage1.Cell(i, 1).SetContent(printds_rows.Tables[0].Rows[i]["subject_code"].ToString());
                                    table1forpage1.Cell(i, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1forpage1.Cell(i, 2).SetContent(printds_rows.Tables[0].Rows[i]["subject_name"].ToString());
                                    double totfinal = Convert.ToDouble(printds_rows.Tables[0].Rows[i]["total"].ToString());
                                    totfinal = Math.Round(totfinal, 0);
                                    table1forpage1.Cell(i, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage1.Cell(i, 3).SetContent(totfinal);
                                    double checkmarkmm = Convert.ToDouble(printds_rows.Tables[0].Rows[i]["total"].ToString());
                                    string gradesql = "    select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='" + sem + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";
                                    gradeds.Clear();
                                    gradeds = da.select_method_wo_parameter(gradesql, "Text");
                                    if (gradeds.Tables[0].Rows.Count == 0)
                                    {
                                        gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";
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
                                    gradeibtpoint = Math.Round(gradeibtpoint, 0, MidpointRounding.AwayFromZero);
                                    gradepoints = Convert.ToString(gradeibtpoint);
                                    table1forpage1.Cell(i, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage1.Cell(i, 4).SetContent(printds_rows.Tables[0].Rows[i]["credit_points"].ToString());
                                    table1forpage1.Cell(i, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage1.Cell(i, 5).SetContent(grade);
                                    table1forpage1.Cell(i, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage1.Cell(i, 6).SetContent(gradepoints);
                                    Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 23, colval, 547, 50));
                                    mypdfpage.Add(newpdftabpage2);
                                    colval = colval + 20;
                                    syllcode = printds_rows.Tables[0].Rows[i]["syll_code"].ToString();
                                }
                                sem = Session["semforsub"].ToString();
                                Gios.Pdf.PdfTable table1forpagemain = mydoc.NewTable(Fontarial9, 4, 11, 3);
                                table1forpagemain.VisibleHeaders = false;
                                table1forpagemain.SetBorders(Color.Black, 1, BorderType.None);
                                table1forpagemain.SetColumnsWidth(new int[] { 37, 37, 37, 37, 37, 37, 37, 37, 37, 37 });
                                for (int semv = 1; semv <= Convert.ToInt32(sem); semv++)
                                {
                                    if (Convert.ToInt32(sem) >= semv)
                                    {
                                        string examcode = printds_new.Tables[0].Rows[semv - 1]["exam_code"].ToString();
                                        string examnth = printds_new.Tables[0].Rows[semv - 1]["Exam_Month"].ToString();
                                        string examnyr = printds_new.Tables[0].Rows[semv - 1]["Exam_year"].ToString();
                                        sgpa = da.Calulat_GPA_Semwise(rollnosub, degree_code, batch_year, examnth, examnyr, collegecode);
                                        string creditearn1 = creditearn + "  and sy.batch_year='" + batch_year + "' and sy.degree_code='" + degree_code + "' and roll_no='" + rollnosub + "'  and sc.semester='" + semv + "'";
                                        DataSet dscredits = da.select_method_wo_parameter(creditearn1, "text");
                                        creditrn = dscredits.Tables[0].Rows[0][0].ToString();
                                        string creditsylll = creditsyll + "   and sy.batch_year='" + batch_year + "' and sy.degree_code='" + degree_code + "' and roll_no='" + rollnosub + "' and sc.semester='" + semv + "'";
                                        dscreditsyll = da.select_method_wo_parameter(creditsylll, "text");
                                        dssyllcode = dscreditsyll.Tables[0].Rows[0][0].ToString();
                                        creditdreg = " Select sum( S.Credit_Points) as credits FROM Mark_Entry M,Subject S,Syllabus_Master Y  WHERE M.Subject_No = S.Subject_No AND S.Syll_Code = Y.Syll_Code AND S.Syll_Code = '" + dssyllcode + "' AND roll_no='" + rollnosub + "' AND Degree_Code ='" + degree_code + "' AND Y.Semester ='" + semv + "' AND Upper(Result) = 'PASS' AND Exam_Code IN (SELECT DISTINCT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degree_code + "'  and batch_year='" + batch_year + "' AND Exam_Year ='" + examnyr + "' and Exam_Month='" + examnth + "') ";
                                        DataSet sdcreditreg = da.select_method_wo_parameter(creditdreg, "text");
                                        creditregno = sdcreditreg.Tables[0].Rows[0][0].ToString();
                                        string semester = semv.ToString();
                                        cgpa = da.Calculete_CGPA(rollnosub, semester, degree_code, batch_year, latmode, collegecode);
                                        table1forpagemain.Cell(0, semv).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1forpagemain.Cell(0, semv).SetContent(creditrn);
                                        if (creditregno == "")
                                        {
                                            creditregno = "0";
                                        }
                                        table1forpagemain.Cell(1, semv).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1forpagemain.Cell(1, semv).SetContent(creditregno);
                                        table1forpagemain.Cell(2, semv).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1forpagemain.Cell(2, semv).SetContent(sgpa);
                                        if (Convert.ToInt32(creditregno) > 0)
                                        {
                                            sum = Convert.ToInt32(creditregno) + sum;
                                        }
                                    }
                                }
                                table1forpagemain.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpagemain.Cell(3, 1).SetContent(sum.ToString());
                                table1forpagemain.Cell(3, 9).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpagemain.Cell(3, 9).SetContent(cgpa);
                                Gios.Pdf.PdfTablePage newpdftabpagemain = table1forpagemain.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 100, 711, 450, 75));
                                mypdfpage.Add(newpdftabpagemain);
                                colval = colval + 20;
                                generategrade();
                                string generate = da.GetFunction("select value from Master_Settings where settings='Grade Sheet' ");
                                string[] split = generate.Split('-');
                                if (split.GetUpperBound(0) >= 1)
                                {
                                    generate = split[0].ToString() + split[1].ToString();
                                }
                                PdfTextArea pdfdegree253 = new PdfTextArea(Fontarial12, System.Drawing.Color.Black, new PdfArea(mydoc, 515, 95, 544, 50), System.Drawing.ContentAlignment.TopLeft, generate);
                                mypdfpage.Add(pdfdegree253);
                                PdfTextArea pdfdegree1 = new PdfTextArea(Fontpala12, System.Drawing.Color.Black, new PdfArea(mydoc, 125, 780, 305, 50), System.Drawing.ContentAlignment.TopLeft, txtdoi.Text);
                                mypdfpage.Add(pdfdegree1);
                                PdfTextArea pdfdegreew1 = new PdfTextArea(Fontpala12, System.Drawing.Color.Black, new PdfArea(mydoc, 355, 765, 305, 50), System.Drawing.ContentAlignment.TopLeft, "English");
                                mypdfpage.Add(pdfdegreew1);
                                PdfTextArea pdfdegree123aas = new PdfTextArea(Fontpala12, System.Drawing.Color.Black, new PdfArea(mydoc, 233, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, "*** End of Statement ***");
                                mypdfpage.Add(pdfdegree123aas);
                                mypdfpage.SaveToDocument();
                                string appPath = HttpContext.Current.Server.MapPath("~");
                                if (appPath != "")
                                {
                                    string szPath = appPath + "/Report/";
                                    string szFile = "GradeSheet" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                                    mydoc.SaveToFile(szPath + szFile);
                                    Response.ClearHeaders();
                                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                    Response.ContentType = "application/pdf";
                                    Response.WriteFile(szPath + szFile);
                                }
                                else
                                {
                                    errmsg.Visible = true;
                                    errmsg.Text = "No Records Found";
                                    //RadioButton1.Visible = true;
                                    //RadioButton2.Visible = true;
                                    //RadioButton3.Visible = true;
                                    //RadioButton4.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            IblError.Visible = true;
                            IblError.Text = "No Records Found";
                            //RadioButton1.Visible = true;
                            //RadioButton2.Visible = true;
                            //RadioButton3.Visible = true;
                            //RadioButton4.Visible = true;
                        }
                    }
                    else
                    {
                        IblError.Visible = true;
                        IblError.Text = "No Records Found ";
                        //RadioButton1.Visible = true;
                        //RadioButton2.Visible = true;
                        //RadioButton3.Visible = true;
                        //RadioButton4.Visible = true;
                    }
                }
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void bindformatthree()  ////////Added by jeyagandhi////////////
    {
        try
        {
            //////////////////Grade Sheet/////////////////
            collegecode = Session["collegecode"].ToString();
            DataSet printds = new DataSet();
            DataSet printds_new = new DataSet();
            DataSet printds_rows = new DataSet();
            DataSet dssqlmain = new DataSet();
            DataSet dsexammonthyear = new DataSet();
            DataSet dssqlexamdetails = new DataSet();
            DataSet gradeds = new DataSet();
            //RadioButton1.Visible = true;
            //RadioButton2.Visible = true;
            //RadioButton3.Visible = true;
            //RadioButton4.Visible = true;
            ddlformate.Visible = true;
            string sgpa = string.Empty;
            string creditdreg = string.Empty;
            string creditregno = string.Empty;
            string dssyllcode = string.Empty;
            string cgpa = string.Empty;
            string latmode = string.Empty;
            string stdphtsql = string.Empty;
            string monthandyear = string.Empty;
            string studname = string.Empty;
            string creditrn = string.Empty;
            string rollnosub = string.Empty;
            string regnumber = string.Empty;
            string batch_year = string.Empty;
            string degree_code = string.Empty;
            string exam_code = string.Empty;
            string sem = string.Empty;
            int colval = 0;
            string branch = string.Empty;
            string month1 = string.Empty;
            int month = 0;
            string year = string.Empty;
            string monthstr = string.Empty;
            string sql3 = string.Empty;
            string syllcode = string.Empty;
            string semroman = string.Empty;
            string grade = string.Empty;
            string gradepoints = string.Empty;
            string regno = string.Empty;
            string degree1 = string.Empty;
            string examquery = string.Empty;
            DataSet dscreditsyll = new DataSet();
            Font Fontpala12 = new Font("Palatino Linotype", 10, FontStyle.Bold);
            Font Fontarial9 = new Font("Arial", 9, FontStyle.Regular);
            Font Fontarial12 = new Font("Arial", 12, FontStyle.Regular);
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage;
            string creditearn = " select sum(s.credit_points) as credits from syllabus_master as sy,subject as s,subjectchooser as sc where sy.syll_code=s.syll_code and sc.subject_no=s.subject_no ";
            string creditsyll = " select s.syll_code from syllabus_master as sy,subject as s,subjectchooser as sc where sy.syll_code=s.syll_code and sc.subject_no=s.subject_no ";
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
                        IblError.Text = string.Empty;
                        printds.Clear();
                        printds.Dispose();
                        printds_new.Clear();
                        printds_new.Dispose();
                        rollnosub = FpSpread2.Sheets[0].Cells[res, 1].Note;
                        regnumber = FpSpread2.Sheets[0].Cells[res, 2].Text;
                        string sql = "SELECT Reg_No,r.batch_year,r.app_no,r.degree_code,R.current_semester,R.Stud_Name,Course_Name as degree,Dept_name,CONVERT(VARCHAR, dob, 103) as dob FROM Registration R,Applyn A,Degree G,Course C,Department D WHERE R.App_No = A.App_No AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND D.College_Code = G.College_Code AND Roll_No = '" + rollnosub + "' and r.college_code='" + Session["collegecode"].ToString() + "' ";
                        printds = da.select_method_wo_parameter(sql, "Text");
                        batch_year = printds.Tables[0].Rows[0]["batch_year"].ToString();
                        degree_code = printds.Tables[0].Rows[0]["degree_code"].ToString();
                        examquery = "select * from exam_details where  batch_year='" + batch_year + "' and   degree_code='" + degree_code + "'";
                        printds_new = da.select_method_wo_parameter(examquery, "text");
                        if (printds.Tables[0].Rows.Count > 0)
                        {
                            mypdfpage = mydoc.NewPage();
                            degree1 = printds.Tables[0].Rows[0]["degree"].ToString();
                            month1 = ddlMonth.SelectedValue.ToString();
                            month = ddlMonth.SelectedIndex;
                            year = ddlYear.SelectedItem.Text;
                            monthstr = ddlMonth.SelectedIndex.ToString();
                            string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
                            monthandyear = strMonthName + "  " + ddlYear.SelectedItem.Text;
                            monthandyear = monthandyear.ToUpper();
                            regno = printds.Tables[0].Rows[0]["Reg_No"].ToString();
                            studname = printds.Tables[0].Rows[0]["Stud_Name"].ToString();
                            branch = printds.Tables[0].Rows[0]["Dept_name"].ToString();
                            string stdappno = printds.Tables[0].Rows[0]["App_No"].ToString();
                            // sem = printds.Tables[0].Rows[0]["current_semester"].ToString();
                            sem = Session["semforsub"].ToString();
                            exam_code = printds_new.Tables[0].Rows[0]["exam_code"].ToString();
                            int sum = 0;
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
                                mypdfpage.Add(LogoImage, 505, 113, 600);
                            }
                            else
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg"));
                                mypdfpage.Add(LogoImage, 505, 113, 600);
                            }
                            PdfTextArea pdfhead = new PdfTextArea(Fontarial12, System.Drawing.Color.Black, new PdfArea(mydoc, 184, 128, 553, 50), System.Drawing.ContentAlignment.TopLeft, studname.ToString());
                            mypdfpage.Add(pdfhead);
                            PdfTextArea pdfhead1 = new PdfTextArea(Fontarial12, System.Drawing.Color.Black, new PdfArea(mydoc, 113, 148, 553, 50), System.Drawing.ContentAlignment.TopLeft, regnumber);
                            mypdfpage.Add(pdfhead1);
                            PdfTextArea pdfhead2 = new PdfTextArea(Fontarial12, System.Drawing.Color.Black, new PdfArea(mydoc, 230, 148, 553, 50), System.Drawing.ContentAlignment.TopLeft, monthandyear.ToString());
                            mypdfpage.Add(pdfhead2);
                            PdfTextArea pdfhead3 = new PdfTextArea(Fontarial12, System.Drawing.Color.Black, new PdfArea(mydoc, 113, 168, 553, 50), System.Drawing.ContentAlignment.TopLeft, degree1);
                            mypdfpage.Add(pdfhead3);
                            PdfTextArea pdfhead4 = new PdfTextArea(Fontarial12, System.Drawing.Color.Black, new PdfArea(mydoc, 230, 168, 553, 50), System.Drawing.ContentAlignment.TopLeft, branch);
                            mypdfpage.Add(pdfhead4);
                            //Gios.Pdf.PdfTable table1forpagehead = mydoc.NewTable(Fontarial12, 2, 2, 1);
                            //table1forpagehead.VisibleHeaders = false;
                            //table1forpagehead.SetBorders(Color.Black, 1, BorderType.None);
                            //table1forpagehead.SetColumnsWidth(new int[] { 200, 215 });
                            //table1forpagehead.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpagehead.Cell(0, 0).SetContent(studname);
                            //table1forpagehead.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            //table1forpagehead.Cell(0, 1).SetContent("");
                            //table1forpagehead.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                            //table1forpagehead.Cell(1, 0).SetContent(regno);
                            //table1forpagehead.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpagehead.Cell(1, 1).SetContent(monthandyear);
                            //table1forpagehead.Cell(0, 0).ColSpan = 2;
                            //Gios.Pdf.PdfTablePage newpdftabpagehead = table1forpagehead.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 85, 430, 50));
                            //mypdfpage.Add(newpdftabpagehead);
                            //Gios.Pdf.PdfTable table1forpagesubhead = mydoc.NewTable(Fontarial12, 1, 2, 1);
                            //table1forpagesubhead.VisibleHeaders = false;
                            //table1forpagesubhead.SetBorders(Color.Black, 1, BorderType.None);
                            //table1forpagesubhead.SetColumnsWidth(new int[] { 200, 215 });
                            //table1forpagesubhead.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpagesubhead.Cell(0, 0).SetContent(degree1);
                            //table1forpagesubhead.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpagesubhead.Cell(0, 1).SetContent(branch);
                            //Gios.Pdf.PdfTablePage newpdftabpagesubhead = table1forpagesubhead.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 110, 430, 50));
                            //mypdfpage.Add(newpdftabpagesubhead);
                            string details = examquery + "  and exam_month='" + ddlMonth.SelectedIndex.ToString() + "' and exam_year='" + ddlYear.SelectedItem.Text + "' ";
                            DataSet dsdetails = da.select_method_wo_parameter(details, "text");
                            string exam_codes = dsdetails.Tables[0].Rows[0][0].ToString();
                            sql3 = "Select distinct Subject_Type,subject_name,subject.syll_code,subject_code,subject.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,maxtotal,grade,cp,credit_points,mark_entry.subject_no,semester,maxtotal,exam_code from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = '" + exam_codes + "' and roll_no='" + rollnosub + "'  order by subject_code,subject_type";
                            printds_rows = da.select_method_wo_parameter(sql3, "Text");
                            int rowcount = printds_rows.Tables[0].Rows.Count;
                            colval = 230;
                            if (printds_rows.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < printds_rows.Tables[0].Rows.Count; i++)
                                {
                                    Gios.Pdf.PdfTable table1forpage1 = mydoc.NewTable(Fontarial9, rowcount, 7, 1);
                                    table1forpage1.VisibleHeaders = false;
                                    table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                                    table1forpage1.SetColumnsWidth(new int[] { 40, 57, 289, 43, 45, 45, 45 });
                                    table1forpage1.Cell(i, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    sem = printds_rows.Tables[0].Rows[i]["semester"].ToString();
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
                                    table1forpage1.Cell(i, 0).SetContent(semroman);
                                    table1forpage1.Cell(i, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1forpage1.Cell(i, 1).SetContent(printds_rows.Tables[0].Rows[i]["subject_code"].ToString());
                                    table1forpage1.Cell(i, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1forpage1.Cell(i, 2).SetContent(printds_rows.Tables[0].Rows[i]["subject_name"].ToString());
                                    double totfinal = Convert.ToDouble(printds_rows.Tables[0].Rows[i]["total"].ToString());
                                    totfinal = Math.Round(totfinal, 0);
                                    table1forpage1.Cell(i, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage1.Cell(i, 3).SetContent(totfinal);
                                    double checkmarkmm = Convert.ToDouble(printds_rows.Tables[0].Rows[i]["total"].ToString());
                                    string gradesql = "    select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='" + sem + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";
                                    gradeds.Clear();
                                    gradeds = da.select_method_wo_parameter(gradesql, "Text");
                                    if (gradeds.Tables[0].Rows.Count == 0)
                                    {
                                        gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'";
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
                                    gradeibtpoint = Math.Round(gradeibtpoint, 0, MidpointRounding.AwayFromZero);
                                    gradepoints = Convert.ToString(gradeibtpoint);
                                    table1forpage1.Cell(i, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage1.Cell(i, 4).SetContent(printds_rows.Tables[0].Rows[i]["credit_points"].ToString());
                                    table1forpage1.Cell(i, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage1.Cell(i, 5).SetContent(grade);
                                    table1forpage1.Cell(i, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage1.Cell(i, 6).SetContent(gradepoints);
                                    Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 23, colval, 547, 50));
                                    mypdfpage.Add(newpdftabpage2);
                                    colval = colval + 20;
                                    syllcode = printds_rows.Tables[0].Rows[i]["syll_code"].ToString();
                                }
                                sem = Session["semforsub"].ToString();
                                Gios.Pdf.PdfTable table1forpagemain = mydoc.NewTable(Fontarial9, 4, 11, 3);
                                table1forpagemain.VisibleHeaders = false;
                                table1forpagemain.SetBorders(Color.Black, 1, BorderType.None);
                                table1forpagemain.SetColumnsWidth(new int[] { 37, 37, 37, 37, 37, 37, 37, 37, 37, 37 });
                                for (int semv = 1; semv <= Convert.ToInt32(sem); semv++)
                                {
                                    if (Convert.ToInt32(sem) >= semv)
                                    {
                                        string examcode = printds_new.Tables[0].Rows[semv - 1]["exam_code"].ToString();
                                        string examnth = printds_new.Tables[0].Rows[semv - 1]["Exam_Month"].ToString();
                                        string examnyr = printds_new.Tables[0].Rows[semv - 1]["Exam_year"].ToString();
                                        sgpa = da.Calulat_GPA_Semwise(rollnosub, degree_code, batch_year, examnth, examnyr, collegecode);
                                        string creditearn1 = creditearn + "  and sy.batch_year='" + batch_year + "' and sy.degree_code='" + degree_code + "' and roll_no='" + rollnosub + "'  and sc.semester='" + semv + "'";
                                        DataSet dscredits = da.select_method_wo_parameter(creditearn1, "text");
                                        creditrn = dscredits.Tables[0].Rows[0][0].ToString();
                                        string creditsylll = creditsyll + "   and sy.batch_year='" + batch_year + "' and sy.degree_code='" + degree_code + "' and roll_no='" + rollnosub + "' and sc.semester='" + semv + "'";
                                        dscreditsyll = da.select_method_wo_parameter(creditsylll, "text");
                                        dssyllcode = dscreditsyll.Tables[0].Rows[0][0].ToString();
                                        creditdreg = " Select sum( S.Credit_Points) as credits FROM Mark_Entry M,Subject S,Syllabus_Master Y  WHERE M.Subject_No = S.Subject_No AND S.Syll_Code = Y.Syll_Code AND S.Syll_Code = '" + dssyllcode + "' AND roll_no='" + rollnosub + "' AND Degree_Code ='" + degree_code + "' AND Y.Semester ='" + semv + "' AND Upper(Result) = 'PASS' AND Exam_Code IN (SELECT DISTINCT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degree_code + "'  and batch_year='" + batch_year + "' AND Exam_Year ='" + examnyr + "' and Exam_Month='" + examnth + "') ";
                                        DataSet sdcreditreg = da.select_method_wo_parameter(creditdreg, "text");
                                        creditregno = sdcreditreg.Tables[0].Rows[0][0].ToString();
                                        string semester = semv.ToString();
                                        cgpa = da.Calculete_CGPA(rollnosub, semester, degree_code, batch_year, latmode, collegecode);
                                        table1forpagemain.Cell(0, semv).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1forpagemain.Cell(0, semv).SetContent(creditrn);
                                        if (creditregno == "")
                                        {
                                            creditregno = "0";
                                        }
                                        table1forpagemain.Cell(1, semv).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1forpagemain.Cell(1, semv).SetContent(creditregno);
                                        table1forpagemain.Cell(2, semv).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1forpagemain.Cell(2, semv).SetContent(sgpa);
                                        if (Convert.ToInt32(creditregno) > 0)
                                        {
                                            sum = Convert.ToInt32(creditregno) + sum;
                                        }
                                    }
                                }
                                table1forpagemain.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpagemain.Cell(3, 1).SetContent(sum.ToString());
                                table1forpagemain.Cell(3, 9).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpagemain.Cell(3, 9).SetContent(cgpa);
                                Gios.Pdf.PdfTablePage newpdftabpagemain = table1forpagemain.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 100, 711, 450, 75));
                                mypdfpage.Add(newpdftabpagemain);
                                colval = colval + 20;
                                generateIntramural();
                                string generate = da.GetFunction("select value from Master_Settings where settings='Intramural Sheet' ");
                                string[] split = generate.Split('-');
                                if (split.GetUpperBound(0) >= 1)
                                {
                                    generate = split[0].ToString() + split[1].ToString();
                                }
                                PdfTextArea pdfdegree253 = new PdfTextArea(Fontarial12, System.Drawing.Color.Black, new PdfArea(mydoc, 515, 95, 544, 50), System.Drawing.ContentAlignment.TopLeft, generate);
                                mypdfpage.Add(pdfdegree253);
                                PdfTextArea pdfdegree1 = new PdfTextArea(Fontpala12, System.Drawing.Color.Black, new PdfArea(mydoc, 120, 780, 305, 50), System.Drawing.ContentAlignment.TopLeft, txtdoi.Text);
                                mypdfpage.Add(pdfdegree1);
                                PdfTextArea pdfdegreew1 = new PdfTextArea(Fontpala12, System.Drawing.Color.Black, new PdfArea(mydoc, 350, 765, 305, 50), System.Drawing.ContentAlignment.TopLeft, "English");
                                mypdfpage.Add(pdfdegreew1);
                                PdfTextArea pdfdegree123aas = new PdfTextArea(Fontpala12, System.Drawing.Color.Black, new PdfArea(mydoc, 233, colval, 305, 50), System.Drawing.ContentAlignment.TopLeft, "*** End of Statement ***");
                                mypdfpage.Add(pdfdegree123aas);
                                mypdfpage.SaveToDocument();
                                string appPath = HttpContext.Current.Server.MapPath("~");
                                if (appPath != "")
                                {
                                    string szPath = appPath + "/Report/";
                                    string szFile = "Intramural" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                                    mydoc.SaveToFile(szPath + szFile);
                                    Response.ClearHeaders();
                                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                    Response.ContentType = "application/pdf";
                                    Response.WriteFile(szPath + szFile);
                                }
                                else
                                {
                                    errmsg.Visible = true;
                                    errmsg.Text = "No Records Found";
                                    //RadioButton1.Visible = true;
                                    //RadioButton2.Visible = true;
                                    //RadioButton3.Visible = true;
                                    //RadioButton4.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            IblError.Visible = true;
                            IblError.Text = "No Records Found";
                            //RadioButton1.Visible = true;
                            //RadioButton2.Visible = true;
                            //RadioButton3.Visible = true;
                            //RadioButton4.Visible = true;
                        }
                    }
                    else
                    {
                        IblError.Visible = true;
                        IblError.Text = "No Records Found ";
                        //RadioButton1.Visible = true;
                        //RadioButton2.Visible = true;
                        //RadioButton3.Visible = true;
                        //RadioButton4.Visible = true;
                    }
                }
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void hiddenfiels()
    {
        IblError.Visible = false;
        lblno.Visible = false;
        HAllSpread.Visible = false;
        FpSpread2.Visible = false;
        printbtn.Visible = false;
        //RadioButton1.Visible = false;
        //RadioButton2.Visible = false;
        //RadioButton3.Visible = false;
        //RadioButton4.Visible = false;
        // headoffp2.Visible = false;
        ddlformate.Visible = false;
    }

    protected void ddlformate_SelectedIndexChanged(object sender, EventArgs e)
    {
        // hiddenfiels();
        //selectyear.Visible = false;
    }

    public string loadmarkat(string mr)
    {
        string strgetval = string.Empty;
        if (mr == "-1")
        {
            strgetval = "AB";
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

}
