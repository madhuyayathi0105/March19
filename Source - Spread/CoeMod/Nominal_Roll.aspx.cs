using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gios.Pdf;
using wc = System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Configuration;

public partial class Nominal_Roll : System.Web.UI.Page
{
    ArrayList addvalue = new ArrayList();
    DAccess2 da = new DAccess2();

    DataSet ds = new DataSet();
    DataTable dt = new DataTable();
    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();

    Hashtable hat = new Hashtable();
    Hashtable ht = new Hashtable();
    //static Hashtable ht = new Hashtable();
    static Hashtable HashFloor = new Hashtable();
    static Hashtable HashDate = new Hashtable();
    static Hashtable HasSession = new Hashtable();
    static Hashtable Hashhall = new Hashtable();
    static Hashtable boundvl = new Hashtable();
    static Hashtable Hashdenm = new Hashtable();
    static Hashtable Hasdegree = new Hashtable();
    static Hashtable Hasroll = new Hashtable();
    static Hashtable hasbatch = new Hashtable();
    static Hashtable hassubno = new Hashtable();

    string collegeCode = string.Empty;
    string qryCollege = string.Empty;

    string qryHallNo = string.Empty;
    string hallNo = string.Empty;

    string qry = string.Empty;
    string qryDegreeCode = string.Empty;
    string DegreeCode = string.Empty;

    string examDate = string.Empty;
    string examDates = string.Empty;
    string examSession = string.Empty;
    string examSessions = string.Empty;

    string qryExamDate = string.Empty;
    string qryExamSession = string.Empty;

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
            lblerror.Visible = false;
            lblnorec.Visible = false;
            if (!IsPostBack)
            {
                Label1.Visible = false;
                txtSize.Visible = false;
                divHall.Visible = false;
                divPhasing.Visible = false;
                rptprint1.Visible = false;
                rdbtnsubject.Checked = true;
                divStudentWise.Visible = true;
                FSNominee.Visible = false;
                divCourse.Visible = true;
                lblCourse.Visible = true;
                chkNeedSubjectTotal.Checked = true;
                chkconsolidate.Checked = false;
                chkNeedSubjectTotal.Visible = false;
                chkNeedSubjectTotal.Checked = false;
                chkWithoutRegularArrear.Visible = false;
                chkWithoutRegularArrear.Checked = false;
                chkIncludeDepartmentWise.Visible = false;
                chkIncludeDepartmentWise.Checked = false;
                if (ddltype.Items.Count > 0)
                {
                    ddltype.SelectedIndex = 0;
                }
                MonthandYear();
                //LoadDateSession();
                BindExamDateSession();
                Bindcollege();
                BindBatch();
                bindcourse();
                Bindhallno();
                txtcourse.Attributes.Add("readonly", "readonly");
                txtsub.Attributes.Add("readonly", "readonly");
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void MonthandYear()
    {
        try
        {
            ddlExamMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
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
            int year;
            year = Convert.ToInt16(DateTime.Today.Year);
            ddlExamYear.Items.Clear();
            for (int l = 0; l <= 7; l++)
            {
                ddlExamYear.Items.Add(Convert.ToString(year - l));
            }
            ddlExamYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        }
        catch
        {
        }
    }

    //public void LoadDateSession()
    //{
    //    try
    //    {
    //        if (ddlExamYear.SelectedIndex != 0 && ddlExamMonth.SelectedIndex != 0)
    //        {
    //            string s = "select distinct convert(varchar(20),et.exam_date,105) as ExamDate, et.exam_session,et.exam_date from exmtt_det et,exmtt e where et.exam_code=e.exam_code and  e.exam_Month='" + ddlExamMonth.SelectedValue.ToString() + "' and e.Exam_Year='" + ddlExamYear.SelectedItem.Text.ToString() + "' order by et.exam_date,et.exam_session";
    //            ds = da.select_method_wo_parameter(s, "txt");
    //            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //            {
    //                ht.Clear();
    //                ht.Clear();
    //                ddlDate.Enabled = true;
    //                ddlSession.Enabled = true;
    //                ddlDate.Items.Clear();
    //                ddlSession.Items.Clear();
    //                ddlDate.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
    //                ddlSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
    //                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //                {
    //                    if (ht.Count > 0)
    //                    {
    //                        if (ht.Contains(ds.Tables[0].Rows[i]["ExamDate"].ToString()))
    //                        {
    //                        }
    //                        else
    //                        {
    //                            ddlDate.Items.Add(ds.Tables[0].Rows[i]["ExamDate"].ToString());
    //                            ht.Add(ds.Tables[0].Rows[i]["ExamDate"].ToString(), ds.Tables[0].Rows[i]["ExamDate"].ToString());
    //                        }
    //                    }
    //                    else
    //                    {
    //                        ddlDate.Items.Add(ds.Tables[0].Rows[i]["ExamDate"].ToString());
    //                        ht.Add(ds.Tables[0].Rows[i]["ExamDate"].ToString(), ds.Tables[0].Rows[i]["ExamDate"].ToString());
    //                    }
    //                    if (ht.Count > 0)
    //                    {
    //                        if (ht.Contains(ds.Tables[0].Rows[i]["exam_session"].ToString()))
    //                        {
    //                        }
    //                        else
    //                        {
    //                            ddlSession.Items.Add(ds.Tables[0].Rows[i]["exam_session"].ToString());
    //                            ht.Add(ds.Tables[0].Rows[i]["exam_session"].ToString(), ds.Tables[0].Rows[i]["exam_session"].ToString());
    //                        }
    //                    }
    //                    else
    //                    {
    //                        ddlSession.Items.Add(ds.Tables[0].Rows[i]["exam_session"].ToString());
    //                        ht.Add(ds.Tables[0].Rows[i]["exam_session"].ToString(), ds.Tables[0].Rows[i]["exam_session"].ToString());
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                ddlDate.Items.Clear();
    //                ddlSession.Items.Clear();
    //                ddlDate.Enabled = false;
    //                ddlSession.Enabled = false;
    //            }
    //        }
    //        else
    //        {
    //            ddlDate.Items.Clear();
    //            ddlSession.Items.Clear();
    //            ddlDate.Enabled = false;
    //            ddlSession.Enabled = false;
    //        }
    //        if (Convert.ToInt16(ddlExamMonth.SelectedValue) == 0 || ddlExamYear.SelectedIndex == 0)
    //        {
    //            ButtonGo.Enabled = false;
    //        }
    //        else if (ddlDate.Enabled == false || ddlSession.Enabled == false)
    //        {
    //            ButtonGo.Enabled = false;
    //        }
    //        else
    //        {
    //            ButtonGo.Enabled = true;
    //        }
    //        if (chklistsub.Items.Count > 0)
    //        {
    //            for (int i = 0; i < chklistsub.Items.Count; i++)
    //            {
    //                int cout = 0;
    //                cout++;
    //                chklistsub.Items[i].Selected = true;
    //                txtsub.Text = "--Select--";
    //            }
    //        }
    //        else
    //        {
    //            txtsub.Text = "--Select--";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    /// <summary>
    /// Developed By Malang Raja
    /// </summary>
    private void BindExamDateSession()
    {
        try
        {
            cblExamDate.Items.Clear();
            ddlExamDate.Items.Clear();
            cblExamSession.Items.Clear();
            ddlExamSession.Items.Clear();
            ds.Clear();

            chkExamDate.Checked = false;
            txtExamDate.Text = "--Select--";
            chkExamSession.Checked = false;
            txtExamSession.Text = "--Select--";

            string ExamMonth = string.Empty;
            string ExamYear = string.Empty;
            collegeCode = string.Empty;
            qryCollege = string.Empty;
            qryDegreeCode = string.Empty;
            string qryExamDates = string.Empty;
            string qryExamMonth = string.Empty;
            string qryExamYear = string.Empty;
            DataTable dtExamDate = new DataTable();
            DataTable dtExamSession = new DataTable();

            if (ddlExamYear.Items.Count > 0)
            {
                ExamYear = string.Empty;
                foreach (ListItem li in ddlExamYear.Items)
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
                    qryExamYear = " and e.Exam_Year in(" + ExamYear + ")";
                }
            }
            if (ddlExamMonth.Items.Count > 0)
            {
                ExamMonth = string.Empty;
                foreach (ListItem li in ddlExamMonth.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(ExamMonth))
                        {
                            ExamMonth = "'" + li.Value + "'";
                        }
                        else
                        {
                            ExamMonth += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(ExamMonth))
                {
                    qryExamMonth = " and e.exam_Month in(" + ExamMonth + ")";
                }
            }
            if (!string.IsNullOrEmpty(qryExamMonth) && !string.IsNullOrEmpty(ExamMonth) && !string.IsNullOrEmpty(qryExamYear) && !string.IsNullOrEmpty(ExamYear))
            {
                qry = "select distinct convert(varchar(20),et.exam_date,105) as ExamDate,convert(varchar(20),et.exam_date,103) as ExamDateDDMMYYYY,LTRIM(RTRIM(ISNULL(et.exam_session,''))) as exam_session,et.exam_date from exmtt_det et,exmtt e where et.exam_code=e.exam_code " + qryCollege + qryExamYear + qryExamMonth + qryDegreeCode + " order by et.exam_date,exam_session desc";//and  e.exam_Month='11' and e.Exam_Year='2016' and et.coll_code in(15,14,13) and e.degree_code in(52)
                ds.Clear();
                ds = da.select_method_wo_parameter(qry, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    dtExamDate = ds.Tables[0].DefaultView.ToTable(true, "ExamDate", "ExamDateDDMMYYYY", "exam_date");
                    dtExamSession = ds.Tables[0].DefaultView.ToTable(true, "exam_session");
                }
            }
            if (dtExamDate.Rows.Count > 0)
            {
                cblExamDate.DataSource = dtExamDate;
                cblExamDate.DataTextField = "ExamDate";
                cblExamDate.DataValueField = "ExamDateDDMMYYYY";
                cblExamDate.DataBind();
                checkBoxListselectOrDeselect(cblExamDate, false);
                CallCheckboxListChange(chkExamDate, cblExamDate, txtExamDate, lblExamDate.Text, "--Select--");
                txtExamDate.Enabled = true;

                ddlExamDate.DataSource = dtExamDate;
                ddlExamDate.DataTextField = "ExamDate";
                ddlExamDate.DataValueField = "ExamDateDDMMYYYY";
                ddlExamDate.DataBind();
                ddlExamDate.SelectedIndex = 0;
                ddlExamDate.Enabled = true;
            }
            else
            {
                ddlExamDate.Items.Clear();
                cblExamDate.Items.Clear();
                ddlExamDate.Enabled = false;
                chkExamDate.Checked = false;
                txtExamDate.Text = "--Select--";
                txtExamDate.Enabled = false;
            }
            if (dtExamSession.Rows.Count > 0)
            {
                cblExamSession.DataSource = dtExamSession;
                cblExamSession.DataTextField = "exam_session";
                cblExamSession.DataValueField = "exam_session";
                cblExamSession.DataBind();
                checkBoxListselectOrDeselect(cblExamSession, false);
                CallCheckboxListChange(chkExamSession, cblExamSession, txtExamSession, lblExamSession.Text, "--Select--");
                txtExamSession.Enabled = true;

                ddlExamSession.DataSource = dtExamSession;
                ddlExamSession.DataTextField = "exam_session";
                ddlExamSession.DataValueField = "exam_session";
                ddlExamSession.DataBind();
                ddlExamSession.Enabled = true;
                ddlExamSession.SelectedIndex = 0;
            }
            else
            {
                ddlExamSession.Items.Clear();
                cblExamSession.Items.Clear();
                ddlExamSession.Enabled = false;
                chkExamSession.Checked = false;
                txtExamSession.Text = "--Select--";
                txtExamSession.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #region Added By Malang Raja On Nov 04 2016

    public void Bindcollege()
    {
        try
        {
            string columnfield = string.Empty;
            string group_user = Convert.ToString(Session["group_code"]);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            hat.Clear();
            hat.Add("column_field", Convert.ToString(columnfield));
            DataSet dsprint = da.select_method("bind_college", hat, "sp");
            cblCollege.Items.Clear();
            chkCollege.Checked = false;
            txtCollege.Text = "--Select--";
            if (dsprint.Tables[0].Rows.Count > 0)
            {
                cblCollege.DataSource = dsprint;
                cblCollege.DataTextField = "collname";
                cblCollege.DataValueField = "college_code";
                cblCollege.DataBind();
                foreach (ListItem li in cblCollege.Items)
                {
                    li.Selected = true;
                }
            }
            else
            {
                //errmsg.Text = "Set college rights to the staff";
                //errmsg.Visible = true;
                //return;
            }
            CallCheckboxListChange(chkCollege, cblCollege, txtCollege, lblCollege.Text, "--Select--");
        }
        catch (Exception ex)
        {
        }
    }

    public void BindBatch()
    {
        try
        {
            cblBatch.Items.Clear();
            ddlBatch.Items.Clear();
            chkBatch.Checked = false;
            txtBatch.Text = "--Select--";
            ds.Clear();
            string collegeCodes = string.Empty;
            //streamNames = string.Empty;
            //eduLevels = string.Empty;
            //qryStream = string.Empty;
            //qryEduLevel = string.Empty;
            if (cblCollege.Items.Count > 0)
            {
                collegeCodes = getCblSelectedValue(cblCollege);
            }
            //if (cblStream.Items.Count > 0)
            //{
            //    streamNames = getCblSelectedText(cblStream);
            //    if (!string.IsNullOrEmpty(streamNames))
            //    {
            //        qryStream = " and LTRIM(RTRIM(ISNULL(c.type,''))) in(" + streamNames + ")";
            //    }
            //}
            //if (ddlEduLevel.Items.Count > 0)
            //{
            //    eduLevels = string.Empty;
            //    foreach (ListItem li in ddlEduLevel.Items)
            //    {
            //        if (li.Selected)
            //        {
            //            if (string.IsNullOrEmpty(eduLevels))
            //            {
            //                eduLevels = "'" + li.Text + "'";
            //            }
            //            else
            //            {
            //                eduLevels += ",'" + li.Text + "'";
            //            }
            //        }
            //    }
            //    if (!string.IsNullOrEmpty(eduLevels))
            //    {
            //        qryEduLevel = " and c.Edu_Level in(" + eduLevels + ")";
            //    }
            //}
            //--and LTRIM(RTRIM(ISNULL(c.type,''))) in('aided') and r.college_code in(14) and c.Edu_Level in('pg')
            if (!string.IsNullOrEmpty(collegeCodes))
            {
                //qry = "select distinct r.Batch_Year from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=r.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and r.degree_code=dg.Degree_Code  and r.Batch_Year<>'0' and r.Batch_Year<>-1 and r.college_code in(" + collegeCodes + ") " + " order by r.Batch_Year desc";
                qry = "select distinct r.Batch_Year from Registration r where r.Batch_Year<>'0' and r.Batch_Year<>-1 and r.college_code in(" + collegeCodes + ") order by r.Batch_Year desc";
                ds = da.select_method_wo_parameter(qry, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblBatch.DataSource = ds;
                    cblBatch.DataTextField = "Batch_Year";
                    cblBatch.DataValueField = "Batch_Year";
                    cblBatch.DataBind();
                    checkBoxListselectOrDeselect(cblBatch, false);
                    CallCheckboxListChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");

                    ddlBatch.DataSource = ds;
                    ddlBatch.DataTextField = "Batch_Year";
                    ddlBatch.DataValueField = "Batch_Year";
                    ddlBatch.DataBind();
                    ddlBatch.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion

    public void bindcourse()
    {
        try
        {
            int count = 0;
            collegeCode = string.Empty;
            if (cblCollege.Items.Count > 0)
            {
                collegeCode = getCblSelectedValue(cblCollege);
            }
            chklistcourse.Items.Clear();
            chkcourse.Checked = false;
            txtcourse.Text = "--Select--";
            ddldegree.Items.Clear();
            ds.Clear();
            ds.Reset();
            if (!string.IsNullOrEmpty(collegeCode.Trim()))
            {
                ht.Clear();
                //ds = da.select_method("select distinct degree.degree_code,course.course_name + ' - '+ department.dept_name as degree,degree.Acronym,degree.course_id  from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and  department.college_code = degree.college_code and degree.college_code in (" + collegeCode + ") and deptprivilages.Degree_code=degree.Degree_code  order by degree.degree_code ", ht, "text");
                ds.Clear();
                ds = da.select_method_wo_parameter("select distinct dg.degree_code,c.Edu_Level,dt.Dept_Name,c.course_name + ' - '+ dt.dept_name as degree,dg.Acronym,dg.course_id  from degree dg,department dt,course c,deptprivilages dp where c.course_id=dg.course_id and dt.dept_code=dg.dept_code and c.college_code = dg.college_code and  dt.college_code = dg.college_code and dg.college_code in (" + collegeCode + ") and dp.Degree_code=dg.Degree_code  order by c.Edu_Level desc,dt.Dept_Name", "text");
                if (ds.Tables.Count > 0)
                    count = ds.Tables[0].Rows.Count;
            }
            if (count > 0)
            {
                ddldegree.Items.Clear();
                chklistcourse.DataSource = ds;
                chklistcourse.DataTextField = "degree";
                chklistcourse.DataValueField = "degree_code";
                chklistcourse.DataBind();
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "degree";
                ddldegree.DataValueField = "degree_code";
                ddldegree.DataBind();
                //if (rdbtnstudent.Checked)
                //{
                //    ddldegree.Visible = true;
                //}
            }
            ddldegree.Items.Insert(0, "--Select--");
        }
        catch (Exception ex)
        {
        }
    }

    protected void chksub_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FSNominee.Visible = false;
            btngen.Visible = false;
            btnprintpdf.Visible = false;
            Printcontrol.Visible = false;
            txtsub.Text = "--Select--";
            if (chksub.Checked == true)
            {
                for (int i = 0; i < chklistsub.Items.Count; i++)
                {
                    chklistsub.Items[i].Selected = true;
                }
                int a = chklistsub.Items.Count;
                if (a > 0)
                {
                    txtsub.Text = "Subject(" + a + ")";
                    chksub.Checked = true;
                }
            }
            else
            {
                for (int i = 0; i < chklistsub.Items.Count; i++)
                {
                    chklistsub.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void chklistsub_selectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FSNominee.Visible = false;
            btngen.Visible = false;
            btnprintpdf.Visible = false;
            Printcontrol.Visible = false;
            int a = 0;
            txtsub.Text = "--Select--";
            chksub.Checked = false;
            for (int i = 0; i < chklistsub.Items.Count; i++)
            {
                if (chklistsub.Items[i].Selected == true)
                {
                    a++;
                }
            }
            if (a > 0)
            {
                txtsub.Text = "Subject(" + a + ")";
                if (a == chklistsub.Items.Count)
                {
                    chksub.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void SubjectName(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            FSNominee.Visible = false;
            btnprintpdf.Visible = false;
            btngen.Visible = false;
            Bindhallno();
            loadSubjectName();
            btngen.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void SubjectName1(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            FSNominee.Visible = false;
            btnprintpdf.Visible = false;
            btngen.Visible = false;
            Bindhallno();
            loadSubjectName();
            btngen.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    public void loadSubjectName()
    {
        try
        {
            chksub.Checked = false;
            txtsub.Text = "--Select--";
            int cout = 0;
            chklistsub.Items.Clear();
            if (cblExamDate.Items.Count > 0 && txtExamDate.Visible == true)
            {
                examDates = getCblSelectedValue(cblExamDate);
                if (!string.IsNullOrEmpty(examDates))
                {
                    qryExamDate = " and convert(varchar(20),et.exam_date,103) in(" + examDates + ")";
                }
                //else
                //{
                //    lblAlertMsg.Text = "Please Select " + lblExamDate.Text.Trim() + " And Then Proceed";
                //    divPopAlert.Visible = true;
                //    return;
                //}
            }
            else if (ddlExamDate.Items.Count > 0 && ddlExamDate.Visible == true)
            {
                examDates = string.Empty;
                foreach (ListItem li in ddlExamDate.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(examDates))
                        {
                            examDates = "'" + li.Value + "'";
                        }
                        else
                        {
                            examDates += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(examDates))
                {
                    qryExamDate = " and convert(varchar(20),et.exam_date,103) in(" + examDates + ")";
                }
                //else
                //{
                //    lblAlertMsg.Text = "Please Select " + lblExamDate.Text.Trim() + " And Then Proceed";
                //    divPopAlert.Visible = true;
                //    return;
                //}
            }
            //else
            //{
            //    lblAlertMsg.Text = "No " + lblExamDate.Text.Trim() + " Were Found";
            //    divPopAlert.Visible = true;
            //    return;
            //}
            if (cblExamSession.Items.Count > 0 && txtExamSession.Visible == true)
            {
                examSessions = getCblSelectedValue(cblExamSession);
                if (!string.IsNullOrEmpty(examSessions))
                {
                    qryExamSession = " and et.Exam_Session in(" + examSessions + ")";
                }
                //else
                //{
                //    lblAlertMsg.Text = "Please Select " + lblExamDate.Text.Trim() + " And Then Proceed";
                //    divPopAlert.Visible = true;
                //    return;
                //}
            }
            else if (ddlExamSession.Items.Count > 0 && ddlExamSession.Visible == true)
            {
                examSessions = string.Empty;
                foreach (ListItem li in ddlExamSession.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(examSessions))
                        {
                            examSessions = "'" + li.Value + "'";
                        }
                        else
                        {
                            examSessions += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(examSessions))
                {
                    qryExamSession = " and et.Exam_Session in(" + examSessions + ")";
                }
                //else
                //{
                //    lblAlertMsg.Text = "Please Select " + lblExamDate.Text.Trim() + " And Then Proceed";
                //    divPopAlert.Visible = true;
                //    return;
                //}
            }
            //else
            //{
            //    lblAlertMsg.Text = "No " + lblExamSession.Text.Trim() + " Were Found";
            //    divPopAlert.Visible = true;
            //    return;
            //}

            if (ddlExamYear.SelectedIndex != 0 && ddlExamMonth.SelectedIndex != 0)
            {
                string degree_code = string.Empty;
                for (int i = 0; i < chklistcourse.Items.Count; i++)
                {
                    if (chklistcourse.Items[i].Selected == true)
                    {
                        if (degree_code == "")
                        {
                            degree_code = chklistcourse.Items[i].Value;
                        }
                        else
                        {
                            degree_code = degree_code + "'" + "," + "'" + chklistcourse.Items[i].Value;
                        }
                    }
                }
                if (degree_code.Trim() != "")
                {
                    string sql = " select distinct s.subject_code, s.subject_name,exam_date,exam_session from exmtt_det et,exmtt e,subject s where s.subject_no=et.subject_no and  et.exam_code=e.exam_code and  e.exam_Month='" + ddlExamMonth.SelectedItem.Value + "' and e.Exam_Year='" + ddlExamYear.SelectedItem.Text + "'and e.degree_code in ('" + degree_code + "') " + qryExamDate + qryExamSession;
                    //if (ddlDate.SelectedItem.Text.Trim().ToLower() != "all")
                    //{
                    //    sql = sql + " and convert(varchar(20),et.exam_date,105)='" + ddlDate.SelectedItem.Text + "'";
                    //}
                    //if (ddlSession.SelectedItem.Text.Trim().ToLower() != "all")
                    //{
                    //    sql = sql + " and et.Exam_Session='" + ddlSession.SelectedItem.Text + "' ";
                    //}
                    sql = sql + " order by exam_date asc,exam_session desc,s.subject_code";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(sql, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        chklistsub.DataSource = ds;
                        chklistsub.DataTextField = "subject_name";
                        chklistsub.DataValueField = "subject_code";
                        chklistsub.DataBind();
                    }
                }
                else
                {
                    if (ddltype.SelectedIndex == 3)
                    {
                        string sql = " select distinct s.subject_code, s.subject_name,exam_date,exam_session from exmtt_det et,exmtt e,subject s where s.subject_no=et.subject_no and  et.exam_code=e.exam_code and  e.exam_Month='" + ddlExamMonth.SelectedItem.Value + "' and e.Exam_Year='" + ddlExamYear.SelectedItem.Text + "' " + qryExamDate + qryExamSession;// --and e.degree_code in ('" + degree_code + "')
                        //if (ddlDate.SelectedItem.Text.Trim().ToLower() != "all")
                        //{
                        //    sql = sql + " and convert(varchar(20),et.exam_date,105)='" + ddlDate.SelectedItem.Text + "'";
                        //}
                        //if (ddlSession.SelectedItem.Text.Trim().ToLower() != "all")
                        //{
                        //    sql = sql + "and et.Exam_Session='" + ddlSession.SelectedItem.Text + "' ";
                        //}
                        sql = sql + " order by exam_date asc,exam_session desc,s.subject_code";
                        ds.Clear();
                        ds = da.select_method_wo_parameter(sql, "Text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            chklistsub.DataSource = ds;
                            chklistsub.DataTextField = "subject_name";
                            chklistsub.DataValueField = "subject_code";
                            chklistsub.DataBind();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void chkcourse_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FSNominee.Visible = false;
            btngen.Visible = false;
            btnprintpdf.Visible = false;
            Printcontrol.Visible = false;
            if (chkcourse.Checked == true)
            {
                for (int i = 0; i < chklistcourse.Items.Count; i++)
                {
                    chklistcourse.Items[i].Selected = true;
                }
                int a = chklistcourse.Items.Count;
                txtcourse.Text = "Course(" + a + ")";
            }
            else
            {
                for (int i = 0; i < chklistcourse.Items.Count; i++)
                {
                    chklistcourse.Items[i].Selected = false;
                }
                txtcourse.Text = "--Select--";
            }
            loadSubjectName();
        }
        catch (Exception ex)
        {
        }
    }

    protected void chklistcourse_selectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FSNominee.Visible = false;
            btngen.Visible = false;
            btnprintpdf.Visible = false;
            Printcontrol.Visible = false;
            int a = 0;
            chkcourse.Checked = false;
            txtcourse.Text = "--Select--";
            for (int i = 0; i < chklistcourse.Items.Count; i++)
            {
                if (chklistcourse.Items[i].Selected == true)
                {
                    a++;
                }
            }
            if (a > 0)
            {
                txtcourse.Text = "Course(" + a + ")";
                if (a == chklistcourse.Items.Count)
                {
                    chkcourse.Checked = true;
                }
            }
            loadSubjectName();
        }
        catch (Exception ex)
        {
        }
    }

    protected void chkBatch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FSNominee.Visible = false;
            btngen.Visible = false;
            btnprintpdf.Visible = false;
            Printcontrol.Visible = false;
            if (chkBatch.Checked == true)
            {
                for (int i = 0; i < cblBatch.Items.Count; i++)
                {
                    cblBatch.Items[i].Selected = true;
                }
                int a = cblBatch.Items.Count;
                txtBatch.Text = "Batch(" + a + ")";
            }
            else
            {
                for (int i = 0; i < cblBatch.Items.Count; i++)
                {
                    cblBatch.Items[i].Selected = false;
                }
                txtBatch.Text = "--Select--";
            }
            bindcourse();
            loadSubjectName();
        }
        catch (Exception ex)
        {
        }
    }

    protected void cblBatch_selectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FSNominee.Visible = false;
            btngen.Visible = false;
            btnprintpdf.Visible = false;
            Printcontrol.Visible = false;
            int a = 0;
            chkBatch.Checked = false;
            txtBatch.Text = "--Select--";
            for (int i = 0; i < cblBatch.Items.Count; i++)
            {
                if (cblBatch.Items[i].Selected == true)
                {
                    a++;
                }
            }
            if (a > 0)
            {
                txtBatch.Text = "Batch(" + a + ")";
                if (a == cblBatch.Items.Count)
                {
                    chkBatch.Checked = true;
                }
            }
            bindcourse();
            loadSubjectName();
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        Printcontrol.Visible = false;
        FSNominee.Visible = false;
        btnprintpdf.Visible = false;
        btngen.Visible = false;
        bindcourse();
    }

    protected void ddlExamMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            FSNominee.Visible = false;
            btnprintpdf.Visible = false;
            btngen.Visible = false;
            //LoadDateSession();
            BindExamDateSession();
            loadSubjectName();
            Bindhallno();
            btngen.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            FSNominee.Visible = false;
            btnprintpdf.Visible = false;
            btngen.Visible = false;
            //LoadDateSession();
            BindExamDateSession();
            loadSubjectName();
            Bindhallno();
            btngen.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        Printcontrol.Visible = false;
        FSNominee.Visible = false;
        btnprintpdf.Visible = false;
        btngen.Visible = false;
    }

    protected void ButtonGo_Click(object sender, EventArgs e)
    {
        try
        {
            divPhasing.Visible = false;
            rptprint1.Visible = false;
            Printcontrol.Visible = false;
            btnprintpdf.Visible = false;
            FSNominee.Visible = false;
            if (ddltype.SelectedIndex != 2 && ddltype.SelectedIndex != 3)
            {
                FSNominee.Sheets[0].ColumnCount = 0;
                FSNominee.Sheets[0].RowCount = 0;
                FSNominee.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                FSNominee.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                FSNominee.Pager.Align = HorizontalAlign.Right;
                MyStyle.Font.Size = FontUnit.Medium;
                MyStyle.Font.Name = "Book Antiqua";
                MyStyle.Font.Bold = true;
                MyStyle.HorizontalAlign = HorizontalAlign.Center;
                MyStyle.ForeColor = Color.Black;
                MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                FSNominee.ActiveSheetView.ColumnHeader.DefaultStyle = MyStyle;
                FSNominee.Pager.Font.Bold = true;
                FSNominee.Pager.Font.Name = "Book Antiqua";
                FSNominee.Pager.ForeColor = Color.DarkGreen;
                FSNominee.Pager.BackColor = Color.AliceBlue;
                FSNominee.Sheets[0].SheetName = " ";
                FSNominee.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                FSNominee.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Center;
                FSNominee.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                FSNominee.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FSNominee.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FSNominee.Sheets[0].DefaultStyle.Font.Bold = false;
                FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
                style1.Font.Size = 12;
                style1.Font.Bold = true;
                style1.HorizontalAlign = HorizontalAlign.Center;
                style1.ForeColor = Color.Black;
                style1.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                FSNominee.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FSNominee.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FSNominee.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                FSNominee.Sheets[0].AllowTableCorner = true;

                FSNominee.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                FSNominee.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                FSNominee.Pager.Align = HorizontalAlign.Right;
                FSNominee.Pager.Font.Bold = true;
                FSNominee.Pager.Font.Name = "Book Antiqua";
                FSNominee.Pager.ForeColor = Color.DarkGreen;
                FSNominee.Pager.BackColor = Color.Beige;
                FSNominee.Pager.BackColor = Color.AliceBlue;
                FSNominee.Pager.PageCount = 5;
                FSNominee.CommandBar.Visible = false;

                FSNominee.Width = 900;
                FSNominee.Sheets[0].SheetCorner.ColumnCount = 0;
                FSNominee.CommandBar.Visible = false;
                FSNominee.Sheets[0].AutoPostBack = false;

                FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
                chkcell1.AutoPostBack = true;

                string strexamdate = string.Empty;
                //if (ddlDate.SelectedItem.ToString().Trim().ToLower() != "all")
                //{
                //    string[] spd = ddlDate.SelectedItem.ToString().Split('-');
                //    strexamdate = " and et.exam_date='" + spd[1] + '/' + spd[0] + '/' + spd[2] + "'";
                //}
                //string strsession = string.Empty;
                //if (ddlSession.SelectedItem.ToString().Trim().ToLower() != "all")
                //{
                //    strsession = " and et.exam_session='" + ddlSession.SelectedItem.ToString() + "'";
                //}
                if (cblExamDate.Items.Count > 0 && txtExamDate.Visible == true)
                {
                    examDates = getCblSelectedValue(cblExamDate);
                    if (!string.IsNullOrEmpty(examDates))
                    {
                        qryExamDate = " and convert(varchar(20),et.exam_date,103) in(" + examDates + ")";
                    }
                    else
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select " + lblExamDate.Text.Trim() + " And Then Proceed";
                        return;
                    }
                }
                else if (ddlExamDate.Items.Count > 0 && ddlExamDate.Visible == true)
                {
                    examDates = string.Empty;
                    foreach (ListItem li in ddlExamDate.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(examDates))
                            {
                                examDates = "'" + li.Value + "'";
                            }
                            else
                            {
                                examDates += ",'" + li.Value + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(examDates))
                    {
                        qryExamDate = " and convert(varchar(20),et.exam_date,103) in(" + examDates + ")";
                    }
                    else
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select " + lblExamDate.Text.Trim() + " And Then Proceed";
                        return;
                    }
                }
                else
                {
                    FSNominee.Visible = false;
                    btngen.Visible = false;
                    lblnorec.Visible = true;
                    lblnorec.Text = "No " + lblExamDate.Text.Trim() + " Were Found";
                    return;
                }
                if (cblExamSession.Items.Count > 0 && txtExamSession.Visible == true)
                {
                    examSessions = getCblSelectedValue(cblExamSession);
                    if (!string.IsNullOrEmpty(examSessions))
                    {
                        qryExamSession = " and et.Exam_Session in(" + examSessions + ")";
                    }
                    else
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select " + lblExamSession.Text.Trim() + " And Then Proceed";
                        return;
                    }
                }
                else if (ddlExamSession.Items.Count > 0 && ddlExamSession.Visible == true)
                {
                    examSessions = string.Empty;
                    foreach (ListItem li in ddlExamSession.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(examSessions))
                            {
                                examSessions = "'" + li.Value + "'";
                            }
                            else
                            {
                                examSessions += ",'" + li.Value + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(examSessions))
                    {
                        qryExamSession = " and et.Exam_Session in(" + examSessions + ")";
                    }
                    else
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select " + lblExamSession.Text.Trim() + " And Then Proceed";
                        return;
                    }
                }
                else
                {
                    FSNominee.Visible = false;
                    btngen.Visible = false;
                    lblnorec.Visible = true;
                    lblnorec.Text = "No " + lblExamSession.Text.Trim() + " Were Found";
                    return;
                }
                Dictionary<string, string> dicDegreeDetails = new Dictionary<string, string>();
                ArrayList arrBatchDegree = new ArrayList();
                if (rdbtnsubject.Checked == true)
                {
                    collegeCode = string.Empty;
                    string degreeCode = string.Empty;
                    string batchYear = string.Empty;
                    string qryBatch = string.Empty;
                    string qryBatch1 = string.Empty;
                    qryDegreeCode = string.Empty;
                    string qryDegreeCode1 = string.Empty;
                    ListItem[] liDegreeDetails = new ListItem[chklistcourse.Items.Count];
                    //Dictionary<string, string> dicDegreeDetails = new Dictionary<string, string>();
                    //ArrayList arrBatchDegree = new ArrayList();
                    if (cblCollege.Items.Count == 0)
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        btnprintpdf.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "No College Were Found";
                        return;
                    }
                    else
                    {
                        collegeCode = getCblSelectedValue(cblCollege);
                    }
                    if (!string.IsNullOrEmpty(collegeCode.Trim()))
                    {
                        qryCollege = " and r.college_code in (" + collegeCode + ")";
                    }
                    else
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        btnprintpdf.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select Any One College";
                        return;
                    }
                    if (cblBatch.Items.Count == 0)
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        btnprintpdf.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "No Batch Were Found";
                        return;
                    }
                    else
                    {
                        batchYear = getCblSelectedValue(cblBatch);
                    }
                    if (!string.IsNullOrEmpty(batchYear.Trim()))
                    {
                        qryBatch = " and ed.batch_year in (" + batchYear + ")";
                        qryBatch1 = " and e.BatchFrom in (" + batchYear + ")";
                    }
                    else
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        btnprintpdf.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select Any One Batch";
                        return;
                    }
                    if (chklistcourse.Items.Count == 0)
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        btnprintpdf.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "No Course Were Found";
                        return;
                    }
                    else
                    {
                        chklistcourse.Items.CopyTo(liDegreeDetails, 0);
                        degreeCode = getCblSelectedValue(chklistcourse);
                    }
                    if (!string.IsNullOrEmpty(degreeCode.Trim()))
                    {
                        qryDegreeCode = " and ed.degree_code in (" + degreeCode + ")";
                        qryDegreeCode1 = " and e.degree_code in (" + degreeCode + ")";
                    }
                    else
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        btnprintpdf.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select Any One Course";
                        return;
                    }
                    if (ddlExamYear.Items.Count == 0)
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        btnprintpdf.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "No Exam Year Were Fond";
                        return;
                    }
                    else
                    {
                        string examYear = string.Empty;
                        examYear = Convert.ToString(ddlExamYear.SelectedValue).Trim();
                        if (string.IsNullOrEmpty(examYear) || examYear.Trim() == "0" || examYear.Trim().ToLower() == "all")
                        {
                            FSNominee.Visible = false;
                            btngen.Visible = false;
                            btnprintpdf.Visible = false;
                            lblnorec.Visible = true;
                            lblnorec.Text = "Please Select Any One Exam Year";
                            return;
                        }
                    }
                    if (ddlExamMonth.Items.Count == 0)
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        btnprintpdf.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "No Exam Month Were Found";
                        return;
                    }
                    else
                    {
                        string examMonth = string.Empty;
                        examMonth = Convert.ToString(ddlExamMonth.SelectedValue).Trim();
                        if (string.IsNullOrEmpty(examMonth) || examMonth.Trim() == "0" || examMonth.Trim().ToLower() == "all")
                        {
                            FSNominee.Visible = false;
                            btngen.Visible = false;
                            btnprintpdf.Visible = false;
                            lblnorec.Visible = true;
                            lblnorec.Text = "Please Select Any One Exam Month";
                            return;
                        }
                    }
                    if (liDegreeDetails.Length > 0)
                    {
                        foreach (ListItem li in liDegreeDetails)
                        {
                            string value = Convert.ToString(li.Value).Trim();
                            string text = Convert.ToString(li.Text).Trim();
                            if (!dicDegreeDetails.ContainsKey(value.ToLower().Trim()))
                            {
                                dicDegreeDetails.Add(value.ToLower().Trim(), text);
                            }
                        }
                    }
                    string strdept = string.Empty;
                    if (chklistcourse.Items.Count > 0)
                    {
                        for (int cd = 0; cd < chklistcourse.Items.Count; cd++)
                        {
                            if (chklistcourse.Items[cd].Selected == true)
                            {
                                if (strdept == "")
                                {
                                    strdept = chklistcourse.Items[cd].Value.ToString();
                                }
                                else
                                {
                                    strdept = strdept + ',' + chklistcourse.Items[cd].Value.ToString();
                                }
                            }
                        }
                    }
                    else
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "No Degree Were Found";
                        return;
                    }
                    string strdeptvalue = string.Empty;
                    if (!string.IsNullOrEmpty(strdept))
                    {
                        strdeptvalue = " and e.degree_code in(" + strdept + ")";
                    }
                    else
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select The Degree And Then Proceed";
                        return;
                    }
                    string strsubjectcode = string.Empty;
                    if (chklistsub.Items.Count > 0)
                    {
                        for (int cd = 0; cd < chklistsub.Items.Count; cd++)
                        {
                            if (chklistsub.Items[cd].Selected == true)
                            {
                                if (strsubjectcode == "")
                                {
                                    strsubjectcode = "'" + chklistsub.Items[cd].Value.ToString() + "'";
                                }
                                else
                                {
                                    strsubjectcode = strsubjectcode + ",'" + chklistsub.Items[cd].Value.ToString() + "'";
                                }
                            }
                        }
                    }
                    else
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "No Subject Were Found";
                        return;
                    }
                    if (strsubjectcode.Trim() != "")
                    {
                        strsubjectcode = " and s.subject_code in(" + strsubjectcode + ")";
                    }
                    else
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select The Subject And Then Proceed";
                        return;
                    }
                    //string strquery = "select distinct e.Exam_year,e.Exam_month,e.degree_code,e.Semester,convert(nvarchar(15),et.exam_date,103) as edate,et.exam_date,et.exam_session,s.subject_code,s.subject_name,c.edu_level,c.Course_Name,de.Dept_Name,e.Semester,d.college_code from exmtt e,exmtt_det et,subject s,Degree d,Course c,Department de where e.exam_code=et.exam_code and e.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and et.subject_no=s.subject_no and d.Dept_Code=de.Dept_Code and e.Exam_month='" + ddlExamMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlExamYear.SelectedValue.ToString() + "' " + strexamdate + " " + strsession + " " + strdeptvalue + " " + strsubjectcode + " order by et.exam_date ,et.exam_session desc,c.edu_level desc,de.Dept_Name,s.subject_code";
                    Dictionary<string, int> dicRowCount = new Dictionary<string, int>();
                    string strquery = "select distinct e.Exam_year,e.Exam_month,e.degree_code,e.Semester,convert(nvarchar(15),et.exam_date,103) as edate,et.exam_date,et.exam_session,s.subject_code,s.subject_name,s.subjectpriority,c.edu_level,c.Course_Name,de.Dept_Name,de.dept_acronym,e.Semester,d.college_code from exmtt e,exmtt_det et,subject s,Degree d,Course c,Department de where e.exam_code=et.exam_code and e.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and et.subject_no=s.subject_no and d.Dept_Code=de.Dept_Code and e.Exam_month='" + ddlExamMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlExamYear.SelectedValue.ToString() + "' " + qryExamDate + " " + qryExamSession + " " + strdeptvalue + " " + strsubjectcode + qryBatch1 + " order by et.exam_date,et.exam_session desc,c.edu_level desc,s.subjectpriority,s.subject_code,de.dept_acronym";
                    if (chkconsolidate.Checked == false)
                    {
                        DataSet ds = da.select_method_wo_parameter(strquery, "text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            FSNominee.Sheets[0].AutoPostBack = false;
                            btngen.Visible = true;
                            FSNominee.Visible = true;
                            FSNominee.Sheets[0].ColumnHeader.RowCount = 0;
                            FSNominee.Sheets[0].ColumnHeader.RowCount = 1;
                            FSNominee.Sheets[0].RowCount = 1;
                            FSNominee.Sheets[0].ColumnCount = 7;
                            FSNominee.Sheets[0].ColumnHeader.RowCount = 1;
                            FSNominee.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            FSNominee.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                            FSNominee.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree Details";
                            FSNominee.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Exam Date";
                            FSNominee.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Exam Session";
                            FSNominee.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subject Code";
                            FSNominee.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Subject Name";

                            FSNominee.Sheets[0].ColumnHeader.Rows[0].BackColor = Color.AliceBlue;

                            FSNominee.Sheets[0].SpanModel.Add(0, 2, 1, 5);
                            FSNominee.Sheets[0].Cells[0, 1].CellType = chkcell1;

                            FSNominee.Sheets[0].Columns[0].Width = 50;
                            FSNominee.Sheets[0].Columns[1].Width = 50;
                            FSNominee.Sheets[0].Columns[2].Width = 200;
                            FSNominee.Sheets[0].Columns[3].Width = 100;
                            FSNominee.Sheets[0].Columns[4].Width = 80;
                            FSNominee.Sheets[0].Columns[5].Width = 150;
                            FSNominee.Sheets[0].Columns[6].Width = 250;

                            FSNominee.Sheets[0].Columns[0].Locked = true;
                            FSNominee.Sheets[0].Columns[1].Locked = false;
                            FSNominee.Sheets[0].Columns[2].Locked = true;
                            FSNominee.Sheets[0].Columns[3].Locked = true;
                            FSNominee.Sheets[0].Columns[4].Locked = true;
                            FSNominee.Sheets[0].Columns[5].Locked = true;
                            FSNominee.Sheets[0].Columns[6].Locked = true;

                            int srno = 0;
                            string degreeCodeValue = string.Empty;
                            string[] arrDegreeCode = new string[0];

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                string degCode = Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]).Trim().ToLower();
                                string keyValue = Convert.ToString(Convert.ToString(ds.Tables[0].Rows[i]["Course_Name"]).Trim() + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]).Trim().ToLower() + "@" + Convert.ToString(ds.Tables[0].Rows[i]["subject_code"]).Trim().ToLower()).Trim().ToLower();
                                FSNominee.Sheets[0].Columns[2].Visible = true;
                                if (chkconsolidate.Checked == true)
                                {
                                    if (chkIncludeDepartmentWise.Checked)
                                    {
                                        keyValue = Convert.ToString(Convert.ToString(ds.Tables[0].Rows[i]["Course_Name"]).Trim() + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]).Trim().ToLower() + "@").Trim().ToLower();
                                        FSNominee.Sheets[0].Columns[2].Visible = true;
                                    }
                                    keyValue += Convert.ToString(ds.Tables[0].Rows[i]["subject_code"]).Trim().ToLower();
                                    if (!chkWithoutRegularArrear.Checked)
                                    {
                                        keyValue += "-" + Convert.ToString(ds.Tables[0].Rows[i]["status"]).Trim().ToLower();
                                    }
                                }
                                if (!arrBatchDegree.Contains(keyValue.ToLower().Trim()))
                                {
                                    arrBatchDegree.Add(keyValue.ToLower().Trim());
                                    degreeCodeValue = degCode.Trim();
                                    srno++;
                                    FSNominee.Sheets[0].RowCount++;
                                    if (!dicRowCount.ContainsKey(keyValue.Trim().ToLower()))
                                    {
                                        dicRowCount.Add(keyValue.Trim().ToLower(), FSNominee.Sheets[0].RowCount - 1);
                                    }
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 1].CellType = chkcell;
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[i]["college_code"]).Trim();
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["Course_Name"].ToString() + '-' + ds.Tables[0].Rows[i]["Dept_Name"].ToString();
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 2].Tag = ds.Tables[0].Rows[i]["degree_code"].ToString();
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["edate"].ToString();
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 3].Tag = ds.Tables[0].Rows[i]["exam_date"].ToString();
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["exam_session"].ToString();
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[i]["subject_code"].ToString();
                                    if (chkconsolidate.Checked == true)
                                    {
                                        FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 5].Tag = ds.Tables[0].Rows[i]["status"].ToString();
                                        FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 6].Tag = ds.Tables[0].Rows[i]["semester"].ToString();
                                    }
                                    else
                                    {
                                        FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 5].Tag = ds.Tables[0].Rows[i]["Semester"].ToString();
                                    }
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 6].Text = ds.Tables[0].Rows[i]["subject_name"].ToString();
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                }
                                else
                                {
                                    int rowValue = FSNominee.Sheets[0].RowCount - 1;
                                    if (dicRowCount.ContainsKey(keyValue.Trim().ToLower()))
                                    {
                                        rowValue = dicRowCount[keyValue.Trim().ToLower()];
                                    }
                                    if (!string.IsNullOrEmpty(degreeCodeValue))
                                    {
                                        degreeCodeValue += ",'" + degCode + "'";
                                    }
                                    else
                                    {
                                        degreeCodeValue = "'" + degCode + "'";
                                    }
                                    FSNominee.Sheets[0].Cells[rowValue, 2].Tag = degreeCodeValue;
                                }
                            }
                        }
                        else
                        {
                            FSNominee.Visible = false;
                            btngen.Visible = false;
                            lblnorec.Visible = true;
                            lblnorec.Text = "No Records Found";
                        }
                    }
                    else
                    {
                        btngen.Visible = false;
                        strquery = " select distinct convert(nvarchar(15),et.exam_date,103) as edate,et.exam_date,et.exam_session,ed.degree_code,c.edu_level,c.Course_Name,de.Dept_Name,de.dept_acronym,s.subject_code,s.subject_name,s.subjectpriority,case when ead.attempts=0 then 'Regular' else  'Arrear' end as status ,sy.semester,d.college_code from Exam_Details ed,exam_application ea,exam_appl_details ead,exmtt_det et,subject s,Degree d,Course c,Department de,exmtt e,syllabus_master sy where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ead.subject_no=et.subject_no and s.syll_code=sy.syll_code and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and et.subject_no=s.subject_no and d.Dept_Code=de.Dept_Code and e.exam_code=et.exam_code and e.degree_code=ed.degree_code and e.batchFrom=ed.batch_year and e.Exam_month=ed.Exam_Month and e.Exam_year=ed.Exam_year and ed.Exam_month='" + ddlExamMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlExamYear.SelectedValue.ToString() + "' " + qryExamDate + " " + qryExamSession + " " + strdeptvalue + " " + strsubjectcode + qryBatch + "  order by  et.exam_date ,et.exam_session desc,c.edu_level desc,s.subject_code,de.dept_acronym,status desc,sy.semester";//c.edu_level desc,s.subjectpriority,s.subject_code,de.Dept_Nameadmin
                        DataSet ds = da.select_method_wo_parameter(strquery, "text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            FSNominee.Sheets[0].AutoPostBack = true;
                            btnprintpdf.Visible = true;
                            FSNominee.Visible = true;
                            FSNominee.Sheets[0].ColumnHeader.RowCount = 0;
                            FSNominee.Sheets[0].ColumnHeader.RowCount = 1;
                            FSNominee.Sheets[0].ColumnCount = 7;
                            FSNominee.Sheets[0].ColumnHeader.RowCount = 1;
                            FSNominee.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            FSNominee.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Course";
                            FSNominee.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Code";
                            FSNominee.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Subject Name";
                            FSNominee.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Semester";
                            FSNominee.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Regular / Arrear";
                            FSNominee.Sheets[0].ColumnHeader.Cells[0, 6].Text = "No. of Candidates Registered";
                            FSNominee.Sheets[0].ColumnHeader.Rows[0].BackColor = Color.AliceBlue;
                            FSNominee.Sheets[0].SpanModel.Add(0, 2, 1, 5);
                            FSNominee.Sheets[0].Columns[0].Width = 50;
                            FSNominee.Sheets[0].Columns[1].Width = 250;
                            FSNominee.Sheets[0].Columns[2].Width = 120;
                            FSNominee.Sheets[0].Columns[3].Width = 250;
                            FSNominee.Sheets[0].Columns[4].Width = 80;
                            FSNominee.Sheets[0].Columns[5].Width = 150;
                            FSNominee.Sheets[0].Columns[5].Width = 100;
                            FSNominee.Sheets[0].Columns[0].Locked = true;
                            FSNominee.Sheets[0].Columns[1].Locked = false;
                            FSNominee.Sheets[0].Columns[2].Locked = true;
                            FSNominee.Sheets[0].Columns[3].Locked = true;
                            FSNominee.Sheets[0].Columns[4].Locked = true;
                            FSNominee.Sheets[0].Columns[5].Locked = true;
                            int srno = 0;
                            int stucount = 0;
                            int grandtotal = 0;
                            string tempsubcode = string.Empty;
                            string degreeName = string.Empty;
                            int degreeWiseTotal = 0;
                            string month = ddlExamMonth.SelectedValue.ToString();
                            string year = ddlExamYear.SelectedItem.ToString();
                            Dictionary<string, int> dicDegreeWiseTotalCount = new Dictionary<string, int>();
                            Dictionary<string, int> dicSubjectWiseTotalCount = new Dictionary<string, int>();
                            Dictionary<string, int> dicRowCountSubTot = new Dictionary<string, int>();
                            ArrayList arrStudents = new ArrayList();
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (tempsubcode == "")
                                {
                                    tempsubcode = ds.Tables[0].Rows[i]["subject_code"].ToString();
                                }
                                int subjectWiseTotal = 0;
                                string degreecode = ds.Tables[0].Rows[i]["degree_code"].ToString();
                                string subjectcode = ds.Tables[0].Rows[i]["subject_code"].ToString();
                                string isarrea = ds.Tables[0].Rows[i]["status"].ToString();
                                string keyValues = string.Empty;
                                string keyValues1 = string.Empty;
                                string subTotKey = string.Empty;

                                FSNominee.Sheets[0].Columns[1].Visible = false;
                                if (chkIncludeDepartmentWise.Checked)
                                {
                                    keyValues = Convert.ToString(ds.Tables[0].Rows[i]["Course_Name"]).Trim().ToLower() + "-" + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]).Trim().ToLower() + "@";
                                    FSNominee.Sheets[0].Columns[1].Visible = true;
                                }
                                keyValues += subjectcode.Trim().ToLower();
                                keyValues1 += subjectcode.Trim().ToLower();
                                if (!chkWithoutRegularArrear.Checked)
                                {
                                    keyValues += "-" + isarrea.Trim().ToLower();
                                    FSNominee.Sheets[0].Columns[5].Visible = true;
                                }
                                else
                                {
                                    FSNominee.Sheets[0].Columns[5].Visible = false;
                                }
                                if (dicSubjectWiseTotalCount.ContainsKey(keyValues1.Trim().ToLower()))
                                {
                                    subjectWiseTotal = dicSubjectWiseTotalCount[keyValues1.Trim().ToLower()];
                                }

                                if (tempsubcode != ds.Tables[0].Rows[i]["subject_code"].ToString())
                                {
                                    
                                    if (chkNeedSubjectTotal.Checked)
                                    {
                                        subTotKey = string.Empty;
                                        subTotKey += tempsubcode.Trim().ToLower();
                                        subjectWiseTotal = 0;
                                        if (dicSubjectWiseTotalCount.ContainsKey(subTotKey.Trim().ToLower()))
                                        {
                                            subjectWiseTotal = dicSubjectWiseTotalCount[subTotKey.Trim().ToLower()];
                                        }
                                        int subTotRows = 0;//FpStudentStrength.Sheets[0].RowCount - 1;
                                        if (!dicRowCountSubTot.ContainsKey(subTotKey.Trim().ToLower()))
                                        {
                                            FSNominee.Sheets[0].RowCount++;
                                            dicRowCountSubTot.Add(subTotKey.Trim().ToLower(), FSNominee.Sheets[0].RowCount - 1);
                                            subTotRows = FSNominee.Sheets[0].RowCount - 1;
                                        }
                                        else
                                        {
                                            subTotRows = dicRowCountSubTot[subTotKey.Trim().ToLower()];
                                        }
                                        FSNominee.Sheets[0].Cells[subTotRows, 0].Text = "Sub Total";
                                        FSNominee.Sheets[0].SpanModel.Add(subTotRows, 0, 1, 6);
                                        FSNominee.Sheets[0].Cells[subTotRows, 0].Font.Bold = true;
                                        FSNominee.Sheets[0].Cells[subTotRows, 0].Font.Name = "Book Antiqua";
                                        FSNominee.Sheets[0].Cells[subTotRows, 0].Font.Size = FontUnit.Medium;
                                        FSNominee.Sheets[0].Cells[subTotRows, 0].HorizontalAlign = HorizontalAlign.Right;
                                        FSNominee.Sheets[0].Cells[subTotRows, 6].Text = subjectWiseTotal.ToString();
                                        FSNominee.Sheets[0].Cells[subTotRows, 6].Font.Bold = true;
                                        FSNominee.Sheets[0].Cells[subTotRows, 6].Font.Name = "Book Antiqua";
                                        FSNominee.Sheets[0].Cells[subTotRows, 6].Font.Size = FontUnit.Medium;
                                        FSNominee.Sheets[0].Cells[subTotRows, 6].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    tempsubcode = ds.Tables[0].Rows[i]["subject_code"].ToString();
                                    stucount = 0;
                                    degreeWiseTotal = 0;
                                    degreeName = string.Empty;
                                }

                                //if (string.IsNullOrEmpty(degreeName))
                                //{
                                //    degreeName = ds.Tables[0].Rows[i]["Course_Name"].ToString().Trim().ToLower() + '-' + ds.Tables[0].Rows[i]["Dept_Name"].ToString().Trim().ToLower() + "-" + isarrea.Trim().ToLower();
                                //    srno++;
                                //    FSNominee.Sheets[0].RowCount++;
                                //    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                                //    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[i]["college_code"]).Trim();
                                //    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["Course_Name"].ToString() + '-' + ds.Tables[0].Rows[i]["Dept_Name"].ToString();
                                //    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 2].Text = subjectcode;
                                //    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["subject_name"].ToString();
                                //    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["semester"].ToString();
                                //    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[i]["status"].ToString();
                                //}
                                //if (degreeName != ds.Tables[0].Rows[i]["Course_Name"].ToString().Trim().ToLower() + '-' + ds.Tables[0].Rows[i]["Dept_Name"].ToString().Trim().ToLower() + "-" + isarrea.Trim().ToLower())

                                string att = " and ead.attempts=0";
                                if (isarrea.Trim().ToLower() == "arrear")
                                {
                                    att = " and ead.attempts>0";
                                }
                                string keyDegWise = degreecode + "@" + subjectcode + "@" + isarrea;
                                string query = string.Empty;
                                degreeWiseTotal = 0;
                                if (!arrStudents.Contains(keyDegWise.Trim().ToLower()))
                                {
                                    query = da.GetFunction("select count(distinct ea.roll_no) from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and s.subject_code='" + subjectcode + "' and ed.degree_code='" + degreecode + "' and ed.Exam_Month='" + month + "' and ed.Exam_year='" + year + "' " + att + qryBatch + " and s.subject_no in(select et.subject_no from exmtt e,exmtt_det et,subject s1 where e.exam_code=et.exam_code and s1.subject_no=et.subject_no and e.Exam_month='" + month + "' and e.Exam_year='" + year + "' and e.degree_code='" + degreecode + "' and s1.subject_code='" + subjectcode + "')");
                                    //degreeWiseTotal = Convert.ToInt32(query);
                                    int.TryParse(query.Trim(), out degreeWiseTotal);
                                    arrStudents.Add(keyDegWise.Trim().ToLower());
                                }

                                int studentsCount = 0;
                                int.TryParse(query.Trim(), out studentsCount);
                                stucount = stucount + Convert.ToInt32(studentsCount);
                                grandtotal = grandtotal + Convert.ToInt32(studentsCount);
                                if (!dicDegreeWiseTotalCount.ContainsKey(keyValues.Trim().ToLower()))
                                {
                                    dicDegreeWiseTotalCount.Add(keyValues.Trim().ToLower(), degreeWiseTotal);
                                }
                                else
                                {
                                    degreeWiseTotal += dicDegreeWiseTotalCount[keyValues.Trim().ToLower()];
                                    dicDegreeWiseTotalCount[keyValues.Trim().ToLower()] = degreeWiseTotal;
                                }
                                if (!dicSubjectWiseTotalCount.ContainsKey(keyValues1.Trim().ToLower()))
                                {
                                    dicSubjectWiseTotalCount.Add(keyValues1.Trim().ToLower(), degreeWiseTotal);
                                }
                                else
                                {
                                    studentsCount += dicSubjectWiseTotalCount[keyValues1.Trim().ToLower()];
                                    dicSubjectWiseTotalCount[keyValues1.Trim().ToLower()] = studentsCount;
                                }
                                if (!arrBatchDegree.Contains(keyValues.ToLower().Trim()))
                                {
                                    arrBatchDegree.Add(keyValues.ToLower().Trim());
                                    //degreeName = ds.Tables[0].Rows[i]["Course_Name"].ToString().Trim().ToLower() + '-' + ds.Tables[0].Rows[i]["Dept_Name"].ToString().Trim().ToLower() + "-" + isarrea.Trim().ToLower();
                                    srno++;
                                    FSNominee.Sheets[0].RowCount++;
                                    if (!dicRowCount.ContainsKey(keyValues.Trim().ToLower()))
                                    {
                                        dicRowCount.Add(keyValues.Trim().ToLower(), FSNominee.Sheets[0].RowCount - 1);
                                    }

                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[i]["college_code"]).Trim();
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["Course_Name"].ToString() + '-' + ds.Tables[0].Rows[i]["Dept_Name"].ToString();
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 2].Text = subjectcode;
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["subject_name"].ToString();
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["semester"].ToString();
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[i]["status"].ToString();
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 6].Text = degreeWiseTotal.ToString();
                                }
                                else
                                {
                                    int rowValue = FSNominee.Sheets[0].RowCount - 1;
                                    if (dicRowCount.ContainsKey(keyValues.Trim().ToLower()))
                                    {
                                        rowValue = dicRowCount[keyValues.Trim().ToLower()];
                                    }
                                    FSNominee.Sheets[0].Cells[rowValue, 6].Text = degreeWiseTotal.ToString();
                                }
                                FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                                FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;

                                FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                                FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;

                                FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                                FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;

                                FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;

                                if (i == ds.Tables[0].Rows.Count - 1)
                                {
                                    if (chkNeedSubjectTotal.Checked)
                                    {
                                        int subTotRows = 0;//FpStudentStrength.Sheets[0].RowCount - 1;
                                        if (!dicRowCountSubTot.ContainsKey(keyValues1.Trim().ToLower()))
                                        {
                                            FSNominee.Sheets[0].RowCount++;
                                            dicRowCountSubTot.Add(keyValues1.Trim().ToLower(), FSNominee.Sheets[0].RowCount - 1);
                                            subTotRows = FSNominee.Sheets[0].RowCount - 1;
                                        }
                                        else
                                        {
                                            subTotRows = dicRowCountSubTot[keyValues1.Trim().ToLower()];
                                        }
                                        FSNominee.Sheets[0].RowCount++;
                                        FSNominee.Sheets[0].SpanModel.Add(subTotRows, 0, 1, 6);
                                        FSNominee.Sheets[0].Cells[subTotRows, 0].Text = "Sub Total";
                                        FSNominee.Sheets[0].Cells[subTotRows, 0].Font.Bold = true;
                                        FSNominee.Sheets[0].Cells[subTotRows, 0].Font.Name = "Book Antiqua";
                                        FSNominee.Sheets[0].Cells[subTotRows, 0].Font.Size = FontUnit.Medium;
                                        FSNominee.Sheets[0].Cells[subTotRows, 0].HorizontalAlign = HorizontalAlign.Right;

                                        FSNominee.Sheets[0].Cells[subTotRows, 6].Text = subjectWiseTotal.ToString();
                                        FSNominee.Sheets[0].Cells[subTotRows, 6].Font.Bold = true;
                                        FSNominee.Sheets[0].Cells[subTotRows, 6].Font.Name = "Book Antiqua";
                                        FSNominee.Sheets[0].Cells[subTotRows, 6].Font.Size = FontUnit.Medium;
                                        FSNominee.Sheets[0].Cells[subTotRows, 6].HorizontalAlign = HorizontalAlign.Center;
                                    }

                                    FSNominee.Sheets[0].RowCount++;
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].Text = "Total";
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                    FSNominee.Sheets[0].SpanModel.Add(FSNominee.Sheets[0].RowCount - 1, 0, 1, 6);
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 6].Text = grandtotal.ToString();
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                        }
                        else
                        {
                            FSNominee.Visible = false;
                            btngen.Visible = false;
                            lblnorec.Visible = true;
                            lblnorec.Text = "No Records Found";
                        }
                    }
                    FSNominee.Width = 898;
                    FSNominee.Height = 700;
                }
                else
                {
                    btngen.Visible = false;
                    //if (ddldegree.SelectedItem.Text == "--Select--")
                    //{
                    //    FSNominee.Visible = false;
                    //    btngen.Visible = false;
                    //    btnprintpdf.Visible = false;
                    //    lblnorec.Visible = true;
                    //    lblnorec.Text = "Please Select Any One Course";
                    //    return;
                    //}
                    collegeCode = string.Empty;
                    string degreeCode = string.Empty;
                    string batchYear = string.Empty;
                    string qryBatch = string.Empty;
                    string qryBatch1 = string.Empty;
                    qryDegreeCode = string.Empty;
                    string qryDegreeCode1 = string.Empty;
                    ListItem[] liDegreeDetails = new ListItem[chklistcourse.Items.Count];
                    //Dictionary<string, string> dicDegreeDetails = new Dictionary<string, string>();
                    //ArrayList arrBatchDegree = new ArrayList();
                    if (cblCollege.Items.Count == 0)
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        btnprintpdf.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "No College Were Found";
                        return;
                    }
                    else
                    {
                        collegeCode = getCblSelectedValue(cblCollege);
                    }
                    if (!string.IsNullOrEmpty(collegeCode.Trim()))
                    {
                        qryCollege = " and r.college_code in (" + collegeCode + ")";
                    }
                    else
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        btnprintpdf.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select Any One College";
                        return;
                    }
                    if (cblBatch.Items.Count == 0)
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        btnprintpdf.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "No Batch Were Found";
                        return;
                    }
                    else
                    {
                        batchYear = getCblSelectedValue(cblBatch);
                    }
                    if (!string.IsNullOrEmpty(batchYear.Trim()))
                    {
                        qryBatch = " and r.batch_year in (" + batchYear + ")";
                        qryBatch1 = " and e.BatchFrom in (" + batchYear + ")";
                    }
                    else
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        btnprintpdf.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select Any One Batch";
                        return;
                    }
                    if (chklistcourse.Items.Count == 0)
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        btnprintpdf.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "No Course Were Found";
                        return;
                    }
                    else
                    {
                        chklistcourse.Items.CopyTo(liDegreeDetails, 0);
                        degreeCode = getCblSelectedValue(chklistcourse);
                    }
                    if (!string.IsNullOrEmpty(degreeCode.Trim()))
                    {
                        qryDegreeCode = " and ed.degree_code in (" + degreeCode + ")";
                        qryDegreeCode1 = " and e.degree_code in (" + degreeCode + ")";
                    }
                    else
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        btnprintpdf.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select Any One Course";
                        return;
                    }
                    if (ddlExamYear.Items.Count == 0)
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        btnprintpdf.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "No Exam Year Were Fond";
                        return;
                    }
                    else
                    {
                        string examYear = string.Empty;
                        examYear = Convert.ToString(ddlExamYear.SelectedValue).Trim();
                        if (string.IsNullOrEmpty(examYear) || examYear.Trim() == "0" || examYear.Trim().ToLower() == "all")
                        {
                            FSNominee.Visible = false;
                            btngen.Visible = false;
                            btnprintpdf.Visible = false;
                            lblnorec.Visible = true;
                            lblnorec.Text = "Please Select Any One Exam Year";
                            return;
                        }
                    }
                    if (ddlExamMonth.Items.Count == 0)
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        btnprintpdf.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "No Exam Month Were Found";
                        return;
                    }
                    else
                    {
                        string examMonth = string.Empty;
                        examMonth = Convert.ToString(ddlExamMonth.SelectedValue).Trim();
                        if (string.IsNullOrEmpty(examMonth) || examMonth.Trim() == "0" || examMonth.Trim().ToLower() == "all")
                        {
                            FSNominee.Visible = false;
                            btngen.Visible = false;
                            btnprintpdf.Visible = false;
                            lblnorec.Visible = true;
                            lblnorec.Text = "Please Select Any One Exam Month";
                            return;
                        }
                    }
                    DataSet ds = new DataSet();
                    DataSet dssub = new DataSet();
                    if (liDegreeDetails.Length > 0)
                    {
                        foreach (ListItem li in liDegreeDetails)
                        {
                            string value = Convert.ToString(li.Value).Trim();
                            string text = Convert.ToString(li.Text).Trim();
                            if (!dicDegreeDetails.ContainsKey(value.ToLower().Trim()))
                            {
                                dicDegreeDetails.Add(value.ToLower().Trim(), text);
                            }
                        }
                    }
                    //string strquery = "select distinct ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,r.Reg_No,r.Stud_Name from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s, Registration r where ed.exam_code=ea.exam_code  and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and r.Roll_No=ea.roll_no  and ed.degree_code=r.degree_code  and ed.batch_year=r.Batch_Year and ed.degree_code='" + ddldegree.SelectedItem.Value + "'  and ed.Exam_Month='" + ddlExamMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlExamYear.SelectedValue.ToString() + "' and s.subject_no in(select et.subject_no from exmtt e,exmtt_det et,subject s1 where e.exam_code=et.exam_code   and s1.subject_no=et.subject_no and e.Exam_month='" + ddlExamMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlExamYear.SelectedValue.ToString() + "'  and e.degree_code='" + ddldegree.SelectedItem.Value + "' )   order by ed.batch_year desc,ed.degree_code,ed.current_semester,r.Reg_No";
                    //ds = da.select_method_wo_parameter(strquery, "text");
                    //string strsubquery = " select distinct ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,r.Reg_No,r.Stud_Name,s.subject_code,s.subject_name from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s, Registration r where ed.exam_code=ea.exam_code  and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and r.Roll_No=ea.roll_no  and ed.degree_code=r.degree_code  and ed.batch_year=r.Batch_Year and ed.degree_code='" + ddldegree.SelectedItem.Value + "'  and ed.Exam_Month='" + ddlExamMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlExamYear.SelectedValue.ToString() + "'  and s.subject_no in(select et.subject_no from exmtt e,exmtt_det et,subject s1 where e.exam_code=et.exam_code   and s1.subject_no=et.subject_no and e.Exam_month='" + ddlExamMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlExamYear.SelectedValue.ToString() + "'  and e.degree_code='" + ddldegree.SelectedItem.Value + "' )   order by ed.batch_year desc,ed.degree_code,ed.current_semester,r.Reg_No,s.subject_code";
                    //dssub = da.select_method_wo_parameter(strsubquery, "Text");
                    string strquery = "select distinct ed.batch_year,ed.degree_code,c.Edu_Level,c.Priority,c.Course_Name,dt.Dept_Name,dt.dept_acronym,r.current_semester,r.roll_no,r.Reg_No,r.Stud_Name from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Registration r,Degree dg,Department dt,Course c where c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and dg.Degree_Code=r.degree_code and ed.degree_code=r.degree_code and ed.exam_code=ea.exam_code  and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and r.Roll_No=ea.roll_no  and ed.degree_code=r.degree_code  and ed.batch_year=r.Batch_Year and ed.Exam_Month='" + Convert.ToString(ddlExamMonth.SelectedValue).Trim() + "' and ed.Exam_year='" + Convert.ToString(ddlExamYear.SelectedValue).Trim() + "' " + qryDegreeCode + qryBatch + qryCollege + " and s.subject_no in(select et.subject_no from exmtt e,exmtt_det et,subject s1 where e.exam_code=et.exam_code   and s1.subject_no=et.subject_no and e.Exam_month='" + Convert.ToString(ddlExamMonth.SelectedValue).Trim() + "' and e.Exam_year='" + Convert.ToString(ddlExamYear.SelectedValue).Trim() + "' " + qryDegreeCode1 + qryBatch1 + " ) order by c.Edu_Level desc,dt.dept_acronym,ed.batch_year desc,r.current_semester,r.Reg_No";
                    ds = da.select_method_wo_parameter(strquery, "text");
                    //string strsubquery = " select distinct ed.batch_year,ed.degree_code,r.current_semester,ea.roll_no,r.Reg_No,r.Stud_Name,s.subject_code,s.subject_name from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s, Registration r where ed.exam_code=ea.exam_code  and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and r.Roll_No=ea.roll_no  and ed.degree_code=r.degree_code  and ed.batch_year=r.Batch_Year  and ed.Exam_Month='" + Convert.ToString(ddlExamMonth.SelectedValue).Trim() + "' and ed.Exam_year='" + Convert.ToString(ddlExamYear.SelectedValue).Trim() + "' " + qryDegreeCode + qryBatch + qryCollege + " and s.subject_no in(select et.subject_no from exmtt e,exmtt_det et,subject s1 where e.exam_code=et.exam_code   and s1.subject_no=et.subject_no and e.Exam_month='" + Convert.ToString(ddlExamMonth.SelectedValue).Trim() + "' and e.Exam_year='" + Convert.ToString(ddlExamYear.SelectedValue).Trim() + "' " + qryDegreeCode1 + qryBatch1 + " )   order by ed.batch_year desc,ed.degree_code,r.current_semester,r.Reg_No,s.subject_code"; case when LTRIM(RTRIM(ISnull(ead.attempts,'0')))=0 then 1 else 2 end as Regular/Arrear
                    string strsubquery = "select distinct ed.batch_year,ed.degree_code,r.current_semester,ea.roll_no,r.Reg_No,r.Stud_Name,s.subject_code,s.subject_name,s.subjectpriority,case when subjectpriority is not null then subjectpriority else s.subject_no end,case when LTRIM(RTRIM(ISnull(ead.attempts,'0')))=0 then 1 else 2 end as [Regular/Arrear] from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s, Registration r where ed.exam_code=ea.exam_code  and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and r.Roll_No=ea.roll_no  and ed.degree_code=r.degree_code  and ed.batch_year=r.Batch_Year and ed.Exam_Month='" + Convert.ToString(ddlExamMonth.SelectedValue).Trim() + "' and ed.Exam_year='" + Convert.ToString(ddlExamYear.SelectedValue).Trim() + "' " + qryDegreeCode + qryBatch + qryCollege + " and s.subject_no in(select et.subject_no from exmtt e,exmtt_det et,subject s1 where e.exam_code=et.exam_code and s1.subject_no=et.subject_no and  e.Exam_month='" + Convert.ToString(ddlExamMonth.SelectedValue).Trim() + "' and e.Exam_year='" + Convert.ToString(ddlExamYear.SelectedValue).Trim() + "' " + qryDegreeCode1 + qryBatch1 + " ) order by ed.batch_year desc,ed.degree_code,r.current_semester,r.Reg_No,[Regular/Arrear],s.subjectpriority,s.subject_code";
                    dssub = da.select_method_wo_parameter(strsubquery, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                        btngen.Visible = false;
                        FSNominee.Visible = true;
                        btnprintpdf.Visible = true;
                        FSNominee.Sheets[0].ColumnHeader.RowCount = 0;
                        FSNominee.Sheets[0].ColumnHeader.RowCount = 1;
                        FSNominee.Sheets[0].RowCount = 0;
                        FSNominee.Sheets[0].ColumnCount = 6;
                        //FSNominee.Sheets[0].ColumnHeader.Cells[0, 0].Text = ddldegree.SelectedItem.ToString();
                        //FSNominee.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        //FSNominee.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, 6);
                        FSNominee.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FSNominee.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg No";
                        FSNominee.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
                        FSNominee.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Subject Codes";
                        FSNominee.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Semester";
                        FSNominee.Sheets[0].ColumnHeader.Cells[0, 5].Text = "No of Subjects";

                        FSNominee.Sheets[0].ColumnHeader.Rows[0].BackColor = Color.AliceBlue;
                        //FSNominee.Sheets[0].ColumnHeader.Rows[1].BackColor = Color.AliceBlue;

                        FSNominee.Sheets[0].Columns[1].CellType = txt;
                        FSNominee.Sheets[0].Columns[0].Width = 50;
                        FSNominee.Sheets[0].Columns[1].Width = 200;
                        FSNominee.Sheets[0].Columns[2].Width = 200;
                        FSNominee.Sheets[0].Columns[3].Width = 300;
                        FSNominee.Sheets[0].Columns[4].Width = 50;
                        FSNominee.Sheets[0].Columns[5].Width = 50;
                        FSNominee.Sheets[0].Columns[1].Visible = true;
                        FSNominee.Sheets[0].AutoPostBack = true;
                        int srno = 0;
                        DataView dv = new DataView();

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string getregno = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]).Trim();
                            string degreeCodeValue = Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]).Trim();
                            string batchValue = Convert.ToString(ds.Tables[0].Rows[i]["Batch_year"]).Trim();
                            if (dicDegreeDetails.ContainsKey(degreeCodeValue.Trim().ToLower()))
                            {
                                string degreeName = Convert.ToString(dicDegreeDetails[degreeCodeValue.Trim().ToLower()]).Trim();
                                if (!arrBatchDegree.Contains(batchValue.ToLower().Trim() + "@" + degreeName.ToLower().Trim()))
                                {
                                    FSNominee.Sheets[0].RowCount++;
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].Text = batchValue + " " + degreeName.ToString();
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#458547");
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                    FSNominee.Sheets[0].AddSpanCell(FSNominee.Sheets[0].RowCount - 1, 0, 1, FSNominee.Sheets[0].ColumnCount);
                                    //FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["Reg_No"].ToString();
                                    arrBatchDegree.Add(batchValue.ToLower().Trim() + "@" + degreeName.ToLower().Trim());
                                    srno = 0;
                                }
                            }
                            srno++;
                            FSNominee.Sheets[0].RowCount++;
                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["Reg_No"].ToString();
                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["Stud_Name"].ToString();
                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["current_semester"].ToString();
                            dssub.Tables[0].DefaultView.RowFilter = "Reg_No='" + getregno + "'";
                            DataView dvsub = dssub.Tables[0].DefaultView;
                            string subjectcode = string.Empty;
                            int subno = 0;
                            if (dvsub.Count > 0)
                            {
                                dvsub.Sort = "Regular/Arrear,subjectpriority,subject_code";
                                for (int s = 0; s < dvsub.Count; s++)
                                {
                                    subno++;
                                    if (subjectcode == "")
                                    {
                                        subjectcode = dvsub[s]["subject_code"].ToString();
                                    }
                                    else
                                    {
                                        if (s % 4 == 0)
                                        {
                                            subjectcode = subjectcode + ", " + dvsub[s]["subject_code"].ToString();
                                        }
                                        else
                                        {
                                            subjectcode = subjectcode + "," + dvsub[s]["subject_code"].ToString();
                                        }
                                    }
                                }
                            }
                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 3].Text = subjectcode;
                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 5].Text = subno.ToString();
                            subno = 1;
                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;

                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;

                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;

                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    else
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "No Records Found";
                    }
                }
                FSNominee.Sheets[0].PageSize = FSNominee.Sheets[0].RowCount;
            }
            else if (ddltype.SelectedIndex == 2)
            {
                btnPrintPhasing.Text = "Phasing Sheet";
                qryCollege = string.Empty;
                collegeCode = string.Empty;
                if (cblCollege.Items.Count > 0)
                {
                    collegeCode = getCblSelectedValue(cblCollege);
                }
                if (!string.IsNullOrEmpty(collegeCode.Trim()))
                {
                    qryCollege = " and r.college_code in(" + collegeCode + ")";
                }
                else
                {
                    FSNominee.Visible = false;
                    btngen.Visible = false;
                    lblnorec.Visible = true;
                    lblnorec.Text = "Please Select Any College";
                    return;
                }
                qryHallNo = string.Empty;
                hallNo = string.Empty;
                if (cblHall.Items.Count > 0)
                {
                    hallNo = getCblSelectedValue(cblHall);
                }
                else
                {
                    FSNominee.Visible = false;
                    btngen.Visible = false;
                    lblnorec.Visible = true;
                    lblnorec.Text = "No Hall No Were Found";
                    return;
                }
                if (!string.IsNullOrEmpty(hallNo.Trim()))
                {
                    qryHallNo = " and es.roomno in(" + hallNo + ")";
                }
                qryDegreeCode = string.Empty;
                DegreeCode = string.Empty;
                if (chklistcourse.Items.Count > 0)
                {
                    DegreeCode = getCblSelectedValue(chklistcourse);
                }
                if (!string.IsNullOrEmpty(DegreeCode.Trim()))
                {
                    qryDegreeCode = " and r.degree_code in(" + DegreeCode + ")";
                }
                else
                {
                    FSNominee.Visible = false;
                    btngen.Visible = false;
                    lblnorec.Visible = true;
                    lblnorec.Text = "Please Select The Degree And Then Proceed";
                    return;
                }
                //string examdate = ddlDate.SelectedValue.ToString();
                //string[] dsplit = examdate.Split('-');
                //examdate = dsplit[2].ToString() + "-" + dsplit[1].ToString() + "-" + dsplit[0].ToString();
                string qryDate = string.Empty;
                string examdate = string.Empty; //ddlDate.SelectedValue.ToString();
                string[] dsplit;
                string qrySession = string.Empty;
                // examdate = dsplit[2].ToString() + "-" + dsplit[1].ToString() + "-" + dsplit[0].ToString();
                //if (ddlDate.Items.Count > 0)
                //{
                //    if (ddlDate.SelectedItem.Text.Trim().ToLower() != "all")
                //    {
                //        examdate = ddlDate.SelectedValue.ToString();
                //        dsplit = examdate.Split('-');
                //        examdate = dsplit[2].ToString() + "-" + dsplit[1].ToString() + "-" + dsplit[0].ToString();
                //        if (!string.IsNullOrEmpty(examdate))
                //        {
                //            qryDate = " and es.edate='" + examdate + "'";// " and etd.exam_date='" + examdate + "' ";
                //        }
                //    }
                //}
                //else
                //{
                //    FSNominee.Visible = false;
                //    btngen.Visible = false;
                //    lblnorec.Visible = true;
                //    lblnorec.Text = "No Exam Date Were Found";
                //    return;
                //}

                //if (ddlSession.SelectedItem.Text.Trim().ToLower() == "all")
                //{
                //    qrySession = string.Empty;
                //}
                //else
                //{
                //    qrySession = "  and es.ses_sion='" + ddlSession.SelectedItem.Text + "'";
                //}
                if (cblExamDate.Items.Count > 0 && txtExamDate.Visible == true)
                {
                    examDates = getCblSelectedValue(cblExamDate);
                    if (!string.IsNullOrEmpty(examDates))
                    {
                        qryExamDate = " and convert(varchar(20),es.edate,103) in(" + examDates + ")";
                    }
                    else
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select " + lblExamDate.Text.Trim() + " And Then Proceed";
                        return;
                    }
                }
                else if (ddlExamDate.Items.Count > 0 && ddlExamDate.Visible == true)
                {
                    examDates = string.Empty;
                    foreach (ListItem li in ddlExamDate.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(examDates))
                            {
                                examDates = "'" + li.Value + "'";
                            }
                            else
                            {
                                examDates += ",'" + li.Value + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(examDates))
                    {
                        qryExamDate = " and convert(varchar(20),es.edate,103) in(" + examDates + ")";
                    }
                    else
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select " + lblExamDate.Text.Trim() + " And Then Proceed";
                        return;
                    }
                }
                else
                {
                    FSNominee.Visible = false;
                    btngen.Visible = false;
                    lblnorec.Visible = true;
                    lblnorec.Text = "No " + lblExamDate.Text.Trim() + " Were Found";
                    return;
                }
                if (cblExamSession.Items.Count > 0 && txtExamSession.Visible == true)
                {
                    examSessions = getCblSelectedValue(cblExamSession);
                    if (!string.IsNullOrEmpty(examSessions))
                    {
                        qryExamSession = " and es.ses_sion in(" + examSessions + ")";
                    }
                    else
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select " + lblExamSession.Text.Trim() + " And Then Proceed";
                        return;
                    }
                }
                else if (ddlExamSession.Items.Count > 0 && ddlExamSession.Visible == true)
                {
                    examSessions = string.Empty;
                    foreach (ListItem li in ddlExamSession.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(examSessions))
                            {
                                examSessions = "'" + li.Value + "'";
                            }
                            else
                            {
                                examSessions += ",'" + li.Value + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(examSessions))
                    {
                        qryExamSession = " and es.ses_sion in(" + examSessions + ")";
                    }
                    else
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select " + lblExamSession.Text.Trim() + " And Then Proceed";
                        return;
                    }
                }
                else
                {
                    FSNominee.Visible = false;
                    btngen.Visible = false;
                    lblnorec.Visible = true;
                    lblnorec.Text = "No " + lblExamSession.Text.Trim() + " Were Found";
                    return;
                }
                int selsubjects = 0;
                string strsubjectcode = string.Empty;
                if (chklistsub.Items.Count > 0)
                {
                    for (int cd = 0; cd < chklistsub.Items.Count; cd++)
                    {
                        if (chklistsub.Items[cd].Selected == true)
                        {
                            selsubjects++;
                            if (strsubjectcode == "")
                            {
                                strsubjectcode = "'" + chklistsub.Items[cd].Value.ToString() + "'";
                            }
                            else
                            {
                                strsubjectcode = strsubjectcode + ",'" + chklistsub.Items[cd].Value.ToString() + "'";
                            }
                        }
                    }
                    if (strsubjectcode.Trim() != "")
                    {
                        strsubjectcode = " and s.subject_code in(" + strsubjectcode + ")";
                    }
                    if (selsubjects == 0)
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select The Subject And Then Proceed";
                        return;
                    }
                }
                else
                {
                    FSNominee.Visible = false;
                    btngen.Visible = false;
                    lblnorec.Visible = true;
                    lblnorec.Text = "No Subject(s) Were Found";
                    return;
                }
                //es.edate convert(varchar(50),es.edate,103) edate class_master cm cm.rno=es.roomno  cm.priority
                string spreadbind1 = "select distinct es.roomno,COUNT(1) as strength,es.ses_sion,es.edate,r.degree_code,dp.dept_name,r.batch_year,es.subject_no,es.bundle_no,cm.priority  from registration r,exam_details ed,exam_application ea,exam_appl_details ead,exam_seating as es,degree d,department dp,subject s,class_master cm where cm.rno=es.roomno and s.subject_no=es.subject_no and ead.subject_no=s.subject_no and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no=r.roll_no and r.exam_flag<>'Debar' and es.regno=r.Reg_No and ead.subject_no=es.subject_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ed.degree_code=d.Degree_Code and r.degree_code=d.Degree_Code and dp.dept_code=d.dept_code and d.college_code=r.college_code  and ed.Exam_Month='" + ddlExamMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlExamYear.SelectedValue.ToString() + "' " + qryHallNo + "" + qryExamDate + qryDegreeCode + " " + qryExamSession + strsubjectcode + " and r.college_code in(" + collegeCode + ") group by es.roomno,es.ses_sion,es.edate ,r.degree_code,dp.dept_name,r.batch_year,es.subject_no,es.bundle_no,cm.priority order by cm.priority";
                spreadbind1 = "select count(regno) as strength,s.subject_code,r.batch_year,edate,ses_sion,roomno,es.bundle_no,Priority from exam_seating es,subject s,Registration r,class_master cs where es.subject_no=s.subject_no and r.reg_no=es.regno and cs.rno=es.roomno " + qryExamDate + " " + qryExamSession + " " + qryDegreeCode + " " + qryHallNo + " and r.college_code in(" + collegeCode + ") " + strsubjectcode + " group by s.subject_code,r.batch_year,edate,ses_sion,roomno,es.bundle_no,Priority order by Priority";
                DataSet ds2 = da.select_method_wo_parameter(spreadbind1, "Text");
                FarPoint.Web.Spread.CheckBoxCellType cheall = new FarPoint.Web.Spread.CheckBoxCellType();
                FarPoint.Web.Spread.CheckBoxCellType cheselectall = new FarPoint.Web.Spread.CheckBoxCellType();
                cheselectall.AutoPostBack = true;
                string strength = string.Empty;
                string roomno = string.Empty;
                string sesson = string.Empty;
                string exdate = string.Empty;
                string dept = string.Empty;
                string bun = string.Empty;
                string degrrcode = string.Empty;
                string batchyr = string.Empty;
                string sbjno = string.Empty;
                int sno = 0;
                string getfinbundleno = da.GetFunction("select max(e.bundle_no),len(e.bundle_no) from exam_seating e,exmtt_det ed,exmtt et where ed.exam_code=et.exam_code and convert(nvarchar(15),ed.subject_no)=convert(nvarchar(15),e.subject_no )and ed.exam_date=e.edate and ed.exam_session=e.ses_sion and et.exam_month=" + ddlExamMonth.SelectedValue + " and et.exam_year=" + ddlExamYear.SelectedItem.Text + " group by len(e.bundle_no) order by len(e.bundle_no) desc");
                if (getfinbundleno.Trim() == "" || getfinbundleno == "0")
                {
                    getfinbundleno = da.GetFunction("select value from COE_Master_Settings where settings='Bundle Number Generation'");
                }
                else
                {
                    int incbun = Convert.ToInt32(getfinbundleno);
                    incbun++;
                    getfinbundleno = incbun.ToString();
                }
                if (getfinbundleno == "")
                {
                    getfinbundleno = "1";
                }
                int bundle = Convert.ToInt32(getfinbundleno);
                int height = 45;
                if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                {
                    Init_Spread();
                    FpPhasing.Width = 950;
                    FpPhasing.Visible = true;
                    FpPhasing.Sheets[0].RowCount++;
                    FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 1].CellType = cheselectall;
                    FpPhasing.Sheets[0].SpanModel.Add(FpPhasing.Sheets[0].RowCount - 1, 2, 1, 3);
                    //for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                    //{
                    //    string bundleno = ds2.Tables[0].Rows[i]["bundle_no"].ToString();
                    //    roomno = ds2.Tables[0].Rows[i]["roomno"].ToString();
                    //    strength = ds2.Tables[0].Rows[i]["strength"].ToString();
                    //    dept = ds2.Tables[0].Rows[i]["dept_name"].ToString();
                    //    sesson = ds2.Tables[0].Rows[i]["ses_sion"].ToString();
                    //    exdate = ds2.Tables[0].Rows[i]["edate"].ToString();
                    //    degrrcode = ds2.Tables[0].Rows[i]["degree_code"].ToString();
                    //    batchyr = ds2.Tables[0].Rows[i]["batch_year"].ToString();
                    //    sbjno = ds2.Tables[0].Rows[i]["subject_no"].ToString();
                    //    if (!ht.ContainsKey(sbjno + '-' + roomno))
                    //    {
                    //        FpPhasing.Sheets[0].RowCount++;
                    //        sno++;
                    //        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 0].Text = sno + "";
                    //        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 0].Note = sbjno;

                    //        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].Note = exdate;
                    //        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 4].Note = sesson;
                    //        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 1].CellType = cheall;
                    //        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 7].Text = ds2.Tables[0].Rows[i]["degree_code"].ToString();
                    //        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].Text = roomno;
                    //        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 3].Note = batchyr;
                    //        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 3].Text = dept;
                    //        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                    //        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    //        ds2.Tables[0].DefaultView.RowFilter = "subject_no='" + sbjno + "' and edate='" + exdate + "' and ses_sion='" + sesson + "' and roomno='" + roomno + "'";
                    //        DataView dvstucount = ds2.Tables[0].DefaultView;
                    //        int stuco = 0;
                    //        for (int st = 0; st < dvstucount.Count; st++)
                    //        {
                    //            stuco = stuco + Convert.ToInt32(dvstucount[st]["strength"].ToString());
                    //            strength = stuco.ToString();
                    //        }
                    //        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 4].Text = strength;
                    //    }
                    //    int nodtubun = 0;
                    //    string nofstubundel = da.GetFunction("select value from COE_Master_Settings where settings='Bundle Per Student'");
                    //    if (nofstubundel != "" && nofstubundel != null)
                    //    {
                    //        nodtubun = Convert.ToInt32(nofstubundel);
                    //    }
                    //    string[] dtt = exdate.Split(' ');
                    //    exdate = dtt[0].ToString();
                    //    //  string cnt = "select * from exam_seating e,registration r where r.reg_no=e.regno and e.edate='" + exdate + "' and e.ses_sion ='" + sesson + "' and e.degree_code='" + degrrcode + "' and r.batch_year='" + batchyr + "' and roomno='" + roomno + "' and e.subject_no  ='" + sbjno + "' order by e.seat_no";
                    //    string cnt = "select es.edate,es.ses_sion,es.roomno,es.subject_no,es.regno,es.degree_code,r.Batch_Year,r.roll_no,r.college_code from Exam_Details ed,exam_application ea,exam_appl_details ead ,exam_seating es,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and r.Roll_No=ea.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ed.degree_code=es.degree_code and es.degree_code=r.degree_code and es.regno=r.Reg_No and es.edate='" + exdate + "' and es.ses_sion ='" + sesson + "'  and es.degree_code='" + degrrcode + "' and r.batch_year='" + batchyr + "' and roomno='" + roomno + "' and es.subject_no ='" + sbjno + "' and ed.Exam_Month='" + ddlExamMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlExamYear.SelectedValue.ToString() + "' order by es.seat_no";
                    //    DataSet dsv = new DataSet();
                    //    dsv = da.select_method_wo_parameter(cnt, "text");
                    //    int kstartno = 0;
                    //    FarPoint.Web.Spread.TextCellType txtceltype = new FarPoint.Web.Spread.TextCellType();
                    //    string ksregno = string.Empty;
                    //    if (dsv.Tables[0].Rows.Count > 0)
                    //    {
                    //        if (!ht.ContainsKey(sbjno + '-' + roomno))
                    //        {
                    //            ht.Add(sbjno + '-' + roomno, sbjno + '-' + roomno);
                    //            for (int k = 0; k < dsv.Tables[0].Rows.Count; k++)
                    //            {
                    //                bundleno = da.GetFunction("select bundle_no from exam_seating where regno='" + dsv.Tables[0].Rows[k]["regno"].ToString() + "' and edate='" + exdate + "' and ses_sion='" + sesson + "' and degree_code='" + degrrcode + "' ");
                    //                if (ksregno == "")
                    //                {
                    //                    ksregno = dsv.Tables[0].Rows[k]["regno"].ToString();
                    //                }
                    //                else
                    //                {
                    //                    ksregno = ksregno + "','" + dsv.Tables[0].Rows[k]["regno"].ToString();
                    //                }
                    //                //if (k > 0)
                    //                //{
                    //                //    sno++;
                    //                //    FpPhasing.Sheets[0].RowCount = FpPhasing.Sheets[0].RowCount + 1;
                    //                //    FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 0].Text = sno + "";
                    //                //    FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 0].Note = sbjno;
                    //                //    FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].Note = exdate;
                    //                //    FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 4].Note = sesson;
                    //                //    FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 1].CellType = cheall;
                    //                //    FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 8].Text = ds2.Tables[0].Rows[i]["degree_code"].ToString();
                    //                //    FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].Text = roomno;
                    //                //    FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 3].Note = batchyr;
                    //                //    FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 3].Text = dept;
                    //                //}
                    //                //if (bundleno.Trim() == "")
                    //                //{
                    //                //    bun = bundle.ToString();
                    //                //    bundle++;
                    //                //}
                    //                //else
                    //                //{
                    //                //    bun = bundleno.ToString();
                    //                //}
                    //                FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 5].CellType = txtceltype;
                    //                //FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 5].Text = bun;
                    //                FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 3].Text = ds2.Tables[0].Rows[i]["dept_name"].ToString();
                    //                FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                    //                FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    //                FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].Text = dsv.Tables[0].Rows[k]["roomno"].ToString();
                    //                FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].Note = exdate;
                    //                FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 4].Note = sesson;
                    //                FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 1].CellType = cheall;
                    //                FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 7].Text = dsv.Tables[0].Rows[k]["degree_code"].ToString();
                    //                FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 5].Text = dsv.Tables[0].Rows[k]["roll_no"].ToString();
                    //                FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 5].Tag = dsv.Tables[0].Rows[k]["college_code"].ToString();
                    //                // k = k + nodtubun - 1;
                    //                //if (k < dsv.Tables[0].Rows.Count)
                    //                //{
                    //                //    FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 7].Text = dsv.Tables[0].Rows[k]["roll_no"].ToString();
                    //                //    for (int ks = kstartno; ks <= k; ks++)
                    //                //    {
                    //                //        if (ksregno == "")
                    //                //        {
                    //                //            ksregno = dsv.Tables[0].Rows[ks]["regno"].ToString();
                    //                //        }
                    //                //        else
                    //                //        {
                    //                //            ksregno = ksregno + "','" + dsv.Tables[0].Rows[ks]["regno"].ToString();
                    //                //        }
                    //                //    }
                    //                //    FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 7].Tag = ksregno;
                    //                //    FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 7].CellType = txtceltype;
                    //                //    string sv = "update exam_seating set bundle_no='" + bun + "' where regno in ( '" + ksregno + "') and subject_no='" + sbjno + "' and edate='" + exdate + "' and ses_sion='" + sesson + "' and roomno='" + dsv.Tables[0].Rows[k]["roomno"].ToString() + "' and degree_code='" + dsv.Tables[0].Rows[k]["degree_code"].ToString() + "'";
                    //                //    int k1 = da.update_method_wo_parameter(sv, "text");
                    //                //    ksregno = string.Empty;
                    //                //    kstartno = k + 1;
                    //                //}
                    //                //else
                    //                //{
                    //                //for (int ks = kstartno; ks < dsv.Tables[0].Rows.Count; ks++)
                    //                //{
                    //                //}
                    //                //FpPhasing.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    //                //FpPhasing.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Bottom;
                    //            }
                    //            FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 6].Tag = ksregno;
                    //            FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 6].CellType = txtceltype;
                    //            FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dsv.Tables[0].Rows[dsv.Tables[0].Rows.Count - 1]["roll_no"]);
                    //            string sv = "update exam_seating set bundle_no='" + bun + "' where regno in ('" + ksregno + "') and subject_no='" + sbjno + "' and edate='" + exdate + "' and ses_sion='" + sesson + "' and roomno='" + FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].Text + "' and degree_code='" + FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 7].Text + "'";
                    //            int k1 = da.update_method_wo_parameter(sv, "text");
                    //            //kstartno = k;
                    //            //ksregno = string.Empty;
                    //            //}
                    //            height = height + height + FpPhasing.Sheets[0].Rows[FpPhasing.Sheets[0].RowCount - 1].Height;
                    //        }
                    //    }
                    //}
                    FpPhasing.SaveChanges();
                    FpPhasing.Sheets[0].PageSize = FpPhasing.Sheets[0].RowCount;
                    FpPhasing.Height = 500;
                    FpPhasing.SaveChanges();
                    FpPhasing.Visible = true;
                    divPhasing.Visible = true;
                    rptprint1.Visible = true;
                }
                else
                {
                    rptprint1.Visible = false;
                    divPhasing.Visible = false;
                    lblnorec.Visible = true;
                    lblnorec.Text = "No Record(s) Were Found";
                    return;
                }
            }
            else if (ddltype.SelectedIndex == 3)
            {
                btnPrintPhasing.Text = "Cover Sheet";
                qryCollege = string.Empty;
                collegeCode = string.Empty;
                if (cblCollege.Items.Count > 0)
                {
                    collegeCode = getCblSelectedValue(cblCollege);
                }
                if (!string.IsNullOrEmpty(collegeCode.Trim()))
                {
                    //and etd.coll_code in ('13')
                    qryCollege = " and etd.coll_code in(" + collegeCode + ")";
                }
                else
                {
                    FSNominee.Visible = false;
                    btngen.Visible = false;
                    lblnorec.Visible = true;
                    lblnorec.Text = "Please Select Any College";
                    return;
                }
                //qryHallNo = string.Empty;
                //hallNo = string.Empty;
                //if (cblHall.Items.Count > 0)
                //{
                //    hallNo = getCblSelectedValue(cblHall);
                //}
                //if (!string.IsNullOrEmpty(hallNo.Trim()))
                //{
                //    qryHallNo = " and es.roomno in(" + hallNo + ")";
                //}
                //qryDegreeCode = string.Empty;
                //DegreeCode = string.Empty;
                //if (chklistcourse.Items.Count > 0)
                //{
                //    DegreeCode = getCblSelectedValue(chklistcourse);
                //}
                //if (!string.IsNullOrEmpty(DegreeCode.Trim()))
                //{
                //    qryDegreeCode = " and r.degree_code in(" + DegreeCode + ")";
                //}
                //else
                //{
                //    FSNominee.Visible = false;
                //    btngen.Visible = false;
                //    lblnorec.Visible = true;
                //    lblnorec.Text = "Please Select The Degree And Then Proceed";
                //    return;
                //}
                string qryDate = string.Empty;
                string examdate = string.Empty; //ddlDate.SelectedValue.ToString();
                string[] dsplit;
                // examdate = dsplit[2].ToString() + "-" + dsplit[1].ToString() + "-" + dsplit[0].ToString();
                //if (ddlDate.Items.Count > 0)
                //{
                //    if (ddlDate.SelectedItem.Text.Trim().ToLower() != "all")
                //    {
                //        examdate = ddlDate.SelectedValue.ToString();
                //        dsplit = examdate.Split('-');
                //        examdate = dsplit[2].ToString() + "-" + dsplit[1].ToString() + "-" + dsplit[0].ToString();
                //        if (!string.IsNullOrEmpty(examdate))
                //        {
                //            qryDate = " and etd.exam_date='" + examdate + "' ";
                //        }
                //    }
                //}
                //else
                //{
                //    FSNominee.Visible = false;
                //    btngen.Visible = false;
                //    lblnorec.Visible = true;
                //    lblnorec.Text = "No Exam Date Were Found";
                //    return;
                //}
                string qrySession = string.Empty;
                //if (ddlSession.Items.Count > 0)
                //{
                //    if (ddlSession.SelectedItem.Text.ToLower() == "all")
                //    {
                //        qrySession = string.Empty;
                //    }
                //    else
                //    {
                //        qrySession = "  and etd.exam_session='" + ddlSession.SelectedItem.Text + "'";
                //    }
                //}
                if (cblExamDate.Items.Count > 0 && txtExamDate.Visible == true)
                {
                    examDates = getCblSelectedValue(cblExamDate);
                    if (!string.IsNullOrEmpty(examDates))
                    {
                        qryExamDate = " and convert(varchar(20),etd.exam_date,103) in(" + examDates + ")";
                    }
                    else
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select " + lblExamDate.Text.Trim() + " And Then Proceed";
                        return;
                    }
                }
                else if (ddlExamDate.Items.Count > 0 && ddlExamDate.Visible == true)
                {
                    examDates = string.Empty;
                    foreach (ListItem li in ddlExamDate.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(examDates))
                            {
                                examDates = "'" + li.Value + "'";
                            }
                            else
                            {
                                examDates += ",'" + li.Value + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(examDates))
                    {
                        qryExamDate = " and convert(varchar(20),etd.exam_date,103) in(" + examDates + ")";
                    }
                    else
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select " + lblExamDate.Text.Trim() + " And Then Proceed";
                        return;
                    }
                }
                else
                {
                    FSNominee.Visible = false;
                    btngen.Visible = false;
                    lblnorec.Visible = true;
                    lblnorec.Text = "No " + lblExamDate.Text.Trim() + " Were Found";
                    return;
                }
                if (cblExamSession.Items.Count > 0 && txtExamSession.Visible == true)
                {
                    examSessions = getCblSelectedValue(cblExamSession);
                    if (!string.IsNullOrEmpty(examSessions))
                    {
                        qryExamSession = " and etd.exam_session in(" + examSessions + ")";
                    }
                    else
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select " + lblExamSession.Text.Trim() + " And Then Proceed";
                        return;
                    }
                }
                else if (ddlExamSession.Items.Count > 0 && ddlExamSession.Visible == true)
                {
                    examSessions = string.Empty;
                    foreach (ListItem li in ddlExamSession.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(examSessions))
                            {
                                examSessions = "'" + li.Value + "'";
                            }
                            else
                            {
                                examSessions += ",'" + li.Value + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(examSessions))
                    {
                        qryExamSession = " and etd.exam_session in(" + examSessions + ")";
                    }
                    else
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select " + lblExamSession.Text.Trim() + " And Then Proceed";
                        return;
                    }
                }
                else
                {
                    FSNominee.Visible = false;
                    btngen.Visible = false;
                    lblnorec.Visible = true;
                    lblnorec.Text = "No " + lblExamSession.Text.Trim() + " Were Found";
                    return;
                }
                int selsubjects = 0;
                string strsubjectcode = string.Empty;
                if (chklistsub.Items.Count > 0)
                {
                    for (int cd = 0; cd < chklistsub.Items.Count; cd++)
                    {
                        if (chklistsub.Items[cd].Selected == true)
                        {
                            selsubjects++;
                            if (strsubjectcode == "")
                            {
                                strsubjectcode = "'" + chklistsub.Items[cd].Value.ToString() + "'";
                            }
                            else
                            {
                                strsubjectcode = strsubjectcode + ",'" + chklistsub.Items[cd].Value.ToString() + "'";
                            }
                        }
                    }
                    if (strsubjectcode.Trim() != "")
                    {
                        strsubjectcode = " and s.subject_code in(" + strsubjectcode + ")";
                    }
                    if (selsubjects == 0)
                    {
                        FSNominee.Visible = false;
                        btngen.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select The Subject And Then Proceed";
                        return;
                    }
                }
                else
                {
                    FSNominee.Visible = false;
                    btngen.Visible = false;
                    lblnorec.Visible = true;
                    lblnorec.Text = "No Subject(s) Were Found";
                    return;
                }
                //es.edate convert(varchar(50),es.edate,103) edate class_master cm
                //string spreadbind1 = "select distinct es.roomno,COUNT(1) as strength,es.ses_sion,es.edate,r.degree_code,dp.dept_name,r.batch_year,es.subject_no,es.bundle_no  from registration r,exam_details ed,exam_application ea,exam_appl_details ead,exam_seating as es,degree d,department dp,subject s where s.subject_no=es.subject_no and ead.subject_no=s.subject_no and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no=r.roll_no and r.exam_flag<>'Debar' and es.regno=r.Reg_No and ead.subject_no=es.subject_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ed.degree_code=d.Degree_Code and r.degree_code=d.Degree_Code and dp.dept_code=d.dept_code and d.college_code=r.college_code  and ed.Exam_Month='" + ddlExamMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlExamYear.SelectedValue.ToString() + "' and es.roomno in (" + hallNo + ") and es.edate='" + examdate + "' " + qryDegreeCode + " " + qrySession + strsubjectcode + " and r.college_code in(" + collegeCode + ") group by es.roomno,es.ses_sion,es.edate ,r.degree_code,dp.dept_name,r.batch_year,es.subject_no,es.bundle_no ";
                string spreadbind1 = "select distinct Count(distinct ex.roll_no) as strength,etd.coll_code,s.subject_code,s.subject_name,s.subject_name+' ( '+s.subject_code+' )' as SubjectDetails,CONVERT(VARCHAR(50),etd.exam_date,103) exam_date,etd.exam_session,etd.exam_date as Date from exmtt et,exmtt_det etd,subject s,exam_appl_details ea,exam_application ex,Registration r where ex.roll_no=r.Roll_No and r.degree_code=et.degree_code and r.Batch_Year=et.batchFrom and ex.appl_no=ea.appl_no and et.exam_code=etd.exam_code and etd.subject_no=s.subject_no and etd.subject_no=ea.subject_no and ea.subject_no=s.subject_no " + strsubjectcode + qryCollege + qryExamSession + qryExamDate + " and et.Exam_year='" + ddlExamYear.SelectedValue.ToString() + "' and et.Exam_month='" + ddlExamMonth.SelectedValue.ToString() + "' group by s.subject_code,s.subject_name,etd.exam_date,etd.exam_session,etd.coll_code  order by exam_date asc,etd.exam_session desc,s.subject_code,etd.coll_code";
                DataSet ds2 = da.select_method_wo_parameter(spreadbind1, "Text");
                DataSet dsAllStudents = new DataSet();
                string qry = "select distinct r.Roll_No,r.Reg_No,r.Stud_Name,etd.coll_code,r.degree_code,r.Batch_Year,r.Current_Semester,s.subject_code,s.subject_name,s.subject_name+' ( '+s.subject_code+' )' as SubjectDetails,etd.exam_date,etd.exam_session from exmtt et,exmtt_det etd,subject s,exam_appl_details ea,exam_application ex ,Registration r where ex.roll_no=r.Roll_No and r.degree_code=et.degree_code and r.Batch_Year=et.batchFrom and ex.appl_no=ea.appl_no and et.exam_code=etd.exam_code and etd.subject_no=s.subject_no and etd.subject_no=ea.subject_no and ea.subject_no=s.subject_no  " + strsubjectcode + qryCollege + qryExamSession + qryExamDate + " and et.Exam_year='" + ddlExamYear.SelectedValue.ToString() + "' and et.Exam_month='" + ddlExamMonth.SelectedValue.ToString() + "' order by  etd.coll_code,r.Batch_Year,r.Degree_code,r.Reg_No,etd.exam_date asc,etd.exam_session desc,s.subject_code";
                dsAllStudents = da.select_method_wo_parameter(qry, "Text");
                FarPoint.Web.Spread.CheckBoxCellType chkOneByOne = new FarPoint.Web.Spread.CheckBoxCellType();
                FarPoint.Web.Spread.CheckBoxCellType chkSelectAll = new FarPoint.Web.Spread.CheckBoxCellType();
                chkSelectAll.AutoPostBack = true;
                string strength = string.Empty;
                string roomno = string.Empty;
                string sesson = string.Empty;
                string exdate = string.Empty;
                string dept = string.Empty;
                string bun = string.Empty;
                string degrrcode = string.Empty;
                string batchyr = string.Empty;
                string sbjno = string.Empty;
                int sno = 0;
                int height = 45;
                if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                {
                    Init_Spread(1);
                    ht.Clear();
                    FpPhasing.Width = 950;
                    FpPhasing.Visible = true;
                    FpPhasing.Sheets[0].RowCount = 0;
                    FpPhasing.Sheets[0].RowCount++;
                    FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 1].CellType = chkSelectAll;
                    FpPhasing.Sheets[0].SpanModel.Add(FpPhasing.Sheets[0].RowCount - 1, 2, 1, 3);
                    sno = 0;
                    for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                    {
                        string subjectCode = Convert.ToString(ds2.Tables[0].Rows[i]["subject_code"]).Trim();
                        string subjectName = Convert.ToString(ds2.Tables[0].Rows[i]["subject_name"]).Trim();
                        string subjectDetails = Convert.ToString(ds2.Tables[0].Rows[i]["SubjectDetails"]).Trim();
                        string studentsCont = Convert.ToString(ds2.Tables[0].Rows[i]["strength"]).Trim();
                        string examDate = Convert.ToString(ds2.Tables[0].Rows[i]["exam_date"]).Trim();
                        string examSession = Convert.ToString(ds2.Tables[0].Rows[i]["exam_session"]).Trim();
                        string collCOde = Convert.ToString(ds2.Tables[0].Rows[i]["coll_code"]).Trim();
                        string ddate = Convert.ToString(ds2.Tables[0].Rows[i]["Date"]).Trim();
                        if (!ht.Contains(examDate + "-" + examSession))
                        {
                            FpPhasing.Sheets[0].RowCount++;
                            FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].Text = examDate + " - " + examSession;
                            FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].BackColor = ColorTranslator.FromHtml("#458547");
                            FpPhasing.Sheets[0].Rows[FpPhasing.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#458547");
                            FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].Locked = true;
                            FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                            FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            ht.Add(examDate + "-" + examSession, examDate + "-" + examSession);
                        }
                        string regNo = string.Empty;
                        DataView dvAllStudent = new DataView();
                        if (dsAllStudents.Tables.Count > 0 && dsAllStudents.Tables[0].Rows.Count > 0)
                        {
                            dsAllStudents.Tables[0].DefaultView.RowFilter = "subject_code='" + subjectCode + "'";
                            dvAllStudent = dsAllStudents.Tables[0].DefaultView;
                        }
                        if (dvAllStudent.Count > 0)
                        {
                            foreach (DataRowView drv in dvAllStudent)
                            {
                                if (string.IsNullOrEmpty(regNo))
                                {
                                    regNo = "'" + Convert.ToString(drv["Reg_No"]).Trim() + "'";
                                }
                                else
                                {
                                    regNo += ",'" + Convert.ToString(drv["Reg_No"]).Trim() + "'";
                                }
                            }
                        }
                        sno++;
                        FpPhasing.Sheets[0].RowCount++;
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno).Trim();
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(examDate).Trim();
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(examSession).Trim();
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 0].Locked = true;
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 1].CellType = chkOneByOne;
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 1].Tag = ddate;
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(subjectDetails).Trim();
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(subjectCode).Trim();
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].Note = Convert.ToString(subjectName).Trim();
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].Locked = true;
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;

                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(studentsCont).Trim();
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(regNo).Trim();
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(collCOde).Trim();
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 3].Locked = true;
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    }
                    FpPhasing.SaveChanges();
                    FpPhasing.Sheets[0].PageSize = FpPhasing.Sheets[0].RowCount;
                    FpPhasing.Height = 500;
                    FpPhasing.SaveChanges();
                    FpPhasing.Visible = true;
                    divPhasing.Visible = true;
                    rptprint1.Visible = true;
                }
                else
                {
                    rptprint1.Visible = false;
                    divPhasing.Visible = false;
                    lblnorec.Visible = true;
                    lblnorec.Text = "No Record(s) Were Found";
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }

    protected void FSNominee_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string activerow = FSNominee.ActiveSheetView.ActiveRow.ToString();
            string activecol = FSNominee.ActiveSheetView.ActiveColumn.ToString();
            if (activerow == "0" && activecol == "1")
            {
                int val = 0;
                string getval = e.EditValues[1].ToString();
                if (getval.Trim().ToLower() == "true")
                {
                    val = 1;
                }
                for (int i = 1; i < FSNominee.Sheets[0].RowCount; i++)
                {
                    FSNominee.Sheets[0].Cells[i, 1].Value = val;
                }
            }
        }
        catch
        {
        }
    }

    public void bindbutn()
    {
        try
        {
            System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
            System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
            System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
            System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage;
            mypdfpage = mydoc.NewPage();
            FSNominee.SaveChanges();
            DataRow dr = dt.NewRow();
            string query = string.Empty;
            string currentsem = string.Empty;
            string sem = string.Empty;
            string year = string.Empty;
            string month = string.Empty;
            string degreecode = string.Empty;
            string degreename = string.Empty;
            string subjectname = string.Empty;
            string subjectcode = string.Empty;
            string date = string.Empty;
            string collegeCodeNew = string.Empty;
            string session = string.Empty;
            string arrsem = string.Empty;
            string dept_acromy = string.Empty;
            int columnvalue = 0;
            int checkcount = 0;
            int cvg = 0;
            bool flag = false;
            DataSet ds7 = new DataSet();
            ArrayList arrCollege = new ArrayList();
            if (FSNominee.Sheets[0].RowCount > 0)
            {
                for (int row = 1; row < FSNominee.Sheets[0].RowCount; row++)
                {
                    int isval = 0;
                    isval = Convert.ToInt32(FSNominee.Sheets[0].Cells[row, 1].Value);
                    if (isval == 1)
                    {
                        flag = true;
                        year = Convert.ToString(ddlExamYear.SelectedItem.Text);
                        month = Convert.ToString(ddlExamMonth.SelectedItem.Value);
                        subjectname = FSNominee.Sheets[0].Cells[row, 6].Text.ToString();
                        subjectcode = FSNominee.Sheets[0].Cells[row, 5].Text.ToString();
                        degreecode = FSNominee.Sheets[0].Cells[row, 2].Tag.ToString();
                        dept_acromy = FSNominee.Sheets[0].Cells[row, 2].Text.ToString();
                        session = FSNominee.Sheets[0].Cells[row, 4].Text.ToString();
                        date = FSNominee.Sheets[0].Cells[row, 3].Tag.ToString();
                        string semba = FSNominee.Sheets[0].Cells[row, 5].Tag.ToString();
                        collegeCodeNew = string.Empty;
                        collegeCodeNew = Convert.ToString(FSNominee.Sheets[0].Cells[row, 0].Tag);
                        query = "select  distinct len(r.reg_no),r.reg_no ,r.Current_Semester from subjectchooser sc,subject s,exmtt_det ex,exmtt e,registration r where r.delflag=0 and r.exam_flag<>'Debare' and r.reg_no<>' 'and r.roll_no=sc.roll_no and sc.subject_no=ex.subject_no and sc.semester=e.Semester and ex.subject_no=s.subject_no and s.subject_Name='" + subjectname + "' and s.subject_code='" + subjectcode + "'and convert(varchar(20),ex.exam_date,105)='" + date + "' and ex.Exam_Session='" + session + "' and ex.exam_code=e.exam_code and e.exam_Month='" + month + "' and e.exam_Year='" + year + "' and r.degree_code in(" + degreecode + ")  order by len(r.reg_no),r.reg_no ";
                        query = query + "select  distinct len(r.reg_no), r.reg_no  from mark_entry m,exam_details e,exmtt_det ex,exmtt et,subject s,registration r where r.delflag=0 and r.exam_flag<>'Debare' and r.roll_no<>' ' and r.roll_no=m.roll_no and m.passorfail=0 and m.result='Fail' and m.attempts>1 and s.subject_no=m.subject_no and m.subject_no=ex.subject_no and ex.subject_no=s.subject_no and s.subject_Name='" + subjectname + "' and s.subject_code='" + subjectcode + "' and convert(varchar(20),ex.exam_date,105)='" + date + "' and ex.Exam_Session='" + session + "' and ex.exam_code=et.exam_code and et.exam_month='" + month + "' and et.exam_year='" + year + "' and m.exam_code=e.exam_code and e.current_Semester >= 1 and e.current_Semester <et.Semester and r.degree_code in (" + degreecode + ") order by len(r.reg_no),r.reg_no";
                        query = " select distinct ed.Exam_Month,ed.Exam_year,ed.batch_year,ed.degree_code,ed.current_semester,ea.roll_no,s.subject_name,s.subject_code ,s.subject_no ,ead.attempts,r.Reg_No from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and r.Roll_No=ea.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and s.subject_code='" + subjectcode + "' and ed.degree_code in(" + degreecode + ") and ed.Exam_Month='" + month + "' and ed.Exam_year='" + year + "' and s.subject_no in(select et.subject_no from exmtt e,exmtt_det et,subject s1 where e.exam_code=et.exam_code and s1.subject_no=et.subject_no and e.Exam_month='" + month + "' and e.Exam_year='" + year + "' and e.degree_code in(" + degreecode + ") and s1.subject_code='" + subjectcode + "' ) order by ead.attempts,r.reg_no";
                        ds = da.select_method_wo_parameter(query, "Text");
                        string collegenew1 = string.Empty;
                        string address1 = string.Empty;
                        string affiateby = string.Empty;
                        string univrersity = string.Empty;
                        // string state = string.Empty;
                        //string phoneno = string.Empty;
                        //string website = string.Empty;
                        //string email = string.Empty;
                        string collegetitle = "select collname , (address1+','+address2+','+address3)as address ,affliatedby,(district+','+state+','+pincode) as state ,phoneno,website ,email,pincode,University from collinfo where college_code='" + collegeCodeNew + "'";
                        ds7.Clear();
                        ds7 = da.select_method_wo_parameter(collegetitle, "Text");
                        if (ds7.Tables.Count > 0 && ds7.Tables[0].Rows.Count > 0)
                        {
                            for (int count = 0; count < ds7.Tables[0].Rows.Count; count++)
                            {
                                collegenew1 = Convert.ToString(ds7.Tables[0].Rows[count]["collname"]);
                                address1 = Convert.ToString(ds7.Tables[0].Rows[count]["address"]);
                                affiateby = Convert.ToString(ds7.Tables[0].Rows[count]["affliatedby"]);
                                univrersity = Convert.ToString(ds7.Tables[0].Rows[count]["University"]);
                                address1 = address1 + " Pin-" + Convert.ToString(ds7.Tables[0].Rows[count]["pincode"]) + "";
                            }
                        }
                        if (ddltype.SelectedIndex == 0)
                        {
                            if (cvg != 0)
                            {
                                mypdfpage.SaveToDocument();
                                mypdfpage = mydoc.NewPage();
                            }
                            cvg++;
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                            {
                                Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 20, 10, 450);
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                            {
                                Gios.Pdf.PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                mypdfpage.Add(LogoImage1, 500, 10, 450);
                            }
                            int y = 15;
                            PdfTextArea pdfcladdr = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, collegenew1);
                            mypdfpage.Add(pdfcladdr);
                            y = y + 15;
                            PdfTextArea pdfaddr1 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, affiateby);
                            mypdfpage.Add(pdfaddr1);

                            y = y + 15;
                            PdfTextArea pdfaddr223 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, univrersity);
                            mypdfpage.Add(pdfaddr223);

                            y = y + 15;
                            PdfTextArea pdfaddr2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, address1);
                            mypdfpage.Add(pdfaddr2);

                            y = y + 15;
                            PdfTextArea pdfdet = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "Details of Candidates Registered for the Examination - " + ddlExamMonth.SelectedItem.ToString() + " - " + ddlExamYear.SelectedItem.ToString() + "");
                            mypdfpage.Add(pdfdet);

                            y = y + 20;
                            PdfTextArea pdfaddr22389 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "Phasing Sheet");
                            mypdfpage.Add(pdfaddr22389);
                            y = y + 25;
                            PdfTextArea pdf = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Course:");
                            mypdfpage.Add(pdf);
                            PdfTextArea pdf001 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 130, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, dept_acromy.ToString());
                            mypdfpage.Add(pdf001);
                            y = y + 25;
                            PdfTextArea pdf0 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Subject Code:");
                            mypdfpage.Add(pdf0);
                            PdfTextArea pdf00 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 130, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, subjectcode.ToString());
                            mypdfpage.Add(pdf00);
                            PdfTextArea pdf02 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 100, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, degreename.ToString());
                            mypdfpage.Add(pdf02);
                            PdfTextArea pdf1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 250, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Subject Name:");
                            mypdfpage.Add(pdf1);
                            PdfTextArea pdf01 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 340, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, subjectname.ToString());
                            mypdfpage.Add(pdf01);
                            y = y + 25;
                            DateTime dtb = Convert.ToDateTime(date);
                            PdfTextArea pdf2 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Date:");
                            mypdfpage.Add(pdf2);
                            PdfTextArea pdf03 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 130, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, dtb.ToString("dd-MM-yyyy"));
                            mypdfpage.Add(pdf03);
                            PdfTextArea pdf4 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 460, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Session:");
                            mypdfpage.Add(pdf4);
                            PdfTextArea pdf04 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 510, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, session);
                            mypdfpage.Add(pdf04);
                            string currentsem1 = string.Empty;
                            addvalue.Clear();
                            addvalue.Clear();
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                for (int s = 0; s < ds.Tables[0].Rows.Count; s++)
                                {
                                    string attempts = ds.Tables[0].Rows[s]["attempts"].ToString();
                                    if (attempts.Trim() == "0")
                                    {
                                        currentsem = ds.Tables[0].Rows[s]["reg_no"].ToString();
                                        currentsem1 = ds.Tables[0].Rows[s]["Current_Semester"].ToString();
                                        addvalue.Add(currentsem);
                                    }
                                    else
                                    {
                                        currentsem1 = ds.Tables[0].Rows[s]["Current_Semester"].ToString();
                                        arrsem = Convert.ToString(ds.Tables[0].Rows[s]["reg_no"]);
                                        addvalue.Add(arrsem);
                                    }
                                }
                            }
                            PdfTextArea pdf323 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 250, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Semester:");
                            mypdfpage.Add(pdf323);
                            if (currentsem.Trim() == "")
                            {
                                currentsem = semba;
                            }
                            PdfTextArea pdf324 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 340, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(currentsem1));
                            mypdfpage.Add(pdf324);
                            int count_student = 0;
                            int check_count = addvalue.Count;
                            int sk = 250;
                            int x = 0;
                            if (addvalue.Count > 0)
                            {
                                int count = 0;
                                for (int i = 0; i < addvalue.Count; i++)
                                {
                                    count++;
                                    x = x + 90;
                                    if (x > 480)
                                    {
                                        x = 90;
                                        sk = sk + 40;
                                    }
                                    if (sk > 760)
                                    {
                                        x = 480;
                                        sk = sk + 40;
                                        PdfTextArea pdf0566 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, x - 85, sk, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Total No Of Student :");
                                        mypdfpage.Add(pdf0566);
                                        if (check_count > 25)
                                        {
                                            count_student = 25;
                                            check_count = check_count - 25;
                                        }
                                        else
                                        {
                                            count_student = check_count;
                                        }
                                        PdfTextArea pdf0567 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, x + 40, sk, 500, 30), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(count_student));
                                        mypdfpage.Add(pdf0567);
                                        mypdfpage.SaveToDocument();
                                        x = 90;
                                        sk = 40;
                                        mypdfpage = mydoc.NewPage();
                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                        {
                                            Gios.Pdf.PdfImage LogoImage3 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                            mypdfpage.Add(LogoImage3, 20, 10, 450);
                                        }
                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                        {
                                            Gios.Pdf.PdfImage LogoImage4 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                            mypdfpage.Add(LogoImage4, 500, 10, 450);
                                        }
                                        //PdfTextArea pdfcladdr1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 80, 15, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, collegenew1);
                                        //PdfTextArea pdfaddr11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 80, 30, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, address1);
                                        //PdfTextArea pdfaddr21 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 80, 45, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, address2);
                                        //PdfTextArea pdf11 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 20, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Course:");
                                        //PdfTextArea pdf0011 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 130, y + 20, 500, 30), System.Drawing.ContentAlignment.TopLeft, dept_acromy.ToString());
                                        //PdfTextArea pdf012 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 50, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Subject Code:");
                                        //PdfTextArea pdf00113 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 130, y + 50, 500, 30), System.Drawing.ContentAlignment.TopLeft, subjectcode.ToString());
                                        //PdfTextArea pdf01214 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 100, y + 20, 500, 30), System.Drawing.ContentAlignment.TopLeft, degreename.ToString());
                                        //PdfTextArea pdf115 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 250, y + 50, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Subject Name:");
                                        //PdfTextArea pdf016= new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 340, y + 50, 500, 30), System.Drawing.ContentAlignment.TopLeft, subjectname.ToString());
                                        //PdfTextArea pdf27 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 70, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Date:");
                                        //PdfTextArea pdf038 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 130, y + 70, 500, 30), System.Drawing.ContentAlignment.TopLeft, date);
                                        ////PdfTextArea pdf3 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 250, y + 70, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Semester:");
                                        //PdfTextArea pdf49 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 460, y + 70, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Session:");
                                        //PdfTextArea pdf0410 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 510, y + 70, 500, 30), System.Drawing.ContentAlignment.TopLeft, session);
                                        mypdfpage.Add(pdfaddr223);
                                        mypdfpage.Add(pdfcladdr);
                                        mypdfpage.Add(pdfaddr1);
                                        mypdfpage.Add(pdfaddr2);
                                        mypdfpage.Add(pdf);
                                        mypdfpage.Add(pdf001);
                                        mypdfpage.Add(pdf0);
                                        mypdfpage.Add(pdf00);
                                        mypdfpage.Add(pdf01);
                                        mypdfpage.Add(pdf02);
                                        mypdfpage.Add(pdf03);
                                        mypdfpage.Add(pdf04);
                                        mypdfpage.Add(pdf1);
                                        mypdfpage.Add(pdf2);
                                        //mypdfpage.Add(pdf3);
                                        mypdfpage.Add(pdf4);
                                    }
                                    if (count > 25)
                                    {
                                        count = 1;
                                        x = 480;
                                        sk = sk + 40;
                                        PdfTextArea pdf0566 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, x - 85, sk, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Total No Of Student :");
                                        mypdfpage.Add(pdf0566);
                                        if (check_count > 25)
                                        {
                                            count_student = 25;
                                            check_count = check_count - 25;
                                        }
                                        else
                                        {
                                            count_student = check_count;
                                        }
                                        PdfTextArea pdf0567 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, x + 40, sk, 500, 30), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(count_student));
                                        mypdfpage.Add(pdf0567);
                                        mypdfpage.SaveToDocument();
                                        x = 90;
                                        sk = 250;
                                        mypdfpage = mydoc.NewPage();
                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                        {
                                            Gios.Pdf.PdfImage LogoImage6 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                            mypdfpage.Add(LogoImage6, 20, 10, 450);
                                        }
                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                        {
                                            Gios.Pdf.PdfImage LogoImage17 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                            mypdfpage.Add(LogoImage17, 500, 10, 450);
                                        }
                                        //PdfTextArea pdfcladdr = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 80, 15, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, collegenew1);
                                        //PdfTextArea pdfaddr1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 80, 30, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, address1);
                                        //PdfTextArea pdfaddr2 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 80, 45, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, address2);
                                        //PdfTextArea pdf = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 20, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Course:");
                                        //PdfTextArea pdf001 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 130, y + 20, 500, 30), System.Drawing.ContentAlignment.TopLeft, dept_acromy.ToString());
                                        //PdfTextArea pdf0 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 50, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Subject Code:");
                                        //PdfTextArea pdf00 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 130, y + 50, 500, 30), System.Drawing.ContentAlignment.TopLeft, subjectcode.ToString());
                                        //PdfTextArea pdf02 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 100, y + 20, 500, 30), System.Drawing.ContentAlignment.TopLeft, degreename.ToString());
                                        //PdfTextArea pdf1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 250, y + 50, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Subject Name:");
                                        //PdfTextArea pdf01 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 340, y + 50, 500, 30), System.Drawing.ContentAlignment.TopLeft, subjectname.ToString());
                                        //PdfTextArea pdf2 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 70, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Date:");
                                        //PdfTextArea pdf03 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 130, y + 70, 500, 30), System.Drawing.ContentAlignment.TopLeft, date);
                                        ////PdfTextArea pdf3 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 250, y + 70, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Semester:");
                                        //PdfTextArea pdf4 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 460, y + 70, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Session:");
                                        //PdfTextArea pdf04 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 510, y + 70, 500, 30), System.Drawing.ContentAlignment.TopLeft, session);
                                        mypdfpage.Add(pdfaddr223);
                                        mypdfpage.Add(pdfcladdr);
                                        mypdfpage.Add(pdfaddr1);
                                        mypdfpage.Add(pdfaddr2);
                                        mypdfpage.Add(pdf);
                                        mypdfpage.Add(pdf001);
                                        mypdfpage.Add(pdf0);
                                        mypdfpage.Add(pdf00);
                                        mypdfpage.Add(pdf01);
                                        mypdfpage.Add(pdf02);
                                        mypdfpage.Add(pdf03);
                                        mypdfpage.Add(pdf04);
                                        mypdfpage.Add(pdf1);
                                        mypdfpage.Add(pdf2);
                                        //mypdfpage.Add(pdf3);
                                        mypdfpage.Add(pdf4);
                                    }
                                    PdfTextArea pdf05 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, x, sk, 500, 30), System.Drawing.ContentAlignment.TopLeft, addvalue[i].ToString());
                                    mypdfpage.Add(pdf05);
                                }
                                x = 480;
                                sk = sk + 40;
                                PdfTextArea pdf0566222 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, x - 85, sk, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Total No Of Student :");
                                mypdfpage.Add(pdf0566222);
                                if (check_count > 25)
                                {
                                    count_student = 25;
                                    check_count = check_count - 25;
                                }
                                else
                                {
                                    count_student = check_count;
                                }
                                PdfTextArea pdf0567777 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, x + 40, sk, 500, 30), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(count_student));
                                mypdfpage.Add(pdf0567777);
                            }
                        }
                        else if (ddltype.SelectedIndex == 1)
                        {
                            int y = columnvalue + 15;
                            if (columnvalue >= 700)
                            {
                                mypdfpage.SaveToDocument();
                                mypdfpage = mydoc.NewPage();
                                columnvalue = 0;
                                y = 40;
                            }
                            if (checkcount == 0 || !arrCollege.Contains(collegeCodeNew))
                            {
                                arrCollege.Add(collegeCodeNew);
                                if (checkcount != 0)
                                {
                                    mypdfpage.SaveToDocument();
                                    mypdfpage = mydoc.NewPage();
                                    columnvalue = 0;
                                    y = 40;
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                {
                                    Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                    mypdfpage.Add(LogoImage, 20, 10, 450);
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                {
                                    Gios.Pdf.PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                    mypdfpage.Add(LogoImage1, 500, 10, 450);
                                }
                                PdfTextArea pdfcladdr = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "NOMINAL ROLL");
                                mypdfpage.Add(pdfcladdr);
                                y = y + 15;
                                PdfTextArea pdfaddr1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, collegenew1);
                                mypdfpage.Add(pdfaddr1);
                                y = y + 15;
                                PdfTextArea pdfaddr223 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, univrersity);
                                mypdfpage.Add(pdfaddr223);
                                y = y + 15;
                                PdfTextArea pdfaddr2 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "Exam Month & Year : " + Convert.ToString(ddlExamMonth.SelectedItem.Text).ToUpper() + "   " + Convert.ToString(ddlExamYear.SelectedItem.Text) + "");
                                mypdfpage.Add(pdfaddr2);
                                checkcount++;
                            }
                            if (y >= 700)
                            {
                                mypdfpage.SaveToDocument();
                                mypdfpage = mydoc.NewPage();
                                y = 40;
                            }
                            string[] splitvalue = dept_acromy.Split('-');
                            if (y >= 700)
                            {
                                mypdfpage.SaveToDocument();
                                mypdfpage = mydoc.NewPage();
                                y = 40;
                            }
                            y = y + 15;
                            PdfTextArea pdfdet = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Degree :  " + Convert.ToString(splitvalue[0]) + "");
                            mypdfpage.Add(pdfdet);
                            //y = y + 20;
                            PdfTextArea pdfaddr22389 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 180, y, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "Branch :  " + Convert.ToString(splitvalue[1]) + "");
                            mypdfpage.Add(pdfaddr22389);
                            //y = y + 25;
                            //PdfTextArea pdf = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Course:");
                            //mypdfpage.Add(pdf);
                            //PdfTextArea pdf001 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 130, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, dept_acromy.ToString());
                            //mypdfpage.Add(pdf001);
                            if (y >= 700)
                            {
                                mypdfpage.SaveToDocument();
                                mypdfpage = mydoc.NewPage();
                                y = 40;
                            }
                            y = y + 25;
                            PdfTextArea pdf0 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Subject Code:");
                            mypdfpage.Add(pdf0);
                            PdfTextArea pdf00 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 130, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, subjectcode.ToString());
                            mypdfpage.Add(pdf00);
                            //PdfTextArea pdf02 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 100, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, degreename.ToString());
                            //mypdfpage.Add(pdf02);
                            PdfTextArea pdf1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 200, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Subject Name:");
                            mypdfpage.Add(pdf1);
                            PdfTextArea pdf01 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 300, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, subjectname.ToString());
                            mypdfpage.Add(pdf01);
                            if (y >= 700)
                            {
                                mypdfpage.SaveToDocument();
                                mypdfpage = mydoc.NewPage();
                                y = 40;
                            }
                            y = y + 25;
                            DateTime dtb = Convert.ToDateTime(date);
                            PdfTextArea pdf2 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Date:");
                            mypdfpage.Add(pdf2);
                            PdfTextArea pdf03 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, dtb.ToString("dd-MM-yyyy"));
                            mypdfpage.Add(pdf03);
                            PdfTextArea pdf4 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 460, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Session:");
                            mypdfpage.Add(pdf4);
                            PdfTextArea pdf04 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 510, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, session);
                            mypdfpage.Add(pdf04);
                            string currentsem1 = string.Empty;
                            addvalue.Clear();
                            addvalue.Clear();
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                for (int s = 0; s < ds.Tables[0].Rows.Count; s++)
                                {
                                    string attempts = ds.Tables[0].Rows[s]["attempts"].ToString();
                                    if (attempts.Trim() == "0")
                                    {
                                        currentsem = ds.Tables[0].Rows[s]["reg_no"].ToString();
                                        currentsem1 = ds.Tables[0].Rows[s]["Current_Semester"].ToString();
                                        addvalue.Add(currentsem);
                                    }
                                    else
                                    {
                                        currentsem1 = ds.Tables[0].Rows[s]["Current_Semester"].ToString();
                                        arrsem = Convert.ToString(ds.Tables[0].Rows[s]["reg_no"]);
                                        addvalue.Add(arrsem);
                                    }
                                }
                            }
                            if (addvalue.Count > 0)
                            {
                                int x = 20;
                                if (y >= 700)
                                {
                                    mypdfpage.SaveToDocument();
                                    mypdfpage = mydoc.NewPage();
                                    y = 20;
                                }
                                y = y + 25;
                                for (int i = 0; i < addvalue.Count; i++)
                                {
                                    if (x <= 480)
                                    {
                                        if (i != 0)
                                        {
                                            x += 80;
                                        }
                                        PdfTextArea pdf045 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, x, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(addvalue[i]));
                                        mypdfpage.Add(pdf045);
                                    }
                                    else
                                    {
                                        x = 20;
                                        if (y >= 700)
                                        {
                                            mypdfpage.SaveToDocument();
                                            mypdfpage = mydoc.NewPage();
                                            y = 20;
                                        }
                                        y = y + 15;
                                        PdfTextArea pdf045 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, x, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(addvalue[i]));
                                        mypdfpage.Add(pdf045);
                                    }
                                }
                                if (y >= 700)
                                {
                                    mypdfpage.SaveToDocument();
                                    mypdfpage = mydoc.NewPage();
                                    y = 20;
                                }
                                y = y + 20;
                                PdfTextArea pdf0566222 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 500, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Total: " + Convert.ToString(addvalue.Count) + "");
                                mypdfpage.Add(pdf0566222);
                                //if (y >= 700)
                                //{
                                //    mypdfpage.SaveToDocument();
                                //    mypdfpage = mydoc.NewPage();
                                //    y = 20;
                                //}
                                //y = y + 20;
                                //PdfTextArea pdf05662221 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 20, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, "________________________________________________________________________________________________________________________");
                                //mypdfpage.Add(pdf05662221);
                            }
                            columnvalue = y;
                        }
                        //Rajkumar 26/01/2018
                        else if (ddltype.SelectedIndex == 4)
                        {
                            int size = 0;
                            if (!string.IsNullOrEmpty(txtSize.Text))
                            {
                                int.TryParse(txtSize.Text, out size);
                            }
                            if (size == 0 || string.IsNullOrEmpty(txtSize.Text))
                            {
                                lblAlert.Text = "pls Enter Size";
                                imgAlert.Visible = true;
                                return;
                            }
                            int y = columnvalue + 15;
                            if (columnvalue >= 700)
                            {
                                mypdfpage.SaveToDocument();
                                mypdfpage = mydoc.NewPage();
                                columnvalue = 0;
                                y = 40;
                            }
                            if (checkcount == 0 || !arrCollege.Contains(collegeCodeNew))
                            {
                                arrCollege.Add(collegeCodeNew);
                                if (checkcount != 0)
                                {
                                    mypdfpage.SaveToDocument();
                                    mypdfpage = mydoc.NewPage();
                                    columnvalue = 0;
                                    y = 40;
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                {
                                    Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                    mypdfpage.Add(LogoImage, 20, 10, 450);
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                {
                                    Gios.Pdf.PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                    mypdfpage.Add(LogoImage1, 500, 10, 450);
                                }
                                PdfTextArea pdfcladdr = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "NOMINAL ROLL");
                                mypdfpage.Add(pdfcladdr);
                                y = y + 15;
                                PdfTextArea pdfaddr1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, collegenew1);
                                mypdfpage.Add(pdfaddr1);
                                y = y + 15;
                                PdfTextArea pdfaddr223 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, univrersity);
                                mypdfpage.Add(pdfaddr223);
                                y = y + 15;
                                PdfTextArea pdfaddr2 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "Exam Month & Year : " + Convert.ToString(ddlExamMonth.SelectedItem.Text).ToUpper() + "   " + Convert.ToString(ddlExamYear.SelectedItem.Text) + "");
                                mypdfpage.Add(pdfaddr2);
                                checkcount++;
                            }
                            if (y >= 700)
                            {
                                mypdfpage.SaveToDocument();
                                mypdfpage = mydoc.NewPage();
                                y = 40;
                            }
                            string[] splitvalue = dept_acromy.Split('-');
                            if (y >= 700)
                            {
                                mypdfpage.SaveToDocument();
                                mypdfpage = mydoc.NewPage();
                                y = 40;
                            }
                            y = y + 15;
                            PdfTextArea pdfdet = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Degree :  " + Convert.ToString(splitvalue[0]) + "");
                            mypdfpage.Add(pdfdet);
                            //y = y + 20;
                            PdfTextArea pdfaddr22389 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 180, y, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "Branch :  " + Convert.ToString(splitvalue[1]) + "");
                            mypdfpage.Add(pdfaddr22389);

                            if (y >= 700)
                            {
                                mypdfpage.SaveToDocument();
                                mypdfpage = mydoc.NewPage();
                                y = 40;
                            }
                            y = y + 25;
                            PdfTextArea pdf0 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Subject Code:");
                            mypdfpage.Add(pdf0);
                            PdfTextArea pdf00 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 130, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, subjectcode.ToString());
                            mypdfpage.Add(pdf00);

                            PdfTextArea pdf1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 200, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Subject Name:");
                            mypdfpage.Add(pdf1);
                            PdfTextArea pdf01 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 300, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, subjectname.ToString());
                            mypdfpage.Add(pdf01);
                            if (y >= 700)
                            {
                                mypdfpage.SaveToDocument();
                                mypdfpage = mydoc.NewPage();
                                y = 40;
                            }
                            y = y + 25;
                            DateTime dtb = Convert.ToDateTime(date);
                            PdfTextArea pdf2 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Date:");
                            mypdfpage.Add(pdf2);
                            PdfTextArea pdf03 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, dtb.ToString("dd-MM-yyyy"));
                            mypdfpage.Add(pdf03);
                            PdfTextArea pdf4 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 460, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Session:");
                            mypdfpage.Add(pdf4);
                            PdfTextArea pdf04 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 510, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, session);
                            mypdfpage.Add(pdf04);
                            string currentsem1 = string.Empty;
                            addvalue.Clear();
                            addvalue.Clear();
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                for (int s = 0; s < ds.Tables[0].Rows.Count; s++)
                                {
                                    string attempts = ds.Tables[0].Rows[s]["attempts"].ToString();
                                    if (attempts.Trim() == "0")
                                    {
                                        currentsem = ds.Tables[0].Rows[s]["reg_no"].ToString();
                                        currentsem1 = ds.Tables[0].Rows[s]["Current_Semester"].ToString();
                                        addvalue.Add(currentsem);
                                    }
                                    else
                                    {
                                        currentsem1 = ds.Tables[0].Rows[s]["Current_Semester"].ToString();
                                        arrsem = Convert.ToString(ds.Tables[0].Rows[s]["reg_no"]);
                                        addvalue.Add(arrsem);
                                    }
                                }
                            }
                            if (addvalue.Count > 0)
                            {
                                int x = 20;
                                if (y >= 700)
                                {
                                    mypdfpage.SaveToDocument();
                                    mypdfpage = mydoc.NewPage();
                                    y = 20;
                                }
                                y = y + 25;

                                int count = 0;
                                for (int i = 0; i < addvalue.Count; i++)
                                {
                                    count++;
                                    if (i > size-1)
                                    {
                                        if (i % size == 0)
                                        {
                                            if (y >= 700)
                                            {
                                                mypdfpage.SaveToDocument();
                                                mypdfpage = mydoc.NewPage();
                                                y = 20;
                                            }
                                            y = y + 20;
                                            PdfTextArea pdf0566222 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 500, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Total: " + Convert.ToString(count-1) + "");
                                            mypdfpage.Add(pdf0566222);
                                            count = 1;
                                            y = y + 15;
                                            PdfTextArea pdfdet1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Degree :  " + Convert.ToString(splitvalue[0]) + "");
                                            mypdfpage.Add(pdfdet1);
                                            //y = y + 20;
                                            PdfTextArea pdfaddr223891 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 180, y, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "Branch :  " + Convert.ToString(splitvalue[1]) + "");
                                            mypdfpage.Add(pdfaddr223891);

                                            if (y >= 700)
                                            {
                                                mypdfpage.SaveToDocument();
                                                mypdfpage = mydoc.NewPage();
                                                y = 40;
                                            }
                                            y = y + 25;
                                            PdfTextArea pdf001 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Subject Code:");
                                            mypdfpage.Add(pdf001);
                                            PdfTextArea pdf002 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 130, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, subjectcode.ToString());
                                            mypdfpage.Add(pdf002);

                                            PdfTextArea pdf11 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 200, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Subject Name:");
                                            mypdfpage.Add(pdf11);
                                            PdfTextArea pdf011 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 300, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, subjectname.ToString());
                                            mypdfpage.Add(pdf011);
                                            if (y >= 700)
                                            {
                                                mypdfpage.SaveToDocument();
                                                mypdfpage = mydoc.NewPage();
                                                y = 40;
                                            }
                                            y = y + 25;
                                            //DateTime dtb1 = Convert.ToDateTime(date);
                                            PdfTextArea pdf21 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Date:");
                                            mypdfpage.Add(pdf21);
                                            PdfTextArea pdf031 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, dtb.ToString("dd-MM-yyyy"));
                                            mypdfpage.Add(pdf031);
                                            PdfTextArea pdf41 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 460, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Session:");
                                            mypdfpage.Add(pdf41);
                                            PdfTextArea pdf041 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 510, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, session);
                                            mypdfpage.Add(pdf041);
                                            y = y + 15;
                                        }
                                        if (x <= 480)
                                        {
                                            if (i != 0)
                                            {
                                                x += 80;
                                            }
                                            //y = y + 15;
                                            PdfTextArea pdf045 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, x, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(addvalue[i]));
                                            mypdfpage.Add(pdf045);
                                        }
                                        else
                                        {
                                            x = 20;//20
                                            if (y >= 700)
                                            {
                                                mypdfpage.SaveToDocument();
                                                mypdfpage = mydoc.NewPage();
                                                y = 20;
                                            }
                                            y = y + 15;
                                            PdfTextArea pdf045 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, x, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(addvalue[i]));
                                            mypdfpage.Add(pdf045);
                                        }

                                    }
                                    else if (x <= 480)
                                        {
                                            if (i != 0)
                                            {
                                                x += 80;
                                            }
                                            PdfTextArea pdf045 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, x, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(addvalue[i]));
                                            mypdfpage.Add(pdf045);
                                        }

                                        else
                                        {
                                            x = 20;
                                            if (y >= 700)
                                            {
                                                mypdfpage.SaveToDocument();
                                                mypdfpage = mydoc.NewPage();
                                                y = 20;
                                            }
                                            y = y + 15;
                                            PdfTextArea pdf045 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, x, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(addvalue[i]));
                                            mypdfpage.Add(pdf045);
                                        }

                                    //}
                                }
                                if (y >= 700)
                                {
                                    mypdfpage.SaveToDocument();
                                    mypdfpage = mydoc.NewPage();
                                    y = 20;
                                }
                                y = y + 20;
                                PdfTextArea pdf05662221 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 500, y, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Total: " + Convert.ToString(count) + "");
                                mypdfpage.Add(pdf05662221);
                            }
                            columnvalue = y;
                            }
                    }
                }
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "NominalRoll" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                    mypdfpage.SaveToDocument();
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
            if (flag == false)
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Any one Record\");", true);
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = Convert.ToString(ex);
            lblerror.Visible = true;
        }
    }

    public void bindconsolidate()
    {
        try
        {
            FSNominee.SaveChanges();
            System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
            System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
            System.Drawing.Font Fontsmallbold = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
            System.Drawing.Font fontbody = new System.Drawing.Font("Book Antiqua", 8, FontStyle.Regular);
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            DataRow dr = dt.NewRow();
            string query = string.Empty;
            string currentsem = string.Empty;
            string sem = string.Empty;
            string year = string.Empty;
            string month = string.Empty;
            string degreecode = string.Empty;
            string degreename = string.Empty;
            string subjectname = string.Empty;
            string subjectcode = string.Empty;
            string date = string.Empty;
            string session = string.Empty;
            string arrsem = string.Empty;
            string dept_acromy = string.Empty;
            bool flag = false;
            DataSet ds7 = new DataSet();
            if (FSNominee.Sheets[0].RowCount > 0)
            {
                string collegenew1 = string.Empty;
                string address1 = string.Empty;
                string affiateby = string.Empty;
                string state = string.Empty;
                string phoneno = string.Empty;
                string website = string.Empty;
                string email = string.Empty;
                string university = string.Empty;
                string pincode = string.Empty;
                collegeCode = string.Empty;
                if (cblCollege.Items.Count > 0)
                {
                    collegeCode = getCblSelectedValue(cblCollege);
                }
                string collegetitle = "select collname , (address1+','+address2+','+address3)as address ,affliatedby,(district+','+state+','+pincode) as state ,phoneno,website ,email,university,pincode from collinfo where college_code in(" + collegeCode + ")";
                ds7.Clear();
                ds7 = da.select_method_wo_parameter(collegetitle, "Text");
                if (ds7.Tables[0].Rows.Count > 0)
                {
                    for (int count = 0; count < ds7.Tables[0].Rows.Count; count++)
                    {
                        collegenew1 = Convert.ToString(ds7.Tables[0].Rows[count]["collname"]);
                        address1 = Convert.ToString(ds7.Tables[0].Rows[count]["address"]);
                        affiateby = Convert.ToString(ds7.Tables[0].Rows[count]["affliatedby"]);
                        state = Convert.ToString(ds7.Tables[0].Rows[count]["state"]);
                        phoneno = Convert.ToString(ds7.Tables[0].Rows[count]["phoneno"]);
                        website = Convert.ToString(ds7.Tables[0].Rows[count]["website"]);
                        email = Convert.ToString(ds7.Tables[0].Rows[count]["email"]);
                        university = Convert.ToString(ds7.Tables[0].Rows[count]["university"]);
                        pincode = Convert.ToString(ds7.Tables[0].Rows[count]["pincode"]);
                    }
                }
                Gios.Pdf.PdfPage mypdfpage;
                int totalrowcount = 0;
                string tempdate = string.Empty;
                int startrow = 0;
                Boolean exonfalg = false;
                for (int hrow = 1; hrow < FSNominee.Sheets[0].RowCount; hrow++)
                {
                reset:
                    session = FSNominee.Sheets[0].Cells[hrow, 4].Text.ToString();
                    date = FSNominee.Sheets[0].Cells[hrow, 3].Tag.ToString();
                    string getdate = date + '*' + session;
                    string collCode = FSNominee.Sheets[0].Cells[hrow, 0].Tag.ToString();
                    if (tempdate == "")
                    {
                        tempdate = getdate;
                        startrow = hrow;
                    }
                    if (tempdate != getdate || hrow == FSNominee.Sheets[0].RowCount - 1)
                    {
                        totalrowcount = 0;
                        startrow = 0;
                        Boolean fadsetg = false;
                        int endora = 0;
                        for (int hrow1 = 1; hrow1 < FSNominee.Sheets[0].RowCount; hrow1++)
                        {
                            session = FSNominee.Sheets[0].Cells[hrow1, 4].Text.ToString();
                            date = FSNominee.Sheets[0].Cells[hrow1, 3].Tag.ToString();
                            if (tempdate == date + '*' + session)
                            {
                                totalrowcount++;
                                if (fadsetg == false)
                                {
                                    startrow = hrow1;
                                    fadsetg = true;
                                }
                                endora = hrow1;
                            }
                        }
                        totalrowcount = endora - startrow;
                        totalrowcount++;
                        int nopage = totalrowcount / 12;
                        int expage = totalrowcount % 12;
                        if (expage > 0)
                        {
                            nopage++;
                        }
                        expage = startrow;
                        int finalrow = 0;
                        string tempdegree = string.Empty;
                        int srno = 0;
                        int endrow = 0;
                        int datetotal = 0;
                        int degtotal = 0;
                        for (int np = 0; np < nopage; np++)
                        {
                            mypdfpage = mydoc.NewPage();
                            if (np > 0)
                            {
                                expage = expage + 12;
                                if (totalrowcount > (np + 1) * 12)
                                {
                                    finalrow = 11;
                                    endrow = endrow + 12;
                                }
                                else
                                {
                                    finalrow = totalrowcount - (((np) * 12));
                                    endrow = endrow + totalrowcount - (((np) * 12));
                                }
                            }
                            else
                            {
                                if (totalrowcount > 12)
                                {
                                    finalrow = 11;
                                    endrow = startrow + 11;
                                }
                                else
                                {
                                    finalrow = totalrowcount - 1;
                                    endrow = endora;
                                }
                            }
                            DataView dvCollege = new DataView();
                            if (ds7.Tables[0].Rows.Count > 0)
                            {
                                ds7.Tables[0].DefaultView.RowFilter = "college_code='" + collCode + "'";
                                dvCollege = ds7.Tables[0].DefaultView;
                                for (int count = 0; count < dvCollege.Count; count++)
                                {
                                    collegenew1 = Convert.ToString(dvCollege[count]["collname"]);
                                    address1 = Convert.ToString(dvCollege[count]["address"]);
                                    affiateby = Convert.ToString(dvCollege[count]["affliatedby"]);
                                    state = Convert.ToString(dvCollege[count]["state"]);
                                    phoneno = Convert.ToString(dvCollege[count]["phoneno"]);
                                    website = Convert.ToString(dvCollege[count]["website"]);
                                    email = Convert.ToString(dvCollege[count]["email"]);
                                    university = Convert.ToString(dvCollege[count]["university"]);
                                    pincode = Convert.ToString(dvCollege[count]["pincode"]);
                                }
                            }
                            int y = 20;
                            PdfTextArea pdfcladdr = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 20, y, 600, 30), System.Drawing.ContentAlignment.MiddleCenter, collegenew1);
                            mypdfpage.Add(pdfcladdr);
                            y = y + 15;
                            PdfTextArea pdfaddr1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, y, 600, 30), System.Drawing.ContentAlignment.MiddleCenter, affiateby);
                            mypdfpage.Add(pdfaddr1);
                            y = y + 15;
                            PdfTextArea pdfuniv = new PdfTextArea(Fontsmallbold, System.Drawing.Color.Black, new PdfArea(mydoc, 20, y, 600, 30), System.Drawing.ContentAlignment.MiddleCenter, university);
                            mypdfpage.Add(pdfuniv);
                            y = y + 15;
                            PdfTextArea pdfaddr2 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, y, 600, 30), System.Drawing.ContentAlignment.MiddleCenter, address1 + " Pin - " + pincode);
                            mypdfpage.Add(pdfaddr2);
                            y = y + 20;
                            PdfTextArea pdfaddr22389 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, y, 600, 30), System.Drawing.ContentAlignment.MiddleCenter, "Details of Candidates Registered for the Examination (Subject wise list)- " + ddlExamMonth.SelectedItem.ToString() + " - " + ddlExamYear.SelectedItem.ToString() + "");
                            mypdfpage.Add(pdfaddr22389);
                            y = y + 30;
                            string[] sptemp = tempdate.Split('*');
                            DateTime dtb = Convert.ToDateTime(sptemp[0].ToString());
                            PdfTextArea pdfdatev = new PdfTextArea(Fontsmallbold, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Date : " + dtb.ToString("dd-MM-yyyy") + "");
                            mypdfpage.Add(pdfdatev);
                            PdfTextArea pdfsessions = new PdfTextArea(Fontsmallbold, System.Drawing.Color.Black, new PdfArea(mydoc, 380, y, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Session : " + sptemp[1].ToString() + "");
                            mypdfpage.Add(pdfsessions);
                            Gios.Pdf.PdfTable table;
                            if (np == nopage - 1 && finalrow < 12 && np > 0)
                            {
                                table = mydoc.NewTable(Fontsmall, (finalrow * 2) + 2, 6, 1);
                            }
                            else
                            {
                                table = mydoc.NewTable(Fontsmall, (finalrow * 2) + 4, 6, 1);
                            }
                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            table.VisibleHeaders = false;
                            table.Cell(0, 0).SetContent("S.No");
                            table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Columns[0].SetWidth(50);
                            table.Columns[0].SetFont(Fontsmallbold);
                            table.Cell(0, 1).SetContent("Course");
                            table.Columns[1].SetWidth(270);
                            table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Columns[1].SetFont(Fontsmallbold);
                            table.Cell(0, 2).SetContent("Subject Code");
                            table.Columns[2].SetWidth(130);
                            table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Columns[2].SetFont(Fontsmallbold);
                            table.Cell(0, 3).SetContent("Subject Name");
                            table.Columns[3].SetWidth(280);
                            table.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Columns[3].SetFont(Fontsmallbold);
                            table.Cell(0, 4).SetContent("Semester");
                            table.Columns[4].SetWidth(80);
                            table.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Columns[4].SetFont(Fontsmallbold);
                            table.Cell(0, 5).SetContent("No. of Candidates Registered");
                            table.Columns[5].SetWidth(150);
                            table.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Columns[5].SetFont(Fontsmallbold);
                            y = y + 30;
                            int rowset = 1;
                            for (int row = expage; row <= endrow; row++)
                            {
                                int noofst = 0;
                                flag = true;
                                year = Convert.ToString(ddlExamYear.SelectedItem.Text);
                                month = Convert.ToString(ddlExamMonth.SelectedItem.Value);
                                subjectname = FSNominee.Sheets[0].Cells[row, 6].Text.ToString();
                                subjectcode = FSNominee.Sheets[0].Cells[row, 5].Text.ToString();
                                degreecode = FSNominee.Sheets[0].Cells[row, 2].Tag.ToString();
                                dept_acromy = FSNominee.Sheets[0].Cells[row, 2].Text.ToString();
                                session = FSNominee.Sheets[0].Cells[row, 4].Text.ToString();
                                date = FSNominee.Sheets[0].Cells[row, 3].Tag.ToString();
                                string semester = FSNominee.Sheets[0].Cells[row, 5].Tag.ToString();
                                string subjsem = FSNominee.Sheets[0].Cells[row, 6].Tag.ToString();
                                string att = " and ead.attempts=0";
                                if (semester.Trim().ToLower() == "arrear")
                                {
                                    att = " and ead.attempts>0";
                                }
                                query = "select  count(  distinct ea.roll_no) from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and s.subject_code='" + subjectcode + "' and ed.degree_code='" + degreecode + "' and ed.Exam_Month='" + month + "' and ed.Exam_year='" + year + "' " + att + " and s.subject_no in(select et.subject_no from exmtt e,exmtt_det et,subject s1 where e.exam_code=et.exam_code and s1.subject_no=et.subject_no and e.Exam_month='" + month + "' and e.Exam_year='" + year + "' and e.degree_code='" + degreecode + "' and s1.subject_code='" + subjectcode + "')";
                                //string strab = da.GetFunction("select count(ea.roll_no) as stucount from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Degree d,course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and ed.exam_Year='" + year + "'  and ed.Exam_Month='" + month + "' and ed.degree_code in('" + degreecode + "') ");
                                string strab = da.GetFunction(query);
                                srno++;
                                datetotal = datetotal + Convert.ToInt32(strab);
                                noofst = Convert.ToInt32(strab);
                                table.Cell(rowset, 0).SetContent(srno.ToString());
                                table.Cell(rowset, 0).SetFont(fontbody);
                                table.Cell(rowset, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(rowset, 1).SetContent(dept_acromy);
                                table.Cell(rowset, 1).SetFont(fontbody);
                                table.Cell(rowset, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(rowset, 2).SetContent(subjectcode);
                                table.Cell(rowset, 2).SetFont(fontbody);
                                table.Cell(rowset, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(rowset, 3).SetFont(fontbody);
                                table.Cell(rowset, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(rowset, 3).SetContent(subjectname);
                                table.Cell(rowset, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(rowset, 4).SetFont(fontbody);
                                table.Cell(rowset, 4).SetContent(subjsem);
                                table.Cell(rowset, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(rowset, 5).SetContent(strab);
                                table.Cell(rowset, 5).SetFont(fontbody);
                                if (tempdegree != dept_acromy + '-' + subjectcode)
                                {
                                    degtotal = noofst;
                                    tempdegree = dept_acromy + '-' + subjectcode;
                                }
                                else
                                {
                                    degtotal = degtotal + noofst;
                                }
                                rowset++;
                                table.Cell(rowset, 5).SetContent(degtotal.ToString());
                                table.Cell(rowset, 0).SetContent("Sub Total");
                                table.Cell(rowset, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                                foreach (PdfCell pc in table.CellRange(rowset, 0, rowset, 0).Cells)
                                {
                                    pc.ColSpan = 5;
                                }
                                rowset++;
                            }
                            table.Columns[0].SetCellPadding(5);
                            table.Columns[1].SetCellPadding(5);
                            table.Columns[2].SetCellPadding(5);
                            table.Columns[5].SetCellPadding(5);
                            table.Columns[3].SetCellPadding(5);
                            table.Columns[4].SetCellPadding(5);
                            table.Cell(rowset, 0).SetContent("Total");
                            table.Cell(rowset, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            foreach (PdfCell pc in table.CellRange(rowset, 0, rowset, 0).Cells)
                            {
                                pc.ColSpan = 5;
                            }
                            table.Cell(rowset, 5).SetContent(datetotal.ToString());
                            Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, y, 550, 960));
                            mypdfpage.Add(newpdftabpage);
                            mypdfpage.SaveToDocument();
                        }
                        startrow = hrow;
                        totalrowcount = 1;
                        if (tempdate != getdate && hrow == FSNominee.Sheets[0].RowCount - 1 && exonfalg == false)
                        {
                            session = FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 4].Text.ToString();
                            date = FSNominee.Sheets[0].Cells[FSNominee.Sheets[0].RowCount - 1, 3].Tag.ToString();
                            tempdate = date + '*' + session;
                            exonfalg = true;
                            goto reset;
                        }
                        tempdate = getdate;
                    }
                }
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "NominalRoll" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
            if (flag == false)
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Any one Record\");", true);
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = Convert.ToString(ex);
            lblerror.Visible = true;
        }
    }

    protected void btngen_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkconsolidate.Checked == false)
            {
                bindbutn();
            }
            else
            {
                bindconsolidate();
            }
        }
        catch
        {
        }
    }

    protected void redbtnsubject_change(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            chkconsolidate.Visible = true;
            chkconsolidate.Checked = false;
            divCourse.Visible = true;
            lblCourse.Visible = true;
            txtcourse.Visible = true;
            txtsub.Visible = true;
            divSubjectName.Visible = true;
            ddldegree.Visible = false;
            lblSubjectName.Visible = true;
            FSNominee.Visible = false;
            panel1.Visible = true;
            panel14.Visible = true;
            btnprintpdf.Visible = false;
            btngen.Visible = false;
            lblSubjectName.Visible = true;
            ddltype.Visible = true;
            ddltype.Enabled = true;            
            lblFormat.Visible = true;
            divHall.Visible = false;
            divPhasing.Visible = false;
            rptprint1.Visible = false;
            divStudentWise.Visible = true;
            chkNeedSubjectTotal.Visible = false;
            chkNeedSubjectTotal.Checked = false;
            chkIncludeDepartmentWise.Visible = false;
            chkIncludeDepartmentWise.Checked = false;
            chkWithoutRegularArrear.Visible = false;
            chkWithoutRegularArrear.Checked = false;
            if (ddltype.Items.Count > 0)
            {
                if (ddltype.SelectedIndex == 2)
                {
                    divHall.Visible = true;
                }
                else if (ddltype.SelectedIndex == 3)
                {
                    //divHall.Visible = true;
                }
            }
        }
        catch
        {
        }
    }

    protected void rdbtnstudent_change(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            divCourse.Visible = true;
            lblCourse.Visible = true;
            chkconsolidate.Checked = false;
            chkconsolidate.Visible = false;
            txtcourse.Visible = true;
            txtsub.Visible = false;
            divSubjectName.Visible = false;
            panel1.Visible = false;
            panel14.Visible = true;
            ddldegree.Visible = false;
            lblSubjectName.Visible = false;
            btnprintpdf.Visible = false;
            FSNominee.Visible = false;
            btngen.Visible = false;
            lblSubjectName.Visible = false;
            ddltype.Visible = false;
            lblFormat.Visible = false;
            divHall.Visible = false;
            divPhasing.Visible = false;
            rptprint1.Visible = false;
            divStudentWise.Visible = true;
            chkNeedSubjectTotal.Visible = false;
            chkNeedSubjectTotal.Checked = false;
            chkIncludeDepartmentWise.Visible = false;
            chkIncludeDepartmentWise.Checked = false;
            chkWithoutRegularArrear.Visible = false;
            chkWithoutRegularArrear.Checked = false;
            if (ddltype.Items.Count > 0)
            {
                ddltype.SelectedIndex = 0;
            }
        }
        catch
        {
        }
    }

    protected void btnprintpdf_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Examination Nominal Roll " + '@' + "Exam Month & Year:" + ddlExamMonth.SelectedItem.Text + " & " + ddlExamYear.SelectedItem.Text + "";
            string pagename = "Nominal_Roll.aspx";
            if (rdbtnsubject.Checked == true && chkconsolidate.Checked == true)
            {
                degreedetails = "Details of Candidates Registered for the Examination (Subject wise list) - " + ddlExamMonth.SelectedItem.ToString() + " - " + ddlExamYear.SelectedItem.ToString();
            }
            Printcontrol.loadspreaddetails(FSNominee, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }

    protected void chkconsolidate_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            chkNeedSubjectTotal.Visible = false;
            chkNeedSubjectTotal.Checked = false;
            chkIncludeDepartmentWise.Visible = false;
            chkIncludeDepartmentWise.Checked = false;
            chkWithoutRegularArrear.Visible = false;
            chkWithoutRegularArrear.Checked = false;
            if (chkconsolidate.Checked == true)
            {
                FSNominee.Visible = false;
                btngen.Visible = false;
                btnprintpdf.Visible = false;
                Printcontrol.Visible = false;
                ddltype.Enabled = false;
                if (ddltype.Items.Count > 0)
                {
                    ddltype.SelectedIndex = 0;
                }
                rdbtnsubject.Checked = true;
                rdbtnstudent.Checked = false;
                chkNeedSubjectTotal.Visible = true;
                chkNeedSubjectTotal.Checked = true;
                chkWithoutRegularArrear.Visible = true;
                chkWithoutRegularArrear.Checked = true;
                chkIncludeDepartmentWise.Visible = true;
                chkIncludeDepartmentWise.Checked = true;
            }
            else
            {
                ddltype.Enabled = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FSNominee.Visible = false;
            btngen.Visible = false;
            btnprintpdf.Visible = false;
            Printcontrol.Visible = false;
            divHall.Visible = false;
            divPhasing.Visible = false;
            rptprint1.Visible = false;
            divCourse.Visible = true;
            lblCourse.Visible = true;
            if (ddltype.Items.Count > 0)
            {
                if (ddltype.SelectedIndex == 2)
                {
                    divHall.Visible = true;
                    Label1.Visible = false;
                    txtSize.Visible = false;
                }
                else if (ddltype.SelectedIndex == 3)
                {
                    divCourse.Visible = false;
                    lblCourse.Visible = false;
                    Label1.Visible = false;
                    txtSize.Visible = false;
                    loadSubjectName();
                }
                else if (ddltype.SelectedIndex == 4)
                {
                    //divHall.Visible = true;
                    Label1.Visible = true;
                    txtSize.Visible = true;
                }
                else
                {
                    Label1.Visible = false;
                    txtSize.Visible = false;
                }
            }
        }
        catch
        {

        }
    }

    #region Added By Malang Raja On Nov 04 2016

    protected void chkCollege_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FSNominee.Visible = false;
            btngen.Visible = false;
            btnprintpdf.Visible = false;
            Printcontrol.Visible = false;
            divPhasing.Visible = false;
            rptprint1.Visible = false;
            CallCheckboxChange(chkCollege, cblCollege, txtCollege, lblCollege.Text, "--Select--");
            bindcourse();
            loadSubjectName();
            Bindhallno();
        }
        catch
        {
        }
    }

    protected void cblCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FSNominee.Visible = false;
            btngen.Visible = false;
            btnprintpdf.Visible = false;
            Printcontrol.Visible = false;
            divPhasing.Visible = false;
            rptprint1.Visible = false;
            CallCheckboxListChange(chkCollege, cblCollege, txtCollege, lblCollege.Text, "--Select--");
            bindcourse();
            Bindhallno();
            loadSubjectName();
        }
        catch
        {
        }
    }

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append("'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
                    }
                    else
                    {
                        selectedvalue.Append(",'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }

    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append("'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                    else
                    {
                        selectedText.Append(",'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }

    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = string.Empty;
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 0)
                {
                    txt.Text = deft;
                }
                else if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
    }

    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = string.Empty;
            cb.Checked = false;
            txt.Text = deft;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
        }
        catch { }
    }

    private void checkBoxListselectOrDeselect(CheckBoxList cbl, bool selected = true)
    {
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                li.Selected = selected;
            }
        }
        catch
        {
        }
    }

    #endregion

    #endregion

    #region Added By Malang Raja On Nov 05 2016

    public void Init_Spread(int type = 0)
    {
        try
        {
            #region FpSpread Style

            FpPhasing.Visible = false;
            FpPhasing.Sheets[0].ColumnCount = 0;
            FpPhasing.Sheets[0].RowCount = 0;
            FpPhasing.Sheets[0].SheetCorner.ColumnCount = 0;
            FpPhasing.CommandBar.Visible = false;

            #endregion FpSpread Style

            FpPhasing.Visible = false;
            FpPhasing.CommandBar.Visible = false;
            FpPhasing.RowHeader.Visible = false;
            FpPhasing.Sheets[0].AutoPostBack = false;
            FpPhasing.Sheets[0].RowCount = 0;

            #region SpreadStyles

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            //darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Font.Bold = true;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.VerticalAlign = VerticalAlign.Middle;
            darkstyle.ForeColor = System.Drawing.Color.White;
            darkstyle.Border.BorderSize = 1;
            darkstyle.Border.BorderColor = System.Drawing.Color.Black;

            FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
            //sheetstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
            //darkstyle.ForeColor = System.Drawing.Color.Black;
            sheetstyle.Font.Name = "Book Antiqua";
            sheetstyle.Font.Size = FontUnit.Medium;
            sheetstyle.Font.Bold = true;
            sheetstyle.HorizontalAlign = HorizontalAlign.Left;
            sheetstyle.VerticalAlign = VerticalAlign.Middle;
            sheetstyle.ForeColor = System.Drawing.Color.Black;
            sheetstyle.Border.BorderSize = 1;
            sheetstyle.Border.BorderColor = System.Drawing.Color.Black;

            #endregion SpreadStyles

            FpPhasing.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpPhasing.Sheets[0].DefaultStyle = sheetstyle;
            FpPhasing.Sheets[0].ColumnHeader.RowCount = 2;
            FpPhasing.Sheets[0].SelectionBackColor = Color.Transparent;
            FpPhasing.Sheets[0].AutoPostBack = false;

            if (type == 0)
            {
                FpPhasing.Sheets[0].FrozenRowCount = 1;
                FpPhasing.Sheets[0].ColumnCount = 8;
                FpPhasing.Sheets[0].Columns[4].Locked = true;
                FpPhasing.Sheets[0].Columns[5].Locked = true;
                FpPhasing.Sheets[0].Columns[6].Locked = true;
                FpPhasing.Sheets[0].Columns[7].Locked = true;

                FpPhasing.Sheets[0].Columns[7].Visible = false;

                FpPhasing.Sheets[0].Columns[2].Width = 150;
                FpPhasing.Sheets[0].Columns[3].Width = 150;
                FpPhasing.Sheets[0].Columns[4].Width = 100;
                FpPhasing.Sheets[0].Columns[5].Width = 150;
                FpPhasing.Sheets[0].Columns[6].Width = 150;

                FpPhasing.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpPhasing.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                FpPhasing.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Hall No";
                FpPhasing.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Dept";
                FpPhasing.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total";
                FpPhasing.Sheets[0].ColumnHeader.Cells[0, 5].Text = "From";
                FpPhasing.Sheets[0].ColumnHeader.Cells[0, 6].Text = "To";
                FpPhasing.Sheets[0].ColumnHeader.Cells[0, 7].Text = "degreecode";

                FpPhasing.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpPhasing.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                FpPhasing.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                FpPhasing.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                FpPhasing.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                FpPhasing.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                FpPhasing.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;

                FpPhasing.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                FpPhasing.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                FpPhasing.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                FpPhasing.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
                FpPhasing.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
                FpPhasing.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
                FpPhasing.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;

                FpPhasing.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                FpPhasing.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                FpPhasing.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                FpPhasing.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
            }
            else
            {
                FpPhasing.Sheets[0].ColumnCount = 4;
                FpPhasing.Sheets[0].FrozenRowCount = 1;
                FpPhasing.Sheets[0].Columns[2].Width = 600;
                FpPhasing.Sheets[0].Columns[3].Width = 150;
                FpPhasing.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpPhasing.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                FpPhasing.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Name ( Subject Code )";
                FpPhasing.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Count";

                FpPhasing.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpPhasing.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                FpPhasing.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                FpPhasing.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;

                FpPhasing.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                FpPhasing.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                FpPhasing.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                FpPhasing.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            }

            FpPhasing.Sheets[0].Columns[0].Locked = true;
            FpPhasing.Sheets[0].Columns[2].Locked = true;
            FpPhasing.Sheets[0].Columns[3].Locked = true;

            FpPhasing.Sheets[0].Columns[0].Width = 40;
            FpPhasing.Sheets[0].Columns[1].Width = 80;

            FpPhasing.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpPhasing.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpPhasing.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            FpPhasing.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
        }
        catch (Exception ex)
        {
        }
    }

    public void Bindhallno()
    {
        try
        {
            string months = ddlExamMonth.SelectedValue.ToString();
            string years = ddlExamYear.SelectedValue.ToString();
            string datess = string.Empty;// ddlDate.SelectedItem.Text;
            string[] fromdatespit99;//= datess.ToString().Split('-');
            //datess = fromdatespit99[2] + '-' + fromdatespit99[1] + '-' + fromdatespit99[0];
            string qryDate = string.Empty;
            //if (ddlDate.Items.Count > 0)
            //{
            //    if (ddlDate.SelectedItem.Text.Trim().ToLower() != "all")
            //    {
            //        datess = ddlDate.SelectedItem.Text;
            //        fromdatespit99 = datess.ToString().Split('-');
            //        datess = fromdatespit99[2] + '-' + fromdatespit99[1] + '-' + fromdatespit99[0];
            //        if (!string.IsNullOrEmpty(datess))
            //        {
            //            qryDate = " and edate='" + datess + "' ";//" and es.edate='" + datess + "'";// " and etd.exam_date='" + examdate + "' ";
            //        }
            //    }
            //}
            cblHall.Items.Clear();
            txtHall.Text = "--Select--";
            chkHall.Checked = false;
            string session = string.Empty;
            //if (ddlSession.Items.Count > 0)
            //{
            //    if (ddlSession.SelectedItem.Text == "All")
            //    {
            //        session = string.Empty;
            //    }
            //    else
            //    {
            //        session = " and ses_sion='" + ddlSession.SelectedItem.Text + "'";
            //    }
            //}
            qryCollege = string.Empty;
            collegeCode = string.Empty;
            qryDate = string.Empty;
            string qrySession = string.Empty;
            if (cblCollege.Items.Count > 0)
            {
                collegeCode = getCblSelectedValue(cblCollege);
            }
            if (!string.IsNullOrEmpty(collegeCode.Trim()))
            {
                qryCollege = " and cm.coll_code in(" + collegeCode + ")";
            }
            if (cblExamDate.Items.Count > 0 && txtExamDate.Visible == true)
            {
                examDates = getCblSelectedValue(cblExamDate);
                if (!string.IsNullOrEmpty(examDates))
                {
                    qryExamDate = " and convert(varchar(20),es.edate,103) in(" + examDates + ")";
                    qryDate = " exam_date in(" + examDates + ")";
                }
            }
            else if (ddlExamDate.Items.Count > 0 && ddlExamDate.Visible == true)
            {
                examDates = string.Empty;
                foreach (ListItem li in ddlExamDate.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(examDates))
                        {
                            examDates = "'" + li.Value + "'";
                        }
                        else
                        {
                            examDates += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(examDates))
                {
                    qryExamDate = " and convert(varchar(20),es.edate,103) in(" + examDates + ")";
                    qryDate = " exam_date in(" + examDates + ")";
                }
            }
            if (cblExamSession.Items.Count > 0 && txtExamSession.Visible == true)
            {
                examSessions = getCblSelectedValue(cblExamSession);
                if (!string.IsNullOrEmpty(examSessions))
                {
                    qryExamSession = " and es.ses_sion in(" + examSessions + ")";
                    qrySession = " Exam_Session in (" + examSessions + ")";
                }
            }
            else if (ddlExamSession.Items.Count > 0 && ddlExamSession.Visible == true)
            {
                examSessions = string.Empty;
                foreach (ListItem li in ddlExamSession.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(examSessions))
                        {
                            examSessions = "'" + li.Value + "'";
                        }
                        else
                        {
                            examSessions += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(examSessions))
                {
                    qryExamSession = " and es.ses_sion in(" + examSessions + ")";
                }
            }

            //string strtype = string.Empty;
            //if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            //{
            //    if (ddltype.SelectedItem.ToString().Trim() != "" && ddltype.SelectedItem.ToString().Trim() != "ALL")
            //    {
            //        strtype = "and c.type='" + ddltype.SelectedItem.ToString() + "'";
            //    }
            //    if (ddltype.SelectedItem.ToString().Trim().ToLower() == "day")
            //    {
            //        strtype = "and c.type in('Day','MCA')";
            //    }
            //}
            // string getdeteails = "SELECT distinct roomno FROM exam_seating where edate='" + datess + "' " + sedd + "";
            string getdeteails = "SELECT distinct es.roomno,cm.priority,es.ses_sion as Exam_Session,convert(varchar(20),es.edate,103) as exam_date FROM exam_seating es,Registration r,Degree d,course c,class_master cm where cm.rno=es.roomno and es.regno=r.Reg_No and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id " + qryCollege + " order by cm.priority";//cm.rno=es.roomno  cm.priority
            DataSet dsHall = da.select_method_wo_parameter(getdeteails, "Text");
            DataTable dtHallNo = new DataTable();
            if (dsHall.Tables.Count > 0 && dsHall.Tables[0].Rows.Count > 0)
            {
                DataView dvHallNo = new DataView();
                dsHall.Tables[0].DefaultView.RowFilter = qryDate + "" + ((!string.IsNullOrEmpty(qrySession) ? (!string.IsNullOrEmpty(qryDate) ? " and " + qrySession : qrySession) : ""));
                dvHallNo = dsHall.Tables[0].DefaultView;
                dvHallNo.Sort = "priority";
                dtHallNo = dvHallNo.ToTable(true, "roomno", "priority");
            }
            if (dtHallNo.Rows.Count > 0)
            {
                cblHall.DataSource = dtHallNo;
                cblHall.DataTextField = "roomno";
                cblHall.DataValueField = "roomno";
                cblHall.DataBind();
            }
            else
            {
                cblHall.Items.Clear();
                txtHall.Text = "--Select--";
            }
            if (dsHall.Tables.Count > 0 && dsHall.Tables[0].Rows.Count > 0)
            {
                chkHall.Checked = true;
                for (int i = 0; i < cblHall.Items.Count; i++)
                {
                    cblHall.Items[i].Selected = true;
                    txtHall.Text = "Hall No(" + cblHall.Items.Count + ")";
                }
            }
            // Chkdep.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        }
        catch
        {
        }
    }

    protected void chkHall_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FSNominee.Visible = false;
            btngen.Visible = false;
            btnprintpdf.Visible = false;
            Printcontrol.Visible = false;
            divPhasing.Visible = false;
            rptprint1.Visible = false;
            CallCheckboxChange(chkHall, cblHall, txtHall, lblHall.Text, "--Select--");
        }
        catch
        {
        }
    }

    protected void cblHall_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FSNominee.Visible = false;
            btngen.Visible = false;
            btnprintpdf.Visible = false;
            Printcontrol.Visible = false;
            divPhasing.Visible = false;
            rptprint1.Visible = false;
            CallCheckboxListChange(chkHall, cblHall, txtHall, lblHall.Text, "--Select--");
        }
        catch
        {

        }
    }

    #region Generate Excel

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol1.Visible = false;
            string reportname = txtexcelname1.Text;
            if (Convert.ToString(reportname).Trim() != "")
            {
                if (FpPhasing.Visible == true)
                {
                    da.printexcelreport(FpPhasing, reportname);
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
        }
    }

    #endregion Generate Excel

    #region Print PDF

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string rptheadname = "Phasing Sheets";
            string pagename = "Nominal_Roll.aspx";
            if (FpPhasing.Visible == true)
            {
                Printcontrol1.loadspreaddetails(FpPhasing, pagename, rptheadname);
            }
            Printcontrol1.Visible = true;
            lbl_norec1.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    #endregion Print PDF

    #region Print Phasing Sheet

    protected void btnPrintPhasing_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddltype.SelectedIndex == 2)
            {
                DisplayPhaseSheet();
            }
            else if (ddltype.SelectedIndex == 3)
            {
                printCoverSheet();
            }
        }
        catch
        {
        }
    }

    protected void btnQPaperPacking_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddltype.SelectedIndex == 3)
            {
                printQuestionPaperPacking();
            }
        }
        catch
        {
        }
    }

    #endregion

    public void DisplayPhaseSheet()
    {
        try
        {
            FpPhasing.SaveChanges();
            int g = 0;
            string collgr = string.Empty;
            string affilitied = string.Empty;
            string collname = string.Empty;
            string pincode = string.Empty;
            string district = string.Empty;
            string Date = string.Empty;
            int mm = 0;
            int y = 0;
            string HallNo = string.Empty;
            string session = string.Empty;
            string hdeg = "", hroll = "", bndlee = string.Empty;
            string batch = string.Empty;
            string subno = string.Empty;
            string hall = string.Empty;
            DataSet dsdisplay = new DataSet();
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
            Font Fontbold = new Font("Book Antique", 10, FontStyle.Bold);
            Font Fontnormal = new Font("Book Antique", 10, FontStyle.Regular);
            Font Fonttitle = new Font("Book Antique", 9, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antique", 8, FontStyle.Regular);
            Font Fonthead = new Font("Book Antique", 10, FontStyle.Regular);
            Font head = new Font("Book Antique", 16, FontStyle.Bold);
            Boolean chkgenflag = false;
            DateTime dt = new DateTime();
            int coltop = 10;
            coltop = coltop + 5;
            int coltop1 = coltop;
            int finctop = coltop;
            int yq = 180;
            string strquery = string.Empty;
            int isval = 0;
            int ji = 0;
            int tablepadding = 10;
            strquery = "Select * from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
            DataSet ds = da.select_method_wo_parameter(strquery, "Text");
            string sml = da.GetFunction("select value from COE_Master_Settings where settings='Bundle Per Student'");
            //if (sml.Trim() != "" && sml.Trim() != "0")
            //{
            //if (Convert.ToInt32(sml) > 15)
            //{
            //    tablepadding = 3;
            //}
            //else
            //{
            //    tablepadding = 10;
            //}               
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds = da.select_method_wo_parameter(strquery, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    collname = ds.Tables[0].Rows[0]["collname"].ToString();
                    affilitied = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                    district = ds.Tables[0].Rows[0]["district"].ToString();
                    pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
                    string[] aff = affilitied.Split(',');
                    affilitied = aff[0].ToString();
                    boundvl.Clear();
                    HasSession.Clear();
                    Hasdegree.Clear();
                    Hashdenm.Clear();
                    Hashhall.Clear();
                    HashDate.Clear();
                    Hasroll.Clear();
                    hassubno.Clear();
                    hasbatch.Clear();
                    int u = 0;
                    for (mm = 0; mm < FpPhasing.Sheets[0].Rows.Count; mm++)
                    {
                        isval = Convert.ToInt32(FpPhasing.Sheets[0].Cells[u, 1].Value);
                        u = u + 1;
                        if (isval == 1 && u > 1)
                        {
                            y = y + 1;
                            chkgenflag = true;
                            //lblerr1.Visible = false;
                            //lblerr1.Text = string.Empty;
                            coltop = 10;
                            hall = FpPhasing.Sheets[0].Cells[u - 1, 2].Text.ToString();
                            dt = Convert.ToDateTime(FpPhasing.Sheets[0].Cells[u - 1, 2].Note.ToString());
                            PdfArea tete = new PdfArea(mydocument, 15, 10, 825, 565);
                            PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);
                            Date = FpPhasing.Sheets[0].Cells[u - 1, 2].Note.ToString();
                            string newDate = Date;
                            session = FpPhasing.Sheets[0].Cells[u - 1, 4].Note.ToString();
                            HallNo = FpPhasing.Sheets[0].Cells[u - 1, 2].Text.ToString();
                            hdeg = FpPhasing.Sheets[0].Cells[u - 1, 7].Text.ToString();
                            hroll = FpPhasing.Sheets[0].Cells[u - 1, 5].Text.ToString();
                            //hroll = "'" + hroll + "'  and  '" + FpPhasing.Sheets[0].Cells[u - 1, 7].Text.ToString() + "'";
                            // bndlee = FpPhasing.Sheets[0].Cells[u - 1, 5].Text.ToString();
                            batch = FpPhasing.Sheets[0].Cells[u - 1, 3].Note.ToString();
                            subno = FpPhasing.Sheets[0].Cells[u - 1, 0].Note.ToString();

                            string[] dummy_date_split = Date.Split(' ');
                            string[] dsplit = dummy_date_split[0].Split('/');
                            Date = dsplit[2].ToString() + "-" + dsplit[0].ToString() + "-" + dsplit[1].ToString();

                            newDate = dsplit[1].ToString().PadLeft(2, '0') + "-" + dsplit[0].ToString().PadLeft(2, '0') + "-" + dsplit[2].ToString();
                            collgr = Session["collegecode"].ToString();
                            //class_master cm cm.rno=es.roomno  cm.priority
                            string query = "select r.Reg_No,r.Stud_Name,r.Stud_Type,r.current_semester,s.subject_code,es.seat_no,es.roomno,(c.Course_Name +'-'+ d.Acronym)  as Grade,sc.semester,(s.subject_code +'-'+ s.subject_name) as subjectname ,(c.Course_Name +'-'+ de.dept_name) as deptname,cm.priority from Exam_Details ed,exam_application ea,exam_appl_details ead ,exam_seating es,Registration r,subject s,Degree d,course c,Department de,subjectchooser sc ,class_master cm where cm.rno=es.roomno and sc.subject_no=s.subject_no and sc.roll_no=r.roll_no and ea.roll_no=sc.roll_no and ead.subject_no=sc.subject_no and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and r.Roll_No=ea.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ed.degree_code=es.degree_code and es.degree_code=r.degree_code and es.regno=r.Reg_No and r.degree_code=d.Degree_Code and ed.degree_code=d.Degree_Code and es.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and ead.subject_no=s.subject_no and es.subject_no=s.subject_no and ed.Exam_Month='" + ddlExamMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlExamYear.SelectedValue.ToString() + "' and es.edate='" + Date + "' and es.ses_sion ='" + session + "' and r.degree_code='" + hdeg + "' and r.batch_year='" + batch + "' and roomno='" + HallNo + "' and es.subject_no  ='" + subno + "' and r.Reg_No in ('" + FpPhasing.Sheets[0].Cells[u - 1, 6].Tag.ToString() + "') order by cm.priority,es.seat_no";
                            dsdisplay = da.select_method_wo_parameter(query, "text");
                            int count = dsdisplay.Tables[0].Rows.Count;
                            int rOw = 0;
                        raja:
                            coltop = 10;
                            PdfTextArea ptc;
                            PdfTextArea ptColegeAddress = new PdfTextArea(head, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, 820, 50), System.Drawing.ContentAlignment.MiddleCenter, collname + "," + district + "-" + pincode);
                            mypdfpage.Add(ptColegeAddress);
                            coltop = coltop + 25;
                            PdfTextArea ptExam = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, 820, 50), System.Drawing.ContentAlignment.MiddleCenter, "Name of the Examinations : END SEMESTER EXAMINATIONS" + "-" + ddlExamMonth.SelectedItem.Text.ToUpper() + " " + ddlExamYear.SelectedItem.Text + "");
                            mypdfpage.Add(ptExam);
                            int rows = 6;
                            rows = (dsdisplay.Tables[0].Rows.Count / 5);
                            if (dsdisplay.Tables[0].Rows.Count % 5 > 0) rows++;
                            rows = 6;
                            Gios.Pdf.PdfTable table1 = mydocument.NewTable(Fontbold, rows, 10, 10);
                            table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            table1.VisibleHeaders = false;
                            table1.Columns[0].SetWidth(70);
                            table1.Columns[1].SetWidth(70);
                            table1.Columns[2].SetWidth(70);
                            table1.Columns[3].SetWidth(70);
                            table1.Columns[4].SetWidth(70);
                            table1.Columns[5].SetWidth(70);
                            table1.Columns[6].SetWidth(70);
                            table1.Columns[7].SetWidth(70);
                            table1.Columns[8].SetWidth(70);
                            table1.Columns[9].SetWidth(70);
                            table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(0, 0).SetContent("REG.No.");
                            table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(0, 1).SetContent("P / A");
                            table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(0, 2).SetContent("REG.No.");
                            table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(0, 3).SetContent("P / A");
                            table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(0, 4).SetContent("REG.No.");
                            table1.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(0, 5).SetContent("P / A");
                            table1.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(0, 6).SetContent("REG.No.");
                            table1.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(0, 7).SetContent("P / A");
                            table1.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(0, 8).SetContent("REG.No.");
                            table1.Cell(0, 9).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(0, 9).SetContent("P / A");
                            if (dsdisplay.Tables[0].Rows.Count > 0)
                            {
                                //for (ji = 0; ji < dsdisplay.Tables[0].Rows.Count; ji++)
                                //{
                                string deptname = dsdisplay.Tables[0].Rows[0]["deptname"].ToString();
                                string semester = dsdisplay.Tables[0].Rows[0]["semester"].ToString();
                                string sub_code = dsdisplay.Tables[0].Rows[0]["subject_code"].ToString();
                                string sub_name = dsdisplay.Tables[0].Rows[0]["subjectname"].ToString();
                                string[] sub = sub_name.Split('-');
                                string subname = sub[1].ToString();
                                coltop = coltop + 45;
                                Gios.Pdf.PdfTablePage newpdftabpage1;
                                Gios.Pdf.PdfTablePage newpdftabpage0;
                                Gios.Pdf.PdfTable table2;
                                PdfArea tete3;
                                PdfRectangle pr3;
                                PdfTextArea ptRoomNo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Room No");
                                mypdfpage.Add(ptRoomNo);
                                PdfTextArea ptHall = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 140, coltop, 180, 100), System.Drawing.ContentAlignment.TopLeft, ":  " + hall);
                                mypdfpage.Add(ptHall);
                                PdfTextArea ptBundle = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 260, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Bundle No");
                                mypdfpage.Add(ptBundle);
                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 370, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  ");
                                mypdfpage.Add(ptc);
                                coltop = coltop + 25;
                                PdfTextArea ptDate = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Date");
                                mypdfpage.Add(ptDate);
                                ptDate = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 140, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + newDate);
                                mypdfpage.Add(ptDate);
                                PdfTextArea ptSession = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 260, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Session");
                                mypdfpage.Add(ptSession);
                                ptSession = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 370, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + session);
                                mypdfpage.Add(ptSession);
                                coltop = coltop + 25;
                                PdfTextArea ptSubjectCode = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Code");
                                mypdfpage.Add(ptSubjectCode);
                                ptSubjectCode = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 140, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, ":  ");
                                mypdfpage.Add(ptSubjectCode);
                                ptSubjectCode = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 150, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, sub_code);
                                mypdfpage.Add(ptSubjectCode);
                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 260, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Title of the paper");//"Room No / Bundle No"
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 370, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + subname);//":  " + hall + " / " + bndlee
                                mypdfpage.Add(ptc);
                                bool newPage = false;
                                for (int cOl = 0; cOl < 10; cOl += 2)
                                {
                                    for (int rro = 1; rro < 6; rro++)
                                    {
                                        if (dsdisplay.Tables[0].Rows.Count > rOw)
                                        {
                                            //if(rOw < ds.Tables[0].Rows.Count )
                                            string regno = dsdisplay.Tables[0].Rows[rOw]["Reg_No"].ToString();
                                            string name = dsdisplay.Tables[0].Rows[rOw]["Stud_Name"].ToString();
                                            string roomno = dsdisplay.Tables[0].Rows[rOw]["roomno"].ToString();
                                            string seatno = dsdisplay.Tables[0].Rows[rOw]["seat_no"].ToString();
                                            table1.Cell(rro, cOl).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(rro, cOl).SetContent(regno.ToString());
                                            table1.Cell(rro, cOl).SetFont(Fontnormal);
                                            rOw++;
                                        }
                                        else
                                        {
                                            table1.Cell(rro, cOl).SetContent("\n");
                                        }
                                        #region HIDE
                                        //if (rOw % 25 == 0 && rOw != 0)
                                        //{
                                        //    coltop = 500;
                                        //    ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 35, (coltop - 130), 600, 20), System.Drawing.ContentAlignment.TopLeft, "PLEASE NOTE:");
                                        //    mypdfpage.Add(ptc);
                                        //    ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 45, (coltop - 130), 600, 50), System.Drawing.ContentAlignment.TopLeft, "\nI) THIS PACKET IS INTENDED TO HOLD 25 ANSWER BOOKS ONLY\nII) MARK 'P' FOR PRESENT AND 'AAA' FOR ABSENT IN THE BOX PROVIDED");
                                        //    mypdfpage.Add(ptc);
                                        //    ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 500, (coltop - 150), 600, 50), System.Drawing.ContentAlignment.TopLeft, "TOTAL NO. OF ANSWER BOOKS IN THE PACKET ");// (dsdisplay.Tables[0].Rows.Count > 25 ? 25 : dsdisplay.Tables[0].Rows.Count)
                                        //    mypdfpage.Add(ptc);
                                        //    ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 545, (coltop - 60), 250, 50), System.Drawing.ContentAlignment.MiddleCenter, "PACKED AND SEALED IN MY PRESENCE \n\n\n\nSIGNATURE OF CHEIF SUPDT");
                                        //    mypdfpage.Add(ptc);
                                        //    tete3 = new PdfArea(mydocument, 745, (coltop - 160), 60, 30);
                                        //    pr3 = new PdfRectangle(mydocument, tete3, Color.Black);
                                        //    mypdfpage.Add(pr3);
                                        //    //ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                        //    //                                new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Date");
                                        //    //mypdfpage.Add(ptc);
                                        //    ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 245, (coltop - 40), 300, 50), System.Drawing.ContentAlignment.MiddleCenter, "Signature of Invigilator\n\n(Name in Block Letters)");
                                        //    mypdfpage.Add(ptc);
                                        //    table2 = mydocument.NewTable(Fontbold, 3, 3, 5);
                                        //    table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        //    table2.VisibleHeaders = false;
                                        //    table2.Columns[0].SetWidth(70);
                                        //    table2.Columns[1].SetWidth(150);
                                        //    table2.Columns[2].SetWidth(70);
                                        //    table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //    table2.Cell(0, 1).SetContent("SIGNATURE");
                                        //    table2.Cell(0, 1).SetFont(Fontbold);
                                        //    table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //    table2.Cell(0, 2).SetContent("DATE");
                                        //    table2.Cell(0, 2).SetFont(Fontbold);
                                        //    table2.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //    table2.Cell(1, 0).SetContent("EXTERNAL");
                                        //    table2.Cell(1, 0).SetFont(Fontbold);
                                        //    table2.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //    table2.Cell(2, 0).SetContent("INTERNAL");
                                        //    table2.Cell(2, 0).SetFont(Fontbold);
                                        //    table2.Rows[1].SetCellPadding(10);
                                        //    table2.Rows[2].SetCellPadding(10);
                                        //    newpdftabpage0 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, (coltop - 30), 230, 80));
                                        //    mypdfpage.Add(newpdftabpage0);
                                        //    ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 35, (coltop - 65), 250, 50), System.Drawing.ContentAlignment.MiddleLeft, "VALUATION");
                                        //    mypdfpage.Add(ptc);
                                        //    ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 150, (coltop + 50), 200, 50), System.Drawing.ContentAlignment.MiddleRight, "CAMP OFFICER");
                                        //    mypdfpage.Add(ptc);
                                        //    newpdftabpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, (yq - 25), 810, 560));
                                        //    mypdfpage.Add(newpdftabpage1);
                                        //    //mypdfpage.Add(pr1);
                                        //    coltop += 40;
                                        //    g = 1;
                                        //    if (yq >= 180)
                                        //    {
                                        //        mypdfpage.SaveToDocument();
                                        //        mypdfpage = mydocument.NewPage();
                                        //        yq = 180;
                                        //    }
                                        //    mypdfpage = mydocument.NewPage();
                                        //    coltop = 10;
                                        //    ptc = new PdfTextArea(head, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, 820, 50), System.Drawing.ContentAlignment.MiddleCenter, collname + "," + district + "-" + pincode);
                                        //    mypdfpage.Add(ptColegeAddress);
                                        //    coltop = coltop + 25;
                                        //    ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, 820, 50), System.Drawing.ContentAlignment.MiddleCenter, "Name of the Examinations : END SEMESTER EXAMINATIONS" + "-" + ddlExamMonth.SelectedItem.Text + " " + ddlExamYear.SelectedItem.Text + "");
                                        //    mypdfpage.Add(ptExam);
                                        //    rows = 6;
                                        //    rows = (dsdisplay.Tables[0].Rows.Count / 5);
                                        //    if (dsdisplay.Tables[0].Rows.Count % 5 > 0) rows++;
                                        //    rows = 6;
                                        //    table1 = mydocument.NewTable(Fontbold, rows, 10, 10);
                                        //    table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        //    table1.VisibleHeaders = false;
                                        //    table1.Columns[0].SetWidth(70);
                                        //    table1.Columns[1].SetWidth(70);
                                        //    table1.Columns[2].SetWidth(70);
                                        //    table1.Columns[3].SetWidth(70);
                                        //    table1.Columns[4].SetWidth(70);
                                        //    table1.Columns[5].SetWidth(70);
                                        //    table1.Columns[6].SetWidth(70);
                                        //    table1.Columns[7].SetWidth(70);
                                        //    table1.Columns[8].SetWidth(70);
                                        //    table1.Columns[9].SetWidth(70);
                                        //    table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //    table1.Cell(0, 0).SetContent("REG.No.");
                                        //    table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //    table1.Cell(0, 1).SetContent("P / A");
                                        //    table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //    table1.Cell(0, 2).SetContent("REG.No.");
                                        //    table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //    table1.Cell(0, 3).SetContent("P / A");
                                        //    table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //    table1.Cell(0, 4).SetContent("REG.No.");
                                        //    table1.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //    table1.Cell(0, 5).SetContent("P / A");
                                        //    table1.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //    table1.Cell(0, 6).SetContent("REG.No.");
                                        //    table1.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //    table1.Cell(0, 7).SetContent("P / A");
                                        //    table1.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //    table1.Cell(0, 8).SetContent("REG.No.");
                                        //    table1.Cell(0, 9).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //    table1.Cell(0, 9).SetContent("P / A");
                                        //    coltop = coltop + 45;
                                        //    ptRoomNo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Room No");
                                        //    mypdfpage.Add(ptRoomNo);
                                        //    ptHall = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 140, coltop, 180, 100), System.Drawing.ContentAlignment.TopLeft, ":  " + hall);
                                        //    mypdfpage.Add(ptHall);
                                        //    ptBundle = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 260, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Bundle No");
                                        //    mypdfpage.Add(ptBundle);
                                        //    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 370, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  ");
                                        //    mypdfpage.Add(ptc);
                                        //    coltop = coltop + 25;
                                        //    ptDate = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Date");
                                        //    mypdfpage.Add(ptDate);
                                        //    ptDate = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 140, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + Date);
                                        //    mypdfpage.Add(ptDate);
                                        //    ptSession = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 260, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Session");
                                        //    mypdfpage.Add(ptSession);
                                        //    ptSession = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 370, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + session);
                                        //    mypdfpage.Add(ptSession);
                                        //    coltop = coltop + 25;
                                        //    ptSubjectCode = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Code");
                                        //    mypdfpage.Add(ptSubjectCode);
                                        //    ptSubjectCode = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 140, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, ":  ");
                                        //    mypdfpage.Add(ptSubjectCode);
                                        //    ptSubjectCode = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 150, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, sub_code);
                                        //    mypdfpage.Add(ptSubjectCode);
                                        //    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 260, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Title of the paper");//"Room No / Bundle No"
                                        //    mypdfpage.Add(ptc);
                                        //    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 370, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + subname);//":  " + hall + " / " + bndlee
                                        //    mypdfpage.Add(ptc);
                                        //    cOl = 0;
                                        //    rro = 1;
                                        //newPage = true;
                                        //} 
                                        #endregion
                                    }
                                    //if (newPage)
                                    //{
                                    //    cOl = 0;
                                    //}
                                }
                                coltop = 500;
                                ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 35, (coltop - 130), 600, 20), System.Drawing.ContentAlignment.TopLeft, "PLEASE NOTE:");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 45, (coltop - 130), 600, 50), System.Drawing.ContentAlignment.TopLeft, "\nI) THIS PACKET IS INTENDED TO HOLD 25 ANSWER BOOKS ONLY\nII) MARK 'P' FOR PRESENT AND 'AAA' FOR ABSENT IN THE BOX PROVIDED");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 500, (coltop - 150), 600, 50), System.Drawing.ContentAlignment.TopLeft, "TOTAL NO. OF ANSWER BOOKS IN THE PACKET ");// (dsdisplay.Tables[0].Rows.Count > 25 ? 25 : dsdisplay.Tables[0].Rows.Count)
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 545, (coltop - 60), 250, 50), System.Drawing.ContentAlignment.MiddleCenter, "PACKED AND SEALED IN MY PRESENCE \n\n\n\nSIGNATURE OF " + Convert.ToString("Chief Superintendent").ToUpper());
                                mypdfpage.Add(ptc);
                                tete3 = new PdfArea(mydocument, 745, (coltop - 160), 60, 30);
                                pr3 = new PdfRectangle(mydocument, tete3, Color.Black);
                                mypdfpage.Add(pr3);
                                //ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                //                                new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Date");
                                //mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 245, (coltop - 40), 300, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString("Signature of Invigilator").ToUpper() + "\n\n\n\n(Name in Block Letters)");
                                mypdfpage.Add(ptc);
                                table2 = mydocument.NewTable(Fontbold, 3, 3, 5);
                                table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table2.VisibleHeaders = false;
                                table2.Columns[0].SetWidth(70);
                                table2.Columns[1].SetWidth(150);
                                table2.Columns[2].SetWidth(70);
                                table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 1).SetContent("SIGNATURE");
                                table2.Cell(0, 1).SetFont(Fontbold);
                                table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 2).SetContent("DATE");
                                table2.Cell(0, 2).SetFont(Fontbold);
                                table2.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(1, 0).SetContent("EXTERNAL");
                                table2.Cell(1, 0).SetFont(Fontbold);
                                table2.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(2, 0).SetContent("INTERNAL");
                                table2.Cell(2, 0).SetFont(Fontbold);
                                table2.Rows[1].SetCellPadding(10);
                                table2.Rows[2].SetCellPadding(10);
                                newpdftabpage0 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, (coltop - 30), 230, 80));
                                mypdfpage.Add(newpdftabpage0);
                                ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 35, (coltop - 65), 250, 50), System.Drawing.ContentAlignment.MiddleLeft, "VALUATION");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 150, (coltop + 50), 200, 50), System.Drawing.ContentAlignment.MiddleRight, "CAMP OFFICER");
                                mypdfpage.Add(ptc);
                                newpdftabpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, (yq - 25), 810, 560));
                                mypdfpage.Add(newpdftabpage1);
                                //mypdfpage.Add(pr1);
                                coltop += 40;
                                g = 1;
                                if (yq >= 180)
                                {
                                    mypdfpage.SaveToDocument();
                                    mypdfpage = mydocument.NewPage();
                                    yq = 180;
                                }
                            }
                            if (rOw >= count)
                            {
                            }
                            else
                            {
                                goto raja;
                            }
                            //}
                            string appPath = HttpContext.Current.Server.MapPath("~");
                            if (appPath != "")
                            {
                                string szPath = appPath + "/Report/";
                                string szFile = "PhasingSheet_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                                mydocument.SaveToFile(szPath + szFile);
                                Response.ClearHeaders();
                                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                Response.ContentType = "application/pdf";
                                Response.WriteFile(szPath + szFile);
                            }
                            else
                            {
                                //lblerror1.Visible = true;
                                //lblerror1.Text = "No Records Found";
                            }
                        }
                    }
                }
                else
                {
                }
            }
            else
            {
            }
            if (chkgenflag == false)
            {
                //lblerror1.Visible = true;
                //lblerror1.Text = "Please Select Any One Record";
            }
            //}
            #region Hide
            //else
            //{
            //    ArrayList arr_subjectunique = new ArrayList();
            //    if (sml.Trim() != "0")
            //    {
            //        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //        {
            //            ds = da.select_method_wo_parameter(strquery, "Text");
            //            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //            {
            //                collname = ds.Tables[0].Rows[0]["collname"].ToString();
            //                affilitied = ds.Tables[0].Rows[0]["affliatedby"].ToString();
            //                district = ds.Tables[0].Rows[0]["district"].ToString();
            //                pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
            //                string sessiond1 = string.Empty;
            //                if (ddlSession.SelectedItem.Text == "All")
            //                {
            //                    sessiond1 = string.Empty;
            //                }
            //                else
            //                {
            //                    sessiond1 = "  and es.ses_sion='" + ddlSession.SelectedItem.Text + "'";
            //                }
            //                string[] aff = affilitied.Split(',');
            //                affilitied = aff[0].ToString();
            //                string datess = ddlDate.SelectedItem.Text;
            //                string[] fromdatespit99 = datess.ToString().Split('-');
            //                datess = fromdatespit99[2] + '-' + fromdatespit99[1] + '-' + fromdatespit99[0];
            //                //string overall = "select distinct  top 40 es.roomno,COUNT(1) as strength,es.ses_sion,es.edate  from registration r,subjectchooser sc,exam_seating as es where sc.roll_no=r.roll_no  and exam_flag<>'Debar' and es.regno=r.Reg_No and es.subject_no=sc.subject_no " + sessiond1 + "  group by es.roomno,es.ses_sion,es.edate  ";
            //                string overall = "select distinct   es.roomno,COUNT(1) as strength,es.ses_sion,es.edate  from registration r,subjectchooser sc,exam_seating as es where sc.roll_no=r.roll_no  and exam_flag<>'Debar' and es.regno=r.Reg_No and es.subject_no=sc.subject_no and es.edate='" + datess + "' " + sessiond1 + "  group by es.roomno,es.ses_sion,es.edate  ";
            //                //string overall = "select distinct es.roomno ,c.Course_Name,es.edate,s.subject_no,de.Dept_Name,d.Degree_Code,s.subject_name,s.subject_code,d.Acronym,es.edate,es.ses_sion from exmtt e,exmtt_det et,exam_seating es,course c,Degree d,Department de,subject s where e.exam_code=et.exam_code and et.subject_no=es.subject_no and  e.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id and   d.Dept_Code=de.Dept_Code and es.subject_no=s.subject_no    and et.subject_no=s.subject_no and e.Exam_year='" + ddlExamYear.SelectedItem.Text + "'   and e.Exam_month='" + ddlExamMonth.SelectedValue + "' and es.edate='" + datess + "' and es.ses_sion='" + ddlsession.SelectedItem.Text + "'";
            //                DataSet dsoverall = new DataSet();
            //                dsoverall = da.select_method_wo_parameter(overall, "text");
            //                int u = 0;
            //                int startrow = 0;
            //                int tablerowscount = 0;
            //                for (int sew = 0; sew < FpPhasing.Sheets[0].Rows.Count; sew++)
            //                {
            //                    isval = Convert.ToInt16(FpPhasing.Sheets[0].Cells[u, 1].Value);
            //                    u = u + 1;
            //                    if (isval == 1)
            //                    {
            //                        int we = 1;
            //                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
            //                        {
            //                            PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
            //                            mypdfpage.Add(LogoImage, 35, 25, 700);
            //                        }
            //                        if (dsoverall.Tables[0].Rows.Count > 0)
            //                        {
            //                            coltop = 10;
            //                            PdfTextArea ptc = new PdfTextArea(head, System.Drawing.Color.Black,
            //                                                                            new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
            //                            mypdfpage.Add(ptc);
            //                            coltop = coltop + 15;
            //                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
            //                                                                    new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, affilitied);
            //                            mypdfpage.Add(ptc);
            //                            coltop = coltop + 15;
            //                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
            //                                                                    new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, district + "-" + pincode);
            //                            mypdfpage.Add(ptc);
            //                            coltop = coltop + 15;
            //                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
            //                                                                    new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "ATTENDANCE FOR THE END OF SEMESTER EXAMINATIONS" + "-" + ddlExamMonth.SelectedItem.Text + " " + ddlExamYear.SelectedItem.Text + "");
            //                            mypdfpage.Add(ptc);
            //                            coltop = coltop + 10;
            //                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
            //                                                                    new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "______________________________________________________________________________________________________");
            //                            mypdfpage.Add(ptc);
            //                            string roomnoco = dsoverall.Tables[0].Rows[sew]["roomno"].ToString();
            //                            string queryreg = string.Empty;
            //                            queryreg = "select distinct  sub.subject_no,r.Reg_No,r.Current_Semester,r.Stud_Name,r.Stud_Type,es.seat_no,es.roomno,(c.Course_Name +'-'+ d.Acronym) as Grade,(sub.subject_code +'-'+ sub.subject_name) as subjectname,sc.semester,sub.subject_code,sub.subject_name ,c.Course_Name, (select dept_name from Department where d.dept_code=Dept_Code) as deptname,r.degree_code  from registration r,subjectchooser sc,exam_seating as es ,Degree d,course c,Department de,subject sub where sc.subject_no=sub.subject_no and sc.roll_no=r.roll_no  and delflag=0 and exam_flag<>'Debar' and es.regno=r.Reg_No and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and d.Degree_Code=es.degree_code and r.degree_code=d.Degree_Code and es.subject_no=sc.subject_no and es.roomno='" + roomnoco + "' and es.edate='" + datess + "'  and es.ses_sion='" + ddlSession.SelectedItem.Text + "'  order by es.seat_no";
            //                            //  queryreg = "select distinct  top 102 r.Reg_No,r.Current_Semester,r.Stud_Name,r.Stud_Type,es.seat_no,es.roomno,(c.Course_Name +'-'+ d.Acronym) as Grade,(sub.subject_code +'-'+ sub.subject_name) as subjectname,sub.subject_code,sub.subject_name   from registration r,subjectchooser sc,exam_seating as es ,Degree d,course c,Department de,subject sub where sc.subject_no=sub.subject_no and sc.roll_no=r.roll_no  and delflag=0 and exam_flag<>'Debar' and es.regno=r.Reg_No and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and d.Degree_Code=es.degree_code and r.degree_code=d.Degree_Code and es.subject_no=sc.subject_no and es.roomno='" + roomnoco + "'   and es.ses_sion='" + ddlsession.SelectedItem.Text + "'  order by es.seat_no";
            //                            //queryreg = "select distinct  r.Reg_No,r.Current_Semester,r.Stud_Name,r.Stud_Type,es.seat_no,es.roomno,(c.Course_Name +'-'+ d.Acronym) as Grade,(sub.subject_code +'-'+ sub.subject_name) as subjectname,sub.subject_code,sub.subject_name   from registration r,subjectchooser sc,exam_seating as es ,Degree d,course c,Department de,subject sub where sc.subject_no=sub.subject_no and sc.roll_no=r.roll_no  and delflag=0 and exam_flag<>'Debar' and es.regno=r.Reg_No and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and d.Degree_Code=es.degree_code and r.degree_code=d.Degree_Code and es.subject_no=sc.subject_no and es.roomno='" + roomnoco + "' and es.edate='" + datess + "' and es.ses_sion='" + ddlsession.SelectedItem.Text + "'  order by es.seat_no";
            //                            DataSet dschecksubjlist = new DataSet();
            //                            dschecksubjlist = da.select_method_wo_parameter(queryreg, "text");
            //                            DataSet dscheck = new DataSet();
            //                            //dscheck = d2.select_method_wo_parameter(queryreg, "text");
            //                            for (int subjlist = 0; subjlist < dschecksubjlist.Tables[0].Rows.Count; subjlist++)
            //                            {
            //                                if (!arr_subjectunique.Contains(dschecksubjlist.Tables[0].Rows[subjlist]["subject_no"].ToString().Trim().ToLower() + dschecksubjlist.Tables[0].Rows[subjlist]["degree_code"].ToString().Trim().ToLower()))
            //                                {
            //                                    DataView DVsubjlist = new DataView();
            //                                    dschecksubjlist.Tables[0].DefaultView.RowFilter = " subject_no='" + dschecksubjlist.Tables[0].Rows[subjlist]["subject_no"].ToString() + "'and degree_code='" + dschecksubjlist.Tables[0].Rows[subjlist]["degree_code"].ToString() + "'";
            //                                    DVsubjlist = dschecksubjlist.Tables[0].DefaultView;
            //                                    dscheck.Clear();
            //                                    dscheck.Tables.Clear();
            //                                    dscheck.Tables.Add(DVsubjlist.ToTable());
            //                                    arr_subjectunique.Add(dschecksubjlist.Tables[0].Rows[subjlist]["subject_no"].ToString().Trim().ToLower() + dschecksubjlist.Tables[0].Rows[subjlist]["degree_code"].ToString().Trim().ToLower());
            //                                    string deptname = dscheck.Tables[0].Rows[0]["Course_Name"].ToString() + " - " + dscheck.Tables[0].Rows[0]["deptname"].ToString();
            //                                    string sub_code = dscheck.Tables[0].Rows[0]["subject_code"].ToString();
            //                                    string semester = dscheck.Tables[0].Rows[0]["semester"].ToString();
            //                                    string subname = dscheck.Tables[0].Rows[0]["subject_name"].ToString();
            //                                    we = we + 1;
            //                                    coltop = coltop + 35;
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                          new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Degree & Branch");
            //                                    mypdfpage.Add(ptc);
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                          new PdfArea(mydocument, 130, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
            //                                    mypdfpage.Add(ptc);
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                          new PdfArea(mydocument, 140, coltop, 180, 100), System.Drawing.ContentAlignment.TopLeft, deptname);
            //                                    mypdfpage.Add(ptc);
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                          new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Semester");
            //                                    mypdfpage.Add(ptc);
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                          new PdfArea(mydocument, 463, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
            //                                    mypdfpage.Add(ptc);
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                          new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, semester);
            //                                    mypdfpage.Add(ptc);
            //                                    coltop = coltop + 25;
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                          new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Code");
            //                                    mypdfpage.Add(ptc);
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                          new PdfArea(mydocument, 130, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
            //                                    mypdfpage.Add(ptc);
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                          new PdfArea(mydocument, 140, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, sub_code);
            //                                    mypdfpage.Add(ptc);
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                          new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Date of Exam /Session");
            //                                    mypdfpage.Add(ptc);
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                          new PdfArea(mydocument, 463, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
            //                                    mypdfpage.Add(ptc);
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                          new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ddlDate.SelectedItem.Text + "/" + ddlSession.SelectedItem.Text);
            //                                    mypdfpage.Add(ptc);
            //                                    coltop = coltop + 25;
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                          new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Name");
            //                                    mypdfpage.Add(ptc);
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                          new PdfArea(mydocument, 130, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
            //                                    mypdfpage.Add(ptc);
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                          new PdfArea(mydocument, 140, coltop, 180, 100), System.Drawing.ContentAlignment.TopLeft, subname);
            //                                    mypdfpage.Add(ptc);
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                          new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Room No");
            //                                    mypdfpage.Add(ptc);
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                          new PdfArea(mydocument, 463, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
            //                                    mypdfpage.Add(ptc);
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                          new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, dsoverall.Tables[0].Rows[sew]["roomno"].ToString());
            //                                    mypdfpage.Add(ptc);
            //                                    int tblrocc = 0;
            //                                    sml = "25";
            //                                    if (dscheck.Tables[0].Rows.Count < Convert.ToInt32(sml))
            //                                    {
            //                                        tblrocc = dscheck.Tables[0].Rows.Count;
            //                                    }
            //                                    else
            //                                    {
            //                                        tblrocc = Convert.ToInt32(sml);
            //                                    }
            //                                    Gios.Pdf.PdfTable table1 = mydocument.NewTable(Fontbold, tblrocc + 1, 5, 4);
            //                                    table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
            //                                    table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
            //                                    table1.Cell(0, 0).SetContent("S.No");
            //                                    table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
            //                                    table1.Cell(0, 0).SetFont(Fontbold);
            //                                    table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
            //                                    table1.Cell(0, 1).SetContent("Register Number");
            //                                    table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
            //                                    table1.Cell(0, 1).SetFont(Fontbold);
            //                                    table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
            //                                    table1.Cell(0, 2).SetContent("Name of the Candidate");
            //                                    table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
            //                                    table1.Cell(0, 2).SetFont(Fontbold);
            //                                    table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
            //                                    table1.Cell(0, 3).SetContent("Answer Booklet No");
            //                                    table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
            //                                    table1.Cell(0, 3).SetFont(Fontbold);
            //                                    table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
            //                                    table1.Cell(0, 4).SetContent("Signature of Candidate");
            //                                    table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
            //                                    table1.Cell(0, 4).SetFont(Fontbold);
            //                                    table1.VisibleHeaders = false;
            //                                    table1.Columns[0].SetWidth(20);
            //                                    table1.Columns[1].SetWidth(40);
            //                                    table1.Columns[2].SetWidth(80);
            //                                    table1.Columns[3].SetWidth(50);
            //                                    table1.Columns[4].SetWidth(60);
            //                                    int gwe = 1;
            //                                    int ast = 0;
            //                                    tablerowscount = dscheck.Tables[0].Rows.Count;
            //                                    for (ast = startrow; ast < dscheck.Tables[0].Rows.Count; ast++)
            //                                    {
            //                                        if (ast != 0 && ast % Convert.ToInt32(sml) == 0)
            //                                        {
            //                                            Gios.Pdf.PdfTablePage newpdftabpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, yq, 550, 650));
            //                                            mypdfpage.Add(newpdftabpage1);
            //                                            tablerowscount = tablerowscount - 25;
            //                                            coltop = 680;
            //                                            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
            //                                                                                new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Certified that the following particulars have been verified");
            //                                            mypdfpage.Add(ptc);
            //                                            coltop = coltop + 30;
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                            new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "1.The Register No. in the attendance sheet with that in the hall ticket.");
            //                                            mypdfpage.Add(ptc);
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                            new PdfArea(mydocument, 395, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Total Present");
            //                                            mypdfpage.Add(ptc);
            //                                            coltop = coltop + 12;
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                            new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "2.The identification of the candidate with the photo pasted in the hall ticket");
            //                                            mypdfpage.Add(ptc);
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                         new PdfArea(mydocument, 395, 735, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Total Absent");
            //                                            mypdfpage.Add(ptc);
            //                                            coltop = coltop + 12;
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                            new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "3.The answer book number entered in the attendance sheet by the candidate");
            //                                            mypdfpage.Add(ptc);
            //                                            coltop = coltop + 75;
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                            new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Invigilator");
            //                                            mypdfpage.Add(ptc);
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                            new PdfArea(mydocument, 245, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Chief Invigilator");
            //                                            mypdfpage.Add(ptc);
            //                                            PdfArea pa8 = new PdfArea(mydocument, 20, 700, 360, 125);
            //                                            PdfRectangle pr8 = new PdfRectangle(mydocument, pa8, Color.Black);
            //                                            mypdfpage.Add(pr8);
            //                                            PdfArea pa9 = new PdfArea(mydocument, 470, 700, 60, 25);
            //                                            PdfRectangle pr9 = new PdfRectangle(mydocument, pa9, Color.Black);
            //                                            mypdfpage.Add(pr9);
            //                                            PdfArea pa6 = new PdfArea(mydocument, 470, 725, 60, 25);
            //                                            PdfRectangle pr6 = new PdfRectangle(mydocument, pa6, Color.Black);
            //                                            mypdfpage.Add(pr6);
            //                                            PdfArea tete = new PdfArea(mydocument, 15, 10, 565, 825);
            //                                            PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);
            //                                            mypdfpage.Add(pr1);
            //                                            mypdfpage.SaveToDocument();
            //                                            mypdfpage = mydocument.NewPage();
            //                                            coltop = 10;
            //                                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
            //                                            {
            //                                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
            //                                                mypdfpage.Add(LogoImage, 35, 25, 700);
            //                                            }
            //                                            ptc = new PdfTextArea(head, System.Drawing.Color.Black,
            //                                                                                           new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
            //                                            mypdfpage.Add(ptc);
            //                                            coltop = coltop + 15;
            //                                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
            //                                                                                    new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, affilitied);
            //                                            mypdfpage.Add(ptc);
            //                                            coltop = coltop + 15;
            //                                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
            //                                                                                    new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, district + "-" + pincode);
            //                                            mypdfpage.Add(ptc);
            //                                            coltop = coltop + 15;
            //                                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
            //                                                                                    new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "ATTENDANCE FOR THE END OF SEMESTER EXAMINATIONS" + "-" + ddlExamMonth.SelectedItem.Text + " " + ddlExamYear.SelectedItem.Text + "");
            //                                            mypdfpage.Add(ptc);
            //                                            coltop = coltop + 10;
            //                                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
            //                                                                                    new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "______________________________________________________________________________________________________");
            //                                            mypdfpage.Add(ptc);
            //                                            we = we + 1;
            //                                            coltop = coltop + 35;
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                         new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Degree & Branch");
            //                                            mypdfpage.Add(ptc);
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                                  new PdfArea(mydocument, 130, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
            //                                            mypdfpage.Add(ptc);
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                                  new PdfArea(mydocument, 140, coltop, 180, 100), System.Drawing.ContentAlignment.TopLeft, deptname);
            //                                            mypdfpage.Add(ptc);
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                                  new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Semester");
            //                                            mypdfpage.Add(ptc);
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                                  new PdfArea(mydocument, 463, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
            //                                            mypdfpage.Add(ptc);
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                                  new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, semester);
            //                                            mypdfpage.Add(ptc);
            //                                            coltop = coltop + 25;
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                                  new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Code");
            //                                            mypdfpage.Add(ptc);
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                                  new PdfArea(mydocument, 130, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
            //                                            mypdfpage.Add(ptc);
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                                  new PdfArea(mydocument, 140, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, sub_code);
            //                                            mypdfpage.Add(ptc);
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                                  new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Date of Exam /Session");
            //                                            mypdfpage.Add(ptc);
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                                  new PdfArea(mydocument, 463, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
            //                                            mypdfpage.Add(ptc);
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                                  new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ddlDate.SelectedItem.Text + "/" + ddlSession.SelectedItem.Text);
            //                                            mypdfpage.Add(ptc);
            //                                            coltop = coltop + 25;
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                                  new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Name");
            //                                            mypdfpage.Add(ptc);
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                                  new PdfArea(mydocument, 130, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
            //                                            mypdfpage.Add(ptc);
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                                  new PdfArea(mydocument, 140, coltop, 180, 100), System.Drawing.ContentAlignment.TopLeft, subname);
            //                                            mypdfpage.Add(ptc);
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                                  new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Room No");
            //                                            mypdfpage.Add(ptc);
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                                  new PdfArea(mydocument, 463, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
            //                                            mypdfpage.Add(ptc);
            //                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                                  new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, dsoverall.Tables[0].Rows[sew]["roomno"].ToString());
            //                                            mypdfpage.Add(ptc);
            //                                            if (tablerowscount > 25)
            //                                            {
            //                                                tblrocc = 25;
            //                                            }
            //                                            else
            //                                            {
            //                                                tblrocc = tablerowscount;
            //                                            }
            //                                            table1 = mydocument.NewTable(Fontbold, tblrocc + 1, 5, 4);
            //                                            table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
            //                                            table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
            //                                            table1.Cell(0, 0).SetContent("S.No");
            //                                            table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
            //                                            table1.Cell(0, 0).SetFont(Fontbold);
            //                                            table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
            //                                            table1.Cell(0, 1).SetContent("Register Number");
            //                                            table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
            //                                            table1.Cell(0, 1).SetFont(Fontbold);
            //                                            table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
            //                                            table1.Cell(0, 2).SetContent("Name of the Candidate");
            //                                            table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
            //                                            table1.Cell(0, 2).SetFont(Fontbold);
            //                                            table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
            //                                            table1.Cell(0, 3).SetContent("Answer Booklet No");
            //                                            table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
            //                                            table1.Cell(0, 3).SetFont(Fontbold);
            //                                            table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
            //                                            table1.Cell(0, 4).SetContent("Signature of Candidate");
            //                                            table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
            //                                            table1.Cell(0, 4).SetFont(Fontbold);
            //                                            table1.VisibleHeaders = false;
            //                                            table1.Columns[0].SetWidth(20);
            //                                            table1.Columns[1].SetWidth(40);
            //                                            table1.Columns[2].SetWidth(80);
            //                                            table1.Columns[3].SetWidth(50);
            //                                            table1.Columns[4].SetWidth(60);
            //                                            gwe = 1;
            //                                        }
            //                                        string regno = dscheck.Tables[0].Rows[ast]["Reg_No"].ToString();
            //                                        string name = dscheck.Tables[0].Rows[ast]["Stud_Name"].ToString();
            //                                        string seat = dscheck.Tables[0].Rows[ast]["seat_no"].ToString();
            //                                        string hallno = dscheck.Tables[0].Rows[ast]["roomno"].ToString();
            //                                        table1.Cell(gwe, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
            //                                        table1.Cell(gwe, 0).SetContent(gwe.ToString());
            //                                        table1.Cell(gwe, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
            //                                        table1.Cell(gwe, 1).SetContent(regno.ToString());
            //                                        table1.Cell(gwe, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
            //                                        table1.Cell(gwe, 2).SetContent(name.ToString());
            //                                        gwe = gwe + 1;
            //                                    }
            //                                    int h = 650;
            //                                    Gios.Pdf.PdfTablePage newpdftabpage11 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, yq, 550, h));
            //                                    mypdfpage.Add(newpdftabpage11);
            //                                    coltop = 680;
            //                                    ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
            //                                                                        new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Certified that the following particulars have been verified");
            //                                    mypdfpage.Add(ptc);
            //                                    coltop = coltop + 30;
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                    new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "1.The Register No. in the attendance sheet with that in the hall ticket.");
            //                                    mypdfpage.Add(ptc);
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                    new PdfArea(mydocument, 395, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Total Present");
            //                                    mypdfpage.Add(ptc);
            //                                    coltop = coltop + 12;
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                    new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "2.The identification of the candidate with the photo pasted in the hall ticket");
            //                                    mypdfpage.Add(ptc);
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                 new PdfArea(mydocument, 395, 735, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Total Absent");
            //                                    mypdfpage.Add(ptc);
            //                                    coltop = coltop + 12;
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                    new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "3.The answer book number entered in the attendance sheet by the candidate");
            //                                    mypdfpage.Add(ptc);
            //                                    coltop = coltop + 75;
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                    new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Invigilator");
            //                                    mypdfpage.Add(ptc);
            //                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
            //                                                                    new PdfArea(mydocument, 245, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Chief Invigilator");
            //                                    mypdfpage.Add(ptc);
            //                                    PdfArea pa81 = new PdfArea(mydocument, 20, 700, 360, 125);
            //                                    PdfRectangle pr81 = new PdfRectangle(mydocument, pa81, Color.Black);
            //                                    mypdfpage.Add(pr81);
            //                                    PdfArea pa91 = new PdfArea(mydocument, 470, 700, 60, 25);
            //                                    PdfRectangle pr91 = new PdfRectangle(mydocument, pa91, Color.Black);
            //                                    mypdfpage.Add(pr91);
            //                                    PdfArea pa61 = new PdfArea(mydocument, 470, 725, 60, 25);
            //                                    PdfRectangle pr61 = new PdfRectangle(mydocument, pa61, Color.Black);
            //                                    mypdfpage.Add(pr61);
            //                                    PdfArea tete1 = new PdfArea(mydocument, 15, 10, 565, 825);
            //                                    PdfRectangle pr11 = new PdfRectangle(mydocument, tete1, Color.Black);
            //                                    mypdfpage.Add(pr11);
            //                                    g = 1;
            //                                    if (h >= 500)
            //                                    {
            //                                        coltop = 10;
            //                                        ptc = new PdfTextArea(head, System.Drawing.Color.Black,
            //                                                                                       new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
            //                                        mypdfpage.Add(ptc);
            //                                        coltop = coltop + 15;
            //                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
            //                                                                                new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, affilitied);
            //                                        mypdfpage.Add(ptc);
            //                                        coltop = coltop + 15;
            //                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
            //                                                                                new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, district + "-" + pincode);
            //                                        mypdfpage.Add(ptc);
            //                                        coltop = coltop + 15;
            //                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
            //                                                                                new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "ATTENDANCE FOR THE END OF SEMESTER EXAMINATIONS" + "-" + ddlExamMonth.SelectedItem.Text + " " + ddlExamYear.SelectedItem.Text + "");
            //                                        mypdfpage.Add(ptc);
            //                                        coltop = coltop + 10;
            //                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
            //                                                                                new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "______________________________________________________________________________________________________");
            //                                        mypdfpage.Add(ptc);
            //                                        mypdfpage.SaveToDocument();
            //                                        mypdfpage = mydocument.NewPage();
            //                                        //yq = 190;
            //                                    }
            //                                }
            //                            }
            //                            string appPath = HttpContext.Current.Server.MapPath("~");
            //                            if (appPath != "")
            //                            {
            //                                string szPath = appPath + "/Report/";
            //                                string szFile = "ExamAttendanceSheet" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            //                                mydocument.SaveToFile(szPath + szFile);
            //                                Response.ClearHeaders();
            //                                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
            //                                Response.ContentType = "application/pdf";
            //                                Response.WriteFile(szPath + szFile);
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        //lblnorecc.Visible = true;
            //        //lblnorecc.Text = "Please Allot Bundle No And Then Proceed";
            //    }
            //}
            #endregion
        }
        catch (Exception ex)
        {
            //lblnorecc.Text = ex.ToString();
            //lblnorecc.Visible = true;
        }
    }

    protected void FpPhasing_OnUpdateCommand(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(FpPhasing.Sheets[0].Cells[0, 1].Value) == 1)
            {
                for (int i = 0; i < FpPhasing.Sheets[0].RowCount; i++)
                {
                    if (FpPhasing.Sheets[0].Cells[i, 0].Text != string.Empty)
                        FpPhasing.Sheets[0].Cells[i, 1].Value = 1;
                }
            }
            else if (Convert.ToInt32(FpPhasing.Sheets[0].Cells[0, 1].Value) == 0)
            {
                for (int i = 0; i < FpPhasing.Sheets[0].RowCount; i++)
                {
                    if (FpPhasing.Sheets[0].Cells[i, 0].Text != string.Empty)
                        FpPhasing.Sheets[0].Cells[i, 1].Value = 0;
                }
            }
        }
        catch
        {
        }
    }

    #endregion

    #region Added By Malang Raja On Nov 09 2016

    public void printCoverSheet()
    {
        try
        {
            FpPhasing.SaveChanges();
            string Line1 = string.Empty;
            string Line2 = string.Empty;
            string Line3 = string.Empty;
            string Line4 = string.Empty;
            string Line5 = string.Empty;
            string Line6 = string.Empty;
            string Line7 = string.Empty;
            string Line8 = string.Empty;
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
            Gios.Pdf.PdfPage mypdfpage;//= mydocument.NewPage();

            Font Fontbold = new Font("Book Antique", 10, FontStyle.Bold);
            Font Fontnormal = new Font("Book Antique", 10, FontStyle.Regular);
            Font Fonttitle = new Font("Book Antique", 9, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antique", 8, FontStyle.Regular);
            Font Fonthead = new Font("Book Antique", 10, FontStyle.Regular);
            Font head = new Font("Book Antique", 16, FontStyle.Bold);

            System.Drawing.Font Fontbold1 = new System.Drawing.Font("Algerian", 13, FontStyle.Bold);
            System.Drawing.Font font2bold = new System.Drawing.Font("Palatino Linotype", 11, FontStyle.Bold);
            System.Drawing.Font font2small = new System.Drawing.Font("Palatino Linotype", 11, FontStyle.Regular);
            System.Drawing.Font font3bold = new System.Drawing.Font("Palatino Linotype", 9, FontStyle.Bold);
            System.Drawing.Font font3small = new System.Drawing.Font("Palatino Linotype", 9, FontStyle.Regular);
            System.Drawing.Font font4bold = new System.Drawing.Font("Palatino Linotype", 7, FontStyle.Bold);
            System.Drawing.Font font4small = new System.Drawing.Font("Palatino Linotype", 7, FontStyle.Regular);
            System.Drawing.Font font4smallnew = new System.Drawing.Font("Palatino Linotype", 7, FontStyle.Bold);

            bool selected = false;
            qryCollege = string.Empty;
            collegeCode = string.Empty;
            if (cblCollege.Items.Count > 0)
            {
                collegeCode = getCblSelectedValue(cblCollege);
            }
            if (!string.IsNullOrEmpty(collegeCode.Trim()))
            {
                qryCollege = " college_code in (" + collegeCode + ")";
            }
            else
            {
                FSNominee.Visible = false;
                btngen.Visible = false;
                lblnorec.Visible = true;
                lblnorec.Text = "Please Select Any College";
                return;
            }
            if (FpPhasing.Sheets[0].RowCount > 0)
            {
                for (int row = 0; row < FpPhasing.Sheets[0].RowCount; row++)
                {
                    int sel = 0;
                    int.TryParse(Convert.ToString(FpPhasing.Sheets[0].Cells[row, 1].Value).Trim(), out sel);
                    if (sel == 1)
                    {
                        selected = true;
                    }
                }
            }
            DataSet dsColInfo = da.select_method_wo_parameter("select college_code,UPPER(collname)+' ('+UPPER(Category)+')' as Line1,UPPER(district)+' - '+pincode as distr,affliatedby from collinfo", "Text");
            if (dsColInfo.Tables.Count > 0 && dsColInfo.Tables[0].Rows.Count > 0)
            {
                //Line1 = Convert.ToString(dsColInfo.Tables[0].Rows[0]["Line1"]).Trim();
                //try
                //{
                //    string[] affli = Convert.ToString(dsColInfo.Tables[0].Rows[0]["affliatedby"]).Trim().Split('\\');
                //    Line2 = affli[0].Split(',')[0];
                //    Line4 = "(" + affli[2].Split(',')[0] + ")";
                //    Line3 = affli[1].Split(',')[0];
                //}
                //catch { }
                //Line5 = Convert.ToString(dsColInfo.Tables[0].Rows[0]["distr"]).Trim();
            }
            Line6 = "COVER SHEET";
            Line7 = "SEMESTER EXAMINATION - " + Convert.ToString(ddlExamMonth.SelectedItem.Text.Trim()).ToUpper() + " " + Convert.ToString(ddlExamYear.SelectedItem.Text).Trim();
            Line8 = "COURSE - ";
            string subjectCode = string.Empty;
            string major = string.Empty;
            string subjectName = string.Empty;
            string examDate = string.Empty;
            string examSession = string.Empty;
            //string studName = "STUDENT NAME : " + lblsname.Text.Trim().ToUpper();
            //string rollNumber = "ROLL NO : " + rollno.ToUpper();
            //string regNumber = "REG.NO : " + regNo.ToUpper();
            int posY = 0;
            bool status = false;
            if (selected)
            {
                for (int row = 1; row < FpPhasing.Sheets[0].RowCount; row++)
                {
                    int sel = 0;
                    int.TryParse(Convert.ToString(FpPhasing.Sheets[0].Cells[row, 1].Value).Trim(), out sel);
                    string rowno = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 0].Text).Trim();
                    if (sel == 1)
                    {
                        int PageNo = 1;
                        int ToatlPage = 1;
                        status = true;
                        bool pageHas = false;
                        posY = 10;
                        string allRegNo = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 3].Tag).Trim();
                        string subName = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 2].Note).Trim();
                        string subCode = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 2].Tag).Trim();
                        string examDateNew = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 0].Tag).Trim();
                        string examSessionNew = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 0].Note).Trim();
                        string[] RegNo = allRegNo.Split(',');
                        string collcode = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 3].Note).Trim();
                        if (dsColInfo.Tables.Count > 0 && dsColInfo.Tables[0].Rows.Count > 0)
                        {
                            DataView dvColege = new DataView();
                            dsColInfo.Tables[0].DefaultView.RowFilter = "college_code='" + collcode + "'";
                            dvColege = dsColInfo.Tables[0].DefaultView;
                            if (dvColege.Count > 0)
                            {
                                Line1 = Convert.ToString(dvColege[0]["Line1"]).Trim();
                                try
                                {
                                    string[] affli = Convert.ToString(dvColege[0]["affliatedby"]).Trim().Split('\\');
                                    Line2 = affli[0].Split(',')[0];
                                    Line4 = "(" + affli[2].Split(',')[0] + ")";
                                    Line3 = affli[1].Split(',')[0];
                                }
                                catch { }
                                Line5 = Convert.ToString(dvColege[0]["distr"]).Trim();
                                if (RegNo.Length > 0)
                                {
                                    if (RegNo.Length % 50 == 0)
                                    {
                                        ToatlPage = RegNo.Length / 50;
                                    }
                                    else
                                    {
                                        ToatlPage = (RegNo.Length / 50) + 1;
                                    }
                                    pageHas = true;
                                    mypdfpage = mydocument.NewPage();
                                    PdfTable table2;
                                    Line6 = "COVER SHEET";
                                    Line7 = "SEMESTER EXAMINATION - " + Convert.ToString(ddlExamMonth.SelectedItem.Text.Trim()).ToUpper() + " " + Convert.ToString(ddlExamYear.SelectedItem.Text).Trim();
                                    subjectName = "SUBJECT TITLE\t\t:\t\t " + subName;
                                    subjectCode = "CODE\t\t:\t\t" + subCode;
                                    Gios.Pdf.PdfImage LogoImage;
                                    PdfTablePage tblPage;
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg")))
                                    {
                                        LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg"));
                                        mypdfpage.Add(LogoImage, posY, 10, 500);
                                    }
                                    PdfTextArea pdfSince = new PdfTextArea(font3small, System.Drawing.Color.Black, new PdfArea(mydocument, 15, 60, 100, 30), System.Drawing.ContentAlignment.MiddleLeft, "Since 1951");
                                    mypdfpage.Add(pdfSince);

                                    PdfTextArea pdfLine1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line1);
                                    mypdfpage.Add(pdfLine1);
                                    int rightY = posY;
                                    int neee = Convert.ToInt16((mydocument.PageWidth / 2) + 90);

                                    Gios.Pdf.PdfTable paftblPageNo = mydocument.NewTable(Fontbold, 2, 1, 5);
                                    paftblPageNo.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                    paftblPageNo.VisibleHeaders = false;
                                    paftblPageNo.Columns[0].SetWidth(50);

                                    paftblPageNo.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    paftblPageNo.Cell(0, 0).SetContent(PageNo);
                                    paftblPageNo.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    paftblPageNo.Cell(1, 0).SetContent(ToatlPage);

                                    tblPage = paftblPageNo.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, (mydocument.PageWidth - 100), rightY, 50, 80));
                                    mypdfpage.Add(tblPage);

                                    PdfTextArea pdfSubCode = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, (mydocument.PageWidth / 2) + 100, rightY, (mydocument.PageWidth / 2) - 100, 50), System.Drawing.ContentAlignment.MiddleLeft, subjectCode);
                                    mypdfpage.Add(pdfSubCode);

                                    posY += 20;
                                    PdfTextArea pdfLine2 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line2);
                                    mypdfpage.Add(pdfLine2);

                                    rightY += 30;
                                    PdfTextArea pdfMajor = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, (mydocument.PageWidth / 2) + 100, rightY, (mydocument.PageWidth / 2) - 100, 50), System.Drawing.ContentAlignment.MiddleLeft, "MAJOR : ");
                                    mypdfpage.Add(pdfMajor);

                                    posY += 15;
                                    PdfTextArea pdfLine3 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line3);
                                    mypdfpage.Add(pdfLine3);

                                    rightY += 30;
                                    PdfTextArea pdfDateSession = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, (mydocument.PageWidth / 2) + 100, rightY, (mydocument.PageWidth / 2) - 100, 50), System.Drawing.ContentAlignment.MiddleLeft, "DATE & DURATION\t\t:\t\t" + examDateNew + "-" + examSessionNew);
                                    mypdfpage.Add(pdfDateSession);

                                    posY += 15;
                                    PdfTextArea pdfLine4 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line4);
                                    mypdfpage.Add(pdfLine4);

                                    rightY += 30;
                                    PdfTextArea pdfNoofBooks = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, (mydocument.PageWidth / 2) + 100, rightY, (mydocument.PageWidth / 2) - 100, 50), System.Drawing.ContentAlignment.MiddleLeft, "TOT. NO. OF ANS. BOOKS IN PACK.");
                                    mypdfpage.Add(pdfNoofBooks);

                                    posY += 15;
                                    PdfTextArea pdfLine5 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line5);
                                    mypdfpage.Add(pdfLine5);

                                    posY += 15;
                                    PdfTextArea pdfLine6 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line6);
                                    mypdfpage.Add(pdfLine6);

                                    posY += 15;
                                    PdfTextArea pdfLine7 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line7);
                                    mypdfpage.Add(pdfLine7);

                                    posY += 30;
                                    PdfLine pdfVertcalLine = new PdfLine(mydocument, new Point(neee, 10), new Point(neee, posY - 5), Color.Black, 1);
                                    mypdfpage.Add(pdfVertcalLine);
                                    neee = Convert.ToInt16(mydocument.PageWidth - 15);
                                    PdfLine pdfHeaderLine = new PdfLine(mydocument, new Point(15, posY), new Point(neee, posY), Color.Black, 1);
                                    mypdfpage.Add(pdfHeaderLine);

                                    posY += 8;
                                    PdfTextArea pdfSubjectName = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 15, posY, (mydocument.PageWidth / 2) + 120, 20), System.Drawing.ContentAlignment.MiddleLeft, subjectName);
                                    mypdfpage.Add(pdfSubjectName);

                                    PdfTextArea pdfFooterText;
                                    Gios.Pdf.PdfTable table1 = mydocument.NewTable(Fontbold, 11, 10, 11);
                                    table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                    table1.VisibleHeaders = false;
                                    table1.Columns[0].SetWidth(70);
                                    table1.Columns[1].SetWidth(40);
                                    table1.Columns[2].SetWidth(70);
                                    table1.Columns[3].SetWidth(40);
                                    table1.Columns[4].SetWidth(70);
                                    table1.Columns[5].SetWidth(40);
                                    table1.Columns[6].SetWidth(70);
                                    table1.Columns[7].SetWidth(40);
                                    table1.Columns[8].SetWidth(70);
                                    table1.Columns[9].SetWidth(40);

                                    table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(0, 0).SetContent("REG.No.");
                                    table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(0, 1).SetContent("P/A");
                                    table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(0, 2).SetContent("REG.No.");
                                    table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(0, 3).SetContent("P/A");
                                    table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(0, 4).SetContent("REG.No.");
                                    table1.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(0, 5).SetContent("P/A");
                                    table1.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(0, 6).SetContent("REG.No.");
                                    table1.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(0, 7).SetContent("P/A");
                                    table1.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(0, 8).SetContent("REG.No.");
                                    table1.Cell(0, 9).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(0, 9).SetContent("P/A");
                                    table1.Rows[0].SetCellPadding(10);

                                    int rOw = 0;
                                    bool newPage = false;
                                    int tempRow = 0;
                                    for (int roow = rOw; roow < RegNo.Length; roow++)
                                    {
                                        if (rOw % 50 == 0 && rOw != 0 && (RegNo.Length > rOw))
                                        {
                                            posY += 20;
                                            PageNo++;
                                            tblPage = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, posY, mydocument.PageWidth - 100, 500));
                                            mypdfpage.Add(tblPage);

                                            posY += Convert.ToInt16(tblPage.Area.Height) + 15;
                                            pdfFooterText = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 50, posY, (mydocument.PageWidth / 2) + 120, 20), System.Drawing.ContentAlignment.MiddleLeft, "This Packet is indented to hold 50 Answer Books Only.\t\t|\t\tPresence or Absence of Candidates to be marked in small box provided P/A");
                                            mypdfpage.Add(pdfFooterText);

                                            table2 = mydocument.NewTable(Fontbold, 3, 2, 5);
                                            table2.SetBorders(Color.Black, 1, BorderType.ColumnsAndBounds);

                                            table2.VisibleHeaders = false;
                                            table2.Columns[0].SetWidth(150);
                                            table2.Columns[1].SetWidth(280);

                                            table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table2.Cell(0, 0).SetContent("Date\t:");
                                            table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table2.Cell(0, 1).SetContent("Name of Examiner(s)\t:");

                                            table2.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table2.Cell(2, 0).SetContent("Signature of the chief Superintendent");
                                            table2.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table2.Cell(2, 1).SetContent("Signature with Date\t:");
                                            posY += 20;
                                            tblPage = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, posY, mydocument.PageWidth - 80, 100));
                                            mypdfpage.Add(tblPage);

                                            mypdfpage.SaveToDocument();
                                            mypdfpage = mydocument.NewPage();
                                            tempRow = 0;
                                            posY = 10;
                                            tempRow = 0;
                                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg")))
                                            {
                                                LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg"));
                                                mypdfpage.Add(LogoImage, posY, 10, 500);
                                            }
                                            pdfSince = new PdfTextArea(font3small, System.Drawing.Color.Black, new PdfArea(mydocument, 15, 60, 100, 30), System.Drawing.ContentAlignment.MiddleLeft, "Since 1951");
                                            mypdfpage.Add(pdfSince);

                                            pdfLine1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line1);
                                            mypdfpage.Add(pdfLine1);

                                            rightY = posY;
                                            neee = Convert.ToInt16((mydocument.PageWidth / 2) + 90);

                                            paftblPageNo = mydocument.NewTable(Fontbold, 2, 1, 5);
                                            paftblPageNo.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                            paftblPageNo.VisibleHeaders = false;
                                            paftblPageNo.Columns[0].SetWidth(50);

                                            paftblPageNo.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            paftblPageNo.Cell(0, 0).SetContent(PageNo.ToString());
                                            paftblPageNo.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            paftblPageNo.Cell(1, 0).SetContent(ToatlPage);

                                            tblPage = paftblPageNo.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, (mydocument.PageWidth - 100), rightY, 50, 80));
                                            mypdfpage.Add(tblPage);

                                            pdfSubCode = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, (mydocument.PageWidth / 2) + 100, rightY, (mydocument.PageWidth / 2) - 100, 50), System.Drawing.ContentAlignment.MiddleLeft, subjectCode);
                                            mypdfpage.Add(pdfSubCode);

                                            posY += 20;
                                            pdfLine2 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line2);
                                            mypdfpage.Add(pdfLine2);

                                            rightY += 30;
                                            pdfMajor = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, (mydocument.PageWidth / 2) + 100, rightY, (mydocument.PageWidth / 2) - 100, 50), System.Drawing.ContentAlignment.MiddleLeft, "MAJOR : ");
                                            mypdfpage.Add(pdfMajor);

                                            posY += 15;
                                            pdfLine3 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line3);
                                            mypdfpage.Add(pdfLine3);
                                            rightY += 30;
                                            pdfDateSession = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, (mydocument.PageWidth / 2) + 100, rightY, (mydocument.PageWidth / 2) - 100, 50), System.Drawing.ContentAlignment.MiddleLeft, "DATE & DURATION\t\t:\t\t" + examDateNew + "-" + examSessionNew);
                                            mypdfpage.Add(pdfDateSession);

                                            posY += 15;
                                            pdfLine4 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line4);
                                            mypdfpage.Add(pdfLine4);
                                            rightY += 30;
                                            pdfNoofBooks = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, (mydocument.PageWidth / 2) + 100, rightY, (mydocument.PageWidth / 2) - 100, 50), System.Drawing.ContentAlignment.MiddleLeft, "TOT. NO. OF ANS. BOOKS IN PACK.");
                                            mypdfpage.Add(pdfNoofBooks);

                                            posY += 15;
                                            pdfLine5 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line5);
                                            mypdfpage.Add(pdfLine5);

                                            posY += 15;
                                            pdfLine6 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line6);
                                            mypdfpage.Add(pdfLine6);

                                            posY += 15;
                                            pdfLine7 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line7);
                                            mypdfpage.Add(pdfLine7);

                                            posY += 30;
                                            pdfVertcalLine = new PdfLine(mydocument, new Point(neee, 10), new Point(neee, posY - 5), Color.Black, 1);
                                            mypdfpage.Add(pdfVertcalLine);
                                            neee = Convert.ToInt16(mydocument.PageWidth - 15);
                                            pdfHeaderLine = new PdfLine(mydocument, new Point(15, posY), new Point(neee, posY), Color.Black, 1);
                                            mypdfpage.Add(pdfHeaderLine);

                                            posY += 8;
                                            pdfSubjectName = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 15, posY, (mydocument.PageWidth / 2) + 120, 20), System.Drawing.ContentAlignment.MiddleLeft, subjectName);
                                            mypdfpage.Add(pdfSubjectName);

                                            table1 = mydocument.NewTable(Fontbold, 11, 10, 11);
                                            table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                            table1.VisibleHeaders = false;
                                            table1.Columns[0].SetWidth(70);
                                            table1.Columns[1].SetWidth(40);
                                            table1.Columns[2].SetWidth(70);
                                            table1.Columns[3].SetWidth(40);
                                            table1.Columns[4].SetWidth(70);
                                            table1.Columns[5].SetWidth(40);
                                            table1.Columns[6].SetWidth(70);
                                            table1.Columns[7].SetWidth(40);
                                            table1.Columns[8].SetWidth(70);
                                            table1.Columns[9].SetWidth(40);

                                            table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 0).SetContent("REG.No.");
                                            table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 1).SetContent("P/A");
                                            table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 2).SetContent("REG.No.");
                                            table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 3).SetContent("P/A");
                                            table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 4).SetContent("REG.No.");
                                            table1.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 5).SetContent("P/A");
                                            table1.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 6).SetContent("REG.No.");
                                            table1.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 7).SetContent("P/A");
                                            table1.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 8).SetContent("REG.No.");
                                            table1.Cell(0, 9).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 9).SetContent("P/A");
                                            table1.Rows[0].SetCellPadding(10);

                                        }
                                        for (int cOl = 0; cOl < 10; cOl += 2)
                                        {
                                            if (RegNo.Length > rOw)
                                            {
                                                table1.Cell(tempRow + 1, cOl).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Rows[tempRow + 1].SetCellPadding(10);
                                                table1.Cell(tempRow + 1, cOl).SetContent(RegNo[rOw].ToString().Trim(new char[] { '\'' }).Replace("'", "").Trim());
                                                table1.Cell(tempRow + 1, cOl).SetFont(Fontnormal);
                                                rOw++;
                                            }
                                            else
                                            {
                                                if (tempRow + 1 < 11)
                                                    table1.Cell(tempRow + 1, cOl).SetContent("\n");
                                            }
                                        }
                                        tempRow++;
                                    }

                                    //for (int cOl = 0; cOl < 10; cOl += 2)
                                    //{
                                    //    for (int rro = 1; rro < 11; rro++)
                                    //    {
                                    //        if (RegNo.Length > rOw)
                                    //        {
                                    //            //string regno = dsdisplay.Tables[0].Rows[rOw]["Reg_No"].ToString();
                                    //            //string name = dsdisplay.Tables[0].Rows[rOw]["Stud_Name"].ToString();
                                    //            //string roomno = dsdisplay.Tables[0].Rows[rOw]["roomno"].ToString();
                                    //            //string seatno = dsdisplay.Tables[0].Rows[rOw]["seat_no"].ToString();

                                    //            table1.Cell(rro, cOl).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    //            table1.Cell(rro, cOl).SetContent(RegNo[rOw].ToString().Trim(new char[] { '\''}).Replace("'", "").Trim());
                                    //            table1.Cell(rro, cOl).SetFont(Fontnormal);
                                    //            rOw++;
                                    //        }
                                    //        else
                                    //        {
                                    //            table1.Cell(rro, cOl).SetContent("\n");
                                    //        }
                                    //    }
                                    //}

                                    posY += 20;
                                    tblPage = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, posY, mydocument.PageWidth - 100, 500));
                                    mypdfpage.Add(tblPage);
                                    posY += Convert.ToInt16(tblPage.Area.Height) + 15;

                                    pdfFooterText = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 50, posY, (mydocument.PageWidth / 2) + 120, 20), System.Drawing.ContentAlignment.MiddleLeft, "This Packet is indented to hold 50 Answer Books Only.\t\t|\t\tPresence or Absence of Candidates to be marked in small box provided P/A");
                                    mypdfpage.Add(pdfFooterText);

                                    table2 = mydocument.NewTable(Fontbold, 3, 2, 5);
                                    table2.SetBorders(Color.Black, 1, BorderType.ColumnsAndBounds);

                                    table2.VisibleHeaders = false;
                                    table2.Columns[0].SetWidth(150);
                                    table2.Columns[1].SetWidth(280);

                                    table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table2.Cell(0, 0).SetContent("Date\t:");
                                    table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table2.Cell(0, 1).SetContent("Name of Examiner(s)\t:");

                                    table2.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                                    table2.Cell(2, 0).SetContent("Signature of the chief Superintendent");
                                    table2.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table2.Cell(2, 1).SetContent("Signature with Date\t:");
                                    posY += 20;
                                    tblPage = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, posY, mydocument.PageWidth - 80, 100));
                                    mypdfpage.Add(tblPage);
                                    mypdfpage.SaveToDocument();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                lblAlert.Text = "Please Select Any One Record";
                imgAlert.Visible = true;
                return;
            }
            if (status)
            {
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "CoverSheet_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                    mydocument.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void printQuestionPaperPacking()
    {
        try
        {
            FpPhasing.SaveChanges();
            string Line1 = string.Empty;
            string Line2 = string.Empty;
            string Line3 = string.Empty;
            string Line4 = string.Empty;
            string Line5 = string.Empty;
            string Line6 = string.Empty;
            string Line7 = string.Empty;
            string Line8 = string.Empty;

            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
            Gios.Pdf.PdfPage mypdfpage;

            Font Fontbold = new Font("Book Antique", 10, FontStyle.Bold);
            Font Fontnormal = new Font("Book Antique", 10, FontStyle.Regular);
            Font Fonttitle = new Font("Book Antique", 9, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antique", 8, FontStyle.Regular);
            Font Fonthead = new Font("Book Antique", 10, FontStyle.Regular);
            Font head = new Font("Book Antique", 16, FontStyle.Bold);

            System.Drawing.Font Fontbold1 = new System.Drawing.Font("Algerian", 13, FontStyle.Bold);
            System.Drawing.Font font2bold = new System.Drawing.Font("Palatino Linotype", 11, FontStyle.Bold);
            System.Drawing.Font font2small = new System.Drawing.Font("Palatino Linotype", 11, FontStyle.Regular);
            System.Drawing.Font font3bold = new System.Drawing.Font("Palatino Linotype", 9, FontStyle.Bold);
            System.Drawing.Font font3small = new System.Drawing.Font("Palatino Linotype", 9, FontStyle.Regular);
            System.Drawing.Font font4bold = new System.Drawing.Font("Palatino Linotype", 7, FontStyle.Bold);
            System.Drawing.Font font4small = new System.Drawing.Font("Palatino Linotype", 7, FontStyle.Regular);
            System.Drawing.Font font4smallnew = new System.Drawing.Font("Palatino Linotype", 7, FontStyle.Bold);

            bool selected = false;
            string subjectCodeAll = string.Empty;
            qryCollege = string.Empty;
            collegeCode = string.Empty;
            ArrayList arrDate = new ArrayList();
            ArrayList arrSession = new ArrayList();
            ArrayList arrCollege = new ArrayList();
            ArrayList arrSubjectCode = new ArrayList();
            ArrayList arrRegNo = new ArrayList();
            if (cblCollege.Items.Count > 0)
            {
                collegeCode = getCblSelectedValue(cblCollege);
            }
            //if (!string.IsNullOrEmpty(collegeCode.Trim()))
            //{
            //    qryCollege = " and r.college_code in (" + collegeCode + ")";
            //}
            //else
            //{
            //    FSNominee.Visible = false;
            //    btngen.Visible = false;
            //    lblnorec.Visible = true;
            //    lblnorec.Text = "Please Select Any College";
            //    return;
            //}
            string qryDate = string.Empty;
            string examdate = string.Empty;
            string[] dsplit;
            string qrySession = string.Empty;
            string strsubjectcode = string.Empty;
            string RegAll = string.Empty;
            string dateNew = string.Empty;
            string sessionNew = string.Empty;
            if (FpPhasing.Sheets[0].RowCount > 0)
            {
                //collegeCode = string.Empty;
                for (int row = 0; row < FpPhasing.Sheets[0].RowCount; row++)
                {
                    int sel = 0;
                    int.TryParse(Convert.ToString(FpPhasing.Sheets[0].Cells[row, 1].Value).Trim(), out sel);
                    if (sel == 1)
                    {
                        selected = true;
                        string allRegNo = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 3].Tag).Trim();
                        string subName = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 2].Note).Trim();
                        string subCode = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 2].Tag).Trim();
                        string examDateNew = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 1].Tag).Trim();
                        string examSessionNew = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 0].Note).Trim();
                        string[] RegNo = allRegNo.Split(',');
                        string collcode = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 3].Note).Trim();
                        if (!arrSubjectCode.Contains(subCode))
                        {
                            if (string.IsNullOrEmpty(subjectCodeAll))
                            {
                                subjectCodeAll = "'" + subCode + "'";
                            }
                            else
                            {
                                subjectCodeAll += ",'" + subCode + "'";
                            }
                            arrSubjectCode.Add(subCode);
                        }
                        for (int reg = 0; reg < RegNo.Length; reg++)
                        {
                            if (!arrRegNo.Contains(RegNo[reg]))
                            {
                                if (string.IsNullOrEmpty(RegAll))
                                {
                                    RegAll = "" + RegNo[reg] + "";
                                }
                                else
                                {
                                    RegAll += "," + RegNo[reg] + "";
                                }
                                arrRegNo.Add(RegNo[reg]);
                            }
                        }
                        if (!arrDate.Contains(examDateNew))
                        {
                            if (string.IsNullOrEmpty(dateNew))
                            {
                                dateNew = "'" + examDateNew + "'";
                            }
                            else
                            {
                                dateNew += ",'" + examDateNew + "'";
                            }
                            arrDate.Add(examDateNew);
                        }
                        if (!arrSession.Contains(examSessionNew))
                        {
                            if (string.IsNullOrEmpty(sessionNew))
                            {
                                sessionNew = "'" + examSessionNew + "'";
                            }
                            else
                            {
                                sessionNew += ",'" + examSessionNew + "'";
                            }
                            arrSession.Add(examSessionNew);
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(collegeCode.Trim()))
            {
                qryCollege = " and r.college_code in (" + collegeCode + ") ";
            }
            string qrRegNo = string.Empty;
            if (!string.IsNullOrEmpty(RegAll.Trim()))
            {
                qrRegNo = " and r.Reg_No in (" + RegAll + ") ";
            }
            if (!string.IsNullOrEmpty(dateNew.Trim()))
            {
                qryDate = " and etd.exam_date in(" + dateNew + ") ";
            }
            if (!string.IsNullOrEmpty(sessionNew.Trim()))
            {
                qrySession = "  and etd.exam_session in (" + sessionNew + ") ";
            }
            if (!string.IsNullOrEmpty(subjectCodeAll.Trim()))
            {
                strsubjectcode = " and s.subject_code in (" + subjectCodeAll + ") ";
            }
            DataSet dsColInfo = da.select_method_wo_parameter("select college_code,UPPER(collname)+' ('+UPPER(Category)+')' as Line1,UPPER(district)+' - '+pincode as distr,affliatedby from collinfo", "Text");
            Line6 = "COVER SHEET";
            Line7 = "SEMESTER EXAMINATION - " + Convert.ToString(ddlExamMonth.SelectedItem.Text.Trim()).ToUpper() + " " + Convert.ToString(ddlExamYear.SelectedItem.Text).Trim();
            Line8 = "COURSE - ";
            string subjectCode = string.Empty;
            string major = string.Empty;
            string subjectName = string.Empty;
            string examDate = string.Empty;
            string examSession = string.Empty;
            string qry = "select distinct c.Edu_Level,c.Course_Name,s.subject_code,s.subject_name,convert(nvarchar(15),etd.exam_date,103) as edate,etd.exam_date,etd.exam_session,CONVERT(varchar, etd.start_time, 108) AS start_time,CONVERT(varchar, etd.end_time, 108) AS end_time,etd.start_time as ST,etd.end_time as ET from Exam_Details et,exmtt_det etd,subject s,Course c,Degree dg,Department dt,exam_application ea,exam_appl_details ed,Registration r where r.degree_code=et.degree_code and dg.Degree_Code=r.degree_code and et.batch_year=r.Batch_Year and r.Roll_No=ea.roll_no and ea.appl_no=ed.appl_no  and ed.subject_no=s.subject_no and etd.subject_no=ed.subject_no and etd.subject_no=s.subject_no and dg.Degree_Code=et.degree_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code  " + strsubjectcode + qryCollege + qryDate + qrRegNo + qrySession + " and et.Exam_year='" + Convert.ToString(ddlExamYear.SelectedItem.Text).Trim() + "' and et.Exam_month='" + Convert.ToString(ddlExamMonth.SelectedValue).Trim() + "'";
            DataSet dsAll = da.select_method_wo_parameter(qry, "Text");
            int posY = 0;
            bool status = false;
            ArrayList arrSubjectsList = new ArrayList();
            if (selected)
            {
                bool notSave = false;
                mypdfpage = mydocument.NewPage();
                for (int row = 1; row < FpPhasing.Sheets[0].RowCount; row++)
                {
                    int sel = 0;
                    int.TryParse(Convert.ToString(FpPhasing.Sheets[0].Cells[row, 1].Value).Trim(), out sel);
                    string rowno = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 0].Text).Trim();
                    if (sel == 1)
                    {
                        status = true;
                        bool pageHas = false;
                        //posY = 10;
                        string allRegNo = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 3].Tag).Trim();
                        string subName = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 2].Note).Trim();
                        string subCode = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 2].Tag).Trim();
                        string examDateNew = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 0].Tag).Trim();
                        string examSessionNew = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 0].Note).Trim();
                        string[] RegNo = allRegNo.Split(',');
                        string collcode = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 3].Note).Trim();
                        DataView dvAll = new DataView();
                        string edulevel = string.Empty;
                        string course = string.Empty;
                        string examStartTime = string.Empty;
                        string examEndTime = string.Empty;
                        if (!arrSubjectsList.Contains(subCode))
                        {
                            if (dsAll.Tables.Count > 0 && dsAll.Tables[0].Rows.Count > 0)
                            {
                                dsAll.Tables[0].DefaultView.RowFilter = "subject_code='" + subCode + "'";
                                dvAll = dsAll.Tables[0].DefaultView;
                            }
                            //if (dsColInfo.Tables.Count > 0 && dsColInfo.Tables[0].Rows.Count > 0)
                            //{
                            //    DataView dvColege = new DataView();
                            //    dsColInfo.Tables[0].DefaultView.RowFilter = "college_code='" + collcode + "'";
                            //    dvColege = dsColInfo.Tables[0].DefaultView;
                            //    if (dvColege.Count > 0)
                            //    {
                            //    }
                            //}
                            PdfTable pdfTbl;
                            PdfTablePage pdfTblPAge;
                            PdfLine pdfLine;
                            if (dvAll.Count > 0)
                            {
                                edulevel = Convert.ToString(dvAll[0]["Edu_Level"]).Trim();
                                course = Convert.ToString(dvAll[0]["Course_Name"]).Trim();
                                DateTime st = new DateTime();
                                DateTime.TryParseExact(Convert.ToString(dvAll[0]["start_time"]).Trim(), "HH:mm:ss", null, DateTimeStyles.None, out st);
                                examStartTime = st.ToString("hh:mm tt");
                                DateTime et = new DateTime();
                                DateTime.TryParseExact(Convert.ToString(dvAll[0]["end_time"]).Trim(), "HH:mm:ss", null, DateTimeStyles.None, out et);
                                examEndTime = et.ToString("hh:mm tt");

                                string deg = " DEGREE \n" + Convert.ToString(ddlExamMonth.SelectedItem.Text).Trim().ToUpper() + "." + Convert.ToString(ddlExamYear.SelectedItem.Text).Trim();
                                string dateVAlue = examDateNew + "\n" + examStartTime + "\nTo\n" + examEndTime;
                                if (dvAll.Count == 1)
                                {
                                    pdfTbl = mydocument.NewTable(Fontbold, 1, 4, 10);
                                    pdfTbl.SetBorders(Color.Black, 1, BorderType.Rows);

                                    pdfTbl.VisibleHeaders = false;
                                    pdfTbl.Columns[0].SetWidth(100);
                                    pdfTbl.Columns[1].SetWidth(100);
                                    pdfTbl.Columns[2].SetWidth(250);
                                    pdfTbl.Columns[3].SetWidth(150);

                                    pdfTbl.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTbl.Cell(0, 0).SetContent(course + deg);
                                    pdfTbl.Cell(0, 0).SetFont(head);

                                    pdfTbl.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTbl.Cell(0, 1).SetContent(subCode);

                                    pdfTbl.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTbl.Cell(0, 2).SetContent(subName);

                                    pdfTbl.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTbl.Cell(0, 3).SetContent(dateVAlue);

                                    //posY += 10;
                                    //pdfTblPAge = pdfTbl.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, posY, mydocument.PageWidth - 50, 100));
                                    //mypdfpage.Add(pdfTblPAge);
                                    //posY += Convert.ToInt16(pdfTblPAge.Area.Height) + 15;
                                    //mypdfpage.SaveToDocument();
                                    if (posY > mydocument.PageHeight - 120)
                                    {
                                        notSave = true;
                                        mypdfpage.SaveToDocument();
                                        mypdfpage = mydocument.NewPage();
                                        posY = 10;
                                        pdfTblPAge = pdfTbl.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, posY, mydocument.PageWidth - 50, 100));
                                        mypdfpage.Add(pdfTblPAge);

                                        //pdfLine = pdfTblPAge.Area.LowerBound(Color.Black, 1);
                                        //mypdfpage.Add(pdfLine);

                                        pdfLine = pdfTblPAge.Area.UpperBound(Color.Black, 1);
                                        mypdfpage.Add(pdfLine);
                                        posY += Convert.ToInt16(pdfTblPAge.Area.Height) + 15;
                                        notSave = false;
                                    }
                                    else
                                    {
                                        posY += 10;
                                        pdfTblPAge = pdfTbl.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, posY, mydocument.PageWidth - 50, 100));
                                        mypdfpage.Add(pdfTblPAge);
                                        //pdfLine = pdfTblPAge.Area.LowerBound(Color.Black, 1);
                                        //mypdfpage.Add(pdfLine);
                                        pdfLine = pdfTblPAge.Area.UpperBound(Color.Black, 1);
                                        mypdfpage.Add(pdfLine);
                                        posY += Convert.ToInt16(pdfTblPAge.Area.Height) + 15;
                                        notSave = false;
                                    }
                                }
                                else if (dvAll.Count > 1)
                                {
                                    pdfTbl = mydocument.NewTable(Fontbold, 1, 4, 10);
                                    pdfTbl.SetBorders(Color.Black, 1, BorderType.Rows);

                                    pdfTbl.VisibleHeaders = false;
                                    pdfTbl.Columns[0].SetWidth(100);
                                    pdfTbl.Columns[1].SetWidth(100);
                                    pdfTbl.Columns[2].SetWidth(250);
                                    pdfTbl.Columns[3].SetWidth(150);

                                    pdfTbl.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTbl.Cell(0, 0).SetContent(edulevel + deg);
                                    pdfTbl.Cell(0, 0).SetFont(head);

                                    pdfTbl.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTbl.Cell(0, 1).SetContent(subCode);

                                    pdfTbl.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTbl.Cell(0, 2).SetContent(subName);

                                    pdfTbl.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTbl.Cell(0, 3).SetContent(dateVAlue);

                                    if (posY > mydocument.PageHeight - 120)
                                    {
                                        notSave = true;
                                        mypdfpage.SaveToDocument();
                                        mypdfpage = mydocument.NewPage();
                                        posY = 10;
                                        pdfTblPAge = pdfTbl.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, posY, mydocument.PageWidth - 50, 100));
                                        mypdfpage.Add(pdfTblPAge);

                                        //pdfLine = pdfTblPAge.Area.LowerBound(Color.Black, 1);
                                        //mypdfpage.Add(pdfLine);

                                        pdfLine = pdfTblPAge.Area.UpperBound(Color.Black, 1);
                                        mypdfpage.Add(pdfLine);

                                        posY += Convert.ToInt16(pdfTblPAge.Area.Height) + 15;
                                        notSave = false;
                                    }
                                    else
                                    {
                                        posY += 10;
                                        pdfTblPAge = pdfTbl.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, posY, mydocument.PageWidth - 50, 100));
                                        mypdfpage.Add(pdfTblPAge);
                                        //pdfLine = pdfTblPAge.Area.LowerBound(Color.Black, 1);
                                        //mypdfpage.Add(pdfLine);
                                        pdfLine = pdfTblPAge.Area.UpperBound(Color.Black, 1);
                                        mypdfpage.Add(pdfLine);

                                        posY += Convert.ToInt16(pdfTblPAge.Area.Height) + 15;
                                        notSave = false;
                                    }
                                }
                                arrSubjectsList.Add(subCode);
                            }
                        }
                    }
                }
                if (!notSave)
                    mypdfpage.SaveToDocument();
            }
            else
            {
                lblAlert.Text = "Please Select Any One Record";
                imgAlert.Visible = true;
                return;
            }
            if (status)
            {
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "QPaperPacking_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                    mydocument.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnPopupClose_Click(object sender, EventArgs e)
    {
        lblAlert.Text = string.Empty;
        imgAlert.Visible = false;
    }

    #endregion Added By Malang Raja On Nov 09 2016

    #region Added By Malang Raja T on May 2017

    protected void chkExamDate_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chkExamDate, cblExamDate, txtExamDate, lblExamDate.Text, "--Select--");
            Printcontrol.Visible = false;
            FSNominee.Visible = false;
            btnprintpdf.Visible = false;
            btngen.Visible = false;
            Bindhallno();
            loadSubjectName();
            btngen.Visible = false;
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void cblExamDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chkExamDate, cblExamDate, txtExamDate, lblExamDate.Text, "--Select--");
            Printcontrol.Visible = false;
            FSNominee.Visible = false;
            btnprintpdf.Visible = false;
            btngen.Visible = false;
            Bindhallno();
            loadSubjectName();
            btngen.Visible = false;
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlExamDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            FSNominee.Visible = false;
            btnprintpdf.Visible = false;
            btngen.Visible = false;
            Bindhallno();
            loadSubjectName();
            btngen.Visible = false;
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkExamSession_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chkExamSession, cblExamSession, txtExamSession, lblExamSession.Text, "--Select--");
            Printcontrol.Visible = false;
            FSNominee.Visible = false;
            btnprintpdf.Visible = false;
            btngen.Visible = false;
            Bindhallno();
            loadSubjectName();
            btngen.Visible = false;
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void cblExamSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chkExamSession, cblExamSession, txtExamSession, lblExamSession.Text, "--Select--");
            Printcontrol.Visible = false;
            FSNominee.Visible = false;
            btnprintpdf.Visible = false;
            btngen.Visible = false;
            Bindhallno();
            loadSubjectName();
            btngen.Visible = false;
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlExamSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            FSNominee.Visible = false;
            btnprintpdf.Visible = false;
            btngen.Visible = false;
            Bindhallno();
            loadSubjectName();
            btngen.Visible = false;
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion

}