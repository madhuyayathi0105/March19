using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Configuration;

public partial class CamResultAnalysisi : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();

    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();
    int rankcou = 0;
    string group_user = "", singleuser = "", usercode = "", collegecode = string.Empty;
    string gpa = string.Empty;
    string creitpoint = string.Empty;
    string sections = string.Empty;
    string current_sem = string.Empty;
    string strsubject = string.Empty;

    int ExamCode = 0;
    int allpasscount = 0;
    int allappeared = 0;

    ArrayList alv = new ArrayList();
    Hashtable hashmark = new Hashtable();
    DataTable data = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblerror.Visible = false;
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            if (!Request.FilePath.Contains("CAMHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/MarkMod/CAMHome.aspx");
                    return;
                }
            }  
            collegecode = Session["collegecode"].ToString();
            if (!IsPostBack)
            {
                Chart1.Visible = false;
                bindcollege();
                bindbatch();
                binddegree();
                bindbranch();
                bindsem();
                bindsec();
                GetTest();
                clear();
                for (int c = 0; c < chklscolumn.Items.Count; c++)
                {
                    chklscolumn.Items[c].Selected = true;
                }
            }
        }
        catch (Exception ex) { }
    }

    public void bindbatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            ds = da.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }
            int count1 = ds.Tables[1].Rows.Count;
            if (count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                ddlbatch.SelectedValue = max_bat.ToString();
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void binddegree()
    {
        try
        {
            ddldegree.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = ddlcollege.SelectedItem.Value;
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            has.Clear();
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("college_code", collegecode);
            has.Add("user_code", usercode);
            ds = da.select_method("bind_degree", has, "sp");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void bindbranch()
    {
        try
        {
            ddlsem.Items.Clear();
            has.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = ddlcollege.SelectedItem.Value;
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("course_id", ddldegree.SelectedValue);
            has.Add("college_code", collegecode);
            has.Add("user_code", usercode);
            ds = da.select_method("bind_branch", has, "sp");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void bindsem()
    {
        try
        {
            ddlsem.Items.Clear();
            string duration = string.Empty;
            bool first_year = false;
            has.Clear();
            collegecode = ddlcollege.SelectedItem.Value;
            has.Add("degree_code", ddlbranch.SelectedValue.ToString());
            has.Add("batch_year", ddlbatch.SelectedValue.ToString());
            has.Add("college_code", collegecode);
            ds = da.select_method("bind_sem", has, "sp");
            int count3 = ds.Tables[0].Rows.Count;
            if (count3 > 0)
            {
                ddlsem.Enabled = true;
                duration = ds.Tables[0].Rows[0][0].ToString();
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                {
                    if (first_year == false)
                    {
                        ddlsem.Items.Add(loop_val.ToString());
                    }
                    else if (first_year == true && loop_val != 2)
                    {
                        ddlsem.Items.Add(loop_val.ToString());
                    }
                }
            }
            else
            {
                count3 = ds.Tables[1].Rows.Count;
                if (count3 > 0)
                {
                    ddlsem.Enabled = true;
                    duration = ds.Tables[1].Rows[0][0].ToString();
                    first_year = Convert.ToBoolean(ds.Tables[1].Rows[0][1].ToString());
                    for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                    {
                        if (first_year == false)
                        {
                            ddlsem.Items.Add(loop_val.ToString());
                        }
                        else if (first_year == true && loop_val != 2)
                        {
                            ddlsem.Items.Add(loop_val.ToString());
                        }
                    }
                }
                else
                {
                    ddlsem.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void bindsec()
    {
        try
        {
            ddlSec.Items.Clear();
            hat.Clear();
            hat.Add("batch_year", ddlbatch.SelectedValue.ToString());
            hat.Add("degree_code", ddlbranch.SelectedValue);
            ds = da.select_method("bind_sec", hat, "sp");
            int count5 = ds.Tables[0].Rows.Count;
            if (count5 > 0)
            {
                ddlSec.DataSource = ds;
                ddlSec.DataTextField = "sections";
                ddlSec.DataValueField = "sections";
                ddlSec.DataBind();
                ddlSec.Enabled = true;
            }
            else
            {
                ddlSec.Enabled = false;
            }
            ddlSec.Items.Add("ALL");
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void GetTest()
    {
        try
        {
            ddltest.Items.Clear();
            string SyllabusYr;
            string SyllabusQry;
            SyllabusQry = "select syllabus_year from syllabus_master where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester =" + ddlsem.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + "";
            SyllabusYr = d2.GetFunction(SyllabusQry.ToString());
            string Sqlstr = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and syllabus_year=" + SyllabusYr.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " order by criteria";
            DataSet titles = d2.select_method_wo_parameter(Sqlstr, "text");
            if (titles.Tables[0].Rows.Count > 0)
            {
                ddltest.DataSource = titles;
                ddltest.DataValueField = "Criteria_No";
                ddltest.DataTextField = "Criteria";
                ddltest.DataBind();
            }
        }
        catch
        {
        }
    }

    public void bindcollege()
    {
        try
        {
            string group_code = Session["group_code"].ToString();
            string columnfield = string.Empty;
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
            ds = da.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect("~/Default.aspx");
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void clear()
    {
        Showgrid.Visible = false;
        lblexcel.Visible = false;
        txtexcelname.Visible = false;
        btnexcel.Visible = false;
        btnprint.Visible = false;
        btndirectprt.Visible = false;
        Printcontrol.Visible = false;
        txtexcelname.Text = string.Empty;
        flow.Visible = false;
        Chart1.Visible = false;
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch();
            bindsem();
            bindsec();
            GetTest();
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsem();
            bindsec();
            GetTest();
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void txtTop_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string val = txtTop.Text;
            if (val != "0" && val != "00")
            {
                lblerror.Visible = false;
            }
            else
            {
                lblerror.Text = "Please Enter value Greater than Zero";
                lblerror.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            binddegree();
            bindbranch();
            bindsem();
            bindsec();
            GetTest();
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsem();
            bindsec();
            GetTest();
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsec();
            GetTest();
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void Buttongo_Click(object sender, EventArgs e)
    {
        try
        {

            btnPrint11();
            DataRow drow;

            int rowcnt = 0;
            int rowcnt1 = 0;
            Boolean toprank = false;
            clear();
            Chart1.Visible = false;
            string val = txtTop.Text;
            if (val.Trim() != "")
            {
                int vals = Convert.ToInt32(val);
                if (vals == 0)
                {
                    lblerror.Text = "Please Enter value Greater than Zero";
                    lblerror.Visible = true;
                    return;
                }
            }
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("SUBJECT NAME", typeof(string));
            dt2.Columns.Add("PASS", typeof(double));
            bool visfalg = false;
            for (int c = 0; c < chklscolumn.Items.Count; c++)
            {
                if (chklscolumn.Items[c].Selected == true)
                {
                    visfalg = true;
                }
            }
            if (visfalg == false)
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Column Order And Then Proceed";
                return;
            }
            if (ddltest.Items.Count == 0)
            {
                lblerror.Visible = true;
                lblerror.Text = "No Test Conducted";
                return;
            }

            ArrayList arrColHdrNames1 = new ArrayList();
            ArrayList arrColHdrNames2 = new ArrayList();

            arrColHdrNames1.Add("S.No");
            arrColHdrNames1.Add("Subject Code");
            arrColHdrNames1.Add("Subject Name");
            arrColHdrNames1.Add("Staff Name");
            arrColHdrNames1.Add("Subject Type");
            arrColHdrNames1.Add("Student Strength");
            arrColHdrNames1.Add("Before Retest");
            arrColHdrNames1.Add("Before Retest");
            arrColHdrNames1.Add("Before Retest");
            arrColHdrNames1.Add("Before Retest");
            arrColHdrNames1.Add("After Retest");
            arrColHdrNames1.Add("After Retest");
            arrColHdrNames1.Add("After Retest");
            arrColHdrNames1.Add("After Retest");
            arrColHdrNames1.Add("Remarks");

            arrColHdrNames2.Add("S.No");
            arrColHdrNames2.Add("Subject Code");
            arrColHdrNames2.Add("Subject Name");
            arrColHdrNames2.Add("Staff Name");
            arrColHdrNames2.Add("Subject Type");
            arrColHdrNames2.Add("Student Strength");
            arrColHdrNames2.Add("Appear");
            arrColHdrNames2.Add("Pass");
            arrColHdrNames2.Add("Fail");
            arrColHdrNames2.Add("Pass Percentage");
            arrColHdrNames2.Add("Appear ");
            arrColHdrNames2.Add("Pass ");
            arrColHdrNames2.Add("Fail ");
            arrColHdrNames2.Add("Pass Percentage ");
            arrColHdrNames2.Add("Remarks");

            data.Columns.Add("S.No", typeof(string));
            data.Columns.Add("Subject Code", typeof(string));
            data.Columns.Add("Subject Name", typeof(string));
            data.Columns.Add("Staff Name", typeof(string));
            data.Columns.Add("Subject Type", typeof(string));
            data.Columns.Add("Student Strength", typeof(string));
            data.Columns.Add("Appear", typeof(string));
            data.Columns.Add("Pass", typeof(string));
            data.Columns.Add("Fail", typeof(string));
            data.Columns.Add("Pass Percentage", typeof(string));
            data.Columns.Add("Appear ", typeof(string));
            data.Columns.Add("Pass ", typeof(string));
            data.Columns.Add("Fail ", typeof(string));
            data.Columns.Add("Pass Percentage ", typeof(string));
            data.Columns.Add("Remarks", typeof(string));


            DataRow drHdr1 = data.NewRow();
            DataRow drHdr2 = data.NewRow();


            for (int grCol = 0; grCol < data.Columns.Count; grCol++)
            {
                drHdr1[grCol] = arrColHdrNames1[grCol];
                drHdr2[grCol] = arrColHdrNames2[grCol];
            }

            data.Rows.Add(drHdr1);
            data.Rows.Add(drHdr2);

            int sno = 0;
            string secval = string.Empty;
            string section = string.Empty;
            string qrySection = string.Empty;
            if (ddlSec.SelectedItem.Text.Trim().ToLower() != "all")
            {
                secval = " and e.Sections='" + ddlSec.SelectedItem.Text + "'";
                qrySection = " and et.Sections='" + ddlSec.SelectedItem.Text + "'";
                section = ddlSec.SelectedItem.Text;
            }
            string getquery = "select distinct s.subject_no,s.subject_name,s.acronym,s.subject_code from subject s,exam_type e,result r where e.subject_no=s.subject_no and e.exam_code= r.exam_code and criteria_no='" + ddltest.SelectedValue.ToString() + "' " + secval + " order by s.subject_no ";
            getquery = getquery + " select ss.subject_type,s.subject_no,sm.staff_name,e.Sections from syllabus_master sy,sub_sem ss,subject s,staff_selector e,staffmaster sm where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and ss.subType_no=s.subType_no and ss.syll_code=s.syll_code and s.subject_no=e.subject_no and e.staff_code=sm.staff_code and sy.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and sy.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and sy.semester='" + ddlsem.SelectedItem.ToString() + "' " + secval + "";
            // getquery = getquery + " select isnull(count(re.roll_no),0) as repass,subject_no,e.sections,c.Criteria_no from tbl_result_retest re,Exam_type e,CriteriaForInternal c where re.exam_code=e.exam_code and re.marks_obtained>=e.min_mark and c.criteria_no=e.criteria_no and e.criteria_no='" + ddltest.SelectedValue.ToString() + "' " + secval + " group by subject_no,e.sections,c.Criteria_no";
            //getquery = getquery + " select subject_no,e.sections,e.min_mark,c.Criteria_no,r.roll_no as mroll,r.marks_obtained mmark,re.Roll_No,re.Marks_Obtained from tbl_result_retest re,Exam_type e,CriteriaForInternal c,Result r where re.exam_code=e.exam_code and r.roll_no=re.Roll_No and r.exam_code=re.Exam_Code and c.criteria_no=e.criteria_no and e.criteria_no='" + ddltest.SelectedValue.ToString() + "' " + secval + "";
            getquery = getquery + " select subject_no,e.sections,e.min_mark,c.Criteria_no,r.roll_no as mroll,r.marks_obtained mmark,re.Roll_No,r.Retest_Marks_obtained as Marks_Obtained from result re,Exam_type e,CriteriaForInternal c,Result r where re.exam_code=e.exam_code and r.roll_no=re.Roll_No and r.exam_code=re.Exam_Code and c.criteria_no=e.criteria_no and e.criteria_no='" + ddltest.SelectedValue.ToString() + "' " + secval + " and r.Retest_Marks_obtained is not null";
            getquery = getquery + " select count(distinct e.roll_no) as stucount,sc.subject_no from Registration e,subjectChooser sc where e.Roll_No=sc.roll_no and e.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and e.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and sc.semester='" + ddlsem.SelectedValue.ToString() + "' and e.cc=0 and e.delflag=0 and e.exam_flag<>'debar' " + secval + " group by sc.subject_no";
            DataSet ds = d2.select_method_wo_parameter(getquery, "text");

            DataSet dsResultAnalysis = new DataSet();
            string qry = " select c.criteria,c.Criteria_no,re.roll_no,et.subject_no,et.min_mark,et.max_mark,case when re.Retest_Marks_obtained is null then re.marks_obtained else re.Retest_Marks_obtained end OriginalMark,case when re.Retest_Marks_obtained is null then re.marks_obtained when (re.Retest_Marks_obtained is not null and re.marks_obtained>=re.Retest_Marks_obtained) then re.marks_obtained when re.Retest_Marks_obtained is not null and re.Retest_Marks_obtained>=re.marks_obtained then re.Retest_Marks_obtained end as ReTest ,re.marks_obtained,re.Retest_Marks_obtained from Result re,Exam_type et,CriteriaForInternal c where c.Criteria_no=et.criteria_no and et.exam_code=re.exam_code " + qrySection + " and c.Criteria_no='" + ddltest.SelectedValue.ToString() + "'";
            dsResultAnalysis = d2.select_method_wo_parameter(qry, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                btndirectprt.Visible = true;
                btnprint.Visible = true;
                btnexcel.Visible = true;
                lblexcel.Visible = true;
                txtexcelname.Visible = true;
                flow.Visible = true;
                int rankcnt = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sno++;

                    int beforeRetestStudents = 0;
                    int beforeRetestAppeared = 0;
                    int beforeRetestPassed = 0;
                    int beforeRetestFailed = 0;
                    int beforeRetestAbsent = 0;

                    int afterRetestStudents = 0;
                    int afterRetestAppeared = 0;
                    int afterRetestPassed = 0;
                    int afterRetestFailed = 0;
                    int afterRetestAbsent = 0;

                    double beforeRetestPassPercentage = 0;
                    double afterRetestPassPercentage = 0;
                    //double beforeRetestStudents = 0;
                    //double beforeRetestAppeared = 0;
                    //double beforeRetestPassed = 0;
                    //double beforeRetestFailed = 0;
                    //double beforeRetestAbsent = 0;

                    //double afterRetestStudents = 0;
                    //double afterRetestAppeared = 0;
                    //double afterRetestPassed = 0;
                    //double afterRetestFailed = 0;
                    //double afterRetestAbsent = 0;

                    drow = data.NewRow();
                    drow["S.No"] = sno.ToString();
                    drow["Subject Code"] = ds.Tables[0].Rows[i]["subject_name"].ToString();
                    drow["Subject Name"] = ds.Tables[0].Rows[i]["subject_code"].ToString();


                    string subjectno = ds.Tables[0].Rows[i]["subject_no"].ToString();
                    ds.Tables[1].DefaultView.RowFilter = "subject_no='" + subjectno + "'";
                    DataView dvstaff = ds.Tables[1].DefaultView;
                    string staffname = string.Empty;
                    string subtype = string.Empty;
                    for (int s = 0; s < dvstaff.Count; s++)
                    {
                        subtype = dvstaff[s]["subject_type"].ToString();
                        if (staffname == "")
                        {
                            staffname = dvstaff[s]["staff_name"].ToString();
                        }
                        else
                        {
                            staffname = staffname + ", " + dvstaff[s]["staff_name"].ToString();
                        }
                    }
                    drow["Staff Name"] = staffname;
                    drow["Subject Type"] = subtype;

                    DataView dvstucoun = new DataView();
                    if (ds.Tables.Count > 3 && ds.Tables[3].Rows.Count > 0)
                    {
                        ds.Tables[3].DefaultView.RowFilter = "subject_no='" + subjectno + "'";
                        dvstucoun = ds.Tables[3].DefaultView;
                    }
                    if (dvstucoun.Count > 0)
                    {
                        drow["Student Strength"] = dvstucoun[0]["stucount"].ToString();

                    }
                    DataTable dtBfTotalStudents = new DataTable();
                    DataTable dtAfTotalStudents = new DataTable();

                    DataTable dtBfPresent = new DataTable();
                    DataTable dtAfPresent = new DataTable();

                    DataTable dtBfPassed = new DataTable();
                    DataTable dtAfPassed = new DataTable();

                    DataTable dtBfFailed = new DataTable();
                    DataTable dtAfFailed = new DataTable();

                    DataTable dtBfAbsent = new DataTable();
                    DataTable dtAfAbsent = new DataTable();

                    if (dsResultAnalysis.Tables.Count > 0 && dsResultAnalysis.Tables[0].Rows.Count > 0)
                    {
                        dsResultAnalysis.Tables[0].DefaultView.RowFilter = "subject_no='" + subjectno + "' and (OriginalMark>='0' or OriginalMark='-2' or OriginalMark='-3' or OriginalMark='-1') ";
                        dtBfTotalStudents = dsResultAnalysis.Tables[0].DefaultView.ToTable(true, "roll_no");
                        beforeRetestStudents = dtBfTotalStudents.Rows.Count;

                        dsResultAnalysis.Tables[0].DefaultView.RowFilter = "subject_no='" + subjectno + "' and (ReTest>='0' or ReTest='-2' or ReTest='-3' or ReTest='-1') ";
                        dtAfTotalStudents = dsResultAnalysis.Tables[0].DefaultView.ToTable(true, "roll_no");
                        afterRetestStudents = dtAfTotalStudents.Rows.Count;

                        dsResultAnalysis.Tables[0].DefaultView.RowFilter = "subject_no='" + subjectno + "' and (OriginalMark>='0' or OriginalMark='-2' or OriginalMark='-3') ";
                        dtBfPresent = dsResultAnalysis.Tables[0].DefaultView.ToTable(true, "roll_no");
                        beforeRetestAppeared = dtBfPresent.Rows.Count;

                        dsResultAnalysis.Tables[0].DefaultView.RowFilter = "subject_no='" + subjectno + "' and (ReTest>='0' or ReTest='-2' or ReTest='-3') ";
                        dtAfPresent = dsResultAnalysis.Tables[0].DefaultView.ToTable(true, "roll_no");
                        afterRetestAppeared = dtAfPresent.Rows.Count;

                        dsResultAnalysis.Tables[0].DefaultView.RowFilter = "subject_no='" + subjectno + "' and (OriginalMark>=min_mark or OriginalMark='-2' or OriginalMark='-3') ";
                        dtBfPassed = dsResultAnalysis.Tables[0].DefaultView.ToTable(true, "roll_no");
                        beforeRetestPassed = dtBfPassed.Rows.Count;

                        dsResultAnalysis.Tables[0].DefaultView.RowFilter = "subject_no='" + subjectno + "' and (ReTest>=min_mark or ReTest='-2' or ReTest='-3')";
                        dtAfPassed = dsResultAnalysis.Tables[0].DefaultView.ToTable(true, "roll_no");
                        afterRetestPassed = dtAfPassed.Rows.Count;

                        dsResultAnalysis.Tables[0].DefaultView.RowFilter = "subject_no='" + subjectno + "' and (OriginalMark<min_mark and OriginalMark<>'-2' and OriginalMark<>'-3' and OriginalMark<>'-1' and OriginalMark<>'-4' and OriginalMark<>'-5' and OriginalMark<>'-6' and OriginalMark<>'-7' and OriginalMark<>'-8' and OriginalMark<>'-9' and OriginalMark<>'-10' and OriginalMark<>'-11' and OriginalMark<>'-12' and OriginalMark<>'-13' and OriginalMark<>'-14' and OriginalMark<>'-15' and OriginalMark<>'-16' and OriginalMark<>'-17')";
                        dtBfFailed = dsResultAnalysis.Tables[0].DefaultView.ToTable(true, "roll_no");
                        beforeRetestFailed = dtBfFailed.Rows.Count;

                        dsResultAnalysis.Tables[0].DefaultView.RowFilter = "subject_no='" + subjectno + "' and (ReTest<min_mark and ReTest<>'-2' and ReTest<>'-3' and ReTest<>'-1' and ReTest<>'-4' and ReTest<>'-5' and ReTest<>'-6' and ReTest<>'-7' and ReTest<>'-8' and ReTest<>'-9' and ReTest<>'-10' and ReTest<>'-11' and ReTest<>'-12' and ReTest<>'-13' and ReTest<>'-14' and ReTest<>'-15' and ReTest<>'-16' and ReTest<>'-17')";
                        dtAfFailed = dsResultAnalysis.Tables[0].DefaultView.ToTable(true, "roll_no");
                        afterRetestFailed = dtAfFailed.Rows.Count;

                        dsResultAnalysis.Tables[0].DefaultView.RowFilter = "subject_no='" + subjectno + "' and OriginalMark='-1'";
                        dtBfAbsent = dsResultAnalysis.Tables[0].DefaultView.ToTable(true, "roll_no");
                        beforeRetestAbsent = dtBfAbsent.Rows.Count;

                        dsResultAnalysis.Tables[0].DefaultView.RowFilter = "subject_no='" + subjectno + "' and ReTest='-1'";
                        dtAfAbsent = dsResultAnalysis.Tables[0].DefaultView.ToTable(true, "roll_no");
                        afterRetestAbsent = dtAfAbsent.Rows.Count;

                    }

                    //string present = d2.GetFunction("select isnull(count(marks_obtained),0) as 'PRESENT_COUNT' from result r,registration rt,exam_type e,subjectchooser sc where r.roll_no=sc.roll_no and e.subject_no=sc.subject_no and r.exam_code=e.exam_code  and e.subject_no='" + subjectno + "' and  e.criteria_no='" + ddltest.SelectedValue.ToString() + "' " + secval + " and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3' ) and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0  and r.roll_no=rt.roll_no  and rt.RollNo_Flag<>0");
                    //string pass = d2.GetFunction("select isnull(count(marks_obtained),0) as 'PASS_COUNT' from result r,registration reg,exam_type e,subjectchooser sc where e.subject_no=sc.subject_no and r.roll_no=sc.roll_no and r.exam_code=e.exam_code and e.subject_no='" + subjectno + "' and  e.criteria_no='" + ddltest.SelectedValue.ToString() + "' " + secval + " and (marks_obtained>=e.min_mark or marks_obtained='-3' or marks_obtained='-2') and reg.roll_no=r.roll_no and reg.cc=0 and reg.delflag=0 and reg.exam_flag<>'debar' and reg.RollNo_Flag<>0 ");
                    string present = d2.GetFunction("select isnull(count(case when Retest_Marks_obtained is null then marks_obtained else Retest_Marks_obtained end),0) as 'PRESENT_COUNT' from result r,registration rt,exam_type e,subjectchooser sc where r.roll_no=sc.roll_no and e.subject_no=sc.subject_no and r.exam_code=e.exam_code  and e.subject_no='" + subjectno + "' and  e.criteria_no='" + ddltest.SelectedValue.ToString() + "' " + secval + " and (case when Retest_Marks_obtained is null then marks_obtained else Retest_Marks_obtained end>=0 or case when Retest_Marks_obtained is null then marks_obtained else Retest_Marks_obtained end='-2' or case when Retest_Marks_obtained is null then marks_obtained else Retest_Marks_obtained end='-3' ) and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0  and r.roll_no=rt.roll_no  and rt.RollNo_Flag<>0");
                    string pass = d2.GetFunction("select isnull(count(case when Retest_Marks_obtained is null then marks_obtained else Retest_Marks_obtained end),0)as 'PASS_COUNT' from result r,registration reg,exam_type e,subjectchooser sc where e.subject_no=sc.subject_no and r.roll_no=sc.roll_no and r.exam_code=e.exam_code and e.subject_no='" + subjectno + "' and  e.criteria_no='" + ddltest.SelectedValue.ToString() + "' " + secval + " and (case when Retest_Marks_obtained is null then marks_obtained else Retest_Marks_obtained end>=e.min_mark or case when Retest_Marks_obtained is null then marks_obtained else Retest_Marks_obtained end='-3' or case when Retest_Marks_obtained is null then marks_obtained else Retest_Marks_obtained end='-2') and reg.roll_no=r.roll_no and reg.cc=0 and reg.delflag=0 and reg.exam_flag<>'debar' and reg.RollNo_Flag<>0 ");
                    double appera = Convert.ToDouble(present);
                    double passs = Convert.ToDouble(pass);
                    double fail = appera - passs;
                    double passperce = passs / appera * 100;

                    beforeRetestPassPercentage = 0;
                    double beforeAbsentCount = 0;
                    if (chkIncludeAbsent.Checked)
                    {
                        beforeAbsentCount = beforeRetestAbsent;
                    }
                    if (beforeRetestAppeared > 0 && beforeRetestPassed > 0)
                    {
                        beforeRetestPassPercentage = beforeRetestPassed / (beforeRetestAppeared + beforeAbsentCount) * 100;
                    }
                    beforeRetestPassPercentage = Math.Round(beforeRetestPassPercentage, 2, MidpointRounding.AwayFromZero);
                    if (passperce.ToString().Trim().ToLower() == "nan" || passperce.ToString().Trim().ToLower() == "infinity")
                    {
                        passperce = 0;
                    }
                    passperce = Math.Round(passperce, 2, MidpointRounding.AwayFromZero);

                    drow["Appear"] = present;
                    drow["Pass"] = pass;
                    drow["Fail"] = fail.ToString();
                    drow["Pass Percentage"] = beforeRetestPassPercentage.ToString();


                    double resetappera = 0;
                    double retestpass = 0;
                    double retestfail = 0;
                    double resetonlyappear = 0;
                    DataView dvretes = new DataView();
                    if (ds.Tables.Count > 3 && ds.Tables[3].Rows.Count > 0)
                    {
                        ds.Tables[2].DefaultView.RowFilter = "subject_no='" + subjectno + "'";
                        dvretes = ds.Tables[2].DefaultView;
                    }
                    if (dvretes.Count > 0)
                    {
                        for (int re = 0; re < dvretes.Count; re++)
                        {
                            string afterresetmark = dvretes[re]["mmark"].ToString();
                            string beformark = dvretes[re]["Marks_Obtained"].ToString();
                            if (afterresetmark != "-1" || beformark != "-1")
                            {
                                resetappera++;
                                if (beformark == "-1" && afterresetmark != "-1")
                                {
                                    resetonlyappear++;
                                }
                                double minmarkal = Convert.ToDouble(dvretes[re]["min_mark"].ToString());
                                double resm = Convert.ToDouble(afterresetmark);
                                if (resm >= minmarkal)
                                {
                                    retestpass++;
                                }
                                else
                                {
                                    retestfail++;
                                }
                            }
                        }
                        double repasspercen = (passs + retestpass) / (appera + resetonlyappear) * 100;

                        afterRetestPassPercentage = 0;
                        double afterAbsentCount = 0;
                        if (chkIncludeAbsent.Checked)
                        {
                            afterAbsentCount = afterRetestAbsent;
                        }
                        if (afterRetestAppeared > 0 && afterRetestPassed > 0)
                        {
                            afterRetestPassPercentage = afterRetestPassed / (afterRetestAppeared + afterAbsentCount) * 100;
                        }
                        afterRetestPassPercentage = Math.Round(afterRetestPassPercentage, 2, MidpointRounding.AwayFromZero);
                        if (repasspercen.ToString().Trim().ToLower() == "nan" || repasspercen.ToString().Trim().ToLower() == "infinity")
                        {
                            repasspercen = 0;
                        }
                        repasspercen = Math.Round(repasspercen, 2, MidpointRounding.AwayFromZero);

                        drow["Appear "] = resetappera.ToString();
                        drow["Pass "] = retestpass.ToString();
                        drow["Fail "] = retestfail.ToString();
                        drow["Pass Percentage "] = afterRetestPassPercentage.ToString();

                    }
                    else
                    {
                        drow["Appear "] = "-";
                        drow["Pass "] = "-";
                        drow["Fail "] = "-";
                        drow["Pass Percentage "] = "-";


                    }
                    data.Rows.Add(drow);
                    DataRow dr2 = dt2.NewRow();
                    dr2[0] = ds.Tables[0].Rows[i]["subject_name"].ToString();
                    dr2[1] = beforeRetestPassPercentage.ToString();
                    dt2.Rows.Add(dr2);
                }
                int minmark = 0;
                bool diflag = false;
                for (int c = 14; c < chklscolumn.Items.Count; c++)
                {
                    if (chklscolumn.Items[c].Selected == true)
                    {
                        diflag = true;
                    }
                }
                if (diflag == true)
                {
                    string examcode = string.Empty;
                    string getexamcode = "select distinct max_mark,isnull(min_mark,'0') as min_mark,r.exam_code from subject s,exam_type e,result r where e.subject_no=s.subject_no and e.exam_code= r.exam_code and criteria_no='" + ddltest.SelectedValue.ToString() + "' " + secval + " ";
                    DataSet daexamcode = d2.select_method_wo_parameter(getexamcode, "Text");
                    for (int d = 0; d < daexamcode.Tables[0].Rows.Count; d++)
                    {
                        if (examcode == "")
                        {
                            examcode = daexamcode.Tables[0].Rows[d]["exam_code"].ToString();
                        }
                        else
                        {
                            examcode = examcode + ',' + daexamcode.Tables[0].Rows[d]["exam_code"].ToString();
                        }
                        minmark = Convert.ToInt32(daexamcode.Tables[0].Rows[d]["min_mark"].ToString());
                    }
                    drow = data.NewRow();
                    data.Rows.Add(drow);

                    if (ddlSec.SelectedItem.Text.Trim().ToLower() != "all")
                    {
                        secval = " and rt.Sections='" + ddlSec.SelectedItem.Text + "'";
                        section = ddlSec.SelectedItem.Text;
                    }
                    if (chklscolumn.Items[15].Selected == true)
                    {
                        rowcnt++;
                        drow = data.NewRow();
                        drow["S.No"] = "Total Number of Students";
                        data.Rows.Add(drow);
                        double getallapera = Convert.ToDouble(d2.GetFunction("select isnull(count(distinct rt.roll_no),0) as 'appear' from result r,registration rt where r.exam_code  in(" + examcode + ") and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3') " + secval + " and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0"));
                        double getallpass = Convert.ToDouble(d2.GetFunction("select isnull(count(distinct rt.roll_no),0) as 'allpass_count' from result r,registration rt where r.exam_code in(" + examcode + ") and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3' or marks_obtained='-1') " + secval + " and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0"));
                        double getallfail = Convert.ToDouble(d2.GetFunction("select isnull(count(distinct rt.roll_no),0) from result rt,registration r where rt.exam_code in(" + examcode + ") and rt.roll_no=r.roll_no and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and r.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and (rt.marks_obtained<" + minmark + " and rt.marks_obtained<>'-3' and rt.marks_obtained<>'-2') and r.cc=0 and r.exam_flag <>'DEBAR' and r.delflag=0 and r.RollNo_Flag<>0"));
                        double allpass = getallpass - getallfail;
                        double allpassperce = allpass / getallapera * 100;
                        allpassperce = Math.Round(allpassperce, 2, MidpointRounding.AwayFromZero);
                        if (allpassperce.ToString().Trim().ToLower() == "nan" || allpassperce.ToString().Trim().ToLower() == "infinity")
                        {
                            allpassperce = 0;
                        }
                        data.Rows[data.Rows.Count - 1][6] = getallapera.ToString();
                        data.Rows[data.Rows.Count - 1][7] = allpass.ToString();
                        data.Rows[data.Rows.Count - 1][8] = getallfail.ToString();
                        data.Rows[data.Rows.Count - 1][9] = allpassperce.ToString();


                    }
                    string strdetail = "select isnull(count(distinct rt.roll_no),0) as 'appear',rt.Stud_Type,a.sex from result r,registration rt,applyn a where a.app_no=rt.App_No and r.exam_code  in(" + examcode + ") and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3' ) and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0  " + secval + " group by rt.Stud_Type,a.sex ";
                    strdetail = strdetail + " select isnull(count(distinct rt.roll_no),0) as 'allpass_count',rt.Stud_Type,a.sex from result r,registration rt,applyn a where a.app_no=rt.App_No and r.exam_code in(" + examcode + ") and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-1')  and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0  " + secval + " group by rt.Stud_Type,a.sex ";
                    //strdetail = strdetail + " select r.Stud_Type,a.sex,count(rt.marks_obtained) as nooffailure,count(distinct rt.roll_no) as fail from result rt,registration r,applyn a where a.app_no=r.App_No and rt.exam_code in(" + examcode + ") and rt.roll_no=r.roll_no and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and r.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and (rt.marks_obtained<" + minmark + " and rt.marks_obtained<>'-3' and rt.marks_obtained<>'-2') and r.cc=0 and r.exam_flag <>'DEBAR' and r.delflag=0 and r.RollNo_Flag<>0 group by r.Stud_Type,a.sex,rt.roll_no";
                    strdetail = strdetail + " select r.Stud_Type,a.sex,count(rt.marks_obtained) as nooffailure,count(distinct rt.roll_no) as fail from result rt,registration r,applyn a where a.app_no=r.App_No and rt.exam_code in(" + examcode + ") and rt.roll_no=r.roll_no and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and r.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and (rt.marks_obtained<" + minmark + " and rt.marks_obtained<>'-3' and rt.marks_obtained<>'-2') and r.cc=0 and r.exam_flag <>'DEBAR' and r.delflag=0 and r.RollNo_Flag<>0 group by r.Stud_Type,a.sex,rt.roll_no";
                    DataSet dsdetails = d2.select_method_wo_parameter(strdetail, "text");
                    double gappear = 0, gtotal = 0, gfail = 0, gpass = 0;
                    double bappear = 0, btotal = 0, bfail = 0, bpass = 0;
                    double ghappear = 0, ghtotal = 0, ghfail = 0, ghpass = 0;
                    double bhappear = 0, bhtotal = 0, bhfail = 0, bhpass = 0;
                    double gdappear = 0, gdtotal = 0, gdfail = 0, gdpass = 0;
                    double bdappear = 0, bdtotal = 0, bdfail = 0, bdpass = 0;
                    int onf = 0, tw0f = 0, thref = 0;
                    for (int aa = 0; aa < dsdetails.Tables[0].Rows.Count; aa++)
                    {
                        string asex = dsdetails.Tables[0].Rows[aa]["sex"].ToString().Trim();
                        string astype = dsdetails.Tables[0].Rows[aa]["Stud_Type"].ToString().Trim().ToLower();
                        //string atrans = dsdetails.Tables[0].Rows[aa]["trans"].ToString();
                        string acount = dsdetails.Tables[0].Rows[aa]["appear"].ToString();
                        if (asex == "1")
                        {
                            gappear = gappear + Convert.ToDouble(acount);
                            if (astype == "day scholar")
                            {
                                gdappear = gdappear + Convert.ToDouble(acount);
                            }
                            else
                            {
                                ghappear = ghappear + Convert.ToDouble(acount);
                            }
                        }
                        else
                        {
                            bappear = bappear + Convert.ToDouble(acount);
                            if (astype == "day scholar")
                            {
                                bdappear = bdappear + Convert.ToDouble(acount);
                            }
                            else
                            {
                                bhappear = bhappear + Convert.ToDouble(acount);
                            }
                        }
                    }
                    for (int ast = 0; ast < dsdetails.Tables[1].Rows.Count; ast++)
                    {
                        string asex = dsdetails.Tables[1].Rows[ast]["sex"].ToString().Trim();
                        string astype = dsdetails.Tables[1].Rows[ast]["Stud_Type"].ToString().Trim().ToLower();
                        //string atrans = dsdetails.Tables[1].Rows[ast]["trans"].ToString();
                        string acount = dsdetails.Tables[1].Rows[ast]["allpass_count"].ToString();
                        if (asex == "1")
                        {
                            gtotal = gtotal + Convert.ToDouble(acount);
                            if (astype == "day scholar")
                            {
                                gdtotal = gdtotal + Convert.ToDouble(acount);
                            }
                            else
                            {
                                ghtotal = ghtotal + Convert.ToDouble(acount);
                            }
                        }
                        else
                        {
                            btotal = btotal + Convert.ToDouble(acount);
                            if (astype == "day scholar")
                            {
                                bdtotal = bdtotal + Convert.ToDouble(acount);
                            }
                            else
                            {
                                bhtotal = bhtotal + Convert.ToDouble(acount);
                            }
                        }
                    }
                    for (int af = 0; af < dsdetails.Tables[2].Rows.Count; af++)
                    {
                        string asex = dsdetails.Tables[2].Rows[af]["sex"].ToString().Trim();
                        string astype = dsdetails.Tables[2].Rows[af]["Stud_Type"].ToString().Trim().ToLower();
                        // string atrans = dsdetails.Tables[2].Rows[af]["trans"].ToString();
                        string acount = dsdetails.Tables[2].Rows[af]["fail"].ToString();
                        string nfs = dsdetails.Tables[2].Rows[af]["nooffailure"].ToString();
                        if (nfs == "1")
                        {
                            onf++;
                        }
                        else if (nfs == "2")
                        {
                            tw0f++;
                        }
                        else
                        {
                            thref++;
                        }
                        if (asex == "1")
                        {
                            gfail = gfail + Convert.ToDouble(acount);
                            if (astype == "day scholar")
                            {
                                gdfail = gdfail + Convert.ToDouble(acount);
                            }
                            else
                            {
                                ghfail = ghfail + Convert.ToDouble(acount);
                            }
                        }
                        else
                        {
                            bfail = bfail + Convert.ToDouble(acount);
                            if (astype == "day scholar")
                            {
                                bdfail = bdfail + Convert.ToDouble(acount);
                            }
                            else
                            {
                                bhfail = bhfail + Convert.ToDouble(acount);
                            }
                        }
                    }
                    if (chklscolumn.Items[16].Selected == true)
                    {
                        rowcnt++;
                        drow = data.NewRow();
                        drow["S.No"] = "Total Number of Girl Students";
                        data.Rows.Add(drow);

                        gpass = gtotal - gfail;
                        double gallpassperec = gpass / gappear * 100;
                        if (gallpassperec.ToString().Trim().ToLower() == "nan" || gallpassperec.ToString().Trim().ToLower() == "infinity")
                        {
                            gallpassperec = 0;
                        }
                        gallpassperec = Math.Round(gallpassperec, 2, MidpointRounding.AwayFromZero);

                        data.Rows[data.Rows.Count - 1][6] = gappear.ToString();
                        data.Rows[data.Rows.Count - 1][7] = gpass.ToString();
                        data.Rows[data.Rows.Count - 1][8] = gfail.ToString();
                        data.Rows[data.Rows.Count - 1][9] = gallpassperec.ToString();


                    }
                    if (chklscolumn.Items[17].Selected == true)
                    {
                        rowcnt++;
                        drow = data.NewRow();
                        drow["S.No"] = "Total Number of Boy Students";
                        data.Rows.Add(drow);

                        bpass = btotal - bfail;
                        double ballpassperec = bpass / bappear * 100;
                        if (ballpassperec.ToString().Trim().ToLower() == "nan" || ballpassperec.ToString().Trim().ToLower() == "infinity")
                        {
                            ballpassperec = 0;
                        }
                        ballpassperec = Math.Round(ballpassperec, 2, MidpointRounding.AwayFromZero);

                        data.Rows[data.Rows.Count - 1][6] = bappear.ToString();
                        data.Rows[data.Rows.Count - 1][7] = bpass.ToString();
                        data.Rows[data.Rows.Count - 1][8] = bfail.ToString();
                        data.Rows[data.Rows.Count - 1][9] = ballpassperec.ToString();




                    }
                    if (chklscolumn.Items[18].Selected == true)
                    {
                        rowcnt++;
                        drow = data.NewRow();
                        drow["S.No"] = "Total Number of Girl Hostel Students";
                        data.Rows.Add(drow);

                        ghpass = ghtotal - ghfail;
                        double ghpassper = ghpass / ghappear * 100;
                        if (ghpassper.ToString().Trim().ToLower() == "nan" || ghpassper.ToString().Trim().ToLower() == "infinity")
                        {
                            ghpassper = 0;
                        }
                        ghpassper = Math.Round(ghpassper, 2, MidpointRounding.AwayFromZero);

                        data.Rows[data.Rows.Count - 1][6] = ghappear.ToString();
                        data.Rows[data.Rows.Count - 1][7] = ghpass.ToString();
                        data.Rows[data.Rows.Count - 1][8] = ghfail.ToString();
                        data.Rows[data.Rows.Count - 1][9] = ghpassper.ToString();

                    }
                    if (chklscolumn.Items[19].Selected == true)
                    {
                        rowcnt++;
                        drow = data.NewRow();
                        drow["S.No"] = "Total Number of Boys Hostel Students";
                        data.Rows.Add(drow);


                        bhpass = bhtotal - bhfail;
                        double bhpassper = bhpass / bhappear * 100;
                        if (bhpassper.ToString().Trim().ToLower() == "nan" || bhpassper.ToString().Trim().ToLower() == "infinity")
                        {
                            bhpassper = 0;
                        }
                        bhpassper = Math.Round(bhpassper, 2, MidpointRounding.AwayFromZero);


                        data.Rows[data.Rows.Count - 1][6] = bhappear.ToString();
                        data.Rows[data.Rows.Count - 1][7] = bhpass.ToString();
                        data.Rows[data.Rows.Count - 1][8] = bhfail.ToString();
                        data.Rows[data.Rows.Count - 1][9] = bhpassper.ToString();


                    }
                    if (chklscolumn.Items[20].Selected == true)
                    {
                        rowcnt++;
                        drow = data.NewRow();
                        drow["S.No"] = "Total Number of Girl Day Scholar Students";
                        data.Rows.Add(drow);


                        gdpass = gdtotal - gdfail;
                        double gdpassper = gdpass / gdappear * 100;
                        if (gdpassper.ToString().Trim().ToLower() == "nan" || gdpassper.ToString().Trim().ToLower() == "infinity")
                        {
                            gdpassper = 0;
                        }
                        gdpassper = Math.Round(gdpassper, 2, MidpointRounding.AwayFromZero);

                        data.Rows[data.Rows.Count - 1][6] = gdappear.ToString();
                        data.Rows[data.Rows.Count - 1][7] = gdpass.ToString();
                        data.Rows[data.Rows.Count - 1][8] = gdfail.ToString();
                        data.Rows[data.Rows.Count - 1][9] = gdpassper.ToString();


                    }
                    if (chklscolumn.Items[21].Selected == true)
                    {
                        rowcnt++;
                        drow = data.NewRow();
                        drow["S.No"] = "Total Number of Boys Day Scholar Students";
                        data.Rows.Add(drow);



                        bdpass = bdtotal - bdfail;
                        double bdpassper = bdpass / bdappear * 100;
                        if (bdpassper.ToString().Trim().ToLower() == "nan" || bdpassper.ToString().Trim().ToLower() == "infinity")
                        {
                            bdpassper = 0;
                        }
                        bdpassper = Math.Round(bdpassper, 2, MidpointRounding.AwayFromZero);

                        data.Rows[data.Rows.Count - 1][6] = bdappear.ToString();
                        data.Rows[data.Rows.Count - 1][7] = bdpass.ToString();
                        data.Rows[data.Rows.Count - 1][8] = bdfail.ToString();
                        data.Rows[data.Rows.Count - 1][9] = bdpassper.ToString();


                    }
                    if (chklscolumn.Items[22].Selected == true)
                    {
                        rowcnt1++;
                        drow = data.NewRow();
                        drow["S.No"] = "Total Number of Students Failed In One Subject";
                        data.Rows.Add(drow);

                        data.Rows[data.Rows.Count - 1][8] = onf.ToString();


                    }
                    if (chklscolumn.Items[23].Selected == true)
                    {
                        rowcnt1++;
                        drow = data.NewRow();
                        drow["S.No"] = "Total Number of Students Failed In Two Subjects";
                        data.Rows.Add(drow);

                        data.Rows[data.Rows.Count - 1][8] = tw0f.ToString();

                    }
                    if (chklscolumn.Items[24].Selected == true)
                    {
                        rowcnt1++;
                        drow = data.NewRow();
                        drow["S.No"] = "Total Number of Students Failed In 3 & Above Subjects";
                        data.Rows.Add(drow);


                        data.Rows[data.Rows.Count - 1][8] = thref.ToString();

                    }
                }
                //==============================Rank List=============================


                string getcou = txtTop.Text.ToString();
                if (getcou.Trim() != "" && getcou.Trim() != "0")
                {
                    rankcou = Convert.ToInt32(getcou);
                }
                if (rankcou > 0)
                {
                    if (ddlSec.SelectedItem.Text != "ALL")
                    {
                        secval = " and r.Sections='" + ddlSec.SelectedItem.Text + "'";
                        section = ddlSec.SelectedItem.Text;
                    }
                    else
                    {
                        section = string.Empty;
                        secval = string.Empty;
                    }
                    int delval = d2.update_method_wo_parameter("delete rank", "Text");
                    string filterwithsection = "a.app_no=r.app_no and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and et.subject_no=s.subject_no and et.sections=r.sections and r.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and et.criteria_no ='" + ddltest.SelectedValue.ToString() + "' " + secval + " and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0   and r.sections='" + ddlSec.SelectedValue.ToString() + "' order by s.subject_no";
                    string filterwithoutsection = "a.app_no=r.app_no and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and et.subject_no=s.subject_no and et.sections=r.sections and r.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and et.criteria_no ='" + ddltest.SelectedValue.ToString() + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0  order by s.subject_no";
                    hat.Clear();
                    hat.Add("batchyear", ddlbatch.SelectedValue.ToString());
                    hat.Add("degreecode", ddlbranch.SelectedValue.ToString());
                    hat.Add("criteria_no", ddltest.SelectedValue.ToString());
                    hat.Add("sections", section);
                    hat.Add("filterwithsection", filterwithsection.ToString());
                    hat.Add("filterwithoutsection", filterwithoutsection.ToString());
                    DataSet ds2 = d2.select_method("PROC_STUD_ALL_SUBMARK", hat, "sp");
                    double fail_sub_cnt = 0;
                    double find_total = 0;
                    double sum_max_mark = 0;
                    double percent = 0;
                    string sqlStr = "select distinct len(registration.Roll_No),registration.Roll_No as roll,registration.Reg_No as regno,registration.stud_name as studname,registration.stud_type as studtype,registration.App_No as ApplicationNumber from registration, applyn a,exam_type et,result rt where a.app_no=registration.app_no and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + "   and registration.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0  and  rt.exam_code=et.exam_code and registration.roll_no=rt.roll_no and et.criteria_no =" + ddltest.SelectedValue.ToString() + " ";
                    ds1.Clear();
                    ds1.Reset();
                    ds1 = d2.select_method_wo_parameter(sqlStr, "text");
                    int subrow = 0;
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        for (int row = 0; row < ds1.Tables[0].Rows.Count; row++)
                        {
                            fail_sub_cnt = 0;
                            sum_max_mark = 0;
                            find_total = 0;
                            string getroll = ds1.Tables[0].Rows[row]["roll"].ToString();
                            ds2.Tables[0].DefaultView.RowFilter = "roll='" + ds1.Tables[0].Rows[row]["roll"].ToString() + "'";
                            DataView dvmark = ds2.Tables[0].DefaultView;
                            for (int j = 0; j < dvmark.Count; j++)
                            {
                                if (double.Parse(dvmark[j]["mark"].ToString()) != -2 && double.Parse(dvmark[j]["mark"].ToString()) != -3 && (double.Parse(dvmark[j]["mark"].ToString()) < double.Parse(dvmark[j]["min_mark"].ToString())))
                                {
                                    fail_sub_cnt++;
                                }
                                if (double.Parse(dvmark[j]["mark"].ToString()) >= 0 && (double.Parse(dvmark[j]["mark"].ToString()) >= Convert.ToInt32(dvmark[j]["min_mark"].ToString())))
                                {
                                    find_total = (Convert.ToDouble(find_total) + Convert.ToDouble(dvmark[j]["mark"].ToString()));
                                    sum_max_mark = sum_max_mark + Convert.ToInt32(dvmark[j]["max_mark"].ToString());
                                    percent = Convert.ToDouble((Convert.ToDouble(find_total) / sum_max_mark) * 100);
                                }
                            }
                            if (fail_sub_cnt == 0 && find_total > 0)
                            {
                                hat.Clear();
                                hat.Add("RollNumber", ds1.Tables[0].Rows[row]["roll"].ToString());
                                hat.Add("criteria_no", ddltest.SelectedValue.ToString());
                                hat.Add("Total", find_total.ToString());
                                hat.Add("avg", percent.ToString());
                                hat.Add("rank", "");
                                int o = d2.insert_method("INSERT_RANK", hat, "sp");
                            }
                        }
                    }
                    DataSet ds3 = d2.select_method_wo_parameter("SELECT_RANK", "sp");
                    int rank_row_count = 0;

                    if (ds3.Tables[0].Rows.Count > 0)
                    {
                        toprank = true;
                        int sno1 = 0;
                        drow = data.NewRow();
                        data.Rows.Add(drow);


                        drow = data.NewRow();
                        drow["S.No"] = "Top  " + rankcou + "  Students List";
                        data.Rows.Add(drow);
                        rankcnt++;


                        drow = data.NewRow();
                        drow["S.No"] = "SNo";
                        drow["Subject Code"] = "Roll No";
                        drow["Subject Name"] = "Reg No";
                        drow["Staff Name"] = "Student Name";
                        drow["Subject Type"] = "Student Type";
                        drow["Student Strength"] = "Total Mark";
                        drow["Appear"] = "Rank";

                        data.Rows.Add(drow);
                        rankcnt++;


                        for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                        {
                            rank_row_count++;
                            if (rank_row_count <= rankcou)
                            {
                                sno1++;
                                rankcnt++;
                                drow = data.NewRow();
                                drow["S.No"] = rank_row_count.ToString();
                                drow["Subject Code"] = ds3.Tables[0].Rows[i]["Rollno"].ToString();

                                drow["Student Strength"] = ds3.Tables[0].Rows[i]["total"].ToString();
                                drow["Appear"] = "Rank " + rank_row_count;

                                string roll_no = ds3.Tables[0].Rows[i]["Rollno"].ToString();
                                ds2.Tables[0].DefaultView.RowFilter = "roll='" + ds3.Tables[0].Rows[i]["Rollno"].ToString() + "'";
                                DataView dvstuude = ds2.Tables[0].DefaultView;
                                if (dvstuude.Count > 0)
                                {
                                    drow["Subject Name"] = dvstuude[0]["regno"].ToString();
                                    drow["Staff Name"] = dvstuude[0]["studname"].ToString();
                                    drow["Subject Type"] = dvstuude[0]["studtype"].ToString();

                                    data.Rows.Add(drow);
                                }
                            }
                            else
                            {
                                i = ds3.Tables[0].Rows.Count;
                            }
                        }

                    }
                }
                //==============================Chart=============================
                Chart1.DataSource = dt2;
                Chart1.DataBind();
                Chart1.Visible = true;
                Chart1.Enabled = false;
                Chart1.ChartAreas[0].AxisX.RoundAxisValues();
                Chart1.ChartAreas[0].AxisX.Minimum = 0;
                Chart1.ChartAreas[0].AxisX.Interval = 1;
                Chart1.Series["Series1"].IsValueShownAsLabel = true;
                Chart1.Series[0].ChartType = SeriesChartType.Column;
                Chart1.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                Chart1.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                Chart1.ChartAreas[0].AxisX.Title = "SUBJECT NAME";
                Chart1.ChartAreas[0].AxisY.Title = "PASS%";
                Chart1.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
                Chart1.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
                Chart1.Series["Series1"].XValueMember = "SUBJECT NAME";
                Chart1.Series["Series1"].YValueMembers = "PASS";
                Chart1.ChartAreas[0].AxisX.TitleForeColor = System.Drawing.Color.Black;
                Chart1.ChartAreas[0].AxisY.TitleForeColor = System.Drawing.Color.Black;
                Chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = System.Drawing.Color.Black;
                Chart1.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Verdana", 8f);
                Chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = System.Drawing.Color.Black;

                if (data.Columns.Count > 0 && data.Rows.Count > 1)
                {


                    Showgrid.DataSource = data;
                    Showgrid.DataBind();
                    Showgrid.Visible = true;
                    divMainContents.Visible = true;

                    int rcnt = data.Rows.Count - rankcnt;
                    int colcnt = data.Columns.Count;
                    Showgrid.Rows[sno + 2].Cells[0].ColumnSpan = colcnt;
                    for (int a = 1; a < colcnt; a++)
                        Showgrid.Rows[sno + 2].Cells[a].Visible = false;

                    Showgrid.Rows[rcnt].Cells[0].ColumnSpan = colcnt;
                    Showgrid.Rows[rcnt].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    Showgrid.Rows[rcnt].Cells[0].Font.Bold = true;
                    Showgrid.Rows[rcnt + 1].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    Showgrid.Rows[rcnt + 1].Cells[0].Font.Bold = true;
                    for (int a = 1; a < colcnt; a++)
                    {
                        Showgrid.Rows[rcnt].Cells[a].Visible = false;
                        Showgrid.Rows[rcnt + 1].Cells[a].HorizontalAlign = HorizontalAlign.Center;
                        Showgrid.Rows[rcnt + 1].Cells[a].Font.Bold = true;
                    }


                    for (int c = 0; c < 14; c++)
                    {
                        if (chklscolumn.Items[c].Selected == false)
                            for (int r = 0; r < data.Rows.Count; r++)
                                Showgrid.Rows[r].Cells[c].Visible = false;
                    }
                    if (chklscolumn.Items[0].Selected == true)
                    {
                        int snocnt = sno + 3;
                        if (rowcnt > 0)
                        {
                            for (int g = snocnt; g < rowcnt + snocnt; g++)
                            {
                                Showgrid.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                                Showgrid.Rows[g].Cells[0].ColumnSpan = 6;
                                Showgrid.Rows[g].Cells[0].Font.Bold = true;
                                for (int a = 1; a < 6; a++)
                                    Showgrid.Rows[g].Cells[a].Visible = false;
                            }
                        }
                        if (rowcnt1 > 0)
                        {
                            for (int g = snocnt + rowcnt; g < rowcnt1 + snocnt + rowcnt; g++)
                            {
                                Showgrid.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                                Showgrid.Rows[g].Cells[0].ColumnSpan = 8;
                                Showgrid.Rows[g].Cells[0].Font.Bold = true;
                                for (int a = 1; a < 8; a++)
                                    Showgrid.Rows[g].Cells[a].Visible = false;
                            }
                        }
                        if (toprank)
                        {
                            Showgrid.Rows[rowcnt1 + snocnt + rowcnt].Cells[0].ColumnSpan = colcnt;

                            for (int a = 1; a < colcnt; a++)
                                Showgrid.Rows[rowcnt1 + snocnt + rowcnt].Cells[a].Visible = false;
                            for (int c = 0; c < 6; c++)
                                Showgrid.Rows[rowcnt1 + snocnt + rowcnt + 1].Cells[c].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }



                    int rowcn = Showgrid.Rows.Count - 2;
                    //Rowspan
                    for (int rowIndex = Showgrid.Rows.Count - rowcn - 1; rowIndex >= 0; rowIndex--)
                    {
                        GridViewRow row1 = Showgrid.Rows[rowIndex];
                        GridViewRow previousRow = Showgrid.Rows[rowIndex + 1];
                        Showgrid.Rows[rowIndex].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        Showgrid.Rows[rowIndex].Font.Bold = true;
                        Showgrid.Rows[rowIndex].HorizontalAlign = HorizontalAlign.Center;

                        for (int i = 0; i < row1.Cells.Count; i++)
                        {
                            if (row1.Cells[i].Text == previousRow.Cells[i].Text)
                            {
                                row1.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                                       previousRow.Cells[i].RowSpan + 1;
                                previousRow.Cells[i].Visible = false;
                            }

                        }


                    }

                    //ColumnSpan
                    for (int rowIndex = Showgrid.Rows.Count - rowcn - 1; rowIndex >= 0; rowIndex--)
                    {
                        for (int cell = Showgrid.Rows[rowIndex].Cells.Count - 1; cell > 0; cell--)
                        {
                            TableCell colum = Showgrid.Rows[rowIndex].Cells[cell];
                            TableCell previouscol = Showgrid.Rows[rowIndex].Cells[cell - 1];
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

                    }
                }
            }
            else
            {

                Showgrid.Visible = false;
                btnprint.Visible = false;
                btndirectprt.Visible = false;
                btnexcel.Visible = false;
                lblexcel.Visible = false;
                txtexcelname.Visible = false;
                lblerror.Text = "No Records Found";
                lblerror.Visible = true;
            }

        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string ss = Session["usercode"].ToString();
        Printcontrol.loadspreaddetails(Showgrid, "FacultyPerformance.aspx", ddltest.SelectedItem.ToString() + " - RESULT ANALYSIS$DEPARTMENT OF " + ddlbranch.SelectedItem.ToString() + "", 0, ss);
        Printcontrol.Visible = true;
    }

    protected void btnexcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(Showgrid, reportname);

            }
            else
            {
                lblerror.Text = "Please Enter Your Report Name";
                lblerror.Visible = true;
            }
            txtexcelname.Text = string.Empty;
            reportname = string.Empty;
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void chklscolumn_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void ddltest_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }


    protected void Showgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                    e.Row.Cells[grCol].Visible = false;


                int c1 = 0;
                int c2 = 0;
                int c3 = 0;
                for (int c = 0; c < 6; c++)
                {
                    if (chklscolumn.Items[c].Selected == true)
                    {
                        c1++;
                        e.Row.Cells[c].HorizontalAlign = HorizontalAlign.Center;
                    }
                    else
                        e.Row.Cells[c].Visible = false;
                }
                for (int c = 6; c < 10; c++)
                {
                    if (chklscolumn.Items[c].Selected == true)
                    {
                        e.Row.Cells[c].HorizontalAlign = HorizontalAlign.Center;
                        c2++;
                    }
                    else
                        e.Row.Cells[c].Visible = false;
                }
                for (int c = 10; c < 14; c++)
                {
                    if (chklscolumn.Items[c].Selected == true)
                    {
                        e.Row.Cells[c].HorizontalAlign = HorizontalAlign.Center;
                        c3++;
                    }
                    else
                        e.Row.Cells[c].Visible = false;
                }

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (chklscolumn.Items[0].Selected == true)
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                for (int c = 5; c < 14; c++)
                {
                    if (chklscolumn.Items[c].Selected == true)
                    {
                        e.Row.Cells[c].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }

        }
        catch
        {


        }

    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    public void btnPrint11()
    {
        DAccess2 ddd2 = new DAccess2();
        string college_code = Convert.ToString(Session["collegecode"].ToString());
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = ddd2.select_method_wo_parameter(colQ, "Text");
        string collegeName = string.Empty;
        string collegeCateg = string.Empty;
        string collegeAff = string.Empty;
        string collegeAdd = string.Empty;
        string collegePhone = string.Empty;
        string collegeFax = string.Empty;
        string collegeWeb = string.Empty;
        string collegeEmai = string.Empty;
        string collegePin = string.Empty;
        string acr = string.Empty;
        string City = string.Empty;
        if (dsCol.Tables.Count > 0 && dsCol.Tables[0].Rows.Count > 0)
        {
            collegeName = Convert.ToString(dsCol.Tables[0].Rows[0]["Collname"]);
            City = Convert.ToString(dsCol.Tables[0].Rows[0]["address3"]);
            collegeAff = "(Affiliated to " + Convert.ToString(dsCol.Tables[0].Rows[0]["university"]) + ")";
            collegeAdd = Convert.ToString(dsCol.Tables[0].Rows[0]["address1"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["address2"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["district"]) + " - " + Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePin = Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePhone = "OFFICE: " + Convert.ToString(dsCol.Tables[0].Rows[0]["phoneno"]);
            collegeFax = "FAX: " + Convert.ToString(dsCol.Tables[0].Rows[0]["faxno"]);
            collegeWeb = "Website: " + Convert.ToString(dsCol.Tables[0].Rows[0]["website"]);
            collegeEmai = "E-Mail: " + Convert.ToString(dsCol.Tables[0].Rows[0]["email"]);
            collegeCateg = "(" + Convert.ToString(dsCol.Tables[0].Rows[0]["category"]) + ")";
        }
        DateTime dt = DateTime.Now;
        int year = dt.Year;
        spCollegeName.InnerHtml = collegeName;
        spAddr.InnerHtml = collegeAdd;
        spDegreeName.InnerHtml = acr;
        spReportName.InnerHtml = "Internal Result Analysis";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }
}