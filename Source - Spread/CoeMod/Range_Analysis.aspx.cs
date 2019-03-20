using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using FarPoint.Web.Spread;



public partial class Range_Analysis : System.Web.UI.Page
{

    [Serializable()]
    public class MyImg : ImageCellType
    {

        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(110);
            return img;
        }
    }
    public class MyImg1 : ImageCellType
    {

        //public override Control paintcell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, Boolean upperLevel)
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(80);
            img.Height = Unit.Percentage(90);
            return img;


        }
    }

    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_p = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_app = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlCommand cmd;
    DAccess2 d2 = new DAccess2();
    string batchyear_value = "";
    string degree_value = "";
    string s4_degreecode = "";
    string query1 = "";
    int branch_cnt = 0;
    int i = 0;
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {

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
            if (!Page.IsPostBack)
            {
                btnxl.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnprintmaster.Visible = false;
                Printcontrol.Visible = false;
                bindbatch();
                binddegree();
                bindbranch();
                FpSpread1.Visible = false;
                RadioHeader.Visible = false;
                Radiowithoutheader.Visible = false;
                lblpages.Visible = false;
                ddlpage.Visible = false;

                FpSpread1.Sheets[0].ColumnHeader.RowCount = 7;
                FpSpread1.Sheets[0].ColumnCount = 9;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FpSpread1.Sheets[0].AutoPostBack = true;
                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
                FpSpread1.CommandBar.Visible = true;

                ddlMonth.Items.Clear();
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

                int year = Convert.ToInt16(DateTime.Today.Year);
                ddlYear.Items.Clear();
                for (int l = 0; l <= 20; l++)
                {
                    ddlYear.Items.Add(Convert.ToString(year - l));
                }
                ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
            }
            lblerror.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
    }
    public void bindbatch()
    {

        ddlbatch.Items.Clear();

        string sqlstring = "";
        int max_bat = 0;
        con.Close();
        con.Open();
        cmd = new SqlCommand(" select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year", con);
        SqlDataAdapter da1 = new SqlDataAdapter(cmd);
        DataSet ds1 = new DataSet();
        da1.Fill(ds1);
        ddlbatch.DataSource = ds1;
        ddlbatch.DataValueField = "batch_year";
        ddlbatch.DataTextField = "batch_year";
        ddlbatch.DataBind();
        ddlbatch.Items.Insert(0, "ALL");

        //----------------display max year value 
        sqlstring = "select max(batch_year) from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' ";
        max_bat = Convert.ToInt32(GetFunction(sqlstring));
        ddlbatch.SelectedValue = max_bat.ToString();
        con.Close();
        //binddegree();

    }
    public void binddegree()
    {
        ////degree
        ddldegree.Items.Clear();
        con.Close();
        con.Open();
        string collegecode = Session["collegecode"].ToString();
        string usercode = Session["usercode"].ToString();

        DataSet ds = Bind_Degree(collegecode, usercode);
        ddldegree.DataSource = ds;
        ddldegree.DataValueField = "course_id";
        ddldegree.DataTextField = "course_name";
        ddldegree.DataBind();
        ddldegree.Items.Insert(0, "ALL");
        //bindbranch();

    }
    public DataSet Bind_Degree(string college_code, string user_code)
    {
        SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        dcon.Close();
        dcon.Open();
        string dddd = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "";
        SqlCommand cmd = new SqlCommand(dddd, dcon);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }
    public void bindbranch()
    {
        if (ddldegree.SelectedItem.Text == "ALL")
        {
            ddlbranch.Items.Clear();
            con.Close();
            con.Open();
            string collegecode = Session["collegecode"].ToString();
            string usercode = Session["usercode"].ToString();
            string course_id = ddldegree.SelectedValue.ToString();
            DataSet ds = Bind_branch_new(course_id, collegecode.ToString(), usercode);
            ddlbranch.DataSource = ds;
            ddlbranch.DataTextField = "dept_name";
            ddlbranch.DataValueField = "degree_code";
            ddlbranch.DataBind();
            ddlbranch.Items.Insert(0, "ALL");
            con.Close();
        }
        else
        {
            ddlbranch.Items.Clear();
            con.Close();
            con.Open();
            string collegecode = Session["collegecode"].ToString();
            string usercode = Session["usercode"].ToString();
            string course_id = ddldegree.SelectedValue.ToString();
            DataSet ds = Bind_branch(course_id, collegecode.ToString(), usercode);
            ddlbranch.DataSource = ds;
            ddlbranch.DataTextField = "dept_name";
            ddlbranch.DataValueField = "degree_code";
            ddlbranch.DataBind();
            ddlbranch.Items.Insert(0, "ALL");
            con.Close();
        }
    }
    public DataSet Bind_branch(string degree_code, string college_code, string user_code)
    {
        SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        dcon.Close();
        dcon.Open();
        string ffff = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + degree_code + " and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "";

        SqlCommand cmd = new SqlCommand(ffff, dcon);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }
    public DataSet Bind_branch_new(string degree_code, string college_code, string user_code)
    {
        SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        dcon.Close();
        dcon.Open();
        string ffff = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "";
        SqlCommand cmd = new SqlCommand(ffff, dcon);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }
    public string GetFunction(string sql)
    {
        string s;
        SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        con1.Open();
        SqlCommand com3 = new SqlCommand(sql, con1);
        SqlDataReader dr5;
        dr5 = com3.ExecuteReader();
        dr5.Read();
        if (dr5.HasRows == true)
        {
            if (dr5[0].ToString() == null)
            {
                s = "";
            }
            else
            {
                s = dr5[0].ToString();
            }
        }
        else
        {
            s = "";
        }

        con1.Close();
        return s;
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            txtexcelname.Text = "";
            btnxl.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            Hashtable hat = new Hashtable();
            batchyear_value = ddlbatch.SelectedValue.ToString();
            degree_value = ddldegree.SelectedValue.ToString();
            s4_degreecode = ddlbranch.SelectedValue.ToString();
            int sl_no = 1;
            string batchyearquery = "";
            if (ddlYear.SelectedValue == "0")
            {
                FpSpread1.Visible = false;
                lblerror.Text = "Please Select Exam Year";
                lblerror.Visible = true;
                return;
            }
            if (ddlMonth.SelectedValue == "0")
            {
                FpSpread1.Visible = false;
                lblerror.Text = "Please Select Exam Month";
                lblerror.Visible = true;
                return;
            }
            string strgdare = d2.GetFunction("select distinct grade_flag from  grademaster where exam_month=5 and exam_year=2015");
            logo_set(strgdare);

            batchyearquery = "select distinct r.batch_year,e.current_semester,r.degree_code,dept_name,course_name+'-'+dept_acronym as branch,e.exam_code from registration r,exam_details e,department dept,degree d,course c where ";
            batchyearquery = batchyearquery + " e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "'";
            if (ddlbranch.SelectedItem.ToString() != "ALL")
            {
                batchyearquery = batchyearquery + " and r.degree_code=" + ddlbranch.SelectedValue.ToString() + "";
            }
            if (ddlbatch.SelectedItem.ToString() != "ALL")
            {
                batchyearquery = batchyearquery + " and r.batch_year=" + ddlbatch.SelectedValue.ToString() + "";
            }
            if (ddldegree.SelectedItem.Text != "ALL")
            {
                batchyearquery = batchyearquery + " and c.course_id=" + ddldegree.SelectedValue.ToString() + "";
            }
            batchyearquery = batchyearquery + " and d.course_id=c.course_id and e.batch_year=r.batch_year  and d.degree_code=e.degree_code and e.degree_code=r.degree_code and d.dept_code=dept.dept_code  and r.college_code=" + Session["collegecode"].ToString() + " and cc=0 and delflag=0 and exam_flag <> 'DEBAR' order by r.batch_year desc";

            DataSet dsbatchyearquery = d2.select_method_wo_parameter(batchyearquery, "text");

            if (dsbatchyearquery.Tables[0].Rows.Count > 0)
            {
                btnxl.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnprintmaster.Visible = true;
                string batchyear = "";
                string current_sem = "";
                string degreecode = "";
                string dept_name = "";
                string examCode = "";
                for (int batchi = 0; batchi < dsbatchyearquery.Tables[0].Rows.Count; batchi++)
                {
                    batchyear = dsbatchyearquery.Tables[0].Rows[batchi]["batch_year"].ToString();
                    current_sem = dsbatchyearquery.Tables[0].Rows[batchi]["current_semester"].ToString();
                    examCode = dsbatchyearquery.Tables[0].Rows[batchi]["exam_code"].ToString();
                    dept_name = dsbatchyearquery.Tables[0].Rows[batchi]["branch"].ToString();
                    degreecode = dsbatchyearquery.Tables[0].Rows[batchi]["degree_code"].ToString();

                    hat.Clear();
                    hat.Add("degreecode_p", degreecode);
                    hat.Add("batchyear_p", batchyear);
                    hat.Add("semester_p", current_sem);
                    hat.Add("examcode_p", examCode);
                    DataSet studinfoads = new DataSet();

                    if (strgdare == "2")
                    {
                        studinfoads = d2.select_method("proc_range_grade", hat, "sp");
                        if (studinfoads.Tables[0].Rows.Count > 0)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sl_no.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = batchyear.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dept_name.ToString();
                            int stucoun = 0;
                            for (int c = 3; c < FpSpread1.Sheets[0].ColumnCount - 1; c++)
                            {
                                studinfoads.Tables[0].DefaultView.RowFilter = "Mark_Grade='" + FpSpread1.Sheets[0].ColumnHeader.Cells[0, c].Text.ToString() + "'";
                                DataView dvg = studinfoads.Tables[0].DefaultView;
                                string value = "0";
                                if (dvg.Count > 0)
                                {
                                    value = dvg[0]["stucount"].ToString();
                                }
                                stucoun = stucoun + Convert.ToInt32(value);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = value.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = stucoun.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            sl_no++;
                        }
                    }
                    else
                    {
                        studinfoads = d2.select_method("proc_range", hat, "sp");
                        if (studinfoads.Tables[0].Rows.Count > 0)
                        {
                            int fourth = 0;
                            int five = 0;
                            int six = 0;
                            int seven = 0;
                            int eight = 0;
                            int nine = 0;

                            for (int studproci = 0; studproci < studinfoads.Tables[0].Rows.Count; studproci++)
                            {
                                fourth = int.Parse(studinfoads.Tables[0].Rows[0][0].ToString());
                                five = int.Parse(studinfoads.Tables[1].Rows[0][0].ToString());
                                six = int.Parse(studinfoads.Tables[2].Rows[0][0].ToString());
                                seven = int.Parse(studinfoads.Tables[3].Rows[0][0].ToString());
                                eight = int.Parse(studinfoads.Tables[4].Rows[0][0].ToString());

                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sl_no.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = batchyear.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dept_name.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = fourth.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = five.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = six.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = seven.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = eight.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                nine = fourth + five + six + seven + eight;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = nine.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                sl_no++;
                            }
                            nine = 0;
                        }
                    }
                }

                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "G.TOTAL";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                branch_cnt = sl_no - 1;

                double s = 0;
                for (int colcount = 3; colcount < FpSpread1.Sheets[0].ColumnCount; colcount++)
                {
                    int degreecount1 = branch_cnt;
                    for (int fintot = 0; fintot < branch_cnt; fintot++)
                    {
                        string a1 = FpSpread1.Sheets[0].Cells[((FpSpread1.Sheets[0].RowCount - 1) - degreecount1), colcount].Text;
                        double d = 0;
                        if (double.TryParse(a1, out d))
                        {
                            s = s + Convert.ToDouble(a1);
                        }
                        degreecount1--;
                    }

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString(s);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount].Font.Size = FontUnit.Medium;
                    s = 0;
                }

                FpSpread1.Visible = true;
                RadioHeader.Visible = true;
                Radiowithoutheader.Visible = true;
                lblpages.Visible = true;
                ddlpage.Visible = true;
                lblerror.Visible = false;
            }
            else
            {
                RadioHeader.Visible = false;
                Radiowithoutheader.Visible = false;
                lblpages.Visible = false;
                ddlpage.Visible = false;
                FpSpread1.Visible = false;
                lblerror.Visible = true;
                lblerror.Text = "No Records Found";
            }
            RadioHeader.Visible = false;
            Radiowithoutheader.Visible = false;
            lblpages.Visible = false;
            ddlpage.Visible = false;
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }
    public void logo_set(string strgdare)
    {
        FpSpread1.Sheets[0].ColumnHeader.RowCount = 0;
        FpSpread1.Sheets[0].ColumnCount = 0;
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].ColumnCount = 3;
        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch Year";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Branch";
        if (strgdare == "2")
        {

            string stqu = " select distinct Mark_Grade,frange from grade_master";
            if (ddlbatch.SelectedItem.ToString() != "ALL" && ddlbranch.SelectedItem.Text != "ALL")
            {
                stqu = stqu + " where Degree_Code='" + ddlbranch.SelectedValue.ToString() + "' and batch_year='" + ddlbatch.SelectedItem.ToString() + "'";
            }
            else if (ddlbatch.SelectedItem.ToString() == "ALL" && ddlbranch.SelectedItem.Text != "ALL")
            {
                stqu = stqu + " where Degree_Code='" + ddlbranch.SelectedValue.ToString() + "'";
            }
            else if (ddlbatch.SelectedItem.ToString() != "ALL" && ddlbranch.SelectedItem.Text == "ALL")
            {
                stqu = stqu + " where batch_year='" + ddlbatch.SelectedItem.ToString() + "'";
            }
            stqu = stqu + " order by Frange";
            DataSet dssr = d2.select_method_wo_parameter(stqu, "Text");
            for (int i = 0; i < dssr.Tables[0].Rows.Count; i++)
            {
                FpSpread1.Sheets[0].ColumnCount++;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = dssr.Tables[0].Rows[i]["Mark_Grade"].ToString();
            }
            FpSpread1.Sheets[0].ColumnCount++;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";
        }
        else
        {
            FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 6;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Below 40";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "40-59";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "60-74";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "75-90";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Above 90";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Total";
        }

    }

    protected void RadioHeader_CheckedChanged(object sender, EventArgs e)
    {
        ddlpage.Items.Clear();

        int totrowcount = FpSpread1.Sheets[0].RowCount;
        int pages = totrowcount / 30;
        int intialrow = 1;
        int remainrows = totrowcount % 30;
        if (FpSpread1.Sheets[0].RowCount > 0)
        {
            int i5 = 0;
            ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("0", "0"));


            for (i = 1; i <= pages; i++)
            {
                i5 = i;

                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                intialrow = intialrow + 30;
            }
            if (remainrows > 0)
            {
                i = i5 + 1;
                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
            }
        }

        RadioHeader.Visible = true;
        Radiowithoutheader.Visible = true;
        lblpages.Visible = true;
        ddlpage.Visible = true;
    }
    protected void Radiowithoutheader_CheckedChanged(object sender, EventArgs e)
    {
        ddlpage.Items.Clear();
        int totrowcount = FpSpread1.Sheets[0].RowCount;
        int pages = totrowcount / 35;
        int intialrow = 1;
        int remainrows = totrowcount % 35;
        if (FpSpread1.Sheets[0].RowCount > 0)
        {
            int i5 = 0;

            ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("0", "0"));
            for (int i = 1; i <= pages; i++)
            {
                i5 = i;

                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                intialrow = intialrow + 35;
            }
            if (remainrows > 0)
            {
                i = i5 + 1;
                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
            }
        }

        RadioHeader.Visible = true;
        Radiowithoutheader.Visible = true;
        lblpages.Visible = true;
        ddlpage.Visible = true;
    }
    protected void ddlpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlpage.SelectedValue.ToString() == "0" && RadioHeader.Checked == true)
        {
            FpSpread1.Sheets[0].ColumnHeader.Rows[0].Visible = true;
            FpSpread1.Sheets[0].ColumnHeader.Rows[1].Visible = true;

            FpSpread1.Sheets[0].ColumnHeader.Rows[2].Visible = true;
            FpSpread1.Sheets[0].ColumnHeader.Rows[3].Visible = true;
            FpSpread1.Sheets[0].ColumnHeader.Rows[4].Visible = true;
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                FpSpread1.Sheets[0].Rows[i].Visible = true;
            }
        }
        else if (ddlpage.SelectedValue.ToString() == "0" && Radiowithoutheader.Checked == true)
        {
            FpSpread1.Sheets[0].ColumnHeader.Rows[0].Visible = true;
            FpSpread1.Sheets[0].ColumnHeader.Rows[1].Visible = true;
            FpSpread1.Sheets[0].ColumnHeader.Rows[2].Visible = true;
            FpSpread1.Sheets[0].ColumnHeader.Rows[3].Visible = true;
            FpSpread1.Sheets[0].ColumnHeader.Rows[4].Visible = true;
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                FpSpread1.Sheets[0].Rows[i].Visible = true;
            }
        }

        else if (RadioHeader.Checked == true)
        {


            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                FpSpread1.Sheets[0].Rows[i].Visible = false;
            }
            int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
            int end = start + 30;//**
            if (end >= FpSpread1.Sheets[0].RowCount)
            {
                end = FpSpread1.Sheets[0].RowCount;
            }
            int rowstart = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(start);
            int rowend = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(end);
            for (int i = start - 1; i < end; i++)
            {
                FpSpread1.Sheets[0].Rows[i].Visible = true;
            }
            FpSpread1.Sheets[0].ColumnHeader.Rows[0].Visible = true;
            FpSpread1.Sheets[0].ColumnHeader.Rows[1].Visible = true;
            FpSpread1.Sheets[0].ColumnHeader.Rows[2].Visible = true;
            FpSpread1.Sheets[0].ColumnHeader.Rows[3].Visible = true;
            FpSpread1.Sheets[0].ColumnHeader.Rows[4].Visible = true;

        }


        else if (Radiowithoutheader.Checked == true)
        {
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                FpSpread1.Sheets[0].Rows[i].Visible = false;
            }
            int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
            int end = start + 35;//***
            if (end >= FpSpread1.Sheets[0].RowCount)
            {
                end = FpSpread1.Sheets[0].RowCount;
            }
            int rowstart = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(start);
            int rowend = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(end);
            for (int i = start - 1; i < end; i++)
            {
                FpSpread1.Sheets[0].Rows[i].Visible = true;
            }
            if (Convert.ToInt32(ddlpage.SelectedValue.ToString()) == 1)
            {
                FpSpread1.Sheets[0].ColumnHeader.Rows[0].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Rows[1].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Rows[2].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Rows[3].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Rows[4].Visible = true;
            }
            else
            {
                FpSpread1.Sheets[0].ColumnHeader.Rows[0].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Rows[1].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Rows[2].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Rows[3].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Rows[4].Visible = false;

            }
        }

        RadioHeader.Visible = true;
        Radiowithoutheader.Visible = true;
        lblpages.Visible = true;
        ddlpage.Visible = true;
        FpSpread1.Visible = true;
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
    }
    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string degreedetails = "Range Analysis Report";
        string pagename = "Range_Analysis.aspx";
        Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
        Printcontrol.Visible = true;
    }
    protected void btnxl_Click(object sender, EventArgs e)
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
                lblerror.Text = "Please Enter Your Report Name";
                lblerror.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }

}