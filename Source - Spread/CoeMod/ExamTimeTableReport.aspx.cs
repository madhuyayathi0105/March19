using System;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Configuration;

public partial class ExamTimeTableReport : System.Web.UI.Page
{
    string CollegeCode = string.Empty;
    Hashtable hat = new Hashtable();
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    FarPoint.Web.Spread.TextCellType txtceltype = new FarPoint.Web.Spread.TextCellType();

    string collegeCode = string.Empty;
    string examYear = string.Empty;
    string examMonth = string.Empty;

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
            CollegeCode = Convert.ToString(Session["collegecode"]).Trim();
            lblerror.Visible = false;
            lblvalidation1.Visible = false;
            if (!IsPostBack)
            {
                // Panel2.Visible = true;
                cbDate.Checked = false;
                cbBatchYear.Checked = false;
                cbCourse.Checked = false;
                cbDepartment.Checked = false;
                cbSubject.Checked = false;

                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
                ddlBatchYear.Enabled = false;
                ddlCourse.Enabled = false;
                ddlDepartment.Enabled = false;
                ddlSubjectName.Enabled = false;

                // pnlFilter.Visible = false;
                Fpstudents.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                txtexcelname.Text = string.Empty;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                lblvalidation1.Visible = false;
                rbformat1.Checked = true;
                //loadyear();
                BindExamYear();
                BindExamMonth();
                if (ddlYear.Items.Count > 0)
                {
                    //loadmonth();
                    if (ddlMonth.Items.Count > 0)
                    {
                        loadbatch();
                        loaddegree();
                        loaddepartment();
                        loadsubject();
                        btnView.Enabled = true;
                        DateTime now = DateTime.Now;
                        txtFromDate.Text = now.Date.ToString("dd/MM/yyyy");
                        txtToDate.Text = now.Date.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        //cbBatchYear.Visible = false;
                        //cbCourse.Visible = false;
                        //cbDate.Visible = false;
                        //cbDepartment.Visible = false;
                        //cbSubject.Visible = false;
                        //ddlBatchYear.Visible = false;
                        //ddlCourse.Visible = false;
                        //ddlDepartment.Visible = false;
                        //ddlSubjectName.Visible = false;
                        //txtFromDate.Visible = false;
                        //txtToDate.Visible = false;
                        btnView.Enabled = false;
                        lblerror.Visible = true;
                        lblerror.Text = "No Exam Conducted";
                        chkindegee.Visible = false;
                        btnView.Visible = false;
                    }
                }
                else
                {
                    //cbBatchYear.Visible = false;
                    //cbCourse.Visible = false;
                    //cbDate.Visible = false;
                    //cbDepartment.Visible = false;
                    //cbSubject.Visible = false;
                    //ddlBatchYear.Visible = false;
                    //ddlCourse.Visible = false;
                    //ddlDepartment.Visible = false;
                    //ddlSubjectName.Visible = false;
                    txtFromDate.Visible = false;
                    txtToDate.Visible = false;
                    lblerror.Visible = true;
                    btnView.Enabled = false;
                    lblerror.Text = "No Exam Conducted";
                    chkindegee.Visible = false;
                    btnView.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void loadyear()
    {
        try
        {
            ddlYear.Items.Clear();
            DataSet ds = da.Examyear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlYear.DataSource = ds;
                ddlYear.DataTextField = "Exam_year";
                ddlYear.DataValueField = "Exam_year";
                ddlYear.DataBind();
                // ddlYear.SelectedIndex = ddlYear.Items.Count - 1;
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = Convert.ToString(ex);
        }
    }

    public void loadmonth()
    {
        try
        {
            ddlMonth.Items.Clear();
            DataSet ds = new DataSet();
            string year1 = ddlYear.SelectedValue;
            ds.Clear();
            ds = da.Exammonth(year1);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlMonth.DataSource = ds;
                ddlMonth.DataTextField = "monthName";
                ddlMonth.DataValueField = "Exam_month";
                ddlMonth.DataBind();
                ddlMonth.SelectedIndex = ddlMonth.Items.Count - 1;
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = Convert.ToString(ex);
        }
    }

    /// <summary>
    /// Developed By Malang Raja T
    /// </summary>
    public void BindExamYear()
    {
        try
        {
            ddlYear.Items.Clear();
            string qry = "select distinct ed.Exam_year from exam_details ed where ed.Exam_year<>'0' order by ed.Exam_year desc";
            ds.Clear();
            ds.Reset();
            ds.Dispose();
            ds = da.select_method_wo_parameter(qry, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlYear.DataSource = ds;
                ddlYear.DataTextField = "Exam_year";
                ddlYear.DataValueField = "Exam_year";
                ddlYear.DataBind();
                ddlYear.SelectedIndex = 0;
            }
            //ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)));
        }
    }

    /// <summary>
    /// Developed By Malang Raja T
    /// </summary>
    private void BindExamMonth()
    {
        try
        {
            ddlMonth.Items.Clear();
            string ExamYear = string.Empty;
            if (ddlYear.Items.Count > 0)
            {
                foreach (System.Web.UI.WebControls.ListItem li in ddlYear.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(ExamYear))
                        {
                            ExamYear = "'" + li.Value + "'";
                        }
                        else
                        {
                            ExamYear += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(ExamYear))
                {
                    ExamYear = " and Exam_year in (" + ExamYear + ")";
                }
            }
            if (!string.IsNullOrEmpty(ExamYear))
            {
                string qry = "select distinct ed.Exam_Month,upper(convert(varchar(3),DateAdd(month,ed.Exam_Month,-1))) as Month_Name from exam_details ed where  ed.Exam_Month<>'0' " + ExamYear + " order by Exam_Month";
                ds.Clear();
                ds.Reset();
                ds.Dispose();
                ds = da.select_method_wo_parameter(qry, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlMonth.DataSource = ds;
                    ddlMonth.DataTextField = "Month_Name";
                    ddlMonth.DataValueField = "Exam_Month";
                    ddlMonth.DataBind();
                    ddlMonth.SelectedIndex = 0;
                }
            }
            //ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)));
        }
    }

    public void loadbatch()
    {
        try
        {
            ddlBatchYear.Items.Clear();
            if (Session["collegecode"] != null)
            {
                CollegeCode = Convert.ToString(Session["collegecode"]).Trim();
            }
            collegeCode = CollegeCode;
            examYear = string.Empty;
            examMonth = string.Empty;
            if (ddlYear.Items.Count > 0)
            {
                examYear = Convert.ToString(ddlYear.SelectedItem.Text).Trim();
            }
            if (ddlMonth.Items.Count > 0)
            {
                examMonth = Convert.ToString(ddlMonth.SelectedValue).Trim();
            }
            ds.Reset();
            ds.Dispose();
            ds.Clear();
            if (!string.IsNullOrEmpty(CollegeCode) && !string.IsNullOrEmpty(examYear) && !string.IsNullOrEmpty(examMonth))
            {
                string strquery = "select distinct e.batchFrom as BatchYear from exmtt e,exmtt_det ex where  ex.coll_code='" + CollegeCode + "' and ex.exam_code=e.exam_code  and e.exam_month='" + examMonth + "' and e.exam_year='" + examYear + "' order by BatchYear desc";
                ds = da.select_method_wo_parameter(strquery, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //for (int b = 0; b < ds.Tables[0].Rows.Count; b++)
                //{
                //    ddlBatchYear.Items.Add( Convert.ToString(ds.Tables[0].Rows[b]["BatchYear"]).Trim());
                //}
                ddlBatchYear.DataSource = ds;
                ddlBatchYear.DataTextField = "BatchYear";
                ddlBatchYear.DataValueField = "BatchYear";
                ddlBatchYear.DataBind();
                ddlBatchYear.SelectedIndex = 0;
            }
            if (cbBatchYear.Checked)
            {
                if (ddlBatchYear.Items.Count > 0)
                {
                    ddlBatchYear.Enabled = true;
                }
                else
                {
                    ddlCourse.Enabled = false;
                }
            }
            else
            {
                ddlBatchYear.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = Convert.ToString(ex);
        }
    }

    public void loaddegree()
    {
        try
        {
            ddlCourse.Items.Clear();
            DataSet dss1 = new DataSet();
            string batch = string.Empty;
            //if (Session["collegecode"] != null)
            //{
            //    CollegeCode = Convert.ToString(Session["collegecode"]).Trim();
            //}
            examYear = string.Empty;
            examMonth = string.Empty;
            collegeCode = CollegeCode;
            if (ddlYear.Items.Count > 0)
            {
                examYear = Convert.ToString(ddlYear.SelectedItem.Text).Trim();
            }
            if (ddlMonth.Items.Count > 0)
            {
                examMonth = Convert.ToString(ddlMonth.SelectedValue).Trim();
            }
            if (cbBatchYear.Checked == true)
            {
                if (ddlBatchYear.Items.Count > 0)
                {
                    batch = " and e.batchFrom='" + Convert.ToString(ddlBatchYear.SelectedItem.Text).Trim() + "'";
                }
            }
            if (!string.IsNullOrEmpty(CollegeCode) && !string.IsNullOrEmpty(examYear) && !string.IsNullOrEmpty(examMonth))
            {
                string s1 = "select distinct c.course_Name as CourseName,c.course_id as CourseId from exmtt e,exmtt_det ex,department dpt,degree d ,course c,Subject s where c.Course_id=d.Course_Id and dpt.dept_code =d.dept_code and  d.degree_code=e.degree_code and s.subject_no=ex.subject_no and ex.coll_code='" + CollegeCode + "' and ex.exam_code=e.exam_code and e.exam_month='" + examMonth + "'and e.exam_year='" + examYear + "' " + batch + " order by CourseName";
                dss1 = da.select_method_wo_parameter(s1, "Text");
            }
            if (dss1.Tables.Count > 0 && dss1.Tables[0].Rows.Count > 0)
            {
                //for (int j1 = 0; j1 < dss1.Tables[0].Rows.Count; j1++)
                //{
                //    ddlCourse.Items.Add(new ListItem( Convert.ToString(dss1.Tables[0].Rows[j1]["CourseName"]).Trim(),  Convert.ToString(dss1.Tables[0].Rows[j1]["CourseId"]).Trim()));
                //}
                ddlCourse.DataSource = dss1;
                ddlCourse.DataTextField = "CourseName";
                ddlCourse.DataValueField = "CourseId";
                ddlCourse.DataBind();
                ddlCourse.SelectedIndex = 0;
            }
            if (cbCourse.Checked)
            {
                if (ddlCourse.Items.Count > 0)
                {
                    ddlCourse.Enabled = true;
                }
                else
                {
                    ddlCourse.Enabled = false;
                }
            }
            else
            {
                ddlCourse.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = Convert.ToString(ex);
        }
    }

    public void loaddepartment()
    {
        try
        {
            ddlDepartment.Items.Clear();
            DataSet dss = new DataSet();
            string degree = string.Empty;
            string batch = string.Empty;
            examYear = string.Empty;
            examMonth = string.Empty;
            collegeCode = CollegeCode;
            if (ddlYear.Items.Count > 0)
            {
                examYear = Convert.ToString(ddlYear.SelectedItem.Text).Trim();
            }
            if (ddlMonth.Items.Count > 0)
            {
                examMonth = Convert.ToString(ddlMonth.SelectedValue).Trim();
            }
            if (cbBatchYear.Checked == true)
            {
                if (ddlBatchYear.Items.Count > 0)
                {
                    batch = " and e.batchFrom='" + Convert.ToString(ddlBatchYear.SelectedItem.Text).Trim() + "'";//and e.batchTo='" + Convert.ToString(ddlBatchYear.SelectedItem.Text).Trim() + "'
                }
            }
            if (cbCourse.Checked == true)
            {
                if (ddlCourse.Items.Count > 0)
                {
                    degree = " and c.course_id='" + Convert.ToString(ddlCourse.SelectedValue).Trim() + "'";
                }
            }
            if (!string.IsNullOrEmpty(CollegeCode) && !string.IsNullOrEmpty(examYear) && !string.IsNullOrEmpty(examMonth))
            {
                string s = "select distinct dpt.Dept_Name as DepartmentName,d.degree_Code as DepartmentCode from exmtt e,exmtt_det ex,department dpt,degree d ,course c where c.Course_id=d.Course_Id and dpt.dept_code =d.dept_code and  d.degree_code=e.degree_code and ex.exam_code=e.exam_code and ex.coll_code='" + CollegeCode + "' and ex.exam_code=e.exam_code  and e.exam_month='" + examMonth + "'and e.exam_year='" + examYear + "' " + batch + degree + " order by DepartmentName";
                //if (batch.Trim() != "")
                //{
                //    s = s + batch;
                //}
                //if (degree.Trim() != "")
                //{
                //    s = s + degree;
                //}
                //s = s + " order by DepartmentName";
                dss = da.select_method_wo_parameter(s, "Text");
            }
            if (dss.Tables.Count > 0 && dss.Tables[0].Rows.Count > 0)
            {
                //for (int j = 0; j < dss.Tables[0].Rows.Count; j++)
                //{
                //    ddlDepartment.Items.Add(new ListItem(Convert.ToString(dss.Tables[0].Rows[j]["DepartmentName"]).Trim(), Convert.ToString(dss.Tables[0].Rows[j]["DepartmentCode"]).Trim()));
                //}
                ddlDepartment.DataSource = dss;
                ddlDepartment.DataTextField = "DepartmentName";
                ddlDepartment.DataValueField = "DepartmentCode";
                ddlDepartment.DataBind();
                ddlDepartment.SelectedIndex = 0;
            }
            if (cbDepartment.Checked)
            {
                if (ddlDepartment.Items.Count > 0)
                {
                    ddlDepartment.Enabled = true;
                }
                else
                {
                    ddlDepartment.Enabled = false;
                }
            }
            else
            {
                ddlDepartment.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = Convert.ToString(ex);
        }
    }

    public void loadsubject()
    {
        try
        {
            DataSet dssubject = new DataSet();
            ddlSubjectName.Items.Clear();
            string degree = string.Empty;
            string batch = string.Empty;
            string course = string.Empty;
            examYear = string.Empty;
            examMonth = string.Empty;
            collegeCode = CollegeCode;
            if (ddlYear.Items.Count > 0)
            {
                examYear = Convert.ToString(ddlYear.SelectedItem.Text).Trim();
            }
            if (ddlMonth.Items.Count > 0)
            {
                examMonth = Convert.ToString(ddlMonth.SelectedValue).Trim();
            }
            if (cbBatchYear.Checked == true)
            {
                if (ddlBatchYear.Items.Count > 0)
                {
                    batch = " and e.batchFrom='" + Convert.ToString(ddlBatchYear.SelectedItem.Text).Trim() + "' ";
                }
            }
            if (cbCourse.Checked == true)
            {
                if (ddlCourse.Items.Count > 0)
                {
                    course = " and c.course_id='" + Convert.ToString(ddlCourse.SelectedValue).Trim() + "' ";
                }
            }
            if (cbDepartment.Checked == true)
            {
                if (ddlDepartment.Items.Count > 0)
                {
                    degree = " and e.degree_code='" + Convert.ToString(ddlDepartment.SelectedValue).Trim() + "' ";
                }
            }
            if (!string.IsNullOrEmpty(CollegeCode) && !string.IsNullOrEmpty(examYear) && !string.IsNullOrEmpty(examMonth))
            {
                string strquery = "select distinct rtrim(s.subject_name) as subjectname,s.subject_code as subjectcode from exmtt e,exmtt_det ex,department dpt,degree d ,course c,subject s where c.Course_id=d.Course_Id and dpt.dept_code =d.dept_code and  d.degree_code=e.degree_code and ex.exam_code=e.exam_code and s.subject_no=ex.subject_no and ex.coll_code='" + CollegeCode + "' and e.exam_month='" + examMonth + "' and e.exam_year='" + examYear + "' " + batch + course + degree + " order by subjectname,subjectcode";
                dssubject = da.select_method_wo_parameter(strquery, "Text");
            }
            if (dssubject.Tables.Count > 0 && dssubject.Tables[0].Rows.Count > 0)
            {
                //for (int j = 0; j < dssubject.Tables[0].Rows.Count; j++)
                //{
                //    ddlSubjectName.Items.Add(new ListItem(Convert.ToString(dssubject.Tables[0].Rows[j]["subjectname"]).Trim(),Convert.ToString( dssubject.Tables[0].Rows[j]["subjectcode"]).Trim()));
                //}
                ddlSubjectName.DataSource = dssubject;
                ddlSubjectName.DataTextField = "subjectname";
                ddlSubjectName.DataValueField = "subjectcode";
                ddlSubjectName.DataBind();
                ddlSubjectName.SelectedIndex = 0;
            }
            if (cbSubject.Checked)
            {
                if (ddlSubjectName.Items.Count > 0)
                {
                    ddlSubjectName.Enabled = true;
                }
                else
                {
                    ddlSubjectName.Enabled = false;
                }
            }
            else
            {
                ddlSubjectName.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = Convert.ToString(ex);
        }
    }

    public void clear()
    {
        Fpstudents.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        txtexcelname.Text = string.Empty;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        lblvalidation1.Visible = false;
        Printcontrol.Visible = false;
    }

    protected void chkindegee_CheckedChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        //loadmonth();
        BindExamMonth();
        loadbatch();
        loaddegree();
        loaddepartment();
        loadsubject();
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        loadbatch();
        loaddegree();
        loaddepartment();
        loadsubject();
    }

    protected void cbDate_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        if (cbDate.Checked == true)
        {
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
        }
        else
        {
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
        }
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        clear();
        string date1 = Convert.ToString(txtFromDate.Text).Trim();
        string date2 = Convert.ToString(txtToDate.Text).Trim();
        string[] spf = date1.Split(new Char[] { '/' });
        string[] spd = date2.Split(new Char[] { '/' });
        DateTime dt1 = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
        DateTime dt2 = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
        if (dt1 > dt2)
        {
            lblerror.Visible = true;
            lblerror.Text = "From Date Should be Less then To Date";
            txtFromDate.Text = date2;
        }
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        clear();
        string date1 = Convert.ToString(txtFromDate.Text).Trim();
        string date2 = Convert.ToString(txtToDate.Text).Trim();
        string[] spf = date1.Split(new Char[] { '/' });
        string[] spd = date2.Split(new Char[] { '/' });
        DateTime dt1 = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
        DateTime dt2 = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
        if (dt1 > dt2)
        {
            lblerror.Visible = true;
            lblerror.Text = "From Date Should be Less then To Date";
            txtFromDate.Text = date2;
        }
    }

    protected void cbBatchYear_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        ddlBatchYear.Enabled = false;
        if (cbBatchYear.Checked == true)
        {
            loadbatch();
            if (ddlBatchYear.Items.Count > 0)
            {
                ddlBatchYear.Enabled = true;
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "No Batch Year's Available";
            }
        }
        loaddegree();
        loaddepartment();
        loadsubject();
    }

    protected void ddlBatchYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        loaddegree();
        loaddepartment();
        loadsubject();
    }

    protected void cbCourse_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        ddlCourse.Enabled = false;
        if (cbCourse.Checked == true)
        {
            loaddegree();
            if (ddlCourse.Items.Count > 0)
            {
                ddlCourse.Enabled = true;
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = " No Course's Available";
            }
        }
        loaddepartment();
        loadsubject();
    }

    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        loaddepartment();
        loadsubject();
    }

    protected void cbDepartment_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        ddlDepartment.Enabled = false;
        if (cbDepartment.Checked == true)
        {
            loaddepartment();
            if (ddlDepartment.Items.Count > 0)
            {
                ddlDepartment.Enabled = true;
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "No Department's Available";
            }
        }
        loadsubject();
    }

    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        loadsubject();
    }

    protected void cbSubject_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        ddlSubjectName.Enabled = false;
        if (cbSubject.Checked == true)
        {
            loadsubject();
            if (ddlSubjectName.Items.Count > 0)
            {
                ddlSubjectName.Enabled = true;
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "No Subject's Available";
            }
        }
    }

    protected void ddlSubjectName_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (Convert.ToString(reportname).Trim().Replace(" ", "_").Trim() != "")
            {
                da.printexcelreport(Fpstudents, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {
        }
    }

    protected void btnprintmaster_Clcik(object sender, EventArgs e)
    {
        try
        {
            string strgettime = "select distinct RIGHT(CONVERT(VARCHAR,start_time,100),7) as exstart,RIGHT(CONVERT(VARCHAR,end_time,100),7) exend,exam_session from exmtt_det et,exmtt e where e.exam_code=et.exam_code and e.Exam_month='" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "' and e.Exam_year='" + Convert.ToString(ddlYear.SelectedValue).Trim() + "'";
            DataSet dstime = da.select_method_wo_parameter(strgettime, "text");
            string foorenoontime = string.Empty;
            string afterenoontime = string.Empty;
            if (dstime.Tables.Count > 0 && dstime.Tables[0].Rows.Count > 0)
            {
                for (int t = 0; t < dstime.Tables[0].Rows.Count; t++)
                {
                    string getss = Convert.ToString(dstime.Tables[0].Rows[t]["exam_session"]).Trim().ToLower();
                    if (getss.Contains("f"))
                    {
                        foorenoontime = Convert.ToString(dstime.Tables[0].Rows[t]["exstart"]).Trim() + " TO " + Convert.ToString(dstime.Tables[0].Rows[t]["exend"]).Trim();
                    }
                    else
                    {
                        afterenoontime = Convert.ToString(dstime.Tables[0].Rows[t]["exstart"]).Trim() + " TO " + Convert.ToString(dstime.Tables[0].Rows[t]["exend"]).Trim();
                    }
                }
            }
            lblvalidation1.Visible = false;
            string getdetails = string.Empty;
            if (cbDate.Checked == true)
            {
                getdetails = "@Date : " + Convert.ToString(txtFromDate.Text).Trim() + " To " + Convert.ToString(txtToDate.Text).Trim() + "";
            }
            if (cbBatchYear.Checked == true)
            {
                if (getdetails == "")
                {
                    getdetails = "@Batch Year : " + Convert.ToString(ddlBatchYear.SelectedItem).Trim() + "";
                }
                else
                {
                    getdetails = getdetails + "@Batch Year : " + Convert.ToString(ddlBatchYear.SelectedItem).Trim() + "";
                }
            }
            string gettype = string.Empty;
            if (cbCourse.Checked == true && cbDepartment.Checked == true)
            {
                gettype = da.GetFunction("select c.type+'%'+c.Edu_Level from Degree d,course c where d.Course_Id=c.Course_Id and d.Degree_Code='" + Convert.ToString(ddlDepartment.SelectedValue).Trim() + "'");
                if (gettype.Trim() != "" && gettype != null)
                {
                    string[] spt = gettype.Split('%');
                    if (spt.GetUpperBound(0) >= 1)
                    {
                        if (Convert.ToString(spt[0]).Trim() != "")
                        {
                            if (Convert.ToString(spt[0]).Trim().ToLower() == "day")
                            {
                                if (!Convert.ToString(ddlCourse.SelectedItem).Trim().ToLower().Contains("m.phil"))
                                {
                                    gettype = "Regular - " + Convert.ToString(spt[1]).Trim() + " -";
                                }
                                else
                                {
                                    gettype = "Regular - ";
                                }
                            }
                            else
                            {
                                gettype = spt[0] + " - " + Convert.ToString(spt[1]).Trim() + " -";
                            }
                        }
                    }
                }
                if (getdetails == "")
                {
                    getdetails = "@Degree : " + gettype + " " + Convert.ToString(ddlCourse.SelectedItem).Trim() + " - " + Convert.ToString(ddlDepartment.SelectedItem).Trim() + "";
                }
                else
                {
                    getdetails = getdetails + "@Degree : " + gettype + " " + Convert.ToString(ddlCourse.SelectedItem).Trim() + " - " + Convert.ToString(ddlDepartment.SelectedItem).Trim() + "";
                }
            }
            if (cbCourse.Checked == true && cbDepartment.Checked == false)
            {
                gettype = da.GetFunction("select c.type+'%'+c.Edu_Level from Degree d,course c where d.Course_Id=c.Course_Id and c.Course_Id='" + Convert.ToString(ddlCourse.SelectedValue).Trim() + "'");
                if (gettype.Trim() != "" && gettype != null)
                {
                    string[] spt = gettype.Split('%');
                    if (spt.GetUpperBound(0) >= 1)
                    {
                        if (Convert.ToString(spt[0]).Trim() != "")
                        {
                            if (Convert.ToString(spt[0]).Trim().ToLower() == "day")
                            {
                                if (!Convert.ToString(ddlCourse.SelectedItem).Trim().ToLower().Contains("m.phil"))
                                {
                                    gettype = "Regular - " + Convert.ToString(spt[1]).Trim() + " -";
                                }
                                else
                                {
                                    gettype = "Regular";
                                }
                            }
                            else
                            {
                                gettype = spt[0] + " - " + Convert.ToString(spt[1]).Trim() + " -";
                            }
                        }
                    }
                }
                if (getdetails == "")
                {
                    getdetails = "@Course : " + gettype + " " + Convert.ToString(ddlCourse.SelectedItem).Trim() + "";
                }
                else
                {
                    getdetails = getdetails + "@Degree : " + gettype + " " + Convert.ToString(ddlCourse.SelectedItem).Trim() + "";
                }
            }
            if (cbCourse.Checked == false && cbDepartment.Checked == true)
            {
                gettype = da.GetFunction("select c.type+'%'+c.Edu_Level+'%'+c.Course_Name from Degree d,course c where d.Course_Id=c.Course_Id and d.Degree_Code='" + Convert.ToString(ddlDepartment.SelectedValue).Trim() + "'");
                if (gettype.Trim() != "" && gettype != null)
                {
                    string[] spt = gettype.Split('%');
                    if (spt.GetUpperBound(0) >= 1)
                    {
                        if (Convert.ToString(spt[0]).Trim() != "")
                        {
                            if (Convert.ToString(spt[0]).Trim().ToLower() == "day")
                            {
                                gettype = "Regular - " + Convert.ToString(spt[1]).Trim() + " - " + Convert.ToString(spt[2]).Trim() + " - ";
                            }
                            else
                            {
                                gettype = spt[0] + " - " + Convert.ToString(spt[1]).Trim() + " - " + Convert.ToString(spt[2]).Trim() + " - ";
                            }
                        }
                    }
                }
                if (getdetails == "")
                {
                    getdetails = "@Department : " + gettype + " " + Convert.ToString(ddlDepartment.SelectedItem).Trim() + "";
                }
                else
                {
                    getdetails = getdetails + "@Department : " + gettype + " " + Convert.ToString(ddlDepartment.SelectedItem).Trim() + "";
                }
            }
            if (foorenoontime.Trim() != "")
            {
                if (getdetails == "")
                {
                    getdetails = "@FORENOON   [" + foorenoontime + "]";
                }
                else
                {
                    getdetails = getdetails + "@FORENOON   [" + foorenoontime + "]";
                }
            }
            if (afterenoontime.Trim() != "")
            {
                if (getdetails == "")
                {
                    getdetails = "@AFTERNOON [" + afterenoontime + "]";
                }
                else
                {
                    getdetails = getdetails + "@AFTERNOON [" + afterenoontime + "]";
                }
            }
            string degreedetails = "Office of the Controller of Examinations $TIME TABLE FOR THE EXAMINATION " + Convert.ToString(ddlMonth.SelectedItem).Trim() + " - " + Convert.ToString(ddlYear.SelectedItem).Trim() + "" + getdetails;
            string pagename = "ExamTimeTableReport.aspx";
            Printcontrol.loadspreaddetails(Fpstudents, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }
    }

    protected void RadioCHange(object sender, EventArgs e)
    {
        clear();
    }

    public void timetablereport2()
    {
        Fpstudents.Sheets[0].RowCount = 0;
        Fpstudents.Sheets[0].ColumnCount = 4;
        Fpstudents.CommandBar.Visible = false;
        Fpstudents.RowHeader.Visible = false;
        Fpstudents.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        Fpstudents.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        Fpstudents.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        Fpstudents.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        Fpstudents.Sheets[0].DefaultStyle.Font.Bold = false;
        Fpstudents.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        Fpstudents.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
        Fpstudents.Sheets[0].Columns[0].Width = 50;
        Fpstudents.Sheets[0].Columns[1].Width = 100;
        Fpstudents.Sheets[0].Columns[2].Width = 325;
        Fpstudents.Sheets[0].Columns[3].Width = 325;
        Fpstudents.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        Fpstudents.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
        Fpstudents.Sheets[0].ColumnHeader.Cells[0, 2].Text = "FORENOON";
        Fpstudents.Sheets[0].ColumnHeader.Cells[0, 3].Text = "AFTERNOON";
        Fpstudents.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
        Fpstudents.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
        Fpstudents.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
        Fpstudents.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
        FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
        style2.Font.Size = 13;
        style2.Font.Name = "Book Antiqua";
        style2.Font.Bold = true;
        style2.HorizontalAlign = HorizontalAlign.Center;
        style2.ForeColor = System.Drawing.Color.White;
        style2.BackColor = System.Drawing.Color.Teal;
        Fpstudents.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
        Fpstudents.Sheets[0].SheetName = " ";
        Fpstudents.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
        Fpstudents.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
        Fpstudents.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        Fpstudents.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        Fpstudents.Sheets[0].DefaultStyle.Font.Bold = false;

        collegeCode = CollegeCode;
        examYear = string.Empty;
        examMonth = string.Empty;
        if (ddlYear.Items.Count > 0)
        {
            examYear = Convert.ToString(ddlYear.SelectedItem.Text).Trim();
            if (string.IsNullOrEmpty(examYear) || examYear.Trim() == "0" || examYear.Trim() == "-1" || examYear.Trim().ToLower() == "all")
            {
                examYear = string.Empty;
            }
        }
        if (ddlMonth.Items.Count > 0)
        {
            examMonth = Convert.ToString(ddlMonth.SelectedValue).Trim();
            if (string.IsNullOrEmpty(examMonth) || examMonth.Trim() == "0" || examMonth.Trim() == "-1" || examMonth.Trim().ToLower() == "all")
            {
                examMonth = string.Empty;
            }
        }

        if (ddlYear.Items.Count > 0 && ddlMonth.Items.Count > 0 && !string.IsNullOrEmpty(examMonth) && !string.IsNullOrEmpty(examYear))
        {
            string order = string.Empty;
            string columnvisible = string.Empty;
            if (chkindegee.Checked == true)
            {
                columnvisible = ",e.batchFrom,dpt.Dept_Name,c.Course_Name,e.degree_code";
                order = " e.batchFrom desc,e.degree_code,dpt.Dept_Name,c.Course_Name,";
            }
            string dateval = string.Empty;
            if (cbDate.Checked == true)
            {
                string date1 = Convert.ToString(txtFromDate.Text).Trim();
                string date2 = Convert.ToString(txtToDate.Text).Trim();
                string[] spf = date1.Split(new Char[] { '/' });
                string[] spd = date2.Split(new Char[] { '/' });
                DateTime dt1 = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
                DateTime dt2 = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
                if (dt1 > dt2)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "From Date Should be Less then To Date";
                    return;
                }
                dateval = " and ex.exam_Date between '" + dt1.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "'";
            }
            string batch = string.Empty;
            if (cbBatchYear.Checked == true)
            {
                if (ddlBatchYear.Items.Count > 0)
                {
                    batch = " and e.batchfrom='" + Convert.ToString(ddlBatchYear.SelectedItem).Trim() + "'";
                }
            }
            string course = string.Empty;
            if (cbCourse.Checked == true)
            {
                if (ddlCourse.Items.Count > 0)
                {
                    course = " and c.course_id='" + Convert.ToString(ddlCourse.SelectedValue).Trim() + "'";
                }
            }
            string degree = string.Empty;
            if (cbDepartment.Checked == true)
            {
                if (ddlDepartment.Items.Count > 0)
                {
                    degree = " and e.degree_code='" + Convert.ToString(ddlDepartment.SelectedValue).Trim() + "'";
                }
            }
            string subject = string.Empty;
            if (cbSubject.Checked == true)
            {
                if (ddlSubjectName.Items.Count > 0)
                {
                    subject = " and Ltrim(rtrim(s.subject_name))='" + Convert.ToString(ddlSubjectName.SelectedItem).Trim() + "'";
                }
            }
            Hashtable hatequalsubcode = new Hashtable();
            string strquery = "select distinct s.subject_Name as SubjectName,ex.exam_session as Session ,convert(Varchar(20),ex.exam_Date,105) as ExamDate,ex.exam_Date,s.subject_code as subjectcode" + columnvisible + " ,isnull(t.Com_Subject_Code,'') as Com_Subject_Code from exmtt e,exmtt_det ex,department dpt,degree d ,course c,subject s left join tbl_equal_paper_Matching t on t.Equal_Subject_Code=s.subject_code where c.Course_id=d.Course_Id and dpt.dept_code =d.dept_code and  d.degree_code=e.degree_code and s.subject_no=ex.subject_no and ex.coll_code='" + Convert.ToString(Session["collegecode"]).Trim() + "' and ex.exam_code=e.exam_code  and e.exam_month='" + examMonth + "' and e.exam_year='" + examYear + "' " + dateval + " " + batch + " " + course + " " + degree + " " + subject + " order by " + order + "ex.exam_Date,Session desc,SubjectName ";
            DataSet ds = da.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Fpstudents.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
                btnprintmaster.Visible = true;
                lblvalidation1.Visible = false;
                int sno = 0;
                int startrow = 0;
                string tempdate = string.Empty;
                string tempdegree = string.Empty;
                string loopdate = string.Empty;
                Hashtable hatalreadyset = new Hashtable();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string getdate = Convert.ToString(ds.Tables[0].Rows[i]["ExamDate"]).Trim();
                    if (chkindegee.Checked == true)
                    {
                        string Degree = Convert.ToString(ds.Tables[0].Rows[i]["batchFrom"]).Trim() + " - " + Convert.ToString(ds.Tables[0].Rows[i]["Course_Name"]).Trim() + " - " + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]).Trim();
                        if (tempdegree != Degree)
                        {
                            Fpstudents.Sheets[0].RowCount++;
                            Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].Text = Degree;
                            Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].Font.Size = 12;
                            Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            //Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            Fpstudents.Sheets[0].SpanModel.Add(Fpstudents.Sheets[0].RowCount - 1, 1, 1, 3);
                            tempdate = string.Empty;
                            tempdegree = Degree;
                        }
                    }
                    string subjectval = Convert.ToString(ds.Tables[0].Rows[i]["subjectcode"]).Trim() + " - " + Convert.ToString(ds.Tables[0].Rows[i]["SubjectName"]).Trim();
                    string sesva = Convert.ToString(ds.Tables[0].Rows[i]["Session"]).Trim().ToLower();
                    if (loopdate != Convert.ToString(ds.Tables[0].Rows[i]["ExamDate"]).Trim())
                    {
                        startrow = Fpstudents.Sheets[0].RowCount;
                        loopdate = Convert.ToString(ds.Tables[0].Rows[i]["ExamDate"]).Trim();
                    }
                    Fpstudents.Sheets[0].Columns[0].Visible = false;
                    if (sesva.Contains("f"))
                    {
                        Fpstudents.Sheets[0].RowCount++;
                        sno++;
                        Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].CellType = txtceltype;
                        Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].Text = loopdate;
                        Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Large;
                        Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                        Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["subjectcode"]).Trim() + " - " + Convert.ToString(ds.Tables[0].Rows[i]["SubjectName"]).Trim();
                        if (Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 3].Text.Trim() == "")
                        {
                            Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 3].Text = "----";
                            Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    else
                    {
                        //if (startrow > Fpstudents.Sheets[0].RowCount - 1)
                        //{
                        //    Fpstudents.Sheets[0].RowCount++;
                        //}
                        //Fpstudents.Sheets[0].Cells[startrow, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["SubjectName"]).Trim();
                        //Fpstudents.Sheets[0].Cells[startrow, 3].Font.Size = FontUnit.Medium;
                        //Fpstudents.Sheets[0].Cells[startrow, 3].HorizontalAlign = HorizontalAlign.Left;
                        if (startrow > Fpstudents.Sheets[0].RowCount - 1)
                        {
                            Fpstudents.Sheets[0].RowCount++;
                        }
                        Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].CellType = txtceltype;
                        Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].Text = loopdate;
                        Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Large;
                        Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpstudents.Sheets[0].Cells[startrow, 3].Font.Size = FontUnit.Medium;
                        Fpstudents.Sheets[0].Cells[startrow, 3].Text = subjectval;
                        Fpstudents.Sheets[0].Cells[startrow, 3].HorizontalAlign = HorizontalAlign.Left;
                        if (Fpstudents.Sheets[0].Cells[startrow, 2].Text.Trim() == "")
                        {
                            Fpstudents.Sheets[0].Cells[startrow, 2].Text = "----";
                            Fpstudents.Sheets[0].Cells[startrow, 2].HorizontalAlign = HorizontalAlign.Center;
                        }
                        //startrow++;
                        startrow++;
                    }
                }
            }
            else
            {
                Fpstudents.Visible = false;
                lblerror.Visible = true;
                lblerror.Text = "No Record(s) Found";
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                txtexcelname.Text = string.Empty;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                lblvalidation1.Visible = false;
            }
        }
        else
        {
            Fpstudents.Visible = false;
            lblerror.Visible = true;
            lblerror.Text = "No Exam Conducted";
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            txtexcelname.Text = string.Empty;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            lblvalidation1.Visible = false;
        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (rbformat1.Checked == true)
            {
                Fpstudents.Sheets[0].RowCount = 0;
                Fpstudents.Sheets[0].ColumnCount = 3;
                Fpstudents.CommandBar.Visible = false;
                Fpstudents.RowHeader.Visible = false;
                Fpstudents.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                Fpstudents.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                Fpstudents.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                Fpstudents.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                Fpstudents.Sheets[0].DefaultStyle.Font.Bold = false;
                // Fpstudents.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                Fpstudents.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                Fpstudents.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                Fpstudents.Sheets[0].Columns[0].Width = 50;
                Fpstudents.Sheets[0].Columns[1].Width = 325;
                Fpstudents.Sheets[0].Columns[2].Width = 325;
                Fpstudents.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpstudents.Sheets[0].ColumnHeader.Cells[0, 1].Text = "FORENOON";
                Fpstudents.Sheets[0].ColumnHeader.Cells[0, 2].Text = "AFTERNOON";
                Fpstudents.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                Fpstudents.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                Fpstudents.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                collegeCode = CollegeCode;
                examYear = string.Empty;
                examMonth = string.Empty;
                if (ddlYear.Items.Count > 0)
                {
                    examYear = Convert.ToString(ddlYear.SelectedItem.Text).Trim();
                    if (string.IsNullOrEmpty(examYear) || examYear.Trim() == "0" || examYear.Trim() == "-1" || examYear.Trim().ToLower() == "all")
                    {
                        examYear = string.Empty;
                    }
                }
                if (ddlMonth.Items.Count > 0)
                {
                    examMonth = Convert.ToString(ddlMonth.SelectedValue).Trim();
                    if (string.IsNullOrEmpty(examMonth) || examMonth.Trim() == "0" || examMonth.Trim() == "-1" || examMonth.Trim().ToLower() == "all")
                    {
                        examMonth = string.Empty;
                    }
                }

                if (ddlYear.Items.Count > 0 && ddlMonth.Items.Count > 0 && !string.IsNullOrEmpty(examMonth) && !string.IsNullOrEmpty(examYear))
                {
                    string order = string.Empty;
                    string columnvisible = string.Empty;
                    if (chkindegee.Checked == true)
                    {
                        columnvisible = ",e.batchFrom,dpt.Dept_Name,c.Course_Name,e.degree_code";
                        order = " e.batchFrom desc,e.degree_code,dpt.Dept_Name,c.Course_Name,";
                    }
                    string dateval = string.Empty;
                    if (cbDate.Checked == true)
                    {
                        string date1 = Convert.ToString(txtFromDate.Text).Trim();
                        string date2 = Convert.ToString(txtToDate.Text).Trim();
                        string[] spf = date1.Split(new Char[] { '/' });
                        string[] spd = date2.Split(new Char[] { '/' });
                        DateTime dt1 = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
                        DateTime dt2 = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
                        if (dt1 > dt2)
                        {
                            lblerror.Visible = true;
                            lblerror.Text = "From Date Should be Less then To Date";
                            return;
                        }
                        dateval = " and ex.exam_Date between '" + dt1.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "'";
                    }
                    string batch = string.Empty;
                    if (cbBatchYear.Checked == true)
                    {
                        if (ddlBatchYear.Items.Count > 0)
                        {
                            batch = " and e.batchfrom='" + Convert.ToString(ddlBatchYear.SelectedItem).Trim() + "'";
                        }
                    }
                    string course = string.Empty;
                    if (cbCourse.Checked == true)
                    {
                        if (ddlCourse.Items.Count > 0)
                        {
                            course = " and c.course_id='" + Convert.ToString(ddlCourse.SelectedValue).Trim() + "'";
                        }
                    }
                    string degree = string.Empty;
                    if (cbDepartment.Checked == true)
                    {
                        if (ddlDepartment.Items.Count > 0)
                        {
                            degree = " and e.degree_code='" + Convert.ToString(ddlDepartment.SelectedValue).Trim() + "'";
                        }
                    }
                    string subject = string.Empty;
                    if (cbSubject.Checked == true)
                    {
                        if (ddlSubjectName.Items.Count > 0)
                        {
                            subject = " and Ltrim(rtrim(s.subject_name))='" + Convert.ToString(ddlSubjectName.SelectedItem).Trim() + "'";
                        }
                    }
                    Hashtable hatequalsubcode = new Hashtable();
                    string strquery = "select distinct s.subject_Name as SubjectName,ex.exam_session as Session ,convert(Varchar(20),ex.exam_Date,105) as ExamDate,ex.exam_Date,s.subject_code as subjectcode" + columnvisible + " ,isnull(t.Com_Subject_Code,'') as Com_Subject_Code from exmtt e,exmtt_det ex,department dpt,degree d ,course c,subject s left join tbl_equal_paper_Matching t on t.Equal_Subject_Code=s.subject_code where c.Course_id=d.Course_Id and dpt.dept_code =d.dept_code and  d.degree_code=e.degree_code and s.subject_no=ex.subject_no and ex.coll_code='" + Convert.ToString(Session["collegecode"]).Trim() + "' and ex.exam_code=e.exam_code  and e.exam_month='" + examMonth + "' and e.exam_year='" + examYear + "' " + dateval + " " + batch + " " + course + " " + degree + " " + subject + " order by " + order + "ex.exam_Date,Session desc,SubjectName ";
                    DataSet ds = da.select_method_wo_parameter(strquery, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string getcode = "select distinct t.Com_Subject_Code,s.subject_name,t.Equal_Subject_Code from tbl_equal_paper_Matching t,subject s where t.Com_Subject_Code=s.subject_code order by t.Com_Subject_Code,t.Equal_Subject_Code desc,s.subject_name";
                        DataSet dsequalcode = da.select_method_wo_parameter(getcode, "text");
                        string tempcode = string.Empty;
                        string seteqvalus = string.Empty;
                        if (dsequalcode.Tables.Count > 0 && dsequalcode.Tables[0].Rows.Count > 0)
                        {
                            for (int eq = 0; eq < dsequalcode.Tables[0].Rows.Count; eq++)
                            {
                                string comcode = Convert.ToString(dsequalcode.Tables[0].Rows[eq]["Com_Subject_Code"]).Trim();
                                string subname = Convert.ToString(dsequalcode.Tables[0].Rows[eq]["subject_name"]).Trim();
                                string seteqsubno = Convert.ToString(dsequalcode.Tables[0].Rows[eq]["Equal_Subject_Code"]).Trim();
                                if (comcode != tempcode)
                                {
                                    seteqvalus = seteqsubno;
                                    tempcode = comcode;
                                }
                                if (!hatequalsubcode.Contains(comcode))
                                {
                                    hatequalsubcode.Add(comcode, seteqvalus);
                                }
                                else
                                {
                                    string setval = Convert.ToString(hatequalsubcode[comcode]).Trim();
                                    setval = setval + " , " + seteqsubno;
                                    hatequalsubcode[comcode] = setval;
                                }
                            }
                        }
                        Fpstudents.Visible = true;
                        lblrptname.Visible = true;
                        txtexcelname.Visible = true;
                        btnxl.Visible = true;
                        btnprintmaster.Visible = true;
                        lblvalidation1.Visible = false;
                        int sno = 0;
                        int startrow = 0;
                        string tempdate = string.Empty;
                        string tempdegree = string.Empty;
                        Hashtable hatalreadyset = new Hashtable();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string getdate = Convert.ToString(ds.Tables[0].Rows[i]["ExamDate"]).Trim();
                            if (chkindegee.Checked == true)
                            {
                                string Degree = Convert.ToString(ds.Tables[0].Rows[i]["batchFrom"]).Trim() + " - " + Convert.ToString(ds.Tables[0].Rows[i]["Course_Name"]).Trim() + " - " + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]).Trim();
                                if (tempdegree != Degree)
                                {
                                    Fpstudents.Sheets[0].RowCount++;
                                    Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 0].Text = Degree;
                                    Fpstudents.Sheets[0].SpanModel.Add(Fpstudents.Sheets[0].RowCount - 1, 0, 1, 3);
                                    tempdate = string.Empty;
                                    tempdegree = Degree;
                                }
                            }
                            if (tempdate != getdate)
                            {
                                sno++;
                                Fpstudents.Sheets[0].RowCount++;
                                Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["ExamDate"]).Trim();
                                Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Large;
                                Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                Fpstudents.Sheets[0].SpanModel.Add(Fpstudents.Sheets[0].RowCount - 1, 1, 1, 3);
                                Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                // Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 0].BackColor = Color.LightSeaGreen;
                                // Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].BackColor = Color.LightSeaGreen;
                                tempdate = getdate;
                                startrow = Fpstudents.Sheets[0].RowCount;
                            }
                            string subjectval = Convert.ToString(ds.Tables[0].Rows[i]["subjectcode"]).Trim() + " - " + Convert.ToString(ds.Tables[0].Rows[i]["SubjectName"]).Trim();
                            //string alchede = subjectval + '-' + tempdate+'-'+tempdegree;
                            //if (chkindegee.Checked == false)
                            //{
                            //    alchede = subjectval +"- "+ tempdate;
                            //}
                            //if (!hatalreadyset.Contains(alchede))
                            //{
                            bool setflag = false;
                            string comsub = Convert.ToString(ds.Tables[0].Rows[i]["Com_Subject_Code"]).Trim();
                            //if (comsub.Trim() != "" && comsub != null)
                            //{
                            //    if (comsub.Trim().ToLower() != Convert.ToString(ds.Tables[0].Rows[i]["subjectcode"]).Trim().ToLower())
                            //    {
                            //        setflag = true;
                            //    }
                            //}
                            if (setflag == false)
                            {
                                string sesva = Convert.ToString(ds.Tables[0].Rows[i]["Session"]).Trim().ToLower();
                                if (sesva.Contains("f"))
                                {
                                    Fpstudents.Sheets[0].RowCount++;
                                    Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["SubjectName"]).Trim();
                                    Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    // Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].BackColor = Color.LightSlateGray;
                                    if (comsub.Trim() != "" && comsub != null)
                                    {
                                        if (hatequalsubcode.Contains(comsub))
                                        {
                                            subjectval = Convert.ToString(hatequalsubcode[comsub]).Trim();
                                        }
                                    }
                                    else
                                    {
                                        subjectval = Convert.ToString(ds.Tables[0].Rows[i]["subjectcode"]).Trim();
                                    }
                                    Fpstudents.Sheets[0].RowCount++;
                                    Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 1].Text = subjectval;
                                }
                                else
                                {
                                    if (startrow > Fpstudents.Sheets[0].RowCount - 1)
                                    {
                                        Fpstudents.Sheets[0].RowCount++;
                                    }
                                    Fpstudents.Sheets[0].Cells[startrow, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["SubjectName"]).Trim();
                                    Fpstudents.Sheets[0].Cells[startrow, 2].Font.Size = FontUnit.Medium;
                                    //  Fpstudents.Sheets[0].Cells[startrow, 2].BackColor = Color.LightSlateGray;
                                    Fpstudents.Sheets[0].Cells[startrow, 2].HorizontalAlign = HorizontalAlign.Left;
                                    startrow++;
                                    if (startrow > Fpstudents.Sheets[0].RowCount - 1)
                                    {
                                        Fpstudents.Sheets[0].RowCount++;
                                    }
                                    if (comsub.Trim() != "" && comsub != null)
                                    {
                                        if (hatequalsubcode.Contains(comsub))
                                        {
                                            subjectval = Convert.ToString(hatequalsubcode[comsub]).Trim();
                                        }
                                    }
                                    else
                                    {
                                        subjectval = Convert.ToString(ds.Tables[0].Rows[i]["subjectcode"]).Trim();
                                    }
                                    Fpstudents.Sheets[0].Cells[startrow, 2].Text = subjectval;
                                    Fpstudents.Sheets[0].Cells[startrow, 2].HorizontalAlign = HorizontalAlign.Left;
                                    startrow++;
                                }
                                // hatalreadyset.Add(alchede, alchede);
                                //}
                            }
                        }
                    }
                    else
                    {
                        Fpstudents.Visible = false;
                        lblerror.Visible = true;
                        lblerror.Text = "No Record(s) Found";
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        txtexcelname.Text = string.Empty;
                        btnxl.Visible = false;
                        btnprintmaster.Visible = false;
                        lblvalidation1.Visible = false;
                    }
                }
                else
                {
                    Fpstudents.Visible = false;
                    lblerror.Visible = true;
                    lblerror.Text = "No Exam Conducted";
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    txtexcelname.Text = string.Empty;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    lblvalidation1.Visible = false;
                }
            }
            else
            {
                timetablereport2();
            }
            Fpstudents.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            Fpstudents.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
            Fpstudents.Sheets[0].PageSize = Fpstudents.Sheets[0].RowCount;
            Double widthva = 0;
            for (int c = 0; c < Fpstudents.Sheets[0].ColumnCount; c++)
            {
                if (Fpstudents.Sheets[0].Columns[c].Visible == true)
                {
                    widthva = widthva + Convert.ToDouble(Fpstudents.Sheets[0].Columns[c].Width);
                }
            }
            widthva = Math.Round(widthva, 0, MidpointRounding.AwayFromZero);
            widthva = widthva + 20;
            Fpstudents.Width = Convert.ToInt32(widthva);
            Double heighva = 20;
            if (Fpstudents.Sheets[0].RowCount > 500)
            {
                heighva = 1000;
            }
            else
            {
                heighva = Fpstudents.Sheets[0].RowCount * 20 + 25;
            }
            heighva = Math.Round(heighva, 0, MidpointRounding.AwayFromZero);
            heighva = heighva + 20;
            Fpstudents.Height = Convert.ToInt32(heighva);
            Fpstudents.Sheets[0].AutoPostBack = true;
            Fpstudents.Sheets[0].PageSize = Fpstudents.Sheets[0].RowCount;
            Fpstudents.SaveChanges();
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = Convert.ToString(ex);
        }
    }

    //protected void btnView_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Connection();
    //        if (cb_date_type.Checked == false)
    //        {
    //            if (cbDepartment.Checked == false && cbDate.Checked == false && cbBatchYear.Checked == false && cbSubject.Checked == false && cbCourse.Checked == false)
    //            {
    //                Fpstudents.Sheets[0].RowCount = 0;
    //                CollegeCode = Convert.ToString(Session["collegecode"]);
    //                if (ddlMonth.SelectedValue == "0")
    //                {
    //                    lblerror.Text = "Select the Month";
    //                    lblerror.Visible = true;
    //                    Button1.Enabled = false;
    //                    btnView.Enabled = false;
    //                }
    //                else if (ddlYear.SelectedValue == "0")
    //                {
    //                    lblerror.Text = "Select the Year";
    //                    lblerror.Visible = true;
    //                    Button1.Enabled = false;
    //                    btnView.Enabled = false;
    //                }
    //                else
    //                {
    //                    Button1.Enabled = true;
    //                    btnView.Enabled = true;
    //                    lblerror.Visible = false;
    //                    int SNo = 0;
    //                    string temp =string.Empty;
    //                    //SqlCommand cmd = new SqlCommand("ProcExamTimeTableReport", con);
    //                    //cmd.CommandType = CommandType.StoredProcedure;
    //                    //cmd.Parameters.AddWithValue("@ExamMonth", Convert.ToString(ddlMonth.SelectedIndex));
    //                    //cmd.Parameters.AddWithValue("@ExamYear", Convert.ToString(ddlYear.SelectedItem.Text));
    //                    //cmd.Parameters.AddWithValue("@CollegeCode", );
    //                    //SqlDataAdapter da = new SqlDataAdapter(cmd);
    //                    //DataSet ds = new DataSet();
    //                    hat.Clear();
    //                    hat.Add("ExamMonth", Convert.ToString(ddlMonth.SelectedIndex));
    //                    hat.Add("ExamYear", Convert.ToString(ddlYear.SelectedItem.Text));
    //                    hat.Add("CollegeCode", CollegeCode);
    //                    ds = da.select_method("ProcExamTimeTableReport", hat, "sp");
    //                    Fpstudents.Sheets[0].RowCount = 1;
    //                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //                    {
    //                        Panel2.Visible = false;
    //                        pnlFilter.Visible = true;
    //                        Fpstudents.Visible = true;
    //                        btnView.Visible = false;
    //                        cbBatchYear.Visible = true;
    //                        cbCourse.Visible = true;
    //                        cbDate.Visible = true;
    //                        cbDepartment.Visible = true;
    //                        cbSubject.Visible = true;
    //                        Button1.Visible = true;
    //                        ddlBatchYear.Visible = true;
    //                        ddlCourse.Visible = true;
    //                        ddlDepartment.Visible = true;
    //                        ddlSubjectName.Visible = true;
    //                        txtFromDate.Visible = true;
    //                        txtToDate.Visible = true;
    //                        int count = 0;
    //                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //                        {
    //                            if (temp != Convert.ToString(ds.Tables[0].Rows[i]["ExamDate"]))
    //                            {
    //                                SNo = SNo + 1;
    //                            }
    //                            count = Fpstudents.Sheets[0].RowCount - 1;
    //                            Fpstudents.Sheets[0].Cells[count, 0].Text = Convert.ToString(SNo).Trim();
    //                            Fpstudents.Sheets[0].Cells[count, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["ExamDate"]).Trim();
    //                            Fpstudents.Sheets[0].Cells[count, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Session"]).Trim();
    //                            Fpstudents.Sheets[0].Cells[count, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["BatchYear"]).Trim();
    //                            Fpstudents.Sheets[0].Cells[count, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["CourseName"]).Trim();
    //                            Fpstudents.Sheets[0].Cells[count, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["DeptAcronym"]).Trim();
    //                            Fpstudents.Sheets[0].Cells[count, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["SubjectName"]).Trim();
    //                            Fpstudents.Sheets[0].RowCount++;
    //                            temp = Convert.ToString(ds.Tables[0].Rows[i]["ExamDate"]).Trim();
    //                        }
    //                        lblerror.Visible = false;
    //                        lblrptname.Visible = true;
    //                        txtexcelname.Visible = true;
    //                        btnxl.Visible = true;
    //                        btnprintmaster.Visible = true;
    //                        lblvalidation1.Visible = false;
    //                    }
    //                    else
    //                    {
    //                        Fpstudents.Visible = false;
    //                        lblerror.Visible = true;
    //                        lblerror.Text = "No Record(s) Found";
    //                        lblrptname.Visible = false;
    //                        txtexcelname.Visible = false;
    //                        txtexcelname.Text =string.Empty;
    //                        btnxl.Visible = false;
    //                        btnprintmaster.Visible = false;
    //                        lblvalidation1.Visible = false;
    //                    }
    //                    string course =string.Empty;
    //                    if (ds.Tables[1].Rows.Count > 0)
    //                    {
    //                        string dept =string.Empty;
    //                        ddlDepartment.Items.Clear();
    //                        ddlSubjectName.Items.Clear();
    //                        ddlCourse.Items.Clear();
    //                        for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
    //                        {
    //                            if (dept != ds.Tables[1].Rows[j]["DepartmentCode"].ToString())
    //                            {
    //                                ddlDepartment.Items.Add(new ListItem(ds.Tables[1].Rows[j]["DepartmentName"].ToString(), ds.Tables[1].Rows[j]["DepartmentCode"].ToString()));
    //                            }
    //                            dept = Convert.ToString(ds.Tables[1].Rows[j]["DepartmentCode"]);
    //                            ddlSubjectName.Items.Add(new ListItem(ds.Tables[1].Rows[j]["SubjectName"].ToString() + "-" + ds.Tables[1].Rows[j]["Acronym"].ToString(), ds.Tables[1].Rows[j]["SubjectNo"].ToString()));
    //                        }
    //                    }
    //                    ddlCourse.Items.Clear();
    //                    if (ds.Tables[3].Rows.Count > 0)
    //                    {
    //                        for (int j1 = 0; j1 < ds.Tables[3].Rows.Count; j1++)
    //                        {
    //                            if (course != ds.Tables[3].Rows[j1]["CourseName"].ToString())
    //                            {
    //                                ddlCourse.Items.Add(new ListItem(ds.Tables[3].Rows[j1]["CourseName"].ToString(), ds.Tables[3].Rows[j1]["CourseId"].ToString()));
    //                            }
    //                            course = ds.Tables[3].Rows[j1]["CourseName"].ToString();
    //                        }
    //                    }
    //                    ddlBatchYear.Items.Clear();
    //                    if (ds.Tables[2].Rows.Count > 0)
    //                    {
    //                        for (int k = 0; k < ds.Tables[2].Rows.Count; k++)
    //                        {
    //                            ddlBatchYear.Items.Add(ds.Tables[2].Rows[k]["BatchYear"].ToString());
    //                        }
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                Connection();
    //                CollegeCode = Session["collegecode"].ToString();
    //                string FromDate;
    //                string ToDate;
    //                DateTime dt1;
    //                DateTime dt2;
    //                DataView dv = new DataView();
    //                DataView dv1 = new DataView();
    //                DataSet ds11 = new DataSet();
    //                string frmdate;
    //                string tdate;
    //                string[] frmdate1;
    //                string[] tdate1;
    //                int date = 0;
    //                int sub = 0;
    //                int batch = 0;
    //                int dept = 0;
    //                int Course = 0;
    //                int date_order = 0;
    //                if (cbBatchYear.Checked == true)
    //                {
    //                    batch = 1;
    //                }
    //                if (cbDate.Checked == true)
    //                {
    //                    date = 1;
    //                }
    //                if (cbSubject.Checked == true)
    //                {
    //                    sub = 1;
    //                }
    //                if (cbDepartment.Checked == true)
    //                {
    //                    dept = 1;
    //                }
    //                if (cbCourse.Checked == true)
    //                {
    //                    Course = 1;
    //                }
    //                frmdate = txtFromDate.Text.ToString();
    //                tdate = txtToDate.Text.ToString();
    //                frmdate1 = frmdate.Split(new char[] { '-' });
    //                tdate1 = tdate.Split(new char[] { '-' });
    //                FromDate = frmdate1[1].ToString() + "-" + frmdate1[0].ToString() + "-" + frmdate1[2].ToString();
    //                ToDate = tdate1[1].ToString() + "-" + tdate1[0].ToString() + "-" + tdate1[2].ToString();
    //                dt1 = Convert.ToDateTime(frmdate1[1].ToString() + "-" + frmdate1[0].ToString() + "-" + frmdate1[2].ToString());
    //                dt2 = Convert.ToDateTime(tdate1[1].ToString() + "-" + tdate1[0].ToString() + "-" + tdate1[2].ToString());
    //                int SNo = 0;
    //                string temp =string.Empty;
    //                //SqlCommand cmd = new SqlCommand("ProcExamTimeTableFilterReport", con);
    //                //cmd.CommandType = CommandType.StoredProcedure;
    //                //cmd.Parameters.AddWithValue("@ExamMonth", ddlMonth.SelectedIndex.ToString());
    //                //cmd.Parameters.AddWithValue("@ExamYear", ddlYear.SelectedItem.Text.ToString());
    //                //cmd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                //cmd.Parameters.AddWithValue("@DegreeCode", ddlDepartment.SelectedValue.ToString());
    //                //cmd.Parameters.AddWithValue("@SubjectNo", ddlSubjectName.SelectedValue.ToString());
    //                //cmd.Parameters.AddWithValue("@BatchYear", ddlBatchYear.SelectedItem.Text.ToString());
    //                //cmd.Parameters.AddWithValue("@FromDate", FromDate);
    //                //cmd.Parameters.AddWithValue("@ToDate", ToDate);
    //                //cmd.Parameters.AddWithValue("@Date", date.ToString());
    //                //cmd.Parameters.AddWithValue("@Dept", dept.ToString());
    //                //cmd.Parameters.AddWithValue("@Sub", sub.ToString());
    //                //cmd.Parameters.AddWithValue("@Batch", batch.ToString());
    //                //SqlDataAdapter da = new SqlDataAdapter(cmd);
    //                DataSet ds1; //= new DataSet();
    //                DAccess2 da1 = new DAccess2();
    //                hat.Clear();
    //                hat.Add("ExamMonth", ddlMonth.SelectedIndex.ToString());
    //                hat.Add("ExamYear", ddlYear.SelectedItem.Text.ToString());
    //                hat.Add("CollegeCode", CollegeCode);
    //                hat.Add("DegreeCode", ddlDepartment.SelectedValue.ToString());
    //                hat.Add("SubjectNo", ddlSubjectName.SelectedValue.ToString());
    //                hat.Add("BatchYear", ddlBatchYear.SelectedItem.Text.ToString());
    //                hat.Add("CourseId", ddlCourse.SelectedValue.ToString());
    //                hat.Add("FromDate", FromDate);
    //                hat.Add("ToDate", ToDate);
    //                hat.Add("Date", date.ToString());
    //                hat.Add("Dept", dept.ToString());
    //                hat.Add("Sub", sub.ToString());
    //                hat.Add("Batch", batch.ToString());
    //                hat.Add("Course", Course.ToString());
    //                ds1 = da1.select_method("ProcExamTimeTableFilterReport", hat, "sp");
    //                Fpstudents.Sheets[0].RowCount = 1;
    //                if (ds1.Tables[0].Rows.Count > 0)
    //                {
    //                    Fpstudents.Visible = true;
    //                    int count = 0;
    //                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
    //                    {
    //                        if (temp != ds1.Tables[0].Rows[i]["ExamDate"].ToString())
    //                        {
    //                            SNo = SNo + 1;
    //                        }
    //                        count = Fpstudents.Sheets[0].RowCount - 1;
    //                        Fpstudents.Sheets[0].Cells[count, 0].Text = SNo.ToString();
    //                        Fpstudents.Sheets[0].Cells[count, 1].Text = ds1.Tables[0].Rows[i]["ExamDate"].ToString();
    //                        Fpstudents.Sheets[0].Cells[count, 2].Text = ds1.Tables[0].Rows[i]["Session"].ToString();
    //                        Fpstudents.Sheets[0].Cells[count, 3].Text = ds1.Tables[0].Rows[i]["BatchYear"].ToString();
    //                        Fpstudents.Sheets[0].Cells[count, 4].Text = ds1.Tables[0].Rows[i]["CourseName"].ToString();
    //                        Fpstudents.Sheets[0].Cells[count, 5].Text = ds1.Tables[0].Rows[i]["DeptAcronym"].ToString();
    //                        Fpstudents.Sheets[0].Cells[count, 6].Text = ds1.Tables[0].Rows[i]["SubjectName"].ToString();
    //                        Fpstudents.Sheets[0].RowCount++;
    //                        temp = ds1.Tables[0].Rows[i]["ExamDate"].ToString();
    //                    }
    //                    lblerror.Visible = false;
    //                    Fpstudents.Visible = true;
    //                    Fpstudents.Sheets[0].Visible = true;
    //                    lblrptname.Visible = true;
    //                    txtexcelname.Visible = true;
    //                    btnxl.Visible = true;
    //                    btnprintmaster.Visible = true;
    //                    lblvalidation1.Visible = false;
    //                }
    //                else
    //                {
    //                    Fpstudents.Visible = false;
    //                    lblerror.Visible = true;
    //                    lblerror.Text = "No Record(s) Found";
    //                    lblrptname.Visible = false;
    //                    txtexcelname.Visible = false;
    //                    txtexcelname.Text =string.Empty;
    //                    btnxl.Visible = false;
    //                    btnprintmaster.Visible = false;
    //                    lblvalidation1.Visible = false;
    //                }
    //            }
    //        }
    //        else  // Added By ******************** jairam 29-10-2014 *****************
    //        {
    //            if (cb_date_type.Checked == true)
    //            {
    //                DataSet ds111 = new DataSet();
    //                if (cbDepartment.Checked == false && cbDate.Checked == false && cbBatchYear.Checked == false && cbSubject.Checked == false && cbCourse.Checked == false)
    //                {
    //                    CollegeCode = Session["collegecode"].ToString();
    //                    if (ddlMonth.SelectedValue == "0")
    //                    {
    //                        lblerror.Text = "Select the Month";
    //                        lblerror.Visible = true;
    //                        Button1.Enabled = false;
    //                        btnView.Enabled = false;
    //                    }
    //                    else if (ddlYear.SelectedValue == "0")
    //                    {
    //                        lblerror.Text = "Select the Year";
    //                        lblerror.Visible = true;
    //                        Button1.Enabled = false;
    //                        btnView.Enabled = false;
    //                    }
    //                    else
    //                    {
    //                        Button1.Enabled = true;
    //                        btnView.Enabled = true;
    //                        lblerror.Visible = false;
    //                        int SNo1 = 0;
    //                        string temp1 =string.Empty;
    //                        //SqlCommand cmd = new SqlCommand("ProcExamTimeTableReport", con);
    //                        //cmd.CommandType = CommandType.StoredProcedure;
    //                        //cmd.Parameters.AddWithValue("@ExamMonth", ddlMonth.SelectedIndex.ToString());
    //                        //cmd.Parameters.AddWithValue("@ExamYear", ddlYear.SelectedItem.Text.ToString());
    //                        //cmd.Parameters.AddWithValue("@CollegeCode", );
    //                        //SqlDataAdapter da = new SqlDataAdapter(cmd);
    //                        //DataSet ds = new DataSet();
    //                        hat.Clear();
    //                        hat.Add("ExamMonth", ddlMonth.SelectedIndex.ToString());
    //                        hat.Add("ExamYear", ddlYear.SelectedItem.Text.ToString());
    //                        hat.Add("CollegeCode", CollegeCode);
    //                        ds = da.select_method("ProcExamTimeTableReport", hat, "sp");
    //                        Fpstudents.Sheets[0].RowCount = 1;
    //                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //                        {
    //                            Panel2.Visible = false;
    //                            pnlFilter.Visible = true;
    //                            Fpstudents.Visible = true;
    //                            btnView.Visible = false;
    //                            cbBatchYear.Visible = true;
    //                            cbCourse.Visible = true;
    //                            cbDate.Visible = true;
    //                            cbDepartment.Visible = true;
    //                            cbSubject.Visible = true;
    //                            Button1.Visible = true;
    //                            ddlBatchYear.Visible = true;
    //                            ddlCourse.Visible = true;
    //                            ddlDepartment.Visible = true;
    //                            ddlSubjectName.Visible = true;
    //                            txtFromDate.Visible = true;
    //                            txtToDate.Visible = true;
    //                            string session_query = "select distinct RIGHT(CONVERT(VARCHAR, start_time, 100), 7)as start_time,RIGHT(CONVERT(VARCHAR, end_time, 100), 7) as end_time,CONVERT(varchar(10), exam_date,105)as exam_date ,exam_session from exmtt_det where coll_code =" + Session["collegecode"].ToString() + " order by exam_date,exam_session desc";
    //                            ds111.Clear();
    //                            ds111 = da.select_method_wo_parameter(session_query, "Text");
    //                            ArrayList arr = new ArrayList();
    //                            DataView dv11 = new DataView();
    //                            arr.Add("F.N");
    //                            arr.Add("A.N");
    //                            if (ds111.Tables[0].Rows.Count > 0)
    //                            {
    //                                for (int jj = 0; jj < ds111.Tables[0].Rows.Count; jj++)
    //                                {
    //                                    string exam_date = ds111.Tables[0].Rows[jj]["exam_date"].ToString();
    //                                    string session = ds111.Tables[0].Rows[jj]["exam_session"].ToString();
    //                                    string start_time = ds111.Tables[0].Rows[jj]["start_time"].ToString();
    //                                    string end_time = ds111.Tables[0].Rows[jj]["end_time"].ToString();
    //                                    for (int kk = 0; kk < arr.Count; kk++)
    //                                    {
    //                                        ds.Tables[0].DefaultView.RowFilter = "Session='" + arr[kk].ToString() + "' and ExamDate='" + exam_date.ToString() + "'";
    //                                        dv11 = ds.Tables[0].DefaultView;
    //                                        if (dv11.Count > 0)
    //                                        {
    //                                            temp1 =string.Empty;
    //                                            int count = 0;
    //                                            Fpstudents.Sheets[0].SpanModel.Add(Fpstudents.Sheets[0].RowCount - 1, 0, 1, Fpstudents.Sheets[0].ColumnCount);
    //                                            Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 0].Text = exam_date.ToString() + " " + arr[kk].ToString() + "  [" + start_time.ToString() + " to " + end_time.ToString() + "]";
    //                                            Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 0].Font.Bold = true;
    //                                            Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
    //                                            Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
    //                                            Fpstudents.Sheets[0].RowCount++;
    //                                            for (int i = 0; i < dv11.Count; i++)
    //                                            {
    //                                                if (temp1 != dv11[i]["ExamDate"].ToString())
    //                                                {
    //                                                    SNo1 = SNo1 + 1;
    //                                                }
    //                                                count = Fpstudents.Sheets[0].RowCount - 1;
    //                                                Fpstudents.Sheets[0].Cells[count, 0].Text = SNo1.ToString();
    //                                                Fpstudents.Sheets[0].Cells[count, 1].Text = dv11[i]["ExamDate"].ToString();
    //                                                Fpstudents.Sheets[0].Cells[count, 2].Text = dv11[i]["Session"].ToString();
    //                                                Fpstudents.Sheets[0].Cells[count, 3].Text = dv11[i]["BatchYear"].ToString();
    //                                                Fpstudents.Sheets[0].Cells[count, 4].Text = dv11[i]["CourseName"].ToString();
    //                                                Fpstudents.Sheets[0].Cells[count, 5].Text = dv11[i]["DeptAcronym"].ToString();
    //                                                Fpstudents.Sheets[0].Cells[count, 6].Text = dv11[i]["SubjectName"].ToString();
    //                                                Fpstudents.Sheets[0].RowCount++;
    //                                                temp1 = dv11[i]["ExamDate"].ToString();
    //                                            }
    //                                        }
    //                                    }
    //                                }
    //                                lblerror.Visible = false;
    //                                lblrptname.Visible = true;
    //                                txtexcelname.Visible = true;
    //                                btnxl.Visible = true;
    //                                btnprintmaster.Visible = true;
    //                                lblvalidation1.Visible = false;
    //                            }
    //                        }
    //                        else
    //                        {
    //                            Fpstudents.Visible = false;
    //                            lblerror.Visible = true;
    //                            lblerror.Text = "No Record(s) Found";
    //                            lblrptname.Visible = false;
    //                            txtexcelname.Visible = false;
    //                            txtexcelname.Text =string.Empty;
    //                            btnxl.Visible = false;
    //                            btnprintmaster.Visible = false;
    //                            lblvalidation1.Visible = false;
    //                        }
    //                        string course =string.Empty;
    //                        if (ds.Tables[1].Rows.Count > 0)
    //                        {
    //                            string dept1 =string.Empty;
    //                            ddlDepartment.Items.Clear();
    //                            ddlSubjectName.Items.Clear();
    //                            ddlCourse.Items.Clear();
    //                            for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
    //                            {
    //                                if (dept1 != ds.Tables[1].Rows[j]["DepartmentCode"].ToString())
    //                                {
    //                                    ddlDepartment.Items.Add(new ListItem(ds.Tables[1].Rows[j]["DepartmentName"].ToString(), ds.Tables[1].Rows[j]["DepartmentCode"].ToString()));
    //                                }
    //                                dept1 = ds.Tables[1].Rows[j]["DepartmentCode"].ToString();
    //                                ddlSubjectName.Items.Add(new ListItem(ds.Tables[1].Rows[j]["SubjectName"].ToString() + "-" + ds.Tables[1].Rows[j]["Acronym"].ToString(), ds.Tables[1].Rows[j]["SubjectNo"].ToString()));
    //                            }
    //                        }
    //                        ddlCourse.Items.Clear();
    //                        if (ds.Tables[3].Rows.Count > 0)
    //                        {
    //                            for (int j1 = 0; j1 < ds.Tables[3].Rows.Count; j1++)
    //                            {
    //                                if (course != ds.Tables[3].Rows[j1]["CourseName"].ToString())
    //                                {
    //                                    ddlCourse.Items.Add(new ListItem(ds.Tables[3].Rows[j1]["CourseName"].ToString(), ds.Tables[3].Rows[j1]["CourseId"].ToString()));
    //                                }
    //                                course = ds.Tables[3].Rows[j1]["CourseName"].ToString();
    //                            }
    //                        }
    //                        ddlBatchYear.Items.Clear();
    //                        if (ds.Tables[2].Rows.Count > 0)
    //                        {
    //                            for (int k = 0; k < ds.Tables[2].Rows.Count; k++)
    //                            {
    //                                ddlBatchYear.Items.Add(ds.Tables[2].Rows[k]["BatchYear"].ToString());
    //                            }
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    Connection();
    //                    CollegeCode = Session["collegecode"].ToString();
    //                    string FromDate;
    //                    string ToDate;
    //                    DateTime dt1;
    //                    DateTime dt2;
    //                    DataView dv = new DataView();
    //                    DataView dv1 = new DataView();
    //                    DataSet ds11 = new DataSet();
    //                    string frmdate;
    //                    string tdate;
    //                    string[] frmdate1;
    //                    string[] tdate1;
    //                    int date = 0;
    //                    int sub = 0;
    //                    int batch = 0;
    //                    int dept = 0;
    //                    int Course = 0;
    //                    int date_order = 0;
    //                    if (cbBatchYear.Checked == true)
    //                    {
    //                        batch = 1;
    //                    }
    //                    if (cbDate.Checked == true)
    //                    {
    //                        date = 1;
    //                    }
    //                    if (cbSubject.Checked == true)
    //                    {
    //                        sub = 1;
    //                    }
    //                    if (cbDepartment.Checked == true)
    //                    {
    //                        dept = 1;
    //                    }
    //                    if (cbCourse.Checked == true)
    //                    {
    //                        Course = 1;
    //                    }
    //                    frmdate = txtFromDate.Text.ToString();
    //                    tdate = txtToDate.Text.ToString();
    //                    frmdate1 = frmdate.Split(new char[] { '-' });
    //                    tdate1 = tdate.Split(new char[] { '-' });
    //                    FromDate = frmdate1[1].ToString() + "-" + frmdate1[0].ToString() + "-" + frmdate1[2].ToString();
    //                    ToDate = tdate1[1].ToString() + "-" + tdate1[0].ToString() + "-" + tdate1[2].ToString();
    //                    dt1 = Convert.ToDateTime(frmdate1[1].ToString() + "-" + frmdate1[0].ToString() + "-" + frmdate1[2].ToString());
    //                    dt2 = Convert.ToDateTime(tdate1[1].ToString() + "-" + tdate1[0].ToString() + "-" + tdate1[2].ToString());
    //                    int SNo = 0;
    //                    string temp =string.Empty;
    //                    //SqlCommand cmd = new SqlCommand("ProcExamTimeTableFilterReport", con);
    //                    //cmd.CommandType = CommandType.StoredProcedure;
    //                    //cmd.Parameters.AddWithValue("@ExamMonth", ddlMonth.SelectedIndex.ToString());
    //                    //cmd.Parameters.AddWithValue("@ExamYear", ddlYear.SelectedItem.Text.ToString());
    //                    //cmd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                    //cmd.Parameters.AddWithValue("@DegreeCode", ddlDepartment.SelectedValue.ToString());
    //                    //cmd.Parameters.AddWithValue("@SubjectNo", ddlSubjectName.SelectedValue.ToString());
    //                    //cmd.Parameters.AddWithValue("@BatchYear", ddlBatchYear.SelectedItem.Text.ToString());
    //                    //cmd.Parameters.AddWithValue("@FromDate", FromDate);
    //                    //cmd.Parameters.AddWithValue("@ToDate", ToDate);
    //                    //cmd.Parameters.AddWithValue("@Date", date.ToString());
    //                    //cmd.Parameters.AddWithValue("@Dept", dept.ToString());
    //                    //cmd.Parameters.AddWithValue("@Sub", sub.ToString());
    //                    //cmd.Parameters.AddWithValue("@Batch", batch.ToString());
    //                    //SqlDataAdapter da = new SqlDataAdapter(cmd);
    //                    DataSet ds1; //= new DataSet();
    //                    DAccess2 da1 = new DAccess2();
    //                    hat.Clear();
    //                    hat.Add("ExamMonth", ddlMonth.SelectedIndex.ToString());
    //                    hat.Add("ExamYear", ddlYear.SelectedItem.Text.ToString());
    //                    hat.Add("CollegeCode", CollegeCode);
    //                    hat.Add("DegreeCode", ddlDepartment.SelectedValue.ToString());
    //                    hat.Add("SubjectNo", ddlSubjectName.SelectedValue.ToString());
    //                    hat.Add("BatchYear", ddlBatchYear.SelectedItem.Text.ToString());
    //                    hat.Add("CourseId", ddlCourse.SelectedValue.ToString());
    //                    hat.Add("FromDate", FromDate);
    //                    hat.Add("ToDate", ToDate);
    //                    hat.Add("Date", date.ToString());
    //                    hat.Add("Dept", dept.ToString());
    //                    hat.Add("Sub", sub.ToString());
    //                    hat.Add("Batch", batch.ToString());
    //                    hat.Add("Course", Course.ToString());
    //                    ds1 = da1.select_method("ProcExamTimeTableFilterReport", hat, "sp");
    //                    Fpstudents.Sheets[0].RowCount = 1;
    //                    if (ds1.Tables[0].Rows.Count > 0)
    //                    {
    //                        string session_query = "select distinct RIGHT(CONVERT(VARCHAR, start_time, 100), 7)as start_time,RIGHT(CONVERT(VARCHAR, end_time, 100), 7) as end_time,CONVERT(varchar(10), exam_date,105)as exam_date ,exam_session from exmtt_det where coll_code =" + Session["collegecode"].ToString() + " order by exam_date,exam_session desc";
    //                        ds11.Clear();
    //                        ds11 = da.select_method_wo_parameter(session_query, "Text");
    //                        if (cbDate.Checked == true)
    //                        {
    //                            while (dt2 >= dt1)
    //                            {
    //                                if (ds11.Tables[0].Rows.Count > 0)
    //                                {
    //                                    ds11.Tables[0].DefaultView.RowFilter = "exam_date ='" + dt1.ToString("dd-MM-yyyy") + "'";
    //                                    dv = ds11.Tables[0].DefaultView;
    //                                    if (dv.Count > 0)
    //                                    {
    //                                        for (int dd = 0; dd < dv.Count; dd++)
    //                                        {
    //                                            string session = dv[dd]["exam_session"].ToString();
    //                                            string start_time = dv[dd]["start_time"].ToString();
    //                                            string end_time = dv[dd]["end_time"].ToString();
    //                                            if (ds1.Tables[0].Rows.Count > 0)
    //                                            {
    //                                                ds1.Tables[0].DefaultView.RowFilter = "Session='" + session + "' and ExamDate='" + dt1.ToString("dd-MM-yyyy") + "'";
    //                                                dv1 = ds1.Tables[0].DefaultView;
    //                                                if (dv1.Count > 0)
    //                                                {
    //                                                    string session_value =string.Empty;
    //                                                    string exam_date = dt1.ToString("dd/MM/yyyy");
    //                                                    if (session == "F.N")
    //                                                    {
    //                                                        session_value = "FORENOON";
    //                                                    }
    //                                                    else if (session == "A.N")
    //                                                    {
    //                                                        session_value = "AFTERNOON";
    //                                                    }
    //                                                    int count = 0;
    //                                                    Fpstudents.Sheets[0].SpanModel.Add(Fpstudents.Sheets[0].RowCount - 1, 0, 1, Fpstudents.Sheets[0].ColumnCount);
    //                                                    Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 0].Text = exam_date.ToString() + " " + session_value.ToString() + "  [" + start_time.ToString() + " to " + end_time.ToString() + "]";
    //                                                    Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 0].Font.Bold = true;
    //                                                    Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
    //                                                    Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
    //                                                    Fpstudents.Sheets[0].RowCount++;
    //                                                    string temp1 =string.Empty;
    //                                                    for (int dd1 = 0; dd1 < dv1.Count; dd1++)
    //                                                    {
    //                                                        if (temp1 != dv1[dd1]["ExamDate"].ToString())
    //                                                        {
    //                                                            SNo = SNo + 1;
    //                                                        }
    //                                                        count = Fpstudents.Sheets[0].RowCount - 1;
    //                                                        Fpstudents.Sheets[0].Cells[count, 0].Text = SNo.ToString();
    //                                                        Fpstudents.Sheets[0].Cells[count, 1].Text = dv1[dd1]["ExamDate"].ToString();
    //                                                        Fpstudents.Sheets[0].Cells[count, 2].Text = dv1[dd1]["Session"].ToString();
    //                                                        Fpstudents.Sheets[0].Cells[count, 3].Text = dv1[dd1]["BatchYear"].ToString();
    //                                                        Fpstudents.Sheets[0].Cells[count, 4].Text = dv1[dd1]["CourseName"].ToString();
    //                                                        Fpstudents.Sheets[0].Cells[count, 5].Text = dv1[dd1]["DeptAcronym"].ToString();
    //                                                        Fpstudents.Sheets[0].Cells[count, 6].Text = dv1[dd1]["SubjectName"].ToString();
    //                                                        Fpstudents.Sheets[0].RowCount++;
    //                                                        temp1 = dv1[dd1]["ExamDate"].ToString();
    //                                                    }
    //                                                }
    //                                            }
    //                                        }
    //                                    }
    //                                }
    //                                dt1 = dt1.AddDays(1);
    //                            }
    //                            Fpstudents.Sheets[0].RowCount--;
    //                            Fpstudents.Sheets[0].PageSize = Fpstudents.Sheets[0].RowCount * 20 + 25;
    //                            Fpstudents.Height = Fpstudents.Sheets[0].RowCount * 20 + 25;
    //                            lblerror.Visible = false;
    //                            Fpstudents.Visible = true;
    //                            Fpstudents.Sheets[0].Visible = true;
    //                            lblrptname.Visible = true;
    //                            txtexcelname.Visible = true;
    //                            btnxl.Visible = true;
    //                            btnprintmaster.Visible = true;
    //                            lblvalidation1.Visible = false;
    //                        }
    //                        else
    //                        {
    //                            ArrayList arr = new ArrayList();
    //                            DataView dv11 = new DataView();
    //                            arr.Add("F.N");
    //                            arr.Add("A.N");
    //                            int SNo11 = 0;
    //                            if (ds11.Tables[0].Rows.Count > 0)
    //                            {
    //                                for (int jj = 0; jj < ds11.Tables[0].Rows.Count; jj++)
    //                                {
    //                                    string exam_date = ds11.Tables[0].Rows[jj]["exam_date"].ToString();
    //                                    string session = ds11.Tables[0].Rows[jj]["exam_session"].ToString();
    //                                    string start_time = ds11.Tables[0].Rows[jj]["start_time"].ToString();
    //                                    string end_time = ds11.Tables[0].Rows[jj]["end_time"].ToString();
    //                                    for (int kk = 0; kk < arr.Count; kk++)
    //                                    {
    //                                        ds1.Tables[0].DefaultView.RowFilter = "Session='" + arr[kk].ToString() + "' and ExamDate='" + exam_date.ToString() + "'";
    //                                        dv11 = ds1.Tables[0].DefaultView;
    //                                        if (dv11.Count > 0)
    //                                        {
    //                                            string temp1 =string.Empty;
    //                                            int count = 0;
    //                                            Fpstudents.Sheets[0].SpanModel.Add(Fpstudents.Sheets[0].RowCount - 1, 0, 1, Fpstudents.Sheets[0].ColumnCount);
    //                                            Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 0].Text = exam_date.ToString() + " " + arr[kk].ToString() + "  [" + start_time.ToString() + " to " + end_time.ToString() + "]";
    //                                            Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 0].Font.Bold = true;
    //                                            Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
    //                                            Fpstudents.Sheets[0].Cells[Fpstudents.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
    //                                            Fpstudents.Sheets[0].RowCount++;
    //                                            for (int i = 0; i < dv11.Count; i++)
    //                                            {
    //                                                if (temp1 != dv11[i]["ExamDate"].ToString())
    //                                                {
    //                                                    SNo11 = SNo11 + 1;
    //                                                }
    //                                                count = Fpstudents.Sheets[0].RowCount - 1;
    //                                                Fpstudents.Sheets[0].Cells[count, 0].Text = SNo11.ToString();
    //                                                Fpstudents.Sheets[0].Cells[count, 1].Text = dv11[i]["ExamDate"].ToString();
    //                                                Fpstudents.Sheets[0].Cells[count, 2].Text = dv11[i]["Session"].ToString();
    //                                                Fpstudents.Sheets[0].Cells[count, 3].Text = dv11[i]["BatchYear"].ToString();
    //                                                Fpstudents.Sheets[0].Cells[count, 4].Text = dv11[i]["CourseName"].ToString();
    //                                                Fpstudents.Sheets[0].Cells[count, 5].Text = dv11[i]["DeptAcronym"].ToString();
    //                                                Fpstudents.Sheets[0].Cells[count, 6].Text = dv11[i]["SubjectName"].ToString();
    //                                                Fpstudents.Sheets[0].RowCount++;
    //                                                temp1 = dv11[i]["ExamDate"].ToString();
    //                                            }
    //                                        }
    //                                    }
    //                                }
    //                                lblerror.Visible = false;
    //                                Fpstudents.Visible = true;
    //                                Fpstudents.Sheets[0].Visible = true;
    //                                lblrptname.Visible = true;
    //                                txtexcelname.Visible = true;
    //                                btnxl.Visible = true;
    //                                btnprintmaster.Visible = true;
    //                                lblvalidation1.Visible = false;
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        Fpstudents.Visible = false;
    //                        lblerror.Visible = true;
    //                        lblerror.Text = "No Record(s) Found";
    //                        lblrptname.Visible = false;
    //                        txtexcelname.Visible = false;
    //                        txtexcelname.Text =string.Empty;
    //                        btnxl.Visible = false;
    //                        btnprintmaster.Visible = false;
    //                        lblvalidation1.Visible = false;
    //                    }
    //                }
    //            }
    //        } // End ******************** jairam 29-10-2014 *****************
    //        Fpstudents.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
    //        Fpstudents.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
    //        Fpstudents.Sheets[0].PageSize = Fpstudents.Sheets[0].RowCount;
    //        if (Fpstudents.Sheets[0].RowCount > 500)
    //        {
    //            Fpstudents.Height = 1000;
    //        }
    //        else
    //        {
    //            Fpstudents.Height = Fpstudents.Sheets[0].RowCount * 20 + 25;
    //        }
    //        Fpstudents.Width = 721;
    //        Fpstudents.Sheets[0].PageSize = Fpstudents.Sheets[0].RowCount;
    //        Fpstudents.SaveChanges();
    //    }
    //    catch (Exception ex)
    //    {
    //        lblerror.Visible = true;
    //        lblerror.Text = Convert.ToString(ex);
    //    }
    //}

}