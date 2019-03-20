using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Web.UI.DataVisualization.Charting;
using System.Collections.Generic;
using System.Configuration;

public partial class staffcutoffchart : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DAccess2 da = new DAccess2();
    Hashtable ht = new Hashtable();
    DataSet syllbus = new DataSet();
    DataSet syllbus1 = new DataSet();
    DataSet purpo = new DataSet();
    string usercode = "";
    string collegecode = "";
    string singleuser = "";
    string group_user = "";
    //string college = "";
    string strbatch = "";
    string strdep = "";
    string strdegree = "";
    Boolean flag_select = false;
    Boolean Cellclick = false;

    string SenderID = string.Empty;
    string Password = string.Empty;
    string user_id = string.Empty;

    string message = string.Empty;
    string message1 = string.Empty;
    string strmobileno = string.Empty;
    string mobilenos = "";

    string mailid = string.Empty;
    string mailpwd = string.Empty;
    string to_mail = string.Empty;
    string strstuname = string.Empty;
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
        // datacam1.Visible = true;
        Label1.Visible = false;
        lblerror.Visible = false;
        Label2.Visible = false;
        if (!IsPostBack)
        {
            setLabelText();
            college();
            bindyear();
            bindcourse();
            bindbranch(collegecode);
            bindsem();
            BindSectionDetail();
            test();
            fpcammarkstaff.Visible = false;
            labheading.Visible = false;
            chkboxsms.Checked = true;
            //labpurpose.Visible = false;
            //ddlpurpose.Visible = false;
            //fpspreadpurpose.Visible = false;
            //btnaddtemplate.Visible = false;
            //btndeletetemplate.Visible = false;
            txtmessage.Visible = false;
            Tablenote.Visible = false;
            btnsms.Visible = false;
            // btnxl.Visible = false;
        }
        collegecode = Session["collegecode"].ToString();
        if (lblcollege.Text.Trim().ToLower() == "school")
        {
            labbatch.Text = "Year";
        }
    }
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
        lbl.Add(lblcollege);
        fields.Add(0);
        //lbl.Add(lbl_Stream);
        //fields.Add(1);
        lbl.Add(lbldegree);
        fields.Add(2);
        lbl.Add(lbldept);
        fields.Add(3);
        lbl.Add(lblsem);
        fields.Add(4);
        //Name -0, Stream - 1 ,Degree - 2, Branch - 3, Term - 4
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
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
    protected void logout_btn_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.RemoveAll();
        Session.Abandon();
        Response.Redirect("~/Default.aspx");
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
    public void test()
    {
        try
        {
            string SyllabusYr = "";
            string SyllabusQry = "select distinct syllabus_year from syllabus_master where degree_code ='" + ddldept.SelectedItem.Value + "' and batch_year ='" + ddlbatch.SelectedItem.Value + "' ";
            syllbus = da.select_method_wo_parameter(SyllabusQry, "text");
            DropDownList2.Items.Clear();

            if (syllbus.Tables[0].Rows.Count > 0)
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

                string Sqlstr = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code=" + ddldept.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and syllabus_year in(" + SyllabusYr.ToString() + ") and batch_year=" + ddlbatch.SelectedValue.ToString() + " order by criteria";
                syllbus1 = da.select_method_wo_parameter(Sqlstr, "Text");
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
        catch
        {
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            if (radiobutton1.Text == "CAM Wise")
            {
                if (ddlsem.Text == "")
                {
                    Label2.Visible = true;
                    Label2.Text = "Please Select Any One Semester ";
                }
                else if (DropDownList2.Text == "")
                {
                    Label2.Visible = true;
                    Label2.Text = "Please Select Any One Test";
                }
                else if (txtcutoff.Text == "")
                {
                    Label2.Visible = true;
                    Label2.Text = "Please Enter The Cutoff Range ";
                }
                else
                {
                    fpcammarkstaff.Visible = false;
                    labheading.Visible = false;
                    //labpurpose.Visible = false;
                    //ddlpurpose.Visible = false;
                    //fpspreadpurpose.Visible = false;
                    //btnaddtemplate.Visible = false;
                    //btndeletetemplate.Visible = false;
                    txtmessage.Visible = false;
                    txtmessage.Text = "";
                    Tablenote.Visible = false;
                    btnsms.Visible = false;
                    lblnotification.Visible = false;
                    // btnxl.Visible = false;
                    if (Cellclick == false)
                    {
                        camchart();
                        Cellclick = true;
                    }
                }
            }
            else
            {
                if (ddlsem.Text == "")
                {
                    Label2.Visible = true;
                    Label2.Text = "Please Select Any One Semester ";
                }
                else if (txtcutoff.Text == "")
                {
                    Label2.Visible = true;
                    Label2.Text = "Please Enter The Cutoff Range ";
                }
                else
                {
                    fpcammarkstaff.Visible = false;
                    labheading.Visible = false;
                    //labpurpose.Visible = false;
                    //ddlpurpose.Visible = false;
                    //fpspreadpurpose.Visible = false;
                    //btnaddtemplate.Visible = false;
                    //btndeletetemplate.Visible = false;
                    txtmessage.Visible = false;
                    Tablenote.Visible = false;
                    btnsms.Visible = false;
                    lblnotification.Visible = false;
                    // btnxl.Visible = false;
                    if (Cellclick == false)
                    {
                        camchart();
                        Cellclick = true;
                    }
                }
            }
        }
        catch
        {
        }

    }
    protected void camchart()
    {
        try
        {
            string subject_no = "";
            string sec1;
            if (DropDownList1.Text == "")
            {
                sec1 = "";
            }
            else
            {
                sec1 = "and e.sections='" + DropDownList1.SelectedItem.Value + "' ";
            }

            //  datacam1.Series.Clear();
            if (radiobutton1.Text == "CAM Wise")
            {
                if (DropDownList2.Text != "")
                {
                    double cammark1;
                    DataSet ds1 = new DataSet();
                    string sec;
                    if (DropDownList1.Text == "")
                    {
                        sec = "";
                    }
                    else
                    {
                        sec = "and e.sections='" + DropDownList1.SelectedItem.Value + "' ";
                    }
                    string sqlquery1 = "select e.criteria_no,c.criteria,e.subject_no,sub.subject_name,exam_code,e.staff_code,s.staff_name,stf.email,stf.per_mobileno,dept_name,dm.desig_name,stf.college_code from Exam_type e,CriteriaForInternal c,staffmaster s,staff_appl_master stf,subject sub,Desig_Master dm where dm.desig_code=stf.desig_code and s.appl_no=stf.appl_no and s.staff_code=e.staff_code and  e.criteria_no=c.Criteria_no and sub.subject_no=e.subject_no " + sec + " and c.Criteria_no='" + DropDownList2.SelectedItem.Value + "'  and s.college_code=dm.collegeCode  group by e.criteria_no,c.criteria,e.subject_no,sub.subject_name,exam_code,e.staff_code,s.staff_name,stf.email,stf.per_mobileno,dept_name,dm.desig_name,stf.college_code";
                    ds = da.select_method_wo_parameter(sqlquery1, "text");
                    ds.Tables[0].Columns.Add("cammark", typeof(string));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int g = 0; g < ds.Tables[0].Rows.Count; g++)
                        {
                            if (subject_no.ToString() == "")
                            {
                                subject_no = ds.Tables[0].Rows[g]["subject_no"].ToString();
                            }
                            else
                            {
                                subject_no = subject_no + "," + ds.Tables[0].Rows[g]["subject_no"].ToString();
                            }
                        }
                        if (subject_no.ToString() != "")
                        {
                            // string sqlcam = "select distinct c.criteria,c.criteria_no,r.marks_obtained,s.acronym,e.subject_no,s.subject_name,e.min_mark, e.max_mark,reg.Roll_No,reg.Sections,sm.staff_code,stf.appl_name,stf.email,stf.per_mobileno,dept_name,desig_code,stf.college_code from criteriaforinternal c, result r,exam_type e,subject s,syllabus_master sn,Registration reg,staff_appl_master stf,staffmaster sm where  e.staff_code=sm.staff_code  and sm.appl_no=stf.appl_no and r.exam_code=e.exam_code and  c.Criteria_no=e.criteria_no and e.batch_year='" + ddlbatch.SelectedItem.Text + "' and r.roll_no=reg.Roll_No and c.Criteria_no ='" + ds.Tables[0].Rows[0]["criteria_no"] + "' and s.subject_no=e.subject_no and e.subject_no in (" + subject_no + ") order by criteria";
                            string sqlcam = "select distinct c.criteria,c.criteria_no,r.marks_obtained,s.acronym,e.subject_no,s.subject_name,e.min_mark, e.max_mark,reg.Roll_No,reg.Sections,staff_code  from criteriaforinternal c, result r,exam_type e,subject s,syllabus_master sn,Registration reg where r.exam_code=e.exam_code and  c.Criteria_no=e.criteria_no and e.batch_year='" + ddlbatch.SelectedItem.Text + "' and r.roll_no=reg.Roll_No and c.Criteria_no ='" + ds.Tables[0].Rows[0]["criteria_no"] + "' and s.subject_no=e.subject_no and e.subject_no in (" + subject_no + ") and CC=0 and DelFlag=0 and Exam_Flag<>'debar' order by criteria";
                            ds1 = da.select_method_wo_parameter(sqlcam, "text");
                            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                            {


                                DataView dv = new DataView();
                                ds1.Tables[0].DefaultView.RowFilter = "staff_code='" + ds.Tables[0].Rows[k]["staff_code"] + "' and subject_no='" + ds.Tables[0].Rows[k]["subject_no"] + "'";
                                dv = ds1.Tables[0].DefaultView;
                                double data = dv.Count;
                                cammark1 = 0.0;
                                DataView dv1 = new DataView();
                                dv.RowFilter = "marks_obtained>=min_mark and staff_code='" + ds.Tables[0].Rows[k]["staff_code"] + "' and subject_no='" + ds.Tables[0].Rows[k]["subject_no"] + "'";
                                dv1 = dv;
                                cammark1 = (dv1.Count / data) * 100;
                                ds.Tables[0].Rows[k]["cammark"] = Math.Round(cammark1);

                            }

                        }
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds1.Tables[0].Rows.Count > 0)
                        {

                            DataView dv2 = new DataView();
                            ds.Tables[0].DefaultView.RowFilter = "cammark>='" + txtcutoff.Text + "'";
                            dv2 = ds.Tables[0].DefaultView;
                            // datacam1.ChartAreas.Add("0");

                            datacam1.DataSource = ds;//datacam1.Series.Add("Series1");
                            // datacam1.ChartAreas[0].AxisY.IsStartedFromZero = false;
                            Title da4 = datacam1.Titles.Add("Staff Chart");
                            da4.Font = new System.Drawing.Font("Book Antiqua", 15, FontStyle.Bold);

                            datacam1.ChartAreas[0].AxisX.Interval = 1;
                            // datacam1.ChartAreas[0].AxisY.Maximum = 110;
                            datacam1.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
                            datacam1.ChartAreas[0].AxisY.TitleAlignment = StringAlignment.Center;
                            datacam1.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Book Antiqua", 15, FontStyle.Bold);
                            datacam1.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Book Antiqua", 15, FontStyle.Bold);
                            datacam1.Series["Series1"].IsValueShownAsLabel = true;
                            datacam1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                            datacam1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                            //datacam1.Series["Series1"].Color = Color.Green;
                            datacam1.Series["Series1"].BorderWidth = 3;
                            datacam1.Series["Series1"].Font = new System.Drawing.Font("Trebuchet MS", 9, FontStyle.Bold);
                            //datacam1.ChartAreas[0].AxisY.Minimum = Convert.ToInt32(txtcutoff.Text);
                            datacam1.ChartAreas[0].AxisY.Crossing = 0;



                            Random random = new Random();
                            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                            {
                                if (Convert.ToInt32(ds.Tables[0].Rows[k]["cammark"]) >= Convert.ToInt32(txtcutoff.Text))
                                {
                                    string data4 = ds.Tables[0].Rows[k]["cammark"].ToString();
                                    datacam1.Series["Series1"].Points.AddXY(ds.Tables[0].Rows[k]["staff_name"].ToString(), Convert.ToInt32(data4));
                                }
                                else
                                {
                                    string data4 = "-" + ds.Tables[0].Rows[k]["cammark"];
                                    datacam1.Series["Series1"].Points.AddXY(ds.Tables[0].Rows[k]["staff_name"].ToString(), Convert.ToInt32(data4));
                                }
                            }
                            int f = 0;


                            foreach (Series series in datacam1.Series)
                            {
                                foreach (DataPoint point in series.Points)
                                {
                                    int Y;
                                    string data = point.ToString();
                                    string[] spl_date1 = data.Split(new char[] { ',' });
                                    string data2 = spl_date1[1].ToString();
                                    string[] spl_date2 = data2.Split(new char[] { '-', '=', '}' });
                                    if (spl_date2[2].ToString() == "")
                                    {
                                        Y = Convert.ToInt32(spl_date2[1].ToString());

                                    }
                                    else
                                    {
                                        Y = Convert.ToInt32(spl_date2[2].ToString());
                                    }
                                    if (Y >= Convert.ToInt32(txtcutoff.Text))
                                    {

                                        datacam1.Series["Series1"].Points[f].Color = Color.Green;


                                    }
                                    else
                                    {
                                        point.Color = Color.Red;//  item.Color = c;
                                    }

                                    f++;

                                }
                            }



                            datacam1.ChartAreas[0].AxisX.Title = "Staff Name";
                            datacam1.ChartAreas[0].AxisY.Title = "Pass Percentage";

                            datacam1.DataBind();
                            datacam1.SaveImage(Server.MapPath("App_Data/Sample.jpg"));

                        }
                    }
                    else
                    {
                        Label2.Text = "No Records Found";
                        Label2.Visible = true;
                    }
                }
            }
            else
            {

                double grademark;
                double grademark1;
                DataSet datagrade = new DataSet();
                Boolean flag_university = new Boolean();
                if (DropDownList1.Text == "")
                {
                }
                else
                {
                    sec1 = "and Sections='" + DropDownList1.SelectedItem.Value + "'";
                }
                string sqldatabind = " select distinct Mark_Grade,Credit_Points from Grade_Master where Degree_Code='" + ddldept.SelectedItem.Value + "'  and college_code='" + ddlcollege.SelectedItem.Value + "' and batch_year='" + ddlbatch.SelectedItem.Value + "'order by Credit_Points desc";
                datagrade = da.select_method_wo_parameter(sqldatabind, "text");
                ht.Clear();
                string value = "";
                if (datagrade.Tables[0].Rows.Count > 0)
                {
                    ht.Clear();
                    for (int l = 0; l < datagrade.Tables[0].Rows.Count; l++)
                    {
                        value = datagrade.Tables[0].Rows[0]["Mark_Grade"].ToString();
                        ht.Add(datagrade.Tables[0].Rows[l]["Mark_Grade"], "" + datagrade.Tables[0].Rows[l]["Credit_Points"] + "");
                    }
                }
                string sqlqurey1 = " Select distinct Current_Semester,Exam_Code,exam_month,exam_year from Exam_Details where Degree_Code = '" + ddldept.SelectedItem.Value + "'  and Current_Semester='" + ddlsem.SelectedItem.Value + "' and Batch_Year = '" + ddlbatch.SelectedItem.Value + "'";
                ds2 = da.select_method_wo_parameter(sqlqurey1, "text");
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    string sqlquery = "Select distinct st.staff_code,sm.staff_name,stf.per_mobileno,stf.email,stf.desig_name,stf.dept_name, s.mintotal as mintot,s.min_int_marks as mimark, s.min_ext_marks as mxmark,s.maxtotal as maxtot,s.acronym as subacr,subject_name,subject_code as Subject_Code,mark_entry.subject_no as Subject_No,semester,subject_type as Subtype,credit_points from Mark_Entry,Subject s,sub_sem,syllabus_master,staff_selector st,staffmaster sm,staff_appl_master stf,desig_master dm  where stf.appl_no=sm.appl_no and stf.desig_code=dm.desig_code and sm.college_code=dm.collegeCode and syllabus_master.syll_code=s.syll_code and Mark_Entry.Subject_No = s..Subject_No and s.subtype_no= sub_sem.subtype_no and sm.staff_code=st.staff_code and Exam_Code = '" + ds2.Tables[0].Rows[0]["Exam_Code"] + "' and attempts=1 and st.subject_no=s.subject_no order by semester desc,subject_type desc, mark_entry.subject_no asc";
                    ds = da.select_method_wo_parameter(sqlquery, "text");
                    ds.Tables[0].Columns.Add("cammark", typeof(string));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int g = 0; g < ds.Tables[0].Rows.Count; g++)
                        {
                            if (subject_no.ToString() == "")
                            {
                                subject_no = ds.Tables[0].Rows[g]["subject_no"].ToString();
                            }
                            else
                            {
                                subject_no = subject_no + "," + ds.Tables[0].Rows[g]["subject_no"].ToString();
                            }
                        }
                    }
                    if (subject_no.ToString() != "")
                    {
                        DataSet university = new DataSet();
                        string sqlquery1 = "Select grade_flag from grademaster where exam_month='" + ds2.Tables[0].Rows[0]["exam_month"] + "' and exam_year='" + ds2.Tables[0].Rows[0]["exam_year"] + "' and Batch_year='" + ddlbatch.SelectedItem.Value + "' and degree_code='" + ddldept.SelectedItem.Value + "'";
                        university = da.select_method_wo_parameter(sqlquery1, "text");
                        string sqlroll = "Select acronym as subject_name,subject_code,subject.subject_no,result,total,grade,Registration.roll_no,cp,mark_entry.subject_no,subject.min_ext_marks,subject.min_int_marks,isnull(Mark_Entry.internal_mark,0) as internal_mark ,isnull(Mark_Entry.external_mark,0) as external_mark,semester,Sections from Mark_Entry,Subject,sub_sem,syllabus_master,Registration where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No =Subject.Subject_No and  subject.subtype_no= sub_sem.subtype_no and Registration.Roll_No=mark_entry.roll_no and  Exam_Code in(" + ds2.Tables[0].Rows[0]["Exam_Code"] + ") and Subject.subject_no in (" + subject_no + ") " + sec1 + " and CC=0 and DelFlag=0 and Exam_Flag<>'debar' order by semester desc,subject_type desc,subject.subject_no asc";
                        ds3 = da.select_method_wo_parameter(sqlroll, "Text");
                        if (university.Tables[0].Rows.Count > 0)
                        {

                            if (university.Tables[0].Rows.Count > 0)
                            {
                                if (university.Tables[0].Rows[0]["grade_flag"].ToString() == "2")
                                {

                                    if (ds.Tables[0].Rows.Count > 0)
                                    {

                                        double totalmark = 0;
                                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                        {
                                            grademark = 0.0;
                                            DataView dv = new DataView();
                                            ds3.Tables[0].DefaultView.RowFilter = "subject_code='" + ds.Tables[0].Rows[i]["subject_code"] + "'";
                                            dv = ds3.Tables[0].DefaultView;
                                            for (int j = 0; j < dv.Count; j++)
                                            {

                                                //int count = hat.Count;
                                                if (ds3.Tables[0].Rows[j]["cp"] == DBNull.Value)
                                                {

                                                }
                                                else
                                                {
                                                    string mark = dv[j]["grade"].ToString();
                                                    double mark2 = Convert.ToInt32(ht[value]) * Convert.ToInt32(dv[j]["cp"]);
                                                    double mark1 = 0;

                                                    foreach (DictionaryEntry child in ht)
                                                    {


                                                        if (mark.ToString() == child.Key.ToString())
                                                        {
                                                            if (child.Value.ToString() == "0")
                                                            {



                                                            }
                                                            else
                                                            {
                                                                mark1 = Convert.ToInt32(child.Value) * Convert.ToInt32(dv[j]["cp"]);
                                                                grademark++;
                                                                flag_university = true;
                                                            }
                                                        }

                                                    }
                                                }


                                            }
                                            for (int l = 0; l < ds.Tables[0].Rows.Count; l++)
                                            {
                                                if (ds.Tables[0].Rows[l]["subject_no"].ToString() == dv[0]["subject_no"].ToString())
                                                {
                                                    grademark1 = (grademark / dv.Count) * 100;
                                                    ds.Tables[0].Rows[l]["cammark"] = Math.Round(grademark1);

                                                }
                                            }
                                        }
                                    }
                                }
                                else if (university.Tables[0].Rows[0]["grade_flag"].ToString() == "3")
                                {
                                    string dd = "select linkvalue from inssettings where linkname='corresponding grade' and college_code='" + Session["collegecode"].ToString() + "'";

                                    DataSet df1 = new DataSet();
                                    df1 = da.select_method_wo_parameter(dd, "text");
                                    if (df1.Tables[0].Rows[0]["linkvalue"].ToString() == "0")
                                    {
                                        string sqlroll1 = "Select acronym as subject_name,subject_code,subject.subject_no,result,total,grade,Registration.roll_no,cp,mark_entry.subject_no,subject.min_ext_marks,subject.min_int_marks,isnull(Mark_Entry.internal_mark,0) as internal_mark ,isnull(Mark_Entry.external_mark,0) as external_mark,semester,Sections from Mark_Entry,Subject,sub_sem,syllabus_master,Registration where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No =Subject.Subject_No and  subject.subtype_no= sub_sem.subtype_no and Registration.Roll_No=mark_entry.roll_no and  Exam_Code in(" + ds2.Tables[0].Rows[0]["Exam_Code"] + ") and Subject.subject_no in (" + subject_no + ") " + sec1 + " and CC=0 and DelFlag=0 and Exam_Flag<>'debar' order by semester desc,subject_type desc,subject.subject_no asc";
                                        ds3 = da.select_method_wo_parameter(sqlroll1, "Text");

                                        for (int j = 0; j < ds3.Tables[0].Rows.Count; j++)
                                        {
                                            grademark = 0.0;
                                            DataView dv = new DataView();
                                            ds3.Tables[0].DefaultView.RowFilter = "subject_code='" + ds3.Tables[0].Rows[j]["subject_code"] + "'";
                                            dv = ds3.Tables[0].DefaultView;
                                            for (int k = 0; k < dv.Count; k++)
                                            {
                                                double internal1 = Convert.ToDouble(dv[k]["internal_mark"].ToString());
                                                double external = Convert.ToDouble(dv[k]["external_mark"].ToString());
                                                double mark1 = Convert.ToDouble(dv[k]["min_ext_marks"].ToString());
                                                double mark2 = Convert.ToDouble(dv[k]["min_int_marks"].ToString());
                                                if (external == 0.0 && internal1 == 0.0)
                                                {
                                                    flag_university = false;

                                                }
                                                else
                                                {
                                                    if (internal1 >= mark2 && external >= mark1)
                                                    {
                                                        grademark++;
                                                        flag_university = true;

                                                    }

                                                }
                                            }
                                            for (int l = 0; l < ds.Tables[0].Rows.Count; l++)
                                            {
                                                if (ds.Tables[0].Rows[l]["subject_no"].ToString() == dv[0]["subject_no"].ToString())
                                                {
                                                    grademark1 = (grademark / dv.Count) * 100;
                                                    ds.Tables[0].Rows[l]["cammark"] = Math.Round(grademark1);

                                                }
                                            }
                                        }



                                    }
                                    else if (df1.Tables[0].Rows[0]["linkvalue"].ToString() == "1")
                                    {


                                        double totalmark = 0;
                                        for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                                        {
                                            grademark = 0.0;
                                            DataView dv = new DataView();
                                            ds3.Tables[0].DefaultView.RowFilter = "subject_code='" + ds3.Tables[0].Rows[i]["subject_code"] + "'";
                                            dv = ds3.Tables[0].DefaultView;
                                            for (int j = 0; j < dv.Count; j++)
                                            {

                                                //int count = hat.Count;
                                                if (ds3.Tables[0].Rows[j]["cp"] == DBNull.Value)
                                                {

                                                }
                                                else
                                                {
                                                    string mark = dv[j]["grade"].ToString();
                                                    double mark2 = Convert.ToInt32(ht[value]) * Convert.ToInt32(dv[j]["cp"]);
                                                    double mark1 = 0;

                                                    foreach (DictionaryEntry child in ht)
                                                    {


                                                        if (mark.ToString() == child.Key.ToString())
                                                        {
                                                            if (child.Value.ToString() == "0")
                                                            {



                                                            }
                                                            else
                                                            {
                                                                mark1 = Convert.ToInt32(child.Value) * Convert.ToInt32(dv[j]["cp"]);
                                                                grademark++;
                                                                flag_university = true;
                                                            }
                                                        }

                                                    }
                                                }


                                            }
                                            for (int l = 0; l < ds.Tables[0].Rows.Count; l++)
                                            {
                                                if (ds.Tables[0].Rows[l]["subject_no"].ToString() == dv[0]["subject_no"].ToString())
                                                {
                                                    grademark1 = (grademark / dv.Count) * 100;
                                                    ds.Tables[0].Rows[l]["cammark"] = Math.Round(grademark1);

                                                }
                                            }

                                        }
                                    }
                                }
                                else if (university.Tables[0].Rows[0]["grade_flag"].ToString() == "1")
                                {
                                    string sqlroll1 = "Select acronym as subject_name,subject_code,subject.subject_no,result,total,grade,Registration.roll_no,cp,mark_entry.subject_no,subject.min_ext_marks,subject.min_int_marks,isnull(Mark_Entry.internal_mark,0) as internal_mark ,isnull(Mark_Entry.external_mark,0) as external_mark,semester,Sections from Mark_Entry,Subject,sub_sem,syllabus_master,Registration where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No =Subject.Subject_No and  subject.subtype_no= sub_sem.subtype_no and Registration.Roll_No=mark_entry.roll_no and  Exam_Code in(" + ds2.Tables[0].Rows[0]["Exam_Code"] + ") and Subject.subject_no in (" + subject_no + ") " + sec1 + " and CC=0 and DelFlag=0 and Exam_Flag<>'debar' order by semester desc,subject_type desc,subject.subject_no asc";
                                    ds3 = da.select_method_wo_parameter(sqlroll1, "Text");

                                    for (int j = 0; j < ds3.Tables[0].Rows.Count; j++)
                                    {
                                        grademark = 0.0;
                                        DataView dv = new DataView();
                                        ds3.Tables[0].DefaultView.RowFilter = "subject_code='" + ds3.Tables[0].Rows[j]["subject_code"] + "'";
                                        dv = ds3.Tables[0].DefaultView;
                                        for (int k = 0; k < dv.Count; k++)
                                        {
                                            double internal1 = Convert.ToDouble(dv[k]["internal_mark"].ToString());
                                            double external = Convert.ToDouble(dv[k]["external_mark"].ToString());
                                            double mark1 = Convert.ToDouble(dv[k]["min_ext_marks"].ToString());
                                            double mark2 = Convert.ToDouble(dv[k]["min_int_marks"].ToString());
                                            if (external == 0.0 && internal1 == 0.0)
                                            {
                                                flag_university = false;
                                            }
                                            else
                                            {
                                                if (internal1 >= mark2 && external >= mark1)
                                                {
                                                    grademark++;
                                                    flag_university = true;

                                                }
                                            }
                                        }
                                        for (int l = 0; l < ds.Tables[0].Rows.Count; l++)
                                        {
                                            if (ds.Tables[0].Rows[l]["subject_no"].ToString() == dv[0]["subject_no"].ToString())
                                            {
                                                grademark1 = (grademark / dv.Count) * 100;
                                                ds.Tables[0].Rows[l]["cammark"] = Math.Round(grademark1);

                                            }
                                        }
                                    }
                                }
                            }
                            if (flag_university == true)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {



                                    DataView dv2 = new DataView();
                                    ds.Tables[0].DefaultView.RowFilter = "cammark>='" + txtcutoff.Text + "'";
                                    dv2 = ds.Tables[0].DefaultView;
                                    // datacam1.ChartAreas.Add("0");
                                    //datacam1.Series.Add("Series1");
                                    datacam1.DataSource = ds;
                                    // datacam1.ChartAreas[0].AxisY.IsStartedFromZero = false;
                                    Title da4 = datacam1.Titles.Add("Staff Chart");
                                    da4.Font = new System.Drawing.Font("Book Antiqua", 15, FontStyle.Bold);

                                    datacam1.ChartAreas[0].AxisX.Interval = 1;
                                    // datacam1.ChartAreas[0].AxisY.Maximum = 110;
                                    datacam1.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
                                    datacam1.ChartAreas[0].AxisY.TitleAlignment = StringAlignment.Center;
                                    datacam1.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Book Antiqua", 15, FontStyle.Bold);
                                    datacam1.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Book Antiqua", 15, FontStyle.Bold);
                                    datacam1.Series["Series1"].IsValueShownAsLabel = true;
                                    datacam1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                                    datacam1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                                    //datacam1.Series["Series1"].Color = Color.Green;
                                    datacam1.Series["Series1"].BorderWidth = 3;
                                    datacam1.Series["Series1"].Font = new System.Drawing.Font("Trebuchet MS", 9, FontStyle.Bold);
                                    //datacam1.ChartAreas[0].AxisY.Minimum = Convert.ToInt32(txtcutoff.Text);
                                    datacam1.ChartAreas[0].AxisY.Crossing = 0;



                                    Random random = new Random();
                                    for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                                    {
                                        string data = ds.Tables[0].Rows[k]["cammark"].ToString();
                                        if (data.ToString() != "")
                                        {
                                            if (Convert.ToInt32(data) >= Convert.ToInt32(txtcutoff.Text))
                                            {
                                                string data4 = ds.Tables[0].Rows[k]["cammark"].ToString();
                                                datacam1.Series["Series1"].Points.AddXY(ds.Tables[0].Rows[k]["staff_name"].ToString(), Convert.ToInt32(data4));
                                            }
                                            else
                                            {
                                                string data4 = "-" + ds.Tables[0].Rows[k]["cammark"];
                                                datacam1.Series["Series1"].Points.AddXY(ds.Tables[0].Rows[k]["staff_name"].ToString(), Convert.ToInt32(data4));
                                            }
                                        }
                                        else
                                        {
                                        }
                                    }
                                    int f = 0;


                                    foreach (Series series in datacam1.Series)
                                    {
                                        foreach (DataPoint point in series.Points)
                                        {
                                            int Y;
                                            string data = point.ToString();
                                            string[] spl_date1 = data.Split(new char[] { ',' });
                                            string data2 = spl_date1[1].ToString();
                                            string[] spl_date2 = data2.Split(new char[] { '-', '=', '}' });
                                            if (spl_date2[2].ToString() == "")
                                            {
                                                Y = Convert.ToInt32(spl_date2[1].ToString());

                                            }
                                            else
                                            {
                                                Y = Convert.ToInt32(spl_date2[2].ToString());
                                            }
                                            if (Y >= Convert.ToInt32(txtcutoff.Text))
                                            {

                                                datacam1.Series["Series1"].Points[f].Color = Color.Green;


                                            }
                                            else
                                            {
                                                //string[] spl_date3 = data.Split(new char[] { ',' });
                                                //string data5=spl_date1[0].ToString();
                                                //string[] spl_date4 = data5.Split(new char[] { '=', '}' });
                                                //int data3 = Convert.ToInt32(spl_date4[1].ToString());
                                                //string data6=spl_date1[1].ToString();
                                                //string[] spl_date = data6.Split(new char[] { '=', '}' });
                                                //string  data4 = "-"+spl_date[1].ToString();

                                                //datacam1.Series["Series1"].Points.AddXY(data3,Convert.ToInt32(data4));
                                                point.Color = Color.Red;//  item.Color = c;

                                            }

                                            f++;

                                        }
                                    }



                                    datacam1.ChartAreas[0].AxisX.Title = "Staff Name";
                                    datacam1.ChartAreas[0].AxisY.Title = "Pass Percentage";
                                    datacam1.SaveImage(Server.MapPath("App_Data/Sample.jpg"));
                                    datacam1.DataBind();


                                }
                                else
                                {
                                    Label2.Text = "No Records Found";
                                    Label2.Visible = true;
                                }
                            }
                            else
                            {
                                Label2.Text = "No Records Found";
                                Label2.Visible = true;
                            }

                        }
                        else
                        {
                            Label2.Text = "No Records Found";
                            Label2.Visible = true;
                        }
                    }
                    else
                    {
                        Label2.Text = "No Records Found";
                        Label2.Visible = true;
                    }
                }
                else
                {
                    Label2.Text = "No Records Found";
                    Label2.Visible = true;
                }



            }

        }
        catch
        {
        }


    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch(collegecode);
            bindsem();
            BindSectionDetail();
            test();

        }
        catch
        {
        }

    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch(collegecode);
            bindsem();
            BindSectionDetail();
            test();

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

        }
        catch
        {
        }
    }



    protected void datacam1_Click(object sender, ImageMapEventArgs e)
    {
        try
        {
            camchart();
            datacam1.Visible = true;
            fpcammarkstaff.Visible = true;
            labheading.Visible = true;
            //labpurpose.Visible = true;
            //ddlpurpose.Visible = true;
            //fpspreadpurpose.Visible = true;
            //btnaddtemplate.Visible = true;
            //btndeletetemplate.Visible = true;
            txtmessage.Visible = true;
            if (chknotification.Checked == true)
            {
                Tablenote.Visible = true;
            }
            btnsms.Visible = true;
            //btnxl.Visible = true;
            labheading.Visible = true;
            //labpurpose.Visible = true;
            //ddlpurpose.Visible = true;
            //fpspreadpurpose.Visible = true;
            if (radiobutton1.Text == "CAM Wise")
            {
                labheading.Text = "CAM MARK BASED ON STAFF PERCENTAGE WITH CUTOFF PERCENTAGE=" + txtcutoff.Text + "";
            }
            else
            {
                labheading.Text = "UNIVERSITY MARK BASED ON STAFF PERCENTAGE WITH CUTOFF PERCENTAGE=" + txtcutoff.Text + "";
            }
            //  labheading.Font=Font
            DataView dv = new DataView();

            fpcammarkstaff.Visible = true;
            string dd = e.PostBackValue;
            fpcammarkstaff.Sheets[0].AutoPostBack = false;
            fpcammarkstaff.Sheets[0].RowCount = 0;
            fpcammarkstaff.Sheets[0].ColumnCount = 9;
            fpcammarkstaff.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            fpcammarkstaff.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            fpcammarkstaff.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpcammarkstaff.Sheets[0].RowHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            fpcammarkstaff.Height = 310;
            fpcammarkstaff.Width = 900;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = Color.Black;
            style2.BackColor = Color.AliceBlue;

            //  fpcammarkstaff.VerticalScrollBarPolicy = ScrollBarPolicy.AsNeeded;
            fpcammarkstaff.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            fpcammarkstaff.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

            fpcammarkstaff.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            fpcammarkstaff.Sheets[0].AllowTableCorner = true;
            fpcammarkstaff.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            fpcammarkstaff.Sheets[0].AllowTableCorner = true;
            fpcammarkstaff.Sheets[0].AllowTableCorner = true;
            // fpcammarkstaff.Sheets[0].AutoPostBack = true;

            fpcammarkstaff.Sheets[0].Columns[2].Width = 150;
            fpcammarkstaff.Sheets[0].Columns[3].Width = 150;
            fpcammarkstaff.Sheets[0].Columns[4].Width = 150;
            fpcammarkstaff.Sheets[0].Columns[5].Width = 180;
            fpcammarkstaff.Sheets[0].Columns[6].Width = 100;
            fpcammarkstaff.Sheets[0].Columns[7].Width = 200;
            fpcammarkstaff.Sheets[0].Columns[8].Width = 100;
            fpcammarkstaff.Sheets[0].Columns[1].Width = 50;
            fpcammarkstaff.Sheets[0].Columns[0].Width = 50;
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Designation";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Mail ID";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Mobile No";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Subject Name";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Percentage";

            fpcammarkstaff.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            if (Convert.ToInt32(dd.ToString()) >= Convert.ToInt32(txtcutoff.Text))
            {
                ds.Tables[0].DefaultView.RowFilter = "cammark>=" + txtcutoff.Text + "";
                dv = ds.Tables[0].DefaultView;
            }
            else
            {
                ds.Tables[0].DefaultView.RowFilter = "cammark <" + txtcutoff.Text + "";
                dv = ds.Tables[0].DefaultView;

            }
            if (dv.Count > 0)
            {
                int cn = 0;
                fpcammarkstaff.Sheets[0].RowCount++;
                fpcammarkstaff.Sheets[0].SpanModel.Add(0, 0, 1, 1);
                FarPoint.Web.Spread.CheckBoxCellType chtbox1 = new FarPoint.Web.Spread.CheckBoxCellType();
                fpcammarkstaff.Sheets[0].Cells[0, 1].CellType = chtbox1;
                chtbox1.AutoPostBack = true;
                for (int l = 0; l < dv.Count; l++)
                {
                    fpcammarkstaff.Sheets[0].RowCount++;
                    cn++;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 0].Text = cn.ToString();
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 2].Text = dv[l]["staff_name"].ToString();
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 2].Tag = dv[l]["staff_code"].ToString();
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 3].Text = dv[l]["dept_name"].ToString();
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 4].Text = dv[l]["desig_name"].ToString();
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 5].Text = dv[l]["email"].ToString();
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 6].Text = dv[l]["per_mobileno"].ToString();
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 7].Text = dv[l]["subject_name"].ToString();
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 8].Text = dv[l]["cammark"].ToString();

                    FarPoint.Web.Spread.CheckBoxCellType chtbox = new FarPoint.Web.Spread.CheckBoxCellType();
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 1].CellType = chtbox;
                    chtbox.AutoPostBack = false;
                    fpcammarkstaff.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                    fpcammarkstaff.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                    fpcammarkstaff.Sheets[0].PageSize = fpcammarkstaff.Sheets[0].RowCount;
                    fpcammarkstaff.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    fpcammarkstaff.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    fpcammarkstaff.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
                }
                fpcammarkstaff.Sheets[0].Columns[0].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[8].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[2].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[3].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[4].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[5].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[6].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[7].Locked = true;
                //  fpcammarkstaff.SaveChanges();

            }
            //purpose();
            //spread();
        }
        catch (Exception ex)
        {
        }
    }
    protected void fpcammarkstaff_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            if (Cellclick == false)
            {
                camchart();
                Cellclick = true;
            }
            string activerow = fpcammarkstaff.Sheets[0].ActiveRow.ToString();
            if (flag_select == false && activerow == "0")
            {
                string selecttext = "";
                string actcol = "1";
                selecttext = e.EditValues[Convert.ToInt32(actcol)].ToString();
                for (int i = 1; i < fpcammarkstaff.Sheets[0].RowCount; i++)
                {
                    if (selecttext != "System Object")
                    {
                        fpcammarkstaff.Sheets[0].Cells[i, Convert.ToInt32(actcol)].Text = selecttext.ToString();
                    }

                }
            }
        }
        catch
        {
        }


    }

    //public void purpose()
    //{
    //    labpurpose.Visible = true;
    //    ddlpurpose.Items.Clear();
    //    string sqlquery = "select purpose,temp_code from sms_purpose where college_code = '" + collegecode + "'";
    //    purpo = da.select_method_wo_parameter(sqlquery, "text");

    //    if (purpo.Tables[0].Rows.Count > 0)
    //    {

    //        ddlpurpose.DataSource = purpo;
    //        ddlpurpose.DataTextField = "purpose";
    //        ddlpurpose.DataValueField = "temp_code";
    //        ddlpurpose.DataBind();
    //        ddlpurpose.Items.Insert(0, "");


    //        ddlpurposemsg.DataSource = purpo;
    //        ddlpurposemsg.DataTextField = "purpose";
    //        ddlpurposemsg.DataValueField = "temp_code";
    //        ddlpurposemsg.DataBind();
    //        ddlpurposemsg.Items.Insert(0, "");

    //    }
    //}
    //protected void bindpurpose()
    //{
    //    try
    //    {
    //        fpspreadpurpose.Sheets[0].ColumnHeaderVisible = false;
    //        fpspreadpurpose.Sheets[0].SheetCorner.Columns[0].Visible = false;
    //        fpspreadpurpose.Visible = true;

    //        //lblpurpose1.Visible = true;
    //        ddlpurpose.Visible = true;
    //        fpspreadpurpose.Sheets[0].RowCount = 1;
    //        fpspreadpurpose.Sheets[0].ColumnCount = 2;
    //        fpspreadpurpose.Columns[1].Width = 900;
    //        fpspreadpurpose.Height = 200;
    //        fpspreadpurpose.Sheets[0].AutoPostBack = true;
    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Font.Bold = true;
    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Text = "S.No";
    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Locked = true;

    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Font.Bold = true;
    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Text = "Template";
    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Locked = true;
    //        string gfg = ddlpurpose.SelectedValue.ToString();
    //        //string gfvgj = ddlpurposemsg.Text;


    //        if (gfg == "")
    //        {
    //            ds.Dispose();
    //            ds.Reset();

    //            string spread2query = "select ROW_NUMBER() OVER (ORDER BY  Template) as SrNo,Template from sms_template";
    //            ds = da.select_method_wo_parameter(spread2query, "Text");
    //        }
    //        else
    //        {
    //            string spread2query1 = "select ROW_NUMBER() OVER (ORDER BY  Template) as SrNo,Template from sms_template where temp_code = " + ddlpurpose.SelectedValue + "";
    //            ds = da.select_method_wo_parameter(spread2query1, "Text");
    //        }


    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            for (int dscnt = 0; dscnt < ds.Tables[0].Rows.Count; dscnt++)
    //            {
    //                fpspreadpurpose.Sheets[0].RowCount++;
    //                fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
    //                fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
    //                fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
    //                fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["SrNo"]);

    //                fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
    //                fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
    //                fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["Template"]);
    //            }
    //        }
    //        fpspreadpurpose.Sheets[0].PageSize = fpspreadpurpose.Sheets[0].RowCount;
    //        fpspreadpurpose.SaveChanges();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }


    //}
    //protected void ddlpurpose_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    bindpurpose();
    //}

    //protected void spread()
    //{
    //    try
    //    {
    //        fpspreadpurpose.Sheets[0].ColumnHeaderVisible = false;

    //        fpspreadpurpose.Sheets[0].SheetCorner.Columns[0].Visible = false;
    //        //FpSpread2.Visible = true;

    //        //lblpurpose1.Visible = true;
    //        //ddlpurpose.Visible = true;
    //        fpspreadpurpose.Sheets[0].RowCount = 1;
    //        fpspreadpurpose.Sheets[0].ColumnCount = 2;
    //        fpspreadpurpose.Columns[1].Width = 900;
    //        fpspreadpurpose.Height = 100;
    //        fpspreadpurpose.Sheets[0].AutoPostBack = true;
    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Font.Bold = true;
    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Text = "S.No";
    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Locked = true;
    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Font.Bold = true;
    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Text = "Template";
    //        fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Locked = true;


    //        string spread2query1 = "select ROW_NUMBER() OVER (ORDER BY  Template) as SrNo,Template from sms_template";
    //        ds = da.select_method_wo_parameter(spread2query1, "Text");

    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            for (int dscnt = 0; dscnt < ds.Tables[0].Rows.Count; dscnt++)
    //            {
    //                fpspreadpurpose.Sheets[0].RowCount++;
    //                fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
    //                fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
    //                fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
    //                fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["SrNo"]);

    //                fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
    //                fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
    //                fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["Template"]);
    //            }
    //        }
    //        fpspreadpurpose.Sheets[0].PageSize = fpspreadpurpose.Sheets[0].RowCount;
    //        fpspreadpurpose.SaveChanges();
    //        fpspreadpurpose.Sheets[0].Columns[0].Locked = true;
    //        fpspreadpurpose.Sheets[0].Columns[1].Locked = true;

    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //protected void btnaddtemplate_Click(object sender, EventArgs e)
    //{
    //    camchart();
    //    templatepanel.Visible = true;
    //}
    //protected void btndeletetemplate_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        camchart();
    //        Cellclick = true;

    //        if (Cellclick == true)
    //        {
    //            string activerow = "";
    //            string activecol = "";
    //            activerow = fpspreadpurpose.ActiveSheetView.ActiveRow.ToString();
    //            activecol = fpspreadpurpose.ActiveSheetView.ActiveColumn.ToString();
    //            int ar;
    //            int ac;
    //            ar = Convert.ToInt32(activerow.ToString());
    //            ac = Convert.ToInt32(activecol.ToString());
    //            if (ar != -1)
    //            {
    //                string msg = fpspreadpurpose.Sheets[0].GetText(ar, 1);
    //                string strdeletequery = "delete   sms_template where Template='" + msg + "'";
    //                int vvv = da.insert_method(strdeletequery, ht, "text");

    //                if (vvv == 1)
    //                {
    //                    lblerror.Visible = true;
    //                    lblerror.Text = "Delete Template Succefully";
    //                }
    //                else
    //                {
    //                    lblerror.Text = "Delete Template  failed";
    //                }
    //            }
    //            spread();
    //            Cellclick = false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }

    //}
    //protected void fpspreadpurpose_CellClick(object sender, EventArgs e)
    //{
    //    Cellclick = true;
    //}
    //protected void fpspreadpurpose_SelectedIndexChanged(Object sender, EventArgs e)
    //{
    //    Cellclick = true;

    //    if (Cellclick == true)
    //    {
    //        string activerow = "";
    //        string activecol = "";
    //        activerow = fpspreadpurpose.ActiveSheetView.ActiveRow.ToString();
    //        activecol = fpspreadpurpose.ActiveSheetView.ActiveColumn.ToString();
    //        int ar;
    //        int ac;
    //        ar = Convert.ToInt32(activerow.ToString());
    //        ac = Convert.ToInt32(activecol.ToString());
    //        if (ar != -1)
    //        {
    //            txtmessage.Text = fpspreadpurpose.Sheets[0].GetText(ar, 1);
    //        }
    //        Cellclick = false;
    //    }
    //}
    //protected void btnpurposeadd_Click(object sender, EventArgs e)
    //{
    //    camchart();
    //    int i = 0;
    //    string purposemessage = txtpurposecaption.Text;
    //    if (purposemessage != "")
    //    {
    //        string sqlquery = "insert into sms_purpose (Purpose,college_code) values ( '" + purposemessage + "','" + ddlcollege.SelectedValue.ToString() + "') ";
    //        i = da.insert_method(sqlquery, ht, "text");
    //        if (i != 0)
    //        {
    //            purpose();
    //            ddlpurposemsg.SelectedIndex = ddlpurposemsg.Items.IndexOf(ddlpurposemsg.Items.FindByText(txtpurposecaption.Text.Trim()));
    //            ddlpurpose.SelectedIndex = ddlpurposemsg.Items.IndexOf(ddlpurposemsg.Items.FindByText(txtpurposecaption.Text.Trim()));
    //        }
    //    }

    //}
    //protected void btnpurposeexit_Click(object sender, EventArgs e)
    //{
    //    camchart();
    //    purposepanel.Visible = false;
    //    templatepanel.Visible = false;

    //}
    //protected void btnexit_Click(object sender, EventArgs e)
    //{
    //    camchart();
    //    templatepanel.Visible = false;
    //}

    //protected void btnsave_Click(object sender, EventArgs e)
    //{
    //    camchart();
    //    int i = 0;
    //    string content = txtpurposemsg.Text;
    //    string sqlquery = "insert into sms_template (temp_code,Template,college_code) values ('" + ddlpurposemsg.SelectedItem.Value + "','" + content + "','" + ddlcollege.SelectedItem.Value + "') ";
    //    i = da.insert_method(sqlquery, ht, "text");
    //    if (i != 0)
    //    {
    //        purpose();

    //    }

    //}

    //protected void btnminus_Click(object sender, EventArgs e)
    //{

    //    try
    //    {
    //        camchart();
    //        int i = 0;

    //        string strdelpurpose = "Delete from sms_purpose where temp_code = '" + ddlpurposemsg.SelectedValue + "'";
    //        i = da.insert_method(strdelpurpose, ht, "Text");
    //        if (i == 1)
    //        {
    //            lblerror.Text = "Purpose deleted Successfully";
    //            lblerror.Visible = true;
    //            purpose();
    //        }
    //        else
    //        {
    //            lblerror.Text = "Purpose deleted Failed";
    //            lblerror.Visible = true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblerror.Text = ex.ToString();
    //        lblerror.Visible = true;
    //    }
    //}
    //protected void btnplus_Click(object sender, EventArgs e)
    //{
    //    camchart();
    //    purposepanel.Visible = true;



    //}
    protected void btnnotfsave_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean check_flag = false;
            if (Cellclick == false)
            {
                camchart();
                Cellclick = true;
            }
            if (txtsubject.Text == "" && txtnotification.Text == "" && !fudfile.HasFile && !fudattachemnts.HasFile)
            {
                lblnotification.Visible = true;
                lblnotification.Text = "Please Give All Details And Then Proceed";
            }
            else
            {
                string viewer = "", notificationdate = "", subject = "", notifiaction = "", filetype = "", isstaff = "", staus = "0", dtdate = "", dttime = "";

                string file_extension = "", file_type = "";
                int fileSize = 0;
                byte[] documentBinary = new byte[0];

                byte[] attchementfile = new byte[0];
                int attachfile = 0;
                string attchefileexten = "", attachfiletype = "";
                Boolean atchflag = false;
                string filename = "";
                if (fudattachemnts.HasFile)
                {
                    if (fudattachemnts.FileName.EndsWith(".txt") || fudattachemnts.FileName.EndsWith(".pdf") || fudattachemnts.FileName.EndsWith(".doc") || fudattachemnts.FileName.EndsWith(".xls") || fudattachemnts.FileName.EndsWith(".xlsx") || fudattachemnts.FileName.EndsWith(".docx"))
                    {
                        atchflag = true;
                        attachfile = fudattachemnts.PostedFile.ContentLength;
                        attchementfile = new byte[attachfile];
                        fudattachemnts.PostedFile.InputStream.Read(attchementfile, 0, attachfile);
                        filename = fudattachemnts.PostedFile.FileName;
                        attchefileexten = Path.GetExtension(fudattachemnts.PostedFile.FileName);
                        attachfiletype = Get_file_format(attchefileexten);
                    }
                }

                Boolean fle = false;
                if (fudfile.HasFile)
                {
                    if (fudfile.FileName.EndsWith(".jpg") || fudfile.FileName.EndsWith(".jpeg") || fudfile.FileName.EndsWith(".JPG") || fudfile.FileName.EndsWith(".gif") || fudfile.FileName.EndsWith(".png"))
                    {
                        fle = true;
                        fileSize = fudfile.PostedFile.ContentLength;
                        documentBinary = new byte[fileSize];
                        fudfile.PostedFile.InputStream.Read(documentBinary, 0, fileSize);

                        file_extension = Path.GetExtension(fudfile.PostedFile.FileName);
                        file_type = Get_file_format(file_extension);
                    }
                }

                string senderid = "", senderstaff = "0", descrip = "";

                string staffcode = Session["Staff_Code"].ToString();
                ds.Reset();
                ds.Dispose();
                string strquery = "";
                if (staffcode != "" && staffcode != null)
                {
                    senderstaff = "1";

                    strquery = "select Staff_name,dm.desig_name,hm.dept_name from staffmaster sm,stafftrans st,Desig_Master dm,HRDept_Master hm where sm.staff_code=st.staff_code and st.desig_code=dm.desig_code and st.dept_code=hm.dept_code and sm.staff_code='" + staffcode + "'";
                    ds = da.select_method_wo_parameter(strquery, "Text");
                    ds = da.select_method_wo_parameter(strquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        senderid = ds.Tables[0].Rows[0]["Staff_name"].ToString();
                        descrip = ds.Tables[0].Rows[0]["desig_name"].ToString() + " - " + ds.Tables[0].Rows[0]["dept_name"].ToString();
                    }
                }
                else
                {
                    if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                    {
                        group_user = Session["group_code"].ToString();
                        if (group_user.Contains(';'))
                        {
                            string[] group_semi = group_user.Split(';');
                            group_user = group_semi[0].ToString();
                        }
                        strquery = "select full_name,description from usermaster where group_code='" + group_user + "'";
                    }
                    else
                    {
                        strquery = "select full_name,description from usermaster where user_code='" + Session["UserCode"].ToString() + "' ";
                    }
                    usercode = Session["usercode"].ToString();
                    group_user = Session["group_code"].ToString();

                    ds = da.select_method_wo_parameter(strquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        senderid = ds.Tables[0].Rows[0]["full_name"].ToString();
                        descrip = ds.Tables[0].Rows[0]["description"].ToString();
                    }
                }



                Boolean saveflag = false;
                dtdate = DateTime.Now.ToString("MM/dd/yyyy");
                dttime = DateTime.Now.ToLongTimeString();
                subject = txtsubject.Text.ToString().Trim();
                notifiaction = txtnotification.Text.ToString().Trim();
                fpcammarkstaff.SaveChanges();
                if (notifiaction == null || notifiaction == "")
                {

                    return;
                }
                if (notifiaction.Length > 8999)
                {

                    return;
                }
                for (int i = 1; i < fpcammarkstaff.Sheets[0].RowCount; i++)
                {
                    int gam = Convert.ToInt32(fpcammarkstaff.Sheets[0].Cells[i, 1].Value);
                    if (gam == 1)
                    {
                        check_flag = true;
                        viewer = fpcammarkstaff.Sheets[0].Cells[i, 2].Tag.ToString();
                        string query = "";
                        isstaff = "1";
                        if (fle == false && atchflag == false)
                        {
                            query = "insert into tbl_notification(viewrs,notification_date,notification_time,subject,notification,isstaff,College_code,status,staff_code,sender_id,Sender_Description,sender_staff)";
                            query = query + "  values('" + viewer + "','" + dtdate + "','" + dttime + "','" + subject + "','" + notifiaction + "','" + isstaff + "','" + ddlcollege.SelectedItem.Value + "','" + staus + "','" + staffcode + "','" + senderid + "','" + descrip + "','" + senderstaff + "')";

                        }
                        else if (fle == true && atchflag == false)
                        {
                            query = "insert into tbl_notification(viewrs,notification_date,notification_time,subject,notification,filetype,fileupload,isstaff,College_code,status,staff_code,sender_id,Sender_Description,sender_staff)";
                            query = query + "  values('" + viewer + "','" + dtdate + "','" + dttime + "','" + subject + "','" + notifiaction + "','" + file_type + "','" + documentBinary + "','" + isstaff + "','" + ddlcollege.SelectedItem.Value + "','" + staus + "','" + staffcode + "','" + senderid + "','" + descrip + "','" + senderstaff + "')";
                        }
                        else if (fle == false && atchflag == true)
                        {
                            query = "insert into tbl_notification(viewrs,notification_date,notification_time,subject,notification,isstaff,College_code,status,staff_code,sender_id,Sender_Description,sender_staff,attche_filetype,attache_file,filename)";
                            query = query + "  values(('" + viewer + "','" + dtdate + "','" + dttime + "','" + subject + "','" + notifiaction + "','" + isstaff + "','" + ddlcollege.SelectedItem.Value + "','" + staus + "','" + staffcode + "','" + senderid + "','" + descrip + "','" + senderstaff + "','" + attachfiletype + "','" + attchementfile + "','" + filename + "')";
                        }
                        else if (fle == true && atchflag == true)
                        {
                            query = "insert into tbl_notification(viewrs,notification_date,notification_time,subject,notification,isstaff,College_code,status,staff_code,sender_id,Sender_Description,sender_staff,attche_filetype,attache_file,filetype,fileupload,filename)";
                            query = query + "  values('" + viewer + "','" + dtdate + "','" + dttime + "','" + subject + "','" + notifiaction + "','" + isstaff + "','" + ddlcollege.SelectedItem.Value + "','" + staus + "','" + staffcode + "','" + senderid + "','" + descrip + "','" + senderstaff + "','" + attachfiletype + "','" + attchementfile + "','" + file_type + "','" + documentBinary + "','" + filename + "')";
                        }

                        int d = da.insert_method(query, ht, "text");
                        saveflag = true;
                    }

                }

                txtnotification.Text = "";
                txtsubject.Text = "";
                if (check_flag == true)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert(' Saved Successfully')", true);
                }
                else
                {
                    lblnotification.Visible = true;
                    lblnotification.Text = "Please Select Any One Staff And Then Proceed";

                }

            }
        }
        catch
        {
        }

    }
    public string Get_file_format(string file_extension)
    {
        try
        {
            string file_type = "";
            switch (file_extension)
            {

                case ".pdf":
                    file_type = "application/pdf";
                    break;

                case ".txt":
                    file_type = "application/notepad";
                    break;

                case ".xls":
                    file_type = "application/vnd.ms-excel";
                    break;

                case ".xlsx":
                    file_type = "application/vnd.ms-excel";
                    break;

                case ".doc":
                    file_type = "application/vnd.ms-word";
                    break;

                case ".docx":
                    file_type = "application/vnd.ms-word";
                    break;

                case ".gif":
                    file_type = "image/gif";
                    break;

                case ".png":
                    file_type = "image/png";
                    break;

                case ".jpg":
                    file_type = "image/jpg";
                    break;

                case ".jpeg":
                    file_type = "image/jpeg";
                    break;

            }
            return file_type;
        }
        catch
        {
            return null;
        }
    }
    protected void btnsms_Click(object sender, EventArgs e)
    {
        try
        {
            if (Cellclick == false)
            {
                camchart();
                Cellclick = true;
            }
            Boolean check_flag = false;
            Boolean send = false;
            datacam1.Visible = true;

            if (chkboxsms.Checked == true)
            {
                string collegeusercode = string.Empty;

                string sqlcollege = "select SMS_User_ID,college_code from track_value where college_code='" + ddlcollege.SelectedItem.Value + "'";
                ds = da.select_method_wo_parameter(sqlcollege, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    user_id = ds.Tables[0].Rows[0]["SMS_User_ID"].ToString();
                }
                GetUserapi(user_id);
                message = txtmessage.Text;
                //string messagetext=txtmessage.Text;
                //string[] split1 = message.Split(new char[] {'$'});
                int k = 0;
                fpcammarkstaff.SaveChanges();
                for (int j = 1; j < fpcammarkstaff.Sheets[0].RowCount; j++)
                {
                    int gam = Convert.ToInt32(fpcammarkstaff.Sheets[0].Cells[j, 1].Value);

                    if (gam == 1)
                    {
                        check_flag = true;
                        strmobileno = fpcammarkstaff.Sheets[0].Cells[j, 6].Text;
                        string staffname = fpcammarkstaff.Sheets[0].Cells[j, 2].Text;
                        string mark = fpcammarkstaff.Sheets[0].Cells[j, 8].Text;
                        string subjectname = fpcammarkstaff.Sheets[0].Cells[j, 7].Text;
                        string department = ddldept.SelectedItem.Text;
                        string batchyear = ddlbatch.SelectedItem.Text;
                        string test = DropDownList2.SelectedItem.Text;
                        if (radiobutton1.Text == "University Wise")
                        {
                            message1 = "Dear" + " " + staffname + " " + "You got" + " " + mark + "%" + " " + "for University" + " " + " performance in" + ddlsem.Text + " " + "Sem " + department + " " + "for Subject" + " " + subjectname + "." + "Thank you !!!";
                        }
                        else if (radiobutton1.Text == "CAM Wise")
                        {
                            message1 = "Dear" + " " + staffname + " " + "You got" + " " + mark + "%" + " " + "for CAM in" + " " + test + " " + " performance in" + ddlsem.Text + " " + "Sem " + department + " " + "for Subject" + " " + subjectname + "." + "Thank you !!!";
                        }

                        if (strmobileno != "Nil" && strmobileno != "")
                        {
                            mobilenos = strmobileno.ToString();
                            //string strpath1 = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobilenos + "&text=" + message + " " + message1 + "&priority=ndnd&stype=normal";
                            //string isstf = "1";
                            //smsreport(strpath1, isstf);
                            int nofosmssend = da.send_sms(user_id, ddlcollege.SelectedItem.Value, Session["usercode"].ToString(), mobilenos, message, "0");
                            send = true;

                        }
                        else
                        {
                            k++;
                        }
                    }
                }
                if (k == 0)
                {

                }
                else
                {
                    lblerror.Text = "Phone no is not avaliable for " + k + " Staffs";
                    lblerror.Visible = true;

                }


                if (chkboxmail.Checked == false)
                {
                    txtmessage.Text = "";
                }

            }


            if (chkboxmail.Checked == true)
            {
                message = txtmessage.Text;
                string strquery = "select massemail,masspwd from collinfo where college_code = " + ddlcollege.SelectedValue.ToString() + " ";
                ds = da.select_method_wo_parameter(strquery, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    mailid = ds.Tables[0].Rows[0]["massemail"].ToString();
                    mailpwd = ds.Tables[0].Rows[0]["masspwd"].ToString();
                }
                int j = 0;
                fpcammarkstaff.SaveChanges();
                for (int l = 1; l < fpcammarkstaff.Sheets[0].RowCount; l++)
                {
                    int isval = Convert.ToInt32(fpcammarkstaff.Sheets[0].Cells[l, 1].Value);
                    if (isval == 1)
                    {

                        check_flag = true;
                        strmobileno = fpcammarkstaff.Sheets[0].Cells[l, 6].Text;
                        string staffname = fpcammarkstaff.Sheets[0].Cells[l, 2].Text;
                        string mark = fpcammarkstaff.Sheets[0].Cells[l, 8].Text;
                        string subjectname = fpcammarkstaff.Sheets[0].Cells[l, 7].Text;
                        string department = ddldept.SelectedItem.Text;
                        string batchyear = ddlbatch.SelectedItem.Text;
                        string test = DropDownList2.SelectedItem.Text;
                        if (radiobutton1.Text == "University Wise")
                        {
                            message1 = "You got" + " " + mark + "%" + " " + "for University" + " " + " performance in " + " " + ddlsem.Text + " " + "Semester " + " " + department + " " + "for Subject " + " " + subjectname;
                        }
                        else if (radiobutton1.Text == "CAM Wise")
                        {
                            message1 = "You got" + " " + mark + "%" + " " + "for CAM in" + " " + test + " " + " performance in " + " " + ddlsem.Text + " " + "Semester " + " " + department + " " + "for Subject " + " " + subjectname;
                        }
                        strstuname = (fpcammarkstaff.Sheets[0].Cells[l, 2].Text);
                        to_mail = (fpcammarkstaff.Sheets[0].Cells[l, 5].Text);
                        if (to_mail.ToString() != "")
                        {
                            SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                            MailMessage mailmsg = new MailMessage();
                            MailAddress mfrom = new MailAddress(mailid);
                            mailmsg.From = mfrom;
                            mailmsg.To.Add(to_mail);
                            mailmsg.Subject = "Report";
                            mailmsg.IsBodyHtml = true;
                            mailmsg.Body = "Hi  ";
                            mailmsg.Body = mailmsg.Body + strstuname;
                            mailmsg.Body = mailmsg.Body + "<br>";
                            mailmsg.Body = mailmsg.Body + message;
                            mailmsg.Body = mailmsg.Body + message1;
                            mailmsg.Body = mailmsg.Body + "<br><br>Thank You...";
                            Mail.EnableSsl = true;
                            NetworkCredential credentials = new NetworkCredential(mailid, mailpwd);
                            Mail.UseDefaultCredentials = false;
                            Mail.Credentials = credentials;
                            Mail.Send(mailmsg);
                            send = true;
                        }
                        else
                        {
                            j++;
                        }
                    }

                }
                if (j == 0)
                {
                }
                else
                {
                    Label1.Text = "Mail ID is not avaliable for " + j + " Staffs";
                    Label1.Visible = true;
                }
            }
            if (send == true)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert(' Send Successfully')", true);
            }
            if (check_flag == true)
            {
            }
            else
            {
                Label1.Text = "Please Select Any One Staff and Then Proceed";
                Label1.Visible = true;

            }
        }
        catch
        {
        }

    }


    public void smsreport(string uril, string isstaff)
    {
        try
        {
            string date = DateTime.Now.ToString("MM/dd/yyyy");
            WebRequest request = WebRequest.Create(uril);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader sr = new StreamReader(data);
            string strvel = sr.ReadToEnd();

            string groupmsgid = "";
            groupmsgid = strvel.Trim().ToString(); //aruna 02oct2013 strvel;       

            int sms = 0;
            string smsreportinsert = "";

            string[] split_mobileno = mobilenos.Split(new Char[] { ',' });

            for (int icount = 0; icount <= split_mobileno.GetUpperBound(0); icount++)
            {
                smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date,sender_id)values( '" + split_mobileno[icount] + "','" + groupmsgid + "','" + message + "," + message1 + "','" + ddlcollege.SelectedValue.ToString() + "','" + isstaff + "','" + date + "','" + Session["UserCode"].ToString() + "')"; // Added by jairam 21-11-2014
                sms = da.insert_method(smsreportinsert, ht, "Text");
            }
        }
        catch (Exception ex)
        {
        }

    }
    public void GetUserapi(string user_id)
    {
        try
        {
            if (user_id == "AAACET")
            {
                SenderID = "AAACET";
                Password = "AAACET";
            }
            else if (user_id == "AALIME")
            {
                SenderID = "AALIME";
                Password = "AALIME";
            }
            else if (user_id == "ACETVM")
            {
                SenderID = "ACETVM";
                Password = "ACETVM";
            }
            else if (user_id == "AGNICT")
            {
                SenderID = "AGNICT";
                Password = "AGNICT";
            }
            else if (user_id == "AMSPTC")
            {
                SenderID = "AMSPTC";
                Password = "AMSPTC";
            }
            else if (user_id == "ANGE")
            {
                SenderID = "ANGE";
                Password = "ANGE";
            }
            else if (user_id == "ARASUU")
            {
                SenderID = "ARASUU";
                Password = "ARASUU";
            }
            else if (user_id == "DAVINC")
            {
                SenderID = "DAVINC";
                Password = "DAVINC";
            }
            else if (user_id == "EASACG")
            {
                SenderID = "EASACG";
                Password = "EASACG";
            }
            else if (user_id == "ECESMS")
            {
                SenderID = "ECESMS";
                Password = "ECESMS";
            }
            else if (user_id == "ESECED")
            {
                SenderID = "ESECED";
                Password = "ESECED";
            }
            else if (user_id == "ESENGG")
            {
                SenderID = "ESENGG";
                Password = "ESENGG";
            }
            else if (user_id == "ESEPTC")
            {
                SenderID = "ESEPTC";
                Password = "ESEPTC";
            }
            else if (user_id == "ESMSCH")
            {
                SenderID = "ESMSCH";
                Password = "ESMSCH";
            }
            else if (user_id == "GKMCET")
            {
                SenderID = "GKMCET";
                Password = "GKMCET";
            }
            else if (user_id == "IJAYAM")
            {
                SenderID = "IJAYAM";
                Password = "IJAYAM";
            }
            else if (user_id == "JJAAMC")
            {
                SenderID = "JJAAMC";
                Password = "JJAAMC";
            }

            else if (user_id == "KINGSE")
            {
                SenderID = "KINGSE";
                Password = "KINGSE";
            }
            else if (user_id == "KNMHSS")
            {
                SenderID = "KNMHSS";
                Password = "KNMHSS";
            }
            else if (user_id == "KSRIET")
            {
                SenderID = "KSRIET";
                Password = "KSRIET";
            }
            else if (user_id == "KTVRKP")
            {
                SenderID = "KTVRKP";
                Password = "KTVRKP";
            }
            else if (user_id == "MPNMJS")
            {
                SenderID = "MPNMJS";
                Password = "MPNMJS";
            }
            else if (user_id == "NANDHA")
            {
                SenderID = "NANDHA";
                Password = "NANDHA";
            }
            else if (user_id == "NECARE")
            {
                SenderID = "NECARE";
                Password = "NECARE";
            }
            else if (user_id == "NSNCET")
            {
                SenderID = "NSNCET";
                Password = "NSNCET";
            }
            else if (user_id == "PETENG")
            {
                SenderID = "PETENG";
                Password = "PETENG";
            }
            else if (user_id == "PMCTEC")
            {
                SenderID = "PMCTEC";
                Password = "PMCTEC";
            }
            else if (user_id == "PPGITS")
            {
                SenderID = "PPGITS";
                Password = "PPGITS";
            }
            else if (user_id == "PROFCL")
            {
                SenderID = "PROFCL";
                Password = "PROFCL";
            }
            else if (user_id == "PSVCET")
            {
                SenderID = "PSVCET";
                Password = "PSVCET";
            }
            else if (user_id == "SASTH")
            {
                SenderID = "SASTH";
                Password = "SASTH";
            }
            else if (user_id == "SCTSBS")
            {
                SenderID = "SCTSBS";
                Password = "SCTSBS";
            }
            else if (user_id == "SCTSCE")
            {
                SenderID = "SCTSCE";
                Password = "SCTSCE";
            }
            else if (user_id == "SCTSEC")
            {
                SenderID = "SCTSEC";
                Password = "SCTSEC";
            }
            else if (user_id == "SKCETC")
            {
                SenderID = "SKCETC";
                Password = "SKCETC";
            }
            else if (user_id == "SRECCG")
            {
                SenderID = "SRECCG";
                Password = "SRECCG";
            }
            else if (user_id == "SLAECT")
            {
                SenderID = "SLAECT";
                Password = "SLAECT";
            }
            else if (user_id == "SSCENG")
            {
                SenderID = "SSCENG";
                Password = "SSCENG";
            }
            else if (user_id == "SSMCEE")
            {
                SenderID = "SSMCEE";
                Password = "SSMCEE";
            }
            else if (user_id == "SVICET")
            {
                SenderID = "SVICET";
                Password = "SVICET";
            }
            else if (user_id == "SVCTCG")
            {
                SenderID = "SVCTCG";
                Password = "SVCTCG";
            }
            else if (user_id == "SVSCBE")
            {
                SenderID = "SVSCBE";
                Password = "SVSCBE";
            }
            else if (user_id == "TECENG")
            {
                SenderID = "TECENG";
                Password = "TECENG";
            }
            else if (user_id == "TJENGG")
            {
                SenderID = "TJENGG";
                Password = "TJENGG";
            }
            else if (user_id == "TSMJCT")
            {
                SenderID = "TSMJCT";
                Password = "TSMJCT";
            }
            else if (user_id == "VCWSMS")
            {
                SenderID = "VCWSMS";
                Password = "VCWSMS";
            }
            else if (user_id == "VRSCET")
            {
                SenderID = "VRSCET";
                Password = "VRSCET";
            }
            else if (user_id == "AUDIIT")
            {
                SenderID = "AUDIIT";
                Password = "AUDIIT";
            }
            else if (user_id == "SAENGG")
            {
                SenderID = "SAENGG";
                Password = "SAENGG";
            }

            else if (user_id == "STANE")
            {
                SenderID = "STANES";
                Password = "STANES";
            }

            else if (user_id == "MBCBSE")
            {
                SenderID = "MBCBSE";
                Password = "MBCBSE";
            }

            else if (user_id == "HIETPT")
            {
                SenderID = "HIETPT";
                Password = "HIETPT";
            }

            else if (user_id == "SVPITM")
            {
                SenderID = "SVPITM";
                Password = "SVPITM";
            }

            else if (user_id == "AUDCET")
            {
                SenderID = "AUDCET";
                Password = "AUDCET";
            }
            else if (user_id == "AUDWOM")
            {
                SenderID = "AUDWOM";
                Password = "AUDWOM";
            }

            else if (user_id == "AUDIPG")
            {
                SenderID = "AUDIPG";
                Password = "AUDIPG";
            }

            else if (user_id == "MCCDAY")
            {
                SenderID = "MCCDAY";
                Password = "MCCDAY";
            }

            else if (user_id == "MCCSFS")
            {
                SenderID = "MCCSFS";
                Password = "MCCSFS";
            }

            Session["api"] = user_id;
            Session["senderid"] = SenderID;
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
        }
        catch
        {
        }


    }
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindyear();
            bindcourse();
            bindbranch(ddlcollege.SelectedItem.Value);
            bindsem();
            BindSectionDetail();
            test();
        }
        catch
        {
        }



    }
    public void bindsem()
    {
        try
        {
            //--------------------semester load
            DataSet ds3 = new DataSet();
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
            DropDownList1.Items.Clear();
            string branch = ddldept.SelectedValue.ToString();
            string batch = ddlbatch.SelectedValue.ToString();

            string sqlquery = "select distinct sections from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddldept.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'";

            DataSet ds = new DataSet();
            ds = da.select_method_wo_parameter(sqlquery, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DropDownList1.DataSource = ds;
                DropDownList1.DataTextField = "sections";
                DropDownList1.DataValueField = "sections";
                DropDownList1.DataBind();
            }
            //ddlSec.Items.Insert(0, new ListItem("--Select--", "-1"));

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["sections"].ToString() == "")
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
        catch
        {
        }
    }

    //protected void txtcutoff_TextChanged(object sender, EventArgs e)
    //{
    //    camchart();
    //}
    protected void chkboxsms_CheckedChangeds(object sender, EventArgs e)
    {
        fpcammarkstaff.Visible = false;
        labheading.Visible = false;
        //labpurpose.Visible = false;
        //ddlpurpose.Visible = false;
        //fpspreadpurpose.Visible = false;
        //btnaddtemplate.Visible = false;
        //btndeletetemplate.Visible = false;
        txtmessage.Visible = false;
        Tablenote.Visible = false;
        btnsms.Visible = false;

    }
    protected void chkboxmail_CheckedChanged(object sender, EventArgs e)
    {
        fpcammarkstaff.Visible = false;
        labheading.Visible = false;
        //labpurpose.Visible = false;
        //ddlpurpose.Visible = false;
        //fpspreadpurpose.Visible = false;
        //btnaddtemplate.Visible = false;
        //btndeletetemplate.Visible = false;
        txtmessage.Visible = false;
        Tablenote.Visible = false;
        btnsms.Visible = false;
    }

    protected void chknotification_CheckedChanged(object sender, EventArgs e)
    {
        fpcammarkstaff.Visible = false;
        labheading.Visible = false;
        //labpurpose.Visible = false;
        //ddlpurpose.Visible = false;
        //fpspreadpurpose.Visible = false;
        //btnaddtemplate.Visible = false;
        //btndeletetemplate.Visible = false;
        txtmessage.Visible = false;
        Tablenote.Visible = false;
        btnsms.Visible = false;
    }
    protected void radiobutton1_selectedindexchanged(object sender, EventArgs e)
    {
        if (radiobutton1.Text == "University Wise")
        {

            DropDownList2.Enabled = false;
        }
        else
        {

            DropDownList2.Enabled = true;
        }
    }



}


