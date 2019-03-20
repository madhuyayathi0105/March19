using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;


public partial class AttendanceMOD_Inspro_Calendar_Report : System.Web.UI.Page
{
    SqlConnection con2a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection dar_con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con3a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con4a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getcona = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con5a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con6a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con8 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection tempcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlCommand cmd8 = new SqlCommand();
    SqlCommand cmda;
    SqlCommand cmd1a;
    SqlCommand cmd3a;
    SqlCommand cmd4a;
    SqlCommand cmd6a;
    SqlCommand cmd;
    DAccess2 d2 = new DAccess2();
    static Boolean forschoolsetting = false;
    Hashtable hat = new Hashtable();
    Hashtable has = new Hashtable();
    DataSet ds_load = new DataSet();
    DAccess2 daccess = new DAccess2();
    string collegecode = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";
    string strbatch = string.Empty;
    string strbatchyear = string.Empty;
    string strbranch = string.Empty;
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
                txt_startdate.Text = DateTime.Now.ToString("dd/MM/yyy");
                txt_enddate.Text = DateTime.Now.ToString("dd/MM/yyy");
                //Lblreport.Visible = false;
                //txtexcl.Visible = false;
                //btnprnt.Visible = false;
                //btnxcl.Visible = false;
                //lblexer.Visible = false;
                colg();
                bindbatchs();
                binddegrees();
                bindbranchs();
                bindsem();
                bindsec();
                binddate();

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
            // lblexer.Visible = false;
        }
        catch
        {
        }
    }
    public void bindbatchs()
    {
        ////batch
        try
        {
            string userCode = string.Empty;
            string groupUserCode = string.Empty;
            string qryUserOrGroupCode = string.Empty;
            DataSet dsBatch = new DataSet();
            ddl_Bat.Items.Clear();

            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                if (group.Contains(";"))
                {
                    string[] group_semi = group.Split(';');
                    groupUserCode = Convert.ToString(group_semi[0]);
                }
                else
                {
                    groupUserCode = group;
                }
                if (!string.IsNullOrEmpty(groupUserCode))
                {
                    qryUserOrGroupCode = " and user_id='" + groupUserCode + "'";
                }
            }
            else if (Session["usercode"] != null)
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                if (!string.IsNullOrEmpty(userCode))
                {
                    qryUserOrGroupCode = " and user_id='" + userCode + "'";
                }
            }
            dsBatch.Clear();
            if (!string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                string qry = "select distinct batch_year from tbl_attendance_rights r where batch_year<>'' " + qryUserOrGroupCode + " order by batch_year desc";
                dsBatch = d2.select_method_wo_parameter(qry, "Text");
            }
            string qryBatch = string.Empty;
            if (dsBatch.Tables.Count > 0 && dsBatch.Tables[0].Rows.Count > 0)
            {
                List<int> lstBatch = dsBatch.Tables[0].AsEnumerable().Select(r => r.Field<int>("batch_year")).ToList();
                if (lstBatch.Count > 0)
                    qryBatch = " and r.Batch_Year in('" + string.Join("','", lstBatch.ToArray()) + "')";
            }

            string sqlstring = string.Empty;
            int max_bat = 0;
            con.Close();
            con.Open();
            // cmd = new SqlCommand(" select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year", con);
            cmd = new SqlCommand("select distinct r.Batch_Year from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=r.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and r.degree_code=dg.Degree_Code  and r.Batch_Year<>'0' and r.Batch_Year<>-1 and r.cc='0' and delflag='0' and exam_flag<>'debar' " + qryBatch + " order by r.Batch_Year desc", con);
            SqlDataAdapter da1 = new SqlDataAdapter(cmd);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1);
            ddl_Bat.DataSource = ds1;
            ddl_Bat.DataValueField = "batch_year";
            ddl_Bat.DataBind();

            con.Close();
        }
        catch
        {
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

        string group_code = Session["group_code"].ToString();
        string columnfield = "";
        if (group_code.Contains(';'))
        {
            string[] group_semi = group_code.Split(';');
            group_code = group_semi[0].ToString();
        }
        if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
        {
            columnfield = " and group_code='" + group_code + "'";
        }
        else
        {
            columnfield = " and user_code='" + Session["usercode"] + "'";
        }
        hat.Clear();
        hat.Add("column_field", columnfield.ToString());
        ds_load = d2.select_method("bind_college", hat, "sp");
        ddlclg.Items.Clear();
        string clgname = "select college_code,collname from collinfo ";
        // if (clgname != "")
        // {
        // ds_load = daccess.select_method(clgname, hat, "Text");
        if (ds_load.Tables[0].Rows.Count > 0)
        {
            ddlclg.DataSource = ds_load;
            ddlclg.DataTextField = "collname";
            ddlclg.DataValueField = "college_code";
            ddlclg.DataBind();
        }

    }

    public void binddegrees()
    {
        ////degree
        try
        {
            hat.Clear();
            ddl_degree.Items.Clear();
            ddl_degree.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Convert.ToString(ddlclg.SelectedValue);
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Clear();
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            DataSet ds = d2.select_method("bind_degree", hat, "sp");
            ddl_degree.DataSource = ds;
            ddl_degree.DataValueField = "course_id";
            ddl_degree.DataTextField = "course_name";
            ddl_degree.DataBind();
            //bindbranch();
        }
        catch
        {
        }
    }

    public void bindsem()
    {
        //--------------------semester load
        try
        {
            ddlduration.Items.Clear();
            if (ddl_degree.SelectedValue.ToString() != "" && ddl_degree.SelectedValue.ToString() != null)
            {
                Boolean first_year;
                first_year = false;
                int duration = 0;
                int i = 0;
                con.Close();
                con.Open();
                SqlDataReader dr;
                cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddl_Bat.Text.ToString() + " and college_code=" + ddlclg.SelectedValue + "", con);
                dr = cmd.ExecuteReader();
                dr.Read();

                if (dr.HasRows == true)
                {
                    string sqlcurrentsem = "select distinct current_semester from Registration where Batch_Year='" + ddl_Bat.Text.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' and college_code='" + ddlclg.SelectedValue + "'";
                    int currentsem = 0;
                    Int32.TryParse(d2.GetFunction(sqlcurrentsem).Trim(), out currentsem);  //added by prabha on feb 22 2018
                    first_year = Convert.ToBoolean(dr[1].ToString());
                    duration = Convert.ToInt32(dr[0].ToString());
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlduration.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlduration.Items.Add(i.ToString());
                        }
                        if (i == currentsem)
                        {
                            ddlduration.SelectedIndex = i - 1;
                        }
                    }
                }
                else
                {
                    dr.Close();
                    SqlDataReader dr1;
                    cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and college_code=" + ddlclg.SelectedValue + "", con);
                    string sqlcurrentsem = "select distinct current_semester from Registration where Batch_Year='" + ddl_Bat.Text.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' and college_code='" + ddlclg.SelectedValue + "'";
                    int currentsem = 0;
                    Int32.TryParse(d2.GetFunction(sqlcurrentsem).Trim(), out currentsem);   //added by prabha on feb 22 2018
                    ddlduration.Items.Clear();
                    dr1 = cmd.ExecuteReader();
                    dr1.Read();
                    if (dr1.HasRows == true)
                    {
                        first_year = Convert.ToBoolean(dr1[1].ToString());
                        duration = Convert.ToInt32(dr1[0].ToString());
                        for (i = 1; i <= duration; i++)
                        {
                            if (first_year == false)
                            {
                                ddlduration.Items.Add(i.ToString());
                            }
                            else if (first_year == true && i != 2)
                            {
                                ddlduration.Items.Add(i.ToString());
                            }
                            if (i == currentsem)
                            {
                                ddlduration.SelectedIndex = i - 1;
                            }
                        }
                    }
                    dr1.Close();
                }
                con.Close();
            }

        }
        catch
        {
        }
    }
    public void binddate()
    {
        try
        {





            //string startdate = d2.GetFunction("Select convert(nvarchar(15),start_date,103) as date from seminfo where batch_year in('" + Lbbatch.Text + "') and degree_code in('" + typ1 + "' ) and semester in('" + lbsem.Text + " ') order by start_date desc ");

            //string enddate = d2.GetFunction("Select convert(nvarchar(15),end_date,103) as date from seminfo where batch_year in('" + Lbbatch.Text + "') and degree_code in('" + typ1 + " ') and semester in('" + lbsem.Text + " ')order by end_date desc ");
            if (ddlduration.SelectedValue != "" && ddlbranch.SelectedValue != "" && ddl_Bat.SelectedValue != "")
            {
                string sql = "select top 1 s.batch_year , Dept_Name,batch_year,d.degree_code,CONVERT(nvarchar(15),start_date,103) as start_date,CONVERT(datetime,start_date,103) as st_date,CONVERT(datetime,end_date,103) as en_date,CONVERT(varchar,end_date,103) as end_date,s.semester,no_of_working_Days,no_of_working_hrs,schOrder,starting_dayorder,nodays  from Degree d,Department de,course c,seminfo s,PeriodAttndSchedule p where s.degree_code=p.degree_code and s.semester=p.semester and p.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code=de.college_code and s.degree_code in('" + ddlbranch.SelectedValue + "')  and batch_year in('" + ddl_Bat.SelectedValue + "')   and s.semester in('" + ddlduration.SelectedValue + "')  and c.college_code='" + Convert.ToString(ddlclg.SelectedValue) + "' order by   c.college_code,s.batch_year asc ,s.semester desc,d.Degree_Code,datepart(year,start_date) desc , datepart(month ,start_date) desc";

                DataSet dsattabsent = daccess.select_method_wo_parameter(sql, "text");
                if (dsattabsent.Tables.Count > 0 && dsattabsent.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsattabsent.Tables[0].Rows.Count; i++)
                    {
                        string batch_year = Convert.ToString(dsattabsent.Tables[0].Rows[i]["batch_year"]);
                        string degree_code = Convert.ToString(dsattabsent.Tables[0].Rows[i]["degree_code"]);
                        string semester = Convert.ToString(dsattabsent.Tables[0].Rows[i]["semester"]);
                        string start_dayorder = Convert.ToString(dsattabsent.Tables[0].Rows[i]["starting_dayorder"]);
                        string nodays = Convert.ToString(dsattabsent.Tables[0].Rows[i]["nodays"]);
                        string sch = Convert.ToString(dsattabsent.Tables[0].Rows[0]["schOrder"]);
                        string st_date = Convert.ToString(dsattabsent.Tables[0].Rows[0]["start_date"]);
                        DateTime semst_date = Convert.ToDateTime(dsattabsent.Tables[0].Rows[0]["st_date"]);
                        string en_date = Convert.ToString(dsattabsent.Tables[0].Rows[0]["end_date"]);

                        txt_startdate.Text = Convert.ToString(st_date);
                        txt_enddate.Text = Convert.ToString(en_date);


                    }
                }

                else
                {
                    txt_startdate.Text = DateTime.Now.ToString("dd/MM/yyy");
                    txt_enddate.Text = DateTime.Now.ToString("dd/MM/yyy");
                }
            }
            else
            {
                txt_startdate.Text = DateTime.Now.ToString("dd/MM/yyy");
                txt_enddate.Text = DateTime.Now.ToString("dd/MM/yyy");
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void bindsec()
    {
        //----------load section
        try
        {
            ddlsec.Items.Clear();
            if (ddlbranch.SelectedValue.ToString() != "" && ddlbranch.SelectedValue.ToString() != null)
            {
                con.Close();
                con.Open();
                cmd = new SqlCommand("select distinct sections from registration where batch_year=" + ddl_Bat.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                ddlsec.DataSource = ds;
                ddlsec.DataTextField = "sections";
                ddlsec.DataBind();
                SqlDataReader dr_sec;
                dr_sec = cmd.ExecuteReader();
                dr_sec.Read();
                if (dr_sec.HasRows == true)
                {
                    if (dr_sec["sections"].ToString() == string.Empty)
                    {
                        ddlsec.Enabled = false;
                    }
                    else
                    {
                        ddlsec.Enabled = true;
                    }
                }
                else
                {
                    ddlsec.Enabled = false;
                }

                con.Close();
            }

        }
        catch
        {
        }
    }

    public void bindbranchs()
    {
        //--------load degree
        try
        {
            ddlbranch.Items.Clear();

            ddlbranch.Items.Clear();
            hat.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Convert.ToString(ddlclg.SelectedValue);
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddl_degree.SelectedValue);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            DataSet ds = d2.select_method("bind_branch", hat, "sp");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch
        {
        }
    }

    protected void ddlclg_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddegrees();
        bindbranchs();
        bindsem();
        binddate();

    }

    protected void ddl_Bat_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranchs();
            bindsem();
            bindsec();
        }
        catch
        {
        }
    }

    protected void ddl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {



            bindbranchs();
            bindsem();
            bindsec();
            binddate();
        }
        catch
        {
        }
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // errmsg.Visible = false;

            //  chkForAlternateStaff.Visible = false;



            bindsem();
            bindsec();
            binddate();
        }
        catch
        {
        }
    }

    protected void ddlduration_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsec();
            binddate();
        }
        catch
        {
        }
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch
        {
        }
    }
    public void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            gview.Visible = false;
            btndirectPrint.Visible = false;
            Hashtable headrow = new Hashtable();
            Hashtable headyear = new Hashtable();
            Hashtable headholi = new Hashtable();
            DataTable dtyear = new DataTable();
            DataRow dyear;
            dtyear.Columns.Add("Date01");
            dtyear.Columns.Add("Day01");
            dtyear.Columns.Add("Holiday01");
            dtyear.Columns.Add("Holi_day01");
            dtyear.Columns.Add("Date02");
            dtyear.Columns.Add("Day02");
            dtyear.Columns.Add("Holiday02");
            dtyear.Columns.Add("Holi_day02");
            dtyear.Columns.Add("Date03");
            dtyear.Columns.Add("Day03");
            dtyear.Columns.Add("Holiday03");
            dtyear.Columns.Add("Holi_day03");
            dtyear.Columns.Add("Date04");
            dtyear.Columns.Add("Day04");
            dtyear.Columns.Add("Holiday04");
           dtyear.Columns.Add("Holi_day04");
            dtyear.Columns.Add("Date05");
            dtyear.Columns.Add("Day05");
            dtyear.Columns.Add("Holiday05");
            dtyear.Columns.Add("Holi_day05");
            dtyear.Columns.Add("Date06");
            dtyear.Columns.Add("Day06");
            dtyear.Columns.Add("Holiday06");
            dtyear.Columns.Add("Holi_day06");
            dtyear.Columns.Add("Date07");
            dtyear.Columns.Add("Day07");
            dtyear.Columns.Add("Holiday07");
            dtyear.Columns.Add("Holi_day07");
            dtyear.Columns.Add("Date08");
            dtyear.Columns.Add("Day08");
            dtyear.Columns.Add("Holiday08");
            dtyear.Columns.Add("Holi_day08");
            dtyear.Columns.Add("Date09");
            dtyear.Columns.Add("Day09");
            dtyear.Columns.Add("Holiday09");
            dtyear.Columns.Add("Holi_day09");
            dtyear.Columns.Add("Date10");
            dtyear.Columns.Add("Day10");
            dtyear.Columns.Add("Holiday10");
            dtyear.Columns.Add("Holi_day10");
            dtyear.Columns.Add("Date11");
            dtyear.Columns.Add("Day11");
            dtyear.Columns.Add("Holiday11");
            dtyear.Columns.Add("Holi_day11");
            dtyear.Columns.Add("Date12");
            dtyear.Columns.Add("Day12");
            dtyear.Columns.Add("Holiday12");
            //dtyear.Columns.Add("Holi_day12");
          //  dtyear.Columns.Add("January");
            //dtyear.Columns.Add("January");
            //dtyear.Columns.Add("January");
            //dtyear.Columns.Add("January");
            //dtyear.Columns.Add("February");
            //dtyear.Columns.Add("February");
            //dtyear.Columns.Add("February");
            //dtyear.Columns.Add("February");
            //dtyear.Columns.Add("March");
            //dtyear.Columns.Add("March");
            //dtyear.Columns.Add("March");
            //dtyear.Columns.Add("Holi_day03");
            //dtyear.Columns.Add("Date04");
            //dtyear.Columns.Add("Day04");
            //dtyear.Columns.Add("Holiday04");
            //dtyear.Columns.Add("Holi_day04");
            //dtyear.Columns.Add("Date05");
            //dtyear.Columns.Add("Day05");
            //dtyear.Columns.Add("Holiday05");
            //dtyear.Columns.Add("Holi_day05");
            //dtyear.Columns.Add("Date06");
            //dtyear.Columns.Add("Day06");
            //dtyear.Columns.Add("Holiday06");
            //dtyear.Columns.Add("Holi_day06");
            //dtyear.Columns.Add("Date07");
            //dtyear.Columns.Add("Day07");
            //dtyear.Columns.Add("Holiday07");
            //dtyear.Columns.Add("Holi_day07");
            //dtyear.Columns.Add("Date08");
            //dtyear.Columns.Add("Day08");
            //dtyear.Columns.Add("Holiday08");
            //dtyear.Columns.Add("Holi_day08");
            //dtyear.Columns.Add("Date09");
            //dtyear.Columns.Add("Day09");
            //dtyear.Columns.Add("Holiday09");
            //dtyear.Columns.Add("Holi_day09");
            //dtyear.Columns.Add("Date10");
            //dtyear.Columns.Add("Day10");
            //dtyear.Columns.Add("Holiday10");
            //dtyear.Columns.Add("Holi_day10");
            //dtyear.Columns.Add("Date11");
            //dtyear.Columns.Add("Day11");
            //dtyear.Columns.Add("Holiday11");
            //dtyear.Columns.Add("Holi_day11");
            //dtyear.Columns.Add("Date12");
            //dtyear.Columns.Add("Day12");
            //dtyear.Columns.Add("Holiday12");
            //dtyear.Columns.Add("Holi_day12");
            dyear = dtyear.NewRow();
            dyear["Date01"] = "January";
            dyear["Day01"] = "January";
            dyear["Holiday01"] = "January";
            //dyear["Holi_day01"] = "January";

            dyear["Date02"] = "February";
            dyear["Day02"] = "February";
            dyear["Holiday02"] = "February";
            //dyear["Holi_day02"] = "February";

            dyear["Date03"] = "March";
            dyear["Day03"] = "March";
            dyear["Holiday03"] = "March";
            //dyear["Holi_day03"] = "March";
            dyear["Date04"] = "April";
            dyear["Day04"] = "April";
            dyear["Holiday04"] = "April";
            //dyear["Holi_day04"] = "April";
            dyear["Date05"] = "May";
            dyear["Day05"] = "May";
            dyear["Holiday05"] = "May";
            //dyear["Holi_day05"] = "May";

            dyear["Date06"] = "June";
            dyear["Day06"] = "June";
            dyear["Holiday06"] = "June";
            //dyear["Holi_day06"] = "June";

            dyear["Date07"] = "July";
            dyear["Day07"] = "July";
            dyear["Holiday07"] = "July";
            //dyear["Holi_day07"] = "July";

            dyear["Date08"] = "August";
            dyear["Day08"] = "August";
            dyear["Holiday08"] = "August";
            //dyear["Holi_day08"] = "August";

            dyear["Date09"] = "September";
            dyear["Day09"] = "September";
            dyear["Holiday09"] = "September";
            //dyear["Holi_day09"] = "September";

            dyear["Date10"] = "October";
            dyear["Day10"] = "October";
            dyear["Holiday10"] = "October";
            //dyear["Holi_day10"] = "October";

            dyear["Date11"] = "November";
            dyear["Day11"] = "November";
            dyear["Holiday11"] = "November";
            //dyear["Holi_day11"] = "November";

            dyear["Date12"] = "December";
            dyear["Day12"] = "December";
            dyear["Holiday12"] = "December";
            //dyear["Holi_day12"] = "December";
            //dyear["Date1"] = "Date1";
            //dyear["Day1"] = "Day1";
            //dyear["Holiday1"] = "Holiday1";
            //dyear["April"] = "April";
            //dyear["May"] = "May";
            //dyear["Jun"] = "Jun";
            //dyear["July"] = "July";
            //dyear["Aug"] = "Aug";
            //dyear["Sep"] = "Sep";
            //dyear["Oct"] = "Oct";
            //dyear["Nov"] = "Nov";
            //dyear["Dec"] = "Dec";
            dtyear.Rows.Add(dyear);

            string quer = string.Empty;
            //dyear = dtyear.NewRow();
            //dyear["Date01"] = "Date";
            //dyear["Day01"] = "Day";
            //dyear["Holiday01"] = "Day Order";
            ////dyear["Holi_day01"] = "";

            //dyear["Date02"] = "Date";
            //dyear["Day02"] = "Day";
            //dyear["Holiday02"] = "Day Order";
            ////dyear["Holi_day02"] = "";

            //dyear["Date03"] = "Date";
            //dyear["Day03"] = "Day";
            //dyear["Holiday03"] = "Day Order";
            ////dyear["Holi_day03"] = "";
            //dyear["Date04"] = "Date";
            //dyear["Day04"] = "Day";
            //dyear["Holiday04"] = "Day Order";
            ////dyear["Holi_day04"] = "";
            //dyear["Date05"] = "Date";
            //dyear["Day05"] = "Day";
            //dyear["Holiday05"] = "Day Order";
            ////dyear["Holi_day05"] = "";

            //dyear["Date06"] = "Date";
            //dyear["Day06"] = "Day";
            //dyear["Holiday06"] = "Day Order";
            ////dyear["Holi_day06"] = "";

            //dyear["Date07"] = "Date";
            //dyear["Day07"] = "Day";
            //dyear["Holiday07"] = "Day Order";
            ////dyear["Holi_day07"] = "";

            //dyear["Date08"] = "Date";
            //dyear["Day08"] = "August";
            //dyear["Holiday08"] = "Day Order";
            ////dyear["Holi_day08"] = "";

            //dyear["Date09"] = "Date";
            //dyear["Day09"] = "Day";
            //dyear["Holiday09"] = "Day Order";
            ////dyear["Holi_day09"] = "";

            //dyear["Date10"] = "Date";
            //dyear["Day10"] = "Day";
            //dyear["Holiday10"] = "Day Order";
            ////dyear["Holi_day10"] = "";

            //dyear["Date11"] = "Date";
            //dyear["Day11"] = "Day";
            //dyear["Holiday11"] = "Day Order";
            ////dyear["Holi_day11"] = "";

            //dyear["Date12"] = "Date";
            //dyear["Day12"] = "Day";
            //dyear["Holiday12"] = "Day Order";
            ////dyear["Holi_day12"] = "";
            //dtyear.Rows.Add(dyear);
            //dyear = dtyear.NewRow();
            //dtyear.Rows.Add(dyear);
            int datre = 0;
            //if (ddlsem.SelectedIndex==0)
            //    quer = "Order by textval asc";
            //else
            //    quer = "Order by textval desc";
            //string sem = d2.GetFunction("select textval from FT_ACADEMICYEAR_DETAILED dt,textvaltable tv where ACD_YEAR='" + Convert.ToString(ddlBatch.SelectedValue) + "' and TextCode=ACD_FEECATEGORY and college_code='" + ddlclg.SelectedValue + "' and  TextCriteria='FEECA' " + quer + " ");
            //string[] semester=sem.Split();
            //if (semester.Length >1)
            //    sem = semester[0];
            string sql = "  select top 1 s.batch_year , Dept_Name,batch_year,d.degree_code,CONVERT(varchar,start_date,103) as start_date,CONVERT(datetime,start_date,103) as st_date,CONVERT(datetime,end_date,103) as en_date,CONVERT(varchar,end_date,103) as end_date,s.semester,no_of_working_Days,no_of_working_hrs,schOrder,starting_dayorder,nodays  from Degree d,Department de,course c,seminfo s,PeriodAttndSchedule p where s.degree_code=p.degree_code and s.semester=p.semester and p.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code=de.college_code and s.degree_code in('" + ddlbranch.SelectedValue + "')  and batch_year in('" + ddl_Bat.SelectedValue + "')   and s.semester in('" + ddlduration.SelectedValue + "') and c.college_code='" + Convert.ToString(ddlclg.SelectedValue) + "' order by   c.college_code,s.batch_year asc ,s.semester desc,d.Degree_Code,datepart(year,start_date) desc , datepart(month ,start_date) desc";

            DataSet dsattabsent = daccess.select_method_wo_parameter(sql, "text");
            if (dsattabsent.Tables.Count > 0 && dsattabsent.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsattabsent.Tables[0].Rows.Count; i++)
                {
                    string batch_year = Convert.ToString(dsattabsent.Tables[0].Rows[i]["batch_year"]);
                    string degree_code = Convert.ToString(dsattabsent.Tables[0].Rows[i]["degree_code"]);
                    string semester = Convert.ToString(dsattabsent.Tables[0].Rows[i]["semester"]);
                    string start_dayorder = Convert.ToString(dsattabsent.Tables[0].Rows[i]["starting_dayorder"]);
                    string nodays = Convert.ToString(dsattabsent.Tables[0].Rows[i]["nodays"]);
                    string sch = Convert.ToString(dsattabsent.Tables[0].Rows[0]["schOrder"]);
                    DateTime st_date = Convert.ToDateTime(dsattabsent.Tables[0].Rows[0]["st_date"]);
                    DateTime semst_date = Convert.ToDateTime(dsattabsent.Tables[0].Rows[0]["st_date"]);
                    DateTime en_date = Convert.ToDateTime(dsattabsent.Tables[0].Rows[0]["en_date"]);
                    int s = 0;
                    string[] sps = st_date.ToString().Split('/');
                    string semdate = sps[0] + '/' + sps[1] + '/' + sps[2];
                    string Day_Order = string.Empty;
                    string double_dayorder = string.Empty;
                    headrow.Clear();
                    headyear.Clear();
                    headholi.Clear();
                    int headsn = 0;
                    int holidays = 0;
                    string strdayoredr = "Select * from tbl_consider_day_order where degree_code=" + degree_code + " and semester=" + semester + " and batch_year=" + batch_year + "  and ((From_Date between '" + st_date.ToString("yyyy-MM-dd") + "' and '" + en_date.ToString("yyyy-MM-dd") + "') or (To_Date between '" + st_date.ToString("yyyy-MM-dd") + "' and '" + en_date.ToString("yyyy-MM-dd") + "'))";
                    DataSet dsdayorder = d2.select_method_wo_parameter(strdayoredr, "Text");
                    Hashtable hatdoc = new Hashtable();
                    Dictionary<DateTime, byte> dicAlternateDayOrder = new Dictionary<DateTime, byte>();
                    for (int doc = 0; doc < dsdayorder.Tables[0].Rows.Count; doc++)
                    {
                        DateTime dtsdoc = Convert.ToDateTime(dsdayorder.Tables[0].Rows[doc]["from_date"].ToString());
                        DateTime dtedoc = Convert.ToDateTime(dsdayorder.Tables[0].Rows[doc]["to_date"].ToString());
                        string reason = dsdayorder.Tables[0].Rows[doc]["Reason"].ToString();
                        string alternateDayOrder = Convert.ToString(dsdayorder.Tables[0].Rows[doc]["DayOrder"]).Trim();
                        byte alternateDay = 0;

                        byte.TryParse(alternateDayOrder, out alternateDay);
                        for (DateTime dtc = dtsdoc; dtc <= dtedoc; dtc = dtc.AddDays(1))
                        {
                            if (!hatdoc.Contains(dtc))
                            {
                                hatdoc.Add(dtc, reason);
                            }
                            if (!dicAlternateDayOrder.ContainsKey(dtc))
                            {
                                dicAlternateDayOrder.Add(dtc, alternateDay);
                            }
                        }
                    }
                    string mons = st_date.ToString("MM");
                    st_date = st_date.AddDays(-1);
                    Boolean doubleday1 = false;
                    string startmons = string.Empty;
                    string nextmon = string.Empty;
                    string months = st_date.ToString("MM");
                    string coldt = string.Empty;
                    int daycun = 0;
                    int stcol = 0;
                    if (mons != months)
                        st_date = st_date.AddDays(1);
                    while (st_date <= en_date)
                    {

                        string mon = st_date.ToString("MM");

                        string munth = st_date.ToString("M");
                        string year = st_date.ToString("yyyy");
                        string date = st_date.ToString("dd");
                        string day = st_date.ToString("ddd");
                        string weekday = st_date.ToString("dddd");
                        int col = 0;
                        int.TryParse(mon, out col);

                        if (!headrow.ContainsValue(col))
                        {
                            headsn++;
                            headrow.Add(headsn, col);
                            headyear.Add(headsn, year);
                        }
                        int row = 0;
                        int.TryParse(date, out row);
                        if (sch == "1")
                        {
                            if (s == 0)
                            {
                                int.TryParse(date, out datre);

                                for (int j = 1; j <= datre; j++)
                                {
                                    startmons = mon;
                                    if (stcol == 0)
                                    {
                                        stcol++;
                                        if (stcol <= 9)
                                            coldt = "0" + stcol;
                                        else
                                            coldt = Convert.ToString(stcol);
                                    }
                                    string[] sp = st_date.ToString().Split('/');
                                    string curdate = mon + '/' + j + '/' + sp[2];
                                    DateTime starday = Convert.ToDateTime(curdate);
                                    string day1 = starday.ToString("ddd");

                                    Day_Order = "0-" + Convert.ToString(day1);
                                    if (dtyear.Rows.Count <= j + 1)
                                    {
                                        dyear = dtyear.NewRow();
                                        dtyear.Rows.Add(dyear);
                                    }
                                    dtyear.Rows[j + 1]["date" + coldt + ""] = j;
                                    dtyear.Rows[j + 1]["day" + coldt + ""] = day1;
                                    dtyear.Rows[j + 1]["Holiday" + coldt + ""] = "";
                                    //dtyear.Rows[j + 1]["Holi_day" + coldt + ""] = "";
                                }
                                s = 1;
                            }
                            else
                            {
                                if (startmons != mon)
                                {
                                    startmons = mon;
                                    stcol++;
                                    if (stcol <= 9)
                                        coldt = "0" + stcol;
                                    else
                                        coldt = Convert.ToString(stcol);
                                }
                                if (dtyear.Rows.Count <row)
                                {
                                    dyear = dtyear.NewRow();
                                    dtyear.Rows.Add(dyear);
                                }
                                dtyear.Rows[row - 1]["date" + coldt + ""] = date;
                                dtyear.Rows[row - 1]["day" + coldt + ""] = day;

                                //dyear["date" + mon + ""] = date;
                                //dyear["day" + mon + ""] = day;
                                string holi = d2.GetFunction("select holiday_desc from holidaystudents where degree_code in('" + degree_code + "')   and semester=" + semester + " and holiday_date='" + st_date + "'");
                                if (holi != "" && holi != "0")
                                {
                                    holidays++;
                                    dtyear.Rows[row - 1]["Holiday" + coldt + ""] = holi;
                                    //dtyear.Rows[row + 1]["Holi_day" + coldt + ""] = st_date;
                                    headholi.Add(holidays, row + 1 + "-" + coldt + "");
                                }
                                else
                                {
                                    dtyear.Rows[row - 1]["Holiday" + coldt + ""] = weekday;
                                    //dtyear.Rows[row + 1]["Holi_day" + coldt + ""] = st_date;

                                }
                                if (st_date == en_date)
                                {
                                    st_date = st_date.AddDays(1);
                                    date = st_date.ToString("dd");
                                    int.TryParse(date, out datre);
                                    string monthyear = st_date.ToString("MM");
                                    if (mon == monthyear)
                                    {
                                        for (int j = datre; j <= 30; j++)
                                        {
                                            if (startmons != mon)
                                            {
                                                startmons = mon;
                                                stcol++;
                                                if (stcol <= 9)
                                                    coldt = "0" + stcol;
                                                else
                                                    coldt = Convert.ToString(stcol);
                                            }
                                            date = Convert.ToString(j);
                                            string[] sp = st_date.ToString().Split('/');
                                            string curdate = mon + '/' + j + '/' + sp[2];
                                            DateTime starday = Convert.ToDateTime(curdate);
                                            string mon1 = starday.ToString("MM");
                                            string munth1 = starday.ToString("M");
                                            string date1 = starday.ToString("dd");
                                            string day1 = starday.ToString("ddd");
                                            if (dtyear.Rows.Count < j)
                                            {
                                                dyear = dtyear.NewRow();
                                                dtyear.Rows.Add(dyear);
                                            }
                                            //dyear["date" + mon + ""] = j;
                                            //dyear["day" + mon + ""] = day1;
                                            //dyear["Holiday" + mon + ""] = "";
                                            dtyear.Rows[j - 1]["date" + coldt + ""] = j;
                                            dtyear.Rows[j- 1]["day" + coldt + ""] = day1;
                                            dtyear.Rows[j - 1]["Holiday" + coldt + ""] = "";
                                            //dtyear.Rows.Add(dyear);
                                            //dtyear.Rows[row + 1]["Holi_day" + coldt + ""] = "";
                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            if (s == 0)
                            {
                                int.TryParse(date, out datre);

                                for (int j = 1; j <= datre; j++)
                                {
                                    if (startmons != mon)
                                    {
                                        startmons = mon;
                                        stcol++;
                                        if (stcol <= 9)
                                        {
                                            coldt = "0" + stcol;
                                            //int.TryParse(coldt, out stcol);
                                            //stcol = Convert.ToInt32("0" + stcol);
                                        }
                                        else
                                            coldt = Convert.ToString(stcol);
                                    }
                                    string[] sp = st_date.ToString().Split('/');
                                    string curdate = mon + '/' + j + '/' + sp[2];
                                    DateTime starday = Convert.ToDateTime(curdate);
                                    string mon1 = starday.ToString("MM");
                                    string munth1 = starday.ToString("M");
                                    string date1 = starday.ToString("dd");
                                    string day1 = starday.ToString("ddd");
                                    if (dtyear.Rows.Count <= j)
                                    {
                                        dyear = dtyear.NewRow();
                                        dtyear.Rows.Add(dyear);
                                    }
                                    //dyear["date" + mon + ""] = j;
                                    //dyear["day" + mon + ""] = day1;
                                    //dyear["Holiday" + mon + ""] = "";
                                    dtyear.Rows[j]["date" + coldt + ""] = j;
                                    dtyear.Rows[j ]["day" + coldt + ""] = day1;
                                    dtyear.Rows[j ]["Holiday" + coldt + ""] = "";
                                    //dtyear.Rows[j + 1]["Holi_day" + coldt + ""] = "";
                                    //dtyear.Rows.Add(dyear);
                                }
                                s = 1;
                            }
                            else
                            {
                                if (startmons != mon)
                                {
                                    startmons = mon;
                                    stcol++;
                                    if (stcol <= 9)
                                        coldt = "0" + stcol;
                                    else
                                        coldt = Convert.ToString(stcol);
                                }
                                if (dtyear.Rows.Count <= row)
                                {
                                    dyear = dtyear.NewRow();
                                    dtyear.Rows.Add(dyear);
                                }
                                dtyear.Rows[row ]["date" + coldt + ""] = date;
                                dtyear.Rows[row]["day" + coldt + ""] = day;

                                //dyear["date" + mon + ""] = date;
                                //dyear["day" + mon + ""] = day;
                                string holi = d2.GetFunction("select holiday_desc from holidaystudents where degree_code in('" + degree_code + "')   and semester=" + semester + " and holiday_date='" + st_date + "'");
                                if (holi != "" && holi != "0")
                                {
                                    holidays++;
                                    dtyear.Rows[row ]["Holiday" + coldt + ""] = holi;
                                    //dtyear.Rows[row+1 ]["Holi_day" + coldt + ""] = st_date;
                                    headholi.Add(holidays, row  + "-" + coldt + "");
                                }
                                // dyear["Holiday" + mon + ""] = holi;
                                else
                                {
                                    daycun++;
                                    DateTime st_semdate = st_date;
                                    if (doubleday1 == true)
                                        st_semdate = st_date.AddDays(+1);
                                    string[] sp = st_semdate.ToString().Split('/');
                                    string curdate = sp[0] + '/' + sp[1] + '/' + sp[2];
                                    Session["SchOrderdouble"] = sch;
                                    string strday = d2.findday(curdate, degree_code, semester, batch_year, semdate, nodays, start_dayorder);
                                    if (doubleday1 == true)
                                        st_semdate = st_semdate.AddDays(-1);
                                    if (dicAlternateDayOrder.ContainsKey(st_semdate))
                                    {
                                        strday = findDayName(dicAlternateDayOrder[st_date]);
                                        Day_Order = Convert.ToString(dicAlternateDayOrder[st_date]).Trim();
                                        Day_Order = "Day" + Day_Order;
                                        if (Day_Order == "Day0")
                                            Day_Order = Convert.ToString(hatdoc[st_date]).Trim();


                                    }
                                    else
                                    {
                                        if (strday.Trim().ToLower() == "mon")
                                            Day_Order = "Day1";
                                        else if (strday.Trim().ToLower() == "tue")
                                            Day_Order = "Day2";
                                        else if (strday.Trim().ToLower() == "wed")
                                            Day_Order = "Day3";
                                        else if (strday.Trim().ToLower() == "thu")
                                            Day_Order = "Day4";
                                        else if (strday.Trim().ToLower() == "fri")
                                            Day_Order = "Day5";
                                        else if (strday.Trim().ToLower() == "sat")
                                            Day_Order = "Day6";
                                        else if (strday.Trim().ToLower() == "sun")
                                            Day_Order = "Day7";
                                    }
                                    if (doubleday1 == true)
                                        Day_Order = double_dayorder + ',' + Day_Order;
                                    if (sch == "0")
                                    {
                                        string chkdoubleday = d2.GetFunction("select * from doubledayorder where doubleDate='" + curdate + "' and batchYear='" + batch_year + "' and degreecode='" + degree_code + "'");
                                        if (chkdoubleday != "" && chkdoubleday != "0")
                                        {
                                            if (doubleday1 == false)
                                            {
                                                doubleday1 = true;
                                                Session["doubledayshk"] = "true";
                                                double_dayorder = Day_Order;
                                                st_date = st_date.AddDays(-1);
                                                daycun--;
                                            }
                                            else
                                            {
                                                doubleday1 = false;
                                                Session["doubledayshk"] = "false";

                                            }
                                        }
                                        else
                                        {
                                            doubleday1 = false;
                                            Session["doubledayshk"] = "false";
                                        }
                                    }


                                    //dyear["Holiday" + mon + ""] = holi;
                                    dtyear.Rows[row ]["Holiday" + coldt + ""] = Day_Order + '/' + daycun;
                                    //dtyear.Rows[row + 1]["Holi_day" + coldt + ""] = st_date;
                                }
                                // dtyear.Rows.Add(dyear);
                                if (st_date == en_date)
                                {

                                    st_date = st_date.AddDays(1);
                                    date = st_date.ToString("dd");
                                    int.TryParse(date, out datre);
                                    string monthyear = st_date.ToString("MM");
                                    if (mon == monthyear)
                                    {
                                        for (int j = datre; j <= 30; j++)
                                        {
                                            if (startmons != mon)
                                            {
                                                startmons = mon;
                                                stcol++;
                                                if (stcol <= 9)
                                                    coldt = "0" + stcol;
                                                else
                                                    coldt = Convert.ToString(stcol);
                                            }
                                            date = Convert.ToString(j);
                                            string[] sp = st_date.ToString().Split('/');
                                            string curdate = mon + '/' + j + '/' + sp[2];
                                            DateTime starday = Convert.ToDateTime(curdate);
                                            string mon1 = starday.ToString("MM");
                                            string munth1 = starday.ToString("M");
                                            string date1 = starday.ToString("dd");
                                            string day1 = starday.ToString("ddd");
                                            if (dtyear.Rows.Count <= j)
                                            {
                                                dyear = dtyear.NewRow();
                                                dtyear.Rows.Add(dyear);
                                            }
                                            //dyear["date" + mon + ""] = j;
                                            //dyear["day" + mon + ""] = day1;
                                            //dyear["Holiday" + mon + ""] = "";
                                            dtyear.Rows[j ]["date" + coldt + ""] = j;
                                            dtyear.Rows[j ]["day" + coldt + ""] = day1;
                                            dtyear.Rows[j ]["Holiday" + coldt + ""] = "";
                                            //dtyear.Rows.Add(dyear);
                                            //dtyear.Rows[j + 1]["Holi_day" + coldt + ""] = "";
                                        }
                                    }

                                }
                            }
                        }
                        //gview.Columns[(stcol * 3) - 3].Visible = true;
                        //gview.Columns[((stcol * 3) - 3) + 1].Visible = true;
                        //gview.Columns[((stcol * 3) - 3) + 2].Visible = true;
                        st_date = st_date.AddDays(1);

                    }

                }
            }

           
            if (dtyear.Rows.Count > 1)
            {
               

                gview.DataSource = dtyear;
                gview.DataBind();
                gview.Visible = true;
                btndirectPrint.Visible = true;


 


                //if (dtyear.Rows.Count > 0)
                //{
                //    for (int ms = 0; ms < dtyear.Rows.Count; ms++)
                //    {
                //        for (int s = 0; s < dtyear.Columns.Count; s++)
                //        {
                //            if((s+1)%4==0)
                //            gview.Rows[ms].Cells[s].Visible = false;
                //        }
                //    }
                //}
                if (dtyear.Rows.Count > 0)
                {
                    for (int ms = 0; ms < dtyear.Rows.Count; ms++)
                    {
                        for (int s = 0; s < dtyear.Columns.Count; s++)
                        {
                            gview.Rows[ms].Cells[s].Visible = false;
                        }
                    }
                }
                for (int ms = 1; ms < dtyear.Rows.Count; ms++)
                {
                    for (int s = 0; s < gview.Rows[0].Cells.Count - 1; s++)
                    {
                        if ((s ) % 4 == 1)
                        {
                            gview.Rows[ms].Cells[s].BackColor = Color.Aqua;
                        }
                        else if ((s) % 4 == 0)
                        {
                            gview.Rows[ms].Cells[s].BackColor = Color.Pink;
                        }



                    }
                }

                if (headrow.Count > 0)
                {
                    for (int head = 0; head < headrow.Count; head++)
                    {
                        int val = Convert.ToInt32(headrow[head + 1]);
                        string years = Convert.ToString(headyear[head + 1]);
                        //  gview.Rows[0].Cells[(head * 4)].ColumnSpan = 4;
                        //gview.Rows[0].Cells[(head * 4)].Visible = true;
                        //gview.Rows[0].Cells[((head * 4)) + 1].Visible = false;
                        //gview.Rows[0].Cells[((head * 4)) + 2].Visible = false;
                        //gview.Rows[0].Cells[((head * 4)) + 3].Visible = false;
                        string year = string.Empty;
                        if (val == 1)
                            year = "January";
                        if (val == 2)
                            year = "February";
                        if (val == 3)
                            year = "March";
                        if (val == 4)
                            year = "April";
                        if (val == 5)
                            year = "May";
                        if (val == 6)
                            year = "June";
                        if (val == 7)
                            year = "July";
                        if (val == 8)
                            year = "August";
                        if (val == 9)
                            year = "September";
                        if (val == 10)
                            year = "October";
                        if (val == 11)
                            year = "November";
                        if (val == 12)
                            year = "December";
                        gview.Rows[0].Cells[(head * 4)].Text = year + '-' + years;
                        gview.Rows[0].Cells[((head * 4)) + 1].Text = year + '-' + years;
                        gview.Rows[0].Cells[((head * 4)) + 2].Text = year + '-' + years;
                        // gview.Rows[0].Cells[((head * 3)) + 3].Text = year + '-' + years;
                        if (gview.Rows.Count > 0)
                        {
                            for (int ms = 0; ms < gview.Rows.Count; ms++)
                            {
                                gview.Rows[0].Cells[(head * 4)].Visible = true;
                                gview.Rows[ms].Cells[(head * 4)].Visible = true;
                                gview.Rows[ms].Cells[((head * 4)) + 1].Visible = true;
                                gview.Rows[ms].Cells[((head * 4)) + 2].Visible = true;
                               //  gview.Rows[ms].Cells[((head * 3)) + 3].Visible = true;
                                    gview.Rows[ms].Cells[((head * 4)) + 3].Visible = false;

                            }
                        }

                    }
                }




                for (int cell = gview.Rows[0].Cells.Count - 1; cell > 0; cell--)
                {
                    TableCell colum = gview.Rows[0].Cells[cell];
                    TableCell previouscol = gview.Rows[0].Cells[cell - 1];
                    if (colum.Text == previouscol.Text)
                    {
                        if (previouscol.ColumnSpan == 0)
                        {
                            if (colum.ColumnSpan == 0)
                            {
                                previouscol.ColumnSpan += 2;

                            }
                            else
                            {
                                previouscol.ColumnSpan += colum.ColumnSpan + 1;

                            }
                            colum.Visible = false;

                        }
                    }

                }

               
                if (headholi.Count > 0)
                {
                    for (int head = 0; head < headholi.Count; head++)
                    {
                        string val = Convert.ToString(headholi[head + 1]);
                        string[] spl = val.Split('-');
                        int row = Convert.ToInt32(spl[0]);
                        int col = Convert.ToInt32(spl[1]);

                        gview.Rows[row].Cells[((col * 4) - 4) + 2].BackColor = Color.DarkSeaGreen;
                        //gview.HeaderRow.Cells[((val * 3) - 3) + 1].Visible = false;
                        //gview.HeaderRow.Cells[((val * 3) - 3) + 2].Visible = false;

                    }
                }


                //Div4.Visible = true;
                RowHead(gview);
                
            
               
               
            }
           
        }
        catch
        {
        }
    }
    private string findDayName(byte dayOrder)
    {
        string dayName = string.Empty;
        switch (dayOrder)
        {
            case 0:
                dayName = string.Empty;
                break;
            case 1:
                dayName = "Mon";
                break;
            case 2:
                dayName = "Tue";
                break;
            case 3:
                dayName = "Wed";
                break;
            case 4:
                dayName = "Thu";
                break;
            case 5:
                dayName = "Fri";
                break;
            case 6:
                dayName = "Sat";
                break;
            case 7:
                dayName = "Sun";
                break;
            default:
                break;
        }
        return dayName;
    }
    protected void RowHead(GridView gview)
    {
        for (int head = 0; head < 1; head++)
        {
            gview.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            gview.Rows[head].Font.Bold = true;
            gview.Rows[head].HorizontalAlign = HorizontalAlign.Center;
            gview.Rows[head].Font.Name = "Book Antique";
        }
    }

    #region Print
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        //Printcontrol.loadspreaddetails(FpSpread1, "ConsiderDayOrder.aspx", "Consider Day Order Report");
        //Printcontrol.Visible = true;
        string ss = Convert.ToString(Session["usercode"]);
       string degreedetails = "Inspro Calendar Report" + '@' + " Branch   : " + ddlbranch.SelectedItem.Text + "";
       NEWPrintMater1.loadspreaddetails(gview, "Inspro Calendar Report.aspx", degreedetails, 0, ss);
        NEWPrintMater1.Visible = true;
    }
    #endregion

}
