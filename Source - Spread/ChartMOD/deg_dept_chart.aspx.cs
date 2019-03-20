using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.Configuration;

public partial class deg_dept_chart : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DAccess2 da = new DAccess2();
    Hashtable ht = new Hashtable();
    ArrayList al = new ArrayList();
    ArrayList al2 = new ArrayList();
    string usercode = "";
    string collegecode = "";
    string singleuser = "";
    string group_user = "";
    int ddlcount = 0;
    Boolean deptment = new Boolean();
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
        if (!IsPostBack)
        {
            college();
            bindyear();
            bindcourse();
            bindbranch(collegecode);
        }
        lblerror.Visible = false;


    }
    protected void logout_btn_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.RemoveAll();
        Session.Abandon();
        Response.Redirect("~/Default.aspx");

    }
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
        catch
        {
        }
    }
    public void bindcourse()
    {
        try
        {
            //CheckBoxListdegree.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
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
            ht.Add("college_code", ddlcollege.SelectedItem.Value);
            ht.Add("user_code", usercode);
            ds = da.select_method("bind_degree", ht, "sp");
            int count1 = ds.Tables[0].Rows.Count;
            ddldegree.Items.Clear();
            if (count1 > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch
        {
        }

    }
    public void bindbranch(string branch)
    {
        try
        {
            string commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + ddldegree.SelectedItem.Value + "') and deptprivilages.Degree_code=degree.Degree_code ";


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
    public void bindyear()
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
        catch
        {
        }


    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        Boolean falg_chart = false;
        if (radiobutton1.SelectedItem.Value == "1")
        {
            double cammark = 0.0;
            string da1 = "select s.subject_no,s.subject_name,e.batch_year,degree_code,semester,acronym from Exam_type e,syllabus_master sm,subject s,CriteriaForInternal c where s.syll_code=sm.syll_code and e.batch_year=sm.Batch_Year and c.syll_code=sm.syll_code and e.criteria_no=c.Criteria_no  and e.subject_no=s.subject_no group by s.subject_no,s.subject_name,e.batch_year,degree_code,semester,acronym";
            //  da1 = da1 + "select * from result";
            ds = da.select_method_wo_parameter(da1, "text");
            if (ddldept.Items.Count > 0)
            {
                for (int i = 0; i < ddldept.Items.Count; i++)
                {
                    if (ddldept.Items[i].Selected == true)
                    {
                        if (!al.Contains(ddldept.Items[i].Value.ToString()))
                        {
                            al.Add(ddldept.Items[i].Value.ToString());
                            al2.Add(ddldept.Items[i].ToString());
                        }

                    }

                }

            }
            if (al.Count > 0)
            {
                for (int k = 0; k < al.Count; k++)
                {
                    falg_chart = false;
                    string data = "select current_semester from Registration where batch_year=" + ddlbatch.SelectedItem.Value + " and degree_code=" + al[k].ToString() + "and cc=0 and delflag=0 and exam_flag<>'debar' group by Current_Semester";
                    ds1 = da.select_method_wo_parameter(data, "text");
                    if (ds1.Tables[0].Rows.Count > 0 && ds1 != null && ds1.Tables != null)
                    {
                        ds.Tables[0].DefaultView.RowFilter = "degree_code=" + al[k].ToString() + " and batch_year=" + ddlbatch.SelectedItem.Value + " and semester=" + ds1.Tables[0].Rows[0]["current_semester"].ToString() + "";
                        DataView dv = new DataView();
                        dv = ds.Tables[0].DefaultView;
                        if (al[0].ToString() == al[k].ToString())
                        {
                            dv.Table.Columns.Add("cam", typeof(string));
                        }
                        if (dv != null || dv.Count > 0)
                        {
                            for (int b = 0; b < dv.Count; b++)
                            {
                                string sqlquery = "select c.Criteria_no,s.subject_no,subject_name,e.min_mark,e.max_mark,re.marks_obtained,r.roll_no,r.Stud_Name from Exam_type e,CriteriaForInternal c,subject s,Result re,Registration r,syllabus_master SM where re.exam_code=e.exam_code and SM.syll_code=S.syll_code AND  r.Roll_No=re.roll_no and s.subject_no=e.subject_no and c.Criteria_no=e.criteria_no  and cc=0 and delflag=0 and exam_flag<>'debar' and r.Batch_Year=" + ddlbatch.SelectedItem.Value + " and r.degree_code=" + al[k].ToString() + " and semester=" + ds1.Tables[0].Rows[0]["current_semester"].ToString() + "";
                                ds2 = da.select_method_wo_parameter(sqlquery, "text");
                                DataView dv2 = new DataView();
                                ds2.Tables[0].DefaultView.RowFilter = "subject_no=" + dv[b]["subject_no"].ToString() + "";
                                dv2 = ds2.Tables[0].DefaultView;
                                double totalcount = dv2.Count;

                                DataView dv1 = new DataView();
                                ds2.Tables[0].DefaultView.RowFilter = "subject_no=" + dv[b]["subject_no"].ToString() + " and marks_obtained>=min_mark";
                                dv1 = ds2.Tables[0].DefaultView;
                                dv1 = dv;
                                cammark = (dv2.Count / totalcount) * 100;
                                dv[b]["cam"] = Math.Round(cammark);
                                falg_chart = true;
                            }
                            if (falg_chart == true)
                            {
                                Chart Chart1 = new Chart();
                                Chart1.ChartAreas.Add("ChartArea1");

                                Chart1.Series.Add("Series1").ChartType = SeriesChartType.Radar;
                                //Chart1.ChartAreas["CHARTAREA1"].AlignmentStyle = 
                                Chart1.Width = 450;
                                Chart1.Height = 400;
                                Chart1.Series["Series1"].BorderColor = Color.Blue;
                                Chart1.Series["Series1"]["RadarDrawingStyle"] = "marker";
                                Chart1.Series["Series1"]["CircularLabelsStyle"] = "Horizontal";
                                Chart1.Series["Series1"].MarkerSize = 7;
                                Chart1.Series["Series1"].MarkerStyle = MarkerStyle.Star4;
                                Chart1.Series["Series1"].MarkerColor = Color.DarkBlue;
                                Chart1.Series["Series1"].MarkerBorderWidth = 7;
                                Chart1.Series["Series1"].BorderWidth = 3;
                                Chart1.ChartAreas["ChartArea1"].AxisY.Interval = 25;
                                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 100;
                                Chart1.ChartAreas["ChartArea1"].AxisY.TitleForeColor = Chart1.ForeColor;
                                Chart1.Series["Series1"].IsValueShownAsLabel = true;
                                Chart1.ChartAreas[0].AxisY.LineColor = Color.Blue;
                                Chart1.ChartAreas[0].AxisY.LineWidth = 1;
                                Chart1.Series["Series1"]["RadarDrawingStyle"] = "Line";
                                Chart1.Series["Series1"].Color = Color.Red;
                                Chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.BlueViolet;
                                Chart1.Series["Series1"].XValueMember = "acronym";
                                Chart1.Series["Series1"].YValueMembers = "cam";
                                Chart1.Series["Series1"].Font = new Font("Book Antiqua", 10, FontStyle.Bold);
                                Chart1.Series["Series1"].Name = "Subject wise %";

                                Chart1.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Book Antiqua", 8, FontStyle.Bold);
                                Chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Green;
                                Chart1.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Book Antiqua", 8);
                                Chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Black;
                                Title radarchart = Chart1.Titles.Add("" + al2[k].ToString() + " (Over All CAM Subject Wise)");
                                radarchart.Font = new Font("Book Antiqua", 10, FontStyle.Bold);
                                Chart1.DataSource = dv;
                                Chart1.DataBind();
                                Chart1.SaveImage(Server.MapPath("App_Data/Sample.jpg"));
                                panelchart.Controls.Add(Chart1);
                            }
                            else
                            {
                                Table b = new Table();
                                TableCell tc4 = new TableCell();
                                TableRow tr4 = new TableRow();
                                Label lblerr = new Label();
                                lblerr.Text = "No Records Found for " + al2[k].ToString() + "";
                                lblerr.Font.Size = FontUnit.Medium;
                                lblerr.Font.Name = "Book Antiqua";
                                lblerr.ForeColor = Color.Red;
                                lblerr.Font.Bold = true;
                                //lblerr.Font.Name 
                                lblerr.Visible = true;
                                panelerror.Controls.Add(b);
                                tr4.Cells.Add(tc4);
                                b.Rows.Add(tr4);
                                tc4.Controls.Add(lblerr);
                            }
                        }
                        else
                        {
                            Table b = new Table();
                            TableCell tc4 = new TableCell();
                            TableRow tr4 = new TableRow();
                            Label lblerr = new Label();
                            lblerr.Text = "No Records Found for " + al2[k].ToString() + "";
                            lblerr.Font.Size = FontUnit.Medium;
                            lblerr.Font.Name = "Book Antiqua";
                            lblerr.ForeColor = Color.Red;
                            lblerr.Font.Bold = true;
                            //lblerr.Font.Name 
                            lblerr.Visible = true;
                            panelerror.Controls.Add(b);
                            tr4.Cells.Add(tc4);
                            b.Rows.Add(tr4);
                            tc4.Controls.Add(lblerr);
                        }
                    }
                    else
                    {
                        Table b = new Table();
                        TableCell tc4 = new TableCell();
                        TableRow tr4 = new TableRow();
                        Label lblerr = new Label();
                        lblerr.Text = "No Students Avaliable In " + al2[k].ToString() + "";
                        lblerr.Font.Size = FontUnit.Medium;
                        lblerr.Font.Name = "Book Antiqua";
                        lblerr.ForeColor = Color.Red;
                        lblerr.Font.Bold = true;
                        //lblerr.Font.Name 
                        lblerr.Visible = true;
                        panelerror.Controls.Add(b);
                        tr4.Cells.Add(tc4);
                        b.Rows.Add(tr4);
                        tc4.Controls.Add(lblerr);
                    }


                }
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select Any One Department";
            }
        }
        //else if (radiobutton1.SelectedItem.Value == "2")
        //{
        //    if (ddldept.Items.Count > 0)
        //    {
        //        for (int i = 0; i < ddldept.Items.Count; i++)
        //        {
        //            if (ddldept.Items[i].Selected == true)
        //            {
        //                if (!al.Contains(ddldept.Items[i].Value.ToString()))
        //                {
        //                    al.Add(ddldept.Items[i].Value.ToString());
        //                    al2.Add(ddldept.Items[i].ToString());
        //                }

        //            }

        //        }
        //    }
        //    if (al.Count > 0)
        //    {
        //        for (int lk = 0; lk < al.Count; lk++)
        //        {

        //            string sec1;
        //            double grademark;
        //            double grademark1;
        //            DataSet ds2 = new DataSet();
        //            DataSet ds3 = new DataSet();
        //            DataSet datagrade = new DataSet();
        //            Boolean flag_university = new Boolean();
        //            flag_university = false;
        //            string sqldatabind = " select distinct Mark_Grade,Credit_Points from Grade_Master where Degree_Code='" + al[lk].ToString() + "'  and college_code='" + ddlcollege.SelectedItem.Value + "' and batch_year='" + ddlbatch.SelectedItem.Value + "'order by Credit_Points desc";
        //            datagrade = da.select_method_wo_parameter(sqldatabind, "text");
        //            ht.Clear();
        //            string value = "";
        //            if (datagrade.Tables[0].Rows.Count > 0)
        //            {
        //                ht.Clear();
        //                for (int l = 0; l < datagrade.Tables[0].Rows.Count; l++)
        //                {
        //                    value = datagrade.Tables[0].Rows[0]["Mark_Grade"].ToString();
        //                    ht.Add(datagrade.Tables[0].Rows[l]["Mark_Grade"], "" + datagrade.Tables[0].Rows[l]["Credit_Points"] + "");
        //                }
        //            }
        //            string data = "select current_semester from Registration where batch_year=" + ddlbatch.SelectedItem.Value + " and degree_code=" + al[lk].ToString() + "and cc=0 and delflag=0 and exam_flag<>'debar' group by Current_Semester";
        //            ds = da.select_method_wo_parameter(data, "text");
        //            if (ds.Tables[0].Rows.Count > 0 && ds != null && ds.Tables != null)
        //            {
        //                string sqlqurey1 = " Select distinct Current_Semester,Exam_Code,exam_month,exam_year from Exam_Details where Degree_Code = '" + al[lk].ToString() + "'  and Current_Semester='" + ds.Tables[0].Rows[0]["current_semester"].ToString() + "' and Batch_Year = '" + ddlbatch.SelectedItem.Value + "'";
        //                ds2 = da.select_method_wo_parameter(sqlqurey1, "text");
        //                if (ds2.Tables[0].Rows.Count > 0)
        //                {
        //                    string subject_no = "";
        //                    string sqlquery = "Select distinct s.mintotal as mintot,s.min_int_marks as mimark, s.min_ext_marks as mxmark,s.maxtotal as maxtot,s.acronym,subject_name,subject_code as Subject_Code,mark_entry.subject_no as Subject_No,semester,subject_type as Subtype,credit_points from Mark_Entry,Subject s,sub_sem,syllabus_master,staff_selector st,staffmaster sm,staff_appl_master stf,desig_master dm  where stf.appl_no=sm.appl_no and stf.desig_code=dm.desig_code and stf.desig_name=dm.desig_name and syllabus_master.syll_code=s.syll_code and Mark_Entry.Subject_No = s..Subject_No and s.subtype_no= sub_sem.subtype_no and sm.staff_code=st.staff_code and Exam_Code = '" + ds2.Tables[0].Rows[0]["Exam_Code"] + "' and attempts=1 and st.subject_no=s.subject_no order by semester desc,subject_type desc, mark_entry.subject_no asc";
        //                    ds = da.select_method_wo_parameter(sqlquery, "text");
        //                    ds.Tables[0].Columns.Add("cammark", typeof(string));
        //                    if (ds.Tables[0].Rows.Count > 0)
        //                    {
        //                        for (int g = 0; g < ds.Tables[0].Rows.Count; g++)
        //                        {
        //                            if (subject_no.ToString() == "")
        //                            {
        //                                subject_no = ds.Tables[0].Rows[g]["subject_no"].ToString();
        //                            }
        //                            else
        //                            {
        //                                subject_no = subject_no + "," + ds.Tables[0].Rows[g]["subject_no"].ToString();
        //                            }
        //                        }
        //                    }
        //                    if (subject_no.ToString() != "")
        //                    {
        //                        DataSet university = new DataSet();
        //                        string sqlquery1 = "Select grade_flag from grademaster where exam_month='" + ds2.Tables[0].Rows[0]["exam_month"] + "' and exam_year='" + ds2.Tables[0].Rows[0]["exam_year"] + "' and Batch_year='" + ddlbatch.SelectedItem.Value + "' and degree_code='" + al[lk].ToString() + "'";
        //                        university = da.select_method_wo_parameter(sqlquery1, "text");
        //                        string sqlroll = "Select acronym as subject_name,subject_code,subject.subject_no,result,total,grade,Registration.roll_no,cp,mark_entry.subject_no,subject.min_ext_marks,subject.min_int_marks,isnull(Mark_Entry.internal_mark,0) as internal_mark ,isnull(Mark_Entry.external_mark,0) as external_mark,semester,Sections from Mark_Entry,Subject,sub_sem,syllabus_master,Registration where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No =Subject.Subject_No and  subject.subtype_no= sub_sem.subtype_no and Registration.Roll_No=mark_entry.roll_no and  Exam_Code in(" + ds2.Tables[0].Rows[0]["Exam_Code"] + ") and Subject.subject_no in (" + subject_no + ")  and CC=0 and DelFlag=0 and Exam_Flag<>'debar' order by semester desc,subject_type desc,subject.subject_no asc";
        //                        ds3 = da.select_method_wo_parameter(sqlroll, "Text");
        //                        if (university.Tables[0].Rows.Count > 0)
        //                        {

        //                            if (university.Tables[0].Rows.Count > 0)
        //                            {
        //                                if (university.Tables[0].Rows[0]["grade_flag"].ToString() == "2")
        //                                {

        //                                    if (ds.Tables[0].Rows.Count > 0)
        //                                    {

        //                                        double totalmark = 0;
        //                                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //                                        {
        //                                            grademark = 0.0;
        //                                            DataView dv = new DataView();
        //                                            ds3.Tables[0].DefaultView.RowFilter = "subject_code='" + ds.Tables[0].Rows[i]["subject_code"] + "'";
        //                                            dv = ds3.Tables[0].DefaultView;
        //                                            for (int j = 0; j < dv.Count; j++)
        //                                            {

        //                                                //int count = hat.Count;
        //                                                if (ds3.Tables[0].Rows[j]["cp"] == DBNull.Value)
        //                                                {

        //                                                }
        //                                                else
        //                                                {
        //                                                    string mark = dv[j]["grade"].ToString();
        //                                                    double mark2 = Convert.ToInt32(ht[value]) * Convert.ToInt32(dv[j]["cp"]);
        //                                                    double mark1 = 0;

        //                                                    foreach (DictionaryEntry child in ht)
        //                                                    {


        //                                                        if (mark.ToString() == child.Key.ToString())
        //                                                        {
        //                                                            if (child.Value.ToString() == "0")
        //                                                            {



        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                mark1 = Convert.ToInt32(child.Value) * Convert.ToInt32(dv[j]["cp"]);
        //                                                                grademark++;
        //                                                                flag_university = true;
        //                                                            }
        //                                                        }

        //                                                    }
        //                                                }


        //                                            }
        //                                            for (int l = 0; l < ds.Tables[0].Rows.Count; l++)
        //                                            {
        //                                                if (ds.Tables[0].Rows[l]["subject_no"].ToString() == dv[0]["subject_no"].ToString())
        //                                                {
        //                                                    grademark1 = (grademark / dv.Count) * 100;
        //                                                    ds.Tables[0].Rows[l]["cammark"] = Math.Round(grademark1);

        //                                                }
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                else if (university.Tables[0].Rows[0]["grade_flag"].ToString() == "3")
        //                                {
        //                                    string dd = "select linkvalue from inssettings where linkname='corresponding grade' and college_code='" + Session["collegecode"].ToString() + "'";

        //                                    DataSet df1 = new DataSet();
        //                                    df1 = da.select_method_wo_parameter(dd, "text");
        //                                    if (df1.Tables[0].Rows[0]["linkvalue"].ToString() == "0")
        //                                    {
        //                                        string sqlroll1 = "Select acronym as subject_name,subject_code,subject.subject_no,result,total,grade,Registration.roll_no,cp,mark_entry.subject_no,subject.min_ext_marks,subject.min_int_marks,isnull(Mark_Entry.internal_mark,0) as internal_mark ,isnull(Mark_Entry.external_mark,0) as external_mark,semester,Sections from Mark_Entry,Subject,sub_sem,syllabus_master,Registration where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No =Subject.Subject_No and  subject.subtype_no= sub_sem.subtype_no and Registration.Roll_No=mark_entry.roll_no and  Exam_Code in(" + ds2.Tables[0].Rows[0]["Exam_Code"] + ") and Subject.subject_no in (" + subject_no + ") and CC=0 and DelFlag=0 and Exam_Flag<>'debar' order by semester desc,subject_type desc,subject.subject_no asc";
        //                                        ds3 = da.select_method_wo_parameter(sqlroll1, "Text");

        //                                        for (int j = 0; j < ds3.Tables[0].Rows.Count; j++)
        //                                        {
        //                                            grademark = 0.0;
        //                                            DataView dv = new DataView();
        //                                            ds3.Tables[0].DefaultView.RowFilter = "subject_code='" + ds3.Tables[0].Rows[j]["subject_code"] + "'";
        //                                            dv = ds3.Tables[0].DefaultView;
        //                                            for (int k = 0; k < dv.Count; k++)
        //                                            {
        //                                                double internal1 = Convert.ToDouble(dv[k]["internal_mark"].ToString());
        //                                                double external = Convert.ToDouble(dv[k]["external_mark"].ToString());
        //                                                double mark1 = Convert.ToDouble(dv[k]["min_ext_marks"].ToString());
        //                                                double mark2 = Convert.ToDouble(dv[k]["min_int_marks"].ToString());
        //                                                if (external == 0.0 && internal1 == 0.0)
        //                                                {
        //                                                    flag_university = false;

        //                                                }
        //                                                else
        //                                                {
        //                                                    if (internal1 >= mark2 && external >= mark1)
        //                                                    {
        //                                                        grademark++;
        //                                                        flag_university = true;

        //                                                    }

        //                                                }
        //                                            }
        //                                            for (int l = 0; l < ds.Tables[0].Rows.Count; l++)
        //                                            {
        //                                                if (ds.Tables[0].Rows[l]["subject_no"].ToString() == dv[0]["subject_no"].ToString())
        //                                                {
        //                                                    grademark1 = (grademark / dv.Count) * 100;
        //                                                    if (grademark1.ToString() != "NaN")
        //                                                    {
        //                                                        ds.Tables[0].Rows[l]["cammark"] = Math.Round(grademark1);
        //                                                        flag_university = true;

        //                                                    }

        //                                                }
        //                                            }
        //                                        }



        //                                    }
        //                                    else if (df1.Tables[0].Rows[0]["linkvalue"].ToString() == "1")
        //                                    {


        //                                        double totalmark = 0;
        //                                        for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
        //                                        {
        //                                            grademark = 0.0;
        //                                            DataView dv = new DataView();
        //                                            ds3.Tables[0].DefaultView.RowFilter = "subject_code='" + ds3.Tables[0].Rows[i]["subject_code"] + "'";
        //                                            dv = ds3.Tables[0].DefaultView;
        //                                            for (int j = 0; j < dv.Count; j++)
        //                                            {

        //                                                //int count = hat.Count;
        //                                                if (ds3.Tables[0].Rows[j]["cp"] == DBNull.Value)
        //                                                {

        //                                                }
        //                                                else
        //                                                {
        //                                                    string mark = dv[j]["grade"].ToString();
        //                                                    double mark2 = Convert.ToInt32(ht[value]) * Convert.ToInt32(dv[j]["cp"]);
        //                                                    double mark1 = 0;

        //                                                    foreach (DictionaryEntry child in ht)
        //                                                    {


        //                                                        if (mark.ToString() == child.Key.ToString())
        //                                                        {
        //                                                            if (child.Value.ToString() == "0")
        //                                                            {



        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                mark1 = Convert.ToInt32(child.Value) * Convert.ToInt32(dv[j]["cp"]);
        //                                                                grademark++;
        //                                                                flag_university = true;
        //                                                            }
        //                                                        }

        //                                                    }
        //                                                }


        //                                            }
        //                                            for (int l = 0; l < ds.Tables[0].Rows.Count; l++)
        //                                            {
        //                                                if (ds.Tables[0].Rows[l]["subject_no"].ToString() == dv[0]["subject_no"].ToString())
        //                                                {
        //                                                    grademark1 = (grademark / dv.Count) * 100;
        //                                                    ds.Tables[0].Rows[l]["cammark"] = Math.Round(grademark1);

        //                                                }
        //                                            }

        //                                        }
        //                                    }
        //                                }
        //                                else if (university.Tables[0].Rows[0]["grade_flag"].ToString() == "1")
        //                                {
        //                                    string sqlroll1 = "Select acronym as subject_name,subject_code,subject.subject_no,result,total,grade,Registration.roll_no,cp,mark_entry.subject_no,subject.min_ext_marks,subject.min_int_marks,isnull(Mark_Entry.internal_mark,0) as internal_mark ,isnull(Mark_Entry.external_mark,0) as external_mark,semester,Sections from Mark_Entry,Subject,sub_sem,syllabus_master,Registration where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No =Subject.Subject_No and  subject.subtype_no= sub_sem.subtype_no and Registration.Roll_No=mark_entry.roll_no and  Exam_Code in(" + ds2.Tables[0].Rows[0]["Exam_Code"] + ") and Subject.subject_no in (" + subject_no + ")  and CC=0 and DelFlag=0 and Exam_Flag<>'debar' order by semester desc,subject_type desc,subject.subject_no asc";
        //                                    ds3 = da.select_method_wo_parameter(sqlroll1, "Text");

        //                                    for (int j = 0; j < ds3.Tables[0].Rows.Count; j++)
        //                                    {
        //                                        grademark = 0.0;
        //                                        DataView dv = new DataView();
        //                                        ds3.Tables[0].DefaultView.RowFilter = "subject_code='" + ds3.Tables[0].Rows[j]["subject_code"] + "'";
        //                                        dv = ds3.Tables[0].DefaultView;
        //                                        for (int k = 0; k < dv.Count; k++)
        //                                        {
        //                                            double internal1 = Convert.ToDouble(dv[k]["internal_mark"].ToString());
        //                                            double external = Convert.ToDouble(dv[k]["external_mark"].ToString());
        //                                            double mark1 = Convert.ToDouble(dv[k]["min_ext_marks"].ToString());
        //                                            double mark2 = Convert.ToDouble(dv[k]["min_int_marks"].ToString());
        //                                            if (external == 0.0 && internal1 == 0.0)
        //                                            {
        //                                                flag_university = false;

        //                                            }
        //                                            else
        //                                            {
        //                                                if (internal1 >= mark2 && external >= mark1)
        //                                                {
        //                                                    grademark++;
        //                                                    flag_university = true;

        //                                                }

        //                                            }
        //                                        }
        //                                        for (int l = 0; l < ds.Tables[0].Rows.Count; l++)
        //                                        {
        //                                            if (ds.Tables[0].Rows[l]["subject_no"].ToString() == dv[0]["subject_no"].ToString())
        //                                            {
        //                                                grademark1 = (grademark / dv.Count) * 100;
        //                                                if (grademark1.ToString() != "NaN")
        //                                                {
        //                                                    ds.Tables[0].Rows[l]["cammark"] = Math.Round(grademark1);
        //                                                    flag_university = true;

        //                                                }

        //                                            }
        //                                        }
        //                                    }


        //                                }


        //                            }
        //                            if (flag_university == true)
        //                            {

        //                                if (ds.Tables[0].Rows.Count > 0)
        //                                {
        //                                    Chart Chart1 = new Chart();
        //                                    Chart1.ChartAreas.Add("ChartArea1");

        //                                    Chart1.Series.Add("Series1").ChartType = SeriesChartType.Radar;
        //                                    Chart1.Series["Series1"].BorderColor = Color.Blue;
        //                                    Chart1.Series["Series1"]["RadarDrawingStyle"] = "marker";
        //                                    Chart1.Series["Series1"]["CircularLabelsStyle"] = "Horizontal";
        //                                    Chart1.Series["Series1"].MarkerSize = 7;
        //                                    Chart1.Series["Series1"].MarkerStyle = MarkerStyle.Star4;
        //                                    Chart1.Series["Series1"].MarkerColor = Color.DarkBlue;
        //                                    Chart1.Series["Series1"].MarkerBorderWidth = 7;
        //                                    Chart1.Series["Series1"].BorderWidth = 3;
        //                                    Chart1.ChartAreas["ChartArea1"].AxisY.Interval = 25;
        //                                    Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 100;
        //                                    Chart1.ChartAreas["ChartArea1"].AxisY.TitleForeColor = Chart1.ForeColor;
        //                                    Chart1.Series["Series1"].IsValueShownAsLabel = true;
        //                                    Chart1.Series["Series1"]["RadarDrawingStyle"] = "Line";
        //                                    Chart1.Series["Series1"].Color = Color.Red;
        //                                    Chart1.Series["Series1"].XValueMember = "acronym";
        //                                    Chart1.Series["Series1"].YValueMembers = "Cammark";
        //                                    Chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.BlueViolet;
        //                                    Chart1.ChartAreas[0].AxisY.LineColor = Color.Blue;
        //                                    Chart1.Series["Series1"].Font = new Font("Book Antiqua", 10, FontStyle.Bold);
        //                                    Chart1.Series["Series1"].Name = "University Subject Wise Over All Class Percentage";
        //                                    Chart1.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Book Antiqua", 14);
        //                                    Chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Green;
        //                                    Chart1.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Book Antiqua", 9);
        //                                    Chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Black;
        //                                    Title radarchart = Chart1.Titles.Add("University Subject Wise Over All Percentage");
        //                                    radarchart.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
        //                                    Chart1.Width = 900;
        //                                    Chart1.Height = 500;
        //                                    Chart1.DataSource = ds;
        //                                    Chart1.DataBind();
        //                                    panelchart.Controls.Add(Chart1);
        //                                    Chart1.SaveImage(Server.MapPath("App_Data/Sample.jpg"));
        //                                }


        //                            }
        //                            else
        //                            {
        //                                Table b = new Table();
        //                                TableCell tc4 = new TableCell();
        //                                TableRow tr4 = new TableRow();
        //                                Label lblerr = new Label();
        //                                lblerr.Text = "No Records Found In " + al2[lk].ToString() + "";
        //                                lblerr.Font.Size = FontUnit.Medium;
        //                                lblerr.Font.Name = "Book Antiqua";
        //                                lblerr.ForeColor = Color.Red;
        //                                lblerr.Font.Bold = true;
        //                                //lblerr.Font.Name 
        //                                lblerr.Visible = true;
        //                                panelerror.Controls.Add(b);
        //                                tr4.Cells.Add(tc4);
        //                                b.Rows.Add(tr4);
        //                                tc4.Controls.Add(lblerr);
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Table b = new Table();
        //                        TableCell tc4 = new TableCell();
        //                        TableRow tr4 = new TableRow();
        //                        Label lblerr = new Label();
        //                        lblerr.Text = "No Records Found In " + al2[lk].ToString() + "";
        //                        lblerr.Font.Size = FontUnit.Medium;
        //                        lblerr.Font.Name = "Book Antiqua";
        //                        lblerr.ForeColor = Color.Red;
        //                        lblerr.Font.Bold = true;
        //                        //lblerr.Font.Name 
        //                        lblerr.Visible = true;
        //                        panelerror.Controls.Add(b);
        //                        tr4.Cells.Add(tc4);
        //                        b.Rows.Add(tr4);
        //                        tc4.Controls.Add(lblerr);

        //                    }
        //                }
        //                else
        //                {
        //                    Table b = new Table();
        //                    TableCell tc4 = new TableCell();
        //                    TableRow tr4 = new TableRow();
        //                    Label lblerr = new Label();
        //                    lblerr.Text = "No Records Found In " + al2[lk].ToString() + "";
        //                    lblerr.Font.Size = FontUnit.Medium;
        //                    lblerr.Font.Name = "Book Antiqua";
        //                    lblerr.ForeColor = Color.Red;
        //                    lblerr.Font.Bold = true;
        //                    //lblerr.Font.Name 
        //                    lblerr.Visible = true;
        //                    panelerror.Controls.Add(b);
        //                    tr4.Cells.Add(tc4);
        //                    b.Rows.Add(tr4);
        //                    tc4.Controls.Add(lblerr);

        //                }
        //            }
        //            else
        //            {
        //                Table b = new Table();
        //                TableCell tc4 = new TableCell();
        //                TableRow tr4 = new TableRow();
        //                Label lblerr = new Label();
        //                lblerr.Text = "No Students Avaliable In " + al2[lk].ToString() + "";
        //                lblerr.Font.Size = FontUnit.Medium;
        //                lblerr.Font.Name = "Book Antiqua";
        //                lblerr.ForeColor = Color.Red;
        //                lblerr.Font.Bold = true;
        //                //lblerr.Font.Name 
        //                lblerr.Visible = true;
        //                panelerror.Controls.Add(b);
        //                tr4.Cells.Add(tc4);
        //                b.Rows.Add(tr4);
        //                tc4.Controls.Add(lblerr);

        //            }
        //        }
        //    }
        //    else
        //    {
        //        lblerror.Visible = true;
        //        lblerror.Text = "Please Select Any One Department";
        //    }



        //}

    }
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_Dept.Text = "--Select--";
        chkdept.Checked = false;
        bindyear();
        bindcourse();
        bindbranch(collegecode);
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_Dept.Text = "--Select--";
        chkdept.Checked = false;
        bindcourse();
        bindbranch(collegecode);

    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_Dept.Text = "--Select--";
        chkdept.Checked = false;
        bindbranch(collegecode);
    }

    protected void radiobutton1_selectedindexchanged(object sender, EventArgs e)
    {

    }
    protected void chkdept_checkedchanged(object sender, EventArgs e)
    {
        txt_Dept.Text = "--Select--";
        if (chkdept.Checked == true)
        {

            for (int i = 0; i < ddldept.Items.Count; i++)
            {

                ddldept.Items[i].Selected = true;
                txt_Dept.Text = "Department(" + (ddldept.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < ddldept.Items.Count; i++)
            {
                ddldept.Items[i].Selected = false;
                txt_Dept.Text = "--Select--";
            }
        }
    }
    protected void ddldept_selectedchanged(object sender, EventArgs e)
    {
        try
        {
            txt_Dept.Text = "--Select--";

            string value = "";
            string code = "";
            for (int i = 0; i < ddldept.Items.Count; i++)
            {

                if (ddldept.Items[i].Selected == true)
                {

                    value = ddldept.Items[i].Text;
                    code = ddldept.Items[i].Value.ToString();
                    ddlcount = ddlcount + 1;
                    txt_Dept.Text = "Department(" + ddlcount.ToString() + ")";
                }



            }

            if (ddlcount == 0)
                txt_Dept.Text = "---Select---";
        }
        catch
        {

        }

    }
}