using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using FarPoint.Web.Spread;
using System.Collections.Generic;

public partial class Studentwise_Arrear_List : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_p = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_name = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlCommand cmd;
    SortedDictionary<string, string> arrcount = new SortedDictionary<string, string>();
    //Hashtable arrcount = new Hashtable();
    Hashtable hat = new Hashtable();
    DAccess2 d2 = new DAccess2();
    string batchyear_ddl = "";
    int first = 0;
    string courseid_ddl = "";
    string degreecode_ddl = "";
    string fromsem_ddl = "";
    string tosem_ddl = "";
    string getrollno = "";
    int sl_no = 1;
    string be = "", cse = "";
    string post = "";
    int semarrearcount = 0;
    string stu_name = "";
    string regno = "";
    string batchyeartbl = "";
    string cur_semtbl = "";
    string degreecodetbl = "";
    string rollno = "", deg = "", sem3 = "";




    protected void btnGo_Click(object sender, EventArgs e)
    {
        btnprintmaster.Visible = false;
        arrcount.Clear();
        batchyear_ddl = ddlbatch.SelectedValue.ToString();
        courseid_ddl = ddldegree.SelectedValue.ToString();
        degreecode_ddl = ddlbranch.SelectedValue.ToString();
        fromsem_ddl = ddlfrmsem.SelectedValue.ToString();
        tosem_ddl = ddltosem.SelectedValue.ToString();

        if (Convert.ToInt32(fromsem_ddl) <= Convert.ToInt32(tosem_ddl))
        {
            logo_set();
            FpSpread1.Sheets[0].RowCount = 0;
            lblnorec.Visible = false;
            getrollno = "select distinct current_semester,batch_year,r.degree_code,d.course_id,c.course_name,d.acronym from registration r,degree d,course c where ";
            if (ddlbatch.SelectedItem.Text != "ALL")
            {
                getrollno = getrollno + " batch_year=" + batchyear_ddl + " and ";
            }
            if (ddldegree.SelectedItem.Text != "ALL")
            {
                getrollno = getrollno + "c.course_id=" + courseid_ddl + " and ";
            }
            if (ddlbranch.SelectedItem.Text != "ALL")
            {
                getrollno = getrollno + " r.degree_code= " + degreecode_ddl + " and ";
            }
            //modified by srinath 8/3/2014
            //getrollno = getrollno + " cc=0 and r.degree_code=d.degree_code and c.course_id=d.course_id and delflag =0 and exam_flag <>'Debar' ";
            getrollno = getrollno + "  r.degree_code=d.degree_code and c.course_id=d.course_id and delflag =0 and exam_flag <>'Debar' ";

            SqlDataAdapter dagetrollno = new SqlDataAdapter(getrollno, con);
            DataSet dsgetrollno = new DataSet();
            con.Close();
            con.Open();
            dagetrollno.Fill(dsgetrollno);
            if (0 < dsgetrollno.Tables[0].Rows.Count)
            {
                for (int ff = 0; ff < dsgetrollno.Tables[0].Rows.Count; ff++)
                {
                    arrcount.Clear();
                    batchyeartbl = dsgetrollno.Tables[0].Rows[ff]["batch_year"].ToString();
                    cur_semtbl = dsgetrollno.Tables[0].Rows[ff]["current_semester"].ToString();
                    Session["qqq"] = cur_semtbl;
                    degreecodetbl = dsgetrollno.Tables[0].Rows[ff]["degree_code"].ToString();

                    deg = dsgetrollno.Tables[0].Rows[ff]["course_id"].ToString();
                    be = dsgetrollno.Tables[0].Rows[ff]["course_name"].ToString();

                    cse = dsgetrollno.Tables[0].Rows[ff]["acronym"].ToString();
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = batchyear_ddl + "-" + be + "-" + cse;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].RowCount++;
                    con_name.Close();
                    con_name.Close();
                    SqlCommand studinfo_new = new SqlCommand("proc_stud_name", con_name);
                    studinfo_new.CommandType = CommandType.StoredProcedure;
                    studinfo_new.Parameters.AddWithValue("@batchyear_p", batchyeartbl);
                    studinfo_new.Parameters.AddWithValue("@degreecode_p", degreecodetbl);
                    studinfo_new.Parameters.AddWithValue("@fromsem_ddl_p", fromsem_ddl);
                    studinfo_new.Parameters.AddWithValue("@tosem_ddl_p", tosem_ddl);
                    SqlDataAdapter ada_roll = new SqlDataAdapter(studinfo_new);
                    DataSet ds_roll = new DataSet();
                    ada_roll.Fill(ds_roll);
                    if (0 < ds_roll.Tables[0].Rows.Count)
                    {
                        for (int sub = 0; sub < ds_roll.Tables[0].Rows.Count; sub++)
                        {
                            rollno = ds_roll.Tables[0].Rows[sub]["roll_no"].ToString();
                            stu_name = ds_roll.Tables[0].Rows[sub]["stud_name"].ToString();
                            regno = ds_roll.Tables[0].Rows[sub]["reg_no"].ToString();

                            con_p.Close();
                            con_p.Open();
                            SqlCommand studinfo = new SqlCommand("proc_stud_arrear", con_p);
                            studinfo.CommandType = CommandType.StoredProcedure;

                            studinfo.Parameters.AddWithValue("@rollno_p", rollno);
                            studinfo.Parameters.AddWithValue("@fromsem_ddl_p", fromsem_ddl);
                            studinfo.Parameters.AddWithValue("@tosem_ddl_p", tosem_ddl);
                            SqlDataAdapter daarrsub = new SqlDataAdapter(studinfo);


                            int fsem =Convert.ToInt32(fromsem_ddl);
                            int tosem = Convert.ToInt32(tosem_ddl);
                            DataSet dsarrsub = new DataSet();
                            String qry =
                                "Select isnull(Subject_Code,'') as scode , isnull(subjecT_name,'') as sname , semester from subject,syllabus_master as smas where smas.syll_code = subject.syll_code and subject_no in (select distinct subject_no from mark_entry where subject_no not in (select distinct subject_no from mark_entry where passorfail=1 and result='Pass' and ltrim(rtrim(roll_no))='" + rollno + "')and ltrim(rtrim(roll_no))='" + rollno + "' and semester between " + fsem + " and "+tosem+")  order by smas.semester,scode"; //Added by madhumathi
                            dsarrsub = d2.select_method(qry, hat, "Text");
                            //daarrsub.Fill(dsarrsub);
                            post = "";
                            if (dsarrsub.Tables[0].Rows.Count > 0)
                            {
                                for (int arrsubcount = 0; arrsubcount < dsarrsub.Tables[0].Rows.Count; arrsubcount++)
                                {
                                    string scode =dsarrsub.Tables[0].Rows[arrsubcount]["scode"].ToString();

                                    String get_attempts = "select max(attempts) from mark_entry where subject_no in (select subject_no from subject where subject_code = '" + scode + "')and result<>'pass' and roll_no = '" + rollno + "'";//Added by madhumathi
                                    String att = d2.GetFunction(get_attempts);
                                    semarrearcount = dsarrsub.Tables[0].Rows.Count;
                                    if (post == "")
                                    {
                                        post = dsarrsub.Tables[0].Rows[arrsubcount]["scode"].ToString();
                                        post = post + "+" + dsarrsub.Tables[0].Rows[arrsubcount]["semester"].ToString();
                                        post = post + "+" + att;//Added by madhumathi
                                            
                                            //post = post + "+" +dsarrsub.Tables[0].Rows[arrsubcount]["attempts"].ToString();
                                    }
                                    else
                                    {
                                        post = post + "." + dsarrsub.Tables[0].Rows[arrsubcount]["scode"].ToString();
                                        post = post + "+" + dsarrsub.Tables[0].Rows[arrsubcount]["semester"].ToString();
                                       // post = post + "+" + dsarrsub.Tables[0].Rows[arrsubcount]["attempts"].ToString();
                                        post = post + "+" + att;//Added by madhumathi
                                    }
                                }
                                //Added by srinath 11/3/2014
                                //arrcount.Add(degreecodetbl + "," + batchyeartbl + "," + semarrearcount + "," + rollno + "," + stu_name + "," + regno, post);
                                arrcount.Add(rollno + "," + regno + "," + semarrearcount + "," + degreecodetbl + "," + stu_name + "," + batchyeartbl, post);
                            }
                            else
                            {
                                semarrearcount = 0;
                            }
                            //hidden by srinath 11/3/2014
                            //  arrcount.Add(degreecodetbl + "," + batchyeartbl + "," + semarrearcount + "," + rollno + "," + stu_name + "," + regno, post);
                            Span3.Visible = true;
                            Span2.Visible = true;
                            spnCollegeHeader.Visible = true;
                            Image1.Visible = true;
                            Span1.Visible = true;
                            imgLeftLogo2.Visible = true;

                        }
                    }
                    else
                    {
                        lblnorec.Visible = true;
                        lblnorec.Text = "No Record(s) Found";
                        FpSpread1.Visible = false;
                    }
                }

                foreach (KeyValuePair<string, string> parameter in arrcount)
                //foreach (DictionaryEntry parameter in arrcount)
                {
                    string st_name = "", re_no = "";
                    string b_year = "";
                    string d_code = "";

                    string roll_no_new = "";
                    string subcount = Convert.ToString(parameter.Key);
                    string Rollno = Convert.ToString(parameter.Value);
                    string[] splitsubcount = subcount.Split(new char[] { ',' });

                    d_code = splitsubcount[3].ToString();
                    b_year = splitsubcount[5].ToString();
                    subcount = splitsubcount[2].ToString();

                    roll_no_new = splitsubcount[0].ToString();

                    st_name = splitsubcount[4].ToString();

                    re_no = splitsubcount[1].ToString();

                    for (int i = 0; i <= Convert.ToInt32(subcount); i++)
                    {

                        if (Convert.ToInt32(subcount) == i && b_year == batchyeartbl && d_code == degreecodetbl)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            string[] split = Rollno.Split(new char[] { '.' });

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sl_no.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = roll_no_new.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = re_no.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = st_name.ToString();
                            first = 0;

                            for (int d = 0; d < Convert.ToInt32(subcount); d++)
                            {

                                string fp1 = split[d].ToString();


                                string[] asx = fp1.Split(new char[] { '+' });
                                string fp = asx[0];
                                string gg = asx[1];
                                string atemps = asx[2];

                                string qqq = "select distinct subject_name from subject as S,syllabus_master as SM,subjectchooser as SC,Sub_sem as Sem where S.subject_no=SC.Subject_no and s.syll_code=SM.syll_code and degree_code='" + degreecodetbl + "' and SM.Semester<=" + cur_semtbl + " and batch_year=" + batchyeartbl + " and S.subtype_no = Sem.subtype_no and promote_count=1 and subject_code='" + fp + "'order by subject_name";
                                string aq = GetFunction(qqq);


                                if (first == 0)
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = fp.ToString();//aq
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = gg.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = atemps.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = fp.ToString();//aq
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = gg.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = atemps.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                }
                                first++;
                            }
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = "    No.of papers : " + subcount;
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                            FpSpread1.Sheets[0].ColumnHeader.Columns[1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Columns[1].Width = 110;
                            sl_no++;
                        }
                    }
                    FpSpread1.Visible = true;
                    btnprintmaster.Visible = true;
                }
            }
            else
            {
                lblnorec.Visible = true;
                lblnorec.Text = "No Record(s) Found";
                FpSpread1.Visible = false;
            }

            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();
            
        }
        else
        {
            lblnorec.Visible = true;
            FpSpread1.Visible = false;
            lblnorec.Text = "From Sem Should Be Less To Sem";
        }
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
                FpSpread1.Visible = false;
                lblnorec.Visible = false;
                // FpSpread1.Sheets[0].SheetCorner.RowCount = 8;
                FpSpread1.Sheets[0].ColumnCount = 7;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FpSpread1.Sheets[0].AutoPostBack = true;
                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
                FpSpread1.CommandBar.Visible = true;
                bindbatch();
                binddegree();
                bindbranch();
                bindfromsem();
                bindtosem();

            }
        }
        catch (Exception ex)
        {
        }

    }
    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        bindfromsem();
        bindtosem();
    }
    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        bindfromsem();
        bindtosem();
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindfromsem();
        bindtosem();
    }
    protected void ddlfrmsem_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddltosem_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlfrmsem.SelectedItem.Text != "" && ddltosem.SelectedItem.Text != "")
        {
            int a = int.Parse(ddlfrmsem.SelectedValue.ToString());
            int b = int.Parse(ddltosem.SelectedValue.ToString());
            if (a <= b)
            {

            }
            else
            {
                lblnorec.Visible = true;
                lblnorec.Text = "From Sem Should Be Less To Sem";
            }
        }
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
    public void bindfromsem()
    {
        ddlfrmsem.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        con.Close();
        con.Open();
        SqlDataReader dr;
        string dd = "";
        bool isfalse = false;
        dd = "select max(ndurations) from ndegree where ";

        if (ddlbranch.SelectedItem.Text != "ALL")
        {
            dd = dd + " degree_code=" + ddlbranch.SelectedValue.ToString() + " and";
        }
        if (ddlbatch.SelectedItem.Text != "ALL")
        {
            dd = dd + "  batch_year=" + ddlbatch.Text.ToString() + " and";

        }
        dd = dd + " college_code=" + Session["collegecode"] + "";

        cmd = new SqlCommand(dd, con);
        dr = cmd.ExecuteReader();
        //+++++++++++++++
        string dqd = "";
        dqd = "select distinct first_year_nonsemester from ndegree where ";

        if (ddlbranch.SelectedItem.Text != "ALL")
        {
            dqd = dqd + " degree_code=" + ddlbranch.SelectedValue.ToString() + " and";
        }
        if (ddlbatch.SelectedItem.Text != "ALL")
        {
            dqd = dqd + "  batch_year=" + ddlbatch.Text.ToString() + " and";

        }
        dqd = dqd + " college_code=" + Session["collegecode"] + "";

        Boolean cc = new Boolean();
        bool.TryParse(GetFunction(dqd), out cc);
        //+++++++++++++++++++++++
        dr.Read();
        if (dr.HasRows == true)
        {
            //first_year = Convert.ToBoolean(dr[1].ToString());
            first_year = Convert.ToBoolean(cc);
            // duration = Convert.ToInt16(dr[0].ToString());
            int.TryParse(dr[0].ToString(), out duration);
            for (i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    ddlfrmsem.Items.Add(i.ToString());
                    isfalse = true;
                }
                else if (first_year == true && i != 2)
                {
                    ddlfrmsem.Items.Add(i.ToString());
                    isfalse = true;
                }

            }
        }
        if (!isfalse)
        {
            dr.Close();
            SqlDataReader dr1;
            string ddd = "";
            ddd = "select  max(duration) from degree where ";

            if (ddlbranch.SelectedItem.Text != "ALL")
            {
                ddd = ddd + " degree_code=" + ddlbranch.SelectedValue.ToString() + " and";

            }
            ddd = ddd + "  college_code=" + Session["collegecode"] + "";

            //++++++++++
            string ddd1 = "";
            ddd1 = "select  distinct first_year_nonsemester from degree where ";

            if (ddlbranch.SelectedItem.Text != "ALL")
            {
                ddd1 = ddd1 + " degree_code=" + ddlbranch.SelectedValue.ToString() + " and";

            }
            ddd1 = ddd1 + "  college_code=" + Session["collegecode"] + "";
            Boolean ww = new Boolean();
            bool.TryParse(GetFunction(ddd1), out ww);
            cmd = new SqlCommand(ddd, con);
            ddlfrmsem.Items.Clear();
            dr1 = cmd.ExecuteReader();
            dr1.Read();
            if (dr1.HasRows == true)
            {
                first_year = Convert.ToBoolean(ww);
                //duration = Convert.ToInt16(dr1[0].ToString());
                int.TryParse(dr1[0].ToString(), out duration);

                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlfrmsem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlfrmsem.Items.Add(i.ToString());
                    }
                }
            }

            dr1.Close();
        }
        con.Close();

    }
    public void bindtosem()
    {
        ddltosem.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        bool isfalse = false;
        con.Close();
        con.Open();
        SqlDataReader dr;
        string dd = "";
        dd = "select max(ndurations) from ndegree where ";

        if (ddlbranch.SelectedItem.Text != "ALL")
        {
            dd = dd + " degree_code=" + ddlbranch.SelectedValue.ToString() + " and";
        }
        if (ddlbatch.SelectedItem.Text != "ALL")
        {
            dd = dd + "  batch_year=" + ddlbatch.Text.ToString() + " and";

        }
        dd = dd + " college_code=" + Session["collegecode"] + "";

        cmd = new SqlCommand(dd, con);
        dr = cmd.ExecuteReader();
        //+++++++++++++++
        string dqd = "";
        dqd = "select distinct first_year_nonsemester from ndegree where ";

        if (ddlbranch.SelectedItem.Text != "ALL")
        {
            dqd = dqd + " degree_code=" + ddlbranch.SelectedValue.ToString() + " and";
        }
        if (ddlbatch.SelectedItem.Text != "ALL")
        {
            dqd = dqd + "  batch_year=" + ddlbatch.Text.ToString() + " and";

        }
        dqd = dqd + " college_code=" + Session["collegecode"] + "";

        Boolean cc = new Boolean();
        bool.TryParse(GetFunction(dqd), out cc);
        //+++++++++++++++++++++++
        dr.Read();
        if (dr.HasRows == true)
        {
            //first_year = Convert.ToBoolean(dr[1].ToString());
            first_year = Convert.ToBoolean(cc);
            int.TryParse(dr[0].ToString(), out duration);
            for (i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    ddltosem.Items.Add(i.ToString());
                    isfalse = true;
                }
                else if (first_year == true && i != 2)
                {
                    ddltosem.Items.Add(i.ToString());
                    isfalse = true;
                }

            }
        }
        if (!isfalse)
        {
            dr.Close();
            SqlDataReader dr1;
            string ddd = "";
            ddd = "select  max(duration) from degree where ";

            if (ddlbranch.SelectedItem.Text != "ALL")
            {
                ddd = ddd + " degree_code=" + ddlbranch.SelectedValue.ToString() + " and";

            }
            ddd = ddd + "  college_code=" + Session["collegecode"] + "";

            //++++++++++
            string ddd1 = "";
            ddd1 = "select  distinct first_year_nonsemester from degree where ";

            if (ddlbranch.SelectedItem.Text != "ALL")
            {
                ddd1 = ddd1 + " degree_code=" + ddlbranch.SelectedValue.ToString() + " and";

            }
            ddd1 = ddd1 + "  college_code=" + Session["collegecode"] + "";
            Boolean ww = new Boolean();
            bool.TryParse(GetFunction(ddd1), out ww);

            cmd = new SqlCommand(ddd, con);
             //ddltosem.Items.Clear();
            dr1 = cmd.ExecuteReader();
            dr1.Read();
            if (dr1.HasRows == true)
            {
                first_year = Convert.ToBoolean(ww);
                int.TryParse(dr1[0].ToString(), out duration);

                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddltosem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddltosem.Items.Add(i.ToString());
                    }
                }
            }

            dr1.Close();
        }
        con.Close();

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
    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value;
            }
        }
        return null;
    }
    public void logo_set()
    {
        FpSpread1.Visible = true;
        FpSpread1.Sheets[0].SheetName = " ";
        FpSpread1.Sheets[0].RowCount = 0;
       
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Sl. No";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Register No";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Subject";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Attempts";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Semester";

        FpSpread1.Sheets[0].ColumnHeader.Rows[0].HorizontalAlign = HorizontalAlign.Center;
      
        FarPoint.Web.Spread.TextCellType txt = new TextCellType();
        FpSpread1.Sheets[0].Columns[1].CellType = txt;
        FpSpread1.Sheets[0].Columns[2].CellType = txt;
        //=header
        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        style.Font.Size = 10;
        style.Font.Bold = true;
        FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
        
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
        Span1.InnerHtml = coll_name;
        spnCollegeHeader.InnerHtml = address;
        Span2.InnerHtml = affiliated;
        Span3.InnerHtml = "Students Wise Arrears List";
       // FpSpread1.Sheets[0].ColumnHeader.Rows[1].Visible = false;
        FpSpread1.Sheets[0].ColumnHeader.Rows[0].BackColor = Color.FromArgb(214, 235, 255);
        FpSpread1.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
        FpSpread1.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Medium;
        
    }

    //protected void btnprintmaster_Click(Object sender, EventArgs e)
    //{
    //    try
    //    {

    //        string degreedetails = "StudentWise Arrear List";
    //        string pagename = "Studentwise_Arrear_List.aspx";
    //        Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
    //        Printcontrol.Visible = true;
    //        //Fpsmarks.Visible = true;
    //        //Printcontrol.Visible = true;
    //        //Fpsmarks.Visible = true;
    //        //Printcontrol.loadspreaddetails(Fpsmarks, "IndReport.aspx", "Marks Report");
    //    }
    //    catch
    //    {
    //    }
    //}
}