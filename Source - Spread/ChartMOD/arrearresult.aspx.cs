using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.Configuration;

public partial class arrearresult : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();

    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet sec1 = new DataSet();
    DataSet syllbus = new DataSet();

    Hashtable hat = new Hashtable();
    Hashtable ht = new Hashtable();

    Institution institute;

    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string subject = string.Empty;
    string sec = string.Empty;
    string present_Count = string.Empty;

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        //****************************************************//
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("ChartMaster"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/ChartMOD/ChartMaster.aspx");
                return;
            }
        }
        //****************************************************//
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();

        if (!IsPostBack)
        {
            //lblerrmsg.Visible = false;
            setLabelText();
            bindbatch();
            college();
            bindcourse();
            bindbranch(ddlcollege.SelectedItem.Value);
            bindsem();
            BindSectionDetail();
            test();
            Chart1.Visible = false;
            fpstudentdetails.Visible = false;
        }
        lblerrmsg.Visible = false;
    }

    #endregion Page Load

    #region Bind Header

    public void college()
    {
        try
        {
            ddlcollege.Items.Insert(0, "All");
            ds = da.select_method_wo_parameter("select collname,college_code,acr from collinfo", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindbatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            //ds = da.select_method_wo_parameter("bind_batch", "sp");
            ds = da.BindBatch();
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }
            //int count1 = ds.Tables[1].Rows.Count;
            //if (count > 0)
            //{
            //    int max_bat = 0;
            //    max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
            //    ddlbatch.SelectedValue = max_bat.ToString();

            //}
        }
        catch (Exception ex)
        {
        }
    }

    public void bindcourse()
    {
        try
        {
            //CheckBoxListdegree.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = ddlcollege.SelectedItem.Value;
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ht.Clear();
            ht.Add("single_user", singleuser);
            ht.Add("group_code", group_user);
            ht.Add("college_code", collegecode);
            ht.Add("user_code", usercode);
            ds = da.select_method("bind_degree", ht, "sp");
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

        }

    }

    public void bindbranch(string branch)
    {
        try
        {
            string commname = "";


            commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + ddldegree.SelectedItem.Value + "') and deptprivilages.Degree_code=degree.Degree_code ";


            //commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";

            {
                ds = da.select_method_wo_parameter(commname, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddldept.DataSource = ds;
                    ddldept.DataTextField = "dept_name";
                    ddldept.DataValueField = "degree_code";
                    ddldept.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindsem()
    {
        try
        {
            //--------------------semester load
            ddlsem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;

            string sqluery = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddldept.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and college_code=" + ddlcollege.SelectedItem.Value + "";

            ds3 = da.select_method_wo_parameter(sqluery, "text");
            if (ds3.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds3.Tables[0].Rows[0]["first_year_nonsemester"]);
                duration = Convert.ToInt16(ds3.Tables[0].Rows[0]["ndurations"]);
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


                sqluery = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddldept.SelectedValue.ToString() + " and college_code=" + ddlcollege.SelectedItem.Value + "";
                ddlsem.Items.Clear();
                ds3 = da.select_method_wo_parameter(sqluery, "text");
                if (ds3.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(ds3.Tables[0].Rows[0]["first_year_nonsemester"]);
                    duration = Convert.ToInt16(ds3.Tables[0].Rows[0]["duration"]);
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


            }


        }
        catch
        {
        }
    }

    public void BindSectionDetail()
    {
        try
        {
            if (ddlsem.Text != "")
            {
                string branch = ddldept.SelectedValue.ToString();
                string batch = ddlbatch.SelectedValue.ToString();

                string sqlquery = "select distinct sections from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddldept.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'";


                sec1 = da.select_method_wo_parameter(sqlquery, "text");
                if (sec1.Tables[0].Rows.Count > 0)
                {
                    DropDownList1.DataSource = sec1;
                    DropDownList1.DataTextField = "sections";
                    DropDownList1.DataValueField = "sections";
                    DropDownList1.DataBind();
                }
                //ddlSec.Items.Insert(0, new ListItem("--Select--", "-1"));

                if (sec1.Tables[0].Rows.Count > 0)
                {
                    if (sec1.Tables[0].Rows[0]["sections"].ToString() == "")
                    {
                        DropDownList1.Enabled = false;

                    }
                    else
                    {
                        DropDownList1.Enabled = true;
                        //RequiredFieldValidator5.Visible = true;
                    }
                }
                else
                {
                    DropDownList1.Enabled = false;

                }
            }

        }
        catch
        {
        }
    }

    public void test()
    {
        try
        {
            if (radiobutton1.Text == "CAM Wise")
            {
                DataSet syllbus1 = new DataSet();
                string SyllabusYr = "";
                string SyllabusQry = "select distinct syllabus_year from syllabus_master where degree_code ='" + ddldept.SelectedItem.Value + "' and batch_year ='" + ddlbatch.SelectedItem.Value + "'";
                syllbus = da.select_method_wo_parameter(SyllabusQry, "text");
                DropDownList2.Items.Clear();

                if (syllbus.Tables.Count > 0 && syllbus.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < syllbus.Tables[0].Rows.Count; i++)
                    {
                        if (SyllabusYr == "")
                        {
                            SyllabusYr = syllbus.Tables[0].Rows[i]["syllabus_year"].ToString();
                        }
                        else
                        {
                            SyllabusYr = SyllabusYr + "," + syllbus.Tables[0].Rows[i]["syllabus_year"].ToString();
                        }

                    }
                }


                if (SyllabusYr != "")
                {
                    if (ddlsem.Text != "")
                    {
                        string strsql = "select distinct criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code='" + ddldept.SelectedItem.Value + "' and syllabus_year in(" + SyllabusYr + ") and batch_year ='" + ddlbatch.SelectedItem.Text + "' and criteria != '' and semester='" + ddlsem.SelectedItem.Value + "' order by criteria";
                        syllbus1 = da.select_method_wo_parameter(strsql, "Text");
                        // DropDownList2.Items.Clear();
                        if (syllbus1.Tables[0].Rows.Count > 0)
                        {
                            DropDownList2.DataSource = syllbus1;
                            DropDownList2.DataTextField = "criteria";
                            DropDownList2.DataValueField = "criteria_no";
                            DropDownList2.DataBind();
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    #endregion Bind Header

    public void chartbind()
    {
        try
        {
            ArrayList degree_code = new ArrayList();
            hat.Clear();
            DataView dv1 = new DataView();
            DataView dv = new DataView();
            DataSet exam = new DataSet();
            Chart1.Visible = true;
            Boolean ra = new Boolean();
            if (radiobutton1.Text == "University Wise")
            {
                string sqlquery = " select (c.Course_Name+'-'+dp.dept_acronym) as dept,r.current_semester,r.degree_code from registration r,degree de,course c,department dp,deptprivilages dv where  r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.exam_flag<>'debar' and r.college_code=" + collegecode + " and r.degree_code=dv.degree_code  and dv.Degree_code=de.Degree_code and  user_code=" + usercode + " and r.Batch_Year=" + ddlbatch.Text + " group by r.batch_year,r.degree_code,course_name,dept_acronym,current_semester  order by  c.Course_Name ASC ,r.degree_code ASC";
                string sqlqurty2 = " select distinct r.current_semester from registration r,degree de,course c,department dp,deptprivilages dv,seminfo where  r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.exam_flag<>'debar' and r.college_code=" + collegecode + " And r.current_semester = seminfo.semester  and r.degree_code=seminfo.degree_code and r.batch_year=seminfo.batch_year  and  r.degree_code=dv.degree_code and dv.Degree_code=de.Degree_code and  user_code=" + usercode + " and r.Batch_Year=" + ddlbatch.Text + "";
                ds = da.select_method_wo_parameter(sqlquery, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string degree = ds.Tables[0].Rows[i]["degree_code"].ToString();
                        string current_sem = ds.Tables[0].Rows[i]["current_semester"].ToString();
                        if (!degree_code.Contains(current_sem))
                        {
                            degree_code.Add(current_sem);
                        }
                    }
                    for (int k = 0; k < degree_code.Count; k++)
                    {
                        string current_sem1 = degree_code[k].ToString();
                        ds.Tables[0].DefaultView.RowFilter = "current_semester='" + current_sem1 + "'";
                        dv = ds.Tables[0].DefaultView;
                        string degreecode = " ";
                        //string sqlqurty = "select distinct subject_no from mark_entry where exam_code in (select exam_code from Exam_Details  where degree_code in('" + ddldept.SelectedItem.Value + "') and batch_year='" + ddlbatch.Text + "' and current_semester<='" + current_sem1 + "') order by subject_no ";

                        string sqlqurty = "select distinct m.subject_no from mark_entry m,subjectChooser sc where m.roll_no=sc.roll_no and sc.subject_no=m.subject_no and  exam_code in (select exam_code from Exam_Details  where degree_code in('" + ddldept.SelectedItem.Value + "') and batch_year='" + ddlbatch.Text + "' and current_semester<='" + current_sem1 + "') order by m.subject_no ";

                        //sqlqurty = " select distinct m.subject_no from mark_entry m,subject s,syllabus_master sm where exam_code in (select exam_code from Exam_Details  where degree_code in('" + ddldept.SelectedItem.Value + "') and batch_year='" + ddlbatch.Text + "' and current_semester<='" + current_sem1 + "') and s.syll_code=sm.syll_code and s.subject_no=m.subject_no and sm.Batch_Year='" + ddlbatch.Text + "' and sm.degree_code in('" + ddldept.SelectedItem.Value + "') and sm.semester<='" + current_sem1 + "'  order by m.subject_no ";

                        ds1 = da.select_method_wo_parameter(sqlqurty, "text");
                        subject = "";
                        if (ds1.Tables[0].Rows.Count > 0)
                        {

                            for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                            {
                                if (subject.ToString() == "")
                                {
                                    subject = ds1.Tables[0].Rows[j]["subject_no"].ToString();
                                }
                                else
                                {
                                    subject = subject + "," + ds1.Tables[0].Rows[j]["subject_no"].ToString();
                                }
                            }
                        }
                    }
                    if (subject.ToString() != "")
                    {
                        //string sqlallpass = "Select arr arrear,count(1) students from( Select count(roll_no) arr from (Select distinct m.roll_no as roll_no,subject_no from mark_entry m,  registration r where r.roll_no=m.roll_no and r.delflag=0  and r.cc=0 and r.exam_flag<>'debar' and subject_no in (" + subject + ") and result='pass' ) as my_table group by roll_no ) as  count_table group by arr order by arr";
                        //ds3 = da.select_method_wo_parameter(sqlallpass, "text");
                        //if (ds3.Tables[0].Rows.Count > 0)
                        //{
                        //    present_Count = ds3.Tables[0].Rows[ds3.Tables[0].Rows.Count - 1]["arrear"].ToString();
                        //    if (present_Count == ds1.Tables[0].Rows.Count.ToString())
                        //    {
                        //        ds3.Tables[0].Rows[ds3.Tables[0].Rows.Count - 1]["arrear"] = "0";
                        //    }
                        //}
                        //string sqlquery2 = "Select arr arrear,count(1) students from( Select count(roll_no) arr from (select distinct m.subject_no,m.roll_no,r.degree_code,r.Batch_Year from mark_entry m,Registration r where m.roll_no=r.Roll_No and r.delflag=0  and r.cc=0 and r.exam_flag<>'debar' and r.Batch_Year=" + ddlbatch.SelectedValue + "  and degree_code=" + ddldept.SelectedValue + " and subject_no not in(select subject_no from mark_entry m1  where result='pass' and passorfail=1 and r.Batch_Year=" + ddlbatch.SelectedValue + " and degree_code=" + ddldept.SelectedValue + " and r.Roll_No=m1.roll_no)) as my_table group by roll_no ) as  count_table group by arr order by arr";
                        //ds2 = da.select_method_wo_parameter(sqlquery2, "text");
                        //if (present_Count == ds1.Tables[0].Rows.Count.ToString())
                        //{
                        //    ds2.Tables[0].Rows.Add("0", ds3.Tables[0].Rows[ds3.Tables[0].Rows.Count - 1]["students"]);
                        //}
                        //dv1 = ds2.Tables[0].DefaultView;
                        //dv1.Sort = "arrear ASC";
                        DataTable dt2 = new DataTable();
                        dt2.Columns.Add("arrear", typeof(double));
                        dt2.Columns.Add("students", typeof(double));
                        Hashtable hatstucoun = new Hashtable();
                        string strgetstudetails = "select r.Roll_No,r.Reg_No,r.Stud_Name,r.mode,isnull((select count(distinct m.subject_no) from mark_entry m ,subjectChooser sc where m.roll_no=sc.roll_no and sc.subject_no=m.subject_no and m.roll_no=r.Roll_No),'0') marksub,isnull((select count(distinct m1.subject_no) from mark_entry m1 ,subjectChooser sc where m1.roll_no=sc.roll_no and sc.subject_no=m1.subject_no and m1.roll_no=r.Roll_No and m1.result='Pass' and passorfail=1),'0') passsub from Registration r where r.Batch_Year='" + ddlbatch.SelectedValue + "' and r.degree_code='" + ddldept.SelectedValue + "' and DelFlag=0 and Exam_Flag<>'debar' order by r.Reg_No";
                        //strgetstudetails = "  select r.Roll_No,r.Reg_No,r.Stud_Name,r.mode,isnull((select count(distinct m.subject_no) from mark_entry m ,subject s,syllabus_master sm  where s.syll_code=sm.syll_code and s.subject_no=m.subject_no and sm.Batch_Year='" + ddlbatch.SelectedValue + "' and sm.degree_code in('" + ddldept.SelectedValue + "') and sm.semester<='3' and m.roll_no=r.Roll_No),'0') marksub,(select count(distinct m1.subject_no) from mark_entry m1,subject s,syllabus_master sm  where m1.roll_no=r.Roll_No and m1.result='Pass' and passorfail=1 and s.syll_code=sm.syll_code and s.subject_no=m1.subject_no and sm.Batch_Year='2015' and sm.degree_code in('54') and sm.semester<='3') passsub from Registration r where r.Batch_Year='2015' and r.degree_code='54' and DelFlag=0 and Exam_Flag<>'debar' order by r.Reg_No";

                        DataSet dsarrdetails = da.select_method_wo_parameter(strgetstudetails, "Text");
                        for (int i = 0; i < dsarrdetails.Tables[0].Rows.Count; i++)
                        {
                            int nosuball = Convert.ToInt32(dsarrdetails.Tables[0].Rows[i]["marksub"].ToString());
                            if (nosuball > 0)
                            {
                                int noofsubpass = Convert.ToInt32(dsarrdetails.Tables[0].Rows[i]["passsub"].ToString());
                                int difsub = nosuball - noofsubpass;
                                if (hatstucoun.Contains(difsub))
                                {
                                    int noofstureg = Convert.ToInt32(hatstucoun[difsub]);
                                    noofstureg++;
                                    hatstucoun[difsub] = noofstureg;
                                }
                                else
                                {
                                    hatstucoun.Add(difsub, 1);
                                }
                            }
                        }

                        for (int so = 0; so < ds1.Tables[0].Rows.Count; so++)
                        {
                            if (hatstucoun.Contains(so))
                            {
                                DataRow dr1 = dt2.NewRow();
                                int nofost = Convert.ToInt32(hatstucoun[so]);
                                dr1[0] = so;
                                dr1[1] = nofost;
                                dt2.Rows.Add(dr1);
                            }
                        }
                        dv1 = dt2.DefaultView;
                        dv1.Sort = "arrear ASC";
                    }
                }

                if (dv1.Count > 0)
                {
                    Chart1.DataSource = dv1;
                    Chart1.ChartAreas[0].AxisX.Title = "No.Of.Arrear";
                    Chart1.ChartAreas[0].AxisY.Title = "No.Of.Students";
                    Chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Blue;
                    Chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Blue;
                    Chart1.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
                    Chart1.ChartAreas[0].AxisY.TitleAlignment = StringAlignment.Center;
                    Chart1.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Book Antiqua", 15, FontStyle.Bold);
                    Chart1.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Book Antiqua", 15, FontStyle.Bold);
                    Chart1.Series["Series1"].IsValueShownAsLabel = true;
                    Chart1.Series["Series1"].Font = new System.Drawing.Font("Trebuchet MS", 9, FontStyle.Bold);
                    Chart1.ChartAreas[0].AxisX.LabelStyle.Font.Bold.ToString();
                    Chart1.ChartAreas[0].AxisY.LabelStyle.Font.Bold.ToString();
                    Chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                    Chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                    Chart1.Series["Series1"].Color = Color.BlueViolet;
                    Chart1.ChartAreas[0].AxisX.Interval = 1;
                    Chart1.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Book Antiqua", 8, FontStyle.Bold);
                    Chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Blue;
                    Chart1.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Book Antiqua", 8, FontStyle.Bold);
                    Chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Blue;
                    Chart1.Series["Series1"].Font = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);

                    Random random = new Random();
                    for (int k = 0; k < dv1.Count; k++)
                    {
                        Chart1.Series["Series1"].Points.AddXY(dv1[k]["arrear"], dv1[k]["students"]);
                    }
                    int f = 0;

                    Boolean all_flag = false;
                    foreach (Series series in Chart1.Series)
                    {
                        foreach (DataPoint point in series.Points)
                        {
                            string data = point.ToString();
                            string[] spl_date1 = data.Split(new char[] { ',' });
                            string data2 = spl_date1[1].ToString();
                            string[] spl_date2 = data2.Split(new char[] { '=', '}' });
                            if (all_flag == false)
                            {
                                if (dv1[0]["arrear"].ToString() == "0")
                                {
                                    Chart1.Series["Series1"].Points[f].Color = Color.Green;
                                }
                                else
                                {
                                    point.Color = Color.Red;
                                }
                                all_flag = true;
                            }
                            else
                            {
                                point.Color = Color.Red;
                            }
                        }
                    }
                    Chart1.DataBind();
                }

                else
                {
                    lblerrmsg.Text = "No Records Found";
                    lblerrmsg.Visible = true;
                    Chart1.Visible = false;
                }
            }
            else
            {
                if (DropDownList2.Text == "")
                {
                    lblerrmsg.Text = "Please Select One Test";
                    lblerrmsg.Visible = true;
                    Chart1.Visible = false;
                }
                else
                {
                    string sqldata = "select distinct e.criteria_no,c.criteria,e.subject_no,sub.subject_name from Exam_type e,CriteriaForInternal c,subject sub,Desig_Master dm where  e.criteria_no=c.Criteria_no and sub.subject_no=e.subject_no   and c.Criteria_no='" + DropDownList2.SelectedItem.Value + "'";
                    ds = da.select_method_wo_parameter(sqldata, "text");
                    if (DropDownList1.Enabled == true)
                    {
                        if (DropDownList1.Text == "")
                        {
                            sec = "";
                        }
                        else
                        {
                            sec = "and re.sections='" + DropDownList1.SelectedItem.Value + "'";
                        }
                    }
                    subject = "";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (subject == "")
                            {
                                subject = ds.Tables[0].Rows[i]["subject_no"].ToString();
                            }
                            else
                            {
                                subject = subject + "," + ds.Tables[0].Rows[i]["subject_no"].ToString();
                            }
                        }
                    }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        double cammark1;
                        DataSet ds1 = new DataSet();
                        string subject_no = "";
                        string sqlallpass = "Select arr arrear,count(1) students from( Select count(roll_no) arr from (select distinct c.criteria,c.criteria_no,r.marks_obtained,s.acronym,e.subject_no,s.subject_name,e.min_mark,e.max_mark,re.Roll_No from criteriaforinternal c, result r,exam_type e,subject s,syllabus_master sn,Registration re where re.Roll_No=r.roll_no and delflag=0 and exam_flag<>'debar'and r.exam_code=e.exam_code and c.Criteria_no=e.criteria_no and e.batch_year='" + ddlbatch.SelectedItem.Value + "' " + sec + "  and c.Criteria_no  in('" + DropDownList2.SelectedItem.Value + "')  and s.subject_no=e.subject_no and marks_obtained >= e.min_mark ) as my_table group by roll_no ) as  count_table group by arr order by arr";
                        ds3 = da.select_method_wo_parameter(sqlallpass, "text");
                        ds3.Tables[0].DefaultView.RowFilter = "arrear='" + ds.Tables[0].Rows.Count + "'";
                        dv = ds3.Tables[0].DefaultView;
                        ds.Tables.Clear();
                        ds.Tables.Add(dv.ToTable());

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ds.Tables[0].Rows[0]["arrear"] = "0";
                            ra = true;
                        }

                        string sqlquery1 = "Select arr arrear,count(1) students from( Select count(roll_no) arr from (select distinct c.criteria,c.criteria_no,r.marks_obtained,s.acronym,e.subject_no,s.subject_name,e.min_mark,e.max_mark,re.Roll_No from criteriaforinternal c, result r,exam_type e,subject s,syllabus_master sn,Registration re where re.Roll_No=r.roll_no and delflag=0 and exam_flag<>'debar'and r.exam_code=e.exam_code and c.Criteria_no=e.criteria_no and e.batch_year='" + ddlbatch.SelectedItem.Value + "'  " + sec + "  and c.Criteria_no  in('" + DropDownList2.SelectedItem.Value + "')  and s.subject_no=e.subject_no and marks_obtained < e.min_mark ) as my_table group by roll_no ) as  count_table group by arr order by arr";
                        ds2 = da.select_method_wo_parameter(sqlquery1, "text");
                        if (ds2.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                            {
                                ds.Tables[0].Rows.Add(ds2.Tables[0].Rows[i]["arrear"], ds2.Tables[0].Rows[i]["students"]);
                                ra = true;
                            }
                        }
                    }
                    if (ra == true)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Chart1.DataSource = dv1;
                            Chart1.ChartAreas[0].AxisX.Title = "No.Of.Arrear";
                            Chart1.ChartAreas[0].AxisY.Title = "No.Of.Students";
                            Chart1.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
                            Chart1.ChartAreas[0].AxisY.TitleAlignment = StringAlignment.Center;
                            Chart1.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Book Antiqua", 15, FontStyle.Bold);
                            Chart1.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Book Antiqua", 15, FontStyle.Bold);
                            Chart1.Series["Series1"].IsValueShownAsLabel = true;
                            Chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Blue;
                            Chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Blue;
                            Chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                            Chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                            Chart1.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Book Antiqua", 10, FontStyle.Bold);
                            Chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Blue;
                            Chart1.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Book Antiqua", 10, FontStyle.Bold);
                            Chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Blue;
                            Chart1.Series["Series1"].Color = Color.BlueViolet;
                            Chart1.ChartAreas[0].AxisX.Interval = 1;

                            Chart1.Series["Series1"].Font = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
                            Random random = new Random();
                            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                            {
                                Chart1.Series["Series1"].Points.AddXY(ds.Tables[0].Rows[k]["arrear"].ToString(), ds.Tables[0].Rows[k]["students"]);
                            }
                            int f = 0;

                            Boolean all_flag = false;
                            foreach (Series series in Chart1.Series)
                            {
                                foreach (DataPoint point in series.Points)
                                {
                                    string data = point.ToString();
                                    string[] spl_date1 = data.Split(new char[] { ',' });
                                    string data2 = spl_date1[1].ToString();
                                    string[] spl_date2 = data2.Split(new char[] { '=', '}' });
                                    // int Y = Convert.ToInt32(spl_date2[1].ToString());

                                    if (all_flag == false)
                                    {
                                        if (ds.Tables[0].Rows[0]["arrear"].ToString() == "0")
                                        {
                                            Chart1.Series["Series1"].Points[f].Color = Color.Green;
                                        }
                                        else
                                        {
                                            point.Color = Color.Red;
                                        }
                                        all_flag = true;
                                    }
                                    else
                                    {
                                        point.Color = Color.Red;
                                    }
                                    f++;
                                }
                            }
                            Chart1.DataBind();
                        }
                    }
                    else
                    {
                        lblerrmsg.Text = "No Records Found";
                        lblerrmsg.Visible = true;
                        Chart1.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = ex.ToString();
            lblerrmsg.Visible = true;
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindcourse();
            bindbranch(ddlcollege.SelectedItem.Value);

            bindsem();
            BindSectionDetail();
            test();
            fpstudentdetails.Visible = false;
        }
        catch
        {
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch(ddlcollege.SelectedItem.Value);
            bindsem();
            BindSectionDetail();
            test();
            fpstudentdetails.Visible = false;
        }
        catch
        {
        }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch(ddlcollege.SelectedItem.Value);
            bindsem();
            BindSectionDetail();
            test();
            fpstudentdetails.Visible = false;
        }
        catch
        {
        }
    }

    protected void ddldept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsem();
            BindSectionDetail();
            test();
            fpstudentdetails.Visible = false;
        }
        catch
        {
        }
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            test();
            fpstudentdetails.Visible = false;
        }
        catch
        {
        }
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindSectionDetail();
            test();
            fpstudentdetails.Visible = false;
        }
        catch
        {
        }
    }

    protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            fpstudentdetails.Visible = false;
        }
        catch
        {
        }
    }

    protected void radiobutton1_selectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            if (radiobutton1.Text == "University Wise")
            {
                lbltest.Visible = false;
                DropDownList2.Visible = false;
                lblsem.Visible = false;
                ddlsem.Visible = false;
                DropDownList1.Visible = false;
                Label3.Visible = false;
                fpstudentdetails.Visible = false;
            }
            else
            {
                lbltest.Visible = true;
                DropDownList2.Visible = true;
                lblsem.Visible = true;
                ddlsem.Visible = true;
                DropDownList1.Visible = true;
                Label3.Visible = true;
                fpstudentdetails.Visible = false;
                test();
            }
        }
        catch
        {
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            chartbind();
            fpstudentdetails.Visible = false;
        }
        catch (Exception ex)
        {
            lblerrmsg.Visible = true;
            lblerrmsg.Text = ex.ToString();
        }
    }

    protected void Chart1_Click(object sender, ImageMapEventArgs e)
    {
        try
        {
            chartbind();
            Boolean falg_dd = false;
            string dd = e.PostBackValue;
            fpstudentdetails.Sheets[0].RowCount = 0;
            fpstudentdetails.Sheets[0].ColumnCount = 4;
            fpstudentdetails.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            fpstudentdetails.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            fpstudentdetails.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpstudentdetails.Sheets[0].RowHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            fpstudentdetails.Height = 310;
            fpstudentdetails.Width = 600;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = Color.Black;
            style2.BackColor = ColorTranslator.FromHtml("#0CA6CA");

            //  fpcammarkstaff.VerticalScrollBarPolicy = ScrollBarPolicy.AsNeeded;
            fpstudentdetails.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            fpstudentdetails.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

            fpstudentdetails.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            fpstudentdetails.Sheets[0].AllowTableCorner = true;
            fpstudentdetails.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            fpstudentdetails.Sheets[0].AllowTableCorner = true;
            fpstudentdetails.Sheets[0].AllowTableCorner = true;
            // fpcammarkstaff.Sheets[0].AutoPostBack = true;

            fpstudentdetails.Sheets[0].Columns[0].Width = 50;
            fpstudentdetails.Sheets[0].Columns[1].Width = 100;
            fpstudentdetails.Sheets[0].Columns[2].Width = 200;
            fpstudentdetails.Sheets[0].Columns[3].Width = 200;

            fpstudentdetails.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            fpstudentdetails.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
            fpstudentdetails.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Subject Name";
            fpstudentdetails.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            string[] subject_split = subject.Split(',');
            int subject_count = subject_split.Length;
            string sqlquery = "";
            if (radiobutton1.Text == "University Wise")
            {
                if (dd.ToString() == "0")
                {
                    falg_dd = true;
                    dd = present_Count;
                    dd = "0";
                    sqlquery = "Select count(roll_no) arr,roll_no from (Select distinct m.roll_no as roll_no,subject_no from mark_entry m,  registration r where r.roll_no=m.roll_no and r.delflag=0 and r.exam_flag<>'debar' and subject_no in (" + subject + ") and result='pass' and passorfail=1 ) as my_table group by roll_no ";
                }
                else
                {
                    sqlquery = "Select count(roll_no) arr,roll_no  from (select distinct m.subject_no,m.roll_no,r.degree_code,r.Batch_Year from mark_entry m,Registration r where m.roll_no=r.Roll_No and r.delflag=0 and r.exam_flag<>'debar' and r.Batch_Year=" + ddlbatch.SelectedValue + "  and degree_code=" + ddldept.SelectedValue + " and subject_no not in(select subject_no from mark_entry m1  where result='pass' and passorfail=1 and r.Batch_Year=" + ddlbatch.SelectedValue + " and degree_code=" + ddldept.SelectedValue + " and r.Roll_No=m1.roll_no))as my_table group by roll_no";
                    sqlquery = "Select count(roll_no) arr,roll_no  from (select distinct m.subject_no,m.roll_no,r.degree_code,r.Batch_Year from mark_entry m,Registration r,subjectChooser sc where m.roll_no=r.Roll_No and sc.roll_no=m.roll_no and m.subject_no=sc.subject_no and m.roll_no=r.Roll_No and r.delflag=0 and r.exam_flag<>'debar' and r.Batch_Year='" + ddlbatch.SelectedValue + "'  and r.degree_code='" + ddldept.SelectedValue + "' and m.subject_no not in(select m1.subject_no from mark_entry m1  where m1.roll_no=sc.roll_no and sc.subject_no=m1.subject_no and result='pass' and passorfail=1 and r.Batch_Year='" + ddlbatch.SelectedValue + "' and degree_code='" + ddldept.SelectedValue + "' and r.Roll_No=m1.roll_no))as my_table group by roll_no";

                }
                ds = da.select_method_wo_parameter(sqlquery, "text");
                DataView dvcount = new DataView();
                if (dd != "0")
                {
                    ds.Tables[0].DefaultView.RowFilter = "arr='" + dd + "'";
                }
                else
                {
                    ds.Tables[0].DefaultView.RowFilter = "";
                }
                dvcount = ds.Tables[0].DefaultView;
                if (dvcount.Count > 0)
                {
                    int cn = 0;
                    string dataset = "";
                    for (int h = 0; h < dvcount.Count; h++)
                    {
                        cn++;
                        if (falg_dd == true)
                        {
                            // dataset = "select distinct m.roll_no,Stud_Name from mark_entry m,Registration r,subject where m.subject_no=subject.subject_no and r.delflag=0  and r.cc=0 and r.exam_flag<>'debar' and r.roll_no='" + dvcount[h]["roll_no"].ToString() + "' and r.Roll_No=m.roll_no and  subject.subject_no in (" + subject + ") and result='pass' group by m.roll_no,Stud_Name,m.subject_no,subject_name";
                            dataset = "select r.Roll_No,r.Stud_Name from Registration r where r.Batch_Year='" + ddlbatch.SelectedValue + "' and r.degree_code='" + ddldept.SelectedValue + "' and isnull((select count(distinct m.subject_no) from mark_entry m where m.roll_no=r.Roll_No),'0')-(select count(distinct m1.subject_no) from mark_entry m1 where m1.roll_no=r.Roll_No and m1.result='Pass' and passorfail=1)=0 and DelFlag=0 and Exam_Flag<>'debar' order by r.Reg_No";
                        }
                        else
                        {
                            //dataset = "select m.roll_no,Stud_Name,m.subject_no,subject_name from mark_entry m,Registration r,subject where m.subject_no=subject.subject_no and r.delflag=0 and r.exam_flag<>'debar' and r.Roll_No=m.roll_no and  subject.subject_no not in (select subject_no from mark_entry m1  where result='pass' and passorfail=1 and r.Batch_Year=" + ddlbatch.SelectedValue + " and degree_code=" + ddldept.SelectedValue + " and r.Roll_No=m1.roll_no) and m.roll_no='" + dvcount[h]["roll_no"].ToString() + "' group by m.roll_no,Stud_Name,m.subject_no,subject_name";
                            dataset = "select m.roll_no,Stud_Name,m.subject_no,subject_name from mark_entry m,Registration r,subject s,subjectChooser sc where sc.roll_no=r.Roll_No and sc.roll_no=m.roll_no and sc.subject_no=m.subject_no and sc.subject_no=s.subject_no and m.subject_no=sc.subject_no and r.delflag=0 and r.exam_flag<>'debar' and r.Roll_No=m.roll_no and  s.subject_no not in (select m1.subject_no from mark_entry m1  where m1.subject_no=sc.subject_no and m1.roll_no=sc.roll_no and result='pass' and passorfail=1 and r.Batch_Year='" + ddlbatch.SelectedValue + "' and r.degree_code='" + ddldept.SelectedValue + " ' and r.Roll_No=m1.roll_no) and m.roll_no='" + dvcount[h]["roll_no"].ToString() + "' group by m.roll_no,Stud_Name,m.subject_no,subject_name";
                        }
                        ds1 = da.select_method_wo_parameter(dataset, "text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            if (ds1.Tables[0].Columns.Count == 2)
                            {
                                for (int jsa = 0; jsa < ds1.Tables[0].Rows.Count; jsa++)
                                {
                                    h = dvcount.Count;
                                    fpstudentdetails.Sheets[0].RowCount++;
                                    fpstudentdetails.Sheets[0].ColumnHeader.Columns[3].Visible = false;
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 0].Text = cn.ToString();
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 1].Text = ds1.Tables[0].Rows[jsa]["roll_no"].ToString();
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 2].Text = ds1.Tables[0].Rows[jsa]["Stud_Name"].ToString();
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    cn++;
                                }
                            }
                            else
                            {
                                for (int jsa = 0; jsa < ds1.Tables[0].Rows.Count; jsa++)
                                {

                                    fpstudentdetails.Sheets[0].RowCount++;
                                    fpstudentdetails.Sheets[0].ColumnHeader.Columns[3].Visible = true;
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 0].Text = cn.ToString();
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 1].Text = ds1.Tables[0].Rows[jsa]["roll_no"].ToString();
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 2].Text = ds1.Tables[0].Rows[jsa]["Stud_Name"].ToString();
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 3].Text = ds1.Tables[0].Rows[jsa]["subject_name"].ToString();
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;


                                }
                            }
                        }

                    }
                    fpstudentdetails.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    fpstudentdetails.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

                    fpstudentdetails.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);

                    fpstudentdetails.Sheets[0].Columns[0].Locked = true;
                    fpstudentdetails.Sheets[0].Columns[1].Locked = true;
                    fpstudentdetails.Sheets[0].Columns[2].Locked = true;
                    fpstudentdetails.Sheets[0].Columns[3].Locked = true;
                    fpstudentdetails.Visible = true;
                    fpstudentdetails.Sheets[0].PageSize = fpstudentdetails.Sheets[0].RowCount;
                }
            }
            else if (radiobutton1.Text == "CAM Wise")
            {
                string dataset2 = "";
                if (dd.ToString() == "0")
                {
                    falg_dd = true;
                    dd = subject_count.ToString();
                    dataset2 = "Select count(roll_no) arr,Roll_No from (select distinct c.criteria,c.criteria_no,r.marks_obtained,s.acronym,e.subject_no,s.subject_name,e.min_mark,e.max_mark,re.Roll_No from criteriaforinternal c, result r,exam_type e,subject s,syllabus_master sn,Registration re where re.Roll_No=r.roll_no and delflag=0 and exam_flag<>'debar'and r.exam_code=e.exam_code and c.Criteria_no=e.criteria_no and e.batch_year='" + ddlbatch.SelectedItem.Value + "'  " + sec + "   and c.Criteria_no  in('" + DropDownList2.SelectedItem.Value + "')  and s.subject_no=e.subject_no and marks_obtained >= e.min_mark ) as my_table group by roll_no ";

                }
                else
                {
                    dataset2 = "select count(roll_no) arr,Roll_No from (select distinct c.criteria,c.criteria_no,r.marks_obtained,s.acronym,e.subject_no,s.subject_name,e.min_mark,e.max_mark,re.Roll_No from criteriaforinternal c, result r,exam_type e,subject s,syllabus_master sn,Registration re where re.Roll_No=r.roll_no and delflag=0 and exam_flag<>'debar'and r.exam_code=e.exam_code and c.Criteria_no=e.criteria_no and e.batch_year='" + ddlbatch.SelectedItem.Value + "'  " + sec + "  and c.Criteria_no  in('" + DropDownList2.SelectedItem.Value + "')  and s.subject_no=e.subject_no and marks_obtained < e.min_mark ) as my_table group by roll_no ";
                }
                ds = da.select_method_wo_parameter(dataset2, "text");
                DataView dvcount1 = new DataView();
                ds.Tables[0].DefaultView.RowFilter = "arr='" + dd + "'";
                dvcount1 = ds.Tables[0].DefaultView;
                string query = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int cn = 0;
                    for (int i = 0; i < dvcount1.Count; i++)
                    {
                        cn++;
                        if (falg_dd == true)
                        {
                            query = "select distinct r.roll_no,re.Stud_Name from criteriaforinternal c, result r,exam_type e,subject s,syllabus_master sn,Registration re where s.syll_code=sn.syll_code and c.syll_code=s.syll_code and re.Roll_No=r.roll_no and delflag=0 and exam_flag<>'debar'and sn.Batch_Year=re.Batch_Year and r.exam_code=e.exam_code and c.Criteria_no=e.criteria_no and  e.batch_year='" + ddlbatch.SelectedItem.Value + "'  " + sec + "  and c.Criteria_no  in('" + DropDownList2.SelectedItem.Value + "')  and s.subject_no=e.subject_no and marks_obtained >= e.min_mark and re.roll_no='" + dvcount1[i]["Roll_no"].ToString() + "'";
                        }
                        else
                        {
                            query = "select  r.roll_no,re.Stud_Name,s.subject_no,s.subject_name from criteriaforinternal c, result r,exam_type e,subject s,syllabus_master sn,Registration re where re.Batch_Year=e.batch_year and s.syll_code=sn.syll_code and c.syll_code=s.syll_code and re.Roll_No=r.roll_no and delflag=0 and exam_flag<>'debar'and r.exam_code=e.exam_code and c.Criteria_no=e.criteria_no and  e.batch_year='" + ddlbatch.SelectedItem.Value + "' " + sec + "   and c.Criteria_no  in('" + DropDownList2.SelectedItem.Value + "')  and s.subject_no=e.subject_no  and marks_obtained < e.min_mark and re.roll_no='" + dvcount1[i]["Roll_no"].ToString() + "'";

                        }
                        ds1 = da.select_method_wo_parameter(query, "text");
                        if (ds1.Tables[0].Columns.Count == 2)
                        {
                            for (int jsa = 0; jsa < ds1.Tables[0].Rows.Count; jsa++)
                            {

                                fpstudentdetails.Sheets[0].RowCount++;
                                fpstudentdetails.Sheets[0].ColumnHeader.Columns[3].Visible = false;
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 0].Text = cn.ToString();
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 1].Text = ds1.Tables[0].Rows[jsa]["roll_no"].ToString();
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 2].Text = ds1.Tables[0].Rows[jsa]["Stud_Name"].ToString();
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            }
                        }
                        else
                        {
                            for (int jsa = 0; jsa < ds1.Tables[0].Rows.Count; jsa++)
                            {

                                fpstudentdetails.Sheets[0].RowCount++;
                                fpstudentdetails.Sheets[0].ColumnHeader.Columns[3].Visible = true;
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 0].Text = cn.ToString();
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 1].Text = ds1.Tables[0].Rows[jsa]["roll_no"].ToString();
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 2].Text = ds1.Tables[0].Rows[jsa]["Stud_Name"].ToString();
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 3].Text = ds1.Tables[0].Rows[jsa]["subject_name"].ToString();
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                fpstudentdetails.Sheets[0].Cells[fpstudentdetails.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;

                            }

                        }

                    }
                }
                fpstudentdetails.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                fpstudentdetails.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                fpstudentdetails.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                fpstudentdetails.Sheets[0].Columns[0].Locked = true;
                fpstudentdetails.Sheets[0].Columns[1].Locked = true;
                fpstudentdetails.Sheets[0].Columns[2].Locked = true;
                fpstudentdetails.Sheets[0].Columns[3].Locked = true;
                fpstudentdetails.Visible = true;
                fpstudentdetails.Sheets[0].PageSize = fpstudentdetails.Sheets[0].RowCount;

            }
        }
        catch
        {
        }

    }

    private void setLabelText()
    {
        try
        {
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim().Split(',')[0] + "";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }
            institute = new Institution(grouporusercode);
            List<Label> lbl = new List<Label>();
            List<byte> fields = new List<byte>();
            lbl.Add(lblcollege);
            fields.Add(0);
            lbl.Add(lbldegree);
            fields.Add(2);
            lbl.Add(lbldept);
            fields.Add(3);
            lbl.Add(lblsem);
            fields.Add(4);
            if (institute != null && institute.TypeInstitute == 1)
            {
                lblbatch.Text = "Year";
            }
            else
            {
                lblbatch.Text = "Batch";
            }
            new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
        }
        catch (Exception ex)
        {

        }
    }

}
