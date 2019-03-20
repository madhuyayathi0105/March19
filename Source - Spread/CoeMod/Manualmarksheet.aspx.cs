using System;
using System.Collections;
using System.Configuration;
using System.Data;
//using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gios.Pdf;

public partial class Manualmarksheet : System.Web.UI.Page
{
    FarPoint.Web.Spread.TextCellType txtceltype = new FarPoint.Web.Spread.TextCellType();
    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
    SqlCommand cmd;
    DataSet ds = new DataSet();
    string totcredits = "";
    string calculate = "";
    double total1 = 0;
    double gpacal1 = 0;
    string sectionddl = "";
    string grpsectionddl = "";
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
    string collegecode = "";
    string Master = "";
    int sn0 = 0;
    int year = 0;
    FarPoint.Web.Spread.CheckBoxCellType chkboxcol = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkboxsel_all = new FarPoint.Web.Spread.CheckBoxCellType();

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
            lbldop.Visible = false;
            txtdop.Visible = false;
            lbldoi.Visible = false;
            txtdoi.Visible = false;
            FpSpread2.Visible = false;
            if (!Page.IsPostBack)
            {
                bindedulevel();
                loadtype();
                loadsubjecttype();
                chk_subjectwise_CheckedChanged(sender, e);
                // bindyear();
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
                chkboxsel_all.AutoPostBack = true;
                HAllSpread.Sheets[0].RowHeader.Visible = false;
                HAllSpread.Sheets[0].ColumnCount = 7;
                HAllSpread.Sheets[0].RowCount = 1;
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
                HAllSpread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Select";
                HAllSpread.Sheets[0].Columns[0].Width = 40;
                HAllSpread.Sheets[0].Columns[1].Width = 40;
                HAllSpread.Sheets[0].Columns[5].Font.Underline = true;
                HAllSpread.Sheets[0].Columns[5].ForeColor = Color.Black;
                HAllSpread.Sheets[0].AutoPostBack = false;
                HAllSpread.CommandBar.Visible = false;
                HAllSpread.Sheets[0].PageSize = HAllSpread.Sheets[0].RowCount;
                bindsec();
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindsec()
    {
        string exammonth = ddlMonth.SelectedValue.ToString();
        string examyear = ddlYear.SelectedValue.ToString();
        string sql = "select distinct r.sections  from exam_details e,exam_application a,registration r, degree g,course c,department p  where e.exam_code = a.exam_code and a.roll_no = r.roll_no and r.degree_code = g.degree_code  and g.course_id = c.course_id and g.dept_code = p.dept_code   and c.college_code='" + Session["collegecode"].ToString() + "'  and e.Exam_Month='" + exammonth + "' and e.Exam_Year='" + examyear + "'   and ltrim(r.roll_no) <>''  and cc=0 and delflag=0 and exam_flag<>'Debar'  and r.Sections is not null and r.Sections<>''   group by r.sections     ";
        ds1.Clear();
        ds1 = da.select_method_wo_parameter(sql, "Text");
        ddlsec.Items.Clear();
        if (ds1.Tables[0].Rows.Count > 0)
        {
            ddlsec.DataSource = ds1;
            ddlsec.DataTextField = "sections";
            ddlsec.DataBind();
            ddlsec.Items.Insert(0, "All");
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        hiddenfiels();
        lblno.Visible = false;
        string semesters = "";
        string typefilter = "";
        if (ddledulevel.Items.Count > 0 && ddlcoltypeadd.Items.Count > 0)
        {
            typefilter = "and c.Edu_Level='" + ddledulevel.SelectedItem.Text.ToString() + "' and c.type='" + ddlcoltypeadd.SelectedItem.Text.ToString() + "' ";
        }
        string batchyearcal = "";
        if (dropterm.Items.Count > 0)
        {
            semesters = "and semester='" + dropterm.SelectedItem.Text.ToString() + "'";
            if (Convert.ToInt32(dropterm.SelectedItem.Text.ToString()) % 2 == 0)
            {
                batchyearcal = da.GetFunction("  select max(Batch_Year) from Registration where Current_Semester between " + (Convert.ToInt32(dropterm.SelectedItem.Text.ToString()) - 1) + " and " + Convert.ToInt32(dropterm.SelectedItem.Text.ToString()) + "");
            }
            else
            {
                batchyearcal = da.GetFunction("  select max(Batch_Year) from Registration where Current_Semester between " + (Convert.ToInt32(dropterm.SelectedItem.Text.ToString())) + " and " + (Convert.ToInt32(dropterm.SelectedItem.Text.ToString()) + 1) + "");
            }
            batchyearcal = batchyearcal + "," + Convert.ToString(Convert.ToInt32(batchyearcal) - 1);
        }
        //  errmsg.Visible = true;
        //if (ddlsec.Items.Count ==0)
        //{
        //    lblno.Text = "Please Select Year";
        //    lblno.Visible = true;
        //    return;
        //}
        if (chk_subjectwise.Checked == true)
        {
            FpSpread2.Sheets[0].RowHeader.Visible = false;
            FpSpread2.CommandBar.Visible = false;
            FpSpread2.Sheets[0].ColumnCount = 3;
            FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread2.Sheets[0].AutoPostBack = false;
            FpSpread2.Sheets[0].Columns[0].Locked = true;
            FpSpread2.Sheets[0].Columns[1].Locked = true;
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.Font.Bold = true;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Subject Code and subject Name";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Select";
            FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns[0].Locked = true;
            FpSpread2.Sheets[0].Columns[1].Locked = true;
            FpSpread2.Sheets[0].Columns[1].Width = 350;
            //FpSpread2.Sheets[0].Columns[2].Locked = true;
            sectionddl = "";
            //bindsubjectpdf();
            FpSpread2.Sheets[0].RowCount = 1;
            FpSpread2.Sheets[0].Cells[0, 2].CellType = chkboxsel_all;
            FpSpread2.Sheets[0].Cells[0, 2].Value = 0;
            chkboxsel_all.AutoPostBack = true;
            FpSpread2.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.SaveChanges();
            if (dropterm.Items.Count > 0)
            {
                semesters = "and semester='" + dropterm.SelectedItem.Text.ToString() + "'";
            }
            string subjecttypesql = "";
            if (dropsubjecttype.SelectedIndex != 0 && dropsubjecttype.Items.Count > 0)
            {
                subjecttypesql = " and subject_type='" + dropsubjecttype.SelectedItem.Text.ToString() + "'";
            }
            if (dropsubjecttype.Items.Count > 0)
            {
                string subject = "select distinct  subject_code+'-'+subject_name as subject_name,subject_code from course c , Degree d,subject s,syllabus_master y,sub_sem ss  where s.syll_code = y.syll_code and ss.syll_code=s.syll_code and s.subType_no=ss.subType_no and c.Course_Id=d.Course_Id and d.Degree_Code=y.degree_code and y.syll_code=ss.syll_code   " + subjecttypesql + "   and y.Batch_Year in (" + batchyearcal + ")  " + typefilter + " " + semesters + " ; ";
                ds = d2.select_method_wo_parameter(subject, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //ddlsubject.DataSource = ds;
                    //ddlsubject.DataTextField = "subject_name";
                    //ddlsubject.DataValueField = "subject_code";
                    //ddlsubject.DataBind();
                    for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                    {
                        FpSpread2.Sheets[0].Rows.Count++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(ii + 1);
                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].CellType = txtceltype;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[ii]["subject_name"].ToString());
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[ii]["subject_code"].ToString()); ;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].CellType = chkboxcol;
                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].Text = ds.Tables[0].Rows[ii]["Reg_No"].ToString();
                    }
                    int rowheigth = FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Height;
                    rowheigth = rowheigth * FpSpread2.Sheets[0].Rows.Count;
                    FpSpread2.Height = rowheigth + 100;
                    FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                    FpSpread2.SaveChanges();
                    FpSpread2.Visible = true;
                    printbtn.Visible = true;
                }
                if (FpSpread2.Sheets[0].Rows.Count == 1)
                {
                    lblno.Text = "No Records Found";
                    lblno.Visible = true;
                    FpSpread2.SaveChanges();
                    FpSpread2.Visible = false;
                    printbtn.Visible = false;
                    return;
                }
            }
        }
        else
        {
            if (ddlMonth.SelectedIndex == 0)
            {
                lblno.Text = "Please Select Month";
                lblno.Visible = true;
                return;
            }
            if (ddlYear.SelectedIndex == 0)
            {
                lblno.Text = "Please Select Year";
                lblno.Visible = true;
                return;
            }
            int rowheigth = 1;
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
            string secval = "";
            if (ddlsec.Items.Count > 0)
            {
                if (ddlsec.SelectedIndex != 0)
                {
                    sectionddl = "and sections='" + ddlsec.SelectedItem.Text.ToString() + "'";
                    grpsectionddl = "r.sections";
                    secval = "and sections='" + ddlsec.SelectedItem.Text.ToString() + "'";
                }
            }
            if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
            {
                year++;
                //modified on 26/9/2017 by PRABHA
                string spraedbind = "select distinct r.batch_year as batchto,p.Dept_Acronym as dept,p.Dept_Name as deptname,c.course_name as course,r.current_semester as sem, r.degree_code as degree,p.dept_code as dptcode,count(distinct r.roll_no) as studcount  from registration r,degree g,course c,department p  where  r.degree_code = g.degree_code and g.course_id = c.course_id and g.dept_code = p.dept_code and r.CC=0 and r.DelFlag='0' and r.Exam_Flag<>'debar' and c.college_code=" + Session["collegecode"].ToString() + "  and ltrim(r.roll_no) <>'' and r.current_semester between 1 and 2 " + sectionddl + " " + typefilter + "  group by r.batch_year,p.Dept_Acronym,p.Dept_Name,c.course_name,r.current_semester,r.degree_code,p.dept_code order by r.current_semester  ";//r.CC=0 and r.DelFlag='0' and r.Exam_Flag<>'debar'

                if (chkBasedOnExamApplication.Checked)
                {
                    spraedbind = "select distinct e.batch_year as batchto,p.Dept_Acronym as dept,p.Dept_Name as deptname,c.course_name as course,r.current_semester as sem, e.degree_code as degree,p.dept_code as dptcode,count(distinct a.roll_no) as studcount   from  exam_appl_details ad,exam_details e,exam_application a,registration r,degree g,course c,department p  where e.exam_code = a.exam_code and a.roll_no = r.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id and g.dept_code = p.dept_code and r.CC=0 and r.DelFlag='0' and r.Exam_Flag<>'debar' and c.college_code=" + Session["collegecode"].ToString() + " and ad.appl_no=a.appl_no and ad.attempts=0   and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + "  and ltrim(r.roll_no) <>'' and r.current_semester between 1 and 2 " + sectionddl + "  " + typefilter + " group by e.batch_year,p.Dept_Acronym,p.Dept_Name,c.course_name,r.current_semester,e.degree_code,p.dept_code  order by r.current_semester ";//r.CC=0 and r.DelFlag='0' and r.Exam_Flag<>'debar'
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
                HAllSpread.Sheets[0].RowCount = 1;
                chkboxsel_all.AutoPostBack = true;
                HAllSpread.Sheets[0].Cells[0, 6].CellType = chkboxsel_all;
                HAllSpread.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                HAllSpread.SaveChanges();
                Toadeptreader = Todptcmd.ExecuteReader();
                if (Toadeptreader.HasRows)
                {
                    sn0++;
                    while (Toadeptreader.Read())
                    {
                        HAllSpread.Visible = true;
                        HAllSpread.Sheets[0].RowCount = HAllSpread.Sheets[0].RowCount + 1;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 6].CellType = chkboxcol;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 6].Value = 0;
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
                        string totstud = "select  isnull(count(*),0) as total from registration  where batch_year='" + batchyear + "' and degree_code='" + degree + "' and current_semester='" + sem + "' and CC=0 and DelFlag='0' and Exam_Flag<>'debar' and college_code=" + Session["collegecode"].ToString() + "  " + secval + "";
                        string studinfo = da.GetFunction(totstud);
                        totstud = studinfo;
                        int totalstudents = 0;
                        if (Int32.TryParse(studinfo, out totalstudents))
                        {
                            totalstudents = totalstudents + Convert.ToInt32(totstud);
                            overalltot = overalltot + Convert.ToInt32(totstud);
                        }
                        else
                        {
                            totalstudents = 0;
                        }
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(totstud); //totalstudents + "";
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        int semval = Convert.ToInt16(sem) + Convert.ToInt16(1);
                    }
                }
                HAllSpread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                HAllSpread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                HAllSpread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            }
            //=======second year
            //modified on 26/9/2017 by PRABHA
            if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
            {
                year++;
                string spraedbind = "select distinct r.batch_year as batchto,p.Dept_Acronym as dept,p.Dept_Name as deptname,c.course_name as course,r.current_semester as sem, r.degree_code as degree,p.dept_code as dptcode,count(distinct r.roll_no) as studcount   from registration r,degree g,course c,department p  where  r.degree_code = g.degree_code and g.course_id = c.course_id and g.dept_code = p.dept_code and r.CC=0 and r.DelFlag='0' and r.Exam_Flag<>'debar'   and c.college_code=" + Session["collegecode"].ToString() + "  and ltrim(r.roll_no) <>''      and r.current_semester between 3 and 4 " + sectionddl + " " + typefilter + "  group by r.batch_year,p.Dept_Acronym,p.Dept_Name,c.course_name,r.current_semester,r.degree_code,p.dept_code order by r.current_semester  ";
                if (chkBasedOnExamApplication.Checked)
                {
                    spraedbind = "select distinct e.batch_year as batchto,p.Dept_Acronym as dept,p.Dept_Name as deptname,c.course_name as course,r.current_semester as sem, e.degree_code as degree,p.dept_code as dptcode,count(distinct a.roll_no) as studcount   from exam_appl_details ad, exam_details e,exam_application a,registration r,degree g,course c,department p  where e.exam_code = a.exam_code and a.roll_no = r.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id and g.dept_code = p.dept_code and r.CC=0 and r.DelFlag='0' and r.Exam_Flag<>'debar'  and c.college_code=" + Session["collegecode"].ToString() + " and ad.appl_no=a.appl_no and ad.attempts=0  and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + "  and ltrim(r.roll_no) <>''      and r.current_semester between 3 and 4  " + sectionddl + " " + typefilter + "  group by e.batch_year,p.Dept_Acronym,p.Dept_Name,c.course_name,r.current_semester,e.degree_code,p.dept_code     order by r.current_semester ";//r.CC=0 and r.DelFlag='0' and r.Exam_Flag<>'debar'
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
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 6].CellType = chkboxcol;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 6].Value = 0;
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
                        string totstud = "select  isnull(count(*),0) as total from registration where batch_year='" + batchyear + "' and degree_code='" + degree + "' and current_semester='" + sem + "' and CC=0 and DelFlag='0' and Exam_Flag<>'debar' and college_code=" + Session["collegecode"].ToString() + "  " + sectionddl + "";
                        string studinfo = da.GetFunction(totstud);
                        totstud = studinfo;
                        int totalstudents = 0;
                        if (Int32.TryParse(studinfo, out totalstudents))
                        {
                            totalstudents = totalstudents + Convert.ToInt32(totstud);
                            overalltot = overalltot + Convert.ToInt32(totstud);
                        }
                        else
                        {
                            totalstudents = 0;
                        }
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(totstud); //totalstudents + "";
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        int semval = Convert.ToInt16(sem) + Convert.ToInt16(1);
                    }
                }
                HAllSpread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                HAllSpread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                HAllSpread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            }
            //====3rd year
            //modified on 26/9/2017 by PRABHA
            if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
            {
                year++;
                string spraedbind = "select distinct r.batch_year as batchto,p.Dept_Acronym as dept,p.Dept_Name as deptname,c.course_name as course,r.current_semester as sem, r.degree_code as degree,p.dept_code as dptcode,count(distinct r.roll_no) as studcount   from registration r,degree g,course c,department p  where  r.degree_code = g.degree_code and g.course_id = c.course_id and g.dept_code = p.dept_code and r.CC=0 and r.DelFlag='0' and r.Exam_Flag<>'debar' and c.college_code=" + Session["collegecode"].ToString() + "  and ltrim(r.roll_no) <>''      and r.current_semester between 5 and 6 " + sectionddl + " " + typefilter + "  group by r.batch_year,p.Dept_Acronym,p.Dept_Name,c.course_name,r.current_semester,r.degree_code,p.dept_code order by r.current_semester  ";//r.CC=0 and r.DelFlag='0' and r.Exam_Flag<>'debar'
                if (chkBasedOnExamApplication.Checked)
                {
                    spraedbind = "select distinct e.batch_year as batchto,p.Dept_Acronym as dept,p.Dept_Name as deptname,c.course_name as course,r.current_semester as sem, e.degree_code as degree,p.dept_code as dptcode,count(distinct a.roll_no) as studcount from exam_appl_details ad, exam_details e,exam_application a,registration r,degree g,course c,department p  where e.exam_code = a.exam_code and a.roll_no = r.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id and g.dept_code = p.dept_code and r.CC=0 and r.DelFlag='0' and r.Exam_Flag<>'debar'  and c.college_code=" + Session["collegecode"].ToString() + " and ad.appl_no=a.appl_no and ad.attempts=0  and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + "  and ltrim(r.roll_no) <>'' and r.current_semester between 5 and 6  " + sectionddl + "  " + typefilter + "  group by e.batch_year,p.Dept_Acronym,p.Dept_Name,c.course_name,r.current_semester,e.degree_code,p.dept_code order by r.current_semester ";//r.CC=0 and r.DelFlag='0' and r.Exam_Flag<>'debar'
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
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 6].CellType = chkboxcol;
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 6].Value = 0;
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
                        string totstud = "select  isnull(count(*),0) as total from registration where batch_year='" + batchyear + "' and degree_code='" + degree + "' and current_semester='" + sem + "' and CC=0 and DelFlag='0' and Exam_Flag<>'debar'  and college_code=" + Session["collegecode"].ToString() + "  " + sectionddl + "";
                        string studinfo = da.GetFunction(totstud);
                        totstud = studinfo;
                        int totalstudents = 0;
                        if (Int32.TryParse(studinfo, out totalstudents))
                        {
                            totalstudents = totalstudents + Convert.ToInt32(totstud);
                            overalltot = overalltot + Convert.ToInt32(totstud);
                        }
                        else
                        {
                            totalstudents = 0;
                        }
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(totstud); //totalstudents + "";
                        HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        int semval = Convert.ToInt16(sem) + Convert.ToInt16(1);
                    }
                }
                HAllSpread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                HAllSpread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                HAllSpread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            }
            //====4th year
            //if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
            //{
            //    year++;
            //    string spraedbind = "select distinct e.batch_year as batchto,p.Dept_Acronym as dept,p.Dept_Name as deptname,c.course_name as course,r.current_semester as sem, e.degree_code as degree,p.dept_code as dptcode,count(distinct a.roll_no) as studcount   from exam_appl_details ad, exam_details e,exam_application a,registration r,degree g,course c,department p  where e.exam_code = a.exam_code and a.roll_no = r.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id and g.dept_code = p.dept_code   and c.college_code=" + Session["collegecode"].ToString() + " and ad.appl_no=a.appl_no and ad.attempts=0  and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + "  and ltrim(r.roll_no) <>''       and r.current_semester between 7 and 8  " + sectionddl + " " + typefilter + "  group by e.batch_year,p.Dept_Acronym,p.Dept_Name,c.course_name,r.current_semester,e.degree_code,p.dept_code     order by r.current_semester ";
            //    SqlCommand Todptcmd = new SqlCommand(spraedbind, con);
            //    SqlDataReader Toadeptreader;
            //    string course = "";
            //    string department = "";
            //    string sem = "";
            //    string degree = "";
            //    string batchyear = "";
            //    string department_code = "";
            //    string department_name = "";
            //    con.Close();
            //    con.Open();
            //    Toadeptreader = Todptcmd.ExecuteReader();
            //    if (Toadeptreader.HasRows)
            //    {
            //        sn0++;
            //        while (Toadeptreader.Read())
            //        {
            //            batchyear = Toadeptreader["batchto"].ToString();
            //            HAllSpread.Visible = true;
            //            HAllSpread.Sheets[0].RowCount = HAllSpread.Sheets[0].RowCount + 1;
            //            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 6].CellType = chkboxcol;
            //            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 6].Value = 0;
            //            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Note = batchyear;
            //            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 0].Text = sn0 + "";
            //            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Text = year + "";
            //            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            //            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
            //            degree = Toadeptreader["degree"].ToString();
            //            course = Toadeptreader["course"].ToString();
            //            department = Toadeptreader["dept"].ToString();
            //            sem = Toadeptreader["sem"].ToString();
            //            department_code = Toadeptreader["dptcode"].ToString();
            //            department_name = Toadeptreader["deptname"].ToString();
            //            batchyear = Toadeptreader["batchto"].ToString();
            //            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 1].Tag = degree;
            //            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Tag = department_name;
            //            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].Text = sem + "";
            //            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Note = degree;
            //            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 3].Text = department;
            //            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 2].Text = course;
            //            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
            //            string totstud = "select  isnull(count(*),0) as total from registration where batch_year='" + batchyear + "' and degree_code='" + degree + "' and current_semester='" + sem + "' and college_code=" + Session["collegecode"].ToString() + "  " + sectionddl + "";
            //            string studinfo = da.GetFunction(totstud);
            //            totstud = studinfo;
            //            int totalstudents = 0;
            //            if (Int32.TryParse(studinfo, out totalstudents))
            //            {
            //                totalstudents = totalstudents + Convert.ToInt32(totstud);
            //                overalltot = overalltot + Convert.ToInt32(totstud);
            //            }
            //            else
            //            {
            //                totalstudents = 0;
            //            }
            //            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(totstud); //totalstudents + "";
            //            HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
            //            int semval = Convert.ToInt16(sem) + Convert.ToInt16(1);
            //        }
            //    }
            //    HAllSpread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //    HAllSpread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //    HAllSpread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //}
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
            if (HAllSpread.Sheets[0].Rows.Count > 2)
            {
                rowheigth = HAllSpread.Sheets[0].Rows[HAllSpread.Sheets[0].RowCount - 1].Height;
                rowheigth = rowheigth * HAllSpread.Sheets[0].Rows.Count;
                HAllSpread.Height = rowheigth + 100;
                HAllSpread.Visible = true;
                printbtn.Visible = true;
                FpSpread2.Visible = false;
                Label2.Visible = false;
            }
            else
            {
                lblno.Text = "No Records Found";
                lblno.Visible = true;
                HAllSpread.Visible = false;
                FpSpread2.SaveChanges();
                FpSpread2.Visible = false;
                printbtn.Visible = false;
                return;
            }
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
        bindsec();
        hiddenfiels();
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
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
        // bindsec();
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
        bindsec();
        hiddenfiels();
    }

    //protected void HAllSpread_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{
    //    Cellclick = true;
    //}
    //protected void HAllSpread_SelectedIndexChanged(Object sender, EventArgs e)
    //{
    //    Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
    //    System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
    //    System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
    //    System.Drawing.Font Fontbold1 = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Bold);
    //    System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
    //    System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
    //    System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
    //    System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
    //    System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
    //    System.Drawing.Font Fontsmall1bold = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
    //    System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);
    //    Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
    //    string sql = "SELECT collname,affliatedby,address1,address2,address3,pincode,email,logo1,logo2,email  from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
    //    DataSet ds = new DataSet();
    //    ds.Clear();
    //    ds.Dispose();
    //    ds = da.select_method_wo_parameter(sql, "Text");
    //    if (Cellclick == true)
    //    {
    //        PdfTextArea collinfo = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 20, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][0].ToString() + "");
    //        mypdfpage.Add(collinfo);
    //        collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 40, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][1].ToString() + "");
    //        mypdfpage.Add(collinfo);
    //        string address = ds.Tables[0].Rows[0][2].ToString() + "" + ds.Tables[0].Rows[0][3].ToString() + "" + ds.Tables[0].Rows[0][4].ToString();
    //        collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 55, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + address);
    //        mypdfpage.Add(collinfo);
    //        collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 70, 595, 50), System.Drawing.ContentAlignment.TopCenter, "E-mail  :" + ds.Tables[0].Rows[0][6].ToString());
    //        mypdfpage.Add(collinfo);
    //        PdfArea pa1 = new PdfArea(mydoc, 14, 12, 564, 821);
    //        PdfRectangle border = new PdfRectangle(mydoc, pa1, Color.Black);
    //        mypdfpage.Add(border);
    //        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
    //        {
    //            Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
    //            mypdfpage.Add(LogoImage, 50, 96, 450);
    //        }
    //        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
    //        {
    //            Gios.Pdf.PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
    //            mypdfpage.Add(LogoImage1, 280, 96, 450);
    //        }
    //        string exammonth = ddlMonth.SelectedValue.ToString();
    //        string examyear = ddlYear.SelectedValue.ToString();
    //        //RadioButton1.Visible = true;
    //        //RadioButton2.Visible = true;
    //        //RadioButton3.Visible = true;
    //        //RadioButton4.Visible = true;
    //        ddlformate.Visible = true;
    //        printbtn.Visible = false;
    //        string activerow = "";
    //        string activecol = "";
    //        string depart_code = "";
    //        int sno = 0;
    //        activerow = HAllSpread.ActiveSheetView.ActiveRow.ToString();
    //        activecol = HAllSpread.ActiveSheetView.ActiveColumn.ToString();
    //        // added by sridhar 11 sep 2014 ====start
    //        int totalrowssri = Convert.ToInt32(HAllSpread.Sheets[0].RowCount.ToString()) - 1;
    //        if (Convert.ToString(totalrowssri) == activerow)
    //        {
    //            return;
    //        }
    //        // added by sridhar 11 sep 2014 ====end
    //        string year = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
    //        string degree = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
    //        string course = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
    //        string studenttot = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text;
    //        depart_code = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Note;
    //        string sem = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
    //        string batchyearatt = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Note;
    //        string label1 = " " + degree + " " + course + " ";
    //        Label2.Text = label1;
    //        collinfo = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 70, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Exam Month & Year  :" + ddlMonth.SelectedItem.Text.ToString().ToUpper() + " & " + examyear);
    //        mypdfpage.Add(collinfo);
    //        collinfo = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 85, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Class & Group           : " + label1);
    //        mypdfpage.Add(collinfo);
    //        Session["semforsub"] = sem;
    //        Session["selecteddegreecode"] = depart_code;
    //        Session["selecteddegree"] = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
    //        Session["selectedcourse"] = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag;
    //        //=========
    //        lblno.Visible = false;
    //        int sprdvisibleflag = 0;
    //        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
    //        // string studinfo = "select len(r.reg_no),r.reg_no,r.stud_name,r.roll_no,r.batch_year from registration r where r.current_semester=" + sem + "and r.degree_code=" + depart_code + "  and cc=0 and delflag=0 and exam_flag<>'Debar' order by len(r.reg_no),r.reg_no,r.stud_name";
    //        string studinfo = "select distinct  r.reg_no,r.stud_name,r.roll_no,r.batch_year  from exam_details e,exam_application a,registration r,degree g,course c,department p        where e.exam_code = a.exam_code and a.roll_no = r.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id         and g.dept_code = p.dept_code          and c.college_code='" + Session["collegecode"].ToString() + "'  and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + "            and ltrim(r.roll_no) <>''      and cc=0 and delflag=0 and exam_flag<>'Debar'              and r.degree_code=" + depart_code + " and e.current_semester= " + sem + "  order by r.reg_no        ";
    //        int semval = Convert.ToInt16(sem) + Convert.ToInt16(1);
    //        SqlDataAdapter dastudinfo = new SqlDataAdapter(studinfo, con1);
    //        DataSet dsstudinfo = new DataSet();
    //        con1.Close();
    //        con1.Open();
    //        dastudinfo.Fill(dsstudinfo);
    //        Gios.Pdf.PdfTable studinfoss;
    //        int rowscc = 0;
    //        if (dsstudinfo.Tables[0].Rows.Count < 30)
    //        {
    //            studinfoss = mydoc.NewTable(Fontsmall1, dsstudinfo.Tables[0].Rows.Count + 6, 11, 5);
    //        }
    //        else
    //        {
    //            rowscc = dsstudinfo.Tables[0].Rows.Count;
    //            rowscc = rowscc - 30;
    //            studinfoss = mydoc.NewTable(Fontsmall1, 32, 11, 5);
    //        }
    //        studinfoss.VisibleHeaders = false;
    //        studinfoss.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
    //        studinfoss.Cell(0, 0).SetContent("S.No");
    //        studinfoss.Cell(0, 0).SetFont(Fontsmall1bold);
    //        studinfoss.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //        studinfoss.Cell(0, 1).SetContent("Name");
    //        studinfoss.Cell(0, 1).SetFont(Fontsmall1bold);
    //        studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //        studinfoss.Cell(0, 2).SetContent("Reg.No");
    //        studinfoss.Cell(0, 2).SetFont(Fontsmall1bold);
    //        studinfoss.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //        studinfoss.Cell(0, 3).SetContent("Paper");
    //        studinfoss.Cell(0, 3).SetFont(Fontsmall1bold);
    //        studinfoss.Cell(1, 3).SetContent("1");
    //        studinfoss.Cell(1, 3).SetFont(Fontsmall1bold);
    //        studinfoss.Cell(1, 4).SetContent("2");
    //        studinfoss.Cell(1, 4).SetFont(Fontsmall1bold);
    //        studinfoss.Cell(1, 5).SetContent("3");
    //        studinfoss.Cell(1, 5).SetFont(Fontsmall1bold);
    //        studinfoss.Cell(1, 6).SetContent("4");
    //        studinfoss.Cell(1, 6).SetFont(Fontsmall1bold);
    //        studinfoss.Cell(1, 7).SetContent("5");
    //        studinfoss.Cell(1, 7).SetFont(Fontsmall1bold);
    //        studinfoss.Cell(1, 8).SetContent("6");
    //        studinfoss.Cell(1, 8).SetFont(Fontsmall1bold);
    //        studinfoss.Cell(1, 9).SetContent("7");
    //        studinfoss.Cell(1, 9).SetFont(Fontsmall1bold);
    //        studinfoss.Cell(1, 10).SetContent("8");
    //        studinfoss.Cell(1, 10).SetFont(Fontsmall1bold);
    //        studinfoss.Columns[0].SetContentAlignment(ContentAlignment.MiddleCenter);
    //        studinfoss.Columns[2].SetContentAlignment(ContentAlignment.MiddleCenter);
    //        studinfoss.Columns[1].SetContentAlignment(ContentAlignment.MiddleLeft);
    //        studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //        studinfoss.Columns[0].SetWidth(8);
    //        studinfoss.Columns[1].SetWidth(30);
    //        studinfoss.Columns[2].SetWidth(15);
    //        foreach (PdfCell pr in studinfoss.CellRange(0, 0, 0, 0).Cells)
    //        {
    //            pr.RowSpan = 2;
    //        }
    //        foreach (PdfCell pr in studinfoss.CellRange(0, 1, 0, 1).Cells)
    //        {
    //            pr.RowSpan = 2;
    //        }
    //        foreach (PdfCell pr in studinfoss.CellRange(0, 2, 0, 2).Cells)
    //        {
    //            pr.RowSpan = 2;
    //        }
    //        foreach (PdfCell pr in studinfoss.CellRange(0, 3, 0, 3).Cells)
    //        {
    //            pr.ColSpan = 8;
    //        }
    //        int newtablerow = 0;
    //        Boolean finish = false;
    //        if (dsstudinfo.Tables[0].Rows.Count > 0)
    //        {
    //            for (int studcount = 0; studcount < dsstudinfo.Tables[0].Rows.Count; studcount++)
    //            {
    //                string regno = "";
    //                string studname = "";
    //                string rollno = "";
    //                string batchyear = "";
    //                Label2.Visible = true;
    //                printbtn.Visible = true;
    //                FpSpread2.Visible = true;
    //                batchyear = dsstudinfo.Tables[0].Rows[studcount]["batch_year"].ToString();
    //                regno = dsstudinfo.Tables[0].Rows[studcount]["reg_no"].ToString();
    //                studname = dsstudinfo.Tables[0].Rows[studcount]["stud_name"].ToString();
    //                rollno = dsstudinfo.Tables[0].Rows[studcount]["roll_no"].ToString();
    //                sno++;
    //                studinfoss.Cell(newtablerow + 2, 0).SetContent(sno);
    //                studinfoss.Cell(newtablerow + 2, 1).SetContent(studname);
    //                studinfoss.Cell(newtablerow + 2, 2).SetContent(regno);
    //                if (newtablerow == 29)
    //                {
    //                    int rowsccheck = rowscc - 30;
    //                    Gios.Pdf.PdfTablePage addtabletopage = studinfoss.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 120, 553, 800));
    //                    mypdfpage.Add(addtabletopage);
    //                    mypdfpage.SaveToDocument();
    //                    mypdfpage = mydoc.NewPage();
    //                    mypdfpage.Add(border);
    //                    if (rowsccheck > 0)
    //                    {
    //                        studinfoss = mydoc.NewTable(Fontsmall1, 32, 11, 5);
    //                        studinfoss.VisibleHeaders = false;
    //                        studinfoss.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
    //                        studinfoss.Cell(0, 0).SetContent("S.No");
    //                        studinfoss.Cell(0, 0).SetFont(Fontsmall1bold);
    //                        studinfoss.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        studinfoss.Cell(0, 1).SetContent("Name");
    //                        studinfoss.Cell(0, 1).SetFont(Fontsmall1bold);
    //                        studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        studinfoss.Cell(0, 2).SetContent("Reg.No");
    //                        studinfoss.Cell(0, 2).SetFont(Fontsmall1bold);
    //                        studinfoss.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        studinfoss.Cell(0, 3).SetContent("Paper");
    //                        studinfoss.Cell(0, 3).SetFont(Fontsmall1bold);
    //                        studinfoss.Cell(1, 3).SetContent("1");
    //                        studinfoss.Cell(1, 3).SetFont(Fontsmall1bold);
    //                        studinfoss.Cell(1, 4).SetContent("2");
    //                        studinfoss.Cell(1, 4).SetFont(Fontsmall1bold);
    //                        studinfoss.Cell(1, 5).SetContent("3");
    //                        studinfoss.Cell(1, 5).SetFont(Fontsmall1bold);
    //                        studinfoss.Cell(1, 6).SetContent("4");
    //                        studinfoss.Cell(1, 6).SetFont(Fontsmall1bold);
    //                        studinfoss.Cell(1, 7).SetContent("5");
    //                        studinfoss.Cell(1, 7).SetFont(Fontsmall1bold);
    //                        studinfoss.Cell(1, 8).SetContent("6");
    //                        studinfoss.Cell(1, 8).SetFont(Fontsmall1bold);
    //                        studinfoss.Cell(1, 9).SetContent("7");
    //                        studinfoss.Cell(1, 9).SetFont(Fontsmall1bold);
    //                        studinfoss.Cell(1, 10).SetContent("8");
    //                        studinfoss.Cell(1, 10).SetFont(Fontsmall1bold);
    //                        studinfoss.Columns[0].SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        studinfoss.Columns[2].SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        studinfoss.Columns[1].SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        studinfoss.Columns[0].SetWidth(8);
    //                        studinfoss.Columns[1].SetWidth(30);
    //                        studinfoss.Columns[2].SetWidth(15);
    //                        foreach (PdfCell pr in studinfoss.CellRange(0, 0, 0, 0).Cells)
    //                        {
    //                            pr.RowSpan = 2;
    //                        }
    //                        foreach (PdfCell pr in studinfoss.CellRange(0, 1, 0, 1).Cells)
    //                        {
    //                            pr.RowSpan = 2;
    //                        }
    //                        foreach (PdfCell pr in studinfoss.CellRange(0, 2, 0, 2).Cells)
    //                        {
    //                            pr.RowSpan = 2;
    //                        }
    //                        foreach (PdfCell pr in studinfoss.CellRange(0, 3, 0, 3).Cells)
    //                        {
    //                            pr.ColSpan = 8;
    //                        }
    //                        rowscc = rowscc - 30;
    //                    }
    //                    else if (rowsccheck != -30)
    //                    {
    //                        studinfoss = mydoc.NewTable(Fontsmall1, rowscc + 6, 11, 5);
    //                        studinfoss.VisibleHeaders = false;
    //                        studinfoss.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
    //                        studinfoss.Cell(0, 0).SetContent("S.No");
    //                        studinfoss.Cell(0, 0).SetFont(Fontsmall1bold);
    //                        studinfoss.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        studinfoss.Cell(0, 1).SetContent("Name");
    //                        studinfoss.Cell(0, 1).SetFont(Fontsmall1bold);
    //                        studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        studinfoss.Cell(0, 2).SetContent("Reg.No");
    //                        studinfoss.Cell(0, 2).SetFont(Fontsmall1bold);
    //                        studinfoss.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        studinfoss.Cell(0, 3).SetContent("Paper");
    //                        studinfoss.Cell(0, 3).SetFont(Fontsmall1bold);
    //                        studinfoss.Cell(1, 3).SetContent("1");
    //                        studinfoss.Cell(1, 3).SetFont(Fontsmall1bold);
    //                        studinfoss.Cell(1, 4).SetContent("2");
    //                        studinfoss.Cell(1, 4).SetFont(Fontsmall1bold);
    //                        studinfoss.Cell(1, 5).SetContent("3");
    //                        studinfoss.Cell(1, 5).SetFont(Fontsmall1bold);
    //                        studinfoss.Cell(1, 6).SetContent("4");
    //                        studinfoss.Cell(1, 6).SetFont(Fontsmall1bold);
    //                        studinfoss.Cell(1, 7).SetContent("5");
    //                        studinfoss.Cell(1, 7).SetFont(Fontsmall1bold);
    //                        studinfoss.Cell(1, 8).SetContent("6");
    //                        studinfoss.Cell(1, 8).SetFont(Fontsmall1bold);
    //                        studinfoss.Cell(1, 9).SetContent("7");
    //                        studinfoss.Cell(1, 9).SetFont(Fontsmall1bold);
    //                        studinfoss.Cell(1, 10).SetContent("8");
    //                        studinfoss.Cell(1, 10).SetFont(Fontsmall1bold);
    //                        studinfoss.Columns[0].SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        studinfoss.Columns[2].SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        studinfoss.Columns[1].SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        studinfoss.Columns[0].SetWidth(8);
    //                        studinfoss.Columns[1].SetWidth(30);
    //                        studinfoss.Columns[2].SetWidth(15);
    //                        foreach (PdfCell pr in studinfoss.CellRange(0, 0, 0, 0).Cells)
    //                        {
    //                            pr.RowSpan = 2;
    //                        }
    //                        foreach (PdfCell pr in studinfoss.CellRange(0, 1, 0, 1).Cells)
    //                        {
    //                            pr.RowSpan = 2;
    //                        }
    //                        foreach (PdfCell pr in studinfoss.CellRange(0, 2, 0, 2).Cells)
    //                        {
    //                            pr.RowSpan = 2;
    //                        }
    //                        foreach (PdfCell pr in studinfoss.CellRange(0, 3, 0, 3).Cells)
    //                        {
    //                            pr.ColSpan = 8;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        studinfoss = mydoc.NewTable(Fontsmall1, rowscc + 4, 11, 5);
    //                        studinfoss.VisibleHeaders = false;
    //                        studinfoss.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
    //                        finish = true;
    //                    }
    //                    newtablerow = -1;
    //                }
    //                newtablerow++;
    //            }
    //            if (finish == false)
    //            {
    //                studinfoss.Cell(newtablerow + 2, 0).SetContent("Paper 1 :");
    //                studinfoss.Cell(newtablerow + 2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                studinfoss.Cell(newtablerow + 3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                studinfoss.Cell(newtablerow + 4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                studinfoss.Cell(newtablerow + 5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 2, 0, newtablerow + 2, 0).Cells)
    //                {
    //                    pr.ColSpan = 5;
    //                }
    //                studinfoss.Cell(newtablerow + 3, 0).SetContent("Paper 2 : ");
    //                foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 3, 0, newtablerow + 3, 0).Cells)
    //                {
    //                    pr.ColSpan = 5;
    //                }
    //                studinfoss.Cell(newtablerow + 4, 0).SetContent("Paper 3 : ");
    //                foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 4, 0, newtablerow + 4, 0).Cells)
    //                {
    //                    pr.ColSpan = 5;
    //                }
    //                studinfoss.Cell(newtablerow + 5, 0).SetContent("Paper 4 : ");
    //                foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 5, 0, newtablerow + 5, 0).Cells)
    //                {
    //                    pr.ColSpan = 5;
    //                }
    //                studinfoss.Cell(newtablerow + 2, 5).SetContent("Paper 5 : ");
    //                foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 2, 5, newtablerow + 2, 5).Cells)
    //                {
    //                    pr.ColSpan = 6;
    //                }
    //                studinfoss.Cell(newtablerow + 3, 5).SetContent("Paper 6 : ");
    //                foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 3, 5, newtablerow + 3, 5).Cells)
    //                {
    //                    pr.ColSpan = 6;
    //                }
    //                studinfoss.Cell(newtablerow + 4, 5).SetContent("Paper 7 : ");
    //                foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 4, 5, newtablerow + 4, 5).Cells)
    //                {
    //                    pr.ColSpan = 6;
    //                }
    //                studinfoss.Cell(newtablerow + 5, 5).SetContent("Paper 8 : ");
    //                foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 5, 5, newtablerow + 5, 5).Cells)
    //                {
    //                    pr.ColSpan = 6;
    //                }
    //                studinfoss.Cell(newtablerow + 2, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                studinfoss.Cell(newtablerow + 3, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                studinfoss.Cell(newtablerow + 4, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                studinfoss.Cell(newtablerow + 5, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                Gios.Pdf.PdfTablePage addtabletopage001 = studinfoss.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 120, 553, 800));
    //                mypdfpage.Add(addtabletopage001);
    //                mypdfpage.SaveToDocument();
    //            }
    //            else
    //            {
    //                studinfoss.Cell(newtablerow + 0, 0).SetContent("Paper 1 :");
    //                studinfoss.Cell(newtablerow + 0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                studinfoss.Cell(newtablerow + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                studinfoss.Cell(newtablerow + 2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                studinfoss.Cell(newtablerow + 3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 0, 0, newtablerow + 0, 0).Cells)
    //                {
    //                    pr.ColSpan = 5;
    //                }
    //                studinfoss.Cell(newtablerow + 1, 0).SetContent("Paper 2 : ");
    //                foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 1, 0, newtablerow + 1, 0).Cells)
    //                {
    //                    pr.ColSpan = 5;
    //                }
    //                studinfoss.Cell(newtablerow + 2, 0).SetContent("Paper 3 : ");
    //                foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 2, 0, newtablerow + 2, 0).Cells)
    //                {
    //                    pr.ColSpan = 5;
    //                }
    //                studinfoss.Cell(newtablerow + 3, 0).SetContent("Paper 4 : ");
    //                foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 3, 0, newtablerow + 3, 0).Cells)
    //                {
    //                    pr.ColSpan = 5;
    //                }
    //                studinfoss.Cell(newtablerow + 0, 5).SetContent("Paper 5 : ");
    //                foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 0, 5, newtablerow + 0, 5).Cells)
    //                {
    //                    pr.ColSpan = 6;
    //                }
    //                studinfoss.Cell(newtablerow + 1, 5).SetContent("Paper 6 : ");
    //                foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 1, 5, newtablerow + 1, 5).Cells)
    //                {
    //                    pr.ColSpan = 6;
    //                }
    //                studinfoss.Cell(newtablerow + 2, 5).SetContent("Paper 7 : ");
    //                foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 2, 5, newtablerow + 2, 5).Cells)
    //                {
    //                    pr.ColSpan = 6;
    //                }
    //                studinfoss.Cell(newtablerow + 3, 5).SetContent("Paper 8 : ");
    //                foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 3, 5, newtablerow + 3, 5).Cells)
    //                {
    //                    pr.ColSpan = 6;
    //                }
    //                studinfoss.Cell(newtablerow + 0, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                studinfoss.Cell(newtablerow + 1, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                studinfoss.Cell(newtablerow + 2, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                studinfoss.Cell(newtablerow + 3, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                Gios.Pdf.PdfTablePage addtabletopage001 = studinfoss.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 120, 553, 800));
    //                mypdfpage.Add(addtabletopage001);
    //                mypdfpage.SaveToDocument();
    //            }
    //        }
    //        string appPath = HttpContext.Current.Server.MapPath("~");
    //        if (appPath != "")
    //        {
    //            string szPath = appPath + "/Report/";
    //            string szFile = "Marksheets" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
    //            Response.Buffer = true;
    //            Response.Clear();
    //            mydoc.SaveToFile(szPath + szFile);
    //            Response.ClearHeaders();
    //            Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
    //            Response.ContentType = "application/pdf";
    //            Response.WriteFile(szPath + szFile);
    //        }
    //    }
    //}

    public void bindsubjectpdf()
    {
        try
        {
            string typefilter = "";
            if (ddledulevel.Items.Count > 0 && ddlcoltypeadd.Items.Count > 0)
            {
                typefilter = "and c.Edu_Level='" + ddledulevel.SelectedItem.Text.ToString() + "' and c.type='" + ddlcoltypeadd.SelectedItem.Text.ToString() + "' ";
            }
            FpSpread2.SaveChanges();
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
            System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
            System.Drawing.Font Fontbold1 = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Bold);
            System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
            System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
            System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
            System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
            System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
            System.Drawing.Font Fontsmall1bold = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
            System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            Gios.Pdf.PdfPage mypdfpage;
            for (int iy = 1; iy < FpSpread2.Sheets[0].RowCount; iy++)
            {
                if (Convert.ToInt32(FpSpread2.Sheets[0].Cells[iy, 2].Value) == 1)
                {
                    //string subjectcccode = ddlsubject.Items[iy].Value.ToString();
                    //string subjectcctext = ddlsubject.Items[iy].Text.ToString();
                    string subjectcccode = FpSpread2.Sheets[0].Cells[iy, 1].Tag.ToString();
                    string subjectcctext = FpSpread2.Sheets[0].Cells[iy, 1].Text.ToString();
                    string semesters = "";
                    string batchyearcal = "";
                    if (dropterm.Items.Count > 0)
                    {
                        semesters = "and sc.semester='" + dropterm.SelectedItem.Text.ToString() + "'";
                        if (Convert.ToInt32(dropterm.SelectedItem.Text.ToString()) % 2 == 0)
                        {
                            batchyearcal = da.GetFunction("  select max(Batch_Year) from Registration where Current_Semester between " + (Convert.ToInt32(dropterm.SelectedItem.Text.ToString()) - 1) + " and " + Convert.ToInt32(dropterm.SelectedItem.Text.ToString()) + "");
                        }
                        else
                        {
                            batchyearcal = da.GetFunction("  select max(Batch_Year) from Registration where Current_Semester between " + (Convert.ToInt32(dropterm.SelectedItem.Text.ToString())) + " and " + (Convert.ToInt32(dropterm.SelectedItem.Text.ToString()) + 1) + "");
                        }
                        batchyearcal = batchyearcal + "," + Convert.ToString(Convert.ToInt32(batchyearcal) - 1);
                    }
                    if (ddlsec.Items.Count > 0)
                    {
                        if (ddlsec.SelectedIndex != 0)
                        {
                            sectionddl = "and r.sections='" + ddlsec.SelectedItem.Text.ToString() + "'";
                            grpsectionddl = "r.sections";
                        }
                    }
                    mypdfpage = mydoc.NewPage();
                    //Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
                    string sql = "SELECT collname,affliatedby,address1,address2,address3,pincode,email,logo1,logo2,email  from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
                    DataSet ds = new DataSet();
                    ds.Clear();
                    ds.Dispose();
                    ds = da.select_method_wo_parameter(sql, "Text");
                    HAllSpread.SaveChanges();
                    PdfTextArea collinfo = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 20, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][0].ToString() + " (AUTONOMOUS)");
                    mypdfpage.Add(collinfo);
                    //collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 40, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][1].ToString() + "");
                    //mypdfpage.Add(collinfo);
                    string address = ds.Tables[0].Rows[0][2].ToString() + "" + ds.Tables[0].Rows[0][3].ToString() + "" + ds.Tables[0].Rows[0][4].ToString() + " - " + ds.Tables[0].Rows[0][5].ToString();
                    collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 55, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + address);
                    mypdfpage.Add(collinfo);
                    //collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 70, 595, 50), System.Drawing.ContentAlignment.TopCenter, "E-mail  :" + ds.Tables[0].Rows[0][6].ToString());
                    //mypdfpage.Add(collinfo);
                    PdfArea pa1 = new PdfArea(mydoc, 14, 12, 564, 821);
                    PdfRectangle border = new PdfRectangle(mydoc, pa1, Color.Black);
                    mypdfpage.Add(border);
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                    {
                        Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                        mypdfpage.Add(LogoImage, 20, 20, 450);
                    }
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                    {
                        Gios.Pdf.PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                        mypdfpage.Add(LogoImage1, 500, 20, 450);
                    }
                    string exammonth = ddlMonth.SelectedValue.ToString();
                    string examyear = ddlYear.SelectedValue.ToString();
                    //RadioButton1.Visible = true;
                    //RadioButton2.Visible = true;
                    //RadioButton3.Visible = true;
                    //RadioButton4.Visible = true;
                    ddlformate.Visible = true;
                    printbtn.Visible = false;
                    string activerow = "";
                    string activecol = "";
                    string depart_code = "";
                    int sno = 0;
                    //activecol = HAllSpread.ActiveSheetView.ActiveColumn.ToString();
                    // added by sridhar 11 sep 2014 ====start
                    int totalrowssri = Convert.ToInt32(HAllSpread.Sheets[0].RowCount.ToString()) - 1;
                    if (Convert.ToString(totalrowssri) == activerow)
                    {
                        return;
                    }
                    // added by sridhar 11 sep 2014 ====end
                    //string year = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                    //string degree = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                    //string course = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                    //string studenttot = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text;
                    //depart_code = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Note;
                    //string sem = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
                    //string batchyearatt = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Note;
                    //string label1 = " " + degree + " " + course + " ";
                    //Label2.Text = label1;
                    collinfo = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 70, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Exam Month & Year  : " + ddlMonth.SelectedItem.Text.ToString().ToUpper() + "  " + examyear);
                    mypdfpage.Add(collinfo);
                    collinfo = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 85, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Subject                      : " + subjectcctext);
                    mypdfpage.Add(collinfo);
                    //=========
                    lblno.Visible = false;
                    int sprdvisibleflag = 0;
                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                    // string studinfo = "select len(r.reg_no),r.reg_no,r.stud_name,r.roll_no,r.batch_year from registration r where r.current_semester=" + sem + "and r.degree_code=" + depart_code + "  and cc=0 and delflag=0 and exam_flag<>'Debar' order by len(r.reg_no),r.reg_no,r.stud_name";
                    //string studinfo = "select distinct  r.reg_no,r.stud_name,r.roll_no,r.batch_year  from exam_details e,exam_application a,registration r,degree g,course c,department p        where e.exam_code = a.exam_code and a.roll_no = r.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id         and g.dept_code = p.dept_code          and c.college_code='" + Session["collegecode"].ToString() + "'  and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + "            and ltrim(r.roll_no) <>''      and cc=0 and delflag=0 and exam_flag<>'Debar'              and r.degree_code=" + depart_code + " and e.current_semester= " + sem + " " + sectionddl + "  order by r.reg_no        ";
                    string studinfo = "select r.Stud_Name,r.Reg_No from course c , Degree d, subjectChooser sc,subject s,Registration r where r.Roll_No=sc.roll_no  and s.subject_no=sc.subject_no and c.Course_Id=d.Course_Id and d.Degree_Code=r.degree_code and cc=0 and delflag=0 and exam_flag<>'Debar' and r.current_semester=sc.semester  and s.subject_code='" + subjectcccode + "' and r.Batch_Year in (" + batchyearcal + ")  " + typefilter + " " + sectionddl + " " + semesters + "  order by r.reg_no  ";
                    SqlDataAdapter dastudinfo = new SqlDataAdapter(studinfo, con1);
                    DataSet dsstudinfo = new DataSet();
                    con1.Close();
                    con1.Open();
                    dastudinfo.Fill(dsstudinfo);
                    Gios.Pdf.PdfTable studinfoss;
                    int rowscc = 0;
                    if (dsstudinfo.Tables[0].Rows.Count < 30)
                    {
                        studinfoss = mydoc.NewTable(Fontsmall1, dsstudinfo.Tables[0].Rows.Count + 6, 11, 5);
                    }
                    else
                    {
                        rowscc = dsstudinfo.Tables[0].Rows.Count;
                        rowscc = rowscc - 30;
                        studinfoss = mydoc.NewTable(Fontsmall1, 32, 11, 5);
                    }
                    studinfoss.VisibleHeaders = false;
                    studinfoss.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                    studinfoss.Cell(0, 0).SetContent("S.No");
                    studinfoss.Cell(0, 0).SetFont(Fontsmall1bold);
                    studinfoss.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                    studinfoss.Cell(0, 1).SetContent("Name");
                    studinfoss.Cell(0, 1).SetFont(Fontsmall1bold);
                    studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    studinfoss.Cell(0, 2).SetContent("Reg.No");
                    studinfoss.Cell(0, 2).SetFont(Fontsmall1bold);
                    studinfoss.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                    studinfoss.Cell(0, 3).SetContent("Paper (Max. Mks. : 50)");
                    studinfoss.Cell(0, 3).SetFont(Fontsmall1bold);
                    studinfoss.Cell(1, 3).SetContent("1");
                    studinfoss.Cell(1, 3).SetFont(Fontsmall1bold);
                    studinfoss.Cell(1, 4).SetContent("2");
                    studinfoss.Cell(1, 4).SetFont(Fontsmall1bold);
                    studinfoss.Cell(1, 5).SetContent("3");
                    studinfoss.Cell(1, 5).SetFont(Fontsmall1bold);
                    studinfoss.Cell(1, 6).SetContent("4");
                    studinfoss.Cell(1, 6).SetFont(Fontsmall1bold);
                    studinfoss.Cell(1, 7).SetContent("5");
                    studinfoss.Cell(1, 7).SetFont(Fontsmall1bold);
                    studinfoss.Cell(1, 8).SetContent("6");
                    studinfoss.Cell(1, 8).SetFont(Fontsmall1bold);
                    studinfoss.Cell(1, 9).SetContent("7");
                    studinfoss.Cell(1, 9).SetFont(Fontsmall1bold);
                    studinfoss.Cell(1, 10).SetContent("8");
                    studinfoss.Cell(1, 10).SetFont(Fontsmall1bold);
                    studinfoss.Columns[0].SetContentAlignment(ContentAlignment.MiddleCenter);
                    studinfoss.Columns[2].SetContentAlignment(ContentAlignment.MiddleCenter);
                    studinfoss.Columns[1].SetContentAlignment(ContentAlignment.MiddleLeft);
                    studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    studinfoss.Columns[0].SetWidth(8);
                    studinfoss.Columns[1].SetWidth(40);
                    studinfoss.Columns[2].SetWidth(25);
                    foreach (PdfCell pr in studinfoss.CellRange(0, 0, 0, 0).Cells)
                    {
                        pr.RowSpan = 2;
                    }
                    foreach (PdfCell pr in studinfoss.CellRange(0, 1, 0, 1).Cells)
                    {
                        pr.RowSpan = 2;
                    }
                    foreach (PdfCell pr in studinfoss.CellRange(0, 2, 0, 2).Cells)
                    {
                        pr.RowSpan = 2;
                    }
                    foreach (PdfCell pr in studinfoss.CellRange(0, 3, 0, 3).Cells)
                    {
                        pr.ColSpan = 8;
                    }
                    int newtablerow = 0;
                    Boolean finish = false;
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
                            // batchyear = dsstudinfo.Tables[0].Rows[studcount]["batch_year"].ToString();
                            regno = dsstudinfo.Tables[0].Rows[studcount]["Reg_No"].ToString();
                            studname = dsstudinfo.Tables[0].Rows[studcount]["stud_name"].ToString();
                            //rollno = dsstudinfo.Tables[0].Rows[studcount]["roll_no"].ToString();
                            sno++;
                            studinfoss.Cell(newtablerow + 2, 0).SetContent(sno);
                            studinfoss.Cell(newtablerow + 2, 1).SetContent(studname);
                            studinfoss.Cell(newtablerow + 2, 2).SetContent(regno);
                            if (newtablerow == 29)
                            {
                                int rowsccheck = rowscc - 30;
                                Gios.Pdf.PdfTablePage addtabletopage = studinfoss.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 120, 553, 800));
                                mypdfpage.Add(addtabletopage);
                                mypdfpage.SaveToDocument();
                                mypdfpage = mydoc.NewPage();
                                collinfo = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 20, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][0].ToString() + " (AUTONOMOUS)");
                                mypdfpage.Add(collinfo);
                                //collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 40, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][1].ToString() + "");
                                //mypdfpage.Add(collinfo);
                                collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 55, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + address);
                                mypdfpage.Add(collinfo);
                                //collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 70, 595, 50), System.Drawing.ContentAlignment.TopCenter, "E-mail  : " + ds.Tables[0].Rows[0][6].ToString());
                                //mypdfpage.Add(collinfo);
                                collinfo = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 70, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Exam Month & Year  : " + ddlMonth.SelectedItem.Text.ToString().ToUpper() + "  " + examyear);
                                mypdfpage.Add(collinfo);
                                collinfo = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 85, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Subject          : " + subjectcctext);
                                mypdfpage.Add(collinfo);
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                {
                                    Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                    mypdfpage.Add(LogoImage, 20, 20, 450);
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                {
                                    Gios.Pdf.PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                    mypdfpage.Add(LogoImage1, 500, 20, 450);
                                }
                                mypdfpage.Add(border);
                                if (rowsccheck > 0)
                                {
                                    studinfoss = mydoc.NewTable(Fontsmall1, 32, 11, 5);
                                    studinfoss.VisibleHeaders = false;
                                    studinfoss.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    studinfoss.Cell(0, 0).SetContent("S.No");
                                    studinfoss.Cell(0, 0).SetFont(Fontsmall1bold);
                                    studinfoss.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    studinfoss.Cell(0, 1).SetContent("Name");
                                    studinfoss.Cell(0, 1).SetFont(Fontsmall1bold);
                                    studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    studinfoss.Cell(0, 2).SetContent("Reg.No");
                                    studinfoss.Cell(0, 2).SetFont(Fontsmall1bold);
                                    studinfoss.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    studinfoss.Cell(0, 3).SetContent("Paper (Max. Mks. : 50)");
                                    studinfoss.Cell(0, 3).SetFont(Fontsmall1bold);
                                    studinfoss.Cell(1, 3).SetContent("1");
                                    studinfoss.Cell(1, 3).SetFont(Fontsmall1bold);
                                    studinfoss.Cell(1, 4).SetContent("2");
                                    studinfoss.Cell(1, 4).SetFont(Fontsmall1bold);
                                    studinfoss.Cell(1, 5).SetContent("3");
                                    studinfoss.Cell(1, 5).SetFont(Fontsmall1bold);
                                    studinfoss.Cell(1, 6).SetContent("4");
                                    studinfoss.Cell(1, 6).SetFont(Fontsmall1bold);
                                    studinfoss.Cell(1, 7).SetContent("5");
                                    studinfoss.Cell(1, 7).SetFont(Fontsmall1bold);
                                    studinfoss.Cell(1, 8).SetContent("6");
                                    studinfoss.Cell(1, 8).SetFont(Fontsmall1bold);
                                    studinfoss.Cell(1, 9).SetContent("7");
                                    studinfoss.Cell(1, 9).SetFont(Fontsmall1bold);
                                    studinfoss.Cell(1, 10).SetContent("8");
                                    studinfoss.Cell(1, 10).SetFont(Fontsmall1bold);
                                    studinfoss.Columns[0].SetContentAlignment(ContentAlignment.MiddleCenter);
                                    studinfoss.Columns[2].SetContentAlignment(ContentAlignment.MiddleCenter);
                                    studinfoss.Columns[1].SetContentAlignment(ContentAlignment.MiddleLeft);
                                    studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    studinfoss.Columns[0].SetWidth(8);
                                    studinfoss.Columns[1].SetWidth(40);
                                    studinfoss.Columns[2].SetWidth(25);
                                    foreach (PdfCell pr in studinfoss.CellRange(0, 0, 0, 0).Cells)
                                    {
                                        pr.RowSpan = 2;
                                    }
                                    foreach (PdfCell pr in studinfoss.CellRange(0, 1, 0, 1).Cells)
                                    {
                                        pr.RowSpan = 2;
                                    }
                                    foreach (PdfCell pr in studinfoss.CellRange(0, 2, 0, 2).Cells)
                                    {
                                        pr.RowSpan = 2;
                                    }
                                    foreach (PdfCell pr in studinfoss.CellRange(0, 3, 0, 3).Cells)
                                    {
                                        pr.ColSpan = 8;
                                    }
                                    rowscc = rowscc - 30;
                                }
                                else if (rowsccheck != -30)
                                {
                                    studinfoss = mydoc.NewTable(Fontsmall1, rowscc + 6, 11, 5);
                                    studinfoss.VisibleHeaders = false;
                                    studinfoss.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    studinfoss.Cell(0, 0).SetContent("S.No");
                                    studinfoss.Cell(0, 0).SetFont(Fontsmall1bold);
                                    studinfoss.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    studinfoss.Cell(0, 1).SetContent("Name");
                                    studinfoss.Cell(0, 1).SetFont(Fontsmall1bold);
                                    studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    studinfoss.Cell(0, 2).SetContent("Reg.No");
                                    studinfoss.Cell(0, 2).SetFont(Fontsmall1bold);
                                    studinfoss.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    studinfoss.Cell(0, 3).SetContent("Paper (Max. Mks. : 50)");
                                    studinfoss.Cell(0, 3).SetFont(Fontsmall1bold);
                                    studinfoss.Cell(1, 3).SetContent("1");
                                    studinfoss.Cell(1, 3).SetFont(Fontsmall1bold);
                                    studinfoss.Cell(1, 4).SetContent("2");
                                    studinfoss.Cell(1, 4).SetFont(Fontsmall1bold);
                                    studinfoss.Cell(1, 5).SetContent("3");
                                    studinfoss.Cell(1, 5).SetFont(Fontsmall1bold);
                                    studinfoss.Cell(1, 6).SetContent("4");
                                    studinfoss.Cell(1, 6).SetFont(Fontsmall1bold);
                                    studinfoss.Cell(1, 7).SetContent("5");
                                    studinfoss.Cell(1, 7).SetFont(Fontsmall1bold);
                                    studinfoss.Cell(1, 8).SetContent("6");
                                    studinfoss.Cell(1, 8).SetFont(Fontsmall1bold);
                                    studinfoss.Cell(1, 9).SetContent("7");
                                    studinfoss.Cell(1, 9).SetFont(Fontsmall1bold);
                                    studinfoss.Cell(1, 10).SetContent("8");
                                    studinfoss.Cell(1, 10).SetFont(Fontsmall1bold);
                                    studinfoss.Columns[0].SetContentAlignment(ContentAlignment.MiddleCenter);
                                    studinfoss.Columns[2].SetContentAlignment(ContentAlignment.MiddleCenter);
                                    studinfoss.Columns[1].SetContentAlignment(ContentAlignment.MiddleLeft);
                                    studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    studinfoss.Columns[0].SetWidth(8);
                                    studinfoss.Columns[1].SetWidth(40);
                                    studinfoss.Columns[2].SetWidth(25);
                                    foreach (PdfCell pr in studinfoss.CellRange(0, 0, 0, 0).Cells)
                                    {
                                        pr.RowSpan = 2;
                                    }
                                    foreach (PdfCell pr in studinfoss.CellRange(0, 1, 0, 1).Cells)
                                    {
                                        pr.RowSpan = 2;
                                    }
                                    foreach (PdfCell pr in studinfoss.CellRange(0, 2, 0, 2).Cells)
                                    {
                                        pr.RowSpan = 2;
                                    }
                                    foreach (PdfCell pr in studinfoss.CellRange(0, 3, 0, 3).Cells)
                                    {
                                        pr.ColSpan = 8;
                                    }
                                }
                                else
                                {
                                    studinfoss = mydoc.NewTable(Fontsmall1, rowscc + 4, 11, 5);
                                    studinfoss.VisibleHeaders = false;
                                    studinfoss.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    finish = true;
                                }
                                newtablerow = -1;
                            }
                            newtablerow++;
                        }
                        if (finish == false)
                        {
                            studinfoss.Cell(newtablerow + 2, 0).SetContent("Paper 1 :");
                            studinfoss.Cell(newtablerow + 2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            studinfoss.Cell(newtablerow + 3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            studinfoss.Cell(newtablerow + 4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            studinfoss.Cell(newtablerow + 5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 2, 0, newtablerow + 2, 0).Cells)
                            {
                                pr.ColSpan = 5;
                            }
                            studinfoss.Cell(newtablerow + 3, 0).SetContent("Paper 2 : ");
                            foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 3, 0, newtablerow + 3, 0).Cells)
                            {
                                pr.ColSpan = 5;
                            }
                            studinfoss.Cell(newtablerow + 4, 0).SetContent("Paper 3 : ");
                            foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 4, 0, newtablerow + 4, 0).Cells)
                            {
                                pr.ColSpan = 5;
                            }
                            studinfoss.Cell(newtablerow + 5, 0).SetContent("Paper 4 : ");
                            foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 5, 0, newtablerow + 5, 0).Cells)
                            {
                                pr.ColSpan = 5;
                            }
                            studinfoss.Cell(newtablerow + 2, 5).SetContent("Paper 5 : ");
                            foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 2, 5, newtablerow + 2, 5).Cells)
                            {
                                pr.ColSpan = 6;
                            }
                            studinfoss.Cell(newtablerow + 3, 5).SetContent("Paper 6 : ");
                            foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 3, 5, newtablerow + 3, 5).Cells)
                            {
                                pr.ColSpan = 6;
                            }
                            studinfoss.Cell(newtablerow + 4, 5).SetContent("Paper 7 : ");
                            foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 4, 5, newtablerow + 4, 5).Cells)
                            {
                                pr.ColSpan = 6;
                            }
                            studinfoss.Cell(newtablerow + 5, 5).SetContent("Paper 8 : ");
                            foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 5, 5, newtablerow + 5, 5).Cells)
                            {
                                pr.ColSpan = 6;
                            }
                            studinfoss.Cell(newtablerow + 2, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            studinfoss.Cell(newtablerow + 3, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            studinfoss.Cell(newtablerow + 4, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            studinfoss.Cell(newtablerow + 5, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            Gios.Pdf.PdfTablePage addtabletopage001 = studinfoss.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 120, 553, 800));
                            mypdfpage.Add(addtabletopage001);
                            double additionaldata = addtabletopage001.Area.Height;
                            collinfo = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 35, additionaldata + 145, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "DATE : ");
                            mypdfpage.Add(collinfo);
                            collinfo = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 370, additionaldata + 145, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "SIGNATURE : ");
                            mypdfpage.Add(collinfo);
                            mypdfpage.SaveToDocument();
                            mypdfpage = mydoc.NewPage();
                        }
                        else
                        {
                            studinfoss.Cell(newtablerow + 0, 0).SetContent("Paper 1 :");
                            studinfoss.Cell(newtablerow + 0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            studinfoss.Cell(newtablerow + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            studinfoss.Cell(newtablerow + 2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            studinfoss.Cell(newtablerow + 3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 0, 0, newtablerow + 0, 0).Cells)
                            {
                                pr.ColSpan = 5;
                            }
                            studinfoss.Cell(newtablerow + 1, 0).SetContent("Paper 2 : ");
                            foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 1, 0, newtablerow + 1, 0).Cells)
                            {
                                pr.ColSpan = 5;
                            }
                            studinfoss.Cell(newtablerow + 2, 0).SetContent("Paper 3 : ");
                            foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 2, 0, newtablerow + 2, 0).Cells)
                            {
                                pr.ColSpan = 5;
                            }
                            studinfoss.Cell(newtablerow + 3, 0).SetContent("Paper 4 : ");
                            foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 3, 0, newtablerow + 3, 0).Cells)
                            {
                                pr.ColSpan = 5;
                            }
                            studinfoss.Cell(newtablerow + 0, 5).SetContent("Paper 5 : ");
                            foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 0, 5, newtablerow + 0, 5).Cells)
                            {
                                pr.ColSpan = 6;
                            }
                            studinfoss.Cell(newtablerow + 1, 5).SetContent("Paper 6 : ");
                            foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 1, 5, newtablerow + 1, 5).Cells)
                            {
                                pr.ColSpan = 6;
                            }
                            studinfoss.Cell(newtablerow + 2, 5).SetContent("Paper 7 : ");
                            foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 2, 5, newtablerow + 2, 5).Cells)
                            {
                                pr.ColSpan = 6;
                            }
                            studinfoss.Cell(newtablerow + 3, 5).SetContent("Paper 8 : ");
                            foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 3, 5, newtablerow + 3, 5).Cells)
                            {
                                pr.ColSpan = 6;
                            }
                            studinfoss.Cell(newtablerow + 0, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            studinfoss.Cell(newtablerow + 1, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            studinfoss.Cell(newtablerow + 2, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            studinfoss.Cell(newtablerow + 3, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            Gios.Pdf.PdfTablePage addtabletopage001 = studinfoss.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 120, 553, 800));
                            mypdfpage.Add(addtabletopage001);
                            mypdfpage.SaveToDocument();
                            mypdfpage = mydoc.NewPage();
                        }
                    }
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Records Found')", true);
                    //    return;
                    //}
                    // mypdfpage.SaveToDocument();
                    // mypdfpage.SaveToDocument();
                }
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "Marksheets" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                Response.Buffer = true;
                Response.Clear();
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

    public void bindppdf()
    {
        sectionddl = "";
        grpsectionddl = "";
        string secval = "";
        if (ddlsec.Items.Count > 0)
        {
            if (ddlsec.SelectedIndex != 0)
            {
                sectionddl = "and r.sections='" + ddlsec.SelectedItem.Text.ToString() + "'";
                grpsectionddl = "(Section  " + ddlsec.SelectedItem.Text.ToString() + " )";
                secval = " and sections='" + ddlsec.SelectedItem.Text.ToString() + "'";
            }
        }
        double additionaldata = 0;
        Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
        System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
        System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
        System.Drawing.Font Fontbold1 = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Bold);
        System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
        System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
        System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
        System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
        System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
        System.Drawing.Font Fontsmall1bold = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
        System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);
        //Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
        string sql = "SELECT collname,affliatedby,address1,address2,address3,pincode,email,logo1,logo2,email  from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
        DataSet ds = new DataSet();
        ds.Clear();
        ds.Dispose();
        ds = da.select_method_wo_parameter(sql, "Text");
        HAllSpread.SaveChanges();
        for (int rowii = 1; rowii < HAllSpread.Sheets[0].RowCount - 1; rowii++)
        {
            int checkedcell = Convert.ToInt32(HAllSpread.Sheets[0].Cells[rowii, 6].Value.ToString());
            if (checkedcell == 1)
            {
                Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
                PdfTextArea collinfo = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 20, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][0].ToString() + " (AUTONOMOUS)");
                mypdfpage.Add(collinfo);
                //collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 40, 595, 50), System.Drawing.ContentAlignment.TopCenter, "Affiliated to University of Madras");
                //mypdfpage.Add(collinfo);
                collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 40, 595, 50), System.Drawing.ContentAlignment.TopCenter, "");
                mypdfpage.Add(collinfo);
                string address = ds.Tables[0].Rows[0][2].ToString() + "" + ds.Tables[0].Rows[0][3].ToString() + "" + ds.Tables[0].Rows[0][4].ToString() + " - " + ds.Tables[0].Rows[0][5].ToString();
                collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 55, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + address);
                mypdfpage.Add(collinfo);
                //collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 70, 595, 50), System.Drawing.ContentAlignment.TopCenter, "E-mail  : " + ds.Tables[0].Rows[0][6].ToString());
                //mypdfpage.Add(collinfo);
                PdfArea pa1 = new PdfArea(mydoc, 14, 12, 564, 821);
                PdfRectangle border = new PdfRectangle(mydoc, pa1, Color.Black);
                mypdfpage.Add(border);
                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                {
                    Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                    mypdfpage.Add(LogoImage, 20, 20, 450);
                }
                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                {
                    Gios.Pdf.PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                    mypdfpage.Add(LogoImage1, 500, 20, 450);
                }
                string exammonth = ddlMonth.SelectedValue.ToString();
                string examyear = ddlYear.SelectedValue.ToString();
                //RadioButton1.Visible = true;
                //RadioButton2.Visible = true;
                //RadioButton3.Visible = true;
                //RadioButton4.Visible = true;
                ddlformate.Visible = true;
                printbtn.Visible = false;
                string activerow = "";
                string activecol = "";
                string depart_code = "";
                int sno = 0;
                activerow = rowii.ToString();
                //activecol = HAllSpread.ActiveSheetView.ActiveColumn.ToString();
                // added by sridhar 11 sep 2014 ====start
                int totalrowssri = Convert.ToInt32(HAllSpread.Sheets[0].RowCount.ToString()) - 1;
                if (Convert.ToString(totalrowssri) == activerow)
                {
                    return;
                }
                // added by sridhar 11 sep 2014 ====end
                string year = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                string degree = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                string course = Convert.ToString(HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag);
                string studenttot = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text;
                depart_code = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Note;
                string sem = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
                string batchyearatt = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Note;
                string label1 = " " + degree + " " + course + " ";
                Label2.Text = label1;
                collinfo = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 70, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Exam Month & Year  :  " + ddlMonth.SelectedItem.Text.ToString().ToUpper() + "  " + examyear);
                mypdfpage.Add(collinfo);
                collinfo = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 85, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Class & Group           : " + label1 + " " + grpsectionddl + "");
                mypdfpage.Add(collinfo);
                Session["semforsub"] = sem;
                Session["selecteddegreecode"] = depart_code;
                Session["selecteddegree"] = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                Session["selectedcourse"] = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag;
                //=========
                lblno.Visible = false;
                int sprdvisibleflag = 0;
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                // string studinfo = "select len(r.reg_no),r.reg_no,r.stud_name,r.roll_no,r.batch_year from registration r where r.current_semester=" + sem + "and r.degree_code=" + depart_code + "  and cc=0 and delflag=0 and exam_flag<>'Debar' order by len(r.reg_no),r.reg_no,r.stud_name";
                //  string studinfo = "select distinct  r.reg_no,r.stud_name,r.roll_no,r.batch_year  from exam_details e,exam_application a,registration r,degree g,course c,department p        where e.exam_code = a.exam_code and a.roll_no = r.roll_no and r.degree_code = g.degree_code and g.course_id = c.course_id         and g.dept_code = p.dept_code          and c.college_code='" + Session["collegecode"].ToString() + "'  and e.Exam_Month=" + exammonth + " and e.Exam_Year=" + examyear + "            and ltrim(r.roll_no) <>''      and cc=0 and delflag=0 and exam_flag<>'Debar'              and r.degree_code=" + depart_code + " and e.current_semester= " + sem + " " + sectionddl + "  order by r.reg_no        ";
                // string studinfo = "  select r.reg_no,r.stud_name,r.roll_no,r.batch_year from Registration  where current_semester='" + sem + "' and degree_code='" + depart_code + "' " + sectionddl + "";
                string studinfo = "select   reg_no,stud_name,roll_no,batch_year from registration where batch_year='" + batchyearatt + "' and degree_code='" + depart_code + "' and current_semester='" + sem + "'  and cc=0 and delflag=0 and exam_flag<>'Debar' and college_code=" + Session["collegecode"].ToString() + "  " + secval + " order by reg_no";
                int semval = Convert.ToInt16(sem) + Convert.ToInt16(1);
                SqlDataAdapter dastudinfo = new SqlDataAdapter(studinfo, con1);
                DataSet dsstudinfo = new DataSet();
                con1.Close();
                con1.Open();
                dastudinfo.Fill(dsstudinfo);
                Gios.Pdf.PdfTable studinfoss;
                int rowscc = 0;
                if (dsstudinfo.Tables[0].Rows.Count < 30)
                {
                    studinfoss = mydoc.NewTable(Fontsmall1, dsstudinfo.Tables[0].Rows.Count + 6, 11, 5);
                }
                else
                {
                    rowscc = dsstudinfo.Tables[0].Rows.Count;
                    rowscc = rowscc - 30;
                    studinfoss = mydoc.NewTable(Fontsmall1, 32, 11, 5);
                }
                studinfoss.VisibleHeaders = false;
                studinfoss.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                studinfoss.Cell(0, 0).SetContent("S.No");
                studinfoss.Cell(0, 0).SetFont(Fontsmall1bold);
                studinfoss.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                studinfoss.Cell(0, 1).SetContent("Name");
                studinfoss.Cell(0, 1).SetFont(Fontsmall1bold);
                studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                studinfoss.Cell(0, 2).SetContent("Reg.No");
                studinfoss.Cell(0, 2).SetFont(Fontsmall1bold);
                studinfoss.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                studinfoss.Cell(0, 3).SetContent("Paper (Max. Mks. : 50)");
                studinfoss.Cell(0, 3).SetFont(Fontsmall1bold);
                studinfoss.Cell(1, 3).SetContent("1");
                studinfoss.Cell(1, 3).SetFont(Fontsmall1bold);
                studinfoss.Cell(1, 4).SetContent("2");
                studinfoss.Cell(1, 4).SetFont(Fontsmall1bold);
                studinfoss.Cell(1, 5).SetContent("3");
                studinfoss.Cell(1, 5).SetFont(Fontsmall1bold);
                studinfoss.Cell(1, 6).SetContent("4");
                studinfoss.Cell(1, 6).SetFont(Fontsmall1bold);
                studinfoss.Cell(1, 7).SetContent("5");
                studinfoss.Cell(1, 7).SetFont(Fontsmall1bold);
                studinfoss.Cell(1, 8).SetContent("6");
                studinfoss.Cell(1, 8).SetFont(Fontsmall1bold);
                studinfoss.Cell(1, 9).SetContent("7");
                studinfoss.Cell(1, 9).SetFont(Fontsmall1bold);
                studinfoss.Cell(1, 10).SetContent("8");
                studinfoss.Cell(1, 10).SetFont(Fontsmall1bold);
                studinfoss.Columns[0].SetContentAlignment(ContentAlignment.MiddleCenter);
                studinfoss.Columns[2].SetContentAlignment(ContentAlignment.MiddleCenter);
                studinfoss.Columns[1].SetContentAlignment(ContentAlignment.MiddleLeft);
                studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                studinfoss.Columns[0].SetWidth(8);
                studinfoss.Columns[1].SetWidth(40);
                studinfoss.Columns[2].SetWidth(25);
                foreach (PdfCell pr in studinfoss.CellRange(0, 0, 0, 0).Cells)
                {
                    pr.RowSpan = 2;
                }
                foreach (PdfCell pr in studinfoss.CellRange(0, 1, 0, 1).Cells)
                {
                    pr.RowSpan = 2;
                }
                foreach (PdfCell pr in studinfoss.CellRange(0, 2, 0, 2).Cells)
                {
                    pr.RowSpan = 2;
                }
                foreach (PdfCell pr in studinfoss.CellRange(0, 3, 0, 3).Cells)
                {
                    pr.ColSpan = 8;
                }
                int newtablerow = 0;
                Boolean finish = false;
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
                        batchyear = dsstudinfo.Tables[0].Rows[studcount]["batch_year"].ToString();
                        regno = dsstudinfo.Tables[0].Rows[studcount]["reg_no"].ToString();
                        studname = dsstudinfo.Tables[0].Rows[studcount]["stud_name"].ToString();
                        rollno = dsstudinfo.Tables[0].Rows[studcount]["roll_no"].ToString();
                        sno++;
                        studinfoss.Cell(newtablerow + 2, 0).SetContent(sno);
                        studinfoss.Cell(newtablerow + 2, 1).SetContent(studname);
                        studinfoss.Cell(newtablerow + 2, 2).SetContent(regno);
                        if (newtablerow == 29)
                        {
                            int rowsccheck = rowscc - 30;
                            Gios.Pdf.PdfTablePage addtabletopage = studinfoss.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 120, 553, 800));
                            mypdfpage.Add(addtabletopage);
                            mypdfpage.SaveToDocument();
                            mypdfpage = mydoc.NewPage();
                            collinfo = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 20, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][0].ToString() + " (AUTONOMOUS)");
                            mypdfpage.Add(collinfo);
                            //collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 40, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][1].ToString() + "");
                            //mypdfpage.Add(collinfo);
                            collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 55, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + address);
                            mypdfpage.Add(collinfo);
                            //collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 70, 595, 50), System.Drawing.ContentAlignment.TopCenter, "E-mail  :" + ds.Tables[0].Rows[0][6].ToString());
                            //mypdfpage.Add(collinfo);
                            collinfo = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 70, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Exam Month & Year  :  " + ddlMonth.SelectedItem.Text.ToString().ToUpper() + "  " + examyear);
                            mypdfpage.Add(collinfo);
                            collinfo = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 85, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Class & Group           : " + label1 + " " + grpsectionddl + "");
                            mypdfpage.Add(collinfo);
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                            {
                                Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 20, 20, 450);
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                            {
                                Gios.Pdf.PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                mypdfpage.Add(LogoImage1, 500, 20, 450);
                            }
                            mypdfpage.Add(border);
                            if (rowsccheck > 0)
                            {
                                studinfoss = mydoc.NewTable(Fontsmall1, 32, 11, 5);
                                studinfoss.VisibleHeaders = false;
                                studinfoss.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                studinfoss.Cell(0, 0).SetContent("S.No");
                                studinfoss.Cell(0, 0).SetFont(Fontsmall1bold);
                                studinfoss.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                studinfoss.Cell(0, 1).SetContent("Name");
                                studinfoss.Cell(0, 1).SetFont(Fontsmall1bold);
                                studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                studinfoss.Cell(0, 2).SetContent("Reg.No");
                                studinfoss.Cell(0, 2).SetFont(Fontsmall1bold);
                                studinfoss.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                studinfoss.Cell(0, 3).SetContent("Paper (Max. Mks. : 50)");
                                studinfoss.Cell(0, 3).SetFont(Fontsmall1bold);
                                studinfoss.Cell(1, 3).SetContent("1");
                                studinfoss.Cell(1, 3).SetFont(Fontsmall1bold);
                                studinfoss.Cell(1, 4).SetContent("2");
                                studinfoss.Cell(1, 4).SetFont(Fontsmall1bold);
                                studinfoss.Cell(1, 5).SetContent("3");
                                studinfoss.Cell(1, 5).SetFont(Fontsmall1bold);
                                studinfoss.Cell(1, 6).SetContent("4");
                                studinfoss.Cell(1, 6).SetFont(Fontsmall1bold);
                                studinfoss.Cell(1, 7).SetContent("5");
                                studinfoss.Cell(1, 7).SetFont(Fontsmall1bold);
                                studinfoss.Cell(1, 8).SetContent("6");
                                studinfoss.Cell(1, 8).SetFont(Fontsmall1bold);
                                studinfoss.Cell(1, 9).SetContent("7");
                                studinfoss.Cell(1, 9).SetFont(Fontsmall1bold);
                                studinfoss.Cell(1, 10).SetContent("8");
                                studinfoss.Cell(1, 10).SetFont(Fontsmall1bold);
                                studinfoss.Columns[0].SetContentAlignment(ContentAlignment.MiddleCenter);
                                studinfoss.Columns[2].SetContentAlignment(ContentAlignment.MiddleCenter);
                                studinfoss.Columns[1].SetContentAlignment(ContentAlignment.MiddleLeft);
                                studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                studinfoss.Columns[0].SetWidth(8);
                                studinfoss.Columns[1].SetWidth(40);
                                studinfoss.Columns[2].SetWidth(25);
                                foreach (PdfCell pr in studinfoss.CellRange(0, 0, 0, 0).Cells)
                                {
                                    pr.RowSpan = 2;
                                }
                                foreach (PdfCell pr in studinfoss.CellRange(0, 1, 0, 1).Cells)
                                {
                                    pr.RowSpan = 2;
                                }
                                foreach (PdfCell pr in studinfoss.CellRange(0, 2, 0, 2).Cells)
                                {
                                    pr.RowSpan = 2;
                                }
                                foreach (PdfCell pr in studinfoss.CellRange(0, 3, 0, 3).Cells)
                                {
                                    pr.ColSpan = 8;
                                }
                                rowscc = rowscc - 30;
                            }
                            else if (rowsccheck != -30)
                            {
                                studinfoss = mydoc.NewTable(Fontsmall1, rowscc + 6, 11, 5);
                                studinfoss.VisibleHeaders = false;
                                studinfoss.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                studinfoss.Cell(0, 0).SetContent("S.No");
                                studinfoss.Cell(0, 0).SetFont(Fontsmall1bold);
                                studinfoss.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                studinfoss.Cell(0, 1).SetContent("Name");
                                studinfoss.Cell(0, 1).SetFont(Fontsmall1bold);
                                studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                studinfoss.Cell(0, 2).SetContent("Reg.No");
                                studinfoss.Cell(0, 2).SetFont(Fontsmall1bold);
                                studinfoss.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                studinfoss.Cell(0, 3).SetContent("Paper (Max. Mks. : 50)");
                                studinfoss.Cell(0, 3).SetFont(Fontsmall1bold);
                                studinfoss.Cell(1, 3).SetContent("1");
                                studinfoss.Cell(1, 3).SetFont(Fontsmall1bold);
                                studinfoss.Cell(1, 4).SetContent("2");
                                studinfoss.Cell(1, 4).SetFont(Fontsmall1bold);
                                studinfoss.Cell(1, 5).SetContent("3");
                                studinfoss.Cell(1, 5).SetFont(Fontsmall1bold);
                                studinfoss.Cell(1, 6).SetContent("4");
                                studinfoss.Cell(1, 6).SetFont(Fontsmall1bold);
                                studinfoss.Cell(1, 7).SetContent("5");
                                studinfoss.Cell(1, 7).SetFont(Fontsmall1bold);
                                studinfoss.Cell(1, 8).SetContent("6");
                                studinfoss.Cell(1, 8).SetFont(Fontsmall1bold);
                                studinfoss.Cell(1, 9).SetContent("7");
                                studinfoss.Cell(1, 9).SetFont(Fontsmall1bold);
                                studinfoss.Cell(1, 10).SetContent("8");
                                studinfoss.Cell(1, 10).SetFont(Fontsmall1bold);
                                studinfoss.Columns[0].SetContentAlignment(ContentAlignment.MiddleCenter);
                                studinfoss.Columns[2].SetContentAlignment(ContentAlignment.MiddleCenter);
                                studinfoss.Columns[1].SetContentAlignment(ContentAlignment.MiddleLeft);
                                studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                studinfoss.Columns[0].SetWidth(8);
                                studinfoss.Columns[1].SetWidth(40);
                                studinfoss.Columns[2].SetWidth(25);
                                foreach (PdfCell pr in studinfoss.CellRange(0, 0, 0, 0).Cells)
                                {
                                    pr.RowSpan = 2;
                                }
                                foreach (PdfCell pr in studinfoss.CellRange(0, 1, 0, 1).Cells)
                                {
                                    pr.RowSpan = 2;
                                }
                                foreach (PdfCell pr in studinfoss.CellRange(0, 2, 0, 2).Cells)
                                {
                                    pr.RowSpan = 2;
                                }
                                foreach (PdfCell pr in studinfoss.CellRange(0, 3, 0, 3).Cells)
                                {
                                    pr.ColSpan = 8;
                                }
                            }
                            else
                            {
                                studinfoss = mydoc.NewTable(Fontsmall1, rowscc + 4, 11, 5);
                                studinfoss.VisibleHeaders = false;
                                studinfoss.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                finish = true;
                            }
                            newtablerow = -1;
                        }
                        newtablerow++;
                    }
                    if (finish == false)
                    {
                        studinfoss.Cell(newtablerow + 2, 0).SetContent("Paper 1 :");
                        studinfoss.Cell(newtablerow + 2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        studinfoss.Cell(newtablerow + 3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        studinfoss.Cell(newtablerow + 4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        studinfoss.Cell(newtablerow + 5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 2, 0, newtablerow + 2, 0).Cells)
                        {
                            pr.ColSpan = 5;
                        }
                        studinfoss.Cell(newtablerow + 3, 0).SetContent("Paper 2 : ");
                        foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 3, 0, newtablerow + 3, 0).Cells)
                        {
                            pr.ColSpan = 5;
                        }
                        studinfoss.Cell(newtablerow + 4, 0).SetContent("Paper 3 : ");
                        foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 4, 0, newtablerow + 4, 0).Cells)
                        {
                            pr.ColSpan = 5;
                        }
                        studinfoss.Cell(newtablerow + 5, 0).SetContent("Paper 4 : ");
                        foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 5, 0, newtablerow + 5, 0).Cells)
                        {
                            pr.ColSpan = 5;
                        }
                        studinfoss.Cell(newtablerow + 2, 5).SetContent("Paper 5 : ");
                        foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 2, 5, newtablerow + 2, 5).Cells)
                        {
                            pr.ColSpan = 6;
                        }
                        studinfoss.Cell(newtablerow + 3, 5).SetContent("Paper 6 : ");
                        foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 3, 5, newtablerow + 3, 5).Cells)
                        {
                            pr.ColSpan = 6;
                        }
                        studinfoss.Cell(newtablerow + 4, 5).SetContent("Paper 7 : ");
                        foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 4, 5, newtablerow + 4, 5).Cells)
                        {
                            pr.ColSpan = 6;
                        }
                        studinfoss.Cell(newtablerow + 5, 5).SetContent("Paper 8 : ");
                        foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 5, 5, newtablerow + 5, 5).Cells)
                        {
                            pr.ColSpan = 6;
                        }
                        studinfoss.Cell(newtablerow + 2, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                        studinfoss.Cell(newtablerow + 3, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                        studinfoss.Cell(newtablerow + 4, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                        studinfoss.Cell(newtablerow + 5, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                        Gios.Pdf.PdfTablePage addtabletopage001 = studinfoss.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 120, 553, 800));
                        mypdfpage.Add(addtabletopage001);
                        additionaldata = addtabletopage001.Area.Height;
                        collinfo = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 35, additionaldata + 145, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "DATE : ");
                        mypdfpage.Add(collinfo);
                        collinfo = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 370, additionaldata + 145, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "SIGNATURE : ");
                        mypdfpage.Add(collinfo);
                        mypdfpage.SaveToDocument();
                    }
                    else
                    {
                        studinfoss.Cell(newtablerow + 0, 0).SetContent("Paper 1 :");
                        studinfoss.Cell(newtablerow + 0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        studinfoss.Cell(newtablerow + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        studinfoss.Cell(newtablerow + 2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        studinfoss.Cell(newtablerow + 3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 0, 0, newtablerow + 0, 0).Cells)
                        {
                            pr.ColSpan = 5;
                        }
                        studinfoss.Cell(newtablerow + 1, 0).SetContent("Paper 2 : ");
                        foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 1, 0, newtablerow + 1, 0).Cells)
                        {
                            pr.ColSpan = 5;
                        }
                        studinfoss.Cell(newtablerow + 2, 0).SetContent("Paper 3 : ");
                        foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 2, 0, newtablerow + 2, 0).Cells)
                        {
                            pr.ColSpan = 5;
                        }
                        studinfoss.Cell(newtablerow + 3, 0).SetContent("Paper 4 : ");
                        foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 3, 0, newtablerow + 3, 0).Cells)
                        {
                            pr.ColSpan = 5;
                        }
                        studinfoss.Cell(newtablerow + 0, 5).SetContent("Paper 5 : ");
                        foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 0, 5, newtablerow + 0, 5).Cells)
                        {
                            pr.ColSpan = 6;
                        }
                        studinfoss.Cell(newtablerow + 1, 5).SetContent("Paper 6 : ");
                        foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 1, 5, newtablerow + 1, 5).Cells)
                        {
                            pr.ColSpan = 6;
                        }
                        studinfoss.Cell(newtablerow + 2, 5).SetContent("Paper 7 : ");
                        foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 2, 5, newtablerow + 2, 5).Cells)
                        {
                            pr.ColSpan = 6;
                        }
                        studinfoss.Cell(newtablerow + 3, 5).SetContent("Paper 8 : ");
                        foreach (PdfCell pr in studinfoss.CellRange(newtablerow + 3, 5, newtablerow + 3, 5).Cells)
                        {
                            pr.ColSpan = 6;
                        }
                        studinfoss.Cell(newtablerow + 0, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                        studinfoss.Cell(newtablerow + 1, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                        studinfoss.Cell(newtablerow + 2, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                        studinfoss.Cell(newtablerow + 3, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                        Gios.Pdf.PdfTablePage addtabletopage001 = studinfoss.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 120, 553, 800));
                        mypdfpage.Add(addtabletopage001);
                        mypdfpage.SaveToDocument();
                        additionaldata = addtabletopage001.Area.Height;
                    }
                }
            }
        }
        string appPath = HttpContext.Current.Server.MapPath("~");
        if (appPath != "")
        {
            string szPath = appPath + "/Report/";
            string szFile = "Marksheets" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
            Response.Buffer = true;
            Response.Clear();
            mydoc.SaveToFile(szPath + szFile);
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
            Response.ContentType = "application/pdf";
            Response.WriteFile(szPath + szFile);
        }
    }

    protected void FpSpread2_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        FpSpread2.SaveChanges();
        if (Convert.ToInt32(FpSpread2.Sheets[0].Cells[0, 2].Value) == 1)
        {
            for (int i = 0; i < FpSpread2.Sheets[0].RowCount - 1; i++)
            {
                FpSpread2.Sheets[0].Cells[i, 2].Value = 1;
            }
        }
        else if (Convert.ToInt32(FpSpread2.Sheets[0].Cells[0, 2].Value) == 0)
        {
            for (int i = 0; i < FpSpread2.Sheets[0].RowCount - 1; i++)
            {
                FpSpread2.Sheets[0].Cells[i, 2].Value = 0;
            }
        }
        FpSpread2.Visible = true;
        FpSpread2.SaveChanges();
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
            FpSpread2.SaveChanges();
            if (chk_subjectwise.Checked == true)
            {
                bindsubjectpdf();
            }
            else
            {
                bindppdf();
            }
            FpSpread2.Visible = false;
        }
        catch (Exception ex)
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
        string strgetval = "";
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

    protected void HAllSpread_Command(object sender, EventArgs e)
    {
        HAllSpread.SaveChanges();
        if (Convert.ToInt32(HAllSpread.Sheets[0].Cells[0, 6].Value) == 1)
        {
            for (int i = 0; i < HAllSpread.Sheets[0].RowCount - 1; i++)
            {
                HAllSpread.Sheets[0].Cells[i, 6].Value = 1;
            }
        }
        else if (Convert.ToInt32(HAllSpread.Sheets[0].Cells[0, 6].Value) == 0)
        {
            for (int i = 0; i < HAllSpread.Sheets[0].RowCount - 1; i++)
            {
                HAllSpread.Sheets[0].Cells[i, 6].Value = 0;
            }
        }
        HAllSpread.Visible = true;
    }

    protected void ddlsubject_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int cout = 0;
        cbsubj.Checked = false;
        txtsubject.Text = "-Select-";
        for (int i = 0; i < ddlsubject.Items.Count; i++)
        {
            if (ddlsubject.Items[i].Selected == true)
            {
                cout++;
            }
        }
        if (cout > 0)
        {
            txtsubject.Text = "Subject (" + cout + ")";
            if (cout == ddlsubject.Items.Count)
            {
                cbsubj.Checked = true;
            }
        }
        // hiddenfiels();
    }

    protected void dropsubjecttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dropsubjecttype.Items.Count > 0)
        {
            //bindsubject();
            hiddenfiels();
        }
    }

    protected void chk_subjectwise_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_subjectwise.Checked == true)
        {
            lblsubjecttype.Visible = true;
            dropsubjecttype.Visible = true;
            lblsubject.Visible = true;
            txtsubject.Visible = true;
            lblterm.Visible = true;
            dropterm.Visible = true;
            pnlsec.Visible = true;
            loadsubjecttype();
            bindsem();
            bindsubject();
        }
        else
        {
            lblterm.Visible = false;
            dropterm.Visible = false;
            lblsubjecttype.Visible = false;
            dropsubjecttype.Visible = false;
            lblsubject.Visible = false;
            pnlsec.Visible = false;
            txtsubject.Visible = false;
        }
        hiddenfiels();
    }

    public void bindsem()
    {
        dropterm.Items.Clear();
        if (dropsubjecttype.Items.Count > 0)
        {
            string stssql = "select MAX(Duration) from course c, Degree d where c.Course_Id=d.Course_Id and c.type='" + ddlcoltypeadd.SelectedItem.Text.ToString() + "' and c.Edu_Level='" + ddledulevel.SelectedItem.Text.ToString() + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(stssql, "Text");
            dropterm.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()); i++)
                {
                    dropterm.Items.Insert(i, Convert.ToString(i + 1));
                }
            }
        }
    }

    public void bindsubject()
    {
        try
        {
            ddlsubject.Items.Clear();
            hat.Clear();
            ds.Clear();
            string semesters = "";
            if (dropterm.Items.Count > 0)
            {
                semesters = "and semester='" + dropterm.SelectedItem.Text.ToString() + "'";
            }
            if (dropsubjecttype.Items.Count > 0)
            {
                string subject = "select distinct  subject_code+'-'+subject_name as subject_name,subject_code from subject s,syllabus_master y,sub_sem ss  where s.syll_code = y.syll_code and ss.syll_code=s.syll_code and s.subType_no=ss.subType_no and y.syll_code=ss.syll_code   and subject_type<>'others' and y.Batch_Year = '" + dropsubjecttype.SelectedValue + "' " + semesters + " ; ";
                ds = d2.select_method_wo_parameter(subject, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlsubject.DataSource = ds;
                    ddlsubject.DataTextField = "subject_name";
                    ddlsubject.DataValueField = "subject_code";
                    ddlsubject.DataBind();
                }
                else
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
        ddlsubject.Visible = false;
        pnlsec.Visible = false;
        lblsubject.Visible = false;
        txtsubject.Visible = false;
    }

    //public void bindyear()
    //{
    //    try
    //    {
    //        dropsubjecttype.Items.Clear();
    //        ds.Clear();
    //        ds = d2.select_method_wo_parameter("bind_batch", "sp");
    //        int count = ds.Tables[0].Rows.Count;
    //        if (count > 0)
    //        {
    //            dropsubjecttype.DataSource = ds;
    //            dropsubjecttype.DataTextField = "batch_year";
    //            dropsubjecttype.DataValueField = "batch_year";
    //            dropsubjecttype.DataBind();
    //        }
    //        if (ds.Tables[1].Rows.Count > 0)
    //        {
    //            int max_bat = 0;
    //            max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
    //            dropsubjecttype.SelectedValue = max_bat.ToString();
    //            bindsem();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        //lblerrormsg.Text = ex.ToString();
    //        //lblerrormsg.Visible = true;
    //    }
    //}
    protected void dropterm_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (chk_subjectwise.Checked == true)
            {
                lblsubjecttype.Visible = true;
                dropsubjecttype.Visible = true;
                lblsubject.Visible = true;
                txtsubject.Visible = true;
                lblterm.Visible = true;
                dropterm.Visible = true;
                pnlsec.Visible = true;
                bindsubject();
            }
            else
            {
                lblterm.Visible = false;
                dropterm.Visible = false;
                lblsubjecttype.Visible = false;
                dropsubjecttype.Visible = false;
                lblsubject.Visible = false;
                pnlsec.Visible = false;
                txtsubject.Visible = false;
            }
            hiddenfiels();
            //bindsubject();
            //hiddenfiels();
        }
        catch (Exception ex)
        {
            //lblmsg.Visible = true;
            //lblmsg.Text = ex.ToString();
        }
    }

    protected void cbsubj_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbsubj.Checked == true)
            {
                int cout = 0;
                for (int i = 0; i < ddlsubject.Items.Count; i++)
                {
                    cout++;
                    ddlsubject.Items[i].Selected = true;
                    cbsubj.Checked = true;
                    txtsubject.Text = "Subject (" + cout + ")";
                }
            }
            else
            {
                int cout = 0;
                for (int i = 0; i < ddlsubject.Items.Count; i++)
                {
                    cout++;
                    ddlsubject.Items[i].Selected = false;
                    txtsubject.Text = "-Select-";
                    cbsubj.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            //lblmsg.Visible = true;
            //lblmsg.Text = ex.ToString();
        }
    }

    protected void ddledulevel_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    //protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //}

    public void bindedulevel()
    {
        string sql = "select distinct Edu_Level from course where college_code='" + Session["collegecode"].ToString() + "'";
        ds.Clear();
        ds = da.select_method_wo_parameter(sql, "Text");
        ddledulevel.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddledulevel.DataSource = ds;
            ddledulevel.DataTextField = "Edu_Level";
            ddledulevel.DataBind();
        }
    }

    public void loadtype()
    {
        try
        {
            collegecode = Session["collegecode"].ToString();
            string strquery = "select distinct type from course where college_code='" + collegecode + "' and type is not null and type<>''";
            ds.Clear();
            ds = da.select_method_wo_parameter(strquery, "Text");
            ddlcoltypeadd.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcoltypeadd.DataSource = ds;
                ddlcoltypeadd.DataTextField = "type";
                ddlcoltypeadd.DataBind();
            }
        }
        catch
        {
        }
    }

    public void loadsubjecttype()
    {
        try
        {
            string strquery = "  select distinct subject_type from sub_sem order by subject_type";
            ds.Clear();
            ds = da.select_method_wo_parameter(strquery, "Text");
            dropsubjecttype.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                dropsubjecttype.DataSource = ds;
                dropsubjecttype.DataTextField = "subject_type";
                dropsubjecttype.DataBind();
                dropsubjecttype.Items.Insert(0, "All");
            }
        }
        catch
        {
        }
    }

    //public void bindsemedulevel()
    //{
    //    dropterm.Items.Clear();
    //    if (dropsubjecttype.Items.Count > 0)
    //    {
    //        string stssql = "select distinct Current_Semester from Registration where Batch_Year='" + dropsubjecttype.SelectedItem.Text.ToString() + "'";
    //        stssql = "select MAX(r.Current_Semester) from course c,Registration r,Degree d where c.type='" + ddlcoltypeadd.SelectedItem.Text.ToString() + "' and c.Edu_Level='"+ddledulevel.SelectedItem.Text.ToString()+"' and d.Course_Id=c.Course_Id and d.Degree_Code=r.degree_code and CC=0 and DelFlag=0 and Exam_Flag<>'debar'";
    //        ds.Clear();
    //        ds = da.select_method_wo_parameter(stssql, "Text");
    //        ddlsem.Items.Clear();
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            for (int i = 0; i <= Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()); i++)
    //            {
    //                ddlsem.Items.Insert(i, Convert.ToString(i + 1));
    //            }
    //        }
    //    }
    //}

}
