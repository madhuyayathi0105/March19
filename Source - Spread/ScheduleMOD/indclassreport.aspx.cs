using System;//--------------------------1/3/12(spread width), 26/4/12(if no degree rits for staff disable ddl, if no sem info ,d/p msg,disable color lbl)
//==========================, 11/05/12( halforfull='0')
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;


public partial class NewAttendance : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;

    string strbatch = string.Empty;
    string strbatchyear = string.Empty;
    string strbranch = string.Empty;
    string strsem = string.Empty;
    string strbranchname = string.Empty;
    string strsec = string.Empty;
    string strsection = string.Empty;
    string strsection1 = string.Empty;
    string strsecti = "";
    string sqlcmdall1 = "";
    string strbat = "", strdegr = "", strseme = "";

    string strbatchsplit = string.Empty;
    string strbranchsplit = string.Empty;
    string strsecsplit = string.Empty;

    int count = 0;
    int count1 = 0;
    int count2 = 0;
    int count3 = 0;
    int count4 = 0;

    static int batchcnt = 0;
    static int degreecnt = 0;
    static int branchcnt = 0;
    static int sectioncnt = 0;


    DataSet ds2 = new DataSet1();


    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection tempcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mysql2 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());

    SqlCommand cmda;
    SqlCommand cmd;

    DataSet ds1 = new DataSet();
    string Day_Order = "";//Added by bManikandan 25/07/20113
    DAccess2 d2 = new DAccess2();//Added by Manikandan 25/07/2013
    //public DataSet Bind_Degree(string college_code, string user_code)
    //{
    //    SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    //    dcon.Close();
    //    dcon.Open();
    //    SqlCommand cmd = new SqlCommand("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "", dcon);
    //    SqlDataAdapter da = new SqlDataAdapter(cmd);
    //    DataSet ds = new DataSet();
    //    da.Fill(ds);
    //    return ds;
    //}
    //public DataSet Bind_Dept(string degree_code, string college_code, string user_code)
    //{
    //    SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    //    dcon.Close();
    //    dcon.Open();
    //    SqlCommand cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + degree_code + " and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "", dcon);
    //    SqlDataAdapter da = new SqlDataAdapter(cmd);
    //    DataSet ds = new DataSet();
    //    da.Fill(ds);
    //    return ds;
    //}
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
            collegecode = Session["collegecode"].ToString();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();

            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("ScheduleHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/ScheduleMOD/ScheduleHome.aspx");
                    return;
                }
            }
            FarPoint.Web.Spread.NamedStyle fontblue = new FarPoint.Web.Spread.NamedStyle("blue");

            classreport.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            classreport.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            classreport.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
            classreport.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
            classreport.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            classreport.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;


            FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
            style.Font.Size = 13;
            style.Font.Bold = true;
            classreport.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
            classreport.Sheets[0].AllowTableCorner = true;
            classreport.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            classreport.Sheets[0].SheetCorner.Cells[0, 0].Text = "Period";
            classreport.Sheets[0].RowHeader.Columns[0].Width = 100;


            if (!Page.IsPostBack)
            {
                setLabelText();
                Printcontrol.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                Printcontrol.Visible = false;
                txtFromDate.Attributes.Add("ReadOnly", "ReadOnly");//Added by Manikandan
                txtToDate.Attributes.Add("ReadOnly", "ReadOnly");//Added by Manikandan
                //if (Convert.ToString(Session["value"]) == "1")//==========back button visible
                //{
                //    LinkButton3.Visible = false;
                //    LinkButton2.Visible = true;
                //}
                //else
                //{
                //    LinkButton3.Visible = true;
                //    LinkButton2.Visible = false;
                //}

                classreport.Visible = false;
                BindBatch();
                BindDegree(singleuser, group_user, collegecode, usercode);
                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                BindSectionDetailmult(collegecode);
                loadsubject();

                fmlbl.Visible = false;
                tolbl.Visible = false;
                diffdate.Visible = false;
                errlbl.Visible = false;
                colorpnl.Visible = false;

                classreport.ActiveSheetView.AutoPostBack = true;
                classreport.CommandBar.Visible = false;
                //   classreport.SheetCorner.Columns[0].Width = 150;

                string dt = DateTime.Today.ToShortDateString();
                string[] dsplit = dt.Split(new Char[] { '/' });
                Session["curr_year"] = dsplit[2].ToString();

                txtToDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
                txtFromDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
                DateTime stDate;
                stDate = System.DateTime.Today.AddDays(-7);
                // ddlbatch.Text = dsplit[2].ToString(); 
            }
            if (lblbranch.Text.Trim().ToLower() == "standard")
            {
                lblbatch.Text = "Year";
            }
        }
        catch (Exception ex) { }
    }
    //public void enable_ddl()
    //{

    //    errlbl.Visible = true;
    //    errlbl.Text = "Update Degree Rights";
    //    ddlbatch.Enabled = false;
    //    ddldegree.Enabled = false;
    //    ddlbranch.Enabled = false;
    //    ddlsec.Enabled = false;
    //    ddlduration.Enabled = false;
    //    btnGo.Enabled = false;
    //    txtFromDate.Enabled = false;
    //    txtToDate.Enabled = false;
    //    fmlbl.Visible = false;
    //    tolbl.Visible = false;
    //    diffdate.Visible = false;
    //    classreport.Visible = false;
    //    colorpnl.Visible = false;
    //}



    //public void bindbatch()
    //{
    //    ////batch
    //    ddlbatch.Items.Clear();
    //    con.Close();
    //    con.Open();
    //    ds1.Clear();
    //    cmd = new SqlCommand(" select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year", con);
    //    SqlDataAdapter da1 = new SqlDataAdapter(cmd);

    //    da1.Fill(ds1);

    //    if (ds1.Tables[0].Rows.Count > 0)
    //    {
    //        ddlbatch.DataSource = ds1;
    //        ddlbatch.DataValueField = "batch_year";
    //        ddlbatch.DataBind();
    //        string sqlstr = "";
    //        int max_bat = 0;
    //        sqlstr = "select max(batch_year) from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' ";
    //        max_bat = Convert.ToInt32(GetFunction(sqlstr));
    //        ddlbatch.SelectedValue = max_bat.ToString();
    //        con.Close();
    //    }

    //}


    //public void binddegree()
    //{
    //    ////degree
    //    ddldegree.Items.Clear();
    //    con.Close();
    //    con.Open();
    //    //cmd = new SqlCommand("select distinct degree.course_id,course.course_name from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + Session["collegecode"] + " order by course.course_name ", con);
    //    //SqlDataAdapter da = new SqlDataAdapter(cmd);
    //    //DataSet ds = new DataSet();
    //    //da.Fill(ds);
    //    string collegecode = Session["collegecode"].ToString();
    //    string usercode = Session["usercode"].ToString();
    //    DataSet ds = Bind_Degree(collegecode.ToString(), usercode);

    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        ddldegree.DataSource = ds;
    //        ddldegree.DataValueField = "course_id";
    //        ddldegree.DataTextField = "course_name";
    //        ddldegree.DataBind();
    //    }
    //    //else
    //    //{
    //    //    errlbl.Visible = true;
    //    //    errlbl.Text = "Give Degree Rights For This Staff";
    //    //}
    //}

    //public void bindsem()
    //{

    //    if (ddlbatch.Items.Count > 0 && ddlbranch.Items.Count > 0)
    //    {
    //        ddlduration.Items.Clear();
    //        Boolean first_year;
    //        first_year = false;
    //        int duration = 0;
    //        int i = 0;
    //        con.Close();
    //        con.Open();
    //        SqlDataReader dr;
    //        cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
    //        dr = cmd.ExecuteReader();
    //        dr.Read();
    //        if (dr.HasRows == true)
    //        {
    //            first_year = Convert.ToBoolean(dr[1].ToString());
    //            duration = Convert.ToInt16(dr[0].ToString());
    //            for (i = 1; i <= duration; i++)
    //            {
    //                if (first_year == false)
    //                {
    //                    ddlduration.Items.Add(i.ToString());
    //                }
    //                else if (first_year == true && i != 2)
    //                {
    //                    ddlduration.Items.Add(i.ToString());
    //                }

    //            }
    //        }
    //        else
    //        {
    //            dr.Close();
    //            SqlDataReader dr1;
    //            cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and college_code=" + Session["collegecode"] + "", con);
    //            ddlduration.Items.Clear();
    //            dr1 = cmd.ExecuteReader();
    //            dr1.Read();
    //            if (dr1.HasRows == true)
    //            {
    //                first_year = Convert.ToBoolean(dr1[1].ToString());
    //                duration = Convert.ToInt16(dr1[0].ToString());

    //                for (i = 1; i <= duration; i++)
    //                {
    //                    if (first_year == false)
    //                    {
    //                        ddlduration.Items.Add(i.ToString());
    //                    }
    //                    else if (first_year == true && i != 2)
    //                    {
    //                        ddlduration.Items.Add(i.ToString());
    //                    }
    //                }
    //            }

    //            dr1.Close();
    //        }
    //        con.Close();
    //    }
    //    //else
    //    //{
    //    //    errlbl.Visible = true;
    //    //    errlbl.Text = "Give Degree Rights For This Staff";
    //    //}
    //}
    //protected void ddlduration_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    classreport.Visible = false;
    //    colorpnl.Visible = false;
    //    fmlbl.Visible = false;
    //    tolbl.Visible = false;
    //    errlbl.Visible = false;
    //    bindsec();
    //}
    //public void bindsec()
    //{
    //    if (ddlbatch.Items.Count > 0 && ddlbranch.Items.Count > 0)
    //    {
    //        ddlsec.Items.Clear();
    //        con.Close();
    //        con.Open();
    //        cmd = new SqlCommand("select distinct sections from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and delflag=0 and exam_flag<>'Debar' and sections is not null and ltrim(sections)!=''", con);
    //        SqlDataAdapter da = new SqlDataAdapter(cmd);
    //        DataSet ds = new DataSet();
    //        da.Fill(ds);
    //        ddlsec.DataSource = ds;
    //        ddlsec.DataTextField = "sections";
    //        ddlsec.DataBind();
    //        SqlDataReader dr_sec;
    //        dr_sec = cmd.ExecuteReader();
    //        dr_sec.Read();
    //        if (dr_sec.HasRows == true)
    //        {
    //            if (dr_sec["sections"].ToString() == string.Empty)
    //            {
    //                ddlsec.Enabled = false;
    //            }
    //            else
    //            {
    //                ddlsec.Enabled = true;
    //            }
    //        }
    //        else
    //        {
    //            ddlsec.Enabled = false;
    //        }
    //        con.Close();
    //    }
    //    //else
    //    //{
    //    //    errlbl.Visible = true;
    //    //    errlbl.Text = "Give Degree Rights For This Staff";
    //    //}
    //}
    //protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //    bindbranch();
    //    bindsem();
    //    bindsec();
    //    classreport.Visible = false;
    //    fmlbl.Visible = false;
    //    tolbl.Visible = false;
    //    errlbl.Visible = false;
    //    colorpnl.Visible = false;
    //}

    //public void bindbranch()
    //{
    //    ddlbranch.Items.Clear();
    //    con.Close();
    //    con.Open();
    //    ////cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + ddldegree.SelectedValue.ToString() + " and degree.college_code= " + Session["collegecode"] + "", con);
    //    ////SqlDataAdapter da = new SqlDataAdapter(cmd);
    //    ////DataSet ds = new DataSet();
    //    ////da.Fill(ds);

    //    if (ddldegree.Items.Count > 0)
    //    {
    //        string collegecode = Session["collegecode"].ToString();
    //        string usercode = Session["usercode"].ToString();
    //        string course_id = ddldegree.SelectedValue.ToString();
    //        DataSet ds = Bind_Dept(course_id, collegecode, usercode);

    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            ddlbranch.DataSource = ds;
    //            ddlbranch.DataTextField = "dept_name";
    //            ddlbranch.DataValueField = "degree_code";
    //            ddlbranch.DataBind();
    //            con.Close();
    //        }
    //    }
    //    //else
    //    //{
    //    //    errlbl.Visible = true;
    //    //    errlbl.Text = "Give Degree Rights For This Staff";
    //    //}
    //}
    //protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //    classreport.Visible = false;
    //    colorpnl.Visible = false;
    //    fmlbl.Visible = false;
    //    tolbl.Visible = false;
    //    errlbl.Visible = false;
    //    bindsem();
    //    bindsec();
    //}


    //----------------------------GO button

    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();
        //lbl.Add(Label4);
        //fields.Add(0);
        //lbl.Add(lbl_Stream);
        //fields.Add(1);
        lbl.Add(lbldegree);
        fields.Add(2);
        lbl.Add(lblbranch);
        fields.Add(3);
        //lbl.Add(lblDuration);
        //fields.Add(4);
        //Name -0, Stream - 1 ,Degree - 2, Branch - 3, Term - 4
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        //if (ddlbatch.Text == "-1")
        //{
        //    batlbl.Visible = true;
        //}

        if (txtFromDate.Text == "")
        {
            fmlbl.Visible = true;
        }
        if (txtToDate.Text == "")
        {
            tolbl.Visible = true;
        }
        //if (ddlsec.Enabled == true && ddlsec.Text != "-1" && txtFromDate.Text != "" && txtToDate.Text != "")
        //{
        //    gobutton();
        //}
        //if (ddlsec.Enabled == false && txtFromDate.Text != "" && txtToDate.Text != "")
        //{
        gobutton();
        //}
        // classreport.Width=((classreport.Sheets[0].ColumnCount)*200)+100;
    }

    public void gobutton()
    {
        classreport.Sheets[0].RowHeader.Visible = false;
        int slno = 0;
        string strsec = "";
        int SchOrder = 0, nodays = 0;
        string start_dayorder = string.Empty;//Added by Manikandan
        int intNCtr;
        string srt_day = "";
        string splvalnew = "";
        int intNHrs = 0;
        string splval = "";
        string startdate = "";

        int inhighesthr = 0;

        string section = "";
        string degree_code = "";
        string semester = "";
        string batch_year = "";
        classreport.CurrentPage = 0;
        string date1, date2;
        string datefrom, dateto, datefrom_val;
        string todate = "";
        Boolean noflag = false;
        string sec_txt = "";
        //sec_txt = ddlsec.Text;
        int day_val = 0;
        int split_plus = 0;
        Boolean norecord = false;

        classreport.Sheets[0].ColumnCount = 0;
        classreport.Sheets[0].RowCount = 0;
        classreport.Sheets[0].ColumnHeader.RowCount = 2;


        date1 = txtFromDate.Text.ToString();
        string[] split = date1.Split(new Char[] { '/' });
        string maxbatchyear = "";
        string maxbranchyear = "";
        if (txtbatch.Text != "---Select---" || chklstbatch.Items.Count != null)
        {
            int itemcount = 0;

            errlbl.Text = "";
            errlbl.Visible = false;
            for (itemcount = 0; itemcount < chklstbatch.Items.Count; itemcount++)
            {
                if (chklstbatch.Items[itemcount].Selected == true)
                {
                    if (maxbatchyear == "")
                        maxbatchyear = chklstbatch.Items[itemcount].Value.ToString();
                    else
                        maxbatchyear = maxbatchyear + "," + chklstbatch.Items[itemcount].Value.ToString();
                }
            }


        }
        else
        {
            errlbl.Visible = true;
            errlbl.Text = "Plaese Choose Batch";
            return;
        }

        if (txtbranch.Text != "---Select---" || chklstbranch.Items.Count != null)
        {

            errlbl.Text = "";
            errlbl.Visible = false;
            int itemcount1 = 0;

            for (itemcount1 = 0; itemcount1 < chklstbranch.Items.Count; itemcount1++)
            {
                if (chklstbranch.Items[itemcount1].Selected == true)
                {
                    if (maxbranchyear == "")
                        maxbranchyear = chklstbranch.Items[itemcount1].Value.ToString();
                    else
                        maxbranchyear = maxbranchyear + "," + chklstbranch.Items[itemcount1].Value.ToString();
                }
            }

        }
        else
        {
            errlbl.Visible = true;
            errlbl.Text = "Please Choose Degree";
            return;
        }


        Hashtable hatsubject = new Hashtable();
        string subjectcode = "";
        for (int subcount = 0; subcount < chklssubject.Items.Count; subcount++)
        {
            if (chklssubject.Items[subcount].Selected == true)
            {
                if (subjectcode == "")
                {
                    subjectcode = "'" + chklssubject.Items[subcount].Value.ToString() + "'";
                }
                else
                {
                    subjectcode = "" + subjectcode + ",'" + chklssubject.Items[subcount].Value.ToString() + "'";
                }

            }
        }
        if (subjectcode.Trim() != "" && subjectcode != null)
        {
            string getsubnoquery = "Select Subject_no from subject where subject_code in(" + subjectcode + ")";
            DataSet dssubcout = d2.select_method_wo_parameter(getsubnoquery, "Text");
            if (dssubcout.Tables[0].Rows.Count > 0)
            {
                for (int sc = 0; sc < dssubcout.Tables[0].Rows.Count; sc++)
                {
                    subjectcode = dssubcout.Tables[0].Rows[sc]["Subject_no"].ToString();
                    if (!hatsubject.Contains(subjectcode))
                    {
                        hatsubject.Add(subjectcode, subjectcode);
                    }
                }
            }
        }
        if (split.GetUpperBound(0) == 2)
        {
            if (Convert.ToInt16(split[0].ToString()) <= 31 && Convert.ToInt16(split[1].ToString()) <= 12 && Convert.ToInt16(split[0].ToString()) <= Convert.ToInt16(Session["curr_year"]))
            {
                datefrom_val = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                date2 = txtToDate.Text.ToString();
                string[] split1 = date2.Split(new Char[] { '/' });
                if (split1.GetUpperBound(0) == 2)
                {
                    if (Convert.ToInt16(split1[0].ToString()) <= 31 && Convert.ToInt16(split1[1].ToString()) <= 12 && Convert.ToInt16(split1[0].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                    {
                        dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                        DateTime dt1 = Convert.ToDateTime(datefrom_val.ToString());
                        DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                        TimeSpan t = dt2.Subtract(dt1);
                        int days = t.Days;
                        if (days >= 0)
                        {
                            for (day_val = 0; day_val <= days; day_val++)
                            {
                                int startspanrow = 0;
                                int startspancol = 0;
                                int numberofrows = 0;
                                int numberofcols = 0;

                                DateTime from_dt = dt1.AddDays(day_val);
                                string from_dr_str = "";
                                from_dr_str = from_dt.ToString();
                                string[] split_dr = from_dr_str.Split(' ');
                                string[] split_1 = split_dr[0].Split(new Char[] { '/' });
                                split_plus = Convert.ToInt32(split_1[1]) + Convert.ToInt16(day_val.ToString());
                                datefrom = split_1[0].ToString() + "/" + split_1[1].ToString() + "/" + split_1[2].ToString();
                                //classreport.Sheets[0].ColumnHeader.Cells[0, day_val].Text = split_1[1].ToString() + "/" + split_1[0].ToString() + "/" + split_1[2].ToString();
                                //classreport.Sheets[0].ColumnHeader.Cells[0, day_val].Note = split_1[1].ToString() + "/" + split_1[0].ToString() + "/" + split_1[2].ToString();

                                con.Close();
                                con.Open();
                                SqlDataReader sqld;

                                string sems = "";
                                string semquery = "select distinct current_semester from registration where degree_code in(" + maxbranchyear + ") and batch_year in(" + maxbatchyear + ") and cc=0 and exam_flag <> 'DEBAR' and delflag=0 order by current_semester desc";
                                SqlCommand cmd = new SqlCommand(semquery, con);
                                sqld = cmd.ExecuteReader();
                                while (sqld.Read())
                                {
                                    if (sems == "")
                                    {
                                        sems = sqld["current_semester"].ToString();
                                    }
                                    else
                                    {
                                        sems = sems + ',' + sqld["current_semester"].ToString();
                                    }
                                }

                                con.Close();
                                con.Open();


                                string hrquery = "select distinct max(No_of_hrs_per_day) from periodattndschedule where degree_code in(" + maxbranchyear + ") and semester in (" + sems + ") ";
                                string highesthours = GetFunction(hrquery);
                                int inthighesthour = Convert.ToInt16(highesthours);
                                inhighesthr = inthighesthour;

                                if (day_val == 0)
                                {
                                    classreport.Sheets[0].ColumnCount++;
                                    classreport.Sheets[0].ColumnHeader.Cells[0, classreport.Sheets[0].ColumnCount - 1].Text = "S.No";
                                    classreport.Sheets[0].ColumnCount++;
                                    if (lblbranch.Text.Trim().ToLower() == "standard")
                                    {
                                        classreport.Sheets[0].ColumnHeader.Cells[0, classreport.Sheets[0].ColumnCount - 1].Text = "Standard";
                                    }
                                    else
                                    {
                                        classreport.Sheets[0].ColumnHeader.Cells[0, classreport.Sheets[0].ColumnCount - 1].Text = "Degree";
                                    }
                                }
                                for (int hr = 1; hr <= inthighesthour; hr++)
                                {
                                    classreport.Sheets[0].ColumnCount++;
                                    if (hr == 1)
                                    {
                                        startspancol = classreport.Sheets[0].ColumnCount - 1;
                                    }
                                    classreport.Sheets[0].ColumnHeader.Cells[0, classreport.Sheets[0].ColumnCount - 1].Text = split_1[1].ToString() + "/" + split_1[0].ToString() + "/" + split_1[2].ToString();
                                    classreport.Sheets[0].ColumnHeader.Cells[0, classreport.Sheets[0].ColumnCount - 1].Note = split_1[1].ToString() + "/" + split_1[0].ToString() + "/" + split_1[2].ToString();
                                    classreport.Sheets[0].ColumnHeader.Cells[1, classreport.Sheets[0].ColumnCount - 1].Text = hr.ToString();
                                    classreport.Sheets[0].ColumnHeader.Cells[1, classreport.Sheets[0].ColumnCount - 1].Note = hr.ToString();
                                    numberofcols++;

                                }
                                // fsstaff.Sheets[0].SpanModel.Add(fsstaff.Sheets[0].RowCount - 1, 0, 1, 3);
                                classreport.Sheets[0].ColumnHeaderSpanModel.Add(0, startspancol, 1, numberofcols);
                                classreport.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                                classreport.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);



                            }


                            Boolean semflag = false;

                            for (int batch = 0; batch < chklstbatch.Items.Count; batch++)
                            {

                                if (chklstbatch.Items[batch].Selected == true)
                                {

                                    batch_year = chklstbatch.Items[batch].Value;
                                    for (int branch = 0; branch < chklstbranch.Items.Count; branch++)
                                    {

                                        if (chklstbranch.Items[branch].Selected == true)
                                        {
                                            ArrayList alsec = new ArrayList();
                                            alsec.Clear();
                                            degree_code = chklstbranch.Items[branch].Value;

                                            con.Close();
                                            con.Open();
                                            DataTable dtvalue = new DataTable();
                                            string sectionquery = "select distinct sections from registration where batch_year='" + batch_year + "' and degree_code='" + degree_code + "' and cc=0 and exam_flag <> 'DEBAR' and delflag=0 order by sections ";
                                            SqlDataAdapter sqldap = new SqlDataAdapter(sectionquery, con);
                                            sqldap.Fill(dtvalue);
                                            string emptyvalue = "";
                                            if (dtvalue.Rows.Count > 0)
                                            {
                                                for (int sec = 0; sec < dtvalue.Rows.Count; sec++)
                                                {
                                                    emptyvalue = Convert.ToString(dtvalue.Rows[sec]["sections"]);
                                                    if (emptyvalue == "NULL" || emptyvalue == "null" || emptyvalue == "")
                                                    {
                                                        alsec.Add("Empty");
                                                    }
                                                    alsec.Add(Convert.ToString(dtvalue.Rows[sec]["sections"]));
                                                }

                                            }

                                            for (int bindsec = 0; bindsec < chklstsection.Items.Count; bindsec++)
                                            {

                                                if (chklstsection.Items[bindsec].Selected == true)
                                                {

                                                    string secval = chklstsection.Items[bindsec].Value;
                                                    if (alsec.Contains(secval))
                                                    {
                                                        slno++;
                                                        if (secval == "")
                                                        {
                                                            strsec = "";
                                                        }
                                                        else
                                                        {
                                                            section = secval;
                                                            if (section == "Empty")
                                                            {
                                                                section = "";
                                                                strsec = "";
                                                            }
                                                            else
                                                            {
                                                                strsec = " and sections='" + secval + "'";
                                                            }
                                                        }

                                                        semester = GetFunction("select distinct current_semester from registration where batch_year='" + batch_year + "' and  degree_code='" + degree_code + "' and cc=0 and exam_flag <> 'DEBAR' and delflag=0");
                                                        DateTime CtDate;
                                                        CtDate = DateTime.Now;
                                                        con.Close();
                                                        con.Open();
                                                        SqlDataReader dr;
                                                        cmd = new SqlCommand("Select No_of_hrs_per_day,schorder,nodays from periodattndschedule where degree_code=" + degree_code + " and semester = " + semester + "", con);
                                                        dr = cmd.ExecuteReader();
                                                        dr.Read();
                                                        if (dr.HasRows == true)
                                                        {
                                                            if ((dr["No_of_hrs_per_day"].ToString()) != "")
                                                            {
                                                                intNHrs = Convert.ToInt16(dr["No_of_hrs_per_day"]);
                                                                SchOrder = Convert.ToInt16(dr["schorder"]);
                                                                nodays = Convert.ToInt16(dr["nodays"]);
                                                            }
                                                        }
                                                        dr.Close();
                                                        con.Close();
                                                        con.Open();
                                                        SqlDataReader dr1;
                                                        cmd = new SqlCommand("select * from seminfo where degree_code=" + degree_code + " and semester=" + semester + " and batch_year=" + batch_year + " ", con);
                                                        dr1 = cmd.ExecuteReader();
                                                        dr1.Read();
                                                        if (dr1.HasRows == true)
                                                        {
                                                            semflag = false;
                                                            if ((dr1["start_date"].ToString()) != "" && (dr1["start_date"].ToString()) != "\0")
                                                            {
                                                                string[] tmpdate = dr1["start_date"].ToString().Split(new char[] { ' ' });
                                                                startdate = tmpdate[0].ToString();
                                                                start_dayorder = dr1["starting_dayorder"].ToString();
                                                            }
                                                            else
                                                            {
                                                                errlbl.Visible = true;
                                                                errlbl.Text = "Update semester Information";
                                                                //Response.Write("<script>alert('Update semester Information')</script>");
                                                                //return;
                                                            }


                                                            int rccount = 0;
                                                            classreport.Sheets[0].RowCount = classreport.Sheets[0].RowCount + 1;

                                                            classreport.Sheets[0].Cells[classreport.Sheets[0].RowCount - 1, 0].Text = slno.ToString();
                                                            classreport.Sheets[0].Cells[classreport.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                            classreport.Sheets[0].Cells[classreport.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                            classreport.Sheets[0].Cells[classreport.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                            classreport.Sheets[0].Cells[classreport.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;


                                                            con.Close();
                                                            con.Open();
                                                            DataTable dtdegree = new DataTable();
                                                            string degreequery = "select course.course_name as course_name,department.dept_name as dept_name from degree,department,course where degree.course_id=course.course_id and degree.dept_code=department.dept_code and degree.degree_code='" + degree_code + "'";
                                                            SqlDataAdapter cm = new SqlDataAdapter(degreequery, con);
                                                            cm.Fill(dtdegree);

                                                            string degr = "";
                                                            if (dtdegree.Rows.Count > 0)
                                                            {

                                                                degr = dtdegree.Rows[0]["course_name"].ToString() + '-' + dtdegree.Rows[0]["dept_name"].ToString();
                                                            }
                                                            string setdegree = "";
                                                            if (section.Trim() != "" && section != null)
                                                            {
                                                                if (lblbranch.Text.Trim().ToLower() == "standard")
                                                                {
                                                                    setdegree = batch_year + " " + degr + "  section :" + section + " Term :" + semester;
                                                                }
                                                                else
                                                                {
                                                                    setdegree = batch_year + " " + degr + "  section :" + section + " semester :" + semester;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (lblbranch.Text.Trim().ToLower() == "standard")
                                                                {
                                                                    setdegree = batch_year + " " + degr + " Term :" + semester;
                                                                }
                                                                else
                                                                {
                                                                    setdegree = batch_year + " " + degr + " semester :" + semester;
                                                                }
                                                            }
                                                            classreport.Sheets[0].Cells[classreport.Sheets[0].RowCount - 1, 1].Text = setdegree;
                                                            classreport.Sheets[0].Cells[classreport.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                                            classreport.Sheets[0].Cells[classreport.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                            classreport.Sheets[0].Cells[classreport.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                            classreport.Sheets[0].Cells[classreport.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;


                                                            rccount = classreport.Sheets[0].RowCount - 1;
                                                            int colcount = 2;
                                                            int colendcount = inhighesthr;
                                                            int tempcolcount = 2;

                                                            for (day_val = 0; day_val <= days; day_val++)
                                                            {


                                                                DateTime from_dt = dt1.AddDays(day_val);
                                                                string from_dr_str = "";
                                                                from_dr_str = from_dt.ToString();
                                                                string[] split_dr = from_dr_str.Split(' ');
                                                                string[] split_1 = split_dr[0].Split(new Char[] { '/' });
                                                                split_plus = Convert.ToInt32(split_1[1]) + Convert.ToInt16(day_val.ToString());
                                                                datefrom = split_1[0].ToString() + "/" + split_1[1].ToString() + "/" + split_1[2].ToString();
                                                                // classreport.Sheets[0].ColumnCount = classreport.Sheets[0].ColumnCount + 1;
                                                                classreport.Sheets[0].Columns[day_val].Width = 1000;
                                                                //classreport.Sheets[0].Columns[day_val].Font.Name = "Book Antiqua";
                                                                //classreport.Sheets[0].Columns[day_val].Font.Size = FontUnit.Medium;
                                                                //classreport.Sheets[0].Columns[day_val].BackColor = Color.LightSeaGreen;
                                                                //classreport.Sheets[0].Columns[day_val].ForeColor = Color.Maroon;
                                                                int chsundaycol = 0;
                                                                for (intNCtr = 1; intNCtr <= intNHrs; intNCtr++)
                                                                {
                                                                    if (day_val != 0 && intNCtr == 1)
                                                                    {
                                                                        colcount = 0;
                                                                        colcount = colcount + tempcolcount;
                                                                    }
                                                                    colcount++;
                                                                    // classreport.Sheets[0].RowHeader.Cells[intNCtr - 1, 0].Text = (intNCtr.ToString());

                                                                    classreport.Sheets[0].Columns[day_val].Locked = true;
                                                                    // date_increment = Convert.ToInt16(split[0].ToString()) + day_val;

                                                                    // DateTime date_day = Convert.ToDateTime(split[1].ToString() + "/" + date_increment.ToString() + "/" + split[2].ToString());


                                                                    if (intNHrs > 0)
                                                                    {
                                                                        if (SchOrder != 0)
                                                                        {
                                                                            srt_day = from_dt.ToString("ddd");
                                                                        }
                                                                        else
                                                                        {
                                                                            todate = classreport.Sheets[0].ColumnHeader.Cells[0, colcount - 1].Text;
                                                                            //srt_day=findday(no,  sdate,  todate);//hided by Manikandan
                                                                            srt_day = findday(todate, degree_code, semester, batch_year, startdate.ToString(), Convert.ToString(nodays), start_dayorder);//Added by Manikandan 25/07/2013
                                                                            string setdayorder = "";
                                                                            if (srt_day == "mon")
                                                                            {
                                                                                setdayorder = " Day Order : 1";

                                                                            }
                                                                            else if (srt_day == "tue")
                                                                            {
                                                                                setdayorder = " Day Order : 2";
                                                                            }
                                                                            else if (srt_day == "wed")
                                                                            {
                                                                                setdayorder = " Day Order : 3";
                                                                            }
                                                                            else if (srt_day == "thu")
                                                                            {
                                                                                setdayorder = " Day Order : 4";
                                                                            }
                                                                            else if (srt_day == "fri")
                                                                            {
                                                                                setdayorder = " Day Order : 5";
                                                                            }
                                                                            else if (srt_day == "sat")
                                                                            {
                                                                                setdayorder = " Day Order : 6";
                                                                            }
                                                                            classreport.Sheets[0].Cells[classreport.Sheets[0].RowCount - 1, 1].Text = setdegree + setdayorder;
                                                                        }
                                                                    }
                                                                    Session["sun_day"] = srt_day.ToString();
                                                                    if (srt_day != "Sun")
                                                                    {
                                                                        chsundaycol = 0;
                                                                        String sqlsrt = "select top 1 ";

                                                                        sqlsrt = sqlsrt + srt_day + intNCtr.ToString();



                                                                        //-------------alternate schedule

                                                                        tempcon.Close();
                                                                        tempcon.Open();
                                                                        SqlDataReader dr_sch;
                                                                        SqlCommand cmd_sch;
                                                                        // cmd_sch = new SqlCommand(sqlsrt + " degree_code , semester , batch_year from Alternate_schedule where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and FromDate= '" + datefrom.ToString() + "' " + strsec + " ", tempcon);
                                                                        cmd_sch = new SqlCommand(sqlsrt + " from Alternate_schedule where batch_year=" + batch_year + " and degree_code = " + degree_code + " and semester = " + semester + " and FromDate= '" + datefrom.ToString() + "' " + strsec + " ", tempcon);
                                                                        dr_sch = cmd_sch.ExecuteReader();
                                                                        dr_sch.Read();

                                                                        if (dr_sch.HasRows == true && dr_sch[0].ToString() != "" && dr_sch[0].ToString() != "\0")
                                                                        {

                                                                            noflag = true;
                                                                            norecord = true;
                                                                            string[] sple = ((dr_sch[0]).ToString()).Split(new Char[] { ';' });
                                                                            for (int i = 0; i <= sple.GetUpperBound(0); i++)
                                                                            {
                                                                                if (sple.GetUpperBound(0) >= 0)
                                                                                {
                                                                                    string[] sp1 = (sple[i].ToString()).Split(new Char[] { '-' });
                                                                                    if (sp1.GetUpperBound(0) >= 2)
                                                                                    {
                                                                                        if (hatsubject.Contains(sp1[0].ToString()))
                                                                                        {
                                                                                            splval = splval + (GetFunction("select subject_name from subject where subject_no=" + sp1[0].ToString() + " ") + "-" + sp1[1].ToString() + "-" + sp1[2].ToString()) + ";";
                                                                                        }
                                                                                        tempcon.Close();

                                                                                    }
                                                                                }
                                                                            }
                                                                            cmd.CommandText = "select top 1 holiday_desc from holidaystudents where holiday_date='" + datefrom.ToString() + "' and degree_code=" + degree_code + " and semester=" + semester + "";
                                                                            cmd.Connection = mysql2;
                                                                            mysql2.Close();
                                                                            mysql2.Open();
                                                                            SqlDataReader dr_holday = cmd.ExecuteReader();
                                                                            dr_holday.Read();
                                                                            //===================

                                                                            if (dr_holday.HasRows == true)
                                                                            {
                                                                                if (dr_holday["holiday_desc"].ToString() == "Sunday")//this line added by manikandan 24/08/2013
                                                                                {
                                                                                    classreport.Sheets[0].Cells[rccount, colcount - 1].Text = "Sunday Holiday";
                                                                                    classreport.Sheets[0].Cells[rccount, colcount - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                                    classreport.Sheets[0].Cells[rccount, colcount - 1].Font.Name = "Book Antiqua";
                                                                                    classreport.Sheets[0].Cells[rccount, colcount - 1].Font.Size = FontUnit.Medium;
                                                                                    classreport.Sheets[0].Cells[rccount, colcount - 1].Font.Bold = true;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                classreport.Sheets[0].Cells[rccount, colcount - 1].Text = Convert.ToString(splval);
                                                                                classreport.Sheets[0].Cells[rccount, colcount - 1].HorizontalAlign = HorizontalAlign.Left;
                                                                                classreport.Sheets[0].Cells[rccount, colcount - 1].Font.Name = "Book Antiqua";
                                                                                classreport.Sheets[0].Cells[rccount, colcount - 1].Font.Size = FontUnit.Medium;
                                                                                classreport.Sheets[0].Cells[rccount, colcount - 1].BackColor = Color.LightPink;
                                                                                classreport.Sheets[0].Cells[rccount, colcount - 1].ForeColor = Color.Green;
                                                                                classreport.Sheets[0].Cells[rccount, colcount - 1].Font.Bold = true;
                                                                                // classreport.Sheets[0].Cells[rowval, 3].Note = Convert.ToString(setcellnote);
                                                                                splval = "";
                                                                            }
                                                                            // }
                                                                            // }
                                                                        }
                                                                        //---------------------------------class schedule
                                                                        else
                                                                        {
                                                                            dr1.Close();
                                                                            con.Close();
                                                                            con.Open();
                                                                            SqlDataReader dr3;
                                                                            //  cmd = new SqlCommand(sqlsrt + "degree_code,semester,batch_year from semester_schedule where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and FromDate<= ' " + datefrom.ToString() + " ' " + strsec + " ", con);
                                                                            cmd = new SqlCommand(sqlsrt + " from semester_schedule where batch_year=" + batch_year + " and degree_code = " + degree_code + " and semester = " + semester + " and FromDate<= ' " + datefrom.ToString() + " ' " + strsec + " ", con);
                                                                            dr3 = cmd.ExecuteReader();
                                                                            dr3.Read();
                                                                            if (dr3.HasRows == true)
                                                                            {
                                                                                string x = "";
                                                                                x = dr3[0].ToString();
                                                                                if (dr3[0].ToString() != null && dr3[0].ToString() != "")
                                                                                {
                                                                                    noflag = true;
                                                                                    norecord = true;
                                                                                    //for (intNCtr = 1; intNCtr <= intNHrs; intNCtr++)
                                                                                    //{
                                                                                    classreport.Sheets[0].Cells[rccount, colcount - 1].HorizontalAlign = HorizontalAlign.Center;

                                                                                    //if (dr3[intNCtr - 1].ToString() != "" && dr3[intNCtr - 1].ToString() != "\0")
                                                                                    //{

                                                                                    string[] subjnew = ((dr3[0].ToString())).Split(new Char[] { ';' });

                                                                                    for (int i = 0; i <= subjnew.GetUpperBound(0); i++)
                                                                                    {
                                                                                        if (subjnew.GetUpperBound(0) >= 0)
                                                                                        {
                                                                                            string[] subjstr = subjnew[i].Split(new Char[] { '-' });
                                                                                            if (subjstr.GetUpperBound(0) >= 2)
                                                                                            {
                                                                                                if (hatsubject.Contains(subjstr[0].ToString()))
                                                                                                {
                                                                                                    string strsub = GetFunction("select subject_name from subject where subject_no=" + subjstr[0] + " ");
                                                                                                    splvalnew = splvalnew + ((strsub.ToString()) + "-" + subjstr[1] + "-" + subjstr[2]) + ";";

                                                                                                }
                                                                                                con.Close();

                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    //}
                                                                                    cmd.CommandText = "select top 1 holiday_desc from holidaystudents where holiday_date='" + datefrom.ToString() + "' and degree_code=" + degree_code + " and semester=" + semester + "";
                                                                                    cmd.Connection = mysql2;
                                                                                    mysql2.Close();
                                                                                    mysql2.Open();
                                                                                    SqlDataReader dr_holday = cmd.ExecuteReader();
                                                                                    dr_holday.Read();
                                                                                    //===================

                                                                                    if (dr_holday.HasRows == true)
                                                                                    {


                                                                                        if (dr_holday["holiday_desc"].ToString() == "Sunday")//this line added by manikandan 24/08/2013
                                                                                        {
                                                                                            classreport.Sheets[0].Cells[rccount, colcount - 1].Text = "Sunday Holiday";
                                                                                            classreport.Sheets[0].Cells[rccount, colcount - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                                            classreport.Sheets[0].Columns[colcount - 1].Font.Name = "Book Antiqua";
                                                                                            classreport.Sheets[0].Cells[rccount, colcount - 1].Font.Size = FontUnit.Medium;
                                                                                            classreport.Sheets[0].Cells[rccount, colcount - 1].BackColor = Color.LightSeaGreen;
                                                                                            classreport.Sheets[0].Cells[rccount, colcount - 1].ForeColor = Color.Maroon;

                                                                                            classreport.Sheets[0].Cells[rccount, colcount - 1].Font.Bold = true;
                                                                                        }

                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        classreport.Sheets[0].Cells[rccount, colcount - 1].Text = Convert.ToString(splvalnew);
                                                                                        classreport.Sheets[0].Columns[colcount - 1].Font.Name = "Book Antiqua";
                                                                                        classreport.Sheets[0].Cells[rccount, colcount - 1].Font.Size = FontUnit.Medium;
                                                                                        classreport.Sheets[0].Cells[rccount, colcount - 1].BackColor = Color.LightSeaGreen;
                                                                                        classreport.Sheets[0].Cells[rccount, colcount - 1].ForeColor = Color.Maroon;
                                                                                        classreport.Sheets[0].Cells[rccount, colcount - 1].HorizontalAlign = HorizontalAlign.Left;
                                                                                        classreport.Sheets[0].Cells[rccount, colcount - 1].Font.Bold = true;
                                                                                    }


                                                                                    // }
                                                                                    // }
                                                                                }
                                                                            }
                                                                        }
                                                                    }

                                                                    if (intNCtr == 1)
                                                                    {
                                                                        chsundaycol = colcount;
                                                                    }
                                                                    splvalnew = "";
                                                                    splval = "";
                                                                }
                                                                if (noflag == false)
                                                                {
                                                                    classreport.Visible = true;
                                                                    int col_cnttt = 0;
                                                                    col_cnttt = days;
                                                                    string strDayy = "";
                                                                    strDayy = Session["sun_day"].ToString();
                                                                    if (strDayy != "Sun")
                                                                    {

                                                                        for (intNCtr = 1; intNCtr <= intNHrs; intNCtr++)
                                                                        {
                                                                            classreport.Sheets[0].Cells[rccount, colcount - 1].Text = "No Period";
                                                                            classreport.Sheets[0].Cells[rccount, colcount - 1].ForeColor = Color.Red;
                                                                            classreport.Sheets[0].Cells[rccount, colcount - 1].BackColor = Color.White;
                                                                            classreport.Sheets[0].Cells[rccount, colcount - 1].Font.Bold = true;
                                                                            classreport.Sheets[0].Cells[rccount, colcount - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        int colindex = 0;
                                                                        int numcols = 0;
                                                                        int cols = chsundaycol;
                                                                        for (intNCtr = 1; intNCtr <= intNHrs; intNCtr++)
                                                                        {
                                                                            numcols++;
                                                                            if (intNCtr == 1)
                                                                            {
                                                                                colindex = colcount;
                                                                            }
                                                                            else
                                                                            {
                                                                                cols++;
                                                                            }
                                                                            //classreport.Sheets[0].Cells[rccount, colcount-1].Text = "Sunday";
                                                                            //classreport.Sheets[0].Cells[rccount, colcount-1].ForeColor = Color.Green;
                                                                            //classreport.Sheets[0].Cells[rccount, colcount-1].BackColor = Color.White;
                                                                            //classreport.Sheets[0].Cells[rccount, colcount-1].Font.Bold = true;
                                                                            //classreport.Sheets[0].Cells[rccount, colcount-1].HorizontalAlign = HorizontalAlign.Center;

                                                                            classreport.Sheets[0].Cells[rccount, cols - 1].Text = "Sunday";
                                                                            classreport.Sheets[0].Cells[rccount, cols - 1].ForeColor = Color.Green;
                                                                            classreport.Sheets[0].Cells[rccount, cols - 1].BackColor = Color.White;
                                                                            classreport.Sheets[0].Cells[rccount, cols - 1].Font.Bold = true;
                                                                            classreport.Sheets[0].Cells[rccount, cols - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                        }

                                                                        classreport.Sheets[0].SpanModel.Add(rccount, chsundaycol - 1, 1, numcols);

                                                                    }


                                                                    //   diffdate.Visible = false;
                                                                }


                                                                else
                                                                {
                                                                    classreport.Visible = true;
                                                                    colorpnl.Visible = true;
                                                                    diffdate.Visible = false;
                                                                }

                                                                tempcolcount = tempcolcount + inhighesthr;
                                                                noflag = false;

                                                            }



                                                        }
                                                        else
                                                        {
                                                            semflag = true;
                                                            //errlbl.Visible = true;
                                                            //errlbl.Text = "Update semester Information";
                                                            //Response.Write("<script>alert('Update semester Information')</script>");
                                                            //return;
                                                        }


                                                    }


                                                }
                                            }


                                        }
                                    }
                                }
                            }
                            if (norecord == false)
                            {
                                classreport.Visible = false;
                                colorpnl.Visible = false;
                                errlbl.Visible = true;
                                errlbl.Text = "No Data Found";
                            }
                        }


                        else
                        {
                            diffdate.Visible = true;
                            colorpnl.Visible = false;
                            classreport.Visible = false;
                            txtFromDate.Text = "";
                            txtToDate.Text = "";
                        }

                    }
                    else
                    {
                        tolbl.Visible = true;
                        tolbl.Text = "Entar valid to date";
                        colorpnl.Visible = false;
                        classreport.Visible = false;
                    }
                }
                else
                {
                    tolbl.Visible = true;
                    tolbl.Text = "Entar valid to date";
                    colorpnl.Visible = false;
                    classreport.Visible = false;
                }
            }
            else
            {
                fmlbl.Visible = true;
                fmlbl.Text = "Entar valid From date";
                classreport.Visible = false;
            }
        }
        else
        {
            fmlbl.Visible = true;
            fmlbl.Text = "Entar valid From date";
            colorpnl.Visible = false;
            classreport.Visible = false;
        }

        if (classreport.Sheets[0].Rows.Count > 0)
        {
            btnprintmaster.Visible = true;
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            btnxl.Visible = true;
            classreport.Sheets[0].PageSize = classreport.Sheets[0].RowCount;

        }
        if (classreport.Sheets[0].RowCount == 0)
        {
            btnprintmaster.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;

        }
    }
    //start=====Method Added by Manikandan
    public string findday(string curday, string deg_code, string semester, string batch_year, string sdate, string no_days, string stastdayorder)
    {
        int holiday = 0;
        if (no_days == "")
            return "";
        if (sdate != "")
        {
            string[] sp_date = sdate.Split(new Char[] { '/' });
            string start_date = sp_date[1].ToString() + "-" + sp_date[2].ToString() + "-" + sp_date[0].ToString();

            string[] sp_date_1 = curday.Split(new Char[] { '/' });
            string currentdate = sp_date_1[1].ToString() + "/" + sp_date_1[0].ToString() + "/" + sp_date_1[2].ToString();

            DateTime dt1 = Convert.ToDateTime(sdate);
            DateTime dt2 = Convert.ToDateTime(currentdate);
            TimeSpan ts = dt2 - dt1;
            string query1 = "select count(*)as count from holidaystudents  where degree_code=" + deg_code.ToString() + " and semester=" + semester.ToString() + " and holiday_date between'" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "' and isnull(Not_include_dayorder,0)<>'1'";//01.03.17 barath";";
            string holday = d2.GetFunction(query1);
            if (holday != "")
                holiday = Convert.ToInt32(holday);
            int dif_days = ts.Days;
            int nodays = Convert.ToInt32(no_days);
            int order = (dif_days - holiday) % nodays;
            order = order + 1;

            //----------------------------------------------------------     

            if (stastdayorder.ToString().Trim() != "")
            {
                if ((stastdayorder.ToString().Trim() != "1") && (stastdayorder.ToString().Trim() != "0"))
                {
                    order = order + (Convert.ToInt16(stastdayorder) - 1);
                    if (order == (nodays + 1))
                        order = 1;
                    else if (order > nodays)
                        order = order % nodays;
                }
            }
            //-----------------------------------------------------------


            string findday = "";
            if (order == 1)
                findday = "mon";
            else if (order == 2) findday = "tue";
            else if (order == 3) findday = "wed";
            else if (order == 4) findday = "thu";
            else if (order == 5) findday = "fri";
            else if (order == 6) findday = "sat";
            else if (order == 7) findday = "sun";

            Day_Order = Convert.ToString(order) + "-" + Convert.ToString(findday);
            return findday;
        }
        else
            return "";

    }
    //End===================

    public string GetFunction(string Att_strqueryst)
    {
        string sqlstr;
        sqlstr = Att_strqueryst;
        con.Close();
        con.Open();
        SqlDataReader drnew;
        SqlCommand cmd;
        cmd = new SqlCommand(sqlstr, con);
        cmd.Connection = con;
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



    private string GetSyllabusYear(string degree_code, string batch_year, string sem)
    {
        string syl_year = "";
        con.Close();
        con.Open();
        SqlCommand cmd2a;
        SqlDataReader get_syl_year;
        cmd2a = new SqlCommand("select syllabus_year from syllabus_master where degree_code=" + Session["degree_code"] + " and semester =" + Session["semester"] + " and batch_year=" + Session["batch_year"] + " ", con);
        get_syl_year = cmd2a.ExecuteReader();
        get_syl_year.Read();
        if (get_syl_year.HasRows == true)
        {
            if (get_syl_year[0].ToString() == "\0")
            {
                syl_year = "-1";
            }
            else
            {
                syl_year = get_syl_year[0].ToString();
            }
        }
        else
        {
            syl_year = "-1";
        }
        return syl_year;
        con.Close();
    }
    //protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    bindbranch();
    //    bindsem();
    //    bindsec();
    //    classreport.Visible = false;
    //    fmlbl.Visible = false;
    //    tolbl.Visible = false;
    //    errlbl.Visible = false;
    //    colorpnl.Visible = false;
    //}
    //protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    classreport.Visible = false;
    //    colorpnl.Visible = false;
    //    fmlbl.Visible = false;
    //    tolbl.Visible = false;
    //    errlbl.Visible = false;
    //}

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        fmlbl.Visible = false;
        tolbl.Visible = false;
        errlbl.Visible = false;
    }
    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        tolbl.Visible = false;
        errlbl.Visible = false;
    }

    public void BindBatch()
    {
        try
        {

            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBatch();
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstbatch.DataSource = ds2;
                chklstbatch.DataTextField = "Batch_year";
                chklstbatch.DataValueField = "Batch_year";
                chklstbatch.DataBind();
                chklstbatch.SelectedIndex = chklstbatch.Items.Count - 1;
                for (int i = 0; i < chklstbatch.Items.Count; i++)
                {
                    chklstbatch.Items[i].Selected = true;
                    if (chklstbatch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstbatch.Items.Count == count)
                    {
                        chkbatch.Checked = true;
                    }

                }

            }
        }
        catch (Exception ex)
        {

        }

    }

    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            chklstdegree.Items.Clear();
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstdegree.DataSource = ds2;
                chklstdegree.DataTextField = "course_name";
                chklstdegree.DataValueField = "course_id";
                chklstdegree.DataBind();
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                    if (chklstdegree.Items[i].Selected == true)
                    {
                        count1 += 1;
                    }
                    if (chklstdegree.Items.Count == count1)
                    {
                        chkdegree.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                }
            }
            //course_id = chklstdegree.SelectedValue.ToString();
            //chklstbranch.Items.Clear();
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstbranch.DataSource = ds2;
                chklstbranch.DataTextField = "dept_name";
                chklstbranch.DataValueField = "degree_code";
                chklstbranch.DataBind();
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                    if (chklstbranch.Items[i].Selected == true)
                    {
                        count2 += 1;
                    }
                    if (chklstbranch.Items.Count == count2)
                    {
                        chkbranch.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void BindSectionDetailmult(string collegecode)
    {
        try
        {
            int takecount = 0;
            //strbranch = chklstbranch.SelectedValue.ToString();

            chklstsection.Items.Clear();
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindSectionDetailmult(collegecode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                takecount = ds2.Tables[0].Rows.Count;
                chklstsection.DataSource = ds2;
                chklstsection.DataTextField = "sections";
                chklstsection.DataBind();
                chklstsection.Items.Insert(takecount, "Empty");
                if (Convert.ToString(ds2.Tables[0].Columns["sections"]) == string.Empty)
                {
                    chklstsection.Enabled = false;
                }
                else
                {
                    chklstsection.Enabled = true;
                    chklstsection.SelectedIndex = chklstsection.Items.Count - 2;
                    for (int i = 0; i < chklstsection.Items.Count; i++)
                    {
                        chklstsection.Items[i].Selected = true;
                        if (chklstsection.Items[i].Selected == true)
                        {
                            count3 += 1;
                        }
                        if (chklstsection.Items.Count == count3)
                        {
                            chksection.Checked = true;
                        }
                    }
                }
            }
            else
            {
                chklstsection.Enabled = false;
            }

            loadsubject();
        }
        catch (Exception ex)
        {

        }

    }

    protected void chkbatch_CheckedChanged(object sender, EventArgs e)
    {
        if (chkbatch.Checked == true)
        {
            for (int i = 0; i < chklstbatch.Items.Count; i++)
            {
                chklstbatch.Items[i].Selected = true;
                txtbatch.Text = "" + lblbatch.Text + "(" + (chklstbatch.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklstbatch.Items.Count; i++)
            {
                chklstbatch.Items[i].Selected = false;
                txtbatch.Text = "---Select---";
            }
        }
    }

    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        pbatch.Focus();

        int batchcount = 0;
        string value = "";
        string code = "";


        for (int i = 0; i < chklstbatch.Items.Count; i++)
        {
            if (chklstbatch.Items[i].Selected == true)
            {

                value = chklstbatch.Items[i].Text;
                code = chklstbatch.Items[i].Value.ToString();
                batchcount = batchcount + 1;
                txtbatch.Text = "" + lblbatch.Text + "(" + batchcount.ToString() + ")";
            }

        }

        if (batchcount == 0)
            txtbatch.Text = "---Select---";
        else
        {
            Label lbl = batchlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = batchimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(batchimg_Click);
        }
        batchcnt = batchcount;
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        loadsubject();
    }

    public Label batchlabel()
    {
        Label lbc = new Label();

        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton batchimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }

    public void batchimg_Click(object sender, ImageClickEventArgs e)
    {
        batchcnt = batchcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstbatch.Items[r].Selected = false;

        txtbatch.Text = "Batch(" + batchcnt.ToString() + ")";
        if (txtbatch.Text == "Batch(0)")
        {
            txtbatch.Text = "---Select---";

        }

    }

    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        if (chkbranch.Checked == true)
        {
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                chklstbranch.Items[i].Selected = true;
                txtbranch.Text = "" + lblbranch.Text + "(" + (chklstbranch.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                chklstbranch.Items[i].Selected = false;
                txtbranch.Text = "---Select---";
            }
        }
        loadsubject();
    }

    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        pbranch.Focus();

        int branchcount = 0;
        string value = "";
        string code = "";


        for (int i = 0; i < chklstbranch.Items.Count; i++)
        {
            if (chklstbranch.Items[i].Selected == true)
            {

                value = chklstbranch.Items[i].Text;
                code = chklstbranch.Items[i].Value.ToString();
                branchcount = branchcount + 1;
                txtbranch.Text = "" + lblbranch.Text + "(" + branchcount.ToString() + ")";
            }

        }

        if (branchcount == 0)
            txtbranch.Text = "---Select---";
        else
        {
            Label lbl = branchlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = branchimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(branchimg_Click);
        }
        branchcnt = branchcount;
        //BindSem(strbranch, strbatchyear, collegecode);
        BindSectionDetailmult(collegecode);
        loadsubject();

    }

    protected void LinkButtonbranch_Click(object sender, EventArgs e)
    {

        chklstbranch.ClearSelection();
        branchcnt = 0;
        txtbranch.Text = "---Select---";
    }

    public void branchimg_Click(object sender, ImageClickEventArgs e)
    {
        branchcnt = branchcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstbranch.Items[r].Selected = false;

        txtdegree.Text = "Branch(" + branchcnt.ToString() + ")";
        if (txtdegree.Text == "Branch(0)")
        {
            txtdegree.Text = "---Select---";

        }

    }

    public Label branchlabel()
    {
        Label lbc = new Label();

        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton branchimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }
    protected void chksection_CheckedChanged(object sender, EventArgs e)
    {
        if (chksection.Checked == true)
        {
            for (int i = 0; i < chklstsection.Items.Count; i++)
            {
                chklstsection.Items[i].Selected = true;
                txtsection.Text = "Section(" + (chklstsection.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklstsection.Items.Count; i++)
            {
                chklstsection.Items[i].Selected = false;
                txtsection.Text = "---Select---";
            }
        }

        loadsubject();
    }

    protected void chklstsection_SelectedIndexChanged(object sender, EventArgs e)
    {
        psection.Focus();

        int sectioncount = 0;
        string value = "";
        string code = "";


        for (int i = 0; i < chklstsection.Items.Count; i++)
        {
            if (chklstsection.Items[i].Selected == true)
            {

                value = chklstsection.Items[i].Text;
                code = chklstsection.Items[i].Value.ToString();
                sectioncount = sectioncount + 1;
                txtsection.Text = "Section(" + sectioncount.ToString() + ")";
            }

        }

        if (sectioncount == 0)
            txtsection.Text = "---Select---";
        else
        {
            Label lbl = sectionlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = sectionimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(sectionimg_Click);
        }
        sectioncnt = sectioncount;
        loadsubject();

    }

    protected void LinkButtonsection_Click(object sender, EventArgs e)
    {

        chklstsection.ClearSelection();
        sectioncnt = 0;
        txtsection.Text = "---Select---";
    }

    public void sectionimg_Click(object sender, ImageClickEventArgs e)
    {
        sectioncnt = sectioncnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstsection.Items[r].Selected = false;

        txtsection.Text = "Section(" + sectioncnt.ToString() + ")";
        if (txtsection.Text == "Section(0)")
        {
            txtsection.Text = "---Select---";

        }

    }

    public Label sectionlabel()
    {
        Label lbc = new Label();
        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton sectionimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }

    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        if (chkdegree.Checked == true)
        {
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                chklstdegree.Items[i].Selected = true;
                txtdegree.Text = "" + lbldegree.Text + "(" + (chklstdegree.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                chklstdegree.Items[i].Selected = false;
                txtdegree.Text = "---Select---";
            }
        }
    }

    protected void chklstdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        pdegree.Focus();

        int degreecount = 0;
        string value = "";
        string code = "";


        for (int i = 0; i < chklstdegree.Items.Count; i++)
        {
            if (chklstdegree.Items[i].Selected == true)
            {

                value = chklstdegree.Items[i].Text;
                code = chklstdegree.Items[i].Value.ToString();
                degreecount = degreecount + 1;
                txtdegree.Text = "" + lbldegree.Text + "(" + degreecount.ToString() + ")";
            }

        }

        if (degreecount == 0)
            txtdegree.Text = "---Select---";
        else
        {
            Label lbl = degreelabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = degreeimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(degreeimg_Click);
        }
        degreecnt = degreecount;
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        loadsubject();
    }

    protected void LinkButtondegree_Click(object sender, EventArgs e)
    {

        chklstdegree.ClearSelection();
        degreecnt = 0;
        txtdegree.Text = "---Select---";
    }

    public void degreeimg_Click(object sender, ImageClickEventArgs e)
    {
        degreecnt = degreecnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstdegree.Items[r].Selected = false;

        txtdegree.Text = "Degree(" + degreecnt.ToString() + ")";
        if (txtdegree.Text == "Degree(0)")
        {
            txtdegree.Text = "---Select---";

        }

    }

    public Label degreelabel()
    {
        Label lbc = new Label();

        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton degreeimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {

            norecordlbl.Visible = false;
            if (classreport.Visible == true)
            {
                Session["column_header_row_count"] = 2;


                string degreedetails = "Class Report @ From Date : " + txtFromDate.Text + " To Date " + txtToDate.Text + "";
                string pagename = "indclassreport.aspx";
                Printcontrol.loadspreaddetails(classreport, pagename, degreedetails);
                Printcontrol.Visible = true;
            }
            else
            {
                norecordlbl.Visible = true;
                norecordlbl.Text = "Please Click Go Button Before Print";
            }
        }
        catch (Exception ex)
        {
        }

    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            //Modified by Srinath 27/2/2013
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(classreport, reportname);
            }
            else
            {
                norecordlbl.Text = "Please Enter Your Report Name";
                norecordlbl.Visible = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void loadsubject()
    {
        string staff_code = Session["staff_code"].ToString();
        string strquery = "";
        int itemcount = 0;
        string batchva = "", degreeva = "", secva = "";

        if (txtbatch.Text != "---Select---" || chklstbatch.Items.Count != null)
        {
            for (itemcount = 0; itemcount < chklstbatch.Items.Count; itemcount++)
            {
                if (chklstbatch.Items[itemcount].Selected == true)
                {
                    if (batchva == "")
                    {
                        batchva = chklstbatch.Items[itemcount].Text;
                    }
                    else
                    {
                        batchva = batchva + ',' + chklstbatch.Items[itemcount].Text;
                    }
                }
            }
        }

        for (itemcount = 0; itemcount < chklstbranch.Items.Count; itemcount++)
        {
            if (chklstbranch.Items[itemcount].Selected == true)
            {
                if (degreeva == "")
                {
                    degreeva = chklstbranch.Items[itemcount].Value.ToString();
                }
                else
                {
                    degreeva = degreeva + ',' + chklstbranch.Items[itemcount].Value.ToString();
                }
            }
        }

        for (itemcount = 0; itemcount < chklstsection.Items.Count - 1; itemcount++)
        {
            if (chklstsection.Items[itemcount].Selected == true)
            {
                if (secva == "")
                {
                    secva = "'" + chklstsection.Items[itemcount].Text.ToString() + "'";
                }
                else
                {
                    secva = secva + ",'" + chklstsection.Items[itemcount].Text.ToString() + "'";
                }
            }
        }
        string batchset = "", degreeset = "", secset = "";
        if (secva != "" && secva != null)
        {
            secva = " r.sections in (" + secva + ")";
        }
        if (secva == "" && chklstsection.Items[chklstsection.Items.Count - 1].Selected == true)
        {
            secset = " and ( or r.sections is null or r.sections='' or r.sections='-1')";
        }
        else if (secva != "" && chklstsection.Items[chklstsection.Items.Count - 1].Selected == true)
        {
            secset = " and ( " + secva + " or r.sections is null or r.sections='' or r.sections='-1')";
        }



        if (batchva != "" && batchva != null)
        {
            batchset = " and r.batch_year in (" + batchva + ")";
        }
        if (degreeva != "" && degreeva != null)
        {
            degreeset = " and r.degree_code in (" + degreeva + ")";
        }


        if (staff_code == null || staff_code == "")
        {
            strquery = "select distinct s.subject_name,s.subject_code from subject s,syllabus_master sy,staff_selector st,registration r,sub_sem sb where sb.subtype_no=s.subtype_no and r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year  and cc=0 and delflag=0 and exam_flag<>'debar' " + batchset + " " + degreeset + " order by s.subject_name,s.subject_code";
        }
        else
        {
            strquery = "select distinct s.subject_name,s.subject_code from subject s,syllabus_master sy,staff_selector st,registration r,sub_sem sb where sb.subtype_no=s.subtype_no and r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year  and staff_code='" + Session["staff_code"].ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar' " + batchset + " " + degreeset + " " + secset + " order by s.subject_name,s.subject_code";
        }
        chklssubject.Items.Clear();
        DataSet dssubject = d2.select_method_wo_parameter(strquery, "Text");
        if (dssubject.Tables[0].Rows.Count > 0)
        {
            chklssubject.DataSource = dssubject;
            chklssubject.DataTextField = "subject_name";
            chklssubject.DataValueField = "subject_code";
            chklssubject.DataBind();

            for (int i = 0; i < chklssubject.Items.Count; i++)
            {
                chklssubject.Items[i].Selected = true;
            }
            txtsubject.Text = "Subject(" + (chklssubject.Items.Count) + ")";
            chksubject.Checked = true;
        }
        else
        {
            txtsubject.Text = "---Select---";
        }
    }

    protected void chksubject_CheckedChanged(object sender, EventArgs e)
    {
        if (chksubject.Checked == true)
        {
            for (int i = 0; i < chklssubject.Items.Count; i++)
            {
                chklssubject.Items[i].Selected = true;
            }
            txtsubject.Text = "Subject(" + (chklssubject.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < chklssubject.Items.Count; i++)
            {
                chklssubject.Items[i].Selected = false;
                txtsubject.Text = "---Select---";
            }
        }
    }

    protected void chklssubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        int subcount = 0;
        for (int i = 0; i < chklssubject.Items.Count; i++)
        {
            if (chklssubject.Items[i].Selected == true)
            {
                subcount = subcount + 1;
            }

        }
        if (subcount == 0)
        {
            txtsubject.Text = "---Select---";
            chksubject.Checked = false;
        }
        else if (subcount == chklssubject.Items.Count)
        {
            txtsubject.Text = "Subject(" + subcount.ToString() + ")";
            chksubject.Checked = true;
        }
        else
        {
            txtsubject.Text = "Subject(" + subcount.ToString() + ")";
            chksubject.Checked = false;
        }
    }
}
