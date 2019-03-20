using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using System.Drawing;


public partial class MasterTimeTable : System.Web.UI.Page
{
    static Boolean forschoolsetting = false;
    Hashtable hat = new Hashtable();

    DataSet ds_load = new DataSet();
    DAccess2 daccess = new DAccess2();
    DAccess2 dac = new DAccess2();
    string collegecode = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";
    string strbatch = string.Empty;
    string strbatchyear = string.Empty;
    string strbranch = string.Empty;
    string userCollegeCode = string.Empty;

    #region
    DataTable dtable = new DataTable();
    DataRow dtrow = null;
    DataTable dtable1 = new DataTable();
    DataRow dtrow1 = null;
    Hashtable hperiod = new Hashtable();
    Hashtable hnum = new Hashtable();
    Hashtable hatcol = new Hashtable();
    static ArrayList rowperiod = new ArrayList();
    static ArrayList rowhead = new ArrayList();
    #endregion


    //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);

    protected void Init(object sender, EventArgs e)
    {
        gview.Visible = false;
        gviewstaff.Visible = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("AttendanceHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/AttendanceMOD/AttendanceHome.aspx");
                    return;
                }
            }
            //if (Session["collegecode"] == null) //Aruna For Back Button
            //{
            //    Response.Redirect("~/Default.aspx");
            //}
            userCollegeCode = Convert.ToString(Session["collegecode"]);
            collegecode = Session["collegecode"].ToString();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            // Session["Semester"] =Convert.ToString(ddlSem.SelectedValue);
            if (!IsPostBack)
            {
                //'--------------------------------------
                collegecode = Session["collegecode"].ToString();
                usercode = Session["usercode"].ToString();
                singleuser = Session["single_user"].ToString();
                group_user = Session["group_code"].ToString();
                gview.Visible = false;
                gviewstaff.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnprintmaster.Visible = false;
                btnxl.Visible = false;

                Lblreport.Visible = false;
                txtexcl.Visible = false;
                btnprnt.Visible = false;
                btnxcl.Visible = false;

                lblexer.Visible = false;


                bindbatch();
                binddegree();
                bindbranch();
                colg();
                // bindsem();
                string grouporusercode = "";
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                // Added By Sridharan 12 Mar 2015
                //{
                DataSet schoolds = new DataSet();
                string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode + "";
                schoolds.Clear();
                schoolds.Dispose();
                schoolds = daccess.select_method_wo_parameter(sqlschool, "Text");
                if (schoolds.Tables[0].Rows.Count > 0)
                {
                    string schoolvalue = schoolds.Tables[0].Rows[0]["value"].ToString();
                    if (schoolvalue.Trim() == "0")
                    {
                        forschoolsetting = true;
                        Lblclg.Text = "School";
                        lblBatch.Text = "Year";
                        lblDegree.Text = "School Type";
                        lblBranch.Text = "Standard";
                        //lblsem.Text = "Term";
                        //lblDegree.Attributes.Add("style", " width: 95px;");
                        //lblBranch.Attributes.Add("style", " width: 67px;");
                        //ddlBranch.Attributes.Add("style", " width: 241px;");
                    }
                    else
                    {
                        forschoolsetting = false;
                    }
                }
                //} Sridharan
            }
            lblexer.Visible = false;
        }
        catch
        {
        }
    }

    public void bindbatch()
    {
        ddlBatch.Items.Clear();
        ds_load = daccess.select_method_wo_parameter("bind_batch", "sp");
        int count = ds_load.Tables[0].Rows.Count;
        if (count > 0)
        {
            ddlBatch.DataSource = ds_load;
            ddlBatch.DataTextField = "batch_year";
            ddlBatch.DataValueField = "batch_year";
            ddlBatch.DataBind();
        }
        int count1 = ds_load.Tables[1].Rows.Count;
        if (count > 0)
        {
            int max_bat = 0;
            max_bat = Convert.ToInt32(ds_load.Tables[1].Rows[0][0].ToString());
            ddlBatch.SelectedValue = max_bat.ToString();
        }
        ddlBatch.Items.Insert(0, "ALL");
    }

    public void bindbranch()
    {
        ddlBranch.Items.Clear();
        hat.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        hat.Add("single_user", singleuser.ToString());
        hat.Add("group_code", group_user);
        hat.Add("course_id", ddlDegree.SelectedValue);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);

        ds_load = daccess.select_method("bind_branch", hat, "sp");
        int count2 = ds_load.Tables[0].Rows.Count;
        if (count2 > 0)
        {
            ddlBranch.DataSource = ds_load;
            ddlBranch.DataTextField = "dept_name";
            ddlBranch.DataValueField = "degree_code";
            ddlBranch.DataBind();
        }
    }

    public void binddegree()
    {
        ddlDegree.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        hat.Clear();
        hat.Add("single_user", singleuser.ToString());
        hat.Add("group_code", group_user);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        ds_load = daccess.select_method("bind_degree", hat, "sp");
        int count1 = ds_load.Tables[0].Rows.Count;
        if (count1 > 0)
        {
            ddlDegree.DataSource = ds_load;
            ddlDegree.DataTextField = "course_name";
            ddlDegree.DataValueField = "course_id";
            ddlDegree.DataBind();
        }
    }

    public void BindBranch(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            course_id = ddlDegree.SelectedValue.ToString();
            ddlBranch.Items.Clear();
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds_load.Dispose();
            ds_load.Reset();
            ds_load = daccess.BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            if (ds_load.Tables[0].Rows.Count > 0)
            {
                ddlBranch.DataSource = ds_load;
                ddlBranch.DataTextField = "dept_name";
                ddlBranch.DataValueField = "degree_code";
                ddlBranch.DataBind();
            }
        }
        catch (Exception)
        {
            // errmsg.Text = ex.ToString();
        }
    }

    public void colg()
    {

        hat.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        string clgname = "select college_code,collname from collinfo ";
        if (clgname != "")
        {
            ds_load = daccess.select_method(clgname, hat, "Text");
            ddlclg.DataSource = ds_load;
            ddlclg.DataTextField = "collname";
            ddlclg.DataValueField = "college_code";
            ddlclg.DataBind();
        }

    }

    /* protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
     {
         ddlBranch.Items.Clear();

         string course_id = ddlDegree.SelectedValue.ToString();
         BindBranch(singleuser, group_user, course_id, collegecode, usercode);
     }*/

    protected void btnGo_Click(object sender, EventArgs e)
    {
        loadfunction();
    }

    protected void loadfunction()
    {
        try
        {
            bool flag = false;
            rowhead.Clear();
            rowperiod.Clear();
            Hashtable hatHr = new Hashtable();
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            btnprintmaster.Visible = true;
            btnxl.Visible = true;

            btnxcl.Visible = true;
            Lblreport.Visible = true;
            txtexcl.Visible = true;
            btnprnt.Visible = true;

            lblerr.Visible = false;
            lblexer.Visible = false;
            int noofhours = 0;
            int noofdays = 0;
            int dayorder = 0;

            string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            string[] Daymon = new string[7] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            strbatchyear = ddlBatch.Text.ToString();
            strbranch = ddlBranch.SelectedValue.ToString();
            string strpriodquery = string.Empty;
            string cursems = string.Empty;
            if (ddlBatch.SelectedItem.Text.ToLower().Trim() != "all") //added by mullai
            {
                string cursems1 = daccess.GetFunction("select distinct Current_Semester from Registration  where  degree_code='" + ddlBranch.SelectedItem.Value.ToString() + "' and Batch_Year ='" + ddlBatch.SelectedItem.Text.ToString() + "' and CC=0 and DelFlag=0 and Exam_Flag<>'debar'");
                cursems = " and semester='" + cursems1 + "'";
            }

            strpriodquery = "Select No_of_hrs_per_day,nodays from PeriodAttndSchedule where degree_code = " + ddlBranch.SelectedValue.ToString() + " " + cursems + " order by No_of_hrs_per_day desc";
            ds_load = daccess.select_method(strpriodquery, hat, "Text");
            if (ds_load.Tables[0].Rows.Count > 0)
            {

                noofhours = Convert.ToInt32(ds_load.Tables[0].Rows[0]["No_of_hrs_per_day"]);
                noofdays = Convert.ToInt32(ds_load.Tables[0].Rows[0]["nodays"]);
                // noofsem = Convert.ToInt32(ds_load.Tables[0].Rows[0]["semester"]);

                Session["totalhrs"] = Convert.ToString(noofhours);
                Session["totnoofdays"] = Convert.ToString(noofdays);
                // Session["semorder"] = Convert.ToString(noofsem);
            }
            


            if (noofhours > 0)
            {

                dtable.Columns.Add("S.No");
                dtable.Columns.Add("Day/Timings");

                rowperiod.Add("S.No");
                rowperiod.Add("Day/Timings");

                rowhead.Add("S.No");
                rowhead.Add("Day/Timings");

                if (forschoolsetting != true)
                {
                    dtable.Columns.Add("Semester");
                    rowperiod.Add("Semester");
                    rowhead.Add("Semester");
                }
                else
                {
                    dtable.Columns.Add("Term");
                    rowperiod.Add("Term");
                    rowhead.Add("Term");
                }

                dtrow = dtable.NewRow();
                dtable.Rows.Add(dtrow);
                dtrow = dtable.NewRow();
                dtable.Rows.Add(dtrow);

                dtable1.Columns.Add("S.No");
                dtable1.Columns.Add("SUBJECT CODE");
                dtable1.Columns.Add("SUBJECT NAME");
                dtable1.Columns.Add("TOTAL HOURS");
                dtable1.Columns.Add("STAFF");
                dtable1.Columns.Add("STAFF DEPARTMENT");

                dtrow = dtable1.NewRow();
                dtrow["S.No"] = "S.No";
                dtrow["SUBJECT CODE"] = "SUBJECT CODE";
                dtrow["SUBJECT NAME"] = "SUBJECT NAME";
                dtrow["TOTAL HOURS"] = "TOTAL HOURS";
                dtrow["STAFF"] = "STAFF";
                dtrow["STAFF DEPARTMENT"] = "STAFF DEPARTMENT";
                dtable1.Rows.Add(dtrow);

                // if (ddlBatch.SelectedValue.ToString()!= "ALL")
                // {
                // Fptimetable.Sheets[0].ColumnCount = noofhours + 4;
                string sqlsmm = "";
                if (ddlBatch.SelectedItem.Text != "ALL")
                {
                    sqlsmm = " select distinct batch_year,Current_Semester from Registration  where  degree_code=" + ddlBranch.SelectedValue.ToString() + " and Batch_Year =" + ddlBatch.SelectedItem.Text + " and CC=0 and DelFlag=0 and Exam_Flag<>'debar'"; // added by jairam 18-11-2014
                }
                else
                {
                    sqlsmm = " select distinct batch_year,Current_Semester from Registration  where  degree_code=" + ddlBranch.SelectedValue.ToString() + "  and CC=0 and DelFlag=0 and Exam_Flag<>'debar'";
                }
                DataSet dlsql = daccess.select_method(sqlsmm, hat, "Text");
                if (dlsql.Tables[0].Rows.Count > 0)
                {
                    string semes = dlsql.Tables[0].Rows[0]["Current_Semester"].ToString();
                    string sqlbrk = "select no_of_hrs_I_half_day from PeriodAttndSchedule where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + semes + "";
                    DataSet dsbrk = daccess.select_method(sqlbrk, hat, "Text");
                    if (dsbrk.Tables[0].Rows.Count > 0)
                    {
                        string brk = dsbrk.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString();
                        int columncount = 0;

                        for (int i = 1; i <= Convert.ToInt32(brk); i++)
                        {
                            //Fptimetable.Sheets[0].ColumnHeader.Cells[0, i + 2].Text = "Period " + i + "";   //added by Mullai
                            hperiod.Add(i, "Period " + i + "");
                            rowperiod.Add("Period " + i + "");
                            string belltime = "";
                            string sttimequery = "";
                            if (ddlBatch.SelectedItem.Text != "ALL")
                            {
                                sttimequery = "Select start_time,end_time from BellSchedule where degree_code=" + ddlBranch.SelectedValue.ToString() + " and period1='" + i + "' and batch_year ='" + ddlBatch.SelectedItem.Text + "' and semester ='" + semes + "'";  // added by jairam 18-11-2014
                            }
                            else
                            {
                                sttimequery = "Select start_time,end_time from BellSchedule where degree_code=" + ddlBranch.SelectedValue.ToString() + " and period1='" + i + "' and semester ='" + semes + "'";  // added by jairam 18-11-2014
                            }
                            ds_load = daccess.select_method(sttimequery, hat, "Text");
                            if (ds_load.Tables[0].Rows.Count > 0)
                            {
                                if (ds_load.Tables[0].Rows[0]["start_time"].ToString() != "" && ds_load.Tables[0].Rows[0]["start_time"].ToString() != null && ds_load.Tables[0].Rows[0]["end_time"].ToString() != "" && ds_load.Tables[0].Rows[0]["end_time"].ToString() != null)
                                {
                                    string[] splitstarttime = ds_load.Tables[0].Rows[0]["start_time"].ToString().Split(' ');
                                    string[] splitendtime = ds_load.Tables[0].Rows[0]["end_time"].ToString().Split(' ');
                                    belltime = splitstarttime[1].ToString() + ' ' + splitstarttime[2].ToString() + ' ' + " To" + ' ' + splitendtime[1].ToString() + ' ' + splitendtime[2].ToString();

                                }


                                hnum.Add(i, i);
                                dtable.Columns.Add(belltime);

                                rowhead.Add(belltime);
                            }

                            else
                            {
                                hnum.Add(i, i);
                                dtable.Columns.Add(i.ToString());

                                rowhead.Add(i.ToString());
                            }
                        }

                        for (int k = Convert.ToInt32(brk) + 1; k <= noofhours; k++)
                        {
                            //Fptimetable.Sheets[0].ColumnHeader.Cells[0, k+3].Text = "Period " + k + "";  //added by Mullai
                            hperiod.Add(k, "Period " + k + "");
                            rowperiod.Add("Period " + k + "");
                            string belltime = "";
                            string sttimequery = "";
                            if (ddlBatch.SelectedItem.Text != "ALL")
                            {
                                sttimequery = "Select distinct start_time,end_time from BellSchedule where degree_code=" + ddlBranch.SelectedValue.ToString() + " and period1='" + k + "' and batch_year ='" + ddlBatch.SelectedItem.Text + "' and semester='" + semes + "'"; // added by jairam 18-11-2014
                            }
                            else
                            {
                                sttimequery = "Select distinct start_time,end_time from BellSchedule where degree_code=" + ddlBranch.SelectedValue.ToString() + " and period1='" + k + "'  and semester  in (select distinct Current_Semester from Registration  where  degree_code='" + ddlBranch.SelectedValue.ToString() + "'  and CC=0 and DelFlag=0 and Exam_Flag<>'debar')"; // added by jairam 18-11-2014
                            }
                            ds_load = daccess.select_method(sttimequery, hat, "Text");
                            if (ds_load.Tables[0].Rows.Count > 0)
                            {
                                if (ds_load.Tables[0].Rows[0]["start_time"].ToString() != "" && ds_load.Tables[0].Rows[0]["start_time"].ToString() != null && ds_load.Tables[0].Rows[0]["end_time"].ToString() != "" && ds_load.Tables[0].Rows[0]["end_time"].ToString() != null)
                                {
                                    string[] splitstarttime = ds_load.Tables[0].Rows[0]["start_time"].ToString().Split(' ');
                                    string[] splitendtime = ds_load.Tables[0].Rows[0]["end_time"].ToString().Split(' ');
                                    belltime = splitstarttime[1].ToString() + ' ' + splitstarttime[2].ToString() + ' ' + " To " + ' ' + splitendtime[1].ToString() + ' ' + splitendtime[2].ToString();

                                }
                                //Fptimetable.Sheets[0].ColumnHeader.Cells[0, Fptimetable.Sheets[0].ColumnCount - 1].Text = k.ToString();
                                dtable.Columns.Add(belltime);
                                rowhead.Add(belltime);
                            }
                            else
                            {
                                hnum.Add(k, k);
                                dtable.Columns.Add(k.ToString());
                                rowhead.Add(k.ToString());
                            }
                        }
                    }
                    else
                    {
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnprintmaster.Visible = false;
                        btnxl.Visible = false;

                        gviewstaff.Visible = false;
                        gview.Visible = false;
                        lblerr.Text = "No Records Found";
                        lblerr.Visible = true;

                        btnxcl.Visible = false;
                        Lblreport.Visible = false;
                        txtexcl.Visible = false;
                        btnprnt.Visible = false;
                        return;
                    }
                }
            }
            else
            {
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnprintmaster.Visible = false;
                btnxl.Visible = false;

                gviewstaff.Visible = false;
                gview.Visible = false;
                lblerr.Text = "No Records Found";
                lblerr.Visible = true;

                btnxcl.Visible = false;
                Lblreport.Visible = false;
                txtexcl.Visible = false;
                btnprnt.Visible = false;
                return;
            }

            int r = 0;
            int delflag = 0; // Sangeetha On 29 Aug 2014
            for (int day = 0; day < noofdays; day++)
            {

                r++;
                string dayofweek = Days[day];
                string dayofweek1 = Daymon[day];
                string dayvalue = "";
                for (int i = 1; i <= noofhours; i++)
                {

                    if (dayvalue == "")
                    {
                        dayvalue = dayofweek + i;
                    }
                    else
                    {
                        dayvalue = dayvalue + ',' + dayofweek + i;
                    }
                }

                string batch = "";
                if (ddlBatch.SelectedValue.ToString() != "ALL")
                {
                    batch = "and batch_year=" + ddlBatch.SelectedValue.ToString();

                }
                else
                {
                    batch = "";
                }

                string semsql = "select distinct batch_year,Current_Semester,ltrim(rtrim(isnull(sections,''))) as Sections from Registration  where  degree_code=" + ddlBranch.SelectedValue.ToString() + " and CC=0 and DelFlag=0 and Exam_Flag<>'debar'" + batch + " order by batch_year desc";
                DataSet dssemester = daccess.select_method(semsql, hat, "Text");

                if (dssemester.Tables[0].Rows.Count > 0)
                {

                    for (int k = 0; k <= dssemester.Tables[0].Rows.Count - 1; k++)
                    {
                        string sec = "";
                        string semester = dssemester.Tables[0].Rows[k]["Current_Semester"].ToString();
                        string batchyear = dssemester.Tables[0].Rows[k]["batch_year"].ToString();
                        string strsection = dssemester.Tables[0].Rows[k]["Sections"].ToString();
                        if (strsection.Trim() == "" || strsection == null || strsection.Trim() == "-1")
                        {
                            sec = " ";
                        }
                        else
                        {
                            sec = " and sections='" + dssemester.Tables[0].Rows[k]["Sections"].ToString() + "'";
                        }
                        string schedule = "Select  top 1 " + dayvalue + " from semester_schedule where degree_code=" + ddlBranch.SelectedValue.ToString() + "and batch_year=" + batchyear + " " + sec + " and semester='" + semester + "' order by FromDate Desc";
                        ds_load = daccess.select_method(schedule, hat, "Text");


                        if (ds_load.Tables[0].Rows.Count > 0)
                        {

                            for (int i = 0; i < ds_load.Tables[0].Rows.Count; i++)
                            {
                                dtrow = dtable.NewRow();

                                // Fptimetable.Sheets[0].Cells[Fptimetable.Sheets[0].RowCount - 1, 0].Text = srno+1.ToString();
                                string value = dayofweek;

                                for (int j = 1; j < ds_load.Tables[0].Columns.Count + 1; j++)
                                {

                                    string dsvalue = value + j;
                                    string classhour = ds_load.Tables[0].Rows[i]["" + dsvalue + ""].ToString();
                                    if (classhour.Trim() != "" && classhour.Trim() != "0" && classhour != null)
                                    {
                                        string[] spiltmulpl = classhour.Split(';');
                                        string setclasshour = "";
                                        for (int mul = 0; mul <= spiltmulpl.GetUpperBound(0); mul++)
                                        {

                                            string[] spiltclasshour = spiltmulpl[mul].Split('-');

                                            string sqlbrk = "select no_of_hrs_I_half_day from PeriodAttndSchedule where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester =" + semester + "";
                                            DataSet dsbrk = daccess.select_method(sqlbrk, hat, "Text");

                                            string brk = dsbrk.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString();

                                            if (j <= Convert.ToInt32(brk))
                                            {
                                                if (setclasshour == "")
                                                {
                                                    if (j == Convert.ToInt32(brk))
                                                    {
                                                        if (!hatcol.Contains("Lunch Break"))
                                                        {
                                                            int breaktme = Convert.ToInt32(brk) + 3;
                                                            dtable.Columns.Add("Lunch Break").SetOrdinal(j + 3);
                                                            hatcol.Add("Lunch Break", 0);

                                                            rowhead.Insert(breaktme, "Lunch Break");
                                                            rowperiod.Insert(breaktme, "Lunch Break");
                                                        }
                                                        dtrow[j + 3] = "Break";
                                                    }

                                                    try
                                                    {
                                                        delflag = 1;
                                                        setclasshour = daccess.GetFunction("select acronym from subject where subject_no='" + spiltclasshour[0].ToString() + "'");
                                                        dtrow[j + 2] = setclasshour;
                                                    }
                                                    catch (Exception)
                                                    {
                                                    }
                                                }
                                                else
                                                {
                                                    delflag = 1;
                                                    setclasshour = setclasshour + "/" + daccess.GetFunction("select acronym from subject where subject_no='" + spiltclasshour[0].ToString() + "'");
                                                    dtrow[j + 2] = setclasshour;
                                                }
                                            }
                                            else
                                            {
                                                if (setclasshour == "")
                                                {
                                                    try
                                                    {
                                                        delflag = 1;
                                                        setclasshour = daccess.GetFunction("select acronym from subject where subject_no='" + spiltclasshour[0].ToString() + "'");
                                                        dtrow[j + 3] = setclasshour;
                                                    }
                                                    catch (Exception) { }
                                                }
                                                else
                                                {
                                                        delflag = 1;
                                                        setclasshour = setclasshour + "/" + daccess.GetFunction("select acronym from subject where subject_no='" + spiltclasshour[0].ToString() + "'");
                                                        dtrow[j + 3] = setclasshour;
                                                    
                                                }
                                            }
                                            string sqlorder = "select schOrder from PeriodAttndSchedule where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + semester + "";
                                            DataSet dsorder = daccess.select_method(sqlorder, hat, "Text");
                                            dayorder = Convert.ToInt32(dsorder.Tables[0].Rows[0]["schOrder"]);


                                            if (dayorder == 1)
                                            {
                                                dtrow[1] = dayofweek1;
                                            }
                                            else
                                            {
                                                int date = day + 1;
                                                dtrow[1] = "Day " + date;
                                            }

                                            dtrow[2] = getroman(semester) + "/" + strsection.ToString();
                                            dtrow[0] = r.ToString();

                                            for (int cc = 0; cc < spiltclasshour.Length; cc++)
                                            {
                                                string va = Convert.ToString(spiltclasshour[cc]);
                                                if (!hatHr.ContainsKey(spiltclasshour[0].ToString() + "-" + va + "-" + strsection))
                                                {
                                                    hatHr.Add(spiltclasshour[0].ToString() + "-" + va + "-" + strsection, 1);
                                                }
                                                else
                                                {
                                                    int mark = Convert.ToInt32(hatHr[spiltclasshour[0].ToString() + "-" + va + "-" + strsection]);
                                                    mark = mark + 1;
                                                    hatHr[spiltclasshour[0].ToString() + "-" + va + "-" + strsection] = mark;
                                                }
                                            }
                                        }
                                    }
                                }
                                dtable.Rows.Add(dtrow);
                            }
                        }

                        if (hatHr.Count > 0)
                        {

                        }

                        if (day == noofdays - 1)
                        {
                            if (ds_load.Tables[0].Rows.Count > 0)
                            {
                                dtrow1 = dtable1.NewRow();

                                dtrow1[0] = "Batch : " + batchyear + " " + '-' + " Branch : " + ddlBranch.SelectedItem.ToString() + " - Sem : " + getroman(semester) + " " + '-' + " Section " + '-' + " " + strsection + " ";
                                if (forschoolsetting == true)
                                {
                                    dtrow1[0] = "Year : " + batchyear + " " + '-' + " Standard : " + ddlBranch.SelectedItem.ToString() + " - Term : " + getroman(semester) + " " + '-' + " Section " + '-' + " " + strsection + " ";
                                }
                                dtable1.Rows.Add(dtrow1);
                            }

                            string staff = "select distinct noofhrsperweek,s.acronym,s.subject_no,subject_code,subject_name,sm.staff_name,sm.staff_code,sam.dept_name from subject s,syllabus_master sy,staffmaster sm,staff_selector ss ,staff_appl_master sam where s.syll_code=sy.syll_code and sy.batch_year= " + batchyear + " and sy.degree_code=" + ddlBranch.SelectedValue.ToString() + " and sy.semester=" + semester + " " + sec + "  and sm.staff_code=ss.staff_code and ss.subject_no=s.subject_no and sam.appl_no=sm.appl_no order by s.subject_no";   //modified by Mullai
                            DataSet desub = daccess.select_method(staff, hat, "Text");


                            if (desub.Tables[0].Rows.Count > 0)
                            {
                                int srno = 0;
                                string temp = "";
                                // string temp1 = "";
                                for (int s = 0; s < desub.Tables[0].Rows.Count; s++)
                                {
                                    dtrow1 = dtable1.NewRow();
                                    string SubjectNo = desub.Tables[0].Rows[s]["subject_no"].ToString();//staff_code
                                    string staffC = desub.Tables[0].Rows[s]["staff_code"].ToString();
                                    string hours = desub.Tables[0].Rows[s]["noofhrsperweek"].ToString();
                                    string Subcode = desub.Tables[0].Rows[s]["subject_code"].ToString();
                                    string subname = desub.Tables[0].Rows[s]["subject_name"].ToString();
                                    string staffname = desub.Tables[0].Rows[s]["staff_name"].ToString();
                                    string acronym = desub.Tables[0].Rows[s]["acronym"].ToString();
                                    string deptname = desub.Tables[0].Rows[s]["dept_name"].ToString();
                                    if (temp != Subcode) //modified by Mullai
                                    {
                                        srno++;
                                        temp = Subcode;
                                    }

                                    int totHr = Convert.ToInt32(hatHr[SubjectNo + "-" + staffC + "-" + strsection]);

                                    dtrow1[0] = srno.ToString();
                                    dtrow1[1] = Subcode.ToString();
                                    dtrow1[2] = subname + "(" + acronym + ")";
                                    dtrow1[3] = totHr.ToString();
                                    dtrow1[4] = staffname.ToString();
                                    dtrow1[5] = deptname.ToString();

                                    dtable1.Rows.Add(dtrow1);
                                }
                                gviewstaff.DataSource = dtable1;
                                gviewstaff.DataBind();
                                RowHead1(gviewstaff);
                                gviewstaff.Visible = true;
                            }
                        }
                    }
                }
                else
                {
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnprintmaster.Visible = false;
                    btnxl.Visible = false;

                    gview.Visible = false;
                    gviewstaff.Visible = false;
                    lblerr.Text = "No Records Found";
                    flag = true;
                    lblerr.Visible = true;

                    btnxcl.Visible = false;
                    Lblreport.Visible = false;
                    txtexcl.Visible = false;
                    btnprnt.Visible = false;
                }

                if (delflag == 0)
                {
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnprintmaster.Visible = false;
                    btnxl.Visible = false;

                    gviewstaff.Visible = false;
                    gview.Visible = false;
                    lblerr.Text = "No Records Found";
                    flag = true;
                    lblerr.Visible = true;

                    btnxcl.Visible = false;
                    Lblreport.Visible = false;
                    txtexcl.Visible = false;
                    btnprnt.Visible = false;

                }
            }
            gview.DataSource = dtable;
            gview.DataBind();
            gview.Visible = true;
            if (flag == true)
            {
                gview.Visible = false;
                gviewstaff.Visible = false;
            }

            RowHead(gview);
            RowHeadSpan(gview);
        }
        catch (Exception ex) { dac.sendErrorMail(ex, userCollegeCode, "Master Time Table"); }
    }

    protected void RowHead1(GridView gview)
    {
        for (int head = 0; head < 1; head++)
        {
            gview.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            gview.Rows[head].Font.Bold = true;
            gview.Rows[head].HorizontalAlign = HorizontalAlign.Center;
            gview.Rows[head].Font.Name = "Book Antique";

            for (int cell = 0; cell < gview.Rows[head].Cells.Count; cell++)
            {
                gview.Rows[head].Cells[cell].Height = 50;
            }
        }
    }

    protected void RowHead(GridView gview)
    {
        for (int head = 0; head < 2; head++)
        {
            gview.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            gview.Rows[head].Font.Bold = true;
            gview.Rows[head].HorizontalAlign = HorizontalAlign.Center;
            gview.Rows[head].Font.Name = "Book Antique";
        }
    }

    protected void RowHeadSpan(GridView gview)
    {
        for (int row = 2; row > 0; row--)
        {
            GridViewRow roww = gview.Rows[row];
            GridViewRow previousRow = gview.Rows[row - 1];
            for (int cell = 0; cell < gview.Rows[row].Cells.Count; cell++)
            {
                if (gview.HeaderRow.Cells[cell].Text.Trim() == "Semester")
                {
                    if (roww.Cells[cell].Text == previousRow.Cells[cell].Text)
                    {
                        if (previousRow.Cells[cell].RowSpan == 0)
                        {
                            if (roww.Cells[cell].RowSpan == 0)
                            {
                                previousRow.Cells[cell].RowSpan += 2;
                            }
                            else
                            {
                                previousRow.Cells[cell].RowSpan = roww.Cells[cell].RowSpan + 1;
                            }
                            roww.Cells[cell].Visible = false;
                        }
                    }
                }
            }
        }
    }

    protected void gview_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //GridView HeaderGrid = (GridView)sender;
            //GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            //TableCell headerCell = new TableCell();

            //Table table = (Table)gview.Controls[0];
            //TableRow headerRow = table.Rows[0];
            //int numberOfHeaderCellsToMove = headerRow.Cells.Count - 1;

            //for (int i = 0; i < 3; i++)
            //{
            //    headerCell = headerRow.Cells[0];
            //    HeaderGridRow.Cells.Add(headerCell);
            //    headerCell.RowSpan = 2;
            //}
            //gview.Controls[0].Controls.AddAt(0, HeaderGridRow);

            //TableHeaderCell HeaderCell = new TableHeaderCell();

            //HeaderCell = new TableHeaderCell();
            //HeaderCell.Text = hperiod[1].ToString();
            //HeaderCell.ColumnSpan = 1;
            //HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            //HeaderGridRow.Cells.Add(HeaderCell);
            //gview.Controls[0].Controls.AddAt(0, HeaderGridRow);

            //HeaderCell = new TableHeaderCell();
            //HeaderCell.Text = hperiod[2].ToString();
            //HeaderCell.ColumnSpan = 1;
            //HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            //HeaderGridRow.Cells.Add(HeaderCell);
            //gview.Controls[0].Controls.AddAt(0, HeaderGridRow);

            //HeaderCell = new TableHeaderCell();
            //HeaderCell.Text = hperiod[3].ToString();
            //HeaderCell.ColumnSpan = 1;
            //HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            //HeaderGridRow.Cells.Add(HeaderCell);
            //gview.Controls[0].Controls.AddAt(0, HeaderGridRow);

            //HeaderCell = new TableHeaderCell();
            //HeaderCell.Text = hperiod[4].ToString();
            //HeaderCell.ColumnSpan = 1;
            //HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            //HeaderGridRow.Cells.Add(HeaderCell);
            //gview.Controls[0].Controls.AddAt(0, HeaderGridRow);

            //int brek = Convert.ToInt32(ViewState["break"]);
            //for (int i = brek; i <= brek; i++)
            //{
            //    headerCell = headerRow.Cells[brek];
            //    HeaderGridRow.Cells.Add(headerCell);
            //    headerCell.RowSpan = 2;
            //}
            //gview.Controls[0].Controls.AddAt(0, HeaderGridRow);

            //HeaderCell = new TableHeaderCell();
            //HeaderCell.Text = hperiod[5].ToString();
            //HeaderCell.ColumnSpan = 1;
            //HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            //HeaderGridRow.Cells.Add(HeaderCell);
            //gview.Controls[0].Controls.AddAt(0, HeaderGridRow);

            //HeaderCell = new TableHeaderCell();
            //HeaderCell.Text = hperiod[6].ToString();
            //HeaderCell.ColumnSpan = 1;
            //HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            //HeaderGridRow.Cells.Add(HeaderCell);
            //gview.Controls[0].Controls.AddAt(0, HeaderGridRow);

            //HeaderCell = new TableHeaderCell();
            //HeaderCell.Text = hperiod[7].ToString();
            //HeaderCell.ColumnSpan = 1;
            //HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            //HeaderGridRow.Cells.Add(HeaderCell);
            //gview.Controls[0].Controls.AddAt(0, HeaderGridRow);

            //HeaderCell = new TableHeaderCell();
            //HeaderCell.Text = hperiod[8].ToString();
            //HeaderCell.ColumnSpan = 1;
            //HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            //HeaderGridRow.Cells.Add(HeaderCell);
            //gview.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int c = rowhead.Count;
            int c1 = rowperiod.Count;

            if (e.Row.RowIndex == 0)
            {
                for (int cell = 0; cell < rowperiod.Count; cell++)
                {
                    e.Row.Cells[cell].Text = Convert.ToString(rowperiod[cell]);
                }
            }
            if (e.Row.RowIndex == 1)
            {
                for (int cell = 0; cell < rowperiod.Count; cell++)
                {
                    e.Row.Cells[cell].Text = Convert.ToString(rowhead[cell]);
                }
            }
        }
    }

    protected void gview_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            for (int row = gview.Rows.Count - 1; row > 0; row--)
            {
                GridViewRow roww = gview.Rows[row];
                GridViewRow previousRow = gview.Rows[row - 1];
                for (int cell = 0; cell < roww.Cells.Count; cell++)
                {
                    if (dtable.Columns[cell].ColumnName.Trim() == "S.No" || dtable.Columns[cell].ColumnName.Trim() == "Lunch Break" || dtable.Columns[cell].ColumnName.Trim() == "Day/Timings")
                    {
                        if (roww.Cells[cell].Text == previousRow.Cells[cell].Text)
                        {
                            if (previousRow.Cells[cell].RowSpan == 0)
                            {
                                if (roww.Cells[cell].RowSpan == 0)
                                {
                                    previousRow.Cells[cell].RowSpan += 2;
                                }
                                else
                                {
                                    previousRow.Cells[cell].RowSpan = roww.Cells[cell].RowSpan + 1;
                                }
                                roww.Cells[cell].Visible = false;
                            }
                        }
                    }
                }
            }
            for (int row = 0; row < gview.Rows.Count; row++)
            {
                int cnt = 0;
                for (int cell = 0; cell < gview.Rows[row].Cells.Count; cell++)
                {
                    if (dtable.Columns[cell].ColumnName.Trim() != "Day/Timings")
                    {
                        gview.Rows[row].Cells[cell].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
                for (int cell = gview.Rows[row].Cells.Count - 1; cell > 0; cell--)
                {
                    TableCell col = gview.Rows[row].Cells[cell];
                    TableCell previouscol = gview.Rows[row].Cells[cell - 1];
                    if (dtable.Columns[cell].ColumnName.Trim() != "S.No" && dtable.Columns[cell].ColumnName.Trim() != "Day/Timings" && dtable.Columns[cell].ColumnName.Trim() != "Semester")
                    {

                        if (col.Text == previouscol.Text)
                        {
                            if (previouscol.ColumnSpan == 0)
                            {
                                if (col.ColumnSpan == 0)
                                {
                                    previouscol.ColumnSpan += 2;
                                    cnt++;
                                }
                                else
                                {
                                    previouscol.ColumnSpan += col.ColumnSpan + 1;
                                    cnt++;
                                }
                                col.Visible = false;

                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex) { dac.sendErrorMail(ex, userCollegeCode, "Master Time Table"); }
    }

    protected void gviewstaff_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Width = 30;
                e.Row.Cells[1].Width = 100;
                e.Row.Cells[2].Width = 350;
                e.Row.Cells[3].Width = 30;
                e.Row.Cells[4].Width = 200;
                e.Row.Cells[5].Width = 200;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[0].Text.Contains("Batch"))
                {
                    e.Row.Cells[0].ColumnSpan = 6;
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[0].ForeColor = Color.Blue;
                    e.Row.Cells[0].BorderColor = Color.Black;
                    e.Row.Cells[0].Font.Bold = true;
                    e.Row.Cells[0].Font.Size = FontUnit.Large;
                    e.Row.Cells[0].Font.Name = "Book Antiqua";

                    for (int i = 1; i < e.Row.Cells.Count; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        catch (Exception ex) { dac.sendErrorMail(ex, userCollegeCode, "Master Time Table"); }
    }

    protected void gviewstaff_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            for (int i = gviewstaff.Rows.Count - 1; i > 1; i--)
            {
                GridViewRow row = gviewstaff.Rows[i];
                GridViewRow previousRow = gviewstaff.Rows[i - 1];
                string pre = previousRow.Cells[0].Text;
                string cur = row.Cells[0].Text;
                for (int j = 0; j < row.Cells.Count; j++)
                {
                    if (row.Cells[j].Text == previousRow.Cells[j].Text)
                    {
                        if (pre == cur)
                        {
                            if (gviewstaff.HeaderRow.Cells[j].Text == "TOTAL HOURS")
                            {
                                if (row.Cells[j].Text == previousRow.Cells[j].Text)
                                {
                                    if (previousRow.Cells[j].RowSpan == 0)
                                    {
                                        if (row.Cells[j].RowSpan == 0)
                                        {
                                            previousRow.Cells[j].RowSpan += 2;
                                        }
                                        else
                                        {
                                            previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                                        }
                                        row.Cells[j].Visible = false;
                                    }
                                }
                            }
                        }
                        if (gviewstaff.HeaderRow.Cells[j].Text != "TOTAL HOURS")
                        {
                            if (previousRow.Cells[j].RowSpan == 0)
                            {
                                if (row.Cells[j].RowSpan == 0)
                                {
                                    previousRow.Cells[j].RowSpan += 2;
                                }
                                else
                                {
                                    previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                                }
                                row.Cells[j].Visible = false;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex) { dac.sendErrorMail(ex, userCollegeCode, "Master Time Table"); }
    }

    public string getroman(string n)
    {
        string roman = "";
        switch (n)
        {
            case "1":
                roman = "I";
                break;
            case "2":
                roman = "II";
                break;
            case "3":
                roman = "III";
                break;
            case "4":
                roman = "IV";
                break;
            case "5":
                roman = "V";
                break;
            case "6":
                roman = "VI";
                break;
            case "7":
                roman = "VII";
                break;
            case "8":
                roman = "VIII";
                break;
        }
        return roman;

    }

    protected void btnxcl_click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcl.Text;

            if (reportname.ToString().Trim() != "")
            {
                daccess.printexcelreportgrid(gview, reportname);
            }
            else
            {
                lblexer.Text = "Please Enter Your Report Name";
                lblexer.Visible = true;
            }
        }
        catch (Exception ex) { dac.sendErrorMail(ex, userCollegeCode, "Master Time Table"); }
    }

    //public override void VerifyRenderingInServerForm(Control control)
    //{ }


    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;

            if (reportname.ToString().Trim() != "")
            {
                daccess.printexcelreportgrid(gviewstaff, reportname);
            }
            else
            {
                lblexer.Text = "Please Enter Your Report Name";
                lblexer.Visible = true;
            }
        }
        catch (Exception ex) { dac.sendErrorMail(ex, userCollegeCode, "Master Time Table"); }
    }

    //public override void VerifyRenderingInServerForm(Control control)
    //{ }

    protected void btnprnt_Click(object sender, EventArgs e)
    {
        string degreedetails = "" + ddlBatch.SelectedValue.ToString() + '-' + ddlDegree.SelectedItem.ToString() + '-' + ddlBranch.SelectedItem.ToString();
        string pagename = "MasterTimeTable.aspx";

        string ss = null;
        NEWPrintMater1.loadspreaddetails(gview, pagename, degreedetails, 0, ss);
        NEWPrintMater1.Visible = true;

    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string degreedetails = "" + ddlBatch.SelectedValue.ToString() + '-' + ddlDegree.SelectedItem.ToString() + '-' + ddlBranch.SelectedItem.ToString();
        string pagename = "MasterTimeTable.aspx";

        string ss = null;
        NEWPrintMater2.loadspreaddetails(gviewstaff, pagename, degreedetails, 0, ss);
        NEWPrintMater2.Visible = true;

    }

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        gview.Visible = false;
        Lblreport.Visible = false;
        txtexcl.Visible = false;
        btnxcl.Visible = false;
        btnprnt.Visible = false;
        gviewstaff.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;

        ddlBranch.Items.Clear();

        string course_id = ddlDegree.SelectedValue.ToString();
        BindBranch(singleuser, group_user, course_id, collegecode, usercode);

    }

    protected void ddlclg_SelectedIndexChanged(object sender, EventArgs e)
    {
        gview.Visible = false;
        Lblreport.Visible = false;
        txtexcl.Visible = false;
        btnxcl.Visible = false;
        btnprnt.Visible = false;
        gviewstaff.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        gview.Visible = false;
        Lblreport.Visible = false;
        txtexcl.Visible = false;
        btnxcl.Visible = false;
        btnprnt.Visible = false;
        gviewstaff.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        gview.Visible = false;
        Lblreport.Visible = false;
        txtexcl.Visible = false;
        btnxcl.Visible = false;
        btnprnt.Visible = false;
        gviewstaff.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
    }
}