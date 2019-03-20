using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BalAccess;
using DalConnection;
//using System.Windows.Forms;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using FarPoint.Web.Spread;
using System.Drawing;
using Gios.Pdf;
using System.IO;

public partial class Hallticket : System.Web.UI.Page
{
    [Serializable()]
    public class MyImg : ImageCellType
    {
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(75);
            img.Height = Unit.Percentage(70);

            return img;
            //System.Web.UI.WebControls.Image img1 = new System.Web.UI.WebControls.Image();
            //img1.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            //img1.Width = Unit.Percentage(75);
            //img1.Height = Unit.Percentage(70);
            //return img1;



        }
    }

    public class MyImgphotp : ImageCellType
    {


        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(100);
            img.Height = Unit.Percentage(100);

            //img.Width = 1400;
            //img.Height = 1400;
            return img;
            //System.Web.UI.WebControls.Image img1 = new System.Web.UI.WebControls.Image();
            //img1.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            //img1.Width = Unit.Percentage(75);
            //img1.Height = Unit.Percentage(70);
            //return img1;



        }
    }

    SqlCommand cmd;
    int sn0 = 0;
    int year = 0;
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection conexam = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection setcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection holidaycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection imgcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);

    //addee by sasi klumar 

    string time = string.Empty;
    string time1 = string.Empty;
    // ---end------
    Boolean Cellclick;
    Boolean flag_true = false;
    string Master = string.Empty;

    Hashtable hat = new Hashtable();
    Hashtable htattperc = new Hashtable();

    DAccess2 daccess2 = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    int moncount;
    int student = 0;
    double dif_date = 0;
    double dif_date1 = 0;
    int abs = 0, att = 0;
    int dum_diff_date, unmark;
    string roll_no, reg_no, roll_ad, studname;
    int check;
    DateTime per_from_date;
    DateTime per_to_date;
    DateTime dumm_from_date;
    string frdate, todate;
    TimeSpan ts;
    string diff_date;
    int NoHrs = 0;
    int fnhrs = 0;
    int anhrs = 0;
    int minpresI = 0;
    int eligiblepercent = 0;
    int count;
    int next = 0;
    int minpresII = 0;
    int countds = 0;
    string value, date;
    int i, rows_count;
    string tempvalue = "-1";
    int ObtValue = -1;
    int cal_from_date;
    int cal_to_date;
    double per_perhrs, per_abshrs, cum_perhrs, cum_abshrs;
    double per_ondu, per_leave, per_hhday, cum_ondu, cum_leave, cum_hhday;
    double Present = 0;
    double Absent = 0;
    double Onduty = 0;
    double Leave = 0;
    double halfday = 0;
    double pre_present_date, pre_ondu_date, pre_leave_date, per_absent_date;
    double cum_present_date, cum_ondu_date, cum_leave_date, cum_absent_date;
    double workingdays = 0;
    double per_workingdays = 0;
    double cum_workingdays = 0;
    double leave_pointer, absent_pointer;
    double leave_point, absent_point;
    double per_tage_date, cum_tage_date, per_tage_hrs, cum_tage_hrs;
    double cum_tot_point, per_holidate, cum_per_holidate;
    int per_dum_unmark, cum_dum_unmark, dum_unmark;
    int tot_per_hrs, per_tot_per_hrs, cum_per_tot_per, tot_wok_hrs;
    double per_con_hrs, cum_con_hrs;
    double njhr, njdate, per_njhr, per_njdate, cum_njhr, cum_njdate;
    double per_per_hrs, cum_per_perhrs;
    double tot_ondu, per_tot_ondu, cum_tot_ondu;

    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();//ADded By Srinath 21/3/2013
    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
    string university = string.Empty;
    string collnamenew1 = string.Empty;
    string address1 = string.Empty;
    string address3 = string.Empty;
    string address = string.Empty;
    string affliatedby = string.Empty;
    string catgory = string.Empty;
    string pincode = string.Empty;
    string affiliated = string.Empty;
    int sno = 0;

    int subjnos = 0;
    string srno = string.Empty;
    string examdatenew = string.Empty;
    string subject_code = string.Empty;
    string subject_name = string.Empty;
    string exam_session = string.Empty;
    int mc = 0;

    string sem = string.Empty;
    string dob = string.Empty;
    string exam_date = string.Empty;

    Boolean tempvar = false;

    string stdroll = string.Empty;

    string rollnosub = string.Empty;
    string exammonth = string.Empty;
    string exammonthnew = string.Empty;
    string examyear = string.Empty;
    string exammonthnew1 = string.Empty;
    string stuname = string.Empty;
    string regnumber = string.Empty;
    string collegecode = string.Empty;
    string degreecode = string.Empty;
    string batch = string.Empty;
    string degree = string.Empty;
    string course = string.Empty;
    string exam_code = string.Empty;

    string srno1 = string.Empty;
    string examne = string.Empty;
    string examse = string.Empty;
    string subjeccode = string.Empty;
    string subjname = string.Empty;


    //protected override void Render(System.Web.UI.HtmlTextWriter writer)
    //{

    //    Control cntUpdateBtn = printspread.FindControl("Update");
    //    Control cntCancelBtn = printspread.FindControl("Cancel");
    //    Control cntCopyBtn = printspread.FindControl("Copy");
    //    Control cntCutBtn = printspread.FindControl("Clear");
    //    Control cntPasteBtn = printspread.FindControl("Paste");
    //    //Control cntPageNextBtn = FpSpread1.FindControl("Next");
    //    //Control cntPagePreviousBtn = FpSpread1.FindControl("Prev");
    //    Control cntPagePrintBtn = printspread.FindControl("Print");
    //    Control cntPagePrintPDFBtn = printspread.FindControl("PrintPDF");

    //    if ((cntUpdateBtn != null))
    //    {

    //        TableCell tc = (TableCell)cntUpdateBtn.Parent;
    //        TableRow tr = (TableRow)tc.Parent;

    //        tr.Cells.Remove(tc);

    //        tc = (TableCell)cntCancelBtn.Parent;
    //        tr.Cells.Remove(tc);


    //        tc = (TableCell)cntCopyBtn.Parent;
    //        tr.Cells.Remove(tc);

    //        tc = (TableCell)cntCutBtn.Parent;
    //        tr.Cells.Remove(tc);

    //        tc = (TableCell)cntPasteBtn.Parent;
    //        tr.Cells.Remove(tc);

    //        //tc = (TableCell)cntPageNextBtn.Parent;
    //        //tr.Cells.Remove(tc);

    //        //tc = (TableCell)cntPagePreviousBtn.Parent;
    //        //tr.Cells.Remove(tc);

    //        tc = (TableCell)cntPagePrintPDFBtn.Parent;
    //        tr.Cells.Remove(tc);

    //    }

    //    base.Render(writer);
    //}

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
            HAllSpread.SaveChanges();
            FpSpread2.SaveChanges();
            if (!Page.IsPostBack)
            {
                collegecode = Session["collegecode"].ToString();

                Image1.ImageUrl = "";
                Image1.ImageUrl = "Handler/Leftlogo.ashx?id=" + Session["collegecode"].ToString();
                CheckRegular.Checked = true;
                RadioButton2.Checked = true;
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

                    ddlYear.Items.Add(Convert.ToString(year1 - l + 1));

                }
                ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));

                //HAllSpread.Sheets[0].PageSize = 10;
                HAllSpread.Sheets[0].RowHeader.Visible = false;
                HAllSpread.Sheets[0].ColumnCount = 6;
                MyStyle.Font.Size = FontUnit.Medium;
                MyStyle.Font.Name = "Book Antiqua";
                MyStyle.Font.Bold = true;
                MyStyle.HorizontalAlign = HorizontalAlign.Center;
                MyStyle.ForeColor = Color.Black;
                MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                HAllSpread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                HAllSpread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                HAllSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                HAllSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Year";
                HAllSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
                HAllSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Course";
                HAllSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Sem";
                HAllSpread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Total Students";
                HAllSpread.Sheets[0].Columns[0].Width = 40;
                HAllSpread.Sheets[0].Columns[1].Width = 40;
                HAllSpread.Sheets[0].Columns[5].Font.Underline = true;
                HAllSpread.Sheets[0].Columns[5].ForeColor = Color.Black;
                HAllSpread.Sheets[0].AutoPostBack = true;
                HAllSpread.CommandBar.Visible = false;
                HAllSpread.Sheets[0].Columns[0].Locked = true;
                HAllSpread.Sheets[0].Columns[1].Locked = true;
                HAllSpread.Sheets[0].Columns[2].Locked = true;
                HAllSpread.Sheets[0].Columns[3].Locked = true;
                HAllSpread.Sheets[0].Columns[4].Locked = true;
                HAllSpread.Sheets[0].Columns[5].Locked = true;
                Rangechk.Visible = false;

            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddldate_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void btngo_Click(object sender, EventArgs e)
    {

        lblno.Visible = false;
        HAllSpread.Visible = false;
        int overalltot = 0;
        string exammonth = ddlMonth.SelectedValue.ToString();
        string examyear = ddlYear.SelectedValue.ToString();
        //string date = ddldate.SelectedValue.ToString();
        //string[] splitdate = date.Split(new Char[] { '-' });
        //string reqdate = splitdate[0].ToString();
        //string reqmonth = splitdate[1].ToString();
        //string reqyear = splitdate[2].ToString();
        //string datechange = reqyear + "-" + reqmonth + "-" + reqdate;
        //===first year
        if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
        {

            year++;
            // SELECT e.Batch_Year,c.Course_Name,d.Dept_Name,e.Current_Semester,Count(*) TotStud From Exam_Application A,Exam_Details E,Registration r,Degree G,Course C,Department D WHERE A.exam_code = E.exam_code and E.degree_code = G.Degree_Code and a.roll_no = r.roll_no and G.Course_Id = C.Course_Id and G.college_code = C.college_code and g.Dept_Code = d.dept_code and g.college_code = d.college_code and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and e.current_semester between 1 and 2 and c.college_code=" + Session["collegecode"].ToString() + " and r.Current_Semester = e.current_semester group by e.Batch_Year,Course_Name,Dept_Name,e.Current_Semester


            string spraedbind = "select  distinct e.batchto,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.semester as sem,d.degree_code as degree,dpt.dept_code as dptcode from exmtt e,exmtt_det ex,Department dpt,degree d,course c,registration r where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.semester between 1 and 2 and  c.college_code=" + Session["collegecode"].ToString() + "  and ex.exam_code=e.exam_code and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''  and cc=0 and delflag=0 and exam_flag<>'Debar' order by e.semester ";
            if (chkpassout.Checked == true)
            {
                spraedbind = "select  distinct e.batchto,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.semester as sem,d.degree_code as degree,dpt.dept_code as dptcode from exmtt e,exmtt_det ex,Department dpt,degree d,course c,registration r where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.semester between 1 and 2 and  c.college_code=" + Session["collegecode"].ToString() + "  and ex.exam_code=e.exam_code and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''  and delflag=0 and exam_flag<>'Debar' order by e.semester ";
            }
            if (chksupplym.Checked == true)
            {
                goto fouthyear;
            }
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
                    batchyear = Toadeptreader["batchto"].ToString();
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Note = batchyear;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Tag = degree;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Text = department;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Note = degree;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Tag = department_name;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 2].Text = course;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].Text = sem + "";
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                    string totstud = "select  isnull(count(*),0) as total from registration where degree_code='" + degree + "' and current_semester='" + sem + "' and college_code=" + Session["collegecode"].ToString() + " and cc=0  and delflag=0 and exam_flag<>'Debar'";
                    int semval = Convert.ToInt16(sem) + Convert.ToInt16(1);
                    if (chkpassout.Checked == true)
                    {
                        totstud = "select  isnull(count(*),0) as total from registration where degree_code='" + degree + "' and current_semester='" + semval + "' and college_code=" + Session["collegecode"].ToString() + "  and delflag=0 and exam_flag<>'Debar'";
                    }
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
            string spraedbind = "select  distinct e.batchto,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.semester as sem,d.degree_code as degree,dpt.dept_code as dptcode from exmtt e,exmtt_det ex,Department dpt,degree d,course c,registration r where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.semester between 3 and 4 and  c.college_code=" + Session["collegecode"].ToString() + " and ex.exam_code=e.exam_code and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''  and cc=0 and delflag=0 and exam_flag<>'Debar' order by e.semester ";
            if (chkpassout.Checked == true)
            {
                spraedbind = "select  distinct e.batchto,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.semester as sem,d.degree_code as degree,dpt.dept_code as dptcode from exmtt e,exmtt_det ex,Department dpt,degree d,course c,registration r where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.semester between 3 and 4 and  c.college_code=" + Session["collegecode"].ToString() + " and ex.exam_code=e.exam_code and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''   and delflag=0 and exam_flag<>'Debar' order by e.semester ";
            }
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
                    batchyear = Toadeptreader["batchto"].ToString();
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Note = batchyear;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Tag = degree;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Tag = department_name;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].Text = sem + "";
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Note = degree;

                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Text = department;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 2].Text = course;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                    string totstud = "select isnull(count(*),0) as total from registration where degree_code='" + degree + "' and current_semester='" + sem + "' and college_code=" + Session["collegecode"].ToString() + " and cc=0  and delflag=0 and exam_flag<>'Debar'";
                    int semval = Convert.ToInt16(sem) + Convert.ToInt16(1);
                    if (chkpassout.Checked == true)
                    {
                        totstud = "select isnull(count(*),0) as total from registration where degree_code='" + degree + "' and current_semester=" + semval + " and college_code=" + Session["collegecode"].ToString() + "  and delflag=0 and exam_flag<>'Debar'";
                    }
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
            string spraedbind = "select  distinct e.batchto,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.semester as sem,d.degree_code as degree,dpt.dept_code as dptcode from exmtt e,exmtt_det ex,Department dpt,degree d,course c,registration r where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.semester between 5 and 6 and  c.college_code=" + Session["collegecode"].ToString() + "  and ex.exam_code=e.exam_code and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''  and cc=0 and delflag=0 and exam_flag<>'Debar' order by e.semester ";
            if (chkpassout.Checked == true)
            {
                spraedbind = "select  distinct e.batchto,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.semester as sem,d.degree_code as degree,dpt.dept_code as dptcode from exmtt e,exmtt_det ex,Department dpt,degree d,course c,registration r where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.semester between 5 and 6 and  c.college_code=" + Session["collegecode"].ToString() + "  and ex.exam_code=e.exam_code and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''  and delflag=0 and exam_flag<>'Debar' order by e.semester ";
            }
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
                    batchyear = Toadeptreader["batchto"].ToString();
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Note = batchyear;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Tag = degree;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Tag = department_name;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].Text = sem + "";
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Note = degree;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Text = department;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 2].Text = course;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    string totstud = "select count(*)as total from registration where degree_code='" + degree + "' and current_semester='" + sem + "' and college_code=" + Session["collegecode"].ToString() + "  and cc=0 and delflag=0 and exam_flag<>'Debar'";
                    int semval = Convert.ToInt16(sem) + Convert.ToInt16(1);
                    if (chkpassout.Checked == true)
                    {
                        totstud = "select count(*)as total from registration where degree_code='" + degree + "' and current_semester='" + semval + "' and college_code=" + Session["collegecode"].ToString() + "   and delflag=0 and exam_flag<>'Debar'";
                    }
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
    fouthyear:
        if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
        {

            year++;
            string spraedbind = "select  distinct e.batchto,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.semester as sem,d.degree_code as degree,dpt.dept_code as dptcode from exmtt e,exmtt_det ex,Department dpt,degree d,course c,registration r where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.semester between 7 and 8 and  c.college_code=" + Session["collegecode"].ToString() + "  and ex.exam_code=e.exam_code and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''  and cc=0 and delflag=0 and exam_flag<>'Debar' order by e.semester ";
            if (chkpassout.Checked == true)
            {
                spraedbind = "select  distinct e.batchto,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.semester as sem,d.degree_code as degree,dpt.dept_code as dptcode from exmtt e,exmtt_det ex,Department dpt,degree d,course c,registration r , exam_application ea where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.semester=8 and ea.Exam_type=4 and ea.roll_no=r.Roll_No  and  c.college_code=" + Session["collegecode"].ToString() + "  and ex.exam_code=e.exam_code and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''  and delflag=0 and exam_flag<>'Debar' order by e.semester ";
            }

            if (chksupplym.Checked == true)
            {

                HAllSpread.Sheets[0].RowCount = 0;
                // spraedbind = "select r.batch_year as batchto,dt.Dept_Acronym as dept,dt.Dept_Name as deptname,c.Course_Name as course  ,      r.Current_Semester as sem , d.Degree_Code as degree ,dt.Dept_Code as dptcode   from exam_application ea ,Exam_Details ed,Degree d,Department dt,course c ,Registration r       where ea.exam_code =ed.exam_code  and d.Degree_Code =ed.degree_code and d.Course_Id =c.Course_Id       and d.Dept_Code =dt.Dept_Code and r.Roll_No =ea.roll_no and Exam_Month ='" + exammonth + "' and Exam_year ='" + examyear + "' and Exam_type=4        group by r.batch_year ,dt.Dept_Acronym,d.degree_code ,Course_Name,Dept_Name,r.Current_Semester, d.Degree_Code ,dt.Dept_Code";
                spraedbind = "select  distinct e.batchto,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.semester as sem,d.degree_code as degree,dpt.dept_code as dptcode from exmtt e,exmtt_det ex,Department dpt,degree d,course c,registration r where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id   and  c.college_code=" + Session["collegecode"].ToString() + "  and ex.exam_code=e.exam_code and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''  and cc=0 and delflag=0 and exam_flag<>'Debar' order by e.semester ";
            }
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
                    batchyear = Toadeptreader["batchto"].ToString();
                    HAllSpread.Visible = true;
                    HAllSpread.Sheets[0].RowCount = HAllSpread.Sheets[0].RowCount + 1;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Note = batchyear;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 0].Text = sn0 + "";
                    int semforyear = Convert.ToInt32(Toadeptreader["sem"].ToString());
                    if (chksupplym.Checked == true)
                    {
                        if (semforyear == 1 || semforyear == 2)
                        {
                            year = 1;
                        }

                        if (semforyear == 3 || semforyear == 4)
                        {
                            year = 2;
                        }
                        if (semforyear == 5 || semforyear == 6)
                        {
                            year = 3;
                        }
                        if (semforyear == 7 || semforyear == 8)
                        {
                            year = 4;
                        }
                        if (semforyear == 9 || semforyear == 10)
                        {
                            year = 5;
                        }
                    }

                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Text = year + "";
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    degree = Toadeptreader["degree"].ToString();
                    course = Toadeptreader["course"].ToString();
                    department = Toadeptreader["dept"].ToString();
                    sem = Toadeptreader["sem"].ToString();
                    department_code = Toadeptreader["dptcode"].ToString();
                    department_name = Toadeptreader["deptname"].ToString();
                    // batchyear = Toadeptreader["batchto"].ToString();
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Tag = degree;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Tag = department_name;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].Text = sem + "";
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Note = degree;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Text = department;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 2].Text = course;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    string totstud = "select count(*)as total from registration where degree_code='" + degree + "' and current_semester='" + sem + "' and college_code=" + Session["collegecode"].ToString() + "  and cc=0 and delflag=0 and exam_flag<>'Debar'";
                    int semval = Convert.ToInt16(sem) + Convert.ToInt16(1);
                    if (chkpassout.Checked == true)
                    {
                        totstud = "select count(*)as total from registration where degree_code='" + degree + "' and current_semester='" + semval + "' and college_code=" + Session["collegecode"].ToString() + "  and delflag=0 and exam_flag<>'Debar'";
                    }
                    if (chksupplym.Checked == true)
                    {
                        totstud = "select  count (distinct r.roll_no) as total from registration r, exam_application ea where degree_code='" + degree + "'  and college_code=" + Session["collegecode"].ToString() + "  and cc=0 and delflag=0 and ea.Exam_type=4 and ea.roll_no=r.Roll_No   and exam_flag<>'Debar'";
                    }
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
    FilfthYear:
        if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
        {

            year++;
            string spraedbind = "select  distinct e.batchto,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.semester as sem,d.degree_code as degree,dpt.dept_code as dptcode from exmtt e,exmtt_det ex,Department dpt,degree d,course c,registration r where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.semester between 9 and 10 and  c.college_code=" + Session["collegecode"].ToString() + "  and ex.exam_code=e.exam_code and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''  and cc=0 and delflag=0 and exam_flag<>'Debar' order by e.semester ";
            if (chkpassout.Checked == true)
            {
                spraedbind = "select  distinct e.batchto,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.semester as sem,d.degree_code as degree,dpt.dept_code as dptcode from exmtt e,exmtt_det ex,Department dpt,degree d,course c,registration r , exam_application ea where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id  and  e.semester=8 and ea.Exam_type=4 and ea.roll_no=r.Roll_No  and  c.college_code=" + Session["collegecode"].ToString() + "  and ex.exam_code=e.exam_code and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''  and delflag=0 and exam_flag<>'Debar' order by e.semester ";
            }

            if (chksupplym.Checked == true)
            {

                HAllSpread.Sheets[0].RowCount = 0;
                // spraedbind = "select r.batch_year as batchto,dt.Dept_Acronym as dept,dt.Dept_Name as deptname,c.Course_Name as course  ,      r.Current_Semester as sem , d.Degree_Code as degree ,dt.Dept_Code as dptcode   from exam_application ea ,Exam_Details ed,Degree d,Department dt,course c ,Registration r       where ea.exam_code =ed.exam_code  and d.Degree_Code =ed.degree_code and d.Course_Id =c.Course_Id       and d.Dept_Code =dt.Dept_Code and r.Roll_No =ea.roll_no and Exam_Month ='" + exammonth + "' and Exam_year ='" + examyear + "' and Exam_type=4        group by r.batch_year ,dt.Dept_Acronym,d.degree_code ,Course_Name,Dept_Name,r.Current_Semester, d.Degree_Code ,dt.Dept_Code";
                spraedbind = "select  distinct e.batchto,dpt.Dept_Acronym as dept,dpt.Dept_Name as deptname,c.course_name as course,e.semester as sem,d.degree_code as degree,dpt.dept_code as dptcode from exmtt e,exmtt_det ex,Department dpt,degree d,course c,registration r where d.Degree_Code=e.degree_code  and dpt.Dept_Code=d.Dept_Code and d.course_Id=c.Course_Id   and  c.college_code=" + Session["collegecode"].ToString() + "  and ex.exam_code=e.exam_code and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + " and ltrim(r.roll_no) <>''  and cc=0 and delflag=0 and exam_flag<>'Debar' order by e.semester ";
            }
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
                    batchyear = Toadeptreader["batchto"].ToString();
                    HAllSpread.Visible = true;
                    HAllSpread.Sheets[0].RowCount = HAllSpread.Sheets[0].RowCount + 1;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Note = batchyear;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 0].Text = sn0 + "";
                    int semforyear = Convert.ToInt32(Toadeptreader["sem"].ToString());
                    if (chksupplym.Checked == true)
                    {
                        if (semforyear == 1 || semforyear == 2)
                        {
                            year = 1;
                        }

                        if (semforyear == 3 || semforyear == 4)
                        {
                            year = 2;
                        }
                        if (semforyear == 5 || semforyear == 6)
                        {
                            year = 3;
                        }
                        if (semforyear == 7 || semforyear == 8)
                        {
                            year = 4;
                        }
                        if (semforyear == 9 || semforyear == 10)
                        {
                            year = 5;
                        }

                    }

                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Text = year + "";
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    degree = Toadeptreader["degree"].ToString();
                    course = Toadeptreader["course"].ToString();
                    department = Toadeptreader["dept"].ToString();
                    sem = Toadeptreader["sem"].ToString();
                    department_code = Toadeptreader["dptcode"].ToString();
                    department_name = Toadeptreader["deptname"].ToString();
                    // batchyear = Toadeptreader["batchto"].ToString();
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Tag = degree;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Tag = department_name;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].Text = sem + "";
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Note = degree;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Text = department;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 2].Text = course;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    string totstud = "select count(*)as total from registration where degree_code='" + degree + "' and current_semester='" + sem + "' and college_code=" + Session["collegecode"].ToString() + "  and cc=0 and delflag=0 and exam_flag<>'Debar'";
                    int semval = Convert.ToInt16(sem) + Convert.ToInt16(1);
                    if (chkpassout.Checked == true)
                    {
                        totstud = "select count(*)as total from registration where degree_code='" + degree + "' and current_semester='" + semval + "' and college_code=" + Session["collegecode"].ToString() + "  and delflag=0 and exam_flag<>'Debar'";
                    }
                    if (chksupplym.Checked == true)
                    {
                        totstud = "select  count (distinct r.roll_no) as total from registration r, exam_application ea where degree_code='" + degree + "'  and college_code=" + Session["collegecode"].ToString() + "  and cc=0 and delflag=0 and ea.Exam_type=4 and ea.roll_no=r.Roll_No   and exam_flag<>'Debar'";
                    }
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
        }
        else
        {
            lblno.Visible = false;
        }
    }

    protected void FpSpread2_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
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

    public void persentmonthcal(string rollno, string batchyear1, string degreecode1, string sem1)
    {

        int demfcal, demtcal;
        string monthcal;
        int mmyycount = 0;
        string getdate = "select start_date,end_date from seminfo where degree_code=" + degreecode1 + " and batch_year=" + batchyear1 + " and semester=" + sem1 + "";
        SqlDataAdapter dagetdate = new SqlDataAdapter(getdate, con1);
        DataSet dsgetdate = new DataSet();
        con1.Close();
        con1.Open();
        dagetdate.Fill(dsgetdate);
        if (dsgetdate.Tables[0].Rows.Count > 0)
        {
            frdate = dsgetdate.Tables[0].Rows[0]["start_date"].ToString();
            todate = dsgetdate.Tables[0].Rows[0]["end_date"].ToString();
        }
        //frdate = "20/05/2010";
        //todate = "20/05/2010";
        if (frdate != "" && todate != "")
        {
            string dt = frdate;
            string[] dsplitspace = dt.Split(new Char[] { ' ' });
            string dt1 = dsplitspace[0].ToString();
            string[] dsplit = dt1.Split(new Char[] { '/' });
            frdate = dsplit[2].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[1].ToString();
            demfcal = int.Parse(dsplit[2].ToString());
            demfcal = demfcal * 12;
            cal_from_date = demfcal + int.Parse(dsplit[0].ToString());
            monthcal = cal_from_date.ToString();
            dt = todate;
            dsplitspace = dt.Split(new Char[] { ' ' });
            dt1 = dsplitspace[0].ToString();
            dsplit = dt1.Split(new Char[] { '/' });
            todate = dsplit[2].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[1].ToString();
            demtcal = int.Parse(dsplit[2].ToString());
            demtcal = demtcal * 12;
            cal_to_date = demtcal + int.Parse(dsplit[0].ToString());
            per_from_date = Convert.ToDateTime(frdate);
            per_to_date = Convert.ToDateTime(todate);
            dumm_from_date = per_from_date;
            hat.Clear();
            hat.Add("std_rollno", rollno);
            hat.Add("from_month", cal_from_date);
            hat.Add("to_month", cal_to_date);
            ds2 = daccess2.select_method("STUD_ATTENDANCE", hat, "sp");

            hat.Clear();
            hat.Add("degree_code", int.Parse(Session["selecteddegreecode"].ToString()));
            hat.Add("sem", int.Parse(Session["semforsub"].ToString()));
            hat.Add("from_date", frdate.ToString());
            hat.Add("to_date", todate.ToString());
            hat.Add("coll_code", int.Parse(Session["collegecode"].ToString()));

            //ds3 = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
            //mmyycount  = ds2.Tables[0].Rows.Count;
            //moncount = mmyycount  - 1;

            //------------------------------------------------------------------
            int iscount = 0;
            holidaycon.Close();
            holidaycon.Open();
            string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + Session["selecteddegreecode"].ToString() + " and semester=" + Session["semforsub"].ToString() + "";
            SqlCommand cmdholiday = new SqlCommand(sqlstr_holiday, holidaycon);
            SqlDataAdapter daholiday = new SqlDataAdapter(cmdholiday);
            DataSet dsholiday = new DataSet();
            daholiday.Fill(dsholiday);
            if (dsholiday.Tables[0].Rows.Count > 0)
            {
                iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
            }
            hat.Add("iscount", iscount);
            //  ds3 = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
            mmyycount = ds2.Tables[0].Rows.Count;
            moncount = mmyycount - 1;

            ds3 = daccess2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
            //------------------------------------------------------------------
            if (ds3.Tables[0].Rows.Count != 0)
            {
                ts = DateTime.Parse(ds3.Tables[0].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                diff_date = Convert.ToString(ts.Days);
                dif_date1 = double.Parse(diff_date.ToString());
            }
            next = 0;
            int rowcount = 0;
            int ccount;
            ccount = ds3.Tables[1].Rows.Count;
            ccount = ccount - 1;
            if ((ds2.Tables[0].Rows.Count != 0) && (ds3.Tables[1].Rows.Count != 0))
            {

                //ccount = ccount - 1;
                //if ( == ds2.Tables [0].Rows [mmyycount].["Month_year"])
                while (dumm_from_date <= (per_to_date))
                {
                    //for (int i = 1; i <= mmyycount; i++)
                    //{
                    if (cal_from_date == int.Parse(ds2.Tables[0].Rows[next]["month_year"].ToString()))
                    {
                        if (dumm_from_date != DateTime.Parse(ds3.Tables[1].Rows[rowcount]["HOLI_DATE"].ToString()))
                        {
                            //    ts = DateTime.Parse(ds3.Tables[1].Rows[rowcount]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                            //    diff_date = Convert.ToString(ts.Days);
                            //    dif_date = double.Parse(diff_date.ToString());

                            for (i = 1; i <= fnhrs; i++)
                            {
                                date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                value = ds2.Tables[0].Rows[next][date].ToString();

                                if (value != null && value != "0" && value != "7" && value != "")
                                {
                                    if (tempvalue != value)
                                    {
                                        tempvalue = value;
                                        for (int j = 0; j < countds; j++)
                                        {

                                            if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                            {
                                                ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                j = countds;
                                            }
                                        }
                                    }

                                    if (ObtValue == 0)
                                    {
                                        per_perhrs += 1;
                                        tot_per_hrs += 1;
                                    }

                                }
                                else
                                {

                                    njhr += 1;

                                }

                            }

                            if (per_perhrs >= minpresI)
                            {
                                Present += 0.5;
                            }


                            else if (njhr == fnhrs)
                            {
                                njdate += 0.5;

                            }

                            per_perhrs = 0;

                            //    njhr = 0;

                            int k = i;
                            for (i = k; i <= NoHrs; i++)
                            {
                                date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                value = ds2.Tables[0].Rows[next][date].ToString();

                                if (value != null && value != "0" && value != "7" && value != "")
                                {
                                    if (tempvalue != value)
                                    {
                                        tempvalue = value;
                                        for (int j = 0; j < countds; j++)
                                        {

                                            if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                            {
                                                ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                j = countds;
                                            }
                                        }
                                    }
                                    if (ObtValue == 0)
                                    {
                                        per_perhrs += 1;
                                        tot_per_hrs += 1;
                                    }

                                }
                                else
                                {

                                    njhr += 1;

                                }

                            }
                            if (per_perhrs >= minpresII)
                            {
                                Present += 0.5;
                            }


                            else if (njhr == NoHrs)
                            {

                                njdate += 0.5;
                            }

                            per_perhrs = 0;

                            njhr = 0;


                            dumm_from_date = dumm_from_date.AddDays(1);
                            if (dumm_from_date.Day == 1)
                            {
                                cal_from_date++;
                                if (moncount > next)
                                {
                                    next++;

                                }
                            }

                            workingdays += 1;
                            per_perhrs = 0;

                        }
                        else
                        {
                            workingdays += 1;
                            dumm_from_date = dumm_from_date.AddDays(1);
                            if (dumm_from_date.Day == 1)
                            {
                                if (moncount > next)
                                {
                                    next++;

                                }
                            }
                            per_holidate += 1;
                            if (ccount > rowcount)
                            {
                                rowcount++;
                            }
                        }
                    }
                    else
                    {
                        DateTime dumm_fdate = dumm_from_date.AddDays(1 - Convert.ToInt16(dumm_from_date.Day.ToString()));
                        dumm_from_date = dumm_fdate;
                        dumm_fdate = dumm_fdate.AddMonths(1);
                        dumm_from_date = dumm_fdate;

                        if (dumm_from_date.Day == 1)
                        {



                            cal_from_date++;


                            if (moncount > next)
                            {
                                next++;
                                i++;
                            }

                        }



                        if (moncount > next)
                        {
                            i--;
                        }
                    }

                    //  }
                }//'----end while
                int diff_Date = per_from_date.Day - dumm_from_date.Day;
            }
        }
        per_tot_ondu = tot_ondu;
        per_njdate = njdate;
        pre_present_date = Present;
        per_per_hrs = tot_per_hrs;
        per_absent_date = Absent;
        pre_ondu_date = Onduty;
        pre_leave_date = Leave;
        per_workingdays = workingdays - per_holidate - per_njdate;
        per_dum_unmark = dum_unmark;

        Present = 0;
        tot_per_hrs = 0;
        Absent = 0;
        Onduty = 0;
        Leave = 0;
        workingdays = 0;
        per_holidate = 0;
        dum_unmark = 0;
        absent_point = 0;
        leave_point = 0;
        njdate = 0;
        tot_ondu = 0;
    }

    //-----------------------------------------------func to get the hash key---------------------------------
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

    protected void HAllSpread_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

        Cellclick = true;

        //Backbtn.Visible = true;
    }

    protected void HAllSpread_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {

            //for (int i = 0; i < FpSpread2.Sheets[0].RowCount; i++)
            //{ 
            
            
            
            //}
                if (Cellclick == true)
                {
                    printbtn.Visible = false;
                    FpSpread2.Visible = false;
                    Rangechk.Visible = false;

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

                    FpSpread2.Sheets[0].Columns[0].Locked = true;
                    FpSpread2.Sheets[0].Columns[1].Locked = true;
                    FpSpread2.Sheets[0].Columns[2].Locked = true;
                    FpSpread2.Sheets[0].Columns[3].Locked = true;
                    FpSpread2.Sheets[0].Columns[4].Locked = true;
                    FpSpread2.Sheets[0].Columns[5].Locked = true;
                    // FpSpread2.Sheets[0].Columns[6].Locked = true;
                    //FpSpread2.Sheets[0].AutoPostBack = true;
                    FpSpread2.Sheets[0].RowHeader.Visible = false;
                    MyStyle.Font.Size = FontUnit.Medium;
                    MyStyle.Font.Name = "Book Antiqua";
                    MyStyle.Font.Bold = true;
                    MyStyle.HorizontalAlign = HorizontalAlign.Center;
                    MyStyle.ForeColor = Color.Black;
                    MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    FpSpread2.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
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

                    ////////////////////////////////Added by sridharan 16.06.2015 for supplementy


                    if (chksupplym.Checked == true)
                    {
                        string studinfo = "select distinct len(r.reg_no),r.reg_no,r.stud_name,r.roll_no,r.batch_year from registration r, exam_application ea where  r.degree_code=" + depart_code + " and ea.Exam_type=4 and ea.roll_no=r.Roll_No   and cc=0 and delflag=0 and exam_flag<>'Debar' order by len(r.reg_no),r.reg_no,r.stud_name";


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
                                Rangechk.Visible = true;
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
                                    regpaper = "select count(ea.subject_no) as regularpap   from Exam_Details ed,exam_appl_details ea,exam_application e,subject s, syllabus_master sy,sub_sem su where ed.exam_code =e.exam_code  and e.appl_no =ea.appl_no   and  s.subject_no =ea.subject_no   and  su.syll_code =sy.syll_code and su.subType_no =s.subType_no   and  sy.syll_code =s.syll_code and e.roll_no ='" + rollno + "' and e.Exam_type=4 and ed.Exam_Month='" + ddlMonth.SelectedItem.Value + "' and ed.Exam_year='" + ddlYear.SelectedItem.Value + "'";
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
                                    if (CheckArrear.Checked == true && CheckRegular.Checked == false)
                                    {
                                        if (arrearpaper != "0")
                                        {
                                            sprdvisibleflag = 1;
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = arrearpaper;
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else
                                        {
                                            FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Visible = false;
                                        }
                                    }
                                }
                            }
                        }
                        goto supply;
                    }





                    /////////////end sridharan 16.06.2015

                    if (Checkeligible.Checked == true)
                    {
                        string studinfo = "select len(r.reg_no),r.reg_no,r.stud_name,r.roll_no,r.batch_year from registration r where r.current_semester=" + sem + "and r.degree_code=" + depart_code + "  and cc=0 and delflag=0 and exam_flag<>'Debar' order by len(r.reg_no),r.reg_no,r.stud_name";
                        if (chkpassout.Checked == true)
                        {
                            studinfo = "select len(r.reg_no),r.reg_no,r.stud_name,r.roll_no,r.batch_year from registration r where r.current_semester=" + sem + "and r.degree_code=" + depart_code + "   and delflag=0 and exam_flag<>'Debar' order by len(r.reg_no),r.reg_no,r.stud_name";
                        }
                        SqlDataAdapter dastudinfo = new SqlDataAdapter(studinfo, con1);
                        DataSet dsstudinfo = new DataSet();
                        con1.Close();
                        con1.Open();
                        dastudinfo.Fill(dsstudinfo);
                        if (dsstudinfo.Tables[0].Rows.Count > 0)
                        {
                            hat.Clear();
                            hat.Add("degree_code", depart_code);
                            hat.Add("sem_ester", int.Parse(sem));
                            ds = d2.select_method("period_attnd_schedule", hat, "sp");
                            if (ds.Tables[0].Rows.Count != 0)
                            {
                                NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                                fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                                anhrs = int.Parse(ds.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                                minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                                minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                                eligiblepercent = int.Parse(ds.Tables[0].Rows[0]["Eligible_Percent"].ToString());
                            }
                            hat.Clear();
                            hat.Add("colege_code", Session["collegecode"].ToString());
                            ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
                            countds = ds1.Tables[0].Rows.Count;
                            //'----------------------------------------new start---------------------------------------------
                            string dum_tage_date = "";
                            string dum_tage_hrs = "";
                            string strsec1 = "";
                            string rol_no = "";
                            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                            for (int att = 0; att < dsstudinfo.Tables[0].Rows.Count; att++)
                            {
                                string roll_no1 = dsstudinfo.Tables[0].Rows[att]["roll_no"].ToString();
                                persentmonthcal(roll_no1, batchyearatt, depart_code, sem);
                                per_tage_date = ((pre_present_date / per_workingdays) * 100);
                                if (per_tage_date > 100)
                                {
                                    per_tage_date = 100;
                                }
                                per_con_hrs = ((per_workingdays * NoHrs) - per_dum_unmark);
                                per_tage_hrs = ((per_per_hrs / per_con_hrs) * 100);
                                if (per_tage_hrs > 100)
                                {
                                    per_tage_hrs = 100;
                                }

                                dum_tage_date = String.Format("{0:0,0.00}", float.Parse(per_tage_date.ToString()));
                                dum_tage_hrs = String.Format("{0:0,0.00}", float.Parse(per_tage_hrs.ToString()));

                                if (dum_tage_hrs == "NaN")
                                {
                                    dum_tage_hrs = "0";
                                }
                                else if (dum_tage_hrs == "Infinity")
                                {
                                    dum_tage_hrs = "0";
                                }

                                if (dum_tage_date == "NaN")
                                {
                                    dum_tage_date = "0";
                                }
                                else if (dum_tage_date == "Infinity")
                                {
                                    dum_tage_date = "0";
                                }
                                //'------------------------------------------------new end------------
                                //'----------------------adding the percentage below 80 % to hash table-----------------------------
                                if (htattperc.Contains(Convert.ToString(dsstudinfo.Tables[0].Rows[student]["roll_no"].ToString())))
                                {
                                    int value1 = Convert.ToInt32(GetCorrespondingKey(Convert.ToString(rol_no), htattperc));
                                    value1++;//fail count
                                    htattperc[Convert.ToString(rol_no)] = value1;

                                }
                                else
                                {
                                    if (Convert.ToDouble(dum_tage_date) > Convert.ToDouble(eligiblepercent))
                                    {
                                        htattperc.Add(Convert.ToString(dsstudinfo.Tables[0].Rows[student]["roll_no"].ToString()), dum_tage_date.ToString());


                                        student++;

                                        //================

                                        string regno = "";
                                        string studname = "";
                                        string rollno = "";
                                        string batchyear = "";
                                        Label2.Visible = true;
                                        printbtn.Visible = true;
                                        FpSpread2.Visible = true;
                                        Rangechk.Visible = true;

                                        sno++;
                                        batchyear = dsstudinfo.Tables[0].Rows[att]["batch_year"].ToString();
                                        regno = dsstudinfo.Tables[0].Rows[att]["reg_no"].ToString();
                                        studname = dsstudinfo.Tables[0].Rows[att]["stud_name"].ToString();
                                        rollno = dsstudinfo.Tables[0].Rows[att]["roll_no"].ToString();
                                        FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 1;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = sno + "";
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Note = batchyear;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = txt;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = rollno;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = txt; // added by jairam 24-03-2015
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Note = rollno;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].CellType = txt;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = regno;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].CellType = txt;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = studname;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                        if (rollno != "")
                                        {
                                            if (CheckRegular.Checked == true)
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

                                            }
                                            if (CheckArrear.Checked == true)
                                            {
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
                                                if (CheckArrear.Checked == true && CheckRegular.Checked == false)
                                                {
                                                    if (arrearpaper != "0")
                                                    {
                                                        sprdvisibleflag = 1;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = arrearpaper;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                                    }
                                                    else
                                                    {
                                                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Visible = false;
                                                    }
                                                }
                                            }
                                        }
                                    }


                                }
                            }
                        }
                    }
                    if (Checkeligible.Checked != true)
                    {
                        string studinfo = "select len(r.reg_no),r.reg_no,r.stud_name,r.roll_no,r.batch_year from registration r where r.current_semester=" + sem + "and r.degree_code=" + depart_code + "  and cc=0 and delflag=0 and exam_flag<>'Debar' order by len(r.reg_no),r.reg_no,r.stud_name";
                        int semval = Convert.ToInt16(sem) + Convert.ToInt16(1);
                        if (chkpassout.Checked == true)
                        {
                            studinfo = "select len(r.reg_no),r.reg_no,r.stud_name,r.roll_no,r.batch_year from registration r where r.current_semester=" + semval + "and r.degree_code=" + depart_code + "   and delflag=0 and exam_flag<>'Debar' order by len(r.reg_no),r.reg_no,r.stud_name";
                        }
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
                                Rangechk.Visible = true;
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
                                    if (CheckArrear.Checked == true && CheckRegular.Checked == false)
                                    {
                                        if (arrearpaper != "0")
                                        {
                                            sprdvisibleflag = 1;
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = arrearpaper;
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else
                                        {
                                            FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Visible = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                //===============
                supply:
                    string totalrows = FpSpread2.Sheets[0].RowCount.ToString();
                    FpSpread2.Sheets[0].PageSize = (Convert.ToInt32(totalrows) * 20) + 40;
                    FpSpread2.Height = (Convert.ToInt32(totalrows) * 20) + 40;
                    if (CheckArrear.Checked == true || CheckRegular.Checked == true)
                    {
                        if (CheckRegular.Checked == true)
                        {
                            FpSpread2.Sheets[0].Columns[5].Visible = false;
                            if (Session["Rollflag"] == "0")
                            {
                                FpSpread2.Width = 610;
                                FpSpread2.Sheets[0].Columns[1].Visible = false;
                            }
                            if (Session["Regflag"] == "0")
                            {
                                FpSpread2.Width = 510;
                                FpSpread2.Sheets[0].Columns[2].Visible = false;
                            }
                        }
                        if (CheckArrear.Checked == true)
                        {
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, 5].Text = "Arrear";
                            FpSpread2.Sheets[0].Columns[4].Visible = false;
                            if (Session["Rollflag"] == "0")
                            {
                                FpSpread2.Width = 610;
                                FpSpread2.Sheets[0].Columns[1].Visible = false;
                            }
                            if (Session["Regflag"] == "0")
                            {
                                FpSpread2.Width = 510;
                                FpSpread2.Sheets[0].Columns[2].Visible = false;
                            }
                        }
                    }
                    if (CheckArrear.Checked == true && CheckRegular.Checked == true)
                    {
                        if (CheckRegular.Checked == true)
                        {
                            FpSpread2.Sheets[0].Columns[5].Visible = true;
                        }
                        if (CheckArrear.Checked == true)
                        {
                            FpSpread2.Sheets[0].Columns[4].Visible = true;
                        }
                        if (Session["Rollflag"] == "0")
                        {
                            FpSpread2.Width = 700;
                            FpSpread2.Sheets[0].Columns[1].Visible = false;
                        }
                        if (Session["Regflag"] == "0")
                        {
                            FpSpread2.Width = 600;
                            FpSpread2.Sheets[0].Columns[2].Visible = false;
                        }
                    }
                    if (sprdvisibleflag == 0)
                    {
                        FpSpread2.Visible = false;
                        printbtn.Visible = false;
                        Rangechk.Visible = false;
                        //Button2.Visible = false;
                        lblno.Visible = true;
                        Label2.Visible = false;
                    }
                    FpSpread2.SaveChanges();
                    for (int c = 0; c < FpSpread2.Sheets[0].RowCount; c++)
                    {
                        FpSpread2.Sheets[0].Cells[c, 6].Value = "0";
                    }
                    if (chksupplym.Checked == true)
                    {
                        FpSpread2.Sheets[0].ColumnHeader.Columns[5].Visible = false;
                    }
                    else
                    {
                        FpSpread2.Sheets[0].ColumnHeader.Columns[5].Visible = true;
                    }
                }
        }
        catch (Exception ex)
        {
            //lb1.Text = ex.ToString();
            //lb1.Visible = true;
        }
    }

    protected string GetUnivExamCode(string DegreeCode, int semester, int Batch)
    {
        string forexamcode = "Select Exam_Code as exmcode from Exam_Details where Degree_Code = '" + DegreeCode + "' and Current_Semester = " + semester + " and Batch_Year = " + Batch + "";
        SqlDataAdapter daexamcode = new SqlDataAdapter(forexamcode, conexam);
        string exam_code = "";
        DataSet dsexamcode = new DataSet();
        daexamcode.Fill(dsexamcode);
        conexam.Close();
        conexam.Open();

        if (dsexamcode.Tables[0].Rows.Count > 0)
        {
            exam_code = dsexamcode.Tables[0].Rows[0]["exmcode"].ToString();

        }
        //else
        //{
        //    exam_code = " ";
        //}
        return exam_code;
    }

    //protected void hallticketNECFormat()
    //{

    //    string exam_code = "";
    //    try
    //    {
    //        if ((ddlMonth.SelectedValue.ToString() != "0") && (ddlYear.SelectedValue.ToString() != "0"))
    //        {
    //            printspread.Sheets[0].SheetName = " ";
    //            //Button2.Visible = false;
    //            FpSpread2.SaveChanges();
    //            printspread.SaveChanges();
    //            printspread.Visible = true;
    //            printspread.Sheets[0].RowCount = 0;
    //            printspread.Sheets[0].AutoPostBack = true;
    //            printspread.Sheets[0].PageSize = 45;
    //            printspread.Sheets[0].ColumnCount = 7;
    //            printspread.Sheets[0].RowHeader.Visible = false;
    //            printspread.Sheets[0].ColumnHeader.Visible = false;
    //            printspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Small;
    //            printspread.Sheets[0].Columns[0].Width = 100;
    //            printspread.Sheets[0].Columns[1].Width = 100;
    //            printspread.Sheets[0].Columns[2].Width = 120;
    //            printspread.Sheets[0].Columns[3].Width = 100;
    //            printspread.Sheets[0].Columns[4].Width = 150;
    //            printspread.Sheets[0].Columns[5].Width = 150;
    //            printspread.Sheets[0].Columns[6].Width = 100;
    //            printspread.Sheets[0].Columns[0].Locked = true;
    //            printspread.Sheets[0].Columns[1].Locked = true;
    //            printspread.Sheets[0].Columns[2].Locked = true;
    //            printspread.Sheets[0].Columns[3].Locked = true;
    //            printspread.Sheets[0].Columns[4].Locked = true;
    //            printspread.Sheets[0].Columns[5].Locked = true;
    //            printspread.Sheets[0].Columns[6].Locked = true;
    //            printspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
    //            //printspread.Sheets[0].DefaultStyle.Font.Bold = true;
    //            string collnamenew1 = "";
    //            string address1 = "";
    //            string address3 = "";
    //            string address = "";
    //            string affliatedby = "";
    //            string catgory = "";
    //            string pincode = "";
    //            string affiliated = "";
    //            int bottomtext = 0;
    //            for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
    //            {

    //                int isval = 0;
    //                string s = FpSpread2.Sheets[0].Cells[res, 6].Text;

    //                isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);
    //                if (isval == 1)
    //                {

    //                    //for photo span
    //                    printspread.Sheets[0].RowCount = printspread.Sheets[0].RowCount + 45;
    //                    printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 45, 0, 5, 1);
    //                    printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 45, 6, 5, 1);
    //                    //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 34, 6, 4, 1);
    //                    //

    //                    printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 45, 1, 1, 5);
    //                    printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 44, 1, 1, 5);
    //                    printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 43, 1, 1, 5);
    //                    printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 42, 1, 1, 5);
    //                    printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 41, 1, 1, 5);
    //                    printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 40, 1, 1, 5);
    //                    //for reg spanning
    //                    //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 40, 0, 1, 2);
    //                    printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 40, 1, 1, 6);
    //                    printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 39, 0, 1, 2);
    //                    printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 39, 2, 1, 2);
    //                    printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 39, 5, 1, 2);
    //                    printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 38, 0, 1, 2);
    //                    printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 38, 2, 1, 2);
    //                    printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 38, 5, 1, 2);
    //                    printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 37, 0, 1, 2);
    //                    printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 37, 2, 1, 5);
    //                    printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 36, 0, 1, 2);
    //                    printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 36, 2, 1, 4);
    //                    //bottom page
    //                    //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 7, 0, 1, 3);
    //                    //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 3, 0, 1, 3);
    //                    //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 3, 4, 1, 3);
    //                    //for subjectname span
    //                    printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 34, 4, 1, 3);
    //                    //for session span
    //                    //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 11, 4, 1, 3);
    //                    //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 10, 4, 1, 3);
    //                    if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
    //                    {
    //                        string college = "select isnull(collname,'') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code=" + Session["collegecode"] + "";
    //                        SqlCommand collegecmd = new SqlCommand(college, con);
    //                        SqlDataReader collegename;
    //                        con.Close();
    //                        con.Open();
    //                        collegename = collegecmd.ExecuteReader();
    //                        if (collegename.HasRows)
    //                        {

    //                            while (collegename.Read())
    //                            {
    //                                affliatedby = collegename["affliated"].ToString();
    //                                catgory = collegename["category"].ToString();
    //                                collnamenew1 = collegename["collname"].ToString();
    //                                address1 = collegename["address1"].ToString();
    //                                address3 = collegename["address3"].ToString();
    //                                pincode = collegename["pincode"].ToString();
    //                                address = address1 + ", " + " " + address3 + "-" + " " + pincode + ".";
    //                                catgory = "(An " + catgory + " Institution";
    //                                affiliated = "Affiliated  to" + " " + affliatedby + ")";
    //                            }
    //                        }

    //                    }
    //                    MyImg collogo = new MyImg();
    //                    collogo.ImageUrl = "Handler/Handler2.ashx?";
    //                    MyImg collogoright = new MyImg();
    //                    collogoright.ImageUrl = "~/images/10BIT001.jpeg";
    //                    collogoright.ImageUrl = "Handler/Handler5.ashx?";
    //                    printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 45, 0].CellType = collogo;
    //                    //printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 45, 6].CellType = collogoright;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 0].HorizontalAlign = HorizontalAlign.Center;
    //                    string rollnosub = FpSpread2.Sheets[0].Cells[res, 1].Note;
    //                    MyImgphotp mi1 = new MyImgphotp();
    //                    mi1.ImageUrl = "Handler/Handler4.ashx?rollno=" + rollnosub;

    //                    string exammonth = ddlMonth.SelectedIndex.ToString();
    //                    string exammonthnew = ddlMonth.SelectedItem.Text;
    //                    string examyear = ddlYear.SelectedValue.ToString();
    //                    string exammonthnew1 = monthinwords(exammonthnew);
    //                    //printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 39, 6].CellType = mi1;
    //                    printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 45, 6].CellType = mi1;

    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 6].HorizontalAlign = HorizontalAlign.Center;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 1].Font.Size = FontUnit.Medium;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 1].Font.Bold = true;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 1].Text = collnamenew1 + "," + " " + address;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 44, 1].Text = catgory + " " + affiliated;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 43, 1].Text = "Office of the Controller of Examinations";
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 43, 1].Font.Bold = true;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 42, 1].Text = "UG/PG End Semester Examinations " + exammonthnew1 + " " + examyear + "";
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 41, 0].Margin.Left = 50;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 41, 1].Text = "HALL TICKET";
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 41, 1].Font.Bold = true;
    //                    string stuname = FpSpread2.Sheets[0].Cells[res, 3].Text;
    //                    string regnumber = FpSpread2.Sheets[0].Cells[res, 2].Text;
    //                    string sem = Session["semforsub"].ToString();
    //                    string degreecode = Session["selecteddegreecode"].ToString();
    //                    string batch = FpSpread2.Sheets[0].Cells[res, 0].Note;
    //                    string degree = Session["selecteddegree"].ToString();
    //                    string course = Session["selectedcourse"].ToString();
    //                    exam_code = GetFunction("select distinct exam_code from exmtt where degree_code=" + degreecode + " and exam_month=" + exammonth + " and exam_year=" + examyear + " and batchfrom=" + batch + " and semester=" + sem + " ");


    //                    string dateofbirth = "select convert(varchar(20),a.dob,103) as dobstudent from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollnosub + "'";
    //                    SqlDataAdapter da3 = new SqlDataAdapter(dateofbirth, con2);
    //                    string dob = "";
    //                    DataSet ds3 = new DataSet();
    //                    da3.Fill(ds3);
    //                    con2.Close();
    //                    con2.Open();

    //                    if (ds3.Tables[0].Rows.Count > 0)
    //                    {
    //                        dob = ds3.Tables[0].Rows[0]["dobstudent"].ToString();
    //                    }
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 0].Border.BorderColorBottom = Color.Black;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 1].Border.BorderColorBottom = Color.Black;
    //                    printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 39].Border.BorderColor = Color.Black;
    //                    printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 38].Border.BorderColor = Color.Black;
    //                    printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 37].Border.BorderColor = Color.Black;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 0].Margin.Left = 15;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 0].Text = "Register Number";
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 0].Margin.Left = 15;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 2].Margin.Left = 15;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 2].Text = " " + regnumber;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 4].Margin.Left = 15;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 4].Text = "Semester";
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 5].Margin.Left = 15;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 5].Text = " " + sem;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 0].Text = "Name";
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 2].Margin.Left = 15;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 2].Text = " " + stuname;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 4].Margin.Left = 15;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 4].Text = "Date Of Birth";
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 5].Margin.Left = 15;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 5].Text = " " + dob;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 0].Text = "Degree & Branch";
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 0].Margin.Left = 15;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 2].Margin.Left = 15;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 2].Text = degree + "." + " " + "(" + " " + course + ")";
    //                    //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 0].Text = "Programme" + ":";
    //                    //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 0].Margin.Left = 15;
    //                    //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 2].Text = degree + "-" + course;
    //                    //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 0].Text = "Branch" + ":";
    //                    //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 0].Margin.Left = 15;
    //                    //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 2].Text = course;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 1].HorizontalAlign = HorizontalAlign.Center;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 44, 1].HorizontalAlign = HorizontalAlign.Center;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 43, 1].HorizontalAlign = HorizontalAlign.Center;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 42, 1].HorizontalAlign = HorizontalAlign.Center;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 41, 1].HorizontalAlign = HorizontalAlign.Center;
    //                    printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 35].Border.BorderColorBottom = Color.Black;
    //                    printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 34].HorizontalAlign = HorizontalAlign.Center;
    //                    printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 34].Border.BorderColor = Color.Black;
    //                    printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 34].Font.Bold = true;
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 0].Text = "Sl.No";
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 1].Text = "Date";
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 2].Text = "Session";
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 3].Text = "Sub.Code";
    //                    printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 4].Text = "Subject Title";
    //                    string subject_nofromexmtt = "";
    //                    string time = "";
    //                    string time1 = "";
    //                    string subjectquery = "select distinct isnull(s.Subject_Code,'') as scode , isnull(s.subjecT_name,'') as sname , isnull(ed.subject_no,'') as subno, semester as ssem,ed.start_time,ed.end_time,ed.exam_date,ed.exam_session,feesub.lab from exmtt_det ed,exmtt e,subject s,sub_sem feesub  where feesub.subtype_no=s.subtype_no and s.subject_no=ed.subject_no and e.exam_code=ed.exam_code and e.exam_code=" + exam_code + "  order by exam_date asc,exam_session desc";
    //                    SqlDataAdapter da15 = new SqlDataAdapter(subjectquery, con1);
    //                    DataSet ds15 = new DataSet();
    //                    da15.Fill(ds15);
    //                    con1.Close();
    //                    int sno = 0;
    //                    int i1 = 0;
    //                    int subjectcount = 0;
    //                    int rowcount = 24;
    //                    int count = printspread.Sheets[0].RowCount - 33;
    //                    int regularcount = printspread.Sheets[0].RowCount - 33;
    //                    int countforregular = regularcount;
    //                    con1.Open();
    //                    int subarrregflag = 0;
    //                    if (ds15.Tables[0].Rows.Count > 0)
    //                    {
    //                        for (int i4 = 0; i4 < ds15.Tables[0].Rows.Count; i4++)
    //                        {
    //                            string subject_code1 = "";
    //                            string subject_name1 = "";
    //                            string subject_no2 = "";
    //                            string exam_code1 = "";
    //                            string arrearsem = "";
    //                            string exam_date = "";
    //                            string exam_session = "";
    //                            string labiv = "";
    //                            subject_nofromexmtt = ds15.Tables[0].Rows[i4]["subno"].ToString();

    //                            if (CheckArrear.Checked == true)
    //                            {
    //                                string Arrearsub = "Select distinct isnull(Subject_Code,'') as scode , isnull(subjecT_name,'') as sname , isnull(subject.subject_no,'') as subno, semester as ssem,e.start_time,e.end_time,e.exam_date,e.exam_session,feesub.lab from subject,syllabus_master smas,sub_sem feesub,exmtt_det e where feesub.subtype_no=subject.subtype_no and subject.subject_no=e.subject_no and  feesub.syll_code=subject.syll_code and feesub.promote_count=1 and smas.syll_code = subject.syll_code and subject.subject_no=" + subject_nofromexmtt + " and subject.subject_no in (select distinct subject_no from  mark_entry where mark_entry.subject_no in (select distinct mark_entry.subject_no from mark_entry where passorfail=0 and (result='Fail' or result='AAA') and roll_no='" + rollnosub + "') and roll_no='" + rollnosub + "' and Semester >= 1and Semester < " + Session["semforsub"].ToString() + "  ) and e.exam_code =" + exam_code + " order by smas.semester , scode,exam_date";
    //                                //string Arrearsub = "Select isnull(Subject_Code,'') as scode , isnull(subjecT_name,'') as sname , isnull(subject_no,'') as subno, semester as ssem from subject,syllabus_master as smas,sub_sem as feesub where feesub.subtype_no=subject.subtype_no and  feesub.syll_code=subject.syll_code and feesub.promote_count=1 and smas.syll_code = subject.syll_code and subject_no in (select distinct subject_no from  mark_entry where subject_no in (select distinct subject_no from mark_entry where passorfail=0 and result='Fail' and ltrim(rtrim(roll_no))='" + rollnosub + "') and ltrim(rtrim(roll_no))='" + rollnosub + "' and Semester >= 1 and Semester < " + Session["semforsub"].ToString() + " ) order by smas.semester , scode";
    //                                //string Arrearsub = "Select isnull(Subject_Code,'') as subcode , isnull(subjecT_name,'') as subname  ,sc.subject_no as subject_no, semester as ssem,feesub.fee_per_paper,feesub.arr_fee as arrearfees from subject as s,syllabus_master as smas,sub_sem as feesub where s.subject_no=sc.subject_no and s.subtype_no=sc.subtype_no and feesub.subtype_no=s.subtype_no and smas.syll_code = s.syll_code and subject_no in (select distinct subject_no from  mark_entry where subject_no in (select distinct subject_no from mark_entry where passorfail=0 and result='Fail' and ltrim(rtrim(roll_no))='" + rollnosub + "') and ltrim(rtrim(roll_no))='" + rollnosub + "' and Semester >= 1 and Semester < " + Session["semforsub"].ToString() + " ) order by smas.semester , scode";
    //                                SqlDataAdapter da8 = new SqlDataAdapter(Arrearsub, con3);
    //                                DataSet ds8 = new DataSet();
    //                                da8.Fill(ds8);
    //                                con3.Close();
    //                                con3.Open();



    //                                if (ds8.Tables[0].Rows.Count > 0)
    //                                {
    //                                    subarrregflag = 1;
    //                                    count = regularcount;
    //                                    //Button2.Visible = true;
    //                                    //int count = printspread.Sheets[0].RowCount - 33;
    //                                    //int i1 = 0;
    //                                    for (int i = 0; i < ds8.Tables[0].Rows.Count; i++)
    //                                    {
    //                                        sno++;
    //                                        subjectcount++;
    //                                        if (subjectcount > rowcount)
    //                                        {


    //                                            //===================
    //                                            rowcount = rowcount + 24;
    //                                            printspread.Sheets[0].RowCount = printspread.Sheets[0].RowCount + 45;
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 45, 0, 5, 1);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 45, 6, 5, 1);
    //                                            //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 34, 6, 4, 1);
    //                                            //

    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 45, 1, 1, 5);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 44, 1, 1, 5);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 43, 1, 1, 5);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 42, 1, 1, 5);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 41, 1, 1, 5);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 40, 1, 1, 5);
    //                                            //for reg spanning
    //                                            //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 40, 0, 1, 2);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 40, 1, 1, 6);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 39, 0, 1, 2);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 39, 2, 1, 2);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 39, 5, 1, 2);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 38, 0, 1, 2);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 38, 2, 1, 2);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 38, 5, 1, 2);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 37, 0, 1, 2);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 37, 2, 1, 5);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 36, 0, 1, 2);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 36, 2, 1, 4);
    //                                            //bottom page
    //                                            //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 7, 0, 1, 3);
    //                                            //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 3, 0, 1, 3);
    //                                            //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 3, 4, 1, 3);
    //                                            //for subjectname span
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 34, 4, 1, 3);
    //                                            //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 11, 4, 1, 3);
    //                                            //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 10, 4, 1, 3);
    //                                            if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
    //                                            {
    //                                                string college = "select isnull(collname,'') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code=" + Session["collegecode"] + "";
    //                                                SqlCommand collegecmd = new SqlCommand(college, con);
    //                                                SqlDataReader collegename;
    //                                                con.Close();
    //                                                con.Open();
    //                                                collegename = collegecmd.ExecuteReader();
    //                                                if (collegename.HasRows)
    //                                                {

    //                                                    while (collegename.Read())
    //                                                    {
    //                                                        affliatedby = collegename["affliated"].ToString();
    //                                                        catgory = collegename["category"].ToString();
    //                                                        collnamenew1 = collegename["collname"].ToString();
    //                                                        address1 = collegename["address1"].ToString();
    //                                                        address3 = collegename["address3"].ToString();
    //                                                        pincode = collegename["pincode"].ToString();
    //                                                        address = address1 + ", " + " " + address3 + "-" + " " + pincode + ".";
    //                                                        catgory = "(An " + catgory + " Institution";
    //                                                        affiliated = "Affiliated  to" + " " + affliatedby + ")";

    //                                                    }
    //                                                }

    //                                            }
    //                                            MyImg collogo1 = new MyImg();
    //                                            collogo1.ImageUrl = "Handler/Handler2.ashx?";
    //                                            MyImg collogoright2 = new MyImg();
    //                                            collogoright2.ImageUrl = "~/images/10BIT001.jpeg";
    //                                            collogoright2.ImageUrl = "Handler/Handler5.ashx?";
    //                                            printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 45, 0].CellType = collogo;
    //                                            //printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 45, 6].CellType = collogoright;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 0].HorizontalAlign = HorizontalAlign.Center;
    //                                            rollnosub = FpSpread2.Sheets[0].Cells[res, 1].Note;
    //                                            MyImgphotp mi2 = new MyImgphotp();
    //                                            mi2.ImageUrl = "Handler/Handler4.ashx?rollno=" + rollnosub;
    //                                            exammonthnew = ddlMonth.SelectedItem.Text;
    //                                            examyear = ddlYear.SelectedValue.ToString();
    //                                            exammonthnew1 = monthinwords(exammonthnew);
    //                                            //printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 39, 6].CellType = mi1;
    //                                            printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 45, 6].CellType = mi1;

    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 6].HorizontalAlign = HorizontalAlign.Center;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 1].Font.Size = FontUnit.Medium;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 1].Font.Bold = true;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 1].Text = collnamenew1 + "," + " " + address;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 44, 1].Text = catgory + " " + affiliated;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 43, 1].Text = "Office of the Controller of Examinations";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 43, 1].Font.Bold = true;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 42, 1].Text = "UG/PG End Semester Examinations " + exammonthnew1 + " " + examyear + "";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 41, 0].Margin.Left = 50;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 41, 1].Text = "HALL TICKET";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 41, 1].Font.Bold = true;
    //                                            stuname = FpSpread2.Sheets[0].Cells[res, 3].Text;
    //                                            regnumber = FpSpread2.Sheets[0].Cells[res, 2].Text;
    //                                            sem = Session["semforsub"].ToString();
    //                                            degreecode = Session["selecteddegreecode"].ToString();
    //                                            batch = FpSpread2.Sheets[0].Cells[res, 0].Note;
    //                                            degree = Session["selecteddegree"].ToString();
    //                                            course = Session["selectedcourse"].ToString();
    //                                            dateofbirth = "select convert(varchar(20),a.dob,103) as dobstudent from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollnosub + "'";
    //                                            SqlDataAdapter da14 = new SqlDataAdapter(dateofbirth, con2);
    //                                            dob = "";
    //                                            DataSet ds14 = new DataSet();
    //                                            da14.Fill(ds14);
    //                                            con2.Close();
    //                                            con2.Open();

    //                                            if (ds14.Tables[0].Rows.Count > 0)
    //                                            {
    //                                                dob = ds14.Tables[0].Rows[0]["dobstudent"].ToString();
    //                                            }
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 0].Border.BorderColorBottom = Color.Black;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 1].Border.BorderColorBottom = Color.Black;
    //                                            printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 39].Border.BorderColor = Color.Black;
    //                                            printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 38].Border.BorderColor = Color.Black;
    //                                            printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 37].Border.BorderColor = Color.Black;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 0].Margin.Left = 15;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 0].Text = "Register Number";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 0].Margin.Left = 15;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 2].Margin.Left = 15;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 2].Text = " " + regnumber;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 4].Margin.Left = 15;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 4].Text = "Semester";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 5].Margin.Left = 15;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 5].Text = " " + sem;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 0].Text = "Name";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 2].Margin.Left = 15;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 2].Text = " " + stuname;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 4].Margin.Left = 15;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 4].Text = "Date Of Birth";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 5].Margin.Left = 15;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 5].Text = " " + dob;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 0].Text = "Degree & Branch";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 0].Margin.Left = 15;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 2].Margin.Left = 15;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 2].Text = degree + "." + " " + "(" + " " + course + ")";
    //                                            //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 0].Text = "Programme" + ":";
    //                                            //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 0].Margin.Left = 15;
    //                                            //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 2].Text = degree + "-" + course;
    //                                            //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 0].Text = "Branch" + ":";
    //                                            //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 0].Margin.Left = 15;
    //                                            //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 2].Text = course;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 1].HorizontalAlign = HorizontalAlign.Center;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 44, 1].HorizontalAlign = HorizontalAlign.Center;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 43, 1].HorizontalAlign = HorizontalAlign.Center;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 42, 1].HorizontalAlign = HorizontalAlign.Center;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 41, 1].HorizontalAlign = HorizontalAlign.Center;
    //                                            printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 35].Border.BorderColorBottom = Color.Black;
    //                                            printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 34].HorizontalAlign = HorizontalAlign.Center;
    //                                            printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 34].Border.BorderColor = Color.Black;
    //                                            printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 34].Font.Bold = true;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 0].Text = "Sl.No";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 1].Text = "Date";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 2].Text = "Session";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 3].Text = "Sub.Code";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 4].Text = "Subject Title";
    //                                            count = printspread.Sheets[0].RowCount - 33;
    //                                            i1 = 0;
    //                                            //string excode = GetUnivExamCode(degreecode, Convert.ToInt32(sem), Convert.ToInt32(batch)).ToString();
    //                                            //if (excode != " ")
    //                                            //{


    //                                            //===================
    //                                        }


    //                                        subject_code1 = ds8.Tables[0].Rows[i]["scode"].ToString();
    //                                        subject_name1 = ds8.Tables[0].Rows[i]["sname"].ToString();
    //                                        subject_no2 = ds8.Tables[0].Rows[i]["subno"].ToString();
    //                                        arrearsem = ds8.Tables[0].Rows[i]["ssem"].ToString();
    //                                        exam_date = ds8.Tables[0].Rows[i]["exam_date"].ToString();

    //                                        DateTime obtaineddate = Convert.ToDateTime(exam_date);
    //                                        string examdatenew = obtaineddate.ToString("dd/MM/yyyy");
    //                                        exam_session = ds8.Tables[0].Rows[i]["exam_session"].ToString();
    //                                        //exam_code = ds4.Tables[0].Rows[0]["exmcode"].ToString();
    //                                        printspread.Sheets[0].Cells[count + i1, 0].Text = Convert.ToString(sno);
    //                                        printspread.Sheets[0].Cells[count + i1, 0].HorizontalAlign = HorizontalAlign.Center;
    //                                        printspread.Sheets[0].Cells[count + i1, 3].Text = subject_code1;
    //                                        printspread.Sheets[0].Cells[count + i1, 3].HorizontalAlign = HorizontalAlign.Center;
    //                                        printspread.Sheets[0].Cells[count + i1, 4].Margin.Left = 50;
    //                                        printspread.Sheets[0].Cells[count + i1, 4].Text = subject_name1;
    //                                        printspread.Sheets[0].Cells[count + i1, 4].HorizontalAlign = HorizontalAlign.Left;
    //                                        printspread.Sheets[0].Rows[count + i1].Border.BorderColor = Color.Black;
    //                                        printspread.Sheets[0].SpanModel.Add(count + i1, 4, 1, 3);
    //                                        printspread.Sheets[0].Cells[regularcount + i1, 1].CellType = txt;//Added By Srinath 21/3/2013
    //                                        printspread.Sheets[0].Cells[regularcount + i1, 1].Text = examdatenew;
    //                                        printspread.Sheets[0].Cells[regularcount + i1, 1].HorizontalAlign = HorizontalAlign.Center;
    //                                        printspread.Sheets[0].Cells[regularcount + i1, 2].Text = exam_session;
    //                                        printspread.Sheets[0].Cells[regularcount + i1, 2].HorizontalAlign = HorizontalAlign.Center;
    //                                        if (chkboxvdate.Checked == false && labiv == "True")
    //                                        {
    //                                            printspread.Sheets[0].AddSpanCell(regularcount + i1, 1, 1, 2);
    //                                            printspread.Sheets[0].Cells[regularcount + i1, 1].Text = "";
    //                                            printspread.Sheets[0].Cells[regularcount + i1, 1].HorizontalAlign = HorizontalAlign.Center;
    //                                            printspread.Sheets[0].Cells[regularcount + i1, 2].Text = "";
    //                                            printspread.Sheets[0].Cells[regularcount + i1, 2].HorizontalAlign = HorizontalAlign.Center;

    //                                        }
    //                                        regularcount = count; //+ i1+1;
    //                                        countforregular = regularcount;
    //                                        i1++;
    //                                    }
    //                                    bottomtext = regularcount + i1;
    //                                }
    //                            }
    //                            //for regular
    //                            //string examinforegular = "select isnull(Subject_Code,'') as subcode , isnull(subjecT_name,'') as subname ,sc.subject_no as subject_no, semester as ssem from subjectchooser sc,subject s,sub_sem as feesub where feesub.subtype_no=s.subtype_no and  feesub.syll_code=s.syll_code and feesub.promote_count=1 and s.subject_no=sc.subject_no and s.subtype_no=sc.subtype_no and s.subject_no="+subject_nofromexmtt +" and ltrim(rtrim(roll_no))='" + rollnosub + "' and semester=" + Session["semforsub"].ToString() + "";
    //                            if (CheckRegular.Checked == true)
    //                            {
    //                                string examinforegular = "select distinct isnull(Subject_Code,'') as subcode , isnull(subjecT_name,'') as subname ,sc.subject_no as subject_no, semester as ssem,e.start_time,e.end_time,e.exam_date,e.exam_session,feesub.lab from subjectchooser sc,subject s,sub_sem as feesub,exmtt_det as e where feesub.subtype_no=s.subtype_no and s.subject_no=e.subject_no and  feesub.syll_code=s.syll_code and feesub.promote_count=1 and s.subject_no=sc.subject_no and s.subject_no=" + subject_nofromexmtt + " and s.subtype_no=sc.subtype_no and roll_no='" + rollnosub + "' and semester=" + Session["semforsub"].ToString() + " order by exam_date";
    //                                SqlDataAdapter da4 = new SqlDataAdapter(examinforegular, con3);
    //                                string subject_code = "";
    //                                string subject_name = "";
    //                                string subject_no1 = "";
    //                                string lab1 = "";
    //                                //string exam_code = "";
    //                                //int subjectcount1 = 0;
    //                                //int rowcount1 = 20;
    //                                DataSet ds4 = new DataSet();
    //                                da4.Fill(ds4);
    //                                con3.Close();
    //                                con3.Open();

    //                                if (ds4.Tables[0].Rows.Count > 0)
    //                                {
    //                                    subarrregflag = 1;
    //                                    //i1++;
    //                                    //Button2.Visible = true;
    //                                    //int i1 = 0;
    //                                    for (int i = 0; i < ds4.Tables[0].Rows.Count; i++)
    //                                    {
    //                                        sno++;
    //                                        subjectcount++;
    //                                        regularcount = countforregular;
    //                                        if (subjectcount > rowcount)
    //                                        {
    //                                            rowcount = rowcount + 24;
    //                                            printspread.Sheets[0].RowCount = printspread.Sheets[0].RowCount + 45;
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 45, 0, 5, 1);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 45, 6, 5, 1);
    //                                            //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 34, 6, 4, 1);
    //                                            //

    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 45, 1, 1, 5);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 44, 1, 1, 5);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 43, 1, 1, 5);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 42, 1, 1, 5);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 41, 1, 1, 5);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 40, 1, 1, 5);
    //                                            //for reg spanning
    //                                            //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 40, 0, 1, 2);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 40, 1, 1, 6);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 39, 0, 1, 2);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 39, 2, 1, 2);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 39, 5, 1, 2);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 38, 0, 1, 2);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 38, 2, 1, 2);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 38, 5, 1, 2);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 37, 0, 1, 2);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 37, 2, 1, 5);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 36, 0, 1, 2);
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 36, 2, 1, 4);
    //                                            //bottom page
    //                                            //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 7, 0, 1, 3);
    //                                            //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 3, 0, 1, 3);
    //                                            //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 3, 4, 1, 3);
    //                                            //for subjectname span
    //                                            printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 34, 4, 1, 3);
    //                                            //for session span
    //                                            //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 11, 4, 1, 3);
    //                                            //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 10, 4, 1, 3);
    //                                            if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
    //                                            {
    //                                                string college = "select isnull(collname,'') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code=" + Session["collegecode"] + "";
    //                                                SqlCommand collegecmd = new SqlCommand(college, con);
    //                                                SqlDataReader collegename;
    //                                                con.Close();
    //                                                con.Open();
    //                                                collegename = collegecmd.ExecuteReader();
    //                                                if (collegename.HasRows)
    //                                                {

    //                                                    while (collegename.Read())
    //                                                    {
    //                                                        affliatedby = collegename["affliated"].ToString();
    //                                                        catgory = collegename["category"].ToString();
    //                                                        collnamenew1 = collegename["collname"].ToString();
    //                                                        address1 = collegename["address1"].ToString();
    //                                                        address3 = collegename["address3"].ToString();
    //                                                        pincode = collegename["pincode"].ToString();
    //                                                        address = address1 + ", " + " " + address3 + "-" + " " + pincode + ".";
    //                                                        catgory = "(An " + catgory + " Institution";
    //                                                        affiliated = "Affiliated  to" + " " + affliatedby + ")";

    //                                                    }
    //                                                }

    //                                            }
    //                                            MyImg collogo2 = new MyImg();
    //                                            collogo2.ImageUrl = "Handler/Handler2.ashx?";
    //                                            MyImgphotp collogoright3 = new MyImgphotp();
    //                                            collogoright3.ImageUrl = "~/images/10BIT001.jpeg";
    //                                            collogoright3.ImageUrl = "Handler/Handler5.ashx?";
    //                                            printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 45, 0].CellType = collogo;
    //                                            //printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 45, 6].CellType = collogoright;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 0].HorizontalAlign = HorizontalAlign.Center;
    //                                            rollnosub = FpSpread2.Sheets[0].Cells[res, 1].Note;
    //                                            MyImg mi2 = new MyImg();
    //                                            mi2.ImageUrl = "Handler/Handler4.ashx?rollno=" + rollnosub;
    //                                            exammonthnew = ddlMonth.SelectedItem.Text;
    //                                            examyear = ddlYear.SelectedValue.ToString();
    //                                            exammonthnew1 = monthinwords(exammonthnew);
    //                                            printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 45, 6].CellType = mi1;

    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 6].HorizontalAlign = HorizontalAlign.Center;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 1].Font.Size = FontUnit.Medium;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 1].Font.Bold = true;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 1].Text = collnamenew1 + "," + " " + address;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 44, 1].Text = catgory + " " + affiliated;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 43, 1].Text = "Office of the Controller of Examinations";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 43, 1].Font.Bold = true;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 42, 1].Text = "UG/PG End Semester Examinations " + exammonthnew1 + " " + examyear + "";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 41, 0].Margin.Left = 50;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 41, 1].Text = "HALL TICKET";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 41, 1].Font.Bold = true;

    //                                            stuname = FpSpread2.Sheets[0].Cells[res, 3].Text;
    //                                            regnumber = FpSpread2.Sheets[0].Cells[res, 2].Text;
    //                                            sem = Session["semforsub"].ToString();
    //                                            degreecode = Session["selecteddegreecode"].ToString();
    //                                            batch = FpSpread2.Sheets[0].Cells[res, 0].Note;
    //                                            degree = Session["selecteddegree"].ToString();
    //                                            course = Session["selectedcourse"].ToString();
    //                                            dateofbirth = "select convert(varchar(20),a.dob,103) as dobstudent from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollnosub + "'";
    //                                            SqlDataAdapter da14 = new SqlDataAdapter(dateofbirth, con2);
    //                                            dob = "";
    //                                            DataSet ds14 = new DataSet();
    //                                            da14.Fill(ds14);
    //                                            con2.Close();
    //                                            con2.Open();

    //                                            if (ds14.Tables[0].Rows.Count > 0)
    //                                            {
    //                                                dob = ds14.Tables[0].Rows[0]["dobstudent"].ToString();
    //                                            }
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 0].Border.BorderColorBottom = Color.Black;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 1].Border.BorderColorBottom = Color.Black;
    //                                            printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 39].Border.BorderColor = Color.Black;
    //                                            printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 38].Border.BorderColor = Color.Black;
    //                                            printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 37].Border.BorderColor = Color.Black;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 0].Margin.Left = 15;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 0].Text = "Register Number";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 0].Margin.Left = 15;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 2].Margin.Left = 15;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 2].Text = " " + regnumber;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 4].Margin.Left = 15;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 4].Text = "Semester";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 5].Margin.Left = 15;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 5].Text = " " + sem;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 0].Text = "Name";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 2].Margin.Left = 15;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 2].Text = " " + stuname;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 4].Margin.Left = 15;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 4].Text = "Date Of Birth";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 5].Margin.Left = 15;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 5].Text = " " + dob;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 0].Text = "Degree & Branch";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 0].Margin.Left = 15;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 2].Margin.Left = 15;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 2].Text = degree + "." + " " + "(" + " " + course + ")";
    //                                            //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 0].Text = "Programme" + ":";
    //                                            //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 0].Margin.Left = 15;
    //                                            //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 2].Text = degree + "-" + course;
    //                                            //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 0].Text = "Branch" + ":";
    //                                            //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 0].Margin.Left = 15;
    //                                            //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 2].Text = course;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 1].HorizontalAlign = HorizontalAlign.Center;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 44, 1].HorizontalAlign = HorizontalAlign.Center;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 43, 1].HorizontalAlign = HorizontalAlign.Center;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 42, 1].HorizontalAlign = HorizontalAlign.Center;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 41, 1].HorizontalAlign = HorizontalAlign.Center;
    //                                            printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 35].Border.BorderColorBottom = Color.Black;
    //                                            printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 34].HorizontalAlign = HorizontalAlign.Center;
    //                                            printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 34].Border.BorderColor = Color.Black;
    //                                            printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 34].Font.Bold = true;
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 0].Text = "Sl.No";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 1].Text = "Date";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 2].Text = "Session";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 3].Text = "Sub.Code";
    //                                            printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 4].Text = "Subject Title";

    //                                            regularcount = countforregular + 45;
    //                                            i1 = 0;

    //                                        }

    //                                        subject_code = ds4.Tables[0].Rows[i]["subcode"].ToString();
    //                                        subject_name = ds4.Tables[0].Rows[i]["subname"].ToString();
    //                                        subject_no1 = ds4.Tables[0].Rows[i]["subject_no"].ToString();
    //                                        exam_date = ds4.Tables[0].Rows[i]["exam_date"].ToString();
    //                                        lab1 = ds4.Tables[0].Rows[i]["lab"].ToString();

    //                                        DateTime obtaineddate = Convert.ToDateTime(exam_date);
    //                                        string examdatenew = obtaineddate.ToString("dd/MM/yyyy");
    //                                        exam_session = ds4.Tables[0].Rows[0]["exam_session"].ToString();
    //                                        //exam_code = ds4.Tables[0].Rows[0]["exmcode"].ToString();
    //                                        printspread.Sheets[0].Cells[regularcount + i1, 0].Text = Convert.ToString(sno);
    //                                        printspread.Sheets[0].Cells[regularcount + i1, 0].HorizontalAlign = HorizontalAlign.Center;
    //                                        printspread.Sheets[0].Cells[regularcount + i1, 3].Text = subject_code;
    //                                        printspread.Sheets[0].Cells[regularcount + i1, 3].HorizontalAlign = HorizontalAlign.Center;
    //                                        printspread.Sheets[0].Cells[regularcount + i1, 4].Margin.Left = 50;
    //                                        printspread.Sheets[0].Cells[regularcount + i1, 4].Text = subject_name;
    //                                        printspread.Sheets[0].Cells[regularcount + i1, 4].HorizontalAlign = HorizontalAlign.Left;
    //                                        printspread.Sheets[0].Rows[regularcount + i1].Border.BorderColor = Color.Black;
    //                                        printspread.Sheets[0].SpanModel.Add(regularcount + i1, 4, 1, 3);
    //                                        printspread.Sheets[0].Cells[regularcount + i1, 1].CellType = txt;//Added By Srinath 21/3/20132
    //                                        printspread.Sheets[0].Cells[regularcount + i1, 1].Text = examdatenew;
    //                                        printspread.Sheets[0].Cells[regularcount + i1, 1].HorizontalAlign = HorizontalAlign.Center;
    //                                        printspread.Sheets[0].Cells[regularcount + i1, 2].Text = exam_session;
    //                                        printspread.Sheets[0].Cells[regularcount + i1, 2].HorizontalAlign = HorizontalAlign.Center;
    //                                        if (chkboxvdate.Checked == false && lab1 == "True")
    //                                        {
    //                                            printspread.Sheets[0].AddSpanCell(regularcount + i1, 1, 1, 2);
    //                                            printspread.Sheets[0].Cells[regularcount + i1, 1].Text = "";
    //                                            printspread.Sheets[0].Cells[regularcount + i1, 1].HorizontalAlign = HorizontalAlign.Center;
    //                                            printspread.Sheets[0].Cells[regularcount + i1, 2].Text = "";
    //                                            printspread.Sheets[0].Cells[regularcount + i1, 2].HorizontalAlign = HorizontalAlign.Center;

    //                                        }
    //                                        //string examyear = ddlYear.SelectedValue.ToString();
    //                                        countforregular = regularcount;

    //                                        i1++;

    //                                    }

    //                                    bottomtext = regularcount + i1;

    //                                }
    //                            }
    //                            exammonth = ddlMonth.SelectedIndex.ToString();
    //                            if (exam_session == "F.N")
    //                            {
    //                                //for FN
    //                                string fntime = "select distinct convert(Varchar(8),ex.start_time,108) as start,convert(Varchar(8),ex.end_time,108) as end1 from exmtt e,exmtt_det ex  where ex.exam_session='F.N' and ex.start_time<> ex.end_time and e.exam_code=ex.exam_code and e.exam_code=" + exam_code + "";
    //                                SqlDataAdapter da7 = new SqlDataAdapter(fntime, con1);

    //                                string start_time1 = "";
    //                                string end_time1 = "";
    //                                DataSet ds7 = new DataSet();
    //                                da7.Fill(ds7);
    //                                con1.Close();
    //                                con1.Open();

    //                                if (ds7.Tables[0].Rows.Count > 0)
    //                                {
    //                                    start_time1 = ds7.Tables[0].Rows[0]["start"].ToString();
    //                                    end_time1 = ds7.Tables[0].Rows[0]["end1"].ToString();
    //                                    if ((start_time1 != "") && (end_time1 != ""))
    //                                    {
    //                                        string[] splitdate = start_time1.Split(new Char[] { ':' });
    //                                        string starthour2 = splitdate[0].ToString();
    //                                        string startmin2 = splitdate[1].ToString();
    //                                        string startsec2 = splitdate[2].ToString();
    //                                        start_time1 = starthour2 + "." + startmin2;
    //                                        string[] splitdate1 = end_time1.Split(new Char[] { ':' });
    //                                        string endhour2 = splitdate1[0].ToString();
    //                                        string endmin2 = splitdate1[1].ToString();
    //                                        string endsec2 = splitdate1[2].ToString();

    //                                        if (Convert.ToInt32(endhour2) > 12)
    //                                        {
    //                                            endhour2 = Convert.ToString(railwaytime(Convert.ToInt32(endhour2)));

    //                                        }
    //                                        end_time1 = endhour2 + "." + endmin2;
    //                                        //time = "*FN" + ":" + start_time1 + " " + "a.m" + " "+"-" + " " + end_time1 + " " + "AM";
    //                                        time = "FN" + " " + "-" + " " + "Forenoon" + " " + start_time1 + " " + "a.m" + " " + "-" + " " + end_time1 + " " + "p.m";

    //                                    }

    //                                }
    //                            }
    //                            //for AN
    //                            if (exam_session == "A.N")
    //                            {
    //                                string antime = "select distinct convert(Varchar(8),ex.start_time,108) as start,convert(Varchar(8),ex.end_time,108) as end1 from exmtt e,exmtt_det ex  where ex.exam_session='A.N' and ex.start_time<> ex.end_time and e.exam_code=ex.exam_code and e.exam_code=" + exam_code + " ";
    //                                SqlDataAdapter da6 = new SqlDataAdapter(antime, con1);

    //                                string start_time = "";
    //                                string end_time = "";
    //                                DataSet ds6 = new DataSet();
    //                                da6.Fill(ds6);
    //                                con1.Close();
    //                                con1.Open();

    //                                if (ds6.Tables[0].Rows.Count > 0)
    //                                {
    //                                    start_time = ds6.Tables[0].Rows[0]["start"].ToString();
    //                                    end_time = ds6.Tables[0].Rows[0]["end1"].ToString();
    //                                    if ((start_time != "") && (end_time != ""))
    //                                    {
    //                                        string[] splitdate = start_time.Split(new Char[] { ':' });
    //                                        string starthour1 = splitdate[0].ToString();
    //                                        string startmin1 = splitdate[1].ToString();
    //                                        string startsec1 = splitdate[2].ToString();

    //                                        string[] splitdate1 = end_time.Split(new Char[] { ':' });
    //                                        string endhour1 = splitdate1[0].ToString();
    //                                        string endmin1 = splitdate1[1].ToString();
    //                                        string endsec1 = splitdate1[2].ToString();

    //                                        if (Convert.ToInt32(starthour1) > 12)
    //                                        {
    //                                            starthour1 = Convert.ToString(railwaytime(Convert.ToInt32(starthour1)));

    //                                        }
    //                                        if (Convert.ToInt32(endhour1) > 12)
    //                                        {
    //                                            endhour1 = Convert.ToString(railwaytime(Convert.ToInt32(endhour1)));

    //                                        }
    //                                        start_time = starthour1 + "." + startmin1;
    //                                        end_time = endhour1 + "." + endmin1;
    //                                        //time1 = "*AN" + ":" + start_time + " " + "PM" + "to" + " " + end_time + " " + "PM";
    //                                        time1 = "AN" + " " + "-" + " " + "Afternoon" + " " + start_time + " " + "p.m" + " " + "-" + " " + end_time + " " + "p.m";

    //                                    }


    //                                }
    //                            }

    //                            if (subarrregflag == 1)
    //                            {
    //                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 9, 0, 1, 7);
    //                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 6, 0, 1, 7);
    //                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 1, 0, 1, 7);
    //                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 0, 0, 1, 7);
    //                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 1].Border.BorderColorBottom = Color.Black;
    //                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 9].Border.BorderColorBottom = Color.Black;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 6, 0].Border.BorderColorBottom = Color.Black;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 0].Text = "Note: If any discrepancies are found in the Hall Ticket, report to the COE office immediately.";
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Small; printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 3, 0, 1, 3);
    //                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 2, 4, 1, 3);
    //                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 2, 0, 1, 3);
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 0].Margin.Left = 20;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 0].Text = "Signature of the Candidate";
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 4].HorizontalAlign = HorizontalAlign.Right;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 4].Margin.Right = 30;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 4].Text = "Controller of Examinations";
    //                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 8, 0, 1, 3);
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 8, 0].Margin.Left = 15;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 8, 0].Text = "No. of Subjects Registered" + " " + sno;
    //                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 8, 4, 1, 3);
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 8, 4].Text = time;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 8, 4].HorizontalAlign = HorizontalAlign.Right;
    //                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 7, 4, 1, 3);
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 7, 4].Text = time1;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 7, 4].HorizontalAlign = HorizontalAlign.Right;
    //                                /////===
    //                                //printspread.Sheets[0].SpanModel.Add(bottomtext + 6, 0, 1, 3);
    //                                //printspread.Sheets[0].SpanModel.Add(bottomtext + 6, 4, 1, 3);
    //                                //printspread.Sheets[0].Cells[bottomtext + 6, 0].Margin.Left = 20;
    //                                //printspread.Sheets[0].Cells[bottomtext + 6, 0].Text = "Signature of the Candidate";
    //                                //printspread.Sheets[0].Cells[bottomtext + 6, 4].HorizontalAlign = HorizontalAlign.Center;
    //                                //printspread.Sheets[0].Cells[bottomtext + 6, 4].Text = "Controller of Examinations";
    //                                //printspread.Sheets[0].SpanModel.Add(bottomtext + 2, 4, 1, 3);
    //                                //printspread.Sheets[0].Cells[bottomtext + 2, 4].Text = time;
    //                                //printspread.Sheets[0].Cells[bottomtext + 2, 4].HorizontalAlign = HorizontalAlign.Right;
    //                                //printspread.Sheets[0].SpanModel.Add(bottomtext + 3, 4, 1, 3);
    //                                //printspread.Sheets[0].Cells[bottomtext + 3, 4].Text = time1;
    //                                //printspread.Sheets[0].Cells[bottomtext + 3, 4].HorizontalAlign = HorizontalAlign.Right;
    //                                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Click Print Button to view HallTicket')", true);
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 0].Border.BorderColor = Color.Black;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 1].Border.BorderColor = Color.Black;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 2].Border.BorderColor = Color.Black;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 3].Border.BorderColor = Color.Black;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 4].Border.BorderColor = Color.Black;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 5].Border.BorderColor = Color.Black;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 6].Border.BorderColor = Color.Black;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 0].Border.BorderColorTop = Color.White;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 1].Border.BorderColorTop = Color.White;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 2].Border.BorderColorTop = Color.White;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 3].Border.BorderColorTop = Color.White;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 4].Border.BorderColorTop = Color.White;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 5].Border.BorderColorTop = Color.White;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 6].Border.BorderColorTop = Color.White;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 0].Border.BorderColorRight = Color.White;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 1].Border.BorderColorRight = Color.White;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 2].Border.BorderColorRight = Color.White;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 3].Border.BorderColorRight = Color.White;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 4].Border.BorderColorRight = Color.White;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 5].Border.BorderColorRight = Color.White;
    //                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 6].Border.BorderColorRight = Color.White;
    //                            }
    //                        }
    //                    }
    //                    FpSpread2.Sheets[0].Cells[res, 6].Value = 0;
    //                }
    //            }
    //        }
    //    }
    //    catch
    //    {

    //    }
    //}

    protected void hallticketNECFormat()
    {

        string exam_code = "";
        try
        {
            if ((ddlMonth.SelectedValue.ToString() != "0") && (ddlYear.SelectedValue.ToString() != "0"))
            {
                printspread.Sheets[0].SheetName = " ";
                //Button2.Visible = false;
                FpSpread2.SaveChanges();
                printspread.SaveChanges();
                printspread.Visible = true;
                printspread.Sheets[0].RowCount = 0;
                printspread.Sheets[0].AutoPostBack = true;
                printspread.Sheets[0].PageSize = 46;
                printspread.Sheets[0].ColumnCount = 7;
                printspread.Sheets[0].RowHeader.Visible = false;
                printspread.Sheets[0].ColumnHeader.Visible = false;
                printspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Small;
                printspread.Sheets[0].Columns[0].Width = 100;
                printspread.Sheets[0].Columns[1].Width = 100;
                printspread.Sheets[0].Columns[2].Width = 120;
                printspread.Sheets[0].Columns[3].Width = 100;
                printspread.Sheets[0].Columns[4].Width = 150;
                printspread.Sheets[0].Columns[5].Width = 150;
                printspread.Sheets[0].Columns[6].Width = 100;
                printspread.Sheets[0].Columns[0].Locked = true;
                printspread.Sheets[0].Columns[1].Locked = true;
                printspread.Sheets[0].Columns[2].Locked = true;
                printspread.Sheets[0].Columns[3].Locked = true;
                printspread.Sheets[0].Columns[4].Locked = true;
                printspread.Sheets[0].Columns[5].Locked = true;
                printspread.Sheets[0].Columns[6].Locked = true;
                printspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                //printspread.Sheets[0].DefaultStyle.Font.Bold = true;
                string collnamenew1 = "";
                string address1 = "";
                string address3 = "";
                string address = "";
                string affliatedby = "";
                string catgory = "";
                string pincode = "";
                string affiliated = "";
                int bottomtext = 0;
                for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
                {

                    int isval = 0;
                    string s = FpSpread2.Sheets[0].Cells[res, 6].Text;

                    isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);
                    if (isval == 1)
                    {

                        //for photo span
                        printspread.Sheets[0].RowCount = printspread.Sheets[0].RowCount + 46;
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (45 + 1), 0, 5, 1);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (45 + 1), 6, 5, 1);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (45 + 1), 1, 1, 5);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (44 + 1), 1, 1, 5);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (43 + 1), 1, 1, 5);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (42 + 1), 1, 1, 5);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (41 + 1), 1, 1, 5);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (40 + 1), 1, 1, 5);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (40 + 1), 1, 1, 6);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (39 + 1), 0, 1, 2);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (39 + 1), 2, 1, 2);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (39 + 1), 5, 1, 2);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (38 + 1), 0, 1, 2);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (38 + 1), 2, 1, 2);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (38 + 1), 5, 1, 2);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (37 + 1), 0, 1, 2);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (37 + 1), 2, 1, 5);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (36 + 1), 0, 1, 2);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (36 + 1), 2, 1, 4);
                        //for subjectname span
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (34 + 1), 4, 1, 3);

                        if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                        {
                            string college = "select isnull(collname,'') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code=" + Session["collegecode"] + "";
                            SqlCommand collegecmd = new SqlCommand(college, con);
                            SqlDataReader collegename;
                            con.Close();
                            con.Open();
                            collegename = collegecmd.ExecuteReader();
                            if (collegename.HasRows)
                            {

                                while (collegename.Read())
                                {
                                    affliatedby = collegename["affliated"].ToString();
                                    catgory = collegename["category"].ToString();
                                    collnamenew1 = collegename["collname"].ToString();
                                    address1 = collegename["address1"].ToString();
                                    address3 = collegename["address3"].ToString();
                                    pincode = collegename["pincode"].ToString();
                                    address = address1 + ", " + " " + address3 + "-" + " " + pincode + ".";

                                    catgory = "An " + catgory + " Institution";


                                    affiliated = "Affiliated  to" + " " + affliatedby;
                                }
                            }

                        }
                        MyImg collogo = new MyImg();
                        collogo.ImageUrl = "Handler/Handler2.ashx?";
                        MyImg collogoright = new MyImg();
                        collogoright.ImageUrl = "~/images/10BIT001.jpeg";
                        collogoright.ImageUrl = "Handler/Handler5.ashx?";
                        printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - (45 + 1), 0].CellType = collogo;
                        //printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 45, 6].CellType = collogoright;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (45 + 1), 0].HorizontalAlign = HorizontalAlign.Center;
                        string rollnosub = FpSpread2.Sheets[0].Cells[res, 1].Note;
                        MyImgphotp mi1 = new MyImgphotp();
                        mi1.ImageUrl = "Handler/Handler4.ashx?rollno=" + rollnosub;

                        string exammonth = ddlMonth.SelectedIndex.ToString();
                        string exammonthnew = ddlMonth.SelectedItem.Text;
                        string examyear = ddlYear.SelectedValue.ToString();
                        string exammonthnew1 = monthinwords(exammonthnew);
                        printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - (45 + 1), 6].CellType = mi1;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (45 + 1), 6].HorizontalAlign = HorizontalAlign.Center;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (45 + 1), 1].Font.Size = FontUnit.Medium;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (45 + 1), 1].Font.Bold = true;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (45 + 1), 1].Text = collnamenew1 + "," + " " + address;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (44 + 1), 1].Text = catgory + " " + affiliated;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (43 + 1), 1].Text = "Office of the Controller of Examinations";
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (43 + 1), 1].Font.Bold = true;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (42 + 1), 1].Text = "UG/PG Degree End Semester Examinations " + exammonthnew1 + " " + examyear + "";
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (41 + 1), 0].Margin.Left = 50;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (41 + 1), 1].Text = "HALL TICKET";
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (41 + 1), 1].Font.Bold = true;
                        string stuname = FpSpread2.Sheets[0].Cells[res, 3].Text;
                        string regnumber = FpSpread2.Sheets[0].Cells[res, 2].Text;
                        string sem = Session["semforsub"].ToString();
                        string degreecode = Session["selecteddegreecode"].ToString();
                        string batch = FpSpread2.Sheets[0].Cells[res, 0].Note;
                        string degree = Session["selecteddegree"].ToString();
                        string course = Session["selectedcourse"].ToString();
                        exam_code = GetFunction("select distinct exam_code from exmtt where degree_code=" + degreecode + " and exam_month=" + exammonth + " and exam_year=" + examyear + " and batchfrom=" + batch + " and semester=" + sem + " ");


                        string dateofbirth = "select convert(varchar(20),a.dob,103) as dobstudent from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollnosub + "'";
                        SqlDataAdapter da3 = new SqlDataAdapter(dateofbirth, con2);
                        string dob = "";
                        DataSet ds3 = new DataSet();
                        da3.Fill(ds3);
                        con2.Close();
                        con2.Open();

                        if (ds3.Tables[0].Rows.Count > 0)
                        {
                            dob = ds3.Tables[0].Rows[0]["dobstudent"].ToString();
                        }
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (40 + 1), 0].Border.BorderColorBottom = Color.Black;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (40 + 1), 1].Border.BorderColorBottom = Color.Black;
                        printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - (39 + 1)].Border.BorderColor = Color.Black;
                        printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - (38 + 1)].Border.BorderColor = Color.Black;
                        printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - (37 + 1)].Border.BorderColor = Color.Black;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 0].Margin.Left = 15;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 0].Text = "Register Number";
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 0].Margin.Left = 15;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 2].Margin.Left = 15;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 2].Text = " " + regnumber;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 4].Margin.Left = 15;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 4].Text = "Semester";
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 5].Margin.Left = 15;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 5].Text = " " + sem;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 0].Text = "Name";
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 2].Margin.Left = 15;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 2].Text = " " + stuname;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 4].Margin.Left = 15;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 4].Text = "Date of Birth";
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 5].Margin.Left = 15;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 5].Text = " " + dob;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (37 + 1), 0].Text = "Degree & Branch";
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (37 + 1), 0].Margin.Left = 15;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (37 + 1), 2].Margin.Left = 15;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (37 + 1), 2].Text = degree + "." + " " + "(" + " " + course + ")";
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (45 + 1), 1].HorizontalAlign = HorizontalAlign.Center;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (44 + 1), 1].HorizontalAlign = HorizontalAlign.Center;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (43 + 1), 1].HorizontalAlign = HorizontalAlign.Center;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (42 + 1), 1].HorizontalAlign = HorizontalAlign.Center;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (41 + 1), 1].HorizontalAlign = HorizontalAlign.Center;
                        printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - (35 + 1)].Border.BorderColorBottom = Color.Black;
                        printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - (34 + 1)].HorizontalAlign = HorizontalAlign.Center;
                        printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - (34 + 1)].Border.BorderColor = Color.Black;
                        printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - (34 + 1)].Font.Bold = true;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (34 + 1), 0].Text = "Sl.No";
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (34 + 1), 1].Text = "Date";
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (34 + 1), 2].Text = "Session";
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (34 + 1), 3].Text = "Sub.Code";
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (34 + 1), 4].Text = "Subject Title";
                        string subject_nofromexmtt = "";
                        string time = "";
                        string time1 = "";
                        string subjectquery = "select distinct isnull(s.Subject_Code,'') as scode , isnull(s.subjecT_name,'') as sname , isnull(ed.subject_no,'') as subno, semester as ssem,ed.start_time,ed.end_time,ed.exam_date,ed.exam_session,feesub.lab from exmtt_det ed,exmtt e,subject s,sub_sem feesub  where feesub.subtype_no=s.subtype_no and s.subject_no=ed.subject_no and e.exam_code=ed.exam_code and e.exam_code=" + exam_code + "  order by exam_date asc,exam_session desc";
                        SqlDataAdapter da15 = new SqlDataAdapter(subjectquery, con1);
                        DataSet ds15 = new DataSet();
                        da15.Fill(ds15);
                        con1.Close();
                        int sno = 0;
                        int i1 = 0;
                        int subjectcount = 0;
                        int rowcount = (24 + 3);
                        int count = printspread.Sheets[0].RowCount - (33 + 1);
                        int regularcount = printspread.Sheets[0].RowCount - (33 + 1);
                        int countforregular = regularcount;
                        con1.Open();
                        int subarrregflag = 0;
                        if (ds15.Tables[0].Rows.Count > 0)
                        {
                            for (int i4 = 0; i4 < ds15.Tables[0].Rows.Count; i4++)
                            {
                                string subject_code1 = "";
                                string subject_name1 = "";
                                string subject_no2 = "";
                                string exam_code1 = "";
                                string arrearsem = "";
                                string exam_date = "";
                                string exam_session = "";
                                string labiv = "";
                                subject_nofromexmtt = ds15.Tables[0].Rows[i4]["subno"].ToString();

                                if (CheckArrear.Checked == true)
                                {
                                    string Arrearsub = "Select distinct isnull(Subject_Code,'') as scode , isnull(subjecT_name,'') as sname , isnull(subject.subject_no,'') as subno, semester as ssem,e.start_time,e.end_time,e.exam_date,e.exam_session,feesub.lab from subject,syllabus_master smas,sub_sem feesub,exmtt_det e where feesub.subtype_no=subject.subtype_no and subject.subject_no=e.subject_no and  feesub.syll_code=subject.syll_code and feesub.promote_count=1 and smas.syll_code = subject.syll_code and subject.subject_no=" + subject_nofromexmtt + " and subject.subject_no in (select distinct subject_no from  mark_entry where mark_entry.subject_no in (select distinct mark_entry.subject_no from mark_entry where passorfail=0 and (result='Fail' or result='AAA') and roll_no='" + rollnosub + "') and roll_no='" + rollnosub + "' and Semester >= 1and Semester < " + Session["semforsub"].ToString() + "  ) and e.exam_code =" + exam_code + " order by smas.semester , scode,exam_date";
                                    //string Arrearsub = "Select isnull(Subject_Code,'') as scode , isnull(subjecT_name,'') as sname , isnull(subject_no,'') as subno, semester as ssem from subject,syllabus_master as smas,sub_sem as feesub where feesub.subtype_no=subject.subtype_no and  feesub.syll_code=subject.syll_code and feesub.promote_count=1 and smas.syll_code = subject.syll_code and subject_no in (select distinct subject_no from  mark_entry where subject_no in (select distinct subject_no from mark_entry where passorfail=0 and result='Fail' and ltrim(rtrim(roll_no))='" + rollnosub + "') and ltrim(rtrim(roll_no))='" + rollnosub + "' and Semester >= 1 and Semester < " + Session["semforsub"].ToString() + " ) order by smas.semester , scode";
                                    //string Arrearsub = "Select isnull(Subject_Code,'') as subcode , isnull(subjecT_name,'') as subname  ,sc.subject_no as subject_no, semester as ssem,feesub.fee_per_paper,feesub.arr_fee as arrearfees from subject as s,syllabus_master as smas,sub_sem as feesub where s.subject_no=sc.subject_no and s.subtype_no=sc.subtype_no and feesub.subtype_no=s.subtype_no and smas.syll_code = s.syll_code and subject_no in (select distinct subject_no from  mark_entry where subject_no in (select distinct subject_no from mark_entry where passorfail=0 and result='Fail' and ltrim(rtrim(roll_no))='" + rollnosub + "') and ltrim(rtrim(roll_no))='" + rollnosub + "' and Semester >= 1 and Semester < " + Session["semforsub"].ToString() + " ) order by smas.semester , scode";
                                    SqlDataAdapter da8 = new SqlDataAdapter(Arrearsub, con3);
                                    DataSet ds8 = new DataSet();
                                    da8.Fill(ds8);
                                    con3.Close();
                                    con3.Open();



                                    if (ds8.Tables[0].Rows.Count > 0)
                                    {
                                        subarrregflag = 1;
                                        count = regularcount;
                                        for (int i = 0; i < ds8.Tables[0].Rows.Count; i++)
                                        {
                                            sno++;
                                            subjectcount++;
                                            if (subjectcount > rowcount)
                                            {
                                                printspread.Sheets[0].RowCount = printspread.Sheets[0].RowCount + 46;
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (45 + 1), 0, 5, 1);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (45 + 1), 6, 5, 1);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (45 + 1), 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (44 + 1), 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (43 + 1), 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (42 + 1), 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (41 + 1), 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (40 + 1), 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (40 + 1), 1, 1, 6);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (39 + 1), 0, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (39 + 1), 2, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (39 + 1), 5, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (38 + 1), 0, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (38 + 1), 2, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (38 + 1), 5, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (37 + 1), 0, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (37 + 1), 2, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (36 + 1), 0, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (36 + 1), 2, 1, 4);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (34 + 1), 4, 1, 3);

                                                if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                                                {
                                                    string college = "select isnull(collname,'') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code=" + Session["collegecode"] + "";
                                                    SqlCommand collegecmd = new SqlCommand(college, con);
                                                    SqlDataReader collegename;
                                                    con.Close();
                                                    con.Open();
                                                    collegename = collegecmd.ExecuteReader();
                                                    if (collegename.HasRows)
                                                    {

                                                        while (collegename.Read())
                                                        {
                                                            affliatedby = collegename["affliated"].ToString();
                                                            catgory = collegename["category"].ToString();
                                                            collnamenew1 = collegename["collname"].ToString();
                                                            address1 = collegename["address1"].ToString();
                                                            address3 = collegename["address3"].ToString();
                                                            pincode = collegename["pincode"].ToString();
                                                            address = address1 + ", " + " " + address3 + "-" + " " + pincode + ".";
                                                            catgory = "(An " + catgory + " Institution";
                                                            affiliated = "Affiliated  to" + " " + affliatedby;

                                                        }
                                                    }

                                                }

                                                exammonth = ddlMonth.SelectedIndex.ToString();
                                                exammonthnew = ddlMonth.SelectedItem.Text;
                                                examyear = ddlYear.SelectedValue.ToString();
                                                exammonthnew1 = monthinwords(exammonthnew);
                                                printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - (45 + 1), 6].CellType = mi1;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (45 + 1), 6].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (45 + 1), 1].Font.Size = FontUnit.Medium;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (45 + 1), 1].Font.Bold = true;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (45 + 1), 1].Text = collnamenew1 + "," + " " + address;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (44 + 1), 1].Text = catgory + " " + affiliated;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (43 + 1), 1].Text = "Office of the Controller of Examinations";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (43 + 1), 1].Font.Bold = true;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (42 + 1), 1].Text = "UG/PG Degree End Semester Examinations " + exammonthnew1 + " " + examyear + "";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (41 + 1), 0].Margin.Left = 50;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (41 + 1), 1].Text = "HALL TICKET";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (41 + 1), 1].Font.Bold = true;
                                                MyImg collogo1 = new MyImg();
                                                collogo1.ImageUrl = "Handler/Handler2.ashx?";
                                                MyImg collogoright2 = new MyImg();
                                                collogoright2.ImageUrl = "~/images/10BIT001.jpeg";
                                                collogoright2.ImageUrl = "Handler/Handler5.ashx?";
                                                printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - (45 + 3), 0].CellType = collogo;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (45 + 3), 0].HorizontalAlign = HorizontalAlign.Center;
                                                rollnosub = FpSpread2.Sheets[0].Cells[res, 1].Note;
                                                MyImgphotp mi2 = new MyImgphotp();
                                                mi2.ImageUrl = "Handler/Handler4.ashx?rollno=" + rollnosub;
                                                exammonthnew = ddlMonth.SelectedItem.Text;
                                                examyear = ddlYear.SelectedValue.ToString();
                                                exammonthnew1 = monthinwords(exammonthnew);
                                                printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - (45 + 3), 6].CellType = mi1;

                                                stuname = FpSpread2.Sheets[0].Cells[res, 3].Text;
                                                regnumber = FpSpread2.Sheets[0].Cells[res, 2].Text;
                                                sem = Session["semforsub"].ToString();
                                                degreecode = Session["selecteddegreecode"].ToString();
                                                batch = FpSpread2.Sheets[0].Cells[res, 0].Note;
                                                degree = Session["selecteddegree"].ToString();
                                                course = Session["selectedcourse"].ToString();
                                                dateofbirth = "select convert(varchar(20),a.dob,103) as dobstudent from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollnosub + "'";
                                                SqlDataAdapter da14 = new SqlDataAdapter(dateofbirth, con2);
                                                dob = "";
                                                DataSet ds14 = new DataSet();
                                                da14.Fill(ds14);
                                                con2.Close();
                                                con2.Open();

                                                if (ds14.Tables[0].Rows.Count > 0)
                                                {
                                                    dob = ds14.Tables[0].Rows[0]["dobstudent"].ToString();
                                                }
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (40 + 1), 0].Border.BorderColorBottom = Color.Black;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (40 + 1), 1].Border.BorderColorBottom = Color.Black;
                                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - (39 + 1)].Border.BorderColor = Color.Black;
                                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - (38 + 1)].Border.BorderColor = Color.Black;
                                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - (37 + 1)].Border.BorderColor = Color.Black;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 0].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 0].Text = "Register Number";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 0].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 2].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 2].Text = " " + regnumber;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 4].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 4].Text = "Semester";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 5].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 5].Text = " " + sem;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 0].Text = "Name";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 2].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 2].Text = " " + stuname;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 4].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 4].Text = "Date of Birth";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 5].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 5].Text = " " + dob;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (37 + 1), 0].Text = "Degree & Branch";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (37 + 1), 0].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (37 + 1), 2].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (37 + 1), 2].Text = degree + "." + " " + "(" + " " + course + ")";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (45 + 1), 1].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (44 + 1), 1].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (43 + 1), 1].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (42 + 1), 1].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (41 + 1), 1].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - (35 + 1)].Border.BorderColorBottom = Color.Black;
                                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - (34 + 1)].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - (34 + 1)].Border.BorderColor = Color.Black;
                                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - (34 + 1)].Font.Bold = true;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (34 + 1), 0].Text = "Sl.No";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (34 + 1), 1].Text = "Date";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (34 + 1), 2].Text = "Session";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (34 + 1), 3].Text = "Sub.Code";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (34 + 1), 4].Text = "Subject Title";
                                                count = printspread.Sheets[0].RowCount - (33 + 1);
                                                i1 = 0;
                                                //===================
                                            }
                                            subject_code1 = ds8.Tables[0].Rows[i]["scode"].ToString();
                                            subject_name1 = ds8.Tables[0].Rows[i]["sname"].ToString();
                                            subject_no2 = ds8.Tables[0].Rows[i]["subno"].ToString();
                                            arrearsem = ds8.Tables[0].Rows[i]["ssem"].ToString();
                                            exam_date = ds8.Tables[0].Rows[i]["exam_date"].ToString();
                                            labiv = ds8.Tables[0].Rows[i]["lab"].ToString();
                                            DateTime obtaineddate = Convert.ToDateTime(exam_date);
                                            string examdatenew = obtaineddate.ToString("dd/MM/yyyy");
                                            exam_session = ds8.Tables[0].Rows[i]["exam_session"].ToString();
                                            printspread.Sheets[0].Cells[count + i1, 0].Text = Convert.ToString(sno);
                                            printspread.Sheets[0].Cells[count + i1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            printspread.Sheets[0].Cells[count + i1, 3].Text = subject_code1;
                                            printspread.Sheets[0].Cells[count + i1, 3].HorizontalAlign = HorizontalAlign.Center;
                                            printspread.Sheets[0].Cells[count + i1, 4].Margin.Left = 50;
                                            printspread.Sheets[0].Cells[count + i1, 4].Text = subject_name1;
                                            printspread.Sheets[0].Cells[count + i1, 4].HorizontalAlign = HorizontalAlign.Left;
                                            printspread.Sheets[0].Rows[count + i1].Border.BorderColor = Color.Black;
                                            printspread.Sheets[0].SpanModel.Add(count + i1, 4, 1, 3);
                                            printspread.Sheets[0].Cells[regularcount + i1, 1].CellType = txt;//Added By Srinath 21/3/2013
                                            printspread.Sheets[0].Cells[regularcount + i1, 1].Text = examdatenew;
                                            printspread.Sheets[0].Cells[regularcount + i1, 1].HorizontalAlign = HorizontalAlign.Center;
                                            printspread.Sheets[0].Cells[regularcount + i1, 2].Text = exam_session;
                                            printspread.Sheets[0].Cells[regularcount + i1, 2].HorizontalAlign = HorizontalAlign.Center;
                                            if (chkboxvdate.Checked == false && labiv == "True")
                                            {
                                                printspread.Sheets[0].AddSpanCell(regularcount + i1, 1, 1, 2);
                                                printspread.Sheets[0].Cells[regularcount + i1, 1].Text = "";
                                                printspread.Sheets[0].Cells[regularcount + i1, 1].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[regularcount + i1, 2].Text = "";
                                                printspread.Sheets[0].Cells[regularcount + i1, 2].HorizontalAlign = HorizontalAlign.Center;

                                            }
                                            regularcount = count; //+ i1+1;
                                            countforregular = regularcount;
                                            i1++;
                                        }
                                        bottomtext = regularcount + i1;
                                    }
                                }
                                //for regular
                                if (CheckRegular.Checked == true)
                                {
                                    string examinforegular = "select distinct isnull(Subject_Code,'') as subcode , isnull(subjecT_name,'') as subname ,sc.subject_no as subject_no, semester as ssem,e.start_time,e.end_time,e.exam_date,e.exam_session,feesub.lab from subjectchooser sc,subject s,sub_sem as feesub,exmtt_det as e where feesub.subtype_no=s.subtype_no and s.subject_no=e.subject_no and  feesub.syll_code=s.syll_code and feesub.promote_count=1 and s.subject_no=sc.subject_no and s.subject_no=" + subject_nofromexmtt + " and s.subtype_no=sc.subtype_no and roll_no='" + rollnosub + "' and semester=" + Session["semforsub"].ToString() + " order by exam_date";
                                    SqlDataAdapter da4 = new SqlDataAdapter(examinforegular, con3);
                                    string subject_code = "";
                                    string subject_name = "";
                                    string subject_no1 = "";
                                    string lab1 = "";
                                    DataSet ds4 = new DataSet();
                                    da4.Fill(ds4);
                                    con3.Close();
                                    con3.Open();

                                    if (ds4.Tables[0].Rows.Count > 0)
                                    {
                                        subarrregflag = 1;
                                        for (int i = 0; i < ds4.Tables[0].Rows.Count; i++)
                                        {
                                            sno++;
                                            subjectcount++;
                                            regularcount = countforregular;
                                            if (subjectcount > rowcount)
                                            {
                                                printspread.Sheets[0].RowCount = printspread.Sheets[0].RowCount + 46;
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (45 + 1), 0, 5, 1);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (45 + 1), 6, 5, 1);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (45 + 1), 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (44 + 1), 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (43 + 1), 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (42 + 1), 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (41 + 1), 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (40 + 1), 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (40 + 1), 1, 1, 6);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (39 + 1), 0, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (39 + 1), 2, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (39 + 1), 5, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (38 + 1), 0, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (38 + 1), 2, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (38 + 1), 5, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (37 + 1), 0, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (37 + 1), 2, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (36 + 1), 0, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (36 + 1), 2, 1, 4);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - (34 + 1), 4, 1, 3);


                                                if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                                                {
                                                    string college = "select isnull(collname,'') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code=" + Session["collegecode"] + "";
                                                    SqlCommand collegecmd = new SqlCommand(college, con);
                                                    SqlDataReader collegename;
                                                    con.Close();
                                                    con.Open();
                                                    collegename = collegecmd.ExecuteReader();
                                                    if (collegename.HasRows)
                                                    {

                                                        while (collegename.Read())
                                                        {
                                                            affliatedby = collegename["affliated"].ToString();
                                                            catgory = collegename["category"].ToString();
                                                            collnamenew1 = collegename["collname"].ToString();
                                                            address1 = collegename["address1"].ToString();
                                                            address3 = collegename["address3"].ToString();
                                                            pincode = collegename["pincode"].ToString();
                                                            address = address1 + ", " + " " + address3 + "-" + " " + pincode + ".";
                                                            catgory = "(An " + catgory + " Institution";
                                                            affiliated = "Affiliated  to" + " " + affliatedby;

                                                        }
                                                    }

                                                }
                                                MyImg collogo2 = new MyImg();
                                                collogo2.ImageUrl = "Handler/Handler2.ashx?";
                                                MyImgphotp collogoright3 = new MyImgphotp();
                                                collogoright3.ImageUrl = "~/images/10BIT001.jpeg";
                                                collogoright3.ImageUrl = "Handler/Handler5.ashx?";
                                                printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 45 + 5, 0].CellType = collogo;
                                                //printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 45, 6].CellType = collogoright;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45 + 5, 0].HorizontalAlign = HorizontalAlign.Center;
                                                rollnosub = FpSpread2.Sheets[0].Cells[res, 1].Note;
                                                MyImg mi2 = new MyImg();
                                                mi2.ImageUrl = "Handler/Handler4.ashx?rollno=" + rollnosub;
                                                exammonthnew = ddlMonth.SelectedItem.Text;
                                                examyear = ddlYear.SelectedValue.ToString();
                                                exammonthnew1 = monthinwords(exammonthnew);
                                                printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - (45 + 1), 6].CellType = mi1;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (45 + 1), 6].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (45 + 1), 1].Font.Size = FontUnit.Medium;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (45 + 1), 1].Font.Bold = true;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (45 + 1), 1].Text = collnamenew1 + "," + " " + address;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (44 + 1), 1].Text = catgory + " " + affiliated;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (43 + 1), 1].Text = "Office of the Controller of Examinations";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (43 + 1), 1].Font.Bold = true;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (42 + 1), 1].Text = "UG/PG Degree End Semester Examinations " + exammonthnew1 + " " + examyear + "";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (41 + 1), 0].Margin.Left = 50;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (41 + 1), 1].Text = "HALL TICKET";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (41 + 1), 1].Font.Bold = true;
                                                stuname = FpSpread2.Sheets[0].Cells[res, 3].Text;
                                                regnumber = FpSpread2.Sheets[0].Cells[res, 2].Text;
                                                sem = Session["semforsub"].ToString();
                                                degreecode = Session["selecteddegreecode"].ToString();
                                                batch = FpSpread2.Sheets[0].Cells[res, 0].Note;
                                                degree = Session["selecteddegree"].ToString();
                                                course = Session["selectedcourse"].ToString();
                                                dateofbirth = "select convert(varchar(20),a.dob,103) as dobstudent from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollnosub + "'";
                                                SqlDataAdapter da14 = new SqlDataAdapter(dateofbirth, con2);
                                                dob = "";
                                                DataSet ds14 = new DataSet();
                                                da14.Fill(ds14);
                                                con2.Close();
                                                con2.Open();

                                                if (ds14.Tables[0].Rows.Count > 0)
                                                {
                                                    dob = ds14.Tables[0].Rows[0]["dobstudent"].ToString();
                                                }
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (40 + 1), 0].Border.BorderColorBottom = Color.Black;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (40 + 1), 1].Border.BorderColorBottom = Color.Black;
                                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - (39 + 1)].Border.BorderColor = Color.Black;
                                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - (38 + 1)].Border.BorderColor = Color.Black;
                                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - (37 + 1)].Border.BorderColor = Color.Black;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 0].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 0].Text = "Register Number";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 0].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 2].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 2].Text = " " + regnumber;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 4].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 4].Text = "Semester";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 5].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (39 + 1), 5].Text = " " + sem;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 0].Text = "Name";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 2].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 2].Text = " " + stuname;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 4].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 4].Text = "Date of Birth";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 5].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (38 + 1), 5].Text = " " + dob;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (37 + 1), 0].Text = "Degree & Branch";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (37 + 1), 0].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (37 + 1), 2].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (37 + 1), 2].Text = degree + "." + " " + "(" + " " + course + ")";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (45 + 1), 1].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (44 + 1), 1].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (43 + 1), 1].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (42 + 1), 1].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (41 + 1), 1].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - (35 + 1)].Border.BorderColorBottom = Color.Black;
                                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - (34 + 1)].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - (34 + 1)].Border.BorderColor = Color.Black;
                                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - (34 + 1)].Font.Bold = true;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (34 + 1), 0].Text = "Sl.No";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (34 + 1), 1].Text = "Date";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (34 + 1), 2].Text = "Session";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (34 + 1), 3].Text = "Sub.Code";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - (34 + 1), 4].Text = "Subject Title";
                                                count = printspread.Sheets[0].RowCount - (33 + 1);
                                                i1 = 0;

                                            }

                                            subject_code = ds4.Tables[0].Rows[i]["subcode"].ToString();
                                            subject_name = ds4.Tables[0].Rows[i]["subname"].ToString();
                                            subject_no1 = ds4.Tables[0].Rows[i]["subject_no"].ToString();
                                            exam_date = ds4.Tables[0].Rows[i]["exam_date"].ToString();
                                            lab1 = ds4.Tables[0].Rows[i]["lab"].ToString();

                                            DateTime obtaineddate = Convert.ToDateTime(exam_date);
                                            string examdatenew = obtaineddate.ToString("dd/MM/yyyy");
                                            exam_session = ds4.Tables[0].Rows[0]["exam_session"].ToString();
                                            printspread.Sheets[0].Cells[regularcount + i1, 0].Text = Convert.ToString(sno);
                                            printspread.Sheets[0].Cells[regularcount + i1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            printspread.Sheets[0].Cells[regularcount + i1, 3].Text = subject_code;
                                            printspread.Sheets[0].Cells[regularcount + i1, 3].HorizontalAlign = HorizontalAlign.Center;
                                            printspread.Sheets[0].Cells[regularcount + i1, 4].Margin.Left = 50;
                                            printspread.Sheets[0].Cells[regularcount + i1, 4].Text = subject_name;
                                            printspread.Sheets[0].Cells[regularcount + i1, 4].HorizontalAlign = HorizontalAlign.Left;
                                            printspread.Sheets[0].Rows[regularcount + i1].Border.BorderColor = Color.Black;
                                            printspread.Sheets[0].SpanModel.Add(regularcount + i1, 4, 1, 3);
                                            printspread.Sheets[0].Cells[regularcount + i1, 1].CellType = txt;//Added By Srinath 21/3/20132
                                            printspread.Sheets[0].Cells[regularcount + i1, 1].Text = examdatenew;
                                            printspread.Sheets[0].Cells[regularcount + i1, 1].HorizontalAlign = HorizontalAlign.Center;
                                            printspread.Sheets[0].Cells[regularcount + i1, 2].Text = exam_session;
                                            printspread.Sheets[0].Cells[regularcount + i1, 2].HorizontalAlign = HorizontalAlign.Center;
                                            if (chkboxvdate.Checked == false && lab1 == "True")
                                            {
                                                printspread.Sheets[0].AddSpanCell(regularcount + i1, 1, 1, 2);
                                                printspread.Sheets[0].Cells[regularcount + i1, 1].Text = "";
                                                printspread.Sheets[0].Cells[regularcount + i1, 1].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[regularcount + i1, 2].Text = "";
                                                printspread.Sheets[0].Cells[regularcount + i1, 2].HorizontalAlign = HorizontalAlign.Center;

                                            }
                                            countforregular = regularcount;
                                            i1++;
                                        }
                                        bottomtext = regularcount + i1;
                                    }
                                }
                                exammonth = ddlMonth.SelectedIndex.ToString();
                                if (exam_session == "F.N")
                                {
                                    //for FN
                                    string fntime = "select distinct convert(Varchar(8),ex.start_time,108) as start,convert(Varchar(8),ex.end_time,108) as end1 from exmtt e,exmtt_det ex  where ex.exam_session='F.N' and ex.start_time<> ex.end_time and e.exam_code=ex.exam_code and e.exam_code=" + exam_code + "";
                                    SqlDataAdapter da7 = new SqlDataAdapter(fntime, con1);

                                    string start_time1 = "";
                                    string end_time1 = "";
                                    DataSet ds7 = new DataSet();
                                    da7.Fill(ds7);
                                    con1.Close();
                                    con1.Open();

                                    if (ds7.Tables[0].Rows.Count > 0)
                                    {
                                        start_time1 = ds7.Tables[0].Rows[0]["start"].ToString();
                                        end_time1 = ds7.Tables[0].Rows[0]["end1"].ToString();
                                        if ((start_time1 != "") && (end_time1 != ""))
                                        {
                                            string[] splitdate = start_time1.Split(new Char[] { ':' });
                                            string starthour2 = splitdate[0].ToString();
                                            string startmin2 = splitdate[1].ToString();
                                            string startsec2 = splitdate[2].ToString();
                                            start_time1 = starthour2 + "." + startmin2;
                                            string[] splitdate1 = end_time1.Split(new Char[] { ':' });
                                            string endhour2 = splitdate1[0].ToString();
                                            string endmin2 = splitdate1[1].ToString();
                                            string endsec2 = splitdate1[2].ToString();

                                            if (Convert.ToInt32(endhour2) > 12)
                                            {
                                                endhour2 = Convert.ToString(railwaytime(Convert.ToInt32(endhour2)));

                                            }
                                            end_time1 = endhour2 + "." + endmin2;
                                            //time = "*FN" + ":" + start_time1 + " " + "a.m" + " "+"-" + " " + end_time1 + " " + "AM";
                                            time = "FN" + " " + "-" + " " + "Forenoon" + " " + start_time1 + " " + "a.m" + " " + "-" + " " + end_time1 + " " + "p.m";

                                        }

                                    }
                                }
                                //for AN
                                if (exam_session == "A.N")
                                {
                                    string antime = "select distinct convert(Varchar(8),ex.start_time,108) as start,convert(Varchar(8),ex.end_time,108) as end1 from exmtt e,exmtt_det ex  where ex.exam_session='A.N' and ex.start_time<> ex.end_time and e.exam_code=ex.exam_code and e.exam_code=" + exam_code + " ";
                                    SqlDataAdapter da6 = new SqlDataAdapter(antime, con1);

                                    string start_time = "";
                                    string end_time = "";
                                    DataSet ds6 = new DataSet();
                                    da6.Fill(ds6);
                                    con1.Close();
                                    con1.Open();

                                    if (ds6.Tables[0].Rows.Count > 0)
                                    {
                                        start_time = ds6.Tables[0].Rows[0]["start"].ToString();
                                        end_time = ds6.Tables[0].Rows[0]["end1"].ToString();
                                        if ((start_time != "") && (end_time != ""))
                                        {
                                            string[] splitdate = start_time.Split(new Char[] { ':' });
                                            string starthour1 = splitdate[0].ToString();
                                            string startmin1 = splitdate[1].ToString();
                                            string startsec1 = splitdate[2].ToString();

                                            string[] splitdate1 = end_time.Split(new Char[] { ':' });
                                            string endhour1 = splitdate1[0].ToString();
                                            string endmin1 = splitdate1[1].ToString();
                                            string endsec1 = splitdate1[2].ToString();

                                            if (Convert.ToInt32(starthour1) > 12)
                                            {
                                                starthour1 = Convert.ToString(railwaytime(Convert.ToInt32(starthour1)));

                                            }
                                            if (Convert.ToInt32(endhour1) > 12)
                                            {
                                                endhour1 = Convert.ToString(railwaytime(Convert.ToInt32(endhour1)));

                                            }
                                            start_time = starthour1 + "." + startmin1;
                                            end_time = endhour1 + "." + endmin1;
                                            //time1 = "*AN" + ":" + start_time + " " + "PM" + "to" + " " + end_time + " " + "PM";
                                            time1 = "AN" + " " + "-" + " " + "Afternoon" + " " + start_time + " " + "p.m" + " " + "-" + " " + end_time + " " + "p.m";

                                        }


                                    }
                                }

                                if (subarrregflag == 1)
                                {
                                    if (i4 == ds15.Tables[0].Rows.Count - 1)
                                    {
                                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 9, 0, 1, 7);
                                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 6, 0, 1, 7);
                                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 1, 0, 1, 7);
                                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 0, 0, 1, 7);
                                        printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 1].Border.BorderColorBottom = Color.Black;
                                        printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 9].Border.BorderColorBottom = Color.Black;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 6, 0].Border.BorderColorBottom = Color.Black;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 0].Text = "Note: If any discrepancies are found in the Hall Ticket, report to the COE office immediately.";
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Small; printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 3, 0, 1, 3);
                                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 2, 4, 1, 3);
                                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 2, 0, 1, 3);
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 0].Margin.Left = 20;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 0].Text = "Signature of the Candidate";
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 4].HorizontalAlign = HorizontalAlign.Right;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 4].Margin.Right = 30;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 4].Text = "Controller of Examinations";
                                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 8, 0, 1, 3);
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 8, 0].Margin.Left = 15;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 8, 0].Text = "No. of Subjects Registered" + " " + sno;
                                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 8, 4, 1, 3);
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 8, 4].Text = time;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 8, 4].HorizontalAlign = HorizontalAlign.Right;
                                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 7, 4, 1, 3);
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 7, 4].Text = time1;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 7, 4].HorizontalAlign = HorizontalAlign.Right;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 0].Border.BorderColor = Color.Black;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 1].Border.BorderColor = Color.Black;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 2].Border.BorderColor = Color.Black;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 3].Border.BorderColor = Color.Black;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 4].Border.BorderColor = Color.Black;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 5].Border.BorderColor = Color.Black;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 6].Border.BorderColor = Color.Black;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 0].Border.BorderColorTop = Color.White;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 1].Border.BorderColorTop = Color.White;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 2].Border.BorderColorTop = Color.White;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 3].Border.BorderColorTop = Color.White;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 4].Border.BorderColorTop = Color.White;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 5].Border.BorderColorTop = Color.White;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 6].Border.BorderColorTop = Color.White;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 0].Border.BorderColorRight = Color.White;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 1].Border.BorderColorRight = Color.White;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 2].Border.BorderColorRight = Color.White;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 3].Border.BorderColorRight = Color.White;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 4].Border.BorderColorRight = Color.White;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 5].Border.BorderColorRight = Color.White;
                                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 2, 6].Border.BorderColorRight = Color.White;
                                    }
                                }
                            }
                        }
                        FpSpread2.Sheets[0].Cells[res, 6].Value = 0;

                    }
                }
                printspread.SaveChanges();
            }
        }
        catch
        {

        }
    }

    protected string monthinwords(string month)
    {
        string month1 = "";
        if (month == "Jan")
        {
            month1 = "January";
        }
        else if (month == "Feb")
        {
            month1 = "February";
        }
        else if (month == "Mar")
        {
            month1 = "March";
        }
        else if (month == "Apr")
        {
            month1 = "April";
        }
        else if (month == "May")
        {
            month1 = "May";
        }
        else if (month == "Jun")
        {
            month1 = "June";
        }
        else if (month == "Jul")
        {
            month1 = "July";
        }
        else if (month == "Aug")
        {
            month1 = "August";
        }
        else if (month == "Sep")
        {
            month1 = "September";
        }
        else if (month == "Oct")
        {
            month1 = "October";
        }
        else if (month == "Nov")
        {
            month1 = "November";
        }
        else if (month == "Dec")
        {
            month1 = "December";
        }
        return month1;

    }

    protected int railwaytime(int endhour)
    {
        int enhournew = 0;
        if (Convert.ToInt32(endhour) == 13)
        {
            enhournew = 1;
        }
        else if (Convert.ToInt32(endhour) == 14)
        {
            enhournew = 2;
        }
        else if (Convert.ToInt32(endhour) == 15)
        {
            enhournew = 3;
        }
        else if (Convert.ToInt32(endhour) == 16)
        {
            enhournew = 4;
        }
        else if (Convert.ToInt32(endhour) == 17)
        {
            enhournew = 5;
        }
        else if (Convert.ToInt32(endhour) == 18)
        {
            enhournew = 6;
        }
        else if (Convert.ToInt32(endhour) == 19)
        {
            enhournew = 7;
        }
        else if (Convert.ToInt32(endhour) == 20)
        {
            enhournew = 8;
        }
        else if (Convert.ToInt32(endhour) == 21)
        {
            enhournew = 9;
        }
        else if (Convert.ToInt32(endhour) == 22)
        {
            enhournew = 10;
        }
        else if (Convert.ToInt32(endhour) == 23)
        {
            enhournew = 11;
        }
        else if (Convert.ToInt32(endhour) == 24)
        {
            enhournew = 12;

        }
        return enhournew;
    }

    protected void printbtn_Click(object sender, EventArgs e)
    {
        if (RadioButton1.Checked == true)
        {
            FpSpread2.SaveChanges();
            errmsg.Visible = true;
            ModalPopupExtender1.Show();
            hallticket();

        }
        if (RadioButton2.Checked == true)
        {
            FpSpread2.SaveChanges();
            errmsg.Visible = true;
            //hallticketNECFormat();
            //ModalPopupExtender1.Show();
            btnhallpdf();
        }
        if (RadioButton3.Checked == true)
        {
            FpSpread2.SaveChanges();
            errmsg.Visible = true;
            loadhallticketformat3();
        }
        if (RadioButton4.Checked == true)
        {
            FpSpread2.SaveChanges();
            errmsg.Visible = true;
            formatfour();
        }
        if (RadioButton5.Checked == true)
        {
            FpSpread2.SaveChanges();
            errmsg.Visible = true;
            loadhallticketformat5();
        }
        if (rbFormat6.Checked == true)
        {
            FpSpread2.SaveChanges();
            errmsg.Visible = true;
            loadhallticketformat6();
        }

    }

    protected void hallticket()
    {
        try
        {
            if ((ddlMonth.SelectedValue.ToString() != "0") && (ddlYear.SelectedValue.ToString() != "0"))
            {
                int selectedcount = 0;
                for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
                {
                    int isval = 0;
                    string s = FpSpread2.Sheets[0].Cells[res, 6].Text;

                    isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);
                    if (isval == 1)
                    {
                        selectedcount++;
                        errmsg.Text = "";
                    }
                }
                if (selectedcount == 0)
                {
                    errmsg.Text = "Please Select the Student and then Proceed";
                    ModalPopupExtender1.Hide();
                    return;
                }
                //Button2.Visible = false;
                FpSpread2.SaveChanges();
                printspread.SaveChanges();
                printspread.Visible = true;
                printspread.Sheets[0].RowCount = 0;
                //printspread.Sheets[0].AutoPostBack = true;
                printspread.Sheets[0].PageSize = 45;
                printspread.Sheets[0].ColumnCount = 7;
                printspread.Sheets[0].RowHeader.Visible = false;
                printspread.Sheets[0].ColumnHeader.Visible = false;
                printspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Small;
                printspread.Sheets[0].Columns[0].Width = 100;
                printspread.Sheets[0].Columns[1].Width = 100;
                printspread.Sheets[0].Columns[2].Width = 120;
                printspread.Sheets[0].Columns[3].Width = 80;
                printspread.Sheets[0].Columns[4].Width = 90;
                printspread.Sheets[0].Columns[5].Width = 100;
                printspread.Sheets[0].Columns[6].Width = 100;
                printspread.Sheets[0].Columns[0].Locked = true;
                printspread.Sheets[0].Columns[1].Locked = true;
                printspread.Sheets[0].Columns[2].Locked = true;
                printspread.Sheets[0].Columns[3].Locked = true;
                printspread.Sheets[0].Columns[4].Locked = true;
                printspread.Sheets[0].Columns[5].Locked = true;
                printspread.Sheets[0].Columns[6].Locked = true;
                printspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                printspread.Sheets[0].DefaultStyle.Font.Bold = true;
                string collnamenew1 = "";
                string address1 = "";
                string address3 = "";
                string address = "";
                string affliatedby = "";
                string catgory = "";
                string pincode = "";
                string affiliated = "";
                int bottomtext = 0;

                for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
                {

                    int isval = 0;
                    string s = FpSpread2.Sheets[0].Cells[res, 6].Text;

                    isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);
                    if (isval == 1)
                    {

                        //for photo span
                        printspread.Sheets[0].RowCount = printspread.Sheets[0].RowCount + 45;
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 45, 0, 5, 1);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 45, 6, 5, 1);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 39, 6, 4, 1);
                        //

                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 45, 1, 1, 5);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 44, 1, 1, 5);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 43, 1, 1, 5);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 42, 1, 1, 5);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 41, 1, 1, 5);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 40, 1, 1, 5);
                        //for reg spanning
                        //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 40, 0, 1, 2);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 40, 2, 1, 4);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 39, 0, 1, 2);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 39, 2, 1, 4);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 38, 0, 1, 2);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 38, 2, 1, 4);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 37, 0, 1, 2);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 37, 2, 1, 4);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 36, 0, 1, 2);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 36, 2, 1, 4);
                        //bottom page
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 7, 0, 1, 3);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 3, 0, 1, 3);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 3, 4, 1, 3);
                        //for subjectname span
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 34, 3, 1, 3);
                        //for session span
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 11, 4, 1, 3);
                        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 10, 4, 1, 3);
                        if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                        {
                            string college = "select isnull(collname,'') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code=" + Session["collegecode"] + "";
                            SqlCommand collegecmd = new SqlCommand(college, con);
                            SqlDataReader collegename;
                            con.Close();
                            con.Open();
                            collegename = collegecmd.ExecuteReader();
                            if (collegename.HasRows)
                            {

                                while (collegename.Read())
                                {
                                    affliatedby = collegename["affliated"].ToString();
                                    catgory = collegename["category"].ToString();
                                    collnamenew1 = collegename["collname"].ToString();
                                    address1 = collegename["address1"].ToString();
                                    address3 = collegename["address3"].ToString();
                                    pincode = collegename["pincode"].ToString();
                                    address = address1 + ", " + address3 + "-" + " " + pincode;
                                    catgory = "(An " + catgory + " Institution)";
                                    affiliated = "Affliated to" + " " + affliatedby;
                                }
                            }

                        }
                        MyImg collogo = new MyImg();
                        collogo.ImageUrl = "Handler/Handler2.ashx?";
                        MyImg collogoright = new MyImg();
                        collogoright.ImageUrl = "~/images/10BIT001.jpeg";
                        collogoright.ImageUrl = "Handler/Handler5.ashx?";
                        printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 45, 0].CellType = collogo;
                        printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 45, 6].CellType = collogoright;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 0].HorizontalAlign = HorizontalAlign.Center;
                        string rollnosub = FpSpread2.Sheets[0].Cells[res, 1].Text;
                        MyImg mi1 = new MyImg();
                        mi1.ImageUrl = "Handler/Handler4.ashx?rollno=" + rollnosub;
                        string exammonthnew = ddlMonth.SelectedItem.Text;
                        string exammonth = ddlMonth.SelectedIndex.ToString();
                        string examyear = ddlYear.SelectedValue.ToString();
                        monthinwords(exammonth);
                        printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 39, 6].CellType = mi1;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 6].HorizontalAlign = HorizontalAlign.Center;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 1].Font.Size = FontUnit.Medium;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 1].Font.Bold = true;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 1].Text = collnamenew1;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 44, 1].Text = address;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 43, 1].Text = catgory;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 42, 1].Text = affiliated;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 41, 1].Text = "UG/PG Examinations, " + exammonthnew + "-" + examyear + "";
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 0].Margin.Left = 50;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 1].Text = "HALL TICKET";
                        //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 7, 0].Text = "Signature of the Candidate";
                        //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 7, 0].Margin.Left = 15;
                        //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 3, 4].Text = "Controller of Examinations";
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 1].HorizontalAlign = HorizontalAlign.Center;
                        //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 3, 4].HorizontalAlign = HorizontalAlign.Right;
                        //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 3, 0].Margin.Left = 15;
                        //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 3, 0].Text = "Head of the Department";

                        string stuname = FpSpread2.Sheets[0].Cells[res, 3].Text;
                        string regnumber = FpSpread2.Sheets[0].Cells[res, 2].Text;
                        string sem = Session["semforsub"].ToString();
                        string degreecode = Session["selecteddegreecode"].ToString();
                        string batch = FpSpread2.Sheets[0].Cells[res, 0].Note;
                        string degree = Session["selecteddegree"].ToString();
                        string course = Session["selectedcourse"].ToString();
                        string dateofbirth = "select convert(varchar(20),a.dob,103) as dobstudent from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollnosub + "'";
                        SqlDataAdapter da3 = new SqlDataAdapter(dateofbirth, con2);
                        string dob = "";
                        DataSet ds3 = new DataSet();
                        da3.Fill(ds3);
                        con2.Close();
                        con2.Open();

                        if (ds3.Tables[0].Rows.Count > 0)
                        {
                            dob = ds3.Tables[0].Rows[0]["dobstudent"].ToString();
                        }
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 0].Margin.Left = 15;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 0].Text = "Name of the candidate" + ":";
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 0].Margin.Left = 15;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 2].Text = stuname;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 0].Text = "Date of Birth" + ":";
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 2].Text = dob;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 0].Text = "Registration Number" + ":";
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 0].Margin.Left = 15;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 2].Text = regnumber;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 0].Text = "Programme" + ":";
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 0].Margin.Left = 15;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 2].Text = degree + "-" + course;
                        //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 0].Text = "Branch" + ":";
                        //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 0].Margin.Left = 15;
                        //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 2].Text = course;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 1].HorizontalAlign = HorizontalAlign.Center;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 44, 1].HorizontalAlign = HorizontalAlign.Center;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 43, 1].HorizontalAlign = HorizontalAlign.Center;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 42, 1].HorizontalAlign = HorizontalAlign.Center;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 41, 1].HorizontalAlign = HorizontalAlign.Center;
                        printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 35].Border.BorderColorBottom = Color.Black;
                        printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 34].HorizontalAlign = HorizontalAlign.Center;
                        printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 34].Border.BorderColor = Color.Black;
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 0].Text = "Date";
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 1].Text = "Semester";
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 2].Text = "Subject Code";
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 3].Text = "Subject Title";
                        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 6].Text = "Session";
                        //string excode = GetUnivExamCode(degreecode, Convert.ToInt32(sem), Convert.ToInt32(batch)).ToString();
                        //if (excode != " ")
                        //{
                        string subject_nofromexmtt = "";
                        string time = "";
                        string time1 = "";
                        string subjectquery = "select distinct isnull(s.Subject_Code,'') as scode , isnull(s.subjecT_name,'') as sname , isnull(ed.subject_no,'') as subno, semester as ssem,ed.start_time,ed.end_time,ed.exam_date,ed.exam_session from exmtt_det ed,exmtt e,subject s,sub_sem feesub  where feesub.subtype_no=s.subtype_no and s.subject_no=ed.subject_no and e.exam_code=ed.exam_code and e.degree_code=" + degreecode + " and e.exam_month=" + exammonth + " and e.exam_year=" + examyear + " order by exam_date asc,exam_session desc";
                        if (chksupplym.Checked == true)
                        {
                            subjectquery = "select distinct isnull(s.Subject_Code,'') as scode , isnull(s.subjecT_name,'') as sname , isnull(ed.subject_no,'') as subno, semester as ssem,ed.start_time,ed.end_time,ed.exam_date,ed.exam_session,feesub.lab from exmtt_det ed,exmtt e,subject s,sub_sem feesub  where feesub.subtype_no=s.subtype_no and s.subject_no=ed.subject_no and e.exam_code=ed.exam_code and e.degree_code=" + degreecode + " and e.exam_month=" + exammonth + " and e.exam_year=" + examyear + "  and s.subject_no  in ( select ea.subject_no   from Exam_Details ed,exam_appl_details ea,exam_application e, subject s, syllabus_master sy,sub_sem su where ed.exam_code =e.exam_code  and e.appl_no =ea.appl_no                            and  s.subject_no =ea.subject_no   and  su.syll_code =sy.syll_code and su.subType_no =s.subType_no  and  sy.syll_code =s.syll_code and e.roll_no ='" + rollnosub + "' and e.Exam_type=4 and ed.Exam_Month='" + exammonth + "'  and ed.Exam_year='" + examyear + "') order by exam_date asc, exam_session desc";
                        }


                        SqlDataAdapter da15 = new SqlDataAdapter(subjectquery, con1);
                        DataSet ds15 = new DataSet();
                        da15.Fill(ds15);
                        con1.Close();
                        int i1 = 0;
                        int subjectcount = 0;
                        int rowcount = 20;
                        int count = printspread.Sheets[0].RowCount - 33;
                        int regularcount = printspread.Sheets[0].RowCount - 33;
                        int countforregular = regularcount;
                        con1.Open();
                        if (ds15.Tables[0].Rows.Count > 0)
                        {
                            int flagforarrreg = 0;
                            for (int i4 = 0; i4 < ds15.Tables[0].Rows.Count; i4++)
                            {
                                string subject_code1 = "";
                                string subject_name1 = "";
                                string subject_no2 = "";
                                string exam_code1 = "";
                                string arrearsem = "";
                                string exam_date = "";
                                string exam_session = "";
                                string mnt = ddlMonth.SelectedItem.Value;
                                batch = FpSpread2.Sheets[0].Cells[res, 0].Note;
                                subject_nofromexmtt = ds15.Tables[0].Rows[i4]["subno"].ToString();
                                //added by sridharan 16.06.2016

                                if (chksupplym.Checked == true)
                                {
                                    string examinforegular = "select distinct feesub.lab, isnull(Subject_Code,'') as subcode , isnull(subjecT_name,'') as subname ,sc.subject_no as subject_no, semester as ssem,e.start_time,e.end_time,e.exam_date,e.exam_session from subjectchooser sc,subject s,sub_sem as feesub,exmtt_det as e where feesub.subtype_no=s.subtype_no and s.subject_no=e.subject_no and  feesub.syll_code=s.syll_code and feesub.promote_count=1 and s.subject_no=sc.subject_no and s.subject_no=" + subject_nofromexmtt + " and s.subtype_no=sc.subtype_no and roll_no='" + rollnosub + "' and semester=" + Session["semforsub"].ToString() + " order by exam_date";

                                    SqlDataAdapter da4 = new SqlDataAdapter(examinforegular, con3);
                                    string subject_code = "";
                                    string subject_name = "";
                                    string subject_no1 = "";
                                    string exam_code = "";
                                    //int subjectcount1 = 0;
                                    //int rowcount1 = 20;
                                    DataSet ds4 = new DataSet();
                                    da4.Fill(ds4);
                                    con3.Close();
                                    con3.Open();

                                    if (ds4.Tables[0].Rows.Count > 0)
                                    {
                                        flagforarrreg = 1;
                                        //i1++;
                                        //Button2.Visible = true;
                                        //int i1 = 0;
                                        for (int i = 0; i < ds4.Tables[0].Rows.Count; i++)
                                        {

                                            subjectcount++;
                                            regularcount = countforregular;
                                            if (subjectcount > rowcount)
                                            {
                                                rowcount = rowcount + 20;
                                                printspread.Sheets[0].RowCount = printspread.Sheets[0].RowCount + 45;
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 45, 0, 5, 1);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 45, 6, 5, 1);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 39, 6, 5, 1);
                                                //

                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 45, 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 44, 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 43, 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 42, 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 41, 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 40, 1, 1, 5);
                                                //for reg spanning
                                                //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 40, 0, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 40, 2, 1, 4);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 39, 0, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 39, 2, 1, 4);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 38, 0, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 38, 2, 1, 4);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 37, 0, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 37, 2, 1, 4);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 36, 0, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 36, 2, 1, 4);
                                                //bottom page
                                                //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 7, 0, 1, 3);
                                                //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 3, 0, 1, 3);
                                                //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 3, 4, 1, 3);
                                                //for subjectname span
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 34, 3, 1, 3);
                                                //for session span
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 11, 4, 1, 3);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 10, 4, 1, 3);
                                                if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                                                {
                                                    string college = "select isnull(collname,'') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code=" + Session["collegecode"] + "";
                                                    SqlCommand collegecmd = new SqlCommand(college, con);
                                                    SqlDataReader collegename;
                                                    con.Close();
                                                    con.Open();
                                                    collegename = collegecmd.ExecuteReader();
                                                    if (collegename.HasRows)
                                                    {

                                                        while (collegename.Read())
                                                        {
                                                            affliatedby = collegename["affliated"].ToString();
                                                            catgory = collegename["category"].ToString();
                                                            collnamenew1 = collegename["collname"].ToString();
                                                            address1 = collegename["address1"].ToString();
                                                            address3 = collegename["address3"].ToString();
                                                            pincode = collegename["pincode"].ToString();
                                                            address = address1 + "," + address3 + "-" + " " + pincode;
                                                            catgory = "(An " + catgory + " Institution)";
                                                            affiliated = "Affliated to" + " " + affliatedby;

                                                        }
                                                    }

                                                }
                                                MyImg collogo2 = new MyImg();
                                                collogo2.ImageUrl = "Handler/Handler2.ashx?";
                                                MyImg collogoright2 = new MyImg();
                                                collogoright2.ImageUrl = "~/images/10BIT001.jpeg";
                                                collogoright2.ImageUrl = "Handler/Handler5.ashx?";
                                                printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 45, 6].CellType = collogoright2;
                                                printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 45, 0].CellType = collogo2;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 0].HorizontalAlign = HorizontalAlign.Center;
                                                rollnosub = FpSpread2.Sheets[0].Cells[res, 1].Text;
                                                MyImg mi3 = new MyImg();
                                                mi3.ImageUrl = "Handler/Handler4.ashx?rollno=" + rollnosub;
                                                exammonthnew = ddlMonth.SelectedItem.Text;
                                                examyear = ddlYear.SelectedValue.ToString();
                                                printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 39, 6].CellType = mi3;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 6].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 0].Font.Size = FontUnit.Medium;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 0].Font.Bold = true;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 1].Text = collnamenew1;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 44, 1].Text = address;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 43, 1].Text = catgory;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 42, 1].Text = affiliated;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 41, 1].Text = "UG/PG Examinations," + exammonthnew + "-" + examyear + "";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 0].Margin.Left = 50;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 1].Text = "HALL TICKET";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 1].HorizontalAlign = HorizontalAlign.Center;
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 7, 0].Text = "Signature of the Candidate";
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 7, 0].Margin.Left = 15;
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 3, 4].Text = "Controller of Examinations";
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 3, 4].HorizontalAlign = HorizontalAlign.Right;
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 3, 0].Margin.Left = 15;
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 3, 0].Text = "Head of the Department";

                                                stuname = FpSpread2.Sheets[0].Cells[res, 3].Text;
                                                regnumber = FpSpread2.Sheets[0].Cells[res, 2].Text;
                                                sem = Session["semforsub"].ToString();
                                                degreecode = Session["selecteddegreecode"].ToString();
                                                batch = FpSpread2.Sheets[0].Cells[res, 0].Note;
                                                degree = Session["selecteddegree"].ToString();
                                                course = Session["selectedcourse"].ToString();
                                                dateofbirth = "select convert(varchar(20),a.dob,103) as dobstudent from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollnosub + "'";
                                                SqlDataAdapter da14 = new SqlDataAdapter(dateofbirth, con2);
                                                dob = "";
                                                DataSet ds14 = new DataSet();
                                                da14.Fill(ds14);
                                                con2.Close();
                                                con2.Open();

                                                if (ds14.Tables[0].Rows.Count > 0)
                                                {
                                                    dob = ds14.Tables[0].Rows[0]["dobstudent"].ToString();
                                                }
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 0].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 0].Text = "Name of the candidate" + ":";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 0].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 2].Text = stuname;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 0].Text = "Date of Birth" + ":";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 2].Text = dob;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 0].Text = "Registration Number" + ":";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 0].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 2].Text = regnumber;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 0].Text = "Programme" + ":";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 0].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 2].Text = degree + "-" + course;
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 0].Text = "Branch" + ":";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 0].Margin.Left = 15;
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 2].Text = course;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 0].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 44, 0].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 43, 0].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 42, 0].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 41, 0].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 35].Border.BorderColorBottom = Color.Black;
                                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 34].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 34].Border.BorderColor = Color.Black;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 0].Text = "Date";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 1].Text = "Semester";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 2].Text = "Subject Code";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 3].Text = "Subject Title";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 6].Text = "Session";

                                                regularcount = countforregular + 44;
                                                i1 = 0;

                                            }

                                            subject_code = ds4.Tables[0].Rows[i]["subcode"].ToString();
                                            subject_name = ds4.Tables[0].Rows[i]["subname"].ToString();
                                            subject_no1 = ds4.Tables[0].Rows[i]["subject_no"].ToString();
                                            exam_date = ds4.Tables[0].Rows[i]["exam_date"].ToString();

                                            DateTime obtaineddate = Convert.ToDateTime(exam_date);
                                            string examdatenew = obtaineddate.ToString("dd/MM/yyyy");
                                            exam_session = ds4.Tables[0].Rows[0]["exam_session"].ToString();

                                            Boolean subjecttype = Convert.ToBoolean(ds4.Tables[0].Rows[0]["lab"].ToString());
                                            if (chkboxvdate.Checked == false && CheckBox1.Checked == false)
                                            {


                                                exam_session = "";
                                                examdatenew = "";


                                            }
                                            else if (chkboxvdate.Checked == true && CheckBox1.Checked == true)
                                            {

                                            }
                                            else if (chkboxvdate.Checked == true && CheckBox1.Checked == false)
                                            {
                                                if (subjecttype == false)
                                                {
                                                    exam_session = "";
                                                    examdatenew = "";
                                                }
                                            }
                                            else if (chkboxvdate.Checked == false && CheckBox1.Checked == true)
                                            {
                                                if (subjecttype == true)
                                                {
                                                    exam_session = "";
                                                    examdatenew = "";
                                                }
                                            }
                                            //exam_code = ds4.Tables[0].Rows[0]["exmcode"].ToString();
                                            printspread.Sheets[0].Cells[regularcount + i1, 1].Text = sem;
                                            printspread.Sheets[0].Cells[regularcount + i1, 1].HorizontalAlign = HorizontalAlign.Center;
                                            printspread.Sheets[0].Cells[regularcount + i1, 2].Text = subject_code;
                                            printspread.Sheets[0].Cells[regularcount + i1, 2].HorizontalAlign = HorizontalAlign.Center;
                                            printspread.Sheets[0].Cells[regularcount + i1, 3].Text = subject_name;
                                            printspread.Sheets[0].Cells[regularcount + i1, 3].HorizontalAlign = HorizontalAlign.Left;
                                            printspread.Sheets[0].Rows[regularcount + i1].Border.BorderColor = Color.Black;
                                            printspread.Sheets[0].SpanModel.Add(regularcount + i1, 3, 1, 3);
                                            printspread.Sheets[0].Cells[regularcount + i1, 1].CellType = txt;//Added By Srinath 21/3/20132
                                            printspread.Sheets[0].Cells[regularcount + i1, 0].Text = examdatenew;
                                            printspread.Sheets[0].Cells[regularcount + i1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            printspread.Sheets[0].Cells[regularcount + i1, 6].Text = exam_session;
                                            printspread.Sheets[0].Cells[regularcount + i1, 6].HorizontalAlign = HorizontalAlign.Center;

                                            //string examyear = ddlYear.SelectedValue.ToString();


                                            i1++;

                                        }

                                        bottomtext = regularcount + i1;

                                    }

                                    goto supplyformat_i;
                                }

                                //end 16.06.2014
                                if (CheckArrear.Checked == true)
                                {
                                    string Arrearsub = "select ed.Exam_Month,ed.Exam_year,r.Reg_No,r.Roll_No,r.Stud_Name,sc.semester,s.subject_code,s.subject_name ,s.subject_no,ead.attempts  from Exam_Details ed,exam_application ea,exam_appl_details ead ,Registration r,subject s,subjectChooser sc  where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ead.subject_no=s.subject_no and r.Roll_No=sc.roll_no and sc.roll_no=ea.roll_no and sc.subject_no=ead.subject_no and s.subject_no=sc.subject_no and ed.Exam_year='2015' and ed.Exam_Month='" + mnt + "' and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'and r.Roll_No='" + rollnosub + "'  and ead.attempts > 0  order by r.Reg_No,sc.semester desc, s.subject_code ";
                                    //string Arrearsub = "Select isnull(Subject_Code,'') as scode , isnull(subjecT_name,'') as sname , isnull(subject_no,'') as subno, semester as ssem from subject,syllabus_master as smas,sub_sem as feesub where feesub.subtype_no=subject.subtype_no and  feesub.syll_code=subject.syll_code and feesub.promote_count=1 and smas.syll_code = subject.syll_code and subject_no in (select distinct subject_no from  mark_entry where subject_no in (select distinct subject_no from mark_entry where passorfail=0 and result='Fail' and ltrim(rtrim(roll_no))='" + rollnosub + "') and ltrim(rtrim(roll_no))='" + rollnosub + "' and Semester >= 1 and Semester < " + Session["semforsub"].ToString() + " ) order by smas.semester , scode";
                                    //string Arrearsub = "Select isnull(Subject_Code,'') as subcode , isnull(subjecT_name,'') as subname  ,sc.subject_no as subject_no, semester as ssem,feesub.fee_per_paper,feesub.arr_fee as arrearfees from subject as s,syllabus_master as smas,sub_sem as feesub where s.subject_no=sc.subject_no and s.subtype_no=sc.subtype_no and feesub.subtype_no=s.subtype_no and smas.syll_code = s.syll_code and subject_no in (select distinct subject_no from  mark_entry where subject_no in (select distinct subject_no from mark_entry where passorfail=0 and result='Fail' and ltrim(rtrim(roll_no))='" + rollnosub + "') and ltrim(rtrim(roll_no))='" + rollnosub + "' and Semester >= 1 and Semester < " + Session["semforsub"].ToString() + " ) order by smas.semester , scode";
                                    SqlDataAdapter da8 = new SqlDataAdapter(Arrearsub, con3);
                                    DataSet ds8 = new DataSet();
                                    da8.Fill(ds8);
                                    con3.Close();
                                    con3.Open();


                                    if (ds8.Tables[0].Rows.Count > 0)
                                    {
                                        flagforarrreg = 1;
                                        count = regularcount;
                                        //Button2.Visible = true;
                                        //int count = printspread.Sheets[0].RowCount - 33;
                                        //int i1 = 0;
                                        for (int i = 0; i < ds8.Tables[0].Rows.Count; i++)
                                        {

                                            subjectcount++;
                                            if (subjectcount > rowcount)
                                            {

                                                //===================
                                                rowcount = rowcount + 20;
                                                printspread.Sheets[0].RowCount = printspread.Sheets[0].RowCount + 45;
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 45, 0, 5, 1);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 45, 6, 5, 1);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 39, 6, 5, 1);
                                                //

                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 45, 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 44, 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 43, 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 42, 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 41, 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 40, 1, 1, 5);
                                                //for reg spanning
                                                //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 40, 0, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 40, 2, 1, 4);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 39, 0, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 39, 2, 1, 4);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 38, 0, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 38, 2, 1, 4);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 37, 0, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 37, 2, 1, 4);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 36, 0, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 36, 2, 1, 4);
                                                //bottom page
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 7, 0, 1, 3);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 3, 0, 1, 3);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 3, 4, 1, 3);
                                                //for subjectname span
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 34, 3, 1, 3);
                                                //for session span
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 11, 4, 1, 3);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 10, 4, 1, 3);
                                                if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                                                {
                                                    string college = "select isnull(collname,'') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code=" + Session["collegecode"] + "";
                                                    SqlCommand collegecmd = new SqlCommand(college, con);
                                                    SqlDataReader collegename;
                                                    con.Close();
                                                    con.Open();
                                                    collegename = collegecmd.ExecuteReader();
                                                    if (collegename.HasRows)
                                                    {

                                                        while (collegename.Read())
                                                        {
                                                            affliatedby = collegename["affliated"].ToString();
                                                            catgory = collegename["category"].ToString();
                                                            collnamenew1 = collegename["collname"].ToString();
                                                            address1 = collegename["address1"].ToString();
                                                            address3 = collegename["address3"].ToString();
                                                            pincode = collegename["pincode"].ToString();
                                                            address = address1 + ", " + address3 + "-" + " " + pincode;
                                                            catgory = "(An " + catgory + " Institution)";
                                                            affiliated = "Affliated to" + " " + affliatedby;

                                                        }
                                                    }

                                                }
                                                MyImg collogo1 = new MyImg();
                                                collogo1.ImageUrl = "Handler/Handler2.ashx?";
                                                MyImg collogoright1 = new MyImg();
                                                collogoright1.ImageUrl = "~/images/10BIT001.jpeg";
                                                collogoright1.ImageUrl = "Handler/Handler5.ashx?";
                                                printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 45, 6].CellType = collogoright1;
                                                printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 45, 0].CellType = collogo1;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 0].HorizontalAlign = HorizontalAlign.Center;
                                                rollnosub = FpSpread2.Sheets[0].Cells[res, 1].Text;
                                                MyImg mi2 = new MyImg();
                                                mi2.ImageUrl = "Handler/Handler4.ashx?rollno=" + rollnosub;
                                                exammonthnew = ddlMonth.SelectedItem.Text;
                                                examyear = ddlYear.SelectedValue.ToString();
                                                printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 39, 6].CellType = mi2;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 6].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 0].Font.Size = FontUnit.Medium;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 0].Font.Bold = true;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 1].Text = collnamenew1;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 44, 1].Text = address;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 43, 1].Text = catgory;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 42, 1].Text = affiliated;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 41, 1].Text = "UG/PG Examinations, " + exammonthnew + "-" + examyear + "";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 0].Margin.Left = 50;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 1].Text = "HALL TICKET";
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 7, 0].Text = "Signature of the Candidate";
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 7, 0].Margin.Left = 15;
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 3, 4].Text = "Controller of Examinations";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 1].HorizontalAlign = HorizontalAlign.Center;
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 3, 4].HorizontalAlign = HorizontalAlign.Right;
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 3, 0].Margin.Left = 15;
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 3, 0].Text = "Head of the Department";

                                                stuname = FpSpread2.Sheets[0].Cells[res, 3].Text;
                                                regnumber = FpSpread2.Sheets[0].Cells[res, 2].Text;
                                                sem = Session["semforsub"].ToString();
                                                degreecode = Session["selecteddegreecode"].ToString();
                                                batch = FpSpread2.Sheets[0].Cells[res, 0].Note;
                                                degree = Session["selecteddegree"].ToString();
                                                course = Session["selectedcourse"].ToString();
                                                dateofbirth = "select convert(varchar(20),a.dob,103) as dobstudent from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollnosub + "'";
                                                SqlDataAdapter da14 = new SqlDataAdapter(dateofbirth, con2);
                                                dob = "";
                                                DataSet ds14 = new DataSet();
                                                da14.Fill(ds14);
                                                con2.Close();
                                                con2.Open();

                                                if (ds14.Tables[0].Rows.Count > 0)
                                                {
                                                    dob = ds14.Tables[0].Rows[0]["dobstudent"].ToString();
                                                }
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 0].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 0].Text = "Name of the candidate" + ":";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 0].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 2].Text = stuname;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 0].Text = "Date of Birth" + ":";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 2].Text = dob;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 0].Text = "Registration Number" + ":";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 0].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 2].Text = regnumber;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 0].Text = "Programme" + ":";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 0].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 2].Text = degree + "-" + course;
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 0].Text = "Branch" + ":";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 0].Margin.Left = 15;
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 2].Text = course;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 0].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 44, 0].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 43, 0].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 42, 0].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 41, 0].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 35].Border.BorderColorBottom = Color.Black;
                                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 34].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 34].Border.BorderColor = Color.Black;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 0].Text = "Date";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 1].Text = "Semester";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 2].Text = "Subject Code";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 3].Text = "Subject Title";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 6].Text = "Session";
                                                count = printspread.Sheets[0].RowCount - 33;
                                                i1 = 0;
                                                //string excode = GetUnivExamCode(degreecode, Convert.ToInt32(sem), Convert.ToInt32(batch)).ToString();
                                                //if (excode != " ")
                                                //{


                                                //===================
                                            }


                                            subject_code1 = ds8.Tables[0].Rows[i]["scode"].ToString();
                                            subject_name1 = ds8.Tables[0].Rows[i]["sname"].ToString();
                                            subject_no2 = ds8.Tables[0].Rows[i]["subno"].ToString();
                                            arrearsem = ds8.Tables[0].Rows[i]["ssem"].ToString();
                                            exam_date = ds8.Tables[0].Rows[i]["exam_date"].ToString();

                                            DateTime obtaineddate = Convert.ToDateTime(exam_date);
                                            string examdatenew = obtaineddate.ToString("dd/MM/yyyy");
                                            exam_session = ds8.Tables[0].Rows[i]["exam_session"].ToString();
                                            //exam_code = ds4.Tables[0].Rows[0]["exmcode"].ToString();
                                            printspread.Sheets[0].Cells[count + i1, 1].Text = arrearsem;
                                            printspread.Sheets[0].Cells[count + i1, 1].HorizontalAlign = HorizontalAlign.Center;
                                            printspread.Sheets[0].Cells[count + i1, 2].Text = subject_code1;
                                            printspread.Sheets[0].Cells[count + i1, 2].HorizontalAlign = HorizontalAlign.Center;
                                            printspread.Sheets[0].Cells[count + i1, 3].Text = subject_name1;
                                            printspread.Sheets[0].Cells[count + i1, 3].HorizontalAlign = HorizontalAlign.Left;
                                            printspread.Sheets[0].Rows[count + i1].Border.BorderColor = Color.Black;
                                            printspread.Sheets[0].SpanModel.Add(count + i1, 3, 1, 3);

                                            printspread.Sheets[0].Cells[regularcount + i1, 1].CellType = txt;//Added By Srinath 21/3/20132
                                            printspread.Sheets[0].Cells[count + i1, 0].Text = examdatenew;
                                            printspread.Sheets[0].Cells[count + i1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            printspread.Sheets[0].Cells[count + i1, 6].Text = exam_session;
                                            printspread.Sheets[0].Cells[count + i1, 6].HorizontalAlign = HorizontalAlign.Center;

                                            regularcount = count; //+ i1+1;
                                            countforregular = regularcount;
                                            i1++;
                                        }
                                        bottomtext = regularcount + i1;
                                    }
                                }
                                //for regular
                                //string examinforegular = "select isnull(Subject_Code,'') as subcode , isnull(subjecT_name,'') as subname ,sc.subject_no as subject_no, semester as ssem from subjectchooser sc,subject s,sub_sem as feesub where feesub.subtype_no=s.subtype_no and  feesub.syll_code=s.syll_code and feesub.promote_count=1 and s.subject_no=sc.subject_no and s.subtype_no=sc.subtype_no and s.subject_no="+subject_nofromexmtt +" and ltrim(rtrim(roll_no))='" + rollnosub + "' and semester=" + Session["semforsub"].ToString() + "";
                                if (CheckRegular.Checked == true)
                                {
                                    string examinforegular = "select distinct isnull(Subject_Code,'') as subcode , isnull(subjecT_name,'') as subname ,sc.subject_no as subject_no, semester as ssem,e.start_time,e.end_time,e.exam_date,e.exam_session from subjectchooser sc,subject s,sub_sem as feesub,exmtt_det as e where feesub.subtype_no=s.subtype_no and s.subject_no=e.subject_no and  feesub.syll_code=s.syll_code and feesub.promote_count=1 and s.subject_no=sc.subject_no and s.subject_no=" + subject_nofromexmtt + " and s.subtype_no=sc.subtype_no and roll_no='" + rollnosub + "' and semester=" + Session["semforsub"].ToString() + " order by exam_date";
                                    SqlDataAdapter da4 = new SqlDataAdapter(examinforegular, con3);
                                    string subject_code = "";
                                    string subject_name = "";
                                    string subject_no1 = "";
                                    string exam_code = "";
                                    //int subjectcount1 = 0;
                                    //int rowcount1 = 20;
                                    DataSet ds4 = new DataSet();
                                    da4.Fill(ds4);
                                    con3.Close();
                                    con3.Open();

                                    if (ds4.Tables[0].Rows.Count > 0)
                                    {
                                        flagforarrreg = 1;
                                        //i1++;
                                        //Button2.Visible = true;
                                        //int i1 = 0;
                                        for (int i = 0; i < ds4.Tables[0].Rows.Count; i++)
                                        {

                                            subjectcount++;
                                            regularcount = countforregular;
                                            if (subjectcount > rowcount)
                                            {
                                                rowcount = rowcount + 20;
                                                printspread.Sheets[0].RowCount = printspread.Sheets[0].RowCount + 45;
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 45, 0, 5, 1);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 45, 6, 5, 1);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 39, 6, 5, 1);
                                                //

                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 45, 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 44, 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 43, 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 42, 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 41, 1, 1, 5);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 40, 1, 1, 5);
                                                //for reg spanning
                                                //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 40, 0, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 40, 2, 1, 4);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 39, 0, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 39, 2, 1, 4);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 38, 0, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 38, 2, 1, 4);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 37, 0, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 37, 2, 1, 4);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 36, 0, 1, 2);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 36, 2, 1, 4);
                                                //bottom page
                                                //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 7, 0, 1, 3);
                                                //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 3, 0, 1, 3);
                                                //printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 3, 4, 1, 3);
                                                //for subjectname span
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 34, 3, 1, 3);
                                                //for session span
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 11, 4, 1, 3);
                                                printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 10, 4, 1, 3);
                                                if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                                                {
                                                    string college = "select isnull(collname,'') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code=" + Session["collegecode"] + "";
                                                    SqlCommand collegecmd = new SqlCommand(college, con);
                                                    SqlDataReader collegename;
                                                    con.Close();
                                                    con.Open();
                                                    collegename = collegecmd.ExecuteReader();
                                                    if (collegename.HasRows)
                                                    {

                                                        while (collegename.Read())
                                                        {
                                                            affliatedby = collegename["affliated"].ToString();
                                                            catgory = collegename["category"].ToString();
                                                            collnamenew1 = collegename["collname"].ToString();
                                                            address1 = collegename["address1"].ToString();
                                                            address3 = collegename["address3"].ToString();
                                                            pincode = collegename["pincode"].ToString();
                                                            address = address1 + "," + address3 + "-" + " " + pincode;
                                                            catgory = "(An " + catgory + " Institution)";
                                                            affiliated = "Affliated to" + " " + affliatedby;

                                                        }
                                                    }

                                                }
                                                MyImg collogo2 = new MyImg();
                                                collogo2.ImageUrl = "Handler/Handler2.ashx?";
                                                MyImg collogoright2 = new MyImg();
                                                collogoright2.ImageUrl = "~/images/10BIT001.jpeg";
                                                collogoright2.ImageUrl = "Handler/Handler5.ashx?";
                                                printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 45, 6].CellType = collogoright2;
                                                printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 45, 0].CellType = collogo2;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 0].HorizontalAlign = HorizontalAlign.Center;
                                                rollnosub = FpSpread2.Sheets[0].Cells[res, 1].Text;
                                                MyImg mi3 = new MyImg();
                                                mi3.ImageUrl = "Handler/Handler4.ashx?rollno=" + rollnosub;
                                                exammonthnew = ddlMonth.SelectedItem.Text;
                                                examyear = ddlYear.SelectedValue.ToString();
                                                printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 39, 6].CellType = mi3;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 6].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 0].Font.Size = FontUnit.Medium;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 0].Font.Bold = true;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 1].Text = collnamenew1;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 44, 1].Text = address;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 43, 1].Text = catgory;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 42, 1].Text = affiliated;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 41, 1].Text = "UG/PG Examinations," + exammonthnew + "-" + examyear + "";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 0].Margin.Left = 50;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 1].Text = "HALL TICKET";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 1].HorizontalAlign = HorizontalAlign.Center;
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 7, 0].Text = "Signature of the Candidate";
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 7, 0].Margin.Left = 15;
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 3, 4].Text = "Controller of Examinations";
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 3, 4].HorizontalAlign = HorizontalAlign.Right;
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 3, 0].Margin.Left = 15;
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 3, 0].Text = "Head of the Department";

                                                stuname = FpSpread2.Sheets[0].Cells[res, 3].Text;
                                                regnumber = FpSpread2.Sheets[0].Cells[res, 2].Text;
                                                sem = Session["semforsub"].ToString();
                                                degreecode = Session["selecteddegreecode"].ToString();
                                                batch = FpSpread2.Sheets[0].Cells[res, 0].Note;
                                                degree = Session["selecteddegree"].ToString();
                                                course = Session["selectedcourse"].ToString();
                                                dateofbirth = "select convert(varchar(20),a.dob,103) as dobstudent from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollnosub + "'";
                                                SqlDataAdapter da14 = new SqlDataAdapter(dateofbirth, con2);
                                                dob = "";
                                                DataSet ds14 = new DataSet();
                                                da14.Fill(ds14);
                                                con2.Close();
                                                con2.Open();

                                                if (ds14.Tables[0].Rows.Count > 0)
                                                {
                                                    dob = ds14.Tables[0].Rows[0]["dobstudent"].ToString();
                                                }
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 0].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 0].Text = "Name of the candidate" + ":";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 0].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 2].Text = stuname;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 0].Text = "Date of Birth" + ":";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 2].Text = dob;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 0].Text = "Registration Number" + ":";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 0].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 2].Text = regnumber;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 0].Text = "Programme" + ":";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 0].Margin.Left = 15;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 2].Text = degree + "-" + course;
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 0].Text = "Branch" + ":";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 0].Margin.Left = 15;
                                                //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 2].Text = course;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 45, 0].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 44, 0].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 43, 0].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 42, 0].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 41, 0].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 35].Border.BorderColorBottom = Color.Black;
                                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 34].HorizontalAlign = HorizontalAlign.Center;
                                                printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 34].Border.BorderColor = Color.Black;
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 0].Text = "Date";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 1].Text = "Semester";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 2].Text = "Subject Code";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 3].Text = "Subject Title";
                                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 6].Text = "Session";

                                                regularcount = countforregular + 44;
                                                i1 = 0;

                                            }

                                            subject_code = ds4.Tables[0].Rows[i]["subcode"].ToString();
                                            subject_name = ds4.Tables[0].Rows[i]["subname"].ToString();
                                            subject_no1 = ds4.Tables[0].Rows[i]["subject_no"].ToString();
                                            exam_date = ds4.Tables[0].Rows[i]["exam_date"].ToString();

                                            DateTime obtaineddate = Convert.ToDateTime(exam_date);
                                            string examdatenew = obtaineddate.ToString("dd/MM/yyyy");
                                            exam_session = ds4.Tables[0].Rows[0]["exam_session"].ToString();
                                            //exam_code = ds4.Tables[0].Rows[0]["exmcode"].ToString();
                                            printspread.Sheets[0].Cells[regularcount + i1, 1].Text = sem;
                                            printspread.Sheets[0].Cells[regularcount + i1, 1].HorizontalAlign = HorizontalAlign.Center;
                                            printspread.Sheets[0].Cells[regularcount + i1, 2].Text = subject_code;
                                            printspread.Sheets[0].Cells[regularcount + i1, 2].HorizontalAlign = HorizontalAlign.Center;
                                            printspread.Sheets[0].Cells[regularcount + i1, 3].Text = subject_name;
                                            printspread.Sheets[0].Cells[regularcount + i1, 3].HorizontalAlign = HorizontalAlign.Left;
                                            printspread.Sheets[0].Rows[regularcount + i1].Border.BorderColor = Color.Black;
                                            printspread.Sheets[0].SpanModel.Add(regularcount + i1, 3, 1, 3);
                                            printspread.Sheets[0].Cells[regularcount + i1, 1].CellType = txt;//Added By Srinath 21/3/20132
                                            printspread.Sheets[0].Cells[regularcount + i1, 0].Text = examdatenew;
                                            printspread.Sheets[0].Cells[regularcount + i1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            printspread.Sheets[0].Cells[regularcount + i1, 6].Text = exam_session;
                                            printspread.Sheets[0].Cells[regularcount + i1, 6].HorizontalAlign = HorizontalAlign.Center;

                                            //string examyear = ddlYear.SelectedValue.ToString();


                                            i1++;

                                        }

                                        bottomtext = regularcount + i1;

                                    }
                                }
                            supplyformat_i:
                                exammonth = ddlMonth.SelectedIndex.ToString();
                                if (exam_session == "F.N")
                                {
                                    //for FN
                                    string fntime = "select distinct convert(Varchar(8),ex.start_time,108) as start,convert(Varchar(8),ex.end_time,108) as end1 from exmtt e,exmtt_det ex  where ex.exam_session='F.N' and ex.start_time<> ex.end_time and e.exam_code=ex.exam_code and e.degree_code=" + degreecode + " and e.batchto in(" + batch + ")  and e.exam_month=" + exammonth + " and e.exam_year =" + examyear + " ";
                                    SqlDataAdapter da7 = new SqlDataAdapter(fntime, con1);

                                    string start_time1 = "";
                                    string end_time1 = "";
                                    DataSet ds7 = new DataSet();
                                    da7.Fill(ds7);
                                    con1.Close();
                                    con1.Open();

                                    if (ds7.Tables[0].Rows.Count > 0)
                                    {
                                        start_time1 = ds7.Tables[0].Rows[0]["start"].ToString();
                                        end_time1 = ds7.Tables[0].Rows[0]["end1"].ToString();
                                        if ((start_time1 != "") && (end_time1 != ""))
                                        {
                                            time = "*FN" + ":" + start_time1 + " " + "AM" + "to" + " " + end_time1 + " " + "AM";

                                        }

                                    }
                                }
                                //for AN
                                if (exam_session == "A.N")
                                {
                                    string antime = "select distinct convert(Varchar(8),ex.start_time,108) as start,convert(Varchar(8),ex.end_time,108) as end1 from exmtt e,exmtt_det ex  where ex.exam_session='A.N' and ex.start_time<> ex.end_time and e.exam_code=ex.exam_code and e.degree_code=" + degreecode + " and e.batchto in(" + batch + ")  and e.exam_month=" + exammonth + " and e.exam_year =" + examyear + " ";
                                    SqlDataAdapter da6 = new SqlDataAdapter(antime, con1);

                                    string start_time = "";
                                    string end_time = "";
                                    DataSet ds6 = new DataSet();
                                    da6.Fill(ds6);
                                    con1.Close();
                                    con1.Open();

                                    if (ds6.Tables[0].Rows.Count > 0)
                                    {
                                        start_time = ds6.Tables[0].Rows[0]["start"].ToString();
                                        end_time = ds6.Tables[0].Rows[0]["end1"].ToString();
                                        if ((start_time != "") && (end_time != ""))
                                        {
                                            time1 = "*AN" + ":" + start_time + " " + "PM" + "to" + " " + end_time + " " + "PM";

                                        }


                                    }
                                }
                            }

                            //
                            if (flagforarrreg == 1)
                            {
                                printspread.Sheets[0].SpanModel.Add(bottomtext + 6, 0, 1, 3);
                                printspread.Sheets[0].SpanModel.Add(bottomtext + 6, 4, 1, 3);
                                printspread.Sheets[0].Cells[bottomtext + 6, 0].Margin.Left = 20;
                                printspread.Sheets[0].Cells[bottomtext + 6, 0].Text = "Signature of the Candidate";
                                printspread.Sheets[0].Cells[bottomtext + 6, 4].HorizontalAlign = HorizontalAlign.Right;
                                printspread.Sheets[0].Cells[bottomtext + 6, 4].Margin.Right = 30;
                                printspread.Sheets[0].Cells[bottomtext + 6, 4].Text = "Controller of Examinations";
                                printspread.Sheets[0].SpanModel.Add(bottomtext + 2, 4, 1, 3);
                                printspread.Sheets[0].Cells[bottomtext + 2, 4].Text = time;
                                printspread.Sheets[0].Cells[bottomtext + 2, 4].HorizontalAlign = HorizontalAlign.Right;
                                printspread.Sheets[0].SpanModel.Add(bottomtext + 3, 4, 1, 3);
                                printspread.Sheets[0].Cells[bottomtext + 3, 4].Text = time1;
                                printspread.Sheets[0].Cells[bottomtext + 3, 4].HorizontalAlign = HorizontalAlign.Right;
                                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Click Print Button to view HallTicket')", true);
                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 0].Border.BorderColor = Color.Black;
                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 1].Border.BorderColor = Color.Black;
                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 2].Border.BorderColor = Color.Black;
                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 3].Border.BorderColor = Color.Black;
                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 4].Border.BorderColor = Color.Black;
                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 5].Border.BorderColor = Color.Black;
                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 6].Border.BorderColor = Color.Black;
                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 0].Border.BorderColorTop = Color.White;
                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 1].Border.BorderColorTop = Color.White;
                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 2].Border.BorderColorTop = Color.White;
                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 3].Border.BorderColorTop = Color.White;
                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 4].Border.BorderColorTop = Color.White;
                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 5].Border.BorderColorTop = Color.White;
                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 6].Border.BorderColorTop = Color.White;
                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 0].Border.BorderColorRight = Color.White;
                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 1].Border.BorderColorRight = Color.White;
                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 2].Border.BorderColorRight = Color.White;
                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 3].Border.BorderColorRight = Color.White;
                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 4].Border.BorderColorRight = Color.White;
                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 5].Border.BorderColorRight = Color.White;
                                printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 6].Border.BorderColorRight = Color.White;
                            }
                        }

                        FpSpread2.Sheets[0].Cells[res, 6].Value = 0;
                    }
                }
            }
            else
            {
                if (ddlMonth.SelectedValue.ToString() == "0")
                {
                    selectMonth.Visible = true;
                }
                if (ddlYear.SelectedValue.ToString() == "0")
                {
                    selectyear.Visible = true;
                }
            }
        }
        catch
        {

        }
    }

    //Button2.Visible = false;
    //FpSpread2.SaveChanges();
    //printspread.SaveChanges();
    //printspread.Visible = true;
    //printspread.Sheets[0].RowCount = 0;
    ////printspread.Sheets[0].AutoPostBack = true;
    //printspread.Sheets[0].PageSize = 40;
    //printspread.Sheets[0].ColumnCount = 12;
    //printspread.Sheets[0].RowHeader.Visible = false;
    //printspread.Sheets[0].ColumnHeader.Visible = false;
    //printspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Small;
    //printspread.Sheets[0].Columns[0].Width = 60;
    //printspread.Sheets[0].Columns[1].Width = 80;
    //printspread.Sheets[0].Columns[2].Width = 60;
    //printspread.Sheets[0].Columns[3].Width = 90;
    //printspread.Sheets[0].Columns[4].Width = 70;
    //printspread.Sheets[0].Columns[5].Width = 70;
    //printspread.Sheets[0].Columns[6].Width = 60;
    //printspread.Sheets[0].Columns[7].Width = 80;
    //printspread.Sheets[0].Columns[8].Width = 60;
    //printspread.Sheets[0].Columns[9].Width = 90;
    //printspread.Sheets[0].Columns[10].Width = 70;
    //printspread.Sheets[0].Columns[11].Width = 70;
    //printspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
    //printspread.Sheets[0].DefaultStyle.Font.Bold = true;
    //string collnamenew1 = "";
    //string address1 = "";
    //string address3 = "";
    //string address = "";
    //string pincode = "";

    //for (int res = 0; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
    //{

    //    int isval = 0;
    //    string s = FpSpread2.Sheets[0].Cells[res, 6].Text;

    //    isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);
    //    if (isval == 1)
    //    {
    //        Button2.Visible = true;
    //        //printbtn.Visible = false;
    //        printspread.Sheets[0].RowCount = printspread.Sheets[0].RowCount + 40;
    //        //for logo
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 40, 1, 5, 1);
    //        //for photo
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 34, 10, 4, 1);
    //        //
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 40, 2, 1, 12);
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 39, 2, 1, 12);
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 38, 2, 1, 12);
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 37, 2, 1, 12);
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 36, 2, 1, 12);
    //        //for reg spanning
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 35, 0, 1, 2);
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 35, 2, 1, 2);
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 35, 6, 1, 2);
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 35, 8, 1, 2);
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 33, 0, 1, 2);
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 33, 2, 1, 4);
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 31, 0, 1, 2);
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 31, 2, 1, 2);
    //        //for subjectname span
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 29, 4, 1, 2);
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 29, 10, 1, 2);
    //        //for after pagedownspan
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 11, 0, 1, 3);
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 11, 4, 1, 2);
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 9, 4, 1, 2);
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 7, 4, 1, 2);
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 7, 8, 1, 3);
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 5, 3, 1, 5);
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 3, 0, 1, 3);
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 1, 3, 1, 4);
    //        printspread.Sheets[0].SpanModel.Add(printspread.Sheets[0].RowCount - 1, 8, 1, 3);
    //        if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
    //        {
    //            string college = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code=" + Session["collegecode"] + "";
    //            SqlCommand collegecmd = new SqlCommand(college, con);
    //            SqlDataReader collegename;
    //            con.Close();
    //            con.Open();
    //            collegename = collegecmd.ExecuteReader();
    //            if (collegename.HasRows)
    //            {

    //                while (collegename.Read())
    //                {
    //                    collnamenew1 = collegename["collname"].ToString();
    //                    address1 = collegename["address1"].ToString();
    //                    address3 = collegename["address3"].ToString();

    //                    pincode = collegename["pincode"].ToString();
    //                    address = address1 + "," + address3 + "-" + pincode;

    //                }
    //            }

    //        }
    //        MyImg collogo = new MyImg();
    //        collogo.ImageUrl = "Handler/Handler2.ashx?";

    //        printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 40, 1].CellType = collogo;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 1].HorizontalAlign = HorizontalAlign.Center;
    //        string rollnosub = FpSpread2.Sheets[0].Cells[res, 1].Text;
    //        MyImg mi1 = new MyImg();
    //        mi1.ImageUrl = "Handler/Handler4.ashx?rollno=" + rollnosub;

    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 10].Border.BorderColor = Color.Black;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 35, 10].Border.BorderColorBottom = Color.Black;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 33, 9].Border.BorderColorRight = Color.Black;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 32, 9].Border.BorderColorRight = Color.Black;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 31, 9].Border.BorderColorRight = Color.Black;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 9].Border.BorderColorRight = Color.Black;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 1].Border.BorderColorBottom = Color.Black;
    //        printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 30].Border.BorderColorBottom = Color.Black;
    //        printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 29].Border.BorderColorBottom = Color.Black;
    //        //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 29, 6].Border.BorderColorLeft  = Color.Black;
    //        //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 29, 5].Border.BorderColorBottom = Color.Black;
    //        printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 36].Border.BorderColorBottom = Color.Black;
    //        printspread.Sheets[0].Cells[Convert.ToInt16(printspread.Sheets[0].RowCount) - 34, 10].CellType = mi1;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 34, 10].HorizontalAlign = HorizontalAlign.Center;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 2].Font.Size = FontUnit.Medium;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 2].Font.Bold = true;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 2].Text = collnamenew1;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 2].Text = address;
    //        //printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 2].Font.Size = FontUnit.Small ;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 2].Text = "OFFICE OF THE CONTROLLER OF EXAMINATIONS";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 2].Text = "APPLICATION FORM FOR END SEMESTER EXAMINATIONS FEB.2011 / MAR.2011";
    //        string stuname = FpSpread2.Sheets[0].Cells[res, 3].Text;
    //        string regnumber = FpSpread2.Sheets[0].Cells[res, 2].Text;
    //        string degree = Session["selecteddegree"].ToString();
    //        string course = Session["selectedcourse"].ToString();

    //        string dateofbirth = "select convert(varchar(20),a.dob,103) as dobstudent from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollnosub + "'";
    //        SqlDataAdapter da3 = new SqlDataAdapter(dateofbirth, con2);
    //        string dob = "";
    //        DataSet ds3 = new DataSet();
    //        da3.Fill(ds3);
    //        con2.Close();
    //        con2.Open();

    //        if (ds3.Tables[0].Rows.Count > 0)
    //        {
    //            dob = ds3.Tables[0].Rows[0]["dobstudent"].ToString();
    //        }
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 35, 0].Text = "Student Name" + ":";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 35, 2].Text = stuname;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 35, 6].Text = "Register Number" + ":";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 35, 8].Text = regnumber;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 33, 0].Text = "Degree & Branch" + ":";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 33, 2].Text = degree + "-" + course;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 31, 0].Text = "Date Of Birth" + ":";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 31, 2].Text = dob;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 35, 0].HorizontalAlign = HorizontalAlign.Center;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 35, 6].HorizontalAlign = HorizontalAlign.Center;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 33, 0].HorizontalAlign = HorizontalAlign.Center;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 31, 0].HorizontalAlign = HorizontalAlign.Center;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 40, 2].HorizontalAlign = HorizontalAlign.Center;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 39, 2].HorizontalAlign = HorizontalAlign.Center;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 38, 2].HorizontalAlign = HorizontalAlign.Center;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 37, 2].HorizontalAlign = HorizontalAlign.Center;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 36, 2].HorizontalAlign = HorizontalAlign.Center;
    //        printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 29].HorizontalAlign = HorizontalAlign.Center;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 29, 0].Text = "Sem";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 29, 1].Text = "Date";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 29, 2].Text = "Session";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 29, 3].Text = "Sub.code";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 29, 4].Text = "Subject Title";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 29, 6].Text = "Sem";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 29, 7].Text = "Date";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 29, 8].Text = "Session";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 29, 9].Text = "Sub.code";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 29, 10].Text = "Subject Title";

    //        string Arrearsub = "Select isnull(Subject_Code,'') as scode , isnull(subjecT_name,'') as sname , semester as ssem,feesub.fee_per_paper,feesub.arr_fee as arrearfees from subject,syllabus_master as smas,sub_sem as feesub where feesub.subtype_no=subject.subtype_no and smas.syll_code = subject.syll_code and subject_no in (select distinct subject_no from  mark_entry where subject_no in (select distinct subject_no from mark_entry where passorfail=0 and result='Fail' and ltrim(rtrim(roll_no))='" + rollnosub + "') and ltrim(rtrim(roll_no))='" + rollnosub + "' and Semester >= 1 and Semester < " + Session["semforsub"].ToString() + " ) order by smas.semester , scode";
    //        SqlDataAdapter da1 = new SqlDataAdapter(Arrearsub, con2);
    //        DataSet ds1 = new DataSet();
    //        da1.Fill(ds1);
    //        con2.Close();
    //        con2.Open();
    //        int totalsubjectcount = 0;
    //        int totalfees = 0;
    //        int regularcount = printspread.Sheets[0].RowCount - 28;
    //        if (ds1.Tables[0].Rows.Count > 0)
    //        {
    //            totalsubjectcount = ds1.Tables[0].Rows.Count;
    //            int count;
    //            int j;
    //            count = printspread.Sheets[0].RowCount - 28;
    //            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
    //            {

    //                printspread.Sheets[0].Cells[count + i, 0].Text = ds1.Tables[0].Rows[i]["ssem"].ToString();
    //                printspread.Sheets[0].Cells[count + i, 1].Text = ds1.Tables[0].Rows[i]["scode"].ToString();
    //                printspread.Sheets[0].SpanModel.Add(count + i, 2, 1, 3);
    //                printspread.Sheets[0].Cells[count + i, 2].Text = ds1.Tables[0].Rows[i]["sname"].ToString();
    //                printspread.Sheets[0].Cells[count + i, 5].Text = ds1.Tables[0].Rows[i]["arrearfees"].ToString();
    //                totalfees = totalfees + Convert.ToInt32(ds1.Tables[0].Rows[i]["arrearfees"]);
    //                printspread.Sheets[0].Cells[count + i, 0].HorizontalAlign = HorizontalAlign.Center;
    //                printspread.Sheets[0].Cells[count + i, 0].Font.Size = FontUnit.Small;
    //                printspread.Sheets[0].Cells[count + i, 1].Font.Size = FontUnit.Small;
    //                printspread.Sheets[0].Cells[count + i, 2].Font.Size = FontUnit.Small;
    //                printspread.Sheets[0].Cells[count + i, 5].Font.Size = FontUnit.Small;
    //                printspread.Sheets[0].Cells[regularcount + i, 5].HorizontalAlign = HorizontalAlign.Center;
    //                printspread.Sheets[0].Cells[count + i, 0].Font.Bold = false;
    //                printspread.Sheets[0].Cells[count + i, 1].Font.Bold = false;
    //                printspread.Sheets[0].Cells[count + i, 2].Font.Bold = false;
    //                printspread.Sheets[0].Cells[count + i, 5].Font.Bold = false;
    //                regularcount = regularcount + 1;
    //                if (count + i == printspread.Sheets[0].RowCount - 12)
    //                {
    //                    for (j = i; j < ds1.Tables[0].Rows.Count; j++)
    //                    {
    //                        printspread.Sheets[0].Cells[count, 5].Text = ds1.Tables[0].Rows[j]["ssem"].ToString();
    //                        printspread.Sheets[0].Cells[count, 6].Text = ds1.Tables[0].Rows[j]["scode"].ToString();
    //                        printspread.Sheets[0].SpanModel.Add(count, 7, 1, 3);
    //                        printspread.Sheets[0].Cells[count, 7].Text = ds1.Tables[0].Rows[j]["sname"].ToString();
    //                        printspread.Sheets[0].Cells[count, 11].Text = ds1.Tables[0].Rows[j]["arrearfees"].ToString();
    //                        //printspread.Sheets[0].Cells[count, 11].Border.BorderColorRight = Color.Black;
    //                        totalfees = totalfees + Convert.ToInt32(ds1.Tables[0].Rows[i]["arrearfees"]);
    //                        printspread.Sheets[0].Cells[count, 5].HorizontalAlign = HorizontalAlign.Center;
    //                        printspread.Sheets[0].Cells[count, 5].Font.Size = FontUnit.Small;
    //                        printspread.Sheets[0].Cells[count, 6].Font.Size = FontUnit.Small;
    //                        printspread.Sheets[0].Cells[count, 7].Font.Size = FontUnit.Small;
    //                        printspread.Sheets[0].Cells[count, 11].Font.Size = FontUnit.Small;
    //                        printspread.Sheets[0].Cells[regularcount, 11].HorizontalAlign = HorizontalAlign.Center;
    //                        printspread.Sheets[0].Cells[count, 5].Font.Bold = false;
    //                        printspread.Sheets[0].Cells[count, 6].Font.Bold = false;
    //                        printspread.Sheets[0].Cells[count, 7].Font.Bold = false;
    //                        printspread.Sheets[0].Cells[count, 11].Font.Bold = false;
    //                        count++;
    //                    }
    //                    i = j;
    //                    regularcount = count;
    //                }

    //            }

    //        }
    //        else
    //        {
    //            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('There is no arrear subjects')", true);

    //        }


    //        string regularsub = "select isnull(Subject_Code,'') as scode , isnull(subjecT_name,'') as sname , semester as ssem,feesub.fee_per_paper as regularfees,feesub.arr_fee from subjectchooser sc,subject s,sub_sem as feesub where feesub.subtype_no=s.subtype_no and  s.subject_no=sc.subject_no and s.subtype_no=sc.subtype_no and ltrim(rtrim(roll_no))='" + rollnosub + "' and semester=" + Session["semforsub"].ToString() + "";
    //        SqlDataAdapter da2 = new SqlDataAdapter(regularsub, con3);
    //        DataSet ds2 = new DataSet();
    //        da2.Fill(ds2);
    //        con3.Close();
    //        con3.Open();
    //        if (ds2.Tables[0].Rows.Count > 0)
    //        {
    //            totalsubjectcount = totalsubjectcount + ds2.Tables[0].Rows.Count;
    //            int j;
    //            for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
    //            {

    //                printspread.Sheets[0].Cells[regularcount + i, 0].Text = ds2.Tables[0].Rows[i]["ssem"].ToString();
    //                printspread.Sheets[0].Cells[regularcount + i, 1].Text = ds2.Tables[0].Rows[i]["scode"].ToString();
    //                printspread.Sheets[0].SpanModel.Add(regularcount + i, 2, 1, 3);
    //                printspread.Sheets[0].Cells[regularcount + i, 2].Text = ds2.Tables[0].Rows[i]["sname"].ToString();
    //                printspread.Sheets[0].Cells[regularcount + i, 5].Text = ds2.Tables[0].Rows[i]["regularfees"].ToString();
    //                totalfees = totalfees + Convert.ToInt32(ds2.Tables[0].Rows[i]["regularfees"]);
    //                printspread.Sheets[0].Cells[regularcount + i, 0].HorizontalAlign = HorizontalAlign.Center;
    //                printspread.Sheets[0].Cells[regularcount + i, 0].Font.Size = FontUnit.Small;
    //                printspread.Sheets[0].Cells[regularcount + i, 1].Font.Size = FontUnit.Small;
    //                printspread.Sheets[0].Cells[regularcount + i, 2].Font.Size = FontUnit.Small;
    //                printspread.Sheets[0].Cells[regularcount + i, 5].Font.Size = FontUnit.Small;
    //                printspread.Sheets[0].Cells[regularcount + i, 5].HorizontalAlign = HorizontalAlign.Center;
    //                printspread.Sheets[0].Cells[regularcount + i, 0].Font.Bold = false;
    //                printspread.Sheets[0].Cells[regularcount + i, 1].Font.Bold = false;
    //                printspread.Sheets[0].Cells[regularcount + i, 2].Font.Bold = false;
    //                printspread.Sheets[0].Cells[regularcount + i, 5].Font.Bold = false;
    //                if (regularcount + i == printspread.Sheets[0].RowCount - 12)
    //                {
    //                    for (j = i; j < ds2.Tables[0].Rows.Count; j++)
    //                    {
    //                        printspread.Sheets[0].Cells[regularcount, 5].Text = ds2.Tables[0].Rows[j]["ssem"].ToString();
    //                        printspread.Sheets[0].Cells[regularcount, 6].Text = ds2.Tables[0].Rows[j]["scode"].ToString();
    //                        printspread.Sheets[0].SpanModel.Add(regularcount, 7, 1, 3);
    //                        printspread.Sheets[0].Cells[regularcount, 7].Text = ds2.Tables[0].Rows[j]["sname"].ToString();
    //                        printspread.Sheets[0].Cells[regularcount, 11].Text = ds2.Tables[0].Rows[j]["regularfees"].ToString();
    //                        totalfees = totalfees + Convert.ToInt32(ds2.Tables[0].Rows[i]["regularfees"]);
    //                        printspread.Sheets[0].Cells[regularcount, 5].HorizontalAlign = HorizontalAlign.Center;
    //                        printspread.Sheets[0].Cells[regularcount, 5].Font.Size = FontUnit.Small;
    //                        printspread.Sheets[0].Cells[regularcount, 6].Font.Size = FontUnit.Small;
    //                        printspread.Sheets[0].Cells[regularcount, 7].Font.Size = FontUnit.Small;
    //                        printspread.Sheets[0].Cells[regularcount, 11].Font.Size = FontUnit.Small;
    //                        printspread.Sheets[0].Cells[regularcount, 11].HorizontalAlign = HorizontalAlign.Center;
    //                        printspread.Sheets[0].Cells[regularcount, 5].Font.Bold = false;
    //                        printspread.Sheets[0].Cells[regularcount, 6].Font.Bold = false;
    //                        printspread.Sheets[0].Cells[regularcount, 7].Font.Bold = false;
    //                        printspread.Sheets[0].Cells[regularcount, 11].Font.Bold = false;
    //                        regularcount++;
    //                    }
    //                    i = j;

    //                }
    //            }

    //        }
    //        else
    //        {
    //            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('There is no regular subjects')", true);

    //        }
    //        string sem = HAllSpread.Sheets[0].Cells[res, 4].Text;
    //        string degreecode = HAllSpread.Sheets[0].Cells[res, 3].Note;
    //        string batyear = FpSpread2.Sheets[0].Cells[res, 0].Note;
    //        string appcost = "select top 1 cost_mark as appfees from exam_application where roll_no='" + rollnosub + "' and exam_code in (select exam_code from exam_details where degree_code=" + degreecode + " and current_semester=" + sem + " and batch_year=" + batyear + ")";
    //        SqlDataAdapter dacost = new SqlDataAdapter(appcost, con3);
    //        DataSet dscost = new DataSet();
    //        dacost.Fill(dscost);
    //        con3.Close();
    //        con3.Open();
    //        int marksheetfees = 0;
    //        if (dscost.Tables[0].Rows.Count > 0)
    //        {
    //            marksheetfees = Convert.ToInt32(dscost.Tables[0].Rows[0]["appfees"]);
    //        }
    //        else
    //        {
    //            marksheetfees = 0;
    //        }
    //        DateTime currentdate = System.DateTime.Now;
    //        string newdate = currentdate.ToString("dd/MM/yyyy");
    //        printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 12].Border.BorderColorBottom = Color.Black;
    //        printspread.Sheets[0].Rows[printspread.Sheets[0].RowCount - 6].Border.BorderColorBottom = Color.Black;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 11, 0].Text = "No.of Subject(s) Registered";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 11, 3].Text = totalsubjectcount + "";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 11, 4].Text = "Exam Fees (Rs)";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 11, 6].Text = totalfees + "";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 9, 4].Text = "Marksheet Fees (Rs)";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 9, 6].Text = marksheetfees + "";
    //        int totalfeesnew = totalfees + marksheetfees;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 7, 4].Text = "Total Fees (Rs)";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 7, 6].Text = totalfeesnew + "";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 7, 8].Text = "Signature of the Candidate";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 5, 3].Text = "These Particulars are verified and found correct";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 3, 0].Margin.Left = 20;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 3, 0].Text = "Date :" + newdate;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 3].Text = "Signature of the HOD";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 8].Text = "Signature of the Principal";
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 11, 0].HorizontalAlign = HorizontalAlign.Center;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 7, 8].HorizontalAlign = HorizontalAlign.Right;
    //        printspread.Sheets[0].Cells[printspread.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Right;
    //    }
    //}

    protected void Backbtn_Click(object sender, EventArgs e)
    {
        //FpSpread2.Visible = false;
        //HAllSpread.Visible = true;
        //Backbtn.Visible = false;
        //printbtn.Visible = false;
        //Button2.Visible = false;
        //Label2.Visible = false;
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        //printbtn.Visible = true;
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Button2.Visible = false;
        FpSpread2.Visible = false;
        printbtn.Visible = false;
        Rangechk.Visible = false;
        Label2.Visible = false;
        HAllSpread.Visible = false;
        selectMonth.Visible = false;
        //string getexamdate = "select distinct convert(varchar(10),exdt.Exam_date,105) as exam_date from exmtt_det as exdt,exmtt as exm where exm.exam_code=exdt.exam_code and exm.exam_month=" + ddlMonth.SelectedIndex.ToString() + " and exm.exam_year=" + ddlYear.SelectedValue.ToString() + " order by exam_date";
        //SqlDataAdapter da9 = new SqlDataAdapter(getexamdate, con2);
        //DataSet ds9 = new DataSet();
        //da9.Fill(ds9);
        //con2.Close();
        //con2.Open();

        //if (ds9.Tables[0].Rows.Count > 0)
        //{
        //    ddldate.DataSource = ds9;
        //    ddldate.DataValueField = "Exam_date";
        //    ddldate.DataBind();
        //    ddldate.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));

        //}
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Button2.Visible = false;
        FpSpread2.Visible = false;
        printbtn.Visible = false;
        Rangechk.Visible = false;
        Label2.Visible = false;
        HAllSpread.Visible = false;
        selectyear.Visible = false;
        //string getexamdate = "select distinct convert(varchar(10),exdt.Exam_date,105) as exam_date from exmtt_det as exdt,exmtt as exm where exm.exam_code=exdt.exam_code and exm.exam_month=" + ddlMonth.SelectedIndex.ToString() + " and exm.exam_year=" + ddlYear.SelectedValue.ToString() + " order by exam_date";
        //SqlDataAdapter da9 = new SqlDataAdapter(getexamdate, con2);
        //DataSet ds9 = new DataSet();
        //da9.Fill(ds9);
        //con2.Close();
        //con2.Open();

        //if (ds9.Tables[0].Rows.Count > 0)
        //{
        //    ddldate.DataSource = ds9;
        //    ddldate.DataValueField = "Exam_date";
        //    ddldate.DataBind();
        //    ddldate.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));

        //}
    }

    protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
    {

        if (RadioButton4.Checked == true)
        {
            chk_sesdat.Visible = true;
        }
        else
        {
            chk_sesdat.Visible = false;
        }

       
        hallticket();
    }

    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        chk_sesdat.Visible = false;
        hallticketNECFormat();
    }

    public string GetFunction(string Att_strqueryst)
    {

        string sqlstr;
        sqlstr = Att_strqueryst;
        getsql.Close();
        getsql.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, getsql);
        SqlDataReader drnew;
        SqlCommand cmd = new SqlCommand(sqlstr);
        cmd.Connection = getsql;
        drnew = cmd.ExecuteReader();
        drnew.Read();

        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "";
        }
    }

    public void newhallticket()
    {
        try
        {
            btnhallpdf();
        }
        catch
        {

        }
    }

    //modified by Prabha 26/09/2017 to sort by subject priority uncomment the qry variable
    public void formatfour()
    {
        try
        {
            int selectedcount = 0;
            for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
            {
                int isval = 0;
                string s = FpSpread2.Sheets[0].Cells[res, 6].Text;

                isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);
                if (isval == 1)
                {
                    selectedcount++;
                    errmsg.Text = "";
                }
            }
            if (selectedcount == 0)
            {
                errmsg.Text = "Please Select the Student and then Proceed";
                return;
            }
            Font Fontbold4 = new Font("Times New Roman", 18, FontStyle.Bold);
            Font Fontsmall5 = new Font("Times New Roman", 14, FontStyle.Regular);
            Font Fontbold5 = new Font("Times New Roman", 10, FontStyle.Bold);
            Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);

            Font Font = new Font("Times New Roman", 12, FontStyle.Regular);
            Font Fontbold = new Font("Times New Roman", 12, FontStyle.Bold);
            Font Fontboldbig = new Font("Times New Roman", 14, FontStyle.Bold);
            Font Fontbold1 = new Font("Times New Roman", 12, FontStyle.Bold);
            Font Fontbold2 = new Font("Book Antiqua", 17, FontStyle.Regular);
            Font Fontsmall = new Font("Times New Roman", 12, FontStyle.Bold);
            Font Fontsmall1 = new Font("Arial", 12, FontStyle.Bold);
            Font Fontsmall4 = new Font("Times New Roman", 12, FontStyle.Bold);
            Font Fontsmall2 = new Font("Times New Roman", 12, FontStyle.Regular);
            Font Fontsmall3 = new Font("Times New Roman", 11, FontStyle.Regular);
            Font Fontboldbig1 = new Font("Times New Roman", 16, FontStyle.Bold);//barath 1.04.17
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
            Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
            PdfArea tete = new PdfArea(mydoc, 25, 10, 800, 1100);

            PdfRectangle pr1 = new PdfRectangle(mydoc, tete, Color.Black);

            degreecode = Session["selecteddegreecode"].ToString();
            degree = Session["selecteddegree"].ToString();
            course = Session["selectedcourse"].ToString();
            Boolean halfflag = false;

            if ((ddlMonth.SelectedValue.ToString() != "0") && (ddlYear.SelectedValue.ToString() != "0"))
            {
                FpSpread2.SaveChanges();
                if (FpSpread2.Sheets[0].RowCount > 0)
                {
                    batch = FpSpread2.Sheets[0].Cells[1, 0].Note;
                }
                //string rolno1 = FpSpread2.Sheets[0].Cells[res, 1].Text.ToString();
                string strquery = "select * from collinfo where  college_code='" + Session["collegecode"].ToString() + "' ;";
                strquery = strquery + " Select  * from exam_seating where degree_code='" + degreecode + "'";
                strquery = strquery + " select distinct right(convert(nvarchar(100),ex.start_time,100),7) as start,right(convert(nvarchar(100),ex.end_time,100),7) as end1,ex.exam_session from exmtt e,exmtt_det ex  where ex.start_time<> ex.end_time and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and e.degree_code='" + degreecode + "' and e.batchFrom='" + batch + "'  and e.exam_code=ex.exam_code order by start desc";
                strquery = strquery + " select reg_no,roll_no,current_semester,(select photo from stdphoto s where r.app_no=s.app_no) as photo from registration r where r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'";
                DataSet dshall = d2.select_method_wo_parameter(strquery, "Text");


                string forenon = "";
                string afterenon = "";
                dshall.Tables[2].DefaultView.RowFilter = " exam_session='F.N'";
                DataView dvse = dshall.Tables[2].DefaultView;
                if (dvse.Count > 0)
                {
                    forenon = dvse[0]["start"].ToString() + " - " + dvse[0]["end1"].ToString();
                }
                dshall.Tables[2].DefaultView.RowFilter = " exam_session='A.N'";
                dvse = dshall.Tables[2].DefaultView;
                if (dvse.Count > 0)
                {
                    afterenon = dvse[dvse.Count - 1]["start"].ToString() + " - " + dvse[dvse.Count - 1]["end1"].ToString();
                }
                string name = "";
                string regno = "";
                string rolno = "";
                string collname = "";
                string address = "";
                string pincode = "";
                string university = "";
                string category = "";
                string ad3 = "";
                string affiliated = "";
                int xy = 20;
                if (dshall.Tables[0].Rows.Count > 0)
                {
                    collname = dshall.Tables[0].Rows[0]["collname"].ToString();
                    affiliated = dshall.Tables[0].Rows[0]["affliatedby"].ToString();
                    string ad1 = dshall.Tables[0].Rows[0]["address1"].ToString();
                    string ad2 = dshall.Tables[0].Rows[0]["district"].ToString();
                    string ad4 = dshall.Tables[0].Rows[0]["state"].ToString();
                    ad3 = dshall.Tables[0].Rows[0]["address3"].ToString();
                    university = dshall.Tables[0].Rows[0]["university"].ToString();
                    category = dshall.Tables[0].Rows[0]["category"].ToString();
                    pincode = dshall.Tables[0].Rows[0]["pincode"].ToString();
                    address = ad1 + "-" + pincode + "," + ad2 + "," + ad4 + ".";

                }
                string iscurregual = "";
                if (CheckRegular.Checked == true && CheckArrear.Checked == false)
                {
                    iscurregual = " and  ead.attempts=0";
                }
                if (CheckRegular.Checked == false && CheckArrear.Checked == true)
                {
                    iscurregual = " and ead.attempts>0";
                }
                int isval = 0;
                string sem = "";
                for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
                {

                    Double coltop = 0;

                    string s = FpSpread2.Sheets[0].Cells[res, 6].Text;
                    isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);
                    
                    if (isval == 1)
                    {
                        name = FpSpread2.Sheets[0].Cells[res, 3].Text.ToString();
                        regno = FpSpread2.Sheets[0].Cells[res, 2].Text.ToString();
                        rolno = FpSpread2.Sheets[0].Cells[res, 1].Text.ToString();
                        string sql = "";
                        if (chksupplym.Checked == true)
                        {
                            sql = "  select ed.Exam_Month,ed.Exam_year,sk.lab,sk.subject_type,r.Reg_No,r.Roll_No,r.Stud_Name,sc.semester,s.subject_code,s.subject_name ,s.subject_no,ead.attempts from Exam_Details ed,exam_application ea,exam_appl_details ead ,Registration r,subject s,subjectChooser sc,sub_sem sk  where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ead.subject_no=s.subject_no and r.Roll_No=sc.roll_no and sk.subType_no=s.subType_no and sk.syll_code=s.syll_code and sk.subType_no=sc.subtype_no and sc.roll_no=ea.roll_no and sc.subject_no=ead.subject_no and s.subject_no=sc.subject_no and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "'  and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'and r.Roll_No='" + rolno + "' and ea.Exam_type=4   order by s.subjectpriority,sk.subject_type";//altered by madhumathi
                            //sql = " select  distinct ed.Exam_Month,ed.Exam_year,sk.lab,sk.subject_type,r.Reg_No,r.Roll_No,r.Stud_Name,sc.semester,s.subject_code,s.subject_name ,s.subject_no,ead.attempts,ISNULL(s.subjectpriority,'') as subjectpriority from syllabus_master sm, Exam_Details ed,exam_application ea,exam_appl_details ead ,Registration r,subject s,subjectChooser sc,sub_sem sk  where sm.degree_code=r.degree_code and sm.batch_year=r.batch_year and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ead.subject_no=s.subject_no and r.Roll_No=sc.roll_no and sk.subType_no=s.subType_no and sk.syll_code=s.syll_code and sk.subType_no=sc.subtype_no and sc.roll_no=ea.roll_no and sc.subject_no=ead.subject_no and s.subject_no=sc.subject_no and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "'  and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'and r.Roll_No='" + rolno + "' and ea.Exam_type=4   order by sc.semester desc, subjectpriority";

                            goto supplyfrmt4;
                        }
                        if (CheckRegular.Checked == true && CheckArrear.Checked == true)
                        {
                            sql = " select ed.Exam_Month,ed.Exam_year,sk.lab,sk.subject_type,r.Reg_No,r.Roll_No,r.Stud_Name,sc.semester,s.subject_code,s.subject_name ,s.subject_no,ead.attempts from Exam_Details ed,exam_application ea,exam_appl_details ead ,Registration r,subject s,subjectChooser sc,sub_sem sk  where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ead.subject_no=s.subject_no and r.Roll_No=sc.roll_no and sk.subType_no=s.subType_no and sk.syll_code=s.syll_code and sk.subType_no=sc.subtype_no and sc.roll_no=ea.roll_no and sc.subject_no=ead.subject_no and s.subject_no=sc.subject_no " + iscurregual + " and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "'  and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'and r.Roll_No='" + rolno + "' order by s.subjectpriority,sk.subject_type"; //altered by madhumathi
                            //sql = "select distinct ed.Exam_Month,ed.Exam_year,sk.lab,sk.subject_type,r.Reg_No,r.Roll_No,r.Stud_Name,sc.semester,s.subject_code,s.subject_name ,s.subject_no,ead.attempts,ISNULL(s.subjectpriority,'') as subjectpriority from syllabus_master sm, Exam_Details ed,exam_application ea,exam_appl_details ead ,Registration r,subject s,subjectChooser sc,sub_sem sk  where  sm.degree_code=r.degree_code and sm.batch_year=r.batch_year and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ead.subject_no=s.subject_no and r.Roll_No=sc.roll_no and sk.subType_no=s.subType_no and sk.syll_code=s.syll_code and sk.subType_no=sc.subtype_no and sc.roll_no=ea.roll_no and sc.subject_no=ead.subject_no and s.subject_no=sc.subject_no " + iscurregual + " and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "'  and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'and r.Roll_No='" + rolno + "'  order by sc.semester desc, subjectpriority";

                        }
                        else if (CheckRegular.Checked == true)
                        {
                            sql = "  select ed.Exam_Month,ed.Exam_year,sk.lab,sk.subject_type,r.Reg_No,r.Roll_No,r.Stud_Name,sc.semester,s.subject_code,s.subject_name ,s.subject_no,ead.attempts from Exam_Details ed,exam_application ea,exam_appl_details ead ,Registration r,subject s,subjectChooser sc,sub_sem sk  where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ead.subject_no=s.subject_no and r.Roll_No=sc.roll_no and sk.subType_no=s.subType_no and sk.syll_code=s.syll_code and sk.subType_no=sc.subtype_no and sc.roll_no=ea.roll_no and sc.subject_no=ead.subject_no and s.subject_no=sc.subject_no " + iscurregual + " and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "'  and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'and r.Roll_No='" + rolno + "'and ead.attempts='0' order by s.subjectpriority,sk.subject_type"; // altered by madhumathi
                            //sql = "  select distinct ed.Exam_Month,ed.Exam_year,sk.lab,sk.subject_type,r.Reg_No,r.Roll_No,r.Stud_Name,sc.semester,s.subject_code,s.subject_name ,s.subject_no,ead.attempts ,ISNULL(s.subjectpriority,'') as subjectpriority from syllabus_master sm, Exam_Details ed,exam_application ea,exam_appl_details ead ,Registration r,subject s,subjectChooser sc,sub_sem sk  where  sm.degree_code=r.degree_code and sm.batch_year=r.batch_year and  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ead.subject_no=s.subject_no and r.Roll_No=sc.roll_no and sk.subType_no=s.subType_no and sk.syll_code=s.syll_code and sk.subType_no=sc.subtype_no and sc.roll_no=ea.roll_no and sc.subject_no=ead.subject_no and s.subject_no=sc.subject_no  " + iscurregual + " and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "'  and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "'  and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'and r.Roll_No='" + rolno + "'and ead.attempts='0' order by sc.semester desc, subjectpriority  ";

                        }
                        else if (CheckArrear.Checked == true)
                        {
                            sql = "  select ed.Exam_Month,ed.Exam_year,sk.lab,sk.subject_type,r.Reg_No,r.Roll_No,r.Stud_Name,sc.semester,s.subject_code,s.subject_name ,s.subject_no,ead.attempts from Exam_Details ed,exam_application ea,exam_appl_details ead ,Registration r,subject s,subjectChooser sc,sub_sem sk  where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ead.subject_no=s.subject_no and r.Roll_No=sc.roll_no and sk.subType_no=s.subType_no and sk.syll_code=s.syll_code and sk.subType_no=sc.subtype_no and sc.roll_no=ea.roll_no and sc.subject_no=ead.subject_no and s.subject_no=sc.subject_no " + iscurregual + " and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "'  and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'and r.Roll_No='" + rolno + "'and ead.attempts='1' order by s.subjectpriority,sk.subject_type"; //altered by madhumathi

                            //sql = "select  distinct ed.Exam_Month,ed.Exam_year,sk.lab,sk.subject_type,r.Reg_No,r.Roll_No,r.Stud_Name,sc.semester,s.subject_code,s.subject_name ,s.subject_no,ead.attempts,ISNULL(s.subjectpriority,'') as subjectpriority from syllabus_master sm, Exam_Details ed,exam_application ea,exam_appl_details ead ,Registration r,subject s,subjectChooser sc,sub_sem sk where  sm.degree_code=r.degree_code and sm.batch_year=r.batch_year and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ead.subject_no=s.subject_no and r.Roll_No=sc.roll_no and sk.subType_no=s.subType_no and sk.syll_code=s.syll_code and sk.subType_no=sc.subtype_no and sc.roll_no=ea.roll_no and sc.subject_no=ead.subject_no and s.subject_no=sc.subject_no   " + iscurregual + "  and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "'  and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'and r.Roll_No='" + rolno + "'and ead.attempts='1'   order by sc.semester desc, subjectpriority ";
                        }
                    supplyfrmt4:
                        sql = sql + "  select et.start_time,et.end_time,et.subject_no,et.exam_session,convert(varchar(15),et.exam_date,103) as edate from exmtt_det et,exmtt e where et.exam_code=e.exam_code and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "'    and e.degree_code='" + degreecode + "' and e.batchFrom='" + batch + "'";
                        sql = sql + "select convert(varchar(20),a.dob,103) as dobstudent from applyn a,registration r where a.app_no=r.app_no and r.Roll_No='" + rolno + "'";
                        DataSet dsexamsub = d2.select_method_wo_parameter(sql, "Text");

                        if (xy >= 1030)
                        {
                            mypdfpage.SaveToDocument();
                            mypdfpage = mydoc.NewPage();
                            xy = 20;
                        }
                        // sem = "select sc.semester from Exam_Details ed,exam_application ea,exam_appl_details ead ,Registration r,subject s,subjectChooser sc,sub_sem sk  where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ead.subject_no=s.subject_no and r.Roll_No=sc.roll_no and sk.subType_no=s.subType_no and sk.syll_code=s.syll_code and sk.subType_no=sc.subtype_no and sc.roll_no=ea.roll_no and sc.subject_no=ead.subject_no and s.subject_no=sc.subject_no  and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "'   and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'and r.Roll_No='" + rolno + "'order by sc.semester desc";
                        sem = "select Current_Semester from Registration where Roll_No='" + rolno + "'";
                        DataSet sd1 = d2.select_method_wo_parameter(sem, "Text");
                        PdfTextArea ptc = new PdfTextArea(Fontbold4, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, 0, xy, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
                        xy = xy + 20;
                        PdfTextArea pts = new PdfTextArea(Fontsmall5, System.Drawing.Color.Black,
                                                           new PdfArea(mydoc, 0, xy, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, affiliated);
                        xy = xy + 20;
                        PdfTextArea pts1 = new PdfTextArea(Fontsmall5, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 0, xy, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, university);
                        xy = xy + 20;
                        PdfTextArea ptc1 = new PdfTextArea(Fontsmall5, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 0, xy, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, address);
                        xy = xy + 20;
                        PdfTextArea pts2 = new PdfTextArea(Fontsmall5, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 0, xy, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "UNIVERSITY EXAMINATION " + ddlMonth.SelectedItem.ToString() + "  " + ddlYear.SelectedItem.ToString() + "");
                        xy = xy + 25;
                        PdfTextArea pts3 = new PdfTextArea(Fontbold4, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 0, xy, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "EXAMINATION HALL TICKET");

                        mypdfpage.Add(ptc);
                        mypdfpage.Add(ptc1);
                        mypdfpage.Add(pts);
                        mypdfpage.Add(pts1);
                        mypdfpage.Add(pts2);
                        mypdfpage.Add(pts3);
                        Gios.Pdf.PdfTable table1 = mydoc.NewTable(Fontboldbig1, 3, 4, 1);
                        //string dob1 = Convert.ToString(dsexamsub.Tables[2].Rows[0]["dobstudent"]);
                        string dob = string.Empty;
                        string semester = string.Empty;
                        if (dsexamsub.Tables[2].Rows.Count > 0 && dsexamsub.Tables.Count > 2)
                        {
                            if (!string.IsNullOrEmpty(dsexamsub.Tables[2].Rows[0]["dobstudent"].ToString()))
                            {
                                dob = Convert.ToString(dsexamsub.Tables[2].Rows[0]["dobstudent"]);
                            }
                        }
                        else
                        {
                            dob = "00/00/0000";
                        }
                        if (sd1.Tables[0].Rows.Count > 0 && sd1.Tables.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(sd1.Tables[0].Rows[0]["Current_Semester"].ToString()))
                            {
                                semester = sd1.Tables[0].Rows[0]["Current_Semester"].ToString();
                            }
                        }
                        else
                        {
                            semester = "nil";
                        }
                        

                        table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        table1.Columns[0].SetWidth(150);
                        table1.Columns[1].SetWidth(180);
                        table1.Columns[2].SetWidth(100);
                        table1.Columns[3].SetWidth(100);
                        table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(0, 0).SetContent("Registration Number");
                        table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(0, 1).SetContent(regno);

                        table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(0, 2).SetContent("Semester");
                        table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(0, 3).SetContent(semester);


                        table1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(1, 0).SetContent("Name");
                        table1.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(1, 1).SetContent(name);

                        table1.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(1, 2).SetContent("Date of Birth");
                        table1.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(1, 3).SetContent(dob);

                        table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(2, 0).SetContent("Degree& Branch");
                        table1.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(2, 1).SetContent(degree + "-" + course);

                        foreach (PdfCell pc in table1.CellRange(2, 1, 2, 1).Cells)
                            pc.ColSpan = 3;
                        xy = 160;
                        Gios.Pdf.PdfTablePage newpdftabpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 50, xy, 700, 500));

                        mypdfpage.Add(newpdftabpage1);
                        mypdfpage.Add(pr1);
                        int subno = 1;
                        int cnt = subno * sno;
                        int cnt1 = subno * 30;

                        int columncout = 5;
                        if (chk_sesdat.Checked == false) //Adeded by aruna 10apr2017
                        {
                            columncout = 3;
                        }
                        Gios.Pdf.PdfTable table = mydoc.NewTable(Fontsmall, dsexamsub.Tables[0].Rows.Count + 1, columncout, 1);
                        table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        if (chk_sesdat.Checked == true)
                        {
                            table.Columns[0].SetWidth(50);
                            table.Columns[1].SetWidth(100);
                            table.Columns[2].SetWidth(100);
                            table.Columns[3].SetWidth(100);
                            table.Columns[4].SetWidth(350);
                            table.CellRange(0, 0, 0, 4).SetFont(Fontsmall);
                            table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 0).SetContent("SI.No");
                            table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 1).SetContent("Date");
                            table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 2).SetContent("Session");
                            table.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 3).SetContent("Sub.Code");
                            table.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 4).SetContent("Subject Title");
                        }
                        else
                        {

                            table.Columns[0].SetWidth(50);
                            table.Columns[1].SetWidth(100);
                            table.Columns[2].SetWidth(350);
                            table.CellRange(0, 0, 0, 2).SetFont(Fontsmall);
                            table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 0).SetContent("SI.No");
                            table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 1).SetContent("Sub.Code");
                            table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 2).SetContent("Subject Title");

                        }

                        int val = 1;

                        if (subno == 1)
                        {
                            int srno1 = 1;
                            string examse = "";
                            string examne = "";
                            for (int i = 0; i < dsexamsub.Tables[0].Rows.Count; i++)
                            {
                                Boolean subjecttype = Convert.ToBoolean(dsexamsub.Tables[0].Rows[i]["lab"].ToString());
                                string roll_no = dsexamsub.Tables[0].Rows[i]["Roll_No"].ToString();
                                string subjectcode = dsexamsub.Tables[0].Rows[i]["subject_code"].ToString();
                                string subjectname = dsexamsub.Tables[0].Rows[i]["subject_name"].ToString();
                                string subt = dsexamsub.Tables[0].Rows[i]["subject_type"].ToString();
                                string sunbbno = dsexamsub.Tables[0].Rows[i]["subject_no"].ToString();
                                examse = " ";
                                examne = " ";
                                dsexamsub.Tables[1].DefaultView.RowFilter = "subject_no='" + sunbbno + "'";
                                DataView dvedate = dsexamsub.Tables[1].DefaultView;
                                if (dvedate.Count > 0)
                                {
                                    examse = dvedate[0]["exam_session"].ToString();
                                    examne = dvedate[0]["edate"].ToString();
                                }

                                if (subjecttype.ToString().Trim().ToLower() == "true" || subjecttype.ToString().Trim() == "1")
                                {
                                    if (chkboxvdate.Checked == false)
                                    {
                                        examse = " ";
                                        examne = " ";
                                    }
                                }
                                else
                                {
                                    if (CheckBox1.Checked == false)
                                    {
                                        examse = " ";
                                        examne = " ";
                                    }
                                }



                                table.Cell(val, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(val, 0).SetContent(srno1);
                                srno1++;

                                if (chk_sesdat.Checked == true)
                                {
                                    table.Cell(val, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table.Cell(val, 1).SetContent(examne);

                                    table.Cell(val, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table.Cell(val, 2).SetContent(examse);

                                    table.Cell(val, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table.Cell(val, 3).SetContent(subjectcode);

                                    table.Cell(val, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table.Cell(val, 4).SetContent(subjectname);
                                    val++;
                                }
                                else
                                {
                                    table.Cell(val, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table.Cell(val, 1).SetContent(subjectcode);

                                    table.Cell(val, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table.Cell(val, 2).SetContent(subjectname);
                                    val++;
                                }

                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))//Aruna
                                {
                                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                    mypdfpage.Add(LogoImage, 30, 25, 300);
                                }

                                MemoryStream memoryStream = new MemoryStream();
                                dshall.Tables[3].DefaultView.RowFilter = "reg_no='" + roll_no + "'";
                                DataView dvphoto = dshall.Tables[3].DefaultView;
                                if (dvphoto.Count > 0)
                                {
                                    string phot = dvphoto[0]["photo"].ToString();

                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll_no + ".jpeg")))
                                    {
                                        if (dvphoto[0]["photo"] != null && dvphoto[0]["photo"].ToString().Trim() != "")
                                        {
                                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll_no + ".jpeg")))
                                            {
                                                byte[] file = (byte[])dvphoto[0]["photo"];
                                                memoryStream.Write(file, 0, file.Length);
                                                if (file.Length > 0)
                                                {
                                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + roll_no + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                }
                                                memoryStream.Dispose();
                                                memoryStream.Close();
                                            }
                                        }
                                    }

                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll_no + ".jpeg")))
                                {
                                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + roll_no + ".jpeg"));
                                    int sd = 18;
                                    mypdfpage.Add(LogoImage, 695, sd, 230);
                                }
                            }


                            xy = 230;

                            Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 50, xy, 700, 1000));
                            mypdfpage.Add(newpdftabpage);
                            xy = 800;
                            PdfTextArea pt123 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, 25, xy, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "_____________________________________________________________________________________________________________________________________");
                            xy = xy + 20;
                            Font Fontsmall8 = new Font("Times New Roman", 14, FontStyle.Regular);
                            PdfTextArea ptc21 = new PdfTextArea(Fontsmall8, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, 30, xy, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "No. of Subjects Registered :" + " " + dsexamsub.Tables[0].Rows.Count);
                            xy = xy + 20;
                            PdfTextArea pt124 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                           new PdfArea(mydoc, 25, xy, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "_____________________________________________________________________________________________________________________________________");
                            xy = xy + 150;
                            PdfTextArea ptc22 = new PdfTextArea(Fontsmall8, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, 30, xy, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Candidate");
                            PdfTextArea ptc23 = new PdfTextArea(Fontsmall8, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, 10, xy, 800, 50), System.Drawing.ContentAlignment.MiddleRight, "Controller of Examinations");
                            xy = xy + 10;
                            PdfTextArea pt125 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 25, xy, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "_____________________________________________________________________________________________________________________________________");
                            xy = xy + 30;
                            PdfTextArea ptc24 = new PdfTextArea(Fontsmall8, System.Drawing.Color.Black,
                                                            new PdfArea(mydoc, 30, xy, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Note: If any discrepancies are found in the Hall Ticket, report to the COE office immediately.");

                            mypdfpage.Add(pt123);
                            mypdfpage.Add(pt124);
                            mypdfpage.Add(pt125);
                            mypdfpage.Add(ptc21);
                            mypdfpage.Add(ptc22);
                            mypdfpage.Add(ptc23);
                            mypdfpage.Add(ptc24);
                        }
                    }
                }

                mypdfpage.SaveToDocument();

                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    //Aruna on 26feb2013============================
                    string szPath = appPath + "/Report/";
                    string szFile = "Format4.pdf";
                    mypdfpage.SaveToDocument();
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                    //=============================================

                }

                // DataSet ds = d2.select_method_wo_parameter(sql, "Text");
            }




        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void btnhallpdf()
    {
        try
        {
            //Font Fontbold = new Font("Times New Roman", 20, FontStyle.Bold);
            //Font Fontsmall = new Font("Times New Roman", 18, FontStyle.Regular);
            //Font Fontbold1 = new Font("Times New Roman", 14, FontStyle.Bold);
            //Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            Rangechk.Visible = true;
            FpSpread2.SaveChanges();
            int selectedcount = 0;
            for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
            {
                int isval = 0;
                string s = FpSpread2.Sheets[0].Cells[res, 6].Text;

                isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);
                if (isval == 1)
                {
                    selectedcount++;
                    errmsg.Text = "";

                }
            }
            if (selectedcount == 0)
            {
                errmsg.Text = "Please Select the Student and then Proceed";
                errmsg.Visible = true;
                return;
            }
            string district = "";
            string state = "";
            Font Fontbold = new Font("Times New Roman", 18, FontStyle.Bold);
            Font Fontsmall = new Font("Times New Roman", 14, FontStyle.Regular);
            Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
            Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);

            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
            DataSet sk = new DataSet();


            if ((ddlMonth.SelectedValue.ToString() != "0") && (ddlYear.SelectedValue.ToString() != "0"))
            {
                FpSpread2.SaveChanges();
                printspread.Sheets[0].ColumnCount = 7;

                for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
                {
                    int isval = 0;

                    string s = FpSpread2.Sheets[0].Cells[res, 6].Text;
                    isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);

                    if (isval == 1)
                    {
                        sno = 0;
                        stdroll = FpSpread2.Sheets[0].Cells[res, 1].Text;

                        if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                        {
                            string college = "select isnull(collname,'') as collname,university,district,state,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code=" + Session["collegecode"] + "";
                            SqlCommand collegecmd = new SqlCommand(college, con);
                            SqlDataReader collegename;
                            con.Close();
                            con.Open();
                            collegename = collegecmd.ExecuteReader();
                            if (collegename.HasRows)
                            {
                                while (collegename.Read())
                                {
                                    affliatedby = collegename["affliated"].ToString();
                                    catgory = collegename["category"].ToString();
                                    collnamenew1 = collegename["collname"].ToString();
                                    address1 = collegename["address1"].ToString();
                                    address3 = collegename["address3"].ToString();
                                    pincode = collegename["pincode"].ToString();
                                    address = address1 + ", " + " " + address3 + "-" + " " + pincode + ".";
                                    university = collegename["university"].ToString();
                                    district = collegename["district"].ToString();
                                    state = collegename["state"].ToString();
                                    //hide by sasi
                                    // catgory = "(An " + catgory + " Institution";
                                    //---end----

                                    //added by sasikumar
                                    //catgory = "An " + catgory + " Institution";
                                    ////------end------

                                    if (RadioButton2.Checked == true)
                                    {
                                        affiliated = "Affiliated  to" + " " + affliatedby;
                                    }
                                    if (RadioButton4.Checked == true)
                                    {
                                        affiliated = affliatedby; // added by jairam 24-03-2015
                                        address = address1 + "-" + " " + pincode + "," + " " + district + "," + " " + state + ".";
                                    }

                                }
                            }
                        }

                        rollnosub = FpSpread2.Sheets[0].Cells[res, 1].Note;
                        exammonth = ddlMonth.SelectedIndex.ToString();
                        exammonthnew = ddlMonth.SelectedItem.Text;
                        examyear = ddlYear.SelectedValue.ToString();
                        exammonthnew1 = monthinwords(exammonthnew);
                        stuname = FpSpread2.Sheets[0].Cells[res, 3].Text;
                        regnumber = FpSpread2.Sheets[0].Cells[res, 2].Text;
                        sem = Session["semforsub"].ToString();
                        degreecode = Session["selecteddegreecode"].ToString();
                        batch = FpSpread2.Sheets[0].Cells[res, 0].Note;
                        degree = Session["selecteddegree"].ToString();
                        course = Session["selectedcourse"].ToString();


                        //Aruna 17apr2013 Add Student Photo===================================================
                        MemoryStream memoryStream = new MemoryStream();
                        SqlCommand cmd = new SqlCommand();
                        imgcon.Close();
                        imgcon.Open();
                        cmd.CommandText = "select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + rollnosub + "')";
                        cmd.Connection = imgcon;

                        SqlDataReader MyReader = cmd.ExecuteReader();
                        if (MyReader.Read())
                        {

                            byte[] file = (byte[])MyReader["photo"];
                            MyReader.Close();
                            memoryStream.Write(file, 0, file.Length);
                            if (file.Length > 0)
                            {
                                //System.Drawing.Image img = System.Drawing.Image.FromStream(memoryStream, true, true);                                                          
                                //img.Save(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg")))
                                {
                                    //image3.ImageUrl = "~/college/" + stdappno + ".jpeg";
                                }
                                else
                                {
                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                }
                            }
                            memoryStream.Dispose();
                            memoryStream.Close();
                            MyReader.Close();
                        }
                        //Aruna 17apr2013 Add Student Photo===================================================



                        //aruna 09nov2013 exam_code = GetFunction("select distinct exam_code from exmtt where degree_code=" + degreecode + " and exam_month=" + exammonth + " and exam_year=" + examyear + " and batchfrom=" + batch + " and semester=" + sem + " ");
                        exam_code = GetFunction("select distinct exam_code from exmtt where degree_code=" + degreecode + " and exam_month=" + exammonth + " and exam_year=" + examyear + " and batchfrom=" + batch + "");

                        string dateofbirth = "select convert(varchar(20),a.dob,103) as dobstudent from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollnosub + "'";
                        SqlDataAdapter da3 = new SqlDataAdapter(dateofbirth, con2);

                        DataSet ds3 = new DataSet();
                        da3.Fill(ds3);
                        con2.Close();
                        con2.Open();
                        if (ds3.Tables[0].Rows.Count > 0)
                        {
                            dob = ds3.Tables[0].Rows[0]["dobstudent"].ToString();
                        }



                        // hide by sasikumar
                        //string time = "";
                        //string time1 = "";

                        //added by sasikumar
                        string examse = "";
                        //end

                        string subject_nofromexmtt = "";

                        Boolean subjecttype = false;

                        if (exam_code != "")
                        {
                            string subjectquery = "select distinct isnull(s.Subject_Code,'') as scode , isnull(s.subjecT_name,'') as sname , isnull(ed.subject_no,'') as subno, semester as ssem,ed.start_time,ed.end_time,ed.exam_date,ed.exam_session,feesub.lab from exmtt_det ed,exmtt e,subject s,sub_sem feesub  where feesub.subtype_no=s.subtype_no and s.subject_no=ed.subject_no and e.exam_code=ed.exam_code and e.exam_code=" + exam_code + "  order by exam_date asc,exam_session desc";
                            if (chksupplym.Checked == true)
                            {
                                subjectquery = "select distinct isnull(s.Subject_Code,'') as scode , isnull(s.subjecT_name,'') as sname , isnull(ed.subject_no,'') as subno, semester as ssem,ed.start_time,ed.end_time,ed.exam_date,ed.exam_session,feesub.lab from exmtt_det ed,exmtt e,subject s,sub_sem feesub  where feesub.subtype_no=s.subtype_no and s.subject_no=ed.subject_no and e.exam_code=ed.exam_code and e.exam_code=" + exam_code + "  and s.subject_no  in ( select ea.subject_no   from Exam_Details ed,exam_appl_details ea,exam_application e, subject s, syllabus_master sy,sub_sem su where ed.exam_code =e.exam_code  and e.appl_no =ea.appl_no                            and  s.subject_no =ea.subject_no   and  su.syll_code =sy.syll_code and su.subType_no =s.subType_no                              and  sy.syll_code =s.syll_code and e.roll_no ='" + rollnosub + "' and e.Exam_type=4 and ed.Exam_Month='" + exammonth + "'  and ed.Exam_year='" + examyear + "') order by exam_date asc, exam_session desc";
                            }
                            // string subjectquery = "select distinct isnull(s.Subject_Code,'') as scode , isnull(s.subjecT_name,'') as sname , isnull(ed.subject_no,'') as subno, semester as ssem,ed.start_time,ed.end_time,ed.exam_date,ed.exam_session,feesub.lab from exmtt_det ed,exmtt e,subject s,sub_sem feesub  where feesub.subtype_no=s.subtype_no and s.subject_no=ed.subject_no and e.exam_code=ed.exam_code and e.degree_code=" + degreecode + " and e.exam_month=" + exammonth + " and e.exam_year=" + examyear + " order by exam_date asc,exam_session desc";
                            SqlDataAdapter da15 = new SqlDataAdapter(subjectquery, con1);
                            DataSet ds15 = new DataSet();
                            da15.Fill(ds15);
                            con1.Close();
                            con1.Open();

                            if (ds15.Tables[0].Rows.Count > 0)
                            {
                                mc = ds15.Tables[0].Rows.Count;
                                for (int i4 = 0; i4 < ds15.Tables[0].Rows.Count; i4++)
                                {
                                    //sno++;
                                    subjnos++;

                                    subject_nofromexmtt = ds15.Tables[0].Rows[i4]["subno"].ToString();
                                    subjecttype = Convert.ToBoolean(ds15.Tables[0].Rows[i4]["lab"]);
                                    string mnt = ddlMonth.SelectedItem.Value;

                                    if (chksupplym.Checked == true)
                                    {
                                        string examinforegular = "select distinct isnull(Subject_Code,'') as subcode , isnull(subjecT_name,'') as subname ,sc.subject_no as subject_no, semester as ssem,e.start_time,e.end_time,e.exam_date,e.exam_session,feesub.lab from subjectchooser sc,subject s,sub_sem as feesub,exmtt_det as e where feesub.subtype_no=s.subtype_no and s.subject_no=e.subject_no and  feesub.syll_code=s.syll_code and feesub.promote_count=1 and s.subject_no=sc.subject_no and s.subject_no=" + subject_nofromexmtt + " and s.subtype_no=sc.subtype_no and roll_no='" + rollnosub + "' and semester=" + Session["semforsub"].ToString() + " order by exam_date";

                                        // string examinforegular = "select distinct isnull(Subject_Code,'') as subcode , isnull(subjecT_name,'') as subname ,sc.subject_no as subject_no, semester as ssem,e.start_time,e.end_time,e.exam_date,e.exam_session,feesub.lab from subjectchooser sc,subject s,sub_sem as feesub,exmtt_det as e where feesub.subtype_no=s.subtype_no and s.subject_no=e.subject_no and  feesub.syll_code=s.syll_code and feesub.promote_count=1 and s.subject_no=sc.subject_no and s.subject_no=" + subject_nofromexmtt + " and s.subtype_no=sc.subtype_no and roll_no='" + rollnosub + "' and semester=" + Session["semforsub"].ToString() + " order by exam_date";
                                        SqlDataAdapter da4 = new SqlDataAdapter(examinforegular, con3);
                                        DataSet ds4 = new DataSet();
                                        da4.Fill(ds4);
                                        con3.Close();
                                        con3.Open();


                                        if (ds4.Tables[0].Rows.Count > 0)
                                        {

                                            for (int i = 0; i < ds4.Tables[0].Rows.Count; i++)
                                            {
                                                sno++;
                                                exam_date = ds4.Tables[0].Rows[i]["exam_date"].ToString();
                                                DateTime obtaineddate = Convert.ToDateTime(exam_date);

                                                if (subject_code == "")
                                                {
                                                    subject_code = ds4.Tables[0].Rows[i]["subcode"].ToString();
                                                    subject_name = ds4.Tables[0].Rows[i]["subname"].ToString();
                                                    examdatenew = obtaineddate.ToString("dd/MM/yyyy");

                                                    exam_session = ds4.Tables[0].Rows[0]["exam_session"].ToString();
                                                    //added by sasikumar
                                                    examse = exam_session;
                                                    if (chkboxvdate.Checked == false && CheckBox1.Checked == false)
                                                    {


                                                        examdatenew = "";
                                                        exam_session = "";
                                                        examse = "";

                                                    }
                                                    else if (chkboxvdate.Checked == true && CheckBox1.Checked == true)
                                                    {

                                                    }
                                                    else if (chkboxvdate.Checked == true && CheckBox1.Checked == false)
                                                    {
                                                        if (subjecttype == false)
                                                        {
                                                            examdatenew = "";
                                                            exam_session = "";
                                                            examse = "";
                                                        }
                                                    }
                                                    else if (chkboxvdate.Checked == false && CheckBox1.Checked == true)
                                                    {
                                                        if (subjecttype == true)
                                                        {
                                                            examdatenew = "";
                                                            exam_session = "";
                                                            examse = "";
                                                        }
                                                    }

                                                    srno = sno.ToString();
                                                }
                                                else
                                                {
                                                    subject_code = subject_code + '\n' + ds4.Tables[0].Rows[i]["subcode"].ToString();
                                                    subject_name = subject_name + '\n' + ds4.Tables[0].Rows[i]["subname"].ToString();
                                                    string gf = obtaineddate.ToString("dd/MM/yyyy");
                                                    string gff = ds4.Tables[0].Rows[i]["exam_session"].ToString();
                                                    // added by sasi
                                                    examse = gff;
                                                    //---end---
                                                    if (chkboxvdate.Checked == false)
                                                    {
                                                        if (subjecttype == true)
                                                        {
                                                            gf = "";
                                                            gff = "";
                                                            examse = "";
                                                        }
                                                    }

                                                    if (chkboxvdate.Checked == false && CheckBox1.Checked == false)
                                                    {


                                                        gf = "";
                                                        gff = "";
                                                        examse = "";

                                                    }
                                                    else if (chkboxvdate.Checked == true && CheckBox1.Checked == true)
                                                    {

                                                    }
                                                    else if (chkboxvdate.Checked == true && CheckBox1.Checked == false)
                                                    {
                                                        if (subjecttype == false)
                                                        {
                                                            gf = "";
                                                            gff = "";
                                                            examse = "";
                                                        }
                                                    }
                                                    else if (chkboxvdate.Checked == false && CheckBox1.Checked == true)
                                                    {
                                                        if (subjecttype == true)
                                                        {
                                                            gf = "";
                                                            gff = "";
                                                            examse = "";
                                                        }
                                                    }
                                                    examdatenew = examdatenew + '\n' + gf;
                                                    exam_session = exam_session + '\n' + gff;
                                                    srno = srno + '\n' + sno.ToString();

                                                }
                                            }
                                        }
                                        goto supplyformate_ii;
                                    }

                                    if (CheckArrear.Checked == true)
                                    {

                                        string Arrearsub = "select ed.Exam_Month,ed.Exam_year,r.Reg_No,r.Roll_No,r.Stud_Name,sc.semester,s.subject_code,s.subject_name ,s.subject_no,ead.attempts  from Exam_Details ed,exam_application ea,exam_appl_details ead ,Registration r,subject s,subjectChooser sc  where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ead.subject_no=s.subject_no and r.Roll_No=sc.roll_no and sc.roll_no=ea.roll_no and sc.subject_no=ead.subject_no and s.subject_no=sc.subject_no and ed.Exam_year='2015' and ed.Exam_Month='" + mnt + "' and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'and r.Roll_No='" + rollnosub + "'  and ead.attempts > 0  order by r.Reg_No,sc.semester desc, s.subject_code ";

                                        if (chkpassout.Checked == true)
                                        {
                                            Arrearsub = "select ed.Exam_Month,ed.Exam_year,r.Reg_No,r.Roll_No,r.Stud_Name,sc.semester,s.subject_code,s.subject_name ,s.subject_no,ead.attempts  from Exam_Details ed,exam_application ea,exam_appl_details ead ,Registration r,subject s,subjectChooser sc  where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ead.subject_no=s.subject_no and r.Roll_No=sc.roll_no and sc.roll_no=ea.roll_no and sc.subject_no=ead.subject_no and s.subject_no=sc.subject_no and ed.Exam_year='2015' and ed.Exam_Month='" + mnt + "' and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'and r.Roll_No='" + rollnosub + "'  and ead.attempts > 0  order by r.Reg_No,sc.semester desc, s.subject_code ";
                                        }

                                        SqlDataAdapter da8 = new SqlDataAdapter(Arrearsub, con3);
                                        DataSet ds8 = new DataSet();
                                        da8.Fill(ds8);
                                        con3.Close();
                                        con3.Open();
                                        if (ds8.Tables[0].Rows.Count > 0)
                                        {

                                            for (int i = 0; i < ds8.Tables[0].Rows.Count; i++)
                                            {
                                                sno++;
                                                //exam_date = ds8.Tables[0].Rows[i]["exam_date"].ToString();

                                                //DateTime obtaineddate = Convert.ToDateTime(exam_date);
                                                if (subject_code == "")
                                                {
                                                    subject_code = ds8.Tables[0].Rows[i]["subject_code"].ToString();
                                                    subject_name = ds8.Tables[0].Rows[i]["subject_name"].ToString();
                                                    //examdatenew = obtaineddate.ToString("dd/MM/yyyy");
                                                    //exam_session = ds8.Tables[0].Rows[i]["exam_session"].ToString();
                                                    examse = exam_session;
                                                    if (chkboxvdate.Checked == false)
                                                    {
                                                        // hide by sasikumar
                                                        //examdatenew = "";
                                                        //exam_session = "";
                                                        if (subjecttype == true)
                                                        {
                                                            examdatenew = "";
                                                            //exam_session = "";
                                                            examse = "";
                                                        }
                                                    }
                                                    if (chkboxvdate.Checked == false && CheckBox1.Checked == false)
                                                    {


                                                        examdatenew = "";
                                                        exam_session = "";
                                                        examse = "";

                                                    }
                                                    else if (chkboxvdate.Checked == true && CheckBox1.Checked == true)
                                                    {

                                                    }
                                                    else if (chkboxvdate.Checked == true && CheckBox1.Checked == false)
                                                    {
                                                        if (subjecttype == false)
                                                        {
                                                            examdatenew = "";
                                                            exam_session = "";
                                                            examse = "";
                                                        }
                                                    }
                                                    else if (chkboxvdate.Checked == false && CheckBox1.Checked == true)
                                                    {
                                                        if (subjecttype == true)
                                                        {
                                                            examdatenew = "";
                                                            exam_session = "";
                                                            examse = "";
                                                        }
                                                    }

                                                    srno = sno.ToString();
                                                }
                                                else
                                                {
                                                    subject_code = subject_code + '\n' + ds8.Tables[0].Rows[i]["subject_code"].ToString();
                                                    subject_name = subject_name + '\n' + ds8.Tables[0].Rows[i]["subject_name"].ToString();
                                                    //examdatenew = examdatenew + '\n' +  obtaineddate.ToString("dd/MM/yyyy");
                                                    //exam_session = exam_session + '\n' + ds8.Tables[0].Rows[i]["exam_session"].ToString();

                                                    //string gf = obtaineddate.ToString("dd/MM/yyyy");
                                                    //string gff = ds8.Tables[0].Rows[i]["exam_session"].ToString();
                                                    //examse = gff;
                                                    if (chkboxvdate.Checked == false)
                                                    {
                                                        if (subjecttype == true)
                                                        {
                                                            //gf = "";
                                                            //gff = "";
                                                            examse = "";
                                                        }
                                                    }

                                                    if (chkboxvdate.Checked == false && CheckBox1.Checked == false)
                                                    {


                                                        //gf = "";
                                                        //gff = "";
                                                        examse = "";

                                                    }
                                                    else if (chkboxvdate.Checked == true && CheckBox1.Checked == true)
                                                    {

                                                    }
                                                    else if (chkboxvdate.Checked == true && CheckBox1.Checked == false)
                                                    {
                                                        if (subjecttype == false)
                                                        {
                                                            //gf = "";
                                                            //gff = "";
                                                            examse = "";
                                                        }
                                                    }
                                                    else if (chkboxvdate.Checked == false && CheckBox1.Checked == true)
                                                    {
                                                        if (subjecttype == true)
                                                        {
                                                            //gf = "";
                                                            //gff = "";
                                                            examse = "";
                                                        }
                                                    }

                                                    //examdatenew = examdatenew + '\n' + gf;
                                                    //exam_session = exam_session + '\n' + gff;
                                                    srno = srno + '\n' + sno.ToString();
                                                }
                                            }
                                        }

                                    }
                                    if (CheckRegular.Checked == true)
                                    {
                                        string examinforegular = "select distinct isnull(Subject_Code,'') as subcode , isnull(subjecT_name,'') as subname ,sc.subject_no as subject_no, semester as ssem,e.start_time,e.end_time,e.exam_date,e.exam_session,feesub.lab from subjectchooser sc,subject s,sub_sem as feesub,exmtt_det as e where feesub.subtype_no=s.subtype_no and s.subject_no=e.subject_no and  feesub.syll_code=s.syll_code and feesub.promote_count=1 and s.subject_no=sc.subject_no and s.subject_no=" + subject_nofromexmtt + " and s.subtype_no=sc.subtype_no and roll_no='" + rollnosub + "' and semester=" + Session["semforsub"].ToString() + " order by exam_date";

                                        // string examinforegular = "select distinct isnull(Subject_Code,'') as subcode , isnull(subjecT_name,'') as subname ,sc.subject_no as subject_no, semester as ssem,e.start_time,e.end_time,e.exam_date,e.exam_session,feesub.lab from subjectchooser sc,subject s,sub_sem as feesub,exmtt_det as e where feesub.subtype_no=s.subtype_no and s.subject_no=e.subject_no and  feesub.syll_code=s.syll_code and feesub.promote_count=1 and s.subject_no=sc.subject_no and s.subject_no=" + subject_nofromexmtt + " and s.subtype_no=sc.subtype_no and roll_no='" + rollnosub + "' and semester=" + Session["semforsub"].ToString() + " order by exam_date";
                                        SqlDataAdapter da4 = new SqlDataAdapter(examinforegular, con3);
                                        DataSet ds4 = new DataSet();
                                        da4.Fill(ds4);
                                        con3.Close();
                                        con3.Open();


                                        if (ds4.Tables[0].Rows.Count > 0)
                                        {

                                            for (int i = 0; i < ds4.Tables[0].Rows.Count; i++)
                                            {
                                                sno++;
                                                exam_date = ds4.Tables[0].Rows[i]["exam_date"].ToString();
                                                DateTime obtaineddate = Convert.ToDateTime(exam_date);

                                                if (subject_code == "")
                                                {
                                                    subject_code = ds4.Tables[0].Rows[i]["subcode"].ToString();
                                                    subject_name = ds4.Tables[0].Rows[i]["subname"].ToString();
                                                    examdatenew = obtaineddate.ToString("dd/MM/yyyy");

                                                    exam_session = ds4.Tables[0].Rows[0]["exam_session"].ToString();
                                                    //added by sasikumar
                                                    examse = exam_session;
                                                    if (chkboxvdate.Checked == false)
                                                    {
                                                        //hide by sasikumar
                                                        //examdatenew = "";
                                                        //exam_session = "";
                                                        if (subjecttype == true)
                                                        {
                                                            examdatenew = "";
                                                            exam_session = "";
                                                            examse = "";
                                                        }
                                                    }

                                                    if (chkboxvdate.Checked == false && CheckBox1.Checked == false)
                                                    {


                                                        examdatenew = "";
                                                        exam_session = "";
                                                        examse = "";

                                                    }
                                                    else if (chkboxvdate.Checked == true && CheckBox1.Checked == true)
                                                    {

                                                    }
                                                    else if (chkboxvdate.Checked == true && CheckBox1.Checked == false)
                                                    {
                                                        if (subjecttype == false)
                                                        {
                                                            examdatenew = "";
                                                            exam_session = "";
                                                            examse = "";
                                                        }
                                                    }
                                                    else if (chkboxvdate.Checked == false && CheckBox1.Checked == true)
                                                    {
                                                        if (subjecttype == true)
                                                        {
                                                            examdatenew = "";
                                                            exam_session = "";
                                                            examse = "";
                                                        }
                                                    }

                                                    srno = sno.ToString();
                                                }
                                                else
                                                {
                                                    subject_code = subject_code + '\n' + ds4.Tables[0].Rows[i]["subcode"].ToString();
                                                    subject_name = subject_name + '\n' + ds4.Tables[0].Rows[i]["subname"].ToString();
                                                    string gf = obtaineddate.ToString("dd/MM/yyyy");
                                                    string gff = ds4.Tables[0].Rows[i]["exam_session"].ToString();
                                                    // added by sasi
                                                    examse = gff;
                                                    //---end---
                                                    if (chkboxvdate.Checked == false)
                                                    {
                                                        if (subjecttype == true)
                                                        {
                                                            gf = "";
                                                            gff = "";
                                                            examse = "";
                                                        }
                                                    }

                                                    if (chkboxvdate.Checked == false && CheckBox1.Checked == false)
                                                    {


                                                        gf = "";
                                                        gff = "";
                                                        examse = "";

                                                    }
                                                    else if (chkboxvdate.Checked == true && CheckBox1.Checked == true)
                                                    {

                                                    }
                                                    else if (chkboxvdate.Checked == true && CheckBox1.Checked == false)
                                                    {
                                                        if (subjecttype == false)
                                                        {
                                                            gf = "";
                                                            gff = "";
                                                            examse = "";
                                                        }
                                                    }
                                                    else if (chkboxvdate.Checked == false && CheckBox1.Checked == true)
                                                    {
                                                        if (subjecttype == true)
                                                        {
                                                            gf = "";
                                                            gff = "";
                                                            examse = "";
                                                        }
                                                    }
                                                    examdatenew = examdatenew + '\n' + gf;
                                                    exam_session = exam_session + '\n' + gff;
                                                    srno = srno + '\n' + sno.ToString();

                                                }
                                            }
                                        }
                                    }
                                supplyformate_ii:
                                    //for regular
                                    exammonth = ddlMonth.SelectedIndex.ToString();
                                    //added by sasi
                                    if (examse == "F.N")
                                    {
                                        //end
                                        //for FN

                                        string fntime = "select distinct convert(Varchar(8),ex.start_time,108) as start,convert(Varchar(8),ex.end_time,108) as end1 from exmtt e,exmtt_det ex  where ex.exam_session='F.N' and ex.start_time<> ex.end_time  and e.exam_code=" + exam_code + "";
                                        SqlDataAdapter da7 = new SqlDataAdapter(fntime, con1);

                                        string start_time1 = "";
                                        string end_time1 = "";
                                        DataSet ds7 = new DataSet();
                                        da7.Fill(ds7);
                                        con1.Close();
                                        con1.Open();

                                        if (ds7.Tables[0].Rows.Count > 0)
                                        {
                                            start_time1 = ds7.Tables[0].Rows[0]["start"].ToString();
                                            end_time1 = ds7.Tables[0].Rows[0]["end1"].ToString();
                                            if ((start_time1 != "") && (end_time1 != ""))
                                            {
                                                string[] splitdate = start_time1.Split(new Char[] { ':' });
                                                string starthour2 = splitdate[0].ToString();
                                                string startmin2 = splitdate[1].ToString();
                                                string startsec2 = splitdate[2].ToString();
                                                start_time1 = starthour2 + "." + startmin2;
                                                string[] splitdate1 = end_time1.Split(new Char[] { ':' });
                                                string endhour2 = splitdate1[0].ToString();
                                                string endmin2 = splitdate1[1].ToString();
                                                string endsec2 = splitdate1[2].ToString();

                                                if (Convert.ToInt32(endhour2) > 12)
                                                {
                                                    endhour2 = Convert.ToString(railwaytime(Convert.ToInt32(endhour2)));
                                                }
                                                end_time1 = endhour2 + "." + endmin2;
                                                time = "FN" + " " + "-" + " " + "Forenoon" + " " + start_time1 + " " + "a.m" + " " + "-" + " " + end_time1 + " " + "p.m";
                                            }
                                        }
                                    }
                                    //for AN
                                    //added by sasi
                                    if (examse == "A.N")
                                    {
                                        //end
                                        string antime = "select distinct convert(Varchar(8),ex.start_time,108) as start,convert(Varchar(8),ex.end_time,108) as end1 from exmtt e,exmtt_det ex  where ex.exam_session='A.N' and ex.start_time<> ex.end_time and e.exam_code=" + exam_code + " ";
                                        SqlDataAdapter da6 = new SqlDataAdapter(antime, con1);
                                        string start_time = "";
                                        string end_time = "";
                                        DataSet ds6 = new DataSet();
                                        da6.Fill(ds6);
                                        con1.Close();
                                        con1.Open();
                                        if (ds6.Tables[0].Rows.Count > 0)
                                        {
                                            start_time = ds6.Tables[0].Rows[0]["start"].ToString();
                                            end_time = ds6.Tables[0].Rows[0]["end1"].ToString();
                                            if ((start_time != "") && (end_time != ""))
                                            {
                                                string[] splitdate = start_time.Split(new Char[] { ':' });
                                                string starthour1 = splitdate[0].ToString();
                                                string startmin1 = splitdate[1].ToString();
                                                string startsec1 = splitdate[2].ToString();

                                                string[] splitdate1 = end_time.Split(new Char[] { ':' });
                                                string endhour1 = splitdate1[0].ToString();
                                                string endmin1 = splitdate1[1].ToString();
                                                string endsec1 = splitdate1[2].ToString();

                                                if (Convert.ToInt32(starthour1) > 12)
                                                {
                                                    starthour1 = Convert.ToString(railwaytime(Convert.ToInt32(starthour1)));
                                                }
                                                if (Convert.ToInt32(endhour1) > 12)
                                                {
                                                    endhour1 = Convert.ToString(railwaytime(Convert.ToInt32(endhour1)));
                                                }
                                                start_time = starthour1 + "." + startmin1;
                                                end_time = endhour1 + "." + endmin1;
                                                time1 = "AN" + " " + "-" + " " + "Afternoon" + " " + start_time + " " + "p.m" + " " + "-" + " " + end_time + " " + "p.m";
                                            }
                                        }
                                    }
                                    examse = "";
                                }
                            }

                            Bindpdf(mydoc, Fontsmall, Fontbold, Fontbold1, ds15.Tables[0], Response);
                            // tempvar = true;

                        }
                        FpSpread2.Sheets[0].Cells[res, 6].Value = 0;
                    }
                    FpSpread2.SaveChanges();
                }



            }
        }
        catch
        {



        }
    }

    public void Bindpdf(Gios.Pdf.PdfDocument mydoc, Font Fontsmall, Font Fontbold, Font Fontbold1, DataTable dt, HttpResponse response)
    {
        try
        {
            int subno = 0;
            int pagecount = sno / 30;
            int repage = sno % 30;

            int nopages = pagecount;
            if (repage > 0)
            {
                nopages++;
            }
            if (nopages > 0)
            {
                for (int row = 0; row < nopages; row++)
                {
                    subno++;

                    Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();

                    if (RadioButton2.Checked == true)
                    {

                        PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                            new PdfArea(mydoc, 0, 20, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, collnamenew1);
                        PdfTextArea ptc1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                           new PdfArea(mydoc, 0, 40, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, address);// added by sridhar 11 sep 2014
                        PdfTextArea pts = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                           new PdfArea(mydoc, 0, 60, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "(" + catgory + " - " + affiliated + ")");

                        PdfTextArea pts1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 0, 80, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "Office of the Controller of Examinations");

                        PdfTextArea pts2 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 0, 100, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "UG/PG Degree End Semester Examinations " + ddlMonth.SelectedItem.ToString() + "  " + ddlYear.SelectedItem.ToString() + "");

                        PdfTextArea pts3 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 0, 120, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "HALL TICKET");

                        mypdfpage.Add(ptc);
                        mypdfpage.Add(ptc1);
                        mypdfpage.Add(pts);
                        mypdfpage.Add(pts1);
                        mypdfpage.Add(pts2);
                        mypdfpage.Add(pts3);
                    }
                    if (RadioButton4.Checked == true)
                    {
                        PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                           new PdfArea(mydoc, 0, 20, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, collnamenew1);

                        PdfTextArea pts = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                           new PdfArea(mydoc, 0, 40, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, affiliated);

                        PdfTextArea pts1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 0, 60, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, university);

                        PdfTextArea ptc1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 0, 80, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, address);

                        PdfTextArea pts2 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 0, 100, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "UNIVERSITY EXAMINATION " + ddlMonth.SelectedItem.ToString() + "  " + ddlYear.SelectedItem.ToString() + "");

                        PdfTextArea pts3 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 0, 125, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "EXAMINATION HALL TICKET");

                        mypdfpage.Add(ptc);
                        mypdfpage.Add(ptc1);
                        mypdfpage.Add(pts);
                        mypdfpage.Add(pts1);
                        mypdfpage.Add(pts2);
                        mypdfpage.Add(pts3);
                    }


                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))//Aruna
                    {
                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                        mypdfpage.Add(LogoImage, 30, 25, 300);
                    }

                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg")))//Aruna
                    {
                        PdfImage leftimage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg"));
                        mypdfpage.Add(leftimage, 685, 25, 300);
                    }


                    PdfArea tete = new PdfArea(mydoc, 25, 10, 800, 1100);

                    PdfRectangle pr1 = new PdfRectangle(mydoc, tete, Color.Black);






                    Gios.Pdf.PdfTable table1 = mydoc.NewTable(Fontsmall, 3, 4, 1);

                    //  table1.HeadersRow.SetColors(Color.White, Color.Navy);
                    // table1.SetColors(Color.Black, Color.White, Color.Gainsboro);
                    table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);




                    table1.Columns[0].SetWidth(150);
                    table1.Columns[1].SetWidth(180);
                    table1.Columns[2].SetWidth(100);
                    table1.Columns[3].SetWidth(100);


                    table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1.Cell(0, 0).SetContent("Registration Number");
                    table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1.Cell(0, 1).SetContent(regnumber);

                    table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1.Cell(0, 2).SetContent("Semester");
                    table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1.Cell(0, 3).SetContent(sem);


                    table1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1.Cell(1, 0).SetContent("Name");
                    table1.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1.Cell(1, 1).SetContent(stuname);

                    table1.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1.Cell(1, 2).SetContent("Date of Birth");
                    table1.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1.Cell(1, 3).SetContent(dob);



                    table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1.Cell(2, 0).SetContent("Degree& Branch");
                    table1.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1.Cell(2, 1).SetContent(degree + "-" + course);



                    //added by sasikumar

                    foreach (PdfCell pc in table1.CellRange(2, 1, 2, 1).Cells)
                        pc.ColSpan = 3;



                    Gios.Pdf.PdfTablePage newpdftabpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 50, 160, 700, 500));


                    mypdfpage.Add(newpdftabpage1);
                    mypdfpage.Add(pr1);

                    int cnt = subno * sno;
                    int cnt1 = subno * 30;


                    Gios.Pdf.PdfTable table = mydoc.NewTable(Fontsmall, cnt + 1, 5, 1);
                    table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                    table.Columns[0].SetWidth(50);
                    table.Columns[1].SetWidth(100);
                    table.Columns[2].SetWidth(100);
                    table.Columns[3].SetWidth(100);
                    table.Columns[4].SetWidth(350);

                    //table.CellRange(0, 0, 0, 4).SetFont(Fontbold);
                    table.CellRange(0, 0, 0, 4).SetFont(Fontsmall);
                    table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(0, 0).SetContent("SI.No");
                    table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(0, 1).SetContent("Date");
                    table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(0, 2).SetContent("Session");

                    table.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(0, 3).SetContent("Sub.Code");
                    table.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(0, 4).SetContent("Subject Title");
                    int val = 0;
                    if (subno == 1)
                    {
                        if (cnt < 30)
                        {
                            for (int i = 0; i < cnt; i++)
                            {
                                val++;
                                table.Cell(val, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] splitsrno = srno.Split(new Char[] { '\n' });
                                srno1 = splitsrno[i];
                                table.Cell(val, 0).SetContent(srno1);


                                table.Cell(val, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] splitexamdatenew = examdatenew.Split(new Char[] { '\n' });
                                examne = splitexamdatenew[i];
                                table.Cell(val, 1).SetContent(examne);


                                table.Cell(val, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] splitexamsession = exam_session.Split(new Char[] { '\n' });
                                examse = splitexamsession[i];
                                table.Cell(val, 2).SetContent(examse);

                                table.Cell(val, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] splitsubjecode = subject_code.Split(new Char[] { '\n' });
                                subjeccode = splitsubjecode[i];
                                table.Cell(val, 3).SetContent(subjeccode);

                                table.Cell(val, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                string[] splitsubjectname = subject_name.Split(new Char[] { '\n' });
                                subjname = splitsubjectname[i];
                                table.Cell(val, 4).SetContent(subjname);
                                if (examne == "")
                                {

                                    //foreach (PdfCell pc in table1.CellRange(2, 1, 2, 1).Cells)
                                    //    pc.ColSpan = 3;

                                    //added by sasikumar

                                    foreach (PdfCell pc in table.CellRange(val, 1, val, 1).Cells)
                                        pc.ColSpan = 2;
                                    //---end-----
                                    //foreach (PdfCell pc in table.CellRange(val, 1, val, 3).Cells)
                                    //    pc.ColSpan = 1;
                                }
                            }

                        }
                        else
                        {
                            for (int i = 0; i < cnt1; i++)
                            {
                                val++;
                                table.Cell(val, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] splitsrno = srno.Split(new Char[] { '\n' });
                                srno1 = splitsrno[i];
                                table.Cell(val, 0).SetContent(srno1);


                                table.Cell(val, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] splitexamdatenew = examdatenew.Split(new Char[] { '\n' });
                                examne = splitexamdatenew[i];
                                table.Cell(val, 1).SetContent(examne);


                                table.Cell(val, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] splitexamsession = exam_session.Split(new Char[] { '\n' });
                                examse = splitexamsession[i];
                                table.Cell(val, 2).SetContent(examse);

                                table.Cell(val, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] splitsubjecode = subject_code.Split(new Char[] { '\n' });
                                subjeccode = splitsubjecode[i];
                                table.Cell(val, 3).SetContent(subjeccode);

                                table.Cell(val, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                string[] splitsubjectname = subject_name.Split(new Char[] { '\n' });
                                subjname = splitsubjectname[i];
                                table.Cell(val, 4).SetContent(subjname);
                                if (examne == "")
                                {
                                    //added by sasi kumar----
                                    foreach (PdfCell pc in table.CellRange(val, 1, val, 1).Cells)
                                        pc.ColSpan = 2;


                                    //foreach (PdfCell pc in table.CellRange(val, 1, val, 2).Cells)
                                    //    pc.ColSpan = 2;
                                }
                            }
                        }
                    }

                    if (subno > 1)
                    {
                        val = (subno - 1) * 30;
                        int ro = 0;

                        int remaindsubs = sno - val;

                        if (remaindsubs < 30)
                        {

                            table = mydoc.NewTable(Fontsmall, remaindsubs + 1, 5, 1);
                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                            table.Columns[0].SetWidth(50);
                            table.Columns[1].SetWidth(100);
                            table.Columns[2].SetWidth(100);
                            table.Columns[3].SetWidth(100);
                            table.Columns[4].SetWidth(350);

                            table.CellRange(0, 0, 0, 4).SetFont(Fontsmall);
                            table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 0).SetContent("SI.No");
                            table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 1).SetContent("Date");
                            table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 2).SetContent("Session");

                            table.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 3).SetContent("Sub.Code");
                            table.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 4).SetContent("Subject Title");

                            for (int fg = 0; fg < remaindsubs; fg++)
                            {

                                ro++;
                                table.Cell(ro, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] splitsrno = srno.Split(new Char[] { '\n' });
                                srno1 = splitsrno[val];
                                table.Cell(ro, 0).SetContent(srno1);


                                table.Cell(ro, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] splitexamdatenew = examdatenew.Split(new Char[] { '\n' });
                                examne = splitexamdatenew[val];
                                table.Cell(ro, 1).SetContent(examne);


                                table.Cell(ro, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] splitexamsession = exam_session.Split(new Char[] { '\n' });
                                examse = splitexamsession[val];
                                table.Cell(ro, 2).SetContent(examse);

                                table.Cell(ro, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] splitsubjecode = subject_code.Split(new Char[] { '\n' });
                                subjeccode = splitsubjecode[val];
                                table.Cell(ro, 3).SetContent(subjeccode);

                                table.Cell(ro, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                string[] splitsubjectname = subject_name.Split(new Char[] { '\n' });
                                subjname = splitsubjectname[val];
                                table.Cell(ro, 4).SetContent(subjname);
                                if (examne == "")
                                {
                                    //added by sasikumar
                                    foreach (PdfCell pc in table.CellRange(ro, 1, ro, 1).Cells)
                                        pc.ColSpan = 2;
                                    //foreach (PdfCell pc in table.CellRange(ro, 1, ro, 3).Cells)
                                    //    pc.ColSpan = 2;
                                }

                                val++;
                            }
                        }
                        else
                        {
                            table = mydoc.NewTable(Fontsmall, 20 + 1, 5, 1);
                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                            table.Columns[0].SetWidth(50);
                            table.Columns[1].SetWidth(100);
                            table.Columns[2].SetWidth(100);
                            table.Columns[3].SetWidth(100);
                            table.Columns[4].SetWidth(350);

                            table.CellRange(0, 0, 0, 4).SetFont(Fontsmall);
                            table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 0).SetContent("SI.No");
                            table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 1).SetContent("Date");
                            table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 2).SetContent("Session");

                            table.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 3).SetContent("Sub.Code");
                            table.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 4).SetContent("Subject Title");

                            for (int fg = 0; fg < 30; fg++)
                            {
                                ro++;
                                table.Cell(ro, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] splitsrno = srno.Split(new Char[] { '\n' });
                                srno1 = splitsrno[val];
                                table.Cell(ro, 0).SetContent(srno1);


                                table.Cell(ro, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] splitexamdatenew = examdatenew.Split(new Char[] { '\n' });
                                examne = splitexamdatenew[val];
                                table.Cell(ro, 1).SetContent(examne);


                                table.Cell(ro, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] splitexamsession = exam_session.Split(new Char[] { '\n' });
                                examse = splitexamsession[val];
                                table.Cell(ro, 2).SetContent(examse);

                                table.Cell(ro, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] splitsubjecode = subject_code.Split(new Char[] { '\n' });
                                subjeccode = splitsubjecode[val];
                                table.Cell(ro, 3).SetContent(subjeccode);

                                table.Cell(ro, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                string[] splitsubjectname = subject_name.Split(new Char[] { '\n' });
                                subjname = splitsubjectname[val];
                                table.Cell(ro, 4).SetContent(subjname);
                                if (examne == "")
                                {
                                    foreach (PdfCell pc in table.CellRange(ro, 1, ro, 1).Cells)
                                        pc.ColSpan = 2;
                                    //foreach (PdfCell pc in table.CellRange(ro, 1, ro, 2).Cells)
                                    //    pc.ColSpan = 2;
                                }

                                val++;
                            }
                        }
                    }

                    //Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 50, 250, 700, 1000));
                    Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 50, 230, 700, 1000));
                    mypdfpage.Add(newpdftabpage);




                    PdfTextArea pt123 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydoc, 25, 830, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "__________________________________________________________________________________________________________________");

                    PdfTextArea ptc21 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, 30, 860, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "No. of Subjects Registered :" + " " + sno);


                    //added by sasikumar

                    PdfTextArea ptc212 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, 0, 860, 800, 50), System.Drawing.ContentAlignment.MiddleRight, time);


                    PdfTextArea ptc2123 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, 0, 880, 800, 50), System.Drawing.ContentAlignment.MiddleRight, time1);

                    //----end----------
                    PdfTextArea pt122 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, 25, 900, 880, 50), System.Drawing.ContentAlignment.MiddleLeft, "__________________________________________________________________________________________________________________");


                    PdfTextArea pts31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydoc, 30, 990, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Candidate");

                    PdfTextArea pts41 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, 0, 990, 800, 50), System.Drawing.ContentAlignment.MiddleRight, "Controller of Examinations");

                    PdfTextArea pt1222 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                   new PdfArea(mydoc, 25, 1000, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "__________________________________________________________________________________________________________________");


                    PdfTextArea pts51 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, 24, 1020, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Note: If any discrepancies are found in the Hall Ticket, report to the COE office immediately.");
                    mypdfpage.Add(pt123);
                    mypdfpage.Add(ptc21);
                    mypdfpage.Add(pt122);
                    mypdfpage.Add(pts31);
                    mypdfpage.Add(pts41);
                    mypdfpage.Add(pt1222);
                    mypdfpage.Add(pts51);
                    //added by sasikuamr
                    mypdfpage.Add(ptc212);
                    mypdfpage.Add(ptc2123);
                    //-----end----

                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        //Aruna on 26feb2013============================
                        string szPath = appPath + "/Report/";
                        string szFile = "Format1.pdf";
                        mypdfpage.SaveToDocument();
                        mydoc.SaveToFile(szPath + szFile);
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/pdf";
                        Response.WriteFile(szPath + szFile);
                        //=============================================

                    }

                }
            }
            srno1 = "";
            examne = "";
            examse = "";
            subjeccode = "";
            subjname = "";
            subjnos = 0;
            srno = "";
            subject_code = "";
            sno = 0;
            nopages = 0;
            time = "";
            time1 = "";
        }
        catch
        {

        }
    }

    protected void chkpassout_CheckedChanged(object sender, EventArgs e)
    {
        if (chkpassout.Checked == true)
        {
            CheckRegular.Checked = false;
            CheckRegular.Enabled = false;
            CheckArrear.Checked = true;
        }
        else
        {
            CheckRegular.Checked = true;
            CheckRegular.Enabled = true;
        }
    }

    public void loadhallticketformat3()
    {
        try
        {
            int selectedcount = 0;
            for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
            {
                int isval = 0;
                string s = FpSpread2.Sheets[0].Cells[res, 6].Text;

                isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);
                if (isval == 1)
                {
                    selectedcount++;
                    errmsg.Text = "";
                }
            }
            if (selectedcount == 0)
            {
                errmsg.Text = "Please Select the Student and then Proceed";
                return;
            }
            Font Fontbold = new Font("Book Antiqua", 17, FontStyle.Bold);
            Font Fontboldd = new Font("Book Antiqua", 17, FontStyle.Regular);
            Font Fontboldbig = new Font("Book Antiqua", 21, FontStyle.Bold);
            Font Fontbold1 = new Font("Book Antiqua", 12, FontStyle.Bold);
            Font Fontbold2 = new Font("Book Antiqua", 15, FontStyle.Regular);
            Font Fontsmall = new Font("Book Antiqua", 13, FontStyle.Regular);
            Font Fontsmall1 = new Font("Book Antiqua", 15, FontStyle.Regular);

            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();

            degreecode = Session["selecteddegreecode"].ToString();
            degree = Session["selecteddegree"].ToString();
            course = Session["selectedcourse"].ToString();
            Boolean halfflag = false;

            if ((ddlMonth.SelectedValue.ToString() != "0") && (ddlYear.SelectedValue.ToString() != "0"))
            {
                FpSpread2.SaveChanges();
                if (FpSpread2.Sheets[0].RowCount > 0)
                {
                    batch = FpSpread2.Sheets[0].Cells[1, 0].Note;
                }
                string examsupplysql = "";


                string strquery = "select * from collinfo where  college_code='" + Session["collegecode"].ToString() + "' ;";
                strquery = strquery + " Select  * from exam_seating where degree_code='" + degreecode + "'";
                strquery = strquery + " select distinct right(convert(nvarchar(100),ex.start_time,100),7) as start,right(convert(nvarchar(100),ex.end_time,100),7) as end1,ex.exam_session from exmtt e,exmtt_det ex  where ex.start_time<> ex.end_time and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and e.degree_code='" + degreecode + "' and e.batchFrom='" + batch + "'  and e.exam_code=ex.exam_code order by start desc";
                strquery = strquery + " select reg_no,roll_no,current_semester,(select photo from stdphoto s where r.app_no=s.app_no) as photo from registration r where r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'";
                DataSet dshall = d2.select_method_wo_parameter(strquery, "Text");


                if (chksupplym.Checked == true)
                {
                    examsupplysql = "and s.subject_no in(select subject_no from Exam_Details ed,exam_application ea,exam_appl_details ead where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.Exam_type=4 and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "')";
                    strquery = "select distinct ss.lab,r.Roll_No,r.reg_no,s.subject_name,s.subject_code,s.subject_no,sc.semester,ed.start_time,ed.end_time,convert(varchar(15),ed.exam_date,103) as edate,ed.exam_session,ss.lab,right(CONVERT(nvarchar(100),ed.start_time,100),7) as start,right(CONVERT(nvarchar(100),ed.end_time,100),7) as end1,exam_session ";
                    strquery = strquery + " from subjectChooser sc,subject s,sub_sem ss,syllabus_master sy,Registration r,exmtt e,exmtt_det ed ,exam_application ea where ea.roll_no=r.Roll_No and ea.Exam_type=4 and sc.subject_no=s.subject_no ";
                    strquery = strquery + " and ss.subType_no=s.subType_no and s.syll_code=sy.syll_code and sy.syll_code=ss.syll_code and ss.promote_count=1 ";
                    strquery = strquery + " and r.Roll_No=sc.roll_no and r.degree_code=sy.degree_code and r.Batch_Year=sy.Batch_Year and e.batchFrom=r.Batch_Year ";
                    strquery = strquery + " and r.degree_code=e.degree_code and e.exam_code=ed.exam_code and ed.subject_no=s.subject_no  and r.roll_no=sc.roll_no and";
                    strquery = strquery + "    r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and ";
                    strquery = strquery + " e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "' " + examsupplysql + " order by edate,sc.semester";
                }
                else
                {
                    examsupplysql = "";
                    strquery = "select distinct ss.lab,r.Roll_No,r.reg_no,s.subject_name,s.subject_code,s.subject_no,sc.semester,ed.start_time,ed.end_time,convert(varchar(15),ed.exam_date,103) as edate,ed.exam_session,ss.lab,right(CONVERT(nvarchar(100),ed.start_time,100),7) as start,right(CONVERT(nvarchar(100),ed.end_time,100),7) as end1,exam_session ";
                    strquery = strquery + " from subjectChooser sc,subject s,sub_sem ss,syllabus_master sy,Registration r,exmtt e,exmtt_det ed where sc.subject_no=s.subject_no ";
                    strquery = strquery + " and ss.subType_no=s.subType_no and s.syll_code=sy.syll_code and sy.syll_code=ss.syll_code and ss.promote_count=1 ";
                    strquery = strquery + " and r.Roll_No=sc.roll_no and r.degree_code=sy.degree_code and r.Batch_Year=sy.Batch_Year and e.batchFrom=r.Batch_Year ";
                    strquery = strquery + " and r.degree_code=e.degree_code and e.exam_code=ed.exam_code and ed.subject_no=s.subject_no  and r.roll_no=sc.roll_no and";
                    strquery = strquery + "  s.subject_no not in(select distinct subject_no from mark_entry m where m.roll_no=r.roll_no and m.passorfail=1 ) and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and ";
                    strquery = strquery + " e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "' " + examsupplysql + " order by edate,sc.semester";
                }
                DataSet dsexamsub = d2.select_method_wo_parameter(strquery, "Text");

                string forenon = "";
                string afterenon = "";
                dshall.Tables[2].DefaultView.RowFilter = " exam_session='F.N'";
                DataView dvse = dshall.Tables[2].DefaultView;
                if (dvse.Count > 0)
                {
                    forenon = dvse[0]["start"].ToString() + " - " + dvse[0]["end1"].ToString();
                }
                dshall.Tables[2].DefaultView.RowFilter = " exam_session='A.N'";
                dvse = dshall.Tables[2].DefaultView;
                if (dvse.Count > 0)
                {
                    afterenon = dvse[dvse.Count - 1]["start"].ToString() + " - " + dvse[dvse.Count - 1]["end1"].ToString();
                }

                string collname = "";
                string address = "";
                string pincode = "";
                string university = "";
                string category = "";
                if (dshall.Tables[0].Rows.Count > 0)
                {
                    collname = dshall.Tables[0].Rows[0]["collname"].ToString();
                    string ad1 = dshall.Tables[0].Rows[0]["address1"].ToString();
                    string ad2 = dshall.Tables[0].Rows[0]["address2"].ToString();
                    string ad3 = dshall.Tables[0].Rows[0]["address3"].ToString();
                    university = dshall.Tables[0].Rows[0]["university"].ToString();
                    category = dshall.Tables[0].Rows[0]["category"].ToString();
                    pincode = dshall.Tables[0].Rows[0]["pincode"].ToString();
                    if (ad1 != "" && ad1 != null)
                    {
                        address = ad1;
                    }
                    if (ad2 != "" && ad2 != null)
                    {
                        if (address != "")
                        {
                            address = address + " ," + ad2;
                        }
                        else
                        {
                            address = ad2;
                        }
                    }
                    if (ad3 != "" && ad3 != null)
                    {
                        if (address != "")
                        {
                            address = address + " ," + ad3;
                        }
                        else
                        {
                            address = ad3;
                        }
                    }
                    if (pincode != "" && pincode != null)
                    {
                        if (address != "")
                        {
                            address = address + "- " + pincode;
                        }
                        else
                        {
                            address = pincode;
                        }
                    }
                }

                DataSet supplymsubds = new DataSet();
                // ArrayList arrsupplymsub = new ArrayList();
                string strsupplymsub = "";
                for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
                {
                    Double coltop = 0;
                    int isval = 0;
                    string s = FpSpread2.Sheets[0].Cells[res, 6].Text;
                    isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);
                    if (isval == 1)
                    {

                        string name = FpSpread2.Sheets[0].Cells[res, 3].Text.ToString();
                        string regno = FpSpread2.Sheets[0].Cells[res, 2].Text.ToString();
                        string rollno = FpSpread2.Sheets[0].Cells[res, 1].Text.ToString();
                        string applyedsubject = "select ea.subject_no  from Exam_Details ed,exam_appl_details ea,exam_application e,subject s, syllabus_master sy,sub_sem su where ed.exam_code =e.exam_code  and e.appl_no =ea.appl_no   and  s.subject_no =ea.subject_no   and  su.syll_code =sy.syll_code and su.subType_no =s.subType_no   and  sy.syll_code =s.syll_code and e.roll_no ='" + rollno + "' and e.Exam_type=4 and ed.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "'";
                        supplymsubds.Clear();
                        supplymsubds = d2.select_method_wo_parameter(applyedsubject, "text");
                        for (int i = 0; i < supplymsubds.Tables[0].Rows.Count; i++)
                        {
                            //arrsupplymsub.Add(supplymsubds.Tables[0].Rows[i]["subject_no"].ToString());
                            if (strsupplymsub == "")
                            {
                                strsupplymsub = supplymsubds.Tables[0].Rows[i]["subject_no"].ToString();
                            }
                            else
                            {
                                strsupplymsub = strsupplymsub + "','" + supplymsubds.Tables[0].Rows[i]["subject_no"].ToString();
                            }
                        }

                        if (chksupplym.Checked == true)
                        {
                            dsexamsub.Tables[0].DefaultView.RowFilter = " reg_no='" + regno + "' and subject_no in ('" + strsupplymsub + "') ";
                        }
                        else
                        {
                            dsexamsub.Tables[0].DefaultView.RowFilter = " reg_no='" + regno + "'";
                        }
                        DataView dvhall = dsexamsub.Tables[0].DefaultView;

                        int stuexamsubcount = dvhall.Count;
                        if (stuexamsubcount > 0)
                        {
                            halfflag = true;
                            mypdfpage = mydocument.NewPage();
                            coltop = coltop + 10;
                            PdfTextArea ptc = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, collname + " ( " + category + " )");
                            mypdfpage.Add(ptc);


                            coltop = coltop + 20;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, address);
                            mypdfpage.Add(ptc);

                            coltop = coltop + 20;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, university);
                            mypdfpage.Add(ptc);

                            coltop = coltop + 30;
                            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "HALL TICKET");
                            mypdfpage.Add(ptc);

                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                            {
                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 30, 10, 500);
                            }

                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                            {
                                PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                mypdfpage.Add(leftimage, 740, 10, 500);
                            }
                            if ((afterenon.Trim() != "" && afterenon != null) || (forenon.Trim() != "" && forenon != null))
                            {


                                Double cot1 = coltop;
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 15, cot1, 800, 47), System.Drawing.ContentAlignment.MiddleLeft, "EXAM TIMINGS");
                                mypdfpage.Add(ptc);

                                if ((forenon.Trim() != "" && forenon != null))
                                {
                                    cot1 = cot1 + 10;
                                    ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 15, cot1, 800, 51), System.Drawing.ContentAlignment.MiddleLeft, "Forenoon  " + forenon + " ");
                                    mypdfpage.Add(ptc);
                                }

                                if ((afterenon.Trim() != "" && afterenon != null))
                                {
                                    cot1 = cot1 + 10;
                                    ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 15, cot1, 800, 51), System.Drawing.ContentAlignment.MiddleLeft, "Afternoon  " + afterenon + " ");
                                    mypdfpage.Add(ptc);
                                }
                                PdfArea tete = new PdfArea(mydocument, 10, cot1, 190, 35);
                                PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);
                                mypdfpage.Add(pr1);
                            }

                            string batyera = "";
                            dshall.Tables[3].DefaultView.RowFilter = "reg_no='" + regno + "'";
                            DataView dvphoto = dshall.Tables[3].DefaultView;
                            if (dvphoto.Count > 0)
                            {
                                string roll = dvphoto[0]["roll_no"].ToString();

                                string currsem = dvphoto[0]["current_semester"].ToString();
                                if (currsem.Trim() == "1" || currsem.Trim() == "2")
                                {
                                    batyera = "I";
                                }
                                else if (currsem.Trim() == "3" || currsem.Trim() == "4")
                                {
                                    batyera = "II";
                                }
                                else if (currsem.Trim() == "5" || currsem.Trim() == "6")
                                {
                                    batyera = "III";
                                }
                                else if (currsem.Trim() == "7" || currsem.Trim() == "8")
                                {
                                    batyera = "IV";
                                }
                                else if (currsem.Trim() == "9" || currsem.Trim() == "1o")
                                {
                                    batyera = "V";
                                }
                                MemoryStream memoryStream = new MemoryStream();
                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                                {
                                    if (dvphoto[0]["photo"] != null && dvphoto[0]["photo"].ToString().Trim() != "")
                                    {
                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                                        {
                                            byte[] file = (byte[])dvphoto[0]["photo"];
                                            memoryStream.Write(file, 0, file.Length);
                                            if (file.Length > 0)
                                            {
                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            }
                                            memoryStream.Dispose();
                                            memoryStream.Close();
                                        }
                                    }
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                                {
                                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg"));
                                    mypdfpage.Add(LogoImage, 650, coltop - 30, 400);
                                }
                            }
                            coltop = coltop + 60;
                            Gios.Pdf.PdfTable table = mydocument.NewTable(Fontbold, 2, 3, 4);
                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            table.VisibleHeaders = false;
                            table.Columns[0].SetWidth(50);
                            table.Columns[1].SetWidth(150);
                            table.Columns[2].SetWidth(50);

                            table.Cell(0, 1).SetFont(Fontbold);
                            table.Cell(0, 2).SetFont(Fontbold);
                            table.Cell(0, 0).SetFont(Fontbold);

                            table.Cell(0, 0).SetContent("Reg.No");
                            table.Cell(0, 1).SetContent("NAME AND CLASS OF CANDIDATE");
                            table.Cell(0, 2).SetContent("MONTH & YEAR");

                            table.Cell(1, 1).SetFont(Fontboldd);
                            table.Cell(1, 2).SetFont(Fontboldd);
                            table.Cell(1, 0).SetFont(Fontboldd);

                            table.Cell(1, 0).SetContent(regno);
                            table.Cell(1, 1).SetContent(name + " (" + batyera + "  " + degree + " " + course + ")");
                            table.Cell(1, 2).SetContent(ddlMonth.SelectedItem.ToString() + " - " + ddlYear.Text.ToString());
                            Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 10, coltop, 825, 1000));
                            mypdfpage.Add(newpdftabpage);



                            Double getheigh = newpdftabpage.Area.Height;
                            getheigh = Math.Round(getheigh, 0);
                            coltop = coltop + getheigh + 20;

                            Gios.Pdf.PdfTable subtable = mydocument.NewTable(Fontsmall1, stuexamsubcount + 1, 7, 6);
                            subtable.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            subtable.VisibleHeaders = false;

                            subtable.Columns[0].SetWidth(30);
                            subtable.Columns[1].SetWidth(50);
                            subtable.Columns[2].SetWidth(150);
                            subtable.Columns[3].SetWidth(50);
                            subtable.Columns[4].SetWidth(40);
                            subtable.Columns[5].SetWidth(50);
                            subtable.Columns[6].SetWidth(30);

                            subtable.Cell(0, 1).SetFont(Fontbold1);
                            subtable.Cell(0, 2).SetFont(Fontbold1);
                            subtable.Cell(0, 3).SetFont(Fontbold1);
                            subtable.Cell(0, 4).SetFont(Fontbold1);
                            subtable.Cell(0, 5).SetFont(Fontbold1);
                            subtable.Cell(0, 6).SetFont(Fontbold1);
                            subtable.Cell(0, 0).SetFont(Fontbold1);


                            subtable.Cell(0, 0).SetContent("S.No");
                            subtable.Cell(0, 1).SetContent("CODE");
                            subtable.Cell(0, 2).SetContent("TITLE OF THE PAPER");
                            subtable.Cell(0, 3).SetContent(" DATE ");
                            subtable.Cell(0, 4).SetContent("SESSION");
                            subtable.Cell(0, 5).SetContent("HALL / ROOM");
                            subtable.Cell(0, 6).SetContent("SEAT");

                            int srno = 0;
                            for (int subc = 0; subc < dvhall.Count; subc++)
                            {
                                srno++;
                                Boolean subjecttype = Convert.ToBoolean(dvhall[subc]["lab"].ToString());
                                string subcode = dvhall[subc]["subject_code"].ToString();
                                string subname = dvhall[subc]["subject_name"].ToString();
                                string edate = dvhall[subc]["edate"].ToString();
                                string ses = dvhall[subc]["exam_session"].ToString();
                                string subjectno = dvhall[subc]["subject_no"].ToString();
                                string room = "";
                                string seatno = "";
                                string[] sp = edate.Split('/');
                                dshall.Tables[1].DefaultView.RowFilter = "subject_no='" + subjectno + "' and edate='" + sp[1] + '/' + sp[0] + '/' + sp[2] + "' and ses_sion='" + ses + "' and regno='" + regno + "'";
                                DataView dvsea = dshall.Tables[1].DefaultView;
                                if (dvsea.Count > 0)
                                {
                                    room = dvsea[0]["roomno"].ToString();
                                    seatno = dvsea[0]["seat_no"].ToString();
                                }
                                if (chkboxvdate.Checked == false && CheckBox1.Checked == false)
                                {


                                    edate = "";
                                    ses = "";


                                }
                                else if (chkboxvdate.Checked == true && CheckBox1.Checked == true)
                                {

                                }
                                else if (chkboxvdate.Checked == true && CheckBox1.Checked == false)
                                {
                                    if (subjecttype == false)
                                    {
                                        edate = "";
                                        ses = "";
                                    }
                                }
                                else if (chkboxvdate.Checked == false && CheckBox1.Checked == true)
                                {
                                    if (subjecttype == true)
                                    {
                                        edate = "";
                                        ses = "";
                                    }
                                }
                                subtable.Cell(srno, 0).SetContent(srno.ToString());
                                subtable.Cell(srno, 1).SetContent(subcode);
                                subtable.Cell(srno, 2).SetContent(subname);

                                subtable.Cell(srno, 3).SetContent(edate);
                                subtable.Cell(srno, 4).SetContent(ses);

                                subtable.Cell(srno, 5).SetContent(room);
                                subtable.Cell(srno, 6).SetContent(seatno);


                                subtable.Cell(srno, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                subtable.Cell(srno, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                subtable.Cell(srno, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                subtable.Cell(srno, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                subtable.Cell(srno, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                subtable.Cell(srno, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                subtable.Cell(srno, 6).SetContentAlignment(ContentAlignment.MiddleCenter);


                            }

                            Gios.Pdf.PdfTablePage newpdftabpage1 = subtable.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 10, coltop, 825, 1000));
                            mypdfpage.Add(newpdftabpage1);


                            getheigh = newpdftabpage1.Area.Height;
                            getheigh = Math.Round(getheigh, 0);
                            coltop = coltop + getheigh + 50;

                            PdfArea tete1 = new PdfArea(mydocument, 10, coltop - 50, 825, 175);
                            PdfRectangle pr2 = new PdfRectangle(mydocument, tete1, Color.Black);
                            mypdfpage.Add(pr2);

                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 20, coltop + 80, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Candidate");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 560, coltop + 80, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Controller of Examinations");
                            mypdfpage.Add(ptc);


                            coltop = coltop + 30;

                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 20, coltop + 100, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Instructions :");
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 100, coltop + 130, 800, 60), System.Drawing.ContentAlignment.MiddleLeft, "(i)   During the examinations,students should produce Hall-Tickets and ID cards to the Invigilators.");
                            mypdfpage.Add(ptc);

                            coltop = coltop + 10;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 100, coltop + 140, 800, 60), System.Drawing.ContentAlignment.MiddleLeft, "(ii)  Students should enter the examination Hall ten minutes before the commencement of the examination.");
                            mypdfpage.Add(ptc);

                            coltop = coltop + 10;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 100, coltop + 150, 800, 60), System.Drawing.ContentAlignment.MiddleLeft, "(iii) Students shall not bring cell phones and programmable calculators inside the Examination Hall.");
                            mypdfpage.Add(ptc);

                            mypdfpage.SaveToDocument();
                            errmsg.Visible = false;
                        }
                    }
                }
                if (halfflag == true)
                {
                    errmsg.Visible = false;
                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        string szPath = appPath + "/Report/";
                        string szFile = "ExamHallTicket.pdf";
                        mydocument.SaveToFile(szPath + szFile);
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/pdf";
                        Response.WriteFile(szPath + szFile);
                    }
                }
                else
                {
                    errmsg.Text = "Please Select the Student and then Proceed";
                    errmsg.Visible = true;
                }
            }
            else
            {
                errmsg.Text = "Please Select Exam Month And Year";
                errmsg.Visible = true;
            }
        }
        catch
        {
        }
    }

    //*********************Added by Senthil***********************************
    public void loadhallticketformat5()
    {
        try
        {
            FpSpread2.SaveChanges();
            int selectedcount = 0;
            for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
            {
                int isval = 0;
                string s = FpSpread2.Sheets[0].Cells[res, 6].Text;

                isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);
                if (isval == 1)
                {
                    selectedcount++;
                    errmsg.Text = "";
                }
            }
            if (selectedcount == 0)
            {
                errmsg.Text = "Please Select the Student and Proceed";
                return;
            }
            Font Fontboldbig = new Font("Times New Roman", 16, FontStyle.Bold);
            Font Fontbold1 = new Font("Times New Roman", 12, FontStyle.Bold);
            Font Fontbold2 = new Font("Book Antiqua", 17, FontStyle.Regular); ;
            Font Fontsmall2 = new Font("Times New Roman", 12, FontStyle.Regular);
            Font Fontsmall1 = new Font("Arial", 12, FontStyle.Bold);
            Font Fontsmall3 = new Font("Times New Roman", 10, FontStyle.Regular);
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();

            degreecode = Session["selecteddegreecode"].ToString();
            degree = Session["selecteddegree"].ToString();
            course = Session["selectedcourse"].ToString();
            Boolean halfflag = false;

            if ((ddlMonth.SelectedValue.ToString() != "0") && (ddlYear.SelectedValue.ToString() != "0"))
            {
                FpSpread2.SaveChanges();
                if (FpSpread2.Sheets[0].RowCount > 0)
                {
                    batch = FpSpread2.Sheets[0].Cells[1, 0].Note;
                }

                //string strquery = "select * from collinfo where  college_code='" + Session["collegecode"].ToString() + "' ;";
                //strquery = strquery + " Select  * from exam_seating where degree_code='" + degreecode + "'";
                //strquery = strquery + " select distinct right(convert(nvarchar(100),ex.start_time,100),7) as start,right(convert(nvarchar(100),ex.end_time,100),7) as end1,ex.exam_session from exmtt e,exmtt_det ex  where ex.start_time<> ex.end_time and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and e.degree_code='" + degreecode + "' and e.batchFrom='" + batch + "'  and e.exam_code=ex.exam_code order by start desc";
                //strquery = strquery + " select reg_no,roll_no,current_semester,(select photo from stdphoto s where r.app_no=s.app_no) as photo from registration r where r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'";
                //DataSet dshall = d2.select_method_wo_parameter(strquery, "Text");

                //strquery = "select r.Roll_No,r.reg_no,s.subject_name,s.subject_code,s.subject_no,sc.semester,ed.start_time,ed.end_time,convert(varchar(15),ed.exam_date,103) as edate,ed.exam_session,ss.lab,right(CONVERT(nvarchar(100),ed.start_time,100),7) as start,right(CONVERT(nvarchar(100),ed.end_time,100),7) as end1,exam_session ";
                //strquery = strquery + " from subjectChooser sc,subject s,sub_sem ss,syllabus_master sy,Registration r,exmtt e,exmtt_det ed where sc.subject_no=s.subject_no ";
                //strquery = strquery + " and ss.subType_no=s.subType_no and s.syll_code=sy.syll_code and sy.syll_code=ss.syll_code and ss.promote_count=1 ";
                //strquery = strquery + " and r.Roll_No=sc.roll_no and r.degree_code=sy.degree_code and r.Batch_Year=sy.Batch_Year and e.batchFrom=r.Batch_Year ";
                //strquery = strquery + " and r.degree_code=e.degree_code and e.exam_code=ed.exam_code and ed.subject_no=s.subject_no  and r.roll_no=sc.roll_no and";
                //strquery = strquery + "  s.subject_no not in(select distinct subject_no from mark_entry m where m.roll_no=r.roll_no and m.passorfail=1 ) and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and ";
                //strquery = strquery + " e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "' order by exam_date,sc.semester";


                string examsupplysql = "";


                string strquery = "select * from collinfo where  college_code='" + Session["collegecode"].ToString() + "' ;";
                strquery = strquery + " Select  * from exam_seating where degree_code='" + degreecode + "'";
                strquery = strquery + " select distinct right(convert(nvarchar(100),ex.start_time,100),7) as start,right(convert(nvarchar(100),ex.end_time,100),7) as end1,ex.exam_session from exmtt e,exmtt_det ex  where ex.start_time<> ex.end_time and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and e.degree_code='" + degreecode + "' and e.batchFrom='" + batch + "'  and e.exam_code=ex.exam_code order by start desc";
                strquery = strquery + " select reg_no,roll_no,current_semester,(select photo from stdphoto s where r.app_no=s.app_no) as photo from registration r where r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'";
                DataSet dshall = d2.select_method_wo_parameter(strquery, "Text");


                if (chksupplym.Checked == true)
                {
                    examsupplysql = "and s.subject_no in(select subject_no from Exam_Details ed,exam_application ea,exam_appl_details ead where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.Exam_type=4 and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "')";
                    strquery = "select ss.lab,r.Roll_No,r.reg_no,s.subject_name,s.subject_code,s.subject_no,sc.semester,ed.start_time,ed.end_time,convert(varchar(15),ed.exam_date,103) as edate,ed.exam_session,ss.lab,right(CONVERT(nvarchar(100),ed.start_time,100),7) as start,right(CONVERT(nvarchar(100),ed.end_time,100),7) as end1,exam_session ";
                    strquery = strquery + " from subjectChooser sc,subject s,sub_sem ss,syllabus_master sy,Registration r,exmtt e,exmtt_det ed where sc.subject_no=s.subject_no ";
                    strquery = strquery + " and ss.subType_no=s.subType_no and s.syll_code=sy.syll_code and sy.syll_code=ss.syll_code and ss.promote_count=1 ";
                    strquery = strquery + " and r.Roll_No=sc.roll_no and r.degree_code=sy.degree_code and r.Batch_Year=sy.Batch_Year and e.batchFrom=r.Batch_Year ";
                    strquery = strquery + " and r.degree_code=e.degree_code and e.exam_code=ed.exam_code and ed.subject_no=s.subject_no  and r.roll_no=sc.roll_no and";
                    strquery = strquery + "    r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and ";
                    strquery = strquery + " e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "' " + examsupplysql + " order by exam_date,sc.semester";
                }
                else
                {
                    examsupplysql = "";
                    strquery = "select ss.lab,r.Roll_No,r.reg_no,s.subject_name,s.subject_code,s.subject_no,sc.semester,ed.start_time,ed.end_time,convert(varchar(15),ed.exam_date,103) as edate,ed.exam_session,ss.lab,right(CONVERT(nvarchar(100),ed.start_time,100),7) as start,right(CONVERT(nvarchar(100),ed.end_time,100),7) as end1,exam_session ";
                    strquery = strquery + " from subjectChooser sc,subject s,sub_sem ss,syllabus_master sy,Registration r,exmtt e,exmtt_det ed where sc.subject_no=s.subject_no ";
                    strquery = strquery + " and ss.subType_no=s.subType_no and s.syll_code=sy.syll_code and sy.syll_code=ss.syll_code and ss.promote_count=1 ";
                    strquery = strquery + " and r.Roll_No=sc.roll_no and r.degree_code=sy.degree_code and r.Batch_Year=sy.Batch_Year and e.batchFrom=r.Batch_Year ";
                    strquery = strquery + " and r.degree_code=e.degree_code and e.exam_code=ed.exam_code and ed.subject_no=s.subject_no  and r.roll_no=sc.roll_no and";
                    strquery = strquery + "  s.subject_no not in(select distinct subject_no from mark_entry m where m.roll_no=r.roll_no and m.passorfail=1 ) and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and ";
                    strquery = strquery + " e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "' " + examsupplysql + " order by exam_date,sc.semester";
                }

                DataSet dsexamsub = d2.select_method_wo_parameter(strquery, "Text");

                string forenon = "";
                string afterenon = "";
                dshall.Tables[2].DefaultView.RowFilter = " exam_session='F.N'";
                DataView dvse = dshall.Tables[2].DefaultView;
                if (dvse.Count > 0)
                {
                    forenon = dvse[0]["start"].ToString() + " - " + dvse[0]["end1"].ToString();
                }
                dshall.Tables[2].DefaultView.RowFilter = " exam_session='A.N'";
                dvse = dshall.Tables[2].DefaultView;
                if (dvse.Count > 0)
                {
                    afterenon = dvse[dvse.Count - 1]["start"].ToString() + " - " + dvse[dvse.Count - 1]["end1"].ToString();
                }

                string collname = "";
                string address = "";
                string pincode = "";
                string university = "";
                string category = "";
                string ad3 = "";
                if (dshall.Tables[0].Rows.Count > 0)
                {
                    collname = dshall.Tables[0].Rows[0]["collname"].ToString().ToUpper();
                    string ad1 = dshall.Tables[0].Rows[0]["address1"].ToString();
                    string ad2 = dshall.Tables[0].Rows[0]["address2"].ToString();
                    ad3 = dshall.Tables[0].Rows[0]["address3"].ToString();
                    university = dshall.Tables[0].Rows[0]["university"].ToString();
                    category = dshall.Tables[0].Rows[0]["category"].ToString();
                    pincode = dshall.Tables[0].Rows[0]["pincode"].ToString();
                    if (ad1 != "" && ad1 != null)
                    {
                        address = ad1;
                    }
                    if (ad2 != "" && ad2 != null)
                    {
                        if (address != "")
                        {
                            address = address + " ," + ad2;
                        }
                        else
                        {
                            address = ad2;
                        }
                    }
                    if (ad3 != "" && ad3 != null)
                    {
                        if (address != "")
                        {
                            address = address + " ," + ad3;
                        }
                        else
                        {
                            address = ad3;
                        }
                    }
                    if (pincode != "" && pincode != null)
                    {
                        if (address != "")
                        {
                            address = address + "- " + pincode;
                        }
                        else
                        {
                            address = pincode;
                        }
                    }
                }
                DataSet supplymsubds = new DataSet();
                // ArrayList arrsupplymsub = new ArrayList();
                string strsupplymsub = "";

                string iscurregual = "";
                if (CheckRegular.Checked == true && CheckArrear.Checked == false)
                {
                    iscurregual = " and  ead.attempts=0";
                }
                if (CheckRegular.Checked == false && CheckArrear.Checked == true)
                {
                    iscurregual = " and ead.attempts>0";
                }
                if (CheckRegular.Checked == true && CheckArrear.Checked == false)
                {
                    for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
                    {
                        Double coltop = 0;
                        int isval = 0;
                        string s = FpSpread2.Sheets[0].Cells[res, 6].Text;
                        isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);
                        if (isval == 1)
                        {
                            string name = FpSpread2.Sheets[0].Cells[res, 3].Text.ToString();
                            string regno = FpSpread2.Sheets[0].Cells[res, 2].Text.ToString();
                            string rolno = FpSpread2.Sheets[0].Cells[res, 1].Text.ToString();
                            string applyedsubject = "select ea.subject_no  from Exam_Details ed,exam_appl_details ea,exam_application e,subject s, syllabus_master sy,sub_sem su where ed.exam_code =e.exam_code  and e.appl_no =ea.appl_no   and  s.subject_no =ea.subject_no   and  su.syll_code =sy.syll_code and su.subType_no =s.subType_no   and  sy.syll_code =s.syll_code and e.roll_no ='" + rolno + "' and e.Exam_type=4 and ed.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "'";
                            supplymsubds.Clear();
                            supplymsubds = d2.select_method_wo_parameter(applyedsubject, "text");
                            strsupplymsub = "";
                            for (int i = 0; i < supplymsubds.Tables[0].Rows.Count; i++)
                            {
                                //arrsupplymsub.Add(supplymsubds.Tables[0].Rows[i]["subject_no"].ToString());
                                if (strsupplymsub == "")
                                {
                                    strsupplymsub = supplymsubds.Tables[0].Rows[i]["subject_no"].ToString();
                                }
                                else
                                {
                                    strsupplymsub = strsupplymsub + "','" + supplymsubds.Tables[0].Rows[i]["subject_no"].ToString();
                                }
                            }
                            if (chksupplym.Checked == true)
                            {
                                dsexamsub.Tables[0].DefaultView.RowFilter = " reg_no='" + regno + "' and subject_no in ('" + strsupplymsub + "')";
                            }
                            else
                            {
                                dsexamsub.Tables[0].DefaultView.RowFilter = " reg_no='" + regno + "'";
                            }
                            DataView dvhall = dsexamsub.Tables[0].DefaultView;

                            int stuexamsubcount = dvhall.Count;
                            if (stuexamsubcount > 0)
                            {
                                halfflag = true;
                                mypdfpage = mydocument.NewPage();
                                coltop = coltop + 50;

                                PdfTextArea ptc = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 0, coltop - 20, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, collname.ToUpper() + " , " + ad3.ToUpper() + " - " + pincode.ToUpper());
                                mypdfpage.Add(ptc);

                                //ptc = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                                //                                            new PdfArea(mydocument, 150, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, pincode);
                                //mypdfpage.Add(ptc);


                                //ptc = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                                //                                           new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, category);
                                //mypdfpage.Add(ptc);
                                string month = ddlMonth.SelectedItem.Text.ToString();
                                string year = ddlYear.SelectedItem.Text.ToString();
                                coltop = coltop + 20;
                                ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 100, coltop - 20, 550, 50), System.Drawing.ContentAlignment.MiddleCenter, university);
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "Office of  the Controller of Examinations");
                                mypdfpage.Add(ptc);
                                coltop = coltop + 20;

                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 300, coltop + 18, 800, 50), System.Drawing.ContentAlignment.TopLeft, "Autonomous Examinations - " + month + " " + year + "");
                                mypdfpage.Add(ptc);
                                //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                //                                            new PdfArea(mydocument, 70, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, " - " + month);
                                //mypdfpage.Add(ptc);
                                //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                //                                           new PdfArea(mydocument, 98, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, year);
                                //mypdfpage.Add(ptc);
                                coltop = coltop + 30;
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 1, coltop - 10, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "HALL TICKET");
                                mypdfpage.Add(ptc);

                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                {
                                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                    mypdfpage.Add(LogoImage, 40, 50, 500);
                                }
                                string roll = dvhall[0]["roll_no"].ToString();
                                dshall.Tables[3].DefaultView.RowFilter = "reg_no='" + roll + "'";


                                DataView dvphoto = dshall.Tables[3].DefaultView;

                                MemoryStream memoryStream = new MemoryStream();
                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                                {
                                    if (dvphoto[0]["photo"] != null && dvphoto[0]["photo"].ToString().Trim() != "")
                                    {
                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                                        {
                                            byte[] file = (byte[])dvphoto[0]["photo"];
                                            memoryStream.Write(file, 0, file.Length);
                                            if (file.Length > 0)
                                            {
                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            }
                                            memoryStream.Dispose();
                                            memoryStream.Close();
                                        }
                                    }
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                                {
                                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg"));
                                    mypdfpage.Add(LogoImage, 670, coltop - 30, 180);
                                }
                                int srno = 0;
                                int subno = 0;
                                int yx = 320;
                                int sk = 0;
                                int sp5 = 0;
                                string endtim = "";
                                string stattim = "";
                                string ses = "";
                                //   Hashtable sk = new Hashtable();
                                string sql = "";
                                string exammtype = "";
                                if (chksupplym.Checked == true)
                                {
                                    exammtype = "and ea.Exam_type=4";
                                }

                                sql = sql + "  select distinct ss.lab,ed.Exam_Month,ed.Exam_year,r.Reg_No,r.Roll_No,r.Stud_Name,sc.semester,s.subject_code,s.subject_name ,s.subject_no,ead.attempts  from Exam_Details ed,exam_application ea,exam_appl_details ead ,Registration r,subject s,subjectChooser sc,sub_sem ss  where  s.subType_no=ss.subType_no and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ead.subject_no=s.subject_no and r.Roll_No=sc.roll_no and sc.roll_no=ea.roll_no and sc.subject_no=ead.subject_no and s.subject_no=sc.subject_no " + iscurregual + " and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "'  and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'and r.Roll_No='" + rolno + "' " + exammtype + " order by r.Reg_No,sc.semester desc,s.subject_code";
                                sql = sql + "  select et.start_time,et.end_time,et.subject_no,et.exam_session,convert(varchar(15),et.exam_date,103) as edate from exmtt_det et,exmtt e where et.exam_code=e.exam_code and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "'    and e.degree_code='" + degreecode + "' and e.batchFrom='" + batch + "'";
                                DataSet ds = d2.select_method_wo_parameter(sql, "Text");
                                for (int subc = 0; subc < ds.Tables[0].Rows.Count; subc++)
                                {
                                    //if (!sk.ContainsKey(ds.Tables[0].Rows[subc]["Roll_No"].ToString()))

                                    // sk.Add(ds.Tables[0].Rows[subc]["Roll_No"].ToString(), ds.Tables[0].Rows[subc]["Roll_No"].ToString());
                                    srno++;
                                    string semm = ds.Tables[0].Rows[subc]["semester"].ToString();
                                    string subcode = ds.Tables[0].Rows[subc]["subject_code"].ToString();
                                    string subname = ds.Tables[0].Rows[subc]["subject_name"].ToString();
                                    Boolean subjecttype = Convert.ToBoolean(ds.Tables[0].Rows[subc]["lab"].ToString());
                                    string subjectno = ds.Tables[0].Rows[subc]["subject_no"].ToString();

                                    ds.Tables[1].DefaultView.RowFilter = "subject_no='" + subjectno + "'";
                                    DataView dvsea = ds.Tables[1].DefaultView;
                                    if (dvsea.Count > 0)
                                    {
                                        string edate = dvsea[0]["edate"].ToString();
                                        ses = dvsea[0]["exam_session"].ToString();
                                        if (chkboxvdate.Checked == false && CheckBox1.Checked == false)
                                        {


                                            edate = "";
                                            ses = "";

                                        }
                                        else if (chkboxvdate.Checked == true && CheckBox1.Checked == true)
                                        {

                                        }
                                        else if (chkboxvdate.Checked == true && CheckBox1.Checked == false)
                                        {
                                            if (subjecttype == false)
                                            {
                                                edate = "";
                                                ses = "";
                                            }
                                        }
                                        else if (chkboxvdate.Checked == false && CheckBox1.Checked == true)
                                        {
                                            if (subjecttype == true)
                                            {
                                                edate = "";
                                                ses = "";
                                            }
                                        }
                                        stattim = dvsea[0]["start_time"].ToString();
                                        endtim = dvsea[0]["end_time"].ToString();
                                        string[] sp = edate.Split('/');
                                        string[] sp1 = stattim.Split(' ');
                                        string[] sp2 = sp1[1].Split(':');
                                        string[] end = endtim.Split(' ');
                                        string[] sp3 = end[1].Split(':');
                                        stattim = sp2[0].ToString() + ":" + sp2[1].ToString();
                                        endtim = sp3[0].ToString() + ":" + sp3[1].ToString();


                                        ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, 60, coltop + 200, 800, 60), System.Drawing.ContentAlignment.TopLeft, semm);
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 98, coltop + 200, 800, 60), System.Drawing.ContentAlignment.TopLeft, edate);
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 168, coltop + 200, 800, 60), System.Drawing.ContentAlignment.TopLeft, ses);
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 220, coltop + 200, 800, 80), System.Drawing.ContentAlignment.TopLeft, subcode);
                                        mypdfpage.Add(ptc);

                                        ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                   new PdfArea(mydocument, 280, coltop + 200, 150, 150), System.Drawing.ContentAlignment.TopLeft, subname);
                                        mypdfpage.Add(ptc);



                                        subno++;

                                        coltop += 40;

                                        if (ses == "F.N")
                                        {
                                            if (sk == 0)
                                            {
                                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, 440, 835, 800, 60), System.Drawing.ContentAlignment.TopLeft, "FN -FORENOON ");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 580, 835, 800, 60), System.Drawing.ContentAlignment.TopLeft, stattim + "AM" + "  -");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 650, 835, 800, 60), System.Drawing.ContentAlignment.TopLeft, endtim + "PM" + "");
                                                mypdfpage.Add(ptc);
                                                sk = 1;
                                            }
                                        }
                                        else
                                        {
                                            if (sp5 == 0)
                                            {
                                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 440, 852, 800, 60), System.Drawing.ContentAlignment.TopLeft, "AN-AFTERNOON ");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 580, 852, 800, 60), System.Drawing.ContentAlignment.TopLeft, stattim + "AM" + "  -");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 650, 852, 800, 60), System.Drawing.ContentAlignment.TopLeft, endtim + "PM" + "");
                                                mypdfpage.Add(ptc);
                                                sp5 = 1;
                                            }
                                        }
                                    }

                                }
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, 300, 840, 800, 60), System.Drawing.ContentAlignment.TopLeft, srno.ToString());
                                mypdfpage.Add(ptc);
                                srno = 0;
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                new PdfArea(mydocument, 50, 840, 800, 60), System.Drawing.ContentAlignment.TopLeft, "No. of Subjects Registered");
                                mypdfpage.Add(ptc);

                                PdfArea pa1 = new PdfArea(mydocument, 30, 40, 800, 1050);
                                PdfRectangle pr3 = new PdfRectangle(mydocument, pa1, Color.Black);
                                mypdfpage.Add(pr3);



                                PdfArea pa2 = new PdfArea(mydocument, 30, 150, 640, 40);
                                PdfRectangle pr5 = new PdfRectangle(mydocument, pa2, Color.Black);
                                mypdfpage.Add(pr5);
                                PdfArea pa3 = new PdfArea(mydocument, 30, 150, 350, 40);
                                PdfRectangle pr6 = new PdfRectangle(mydocument, pa3, Color.Black);
                                mypdfpage.Add(pr6);
                                PdfArea pa5 = new PdfArea(mydocument, 30, 150, 470, 40);
                                PdfRectangle pr7 = new PdfRectangle(mydocument, pa5, Color.Black);
                                mypdfpage.Add(pr7);
                                PdfArea pa6 = new PdfArea(mydocument, 30, 150, 180, 120);
                                PdfRectangle pr8 = new PdfRectangle(mydocument, pa6, Color.Black);
                                mypdfpage.Add(pr8);
                                PdfArea pa7 = new PdfArea(mydocument, 30, 190, 640, 40);
                                PdfRectangle pr9 = new PdfRectangle(mydocument, pa7, Color.Black);
                                mypdfpage.Add(pr9);
                                PdfArea pa8 = new PdfArea(mydocument, 30, 230, 640, 40);
                                PdfRectangle pr10 = new PdfRectangle(mydocument, pa8, Color.Black);
                                mypdfpage.Add(pr10);
                                PdfArea pa9 = new PdfArea(mydocument, 30, 270, 800, 40);
                                PdfRectangle pr11 = new PdfRectangle(mydocument, pa9, Color.Black);
                                mypdfpage.Add(pr11);
                                PdfArea pa10 = new PdfArea(mydocument, 30, 270, 400, 600);
                                PdfRectangle pr12 = new PdfRectangle(mydocument, pa10, Color.Black);
                                mypdfpage.Add(pr12);
                                PdfArea pa11 = new PdfArea(mydocument, 30, 830, 800, 40);
                                PdfRectangle pr13 = new PdfRectangle(mydocument, pa11, Color.Black);
                                mypdfpage.Add(pr13);
                                PdfArea pa12 = new PdfArea(mydocument, 30, 1050, 800, 40);
                                PdfRectangle pr14 = new PdfRectangle(mydocument, pa12, Color.Black);
                                mypdfpage.Add(pr14);
                                PdfArea pa4 = new PdfArea(mydocument, 670, 40, 160, 230);
                                PdfRectangle pr4 = new PdfRectangle(mydocument, pa4, Color.Black);
                                mypdfpage.Add(pr4);


                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 50, 160, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Register Number");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, 220, 160, 800, 60), System.Drawing.ContentAlignment.TopLeft, regno);
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 680, 50, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Photo of the Candidate");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 400, 160, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Semester");
                                mypdfpage.Add(ptc);
                                string semm1 = dvhall[0]["semester"].ToString();

                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 530, 160, 800, 60), System.Drawing.ContentAlignment.TopLeft, semm1);
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 50, 200, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Name");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 220, 200, 800, 60), System.Drawing.ContentAlignment.TopLeft, name);
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 50, 240, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Course/BranchName");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 290, 240, 800, 60), System.Drawing.ContentAlignment.TopLeft, course);
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 220, 240, 800, 60), System.Drawing.ContentAlignment.TopLeft, degree);
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 50, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sem");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, 110, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Date");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 160, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Session");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 220, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sub Code");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 290, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sub Name");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 450, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sem");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 505, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Date");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 555, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Session");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, 615, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sub Code");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                               new PdfArea(mydocument, 685, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sub Name");
                                mypdfpage.Add(ptc);



                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                              new PdfArea(mydocument, 50, 940, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Signature of the Candidate");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                              new PdfArea(mydocument, 640, 940, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Controller of Examinations");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                             new PdfArea(mydocument, 50, 1020, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Signature of the HOD");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                           new PdfArea(mydocument, 643, 1020, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Signature of the Principal");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                           new PdfArea(mydocument, 50, 1060, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Note:If any discrepancies are found in the Hall Ticket, report to the COE office immediately.");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                          new PdfArea(mydocument, 50, 1075, 800, 60), System.Drawing.ContentAlignment.TopLeft, "*: Date will be announced later.");
                                mypdfpage.Add(ptc);

                                mypdfpage.SaveToDocument();
                                errmsg.Visible = false;

                            }
                        }
                    }
                }
                else if (CheckArrear.Checked == true && CheckRegular.Checked == false)
                {
                    for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
                    {
                        Double coltop = 0;
                        int isval = 0;
                        string s = FpSpread2.Sheets[0].Cells[res, 6].Text;
                        isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);
                        if (isval == 1)
                        {
                            string name = FpSpread2.Sheets[0].Cells[res, 3].Text.ToString();
                            string regno = FpSpread2.Sheets[0].Cells[res, 2].Text.ToString();
                            string rolno = FpSpread2.Sheets[0].Cells[res, 1].Text.ToString();
                            string applyedsubject = "select ea.subject_no  from Exam_Details ed,exam_appl_details ea,exam_application e,subject s, syllabus_master sy,sub_sem su where ed.exam_code =e.exam_code  and e.appl_no =ea.appl_no   and  s.subject_no =ea.subject_no   and  su.syll_code =sy.syll_code and su.subType_no =s.subType_no   and  sy.syll_code =s.syll_code and e.roll_no ='" + rolno + "' and e.Exam_type=4 and ed.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "'";
                            supplymsubds.Clear();
                            supplymsubds = d2.select_method_wo_parameter(applyedsubject, "text");
                            strsupplymsub = "";
                            for (int i = 0; i < supplymsubds.Tables[0].Rows.Count; i++)
                            {
                                //arrsupplymsub.Add(supplymsubds.Tables[0].Rows[i]["subject_no"].ToString());
                                if (strsupplymsub == "")
                                {
                                    strsupplymsub = supplymsubds.Tables[0].Rows[i]["subject_no"].ToString();
                                }
                                else
                                {
                                    strsupplymsub = strsupplymsub + "','" + supplymsubds.Tables[0].Rows[i]["subject_no"].ToString();
                                }
                            }

                            if (chksupplym.Checked == true)
                            {
                                dsexamsub.Tables[0].DefaultView.RowFilter = " reg_no='" + regno + "' and subject_no in ('" + strsupplymsub + "')";
                            }
                            else
                            {
                                dsexamsub.Tables[0].DefaultView.RowFilter = " reg_no='" + regno + "'";
                            }


                            DataView dvhall = dsexamsub.Tables[0].DefaultView;
                            int stuexamsubcount = dvhall.Count;
                            if (stuexamsubcount > 0)
                            {
                                halfflag = true;
                                mypdfpage = mydocument.NewPage();
                                coltop = coltop + 50;

                                PdfTextArea ptc = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 0, coltop - 20, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
                                mypdfpage.Add(ptc);

                                //ptc = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                                //                                            new PdfArea(mydocument, 150, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, pincode);
                                //mypdfpage.Add(ptc);


                                //ptc = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                                //                                           new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, category);
                                //mypdfpage.Add(ptc);

                                coltop = coltop + 20;
                                ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 100, coltop - 20, 550, 50), System.Drawing.ContentAlignment.MiddleCenter, university);
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "Office of  the Controller of Examinations ");
                                mypdfpage.Add(ptc);
                                coltop = coltop + 20;
                                string month = ddlMonth.SelectedItem.Text.ToString();
                                string year = ddlYear.SelectedItem.Text.ToString();
                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 300, coltop + 18, 800, 50), System.Drawing.ContentAlignment.TopLeft, "Autonomous Examinations - " + month + " " + year + "");
                                mypdfpage.Add(ptc);
                                //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                //                                            new PdfArea(mydocument, 70, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, month);
                                //mypdfpage.Add(ptc);
                                //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                //                                           new PdfArea(mydocument, 98, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, year);
                                //mypdfpage.Add(ptc);
                                coltop = coltop + 30;
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 1, coltop - 10, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "HALL TICKET");
                                mypdfpage.Add(ptc);

                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                {
                                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                    mypdfpage.Add(LogoImage, 40, 50, 500);
                                }
                                string roll = dvhall[0]["roll_no"].ToString();
                                dshall.Tables[3].DefaultView.RowFilter = "reg_no='" + roll + "'";
                                DataView dvphoto = dshall.Tables[3].DefaultView;

                                MemoryStream memoryStream = new MemoryStream();
                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                                {
                                    if (dvphoto[0]["photo"] != null && dvphoto[0]["photo"].ToString().Trim() != "")
                                    {
                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                                        {
                                            byte[] file = (byte[])dvphoto[0]["photo"];
                                            memoryStream.Write(file, 0, file.Length);
                                            if (file.Length > 0)
                                            {
                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            }
                                            memoryStream.Dispose();
                                            memoryStream.Close();
                                        }
                                    }
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                                {
                                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg"));
                                    mypdfpage.Add(LogoImage, 670, coltop - 30, 180);
                                }
                                int srno = 0;
                                int subno = 0;
                                int yx = 320;

                                //   Hashtable sk = new Hashtable();
                                string sql = "";
                                string ses = "";
                                string subjectno = "";
                                int sk = 0;
                                int sp5 = 0;
                                string stattim = "";
                                string endtim = "";
                                string exammtype = "";
                                if (chksupplym.Checked == true)
                                {
                                    exammtype = "and ea.Exam_type=4";
                                }
                                sql = sql + "  select distinct ss.lab, ed.Exam_Month,ed.Exam_year,r.Reg_No,r.Roll_No,r.Stud_Name,sc.semester,s.subject_code,s.subject_name ,s.subject_no,ead.attempts  from Exam_Details ed,exam_application ea,exam_appl_details ead ,Registration r,subject s,subjectChooser sc  ,sub_sem ss  where  s.subType_no=ss.subType_no and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ead.subject_no=s.subject_no and r.Roll_No=sc.roll_no and sc.roll_no=ea.roll_no and sc.subject_no=ead.subject_no and s.subject_no=sc.subject_no " + iscurregual + " and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "'  and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'and r.Roll_No='" + rolno + "' " + exammtype + " order by r.Reg_No,sc.semester desc,s.subject_code";
                                sql = sql + "  select et.start_time,et.end_time,et.subject_no,et.exam_session,convert(varchar(15),et.exam_date,103) as edate from exmtt_det et,exmtt e where et.exam_code=e.exam_code and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "'    and e.degree_code='" + degreecode + "' and e.batchFrom='" + batch + "'";
                                DataSet ds = d2.select_method_wo_parameter(sql, "Text");
                                for (int subc = 0; subc < ds.Tables[0].Rows.Count; subc++)
                                {
                                    //if (!sk.ContainsKey(ds.Tables[0].Rows[subc]["Roll_No"].ToString()))

                                    // sk.Add(ds.Tables[0].Rows[subc]["Roll_No"].ToString(), ds.Tables[0].Rows[subc]["Roll_No"].ToString());
                                    srno++;
                                    string semm = ds.Tables[0].Rows[subc]["semester"].ToString();
                                    string subcode = ds.Tables[0].Rows[subc]["subject_code"].ToString();
                                    string subname = ds.Tables[0].Rows[subc]["subject_name"].ToString();
                                    Boolean subjecttype = Convert.ToBoolean(ds.Tables[0].Rows[subc]["lab"].ToString());

                                    subjectno = ds.Tables[0].Rows[subc]["subject_no"].ToString();

                                    ds.Tables[1].DefaultView.RowFilter = "subject_no='" + subjectno + "'";
                                    DataView dvsea = ds.Tables[1].DefaultView;
                                    if (dvsea.Count > 0)
                                    {
                                        string edate = dvsea[0]["edate"].ToString();
                                        ses = dvsea[0]["exam_session"].ToString();

                                        if (chkboxvdate.Checked == false && CheckBox1.Checked == false)
                                        {


                                            edate = "";
                                            ses = "";


                                        }
                                        else if (chkboxvdate.Checked == true && CheckBox1.Checked == true)
                                        {

                                        }
                                        else if (chkboxvdate.Checked == true && CheckBox1.Checked == false)
                                        {
                                            if (subjecttype == false)
                                            {
                                                edate = "";
                                                ses = "";
                                            }
                                        }
                                        else if (chkboxvdate.Checked == false && CheckBox1.Checked == true)
                                        {
                                            if (subjecttype == true)
                                            {
                                                edate = "";
                                                ses = "";
                                            }
                                        }
                                        stattim = dvsea[0]["start_time"].ToString();
                                        endtim = dvsea[0]["end_time"].ToString();
                                        string[] sp = edate.Split('/');
                                        string[] sp1 = stattim.Split(' ');
                                        string[] sp2 = sp1[1].Split(':');
                                        string[] end = endtim.Split(' ');
                                        string[] sp3 = end[1].Split(':');
                                        stattim = sp2[0].ToString() + ":" + sp2[1].ToString();
                                        endtim = sp3[0].ToString() + ":" + sp3[1].ToString();
                                        if (subc < 21)
                                        {

                                            ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, 60, coltop + 200, 800, 60), System.Drawing.ContentAlignment.TopLeft, semm);
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 95, coltop + 200, 800, 60), System.Drawing.ContentAlignment.TopLeft, edate);
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 178, coltop + 200, 800, 60), System.Drawing.ContentAlignment.TopLeft, ses);
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 220, coltop + 200, 800, 80), System.Drawing.ContentAlignment.TopLeft, subcode);
                                            mypdfpage.Add(ptc);

                                            ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 285, coltop + 200, 120, 60), System.Drawing.ContentAlignment.TopLeft, subname);
                                            mypdfpage.Add(ptc);

                                        }
                                        else
                                        {

                                            ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                new PdfArea(mydocument, 460, yx, 800, 60), System.Drawing.ContentAlignment.TopLeft, semm);
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 490, yx, 800, 60), System.Drawing.ContentAlignment.TopLeft, edate);
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 580, yx, 800, 60), System.Drawing.ContentAlignment.TopLeft, ses);
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 630, yx, 800, 60), System.Drawing.ContentAlignment.TopLeft, subcode);
                                            mypdfpage.Add(ptc);

                                            ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 690, yx, 100, 90), System.Drawing.ContentAlignment.TopLeft, subname);
                                            mypdfpage.Add(ptc);
                                            yx += 32;
                                        }

                                        subno++;

                                        coltop += 32;

                                    }

                                    if (ses == "F.N")
                                    {
                                        if (sk == 0)
                                        {
                                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 440, 835, 800, 60), System.Drawing.ContentAlignment.TopLeft, "FN -FORENOON ");
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 580, 835, 800, 60), System.Drawing.ContentAlignment.TopLeft, stattim + "AM" + "  -");
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, 650, 835, 800, 60), System.Drawing.ContentAlignment.TopLeft, endtim + "PM" + "");
                                            mypdfpage.Add(ptc);
                                            sk = 1;
                                        }
                                    }
                                    else
                                    {
                                        if (sp5 == 0)
                                        {
                                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 440, 852, 800, 60), System.Drawing.ContentAlignment.TopLeft, "AN-AFTERNOON ");
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 580, 852, 800, 60), System.Drawing.ContentAlignment.TopLeft, stattim + "AM" + "  -");
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, 650, 852, 800, 60), System.Drawing.ContentAlignment.TopLeft, endtim + "PM" + "");
                                            mypdfpage.Add(ptc);
                                            sp5 = 1;
                                        }
                                    }

                                }
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, 300, 840, 800, 60), System.Drawing.ContentAlignment.TopLeft, srno.ToString());
                                mypdfpage.Add(ptc);
                                srno = 0;
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                new PdfArea(mydocument, 50, 840, 800, 60), System.Drawing.ContentAlignment.TopLeft, "No. of Subjects Registered");
                                mypdfpage.Add(ptc);

                                PdfArea pa1 = new PdfArea(mydocument, 30, 40, 800, 1050);
                                PdfRectangle pr3 = new PdfRectangle(mydocument, pa1, Color.Black);
                                mypdfpage.Add(pr3);



                                PdfArea pa2 = new PdfArea(mydocument, 30, 150, 640, 40);
                                PdfRectangle pr5 = new PdfRectangle(mydocument, pa2, Color.Black);
                                mypdfpage.Add(pr5);
                                PdfArea pa3 = new PdfArea(mydocument, 30, 150, 350, 40);
                                PdfRectangle pr6 = new PdfRectangle(mydocument, pa3, Color.Black);
                                mypdfpage.Add(pr6);
                                PdfArea pa5 = new PdfArea(mydocument, 30, 150, 470, 40);
                                PdfRectangle pr7 = new PdfRectangle(mydocument, pa5, Color.Black);
                                mypdfpage.Add(pr7);
                                PdfArea pa6 = new PdfArea(mydocument, 30, 150, 180, 120);
                                PdfRectangle pr8 = new PdfRectangle(mydocument, pa6, Color.Black);
                                mypdfpage.Add(pr8);
                                PdfArea pa7 = new PdfArea(mydocument, 30, 190, 640, 40);
                                PdfRectangle pr9 = new PdfRectangle(mydocument, pa7, Color.Black);
                                mypdfpage.Add(pr9);
                                PdfArea pa8 = new PdfArea(mydocument, 30, 230, 640, 40);
                                PdfRectangle pr10 = new PdfRectangle(mydocument, pa8, Color.Black);
                                mypdfpage.Add(pr10);
                                PdfArea pa9 = new PdfArea(mydocument, 30, 270, 800, 40);
                                PdfRectangle pr11 = new PdfRectangle(mydocument, pa9, Color.Black);
                                mypdfpage.Add(pr11);
                                PdfArea pa10 = new PdfArea(mydocument, 30, 270, 400, 600);
                                PdfRectangle pr12 = new PdfRectangle(mydocument, pa10, Color.Black);
                                mypdfpage.Add(pr12);
                                PdfArea pa11 = new PdfArea(mydocument, 30, 830, 800, 40);
                                PdfRectangle pr13 = new PdfRectangle(mydocument, pa11, Color.Black);
                                mypdfpage.Add(pr13);
                                PdfArea pa12 = new PdfArea(mydocument, 30, 1050, 800, 40);
                                PdfRectangle pr14 = new PdfRectangle(mydocument, pa12, Color.Black);
                                mypdfpage.Add(pr14);
                                PdfArea pa4 = new PdfArea(mydocument, 670, 40, 160, 230);
                                PdfRectangle pr4 = new PdfRectangle(mydocument, pa4, Color.Black);
                                mypdfpage.Add(pr4);


                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 50, 160, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Register Number");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, 220, 160, 800, 60), System.Drawing.ContentAlignment.TopLeft, regno);
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 680, 50, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Photo of the Candidate");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 400, 160, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Semester");
                                mypdfpage.Add(ptc);
                                string semm1 = dvhall[0]["semester"].ToString();

                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 530, 160, 800, 60), System.Drawing.ContentAlignment.TopLeft, semm1);
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 50, 200, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Name");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 220, 200, 800, 60), System.Drawing.ContentAlignment.TopLeft, name);
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 50, 240, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Course/BranchName");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 290, 240, 800, 60), System.Drawing.ContentAlignment.TopLeft, course);
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 220, 240, 800, 60), System.Drawing.ContentAlignment.TopLeft, degree);
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 50, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sem");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, 110, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Date");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 160, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Session");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 220, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sub Code");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 280, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sub Name");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 450, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sem");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 510, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Date");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 560, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Session");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, 620, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sub Code");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                               new PdfArea(mydocument, 690, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sub Name");
                                mypdfpage.Add(ptc);



                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                              new PdfArea(mydocument, 50, 940, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Signature of the Candidate");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                              new PdfArea(mydocument, 640, 940, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Controller of Examinations");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                             new PdfArea(mydocument, 50, 1020, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Signature of the HOD");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                           new PdfArea(mydocument, 643, 1020, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Signature of the Principal");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                           new PdfArea(mydocument, 50, 1060, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Note:If any discrepancies are found in the Hall Ticket, report to the COE office immediately.");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                          new PdfArea(mydocument, 50, 1075, 800, 60), System.Drawing.ContentAlignment.TopLeft, "*: Date will be announced later.");
                                mypdfpage.Add(ptc);

                                mypdfpage.SaveToDocument();
                                errmsg.Visible = false;

                            }
                        }
                    }
                }

                else if (CheckRegular.Checked == true && CheckArrear.Checked == true)
                {
                    for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
                    {
                        Double coltop = 0;
                        int isval = 0;
                        string s = FpSpread2.Sheets[0].Cells[res, 6].Text;
                        isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);
                        if (isval == 1)
                        {
                            string name = FpSpread2.Sheets[0].Cells[res, 3].Text.ToString();
                            string regno = FpSpread2.Sheets[0].Cells[res, 2].Text.ToString();
                            string rolno = FpSpread2.Sheets[0].Cells[res, 1].Text.ToString();
                            string applyedsubject = "select ea.subject_no  from Exam_Details ed,exam_appl_details ea,exam_application e,subject s, syllabus_master sy,sub_sem su where ed.exam_code =e.exam_code  and e.appl_no =ea.appl_no   and  s.subject_no =ea.subject_no   and  su.syll_code =sy.syll_code and su.subType_no =s.subType_no   and  sy.syll_code =s.syll_code and e.roll_no ='" + rolno + "' and e.Exam_type=4 and ed.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "'";
                            supplymsubds.Clear();
                            supplymsubds = d2.select_method_wo_parameter(applyedsubject, "text");
                            strsupplymsub = "";
                            for (int i = 0; i < supplymsubds.Tables[0].Rows.Count; i++)
                            {
                                //arrsupplymsub.Add(supplymsubds.Tables[0].Rows[i]["subject_no"].ToString());
                                if (strsupplymsub == "")
                                {
                                    strsupplymsub = supplymsubds.Tables[0].Rows[i]["subject_no"].ToString();
                                }
                                else
                                {
                                    strsupplymsub = strsupplymsub + "','" + supplymsubds.Tables[0].Rows[i]["subject_no"].ToString();
                                }
                            }
                            if (chksupplym.Checked == true)
                            {
                                dsexamsub.Tables[0].DefaultView.RowFilter = " reg_no='" + regno + "' and subject_no in ('" + strsupplymsub + "')";
                            }
                            else
                            {
                                dsexamsub.Tables[0].DefaultView.RowFilter = " reg_no='" + regno + "'";
                            }

                            DataView dvhall = dsexamsub.Tables[0].DefaultView;
                            int stuexamsubcount = dvhall.Count;
                            if (stuexamsubcount > 0)
                            {
                                halfflag = true;
                                mypdfpage = mydocument.NewPage();
                                coltop = coltop + 50;

                                PdfTextArea ptc = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 0, coltop - 20, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
                                mypdfpage.Add(ptc);

                                //ptc = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                                //                                            new PdfArea(mydocument, 150, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, pincode);
                                //mypdfpage.Add(ptc);


                                //ptc = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                                //                                           new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, category);
                                //mypdfpage.Add(ptc);

                                coltop = coltop + 20;
                                ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 100, coltop - 20, 550, 50), System.Drawing.ContentAlignment.MiddleCenter, university);
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "Office of  the Controller of Examinations ");
                                mypdfpage.Add(ptc);
                                coltop = coltop + 20;
                                string month = ddlMonth.SelectedItem.Text.ToString();
                                string year = ddlYear.SelectedItem.Text.ToString();
                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, 300, coltop + 18, 800, 50), System.Drawing.ContentAlignment.TopLeft, "Autonomous Examinations - " + month + " " + year + "");
                                mypdfpage.Add(ptc);
                                //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                //                                            new PdfArea(mydocument, 70, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, month);
                                //mypdfpage.Add(ptc);
                                //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                //                                           new PdfArea(mydocument, 98, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, year);
                                //mypdfpage.Add(ptc);
                                coltop = coltop + 30;
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 1, coltop - 10, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "HALL TICKET");
                                mypdfpage.Add(ptc);

                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                {
                                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                    mypdfpage.Add(LogoImage, 40, 50, 500);
                                }
                                string roll = dvhall[0]["roll_no"].ToString();
                                dshall.Tables[3].DefaultView.RowFilter = "reg_no='" + roll + "'";
                                DataView dvphoto = dshall.Tables[3].DefaultView;

                                MemoryStream memoryStream = new MemoryStream();
                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                                {
                                    if (dvphoto[0]["photo"] != null && dvphoto[0]["photo"].ToString().Trim() != "")
                                    {
                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                                        {
                                            byte[] file = (byte[])dvphoto[0]["photo"];
                                            memoryStream.Write(file, 0, file.Length);
                                            if (file.Length > 0)
                                            {
                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            }
                                            memoryStream.Dispose();
                                            memoryStream.Close();
                                        }
                                    }
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                                {
                                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg"));
                                    mypdfpage.Add(LogoImage, 670, coltop - 30, 180);
                                }
                                int srno = 0;
                                int subno = 0;
                                int yx = 320;

                                //   Hashtable sk = new Hashtable();
                                string sql = "";
                                string ses = "";
                                string subjectno = "";
                                int sk = 0;
                                int sp5 = 0;
                                string stattim = "";
                                string endtim = "";
                                string exammtype = "";
                                if (chksupplym.Checked == true)
                                {
                                    exammtype = "and ea.Exam_type=4";
                                }
                                sql = sql + "  select distinct ss.lab, ed.Exam_Month,ed.Exam_year,r.Reg_No,r.Roll_No,r.Stud_Name,sc.semester,s.subject_code,s.subject_name ,s.subject_no,ead.attempts  from Exam_Details ed,exam_application ea,exam_appl_details ead ,Registration r,subject s,subjectChooser sc  ,sub_sem ss  where  s.subType_no=ss.subType_no and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ead.subject_no=s.subject_no and r.Roll_No=sc.roll_no and sc.roll_no=ea.roll_no and sc.subject_no=ead.subject_no and s.subject_no=sc.subject_no " + iscurregual + " and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "'  and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'and r.Roll_No='" + rolno + "' " + exammtype + " order by r.Reg_No,sc.semester desc,s.subject_code";
                                sql = sql + "  select et.start_time,et.end_time,et.subject_no,et.exam_session,convert(varchar(15),et.exam_date,103) as edate from exmtt_det et,exmtt e where et.exam_code=e.exam_code and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "'    and e.degree_code='" + degreecode + "' and e.batchFrom='" + batch + "'";
                                DataSet ds = d2.select_method_wo_parameter(sql, "Text");
                                for (int subc = 0; subc < ds.Tables[0].Rows.Count; subc++)
                                {
                                    //if (!sk.ContainsKey(ds.Tables[0].Rows[subc]["Roll_No"].ToString()))

                                    // sk.Add(ds.Tables[0].Rows[subc]["Roll_No"].ToString(), ds.Tables[0].Rows[subc]["Roll_No"].ToString());
                                    srno++;
                                    string semm = ds.Tables[0].Rows[subc]["semester"].ToString();
                                    string subcode = ds.Tables[0].Rows[subc]["subject_code"].ToString();
                                    string subname = ds.Tables[0].Rows[subc]["subject_name"].ToString();
                                    Boolean subjecttype = Convert.ToBoolean(ds.Tables[0].Rows[subc]["lab"].ToString());
                                    subjectno = ds.Tables[0].Rows[subc]["subject_no"].ToString();

                                    ds.Tables[1].DefaultView.RowFilter = "subject_no='" + subjectno + "'";
                                    DataView dvsea = ds.Tables[1].DefaultView;
                                    if (dvsea.Count > 0)
                                    {
                                        string edate = dvsea[0]["edate"].ToString();
                                        ses = dvsea[0]["exam_session"].ToString();
                                        if (chkboxvdate.Checked == false && CheckBox1.Checked == false)
                                        {


                                            edate = "";
                                            ses = "";


                                        }
                                        else if (chkboxvdate.Checked == true && CheckBox1.Checked == true)
                                        {

                                        }
                                        else if (chkboxvdate.Checked == true && CheckBox1.Checked == false)
                                        {
                                            if (subjecttype == false)
                                            {
                                                edate = "";
                                                ses = "";

                                            }
                                        }
                                        else if (chkboxvdate.Checked == false && CheckBox1.Checked == true)
                                        {
                                            if (subjecttype == true)
                                            {
                                                edate = "";
                                                ses = "";

                                            }
                                        }
                                        stattim = dvsea[0]["start_time"].ToString();
                                        endtim = dvsea[0]["end_time"].ToString();
                                        string[] sp = edate.Split('/');
                                        string[] sp1 = stattim.Split(' ');
                                        string[] sp2 = sp1[1].Split(':');
                                        string[] end = endtim.Split(' ');
                                        string[] sp3 = end[1].Split(':');
                                        stattim = sp2[0].ToString() + ":" + sp2[1].ToString();
                                        endtim = sp3[0].ToString() + ":" + sp3[1].ToString();
                                        if (subc < 15)
                                        {

                                            ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, 60, coltop + 200, 800, 60), System.Drawing.ContentAlignment.TopLeft, semm);
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 95, coltop + 200, 800, 60), System.Drawing.ContentAlignment.TopLeft, edate);
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 178, coltop + 200, 800, 60), System.Drawing.ContentAlignment.TopLeft, ses);
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 220, coltop + 200, 800, 80), System.Drawing.ContentAlignment.TopLeft, subcode);
                                            mypdfpage.Add(ptc);

                                            ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 285, coltop + 200, 120, 60), System.Drawing.ContentAlignment.TopLeft, subname);
                                            mypdfpage.Add(ptc);

                                        }
                                        else
                                        {

                                            ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                new PdfArea(mydocument, 460, yx, 800, 60), System.Drawing.ContentAlignment.TopLeft, semm);
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 490, yx, 800, 60), System.Drawing.ContentAlignment.TopLeft, edate);
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 580, yx, 800, 60), System.Drawing.ContentAlignment.TopLeft, ses);
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 630, yx, 800, 60), System.Drawing.ContentAlignment.TopLeft, subcode);
                                            mypdfpage.Add(ptc);

                                            ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 690, yx, 100, 90), System.Drawing.ContentAlignment.TopLeft, subname);
                                            mypdfpage.Add(ptc);
                                            yx += 32;
                                        }

                                        subno++;

                                        coltop += 32;



                                        if (ses == "F.N")
                                        {
                                            if (sk == 0)
                                            {
                                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, 440, 835, 800, 60), System.Drawing.ContentAlignment.TopLeft, "FN -FORENOON ");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 580, 835, 800, 60), System.Drawing.ContentAlignment.TopLeft, stattim + "AM" + "  -");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 650, 835, 800, 60), System.Drawing.ContentAlignment.TopLeft, endtim + "PM" + "");
                                                mypdfpage.Add(ptc);
                                                sk = 1;
                                            }
                                        }
                                        else
                                        {
                                            if (sp5 == 0)
                                            {
                                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 440, 852, 800, 60), System.Drawing.ContentAlignment.TopLeft, "AN-AFTERNOON ");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 580, 852, 800, 60), System.Drawing.ContentAlignment.TopLeft, stattim + "AM" + "  -");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 650, 852, 800, 60), System.Drawing.ContentAlignment.TopLeft, endtim + "PM" + "");
                                                mypdfpage.Add(ptc);
                                                sp5 = 1;
                                            }
                                        }
                                    }

                                }
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, 300, 840, 800, 60), System.Drawing.ContentAlignment.TopLeft, srno.ToString());
                                mypdfpage.Add(ptc);
                                srno = 0;
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                new PdfArea(mydocument, 50, 840, 800, 60), System.Drawing.ContentAlignment.TopLeft, "No. of Subjects Registered");
                                mypdfpage.Add(ptc);

                                PdfArea pa1 = new PdfArea(mydocument, 30, 40, 800, 1050);
                                PdfRectangle pr3 = new PdfRectangle(mydocument, pa1, Color.Black);
                                mypdfpage.Add(pr3);



                                PdfArea pa2 = new PdfArea(mydocument, 30, 150, 640, 40);
                                PdfRectangle pr5 = new PdfRectangle(mydocument, pa2, Color.Black);
                                mypdfpage.Add(pr5);
                                PdfArea pa3 = new PdfArea(mydocument, 30, 150, 350, 40);
                                PdfRectangle pr6 = new PdfRectangle(mydocument, pa3, Color.Black);
                                mypdfpage.Add(pr6);
                                PdfArea pa5 = new PdfArea(mydocument, 30, 150, 470, 40);
                                PdfRectangle pr7 = new PdfRectangle(mydocument, pa5, Color.Black);
                                mypdfpage.Add(pr7);
                                PdfArea pa6 = new PdfArea(mydocument, 30, 150, 180, 120);
                                PdfRectangle pr8 = new PdfRectangle(mydocument, pa6, Color.Black);
                                mypdfpage.Add(pr8);
                                PdfArea pa7 = new PdfArea(mydocument, 30, 190, 640, 40);
                                PdfRectangle pr9 = new PdfRectangle(mydocument, pa7, Color.Black);
                                mypdfpage.Add(pr9);
                                PdfArea pa8 = new PdfArea(mydocument, 30, 230, 640, 40);
                                PdfRectangle pr10 = new PdfRectangle(mydocument, pa8, Color.Black);
                                mypdfpage.Add(pr10);
                                PdfArea pa9 = new PdfArea(mydocument, 30, 270, 800, 40);
                                PdfRectangle pr11 = new PdfRectangle(mydocument, pa9, Color.Black);
                                mypdfpage.Add(pr11);
                                PdfArea pa10 = new PdfArea(mydocument, 30, 270, 400, 600);
                                PdfRectangle pr12 = new PdfRectangle(mydocument, pa10, Color.Black);
                                mypdfpage.Add(pr12);
                                PdfArea pa11 = new PdfArea(mydocument, 30, 830, 800, 40);
                                PdfRectangle pr13 = new PdfRectangle(mydocument, pa11, Color.Black);
                                mypdfpage.Add(pr13);
                                PdfArea pa12 = new PdfArea(mydocument, 30, 1050, 800, 40);
                                PdfRectangle pr14 = new PdfRectangle(mydocument, pa12, Color.Black);
                                mypdfpage.Add(pr14);
                                PdfArea pa4 = new PdfArea(mydocument, 670, 40, 160, 230);
                                PdfRectangle pr4 = new PdfRectangle(mydocument, pa4, Color.Black);
                                mypdfpage.Add(pr4);


                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 50, 160, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Register Number");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, 220, 160, 800, 60), System.Drawing.ContentAlignment.TopLeft, regno);
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 680, 50, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Photo of the Candidate");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 400, 160, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Semester");
                                mypdfpage.Add(ptc);
                                string semm1 = dvhall[0]["semester"].ToString();

                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 530, 160, 800, 60), System.Drawing.ContentAlignment.TopLeft, semm1);
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 50, 200, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Name");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 220, 200, 800, 60), System.Drawing.ContentAlignment.TopLeft, name);
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 50, 240, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Course/BranchName");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 290, 240, 800, 60), System.Drawing.ContentAlignment.TopLeft, course);
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 220, 240, 800, 60), System.Drawing.ContentAlignment.TopLeft, degree);
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 50, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sem");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, 110, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Date");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 160, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Session");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 220, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sub Code");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 280, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sub Name");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 450, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sem");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 510, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Date");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 560, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Session");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, 620, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sub Code");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                               new PdfArea(mydocument, 690, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sub Name");
                                mypdfpage.Add(ptc);



                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                              new PdfArea(mydocument, 50, 940, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Signature of the Candidate");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                              new PdfArea(mydocument, 640, 940, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Controller of Examinations");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                             new PdfArea(mydocument, 50, 1020, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Signature of the HOD");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                           new PdfArea(mydocument, 643, 1020, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Signature of the Principal");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                           new PdfArea(mydocument, 50, 1060, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Note:If any discrepancies are found in the Hall Ticket, report to the COE office immediately.");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                          new PdfArea(mydocument, 50, 1075, 800, 60), System.Drawing.ContentAlignment.TopLeft, "*: Date will be announced later.");
                                mypdfpage.Add(ptc);

                                mypdfpage.SaveToDocument();
                                errmsg.Visible = false;

                            }
                        }
                    }
                }










                //else
                //{
                //    for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
                //    {
                //        Double coltop = 0;
                //        int isval = 0;
                //        string s = FpSpread2.Sheets[0].Cells[res, 6].Text;
                //        isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);
                //        if (isval == 1)
                //        {
                //            string name = FpSpread2.Sheets[0].Cells[res, 3].Text.ToString();
                //            string regno = FpSpread2.Sheets[0].Cells[res, 2].Text.ToString();
                //            string rolno = FpSpread2.Sheets[0].Cells[res, 1].Text.ToString();
                //            dsexamsub.Tables[0].DefaultView.RowFilter = " reg_no='" + regno + "'";
                //            DataView dvhall = dsexamsub.Tables[0].DefaultView;
                //            int stuexamsubcount = dvhall.Count;
                //            if (stuexamsubcount > 0)
                //            {
                //                halfflag = true;
                //                mypdfpage = mydocument.NewPage();
                //                coltop = coltop + 50;

                //                PdfTextArea ptc = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                //                                                            new PdfArea(mydocument, 0, coltop - 20, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
                //                mypdfpage.Add(ptc);

                //                //ptc = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                //                //                                            new PdfArea(mydocument, 150, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, pincode);
                //                //mypdfpage.Add(ptc);


                //                //ptc = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                //                //                                           new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, category);
                //                //mypdfpage.Add(ptc);

                //                coltop = coltop + 20;
                //                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                //                                                            new PdfArea(mydocument, 0, coltop - 20, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, university);
                //                mypdfpage.Add(ptc);

                //                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                //                                                            new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "Office of  the Controller of Examinations ");
                //                mypdfpage.Add(ptc);
                //                coltop = coltop + 20;
                //                string month = ddlMonth.SelectedItem.Text.ToString();
                //                string year = ddlYear.SelectedItem.Text.ToString();
                //                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                //                                                            new PdfArea(mydocument, 300, coltop + 18, 800, 50), System.Drawing.ContentAlignment.TopLeft, "Autonomous Examinations");
                //                mypdfpage.Add(ptc);
                //                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                //                                                            new PdfArea(mydocument, 70, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, month);
                //                mypdfpage.Add(ptc);
                //                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                //                                                           new PdfArea(mydocument, 98, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, year);
                //                mypdfpage.Add(ptc);
                //                coltop = coltop + 30;
                //                ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                //                                                            new PdfArea(mydocument, 1, coltop - 10, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "HALL TICKET");
                //                mypdfpage.Add(ptc);

                //                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                //                {
                //                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                //                    mypdfpage.Add(LogoImage, 40, 50, 500);
                //                }
                //                string roll = dvhall[0]["roll_no"].ToString();
                //                dshall.Tables[3].DefaultView.RowFilter = "reg_no='" + roll + "'";
                //                DataView dvphoto = dshall.Tables[3].DefaultView;

                //                MemoryStream memoryStream = new MemoryStream();
                //                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                //                {
                //                    if (dvphoto[0]["photo"] != null && dvphoto[0]["photo"].ToString().Trim() != "")
                //                    {
                //                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                //                        {
                //                            byte[] file = (byte[])dvphoto[0]["photo"];
                //                            memoryStream.Write(file, 0, file.Length);
                //                            if (file.Length > 0)
                //                            {
                //                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                //                                System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                //                                thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                //                            }
                //                            memoryStream.Dispose();
                //                            memoryStream.Close();
                //                        }
                //                    }
                //                }
                //                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                //                {
                //                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg"));
                //                    mypdfpage.Add(LogoImage, 670, coltop - 30, 180);
                //                }
                //                int srno = 0;
                //                int subno = 0;
                //                int yx = 320;
                //                //if (CheckRegular.Checked == true)
                //                //{
                //                //    //   Hashtable sk = new Hashtable();
                //                //    string sql = "";
                //                //    sql = sql + "  select ed.Exam_Month,ed.Exam_year,r.Reg_No,r.Roll_No,r.Stud_Name,sc.semester,s.subject_code,s.subject_name ,s.subject_no,ead.attempts  from Exam_Details ed,exam_application ea,exam_appl_details ead ,Registration r,subject s,subjectChooser sc  where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ead.subject_no=s.subject_no and r.Roll_No=sc.roll_no and sc.roll_no=ea.roll_no and sc.subject_no=ead.subject_no and s.subject_no=sc.subject_no and ead.attempts=0 and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "'  and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "' order by r.Reg_No,sc.semester desc,s.subject_code";
                //                //    sql = sql + "  select et.subject_no,et.exam_session,convert(varchar(15),et.exam_date,103) as edate from exmtt_det et,exmtt e where et.exam_code=e.exam_code and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "'    and e.degree_code='60' and e.batchFrom='" + batch + "'";
                //                //    DataSet ds = d2.select_method_wo_parameter(sql, "Text");
                //                //    for (int subc = 0; subc < ds.Tables[0].Rows.Count; subc++)
                //                //    {
                //                //        // if (!sk.ContainsKey(ds.Tables[0].Rows[subc]["Roll_No"].ToString()))
                //                //        {
                //                //            // sk.Add(ds.Tables[0].Rows[subc]["Roll_No"].ToString(), ds.Tables[0].Rows[subc]["Roll_No"].ToString());
                //                //            srno++;
                //                //            string semm = ds.Tables[0].Rows[subc]["semester"].ToString();
                //                //            string subcode = ds.Tables[0].Rows[subc]["subject_code"].ToString();
                //                //            string subname = ds.Tables[0].Rows[subc]["subject_name"].ToString();

                //                //            string subjectno = ds.Tables[0].Rows[subc]["subject_no"].ToString();

                //                //            ds.Tables[1].DefaultView.RowFilter = "subject_no='" + subjectno + "'";
                //                //            DataView dvsea = ds.Tables[1].DefaultView;
                //                //            string edate = dvsea[0]["edate"].ToString();
                //                //            string ses = dvsea[0]["exam_session"].ToString();
                //                //            string[] sp = edate.Split('/');
                //                //            if (subc < 21)
                //                //            {

                //                //                ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                //                //                         new PdfArea(mydocument, 60, coltop + 200, 800, 60), System.Drawing.ContentAlignment.TopLeft, semm);
                //                //                mypdfpage.Add(ptc);
                //                //                ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                //                //                            new PdfArea(mydocument, 105, coltop + 200, 800, 60), System.Drawing.ContentAlignment.TopLeft, edate);
                //                //                mypdfpage.Add(ptc);
                //                //                ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                //                //                            new PdfArea(mydocument, 185, coltop + 200, 800, 60), System.Drawing.ContentAlignment.TopLeft, ses);
                //                //                mypdfpage.Add(ptc);
                //                //                ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                //                //                            new PdfArea(mydocument, 230, coltop + 200, 800, 80), System.Drawing.ContentAlignment.TopLeft, subcode);
                //                //                mypdfpage.Add(ptc);

                //                //                ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                //                //                           new PdfArea(mydocument, 280, coltop + 200, 120, 60), System.Drawing.ContentAlignment.TopLeft, subname);
                //                //                mypdfpage.Add(ptc);

                //                //            }
                //                //            else
                //                //            {

                //                //                ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                //                //                    new PdfArea(mydocument, 460, yx, 800, 60), System.Drawing.ContentAlignment.TopLeft, semm);
                //                //                mypdfpage.Add(ptc);
                //                //                ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                //                //                            new PdfArea(mydocument, 505, yx, 800, 60), System.Drawing.ContentAlignment.TopLeft, edate);
                //                //                mypdfpage.Add(ptc);
                //                //                ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                //                //                            new PdfArea(mydocument, 580, yx, 800, 60), System.Drawing.ContentAlignment.TopLeft, ses);
                //                //                mypdfpage.Add(ptc);
                //                //                ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                //                //                            new PdfArea(mydocument, 630, yx, 800, 60), System.Drawing.ContentAlignment.TopLeft, subcode);
                //                //                mypdfpage.Add(ptc);

                //                //                ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                //                //                           new PdfArea(mydocument, 690, yx, 100, 90), System.Drawing.ContentAlignment.TopLeft, subname);
                //                //                mypdfpage.Add(ptc);
                //                //                yx += 23;
                //                //            }

                //                //            subno++;

                //                //            coltop += 23;

                //                //        }
                //                //    }
                //                //}
                //                //else
                //                //{
                //                string stattim = "";
                //                string endtim = "";
                //                int sk = 0;
                //                int sp6 = 0;
                //                for (int subc = 0; subc < dvhall.Count; subc++)
                //                {
                //                    srno++;
                //                    string semm = dvhall[subc]["semester"].ToString();
                //                    string subcode = dvhall[subc]["subject_code"].ToString();
                //                    string subname = dvhall[subc]["subject_name"].ToString();
                //                    string edate = dvhall[subc]["edate"].ToString();
                //                    string ses = dvhall[subc]["exam_session"].ToString();
                //                    string subjectno = dvhall[subc]["subject_no"].ToString();

                //                    stattim = dvhall[subc]["start_time"].ToString();
                //                    endtim = dvhall[subc]["end_time"].ToString();
                //                    string[] sp = edate.Split('/');
                //                    string[] sp1 = stattim.Split(' ');
                //                    string[] sp2 = sp1[1].Split(':');
                //                    string[] end = endtim.Split(' ');
                //                    string[] sp3 = sp1[1].Split(':');
                //                    stattim = sp2[0].ToString() + ":" + sp2[1].ToString();
                //                    endtim = sp3[0].ToString() + ":" + sp3[1].ToString();
                //                    dshall.Tables[1].DefaultView.RowFilter = "subject_no='" + subjectno + "' and edate='" + sp[1] + '/' + sp[0] + '/' + sp[2] + "' and ses_sion='" + ses + "' and regno='" + regno + "'";
                //                    DataView dvsea = dshall.Tables[1].DefaultView;
                //                    if (subc < 18)
                //                    {

                //                        ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                //                                 new PdfArea(mydocument, 60, coltop + 200, 800, 60), System.Drawing.ContentAlignment.TopLeft, semm);
                //                        mypdfpage.Add(ptc);
                //                        ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                //                                    new PdfArea(mydocument, 90, coltop + 200, 800, 60), System.Drawing.ContentAlignment.TopLeft, edate);
                //                        mypdfpage.Add(ptc);
                //                        ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                //                                    new PdfArea(mydocument, 175, coltop + 200, 800, 60), System.Drawing.ContentAlignment.TopLeft, ses);
                //                        mypdfpage.Add(ptc);
                //                        ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                //                                    new PdfArea(mydocument, 230, coltop + 200, 800, 80), System.Drawing.ContentAlignment.TopLeft, subcode);
                //                        mypdfpage.Add(ptc);

                //                        ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                //                                   new PdfArea(mydocument, 280, coltop + 200, 120, 60), System.Drawing.ContentAlignment.TopLeft, subname);
                //                        mypdfpage.Add(ptc);

                //                    }
                //                    else
                //                    {

                //                        ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                //                            new PdfArea(mydocument, 460, yx, 800, 60), System.Drawing.ContentAlignment.TopLeft, semm);
                //                        mypdfpage.Add(ptc);
                //                        ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                //                                    new PdfArea(mydocument, 490, yx, 800, 60), System.Drawing.ContentAlignment.TopLeft, edate);
                //                        mypdfpage.Add(ptc);
                //                        ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                //                                    new PdfArea(mydocument, 570, yx, 800, 60), System.Drawing.ContentAlignment.TopLeft, ses);
                //                        mypdfpage.Add(ptc);
                //                        ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                //                                    new PdfArea(mydocument, 630, yx, 800, 60), System.Drawing.ContentAlignment.TopLeft, subcode);
                //                        mypdfpage.Add(ptc);

                //                        ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                //                                   new PdfArea(mydocument, 690, yx, 100, 90), System.Drawing.ContentAlignment.TopLeft, subname);
                //                        mypdfpage.Add(ptc);
                //                        yx += 32;
                //                    }

                //                    subno++;

                //                    coltop += 32;
                //                    if (sk == 0)
                //                    {
                //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                //                                        new PdfArea(mydocument, 440, 890, 800, 60), System.Drawing.ContentAlignment.TopLeft, "FN-FORENOON");
                //                        mypdfpage.Add(ptc);
                //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                //                                              new PdfArea(mydocument, 580, 890, 800, 60), System.Drawing.ContentAlignment.TopLeft, stattim + "AM" + "");
                //                        mypdfpage.Add(ptc);
                //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                //                                      new PdfArea(mydocument, 650, 890, 800, 60), System.Drawing.ContentAlignment.TopLeft, endtim + "PM" + "");
                //                        mypdfpage.Add(ptc);
                //                        sk = 1;
                //                    }
                //                    else
                //                    {
                //                        if (sp6 == 0)
                //                        {
                //                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                //                                           new PdfArea(mydocument, 440, 902, 800, 60), System.Drawing.ContentAlignment.TopLeft, "AN-AFTERNOON");
                //                            mypdfpage.Add(ptc);
                //                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                //                                              new PdfArea(mydocument, 580, 902, 800, 60), System.Drawing.ContentAlignment.TopLeft, stattim + "AM" + "");
                //                            mypdfpage.Add(ptc);
                //                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                //                                          new PdfArea(mydocument, 650, 902, 800, 60), System.Drawing.ContentAlignment.TopLeft, endtim + "PM" + "");
                //                            mypdfpage.Add(ptc);
                //                            sp6 = 1;
                //                        }
                //                    }

                //                }

                //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                //                                          new PdfArea(mydocument, 300, 890, 800, 60), System.Drawing.ContentAlignment.TopLeft, srno.ToString());
                //                mypdfpage.Add(ptc);
                //                srno = 0;
                //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                //                                new PdfArea(mydocument, 50, 890, 800, 60), System.Drawing.ContentAlignment.TopLeft, "No. of Subjects Registered");
                //                mypdfpage.Add(ptc);


                //                PdfArea pa1 = new PdfArea(mydocument, 30, 40, 800, 1075);
                //                PdfRectangle pr3 = new PdfRectangle(mydocument, pa1, Color.Black);
                //                mypdfpage.Add(pr3);



                //                PdfArea pa2 = new PdfArea(mydocument, 30, 150, 640, 40);
                //                PdfRectangle pr5 = new PdfRectangle(mydocument, pa2, Color.Black);
                //                mypdfpage.Add(pr5);
                //                PdfArea pa3 = new PdfArea(mydocument, 30, 150, 350, 40);
                //                PdfRectangle pr6 = new PdfRectangle(mydocument, pa3, Color.Black);
                //                mypdfpage.Add(pr6);
                //                PdfArea pa5 = new PdfArea(mydocument, 30, 150, 470, 40);
                //                PdfRectangle pr7 = new PdfRectangle(mydocument, pa5, Color.Black);
                //                mypdfpage.Add(pr7);
                //                PdfArea pa6 = new PdfArea(mydocument, 30, 150, 180, 120);
                //                PdfRectangle pr8 = new PdfRectangle(mydocument, pa6, Color.Black);
                //                mypdfpage.Add(pr8);
                //                PdfArea pa7 = new PdfArea(mydocument, 30, 190, 640, 40);
                //                PdfRectangle pr9 = new PdfRectangle(mydocument, pa7, Color.Black);
                //                mypdfpage.Add(pr9);
                //                PdfArea pa8 = new PdfArea(mydocument, 30, 230, 640, 40);
                //                PdfRectangle pr10 = new PdfRectangle(mydocument, pa8, Color.Black);
                //                mypdfpage.Add(pr10);
                //                PdfArea pa9 = new PdfArea(mydocument, 30, 270, 900, 40);
                //                PdfRectangle pr11 = new PdfRectangle(mydocument, pa9, Color.Black);
                //                mypdfpage.Add(pr11);
                //                PdfArea pa10 = new PdfArea(mydocument, 30, 270, 400, 650);
                //                PdfRectangle pr12 = new PdfRectangle(mydocument, pa10, Color.Black);
                //                mypdfpage.Add(pr12);
                //                PdfArea pa11 = new PdfArea(mydocument, 30, 880, 800, 40);
                //                PdfRectangle pr13 = new PdfRectangle(mydocument, pa11, Color.Black);
                //                mypdfpage.Add(pr13);
                //                PdfArea pa12 = new PdfArea(mydocument, 30, 1075, 800, 40);
                //                PdfRectangle pr14 = new PdfRectangle(mydocument, pa12, Color.Black);
                //                mypdfpage.Add(pr14);
                //                PdfArea pa4 = new PdfArea(mydocument, 670, 40, 160, 230);
                //                PdfRectangle pr4 = new PdfRectangle(mydocument, pa4, Color.Black);
                //                mypdfpage.Add(pr4);


                //                ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                //                                                               new PdfArea(mydocument, 50, 160, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Register Number");
                //                mypdfpage.Add(ptc);

                //                ptc = new PdfTextArea(Font, System.Drawing.Color.Black,
                //                                                             new PdfArea(mydocument, 220, 160, 800, 60), System.Drawing.ContentAlignment.TopLeft, regno);
                //                mypdfpage.Add(ptc);

                //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                //                                                              new PdfArea(mydocument, 680, 50, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Photo of the candidate");
                //                mypdfpage.Add(ptc);
                //                ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                //                                                               new PdfArea(mydocument, 400, 160, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Semester");
                //                mypdfpage.Add(ptc);
                //                string semm1 = dvhall[0]["semester"].ToString();

                //                ptc = new PdfTextArea(Font, System.Drawing.Color.Black,
                //                                                              new PdfArea(mydocument, 530, 160, 800, 60), System.Drawing.ContentAlignment.TopLeft, semm1);
                //                mypdfpage.Add(ptc);
                //                ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                //                                                               new PdfArea(mydocument, 50, 200, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Name");
                //                mypdfpage.Add(ptc);
                //                ptc = new PdfTextArea(Font, System.Drawing.Color.Black,
                //                                                               new PdfArea(mydocument, 220, 200, 800, 60), System.Drawing.ContentAlignment.TopLeft, name);
                //                mypdfpage.Add(ptc);

                //                ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                //                                                               new PdfArea(mydocument, 50, 240, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Course/BranchName");
                //                mypdfpage.Add(ptc);

                //                ptc = new PdfTextArea(Font, System.Drawing.Color.Black,
                //                                                              new PdfArea(mydocument, 290, 240, 800, 60), System.Drawing.ContentAlignment.TopLeft, course);
                //                mypdfpage.Add(ptc);

                //                ptc = new PdfTextArea(Font, System.Drawing.Color.Black,
                //                                                              new PdfArea(mydocument, 220, 240, 800, 60), System.Drawing.ContentAlignment.TopLeft, degree);
                //                mypdfpage.Add(ptc);
                //                ptc = new PdfTextArea(Fontsmall4, System.Drawing.Color.Black,
                //                                                              new PdfArea(mydocument, 50, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sem");
                //                mypdfpage.Add(ptc);

                //                ptc = new PdfTextArea(Fontsmall4, System.Drawing.Color.Black,
                //                                                             new PdfArea(mydocument, 110, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Date");
                //                mypdfpage.Add(ptc);

                //                ptc = new PdfTextArea(Fontsmall4, System.Drawing.Color.Black,
                //                                                            new PdfArea(mydocument, 160, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Session");
                //                mypdfpage.Add(ptc);

                //                ptc = new PdfTextArea(Fontsmall4, System.Drawing.Color.Black,
                //                                                            new PdfArea(mydocument, 220, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sub Code");
                //                mypdfpage.Add(ptc);

                //                ptc = new PdfTextArea(Fontsmall4, System.Drawing.Color.Black,
                //                                                           new PdfArea(mydocument, 280, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sub Name");
                //                mypdfpage.Add(ptc);

                //                ptc = new PdfTextArea(Fontsmall4, System.Drawing.Color.Black,
                //                                                           new PdfArea(mydocument, 450, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sem");
                //                mypdfpage.Add(ptc);
                //                ptc = new PdfTextArea(Fontsmall4, System.Drawing.Color.Black,
                //                                                           new PdfArea(mydocument, 510, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Date");
                //                mypdfpage.Add(ptc);
                //                ptc = new PdfTextArea(Fontsmall4, System.Drawing.Color.Black,
                //                                                           new PdfArea(mydocument, 560, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Session");
                //                mypdfpage.Add(ptc);
                //                ptc = new PdfTextArea(Fontsmall4, System.Drawing.Color.Black,
                //                                 new PdfArea(mydocument, 620, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sub Code");
                //                mypdfpage.Add(ptc);

                //                ptc = new PdfTextArea(Fontsmall4, System.Drawing.Color.Black,
                //                               new PdfArea(mydocument, 690, 280, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Sub Name");
                //                mypdfpage.Add(ptc);



                //                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                //                              new PdfArea(mydocument, 50, 980, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Signature of the Candidate");
                //                mypdfpage.Add(ptc);
                //                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                //                              new PdfArea(mydocument, 640, 980, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Controller of Examinations");
                //                mypdfpage.Add(ptc);

                //                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                //                             new PdfArea(mydocument, 50, 1030, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Signature of the HOD");
                //                mypdfpage.Add(ptc);

                //                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                //                           new PdfArea(mydocument, 643, 1030, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Signature of the Principal");
                //                mypdfpage.Add(ptc);

                //                ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                //                           new PdfArea(mydocument, 50, 1085, 800, 60), System.Drawing.ContentAlignment.TopLeft, "Note:If any discrepancies are found in the Hall Ticket, report to the COE office immediately.");
                //                mypdfpage.Add(ptc);
                //                ptc = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                //                          new PdfArea(mydocument, 50, 1100, 800, 60), System.Drawing.ContentAlignment.TopLeft, "*: Date will be announced later.");
                //                mypdfpage.Add(ptc);

                //                mypdfpage.SaveToDocument();
                //                errmsg.Visible = false;
                //            }
                //        }
                //    }
                //}
                if (halfflag == true)
                {
                    errmsg.Visible = false;
                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        string szPath = appPath + "/Report/";
                        string szFile = "ExamHallTicket.pdf";
                        mydocument.SaveToFile(szPath + szFile);
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/pdf";
                        Response.WriteFile(szPath + szFile);
                    }
                }
                else
                {
                    errmsg.Text = "Please Select the Student and Proceed";
                    errmsg.Visible = true;
                }
            }
            else
            {
                errmsg.Text = "Please Select Exam Month And Year";
                errmsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void loadhallticketformat6()
    {
        try
        {
            int selectedcount = 0;
            for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
            {
                int isval = 0;
                string s = FpSpread2.Sheets[0].Cells[res, 6].Text;

                isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);
                if (isval == 1)
                {
                    selectedcount++;
                    errmsg.Text = "";
                }
            }
            if (selectedcount == 0)
            {
                errmsg.Text = "Please Select the Student and then Proceed";
                return;
            }
            Font Fontbold = new Font("Book Antiqua", 17, FontStyle.Bold);
            Font Fontboldd = new Font("Book Antiqua", 17, FontStyle.Regular);
            Font fontcolgname = new Font("Book Antiqua", 18, FontStyle.Bold);
            Font Fontboldbig = new Font("Book Antiqua", 21, FontStyle.Bold);
            Font Fontbold1 = new Font("Book Antiqua", 12, FontStyle.Bold);
            Font Fontbold3 = new Font("Book Antiqua", 10, FontStyle.Bold);
            Font Fontbold2 = new Font("Book Antiqua", 15, FontStyle.Regular);
            Font Fontsmall = new Font("Book Antiqua", 13, FontStyle.Regular);
            Font Fontsmall1 = new Font("Book Antiqua", 15, FontStyle.Regular);

            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();

            degreecode = Session["selecteddegreecode"].ToString();
            degree = Session["selecteddegree"].ToString();
            course = Session["selectedcourse"].ToString();
            Boolean halfflag = false;

            if ((ddlMonth.SelectedValue.ToString() != "0") && (ddlYear.SelectedValue.ToString() != "0"))
            {
                FpSpread2.SaveChanges();
                if (FpSpread2.Sheets[0].RowCount > 0)
                {
                    batch = FpSpread2.Sheets[0].Cells[1, 0].Note;
                }
                string examsupplysql = "";


                string strquery = "select * from collinfo where  college_code='" + Session["collegecode"].ToString() + "' ;";
                strquery = strquery + " Select  * from exam_seating where degree_code='" + degreecode + "'";
                strquery = strquery + " select distinct right(convert(nvarchar(100),ex.start_time,100),7) as start,right(convert(nvarchar(100),ex.end_time,100),7) as end1,ex.exam_session from exmtt e,exmtt_det ex  where ex.start_time<> ex.end_time and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and e.degree_code='" + degreecode + "' and e.batchFrom='" + batch + "'  and e.exam_code=ex.exam_code order by start desc";
                strquery = strquery + " select reg_no,roll_no,current_semester,(select photo from stdphoto s where r.app_no=s.app_no) as photo from registration r where r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'";
                DataSet dshall = d2.select_method_wo_parameter(strquery, "Text");


                if (chksupplym.Checked == true)
                {
                    examsupplysql = "and s.subject_no in(select subject_no from Exam_Details ed,exam_application ea,exam_appl_details ead where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.Exam_type=4 and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "')";
                    strquery = "select distinct ss.lab,r.Roll_No,r.reg_no,s.subject_name,s.subject_code,s.subject_no,sc.semester,ed.start_time,ed.end_time,convert(varchar(15),ed.exam_date,103) as edate,ed.exam_session,ss.lab,right(CONVERT(nvarchar(100),ed.start_time,100),7) as start,right(CONVERT(nvarchar(100),ed.end_time,100),7) as end1,exam_session ";
                    strquery = strquery + " from subjectChooser sc,subject s,sub_sem ss,syllabus_master sy,Registration r,exmtt e,exmtt_det ed ,exam_application ea where ea.roll_no=r.Roll_No and ea.Exam_type=4 and sc.subject_no=s.subject_no ";
                    strquery = strquery + " and ss.subType_no=s.subType_no and s.syll_code=sy.syll_code and sy.syll_code=ss.syll_code and ss.promote_count=1 ";
                    strquery = strquery + " and r.Roll_No=sc.roll_no and r.degree_code=sy.degree_code and r.Batch_Year=sy.Batch_Year and e.batchFrom=r.Batch_Year ";
                    strquery = strquery + " and r.degree_code=e.degree_code and e.exam_code=ed.exam_code and ed.subject_no=s.subject_no  and r.roll_no=sc.roll_no and";
                    strquery = strquery + "    r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and ";
                    strquery = strquery + " e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "' " + examsupplysql + " order by edate,sc.semester";
                }
                else
                {
                    examsupplysql = "";
                    strquery = "select distinct ss.lab,r.Roll_No,r.reg_no,s.subject_name,s.subject_code,s.subject_no,sc.semester,ed.start_time,ed.end_time,convert(varchar(15),ed.exam_date,103) as edate,ed.exam_session,ss.lab,right(CONVERT(nvarchar(100),ed.start_time,100),7) as start,right(CONVERT(nvarchar(100),ed.end_time,100),7) as end1,exam_session ";
                    strquery = strquery + " from subjectChooser sc,subject s,sub_sem ss,syllabus_master sy,Registration r,exmtt e,exmtt_det ed where sc.subject_no=s.subject_no ";
                    strquery = strquery + " and ss.subType_no=s.subType_no and s.syll_code=sy.syll_code and sy.syll_code=ss.syll_code and ss.promote_count=1 ";
                    strquery = strquery + " and r.Roll_No=sc.roll_no and r.degree_code=sy.degree_code and r.Batch_Year=sy.Batch_Year and e.batchFrom=r.Batch_Year ";
                    strquery = strquery + " and r.degree_code=e.degree_code and e.exam_code=ed.exam_code and ed.subject_no=s.subject_no  and r.roll_no=sc.roll_no and";
                    strquery = strquery + "  s.subject_no not in(select distinct subject_no from mark_entry m where m.roll_no=r.roll_no and m.passorfail=1 ) and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and ";
                    strquery = strquery + " e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "' " + examsupplysql + " order by edate,sc.semester";
                }
                DataSet dsexamsub = d2.select_method_wo_parameter(strquery, "Text");

                string forenon = "";
                string afterenon = "";
                dshall.Tables[2].DefaultView.RowFilter = " exam_session='F.N'";
                DataView dvse = dshall.Tables[2].DefaultView;
                if (dvse.Count > 0)
                {
                    forenon = dvse[0]["start"].ToString() + " - " + dvse[0]["end1"].ToString();
                }
                dshall.Tables[2].DefaultView.RowFilter = " exam_session='A.N'";
                dvse = dshall.Tables[2].DefaultView;
                if (dvse.Count > 0)
                {
                    afterenon = dvse[dvse.Count - 1]["start"].ToString() + " - " + dvse[dvse.Count - 1]["end1"].ToString();
                }

                string collname = "";
                string address = "";
                string pincode = "";
                string university = "";
                string category = "";
                string addr_city_dist = "";
                string affby = "";
                if (dshall.Tables[0].Rows.Count > 0)
                {
                    collname = Convert.ToString(dshall.Tables[0].Rows[0]["collname"]);
                    string ad1 = Convert.ToString(dshall.Tables[0].Rows[0]["address1"]);
                    string ad2 = Convert.ToString(dshall.Tables[0].Rows[0]["address2"]);
                    string ad3 = Convert.ToString(dshall.Tables[0].Rows[0]["address3"]);
                    pincode = Convert.ToString(dshall.Tables[0].Rows[0]["pincode"]).Trim();
                    pincode = pincode.Substring(pincode.Length - 3);
                    int pin = 0;
                    int.TryParse(pincode, out pin);
                    addr_city_dist = " , " + Convert.ToString(dshall.Tables[0].Rows[0]["address3"]) + " , " + Convert.ToString(dshall.Tables[0].Rows[0]["district"]) + ((pin != 0) ? (" - " + pin.ToString()) : " - " + pincode);
                    university = dshall.Tables[0].Rows[0]["university"].ToString();
                    affby = Convert.ToString(dshall.Tables[0].Rows[0]["affliatedby"]);
                    string[] strpa = affby.Split(',');
                    affby = "( " + university + " " + strpa[0] + " )";

                    category = dshall.Tables[0].Rows[0]["category"].ToString();
                    pincode = dshall.Tables[0].Rows[0]["pincode"].ToString();
                    if (ad1 != "" && ad1 != null)
                    {
                        address = ad1;
                    }
                    if (ad2 != "" && ad2 != null)
                    {
                        if (address != "")
                        {
                            address = address + " ," + ad2;
                        }
                        else
                        {
                            address = ad2;
                        }
                    }
                    if (ad3 != "" && ad3 != null)
                    {
                        if (address != "")
                        {
                            address = address + " ," + ad3;
                        }
                        else
                        {
                            address = ad3;
                        }
                    }
                    if (pincode != "" && pincode != null)
                    {
                        if (address != "")
                        {
                            address = address + "- " + pincode;
                        }
                        else
                        {
                            address = pincode;
                        }
                    }
                }

                DataSet supplymsubds = new DataSet();
                // ArrayList arrsupplymsub = new ArrayList();
                string strsupplymsub = "";
                for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
                {
                    Double coltop = 0;
                    int isval = 0;
                    string s = FpSpread2.Sheets[0].Cells[res, 6].Text;
                    isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);
                    if (isval == 1)
                    {

                        string name = FpSpread2.Sheets[0].Cells[res, 3].Text.ToString();
                        string regno = FpSpread2.Sheets[0].Cells[res, 2].Text.ToString();
                        string rollno = FpSpread2.Sheets[0].Cells[res, 1].Text.ToString();
                        string applyedsubject = "select ea.subject_no  from Exam_Details ed,exam_appl_details ea,exam_application e,subject s, syllabus_master sy,sub_sem su where ed.exam_code =e.exam_code  and e.appl_no =ea.appl_no   and  s.subject_no =ea.subject_no   and  su.syll_code =sy.syll_code and su.subType_no =s.subType_no   and  sy.syll_code =s.syll_code and e.roll_no ='" + rollno + "' and e.Exam_type=4 and ed.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "'";
                        supplymsubds.Clear();
                        supplymsubds = d2.select_method_wo_parameter(applyedsubject, "text");
                        for (int i = 0; i < supplymsubds.Tables[0].Rows.Count; i++)
                        {
                            //arrsupplymsub.Add(supplymsubds.Tables[0].Rows[i]["subject_no"].ToString());
                            if (strsupplymsub == "")
                            {
                                strsupplymsub = supplymsubds.Tables[0].Rows[i]["subject_no"].ToString();
                            }
                            else
                            {
                                strsupplymsub = strsupplymsub + "','" + supplymsubds.Tables[0].Rows[i]["subject_no"].ToString();
                            }
                        }

                        if (chksupplym.Checked == true)
                        {
                            dsexamsub.Tables[0].DefaultView.RowFilter = " reg_no='" + regno + "' and subject_no in ('" + strsupplymsub + "') ";
                        }
                        else
                        {
                            dsexamsub.Tables[0].DefaultView.RowFilter = " reg_no='" + regno + "'";
                        }
                        DataView dvhall = dsexamsub.Tables[0].DefaultView;

                        int stuexamsubcount = dvhall.Count;
                        if (stuexamsubcount > 0)
                        {
                            halfflag = true;
                            mypdfpage = mydocument.NewPage();
                            coltop = coltop + 10;
                            PdfTextArea ptc = new PdfTextArea(fontcolgname, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, collname + addr_city_dist);
                            mypdfpage.Add(ptc);


                            coltop = coltop + 15;
                            ptc = new PdfTextArea(Fontbold3, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, affby);
                            mypdfpage.Add(ptc);


                            coltop = coltop + 20;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "Office of the Controller of Examination");
                            mypdfpage.Add(ptc);

                            //coltop = coltop + 20;
                            //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                            //                                            new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, university);
                            //mypdfpage.Add(ptc);

                            coltop = coltop + 30;
                            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "HALL TICKET");
                            mypdfpage.Add(ptc);

                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                            {
                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 30, 10, 500);
                            }

                            //if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                            //{
                            //    PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                            //    mypdfpage.Add(leftimage, 740, 10, 500);
                            //}

                            #region STudent Photo
                            //string stdphtsql = "select * from StdPhoto where app_no='" + stdappno + "'";
                            //MemoryStream memoryStream = new MemoryStream();
                            //DataSet dsstdpho = new DataSet();
                            //dsstdpho.Clear();
                            //dsstdpho.Dispose();
                            //dsstdpho = da.select_method_wo_parameter(stdphtsql, "Text");
                            //if (dsstdpho.Tables[0].Rows.Count > 0)
                            //{
                            //    byte[] file = (byte[])dsstdpho.Tables[0].Rows[0][1];
                            //    memoryStream.Write(file, 0, file.Length);
                            //    if (file.Length > 0)
                            //    {
                            //        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                            //        System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                            //        if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg")))
                            //        {
                            //        }
                            //        else
                            //        {
                            //            thumb.Save(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                            //        }


                            //    }

                            //}
                            //if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg")))
                            //{
                            //    Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg"));
                            //    mypdfpage.Add(LogoImage2, 485, coltop + 30, 300);
                            //}
                            //else
                            //{
                            //    Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg"));
                            //    mypdfpage.Add(LogoImage2, 485, coltop + 30, 300);
                            //}
                            #endregion

                            if ((afterenon.Trim() != "" && afterenon != null) || (forenon.Trim() != "" && forenon != null))
                            {


                                Double cot1 = coltop;
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 15, cot1 + 2, 800, 47), System.Drawing.ContentAlignment.MiddleLeft, "EXAM TIMINGS");
                                mypdfpage.Add(ptc);

                                if ((forenon.Trim() != "" && forenon != null))
                                {
                                    cot1 = cot1 + 10;
                                    ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 15, cot1, 800, 51), System.Drawing.ContentAlignment.MiddleLeft, "Forenoon  " + forenon + " ");
                                    mypdfpage.Add(ptc);
                                }

                                if ((afterenon.Trim() != "" && afterenon != null))
                                {
                                    cot1 = cot1 + 11;
                                    ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 15, cot1, 800, 51), System.Drawing.ContentAlignment.MiddleLeft, "Afternoon  " + afterenon + " ");
                                    mypdfpage.Add(ptc);
                                }
                                PdfArea tete = new PdfArea(mydocument, 10, cot1 - 9, 200, 45);
                                PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);
                                mypdfpage.Add(pr1);
                            }

                            string batyera = "";
                            dshall.Tables[3].DefaultView.RowFilter = "reg_no='" + regno + "'";
                            DataView dvphoto = dshall.Tables[3].DefaultView;
                            if (dvphoto.Count > 0)
                            {
                                string roll = dvphoto[0]["roll_no"].ToString();

                                string currsem = dvphoto[0]["current_semester"].ToString();
                                if (currsem.Trim() == "1" || currsem.Trim() == "2")
                                {
                                    batyera = "I";
                                }
                                else if (currsem.Trim() == "3" || currsem.Trim() == "4")
                                {
                                    batyera = "II";
                                }
                                else if (currsem.Trim() == "5" || currsem.Trim() == "6")
                                {
                                    batyera = "III";
                                }
                                else if (currsem.Trim() == "7" || currsem.Trim() == "8")
                                {
                                    batyera = "IV";
                                }
                                else if (currsem.Trim() == "9" || currsem.Trim() == "1o")
                                {
                                    batyera = "V";
                                }
                                MemoryStream memoryStream = new MemoryStream();
                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                                {
                                    if (dvphoto[0]["photo"] != null && dvphoto[0]["photo"].ToString().Trim() != "")
                                    {
                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                                        {
                                            byte[] file = (byte[])dvphoto[0]["photo"];
                                            memoryStream.Write(file, 0, file.Length);
                                            if (file.Length > 0)
                                            {
                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            }
                                            memoryStream.Dispose();
                                            memoryStream.Close();
                                        }
                                    }
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                                {
                                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg"));
                                    //mypdfpage.Add(LogoImage, 650, coltop - 30, 400);
                                    mypdfpage.Add(LogoImage, 730, coltop - 45, 300);
                                    //mypdfpage.Add(leftimage, 740, 10, 500);
                                }
                                else
                                {
                                    Gios.Pdf.PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg"));
                                    //mypdfpage.Add(LogoImage, 650, coltop - 30, 400);
                                    mypdfpage.Add(LogoImage, 730, coltop - 45, 300);
                                }
                            }
                            coltop = coltop + 60;
                            Gios.Pdf.PdfTable table = mydocument.NewTable(Fontbold, 2, 3, 4);
                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            table.VisibleHeaders = false;
                            table.Columns[0].SetWidth(50);
                            table.Columns[1].SetWidth(150);
                            table.Columns[2].SetWidth(50);

                            table.Cell(0, 1).SetFont(Fontbold);
                            table.Cell(0, 2).SetFont(Fontbold);
                            table.Cell(0, 0).SetFont(Fontbold);

                            table.Cell(0, 0).SetContent("Reg.No");
                            table.Cell(0, 1).SetContent("NAME AND CLASS OF CANDIDATE");
                            table.Cell(0, 2).SetContent("MONTH & YEAR");

                            table.Cell(1, 1).SetFont(Fontboldd);
                            table.Cell(1, 2).SetFont(Fontboldd);
                            table.Cell(1, 0).SetFont(Fontboldd);

                            table.Cell(1, 0).SetContent(regno);
                            table.Cell(1, 1).SetContent(name + " ( " + degree + " - " + batyera + " Year )");

                            string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(ddlMonth.SelectedValue.ToString()));
                            table.Cell(1, 2).SetContent(strMonthName.ToString() + " - " + ddlYear.Text.ToString());
                            Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 10, coltop, 825, 1000));
                            mypdfpage.Add(newpdftabpage);



                            Double getheigh = newpdftabpage.Area.Height;
                            getheigh = Math.Round(getheigh, 0);
                            coltop = coltop + getheigh + 20;

                            Gios.Pdf.PdfTable subtable = mydocument.NewTable(Fontsmall1, stuexamsubcount + 1, 7, 6);
                            subtable.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            subtable.VisibleHeaders = false;

                            subtable.Columns[0].SetWidth(30);
                            subtable.Columns[1].SetWidth(50);
                            subtable.Columns[2].SetWidth(150);
                            subtable.Columns[3].SetWidth(50);
                            subtable.Columns[4].SetWidth(40);
                            subtable.Columns[5].SetWidth(50);
                            subtable.Columns[6].SetWidth(30);

                            subtable.Cell(0, 1).SetFont(Fontbold1);
                            subtable.Cell(0, 2).SetFont(Fontbold1);
                            subtable.Cell(0, 3).SetFont(Fontbold1);
                            subtable.Cell(0, 4).SetFont(Fontbold1);
                            subtable.Cell(0, 5).SetFont(Fontbold1);
                            subtable.Cell(0, 6).SetFont(Fontbold1);
                            subtable.Cell(0, 0).SetFont(Fontbold1);


                            subtable.Cell(0, 0).SetContent("S.No");
                            subtable.Cell(0, 1).SetContent("Course Code");
                            subtable.Cell(0, 2).SetContent("TITLE OF THE COURSE");
                            subtable.Cell(0, 3).SetContent(" DATE ");
                            subtable.Cell(0, 4).SetContent("SESSION");
                            subtable.Cell(0, 5).SetContent("HALL / ROOM");
                            subtable.Cell(0, 6).SetContent("SEAT");

                            int srno = 0;
                            for (int subc = 0; subc < dvhall.Count; subc++)
                            {
                                srno++;
                                Boolean subjecttype = Convert.ToBoolean(dvhall[subc]["lab"].ToString());
                                string subcode = dvhall[subc]["subject_code"].ToString();
                                string subname = dvhall[subc]["subject_name"].ToString();
                                string edate = dvhall[subc]["edate"].ToString();
                                string ses = dvhall[subc]["exam_session"].ToString();
                                string subjectno = dvhall[subc]["subject_no"].ToString();
                                string room = "";
                                string seatno = "";
                                string[] sp = edate.Split('/');
                                dshall.Tables[1].DefaultView.RowFilter = "subject_no='" + subjectno + "' and edate='" + sp[1] + '/' + sp[0] + '/' + sp[2] + "' and ses_sion='" + ses + "' and regno='" + regno + "'";
                                DataView dvsea = dshall.Tables[1].DefaultView;
                                if (dvsea.Count > 0)
                                {
                                    room = dvsea[0]["roomno"].ToString();
                                    seatno = dvsea[0]["seat_no"].ToString();
                                }
                                if (chkboxvdate.Checked == false && CheckBox1.Checked == false)
                                {
                                    edate = "";
                                    ses = "";
                                }
                                else if (chkboxvdate.Checked == true && CheckBox1.Checked == true)
                                {

                                }
                                else if (chkboxvdate.Checked == true && CheckBox1.Checked == false)
                                {
                                    if (subjecttype == false)
                                    {
                                        edate = "";
                                        ses = "";
                                    }
                                }
                                else if (chkboxvdate.Checked == false && CheckBox1.Checked == true)
                                {
                                    if (subjecttype == true)
                                    {
                                        edate = "";
                                        ses = "";
                                    }
                                }
                                subtable.Cell(srno, 0).SetContent(srno.ToString());
                                subtable.Cell(srno, 1).SetContent(subcode);
                                subtable.Cell(srno, 2).SetContent(subname);

                                subtable.Cell(srno, 3).SetContent(edate);
                                subtable.Cell(srno, 4).SetContent(ses);

                                subtable.Cell(srno, 5).SetContent(room);
                                subtable.Cell(srno, 6).SetContent(seatno);


                                subtable.Cell(srno, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                subtable.Cell(srno, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                subtable.Cell(srno, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                subtable.Cell(srno, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                subtable.Cell(srno, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                subtable.Cell(srno, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                subtable.Cell(srno, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                            }

                            Gios.Pdf.PdfTablePage newpdftabpage1 = subtable.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 10, coltop, 825, 1000));
                            mypdfpage.Add(newpdftabpage1);


                            getheigh = newpdftabpage1.Area.Height;
                            getheigh = Math.Round(getheigh, 0);
                            coltop = coltop + getheigh + 50;

                            PdfArea tete1 = new PdfArea(mydocument, 10, coltop - 50, 825, 175);
                            PdfRectangle pr2 = new PdfRectangle(mydocument, tete1, Color.Black);
                            mypdfpage.Add(pr2);

                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 20, coltop + 80, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Candidate");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 670, coltop + 80, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Controller Of Examinations");
                            mypdfpage.Add(ptc);


                            coltop = coltop + 30;

                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 20, coltop + 100, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Instructions :");
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 100, coltop + 130, 800, 60), System.Drawing.ContentAlignment.MiddleLeft, "(i)   During the examinations,students should produce Hall-Tickets and ID cards to the Invigilators.");
                            mypdfpage.Add(ptc);

                            coltop = coltop + 10;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 100, coltop + 140, 800, 60), System.Drawing.ContentAlignment.MiddleLeft, "(ii)  Students should enter the examinations Hall ten minutes before the commencement of the examinations.");
                            mypdfpage.Add(ptc);

                            coltop = coltop + 10;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 100, coltop + 150, 800, 60), System.Drawing.ContentAlignment.MiddleLeft, "(iii) Students shall not bring cell phones and programmable calculators inside the Examination Hall.");
                            mypdfpage.Add(ptc);

                            mypdfpage.SaveToDocument();
                            errmsg.Visible = false;
                        }
                    }
                }
                if (halfflag == true)
                {
                    errmsg.Visible = false;
                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        string szPath = appPath + "/Report/";
                        string szFile = "Exam_Hall_Ticket_Format-6.pdf";
                        mydocument.SaveToFile(szPath + szFile);
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/pdf";
                        Response.WriteFile(szPath + szFile);
                    }
                }
                else
                {
                    errmsg.Text = "Please Select the Student and then Proceed";
                    errmsg.Visible = true;
                }
            }
            else
            {
                errmsg.Text = "Please Select Exam Month And Year";
                errmsg.Visible = true;
            }
        }
        catch
        {
        }
    }

    protected void Btn_range_Click(object sender, EventArgs e)// Added By SaranyaDevi(30.11.2017)
    {
        try
        {
            if (txt_frange.Text == "" || txt_trange.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Enter Both From And To Range.')", true);
                return;
            }

            if (Convert.ToInt32(txt_frange.Text) > Convert.ToInt32(txt_trange.Text))
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('To Range Should Be Greater Than Or Equal To From Range.')", true);
                return;
            }

            for (int i = 0; i < FpSpread2.Sheets[0].RowCount; i++)
            {
                string sl_no = FpSpread2.Sheets[0].Cells[i, 0].Text.ToString();

                if (sl_no != "")
                {
                    if (Convert.ToInt32(sl_no) >= Convert.ToInt32(txt_frange.Text) && Convert.ToInt32(sl_no) <= Convert.ToInt32(txt_trange.Text))
                    {
                        FpSpread2.Sheets[0].Cells[i, 6].Value = "1";
                        FpSpread2.Sheets[0].Cells[i, 6].Locked = false; // poo

                    }
                    else
                    {
                        FpSpread2.Sheets[0].Cells[i, 6].Value = "0";
                    }
                }
            }

            txt_frange.Text = "";
            txt_trange.Text = "";
        }


        catch
        { }
    } 
  
}