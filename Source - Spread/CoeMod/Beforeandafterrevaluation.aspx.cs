using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using BalAccess;

public partial class Beforeandafterrevaluation : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection newcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    string collegecode = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";
    Hashtable hat = new Hashtable();
    DataSet ds_load = new DataSet();
    DAccess2 daccess = new DAccess2();
    SqlCommand cmd;
    string degree_code = "";
    string current_sem = "";
    string batch_year = "";
    string getgradeflag = "";
    string exam_month = "";
    string exam_year = "";
    int ExamCode = 0;
    Boolean InsFlag;
    double passpercent3 = 0;
    string section = "";
    string yr_val = "";
    string yr_string = "";
    double passpercent1 = 0;
    double passpercent4 = 0;
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
                load_college();
                bindbatch();

                binddegree();
                bindbranch();
                bindsem();
                bindsec();
                bindexammonth();
                bindexamyear();
                FpExternal.CommandBar.Visible = false;
                FpExternal.Visible = false;
                btnExcel.Visible = false;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                btnprintmaster.Visible = false;
            }
        }
        catch(Exception ex)
        {
        }

    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }
    public void bindsem()
    {

        //--------------------semester load
        ddlSemYr.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        con.Close();
        con.Open();
        SqlDataReader dr;
        cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlBranch.Text.ToString() + " and batch_year=" + ddlBatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
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
                    ddlSemYr.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {
                    ddlSemYr.Items.Add(i.ToString());
                }

            }
        }
        else
        {
            dr.Close();
            SqlDataReader dr1;
            cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlBranch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
            //     ddlSemYr.Items.Clear();
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
                        ddlSemYr.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSemYr.Items.Add(i.ToString());
                    }
                }
            }

            dr1.Close();
        }
        //    ddlSemYr.Items.Insert(0, new ListItem("--Select--", "-1"));
        con.Close();
    }
    public void bindexammonth()
    {
        ddlMonth.Items.Clear();


        SqlDataReader drexamyear;
        con.Close();
        con.Open();
        ddlYear.Items.Clear();
        string yearquery = "select distinct exam_month from exam_details where batch_year='" + ddlBatch.SelectedItem.Text + "' and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and current_semester='" + ddlSemYr.SelectedItem.Text + "'";
        SqlCommand cmdyearquery = new SqlCommand(yearquery, con);
        drexamyear = cmdyearquery.ExecuteReader();
        while (drexamyear.Read())
        {

            int exammonth = Convert.ToInt16(drexamyear["exam_month"].ToString());
            string monthtext = bindmonthname(exammonth);

            ddlMonth.Items.Add(new System.Web.UI.WebControls.ListItem(monthtext.ToString(), exammonth.ToString()));
        }
        ddlMonth.Items.Add("Select");

    }
    public string bindmonthname(int mon)
    {
        int value = mon;
        string textvalue = "";
        switch (value)
        {
            case 1:
                textvalue = "Jan";
                break;

            case 2:
                textvalue = "Feb";
                break;

            case 3:
                textvalue = "Mar";
                break;

            case 4:
                textvalue = "Apr";
                break;

            case 5:
                textvalue = "May";
                break;

            case 6:
                textvalue = "Jun";
                break;

            case 7:
                textvalue = "Jul";
                break;

            case 8:
                textvalue = "Aug";
                break;

            case 9:
                textvalue = "Sep";
                break;

            case 10:
                textvalue = "Oct";
                break;

            case 11:
                textvalue = "Nov";
                break;

            case 12:
                textvalue = "Dec";
                break;

        }
        return textvalue;
    }
    public void bindexamyear()
    {
        ddlYear.Items.Clear();
        SqlDataReader drexamyear;
        con.Close();
        con.Open();
        ddlYear.Items.Clear();
        string yearquery = "select distinct exam_year from exam_details where batch_year='" + ddlBatch.SelectedItem.Text + "' and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and current_semester='" + ddlSemYr.SelectedItem.Text + "'";
        SqlCommand cmdyearquery = new SqlCommand(yearquery, con);
        drexamyear = cmdyearquery.ExecuteReader();
        while (drexamyear.Read())
        {
            ddlYear.Items.Add(drexamyear["exam_year"].ToString());
        }
        ddlYear.Items.Add("Select");


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
            //ddlYear.DataSource = ds_load;
            //ddlYear.DataTextField = "batch_year";
            //ddlYear.DataValueField = "batch_year";
            //ddlYear.DataBind();
        }
        int count1 = ds_load.Tables[1].Rows.Count;
        if (count > 0)
        {
            int max_bat = 0;
            max_bat = Convert.ToInt32(ds_load.Tables[1].Rows[0][0].ToString());
            ddlBatch.SelectedValue = max_bat.ToString();
            con.Close();
        }
    }
    void load_college()
    {
        con.Open();
        SqlDataAdapter da_college = new SqlDataAdapter("select distinct collname,college_code from collinfo", con);
        DataTable dt_college = new DataTable();
        da_college.Fill(dt_college);

        ddl_college.DataSource = dt_college;
        ddl_college.DataTextField = "collname";
        ddl_college.DataValueField = "college_code";
        ddl_college.DataBind();
        con.Close();

    }
    public void bindbranch()
    {
        ddlBranch.Items.Clear();
        hat.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
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
        if (group_user.Contains(';'))
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
    public void bindsec()
    {
        ddlSec.Items.Clear();
        hat.Clear();
        hat.Add("batch_year", ddlBatch.SelectedValue.ToString());
        hat.Add("degree_code", ddlBranch.SelectedValue);
        ds_load = daccess.select_method("bind_sec", hat, "sp");
        int count5 = ds_load.Tables[0].Rows.Count;
        if (count5 > 0)
        {
            ddlSec.DataSource = ds_load;
            ddlSec.DataTextField = "sections";
            ddlSec.DataValueField = "sections";
            ddlSec.DataBind();
            ddlSec.Enabled = true;
        }
        else
        {
            ddlSec.Enabled = false;
        }
        ddlSec.Items.Add("ALL");//@@@@@@ added on 29.06.12
    }
    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (!Page.IsPostBack == false)
        {
            ddlSec.Items.Clear();
        }

        bindsec();
        bindexammonth();
        bindexamyear();

    }
    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((ddlDegree.SelectedIndex != 0) && (ddlBranch.SelectedIndex != 0))
        {
            ddlSemYr.Items.Clear();
            Get_Semester();

        }
        bindexammonth();
        bindexamyear();
        ddlSec.SelectedIndex = -1;
    }
    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlBranch.Items.Clear();

        string course_id = ddlDegree.SelectedValue.ToString();

        collegecode = Session["collegecode"].ToString();
        usercode = Session["UserCode"].ToString();
        DataSet ds = ClsAttendanceAccess.GetBranchDetail(course_id.ToString(), collegecode.ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlBranch.DataSource = ds;
            ddlBranch.DataTextField = "Dept_Name";
            ddlBranch.DataValueField = "degree_code";
            ddlBranch.DataBind();


        }
        if ((ddlBranch.SelectedIndex != 0) || (ddlBranch.SelectedIndex > 0) || (ddlBranch.SelectedIndex == 0))
        {

            bindsem();

            bindsec();
            bindexammonth();
            bindexamyear();
        }
    }
    public void Get_Semester()
    {
        Boolean first_year;
        first_year = false;
        int duration = 0;
        string batch_calcode_degree;


        string batch = ddlBatch.SelectedValue.ToString();
        collegecode = Session["collegecode"].ToString();
        string degree = ddlBranch.SelectedValue.ToString();
        batch_calcode_degree = batch.ToString() + "/" + collegecode.ToString() + "/" + degree.ToString();

        DataSet ds = ClsAttendanceAccess.Getsemster_Detail(batch_calcode_degree.ToString());

        if (ds.Tables[0].Rows.Count > 0)
        {
            first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
            duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());

            for (int i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    ddlSemYr.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {
                    ddlSemYr.Items.Add(i.ToString());
                }

            }

        }
    }
    public void clear()
    {
        ddlSemYr.Items.Clear();
        ddlSec.Items.Clear();
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();

        try
        {
            if ((ddlBranch.SelectedIndex != 0) || (ddlBranch.SelectedIndex > 0) || (ddlBranch.SelectedIndex == 0))
            {

                bindsem();

                bindsec();
                bindexammonth();
                bindexamyear();
            }
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        //try
        //{

        string befvalstudentsappeared = "";
        string befvalpassedstudent = "";
        string aftvalstudentappeared = "";
        string aftvalpassedstudent = "";
        string beforevalpercentage = "";
        string aftervaluepercentage = "";
        string beforeoverall = "";
        string afteroverall = "";
        string beforoverallpercentage = "";
        string afteroverallpercentage = "";
        int sno = 0;
        bindspread();
        if (ddlSec.Enabled == false)
        {
            section = "";
        }
        else
        {
            if (ddlSec.SelectedItem.Text == "ALL")
            {
                section = "";
            }
            else
            {
                section = ddlSec.SelectedItem.Text;
            }
        }
        degree_code = ddlBranch.SelectedValue.ToString();
        yr_val = ddlSemYr.SelectedItem.ToString();
        getyear();
        current_sem = ddlSemYr.SelectedValue.ToString();
        batch_year = ddlBatch.SelectedValue.ToString();
        ExamCode = Get_UnivExamCode(Convert.ToInt32(degree_code), GetSemester_AsNumber(Convert.ToInt32(current_sem)), Convert.ToInt32(batch_year));
        exam_month = ddlMonth.SelectedValue.ToString();
        exam_year = ddlYear.SelectedItem.ToString();
        string strsubject = "";
        string getgradeflag = "";
        if (ExamCode != 0)
        {
            string grade = "select grade_flag from grademaster where degree_code=" + degree_code + " and batch_year='" + batch_year + "' and exam_month=" + exam_month + " and exam_year= " + exam_year + "";
            con.Close();
            con.Open();
            SqlDataReader drgrade;
            newcon.Close();
            newcon.Open();
            SqlCommand cmd_grade = new SqlCommand(grade, newcon);
            drgrade = cmd_grade.ExecuteReader();
            int gradecheckcount = 0;
            while (drgrade.Read())
            {

                getgradeflag = drgrade["grade_flag"].ToString();

                if (ddlSec.Enabled == false || ddlSec.SelectedItem.Text == "ALL")
                {
                    strsubject = "Select distinct st.staff_code,sm.staff_name, s.mintotal as mintot,s.min_int_marks as mimark, s.min_ext_marks as mxmark,s.maxtotal as maxtot,s.acronym as subacr,subject_name,subject_code as Subject_Code,mark_entry.subject_no as Subject_No,semester,subject_type as Subtype,credit_points from Mark_Entry,Subject s,sub_sem,syllabus_master,staff_selector st,staffmaster sm  where syllabus_master.syll_code=s.syll_code and Mark_Entry.Subject_No = s..Subject_No and s.subtype_no= sub_sem.subtype_no and sm.staff_code=st.staff_code  and Exam_Code = '" + ExamCode + "' and attempts=1 and st.subject_no=s.subject_no order by semester desc,subject_type desc, mark_entry.subject_no asc";
                }

                else
                {
                    strsubject = "Select distinct st.staff_code,sm.staff_name, s.mintotal as mintot,s.maxtotal as maxtot,s.min_int_marks as mimark, s.min_ext_marks as mxmark,s.acronym as subacr,subject_name,subject_code as Subject_Code,mark_entry.subject_no as Subject_No,semester,subject_type as Subtype,credit_points from Mark_Entry,Subject s,sub_sem,syllabus_master,staff_selector st,staffmaster sm,registration r  where syllabus_master.syll_code=s.syll_code and Mark_Entry.Subject_No = s.Subject_No and s.subtype_no= sub_sem.subtype_no and sm.staff_code=st.staff_code  and Exam_Code = '" + ExamCode + "' and mark_entry.attempts=1 and st.subject_no=s.subject_no and st.sections=r.sections and r.sections='" + ddlSec.SelectedItem.Text + "' and r.batch_year='" + ddlBatch.SelectedItem.Text + "' and r.degree_code='" + ddlBranch.SelectedValue.ToString() + "' order by semester desc,subject_type desc, mark_entry.subject_no asc";
                }
                con.Close();
                con.Open();
                SqlCommand cmd_loadSub = new SqlCommand(strsubject, con);
                SqlDataReader dr_loadSub;
                dr_loadSub = cmd_loadSub.ExecuteReader();
                
                while (dr_loadSub.Read())
                {
                    gradecheckcount++;
                    if (Convert.ToInt32(getgradeflag) == 1)
                    {
                        sno++;
                        FpExternal.Sheets[0].RowCount = FpExternal.Sheets[0].RowCount + 1;
                        int rc = FpExternal.Sheets[0].RowCount - 1;

                        FpExternal.Sheets[0].Cells[rc, 0].Text = sno.ToString();
                        FpExternal.Sheets[0].Cells[rc, 0].Tag = dr_loadSub["mintot"].ToString();
                        FpExternal.Sheets[0].Cells[rc, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpExternal.Sheets[0].Cells[rc, 1].Text = dr_loadSub["Subject_Code"].ToString();
                        FpExternal.Sheets[0].Cells[rc, 1].Tag = dr_loadSub["Subject_No"].ToString();
                        FpExternal.Sheets[0].Cells[rc, 2].Text = dr_loadSub["Subject_name"].ToString();
                        FpExternal.Sheets[0].Cells[rc, 2].Tag = dr_loadSub["mimark"].ToString();
                        FpExternal.Sheets[0].Cells[rc, 3].Text = dr_loadSub["staff_name"].ToString();
                        FpExternal.Sheets[0].Cells[rc, 3].Tag = dr_loadSub["mxmark"].ToString();


                    }
                    if (Convert.ToInt32(getgradeflag) == 2)
                    {
                        sno++;
                        FpExternal.Sheets[0].RowCount = FpExternal.Sheets[0].RowCount + 1;
                        int rc = FpExternal.Sheets[0].RowCount - 1;

                        FpExternal.Sheets[0].Cells[rc, 0].Text = sno.ToString();
                        FpExternal.Sheets[0].Cells[rc, 0].Tag = dr_loadSub["mintot"].ToString();
                        FpExternal.Sheets[0].Cells[rc, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpExternal.Sheets[0].Cells[rc, 1].Text = dr_loadSub["Subject_Code"].ToString();
                        FpExternal.Sheets[0].Cells[rc, 1].Tag = dr_loadSub["Subject_No"].ToString();
                        FpExternal.Sheets[0].Cells[rc, 2].Text = dr_loadSub["Subject_name"].ToString();
                        FpExternal.Sheets[0].Cells[rc, 2].Tag = dr_loadSub["mimark"].ToString();
                        FpExternal.Sheets[0].Cells[rc, 3].Text = dr_loadSub["staff_name"].ToString();
                        FpExternal.Sheets[0].Cells[rc, 3].Tag = dr_loadSub["mxmark"].ToString();

                    }

                }
            }

            if (gradecheckcount != 0)
            {
                FpExternal.SaveChanges();
                for (int chk = 0; chk <= FpExternal.Sheets[0].RowCount - 1; chk++)
                {
                    befvalstudentsappeared = "";
                    befvalpassedstudent = "";
                    aftvalstudentappeared = "";
                    aftvalpassedstudent = "";
                    beforevalpercentage = "";
                    aftervaluepercentage = "";
                    beforeoverall = "";
                    afteroverall = "";
                    beforoverallpercentage = "";
                    afteroverallpercentage = "";
                    string subno=FpExternal.Sheets[0].Cells[chk, 1].Tag.ToString();
                    if (!string.IsNullOrEmpty(subno))
                    {
                    }
                    int subjectno = Convert.ToInt32(FpExternal.Sheets[0].Cells[chk, 1].Tag.ToString());
                    int gradeflag = Convert.ToInt32(getgradeflag);
                    double minintmark = Convert.ToInt32(FpExternal.Sheets[0].Cells[chk, 2].Tag.ToString());
                    double minextmark = Convert.ToInt32(FpExternal.Sheets[0].Cells[chk, 3].Tag.ToString());
                    double mintot = Convert.ToInt32(FpExternal.Sheets[0].Cells[chk, 0].Tag.ToString());
                    con.Close();
                    con.Open();
                    SqlCommand studinfo = new SqlCommand("spbeforeandafterrevaluation", con);
                    studinfo.CommandType = CommandType.StoredProcedure;
                    studinfo.Parameters.AddWithValue("@degreecode", degree_code);
                    studinfo.Parameters.AddWithValue("@batchyear", batch_year);
                    studinfo.Parameters.AddWithValue("@semester", current_sem);
                    studinfo.Parameters.AddWithValue("@subject_no", subjectno);
                    studinfo.Parameters.AddWithValue("@examcode", ExamCode);
                    studinfo.Parameters.AddWithValue("@Section", section);
                    studinfo.Parameters.AddWithValue("@gradeflag", gradeflag);
                    studinfo.Parameters.AddWithValue("@minintmark", minintmark);
                    studinfo.Parameters.AddWithValue("@minextmark", minextmark);
                    studinfo.Parameters.AddWithValue("@mintot", mintot - 1);
                    SqlDataAdapter studinfoada = new SqlDataAdapter(studinfo);
                    DataSet studinfoads = new DataSet();
                    studinfoada.Fill(studinfoads);
                    if (studinfoads.Tables[0].Rows.Count > 0)
                    {
                        for (int cnt = 0; cnt < studinfoads.Tables[0].Rows.Count; cnt++)
                        {
                            befvalstudentsappeared = studinfoads.Tables[0].Rows[cnt][0].ToString();
                            befvalpassedstudent = studinfoads.Tables[1].Rows[cnt][0].ToString();
                            aftvalstudentappeared = befvalstudentsappeared;
                            aftvalpassedstudent = studinfoads.Tables[2].Rows[cnt][0].ToString();
                            beforeoverall = studinfoads.Tables[3].Rows[cnt][0].ToString();
                            afteroverall = studinfoads.Tables[4].Rows[cnt][0].ToString();
                            if (befvalstudentsappeared != "0")
                            {
                                passpercent1 = Convert.ToDouble((Convert.ToDouble(befvalpassedstudent) / Convert.ToDouble(befvalstudentsappeared)) * 100);
                                double passpercent2 = Math.Round(passpercent1, 2);
                                beforevalpercentage = Convert.ToString(passpercent2);
                            }

                            if (aftvalstudentappeared != "0")
                            {
                                double passpercent1 = 0;
                                passpercent1 = Convert.ToDouble((Convert.ToDouble(aftvalpassedstudent) / Convert.ToDouble(aftvalstudentappeared)) * 100);
                                double passpercent2 = Math.Round(passpercent1, 2);
                                aftervaluepercentage = Convert.ToString(passpercent2);
                            }

                            if (beforeoverall != "0")
                            {
                                double passpercent1 = 0;
                                passpercent1 = Convert.ToDouble((Convert.ToDouble(beforeoverall) / Convert.ToDouble(aftvalstudentappeared)) * 100);
                                double passpercent2 = Math.Round(passpercent1, 2);
                                passpercent3 = passpercent3 + passpercent2;
                                beforoverallpercentage = Convert.ToString(passpercent3);
                               
                            }

                            if (afteroverall != "0")
                            {
                               
                                passpercent1 = Convert.ToDouble((Convert.ToDouble(afteroverall) / Convert.ToDouble(aftvalstudentappeared)) * 100);
                                double passpercent2 = Math.Round(passpercent1, 2);
                                 passpercent4 = passpercent4 + passpercent2;
                                afteroverallpercentage = Convert.ToString(passpercent4);
                            }



                        }
                        FpExternal.Sheets[0].Cells[chk, 4].Text = befvalstudentsappeared.ToString();
                        FpExternal.Sheets[0].Cells[chk, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpExternal.Sheets[0].Cells[chk, 5].Text = befvalpassedstudent.ToString();
                        FpExternal.Sheets[0].Cells[chk, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpExternal.Sheets[0].Cells[chk, 6].Text = beforevalpercentage.ToString();
                        FpExternal.Sheets[0].Cells[chk, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpExternal.Sheets[0].Cells[chk, 7].Text = aftvalstudentappeared.ToString();
                        FpExternal.Sheets[0].Cells[chk, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpExternal.Sheets[0].Cells[chk, 8].Text = aftvalpassedstudent.ToString();
                        FpExternal.Sheets[0].Cells[chk, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpExternal.Sheets[0].Cells[chk, 9].Text = aftervaluepercentage.ToString();
                        FpExternal.Sheets[0].Cells[chk, 9].HorizontalAlign = HorizontalAlign.Center;


                    }
                }
                FpExternal.Sheets[0].RowCount = FpExternal.Sheets[0].RowCount + 1;
                double afteroverallpercentage1 = Convert.ToDouble(afteroverallpercentage) / sno;
                double overallafter = Math.Round(afteroverallpercentage1, 2);
                double beforoverallpercentage1 = Convert.ToDouble(beforoverallpercentage) / sno;
                double overbefore = Math.Round(beforoverallpercentage1, 2);
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "Overall";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 4);
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Text = befvalstudentsappeared.ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Text = beforeoverall.ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].Text = overbefore.ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;

                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7].Text = befvalstudentsappeared.ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8].Text = afteroverall.ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9].Text = overallafter.ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;


                FpExternal.Sheets[0].RowCount = FpExternal.Sheets[0].RowCount + 1;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "Department OverAll Pass%(" + yr_string + ")";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, FpExternal.Sheets[0].ColumnCount);
                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "Before Revaluation:" + overbefore + " ";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, FpExternal.Sheets[0].ColumnCount);
                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "After Revaluation:" + overallafter + " ";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, FpExternal.Sheets[0].ColumnCount);


                FpExternal.Sheets[0].RowCount = FpExternal.Sheets[0].RowCount + 1;


                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "HOD";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 5);
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Text = "PRINCIPAL";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 5, 1, FpExternal.Sheets[0].ColumnCount);
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                FpExternal.Sheets[0].PageSize = FpExternal.Sheets[0].RowCount;
                FpExternal.SaveChanges();

            }
            else
            {
            }
        }
        else
        {

            FpExternal.Visible = false;
            btnExcel.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            btnprintmaster.Visible = false;
            lblerrormsg.Text = "No record found";
            lblerrormsg.Visible = true;

        }

        //}
        //catch (Exception ex)
        //{

        //}
    }
    public int GetSemester_AsNumber(int IpValue)
    {
        InsFlag = false;
        string strinssetting = "";
        string VarProcessValue = "";
        int GetSemesterAsNumber = 0;

        strinssetting = "select * from inssettings where LinkName='Semester Display'";
        con.Close();
        con.Open();
        SqlCommand cmd_ins = new SqlCommand(strinssetting, con);
        SqlDataReader dr_ins;
        dr_ins = cmd_ins.ExecuteReader();
        while (dr_ins.Read())
        {
            if (dr_ins.HasRows == true)
            {
                if (dr_ins["LinkName"].ToString() == "Semester Display")
                {
                    InsFlag = true;
                }
                if (Convert.ToInt32(dr_ins["LinkValue"]) == 0)
                {
                    GetSemesterAsNumber = IpValue;
                }
                else if (Convert.ToInt32(dr_ins["LinkValue"]) == 1)
                {
                    VarProcessValue = Convert.ToString(IpValue).Trim();
                }

            }
        }

        return IpValue;
    }
    public int Get_UnivExamCode(int DegreeCode, int Semester, int Batch)
    {

        string GetUnivExamCode = "";

        string strExam_code = "";
        strExam_code = "Select Exam_Code from Exam_Details where Degree_Code = " + DegreeCode.ToString() + " and Current_Semester = " + Semester.ToString() + " and Batch_Year = " + Batch.ToString() + " and exam_month='" + ddlMonth.SelectedValue.ToString() + "' and exam_year='" + ddlYear.SelectedItem.Text + "'";
        con.Close();
        con.Open();

        SqlDataReader dr_examcode;
        SqlCommand cmd_examcode = new SqlCommand(strExam_code, con);
        dr_examcode = cmd_examcode.ExecuteReader();
        while (dr_examcode.Read())
        {
            if (dr_examcode.HasRows == true)
            {
                if (dr_examcode["Exam_Code"].ToString() != "")
                {
                    GetUnivExamCode = dr_examcode["Exam_Code"].ToString();
                }
            }
        }
        if (GetUnivExamCode != "")
        {
            return Convert.ToInt32(GetUnivExamCode);
        }
        else
        {
            return 0;
        }


    }
    public void bindspread()
    {
        FpExternal.Sheets[0].ColumnCount = 10;
        FpExternal.Sheets[0].AutoPostBack = true;
        FpExternal.Sheets[0].ColumnHeader.RowCount = 2;
        FpExternal.Sheets[0].RowCount = 0;
        FpExternal.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        FpExternal.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        FpExternal.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
        FpExternal.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
        FpExternal.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
        FpExternal.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        FpExternal.Sheets[0].DefaultStyle.Font.Bold = false;
        FpExternal.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Sub Code";
        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Sub Name";
        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Name";
        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Before Revaluation";

        FpExternal.Sheets[0].ColumnHeader.Cells[0, 7].Text = "After Revaluation";

        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 1, 3);
        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 1, 3);

        FpExternal.Sheets[0].ColumnHeader.Cells[1, 4].Text = "No Of Students Appear";
        FpExternal.Sheets[0].ColumnHeader.Cells[1, 5].Text = "No Of Students Passed";
        FpExternal.Sheets[0].ColumnHeader.Cells[1, 6].Text = "% Of Pass";
        FpExternal.Sheets[0].ColumnHeader.Cells[1, 7].Text = "No Of Students Appear";
        FpExternal.Sheets[0].ColumnHeader.Cells[1, 8].Text = "No Of Students Passed";
        FpExternal.Sheets[0].ColumnHeader.Cells[1, 9].Text = "% Of Pass";

        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        style.Font.Size = 11;
        style.Font.Name = "Book Antiqua";

        FpExternal.Sheets[0].DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);

        FpExternal.Sheets[0].RowHeader.Visible = false;
        FpExternal.Visible = true;
        btnExcel.Visible = true;
        txtexcelname.Visible = true;
        lblrptname.Visible = true;
        btnprintmaster.Visible = true;

    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        Session["column_header_row_count"] = 2;

        string filt_details = "";
        string sec_details = "";
        string strsec = "";
        if (ddlSec.Enabled == true)
        {
            strsec = " Sec: " + ddlSec.SelectedItem.Text.ToString();
        }
        filt_details = "Batch: " + ddlBatch.SelectedItem.ToString() + "-" + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString();
        sec_details = "Sem :" + ddlSemYr.SelectedItem.ToString() + "-" + strsec;



        string degreedetails = string.Empty;

        degreedetails = "Result Analysis Report" + "@" + filt_details + "@" + sec_details;
        string pagename = "newuniversityresultanalysis.aspx";

        Printcontrol.loadspreaddetails(FpExternal, pagename, degreedetails);
        Printcontrol.Visible = true;
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string print = "";
            string appPath = HttpContext.Current.Server.MapPath("~");
            string strexcelname = "";
            if (appPath != "")
            {
                strexcelname = txtexcelname.Text;
                appPath = appPath.Replace("\\", "/");
                if (strexcelname != "")
                {
                    print = strexcelname;

                    string szPath = appPath + "/Report/";
                    string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")

                    FpExternal.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.Flush();
                    Response.WriteFile(szPath + szFile);
                    //=============================================
                }
                else
                {
                    lblnorec.Text = "Please enter your Report Name";
                    lblnorec.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
        }

    }
    public void getyear()
    {

        if (yr_val != "")
        {
            if (yr_val == "1" || yr_val == "2")
            {
                yr_string = "First Year";
            }
            if (yr_val == "3" || yr_val == "4")
            {
                yr_string = "Second Year";
            }
            if (yr_val == "5" || yr_val == "6")
            {
                yr_string = "Third Year";
            }
            if (yr_val == "7" || yr_val == "8")
            {
                yr_string = "Fourth Year";
            }
            if (yr_val == "9" || yr_val == "10")
            {
                yr_string = "Fifth Year";
            }
            if (yr_val == "11" || yr_val == "12")
            {
                yr_string = "Sixth Year";
            }
        }
    }

}