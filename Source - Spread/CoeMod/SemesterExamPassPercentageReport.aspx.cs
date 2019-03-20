using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class CoeMod_SemesterExamPassPercentageNew : System.Web.UI.Page
{
    #region Field Declaration

    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string batch_year = string.Empty;
    string degree_code = string.Empty;
    string semester = string.Empty;
    string section = string.Empty;
    string subject_no = string.Empty;
    string exam_type = string.Empty;
    string exam_code = string.Empty;
    string questionid = string.Empty;
    string edulevel = string.Empty;
    string exam_month = string.Empty;
    string exam_yr = string.Empty;
    string calculate = string.Empty;
    string qry = string.Empty;
    string course_id = string.Empty;

    connection connection = new connection();
    SqlCommand cmd;
    SqlDataAdapter adaload;

    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();

    DataSet daload = new DataSet();
    DataSet ds = new DataSet();

    double total1 = 0;

    #endregion Field Declaration

    #region Page Load

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
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if (!IsPostBack)
            {
                bindEduLevel();
                BindBatch();
                BindDegree(singleuser, group_user, collegecode, usercode);
                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                bindexamyear();
                loadmonth();
                Init_Spread();
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion Page Load

    #region Logout

    protected void logout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);
    }

    #endregion Logout

    #region Bind Header

    public void bindEduLevel()
    {
        try
        {
            ds.Clear();
            ddlEdulevel.Items.Clear();
            string qry = "select distinct Edu_Level from course where college_code='" + Convert.ToString(Session["collegecode"]) + "' order by Edu_Level desc";
            ds = d2.select_method_wo_parameter(qry, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlEdulevel.DataSource = ds;
                ddlEdulevel.DataTextField = "Edu_Level";
                ddlEdulevel.DataValueField = "Edu_Level";
                ddlEdulevel.DataBind();
                ddlEdulevel.SelectedIndex = 0;
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    public void BindBatch()
    {
        try
        {
            int count = 0;
            chklsbatch.Items.Clear();
            chkbatch.Checked = false;
            txtbatch.Text = "---Select---";
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBatch();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklsbatch.DataSource = ds;
                chklsbatch.DataTextField = "Batch_year";
                chklsbatch.DataValueField = "Batch_year";
                chklsbatch.DataBind();
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                    count += 1;
                }
                if (count > 0)
                {
                    if (chklsbatch.Items.Count == count)
                    {
                        chkbatch.Checked = true;
                        txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
                    }
                }
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            int count = 0;
            chklstdegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            //ds = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (singleuser.ToLower().Trim() == "true")
            {

                qry = "select distinct g.course_id,course_name from Degree g,course c,DeptPrivilages p where g.Course_Id = c.Course_Id  and g.Degree_Code = p.degree_code  and g.college_code = '" + collegecode + "' and Edu_Level = '" + Convert.ToString(ddlEdulevel.SelectedItem) + "' and user_code='" + usercode + "' order by course_id,course_name ";
            }
            else
            {
                qry = "select distinct g.course_id,course_name from Degree g,course c,DeptPrivilages p where g.Course_Id = c.Course_Id  and g.Degree_Code = p.degree_code  and g.college_code = '" + collegecode + "' and Edu_Level = '" + Convert.ToString(ddlEdulevel.SelectedItem) + "' and group_code='" + group_user + "' order by course_id,course_name ";
            }
            ds = d2.select_method_wo_parameter(qry, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklstdegree.DataSource = ds;
                chklstdegree.DataTextField = "course_name";
                chklstdegree.DataValueField = "course_id";
                chklstdegree.DataBind();
                chklstdegree.Items[0].Selected = true;
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                    if (chklstdegree.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstdegree.Items.Count == count)
                    {
                        chkdegree.Checked = true;
                    }
                }
                if (chkdegree.Checked == true)
                {
                    for (int i = 0; i < chklstdegree.Items.Count; i++)
                    {
                        chklstdegree.Items[i].Selected = true;
                        txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
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
                txtdegree.Enabled = true;
            }
            else
            {
                txtdegree.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }

    }

    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            int count = 0;

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
            chklstbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            if (course_id.Trim() != "")
            {
                ds = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                //ds = d2.select_method_wo_parameter(qry,"Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    chklstbranch.DataSource = ds;
                    chklstbranch.DataTextField = "dept_name";
                    chklstbranch.DataValueField = "degree_code";
                    chklstbranch.DataBind();
                    chklstbranch.Items[0].Selected = true;
                    for (int i = 0; i < chklstbranch.Items.Count; i++)
                    {
                        chklstbranch.Items[i].Selected = true;
                        if (chklstbranch.Items[i].Selected == true)
                        {
                            count += 1;
                        }
                        if (chklstbranch.Items.Count == count)
                        {
                            chkbranch.Checked = true;
                        }
                    }
                    if (chkbranch.Checked == true)
                    {
                        for (int i = 0; i < chklstbranch.Items.Count; i++)
                        {
                            chklstbranch.Items[i].Selected = true;
                            txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
                        }
                    }
                    else
                    {
                        for (int i = 0; i < chklstbranch.Items.Count; i++)
                        {
                            chkbranch.Checked = false;
                            chklstbranch.Items[i].Selected = false;
                            txtbranch.Text = "---Select---";
                        }
                    }
                }
            }
            else
            {
                txtbranch.Text = "---Select---";
                chklstbranch.Items.Clear();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    public void bindexamyear()
    {
        DataSet dsbindexamyear = new DataSet();
        SqlDataAdapter sqldap = new SqlDataAdapter();
        string batchquery = "select distinct Exam_year  from Exam_Details where Exam_year<>'0' order by  Exam_year asc ";
        dsbindexamyear = d2.select_method(batchquery, hat, "text ");
        if (dsbindexamyear.Tables[0].Rows.Count > 0)
        {
            ddlExamyr.DataSource = dsbindexamyear;
            ddlExamyr.DataTextField = "Exam_year";
            ddlExamyr.DataValueField = "Exam_year";
            ddlExamyr.DataBind();
            ddlExamyr.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        }
    }

    protected void loadmonth()
    {
        ddlExamMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        ddlExamMonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
        ddlExamMonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
        ddlExamMonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
        ddlExamMonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
        ddlExamMonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
        ddlExamMonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
        ddlExamMonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
        ddlExamMonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
        ddlExamMonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
        ddlExamMonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
        ddlExamMonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
        ddlExamMonth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));
    }

    public void Init_Spread()
    {
        try
        {
            #region FpSpread Style

            FpSpread1.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;

            #endregion FpSpread Style

            FpSpread1.Visible = false;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.RowHeader.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 5;

            #region SpreadStyles

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
            //darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Font.Bold = true;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.VerticalAlign = VerticalAlign.Middle;
            darkstyle.ForeColor = System.Drawing.Color.White;
            darkstyle.Border.BorderSize = 0;
            darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;

            FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
            //sheetstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
            //darkstyle.ForeColor = System.Drawing.Color.Black;
            sheetstyle.Font.Name = "Book Antiqua";
            sheetstyle.Font.Size = FontUnit.Medium;
            sheetstyle.Font.Bold = true;
            sheetstyle.HorizontalAlign = HorizontalAlign.Center;
            sheetstyle.VerticalAlign = VerticalAlign.Middle;
            sheetstyle.ForeColor = System.Drawing.Color.Black;
            sheetstyle.Border.BorderSize = 1;
            sheetstyle.Border.BorderColor = System.Drawing.Color.Black;

            #endregion SpreadStyles

            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Sheets[0].DefaultStyle = sheetstyle;
            //FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].Columns[0].Width = 58;
            FpSpread1.Sheets[0].Columns[1].Width = 110;
            FpSpread1.Sheets[0].Columns[2].Width = 130;
            FpSpread1.Sheets[0].Columns[3].Width = 200;
            FpSpread1.Sheets[0].Columns[4].Width = 120;
            FpSpread1.Sheets[0].Columns[0].Locked = true;
            FpSpread1.Sheets[0].Columns[1].Locked = true;
            FpSpread1.Sheets[0].Columns[2].Locked = true;
            FpSpread1.Sheets[0].Columns[3].Locked = true;
            FpSpread1.Sheets[0].Columns[4].Locked = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch Year";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Branch Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Appeared";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Locked = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Locked = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Locked = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Locked = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Locked = true;
            FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].FrozenColumnCount = 5;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    #endregion Bind Header

    #region Comments

    //public void ugconsolidatedGrade_Mark(string roll_no)
    //{
    //    try
    //    {
    //        DataSet printds = new DataSet();
    //        string lblerror1 =string.Empty;
    //        DataSet printds_new = new DataSet();
    //        DataSet printds_rows = new DataSet();
    //        //578
    //        string max_sem =string.Empty;
    //        int maxSem = 0;
    //        Boolean printpage = false;
    //        string edu_level =string.Empty;
    //        string degree =string.Empty;
    //        string monthandyear =string.Empty;
    //        string studname =string.Empty;
    //        string dob =string.Empty;
    //        string rollnosub =string.Empty;
    //        string regnumber =string.Empty;
    //        string batch_year =string.Empty;
    //        string degree_code =string.Empty;
    //        string exam_code =string.Empty;
    //        string sem =string.Empty;

    //        string branch =string.Empty;
    //        int month = 0;
    //        string monthstr =string.Empty;
    //        string sql2 =string.Empty;
    //        string sql3 =string.Empty;
    //        string roman =string.Empty;
    //        string semroman =string.Empty;
    //        string grade =string.Empty;
    //        string gradepoints =string.Empty;
    //        string coe =string.Empty;
    //        string subjectcode_Part1 =string.Empty;
    //        string subjectcode_Part2 =string.Empty;
    //        string subjectcode_Part3 =string.Empty;
    //        string subjectcode_Part4 =string.Empty;
    //        string cal_gpa =string.Empty;
    //        string current_semester =string.Empty;
    //        string app_no =string.Empty;
    //        string subtype =string.Empty;
    //        string admid_date =string.Empty;
    //        string additionalmsg =string.Empty;
    //        bool onlyca = false;
    //        bool onlyes = false;
    //        DataSet gradeds = new DataSet();
    //        DataSet dsSpl = new DataSet();
    //        DataTable dtStar = new DataTable();
    //        double ugpgminpass = 0;

    //        Font fontdegreename = new Font("Times New Roman", 14, FontStyle.Bold);
    //        Font fontStudDetails = new Font("Times New Roman", 11, FontStyle.Bold);
    //        Font fontStmtMarks = new Font("Times New Roman", 11, FontStyle.Regular);
    //        Font fontcgpa = new Font("Times New Roman", 11, FontStyle.Regular);
    //        Font fontDate = new Font("Times New Roman", 11, FontStyle.Regular);


    //        Font f1_cos10bold = new Font("Times New Roman", 10, FontStyle.Bold);
    //        Font f2_cos9bold = new Font("Comic Sans MS", 9, FontStyle.Bold);
    //        Font f3_arial10bold = new Font("Arial", 10, FontStyle.Bold);
    //        Font Fontarial7r = new Font("Arial", 6, FontStyle.Bold);
    //        Font f4_arial7reg = new Font("Arial", 7, FontStyle.Regular);
    //        Font f5_pal10bold = new Font("Palatino Linotype", 10, FontStyle.Bold);

    //        //Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(25.5, 35.6));
    //        //Gios.Pdf.PdfPage mypdfpage;
    //        //FpSpread2.SaveChanges();
    //        for (int res = 1; res < Convert.ToInt32(FpSpread2.Sheets[0].RowCount); res++)
    //        {
    //            int isval = 0;
    //            int additonalstatus = 0;
    //            onlyca = false;
    //            onlyes = false;
    //            isval = 1;
    //            additionalmsg =string.Empty;
    //            int splcredit = 0;
    //            if (isval == 1)
    //            {
    //                ugpgminpass = 0;
    //                printds.Clear();
    //                printds.Dispose();
    //                printds_new.Clear();
    //                printds_new.Dispose();
    //                string grade_set =string.Empty;
    //                rollnosub = roll_no;
    //                //regnumber = FpSpread2.Sheets[0].Cells[res, 2].Text;
    //                //CONVERT(VARCHAR(11),GETDATE(),106)
    //                int setng_ovrtotalcreadits = 0;
    //                int setng_mintotalcreadits = 0;
    //                int totalcreitdsened = 0;
    //                int noofsubapplied = 0;
    //                int noofsubpassed = 0;
    //                int noofsubfailed = 0;
    //                int Totalfailcount = 0;
    //                int tot_credits = 0;
    //                int Tot_credit_settings = 0;
    //                string sql = "SELECT Reg_No,r.batch_year,r.app_no,r.degree_code,R.current_semester,R.Stud_Name,Course_Name as degree,Dept_name,CONVERT(VARCHAR, dob, 110) as dob,c.edu_level,CONVERT(VARCHAR(11),R.Adm_Date,106) as ADM_DATE FROM Registration R,Applyn A,Degree G,Course C,Department D WHERE R.App_No = A.App_No AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND D.College_Code = G.College_Code AND Roll_No = '" + rollnosub + "' and r.college_code='" + Session["collegecode"].ToString() + "' ;  select coe,principal from collinfo where college_code='" + Session["collegecode"].ToString() + "';select * from exam_details";
    //                sql = sql + "  select count(s.subject_no) as total from subjectchooser sc,subject s,registration r where sc.roll_no=r.roll_no and s.subject_no=sc.subject_no and r.roll_no='" + rollnosub + "'";
    //                sql = sql + "   Select count(subject.subject_no) as total from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'  and roll_no='" + rollnosub + "'; ";
    //                sql = sql + "   Select count(subject.subject_no) as total from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='fail'  and roll_no='" + rollnosub + "'; select distinct m.subject_no from mark_entry m,subjectChooser sc where m.roll_no=sc.roll_no and m.subject_no=sc.subject_no and m.roll_no='" + rollnosub + "' and m.result<>'Pass' and m.subject_no not in(select distinct m.subject_no from mark_entry m,subjectChooser sc where m.roll_no=sc.roll_no and m.subject_no=sc.subject_no and m.roll_no='" + rollnosub + "' and m.result='Pass');  select distinct COUNT(teq.Equal_Subject_Code),teq.Com_Subject_Code from  subjectchooser sc,subject s,registration r , tbl_equal_paper_Matching teq   where sc.roll_no=r.roll_no and s.subject_no=sc.subject_no   and r.roll_no='" + rollnosub + "'  and teq.Equal_Subject_Code=s.subject_code group by teq.Com_Subject_Code having COUNT(teq.Equal_Subject_Code)>1; Select sum(credit_points) from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'  and roll_no='" + rollnosub + "'";
    //                sql = sql + "     Select distinct  subject.subject_no, subject_name, subject_code from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='fail'  and roll_no='" + rollnosub + "'  and SUBSTRING(subject_code,7,1)!='M'    and  subject.subject_no not in (   Select subject.subject_no from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'   and roll_no='" + rollnosub + "')";
    //                sql = sql + "     Select distinct  subject.subject_no, subject_name, subject_code from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='fail'  and roll_no='" + rollnosub + "'    and SUBSTRING(subject_code,7,1)='M'    and  subject.subject_no not in (   Select subject.subject_no from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'   and roll_no='" + rollnosub + "')";
    //                sql = sql + "  SELECT STUFF((SELECT distinct ''',''' + convert(nvarchar(max),[subject_code])  FROM subject sy   where  subject_name='Computer training'   FOR XML PATH('')),1,1,'''') as [Roll_No]";
    //                printds = d2.select_method_wo_parameter(sql, "Text");
    //                if (printds.Tables.Count > 0)
    //                {
    //                    noofsubapplied = Convert.ToInt32(printds.Tables[3].Rows[0][0].ToString());
    //                    noofsubapplied = noofsubapplied - Convert.ToInt32(printds.Tables[7].Rows.Count);
    //                    noofsubpassed = Convert.ToInt32(printds.Tables[4].Rows[0][0].ToString());

    //                    noofsubfailed = Convert.ToInt32(printds.Tables[5].Rows[0][0].ToString());

    //                    Totalfailcount = printds.Tables[6].Rows.Count;
    //                    int.TryParse(Convert.ToString(printds.Tables[8].Rows[0][0]), out tot_credits);
    //                }

    //                if (printds.Tables[0].Rows.Count > 0)
    //                {
    //                    batch_year = printds.Tables[0].Rows[0]["batch_year"].ToString();
    //                    degree_code = printds.Tables[0].Rows[0]["degree_code"].ToString();
    //                    edu_level = printds.Tables[0].Rows[0]["edu_level"].ToString();
    //                    app_no = Convert.ToString(printds.Tables[0].Rows[0]["app_no"]);

    //                    degree = printds.Tables[0].Rows[0]["degree"].ToString();
    //                    coe = printds.Tables[1].Rows[0]["coe"].ToString();
    //                    admid_date = Convert.ToString(printds.Tables[0].Rows[0]["ADM_DATE"]);

    //                    setng_ovrtotalcreadits = Convert.ToInt32(d2.GetFunctionv("select totalcredits from coe_ovrl_credits_Dts where degree_code='" + degree_code + "'"));

    //                    setng_mintotalcreadits = Convert.ToInt32(d2.GetFunctionv("select minimcredits from coe_ovrl_credits_Dts where degree_code='" + degree_code + "'"));

    //                    max_sem = d2.GetFunctionv("select NDurations from ndegree where batch_year='" + batch_year + "'  and Degree_code='" + degree_code + "' and college_code='" + Convert.ToString(Session["collegecode"]) + "'");
    //                    if (max_sem == "" || max_sem == null)
    //                    {
    //                        max_sem = d2.GetFunctionv("SELECT Duration FROM Degree where  Degree_Code='" + degree_code + "' and college_code='" + Convert.ToString(Session["collegecode"]) + "'");
    //                    }
    //                    int.TryParse(max_sem, out maxSem);
    //                    int newbatch = 0;
    //                    int.TryParse(batch_year, out newbatch);
    //                    DateTime dt = new DateTime();
    //                    int cur_year = DateTime.Now.Year;
    //                    int diff = newbatch + (maxSem / 2);
    //                    //if (cur_year >= diff)
    //                    //{
    //                    //}
    //                    //else
    //                    //{
    //                    //    lbl_popuperr.Text = "The Consolidated is Generated Only For Passed Out Students.";
    //                    //    errdiv.Visible = true;
    //                    //    return;
    //                    //}
    //                }


    //                if (edu_level.Trim().ToLower() == "ug" && noofsubpassed != noofsubapplied)
    //                {
    //                    string comcode =string.Empty;

    //                    DataSet dspassorfail = new DataSet();
    //                    DataView dvcomptraing = new DataView();
    //                    DataView dvcomsubject = new DataView();
    //                    int comsubjectcount = 0;
    //                    DataSet dssequalpaers = new DataSet();
    //                    ArrayList comsubjects = new ArrayList();
    //                    for (int isub = 0; isub < printds.Tables[9].Rows.Count; isub++)
    //                    {
    //                        string commsubjectpaper1 = d2.GetFunctionv("select  Com_Subject_Code from tbl_equal_paper_Matching where Equal_Subject_Code='" + printds.Tables[9].Rows[isub][2].ToString() + "' ");
    //                        sql = "  select * from tbl_equal_paper_Matching where  Com_Subject_Code  in ('" + commsubjectpaper1 + "') ";
    //                        dssequalpaers.Clear();
    //                        dssequalpaers = d2.select_method_wo_parameter(sql, "Text");
    //                        for (int eqlpap = 0; eqlpap < dssequalpaers.Tables[0].Rows.Count; eqlpap++)
    //                        {
    //                            string syllcode = d2.GetFunctionv("select syll_code from subject where subject_no='" + printds.Tables[9].Rows[isub][0].ToString() + "'");
    //                            string equlpapersubjectno = d2.GetFunctionv("select subject_no from subject where syll_code='" + syllcode + "' and  subject_code='" + dssequalpaers.Tables[0].Rows[eqlpap]["Equal_Subject_Code"].ToString() + "'  ");
    //                            if (equlpapersubjectno.Trim() != "" && equlpapersubjectno.Trim() != "0")
    //                            {

    //                                dspassorfail.Clear();
    //                                dspassorfail = d2.select_method_wo_parameter(" select * from mark_entry where subject_no='" + equlpapersubjectno + "' and  result='pass' and roll_no='" + rollnosub + "'  ", "Text");
    //                                if (dspassorfail.Tables[0].Rows.Count > 0)
    //                                {
    //                                    if (!comsubjects.Contains(commsubjectpaper1))
    //                                    {
    //                                        comsubjectcount++;
    //                                        comsubjects.Add(commsubjectpaper1);
    //                                    }
    //                                }

    //                            }
    //                        }

    //                    }

    //                    string computersubjectcode = printds.Tables[11].Rows[0][0].ToString();
    //                    if (computersubjectcode != "")
    //                    {
    //                        computersubjectcode = computersubjectcode.Remove(0, 2);
    //                        computersubjectcode = computersubjectcode + "'";


    //                    }
    //                    printds.Tables[10].DefaultView.RowFilter = "subject_code in (" + computersubjectcode + ")";
    //                    dvcomptraing = printds.Tables[10].DefaultView;

    //                    int majorpaperscount = printds.Tables[10].Rows.Count;
    //                    int comcodecount = 0;

    //                    if (dvcomptraing.Count > 0)
    //                    {
    //                        majorpaperscount = printds.Tables[10].Rows.Count - dvcomptraing.Count;
    //                        comcodecount = comcodecount + 1;
    //                    }


    //                    comcodecount = Convert.ToInt32(printds.Tables[9].Rows.Count) - comsubjectcount;

    //                    int subjectmissed = noofsubapplied - noofsubpassed;
    //                    if (printds.Tables[10].Rows.Count <= 2 && subjectmissed <= 2 && comcodecount == 0 && setng_ovrtotalcreadits != setng_mintotalcreadits)
    //                    {
    //                        noofsubpassed = noofsubapplied;
    //                    }
    //                }

    //                //if (printds.Tables[0].Rows.Count > 0 && noofsubpassed == noofsubapplied)
    //                if (printds.Tables[0].Rows.Count > 0 && noofsubpassed == noofsubapplied) //Totalfailcount == 0)
    //                {
    //                    printpage = true;
    //                    string principal =string.Empty;
    //                    edu_level = printds.Tables[0].Rows[0]["edu_level"].ToString();
    //                    app_no = Convert.ToString(printds.Tables[0].Rows[0]["app_no"]);
    //                    degree = printds.Tables[0].Rows[0]["degree"].ToString();
    //                    coe = printds.Tables[1].Rows[0]["coe"].ToString();
    //                    admid_date = Convert.ToString(printds.Tables[0].Rows[0]["ADM_DATE"]);

    //                    string[] adm_dt = admid_date.Split(' ');

    //                    if (adm_dt.Length > 0)
    //                    {
    //                        if (adm_dt.Length == 3)
    //                            admid_date = adm_dt[2] + " - JUN";
    //                    }

    //                    // month = ddlMonth.SelectedIndex;
    //                    //monthstr = ddlMonth.SelectedIndex.ToString();
    //                    string strMonthName =string.Empty;
    //                    //monthandyear = strMonthName + "  " + ddlYear.SelectedItem.Text;
    //                    //monthandyear = monthandyear.ToUpper();
    //                    studname = printds.Tables[0].Rows[0]["Stud_Name"].ToString();
    //                    branch = printds.Tables[0].Rows[0]["Dept_name"].ToString();
    //                    dob = printds.Tables[0].Rows[0]["dob"].ToString();

    //                    string stdappno = printds.Tables[0].Rows[0]["App_No"].ToString();
    //                    batch_year = printds.Tables[0].Rows[0]["batch_year"].ToString();
    //                    degree_code = printds.Tables[0].Rows[0]["degree_code"].ToString();

    //                    if (sem == "1")
    //                    {
    //                        semroman = "I";
    //                    }
    //                    else if (sem == "2")
    //                    {
    //                        semroman = "II";
    //                    }
    //                    else if (sem == "3")
    //                    {
    //                        semroman = "III";
    //                    }
    //                    else if (sem == "4")
    //                    {
    //                        semroman = "IV";
    //                    }
    //                    else if (sem == "5")
    //                    {
    //                        semroman = "V";
    //                    }
    //                    else if (sem == "6")
    //                    {
    //                        semroman = "VI";
    //                    }
    //                    else if (sem == "7")
    //                    {
    //                        semroman = "VII";
    //                    }
    //                    else if (sem == "8")
    //                    {
    //                        semroman = "VIII";
    //                    }


    //                    // sql3 = "Select syllabus_master.semester,Subject_Type,subject_name,subject_code,subject.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,maxtotal,grade,cp,credit_points,mark_entry.subject_no,semester,maxtotal,exam_code,subject.Part_Type,sub_sem.priority,sub_sem.lab,subject.subjectpriority,SUBSTRING(subject_code,(LEN(subject_code)-2) ,1) as Prac_code from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'  and roll_no='" + rollnosub + "' order by syllabus_master.semester,isnull(subject.Part_Type,'3') asc,case when (SUBSTRING(subject_code,(LEN(subject_code)-2) ,1)='F' and lab=0) then null when (SUBSTRING(subject_code,(LEN(subject_code)-2) ,1)='M' and lab=0) then 'A' when (SUBSTRING(subject_code,(LEN(subject_code)-2) ,1)='M' and lab=1) then 'B' When (SUBSTRING(subject_code,(LEN(subject_code)-2) ,1)='A' and lab=0) then 'C' When (SUBSTRING(subject_code,(LEN(subject_code)-2) ,1)='A' and lab=1) then 'D' When (SUBSTRING(subject_code,(LEN(subject_code)-2) ,0)='E' and lab=1) then 'E' When (SUBSTRING(subject_code,(LEN(subject_code)-2) ,1)='E' and lab=1) then 'F' Else SUBSTRING(subject_code,(LEN(subject_code)-2) ,1) End asc,subject.subjectpriority,sub_sem.lab,subject_type desc,subject_code asc"; hide by sridhar
    //                    sql3 = "Select syllabus_master.semester,Subject_Type,subject_name,subject_code,subject.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,maxtotal,grade,cp,credit_points,mark_entry.subject_no,semester,maxtotal,exam_code,subject.Part_Type,sub_sem.priority,sub_sem.lab,subject.subjectpriority,SUBSTRING(subject_code,(LEN(subject_code)-2) ,1) as Prac_code from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'  and roll_no='" + rollnosub + "' order by syllabus_master.semester, subject.subjectpriority ";
    //                    //sub_sem.priority,subject.Part_Type,subject_type,sub_sem.lab,subject.subjectpriority,subject.subject_no";
    //                    printds_rows.Clear();
    //                    printds_rows.Dispose();
    //                    printds_rows = d2.select_method_wo_parameter(sql3, "Text");

    //                    string batch_year1 = printds.Tables[0].Rows[0]["batch_year"].ToString() + "-";
    //                    if (edu_level.Trim().ToLower() == "ug")
    //                    {
    //                        batch_year1 = batch_year1 + Convert.ToString((Convert.ToInt32(batch_year) + 3));
    //                        grade_set = "0";
    //                        ugpgminpass = 50;
    //                    }
    //                    else
    //                    {
    //                        batch_year1 = batch_year1 + Convert.ToString((Convert.ToInt32(batch_year) + 2));
    //                        grade_set = "1";
    //                        ugpgminpass = 50;
    //                    }



    //                    double overallcreditearned = 0;

    //                    if (printds_rows.Tables[0].Rows.Count > 0)
    //                    {
    //                        int rowSpecial = 0;
    //                        if (true)
    //                        {
    //                            //string nwqry = "select top 1 from SpecialCourseSubject where App_no='" + stdappno + "' and CurrentSem='"+curr_sem+"' and IsFinalsem='1'";
    //                            string nwqry = "select s.subject_name,scs.Subject_No,App_no,MarkType,IsFinalsem,CurrentSem,s.credit_points,ss.subject_type,s.acronym from SpecialCourseSubject scs,subject s,sub_sem ss where s.subject_no=scs.Subject_No and ss.subType_no=s.subType_no and App_no='" + stdappno + "' and CurrentSem='" + maxSem + "' and IsFinalsem='1'";
    //                            dsSpl = d2.select_method_wo_parameter(nwqry, "Text");
    //                            rowSpecial = dsSpl.Tables[0].Rows.Count;
    //                        }

    //                        bool starP3 = false;
    //                        string[] star = new string[2];
    //                        ArrayList arr_star = new ArrayList();
    //                        double[] starmrk = new double[2];
    //                        double[] starcredit = new double[2];
    //                        double[] stargpa = new double[2];
    //                        double[] starwpm = new double[2];


    //                        tot_credits = tot_credits + 1;
    //                        int creditsdiff = 0;
    //                        if (tot_credits > setng_mintotalcreadits)
    //                        {
    //                            creditsdiff = tot_credits - setng_mintotalcreadits;
    //                            creditsdiff = creditsdiff / 5;
    //                        }
    //                        string removesubjetcs =string.Empty;
    //                        DataSet cutsubject = new DataSet();
    //                        if (creditsdiff > 0)
    //                        {

    //                            sql = "Select  top " + creditsdiff + " subject.subject_code,credit_points from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'  and roll_no='" + rollnosub + "' and Part_Type=3 and SUBSTRING(subject_code,7,1)='M' order by total asc,credit_points asc";

    //                            cutsubject.Clear();
    //                            cutsubject = d2.select_method_wo_parameter(sql, "Text");
    //                            int removecredites = 0;
    //                            for (int ii = 0; ii < cutsubject.Tables[0].Rows.Count; ii++)
    //                            {
    //                                if (removecredites == 0)
    //                                {
    //                                    removecredites = Convert.ToInt32(cutsubject.Tables[0].Rows[ii][1].ToString());
    //                                }
    //                                else
    //                                {
    //                                    removecredites = removecredites + Convert.ToInt32(cutsubject.Tables[0].Rows[ii][1].ToString());
    //                                }
    //                            }
    //                            if (removecredites <= 10)
    //                            {

    //                                for (int ii = 0; ii < cutsubject.Tables[0].Rows.Count; ii++)
    //                                {
    //                                    if (removesubjetcs.Trim() == "")
    //                                    {
    //                                        removesubjetcs = cutsubject.Tables[0].Rows[ii][0].ToString();
    //                                    }
    //                                    else
    //                                    {
    //                                        removesubjetcs = removesubjetcs + "," + cutsubject.Tables[0].Rows[ii][0].ToString();
    //                                    }
    //                                    arr_star.Add(Convert.ToString(cutsubject.Tables[0].Rows[ii][0]));
    //                                }
    //                            }
    //                            else
    //                            {
    //                                removesubjetcs = cutsubject.Tables[0].Rows[0][0].ToString();
    //                                arr_star.Add(Convert.ToString(cutsubject.Tables[0].Rows[0][0]));
    //                            }
    //                        }
    //                        int mr = 0;
    //                        int semescount = 0;
    //                        for (int s = 1; s <= maxSem; s++)
    //                        {
    //                            semescount++;
    //                            DataView dvMark = new DataView();
    //                            printds_rows.Tables[0].DefaultView.RowFilter = "semester='" + s + "'";
    //                            dvMark = printds_rows.Tables[0].DefaultView;
    //                            if (dvMark.Count > 0)
    //                            {
    //                                for (int i = 0; i < dvMark.Count; i++)
    //                                {
    //                                    if (mr != 0 && mr % 46 == 0)
    //                                    {
    //                                        if (mr % 46 == 0)
    //                                        {
    //                                            fullmark.Cell(mr, 1).SetCellPadding(5);
    //                                            fullmark.Cell(mr, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                            fullmark.Cell(mr, 1).SetContent("--- STATEMENT CONTINUED ---");

    //                                            newpdftabpage2 = fullmark.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 0, 175, 730, 650));

    //                                            mypdfpage.Add(newpdftabpage2);

    //                                            pdfdoi = new PdfTextArea(fontDate, System.Drawing.Color.Black, new PdfArea(mydoc, 7, 990, 71, 25), System.Drawing.ContentAlignment.TopLeft, txtdoi.Text.ToString());
    //                                            mypdfpage.Add(pdfdoi);

    //                                            mypdfpage.SaveToDocument();

    //                                            mypdfpage = mydoc.NewPage();
    //                                            mr = 0;


    //                                            pdfdoi = new PdfTextArea(fontdegreename, System.Drawing.Color.Black, new PdfArea(mydoc, 227, 45, 100, 25), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(degree));
    //                                            mypdfpage.Add(pdfdoi);
    //                                            if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg")))
    //                                            {
    //                                                LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg"));
    //                                                mypdfpage.Add(LogoImage2, 645, 25, 420);
    //                                            }
    //                                            else
    //                                            {

    //                                            }

    //                                            tblstudDetail = mydoc.NewTable(fontStudDetails, 1, 5, 5);
    //                                            tblstudDetail.VisibleHeaders = false;
    //                                            tblstudDetail.SetBorders(Color.Black, 1, BorderType.None);
    //                                            //255,100,106,156,88
    //                                            tblstudDetail.SetColumnsWidth(new int[] { 255, 100, 108, 157, 90 });
    //                                            tblstudDetail.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                            tblstudDetail.Cell(0, 0).SetContent(studname);
    //                                            tblstudDetail.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            tblstudDetail.Cell(0, 1).SetContent(dob);
    //                                            tblstudDetail.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            tblstudDetail.Cell(0, 2).SetContent(regnumber);
    //                                            tblstudDetail.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                            tblstudDetail.Cell(0, 3).SetContent(branch);
    //                                            tblstudDetail.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            tblstudDetail.Cell(0, 4).SetContent(admid_date.ToUpper());

    //                                            // x=13,y=121.8,W=706 ,H=41
    //                                            newpdftabpage2 = tblstudDetail.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 0, 115, 730, 50));//mydoc, 1, 115, 730, 50
    //                                            mypdfpage.Add(newpdftabpage2);

    //                                            fullmark = mydoc.NewTable(fontStmtMarks, printds_rows.Tables[0].Rows.Count + maxSem + rowSpecial + 2, 10, 1);
    //                                            fullmark.VisibleHeaders = false;
    //                                            fullmark.SetBorders(Color.Black, 1, BorderType.None);
    //                                            fullmark.Columns[0].SetWidth(71);//
    //                                            fullmark.Columns[1].SetWidth(250);
    //                                            fullmark.Columns[2].SetWidth(43);
    //                                            fullmark.Columns[3].SetWidth(43);
    //                                            fullmark.Columns[4].SetWidth(43);
    //                                            fullmark.Columns[5].SetWidth(51);
    //                                            fullmark.Columns[6].SetWidth(51);
    //                                            fullmark.Columns[7].SetWidth(43);
    //                                            fullmark.Columns[8].SetWidth(43);
    //                                            fullmark.Columns[9].SetWidth(71);

    //                                            fullmark.Cell(mr, 1).SetCellPadding(5);
    //                                            fullmark.Cell(mr, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                            fullmark.Cell(mr, 1).SetContent("--- CONTINUATION OF STATEMENT ---");
    //                                            mr += 2;
    //                                            //fullmark.Cell(mr, 1).SetCellPadding(5);
    //                                        }
    //                                        bool calgpa = true;
    //                                        fullmark.Cell(mr, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                        fullmark.Cell(mr, 0).SetContent(Convert.ToString(dvMark[i]["subject_code"]));

    //                                        fullmark.Cell(mr, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                        fullmark.Cell(mr, 1).SetContent(Convert.ToString(dvMark[i]["subject_name"]));

    //                                        string subtypeaccr =string.Empty;
    //                                        string sub_code = Convert.ToString(dvMark[i]["subject_code"]).Trim();
    //                                        subtypeaccr = Convert.ToString(dvMark[i]["subject_type"]);
    //                                        subtypeaccr = findSubTypeAccromy(subtypeaccr);
    //                                        if (subtypeaccr.ToUpper().Trim() == "PRAC")
    //                                        {
    //                                            if (Convert.ToString(dvMark[i]["subject_name"]).ToLower() == "computer training" || Convert.ToString(dvMark[i]["subject_name"]).ToLower() == "skillbased computer training intro.to information techn. & ms office")
    //                                            {
    //                                                subtypeaccr = "CT";
    //                                            }
    //                                            if (subtypeaccr.ToUpper().Trim() == "PRAC")
    //                                            {
    //                                                string gt_sub_code = sub_code.Substring(sub_code.Length - 3, 1);
    //                                                if (gt_sub_code.ToUpper().Trim() == "M")
    //                                                {
    //                                                    subtypeaccr = "MC";
    //                                                }
    //                                                else if (gt_sub_code.ToUpper().Trim() == "A")
    //                                                {
    //                                                    subtypeaccr = "AC";
    //                                                }
    //                                                else if (gt_sub_code.ToUpper().Trim() == "E")
    //                                                {
    //                                                    subtypeaccr = "EC";
    //                                                }
    //                                                else if (gt_sub_code.ToUpper().Trim() == "L")
    //                                                {
    //                                                    subtypeaccr = "GE";
    //                                                }
    //                                                else
    //                                                {
    //                                                    subtypeaccr = "PRAC";
    //                                                }
    //                                            }
    //                                        }
    //                                        fullmark.Cell(mr, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                        fullmark.Cell(mr, 2).SetContent(subtypeaccr);
    //                                        double internalmmark = 0;
    //                                        double externalmark1 = 0;
    //                                        double totalintext = 0;
    //                                        double.TryParse(Convert.ToString(dvMark[i]["internal_mark"]), out internalmmark);
    //                                        fullmark.Cell(mr, 4).SetContentAlignment(ContentAlignment.MiddleCenter);

    //                                        string maxinternal = Convert.ToString(dvMark[i]["max_int_marks"]).Trim();
    //                                        string maxexternal = Convert.ToString(dvMark[i]["max_ext_marks"]).Trim();

    //                                        double extfinal = 0;
    //                                        double.TryParse(Convert.ToString(dvMark[i]["external_mark"]), out extfinal);
    //                                        extfinal = Math.Round(extfinal, 0);
    //                                        externalmark1 = extfinal;
    //                                        string checkedmark =string.Empty;
    //                                        if (extfinal < 0)
    //                                        {
    //                                            checkedmark = loadmarkat(Convert.ToString(extfinal));
    //                                        }
    //                                        else
    //                                        {
    //                                            checkedmark = Convert.ToString(extfinal);
    //                                        }

    //                                        if (internalmmark >= 0)
    //                                        {
    //                                            if (extfinal > 0)
    //                                            {
    //                                                totalintext = internalmmark + externalmark1;
    //                                            }
    //                                            else
    //                                            {
    //                                                totalintext = internalmmark;
    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            if (extfinal > 0)
    //                                            {
    //                                                totalintext = externalmark1;
    //                                            }
    //                                            else
    //                                            {
    //                                                totalintext = 0;
    //                                            }
    //                                        }
    //                                        onlyca = false;
    //                                        onlyes = false;

    //                                        if (maxinternal == "0" && maxexternal == "100")
    //                                        {
    //                                            onlyes = true;
    //                                        }
    //                                        if (maxinternal == "100" && maxexternal == "0")
    //                                        {
    //                                            onlyca = true;
    //                                        }
    //                                        fullmark.Cell(mr, 5).SetContentAlignment(ContentAlignment.MiddleCenter);

    //                                        if (onlyes == true && extfinal >= 0)
    //                                        {
    //                                            fullmark.Cell(mr, 5).SetContent(checkedmark + "/100");
    //                                        }
    //                                        else
    //                                        {
    //                                            fullmark.Cell(mr, 5).SetContent(checkedmark);
    //                                        }

    //                                        if (onlyca == true && internalmmark >= 0)
    //                                        {
    //                                            fullmark.Cell(mr, 4).SetContent(internalmmark + "/100");
    //                                        }
    //                                        else if (onlyes == false)
    //                                        {
    //                                            fullmark.Cell(mr, 4).SetContent(internalmmark);
    //                                        }
    //                                        else
    //                                        {
    //                                            fullmark.Cell(mr, 4).SetContent("NA");
    //                                            additonalstatus = 1;
    //                                        }

    //                                        if (onlyca)
    //                                        {
    //                                            fullmark.Cell(mr, 5).SetContent("NA");
    //                                            additonalstatus = 1;
    //                                        }

    //                                        //fullmark.Cell(mr, 4).SetContent(internalmmark);
    //                                        double totfinal = 0;
    //                                        totfinal = Math.Round(totalintext, 0, MidpointRounding.AwayFromZero);

    //                                        fullmark.Cell(mr, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                        fullmark.Cell(mr, 6).SetContent(Convert.ToString(totfinal));

    //                                        double credit = 0;
    //                                        double.TryParse(Convert.ToString(dvMark[i]["credit_points"]), out credit);
    //                                        overallcreditearned += credit;
    //                                        fullmark.Cell(mr, 3).SetContentAlignment(ContentAlignment.MiddleCenter);

    //                                        if (starP3 == true)
    //                                        {
    //                                            if (arr_star.Contains(sub_code))
    //                                            {
    //                                                fullmark.Cell(mr, 3).SetContent(Convert.ToString(dvMark[i]["credit_points"] + "*"));
    //                                            }
    //                                            else
    //                                            {
    //                                                fullmark.Cell(mr, 3).SetContent(Convert.ToString(dvMark[i]["credit_points"]));
    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            fullmark.Cell(mr, 3).SetContent(Convert.ToString(dvMark[i]["credit_points"]));
    //                                        }

    //                                        double checkmarkmm = 0;
    //                                        double.TryParse(Convert.ToString(dvMark[i]["total"]), out checkmarkmm);
    //                                        checkmarkmm = Math.Round(checkmarkmm, 0, MidpointRounding.AwayFromZero);
    //                                        string gradesql = "    select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='" + sem + "' and Degree_Code='" + grade_set + "' and batch_year='" + grade_set + "' and '" + checkmarkmm + "' between Frange and Trange";
    //                                        gradeds.Clear();
    //                                        gradeds = d2.select_method_wo_parameter(gradesql, "Text");
    //                                        if (gradeds.Tables[0].Rows.Count == 0)
    //                                        {
    //                                            gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and Degree_Code='" + grade_set + "' and batch_year='" + grade_set + "'  and '" + checkmarkmm + "' between Frange and Trange";//added by sridhar 
    //                                            gradeds.Clear();
    //                                            gradeds = d2.select_method_wo_parameter(gradesql, "Text");
    //                                        }
    //                                        for (int grd = 0; grd < gradeds.Tables[0].Rows.Count; grd++)
    //                                        {
    //                                            if (Convert.ToInt32(gradeds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(gradeds.Tables[0].Rows[grd][2]) >= checkmarkmm)
    //                                            {
    //                                                grade = gradeds.Tables[0].Rows[grd][0].ToString();
    //                                                gradepoints = gradeds.Tables[0].Rows[grd]["Credit_Points"].ToString();
    //                                            }
    //                                        }
    //                                        double gradeibtpoint = 0;
    //                                        double.TryParse(Convert.ToString(dvMark[i]["total"].ToString()), out gradeibtpoint);
    //                                        gradeibtpoint = gradeibtpoint / 10;
    //                                        gradeibtpoint = Math.Round(gradeibtpoint, 1, MidpointRounding.AwayFromZero);
    //                                        string gp = String.Format("{0:0.0}", gradeibtpoint);
    //                                        gradepoints = Convert.ToString(gradeibtpoint);


    //                                        gradepoints = Convert.ToString(Math.Round(Convert.ToDouble(gradeibtpoint), 1, MidpointRounding.AwayFromZero));

    //                                        gradepoints = String.Format("{0:0.0}", gradepoints);

    //                                        string result = Convert.ToString(dvMark[i]["result"]).ToLower();
    //                                        if (result == "fail")
    //                                        {
    //                                            result = "RA";
    //                                            grade = "U";
    //                                        }
    //                                        else if (result == "pass")
    //                                        {
    //                                            result = "P";
    //                                        }
    //                                        else
    //                                        {
    //                                            result = "AB";
    //                                            grade = "U";
    //                                        }
    //                                        totfinal = totfinal * Convert.ToDouble(dvMark[i]["credit_points"].ToString());
    //                                        fullmark.Cell(mr, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                        fullmark.Cell(mr, 8).SetContent(grade);
    //                                        fullmark.Cell(mr, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                        fullmark.Cell(mr, 7).SetContent(gp);
    //                                        totfinal = Math.Round(totfinal, 0);

    //                                        //fullmark.Cell(i, 9).SetContentAlignment(ContentAlignment.MiddleCenter);

    //                                        fullmark.Cell(mr, 9).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                        DataView dv = new DataView();
    //                                        printds.Tables[2].DefaultView.RowFilter = "exam_code='" + dvMark[i]["exam_code"].ToString() + "'";
    //                                        dv = printds.Tables[2].DefaultView;
    //                                        if (dv.Count > 0)
    //                                        {
    //                                            string exam_y = dv[0]["Exam_year"].ToString();
    //                                            string exam_m = dv[0]["Exam_Month"].ToString();

    //                                            strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(exam_m));
    //                                            strMonthName = strMonthName[0].ToString() + strMonthName[1].ToString() + strMonthName[2].ToString();
    //                                            strMonthName = exam_y + " - " + strMonthName.ToUpper() + " ";
    //                                            fullmark.Cell(mr, 9).SetContent(strMonthName);
    //                                        }
    //                                        mr++;
    //                                    }
    //                                    else
    //                                    {
    //                                        bool calgpa = true;
    //                                        fullmark.Cell(mr, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                        fullmark.Cell(mr, 0).SetContent(Convert.ToString(dvMark[i]["subject_code"]));

    //                                        fullmark.Cell(mr, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                        fullmark.Cell(mr, 1).SetContent(Convert.ToString(dvMark[i]["subject_name"]));

    //                                        string subtypeaccr =string.Empty;
    //                                        string sub_code = Convert.ToString(dvMark[i]["subject_code"]).Trim();
    //                                        subtypeaccr = Convert.ToString(dvMark[i]["subject_type"]);
    //                                        subtypeaccr = findSubTypeAccromy(subtypeaccr);
    //                                        if (subtypeaccr.ToUpper().Trim() == "PRAC")
    //                                        {
    //                                            if (Convert.ToString(dvMark[i]["subject_name"]).ToLower() == "computer training" || Convert.ToString(dvMark[i]["subject_name"]).ToLower() == "skillbased computer training intro.to information techn. & ms office")
    //                                            {
    //                                                subtypeaccr = "CT";
    //                                            }
    //                                            if (subtypeaccr.ToUpper().Trim() == "PRAC")
    //                                            {
    //                                                string gt_sub_code = sub_code.Substring(sub_code.Length - 3, 1);
    //                                                if (gt_sub_code.ToUpper().Trim() == "M")
    //                                                {
    //                                                    subtypeaccr = "MC";
    //                                                }
    //                                                else if (gt_sub_code.ToUpper().Trim() == "A")
    //                                                {
    //                                                    subtypeaccr = "AC";
    //                                                }
    //                                                else if (gt_sub_code.ToUpper().Trim() == "E")
    //                                                {
    //                                                    subtypeaccr = "EC";
    //                                                }
    //                                                else if (gt_sub_code.ToUpper().Trim() == "L")
    //                                                {
    //                                                    subtypeaccr = "GE";
    //                                                }
    //                                                else
    //                                                {
    //                                                    subtypeaccr = "PRAC";
    //                                                }
    //                                            }
    //                                        }
    //                                        fullmark.Cell(mr, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                        fullmark.Cell(mr, 2).SetContent(subtypeaccr);
    //                                        double internalmmark = 0;
    //                                        double externalmark1 = 0;
    //                                        double totalintext = 0;
    //                                        double.TryParse(Convert.ToString(dvMark[i]["internal_mark"]), out internalmmark);
    //                                        fullmark.Cell(mr, 4).SetContentAlignment(ContentAlignment.MiddleCenter);

    //                                        string maxinternal = Convert.ToString(dvMark[i]["max_int_marks"]).Trim();
    //                                        string maxexternal = Convert.ToString(dvMark[i]["max_ext_marks"]).Trim();
    //                                        double extfinal = 0;
    //                                        double.TryParse(Convert.ToString(dvMark[i]["external_mark"]), out extfinal);
    //                                        extfinal = Math.Round(extfinal, 0);
    //                                        externalmark1 = extfinal;
    //                                        string checkedmark =string.Empty;
    //                                        if (extfinal < 0)
    //                                        {
    //                                            checkedmark = loadmarkat(Convert.ToString(extfinal));
    //                                        }
    //                                        else
    //                                        {
    //                                            checkedmark = Convert.ToString(extfinal);
    //                                        }

    //                                        if (internalmmark >= 0)
    //                                        {
    //                                            if (extfinal > 0)
    //                                            {
    //                                                totalintext = internalmmark + externalmark1;
    //                                            }
    //                                            else
    //                                            {
    //                                                totalintext = internalmmark;
    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            if (extfinal > 0)
    //                                            {
    //                                                totalintext = externalmark1;
    //                                            }
    //                                            else
    //                                            {
    //                                                totalintext = 0;
    //                                            }
    //                                        }
    //                                        onlyca = false;
    //                                        onlyes = false;


    //                                        if (maxinternal == "0" && maxexternal == "100")
    //                                        {
    //                                            onlyes = true;
    //                                        }
    //                                        if (maxinternal == "100" && maxexternal == "0")
    //                                        {
    //                                            onlyca = true;
    //                                        }
    //                                        fullmark.Cell(mr, 5).SetContentAlignment(ContentAlignment.MiddleCenter);

    //                                        if (onlyes == true && extfinal >= 0)
    //                                        {
    //                                            fullmark.Cell(mr, 5).SetContent(checkedmark + "/100");
    //                                        }
    //                                        else
    //                                        {
    //                                            fullmark.Cell(mr, 5).SetContent(checkedmark);
    //                                        }

    //                                        if (onlyca == true && internalmmark >= 0)
    //                                        {
    //                                            fullmark.Cell(mr, 4).SetContent(internalmmark + "/100");
    //                                        }
    //                                        else if (onlyes == false)
    //                                        {
    //                                            fullmark.Cell(mr, 4).SetContent(internalmmark);
    //                                        }
    //                                        else
    //                                        {
    //                                            fullmark.Cell(mr, 4).SetContent("NA");
    //                                            additonalstatus = 1;
    //                                        }

    //                                        if (onlyca)
    //                                        {
    //                                            fullmark.Cell(mr, 5).SetContent("NA");
    //                                            additonalstatus = 1;
    //                                        }
    //                                        double totfinal = 0;
    //                                        totfinal = Math.Round(totalintext, 0, MidpointRounding.AwayFromZero);

    //                                        fullmark.Cell(mr, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                        fullmark.Cell(mr, 6).SetContent(Convert.ToString(totfinal));

    //                                        double credit = 0;
    //                                        double.TryParse(Convert.ToString(dvMark[i]["credit_points"]), out credit);
    //                                        overallcreditearned += credit;
    //                                        fullmark.Cell(mr, 3).SetContentAlignment(ContentAlignment.MiddleCenter);

    //                                        if (starP3 == true)
    //                                        {
    //                                            if (arr_star.Contains(sub_code))
    //                                            {
    //                                                fullmark.Cell(mr, 3).SetContent(Convert.ToString(dvMark[i]["credit_points"] + "*"));
    //                                            }
    //                                            else
    //                                            {
    //                                                fullmark.Cell(mr, 3).SetContent(Convert.ToString(dvMark[i]["credit_points"]));
    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            fullmark.Cell(mr, 3).SetContent(Convert.ToString(dvMark[i]["credit_points"]));
    //                                        }
    //                                        double checkmarkmm = 0;
    //                                        double.TryParse(Convert.ToString(dvMark[i]["total"]), out checkmarkmm);
    //                                        checkmarkmm = Math.Round(checkmarkmm, 0, MidpointRounding.AwayFromZero);
    //                                        string gradesql = "    select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='" + sem + "' and Degree_Code='" + grade_set + "' and batch_year='" + grade_set + "' and '" + checkmarkmm + "' between Frange and Trange";
    //                                        gradeds.Clear();
    //                                        gradeds = da.select_method_wo_parameter(gradesql, "Text");
    //                                        if (gradeds.Tables[0].Rows.Count == 0)
    //                                        {
    //                                            gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and Degree_Code='" + grade_set + "' and batch_year='" + grade_set + "'  and '" + checkmarkmm + "' between Frange and Trange";//added by sridhar 
    //                                            gradeds.Clear();
    //                                            gradeds = da.select_method_wo_parameter(gradesql, "Text");
    //                                        }
    //                                        for (int grd = 0; grd < gradeds.Tables[0].Rows.Count; grd++)
    //                                        {
    //                                            if (Convert.ToInt32(gradeds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(gradeds.Tables[0].Rows[grd][2]) >= checkmarkmm)
    //                                            {
    //                                                grade = gradeds.Tables[0].Rows[grd][0].ToString();
    //                                                gradepoints = gradeds.Tables[0].Rows[grd]["Credit_Points"].ToString();
    //                                            }
    //                                        }
    //                                        double gradeibtpoint = 0;
    //                                        double.TryParse(Convert.ToString(dvMark[i]["total"].ToString()), out gradeibtpoint);
    //                                        gradeibtpoint = gradeibtpoint / 10;
    //                                        gradeibtpoint = Math.Round(gradeibtpoint, 1, MidpointRounding.AwayFromZero);
    //                                        string gp = String.Format("{0:0.0}", gradeibtpoint);
    //                                        gradepoints = Convert.ToString(gradeibtpoint);


    //                                        gradepoints = Convert.ToString(Math.Round(Convert.ToDouble(gradeibtpoint), 1, MidpointRounding.AwayFromZero));

    //                                        gradepoints = String.Format("{0:0.0}", gradepoints);

    //                                        string result = Convert.ToString(dvMark[i]["result"]).ToLower();
    //                                        if (result == "fail")
    //                                        {
    //                                            result = "RA";
    //                                            grade = "U";
    //                                        }
    //                                        else if (result == "pass")
    //                                        {
    //                                            result = "P";
    //                                        }
    //                                        else
    //                                        {
    //                                            result = "AB";
    //                                            grade = "U";
    //                                        }
    //                                        totfinal = totfinal * Convert.ToDouble(dvMark[i]["credit_points"].ToString());
    //                                        fullmark.Cell(mr, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                        fullmark.Cell(mr, 8).SetContent(grade);
    //                                        fullmark.Cell(mr, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                        fullmark.Cell(mr, 7).SetContent(gp);
    //                                        totfinal = Math.Round(totfinal, 0);

    //                                        fullmark.Cell(mr, 9).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                        DataView dv = new DataView();
    //                                        printds.Tables[2].DefaultView.RowFilter = "exam_code='" + dvMark[i]["exam_code"].ToString() + "'";
    //                                        dv = printds.Tables[2].DefaultView;
    //                                        if (dv.Count > 0)
    //                                        {
    //                                            string exam_y = dv[0]["Exam_year"].ToString();
    //                                            string exam_m = dv[0]["Exam_Month"].ToString();

    //                                            strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(exam_m));
    //                                            strMonthName = strMonthName[0].ToString() + strMonthName[1].ToString() + strMonthName[2].ToString();
    //                                            strMonthName = exam_y + " - " + strMonthName.ToUpper() + " ";
    //                                            fullmark.Cell(mr, 9).SetContent(strMonthName);
    //                                        }
    //                                        mr++;
    //                                    }
    //                                }
    //                                fullmark.Cell(mr, 9).SetCellPadding(5);
    //                                mr++;
    //                            }
    //                        }

    //                        if (rowSpecial == 0)
    //                        {
    //                            splcredit = 0;
    //                            fullmark.Cell(mr, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                            fullmark.Cell(mr, 1).SetFont(fontStmtMarks);
    //                            fullmark.Cell(mr, 1).SetContent("--- END OF STATEMENT ---");
    //                        }
    //                        else
    //                        {
    //                            if (dsSpl.Tables[0].Rows.Count > 0)
    //                            {
    //                                if ((edu_level.Trim().ToLower() == "ug"))
    //                                {
    //                                    splcredit = 1;
    //                                    fullmark.Cell(mr, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                    fullmark.Cell(mr, 1).SetContent("Part V " + Convert.ToString(dsSpl.Tables[0].Rows[0]["Subject_name"]));
    //                                    fullmark.Cell(mr, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                }
    //                                else
    //                                {
    //                                    splcredit = 2;
    //                                    fullmark.Cell(mr, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                    fullmark.Cell(mr, 1).SetContent("Part II " + Convert.ToString(dsSpl.Tables[0].Rows[0]["Subject_name"]));
    //                                    fullmark.Cell(mr, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                }
    //                                string subtypeaccr = Convert.ToString(dsSpl.Tables[0].Rows[0]["Subject_name"]);
    //                                subtypeaccr = findSubTypeAccromy(subtypeaccr);
    //                                fullmark.Cell(mr, 2).SetContent(Convert.ToString(dsSpl.Tables[0].Rows[0]["acronym"]));

    //                                fullmark.Cell(mr, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                fullmark.Cell(mr, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                fullmark.Cell(mr, 3).SetContent(splcredit);

    //                                fullmark.Cell(mr, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                fullmark.Cell(mr, 4).SetContent("NA");
    //                                fullmark.Cell(mr, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                fullmark.Cell(mr, 5).SetContent("NA");
    //                                fullmark.Cell(mr, 6).SetContentAlignment(ContentAlignment.MiddleCenter);

    //                                if (Convert.ToString(dsSpl.Tables[0].Rows[0]["MarkType"]).Trim() == "1")
    //                                {
    //                                    fullmark.Cell(mr, 6).SetContent("Good");
    //                                }
    //                                else if (Convert.ToString(dsSpl.Tables[0].Rows[0]["MarkType"]).Trim() == "2")
    //                                {
    //                                    fullmark.Cell(mr, 6).SetContent("Excellent");
    //                                }
    //                                else if (Convert.ToString(dsSpl.Tables[0].Rows[0]["MarkType"]).Trim() == "3")
    //                                {
    //                                    fullmark.Cell(mr, 6).SetContent("Outstanding");
    //                                }
    //                                else if (Convert.ToString(dsSpl.Tables[0].Rows[0]["MarkType"]).Trim() == "4")
    //                                {
    //                                    fullmark.Cell(mr, 6).SetContent("Average");
    //                                }
    //                                fullmark.Cell(mr, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                fullmark.Cell(mr, 7).SetContent("NA");

    //                                fullmark.Cell(mr, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                fullmark.Cell(mr, 8).SetContent("NA");
    //                            }

    //                            DataView dv1 = new DataView();
    //                            printds.Tables[2].DefaultView.RowFilter = "degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and Exam_year='" + batch_year1.Split('-')[1] + "'";
    //                            dv1 = printds.Tables[2].DefaultView;
    //                            dv1.Sort = "exam_code,current_semester";
    //                            if (dv1.Count > 0)
    //                            {
    //                                string exam_y = dv1[0]["Exam_year"].ToString();
    //                                string exam_m = dv1[0]["Exam_Month"].ToString();

    //                                strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(exam_m));
    //                                strMonthName = strMonthName[0].ToString() + strMonthName[1].ToString() + strMonthName[2].ToString();
    //                                strMonthName = exam_y + " - " + strMonthName.ToUpper() + " ";
    //                                fullmark.Cell(mr, 9).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                fullmark.Cell(mr, 9).SetContent(strMonthName);
    //                            }
    //                            fullmark.Cell(mr + 1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

    //                            fullmark.Cell(mr + 1, 1).SetCellPadding(12);
    //                            fullmark.Cell(mr + 1, 1).SetContent("--- END OF STATEMENT ---");

    //                        }
    //                        // x=13,y=70.8,W= ,H= 
    //                        newpdftabpage2 = fullmark.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 0, 175, 730, 650));

    //                        mypdfpage.Add(newpdftabpage2);
    //                        PdfTextArea pdfAdditional = new PdfTextArea(fontStmtMarks, System.Drawing.Color.Black, new PdfArea(mydoc, 86, 785 + 10, 500, 25), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(additionalmsg));
    //                        mypdfpage.Add(pdfAdditional);

    //                        int classifyrows = 0;
    //                        bool clasipg = false;
    //                        if (((edu_level.Trim().ToLower() == "ug" || edu_level.Trim().ToLower() == "u.g")))
    //                        {
    //                            classifyrows = 6;
    //                            splcredit = 1;
    //                            clasipg = false;
    //                        }
    //                        else
    //                        {
    //                            clasipg = true;
    //                            splcredit = 2;
    //                            classifyrows = 4;
    //                        }
    //                        PdfTable tblcgpaclass = mydoc.NewTable(fontcgpa, classifyrows, 8, 4);
    //                        tblcgpaclass.VisibleHeaders = false;
    //                        tblcgpaclass.SetBorders(Color.Black, 1, BorderType.None);
    //                        tblcgpaclass.Columns[0].SetWidth(28);
    //                        tblcgpaclass.Columns[1].SetWidth(111);
    //                        tblcgpaclass.Columns[2].SetWidth(57);
    //                        tblcgpaclass.Columns[3].SetWidth(57);
    //                        tblcgpaclass.Columns[4].SetWidth(57);
    //                        tblcgpaclass.Columns[5].SetWidth(193);
    //                        tblcgpaclass.Columns[6].SetWidth(135);
    //                        tblcgpaclass.Columns[7].SetWidth(71);

    //                        DataTable dtPart1 = new DataTable();
    //                        string batchsetting = "0";

    //                        double partsums = 0.000;
    //                        double partwpmsum = 0.000;
    //                        int partrowcount = 0;
    //                        Double Credit_Points = 0.0;
    //                        Double grade_points = 0.0;
    //                        double creditstotal = 0;
    //                        double overalltotgrade = 0;
    //                        double Marks = 0;

    //                        tblcgpaclass.Cell(2, 0).SetContent("I");
    //                        tblcgpaclass.Cell(3, 0).SetContent("II");


    //                        if (clasipg == false)
    //                        {
    //                            tblcgpaclass.Cell(4, 0).SetContent("III");
    //                            tblcgpaclass.Cell(5, 0).SetContent("IV");
    //                            tblcgpaclass.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            tblcgpaclass.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        }
    //                        tblcgpaclass.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        tblcgpaclass.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);

    //                        if (printds_rows.Tables[0].Rows.Count > 0)
    //                        {
    //                            tblcgpaclass.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            tblcgpaclass.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            tblcgpaclass.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            string sumpart =string.Empty;
    //                            string wpm =string.Empty;
    //                            DataView dv_demand_data = new DataView();
    //                            DataView dv_demand_datadummy = new DataView();
    //                            printds_rows.Tables[0].DefaultView.RowFilter = "Part_Type='1'";
    //                            dv_demand_datadummy = printds_rows.Tables[0].DefaultView;
    //                            partrowcount = dv_demand_datadummy.Count;
    //                            printds_rows.Tables[0].DefaultView.RowFilter = "Part_Type='1' and result='pass'";
    //                            dtPart1 = printds_rows.Tables[0].DefaultView.ToTable();
    //                            if (dtPart1.Rows.Count > 0 && partrowcount == dtPart1.Rows.Count)
    //                            {
    //                                for (int sum = 0; sum < dtPart1.Rows.Count; sum++)
    //                                {
    //                                    //double checkmarkmm = Convert.ToDouble(dtPart1.Rows[sum]["total"].ToString());

    //                                    double checkmarkmm = 0;
    //                                    double.TryParse(Convert.ToString(dtPart1.Rows[sum]["total"]), out checkmarkmm);
    //                                    checkmarkmm = Math.Round(checkmarkmm, 0, MidpointRounding.AwayFromZero);
    //                                    string gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='" + sem + "' and Degree_Code='" + grade_set + "' and batch_year='" + grade_set + "' and '" + checkmarkmm + "' between frange and trange";//added by sridhar 16/aug 2014
    //                                    gradeds.Clear();
    //                                    gradeds = da.select_method_wo_parameter(gradesql, "Text");
    //                                    if (gradeds.Tables[0].Rows.Count == 0)
    //                                    {
    //                                        gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "' and Degree_Code='" + grade_set + "' and batch_year='" + grade_set + "'  and '" + checkmarkmm + "' between frange and trange";//added by sridhar 
    //                                        gradeds.Clear();
    //                                        gradeds = da.select_method_wo_parameter(gradesql, "Text");
    //                                    }
    //                                    for (int grd = 0; grd < gradeds.Tables[0].Rows.Count; grd++)
    //                                    {
    //                                        if (Convert.ToInt32(gradeds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(gradeds.Tables[0].Rows[grd][2]) >= checkmarkmm)
    //                                        {
    //                                            //grade_points = Convert.ToDouble(dtPart1.Rows[sum]["total"].ToString());
    //                                            double.TryParse(Convert.ToString(dtPart1.Rows[sum]["total"]), out grade_points);
    //                                            double.TryParse(Convert.ToString(dtPart1.Rows[sum]["total"]), out Marks);
    //                                            grade_points = grade_points / 10;
    //                                            //Credit_Points = Convert.ToDouble(dtPart1.Rows[sum]["credit_points"].ToString());
    //                                            double.TryParse(Convert.ToString(dtPart1.Rows[sum]["credit_points"]), out Credit_Points);
    //                                            creditstotal = creditstotal + Credit_Points;
    //                                            partwpmsum += (Credit_Points * Marks);
    //                                            partsums = partsums + (grade_points * Credit_Points);
    //                                        }
    //                                    }
    //                                }

    //                                if (creditstotal == 0)
    //                                {
    //                                    sumpart = "0.000";
    //                                    wpm = "0.00";
    //                                }
    //                                else if (creditstotal > 0)
    //                                {
    //                                    partsums = (partsums / creditstotal);
    //                                    partsums = Math.Round(partsums, 3, MidpointRounding.AwayFromZero);
    //                                    partwpmsum = (partwpmsum / creditstotal);
    //                                    partwpmsum = Math.Round(partwpmsum, 2, MidpointRounding.AwayFromZero);
    //                                    sumpart = String.Format("{0:0.000}", partsums);
    //                                    wpm = string.Format("{0:0.00}", partwpmsum);
    //                                }
    //                                else
    //                                {
    //                                    sumpart = "0.000";
    //                                    wpm = "0.00";
    //                                }
    //                            }
    //                            else if (partrowcount > 0)
    //                            {
    //                                sumpart = "0.000";
    //                                wpm = "0.00";
    //                            }
    //                            else
    //                            {
    //                                sumpart = "---";
    //                                wpm = "---";
    //                            }
    //                            if (sumpart != "---")
    //                            {
    //                                double sumpartgrade = 0;
    //                                if (double.TryParse(sumpart, out sumpartgrade))
    //                                {
    //                                    sumpartgrade = Convert.ToDouble(sumpart);
    //                                    overalltotgrade = overalltotgrade + sumpartgrade;
    //                                }
    //                                else
    //                                {
    //                                    sumpartgrade = 0;
    //                                }
    //                                tblcgpaclass.Cell(2, 1).SetContent(creditstotal);
    //                                tblcgpaclass.Cell(2, 3).SetContent(sumpart);
    //                                tblcgpaclass.Cell(2, 2).SetContent(wpm);
    //                                batchsetting = "1";
    //                                if (noofsubfailed != 0 && overalltotgrade >= 6)
    //                                {
    //                                    string gradesqlclass = "select * from coe_classification where edu_level='" + edu_level + "'  and  '" + overalltotgrade + "'>= frompoint and '" + overalltotgrade + "'<= topoint and  markgradeflag='" + batchsetting + "'";//added by sridhar 16/aug 2014  and (classification='First Class' or classification='First')
    //                                    gradeds.Clear();
    //                                    gradeds = da.select_method_wo_parameter(gradesqlclass, "Text");
    //                                    string cclass = "First Class";
    //                                    if (gradeds.Tables[0].Rows.Count > 0)
    //                                    {
    //                                        tblcgpaclass.Cell(2, 4).SetContent(Convert.ToString(gradeds.Tables[0].Rows[0]["grade"]));
    //                                        tblcgpaclass.Cell(2, 5).SetContent(cclass);
    //                                    }
    //                                    else
    //                                    {
    //                                        tblcgpaclass.Cell(2, 4).SetContent(Convert.ToString("A"));
    //                                        tblcgpaclass.Cell(2, 5).SetContent(cclass);
    //                                    }
    //                                    tblcgpaclass.Cell(2, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    tblcgpaclass.Cell(2, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                }
    //                                else
    //                                {
    //                                    string gradesqlclass = "select * from coe_classification where edu_level='" + edu_level + "'  and  '" + overalltotgrade + "'>= frompoint and '" + overalltotgrade + "'<= topoint and  markgradeflag='" + batchsetting + "'";//added by sridhar 16/aug 2014
    //                                    gradeds.Clear();
    //                                    gradeds = da.select_method_wo_parameter(gradesqlclass, "Text");
    //                                    if (gradeds.Tables[0].Rows.Count > 0)
    //                                    {
    //                                        tblcgpaclass.Cell(2, 4).SetContent(Convert.ToString(gradeds.Tables[0].Rows[0]["grade"]));
    //                                        tblcgpaclass.Cell(2, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                        tblcgpaclass.Cell(2, 5).SetContent(gradeds.Tables[0].Rows[0]["classification"].ToString());
    //                                        tblcgpaclass.Cell(2, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                    }
    //                                }
    //                            }
    //                            else
    //                            {
    //                                tblcgpaclass.Cell(2, 1).SetContent(creditstotal);
    //                                tblcgpaclass.Cell(2, 3).SetContent(sumpart);
    //                                tblcgpaclass.Cell(2, 2).SetContent(wpm);
    //                            }
    //                        }
    //                        else
    //                        {
    //                            tblcgpaclass.Cell(2, 1).SetContent("---");
    //                            tblcgpaclass.Cell(2, 2).SetContent("---");
    //                            tblcgpaclass.Cell(2, 3).SetContent("---");
    //                        }

    //                        partsums = 0;
    //                        partrowcount = 0;
    //                        creditstotal = 0;
    //                        partwpmsum = 0;
    //                        overalltotgrade = 0;
    //                        if (printds_rows.Tables[0].Rows.Count > 0)
    //                        {
    //                            string sumpart =string.Empty;
    //                            string wpm =string.Empty;
    //                            DataView dv_demand_data = new DataView();
    //                            DataView dv_demand_datadummy = new DataView();
    //                            printds_rows.Tables[0].DefaultView.RowFilter = "Part_Type='2'";
    //                            dv_demand_datadummy = printds_rows.Tables[0].DefaultView;
    //                            partrowcount = dv_demand_datadummy.Count;
    //                            printds_rows.Tables[0].DefaultView.RowFilter = "Part_Type='2' and result='pass'";
    //                            dtPart1 = printds_rows.Tables[0].DefaultView.ToTable();
    //                            tblcgpaclass.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            tblcgpaclass.Cell(3, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            tblcgpaclass.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            tblcgpaclass.Cell(3, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            tblcgpaclass.Cell(3, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                            if (dtPart1.Rows.Count > 0 && partrowcount == dtPart1.Rows.Count)
    //                            {
    //                                for (int sum = 0; sum < dtPart1.Rows.Count; sum++)
    //                                {
    //                                    double checkmarkmm = 0;
    //                                    double.TryParse(Convert.ToString(dtPart1.Rows[sum]["total"]), out checkmarkmm);
    //                                    checkmarkmm = Math.Round(checkmarkmm, 0, MidpointRounding.AwayFromZero);
    //                                    string gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='" + sem + "' and Degree_Code='" + grade_set + "' and batch_year='" + grade_set + "' and '" + checkmarkmm + "' between frange and trange";//added by sridhar 16/aug 2014
    //                                    gradeds.Clear();
    //                                    gradeds = da.select_method_wo_parameter(gradesql, "Text");
    //                                    if (gradeds.Tables[0].Rows.Count == 0)
    //                                    {
    //                                        gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "' and Degree_Code='" + grade_set + "' and batch_year='" + grade_set + "'  and '" + checkmarkmm + "' between frange and trange";//added by sridhar 
    //                                        gradeds.Clear();
    //                                        gradeds = da.select_method_wo_parameter(gradesql, "Text");
    //                                    }
    //                                    for (int grd = 0; grd < gradeds.Tables[0].Rows.Count; grd++)
    //                                    {
    //                                        if (Convert.ToInt32(gradeds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(gradeds.Tables[0].Rows[grd][2]) >= checkmarkmm)
    //                                        {
    //                                            //grade_points = Convert.ToDouble(dtPart1.Rows[sum]["total"].ToString());
    //                                            double.TryParse(Convert.ToString(dtPart1.Rows[sum]["total"]), out grade_points);

    //                                            double.TryParse(Convert.ToString(dtPart1.Rows[sum]["total"]), out Marks);
    //                                            grade_points = grade_points / 10;
    //                                            //Credit_Points = Convert.ToDouble(dtPart1.Rows[sum]["credit_points"].ToString());
    //                                            double.TryParse(Convert.ToString(dtPart1.Rows[sum]["credit_points"]), out Credit_Points);

    //                                            creditstotal = creditstotal + Credit_Points;

    //                                            partwpmsum += (Credit_Points * Marks);
    //                                            partsums = partsums + (grade_points * Credit_Points);
    //                                        }

    //                                    }
    //                                }

    //                                if (creditstotal == 0)
    //                                {
    //                                    sumpart = "0.000";
    //                                    wpm = "0.00";
    //                                }
    //                                else if (creditstotal > 0)
    //                                {
    //                                    partsums = (partsums / creditstotal);
    //                                    partsums = Math.Round(partsums, 3, MidpointRounding.AwayFromZero);
    //                                    partwpmsum = (partwpmsum / creditstotal);
    //                                    partwpmsum = Math.Round(partwpmsum, 2, MidpointRounding.AwayFromZero);
    //                                    sumpart = String.Format("{0:0.000}", partsums);
    //                                    wpm = string.Format("{0:0.00}", partwpmsum);
    //                                }
    //                                else
    //                                {
    //                                    sumpart = "0.000";
    //                                    wpm = "0.00";
    //                                }
    //                            }
    //                            else if (partrowcount > 0)
    //                            {
    //                                sumpart = "0.000";
    //                                wpm = "0.00";
    //                            }
    //                            else
    //                            {
    //                                sumpart = "---";
    //                                wpm = "---";
    //                            }
    //                            if (sumpart != "---")
    //                            {
    //                                double sumpartgrade = 0;
    //                                if (double.TryParse(sumpart, out sumpartgrade))
    //                                {
    //                                    sumpartgrade = Convert.ToDouble(sumpart);
    //                                    overalltotgrade = overalltotgrade + sumpartgrade;

    //                                }
    //                                else
    //                                {
    //                                    sumpartgrade = 0;
    //                                }
    //                                if (clasipg == false)
    //                                {
    //                                    tblcgpaclass.Cell(3, 1).SetContent(creditstotal);
    //                                    tblcgpaclass.Cell(3, 3).SetContent(sumpart);
    //                                    tblcgpaclass.Cell(3, 2).SetContent(wpm);
    //                                }
    //                                else
    //                                {
    //                                    tblcgpaclass.Cell(3, 1).SetContent(creditstotal + splcredit);
    //                                    tblcgpaclass.Cell(3, 3).SetContent("---");
    //                                    tblcgpaclass.Cell(3, 2).SetContent("---");
    //                                }
    //                                batchsetting = "1";
    //                                if (noofsubfailed != 0 && overalltotgrade >= 6)
    //                                {
    //                                    string gradesqlclass = "select * from coe_classification where edu_level='" + edu_level + "'  and  '" + overalltotgrade + "'>= frompoint and '" + overalltotgrade + "'<= topoint and  markgradeflag='" + batchsetting + "'";//added by sridhar 16/aug 2014  and (classification='First Class' or classification='First')
    //                                    gradeds.Clear();
    //                                    gradeds = da.select_method_wo_parameter(gradesqlclass, "Text");
    //                                    string cclass = "First Class";
    //                                    if (gradeds.Tables[0].Rows.Count > 0)
    //                                    {
    //                                        if (clasipg == false)
    //                                        {
    //                                            tblcgpaclass.Cell(3, 4).SetContent(Convert.ToString(gradeds.Tables[0].Rows[0]["grade"]));
    //                                            tblcgpaclass.Cell(3, 5).SetContent(cclass);
    //                                        }
    //                                        else
    //                                        {
    //                                            tblcgpaclass.Cell(3, 4).SetContent("---");//Convert.ToString(gradeds.Tables[0].Rows[0]["grade"]));
    //                                            tblcgpaclass.Cell(3, 5).SetContent("---");//cclass);
    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        if (clasipg == false)
    //                                        {
    //                                            tblcgpaclass.Cell(3, 4).SetContent(Convert.ToString("A"));
    //                                            tblcgpaclass.Cell(3, 5).SetContent(cclass);
    //                                        }
    //                                        else
    //                                        {
    //                                            tblcgpaclass.Cell(3, 4).SetContent("---");
    //                                            tblcgpaclass.Cell(3, 5).SetContent("---");//cclass);
    //                                        }
    //                                    }
    //                                    tblcgpaclass.Cell(3, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    tblcgpaclass.Cell(3, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                }
    //                                else
    //                                {
    //                                    string gradesqlclass = "select * from coe_classification where edu_level='" + edu_level + "'  and  '" + overalltotgrade + "'>= frompoint and '" + overalltotgrade + "'<= topoint and  markgradeflag='" + batchsetting + "'";//added by sridhar 16/aug 2014
    //                                    gradeds.Clear();
    //                                    gradeds = da.select_method_wo_parameter(gradesqlclass, "Text");
    //                                    if (gradeds.Tables[0].Rows.Count > 0)
    //                                    {
    //                                        if (clasipg == false)
    //                                        {
    //                                            tblcgpaclass.Cell(3, 4).SetContent(Convert.ToString(gradeds.Tables[0].Rows[0]["grade"]));
    //                                            tblcgpaclass.Cell(3, 5).SetContent(gradeds.Tables[0].Rows[0]["classification"].ToString());
    //                                        }
    //                                        else
    //                                        {
    //                                            tblcgpaclass.Cell(3, 4).SetContent("---");//Convert.ToString(gradeds.Tables[0].Rows[0]["grade"]));
    //                                            tblcgpaclass.Cell(3, 5).SetContent("---");//cclass);
    //                                        }
    //                                    }
    //                                }
    //                                tblcgpaclass.Cell(3, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                tblcgpaclass.Cell(3, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                            }
    //                            else
    //                            {
    //                                if (clasipg == false)
    //                                {
    //                                    tblcgpaclass.Cell(3, 1).SetContent(creditstotal);
    //                                    tblcgpaclass.Cell(3, 3).SetContent(sumpart);
    //                                    tblcgpaclass.Cell(3, 2).SetContent(wpm);
    //                                }
    //                                else
    //                                {
    //                                    tblcgpaclass.Cell(3, 1).SetContent(creditstotal + splcredit);
    //                                    tblcgpaclass.Cell(3, 3).SetContent("---");
    //                                    tblcgpaclass.Cell(3, 2).SetContent("---");
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            tblcgpaclass.Cell(3, 1).SetContent("---");
    //                            tblcgpaclass.Cell(3, 2).SetContent("---");
    //                            tblcgpaclass.Cell(3, 3).SetContent("---");
    //                        }

    //                        DataTable dtPart3 = new DataTable();

    //                        if ((edu_level.Trim().ToLower() == "ug" || edu_level.Trim().ToLower() == "u.g"))
    //                        {
    //                            tot_credits = tot_credits + 1;
    //                            creditsdiff = 0;
    //                            if (tot_credits > setng_mintotalcreadits)
    //                            {
    //                                creditsdiff = tot_credits - setng_mintotalcreadits;
    //                                creditsdiff = creditsdiff / 5;
    //                            }
    //                            partsums = 0;
    //                            partrowcount = 0;
    //                            creditstotal = 0;
    //                            partwpmsum = 0;
    //                            overalltotgrade = 0;
    //                            double min_credit = 0;
    //                            double Majorcredit = 0;
    //                            double Tot_Part3_Credit = 0;
    //                            double Tot_part3_Earned_credit = 0;
    //                            double Tot_Major_credit = 0;
    //                            double Tot_alied_Credit = 0;
    //                            double aliedCredit = 0;
    //                            if (printds_rows.Tables[0].Rows.Count > 0)
    //                            {
    //                                string sumpart =string.Empty;
    //                                string wpm =string.Empty;
    //                                removesubjetcs =string.Empty;
    //                                DataView dv_demand_data = new DataView();
    //                                DataView dv_demand_datadummy = new DataView();
    //                                cutsubject = new DataSet();
    //                                if (creditsdiff > 0)
    //                                {

    //                                    sql = "Select  top " + creditsdiff + " subject.subject_no,credit_points from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'  and roll_no='" + rollnosub + "' and Part_Type=3 and SUBSTRING(subject_code,7,1)='M' order by total asc,credit_points asc";

    //                                    cutsubject.Clear();
    //                                    cutsubject = da.select_method_wo_parameter(sql, "Text");
    //                                    int removecredites = 0;
    //                                    for (int ii = 0; ii < cutsubject.Tables[0].Rows.Count; ii++)
    //                                    {
    //                                        if (removecredites == 0)
    //                                        {
    //                                            removecredites = Convert.ToInt32(cutsubject.Tables[0].Rows[ii][1].ToString());
    //                                        }
    //                                        else
    //                                        {
    //                                            removecredites = removecredites + Convert.ToInt32(cutsubject.Tables[0].Rows[ii][1].ToString());
    //                                        }
    //                                    }
    //                                    if (removecredites <= 10)
    //                                    {

    //                                        for (int ii = 0; ii < cutsubject.Tables[0].Rows.Count; ii++)
    //                                        {
    //                                            if (removesubjetcs.Trim() == "")
    //                                            {
    //                                                removesubjetcs = cutsubject.Tables[0].Rows[ii][0].ToString();
    //                                            }
    //                                            else
    //                                            {
    //                                                removesubjetcs = removesubjetcs + "," + cutsubject.Tables[0].Rows[ii][0].ToString();
    //                                            }
    //                                            arr_star.Add(Convert.ToString(cutsubject.Tables[0].Rows[ii][0]));
    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        removesubjetcs = cutsubject.Tables[0].Rows[0][0].ToString();
    //                                        arr_star.Add(Convert.ToString(cutsubject.Tables[0].Rows[0][0]));
    //                                    }
    //                                }
    //                                if (removesubjetcs.Trim() != "")
    //                                {
    //                                    printds_rows.Tables[0].DefaultView.RowFilter = "part_type='3' and subject_no not in (" + removesubjetcs + ")";
    //                                }
    //                                else
    //                                {
    //                                    printds_rows.Tables[0].DefaultView.RowFilter = "part_type='3'";
    //                                }
    //                                // printds_rows.Tables[0].DefaultView.RowFilter = "Part_Type='3'";
    //                                dv_demand_datadummy = printds_rows.Tables[0].DefaultView;
    //                                partrowcount = dv_demand_datadummy.Count;
    //                                if (removesubjetcs.Trim() != "")
    //                                {
    //                                    printds_rows.Tables[0].DefaultView.RowFilter = "part_type='3' and result='pass'  and subject_no not in (" + removesubjetcs + ")";
    //                                }
    //                                else
    //                                {
    //                                    printds_rows.Tables[0].DefaultView.RowFilter = "part_type='3' and result='pass'";
    //                                }
    //                                // printds_rows.Tables[0].DefaultView.RowFilter = "Part_Type='3' and result='pass'";
    //                                dtPart1 = printds_rows.Tables[0].DefaultView.ToTable();
    //                                dtPart3 = printds_rows.Tables[0].DefaultView.ToTable();
    //                                //object mini = Convert.ToInt16(dtPart3.Compute("Min(total)", "credit_points=5 and subject_type='Major 


    //                                object mini = Convert.ToInt16(dtPart3.Compute("Min(total)", "subject_type='Major Course'"));
    //                                double minimum = 0;
    //                                double.TryParse(Convert.ToString(mini), out minimum);
    //                                double min_gpa = 0;
    //                                double min_wpm = 0;
    //                                min_gpa = (minimum / 10) * 5;
    //                                min_wpm = minimum * 5;

    //                                if (dv_demand_datadummy.Count > 0)
    //                                {
    //                                    Tot_Part3_Credit = 0;
    //                                    for (int tc = 0; tc < dv_demand_datadummy.Count; tc++)
    //                                    {
    //                                        double dummycredit = 0;
    //                                        string sub_code = Convert.ToString(dv_demand_datadummy[tc]["subject_code"]).Trim();
    //                                        string gt_sub_code = sub_code.Substring(sub_code.Length - 3, 1);
    //                                        string subtypeaccr =string.Empty;
    //                                        subtypeaccr = Convert.ToString(dv_demand_datadummy[tc]["subject_type"]);
    //                                        subtypeaccr = findSubTypeAccromy(subtypeaccr);
    //                                        if (subtypeaccr.ToUpper().Trim() == "PRAC")
    //                                        {
    //                                            if (gt_sub_code.ToUpper().Trim() == "M")
    //                                            {
    //                                                subtypeaccr = "MC";
    //                                            }
    //                                            else if (gt_sub_code.ToUpper().Trim() == "A")
    //                                            {
    //                                                subtypeaccr = "AC";
    //                                            }
    //                                            else
    //                                            {
    //                                                subtypeaccr = "PRAC";
    //                                            }
    //                                        }
    //                                        double.TryParse(Convert.ToString(dv_demand_datadummy[tc]["credit_points"]), out dummycredit);
    //                                        Tot_Part3_Credit += dummycredit;
    //                                        if (Convert.ToString(dv_demand_datadummy[tc]["subject_type"]).Trim().ToLower() == "major course" && subtypeaccr.ToLower().Trim() == "mc")
    //                                        {
    //                                            Tot_Major_credit += dummycredit;
    //                                        }
    //                                        else if (Convert.ToString(dv_demand_datadummy[tc]["subject_type"]).Trim().ToLower() == "allied course" && subtypeaccr.ToLower().Trim() == "ac")
    //                                        {
    //                                            Tot_alied_Credit += dummycredit;
    //                                        }
    //                                    }
    //                                }

    //                                if (clasipg == false)
    //                                {
    //                                    tblcgpaclass.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    tblcgpaclass.Cell(4, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    tblcgpaclass.Cell(4, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    tblcgpaclass.Cell(4, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    tblcgpaclass.Cell(4, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                }
    //                                else
    //                                {
    //                                    tblcgpaclass.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    tblcgpaclass.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    tblcgpaclass.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    tblcgpaclass.Cell(2, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    tblcgpaclass.Cell(2, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                }

    //                                //if (dtPart1.Rows.Count > 0 && partrowcount == dtPart1.Rows.Count) //hidden by malang raja on 30/03/2016 
    //                                if (dtPart1.Rows.Count > 0)
    //                                {
    //                                    for (int sum = 0; sum < dtPart1.Rows.Count; sum++)
    //                                    {
    //                                        //if (Convert.ToString(dtPart1.Rows[sum]["total"]) != Convert.ToString(minimum))
    //                                        //{
    //                                        double dummycredit = 0;
    //                                        string sub_code = Convert.ToString(dtPart1.Rows[sum]["subject_code"]).Trim();
    //                                        string gt_sub_code = sub_code.Substring(sub_code.Length - 3, 1);
    //                                        string subtypeaccr =string.Empty;
    //                                        subtypeaccr = Convert.ToString(dtPart1.Rows[sum]["subject_type"]);
    //                                        subtypeaccr = findSubTypeAccromy(subtypeaccr);
    //                                        if (subtypeaccr.ToUpper().Trim() == "PRAC")
    //                                        {
    //                                            if (gt_sub_code.ToUpper().Trim() == "M")
    //                                            {
    //                                                subtypeaccr = "MC";
    //                                            }
    //                                            else if (gt_sub_code.ToUpper().Trim() == "A")
    //                                            {
    //                                                subtypeaccr = "AC";
    //                                            }
    //                                            else if (gt_sub_code.ToUpper().Trim() == "E")
    //                                            {
    //                                                subtypeaccr = "EC";
    //                                            }
    //                                            else
    //                                            {
    //                                                subtypeaccr = "PRAC";
    //                                            }
    //                                        }
    //                                        double.TryParse(Convert.ToString(dtPart1.Rows[sum]["credit_points"]), out dummycredit);
    //                                        Tot_part3_Earned_credit += dummycredit;
    //                                        if (Convert.ToString(dtPart1.Rows[sum]["subject_type"]).Trim().ToLower() == "major course" && subtypeaccr.ToLower().Trim() == "mc")
    //                                        {
    //                                            Majorcredit += dummycredit;
    //                                        }
    //                                        else if (Convert.ToString(dtPart1.Rows[sum]["subject_type"]).Trim().ToLower() == "allied course" && subtypeaccr.ToLower().Trim() == "ac")
    //                                        {
    //                                            aliedCredit += dummycredit;
    //                                        }

    //                                        double checkmarkmm = 0;
    //                                        double.TryParse(Convert.ToString(dtPart1.Rows[sum]["total"]), out checkmarkmm);
    //                                        checkmarkmm = Math.Round(checkmarkmm, 0, MidpointRounding.AwayFromZero);
    //                                        string gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='" + sem + "' and Degree_Code='" + grade_set + "' and batch_year='" + grade_set + "' and '" + checkmarkmm + "' between frange and trange";
    //                                        gradeds.Clear();
    //                                        gradeds = da.select_method_wo_parameter(gradesql, "Text");
    //                                        if (gradeds.Tables[0].Rows.Count == 0)
    //                                        {
    //                                            gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "' and Degree_Code='" + grade_set + "' and batch_year='" + grade_set + "'  and '" + checkmarkmm + "' between frange and trange";//added by sridhar 
    //                                            gradeds.Clear();
    //                                            gradeds = da.select_method_wo_parameter(gradesql, "Text");
    //                                        }
    //                                        for (int grd = 0; grd < gradeds.Tables[0].Rows.Count; grd++)
    //                                        {
    //                                            if (Convert.ToInt32(gradeds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(gradeds.Tables[0].Rows[grd][2]) >= checkmarkmm)
    //                                            {  
    //                                                double.TryParse(Convert.ToString(dtPart1.Rows[sum]["total"]), out grade_points);

    //                                                double.TryParse(Convert.ToString(dtPart1.Rows[sum]["total"]), out Marks);
    //                                                grade_points = grade_points / 10;

    //                                                double.TryParse(Convert.ToString(dtPart1.Rows[sum]["credit_points"]), out Credit_Points);

    //                                                creditstotal = creditstotal + Credit_Points;

    //                                                partwpmsum += (Credit_Points * Marks);
    //                                                partsums = partsums + (grade_points * Credit_Points);
    //                                            }
    //                                        }
    //                                    }

    //                                    if (creditstotal == 0)
    //                                    {
    //                                        sumpart = "0.000";
    //                                        wpm = "0.00";
    //                                    }
    //                                    else if (creditstotal > 0)
    //                                    {                                           
    //                                        partsums = (partsums / creditstotal);
    //                                        partsums = Math.Round(partsums, 3, MidpointRounding.AwayFromZero);
    //                                        partwpmsum = (partwpmsum / creditstotal);
    //                                        partwpmsum = Math.Round(partwpmsum, 2, MidpointRounding.AwayFromZero);
    //                                        sumpart = String.Format("{0:0.000}", partsums);
    //                                        wpm = string.Format("{0:0.00}", partwpmsum);
    //                                    }
    //                                    else
    //                                    {
    //                                        sumpart = "0.000";
    //                                        wpm = "0.00";
    //                                    }
    //                                }
    //                                else if (partrowcount > 0)
    //                                {
    //                                    sumpart = "0.000";
    //                                    wpm = "0.00";
    //                                }
    //                                else
    //                                {
    //                                    sumpart = "---";
    //                                    wpm = "---";
    //                                }
    //                                if (sumpart != "---")
    //                                {
    //                                    double sumpartgrade = 0;
    //                                    if (double.TryParse(sumpart, out sumpartgrade))
    //                                    {
    //                                        sumpartgrade = Convert.ToDouble(sumpart);
    //                                        overalltotgrade = overalltotgrade + sumpartgrade;
    //                                    }
    //                                    else
    //                                    {
    //                                        sumpartgrade = 0;
    //                                    }
    //                                    if (clasipg == false)
    //                                    {
    //                                        tblcgpaclass.Cell(4, 1).SetContent(creditstotal);
    //                                        tblcgpaclass.Cell(4, 3).SetContent(sumpart);
    //                                        tblcgpaclass.Cell(4, 2).SetContent(wpm);
    //                                    }
    //                                    else
    //                                    {
    //                                        tblcgpaclass.Cell(2, 1).SetContent(creditstotal);
    //                                        tblcgpaclass.Cell(2, 3).SetContent(sumpart);
    //                                        tblcgpaclass.Cell(2, 2).SetContent(wpm);
    //                                    }

    //                                    batchsetting = "1";
    //                                    if (noofsubfailed != 0 && overalltotgrade >= 6)
    //                                    {
    //                                        string gradesqlclass = "select * from coe_classification where edu_level='" + edu_level + "'  and  '" + overalltotgrade + "'>= frompoint and '" + overalltotgrade + "'<= topoint and  markgradeflag='" + batchsetting + "'";//added by sridhar 16/aug 2014 and (classification='First Class' or classification='First')
    //                                        gradeds.Clear();
    //                                        gradeds = da.select_method_wo_parameter(gradesqlclass, "Text");
    //                                        string cclass = "First Class";
    //                                        if (gradeds.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (clasipg == false)
    //                                            {
    //                                                tblcgpaclass.Cell(4, 4).SetContent(Convert.ToString(gradeds.Tables[0].Rows[0]["grade"]));
    //                                                tblcgpaclass.Cell(4, 5).SetContent(cclass);
    //                                            }
    //                                            else
    //                                            {
    //                                                tblcgpaclass.Cell(2, 4).SetContent(Convert.ToString(gradeds.Tables[0].Rows[0]["grade"]));
    //                                                tblcgpaclass.Cell(2, 5).SetContent(cclass);
    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            if (clasipg == false)
    //                                            {
    //                                                tblcgpaclass.Cell(4, 4).SetContent("A");
    //                                                tblcgpaclass.Cell(4, 5).SetContent(cclass);
    //                                            }
    //                                            else
    //                                            {
    //                                                tblcgpaclass.Cell(2, 4).SetContent("A");
    //                                                tblcgpaclass.Cell(2, 5).SetContent(cclass);
    //                                            }
    //                                        }
    //                                        if (clasipg == false)
    //                                        {
    //                                            tblcgpaclass.Cell(4, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            tblcgpaclass.Cell(4, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                        }
    //                                        else
    //                                        {
    //                                            tblcgpaclass.Cell(2, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            tblcgpaclass.Cell(2, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        string gradesqlclass = "select * from coe_classification where edu_level='" + edu_level + "'  and  '" + overalltotgrade + "'>= frompoint and '" + overalltotgrade + "'<= topoint and  markgradeflag='" + batchsetting + "'";//added by sridhar 16/aug 2014
    //                                        gradeds.Clear();
    //                                        gradeds = da.select_method_wo_parameter(gradesqlclass, "Text");
    //                                        if (gradeds.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (clasipg == false)
    //                                            {
    //                                                tblcgpaclass.Cell(4, 4).SetContent(Convert.ToString(gradeds.Tables[0].Rows[0]["grade"]));
    //                                                tblcgpaclass.Cell(4, 5).SetContent(gradeds.Tables[0].Rows[0]["classification"].ToString());
    //                                            }
    //                                            else
    //                                            {
    //                                                tblcgpaclass.Cell(2, 4).SetContent(Convert.ToString(gradeds.Tables[0].Rows[0]["grade"]));
    //                                                tblcgpaclass.Cell(2, 5).SetContent(gradeds.Tables[0].Rows[0]["classification"].ToString());
    //                                            }
    //                                        }
    //                                        if (clasipg == false)
    //                                        {
    //                                            tblcgpaclass.Cell(4, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            tblcgpaclass.Cell(4, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                        }
    //                                        else
    //                                        {
    //                                            tblcgpaclass.Cell(2, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            tblcgpaclass.Cell(2, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                        }
    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    if (clasipg == false)
    //                                    {
    //                                        tblcgpaclass.Cell(4, 1).SetContent(creditstotal);
    //                                        tblcgpaclass.Cell(4, 3).SetContent(sumpart);
    //                                        tblcgpaclass.Cell(4, 2).SetContent(wpm);
    //                                    }
    //                                    else
    //                                    {
    //                                        tblcgpaclass.Cell(2, 1).SetContent(creditstotal);
    //                                        tblcgpaclass.Cell(2, 3).SetContent(sumpart);
    //                                        tblcgpaclass.Cell(2, 2).SetContent(wpm);
    //                                    }
    //                                }
    //                            }
    //                            else
    //                            {
    //                                if (clasipg == false)
    //                                {
    //                                    tblcgpaclass.Cell(4, 1).SetContent("---");
    //                                    tblcgpaclass.Cell(4, 3).SetContent("---");
    //                                    tblcgpaclass.Cell(4, 2).SetContent("---");
    //                                }
    //                                else
    //                                {
    //                                    tblcgpaclass.Cell(2, 1).SetContent("---");
    //                                    tblcgpaclass.Cell(2, 3).SetContent("---");
    //                                    tblcgpaclass.Cell(2, 2).SetContent("---");
    //                                }
    //                            }
    //                            partsums = 0;
    //                            partrowcount = 0;
    //                            creditstotal = 0;
    //                            partwpmsum = 0;
    //                            overalltotgrade = 0;
    //                            if (printds_rows.Tables[0].Rows.Count > 0)
    //                            {
    //                                string sumpart =string.Empty;
    //                                string wpm =string.Empty;
    //                                DataView dv_demand_data = new DataView();
    //                                DataView dv_demand_datadummy = new DataView();
    //                                int part = 4;
    //                                if (clasipg == false)
    //                                {
    //                                    part = 4;
    //                                }
    //                                else
    //                                {
    //                                    part = 2;
    //                                }

    //                                printds_rows.Tables[0].DefaultView.RowFilter = "Part_Type='" + part + "'";
    //                                dv_demand_datadummy = printds_rows.Tables[0].DefaultView;
    //                                partrowcount = dv_demand_datadummy.Count;
    //                                printds_rows.Tables[0].DefaultView.RowFilter = "Part_Type='" + part + "' and result='pass'";
    //                                dtPart1 = printds_rows.Tables[0].DefaultView.ToTable();
    //                                if (clasipg == false)
    //                                {
    //                                    tblcgpaclass.Cell(5, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    tblcgpaclass.Cell(5, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    tblcgpaclass.Cell(5, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    tblcgpaclass.Cell(5, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    tblcgpaclass.Cell(5, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                }
    //                                else
    //                                {
    //                                    tblcgpaclass.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    tblcgpaclass.Cell(3, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    tblcgpaclass.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    tblcgpaclass.Cell(3, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    tblcgpaclass.Cell(3, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                }
    //                                if (dtPart1.Rows.Count > 0 && partrowcount == dtPart1.Rows.Count)
    //                                {
    //                                    for (int sum = 0; sum < dtPart1.Rows.Count; sum++)
    //                                    {
    //                                        //double checkmarkmm = Convert.ToDouble(dtPart1.Rows[sum]["total"].ToString());
    //                                        double checkmarkmm = 0;
    //                                        double.TryParse(Convert.ToString(dtPart1.Rows[sum]["total"]), out checkmarkmm);
    //                                        checkmarkmm = Math.Round(checkmarkmm, 0, MidpointRounding.AwayFromZero);

    //                                        string gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='" + sem + "' and Degree_Code='" + grade_set + "' and batch_year='" + grade_set + "' and '" + checkmarkmm + "' between frange and trange";//added by sridhar 16/aug 2014
    //                                        gradeds.Clear();
    //                                        gradeds = da.select_method_wo_parameter(gradesql, "Text");
    //                                        if (gradeds.Tables[0].Rows.Count == 0)
    //                                        {
    //                                            gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "' and Degree_Code='" + grade_set + "' and batch_year='" + grade_set + "'  and '" + checkmarkmm + "' between frange and trange";
    //                                            gradeds.Clear();
    //                                            gradeds = da.select_method_wo_parameter(gradesql, "Text");
    //                                        }
    //                                        for (int grd = 0; grd < gradeds.Tables[0].Rows.Count; grd++)
    //                                        {
    //                                            if (Convert.ToInt32(gradeds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(gradeds.Tables[0].Rows[grd][2]) >= checkmarkmm)
    //                                            {
    //                                                double.TryParse(Convert.ToString(dtPart1.Rows[sum]["total"]), out grade_points);
    //                                                double.TryParse(Convert.ToString(dtPart1.Rows[sum]["total"]), out Marks);
    //                                                grade_points = grade_points / 10;
    //                                                //Credit_Points = Convert.ToDouble(dtPart1.Rows[sum]["credit_points"].ToString());
    //                                                double.TryParse(Convert.ToString(dtPart1.Rows[sum]["credit_points"]), out Credit_Points);
    //                                                creditstotal = creditstotal + Credit_Points;
    //                                                partwpmsum += (Credit_Points * Marks);
    //                                                partsums = partsums + (grade_points * Credit_Points);
    //                                            }

    //                                        }
    //                                    }

    //                                    if (creditstotal == 0)
    //                                    {
    //                                        sumpart = "0.000";
    //                                        wpm = "0.00";
    //                                    }
    //                                    else if (creditstotal > 0)
    //                                    {
    //                                        partsums = (partsums / creditstotal);
    //                                        partsums = Math.Round(partsums, 3, MidpointRounding.AwayFromZero);
    //                                        partwpmsum = (partwpmsum / creditstotal);
    //                                        partwpmsum = Math.Round(partwpmsum, 2, MidpointRounding.AwayFromZero);
    //                                        sumpart = String.Format("{0:0.000}", partsums);
    //                                        wpm = string.Format("{0:0.00}", partwpmsum);
    //                                    }
    //                                    else
    //                                    {
    //                                        sumpart = "0.000";
    //                                        wpm = "0.00";
    //                                    }
    //                                }
    //                                else if (partrowcount > 0)
    //                                {
    //                                    sumpart = "0.000";
    //                                    wpm = "0.00";
    //                                }
    //                                else
    //                                {
    //                                    sumpart = "---";
    //                                    wpm = "---";
    //                                }
    //                                if (sumpart != "---")
    //                                {
    //                                    double sumpartgrade = 0;
    //                                    if (double.TryParse(sumpart, out sumpartgrade))
    //                                    {
    //                                        sumpartgrade = Convert.ToDouble(sumpart);
    //                                        // overalltotgrade = overalltotgrade + sumpartgrade;
    //                                        overalltotgrade = overalltotgrade + sumpartgrade;
    //                                    }
    //                                    else
    //                                    {
    //                                        sumpartgrade = 0;
    //                                    }
    //                                    if (clasipg == false)
    //                                    {
    //                                        tblcgpaclass.Cell(5, 1).SetContent(creditstotal);
    //                                        tblcgpaclass.Cell(5, 3).SetContent(sumpart);
    //                                        tblcgpaclass.Cell(5, 2).SetContent(wpm);
    //                                    }
    //                                    else
    //                                    {
    //                                        tblcgpaclass.Cell(3, 1).SetContent(creditstotal + splcredit);
    //                                        tblcgpaclass.Cell(3, 3).SetContent("---");
    //                                        tblcgpaclass.Cell(3, 2).SetContent("---");
    //                                    }
    //                                    batchsetting = "1";
    //                                    if (noofsubfailed != 0 && overalltotgrade >= 6)
    //                                    {
    //                                        string gradesqlclass = "select * from coe_classification where edu_level='" + edu_level + "'  and  '" + overalltotgrade + "'>= frompoint and '" + overalltotgrade + "'<= topoint and  markgradeflag='" + batchsetting + "'";//added by sridhar 16/aug 2014 and (classification='First Class' or classification='First')
    //                                        gradeds.Clear();
    //                                        gradeds = da.select_method_wo_parameter(gradesqlclass, "Text");
    //                                        string cclass = "First Class";
    //                                        if (gradeds.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (clasipg == false)
    //                                            {
    //                                                tblcgpaclass.Cell(5, 4).SetContent(Convert.ToString(gradeds.Tables[0].Rows[0]["grade"]));
    //                                                tblcgpaclass.Cell(5, 5).SetContent(cclass);
    //                                            }
    //                                            else
    //                                            {
    //                                                tblcgpaclass.Cell(3, 4).SetContent("---");//Convert.ToString(gradeds.Tables[0].Rows[0]["grade"]));
    //                                                tblcgpaclass.Cell(3, 5).SetContent("---");//cclass);
    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            if (clasipg == false)
    //                                            {
    //                                                tblcgpaclass.Cell(5, 4).SetContent(Convert.ToString("A"));
    //                                                tblcgpaclass.Cell(5, 5).SetContent(cclass);
    //                                            }
    //                                            else
    //                                            {
    //                                                tblcgpaclass.Cell(3, 4).SetContent("---");//Convert.ToString("A"));
    //                                                tblcgpaclass.Cell(3, 5).SetContent("---");//cclass);
    //                                            }
    //                                        }
    //                                        if (clasipg == false)
    //                                        {
    //                                            tblcgpaclass.Cell(5, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            tblcgpaclass.Cell(5, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                        }
    //                                        else
    //                                        {
    //                                            tblcgpaclass.Cell(3, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            tblcgpaclass.Cell(3, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        string gradesqlclass = "select * from coe_classification where edu_level='" + edu_level + "'  and  '" + overalltotgrade + "'>= frompoint and '" + overalltotgrade + "'<= topoint and  markgradeflag='" + batchsetting + "'";//added by sridhar 16/aug 2014
    //                                        gradeds.Clear();
    //                                        gradeds = da.select_method_wo_parameter(gradesqlclass, "Text");
    //                                        if (gradeds.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (clasipg == false)
    //                                            {
    //                                                tblcgpaclass.Cell(5, 4).SetContent(Convert.ToString(gradeds.Tables[0].Rows[0]["grade"]));
    //                                                tblcgpaclass.Cell(5, 5).SetContent(gradeds.Tables[0].Rows[0]["classification"].ToString());
    //                                            }
    //                                            else
    //                                            {
    //                                                tblcgpaclass.Cell(3, 4).SetContent("---");//Convert.ToString(gradeds.Tables[0].Rows[0]["grade"]));
    //                                                tblcgpaclass.Cell(3, 5).SetContent("---");//gradeds.Tables[0].Rows[0]["classification"].ToString());
    //                                            }
    //                                        }
    //                                        if (clasipg == false)
    //                                        {
    //                                            tblcgpaclass.Cell(5, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            tblcgpaclass.Cell(5, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                        }
    //                                        else
    //                                        {
    //                                            tblcgpaclass.Cell(3, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            tblcgpaclass.Cell(3, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                        }
    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    if (clasipg == false)
    //                                    {
    //                                        tblcgpaclass.Cell(5, 1).SetContent(creditstotal);
    //                                        tblcgpaclass.Cell(5, 3).SetContent(sumpart);
    //                                        tblcgpaclass.Cell(5, 2).SetContent(wpm);
    //                                    }
    //                                    else
    //                                    {
    //                                        tblcgpaclass.Cell(3, 1).SetContent(creditstotal + splcredit);
    //                                        tblcgpaclass.Cell(3, 3).SetContent("---");
    //                                        tblcgpaclass.Cell(3, 2).SetContent("---");

    //                                    }
    //                                }
    //                            }
    //                            else
    //                            {
    //                                if (clasipg == false)
    //                                {
    //                                    tblcgpaclass.Cell(5, 1).SetContent("---");
    //                                    tblcgpaclass.Cell(5, 3).SetContent("---");
    //                                    tblcgpaclass.Cell(5, 2).SetContent("---");
    //                                }
    //                                else
    //                                {
    //                                    tblcgpaclass.Cell(3, 1).SetContent("---");
    //                                    tblcgpaclass.Cell(3, 3).SetContent("---");
    //                                    tblcgpaclass.Cell(3, 2).SetContent("---");
    //                                }
    //                            }
    //                        }
    //                        PdfTable tblPass = mydoc.NewTable(fontcgpa, 3, 2, 3);
    //                        tblPass.VisibleHeaders = false;
    //                        tblPass.SetBorders(Color.Black, 1, BorderType.None);
    //                        //255,100,106,156,88
    //                        tblPass.SetColumnsWidth(new int[] { 255, 100 });
    //                        if (clasipg == false)
    //                        {
    //                            tblPass.Cell(0, 1).SetContent(setng_mintotalcreadits);
    //                            tblPass.Cell(1, 1).SetContent(overallcreditearned + splcredit);
    //                            tblPass.Cell(2, 1).SetContent("PASS");

    //                            tblPass.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            tblPass.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            tblPass.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

    //                            tblPass.Cell(0, 1).SetCellPadding(10);
    //                            tblPass.Cell(1, 1).SetCellPadding(10);
    //                            tblPass.Cell(2, 1).SetCellPadding(10);
    //                        }
    //                        else
    //                        {
    //                            if (degree.Trim().ToLower() == "mca" || degree.Trim().ToLower().Trim('.') == "m.c.a")
    //                            {
    //                                tblPass.Cell(0, 1).SetContent(setng_mintotalcreadits);

    //                            }
    //                            else
    //                            {
    //                                tblPass.Cell(0, 1).SetContent(setng_mintotalcreadits);
    //                            }
    //                            tblPass.Cell(0, 1).SetCellPadding(10);
    //                            tblPass.Cell(1, 1).SetCellPadding(10);
    //                            tblPass.Cell(2, 1).SetCellPadding(10);


    //                            tblPass.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            tblPass.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            tblPass.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

    //                            tblPass.Cell(1, 1).SetContent(overallcreditearned + splcredit);

    //                            tblPass.Cell(2, 1).SetContent("PASS");
    //                        }
    //                        newpdftabpage2 = tblPass.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 482, 815, 250, 250));
    //                        mypdfpage.Add(newpdftabpage2);
    //                        // x=13,y=70.8,W= ,H= 
    //                        newpdftabpage2 = tblcgpaclass.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 0, 815, 730, 150));
    //                        mypdfpage.Add(newpdftabpage2);
    //                    }

    //                    pdfdoi = new PdfTextArea(fontDate, System.Drawing.Color.Black, new PdfArea(mydoc, 7, 990, 71, 25), System.Drawing.ContentAlignment.TopLeft, txtdoi.Text.ToString());
    //                    mypdfpage.Add(pdfdoi);

    //                    mypdfpage.SaveToDocument();
    //                }

    //                else
    //                {
    //                    if (lblerror1 == "")
    //                    {
    //                        lblerror1 = rollnosub;
    //                    }
    //                    else
    //                    {
    //                        lblerror1 = lblerror1 + "," + rollnosub;
    //                    }
    //                }
    //            }
    //        }
    //        if (lblerror1 != "")
    //        {
    //            lblerror.Text = lblerror1;
    //            lblerror.Visible = true;
    //        }
    //        else
    //        {
    //            lblerror.Text =string.Empty;
    //            lblerror.Visible = false;
    //        }
    //        if (printpage == true)
    //        {
    //            string appPath = HttpContext.Current.Server.MapPath("~");
    //            if (appPath != "")
    //            {
    //                string szPath = appPath + "/Report/";
    //                string szFile = "consolidatedmarksheet" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHmmss") + ".pdf";
    //                mydoc.SaveToFile(szPath + szFile);
    //                Response.ClearHeaders();
    //                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
    //                Response.ContentType = "application/pdf";
    //                Response.WriteFile(szPath + szFile);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

    #endregion

    #region DropDown Events

    protected void ddlEdulevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            rptprint1.Visible = false;
            divSpread.Visible = false;
            lblErrSearch.Visible = false;
            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            bindexamyear();
            loadmonth();
        }
        catch (Exception ex)
        {

        }
    }

    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            rptprint1.Visible = false;
            divSpread.Visible = false;
            lblErrSearch.Visible = false;
            if (chkdegree.Checked == true)
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                }
                txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = false;
                }
                txtdegree.Text = "--Select--";
                txtbranch.Text = "--Select--";
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    protected void chklstdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            rptprint1.Visible = false;
            divSpread.Visible = false;
            lblErrSearch.Visible = false;
            int commcount = 0;
            chkdegree.Checked = false;
            txtdegree.Text = "--Select--";
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtdegree.Text = "Degree(" + commcount.ToString() + ")";
                if (commcount == chklstdegree.Items.Count)
                {
                    chkdegree.Checked = true;
                }
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            rptprint1.Visible = false;
            divSpread.Visible = false;
            lblErrSearch.Visible = false;
            if (chkbranch.Checked == true)
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                }
                txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = false;
                }
                chkbranch.Checked = false;
                txtbranch.Text = "--Select--";
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            rptprint1.Visible = false;
            divSpread.Visible = false;
            lblErrSearch.Visible = false;
            string clg = string.Empty;
            int commcount = 0;
            txtbranch.Text = "--Select--";
            chkbranch.Checked = false;
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtbranch.Text = "Branch(" + commcount.ToString() + ")";
                if (commcount == chklstbranch.Items.Count)
                {
                    chkbranch.Checked = true;
                }
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    protected void chkbatch_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            rptprint1.Visible = false;
            divSpread.Visible = false;
            lblErrSearch.Visible = false;
            if (chkbatch.Checked == true)
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                    txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = false;
                    txtbatch.Text = "---Select---";
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }
    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            rptprint1.Visible = false;
            divSpread.Visible = false;
            lblErrSearch.Visible = false;
            int i = 0;
            txtbatch.Text = "--Select--";
            chkbatch.Checked = false;
            string clg = string.Empty;
            int commcount = 0;
            for (i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtbatch.Text = "Batch(" + commcount.ToString() + ")";
                if (commcount == chklsbatch.Items.Count)
                {
                    chkbatch.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlExamMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            rptprint1.Visible = false;
            divSpread.Visible = false;
            lblErrSearch.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlExamyr_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            rptprint1.Visible = false;
            divSpread.Visible = false;
            lblErrSearch.Visible = false;

            string strsql = "select distinct Exam_Month  from Exam_Details  where exam_year='" + ddlExamyr.SelectedItem.Value.ToString() + "' and Exam_Month<>0 ";
            ds = d2.select_method_wo_parameter(strsql, "Text");
            ddlExamMonth.Items.Clear();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int month = Convert.ToInt32(ds.Tables[0].Rows[i][0].ToString());
                if (month == 1)
                {
                    ddlExamMonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                }
                if (month == 2)
                {
                    ddlExamMonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                }
                if (month == 3)
                {
                    ddlExamMonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                }
                if (month == 4)
                {
                    ddlExamMonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                }
                if (month == 5)
                {
                    ddlExamMonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("May", "5"));
                }
                if (month == 6)
                {
                    ddlExamMonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                }
                if (month == 7)
                {
                    ddlExamMonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                }
                if (month == 8)
                {
                    ddlExamMonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                }
                if (month == 9)
                {
                    ddlExamMonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                }
                if (month == 10)
                {
                    ddlExamMonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                }
                if (month == 11)
                {
                    ddlExamMonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                }
                if (month == 12)
                {
                    ddlExamMonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Dec", "12"));
                }

            }
            ddlExamMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    #endregion DropDown Events

    #region Button Events

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            batch_year = string.Empty;
            degree_code = string.Empty;
            exam_code = string.Empty;
            edulevel = string.Empty;
            exam_month = string.Empty;
            exam_yr = string.Empty;
            collegecode = string.Empty;
            int maxSem = 0;
            string max_sem = string.Empty;
            rptprint1.Visible = false;
            divSpread.Visible = false;
            lblErrSearch.Visible = false;

            DataSet dsClassify = new DataSet();
            DataSet dsArrearList = new DataSet();
            ArrayList arrearList = new ArrayList();
            ArrayList distinctarrearlist = new ArrayList();

            Hashtable hatclasifycolumn = new Hashtable();
            Hashtable hatclasifycount = new Hashtable();
            Dictionary<string, int> dicclassifycount = new Dictionary<string, int>();
            if (Session["collegecode"] != null)
            {
                collegecode = Convert.ToString(Session["collegecode"]);
            }
            int batch_count = 0, degreecount = 0;
            if (ddlEdulevel.Items.Count > 0)
            {
                edulevel = Convert.ToString(ddlEdulevel.SelectedItem);
            }
            else
            {
                divpopupErr.Visible = true;
                lblpopuperr.Text = "No Education Level Found";
                return;
            }
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    batch_count++;
                    if (batch_year == "")
                    {
                        batch_year = chklsbatch.Items[i].Text;
                    }
                    else
                    {
                        batch_year += ",'" + chklsbatch.Items[i].Text + "'";
                    }
                }
            }
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    degreecount++;
                    if (degree_code == "")
                    {
                        degree_code = chklstbranch.Items[i].Value;
                    }
                    else
                    {
                        degree_code += ",'" + chklstbranch.Items[i].Value + "'";
                    }
                }
            }
            if (batch_count == 0)
            {
                divSpread.Visible = false;
                rptprint1.Visible = false;
                FpSpread1.Visible = false;
                divpopupErr.Visible = true;
                lblpopuperr.Text = "Please Select Any One Batch";
                return;
            }
            if (degreecount == 0)
            {
                divSpread.Visible = false;
                rptprint1.Visible = false;
                FpSpread1.Visible = false;
                divpopupErr.Visible = true;
                lblpopuperr.Text = "Please Select Any One Branch";
                return;
            }
            if (ddlExamyr.Items.Count > 0)
            {
                if (ddlExamyr.SelectedValue == "0")
                {
                    divSpread.Visible = false;
                    rptprint1.Visible = false;
                    FpSpread1.Visible = false;
                    divpopupErr.Visible = true;
                    lblpopuperr.Text = "Please Select Exam Year";
                    return;
                }
                else
                {
                    exam_yr = Convert.ToString(ddlExamyr.SelectedValue);
                }
            }
            else
            {
                divSpread.Visible = false;
                rptprint1.Visible = false;
                FpSpread1.Visible = false;
                divpopupErr.Visible = true;
                lblpopuperr.Text = "No Exam Year Found";
                return;
            }
            if (ddlExamMonth.Items.Count > 0)
            {
                if (ddlExamMonth.SelectedValue == "0")
                {
                    divSpread.Visible = false;
                    rptprint1.Visible = false;
                    FpSpread1.Visible = false;
                    divpopupErr.Visible = true;
                    lblpopuperr.Text = "Please Select Exam Month";
                    return;
                }
                else
                {
                    exam_month = Convert.ToString(ddlExamMonth.SelectedValue);
                }
            }
            else
            {
                divSpread.Visible = false;
                rptprint1.Visible = false;
                FpSpread1.Visible = false;
                divpopupErr.Visible = true;
                lblpopuperr.Text = "No Exam Month Found";
                return;
            }
            if (exam_month != "" && exam_month != null && exam_yr != "" && exam_yr != null)
            {
                Init_Spread();
                /*
                 * Table 0 Classification 
                 * Table 1 Total Degree and Branch
                 * Table 2 Appeared
                 * */
                //qry = "select classification,MAX(frompoint) from coe_classification group by classification  order by MAX(frompoint) desc ; select distinct c.Course_Name,dt.Dept_Name,dg.Degree_Code from Course c, Degree dg,Department dt where dg.Course_Id=c.Course_Id and dt.Dept_Code=dg.Dept_Code and c.college_code=dt.college_code and dt.college_code=dg.college_code and dg.college_code='" + collegecode + "' and Edu_Level='" + edulevel + "' and dg.Degree_Code in(" + degree_code + ") order by c.Course_Name,dt.Dept_Name ; select distinct r.Batch_Year,r.degree_code, m.roll_no,r.mode from Exam_Details ed,mark_entry m,Registration r where ed.exam_code=m.exam_code and m.roll_no=r.Roll_No and ed.Exam_year='" + exam_yr + "' and ed.Exam_Month='" + exam_month + "' and college_code='" + collegecode + "' and r.degree_code in (" + degree_code + ") and  r.Batch_Year in (" + batch_year + ");";

                qry = "select classification,MAX(frompoint) from coe_classification group by classification having MAX(frompoint)>0   order by MAX(frompoint) desc ; select distinct c.Course_Name,dt.Dept_Name,dg.Degree_Code from Course c, Degree dg,Department dt where dg.Course_Id=c.Course_Id and dt.Dept_Code=dg.Dept_Code and c.college_code=dt.college_code and dt.college_code=dg.college_code and dg.college_code='" + collegecode + "' and Edu_Level='" + edulevel + "' and dg.Degree_Code in(" + degree_code + ") order by c.Course_Name,dt.Dept_Name ; select distinct r.Batch_Year,r.degree_code, m.roll_no,r.mode,r.Current_Semester from Exam_Details ed,mark_entry m,Registration r where ed.exam_code=m.exam_code and m.roll_no=r.Roll_No  and college_code='" + collegecode + "' and r.degree_code in (" + degree_code + ") and  r.Batch_Year in (" + batch_year + ") and Exam_Month='" + exam_month + "' and Exam_year='" + exam_yr + "' and m.internal_mark>=0 and m.external_mark>=0; select distinct r.Batch_Year,r.degree_code, m.roll_no,r.mode,r.Current_Semester from Exam_Details ed,mark_entry m,Registration r where ed.exam_code=m.exam_code and m.roll_no=r.Roll_No  and college_code='" + collegecode + "' and r.degree_code in (" + degree_code + ") and  r.Batch_Year in (" + batch_year + ");";
                ds = d2.select_method_wo_parameter(qry, "Text");

                qry = "select frompoint,topoint,classification from coe_classification where collegecode='" + collegecode + "' and edu_level='" + edulevel + "'";
                dsClassify = d2.select_method_wo_parameter(qry, "Text");

                //qry = "Select count(roll_no) arr,roll_no  from (select distinct m.subject_no,m.roll_no,r.degree_code,r.Batch_Year,m.exam_code from mark_entry m,Registration r,Exam_Details ed where r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and ed.exam_code=m.exam_code and m.roll_no=r.Roll_No and r.delflag=0  and r.cc=0 and r.exam_flag<>'debar' and r.Batch_Year in(" + batch_year + ") and r.degree_code in(" + degree_code + ") and ed.Exam_Month='" + exam_month + "' and ed.Exam_year='" + exam_yr + "'  and subject_no not in(select subject_no from mark_entry m1,Exam_Details ed1  where ed1.exam_code=m1.exam_code   and result='pass' and passorfail=1 and r.Batch_Year in(" + batch_year + ") and ed1.degree_code in(" + degree_code + ") and ed1.Exam_Month='" + exam_month + "' and ed1.Exam_year='" + exam_yr + "' and r.Roll_No=m1.roll_no))as my_table group by roll_no order by arr";//and r.Batch_Year=ed1.batch_year and r.degree_code=ed1.degree_code

                //qry = "Select count(roll_no) arr,roll_no  from (select distinct m.subject_no,m.roll_no,r.degree_code,r.Batch_Year,m.exam_code from mark_entry m,Registration r,Exam_Details ed where ed.exam_code=m.exam_code and m.roll_no=r.Roll_No and r.delflag=0  and r.exam_flag<>'debar' and r.Batch_Year in(" + batch_year + ") and r.degree_code in(" + degree_code + ") and subject_no not in(select subject_no from mark_entry m1,Exam_Details ed1  where ed1.exam_code=m1.exam_code   and result='pass' and passorfail=1 and r.Batch_Year in(" + batch_year + ") and ed1.degree_code in(" + degree_code + ") and r.Roll_No=m1.roll_no))as my_table group by roll_no order by arr ; Select distinct m.roll_no from mark_entry m,Registration r where m.roll_no = r.Roll_No and subject_no in (select subject_no from mark_entry m1 where m1.roll_no = m.roll_no  and result='fail' and  passorfail=0) and degree_code in(" + degree_code + ") and Batch_Year in(" + batch_year + ") group by m.roll_no ";
                qry = "select m.roll_no,r.Reg_No,count(distinct m.subject_no) as arrearcount,r.Batch_Year from mark_entry m,Registration r,subject s where m.roll_no=r.Roll_No and s.subject_no=m.subject_no and r.degree_code in(" + degree_code + ") and r.Batch_Year in(" + batch_year + ") and  s.subject_no not in(select m1.subject_no from mark_entry m1 where m.roll_no=m1.roll_no and m.subject_no=m1.subject_no and m1.result='pass') group by m.roll_no,r.Batch_Year,r.degree_code, m.roll_no ,r.Reg_No order by arrearcount,r.Batch_Year,r.degree_code, m.roll_no; Select distinct m.roll_no from mark_entry m,Registration r where m.roll_no = r.Roll_No and subject_no in (select subject_no from mark_entry m1 where m1.roll_no = m.roll_no  and result='fail' and  passorfail=0) and degree_code in(" + degree_code + ") and Batch_Year in(" + batch_year + ") group by m.roll_no ";
                dsArrearList = d2.select_method_wo_parameter(qry, "text");
                if (dsArrearList.Tables.Count > 0 && dsArrearList.Tables[0].Rows.Count > 0)
                {
                    arrearList.Clear();
                    for (int i = 0; i < dsArrearList.Tables[0].Rows.Count; i++)
                    {
                        if (!arrearList.Contains(Convert.ToString(dsArrearList.Tables[0].Rows[i]["roll_no"])))
                        {
                            arrearList.Add(Convert.ToString(dsArrearList.Tables[0].Rows[i]["roll_no"]));
                        }
                    }
                    if (dsArrearList.Tables[1].Rows.Count > 0)
                    {
                        distinctarrearlist.Clear();
                        for (int i = 0; i < dsArrearList.Tables[1].Rows.Count; i++)
                        {
                            if (!distinctarrearlist.Contains(Convert.ToString(dsArrearList.Tables[1].Rows[i]["roll_no"])))
                            {
                                distinctarrearlist.Add(Convert.ToString(dsArrearList.Tables[1].Rows[i]["roll_no"]));
                            }
                        }
                    }
                }
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        hatclasifycount.Clear();
                        dicclassifycount.Clear();
                        for (int col = 0; col < ds.Tables[0].Rows.Count; col++)
                        {
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[0].Rows[col][0]);
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = Convert.ToString(ds.Tables[0].Rows[col][0]).Length * 10;
                            hatclasifycount.Add(Convert.ToString(ds.Tables[0].Rows[col][0]), 0);
                            dicclassifycount.Add(Convert.ToString(ds.Tables[0].Rows[col][0]), 0);
                            hatclasifycolumn.Add(Convert.ToString(ds.Tables[0].Rows[col][0]), 0);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[0].Rows[col][1]);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Locked = true;
                        }
                        FpSpread1.Sheets[0].ColumnCount += 2;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].Text = "Passed Count";
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = Convert.ToString("Passed Count").Length * 10;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].Locked = true;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Pass Percentage %";
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = Convert.ToString("Pass Percentage %").Length * 10;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Locked = true;
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            string[] batch = batch_year.Split(',');
                            int slno = 0;
                            for (int i = 0; i < batch.Length; i++)
                            {
                                string cellbatch = batch[i].Replace("'", "").Trim();
                                for (int rows = 0; rows < ds.Tables[1].Rows.Count; rows++)
                                {
                                    //foreach (DictionaryEntry ht in hatclasifycolumn)
                                    //{
                                    //    string k = Convert.ToString(ht.Key);
                                    //    hatclasifycolumn[k] = 0;
                                    //}
                                    //foreach (DictionaryEntry ht in hatclasifycount)
                                    //{
                                    //    string k = Convert.ToString(ht.Key);
                                    //    hatclasifycount[k] = 0;
                                    //}
                                    HashValueToZero(hatclasifycolumn);
                                    HashValueToZero(hatclasifycount);
                                    string deg = Convert.ToString(ds.Tables[1].Rows[rows]["Degree_Code"]);

                                    max_sem = d2.GetFunctionv("select NDurations from ndegree where batch_year='" + cellbatch + "'  and Degree_code='" + deg + "' and college_code='" + Convert.ToString(Session["collegecode"]) + "'");
                                    if (max_sem == "" || max_sem == null)
                                    {
                                        max_sem = d2.GetFunctionv("SELECT Duration FROM Degree where  Degree_Code='" + deg + "' and college_code='" + Convert.ToString(Session["collegecode"]) + "'");
                                    }
                                    int.TryParse(max_sem, out maxSem);
                                    int newbatch = 0;
                                    int.TryParse(cellbatch, out newbatch);
                                    DateTime dt = new DateTime();
                                    int cur_year = DateTime.Now.Year;
                                    int diff = newbatch + (maxSem / 2);
                                    if (cur_year >= diff)
                                    {
                                        maxSem = maxSem + 1;
                                        FpSpread1.Sheets[0].RowCount++;
                                        slno++;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(slno);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Locked = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(batch[i].Replace("'", "").Trim());
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Locked = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[1].Rows[rows]["Course_Name"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Locked = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[1].Rows[rows]["Dept_Name"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[1].Rows[rows]["Degree_Code"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Locked = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                        DataView dvAppeared = new DataView();
                                        if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                                        {
                                            string stubatch = Convert.ToString(batch[i].Replace("'", "").Trim());
                                            ds.Tables[2].DefaultView.RowFilter = "Batch_Year='" + Convert.ToString(batch[i].Replace("'", "").Trim()) + "' and degree_code='" + Convert.ToString(ds.Tables[1].Rows[rows]["Degree_Code"]) + "'";
                                            dvAppeared = ds.Tables[2].DefaultView;
                                            DataTable dtall = new DataTable();

                                            string roll_no = string.Empty;
                                            if (dvAppeared.Count > 0)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dvAppeared.Count);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                                                dtall = dvAppeared.ToTable();
                                                ugconsolidatedGrade_Mark(dtall, ref hatclasifycount, hatclasifycolumn);
                                                //for (int stu = 0; stu < dvAppeared.Count; stu++)
                                                //{
                                                //    roll_no = Convert.ToString(dvAppeared[stu]["roll_no"]);//roll_no
                                                //    string deg_code = Convert.ToString(dvAppeared[stu]["Degree_Code"]);
                                                //    string mode = Convert.ToString(dvAppeared[stu]["mode"]);

                                                //    string cur_arrqry = "select Count(*)  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and sc.roll_no='" + roll_no + "'  and s.subject_no not in (select m.subject_no from mark_entry m where m.roll_no='" + roll_no + "' and m.result='Pass')";
                                                //    string arrear_count = d2.GetFunction(cur_arrqry);
                                                //    int failcount = 0;
                                                //    int.TryParse(arrear_count, out failcount);
                                                //    //failcount = Convert.ToInt32(d2.GetFunction(" Select COUNT(*) from Mark_Entry,Subject where  Mark_Entry.Subject_No = Subject.Subject_No  and roll_no='" + roll_no + "' and result<>'pass' and Subject.subject_no not in(select m.subject_no from mark_entry m where roll_no='" + roll_no + "' and m.result='Pass')"));
                                                //    if (failcount==0)
                                                //    {
                                                //        string gpa =string.Empty;
                                                //        //Calulat_GPA_Semwise(roll_no, deg_code, stubatch, exam_month, exam_yr, collegecode);
                                                //        Calculete_CGPA(roll_no, Convert.ToString(maxSem), deg_code, stubatch, mode, collegecode);
                                                //        gpa = calculate;
                                                //        double gpa1 = 0;
                                                //        double.TryParse(calculate, out gpa1);
                                                //        //gpa1 = Math.Round(gpa1, 1, MidpointRounding.AwayFromZero);
                                                //        DataView dvClassify = new DataView();
                                                //        if (dsClassify.Tables.Count > 0 && dsClassify.Tables[0].Rows.Count > 0)
                                                //        {
                                                //            //"frompoint='" + gpa + "' and topoint='" + gpa + "'";
                                                //            //"'"+gpa+"'" between frompoint and topoint";
                                                //            dsClassify.Tables[0].DefaultView.RowFilter = "frompoint<='" + gpa1 + "' and topoint>='" + gpa1 + "'";
                                                //            dvClassify = dsClassify.Tables[0].DefaultView;
                                                //            if (dvClassify.Count > 0)
                                                //            {
                                                //                string classify = Convert.ToString(dvClassify[0]["classification"]);
                                                //                if (classify != "")
                                                //                {

                                                //                    if (classify == "First Class - Exemplary" || classify == "First Class with Distinction")
                                                //                    {
                                                //                        string qryno_arr = "select Count(*) from mark_entry m,subject s where m.subject_no=s.subject_no and  roll_no='"+roll_no+"' and result='Fail'";
                                                //                        string woarrear = d2.GetFunction(qryno_arr);
                                                //                        int historyarrear = 0;
                                                //                        int.TryParse(woarrear, out historyarrear);
                                                //                        if (historyarrear!=0)
                                                //                        {
                                                //                            classify = "First Class";
                                                //                        }
                                                //                    }
                                                //                    double tempcount = 0;
                                                //                    if (hatclasifycount.Contains(classify))
                                                //                    {
                                                //                        double.TryParse(Convert.ToString(hatclasifycount[classify]), out tempcount);
                                                //                        hatclasifycount[classify] = tempcount + 1;
                                                //                    }
                                                //                }
                                                //            }
                                                //        }
                                                //        else
                                                //        {
                                                //            divSpread.Visible = false;
                                                //            rptprint1.Visible = false;
                                                //            FpSpread1.Visible = false;
                                                //            divpopupErr.Visible = true;
                                                //            lblpopuperr.Text = "Not Found Classification. Please Set Classification First Before Proceed!!!";
                                                //            return;
                                                //        }
                                                //    }
                                                //}
                                                double total_passed = 0;
                                                for (int ss = 5; ss < FpSpread1.Sheets[0].ColumnCount - 2; ss++)
                                                {
                                                    double temppassed = 0;
                                                    double.TryParse(Convert.ToString(hatclasifycount[FpSpread1.Sheets[0].ColumnHeader.Cells[0, ss].Text]), out temppassed);
                                                    if (Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, ss].Tag) != "0")
                                                        total_passed += temppassed;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ss].Text = Convert.ToString(hatclasifycount[FpSpread1.Sheets[0].ColumnHeader.Cells[0, ss].Text]);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ss].Text = Convert.ToString(temppassed);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ss].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ss].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ss].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ss].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ss].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ss].Locked = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ss].VerticalAlign = VerticalAlign.Middle;
                                                }
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(total_passed);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Locked = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;

                                                double avg = 0;
                                                if (total_passed == 0)
                                                {
                                                    avg = 0;
                                                }
                                                else
                                                {
                                                    double tempavg = (total_passed / Convert.ToDouble(dvAppeared.Count)) * 100;
                                                    avg = Math.Round(tempavg, 0, MidpointRounding.AwayFromZero);
                                                }
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(total_passed);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Locked = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(avg);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Locked = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;

                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dvAppeared.Count);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                                                for (int col = 5; col < FpSpread1.Sheets[0].ColumnCount; col++)
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "0";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Locked = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].VerticalAlign = VerticalAlign.Middle;
                                                }
                                            }

                                        }
                                        else
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "0";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = true;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                                            for (int col = 5; col < FpSpread1.Sheets[0].ColumnCount; col++)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "0";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Locked = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].VerticalAlign = VerticalAlign.Middle;
                                            }
                                        }
                                    }
                                    else
                                    {

                                    }
                                }
                            }
                            if (FpSpread1.Sheets[0].RowCount > 0)
                            {
                                divSpread.Visible = true;
                                rptprint1.Visible = true;
                                FpSpread1.Visible = true;
                                FpSpread1.SaveChanges();
                                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                FpSpread1.Height = (FpSpread1.Sheets[0].RowCount * 23) + 70;
                                if (((FpSpread1.Sheets[0].RowCount * 23) + 70) < 250)
                                {
                                    FpSpread1.Height = 250;
                                }
                                if (((FpSpread1.Sheets[0].RowCount * 23) + 70) > 950)
                                {
                                    FpSpread1.Height = 950;
                                }
                                FpSpread1.Width = 1000;
                            }
                            else
                            {
                                divSpread.Visible = false;
                                rptprint1.Visible = false;
                                FpSpread1.Visible = false;
                                divpopupErr.Visible = true;
                                lblpopuperr.Text = "No Record(s) Found";
                                return;
                            }
                        }
                        else
                        {
                            divSpread.Visible = false;
                            rptprint1.Visible = false;
                            FpSpread1.Visible = false;
                            divpopupErr.Visible = true;
                            lblpopuperr.Text = "No Record(s) Found";
                            return;
                        }
                    }
                    else
                    {
                        divSpread.Visible = false;
                        rptprint1.Visible = false;
                        FpSpread1.Visible = false;
                        divpopupErr.Visible = true;
                        lblpopuperr.Text = "Please Set Classification First Then Proceed!!!";
                        return;
                    }

                }
                else
                {
                    divSpread.Visible = false;
                    rptprint1.Visible = false;
                    FpSpread1.Visible = false;
                    divpopupErr.Visible = true;
                    lblpopuperr.Text = "No Record(s) Found";
                    return;
                }
            }
            else
            {
                divSpread.Visible = false;
                rptprint1.Visible = false;
                FpSpread1.Visible = false;
                divpopupErr.Visible = true;
                lblpopuperr.Text = "There is No Exam Year or Exam Month!!!";
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }


    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        divpopupErr.Visible = false;
    }

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol1.Visible = false;
            string reportname = txtexcelname1.Text;
            if (reportname.ToString().Trim() != "")
            {
                if (FpSpread1.Visible == true)
                {
                    d2.printexcelreport(FpSpread1, reportname);
                }

                lbl_norec1.Visible = false;
            }
            else
            {
                lbl_norec1.Text = "Please Enter Your Report Name";
                lbl_norec1.Visible = true;
                txtexcelname1.Focus();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string dptname = "Semester Exam Pass Percentage Report";
            string pagename = "SemesterExamPassPercentageReport.aspx";
            dptname = dptname + "@ " + "Exam Year : " + Convert.ToString(ddlExamyr.SelectedItem) + "@Exam Month : " + Convert.ToString(ddlExamMonth.SelectedItem);
            if (FpSpread1.Visible == true)
            {
                Printcontrol1.loadspreaddetails(FpSpread1, pagename, dptname);
            }
            Printcontrol1.Visible = true;
            lbl_norec1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    public void GetSem(string batchyr, string exmyr, string month, string max_sem, ref string cur_sem)
    {
        int batchyear = 0;
        int.TryParse(batchyr, out batchyear);
        int exmyear = 0;
        int.TryParse(exmyr, out exmyear);
        int mon = 0;
        int.TryParse(month, out mon);
        int maxsem = 0;
        int.TryParse(max_sem, out maxsem);
        int year = 0;
        year = exmyear - batchyear;
        int oddoreven = year % 2;
        int cursem = 1;
        int year1 = maxsem / 2;
        if (year1 == year && mon == 11 && (year > year1 || mon == 4 || mon == 11))
        {
            cur_sem = Convert.ToString(maxsem + 1);
        }
        else
        {
            if (year == 0 && mon == 11)
            {
                cur_sem = "1";
            }
            else if (oddoreven == 1 && mon == 4)
            {
                cursem = year + year;
                cur_sem = Convert.ToString(cursem);
            }
            else if (oddoreven == 1 && mon == 11)
            {
                cursem += year + 1;
                cur_sem = Convert.ToString(cursem);
            }
            else if (oddoreven == 0 && mon == 4)
            {
                cursem = 0;
                cursem += year + 2;
                cur_sem = Convert.ToString(cursem);
            }
            else if (oddoreven == 0 && mon == 11)
            {
                cursem += year + 2;
                cur_sem = Convert.ToString(cursem);
            }
            if (cursem > maxsem)
            {
                cur_sem = Convert.ToString(maxsem + 1);
            }
        }
    }

    public void Calulat_GPA_Semwise(string RollNo, string degree_code, string batch_year, string exam_month, string exam_year, string collegecode)
    {
        string ccva = string.Empty;
        string strgrade = string.Empty;
        double creditval = 0;
        double finalgpa1 = 0;
        double creditsum1 = 0;
        double gpacal1 = 0;
        string strsubcrd = string.Empty;
        string examcodeval = string.Empty;
        double strtot = 0;
        double strgradetempfrm = 0;
        double strgradetempto = 0;
        string strtotgrac = string.Empty;
        string strgradetempgrade = string.Empty;
        string syll_code = string.Empty;
        DataSet dggradetot = new DataSet();

        try
        {
            dggradetot.Dispose();
            daload.Reset();
            string strsqlstaffname = "select distinct frange,trange,credit_points,mark_grade  from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + collegecode + "";
            cmd = new SqlCommand(strsqlstaffname);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(dggradetot);
        }
        catch (SqlException qle)
        {
            throw qle;
        }
        finally
        {
            connection.Close();
        }

        syll_code = d2.GetFunction("select distinct syll_code from exam_details e,syllabus_master s where e.degree_code=s.degree_code and e.batch_year=s.batch_year and e.current_semester=s.semester and e.degree_code='" + degree_code + "' and e.batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year=" + exam_year + "");
        ccva = d2.GetFunction("select cc from registration where roll_no='" + RollNo + "'");
        if (ccva == "False")
        {

            strsubcrd = " Select Subject.credit_points,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and subject.syll_code=" + syll_code.ToString() + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') ";
        }
        else if (ccva == "True")
        {

            strsubcrd = " Select Subject.credit_points,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and subject.syll_code=" + syll_code.ToString() + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') ";
        }
        if (strsubcrd != "" && strsubcrd != null)
        {

            DataSet dssubgrd = d2.select_method_wo_parameter(strsubcrd, "Text");

            for (int s = 0; s < dssubgrd.Tables[0].Rows.Count; s++)
            {
                if ((dssubgrd.Tables[0].Rows[s]["total"].ToString() != string.Empty) && (dssubgrd.Tables[0].Rows[s]["total"].ToString() != "0"))
                {

                    if (dggradetot != null && dggradetot.Tables[0] != null && dggradetot.Tables[0].Rows.Count > 0)
                    {
                        strtot = Convert.ToDouble(dssubgrd.Tables[0].Rows[s]["total"].ToString());

                        foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                        {
                            if (gratemp["frange"].ToString() != null && gratemp["frange"].ToString() != "" && gratemp["trange"].ToString() != null && gratemp["trange"].ToString() != "")
                            {
                                strgradetempfrm = Convert.ToDouble(gratemp["frange"].ToString());
                                strgradetempto = Convert.ToDouble(gratemp["trange"].ToString());

                                if (strgradetempfrm <= strtot && strgradetempto >= strtot)
                                {
                                    strgrade = gratemp["credit_points"].ToString();
                                    break;
                                }
                            }
                        }

                    }

                }
                else if ((dssubgrd.Tables[0].Rows[s]["grade"].ToString() != string.Empty))
                {

                    if (dggradetot != null && dggradetot.Tables[0] != null && dggradetot.Tables[0].Rows.Count > 0)
                    {
                        strtotgrac = Convert.ToString(dssubgrd.Tables[0].Rows[s]["grade"].ToString());
                        foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                        {
                            strgradetempgrade = Convert.ToString(gratemp["mark_grade"].ToString());
                            if (strgradetempgrade.ToString().Trim() == strtotgrac.ToString().Trim())
                            {
                                strgrade = gratemp["credit_points"].ToString();
                                break;
                            }

                        }

                    }

                }

                if (strgrade != "" && strgrade != null)
                {


                    if (dssubgrd.Tables[0].Rows[s]["credit_points"].ToString() != null && dssubgrd.Tables[0].Rows[s]["credit_points"].ToString() != "")
                    {
                        creditval = Convert.ToDouble(dssubgrd.Tables[0].Rows[s]["credit_points"].ToString());
                        if (creditsum1 == 0)
                        {
                            creditsum1 = Convert.ToDouble(dssubgrd.Tables[0].Rows[s]["credit_points"].ToString());
                        }
                        else
                        {
                            creditsum1 = creditsum1 + Convert.ToDouble(dssubgrd.Tables[0].Rows[s]["credit_points"].ToString());
                        }
                    }
                    if (gpacal1 == 0)
                    {
                        gpacal1 = Convert.ToDouble(strgrade) * creditval;
                    }
                    else
                    {
                        gpacal1 = gpacal1 + (Convert.ToDouble(strgrade) * creditval);
                    }

                }
            }

        }
        if (creditsum1 != 0)
        {
            finalgpa1 = Math.Round((gpacal1 / creditsum1), 2, MidpointRounding.AwayFromZero);
        }
        calculate = finalgpa1.ToString();
        total1 = Math.Round(total1, 0, MidpointRounding.AwayFromZero);
    }

    public string Calculete_CGPA(string RollNo, string semval, string degree_code, string batch_year, string latmode, string collegecode)
    {
        double total = 0;
        bool flag = true;
        try
        {
            int jvalue = 0;
            string strgrade = string.Empty;
            double creditval = 0;
            double finalgpa1 = 0;
            double creditsum1 = 0;
            double gpacal1 = 0;
            string strsubcrd = string.Empty;
            int gtempejval = 0;
            string syll_code = string.Empty;
            string examcodevalg = string.Empty;
            DataSet dggradetot = new DataSet();
            DataSet dssem = new DataSet();
            double strtot = 0, inte = 0, exte = 0;
            double strgradetempfrm = 0;
            double strgradetempto = 0;
            string strgradetempgrade = string.Empty;
            string strtotgrac = string.Empty;
            string sqlcmdgraderstotal = string.Empty;
            int attemptswith = 0;
            string strattmaxmark = string.Empty;
            int attmpt = 0, maxmark = 0;
            string batch_code = string.Empty;
            if (Convert.ToString(ddlEdulevel.SelectedItem).ToUpper().Trim() == "UG")
            {
                batch_code = "0";
            }
            else
            {
                batch_code = "1";
            }
            strattmaxmark = d2.GetFunctionv("select convert(varchar,attempts)+'-'+convert(varchar,maxmark) as amm from coe_attmaxmark where collegecode='" + collegecode + "'");

            string[] semecount = strattmaxmark.Split(new Char[] { '-' });

            if (semecount.GetUpperBound(0) == 1)
            {
                attmpt = Convert.ToInt32(semecount[0].ToString());
                maxmark = Convert.ToInt32(semecount[1].ToString());
                flag = true;
            }
            else
            {
                flag = false;
            }
            sqlcmdgraderstotal = " select distinct frange,trange,credit_points,mark_grade  from grade_master where degree_code=" + batch_code + " and batch_year=" + batch_code + " and college_code=" + collegecode + "";
            dggradetot = d2.select_method(sqlcmdgraderstotal, hat, "Text");

            strsubcrd = " Select Subject.credit_points,Mark_Entry.internal_mark,Mark_Entry.external_mark,Mark_Entry.total,Mark_Entry.grade,Mark_Entry.attempts from Mark_Entry,Subject,Syllabus_Master where Mark_Entry.Subject_No = Subject.Subject_No and Syllabus_Master.syll_Code = Subject.syll_Code and roll_no='" + RollNo + "' and (result='Pass' or result='pass') ";
            strsubcrd = strsubcrd + " AND Exam_Code IN (SELECT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degree_code + "' AND Batch_Year =" + batch_year + " AND current_semester<=" + semval + " ) ";
            //strsubcrd = strsubcrd + " AND Exam_Code NOT IN (SELECT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degree_code + "' and batch_year=" + batch_year + " AND Exam_Year =2013 AND Exam_Month >4)";
            strsubcrd = strsubcrd + " AND Roll_No='" + RollNo + "' AND Syllabus_Master.Semester <=" + semval + " AND UPPER(Result) ='PASS' ";


            if (strsubcrd != null && strsubcrd != "")
            {
                DataSet dssubmark = d2.select_method_wo_parameter(strsubcrd, "text");
                for (int s = 0; s < dssubmark.Tables[0].Rows.Count; s++)
                {
                    if ((dssubmark.Tables[0].Rows[s]["total"].ToString() != string.Empty) && (dssubmark.Tables[0].Rows[s]["total"].ToString() != "0"))
                    {
                        if (dggradetot != null && dggradetot.Tables[0] != null && dggradetot.Tables[0].Rows.Count > 0)
                        {
                            strtot = 0;
                            double.TryParse(Convert.ToString(dssubmark.Tables[0].Rows[s]["total"]), out strtot);
                            inte = 0;
                            double.TryParse(Convert.ToString(dssubmark.Tables[0].Rows[s]["internal_mark"]), out inte);
                            exte = 0;
                            double.TryParse(Convert.ToString(dssubmark.Tables[0].Rows[s]["external_mark"]), out exte);
                            attemptswith = 0;
                            int.TryParse(Convert.ToString(dssubmark.Tables[0].Rows[s]["attempts"]), out attemptswith);
                            total = Convert.ToDouble(strtot) + Convert.ToDouble(total);

                            if (flag == true)
                            {
                                if (attmpt > attemptswith)//ATTEMPTS compared with attempts in coe settings if attempts lower than coe settings
                                {
                                    foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                    {
                                        if (gratemp["frange"].ToString() != null && gratemp["frange"].ToString() != "" && gratemp["trange"].ToString() != null && gratemp["trange"].ToString() != "")
                                        {
                                            strgradetempfrm = Convert.ToDouble(gratemp["frange"].ToString());
                                            strgradetempto = Convert.ToDouble(gratemp["trange"].ToString());

                                            if (strgradetempfrm <= strtot && strgradetempto >= strtot)
                                            {
                                                strgrade = gratemp["credit_points"].ToString();
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    inte = 0;
                                    strtot = exte;// total only consider extermarks only
                                    foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                    {
                                        if (gratemp["frange"].ToString() != null && gratemp["frange"].ToString() != "" && gratemp["trange"].ToString() != null && gratemp["trange"].ToString() != "")
                                        {
                                            strgradetempfrm = Convert.ToDouble(gratemp["frange"].ToString());
                                            strgradetempto = Convert.ToDouble(gratemp["trange"].ToString());

                                            if (strgradetempfrm <= strtot && strgradetempto >= strtot)
                                            {
                                                strgrade = gratemp["credit_points"].ToString();
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                strtot = Convert.ToDouble(dssubmark.Tables[0].Rows[s]["total"].ToString());

                                foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                {
                                    if (gratemp["frange"].ToString() != null && gratemp["frange"].ToString() != "" && gratemp["trange"].ToString() != null && gratemp["trange"].ToString() != "")
                                    {
                                        strgradetempfrm = Convert.ToDouble(gratemp["frange"].ToString());
                                        strgradetempto = Convert.ToDouble(gratemp["trange"].ToString());

                                        if (strgradetempfrm <= strtot && strgradetempto >= strtot)
                                        {
                                            strgrade = gratemp["credit_points"].ToString();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if ((dssubmark.Tables[0].Rows[s]["grade"].ToString() != string.Empty))
                    {

                        if (dggradetot != null && dggradetot.Tables[0] != null && dggradetot.Tables[0].Rows.Count > 0)
                        {
                            strtotgrac = Convert.ToString(dssubmark.Tables[0].Rows[s]["grade"].ToString());
                            foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                            {
                                strgradetempgrade = Convert.ToString(gratemp["mark_grade"].ToString());
                                if (strgradetempgrade.ToString().Trim() == strtotgrac.ToString().Trim())
                                {
                                    strgrade = gratemp["credit_points"].ToString();
                                    break;
                                }

                            }

                        }
                    }

                    creditval = Convert.ToDouble(dssubmark.Tables[0].Rows[s]["credit_points"].ToString());
                    if (creditsum1 == 0)
                    {
                        creditsum1 = Convert.ToDouble(dssubmark.Tables[0].Rows[s]["credit_points"].ToString());
                    }
                    else
                    {
                        creditsum1 = creditsum1 + Convert.ToDouble(dssubmark.Tables[0].Rows[s]["credit_points"].ToString());
                    }

                    if (gpacal1 == 0)
                    {
                        if (strgrade != "")
                        {
                            gpacal1 = Convert.ToDouble(strgrade) * creditval;
                        }
                    }
                    else
                    {
                        if (strgrade != "")
                        {
                            gpacal1 = gpacal1 + (Convert.ToDouble(strgrade) * creditval);
                        }
                    }
                }
            }

            creditval = 0;
            strgrade = string.Empty;

            finalgpa1 = Math.Round((gpacal1 / creditsum1), 2, MidpointRounding.AwayFromZero);
            calculate = Convert.ToString(finalgpa1);
            creditsum1 = 0;
            gpacal1 = 0;
            finalgpa1 = 0;
            total1 = Convert.ToDouble(total);

        }
        catch (Exception vel)
        {
            lblErrSearch.Text = vel.StackTrace;
            lblErrSearch.Visible = true;
        }
        if (calculate == "NaN")
        {
            return "-";
        }
        else
        {
            return calculate;
        }

    }

    private Hashtable HashValueToZero(Hashtable theHash)
    {
        object[] keys = new object[theHash.Keys.Count];
        theHash.Keys.CopyTo(keys, 0);
        foreach (object key in keys)
        {
            theHash[key] = 0;
        }
        return theHash;
    }

    #endregion

    public void ugconsolidatedGrade_Mark(DataTable dtAll, ref Hashtable hatclasifycount, Hashtable hatclasifycolumn)
    {
        try
        {
            DataSet printds = new DataSet();
            string lblerror1 = string.Empty;
            DataSet printds_new = new DataSet();
            DataSet printds_rows = new DataSet();
            //578
            string max_sem = string.Empty;
            int maxSem = 0;
            Boolean printpage = false;
            string edu_level = string.Empty;
            string degree = string.Empty;
            string monthandyear = string.Empty;
            string studname = string.Empty;
            string dob = string.Empty;
            string rollnosub = string.Empty;
            string regnumber = string.Empty;
            string batch_year = string.Empty;
            string degree_code = string.Empty;
            string exam_code = string.Empty;
            string sem = string.Empty;

            string branch = string.Empty;
            int month = 0;
            string monthstr = string.Empty;
            string sql2 = string.Empty;
            string sql3 = string.Empty;
            string roman = string.Empty;
            string semroman = string.Empty;
            string grade = string.Empty;
            string gradepoints = string.Empty;
            string coe = string.Empty;
            string app_no = string.Empty;
            string admid_date = string.Empty;
            DataSet gradeds = new DataSet();
            DataSet dsSpl = new DataSet();
            DataTable dtStar = new DataTable();
            double ugpgminpass = 0;

            //foreach (DictionaryEntry ht in hatclasifycolumn)
            //{
            //    string k = Convert.ToString(ht.Key);
            //    hatclasifycolumn[k] = 0;
            //}

            //foreach (DictionaryEntry ht in hatclasifycount)
            //{
            //    string k = Convert.ToString(ht.Key);
            //    hatclasifycount[k] = 0;
            //}
            //HashValueToZero(hatclasifycolumn);
            //HashValueToZero(hatclasifycount);
            for (int res = 0; res < dtAll.Rows.Count; res++)
            {
                int isval = 0;
                int additonalstatus = 0;

                isval = 1;
                int splcredit = 0;
                if (isval == 1)
                {
                    ugpgminpass = 0;
                    printds.Clear();
                    printds.Dispose();
                    printds_new.Clear();
                    printds_new.Dispose();
                    string grade_set = string.Empty;
                    rollnosub = Convert.ToString(dtAll.Rows[res]["roll_no"]);
                    string collegeCode = Convert.ToString(dtAll.Rows[res]["roll_no"]);
                    //regnumber = FpSpread2.Sheets[0].Cells[res, 2].Text;
                    //CONVERT(VARCHAR(11),GETDATE(),106)
                    int setng_ovrtotalcreadits = 0;
                    int setng_mintotalcreadits = 0;
                    int totalcreitdsened = 0;
                    int noofsubapplied = 0;
                    int noofsubpassed = 0;
                    int noofsubfailed = 0;
                    int Totalfailcount = 0;
                    int tot_credits = 0;
                    int Tot_credit_settings = 0;
                    string sql = "SELECT Reg_No,r.batch_year,r.app_no,r.degree_code,R.current_semester,R.Stud_Name,Course_Name as degree,Dept_name,CONVERT(VARCHAR, dob, 110) as dob,c.edu_level,CONVERT(VARCHAR(11),R.Adm_Date,106) as ADM_DATE FROM Registration R,Applyn A,Degree G,Course C,Department D WHERE R.App_No = A.App_No AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND D.College_Code = G.College_Code AND Roll_No = '" + rollnosub + "' and r.college_code='" + Session["collegecode"].ToString() + "' ;  select coe,principal from collinfo where college_code='" + Session["collegecode"].ToString() + "';select * from exam_details";
                    sql = sql + "  select count(s.subject_no) as total from subjectchooser sc,subject s,registration r where sc.roll_no=r.roll_no and s.subject_no=sc.subject_no and r.roll_no='" + rollnosub + "'";
                    sql = sql + "   Select count(subject.subject_no) as total from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'  and roll_no='" + rollnosub + "'; ";
                    sql = sql + "   Select count(subject.subject_no) as total from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='fail'  and roll_no='" + rollnosub + "'; select distinct m.subject_no from mark_entry m,subjectChooser sc where m.roll_no=sc.roll_no and m.subject_no=sc.subject_no and m.roll_no='" + rollnosub + "' and m.result<>'Pass' and m.subject_no not in(select distinct m.subject_no from mark_entry m,subjectChooser sc where m.roll_no=sc.roll_no and m.subject_no=sc.subject_no and m.roll_no='" + rollnosub + "' and m.result='Pass');  select distinct COUNT(teq.Equal_Subject_Code),teq.Com_Subject_Code from  subjectchooser sc,subject s,registration r , tbl_equal_paper_Matching teq   where sc.roll_no=r.roll_no and s.subject_no=sc.subject_no   and r.roll_no='" + rollnosub + "'  and teq.Equal_Subject_Code=s.subject_code group by teq.Com_Subject_Code having COUNT(teq.Equal_Subject_Code)>1; Select sum(credit_points) from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'  and roll_no='" + rollnosub + "'";
                    sql = sql + "     Select distinct  subject.subject_no, subject_name, subject_code from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='fail'  and roll_no='" + rollnosub + "'  and SUBSTRING(subject_code,7,1)!='M'    and  subject.subject_no not in (   Select subject.subject_no from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'   and roll_no='" + rollnosub + "')";
                    sql = sql + "     Select distinct  subject.subject_no, subject_name, subject_code from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='fail'  and roll_no='" + rollnosub + "'    and SUBSTRING(subject_code,7,1)='M'    and  subject.subject_no not in (   Select subject.subject_no from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'   and roll_no='" + rollnosub + "')";
                    sql = sql + "  SELECT STUFF((SELECT distinct ''',''' + convert(nvarchar(max),[subject_code])  FROM subject sy   where  subject_name='Computer training'   FOR XML PATH('')),1,1,'''') as [Roll_No]";
                    //printds = d2.select_method_wo_parameter(sql, "Text");
                    string qry = "SELECT Reg_No,r.batch_year,r.app_no,r.degree_code,R.current_semester,R.Stud_Name,Course_Name as degree,Dept_name,CONVERT(VARCHAR, dob, 105) as dob,c.edu_level,CONVERT(VARCHAR(11),R.Adm_Date,106) as ADM_DATE FROM Registration R,Applyn A,Degree G,Course C,Department D WHERE R.App_No = A.App_No AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND D.College_Code = G.College_Code AND Roll_No = '" + rollnosub + "' and r.college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'; ";
                    qry += " select coe,principal from collinfo where college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "';";
                    qry += " select * from exam_details ed,Registration r where ed.batch_year=r.Batch_Year and ed.degree_code=r.degree_code and r.Roll_No='" + rollnosub + "' order by exam_code; ";
                    qry += " select count(distinct s.subject_no) as total from subjectchooser sc,syllabus_master sm,subject s,registration r where sm.syll_code=s.syll_code and sm.Batch_Year=r.Batch_Year and sm.degree_code=r.degree_code and sc.roll_no=r.roll_no and s.subject_no=sc.subject_no and r.roll_no='" + rollnosub + "'; ";
                    qry += " Select count(distinct s.subject_no) as total from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,subjectchooser sc where sm.syll_code=s.syll_code and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and ss.syll_code=s.syll_code and ss.syll_code=sm.syll_code and s.subject_no=sc.subject_no and sc.subject_no=m.subject_no and sc.roll_no=m.roll_no and  m.result='pass'  and m.roll_no='" + rollnosub + "';";
                    qry += " Select count(distinct s.subject_no) as total from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,subjectchooser sc where sm.syll_code=s.syll_code and sm.syll_code=ss.syll_code and s.syll_code=ss.syll_code and sc.roll_no=m.roll_no and sc.subject_no=s.subject_no and sc.subject_no=m.subject_no and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and  m.result='fail'  and m.roll_no='" + rollnosub + "';  ";
                    qry += " select distinct m.subject_no from mark_entry m,subjectChooser sc,subject s,syllabus_master sm where m.roll_no=sc.roll_no and s.syll_code=sm.syll_code and s.subject_no=sc.subject_no and s.subject_no=m.subject_no and m.subject_no=sc.subject_no and m.roll_no='" + rollnosub + "' and m.result<>'Pass' and m.subject_no not in(select distinct m.subject_no from mark_entry m,subjectChooser sc,syllabus_master sm,subject s where m.roll_no=sc.roll_no and m.subject_no=sc.subject_no and sm.syll_code=s.syll_code and s.subject_no=m.subject_no and s.subject_no=sc.subject_no and m.roll_no='" + rollnosub + "' and m.result='Pass'); ";
                    qry += " select distinct COUNT(teq.Equal_Subject_Code),teq.Com_Subject_Code from  subjectchooser sc,subject s,registration r , tbl_equal_paper_Matching teq,syllabus_master sm where sc.roll_no=r.roll_no and sm.Batch_Year=r.Batch_Year and sm.degree_code=r.degree_code and sm.syll_code=s.syll_code  and s.subject_no=sc.subject_no   and r.roll_no='" + rollnosub + "'  and teq.Equal_Subject_Code=s.subject_code group by teq.Com_Subject_Code having COUNT(teq.Equal_Subject_Code)>1;";
                    qry += " Select sum(credit_points) from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,subjectChooser sc where sm.syll_code=s.syll_code and s.syll_code=ss.syll_code and ss.syll_code=sm.syll_code and s.subject_no=sc.subject_no and sc.roll_no=m.roll_no and sc.subject_no=m.subject_no and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and  m.result='pass'  and m.roll_no='" + rollnosub + "'";
                    qry += " Select distinct  s.subject_no,subject_name, subject_code from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,subjectChooser sc where sm.syll_code=s.syll_code and s.syll_code=ss.syll_code and sm.syll_code=ss.syll_code and sc.subject_no=m.subject_no and sc.subject_no=s.subject_no and sc.roll_no=m.roll_no and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and  m.result='fail'  and m.roll_no='" + rollnosub + "'  and SUBSTRING(s.subject_code,7,1)!='M'    and  s.subject_no not in (Select s.subject_no from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,subjectChooser sc where sm.syll_code=s.syll_code and sm.syll_code=ss.syll_code and ss.syll_code=s.syll_code and sc.roll_no=m.roll_no and sc.subject_no=m.subject_no and sc.subject_no=s.subject_no and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and  m.result='pass' and m.roll_no='" + rollnosub + "')  ";
                    qry += " Select distinct  s.subject_no, subject_name, subject_code from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,subjectChooser sc where sm.syll_code=s.syll_code and s.syll_code=ss.syll_code and sm.syll_code=ss.syll_code and sc.subject_no=s.subject_no and sc.subject_no=m.subject_no and sc.roll_no=m.roll_no and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and  m.result='fail' and m.roll_no='" + rollnosub + "' and SUBSTRING(subject_code,7,1)='M'    and  s.subject_no not in (Select s.subject_no from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,subjectChooser sc where sm.syll_code=s.syll_code and sc.subject_no=s.subject_no and sc.subject_no =m.subject_no and sc.roll_no=m.roll_no and sm.syll_code=ss.syll_code and ss.syll_code=s.syll_code and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and  m.result='pass' and m.roll_no='" + rollnosub + "') ";
                    qry += " SELECT STUFF((SELECT distinct ''',''' + convert(nvarchar(max),[subject_code]) FROM subject sy,syllabus_master sm,Registration r,subjectChooser sc where sm.syll_code=sy.syll_code and sy.subject_no=sc.subject_no and sc.roll_no=r.Roll_No and sm.Batch_Year=r.Batch_Year and sm.degree_code=r.degree_code and subject_name='Computer training' FOR XML PATH('')),1,1,'''') as [Roll_No] ; ";
                    printds = d2.select_method_wo_parameter(qry, "Text");
                    if (printds.Tables.Count > 0)
                    {
                        noofsubapplied = Convert.ToInt32(printds.Tables[3].Rows[0][0].ToString());
                        //noofsubapplied = noofsubapplied - Convert.ToInt32(printds.Tables[7].Rows.Count);
                        noofsubpassed = Convert.ToInt32(printds.Tables[4].Rows[0][0].ToString());
                        noofsubfailed = Convert.ToInt32(printds.Tables[5].Rows[0][0].ToString());

                        Totalfailcount = printds.Tables[6].Rows.Count;
                        int.TryParse(Convert.ToString(printds.Tables[8].Rows[0][0]), out tot_credits);
                    }

                    if (printds.Tables[0].Rows.Count > 0)
                    {
                        batch_year = printds.Tables[0].Rows[0]["batch_year"].ToString();
                        degree_code = printds.Tables[0].Rows[0]["degree_code"].ToString();
                        edu_level = printds.Tables[0].Rows[0]["edu_level"].ToString();
                        app_no = Convert.ToString(printds.Tables[0].Rows[0]["app_no"]);

                        degree = printds.Tables[0].Rows[0]["degree"].ToString();
                        coe = printds.Tables[1].Rows[0]["coe"].ToString();
                        admid_date = Convert.ToString(printds.Tables[0].Rows[0]["ADM_DATE"]);

                        setng_ovrtotalcreadits = Convert.ToInt32(d2.GetFunctionv("select totalcredits from coe_ovrl_credits_Dts where degree_code='" + degree_code + "'"));

                        setng_mintotalcreadits = Convert.ToInt32(d2.GetFunctionv("select minimcredits from coe_ovrl_credits_Dts where degree_code='" + degree_code + "'"));

                        max_sem = d2.GetFunctionv("select NDurations from ndegree where batch_year='" + batch_year + "'  and Degree_code='" + degree_code + "' and college_code='" + Convert.ToString(Session["collegecode"]) + "'");
                        if (max_sem == "" || max_sem == null)
                        {
                            max_sem = d2.GetFunctionv("SELECT Duration FROM Degree where  Degree_Code='" + degree_code + "' and college_code='" + Convert.ToString(Session["collegecode"]) + "'");
                        }
                        int.TryParse(max_sem, out maxSem);
                        int newbatch = 0;
                        int.TryParse(batch_year, out newbatch);
                        DateTime dt = new DateTime();
                        int cur_year = DateTime.Now.Year;
                        int diff = newbatch + (maxSem / 2);
                        //if (cur_year >= diff)
                        //{
                        //}
                        //else
                        //{
                        //    lbl_popuperr.Text = "The Consolidated is Generated Only For Passed Out Students.";
                        //    errdiv.Visible = true;
                        //    return;
                        //}
                    }
                    if (edu_level.Trim().ToLower() == "ug" && noofsubpassed != noofsubapplied)
                    {
                        string comcode = string.Empty;

                        DataSet dspassorfail = new DataSet();
                        DataView dvcomptraing = new DataView();
                        DataView dvcomsubject = new DataView();
                        int comsubjectcount = 0;
                        DataSet dssequalpaers = new DataSet();
                        ArrayList comsubjects = new ArrayList();
                        for (int isub = 0; isub < printds.Tables[9].Rows.Count; isub++)
                        {
                            string commsubjectpaper1 = d2.GetFunctionv("select  Com_Subject_Code from tbl_equal_paper_Matching where Equal_Subject_Code='" + printds.Tables[9].Rows[isub][2].ToString() + "' ");
                            sql = "  select * from tbl_equal_paper_Matching where  Com_Subject_Code  in ('" + commsubjectpaper1 + "') ";
                            dssequalpaers.Clear();
                            dssequalpaers = d2.select_method_wo_parameter(sql, "Text");
                            if (dssequalpaers.Tables.Count > 0 && dssequalpaers.Tables[0].Rows.Count > 0)
                            {
                                for (int eqlpap = 0; eqlpap < dssequalpaers.Tables[0].Rows.Count; eqlpap++)
                                {
                                    string syllcode = d2.GetFunctionv("select syll_code from subject where subject_no='" + printds.Tables[9].Rows[isub][0].ToString() + "'");
                                    string equlpapersubjectno = d2.GetFunctionv("select subject_no from subject where syll_code='" + syllcode + "' and  subject_code='" + dssequalpaers.Tables[0].Rows[eqlpap]["Equal_Subject_Code"].ToString() + "'  ");
                                    if (equlpapersubjectno.Trim() != "" && equlpapersubjectno.Trim() != "0")
                                    {
                                        dspassorfail.Clear();
                                        dspassorfail = d2.select_method_wo_parameter(" select * from mark_entry where subject_no='" + equlpapersubjectno + "' and  result='pass' and roll_no='" + rollnosub + "'  ", "Text");
                                        if (dspassorfail.Tables[0].Rows.Count > 0)
                                        {
                                            if (!comsubjects.Contains(commsubjectpaper1))
                                            {
                                                comsubjectcount++;
                                                comsubjects.Add(commsubjectpaper1);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        string computersubjectcode = printds.Tables[11].Rows[0][0].ToString();
                        if (computersubjectcode != "")
                        {
                            computersubjectcode = computersubjectcode.Remove(0, 2);
                            computersubjectcode = computersubjectcode + "'";
                        }
                        printds.Tables[10].DefaultView.RowFilter = "subject_code in (" + computersubjectcode + ")";
                        dvcomptraing = printds.Tables[10].DefaultView;

                        int majorpaperscount = printds.Tables[10].Rows.Count;
                        int comcodecount = 0;

                        if (dvcomptraing.Count > 0)
                        {
                            majorpaperscount = printds.Tables[10].Rows.Count - dvcomptraing.Count;
                            comcodecount = comcodecount + 1;
                        }

                        comcodecount = Convert.ToInt32(printds.Tables[9].Rows.Count) - comsubjectcount;

                        int subjectmissed = noofsubapplied - noofsubpassed;
                        if (printds.Tables[10].Rows.Count <= 2 && subjectmissed <= 2 && comcodecount == 0 && setng_ovrtotalcreadits != setng_mintotalcreadits)
                        {
                            noofsubpassed = noofsubapplied;
                        }
                    }

                    //if (printds.Tables[0].Rows.Count > 0 && noofsubpassed == noofsubapplied)
                    if (printds.Tables[0].Rows.Count > 0 && noofsubpassed == noofsubapplied) //Totalfailcount == 0)
                    {
                        printpage = true;
                        string principal = string.Empty;
                        edu_level = printds.Tables[0].Rows[0]["edu_level"].ToString();
                        app_no = Convert.ToString(printds.Tables[0].Rows[0]["app_no"]);
                        degree = printds.Tables[0].Rows[0]["degree"].ToString();
                        coe = printds.Tables[1].Rows[0]["coe"].ToString();
                        admid_date = Convert.ToString(printds.Tables[0].Rows[0]["ADM_DATE"]);

                        string[] adm_dt = admid_date.Split(' ');

                        if (adm_dt.Length > 0)
                        {
                            if (adm_dt.Length == 3)
                                admid_date = adm_dt[2] + " - JUN";
                        }

                        // month = ddlMonth.SelectedIndex;
                        //monthstr = ddlMonth.SelectedIndex.ToString();
                        string strMonthName = string.Empty;
                        //monthandyear = strMonthName + "  " + ddlYear.SelectedItem.Text;
                        //monthandyear = monthandyear.ToUpper();
                        studname = printds.Tables[0].Rows[0]["Stud_Name"].ToString();
                        branch = printds.Tables[0].Rows[0]["Dept_name"].ToString();
                        dob = printds.Tables[0].Rows[0]["dob"].ToString();

                        string stdappno = printds.Tables[0].Rows[0]["App_No"].ToString();
                        batch_year = printds.Tables[0].Rows[0]["batch_year"].ToString();
                        degree_code = printds.Tables[0].Rows[0]["degree_code"].ToString();

                        // sql3 = "Select syllabus_master.semester,Subject_Type,subject_name,subject_code,subject.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,maxtotal,grade,cp,credit_points,mark_entry.subject_no,semester,maxtotal,exam_code,subject.Part_Type,sub_sem.priority,sub_sem.lab,subject.subjectpriority,SUBSTRING(subject_code,(LEN(subject_code)-2) ,1) as Prac_code from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'  and roll_no='" + rollnosub + "' order by syllabus_master.semester,isnull(subject.Part_Type,'3') asc,case when (SUBSTRING(subject_code,(LEN(subject_code)-2) ,1)='F' and lab=0) then null when (SUBSTRING(subject_code,(LEN(subject_code)-2) ,1)='M' and lab=0) then 'A' when (SUBSTRING(subject_code,(LEN(subject_code)-2) ,1)='M' and lab=1) then 'B' When (SUBSTRING(subject_code,(LEN(subject_code)-2) ,1)='A' and lab=0) then 'C' When (SUBSTRING(subject_code,(LEN(subject_code)-2) ,1)='A' and lab=1) then 'D' When (SUBSTRING(subject_code,(LEN(subject_code)-2) ,0)='E' and lab=1) then 'E' When (SUBSTRING(subject_code,(LEN(subject_code)-2) ,1)='E' and lab=1) then 'F' Else SUBSTRING(subject_code,(LEN(subject_code)-2) ,1) End asc,subject.subjectpriority,sub_sem.lab,subject_type desc,subject_code asc"; hide by sridhar
                        //sql3 = "Select syllabus_master.semester,Subject_Type,subject_name,subject_code,subject.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,maxtotal,grade,cp,credit_points,mark_entry.subject_no,semester,maxtotal,exam_code,subject.Part_Type from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'  and roll_no='" + rollnosub + "' order by syllabus_master.semester, subject.subjectpriority ";
                        sql3 = "Select distinct sm.semester,ss.subject_type,s.subject_name,s.subject_code,s.subject_no,m.result,s.max_int_marks,s.max_ext_marks,m.internal_mark,m.external_mark,m.total,s.maxtotal,m.grade,m.cp,s.credit_points,sm.semester,s.mintotal,m.exam_code,ISNULL(s.Part_Type,'0') Part_Type,isnull(ss.priority,'0') priority,ss.lab,isNUll(s.subjectpriority,'0') subjectpriority,SUBSTRING(subject_code,(LEN(subject_code)-2) ,1) as Prac_code,ISNULL(s.print_acronmy,'') as print_acronmy from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,subjectChooser sc where sm.syll_code=ss.syll_code and ss.syll_code=s.syll_code and sm.syll_code=s.syll_code and sc.subject_no=s.subject_no and sc.subject_no=m.subject_no and sc.roll_no=m.roll_no and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and  result='pass'  and m.roll_no='" + rollnosub + "' order by sm.semester, subjectpriority ";
                        printds_rows.Clear();
                        printds_rows.Dispose();
                        printds_rows = d2.select_method_wo_parameter(sql3, "Text");

                        string batch_year1 = printds.Tables[0].Rows[0]["batch_year"].ToString() + "-";
                        if (edu_level.Trim().ToLower() == "ug")
                        {
                            batch_year1 = batch_year1 + Convert.ToString((Convert.ToInt32(batch_year) + 3));
                            grade_set = "0";
                            ugpgminpass = 50;
                        }
                        else
                        {
                            batch_year1 = batch_year1 + Convert.ToString((Convert.ToInt32(batch_year) + 2));
                            grade_set = "1";
                            ugpgminpass = 50;
                        }

                        double overallcreditearned = 0;

                        if (printds_rows.Tables[0].Rows.Count > 0)
                        {
                            bool starP3 = false;
                            string[] star = new string[2];
                            ArrayList arr_star = new ArrayList();
                            double[] starmrk = new double[2];
                            double[] starcredit = new double[2];
                            double[] stargpa = new double[2];
                            double[] starwpm = new double[2];

                            tot_credits = tot_credits + 1;
                            int creditsdiff = 0;
                            if (tot_credits > setng_mintotalcreadits)
                            {
                                creditsdiff = tot_credits - setng_mintotalcreadits;
                                creditsdiff = creditsdiff / 5;
                            }
                            string removesubjetcs = string.Empty;
                            DataSet cutsubject = new DataSet();
                            if (creditsdiff > 0)
                            {
                                //sql = "Select  top " + creditsdiff + " subject.subject_code,credit_points from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'  and roll_no='" + rollnosub + "' and Part_Type=3 and SUBSTRING(subject_code,7,1)='M' order by total asc,credit_points asc";
                                sql = "Select  top " + creditsdiff + " subject.subject_code,credit_points from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'  and roll_no='" + rollnosub + "' and Part_Type=3 and SUBSTRING(subject_code,7,1)='M' order by total asc,credit_points asc,syllabus_master.semester asc,subject.subjectpriority asc";
                                cutsubject.Clear();
                                cutsubject = d2.select_method_wo_parameter(sql, "Text");
                                int removecredites = 0;
                                for (int ii = 0; ii < cutsubject.Tables[0].Rows.Count; ii++)
                                {
                                    if (removecredites == 0)
                                    {
                                        removecredites = Convert.ToInt32(cutsubject.Tables[0].Rows[ii][1].ToString());
                                    }
                                    else
                                    {
                                        removecredites = removecredites + Convert.ToInt32(cutsubject.Tables[0].Rows[ii][1].ToString());
                                    }
                                }
                                if (removecredites <= 10)
                                {
                                    for (int ii = 0; ii < cutsubject.Tables[0].Rows.Count; ii++)
                                    {
                                        if (removesubjetcs.Trim() == "")
                                        {
                                            removesubjetcs = cutsubject.Tables[0].Rows[ii][0].ToString();
                                        }
                                        else
                                        {
                                            removesubjetcs = removesubjetcs + "," + cutsubject.Tables[0].Rows[ii][0].ToString();
                                        }
                                        arr_star.Add(Convert.ToString(cutsubject.Tables[0].Rows[ii][0]));
                                    }
                                }
                                else
                                {
                                    removesubjetcs = cutsubject.Tables[0].Rows[0][0].ToString();
                                    arr_star.Add(Convert.ToString(cutsubject.Tables[0].Rows[0][0]));
                                }
                            }

                            bool clasipg = false;
                            if (((edu_level.Trim().ToLower() == "ug" || edu_level.Trim().ToLower() == "u.g")))
                            {
                                starP3 = true;
                                splcredit = 1;
                                clasipg = false;
                            }
                            else
                            {
                                starP3 = false;
                                clasipg = true;
                                splcredit = 2;
                            }


                            DataTable dtPart1 = new DataTable();
                            string batchsetting = "0";

                            double partsums = 0.000;
                            double partwpmsum = 0.000;
                            int partrowcount = 0;
                            Double Credit_Points = 0.0;
                            Double grade_points = 0.0;
                            double creditstotal = 0;
                            double overalltotgrade = 0;
                            double Marks = 0;

                            DataTable dtPart3 = new DataTable();
                            if ((edu_level.Trim().ToLower() == "ug" || edu_level.Trim().ToLower() == "u.g"))
                            {
                                //tot_credits = tot_credits + 1;
                                creditsdiff = 0;
                                if (tot_credits > setng_mintotalcreadits)
                                {
                                    creditsdiff = tot_credits - setng_mintotalcreadits;
                                    creditsdiff = creditsdiff / 5;
                                }
                                partsums = 0;
                                partrowcount = 0;
                                creditstotal = 0;
                                partwpmsum = 0;
                                overalltotgrade = 0;
                                double min_credit = 0;
                                double Majorcredit = 0;
                                double Tot_Part3_Credit = 0;
                                double Tot_part3_Earned_credit = 0;
                                double Tot_Major_credit = 0;
                                double Tot_alied_Credit = 0;
                                double aliedCredit = 0;
                                string classify = string.Empty;
                                if (printds_rows.Tables[0].Rows.Count > 0)
                                {
                                    string sumpart = string.Empty;
                                    string wpm = string.Empty;
                                    removesubjetcs = string.Empty;
                                    DataView dv_demand_data = new DataView();
                                    DataView dv_demand_datadummy = new DataView();
                                    cutsubject = new DataSet();
                                    if (creditsdiff > 0)
                                    {
                                        //sql = "Select  top " + creditsdiff + " subject.subject_no,credit_points from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'  and roll_no='" + rollnosub + "' and Part_Type=3 and SUBSTRING(subject_code,7,1)='M' order by total asc,credit_points asc";
                                        sql = "Select  top " + creditsdiff + " subject.subject_no,credit_points from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'  and roll_no='" + rollnosub + "' and Part_Type=3 and SUBSTRING(subject_code,7,1)='M' order by total asc, credit_points asc,syllabus_master.semester asc,subject.subjectpriority asc";
                                        cutsubject.Clear();
                                        cutsubject = d2.select_method_wo_parameter(sql, "Text");
                                        int removecredites = 0;
                                        if (cutsubject.Tables.Count > 0 && cutsubject.Tables[0].Rows.Count > 0)
                                        {
                                            for (int ii = 0; ii < cutsubject.Tables[0].Rows.Count; ii++)
                                            {
                                                if (removecredites == 0)
                                                {
                                                    removecredites = Convert.ToInt32(cutsubject.Tables[0].Rows[ii][1].ToString());
                                                }
                                                else
                                                {
                                                    removecredites = removecredites + Convert.ToInt32(cutsubject.Tables[0].Rows[ii][1].ToString());
                                                }
                                            }

                                            if (removecredites <= 10)
                                            {

                                                for (int ii = 0; ii < cutsubject.Tables[0].Rows.Count; ii++)
                                                {
                                                    if (removesubjetcs.Trim() == "")
                                                    {
                                                        removesubjetcs = cutsubject.Tables[0].Rows[ii][0].ToString();
                                                    }
                                                    else
                                                    {
                                                        removesubjetcs = removesubjetcs + "," + cutsubject.Tables[0].Rows[ii][0].ToString();
                                                    }
                                                    arr_star.Add(Convert.ToString(cutsubject.Tables[0].Rows[ii][0]));
                                                }
                                            }
                                            else
                                            {
                                                removesubjetcs = cutsubject.Tables[0].Rows[0][0].ToString();
                                                arr_star.Add(Convert.ToString(cutsubject.Tables[0].Rows[0][0]));
                                            }
                                        }
                                    }
                                    if (removesubjetcs.Trim() != "")
                                    {
                                        printds_rows.Tables[0].DefaultView.RowFilter = "part_type='3' and subject_no not in (" + removesubjetcs + ")";
                                    }
                                    else
                                    {
                                        printds_rows.Tables[0].DefaultView.RowFilter = "part_type='3'";
                                    }

                                    dv_demand_datadummy = printds_rows.Tables[0].DefaultView;
                                    partrowcount = dv_demand_datadummy.Count;
                                    if (removesubjetcs.Trim() != "")
                                    {
                                        printds_rows.Tables[0].DefaultView.RowFilter = "part_type='3' and result='pass'  and subject_no not in (" + removesubjetcs + ")";
                                    }
                                    else
                                    {
                                        printds_rows.Tables[0].DefaultView.RowFilter = "part_type='3' and result='pass'";
                                    }

                                    dtPart1 = printds_rows.Tables[0].DefaultView.ToTable();
                                    dtPart3 = printds_rows.Tables[0].DefaultView.ToTable();

                                    object mini = Convert.ToInt16(dtPart3.Compute("Min(total)", "subject_type='Major Course'"));
                                    double minimum = 0;
                                    double.TryParse(Convert.ToString(mini), out minimum);
                                    double min_gpa = 0;
                                    double min_wpm = 0;
                                    min_gpa = (minimum / 10) * 5;
                                    min_wpm = minimum * 5;

                                    if (dv_demand_datadummy.Count > 0)
                                    {
                                        Tot_Part3_Credit = 0;
                                        for (int tc = 0; tc < dv_demand_datadummy.Count; tc++)
                                        {
                                            double dummycredit = 0;
                                            string sub_code = Convert.ToString(dv_demand_datadummy[tc]["subject_code"]).Trim();
                                            string gt_sub_code = sub_code.Substring(sub_code.Length - 3, 1);
                                            string subtypeaccr = string.Empty;
                                            subtypeaccr = Convert.ToString(dv_demand_datadummy[tc]["subject_type"]);
                                            subtypeaccr = findSubTypeAccromy(subtypeaccr);
                                            string printcronmy = Convert.ToString(dv_demand_datadummy[tc]["print_acronmy"]);
                                            if (printcronmy.Trim() != "")
                                            {
                                                subtypeaccr = printcronmy;
                                            }
                                            else
                                            {
                                                if (subtypeaccr.ToUpper().Trim() == "PRAC")
                                                {
                                                    if (gt_sub_code.ToUpper().Trim() == "M")
                                                    {
                                                        subtypeaccr = "MC";
                                                    }
                                                    else if (gt_sub_code.ToUpper().Trim() == "A")
                                                    {
                                                        subtypeaccr = "AC";
                                                    }
                                                    else
                                                    {
                                                        subtypeaccr = "PRAC";
                                                    }
                                                }
                                            }

                                            double.TryParse(Convert.ToString(dv_demand_datadummy[tc]["credit_points"]), out dummycredit);
                                            Tot_Part3_Credit += dummycredit;
                                            if (Convert.ToString(dv_demand_datadummy[tc]["subject_type"]).Trim().ToLower() == "major course" && subtypeaccr.ToLower().Trim() == "mc")
                                            {
                                                Tot_Major_credit += dummycredit;
                                            }
                                            else if (Convert.ToString(dv_demand_datadummy[tc]["subject_type"]).Trim().ToLower() == "allied course" && subtypeaccr.ToLower().Trim() == "ac")
                                            {
                                                Tot_alied_Credit += dummycredit;
                                            }
                                        }
                                    }
                                    if (dtPart1.Rows.Count > 0)
                                    {
                                        for (int sum = 0; sum < dtPart1.Rows.Count; sum++)
                                        {
                                            double dummycredit = 0;
                                            string sub_code = Convert.ToString(dtPart1.Rows[sum]["subject_code"]).Trim();
                                            string gt_sub_code = sub_code.Substring(sub_code.Length - 3, 1);
                                            string subtypeaccr = string.Empty;
                                            subtypeaccr = Convert.ToString(dtPart1.Rows[sum]["subject_type"]);
                                            subtypeaccr = findSubTypeAccromy(subtypeaccr);
                                            if (subtypeaccr.ToUpper().Trim() == "PRAC")
                                            {
                                                if (gt_sub_code.ToUpper().Trim() == "M")
                                                {
                                                    subtypeaccr = "MC";
                                                }
                                                else if (gt_sub_code.ToUpper().Trim() == "A")
                                                {
                                                    subtypeaccr = "AC";
                                                }
                                                else if (gt_sub_code.ToUpper().Trim() == "E")
                                                {
                                                    subtypeaccr = "EC";
                                                }
                                                else
                                                {
                                                    subtypeaccr = "PRAC";
                                                }
                                            }
                                            double.TryParse(Convert.ToString(dtPart1.Rows[sum]["credit_points"]), out dummycredit);
                                            Tot_part3_Earned_credit += dummycredit;
                                            if (Convert.ToString(dtPart1.Rows[sum]["subject_type"]).Trim().ToLower() == "major course" && subtypeaccr.ToLower().Trim() == "mc")
                                            {
                                                Majorcredit += dummycredit;
                                            }
                                            else if (Convert.ToString(dtPart1.Rows[sum]["subject_type"]).Trim().ToLower() == "allied course" && subtypeaccr.ToLower().Trim() == "ac")
                                            {
                                                aliedCredit += dummycredit;
                                            }
                                            double checkmarkmm = 0;
                                            double.TryParse(Convert.ToString(dtPart1.Rows[sum]["total"]), out checkmarkmm);
                                            checkmarkmm = Math.Round(checkmarkmm, 0, MidpointRounding.AwayFromZero);
                                            double maxsubbtotal = 0;
                                            double.TryParse(Convert.ToString(dtPart1.Rows[sum]["maxtotal"]).Trim(), out maxsubbtotal);
                                            if (maxsubbtotal != 0)
                                                checkmarkmm = checkmarkmm / maxsubbtotal * 100;
                                            checkmarkmm = Math.Round(checkmarkmm, 0, MidpointRounding.AwayFromZero);
                                            string gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='" + sem + "' and Degree_Code='" + grade_set + "' and batch_year='" + grade_set + "' and '" + checkmarkmm + "' between frange and trange";
                                            gradeds.Clear();
                                            gradeds = d2.select_method_wo_parameter(gradesql, "Text");
                                            if (gradeds.Tables.Count > 0 && gradeds.Tables[0].Rows.Count == 0)
                                            {
                                                gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "' and Degree_Code='" + grade_set + "' and batch_year='" + grade_set + "'  and '" + checkmarkmm + "' between frange and trange";//added by sridhar 
                                                gradeds.Clear();
                                                gradeds = d2.select_method_wo_parameter(gradesql, "Text");
                                            }
                                            if (gradeds.Tables.Count > 0 && gradeds.Tables[0].Rows.Count > 0)
                                            {
                                                for (int grd = 0; grd < gradeds.Tables[0].Rows.Count; grd++)
                                                {
                                                    if (Convert.ToInt32(gradeds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(gradeds.Tables[0].Rows[grd][2]) >= checkmarkmm)
                                                    {
                                                        double.TryParse(Convert.ToString(dtPart1.Rows[sum]["total"]), out grade_points);
                                                        double.TryParse(Convert.ToString(dtPart1.Rows[sum]["total"]), out Marks);
                                                        grade_points = grade_points / 10;

                                                        double.TryParse(Convert.ToString(dtPart1.Rows[sum]["credit_points"]), out Credit_Points);
                                                        creditstotal = creditstotal + Credit_Points;

                                                        partwpmsum += (Credit_Points * Marks);
                                                        partsums = partsums + (grade_points * Credit_Points);
                                                    }
                                                }
                                            }
                                        }

                                        if (creditstotal == 0)
                                        {
                                            sumpart = "0.000";
                                            wpm = "0.00";
                                        }
                                        else if (creditstotal > 0)
                                        {
                                            partsums = (partsums / creditstotal);
                                            partsums = Math.Round(partsums, 3, MidpointRounding.AwayFromZero);
                                            partwpmsum = (partwpmsum / creditstotal);
                                            partwpmsum = Math.Round(partwpmsum, 2, MidpointRounding.AwayFromZero);
                                            sumpart = String.Format("{0:0.000}", partsums);
                                            wpm = string.Format("{0:0.00}", partwpmsum);
                                        }
                                        else
                                        {
                                            sumpart = "0.000";
                                            wpm = "0.00";
                                        }
                                    }
                                    else if (partrowcount > 0)
                                    {
                                        sumpart = "0.000";
                                        wpm = "0.00";
                                    }
                                    else
                                    {
                                        sumpart = "---";
                                        wpm = "---";
                                    }
                                    if (sumpart != "---")
                                    {
                                        double sumpartgrade = 0;
                                        if (double.TryParse(sumpart, out sumpartgrade))
                                        {
                                            sumpartgrade = Convert.ToDouble(sumpart);
                                            overalltotgrade = overalltotgrade + sumpartgrade;
                                        }
                                        else
                                        {
                                            sumpartgrade = 0;
                                        }
                                        batchsetting = "1";
                                        if (noofsubfailed != 0 && overalltotgrade >= 6)
                                        {
                                            string gradesqlclass = "select * from coe_classification where edu_level='" + edu_level + "'  and  '" + overalltotgrade + "'>= frompoint and '" + overalltotgrade + "'<= topoint and  markgradeflag='" + batchsetting + "'";
                                            gradeds.Clear();
                                            gradeds = d2.select_method_wo_parameter(gradesqlclass, "Text");
                                            string cclass = "First Class";
                                            if (gradeds.Tables.Count > 0 && gradeds.Tables[0].Rows.Count > 0)
                                            {
                                                classify = "First Class";
                                            }
                                            else
                                            {
                                                classify = "First Class";
                                            }
                                        }
                                        else
                                        {
                                            string gradesqlclass = "select * from coe_classification where edu_level='" + edu_level + "'  and  '" + overalltotgrade + "'>= frompoint and '" + overalltotgrade + "'<= topoint and  markgradeflag='" + batchsetting + "'";
                                            gradeds.Clear();
                                            gradeds = d2.select_method_wo_parameter(gradesqlclass, "Text");

                                            if (gradeds.Tables.Count > 0 && gradeds.Tables[0].Rows.Count > 0)
                                            {
                                                classify = Convert.ToString(gradeds.Tables[0].Rows[0]["classification"]);
                                                grade = Convert.ToString(gradeds.Tables[0].Rows[0]["grade"]);

                                            }
                                        }
                                    }
                                    else
                                    {
                                        classify = string.Empty;
                                    }
                                }
                                else
                                {
                                    classify = string.Empty;
                                }
                                if (classify != "")
                                {
                                    if (classify == "First Class - Exemplary" || classify == "First Class with Distinction")
                                    {
                                        string qryno_arr = "select Count(*) from mark_entry m,subject s where m.subject_no=s.subject_no and  roll_no='" + rollnosub + "' and result='Fail'";
                                        qryno_arr = " Select count(distinct s.subject_no) as total from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,subjectchooser sc where sm.syll_code=s.syll_code and sm.syll_code=ss.syll_code and s.syll_code=ss.syll_code and sc.roll_no=m.roll_no and sc.subject_no=s.subject_no and sc.subject_no=m.subject_no and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and  m.result='fail'  and m.roll_no='" + rollnosub + "'";
                                        string woarrear = d2.GetFunction(qryno_arr);
                                        int historyarrear = 0;
                                        int.TryParse(woarrear, out historyarrear);
                                        if (historyarrear != 0)
                                        {
                                            classify = "First Class";
                                        }
                                    }
                                    double tempcount = 0;
                                    if (hatclasifycount.Contains(classify))
                                    {
                                        double.TryParse(Convert.ToString(hatclasifycount[classify]), out tempcount);
                                        hatclasifycount[classify] = tempcount + 1;
                                    }
                                }
                            }
                            else
                            {
                                partsums = 0;
                                partrowcount = 0;
                                creditstotal = 0;
                                partwpmsum = 0;
                                overalltotgrade = 0;
                                string classify = string.Empty;
                                if (printds_rows.Tables[0].Rows.Count > 0)
                                {
                                    string sumpart = string.Empty;
                                    string wpm = string.Empty;
                                    DataView dv_demand_data = new DataView();
                                    DataView dv_demand_datadummy = new DataView();
                                    int part = 1;
                                    part = 1;
                                    printds_rows.Tables[0].DefaultView.RowFilter = "Part_Type='" + part + "'";
                                    dv_demand_datadummy = printds_rows.Tables[0].DefaultView;
                                    partrowcount = dv_demand_datadummy.Count;
                                    printds_rows.Tables[0].DefaultView.RowFilter = "Part_Type='" + part + "' and result='pass'";
                                    dtPart1 = printds_rows.Tables[0].DefaultView.ToTable();

                                    if (dtPart1.Rows.Count > 0 && partrowcount == dtPart1.Rows.Count)
                                    {
                                        for (int sum = 0; sum < dtPart1.Rows.Count; sum++)
                                        {
                                            //double checkmarkmm = Convert.ToDouble(dtPart1.Rows[sum]["total"].ToString());
                                            double checkmarkmm = 0;
                                            double.TryParse(Convert.ToString(dtPart1.Rows[sum]["total"]), out checkmarkmm);
                                            checkmarkmm = Math.Round(checkmarkmm, 0, MidpointRounding.AwayFromZero);

                                            string gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='" + sem + "' and Degree_Code='" + grade_set + "' and batch_year='" + grade_set + "' and '" + checkmarkmm + "' between frange and trange";//added by sridhar 16/aug 2014
                                            gradeds.Clear();
                                            gradeds = d2.select_method_wo_parameter(gradesql, "Text");
                                            if (gradeds.Tables[0].Rows.Count == 0)
                                            {
                                                gradesql = "select * from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "' and Degree_Code='" + grade_set + "' and batch_year='" + grade_set + "'  and '" + checkmarkmm + "' between frange and trange";
                                                gradeds.Clear();
                                                gradeds = d2.select_method_wo_parameter(gradesql, "Text");
                                            }
                                            for (int grd = 0; grd < gradeds.Tables[0].Rows.Count; grd++)
                                            {
                                                if (Convert.ToInt32(gradeds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(gradeds.Tables[0].Rows[grd][2]) >= checkmarkmm)
                                                {
                                                    double.TryParse(Convert.ToString(dtPart1.Rows[sum]["total"]), out grade_points);
                                                    double.TryParse(Convert.ToString(dtPart1.Rows[sum]["total"]), out Marks);
                                                    grade_points = grade_points / 10;
                                                    //Credit_Points = Convert.ToDouble(dtPart1.Rows[sum]["credit_points"].ToString());
                                                    double.TryParse(Convert.ToString(dtPart1.Rows[sum]["credit_points"]), out Credit_Points);
                                                    creditstotal = creditstotal + Credit_Points;
                                                    partwpmsum += (Credit_Points * Marks);
                                                    partsums = partsums + (grade_points * Credit_Points);
                                                }
                                            }
                                        }
                                        if (creditstotal == 0)
                                        {
                                            sumpart = "0.000";
                                            wpm = "0.00";
                                        }
                                        else if (creditstotal > 0)
                                        {
                                            partsums = (partsums / creditstotal);
                                            partsums = Math.Round(partsums, 3, MidpointRounding.AwayFromZero);
                                            partwpmsum = (partwpmsum / creditstotal);
                                            partwpmsum = Math.Round(partwpmsum, 2, MidpointRounding.AwayFromZero);
                                            sumpart = String.Format("{0:0.000}", partsums);
                                            wpm = string.Format("{0:0.00}", partwpmsum);
                                        }
                                        else
                                        {
                                            sumpart = "0.000";
                                            wpm = "0.00";
                                        }
                                    }
                                    else if (partrowcount > 0)
                                    {
                                        sumpart = "0.000";
                                        wpm = "0.00";
                                    }
                                    else
                                    {
                                        sumpart = "---";
                                        wpm = "---";
                                    }
                                    if (sumpart != "---")
                                    {
                                        double sumpartgrade = 0;
                                        if (double.TryParse(sumpart, out sumpartgrade))
                                        {
                                            sumpartgrade = Convert.ToDouble(sumpart);
                                            // overalltotgrade = overalltotgrade + sumpartgrade;
                                            overalltotgrade = overalltotgrade + sumpartgrade;
                                        }
                                        else
                                        {
                                            sumpartgrade = 0;
                                        }

                                        batchsetting = "1";
                                        if (noofsubfailed != 0 && overalltotgrade >= 6)
                                        {
                                            string gradesqlclass = "select * from coe_classification where edu_level='" + edu_level + "'  and  '" + overalltotgrade + "'>= frompoint and '" + overalltotgrade + "'<= topoint and  markgradeflag='" + batchsetting + "'";//added by sridhar 16/aug 2014 and (classification='First Class' or classification='First')
                                            gradeds.Clear();
                                            gradeds = d2.select_method_wo_parameter(gradesqlclass, "Text");
                                            string cclass = "First Class";
                                            if (gradeds.Tables[0].Rows.Count > 0)
                                            {
                                                classify = "First Class";
                                            }
                                            else
                                            {
                                                classify = "First Class";
                                            }

                                        }
                                        else
                                        {
                                            string gradesqlclass = "select * from coe_classification where edu_level='" + edu_level + "'  and  '" + overalltotgrade + "'>= frompoint and '" + overalltotgrade + "'<= topoint and  markgradeflag='" + batchsetting + "'";//added by sridhar 16/aug 2014
                                            gradeds.Clear();
                                            gradeds = d2.select_method_wo_parameter(gradesqlclass, "Text");
                                            if (gradeds.Tables[0].Rows.Count > 0)
                                            {
                                                classify = Convert.ToString(gradeds.Tables[0].Rows[0]["classification"]);
                                            }

                                        }
                                    }
                                    else
                                    {
                                        classify = string.Empty;
                                    }
                                }
                                else
                                {
                                    classify = string.Empty;
                                }
                                if (classify != "")
                                {
                                    //if (classify == "First Class - Exemplary" || classify == "First Class with Distinction")
                                    //{
                                    //    string qryno_arr = "select Count(*) from mark_entry m,subject s where m.subject_no=s.subject_no and  roll_no='" + rollnosub + "' and result='Fail'";
                                    // qryno_arr = "Select count(distinct s.subject_no) as total from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,subjectchooser sc where sm.syll_code=s.syll_code and sm.syll_code=ss.syll_code and s.syll_code=ss.syll_code and sc.roll_no=m.roll_no and sc.subject_no=s.subject_no and sc.subject_no=m.subject_no and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and  m.result='fail'  and m.roll_no='" + rollnosub + "';  ";
                                    //    string woarrear = d2.GetFunction(qryno_arr);
                                    //    int historyarrear = 0;
                                    //    int.TryParse(woarrear, out historyarrear);
                                    //    if (historyarrear != 0)
                                    //    {
                                    //        classify = "First Class";
                                    //    }
                                    //}
                                    double tempcount = 0;
                                    if (hatclasifycount.Contains(classify))
                                    {
                                        double.TryParse(Convert.ToString(hatclasifycount[classify]), out tempcount);
                                        hatclasifycount[classify] = tempcount + 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public string findSubTypeAccromy(string subType)
    {
        string accr = string.Empty;
        string subt = string.Empty;
        subt = subType.Trim().ToLowerInvariant();
        switch (subt)
        {
            case "foundation course - i":
                accr = "FC-I";
                break;
            case "foundation course - ii":
                accr = "FC-II";
                break;
            case "major course":
            case "project":
            case "projects":
                accr = "MC";
                break;
            case "elective course":
                accr = "EC";
                break;
            case "allied course":
                accr = "AC";
                break;
            case "basic tamil":
                accr = "BT";
                break;
            case "advanced tamil":
                accr = "AT";
                break;
            case "general course":
                accr = "GC";
                break;
            case "inter diciplinary elective":
            case "elective interdisciplinary":
                accr = "IDE";
                break;
            case "general elective":
            case "elective general":
                accr = "GE";
                break;
            case "computer training":
                accr = "CT";
                break;
            case "personality development":
                accr = "PD";
                break;
            case "environmental studies":
                accr = "EVS";
                break;
            case "value education":
                accr = "VE";
                break;
            case "service learning":
                accr = "SLP";
                break;
            case "physical education":
                accr = "PE";
                break;
            case "department association activities":
                accr = "DAA";
                break;
            case "national cadet corps":
                accr = "NCC";
                break;
            case "national service scheme":
                accr = "NSS";
                break;
            case "sports activity":
                accr = "SA";
                break;
            case "scrub society":
                accr = "SS";
                break;
            case "community and social service":
                accr = "CSS";
                break;
            case "internship":
                accr = "IN";
                break;
            case "soft skill programme":
            case "soft skill programmes":
                accr = "SSP";
                break;
            case "practicals":
            case "practical":
                accr = "PRAC";
                break;
            default:
                accr = subType;
                break;
        }
        return accr;
    }

}