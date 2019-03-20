using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class ExamICAOnlyApplication : System.Web.UI.Page
{
    string CollegeCode;
    Boolean yes_flag = false;
    DataView dv1 = new DataView();
    DataView dv2 = new DataView();
    DataView dv3 = new DataView();
    DAccess2 da = new DAccess2();
    DataSet ds2 = new DataSet();
    DataSet dsss = new DataSet();
    Hashtable hat = new Hashtable();

    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

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
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            CollegeCode = Session["collegecode"].ToString();
            lblerr1.Visible = false;
            if (!IsPostBack)
            {
                year1();
                loadtype();
                clear();
            }
        }
        catch (Exception ex)
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

    public void year1()
    {
        dsss.Clear();
        dsss = da.Examyear();
        if (dsss.Tables[0].Rows.Count > 0)
        {
            ddlYear1.DataSource = dsss;
            ddlYear1.DataTextField = "Exam_year";
            ddlYear1.DataValueField = "Exam_year";
            ddlYear1.DataBind();
        }
        ddlYear1.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
    }

    protected void month1()
    {
        try
        {
            dsss.Clear();
            string year1 = ddlYear1.SelectedValue;
            dsss = da.Exammonth(year1);
            if (dsss.Tables[0].Rows.Count > 0)
            {
                ddlMonth1.DataSource = dsss;
                ddlMonth1.DataTextField = "monthName";
                ddlMonth1.DataValueField = "Exam_month";
                ddlMonth1.DataBind();
            }
            ddlMonth1.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    public void degree()
    {
        try
        {
            ddldegree1.Items.Clear();
            string usercode = Session["usercode"].ToString();
            string collegecode = Session["collegecode"].ToString();
            string singleuser = Session["single_user"].ToString();
            string group_user = Session["group_code"].ToString();
            string type = string.Empty;
            if (ddltype.Enabled == true)
            {
                if (ddltype.Items.Count > 0)
                {
                    if (ddltype.SelectedItem.ToString() != "All" && ddltype.SelectedItem.ToString() != "")
                    {
                        type = " and course.type='" + ddltype.SelectedItem.ToString() + "'";
                    }
                }
            }
            string codevalues = string.Empty;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                codevalues = "and group_code='" + group_user + "'";
            }
            else
            {
                codevalues = "and user_code='" + usercode + "'";
            }
            string strquery = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where  course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + collegecode + "' and deptprivilages.Degree_code=degree.Degree_code " + codevalues + " " + type + " ";
            ds2 = da.select_method_wo_parameter(strquery, "Text");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddldegree1.DataSource = ds2;
                ddldegree1.DataTextField = "course_name";
                ddldegree1.DataValueField = "course_id";
                ddldegree1.DataBind();
            }
            ddldegree1.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    public void bindbranch1()
    {
        try
        {
            ddlbranch1.Items.Clear();
            hat.Clear();
            string usercode = Session["usercode"].ToString();
            string collegecode = Session["collegecode"].ToString();
            string singleuser = Session["single_user"].ToString();
            string group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Add("single_user", singleuser.ToString());
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddldegree1.SelectedValue);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            DataSet ds = da.select_method("bind_branch", hat, "sp");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlbranch1.DataSource = ds;
                ddlbranch1.DataTextField = "dept_name";
                ddlbranch1.DataValueField = "degree_code";
                ddlbranch1.DataBind();
            }
            ddlbranch1.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    public void bindsem1()
    {
        try
        {
            ddlsem1.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            hat.Clear();
            string usercode = Session["usercode"].ToString();
            string collegecode = Session["collegecode"].ToString();
            string singleuser = Session["single_user"].ToString();
            string group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            DataSet ds = new DataSet();
            if (chksubwise.Checked == true)
            {
                string strsql = "select Max(ndurations),first_year_nonsemester from ndegree where college_code=" + collegecode + " group by first_year_nonsemester order by Max(ndurations) ";
                ds = da.select_method_wo_parameter(strsql, "TExt");
            }
            else
            {
                ds = da.BindSem(ddlbranch1.SelectedValue.ToString(), ddlYear1.SelectedValue.ToString(), collegecode);
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]).ToString());
                duration = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0][0]).ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsem1.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsem1.Items.Add(i.ToString());
                    }
                }
            }
            ddlsem1.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    public void loadtype()
    {
        try
        {
            string collegecode = Session["collegecode"].ToString();
            ddltype.Items.Clear();
            string strquery = "select distinct type from course where college_code='" + collegecode + "' and type is not null and type<>''";
            DataSet dstype = da.select_method_wo_parameter(strquery, "Text");
            if (dstype.Tables[0].Rows.Count > 0)
            {
                ddltype.DataSource = dstype;
                ddltype.DataTextField = "type";
                ddltype.DataBind();
                ddltype.Enabled = true;
                ddltype.Items.Insert(0, "");
                ddltype.Items.Insert(1, "All");
            }
            else
            {
                degree();
                ddltype.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void subjectbind()
    {
        try
        {
            ddlSubject.Items.Clear();
            dsss.Clear();
            string branc = ddlbranch1.SelectedValue.ToString();
            string semmv = ddlsem1.SelectedValue.ToString();
            string typeval = string.Empty;
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (ddltype.Text != "")
                {
                    typeval = " and C.Type='" + ddltype.Text.ToString() + "'";
                }
            }
            string qeryss = "SELECT distinct ss.subject_type,s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode FROM subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and sy.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and ss.promote_count=1 and sy.semester='" + semmv + "' " + typeval + " and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "' order by s.subject_name,s.subject_code desc";
            if (chksubwise.Checked == false)
            {
                qeryss = "SELECT distinct ss.subject_type,s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode FROM subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and sy.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and ss.promote_count=1 and sy.semester='" + semmv + "' " + typeval + " and sy.degree_code='" + branc + "' and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "' order by s.subject_name,s.subject_code desc";
            }
            dsss = da.select_method(qeryss, hat, "Text");
            if (dsss.Tables[0].Rows.Count > 0)
            {
                ddlSubject.DataSource = dsss;
                ddlSubject.DataTextField = "subnamecode";
                ddlSubject.DataValueField = "subject_code";
                ddlSubject.DataBind();
            }
            ddlSubject.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void subjecttypebind()
    {
        try
        {
            ddlsubtype.Items.Clear();
            dsss.Clear();
            string branc = ddlbranch1.SelectedValue.ToString();
            string semmv = ddlsem1.SelectedValue.ToString();
            string typeval = string.Empty;
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (ddltype.Text != "")
                {
                    typeval = " and C.Type='" + ddltype.Text.ToString() + "'";
                }
            }
            string qeryss = "SELECT distinct ss.subject_type FROM subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and sy.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and ss.promote_count=1 and sy.semester='" + semmv + "' " + typeval + " order by ss.subject_type";
            if (chksubwise.Checked == false)
            {
                qeryss = "SELECT distinct ss.subject_type FROM subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and sy.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and ss.promote_count=1 and sy.semester='" + semmv + "' and sy.degree_code='" + branc + "' " + typeval + " order by ss.subject_type";
            }
            dsss = da.select_method(qeryss, hat, "Text");
            if (dsss.Tables[0].Rows.Count > 0)
            {
                ddlsubtype.DataSource = dsss;
                ddlsubtype.DataTextField = "subject_type";
                ddlsubtype.DataBind();
            }
            ddlsubtype.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlbranch1.Items.Clear();
        ddlsubtype.Items.Clear();
        ddlSubject.Items.Clear();
        clear();
        degree();
        bindbranch1();
        bindsem1();
    }

    protected void ddlsem1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            ddlSubject.Items.Clear();
            subjecttypebind();
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void ddldegree1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            bindbranch1();
            bindsem1();
            ddlsubtype.Items.Clear();
            ddlSubject.Items.Clear();
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void ddlbranch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            bindsem1();
            ddlsubtype.Items.Clear();
            ddlSubject.Items.Clear();
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void ddlMonth1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            ddlsubtype.Items.Clear();
            ddlSubject.Items.Clear();
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void ddlYear1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            month1();
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void ddlsubtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            subjectbind();
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void ddlicatype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void chksubwise_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        ddldegree1.Enabled = true;
        ddlbranch1.Enabled = true;
        if (chksubwise.Checked == true)
        {
            ddldegree1.Enabled = false;
            ddlbranch1.Enabled = false;
        }
        bindsem1();
        subjecttypebind();
        subjectbind();
    }

    public void clear()
    {
        lblerr1.Visible = false;
        btnsave1.Visible = false;
        fpspread.Visible = false;
    }

    protected void btnviewre_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (ddlYear1.SelectedIndex == 0)
            {
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Year";
                return;
            }
            if (ddlMonth1.SelectedIndex == 0)
            {
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Month";
                return;
            }
            if (ddltype.SelectedIndex == 0)
            {
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Type";
                return;
            }
            if (ddldegree1.SelectedIndex == 0 && chksubwise.Checked == false)
            {
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Degree";
                return;
            }
            if (ddlbranch1.SelectedIndex == 0 && chksubwise.Checked == false)
            {
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select branch";
                return;
            }
            if (ddlsem1.SelectedIndex == 0)
            {
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Semester";
                return;
            }
            if (ddlSubject.SelectedIndex == 0)
            {
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Subject";
                return;
            }
            fpspread.Width = 906;
            fpspread.Visible = false;
            fpspread.Sheets[0].RowCount = 0;
            fpspread.Sheets[0].ColumnCount = 0;
            fpspread.Sheets[0].ColumnCount = 5;
            fpspread.Sheets[0].RowHeader.Visible = false;
            fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            fpspread.Sheets[0].DefaultStyle.Font.Bold = false;
            fpspread.Sheets[0].ColumnHeader.RowCount = 1;
            fpspread.Sheets[0].AutoPostBack = false;
            fpspread.CommandBar.Visible = false;
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 50;
            fpspread.Sheets[0].ColumnHeader.Columns[1].Width = 100;
            fpspread.Sheets[0].ColumnHeader.Columns[2].Width = 100;
            fpspread.Sheets[0].ColumnHeader.Columns[3].Width = 250;
            fpspread.Sheets[0].ColumnHeader.Columns[4].Width = 80;
            fpspread.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].ColumnHeader.Columns[1].HorizontalAlign = HorizontalAlign.Left;
            fpspread.Sheets[0].ColumnHeader.Columns[2].HorizontalAlign = HorizontalAlign.Left;
            fpspread.Sheets[0].ColumnHeader.Columns[3].HorizontalAlign = HorizontalAlign.Left;
            fpspread.Sheets[0].ColumnHeader.Columns[4].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[0].Locked = true;
            fpspread.Sheets[0].Columns[1].Locked = true;
            fpspread.Sheets[0].Columns[2].Locked = true;
            fpspread.Sheets[0].Columns[3].Locked = true;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Select";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = System.Drawing.Color.White;
            style2.BackColor = System.Drawing.Color.Teal;
            fpspread.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            fpspread.Sheets[0].SheetName = " ";
            fpspread.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].DefaultStyle.Font.Bold = false;
            fpspread.Sheets[0].Columns[1].Visible = false;
            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
            chkall.AutoPostBack = true;
            FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
            string repav = " and r.cc=1";
            if (ddlicatype.SelectedValue.ToString() == "1")
            {
                repav = " and r.cc=0";
            }
            //string strquery1 = "select m.roll_no,m.internal_mark,m.external_mark,m.result from Exam_Details ed,mark_entry m,subject s where m.exam_code=ed.exam_code and m.subject_no=s.subject_no and s.subject_code='"+ddlSubject.SelectedValue.ToString()+"' and ed.Exam_Month=11 and ed.Exam_year=2015";
            //DataSet dsmarkentry = da.select_method_wo_parameter(strquery1, "Text");
            string strbaks = "select ea.roll_no,ea.appl_no,ed.current_semester from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s where ea.exam_code=ed.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ead.type='1' and s.subject_code='" + ddlSubject.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedValue.ToString() + "'";
            DataSet dsexamapp = da.select_method_wo_parameter(strbaks, "Text");
            string degreeval = string.Empty;
            if (chksubwise.Checked == false)
            {
                degreeval = " and r.degree_code='" + ddlbranch1.SelectedValue.ToString() + "'";
            }
            string strquery = "select s.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,r.Batch_Year,r.degree_code,r.current_semester,r.cc,r.delflag,max(m.internal_mark) ICA,max(m.external_mark) EXE from mark_entry m,subject s,Registration r where m.subject_no=s.subject_no and r.Roll_No=m.roll_no and s.subject_code='" + ddlSubject.SelectedValue.ToString() + "' " + degreeval + " " + repav + " and m.roll_no not in (select m1.roll_no from mark_entry m1 where m1.roll_no=m.roll_no and m.subject_no=m1.subject_no and m1.result='Pass') group by s.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,r.Batch_Year,r.degree_code,r.current_semester,r.cc,r.delflag";
            strquery = strquery + " union select s.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,r.Batch_Year,r.degree_code,r.current_semester,r.cc,r.delflag,max(m.internal_mark) ICA,max(m.external_mark) EXE from mark_entry m,subject s,Registration r,Exam_Details ed,exam_application ea,exam_appl_details ead where m.subject_no=s.subject_no and r.Roll_No=m.roll_no and s.subject_code='" + ddlSubject.SelectedValue.ToString() + "' " + degreeval + " " + repav + " and r.cc=1 and r.Roll_No=ea.roll_no and ed.exam_code=m.exam_code and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.type='1' and m.result='Pass' group by s.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,r.Batch_Year,r.degree_code,r.current_semester,r.cc,r.delflag  order by r.batch_year desc,r.degree_code,r.reg_no desc";
            ds2.Dispose();
            ds2.Reset();
            ds2 = da.select_method_wo_parameter(strquery, "Text");
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                btnsave1.Visible = true;
                fpspread.Visible = true;
                int srno = 0;
                fpspread.Sheets[0].RowCount++;
                fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, 4);
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].CellType = chkall;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                {
                    string rollno = ds2.Tables[0].Rows[i]["Roll_No"].ToString();
                    string regno = ds2.Tables[0].Rows[i]["Reg_No"].ToString();
                    string name = ds2.Tables[0].Rows[i]["Stud_Name"].ToString();
                    string subno = ds2.Tables[0].Rows[i]["subject_no"].ToString();
                    srno++;
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].CellType = txt;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].CellType = txt;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].CellType = txt;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = rollno;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = regno;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Text = name;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Tag = ds2.Tables[0].Rows[i]["batch_year"].ToString();
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Tag = ds2.Tables[0].Rows[i]["degree_code"].ToString();
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Tag = ds2.Tables[0].Rows[i]["current_semester"].ToString();
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Tag = ds2.Tables[0].Rows[i]["subject_no"].ToString();
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].CellType = chk;
                    dsexamapp.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "'";
                    DataView dvexmapp = dsexamapp.Tables[0].DefaultView;
                    if (dvexmapp.Count > 0)
                    {
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Value = 1;
                    }
                    else
                    {
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Value = 0;
                    }
                }
            }
            else
            {
                lblerr1.Visible = true;
                lblerr1.Text = "No Records Found";
            }
            fpspread.SaveChanges();
            fpspread.Width = 600;
            fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
            int hei = 300;
            for (int col = 0; col < fpspread.Sheets[0].RowCount; col++)
            {
                hei = hei + fpspread.Sheets[0].Rows[col].Height;
            }
            fpspread.Height = hei;
            fpspread.SaveChanges();
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void fpspread_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string ctrlname = Page.Request.Params["__EVENTTARGET"];
            if (ctrlname != null && ctrlname != String.Empty)
            {
                string[] spiltspreadname = ctrlname.Split('$');
                if (spiltspreadname.GetUpperBound(0) > 1)
                {
                    string getrowxol = spiltspreadname[3].ToString().Trim();
                    string[] spr = getrowxol.Split(',');
                    if (spr.GetUpperBound(0) == 1)
                    {
                        int arow = Convert.ToInt32(spr[0]);
                        int acol = Convert.ToInt32(spr[1]);
                        if (arow == 0 && acol == 4)
                        {
                            string setval = e.EditValues[acol].ToString();
                            int setvalcel = 0;
                            if (setval.Trim().ToLower() == "true" || setval.Trim() == "1")
                            {
                                setvalcel = 1;
                            }
                            for (int r = 1; r < fpspread.Sheets[0].RowCount; r++)
                            {
                                fpspread.Sheets[0].Cells[r, acol].Value = setvalcel;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void btnsavel1_click(object sender, EventArgs e)
    {
        try
        {
            int insert = 0;
            fpspread.SaveChanges();
            Boolean selflag = false;
            string strsaveexamdetailsquery = string.Empty;
            string strquery = "select m.roll_no,s.subject_no,ed.current_semester,ed.batch_year,ed.degree_code,m.internal_mark,m.external_mark,m.total,m.result from mark_entry m,Exam_Details ed,subject s where m.exam_code=ed.exam_code and m.subject_no=s.subject_no and ed.Exam_year='" + ddlYear1.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and s.subject_code='" + Convert.ToString(ddlSubject.SelectedValue).Trim() + "'";// and s.subject_code='121LA1F01'
            DataSet dsexmark = da.select_method_wo_parameter(strquery, "Text");
            Dictionary<string, string> dicappl = new Dictionary<string, string>();
            strquery = "select ea.roll_no,ea.appl_no,ed.current_semester,ed.batch_year,ed.degree_code from Exam_Details ed,exam_application ea,exam_appl_details ead  where ea.exam_code=ed.exam_code and ea.appl_no=ead.appl_no  and ed.Exam_year='" + ddlYear1.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "'";
            DataSet dsappl = da.select_method_wo_parameter(strquery, "Text");
            if (dsappl.Tables.Count > 0 && dsappl.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsappl.Tables[0].Rows.Count; i++)
                {
                    string rollno = dsappl.Tables[0].Rows[i]["roll_no"].ToString().Trim().ToLower();
                    string eaplno = dsappl.Tables[0].Rows[i]["appl_no"].ToString();
                    string eacursem = dsappl.Tables[0].Rows[i]["current_semester"].ToString();
                    if (!dicappl.ContainsKey(rollno))
                    {
                        dicappl.Add(rollno, eaplno);
                    }
                }
            }
            for (int r = 1; r < fpspread.Sheets[0].RowCount; r++)
            {
                int issel = Convert.ToInt32(fpspread.Sheets[0].Cells[r, 4].Value);
                if (issel == 1)
                {
                    selflag = true;
                }
            }
            if (selflag == false)
            {
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select The Student And Then Proceed";
                return;
            }
            bool IsSaved = false;
            for (int r = 1; r < fpspread.Sheets[0].RowCount; r++)
            {
                int issel = Convert.ToInt32(fpspread.Sheets[0].Cells[r, 4].Value);
                string batchyear = fpspread.Sheets[0].Cells[r, 1].Tag.ToString();
                string degree = fpspread.Sheets[0].Cells[r, 2].Tag.ToString();
                string rollno = fpspread.Sheets[0].Cells[r, 1].Text.ToString();
                string feecatsem = fpspread.Sheets[0].Cells[r, 3].Tag.ToString();
                string subjcetno = fpspread.Sheets[0].Cells[r, 4].Tag.ToString();
                string applno = string.Empty;
                if (dicappl.ContainsKey(rollno.Trim().ToLower()))
                {
                    applno = dicappl[rollno.Trim().ToLower()];
                }
                else
                {
                    applno = da.GetFunction("select ea.appl_no from Exam_Details ed,exam_application ea where ed.exam_code=ea.exam_code and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedValue.ToString() + "' and ea.roll_no='" + rollno + "'");
                    if (applno.Trim() == "0" || applno.Trim() == "")
                    {
                        dsappl.Tables[0].DefaultView.RowFilter = "batch_year='" + batchyear + "' and degree_code='" + degree + "'";
                        DataView dvmark = dsappl.Tables[0].DefaultView;
                        if (dvmark.Count > 0)
                        {
                            feecatsem = dvmark[0]["current_semester"].ToString();
                        }
                        //string strexamdetails = "if not exists (select * from Exam_Details where batch_year='" + batchyear + "' and degree_code='" + degree + "' and current_semester='" + feecatsem + "' and Exam_year='" + ddlYear1.SelectedValue.ToString() + "' and Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "')";
                        string strexamdetails = "if not exists (select * from Exam_Details where batch_year='" + batchyear + "' and degree_code='" + degree + "'  and Exam_year='" + ddlYear1.SelectedValue.ToString() + "' and Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "')";//and current_semester='" + feecatsem + "'
                        strexamdetails = strexamdetails + " insert into Exam_Details(batch_year,degree_code,current_semester,Exam_year,Exam_Month)";
                        strexamdetails = strexamdetails + " values('" + batchyear + "','" + degree + "','" + feecatsem + "','" + ddlYear1.SelectedValue.ToString() + "','" + ddlMonth1.SelectedValue.ToString() + "')";
                        insert = da.update_method_wo_parameter(strexamdetails, "text");
                        //string exam_code = da.GetFunction("select exam_code from Exam_Details where batch_year='" + batchyear + "' and degree_code='" + degree + "' and current_semester='" + feecatsem + "' and Exam_year='" + ddlYear1.SelectedValue.ToString() + "' and Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "'");
                        string exam_code = da.GetFunction("select exam_code from Exam_Details where batch_year='" + batchyear + "' and degree_code='" + degree + "' and Exam_year='" + ddlYear1.SelectedValue.ToString() + "' and Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "'");
                        string examapplquery = "if not exists (select * from exam_application where roll_no='" + rollno + "' and exam_code='" + exam_code + "')";
                        examapplquery = examapplquery + " insert into exam_application (roll_no,exam_code) values('" + rollno + "','" + exam_code + "')";
                        int saveexamapp = da.update_method_wo_parameter(examapplquery, "Text");
                        applno = da.GetFunction("select appl_no from exam_application where roll_no='" + rollno + "' and exam_code='" + exam_code + "'");
                    }
                }
                if (issel == 1)
                {
                    selflag = true;
                    strsaveexamdetailsquery = "if not exists (select * from exam_appl_details where appl_no='" + applno + "' and subject_no='" + subjcetno + "')";
                    strsaveexamdetailsquery = strsaveexamdetailsquery + " insert into exam_appl_details (appl_no,subject_no,type) values('" + applno + "','" + subjcetno + "','1')";
                    strsaveexamdetailsquery = strsaveexamdetailsquery + " else update exam_appl_details set type=1  where appl_no='" + applno + "' and subject_no='" + subjcetno + "'";
                }
                else
                {
                    dsexmark.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "'";
                    DataView dvmar = dsexmark.Tables[0].DefaultView;
                    if (dvmar.Count > 0)
                    {
                        strsaveexamdetailsquery = "if exists (select * from exam_appl_details where appl_no='" + applno + "' and subject_no='" + subjcetno + "') update exam_appl_details set type=0 where appl_no='" + applno + "' and subject_no='" + subjcetno + "'";
                    }
                    else
                    {
                        strsaveexamdetailsquery = "if not exists (select * from exam_appl_details where appl_no='" + applno + "' and subject_no='" + subjcetno + "')delete from exam_appl_details where appl_no='" + applno + "' and subject_no='" + subjcetno + "'";
                    }
                }
                insert = da.update_method_wo_parameter(strsaveexamdetailsquery, "Text");
                if (insert > 0)
                {
                    IsSaved = true;
                }
            }
            if (selflag == false)
            {
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select The Student And Then Proceed";
            }
            else
            {
                if (IsSaved)
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                else
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not Saved')", true);
            }
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

}