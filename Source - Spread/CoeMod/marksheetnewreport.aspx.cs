using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//using BalAccess;
//using DalConnection;
//using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;

public partial class marksheetnewreport : System.Web.UI.Page
{
    SqlCommand cmd;
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection setcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    Hashtable hashsubjectno = new Hashtable();
    Hashtable hashsubjectno1 = new Hashtable();
    Hashtable hashsubjectno2 = new Hashtable();
    Hashtable hashsubjectno3 = new Hashtable();
    Hashtable hashsubjectcode = new Hashtable();
    string Master = "";
    public DataSet Bind_Degree(string college_code, string user_code)
    {
        SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        dcon.Close();
        dcon.Open();
        SqlCommand cmd = new SqlCommand("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "", dcon);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }
    public DataSet Bind_Dept(string degree_code, string college_code, string user_code)
    {
        SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        dcon.Close();
        dcon.Open();
        SqlCommand cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + degree_code + " and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "", dcon);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }
    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {

        Control cntUpdateBtn = sprdmarksheet.FindControl("Update");
        Control cntCancelBtn = sprdmarksheet.FindControl("Cancel");
        Control cntCopyBtn = sprdmarksheet.FindControl("Copy");
        Control cntCutBtn = sprdmarksheet.FindControl("Clear");
        Control cntPasteBtn = sprdmarksheet.FindControl("Excel");
        //Control cntPageNextBtn = FpSpread1.FindControl("Next");
        //Control cntPagePreviousBtn = FpSpread1.FindControl("Prev");
        Control cntPagePrintBtn = sprdmarksheet.FindControl("Print");

        if ((cntUpdateBtn != null))
        {

            TableCell tc = (TableCell)cntUpdateBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;

            tr.Cells.Remove(tc);

            tc = (TableCell)cntCancelBtn.Parent;
            tr.Cells.Remove(tc);


            tc = (TableCell)cntCopyBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntCutBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPasteBtn.Parent;
            tr.Cells.Remove(tc);

            //tc = (TableCell)cntPageNextBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPagePreviousBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPagePrintBtn.Parent;
            //tr.Cells.Remove(tc);

        }

        base.Render(writer);
    }
    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
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
            if (!IsPostBack)
            {
                Master = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                setcon.Close();
                setcon.Open();
                SqlDataReader mtrdr;
                string strdayflag = "";
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
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
                DateTime currentdate = System.DateTime.Now;
                string newdate = currentdate.ToString("dd/MM/yyyy");
                TextBox1.Text = newdate;
                sprdmarksheet.Visible = false;
                sprdmarksheet.Sheets[0].AutoPostBack = true;
                sprdmarksheet.Sheets[0].ColumnCount = 5;
                sprdmarksheet.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                sprdmarksheet.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                sprdmarksheet.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                sprdmarksheet.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                sprdmarksheet.Sheets[0].RowCount = 0;
                sprdmarksheet.Sheets[0].Columns[0].Width = 80;
                sprdmarksheet.Sheets[0].Columns[1].Width = 150;
                sprdmarksheet.Sheets[0].Columns[2].Width = 150;
                sprdmarksheet.Sheets[0].Columns[3].Width = 250;
                sprdmarksheet.Sheets[0].Columns[sprdmarksheet.Sheets[0].ColumnCount - 1].Width = 100;
                sprdmarksheet.Sheets[0].RowHeader.Visible = false;

                cmd = new SqlCommand(" select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>''order by batch_year", con);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1);

                ddlbatch.DataSource = ds1;
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
                //ddlBatch.Items.Insert(0, new ListItem("--Select--", "-1"));
                int batch = 0;
                string batchcount = ddlbatch.Items.Count.ToString();
                if (int.TryParse(batchcount, out batch))
                    batch = batch - 1;
                ddlbatch.SelectedIndex = batch;
                con.Open();
                //con.Open();
                string collegecode = Session["collegecode"].ToString();
                string usercode = Session["usercode"].ToString();
                if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                {
                    cmd = new SqlCommand("select distinct degree.course_id,course.course_name from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + Session["collegecode"] + " order by course.course_name ", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    //DataSet ds = Bind_Degree(collegecode.ToString(), usercode);
                    da.Fill(ds);
                    ddldegree.DataSource = ds;
                    ddldegree.DataValueField = "course_id";
                    ddldegree.DataTextField = "course_name";
                    ddldegree.DataBind();
                    //ddlDegree.Items.Insert(0, new ListItem("--Select--", "-1"));
                }

                //bind BRANCH on loaD...
                con.Close();
                con.Open();
                //cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + ddlDegree.SelectedValue.ToString() + " and degree.college_code= " + Session["collegecode"] + " ", con);
                //SqlDataAdapter daBRANCH = new SqlDataAdapter(cmd);
                string course_id = ddldegree.SelectedValue.ToString();

                DataSet dsbranch = Bind_Dept(course_id, collegecode, usercode);
                //daBRANCH.Fill(dsbranch);
                ddlbranch.DataSource = dsbranch;
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataBind();
                //bind semester
                bindsem();
                //bind section
                BindSectionDetail();
            }
        }
        catch(Exception ex)
        {
        }

    }
    public void BindSectionDetail()
    {
        //string branch = ddlBranch.SelectedValue.ToString();
        //string batch = ddlBatch.SelectedValue.ToString();
        //DataSet ds = ClsAttendanceAccess.GetsectionDetail(batch.ToString(), branch.ToString());
        //if (ds.Tables[0].Rows.Count > 0)
        //{


        //    ddlSec.DataSource = ds;
        //    ddlSec.DataTextField = "Sections";
        //    ddlSec.DataValueField = "Sections";
        //    ddlSec.DataBind();
        //    ddlSec.Items.Insert(0, new ListItem("--Select--", "-1"));
        //}

        string branch = ddlbranch.SelectedValue.ToString();
        string batch = ddlbatch.SelectedValue.ToString();
        con.Close();
        con.Open();
        cmd = new SqlCommand("select distinct sections from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlSec.DataSource = ds;
        ddlSec.DataTextField = "sections";
        ddlSec.DataValueField = "sections";
        ddlSec.DataBind();
        //ddlSec.Items.Insert(0, new ListItem("", "-1"));
        SqlDataReader dr_sec;
        dr_sec = cmd.ExecuteReader();
        dr_sec.Read();
        if (dr_sec.HasRows == true)
        {
            if (dr_sec["sections"].ToString() == "")
            {
                ddlSec.Enabled = false;
                //Label8.Visible = false;

            }
            else
            {
                ddlSec.Enabled = true;

            }
        }
        else
        {
            ddlSec.Enabled = false;
            //Label8.Visible = false;

        }
    }
    public void bindsem()
    {

        //--------------------semester load
        ddlsem.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        con.Close();
        con.Open();
        SqlDataReader dr;
        cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlbranch.Text.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
        dr = cmd.ExecuteReader();
        dr.Read();
        if (dr.HasRows == true)
        {
            first_year = Convert.ToBoolean(dr[1].ToString());
            duration = Convert.ToInt16(dr[0].ToString());
            for (i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    ddlsem.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {
                    ddlsem.Items.Add(i.ToString());
                }

            }
        }
        else
        {
            dr.Close();
            SqlDataReader dr1;
            cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlbranch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
            ddlsem.Items.Clear();
            dr1 = cmd.ExecuteReader();
            dr1.Read();
            if (dr1.HasRows == true)
            {
                first_year = Convert.ToBoolean(dr1[1].ToString());
                duration = Convert.ToInt16(dr1[0].ToString());

                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsem.Items.Add(i.ToString());
                    }
                }
            }

            dr1.Close();
        }
        //FpMarkEntry.Visible = false;
        con.Close();
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnexcel.Visible = false;
        sprdmarksheet.Visible = false;
        string collegecode = Session["collegecode"].ToString();
        string usercode = Session["usercode"].ToString();
        if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
        {
            cmd = new SqlCommand("select distinct degree.course_id,course.course_name from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + Session["collegecode"] + " order by course.course_name ", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            //DataSet ds = Bind_Degree(collegecode.ToString(), usercode);
            da.Fill(ds);
            ddldegree.DataSource = ds;
            ddldegree.DataValueField = "course_id";
            ddldegree.DataTextField = "course_name";
            ddldegree.DataBind();

        }

        //bind BRANCH on loaD...
        con.Close();
        con.Open();
        string course_id = ddldegree.SelectedValue.ToString();
        DataSet dsbranch = Bind_Dept(course_id, collegecode, usercode);
        ddlbranch.DataSource = dsbranch;
        ddlbranch.DataValueField = "degree_code";
        ddlbranch.DataTextField = "dept_name";
        ddlbranch.DataBind();
        //bind semester
        bindsem();
        BindSectionDetail();
    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnexcel.Visible = false;
        sprdmarksheet.Visible = false;
        con.Close();
        con.Open();
        string course_id = ddldegree.SelectedValue.ToString();
        string collegecode = Session["collegecode"].ToString();
        string usercode = Session["usercode"].ToString();
        DataSet dsbranch = Bind_Dept(course_id, collegecode, usercode);
        ddlbranch.DataSource = dsbranch;
        ddlbranch.DataValueField = "degree_code";
        ddlbranch.DataTextField = "dept_name";
        ddlbranch.DataBind();
        //bind semester
        bindsem();
        BindSectionDetail();
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnexcel.Visible = false;
        sprdmarksheet.Visible = false;
        bindsem();
        BindSectionDetail();
    }
    protected void Button3_Click(object sender, EventArgs e)
    {


    }
    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnexcel.Visible = false;
        sprdmarksheet.Visible = false;
        BindSectionDetail();
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            btnexcel.Visible = false;
            lblnorec.Visible = false;
            sprdmarksheet.Sheets[0].AutoPostBack = true;
            sprdmarksheet.Sheets[0].ColumnCount = 5;
            sprdmarksheet.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            sprdmarksheet.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            sprdmarksheet.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            sprdmarksheet.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            sprdmarksheet.Sheets[0].RowCount = 0;
            sprdmarksheet.Sheets[0].Columns[0].Width = 80;
            sprdmarksheet.Sheets[0].Columns[1].Width = 150;
            sprdmarksheet.Sheets[0].Columns[2].Width = 150;
            sprdmarksheet.Sheets[0].Columns[3].Width = 250;
            sprdmarksheet.Sheets[0].RowHeader.Visible = false;

            sprdmarksheet.Sheets[0].SheetCorner.RowCount = 7;
            sprdmarksheet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, 6);
            sprdmarksheet.Sheets[0].ColumnHeaderSpanModel.Add(1, 0, 1, 6);
            sprdmarksheet.Sheets[0].ColumnHeaderSpanModel.Add(2, 0, 1, 6);
            sprdmarksheet.Sheets[0].ColumnHeaderSpanModel.Add(3, 0, 1, 6);
            sprdmarksheet.Sheets[0].ColumnHeaderSpanModel.Add(4, 0, 1, 6);
            sprdmarksheet.Sheets[0].ColumnHeaderSpanModel.Add(5, 0, 1, 6);
            sprdmarksheet.Sheets[0].ColumnHeader.Rows[0].Border.BorderColor = Color.White;
            sprdmarksheet.Sheets[0].ColumnHeader.Rows[1].Border.BorderColor = Color.White;
            sprdmarksheet.Sheets[0].ColumnHeader.Rows[2].Border.BorderColor = Color.White;
            sprdmarksheet.Sheets[0].ColumnHeader.Rows[3].Border.BorderColor = Color.White;
            sprdmarksheet.Sheets[0].ColumnHeader.Rows[4].Border.BorderColor = Color.White;
            sprdmarksheet.Sheets[0].ColumnHeader.Rows[5].Border.BorderColor = Color.White;
            FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
            style.Font.Size = 10;
            style.Font.Bold = true;
            sprdmarksheet.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
            sprdmarksheet.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
            sprdmarksheet.Sheets[0].AllowTableCorner = true;
            sprdmarksheet.Sheets[0].ColumnHeader.Rows[0].Border.BorderColorTop = Color.Black;
            sprdmarksheet.Sheets[0].ColumnHeader.Rows[5].Border.BorderColorBottom = Color.Black;
            sprdmarksheet.Sheets[0].ColumnHeader.Rows[6].Border.BorderColorBottom = Color.Black;
            sprdmarksheet.Sheets[0].ColumnHeader.Rows[6].Border.BorderColorRight = Color.Black;
            sprdmarksheet.Sheets[0].ColumnHeader.Rows[6].BackColor = Color.FromArgb(214, 235, 255);
            sprdmarksheet.Sheets[0].ColumnHeader.Rows[6].Font.Bold = true;
            sprdmarksheet.Sheets[0].ColumnHeader.Rows[6].Font.Size = FontUnit.Medium;
            string str = "select isnull(collname, ' ') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
            con1.Close();
            con1.Open();
            SqlCommand comm = new SqlCommand(str, con1);
            SqlDataReader drr = comm.ExecuteReader();
            drr.Read();
            string coll_name = Convert.ToString(drr["collname"]);
            string coll_address1 = Convert.ToString(drr["address1"]);
            string coll_address2 = Convert.ToString(drr["address2"]);
            string coll_address3 = Convert.ToString(drr["address3"]);
            string sem = ddlsem.SelectedValue.ToString();
            string sem2 = "";
            if (sem == "1")
                sem2 = "I";
            else if (sem == "2")
                sem2 = "II";
            else if (sem == "3")
                sem2 = "III";
            else if (sem == "4")
                sem2 = "IV";
            else if (sem == "5")
                sem2 = "V";
            else if (sem == "6")
                sem2 = "VI";
            else if (sem == "7")
                sem2 = "VII";
            else if (sem == "8")
                sem2 = "VIII";
            else if (sem == "9")
                sem2 = "IX";
            else if (sem == "10")
                sem2 = "X";
            sprdmarksheet.Sheets[0].ColumnHeader.Cells[0, 0].Text = TextBox1.Text;
            sprdmarksheet.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Right;
            sprdmarksheet.Sheets[0].ColumnHeader.Cells[1, 0].Text = coll_name + "," + " " + coll_address3;
            sprdmarksheet.Sheets[0].ColumnHeader.Cells[1, 0].HorizontalAlign = HorizontalAlign.Center;
            sprdmarksheet.Sheets[0].ColumnHeader.Cells[3, 0].Text = sem2 + " " + "Term" + " " + "-" + " " + ddlbranch.SelectedItem.Text + " " + "Autonomous Examination";
            sprdmarksheet.Sheets[0].ColumnHeader.Cells[3, 0].HorizontalAlign = HorizontalAlign.Center;

            sprdmarksheet.Sheets[0].ColumnHeader.Cells[4, 0].HorizontalAlign = HorizontalAlign.Left;
            sprdmarksheet.Sheets[0].ColumnHeader.Cells[6, 0].Text = "S No";
            sprdmarksheet.Sheets[0].ColumnHeader.Cells[6, 1].Text = "Roll No";
            sprdmarksheet.Sheets[0].ColumnHeader.Cells[6, 2].Text = "Reg No";
            sprdmarksheet.Sheets[0].ColumnHeader.Cells[6, 3].Text = "Student Name";
            sprdmarksheet.Sheets[0].ColumnHeader.Cells[6, 0].HorizontalAlign = HorizontalAlign.Center;
            sprdmarksheet.Sheets[0].ColumnHeader.Cells[6, 1].HorizontalAlign = HorizontalAlign.Center;
            sprdmarksheet.Sheets[0].ColumnHeader.Cells[6, 2].HorizontalAlign = HorizontalAlign.Center;
            sprdmarksheet.Sheets[0].ColumnHeader.Cells[6, 3].HorizontalAlign = HorizontalAlign.Center;
            sprdmarksheet.Sheets[0].ColumnHeaderSpanModel.Add(6, 3, 1, 2);
            string degreecode = ddlbranch.SelectedValue.ToString();
            int semester = Convert.ToInt32(ddlsem.SelectedValue);
            int batchyear = Convert.ToInt32(ddlbatch.SelectedValue);
            int checkval = 0;
            int checkarrear = 0;
            string section = ddlSec.SelectedValue.ToString();
            string getexamcode = "select exam_code from exam_details where batch_year=" + batchyear + " and degree_code=" + degreecode + " and current_semester=" + semester + "";
            SqlDataAdapter daexamcode = new SqlDataAdapter(getexamcode, con);
            DataSet dsexamcode = new DataSet();
            con.Close();
            con.Open();
            daexamcode.Fill(dsexamcode);
            int examcode = 0;
            if (dsexamcode.Tables[0].Rows.Count > 0)
            {
                examcode = Convert.ToInt32(dsexamcode.Tables[0].Rows[0]["exam_code"]);
            }
            if (examcode != 0)
            {
                con1.Close();
                SqlCommand cmd = new SqlCommand("proctabulatedSubjectCount", con1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@degreecode", degreecode);
                cmd.Parameters.AddWithValue("@currentsemester", semester);
                cmd.Parameters.AddWithValue("@examcode", examcode);
                cmd.Parameters.AddWithValue("@batchyear", batchyear);
                cmd.Parameters.AddWithValue("@checkvalue", checkval);
                cmd.Parameters.AddWithValue("@checkarrear", checkarrear);
                cmd.Parameters.AddWithValue("@sections", section);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet examds = new DataSet();
                da.Fill(examds);
                if (examds.Tables[0].Rows.Count > 0)
                {
                    string subtype1 = "";
                    sprdmarksheet.Visible = true;
                    lblnorec.Visible = false;
                    int count = 0;
                    for (int i = 0; i < examds.Tables[0].Rows.Count; i++)
                    {
                        btnexcel.Visible = true;
                        string subjecttype = "";
                        string subjectcode = "";
                        string subjectno = "";
                        string subjectname = "";
                        subjecttype = examds.Tables[0].Rows[i]["subject_type"].ToString();
                        subjectcode = examds.Tables[0].Rows[i]["subject_code"].ToString();
                        subjectno = examds.Tables[0].Rows[i]["subject_no"].ToString();
                        subjectname = examds.Tables[0].Rows[i]["subject_name"].ToString();
                        hashsubjectno.Add(subjectno, subjectcode);
                        hashsubjectcode.Add(subjectcode, subjectname);
                        sprdmarksheet.Sheets[0].ColumnCount = sprdmarksheet.Sheets[0].ColumnCount + 1;
                        sprdmarksheet.Sheets[0].ColumnHeader.Cells[6, sprdmarksheet.Sheets[0].ColumnCount - 1].Text = " " + subjectcode + " ";
                        sprdmarksheet.Sheets[0].ColumnHeader.Cells[6, sprdmarksheet.Sheets[0].ColumnCount - 1].Margin.Left = 15;
                        sprdmarksheet.Sheets[0].ColumnHeader.Cells[6, sprdmarksheet.Sheets[0].ColumnCount - 1].Note = subjectcode;
                        sprdmarksheet.Sheets[0].ColumnHeader.Cells[6, sprdmarksheet.Sheets[0].ColumnCount - 1].Tag = subjectno;
                        sprdmarksheet.Sheets[0].ColumnHeader.Cells[5, sprdmarksheet.Sheets[0].ColumnCount - 1].Tag = subjectname;
                        sprdmarksheet.Sheets[0].ColumnHeader.Cells[6, sprdmarksheet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                    }
                }
                else
                {
                    lblnorec.Visible = true;
                    sprdmarksheet.Visible = false;
                }
                if (examds.Tables[1].Rows.Count > 0)
                {
                    string Rollno = "";
                    string Regno = "";
                    string studentname = "";
                    string DOB = "";
                    int sno = 0;

                    sprdmarksheet.Sheets[0].ColumnCount = sprdmarksheet.Sheets[0].ColumnCount + 2;
                    sprdmarksheet.Sheets[0].ColumnHeader.Cells[6, sprdmarksheet.Sheets[0].ColumnCount - 2].Text = "Result";
                    sprdmarksheet.Sheets[0].Columns[sprdmarksheet.Sheets[0].ColumnCount - 2].Width = 100;
                    sprdmarksheet.Sheets[0].ColumnHeader.Cells[6, sprdmarksheet.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                    sprdmarksheet.Sheets[0].ColumnHeader.Cells[6, sprdmarksheet.Sheets[0].ColumnCount - 1].Text = "Not Cleared";
                    sprdmarksheet.Sheets[0].Columns[sprdmarksheet.Sheets[0].ColumnCount - 1].Width = 140;
                    sprdmarksheet.Sheets[0].ColumnHeader.Cells[6, sprdmarksheet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    for (int i = 0; i < examds.Tables[1].Rows.Count; i++)
                    {
                        hashsubjectno2.Clear();
                        hashsubjectno1.Clear();
                        double total = 0;
                        int count = 0;
                        string r1 = "";
                        string result = "";
                        int result0 = 0;
                        string failsubno = "";
                        Rollno = examds.Tables[1].Rows[i]["roll_no"].ToString();
                        Regno = examds.Tables[1].Rows[i]["reg_no"].ToString();
                        studentname = examds.Tables[1].Rows[i]["studname"].ToString();
                        //DOB = examds.Tables[1].Rows[i]["dob"].ToString();
                        sno++;
                        sprdmarksheet.Sheets[0].RowCount = sprdmarksheet.Sheets[0].RowCount + 4;
                        sprdmarksheet.Sheets[0].Rows[sprdmarksheet.Sheets[0].RowCount - 4].Border.BorderColor = Color.White;
                        sprdmarksheet.Sheets[0].Rows[sprdmarksheet.Sheets[0].RowCount - 3].Border.BorderColor = Color.White;
                        sprdmarksheet.Sheets[0].Rows[sprdmarksheet.Sheets[0].RowCount - 2].Border.BorderColor = Color.White;
                        sprdmarksheet.Sheets[0].Rows[sprdmarksheet.Sheets[0].RowCount - 1].Border.BorderColorRight = Color.White;
                        sprdmarksheet.Sheets[0].SpanModel.Add(sprdmarksheet.Sheets[0].RowCount - 3, 0, 3, 4);
                        sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 3, 0].Border.BorderColorBottom = Color.LightBlue;
                        sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 3, 0].Border.BorderColorRight = Color.White;
                        sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 2, 0].Border.BorderColorRight = Color.White;
                        sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 1, 0].Border.BorderColorRight = Color.White;
                        sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 4, 0].Text = Convert.ToString(sno);
                        sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 4, 0].HorizontalAlign = HorizontalAlign.Center;

                        sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 4, 1].Text = " " + Rollno + " ";
                        sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 4, 1].Margin.Left = 15;
                        sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 4, 2].Text = " " + Regno + " ";
                        sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 4, 2].Margin.Left = 15;
                        sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 4, 3].Text = " " + studentname + " ";
                        sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 4, 4].Text = "I";
                        sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 3, 4].Text = "E";
                        sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 2, 4].Text = "T";
                        sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 1, 4].Text = "R";
                        sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 4, 4].HorizontalAlign = HorizontalAlign.Center;
                        sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 3, 4].HorizontalAlign = HorizontalAlign.Center;
                        sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 2, 4].HorizontalAlign = HorizontalAlign.Center;
                        sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 4, 1].HorizontalAlign = HorizontalAlign.Center;
                        sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 4, 2].HorizontalAlign = HorizontalAlign.Center;




                        //======================
                        //foreach (DictionaryEntry parameter in hashsubjectno)
                        //{

                        //    int subjectno = Convert.ToInt32(parameter.Key);
                        //    string subjectcode = Convert.ToString(parameter.Value);
                        //   hashsubjectno1.Add(subjectno,subjectcode);

                        //}

                        //foreach (DictionaryEntry parameter in hashsubjectno1)
                        //{

                        //    int subjectno = Convert.ToInt32(parameter.Key);
                        //    string subjectcode = Convert.ToString(parameter.Value);
                        //    hashsubjectno2.Add(subjectno, subjectcode);
                        //}
                        //foreach (DictionaryEntry parameter in hashsubjectno2)
                        //{

                        //    int subjectno = Convert.ToInt32(parameter.Key);
                        //    string subjectcode = Convert.ToString(parameter.Value);
                        //    hashsubjectno3.Add(subjectno, subjectcode);
                        //}
                        foreach (DictionaryEntry parameter in hashsubjectno)
                        {
                            count++;
                            int subjectno = Convert.ToInt32(parameter.Key);
                            string subjectcode = Convert.ToString(parameter.Value);
                            subjectno = Convert.ToInt32(sprdmarksheet.Sheets[0].ColumnHeader.Cells[6, 4 + count].Tag);
                            subjectcode = sprdmarksheet.Sheets[0].ColumnHeader.Cells[6, 4 + count].Note;
                            SqlCommand markcmd = new SqlCommand("proctabulatedmark", con1);
                            markcmd.CommandType = CommandType.StoredProcedure;
                            markcmd.Parameters.AddWithValue("@degreecode", degreecode);
                            markcmd.Parameters.AddWithValue("@currentsemester", semester);
                            markcmd.Parameters.AddWithValue("@roll_no", Rollno);
                            markcmd.Parameters.AddWithValue("@subject_no", subjectno);
                            markcmd.Parameters.AddWithValue("@batchyear", batchyear);
                            SqlDataAdapter damark = new SqlDataAdapter(markcmd);
                            DataSet exammarkds = new DataSet();
                            damark.Fill(exammarkds);
                            if (exammarkds.Tables[0].Rows.Count > 0)
                            {
                                string Im = "";
                                string Em = "";
                                string T = "";
                                string R = "";
                                for (int i1 = 0; i1 < exammarkds.Tables[0].Rows.Count; i1++)
                                {
                                    Im = exammarkds.Tables[0].Rows[i1]["CA"].ToString();
                                    Em = exammarkds.Tables[0].Rows[i1]["EA"].ToString();
                                    T = exammarkds.Tables[0].Rows[i1]["T"].ToString();
                                    R = exammarkds.Tables[0].Rows[i1]["R"].ToString();
                                    // for result option
                                    if (result0 == 0)
                                    {
                                        result0 = 1;
                                        if (R == "Pass")
                                        {
                                            result = "Pass";
                                            r1 = R;
                                        }
                                        else
                                        {
                                            result = "Fail";
                                        }

                                    }
                                    else if (r1 != R)
                                    {
                                        result = "Fail";
                                    }
                                    // for failure subject no
                                    total = total + Convert.ToDouble(T);
                                    if (R == "Fail")
                                    {
                                        if (failsubno == "")
                                        {
                                            failsubno = Convert.ToString(subjectcode);
                                        }
                                        else
                                        {
                                            failsubno = failsubno + "," + " " + Convert.ToString(subjectcode);
                                        }
                                    }


                                    sprdmarksheet.Sheets[0].SpanModel.Add(sprdmarksheet.Sheets[0].RowCount - 4, sprdmarksheet.Sheets[0].ColumnCount - 1, 4, 1);
                                    sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 4, sprdmarksheet.Sheets[0].ColumnCount - 1].Border.BorderColorBottom = Color.LightBlue;
                                    sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 4, 4 + count].Text = Im;
                                    sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 3, 4 + count].Text = Em;
                                    sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 2, 4 + count].Text = T;
                                    sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 1, 4 + count].Text = R;
                                    sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 1, sprdmarksheet.Sheets[0].ColumnCount - 2].Text = result;

                                    sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 4, sprdmarksheet.Sheets[0].ColumnCount - 1].Text = failsubno;
                                    sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 4, sprdmarksheet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 4, 4 + count].HorizontalAlign = HorizontalAlign.Center;
                                    sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 3, 4 + count].HorizontalAlign = HorizontalAlign.Center;
                                    sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 2, 4 + count].HorizontalAlign = HorizontalAlign.Center;
                                    sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 1, 4 + count].HorizontalAlign = HorizontalAlign.Center;
                                    sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 2, sprdmarksheet.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                                    sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 1, sprdmarksheet.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                            else
                            {
                                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 4, 4 + count].Text = "-";
                                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 3, 4 + count].Text = "-";
                                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 2, 4 + count].Text = "-";
                                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 1, 4 + count].Text = "-";
                                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 2, sprdmarksheet.Sheets[0].ColumnCount - 2].Text = "-";
                                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 4, 4 + count].HorizontalAlign = HorizontalAlign.Center;
                                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 3, 4 + count].HorizontalAlign = HorizontalAlign.Center;
                                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 2, 4 + count].HorizontalAlign = HorizontalAlign.Center;
                                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 1, 4 + count].HorizontalAlign = HorizontalAlign.Center;
                                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 2, sprdmarksheet.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 2, sprdmarksheet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                        if (sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 1, sprdmarksheet.Sheets[0].ColumnCount - 2].Text == "Pass")
                        {
                            sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 2, sprdmarksheet.Sheets[0].ColumnCount - 2].Text = Convert.ToString(total);
                        }



                    }

                }
                if (examds.Tables[2].Rows.Count > 0)
                {
                    string exammonth = "";
                    string examyear = "";

                    for (int i3 = 0; i3 < examds.Tables[2].Rows.Count; i3++)
                    {
                        exammonth = examds.Tables[2].Rows[i3]["exammonth"].ToString();
                        examyear = examds.Tables[2].Rows[i3]["examyear"].ToString();
                    }
                    string exammonth1 = "";
                    if (exammonth == "1")
                        exammonth1 = "Jan";
                    else if (exammonth == "2")
                        exammonth1 = "Feb";
                    else if (exammonth == "3")
                        exammonth1 = "Mar";
                    else if (exammonth == "4")
                        exammonth1 = "Apr";
                    else if (exammonth == "5")
                        exammonth1 = "May";
                    else if (exammonth == "6")
                        exammonth1 = "Jun";
                    else if (exammonth == "7")
                        exammonth1 = "Jul";
                    else if (exammonth == "8")
                        exammonth1 = "Aug";
                    else if (exammonth == "9")
                        exammonth1 = "Sep";
                    else if (exammonth == "10")
                        exammonth1 = "OCT";
                    else if (exammonth == "11")
                        exammonth1 = "Nov";
                    else if (exammonth == "12")
                        exammonth1 = "Dec";
                    sprdmarksheet.Sheets[0].ColumnHeader.Cells[4, 0].Text = "Month & Year" + " " + ":" + " " + exammonth1 + " " + examyear;
                }
                int sno1 = 0;
                sprdmarksheet.Sheets[0].RowCount = sprdmarksheet.Sheets[0].RowCount + 2;
                sprdmarksheet.Sheets[0].SpanModel.Add(sprdmarksheet.Sheets[0].RowCount - 2, 0, 1, sprdmarksheet.Sheets[0].ColumnCount);
                sprdmarksheet.Sheets[0].SpanModel.Add(sprdmarksheet.Sheets[0].RowCount - 1, 0, 1, sprdmarksheet.Sheets[0].ColumnCount);
                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 1, 0].Text = "Courses";
                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                sprdmarksheet.Sheets[0].Rows[sprdmarksheet.Sheets[0].RowCount - 2].Border.BorderColorBottom = Color.White;
                sprdmarksheet.Sheets[0].Rows[sprdmarksheet.Sheets[0].RowCount - 1].Border.BorderColorBottom = Color.White;
                int count1 = 0;
                foreach (DictionaryEntry parameter in hashsubjectcode)
                {
                    count1++;
                    sno1++;
                    sprdmarksheet.Sheets[0].RowCount = sprdmarksheet.Sheets[0].RowCount + 1;
                    string subjectcode = Convert.ToString(parameter.Key);
                    string subjectname = Convert.ToString(parameter.Value);
                    subjectname = Convert.ToString(sprdmarksheet.Sheets[0].ColumnHeader.Cells[5, 4 + count1].Tag);
                    subjectcode = sprdmarksheet.Sheets[0].ColumnHeader.Cells[6, 4 + count1].Note;
                    sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno1);
                    sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    if (Session["Rollflag"] == "1")
                    {
                        sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(subjectcode);
                        sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    }
                    if (Session["Regflag"] == "1")
                    {
                        if (Session["Rollflag"] == "1")
                        {
                            sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 1, 1].Text = " " + Convert.ToString(subjectcode);
                            sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        }
                        else
                        {
                            sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(subjectcode);
                            sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }

                    sprdmarksheet.Sheets[0].SpanModel.Add(sprdmarksheet.Sheets[0].RowCount - 1, 3, 1, sprdmarksheet.Sheets[0].ColumnCount);
                    sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 1, 3].Margin.Left = 15;
                    sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 1, 3].Text = " " + subjectname;
                    sprdmarksheet.Sheets[0].Rows[sprdmarksheet.Sheets[0].RowCount - 1].Border.BorderColorBottom = Color.White;
                    sprdmarksheet.Sheets[0].Rows[sprdmarksheet.Sheets[0].RowCount - 1].Border.BorderColorRight = Color.White;

                }
                sprdmarksheet.Sheets[0].RowCount = sprdmarksheet.Sheets[0].RowCount + 5;
                sprdmarksheet.Sheets[0].SpanModel.Add(sprdmarksheet.Sheets[0].RowCount - 4, 0, 1, 3);
                sprdmarksheet.Sheets[0].SpanModel.Add(sprdmarksheet.Sheets[0].RowCount - 3, 0, 1, 3);
                sprdmarksheet.Sheets[0].SpanModel.Add(sprdmarksheet.Sheets[0].RowCount - 2, 0, 1, 3);
                sprdmarksheet.Sheets[0].SpanModel.Add(sprdmarksheet.Sheets[0].RowCount - 1, 0, 1, 3);
                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 4, 0].Text = "I" + " " + ":" + " " + "INTERNAL";
                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 3, 0].Text = "E" + " " + ":" + " " + "EXTERNAL";
                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 2, 0].Text = "T" + " " + ":" + " " + "TOTAL";
                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 1, 0].Text = "R" + " " + ":" + " " + "RESULT";
                sprdmarksheet.Sheets[0].SpanModel.Add(sprdmarksheet.Sheets[0].RowCount - 5, 0, 1, sprdmarksheet.Sheets[0].ColumnCount);
                sprdmarksheet.Sheets[0].SpanModel.Add(sprdmarksheet.Sheets[0].RowCount - 4, 3, 1, sprdmarksheet.Sheets[0].ColumnCount);
                sprdmarksheet.Sheets[0].SpanModel.Add(sprdmarksheet.Sheets[0].RowCount - 3, 3, 1, sprdmarksheet.Sheets[0].ColumnCount);
                sprdmarksheet.Sheets[0].SpanModel.Add(sprdmarksheet.Sheets[0].RowCount - 2, 3, 1, sprdmarksheet.Sheets[0].ColumnCount);
                sprdmarksheet.Sheets[0].SpanModel.Add(sprdmarksheet.Sheets[0].RowCount - 1, 3, 1, sprdmarksheet.Sheets[0].ColumnCount);
                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 4, 3].Margin.Left = 15;
                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 3, 3].Margin.Left = 15;
                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 2, 3].Margin.Left = 15;
                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 4, 3].Text = "NC" + " " + ":" + " " + "NOT CLEARED";
                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 3, 3].Text = "NE" + " " + ":" + " " + "NOT ELIGIBLE";
                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 2, 3].Text = "A" + " " + ":" + " " + "ABSENT";
                sprdmarksheet.Sheets[0].Rows[sprdmarksheet.Sheets[0].RowCount - 5].Border.BorderColorBottom = Color.White;
                sprdmarksheet.Sheets[0].Rows[sprdmarksheet.Sheets[0].RowCount - 4].Border.BorderColorBottom = Color.White;
                sprdmarksheet.Sheets[0].Rows[sprdmarksheet.Sheets[0].RowCount - 3].Border.BorderColorBottom = Color.White;
                sprdmarksheet.Sheets[0].Rows[sprdmarksheet.Sheets[0].RowCount - 2].Border.BorderColorBottom = Color.White;
                sprdmarksheet.Sheets[0].Rows[sprdmarksheet.Sheets[0].RowCount - 1].Border.BorderColorBottom = Color.White;
                sprdmarksheet.Sheets[0].Rows[sprdmarksheet.Sheets[0].RowCount - 5].Border.BorderColorRight = Color.White;
                sprdmarksheet.Sheets[0].Rows[sprdmarksheet.Sheets[0].RowCount - 4].Border.BorderColorRight = Color.White;
                sprdmarksheet.Sheets[0].Rows[sprdmarksheet.Sheets[0].RowCount - 3].Border.BorderColorRight = Color.White;
                sprdmarksheet.Sheets[0].Rows[sprdmarksheet.Sheets[0].RowCount - 2].Border.BorderColorRight = Color.White;
                sprdmarksheet.Sheets[0].Rows[sprdmarksheet.Sheets[0].RowCount - 1].Border.BorderColorRight = Color.White;


                sprdmarksheet.Sheets[0].RowCount = sprdmarksheet.Sheets[0].RowCount + 4;
                sprdmarksheet.Sheets[0].SpanModel.Add(sprdmarksheet.Sheets[0].RowCount - 4, 0, 1, sprdmarksheet.Sheets[0].ColumnCount);
                sprdmarksheet.Sheets[0].SpanModel.Add(sprdmarksheet.Sheets[0].RowCount - 3, 0, 1, sprdmarksheet.Sheets[0].ColumnCount);
                sprdmarksheet.Sheets[0].SpanModel.Add(sprdmarksheet.Sheets[0].RowCount - 2, 0, 1, sprdmarksheet.Sheets[0].ColumnCount);
                sprdmarksheet.Sheets[0].SpanModel.Add(sprdmarksheet.Sheets[0].RowCount - 1, 0, 1, sprdmarksheet.Sheets[0].ColumnCount);
                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 3, 0].Margin.Left = 15;
                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 2, 0].Margin.Left = 15;
                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 3, 0].Text = "Minimum  Marks for Pass in  Each Course - 40 (Theory) and  50 ( Pratical) (both I & E put together)";
                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 2, 0].Text = "Minimum  Marks for  Each Course  in External Examination - 30 (Theory) /38 ( Pratical) /50 (Pratical - PD AUTO)";
                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 3, 0].Font.Bold = true;
                sprdmarksheet.Sheets[0].Cells[sprdmarksheet.Sheets[0].RowCount - 2, 0].Font.Bold = true;
                sprdmarksheet.Sheets[0].Rows[sprdmarksheet.Sheets[0].RowCount - 4].Border.BorderColorBottom = Color.White;
                sprdmarksheet.Sheets[0].Rows[sprdmarksheet.Sheets[0].RowCount - 3].Border.BorderColorBottom = Color.White;
                sprdmarksheet.Sheets[0].Rows[sprdmarksheet.Sheets[0].RowCount - 2].Border.BorderColorBottom = Color.White;
                sprdmarksheet.Sheets[0].Rows[sprdmarksheet.Sheets[0].RowCount - 1].Border.BorderColorBottom = Color.White;
                sprdmarksheet.Sheets[0].RowCount = sprdmarksheet.Sheets[0].RowCount;
                int totalrows = sprdmarksheet.Sheets[0].RowCount;
                sprdmarksheet.Sheets[0].PageSize = totalrows;
                sprdmarksheet.Height = totalrows;
            }
            else
            {
                btnexcel.Visible = false;
                lblnorec.Visible = true;
                sprdmarksheet.Visible = false;
            }
            if (Session["Rollflag"] == "0")
            {
                sprdmarksheet.Sheets[0].Columns[1].Visible = false;
            }
            if (Session["Regflag"] == "0")
            {
                sprdmarksheet.Sheets[0].Columns[2].Visible = false;
            }

        }
        catch
        {

        }
    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }
    protected void btnexcel_Click(object sender, EventArgs e)
    {
        //string websitepath=Request.
        //sprdmarksheet.SaveExcel("c:\\ConsolidatedMarkSheet.xls", websitepath);

        //sprdmarksheet.SaveExcel("c:\App1.xls", FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
        //sprdmarksheet.SaveExcel("c:\App2.xls", FarPoint.Web.Spread.Model.IncludeHeaders.RowHeadersCustomOnly);
        // string appPath = HttpContext.Current.Request.ApplicationPath;
        string appPath = HttpContext.Current.Server.MapPath("~");
        if (appPath != "")
        {
            int i = 1;
            appPath = appPath.Replace("\\", "/");
        //sprdmarksheet.SaveExcel(appPath + "/ConsolidatedMarkSheet.xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet
        //appPath = "~/" + appPath;
        e:
            try
            {
                string print = "ConsolidatedMarkSheet" + i;
                //sprdmarksheet.SaveExcel(appPath + "/ConsolidatedMarkSheet.xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet
                //Aruna on 26feb2013============================
                string szPath = appPath + "/Report/";
                string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")

                sprdmarksheet.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/vnd.ms-excel";
                Response.Flush();
                Response.WriteFile(szPath + szFile);
                //=============================================
            }
            catch
            {
                goto e;
                i++;
            }

        }


    }
}