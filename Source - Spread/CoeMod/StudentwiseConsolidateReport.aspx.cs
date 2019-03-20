using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using InsproDataAccess;
using Gios.Pdf;
using System.Drawing;
using System.IO;
using System.Configuration;
public partial class CoeMod_StudentwiseConsolidateReport : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    string collegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    Hashtable hat = new Hashtable();
    string qryBatch = string.Empty;
    string qryDegreeCode = string.Empty;

    InsproDirectAccess dir = new InsproDirectAccess();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            lbl_msg.Visible = false;
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
            collegecode = Session["collegecode"].ToString();

            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();

            if (!IsPostBack)
            {

                bindcollege();
                bindbatch();
                binddegree();
                bindbranch();
                bindsec();
                BindExamYear();
                BindExamMonth();
            }
        }
        catch
        {
        }

    }

    protected void bindcollege()
    {
        try
        {
            string strUser = d2.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
            ds.Clear();
            ddl_college.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
            }
        }
        catch
        {
        }
    }

    public void bindbatch()
    {
        ddl_batch.Items.Clear();
        ds.Clear();
        ds = d2.select_method_wo_parameter("select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and delflag=0 and exam_flag<>'debar' order by batch_year desc ; select max(batch_year) from Registration where batch_year<>'-1' and batch_year<>'' and delflag=0 and exam_flag<>'debar'", "text");
        int count = 0;
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddl_batch.DataSource = ds.Tables[0];
            ddl_batch.DataTextField = "batch_year";
            ddl_batch.DataValueField = "batch_year";
            ddl_batch.DataBind();
        }
        //if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
        //{
        //    int max_bat = 0;
        //    int.TryParse(Convert.ToString(ds.Tables[1].Rows[0][0]).Trim(), out max_bat);
        //    ddl_batch.SelectedValue = max_bat.ToString();
        //    con.Close();
        //}
    }

    public void binddegree()
    {
        ddl_degree.Items.Clear();
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
        hat.Add("single_user", singleuser);
        hat.Add("group_code", group_user);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        ds = d2.select_method("bind_degree", hat, "sp");
        int count1 = ds.Tables[0].Rows.Count;
        if (count1 > 0)
        {
            ddl_degree.DataSource = ds;
            ddl_degree.DataTextField = "course_name";
            ddl_degree.DataValueField = "course_id";
            ddl_degree.DataBind();
        }
    }

    public void bindbranch()
    {
        ddl_branch.Items.Clear();
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
        hat.Add("single_user", singleuser);
        hat.Add("group_code", group_user);
        hat.Add("course_id", ddl_degree.SelectedValue);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        ds = d2.select_method("bind_branch", hat, "sp");
        int count2 = ds.Tables[0].Rows.Count;
        if (count2 > 0)
        {
            ddl_branch.DataSource = ds;
            ddl_branch.DataTextField = "dept_name";
            ddl_branch.DataValueField = "degree_code";
            ddl_branch.DataBind();
        }
    }

    public void bindsec()
    {
        try
        {
            ddl_section.Items.Clear();
            ds.Clear();
            ds = d2.select_method_wo_parameter("select distinct LTRIM(RTRIM(ISNULL(sections,''))) sections from registration where college_code='" + collegecode + "' and batch_year='" + Convert.ToString(ddl_batch.SelectedValue).Trim() + "' and degree_code='" + Convert.ToString(ddl_branch.SelectedValue).Trim() + "' and LTRIM(RTRIM(ISNULL(sections,'')))<>'-1' and LTRIM(RTRIM(ISNULL(sections,'')))<>'' and sections is not null and delflag=0 and exam_flag<>'Debar'", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_section.DataSource = ds;
                ddl_section.DataTextField = "sections";
                ddl_section.DataValueField = "sections";
                ddl_section.DataBind();

            }
        }
        catch (Exception ex)
        {
        }
    }

    /// <summary>
    /// Added By Malang Raja
    /// </summary>
    public void BindExamYear()
    {
        try
        {
            ddl_exyear.Items.Clear();
            ds.Clear();
            string collegeCode = string.Empty;
            string batchYear = string.Empty;
            string degreeCode = string.Empty;

            collegeCode = collegecode;
            batchYear = Convert.ToString(ddl_batch.SelectedValue).Trim();
            degreeCode = Convert.ToString(ddl_branch.SelectedValue).Trim();

            if (!string.IsNullOrEmpty(collegeCode))
            {
                collegeCode = " and dg.college_code in (" + collegeCode + ")";
            }
            if (!string.IsNullOrEmpty(batchYear))
            {
                qryBatch = " and ed.Batch_year in(" + batchYear + ")";
            }
            if (!string.IsNullOrEmpty(degreeCode))
            {
                qryDegreeCode = " and ed.degree_code in(" + degreeCode + ")";
            }


            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryBatch))
            {
                string qry = "select distinct ed.Exam_year from exam_details ed,Degree dg,Course c,Department dt where dg.Degree_Code=ed.degree_code and  c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and ed.Exam_year<>'0' " + collegeCode + qryDegreeCode + qryBatch + " order by ed.Exam_year desc";
                ds.Clear();
                ds.Reset();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(qry, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddl_exyear.DataSource = ds;
                    ddl_exyear.DataTextField = "Exam_year";
                    ddl_exyear.DataValueField = "Exam_year";
                    ddl_exyear.DataBind();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void BindExamMonth()
    {
        try
        {
            ddl_exmonth.Items.Clear();
            ds.Clear();
            string collegeCode = string.Empty;
            string batchYear = string.Empty;
            string degreeCode = string.Empty;
            string ExamYear = string.Empty;
            collegeCode = collegecode;
            batchYear = Convert.ToString(ddl_batch.SelectedValue).Trim();
            degreeCode = Convert.ToString(ddl_branch.SelectedValue).Trim();
            ExamYear = Convert.ToString(ddl_exyear.SelectedValue).Trim();

            if (!string.IsNullOrEmpty(collegeCode))
            {
                collegeCode = " and dg.college_code in (" + collegeCode + ")";
            }

            if (!string.IsNullOrEmpty(batchYear))
            {
                qryBatch = " and ed.Batch_year in(" + batchYear + ")";
            }

            if (!string.IsNullOrEmpty(degreeCode))
            {
                qryDegreeCode = " and ed.degree_code in(" + degreeCode + ")";
            }

            if (!string.IsNullOrEmpty(ExamYear))
            {
                ExamYear = " and Exam_year in (" + ExamYear + ")";
            }

            if (!string.IsNullOrEmpty(ExamYear) && !string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryBatch) && !string.IsNullOrEmpty(qryDegreeCode))
            {
                string qry = "select distinct ed.Exam_Month,upper(convert(varchar(3),DateAdd(month,ed.Exam_Month,-1))) as Month_Name from exam_details ed,Degree dg,Course c,Department dt where dg.Degree_Code=ed.degree_code and  c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and ed.Exam_Month<>'0' " + collegeCode + qryBatch + qryDegreeCode + ExamYear + " order by Exam_Month";
                ds.Clear();
                ds.Reset();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(qry, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddl_exmonth.DataSource = ds;
                    ddl_exmonth.DataTextField = "Month_Name";
                    ddl_exmonth.DataValueField = "Exam_Month";
                    ddl_exmonth.DataBind();
                }
            }

        }
        catch (Exception ex)
        {
        }
    }

    protected void ddl_college_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbatch();
            binddegree();
            bindbranch();
            bindsec();
            BindExamYear();
            BindExamMonth();
            divgrid.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            binddegree();
            bindbranch();
            bindsec();
            BindExamYear();
            BindExamMonth();
            divgrid.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch();
            bindsec();
            BindExamYear();
            BindExamMonth();
            divgrid.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsec();
            BindExamYear();
            BindExamMonth();
            divgrid.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddl_exyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindExamMonth();
            divgrid.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {

            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch
        {


        }
    }
    protected void Btn_go_Click(object sender, EventArgs e)
    {
        string college = string.Empty;
        string batch = string.Empty;
        string degree = string.Empty;
        string branch = string.Empty;
        string sec = string.Empty;
        string qry = string.Empty;

        GridView1.Visible = true;
        branch = ddl_branch.SelectedValue.ToString();
        batch = ddl_batch.SelectedValue.ToString();
        college = ddl_college.SelectedValue.ToString();
        divgrid.Visible = false;
        degree = ddl_degree.SelectedValue.ToString();
        string qrySections = string.Empty;
        if (ddl_section.Items.Count > 0)
        {
            sec = ddl_section.SelectedValue.ToString();
            if (!string.IsNullOrEmpty(sec) && sec.ToLower() != "all" && sec.ToLower() != "-1")
                qrySections = " and LTRIM(RTRIM(ISNULL(r.Sections,'')))='" + sec + "'";
        }
        try
        {
            //qry = "select distinct r.Batch_Year,r.degree_code,r.Current_Semester,LTRIM(RTRIM(ISNULL(r.Sections,''))) as Sections,r.Roll_No,r.serialno,r.App_No,r.Roll_Admit,r.Reg_No,r.Stud_Name,r.Stud_Type from Registration r, syllabus_master sm ,Subject s,subjectChooser sc where r.Batch_Year=sm.Batch_Year and r.degree_code=sm.degree_code and sm.syll_code=s.syll_code and r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no /*and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar'*/  and r.college_code='" + college + "' and sm.Batch_Year='" + batch + "' and sm.degree_code='" + degree + "' and sm.semester='" + sem + "' and s.subject_no='" + subject + "' order by r.Roll_No";

            //qry = "select distinct r.Batch_Year,r.degree_code,r.Current_Semester,LTRIM(RTRIM(ISNULL(r.Sections,''))) as Sections,r.Roll_No,r.serialno,r.App_No,r.Roll_Admit,r.Reg_No,r.Stud_Name,r.Stud_Type from Registration r, syllabus_master sm ,Subject s,subjectChooser sc where r.Batch_Year=sm.Batch_Year and r.degree_code=sm.degree_code and sm.syll_code=s.syll_code and r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no /*and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar'*/  and r.college_code=13 and sm.Batch_Year=2016 and sm.degree_code=45 and sm.semester=3 and s.subject_no=17966 order by r.Roll_No";

            qry = "select distinct r.Batch_Year,r.degree_code,r.Current_Semester,LTRIM(RTRIM(ISNULL(r.Sections,''))) as Sections,r.Roll_No,r.serialno,r.App_No,r.Roll_Admit,r.Reg_No,r.Stud_Name,r.Stud_Type from Registration r, syllabus_master sm ,Subject s,subjectChooser sc where r.Batch_Year=sm.Batch_Year and r.degree_code=sm.degree_code and sm.syll_code=s.syll_code and r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.DelFlag='0' and r.Exam_Flag<>'debar' and r.college_code='" + college + "' and sm.Batch_Year='" + batch + "' and sm.degree_code='" + branch + "' " + qrySections + " " + orderByStudents(Convert.ToString(ddl_college.SelectedValue).Trim(), "r");
            ds = d2.select_method_wo_parameter(qry, "Text");
            //string orderBy = orderByStudents(collegeCode, "r");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                GridView1.DataSource = ds;
                GridView1.DataBind();
                divgrid.Visible = true;
            }
            else
            {
                lblAlertMsg.Text = "No Record Found";
                divPopAlert.Visible = true;
                return;
            }

            bool isRollNoVisible = ColumnHeaderVisiblity(0);
            bool isRegNoVisible = ColumnHeaderVisiblity(1);
            bool isAdmissionNoVisible = ColumnHeaderVisiblity(2);
            bool isStudentTypeVisible = ColumnHeaderVisiblity(3);

            string tableappno = string.Empty;

            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox cbgrd = row.FindControl("gridcb") as CheckBox;
                //string appno = ((Label)row.FindControl("lblgridapplicationno")).Text.Trim();
                row.Cells[2].Visible = isRollNoVisible;
                row.Cells[3].Visible = isRegNoVisible;
                row.Cells[4].Visible = isAdmissionNoVisible;
                row.Cells[5].Visible = isStudentTypeVisible;
                GridView1.HeaderRow.Cells[2].Visible = isRollNoVisible;
                GridView1.HeaderRow.Cells[3].Visible = isRegNoVisible;
                GridView1.HeaderRow.Cells[4].Visible = isAdmissionNoVisible;
                GridView1.HeaderRow.Cells[5].Visible = isStudentTypeVisible;
            }
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
        }

    }

    protected void Btn_print_Click(object sender, EventArgs e)
    {
        try
        {
            int startingPosX = 0;
            int startingPosY = 25;
            //Font Fontco12 = new Font("Comic Sans MS", 12, FontStyle.Bold);
            //Font Fontpala12 = new Font("Palatino Linotype", 10, FontStyle.Bold);
            //Font Fontco10 = new Font("Comic Sans MS", 10, FontStyle.Regular);
            //Font Fontco12a = new Font("Comic Sans MS", 12, FontStyle.Bold);
            //Font Fontarial7 = new Font("Arial", 8, FontStyle.Regular);
            //Font Fontarial7r = new Font("Arial", 6, FontStyle.Bold);
            //Font Fontarial9 = new Font("Arial", 8, FontStyle.Bold);
            //Font Fontarial10 = new Font("Arial", 10, FontStyle.Regular);
            //Font Fontarial12 = new Font("Arial", 12, FontStyle.Regular);

            Font fontPageHeading = new Font("Times New Roman", 22, FontStyle.Bold);
            Font fontStudentNameHeading = new Font("Arial", 16, FontStyle.Bold);
            Font fontSemHeading = new Font("Arial", 11, FontStyle.Bold);
            Font fontTableColumnHeading = new Font("Times New Roman", 10, FontStyle.Bold);
            Font fontTableContent = new Font("Times New Roman", 9, FontStyle.Regular);

            //Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            PdfDocument mydoc = new PdfDocument(PdfDocumentFormat.A4_Horizontal);
            PdfPage mypdfpage;
            PdfTextArea pdfHeadingTxtArea;
            PdfTextArea pdfStudentNameTxtArea;
            //PdfTextArea pdfSemHeadingTxtArea;
            PdfTable pdfTable;
            //PdfTable pdfTable1;
            //PdfTable pdfTable2;
            //PdfTable pdfTable3;

            PdfTablePage pdftblPage;
            //PdfTablePage pdftblPage1;
            //PdfTablePage pdftblPage2;
            //PdfTablePage pdftblPage3;
            List<string> lstRegNo = new List<string>();
            bool isSheetsSaved = false;

            string batchYear = ddl_batch.SelectedItem.Value;
            string degreeCode = ddl_branch.SelectedItem.Value;

            if (GridView1.Rows.Count > 0)
            {
                foreach (GridViewRow gvrow in GridView1.Rows)
                {
                    CheckBox chkSelect = (CheckBox)gvrow.FindControl("gridcb");
                    Label lblRegNo = (Label)gvrow.FindControl("lblgridregno");
                    if (chkSelect.Checked)
                        if (!string.IsNullOrEmpty(lblRegNo.Text))
                            if (!lstRegNo.Contains(lblRegNo.Text))
                                lstRegNo.Add(lblRegNo.Text);
                }

                if (lstRegNo.Count > 0)
                {
                    string qry = "select m.roll_no,r.stud_name,r.reg_no,r.degree_code,r.Batch_year,sm.semester,s.subject_code,s.subject_name,ss.subject_type,ed.Exam_year,ed.Exam_Month,upper(convert(varchar(3),DateAdd(month,ed.Exam_Month,-1))) as Month_Name,upper(convert(varchar(3),DateAdd(month,ed.Exam_Month,-1)))+' '+CAST(ed.Exam_year as Varchar(5)) as MonthYear, CAST(CONVERT(varchar(10),ed.Exam_Month)+'/01/'+CONVERT(varchar(10),ed.Exam_year) as Datetime) as EDate,ed.exam_code,s.max_int_marks,isnull(m.internal_mark,0) as internal_mark,s.max_ext_marks,case when m.result='WHD' then '-1' else isnull(m.external_mark,0) end external_mark,s.maxtotal, (case when isnull(m.internal_mark,0) >=0  then   isnull(m.internal_mark,0) else 0 end) + (case when isnull(m.external_mark,0) >=0  then   isnull(m.external_mark,0) else 0 end) as total,s.credit_points,case m.result when 'Pass' Then 'PASS' when 'AAA' then 'ABSENT' when 'WHD' then 'AAA' else 'FAIL' end as result,s.Part_Type,ss.priority,ss.lab,SUBSTRING(s.subject_code,(LEN(s.subject_code)-2) ,1) as Prac_code,s.subject_no,s.min_int_marks,s.min_ext_marks,s.mintotal,print_acronmy from registration r, mark_entry m,subject s,syllabus_master sm,sub_sem ss,Exam_Details ed where s.subject_no=m.subject_no and m.exam_code=ed.exam_code and s.syll_code=sm.syll_code and s.syll_code=ss.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and ed.batch_year=sm.Batch_Year and ed.degree_code=sm.degree_code and r.roll_no=m.roll_no and r.Batch_Year=sm.Batch_Year and r.degree_code=sm.degree_code and m.external_mark is not null and m.total is not null and m.result is not null and CAST(CONVERT(varchar(20),ed.Exam_Month)+'/01/'+CONVERT(varchar(20),ed.Exam_year) as Datetime)<=CAST(CONVERT(varchar(20),'" + Convert.ToString(ddl_exmonth.SelectedValue).Trim() + "')+'/01/'+CONVERT(varchar(20),'" + Convert.ToString(ddl_exyear.SelectedValue).Trim() + "') as Datetime) and ISNULL(ed.Exam_Month,'')<>'' and ISNULL(ed.Exam_year,'')<>'' and ISNULL(ed.Exam_Month,'')<>'-1' and ISNULL(ed.Exam_year,'')<>'-1'  and ed.batch_year='" + batchYear + "' and ed.degree_code='" + degreeCode + "' and r.reg_no in('" + string.Join("','", lstRegNo.ToArray()) + "') order by m.roll_no,sm.semester,s.subjectpriority,ed.Exam_year,ed.Exam_Month";
                    ds = d2.select_method_wo_parameter(qry, "Text");
                    foreach (string studentRegNo in lstRegNo)
                    {
                        DataTable dtStudentDet = new DataTable();
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            ds.Tables[0].DefaultView.RowFilter = "reg_no='" + studentRegNo + "'";
                            dtStudentDet = ds.Tables[0].DefaultView.ToTable();
                        }
                        if (dtStudentDet.Rows.Count > 0)
                        {
                            string studBatchYear = Convert.ToString(dtStudentDet.Rows[0]["batch_year"]).Trim();
                            string studDegreeCode = Convert.ToString(dtStudentDet.Rows[0]["degree_code"]).Trim();
                            startingPosX = 0;
                            startingPosY = 15;
                            isSheetsSaved = true;

                            int max_sem1 = 0;
                            string max_sem = d2.GetFunctionv("select NDurations from ndegree where batch_year='" + studBatchYear + "'  and Degree_code='" + studDegreeCode + "'");
                            if (max_sem == "" || max_sem == null)
                            {
                                max_sem = d2.GetFunctionv("SELECT Duration FROM Degree where  Degree_Code='" + studDegreeCode + "'");
                            }
                            int.TryParse(max_sem, out max_sem1);

                            mypdfpage = mydoc.NewPage();
                            pdfHeadingTxtArea = new PdfTextArea(fontPageHeading, Color.Black, new PdfArea(mydoc, startingPosX, startingPosY, mydoc.PageWidth - (2 * startingPosX), 30), ContentAlignment.MiddleCenter, "Record's Office Consolidate Report ");
                            mypdfpage.Add(pdfHeadingTxtArea);

                            startingPosX += 25;
                            startingPosY += 20;
                            pdfStudentNameTxtArea = new PdfTextArea(fontStudentNameHeading, Color.Black, new PdfArea(mydoc, startingPosX, startingPosY, mydoc.PageWidth - (2 * startingPosX), 30), ContentAlignment.TopLeft, Convert.ToString(dtStudentDet.Rows[0]["reg_no"]) + "  " + Convert.ToString(dtStudentDet.Rows[0]["stud_name"]));
                            mypdfpage.Add(pdfStudentNameTxtArea);
                            double tablePageHeight = 0;
                            int iteration = 1;
                            int step = 0;
                            int posY = startingPosY;
                            int rowStep = 1;
                            double[] tblHeight = new double[2];

                            startingPosY += 20;
                            int finalPosY = startingPosY;
                            for (int i = 0; i < max_sem1; i++)
                            {
                                DataTable dtSemesterWiseMarks = new DataTable();
                                dtStudentDet.DefaultView.RowFilter = "semester='" + (i + 1) + "'";
                                dtSemesterWiseMarks = dtStudentDet.DefaultView.ToTable();
                                step = i % 2;
                                if (i % 2 == 0 && i != 0)
                                {
                                    rowStep++;
                                    finalPosY += Convert.ToInt32(tblHeight.Max()) + 6;
                                    startingPosY = finalPosY;
                                    if (dtSemesterWiseMarks.Rows.Count > 0)
                                    {
                                        if (finalPosY + 120 > mydoc.PageHeight - 90)
                                        {
                                            mypdfpage.SaveToDocument();
                                            mypdfpage = mydoc.NewPage();

                                            posY += startingPosY;
                                            startingPosX = 0;
                                            startingPosY = 15;

                                            pdfHeadingTxtArea = new PdfTextArea(fontPageHeading, Color.Black, new PdfArea(mydoc, startingPosX, startingPosY, mydoc.PageWidth - (2 * startingPosX), 30), ContentAlignment.MiddleCenter, "Record's Office Consolidate Report ");
                                            mypdfpage.Add(pdfHeadingTxtArea);

                                            startingPosX += 25;
                                            startingPosY += 20;
                                            pdfStudentNameTxtArea = new PdfTextArea(fontStudentNameHeading, Color.Black, new PdfArea(mydoc, startingPosX, startingPosY, mydoc.PageWidth - (2 * startingPosX), 30), ContentAlignment.TopLeft, Convert.ToString(dtStudentDet.Rows[0]["reg_no"]) + "  " + Convert.ToString(dtStudentDet.Rows[0]["stud_name"]));
                                            mypdfpage.Add(pdfStudentNameTxtArea);

                                            startingPosY += 20;
                                        }
                                    }
                                    tblHeight = new double[2];
                                    step = 0;

                                }

                                if (dtSemesterWiseMarks.Rows.Count > 0)
                                {
                                    DataTable dtDistinctSubject = new DataTable();
                                    dtDistinctSubject = dtSemesterWiseMarks.DefaultView.ToTable(true, "subject_code");
                                    if (dtDistinctSubject.Rows.Count > 0)
                                    {
                                        pdfTable = mydoc.NewTable(fontTableColumnHeading, dtDistinctSubject.Rows.Count + 2, 5, 3);
                                        pdfTable.VisibleHeaders = false;
                                        //pdfTable.SetBorders(Color.Black, 1, BorderType.None);
                                        pdfTable.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        pdfTable.SetColumnsWidth(new int[] { 150, 20, 20, 25, 30 });

                                        pdfTable.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        pdfTable.Cell(0, 0).SetContent("SEM " + (i + 1));
                                        pdfTable.Cell(0, 0).SetFont(fontSemHeading);

                                        foreach (PdfCell pc in pdfTable.CellRange(0, 0, 0, 0).Cells)
                                        {
                                            pc.ColSpan = 5;
                                        }
                                        pdfTable.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        pdfTable.Cell(1, 0).SetContent("Subject");

                                        pdfTable.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        pdfTable.Cell(1, 1).SetContent("ICA");

                                        pdfTable.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        pdfTable.Cell(1, 2).SetContent("ESE");

                                        pdfTable.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        pdfTable.Cell(1, 3).SetContent("Result");

                                        pdfTable.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        pdfTable.Cell(1, 4).SetContent("Year Of\nPassing");
                                        int row = 2;
                                        foreach (DataRow drSemSubject in dtDistinctSubject.Rows)
                                        {
                                            string subjectCode = Convert.ToString(drSemSubject["subject_code"]).Trim();

                                            DataView dvSubjectMark = new DataView();
                                            dtSemesterWiseMarks.DefaultView.RowFilter = "subject_code='" + subjectCode + "'";
                                            dvSubjectMark = dtSemesterWiseMarks.DefaultView;
                                            dvSubjectMark.Sort = "edate desc";

                                            if (dvSubjectMark.Count > 0)
                                            {
                                                string subjectname = Convert.ToString(dvSubjectMark[0]["subject_name"]).Trim();
                                                string internalmark = Convert.ToString(dvSubjectMark[0]["internal_mark"]).Trim();
                                                string externalmark = Convert.ToString(dvSubjectMark[0]["external_mark"]).Trim();
                                                string result = Convert.ToString(dvSubjectMark[0]["result"]).Trim().ToLower();
                                                string monthyear = Convert.ToString(dvSubjectMark[0]["MonthYear"]).Trim();

                                                double internalMarks = 0;
                                                double externalMarks = 0;
                                                double.TryParse(internalmark, out internalMarks);
                                                double.TryParse(externalmark, out externalMarks);

                                                if (internalMarks < 0)
                                                {
                                                    object intMarks = dvSubjectMark.ToTable().Compute("max(internal_mark)", "");
                                                    internalmark = Convert.ToString(intMarks).Trim();
                                                    double.TryParse(Convert.ToString(intMarks).Trim(), out internalMarks);
                                                }

                                                if (externalMarks < 0)
                                                {
                                                    object extMarks = dvSubjectMark.ToTable().Compute("max(external_mark)", "");
                                                    externalmark = Convert.ToString(extMarks).Trim();
                                                    double.TryParse(Convert.ToString(extMarks).Trim(), out externalMarks);
                                                }

                                                if (internalMarks < 0)
                                                {
                                                    switch (internalmark)
                                                    {
                                                        case "-1":
                                                            internalmark = "AB";
                                                            break;
                                                        case "-2":
                                                            internalmark = "NE";
                                                            break;
                                                        case "-3":
                                                            internalmark = "NR";
                                                            break;
                                                        case "-4":
                                                            internalmark = "WHD";
                                                            break;
                                                    }
                                                }
                                                if (externalMarks < 0)
                                                {
                                                    switch (externalmark)
                                                    {
                                                        case "-1":
                                                            externalmark = "AB";
                                                            break;
                                                        case "-2":
                                                            externalmark = "NE";
                                                            break;
                                                        case "-3":
                                                            externalmark = "NR";
                                                            break;
                                                        case "-4":
                                                            externalmark = "WHD";
                                                            break;
                                                    }
                                                }
                                                string displayResult = string.Empty;

                                                switch (result)
                                                {
                                                    case "pass":
                                                    case "p":
                                                        displayResult = "P";
                                                        break;
                                                    case "fail":
                                                    case "f":
                                                        displayResult = "F";
                                                        break;
                                                    case "whd":
                                                    case "w":
                                                    case "www":
                                                        displayResult = "WHD";
                                                        break;
                                                    case "mp":
                                                        displayResult = "MP";
                                                        break;
                                                    case "aaa":
                                                    case "ab":
                                                    case "absent":
                                                        displayResult = "AB";
                                                        break;
                                                    default:
                                                        //if (result.ToLower().Contains(""))
                                                        displayResult = result;
                                                        break;
                                                }

                                                pdfTable.Cell(row, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                pdfTable.Cell(row, 0).SetContent(subjectname);
                                                pdfTable.Cell(row, 0).SetFont(fontTableContent);

                                                pdfTable.Cell(row, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                pdfTable.Cell(row, 1).SetContent(internalmark);
                                                pdfTable.Cell(row, 1).SetFont(fontTableContent);

                                                pdfTable.Cell(row, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                pdfTable.Cell(row, 2).SetContent(externalmark);
                                                pdfTable.Cell(row, 2).SetFont(fontTableContent);

                                                pdfTable.Cell(row, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                pdfTable.Cell(row, 3).SetContent(displayResult);
                                                pdfTable.Cell(row, 3).SetFont(fontTableContent);

                                                pdfTable.Cell(row, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                pdfTable.Cell(row, 4).SetContent(monthyear);
                                                pdfTable.Cell(row, 4).SetFont(fontTableContent);
                                                row++;
                                            }

                                        }
                                        iteration++;
                                        pdftblPage = pdfTable.CreateTablePage(new PdfArea(mydoc, (step == 0) ? 5 : (mydoc.PageWidth / 2) + 5, startingPosY, (mydoc.PageWidth / 2) - 10, 400));
                                        mypdfpage.Add(pdftblPage);
                                        tblHeight[step] = pdftblPage.Area.Height + 5;

                                    }

                                    //if (false)
                                    //{
                                    //    foreach (DataRow drSemSubject in dtSemesterWiseMarks.Rows)
                                    //    {
                                    //        string subjectname = Convert.ToString(drSemSubject["subject_name"]).Trim();
                                    //        string internalmark = Convert.ToString(drSemSubject["internal_mark"]).Trim();
                                    //        string externalmark = Convert.ToString(drSemSubject["external_mark"]).Trim();
                                    //        string result = Convert.ToString(drSemSubject["result"]).Trim().ToLower();
                                    //        string monthyear = Convert.ToString(drSemSubject["MonthYear"]).Trim();

                                    //        double internalMarks = 0;
                                    //        double externalMarks = 0;
                                    //        double.TryParse(internalmark, out internalMarks);
                                    //        double.TryParse(externalmark, out externalMarks);

                                    //        if (internalMarks < 0)
                                    //            switch (internalmark)
                                    //            {
                                    //                case "-1":
                                    //                    internalmark = "AB";
                                    //                    break;
                                    //                case "-2":
                                    //                    internalmark = "NE";
                                    //                    break;
                                    //                case "-3":
                                    //                    internalmark = "NR";
                                    //                    break;
                                    //                case "-4":
                                    //                    internalmark = "WHD";
                                    //                    break;
                                    //            }
                                    //        if (externalMarks < 0)
                                    //            switch (externalmark)
                                    //            {
                                    //                case "-1":
                                    //                    externalmark = "AB";
                                    //                    break;
                                    //                case "-2":
                                    //                    externalmark = "NE";
                                    //                    break;
                                    //                case "-3":
                                    //                    externalmark = "NR";
                                    //                    break;
                                    //                case "-4":
                                    //                    externalmark = "WHD";
                                    //                    break;
                                    //            }
                                    //        string displayResult = string.Empty;

                                    //        switch (result)
                                    //        {
                                    //            case "pass":
                                    //            case "p":
                                    //                displayResult = "P";
                                    //                break;
                                    //            case "fail":
                                    //            case "f":
                                    //                displayResult = "F";
                                    //                break;
                                    //            case "whd":
                                    //            case "w":
                                    //            case "www":
                                    //                displayResult = "WHD";
                                    //                break;
                                    //            case "mp":
                                    //                displayResult = "MP";
                                    //                break;
                                    //            case "aaa":
                                    //            case "ab":
                                    //            case "absent":
                                    //                displayResult = "AB";
                                    //                break;
                                    //            default:
                                    //                //if (result.ToLower().Contains(""))
                                    //                displayResult = result;
                                    //                break;
                                    //        }

                                    //        pdfTable.Cell(row, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    //        pdfTable.Cell(row, 0).SetContent(subjectname);
                                    //        pdfTable.Cell(row, 0).SetFont(fontTableContent);

                                    //        pdfTable.Cell(row, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    //        pdfTable.Cell(row, 1).SetContent(internalmark);
                                    //        pdfTable.Cell(row, 1).SetFont(fontTableContent);

                                    //        pdfTable.Cell(row, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    //        pdfTable.Cell(row, 2).SetContent(externalmark);
                                    //        pdfTable.Cell(row, 2).SetFont(fontTableContent);

                                    //        pdfTable.Cell(row, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    //        pdfTable.Cell(row, 3).SetContent(displayResult);
                                    //        pdfTable.Cell(row, 3).SetFont(fontTableContent);

                                    //        pdfTable.Cell(row, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    //        pdfTable.Cell(row, 4).SetContent(monthyear);
                                    //        pdfTable.Cell(row, 4).SetFont(fontTableContent);
                                    //        row++;
                                    //    }
                                    //    iteration++;
                                    //    pdftblPage = pdfTable.CreateTablePage(new PdfArea(mydoc, (step == 0) ? 5 : (mydoc.PageWidth / 2) + 5, startingPosY, (mydoc.PageWidth / 2) - 10, 400));
                                    //    mypdfpage.Add(pdftblPage);
                                    //    tblHeight[step] = pdftblPage.Area.Height;
                                    //}
                                }
                            }

                            startingPosX += 25;
                            mypdfpage.SaveToDocument();

                        }
                    }
                }
                else
                {

                }
            }
            if (isSheetsSaved)
            {
                string filePath = HttpContext.Current.Server.MapPath("~");
                if (filePath != "")
                {
                    string szPath = filePath + "/Report/";
                    string szFile = "deanOfficeReport" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHmmssfff") + ".pdf";
                    if (!File.Exists(szPath + szFile))
                    {
                        mydoc.SaveToFile(szPath + szFile);
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/pdf";
                        Response.WriteFile(szPath + szFile);
                    }
                }
            }
        }
        catch { }
    }

    private string orderByStudents(string collegecode, string aliasName = null, string tableName = null, byte includeOrderBy = 0)
    {
        string orderBy = string.Empty;
        try
        {
            string orderBySetting = dir.selectScalarString("select value from master_Settings where settings='order_by' ");//and value<>''
            orderBySetting = orderBySetting.Trim();

            string serialNo = dir.selectScalarString("select LinkValue from inssettings where college_code='" + collegecode + "' and linkname='Student Attendance'");

            string aliasOrTableName = ((string.IsNullOrEmpty(aliasName) && string.IsNullOrEmpty(tableName)) ? "" : ((!string.IsNullOrEmpty(tableName)) ? tableName.Trim() + "." : ((!string.IsNullOrEmpty(aliasName)) ? aliasName.Trim() + "." : "")));

            orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no";
            if (serialNo.Trim().ToLower() == "1" || serialNo.ToLower().Trim() == "true")
                orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "serialno";
            else
                switch (orderBySetting)
                {
                    case "0":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no";
                        break;
                    case "1":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "Reg_No";
                        break;
                    case "2":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "Stud_Name";
                        break;
                    case "0,1,2":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no," + aliasOrTableName + "Reg_No," + aliasOrTableName + "stud_name";
                        break;
                    case "0,1":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no," + aliasOrTableName + "Reg_No";
                        break;
                    case "1,2":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "Reg_No," + aliasOrTableName + "Stud_Name";
                        break;
                    case "0,2":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no," + aliasOrTableName + "Stud_Name";
                        break;
                    default:
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no";
                        break;
                }
        }
        catch (Exception ex)
        {

        }
        return orderBy;
    }

    private bool ColumnHeaderVisiblity(int type, DataSet dsSettingsOptional = null)
    {
        bool hasValues = false;
        try
        {
            DataSet dsSettings = new DataSet();
            if (dsSettingsOptional == null)
            {
                string grouporusercode = string.Empty;
                if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    string groupCode = Convert.ToString(Session["group_code"]).Trim();
                    string[] groupUser = Convert.ToString(groupCode).Trim().Split(';');
                    if (groupUser.Length > 0)
                    {
                        groupCode = groupUser[0].Trim();
                    }
                    if (!string.IsNullOrEmpty(groupCode.Trim()))
                    {
                        grouporusercode = " and  group_code=" + Convert.ToString(groupCode).Trim() + "";
                    }
                }
                else if (Session["usercode"] != null)
                {
                    grouporusercode = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
                }
                if (!string.IsNullOrEmpty(grouporusercode))
                {
                    string Master1 = "select * from Master_Settings where settings in('Roll No','Register No','Admission No','Student_Type','Application No') and value='1' " + grouporusercode + "";
                    dsSettings = dir.selectDataSet(Master1);
                }
            }
            else
            {
                dsSettings = dsSettingsOptional;
            }
            if (dsSettings.Tables.Count > 0 && dsSettings.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drSettings in dsSettings.Tables[0].Rows)
                {
                    switch (type)
                    {
                        case 0:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "roll no")
                            {
                                hasValues = true;
                            }
                            break;
                        case 1:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "register no")
                            {
                                hasValues = true;
                            }
                            break;
                        case 2:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "admission no")
                            {
                                hasValues = true;
                            }
                            break;
                        case 3:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "student_type")
                            {
                                hasValues = true;
                            }
                            break;
                        case 4:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "application no")
                            {
                                hasValues = true;
                            }
                            break;
                    }
                    if (hasValues)
                        break;
                }
            }
            return hasValues;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    protected void SelectAll_Checked(object sender, EventArgs e)
    {

        CheckBox chckheader = (CheckBox)GridView1.HeaderRow.FindControl("chkselectall");

        foreach (GridViewRow row in GridView1.Rows)
        {
            CheckBox chckrw = (CheckBox)row.FindControl("gridcb");

            if (chckheader.Checked == true)
            {
                chckrw.Checked = true;
            }
            else
            {
                chckrw.Checked = false;
            }

        }


    }

    public void bindbutn_four(string rollno)
    { }

}