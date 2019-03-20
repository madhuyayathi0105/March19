using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using FarPoint.Web.Spread;


public partial class Result_Analysis_Report : System.Web.UI.Page
{

    [Serializable()]
    public class MyImg : ImageCellType
    {

        //public override Control paintcell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, Boolean upperLevel)
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(110);
            //img.Height = Unit.Percentage(80);
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

    string batchyear_value = "";
    string degree_value = "";
    string s4_degreecode = "";
    string query1 = "";
    string group_user = string.Empty;
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
                bindbatch();
                binddegree();
                bindbranch();
                FpSpread1.Visible = false;


                //  FpSpread1.Sheets[0].SheetCorner.RowCount = 8;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 8;
                FpSpread1.Sheets[0].ColumnCount = 12;
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
            errorlabl.Visible = false;
        }
        catch(Exception ex)
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
        string qryUserCodeOrGroupCode = string.Empty;

        group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);

        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = Convert.ToString(group_semi[0]);
        }
        if ((Convert.ToString(group_user).Trim() != "") && Session["group_code"] != null && Session["single_user"] != null && Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true")
        {
            qryUserCodeOrGroupCode = " and group_code='" + group_user + "'";
        }
        else if (Session["usercode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["usercode"]).Trim()))
        {
            qryUserCodeOrGroupCode = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
        }

        DataSet ds = Bind_Degree(collegecode, qryUserCodeOrGroupCode);
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
        string dddd = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code  " + user_code + "";
        SqlCommand cmd = new SqlCommand(dddd, dcon);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }
    public void bindbranch()
    {
        string qryUserCodeOrGroupCode = string.Empty;

        group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);

        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = Convert.ToString(group_semi[0]);
        }
        if ((Convert.ToString(group_user).Trim() != "") && Session["group_code"] != null && Session["single_user"] != null && Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true")
        {
            qryUserCodeOrGroupCode = " and group_code='" + group_user + "'";
        }
        else if (Session["usercode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["usercode"]).Trim()))
        {
            qryUserCodeOrGroupCode = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
        }

        if (ddldegree.SelectedItem.Text == "ALL")
        {
            ddlbranch.Items.Clear();
            con.Close();
            con.Open();
            string collegecode = Session["collegecode"].ToString();
            string usercode = Session["usercode"].ToString();
            string course_id = ddldegree.SelectedValue.ToString();


            DataSet ds = Bind_branch_new(course_id, collegecode.ToString(), qryUserCodeOrGroupCode);
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
            DataSet ds = Bind_branch(course_id, collegecode.ToString(), qryUserCodeOrGroupCode);
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
        string ffff = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + degree_code + " and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code " + user_code + "";
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
        string ffff = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code " + user_code + "";
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
        FpSpread1.Visible = false;
        batchyear_value = ddlbatch.SelectedValue.ToString();
        degree_value = ddldegree.SelectedValue.ToString();
        s4_degreecode = ddlbranch.SelectedValue.ToString();
        logo_set();
        int fg = 1;
        string batchyearquery = "";
        int papertype = 0;

        if (ddlYear.SelectedValue == "0")
        {
            FpSpread1.Visible = false;
            errorlabl.Text = "Please Select Exam Year";
            errorlabl.Visible = true;
            return;
        }
        if (ddlMonth.SelectedValue == "0")
        {
            FpSpread1.Visible = false;
            errorlabl.Text = "Please Select Exam Month";
            errorlabl.Visible = true;
            return;
        }
        //if (ddlbranch.SelectedItem.Text != "ALL")
        //{
        //    batchyearquery = "select distinct r.batch_year,r.current_semester,e.exam_code from registration r,exam_details e where e.batch_year=r.batch_year  and r.current_semester=e.current_semester and e.degree_code=r.degree_code and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and r.college_code=" + Session["collegecode"].ToString() + "  and cc=0 and delflag=0 and exam_flag <> 'DEBAR' order by r.batch_year desc";
        //}
        //else if (ddlbranch.SelectedItem.Text == "ALL")
        //{
        //    batchyearquery = "select distinct r.batch_year,r.current_semester,r.degree_code,dept_name,course_name+'-'+dept_acronym as branch,e.exam_code from registration r,exam_details e,department dept,degree d,course c where d.course_id=c.course_id and e.batch_year=r.batch_year  and r.current_semester=e.current_semester and d.degree_code=e.degree_code and c.course_id=" + ddldegree.SelectedValue.ToString() + " and e.degree_code=r.degree_code and r.degree_code=dept.dept_code  and r.college_code=" + Session["collegecode"].ToString() + " and cc=0 and delflag=0 and exam_flag <> 'DEBAR' order by r.batch_year desc";
        //    //batchyearquery = "select distinct r.batch_year,r.current_semester,r.degree_code,dept_name,dept_acronym as branch from registration r,exam_details e,department dept,degree d where e.batch_year=r.batch_year and e.degree_code=r.degree_code and r.degree_code=dept.dept_code  and r.college_code=" + Session["collegecode"].ToString() + " and e.exam_month=" + ddlMonth.SelectedValue.ToString() + " and e.exam_year=" + ddlYear.SelectedValue.ToString() + " and cc=0 and delflag=0 and exam_flag <> 'DEBAR' order by r.batch_year desc";
        //}
        //if (ddlbranch.SelectedItem.Text == "ALL" && ddldegree.SelectedItem.Text == "ALL")
        //{
        //    batchyearquery = "select distinct r.batch_year,r.current_semester,r.degree_code,dept_name,c.course_name+' - '+dept_acronym as branch,e.exam_code from registration r,exam_details e,department dept,degree d,course c where e.batch_year=r.batch_year  and r.current_semester=e.current_semester and e.degree_code=r.degree_code and e.degree_code=d.degree_code and c.course_id=d.course_id and r.degree_code=dept.dept_code  and r.college_code=" + Session["collegecode"].ToString() + " and cc=0 and d.dept_code=dept.dept_code and delflag=0 and exam_flag <> 'DEBAR' order by r.batch_year desc";
        //}

        batchyearquery = "select distinct r.batch_year,e.current_semester,r.degree_code,dept_name,course_name+'-'+dept_acronym as branch,e.exam_code from registration r,exam_details e,department dept,degree d,course c where ";
        batchyearquery = batchyearquery + " e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "'";
        if (ddlbranch.SelectedItem.ToString() != "ALL")
        {
            // batchyearquery = batchyearquery + " dept.dept_code=" + ddlbranch.SelectedValue.ToString() + " and";
            batchyearquery = batchyearquery + " And d.Degree_Code=" + ddlbranch.SelectedValue.ToString() + ""; // modify by jairam 11-07-2015
        }
        if (ddlbatch.SelectedItem.ToString() != "ALL")
        {
            batchyearquery = batchyearquery + " And r.batch_year=" + ddlbatch.SelectedValue.ToString() + "";
        }
        if (ddldegree.SelectedItem.Text != "ALL")
        {
            batchyearquery = batchyearquery + " And c.course_id=" + ddldegree.SelectedValue.ToString() + "";
        }
        batchyearquery = batchyearquery + " And d.course_id=c.course_id and e.batch_year=r.batch_year and dept.dept_code=d.dept_code and e.degree_code=r.degree_code and r.degree_code=d.degree_code  and r.college_code=" + Session["collegecode"].ToString() + " and cc=0 and delflag=0 and exam_flag <> 'DEBAR' order by r.batch_year desc";
        SqlDataAdapter dabatchyearquery = new SqlDataAdapter(batchyearquery, con1);
        DataSet dsbatchyearquery = new DataSet();
        con1.Close();
        con1.Open();
        dabatchyearquery.Fill(dsbatchyearquery);

        if (dsbatchyearquery.Tables[0].Rows.Count > 0)
        {

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
                if (ddlbranch.SelectedItem.Text != "ALL")
                {
                    dept_name = ddldegree.SelectedItem.Text + "-" + ddlbranch.SelectedItem.Text;
                    degreecode = ddlbranch.SelectedValue.ToString();
                }
                else if (ddlbranch.SelectedItem.Text == "ALL")
                {
                    dept_name = dsbatchyearquery.Tables[0].Rows[batchi]["branch"].ToString();
                    degreecode = dsbatchyearquery.Tables[0].Rows[batchi]["degree_code"].ToString();
                }
                //con_p.Close();
                //con_p.Open();
                //query1 = "select count(*) FROM exam_application where exam_code=" + examCode + "";
                //SqlCommand com_reg = new SqlCommand(query1, con_p);
                //SqlDataReader dr_reg = com_reg.ExecuteReader();
                //while (dr_reg.Read())
                //{

                //    FpSpread1.Sheets[0].RowCount++;
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dr_reg[0].ToString();
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = batchyear.ToString();
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dept_name.ToString();
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = current_sem.ToString();
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = fg.ToString();
                //    fg++;
                //}

                //}
                //***********
                con_p.Close();
                con_p.Open();
                //=====Rajkumar for time out Exception on 29-5-2018
                string SelectQ = "select count(*) FROM exam_application where exam_code='" + examCode + "' ";
                SelectQ = SelectQ + " select count(distinct m.roll_no) from mark_entry m,registration r Where m.roll_no = r.roll_no And r.delflag = 0 And m.attempts = 1  and m.exam_code in ('" + examCode + "')";//m.exam_code in ('" + examCode + "')//and ltrim(rtrim(type))=''
                SelectQ = SelectQ + "  select  count(distinct roll_no)  from mark_entry where  result = 'Pass' and passorfail=1 and exam_code in ('" + examCode + "') ";//m.exam_code in ('" + examCode + "')//ltrim(rtrim(type))='' and
                SelectQ = SelectQ + "  select  count(distinct roll_no)  from mark_entry where  result = 'Fail' and passorfail=0 and exam_code in ('" + examCode + "') ";//m.exam_code in ('" + examCode + "')//ltrim(rtrim(type))='' and
                //SelectQ = SelectQ + "   select  count(distinct roll_no)  from mark_entry where  result = 'A%' and passorfail=0 and exam_code in ('" + examCode + "' )";//m.exam_code in ('" + examCode + "')//ltrim(rtrim(type))='' and
                SelectQ = SelectQ + "   select  count(distinct roll_no)  from mark_entry where  result like 'A%' and passorfail=0 and exam_code in ('" + examCode + "' )";//modified by rajasekar 17/08/2018
               // ===================
                SqlCommand studinfo = new SqlCommand(SelectQ, con_p);
                //studinfo.CommandType = CommandType.StoredProcedure;
                //studinfo.Parameters.AddWithValue("@degreecode_p", degreecode);
                //studinfo.Parameters.AddWithValue("@batchyear_p", batchyear);
                //studinfo.Parameters.AddWithValue("@semester_p", current_sem);
                //studinfo.Parameters.AddWithValue("@examcode_p", examCode);

                SqlDataAdapter studinfoada = new SqlDataAdapter(studinfo);
                DataSet studinfoads = new DataSet();
                studinfoada.Fill(studinfoads);
                if (studinfoads.Tables[0].Rows.Count > 0)
                {
                    string totalstudents = "";
                    string studentappeared = "";
                    string studentpassed = "";
                    string studentfail = "";
                    string studentabsent = "";
                    string passpercent = "0";
                    string failpercent = "0";
                    int absent = 0;
                    int absentpercent = 0;
                    int studapprdpercent = 0;
                    string type = "";
                    for (int studproci = 0; studproci < studinfoads.Tables[0].Rows.Count; studproci++)
                    {
                        totalstudents = studinfoads.Tables[0].Rows[0][0].ToString();
                        studentappeared = studinfoads.Tables[1].Rows[0][0].ToString();
                        studentpassed = studinfoads.Tables[2].Rows[0][0].ToString();
                        studentfail = studinfoads.Tables[3].Rows[0][0].ToString();
                        studentabsent = studinfoads.Tables[4].Rows[0][0].ToString();
                        int passcount = Convert.ToInt32(studentappeared) - Convert.ToInt32(studentfail);
                        studentpassed = passcount.ToString();
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = batchyear.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dept_name.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = current_sem.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = totalstudents;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = studentappeared;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = studentpassed;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = studentfail;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = studentabsent;

                            int total = Convert.ToInt32(studentpassed) + Convert.ToInt32(studentfail);
                            if (studentappeared != "0")
                            {
                                double studapprdpercent1 = 0;
                                studapprdpercent1 = (Convert.ToDouble(studentappeared) / Convert.ToDouble(totalstudents)) * 100;
                                double studapprdpercent2 = Math.Round(studapprdpercent1, 2);
                                studapprdpercent = Convert.ToInt32(studapprdpercent2);
                            }
                            if (totalstudents != "0")
                            {
                                double absentpercent1 = 0;
                                //absent = Convert.ToInt32(studentabsent) - Convert.ToInt32(studentappeared);
                                absentpercent1 = (Convert.ToDouble(studentabsent) / Convert.ToDouble(totalstudents)) * 100;
                                double absentpercent2 = Math.Round(absentpercent1, 2);
                                absentpercent = Convert.ToInt32(absentpercent2);
                            }
                            if (studentpassed != "0")
                            {
                                double passpercent1 = 0;
                                passpercent1 = Convert.ToDouble((Convert.ToDouble(studentpassed) / total) * 100);
                                double passpercent2 = Math.Round(passpercent1, 2);
                                passpercent = Convert.ToString(passpercent2);
                            }
                            if (studentfail != "0")
                            {
                                double failpercent1 = 0;
                                failpercent1 = Convert.ToDouble((Convert.ToDouble(studentfail) / total) * 100);
                                double failpercent2 = Math.Round(failpercent1, 2);
                                failpercent = Convert.ToString(failpercent2);
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = studapprdpercent.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = passpercent.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = failpercent.ToString();

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Text = absentpercent.ToString();
                        }
                    }
                }
                //*****************
                FpSpread1.Visible = true;
                errorlabl.Visible = false;
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        }
        else
        {
            FpSpread1.Visible = false;
            errorlabl.Text = "No Records Found";
            errorlabl.Visible = true;
        }
    }
    public void logo_set()
    {
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].Columns[0].Width = 50;
        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[1].Width = 150;
        FpSpread1.Sheets[0].Columns[2].Width = 40;
        FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;

        FpSpread1.Sheets[0].Columns[3].Width = 40;
        FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[4].Width = 30;
        FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[5].Width = 30;
        FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[6].Width = 30;
        FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[7].Width = 30;
        FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[8].Width = 30;
        FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[9].Width = 30;
        FpSpread1.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[10].Width = 30;
        FpSpread1.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[11].Width = 30;
        FpSpread1.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Center;

        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, 0, 2, 1);
        FpSpread1.Sheets[0].ColumnHeader.Cells[6, 0].Text = "Batch";

        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, 1, 2, 1);
        FpSpread1.Sheets[0].ColumnHeader.Cells[6, 1].Text = "Branch";

        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, 2, 2, 1);
        FpSpread1.Sheets[0].ColumnHeader.Cells[6, 2].Text = "Semester";

        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, 3, 2, 1);
        FpSpread1.Sheets[0].ColumnHeader.Cells[6, 3].Text = "Total No.of Student";

        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, 4, 1, 2);
        FpSpread1.Sheets[0].ColumnHeader.Cells[6, 4].Text = "Registered";
        FpSpread1.Sheets[0].ColumnHeader.Cells[7, 4].Text = "No.";
        FpSpread1.Sheets[0].ColumnHeader.Cells[7, 5].Text = "%";


        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, 6, 1, 2);
        FpSpread1.Sheets[0].ColumnHeader.Cells[6, 6].Text = "Pass";
        FpSpread1.Sheets[0].ColumnHeader.Cells[7, 6].Text = "No.";
        FpSpread1.Sheets[0].ColumnHeader.Cells[7, 7].Text = "%";

        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, 8, 1, 2);
        FpSpread1.Sheets[0].ColumnHeader.Cells[6, 8].Text = "Fail";
        FpSpread1.Sheets[0].ColumnHeader.Cells[7, 8].Text = "No.";
        FpSpread1.Sheets[0].ColumnHeader.Cells[7, 9].Text = "%";

        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, 10, 1, 2);
        FpSpread1.Sheets[0].ColumnHeader.Cells[6, 10].Text = "Absent";
        FpSpread1.Sheets[0].ColumnHeader.Cells[7, 10].Text = "No.";
        FpSpread1.Sheets[0].ColumnHeader.Cells[7, 11].Text = "%";


        FpSpread1.Sheets[0].ColumnHeader.Rows[6].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].ColumnHeader.Rows[7].HorizontalAlign = HorizontalAlign.Center;
        //=header
        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        style.Font.Size = 10;
        style.Font.Bold = true;
        FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
        MyImg mi = new MyImg();
        mi.ImageUrl = "~/images/10BIT001.jpeg";
        mi.ImageUrl = "Handler/Handler2.ashx?";
        MyImg1 mi2 = new MyImg1();
        mi2.ImageUrl = "~/images/10BIT001.jpeg";
        mi2.ImageUrl = "Handler/Handler5.ashx?";
        string str = "select isnull(collname, ' ') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(district, ' ') as district,isnull(pincode,' ') as pincode from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
        con1.Close();
        con1.Open();
        SqlCommand comm = new SqlCommand(str, con1);
        SqlDataReader drr = comm.ExecuteReader();
        drr.Read();
        string coll_name = Convert.ToString(drr["collname"]);
        string coll_address1 = Convert.ToString(drr["address1"]);
        string coll_address2 = Convert.ToString(drr["address2"]);
        string coll_address3 = Convert.ToString(drr["address3"]);
        string district = Convert.ToString(drr["district"]);
        string pin_code = Convert.ToString(drr["pincode"]);
        string catgory = drr["category"].ToString();
        catgory = "(An " + catgory + " Institution" + " " + "-" + "";
        string affliatedby = drr["affliated"].ToString();
        string affliatedbynew = Regex.Replace(affliatedby, ",", " ");
        string affiliated = catgory + " " + "Affiliated to" + " " + affliatedbynew + ")";
        string address = coll_address1 + "," + " " + coll_address2 + "," + " " + district + "-" + " " + pin_code + ".";

        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 0, 5, 1);
        FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Text = coll_name;
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, FpSpread1.Sheets[0].ColumnCount - 3);
        FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorBottom = Color.White;

        FpSpread1.Sheets[0].ColumnHeader.Cells[2, 1].Text = address;
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, FpSpread1.Sheets[0].ColumnCount - 3);
        FpSpread1.Sheets[0].ColumnHeader.Cells[2, 1].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorBottom = Color.White;

        FpSpread1.Sheets[0].ColumnHeader.Cells[3, 1].Text = affiliated;
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1, FpSpread1.Sheets[0].ColumnCount - 3);
        FpSpread1.Sheets[0].ColumnHeader.Cells[3, 1].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorBottom = Color.White;

        FpSpread1.Sheets[0].ColumnHeader.Cells[4, 1].Text = "Result Analysis";
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(4, 1, 1, FpSpread1.Sheets[0].ColumnCount - 3);
        FpSpread1.Sheets[0].ColumnHeader.Cells[4, 1].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorBottom = Color.White;


        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(5, 1, 1, FpSpread1.Sheets[0].ColumnCount - 3);

        FpSpread1.Sheets[0].ColumnHeader.Cells[5, 1].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].ColumnHeader.Cells[5, 1].ForeColor = Color.FromArgb(64, 64, 255);
        FpSpread1.Sheets[0].ColumnHeader.Rows[0].Border.BorderColorBottom = Color.White;
        FpSpread1.Sheets[0].ColumnHeader.Rows[0].Border.BorderColorRight = Color.White;
        // FpSpread1.Sheets[0].ColumnHeader.Cells[1, 16].Border.BorderColorBottom = Color.Black;
        FpSpread1.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorBottom = Color.Black;
        FpSpread1.Sheets[0].ColumnHeader.Cells[1, 0].Border.BorderColorBottom = Color.Black;
        FpSpread1.Sheets[0].ColumnHeader.Cells[1, 0].Border.BorderColorRight = Color.White;
        FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorRight = Color.White;
        FpSpread1.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorRight = Color.White;
        FpSpread1.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorRight = Color.White;
        FpSpread1.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorRight = Color.White;
        FpSpread1.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorRight = Color.White;
        FpSpread1.Sheets[0].ColumnHeader.Rows[6].BackColor = Color.FromArgb(214, 235, 255);
        FpSpread1.Sheets[0].ColumnHeader.Rows[6].Font.Bold = true;
        FpSpread1.Sheets[0].ColumnHeader.Rows[6].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].ColumnHeader.Rows[7].BackColor = Color.FromArgb(214, 235, 255);
        FpSpread1.Sheets[0].ColumnHeader.Rows[7].Font.Bold = true;
        FpSpread1.Sheets[0].ColumnHeader.Rows[7].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].ColumnHeader.Cells[1, 0].CellType = mi;
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 2, 5, 2);
        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].CellType = mi2;

    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
    }
    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
    }
}